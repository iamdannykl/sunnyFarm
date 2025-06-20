
using System;
using System.Collections.Generic;
using Godot;

namespace SunnyFarm.code;

public partial class baseGun : Equip
{
    [Export] public PackedScene blt;
    [Export] public bulletType btp;
    [Export] public float atkValue;
    [Export] public float shootCooldown = 1.0f; // 添加射击冷却时间

    private Sprite2D gunImage;
    private List<Area2D> enemyList = new();
    private BasicsCore _basicsCore;
    private Marker2D bulletMarker;
    private AudioStreamPlayer bulletSound;

    private Area2D targetObject;
    private float _shootTimer = 0f; // 射击计时器

    public override void _Ready()
    {
        gunImage = GetNode<Sprite2D>("Sprite2D");
        _basicsCore = GetParent().GetParent().GetParent<BasicsCore>();
        bulletMarker = GetNode<Marker2D>("Marker2D");
        bulletSound = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
        
        // 初始查找敌人
        FindNearestEnemy();
    }

    public override void _Process(double delta)
    {
        // 检查目标是否有效，无效则重新索敌
        if (!IsInstanceValid(targetObject))
        {
            FindNearestEnemy();
        }
        
        // 射击冷却时间处理
        _shootTimer -= (float)delta;
        if (_shootTimer <= 0f && IsInstanceValid(targetObject))
        {
            Shoot();
            _shootTimer = shootCooldown;
        }

        AimAtTarget();
    }

    private void FindNearestEnemy()
    {
        // 清理无效敌人
        enemyList.RemoveAll(enemy => enemy == null || !IsInstanceValid(enemy));
        
        if (enemyList.Count == 0)
        {
            targetObject = null;
            return;
        }

        float nearestDistance = float.MaxValue;
        Area2D nearest = null;

        foreach (Area2D enemy in enemyList)
        {
            float distance = enemy.GlobalPosition.DistanceTo(_basicsCore.GlobalPosition);
            if (distance < nearestDistance)
            {
                nearest = enemy;
                nearestDistance = distance;
            }
        }

        targetObject = nearest;
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
                    // 目标移除时立即查找新目标
                    targetObject = null;
                    FindNearestEnemy();
                }
            };
            
            // 如果当前没有目标，立即将此敌人设为目标
            if (!IsInstanceValid(targetObject))
            {
                targetObject = area;
            }
        }
    }

    public void removeEnemy(Area2D area)
    {
        enemyList.Remove(area);
        if (area == targetObject)
        {
            // 目标移除时立即查找新目标
            targetObject = null;
            FindNearestEnemy();
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
    }

    private void Shoot()
    {
        if (!IsInstanceValid(targetObject)) return;

        bulletSound.Play();
        Bullet bltNew = MatchIt.Instance.matchBullet(btp).Instantiate() as Bullet;
        if (bltNew == null) return;
        bltNew.baseGun = this;
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

public enum bulletType
{
    normal,
    fire
}

