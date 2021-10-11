// Decompiled with JetBrains decompiler
// Type: StardewValley.CoopDweller
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley
{
  public class CoopDweller : Character
  {
    public int uniqueFrameAccumulator = -1;
    public const double chancePerUpdateToChangeDirection = 0.007;
    public new const double chanceForSound = 0.002;
    public const int uniqueDownFrame = 16;
    public const int uniqueRightFrame = 18;
    public const int uniqueUpFrame = 20;
    public const int uniqueLeftFrame = 22;
    public const int pushAccumulatorTimeTillPush = 40;
    public const int timePerUniqueFrame = 500;
    public int daysToLay;
    public int daysSinceLastLay;
    public int defaultProduceIndex;
    public int friendshipTowardFarmer;
    public int daysSinceLastFed;
    public int pushAccumulator;
    public int age;
    public int ageWhenMature;
    public bool wasFed;
    public bool wasPet;
    public string sound;
    public string type;

    public CoopDweller()
    {
    }

    public CoopDweller(string type, string name)
      : base((AnimatedSprite) null, new Vector2((float) (Game1.tileSize * Game1.random.Next(2, 9)), (float) (Game1.tileSize * Game1.random.Next(5, 9))), 2, name)
    {
      this.type = type;
      if (!(type == "WhiteChicken"))
      {
        if (!(type == "BrownChicken"))
        {
          if (!(type == "Duck"))
          {
            if (!(type == "Rabbit"))
            {
              if (!(type == "Dinosaur"))
                return;
              this.daysToLay = 7;
              this.ageWhenMature = 0;
              this.defaultProduceIndex = 107;
              this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Animals\\Dinosaur"), 0, Game1.tileSize, Game1.tileSize);
            }
            else
            {
              this.daysToLay = 4;
              this.ageWhenMature = 3;
              this.defaultProduceIndex = 440;
              this.sound = "rabbit";
              this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Animals\\BabyRabbit"), 0, Game1.tileSize, Game1.tileSize);
            }
          }
          else
          {
            this.daysToLay = 2;
            this.ageWhenMature = 1;
            this.defaultProduceIndex = 442;
            this.sound = "cluck";
            this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Animals\\BabyBrownChicken"), 0, Game1.tileSize, Game1.tileSize);
          }
        }
        else
        {
          this.daysToLay = 1;
          this.ageWhenMature = 1;
          this.defaultProduceIndex = 180;
          this.sound = "cluck";
          this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Animals\\BabyBrownChicken"), 0, Game1.tileSize, Game1.tileSize);
        }
      }
      else
      {
        this.daysToLay = 1;
        this.ageWhenMature = 1;
        this.defaultProduceIndex = 176;
        this.sound = "cluck";
        this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Animals\\BabyWhiteChicken"), 0, Game1.tileSize, Game1.tileSize);
      }
    }

    public void reload()
    {
      string str = this.type;
      if (this.age < this.ageWhenMature)
        str = "Baby" + this.type;
      this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Animals\\" + str), 0, Game1.tileSize, Game1.tileSize);
    }

    public void pet()
    {
      if (Game1.timeOfDay >= 1900)
      {
        Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:CoopDweller.cs.550", (object) this.displayName));
      }
      else
      {
        this.Halt();
        this.sprite.StopAnimation();
        this.uniqueFrameAccumulator = -1;
        switch (Game1.player.FacingDirection)
        {
          case 0:
            this.sprite.currentFrame = 0;
            break;
          case 1:
            this.sprite.currentFrame = 12;
            break;
          case 2:
            this.sprite.currentFrame = 8;
            break;
          case 3:
            this.sprite.currentFrame = 4;
            break;
        }
        if (!this.wasPet)
        {
          this.wasPet = true;
          this.friendshipTowardFarmer = Math.Min(1000, this.friendshipTowardFarmer + 10);
          this.doEmote(20, true);
          Game1.playSound(this.sound);
        }
        else if (this.daysSinceLastFed == 0)
          Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:CoopDweller.cs.551", (object) this.displayName));
        else
          Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:CoopDweller.cs.552", (object) this.displayName));
      }
    }

    public void farmerPushing()
    {
      this.pushAccumulator = this.pushAccumulator + 1;
      if (this.pushAccumulator <= 40)
        return;
      switch (Game1.player.facingDirection)
      {
        case 0:
          this.Halt();
          this.SetMovingUp(true);
          break;
        case 1:
          this.Halt();
          this.SetMovingRight(true);
          break;
        case 2:
          this.Halt();
          this.SetMovingDown(true);
          break;
        case 3:
          this.Halt();
          this.SetMovingLeft(true);
          break;
      }
      this.pushAccumulator = 0;
    }

    public void setRandomPosition()
    {
      GameLocation locationFromName = Game1.getLocationFromName("Coop");
      string[] strArray = locationFromName.getMapProperty("ProduceArea").Split(' ');
      int index1 = 0;
      int int32_1 = Convert.ToInt32(strArray[index1]);
      int index2 = 1;
      int int32_2 = Convert.ToInt32(strArray[index2]);
      int index3 = 2;
      int int32_3 = Convert.ToInt32(strArray[index3]);
      int index4 = 3;
      int int32_4 = Convert.ToInt32(strArray[index4]);
      this.position = new Vector2((float) Game1.random.Next(int32_1, int32_1 + int32_3), (float) Game1.random.Next(int32_2, int32_2 + int32_4));
      int num = 0;
      while (locationFromName.Objects.ContainsKey(this.position))
      {
        this.position = new Vector2((float) Game1.random.Next(int32_1, int32_1 + int32_3), (float) Game1.random.Next(int32_2, int32_2 + int32_4));
        ++num;
        if (num > 2)
          break;
      }
      this.position.X *= (float) Game1.tileSize;
      this.position.Y *= (float) Game1.tileSize;
    }

    public int dayUpdate()
    {
      this.age = this.age + 1;
      this.daysSinceLastLay = this.daysSinceLastLay + 1;
      if (this.age == this.ageWhenMature)
        this.sprite.Texture = Game1.content.Load<Texture2D>("Animals\\" + this.type);
      if (!this.wasPet)
        this.friendshipTowardFarmer = Math.Max(0, this.friendshipTowardFarmer - (10 - this.friendshipTowardFarmer / 200));
      this.wasPet = false;
      int num;
      if (!this.wasFed || this.age < this.ageWhenMature || (this.daysSinceLastFed > 0 || this.daysSinceLastLay < this.daysToLay))
      {
        num = -1;
      }
      else
      {
        num = this.defaultProduceIndex;
        if (this.type.Equals("Duck") && Game1.random.NextDouble() < (double) this.friendshipTowardFarmer / 5000.0 + Game1.dailyLuck + (double) Game1.player.LuckLevel * 0.01)
          num = 444;
        else if (this.type.Equals("Rabbit") && Game1.random.NextDouble() < (double) this.friendshipTowardFarmer / 5000.0 + Game1.dailyLuck + (double) Game1.player.LuckLevel * 0.02)
          num = 446;
        this.daysSinceLastLay = 0;
        if (num <= 180)
        {
          if (num != 176)
          {
            if (num == 180)
              ++Game1.stats.ChickenEggsLayed;
          }
          else
            ++Game1.stats.ChickenEggsLayed;
        }
        else if (num != 440)
        {
          if (num == 442)
            ++Game1.stats.DuckEggsLayed;
        }
        else
          ++Game1.stats.RabbitWoolProduced;
        if (Game1.random.NextDouble() < (double) this.friendshipTowardFarmer / 1200.0)
        {
          if (num != 176)
          {
            if (num == 180)
              num += 2;
          }
          else
            num -= 2;
        }
      }
      this.daysSinceLastFed = this.wasFed ? Math.Max(0, this.daysSinceLastFed - 1) : this.daysSinceLastFed + 1;
      this.wasFed = false;
      return num;
    }

    public new void update(GameTime time, GameLocation location)
    {
      if (this.isEmoting)
        this.updateEmote(time);
      if (Game1.timeOfDay >= 1900)
      {
        this.sprite.currentFrame = 16;
        this.sprite.UpdateSourceRect();
        if (this.isEmoting || Game1.random.NextDouble() >= 0.002)
          return;
        this.doEmote(24, true);
      }
      else
      {
        if (Game1.random.NextDouble() < 0.002 && this.age >= this.ageWhenMature && this.sound != null)
          Game1.playSound(this.sound);
        if (Game1.random.NextDouble() < 0.007 && this.uniqueFrameAccumulator == -1)
        {
          int num = Game1.random.Next(5);
          if (num != (this.facingDirection + 2) % 4)
          {
            switch (num)
            {
              case 0:
                this.SetMovingUp(true);
                break;
              case 1:
                this.SetMovingRight(true);
                break;
              case 2:
                this.SetMovingDown(true);
                break;
              case 3:
                this.SetMovingLeft(true);
                break;
              default:
                this.Halt();
                this.sprite.StopAnimation();
                break;
            }
          }
          else
          {
            this.Halt();
            this.sprite.StopAnimation();
          }
        }
        if (this.isMoving() && Game1.random.NextDouble() < 0.014 && this.uniqueFrameAccumulator == -1)
        {
          this.Halt();
          this.sprite.StopAnimation();
          if (Game1.random.NextDouble() < 0.75)
          {
            this.uniqueFrameAccumulator = 0;
            switch (this.facingDirection)
            {
              case 0:
                this.sprite.currentFrame = 20;
                break;
              case 1:
                this.sprite.currentFrame = 18;
                break;
              case 2:
                this.sprite.currentFrame = 16;
                break;
              case 3:
                this.sprite.currentFrame = 22;
                break;
            }
          }
        }
        if (this.uniqueFrameAccumulator != -1)
        {
          this.uniqueFrameAccumulator = this.uniqueFrameAccumulator + time.ElapsedGameTime.Milliseconds;
          if (this.uniqueFrameAccumulator <= 500)
            return;
          this.sprite.CurrentFrame = this.sprite.CurrentFrame + 1 - this.sprite.CurrentFrame % 2 * 2;
          this.uniqueFrameAccumulator = 0;
          if (Game1.random.NextDouble() >= 0.4)
            return;
          this.uniqueFrameAccumulator = -1;
        }
        else if (this.moveUp)
        {
          if (!location.isCollidingPosition(this.nextPosition(0), Game1.viewport, false))
          {
            this.position.Y -= (float) this.speed;
            this.sprite.AnimateUp(time, 0, "");
          }
          else
          {
            this.Halt();
            this.sprite.StopAnimation();
            if (Game1.random.NextDouble() < 0.6)
              this.SetMovingDown(true);
          }
          this.facingDirection = 0;
        }
        else if (this.moveRight)
        {
          if (!location.isCollidingPosition(this.nextPosition(1), Game1.viewport, false))
          {
            this.position.X += (float) this.speed;
            this.sprite.AnimateRight(time, 0, "");
          }
          else
          {
            this.Halt();
            this.sprite.StopAnimation();
            if (Game1.random.NextDouble() < 0.6)
              this.SetMovingLeft(true);
          }
          this.facingDirection = 1;
        }
        else if (this.moveDown)
        {
          if (!location.isCollidingPosition(this.nextPosition(2), Game1.viewport, false))
          {
            this.position.Y += (float) this.speed;
            this.sprite.AnimateDown(time, 0, "");
          }
          else
          {
            this.Halt();
            this.sprite.StopAnimation();
            if (Game1.random.NextDouble() < 0.6)
              this.SetMovingUp(true);
          }
          this.facingDirection = 2;
        }
        else
        {
          if (!this.moveLeft)
            return;
          if (!location.isCollidingPosition(this.nextPosition(3), Game1.viewport, false))
          {
            this.position.X -= (float) this.speed;
            this.sprite.AnimateLeft(time, 0, "");
          }
          else
          {
            this.Halt();
            this.sprite.StopAnimation();
            if (Game1.random.NextDouble() < 0.6)
              this.SetMovingRight(true);
          }
          this.facingDirection = 3;
        }
      }
    }
  }
}
