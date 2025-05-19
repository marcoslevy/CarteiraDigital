using CarteiraDigital.Core.Entities.Usuarios;

namespace CarteiraDigital.Core.Entities.Transacoes;

public class Transacao
{
    public Guid Id { get; private set; }
    public decimal Valor { get; private set; }
    public TipoTransacao Tipo { get; private set; }
    public Usuario Remetente { get; private set; }
    public string RemetenteId { get; set; }
    public Usuario Destinatario { get; private set; }
    public string DestinatarioId { get; set; }
    public DateTime DataCriacao { get; private set; }
    public DateTime? DataConclusao { get; private set; }
    public string Descricao { get; private set; }
    public StatusTransacao Status { get; private set; }

    public Transacao(
        decimal valor,
        TipoTransacao tipo,
        string remetenteId,
        string destinatarioId,
        string descricao)
    {
        Id = Guid.NewGuid();
        Valor = valor;
        Tipo = tipo;
        RemetenteId = remetenteId;
        DestinatarioId = destinatarioId;
        DataCriacao = DateTime.UtcNow;
        Descricao = descricao;
        Status = StatusTransacao.Pendente;
    }

    public void Confirmar()
    {
        Status = StatusTransacao.Concluida;
        DataConclusao = DateTime.UtcNow;
    }

    public void Cancelar(string motivo)
    {
        Status = StatusTransacao.Cancelada;
        Descricao += $". Motivo do cancelamento: {motivo}";
    }

    public void Falha(string erro)
    {
        Status = StatusTransacao.Falha;
        Descricao = erro;
    }
}
