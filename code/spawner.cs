using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;
using YamlDotNet.Serialization;

namespace SunnyFarm.code;

public enum
    enemyTypeEnum
{
    blueBird = 0,
    angryPig = 1,
    ghost = 2
}

public partial class spawner : Node2D
{
    public static spawner Instance;
    [Export] public PackedScene enemy1;
    [Export] private float minSpawnDistance;
    [Export] public Label lastTime;
    public List<enemyBase> enemies = new();
    private Node2D zx, ys;
    private Random random = new();
    private float xJL, yJL;
    private Vector2 lastPos, currentPos;
    public guanQia level;
    private wave currentWave;
    private int crtWaveNum;
    private Panel nextWavePanel;
    [Export] public PackedScene redX;

    [Export] private Timer countdownTimer;
    private int countdown; // 倒计时
    private int crtLitWaveNum;
    public Queue<enemyTypeEnum> types = new();

    public override void _Ready()
    {
        Instance = this;
        zx = GetTree().CurrentScene.GetNode<Node2D>("land/zx");
        ys = GetTree().CurrentScene.GetNode<Node2D>("land/ys");
        xJL = (ys.GlobalPosition - zx.GlobalPosition).X;
        yJL = (ys.GlobalPosition - zx.GlobalPosition).Y;
        lastPos = Vector2.Zero;
        nextWavePanel = GetTree().CurrentScene.GetNode("CanvasLayer/nextWave") as Panel;
        readAndStart();
    }

    private void readAndStart()
    {
        var userDir = ProjectSettings.GlobalizePath("user://");
        var folderPath = Path.Combine(userDir, "saveFolder");
        var realPath = Path.Combine(folderPath, "level.yaml");
        if (File.Exists(realPath))
        {
            var sr = new StreamReader(realPath);
            var yamlStr = sr.ReadToEnd();
            var deSer = new DeserializerBuilder().Build();
            level = deSer.Deserialize<guanQia>(yamlStr);
            currentWave = level.waves[crtWaveNum];
            //GD.Print($"1-2 type:{level.waves[0].litWaves[1].enemyTypes[0].type}");
            //GD.Print($"level.waves.Count:{level.waves.Count}");
        }

        countdown = currentWave.litWaves.Count * 5;
        lastTime.Text = countdown.ToString();
        countdownTimer.Start();
    }

    private void OnCountdownTick()
    {
        if (countdown % 5 == 0) OnMethodCall();
        countdown--;
        lastTime.Text = countdown.ToString();

        if (countdown <= 0)
        {
            countdownTimer.Stop();
            finishThisWave();
        }
    }

    private void OnMethodCall()
    {
        foreach (var ene in currentWave.litWaves[crtLitWaveNum++].enemyTypes)
            for (var i = 0; i < ene.num; i++)
            {
                GD.Print($"type:{ene.type}crtLitWaveNum:{crtLitWaveNum}");
                spawnEntity(ene.type);
            }
    }

    /*async void startSpawn()
    {
        foreach (litWave lwv in currentWave.litWaves)
        {
            foreach (enemyType ene in lwv.enemyTypes)
            {
                for (int i = 0; i < ene.num; i++)
                {
                    spawnEntity();
                }
            }
            await Task.Delay(5000);
        }
        finishThisWave();
    }*/
    public void finishThisWave()
    {
        TestShop();
        if (crtWaveNum >= level.waves.Count - 1)
        {
            nextWavePanel.GetNode<Button>("continue").Visible = false;
            nextWavePanel.GetNode<Button>("finish").Visible = true;
        }

        foreach (var ene in enemies)
            try
            {
                ene.QueueFree(); //add a lock
            }
            catch (Exception e)
            {
                GD.Print(e);
            }

        nextWavePanel.Visible = true;
    }

    public void enterNextWave()
    {
        if (crtWaveNum >= level.waves.Count - 1) return;
        nextWavePanel.Visible = false;
        crtWaveNum++;
        currentWave = level.waves[crtWaveNum];
        crtLitWaveNum = 0;
        countdown = currentWave.litWaves.Count * 5;
        lastTime.Text = countdown.ToString();
        countdownTimer.Start();
    }

    public void finishTheGame()
    {
        GetTree().Quit();
    }

