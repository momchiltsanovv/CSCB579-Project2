/*
 * Програма: Task Manager Application
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
 */

using System;
using System.Windows.Forms;

namespace TaskManager
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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
