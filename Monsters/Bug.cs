// Decompiled with JetBrains decompiler
// Type: StardewValley.Monsters.Bug
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley.Monsters
{
  public class Bug : Monster
  {
    public bool isArmoredBug;

    public Bug()
    {
    }

    public Bug(Vector2 position, int facingDirection)
      : base(nameof (Bug), position, facingDirection)
    {
      this.sprite.spriteHeight = 16;
      this.sprite.UpdateSourceRect();
      this.onCollision = new Monster.collisionBehavior(this.collide);
      this.yOffset = (float) (-Game1.tileSize / 2);
      this.IsWalkingTowardPlayer = false;
      this.setMovingInFacingDirection();
      this.defaultAnimationInterval = 40;
      this.collidesWithOtherCharacters = false;
      if (Game1.mine.getMineArea(-1) != 121)
        return;
      this.isArmoredBug = true;
      this.sprite.Texture = Game1.content.Load<Texture2D>("Characters\\Monsters\\Armored Bug");
      this.damageToFarmer = this.damageToFarmer * 2;
      this.slipperiness = -1;
      this.health = 150;
    }

    public override bool passThroughCharacters()
    {
      return true;
    }

    public override void reloadSprite()
    {
      base.reloadSprite();
      this.sprite.spriteHeight = 16;
      this.sprite.UpdateSourceRect();
    }

    private void collide(GameLocation location)
    {
      Rectangle rectangle = this.nextPosition(this.facingDirection);
      foreach (Character farmer in location.getFarmers())
      {
        if (farmer.GetBoundingBox().Intersects(rectangle))
          return;
      }
      this.facingDirection = (this.facingDirection + 2) % 4;
      this.setMovingInFacingDirection();
    }

    public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
    {
      int num = Math.Max(1, damage - this.resilience);
      if (this.isArmoredBug)
      {
        Game1.playSound("crafting");
        return 0;
      }
      if (Game1.random.NextDouble() < this.missChance - this.missChance * addedPrecision)
      {
        num = -1;
      }
      else
      {
        this.health = this.health - num;
        Game1.playSound("hitEnemy");
        this.setTrajectory(xTrajectory / 3, yTrajectory / 3);
        if (this.health <= 0)
          this.deathAnimation();
      }
      return num;
    }

    public override void draw(SpriteBatch b)
    {
      if (this.isInvisible || !Utility.isOnScreen(this.position, 2 * Game1.tileSize))
        return;
      Vector2 vector2 = new Vector2();
      if (this.facingDirection % 2 == 0)
        vector2.X = (float) (Math.Sin((double) Game1.currentGameTime.TotalGameTime.Milliseconds / 1000.0 * (2.0 * Math.PI)) * 10.0);
      else
        vector2.Y = (float) (Math.Sin((double) Game1.currentGameTime.TotalGameTime.Milliseconds / 1000.0 * (2.0 * Math.PI)) * 10.0);
      SpriteBatch spriteBatch = b;
      Texture2D shadowTexture = Game1.shadowTexture;
      Vector2 position = this.getLocalPosition(Game1.viewport) + new Vector2((float) (this.sprite.spriteWidth * Game1.pixelZoom) / 2f + vector2.X, (float) (this.GetBoundingBox().Height * 5 / 2 - Game1.tileSize * 3 / 4));
      Rectangle? sourceRectangle = new Rectangle?(Game1.shadowTexture.Bounds);
      Color white = Color.White;
      double num1 = 0.0;
      Rectangle bounds = Game1.shadowTexture.Bounds;
      double x = (double) bounds.Center.X;
      bounds = Game1.shadowTexture.Bounds;
      double y = (double) bounds.Center.Y;
      Vector2 origin = new Vector2((float) x, (float) y);
      double num2 = ((double) Game1.pixelZoom + (double) this.yJumpOffset / 40.0) * (double) this.scale;
      int num3 = 0;
      double num4 = (double) Math.Max(0.0f, (float) this.getStandingY() / 10000f) - 9.99999997475243E-07;
      spriteBatch.Draw(shadowTexture, position, sourceRectangle, white, (float) num1, origin, (float) num2, (SpriteEffects) num3, (float) num4);
      b.Draw(this.Sprite.Texture, this.getLocalPosition(Game1.viewport) + new Vector2((float) (Game1.tileSize / 2), (float) this.yJumpOffset) + vector2, new Rectangle?(this.Sprite.SourceRect), Color.White, this.rotation, new Vector2(8f, 16f), (float) Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.991f : (float) this.getStandingY() / 10000f));
    }

    public override void deathAnimation()
    {
      base.deathAnimation();
      Game1.playSound("slimedead");
      TemporaryAnimatedSprite t = new TemporaryAnimatedSprite(44, this.position + new Vector2(0.0f, (float) (-Game1.tileSize / 2)), Color.Violet, 10, false, 100f, 0, -1, -1f, -1, 0);
      t.holdLastFrame = true;
      t.alphaFade = 0.01f;
      t.interval = 70f;
      GameLocation currentLocation = Game1.currentLocation;
      int numAddOns = 4;
      int xRange = 64;
      int yRange = 64;
      Utility.makeTemporarySpriteJuicier(t, currentLocation, numAddOns, xRange, yRange);
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
      double pixelZoom = (double) Game1.pixelZoom;
      Game1.createRadialDebris(currentLocation, texture, sourcerectangle, sizeOfSourceRectSquares, x, y1, numberOfChunks, y2, white, (float) pixelZoom);
    }
  }
}
