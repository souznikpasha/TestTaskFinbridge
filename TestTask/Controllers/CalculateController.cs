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
            return BadRequest("��������� ������������ ���������� ����������.");
        }

        if (IsAnyArgumentOutOfRange(request.Values))
        {
            return BadRequest("��������� ������������ ��� ����������� �������� ���������.");
        }

        await CalculateSquaresAsync(request.Values);

        double sumOfSquares = squares.Sum();
        return Ok(new { Result = sumOfSquares });
    }

    /// <summary>
    /// �������� �� ���������� ������������� ���������� ����������
    /// </summary>
    /// <param name="argumentCount"></param>
    /// <returns></returns>
    private bool IsArgumentCountExceeded(int argumentCount)
    {
        return argumentCount > _appSettings.MaxArgumentCount;
    }

    /// <summary>
    /// �������� �� ���������� ������������� ��� ������������ �������� ���������
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    private bool IsAnyArgumentOutOfRange(List<double> values)
    {
        return values.Any(value => Math.Abs(value) > _appSettings.MaxArgumentValue || Math.Abs(value) < _appSettings.MinArgumentValue);
    }

    /// <summary>
    /// ����������� ���������� ��������� ����� � ���������
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    private async Task CalculateSquaresAsync(List<double> values)
    {
        foreach (var value in values)
        {
            double square;

            // ������� �������� ��������� �� ����
            if (!_memoryCache.TryGetValue(value, out square))
            {
                // ���� ��������� �� ������ � ����, ��������� ���
                int delayMilliseconds = new Random().Next(_appSettings.DelayMinMilliseconds, _appSettings.DelayMaxMilliseconds);
                await Task.Delay(delayMilliseconds);
                square = value * value;

                // �������� ��������� �� ��������� �����
                _memoryCache.Set(value, square, TimeSpan.FromMinutes(10));
            }

            squares.Add(square);
        }
    }


}
