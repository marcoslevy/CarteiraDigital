using CarteiraDigital.Core.Results;
using MediatR;

namespace CarteiraDigital.Application.Carteiras.Commands;

public class DepositarCommand : IRequest<ResultadoOperacaoTransacao>
{
    public string? UsuarioId { get; private set; }
    public decimal Valor { get; set; }
    public string Descricao { get; set; }

    public void SetUsuarioId(string usuarioId)
    {
        UsuarioId = usuarioId;
    }
}
