// Decompiled with JetBrains decompiler
// Type: StardewValley.Monsters.ShadowBrute
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Monsters
{
  public class ShadowBrute : Monster
  {
    public ShadowBrute()
    {
      this.sprite.spriteHeight = 32;
    }

    public ShadowBrute(Vector2 position)
      : base("Shadow Brute", position)
    {
      this.sprite.spriteHeight = 32;
      this.sprite.UpdateSourceRect();
    }

    public override void reloadSprite()
    {
      this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Monsters\\Shadow Brute"));
      this.sprite.spriteHeight = 32;
      this.sprite.UpdateSourceRect();
    }

    public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
    {
      Game1.playSound("shadowHit");
      return base.takeDamage(damage, xTrajectory, yTrajectory, isBomb, addedPrecision);
    }

    public override void deathAnimation()
    {
      Utility.makeTemporarySpriteJuicier(new TemporaryAnimatedSprite(45, this.position, Color.White, 10, false, 100f, 0, -1, -1f, -1, 0), Game1.currentLocation, 4, 64, 64);
      for (int index = 1; index < 3; ++index)
      {
        Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(6, this.position + new Vector2(0.0f, 1f) * (float) Game1.tileSize * (float) index, Color.Gray * 0.75f, 10, false, 100f, 0, -1, -1f, -1, 0)
        {
          delayBeforeAnimationStart = index * 159
        });
        Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(6, this.position + new Vector2(0.0f, -1f) * (float) Game1.tileSize * (float) index, Color.Gray * 0.75f, 10, false, 100f, 0, -1, -1f, -1, 0)
        {
          delayBeforeAnimationStart = index * 159
        });
        Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(6, this.position + new Vector2(1f, 0.0f) * (float) Game1.tileSize * (float) index, Color.Gray * 0.75f, 10, false, 100f, 0, -1, -1f, -1, 0)
        {
          delayBeforeAnimationStart = index * 159
        });
        Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(6, this.position + new Vector2(-1f, 0.0f) * (float) Game1.tileSize * (float) index, Color.Gray * 0.75f, 10, false, 100f, 0, -1, -1f, -1, 0)
        {
          delayBeforeAnimationStart = index * 159
        });
      }
      Game1.playSound("shadowDie");
      Game1.createRadialDebris(Game1.currentLocation, this.sprite.Texture, new Rectangle(this.sprite.SourceRect.X, this.sprite.SourceRect.Y, 16, 5), 16, this.getStandingX(), this.getStandingY() - Game1.tileSize / 2, 1, this.getStandingY() / Game1.tileSize, Color.White, (float) Game1.pixelZoom);
      Game1.createRadialDebris(Game1.currentLocation, this.sprite.Texture, new Rectangle(this.sprite.SourceRect.X + 2, this.sprite.SourceRect.Y + 5, 16, 5), 10, this.getStandingX(), this.getStandingY() - Game1.tileSize / 2, 1, this.getStandingY() / Game1.tileSize, Color.White, (float) Game1.pixelZoom);
    }
  }
}
