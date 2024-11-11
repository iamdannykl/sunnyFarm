using Godot;
using System;
using System.Collections.Generic;
using SunnyFarm.code;
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
	private Node2D zx, ys;
	Random random = new Random();
	private float xJL, yJL;
	Vector2 lastPos, currentPos;
	public override void _Ready()
	{
		zx = GetTree().CurrentScene.GetNode<Node2D>("land/zx");
		ys = GetTree().CurrentScene.GetNode<Node2D>("land/ys");
		xJL = (ys.GlobalPosition - zx.GlobalPosition).X;
		yJL = (ys.GlobalPosition - zx.GlobalPosition).Y;
		lastPos = Vector2.Zero;
		/* var waveCrt = new wave()
		{
			num = 10,
			isCircle = false,
			circleNum = 0
		};
		var serializer = new SerializerBuilder().Build();
		string yamlStr = serializer.Serialize(waveCrt);
		GD.Print(yamlStr); */
	}
	void spawnPerLittleWave()
	{
		for (int i = 0; i < 10; i++)
			spawnEntity();
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
