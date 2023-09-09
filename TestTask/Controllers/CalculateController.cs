using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Runtime;
using TestTask;

[Route("api/[controller]")]
[ApiController]
public class CalculateController : ControllerBase
{
    private readonly AppSettings _appSettings;
    private List<double> squares;

    public CalculateController(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings.Value;
        squares = new List<double>();
    }
    [HttpPost]
    [Route("calculate")]
    public async Task<IActionResult> CalculateSumOfSquares([FromBody] CalculationRequest request)
    {
        if (IsArgumentCountExceeded(request.Values.Count))
        {
            return BadRequest("ѕревышено максимальное количество аргументов.");
        }

        if (IsAnyArgumentOutOfRange(request.Values))
        {
            return BadRequest("ѕревышено максимальное или минимальное значение аргумента.");
        }

        await CalculateSquaresAsync(request.Values);

        double sumOfSquares = squares.Sum();
        return Ok(new { Result = sumOfSquares });
    }

    /// <summary>
    /// ѕроверка на превышение максимального количества аргументов
    /// </summary>
    /// <param name="argumentCount"></param>
    /// <returns></returns>
    private bool IsArgumentCountExceeded(int argumentCount)
    {
        return argumentCount > _appSettings.MaxArgumentCount;
    }

    /// <summary>
    /// ѕроверка на превышение максимального или минимального значени€ аргумента
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    private bool IsAnyArgumentOutOfRange(List<double> values)
    {
        return values.Any(value => Math.Abs(value) > _appSettings.MaxArgumentValue || Math.Abs(value) < _appSettings.MinArgumentValue);
    }

    /// <summary>
    /// јсинхронное вычисление квадратов чисел с задержкой
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    private async Task CalculateSquaresAsync(List<double> values)
    {
        foreach (var value in values)
        {
            int delayMilliseconds = new Random().Next(_appSettings.DelayMinMilliseconds, _appSettings.DelayMaxMilliseconds);
            await Task.Delay(delayMilliseconds);
            double square = value * value;
            squares.Add(square);
        }
    }

}
