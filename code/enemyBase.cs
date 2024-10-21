using Godot;
using System;

public partial class enemyBase : CharacterBody2D
{
    [Export] public float speed;
    [Export] public float atkValue;
    [Export] public float hp;
    public bool canMove = true;
    player player;
    Vector2 direction;
    public override void _Ready()
    {
        player = GetTree().CurrentScene.GetNode<player>("playObjects/CharacterBody2D");
        GD.Print("get" + player.Name);
    }
    public void fbdMove(Node2D playerColl)
    {
        PhysicsBody2D tileColl = playerColl as PhysicsBody2D;
        if (tileColl != null && tileColl.CollisionLayer == 2)
        {
            canMove = false;
            GD.Print("no");
        }
    }
    public void youCanMove(Node2D playerColl)
    {
        PhysicsBody2D tileColl = playerColl as PhysicsBody2D;
        if (tileColl != null && tileColl.CollisionLayer == 2)
        {
            canMove = true;
            GD.Print("yes");
        }
    }
    public override void _PhysicsProcess(double delta)
    {
        if (canMove)
        {
            direction = GlobalPosition.DirectionTo(player.GlobalPosition);
            Velocity = direction * speed;
            MoveAndSlide();
        }
    }
}
