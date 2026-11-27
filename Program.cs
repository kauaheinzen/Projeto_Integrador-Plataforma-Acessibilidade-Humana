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

                var usuario = await Usuario.AutenticarUsuario(e_mail, senha);
                if (usuario != null)
                {
                    Console.WriteLine($"Bem-vindo, {usuario.Nome}!");
                    logado = true;
                    FuncoesAdicionais.AguardarTecla();
                }
                else
                {
                    Console.WriteLine("Email ou senha inválidos.");
                    FuncoesAdicionais.AguardarTecla();
                }
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro: {ex.Message}");
                FuncoesAdicionais.AguardarTecla();
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
                    Console.WriteLine("Usuário cadastrado com sucesso!");
                    FuncoesAdicionais.AguardarTecla();
                }
                else
                {
                    Console.WriteLine("Falha ao cadastrar usuário.");
                    FuncoesAdicionais.AguardarTecla();
                }
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro: {ex.Message}");
                FuncoesAdicionais.AguardarTecla();
                break;
            }
        case "3":
            sair = true;
            break;
        default:
            Console.WriteLine("Opção inválida.");
            FuncoesAdicionais.AguardarTecla();
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
                                double latitude = FuncoesAdicionais.LerDouble("Latitude: ");
                                double longitude = FuncoesAdicionais.LerDouble("Longitude: ");

                                bool registroSucesso = await Estabelecimento.CadastrarEstabelecimento(nome, cnpj, latitude, longitude, telefone);
                                if (registroSucesso)
                                {
                                    Console.WriteLine("Estabelecimento registrado com sucesso!");
                                    FuncoesAdicionais.AguardarTecla();
                                }
                                else
                                {
                                    Console.WriteLine("Falha ao registrar estabelecimento.");
                                    FuncoesAdicionais.AguardarTecla();
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Ocorreu um erro: {ex.Message}");
                                FuncoesAdicionais.AguardarTecla();
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
                            FuncoesAdicionais.AguardarTecla();
                            break;
                        case "3":
                            Console.Write("Você gostaria de buscar o estabelecimento por ID ou CNPJ? (Digite '1' para ID ou '2' para CNPJ): ");
                            var buscaOpcao = Console.ReadLine();
                            if (buscaOpcao == "1")
                            {
                                Console.WriteLine("========= Busca por ID =========");
                                try
                                {
                                    int id = FuncoesAdicionais.LerId("Digite o ID do estabelecimento: ");
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
                                catch (ArgumentException)
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
                            FuncoesAdicionais.AguardarTecla();
                            break;
                        case "4":
                            try
                            {
                                Console.WriteLine("========= Atualização de Estabelecimento =========");
                                int id = FuncoesAdicionais.LerId("Digite o ID do estabelecimento que deseja atualizar: ");
                                var estabelecimentoExistente = await Estabelecimento.ObterEstabelecimentoPorIdOuCnpj(id.ToString());
                                if (estabelecimentoExistente == null)
                                {
                                    Console.WriteLine("Estabelecimento não encontrado.");
                                    FuncoesAdicionais.AguardarTecla();
                                }
                                else
                                {
                                    string nome = FuncoesAdicionais.LerTextoOpcional("Nome", estabelecimentoExistente.Nome);
                                    string cnpj = FuncoesAdicionais.LerTextoOpcional("CNPJ", estabelecimentoExistente.Cnpj);
                                    string? telefone = FuncoesAdicionais.LerTextoOpcional("Telefone", estabelecimentoExistente.Telefone ?? string.Empty);
                                    double latitude = FuncoesAdicionais.LerDoubleOpcional("Latitude", estabelecimentoExistente.Latitude);
                                    double longitude = FuncoesAdicionais.LerDoubleOpcional("Longitude", estabelecimentoExistente.Longitude);

                                    bool atualizacaoSucesso = await Estabelecimento.AtualizarEstabelecimento(id, nome, cnpj, latitude, longitude, telefone);
                                    if (atualizacaoSucesso)
                                    {
                                        Console.WriteLine("Estabelecimento atualizado com sucesso!");
                                        FuncoesAdicionais.AguardarTecla();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Falha ao atualizar estabelecimento.");
                                        FuncoesAdicionais.AguardarTecla();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Ocorreu um erro: {ex.Message}");
                                FuncoesAdicionais.AguardarTecla();
                            }
                            break;
                        case "5":
                            try
                            {
                                Console.WriteLine("========= Avaliação de Estabelecimento =========");
                                int id = FuncoesAdicionais.LerId("Digite o ID do estabelecimento que deseja avaliar: ");
                                var estabelecimentoExistente = await Estabelecimento.ObterEstabelecimentoPorIdOuCnpj(id.ToString());
                                if (estabelecimentoExistente == null)
                                {
                                    Console.WriteLine("Estabelecimento não encontrado.");
                                    FuncoesAdicionais.AguardarTecla();
                                }
                                else
                                {
                                    Console.Write("Digite a nota da avaliação (1 a 5): ");
                                    var notaInput = Console.ReadLine();
                                    notaInput = notaInput?.Replace('.',',');
                                    if (double.TryParse(notaInput, out double nota) && nota >= 1 && nota <= 5)
                                    {
                                        Console.Write("Digite um comentário (opcional): ");
                                        string? comentario = Console.ReadLine();

                                        bool avaliacaoSucesso = await AvaliacaoEstabelecimentos.AdicionarAvaliacao(id, nota, comentario);
                                        if (avaliacaoSucesso)
                                        {
                                            Console.WriteLine("Avaliação registrada com sucesso!");
                                            FuncoesAdicionais.AguardarTecla();
                                        }
                                        else
                                        {
                                            Console.WriteLine("Falha ao registrar avaliação.");
                                            FuncoesAdicionais.AguardarTecla();
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Nota inválida.");
                                        FuncoesAdicionais.AguardarTecla();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Ocorreu um erro: {ex.Message}");
                                FuncoesAdicionais.AguardarTecla();
                            }
                            break;
                        case "6":
                            try
                            {
                                Console.WriteLine("========= Lista de Estabelecimentos =========");
                                foreach (var estabelecimento in await Estabelecimento.ListarEstabelecimentos())
                                {
                                    Console.WriteLine($"ID: {estabelecimento.Id} | Nome: {estabelecimento.Nome} | CNPJ: {estabelecimento.Cnpj} | Telefone: {estabelecimento.Telefone} | Latitude: {estabelecimento.Latitude} | Longitude: {estabelecimento.Longitude} | Média de Avaliação: {estabelecimento.MediaAvaliacao}");
                                }
                                int estabelecimentoId = FuncoesAdicionais.LerId("Digite o ID do estabelecimento para listar suas avaliações: ");
                                var avaliacoes = await AvaliacaoEstabelecimentos.ListarAvaliacoes(estabelecimentoId);
                                Console.WriteLine("========= Avaliações do Estabelecimento =========");
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
                                FuncoesAdicionais.AguardarTecla();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Ocorreu um erro: {ex.Message}");
                                FuncoesAdicionais.AguardarTecla();
                            }
                            break;
                        case "7":
                            try
                            {
                                Console.WriteLine("========= Deletar Estabelecimento =========");
                                int id = FuncoesAdicionais.LerId("Digite o ID do estabelecimento que deseja deletar: ");
                                var estabelecimentoExistente = await Estabelecimento.ObterEstabelecimentoPorIdOuCnpj(id.ToString());
                                if (estabelecimentoExistente == null)
                                {
                                    Console.WriteLine("Estabelecimento não encontrado.");
                                    FuncoesAdicionais.AguardarTecla();
                                }
                                else
                                {
                                    bool deletarSucesso = await Estabelecimento.DeletarEstabelecimento(id);
                                    if (deletarSucesso)
                                    {
                                        Console.WriteLine("Estabelecimento deletado com sucesso!");
                                        FuncoesAdicionais.AguardarTecla();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Falha ao deletar estabelecimento.");
                                        FuncoesAdicionais.AguardarTecla();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Ocorreu um erro: {ex.Message}");
                                FuncoesAdicionais.AguardarTecla();
                            }
                            break;
                        case "8":
                            sairEstabelecimento = true;
                            break;
                        default:
                            Console.WriteLine("Opção inválida.");
                            FuncoesAdicionais.AguardarTecla();
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
                                        FuncoesAdicionais.AguardarTecla();
                                        break;
                                    }

                                    Console.WriteLine("Estabelecimentos disponíveis:");
                                    foreach (var item in estabelecimentosDisponiveis)
                                    {
                                        Console.WriteLine($"ID: {item.Id} | Nome: {item.Nome}");
                                    }

                                    int estabelecimentoId = FuncoesAdicionais.LerId("ID do estabelecimento associado: ");
                                    var estabelecimento = await Estabelecimento.ObterEstabelecimentoPorIdOuCnpj(estabelecimentoId.ToString());
                                    if (estabelecimento == null)
                                    {
                                        Console.WriteLine("Estabelecimento não encontrado.");
                                        FuncoesAdicionais.AguardarTecla();
                                        break;
                                    }

                                    double latitude = FuncoesAdicionais.LerDouble("Latitude de partida: ");
                                    double longitude = FuncoesAdicionais.LerDouble("Longitude de partida: ");
                                    double avaliacao = FuncoesAdicionais.LerDouble("Avaliação (0 a 5): ");
                                    Console.Write("Descrição: ");
                                    string descricao = Console.ReadLine() ?? string.Empty;
                                    FuncoesAdicionais.ValidarDadosCaminho(latitude, longitude, avaliacao, descricao);

                                    await Caminho.CadastrarCaminho(latitude, longitude, estabelecimento, avaliacao, descricao);
                                    Console.WriteLine("Caminho cadastrado com sucesso.");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Não foi possível cadastrar o caminho: {ex.Message}");
                                }
                                FuncoesAdicionais.AguardarTecla();
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

                                    int estabelecimentoId = FuncoesAdicionais.LerId("ID do estabelecimento: ");
                                    var caminhos = await Caminho.ListarCaminhosPorEstabelecimento(estabelecimentoId);
                                    if (caminhos.Count == 0)
                                    {
                                        Console.WriteLine("Nenhum caminho encontrado para esse estabelecimento.");
                                    }
                                    else
                                    {
                                        foreach (var caminho in caminhos)
                                        {
                                            FuncoesAdicionais.ExibirCaminho(caminho);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Não foi possível listar os caminhos: {ex.Message}");
                                }
                                FuncoesAdicionais.AguardarTecla();
                                break;

                            case "3":
                                try
                                {
                                    Console.WriteLine("========= Buscar Caminho =========");
                                    int caminhoId = FuncoesAdicionais.LerId("ID do caminho: ");
                                    var caminho = await Caminho.ObterCaminhoPorId(caminhoId);
                                    if (caminho == null)
                                    {
                                        Console.WriteLine("Caminho não encontrado.");
                                    }
                                    else
                                    {
                                        FuncoesAdicionais.ExibirCaminho(caminho);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Não foi possível buscar o caminho: {ex.Message}");
                                }
                                FuncoesAdicionais.AguardarTecla();
                                break;

                            case "4":
                                try
                                {
                                    Console.WriteLine("========= Atualizar Caminho =========");
                                    int caminhoId = FuncoesAdicionais.LerId("ID do caminho que deseja atualizar: ");
                                    var caminho = await Caminho.ObterCaminhoPorId(caminhoId);
                                    if (caminho == null)
                                    {
                                        Console.WriteLine("Caminho não encontrado.");
                                        FuncoesAdicionais.AguardarTecla();
                                        break;
                                    }

                                    FuncoesAdicionais.ExibirCaminho(caminho);
                                    double latitude = FuncoesAdicionais.LerDoubleOpcional("Latitude de partida", caminho.LatitudePartida);
                                    double longitude = FuncoesAdicionais.LerDoubleOpcional("Longitude de partida", caminho.LongitudePartida);
                                    double avaliacao = FuncoesAdicionais.LerDoubleOpcional("Avaliação (0 a 5)", caminho.Avaliacao);
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


                                    string descricao = FuncoesAdicionais.LerTextoOpcional("Descrição", caminho.Descricao);
                                    FuncoesAdicionais.ValidarDadosCaminho(latitude, longitude, avaliacao, descricao);
                                    await Caminho.AtualizarCaminho(caminhoId, latitude, longitude, estabelecimento.Id, avaliacao, descricao);
                                    Console.WriteLine("Caminho atualizado com sucesso.");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Não foi possível atualizar o caminho: {ex.Message}");
                                }
                                FuncoesAdicionais.AguardarTecla();
                                break;

                            case "5":
                                try
                                {
                                    Console.WriteLine("========= Deletar Caminho =========");
                                    int caminhoId = FuncoesAdicionais.LerId("ID do caminho que deseja deletar: ");
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
                                FuncoesAdicionais.AguardarTecla();
                                break;

                            case "6":
                                sairCaminho = true;
                                break;

                            default:
                                Console.WriteLine("Opção inválida.");
                                FuncoesAdicionais.AguardarTecla();
                                break;
                        }
                    }
                }
                break;
            case "3":
                logado = false;
                break;
            default:
                Console.WriteLine("Opção inválida.");
                FuncoesAdicionais.AguardarTecla();
                break;
        }
    }
}
