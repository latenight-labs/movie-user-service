using Movie.User.Service.Domain.ValueObjects;

namespace Movie.User.Service.Domain.Entities;

public class User
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Username { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;
    public Address Address { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? LastLoginAt { get; private set; }
    public bool IsActive { get; private set; }

    private User() { } // Para EF Core

    public User(string name, string username, string email, string phone, Address address)
    {
        Name = name;
        Username = username;
        Email = email;
        Phone = phone;
        Address = address;
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
    }

    public void Update(string name, string username, string email, string phone, Address address)
    {
        Name = name;
        Username = username;
        Email = email;
        Phone = phone;
        Address = address;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateLastLogin()
    {
        LastLoginAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
}
