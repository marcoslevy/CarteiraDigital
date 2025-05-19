using CarteiraDigital.Core.Entities.Usuarios;

namespace CarteiraDigital.Core.Interfaces.Repositories;

public interface IUsuarioRepository
{
    Task<Usuario> ObterPorIdAsync(string usuarioId);

    //Task<Usuario> BuscarUsuarioWithWalletLockedAsync(Guid userId);
}
