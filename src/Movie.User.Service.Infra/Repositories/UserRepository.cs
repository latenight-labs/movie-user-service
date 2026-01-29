using Microsoft.EntityFrameworkCore;
using Movie.User.Service.Domain.Entities;
using Movie.User.Service.Domain.Repositories;
using Movie.User.Service.Infra.Data;

namespace Movie.User.Service.Infra.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Movie.User.Service.Domain.Entities.User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id && u.IsActive, cancellationToken);
    }

    public async Task<Movie.User.Service.Domain.Entities.User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email && u.IsActive, cancellationToken);
    }

    public async Task<Movie.User.Service.Domain.Entities.User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username && u.IsActive, cancellationToken);
    }

    public async Task<IEnumerable<Movie.User.Service.Domain.Entities.User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Where(u => u.IsActive)
            .OrderBy(u => u.Username)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Movie.User.Service.Domain.Entities.User>> GetByCityAsync(string city, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Where(u => u.IsActive && u.Address.City == city)
            .OrderBy(u => u.Username)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Movie.User.Service.Domain.Entities.User>> GetByStateAsync(string state, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Where(u => u.IsActive && u.Address.State == state)
            .OrderBy(u => u.Username)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Movie.User.Service.Domain.Entities.User>> GetByCountryAsync(string country, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Where(u => u.IsActive && u.Address.Country == country)
            .OrderBy(u => u.Username)
            .ToListAsync(cancellationToken);
    }

    public async Task<Movie.User.Service.Domain.Entities.User> AddAsync(Movie.User.Service.Domain.Entities.User user, CancellationToken cancellationToken = default)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task<Movie.User.Service.Domain.Entities.User> UpdateAsync(Movie.User.Service.Domain.Entities.User user, CancellationToken cancellationToken = default)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var user = await GetByIdAsync(id, cancellationToken);
        if (user != null)
        {
            user.Deactivate();
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AnyAsync(u => u.Id == id && u.IsActive, cancellationToken);
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AnyAsync(u => u.Email == email && u.IsActive, cancellationToken);
    }

    public async Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AnyAsync(u => u.Username == username && u.IsActive, cancellationToken);
    }
}