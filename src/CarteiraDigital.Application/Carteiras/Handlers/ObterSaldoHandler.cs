using CarteiraDigital.Application.Carteiras.Commands;
using CarteiraDigital.Application.Carteiras.Results;
using CarteiraDigital.Core.Interfaces.Repositories;
using CarteiraDigital.Core.Results;
using MediatR;

namespace CarteiraDigital.Application.Carteiras.Handlers;

public class ObterSaldoHandler : IRequestHandler<ObterSaldoCommand, ResultadoOperacao<SaldoCarteiraResult>>
{
    private readonly ICarteiraRepository _carteiraRepository;
    public ObterSaldoHandler(ICarteiraRepository carteiraRepository)
    {
        _carteiraRepository = carteiraRepository;
    }
    public async Task<ResultadoOperacao<SaldoCarteiraResult>> Handle(ObterSaldoCommand request, CancellationToken cancellationToken)
    {
        var carteira = await _carteiraRepository.ObterPorUsuarioAsync(request.UsuarioId);
        
        return ResultadoOperacao<SaldoCarteiraResult>.SucessoResultado(new SaldoCarteiraResult() { Saldo = carteira.Saldo });
    }
}
