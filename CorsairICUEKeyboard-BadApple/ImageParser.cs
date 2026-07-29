using System;
using System.IO;
using System.Collections.Generic;
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

  public string[] GetFrameImagePaths(string framesPath = "frames")
  {
    string[] frameImagePaths = [];

    string path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\", framesPath));

    try
    {
      frameImagePaths = Directory.GetFiles(path);

      return frameImagePaths;
    }
    catch (IOException)
    {
      throw new IOException($"Could Not Find The Path {path}");
    }
  }

  public Frame CreateFrame(string path)
  {
    Frame frame;

    using (MagickImage image = new MagickImage(path))
    {
      var size = new MagickGeometry((uint)width, (uint)height);
      size.IgnoreAspectRatio = true;
      // image.Resize(size);

      frame = new Frame((width, height));
      frame.ParsePixels(image);
    }

    return frame;
  }
}