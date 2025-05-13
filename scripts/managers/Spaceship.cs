using Godot;
using System;

public partial class Spaceship : Node2D
{
    [Export] public float Speed = 300f;
    private Vector2 velocity = Vector2.Zero;
    private AnimatedSprite2D sprite;
    private CollisionShape2D collisionShape;
    [Export] public float LeftLimit = -500f;
    [Export] public float RightLimit = 500f; 

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{        
        sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
		
        sprite.Play("forward"); 
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        HandleMovement(delta);
        UpdateAnimation();
	}

	private void HandleMovement(double delta)
    {
        velocity = Vector2.Zero;

        if (Input.IsActionPressed("move_right"))
        {
            velocity.X += 1;
        }

        if (Input.IsActionPressed("move_left"))
        {
            velocity.X -= 1;
        }

        velocity = velocity.Normalized();

        float newX = Position.X + velocity.X * Speed * (float)delta;
        newX = Mathf.Clamp(newX, LeftLimit, RightLimit);
        Position = new Vector2(newX, Position.Y);
    }

    private void UpdateAnimation()
    {
        if (velocity.X > 0)
        {
            sprite.FlipH = false;
            sprite.Play("side");
        }
        else if (velocity.X < 0)
        {
            sprite.FlipH = true;
            sprite.Play("side");
        }
        else
        {
            sprite.Play("forward");
        }    
    }

    private void OnCollision()
    {
        GD.Print("Boom!");
    }
}
