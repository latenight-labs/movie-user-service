using Movie.User.Service.Domain.Entities;
using Movie.User.Service.Domain.Repositories;
using Movie.User.Service.Service.Users.Queries;

namespace Movie.User.Service.Service.Users.SearchStrategies;

/// <summary>
/// Interface para estratégias de busca de usuários
/// Segue o Strategy Pattern para aplicar SRP
/// </summary>
public interface IUserSearchStrategy
{
    /// <summary>
    /// Verifica se esta estratégia pode ser aplicada com base nos filtros
    /// </summary>
    bool CanApply(GetUsersByFilterQuery query);

    /// <summary>
    /// Executa a busca específica
    /// </summary>
    Task<IEnumerable<Movie.User.Service.Domain.Entities.User>> SearchAsync(GetUsersByFilterQuery query, IUserRepository repository, CancellationToken cancellationToken);
}