    public void spawnEntity(enemyTypeEnum type)
    {
        types.Enqueue(type);
        var redCross = redX.Instantiate() as Sprite2D;
        currentPos = zx.GlobalPosition + new Vector2(xJL * random.NextSingle(), yJL * random.NextSingle());
        var num = 0;
        while (lastPos.DistanceTo(currentPos) <= minSpawnDistance && num <= 3)
        {
            currentPos = zx.GlobalPosition + new Vector2(xJL * random.NextSingle(), yJL * random.NextSingle());
            num++;
        }

        if (redCross == null) return;
        redCross.GlobalPosition = currentPos;
        AddChild(redCross);
    }

    public void outQueue()
    {
    }

    public void AddChild(Area2D area, bool chk)
    {
        AddChild(area);
    }

// 每个稀有度对应的权重
    public static readonly Dictionary<Rarity, float> BaseRarityWeights = new()
    {
        { Rarity.Common, 50f },
        { Rarity.Rare, 30f },
        { Rarity.Epic, 15f },
        { Rarity.Mythic, 4f },
        { Rarity.Legendary, 1f }
    };

    public static Dictionary<Rarity, float> GetAdjustedRarityWeights(int wave)
    {
        var adjustedWeights = new Dictionary<Rarity, float>(BaseRarityWeights);

        foreach (var rarity in BaseRarityWeights.Keys) adjustedWeights[rarity] *= 1 + (wave - 1) * 0.1f; // 每波增加10%概率

        // 增强高品质武器概率
        adjustedWeights[Rarity.Common] *= Mathf.Max(0.5f, 1 - 0.1f * wave); // 减少低品质概率
        return adjustedWeights;
    }

    public List<Weapon> RefreshShop(int wave, List<string> playerWeaponTags)
    {
        // 获取调整后的稀有度权重
        var rarityWeights = GetAdjustedRarityWeights(wave);

        // 按权重随机选择稀有度
        var selectedRarity = WeightedRandomSelect(rarityWeights);
        GD.Print($"selectedRarity:{selectedRarity}");
        // 筛选符合稀有度的武器
        var candidates = player.WeaponPool.Where(weapon => weapon.Rarity == selectedRarity).ToList();
        GD.Print($"selectWeapon:{candidates.Count}");
        // 如果有玩家武器标签，优先选择含有相同标签的武器
        if (playerWeaponTags != null && playerWeaponTags.Count > 0)
        {
            var taggedCandidates = candidates.Where(weapon => weapon.Tags.Any(tag => playerWeaponTags.Contains(tag)))
                .ToList();
            if (taggedCandidates.Count > 0) candidates = taggedCandidates;
        }

        // 随机从候选武器中选择5件
        return candidates.OrderBy(_ => GD.Randf()).Take(5).ToList();
    }

// 按权重随机选择一个稀有度
    public Rarity WeightedRandomSelect(Dictionary<Rarity, float> weights)
    {
        var totalWeight = weights.Values.Sum();
        var randomPoint = GD.Randf() * totalWeight;

        foreach (var kvp in weights)
        {
            if (randomPoint < kvp.Value)
                return kvp.Key;
            randomPoint -= kvp.Value;
        }

        return weights.Keys.First(); // 默认返回第一个稀有度（不太可能触发）
    }

    public void EndWave(int wave, List<Weapon> playerWeapons)
    {
        GD.Print($"it's ending wave:{wave}");
        // 获取玩家武器的标签集合
        var playerTags = playerWeapons.SelectMany(weapon => weapon.Tags).Distinct().ToList();
        GD.Print($"tags:{playerTags.Count}");
        // 刷新商店
        var shopWeapons = RefreshShop(wave, playerTags);
        GD.Print($"shop count:{shopWeapons.Count}");
        // 显示商店中的武器
        foreach (var weapon in shopWeapons) GD.Print($"Shop Weapon: {weapon.Name}, Rarity: {weapon.Rarity}");
    }

    public void TestShop()
    {
        GD.Print("ending wave");
        var wave = 5; // 当前波数
        var playerWeapons = new List<Weapon>
        {
            new("Basic Sword", new List<string> { "Melee" }, Rarity.Common),
            new("Magic Wand", new List<string> { "Magic" }, Rarity.Rare)
        };

        EndWave(wave, playerWeapons);
    }
}