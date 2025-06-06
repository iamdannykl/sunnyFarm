using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace SunnyFarm.code;

public enum bulletType
{
    normal,
    fire
}

public partial class baseGun : Equip
{
    [Export] public PackedScene blt;
    [Export] public bulletType btp;
    [Export] public float atkValue;
    private Sprite2D gunImage;
    private Godot.Collections.Array<Area2D> enemyList = new();
    private BasicsCore _basicsCore;
    private Marker2D bulletMarker;

    private AudioStreamPlayer bulletSound;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        gunImage = GetNode<Sprite2D>("Sprite2D");
        _basicsCore = GetParent().GetParent().GetParent<BasicsCore>();
        bulletMarker = GetNode<Marker2D>("Marker2D");
        bulletSound = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        //enemyList = GetOverlappingAreas();
        //GD.Print(enemyList.Count);
        Area2D tar = enemyList.OrderBy(a => a.GlobalPosition.DistanceTo(_basicsCore.GlobalPosition)).FirstOrDefault();
        if (tar!=null) LookAt(tar.GlobalPosition);
        //GD.Print(GlobalRotationDegrees);
        if (Math.Abs(GlobalRotationDegrees) > 90 && MathF.Abs(GlobalRotationDegrees) < 180)
            gunImage.FlipV = true;
        else
            gunImage.FlipV = false;
    }

    public void addEnemy(Area2D area)
    {
        enemyList.Add(area);
        area.GetParent<enemyBase>().removeSelf += () =>
        {
            GD.Print("has been removed!");
            enemyList.Remove(area);
        };
    }

    public void removeEnemy(Area2D area)
    {
        enemyList.Remove(area);
    }

    public void shootIt()
    {
        if (enemyList.Count > 0)
        {
            bulletSound.Play();
            var bltNew = MatchIt.Instance.matchBullet(btp).Instantiate() as Bullet;
            if (bltNew == null) return;
            bltNew.atkValue = atkValue + _basicsCore.values.GetValueOrDefault(valueDataEnum.atk, 0);
            bltNew.GlobalPosition = bulletMarker.GlobalPosition;
            bltNew.GlobalRotation = bulletMarker.GlobalRotation;
            spawner.Instance.AddChild(bltNew);
            //bulletMarker.AddChild(bltNew);

            //GD.Print(Zaodian.Instance.GetNoiseBasedPosition(GlobalPosition, 0.3f));
        }
        else
        {
            GlobalRotation = 0;
        }
    }

    public override void getThisEqip()
    {
        
    }
}