using Godot;
using System;
using System.Collections.Generic;

public partial class UserScores : Control
{
	private Main main;
	private TextureButton userButton;
	private Sprite2D roleIcon;
	private Panel userPanel;
	private Button backLinkButton;
	private Button previousButton;
	private Button nextButton;
	private HttpRequest request;

	private int currentPage = 1;
	private int pageSize = 5;
	private List<Godot.Collections.Dictionary> allScores = new();

	private List<Label> dateLabels = new();
	private List<Label> scoreLabels = new();

	public override void _Ready()
	{
		main = GetNode<Main>("/root/Main");
		userButton = GetNode<TextureButton>("UserButton");
		roleIcon = GetNode<Sprite2D>("UserButton/RoleIconSprite");
		userPanel = GetNode<Panel>("UserPanel");
		backLinkButton = GetNode<Button>("UserPanel/VBoxContainer/BackLinkButton");
		previousButton = GetNode<Button>("CenterContainer/MarginContainer/VBoxContainer/HBoxContainer/PreviousButton");
		nextButton = GetNode<Button>("CenterContainer/MarginContainer/VBoxContainer/HBoxContainer/NextButton");
		request = GetNode<HttpRequest>("HTTPRequest");

		request.RequestCompleted += OnRequestCompleted;

		if (main.userRole == "admin")
			roleIcon.SetFrame(1);

		userButton.Pressed += OnUserPressed;
		backLinkButton.Pressed += OnBackPressed;
		nextButton.Pressed += OnNextPressed;
		previousButton.Pressed += OnPreviousPressed;

		for (int i = 1; i <= 5; i++)
		{
			dateLabels.Add(GetNode<Label>($"CenterContainer/MarginContainer/VBoxContainer/GridContainer/DateLabel{i}"));
			scoreLabels.Add(GetNode<Label>($"CenterContainer/MarginContainer/VBoxContainer/GridContainer/ScoreLabel{i}"));
		}

		LoadUserScores();
	}

	private void OnUserPressed()
	{
		userPanel.Visible = !userPanel.Visible;
	}

	private void OnBackPressed()
	{
		var main = GetNode<Main>("/root/Main");
		MainMenu mainMenu = GD.Load<PackedScene>("res://scenes/UI/MainMenu.tscn").Instantiate() as MainMenu;
		main.LoadScreen(mainMenu);
	}

	private void OnNextPressed()
	{
		if ((currentPage * pageSize) < allScores.Count)
		{
			currentPage++;
			DisplayCurrentPage();
		}
	}

	private void OnPreviousPressed()
	{
		if (currentPage > 1)
		{
			currentPage--;
			DisplayCurrentPage();
		}
	}

	private void LoadUserScores()
	{
		string url = $"http://localhost:5064/api/v1/users/{main.userId}/scores";

		request.Request(
			url,
			customHeaders: new string[] {
				"Content-Type: application/json",
				"Authorization: Bearer " + main.userToken
			},
			method: HttpClient.Method.Get
		);
	}

	private void OnRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
	{
		if (responseCode != 200)
		{
			GD.PrintErr("Error al obtener las puntuaciones del usuario.");
			return;
		}

		var json = new Json();
		var parseResult = json.Parse(body.GetStringFromUtf8());

		if (parseResult != Error.Ok)
		{
			GD.PrintErr("Error al parsear el JSON.");
			return;
		}

		var response = (Godot.Collections.Dictionary)json.Data;

		if (!response.ContainsKey("data"))
		{
			GD.PrintErr("Respuesta inválida: no contiene 'data'");
			return;
		}

		var scores = (Godot.Collections.Array)response["data"];
		allScores.Clear();

		foreach (var entry in scores)
		{
			allScores.Add((Godot.Collections.Dictionary)entry);
		}

		currentPage = 1;
		DisplayCurrentPage();
	}

	private void DisplayCurrentPage()
	{
		int startIndex = (currentPage - 1) * pageSize;

		for (int i = 0; i < pageSize; i++)
		{
			if (startIndex + i < allScores.Count)
			{
				var score = allScores[startIndex + i];

				DateTime date = DateTime.Parse((string)score["createdAt"]);
				string dateStr = date.ToString("dd/MM/yyyy HH:mm");
				int points = (int)score["points"];

				dateLabels[i].Text = dateStr;
				scoreLabels[i].Text = points.ToString();
			}
			else
			{
				dateLabels[i].Text = "";
				scoreLabels[i].Text = "";
			}
		}

		previousButton.Visible = currentPage > 1;
		nextButton.Visible = (currentPage * pageSize) < allScores.Count;
	}
}
