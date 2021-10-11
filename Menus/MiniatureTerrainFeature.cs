// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.MiniatureTerrainFeature
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.TerrainFeatures;

namespace StardewValley.Menus
{
  public class MiniatureTerrainFeature
  {
    private TerrainFeature feature;
    private Vector2 positionOnScreen;
    private Vector2 tileLocation;
    private float scale;

    public MiniatureTerrainFeature(TerrainFeature feature, Vector2 positionOnScreen, Vector2 tileLocation, float scale)
    {
      this.feature = feature;
      this.positionOnScreen = positionOnScreen;
      this.scale = scale;
      this.tileLocation = tileLocation;
    }

    public void draw(SpriteBatch b)
    {
      this.feature.drawInMenu(b, this.positionOnScreen, this.tileLocation, this.scale, 0.86f);
    }
  }
}
