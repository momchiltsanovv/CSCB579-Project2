/*
 * Програма: Windows Forms Graphics Application
 * Студент: Momchil Georgiev Tsanov
 * Факултетен номер: 113172
 * 
 * Делегати за обработка на графични събития
 * Демонстрира използването на Delegates (Делегати)
 */

using System.Drawing;

namespace WindowsFormsApp.Delegates
{
    /// <summary>
    /// Делегат за събития, свързани с рисуване
    /// </summary>
    /// <param name="sender">Обектът, който излъчва събитието</param>
    /// <param name="graphics">Graphics обект за рисуване</param>
    public delegate void GraphicsEventHandler(object sender, Graphics graphics);

    /// <summary>
    /// Делегат за събития при кликване върху форма
    /// </summary>
    /// <param name="sender">Обектът, който излъчва събитието</param>
    /// <param name="point">Точката, където е кликнато</param>
    public delegate void ShapeClickEventHandler(object sender, Point point);

    /// <summary>
    /// Делегат за събития при промяна на цвят
    /// </summary>
    /// <param name="sender">Обектът, който излъчва събитието</param>
    /// <param name="color">Новият цвят</param>
    public delegate void ColorChangedEventHandler(object sender, Color color);
}
