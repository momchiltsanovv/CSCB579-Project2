/*
 * Програма: Windows Forms Graphics Application
 * Студент: Momchil Georgiev Tsanov
 * Факултетен номер: 113172
 * Курс: Втора година
 * 
 * Това приложение демонстрира знания от втората година на курса:
 * - Windows Forms
 * - Classes (Класове)
 * - Arrays & Collections (Масиви и колекции)
 * - Interfaces (Интерфейси)
 * - Delegates (Делегати)
 * - Serialization (Сериализация)
 * - Multilingual User Interface (Многоезичен интерфейс)
 * - Graphics with C# (Графика с C#)
 * - Graphics II (Графика II)
 * - Drawing3 (Рисуване 3)
 */

using System;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    /// <summary>
    /// Главна точка на влизане в приложението
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Главна точка на влизане за приложението
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Включване на визуални стилове за Windows Forms
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Стартиране на главната форма
            Application.Run(new MainForm());
        }
    }
}
