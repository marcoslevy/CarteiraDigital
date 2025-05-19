using CarteiraDigital.Application.Transacoes.Commands;
using CarteiraDigital.Core.Results;
using MediatR;

namespace CarteiraDigital.Application.Transacoes.Validations;

public class ObterPorIdValidator : IPipelineBehavior<ObterPorIdCommand, ResultadoOperacaoTransacao>
{
    public async Task<ResultadoOperacaoTransacao> Handle(ObterPorIdCommand request, RequestHandlerDelegate<ResultadoOperacaoTransacao> next, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.TransacaoId.ToString()))
            return ResultadoOperacaoTransacao.Falha("O Id da transação deve ser informado");

        return await next();
    }
}
