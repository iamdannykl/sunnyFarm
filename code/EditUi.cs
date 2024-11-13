using Godot;
using SunnyFarm.code;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using YamlDotNet.Serialization;

public partial class EditUi : Control
{
	public static EditUi Instance;
	public int waveNum = -1;
	public int litWaveNum = -1;
	public guanQia gq;
	public List<List<enemyType>> enemyTypes = new List<List<enemyType>>();
	[Export] public VBoxContainer vBoxContainer;
	[Export] public PackedScene enemyPanel;
	[Export] public ItemList waveN, lWaveN;
	public int WaveNum
	{
		get => waveNum;
		set
		{
			confirmIt();
			waveNum = value;
			locateIt();
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
	public void setWaveNum(int idx)
	{
		WaveNum = idx;
	}
	public void setLitWaveNum(int idx)
	{
		LitWaveNum = idx;
	}
	void locateIt()
	{
		if (waveNum == -1 || litWaveNum == -1) return;
		foreach (var enemyData in vBoxContainer.GetChildren())
		{
			enemyData.QueueFree();
		}//clear all items in Vbox
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
		gq.waves[waveNum].litWaves[litWaveNum].enemyTypes.Clear();
		foreach (EnemyPanel enemyPanel in vBoxContainer.GetChildren())
		{
			int nm = 0;
			int cirnm = 0;
			if (enemyPanel.num.Text.Length > 0)
			{
				nm = enemyPanel.num.Text.ToInt();
			}
			if (enemyPanel.cirNum.Text.Length > 0)
			{
				cirnm = enemyPanel.cirNum.Text.ToInt();
			}
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
		GD.PrintT(waveN.ItemCount, lWaveN.ItemCount);
		for (int i = 0; i < waveN.ItemCount; i++)
		{
			gq.waves.Add(new wave(1));
		}
		GD.Print(gq.waves.Count);
	}

	public void finishIt()
	{
		confirmIt();
		var ser = new SerializerBuilder().Build();
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
		if (File.Exists(realPath))
		{
			File.Delete(realPath);
		}
		StreamWriter sw = new StreamWriter(realPath);
		sw.Write(yamlStr);
		sw.Close();
	}
}
