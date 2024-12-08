using Godot;
using System;

public partial class test2D : Node2D
{
	[Export] private PackedScene coinBI;
	void spawnIt()
	{
		Area2D coin=coinBI.Instantiate() as Area2D;
		AddChild(coin);
	}
}
