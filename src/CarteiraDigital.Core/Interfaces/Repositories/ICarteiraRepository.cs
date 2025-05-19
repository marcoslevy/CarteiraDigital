using CarteiraDigital.Core.Entities.Carteiras;

namespace CarteiraDigital.Core.Interfaces.Repositories;

public interface ICarteiraRepository
{
    Task<Carteira> ObterPorUsuarioAsync(string usuarioId);
    Task AtualizarAsync(Carteira carteira);
}
