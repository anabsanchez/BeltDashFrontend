using Godot;
using System;

public partial class Main : Control
{
    private Control screenContainer;
    public string userToken;
    public string userRole;
    public string userUsername;
    private AudioStreamPlayer2D music;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        screenContainer = GetNode<Control>("CanvasLayer/ScreenContainer");

        Login login = GD.Load<PackedScene>("res://scenes/UI/Login.tscn").Instantiate() as Login;
        LoadScreen(login);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void LoadScreen(Node scene)
    {
        foreach (var child in screenContainer.GetChildren())
        {
            if (child is Node node)
                node.QueueFree();
        }

        screenContainer.AddChild(scene);
    }

    public void OverlayScreen(Node overlay)
    {
        screenContainer.AddChild(overlay);
    }

    public void ResetMusic()
    {
        music.SetPitchScale(0.7f);
    }

    public void SetGameMusic()
    {
        music.SetPitchScale(0.7f);
    }
}
