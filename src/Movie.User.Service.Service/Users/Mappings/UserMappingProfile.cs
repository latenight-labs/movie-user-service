using Movie.User.Service.Domain.Entities;
using Movie.User.Service.Domain.ValueObjects;
using Movie.User.Service.Service.Users.DTOs;

namespace Movie.User.Service.Service.Users.Mappings;

public static class UserMappingProfile
{
    public static UserDto ToDto(this Movie.User.Service.Domain.Entities.User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Username = user.Username,
            Email = user.Email,
            Phone = user.Phone,
            Address = user.Address.ToDto(),
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            LastLoginAt = user.LastLoginAt,
            IsActive = user.IsActive
        };
    }

    public static AddressDto ToDto(this Address address)
    {
        return new AddressDto
        {
            Street = address.Street,
            City = address.City,
            State = address.State,
            ZipCode = address.ZipCode,
            Country = address.Country
        };
    }

    public static Address ToDomain(this AddressDto dto)
    {
        return new Address(
            dto.Street,
            dto.City,
            dto.State,
            dto.ZipCode,
            dto.Country
        );
    }
}
