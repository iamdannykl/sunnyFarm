using Godot;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

public partial class MatchIt : Node
{
	public static MatchIt Instance;
	public Dictionary<enemyTypeEnum, PackedScene> findEnemy;
	public Dictionary<bulletType, PackedScene> findBullet;

	[Export] private PackedScene blueBird;

	[Export] private PackedScene angryPig;

	[Export] private PackedScene ghost;

	[Export] public PackedScene coin;
	//******************************************************************************
	[Export] public PackedScene fire;
	[Export] public PackedScene normalBlt;
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
		findBullet = new Dictionary<bulletType, PackedScene>
		{
			{bulletType.normal, normalBlt},
			{bulletType.fire, fire}
		};
	}
	public PackedScene matchBullet(bulletType btp)
	{
		if (findBullet.TryGetValue(btp, out PackedScene result))
		{
			return result;
		}
		return null;
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
