using Godot;

namespace SunnyFarm.code;

public partial class RedX : Sprite2D
{
    public void deQueue()
    {
        var enemyType = MatchIt.Instance.matchEnemy(spawner.Instance.types.Dequeue());
        var enemyNew = enemyType.Instantiate() as enemyBase;
        enemyNew.GlobalPosition = GlobalPosition;
        spawner.Instance.AddChild(enemyNew);
        spawner.Instance.enemies.Add(enemyNew);
        QueueFree();
    }
}