namespace CarteiraDigital.API.Models;

public class UsuarioDto
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class UsuarioResponseDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public DateTime DataCriacao { get; set; }
}

public class UsuarioLoginDto
{
    public string NomeUsuario { get; set; }
    public string Ssenha { get; set; }
}
