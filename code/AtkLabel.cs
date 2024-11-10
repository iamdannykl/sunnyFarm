using Godot;
using System.Threading.Tasks;

public partial class AtkLabel : Label
{
	public override void _Ready()
	{
		desSelf();
		ZIndex = 1;
	}
	async void desSelf()
	{
		await Task.Delay(200);
		QueueFree();
	}
}
