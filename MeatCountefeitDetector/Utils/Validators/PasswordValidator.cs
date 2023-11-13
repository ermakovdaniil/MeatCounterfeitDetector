using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;


namespace MeatCounterfeitDetector.Utils.Validators;

/// <summary>
///     Валидатор для пароля
/// </summary>
internal class PasswordValidator : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        try
        {
            if (value == null)
            {
                return new ValidationResult(false, "Пароль не может быть пустым");
            }
            if (((string)value).Length < 8)
            {
                return new ValidationResult(false, $"Пароль должен быть не меньше 8 символов");
            }
            if (!Regex.IsMatch(((string)value), @"^[a-zA-Z0-9-._@+]+$"))
            {
                return new ValidationResult(false, $"Пароль может содержать только следующие символы:\n \"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 -._@+\"");
            }
            if (((string)value).Contains("abcdefghijklmnopqrstuvwxyz"))
            {
                return new ValidationResult(false, $"Пароль должен содержать строчные символы");
            }
            if (((string)value).Contains("ABCDEFGHIJKLMNOPQRSTUVWXYZ"))
            {
                return new ValidationResult(false, $"Пароль должен содержать прописные символы");
            }
            if (((string)value).Contains("0123456789"))
            {
                return new ValidationResult(false, $"Пароль должен содержать цифры");
            }
            if (((string)value).Contains("-._@+"))
            {
                return new ValidationResult(false, $"Пароль должен содержать специальные символы");
            }
        }
        catch (Exception e)
        {
            return new ValidationResult(false, $"Введено недопустимое значение. {e.Message}");
        }

        return ValidationResult.ValidResult;
    }
}

