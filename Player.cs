using Godot;
using System;

public partial class Player : Area2D
{
	[Export]
	public int speed{get; set;} = 400;
	

	public Vector2 ScreenSize;
	
		// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
			ScreenSize = GetViewportRect().Size;
			
			Position = new Vector2( 
				x: ScreenSize.X/2,
				y: ScreenSize.Y/2);
			GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, false);
			Show();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Show();
		var Velocity = Vector2.Zero;
		
		if ( Input.IsActionPressed("move_right"))
			Velocity.X++;
		if ( Input.IsActionPressed("move_left"))
			Velocity.X--;
		if ( Input.IsActionPressed("move_down"))
			Velocity.Y++;
		if ( Input.IsActionPressed("move_up"))
			Velocity.Y--;
			
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		
		if(Velocity.Length() > 0)
		{
			Velocity = Velocity.Normalized() * speed;
			animatedSprite2D.Play();
		}
		else
		{
			animatedSprite2D.Stop();
		}
		
		Position += Velocity *  (float)delta;
		Position = new Vector2(
			x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
			y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
		);
		
		
		if ( Velocity.X != 0 )
		{
			if ( Velocity.Y == 0 )
			  animatedSprite2D.Animation = "walk";
			animatedSprite2D.FlipH = Velocity.X < 0;	
		}
		
		if ( Velocity.Y != 0 )
		{
			animatedSprite2D.Animation = "up";
			animatedSprite2D.FlipV = Velocity.Y > 0;
		}
	}
	
	[Signal]
	public delegate void HitEventHandler();
	
	public void OnBodyEntered(Node2D body)
	{
		Hide();
		EmitSignal(SignalName.Hit);
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
	}


}
