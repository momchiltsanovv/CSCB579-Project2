/*
 * Програма: Task Manager Application
 * Студент: Momchil Georgiev Tsanov
 * Факултетен номер: 113172
 * 
 * Интерфейс за филтриране на задачи
 * Демонстрира използването на Interfaces (Интерфейси)
 */

using System.Collections.Generic;
using TaskManager.Models;
using Task = TaskManager.Models.Task;

namespace TaskManager.Interfaces
{
    /// <summary>
    /// Интерфейс за филтриране на задачи
    /// </summary>
    public interface ITaskFilter
    {
        /// <summary>
        /// Филтрира списък от задачи според критериите
        /// </summary>
        /// <param name="tasks">Списък с задачи за филтриране</param>
        /// <returns>Филтриран списък с задачи</returns>
        List<Task> Filter(List<Task> tasks);
    }
}
