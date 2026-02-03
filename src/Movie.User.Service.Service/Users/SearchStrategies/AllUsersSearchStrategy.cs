using Movie.User.Service.Domain.Entities;
using Movie.User.Service.Domain.Repositories;
using Movie.User.Service.Service.Users.Queries;

namespace Movie.User.Service.Service.Users.SearchStrategies;

public class AllUsersSearchStrategy : IUserSearchStrategy
{
    public bool CanApply(GetUsersByFilterQuery query)
    {
        return string.IsNullOrWhiteSpace(query.Username) &&
               string.IsNullOrWhiteSpace(query.City) &&
               string.IsNullOrWhiteSpace(query.State) &&
               string.IsNullOrWhiteSpace(query.Country) &&
               string.IsNullOrWhiteSpace(query.Phone) &&
               string.IsNullOrWhiteSpace(query.Street) &&
               string.IsNullOrWhiteSpace(query.ZipCode);
    }

    public async Task<IEnumerable<Movie.User.Service.Domain.Entities.User>> SearchAsync(
        GetUsersByFilterQuery query,
        IUserRepository repository,
        CancellationToken cancellationToken)
    {
        return await repository.GetAllAsync(cancellationToken);
    }
}