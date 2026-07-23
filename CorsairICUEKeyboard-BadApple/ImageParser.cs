using System;
using System.IO;
using System.Linq.Expressions;
using ImageMagick;

public class ImageParser
{
  int width = 21;
  int height = 6;

  public ImageParser((int x, int y) dimensions)
  {
    width = dimensions.x;
    height = dimensions.y;
  }

  public List<Frame>? GetFrames(string framesPath = "frames")
  {
    List<Frame> frames = new List<Frame>();
    string[] imageNames = [];

    string path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\", framesPath));

    try
    {
      imageNames = Directory.GetFiles(path);
    }
    catch (IOException)
    {
      Console.WriteLine($"Could Not Find The Path {path}");

      return null;
    }

    foreach (string imageName in imageNames)
    {
      using (MagickImage image = new MagickImage($"{framesPath}/{imageName}"))
      {
        var size = new MagickGeometry((uint)width, (uint)height);
        size.IgnoreAspectRatio = true;
        image.Resize(size);

        Frame frame = new Frame((width, height));
        frame.ParsePixels(image);

        frames.Add(frame);

        Console.WriteLine(frames.Count());
      }
    }

    return frames;
  }
}