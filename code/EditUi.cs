using Godot;
using SunnyFarm.code;
using System;
using System.Collections.Generic;

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
	public void setWaveNum(int idx)
	{
		waveNum = idx;
		GD.PrintT(waveNum, litWaveNum);
		locateIt();
	}
	public void setLitWaveNum(int idx)
	{
		litWaveNum = idx;
		GD.PrintT(waveNum, litWaveNum);
		locateIt();
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
			newPanel.num.Text = entity.num.ToString();
			newPanel.checkButton.ButtonPressed = entity.isCircle;
			newPanel.cirNum.Text = entity.circleNum.ToString();
			newPanel.optionButton.Selected = (int)entity.type;
			vBoxContainer.AddChild(newPanel);
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
			gq.waves[waveNum].litWaves[litWaveNum].enemyTypes.Add(new enemyType()
			{
				num = enemyPanel.num.Text.ToInt(),
				isCircle = enemyPanel.checkButton.ButtonPressed,
				circleNum = enemyPanel.cirNum.Text.ToInt(),
				type = (enemyTypeEnum)enemyPanel.optionButton.Selected
			});
		}
	}
	public override void _Ready()
	{
		Instance = this;
		gq = new guanQia();
		GD.PrintT(waveN.ItemCount, lWaveN.ItemCount);
		for (int i = 0; i < waveN.ItemCount; i++)
		{
			gq.waves.Add(new wave(

			));
		}
		GD.Print(gq.waves.Count);
	}

	public void finishIt()
	{

	}
}
