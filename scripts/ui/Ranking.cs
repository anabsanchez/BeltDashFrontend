using Godot;
using System;
using System.Collections.Generic;

public partial class Ranking : Control
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
	private int totalPages = 1;

	private List<Label> positionLabels = new();
	private List<Label> usernameLabels = new();
	private List<Label> scoreLabels = new();

	// Called when the node enters the scene tree for the first time.
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
		{
			roleIcon.SetFrame(1);
		}

		userButton.Pressed += OnUserPressed;
		backLinkButton.Pressed += OnBackPressed;
		nextButton.Pressed += OnNextPressed;
		previousButton.Pressed += OnPreviousPressed;

		for (int i = 1; i <= 5; i++)
		{
			positionLabels.Add(GetNode<Label>($"CenterContainer/MarginContainer/VBoxContainer/GridContainer/PositionLabel{i}"));
			usernameLabels.Add(GetNode<Label>($"CenterContainer/MarginContainer/VBoxContainer/GridContainer/UsernameLabel{i}"));
			scoreLabels.Add(GetNode<Label>($"CenterContainer/MarginContainer/VBoxContainer/GridContainer/ScoreLabel{i}"));
		}

		LoadRankingPage(currentPage);
		previousButton.SetVisible(false);
		nextButton.SetVisible(false);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnUserPressed()
	{
		userPanel.SetVisible(userPanel.IsVisible() ? false : true);
	}

	private void OnBackPressed()
	{
		var main = GetNode<Main>("/root/Main");
		MainMenu mainMenu = GD.Load<PackedScene>("res://scenes/UI/MainMenu.tscn").Instantiate() as MainMenu;
		main.LoadScreen(mainMenu);
	}
	
	private void OnNextPressed()
	{
		if (currentPage < totalPages)
		{
			currentPage++;
			LoadRankingPage(currentPage);
		}
	}

	private void OnPreviousPressed()
	{
		if (currentPage > 1)
		{
			currentPage--;
			LoadRankingPage(currentPage);
		}
	}

	private void LoadRankingPage(int page)
	{
		string url = $"http://localhost:5064/api/v1/scores?pageNumber={page}&pageSize=5&sortBy=points&ascending=false";

		request.Request(
			url,
			customHeaders: new string[] { "Content-Type: application/json" },
			method: HttpClient.Method.Get
		);
	}

	private void OnRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
	{
		if (responseCode != 200)
		{
			GD.PrintErr("Error al obtener el ranking");
			return;
		}

		var json = new Json();
		var parseResult = json.Parse(body.GetStringFromUtf8());

		if (parseResult != Error.Ok)
		{
			GD.PrintErr("Error al parsear la respuesta JSON");
			return;
		}

		var response = (Godot.Collections.Dictionary)json.Data;

		if (!response.ContainsKey("data"))
		{
			GD.PrintErr("Respuesta inválida: no contiene 'data'");
			return;
		}

		var data = (Godot.Collections.Dictionary)response["data"];
		var scores = (Godot.Collections.Array)data["scores"];
		totalPages = (int)data["totalPages"];
		int totalCount = (int)data["totalCount"];
		bool hasPrevious = (bool)data["hasPrevious"];
		bool hasNext = (bool)data["hasPrevious"];

		for (int i = 0; i < 5; i++)
		{
			if (i < scores.Count)
			{
				var entry = (Godot.Collections.Dictionary)scores[i];
				string username = (string)entry["username"];
				int points = (int)entry["points"];
				int rankingPos = (currentPage - 1) * 5 + i + 1;

				positionLabels[i].Text = $"{rankingPos}";
				usernameLabels[i].Text = username;
				scoreLabels[i].Text = points.ToString();
			}
			else
			{
				positionLabels[i].Text = "";
				usernameLabels[i].Text = "";
				scoreLabels[i].Text = "";
			}
		}

		previousButton.Visible = hasPrevious;
		nextButton.Visible = hasNext;
	}
}
