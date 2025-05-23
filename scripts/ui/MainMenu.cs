using Godot;
using System;

public partial class MainMenu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var playButton = GetNode<Button>("CenterContainer/MarginContainer/VBoxContainer/ButtonsContainer/PlayButton");
		playButton.Pressed += OnPlayPressed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    private void OnPlayPressed()
    {
        var main = GetNode<Main>("/root/Main");
        Game game = GD.Load<PackedScene>("res://scenes/Gameplay/Game.tscn").Instantiate() as Game;
		main.LoadScreen(game);
    }
}
