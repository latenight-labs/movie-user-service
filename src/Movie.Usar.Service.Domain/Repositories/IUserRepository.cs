using Movie.User.Service.Domain.Entities;

namespace Movie.User.Service.Domain.Repositories;

public interface IUserRepository
{
    Task<Entities.User?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Entities.User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Entities.User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<IEnumerable<Entities.User>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Entities.User>> GetByCityAsync(string city, CancellationToken cancellationToken = default);
    Task<IEnumerable<Entities.User>> GetByStateAsync(string state, CancellationToken cancellationToken = default);
    Task<IEnumerable<Entities.User>> GetByCountryAsync(string country, CancellationToken cancellationToken = default);
    Task<Entities.User> AddAsync(Entities.User user, CancellationToken cancellationToken = default);
    Task<Entities.User> UpdateAsync(Entities.User user, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken = default);
}