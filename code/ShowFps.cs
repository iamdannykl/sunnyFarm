using Godot;
using System;

public partial class ShowFps : Label
{
	public override void _Process(double delta)
	{
		Text = $"Fps:{1 / delta}";
	}
}
