using Godot;
using System;
using System.Collections.Generic;

public partial class Admin : Control
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
	private List<Button> editButtons = new();

	private Godot.Collections.Array currentScores;

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

		roleIcon.SetFrame(1); // Siempre será admin aquí

		userButton.Pressed += OnUserPressed;
		backLinkButton.Pressed += OnBackPressed;
		nextButton.Pressed += OnNextPressed;
		previousButton.Pressed += OnPreviousPressed;

		// Asignar los labels y botones
		for (int i = 1; i <= 5; i++)
		{
			positionLabels.Add(GetNode<Label>($"CenterContainer/MarginContainer/VBoxContainer/GridContainer/PositionLabel{i}"));
			usernameLabels.Add(GetNode<Label>($"CenterContainer/MarginContainer/VBoxContainer/GridContainer/UsernameLabel{i}"));
			scoreLabels.Add(GetNode<Label>($"CenterContainer/MarginContainer/VBoxContainer/GridContainer/ScoreLabel{i}"));
			var editButton = GetNode<Button>($"CenterContainer/MarginContainer/VBoxContainer/GridContainer/EditLinkButton{i}");
			editButtons.Add(editButton);
			int index = i - 1;
			editButton.Pressed += () => OnEditButtonPressed(index);
		}

		LoadRankingPage(currentPage);
	}

	private void OnUserPressed()
	{
		userPanel.Visible = !userPanel.Visible;
	}

	private void OnBackPressed()
	{
		MainMenu menu = GD.Load<PackedScene>("res://scenes/UI/MainMenu.tscn").Instantiate() as MainMenu;
		main.LoadScreen(menu);
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
			GD.PrintErr("Error al obtener el ranking para el panel admin");
			return;
		}

		var json = new Json();
		if (json.Parse(body.GetStringFromUtf8()) != Error.Ok)
		{
			GD.PrintErr("Error al parsear JSON");
			return;
		}

		var response = (Godot.Collections.Dictionary)json.Data;
		if (!response.ContainsKey("data"))
		{
			GD.PrintErr("Respuesta inválida");
			return;
		}

		var data = (Godot.Collections.Dictionary)response["data"];
		currentScores = (Godot.Collections.Array)data["scores"];
		totalPages = (int)data["totalPages"];
		bool hasPrevious = (bool)data["hasPrevious"];
		bool hasNext = (bool)data["hasNext"];

		for (int i = 0; i < 5; i++)
		{
			if (i < currentScores.Count)
			{
				var entry = (Godot.Collections.Dictionary)currentScores[i];
				string username = (string)entry["username"];
				int points = (int)entry["points"];
				int position = (currentPage - 1) * 5 + i + 1;

				positionLabels[i].Text = position.ToString();
				usernameLabels[i].Text = username;
				scoreLabels[i].Text = points.ToString();
				editButtons[i].Visible = true;
			}
			else
			{
				positionLabels[i].Text = "";
				usernameLabels[i].Text = "";
				scoreLabels[i].Text = "";
				editButtons[i].Visible = false;
			}
		}

		previousButton.Visible = hasPrevious;
		nextButton.Visible = hasNext;
	}

	private void OnEditButtonPressed(int index)
	{
		if (index < currentScores.Count)
		{
			var entry = (Godot.Collections.Dictionary)currentScores[index];
			int userId = (int)entry["userId"];

			GD.Print(userId);

			main.userToEditId = userId;

			var userEdition = GD.Load<PackedScene>("res://scenes/UI/UserEdition.tscn").Instantiate();
			main.LoadScreen(userEdition);
		}
	}
}
