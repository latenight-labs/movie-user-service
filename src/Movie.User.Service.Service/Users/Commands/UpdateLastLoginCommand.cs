using MediatR;
using Movie.User.Service.Service.Common;

namespace Movie.User.Service.Service.Users.Commands;

public record UpdateLastLoginCommand(int UserId) : IRequest<Result<bool>>;