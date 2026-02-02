using MediatR;
using Movie.User.Service.Service.Common;
using Movie.User.Service.Service.Users.DTOs;

namespace Movie.User.Service.Service.Users.Queries;

//Arthur
public record GetAllCreatedAfterDateTimeQuery(DateTime Date): IRequest<Result<IEnumerable<UserDto>>>; 
