/*
 * Програма: Task Manager Application
 * Студент: Momchil Georgiev Tsanov
 * Факултетен номер: 113172
 * 
 * Клас за представяне на задача
 * Демонстрира използването на Classes (Класове)
 */

using System;

namespace TaskManager.Models
{
    /// <summary>
    /// Енумерация за приоритет на задачата
    /// </summary>
    public enum TaskPriority
    {
        Low,        // Нисък
        Medium,     // Среден
        High,       // Висок
        Critical    // Критичен
    }

    /// <summary>
    /// Енумерация за статус на задачата
    /// </summary>
    public enum TaskStatus
    {
        Pending,    // Изчакваща
        InProgress, // В процес
        Completed,  // Завършена
        Cancelled   // Отменена
    }

    /// <summary>
    /// Клас за представяне на задача
    /// </summary>
    public class Task
    {
        /// <summary>
        /// Уникален идентификатор на задачата
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Заглавие на задачата
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Описание на задачата
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Приоритет на задачата
        /// </summary>
        public TaskPriority Priority { get; set; }

        /// <summary>
        /// Статус на задачата
        /// </summary>
        public TaskStatus Status { get; set; }

        /// <summary>
        /// Дата на създаване
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Крайна дата (deadline)
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Дата на завършване
        /// </summary>
        public DateTime? CompletedDate { get; set; }

        /// <summary>
        /// Категория на задачата
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Конструктор по подразбиране
        /// </summary>
        public Task()
        {
            Id = 0;
            Title = string.Empty;
            Description = string.Empty;
            Priority = TaskPriority.Medium;
            Status = TaskStatus.Pending;
            CreatedDate = DateTime.Now;
            DueDate = null;
            CompletedDate = null;
            Category = "General";
        }

        /// <summary>
        /// Конструктор с параметри
        /// </summary>
        public Task(string title, string description, TaskPriority priority, DateTime? dueDate = null, string category = "General")
        {
            Title = title;
            Description = description;
            Priority = priority;
            Status = TaskStatus.Pending;
            CreatedDate = DateTime.Now;
            DueDate = dueDate;
            CompletedDate = null;
            Category = category;
        }

        /// <summary>
        /// Проверява дали задачата е изтекла
        /// </summary>
        public bool IsOverdue()
        {
            return DueDate.HasValue && 
                   DueDate.Value < DateTime.Now && 
                   Status != TaskStatus.Completed;
        }

        /// <summary>
        /// Маркира задачата като завършена
        /// </summary>
        public void MarkAsCompleted()
        {
            Status = TaskStatus.Completed;
            CompletedDate = DateTime.Now;
        }

        /// <summary>
        /// Връща цвят според приоритета
        /// </summary>
        public System.Drawing.Color GetPriorityColor()
        {
            return Priority switch
            {
                TaskPriority.Low => System.Drawing.Color.Green,
                TaskPriority.Medium => System.Drawing.Color.Orange,
                TaskPriority.High => System.Drawing.Color.Red,
                TaskPriority.Critical => System.Drawing.Color.DarkRed,
                _ => System.Drawing.Color.Black
            };
        }

        /// <summary>
        /// Връща низово представяне на задачата
        /// </summary>
        public override string ToString()
        {
            return $"{Title} - {Status} ({Priority})";
        }
    }
}
