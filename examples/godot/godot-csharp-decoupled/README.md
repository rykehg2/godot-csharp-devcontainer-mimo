# Dodge the Creeps! (Godot 4.6.2 C#) — Arquitetura Desacoplada

Este é um projeto de exemplo para Godot Engine v4.6.2 utilizando C# (.NET). O jogo é uma implementação do clássico tutorial "Your First 2D Game" (Dodge the Creeps) da documentação oficial da Godot.

O projeto foi reestruturado para seguir uma **arquitetura multi-projeto com separação de responsabilidades**: a lógica pura de domínio foi extraída para uma biblioteca C# independente (`GameLogic`), permitindo testes unitários rápidos com **xUnit** sem dependência do motor Godot. Os scripts Godot (`GameGodot`) atuam como Controladores/Adapters, consumindo essa lógica via interfaces e injeção de dependência.

---

## 🤖 LLM Quick Context (Contexto para LLMs)

*Esta seção fornece um resumo rápido estruturado para que outros modelos de linguagem (LLMs) entendam o projeto instantaneamente.*

### Visão Geral do Jogo
- **Objetivo:** O jogador deve desviar dos monstros (mobs) que surgem nas bordas da tela e se movem em direções aleatórias. O score aumenta a cada segundo de sobrevivência.
- **Resolução da Tela:** 480x720 (modo retrato), com stretch mode configurado como `canvas_items`.

### Por que esta arquitetura?

A arquitetura original do tutorial Godot coloca toda a lógica (movimentação, spawn, score, UI) diretamente nos scripts C# acoplados ao motor. Isso torna impossível testar a lógica de negócio sem iniciar o Godot.

A reestruturação separa:
1. **GameLogic** — Biblioteca C# pura, sem nenhuma dependência do Godot. Toda a lógica de negócio (cálculos, regras, estado) vive aqui e pode ser testada com `dotnet test` em milissegundos.
2. **GameGodot** — Scripts que dependem do Godot (Input, AnimatedSprite2D, AudioStreamPlayer, etc.) usam interfaces definidas no GameLogic, permitindo desacoplamento e troca de implementações.
3. **GameLogic.Tests** — Testes unitários xUnit que validam a lógica pura sem iniciar o motor.

### Estrutura Multi-Projeto (.NET 8.0)

```text
Game/ (Workspace Root)
├── Game.sln                          ← Solução .NET com 3 projetos
│
├── GameLogic/                        ← Biblioteca C# Pura (sem Godot)
│   ├── Interfaces/
│   │   ├── IPlayerMovementService.cs ← Contrato de movimentação do jogador
│   │   ├── IHudService.cs            ← Contrato de serviços de HUD
│   │   └── IMobService.cs            ← Contrato de geração de mobs
│   ├── Models/
│   │   ├── GameState.cs              ← Enum GameStatus + classe GameState
│   │   └── HudState.cs              ← Enum HudMessage (estado removido por código morto)
│   ├── Rules/
│   │   └── MobSpawnRules.cs          ← Regras de dificuldade progressiva
│   ├── Services/
│   │   ├── GameSession.cs            ← Coordenador central (eventos C# para UI)
│   │   ├── PlayerMovementService.cs  ← Cálculo de movimento puro
│   │   ├── MobService.cs             ← Lógica de spawn de mobs
│   │   └── HudService.cs            ← Gerenciamento de estado da UI
│   └── ServiceLocator.cs            ← Container DI simples
│
├── GameGodot/                        ← Projeto Godot (.NET 8.0 + Godot.NET.Sdk)
│   ├── GameGodot.csproj             ← Referencia GameLogic
│   ├── project.godot
│   ├── scenes/
│   │   ├── Main.cs                   ← Bootstrap: registra serviços no ServiceLocator
│   │   ├── main.tscn                 ← Cena raiz
│   │   ├── game_state/
│   │   │   ├── GameState.cs          ← Lógica do jogo (timers, mobs, score)
│   │   │   └── game_state.tscn       ← Cena com nós do jogo
│   │   ├── objects/
│   │   │   ├── Player.cs             ← usa IPlayerMovementService (DI lazy)
│   │   │   ├── player.tscn
│   │   │   ├── Mob.cs                ← apenas animação e auto-destruição
│   │   │   └── mob.tscn
│   │   └── menu_hud/
│   │       ├── Hud.cs               ← usa IHudService (DI lazy)
│   │       └── hud.tscn
│   ├── configuration/                ← ServiceLocator
│   ├── art/ / fonts/
│
└── GameLogic.Tests/                  ← Testes Unitários xUnit (.NET 8.0)
    ├── GameSessionTests.cs           ← 5 testes (fluxo do jogo)
    ├── MobSpawnRulesTests.cs         ← 3 testes (dificuldade progressiva)
    ├── PlayerMovementServiceTests.cs ← 13 testes (movimentação)
    ├── MobServiceTests.cs            ← 6 testes (spawn de mobs)
    └── HudServiceTests.cs            ← 3 testes (estado da UI)
```

