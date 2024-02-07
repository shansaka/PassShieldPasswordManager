 using System.Globalization;
 using PassShieldPasswordManager.Services;
 using PassShieldPasswordManager.Utilities;
using Spectre.Console;

namespace PassShieldPasswordManager
{
    public class UserInterface
    {
        private readonly Account _account;
        private readonly SecurityQuestion _securityQuestion;
        private readonly LoginSession _loginSession;

        public UserInterface()
        {
            _securityQuestion = new SecurityQuestion();
            _loginSession = LoginSession.Instance;
            _account = new Account();
            InitializeAsync().Wait();
        }

        private async Task InitializeAsync()
        {
            if(_loginSession.IsLoggedIn()){
                var username = _loginSession.LoggedInUsername;
                var user = await _account.VerifyUsername(username);
                _loginSession.Login(user);
            }
        }
        
        public async Task Run()
        {
            try
            {
                if (_loginSession.IsLoggedIn())
                {
                    NewView();
                    
                    var passwordMatched = false;
                    var count = 0;
                    while (!passwordMatched)
                    {
                        var password = AnsiConsole.Prompt(
                            new TextPrompt<string>("Enter [green]Password[/] :")
                                .PromptStyle("red")
                                .Secret()
                        );

                        if (new Encryption(password).CreateSha512() == _loginSession.User.Password)
                        {
                            passwordMatched = true;
                            await MainMenu();
                        }
                        else
                        {
                            AnsiConsole.WriteLine();
                            AnsiConsole.Markup("[red]Password is incorrect[/], please try again.");
                            AnsiConsole.WriteLine();
                        }

                        count++;
                        if (count == 3)
                        {
                            AnsiConsole.MarkupLine("[red]You have entered wrong usernames more than 3 times[/] We're logging you off, try log in again.");
                            Console.ReadKey();
                            _loginSession.Logout();
                            await ApplicationMenu();
                            return;
                        }
                    }
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

            var newUser = new User
            {
                Name = AnsiConsole.Ask<string>("Enter [green]Name[/] :")
            };

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
            
            var userId = 0;
            var securityQuestionId = 0;
            var securityAnswer = "";
            
            var usernameExists = false;
            var count = 0;
            while (!usernameExists)
            {
                var username = AnsiConsole.Ask<string>("Enter [green]Username[/] :");

                var user = await _account.VerifyUsername(username);
                if (user == null)
                {
                    AnsiConsole.WriteLine();
                    AnsiConsole.Markup("[red]Username not found[/], please try again.");
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

                count++;
                if (count == 3)
                {
                    AnsiConsole.MarkupLine("[red]You have entered wrong usernames more than 3 times[/] Press any key to go back");
                    Console.ReadKey();
                    await ApplicationMenu();
                    return;
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
                "Search Credentials"
            };
            
            if (_loginSession.User is Admin)
            {
                choicesList.Add("Admin Management");
            }
            
            choicesList.Add( "Logout");
            choicesList.Add("Exit");
            
            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .PageSize(10)
                    .AddChoices(choicesList));

            switch (selection)
            {
                case "Create New Credential":
                    await CreateNewCredential();
                    break; 
                case "View Credentials":
                    await ViewCredentials();
                    break;
                case "Search Credentials":
                    await SearchCredential();
                    break;
                case "Admin Management":
                    await AdminMenu();
                    break;
                case "Logout":
                    _account.Logout();
                    await ApplicationMenu();
                    break;
                case "Exit":
                    await Exit();
                    break;
            }
        }

        private async Task AdminMenu()
        {
            NewView();

            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .PageSize(10)
                    .AddChoices(new[] {
                        "View All Credentials",
                        "View Users",
                        "Back"
                    }));

            switch (selection)
            {
                case "View All Credentials":
                    await ViewAllCredentials();
                    break;
                case "View Users":
                    await ViewUsers(); 
                    break;
                case "Back":
                    await MainMenu();
                    break;
            }
        }

        private async Task ViewUsers()
        {
            Console.Clear();
            if (_loginSession.User is Admin admin)
            {
                var users = await admin.ViewUsers();
                users.RemoveAll(x => x.UserId == _loginSession.User.UserId);
                
                if (!users.Any())
                {
                    AnsiConsole.MarkupLine("[green]Can't find any users[/] Press any key to go back");
                    Console.ReadKey();
                    await AdminMenu();
                }
                
                var selectedRowIndex = 0;

                while (true)
                {
                    // Display the table
                    var table = new Table();
                    table.AddColumn("Name");
                    table.AddColumn("Username");
                    table.AddColumn("Is Admin");
                    table.AddColumn("Created Date");
                   

                    for (var i = 0; i < users.Count; i++)
                    {
                        var user = users[i];
                        var style = i == selectedRowIndex ? "[bold red]" : "[]";
                        var isAdmin = "No";

                        if (user is Admin)
                        {
                            isAdmin = "Yes";
                        }

                        string createdDateText = (user.DateCreated == DateTime.MinValue) ? "" : user.DateCreated.ToString(CultureInfo.InvariantCulture);

                        table.AddRow(
                            $"{style}{user.Name.EscapeMarkup()}[/]",
                            $"{style}{user.Username.EscapeMarkup()}[/]",
                            $"{style}{isAdmin.EscapeMarkup()}[/]",
                            $"{style}{createdDateText.EscapeMarkup()}[/]"
                        );
                        
                    }

                    AnsiConsole.Render(table);

                    AnsiConsole.MarkupLine("Please use up and down arrow keys to select data row");
                    AnsiConsole.MarkupLine("Press [bold green]'a'[/] to [bold green] make user an admin[/]");
                    AnsiConsole.MarkupLine("Press [bold green]'d'[/] to [bold green]delete[/]");
                    AnsiConsole.MarkupLine("Press [bold green]'b'[/] to [bold green]back[/]");
                    
                    
                    var key = Console.ReadKey().Key;

                    var selectedUser = users[selectedRowIndex];
                    switch (key)
                    {
                        case ConsoleKey.UpArrow:
                            selectedRowIndex = Math.Max(0, selectedRowIndex - 1);
                            break;
                        case ConsoleKey.DownArrow:
                            selectedRowIndex = Math.Min(users.Count - 1, selectedRowIndex + 1);
                            break;
                        case ConsoleKey.A:
                            await MakeUserAdmin(selectedUser);
                            break;
                        case ConsoleKey.D:
                            await DeleteUser(selectedUser);
                            break;
                        case ConsoleKey.B:
                            await AdminMenu();
                            break;
                    }

                    Console.Clear(); 
                }
            }
        }

        private async Task DeleteUser(User selectedUser)
        {
            NewView();

            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"Are you sure you want to delete user ({selectedUser.Name.EscapeMarkup()}) ?")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "Yes",
                        "No"
                    }));

