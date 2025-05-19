using CarteiraDigital.Core.Entities.Usuarios;

namespace CarteiraDigital.Core.Entities.Carteiras;

public class Carteira
{
    public Guid Id { get; private set; }
    public string UsuarioId { get; set; }
    public Usuario Usuario { get; private set; }
    public decimal Saldo { get; private set; }

    public Carteira() { }

    public Carteira(Usuario usuario)
    {
        Id = Guid.NewGuid();
        Usuario = usuario;
        Saldo = 0;
    }

    public void Depositar(decimal valor)
    {
        if (valor <= 0)
            throw new ArgumentException("O valor do depósito deve ser positivo");

        Saldo += valor;
    }

    public void Sacar(decimal valor)
    {
        if (valor <= 0)
            throw new ArgumentException("O valor do saque deve ser positivo");

        if (Saldo < valor)
            throw new InvalidOperationException("Saldo insuficiente");

        Saldo -= valor;
    }
}