### Mapeamento de Cenas e Scripts C#

1. **Bootstrap (`GameGodot/scenes/main.tscn` → `GameGodot/scenes/Main.cs`):**
   - Cria e registra todos os serviços no `ServiceLocator`
   - Contém apenas o nó `GameState` como filho

2. **Estado do Jogo (`GameGodot/scenes/game_state/game_state.tscn` → `GameGodot/scenes/game_state/GameState.cs`):**
   - **Nó Raiz:** `GameState` (tipo `Node`)
   - Gerencia o ciclo de vida do jogo: timers, spawn de mobs, score, sons
   - Nós filhos: `Player`, `Hud`, `MobPath/MobSpawnLocation`, Timers, AudioStreamPlayers
   - Usa `GameSession` (de `GameLogic`) para coordenar estado
   - Usa `IMobService` via resolução lazy do ServiceLocator para calcular dados de spawn
   - **Importante:** O campo `Mob Scene` (Export) deve ter `mob.tscn` atribuído no Inspector

3. **Jogador (`GameGodot/scenes/objects/player.tscn` → `GameGodot/scenes/objects/Player.cs`):**
   - Usa `IPlayerMovementService` via resolução lazy do ServiceLocator
   - Delega cálculos de movimentação, clamping e animação para o serviço
   - Mantém no Godot: Input handling, AnimatedSprite2D, sinais

4. **Inimigo (`GameGodot/scenes/objects/mob.tscn` → `GameGodot/scenes/objects/Mob.cs`):**
   - RigidBody2D com gravity scale = 0
   - Escolhe animação aleatória e se destrói ao sair da tela
   - A velocidade é calculada pelo `IMobService` (chamado via lazy resolution em `GameState.OnMobTimerTimeout()`)

5. **Interface (`GameGodot/scenes/menu_hud/hud.tscn` → `GameGodot/scenes/menu_hud/Hud.cs`):**
   - Opera diretamente nos nós do Godot (Label, Button, Timer)
   - Usa `IHudService` via resolução lazy do ServiceLocator para rastrear estado
   - Gerencia mensagens, score e botão de iniciar

### Lógica Pura em `GameLogic`

**Interfaces:**
- `IPlayerMovementService` — CalculateVelocity, ClampPosition, GetAnimationState, GetFlipH, GetFlipV
- `IHudService` — ShowMessage, ShowGameOver, UpdateScore, ResetForNewGame
- `IMobService` — CreateSpawnData (com ângulo aleatório e Random)

**Services:**
- `PlayerMovementService` — Normaliza direção, calcula velocidade, limita posição aos limites da tela, determina estado de animação
- `MobService` — Calcula dados de spawn usando `MobSpawnRules`, rotação de vetores
- `HudService` — Gerencia estado da UI com transições de mensagens
- `GameSession` — Coordena ciclo de vida: StartNewGame, IncrementScore, GameOver. Expõe eventos C# (`OnScoreChanged`, `OnStatusChanged`)

**Models:**
- `GameState` — Enum `GameStatus` (Ready, Running, GameOver) + classe `GameState` (Score, Status)
- `HudState` — Enum `HudMessage` (None, GetReady, GameOver, Title)

**Rules:**
- `MobSpawnRules` — Velocidade base: min 150, max 250. Escala +5% por ponto de score

**ServiceLocator:**
- Container DI simples com `Register<T>()`, `Get<T>()` e `Clear()`

### Desafios de Timing do Godot

