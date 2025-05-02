using Godot;
using System;

public partial class Register : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        var signUpButton = GetNode<Button>("CenterContainer/MarginContainer/VBoxContainer/ButtonsContainer/SignUpButton");
        var loginLinkButton = GetNode<Button>("CenterContainer/MarginContainer/VBoxContainer/ButtonsContainer/LoginLinkButton");
		
		signUpButton.Pressed += OnSignUpPressed;
		loginLinkButton.Pressed += OnLoginLinkPressed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    private void OnSignUpPressed()
    {
        var main = GetNode<Main>("/root/Main");
        main.LoadScreen("res://scenes/UI/MainMenu.tscn");
    }

    private void OnLoginLinkPressed()
    {
        var main = GetNode<Main>("/root/Main");
        main.LoadScreen("res://scenes/UI/Login.tscn");
    }
}
