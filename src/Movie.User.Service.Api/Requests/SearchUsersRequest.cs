namespace Movie.User.Service.Api.Requests;

/// <summary>
/// Request específico da API para busca de usuários
/// </summary>
public class SearchUsersRequest
{
    /// <summary>
    /// Nome do usuário para busca
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// Nome de usuário (username) para busca
    /// </summary>
    public string? Username { get; set; }
    
    /// <summary>
    /// Email do usuário para busca
    /// </summary>
    public string? Email { get; set; }
    
    /// <summary>
    /// Telefone do usuário para busca
    /// </summary>
    public string? Phone { get; set; }
    
    /// <summary>
    /// Endereço (rua) do usuário para busca
    /// </summary>
    public string? Address { get; set; }
    
    /// <summary>
    /// Cidade do usuário para busca
    /// </summary>
    public string? City { get; set; }
    
    /// <summary>
    /// Estado do usuário para busca
    /// </summary>
    public string? State { get; set; }
    
    /// <summary>
    /// CEP do usuário para busca
    /// </summary>
    public string? ZipCode { get; set; }
    
    /// <summary>
    /// País do usuário para busca
    /// </summary>
    public string? Country { get; set; }
    
    /// <summary>
    /// Data de início para filtrar usuários criados a partir desta data
    /// </summary>
    public DateTime? StartDate { get; set; }
}
