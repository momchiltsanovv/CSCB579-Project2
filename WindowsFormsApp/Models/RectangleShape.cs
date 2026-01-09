/*
 * Програма: Windows Forms Graphics Application
 * Студент: Momchil Georgiev Tsanov
 * Факултетен номер: 113172
 * 
 * Клас за правоъгълник
 * Демонстрира използването на Classes (Класове) и Inheritance (Наследяване)
 */

using System.Drawing;
using WindowsFormsApp.Models;

namespace WindowsFormsApp.Models
{
    /// <summary>
    /// Клас за представяне на правоъгълник
    /// </summary>
    public class RectangleShape : Shape
    {
        /// <summary>
        /// Конструктор за създаване на правоъгълник
        /// </summary>
        /// <param name="position">Позиция на правоъгълника</param>
        /// <param name="size">Размер на правоъгълника</param>
        /// <param name="fillColor">Цвят на запълването</param>
        /// <param name="borderColor">Цвят на контура</param>
        public RectangleShape(Point position, Size size, Color fillColor, Color borderColor)
            : base(position, size, fillColor, borderColor)
        {
        }

        /// <summary>
        /// Рисува правоъгълника върху Graphics обекта
        /// </summary>
        /// <param name="g">Graphics обект за рисуване</param>
        public override void Draw(Graphics g)
        {
            // Създаване на кист за запълване
            using (SolidBrush brush = new SolidBrush(FillColor))
            {
                // Рисуване на запълнения правоъгълник
                g.FillRectangle(brush, GetBounds());
            }

            // Създаване на молив за контура
            using (Pen pen = new Pen(BorderColor, BorderWidth))
            {
                // Рисуване на контура на правоъгълника
                g.DrawRectangle(pen, GetBounds());
            }
        }
    }
}
