namespace Task4_5.Model
{
    public interface ICalculateService
    {
        Task<double> CalculateSumOfSquares(List<double> values);
    }
}
