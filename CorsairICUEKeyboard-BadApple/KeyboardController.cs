using RGB.NET.Core;
using RGB.NET.Devices.Corsair;

public class KeyboardController
{
  RGBSurface surface = new RGBSurface();

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

    Console.WriteLine($"Devices Found = {CorsairDeviceProvider.Instance.Devices.Count}");

    foreach (var d in CorsairDeviceProvider.Instance.Devices)
    {
      Console.WriteLine($"  {d.DeviceInfo.DeviceName} ({d.DeviceInfo.DeviceType})");
    }

    surface.Attach(CorsairDeviceProvider.Instance.Devices);
    surface.RegisterUpdateTrigger(new TimerUpdateTrigger());
    surface.AlignDevices();
  }

  public void Start(List<Frame> frames, int videoLengthSeconds)
  {
    Console.WriteLine(surface.Devices.Count);

    foreach (var device in surface.Devices)
    {
      if (device.DeviceInfo.DeviceType == RGBDeviceType.Keyboard)
      {
        DisplayFrames(frames, device, videoLengthSeconds);

        return;
      }
    }

    Console.WriteLine("No Keyboard Was Found");
  }

  void DisplayFrames(List<Frame> frames, IRGBDevice keyboard, int videoLengthSeconds)
  {
    int framesAmount = frames.Count;

    foreach (Frame frame in frames)
    {
      DisplayFrame(frame, keyboard);

      surface.Update();

      if (videoLengthSeconds > 0 && framesAmount > 0)
      {
        int delay = framesAmount / videoLengthSeconds;

        Thread.Sleep(delay);
      }
    }
  }

  void DisplayFrame(Frame frame, IRGBDevice keyboard)
  {
    int ledAmount = keyboard.Count();
    int iterations = 0;

    for (int y = 0; y < frame.Height; y++)
    {
      for (int x = 0; x < frame.Width; x++)
      {
        int pixel = frame.Grid[y, x];

        if (iterations >= ledAmount)
        {
          Console.WriteLine($"Grid Area To LED Amount Ratio Is Un Even. Iteration Index = {iterations} LED Amount = {ledAmount}");

          break;
        }

        Console.WriteLine("Hi!");

        Led led = keyboard.ElementAt(iterations);

        if (pixel == 1)
        {
          led.Color = new Color(1.0f, 1.0f, 1.0f); // White
        }
        else
        {
          led.Color = new Color(0.0f, 0.0f, 0.0f); // Black
        }

        iterations++;
      }
    }
  }
}