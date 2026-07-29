using ImageMagick;

public class Frame
{
  public int Width { get; private set; } = 21;

  public int Height { get; private set; } = 6;

  public int[,] Grid { get; private set; }

  public Frame((int x, int y) dimensions)
  {
    Width = dimensions.x;
    Height = dimensions.y;

    Grid = new int[Height, Width];
  }

  public void ParsePixels(MagickImage image)
  {
    var pixels = image.GetPixels();

    for (int y = 0; y < image.Height; y++)
    {
      for (int x = 0; x < image.Width; x++)
      {
        var pixel = pixels.GetPixel(x, y);

        double luminance = (pixel.GetChannel(0) * 0.299) + (pixel.GetChannel(1) * 0.587) + (pixel.GetChannel(2) * 0.114);

        if (luminance >= 32767.5)
        {
          Grid[y, x] = 1; // White
        }
        else
        {
          Grid[y, x] = 0; // Black
        }
      }
    }
  }
}