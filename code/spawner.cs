using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;
using SkiaSharp;
using YamlDotNet.Serialization;
using Environment = System.Environment;

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
    [Export] public HBoxContainer goodsContainer;
    public List<enemyBase> enemies = new();
    [Export] private Node2D zx, ys;
    private Random random = new();
    private float xJL, yJL;
    private Vector2 lastPos, currentPos;
    public guanQia level;
    private wave currentWave;
    private int crtWaveNum;
    [Export] private Panel nextWavePanel;
    [Export] public PackedScene redX;

    [Export] private Timer countdownTimer;
    private int countdown; // 倒计时
    private int crtLitWaveNum;
    public Queue<enemyTypeEnum> types = new();

    public override void _Ready()
    {
        Instance = this;
        /*zx = GetTree().CurrentScene.GetNode<Node2D>("land/zx");
        ys = GetTree().CurrentScene.GetNode<Node2D>("land/ys");*/
        xJL = (ys.GlobalPosition - zx.GlobalPosition).X;
        yJL = (ys.GlobalPosition - zx.GlobalPosition).Y;
        lastPos = Vector2.Zero;
        //nextWavePanel = GetTree().CurrentScene.GetNode("CanvasLayer/nextWave") as Panel;
        readAndStart();
    }


    private void readAndStart()
    {
        var userDir = ProjectSettings.GlobalizePath("user://");
        var folderPath = Path.Combine(userDir, "saveFolder");
        var realPath = Path.Combine(folderPath, "level.yaml");
        GD.Print($"fould path:{realPath}");
        if (File.Exists(realPath))
        {
            GD.Print("fould exist");
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

    public void finishThisWave()
    {
        TestShop();
        if (crtWaveNum >= level.waves.Count - 1)
        {
            nextWavePanel.GetNode<Button>("continue").Visible = false;
            nextWavePanel.GetNode<Button>("finish").Visible = true;
        }

        foreach (var ene in enemies)
            //ene?.QueueFree();
            try
            {
                ene.QueueFree();
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

    public static Dictionary<Rarity, float> GetAdjustedRarityWeights(int wave)
    {
        var initialValues = new Dictionary<Rarity, float>
        {
            { Rarity.Common, 1f },
            { Rarity.Rare, 0f },
            { Rarity.Epic, 0f },
            { Rarity.Mythic, 0f },
            { Rarity.Legendary, 0f }
        };

        var targetValues = new Dictionary<Rarity, float>
        {
            { Rarity.Common, 0.10f },
            { Rarity.Rare, 0.23f },
            { Rarity.Epic, 0.32f },
            { Rarity.Mythic, 0.25f },
            { Rarity.Legendary, 0.10f }
        };

        // 特殊参数（峰值和宽度）
        var rareWavePeak = 15; // Rare 稀有度的峰值波数
        var epicWavePeak = 20; // Epic 稀有度的峰值波数
        var sigma = 5.0f; // 控制峰值曲线的宽度
        var k = 0.1f; // 控制指数增长/衰减速度

        // 计算稀有度权重
        var weights = new Dictionary<Rarity, float>();

        foreach (var rarity in initialValues.Keys)
        {
            var probability = 0f;

            switch (rarity)
            {
                case Rarity.Common:
                    // 指数衰减
                    probability = targetValues[rarity] +
                                  (initialValues[rarity] - targetValues[rarity]) * Mathf.Exp(-k * 2f * wave);
                    break;

                case Rarity.Rare:
                    // 先升后降（正态分布）
                    var rarePeak = 0.12f; // Rare 的峰值概率
                    probability = targetValues[rarity] +
                                  (initialValues[rarity] - targetValues[rarity]) * Mathf.Exp(-k * 0.25f * wave);
                    break;

                case Rarity.Epic:
                    // 先升后降（正态分布）
                    var epicPeak = 0.18f; // Epic 的峰值概率
                    probability = targetValues[rarity] +
                                  (initialValues[rarity] - targetValues[rarity]) * Mathf.Exp(-k * 0.1f * wave);
                    break;

                case Rarity.Mythic:
                    // 指数增长
                    probability = initialValues[rarity] +
                                  (targetValues[rarity] - initialValues[rarity]) * (1 - Mathf.Exp(-k * 0.08f * wave));
                    break;

                case Rarity.Legendary:
                    // 指数增长
                    probability = initialValues[rarity] +
                                  (targetValues[rarity] - initialValues[rarity]) * (1 - Mathf.Exp(-k * 0.1f * wave));
                    break;
            }

            weights[rarity] = probability;
        }

        // 归一化处理，确保总和为 1
        var totalWeight = weights.Values.Sum();
        foreach (var rarity in weights.Keys.ToList()) weights[rarity] /= totalWeight;

        return weights;
    }

    private EquipInfo findSingleEquip(List<Tags> tags, Rarity rarity)
    {
        // 筛选符合稀有度的武器
        var candidates =
            /*new List<Equip>();*/
            MatchIt.Instance.equipInfos.Where(weapon => weapon.Rarity == rarity).ToList();
        // 如果有玩家武器标签，优先选择含有相同标签的武器
        if (tags != null && tags.Count > 0)
        {
            var taggedCandidates = candidates
                .Where(weapon => weapon.Tags.Any(tag => tags.Contains(tag)))
                .ToList();
            if (taggedCandidates.Count > 0) candidates = taggedCandidates;
        }

        return candidates.OrderBy(x => Guid.NewGuid()).FirstOrDefault() ?? findSingleEquip(tags, Rarity.Common);
    }

    public List<EquipInfo> RefreshShop(int wave, List<Tags> playerWeaponTags)
    {
        // 获取调整后的稀有度权重
        var rarityWeights = GetAdjustedRarityWeights(wave);
        foreach (var VARIABLE in rarityWeights) GD.Print($"wave:{wave + 1}rare:{VARIABLE.Key}value:{VARIABLE.Value}");
        var weaponList = new List<EquipInfo>();
        for (var i = 0; i < 5; i++)
        {
            // 按权重随机选择稀有度
            var selectedRarity = WeightedRandomSelect(rarityWeights);


            weaponList.Add(findSingleEquip(playerWeaponTags, selectedRarity));
        }

        return weaponList;
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

    public void EndWave(int wave, List<Equip> playerWeapons)
    {
        // 获取玩家武器的标签集合
        GD.Print($"player:{playerWeapons.Count}");
        var playerTags = playerWeapons.SelectMany(weapon => weapon.MyTagsList).Distinct().ToList();
        // 刷新商店
        var shopWeapons = RefreshShop(wave, playerTags);
        // 显示商店中的武器
        GD.Print("**********************************************");
        /*foreach (var weapon in shopWeapons)
            GD.Print($"Shop Weapon: {weapon.weaponType.ToString()}, Rarity: {weapon.Rarity}");*/
        for (var i = 0; i < shopWeapons.Count; i++)
        {
            var panel = goodsContainer.GetChild<Panel>(i);
            var currentWeapon = shopWeapons[i];
            panel.GetNode<TextureRect>("TextureRect").Texture = currentWeapon.icon;
            if (!currentWeapon.isProps)
            {
                panel.GetNode<Label>("name").Text = currentWeapon.weaponType.ToString();
            }
            else
            {GD.Print("无尽："+currentWeapon.zhuangBeiType);
                panel.GetNode<Label>("name").Text = currentWeapon.zhuangBeiType.ToString();
            }
            panel.GetNode<Label>("describe").Text = currentWeapon.discribe;
            panel.GetNode<buy>("price").equipInfo = currentWeapon;
            panel.GetNode<buy>("price").Text = currentWeapon.price.ToString();
        }
    }

    public void TestShop()
    {
        GD.Print("ending wave");
        var wave = 100; // 当前波数
        var playerWeapons = BasicsCore.Instance.playerWeapons;
        GD.Print($"playerWeapons: {playerWeapons.Count}");
        /*for (var i = 0; i < wave; i++)
            EndWave(i, playerWeapons);*/
        EndWave(crtWaveNum, playerWeapons);
    }

    public void refreshShop()
    {
        TestShop();
    }
}