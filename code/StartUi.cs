using Godot;
using System;
using System.IO;
using SunnyFarm.code;

public partial class StartUi : Control
{
    private PackedScene playScene;
    private PackedScene editScene;
    [Export] private Button playButton;

    public override void _Ready()
    {
        playScene = GD.Load<PackedScene>("res://scene/main.tscn");
        editScene = GD.Load<PackedScene>("res://scene/editUI.tscn");
        var userDir = ProjectSettings.GlobalizePath("user://");
        var folderPath = Path.Combine(userDir, "saveFolder");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            GD.Print("Folder created: " + folderPath);
        }
        else
        {
            GD.Print("Folder already exists: " + folderPath);
        }

        var realPath = Path.Combine(folderPath, "level.yaml");
        if (File.Exists(realPath)) playButton.Visible = true;
        else
            playButton.Visible = false;
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