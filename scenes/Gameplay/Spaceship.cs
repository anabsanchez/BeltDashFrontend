using Godot;
using System;

public partial class Spaceship : AnimatedSprite2D
{
    [Export] public float Speed = 300f;
    private Vector2 velocity = Vector2.Zero;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Play("forward"); 
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

        // Normalizar para evitar movimientos más rápidos en diagonal
        velocity = velocity.Normalized();

        // Aplicar el movimiento horizontal (sin movimiento vertical)
        Position += velocity * Speed * (float)delta;
    }

    private void UpdateAnimation()
    {
        if (velocity.X > 0)
        {
            FlipH = false;
            Play("side");
        }
        else if (velocity.X < 0)
        {
            FlipH = true;
            Play("side");
        }
        else
        {
            Play("forward");
        }
    }
}
