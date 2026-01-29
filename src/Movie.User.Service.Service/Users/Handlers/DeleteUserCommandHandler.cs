using MediatR;
using Movie.User.Service.Service.Common;
using Movie.User.Service.Domain.Repositories;
using Movie.User.Service.Service.Users.Commands;

namespace Movie.User.Service.Service.Users.Handlers;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var exists = await _userRepository.ExistsAsync(request.Id, cancellationToken);
        
        if (!exists)
            return Result.Failure("Usuário não encontrado.");

        await _userRepository.DeleteAsync(request.Id, cancellationToken);
        return Result.Success();
    }
}
