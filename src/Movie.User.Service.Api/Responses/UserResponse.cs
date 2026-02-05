namespace Movie.User.Service.Api.Responses;

/// <summary>
/// Response específico da API - pode ter campos diferentes do DTO interno
/// </summary>
public class UserResponse
{
    /// <summary>
    /// ID único do usuário
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Nome completo do usuário
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Nome de usuário (username) único
    /// </summary>
    public string Username { get; set; } = string.Empty;
    
    /// <summary>
    /// Email do usuário
    /// </summary>
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// Telefone do usuário
    /// </summary>
    public string Phone { get; set; } = string.Empty;
    
    /// <summary>
    /// Endereço completo do usuário
    /// </summary>
    public AddressResponse Address { get; set; } = null!;
    
    /// <summary>
    /// Data e hora de criação do usuário
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Data e hora da última atualização do usuário
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
    
    /// <summary>
    /// Data e hora do último login do usuário
    /// </summary>
    public DateTime? LastLoginAt { get; set; }
    
    /// <summary>
    /// Indica se o usuário está ativo no sistema
    /// </summary>
    public bool IsActive { get; set; }
}

/// <summary>
/// Response com dados do endereço do usuário
/// </summary>
public class AddressResponse
{
    /// <summary>
    /// Rua e número do endereço
    /// </summary>
    public string Street { get; set; } = string.Empty;
    
    /// <summary>
    /// Cidade do endereço
    /// </summary>
    public string City { get; set; } = string.Empty;
    
    /// <summary>
    /// Estado do endereço
    /// </summary>
    public string State { get; set; } = string.Empty;
    
    /// <summary>
    /// CEP do endereço
    /// </summary>
    public string ZipCode { get; set; } = string.Empty;
    
    /// <summary>
    /// País do endereço
    /// </summary>
    public string Country { get; set; } = string.Empty;
}
