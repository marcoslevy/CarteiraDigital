using CarteiraDigital.Application.Carteiras.Commands;
using CarteiraDigital.Core.Results;
using MediatR;

namespace CarteiraDigital.Application.Carteiras.Validators;

public class DepositarValidator : IPipelineBehavior<DepositarCommand, ResultadoOperacaoTransacao>
{
    public async Task<ResultadoOperacaoTransacao> Handle(DepositarCommand request, RequestHandlerDelegate<ResultadoOperacaoTransacao> next, CancellationToken cancellationToken)
    {
        if (request.Valor <= 0)
            return ResultadoOperacaoTransacao.ValorInvalido();

        if (string.IsNullOrEmpty(request.Descricao))
            return ResultadoOperacaoTransacao.Falha("O campo Descricao deve ser informado");

        return await next();
    }
}
