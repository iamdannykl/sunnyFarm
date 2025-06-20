using System.Collections.Generic;
using Godot;

namespace SunnyFarm.code;

public partial class MatchIt : Node
{
    public static MatchIt Instance;
    public Dictionary<enemyTypeEnum, PackedScene> findEnemy;
    public Dictionary<bulletType, PackedScene> findBullet;
    public Dictionary<weapons, PackedScene> findWeapons;
    public Dictionary<zhuangBei,PackedScene> findZhuangBei;
    public List<EquipInfo> equipInfos = new();

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

    [ExportCategory("zhuangBei")] [Export] public PackedScene infinityEdge;
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
        findZhuangBei = new Dictionary<zhuangBei, PackedScene>
        {
            { zhuangBei.infinityEdge, infinityEdge }
        };
        spawnTheInfoOfEquips();
    }

    private void spawnTheInfoOfEquips()
    {
        foreach (KeyValuePair<weapons, PackedScene> equip in findWeapons)
        {
            Equip temEquip = equip.Value.Instantiate() as Equip;
            if (temEquip != null)
                equipInfos.Add(new EquipInfo(temEquip.discribe, temEquip.weaponType, temEquip.MyTagsList,
                    temEquip.Rarity, temEquip.icon, temEquip.price, temEquip.isProps,temEquip.zhuangBeiType));
            if (temEquip != null) temEquip.QueueFree();
        }
        foreach (KeyValuePair<zhuangBei, PackedScene> zhuangBei in findZhuangBei)
        {
            Equip temEquip = zhuangBei.Value.Instantiate() as Equip;
            if (temEquip != null)
                equipInfos.Add(new EquipInfo(temEquip.discribe, temEquip.weaponType, temEquip.MyTagsList,
                    temEquip.Rarity, temEquip.icon, temEquip.price, temEquip.isProps,temEquip.zhuangBeiType));
            if (temEquip != null) temEquip.QueueFree();
        }
    }

    public PackedScene matchBullet(bulletType btp)
    {
        if (findBullet.TryGetValue(btp, out PackedScene result)) return result;
        return null;
    }

    public PackedScene matchEnemy(enemyTypeEnum type)
    {
        if (findEnemy.TryGetValue(type, out PackedScene scene)) return scene;
        return null;
    }

    public PackedScene matchWeapon(weapons type)
    {
        if (findWeapons.TryGetValue(type, out PackedScene scene)) return scene;
        return null;
    }

    public PackedScene matchZhuangBei(zhuangBei zi)
    {
        if (findZhuangBei.TryGetValue(zi, out PackedScene result)) return result;
        return null;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}