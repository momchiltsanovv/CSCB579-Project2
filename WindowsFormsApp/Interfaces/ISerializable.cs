/*
 * Програма: Windows Forms Graphics Application
 * Студент: Momchil Georgiev Tsanov
 * Факултетен номер: 113172
 * 
 * Интерфейс за сериализация на данни
 * Демонстрира използването на Serialization (Сериализация)
 */

namespace WindowsFormsApp.Interfaces
{
    /// <summary>
    /// Интерфейс за обекти, които могат да бъдат сериализирани
    /// </summary>
    /// <typeparam name="T">Тип на данните за сериализация</typeparam>
    public interface ISerializable<T>
    {
        /// <summary>
        /// Сериализира обекта в низ
        /// </summary>
        /// <returns>Сериализиран низ</returns>
        string Serialize();

        /// <summary>
        /// Десериализира обект от низ
        /// </summary>
        /// <param name="data">Сериализиран низ</param>
        /// <returns>Десериализиран обект</returns>
        T Deserialize(string data);
    }
}
