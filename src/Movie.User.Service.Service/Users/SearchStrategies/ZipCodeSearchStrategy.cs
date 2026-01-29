using Movie.User.Service.Domain.Entities;
using Movie.User.Service.Domain.Repositories;
using Movie.User.Service.Service.Users.Queries;

namespace Movie.User.Service.Service.Users.SearchStrategies;

public class ZipCodeSearchStrategy : IUserSearchStrategy
{
    public bool CanApply(GetUsersByFilterQuery query)
    {
        return !string.IsNullOrWhiteSpace(query.ZipCode);
    }

    public async Task<IEnumerable<Movie.User.Service.Domain.Entities.User>> SearchAsync(
        GetUsersByFilterQuery query,
        IUserRepository repository,
        CancellationToken cancellationToken)
    {
        // Como não temos método específico para buscar por CEP, retornamos todos e filtramos
        var allUsers = await repository.GetAllAsync(cancellationToken);
        return allUsers.Where(u => u.Address.ZipCode.Contains(query.ZipCode!, StringComparison.OrdinalIgnoreCase));
    }
}