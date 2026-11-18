using PI.Data;
using Microsoft.EntityFrameworkCore;
namespace PI.Models;
public class Usuario
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cpf
    {
        // fazer validador de cpf
        get { return Cpf; }
        set
        {
            if (value.Length != 11)
            {
                throw new ArgumentException("O CPF deve ter 11 dígitos.");
            }
            Cpf = new string(value.Where(char.IsDigit).ToArray());
        }
    }
    public string Email { get; set; }
    public string Senha { get; set; }
    public string? Telefone { get; set; }
    public string? Endereco { get; set; }

    private static bool CpfValido(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return false;

        cpf = new string(cpf.Where(char.IsDigit).ToArray());

        if (cpf.Length != 11 || cpf.All(c => c == cpf[0]))
            return false;

        int soma = 0;

        for (int i = 0; i < 9; i++)
            soma += (cpf[i] - '0') * (10 - i);

        int resto = soma % 11;
        int digito1 = resto < 2 ? 0 : 11 - resto;

        if (cpf[9] - '0' != digito1)
            return false;

        soma = 0;

        for (int i = 0; i < 10; i++)
            soma += (cpf[i] - '0') * (11 - i);

        resto = soma % 11;
        int digito2 = resto < 2 ? 0 : 11 - resto;

        if (cpf[10] - '0' != digito2)
            return false;

        return true;
    }

    public static async Task<bool> CadastrarUsuario(string nome, string cpf, string email, string senha, string? telefone = null, string? endereco = null)
    {
        await using (var db = new AppDbContext())
        {
            var usuarioExistente = await db.Usuarios.FirstOrDefaultAsync(u => u.Cpf == cpf || u.Email == email);
            if (usuarioExistente != null)
            {
                throw new InvalidOperationException("Já existe um usuário com o mesmo CPF ou e-mail.");
            }
            if (!CpfValido(cpf))
            {
                throw new ArgumentException("CPF inválido.");
            }
            var usuario = new Usuario
            {
                Nome = nome,
                Cpf = cpf,
                Email = email,
                Senha = senha,
                Telefone = telefone,
                Endereco = endereco
            };
            db.Usuarios.Add(usuario);
            await db.SaveChangesAsync();

            return true;
        }
    }
    public static async Task<bool> AtualizarUsuario(int id, string? nome = null, string? cpf = null, string? email = null, string? senha = null, string? telefone = null, string? endereco = null)
    {
        await using (var db = new AppDbContext())
        {
            var usuario = await db.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                throw new InvalidOperationException("Usuário não encontrado.");
            }

            if (!string.IsNullOrWhiteSpace(nome))
            {
                usuario.Nome = nome;
            }
            else
            {
                throw new ArgumentException("O nome não pode ser nulo ou vazio.");
            }

            if (!string.IsNullOrWhiteSpace(cpf))
            {
                if (!CpfValido(cpf))
                {
                    throw new ArgumentException("CPF inválido.");
                }
                usuario.Cpf = cpf;
            }
            else
            {
                throw new ArgumentException("O CPF não pode ser nulo ou vazio.");
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                usuario.Email = email;
            }
            else
            {
                throw new ArgumentException("O e-mail não pode ser nulo ou vazio.");
            }

            if (!string.IsNullOrWhiteSpace(senha))
            {
                usuario.Senha = senha;
            }
            else
            {
                throw new ArgumentException("A senha não pode ser nula ou vazia.");
            }

            if (!string.IsNullOrWhiteSpace(telefone))
            {
                usuario.Telefone = telefone;
            }
            else
            {
                throw new ArgumentException("O telefone não pode ser nulo ou vazio.");
            }
            
            if (!string.IsNullOrWhiteSpace(endereco))
            {
                usuario.Endereco = endereco;
            }
            else
            {
                throw new ArgumentException("O endereço não pode ser nulo ou vazio.");
            }
            
            await db.SaveChangesAsync();
            
            return true;
        }
    }
    public static async Task<Usuario?> AutenticarUsuario(string email, string senha)
    {
        await using (var db = new AppDbContext())
        {
            var usuario = await db.Usuarios.FirstOrDefaultAsync(u => u.Email == email && u.Senha == senha);
            return usuario.Id != 0 ? usuario : null;
        }
    }
    public static async Task<bool> DeletarUsuario(int id)
    {
        await using (var db = new AppDbContext())
        {
            var usuario = await db.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                db.Usuarios.Remove(usuario);
                await db.SaveChangesAsync();
            }
            return true;
        }
    }
}