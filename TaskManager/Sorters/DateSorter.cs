/*
 * Програма: Task Manager Application
 * Студент: Momchil Georgiev Tsanov
 * Факултетен номер: 113172
 * 
 * Сортирач за сортиране по дата
 * Демонстрира използването на Interfaces (Интерфейси)
 */

using System.Collections.Generic;
using System.Linq;
using TaskManager.Interfaces;
using TaskManager.Models;

namespace TaskManager.Sorters
{
    /// <summary>
    /// Сортирач за сортиране на задачи по дата на създаване
    /// </summary>
    public class DateSorter : ITaskSorter
    {
        private bool _ascending;

        /// <summary>
        /// Конструктор
        /// </summary>
        public DateSorter(bool ascending = true)
        {
            _ascending = ascending;
        }

        /// <summary>
        /// Сортира задачи по дата
        /// </summary>
        public List<Task> Sort(List<Task> tasks)
        {
            if (_ascending)
            {
                return tasks.OrderBy(t => t.CreatedDate).ToList();
            }
            else
            {
                return tasks.OrderByDescending(t => t.CreatedDate).ToList();
            }
        }
    }
}
