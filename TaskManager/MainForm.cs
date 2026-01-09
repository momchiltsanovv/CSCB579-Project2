/*
 * Програма: Task Manager Application
 * Студент: Momchil Georgiev Tsanov
 * Факултетен номер: 113172
 * 
 * Главна форма на приложението
 * Демонстрира използването на Windows Forms, Graphics, Collections, Interfaces, Delegates
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using TaskManager.Delegates;
using TaskManager.Filters;
using TaskManager.Interfaces;
using TaskManager.Models;
using TaskManager.Services;
using TaskManager.Sorters;
using Task = TaskManager.Models.Task;
using TaskStatus = TaskManager.Models.TaskStatus;

namespace TaskManager
{
    /// <summary>
    /// Главна форма на приложението
    /// </summary>
    public partial class MainForm : Form
    {
        // Сервизи
        private TaskManagerService _taskManager;
        private SerializationService _serializationService;
        private LanguageService _languageService;

        // UI Компоненти
        private ListView _taskListView;
        private Panel _inputPanel;
        private TextBox _titleTextBox;
        private TextBox _descriptionTextBox;
        private ComboBox _priorityComboBox;
        private ComboBox _statusComboBox;
        private DateTimePicker _dueDatePicker;
        private CheckBox _hasDueDateCheckBox;
        private TextBox _categoryTextBox;
        private Button _addButton;
        private Button _updateButton;
        private Button _deleteButton;
        private Button _clearButton;

        // Филтри и сортиране
        private ComboBox _filterComboBox;
        private ComboBox _sortComboBox;
        private Label _filterLabel;
        private Label _sortLabel;

        // Графика за статистика
        private Panel _statisticsPanel;
        private PictureBox _statisticsChart;

        // Менюта
        private MenuStrip _menuStrip;
        private ToolStripMenuItem _fileMenu;
        private ToolStripMenuItem _editMenu;
        private ToolStripMenuItem _viewMenu;
        private ToolStripMenuItem _languageMenu;
        private ToolStripMenuItem _helpMenu;

        // Статус бар
        private StatusStrip _statusStrip;
        private ToolStripStatusLabel _statusLabel;

        // Избрана задача
        private Task _selectedTask = null;

        /// <summary>
        /// Конструктор
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            InitializeServices();
            InitializeMenu();
            InitializeUI();
            InitializeStatusBar();
            InitializeEventHandlers();
            RefreshTaskList();
            UpdateStatistics();
        }

        /// <summary>
        /// Инициализира компонентите
        /// </summary>
        private void InitializeComponent()
        {
            this.Text = "Task Manager - Управление на Задачи";
            this.Size = new Size(1600, 1000);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.MinimumSize = new Size(1400, 800);
        }

        /// <summary>
        /// Инициализира сервизите
        /// </summary>
        private void InitializeServices()
        {
            _taskManager = new TaskManagerService();
            _serializationService = new SerializationService();
            _languageService = new LanguageService();
            _languageService.LanguageChanged += LanguageService_LanguageChanged;

            // Регистриране на събития (Delegates)
            _taskManager.TaskAdded += TaskManager_TaskAdded;
            _taskManager.TaskRemoved += TaskManager_TaskRemoved;
            _taskManager.TaskUpdated += TaskManager_TaskUpdated;
            _taskManager.TaskStatusChanged += TaskManager_TaskStatusChanged;
        }

        /// <summary>
        /// Инициализира менютата
        /// </summary>
        private void InitializeMenu()
        {
            _menuStrip = new MenuStrip();

            _fileMenu = new ToolStripMenuItem(_languageService.GetString("MenuFile"));
            _fileMenu.DropDownItems.Add(_languageService.GetString("MenuItemNew"), null, (s, e) => ClearForm());
            _fileMenu.DropDownItems.Add(_languageService.GetString("MenuItemOpen"), null, MenuItemOpen_Click);
            _fileMenu.DropDownItems.Add(_languageService.GetString("MenuItemSave"), null, MenuItemSave_Click);
            _fileMenu.DropDownItems.Add(new ToolStripSeparator());
            _fileMenu.DropDownItems.Add(_languageService.GetString("MenuItemExit"), null, (s, e) => Application.Exit());

            _editMenu = new ToolStripMenuItem(_languageService.GetString("MenuEdit"));
            _editMenu.DropDownItems.Add("Изчисти всичко", null, (s, e) => 
            {
                if (MessageBox.Show("Сигурни ли сте?", "Потвърждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    _taskManager.ClearAllTasks();
                    RefreshTaskList();
                    UpdateStatistics();
                }
            });

            _viewMenu = new ToolStripMenuItem(_languageService.GetString("MenuView"));
            _viewMenu.DropDownItems.Add("Покажи статистика", null, (s, e) => UpdateStatistics());

            _languageMenu = new ToolStripMenuItem(_languageService.GetString("MenuLanguage"));
            _languageMenu.DropDownItems.Add(_languageService.GetString("MenuItemBulgarian"), null, 
                (s, e) => _languageService.CurrentLanguage = Language.Bulgarian);
            _languageMenu.DropDownItems.Add(_languageService.GetString("MenuItemEnglish"), null, 
                (s, e) => _languageService.CurrentLanguage = Language.English);

            _helpMenu = new ToolStripMenuItem(_languageService.GetString("MenuHelp"));
            _helpMenu.DropDownItems.Add(_languageService.GetString("MenuItemAbout"), null, MenuItemAbout_Click);

            _menuStrip.Items.AddRange(new ToolStripItem[] { _fileMenu, _editMenu, _viewMenu, _languageMenu, _helpMenu });
            this.MainMenuStrip = _menuStrip;
            this.Controls.Add(_menuStrip);
        }

        /// <summary>
        /// Инициализира UI компонентите
        /// </summary>
        private void InitializeUI()
        {
            // SplitContainer за разделяне на интерфейса
            SplitContainer mainSplit = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                SplitterDistance = 800
            };

            // Лява страна - задачи и форма
            SplitContainer leftSplit = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 500, // Увеличен размер за по-голяма видима област за задачи
                SplitterWidth = 5
            };

            // ListView за задачи
            _taskListView = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                Font = new Font("Segoe UI", 9)
            };
            _taskListView.Columns.Add("ID", 50);
            _taskListView.Columns.Add(_languageService.GetString("TitleLabel"), 200);
            _taskListView.Columns.Add(_languageService.GetString("PriorityLabel"), 100);
            _taskListView.Columns.Add(_languageService.GetString("StatusLabel"), 100);
            _taskListView.Columns.Add(_languageService.GetString("DueDateLabel"), 150);
            _taskListView.Columns.Add(_languageService.GetString("CategoryLabel"), 100);
            _taskListView.SelectedIndexChanged += TaskListView_SelectedIndexChanged;
            _taskListView.DoubleClick += TaskListView_DoubleClick;

            // Панел за форма за въвеждане със скролбар
            Panel inputContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(250, 250, 250)
            };

            _inputPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(250, 250, 250),
                Padding = new Padding(10),
                AutoScroll = true // Добавяне на автоматичен скролбар
            };
            inputContainer.Controls.Add(_inputPanel);

            int yPos = 10;
            int labelWidth = 120;
            int controlWidth = 400; // Увеличена ширина на контролите
            int spacing = 40; // Увеличен разстояние между полетата

            // Заглавие
            Label titleLabel = new Label { Text = _languageService.GetString("TitleLabel"), Location = new Point(10, yPos), Width = labelWidth, Font = new Font("Segoe UI", 9) };
            _titleTextBox = new TextBox { Location = new Point(140, yPos), Width = controlWidth, Font = new Font("Segoe UI", 9) };
            _inputPanel.Controls.AddRange(new Control[] { titleLabel, _titleTextBox });
            yPos += spacing;

            // Описание
            Label descLabel = new Label { Text = _languageService.GetString("DescriptionLabel"), Location = new Point(10, yPos), Width = labelWidth, Font = new Font("Segoe UI", 9) };
            _descriptionTextBox = new TextBox { Location = new Point(140, yPos), Width = controlWidth, Height = 80, Multiline = true, Font = new Font("Segoe UI", 9), ScrollBars = ScrollBars.Vertical };
            _inputPanel.Controls.AddRange(new Control[] { descLabel, _descriptionTextBox });
            yPos += 90;

            // Приоритет
            Label priorityLabel = new Label { Text = _languageService.GetString("PriorityLabel"), Location = new Point(10, yPos), Width = labelWidth, Font = new Font("Segoe UI", 9) };
            _priorityComboBox = new ComboBox { Location = new Point(140, yPos), Width = controlWidth, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9) };
            _priorityComboBox.Items.AddRange(new string[] { "Low", "Medium", "High", "Critical" });
            _priorityComboBox.SelectedIndex = 1;
            _inputPanel.Controls.AddRange(new Control[] { priorityLabel, _priorityComboBox });
            yPos += spacing;

            // Статус
            Label statusLabel = new Label { Text = _languageService.GetString("StatusLabel"), Location = new Point(10, yPos), Width = labelWidth, Font = new Font("Segoe UI", 9) };
            _statusComboBox = new ComboBox { Location = new Point(140, yPos), Width = controlWidth, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9) };
            _statusComboBox.Items.AddRange(new string[] { "Pending", "InProgress", "Completed", "Cancelled" });
            _statusComboBox.SelectedIndex = 0;
            _inputPanel.Controls.AddRange(new Control[] { statusLabel, _statusComboBox });
            yPos += spacing;

            // Крайна дата
            Label dueDateLabel = new Label { Text = _languageService.GetString("DueDateLabel"), Location = new Point(10, yPos), Width = labelWidth, Font = new Font("Segoe UI", 9) };
            _hasDueDateCheckBox = new CheckBox { Text = "Има крайна дата", Location = new Point(140, yPos), Checked = false, Font = new Font("Segoe UI", 9) };
            _dueDatePicker = new DateTimePicker { Location = new Point(140, yPos + 30), Width = controlWidth, Enabled = false, Font = new Font("Segoe UI", 9) };
            _hasDueDateCheckBox.CheckedChanged += (s, e) => _dueDatePicker.Enabled = _hasDueDateCheckBox.Checked;
            _inputPanel.Controls.AddRange(new Control[] { dueDateLabel, _hasDueDateCheckBox, _dueDatePicker });
            yPos += 70;

            // Категория
            Label categoryLabel = new Label { Text = _languageService.GetString("CategoryLabel"), Location = new Point(10, yPos), Width = labelWidth, Font = new Font("Segoe UI", 9) };
            _categoryTextBox = new TextBox { Location = new Point(140, yPos), Width = controlWidth, Text = "General", Font = new Font("Segoe UI", 9) };
            _inputPanel.Controls.AddRange(new Control[] { categoryLabel, _categoryTextBox });
            yPos += spacing + 20;

            // Бутони
            _addButton = new Button { Text = _languageService.GetString("AddButton"), Location = new Point(140, yPos), Width = 110, Height = 40, Font = new Font("Segoe UI", 9, FontStyle.Bold), BackColor = Color.FromArgb(40, 167, 69), ForeColor = Color.White };
            _addButton.Click += AddButton_Click;
            _updateButton = new Button { Text = _languageService.GetString("UpdateButton"), Location = new Point(260, yPos), Width = 110, Height = 40, Enabled = false, Font = new Font("Segoe UI", 9, FontStyle.Bold), BackColor = Color.FromArgb(0, 123, 255), ForeColor = Color.White };
            _updateButton.Click += UpdateButton_Click;
            _deleteButton = new Button { Text = _languageService.GetString("DeleteButton"), Location = new Point(380, yPos), Width = 110, Height = 40, Enabled = false, BackColor = Color.FromArgb(220, 53, 69), ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            _deleteButton.Click += DeleteButton_Click;
            _clearButton = new Button { Text = "Изчисти", Location = new Point(500, yPos), Width = 110, Height = 40, Font = new Font("Segoe UI", 9) };
            _clearButton.Click += (s, e) => ClearForm();
            _inputPanel.Controls.AddRange(new Control[] { _addButton, _updateButton, _deleteButton, _clearButton });

            // Филтри и сортиране
            yPos += 50;
            _filterLabel = new Label { Text = "Филтър:", Location = new Point(10, yPos), Width = labelWidth, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            _filterComboBox = new ComboBox { Location = new Point(140, yPos), Width = controlWidth, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9) };
            _filterComboBox.Items.AddRange(new string[] { "Всички", "Pending", "InProgress", "Completed", "Cancelled", "Overdue" });
            _filterComboBox.SelectedIndex = 0;
            _filterComboBox.SelectedIndexChanged += FilterComboBox_SelectedIndexChanged;
            yPos += spacing;
            _sortLabel = new Label { Text = "Сортиране:", Location = new Point(10, yPos), Width = labelWidth, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            _sortComboBox = new ComboBox { Location = new Point(140, yPos), Width = controlWidth, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9) };
            _sortComboBox.Items.AddRange(new string[] { "По дата (ново)", "По дата (старо)", "По приоритет" });
            _sortComboBox.SelectedIndex = 0;
            _sortComboBox.SelectedIndexChanged += SortComboBox_SelectedIndexChanged;
            _inputPanel.Controls.AddRange(new Control[] { _filterLabel, _filterComboBox, _sortLabel, _sortComboBox });

            // Задаване на минимална височина на панела за въвеждане
            _inputPanel.MinimumSize = new Size(0, yPos + spacing + 50);

            leftSplit.Panel1.Controls.Add(_taskListView);
            leftSplit.Panel2.Controls.Add(inputContainer);

            // Дясна страна - статистика
            _statisticsPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(10)
            };

            Label statsTitle = new Label
            {
                Text = _languageService.GetString("StatisticsLabel"),
                Dock = DockStyle.Top,
                Height = 30,
                Font = new Font("Segoe UI", 14, FontStyle.Bold)
            };

            _statisticsChart = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };
            _statisticsChart.Paint += StatisticsChart_Paint;

            _statisticsPanel.Controls.Add(_statisticsChart);
            _statisticsPanel.Controls.Add(statsTitle);

            mainSplit.Panel1.Controls.Add(leftSplit);
            mainSplit.Panel2.Controls.Add(_statisticsPanel);

            this.Controls.Add(mainSplit);
        }

        /// <summary>
        /// Инициализира статус бара
        /// </summary>
        private void InitializeStatusBar()
        {
            _statusStrip = new StatusStrip();
            _statusLabel = new ToolStripStatusLabel(_languageService.GetString("StatusReady"));
            _statusStrip.Items.Add(_statusLabel);
            this.Controls.Add(_statusStrip);
        }

        /// <summary>
        /// Инициализира обработчиците на събития
        /// </summary>
        private void InitializeEventHandlers()
        {
            // Обработчици вече са добавени в InitializeUI
        }

        /// <summary>
        /// Обновява списъка с задачи
        /// </summary>
        private void RefreshTaskList()
        {
            if (_taskListView == null) return;

            _taskListView.Items.Clear();

            List<Task> tasks = _taskManager.GetAllTasks();

            // Прилагане на филтър (използване на Interface)
            if (_filterComboBox != null && _filterComboBox.SelectedIndex > 0)
            {
                string filterText = _filterComboBox.SelectedItem.ToString();
                if (filterText == "Overdue")
                {
                    tasks = _taskManager.GetOverdueTasks();
                }
                else if (Enum.TryParse<TaskStatus>(filterText, out TaskStatus status))
                {
                    ITaskFilter filter = new StatusFilter(status);
                    tasks = _taskManager.FilterTasks(filter);
                }
            }

            // Прилагане на сортиране (използване на Interface)
            if (_sortComboBox != null && _sortComboBox.SelectedIndex == 0)
            {
                ITaskSorter sorter = new DateSorter(false);
                tasks = _taskManager.SortTasks(sorter);
            }
            else if (_sortComboBox != null && _sortComboBox.SelectedIndex == 1)
            {
                ITaskSorter sorter = new DateSorter(true);
                tasks = _taskManager.SortTasks(sorter);
            }
            else if (_sortComboBox != null && _sortComboBox.SelectedIndex == 2)
            {
                ITaskSorter sorter = new PrioritySorter();
                tasks = _taskManager.SortTasks(sorter);
            }

            // Добавяне на задачи в ListView
            foreach (Task task in tasks)
            {
                ListViewItem item = new ListViewItem(task.Id.ToString());
                item.SubItems.Add(task.Title);
                item.SubItems.Add(task.Priority.ToString());
                item.SubItems.Add(task.Status.ToString());
                item.SubItems.Add(task.DueDate?.ToString("dd.MM.yyyy") ?? "N/A");
                item.SubItems.Add(task.Category);
                item.Tag = task;
                item.ForeColor = task.IsOverdue() ? Color.Red : Color.Black;
                _taskListView.Items.Add(item);
            }

            if (_statusLabel != null)
            {
                _statusLabel.Text = $"Общо задачи: {_taskManager.GetAllTasks().Count}";
            }
        }

        /// <summary>
        /// Обновява статистиката и рисува графика
        /// </summary>
        private void UpdateStatistics()
        {
            _statisticsChart.Invalidate();
        }

        /// <summary>
        /// Рисува статистиката върху графиката (Graphics)
        /// </summary>
        private void StatisticsChart_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.White);

            var statusStats = _taskManager.GetStatusStatistics();
            var priorityStats = _taskManager.GetPriorityStatistics();

            int width = _statisticsChart.Width;
            int height = _statisticsChart.Height;
            int margin = 40;
            int chartWidth = width - 2 * margin;
            int chartHeight = height - 2 * margin;

            // Заглавие
            using (Font titleFont = new Font("Segoe UI", 12, FontStyle.Bold))
            {
                g.DrawString("Статистика по статус", titleFont, Brushes.Black, margin, 10);
            }

            // Рисуване на бар графика за статус
            if (statusStats.Count > 0)
            {
                int maxValue = statusStats.Values.Max();
                int barWidth = chartWidth / statusStats.Count - 10;
                int x = margin;
                int yBase = margin + 50;

                Color[] colors = { Color.Blue, Color.Orange, Color.Green, Color.Red };
                int colorIndex = 0;

                foreach (var kvp in statusStats)
                {
                    int barHeight = maxValue > 0 ? (int)((kvp.Value / (double)maxValue) * (chartHeight - 100)) : 0;
                    Rectangle barRect = new Rectangle(x, yBase - barHeight, barWidth, barHeight);

                    using (SolidBrush brush = new SolidBrush(colors[colorIndex % colors.Length]))
                    {
                        g.FillRectangle(brush, barRect);
                        g.DrawRectangle(Pens.Black, barRect);
                    }

                    // Етикет
                    using (Font labelFont = new Font("Segoe UI", 8))
                    {
                        string label = $"{kvp.Key}\n({kvp.Value})";
                        SizeF textSize = g.MeasureString(label, labelFont);
                        g.DrawString(label, labelFont, Brushes.Black, 
                            x + (barWidth - textSize.Width) / 2, yBase + 5);
                    }

                    x += barWidth + 10;
                    colorIndex++;
                }
            }

            // Рисуване на кръгова диаграма за приоритет
            if (priorityStats.Count > 0)
            {
                int total = priorityStats.Values.Sum();
                int centerX = width / 2;
                int centerY = height - 150;
                int radius = 80;
                float startAngle = 0;

                Color[] priorityColors = { Color.Green, Color.Orange, Color.Red, Color.DarkRed };
                int colorIdx = 0;

                foreach (var kvp in priorityStats.OrderByDescending(x => x.Key))
                {
                    float sweepAngle = (float)(kvp.Value / (double)total * 360);
                    
                    using (SolidBrush brush = new SolidBrush(priorityColors[colorIdx % priorityColors.Length]))
                    {
                        g.FillPie(brush, centerX - radius, centerY - radius, radius * 2, radius * 2, startAngle, sweepAngle);
                        g.DrawPie(Pens.Black, centerX - radius, centerY - radius, radius * 2, radius * 2, startAngle, sweepAngle);
                    }

                    // Етикет
                    float labelAngle = startAngle + sweepAngle / 2;
                    float labelX = centerX + (float)(radius * 1.3 * Math.Cos(labelAngle * Math.PI / 180));
                    float labelY = centerY + (float)(radius * 1.3 * Math.Sin(labelAngle * Math.PI / 180));
                    using (Font labelFont = new Font("Segoe UI", 8))
                    {
                        g.DrawString($"{kvp.Key}: {kvp.Value}", labelFont, Brushes.Black, labelX, labelY);
                    }

                    startAngle += sweepAngle;
                    colorIdx++;
                }

                using (Font titleFont = new Font("Segoe UI", 10, FontStyle.Bold))
                {
                    g.DrawString("Приоритет", titleFont, Brushes.Black, centerX - 40, centerY - 120);
                }
            }
        }

        // Обработчици на събития

        private void AddButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_titleTextBox.Text))
            {
                MessageBox.Show("Моля въведете заглавие!", "Грешка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Task task = new Task
            {
                Title = _titleTextBox.Text,
                Description = _descriptionTextBox.Text,
                Priority = (TaskPriority)_priorityComboBox.SelectedIndex,
                Status = (TaskStatus)_statusComboBox.SelectedIndex,
                DueDate = _hasDueDateCheckBox.Checked ? _dueDatePicker.Value : null,
                Category = _categoryTextBox.Text
            };

            _taskManager.AddTask(task);
            ClearForm();
            RefreshTaskList();
            UpdateStatistics();
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            if (_selectedTask == null) return;

            _selectedTask.Title = _titleTextBox.Text;
            _selectedTask.Description = _descriptionTextBox.Text;
            _selectedTask.Priority = (TaskPriority)_priorityComboBox.SelectedIndex;
            _selectedTask.Status = (TaskStatus)_statusComboBox.SelectedIndex;
            _selectedTask.DueDate = _hasDueDateCheckBox.Checked ? _dueDatePicker.Value : null;
            _selectedTask.Category = _categoryTextBox.Text;

            _taskManager.UpdateTask(_selectedTask);
            ClearForm();
            RefreshTaskList();
            UpdateStatistics();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (_selectedTask == null) return;

            if (MessageBox.Show("Сигурни ли сте?", "Потвърждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _taskManager.RemoveTask(_selectedTask.Id);
                ClearForm();
                RefreshTaskList();
                UpdateStatistics();
            }
        }

        private void TaskListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_taskListView.SelectedItems.Count > 0)
            {
                _selectedTask = (Task)_taskListView.SelectedItems[0].Tag;
                LoadTaskToForm(_selectedTask);
                _addButton.Enabled = false;
                _updateButton.Enabled = true;
                _deleteButton.Enabled = true;
            }
            else
            {
                ClearForm();
            }
        }

        private void TaskListView_DoubleClick(object sender, EventArgs e)
        {
            if (_selectedTask != null)
            {
                _selectedTask.MarkAsCompleted();
                _taskManager.UpdateTask(_selectedTask);
                RefreshTaskList();
                UpdateStatistics();
            }
        }

        private void FilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshTaskList();
        }

        private void SortComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshTaskList();
        }

        private void LoadTaskToForm(Task task)
        {
            _titleTextBox.Text = task.Title;
            _descriptionTextBox.Text = task.Description;
            _priorityComboBox.SelectedIndex = (int)task.Priority;
            _statusComboBox.SelectedIndex = (int)task.Status;
            _hasDueDateCheckBox.Checked = task.DueDate.HasValue;
            if (task.DueDate.HasValue)
                _dueDatePicker.Value = task.DueDate.Value;
            _categoryTextBox.Text = task.Category;
        }

        private void ClearForm()
        {
            _titleTextBox.Clear();
            _descriptionTextBox.Clear();
            _priorityComboBox.SelectedIndex = 1;
            _statusComboBox.SelectedIndex = 0;
            _hasDueDateCheckBox.Checked = false;
            _categoryTextBox.Text = "General";
            _selectedTask = null;
            _addButton.Enabled = true;
            _updateButton.Enabled = false;
            _deleteButton.Enabled = false;
            _taskListView.SelectedItems.Clear();
        }

        // Обработчици на събития от TaskManagerService (Delegates)

        private void TaskManager_TaskAdded(object sender, Task task)
        {
            _statusLabel.Text = $"Задача добавена: {task.Title}";
        }

        private void TaskManager_TaskRemoved(object sender, Task task)
        {
            _statusLabel.Text = $"Задача премахната: {task.Title}";
        }

        private void TaskManager_TaskUpdated(object sender, Task task)
        {
            _statusLabel.Text = $"Задача обновена: {task.Title}";
        }

        private void TaskManager_TaskStatusChanged(object sender, Task task, TaskStatus oldStatus, TaskStatus newStatus)
        {
            _statusLabel.Text = $"Статус променен: {task.Title} ({oldStatus} -> {newStatus})";
        }

        // Меню обработчици

        private void MenuItemOpen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var tasks = _serializationService.LoadTasks(dialog.FileName);
                        _taskManager.LoadTasks(tasks);
                        RefreshTaskList();
                        UpdateStatistics();
                        MessageBox.Show("Файл зареден успешно!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Грешка при зареждане: {ex.Message}", "Грешка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void MenuItemSave_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        _serializationService.SaveTasks(_taskManager.GetAllTasks(), dialog.FileName);
                        MessageBox.Show("Файл запазен успешно!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Грешка при запазване: {ex.Message}", "Грешка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void MenuItemAbout_Click(object sender, EventArgs e)
        {
            string message = "Task Manager Application\n\n" +
                           "Студент: Momchil Georgiev Tsanov\n" +
                           "Факултетен номер: 113172\n\n" +
                           "Това приложение демонстрира:\n" +
                           "- Windows Forms\n" +
                           "- Classes\n" +
                           "- Arrays & Collections\n" +
                           "- Interfaces\n" +
                           "- Delegates\n" +
                           "- Serialization\n" +
                           "- Multilingual Interface\n" +
                           "- Graphics with C#";

            MessageBox.Show(message, "За програмата", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LanguageService_LanguageChanged(object sender, EventArgs e)
        {
            this.Text = _languageService.GetString("Title");
            _fileMenu.Text = _languageService.GetString("MenuFile");
            _editMenu.Text = _languageService.GetString("MenuEdit");
            _viewMenu.Text = _languageService.GetString("MenuView");
            _languageMenu.Text = _languageService.GetString("MenuLanguage");
            _helpMenu.Text = _languageService.GetString("MenuHelp");
        }

        /// <summary>
        /// Освобождава ресурсите
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
        }
    }
}

