using PI.Models;

bool sair = false;
bool logado = false;
while (!sair)
{
    Console.Clear();
    Console.WriteLine("Sistema de Avaliação de Estabelecimentos e Caminhos");

    Console.WriteLine("1. Login");
    Console.WriteLine("2. Cadastrar Usuário");
    Console.WriteLine("3. Sair");
    Console.Write("Escolha uma opção: ");
    var opcao = Console.ReadLine();
    switch (opcao)
    {
        case "1":
            try
            {
                Console.WriteLine("Login");

                Console.Write("Email: ");
                string e_mail = Console.ReadLine() ?? string.Empty;
                Console.Write("Senha: ");
                string senha = Console.ReadLine() ?? string.Empty;

                Usuario? usuario = await Usuario.AutenticarUsuario(e_mail, senha);
                if (usuario != null)
                {
                    Console.WriteLine($"Bem-vindo, {usuario.Nome}!");
                    logado = true;
                }
                else
                {
                    Console.WriteLine("Email ou senha inválidos. Pressione qualquer tecla para continuar...");
                    Console.ReadKey();
                }
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro: {ex.Message}");
                Console.WriteLine("Pressione qualquer tecla para continuar...");
                Console.ReadKey();
                break;
            }
        case "2":
            try
            {
                Console.WriteLine("Cadastro de Usuário");

                Console.Write("Nome: ");
                string nome = Console.ReadLine() ?? string.Empty;
                Console.Write("CPF: ");
                string cpf = Console.ReadLine() ?? string.Empty;
                Console.Write("Email: ");
                string Email = Console.ReadLine() ?? string.Empty;
                Console.Write("Senha: ");
                string Senha = Console.ReadLine() ?? string.Empty;
                Console.Write("Telefone (opcional): ");
                string? telefone = Console.ReadLine();
                Console.Write("Endereço (opcional): ");
                string? endereco = Console.ReadLine();

                bool cadastroSucesso = await Usuario.CadastrarUsuario(nome, cpf, Email, Senha, telefone, endereco);
                if (cadastroSucesso)
                {
                    Console.WriteLine("Usuário cadastrado com sucesso! Pressione qualquer tecla para continuar...");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Falha ao cadastrar usuário. Pressione qualquer tecla para continuar...");
                    Console.ReadKey();
                }
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro: {ex.Message}");
                Console.WriteLine("Pressione qualquer tecla para continuar...");
                Console.ReadKey();
                break;
            }
        case "3":
            sair = true;
            break;
        default:
            Console.WriteLine("Opção inválida. Pressione qualquer tecla para continuar...");
            Console.ReadKey();
            break;
    }
    
    while (logado)
    {
        Console.Clear();
        Console.WriteLine("Menu Principal");
        Console.WriteLine("1. Estabelecimento");
        Console.WriteLine("2. Caminho");
        Console.WriteLine("3. Logout");
        Console.Write("Escolha uma opção: ");
        var opcaoLogado = Console.ReadLine();
        switch (opcaoLogado)
        {
            case "1":
                // Lógica para avaliar estabelecimento
                break;
            case "2":
                // Lógica para avaliar caminho
                break;
            case "3":
                logado = false;
                break;
            default:
                Console.WriteLine("Opção inválida. Pressione qualquer tecla para continuar...");
                Console.ReadKey();
                break;
        }
    }
}