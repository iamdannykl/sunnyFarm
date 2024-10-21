using Godot;
using System;

public partial class BlueBird : enemyBase
{
    public override void attacked()
    {
        Hp -= 1;
    }
}
