using PI.Data;
using Microsoft.EntityFrameworkCore;
namespace PI.Models;

public class Estabelecimento
{
    public int Id { get; set; }
    public string Nome { get; private set; } = string.Empty;
    public string Cnpj { get; private set; }
    public string? Telefone { get; private set; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }

    public double MediaAvaliacao { get; set; }
    public List<AvaliacaoEstabelecimentos>? Avaliacoes { get; private set; }

    private static bool ValidarCnpj(string cnpj)
    {
        cnpj = cnpj.Trim()
                .Replace(".", "")
                .Replace("/", "")
                .Replace("-", "")
                .ToUpper();

        if (cnpj.Length != 14)
            throw new ArgumentException("O CNPJ deve ter 14 caracteres.");

        if (!cnpj.Take(12).All(c =>
            (c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z')))
            throw new ArgumentException("CNPJ possui caracteres inválidos.");

        if (!cnpj.Substring(12).All(char.IsDigit))
            throw new ArgumentException("Os dois últimos caracteres devem ser números.");

        if (cnpj.All(c => c == cnpj[0]))
            throw new ArgumentException("CNPJ inválido.");

        int[] pesos = { 2, 3, 4, 5, 6, 7, 8, 9 };

        // verificador 1
        int soma = 0;
        int indice = 0;

        for (int i = 11; i >= 0; i--)
        {
            int valor = cnpj[i] - '0';
            soma += valor * pesos[indice % 8];
            indice++;
        }

        int resto = soma % 11;
        int digito1 = resto < 2 ? 0 : 11 - resto;

        if (cnpj[12] - '0' != digito1)
            throw new ArgumentException("Primeiro dígito verificador inválido.");

        // verificador 2
        soma = 0;
        indice = 0;

        for (int i = 12; i >= 0; i--)
        {
            int valor = cnpj[i] - '0';
            soma += valor * pesos[indice % 8];
            indice++;
        }

        resto = soma % 11;
        int digito2 = resto < 2 ? 0 : 11 - resto;

        if (cnpj[13] - '0' != digito2)
            throw new ArgumentException("Segundo dígito verificador inválido.");

        return true;
    }

    public static async Task<bool> CadastrarEstabelecimento(string nome, string cnpj, double latitude, double longitude,  string? telefone = null)
    {
        await using (var db = new AppDbContext())
        {
            if (await db.Set<Estabelecimento>().AnyAsync(e => e.Cnpj == cnpj))
            {
                throw new ArgumentException("CNPJ já cadastrado.");
            }

            if (latitude < -90 || latitude > 90)
            {
                throw new ArgumentException("A latitude deve estar entre -90 e 90.");
            }

            if (longitude < -180 || longitude > 180)
            {
                throw new ArgumentException("A longitude deve estar entre -180 e 180.");
            }

            if (string.IsNullOrWhiteSpace(nome))
            {
                throw new ArgumentException("O nome do estabelecimento não pode ser vazio.");
            }

            if (ValidarCnpj(cnpj) == false)
            {
                throw new ArgumentException("CNPJ inválido.");
            }

            var estabelecimento = new Estabelecimento
            {
                Nome = nome,
                Cnpj = cnpj,
                Telefone = telefone,
                Latitude = Math.Round(latitude, 4),
                Longitude = Math.Round(longitude, 4),
            };

            db.Set<Estabelecimento>().Add(estabelecimento);
            await db.SaveChangesAsync();

            return true;
        }
    }
    public static async Task<Estabelecimento?> ObterEstabelecimentoPorIdOuCnpj(string id_cnpj)
    {
        await using (var db = new AppDbContext())
        {
            try
            {
            if (int.TryParse(id_cnpj, out int id))
            {
                return await db.Set<Estabelecimento>().FindAsync(id);
            }
            else
            {
                return await db.Set<Estabelecimento>().FirstOrDefaultAsync(e => e.Cnpj == id_cnpj);
            }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter estabelecimento: " + ex.Message);
            }
        }
    }
    public static async Task<bool> AtualizarEstabelecimento(int id, string? nome = null, string? cnpj = null, double? latitude = null, double? longitude = null, string? telefone = null)
    {
        await using (var db = new AppDbContext())
        {
            var estabelecimento = await db.Set<Estabelecimento>().FindAsync(id);
            if (estabelecimento == null)
            {
                throw new Exception("Estabelecimento não encontrado.");
            }

            if (!string.IsNullOrWhiteSpace(nome))
            {
                estabelecimento.Nome = nome;
            }
            else
            {
                throw new ArgumentException("O nome do estabelecimento não pode ser nulo ou vazio.");
            }

            if (!string.IsNullOrWhiteSpace(cnpj))
            {
                if (await db.Set<Estabelecimento>().AnyAsync(e => e.Cnpj == cnpj && e.Id != id))
                {
                    throw new ArgumentException("CNPJ já cadastrado.");
                }
                if (ValidarCnpj(cnpj) == false)
                {
                    throw new ArgumentException("CNPJ inválido.");
                }
                estabelecimento.Cnpj = cnpj;
            }
            else
            {
                throw new ArgumentException("O CNPJ não pode ser nulo ou vazio.");
            }

            if (latitude.HasValue)
            {
                if (latitude < -90 || latitude > 90)
                {
                    throw new ArgumentException("A latitude deve estar entre -90 e 90.");
                }
                estabelecimento.Latitude = Math.Round(latitude.Value, 4);
            }
            else
            {
                throw new ArgumentException("A latitude não pode ser nula.");
            }

            if (longitude.HasValue)
            {
                if (longitude < -180 || longitude > 180)
                {
                    throw new ArgumentException("A longitude deve estar entre -180 e 180.");
                }
                estabelecimento.Longitude = Math.Round(longitude.Value, 4);
            }
            else
            {
                throw new ArgumentException("A longitude não pode ser nula.");
            }

            if (!string.IsNullOrWhiteSpace(telefone))
            {
                estabelecimento.Telefone = telefone;
            }
            else
            {
                throw new ArgumentException("O telefone não pode ser nulo ou vazio.");
            }

            await db.SaveChangesAsync();
            return true;
        }
    }
    public static async Task<bool> DeletarEstabelecimento(int id)
    {
        await using (var db = new AppDbContext())
        {
            var estabelecimento = await db.Set<Estabelecimento>().FindAsync(id);
            if (estabelecimento != null)
            {
                db.Set<Estabelecimento>().Remove(estabelecimento);
                await db.SaveChangesAsync();
            }
            return true;
        }
    }
    public static async Task<List<Estabelecimento>> ListarEstabelecimentos()
    {
        try
        {
            await using (var db = new AppDbContext())
            {
                return await db.Set<Estabelecimento>().ToListAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao listar estabelecimentos: " + ex.Message);
        }
    }
}