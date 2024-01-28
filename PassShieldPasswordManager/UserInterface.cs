using Spectre.Console;

namespace PassShieldPasswordManager
{
    public class UserInterface
    {
        private readonly Account _account = new();

        public async Task Run()
        {
            try
            {
                await ApplicationMenu();
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
                        "Exit"
                    }));

            switch (selection)
            {
                case "Login":
                    await Login();
                    break;
                case "Register":
                    await Register(); // Example, you might need to create a separate RegisterMenu method
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
            newUser.Username = AnsiConsole.Ask<string>("Enter [green]Username[/] :");
            newUser.Password = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter [green]Password[/] :")
                    .PromptStyle("red")
                    .Secret()
            );
            newUser.SecurityQuestionId = AnsiConsole.Prompt(
                new SelectionPrompt<int>()
                    .Title("Select [green]Security Question[/] :")
                    .PageSize(10)
                    .AddChoices(new[] {
                        1,
                        2,
                        3
                    }));
            newUser.SecurityAnswer = AnsiConsole.Ask<string>("Enter [green]Security Answer[/] :");
            newUser.DateCreated = DateTime.Now;
            newUser = await _account.Register(newUser);
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
                if (user is Admin)
                {
                    MainMenu(true);
                }
                else
                {
                    MainMenu();
                }
                
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
                        ResetPassword();
                        break;
                    case "Back":
                        await ApplicationMenu();
                        break;
                }
            }
        }

        private void ResetPassword()
        {
            // Your implementation here
        }

        private void MainMenu(bool isAdmin = false)
        {
            // Your implementation here
        }

        private void NewView(string loggedInUser = null)
        {
            Console.Clear();
            AnsiConsole.Write(
                new FigletText("Pass Shield")
                    .Color(Color.Green)
                    .Centered());

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
