using Godot;
using System;

public partial class Register : Control
{
    
	private HttpRequest request;
    private AcceptDialog errorDialog;
    private LineEdit usernameInput;
    private LineEdit emailInput;
    private LineEdit passwordInput;
    private LineEdit confirmPasswordInput;

	// Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var signUpButton = GetNode<Button>("CenterContainer/MarginContainer/VBoxContainer/ButtonsContainer/SignUpButton");
        var loginLinkButton = GetNode<Button>("CenterContainer/MarginContainer/VBoxContainer/ButtonsContainer/LoginLinkButton");
        usernameInput = GetNode<LineEdit>("CenterContainer/MarginContainer/VBoxContainer/FormContainer/UsernameContainer/UsernameInput");
        emailInput = GetNode<LineEdit>("CenterContainer/MarginContainer/VBoxContainer/FormContainer/EmailContainer/EmailInput");
        passwordInput = GetNode<LineEdit>("CenterContainer/MarginContainer/VBoxContainer/FormContainer/PasswordContainer/PasswordInput");
        confirmPasswordInput = GetNode<LineEdit>("CenterContainer/MarginContainer/VBoxContainer/FormContainer/PasswordContainer/PasswordInput");

        request = GetNode<HttpRequest>("HTTPRequest");
        request.RequestCompleted += OnRequestCompleted;

        errorDialog = GetNode<AcceptDialog>("ErrorDialog");

        signUpButton.Pressed += OnSignUpPressed;
        loginLinkButton.Pressed += OnLoginLinkPressed;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    private void OnSignUpPressed()
    {
GD.Print("jelouuu");
        string url = "http://localhost:5064/api/v1/auth/register";

		string jsonBody = Json.Stringify(new Godot.Collections.Dictionary
		{
			{ "username", usernameInput.Text },
			{ "email", emailInput.Text },
			{ "password", passwordInput.Text },
			{ "confirmPassword", confirmPasswordInput.Text },
		});

		request.Request(
			url,
			customHeaders: new string[] { "Content-Type: application/json" },
			method: HttpClient.Method.Post,
			requestData: jsonBody
		);
        //var main = GetNode<Main>("/root/Main");
        //main.LoadScreen("scenes/UI/MainMenu.tscn");
    }

    private void OnRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
{
    GD.Print("Respuesta recibida - Código:", responseCode);
    GD.Print("Cuerpo:", body.GetStringFromUtf8());

    var json = new Json();

    var parseResult = json.Parse(body.GetStringFromUtf8());
    GD.Print("Parse result:", parseResult); 

    if (parseResult == Error.Ok)
    {
    var response = (Godot.Collections.Dictionary)json.Data;

            if (response.ContainsKey("data"))
            {
                var data = (Godot.Collections.Dictionary)response["data"];

                string token = (string)data["token"];
                string role = (string)data["role"];
                string username = (string)data["username"];
                int userId = (int)data["userId"];

                GD.Print("Register successful - Token: ", token);
                GD.Print("Role: ", role);
                GD.Print("USERNAME: ", username);
                GD.Print("UserId: ", userId);

                var main = GetNode<Main>("/root/Main");

                main.userToken = token;
                main.userRole = role;
                main.userUsername = username;
                main.userId = userId;
                
                MainMenu home = GD.Load<PackedScene>("res://scenes/UI/MainMenu.tscn").Instantiate() as MainMenu;
		        main.LoadScreen(home);

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

    private void OnLoginLinkPressed()
    {
        var main = GetNode<Main>("/root/Main");
        Login login = GD.Load<PackedScene>("res://scenes/UI/Login.tscn").Instantiate() as Login;
		main.LoadScreen(login);
    }
}
