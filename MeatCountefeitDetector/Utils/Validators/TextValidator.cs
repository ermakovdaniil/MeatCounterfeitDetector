using System;
using System.Globalization;
using System.Windows.Controls;


namespace MeatCounterfeitDetector.Utils.Validators;

/// <summary>
///     Валидатор для логина
/// </summary>
internal class LoginValidator : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        try
        {
            if (value == null)
            {
                return new ValidationResult(false, "Поле не может быть пустым");
            }
            if (((string)value).Length < 6)
            {
                return new ValidationResult(false, $"Поле должно быть");
            }
        }
        catch (Exception e)
        {
            return new ValidationResult(false, $"Введено недопустимое значение. {e.Message}");
        }


        return ValidationResult.ValidResult;
    }
}

