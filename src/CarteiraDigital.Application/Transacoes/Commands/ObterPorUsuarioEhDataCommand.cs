using CarteiraDigital.Application.Transacoes.Results;
using CarteiraDigital.Core.Results;
using MediatR;

namespace CarteiraDigital.Application.Transacoes.Commands;

public class ObterPorUsuarioEhDataCommand : IRequest<ResultadoOperacao<IEnumerable<TransacaoResult>>>
{
    public string? UsuarioId { get; private set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }

    public void SetUsuarioId(string usuarioId)
    {
        UsuarioId = usuarioId;
    }
}
