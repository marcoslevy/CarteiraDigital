using System.ComponentModel.DataAnnotations;

namespace CarteiraDigital.API.Models;

public class TransactionCreateRequest
{
    [Required(ErrorMessage = "Receiver ID is required")]
    public Guid ReceiverId { get; set; }

    [Required(ErrorMessage = "Amount is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public decimal Amount { get; set; }

    [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
    public string Description { get; set; }
}


public class CriarTransacaoDto
{
    public Guid Id { get; set; }
    public decimal Valor { get; set; }
    public string Descricao { get; set; }
}

public class TransacaoResponse
{
    public Guid TransacaoId { get; set; }
    public decimal Valor { get; set; }
    public string NomeRementente { get; set; }
    public string NomeRecebedor { get; set; }
    public DateTime DataTransacao { get; set; }
    public string Status { get; set; }
    public string Descricao { get; set; }
}

public class TransactionQueryRequest
{
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
}
