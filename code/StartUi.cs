using Godot;
using System;
using SunnyFarm.code;

public partial class StartUi : Control
{
    private PackedScene playScene;
    private PackedScene editScene;

    public override void _Ready()
    {
        playScene = GD.Load<PackedScene>("res://scene/main.tscn");
        editScene = GD.Load<PackedScene>("res://scene/editUI.tscn");
    }

    public void playIt()
    {
        var battleField = playScene.Instantiate<Node2D>();
        GetTree().CurrentScene.QueueFree();
        GetTree().Root.AddChild(battleField);
        GetTree().CurrentScene = battleField;
    }

    public void editIt()
    {
        var editUi = editScene.Instantiate<EditUi>();
        GetTree().CurrentScene.QueueFree();
        GetTree().Root.AddChild(editUi);
        GetTree().CurrentScene = editUi;
    }

    public void closeIt()
    {
        GetTree().Quit();
    }
}