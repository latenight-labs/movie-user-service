using MediatR;
using Movie.User.Service.Service.Common;
using Movie.User.Service.Service.Users.DTOs;

namespace Movie.User.Service.Service.Users.Queries;

public record GetUserByEmailQuery(string Email) : IRequest<Result<UserDto>>;
