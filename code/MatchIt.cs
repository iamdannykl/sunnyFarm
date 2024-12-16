using System.Collections.Generic;
using Godot;

namespace SunnyFarm.code;

public partial class MatchIt : Node
{
    public static MatchIt Instance;
    public Dictionary<enemyTypeEnum, PackedScene> findEnemy;
    public Dictionary<bulletType, PackedScene> findBullet;
    public Dictionary<weapons, PackedScene> findWeapons;

    [Export] private PackedScene blueBird;

    [Export] private PackedScene angryPig;

    [Export] private PackedScene ghost;

    [Export] public PackedScene coin;

    //******************************************************************************
    [Export] public PackedScene fire;

    [Export] public PackedScene normalBlt;

    //******************************************************************************
    [ExportCategory("weapons")] [Export] public PackedScene tutuGun;
    [Export] public PackedScene shortGun;
    [Export] public PackedScene flameThrower;
    //public List<weapons> 

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Instance = this;
        findEnemy = new Dictionary<enemyTypeEnum, PackedScene>
        {
            { enemyTypeEnum.blueBird, blueBird },
            { enemyTypeEnum.angryPig, angryPig },
            { enemyTypeEnum.ghost, ghost }
        };
        findBullet = new Dictionary<bulletType, PackedScene>
        {
            { bulletType.normal, normalBlt },
            { bulletType.fire, fire }
        };
        findWeapons = new Dictionary<weapons, PackedScene>
        {
            { weapons.tutuGun, tutuGun },
            { weapons.shortGun, shortGun },
            { weapons.flamethrower, flameThrower }
        };
    }

    public PackedScene matchBullet(bulletType btp)
    {
        if (findBullet.TryGetValue(btp, out var result)) return result;
        return null;
    }

    public PackedScene matchEnemy(enemyTypeEnum type)
    {
        if (findEnemy.TryGetValue(type, out var scene)) return scene;
        return null;
    }

    public PackedScene matchWeapon(weapons type)
    {
        if (findWeapons.TryGetValue(type, out var scene)) return scene;
        return null;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}