using Godot;

namespace SunnyFarm.code;

public partial class Bullet : Area2D
{
    private float rangeBlt = 1200;
    [Export] public float speed;
    public float atkValue;
    private bool canAtk = true;
    private Godot.Collections.Array<Area2D> target;

    private float dis = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        var direction = Vector2.Right.Rotated(Rotation);
        Position += direction * speed * (float)delta;
        dis += speed * (float)delta;
        if (dis > rangeBlt) QueueFree();
    }

    private void attackObject(Area2D area2D)
    {
        if (!canAtk) return;
        canAtk = false;
        target = GetOverlappingAreas();
        if (target.Count > 0)
        {
            var iattackble = target[0].GetParent<Iattackble>();
            if (iattackble != null) iattackble.attacked(atkValue);
            QueueFree();
        }
    }
}