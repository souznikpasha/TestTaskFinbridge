using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Runtime;
using Microsoft.Extensions.Caching.Memory;
using TestTask;

[Route("api/[controller]")]
[ApiController]
public class CalculateController : ControllerBase
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
    [HttpPost]
    [Route("calculate")]
    public async Task<IActionResult> CalculateSumOfSquares([FromBody] CalculationRequest request)
    {
        if (IsArgumentCountExceeded(request.Values.Count))
        {
            return BadRequest("Превышено максимальное количество аргументов.");
        }

        if (IsAnyArgumentOutOfRange(request.Values))
        {
            return BadRequest("Превышено максимальное или минимальное значение аргумента.");
        }

        await CalculateSquaresAsync(request.Values);

        double sumOfSquares = squares.Sum();
        return Ok(new { Result = sumOfSquares });
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

    /// <summary>
    /// Асинхронное вычисление квадратов чисел с задержкой
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
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
                await Task.Delay(delayMilliseconds);
                square = value * value;

                // Кэшируем результат на указанное время
                _memoryCache.Set(value, square, TimeSpan.FromMinutes(10));
            }

            squares.Add(square);
        }
    }


}
