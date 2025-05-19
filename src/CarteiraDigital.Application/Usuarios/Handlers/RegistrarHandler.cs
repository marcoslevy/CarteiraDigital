using CarteiraDigital.Application.Usuarios.Commands;
using CarteiraDigital.Application.Usuarios.Results;
using CarteiraDigital.Core.Entities.Usuarios;
using CarteiraDigital.Core.Interfaces;
using CarteiraDigital.Core.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CarteiraDigital.Application.Usuarios.Handlers;

public class RegistrarHandler : IRequestHandler<RegistrarCommand, ResultadoOperacao<UsuarioResult>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<Usuario> _userManager;

    public RegistrarHandler(IUnitOfWork unitOfWork, UserManager<Usuario> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task<ResultadoOperacao<UsuarioResult>> Handle(RegistrarCommand request, CancellationToken cancellationToken)
    {
        var user = new Usuario(request.Email);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var result = await _userManager.CreateAsync(user, request.Senha);

            if (!result.Succeeded)
                return ResultadoOperacao<UsuarioResult>.Falha(result.ToString());

            await _unitOfWork.CommitAsync();

            var usuarioResult = new UsuarioResult()
            {
                Mensagem = "Usuário registrado com sucesso vinculado a uma carteira",
                UsuarioId = user.Id,
                CarteiraId = user.Carteira.Id,
                SaldoCarteira = user.Carteira.Saldo
            };

            return ResultadoOperacao<UsuarioResult>.SucessoResultado(usuarioResult);

        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            return ResultadoOperacao<UsuarioResult>.Falha($"Erro ao cadastrar Usuário: {ex.Message}");
        }
    }
}
