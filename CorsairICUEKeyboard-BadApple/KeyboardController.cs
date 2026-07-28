using System.Diagnostics;
using System.Threading.Tasks;
using RGB.NET.Core;
using RGB.NET.Devices.Corsair;

public class KeyboardController
{
  RGBSurface surface = new RGBSurface();

  private IRGBDevice keyboard = null;

  public int KeyboardWidth { get; private set; }
  public int KeyboardHeight { get; private set; }

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

    KeyboardWidth = (int)Math.Ceiling(keyboard.Boundary.Size.Width);
    KeyboardHeight = (int)Math.Ceiling(keyboard.Boundary.Size.Height);

    Console.WriteLine($"{KeyboardWidth} {KeyboardHeight}");
  }

  public async Task Start(List<Frame> frames, int videoLengthSeconds)
  {
    if (keyboard == null)
    {
      throw new NullReferenceException("Can Not Start With No Keyboard");
    }

    await DisplayFrames(frames, videoLengthSeconds);
  }

  async Task DisplayFrames(List<Frame> frames, int videoLengthSeconds)
  {
    int framesAmount = frames.Count;

    if (videoLengthSeconds <= 0 || framesAmount <= 0)
    {
      throw new ArgumentException("No Video Length In Seconds Or Frames Amount");
    }

    double frameDurationSeconds = (double)videoLengthSeconds / framesAmount;

    Stopwatch stopwatch = Stopwatch.StartNew();

    int iterations = 0;

    foreach (Frame frame in frames)
    {
      TimeSpan targetTime = TimeSpan.FromSeconds(iterations * frameDurationSeconds);

      while (stopwatch.Elapsed < targetTime)
      {
        await Task.Delay(1); 
      }

      DisplayFrame(frame);
      surface.Update();

      iterations++;
    }

    stopwatch.Stop();
  }

  void DisplayFrame(Frame frame)
  {
    foreach (Led led in keyboard)
    {
      int x = (int)led.Location.X;
      int y = (int)led.Location.Y;

      Console.WriteLine($"{x} {y}");

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