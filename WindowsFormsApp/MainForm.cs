/*
 * Програма: Windows Forms Graphics Application
 * Студент: Momchil Georgiev Tsanov
 * Факултетен номер: 113172
 * 
 * Главна форма на приложението
 * Демонстрира използването на Windows Forms, Graphics, Menus, Arrays & Collections
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsApp.Delegates;
using WindowsFormsApp.Models;
using WindowsFormsApp.Services;

namespace WindowsFormsApp
{
    /// <summary>
    /// Главна форма на приложението
    /// </summary>
    public partial class MainForm : Form
    {
        // Колекции за съхранение на форми (Arrays & Collections)
        private List<Shape> _shapes;
        private List<AnimatedShape> _animatedShapes;

        // Сервизи
        private LanguageService _languageService;
        private SerializationService _serializationService;

        // Графични компоненти
        private PictureBox _drawingArea;
        private Bitmap _bitmap;
        private Graphics _graphics;
        private Timer _animationTimer;

        // Менюта
        private MenuStrip _menuStrip;
        private ToolStripMenuItem _fileMenu;
        private ToolStripMenuItem _editMenu;
        private ToolStripMenuItem _viewMenu;
        private ToolStripMenuItem _languageMenu;
        private ToolStripMenuItem _helpMenu;

        // Toolbar с инструменти
        private ToolStrip _toolStrip;
        private ToolStripButton _btnCircle;
        private ToolStripButton _btnRectangle;
        private ToolStripButton _btnAnimated;
        private ToolStripSeparator _separator1;
        private ToolStripButton _btnClear;
        private ToolStripSeparator _separator2;
        private ToolStripButton _btnPlayAnimation;
        private ToolStripButton _btnStopAnimation;

        // Панел за цветове
        private Panel _colorPanel;
        private Label _fillColorLabel;
        private Button _fillColorButton;
        private Label _borderColorLabel;
        private Button _borderColorButton;
        private Panel _colorPreviewPanel;

        // Панел за инструменти (лява страна)
        private Panel _toolsPanel;
        private GroupBox _shapesGroupBox;
        private RadioButton _rbCircle;
        private RadioButton _rbRectangle;
        private RadioButton _rbAnimated;
        private GroupBox _propertiesGroupBox;
        private Label _lblFillColor;
        private Panel _fillColorPreview;
        private Label _lblBorderColor;
        private Panel _borderColorPreview;
        private Label _lblBorderWidth;
        private NumericUpDown _numBorderWidth;
        private Button _btnDeleteSelected;

        // Статус бар
        private StatusStrip _statusStrip;
        private ToolStripStatusLabel _statusLabel;
        private ToolStripStatusLabel _coordinatesLabel;

        // Делегати за събития
        private GraphicsEventHandler _onPaintHandler;
        private ShapeClickEventHandler _onShapeClickHandler;

        // Текущ избран цвят
        private Color _currentFillColor = Color.Blue;
        private Color _currentBorderColor = Color.Black;
        private int _currentBorderWidth = 2;

        // Избрана форма за редактиране
        private Shape _selectedShape = null;
        private bool _isDragging = false;
        private Point _dragStartPoint;

        // Режим на рисуване
        private DrawingMode _drawingMode = DrawingMode.None;

        /// <summary>
        /// Енумерация за режими на рисуване
        /// </summary>
        private enum DrawingMode
        {
            None,
            Circle,
            Rectangle,
            Animated
        }

        /// <summary>
        /// Конструктор на главната форма
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            InitializeServices();
            InitializeLayout();
            InitializeToolbar();
            InitializeToolsPanel();
            InitializeDrawingArea();
            InitializeMenu();
            InitializeStatusBar();
            InitializeAnimation();
            SetupEventHandlers();
        }

        /// <summary>
        /// Инициализира компонентите на формата
        /// </summary>
        private void InitializeComponent()
        {
            // Настройки на формата
            this.Text = "Графично Приложение - Graphics Application";
            this.Size = new Size(1400, 900);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.DoubleBuffered = true; // За по-плавна анимация
            this.MinimumSize = new Size(1000, 600);

            // Инициализиране на колекциите (Arrays & Collections)
            _shapes = new List<Shape>();
            _animatedShapes = new List<AnimatedShape>();
        }

        /// <summary>
        /// Инициализира сервизите
        /// </summary>
        private void InitializeServices()
        {
            _languageService = new LanguageService();
            _languageService.LanguageChanged += LanguageService_LanguageChanged;
            
            _serializationService = new SerializationService();
        }

        /// <summary>
        /// Инициализира основния layout на формата
        /// </summary>
        private void InitializeLayout()
        {
            // Създаване на SplitContainer за разделяне на интерфейса
            SplitContainer mainSplit = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 50,
                FixedPanel = FixedPanel.Panel1
            };

            // Горен панел за toolbar
            Panel topPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(245, 245, 245),
                Height = 50
            };
            mainSplit.Panel1.Controls.Add(topPanel);

            // Долен SplitContainer за tools и drawing area
            SplitContainer bottomSplit = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                SplitterDistance = 200,
                FixedPanel = FixedPanel.Panel1
            };
            mainSplit.Panel2.Controls.Add(bottomSplit);

            // Запазване на референции за по-късна употреба
            this.Controls.Add(mainSplit);
        }

        /// <summary>
        /// Инициализира toolbar-а
        /// </summary>
        private void InitializeToolbar()
        {
            _toolStrip = new ToolStrip
            {
                Dock = DockStyle.Fill,
                GripStyle = ToolStripGripStyle.Hidden,
                BackColor = Color.FromArgb(245, 245, 245)
            };

            // Бутон за кръг
            _btnCircle = new ToolStripButton("● Кръг")
            {
                DisplayStyle = ToolStripItemDisplayStyle.Text,
                Font = new Font("Segoe UI", 9, FontStyle.Regular)
            };
            _btnCircle.Click += (s, e) => { _drawingMode = DrawingMode.Circle; UpdateToolbarButtons(); };

            // Бутон за правоъгълник
            _btnRectangle = new ToolStripButton("■ Правоъгълник")
            {
                DisplayStyle = ToolStripItemDisplayStyle.Text,
                Font = new Font("Segoe UI", 9, FontStyle.Regular)
            };
            _btnRectangle.Click += (s, e) => { _drawingMode = DrawingMode.Rectangle; UpdateToolbarButtons(); };

            // Бутон за анимация
            _btnAnimated = new ToolStripButton("◉ Анимация")
            {
                DisplayStyle = ToolStripItemDisplayStyle.Text,
                Font = new Font("Segoe UI", 9, FontStyle.Regular)
            };
            _btnAnimated.Click += (s, e) => { _drawingMode = DrawingMode.Animated; UpdateToolbarButtons(); };

            _separator1 = new ToolStripSeparator();

            // Бутон за изчистване
            _btnClear = new ToolStripButton("🗑 Изчисти")
            {
                DisplayStyle = ToolStripItemDisplayStyle.Text,
                Font = new Font("Segoe UI", 9, FontStyle.Regular)
            };
            _btnClear.Click += MenuItemClear_Click;

            _separator2 = new ToolStripSeparator();

            // Бутон за стартиране на анимация
            _btnPlayAnimation = new ToolStripButton("▶ Старт Анимация")
            {
                DisplayStyle = ToolStripItemDisplayStyle.Text,
                Font = new Font("Segoe UI", 9, FontStyle.Regular)
            };
            _btnPlayAnimation.Click += (s, e) => { _animationTimer.Start(); _statusLabel.Text = "Анимация стартирана"; };

            // Бутон за спиране на анимация
            _btnStopAnimation = new ToolStripButton("⏸ Спри Анимация")
            {
                DisplayStyle = ToolStripItemDisplayStyle.Text,
                Font = new Font("Segoe UI", 9, FontStyle.Regular)
            };
            _btnStopAnimation.Click += (s, e) => { _animationTimer.Stop(); _statusLabel.Text = "Анимация спряна"; };

            _toolStrip.Items.AddRange(new ToolStripItem[]
            {
                _btnCircle, _btnRectangle, _btnAnimated,
                _separator1,
                _btnClear,
                _separator2,
                _btnPlayAnimation, _btnStopAnimation
            });

            // Добавяне на toolbar-а в горния панел
            if (this.Controls[0] is SplitContainer mainSplit && mainSplit.Panel1.Controls.Count > 0)
            {
                mainSplit.Panel1.Controls[0].Controls.Add(_toolStrip);
            }
        }

        /// <summary>
        /// Обновява състоянието на бутоните в toolbar-а
        /// </summary>
        private void UpdateToolbarButtons()
        {
            _btnCircle.Checked = _drawingMode == DrawingMode.Circle;
            _btnRectangle.Checked = _drawingMode == DrawingMode.Rectangle;
            _btnAnimated.Checked = _drawingMode == DrawingMode.Animated;
        }

        /// <summary>
        /// Инициализира панела с инструменти
        /// </summary>
        private void InitializeToolsPanel()
        {
            _toolsPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(250, 250, 250),
                Padding = new Padding(10)
            };

            // Група за форми
            _shapesGroupBox = new GroupBox
            {
                Text = "Форми / Shapes",
                Dock = DockStyle.Top,
                Height = 120,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Padding = new Padding(10, 20, 10, 10)
            };

            _rbCircle = new RadioButton
            {
                Text = "● Кръг / Circle",
                Dock = DockStyle.Top,
                Height = 25,
                Font = new Font("Segoe UI", 9)
            };
            _rbCircle.CheckedChanged += (s, e) => { if (_rbCircle.Checked) _drawingMode = DrawingMode.Circle; };

            _rbRectangle = new RadioButton
            {
                Text = "■ Правоъгълник / Rectangle",
                Dock = DockStyle.Top,
                Height = 25,
                Font = new Font("Segoe UI", 9)
            };
            _rbRectangle.CheckedChanged += (s, e) => { if (_rbRectangle.Checked) _drawingMode = DrawingMode.Rectangle; };

            _rbAnimated = new RadioButton
            {
                Text = "◉ Анимирана / Animated",
                Dock = DockStyle.Top,
                Height = 25,
                Font = new Font("Segoe UI", 9)
            };
            _rbAnimated.CheckedChanged += (s, e) => { if (_rbAnimated.Checked) _drawingMode = DrawingMode.Animated; };

            _shapesGroupBox.Controls.Add(_rbAnimated);
            _shapesGroupBox.Controls.Add(_rbRectangle);
            _shapesGroupBox.Controls.Add(_rbCircle);

            // Група за свойства
            _propertiesGroupBox = new GroupBox
            {
                Text = "Свойства / Properties",
                Dock = DockStyle.Top,
                Height = 200,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Padding = new Padding(10, 20, 10, 10)
            };

            // Fill Color
            _lblFillColor = new Label
            {
                Text = "Цвят запълване / Fill Color:",
                Dock = DockStyle.Top,
                Height = 20,
                Font = new Font("Segoe UI", 8)
            };

            _fillColorPreview = new Panel
            {
                Dock = DockStyle.Top,
                Height = 30,
                BackColor = _currentFillColor,
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Hand
            };
            _fillColorPreview.Click += FillColorPreview_Click;

            // Border Color
            _lblBorderColor = new Label
            {
                Text = "Цвят контур / Border Color:",
                Dock = DockStyle.Top,
                Height = 20,
                Font = new Font("Segoe UI", 8),
                Margin = new Padding(0, 10, 0, 0)
            };

            _borderColorPreview = new Panel
            {
                Dock = DockStyle.Top,
                Height = 30,
                BackColor = _currentBorderColor,
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Hand
            };
            _borderColorPreview.Click += BorderColorPreview_Click;

            // Border Width
            _lblBorderWidth = new Label
            {
                Text = "Дебелина контур / Border Width:",
                Dock = DockStyle.Top,
                Height = 20,
                Font = new Font("Segoe UI", 8),
                Margin = new Padding(0, 10, 0, 0)
            };

            _numBorderWidth = new NumericUpDown
            {
                Dock = DockStyle.Top,
                Height = 25,
                Minimum = 1,
                Maximum = 20,
                Value = _currentBorderWidth,
                Font = new Font("Segoe UI", 9)
            };
            _numBorderWidth.ValueChanged += (s, e) => 
            { 
                _currentBorderWidth = (int)_numBorderWidth.Value;
                if (_selectedShape != null)
                {
                    _selectedShape.BorderWidth = _currentBorderWidth;
                    Redraw();
                }
            };

            // Delete button
            _btnDeleteSelected = new Button
            {
                Text = "🗑 Изтрий избрано / Delete Selected",
                Dock = DockStyle.Top,
                Height = 35,
                Font = new Font("Segoe UI", 9),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(0, 10, 0, 0)
            };
            _btnDeleteSelected.FlatAppearance.BorderSize = 0;
            _btnDeleteSelected.Click += BtnDeleteSelected_Click;

            _propertiesGroupBox.Controls.Add(_btnDeleteSelected);
            _propertiesGroupBox.Controls.Add(_numBorderWidth);
            _propertiesGroupBox.Controls.Add(_lblBorderWidth);
            _propertiesGroupBox.Controls.Add(_borderColorPreview);
            _propertiesGroupBox.Controls.Add(_lblBorderColor);
            _propertiesGroupBox.Controls.Add(_fillColorPreview);
            _propertiesGroupBox.Controls.Add(_lblFillColor);

            _toolsPanel.Controls.Add(_propertiesGroupBox);
            _toolsPanel.Controls.Add(_shapesGroupBox);

            // Добавяне на tools панела в лявата страна
            if (this.Controls[0] is SplitContainer mainSplit && 
                mainSplit.Panel2.Controls[0] is SplitContainer bottomSplit)
            {
                bottomSplit.Panel1.Controls.Add(_toolsPanel);
            }
        }

        /// <summary>
        /// Обработчик за кликване върху fill color preview
        /// </summary>
        private void FillColorPreview_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.Color = _currentFillColor;
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    _currentFillColor = colorDialog.Color;
                    _fillColorPreview.BackColor = _currentFillColor;
                    if (_selectedShape != null)
                    {
                        _selectedShape.FillColor = _currentFillColor;
                        Redraw();
                    }
                }
            }
        }

        /// <summary>
        /// Обработчик за кликване върху border color preview
        /// </summary>
        private void BorderColorPreview_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.Color = _currentBorderColor;
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    _currentBorderColor = colorDialog.Color;
                    _borderColorPreview.BackColor = _currentBorderColor;
                    if (_selectedShape != null)
                    {
                        _selectedShape.BorderColor = _currentBorderColor;
                        Redraw();
                    }
                }
            }
        }

        /// <summary>
        /// Обработчик за изтриване на избраната форма
        /// </summary>
        private void BtnDeleteSelected_Click(object sender, EventArgs e)
        {
            if (_selectedShape != null)
            {
                _shapes.Remove(_selectedShape);
                _selectedShape = null;
                Redraw();
                _statusLabel.Text = "Форма изтрита / Shape deleted";
            }
        }

        /// <summary>
        /// Инициализира областта за рисуване
        /// </summary>
        private void InitializeDrawingArea()
        {
            _drawingArea = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                SizeMode = PictureBoxSizeMode.Normal,
                Cursor = Cursors.Cross
            };

            // Създаване на Bitmap за двойно буфериране
            _bitmap = new Bitmap(1200, 800);
            _graphics = Graphics.FromImage(_bitmap);
            _graphics.SmoothingMode = SmoothingMode.AntiAlias;
            _graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            _graphics.CompositingQuality = CompositingQuality.HighQuality;

            _drawingArea.Image = _bitmap;
            _drawingArea.MouseClick += DrawingArea_MouseClick;
            _drawingArea.MouseDown += DrawingArea_MouseDown;
            _drawingArea.MouseMove += DrawingArea_MouseMove;
            _drawingArea.MouseUp += DrawingArea_MouseUp;
            _drawingArea.Paint += DrawingArea_Paint;
            _drawingArea.Resize += DrawingArea_Resize;

            // Добавяне на drawing area в дясната страна
            if (this.Controls[0] is SplitContainer mainSplit && 
                mainSplit.Panel2.Controls[0] is SplitContainer bottomSplit)
            {
                bottomSplit.Panel2.Controls.Add(_drawingArea);
            }
        }

        /// <summary>
        /// Обработчик за промяна на размера на drawing area
        /// </summary>
        private void DrawingArea_Resize(object sender, EventArgs e)
        {
            if (_drawingArea.Width > 0 && _drawingArea.Height > 0)
            {
                if (_bitmap != null) _bitmap.Dispose();
                if (_graphics != null) _graphics.Dispose();

                _bitmap = new Bitmap(_drawingArea.Width, _drawingArea.Height);
                _graphics = Graphics.FromImage(_bitmap);
                _graphics.SmoothingMode = SmoothingMode.AntiAlias;
                _graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                _graphics.CompositingQuality = CompositingQuality.HighQuality;

                _drawingArea.Image = _bitmap;
                Redraw();
            }
        }

        /// <summary>
        /// Обработчик за натискане на мишката
        /// </summary>
        private void DrawingArea_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Проверка дали е кликнато върху форма
                _selectedShape = _shapes.FirstOrDefault(s => s.Contains(e.Location));
                
                if (_selectedShape != null)
                {
                    _isDragging = true;
                    _dragStartPoint = e.Location;
                    _statusLabel.Text = "Форма избрана / Shape selected";
                }
                else if (_drawingMode != DrawingMode.None)
                {
                    // Създаване на нова форма
                    CreateShapeAt(e.Location);
                }
            }
        }

        /// <summary>
        /// Обработчик за движение на мишката
        /// </summary>
        private void DrawingArea_MouseMove(object sender, MouseEventArgs e)
        {
            // Обновяване на координатите в статус бара
            _coordinatesLabel.Text = $"X: {e.X}, Y: {e.Y}";

            // Drag & Drop на форма
            if (_isDragging && _selectedShape != null)
            {
                int deltaX = e.X - _dragStartPoint.X;
                int deltaY = e.Y - _dragStartPoint.Y;
                _selectedShape.Position = new Point(
                    _selectedShape.Position.X + deltaX,
                    _selectedShape.Position.Y + deltaY
                );
                _dragStartPoint = e.Location;
                Redraw();
            }
        }

        /// <summary>
        /// Обработчик за отпускане на мишката
        /// </summary>
        private void DrawingArea_MouseUp(object sender, MouseEventArgs e)
        {
            _isDragging = false;
        }

        /// <summary>
        /// Създава форма на дадена позиция според текущия режим
        /// </summary>
        private void CreateShapeAt(Point location)
        {
            Random random = new Random();
            Size size = new Size(random.Next(30, 80), random.Next(30, 80));

            switch (_drawingMode)
            {
                case DrawingMode.Circle:
                    Circle circle = new Circle(
                        location,
                        size.Width / 2,
                        _currentFillColor,
                        _currentBorderColor
                    );
                    circle.BorderWidth = _currentBorderWidth;
                    _shapes.Add(circle);
                    break;

                case DrawingMode.Rectangle:
                    RectangleShape rectangle = new RectangleShape(
                        location,
                        size,
                        _currentFillColor,
                        _currentBorderColor
                    );
                    rectangle.BorderWidth = _currentBorderWidth;
                    _shapes.Add(rectangle);
                    break;

                case DrawingMode.Animated:
                    AnimatedShape animatedShape = new AnimatedShape(
                        location,
                        new Size(30, 30),
                        _currentFillColor,
                        _currentBorderColor,
                        random.Next(-5, 6),
                        random.Next(-5, 6),
                        new Rectangle(0, 0, _drawingArea.Width, _drawingArea.Height)
                    );
                    animatedShape.BorderWidth = _currentBorderWidth;
                    _animatedShapes.Add(animatedShape);
                    break;
            }

            Redraw();
            _statusLabel.Text = "Форма добавена / Shape added";
        }

        /// <summary>
        /// Инициализира менютата
        /// </summary>
        private void InitializeMenu()
        {
            _menuStrip = new MenuStrip();

            // Меню Файл
            _fileMenu = new ToolStripMenuItem(_languageService.GetString("MenuFile"));
            _fileMenu.DropDownItems.Add(_languageService.GetString("MenuItemNew"), null, MenuItemNew_Click);
            _fileMenu.DropDownItems.Add(_languageService.GetString("MenuItemOpen"), null, MenuItemOpen_Click);
            _fileMenu.DropDownItems.Add(_languageService.GetString("MenuItemSave"), null, MenuItemSave_Click);
            _fileMenu.DropDownItems.Add(new ToolStripSeparator());
            _fileMenu.DropDownItems.Add(_languageService.GetString("MenuItemExit"), null, MenuItemExit_Click);

            // Меню Редактиране
            _editMenu = new ToolStripMenuItem(_languageService.GetString("MenuEdit"));
            _editMenu.DropDownItems.Add(_languageService.GetString("MenuItemClear"), null, MenuItemClear_Click);
            _editMenu.DropDownItems.Add(new ToolStripSeparator());
            _editMenu.DropDownItems.Add("Избери всичко / Select All", null, (s, e) => { _selectedShape = null; Redraw(); });
            _editMenu.DropDownItems.Add("Отмени избор / Deselect", null, (s, e) => { _selectedShape = null; Redraw(); });

            // Меню Изглед
            _viewMenu = new ToolStripMenuItem(_languageService.GetString("MenuView"));
            _viewMenu.DropDownItems.Add("Добави кръг / Add Circle", null, AddCircle_Click);
            _viewMenu.DropDownItems.Add("Добави правоъгълник / Add Rectangle", null, AddRectangle_Click);
            _viewMenu.DropDownItems.Add("Добави анимация / Add Animation", null, AddAnimatedShape_Click);

            // Меню Език
            _languageMenu = new ToolStripMenuItem(_languageService.GetString("MenuLanguage"));
            var bgItem = new ToolStripMenuItem(_languageService.GetString("MenuItemBulgarian"), null, (s, e) => _languageService.CurrentLanguage = Language.Bulgarian);
            var enItem = new ToolStripMenuItem(_languageService.GetString("MenuItemEnglish"), null, (s, e) => _languageService.CurrentLanguage = Language.English);
            _languageMenu.DropDownItems.Add(bgItem);
            _languageMenu.DropDownItems.Add(enItem);

            // Меню Помощ
            _helpMenu = new ToolStripMenuItem(_languageService.GetString("MenuHelp"));
            _helpMenu.DropDownItems.Add(_languageService.GetString("MenuItemAbout"), null, MenuItemAbout_Click);

            _menuStrip.Items.Add(_fileMenu);
            _menuStrip.Items.Add(_editMenu);
            _menuStrip.Items.Add(_viewMenu);
            _menuStrip.Items.Add(_languageMenu);
            _menuStrip.Items.Add(_helpMenu);

            this.MainMenuStrip = _menuStrip;
            this.Controls.Add(_menuStrip);
        }

        /// <summary>
        /// Инициализира статус бара
        /// </summary>
        private void InitializeStatusBar()
        {
            _statusStrip = new StatusStrip();
            _statusLabel = new ToolStripStatusLabel(_languageService.GetString("StatusReady"));
            _coordinatesLabel = new ToolStripStatusLabel("X: 0, Y: 0")
            {
                Alignment = ToolStripItemAlignment.Right
            };
            _statusStrip.Items.Add(_statusLabel);
            _statusStrip.Items.Add(_coordinatesLabel);
            this.Controls.Add(_statusStrip);
        }

        /// <summary>
        /// Инициализира анимационния таймер
        /// </summary>
        private void InitializeAnimation()
        {
            _animationTimer = new Timer
            {
                Interval = 16 // ~60 FPS
            };
            _animationTimer.Tick += AnimationTimer_Tick;
            _animationTimer.Start();
        }

        /// <summary>
        /// Настройва обработчиците на събития (Delegates)
        /// </summary>
        private void SetupEventHandlers()
        {
            _onPaintHandler = OnCustomPaint;
            _onShapeClickHandler = OnShapeClicked;
        }

        /// <summary>
        /// Обработчик за кликване в областта за рисуване
        /// </summary>
        private void DrawingArea_MouseClick(object sender, MouseEventArgs e)
        {
            if (!_isDragging)
            {
                Point clickPoint = e.Location;
                Shape clickedShape = _shapes.FirstOrDefault(s => s.Contains(clickPoint));
                
                if (clickedShape != null)
                {
                    _selectedShape = clickedShape;
                    _onShapeClickHandler?.Invoke(clickedShape, clickPoint);
                    UpdateSelectedShapeProperties();
                    Redraw();
                }
            }
        }

        /// <summary>
        /// Обновява свойствата на избраната форма в панела
        /// </summary>
        private void UpdateSelectedShapeProperties()
        {
            if (_selectedShape != null)
            {
                _currentFillColor = _selectedShape.FillColor;
                _currentBorderColor = _selectedShape.BorderColor;
                _currentBorderWidth = _selectedShape.BorderWidth;
                _fillColorPreview.BackColor = _currentFillColor;
                _borderColorPreview.BackColor = _currentBorderColor;
                _numBorderWidth.Value = _currentBorderWidth;
            }
        }

        /// <summary>
        /// Обработчик за рисуване на PictureBox
        /// </summary>
        private void DrawingArea_Paint(object sender, PaintEventArgs e)
        {
            _onPaintHandler?.Invoke(this, e.Graphics);
        }

        /// <summary>
        /// Потребителски метод за рисуване (използва се чрез делегат)
        /// </summary>
        private void OnCustomPaint(object sender, Graphics graphics)
        {
            DrawAllShapes(_graphics);
            graphics.DrawImage(_bitmap, 0, 0);
        }

        /// <summary>
        /// Обработчик за събитие при кликване върху форма (използва се чрез делегат)
        /// </summary>
        private void OnShapeClicked(object sender, Point point)
        {
            _statusLabel.Text = $"Кликнато върху форма в точка ({point.X}, {point.Y})";
        }

        /// <summary>
        /// Рисува всички форми
        /// </summary>
        private void DrawAllShapes(Graphics g)
        {
            // Изчистване на областта с градиент
            using (LinearGradientBrush brush = new LinearGradientBrush(
                new Point(0, 0),
                new Point(0, _drawingArea.Height),
                Color.FromArgb(255, 255, 255),
                Color.FromArgb(245, 245, 250)))
            {
                g.FillRectangle(brush, 0, 0, _drawingArea.Width, _drawingArea.Height);
            }

            // Рисуване на статичните форми
            foreach (Shape shape in _shapes)
            {
                shape.Draw(g);
            }

            // Рисуване на анимираните форми
            foreach (AnimatedShape animatedShape in _animatedShapes)
            {
                animatedShape.Draw(g);
            }

            // Рисуване на селекция маркер
            if (_selectedShape != null)
            {
                Rectangle bounds = new Rectangle(_selectedShape.Position, _selectedShape.Size);
                bounds.Inflate(5, 5);
                using (Pen selectionPen = new Pen(Color.Blue, 2))
                {
                    selectionPen.DashStyle = DashStyle.Dash;
                    g.DrawRectangle(selectionPen, bounds);
                }
            }

            // Рисуване на стилизиран текст с информация
            DrawStyledText(g);

            _drawingArea.Invalidate();
        }

        /// <summary>
        /// Рисува стилизиран текст (демонстрация на стилизиране на текст)
        /// </summary>
        private void DrawStyledText(Graphics g)
        {
            // Градиентна кист за заглавие
            using (LinearGradientBrush brush = new LinearGradientBrush(
                new Point(10, 10),
                new Point(300, 10),
                Color.FromArgb(70, 130, 180),
                Color.FromArgb(138, 43, 226)))
            {
                using (Font font = new Font("Segoe UI", 18, FontStyle.Bold))
                {
                    g.DrawString("Графично Приложение", font, brush, 10, 10);
                }
            }

            // Информация с контур
            string info = $"Форми: {_shapes.Count} | Анимации: {_animatedShapes.Count} | Режим: {_drawingMode}";
            using (Font font = new Font("Segoe UI", 10, FontStyle.Regular))
            {
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddString(
                        info,
                        font.FontFamily,
                        (int)font.Style,
                        g.DpiY * font.Size / 72,
                        new Point(10, 50),
                        StringFormat.GenericDefault);

                    // Запълване
                    using (SolidBrush fillBrush = new SolidBrush(Color.FromArgb(255, 255, 200)))
                    {
                        g.FillPath(fillBrush, path);
                    }

                    // Контур
                    using (Pen outlinePen = new Pen(Color.FromArgb(100, 100, 100), 1))
                    {
                        g.DrawPath(outlinePen, path);
                    }
                }
            }
        }

        /// <summary>
        /// Обновява анимацията
        /// </summary>
        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            foreach (AnimatedShape animatedShape in _animatedShapes)
            {
                animatedShape.Update();
            }

            if (_animatedShapes.Count > 0)
            {
                Redraw();
            }
        }

        /// <summary>
        /// Прерисува всичко
        /// </summary>
        private void Redraw()
        {
            DrawAllShapes(_graphics);
            _drawingArea.Invalidate();
        }

        /// <summary>
        /// Добавя кръг
        /// </summary>
        private void AddCircle_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            Point position = new Point(
                random.Next(50, Math.Max(100, _drawingArea.Width - 100)),
                random.Next(50, Math.Max(100, _drawingArea.Height - 100))
            );
            int radius = random.Next(20, 50);

            Circle circle = new Circle(
                position,
                radius,
                _currentFillColor,
                _currentBorderColor
            );
            circle.BorderWidth = _currentBorderWidth;

            _shapes.Add(circle);
            Redraw();
            _statusLabel.Text = _languageService.GetString("StatusDrawing");
        }

        /// <summary>
        /// Добавя правоъгълник
        /// </summary>
        private void AddRectangle_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            Point position = new Point(
                random.Next(50, Math.Max(150, _drawingArea.Width - 150)),
                random.Next(50, Math.Max(150, _drawingArea.Height - 150))
            );
            Size size = new Size(random.Next(50, 150), random.Next(50, 150));

            RectangleShape rectangle = new RectangleShape(
                position,
                size,
                _currentFillColor,
                _currentBorderColor
            );
            rectangle.BorderWidth = _currentBorderWidth;

            _shapes.Add(rectangle);
            Redraw();
            _statusLabel.Text = _languageService.GetString("StatusDrawing");
        }

        /// <summary>
        /// Добавя анимирана форма
        /// </summary>
        private void AddAnimatedShape_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            Point position = new Point(
                random.Next(50, Math.Max(100, _drawingArea.Width - 100)),
                random.Next(50, Math.Max(100, _drawingArea.Height - 100))
            );
            Size size = new Size(30, 30);

            AnimatedShape animatedShape = new AnimatedShape(
                position,
                size,
                _currentFillColor,
                _currentBorderColor,
                random.Next(-5, 6),
                random.Next(-5, 6),
                new Rectangle(0, 0, _drawingArea.Width, _drawingArea.Height)
            );
            animatedShape.BorderWidth = _currentBorderWidth;

            _animatedShapes.Add(animatedShape);
            Redraw();
            _statusLabel.Text = "Анимация добавена / Animation added";
        }

        // Обработчици за менютата

        private void MenuItemNew_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Сигурни ли сте, че искате да изчистите всичко? / Are you sure you want to clear everything?",
                "Потвърждение / Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _shapes.Clear();
                _animatedShapes.Clear();
                _selectedShape = null;
                Redraw();
                _statusLabel.Text = _languageService.GetString("StatusReady");
            }
        }

        private void MenuItemOpen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var collection = _serializationService.LoadShapes(dialog.FileName);
                        _shapes = collection.Shapes ?? new List<Shape>();
                        Redraw();
                        _statusLabel.Text = "Файл зареден / File loaded";
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
                        var collection = new ShapeCollection();
                        collection.Shapes = _shapes;
                        _serializationService.SaveShapes(collection, dialog.FileName);
                        _statusLabel.Text = "Файл запазен / File saved";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Грешка при запазване: {ex.Message}", "Грешка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void MenuItemExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MenuItemClear_Click(object sender, EventArgs e)
        {
            MenuItemNew_Click(sender, e);
        }

        private void MenuItemAbout_Click(object sender, EventArgs e)
        {
            string message = "Графично Приложение\n\n" +
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
                           "- Graphics with C#\n" +
                           "- Dynamic Animations";

            MessageBox.Show(message, "За програмата", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Обработчик за промяна на езика
        /// </summary>
        private void LanguageService_LanguageChanged(object sender, EventArgs e)
        {
            _fileMenu.Text = _languageService.GetString("MenuFile");
            _editMenu.Text = _languageService.GetString("MenuEdit");
            _viewMenu.Text = _languageService.GetString("MenuView");
            _languageMenu.Text = _languageService.GetString("MenuLanguage");
            _helpMenu.Text = _languageService.GetString("MenuHelp");
            
            this.Text = _languageService.GetString("Title");
            _statusLabel.Text = _languageService.GetString("StatusReady");
        }

        /// <summary>
        /// Освобождава ресурсите при затваряне на формата
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _animationTimer?.Stop();
            _animationTimer?.Dispose();
            _graphics?.Dispose();
            _bitmap?.Dispose();
            base.OnFormClosing(e);
        }
    }
}
