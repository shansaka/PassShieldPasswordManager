using PassShieldPasswordManager.Models;
using Spectre.Console;

namespace PassShieldPasswordManager
{
    public class UserInterface
    {
        private readonly Account _account = new();
        private readonly SecurityQuestion _securityQuestion = new();
        private readonly Credential _credential = new();
        private readonly LoginSession _loginSession = LoginSession.Instance;
        
        public async Task Run()
        {
            try
            {
                if (_loginSession.IsLoggedIn())
                {
                    await MainMenu();
                }
                else
                {
                    await ApplicationMenu();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task ApplicationMenu()
        {
            NewView();

            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .PageSize(10)
                    .AddChoices(new[] {
                        "Login",
                        "Register",
                        "ResetPassword",
                        "Exit"
                    }));

            switch (selection)
            {
                case "Login":
                    await Login();
                    break;
                case "Register":
                    await Register(); 
                    break;
                case "ResetPassword":
                    await ResetPassword(); 
                    break;
                case "Exit":
                    await Exit();
                    break;
            }
        }

        private async Task Register()
        {
            NewView();

            var newUser = new User();
            newUser.Name = AnsiConsole.Ask<string>("Enter [green]Name[/] :");

            var usernameExists = true;
            while (usernameExists)
            {
                newUser.Username = AnsiConsole.Ask<string>("Enter [green]Username[/] :");

                var user = await _account.VerifyUsername(newUser.Username);
                if (user != null)
                {
                    AnsiConsole.WriteLine();
                    AnsiConsole.Markup("[red]Username already exists[/], please try again.");
                    AnsiConsole.WriteLine();
                }
                else
                {
                    usernameExists = false;
                }
                
            }
            
            var passwordMatched = false;
            while (!passwordMatched)
            {
                newUser.Password = AnsiConsole.Prompt(
                    new TextPrompt<string>("Enter [green]Password[/] :")
                        .PromptStyle("red")
                        .Secret()
                );
                var confirmPassword = AnsiConsole.Prompt(
                    new TextPrompt<string>("Enter [green]Password[/] Again :")
                        .PromptStyle("red")
                        .Secret()
                );

                if (newUser.Password == confirmPassword)
                {
                    passwordMatched = true;
                }
                else
                {
                    AnsiConsole.WriteLine();
                    AnsiConsole.Markup("[red]Password does not matched[/], please try again.");
                    AnsiConsole.WriteLine();
                }
            }
            
            var securityQuestionList = await _securityQuestion.GetList();
            
            if (securityQuestionList.Any())
            {
                var question  = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Select [green]Security Question[/] :")
                        .PageSize(10)
                        .AddChoices(securityQuestionList.Select(x => x.Question).ToArray())
                    );
                newUser.SecurityQuestionId = securityQuestionList.FirstOrDefault(x => x.Question == question)!.SecurityQuestionId;
                newUser.SecurityAnswer = AnsiConsole.Ask<string>("Enter [green]Security Answer[/] :");
            }
            
            newUser.DateCreated = DateTime.Now;
            newUser = await _account.Register(newUser);
            _loginSession.Login(newUser);
            
            await MainMenu();
        }

        private async Task Exit()
        {
            NewView();

            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Are you sure you want to exit the application?")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "Yes",
                        "No"
                    }));

