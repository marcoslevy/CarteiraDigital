using CarteiraDigital.Core.Results;
using MediatR;

namespace CarteiraDigital.Application.Transacoes.Commands;

public class ObterPorIdCommand : IRequest<ResultadoOperacaoTransacao>
{
    public Guid TransacaoId { get; set; }

    public ObterPorIdCommand(Guid transacaoId)
    {
        TransacaoId = transacaoId;
    }
}
