using Godot;

namespace SunnyFarm.code;

public partial class EnemyPanel : Panel
{
    [Export] public OptionButton optionButton;
    [Export] public CheckButton checkButton;
    [Export] public LineEdit num, cirNum;

    [Export] public Texture2D[] texture2D;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        int i = 0;
        foreach (Texture2D pic in texture2D) optionButton.AddIconItem(pic, i.ToString(), i++);
    }
}