using CarteiraDigital.Application.Transacoes.Commands;
using CarteiraDigital.Core.Entities.Transacoes;
using CarteiraDigital.Core.Results;
using MediatR;

namespace CarteiraDigital.Application.Transacoes.Validations;

public class ObterPorUsuarioEhDataValidator : IPipelineBehavior<ObterPorUsuarioEhDataCommand, ResultadoOperacao<IEnumerable<Transacao>>>
{
    public async Task<ResultadoOperacao<IEnumerable<Transacao>>> Handle(ObterPorUsuarioEhDataCommand request, RequestHandlerDelegate<ResultadoOperacao<IEnumerable<Transacao>>> next, CancellationToken cancellationToken)
    {
        if (request.DataInicio > request.DataFim)
            return ResultadoOperacao<IEnumerable<Transacao>>.Falha("A data de início não pode ser maior que a data fim");

        return await next();
    }
}
