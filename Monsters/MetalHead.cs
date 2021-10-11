// Decompiled with JetBrains decompiler
// Type: StardewValley.Monsters.MetalHead
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley.Monsters
{
  public class MetalHead : Monster
  {
    public Color c;

    public MetalHead()
    {
    }

    public MetalHead(Vector2 tileLocation)
      : this(tileLocation, Game1.mine.getMineArea(-1))
    {
    }

    public MetalHead(Vector2 tileLocation, int mineArea)
      : base("Metal Head", tileLocation)
    {
      this.sprite.spriteHeight = 16;
      this.sprite.UpdateSourceRect();
      this.c = Color.White;
      this.IsWalkingTowardPlayer = true;
      switch (Game1.mine.getMineArea(-1))
      {
        case 0:
          this.c = Color.White;
          break;
        case 40:
          this.c = Color.Turquoise;
          this.health = this.health * 2;
          break;
        case 80:
          this.c = Color.White;
          this.health = this.health * 3;
          break;
      }
    }

    public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
    {
      return this.takeDamage(damage, xTrajectory, yTrajectory, isBomb, addedPrecision, "clank");
    }

    public override void deathAnimation()
    {
      Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(46, this.position, Color.DarkGray, 10, false, 70f, 0, -1, -1f, -1, 0));
      Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(46, this.position + new Vector2((float) (-Game1.tileSize / 2), 0.0f), Color.DarkGray, 10, false, 70f, 0, -1, -1f, -1, 0)
      {
        delayBeforeAnimationStart = 300
      });
      Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(46, this.position + new Vector2((float) (Game1.tileSize / 2), 0.0f), Color.DarkGray, 10, false, 70f, 0, -1, -1f, -1, 0)
      {
        delayBeforeAnimationStart = 600
      });
      Game1.playSound("monsterdead");
      TemporaryAnimatedSprite t = new TemporaryAnimatedSprite(44, this.position, Color.MediumPurple, 10, false, 100f, 0, -1, -1f, -1, 0);
      t.holdLastFrame = true;
      t.alphaFade = 0.01f;
      t.interval = 70f;
      GameLocation currentLocation = Game1.currentLocation;
      int numAddOns = 4;
      int xRange = 64;
      int yRange = 64;
      Utility.makeTemporarySpriteJuicier(t, currentLocation, numAddOns, xRange, yRange);
      base.deathAnimation();
    }

    public override void draw(SpriteBatch b)
    {
      if (this.isInvisible || !Utility.isOnScreen(this.position, 2 * Game1.tileSize))
        return;
      b.Draw(Game1.shadowTexture, this.getLocalPosition(Game1.viewport) + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize * 2 / 3) + this.yOffset), new Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0.0f, new Vector2((float) Game1.shadowTexture.Bounds.Center.X, (float) Game1.shadowTexture.Bounds.Center.Y), (float) (3.5 + (double) this.scale + (double) this.yOffset / 30.0), SpriteEffects.None, (float) (this.getStandingY() - 1) / 10000f);
      b.Draw(this.Sprite.Texture, this.getLocalPosition(Game1.viewport) + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize * 3 / 4 + this.yJumpOffset)), new Rectangle?(this.Sprite.SourceRect), this.c, this.rotation, new Vector2(8f, 16f), Math.Max(0.2f, this.scale) * (float) Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.991f : (float) this.getStandingY() / 10000f));
    }

    public override void shedChunks(int number, float scale)
    {
      GameLocation currentLocation = Game1.currentLocation;
      Texture2D texture = this.sprite.Texture;
      Rectangle sourcerectangle = new Rectangle(0, this.sprite.getHeight() * 4, 16, 16);
      int sizeOfSourceRectSquares = 8;
      Rectangle boundingBox = this.GetBoundingBox();
      int x = boundingBox.Center.X;
      boundingBox = this.GetBoundingBox();
      int y1 = boundingBox.Center.Y;
      int numberOfChunks = number;
      int y2 = (int) this.getTileLocation().Y;
      Color white = Color.White;
      double num = (double) scale * (double) Game1.pixelZoom;
      Game1.createRadialDebris(currentLocation, texture, sourcerectangle, sizeOfSourceRectSquares, x, y1, numberOfChunks, y2, white, (float) num);
    }
  }
}
