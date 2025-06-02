using Godot;

namespace SunnyFarm.code;

public partial class ShowFps : Label
{
    public override void _Process(double delta)
    {
        Text = $"Fps:{(int)(1 / delta)}";
    }
}