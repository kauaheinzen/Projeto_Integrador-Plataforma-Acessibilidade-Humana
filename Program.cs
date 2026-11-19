using Google.Protobuf;
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
                bool sairEstabelecimento = false;
                while (!sairEstabelecimento)
                {
                    Console.Clear();
                    Console.WriteLine("========= Menu Estabelecimento =========");
                    Console.WriteLine("1. Registrar Estabelecimento");
                    Console.WriteLine("2. Listar Estabelecimentos");
                    Console.WriteLine("3. Obter Estabelecimento por ID ou CNPJ");
                    Console.WriteLine("4. Atualizar Estabelecimento");
                    Console.WriteLine("5. Avaliar Estabelecimento");
                    Console.WriteLine("6. Listar Avaliações de Estabelecimentos");
                    Console.WriteLine("7. Deletar Estabelecimento");
                    Console.WriteLine("8. Voltar");
                    Console.Write("Escolha uma opção: ");
                    var opcaoEstabelecimento = Console.ReadLine();
                    switch (opcaoEstabelecimento)
                    {
                        case "1":
                        try
                        {
                                Console.WriteLine("========= Registro de Estabelecimento =========");

                                Console.Write("Nome: ");
                                string nome = Console.ReadLine() ?? string.Empty;
                                Console.Write("CNPJ: ");
                                string cnpj = Console.ReadLine() ?? string.Empty;
                                Console.Write("Telefone (opcional): ");
                                string? telefone = Console.ReadLine();
                                Console.Write("Latitude: ");
                                double latitude = double.Parse(Console.ReadLine());
                                Console.Write("Longitude: ");
                                double longitude = double.Parse(Console.ReadLine());

                                bool registroSucesso = await Estabelecimento.CadastrarEstabelecimento(nome, cnpj, latitude, longitude, telefone);
                                if (registroSucesso)
                                {
                                    Console.WriteLine("Estabelecimento registrado com sucesso! Pressione qualquer tecla para continuar...");
                                    Console.ReadKey();
                                }
                                else
                                {
                                    Console.WriteLine("Falha ao registrar estabelecimento. Pressione qualquer tecla para continuar...");
                                    Console.ReadKey();
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Ocorreu um erro: {ex.Message}");
                                Console.WriteLine("Pressione qualquer tecla para continuar...");
                                Console.ReadKey();
                            }
                            break;
                        case "2":
                            Console.WriteLine("========= Lista de Estabelecimentos =========");
                            var estabelecimentos = await Estabelecimento.ListarEstabelecimentos();
                            if (estabelecimentos == null || estabelecimentos.Count == 0)
                            {
                                Console.WriteLine("Não existem estabelecimentos cadastrados.");
                            }
                            else
                            {
                                foreach (var estabelecimento in estabelecimentos)
                                {
                                    Console.WriteLine($"ID: {estabelecimento.Id} | Nome: {estabelecimento.Nome} | CNPJ: {estabelecimento.Cnpj} | Telefone: {estabelecimento.Telefone} | Latitude: {estabelecimento.Latitude} | Longitude: {estabelecimento.Longitude}");
                                }
                            }
                            Console.WriteLine("Pressione qualquer tecla para continuar...");
                            Console.ReadKey();
                            break;
                        case "3":
                            Console.Write("Você gostaria de buscar o estabelecimento por ID ou CNPJ? (Digite '1' para ID ou '2' para CNPJ)");
                            var buscaOpcao = Console.ReadLine();
                            if (buscaOpcao == "1")
                            {
                                Console.Write("========= Busca por ID =========");
                                Console.Write("Digite o ID do estabelecimento: ");
                                var idInput = Console.ReadLine();
                                if (int.TryParse(idInput, out int id))
                                {
                                    var estabelecimento = await Estabelecimento.ObterEstabelecimentoPorIdOuCnpj(id.ToString());
                                    if (estabelecimento != null)
                                    {
                                        Console.WriteLine($"ID: {estabelecimento.Id} | Nome: {estabelecimento.Nome} | CNPJ: {estabelecimento.Cnpj} | Telefone: {estabelecimento.Telefone} | Latitude: {estabelecimento.Latitude} | Longitude: {estabelecimento.Longitude}");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Estabelecimento não encontrado.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("ID inválido.");
                                }
                            }
                            else if (buscaOpcao == "2")
                            {
                                Console.Write("Digite o CNPJ do estabelecimento: ");
                                var cnpjInput = Console.ReadLine();
                                var estabelecimento = await Estabelecimento.ObterEstabelecimentoPorIdOuCnpj(cnpjInput ?? string.Empty);
                                if (estabelecimento != null)
                                {
                                    Console.WriteLine($"ID: {estabelecimento.Id} | Nome: {estabelecimento.Nome} | CNPJ: {estabelecimento.Cnpj} | Telefone: {estabelecimento.Telefone} | Latitude: {estabelecimento.Latitude} | Longitude: {estabelecimento.Longitude}");
                                }
                                else
                                {
                                    Console.WriteLine("Estabelecimento não encontrado.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Opção inválida.");
                            }
                            break;
                        case "4":
                            
                            break;
                        case "5":
                            
                            break;
                        case "6":
                            
                            break;
                        case "7":
                            
                            break;
                        case "8":
                            sairEstabelecimento = true;
                            break;
                        default:
                            Console.WriteLine("Opção inválida. Pressione qualquer tecla para continuar...");
                            Console.ReadKey();
                            break;
                    }
                }
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