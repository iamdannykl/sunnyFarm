using Godot;
using System.Collections.Generic;

public partial class enemyBase : CharacterBody2D, Iattackble
{
    [Export] public float speed;
    [Export] public float atkValue;
    [Export] public float hp;
    [Export] public AnimationPlayer hitFlasher;
    [Export] public PackedScene atkLabel;
    public bool canMove = true;
    List<Label> labels = new List<Label>();
    player player;
    Vector2 direction;
    public float Hp
    {
        get => hp;
        set
        {
            hp = value;
            if (hp <= 0)
            {
                desSelf();
            }
        }
    }

    public void desSelf()
    {
        spawner.Instance.enemies.Remove(this);
        QueueFree();
    }
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
        if (!canMove) return;
        direction = GlobalPosition.DirectionTo(player.GlobalPosition);
        Velocity = direction * speed;
        MoveAndSlide();
    }
    public void showTheAtkValue()
    {
        Label atkLabelNew = atkLabel.Instantiate() as Label;
        atkLabelNew.Text = atkValue.ToString();
        GetTree().CurrentScene.GetNode<Node2D>("playObjects").AddChild(atkLabelNew);
        atkLabelNew.GlobalPosition = GetNode<Marker2D>("atkText").GlobalPosition;
    }
    public virtual void attacked(float trueDamage)
    {
        /* throw new NotImplementedException(); */
        atkValue = trueDamage;
        showTheAtkValue();
        hitFlasher.Play("flash");
        //GD.Print(111);
    }
    public void playRst()
    {
        hitFlasher.Play("RESET");
    }
}
