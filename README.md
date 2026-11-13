# Projeto Integrador — Plataforma de Acessibilidade Humana

## Primeira etapa (planejamento, sem implementação)

Este documento apresenta o planejamento solicitado **antes de escrever código**.

---

## 1) Arquitetura proposta

Arquitetura em camadas, com separação de responsabilidades:

- **Controllers (API)**: recebem requisições HTTP, validam entrada e delegam para Services.
- **Services (negócio)**: regras de domínio (duplicidade, cálculo de média, autorização, status de denúncia).
- **Repositories (opcional por agregados complexos)**: consultas especializadas e paginação.
- **Models (entidades)**: mapeamento do domínio + EF Core.
- **DTOs**: contratos de entrada/saída da API.
- **Data**: `DbContext`, configurações Fluent API, migrações.
- **Frontend (wwwroot)**: HTML/CSS/JS + Leaflet consumindo API via Fetch.

Padrões principais:
- Autenticação JWT (ou cookie token) para proteger endpoints privados.
- Senha com hash seguro (BCrypt/Argon2 via biblioteca consolidada).
- Versionamento de API (`/api/v1/...`).

---

## 2) Estrutura de pastas sugerida

```text
/src
  /AcessibilidadeUrbana.Api
    /Controllers
      AuthController.cs
      UsuariosController.cs
      EstabelecimentosController.cs
      AvaliacoesController.cs
      DenunciasController.cs
      RecursosController.cs
    /Services
      Interfaces/
      AuthService.cs
      UsuarioService.cs
      EstabelecimentoService.cs
      AvaliacaoService.cs
      DenunciaService.cs
    /Repositories
      Interfaces/
      EstabelecimentoRepository.cs
      AvaliacaoRepository.cs
    /Models
      Usuario.cs
      Endereco.cs
      Estabelecimento.cs
      CategoriaEstabelecimento.cs
      RecursoAcessibilidade.cs
      Avaliacao.cs
      CriterioAvaliacao.cs
      RespostaAvaliacao.cs
      Denuncia.cs
      TipoBarreira.cs
      HistoricoOcorrencia.cs
    /DTOs
      /Auth
      /Usuarios
      /Estabelecimentos
      /Avaliacoes
      /Denuncias
    /Data
      AppDbContext.cs
      /Configurations
      /Migrations
    /Security
    /Validators
    /wwwroot
      /css
      /js
      /imagens
      index.html
      mapa.html
```

---

## 3) Entidades e atributos (proposta)

### Usuario
- `Id` (Guid, PK)
- `Nome` (string, 120)
- `Email` (string, 180, único)
- `SenhaHash` (string)
- `FotoUrl` (string?, opcional)
- `CriadoEm` (DateTime UTC)
- `AtualizadoEm` (DateTime UTC)

### Endereco
- `Id` (Guid, PK)
- `Logradouro` (string, 150)
- `Numero` (string, 20)
- `Complemento` (string?, 80)
- `Bairro` (string, 80)
- `Cidade` (string, 80)
- `Estado` (string, 2)
- `Cep` (string, 9)
- `Referencia` (string?, 120)

### CategoriaEstabelecimento
- `Id` (short, PK)
- `Nome` (string, 60, único)
- `Ativa` (bool)

### Estabelecimento
- `Id` (Guid, PK)
- `Nome` (string, 140)
- `Descricao` (string?, 1000)
- `InformacoesAdicionais` (string?, 1000)
- `Latitude` (decimal(9,6))
- `Longitude` (decimal(9,6))
- `UsuarioId` (Guid, FK -> Usuario)
- `CategoriaId` (short, FK -> CategoriaEstabelecimento)
- `EnderecoId` (Guid, FK -> Endereco)
- `CriadoEm` / `AtualizadoEm` (DateTime UTC)

### RecursoAcessibilidade
- `Id` (Guid, PK)
- `EstabelecimentoId` (Guid, FK)
- `Nome` (string, 80)  
  (ex.: rampa, elevador, piso tátil, etc.)
- `Disponivel` (bool)
- `Observacao` (string?, 300)

### CriterioAvaliacao
- `Id` (short, PK)
- `Nome` (string, 80, único)
- `Descricao` (string?, 200)
- `Ativo` (bool)

### Avaliacao
- `Id` (Guid, PK)
- `UsuarioId` (Guid, FK -> Usuario)
- `EstabelecimentoId` (Guid, FK -> Estabelecimento)
- `Comentario` (string?, 1000)
- `NotaGeral` (decimal(3,2))
- `CriadoEm` / `AtualizadoEm` (DateTime UTC)

### RespostaAvaliacao
- `Id` (Guid, PK)
- `AvaliacaoId` (Guid, FK -> Avaliacao)
- `CriterioId` (short, FK -> CriterioAvaliacao)
- `Nota` (decimal(3,2), 0..5)
- `Observacao` (string?, 300)

### TipoBarreira
- `Id` (short, PK)
- `Nome` (string, 80, único)
- `Ativo` (bool)

### Denuncia
- `Id` (Guid, PK)
- `EstabelecimentoId` (Guid, FK)
- `UsuarioId` (Guid, FK)
- `TipoBarreiraId` (short, FK)
- `Descricao` (string, 1200)
- `Status` (tinyint/enum: Aberta, EmAnalise, Resolvida, Reaberta)
- `CriadoEm` / `AtualizadoEm` (DateTime UTC)

