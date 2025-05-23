using Godot;
using System;
using System.Threading.Tasks;

public partial class GameOver : Control
{
	private HttpRequest request;
	public int score;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var retryButton = GetNode<Button>("CenterContainer/MarginContainer/VBoxContainer/ButtonsContainer/RetryButton");
		var homeButton = GetNode<Button>("CenterContainer/MarginContainer/VBoxContainer/ButtonsContainer/HomeButton");

		request = this.GetNode<HttpRequest>("./HTTPRequest");

		retryButton.Pressed += OnRetryPressed;
		homeButton.Pressed += OnHomePressed;

		SaveScore(score);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnRetryPressed()
	{
		var main = GetNode<Main>("/root/Main");
		Game game = GD.Load<PackedScene>("res://scenes/Gameplay/Game.tscn").Instantiate() as Game;
		main.LoadScreen(game);
	}

	private void OnHomePressed()
	{
		var main = GetNode<Main>("/root/Main");
		MainMenu home = GD.Load<PackedScene>("res://scenes/UI/MainMenu.tscn").Instantiate() as MainMenu;
		main.LoadScreen(home);
	}

	private void SaveScore(int newScore)
	{
		GD.Print("Entro saveScore");
		string url = "http://localhost:5064/api/v1/scores";

		string jsonBody = Json.Stringify(new Godot.Collections.Dictionary
		{
			{ "points", newScore },
		});

		var main = GetNode<Main>("/root/Main");
		GD.Print("Token: ", main.userToken);

		request.Request(
		 url,
			customHeaders: new string[] { "Content-Type: application/json",
										  "Authorization: Bearer " + main.userToken },
			method: HttpClient.Method.Post,
			requestData: jsonBody
		);
		GD.Print("Salgo de saveScore");
	}

	private void _on_http_request_request_completed(long result, long response_code, string[] headers, byte[] body)
	{
		GD.Print("Respuesta recibida - Código:", response_code);
		GD.Print("Cuerpo:", body.GetStringFromUtf8());

		var json = new Json();

		var parseResult = json.Parse(body.GetStringFromUtf8());
		GD.Print("Parse result:", parseResult);

		if (parseResult == Error.Ok)
		{
			var response = (Godot.Collections.Dictionary)json.Data;

			if (response.ContainsKey("data"))
			{
				GD.Print("Score saved");
				return;
			}
			else
			{
				GD.PrintErr("La respuesta no contiene el campo 'data'");
			}
		}

		else
		{
			GD.PrintErr("Error al parsear JSON");
		}
	}
}
