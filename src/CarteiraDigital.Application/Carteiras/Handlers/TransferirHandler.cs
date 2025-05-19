using AutoMapper;
using CarteiraDigital.Application.Carteiras.Commands;
using CarteiraDigital.Application.Transacoes.Results;
using CarteiraDigital.Core.Entities.Transacoes;
using CarteiraDigital.Core.Interfaces;
using CarteiraDigital.Core.Interfaces.Repositories;
using CarteiraDigital.Core.Results;
using MediatR;

namespace CarteiraDigital.Application.Carteiras.Handlers;

public class TransferirHandler : IRequestHandler<TransferirCommand, ResultadoOperacaoTransacao>
{
    private readonly ICarteiraRepository _carteiraRepository;
    private readonly ITransacaoRepository _transacaoRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TransferirHandler(ICarteiraRepository carteiraRepository, ITransacaoRepository transacaoRepository, IUsuarioRepository usuarioRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _carteiraRepository = carteiraRepository;
        _transacaoRepository = transacaoRepository;
        _usuarioRepository = usuarioRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResultadoOperacaoTransacao> Handle(TransferirCommand request, CancellationToken cancellationToken)
    {
        var rementente = await _usuarioRepository.ObterPorIdAsync(request.UsuarioId);
        var destinatario = await _usuarioRepository.ObterPorIdAsync(request.UsuarioDestinoId);

        if (rementente == null || destinatario == null)
            return ResultadoOperacaoTransacao.UsuarioNaoEncontrado();

        if (rementente.Carteira.Saldo < request.Valor)
            return ResultadoOperacaoTransacao.SaldoInsuficiente();

        var transacao = new Transacao(request.Valor, TipoTransacao.Transferencia, rementente.Id, destinatario.Id, request.Descricao);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            rementente.Carteira.Sacar( request.Valor);
            destinatario.Carteira.Depositar(request.Valor);
            transacao.Confirmar();

            await _carteiraRepository.AtualizarAsync(rementente.Carteira);
            await _carteiraRepository.AtualizarAsync(destinatario.Carteira);
            await _transacaoRepository.AdicionarAsync(transacao);

            await _unitOfWork.CommitAsync();

            var result = _mapper.Map<TransacaoResult>(transacao);

            return ResultadoOperacaoTransacao.SucessoResultado(result);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            transacao.Falha(ex.Message);
            await _transacaoRepository.AdicionarAsync(transacao);
            throw;
        }
    }
}
