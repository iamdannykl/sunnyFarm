using System.Threading.Tasks;
using Godot;

namespace SunnyFarm.code;

public partial class AtkLabel : Label
{
    public override void _Ready()
    {
        desSelf();
        ZIndex = 1;
    }

    private async void desSelf()
    {
        await Task.Delay(200);
        QueueFree();
    }
}