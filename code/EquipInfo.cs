using System.Collections.Generic;

namespace SunnyFarm.code;

public class EquipInfo
{
    public string discribe;

    public weapons weaponType;

    //public string Name { get; set; }
    public string[] Tags { get; set; }
    public Rarity Rarity { get; set; }

    public EquipInfo(string inDiscribe, weapons inType, string[] inTags, Rarity inRarity)
    {
        discribe = inDiscribe;
        weaponType = inType;
        Tags = inTags;
        Rarity = inRarity;
    }
}