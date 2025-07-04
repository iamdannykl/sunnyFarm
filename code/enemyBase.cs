using System;
using System.Collections.Generic;
using Godot;

namespace SunnyFarm.code;

public partial class enemyBase : CharacterBody2D, Iattackble
{
    [Export] public float speed;
    [Export] public float atkValue;
    [Export] public float hp;
    [Export] public AnimationPlayer hitFlasher;
    [Export] public AnimationPlayer explodeAnim;
    [Export] public PackedScene atkLabel;
    [Export] public Timer atkGap;
    public bool canMove = true;
    private List<Label> labels = new();
    private BasicsCore _basicsCore;
    private Vector2 direction;
    private Sprite2D sprite2D;
    private bool hasDropCoin;
    public Action removeSelf;

    public float Hp
    {
        get => hp;
        set
        {
            hp = value;
            if (hp <= 0)
            {
                CollisionLayer = 0;
                removeSelf?.Invoke(); //check if it is null
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
        _basicsCore = GetTree().CurrentScene.GetNode<BasicsCore>("playObjects/CharacterBody2D");
        sprite2D = GetNode<Sprite2D>("Sprite2D");
        GD.Print("get" + _basicsCore.Name);
    }

    public void fbdMove(Node2D playerColl)
    {
        PhysicsBody2D tileColl = playerColl as PhysicsBody2D;
        if (tileColl != null && tileColl.CollisionLayer == 2)
        {
            canMove = false;
            atkGap.Start();
            GD.Print("no");
        }
    }

    public void atkPlayer()
    {
        _basicsCore.CurrentHp -= atkValue;
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
        direction = GlobalPosition.DirectionTo(_basicsCore.GlobalPosition);
        if (direction.X < 0)
            sprite2D.FlipH = false;
        else if (direction.X > 0) sprite2D.FlipH = true;
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

    public virtual void attacked(float trueDamage,baseGun gun)
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