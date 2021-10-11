// Decompiled with JetBrains decompiler
// Type: StardewValley.RainDrop
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;

namespace StardewValley
{
  public struct RainDrop
  {
    public int frame;
    public int accumulator;
    public Vector2 position;

    public RainDrop(int x, int y, int frame, int accumulator)
    {
      this.position = new Vector2((float) x, (float) y);
      this.frame = frame;
      this.accumulator = accumulator;
    }
  }
}
