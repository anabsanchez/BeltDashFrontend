using Godot;
using System;

public partial class UserEdition : Control
{
	private Main main;
	private TextureButton userButton;
	private Panel userPanel;
	private LineEdit usernameInput, emailInput;
	private OptionButton roleDropdown;
	private CheckButton statusCheckbox;
	private Button saveButton, backButton;
	private HttpRequest request;

	private int userId;
	private bool isFetchingUserData = false;

	private string pendingUsername;
	private string pendingEmail;
	private string pendingRole;
	private bool pendingStatus;

	private enum UpdateStep { None, User, Role, Status }
	private UpdateStep step = UpdateStep.None;

	public override void _Ready()
	{
		main = GetNode<Main>("/root/Main");
		userId = main.userToEditId;

		userButton = GetNode<TextureButton>("UserButton");
		userPanel = GetNode<Panel>("UserPanel");
		usernameInput = GetNode<LineEdit>("CenterContainer/MarginContainer/VBoxContainer/FormContainer/UsernameContainer/UsernameInput");
		emailInput = GetNode<LineEdit>("CenterContainer/MarginContainer/VBoxContainer/FormContainer/EmailContainer/EmailInput");
		roleDropdown = GetNode<OptionButton>("CenterContainer/MarginContainer/VBoxContainer/FormContainer/RoleContainer/RoleDropdown");
		statusCheckbox = GetNode<CheckButton>("CenterContainer/MarginContainer/VBoxContainer/FormContainer/StatusContainer/StatusCheckbox");
		saveButton = GetNode<Button>("CenterContainer/MarginContainer/VBoxContainer/SaveButton");
		backButton = GetNode<Button>("UserPanel/VBoxContainer/BackLinkButton");
		request = GetNode<HttpRequest>("HTTPRequest");

		request.RequestCompleted += OnRequestCompleted;

		userButton.Pressed += OnUserPressed;
		saveButton.Pressed += OnSavePressed;
		backButton.Pressed += OnBackPressed;

		LoadUserData();
	}

	private void LoadUserData()
	{
		isFetchingUserData = true;
		string url = $"http://localhost:5064/api/v1/users/{userId}";

		request.Request(
			url,
			new string[] {
				"Content-Type: application/json",
				"Authorization: Bearer " + main.userToken
			},
			HttpClient.Method.Get
		);
	}

	private void OnUserPressed()
	{
		userPanel.Visible = !userPanel.Visible;
	}

	private void OnBackPressed()
	{
		var adminScene = GD.Load<PackedScene>("res://scenes/UI/Admin.tscn").Instantiate();
		main.LoadScreen(adminScene);
	}

	private void OnSavePressed()
	{
		saveButton.Disabled = true;

		pendingUsername = usernameInput.Text;
		pendingEmail = emailInput.Text;
		pendingRole = roleDropdown.GetItemText(roleDropdown.Selected);
		pendingStatus = statusCheckbox.ButtonPressed;

		step = UpdateStep.User;
		UpdateNextField();
	}

	private void UpdateNextField()
	{
		switch (step)
		{
			case UpdateStep.User:
				string urlUser = $"http://localhost:5064/api/v1/users/{userId}";
				string jsonUser = Json.Stringify(new Godot.Collections.Dictionary {
					{ "username", pendingUsername },
					{ "email", pendingEmail }
				});
				request.Request(
					urlUser,
					new string[] {
						"Content-Type: application/json",
						"Authorization: Bearer " + main.userToken
					},
					HttpClient.Method.Put,
					requestData: jsonUser
				);
				break;

			case UpdateStep.Role:
				int roleId = pendingRole == "admin" ? 2 : 1;
				string urlRole = $"http://localhost:5064/api/v1/users/{userId}/role";
				string jsonRole = Json.Stringify(new Godot.Collections.Dictionary {
					{ "roleId", roleId }
				});
				request.Request(
					urlRole,
					new string[] {
						"Content-Type: application/json",
						"Authorization: Bearer " + main.userToken
					},
					HttpClient.Method.Patch,
					requestData: jsonRole
				);
				break;

			case UpdateStep.Status:
				string statusVal = pendingStatus ? "Active" : "Banned" ;
				string urlStatus = $"http://localhost:5064/api/v1/users/{userId}/status";
				string jsonStatus = Json.Stringify(new Godot.Collections.Dictionary {
					{ "status", statusVal }
				});
				request.Request(
					urlStatus,
					new string[] {
						"Content-Type: application/json",
						"Authorization: Bearer " + main.userToken
					},
					HttpClient.Method.Patch,
					requestData: jsonStatus
				);
				break;

			case UpdateStep.None:
				GD.Print("Todos los cambios fueron enviados.");
				saveButton.Disabled = false;
				break;
		}
	}

	private void OnRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
	{
		if (responseCode != 200)
		{
			GD.PrintErr("Error al guardar cambios:", body.GetStringFromUtf8());
			saveButton.Disabled = false;
			step = UpdateStep.None;
			return;
		}

		if (isFetchingUserData)
		{
			isFetchingUserData = false;
			ApplyUserData(body);
			return;
		}

		switch (step)
		{
			case UpdateStep.User:
				step = UpdateStep.Role;
				UpdateNextField();
				break;
			case UpdateStep.Role:
				step = UpdateStep.Status;
				UpdateNextField();
				break;
			case UpdateStep.Status:
				step = UpdateStep.None;
				UpdateNextField();
				break;
		}
	}

	private void ApplyUserData(byte[] body)
	{
		var json = new Json();
		if (json.Parse(body.GetStringFromUtf8()) != Error.Ok)
		{
			GD.PrintErr("Error al parsear datos del usuario");
			return;
		}

		var dict = (Godot.Collections.Dictionary)json.Data;
		var user = (Godot.Collections.Dictionary)dict["data"];

		usernameInput.Text = (string)user["username"];
		emailInput.Text = (string)user["email"];
        roleDropdown.Selected = ((string)user["role"]) == "admin" ? 1 : 0;
		GD.Print("Estado bool : ", user["status"], " --- status int : ", (int)user["status"]);
        statusCheckbox.ButtonPressed = (((string)user["status"]) == "Active");
	}
}
