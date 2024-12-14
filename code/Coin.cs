using Godot;

namespace SunnyFarm.code;

public partial class Coin : Area2D
{
    [Export] public float spd;
    private Vector2 direction;
    public bool canMove;

    private player playerTarget;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        playerTarget = player.Instance;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (!canMove) return;
        direction = GlobalPosition.DirectionTo(playerTarget.GlobalPosition);
        GlobalPosition += spd * direction * (float)delta;
    }

    public void flyToPlayer(Area2D player)
    {
        GD.Print($"player.CollisionLayer{player.CollisionLayer}");
        switch (player.CollisionLayer)
        {
            case 16:
                canMove = true;
                break;
            case 32:
                canMove = false;
                playerTarget.MoneyValue += 1;
                QueueFree();
                break;
        }
    }
}