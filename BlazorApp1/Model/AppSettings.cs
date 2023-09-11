using Microsoft.Extensions.Options;

namespace Task4_5.Model
{
    public class AppSettings
    {
        // Максимальное допустимое количество аргументов
        public int MaxArgumentCount { get; set; }

        // Минимальное допустимое значение аргумента
        public double MinArgumentValue { get; set; }

        // Максимальное допустимое значение аргумента
        public double MaxArgumentValue { get; set; }

        // Минимальная задержка в миллисекундах для вычислений
        public int DelayMinMilliseconds { get; set; }

        // Максимальная задержка в миллисекундах для вычислений
        public int DelayMaxMilliseconds { get; set; }
    }
}
