using AutoMapper;
using CarteiraDigital.Core.Interfaces.Repositories;
using CarteiraDigital.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using CarteiraDigital.Application.Carteiras.Handlers;
using CarteiraDigital.Application.Carteiras.Commands;
using CarteiraDigital.Core.Entities.Carteiras;
using FluentAssertions;
using CarteiraDigital.Application.Transacoes.Results;
using CarteiraDigital.Core.Entities.Transacoes;
using CarteiraDigital.Core.Entities.Usuarios;

namespace CarteiraDigital.Test;

public class DepositarHandlerTests
{
    private readonly Mock<ICarteiraRepository> _carteiraRepositoryMock;
    private readonly Mock<ITransacaoRepository> _transacaoRepositoryMock;
    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly DepositarHandler _handler;

    public DepositarHandlerTests()
    {
        _carteiraRepositoryMock = new Mock<ICarteiraRepository>();
        _transacaoRepositoryMock = new Mock<ITransacaoRepository>();
        _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();

        _handler = new DepositarHandler(
            _carteiraRepositoryMock.Object,
            _transacaoRepositoryMock.Object,
            _usuarioRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_CarteiraNaoEncontrada_DeveRetornarFalha()
    {
        // Arrange
        var command = new DepositarCommand
        {
            Valor = 100,
            Descricao = "Depósito"
        };

        command.SetUsuarioId("usuario1");

        _carteiraRepositoryMock.Setup(x => x.ObterPorUsuarioAsync(command.UsuarioId))
            .ReturnsAsync((Carteira)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Falhou.Should().BeTrue();
        result.MensagemErro.Should().Contain("Carteira não encontrada");
        _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(), Times.Once);
        _unitOfWorkMock.Verify(x => x.RollbackAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_DepositoValido_DeveRetornarSucesso()
    {
        // Arrange
        var command = new DepositarCommand
        {
            Valor = 100,
            Descricao = "Depósito"
        };

        command.SetUsuarioId("usuario1");

        var carteira = new Carteira();
        var usuario = new Usuario { Id = command.UsuarioId };
        var transacao = new Transacao(command.Valor, TipoTransacao.Deposito, command.UsuarioId, command.UsuarioId, command.Descricao);
        var transacaoResult = new TransacaoResult();

        _carteiraRepositoryMock.Setup(x => x.ObterPorUsuarioAsync(command.UsuarioId))
            .ReturnsAsync(carteira);

        _usuarioRepositoryMock.Setup(x => x.ObterPorIdAsync(command.UsuarioId))
            .ReturnsAsync(usuario);

        _transacaoRepositoryMock.Setup(x => x.AdicionarAsync(It.IsAny<Transacao>()))
            .Returns(Task.CompletedTask);

        _carteiraRepositoryMock.Setup(x => x.AtualizarAsync(carteira))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(x => x.CommitAsync())
            .Returns(Task.CompletedTask);

        _mapperMock.Setup(x => x.Map<TransacaoResult>(It.IsAny<Transacao>()))
            .Returns(transacaoResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.FoiBemSucedido.Should().BeTrue();
        result.Dado.Should().Be(transacaoResult);
        carteira.Saldo.Should().Be(100);
        _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once);
        _unitOfWorkMock.Verify(x => x.RollbackAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_ErroDuranteProcessamento_DeveFazerRollback()
    {
        // Arrange
        var command = new DepositarCommand
        {
            Valor = 100,
            Descricao = "Depósito"
        };

        command.SetUsuarioId("usuario-123");

        var carteira = new Carteira();
        var usuario = new Usuario { Id = command.UsuarioId };

        _carteiraRepositoryMock.Setup(x => x.ObterPorUsuarioAsync(command.UsuarioId))
            .ReturnsAsync(carteira);

        _usuarioRepositoryMock.Setup(x => x.ObterPorIdAsync(command.UsuarioId))
            .ReturnsAsync(usuario);

        _carteiraRepositoryMock.Setup(x => x.AtualizarAsync(carteira))
            .ThrowsAsync(new Exception("Simulando erro"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Falhou.Should().BeTrue();
        result.MensagemErro.Should().Contain("Erro ao processar depósito");
        _unitOfWorkMock.Verify(x => x.RollbackAsync(), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Never);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-100)]
    public async Task Handle_ValorInvalido_DeveRetornarFalha(decimal valorInvalido)
    {
        // Arrange
        var command = new DepositarCommand
        {
            Valor = valorInvalido,
            Descricao = "Depósito inválido"
        };

        command.SetUsuarioId("usuario-123");

        var carteira = new Carteira();

        _carteiraRepositoryMock.Setup(x => x.ObterPorUsuarioAsync(command.UsuarioId))
            .ReturnsAsync(carteira);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Falhou.Should().BeTrue();
        result.MensagemErro.Should().Contain("Erro ao processar depósito");
        _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Never);
    }
}
