// Decompiled with JetBrains decompiler
// Type: StardewValley.Monsters.Crow
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using StardewValley.TerrainFeatures;
using System;

namespace StardewValley.Monsters
{
  public class Crow : Monster
  {
    private float flightRise = 0.1f;
    private bool startedFlying;
    private bool flyLeft;

    public Crow()
    {
    }

    public Crow(Vector2 position)
      : base(nameof (Crow), position)
    {
      this.IsWalkingTowardPlayer = false;
      this.sprite.spriteWidth = Game1.tileSize;
      this.sprite.spriteHeight = Game1.tileSize;
      this.sprite.UpdateSourceRect();
    }

    public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
    {
      return Math.Max(1, damage - this.resilience);
    }

    public override void behaviorAtGameTick(GameTime time)
    {
      base.behaviorAtGameTick(time);
      if (this.withinPlayerThreshold() && !this.startedFlying)
      {
        if (this.startedFlying)
          return;
        this.speed = 6;
        this.flyLeft = (double) Game1.player.position.X >= (double) this.position.X - (double) Game1.tileSize && ((double) Game1.player.position.X > (double) this.position.X + (double) (Game1.tileSize * 2) || Game1.random.NextDouble() < 0.5);
        if (!this.flyLeft)
          this.flip = true;
        this.startedFlying = true;
        this.drawOnTop = true;
      }
      else if (this.startedFlying)
      {
        this.sprite.Animate(time, 20, 4, 75f);
        this.position.X += this.flyLeft ? (float) -this.speed : (float) this.speed;
        this.position.Y -= this.flightRise;
        this.flightRise = this.flightRise + 0.1f;
      }
      else
      {
        if (Game1.currentLocation.isCropAtTile((int) this.getTileLocation().X, (int) this.getTileLocation().Y) && Game1.random.NextDouble() < 0.003)
        {
          this.Halt();
          switch (this.facingDirection)
          {
            case 0:
              this.sprite.CurrentFrame = 18;
              break;
            case 1:
              this.sprite.CurrentFrame = 17;
              break;
            case 2:
              this.sprite.CurrentFrame = 16;
              break;
            case 3:
              this.sprite.CurrentFrame = 19;
              break;
          }
          this.sprite.UpdateSourceRect();
          if (Game1.currentLocation.terrainFeatures[this.getTileLocation()] != null && Game1.currentLocation.terrainFeatures[this.getTileLocation()].GetType() == typeof (HoeDirt))
            ((HoeDirt) Game1.currentLocation.terrainFeatures[this.getTileLocation()]).destroyCrop(this.getTileLocation(), true);
        }
        else if (Game1.random.NextDouble() < 0.01)
        {
          switch (Game1.random.Next(6))
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
            default:
              this.Halt();
              break;
          }
        }
        this.MovePosition(time, Game1.viewport, Game1.currentLocation);
      }
    }
  }
}
