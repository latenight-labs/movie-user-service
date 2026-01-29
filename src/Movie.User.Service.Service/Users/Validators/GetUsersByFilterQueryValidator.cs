using FluentValidation;
using Movie.User.Service.Service.Users.Queries;

namespace Movie.User.Service.Service.Users.Validators;

/// <summary>
/// Validador para GetUsersByFilterQuery - será executado automaticamente pelo ValidationBehavior
/// </summary>
public class GetUsersByFilterQueryValidator : AbstractValidator<GetUsersByFilterQuery>
{
    public GetUsersByFilterQueryValidator()
    {
        // Valida que pelo menos um filtro seja fornecido
        RuleFor(x => x)
            .Must(HaveAtLeastOneFilter)
            .WithMessage("Pelo menos um filtro deve ser fornecido para a busca.");

        // Validações opcionais para cada campo (quando fornecido)
        When(x => !string.IsNullOrWhiteSpace(x.Username), () =>
        {
            RuleFor(x => x.Username)
                .MinimumLength(3).WithMessage("Nome de usuário deve ter no mínimo 3 caracteres.")
                .MaximumLength(50).WithMessage("Nome de usuário deve ter no máximo 50 caracteres.");
        });

        When(x => !string.IsNullOrWhiteSpace(x.Phone), () =>
        {
            RuleFor(x => x.Phone)
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Telefone inválido. Use o formato internacional.");
        });

        When(x => !string.IsNullOrWhiteSpace(x.ZipCode), () =>
        {
            RuleFor(x => x.ZipCode)
                .Matches(@"^\d{5}-?\d{3}$").WithMessage("CEP inválido. Use o formato 00000-000 ou 00000000.");
        });

        When(x => !string.IsNullOrWhiteSpace(x.City), () =>
        {
            RuleFor(x => x.City)
                .MinimumLength(2).WithMessage("Cidade deve ter no mínimo 2 caracteres.")
                .MaximumLength(100).WithMessage("Cidade deve ter no máximo 100 caracteres.");
        });

        When(x => !string.IsNullOrWhiteSpace(x.State), () =>
        {
            RuleFor(x => x.State)
                .MinimumLength(2).WithMessage("Estado deve ter no mínimo 2 caracteres.")
                .MaximumLength(100).WithMessage("Estado deve ter no máximo 100 caracteres.");
        });

        When(x => !string.IsNullOrWhiteSpace(x.Country), () =>
        {
            RuleFor(x => x.Country)
                .MinimumLength(2).WithMessage("País deve ter no mínimo 2 caracteres.")
                .MaximumLength(100).WithMessage("País deve ter no máximo 100 caracteres.");
        });

        When(x => !string.IsNullOrWhiteSpace(x.Address), () =>
        {
            RuleFor(x => x.Address)
                .MinimumLength(5).WithMessage("Endereço deve ter no mínimo 5 caracteres.")
                .MaximumLength(200).WithMessage("Endereço deve ter no máximo 200 caracteres.");
        });
    }

    private bool HaveAtLeastOneFilter(GetUsersByFilterQuery query)
    {
        return !string.IsNullOrWhiteSpace(query.Username) ||
               !string.IsNullOrWhiteSpace(query.Phone) ||
               !string.IsNullOrWhiteSpace(query.Address) ||
               !string.IsNullOrWhiteSpace(query.City) ||
               !string.IsNullOrWhiteSpace(query.State) ||
               !string.IsNullOrWhiteSpace(query.ZipCode) ||
               !string.IsNullOrWhiteSpace(query.Country);
    }
}
