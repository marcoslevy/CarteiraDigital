using CarteiraDigital.Core.Entities.Usuarios;
using CarteiraDigital.Core.Interfaces.Repositories;
using CarteiraDigital.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace CarteiraDigital.Infra.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly CarteiraDigitalDbContext _context;

    public UsuarioRepository(CarteiraDigitalDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario> ObterPorIdAsync(string usuarioId)
    {
        return await _context.Usuarios
            .Include(c => c.Carteira)
            .FirstOrDefaultAsync(c => c.Id == usuarioId.ToString());
    }
}
