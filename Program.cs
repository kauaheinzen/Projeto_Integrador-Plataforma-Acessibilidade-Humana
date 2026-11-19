using Google.Protobuf;
using PI.Models;

bool sair = false;
bool logado = false;
while (!sair)
{
    Console.Clear();
    Console.WriteLine("========= Sistema de Avaliação de Estabelecimentos e Caminhos =========");

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
                Console.WriteLine("========= Login =========");

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
                Console.WriteLine("========= Cadastro de Usuário =========");

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
                                    Console.WriteLine($"ID: {estabelecimento.Id} | Nome: {estabelecimento.Nome} | CNPJ: {estabelecimento.Cnpj} | Telefone: {estabelecimento.Telefone} | Latitude: {estabelecimento.Latitude} | Longitude: {estabelecimento.Longitude} | Média de Avaliação: {estabelecimento.MediaAvaliacao}");
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
                                        Console.WriteLine($"ID: {estabelecimento.Id} | Nome: {estabelecimento.Nome} | CNPJ: {estabelecimento.Cnpj} | Telefone: {estabelecimento.Telefone} | Latitude: {estabelecimento.Latitude} | Longitude: {estabelecimento.Longitude} | Média de Avaliação: {estabelecimento.MediaAvaliacao}");
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
                                Console.WriteLine("========= Busca por CNPJ =========");
                                Console.Write("Digite o CNPJ do estabelecimento: ");
                                var cnpjInput = Console.ReadLine();
                                var estabelecimento = await Estabelecimento.ObterEstabelecimentoPorIdOuCnpj(cnpjInput ?? string.Empty);
                                if (estabelecimento != null)
                                {
                                    Console.WriteLine($"ID: {estabelecimento.Id} | Nome: {estabelecimento.Nome} | CNPJ: {estabelecimento.Cnpj} | Telefone: {estabelecimento.Telefone} | Latitude: {estabelecimento.Latitude} | Longitude: {estabelecimento.Longitude} | Média de Avaliação: {estabelecimento.MediaAvaliacao}");
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
                            Console.WriteLine("Pressione qualquer tecla para continuar...");
                            Console.ReadKey();
                            break;
                        case "4":
                            try
                            {
                                Console.WriteLine("========= Atualização de Estabelecimento =========");
                                Console.Write("Digite o ID do estabelecimento que deseja atualizar: ");
                                var idInput = Console.ReadLine();
                                if (int.TryParse(idInput, out int id))
                                {
                                    var estabelecimentoExistente = await Estabelecimento.ObterEstabelecimentoPorIdOuCnpj(id.ToString());
                                    if (estabelecimentoExistente != null)
                                    {
                                        Console.Write($"Nome ({estabelecimentoExistente.Nome}): ");
                                        string nome = Console.ReadLine() ?? estabelecimentoExistente.Nome;
                                        Console.Write($"CNPJ ({estabelecimentoExistente.Cnpj}): ");
                                        string cnpj = Console.ReadLine() ?? estabelecimentoExistente.Cnpj;
                                        Console.Write($"Telefone ({estabelecimentoExistente.Telefone}): ");
                                        string? telefone = Console.ReadLine() ?? estabelecimentoExistente.Telefone;
                                        Console.Write($"Latitude ({estabelecimentoExistente.Latitude}): ");
                                        double latitude = double.TryParse(Console.ReadLine(), out double lat) ? lat : estabelecimentoExistente.Latitude;
                                        Console.Write($"Longitude ({estabelecimentoExistente.Longitude}): ");
                                        double longitude = double.TryParse(Console.ReadLine(), out double lon) ? lon : estabelecimentoExistente.Longitude;

                                        bool atualizacaoSucesso = await Estabelecimento.AtualizarEstabelecimento(id, nome, cnpj, latitude, longitude, telefone);
                                        if (atualizacaoSucesso)
                                        {
                                            Console.WriteLine("Estabelecimento atualizado com sucesso! Pressione qualquer tecla para continuar...");
                                            Console.ReadKey();
                                        }
                                        else
                                        {
                                            Console.WriteLine("Falha ao atualizar estabelecimento. Pressione qualquer tecla para continuar...");
                                            Console.ReadKey();
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Estabelecimento não encontrado. Pressione qualquer tecla para continuar...");
                                        Console.ReadKey();
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("ID inválido. Pressione qualquer tecla para continuar...");
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
                        case "5":
                            try
                            {
                                Console.WriteLine("========= Avaliação de Estabelecimento =========");
                                Console.Write("Digite o ID do estabelecimento que deseja avaliar: ");
                                var idInput = Console.ReadLine();
                                if (int.TryParse(idInput, out int id))
                                {
                                    var estabelecimentoExistente = await Estabelecimento.ObterEstabelecimentoPorIdOuCnpj(id.ToString());
                                    if (estabelecimentoExistente != null)
                                    {
                                        Console.Write("Digite a nota da avaliação (1 a 5): ");
                                        var notaInput = Console.ReadLine();
                                        if (int.TryParse(notaInput, out int nota) && nota >= 1 && nota <= 5)
                                        {
                                            Console.Write("Digite um comentário (opcional): ");
                                            string? comentario = Console.ReadLine();

                                            bool avaliacaoSucesso = await AvaliacaoEstabelecimentos.AdicionarAvaliacao(id, nota, comentario);
                                            if (avaliacaoSucesso)
                                            {
                                                Console.WriteLine("Avaliação registrada com sucesso! Pressione qualquer tecla para continuar...");
                                                Console.ReadKey();
                                            }
                                            else
                                            {
                                                Console.WriteLine("Falha ao registrar avaliação. Pressione qualquer tecla para continuar...");
                                                Console.ReadKey();
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Nota inválida. Pressione qualquer tecla para continuar...");
                                            Console.ReadKey();
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Estabelecimento não encontrado. Pressione qualquer tecla para continuar...");
                                        Console.ReadKey();
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("ID inválido. Pressione qualquer tecla para continuar...");
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
                        case "6":
                            try
                            {
                                Console.WriteLine("========= Lista de Avaliações de Estabelecimentos =========");
                                foreach (var estabelecimento in await Estabelecimento.ListarEstabelecimentos())
                                {
                                    Console.WriteLine($"ID: {estabelecimento.Id} | Nome: {estabelecimento.Nome} | CNPJ: {estabelecimento.Cnpj} | Telefone: {estabelecimento.Telefone} | Latitude: {estabelecimento.Latitude} | Longitude: {estabelecimento.Longitude} | Média de Avaliação: {estabelecimento.MediaAvaliacao}");
                                }
                                Console.Write("Digite o ID do estabelecimento para listar suas avaliações: ");
                                var avaliacaoId = Console.ReadLine();
                                var avaliacoes = await AvaliacaoEstabelecimentos.ListarAvaliacoes(int.TryParse(avaliacaoId, out int id) ? id : 0);
                                if (avaliacoes == null || avaliacoes.Count == 0)
                                {
                                    Console.WriteLine("Não existem avaliações cadastradas.");
                                }
                                else
                                {
                                    foreach (var avaliacao in avaliacoes)
                                    {
                                        Console.WriteLine($"ID: {avaliacao.Id} | Estabelecimento ID: {avaliacao.EstabelecimentoId} | Nota: {avaliacao.Nota} | Comentário: {avaliacao.Comentario}");
                                    }
                                }
                                Console.WriteLine("Pressione qualquer tecla para continuar...");
                                Console.ReadKey();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Ocorreu um erro: {ex.Message}");
                                Console.WriteLine("Pressione qualquer tecla para continuar...");
                                Console.ReadKey();
                            }
                            break;
                        case "7":
                            try
                            {
                                Console.WriteLine("========= Deletar Estabelecimento =========");
                                Console.Write("Digite o ID do estabelecimento que deseja deletar: ");
                                var idInput = Console.ReadLine();
                                if (int.TryParse(idInput, out int id))
                                {
                                    var estabelecimentoExistente = await Estabelecimento.ObterEstabelecimentoPorIdOuCnpj(id.ToString());
                                    if (estabelecimentoExistente != null)
                                    {
                                        bool deletarSucesso = await Estabelecimento.DeletarEstabelecimento(id);
                                        if (deletarSucesso)
                                        {
                                            Console.WriteLine("Estabelecimento deletado com sucesso! Pressione qualquer tecla para continuar...");
                                            Console.ReadKey();
                                        }
                                        else
                                        {
                                            Console.WriteLine("Falha ao deletar estabelecimento. Pressione qualquer tecla para continuar...");
                                            Console.ReadKey();
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Estabelecimento não encontrado. Pressione qualquer tecla para continuar...");
                                        Console.ReadKey();
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("ID inválido. Pressione qualquer tecla para continuar...");
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
                {
                    bool sairCaminho = false;
                    while (!sairCaminho)
                    {
                        Console.Clear();
                        Console.WriteLine("========= Menu Caminhos =========");
                        Console.WriteLine("1. Cadastrar caminho");
                        Console.WriteLine("2. Listar caminhos por estabelecimento");
                        Console.WriteLine("3. Buscar caminho por ID");
                        Console.WriteLine("4. Atualizar caminho");
                        Console.WriteLine("5. Deletar caminho");
                        Console.WriteLine("6. Voltar");
                        Console.Write("Escolha uma opção: ");
                        var opcaoCaminho = Console.ReadLine();

                        switch (opcaoCaminho)
                        {
                            case "1":
                                try
                                {
                                    Console.WriteLine("========= Cadastro de Caminho =========");
                                    var estabelecimentosDisponiveis = await Estabelecimento.ListarEstabelecimentos();
                                    if (estabelecimentosDisponiveis.Count == 0)
                                    {
                                        Console.WriteLine("Cadastre um estabelecimento antes de cadastrar um caminho.");
                                        AguardarTecla();
                                        break;
                                    }

                                    Console.WriteLine("Estabelecimentos disponíveis:");
                                    foreach (var item in estabelecimentosDisponiveis)
                                    {
                                        Console.WriteLine($"ID: {item.Id} | Nome: {item.Nome}");
                                    }

                                    int estabelecimentoId = LerId("ID do estabelecimento associado: ");
                                    var estabelecimento = await Estabelecimento.ObterEstabelecimentoPorIdOuCnpj(estabelecimentoId.ToString());
                                    if (estabelecimento == null)
                                    {
                                        Console.WriteLine("Estabelecimento não encontrado.");
                                        AguardarTecla();
                                        break;
                                    }

                                    double latitude = LerDouble("Latitude de partida: ");
                                    double longitude = LerDouble("Longitude de partida: ");
                                    double avaliacao = LerDouble("Avaliação (0 a 5): ");
                                    Console.Write("Descrição: ");
                                    string descricao = Console.ReadLine() ?? string.Empty;
                                    ValidarDadosCaminho(latitude, longitude, avaliacao, descricao);

                                    await Caminho.CadastrarCaminho(latitude, longitude, estabelecimento, avaliacao, descricao);
                                    Console.WriteLine("Caminho cadastrado com sucesso.");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Não foi possível cadastrar o caminho: {ex.Message}");
                                }
                                AguardarTecla();
                                break;

                            case "2":
                                try
                                {
                                    Console.WriteLine("========= Caminhos por Estabelecimento =========");
                                    var estabelecimentosDisponiveis = await Estabelecimento.ListarEstabelecimentos();
                                    foreach (var item in estabelecimentosDisponiveis)
                                    {
                                        Console.WriteLine($"ID: {item.Id} | Nome: {item.Nome}");
                                    }

                                    int estabelecimentoId = LerId("ID do estabelecimento: ");
                                    var caminhos = await Caminho.ListarCaminhosPorEstabelecimento(estabelecimentoId);
                                    if (caminhos.Count == 0)
                                    {
                                        Console.WriteLine("Nenhum caminho encontrado para esse estabelecimento.");
                                    }
                                    else
                                    {
                                        foreach (var caminho in caminhos)
                                        {
                                            ExibirCaminho(caminho);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Não foi possível listar os caminhos: {ex.Message}");
                                }
                                AguardarTecla();
                                break;

                            case "3":
                                try
                                {
                                    Console.WriteLine("========= Buscar Caminho =========");
                                    int caminhoId = LerId("ID do caminho: ");
                                    var caminho = await Caminho.ObterCaminhoPorId(caminhoId);
                                    if (caminho == null)
                                    {
                                        Console.WriteLine("Caminho não encontrado.");
                                    }
                                    else
                                    {
                                        ExibirCaminho(caminho);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Não foi possível buscar o caminho: {ex.Message}");
                                }
                                AguardarTecla();
                                break;

                            case "4":
                                try
                                {
                                    Console.WriteLine("========= Atualizar Caminho =========");
                                    int caminhoId = LerId("ID do caminho que deseja atualizar: ");
                                    var caminho = await Caminho.ObterCaminhoPorId(caminhoId);
                                    if (caminho == null)
                                    {
                                        Console.WriteLine("Caminho não encontrado.");
                                        AguardarTecla();
                                        break;
                                    }

                                    ExibirCaminho(caminho);
                                    double latitude = LerDoubleOpcional("Latitude de partida", caminho.LatitudePartida);
                                    double longitude = LerDoubleOpcional("Longitude de partida", caminho.LongitudePartida);
                                    double avaliacao = LerDoubleOpcional("Avaliação (0 a 5)", caminho.Avaliacao);
                                    Console.Write($"ID do estabelecimento ({caminho.Estabelecimento?.Id}): ");
                                    string estabelecimentoIdInput = Console.ReadLine() ?? string.Empty;
                                    int estabelecimentoId;
                                    if (string.IsNullOrWhiteSpace(estabelecimentoIdInput) && caminho.Estabelecimento != null)
                                    {
                                        estabelecimentoId = caminho.Estabelecimento.Id;
                                    }
                                    else if (!int.TryParse(estabelecimentoIdInput, out estabelecimentoId) || estabelecimentoId <= 0)
                                    {
                                        throw new ArgumentException("Informe um ID de estabelecimento válido.");
                                    }

                                    var estabelecimento = await Estabelecimento.ObterEstabelecimentoPorIdOuCnpj(estabelecimentoId.ToString());
                                    if (estabelecimento == null)
                                    {
                                        throw new ArgumentException("Estabelecimento não encontrado.");
                                    }

                                    string descricao = LerTextoOpcional("Descrição", caminho.Descricao);
                                    ValidarDadosCaminho(latitude, longitude, avaliacao, descricao);
                                    await Caminho.AtualizarCaminho(caminhoId, latitude, longitude, estabelecimento, avaliacao, descricao);
                                    Console.WriteLine("Caminho atualizado com sucesso.");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Não foi possível atualizar o caminho: {ex.Message}");
                                }
                                AguardarTecla();
                                break;

                            case "5":
                                try
                                {
                                    Console.WriteLine("========= Deletar Caminho =========");
                                    int caminhoId = LerId("ID do caminho que deseja deletar: ");
                                    Console.Write("Confirma a exclusão? (S/N): ");
                                    string confirmacao = Console.ReadLine() ?? string.Empty;
                                    if (!string.Equals(confirmacao.Trim(), "S", StringComparison.OrdinalIgnoreCase))
                                    {
                                        Console.WriteLine("Exclusão cancelada.");
                                    }
                                    else
                                    {
                                        bool deletado = await Caminho.DeletarCaminho(caminhoId);
                                        Console.WriteLine(deletado ? "Caminho deletado com sucesso." : "Caminho não encontrado.");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Não foi possível deletar o caminho: {ex.Message}");
                                }
                                AguardarTecla();
                                break;

                            case "6":
                                sairCaminho = true;
                                break;

                            default:
                                Console.WriteLine("Opção inválida.");
                                AguardarTecla();
                                break;
                        }
                    }
                }
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

static void AguardarTecla()
{
    Console.WriteLine("Pressione qualquer tecla para continuar...");
    Console.ReadKey();
}

static int LerId(string mensagem)
{
    Console.Write(mensagem);
    if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
    {
        throw new ArgumentException("ID inválido. Informe um número inteiro positivo.");
    }

    return id;
}

static double LerDouble(string mensagem)
{
    Console.Write(mensagem);
    if (!double.TryParse(Console.ReadLine(), out double valor) || !double.IsFinite(valor))
    {
        throw new ArgumentException("Valor inválido. Informe um número.");
    }

    return valor;
}

static double LerDoubleOpcional(string mensagem, double valorAtual)
{
    Console.Write($"{mensagem} ({valorAtual}): ");
    string entrada = Console.ReadLine() ?? string.Empty;
    if (string.IsNullOrWhiteSpace(entrada))
    {
        return valorAtual;
    }

    if (!double.TryParse(entrada, out double valor) || !double.IsFinite(valor))
    {
        throw new ArgumentException("Valor inválido. Informe um número ou deixe em branco para manter o atual.");
    }

    return valor;
}

static string LerTextoOpcional(string mensagem, string valorAtual)
{
    Console.Write($"{mensagem} ({valorAtual}): ");
    string entrada = Console.ReadLine() ?? string.Empty;
    return string.IsNullOrWhiteSpace(entrada) ? valorAtual : entrada.Trim();
}

static void ValidarDadosCaminho(double latitude, double longitude, double avaliacao, string descricao)
{
    if (latitude < -90 || latitude > 90)
    {
        throw new ArgumentException("A latitude deve estar entre -90 e 90.");
    }

    if (longitude < -180 || longitude > 180)
    {
        throw new ArgumentException("A longitude deve estar entre -180 e 180.");
    }

    if (avaliacao < 0 || avaliacao > 5)
    {
        throw new ArgumentException("A avaliação deve estar entre 0 e 5.");
    }

    if (string.IsNullOrWhiteSpace(descricao))
    {
        throw new ArgumentException("A descrição do caminho não pode ficar vazia.");
    }
}

static void ExibirCaminho(Caminho caminho)
{
    Console.WriteLine($"ID: {caminho.Id} | Estabelecimento: {caminho.Estabelecimento?.Nome ?? "não vinculado"} (ID: {caminho.Estabelecimento?.Id})");
    Console.WriteLine($"Partida: latitude {caminho.LatitudePartida}, longitude {caminho.LongitudePartida} | Avaliação: {caminho.Avaliacao}");
    Console.WriteLine($"Descrição: {caminho.Descricao}");
}
