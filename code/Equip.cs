using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Godot.NativeInterop;
using SunnyFarm.code;
using Godot.Collections;

public partial class Equip : Area2D
{
    [Export] public Texture2D icon;
    [Export] public string discribe;

    [Export] public weapons weaponType;

    //public string Name { get; set; }
    //[Export] public string[] MyTagsList { get; set; }

    [Export] public Rarity Rarity { get; set; }

    [Export] public Array<Tags> MyTagsList { get; set; }

    private void ss()
    {
    }
    /*public Equip(string name, List<string> tags, Rarity rarity)
    {
        Name = name;
        Tags = tags;
        Rarity = rarity;
    }*/
}