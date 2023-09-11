using Task4_5.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Task4_5.Data
{
    public class CalculateController : ControllerBase, ICalculateService
    {

        private readonly AppSettings _appSettings;
        private List<double> squares;
        private readonly IMemoryCache _memoryCache;
        public CalculateController(IOptions<AppSettings> appSettings, IMemoryCache memoryCache)
        {
            _appSettings = appSettings.Value;
            _memoryCache = memoryCache;
            squares = new List<double>();
        }
        public async Task<double> CalculateSumOfSquares(List<double> request)
        {
            squares.Clear();
            try
            {
                if (IsArgumentCountExceeded(request.Count))
                {
                    throw new ArgumentException("Превышено максимальное количество аргументов.");
                }

                if (IsAnyArgumentOutOfRange(request))
                {
                    throw new ArgumentException("Превышено максимальное или минимальное значение аргумента.");
                }

                var sumOfSquares = await CalculateSumOfSquaresAsync(request);
                return sumOfSquares;
            }
            catch (ArgumentException ex)
            {
                // Здесь можно обработать исключение или просто выбросить его дальше
                throw;
            }
        }

        /// <summary>
        /// Проверка на превышение максимального количества аргументов
        /// </summary>
        /// <param name="argumentCount"></param>
        /// <returns></returns>
        private bool IsArgumentCountExceeded(int argumentCount)
        {
            return argumentCount > _appSettings.MaxArgumentCount;
        }

        /// <summary>
        /// Проверка на превышение максимального или минимального значения аргумента
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        private bool IsAnyArgumentOutOfRange(List<double> values)
        {
            return values.Any(value => Math.Abs(value) > _appSettings.MaxArgumentValue || Math.Abs(value) < _appSettings.MinArgumentValue);
        }

        public async Task<double> CalculateSumOfSquaresAsync(List<double> values)
        {
            // Возвращает сумму квадратов чисел
            await CalculateSquaresAsync(values);
            double sumOfSquares = squares.Sum();
            return sumOfSquares;
        }

        private async Task CalculateSquaresAsync(List<double> values)
        {
            foreach (var value in values)
            {
                double square;

                // Попытка получить результат из кэша
                if (!_memoryCache.TryGetValue(value, out square))
                {
                    // Если результат не найден в кэше, вычисляем его
                    int delayMilliseconds = new Random().Next(_appSettings.DelayMinMilliseconds, _appSettings.DelayMaxMilliseconds);

                    var startTime = DateTime.Now;
                    square = value * value;
                    var endTime = DateTime.Now;

                    // Если затраченное время меньше delayMilliseconds, ожидаем оставшееся время
                    if (endTime - startTime < TimeSpan.FromMilliseconds(delayMilliseconds))
                    {
                        var remainingDelay = TimeSpan.FromMilliseconds(delayMilliseconds) - (endTime - startTime);
                        await Task.Delay(remainingDelay);
                    }


                    // Кэшируем результат на указанное время
                    _memoryCache.Set(value, square, TimeSpan.FromMinutes(10));
                }

                squares.Add(square);
            }
        }
    }
}
