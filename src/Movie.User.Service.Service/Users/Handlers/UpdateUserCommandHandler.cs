using MediatR;
using Movie.User.Service.Service.Common;
using Movie.User.Service.Domain.Repositories;
using Movie.User.Service.Service.Users.Commands;
using Movie.User.Service.Service.Users.Mappings;

namespace Movie.User.Service.Service.Users.Handlers;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<DTOs.UserDto>>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<DTOs.UserDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (user == null)
            return Result<DTOs.UserDto>.NotFound("Usuário");

        var errors = new List<string>();

        var emailExists = await _userRepository.EmailExistsAsync(request.Request.Email, cancellationToken);
        if (emailExists && user.Email != request.Request.Email)
            errors.Add("Email já está em uso.");

        var usernameExists = await _userRepository.UsernameExistsAsync(request.Request.Username, cancellationToken);
        if (usernameExists && user.Username != request.Request.Username)
            errors.Add("Nome de usuário já está em uso.");

        if (errors.Any())
            return Result<DTOs.UserDto>.Failure(errors);

        var address = request.Request.Address.ToDomain();
        user.Update(
            request.Request.Name,
            request.Request.Username,
            request.Request.Email,
            request.Request.Phone,
            address
        );

        var updatedUser = await _userRepository.UpdateAsync(user, cancellationToken);
        return Result<DTOs.UserDto>.Success(updatedUser.ToDto());
    }
}
