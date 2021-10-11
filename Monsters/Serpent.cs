// Decompiled with JetBrains decompiler
// Type: StardewValley.Monsters.Serpent
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StardewValley.Monsters
{
  public class Serpent : Monster
  {
    public const float rotationIncrement = 0.04908739f;
    private int wasHitCounter;
    private float targetRotation;
    private bool turningRight;

    public Serpent()
    {
    }

    public Serpent(Vector2 position)
      : base(nameof (Serpent), position)
    {
      this.slipperiness = 24 + Game1.random.Next(10);
      this.Halt();
      this.IsWalkingTowardPlayer = false;
      this.sprite.spriteWidth = 32;
      this.sprite.spriteHeight = 32;
      this.scale = 0.75f;
      this.hideShadow = true;
    }

    public override void reloadSprite()
    {
      this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Monsters\\Serpent"));
      this.sprite.spriteWidth = 32;
      this.sprite.spriteHeight = 32;
      this.scale = 0.75f;
      this.hideShadow = true;
    }

    public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
    {
      int num = Math.Max(1, damage - this.resilience);
      if (Game1.random.NextDouble() < this.missChance - this.missChance * addedPrecision)
      {
        num = -1;
      }
      else
      {
        this.health = this.health - num;
        this.setTrajectory(xTrajectory / 3, yTrajectory / 3);
        this.wasHitCounter = 500;
        Game1.playSound("serpentHit");
        if (this.health <= 0)
        {
          Rectangle boundingBox = this.GetBoundingBox();
          boundingBox.Inflate(-boundingBox.Width / 2 + 1, -boundingBox.Height / 2 + 1);
          Vector2 velocityTowardPlayer = Utility.getVelocityTowardPlayer(boundingBox.Center, 4f, Game1.player);
          this.deathAnimation(-(int) velocityTowardPlayer.X, -(int) velocityTowardPlayer.Y);
        }
      }
      this.addedSpeed = Game1.random.Next(-1, 1);
      return num;
    }

    public void deathAnimation(int xTrajectory, int yTrajectory)
    {
      Game1.playSound("serpentDie");
      Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(this.sprite.Texture, new Rectangle(0, 64, 32, 32), 200f, 4, 0, this.position, false, false, 0.9f, 1f / 1000f, Color.White, (float) Game1.pixelZoom * this.scale, 0.01f, this.rotation + 3.141593f, (float) ((double) Game1.random.Next(3, 5) * Math.PI / 64.0), false)
      {
        motion = new Vector2((float) xTrajectory, (float) yTrajectory),
        layerDepth = 1f
      });
      List<TemporaryAnimatedSprite> temporarySprites1 = Game1.currentLocation.temporarySprites;
      TemporaryAnimatedSprite temporaryAnimatedSprite1 = new TemporaryAnimatedSprite(5, Utility.PointToVector2(this.GetBoundingBox().Center) + new Vector2((float) (-Game1.tileSize / 2), 0.0f), Color.LightGreen * 0.9f, 10, false, 70f, 0, -1, -1f, -1, 0);
      temporaryAnimatedSprite1.delayBeforeAnimationStart = 50;
      temporaryAnimatedSprite1.startSound = "cowboy_monsterhit";
      Vector2 vector2_1 = new Vector2((float) xTrajectory, (float) yTrajectory);
      temporaryAnimatedSprite1.motion = vector2_1;
      double num1 = 1.0;
      temporaryAnimatedSprite1.layerDepth = (float) num1;
      temporarySprites1.Add(temporaryAnimatedSprite1);
      List<TemporaryAnimatedSprite> temporarySprites2 = Game1.currentLocation.temporarySprites;
      TemporaryAnimatedSprite temporaryAnimatedSprite2 = new TemporaryAnimatedSprite(5, Utility.PointToVector2(this.GetBoundingBox().Center) + new Vector2((float) (Game1.tileSize / 2), 0.0f), Color.LightGreen * 0.8f, 10, false, 70f, 0, -1, -1f, -1, 0);
      temporaryAnimatedSprite2.delayBeforeAnimationStart = 100;
      temporaryAnimatedSprite2.startSound = "cowboy_monsterhit";
      Vector2 vector2_2 = new Vector2((float) xTrajectory, (float) yTrajectory) * 0.8f;
      temporaryAnimatedSprite2.motion = vector2_2;
      double num2 = 1.0;
      temporaryAnimatedSprite2.layerDepth = (float) num2;
      temporarySprites2.Add(temporaryAnimatedSprite2);
      List<TemporaryAnimatedSprite> temporarySprites3 = Game1.currentLocation.temporarySprites;
      TemporaryAnimatedSprite temporaryAnimatedSprite3 = new TemporaryAnimatedSprite(5, Utility.PointToVector2(this.GetBoundingBox().Center) + new Vector2(0.0f, (float) (-Game1.tileSize / 2)), Color.LightGreen * 0.7f, 10, false, 100f, 0, -1, -1f, -1, 0);
      temporaryAnimatedSprite3.delayBeforeAnimationStart = 150;
      temporaryAnimatedSprite3.startSound = "cowboy_monsterhit";
      Vector2 vector2_3 = new Vector2((float) xTrajectory, (float) yTrajectory) * 0.6f;
      temporaryAnimatedSprite3.motion = vector2_3;
      double num3 = 1.0;
      temporaryAnimatedSprite3.layerDepth = (float) num3;
      temporarySprites3.Add(temporaryAnimatedSprite3);
      List<TemporaryAnimatedSprite> temporarySprites4 = Game1.currentLocation.temporarySprites;
      TemporaryAnimatedSprite temporaryAnimatedSprite4 = new TemporaryAnimatedSprite(5, Utility.PointToVector2(this.GetBoundingBox().Center), Color.LightGreen * 0.6f, 10, false, 70f, 0, -1, -1f, -1, 0);
      temporaryAnimatedSprite4.delayBeforeAnimationStart = 200;
      temporaryAnimatedSprite4.startSound = "cowboy_monsterhit";
      Vector2 vector2_4 = new Vector2((float) xTrajectory, (float) yTrajectory) * 0.4f;
      temporaryAnimatedSprite4.motion = vector2_4;
      double num4 = 1.0;
      temporaryAnimatedSprite4.layerDepth = (float) num4;
      temporarySprites4.Add(temporaryAnimatedSprite4);
      List<TemporaryAnimatedSprite> temporarySprites5 = Game1.currentLocation.temporarySprites;
      TemporaryAnimatedSprite temporaryAnimatedSprite5 = new TemporaryAnimatedSprite(5, Utility.PointToVector2(this.GetBoundingBox().Center) + new Vector2(0.0f, (float) (Game1.tileSize / 2)), Color.LightGreen * 0.5f, 10, false, 100f, 0, -1, -1f, -1, 0);
      temporaryAnimatedSprite5.delayBeforeAnimationStart = 250;
      temporaryAnimatedSprite5.startSound = "cowboy_monsterhit";
      Vector2 vector2_5 = new Vector2((float) xTrajectory, (float) yTrajectory) * 0.2f;
      temporaryAnimatedSprite5.motion = vector2_5;
      double num5 = 1.0;
      temporaryAnimatedSprite5.layerDepth = (float) num5;
      temporarySprites5.Add(temporaryAnimatedSprite5);
    }

    public override void drawAboveAllLayers(SpriteBatch b)
    {
      if (!Utility.isOnScreen(this.position, 2 * Game1.tileSize))
        return;
      SpriteBatch spriteBatch = b;
      Texture2D shadowTexture = Game1.shadowTexture;
      Vector2 position = this.getLocalPosition(Game1.viewport) + new Vector2((float) Game1.tileSize, (float) this.GetBoundingBox().Height);
      Rectangle? sourceRectangle = new Rectangle?(Game1.shadowTexture.Bounds);
      Color white = Color.White;
      double num1 = 0.0;
      Rectangle bounds = Game1.shadowTexture.Bounds;
      double x = (double) bounds.Center.X;
      bounds = Game1.shadowTexture.Bounds;
      double y = (double) bounds.Center.Y;
      Vector2 origin = new Vector2((float) x, (float) y);
      double pixelZoom = (double) Game1.pixelZoom;
      int num2 = 0;
      double num3 = (double) (this.getStandingY() - 1) / 10000.0;
      spriteBatch.Draw(shadowTexture, position, sourceRectangle, white, (float) num1, origin, (float) pixelZoom, (SpriteEffects) num2, (float) num3);
      b.Draw(this.Sprite.Texture, this.getLocalPosition(Game1.viewport) + new Vector2((float) Game1.tileSize, (float) (this.GetBoundingBox().Height / 2)), new Rectangle?(this.Sprite.SourceRect), Color.White, this.rotation, new Vector2(16f, 16f), Math.Max(0.2f, this.scale) * (float) Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.991f : (float) (this.getStandingY() + 8) / 10000f));
      if (!this.isGlowing)
        return;
      b.Draw(this.Sprite.Texture, this.getLocalPosition(Game1.viewport) + new Vector2((float) Game1.tileSize, (float) (this.GetBoundingBox().Height / 2)), new Rectangle?(this.Sprite.SourceRect), this.glowingColor * this.glowingTransparency, this.rotation, new Vector2(16f, 16f), Math.Max(0.2f, this.scale) * (float) Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.991f : (float) ((double) (this.getStandingY() + 8) / 10000.0 + 9.99999974737875E-05)));
    }

    public override Rectangle GetBoundingBox()
    {
      return new Rectangle((int) this.position.X + Game1.tileSize / 8, (int) this.position.Y, this.sprite.spriteWidth * Game1.pixelZoom * 3 / 4, Game1.tileSize * 2 * 3 / 4);
    }

    public override void behaviorAtGameTick(GameTime time)
    {
      base.behaviorAtGameTick(time);
      if (this.wasHitCounter >= 0)
        this.wasHitCounter = this.wasHitCounter - time.ElapsedGameTime.Milliseconds;
      if (double.IsNaN((double) this.xVelocity) || double.IsNaN((double) this.yVelocity))
        this.health = -500;
      if ((double) this.position.X <= -640.0 || (double) this.position.Y <= -640.0 || ((double) this.position.X >= (double) (Game1.currentLocation.Map.Layers[0].LayerWidth * Game1.tileSize + 640) || (double) this.position.Y >= (double) (Game1.currentLocation.Map.Layers[0].LayerHeight * Game1.tileSize + 640)))
        this.health = -500;
      this.sprite.Animate(time, 0, 9, 40f);
      if (!this.withinPlayerThreshold() || this.invincibleCountdown > 0)
        return;
      this.faceDirection(2);
      Rectangle boundingBox = Game1.player.GetBoundingBox();
      int x1 = boundingBox.Center.X;
      boundingBox = this.GetBoundingBox();
      int x2 = boundingBox.Center.X;
      float num1 = (float) -(x1 - x2);
      boundingBox = Game1.player.GetBoundingBox();
      int y1 = boundingBox.Center.Y;
      boundingBox = this.GetBoundingBox();
      int y2 = boundingBox.Center.Y;
      float num2 = (float) (y1 - y2);
      float num3 = Math.Max(1f, Math.Abs(num1) + Math.Abs(num2));
      if ((double) num3 < (double) Game1.tileSize)
      {
        this.xVelocity = Math.Max(-7f, Math.Min(7f, this.xVelocity * 1.1f));
        this.yVelocity = Math.Max(-7f, Math.Min(7f, this.yVelocity * 1.1f));
      }
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
        this.wasHitCounter = 5 + Game1.random.Next(-1, 2);
      }
      float num6 = Math.Min(7f, Math.Max(2f, (float) (7.0 - (double) num3 / (double) Game1.tileSize / 2.0)));
      float num7 = (float) Math.Cos((double) this.rotation + Math.PI / 2.0);
      float num8 = -(float) Math.Sin((double) this.rotation + Math.PI / 2.0);
      this.xVelocity = this.xVelocity + (float) (-(double) num7 * (double) num6 / 6.0 + (double) Game1.random.Next(-10, 10) / 100.0);
      this.yVelocity = this.yVelocity + (float) (-(double) num8 * (double) num6 / 6.0 + (double) Game1.random.Next(-10, 10) / 100.0);
      if ((double) Math.Abs(this.xVelocity) > (double) Math.Abs((float) (-(double) num7 * 7.0)))
        this.xVelocity = this.xVelocity - (float) (-(double) num7 * (double) num6 / 6.0);
      if ((double) Math.Abs(this.yVelocity) <= (double) Math.Abs((float) (-(double) num8 * 7.0)))
        return;
      this.yVelocity = this.yVelocity - (float) (-(double) num8 * (double) num6 / 6.0);
    }
  }
}
