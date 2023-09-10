namespace BlazorApp1.Data
{
    public interface ICalculateService
    {
        Task<double> CalculateSumOfSquares(List<double> values);
    }
}
