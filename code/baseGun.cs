/*using System;
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
    private List<Area2D> enemyList = new();
    private BasicsCore _basicsCore;
    private Marker2D bulletMarker;

    private AudioStreamPlayer bulletSound;
    Area2D targetObject;

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
        aimming();
    }

    void aimTheTarget()
    {
        if(enemyList.Count<=0) return;
        targetObject = enemyList.MinBy(a=>a.GlobalPosition.DistanceTo(_basicsCore.GlobalPosition));
        if (IsInstanceValid(targetObject))
        {
            shootIt();
        }
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

    public void aimming()
    {
        if (IsInstanceValid(targetObject))
            LookAt(targetObject.GlobalPosition);
        //GD.Print(GlobalRotationDegrees);
        if (Math.Abs(GlobalRotationDegrees) > 90 && MathF.Abs(GlobalRotationDegrees) < 180)
            gunImage.FlipV = true;
        else
            gunImage.FlipV = false;
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
}*/
using System;
using System.Collections.Generic;
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
    private List<Area2D> enemyList = new();
    private BasicsCore _basicsCore;
    private Marker2D bulletMarker;
    private AudioStreamPlayer bulletSound;

    private Area2D targetObject;

    private float _findTargetTimer = 0f;
    private const float TargetSearchInterval = 1f; // 每 0.2 秒查找一次最近敌人

    public override void _Ready()
    {
        gunImage = GetNode<Sprite2D>("Sprite2D");
        _basicsCore = GetParent().GetParent().GetParent<BasicsCore>();
        bulletMarker = GetNode<Marker2D>("Marker2D");
        bulletSound = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
    }

    public override void _Process(double delta)
    {
        _findTargetTimer -= (float)delta;

        if (_findTargetTimer <= 0f)
        {
            _findTargetTimer = TargetSearchInterval;
            FindNearestEnemy();
        }

        AimAtTarget();
    }

    private void FindNearestEnemy()
    {
        float nearestDistance = float.MaxValue;
        Area2D nearest = null;

        foreach (var enemy in enemyList)
        {
            if (enemy == null || !IsInstanceValid(enemy)) continue;

            float distance = enemy.GlobalPosition.DistanceTo(_basicsCore.GlobalPosition);
            if (distance < nearestDistance)
            {
                nearest = enemy;
                nearestDistance = distance;
            }
        }

        targetObject = nearest;
        if (targetObject != null)
        {
            Shoot();
        }
    }

    public void addEnemy(Area2D area)
    {
        if (!enemyList.Contains(area))
        {
            enemyList.Add(area);
            area.GetParent<enemyBase>().removeSelf += () =>
            {
                enemyList.Remove(area);
                if (area == targetObject)
                {
                    targetObject = null;
                }
            };
        }
    }

    public void removeEnemy(Area2D area)
    {
        enemyList.Remove(area);
        if (area == targetObject)
        {
            targetObject = null;
        }
    }

    private void AimAtTarget()
    {
        if (IsInstanceValid(targetObject))
        {
            LookAt(targetObject.GlobalPosition);

            float rot = GlobalRotationDegrees;
            gunImage.FlipV = Math.Abs(rot) > 90 && Math.Abs(rot) < 180;
        }
        else
        {
            GlobalRotation = 0;
            gunImage.FlipV = false;
        }
    }

    private void Shoot()
    {
        if (!IsInstanceValid(targetObject)) return;

        bulletSound.Play();
        var bltNew = MatchIt.Instance.matchBullet(btp).Instantiate() as Bullet;
        if (bltNew == null) return;

        bltNew.atkValue = atkValue + _basicsCore.values.GetValueOrDefault(valueDataEnum.atk, 0);
        bltNew.GlobalPosition = bulletMarker.GlobalPosition;
        bltNew.GlobalRotation = bulletMarker.GlobalRotation;
        spawner.Instance.AddChild(bltNew);
    }

    public override void getThisEqip()
    {
        // implement if needed
    }
}
