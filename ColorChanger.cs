// Decompiled with JetBrains decompiler
// Type: StardewValley.ColorChanger
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley
{
  internal class ColorChanger
  {
    private static bool IsSimilar(Color original, Color test, int redDelta, int blueDelta, int greenDelta)
    {
      if (Math.Abs((int) original.R - (int) test.R) < redDelta && Math.Abs((int) original.G - (int) test.G) < greenDelta)
        return Math.Abs((int) original.B - (int) test.B) < blueDelta;
      return false;
    }

    public static Texture2D changeColor(Texture2D texture, int targetColorIndex, int redtint, int greentint, int bluetint, int range)
    {
      Color[] data = new Color[texture.Width * texture.Height];
      texture.GetData<Color>(data);
      Color test = data[targetColorIndex];
      for (int index = 0; index < data.Length; ++index)
      {
        if (ColorChanger.IsSimilar(data[index], test, range, range, range))
          data[index] = new Color((int) data[index].R + redtint, (int) data[index].G + greentint, (int) data[index].B + bluetint);
      }
      texture.SetData<Color>(data);
      return texture;
    }

    public static Texture2D changeColor(Texture2D texture, int targetColorIndex, Color baseColor, int range)
    {
      Color[] data = new Color[texture.Width * texture.Height];
      texture.GetData<Color>(data);
      Color test = data[targetColorIndex];
      for (int index = 0; index < data.Length; ++index)
      {
        if (ColorChanger.IsSimilar(data[index], test, range, range, range))
          data[index] = new Color((int) baseColor.R + ((int) data[index].R - (int) test.R), (int) baseColor.G + ((int) data[index].G - (int) test.G), (int) baseColor.B + ((int) data[index].B - (int) test.B));
      }
      texture.SetData<Color>(data);
      return texture;
    }

    public static Texture2D swapColor(Texture2D texture, int targetColorIndex, int red, int green, int blue)
    {
      return ColorChanger.swapColor(texture, targetColorIndex, red, green, blue, 0, texture.Width * texture.Height);
    }

    public static Texture2D swapColor(Texture2D texture, int targetColorIndex, int red, int green, int blue, int startPixel, int endPixel)
    {
      red = Math.Min(Math.Max(1, red), (int) byte.MaxValue);
      green = Math.Min(Math.Max(1, green), (int) byte.MaxValue);
      blue = Math.Min(Math.Max(1, blue), (int) byte.MaxValue);
      Color[] data = new Color[texture.Width * texture.Height];
      texture.GetData<Color>(data);
      Color color = data[targetColorIndex];
      for (int index = 0; index < data.Length; ++index)
      {
        if (index >= startPixel && index < endPixel && ((int) data[index].R == (int) color.R && (int) data[index].G == (int) color.G) && (int) data[index].B == (int) color.B)
          data[index] = new Color(red, green, blue);
      }
      texture.SetData<Color>(data);
      return texture;
    }
  }
}
