/*
 * Програма: Windows Forms Graphics Application
 * Студент: Momchil Georgiev Tsanov
 * Факултетен номер: 113172
 * 
 * Сервиз за сериализация на данни
 * Демонстрира използването на Serialization (Сериализация)
 */

using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using WindowsFormsApp.Interfaces;
using WindowsFormsApp.Models;

namespace WindowsFormsApp.Services
{
    /// <summary>
    /// Клас за сериализация на списък с форми
    /// </summary>
    public class ShapeCollection : ISerializable<ShapeCollection>
    {
        /// <summary>
        /// Списък с форми
        /// </summary>
        public List<Shape> Shapes { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ShapeCollection()
        {
            Shapes = new List<Shape>();
        }

        /// <summary>
        /// Сериализира колекцията в JSON формат
        /// </summary>
        /// <returns>JSON низ</returns>
        public string Serialize()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            return JsonSerializer.Serialize(this, options);
        }

        /// <summary>
        /// Десериализира колекция от JSON низ
        /// </summary>
        /// <param name="data">JSON низ</param>
        /// <returns>Десериализирана колекция</returns>
        public ShapeCollection Deserialize(string data)
        {
            return JsonSerializer.Deserialize<ShapeCollection>(data) ?? new ShapeCollection();
        }
    }

    /// <summary>
    /// Сервиз за работа със сериализация
    /// </summary>
    public class SerializationService
    {
        /// <summary>
        /// Запазва колекция от форми във файл
        /// </summary>
        /// <param name="shapes">Колекция от форми</param>
        /// <param name="filePath">Път до файла</param>
        public void SaveShapes(ShapeCollection shapes, string filePath)
        {
            string json = shapes.Serialize();
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// Зарежда колекция от форми от файл
        /// </summary>
        /// <param name="filePath">Път до файла</param>
        /// <returns>Колекция от форми</returns>
        public ShapeCollection LoadShapes(string filePath)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                ShapeCollection collection = new ShapeCollection();
                return collection.Deserialize(json);
            }
            return new ShapeCollection();
        }
    }
}
