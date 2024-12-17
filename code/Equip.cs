using Godot;
using System;
using System.Collections.Generic;
using SunnyFarm.code;

public class Equip
{
    [Export] public Texture2D icon;
    [Export] public string discribe;
    [Export] public weapons weaponType;
    public string Name { get; set; }
    public List<string> Tags { get; set; }
    public Rarity Rarity { get; set; }

    public Equip(string name, List<string> tags, Rarity rarity)
    {
        Name = name;
        Tags = tags;
        Rarity = rarity;
    }
}