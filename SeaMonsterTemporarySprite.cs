// Decompiled with JetBrains decompiler
// Type: StardewValley.SeaMonsterTemporarySprite
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley
{
  public class SeaMonsterTemporarySprite : TemporaryAnimatedSprite
  {
    public Texture2D texture;

    public SeaMonsterTemporarySprite(float animationInterval, int animationLength, int numberOfLoops, Vector2 position)
      : base(-666, animationInterval, animationLength, numberOfLoops, position, false, false)
    {
      this.texture = Game1.content.Load<Texture2D>("LooseSprites\\SeaMonster");
      Game1.playSound("pullItemFromWater");
      this.currentParentTileIndex = 0;
    }

    public override void draw(SpriteBatch spriteBatch, bool localPosition = false, int xOffset = 0, int yOffset = 0)
    {
      spriteBatch.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, this.Position), new Rectangle?(new Rectangle(this.currentParentTileIndex * Game1.tileSize, 0, Game1.tileSize, Game1.tileSize)), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, (float) (((double) this.Position.Y + (double) (Game1.tileSize / 2)) / 10000.0));
    }

    public override bool update(GameTime time)
    {
      this.timer = this.timer + (float) time.ElapsedGameTime.Milliseconds;
      if ((double) this.timer > (double) this.interval)
      {
        this.currentParentTileIndex = this.currentParentTileIndex + 1;
        this.timer = 0.0f;
        if (this.currentParentTileIndex >= this.animationLength)
        {
          this.currentNumberOfLoops = this.currentNumberOfLoops + 1;
          this.currentParentTileIndex = 2;
        }
      }
      if (this.currentNumberOfLoops >= this.totalNumberOfLoops)
      {
        this.position.Y += 2f;
        if ((double) this.position.Y >= (double) Game1.currentLocation.Map.DisplayHeight)
          return true;
      }
      return false;
    }
  }
}
