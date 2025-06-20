using System.Collections.Generic;
using System.IO;
using Godot;
using YamlDotNet.Serialization;

namespace SunnyFarm.code;

public partial class EditUi : Control
{
    public static EditUi Instance;
    public int waveNum = -1;
    public int litWaveNum = -1;
    public guanQia gq;
    public List<List<enemyType>> enemyTypes = new();
    [Export] public VBoxContainer vBoxContainer;
    [Export] public PackedScene enemyPanel;
    [Export] public ItemList waveN, lWaveN;
    private int maxWaveNum, maxLitWaveNum;

    public int WaveNum
    {
        get => waveNum;
        set
        {
            confirmIt();
            waveNum = value;
            if (gq.waves[waveNum].litWaves.Count > 0) locateIt();
        }
    }

    public int LitWaveNum
    {
        get => litWaveNum;
        set
        {
            confirmIt();
            litWaveNum = value;
            locateIt();
        }
    }

    public void addWave()
    {
        gq.waves.Add(new wave(1));
        waveN.AddItem($"{++maxWaveNum}");
    }

    public void addLitWave()
    {
        if (waveNum == -1) return;
        if (!waveN.IsSelected(waveNum)) return;
        gq.waves[waveNum].litWaves.Add(new litWave());
        lWaveN.AddItem($"{gq.waves[waveNum].litWaves.Count}");
    }

    public void setWaveNum(int idx)
    {
        LitWaveNum = -1;
        WaveNum = idx;
        int litNum = gq.waves[waveNum].litWaves.Count;
        foreach (Node enemyData in vBoxContainer.GetChildren()) enemyData.QueueFree(); //clear all items in Vbox
        lWaveN.Clear();
        for (int i = 0; i < litNum; i++) lWaveN.AddItem($"{i + 1}");
    }

    public void setLitWaveNum(int idx)
    {
        LitWaveNum = idx;
    }

    private void locateIt() //显示
    {
        if (waveNum == -1 || litWaveNum == -1) return;
        foreach (Node enemyData in vBoxContainer.GetChildren()) enemyData.QueueFree(); //clear all items in Vbox
        GD.Print($"wave,litwave:{waveNum}{litWaveNum}");
        if (gq.waves[waveNum].litWaves[litWaveNum].enemyTypes.Count <= 0) return;
        foreach (enemyType entity in gq.waves[waveNum].litWaves[litWaveNum].enemyTypes)
        {
            EnemyPanel newPanel = enemyPanel.Instantiate() as EnemyPanel;
            vBoxContainer.AddChild(newPanel);
            newPanel.num.Text = entity.num.ToString();
            newPanel.checkButton.ButtonPressed = entity.isCircle;
            newPanel.cirNum.Text = entity.circleNum.ToString();
            newPanel.optionButton.Selected = (int)entity.type;
            GD.PrintT((int)entity.type, entity.type);
        }
    }

    public void addEnemy()
    {
        if (waveNum == -1 || litWaveNum == -1) return;
        Panel ePanel = enemyPanel.Instantiate() as Panel;
        vBoxContainer.AddChild(ePanel);
    }

    public void confirmIt()
    {
        if (waveNum == -1 || litWaveNum == -1) return;
        GD.Print($"lit{litWaveNum} wave:{waveNum}");
        gq.waves[waveNum].litWaves[litWaveNum].enemyTypes.Clear();
        if (vBoxContainer.GetChildren().Count <= 0) return;
        foreach (EnemyPanel enemyPanel in vBoxContainer.GetChildren())
        {
            int nm = 0;
            int cirnm = 0;
            if (enemyPanel.num.Text.Length > 0) nm = enemyPanel.num.Text.ToInt();
            if (enemyPanel.cirNum.Text.Length > 0) cirnm = enemyPanel.cirNum.Text.ToInt();
            gq.waves[waveNum].litWaves[litWaveNum].enemyTypes.Add(new enemyType()
            {
                num = nm,
                isCircle = enemyPanel.checkButton.ButtonPressed,
                circleNum = cirnm,
                type = (enemyTypeEnum)enemyPanel.optionButton.Selected
            });
            GD.PrintT(enemyPanel.optionButton.Selected, (enemyTypeEnum)enemyPanel.optionButton.Selected);
        }
    }

    public override void _Ready()
    {
        Instance = this;
        gq = new guanQia();
        loadWave();
    }

    private void loadWave()
    {
        for (int i = 0; i < waveN.ItemCount; i++) gq.waves.Add(new wave(1));
        GD.Print(gq.waves.Count);
    }

    public void finishIt()
    {
        confirmIt();
        ISerializer ser = new SerializerBuilder().Build();
        string yamlStr = ser.Serialize(gq);
        GD.Print(yamlStr);
        string userDir = ProjectSettings.GlobalizePath("user://");
        string folderPath = Path.Combine(userDir, "saveFolder");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            GD.Print("Folder created: " + folderPath);
        }
        else
        {
            GD.Print("Folder already exists: " + folderPath);
        }

        string realPath = Path.Combine(folderPath, "level.yaml");
        if (File.Exists(realPath)) File.Delete(realPath);
        StreamWriter sw = new StreamWriter(realPath);
        sw.Write(yamlStr);
        sw.Close();
    }

    public void backToStartUI()
    {
        StartUi startUi = GD.Load<PackedScene>("res://scene/start_ui.tscn").Instantiate<StartUi>();
        GetTree().CurrentScene.QueueFree();
        GetTree().Root.AddChild(startUi);
        GetTree().CurrentScene = startUi;
    }
}