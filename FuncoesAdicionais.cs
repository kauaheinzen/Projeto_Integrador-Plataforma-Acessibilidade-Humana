using PI.Models;
public class FuncoesAdicionais
{
    public static void AguardarTecla()
    {
        Console.WriteLine("Pressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

    public static int LerId(string mensagem)
    {
        Console.Write(mensagem);
        if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
        {
            throw new ArgumentException("ID inválido. Informe um número inteiro positivo.");
        }

        return id;
    }

    public static double LerDouble(string mensagem)
    {
        Console.Write(mensagem);
        if (!double.TryParse(Console.ReadLine(), out double valor) || !double.IsFinite(valor))
        {
            throw new ArgumentException("Valor inválido. Informe um número.");
        }

        return valor;
    }

    public static double LerDoubleOpcional(string mensagem, double valorAtual)
    {
        Console.Write($"{mensagem} ({valorAtual}): ");
        string entrada = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(entrada))
        {
            return valorAtual;
        }

        if (!double.TryParse(entrada, out double valor) || !double.IsFinite(valor))
        {
            throw new ArgumentException("Valor inválido. Informe um número ou deixe em branco para manter o atual.");
        }

        return valor;
    }

    public static string LerTextoOpcional(string mensagem, string valorAtual)
    {
        Console.Write($"{mensagem} ({valorAtual}): ");
        string entrada = Console.ReadLine() ?? string.Empty;
        return string.IsNullOrWhiteSpace(entrada) ? valorAtual : entrada.Trim();
    }

    public static void ValidarDadosCaminho(double latitude, double longitude, double avaliacao, string descricao)
    {
        if (latitude < -90 || latitude > 90)
        {
            throw new ArgumentException("A latitude deve estar entre -90 e 90.");
        }

        if (longitude < -180 || longitude > 180)
        {
            throw new ArgumentException("A longitude deve estar entre -180 e 180.");
        }

        if (avaliacao < 0 || avaliacao > 5)
        {
            throw new ArgumentException("A avaliação deve estar entre 0 e 5.");
        }

        if (string.IsNullOrWhiteSpace(descricao))
        {
            throw new ArgumentException("A descrição do caminho não pode ficar vazia.");
        }
    }

    public static void ExibirCaminho(Caminho caminho)
    {
        Console.WriteLine($"ID: {caminho.Id} | Estabelecimento: {caminho.Estabelecimento?.Nome ?? "não vinculado"} (ID: {caminho.Estabelecimento?.Id})");
        Console.WriteLine($"Partida: latitude {caminho.LatitudePartida}, longitude {caminho.LongitudePartida} | Avaliação: {caminho.Avaliacao}");
        Console.WriteLine($"Descrição: {caminho.Descricao}");
    }
}