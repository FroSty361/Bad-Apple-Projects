using System.Diagnostics;
using System.Threading.Tasks;
using RGB.NET.Core;
using RGB.NET.Devices.Corsair;

public class KeyboardController
{
  RGBSurface surface = new RGBSurface();

  private IRGBDevice keyboard = null;

  private int keyboardWidth;
  private int keyboardHeight;

  ImageParser imageParser = null;

  public KeyboardController()
  {
    Init();
  }

  void Init()
  {
    try
    {
      CorsairDeviceProvider.Instance.Initialize(throwExceptions: true);
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex);

      return;
    }

    Thread.Sleep(1500);

    surface.Attach(CorsairDeviceProvider.Instance.Devices);
    surface.RegisterUpdateTrigger(new TimerUpdateTrigger());
    surface.AlignDevices();

    keyboard = GetKeyboard();
    GetKeyboardData();

    imageParser = new ImageParser((keyboardWidth, keyboardHeight));
  }

  private IRGBDevice GetKeyboard()
  {
    foreach (var device in surface.Devices)
    {
      if (device.DeviceInfo.DeviceType == RGBDeviceType.Keyboard)
      {
        return device;
      }
    }

    throw new ArgumentNullException("No Keyboard Was Found");
  }

  private void GetKeyboardData()
  {
    if (keyboard == null)
    {
      throw new NullReferenceException("Keyboard Is Null");
    }

    keyboardWidth = (int)Math.Ceiling(keyboard.Boundary.Size.Width);
    keyboardHeight = (int)Math.Ceiling(keyboard.Boundary.Size.Height);
  }

  public async Task Start((int seconds, bool loop) userInput, CancellationToken cancelToken = default)
  {
    if (keyboard == null)
    {
      throw new NullReferenceException("Can Not Start With No Keyboard");
    }

    if (imageParser == null)
    {
      throw new NullReferenceException("Can Not Get Frame Paths Because Image Parser Is Null");
    }

    string[] framePaths = imageParser.GetFrameImagePaths();

    if (userInput.loop == true)
    {
      while (!cancelToken.IsCancellationRequested)
      {
        await DisplayFrames(framePaths, userInput.seconds);
      }
    }
    else
    {
      await DisplayFrames(framePaths, userInput.seconds);
    }
  }

  async Task DisplayFrames(string[] framePaths, int videoLengthSeconds)
  {
    int framesAmount = framePaths.Length;

    if (videoLengthSeconds <= 0 || framesAmount <= 0)
    {
      throw new ArgumentException("No Video Length In Seconds Or Frames Amount");
    }

    double fps = (double)framesAmount / videoLengthSeconds;
    double frameDurationSeconds = 1.0 / fps;

    Stopwatch stopwatch = Stopwatch.StartNew();

    for (int i = 0; i < framesAmount; i++)
    {
      TimeSpan targetTime = TimeSpan.FromSeconds(i * frameDurationSeconds);

      if (imageParser == null)
      {
        throw new NullReferenceException("Can Not Create Frame Because Image Parser Is Null");
      }

      Frame frame = imageParser.CreateFrame(framePaths[i]);

      DisplayFrame(frame);
      surface.Update();

      TimeSpan timeToWait = targetTime - stopwatch.Elapsed;

      if (timeToWait > TimeSpan.Zero)
      {
        await Task.Delay(timeToWait);
      }
    }

    stopwatch.Stop();
  }

  void DisplayFrame(Frame frame)
  {
    foreach (Led led in keyboard)
    {
      int x = (int)led.Location.X;
      int y = (int)led.Location.Y;

      if (y >= frame.Height || x >= frame.Width)
      {
        break;
      }
      
      int pixel = frame.Grid[y, x];

      if (pixel == 1)
      {
        led.Color = new Color(1.0f, 1.0f, 1.0f); // White
      }
      else
      {
        led.Color = new Color(0.0f, 0.0f, 0.0f); // Black
      }
    }
  }
}