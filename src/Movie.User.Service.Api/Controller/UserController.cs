using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Movie.User.Service.Api.Mappings;
using Movie.User.Service.Api.Requests;
using Movie.User.Service.Api.Responses;
using Movie.User.Service.Service.Common;
using Movie.User.Service.Service.Users.Commands;
using Movie.User.Service.Service.Users.DTOs;
using Movie.User.Service.Service.Users.Queries;
using Swashbuckle.AspNetCore.Annotations;

namespace Movie.User.Service.Api.Controllers;

/// <summary>
/// Controller para gerenciamento de usuários
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[SwaggerTag("Operações relacionadas ao gerenciamento de usuários")]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<UserController> _logger;
    private readonly IValidator<SearchUsersRequest> _searchUsersValidator;

    public UserController(
        IMediator mediator, 
        ILogger<UserController> logger,
        IValidator<SearchUsersRequest> searchUsersValidator)
    {
        _mediator = mediator;
        _logger = logger;
        _searchUsersValidator = searchUsersValidator;
    }

    /// <summary>
    /// Obtém todos os usuários cadastrados no sistema
    /// </summary>
    /// <returns>Lista de todos os usuários</returns>
    /// <response code="200">Lista de usuários retornada com sucesso</response>
    /// <response code="400">Erro na requisição</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet]
    [SwaggerOperation(
        Summary = "Listar todos os usuários",
        Description = "Retorna uma lista com todos os usuários cadastrados no sistema",
        OperationId = "GetAllUsers",
        Tags = new[] { "Users" }
    )]
    [SwaggerResponse(200, "Lista de usuários retornada com sucesso", typeof(IEnumerable<UserResponse>))]
    [SwaggerResponse(400, "Erro na requisição")]
    [SwaggerResponse(500, "Erro interno do servidor")]
    public async Task<ActionResult<IEnumerable<UserResponse>>> GetAllUsers(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Buscando todos os usuários");
        
        var query = new GetAllUsersQuery();
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning("Falha ao buscar usuários: {Error}", result.Error);
            return BadRequest(new { errors = result.Errors, error = result.Error });
        }

        var response = result.Value!.ToResponse();
        return Ok(response);
    }


    /// <summary>
    /// Obtém todos os usuários cadastrados no sistema que foram criados a partir de uma data específica
    /// </summary>
    /// <returns>Lista de todos os usuários criados a partir de uma data específica</returns>
    /// <response code="200">Lista de usuários retornada com sucesso</response>
    /// <response code="400">Erro na requisição</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet("created-after-date")]
    [SwaggerOperation(
        Summary = "Buscar usuários criados a partir de uma data",
        Description = "Retorna uma lista com todos os usuários criados a partir de uma data específica",
        OperationId = "GetAllUsersCreatedAfterDate",
        Tags = new[] { "Users", "CreatedAfterDate" }
    )]
    [SwaggerResponse(200, "Lista de usuários retornada com sucesso", typeof(IEnumerable<UserResponse>))]
    [SwaggerResponse(400, "Erro na requisição")]
    [SwaggerResponse(500, "Erro interno do servidor")]
    public async Task<ActionResult<IEnumerable<UserResponse>>> GetAllUsersCreatedAfterDate(
                                         [SwaggerParameter("Data ponto de referência para a pesquisa", Required = true)] DateTime date,
                                         CancellationToken cancellationToken)
    {
        _logger.LogInformation("Buscando todos os usuários criados a partir da data: {Date}", date);

        var query = new GetAllCreatedAfterDateTimeQuery(date);
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning("Falha ao buscar usuários criados a partir da data: {Date}: {Error}", date, result.Error);
            return BadRequest(new { errors = result.Errors, error = result.Error });
        }

        var response = result.Value!.ToResponse();
        return Ok(response);
        
    }








    /// <summary>
    /// Obtém um usuário específico pelo seu ID
    /// </summary>
    /// <param name="id">ID único do usuário</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Dados do usuário encontrado</returns>
    /// <response code="200">Usuário encontrado com sucesso</response>
    /// <response code="404">Usuário não encontrado</response>
    /// <response code="400">ID inválido</response>
    [HttpGet("{id:int}")]
    [SwaggerOperation(
        Summary = "Buscar usuário por ID",
        Description = "Retorna os dados de um usuário específico baseado no seu ID único",
        OperationId = "GetUserById",
        Tags = new[] { "Users" }
    )]
    [SwaggerResponse(200, "Usuário encontrado com sucesso", typeof(UserResponse))]
    [SwaggerResponse(404, "Usuário não encontrado")]
    [SwaggerResponse(400, "ID inválido")]
    public async Task<ActionResult<UserResponse>> GetUserById(
        [SwaggerParameter("ID único do usuário", Required = true)] int id, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Buscando usuário com ID: {UserId}", id);
        
        var query = new GetUserByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning("Usuário não encontrado: {UserId}", id);
            return NotFound(new { error = result.Error, errors = result.Errors });
        }

        var response = result.Value!.ToResponse();
        return Ok(response);
    }

    /// <summary>
    /// Obtém um usuário específico pelo seu email
    /// </summary>
    /// <param name="email">Email do usuário</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Dados do usuário encontrado</returns>
    /// <response code="200">Usuário encontrado com sucesso</response>
    /// <response code="404">Usuário não encontrado</response>
    /// <response code="400">Email inválido</response>
    [HttpGet("email/{email}")]
    [SwaggerOperation(
        Summary = "Buscar usuário por email",
        Description = "Retorna os dados de um usuário específico baseado no seu email",
        OperationId = "GetUserByEmail",
        Tags = new[] { "Users" }
    )]
    [SwaggerResponse(200, "Usuário encontrado com sucesso", typeof(UserResponse))]
    [SwaggerResponse(404, "Usuário não encontrado")]
    [SwaggerResponse(400, "Email inválido")]
    public async Task<ActionResult<UserResponse>> GetUserByEmail(
        [SwaggerParameter("Email do usuário", Required = true)] string email, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Buscando usuário com email: {Email}", email);
        
        var query = new GetUserByEmailQuery(email);
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning("Usuário não encontrado: {Email}", email);
            return NotFound(new { error = result.Error, errors = result.Errors });
        }

        var response = result.Value!.ToResponse();
        return Ok(response);
    }

    /// <summary>
    /// Busca usuários por filtros específicos
    /// </summary>
    /// <param name="request">Parâmetros de busca (username, phone, address, city, state, zipcode, country)</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de usuários que atendem aos critérios de busca</returns>
    /// <response code="200">Usuários encontrados com sucesso</response>
    /// <response code="400">Parâmetros de busca inválidos</response>
    [HttpGet("search")]
    [SwaggerOperation(
        Summary = "Buscar usuários por filtros",
        Description = "Permite buscar usuários utilizando diferentes critérios como username, telefone, endereço, cidade, estado, CEP ou país",
        OperationId = "SearchUsers",
        Tags = new[] { "Users" }
    )]
    [SwaggerResponse(200, "Usuários encontrados com sucesso", typeof(IEnumerable<UserResponse>))]
    [SwaggerResponse(400, "Parâmetros de busca inválidos")]
    public async Task<ActionResult<IEnumerable<UserResponse>>> SearchUsers(
        [FromQuery, SwaggerParameter("Parâmetros de busca")] SearchUsersRequest request,
        CancellationToken cancellationToken = default)
    {
        // Validação com FluentValidation
        var validationResult = await _searchUsersValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            _logger.LogWarning("Validação falhou para busca de usuários: {Errors}", string.Join(", ", errors));
            return BadRequest(new { errors = errors });
        }

        _logger.LogInformation("Buscando usuários {@Request}", request);
        
        var query = new GetUsersByFilterQuery(
            request.Username,
            request.Phone,
            request.Address,
            request.City,
            request.State,
            request.ZipCode,
            request.Country,
            request.StartDate
        );
        
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning("Falha ao buscar usuários: {Error}", result.Error);
            return BadRequest(new { errors = result.Errors, error = result.Error });
        }

        var response = result.Value!.ToResponse();
        return Ok(response);
    }

    /// <summary>
    /// Cria um novo usuário no sistema
    /// </summary>
    /// <param name="request">Dados do usuário a ser criado</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Dados do usuário criado</returns>
    /// <response code="201">Usuário criado com sucesso</response>
    /// <response code="400">Dados inválidos ou usuário já existe</response>
    [HttpPost]
    [SwaggerOperation(
        Summary = "Criar novo usuário",
        Description = "Cria um novo usuário no sistema com os dados fornecidos",
        OperationId = "CreateUser",
        Tags = new[] { "Users" }
    )]
    [SwaggerResponse(201, "Usuário criado com sucesso", typeof(UserResponse))]
    [SwaggerResponse(400, "Dados inválidos ou usuário já existe")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<UserResponse>> CreateUser(
        [FromBody, SwaggerParameter("Dados do usuário a ser criado", Required = true)] CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Criando novo usuário: {Username}", request.Username);
        
        var command = new CreateUserCommand(request);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning("Falha ao criar usuário: {Errors}", string.Join(", ", result.Errors));
            return BadRequest(new { errors = result.Errors, error = result.Error });
        }

        var response = result.Value!.ToResponse();
        return CreatedAtAction(
            nameof(GetUserById),
            new { id = response.Id },
            response);
    }

    /// <summary>
    /// Atualiza os dados de um usuário existente
    /// </summary>
    /// <param name="id">ID único do usuário a ser atualizado</param>
    /// <param name="request">Novos dados do usuário</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Dados atualizados do usuário</returns>
    /// <response code="200">Usuário atualizado com sucesso</response>
    /// <response code="404">Usuário não encontrado</response>
    /// <response code="400">Dados inválidos</response>
    [HttpPut("{id:int}")]
    [SwaggerOperation(
        Summary = "Atualizar usuário",
        Description = "Atualiza os dados de um usuário existente no sistema",
        OperationId = "UpdateUser",
        Tags = new[] { "Users" }
    )]
    [SwaggerResponse(200, "Usuário atualizado com sucesso", typeof(UserResponse))]
    [SwaggerResponse(404, "Usuário não encontrado")]
    [SwaggerResponse(400, "Dados inválidos")]
    public async Task<ActionResult<UserResponse>> UpdateUser(
        [SwaggerParameter("ID único do usuário", Required = true)] int id,
        [FromBody, SwaggerParameter("Novos dados do usuário", Required = true)] UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Atualizando usuário: {UserId}", id);
        
        var command = new UpdateUserCommand(id, request);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            if (result.Error?.Contains("não encontrado") == true)
            {
                _logger.LogWarning("Usuário não encontrado: {UserId}", id);
                return NotFound(new { error = result.Error, errors = result.Errors });
            }

            _logger.LogWarning("Falha ao atualizar usuário: {Errors}", string.Join(", ", result.Errors));
            return BadRequest(new { errors = result.Errors, error = result.Error });
        }

        var response = result.Value!.ToResponse();
        return Ok(response);
    }

    /// <summary>
    /// Remove um usuário do sistema
    /// </summary>
    /// <param name="id">ID único do usuário a ser removido</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Confirmação da remoção</returns>
    /// <response code="204">Usuário removido com sucesso</response>
    /// <response code="404">Usuário não encontrado</response>
    /// <response code="400">Erro na requisição</response>
    [HttpDelete("{id:int}")]
    [SwaggerOperation(
        Summary = "Remover usuário",
        Description = "Remove permanentemente um usuário do sistema",
        OperationId = "DeleteUser",
        Tags = new[] { "Users" }
    )]
    [SwaggerResponse(204, "Usuário removido com sucesso")]
    [SwaggerResponse(404, "Usuário não encontrado")]
    [SwaggerResponse(400, "Erro na requisição")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteUser(
        [SwaggerParameter("ID único do usuário", Required = true)] int id, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deletando usuário: {UserId}", id);
        
        var command = new DeleteUserCommand(id);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            if (result.Error?.Contains("não encontrado") == true)
            {
                _logger.LogWarning("Usuário não encontrado: {UserId}", id);
                return NotFound(new { error = result.Error, errors = result.Errors });
            }

            _logger.LogWarning("Falha ao deletar usuário: {Error}", result.Error);
            return BadRequest(new { errors = result.Errors, error = result.Error });
        }

        return NoContent();
    }
}
