using System.Collections.Generic;
using Godot;

namespace SunnyFarm.code;

public partial class playerDanWeiHua : CharacterBody2D
{
    [Export] public float timePerGrid;
    public const float JumpVelocity = -400.0f;
    private List<Vector2I> vector2Is = new();
    private AnimationPlayer animationPlayer;
    private bool isRt;
    private bool isMoving;
    private Vector2 direction;
    [Export] public TileMapLayer tileMapLayer;

    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    private async void fromToMoveByDirection()
    {
        if (isMoving || vector2Is.Count == 0) return;
        isMoving = true;
        var tween = CreateTween().SetTrans(Tween.TransitionType.Linear);
        var characterPosition = GlobalPosition;
        var tileMapPoint = tileMapLayer.LocalToMap(characterPosition) + vector2Is[0] /* (Vector2I)directionMove */;
        var tileGlobalPos = tileMapLayer.MapToLocal(tileMapPoint);
        GD.PrintT(tileMapPoint, tileGlobalPos);
        GD.Print(vector2Is.Count);
        tween.TweenProperty(this, "position", tileGlobalPos, timePerGrid);
        if (direction.X < 0)
        {
            animationPlayer.Play("lftWalk");
            isRt = false;
        }
        else if (direction.X > 0)
        {
            animationPlayer.Play("rtWalk");
            isRt = true;
        }
        else
        {
            if (isRt)
                animationPlayer.Play("rtWalk");
            else
                animationPlayer.Play("lftWalk");
        }

        await ToSignal(tween, "finished");
        isMoving = false;
        if (direction == Vector2.Zero)
        {
            if (isRt)
                animationPlayer.Play("rtIdle");
            else
                animationPlayer.Play("lftIdle");
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        GD.Print(Input.IsActionJustReleased("ui_left"));
        if (Input.IsActionJustReleased("ui_left") && vector2Is.Contains(Vector2I.Left))
        {
            vector2Is.Remove(Vector2I.Left);
            GD.Print(123);
        }

        if (Input.IsActionJustReleased("ui_right") && vector2Is.Contains(Vector2I.Right))
            vector2Is.Remove(Vector2I.Right);
        if (Input.IsActionJustReleased("ui_up") && vector2Is.Contains(Vector2I.Up)) vector2Is.Remove(Vector2I.Up);
        if (Input.IsActionJustReleased("ui_down") && vector2Is.Contains(Vector2I.Down)) vector2Is.Remove(Vector2I.Down);

        if (Input.IsActionPressed("ui_left") && !vector2Is.Contains(Vector2I.Left)) vector2Is.Add(Vector2I.Left);
        if (Input.IsActionPressed("ui_right") && !vector2Is.Contains(Vector2I.Right)) vector2Is.Add(Vector2I.Right);
        if (Input.IsActionPressed("ui_up") && !vector2Is.Contains(Vector2I.Up)) vector2Is.Add(Vector2I.Up);
        if (Input.IsActionPressed("ui_down") && !vector2Is.Contains(Vector2I.Down)) vector2Is.Add(Vector2I.Down);
        //remove from the list
        fromToMoveByDirection();
    }
}