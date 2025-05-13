using Godot;
using System;

public partial class Asteroid : Node2D
{
	[Export] public float Speed = 200f;  
    [Export] public float RotationSpeed = 50f;  
    private Vector2 velocity = new Vector2(0, 1);  
    private Sprite2D sprite;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sprite = GetNode<Sprite2D>("Sprite2D");
        SetRandomSprite();
        SetRandomRotationSpeed();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        Position += new Vector2(0, Speed * (float)delta);

        sprite.Rotation += Mathf.DegToRad(RotationSpeed * (float)delta);


        if (Position.Y > GetViewportRect().Size.Y + 150) 
        {
            QueueFree();
        }
	}

	private void SetRandomSprite()
    {
        Random rnd = new();
        int frameIndex = rnd.Next(0, 9);
        sprite.Frame = frameIndex;
    }

    private void SetRandomRotationSpeed()
    {
        Random rnd = new();
        RotationSpeed = rnd.Next(-50, 50);
    }
}
