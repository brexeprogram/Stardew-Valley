// Decompiled with JetBrains decompiler
// Type: StardewValley.LivestockSprite
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley
{
  public class LivestockSprite : AnimatedSprite
  {
    public LivestockSprite(Texture2D texture, int currentFrame)
      : base(texture, 0, Game1.tileSize, Game1.tileSize * 3 / 2)
    {
    }

    public override void faceDirection(int direction)
    {
      switch (direction)
      {
        case 0:
          this.CurrentFrame = this.spriteTexture.Width / Game1.tileSize * 2 + this.CurrentFrame % (this.spriteTexture.Width / Game1.tileSize);
          break;
        case 1:
          this.CurrentFrame = this.spriteTexture.Width / (Game1.tileSize * 2) + this.CurrentFrame % (this.spriteTexture.Width / (Game1.tileSize * 2));
          break;
        case 2:
          this.CurrentFrame = this.CurrentFrame % (this.spriteTexture.Width / Game1.tileSize);
          break;
        case 3:
          this.CurrentFrame = this.spriteTexture.Width / (Game1.tileSize * 2) * 3 + this.CurrentFrame % (this.spriteTexture.Width / (Game1.tileSize * 2));
          break;
      }
      this.UpdateSourceRect();
    }

    public override void UpdateSourceRect()
    {
      switch (this.currentFrame)
      {
        case 0:
        case 1:
        case 2:
        case 3:
          this.SourceRect = new Rectangle(this.currentFrame * Game1.tileSize, 0, Game1.tileSize, Game1.tileSize * 3 / 2);
          break;
        case 4:
        case 5:
        case 6:
        case 7:
          this.SourceRect = new Rectangle(this.currentFrame % 4 * Game1.tileSize * 2, Game1.tileSize * 3 / 2, Game1.tileSize * 2, Game1.tileSize * 3 / 2);
          break;
        case 8:
        case 9:
        case 10:
        case 11:
          this.SourceRect = new Rectangle(this.currentFrame % 4 * Game1.tileSize, Game1.tileSize * 3, Game1.tileSize, Game1.tileSize * 3 / 2);
          break;
        case 12:
        case 13:
        case 14:
        case 15:
          this.SourceRect = new Rectangle(this.currentFrame % 4 * Game1.tileSize * 2, Game1.tileSize * 9 / 2, Game1.tileSize * 2, Game1.tileSize * 3 / 2);
          break;
        case 24:
        case 25:
        case 26:
        case 27:
          this.SourceRect = new Rectangle((this.currentFrame - 20) * Game1.tileSize, Game1.tileSize * 3, Game1.tileSize, Game1.tileSize * 3 / 2);
          break;
      }
    }
  }
}
