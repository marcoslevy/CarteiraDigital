using AutoMapper;
using CarteiraDigital.Application.Carteiras.Commands;
using CarteiraDigital.Application.Transacoes.Results;
using CarteiraDigital.Core.Entities.Transacoes;
using CarteiraDigital.Core.Interfaces;
using CarteiraDigital.Core.Interfaces.Repositories;
using CarteiraDigital.Core.Results;
using MediatR;

namespace CarteiraDigital.Application.Carteiras.Handlers;

public class SacarHandler : IRequestHandler<SacarCommand, ResultadoOperacaoTransacao>
{
    private readonly ICarteiraRepository _carteiraRepository;
    private readonly ITransacaoRepository _transacaoRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SacarHandler(ICarteiraRepository carteiraRepository, ITransacaoRepository transacaoRepository, IUsuarioRepository usuarioRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _carteiraRepository = carteiraRepository;
        _transacaoRepository = transacaoRepository;
        _usuarioRepository = usuarioRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResultadoOperacaoTransacao> Handle(SacarCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var carteira = await _carteiraRepository.ObterPorUsuarioAsync(request.UsuarioId);
            if (carteira == null)
                return ResultadoOperacaoTransacao.CarteiraNaoEncontrada();

            if (carteira.Saldo < request.Valor)
                return ResultadoOperacaoTransacao.SaldoInsuficiente();

            var usuario = await _usuarioRepository.ObterPorIdAsync(request.UsuarioId);

            var transacao = new Transacao(
                request.Valor,
                TipoTransacao.Saque,
                request.UsuarioId,
                request.UsuarioId,
                request.Descricao);

            carteira.Sacar(request.Valor);
            transacao.Confirmar();

            await _carteiraRepository.AtualizarAsync(carteira);
            await _transacaoRepository.AdicionarAsync(transacao);

            await _unitOfWork.CommitAsync();

            var result = _mapper.Map<TransacaoResult>(transacao);

            return ResultadoOperacaoTransacao.SucessoResultado(result);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            return ResultadoOperacaoTransacao.Falha($"Erro ao processar saque: {ex.Message}");
        }
    }
}
