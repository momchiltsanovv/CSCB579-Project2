/*
 * Програма: Task Manager Application
 * Студент: Momchil Georgiev Tsanov
 * Факултетен номер: 113172
 * 
 * Сервиз за управление на многоезичен интерфейс
 * Демонстрира използването на Multilingual User Interface
 */

using System.Collections.Generic;

namespace TaskManager.Services
{
    /// <summary>
    /// Енумерация за поддържаните езици
    /// </summary>
    public enum Language
    {
        Bulgarian,
        English
    }

    /// <summary>
    /// Сервиз за управление на езиците в приложението
    /// </summary>
    public class LanguageService
    {
        private Language _currentLanguage;
        private Dictionary<string, Dictionary<Language, string>> _translations;

        /// <summary>
        /// Събитие, което се излъчва при промяна на езика
        /// </summary>
        public event System.EventHandler LanguageChanged;

        /// <summary>
        /// Текущ избран език
        /// </summary>
        public Language CurrentLanguage
        {
            get => _currentLanguage;
            set
            {
                if (_currentLanguage != value)
                {
                    _currentLanguage = value;
                    OnLanguageChanged();
                }
            }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public LanguageService()
        {
            _currentLanguage = Language.Bulgarian;
            InitializeTranslations();
        }

        /// <summary>
        /// Инициализира всички преводи
        /// </summary>
        private void InitializeTranslations()
        {
            _translations = new Dictionary<string, Dictionary<Language, string>>
            {
                ["MenuFile"] = new Dictionary<Language, string>
                {
                    [Language.Bulgarian] = "Файл",
                    [Language.English] = "File"
                },
                ["MenuEdit"] = new Dictionary<Language, string>
                {
                    [Language.Bulgarian] = "Редактиране",
                    [Language.English] = "Edit"
                },
                ["MenuView"] = new Dictionary<Language, string>
                {
                    [Language.Bulgarian] = "Изглед",
                    [Language.English] = "View"
                },
                ["MenuLanguage"] = new Dictionary<Language, string>
                {
                    [Language.Bulgarian] = "Език",
                    [Language.English] = "Language"
                },
                ["MenuHelp"] = new Dictionary<Language, string>
                {
                    [Language.Bulgarian] = "Помощ",
                    [Language.English] = "Help"
                },
                ["MenuItemNew"] = new Dictionary<Language, string>
                {
                    [Language.Bulgarian] = "Нова задача",
                    [Language.English] = "New Task"
                },
                ["MenuItemOpen"] = new Dictionary<Language, string>
                {
                    [Language.Bulgarian] = "Отвори",
                    [Language.English] = "Open"
                },
                ["MenuItemSave"] = new Dictionary<Language, string>
                {
                    [Language.Bulgarian] = "Запази",
                    [Language.English] = "Save"
                },
                ["MenuItemExit"] = new Dictionary<Language, string>
                {
                    [Language.Bulgarian] = "Изход",
                    [Language.English] = "Exit"
                },
                ["MenuItemBulgarian"] = new Dictionary<Language, string>
                {
                    [Language.Bulgarian] = "Български",
                    [Language.English] = "Bulgarian"
                },
                ["MenuItemEnglish"] = new Dictionary<Language, string>
                {
                    [Language.Bulgarian] = "Английски",
                    [Language.English] = "English"
                },
                ["MenuItemAbout"] = new Dictionary<Language, string>
                {
                    [Language.Bulgarian] = "За програмата",
                    [Language.English] = "About"
                },
                ["Title"] = new Dictionary<Language, string>
                {
                    [Language.Bulgarian] = "Task Manager - Управление на Задачи",
                    [Language.English] = "Task Manager"
                },
                ["StatusReady"] = new Dictionary<Language, string>
                {
                    [Language.Bulgarian] = "Готов",
                    [Language.English] = "Ready"
                },
                ["TitleLabel"] = new Dictionary<Language, string>
                {
                    [Language.Bulgarian] = "Заглавие:",
                    [Language.English] = "Title:"
                },
                ["DescriptionLabel"] = new Dictionary<Language, string>
                {
                    [Language.Bulgarian] = "Описание:",
                    [Language.English] = "Description:"
                },
                ["PriorityLabel"] = new Dictionary<Language, string>
                {
                    [Language.Bulgarian] = "Приоритет:",
                    [Language.English] = "Priority:"
                },
                ["StatusLabel"] = new Dictionary<Language, string>
                {
                    [Language.Bulgarian] = "Статус:",
                    [Language.English] = "Status:"
                },
                ["DueDateLabel"] = new Dictionary<Language, string>
                {
                    [Language.Bulgarian] = "Крайна дата:",
                    [Language.English] = "Due Date:"
                },
                ["CategoryLabel"] = new Dictionary<Language, string>
                {
                    [Language.Bulgarian] = "Категория:",
                    [Language.English] = "Category:"
                },
                ["AddButton"] = new Dictionary<Language, string>
                {
                    [Language.Bulgarian] = "Добави",
                    [Language.English] = "Add"
                },
                ["UpdateButton"] = new Dictionary<Language, string>
                {
                    [Language.Bulgarian] = "Обнови",
                    [Language.English] = "Update"
                },
                ["DeleteButton"] = new Dictionary<Language, string>
                {
                    [Language.Bulgarian] = "Изтрий",
                    [Language.English] = "Delete"
                },
                ["StatisticsLabel"] = new Dictionary<Language, string>
                {
                    [Language.Bulgarian] = "Статистика",
                    [Language.English] = "Statistics"
                }
            };
        }

        /// <summary>
        /// Връща превод на даден ключ за текущия език
        /// </summary>
        public string GetString(string key)
        {
            if (_translations.ContainsKey(key) && 
                _translations[key].ContainsKey(_currentLanguage))
            {
                return _translations[key][_currentLanguage];
            }
            return key;
        }

        /// <summary>
        /// Излъчва събитието за промяна на езика
        /// </summary>
        protected virtual void OnLanguageChanged()
        {
            LanguageChanged?.Invoke(this, System.EventArgs.Empty);
        }
    }
}
