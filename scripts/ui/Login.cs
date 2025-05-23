using Godot;
using System;

public partial class Login : Control
{
	private LineEdit emailInput, passwordInput;
	private Button signInButton;
    private Button registerLinkButton;
	private HttpRequest request;
    private AcceptDialog errorDialog;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        emailInput = GetNode<LineEdit>("CenterContainer/MarginContainer/VBoxContainer/FormContainer/EmailContainer/EmailInput");
        passwordInput = GetNode<LineEdit>("CenterContainer/MarginContainer/VBoxContainer/FormContainer/PasswordContainer/PasswordInput");
        signInButton = GetNode<Button>("CenterContainer/MarginContainer/VBoxContainer/ButtonsContainer/SignInButton");
        registerLinkButton = GetNode<Button>("CenterContainer/MarginContainer/VBoxContainer/ButtonsContainer/RegisterLinkButton");
        
        request = GetNode<HttpRequest>("HTTPRequest");
        request.RequestCompleted += OnRequestCompleted;
        
        errorDialog = GetNode<AcceptDialog>("ErrorDialog");

        signInButton.Pressed += OnSignInPressed;
        registerLinkButton.Pressed += OnRegisterLinkPressed;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    private void OnSignInPressed()
    {
        GD.Print("jelouuu");
        string url = "http://localhost:5064/api/v1/auth/login";

		string jsonBody = Json.Stringify(new Godot.Collections.Dictionary
		{
			{ "email", emailInput.Text },
			{ "password", passwordInput.Text }
		});

		request.Request(
			url,
			customHeaders: new string[] { "Content-Type: application/json" },
			method: HttpClient.Method.Post,
			requestData: jsonBody
		);
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

                GD.Print("Login successful - Token: ", token);
                GD.Print("Role: ", role);
                GD.Print("Username: ", username);

                var main = GetNode<Main>("/root/Main");

                main.userToken = token;
                main.userRole = role;
                main.userUsername = username;


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

    private void OnRegisterLinkPressed()
    {
        var main = GetNode<Main>("/root/Main");
        Register home = GD.Load<PackedScene>("res://scenes/UI/Register.tscn").Instantiate() as Register;
		main.LoadScreen(home);
    }
    
    
}
