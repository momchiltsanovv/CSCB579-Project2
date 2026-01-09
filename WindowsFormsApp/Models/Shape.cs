/*
 * Програма: Windows Forms Graphics Application
 * Студент: Momchil Georgiev Tsanov
 * Факултетен номер: 113172
 * 
 * Базов клас за геометрични форми
 * Демонстрира използването на Classes (Класове) и Inheritance (Наследяване)
 */

using System.Drawing;
using WindowsFormsApp.Interfaces;

namespace WindowsFormsApp.Models
{
    /// <summary>
    /// Базов клас за геометрични форми
    /// </summary>
    public abstract class Shape : IDrawable
    {
        /// <summary>
        /// Позиция на формата
        /// </summary>
        public Point Position { get; set; }

        /// <summary>
        /// Размер на формата
        /// </summary>
        public Size Size { get; set; }

        /// <summary>
        /// Цвят на запълването
        /// </summary>
        public Color FillColor { get; set; }

        /// <summary>
        /// Цвят на контура
        /// </summary>
        public Color BorderColor { get; set; }

        /// <summary>
        /// Дебелина на контура
        /// </summary>
        public int BorderWidth { get; set; }

        /// <summary>
        /// Конструктор за създаване на форма
        /// </summary>
        /// <param name="position">Позиция на формата</param>
        /// <param name="size">Размер на формата</param>
        /// <param name="fillColor">Цвят на запълването</param>
        /// <param name="borderColor">Цвят на контура</param>
        protected Shape(Point position, Size size, Color fillColor, Color borderColor)
        {
            Position = position;
            Size = size;
            FillColor = fillColor;
            BorderColor = borderColor;
            BorderWidth = 2;
        }

        /// <summary>
        /// Абстрактен метод за рисуване на формата
        /// </summary>
        /// <param name="g">Graphics обект за рисуване</param>
        public abstract void Draw(Graphics g);

        /// <summary>
        /// Проверява дали точката е в границите на формата
        /// </summary>
        /// <param name="point">Точка за проверка</param>
        /// <returns>True ако точката е в границите</returns>
        public virtual bool Contains(Point point)
        {
            Rectangle bounds = new Rectangle(Position, Size);
            return bounds.Contains(point);
        }

        /// <summary>
        /// Връща правоъгълник с границите на формата
        /// </summary>
        /// <returns>Rectangle с границите</returns>
        protected Rectangle GetBounds()
        {
            return new Rectangle(Position, Size);
        }
    }
}