O Godot executa `_Ready()` de baixo para cima (filhos → pai), então `Main._Ready()` que registra os serviços roda por último. Para resolver isso, todos os scripts Godot que consomem serviços usam **resolução lazy** — o serviço é resolvido na primeira chamada, não no `_Ready()`:
- `Player.cs` — `ResolveMovementService()`
- `GameState.cs` — `ResolveMobService()`
- `Hud.cs` — `ResolveHudService()`

### Testes Unitários (xUnit)

**38 testes, todos aprovados** (`dotnet test`):

- **`GameSessionTests.cs`** (5 testes): Fluxo do jogo (início, score, game over, eventos)
- **`MobSpawnRulesTests.cs`** (3 testes): Velocidade, escalonamento
- **`PlayerMovementServiceTests.cs`** (13 testes): Velocidade, clamping, animação, flip
- **`MobServiceTests.cs`** (6 testes): Rotação, spawn data, escala com score, angle offset, seeds diferentes
- **`HudServiceTests.cs`** (5 testes): Mensagens, game over, score, reset, display text

### Ações de Entrada (Input Map em `project.godot`)
- `move_right`: Tecla Seta Direita / D
- `move_left`: Tecla Seta Esquerda / A
- `move_up`: Tecla Seta Acima / W
- `move_down`: Tecla Seta Abaixo / S
- `start_game`: Tecla Enter (atalho para iniciar o jogo)

---

## 🚀 Como Iniciar uma Sessão Rápida

### Requisitos
- **Godot Engine v4.6.2 (versão .NET / Mono)**
- **SDK do .NET 8.0 ou posterior**

### Passo a Passo

1. **Restaurar Dependências e Compilar C#:**
   ```bash
   dotnet build
   ```

2. **Executar os Testes Unitários:**
   ```bash
   dotnet test
   ```
   Resultado esperado: **38/38 testes aprovados**.

3. **Executar o Jogo via Godot CLI:**
   ```bash
   godot --path GameGodot
   ```

4. **Executar via VS Code / IDE:**
   Veja a seção de configuração de IDE abaixo.

---

## ⚙️ Configuração de IDE e Debug (VS Code / Rider)

### 1. VS Code (Visual Studio Code)

#### Extensões Recomendadas:
- **C#** (por *Microsoft*): Inteligência de código C# e suporte a depuração `.NET Core`.
- **C# Tools for Godot** (por *Godot Engine*): Utilitários de depuração C# para Godot.

> [!IMPORTANT]
> **Compatibilidade com IDEs Open-Source (Antigravity, VSCodium, etc.):**
> A extensão oficial de C# da Microsoft possui uma licença proprietária. Para ambientes open-source, utilize:
> - **C#** (por *muhammad-sammy*) com depurador open-source `netcoredbg`
> - **C# Tools for Godot** (por *neikeq*)

#### Configuração:
1. Abra `.vscode/tasks.json` e certifique-se de que `"command"` aponta para o executável do Godot.
2. Abra `.vscode/launch.json` e certifique-se de que `"program"` aponta para o mesmo executável.
3. Verifique que `"cwd"` e `"args"` apontam para `${workspaceRoot}/GameGodot`.

#### Como Depurar:
1. Vá até **Run & Debug** (`Ctrl+Shift+D`).
2. Selecione um perfil CoreCLR (Launch, Launch Scene ou Attach).
3. Pressione `F5`.

### 2. JetBrains Rider
1. Abra `Game.sln` no Rider.
2. Ative o plugin **Godot Support**.
3. Configure o caminho do executável do Godot.
4. Clique em Debug ou `Shift+F9`.

---

## 🛠️ Detalhes de Desenvolvimento

- **Configurações Físicas:** Motor `Jolt Physics`.
- **Renderização:** Driver `gl_compatibility` (OpenGL 3.3).
- **Arquitetura:** Separação em 3 projetos .NET:
  - `GameLogic` — Lógica de domínio pura (testável sem Godot). Interfaces, Services, Models, Rules.
  - `GameGodot` — Interface com o motor Godot (referencia GameLogic). Usa DI via ServiceLocator com resolução lazy no Player.
  - `GameLogic.Tests` — Testes unitários xUnit (38 testes, 38 aprovados).