### HistoricoOcorrencia
- `Id` (Guid, PK)
- `DenunciaId` (Guid, FK -> Denuncia)
- `UsuarioId` (Guid, FK -> Usuario)
- `Acao` (string, 60)
- `Descricao` (string?, 600)
- `DataOcorrencia` (DateTime UTC)

---

## 4) Relacionamentos e cardinalidade

- Usuario **1:N** Estabelecimento
- Usuario **1:N** Avaliacao
- Usuario **1:N** Denuncia
- Usuario **1:N** HistoricoOcorrencia
- CategoriaEstabelecimento **1:N** Estabelecimento
- Endereco **1:1** Estabelecimento (para evitar repetição de campos no agregado)
- Estabelecimento **1:N** Avaliacao
- Estabelecimento **1:N** Denuncia
- Estabelecimento **1:N** RecursoAcessibilidade
- Avaliacao **1:N** RespostaAvaliacao
- CriterioAvaliacao **1:N** RespostaAvaliacao
- TipoBarreira **1:N** Denuncia
- Denuncia **1:N** HistoricoOcorrencia

Regras de integridade importantes:
- Índice único em `(UsuarioId, EstabelecimentoId)` em `Avaliacao` para impedir avaliação duplicada.
- Índice de deduplicação em `Estabelecimento` por `(Nome normalizado, Latitude, Longitude)`.
- Check constraints para latitude [-90, 90] e longitude [-180, 180].

---

## 5) Modelo relacional (resumo)

Tabelas principais:
- `usuarios`
- `enderecos`
- `categorias_estabelecimento`
- `estabelecimentos`
- `recursos_acessibilidade`
- `criterios_avaliacao`
- `avaliacoes`
- `respostas_avaliacao`
- `tipos_barreira`
- `denuncias`
- `historico_ocorrencias`

Índices recomendados:
- `usuarios(email)` unique
- `estabelecimentos(categoria_id)`
- `estabelecimentos(latitude, longitude)` (busca geográfica inicial)
- `estabelecimentos(nome)`
- `avaliacoes(estabelecimento_id)`
- `avaliacoes(usuario_id, estabelecimento_id)` unique
- `denuncias(estabelecimento_id, status)`
- `historico_ocorrencias(denuncia_id, data_ocorrencia)`

Cascata sugerida:
- `Avaliacao -> RespostaAvaliacao`: delete cascade
- `Denuncia -> HistoricoOcorrencia`: delete cascade
- `Usuario -> Avaliacao/Denuncia/Estabelecimento`: restrict (evita perda histórica)

---

## 6) Integração C# + EF Core + MySQL + JavaScript

Fluxo:
1. Frontend (JS Fetch) chama endpoints REST em ASP.NET Core.
2. Controllers recebem DTOs e validam (`DataAnnotations` + validações de negócio no Service).
3. Services executam regras (duplicidade, autorização, média de notas, atualização de denúncia).
4. EF Core persiste no MySQL via `MySql.EntityFrameworkCore` (ou provedor compatível com versão do EF).
5. API responde DTOs prontos para renderização no frontend.

Boas práticas:
- DTOs separados de entidades.
- Paginação em listagens públicas.
- `AsNoTracking()` para consultas de leitura.
- Concurrency token (`RowVersion`) em entidades críticas (ex.: Denuncia/Avaliacao).

---

## 7) Integração Leaflet + API (carga e cadastro de pontos)

Carregamento no mapa:
1. `GET /api/v1/estabelecimentos?bbox=...&categoria=...&acessibilidade=...`
2. API retorna pontos com resumo (nome, categoria, média, recursos-chave, quantidade de avaliações, lat/lng).
3. JS cria `L.marker(...)` para cada ponto e exibe popup com botão “Ver detalhes”.

Cadastro no mapa:
1. Usuário clica no mapa (`map.on('click', ...)`).
2. JS preenche campos `latitude` e `longitude` automaticamente.
3. Formulário envia `POST /api/v1/estabelecimentos`.
4. API valida coordenadas e duplicidade, salva e retorna ponto criado.
5. Frontend adiciona marcador imediatamente.

Geolocalização do navegador:
- `navigator.geolocation.getCurrentPosition(...)` para centralizar mapa e sugerir coordenadas, sempre com consentimento.

---

## 8) Possíveis problemas e melhorias

1. **Duplicidade de locais**: nome parecido + coordenada próxima.  
   Melhoria: normalização de nome + tolerância geográfica + aviso ao usuário.

2. **Avaliação inconsistente** (nota geral diferente das respostas por critério).  
   Melhoria: calcular `NotaGeral` no backend com base em `RespostaAvaliacao`.

3. **Consultas geográficas escaláveis**.  
   Melhoria: evoluir para tipo espacial MySQL (`POINT`) e índice espacial.

4. **Histórico de denúncias sem auditoria forte**.  
   Melhoria: sempre registrar transições de status em `HistoricoOcorrencia`.

5. **Segurança de autenticação**.  
   Melhoria: política de senha forte, lock temporário por tentativas, expiração de token e refresh seguro.

---

## 9) Próximos passos (após sua aprovação)

1. Criar solução ASP.NET Core e estrutura de pastas.
2. Implementar entidades + `AppDbContext` + Fluent API.
3. Criar primeira migration e validar no MySQL.
4. Implementar autenticação e endpoints iniciais.
5. Implementar mapa Leaflet consumindo endpoints de estabelecimentos.

**Aguardando sua aprovação deste planejamento para iniciar a implementação.**
