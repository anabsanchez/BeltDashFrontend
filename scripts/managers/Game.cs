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
        timer.Timeout += OnTimerTimeout; // Conectar señal correctamente
        timer.Start();

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    private void OnTimerTimeout()
    {
        // Instanciar obstáculo nuevo desde una escena si lo deseas
        // var obstacleScene = GD.Load<PackedScene>("res://scenes/Obstacles/Asteroid.tscn");
        // var newObstacle = (Node2D)obstacleScene.Instantiate();

        // O si estás usando un objeto ya definido en código (menos común):
        // Obstacle newObstacle = new();

        Random rnd = new();
        var newObstacle = new Node2D(); // ⚠️ temporal, deberías reemplazarlo por tu escena real

        newObstacle.GlobalPosition = new Vector2(rnd.Next(300, 1700), -4000);
        AddChild(newObstacle);

        // Establecer nuevo intervalo aleatorio entre 1 y 4 segundos
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
