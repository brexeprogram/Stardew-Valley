// Decompiled with JetBrains decompiler
// Type: StardewValley.TerrainFeatures.Leaf
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;

namespace StardewValley.TerrainFeatures
{
  public class Leaf
  {
    public Vector2 position;
    public float rotation;
    public float rotationRate;
    public float yVelocity;
    public int type;

    public Leaf(Vector2 position, float rotationRate, int type, float yVelocity)
    {
      this.position = position;
      this.rotationRate = rotationRate;
      this.type = type;
      this.yVelocity = yVelocity;
    }
  }
}
