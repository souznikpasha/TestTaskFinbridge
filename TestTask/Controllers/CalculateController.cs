using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using TestTask;

[Route("api/[controller]")]
[ApiController]
public class CalculateController : ControllerBase
{
    private readonly AppSettings _appSettings;

    public CalculateController(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings.Value;
    }
    [HttpPost]
    [Route("calculate")]
    public IActionResult CalculateSumOfSquares([FromBody] CalculationRequest request)
    {
        // ѕроверка ограничений по максимальному количеству аргументов и значени€м
        int maxArgumentCount = _appSettings.maxArgumentCount;
        double minArgumentValue = _appSettings.MinArgumentValue;
        double maxArgumentValue = _appSettings.MaxArgumentValue;

        if (request.Values.Count > maxArgumentCount)
        {
            return BadRequest("ѕревышено максимальное количество аргументов.");
        }

        foreach (var value in request.Values)
        {
            if (Math.Abs(value) > maxArgumentValue && Math.Abs(value) < minArgumentValue)
            {
                return BadRequest("ѕревышено максимальное или минимальное значение аргумента.");
            }
        }

        // –асчет суммы квадратов
        double sumOfSquares = request.Values.Sum(x => x * x);

        return Ok(new { Result = sumOfSquares });
    }
}
