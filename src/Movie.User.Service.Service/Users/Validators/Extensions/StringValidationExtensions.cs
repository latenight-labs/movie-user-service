using FluentValidation;

namespace Movie.User.Service.Service.Users.Validators.Extensions;

public static class StringValidationExtensions
{
    public static IRuleBuilderOptions<T, string> RequiredWithLength<T>(
        this IRuleBuilder<T, string> rule,
        int min,
        int max,
        string fieldName)
    {
        return rule
            .NotEmpty().WithMessage($"{fieldName} é obrigatório.")
            .WithMinAndMaxLength(min, max, fieldName);
    }
    
    public static IRuleBuilderOptions<T, string> WithMinAndMaxLength<T>(
        this IRuleBuilder<T, string> rule,
        int min,
        int max,
        string fieldName)
    {
        return rule
            .MinimumLength(min)
            .WithMessage($"{fieldName} deve ter no mínimo {min} caracteres.")
            .MaximumLength(max)
            .WithMessage($"{fieldName} deve ter no máximo {max} caracteres.");
    }
}
