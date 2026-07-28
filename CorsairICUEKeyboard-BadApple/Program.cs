using System.Threading.Tasks;

class Program
{
  static async Task Main(string[] args)
  {
    int userInput = GetInput();

    KeyboardController keyboardController = new KeyboardController();

    ImageParser imageParser = new ImageParser((keyboardController.KeyboardWidth, keyboardController.KeyboardHeight));
    
    List<Frame>? frames = imageParser.GetFrames();

    if (frames == null)
    {
      return;
    }

    await keyboardController.Start(frames, userInput);
  }

  static int GetInput()
  {
    int seconds = 209;

    while (true)
    {
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

    return seconds;
  }
}