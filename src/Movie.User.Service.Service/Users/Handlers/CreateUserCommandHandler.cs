using MediatR;
using Movie.User.Service.Service.Common;
using Movie.User.Service.Domain.Entities;
using Movie.User.Service.Domain.Repositories;
using Movie.User.Service.Domain.ValueObjects;
using Movie.User.Service.Service.Users.Commands;
using Movie.User.Service.Service.Users.Mappings;

namespace Movie.User.Service.Service.Users.Handlers;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<DTOs.UserDto>>
{
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<DTOs.UserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var errors = new List<string>();

        if (await _userRepository.EmailExistsAsync(request.Request.Email, cancellationToken))
            errors.Add("Email já está em uso.");

        if (await _userRepository.UsernameExistsAsync(request.Request.Username, cancellationToken))
            errors.Add("Nome de usuário já está em uso.");

        if (errors.Any())
            return Result<DTOs.UserDto>.Failure(errors);

        var address = request.Request.Address.ToDomain();
        var user = new Movie.User.Service.Domain.Entities.User(
            request.Request.Name,
            request.Request.Username,
            request.Request.Email,
            request.Request.Phone,
            address
        );

        var createdUser = await _userRepository.AddAsync(user, cancellationToken);
        return Result<DTOs.UserDto>.Success(createdUser.ToDto());
    }
}
