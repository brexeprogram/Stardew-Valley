// Decompiled with JetBrains decompiler
// Type: StardewValley.Monsters.Cat
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using System;

namespace StardewValley.Monsters
{
  public class Cat : Monster
  {
    public int currentBehavior = -1;
    public const int lick = 0;
    public const int pounce = 1;
    public const int nap = 2;
    public int timesOnCurrentAnimation;
    public string kittyName;
    public bool wasPet;
    public bool madeSnoozeSoundLastFrame;
    public int loveForMaster;

    public Cat()
    {
    }

    public Cat(Vector2 position, string myName)
      : base(nameof (Cat), position)
    {
      this.IsWalkingTowardPlayer = false;
      this.sprite.spriteWidth = Game1.tileSize;
      this.sprite.spriteHeight = Game1.tileSize;
      this.sprite.UpdateSourceRect();
      this.kittyName = myName;
    }

    public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
    {
      int num = Math.Max(1, damage - this.resilience);
      this.health = this.health - num;
      this.doEmote(12, true);
      return num;
    }

    public override void behaviorAtGameTick(GameTime time)
    {
      base.behaviorAtGameTick(time);
      if (this.isEmoting && this.currentEmote == 20)
      {
        this.Halt();
        this.sprite.CurrentFrame = 22;
        this.sprite.UpdateSourceRect();
        this.currentBehavior = -1;
      }
      else
      {
        if (this.currentBehavior == -1 && Game1.random.NextDouble() < 0.0005)
        {
          Game1.playSound("catHurt");
          this.currentBehavior = -2;
        }
        if (Game1.timeOfDay == 600)
          this.currentBehavior = 2;
        if (this.currentBehavior == 0)
        {
          int currentFrame = this.sprite.currentFrame;
          this.sprite.Animate(time, 20, 2, 400f);
          if (this.sprite.currentFrame != currentFrame)
          {
            this.timesOnCurrentAnimation = this.timesOnCurrentAnimation + 1;
            if (this.timesOnCurrentAnimation > 4 && Game1.random.NextDouble() < 0.2)
            {
              this.timesOnCurrentAnimation = 0;
              this.Halt();
              this.currentBehavior = -1;
            }
          }
        }
        else if (this.currentBehavior == 1)
        {
          int currentFrame = this.sprite.currentFrame;
          this.sprite.Animate(time, 16, 2, 200f);
          if (this.sprite.currentFrame != currentFrame)
          {
            this.timesOnCurrentAnimation = this.timesOnCurrentAnimation + 1;
            if (this.timesOnCurrentAnimation > 4 && Game1.random.NextDouble() < 0.2)
            {
              this.timesOnCurrentAnimation = 0;
              this.setTrajectory(0, -Game1.random.Next(16, 24));
              this.Halt();
              this.currentBehavior = -1;
            }
          }
        }
        else if (this.currentBehavior == 2)
        {
          int currentFrame = this.sprite.currentFrame;
          this.sprite.Animate(time, 18, 2, 1000f);
          if (this.sprite.currentFrame != currentFrame && Game1.random.NextDouble() < 0.02 || ((double) this.xVelocity != 0.0 || (double) this.yVelocity != 0.0))
          {
            this.Halt();
            this.currentBehavior = -1;
          }
          else if (this.sprite.CurrentFrame != currentFrame && this.sprite.CurrentFrame == 18 && (double) Game1.debrisSoundInterval <= 0.0 && Game1.random.NextDouble() < 0.4 + (this.madeSnoozeSoundLastFrame ? 0.4 : -0.1))
          {
            Game1.playSound("breathin");
            Game1.debrisSoundInterval = 999f - this.sprite.timer;
            this.madeSnoozeSoundLastFrame = true;
          }
          else if (this.sprite.CurrentFrame != currentFrame && this.sprite.CurrentFrame == 19 && (double) Game1.debrisSoundInterval <= 0.0 && Game1.random.NextDouble() < 0.4 + (this.madeSnoozeSoundLastFrame ? 0.4 : -0.1))
          {
            Game1.playSound("breathout");
            this.madeSnoozeSoundLastFrame = true;
            Game1.debrisSoundInterval = 999f - this.sprite.timer;
          }
          else
            this.madeSnoozeSoundLastFrame = false;
        }
        else if (Game1.random.NextDouble() < 0.01)
        {
          switch (Game1.random.Next(12))
          {
            case 0:
              this.SetMovingOnlyUp();
              break;
            case 1:
              this.SetMovingOnlyRight();
              break;
            case 2:
              this.SetMovingOnlyDown();
              break;
            case 3:
              this.SetMovingOnlyLeft();
              break;
            case 4:
              this.currentBehavior = 0;
              break;
            case 5:
              this.currentBehavior = 1;
              break;
            case 6:
              this.currentBehavior = 2;
              break;
            default:
              this.Halt();
              break;
          }
        }
        if (Game1.player.ActiveObject != null && (Game1.player.ActiveObject.Category == -6 || Game1.player.ActiveObject.ParentSheetIndex >= 129 && Game1.player.ActiveObject.ParentSheetIndex <= 147))
          this.IsWalkingTowardPlayer = true;
        else
          this.IsWalkingTowardPlayer = false;
        this.MovePosition(time, Game1.viewport, Game1.currentLocation);
      }
    }
  }
}
