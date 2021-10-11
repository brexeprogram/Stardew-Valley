// Decompiled with JetBrains decompiler
// Type: StardewValley.Characters.Cat
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace StardewValley.Characters
{
  public class Cat : Pet
  {
    public Cat()
    {
      this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Animals\\cat"), 0, 32, 32);
      this.hideShadow = true;
      this.breather = false;
      this.willDestroyObjectsUnderfoot = false;
    }

    public Cat(int xTile, int yTile)
    {
      this.name = nameof (Cat);
      this.displayName = this.name;
      this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Animals\\cat"), 0, 32, 32);
      this.position = new Vector2((float) xTile, (float) yTile) * (float) Game1.tileSize;
      this.breather = false;
      this.willDestroyObjectsUnderfoot = false;
      this.currentLocation = Game1.currentLocation;
      this.hideShadow = true;
    }

    public override void initiateCurrentBehavior()
    {
      base.initiateCurrentBehavior();
    }

    public override void update(GameTime time, GameLocation location)
    {
      base.update(time, location);
      if (this.currentLocation == null)
        this.currentLocation = location;
      if (Game1.eventUp)
        return;
      if (Game1.timeOfDay > 2000 && this.sprite.currentAnimation == null && ((double) this.xVelocity == 0.0 && (double) this.yVelocity == 0.0))
        this.CurrentBehavior = 1;
      switch (this.CurrentBehavior)
      {
        case 0:
          if (this.sprite.currentAnimation == null && Game1.random.NextDouble() < 0.01)
          {
            switch (Game1.random.Next(4))
            {
              case 0:
              case 1:
              case 2:
                this.CurrentBehavior = 0;
                break;
              case 3:
                switch (this.facingDirection)
                {
                  case 0:
                  case 2:
                    this.Halt();
                    this.faceDirection(2);
                    this.sprite.loop = false;
                    this.CurrentBehavior = 2;
                    break;
                  case 1:
                    if (Game1.random.NextDouble() < 0.85)
                    {
                      this.sprite.loop = false;
                      this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
                      {
                        new FarmerSprite.AnimationFrame(24, 100),
                        new FarmerSprite.AnimationFrame(25, 100),
                        new FarmerSprite.AnimationFrame(26, 100),
                        new FarmerSprite.AnimationFrame(27, Game1.random.Next(8000, 30000), false, false, new AnimatedSprite.endOfAnimationBehavior(this.flopSound), false)
                      });
                      break;
                    }
                    this.sprite.loop = false;
                    this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
                    {
                      new FarmerSprite.AnimationFrame(30, 300),
                      new FarmerSprite.AnimationFrame(31, 300),
                      new FarmerSprite.AnimationFrame(30, 300),
                      new FarmerSprite.AnimationFrame(31, 300),
                      new FarmerSprite.AnimationFrame(30, 300),
                      new FarmerSprite.AnimationFrame(31, 500),
                      new FarmerSprite.AnimationFrame(24, 800, false, false, new AnimatedSprite.endOfAnimationBehavior(this.leap), false),
                      new FarmerSprite.AnimationFrame(4, 1)
                    });
                    break;
                  case 3:
                    if (Game1.random.NextDouble() < 0.85)
                    {
                      this.sprite.loop = false;
                      this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
                      {
                        new FarmerSprite.AnimationFrame(24, 100, false, true, (AnimatedSprite.endOfAnimationBehavior) null, false),
                        new FarmerSprite.AnimationFrame(25, 100, false, true, (AnimatedSprite.endOfAnimationBehavior) null, false),
                        new FarmerSprite.AnimationFrame(26, 100, false, true, (AnimatedSprite.endOfAnimationBehavior) null, false),
                        new FarmerSprite.AnimationFrame(27, Game1.random.Next(8000, 30000), false, true, new AnimatedSprite.endOfAnimationBehavior(this.flopSound), false),
                        new FarmerSprite.AnimationFrame(12, 1)
                      });
                      break;
                    }
                    this.sprite.loop = false;
                    this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
                    {
                      new FarmerSprite.AnimationFrame(30, 300, false, true, (AnimatedSprite.endOfAnimationBehavior) null, false),
                      new FarmerSprite.AnimationFrame(31, 300, false, true, (AnimatedSprite.endOfAnimationBehavior) null, false),
                      new FarmerSprite.AnimationFrame(30, 300, false, true, (AnimatedSprite.endOfAnimationBehavior) null, false),
                      new FarmerSprite.AnimationFrame(31, 300, false, true, (AnimatedSprite.endOfAnimationBehavior) null, false),
                      new FarmerSprite.AnimationFrame(30, 300, false, true, (AnimatedSprite.endOfAnimationBehavior) null, false),
                      new FarmerSprite.AnimationFrame(31, 500, false, true, (AnimatedSprite.endOfAnimationBehavior) null, false),
                      new FarmerSprite.AnimationFrame(24, 800, false, true, new AnimatedSprite.endOfAnimationBehavior(this.leap), false),
                      new FarmerSprite.AnimationFrame(12, 1)
                    });
                    break;
                }
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
            switch (Game1.random.Next(10))
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
              case 2:
              case 3:
                List<FarmerSprite.AnimationFrame> animation = new List<FarmerSprite.AnimationFrame>()
                {
                  new FarmerSprite.AnimationFrame(19, 300),
                  new FarmerSprite.AnimationFrame(20, 200),
                  new FarmerSprite.AnimationFrame(21, 200),
                  new FarmerSprite.AnimationFrame(22, 200, false, false, new AnimatedSprite.endOfAnimationBehavior(this.lickSound), false),
                  new FarmerSprite.AnimationFrame(23, 200)
                };
                int num = Game1.random.Next(1, 6);
                for (int index = 0; index < num; ++index)
                {
                  animation.Add(new FarmerSprite.AnimationFrame(21, 150));
                  animation.Add(new FarmerSprite.AnimationFrame(22, 150, false, false, new AnimatedSprite.endOfAnimationBehavior(this.lickSound), false));
                  animation.Add(new FarmerSprite.AnimationFrame(23, 150));
                }
                animation.Add(new FarmerSprite.AnimationFrame(18, 1, false, false, new AnimatedSprite.endOfAnimationBehavior(((Pet) this).hold), false));
                this.sprite.loop = false;
                this.sprite.setCurrentAnimation(animation);
                break;
              default:
                bool flag = Game1.random.NextDouble() < 0.45;
                this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
                {
                  new FarmerSprite.AnimationFrame(19, flag ? 200 : Game1.random.Next(1000, 9000)),
                  new FarmerSprite.AnimationFrame(18, 1, false, false, new AnimatedSprite.endOfAnimationBehavior(((Pet) this).hold), false)
                });
                this.sprite.loop = false;
                if (flag && Game1.random.NextDouble() < 0.2)
                {
                  this.playContentSound();
                  this.shake(200);
                  break;
                }
                break;
            }
          }
          else
            break;
      }
      if (this.sprite.currentAnimation != null)
        this.sprite.loop = false;
      if (this.sprite.currentAnimation == null)
      {
        this.MovePosition(time, Game1.viewport, location);
      }
      else
      {
        if ((double) this.xVelocity == 0.0 && (double) this.yVelocity == 0.0)
          return;
        Rectangle boundingBox = this.GetBoundingBox();
        boundingBox.X += (int) this.xVelocity;
        boundingBox.Y -= (int) this.yVelocity;
        if (this.currentLocation == null || !this.currentLocation.isCollidingPosition(boundingBox, Game1.viewport, false, 0, false, (Character) this))
        {
          this.position.X += (float) (int) this.xVelocity;
          this.position.Y -= (float) (int) this.yVelocity;
        }
        this.xVelocity = (float) (int) ((double) this.xVelocity - (double) this.xVelocity / 4.0);
        this.yVelocity = (float) (int) ((double) this.yVelocity - (double) this.yVelocity / 4.0);
      }
    }

    public void lickSound(Farmer who)
    {
      if (!Utility.isOnScreen(this.getTileLocationPoint(), Game1.tileSize * 2, this.currentLocation))
        return;
      Game1.playSound("Cowboy_Footstep");
    }

    public void leap(Farmer who)
    {
      if (this.currentLocation.Equals((object) Game1.currentLocation))
        this.jump();
      if (this.facingDirection == 1)
      {
        this.xVelocity = 8f;
      }
      else
      {
        if (this.facingDirection != 3)
          return;
        this.xVelocity = -8f;
      }
    }

    public void flopSound(Farmer who)
    {
      if (!Utility.isOnScreen(this.getTileLocationPoint(), Game1.tileSize * 2, this.currentLocation))
        return;
      Game1.playSound("thudStep");
    }

    public override void playContentSound()
    {
      if (!Utility.isOnScreen(this.getTileLocationPoint(), Game1.tileSize * 2, this.currentLocation))
        return;
      Game1.playSound("cat");
    }
  }
}
