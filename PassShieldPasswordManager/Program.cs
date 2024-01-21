using PassShieldPasswordManager;
using static System.Console;

//var keyPressed = ReadKey();

// if (keyPressed.Key == ConsoleKey.Enter)
// {
//     WriteLine("You pressed ENTER");
// }

Title = "Pass Shield Password Manager";
RunMainMenu();


void RunMainMenu()
{
    var prompt = "Password Manger";
    List<string> options =
    [
        "Login",
        "Exit"
    ];
    var mainMenu = new Menu(prompt, options);
    var selectedIndex = mainMenu.Run();

    switch (selectedIndex)
    {
        case 0:
            Login();
            break;
        case 1:
            Exit();
            break;
    }
}

void Login()
{
    var prompt = "Password Manger";
    List<string> options =
    [
        "Enter",
        "Logout"
    ];
    var mainMenu = new Menu(prompt, options);
    var selectedIndex = mainMenu.Run();

    switch (selectedIndex)
    {
        case 0:
            
            break;
        case 1:
            RunMainMenu();
            break;
    }
}

void Exit()
{
    Environment.Exit(0);
}