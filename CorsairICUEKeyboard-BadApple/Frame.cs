using ImageMagick;

public class Frame : IDisposable
{
  private int _isDisposed;

  public int[,] Grid { get; private set; }

  public Frame((int x, int y) dimensions)
  {
    Grid = new int[dimensions.y, dimensions.x];
  }

  ~Frame()
  {
    Dispose(false);
  }

  public void ParsePixels(MagickImage image)
  {
    using (var pixels = image.GetPixels())
    {
      int gridHeight = Grid.GetLength(0);
      int gridWidth = Grid.GetLength(1);

      for (int y = 0; y < gridHeight; y++)
      {
        for (int x = 0; x < gridWidth; x++)
        {
          int imageX = (int)((float)x / gridWidth * image.Width);
          imageX = (int)Math.Clamp(imageX, 0, image.Width - 1);

          int imageY = (int)((float)y / gridHeight * image.Height);
          imageY = (int)Math.Clamp(imageY, 0, image.Height - 1);

          var pixel = pixels.GetPixel(imageX,  imageY);

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

  public void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }

  protected virtual void Dispose(bool disposing)
  {
    if (Interlocked.CompareExchange(ref _isDisposed, 1, 0) == 0)
    {
      if (disposing)
      {
        Grid = null;
      }
    }
  }
}