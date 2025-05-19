using CarteiraDigital.Application.Usuarios.Commands;
using CarteiraDigital.Application.Usuarios.Results;
using CarteiraDigital.Core.Results;
using MediatR;

namespace CarteiraDigital.Application.Usuarios.Validations;

public class RegistrarValidator : IPipelineBehavior<RegistrarCommand, ResultadoOperacao<UsuarioResult>>
{
    public async Task<ResultadoOperacao<UsuarioResult>> Handle(RegistrarCommand request, RequestHandlerDelegate<ResultadoOperacao<UsuarioResult>> next, CancellationToken cancellationToken)
    {
        if(string.IsNullOrEmpty(request.Email))
            return ResultadoOperacao<UsuarioResult>.Falha("O campo Email é obrigatório.");

        if (string.IsNullOrEmpty(request.Senha))
            return ResultadoOperacao<UsuarioResult>.Falha("O campo Senha é obrigatório.");

        return await next();
    }
}
