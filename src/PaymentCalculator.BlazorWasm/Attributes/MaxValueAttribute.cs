using System.ComponentModel.DataAnnotations;

namespace PaymentCalculator.BlazorWasm.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class MaxValueAttribute : ValidationAttribute
{
    private readonly string _comparisonProperty;

    public MaxValueAttribute(string comparisonProperty)
    {
        _comparisonProperty = comparisonProperty;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not decimal currentValue)
        {
            return ValidationResult.Success;
        }

        var property = validationContext.ObjectType.GetProperty(_comparisonProperty)
            ?? throw new ArgumentException("Property with this name not found");

        var comparisonValue = property.GetValue(validationContext.ObjectInstance) as decimal?;

        if (currentValue > comparisonValue)
        {
            var errorMessage = ErrorMessage ?? $"{{0}} cannot be greater than {_comparisonProperty}";

            return new ValidationResult(string.Format(errorMessage, validationContext.DisplayName));
        }

        return ValidationResult.Success;
    }
}
