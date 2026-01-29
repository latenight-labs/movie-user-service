using FluentValidation;
using Movie.User.Service.Api.Requests;

namespace Movie.User.Service.Api.Validators;

/// <summary>
/// Validador para SearchUsersRequest usando FluentValidation
/// </summary>
public class SearchUsersRequestValidator : AbstractValidator<SearchUsersRequest>
{
    public SearchUsersRequestValidator()
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

        When(x => !string.IsNullOrWhiteSpace(x.Name), () =>
        {
            RuleFor(x => x.Name)
                .MinimumLength(2).WithMessage("Nome deve ter no mínimo 2 caracteres.")
                .MaximumLength(200).WithMessage("Nome deve ter no máximo 200 caracteres.");
        });
    }

    private bool HaveAtLeastOneFilter(SearchUsersRequest request)
    {
        return !string.IsNullOrWhiteSpace(request.Name) ||
               !string.IsNullOrWhiteSpace(request.Username) ||
               !string.IsNullOrWhiteSpace(request.City) ||
               !string.IsNullOrWhiteSpace(request.State) ||
               !string.IsNullOrWhiteSpace(request.Country);
    }
}
