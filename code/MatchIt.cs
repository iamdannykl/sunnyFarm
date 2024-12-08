using Godot;
using System;
using System.Collections.Generic;

public partial class MatchIt : Node
{
	public static MatchIt Instance;
	public Dictionary<enemyTypeEnum, PackedScene> findEnemy;

	[Export] private PackedScene blueBird;

	[Export] private PackedScene angryPig;

	[Export] private PackedScene ghost;

	[Export] public PackedScene coin;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
		findEnemy = new Dictionary<enemyTypeEnum, PackedScene>
		{
			{ enemyTypeEnum.blueBird, blueBird },
			{ enemyTypeEnum.angryPig, angryPig },
			{ enemyTypeEnum.ghost, ghost }
		};
	}

	public PackedScene matchEnemy(enemyTypeEnum type)
	{
		if (findEnemy.TryGetValue(type, out PackedScene scene))
		{
			return scene;
		}
		return null;
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
