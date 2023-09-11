using System.ComponentModel.DataAnnotations;

namespace BlazorApp1.Model
{
    public class CalculatorModel
    {
        // Строка для ввода чисел
        [Required(ErrorMessage = "Введите числа")]
        public string InputNumbers { get; set; }

        // Результат вычислений, отображаемый пользователю
        public string Result { get; set; }
    }
}
