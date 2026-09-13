using Godot;
using GameLogic;
using GameLogic.Interfaces;
using GameLogic.Models;

public partial class Hud : CanvasLayer
{
    // Don't forget to rebuild the project so the editor knows about the new signal.

    [Signal]
    public delegate void StartGameEventHandler();

    private IHudService? _hudService = null;

    /// <summary>
    /// Lazily resolves the HUD service from the ServiceLocator.
    /// Called on first use rather than in _Ready(), because Main._Ready()
    /// (which registers services) runs AFTER child nodes' _Ready().
    /// </summary>
    private IHudService ResolveHudService()
    {
        _hudService ??= ServiceLocator.Get<IHudService>();
        return _hudService;
    }

    public override void _Ready()
    {
        GetNode<Button>("StartButton").Pressed += OnStartButtonPressed;
        GetNode<Godot.Timer>("MessageTimer").Timeout += OnMessageTimerTimeout;
    }

    public void ShowMessage(string text)
    {
        // Delegate state tracking to the service
        var svc = ResolveHudService();
        if (text == "Get Ready!")
            svc.ShowMessage(HudMessage.GetReady);
        else if (text == "Game Over")
            svc.ShowMessage(HudMessage.GameOver);
        else if (text == "Dodge the Creeps!")
            svc.ShowMessage(HudMessage.Title);

        // Update Godot UI
        var message = GetNode<Label>("Message");
        message.Text = text;
        message.Show();

        GetNode<Godot.Timer>("MessageTimer").Start();
    }

    async public void ShowGameOver()
    {
        var svc = ResolveHudService();
        svc.ShowGameOver();

        ShowMessage("Game Over");

        var messageTimer = GetNode<Godot.Timer>("MessageTimer");
        await ToSignal(messageTimer, Godot.Timer.SignalName.Timeout);

        var message = GetNode<Label>("Message");
        message.Text = "Dodge the Creeps!";
        message.Show();

        await ToSignal(GetTree().CreateTimer(1.0), SceneTreeTimer.SignalName.Timeout);
        GetNode<Button>("StartButton").Show();
    }

    public void UpdateScore(int score)
    {
        var svc = ResolveHudService();
        svc.UpdateScore(score);

        GetNode<Label>("ScoreLabel").Text = score.ToString();
    }

    // We also specified this function name in PascalCase in the editor's connection window.
    private void OnStartButtonPressed()
    {
        var svc = ResolveHudService();
        svc.ResetForNewGame();

        GetNode<Button>("StartButton").Hide();
        EmitSignal(SignalName.StartGame);
        GetNode<Godot.Timer>("MessageTimer").Start();
    }

    // We also specified this function name in PascalCase in the editor's connection window.
    private void OnMessageTimerTimeout()
    {
        GetNode<Label>("Message").Hide();
    }
}