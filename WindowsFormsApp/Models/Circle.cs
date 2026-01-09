/*
 * Програма: Windows Forms Graphics Application
 * Студент: Momchil Georgiev Tsanov
 * Факултетен номер: 113172
 * 
 * Клас за кръг
 * Демонстрира използването на Classes (Класове) и Inheritance (Наследяване)
 */

using System.Drawing;
using WindowsFormsApp.Models;

namespace WindowsFormsApp.Models
{
    /// <summary>
    /// Клас за представяне на кръг
    /// </summary>
    public class Circle : Shape
    {
        /// <summary>
        /// Конструктор за създаване на кръг
        /// </summary>
        /// <param name="position">Позиция на кръга (център)</param>
        /// <param name="radius">Радиус на кръга</param>
        /// <param name="fillColor">Цвят на запълването</param>
        /// <param name="borderColor">Цвят на контура</param>
        public Circle(Point position, int radius, Color fillColor, Color borderColor)
            : base(position, new Size(radius * 2, radius * 2), fillColor, borderColor)
        {
        }

        /// <summary>
        /// Радиус на кръга
        /// </summary>
        public int Radius => Size.Width / 2;

        /// <summary>
        /// Рисува кръга върху Graphics обекта
        /// </summary>
        /// <param name="g">Graphics обект за рисуване</param>
        public override void Draw(Graphics g)
        {
            // Създаване на кист за запълване
            using (SolidBrush brush = new SolidBrush(FillColor))
            {
                // Рисуване на запълнения кръг
                g.FillEllipse(brush, GetBounds());
            }

            // Създаване на молив за контура
            using (Pen pen = new Pen(BorderColor, BorderWidth))
            {
                // Рисуване на контура на кръга
                g.DrawEllipse(pen, GetBounds());
            }
        }

        /// <summary>
        /// Проверява дали точката е в кръга
        /// </summary>
        /// <param name="point">Точка за проверка</param>
        /// <returns>True ако точката е в кръга</returns>
        public override bool Contains(Point point)
        {
            // Изчисляване на разстоянието от центъра до точката
            int dx = point.X - (Position.X + Radius);
            int dy = point.Y - (Position.Y + Radius);
            int distanceSquared = dx * dx + dy * dy;
            int radiusSquared = Radius * Radius;

            return distanceSquared <= radiusSquared;
        }
    }
}
