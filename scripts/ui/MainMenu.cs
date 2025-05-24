using Godot;
using System;

public partial class MainMenu : Control
{
	private Main main;
	private Button playButton;
	private Button userScoresLinkButton;
	private Button globalRankingLinkButton;
	private Button signOutLinkButton;
	private TextureButton userButton;
	private Sprite2D roleIcon;
	private Panel userPanel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		main = GetNode<Main>("/root/Main");
		userButton = GetNode<TextureButton>("UserButton");
		roleIcon = GetNode<Sprite2D>("UserButton/RoleIconSprite");
		playButton = GetNode<Button>("CenterContainer/MarginContainer/VBoxContainer/ButtonsContainer/PlayButton");
		globalRankingLinkButton = GetNode<Button>("UserPanel/VBoxContainer/GlobalRankingLinkButton2");
		userScoresLinkButton = GetNode<Button>("UserPanel/VBoxContainer/UserScoresLinkButton");
		signOutLinkButton = GetNode<Button>("UserPanel/VBoxContainer/SignOutLinkButton");
		userPanel = GetNode<Panel>("UserPanel");

		if (main.userRole == "admin")
		{
			roleIcon.SetFrame(1);
		}

		playButton.Pressed += OnPlayPressed;
		userButton.Pressed += OnUserPressed;
		globalRankingLinkButton.Pressed += onGlobalRankingLinkPressed;
		userScoresLinkButton.Pressed += onUserScoresLinkPressed;
		signOutLinkButton.Pressed += onSignOutLinkPressed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnPlayPressed()
	{
		main.SetGameMusic();
		Game game = GD.Load<PackedScene>("res://scenes/Gameplay/Game.tscn").Instantiate() as Game;
		main.LoadScreen(game);
	}

	private void OnUserPressed()
	{
		userPanel.SetVisible(userPanel.IsVisible() ? false : true);
	}
	
    private void onGlobalRankingLinkPressed()
    {
        var main = GetNode<Main>("/root/Main");
        Register home = GD.Load<PackedScene>("res://scenes/UI/Login.tscn").Instantiate() as Register;
		main.LoadScreen(home);
    }
    private void onUserScoresLinkPressed()
    {
        var main = GetNode<Main>("/root/Main");
        Register home = GD.Load<PackedScene>("res://scenes/UI/Login.tscn").Instantiate() as Register;
		main.LoadScreen(home);
    }
    private void onSignOutLinkPressed()
    {
        var main = GetNode<Main>("/root/Main");
        Login login = GD.Load<PackedScene>("res://scenes/UI/Login.tscn").Instantiate() as Login;
		main.LoadScreen(login);
    }
}