            switch (selection)
            {
                case "Yes":
                    Environment.Exit(0);
                    break;
                case "No":
                    await ApplicationMenu();
                    break;
            }
        }

        private async Task Login()
        {
            NewView();

            var username = AnsiConsole.Ask<string>("Enter [green]Username[/] :");
            var password = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter [green]Password[/] :")
                    .PromptStyle("red")
                    .Secret()
                );

            // Checking login here.
            var user = await _account.Login(username, password);
            if (user != null)
            {
                _loginSession.Login(user);
                
                await MainMenu();
            }
            else
            {
                NewView();

                var selection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[red]Entered username or password is incorrect.[/]")
                        .PageSize(10)
                        .AddChoices(new[] {
                            "Try Again",
                            "Reset Password",
                            "Back"
                        }));

                switch (selection)
                {
                    case "Try Again":
                        await Login();
                        break;
                    case "Reset Password":
                        await ResetPassword();
                        break;
                    case "Back":
                        await ApplicationMenu();
                        break;
                }
            }
        }

        private async Task ResetPassword()
        {
            AnsiConsole.Markup("[red]Foo[/] ");
            AnsiConsole.Markup("[#ff0000]Bar[/] ");
            AnsiConsole.Markup("[rgb(255,0,0)]Baz[/] ");
            
            
            var userId = 0;
            var securityQuestionId = 0;
            var securityAnswer = "";
            
            var usernameExists = false;
            while (!usernameExists)
            {
                var username = AnsiConsole.Ask<string>("Enter [green]Username[/] :");

                var user = await _account.VerifyUsername(username);
                if (user == null)
                {
                    AnsiConsole.WriteLine();
                    AnsiConsole.Markup("[red]Username already exists[/], please try again.");
                    AnsiConsole.WriteLine();
                }
                else
                {
                    usernameExists = true;
                    var securityQuestion = await _securityQuestion.GetById(user.SecurityQuestionId);
                    if (securityQuestion != null)
                    {
                        securityQuestionId = securityQuestion.SecurityQuestionId;
                        securityAnswer = AnsiConsole.Ask<string>($"{securityQuestion.Question} ?");
                    }
                    userId = user.UserId;
                }
            }

            var validatedAnswer = await _account.VerifySecurityAnswer(userId, securityQuestionId, securityAnswer);

            if (validatedAnswer)
            {
                AnsiConsole.WriteLine();
                AnsiConsole.Markup("[green]Security answer verified[/], You can reset your password.");
                AnsiConsole.WriteLine();
                
                var passwordMatched = false;
                var newPassword = "";
                while (!passwordMatched)
                {
                    newPassword = AnsiConsole.Prompt(
                        new TextPrompt<string>("Enter [green]New Password[/] :")
                            .PromptStyle("red")
                            .Secret()
                    );
                    var confirmPassword = AnsiConsole.Prompt(
                        new TextPrompt<string>("Enter [green]New Password[/] Again :")
                            .PromptStyle("red")
                            .Secret()
                    );

                    if (newPassword == confirmPassword)
                    {
                        passwordMatched = true;
                    }
                    else
                    {
                        AnsiConsole.WriteLine();
                        AnsiConsole.Markup("[red]Password does not matched[/], please try again.");
                        AnsiConsole.WriteLine();
                    }
                }

                await _account.ResetPassword(userId, newPassword);
                await Login();
            }
            else
            {
                AnsiConsole.WriteLine();
                AnsiConsole.Markup("[red]We can't verify your answer[/]");
                AnsiConsole.WriteLine();
                var selection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Do you want to try again?")
                        .PageSize(10)
                        .AddChoices(new[] {
                            "Yes",
                            "No"
                        }));

                switch (selection)
                {
                    case "Yes":
                        await ResetPassword();
                        break;
                    case "No":
                        await ApplicationMenu();
                        break;
                }
            }


        }

        private async Task MainMenu()
        {
            NewView();
            var choicesList = new List<string>
            {
                "Create New Credential",
                "View Credentials",
                "Search Credential"
            };
            
            if (_loginSession.User is Admin)
            {
                choicesList.Add("View All Passwords");
            }
            
            choicesList.Add( "Logout");
            choicesList.Add("Exit");
            
            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .PageSize(10)
                    .AddChoices(choicesList));

            switch (selection)
            {
                case "Create New Password":
                    await CreateNewCredential();
                    break; 
                case "View Credentials":
                    await ViewCredentials();
                    break;
                case "Search Credentials":
                    await ViewCredentials();
                    break;
                case "Logout":
                    _loginSession.Logout();
                    await ApplicationMenu();
                    break;
                case "Exit":
                    await Exit();
                    break;
            }
        }

        private async Task CreateNewCredential()
        {
            var username = AnsiConsole.Ask<string>("Enter [green]Username[/] :");
            var password = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter [green]New Password[/] :")
                    .PromptStyle("red")
                    .Secret()
            );
            
            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What type of credential you want to create?")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "Game",
                        "Website",
                        "Desktop Application"
                    }));
            
            switch (selection)
            {
                case "Game":
                    var gameCredential = new CredentialGame
                    {
                        User = _loginSession.User,
                        Username = username,
                        Password = password,
                        GameName = AnsiConsole.Ask<string>("Enter [green]Game Name[/] :"),
                        Developer = AnsiConsole.Ask<string>("Enter [green]Game developer[/] :")
                    };
                    await gameCredential.Create();
                    break;
                case "Website":
                    var websiteCredential = new CredentialWebsite
                    {
                        User = _loginSession.User,
                        Username = username,
                        Password = password,
                        WebsiteName = AnsiConsole.Ask<string>("Enter [green]Website name[/] :"),
                        Url = AnsiConsole.Ask<string>("Enter [green]Website Url[/] :")
                    };
                    await websiteCredential.Create();
                    
                    break;
                case "Desktop Application":
                    var desktopAppCredential = new CredentialDesktopApp
                    {
                        User = _loginSession.User,
                        Username = username,
                        Password = password,
                        DesktopAppName = AnsiConsole.Ask<string>("Enter [green]Desktop application name[/] :")
                    };
                    await desktopAppCredential.Create();
                    break;
            }
            
            AnsiConsole.MarkupLine("[green]Credential created successfully[/] Press any key to go back");
            Console.ReadKey();
            await MainMenu();
        }

        private async Task ViewCredentials()
        {
            Console.Clear();
            var credentialsList = await _credential.GetList(_loginSession.User.UserId); 

            var selectedRowIndex = 0;

            while (true)
            {
                // Display the table
                var table = new Table();
                table.AddColumn("Username");
                table.AddColumn("Password");
                table.AddColumn("Type");
                table.AddColumn("Additional Info");
                table.AddColumn("Created Date");
                table.AddColumn("Updated Date");

                for (var i = 0; i < credentialsList.Count; i++)
                {
                    var credential = credentialsList[i];
                    var style = i == selectedRowIndex ? "[bold red]" : "[]";

                    string additionalInfo;
                    string type;
                    switch (credential)
                    {
                        case CredentialGame game:
                            additionalInfo = $"Game: {game.GameName}, Developer: {game.Developer}";
                            type = "Game";
                            break;
                        case CredentialWebsite website:
                            additionalInfo = $"Website: {website.WebsiteName}, URL: {website.Url}";
                            type = "Website";
                            break;
                        case CredentialDesktopApp website:
                            additionalInfo = $"App Name: {website.DesktopAppName}";
                            type = "Desktop App";
                            break;
                        default:
                            additionalInfo = "";
                            type = "";
                            break;
                    }

                    table.AddRow(
                        $"{style}{credential.Username}[/]",
                        $"{style}{credential.Password}[/]",
                        $"{style}{type}[/]",
                        $"{style}{additionalInfo}[/]",
                        $"{style}{credential.CreatedDate}[/]",
                        $"{style}{credential.UpdatedDate}[/]"
                    );
                }

                AnsiConsole.Render(table);

                AnsiConsole.MarkupLine("Please use up and down arrow keys to select data row");
                AnsiConsole.MarkupLine("Press [bold green]'e'[/] to [bold green]edit[/]");
                AnsiConsole.MarkupLine("Press [bold green]'d'[/] to [bold green]delete[/]");
                AnsiConsole.MarkupLine("Press [bold green]'s'[/] to [bold green]sort[/]");
                AnsiConsole.MarkupLine("Press [bold green]'b'[/] to [bold green]back[/]");
                
                
                var key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedRowIndex = Math.Max(0, selectedRowIndex - 1);
                        break;
                    case ConsoleKey.DownArrow:
                        selectedRowIndex = Math.Min(credentialsList.Count - 1, selectedRowIndex + 1);
                        break;
                    case ConsoleKey.E:
                        // Implement your edit logic here using credentialsList[selectedRowIndex]
                        break;
                    case ConsoleKey.D:
                        // Implement your delete logic here using credentialsList[selectedRowIndex]
                        break;
                    case ConsoleKey.S:
                        // Implement your delete logic here using credentialsList[selectedRowIndex]
                        break;
                    case ConsoleKey.B:
                        await MainMenu();
                        break;
                    default:
                        break;
                }

                Console.Clear(); // Clear console for the next iteration
            }
        }
        
        private static List<Credentials> GetDummyCredentialsList()
        {
            return new List<Credentials>
            {
                new Credentials { CredentialId = 1, Username = "user1", Password = "pass1", Type = 1, Name = "John", UrlOrDeveloper = "http://example.com", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                new Credentials { CredentialId = 2, Username = "user2", Password = "pass2", Type = 2, Name = "Jane", UrlOrDeveloper = "http://example.org", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                // Add more data as needed
            };
        }
        
        private void NewView()
        {
            Console.Clear();
            AnsiConsole.Write(
                new FigletText("Pass Shield")
                    .Color(Color.Green)
                    .Centered());

            if (_loginSession.IsLoggedIn())
            {
                AnsiConsole.Write(new Rule());
                var loggedInUser = new Rule($"[yellow]Welcome {_loginSession.User.Name}[/]");
                loggedInUser.Border = BoxBorder.None;
                loggedInUser.Centered();
                AnsiConsole.Write(loggedInUser);
            }
          
            AnsiConsole.Write(new Rule());
            var rule = new Rule("Please use up and down arrow keys to cycle through menu options");
            rule.Border = BoxBorder.Ascii;
            rule.Centered();
            AnsiConsole.Write(rule);
            AnsiConsole.Write(new Rule());
            AnsiConsole.WriteLine("");
        }
    }
}
