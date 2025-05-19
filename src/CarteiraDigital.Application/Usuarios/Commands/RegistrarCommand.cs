using CarteiraDigital.Application.Usuarios.Results;
using CarteiraDigital.Core.Results;
using MediatR;

namespace CarteiraDigital.Application.Usuarios.Commands;

public class RegistrarCommand : IRequest<ResultadoOperacao<UsuarioResult>>
{
    public required string Email { get; set; }
    public required string Senha { get; set; }
}
