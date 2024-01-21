using static System.Console;
namespace PassShieldPasswordManager;

public class Menu
{
    private int _selectedIndex;
    private List<string> _options;
    private string _prompt;

    public Menu(string prompt, List<string> options)
    {
        _selectedIndex = 0;
        _options = options;
        _prompt = prompt;
    }

    private void DisplayOptions()
    {
        WriteLine(_prompt);

        for (var index = 0; index < _options.Count; index++)
        {
            var option = _options[index];
            string prefix;

            if (index == _selectedIndex)
            {
                prefix = "*";
                ForegroundColor = ConsoleColor.Red;
            }
            else
            {
                prefix = " ";
                ForegroundColor = ConsoleColor.White;
            }
            
            WriteLine($"{prefix}[{option}]");
        }
        
        ResetColor();
    }

    public int Run()
    {

        ConsoleKey keyPressed;

        do
        {
            Clear();
            DisplayOptions();
            var keyInfo = ReadKey(true);
            keyPressed = keyInfo.Key;

            if (keyPressed == ConsoleKey.DownArrow)
            {
                _selectedIndex++;
                if (_selectedIndex == _options.Count)
                {
                    _selectedIndex = 0;
                }
            }
            else if (keyPressed == ConsoleKey.UpArrow)
            {
                _selectedIndex--;
                if (_selectedIndex == -1)
                {
                    _selectedIndex = _options.Count - 1;
                }
            }


        } while (keyPressed != ConsoleKey.Enter);
        
        return _selectedIndex;
    }
}