using Godot;
using System;

public partial class Login : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        var signInButton = GetNode<Button>("CenterContainer/MarginContainer/VBoxContainer/ButtonsContainer/SignInButton");
        var registerLinkButton = GetNode<Button>("CenterContainer/MarginContainer/VBoxContainer/ButtonsContainer/RegisterLinkButton");
		
		signInButton.Pressed += OnSignInPressed;
		registerLinkButton.Pressed += OnRegisterLinkPressed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    private void OnSignInPressed()
    {
        var main = GetNode<Main>("/root/Main");
        main.LoadScreen("scenes/UI/MainMenu.tscn");
    }

    private void OnRegisterLinkPressed()
    {
        var main = GetNode<Main>("/root/Main");
        main.LoadScreen("scenes/UI/Register.tscn");
    }
}
