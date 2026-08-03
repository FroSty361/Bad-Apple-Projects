class Program
{
  static async Task Main(string[] args)
  {
    var userInput = GetUserInput();

    KeyboardController keyboardController = new KeyboardController();

    await keyboardController.Start(userInput);
  }

  static (int seconds, bool loop) GetUserInput()
  {
    int seconds = 209;
    bool loop = false;

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

    while (true)
    {
      Console.WriteLine("Loop Video? {y} Or {n}");
      string loopAnswer = Console.ReadLine().ToLower();

      if (loopAnswer == "y")
      {
        loop = true;

        break;
      }
      else if (loopAnswer == "n")
      {
        loop = false;

        break;
      }
    }

    return (seconds, loop);
  }
}