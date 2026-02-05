using Movie.User.Service.Api.Responses;
using Movie.User.Service.Service.Users.DTOs;

namespace Movie.User.Service.Api.Mappings;

/// <summary>
/// Mapeamento entre DTOs internos (Service Layer) e Responses da API
/// </summary>
public static class ApiMappingProfile
{
    /// <summary>
    /// Converte UserDto para UserResponse
    /// </summary>
    /// <param name="dto">DTO do usuário</param>
    /// <returns>Response do usuário</returns>
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

    /// <summary>
    /// Converte AddressDto para AddressResponse
    /// </summary>
    /// <param name="dto">DTO do endereço</param>
    /// <returns>Response do endereço</returns>
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

    /// <summary>
    /// Converte coleção de UserDto para coleção de UserResponse
    /// </summary>
    /// <param name="dtos">Coleção de DTOs de usuários</param>
    /// <returns>Coleção de responses de usuários</returns>
    public static IEnumerable<UserResponse> ToResponse(this IEnumerable<UserDto> dtos)
    {
        return dtos.Select(dto => dto.ToResponse());
    }
}
