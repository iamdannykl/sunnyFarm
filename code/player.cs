using System;
using System.Collections.Generic;
using Godot;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SunnyFarm.code;

public partial class player : CharacterBody2D
{
    public static player Instance;

    [ExportCategory("valueDatas")] [Export]
    public float hp;

    [Export] public float atk;
    [Export] public float def;
    [Export] public float speed;
    [Export] public float range;
    [Export] public float spAtk;
    [Export] public float luckyValue;
    [Export] public float drain;
    [Export] public float regeneration;
    [Export] public float criticalRate;
    public List<float> ValuesList = new();

    [ExportCategory("normalValues")] [Export]
    public float Speed;

    [Export] public CollisionShape2D collisionShape2D;
    public Label moneyShow;
    public const float JumpVelocity = -400.0f;
    private AnimationPlayer animationPlayer;
    private bool isRt;
    private Vector2 direction;
    public int moneyValue;
    public float atkBuff;
    private float pickRadius;

    public ObservableDictionary<valueDataEnum, float> values;
    public List<Equip> playerWeapons = new();
    [Export] private mainProperty zhuShuXing;

    public int MoneyValue
    {
        get => moneyValue;
        set
        {
            moneyShow.Text = $"Money:{value}";
            moneyValue = value;
        }
    }

    private void AddWeaponsToPlayer()
    {
        foreach (var variable in GetNode<Node2D>("markers").GetChildren())
            if (variable.GetChildCount() > 0)
                playerWeapons.Add(variable.GetChild<Equip>(0));
    }

    public override void _Ready()
    {
        Instance = this;
        pickRadius = (float)collisionShape2D.Shape.Get("radius");
        ValuesList.Add(hp);
        ValuesList.Add(atk);
        ValuesList.Add(def);
        ValuesList.Add(speed);
        ValuesList.Add(range);
        ValuesList.Add(spAtk);
        ValuesList.Add(luckyValue);
        ValuesList.Add(drain);
        ValuesList.Add(regeneration);
        ValuesList.Add(criticalRate);
        values = new ObservableDictionary<valueDataEnum, float>();
        AddWeaponsToPlayer();
        // 订阅事件
        values.OnItemAdded += (key, value, isFst)
            =>
        {
            GD.Print($"Key '{key}' added with value {value}");
            if (!isFst)
                zhuShuXing.SetItemText((int)key, $"{key}: {value}");
            else
                zhuShuXing.AddItem($"{key}: {value}");
            //zhuShuXing.AddItem($"{key}: {value}");
        };
        var i = 0;
        foreach (valueDataEnum value in Enum.GetValues(typeof(valueDataEnum))) values.Add(value, ValuesList[i++], true);
        collisionShape2D.Shape.Set("radius", pickRadius + values.GetValueOrDefault(valueDataEnum.range, 0));

        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        moneyShow = GetTree().CurrentScene.GetNode<Label>("CanvasLayer/showMoney");
    }

    public override void _PhysicsProcess(double delta)
    {
        direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        if (direction != Vector2.Zero)
        {
            if (direction.X < 0)
            {
                animationPlayer.Play("lftWalk");
                isRt = false;
            }
            else if (direction.X > 0)
            {
                animationPlayer.Play("rtWalk");
                isRt = true;
            }
            else
            {
                if (isRt)
                    animationPlayer.Play("rtWalk");
                else
                    animationPlayer.Play("lftWalk");
            }

            Velocity = Speed * direction;
        }
        else
        {
            if (isRt)
                animationPlayer.Play("rtIdle");
            else
                animationPlayer.Play("lftIdle");
            Velocity = Vector2.Zero;
        }

        MoveAndSlide();
    }

    /*public static readonly List<Equip> WeaponPool = new()
    {
        new Equip("Basic Sword", new List<string> { "Melee" }, Rarity.Common),
        new Equip("stone", new List<string> { "Melee" }, Rarity.Common),
        new Equip("littleGun", new List<string> { "Gun", "Magic" }, Rarity.Common),
        new Equip("Magic Wand", new List<string> { "Magic" }, Rarity.Rare),
        new Equip("Epic Bow", new List<string> { "Ranged" }, Rarity.Epic),
        new Equip("Mythic Gun", new List<string> { "Gun" }, Rarity.Mythic),
        new Equip("Legendary Staff", new List<string> { "Magic", "Ranged" }, Rarity.Legendary)
    };*/
}