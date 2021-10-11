// Decompiled with JetBrains decompiler
// Type: StardewValley.Prop
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley
{
  public class Prop
  {
    private Texture2D texture;
    private Rectangle sourceRect;
    private Rectangle drawRect;
    private Rectangle boundingRect;
    private bool solid;

    public Prop(Texture2D texture, int index, int tilesWideSolid, int tilesHighSolid, int tilesHighDraw, int tileX, int tileY, bool solid = true)
    {
      this.texture = texture;
      this.sourceRect = Game1.getSourceRectForStandardTileSheet(texture, index, 16, 16);
      this.sourceRect.Width = tilesWideSolid * 16;
      this.sourceRect.Height = tilesHighDraw * 16;
      this.drawRect = new Rectangle(tileX * Game1.tileSize, tileY * Game1.tileSize + (tilesHighSolid - tilesHighDraw) * Game1.tileSize, tilesWideSolid * Game1.tileSize, tilesHighDraw * Game1.tileSize);
      this.boundingRect = new Rectangle(tileX * Game1.tileSize, tileY * Game1.tileSize, tilesWideSolid * Game1.tileSize, tilesHighSolid * Game1.tileSize);
      this.solid = solid;
    }

    public bool isColliding(Rectangle r)
    {
      if (this.solid)
        return r.Intersects(this.boundingRect);
      return false;
    }

    public void draw(SpriteBatch b)
    {
      this.drawRect.X = this.boundingRect.X - Game1.viewport.X;
      this.drawRect.Y = this.boundingRect.Y + (this.boundingRect.Height - this.drawRect.Height) - Game1.viewport.Y;
      b.Draw(this.texture, this.drawRect, new Rectangle?(this.sourceRect), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, this.solid ? (float) this.boundingRect.Y / 10000f : 0.0f);
    }
  }
}
