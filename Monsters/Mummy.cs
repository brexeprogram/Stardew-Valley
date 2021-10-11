// Decompiled with JetBrains decompiler
// Type: StardewValley.Monsters.Mummy
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StardewValley.Monsters
{
  public class Mummy : Monster
  {
    private int reviveTimer;
    public const int revivalTime = 10000;

    public Mummy()
    {
      this.sprite.spriteHeight = 32;
    }

    public Mummy(Vector2 position)
      : base(nameof (Mummy), position)
    {
      this.sprite.spriteHeight = 32;
      this.sprite.ignoreStopAnimation = true;
      this.sprite.UpdateSourceRect();
    }

    public override void reloadSprite()
    {
      this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Monsters\\Mummy"));
      this.sprite.spriteHeight = 32;
      this.sprite.UpdateSourceRect();
      this.sprite.ignoreStopAnimation = true;
    }

    public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
    {
      int num = Math.Max(1, damage - this.resilience);
      if (this.reviveTimer > 0)
      {
        if (!isBomb)
          return -1;
        this.health = 0;
        TemporaryAnimatedSprite t = new TemporaryAnimatedSprite(44, this.position, Color.BlueViolet, 10, false, 100f, 0, -1, -1f, -1, 0);
        t.holdLastFrame = true;
        t.alphaFade = 0.01f;
        t.interval = 70f;
        GameLocation currentLocation = Game1.currentLocation;
        int numAddOns = 4;
        int xRange = 64;
        int yRange = 64;
        Utility.makeTemporarySpriteJuicier(t, currentLocation, numAddOns, xRange, yRange);
        Game1.playSound("ghost");
        return 999;
      }
      if (Game1.random.NextDouble() < this.missChance - this.missChance * addedPrecision)
      {
        num = -1;
      }
      else
      {
        this.slipperiness = 2;
        this.health = this.health - num;
        this.setTrajectory(xTrajectory, yTrajectory);
        Game1.playSound("shadowHit");
        Game1.playSound("skeletonStep");
        this.IsWalkingTowardPlayer = true;
        if (this.health <= 0)
        {
          this.health = this.maxHealth;
          this.deathAnimation();
        }
      }
      return num;
    }

    public override void deathAnimation()
    {
      Game1.playSound("monsterdead");
      this.reviveTimer = 10000;
      this.sprite.setCurrentAnimation(this.getCrumbleAnimation(false));
      this.Halt();
      this.collidesWithOtherCharacters = false;
      this.IsWalkingTowardPlayer = false;
      this.moveTowardPlayerThreshold = -1;
    }

    private List<FarmerSprite.AnimationFrame> getCrumbleAnimation(bool reverse = false)
    {
      List<FarmerSprite.AnimationFrame> animationFrameList = new List<FarmerSprite.AnimationFrame>();
      if (!reverse)
        animationFrameList.Add(new FarmerSprite.AnimationFrame(16, 100, 0, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false, 0));
      else
        animationFrameList.Add(new FarmerSprite.AnimationFrame(16, 100, 0, false, false, new AnimatedSprite.endOfAnimationBehavior(this.behaviorAfterRevival), true, 0));
      animationFrameList.Add(new FarmerSprite.AnimationFrame(17, 100, 0, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false, 0));
      animationFrameList.Add(new FarmerSprite.AnimationFrame(18, 100, 0, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false, 0));
      if (!reverse)
        animationFrameList.Add(new FarmerSprite.AnimationFrame(19, 100, 0, false, false, new AnimatedSprite.endOfAnimationBehavior(this.behaviorAfterCrumble), false, 0));
      else
        animationFrameList.Add(new FarmerSprite.AnimationFrame(19, 100, 0, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false, 0));
      if (reverse)
        animationFrameList.Reverse();
      return animationFrameList;
    }

    public override void behaviorAtGameTick(GameTime time)
    {
      if (this.sprite.currentAnimation != null && this.Sprite.animateOnce(time))
        this.sprite.currentAnimation = (List<FarmerSprite.AnimationFrame>) null;
      if (this.reviveTimer > 0)
      {
        this.reviveTimer = this.reviveTimer - time.ElapsedGameTime.Milliseconds;
        if (this.reviveTimer < 2000)
          this.shake(this.reviveTimer);
        if (this.reviveTimer <= 0)
        {
          this.sprite.setCurrentAnimation(this.getCrumbleAnimation(true));
          Game1.playSound("skeletonDie");
          this.IsWalkingTowardPlayer = true;
        }
      }
      if (this.withinPlayerThreshold())
        this.IsWalkingTowardPlayer = true;
      base.behaviorAtGameTick(time);
    }

    private void behaviorAfterCrumble(Farmer who)
    {
      this.Halt();
      this.sprite.CurrentFrame = 19;
      this.sprite.currentAnimation = (List<FarmerSprite.AnimationFrame>) null;
    }

    private void behaviorAfterRevival(Farmer who)
    {
      this.IsWalkingTowardPlayer = true;
      this.collidesWithOtherCharacters = true;
      this.sprite.CurrentFrame = 0;
      this.moveTowardPlayerThreshold = 8;
      this.sprite.currentAnimation = (List<FarmerSprite.AnimationFrame>) null;
    }
  }
}
