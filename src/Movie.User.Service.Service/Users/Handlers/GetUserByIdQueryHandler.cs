using MediatR;
using Movie.User.Service.Service.Common;
using Movie.User.Service.Domain.Repositories;
using Movie.User.Service.Service.Users.Mappings;
using Movie.User.Service.Service.Users.Queries;

namespace Movie.User.Service.Service.Users.Handlers;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<DTOs.UserDto>>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<DTOs.UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (user == null)
            return Result<DTOs.UserDto>.NotFound("Usuário");

        return Result<DTOs.UserDto>.Success(user.ToDto());
    }
}
