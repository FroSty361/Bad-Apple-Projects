class Program
{
  static void Main(string[] args)
  {
    var input = GetInput();

    KeyboardController keyboardController = new KeyboardController();

    ImageParser imageParser = new ImageParser((input.x, input.y));
    
    List<Frame>? frames = imageParser.GetFrames();

    if (frames == null)
    {
      return;
    }

    keyboardController.Start(frames, input.seconds);
  }

  static (int x, int y, int seconds) GetInput()
  {
    int x = 21;
    int y = 6;
    
    int seconds = 209;

    while (true)
    {
      try
      {
        Console.WriteLine("Input The Max Width In Keys On Your Keyboard");

        x = Convert.ToInt16(Console.ReadLine());
      }
      catch (FormatException)
      {
        Console.WriteLine("Must Be An Integer");

        continue;
      }

      Console.WriteLine();

      try
      {
        Console.WriteLine("Input The Max Height In Keys On Your Keyboard");
        
        y = Convert.ToInt16(Console.ReadLine());
      }
      catch (FormatException)
      {
        Console.WriteLine("Must Be An Integer");

        continue;
      }

      try
      {
        Console.WriteLine("Input The Length Of Video In Seconds");
        
        seconds = Convert.ToInt16(Console.ReadLine());
      }
      catch (FormatException)
      {
        Console.WriteLine("Must Be An Integer");

        continue;
      }


      break;
    }

    return (x, y, seconds);
  }
}