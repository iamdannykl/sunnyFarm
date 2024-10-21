using Godot;
using System;
using System.Linq;

public partial class bugun : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var enemyList = GetOverlappingAreas();
		GD.Print(enemyList.Count);
		if (enemyList.Count > 0)
		{
			LookAt(enemyList[0].GlobalPosition);
		}
	}
}
