using MediatR;
using Movie.User.Service.Service.Common;
using Movie.User.Service.Domain.Repositories;
using Movie.User.Service.Service.Users.DTOs;
using Movie.User.Service.Service.Users.Mappings;
using Movie.User.Service.Service.Users.Queries;
using Movie.User.Service.Service.Users.SearchStrategies;

namespace Movie.User.Service.Service.Users.Handlers;

/// <summary>
/// Handler refatorado seguindo SRP - delega a busca para estratégias específicas
/// </summary>
public class GetUsersByFilterQueryHandler : IRequestHandler<GetUsersByFilterQuery, Result<IEnumerable<UserDto>>>
{
    private readonly IUserRepository _userRepository;
    private readonly IEnumerable<IUserSearchStrategy> _searchStrategies;

    public GetUsersByFilterQueryHandler(
        IUserRepository userRepository,
        IEnumerable<IUserSearchStrategy> searchStrategies)
    {
        _userRepository = userRepository;
        _searchStrategies = searchStrategies;
    }

    public async Task<Result<IEnumerable<UserDto>>> Handle(GetUsersByFilterQuery request, CancellationToken cancellationToken)
    {
        // Encontra a primeira estratégia que pode ser aplicada
        var strategy = _searchStrategies.FirstOrDefault(s => s.CanApply(request));

        if (strategy == null)
        {
            // Fallback: retorna todos os usuários se nenhuma estratégia se aplica
            var allUsers = await _userRepository.GetAllAsync(cancellationToken);
            var userDtos = allUsers.Select(u => u.ToDto());
            return Result<IEnumerable<UserDto>>.Success(userDtos);
        }

        // Executa a busca usando a estratégia selecionada
        var users = await strategy.SearchAsync(request, _userRepository, cancellationToken);
        var result = users.Select(u => u.ToDto());
        
        return Result<IEnumerable<UserDto>>.Success(result);
    }
}
