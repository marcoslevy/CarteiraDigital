using AutoMapper;
using CarteiraDigital.Application.Transacoes.Commands;
using CarteiraDigital.Application.Transacoes.Results;
using CarteiraDigital.Core.Interfaces.Repositories;
using CarteiraDigital.Core.Results;
using MediatR;

namespace CarteiraDigital.Application.Transacoes.Handlers;

public class ObterPorUsuarioEhDataHandler : IRequestHandler<ObterPorUsuarioEhDataCommand, ResultadoOperacao<IEnumerable<TransacaoResult>>>
{
    private readonly ITransacaoRepository _transacaoRepository;
    private readonly IMapper _mapper;

    public ObterPorUsuarioEhDataHandler(ITransacaoRepository transacaoRepository, IMapper mapper)
    {
        _transacaoRepository = transacaoRepository;
        _mapper = mapper;
    }

    public async Task<ResultadoOperacao<IEnumerable<TransacaoResult>>> Handle(ObterPorUsuarioEhDataCommand request, CancellationToken cancellationToken)
    {
        var transacao = await _transacaoRepository.GetTransacoesPorUsuarioIdEDataAsync(request.UsuarioId, request.DataInicio, request.DataFim);

        var result = _mapper.Map< IEnumerable<TransacaoResult>>(transacao);

        return ResultadoOperacao<IEnumerable<TransacaoResult>>.SucessoResultado(result);
    }
}
