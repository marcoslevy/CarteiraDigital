using CarteiraDigital.Application.Transacoes.Results;

namespace CarteiraDigital.Core.Results;

public class ResultadoOperacaoTransacao : ResultadoOperacao<TransacaoResult>
{
    private ResultadoOperacaoTransacao(bool sucesso, TransacaoResult transacao, string mensagemErro, string codigoErro)
        : base(sucesso, transacao, mensagemErro, codigoErro)
    {
    }

    public static ResultadoOperacaoTransacao SucessoResultado(TransacaoResult transacao)
    {
        return new ResultadoOperacaoTransacao(true, transacao, null, null);
    }

    public static ResultadoOperacaoTransacao Falha(string mensagemErro, string codigoErro = null)
    {
        return new ResultadoOperacaoTransacao(false, null, mensagemErro, codigoErro);
    }

    public static new ResultadoOperacaoTransacao SaldoInsuficiente()
    {
        return new ResultadoOperacaoTransacao(
            sucesso: false,
            transacao: null,
            mensagemErro: "Saldo insuficiente para completar a operação",
            codigoErro: "SALDO_INSUFICIENTE");
    }

    public static new ResultadoOperacaoTransacao ValorInvalido()
    {
        return new ResultadoOperacaoTransacao(
            sucesso: false,
            transacao: null,
            mensagemErro: "O valor da operação deve ser positivo",
            codigoErro: "VALOR_INVALIDO");
    }

    public static new ResultadoOperacaoTransacao CarteiraNaoEncontrada()
    {
        return new ResultadoOperacaoTransacao(
            sucesso: false,
            transacao: null,
            mensagemErro: "Carteira não encontrada",
            codigoErro: "CARTAIRA_NAO_ENCONTRADA");
    }

    public static new ResultadoOperacaoTransacao UsuarioInvalido()
    {
        return new ResultadoOperacaoTransacao(
            sucesso: false,
            transacao: null,
            mensagemErro: "O Usuário deve ser informado",
            codigoErro: "USUARIO_INVALIDO");
    }

    public static new ResultadoOperacaoTransacao UsuarioNaoEncontrado()
    {
        return new ResultadoOperacaoTransacao(
            sucesso: false,
            transacao: null,
            mensagemErro: "Usuário não encontrado",
            codigoErro: "USUARIO_NAO_ENCONTRADO");
    }

    public static new ResultadoOperacaoTransacao LimiteExcedido()
    {
        return new ResultadoOperacaoTransacao(
            sucesso: false,
            transacao: null,
            mensagemErro: "Limite de operação excedido",
            codigoErro: "LIMITE_EXCEDIDO");
    }
}