            switch (selection)
            {
                case "Yes":
                    if (_loginSession.User is Admin admin)
                    {
                        await admin.DeleteUser(selectedUser.UserId);
                    }
                    await ViewUsers();
                    break;
                case "No":
                    await ViewUsers();
                    break;
            }
        }

        private async Task MakeUserAdmin(User selectedUser)
        {
            NewView();

            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"Are you sure you want to make user ({selectedUser.Name.EscapeMarkup()}) as Admin?")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "Yes",
                        "No"
                    }));

            switch (selection)
            {
                case "Yes":
                    if (_loginSession.User is Admin admin)
                    {
                        await admin.MakeUserAdmin(selectedUser.UserId);
                    }
                    await ViewUsers();
                    break;
                case "No":
                    await ViewUsers();
                    break;
            }
        }

        private async Task CreateNewCredential()
        {
            var username = AnsiConsole.Ask<string>("Enter [green]Username[/] :");
            
            var generateRandomPassword = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Do you want a random password generated?")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "Yes",
                        "No"
                    }));

            var password = "";
            if (generateRandomPassword == "Yes")
            {
                var isPasswordSatisfied = false;
                while (!isPasswordSatisfied)
                {
                    password = GenerateRandomPassword();
                    AnsiConsole.MarkupLine($"Your random password is [green]{password.EscapeMarkup()}[/]");
                    
                    var regenerateSelection = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("Do you want to generate again?")
                            .PageSize(10)
                            .AddChoices(new[] {
                                "No",
                                "Yes"
                            }));
                    
                    if (regenerateSelection == "No")
                    {
                        isPasswordSatisfied = true;
                    }
                }
            }
            else
            {
                password = AnsiConsole.Prompt(
                    new TextPrompt<string>("Enter [green]New Password[/] :")
                        .PromptStyle("red")
                        .Secret()
                );
            }
            
            
            
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
                        Username = username,
                        Password = password,
                        GameName = AnsiConsole.Ask<string>("Enter [green]Game Name[/] :"),
                        Developer = AnsiConsole.Ask<string>("Enter [green]Game developer[/] :")
                    };
                    await _loginSession.User.AddCredential(gameCredential);
                    break;
                case "Website":
                    var websiteCredential = new CredentialWebsite
                    {
                        Username = username,
                        Password = password,
                        WebsiteName = AnsiConsole.Ask<string>("Enter [green]Website name[/] :"),
                        Url = AnsiConsole.Ask<string>("Enter [green]Website Url[/] :")
                    };
                    await _loginSession.User.AddCredential(websiteCredential);
                    break;
                case "Desktop Application":
                    var desktopAppCredential = new CredentialDesktopApp
                    {
                        Username = username,
                        Password = password,
                        DesktopAppName = AnsiConsole.Ask<string>("Enter [green]Desktop application name[/] :")
                    };
                    await _loginSession.User.AddCredential(desktopAppCredential);
                    break;
            }
            
            AnsiConsole.MarkupLine("[green]Credential created successfully[/] Press any key to go back");
            Console.ReadKey();
            await MainMenu();
        }

        private async Task ViewCredentials(SortBy sortBy = SortBy.None, SortOrder sortOrder = SortOrder.None, string search = null)
        {
            Console.Clear();
            var credentialsList = await _loginSession.User.ViewCredentials(sortBy, sortOrder, search);
            if (!credentialsList.Any())
            {
                AnsiConsole.MarkupLine("[green]Can't find any credentials[/] Press any key to go back");
                Console.ReadKey();
                await MainMenu();
            }
            
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
                        case CredentialDesktopApp desktopApp:
                            additionalInfo = $"App Name: {desktopApp.DesktopAppName}";
                            type = "Desktop App";
                            break;
                        default:
                            additionalInfo = "";
                            type = "";
                            break;
                    }

                    var createdDateText = (credential.CreatedDate == DateTime.MinValue) ? "" : credential.CreatedDate.ToString(CultureInfo.InvariantCulture);
                    var updatedDateText = (credential.UpdatedDate == DateTime.MinValue) ? "" : credential.UpdatedDate.ToString(CultureInfo.InvariantCulture);

                    table.AddRow(
                        $"{style}{credential.Username.EscapeMarkup()}[/]",
                        $"{style}{credential.Password.EscapeMarkup()}[/]",
                        $"{style}{type.EscapeMarkup()}[/]",
                        $"{style}{additionalInfo.EscapeMarkup()}[/]",
                        $"{style}{createdDateText.EscapeMarkup()}[/]",
                        $"{style}{updatedDateText.EscapeMarkup()}[/]"
                    );
                    
                }

                AnsiConsole.Render(table);

                AnsiConsole.MarkupLine("Please use up and down arrow keys to select data row");
                AnsiConsole.MarkupLine("Press [bold green]'e'[/] to [bold green]edit[/]");
                AnsiConsole.MarkupLine("Press [bold green]'d'[/] to [bold green]delete[/]");
                AnsiConsole.MarkupLine("Press [bold green]'v'[/] to [bold green]view password[/]");
                if (string.IsNullOrEmpty(search))
                {
                    AnsiConsole.MarkupLine("Press [bold green]'s'[/] to [bold green]sort[/]");
                }
                AnsiConsole.MarkupLine("Press [bold green]'b'[/] to [bold green]back[/]");
                
                
                var key = Console.ReadKey().Key;

                var selectedCredential = credentialsList[selectedRowIndex];
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedRowIndex = Math.Max(0, selectedRowIndex - 1);
                        break;
                    case ConsoleKey.DownArrow:
                        selectedRowIndex = Math.Min(credentialsList.Count - 1, selectedRowIndex + 1);
                        break;
                    case ConsoleKey.E:
                        await UpdateCredential(selectedCredential);
                        await ViewCredentials();
                        break;
                    case ConsoleKey.D:
                        await DeleteCredential(selectedCredential);
                        await ViewCredentials();
                        break;
                    case ConsoleKey.S:
                        await SortCredential();
                        break;
                    case ConsoleKey.V:
                        await ViewPassword(selectedCredential);
                        await ViewCredentials();
                        break;
                    case ConsoleKey.B:
                        await MainMenu();
                        break;
                }

                Console.Clear(); 
            }
        }
        
        private async Task ViewAllCredentials()
        {
            Console.Clear();
            if (_loginSession.User is Admin admin)
            {
                var credentialsList = await admin.ViewAllCredentials();
                if (!credentialsList.Any())
                {
                    AnsiConsole.MarkupLine("[green]Can't find any credentials[/] Press any key to go back");
                    Console.ReadKey();
                    await MainMenu();
                }
                
                var selectedRowIndex = 0;

                while (true)
                {
                    // Display the table
                    var table = new Table();
                    table.AddColumn("User's Name");
                    table.AddColumn("User's Username");
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
                            case CredentialDesktopApp desktopApp:
                                additionalInfo = $"App Name: {desktopApp.DesktopAppName}";
                                type = "Desktop App";
                                break;
                            default:
                                additionalInfo = "";
                                type = "";
                                break;
                        }

                        var createdDateText = (credential.CreatedDate == DateTime.MinValue) ? "" : credential.CreatedDate.ToString(CultureInfo.InvariantCulture);
                        var updatedDateText = (credential.UpdatedDate == DateTime.MinValue) ? "" : credential.UpdatedDate.ToString(CultureInfo.InvariantCulture);

                        table.AddRow(
                            $"{style}{credential.User.Name.EscapeMarkup()}[/]",
                            $"{style}{credential.User.Username.EscapeMarkup()}[/]",
                            $"{style}{credential.Username.EscapeMarkup()}[/]",
                            $"{style}{credential.Password.EscapeMarkup()}[/]",
                            $"{style}{type.EscapeMarkup()}[/]",
                            $"{style}{additionalInfo.EscapeMarkup()}[/]",
                            $"{style}{createdDateText.EscapeMarkup()}[/]",
                            $"{style}{updatedDateText.EscapeMarkup()}[/]"
                        );
                        
                    }

                    AnsiConsole.Render(table);

                    AnsiConsole.MarkupLine("Please use up and down arrow keys to select data row");
                    AnsiConsole.MarkupLine("Press [bold green]'e'[/] to [bold green]edit[/]");
                    AnsiConsole.MarkupLine("Press [bold green]'d'[/] to [bold green]delete[/]");
                    AnsiConsole.MarkupLine("Press [bold green]'v'[/] to [bold green]view password[/]");
                    AnsiConsole.MarkupLine("Press [bold green]'b'[/] to [bold green]back[/]");
                    
                    
                    var key = Console.ReadKey().Key;

                    var selectedCredential = credentialsList[selectedRowIndex];
                    switch (key)
                    {
                        case ConsoleKey.UpArrow:
                            selectedRowIndex = Math.Max(0, selectedRowIndex - 1);
                            break;
                        case ConsoleKey.DownArrow:
                            selectedRowIndex = Math.Min(credentialsList.Count - 1, selectedRowIndex + 1);
                            break;
                        case ConsoleKey.E:
                            await UpdateCredential(selectedCredential);
                            await ViewAllCredentials();
                            break;
                        case ConsoleKey.D:
                            await DeleteCredential(selectedCredential);
                            await ViewAllCredentials();
                            break;
                        case ConsoleKey.V:
                            await ViewPassword(selectedCredential);
                            await ViewAllCredentials();
                            break;
                        case ConsoleKey.B:
                            await AdminMenu();
                            break;
                    }

                    Console.Clear(); 
                }
            }
            
        }

        private async Task ViewPassword(Credential credential)
        {
            NewView();
            AnsiConsole.MarkupLine($"Your {credential.Username.EscapeMarkup()} Password is [yellow]{new Encryption(credential.Password).Decrypt().EscapeMarkup()}[/] Press any key to go back");
            Console.ReadKey();
        }

        private async Task SortCredential()
        {
            NewView();
            var sortBy = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What column you need to sort?")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "UpdatedDate",
                        "CreatedDate",
                        "Username"
                    }));
            
            var sortOrder  = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What Sorting order?")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "Ascending",
                        "Descending"
                    }));
            
            SortBy sortByEnum = (SortBy)Enum.Parse(typeof(SortBy), sortBy);
            SortOrder sortOrderEnum = (SortOrder)Enum.Parse(typeof(SortOrder), sortOrder);
            await ViewCredentials(sortByEnum, sortOrderEnum);
        }

        private async Task UpdateCredential(Credential selectedCredential)
        {
            NewView();
            AnsiConsole.WriteLine($"Type the field you want to update, if you doesn't need to update a field just press ENTER.");
            var username = AnsiConsole.Ask($"Enter [green]Username[/] :", defaultValue: selectedCredential.Username.EscapeMarkup());
            var generateRandomPassword = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Do you want a random password generated?")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "No",
                        "Yes"
                    }));

            var password = "";
            if (generateRandomPassword == "Yes")
            {
                var isPasswordSatisfied = false;
                while (!isPasswordSatisfied)
                {
                    password = GenerateRandomPassword();
                    AnsiConsole.MarkupLine($"Your random password is [green]{password.EscapeMarkup()}[/]");
                    
                    var regenerateSelection = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("Do you want to generate again?")
                            .PageSize(10)
                            .AddChoices(new[] {
                                "No",
                                "Yes"
                            }));
                    
                    if (regenerateSelection == "No")
                    {
                        isPasswordSatisfied = true;
                    }
                }
            }
            else
            {
                password = AnsiConsole.Prompt(
                    new TextPrompt<string>($"Enter [green]New Password[/] :")
                        .PromptStyle("red")
                        .Secret()
                        .DefaultValue(new Encryption(selectedCredential.Password).Decrypt().EscapeMarkup())
                );
            }
            
            
            
            switch (selectedCredential)
            {
                case CredentialGame game:
                    var gameCredential = new CredentialGame
                    {
                        CredentialId = selectedCredential.CredentialId,
                        Username = username,
                        Password = password,
                        GameName = AnsiConsole.Ask($"Enter [green]Game Name[/] :", defaultValue: game.GameName.EscapeMarkup()),
                        Developer = AnsiConsole.Ask($"Enter [green]Game developer[/] :", defaultValue: game.Developer.EscapeMarkup())
                    };
                    await _loginSession.User.EditCredential(gameCredential);
                    break;
                case CredentialWebsite website:
                    var websiteCredential = new CredentialWebsite
                    {
                        CredentialId = selectedCredential.CredentialId,
                        Username = username,
                        Password = password,
                        WebsiteName = AnsiConsole.Ask($"Enter [green]Website name[/] :", defaultValue: website.WebsiteName.EscapeMarkup()),
                        Url = AnsiConsole.Ask($"Enter [green]Website Url[/] :", defaultValue: website.Url.EscapeMarkup())
                    };
                    await _loginSession.User.EditCredential(websiteCredential);
                    break;
                case CredentialDesktopApp desktopApp:
                    var desktopAppCredential = new CredentialDesktopApp
                    {
                        CredentialId = selectedCredential.CredentialId,
                        User = _loginSession.User,
                        Username = username,
                        Password = password,
                        DesktopAppName = AnsiConsole.Ask($"Enter [green]Desktop application name[/] :", defaultValue:desktopApp.DesktopAppName.EscapeMarkup())
                    };
                    await _loginSession.User.EditCredential(desktopAppCredential);
                    break;
            }
            
            AnsiConsole.MarkupLine("[green]Credential updated successfully[/] Press any key to go back");
            Console.ReadKey();
            
        }

        private async Task DeleteCredential(Credential selectedCredential)
        {
            NewView();

            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Are you sure you want to delete?")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "Yes",
                        "No"
                    }));

            switch (selection)
            {
                case "Yes":
                    await _loginSession.User.DeleteCredential(selectedCredential.CredentialId);
                    break;
                case "No":
                    break;
            }
        }

        private async Task SearchCredential()
        {
            NewView();
            var name = AnsiConsole.Ask<string>("Enter [green]Name[/] to search :");
            await ViewCredentials(SortBy.None, SortOrder.None, name);
        }

        private string GenerateRandomPassword()
        {
            var passwordGenerator = new RandomPasswordGenerator();

            var options = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                    .Title("Please select [green]following options[/], you can select multiple?")
                    .NotRequired() 
                    .PageSize(10)
                    .InstructionsText(
                        "[grey](Press [blue]<space>[/] to toggle a option, " + 
                        "[green]<enter>[/] to accept)[/]")
                    .AddChoices(new[] {
                        "Include Uppercase", 
                        "Include Numbers",
                        "Include Special Chars"
                    }));

            foreach (var option in options)
            {
                switch (option)
                {
                    case "Include Uppercase":
                        passwordGenerator.IncludeUppercase = true;
                        break;
                    case "Include Numbers":
                        passwordGenerator.IncludeDigits = true;
                        break;
                    case "Include Special Chars":
                        passwordGenerator.IncludeSpecialChars = true;
                        break;
                }
            }
            
            passwordGenerator.Length = AnsiConsole.Ask("Enter [green]length[/] of the password :", defaultValue: 12);
            return new Credential().GenerateRandomPassword(passwordGenerator);
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
                var loggedInUser = new Rule($"[yellow]Welcome {_loginSession.User.Name.EscapeMarkup()}[/]");
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
