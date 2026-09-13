using Godot;
using GameLogic;
using GameLogic.Interfaces;

public partial class Player : Area2D
{
	[Signal]
	public delegate void HitEventHandler();

	[Export]
    public int Speed { get; set; } = 400; // How fast the player will move (pixels/sec).

    public Vector2 ScreenSize; // Size of the game window.

	private IPlayerMovementService? _movementService = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		Hide(); // player will be hidden when the game starts
	}

	/// <summary>
	/// Lazily resolves the movement service from the ServiceLocator.
	/// Called on first use rather than in _Ready(), because Main._Ready()
	/// (which registers services) runs AFTER child nodes' _Ready().
	/// </summary>
	private IPlayerMovementService ResolveMovementService()
	{
		_movementService ??= ServiceLocator.Get<IPlayerMovementService>();
		return _movementService;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var direction = Vector2.Zero;

		if (Input.IsActionPressed("move_right"))
		{
			direction.X += 1;
		}

		if (Input.IsActionPressed("move_left"))
		{
			direction.X -= 1;
		}

		if (Input.IsActionPressed("move_down"))
		{
			direction.Y += 1;
		}

		if (Input.IsActionPressed("move_up"))
		{
			direction.Y -= 1;
		}

		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		// Delegate movement math to pure-logic service
		var svc = ResolveMovementService();
		var snVelocity = svc.CalculateVelocity(ToSystemNumerics(direction), Speed);
		var anim = svc.GetAnimationState(snVelocity);
		var velocity = ToGodotVector2(snVelocity);

		if (velocity.LengthSquared() > 0)
		{
			animatedSprite2D.Play();
		}
		else
		{
			animatedSprite2D.Stop();
		}

		Position += velocity * (float)delta;
		Position = ToGodotVector2(svc.ClampPosition(ToSystemNumerics(Position), ToSystemNumerics(ScreenSize)));

		switch (anim)
		{
			case PlayerAnimation.Walk:
				animatedSprite2D.Animation = "walk";
				animatedSprite2D.FlipV = false;
				animatedSprite2D.FlipH = svc.GetFlipH(snVelocity);
				break;
			case PlayerAnimation.Up:
				animatedSprite2D.Animation = "up";
				animatedSprite2D.FlipV = svc.GetFlipV(snVelocity);
				break;
		}
	}

	public void Start(Vector2 position)
	{
		Position = position;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}

	// We also specified this function name in PascalCase in the editor's connection window.
	private void OnBodyEntered(Node2D body)
	{
		Hide(); // Player disappears after being hit.
		EmitSignal(SignalName.Hit);
		// Must be deferred as we can't change physics properties on a physics callback.
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
	}

	// Conversion helpers between Godot.Vector2 and System.Numerics.Vector2
	private static System.Numerics.Vector2 ToSystemNumerics(Vector2 v)
		=> new System.Numerics.Vector2(v.X, v.Y);

	private static Vector2 ToGodotVector2(System.Numerics.Vector2 v)
		=> new Vector2(v.X, v.Y);
}
