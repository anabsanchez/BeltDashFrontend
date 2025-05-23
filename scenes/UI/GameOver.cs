using Godot;
using System;

public partial class GameOver : Control
{

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var retryButton = GetNode<Button>("CenterContainer/MarginContainer/VBoxContainer/ButtonsContainer/RetryButton");
		var homeButton = GetNode<Button>("CenterContainer/MarginContainer/VBoxContainer/ButtonsContainer/HomeButton");

		retryButton.Pressed += OnRetryPressed;
		homeButton.Pressed += OnHomePressed;

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
    private void OnRetryPressed()
    {
        var main = GetNode<Main>("/root/Main");
        main.LoadScreen("scenes/Gameplay/Game.tscn");
    }

    private void OnHomePressed()
    {
        var main = GetNode<Main>("/root/Main");
        main.LoadScreen("scenes/UI/MainMenu.tscn");
    }
}
