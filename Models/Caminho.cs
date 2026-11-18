using PI.Data;
using Microsoft.EntityFrameworkCore;
namespace PI.Models;

public class Caminho
{
    public int Id { get; set; }
    public double LatitudePartida { get; set; }
    public double LongitudePartida { get; set; }
    public Estabelecimento? Estabelecimento { get; set; }
    public double Avaliacao { get; set; }
    public string Descricao { get; set; } = string.Empty;

    public static async Task<bool> CadastrarCaminho(double latitudePartida, double longitudePartida, Estabelecimento estabelecimento, double avaliacao, string descricao)
    {
        await using (var context = new AppDbContext())
        {
            if (avaliacao < 0 || avaliacao > 5)
            {
                throw new ArgumentException("A avaliação deve estar entre 0 e 5.");
            }
            if (latitudePartida < -90 || latitudePartida > 90)
            {
                throw new ArgumentException("A latitude deve estar entre -90 e 90.");
            }
            if (longitudePartida < -180 || longitudePartida > 180)
            {
                throw new ArgumentException("A longitude deve estar entre -180 e 180.");
            }

            var caminho = new Caminho
            {
                LatitudePartida = Math.Round(latitudePartida, 4),
                LongitudePartida = Math.Round(longitudePartida, 4),
                Estabelecimento = estabelecimento,
                Avaliacao = Math.Round(avaliacao, 1),
                Descricao = descricao
            };

            context.Add(caminho);
            await context.SaveChangesAsync();

            return true;
        }
    }
    public static async Task<Caminho?> ObterCaminhoPorId(int id)
    {
        await using (var context = new AppDbContext())
        {
            return await context.Set<Caminho>().Include(c => c.Estabelecimento).FirstOrDefaultAsync(c => c.Id == id);
        }
    }
    public static async Task<List<Caminho>> ListarCaminhosPorEstabelecimento(int estabelecimentoId)
    {
        await using (var context = new AppDbContext())
        {
            return await context.Set<Caminho>()
                .Include(c => c.Estabelecimento)
                .Where(c => c.Estabelecimento != null && c.Estabelecimento.Id == estabelecimentoId)
                .ToListAsync();
        }
    }
    public static async Task<bool> AtualizarCaminho(int id, double latitudePartida, double longitudePartida, Estabelecimento estabelecimento, double avaliacao, string descricao)
    {
        await using (var context = new AppDbContext())
        {
            var caminho = await context.Set<Caminho>().Include(c => c.Estabelecimento).FirstOrDefaultAsync(c => c.Id == id);
            if (caminho == null)
            {
                throw new Exception("Caminho não encontrado.");
            }

            caminho.LatitudePartida = Math.Round(latitudePartida, 4);
            caminho.LongitudePartida = Math.Round(longitudePartida, 4);
            caminho.Estabelecimento = estabelecimento;
            caminho.Avaliacao = Math.Round(avaliacao, 1);
            caminho.Descricao = descricao;

            await context.SaveChangesAsync();

            return true;
        }
    }
    public static async Task<bool> DeletarCaminho(int id)
    {
        await using (var context = new AppDbContext())
        {
            var caminho = await context.Set<Caminho>().FindAsync(id);
            if (caminho == null)
            {
                return false;
            }

            context.Remove(caminho);
            await context.SaveChangesAsync();

            return true;
        }
    }
}