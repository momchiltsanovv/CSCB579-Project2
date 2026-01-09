/*
 * Програма: Windows Forms Graphics Application
 * Студент: Momchil Georgiev Tsanov
 * Факултетен номер: 113172
 * 
 * Сервиз за стилизиране на изображения
 * Демонстрира използването на Graphics за обработка на изображения
 */

using System.Drawing;
using System.Drawing.Imaging;

namespace WindowsFormsApp.Services
{
    /// <summary>
    /// Сервиз за стилизиране и обработка на изображения
    /// </summary>
    public class ImageStylingService
    {
        /// <summary>
        /// Прилага градиентен ефект върху изображение
        /// </summary>
        /// <param name="image">Изходно изображение</param>
        /// <param name="gradientColor">Цвят на градиента</param>
        /// <returns>Стилизирано изображение</returns>
        public Bitmap ApplyGradientEffect(Bitmap image, Color gradientColor)
        {
            Bitmap result = new Bitmap(image.Width, image.Height);
            
            using (Graphics g = Graphics.FromImage(result))
            {
                // Рисуване на оригиналното изображение
                g.DrawImage(image, 0, 0);
                
                // Прилагане на градиентен овърлей
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    new Point(0, 0),
                    new Point(image.Width, image.Height),
                    Color.FromArgb(100, gradientColor),
                    Color.FromArgb(50, gradientColor)))
                {
                    g.FillRectangle(brush, 0, 0, image.Width, image.Height);
                }
            }
            
            return result;
        }

        /// <summary>
        /// Прилага черно-бял филтър върху изображение
        /// </summary>
        /// <param name="image">Изходно изображение</param>
        /// <returns>Черно-бяло изображение</returns>
        public Bitmap ApplyBlackAndWhiteFilter(Bitmap image)
        {
            Bitmap result = new Bitmap(image.Width, image.Height);
            
            // Използване на ColorMatrix за преобразуване в черно-бяло
            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
            {
                new float[] {0.299f, 0.299f, 0.299f, 0, 0},
                new float[] {0.587f, 0.587f, 0.587f, 0, 0},
                new float[] {0.114f, 0.114f, 0.114f, 0, 0},
                new float[] {0, 0, 0, 1, 0},
                new float[] {0, 0, 0, 0, 1}
            });
            
            using (Graphics g = Graphics.FromImage(result))
            {
                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(colorMatrix);
                
                g.DrawImage(image,
                    new Rectangle(0, 0, image.Width, image.Height),
                    0, 0, image.Width, image.Height,
                    GraphicsUnit.Pixel,
                    attributes);
            }
            
            return result;
        }

        /// <summary>
        /// Прилага сепия филтър върху изображение
        /// </summary>
        /// <param name="image">Изходно изображение</param>
        /// <returns>Изображение със сепия ефект</returns>
        public Bitmap ApplySepiaFilter(Bitmap image)
        {
            Bitmap result = new Bitmap(image.Width, image.Height);
            
            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
            {
                new float[] {0.393f, 0.349f, 0.272f, 0, 0},
                new float[] {0.769f, 0.686f, 0.534f, 0, 0},
                new float[] {0.189f, 0.168f, 0.131f, 0, 0},
                new float[] {0, 0, 0, 1, 0},
                new float[] {0, 0, 0, 0, 1}
            });
            
            using (Graphics g = Graphics.FromImage(result))
            {
                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(colorMatrix);
                
                g.DrawImage(image,
                    new Rectangle(0, 0, image.Width, image.Height),
                    0, 0, image.Width, image.Height,
                    GraphicsUnit.Pixel,
                    attributes);
            }
            
            return result;
        }

        /// <summary>
        /// Добавя рамка около изображение
        /// </summary>
        /// <param name="image">Изходно изображение</param>
        /// <param name="borderWidth">Дебелина на рамката</param>
        /// <param name="borderColor">Цвят на рамката</param>
        /// <returns>Изображение с рамка</returns>
        public Bitmap AddBorder(Bitmap image, int borderWidth, Color borderColor)
        {
            Bitmap result = new Bitmap(
                image.Width + borderWidth * 2,
                image.Height + borderWidth * 2
            );
            
            using (Graphics g = Graphics.FromImage(result))
            {
                // Рисуване на рамката
                using (SolidBrush brush = new SolidBrush(borderColor))
                {
                    g.FillRectangle(brush, 0, 0, result.Width, result.Height);
                }
                
                // Рисуване на изображението върху рамката
                g.DrawImage(image, borderWidth, borderWidth);
            }
            
            return result;
        }

        /// <summary>
        /// Променя яркостта на изображение
        /// </summary>
        /// <param name="image">Изходно изображение</param>
        /// <param name="brightness">Стойност на яркостта (-1.0 до 1.0)</param>
        /// <returns>Изображение с променена яркост</returns>
        public Bitmap AdjustBrightness(Bitmap image, float brightness)
        {
            Bitmap result = new Bitmap(image.Width, image.Height);
            
            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
            {
                new float[] {1, 0, 0, 0, 0},
                new float[] {0, 1, 0, 0, 0},
                new float[] {0, 0, 1, 0, 0},
                new float[] {0, 0, 0, 1, 0},
                new float[] {brightness, brightness, brightness, 0, 1}
            });
            
            using (Graphics g = Graphics.FromImage(result))
            {
                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(colorMatrix);
                
                g.DrawImage(image,
                    new Rectangle(0, 0, image.Width, image.Height),
                    0, 0, image.Width, image.Height,
                    GraphicsUnit.Pixel,
                    attributes);
            }
            
            return result;
        }
    }
}
