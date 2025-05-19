using AutoMapper;
using CarteiraDigital.Application.Transacoes.Commands;
using CarteiraDigital.Application.Transacoes.Results;
using CarteiraDigital.Core.Interfaces.Repositories;
using CarteiraDigital.Core.Results;
using MediatR;

namespace CarteiraDigital.Application.Transacoes.Handlers;

public class ObterPorIdHandler : IRequestHandler<ObterPorIdCommand, ResultadoOperacaoTransacao>
{
    private readonly ITransacaoRepository _transacaoRepository;
    private readonly IMapper _mapper;

    public ObterPorIdHandler(ITransacaoRepository transacaoRepository, IMapper mapper)
    {
        _transacaoRepository = transacaoRepository;
        _mapper = mapper;
    }

    public async Task<ResultadoOperacaoTransacao> Handle(ObterPorIdCommand request, CancellationToken cancellationToken)
    {
        var transacao = await _transacaoRepository.GetTransacaoPorIdAsync(request.TransacaoId);

        var result = _mapper.Map<TransacaoResult>(transacao);

        return ResultadoOperacaoTransacao.SucessoResultado(result);
    }
}
