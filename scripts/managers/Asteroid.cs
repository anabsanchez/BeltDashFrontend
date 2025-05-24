using Godot;
using System;
using System.Threading;
using System.Threading.Tasks;

public partial class Asteroid : Area2D
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
        GlobalPosition += new Vector2(0, Speed * (float)delta);

        sprite.Rotation += Mathf.DegToRad(RotationSpeed * (float)delta);


        if (GlobalPosition.Y > this.GetParent().GetNode<AnimatedSprite2D>("./Spaceship/AnimatedSprite2D").GlobalPosition.Y + 450)
        {
            this.GetParent().RemoveChild(this);
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

    private void _on_body_entered(Node2D body)
    {
        if (body is Spaceship)
        {
            GD.Print("Choque");

            var parallax = GetNode<SpaceParallax>("/root/Main/Parallax2D");
            parallax.GameOver();

            var main = GetNode<Main>("/root/Main");

            var gameScene = GetParent() as Game;
            gameScene.PauseGame();

            var gameScore = GetParent().GetNode<GameScore>("./GameScoreLabel");

            GameOver gameOverScene = GD.Load<PackedScene>("res://scenes/UI/GameOver.tscn").Instantiate() as GameOver;
            gameOverScene.score = gameScore.score;

            //main.LoadScreen(gameOverScene);
            main.OverlayScreen(gameOverScene);
        }
    }
}
