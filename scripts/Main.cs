using Godot;
using System;

public partial class Main : Control
{
    private Control screenContainer;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        screenContainer = GetNode<Control>("CanvasLayer/ScreenContainer");
        
        LoadScreen("res://scenes/UI/Login.tscn");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void LoadScreen(string scenePath)
    {
		// old scene to trash
        foreach (var child in screenContainer.GetChildren())
        {
            if (child is Node node)
                node.QueueFree();
        }

		// new scene on
        var scene = GD.Load<PackedScene>(scenePath);
        var instance = scene.Instantiate<Control>();
        screenContainer.AddChild(instance);
    }
}
