using MediatR;
using Movie.User.Service.Service.Common;
using Movie.User.Service.Domain.Repositories;
using Movie.User.Service.Service.Users.DTOs;
using Movie.User.Service.Service.Users.Mappings;
using Movie.User.Service.Service.Users.Queries;

namespace Movie.User.Service.Service.Users.Handlers;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<IEnumerable<UserDto>>>
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<IEnumerable<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllAsync(cancellationToken);
        var userDtos = users.Select(u => u.ToDto());
        return Result<IEnumerable<UserDto>>.Success(userDtos);
    }
}
