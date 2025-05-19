using CarteiraDigital.Application.Carteiras.Results;
using CarteiraDigital.Core.Results;
using MediatR;

namespace CarteiraDigital.Application.Carteiras.Commands;

public class ObterSaldoCommand : IRequest<ResultadoOperacao<SaldoCarteiraResult>>
{
    public string UsuarioId { get; private set; }

    public ObterSaldoCommand(string usuarioId)
    {
        UsuarioId = usuarioId;
    }
}
