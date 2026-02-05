using FluentValidation;

namespace Movie.User.Service.Service.Users.Validators.Extensions;

public static class StringValidationExtensions
{

    public static IRuleBuilderOptions<T, string> Required<T>(
        this IRuleBuilder<T, string> rule,
        string fieldName)
    {
        return rule
            .NotEmpty().WithMessage($"{fieldName} é obrigatório.");
    }
    public static IRuleBuilderOptions<T, string> RequiredWithLength<T>(
        this IRuleBuilder<T, string> rule,
        int min,
        int max,
        string fieldName)
    {
        return rule
            .Required(fieldName)
            .WithMinAndMaxLength(min, max, fieldName);
    }

    public static IRuleBuilderOptions<T, string> WithMinAndMaxLength<T>(
        this IRuleBuilder<T, string?> rule,
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
    

    public static IRuleBuilderOptions<T, string> MatchesPhoneRegex<T>(
        this IRuleBuilder<T, string> rule,
        string phoneRegex,
        string fieldName)
    {
        return rule
            .Required(fieldName)
            .Matches(phoneRegex).WithMessage($"{fieldName} inválido. Use o formato internacional.");
    }

    public static IRuleBuilderOptions<T, string> MatchesZipCodeRegex<T>(
        this IRuleBuilder<T, string> rule,
        string zipCodeRegex,
        string fieldName)
    {
        return rule
            .Required(fieldName)
            .Matches(zipCodeRegex).WithMessage($"{fieldName} inválido. Use o formato 00000-000 ou 00000000.");
    }
}
