/*
 * Програма: Task Manager Application
 * Студент: Momchil Georgiev Tsanov
 * Факултетен номер: 113172
 * 
 * Сервиз за сериализация на задачи
 * Демонстрира използването на Serialization (Сериализация)
 */

using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using TaskManager.Models;

namespace TaskManager.Services
{
    /// <summary>
    /// Клас за сериализация на колекция от задачи
    /// </summary>
    public class TaskCollection
    {
        /// <summary>
        /// Списък с задачи
        /// </summary>
        public List<Task> Tasks { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public TaskCollection()
        {
            Tasks = new List<Task>();
        }
    }

    /// <summary>
    /// Сервиз за работа със сериализация
    /// </summary>
    public class SerializationService
    {
        /// <summary>
        /// Запазва колекция от задачи във файл
        /// </summary>
        public void SaveTasks(List<Task> tasks, string filePath)
        {
            var collection = new TaskCollection { Tasks = tasks };
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(collection, options);
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// Зарежда колекция от задачи от файл
        /// </summary>
        public List<Task> LoadTasks(string filePath)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                var collection = JsonSerializer.Deserialize<TaskCollection>(json);
                return collection?.Tasks ?? new List<Task>();
            }
            return new List<Task>();
        }
    }
}
