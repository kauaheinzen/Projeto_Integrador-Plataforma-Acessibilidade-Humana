using PI.Data;
using Microsoft.EntityFrameworkCore;
namespace PI.Models;

public class AvaliacaoEstabelecimentos
{
    public int Id { get; set; }

    public int EstabelecimentoId { get; set; }
    public Estabelecimento? Estabelecimento { get; set; }

    public double Nota { get; set; }

    public static async Task<bool> AdicionarAvaliacao(int estabelecimentoId, double nota)
    {
        await using (var db = new AppDbContext())
        {
            var avaliacao = new AvaliacaoEstabelecimentos
            {
                EstabelecimentoId = estabelecimentoId,
                Nota = nota
            };

            var estabelecimento = await db.Set<Estabelecimento>()
                .FirstOrDefaultAsync(e => e.Id == estabelecimentoId);

            if (estabelecimento == null)
                throw new Exception("Estabelecimento não encontrado.");


            db.Add(avaliacao);
            await db.SaveChangesAsync();


            estabelecimento.MediaAvaliacao = await db.Set<AvaliacaoEstabelecimentos>()
                .Where(a => a.EstabelecimentoId == estabelecimentoId)
                .AverageAsync(a => a.Nota);

            
            await db.SaveChangesAsync();

            return true;
        }
    }
    public static async Task<List<AvaliacaoEstabelecimentos>> ListarAvaliacoes(int estabelecimentoId)
    {
        await using (var context = new AppDbContext())
        {
            return await context.Set<AvaliacaoEstabelecimentos>()
                .Where(a => a.EstabelecimentoId == estabelecimentoId)
                .Include(a => a.Estabelecimento)
                .ToListAsync();
        }
    }
}