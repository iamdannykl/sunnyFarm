using Godot;
using System;
using System.Collections.Generic;
using SunnyFarm.code;

public partial class Weapon : Area2D
{
    [Export] public Texture2D icon;
    [Export] public string discribe;
    [Export] public weapons weaponType;
    public string Name { get; set; }
    public List<string> Tags { get; set; }
    public Rarity Rarity { get; set; }

    public Weapon(string name, List<string> tags, Rarity rarity)
    {
        Name = name;
        Tags = tags;
        Rarity = rarity;
    }
}