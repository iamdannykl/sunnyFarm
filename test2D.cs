using Godot;

namespace SunnyFarm;

public partial class test2D : Node2D
{
    [Export] private PackedScene coinBI;

    private void spawnIt()
    {
        Area2D coin = coinBI.Instantiate() as Area2D;
        AddChild(coin);
    }
}