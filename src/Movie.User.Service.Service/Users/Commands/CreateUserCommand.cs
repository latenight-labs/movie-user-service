using MediatR;
using Movie.User.Service.Service.Common;
using Movie.User.Service.Service.Users.DTOs;

namespace Movie.User.Service.Service.Users.Commands;

public record CreateUserCommand(CreateUserRequest Request) : IRequest<Result<UserDto>>;
