using Godot;

namespace SunnyFarm.code;

public partial class mainProperty : ItemList
{
    private BasicsCore _basicsCore;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //wait2s();
    }

    private async void wait2s()
    {
        _basicsCore = BasicsCore.Instance;
        GD.Print($"Waiting 2 seconds");
        await ToSignal(GetTree().CreateTimer(2f), "timeout");
        GD.Print($"after 2 seconds{_basicsCore.values.Count}");
        foreach (var VARIABLE in _basicsCore.values)
        {
            AddItem($"{VARIABLE.Key}: {VARIABLE.Value}");
            GD.Print($"has added {_basicsCore.values.Count}");
        }
    }
}