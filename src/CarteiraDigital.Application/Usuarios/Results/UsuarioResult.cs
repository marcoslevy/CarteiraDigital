namespace CarteiraDigital.Application.Usuarios.Results;

public class UsuarioResult
{
    public required string Mensagem { get; set; }
    public required string UsuarioId { get; set; }
    public required Guid CarteiraId { get; set; }
    public required decimal SaldoCarteira { get; set; }
}
