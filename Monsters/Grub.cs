// Decompiled with JetBrains decompiler
// Type: StardewValley.Monsters.Grub
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley.Monsters
{
  public class Grub : Monster
  {
    private int metamorphCounter = 2000;
    public const int healthToRunAway = 8;
    private bool leftDrift;
    private bool pupating;
    public bool hard;

    public Grub()
    {
    }

    public Grub(Vector2 position, bool hard = false)
      : base(nameof (Grub), position)
    {
      if (Game1.random.NextDouble() < 0.5)
        this.leftDrift = true;
      this.facingDirection = Game1.random.Next(4);
      this.rotation = (float) Game1.random.Next(4) / 3.141593f;
      this.hard = hard;
      if (!hard)
        return;
      this.damageToFarmer = this.damageToFarmer * 3;
      this.health = this.health * 5;
      this.maxHealth = this.health;
      this.experienceGained = this.experienceGained * 3;
      if (Game1.random.NextDouble() >= 0.1)
        return;
      this.objectsToDrop.Add(456);
    }

    public override void reloadSprite()
    {
      base.reloadSprite();
      this.sprite.spriteHeight = 24;
      this.sprite.UpdateSourceRect();
    }

    public void setHard()
    {
      this.hard = true;
      if (!this.hard)
        return;
      this.damageToFarmer = 12;
      this.health = 100;
      this.maxHealth = this.health;
      this.experienceGained = 10;
      if (Game1.random.NextDouble() >= 0.1)
        return;
      this.objectsToDrop.Add(456);
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
        Game1.playSound("slimeHit");
        if (this.pupating)
        {
          Game1.playSound("crafting");
          this.setTrajectory(xTrajectory / 2, yTrajectory / 2);
          return 0;
        }
        this.slipperiness = 4;
        this.health = this.health - num;
        this.setTrajectory(xTrajectory, yTrajectory);
        if (this.health <= 0)
        {
          Game1.playSound("slimedead");
          TemporaryAnimatedSprite t = new TemporaryAnimatedSprite(44, this.position, Color.Orange, 10, false, 100f, 0, -1, -1f, -1, 0);
          t.holdLastFrame = true;
          t.alphaFade = 0.01f;
          t.interval = 50f;
          GameLocation currentLocation = Game1.currentLocation;
          int numAddOns = 4;
          int xRange = 64;
          int yRange = 64;
          Utility.makeTemporarySpriteJuicier(t, currentLocation, numAddOns, xRange, yRange);
        }
      }
      return num;
    }

    public override void defaultMovementBehavior(GameTime time)
    {
      this.scale = (float) (1.0 + (double) Game1.tileSize / 512.0 * Math.Sin(time.TotalGameTime.TotalMilliseconds / (500.0 + (double) this.position.X / 100.0)));
    }

    public override void update(GameTime time, GameLocation location)
    {
      if (!location.Equals((object) Game1.currentLocation))
        return;
      if ((this.health > 8 || this.hard && this.health >= this.maxHealth) && !this.pupating)
        base.update(time, location);
      else
        this.behaviorAtGameTick(time);
    }

    public override void draw(SpriteBatch b)
    {
      b.Draw(this.Sprite.Texture, this.getLocalPosition(Game1.viewport) + new Vector2((float) (this.sprite.spriteWidth * Game1.pixelZoom / 2), (float) (this.GetBoundingBox().Height / 2)) + (this.shakeTimer > 0 ? new Vector2((float) Game1.random.Next(-1, 2), (float) Game1.random.Next(-1, 2)) : Vector2.Zero), new Rectangle?(this.Sprite.SourceRect), this.hard ? Color.Lime : Color.White, this.rotation, new Vector2((float) (this.sprite.spriteWidth / 2), (float) ((double) this.sprite.spriteHeight * 3.0 / 4.0)), Math.Max(0.2f, this.scale) * (float) Game1.pixelZoom, this.flip || this.sprite.currentAnimation != null && this.sprite.currentAnimation[this.sprite.currentAnimationIndex].flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.991f : (float) this.getStandingY() / 10000f));
    }

    public override void behaviorAtGameTick(GameTime time)
    {
      base.behaviorAtGameTick(time);
      TimeSpan timeSpan;
      if (this.invincibleCountdown > 0)
      {
        int invincibleCountdown = this.invincibleCountdown;
        timeSpan = time.ElapsedGameTime;
        int milliseconds = timeSpan.Milliseconds;
        this.invincibleCountdown = invincibleCountdown - milliseconds;
        if (this.invincibleCountdown <= 0)
          this.stopGlowing();
      }
      if (this.pupating)
      {
        double num1 = 1.0;
        timeSpan = time.TotalGameTime;
        double num2 = Math.Sin((double) timeSpan.Milliseconds * 0.392699092626572) / 12.0;
        this.scale = (float) (num1 + num2);
        int metamorphCounter = this.metamorphCounter;
        timeSpan = time.ElapsedGameTime;
        int milliseconds = timeSpan.Milliseconds;
        this.metamorphCounter = metamorphCounter - milliseconds;
        if (this.metamorphCounter > 0)
          return;
        this.health = -500;
        Game1.createRadialDebris(Game1.currentLocation, this.sprite.Texture, new Rectangle(208, 424, 32, 40), 4, this.getStandingX(), this.getStandingY(), 25, (int) this.getTileLocation().Y);
        Game1.createRadialDebris(Game1.currentLocation, this.sprite.Texture, new Rectangle(208, 424, 32, 40), 8, this.getStandingX(), this.getStandingY(), 15, (int) this.getTileLocation().Y);
        Game1.currentLocation.characters.Add((NPC) new Fly(this.position, this.hard));
      }
      else if (this.health <= 8 || this.hard && this.health < this.maxHealth)
      {
        int metamorphCounter = this.metamorphCounter;
        timeSpan = time.ElapsedGameTime;
        int milliseconds = timeSpan.Milliseconds;
        this.metamorphCounter = metamorphCounter - milliseconds;
        if (this.metamorphCounter <= 0)
        {
          this.sprite.Animate(time, 16, 4, 125f);
          if (this.sprite.currentFrame != 19)
            return;
          this.pupating = true;
          this.metamorphCounter = 4500;
        }
        else
        {
          if (Math.Abs(Game1.player.GetBoundingBox().Center.Y - this.GetBoundingBox().Center.Y) > Game1.tileSize * 2)
          {
            if (Game1.player.GetBoundingBox().Center.X > this.GetBoundingBox().Center.X)
              this.SetMovingLeft(true);
            else
              this.SetMovingRight(true);
          }
          else if (Math.Abs(Game1.player.GetBoundingBox().Center.X - this.GetBoundingBox().Center.X) > Game1.tileSize * 2)
          {
            if (Game1.player.GetBoundingBox().Center.Y > this.GetBoundingBox().Center.Y)
              this.SetMovingUp(true);
            else
              this.SetMovingDown(true);
          }
          this.MovePosition(time, Game1.viewport, Game1.currentLocation);
        }
      }
      else if (this.withinPlayerThreshold())
      {
        this.scale = 1f;
        this.rotation = 0.0f;
      }
      else
      {
        if (!this.isMoving())
          return;
        this.Halt();
        this.faceDirection(Game1.random.Next(4));
        this.rotation = (float) Game1.random.Next(4) / 3.141593f;
      }
    }
  }
}
