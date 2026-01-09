/*
 * Програма: Task Manager Application
 * Студент: Momchil Georgiev Tsanov
 * Факултетен номер: 113172
 * 
 * Делегати за обработка на събития свързани с задачи
 * Демонстрира използването на Delegates (Делегати)
 */

using TaskManager.Models;
using Task = TaskManager.Models.Task;
using TaskStatus = TaskManager.Models.TaskStatus;

namespace TaskManager.Delegates
{
    /// <summary>
    /// Делегат за събития свързани с задачи
    /// </summary>
    /// <param name="sender">Обектът, който излъчва събитието</param>
    /// <param name="task">Задачата, свързана със събитието</param>
    public delegate void TaskEventHandler(object sender, Task task);

    /// <summary>
    /// Делегат за събития при промяна на статус на задача
    /// </summary>
    /// <param name="sender">Обектът, който излъчва събитието</param>
    /// <param name="task">Задачата</param>
    /// <param name="oldStatus">Старият статус</param>
    /// <param name="newStatus">Новият статус</param>
    public delegate void TaskStatusChangedEventHandler(object sender, Task task, TaskStatus oldStatus, TaskStatus newStatus);

    /// <summary>
    /// Делегат за събития при промяна на приоритет
    /// </summary>
    /// <param name="sender">Обектът, който излъчва събитието</param>
    /// <param name="task">Задачата</param>
    /// <param name="oldPriority">Старият приоритет</param>
    /// <param name="newPriority">Новият приоритет</param>
    public delegate void TaskPriorityChangedEventHandler(object sender, Task task, TaskPriority oldPriority, TaskPriority newPriority);
}
