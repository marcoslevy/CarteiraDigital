using CarteiraDigital.Application.Carteiras.Commands;
using CarteiraDigital.Core.Results;
using FluentAssertions;
using MediatR;

namespace CarteiraDigital.Test;

public class DepositarCommandTests
{
    [Fact]
    public void SetUsuarioId_DeveAtribuirValorCorretamente()
    {
        // Arrange
        var command = new DepositarCommand();
        var usuarioId = "usuario1";

        // Act
        command.SetUsuarioId(usuarioId);

        // Assert
        command.UsuarioId.Should().Be(usuarioId);
    }

    [Fact]
    public void DepositarCommand_DeveImplementarIRequest()
    {
        // Arrange & Act
        var command = new DepositarCommand();

        // Assert
        command.Should().BeAssignableTo<IRequest<ResultadoOperacaoTransacao>>();
    }

    [Theory]
    [InlineData(100.50, "Depósito inicial")]
    [InlineData(50.25, "Recarga")]
    public void Propriedades_DevemSerAtribuidasCorretamente(decimal valor, string descricao)
    {
        // Arrange & Act
        var command = new DepositarCommand
        {
            Valor = valor,
            Descricao = descricao
        };

        // Assert
        command.Valor.Should().Be(valor);
        command.Descricao.Should().Be(descricao);
    }
}
