/*
 * Програма: Task Manager Application
 * Студент: Momchil Georgiev Tsanov
 * Факултетен номер: 113172
 * 
 * Сервиз за управление на задачи
 * Демонстрира използването на Classes, Collections, Delegates
 */

using System;
using System.Collections.Generic;
using System.Linq;
using TaskManager.Delegates;
using TaskManager.Interfaces;
using TaskManager.Models;

namespace TaskManager.Services
{
    /// <summary>
    /// Сервиз за управление на задачи
    /// </summary>
    public class TaskManagerService
    {
        // Колекция за съхранение на задачи (Arrays & Collections)
        private List<Task> _tasks;
        private int _nextId;

        // Делегати за събития (Delegates)
        public event TaskEventHandler TaskAdded;
        public event TaskEventHandler TaskRemoved;
        public event TaskEventHandler TaskUpdated;
        public event TaskStatusChangedEventHandler TaskStatusChanged;
        public event TaskPriorityChangedEventHandler TaskPriorityChanged;

        /// <summary>
        /// Конструктор
        /// </summary>
        public TaskManagerService()
        {
            _tasks = new List<Task>();
            _nextId = 1;
        }

        /// <summary>
        /// Всички задачи
        /// </summary>
        public List<Task> GetAllTasks()
        {
            return new List<Task>(_tasks);
        }

        /// <summary>
        /// Добавя нова задача
        /// </summary>
        public void AddTask(Task task)
        {
            task.Id = _nextId++;
            _tasks.Add(task);
            TaskAdded?.Invoke(this, task);
        }

        /// <summary>
        /// Премахва задача
        /// </summary>
        public bool RemoveTask(int taskId)
        {
            Task task = _tasks.FirstOrDefault(t => t.Id == taskId);
            if (task != null)
            {
                _tasks.Remove(task);
                TaskRemoved?.Invoke(this, task);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Обновява задача
        /// </summary>
        public bool UpdateTask(Task updatedTask)
        {
            Task existingTask = _tasks.FirstOrDefault(t => t.Id == updatedTask.Id);
            if (existingTask != null)
            {
                TaskStatus oldStatus = existingTask.Status;
                TaskPriority oldPriority = existingTask.Priority;

                existingTask.Title = updatedTask.Title;
                existingTask.Description = updatedTask.Description;
                existingTask.Priority = updatedTask.Priority;
                existingTask.Status = updatedTask.Status;
                existingTask.DueDate = updatedTask.DueDate;
                existingTask.Category = updatedTask.Category;

                if (oldStatus != existingTask.Status)
                {
                    TaskStatusChanged?.Invoke(this, existingTask, oldStatus, existingTask.Status);
                }

                if (oldPriority != existingTask.Priority)
                {
                    TaskPriorityChanged?.Invoke(this, existingTask, oldPriority, existingTask.Priority);
                }

                TaskUpdated?.Invoke(this, existingTask);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Връща задача по ID
        /// </summary>
        public Task GetTaskById(int id)
        {
            return _tasks.FirstOrDefault(t => t.Id == id);
        }

        /// <summary>
        /// Филтрира задачи с помощта на филтър (използване на Interface)
        /// </summary>
        public List<Task> FilterTasks(ITaskFilter filter)
        {
            return filter.Filter(_tasks);
        }

        /// <summary>
        /// Сортира задачи с помощта на сортирач (използване на Interface)
        /// </summary>
        public List<Task> SortTasks(ITaskSorter sorter)
        {
            return sorter.Sort(_tasks);
        }

        /// <summary>
        /// Връща задачи по статус
        /// </summary>
        public List<Task> GetTasksByStatus(TaskStatus status)
        {
            return _tasks.Where(t => t.Status == status).ToList();
        }

        /// <summary>
        /// Връща задачи по приоритет
        /// </summary>
        public List<Task> GetTasksByPriority(TaskPriority priority)
        {
            return _tasks.Where(t => t.Priority == priority).ToList();
        }

        /// <summary>
        /// Връща изтеклали задачи
        /// </summary>
        public List<Task> GetOverdueTasks()
        {
            return _tasks.Where(t => t.IsOverdue()).ToList();
        }

        /// <summary>
        /// Връща статистика за задачите
        /// </summary>
        public Dictionary<TaskStatus, int> GetStatusStatistics()
        {
            return _tasks.GroupBy(t => t.Status)
                         .ToDictionary(g => g.Key, g => g.Count());
        }

        /// <summary>
        /// Връща статистика по приоритет
        /// </summary>
        public Dictionary<TaskPriority, int> GetPriorityStatistics()
        {
            return _tasks.GroupBy(t => t.Priority)
                         .ToDictionary(g => g.Key, g => g.Count());
        }

        /// <summary>
        /// Изчиства всички задачи
        /// </summary>
        public void ClearAllTasks()
        {
            _tasks.Clear();
            _nextId = 1;
        }

        /// <summary>
        /// Зарежда задачи от колекция (за сериализация)
        /// </summary>
        public void LoadTasks(List<Task> tasks)
        {
            _tasks = tasks ?? new List<Task>();
            if (_tasks.Count > 0)
            {
                _nextId = _tasks.Max(t => t.Id) + 1;
            }
            else
            {
                _nextId = 1;
            }
        }
    }
}
