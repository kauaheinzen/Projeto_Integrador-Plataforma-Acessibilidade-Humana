using PI.Data;
using Microsoft.EntityFrameworkCore;
namespace PI.Models;

public class Denuncia
{
    public int Id { get; set; }
    public string Descricao { get; private set; }
    public Caminho? Caminho { get; private set; }
    public Estabelecimento? Estabelecimento { get; private set; }
    public DateTime DataOcorrencia { get; private set; }

    public static async Task<bool> CadastrarDenuncia(string descricao, string dataOcorrencia, Caminho? caminho = null, Estabelecimento? estabelecimento = null)
    {
        await using (var db = new AppDbContext())
        {
            try
            {
                if (caminho == null && estabelecimento == null)
                {
                    throw new ArgumentException("É necessário fornecer um caminho ou um estabelecimento para a denúncia.");
                }
                if (descricao == null || descricao.Length < 10)
                {
                    throw new ArgumentException("A descrição da denúncia deve ter pelo menos 10 caracteres.");
                }
                if (!DateTime.TryParse(dataOcorrencia, out DateTime parsedDate))
                {
                    throw new ArgumentException("A data de ocorrência fornecida não é válida.");
                }
                
                var denuncia = new Denuncia
                {
                    Descricao = descricao,
                    Caminho = caminho,
                    Estabelecimento = estabelecimento,
                    DataOcorrencia = DateTime.Parse(dataOcorrencia)
                };

                db.Add(denuncia);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao cadastrar a denúncia: " + ex.Message);
            }
        }
    }
    public static async Task<List<Denuncia>> ListarDenuncias()
    {
        await using (var db = new AppDbContext())
        {
            try
            {
                return await db.Set<Denuncia>().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar as denúncias: " + ex.Message);
            }
        }
    }
    public static async Task<Denuncia> BuscarDenunciasPorId(int id)
    {
        await using (var db = new AppDbContext())
        {
            try
            {
                var denuncia = await db.Set<Denuncia>().FindAsync(id);
                if (denuncia == null)
                {
                    throw new Exception("Denúncia não encontrada.");
                }
                return denuncia;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar a denúncia: " + ex.Message);
            }
        }
    }
    public static async Task<bool> AtualizarDenuncia(int id, string descricao, string dataOcorrencia, Caminho? caminho = null, Estabelecimento? estabelecimento = null)
    {
        await using (var db = new AppDbContext())
        {
            try
            {
                var denuncia = await db.Set<Denuncia>().FindAsync(id);
                if (denuncia == null)
                {
                    throw new Exception("Denúncia não encontrada.");
                }

                if (descricao != null && descricao.Length >= 10)
                {
                    denuncia.Descricao = descricao;
                }
                else
                {
                    throw new ArgumentException("A descrição da denúncia deve ter pelo menos 10 caracteres.");
                }

                if (DateTime.TryParse(dataOcorrencia, out DateTime parsedDate))
                {
                    denuncia.DataOcorrencia = parsedDate;
                }
                else
                {
                    throw new ArgumentException("A data de ocorrência fornecida não é válida.");
                }

                denuncia.Caminho = caminho;
                denuncia.Estabelecimento = estabelecimento;

                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar a denúncia: " + ex.Message);
            }
        }
    }
    public static async Task<bool> DeletarDenuncia(int id)
    {
        await using (var db = new AppDbContext())
        {
            try
            {
                var denuncia = await db.Set<Denuncia>().FindAsync(id);
                if (denuncia == null)
                {
                    throw new Exception("Denúncia não encontrada.");
                }

                db.Remove(denuncia);
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao deletar a denúncia: " + ex.Message);
            }
        }
    }
}