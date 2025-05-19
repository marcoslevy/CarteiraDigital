using CarteiraDigital.Core.Entities.Carteiras;
using CarteiraDigital.Core.Interfaces.Repositories;
using CarteiraDigital.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace CarteiraDigital.Infra.Repositories;

public class CarteiraRepository : ICarteiraRepository
{
    private readonly CarteiraDigitalDbContext _context;

    public CarteiraRepository(CarteiraDigitalDbContext context)
    {
        _context = context;
    }

    public async Task AtualizarAsync(Carteira carteira)
    {
        _context.Carteiras.Update(carteira);
        await _context.SaveChangesAsync();
    }

    public async Task<Carteira> ObterPorUsuarioAsync(string usuarioId)
    {
        return await _context.Carteiras
               .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId.ToString());
    }
}
