using Godot;
using System;

public partial class RedX : Sprite2D
{
    public void deQueue()
    {
        PackedScene enemyType = MatchIt.Instance.matchEnemy(spawner.Instance.types.Dequeue());
        enemyBase enemyNew = enemyType.Instantiate() as enemyBase;
        enemyNew.GlobalPosition = GlobalPosition;
        spawner.Instance.AddChild(enemyNew);
        spawner.Instance.enemies.Add(enemyNew);
        QueueFree();
    }
}
