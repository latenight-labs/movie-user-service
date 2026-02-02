using MediatR;
using Movie.User.Service.Domain.Repositories;
using Movie.User.Service.Service.Common;
using Movie.User.Service.Service.Users.DTOs;
using Movie.User.Service.Service.Users.Mappings;
using Movie.User.Service.Service.Users.Queries;

namespace Movie.User.Service.Service.Users.Handlers;

//Arthur
public class GetAllCreatedAfterDateTimeHandler : IRequestHandler<GetAllCreatedAfterDateTimeQuery,Result<IEnumerable<UserDto>>>
{
    private readonly IUserRepository _userRepository;
   
    public GetAllCreatedAfterDateTimeHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<Result<IEnumerable<UserDto>>> Handle(GetAllCreatedAfterDateTimeQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllCreatedAfterDateTime(request.Date,cancellationToken);
        var userDtos = users.Select(u => u.ToDto());
        return Result<IEnumerable<UserDto>>.Success(userDtos);
    }
}

