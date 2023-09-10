namespace BlazorApp1.Model
{
    public interface ICalculateService
    {
        Task<double> CalculateSumOfSquares(List<double> values);
    }
}
