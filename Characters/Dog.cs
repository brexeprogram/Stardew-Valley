// Decompiled with JetBrains decompiler
// Type: StardewValley.Characters.Dog
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace StardewValley.Characters
{
  public class Dog : Pet
  {
    public const int behavior_sit_right = 50;
    public const int behavior_sprint = 51;
    private int sprintTimer;
    private bool wagging;

    public Dog()
    {
      this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Animals\\dog"), 0, 32, 32);
      this.hideShadow = true;
      this.breather = false;
      this.willDestroyObjectsUnderfoot = false;
    }

    public Dog(int xTile, int yTile)
    {
      this.name = nameof (Dog);
      this.displayName = this.name;
      this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Animals\\dog"), 0, 32, 32);
      this.position = new Vector2((float) xTile, (float) yTile) * (float) Game1.tileSize;
      this.breather = false;
      this.willDestroyObjectsUnderfoot = false;
      this.currentLocation = Game1.currentLocation;
      this.hideShadow = true;
    }

    public override void dayUpdate(int dayOfMonth)
    {
      base.dayUpdate(dayOfMonth);
      this.sprintTimer = 0;
    }

    public override void update(GameTime time, GameLocation location)
    {
      base.update(time, location);
      if (this.currentLocation == null)
        this.currentLocation = location;
      if (Game1.eventUp)
        return;
      if (this.sprintTimer > 0)
      {
        this.sprite.loop = true;
        this.sprintTimer = this.sprintTimer - time.ElapsedGameTime.Milliseconds;
        this.speed = 6;
        this.tryToMoveInDirection(this.facingDirection, false, -1, false);
        if (this.sprintTimer > 0)
          return;
        this.sprite.currentAnimation = (List<FarmerSprite.AnimationFrame>) null;
        this.Halt();
        this.faceDirection(this.facingDirection);
        this.speed = 2;
        this.CurrentBehavior = 0;
      }
      else
      {
        if (Game1.timeOfDay > 2000 && this.sprite.currentAnimation == null && ((double) this.xVelocity == 0.0 && (double) this.yVelocity == 0.0))
          this.CurrentBehavior = 1;
        switch (this.CurrentBehavior)
        {
          case 0:
            if (this.sprite.currentAnimation == null && Game1.random.NextDouble() < 0.01)
            {
              switch (Game1.random.Next(7 + (this.currentLocation is Farm ? 1 : 0)))
              {
                case 0:
                case 1:
                case 2:
                case 3:
                  this.CurrentBehavior = 0;
                  break;
                case 4:
                case 5:
                  switch (this.facingDirection)
                  {
                    case 0:
                    case 1:
                    case 3:
                      this.Halt();
                      if (this.facingDirection == 0)
                        this.facingDirection = Game1.random.NextDouble() < 0.5 ? 3 : 1;
                      this.faceDirection(this.facingDirection);
                      this.sprite.loop = false;
                      this.CurrentBehavior = 50;
                      break;
                    case 2:
                      this.Halt();
                      this.faceDirection(2);
                      this.sprite.loop = false;
                      this.CurrentBehavior = 2;
                      break;
                  }
                case 6:
                case 7:
                  this.CurrentBehavior = 51;
                  break;
              }
            }
            else
              break;
          case 1:
            if (Game1.timeOfDay < 2000 && Game1.random.NextDouble() < 0.001)
            {
              this.CurrentBehavior = 0;
              return;
            }
            if (Game1.random.NextDouble() >= 0.002)
              return;
            this.doEmote(24, true);
            return;
          case 2:
            if (this.Sprite.currentFrame != 18 && this.sprite.currentAnimation == null)
            {
              this.CurrentBehavior = 2;
              break;
            }
            if (this.Sprite.currentFrame == 18 && Game1.random.NextDouble() < 0.01)
            {
              switch (Game1.random.Next(4))
              {
                case 0:
                  this.CurrentBehavior = 0;
                  this.Halt();
                  this.faceDirection(2);
                  this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
                  {
                    new FarmerSprite.AnimationFrame(17, 200),
                    new FarmerSprite.AnimationFrame(16, 200),
                    new FarmerSprite.AnimationFrame(0, 200)
                  });
                  this.sprite.loop = false;
                  break;
                case 1:
                  List<FarmerSprite.AnimationFrame> animation1 = new List<FarmerSprite.AnimationFrame>()
                  {
                    new FarmerSprite.AnimationFrame(18, 200, false, false, new AnimatedSprite.endOfAnimationBehavior(this.pantSound), false),
                    new FarmerSprite.AnimationFrame(19, 200)
                  };
                  int num1 = Game1.random.Next(7, 20);
                  for (int index = 0; index < num1; ++index)
                  {
                    animation1.Add(new FarmerSprite.AnimationFrame(18, 200, false, false, new AnimatedSprite.endOfAnimationBehavior(this.pantSound), false));
                    animation1.Add(new FarmerSprite.AnimationFrame(19, 200));
                  }
                  this.sprite.setCurrentAnimation(animation1);
                  break;
                case 2:
                case 3:
                  this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
                  {
                    new FarmerSprite.AnimationFrame(27, Game1.random.NextDouble() < 0.3 ? 500 : Game1.random.Next(2000, 15000)),
                    new FarmerSprite.AnimationFrame(18, 1, false, false, new AnimatedSprite.endOfAnimationBehavior(((Pet) this).hold), false)
                  });
                  this.sprite.loop = false;
                  break;
              }
            }
            else
              break;
          case 50:
            if (this.withinPlayerThreshold(2))
            {
              if (!this.wagging)
              {
                this.wag(this.facingDirection == 3);
                break;
              }
              break;
            }
            if (this.Sprite.currentFrame != 23 && this.sprite.currentAnimation == null)
            {
              this.sprite.CurrentFrame = 23;
              break;
            }
            if (this.sprite.currentFrame == 23 && Game1.random.NextDouble() < 0.01)
            {
              bool flag = this.facingDirection == 3;
              switch (Game1.random.Next(7))
              {
                case 0:
                  this.CurrentBehavior = 0;
                  this.Halt();
                  this.faceDirection(flag ? 3 : 1);
                  this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
                  {
                    new FarmerSprite.AnimationFrame(23, 100, false, flag, (AnimatedSprite.endOfAnimationBehavior) null, false),
                    new FarmerSprite.AnimationFrame(22, 100, false, flag, (AnimatedSprite.endOfAnimationBehavior) null, false),
                    new FarmerSprite.AnimationFrame(21, 100, false, flag, (AnimatedSprite.endOfAnimationBehavior) null, false),
                    new FarmerSprite.AnimationFrame(20, 100, false, flag, new AnimatedSprite.endOfAnimationBehavior(((Pet) this).hold), false)
                  });
                  this.sprite.loop = false;
                  break;
                case 1:
                  if (Utility.isOnScreen(this.getTileLocationPoint(), Game1.tileSize * 10, this.currentLocation))
                  {
                    Game1.playSound("dog_bark");
                    this.shake(500);
                  }
                  this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
                  {
                    new FarmerSprite.AnimationFrame(26, 500, false, flag, (AnimatedSprite.endOfAnimationBehavior) null, false),
                    new FarmerSprite.AnimationFrame(23, 1, false, flag, new AnimatedSprite.endOfAnimationBehavior(((Pet) this).hold), false)
                  });
                  break;
                case 2:
                  this.wag(flag);
                  break;
                case 3:
                case 4:
                  this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
                  {
                    new FarmerSprite.AnimationFrame(23, Game1.random.Next(2000, 6000), false, flag, (AnimatedSprite.endOfAnimationBehavior) null, false),
                    new FarmerSprite.AnimationFrame(23, 1, false, flag, new AnimatedSprite.endOfAnimationBehavior(((Pet) this).hold), false)
                  });
                  break;
                default:
                  this.sprite.loop = false;
                  List<FarmerSprite.AnimationFrame> animation2 = new List<FarmerSprite.AnimationFrame>()
                  {
                    new FarmerSprite.AnimationFrame(24, 200, false, flag, new AnimatedSprite.endOfAnimationBehavior(this.pantSound), false),
                    new FarmerSprite.AnimationFrame(25, 200, false, flag, (AnimatedSprite.endOfAnimationBehavior) null, false)
                  };
                  int num2 = Game1.random.Next(5, 15);
                  for (int index = 0; index < num2; ++index)
                  {
                    animation2.Add(new FarmerSprite.AnimationFrame(24, 200, false, flag, new AnimatedSprite.endOfAnimationBehavior(this.pantSound), false));
                    animation2.Add(new FarmerSprite.AnimationFrame(25, 200, false, flag, (AnimatedSprite.endOfAnimationBehavior) null, false));
                  }
                  this.sprite.setCurrentAnimation(animation2);
                  break;
              }
            }
            else
              break;
        }
        if (this.sprite.currentAnimation != null)
          this.sprite.loop = false;
        else
          this.wagging = false;
        if (this.sprite.currentAnimation != null)
          return;
        this.MovePosition(time, Game1.viewport, location);
      }
    }

    public void wag(bool localFlip)
    {
      int milliseconds = this.withinPlayerThreshold(2) ? 120 : 200;
      this.wagging = true;
      this.sprite.loop = false;
      List<FarmerSprite.AnimationFrame> animation = new List<FarmerSprite.AnimationFrame>()
      {
        new FarmerSprite.AnimationFrame(31, milliseconds, false, localFlip, (AnimatedSprite.endOfAnimationBehavior) null, false),
        new FarmerSprite.AnimationFrame(23, milliseconds, false, localFlip, new AnimatedSprite.endOfAnimationBehavior(this.hitGround), false)
      };
      int num = Game1.random.Next(2, 6);
      for (int index = 0; index < num; ++index)
      {
        animation.Add(new FarmerSprite.AnimationFrame(31, milliseconds, false, localFlip, (AnimatedSprite.endOfAnimationBehavior) null, false));
        animation.Add(new FarmerSprite.AnimationFrame(23, milliseconds, false, localFlip, new AnimatedSprite.endOfAnimationBehavior(this.hitGround), false));
      }
      animation.Add(new FarmerSprite.AnimationFrame(23, 2, false, localFlip, new AnimatedSprite.endOfAnimationBehavior(this.doneWagging), false));
      this.sprite.setCurrentAnimation(animation);
    }

    public void doneWagging(Farmer who)
    {
      this.wagging = false;
    }

    public override void initiateCurrentBehavior()
    {
      this.sprintTimer = 0;
      base.initiateCurrentBehavior();
      bool flip1 = this.facingDirection == 3;
      switch (this.CurrentBehavior)
      {
        case 50:
          this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
          {
            new FarmerSprite.AnimationFrame(20, 100, false, flip1, (AnimatedSprite.endOfAnimationBehavior) null, false),
            new FarmerSprite.AnimationFrame(21, 100, false, flip1, (AnimatedSprite.endOfAnimationBehavior) null, false),
            new FarmerSprite.AnimationFrame(22, 100, false, flip1, (AnimatedSprite.endOfAnimationBehavior) null, false),
            new FarmerSprite.AnimationFrame(23, 100, false, flip1, new AnimatedSprite.endOfAnimationBehavior(((Pet) this).hold), false)
          });
          break;
        case 51:
          this.faceDirection(Game1.random.NextDouble() < 0.5 ? 3 : 1);
          bool flip2 = this.facingDirection == 3;
          this.sprintTimer = Game1.random.Next(1000, 3500);
          if (Utility.isOnScreen(this.getTileLocationPoint(), Game1.tileSize, this.currentLocation))
            Game1.playSound("dog_bark");
          this.sprite.loop = true;
          this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
          {
            new FarmerSprite.AnimationFrame(32, 100, false, flip2, (AnimatedSprite.endOfAnimationBehavior) null, false),
            new FarmerSprite.AnimationFrame(33, 100, false, flip2, (AnimatedSprite.endOfAnimationBehavior) null, false),
            new FarmerSprite.AnimationFrame(34, 100, false, flip2, new AnimatedSprite.endOfAnimationBehavior(this.hitGround), false),
            new FarmerSprite.AnimationFrame(33, 100, false, flip2, (AnimatedSprite.endOfAnimationBehavior) null, false)
          });
          break;
      }
    }

    public void hitGround(Farmer who)
    {
      if (!Utility.isOnScreen(this.getTileLocationPoint(), 2 * Game1.tileSize, this.currentLocation))
        return;
      this.currentLocation.playTerrainSound(this.getTileLocation(), (Character) this, false);
    }

    public void pantSound(Farmer who)
    {
      if (!this.withinPlayerThreshold(5))
        return;
      Game1.playSound("dog_pant");
    }

    public void thumpSound(Farmer who)
    {
      if (!this.withinPlayerThreshold(4))
        return;
      Game1.playSound("thudStep");
    }

    public override void playContentSound()
    {
      if (!Utility.isOnScreen(this.getTileLocationPoint(), Game1.tileSize * 2, this.currentLocation))
        return;
      Game1.playSound("dog_pant");
      DelayedAction.playSoundAfterDelay("dog_pant", 400);
    }
  }
}
