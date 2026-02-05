using MediatR;
using Movie.User.Service.Service.Common;
using Movie.User.Service.Service.Users.DTOs;

namespace Movie.User.Service.Service.Users.Queries;

public record GetUsersByFilterQuery(
    string? Username = null,
    string? Phone = null,
    string? Street = null,
    string? City = null,
    string? State = null,
    string? ZipCode = null,
    string? Country = null,
    DateTime? StartDate = null //Arthur
) : IRequest<Result<IEnumerable<UserDto>>>;
