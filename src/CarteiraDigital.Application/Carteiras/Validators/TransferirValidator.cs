using CarteiraDigital.Application.Carteiras.Commands;
using CarteiraDigital.Core.Results;
using MediatR;

namespace CarteiraDigital.Application.Carteiras.Validators;

public class TransferirValidator : IPipelineBehavior<TransferirCommand, ResultadoOperacaoTransacao>
{
    public async Task<ResultadoOperacaoTransacao> Handle(TransferirCommand request, RequestHandlerDelegate<ResultadoOperacaoTransacao> next, CancellationToken cancellationToken)
    {
        if (request.Valor <= 0)
            return ResultadoOperacaoTransacao.ValorInvalido();

        if (string.IsNullOrEmpty(request.UsuarioDestinoId))
            return ResultadoOperacaoTransacao.UsuarioInvalido();

        if(request.UsuarioId == request.UsuarioDestinoId)
            return ResultadoOperacaoTransacao.Falha("Transferência não pode ser feita para o mesmo usuário.");

        if (string.IsNullOrEmpty(request.Descricao))
            return ResultadoOperacaoTransacao.Falha("O campo Descricao deve ser informado");

        return await next();
    }
}
