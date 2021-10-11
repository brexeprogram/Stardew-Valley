// Decompiled with JetBrains decompiler
// Type: StardewValley.Monsters.Fly
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley.Monsters
{
  public class Fly : Monster
  {
    private int spawningCounter = 1000;
    public const float rotationIncrement = 0.04908739f;
    public const int volumeTileRange = 16;
    public const int spawnTime = 1000;
    private int wasHitCounter;
    private float targetRotation;
    public static Cue buzz;
    private bool turningRight;
    public bool hard;

    public Fly()
    {
    }

    public Fly(Vector2 position, bool hard = false)
      : base(nameof (Fly), position)
    {
      this.slipperiness = 24 + Game1.random.Next(-10, 10);
      this.Halt();
      this.IsWalkingTowardPlayer = false;
      this.hard = hard;
      if (hard)
      {
        this.damageToFarmer = this.damageToFarmer * 2;
        this.maxHealth = this.maxHealth * 3;
        this.health = this.maxHealth;
      }
      this.hideShadow = true;
    }

    public void setHard()
    {
      this.hard = true;
      if (!this.hard)
        return;
      this.damageToFarmer = 12;
      this.maxHealth = 66;
      this.health = this.maxHealth;
    }

    public override void reloadSprite()
    {
      this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Monsters\\Fly"));
      if (Game1.soundBank != null)
        Fly.buzz = Game1.soundBank.GetCue("flybuzzing");
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
        Game1.playSound("hitEnemy");
        if (this.health <= 0)
        {
          Game1.playSound("monsterdead");
          TemporaryAnimatedSprite t = new TemporaryAnimatedSprite(44, this.position, Color.HotPink, 10, false, 100f, 0, -1, -1f, -1, 0);
          t.interval = 70f;
          GameLocation currentLocation = Game1.currentLocation;
          int numAddOns = 4;
          int xRange = 64;
          int yRange = 64;
          Utility.makeTemporarySpriteJuicier(t, currentLocation, numAddOns, xRange, yRange);
          if (Game1.soundBank != null && Fly.buzz != null)
            Fly.buzz.Stop(AudioStopOptions.AsAuthored);
        }
      }
      this.addedSpeed = Game1.random.Next(-1, 1);
      return num;
    }

    public override void drawAboveAllLayers(SpriteBatch b)
    {
      if (!Utility.isOnScreen(this.position, 2 * Game1.tileSize))
        return;
      b.Draw(this.Sprite.Texture, this.getLocalPosition(Game1.viewport) + new Vector2((float) (Game1.tileSize / 2), (float) (this.GetBoundingBox().Height / 2 - Game1.tileSize / 2)), new Rectangle?(this.Sprite.SourceRect), this.hard ? Color.Lime : Color.White, this.rotation, new Vector2(8f, 16f), Math.Max(0.2f, this.scale) * (float) Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.991f : (float) (this.getStandingY() + 8) / 10000f));
      b.Draw(Game1.shadowTexture, this.getLocalPosition(Game1.viewport) + new Vector2((float) (Game1.tileSize / 2), (float) (this.GetBoundingBox().Height / 2)), new Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0.0f, new Vector2((float) Game1.shadowTexture.Bounds.Center.X, (float) Game1.shadowTexture.Bounds.Center.Y), (float) Game1.pixelZoom, SpriteEffects.None, (float) (this.getStandingY() - 1) / 10000f);
      if (!this.isGlowing)
        return;
      b.Draw(this.Sprite.Texture, this.getLocalPosition(Game1.viewport) + new Vector2((float) (Game1.tileSize / 2), (float) (this.GetBoundingBox().Height / 2 - Game1.tileSize / 2)), new Rectangle?(this.Sprite.SourceRect), this.glowingColor * this.glowingTransparency, this.rotation, new Vector2(8f, 16f), Math.Max(0.2f, this.scale) * (float) Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.99f : (float) ((double) this.getStandingY() / 10000.0 + 1.0 / 1000.0)));
    }

    public override void drawAboveAlwaysFrontLayer(SpriteBatch b)
    {
      if (!Game1.currentLocation.treatAsOutdoors)
        return;
      this.drawAboveAllLayers(b);
    }

    public override void behaviorAtGameTick(GameTime time)
    {
      base.behaviorAtGameTick(time);
      if (Game1.soundBank != null && (Fly.buzz == null || !Fly.buzz.IsPlaying))
      {
        Fly.buzz = Game1.soundBank.GetCue("flybuzzing");
        Fly.buzz.SetVariable("Volume", 0.0f);
        Fly.buzz.Play();
      }
      if ((double) Game1.fadeToBlackAlpha > 0.8 && Game1.fadeIn && Fly.buzz != null)
        Fly.buzz.Stop(AudioStopOptions.AsAuthored);
      else if (Fly.buzz != null)
      {
        Fly.buzz.SetVariable("Volume", Math.Max(0.0f, Fly.buzz.GetVariable("Volume") - 1f));
        float num = Math.Max(0.0f, (float) (100.0 - (double) Vector2.Distance(this.position, Game1.player.position) / (double) Game1.tileSize / 16.0 * 100.0));
        if ((double) num > (double) Fly.buzz.GetVariable("Volume"))
          Fly.buzz.SetVariable("Volume", num);
      }
      TimeSpan elapsedGameTime;
      if (this.wasHitCounter >= 0)
      {
        int wasHitCounter = this.wasHitCounter;
        elapsedGameTime = time.ElapsedGameTime;
        int milliseconds = elapsedGameTime.Milliseconds;
        this.wasHitCounter = wasHitCounter - milliseconds;
      }
      if (double.IsNaN((double) this.xVelocity) || double.IsNaN((double) this.yVelocity))
        this.health = -500;
      this.sprite.Animate(time, this.facingDirection == 0 ? 8 : (this.facingDirection == 2 ? 0 : this.facingDirection * 4), 4, 75f);
      if ((double) this.position.X <= -640.0 || (double) this.position.Y <= -640.0 || ((double) this.position.X >= (double) (Game1.currentLocation.Map.Layers[0].LayerWidth * Game1.tileSize + 640) || (double) this.position.Y >= (double) (Game1.currentLocation.Map.Layers[0].LayerHeight * Game1.tileSize + 640)))
        this.health = -500;
      if (this.spawningCounter >= 0)
      {
        int spawningCounter = this.spawningCounter;
        elapsedGameTime = time.ElapsedGameTime;
        int milliseconds = elapsedGameTime.Milliseconds;
        this.spawningCounter = spawningCounter - milliseconds;
        this.scale = (float) (1.0 - (double) this.spawningCounter / 1000.0);
      }
      else
      {
        if (!this.withinPlayerThreshold() && !Utility.isOnScreen(this.position, Game1.tileSize * 4) || this.invincibleCountdown > 0)
          return;
        this.faceDirection(0);
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
}
