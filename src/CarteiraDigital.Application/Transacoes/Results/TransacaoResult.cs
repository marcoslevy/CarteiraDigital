namespace CarteiraDigital.Application.Transacoes.Results;

public class TransacaoResult
{
    public Guid Id { get; set; }
    public decimal Valor { get; set; }
    public TipoTransacao Tipo { get; set; }
    public string RemetenteId { get; set; }
    public string DestinatarioId { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataConclusao { get; set; }
    public string Descricao { get; set; }
    public StatusTransacao Status { get; set; }
}
