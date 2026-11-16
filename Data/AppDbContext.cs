using PI.Models;
using Microsoft.EntityFrameworkCore;

namespace PI.Data;
public class AppDbContext : DbContext
{
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Estabelecimento> Estabelecimentos { get; set; }
    public DbSet<Caminho> Caminhos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL(
            @"server=localhost;
            database=PI_Acessibilidade_Urbana;
            user=root;
            password=Senac2026");
    }
}