namespace CarteiraDigital.Core.Results;

public class ResultadoOperacao<T> where T : class
{
    public bool Sucesso { get; }
    public T Dado { get; }
    public string MensagemErro { get; }
    public string CodigoErro { get; }

    protected ResultadoOperacao(bool sucesso, T dado, string mensagemErro, string codigoErro)
    {
        Sucesso = sucesso;
        Dado = dado;
        MensagemErro = mensagemErro;
        CodigoErro = codigoErro;
    }

    public static ResultadoOperacao<T> SucessoResultado(T dado)
    {
        return new ResultadoOperacao<T>(
            sucesso: true,
            dado: dado,
            mensagemErro: null,
            codigoErro: null);
    }

    public static ResultadoOperacao<T> Falha(string mensagemErro, string codigoErro = null)
    {
        return new ResultadoOperacao<T>(
            sucesso: false,
            dado: null,
            mensagemErro: mensagemErro,
            codigoErro: codigoErro ?? "ERRO_GENERICO");
    }

    public static ResultadoOperacao<T> Falha(T dado, string mensagemErro, string codigoErro = null)
    {
        return new ResultadoOperacao<T>(
            sucesso: false,
            dado: dado,
            mensagemErro: mensagemErro,
            codigoErro: codigoErro ?? "ERRO_GENERICO");
    }

    public bool Falhou => !Sucesso;
    public bool FoiBemSucedido => Sucesso;

    public override string ToString()
    {
        return Sucesso
            ? $"Operação bem-sucedida. Dado: {Dado?.ToString()}"
            : $"Falha na operação: {MensagemErro} (Código: {CodigoErro})";
    }
}
