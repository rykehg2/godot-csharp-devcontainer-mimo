# Reorganização de Arquitetura: MVP de Godot C# + xUnit Unit Tests

Este documento apresenta a proposta para separar o domínio e a lógica de negócios da interface gráfica e infraestrutura do Godot, garantindo um código altamente testável, manutenível e ideal para ser expandido com auxílio de Inteligência Artificial.

---

## Avaliação da Arquitetura Proposta

A estrutura proposta faz **total sentido** e é uma excelente prática para desenvolvimento de jogos de médio/gande porte em C# com Godot. Abaixo está uma análise de trade-offs e mitigação de desafios:

### 🌟 Vantagens (Por que faz sentido?)
1. **Testabilidade Isolada:** A lógica pura em `GameLogic` (C#) pode ser testada em milissegundos com o xUnit clássico (`dotnet test`). Não é necessário inicializar o motor da Godot, criar cenas de teste, ou lidar com runner de testes específicos do Godot (que costumam ser lentos e difíceis de rodar em CI/CD).
2. **Ideal para Desenvolvimento com IA:** LLMs conseguem analisar, propor alterações, gerar novos comportamentos e criar testes unitários para classes puras em C# de forma extremamente confiável, sem o risco de corromper cenas do Godot ou gerar APIs inválidas do motor.
3. **Desacoplamento e Responsabilidade Única (SRP):** Os scripts da pasta `GameGodot` passam a agir como **Controllers/Adapters**. Eles simplesmente lêem entradas do motor, chamam a lógica do domínio, e atualizam a UI/Visual de acordo com as mudanças de estado da lógica.
4. **Portabilidade:** A lógica do jogo se torna independente da Godot. Caso decida mudar de motor ou criar uma ferramenta de linha de comando para simular partidas e balancear parâmetros de spawn, a lógica base não sofre alterações.

### ⚠️ Desafios e Como Mitigaremos
1. **Tipos Próprios do Godot (ex: `Vector2`):** `GameLogic` não deve fazer referência ao Godot. Se precisarmos manipular posições ou direções, podemos usar o `System.Numerics.Vector2` (nativo do .NET) e convertê-lo na borda (`GameGodot`).
2. **Ciclo de Vida do Motor e Timers:** O controle fino do tempo (como Timers de spawn ou de score) pode ser disparado pelo Godot, mas a **regra** do que acontece quando o timer expira e quais parâmetros são gerados deve ser computada no `GameLogic`.
3. **Tratamento de Sinais:** Usaremos eventos padrão do C# (`event Action` ou `EventHandler`) em `GameLogic` para sinalizar mudanças de estado. Os componentes do Godot escutarão estes eventos para executar animações, sons e atualizações de tela.

---

## Estrutura do Projeto Proposta

A estrutura de diretórios final ficará assim:

```text
Game/ (Workspace Root)
├── Game.sln                      ← Solução .NET atualizada apontando para os 3 projetos
├── .vscode/                      ← Configurações de tasks/launch atualizadas
│   ├── tasks.json
│   └── launch.json
│
├── GameLogic/                    ← Biblioteca de Classes C# Pura (.NET 8.0)
│   ├── GameLogic.csproj
│   ├── Models/
│   │   ├── GameState.cs          ← Estado do jogo (Score, HighScore, status)
│   │   └── PlayerStats.cs        ← Dados do player (Speed, etc.)
│   ├── Rules/
│   │   ├── ScoreRules.cs         ← Cálculo de pontuação
│   │   └── MobSpawnRules.cs      ← Determina velocidade/frequência de mobs baseada no score
│   └── Services/
│       └── GameSession.cs        ← Coordenador central do fluxo do jogo
│
├── GameGodot/                    ← Projeto Godot (.NET 8.0 + Sdk Godot)
│   ├── GameGodot.csproj          ← Referencia GameLogic
│   ├── project.godot             ← Arquivo de projeto do Godot (movido para cá)
│   ├── Main.cs                   ← Controller da Cena Principal (chama GameSession)
│   ├── Player.cs                 ← Controller do Jogador (visual, física, input)
│   ├── Mob.cs                    ← Controller do Inimigo
│   ├── Hud.cs                    ← Controller da Interface
│   ├── *.tscn                    ← Cenas Godot (main, player, mob, hud)
│   ├── art/                      ← Assets gráficos/áudio
│   └── fonts/                    ← Fontes de texto
│
└── GameLogic.Tests/              ← Projeto de Testes xUnit (.NET 8.0)
    ├── GameLogic.Tests.csproj    ← Referencia GameLogic e pacotes xUnit
    ├── GameSessionTests.cs       ← Testa fluxo da partida (início, fim, score)
    └── MobSpawnRulesTests.cs     ← Testa regras de dificuldade progressiva
```

---

## Proposta de Implementação Detalhada

### 1. Criando e Configurando os Projetos (.csproj)

#### `GameLogic/GameLogic.csproj`
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>
</Project>
```

#### `GameGodot/GameGodot.csproj` (atualização e renomeação do atual `Game.csproj`)
```xml
<Project Sdk="Godot.NET.Sdk/4.6.2">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <TargetFramework Condition=" '$(GodotTargetPlatform)' == 'android' ">net9.0</TargetFramework>
    <EnableDynamicLoading>true</EnableDynamicLoading>
    <Nullable>enable</Nullable>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include="..\GameLogic\GameLogic.csproj" />
  </ItemGroup>
</Project>
```

#### `GameLogic.Tests/GameLogic.Tests.csproj`
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <IsPackable>false</IsPackable>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.8.0" />
    <PackageReference Include="xunit" Version="2.5.3" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.5.3" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include="..\GameLogic\GameLogic.csproj" />
  </ItemGroup>
</Project>
```

---

## Proposta de Código da Lógica de Negócios (`GameLogic`)

Vamos refatorar o núcleo do jogo para que as regras fiquem na biblioteca puramente C#.

### `Models`
* **`GameState.cs`**: Representa o estado atual do jogo.
```csharp
namespace GameLogic.Models;

public enum GameStatus
{
    Ready,
    Running,
    GameOver
}

public class GameState
{
    public int Score { get; set; }
    public GameStatus Status { get; set; } = GameStatus.Ready;
}
```

### `Rules`
* **`MobSpawnRules.cs`**: Define a velocidade de movimentação do Mob de acordo com o Score. Isso possibilita que a dificuldade aumente gradualmente, e é muito fácil de testar!
```csharp
namespace GameLogic.Rules;

public static class MobSpawnRules
{
    // Aumenta a velocidade mínima e máxima do mob de acordo com o score
    public static (float MinSpeed, float MaxSpeed) GetMobSpeedRange(int score)
    {
        float baseMin = 150.0f;
        float baseMax = 250.0f;
        
        // Ex: Aumenta a velocidade em 5% por ponto marcado
        float speedMultiplier = 1.0f + (score * 0.05f);
        
        return (baseMin * speedMultiplier, baseMax * speedMultiplier);
    }
}
```

### `Services`
* **`GameSession.cs`**: Controla o fluxo da sessão de jogo e expõe eventos.
```csharp
using GameLogic.Models;

namespace GameLogic.Services;

public class GameSession
{
    private readonly GameState _state = new();
    
    public event Action<int>? OnScoreChanged;
    public event Action<GameStatus>? OnStatusChanged;

    public int CurrentScore => _state.Score;
    public GameStatus CurrentStatus => _state.Status;

    public void StartNewGame()
    {
        _state.Score = 0;
        _state.Status = GameStatus.Running;
        OnScoreChanged?.Invoke(_state.Score);
        OnStatusChanged?.Invoke(_state.Status);
    }

    public void IncrementScore()
    {
        if (_state.Status != GameStatus.Running) return;
        
        _state.Score++;
        OnScoreChanged?.Invoke(_state.Score);
    }

    public void GameOver()
    {
        _state.Status = GameStatus.GameOver;
        OnStatusChanged?.Invoke(_state.Status);
    }
}
```

---

## Como Ficarão os Componentes Godot (`GameGodot`)

Os nós da cena se ligarão à `GameSession` do `GameLogic`.

* **`Main.cs`**:
  ```csharp
  using Godot;
  using GameLogic.Services;
  using GameLogic.Rules;
  using GameLogic.Models;

  public partial class Main : Node
  {
      [Export]
      public PackedScene MobScene { get; set; }

      private GameSession _gameSession;

      public override void _Ready()
      {
          _gameSession = new GameSession();
          
          // Vincula eventos da lógica pura às atualizações da interface do Godot
          _gameSession.OnScoreChanged += (score) => GetNode<Hud>("Hud").UpdateScore(score);
          _gameSession.OnStatusChanged += OnGameStatusChanged;

          GetNode<Hud>("Hud").StartGame += NewGame;
      }

      public void NewGame()
      {
          _gameSession.StartNewGame();
      }

      private void OnGameStatusChanged(GameStatus status)
      {
          if (status == GameStatus.Running)
          {
              var player = GetNode<Player>("Player");
              var startPosition = GetNode<Marker2D>("StartPosition");
              player.Start(startPosition.Position);

              GetNode<Timer>("StartTimer").Start();
              GetNode<AudioStreamPlayer>("Music").Play();
              GetTree().CallGroup("mobs", Node.MethodName.QueueFree);
              GetNode<Hud>("Hud").ShowMessage("Get Ready!");
          }
          else if (status == GameStatus.GameOver)
          {
              GetNode<Timer>("MobTimer").Stop();
              GetNode<Timer>("ScoreTimer").Stop();
              GetNode<Hud>("Hud").ShowGameOver();
              GetNode<AudioStreamPlayer>("Music").Stop();
              GetNode<AudioStreamPlayer>("DeathSound").Play();
          }
      }

      private void OnScoreTimerTimeout()
      {
          _gameSession.IncrementScore();
      }

      private void OnStartTimerTimeout()
      {
          GetNode<Timer>("MobTimer").Start();
          GetNode<Timer>("ScoreTimer").Start();
      }

      private void OnMobTimerTimeout()
      {
          Mob mob = MobScene.Instantiate<Mob>();
          var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
          mobSpawnLocation.ProgressRatio = GD.Randf();

          float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;
          mob.Position = mobSpawnLocation.Position;

          direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
          mob.Rotation = direction;

          // Obtém o range de velocidade dinamicamente da regra de negócio pura em GameLogic
          var (minSpeed, maxSpeed) = MobSpawnRules.GetMobSpeedRange(_gameSession.CurrentScore);
          var speed = GD.RandRange(minSpeed, maxSpeed);
          
          var velocity = new Vector2((float)speed, 0);
          mob.LinearVelocity = velocity.Rotated(direction);

          AddChild(mob);
      }
      
      // Quando o player colide com o mob, o sinal Hit do Player chama esta função
      public void OnPlayerHit()
      {
          _gameSession.GameOver();
      }
  }
  ```

---

## Plano de Verificação

### Testes Automatizados (xUnit)
Os testes unitários validarão a integridade das regras e o comportamento da sessão:
```bash
dotnet test
```

* **`GameLogic.Tests/GameSessionTests.cs`**:
  - Verifica se `StartNewGame` zera os pontos e define status como `Running`.
  - Verifica se `IncrementScore` aumenta a pontuação apenas se o jogo estiver rodando.
  - Verifica se os eventos `OnScoreChanged` e `OnStatusChanged` disparam corretamente.
* **`GameLogic.Tests/MobSpawnRulesTests.cs`**:
  - Garante que a velocidade mínima e máxima do mob aumenta progressivamente conforme a pontuação sobe.

### Verificação Manual
1. Abrir a cena no Godot Editor a partir da nova pasta `GameGodot/`.
2. Rodar o jogo e verificar o fluxo básico (começar, desviar dos monstros, o score subir, game over, sons).
3. Testar a depuração usando a configuração atualizada no VS Code direcionando para a pasta `GameGodot`.
