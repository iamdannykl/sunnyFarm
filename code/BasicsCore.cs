using System;
using System.Collections.Generic;
using System.Globalization;
using Godot;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SunnyFarm.code;

public partial class BasicsCore : CharacterBody2D
{
    public static BasicsCore Instance;
    private float currentHp;

    public float CurrentHp
    {
        get => currentHp;
        set
        {
            var realValue = value;
            if (realValue < 0) realValue = 0;
            hpBar.Value = realValue;
            hpBar.GetNode<Label>("Label").Text = $"Hp:{realValue.ToString(CultureInfo.InvariantCulture)}";
            currentHp = value;
        }
    }

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
    [Export] public float crtHp;
    public List<float> ValuesList = new();

    [ExportCategory("normalValues")] [Export]
    public float Speed;

    [Export] public CollisionShape2D collisionShape2D;
    [Export] public Label moneyShow;
    private AnimationPlayer animationPlayer;
    private bool isRt;
    private Vector2 direction;
    public int moneyValue;
    public float atkBuff;
    private float pickRadius;

    public ObservableDictionary<valueDataEnum, float> values;
    public List<Equip> playerWeapons = new();
    public List<Marker2D> markers = new();
    [Export] private mainProperty zhuShuXing;
    [Export] public GridContainer equippedContainer;
    [Export] public ProgressBar hpBar;

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
        foreach (var variable in markers)
            if (variable.GetChildCount() > 0)
            {
                GD.Print("added it");
                playerWeapons.Add(variable.GetChild<Equip>(0));
            }
    }

    public void addWeaponsFromShop(EquipInfo inEquipInfo)
    {
        var eqp = MatchIt.Instance.matchWeapon(inEquipInfo.weaponType).Instantiate() as Equip;
        playerWeapons.Add(eqp);
        displayWeaponsInGridContainer();
        foreach (var variable in markers)
            if (variable.GetChildCount() <= 0)
            {
                variable.AddChild(eqp);
                break;
            }
    }

    public void displayWeaponsInGridContainer()
    {
        var i = 0;
        GD.Print($"playerWeapons: {playerWeapons.Count}");
        foreach (var node in equippedContainer.GetChildren())
        {
            var button = (Button)node;
            if (i < playerWeapons.Count)
                button.Icon = playerWeapons[i++].icon;
        }
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
        ValuesList.Add(crtHp);
        values = new ObservableDictionary<valueDataEnum, float>();
        //values[]
        foreach (var variable in GetNode<Node2D>("markers").GetChildren()) markers.Add((Marker2D)variable);
        AddWeaponsToPlayer();
        displayWeaponsInGridContainer();
        // 订阅事件
        values.OnItemAdded += (key, value, isFst)
            =>
        {
            GD.Print($"Key '{key}' added with value {value}");
            if (!isFst)
            {
                zhuShuXing.SetItemText((int)key, $"{key}: {value}");
                matchEnumAndHuiDiao(key, value);
            }
            else
            {
                zhuShuXing.AddItem($"{key}: {value}");
            }
            //zhuShuXing.AddItem($"{key}: {value}");
        };
        var i = 0;
        foreach (valueDataEnum value in Enum.GetValues(typeof(valueDataEnum))) values.Add(value, ValuesList[i++], true);
        collisionShape2D.Shape.Set("radius", pickRadius + values.GetValueOrDefault(valueDataEnum.range, 0));
        hpBar.MaxValue = values[valueDataEnum.hp];
        CurrentHp = values[valueDataEnum.hp];
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        //moneyShow = GetTree().CurrentScene.GetNode<Label>("CanvasLayer/showMoney");
    }

    private void matchEnumAndHuiDiao(valueDataEnum inType, float inValue)
    {
        switch (inType)
        {
            case valueDataEnum.hp:
                hpBar.MaxValue = inValue;
                GD.Print($"{inType} has been set to {inValue}");
                break;
            case valueDataEnum.atk:
                GD.Print($"{inType} has been set to {inValue}");
                break;
            case valueDataEnum.def:
                GD.Print($"{inType} has been set to {inValue}");
                break;
            case valueDataEnum.speed:
                GD.Print($"{inType} has been set to {inValue}");
                break;
            case valueDataEnum.range:
                GD.Print($"{inType} has been set to {inValue}");
                break;
            case valueDataEnum.spAtk:
                GD.Print($"{inType} has been set to {inValue}");
                break;
            case valueDataEnum.luckyValue:
                GD.Print($"{inType} has been set to {inValue}");
                break;
            case valueDataEnum.drain:
                GD.Print($"{inType} has been set to {inValue}");
                break;
            case valueDataEnum.regeneration:
                GD.Print($"{inType} has been set to {inValue}");
                break;
            case valueDataEnum.criticalRate:
                GD.Print($"{inType} has been set to {inValue}");
                break;
            case valueDataEnum.crtHp:
                GD.Print($"{inType} has been set to {inValue}");
                break;
            default:
                break;
        }
    }

    public override void _Process(double delta)
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
}