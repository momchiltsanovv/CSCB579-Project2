/*
 * Програма: Task Manager Application
 * Студент: Momchil Georgiev Tsanov
 * Факултетен номер: 113172
 * 
 * Сортирач за сортиране по приоритет
 * Демонстрира използването на Interfaces (Интерфейси)
 */

using System.Collections.Generic;
using System.Linq;
using TaskManager.Interfaces;
using TaskManager.Models;

namespace TaskManager.Sorters
{
    /// <summary>
    /// Сортирач за сортиране на задачи по приоритет
    /// </summary>
    public class PrioritySorter : ITaskSorter
    {
        /// <summary>
        /// Сортира задачи по приоритет (от критичен към нисък)
        /// </summary>
        public List<Task> Sort(List<Task> tasks)
        {
            return tasks.OrderByDescending(t => t.Priority).ToList();
        }
    }
}
