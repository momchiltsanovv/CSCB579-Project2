/*
 * Програма: Windows Forms Graphics Application
 * Студент: Momchil Georgiev Tsanov
 * Факултетен номер: 113172
 * 
 * Клас за анимирана форма
 * Демонстрира динамика в изображенията и използването на Classes (Класове)
 */

using System;
using System.Drawing;
using WindowsFormsApp.Models;

namespace WindowsFormsApp.Models
{
    /// <summary>
    /// Клас за анимирана форма с движение
    /// </summary>
    public class AnimatedShape : Shape
    {
        /// <summary>
        /// Скорост на движение по X оста
        /// </summary>
        public float VelocityX { get; set; }

        /// <summary>
        /// Скорост на движение по Y оста
        /// </summary>
        public float VelocityY { get; set; }

        /// <summary>
        /// Граници на движение (правоъгълник)
        /// </summary>
        public Rectangle Bounds { get; set; }

        /// <summary>
        /// Конструктор за създаване на анимирана форма
        /// </summary>
        /// <param name="position">Начална позиция</param>
        /// <param name="size">Размер</param>
        /// <param name="fillColor">Цвят на запълването</param>
        /// <param name="borderColor">Цвят на контура</param>
        /// <param name="velocityX">Скорост по X</param>
        /// <param name="velocityY">Скорост по Y</param>
        /// <param name="bounds">Граници на движение</param>
        public AnimatedShape(Point position, Size size, Color fillColor, Color borderColor,
            float velocityX, float velocityY, Rectangle bounds)
            : base(position, size, fillColor, borderColor)
        {
            VelocityX = velocityX;
            VelocityY = velocityY;
            Bounds = bounds;
        }

        /// <summary>
        /// Обновява позицията на формата според скоростта
        /// </summary>
        public void Update()
        {
            // Изчисляване на новата позиция
            float newX = Position.X + VelocityX;
            float newY = Position.Y + VelocityY;

            // Проверка за сблъсък с границите и отразяване
            if (newX <= Bounds.Left || newX + Size.Width >= Bounds.Right)
            {
                VelocityX = -VelocityX; // Обръщане на посоката
                newX = System.Math.Max(Bounds.Left, System.Math.Min(newX, Bounds.Right - Size.Width));
            }

            if (newY <= Bounds.Top || newY + Size.Height >= Bounds.Bottom)
            {
                VelocityY = -VelocityY; // Обръщане на посоката
                newY = System.Math.Max(Bounds.Top, System.Math.Min(newY, Bounds.Bottom - Size.Height));
            }

            // Прилагане на новата позиция
            Position = new Point((int)newX, (int)newY);
        }

        /// <summary>
        /// Рисува анимираната форма като кръг
        /// </summary>
        /// <param name="g">Graphics обект за рисуване</param>
        public override void Draw(Graphics g)
        {
            // Рисуване на кръг (използваме кръг за анимацията)
            using (SolidBrush brush = new SolidBrush(FillColor))
            {
                g.FillEllipse(brush, GetBounds());
            }

            using (Pen pen = new Pen(BorderColor, BorderWidth))
            {
                g.DrawEllipse(pen, GetBounds());
            }

            // Добавяне на визуален индикатор за движение
            using (Pen pen = new Pen(Color.Red, 2))
            {
                // Рисуване на стрелка, показваща посоката на движение
                Point center = new Point(Position.X + Size.Width / 2, Position.Y + Size.Height / 2);
                Point end = new Point(
                    center.X + (int)(VelocityX * 15),
                    center.Y + (int)(VelocityY * 15)
                );
                g.DrawLine(pen, center, end);
                
                // Рисуване на малка стрелка в края на линията
                float angle = (float)Math.Atan2(VelocityY, VelocityX);
                PointF arrowPoint1 = new PointF(
                    end.X - 5 * (float)Math.Cos(angle - Math.PI / 6),
                    end.Y - 5 * (float)Math.Sin(angle - Math.PI / 6)
                );
                PointF arrowPoint2 = new PointF(
                    end.X - 5 * (float)Math.Cos(angle + Math.PI / 6),
                    end.Y - 5 * (float)Math.Sin(angle + Math.PI / 6)
                );
                g.DrawLine(pen, end, arrowPoint1);
                g.DrawLine(pen, end, arrowPoint2);
            }
        }
    }
}
