// Decompiled with JetBrains decompiler
// Type: StardewValley.Monsters.RockCrab
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Tools;
using System;

namespace StardewValley.Monsters
{
  public class RockCrab : Monster
  {
    private int shellHealth = 5;
    private bool leftDrift;
    private bool shellGone;
    private bool waiter;

    public RockCrab()
    {
    }

    public RockCrab(Vector2 position)
      : base("Rock Crab", position)
    {
      this.waiter = Game1.random.NextDouble() < 0.4;
      int num = this.waiter ? 1 : 0;
      this.moveTowardPlayerThreshold = 3;
    }

    public override void reloadSprite()
    {
      base.reloadSprite();
      this.sprite.UpdateSourceRect();
    }

    public RockCrab(Vector2 position, string name)
      : base(name, position)
    {
      this.waiter = Game1.random.NextDouble() < 0.4;
      this.moveTowardPlayerThreshold = 3;
    }

    public override bool hitWithTool(Tool t)
    {
      base.hitWithTool(t);
      if (!(t is Pickaxe))
        return false;
      Game1.playSound("hammer");
      this.shellHealth = this.shellHealth - 1;
      this.shake(500);
      this.waiter = false;
      this.moveTowardPlayerThreshold = 3;
      this.setTrajectory(Utility.getAwayFromPlayerTrajectory(this.GetBoundingBox(), t.getLastFarmerToUse()));
      if (this.shellHealth <= 0)
      {
        this.shellGone = true;
        this.moveTowardPlayer(-1);
        Game1.playSound("stoneCrack");
        Game1.createRadialDebris(Game1.currentLocation, 14, this.getTileX(), this.getTileY(), Game1.random.Next(2, 7), false, -1, false, -1);
        Game1.createRadialDebris(Game1.currentLocation, 14, this.getTileX(), this.getTileY(), Game1.random.Next(2, 7), false, -1, false, -1);
      }
      return true;
    }

    public override void shedChunks(int number)
    {
      GameLocation currentLocation = Game1.currentLocation;
      Texture2D texture = this.sprite.Texture;
      Rectangle sourcerectangle = new Rectangle(0, 120, 16, 16);
      int sizeOfSourceRectSquares = 8;
      Rectangle boundingBox = this.GetBoundingBox();
      int x = boundingBox.Center.X;
      boundingBox = this.GetBoundingBox();
      int y1 = boundingBox.Center.Y;
      int numberOfChunks = number;
      int y2 = (int) this.getTileLocation().Y;
      Color white = Color.White;
      double num = 1.0 * (double) Game1.pixelZoom * (double) this.scale;
      Game1.createRadialDebris(currentLocation, texture, sourcerectangle, sizeOfSourceRectSquares, x, y1, numberOfChunks, y2, white, (float) num);
    }

    public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
    {
      int num = Math.Max(1, damage - this.resilience);
      if (isBomb)
      {
        this.shellGone = true;
        this.waiter = false;
        this.moveTowardPlayer(-1);
      }
      if (Game1.random.NextDouble() < this.missChance - this.missChance * addedPrecision)
        num = -1;
      else if (this.sprite.CurrentFrame % 4 == 0 && !this.shellGone)
      {
        num = 0;
        Game1.playSound("crafting");
      }
      else
      {
        this.health = this.health - num;
        this.slipperiness = 3;
        this.setTrajectory(xTrajectory, yTrajectory);
        Game1.playSound("hitEnemy");
        this.glowingColor = Color.Cyan;
        if (this.health <= 0)
        {
          Game1.playSound("monsterdead");
          this.deathAnimation();
          TemporaryAnimatedSprite t = new TemporaryAnimatedSprite(44, this.position, Color.Red, 10, false, 100f, 0, -1, -1f, -1, 0);
          t.holdLastFrame = true;
          t.alphaFade = 0.01f;
          GameLocation currentLocation = Game1.currentLocation;
          int numAddOns = 4;
          int xRange = 64;
          int yRange = 64;
          Utility.makeTemporarySpriteJuicier(t, currentLocation, numAddOns, xRange, yRange);
        }
      }
      return num;
    }

    public override void update(GameTime time, GameLocation location)
    {
      if (!location.Equals((object) Game1.currentLocation))
        return;
      if (!this.shellGone && !Game1.player.isRafting)
      {
        base.update(time, location);
      }
      else
      {
        if (Game1.player.isRafting)
          return;
        this.behaviorAtGameTick(time);
      }
    }

    public override void behaviorAtGameTick(GameTime time)
    {
      if (this.waiter && this.shellHealth > 4)
      {
        this.moveTowardPlayerThreshold = 0;
      }
      else
      {
        base.behaviorAtGameTick(time);
        if (this.isMoving() && this.sprite.CurrentFrame % 4 == 0)
        {
          ++this.sprite.CurrentFrame;
          this.sprite.UpdateSourceRect();
        }
        if (!this.withinPlayerThreshold() && !this.shellGone)
        {
          this.Halt();
        }
        else
        {
          if (!this.shellGone)
            return;
          this.updateGlow();
          if (this.invincibleCountdown > 0)
          {
            this.glowingColor = Color.Cyan;
            this.invincibleCountdown = this.invincibleCountdown - time.ElapsedGameTime.Milliseconds;
            if (this.invincibleCountdown <= 0)
              this.stopGlowing();
          }
          int y1 = Game1.player.GetBoundingBox().Center.Y;
          Rectangle boundingBox = this.GetBoundingBox();
          int y2 = boundingBox.Center.Y;
          if (Math.Abs(y1 - y2) > Game1.tileSize * 3)
          {
            boundingBox = Game1.player.GetBoundingBox();
            int x1 = boundingBox.Center.X;
            boundingBox = this.GetBoundingBox();
            int x2 = boundingBox.Center.X;
            if (x1 - x2 > 0)
              this.SetMovingLeft(true);
            else
              this.SetMovingRight(true);
          }
          else
          {
            boundingBox = Game1.player.GetBoundingBox();
            int y3 = boundingBox.Center.Y;
            boundingBox = this.GetBoundingBox();
            int y4 = boundingBox.Center.Y;
            if (y3 - y4 > 0)
              this.SetMovingUp(true);
            else
              this.SetMovingDown(true);
          }
          this.MovePosition(time, Game1.viewport, Game1.currentLocation);
          this.sprite.CurrentFrame = 16 + this.sprite.CurrentFrame % 4;
        }
      }
    }
  }
}
