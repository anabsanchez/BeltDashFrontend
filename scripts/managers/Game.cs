using Godot;
using System;

public partial class Game : Control
{
    private GameScore scoreLabel;
    private Timer timer;

	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
        var parallax = GetNode<SpaceParallax>("/root/Main/Parallax2D");
        GD.Print(parallax);

        parallax.StopForTransition();

        await ToSignal(GetTree().CreateTimer(0.5f), "timeout");

        parallax.StartGame();

        scoreLabel = GetNode<GameScore>("./GameScoreLabel");
        scoreLabel.ResetScore();

        timer = this.GetNode<Timer>("./Timer");
        timer.Timeout += OnTimerTimeout;
        timer.Start();

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    private void OnTimerTimeout()
    {
        // var obstacleScene = GD.Load<PackedScene>("res://scenes/Obstacles/Asteroid.tscn");
        // var newObstacle = (Node2D)obstacleScene.Instantiate();

        // Obstacle newObstacle = new();
        // Una vez creado el obstáculo, se vuelve a instanciar una de las dos imágenes

        Random rnd = new();
        var newObstacle = new Node2D();
        newObstacle.GlobalPosition = new Vector2(rnd.Next(300, 1700), -4000);
        AddChild(newObstacle);

        timer.WaitTime = rnd.Next(1, 5);
        timer.Start();
        
    }

    public void _on_timer_timeout(){
        //Obstacle newObstacle = new();
        Random rnd = new();
        //newObstacle.GlobalPosition = new Vector2(rnd.Next(300, 1700), -4000);
        //this.AddChild(newObstacle);

        timer.WaitTime = rnd.Next(0,4);
        timer.Start();
        rnd = null;
    }
}
