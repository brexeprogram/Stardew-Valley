// Decompiled with JetBrains decompiler
// Type: StardewValley.Monsters.Ghost
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using xTile.Dimensions;

namespace StardewValley.Monsters
{
  public class Ghost : Monster
  {
    private int identifier = Game1.random.Next(-99999, 99999);
    public const float rotationIncrement = 0.04908739f;
    private int wasHitCounter;
    private float targetRotation;
    private bool turningRight;
    private bool seenPlayer;
    private int yOffset;
    private int yOffsetExtra;

    public Ghost()
    {
    }

    public Ghost(Vector2 position)
      : base(nameof (Ghost), position)
    {
      this.slipperiness = 8;
      this.isGlider = true;
    }

    public override void reloadSprite()
    {
      this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Monsters\\Ghost"));
    }

    public override void drawAboveAllLayers(SpriteBatch b)
    {
      b.Draw(this.Sprite.Texture, this.getLocalPosition(Game1.viewport) + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 3 + this.yOffset)), new Microsoft.Xna.Framework.Rectangle?(this.Sprite.SourceRect), Color.White, 0.0f, new Vector2(8f, 16f), Math.Max(0.2f, this.scale) * (float) Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.991f : (float) this.getStandingY() / 10000f));
      SpriteBatch spriteBatch = b;
      Texture2D shadowTexture = Game1.shadowTexture;
      Vector2 position = this.getLocalPosition(Game1.viewport) + new Vector2((float) (Game1.tileSize / 2), (float) Game1.tileSize);
      Microsoft.Xna.Framework.Rectangle? sourceRectangle = new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds);
      Color white = Color.White;
      double num1 = 0.0;
      Microsoft.Xna.Framework.Rectangle bounds = Game1.shadowTexture.Bounds;
      double x = (double) bounds.Center.X;
      bounds = Game1.shadowTexture.Bounds;
      double y = (double) bounds.Center.Y;
      Vector2 origin = new Vector2((float) x, (float) y);
      double num2 = 3.0 + (double) this.yOffset / 20.0;
      int num3 = 0;
      double num4 = (double) (this.getStandingY() - 1) / 10000.0;
      spriteBatch.Draw(shadowTexture, position, sourceRectangle, white, (float) num1, origin, (float) num2, (SpriteEffects) num3, (float) num4);
    }

    public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
    {
      int num = Math.Max(1, damage - this.resilience);
      this.slipperiness = 8;
      Utility.addSprinklesToLocation(Game1.currentLocation, this.getTileX(), this.getTileY(), 2, 2, 101, 50, Color.LightBlue, (string) null, false);
      if (Game1.random.NextDouble() < this.missChance - this.missChance * addedPrecision)
      {
        num = -1;
      }
      else
      {
        if (Game1.player.CurrentTool != null && Game1.player.CurrentTool.Name.Equals("Holy Sword") && !isBomb)
        {
          this.health = this.health - damage * 3 / 4;
          Game1.currentLocation.debris.Add(new Debris(string.Concat((object) (damage * 3 / 4)), 1, new Vector2((float) this.getStandingX(), (float) this.getStandingY()), Color.LightBlue, 1f, 0.0f));
        }
        this.health = this.health - num;
        if (this.health <= 0)
          this.deathAnimation();
        this.setTrajectory(xTrajectory, yTrajectory);
      }
      this.addedSpeed = -1;
      Utility.removeLightSource(this.identifier);
      return num;
    }

    public override void deathAnimation()
    {
      Game1.playSound("ghost");
      Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(this.sprite.Texture, new Microsoft.Xna.Framework.Rectangle(0, 96, 16, 24), 100f, 4, 0, this.position, false, false, 0.9f, 1f / 1000f, Color.White, (float) Game1.pixelZoom, 0.01f, 0.0f, (float) Math.PI / 64f, false));
    }

    public override void behaviorAtGameTick(GameTime time)
    {
      base.behaviorAtGameTick(time);
      this.yOffset = (int) (Math.Sin((double) time.TotalGameTime.Milliseconds / 1000.0 * (2.0 * Math.PI)) * 20.0) - this.yOffsetExtra;
      bool flag = false;
      foreach (LightSource currentLightSource in Game1.currentLightSources)
      {
        if (currentLightSource.identifier == this.identifier)
        {
          currentLightSource.position = new Vector2(this.position.X + (float) (Game1.tileSize / 2), this.position.Y + (float) Game1.tileSize + (float) this.yOffset);
          flag = true;
        }
      }
      if (!flag)
        Game1.currentLightSources.Add(new LightSource(5, new Vector2(this.position.X + 8f, this.position.Y + (float) Game1.tileSize), 1f, Color.White * 0.7f, this.identifier));
      Microsoft.Xna.Framework.Rectangle boundingBox1 = Game1.player.GetBoundingBox();
      int x1 = boundingBox1.Center.X;
      boundingBox1 = this.GetBoundingBox();
      int x2 = boundingBox1.Center.X;
      float num1 = (float) -(x1 - x2);
      boundingBox1 = Game1.player.GetBoundingBox();
      float num2 = (float) (boundingBox1.Center.Y - this.GetBoundingBox().Center.Y);
      float num3 = 400f;
      float num4 = num1 / num3;
      float num5 = num2 / num3;
      if (this.wasHitCounter <= 0)
      {
        this.targetRotation = (float) Math.Atan2(-(double) num5, (double) num4) - 1.570796f;
        if ((double) Math.Abs(this.targetRotation) - (double) Math.Abs(this.rotation) > 7.0 * Math.PI / 8.0 && Game1.random.NextDouble() < 0.5)
          this.turningRight = true;
        else if ((double) Math.Abs(this.targetRotation) - (double) Math.Abs(this.rotation) < Math.PI / 8.0)
          this.turningRight = false;
        if (this.turningRight)
          this.rotation = this.rotation - (float) Math.Sign(this.targetRotation - this.rotation) * ((float) Math.PI / 64f);
        else
          this.rotation = this.rotation + (float) Math.Sign(this.targetRotation - this.rotation) * ((float) Math.PI / 64f);
        this.rotation = this.rotation % 6.283185f;
        this.wasHitCounter = 0;
      }
      float num6 = Math.Min(4f, Math.Max(1f, (float) (5.0 - (double) num3 / (double) Game1.tileSize / 2.0)));
      float num7 = (float) Math.Cos((double) this.rotation + Math.PI / 2.0);
      float num8 = -(float) Math.Sin((double) this.rotation + Math.PI / 2.0);
      this.xVelocity = this.xVelocity + (float) (-(double) num7 * (double) num6 / 6.0 + (double) Game1.random.Next(-10, 10) / 100.0);
      this.yVelocity = this.yVelocity + (float) (-(double) num8 * (double) num6 / 6.0 + (double) Game1.random.Next(-10, 10) / 100.0);
      if ((double) Math.Abs(this.xVelocity) > (double) Math.Abs((float) (-(double) num7 * 5.0)))
        this.xVelocity = this.xVelocity - (float) (-(double) num7 * (double) num6 / 6.0);
      if ((double) Math.Abs(this.yVelocity) > (double) Math.Abs((float) (-(double) num8 * 5.0)))
        this.yVelocity = this.yVelocity - (float) (-(double) num8 * (double) num6 / 6.0);
      this.faceGeneralDirection(Game1.player.getStandingPosition(), 0);
      Microsoft.Xna.Framework.Rectangle boundingBox2 = this.GetBoundingBox();
      if (!boundingBox2.Intersects(Game1.player.GetBoundingBox()))
        return;
      int num9 = 0;
      Vector2 vector2;
      // ISSUE: explicit reference operation
      // ISSUE: variable of a reference type
      Vector2& local1 = @vector2;
      boundingBox2 = Game1.player.GetBoundingBox();
      double num10 = (double) (boundingBox2.Center.X / Game1.tileSize + Game1.random.Next(-12, 12));
      boundingBox2 = Game1.player.GetBoundingBox();
      double num11 = (double) (boundingBox2.Center.Y / Game1.tileSize + Game1.random.Next(-12, 12));
      // ISSUE: explicit reference operation
      ^local1 = new Vector2((float) num10, (float) num11);
      for (; num9 < 3 && ((double) vector2.X >= (double) Game1.currentLocation.map.GetLayer("Back").LayerWidth || (double) vector2.Y >= (double) Game1.currentLocation.map.GetLayer("Back").LayerHeight || ((double) vector2.X < 0.0 || (double) vector2.Y < 0.0) || (Game1.currentLocation.map.GetLayer("Back").Tiles[(int) vector2.X, (int) vector2.Y] == null || !Game1.currentLocation.isTilePassable(new Location((int) vector2.X, (int) vector2.Y), Game1.viewport) || vector2.Equals(new Vector2((float) (Game1.player.getStandingX() / Game1.tileSize), (float) (Game1.player.getStandingY() / Game1.tileSize))))); ++num9)
      {
        // ISSUE: explicit reference operation
        // ISSUE: variable of a reference type
        Vector2& local2 = @vector2;
        boundingBox2 = Game1.player.GetBoundingBox();
        double num12 = (double) (boundingBox2.Center.X / Game1.tileSize + Game1.random.Next(-12, 12));
        boundingBox2 = Game1.player.GetBoundingBox();
        double num13 = (double) (boundingBox2.Center.Y / Game1.tileSize + Game1.random.Next(-12, 12));
        // ISSUE: explicit reference operation
        ^local2 = new Vector2((float) num12, (float) num13);
      }
      if (num9 >= 3)
        return;
      this.position = new Vector2(vector2.X * (float) Game1.tileSize, vector2.Y * (float) Game1.tileSize - (float) (Game1.tileSize / 2));
      this.Halt();
    }
  }
}
