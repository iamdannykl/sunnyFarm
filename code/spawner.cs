using Godot;
using Microsoft.VisualBasic.FileIO;
using System;
using System.IO;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

public enum
enemyTypeEnum
{
	blueBird = 0,
	angryPig = 1,
	ghost = 2
}
public partial class spawner : Node2D
{
	[Export] public PackedScene enemy1;
	[Export] private float minSpawnDistance;
	[Export] public Label lastTime;
	private Node2D zx, ys;
	Random random = new Random();
	private float xJL, yJL;
	Vector2 lastPos, currentPos;
	public guanQia level;
	wave currentWave;
	int crtWaveNum;
	private Panel nextWavePanel;
	
	[Export]Timer countdownTimer;
	private int countdown; // 倒计时
	private int crtLitWaveNum;
	public override void _Ready()
	{
		zx = GetTree().CurrentScene.GetNode<Node2D>("land/zx");
		ys = GetTree().CurrentScene.GetNode<Node2D>("land/ys");
		xJL = (ys.GlobalPosition - zx.GlobalPosition).X;
		yJL = (ys.GlobalPosition - zx.GlobalPosition).Y;
		lastPos = Vector2.Zero;
		nextWavePanel =GetTree().CurrentScene.GetNode("CanvasLayer/nextWave") as Panel;
		readAndStart();
	}
	void readAndStart()
	{
		string userDir = ProjectSettings.GlobalizePath("user://");
		string folderPath = Path.Combine(userDir, "saveFolder");
		string realPath = Path.Combine(folderPath, "level.yaml");
		if (File.Exists(realPath))
		{
			var sr = new StreamReader(realPath);
			string yamlStr = sr.ReadToEnd();
			var deSer = new DeserializerBuilder().Build();
			level = deSer.Deserialize<guanQia>(yamlStr);
			currentWave = level.waves[crtWaveNum];
			GD.Print($"level.waves.Count:{level.waves.Count}");
		}

		countdown = currentWave.litWaves.Count * 5;
		lastTime.Text = countdown.ToString();
		countdownTimer.Start();
	}

	void OnCountdownTick()
	{
		if (countdown % 12 == 0)
		{
			OnMethodCall();
		}
		countdown--;
		lastTime.Text = countdown.ToString();

		if (countdown <= 0)
		{
			countdownTimer.Stop();
			finishThisWave();
		}
	}
	void OnMethodCall()
	{
		foreach (enemyType ene in currentWave.litWaves[crtLitWaveNum++].enemyTypes)
		{
			for (int i = 0; i < ene.num; i++)
			{
				spawnEntity();
			}
		}
	}

	async void startSpawn()
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
	}
	public void finishThisWave()
	{
		nextWavePanel.Visible = true;
	}
	public void enterNextWave()
	{
		nextWavePanel.Visible = false;
		crtWaveNum++;
		currentWave = level.waves[crtWaveNum];
		
		countdown = currentWave.litWaves.Count * 5;
		lastTime.Text = countdown.ToString();
		countdownTimer.Start();
	}
	

	public void spawnEntity()
	{
		enemyBase enemyNew = enemy1.Instantiate() as enemyBase;
		AddChild(enemyNew);
		currentPos = zx.GlobalPosition + new Vector2(xJL * random.NextSingle(), yJL * random.NextSingle());
		int num = 0;
		while (lastPos.DistanceTo(currentPos) <= minSpawnDistance && num <= 3)
		{
			currentPos = zx.GlobalPosition + new Vector2(xJL * random.NextSingle(), yJL * random.NextSingle());
			num++;
		}
		enemyNew.GlobalPosition = currentPos;
	}
}
