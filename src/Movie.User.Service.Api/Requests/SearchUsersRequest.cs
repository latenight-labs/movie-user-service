namespace Movie.User.Service.Api.Requests;

/// <summary>
/// Request específico da API para busca de usuários
/// </summary>
public class SearchUsersRequest
{
    public string? Name { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string? Country { get; set; }
    public DateTime? StartDate { get; set; }
}
