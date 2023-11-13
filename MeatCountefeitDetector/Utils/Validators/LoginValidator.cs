using System;
using System.Globalization;
using System.Text.RegularExpressions;
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
                return new ValidationResult(false, "Логин не может быть пустым");
            }
            //if (((string)value).Length < 6)
            //{
            //    return new ValidationResult(false, $"Логин должен быть длинее 6 символов");
            //}
            if(!Regex.IsMatch(((string)value), @"^[a-zA-Z0-9-._@+]+$"))
            {
                return new ValidationResult(false, $"Логин может содержать только следующие символы:\n \"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+\"");
            }
        }
        catch (Exception e)
        {
            return new ValidationResult(false, $"Введено недопустимое значение. {e.Message}");
        }

        return ValidationResult.ValidResult;
    }
}

