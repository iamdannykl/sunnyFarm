using System;
using System.Collections.Generic;
using System.IO;
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
            GD.Print($"1-2 type:{level.waves[0].litWaves[1].enemyTypes[0].type}");
            GD.Print($"level.waves.Count:{level.waves.Count}");
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
}