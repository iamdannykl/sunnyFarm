using Godot;
using System;

public partial class baseGun : Area2D
{
    [Export] public PackedScene blt;
    [Export] public float atkValue;
    Sprite2D gunImage;
    Godot.Collections.Array<Area2D> enemyList;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        gunImage = GetNode<Sprite2D>("Sprite2D");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        enemyList = GetOverlappingAreas();
        //GD.Print(enemyList.Count);
        if (enemyList.Count > 0)
        {
            LookAt(enemyList[0].GlobalPosition);
        }
        //GD.Print(GlobalRotationDegrees);
        if (Math.Abs(GlobalRotationDegrees) > 90 && MathF.Abs(GlobalRotationDegrees) < 180)
        {
            gunImage.FlipV = true;
        }
        else
        {
            gunImage.FlipV = false;
        }
    }
    public void shootIt()
    {
        if (enemyList.Count > 0)
        {
            GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
            Bullet bltNew = blt.Instantiate() as Bullet;
            bltNew.atkValue = atkValue;
            AddChild(bltNew);
            //GD.Print(Zaodian.Instance.GetNoiseBasedPosition(GlobalPosition, 0.3f));
        }
        else
        {
            GlobalRotation = 0;
        }
    }
}
