using Godot;
using System.Collections.Generic;

public partial class enemyBase : CharacterBody2D, Iattackble
{
    [Export] public float speed;
    [Export] public float atkValue;
    [Export] public float hp;
    [Export] public AnimationPlayer hitFlasher;
    [Export] public AnimationPlayer explodeAnim;
    [Export] public PackedScene atkLabel;
    public bool canMove = true;
    List<Label> labels = new List<Label>();
    player player;
    Vector2 direction;
    Sprite2D sprite2D;
    bool hasDropCoin;
    public float Hp
    {
        get => hp;
        set
        {
            hp = value;
            if (hp <= 0)
            {
                crtCoin();
                explodeAnim.Play("explode");
            }
        }
    }
    public void desSelf()
    {
        spawner.Instance.enemies.Remove(this);
        QueueFree();
    }

    public void crtCoin()
    {
        if (hasDropCoin) return;
        hasDropCoin = true;
        Area2D coinNew = MatchIt.Instance.coin.Instantiate() as Area2D;
        if (coinNew != null)
        {
            coinNew.GlobalPosition = GlobalPosition;
            //spawner.Instance.AddChild(coinNew);
            spawner.Instance.CallDeferred("AddChild", coinNew, true);
        }
    }
    public override void _Ready()
    {
        player = GetTree().CurrentScene.GetNode<player>("playObjects/CharacterBody2D");
        sprite2D = GetNode<Sprite2D>("Sprite2D");
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
        if (direction.X < 0)
        {
            sprite2D.FlipH = false;
        }
        else if (direction.X > 0)
        {
            sprite2D.FlipH = true;
        }
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
