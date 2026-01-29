using Movie.User.Service.Api.Responses;
using Movie.User.Service.Service.Users.DTOs;

namespace Movie.User.Service.Api.Mappings;

/// <summary>
/// Mapeamento entre DTOs internos (Service Layer) e Responses da API
/// </summary>
public static class ApiMappingProfile
{
    public static UserResponse ToResponse(this UserDto dto)
    {
        return new UserResponse
        {
            Id = dto.Id,
            Name = dto.Name,
            Username = dto.Username,
            Email = dto.Email,
            Phone = dto.Phone,
            Address = dto.Address.ToResponse(),
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt,
            LastLoginAt = dto.LastLoginAt,
            IsActive = dto.IsActive
        };
    }

    public static AddressResponse ToResponse(this AddressDto dto)
    {
        return new AddressResponse
        {
            Street = dto.Street,
            City = dto.City,
            State = dto.State,
            ZipCode = dto.ZipCode,
            Country = dto.Country
        };
    }

    public static IEnumerable<UserResponse> ToResponse(this IEnumerable<UserDto> dtos)
    {
        return dtos.Select(dto => dto.ToResponse());
    }
}
