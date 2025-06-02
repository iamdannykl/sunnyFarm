using System;

namespace SunnyFarm.code.Equipments;
[AttributeUsage(AttributeTargets.Class)]
public class EquipmentAttribute : Attribute
{
    public string Id { get; }

    public EquipmentAttribute(string id)
    {
        Id = id;
    }
}
