using CarteiraDigital.Core.Entities.Carteiras;
using CarteiraDigital.Core.Entities.Transacoes;
using Microsoft.AspNetCore.Identity;

namespace CarteiraDigital.Core.Entities.Usuarios;

public class Usuario : IdentityUser
{
    public DateTime DataCriacao { get; private set; }
    public Carteira Carteira { get; private set; }
    public ICollection<Transacao> TransacoesEnviadas { get; private set; }
    public ICollection<Transacao> TransacoesRecebidas { get; private set; }

    public Usuario()
    {
        TransacoesEnviadas = [];
        TransacoesRecebidas = [];
    }

    public Usuario(string email)
    {
        UserName = email;   
        Email = email;        
        DataCriacao = DateTime.UtcNow;
        Carteira = new Carteira(this);
        TransacoesEnviadas = [];
        TransacoesRecebidas = [];
    }
}
