/*
 * Програма: Task Manager Application
 * Студент: Momchil Georgiev Tsanov
 * Факултетен номер: 113172
 * 
 * Филтър за филтриране по статус
 * Демонстрира използването на Interfaces (Интерфейси)
 */

using System.Collections.Generic;
using System.Linq;
using TaskManager.Interfaces;
using TaskManager.Models;

namespace TaskManager.Filters
{
    /// <summary>
    /// Филтър за филтриране на задачи по статус
    /// </summary>
    public class StatusFilter : ITaskFilter
    {
        private TaskStatus _status;

        /// <summary>
        /// Конструктор
        /// </summary>
        public StatusFilter(TaskStatus status)
        {
            _status = status;
        }

        /// <summary>
        /// Филтрира задачи по статус
        /// </summary>
        public List<Task> Filter(List<Task> tasks)
        {
            return tasks.Where(t => t.Status == _status).ToList();
        }
    }
}
