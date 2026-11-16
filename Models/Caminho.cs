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

    public static async Task<Caminho> CadastrarCaminho(double latitudePartida, double longitudePartida, Estabelecimento estabelecimento, double avaliacao, string descricao)
    {
        await using (var context = new AppDbContext())
        {
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

            return caminho;
        }
    }
    public static async Task<List<Caminho>> ListarCaminhos()
    {
        await using (var context = new AppDbContext())
        {
            return await context.Set<Caminho>().Include(c => c.Estabelecimento).ToListAsync();
        }
    }
}