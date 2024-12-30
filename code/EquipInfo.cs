using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace SunnyFarm.code;

public class EquipInfo
{
    public string discribe;
    public Texture2D icon;
    public weapons weaponType;
    public zhuangBei zhuangBeiType;
    //public string Name { get; set; }
    public int price;
    public Array<Tags> Tags { get; set; }
    public Rarity Rarity { get; set; }
    public bool isProps { get; set; }

    public EquipInfo(string inDiscribe, weapons inType, Array<Tags> inTags, Rarity inRarity, Texture2D inIcon,
        int inPrice, bool inIsProps,zhuangBei inZhuangBeiType)
    {
        discribe = inDiscribe;
        weaponType = inType;
        Console.WriteLine($"weaponType{inType}");
        Tags = inTags;
        Rarity = inRarity;
        icon = inIcon;
        price = inPrice;
        isProps = inIsProps;
        zhuangBeiType = inZhuangBeiType;
    }
}