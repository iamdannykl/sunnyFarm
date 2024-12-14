using Godot;

namespace SunnyFarm.code;

public partial class mainProperty : ItemList
{
    private player _player;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //wait2s();
    }

    private async void wait2s()
    {
        _player = player.Instance;
        GD.Print($"Waiting 2 seconds");
        await ToSignal(GetTree().CreateTimer(2f), "timeout");
        GD.Print($"after 2 seconds{_player.values.Count}");
        foreach (var VARIABLE in _player.values)
        {
            AddItem($"{VARIABLE.Key}: {VARIABLE.Value}");
            GD.Print($"has added {_player.values.Count}");
        }
    }
}