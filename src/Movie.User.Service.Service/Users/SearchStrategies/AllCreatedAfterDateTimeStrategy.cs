using Movie.User.Service.Domain.Repositories;
using Movie.User.Service.Service.Users.Queries;

namespace Movie.User.Service.Service.Users.SearchStrategies;
public class AllCreatedAfterDateTimeStrategy : IUserSearchStrategy //Arthur
{
    public bool CanApply(GetUsersByFilterQuery query)
    {
        return query.StartDate.HasValue;
    }

    public async Task<IEnumerable<Movie.User.Service.Domain.Entities.User>> SearchAsync(
        GetUsersByFilterQuery query,
        IUserRepository repository,
        CancellationToken cancellationToken)
    {
        return await repository.GetAllCreatedAfterDateTime(query.StartDate,cancellationToken);
    }
}

