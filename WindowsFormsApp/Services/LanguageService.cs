/*
 * Програма: Windows Forms Graphics Application
 * Студент: Momchil Georgiev Tsanov
 * Факултетен номер: 113172
 * 
 * Сервиз за управление на многоезичен интерфейс
 * Демонстрира използването на Multilingual User Interface (Многоезичен интерфейс)
 */

using System.Collections.Generic;
using System.Globalization;
using System.Resources;

namespace WindowsFormsApp.Services
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
        /// Конструктор за LanguageService
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
                // Менюта
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
                    [Language.Bulgarian] = "Нов",
                    [Language.English] = "New"
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
                ["MenuItemClear"] = new Dictionary<Language, string>
                {
                    [Language.Bulgarian] = "Изчисти",
                    [Language.English] = "Clear"
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
                // Други текстове
                ["Title"] = new Dictionary<Language, string>
                {
                    [Language.Bulgarian] = "Графично Приложение",
                    [Language.English] = "Graphics Application"
                },
                ["StatusReady"] = new Dictionary<Language, string>
                {
                    [Language.Bulgarian] = "Готов",
                    [Language.English] = "Ready"
                },
                ["StatusDrawing"] = new Dictionary<Language, string>
                {
                    [Language.Bulgarian] = "Рисуване...",
                    [Language.English] = "Drawing..."
                }
            };
        }

        /// <summary>
        /// Връща превод на даден ключ за текущия език
        /// </summary>
        /// <param name="key">Ключ за превод</param>
        /// <returns>Преведен текст</returns>
        public string GetString(string key)
        {
            if (_translations.ContainsKey(key) && 
                _translations[key].ContainsKey(_currentLanguage))
            {
                return _translations[key][_currentLanguage];
            }
            return key; // Връща ключа, ако няма превод
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
