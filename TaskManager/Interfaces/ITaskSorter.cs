/*
 * Програма: Task Manager Application
 * Студент: Momchil Georgiev Tsanov
 * Факултетен номер: 113172
 * 
 * Интерфейс за сортиране на задачи
 * Демонстрира използването на Interfaces (Интерфейси)
 */

using System.Collections.Generic;
using TaskManager.Models;
using Task = TaskManager.Models.Task;

namespace TaskManager.Interfaces
{
    /// <summary>
    /// Интерфейс за сортиране на задачи
    /// </summary>
    public interface ITaskSorter
    {
        /// <summary>
        /// Сортира списък от задачи
        /// </summary>
        /// <param name="tasks">Списък с задачи за сортиране</param>
        /// <returns>Сортиран списък с задачи</returns>
        List<Task> Sort(List<Task> tasks);
    }
}
