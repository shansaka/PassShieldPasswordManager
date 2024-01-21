using Spectre.Console;
using static System.Console;
namespace PassShieldPasswordManager;

public class Menu
{
    public void Run()
    {
        ApplicationMenu();
    }

    private void ApplicationMenu()
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
                LoginMenu();
                break;
            case "Register":
                LoginMenu();
                break;
            case "Exit":
                Exit();
                break;
        }
    }

    private void Exit()
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
                ApplicationMenu();
                break;
        }
       
    }
    
    private void LoginMenu()
    {
        NewView();

        var username = AnsiConsole.Ask<string>("Enter [green]Username[/] :");
        var password = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter [green]Password[/] :")
                .PromptStyle("red")
                .Secret()
            );
        
        // Checking login here.
        var isLogin = (username == "admin" && password == "123");

        if (isLogin)
        {
            MainMenu();    
        }
        
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
                LoginMenu();
                break;
            case "Reset Password":
                ResetPassword();
                break;
            case "Back":
                ApplicationMenu();
                break;
        }
    }

    private void ResetPassword()
    {
        throw new NotImplementedException();
    }

    private void MainMenu()
    {
        int selectedRowIndex = 0;
        List<UserData> userDataList = new List<UserData>
        {
            new UserData { Id = 1, Name = "John", Age = 25 },
            new UserData { Id = 2, Name = "Alice", Age = 30 },
            new UserData { Id = 3, Name = "Bob", Age = 28 }
        };

        Console.CursorVisible = false;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("ID\tName\tAge");

            for (int i = 0; i < userDataList.Count; i++)
            {
                Console.ForegroundColor = (i == selectedRowIndex) ? ConsoleColor.White : ConsoleColor.Gray;
                Console.WriteLine($"{userDataList[i].Id}\t{userDataList[i].Name}\t{userDataList[i].Age}");
            }

            ConsoleKeyInfo keyInfo = Console.ReadKey();

            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    selectedRowIndex = Math.Max(0, selectedRowIndex - 1);
                    break;
                case ConsoleKey.DownArrow:
                    selectedRowIndex = Math.Min(userDataList.Count - 1, selectedRowIndex + 1);
                    break;
                case ConsoleKey.Enter:
                    Console.WriteLine(selectedRowIndex);
                    break;
                case ConsoleKey.C:
                    Console.WriteLine("Create");
                    return;
                    break;
                case ConsoleKey.G:
                    Console.WriteLine("Genarate Random Password");
                    return;
                    break;
                default:
                    // Handle other keys if needed
                    break;
            }
        }
    }

    private void NewView(string? loggedInUser = null)
    {
        Clear();
        AnsiConsole.Write(
            new FigletText("Pass Shield")
                .Color(Color.Green)
                .Centered());
        
        AnsiConsole.Write(new Rule());
        var rule = new Rule("Please use up and down arrow keys to cycle through menu options");
        rule.Border = BoxBorder.Ascii;
        rule.Centered();
        AnsiConsole.Write(rule);
        //AnsiConsole.WriteLine("Please use up and down arrow keys to cycle through menu options");
        AnsiConsole.Write(new Rule());
        AnsiConsole.WriteLine("");
        
    }
}

class UserData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}