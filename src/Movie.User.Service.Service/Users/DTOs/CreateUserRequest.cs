namespace Movie.User.Service.Service.Users.DTOs;

public class CreateUserRequest
{
    public string Name { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public AddressDto Address { get; set; } = null!;
}
