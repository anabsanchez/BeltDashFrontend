using Godot;
using System;

public partial class Game : Control
{
	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
        GD.Print("Game scene loaded");

        var parallax = GetNode<SpaceParallax>("/root/Main/Parallax2D");

        GD.Print(parallax);
		GD.Print("Parallax node type: ", parallax.GetType());
        // Paso 1: detener fondo (frenada suave)
        parallax.StopForTransition();

        // Paso 2: esperar 0.5 segundos
        await ToSignal(GetTree().CreateTimer(0.5f), "timeout");

        // Paso 3: iniciar desplazamiento con aceleración inversa
        parallax.StartGame();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
