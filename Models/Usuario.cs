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
            
        }
    }
        

    public string Email { get; set; }
    public string Senha { get; set; }
    public string? Telefone { get; set; }
    public string? Endereco { get; set; }
}