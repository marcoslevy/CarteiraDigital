using CarteiraDigital.Core.Results;
using MediatR;

namespace CarteiraDigital.Application.Carteiras.Commands;

public class TransferirCommand : IRequest<ResultadoOperacaoTransacao>
{
    public string? UsuarioId { get; private set; }
    public string UsuarioDestinoId { get; set; }
    public decimal Valor { get; set; }
    public string Descricao { get; set; }
    public void SetUsuarioId(string usuarioId)
    {
        UsuarioId = usuarioId;
    }
}
