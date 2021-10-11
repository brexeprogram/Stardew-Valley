// Decompiled with JetBrains decompiler
// Type: StardewValley.BellsAndWhistles.Birdie
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace StardewValley.BellsAndWhistles
{
  public class Birdie : Critter
  {
    private int characterCheckTimer = 200;
    public const int brownBird = 25;
    public const int blueBird = 45;
    public const int flyingSpeed = 6;
    public const int walkingSpeed = 1;
    public const int pecking = 0;
    public const int flyingAway = 1;
    public const int sleeping = 2;
    public const int stopped = 3;
    public const int walking = 4;
    private int state;
    private float flightOffset;
    private int walkTimer;

    public Birdie(int tileX, int tileY, int startingIndex = 25)
      : base(startingIndex, new Vector2((float) (tileX * Game1.tileSize), (float) (tileY * Game1.tileSize)))
    {
      this.flip = Game1.random.NextDouble() < 0.5;
      this.position.X += (float) (Game1.tileSize / 2);
      this.position.Y += (float) (Game1.tileSize / 2);
      this.startingPosition = this.position;
      this.flightOffset = (float) Game1.random.NextDouble() - 0.5f;
      this.state = 0;
    }

    public void hop(Farmer who)
    {
      this.gravityAffectedDY = -2f;
    }

    public override void drawAboveFrontLayer(SpriteBatch b)
    {
      if (this.state != 1)
        return;
      base.draw(b);
    }

    public override void draw(SpriteBatch b)
    {
      if (this.state == 1)
        return;
      base.draw(b);
    }

    private void donePecking(Farmer who)
    {
      this.state = Game1.random.NextDouble() < 0.5 ? 0 : 3;
    }

    private void playFlap(Farmer who)
    {
      if (!Utility.isOnScreen(this.position, Game1.tileSize))
        return;
      Game1.playSound("batFlap");
    }

    private void playPeck(Farmer who)
    {
      if (!Utility.isOnScreen(this.position, Game1.tileSize))
        return;
      Game1.playSound("shiny4");
    }

    public override bool update(GameTime time, GameLocation environment)
    {
      if ((double) this.yJumpOffset < 0.0 && this.state != 1)
      {
        if (!this.flip && !environment.isCollidingPosition(this.getBoundingBox(-2, 0), Game1.viewport, false, 0, false, (Character) null, false, false, true))
          this.position.X -= 2f;
        else if (!environment.isCollidingPosition(this.getBoundingBox(2, 0), Game1.viewport, false, 0, false, (Character) null, false, false, true))
          this.position.X += 2f;
      }
      this.characterCheckTimer = this.characterCheckTimer - time.ElapsedGameTime.Milliseconds;
      if (this.characterCheckTimer < 0)
      {
        Character character = Utility.isThereAFarmerOrCharacterWithinDistance(this.position / (float) Game1.tileSize, 4, environment);
        this.characterCheckTimer = 200;
        if (character != null && this.state != 1)
        {
          if (Game1.random.NextDouble() < 0.85)
            Game1.playSound("SpringBirds");
          this.state = 1;
          if ((double) character.position.X > (double) this.position.X)
            this.flip = false;
          else
            this.flip = true;
          this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
          {
            new FarmerSprite.AnimationFrame((int) (short) (this.baseFrame + 6), 70),
            new FarmerSprite.AnimationFrame((int) (short) (this.baseFrame + 7), 60, false, this.flip, new AnimatedSprite.endOfAnimationBehavior(this.playFlap), false),
            new FarmerSprite.AnimationFrame((int) (short) (this.baseFrame + 8), 70),
            new FarmerSprite.AnimationFrame((int) (short) (this.baseFrame + 7), 60)
          });
          this.sprite.loop = true;
        }
      }
      switch (this.state)
      {
        case 0:
          if (this.sprite.currentAnimation == null)
          {
            List<FarmerSprite.AnimationFrame> animation = new List<FarmerSprite.AnimationFrame>();
            animation.Add(new FarmerSprite.AnimationFrame((int) (short) (this.baseFrame + 2), 480));
            animation.Add(new FarmerSprite.AnimationFrame((int) (short) (this.baseFrame + 3), 170, false, this.flip, (AnimatedSprite.endOfAnimationBehavior) null, false));
            animation.Add(new FarmerSprite.AnimationFrame((int) (short) (this.baseFrame + 4), 170, false, this.flip, (AnimatedSprite.endOfAnimationBehavior) null, false));
            int num = Game1.random.Next(1, 5);
            for (int index = 0; index < num; ++index)
            {
              animation.Add(new FarmerSprite.AnimationFrame((int) (short) (this.baseFrame + 3), 70));
              animation.Add(new FarmerSprite.AnimationFrame((int) (short) (this.baseFrame + 4), 100, false, this.flip, new AnimatedSprite.endOfAnimationBehavior(this.playPeck), false));
            }
            animation.Add(new FarmerSprite.AnimationFrame((int) (short) (this.baseFrame + 3), 100));
            animation.Add(new FarmerSprite.AnimationFrame((int) (short) (this.baseFrame + 2), 70, false, this.flip, (AnimatedSprite.endOfAnimationBehavior) null, false));
            animation.Add(new FarmerSprite.AnimationFrame((int) (short) (this.baseFrame + 1), 70, false, this.flip, (AnimatedSprite.endOfAnimationBehavior) null, false));
            animation.Add(new FarmerSprite.AnimationFrame((int) (short) this.baseFrame, 500, false, this.flip, new AnimatedSprite.endOfAnimationBehavior(this.donePecking), false));
            this.sprite.loop = false;
            this.sprite.setCurrentAnimation(animation);
            break;
          }
          break;
        case 1:
          if (!this.flip)
            this.position.X -= 6f;
          else
            this.position.X += 6f;
          this.yOffset = this.yOffset - (2f + this.flightOffset);
          break;
        case 2:
          if (this.sprite.currentAnimation == null)
            this.sprite.CurrentFrame = this.baseFrame + 5;
          if (Game1.random.NextDouble() < 0.003 && this.sprite.currentAnimation == null)
          {
            this.state = 3;
            break;
          }
          break;
        case 3:
          if (Game1.random.NextDouble() < 0.008 && this.sprite.currentAnimation == null && (double) this.yJumpOffset >= 0.0)
          {
            switch (Game1.random.Next(6))
            {
              case 0:
                this.state = 2;
                break;
              case 1:
                this.state = 0;
                break;
              case 2:
                this.hop((Farmer) null);
                break;
              case 3:
                this.flip = !this.flip;
                this.hop((Farmer) null);
                break;
              case 4:
              case 5:
                this.state = 4;
                this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
                {
                  new FarmerSprite.AnimationFrame((int) (short) this.baseFrame, 100),
                  new FarmerSprite.AnimationFrame((int) (short) (this.baseFrame + 1), 100)
                });
                this.sprite.loop = true;
                if ((double) this.position.X >= (double) this.startingPosition.X)
                  this.flip = false;
                else
                  this.flip = true;
                this.walkTimer = Game1.random.Next(5, 15) * 100;
                break;
            }
          }
          else
          {
            if (this.sprite.currentAnimation == null)
            {
              this.sprite.CurrentFrame = this.baseFrame;
              break;
            }
            break;
          }
        case 4:
          if (!this.flip && !environment.isCollidingPosition(this.getBoundingBox(-1, 0), Game1.viewport, false, 0, false, (Character) null, false, false, true))
            --this.position.X;
          else if (this.flip && !environment.isCollidingPosition(this.getBoundingBox(1, 0), Game1.viewport, false, 0, false, (Character) null, false, false, true))
            ++this.position.X;
          this.walkTimer = this.walkTimer - time.ElapsedGameTime.Milliseconds;
          if (this.walkTimer < 0)
          {
            this.state = 3;
            this.sprite.loop = false;
            this.sprite.currentAnimation = (List<FarmerSprite.AnimationFrame>) null;
            this.sprite.CurrentFrame = this.baseFrame;
            break;
          }
          break;
      }
      return base.update(time, environment);
    }
  }
}
