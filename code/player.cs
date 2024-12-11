using Godot;
public partial class player : CharacterBody2D
{
	public static player Instance;
	[Export] public float Speed;
	[Export] public Label moneyShow;
	public const float JumpVelocity = -400.0f;
	AnimationPlayer animationPlayer;
	bool isRt;
	Vector2 direction;
	public int moneyValue;
	public float atkBuff;
	public int MoneyValue
	{
		get => moneyValue;
		set
		{
			moneyShow.Text = $"Money:{value}";
			moneyValue = value;
		}
	}
	public override void _Ready()
	{
		Instance = this;
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		moneyShow = GetTree().CurrentScene.GetNode<Label>("CanvasLayer/showMoney");
	}

	public override void _PhysicsProcess(double delta)
	{
		direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
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
				{
					animationPlayer.Play("rtWalk");
				}
				else
				{
					animationPlayer.Play("lftWalk");
				}
			}
			Velocity = Speed * direction;
		}
		else
		{
			if (isRt)
			{
				animationPlayer.Play("rtIdle");
			}
			else
			{
				animationPlayer.Play("lftIdle");
			}
			Velocity = Vector2.Zero;
		}
		MoveAndSlide();
	}
}
