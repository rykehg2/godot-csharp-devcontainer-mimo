using Godot;
using GdUnit4;
using GameLogic.Interfaces;
using GameLogic.Services;
using GameLogic;

namespace GameGodot.Tests;

[TestSuite]
public partial class GodotIntegrationTest
{
    [TestCase]
    public void Should_Register_And_Resolve_Services_In_Godot_Runtime()
    {
        // Setup: Register service implementation in ServiceLocator
        var movementService = new PlayerMovementService();
        ServiceLocator.Register<IPlayerMovementService>(movementService);

        // Assert: Service is retrievable in Godot environment
        var resolved = ServiceLocator.Get<IPlayerMovementService>();
        Assertions.AssertThat(resolved).IsNotNull();
        
        ServiceLocator.Clear();
    }

    [RequireGodotRuntime]
    [TestCase]
    public void Should_Instantiate_Main_Scene_And_Nodes()
    {
        // Assert: Main scene resource exists and can be loaded
        var mainScene = ResourceLoader.Load<PackedScene>("res://scenes/main.tscn");
        Assertions.AssertThat(mainScene).IsNotNull();

        var mainInstance = mainScene.Instantiate();
        Assertions.AssertThat(mainInstance).IsNotNull();
        
        mainInstance.QueueFree();
    }
}
