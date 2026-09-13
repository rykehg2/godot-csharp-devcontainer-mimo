using Godot;
using GameLogic;
using GameLogic.Interfaces;
using GameLogic.Models;
using GameLogic.Services;

public partial class GameState : Node
{
    [Export]
    public PackedScene MobScene { get; set; } = null!;

    private GameSession _gameSession = null!;
    private Player _player = null!;
    private Hud _hud = null!;
    private IMobService? _mobService = null;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _gameSession = new GameSession();
        _player = GetNode<Player>("Player");
        _hud = GetNode<Hud>("Hud");

        // Vincula eventos da lógica pura às atualizações da interface do Godot
        _gameSession.OnScoreChanged += (score) => _hud.UpdateScore(score);
        _gameSession.OnStatusChanged += OnGameStatusChanged;

        _hud.StartGame += NewGame;
    }

    public void NewGame()
    {
        _gameSession.StartNewGame();
    }

    public void GameOver()
    {
        _gameSession.GameOver();
    }

    private void OnGameStatusChanged(GameStatus status)
    {
        if (status == GameStatus.Running)
        {
            var startPosition = GetNode<Marker2D>("StartPosition");
            _player.Start(startPosition.Position);

            GetNode<Godot.Timer>("StartTimer").Start();
            GetNode<AudioStreamPlayer2D>("Music").Play();
            GetTree().CallGroup("mobs", Node.MethodName.QueueFree);
            _hud.ShowMessage("Get Ready!");
        }
        else if (status == GameStatus.GameOver)
        {
            GetNode<Godot.Timer>("MobTimer").Stop();
            GetNode<Godot.Timer>("ScoreTimer").Stop();

            _hud.ShowGameOver();

            GetNode<AudioStreamPlayer2D>("Music").Stop();
            GetNode<AudioStreamPlayer2D>("DeathSound").Play();
        }
    }

    // We also specified this function name in PascalCase in the editor's connection window.
    private void OnScoreTimerTimeout()
    {
        _gameSession.IncrementScore();
    }

    // We also specified this function name in PascalCase in the editor's connection window.
    private void OnStartTimerTimeout()
    {
        GetNode<Godot.Timer>("MobTimer").Start();
        GetNode<Godot.Timer>("ScoreTimer").Start();
    }

    /// <summary>
    /// Lazily resolves the mob service from the ServiceLocator.
    /// Called on first use rather than in _Ready(), because Main._Ready()
    /// (which registers services) runs AFTER child nodes' _Ready().
    /// </summary>
    private IMobService ResolveMobService()
    {
        _mobService ??= ServiceLocator.Get<IMobService>();
        return _mobService;
    }

    // We also specified this function name in PascalCase in the editor's connection window.
    private void OnMobTimerTimeout()
    {
        // Create a new instance of the Mob scene.
        Mob mob = MobScene.Instantiate<Mob>();

        // Choose a random location on Path2D.
        var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
        mobSpawnLocation.ProgressRatio = GD.Randf();

        // Set the mob's position to a random location.
        mob.Position = mobSpawnLocation.Position;

        // Add some randomness to the direction.
        float angleOffset = (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);

        // Delegate spawn logic to MobService (pure logic, testable without Godot)
        var svc = ResolveMobService();
        var spawnData = svc.CreateSpawnData(
            mobSpawnLocation.Rotation,
            _gameSession.CurrentScore,
            angleOffset,
            new System.Random()
        );

        mob.Rotation = spawnData.Rotation;
        mob.LinearVelocity = new Vector2(
            spawnData.LinearVelocity.X,
            spawnData.LinearVelocity.Y
        );

        // Spawn the mob by adding it to the GameState scene.
        AddChild(mob);
    }
}