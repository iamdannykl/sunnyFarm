using Godot;

namespace SunnyFarm.code.Equipments.Sword;

[Equipment("sword_001")]
public class Sword001 : IEquipment
{
    public void Equip(BasicsCore core)
    {
        GD.Print("装备了剑");
    }

    public void Unequip(BasicsCore core)
    {
        GD.Print("卸下了剑");
    }
}