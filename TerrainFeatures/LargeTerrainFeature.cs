// Decompiled with JetBrains decompiler
// Type: StardewValley.TerrainFeatures.LargeTerrainFeature
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.TerrainFeatures
{
  public class LargeTerrainFeature : TerrainFeature
  {
    public Vector2 tilePosition;

    public Rectangle getBoundingBox()
    {
      return this.getBoundingBox(this.tilePosition);
    }

    public void dayUpdate(GameLocation l)
    {
      this.dayUpdate(l, this.tilePosition);
    }

    public bool tickUpdate(GameTime time)
    {
      return this.tickUpdate(time, this.tilePosition);
    }

    public void draw(SpriteBatch b)
    {
      this.draw(b, this.tilePosition);
    }
  }
}
