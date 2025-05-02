using Godot;
using System;

public partial class SpaceParallax : Parallax2D
{
	[Export] public float InitialScrollSpeed = -2f;
    [Export] public float MaxGameScrollSpeed = 8f;
    [Export] public float Acceleration = 0.5f;
    [Export] public float Deceleration = 1.5f;

    private float currentSpeed;
    private float targetSpeed;
    private bool isInGame = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        currentSpeed = targetSpeed = InitialScrollSpeed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        if (Mathf.Abs(currentSpeed - targetSpeed) > 0.01f)
        {
            float direction = Math.Sign(targetSpeed - currentSpeed);
            currentSpeed += direction * (float)delta * (isInGame ? Acceleration : Deceleration);
        }
	
		ScrollOffset += new Vector2(0, currentSpeed * (float)delta);
	}

	public void StartGame()
	{
		isInGame = true;
		targetSpeed = MaxGameScrollSpeed;
	}
	
	public void StopForTransition()
	{
		isInGame = false;
		targetSpeed = 0;
	}

	public void GameOver()
	{
		isInGame = false;
		currentSpeed = targetSpeed = 0;
	}

	public void ResetToIdle()
	{
		isInGame = false;
		currentSpeed = targetSpeed = InitialScrollSpeed;
	}
}
