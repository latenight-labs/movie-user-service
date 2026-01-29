using Movie.User.Service.Domain.Entities;
using Movie.User.Service.Domain.Repositories;
using Movie.User.Service.Service.Users.Queries;

namespace Movie.User.Service.Service.Users.SearchStrategies;

public class StateSearchStrategy : IUserSearchStrategy
{
    public bool CanApply(GetUsersByFilterQuery query)
    {
        return !string.IsNullOrWhiteSpace(query.State);
    }

    public async Task<IEnumerable<Movie.User.Service.Domain.Entities.User>> SearchAsync(
        GetUsersByFilterQuery query,
        IUserRepository repository,
        CancellationToken cancellationToken)
    {
        return await repository.GetByStateAsync(query.State!, cancellationToken);
    }
}