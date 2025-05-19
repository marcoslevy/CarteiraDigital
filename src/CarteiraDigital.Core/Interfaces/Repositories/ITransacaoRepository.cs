using CarteiraDigital.Core.Entities.Transacoes;

namespace CarteiraDigital.Core.Interfaces.Repositories;

public interface ITransacaoRepository
{
    Task<Transacao> GetTransacaoPorIdAsync(Guid id);
    Task<IEnumerable<Transacao>> GetTransacacoesPorUsuarioIdAsync(string usuarioId);
    Task<IEnumerable<Transacao>> GetTransacoesPorUsuarioIdEDataAsync(string usuarioId, DateTime dataInicio, DateTime dataFim);
    Task AdicionarAsync(Transacao transacao);
    Task AtualizarAsync(Transacao transacao);
}
