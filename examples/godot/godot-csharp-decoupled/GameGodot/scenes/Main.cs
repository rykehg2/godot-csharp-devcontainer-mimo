using Godot;
using GameLogic;
using GameLogic.Services;
using GameLogic.Interfaces;

public partial class Main : Node
{
    public override void _Ready()
    {
        // Create and register all services in ServiceLocator for DI
        ServiceLocator.Register<IPlayerMovementService>(new PlayerMovementService());
        ServiceLocator.Register<IMobService>(new MobService());
        ServiceLocator.Register<IHudService>(new HudService());
    }
}