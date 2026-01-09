/*
 * Програма: Windows Forms Graphics Application
 * Студент: Momchil Georgiev Tsanov
 * Факултетен номер: 113172
 * 
 * Интерфейс за обекти, които могат да бъдат рисувани
 * Демонстрира използването на Interfaces (Интерфейси)
 */

using System.Drawing;

namespace WindowsFormsApp.Interfaces
{
    /// <summary>
    /// Интерфейс за обекти, които могат да бъдат рисувани върху Graphics обект
    /// </summary>
    public interface IDrawable
    {
        /// <summary>
        /// Рисува обекта върху дадения Graphics обект
        /// </summary>
        /// <param name="g">Graphics обект за рисуване</param>
        void Draw(Graphics g);

        /// <summary>
        /// Проверява дали точката е в границите на обекта
        /// </summary>
        /// <param name="point">Точка за проверка</param>
        /// <returns>True ако точката е в границите</returns>
        bool Contains(Point point);
    }
}
