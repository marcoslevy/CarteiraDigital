namespace CarteiraDigital.Application.Usuarios.Results;

public class LoginResult
{
    public required string Token { get; set; }
    public required DateTime ExpiraEm { get; set; }
    public required string UsuarioId { get; set; }
    public required string UsuarioEmail { get; set; }
}
