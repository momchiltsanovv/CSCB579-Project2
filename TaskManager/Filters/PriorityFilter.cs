/*
 * Програма: Task Manager Application
 * Студент: Momchil Georgiev Tsanov
 * Факултетен номер: 113172
 * 
 * Филтър за филтриране по приоритет
 * Демонстрира използването на Interfaces (Интерфейси)
 */

using System.Collections.Generic;
using System.Linq;
using TaskManager.Interfaces;
using TaskManager.Models;

namespace TaskManager.Filters
{
    /// <summary>
    /// Филтър за филтриране на задачи по приоритет
    /// </summary>
    public class PriorityFilter : ITaskFilter
    {
        private TaskPriority _priority;

        /// <summary>
        /// Конструктор
        /// </summary>
        public PriorityFilter(TaskPriority priority)
        {
            _priority = priority;
        }

        /// <summary>
        /// Филтрира задачи по приоритет
        /// </summary>
        public List<Task> Filter(List<Task> tasks)
        {
            return tasks.Where(t => t.Priority == _priority).ToList();
        }
    }
}
