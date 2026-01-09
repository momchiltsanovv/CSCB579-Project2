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

        // Статус бар
        private StatusStrip _statusStrip;
        private ToolStripStatusLabel _statusLabel;

        // Делегати за събития
        private GraphicsEventHandler _onPaintHandler;
        private ShapeClickEventHandler _onShapeClickHandler;

        // Текущ избран цвят
        private Color _currentFillColor = Color.Blue;
        private Color _currentBorderColor = Color.Black;

        // Брой на създадените форми
        private int _shapeCounter = 0;

        /// <summary>
        /// Конструктор на главната форма
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            InitializeServices();
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
            this.Text = "Графично Приложение";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            this.DoubleBuffered = true; // За по-плавна анимация

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
        /// Инициализира областта за рисуване
        /// </summary>
        private void InitializeDrawingArea()
        {
            _drawingArea = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                SizeMode = PictureBoxSizeMode.Normal
            };

            // Създаване на Bitmap за двойно буфериране
            _bitmap = new Bitmap(800, 600);
            _graphics = Graphics.FromImage(_bitmap);
            _graphics.SmoothingMode = SmoothingMode.AntiAlias; // За по-гладки линии
            _graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            _drawingArea.Image = _bitmap;
            _drawingArea.MouseClick += DrawingArea_MouseClick;
            _drawingArea.Paint += DrawingArea_Paint;

            this.Controls.Add(_drawingArea);
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

            // Меню Изглед
            _viewMenu = new ToolStripMenuItem(_languageService.GetString("MenuView"));
            var addCircleItem = new ToolStripMenuItem("Добави кръг / Add Circle", null, AddCircle_Click);
            var addRectangleItem = new ToolStripMenuItem("Добави правоъгълник / Add Rectangle", null, AddRectangle_Click);
            var addAnimatedItem = new ToolStripMenuItem("Добави анимация / Add Animation", null, AddAnimatedShape_Click);
            _viewMenu.DropDownItems.Add(addCircleItem);
            _viewMenu.DropDownItems.Add(addRectangleItem);
            _viewMenu.DropDownItems.Add(addAnimatedItem);

            // Меню Език
            _languageMenu = new ToolStripMenuItem(_languageService.GetString("MenuLanguage"));
            var bgItem = new ToolStripMenuItem(_languageService.GetString("MenuItemBulgarian"), null, (s, e) => _languageService.CurrentLanguage = Language.Bulgarian);
            var enItem = new ToolStripMenuItem(_languageService.GetString("MenuItemEnglish"), null, (s, e) => _languageService.CurrentLanguage = Language.English);
            _languageMenu.DropDownItems.Add(bgItem);
            _languageMenu.DropDownItems.Add(enItem);

            // Меню Помощ
            _helpMenu = new ToolStripMenuItem(_languageService.GetString("MenuHelp"));
            _helpMenu.DropDownItems.Add(_languageService.GetString("MenuItemAbout"), null, MenuItemAbout_Click);

            // Добавяне на менютата към MenuStrip
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
            _statusStrip.Items.Add(_statusLabel);
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
            // Използване на делегати за събития
            _onPaintHandler = OnCustomPaint;
            _onShapeClickHandler = OnShapeClicked;
        }

        /// <summary>
        /// Обработчик за кликване в областта за рисуване
        /// </summary>
        private void DrawingArea_MouseClick(object sender, MouseEventArgs e)
        {
            Point clickPoint = e.Location;
            
            // Проверка дали е кликнато върху форма
            Shape clickedShape = _shapes.FirstOrDefault(s => s.Contains(clickPoint));
            
            if (clickedShape != null)
            {
                // Извикване на делегата за събитие при кликване
                _onShapeClickHandler?.Invoke(clickedShape, clickPoint);
                
                // Промяна на цвят на кликнатата форма
                clickedShape.FillColor = GetRandomColor();
                Redraw();
            }
        }

        /// <summary>
        /// Обработчик за рисуване на PictureBox
        /// </summary>
        private void DrawingArea_Paint(object sender, PaintEventArgs e)
        {
            // Извикване на делегата за рисуване
            _onPaintHandler?.Invoke(this, e.Graphics);
        }

        /// <summary>
        /// Потребителски метод за рисуване (използва се чрез делегат)
        /// </summary>
        private void OnCustomPaint(object sender, Graphics graphics)
        {
            // Рисуване върху Bitmap
            DrawAllShapes(_graphics);
            
            // Копиране на Bitmap върху Graphics на PictureBox
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
            // Изчистване на областта
            g.Clear(Color.White);

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

            // Рисуване на стилизиран текст
            DrawStyledText(g);

            // Обновяване на PictureBox
            _drawingArea.Invalidate();
        }

        /// <summary>
        /// Рисува стилизиран текст (демонстрация на стилизиране на текст)
        /// </summary>
        private void DrawStyledText(Graphics g)
        {
            // Градиентна кист за текст
            using (LinearGradientBrush brush = new LinearGradientBrush(
                new Point(10, 10),
                new Point(200, 10),
                Color.Blue,
                Color.Purple))
            {
                // Шрифт с ефекти
                using (Font font = new Font("Arial", 16, FontStyle.Bold | FontStyle.Italic))
                {
                    g.DrawString("Графично Приложение", font, brush, 10, 10);
                }
            }

            // Текст с контур
            using (Font font = new Font("Times New Roman", 14, FontStyle.Bold))
            {
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddString(
                        $"Форми: {_shapes.Count + _animatedShapes.Count}",
                        font.FontFamily,
                        (int)font.Style,
                        g.DpiY * font.Size / 72,
                        new Point(10, 40),
                        StringFormat.GenericDefault);

                    // Запълване
                    using (SolidBrush fillBrush = new SolidBrush(Color.Yellow))
                    {
                        g.FillPath(fillBrush, path);
                    }

                    // Контур
                    using (Pen outlinePen = new Pen(Color.Red, 2))
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
            // Обновяване на всички анимирани форми
            foreach (AnimatedShape animatedShape in _animatedShapes)
            {
                animatedShape.Update();
            }

            // Прерисуване
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
                random.Next(50, _drawingArea.Width - 100),
                random.Next(50, _drawingArea.Height - 100)
            );
            int radius = random.Next(20, 50);

            Circle circle = new Circle(
                position,
                radius,
                GetRandomColor(),
                _currentBorderColor
            );

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
                random.Next(50, _drawingArea.Width - 150),
                random.Next(50, _drawingArea.Height - 150)
            );
            Size size = new Size(random.Next(50, 150), random.Next(50, 150));

            RectangleShape rectangle = new RectangleShape(
                position,
                size,
                GetRandomColor(),
                _currentBorderColor
            );

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
                random.Next(50, _drawingArea.Width - 100),
                random.Next(50, _drawingArea.Height - 100)
            );
            Size size = new Size(30, 30);

            AnimatedShape animatedShape = new AnimatedShape(
                position,
                size,
                GetRandomColor(),
                Color.Black,
                random.Next(-5, 6), // Скорост X
                random.Next(-5, 6), // Скорост Y
                new Rectangle(0, 0, _drawingArea.Width, _drawingArea.Height)
            );

            _animatedShapes.Add(animatedShape);
            Redraw();
            _statusLabel.Text = "Анимация добавена / Animation added";
        }

        /// <summary>
        /// Връща случаен цвят
        /// </summary>
        private Color GetRandomColor()
        {
            Random random = new Random();
            return Color.FromArgb(
                random.Next(50, 255),
                random.Next(50, 255),
                random.Next(50, 255)
            );
        }

        // Обработчици за менютата

        private void MenuItemNew_Click(object sender, EventArgs e)
        {
            _shapes.Clear();
            _animatedShapes.Clear();
            Redraw();
            _statusLabel.Text = _languageService.GetString("StatusReady");
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
                        _shapes = collection.Shapes;
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
            _shapes.Clear();
            _animatedShapes.Clear();
            Redraw();
            _statusLabel.Text = _languageService.GetString("StatusReady");
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
            // Обновяване на текстовете в менютата
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
