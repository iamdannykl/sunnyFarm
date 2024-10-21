using Godot;
using System;

public partial class Bullet : Area2D
{
	float rangeBlt = 1200;
	float speed = 1000;
	float dis = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Vector2 direction = Vector2.Right.Rotated(Rotation);
		Position += direction * speed * (float)delta;
		dis += speed * (float)delta;
		if (dis > rangeBlt)
		{
			QueueFree();
		}
	}
	void attackObject(Area2D area2D)
	{
		Iattackble iattackble = area2D.GetParent<Iattackble>();
		if (iattackble != null)
		{
			iattackble.attacked();
		}
		QueueFree();
	}
}
