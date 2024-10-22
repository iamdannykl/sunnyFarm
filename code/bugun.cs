using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class bugun : Area2D
{
	[Export] public PackedScene blt;
	Godot.Collections.Array<Area2D> enemyList;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		enemyList = GetOverlappingAreas();
		GD.Print(enemyList.Count);
		if (enemyList.Count > 0)
		{
			LookAt(enemyList[0].GlobalPosition);
		}
	}
	public void shootIt()
	{
		if (enemyList.Count > 0)
		{
			Bullet bltNew = blt.Instantiate() as Bullet;
			AddChild(bltNew);
		}
		else
		{
			GlobalRotation = 0;
		}
	}
}
