// Decompiled with JetBrains decompiler
// Type: StardewValley.Monsters.RockGolem
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Locations;
using System;

namespace StardewValley.Monsters
{
  public class RockGolem : Monster
  {
    private bool seenPlayer;

    public RockGolem()
    {
    }

    public RockGolem(Vector2 position)
      : base("Stone Golem", position)
    {
      this.IsWalkingTowardPlayer = false;
      this.sprite.CurrentFrame = 16;
      this.sprite.loop = false;
      this.slipperiness = 2;
      this.jitteriness = 0.0;
      this.hideShadow = true;
      if (!(Game1.currentLocation is MineShaft))
        return;
      int mineLevel = (Game1.currentLocation as MineShaft).mineLevel;
      if (mineLevel > 80)
      {
        this.damageToFarmer = this.damageToFarmer * 2;
        this.health = (int) ((double) this.health * 2.5);
      }
      else
      {
        if (mineLevel <= 40)
          return;
        this.damageToFarmer = (int) ((double) this.damageToFarmer * 1.5);
        this.health = (int) ((double) this.health * 1.75);
      }
    }

    public RockGolem(Vector2 position, int difficultyMod)
      : base("Wilderness Golem", position)
    {
      this.IsWalkingTowardPlayer = false;
      this.sprite.CurrentFrame = 16;
      this.sprite.loop = false;
      this.slipperiness = 3;
      this.hideShadow = true;
      this.jitteriness = 0.0;
      this.damageToFarmer = this.damageToFarmer + difficultyMod;
      this.health = this.health + (int) ((double) (difficultyMod * difficultyMod) * 2.0);
      this.experienceGained = this.experienceGained + difficultyMod;
      if (difficultyMod >= 5 && Game1.random.NextDouble() < 0.05)
        this.objectsToDrop.Add(749);
      if (difficultyMod >= 5 && Game1.random.NextDouble() < 0.2)
        this.objectsToDrop.Add(770);
      if (difficultyMod >= 10 && Game1.random.NextDouble() < 0.01)
        this.objectsToDrop.Add(386);
      if (difficultyMod >= 10 && Game1.random.NextDouble() < 0.01)
        this.objectsToDrop.Add(386);
      if (difficultyMod < 10 || Game1.random.NextDouble() >= 0.001)
        return;
      this.objectsToDrop.Add(74);
    }

    public RockGolem(Vector2 position, bool alreadySpawned)
      : base("Stone Golem", position)
    {
      if (alreadySpawned)
      {
        this.IsWalkingTowardPlayer = true;
        this.seenPlayer = true;
        this.moveTowardPlayerThreshold = 16;
      }
      else
      {
        this.IsWalkingTowardPlayer = false;
        this.sprite.CurrentFrame = 16;
      }
      this.sprite.loop = false;
      this.slipperiness = 2;
    }

    public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
    {
      int num = Math.Max(1, damage - this.resilience);
      this.focusedOnFarmers = true;
      this.IsWalkingTowardPlayer = true;
      if (Game1.random.NextDouble() < this.missChance - this.missChance * addedPrecision)
      {
        num = -1;
      }
      else
      {
        this.health = this.health - num;
        this.setTrajectory(xTrajectory, yTrajectory);
        if (this.health <= 0)
          this.deathAnimation();
        else
          Game1.playSound("rockGolemHit");
        Game1.playSound("hitEnemy");
      }
      return num;
    }

    public override void deathAnimation()
    {
      Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(46, this.position, Color.DarkGray, 10, false, 100f, 0, -1, -1f, -1, 0));
      Game1.playSound("rockGolemDie");
      GameLocation currentLocation = Game1.currentLocation;
      Texture2D texture = this.sprite.Texture;
      Rectangle sourcerectangle = new Rectangle(0, 576, 64, 64);
      int sizeOfSourceRectSquares = 32;
      Rectangle boundingBox = this.GetBoundingBox();
      int x = boundingBox.Center.X;
      boundingBox = this.GetBoundingBox();
      int y1 = boundingBox.Center.Y;
      int numberOfChunks = Game1.random.Next(4, 9);
      int y2 = (int) this.getTileLocation().Y;
      Game1.createRadialDebris(currentLocation, texture, sourcerectangle, sizeOfSourceRectSquares, x, y1, numberOfChunks, y2);
    }

    public override void noMovementProgressNearPlayerBehavior()
    {
      if (!this.IsWalkingTowardPlayer)
        return;
      this.Halt();
      this.faceGeneralDirection(Utility.getNearestFarmerInCurrentLocation(this.getTileLocation()).getStandingPosition(), 0);
    }

    public override void behaviorAtGameTick(GameTime time)
    {
      if (this.IsWalkingTowardPlayer)
        base.behaviorAtGameTick(time);
      if (!this.seenPlayer)
      {
        if (!this.withinPlayerThreshold())
          return;
        Game1.playSound("rockGolemSpawn");
        this.seenPlayer = true;
      }
      else if (this.sprite.CurrentFrame >= 16)
      {
        this.sprite.Animate(time, 16, 8, 75f);
        if (this.sprite.CurrentFrame < 24)
          return;
        this.sprite.loop = true;
        this.sprite.CurrentFrame = 0;
        this.moveTowardPlayerThreshold = 16;
        this.IsWalkingTowardPlayer = true;
        this.jitteriness = 0.01;
        this.hideShadow = false;
      }
      else
      {
        if (!this.IsWalkingTowardPlayer || Game1.random.NextDouble() >= 0.001 || !Utility.isOnScreen(this.getStandingPosition(), 0))
          return;
        this.controller = new PathFindController((Character) this, Game1.currentLocation, new Point((int) Game1.player.getTileLocation().X, (int) Game1.player.getTileLocation().Y), -1, (PathFindController.endBehavior) null, 200);
      }
    }
  }
}
