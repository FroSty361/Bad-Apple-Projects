using System.Threading.Tasks;

class Program
{
  static async Task Main(string[] args)
  {
    int userInput = GetUserInput();

    KeyboardController keyboardController = new KeyboardController();

    await keyboardController.Start(userInput);
  }

  static int GetUserInput()
  {
    int seconds = 209;

    while (true)
    {
      try
      {
        Console.WriteLine("Input The Length Of Video In Seconds");
        
        int secondsAnswer = Convert.ToInt16(Console.ReadLine());

        if (secondsAnswer <= 0)
        {
          Console.WriteLine("Input Must Be More Than 0");
        }
        else
        {
          seconds = secondsAnswer;

          break;
        }
      }
      catch (FormatException)
      {
        Console.WriteLine("Must Be An Integer");
      }
    }

    return seconds;
  }
}