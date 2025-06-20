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
        string userDir = ProjectSettings.GlobalizePath("user://");
        string folderPath = Path.Combine(userDir, "saveFolder");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            GD.Print("Folder created: " + folderPath);
        }
        else
        {
            GD.Print("Folder already exists: " + folderPath);
        }

        string realPath = Path.Combine(folderPath, "level.yaml");
        if (File.Exists(realPath)) playButton.Visible = true;
        else
            playButton.Visible = false;
    }

    public void playIt()
    {
        Node2D battleField = playScene.Instantiate<Node2D>();
        GetTree().CurrentScene.QueueFree();
        GetTree().Root.AddChild(battleField);
        GetTree().CurrentScene = battleField;
    }

    public void editIt()
    {
        EditUi editUi = editScene.Instantiate<EditUi>();
        GetTree().CurrentScene.QueueFree();
        GetTree().Root.AddChild(editUi);
        GetTree().CurrentScene = editUi;
    }

    public void closeIt()
    {
        GetTree().Quit();
    }
}