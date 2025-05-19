using CarteiraDigital.Core.Entities.Transacoes;
using CarteiraDigital.Core.Interfaces.Repositories;
using CarteiraDigital.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace CarteiraDigital.Infra.Repositories;

public class TransacaoRepository : ITransacaoRepository
{
    private readonly CarteiraDigitalDbContext _context;

    public TransacaoRepository(CarteiraDigitalDbContext context)
    {
        _context = context;
    }

    public async Task AdicionarAsync(Transacao transacao)
    {
        await _context.Transacoes.AddAsync(transacao);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Transacao>> GetTransacacoesPorUsuarioIdAsync(string usuarioId)
    {
        return await _context.Transacoes
               .Where(c => c.RemetenteId == usuarioId)
               .ToListAsync();
    }

    public async Task<Transacao> GetTransacaoPorIdAsync(Guid id)
    {
        return await _context.Transacoes
            .Include(r =>r.Remetente)
            .Include(d => d.Destinatario)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Transacao>> GetTransacoesPorUsuarioIdEDataAsync(string usuarioId, DateTime dataInicio, DateTime dataFim)
    {
        dataInicio = dataInicio.Date;
        dataFim = dataFim.Date.AddDays(1).AddTicks(-1);

        return await _context.Transacoes
                .Include(t => t.Remetente)
                .Include(t => t.Destinatario)
                .Where(t => (t.RemetenteId == usuarioId || t.DestinatarioId == usuarioId) &&
                            t.DataCriacao >= dataInicio && t.DataCriacao <= dataFim)
                .OrderByDescending(t => t.DataCriacao)
                .AsNoTracking()
                .ToListAsync();
    }

    public async Task AtualizarAsync(Transacao transacao)
    {
        _context.Transacoes.Update(transacao);
        await _context.SaveChangesAsync();
    }
}
