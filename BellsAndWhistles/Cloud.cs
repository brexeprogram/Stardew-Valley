// Decompiled with JetBrains decompiler
// Type: StardewValley.BellsAndWhistles.Cloud
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.BellsAndWhistles
{
  public class Cloud : Critter
  {
    public int zoom = 5;
    public const int width = 147;
    public const int height = 100;
    private bool verticalFlip;
    private bool horizontalFlip;

    public Cloud()
    {
    }

    public Cloud(Vector2 position)
    {
      this.position = position * (float) Game1.tileSize;
      this.startingPosition = position;
      this.verticalFlip = Game1.random.NextDouble() < 0.5;
      this.horizontalFlip = Game1.random.NextDouble() < 0.5;
      this.zoom = Game1.random.Next(4, 7);
    }

    public override bool update(GameTime time, GameLocation environment)
    {
      this.position.Y -= (float) (time.ElapsedGameTime.TotalMilliseconds * 0.0199999995529652);
      this.position.X -= (float) (time.ElapsedGameTime.TotalMilliseconds * 0.0199999995529652);
      return (double) this.position.X < (double) (-147 * this.zoom) || (double) this.position.Y < (double) (-100 * this.zoom);
    }

    public override Rectangle getBoundingBox(int xOffset, int yOffset)
    {
      return new Rectangle((int) this.position.X, (int) this.position.Y, 147 * this.zoom, 100 * this.zoom);
    }

    public override void draw(SpriteBatch b)
    {
    }

    public override void drawAboveFrontLayer(SpriteBatch b)
    {
      b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(this.position), new Rectangle?(new Rectangle(128, 0, 146, 99)), Color.White, !this.verticalFlip || !this.horizontalFlip ? 0.0f : 3.141593f, Vector2.Zero, (float) this.zoom, !this.verticalFlip || this.horizontalFlip ? (!this.horizontalFlip || this.verticalFlip ? SpriteEffects.None : SpriteEffects.FlipHorizontally) : SpriteEffects.FlipVertically, 1f);
    }
  }
}
