using Godot;
using System;

public partial class Game : Control
{
    private GameScore scoreLabel;
    private Timer timer;

    public override async void _Ready()
    {
        var parallax = GetNode<SpaceParallax>("/root/Main/Parallax2D");
        GD.Print(parallax);

        parallax.StopForTransition();

        await ToSignal(GetTree().CreateTimer(0.5f), "timeout");

        parallax.StartGame();

        scoreLabel = GetNode<GameScore>("./GameScoreLabel");
        scoreLabel.ResetScore();

        timer = GetNode<Timer>("./Timer");
        timer.Timeout += _on_timer_timeout;

        timer.WaitTime = 1f;
        timer.Start();
    }

    private void _on_timer_timeout()
    {
        Random rnd = new Random();

        Asteroid newAsteroid = GD.Load<PackedScene>("res://scenes/Gameplay/asteroid.tscn").Instantiate() as Asteroid;

        // newAsteroid.GlobalPosition = new Vector2(rnd.Next(300, 1700), -200);

        float minX = -500f;
        float maxX = 500f;

        float randomX = (float)rnd.NextDouble() * (maxX - minX) + minX;
        newAsteroid.GlobalPosition = new Vector2(randomX, -200);

        newAsteroid.Visible = true;

        AddChild(newAsteroid);

        timer.WaitTime = (float)rnd.NextDouble() * 3f;
        timer.Start();
    }

    public void PauseGame()
    {
        foreach (var child in GetChildren())
        {
            if (child is Node node)
            {
                node.ProcessMode = ProcessModeEnum.Disabled;
            }
        }
    }

}
