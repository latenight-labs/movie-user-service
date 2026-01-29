using MediatR;
using Movie.User.Service.Domain.Repositories;
using Movie.User.Service.Service.Common;
using Movie.User.Service.Service.Users.Commands;

namespace Movie.User.Service.Service.Users.Handlers;

public class UpdateLastLoginCommandHandler : IRequestHandler<UpdateLastLoginCommand, Result<bool>>
{
    private readonly IUserRepository _userRepository;

    public UpdateLastLoginCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<bool>> Handle(UpdateLastLoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        
        if (user == null)
        {
            return Result<bool>.Failure("Usuário não encontrado");
        }

        user.UpdateLastLogin();
        
        await _userRepository.UpdateAsync(user, cancellationToken);
        
        return Result<bool>.Success(true);
    }
}