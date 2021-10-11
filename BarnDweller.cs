// Decompiled with JetBrains decompiler
// Type: StardewValley.BarnDweller
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley
{
  public class BarnDweller : Character
  {
    public const double chancePerUpdateToChangeDirection = 0.007;
    public new const double chanceForSound = 0.002;
    public const int pushAccumulatorTimeTillPush = 40;
    public int daysToLay;
    public int daysSinceLastLay;
    public int defaultProduceIndex;
    public int friendshipTowardFarmer;
    public int daysSinceLastFed;
    public int pushAccumulator;
    public int age;
    public int ageWhenMature;
    public bool hasProduce;
    public bool wasPet;
    public bool wasFed;
    public string sound;
    public string type;

    public BarnDweller()
    {
    }

    public BarnDweller(string type, string name)
      : base(new AnimatedSprite((Texture2D) null, 0, Game1.tileSize, Game1.tileSize), new Vector2((float) (Game1.tileSize * Game1.random.Next(6, 16)), (float) (Game1.tileSize * Game1.random.Next(6, 13))), 2, name)
    {
      this.type = type;
      if (!(type == "WhiteBlackCow"))
      {
        if (!(type == "Pig"))
        {
          if (!(type == "Goat"))
          {
            if (!(type == "Sheep"))
              return;
            this.defaultProduceIndex = 440;
            this.sound = "goat";
            this.sprite = (AnimatedSprite) new LivestockSprite(Game1.content.Load<Texture2D>("Animals\\BabySheep"), 0);
            this.ageWhenMature = 1;
            this.daysToLay = 3;
          }
          else
          {
            this.defaultProduceIndex = 436;
            this.sound = "goat";
            this.sprite = (AnimatedSprite) new LivestockSprite(Game1.content.Load<Texture2D>("Animals\\BabyGoat"), 0);
            this.ageWhenMature = 1;
            this.daysToLay = 2;
          }
        }
        else
        {
          this.defaultProduceIndex = 430;
          this.sound = "pig";
          this.sprite = (AnimatedSprite) new LivestockSprite(Game1.content.Load<Texture2D>("Animals\\BabyPig"), 0);
          this.ageWhenMature = 1;
          this.daysToLay = 1;
        }
      }
      else
      {
        this.defaultProduceIndex = 184;
        this.sound = "cow";
        this.sprite = (AnimatedSprite) new LivestockSprite(Game1.content.Load<Texture2D>("Animals\\BabyWhiteBlackCow"), 0);
        this.ageWhenMature = 1;
        this.daysToLay = 1;
      }
    }

    public BarnDweller(string type, int tileX, int tileY)
      : base(new AnimatedSprite((Texture2D) null, 0, Game1.tileSize, Game1.tileSize), new Vector2((float) (Game1.tileSize * tileX), (float) (Game1.tileSize * tileY)), 2, "Missingno")
    {
      if (!(type == "Cow"))
        return;
      this.sound = "cow";
      this.sprite = (AnimatedSprite) new LivestockSprite(Game1.content.Load<Texture2D>("Animals\\Cow"), 0);
    }

    public void reload()
    {
      string str = this.type;
      if (this.age < this.ageWhenMature)
        str = "Baby" + this.type;
      this.sprite = (AnimatedSprite) new LivestockSprite(Game1.content.Load<Texture2D>("Animals\\" + str), 0);
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
          if (!Game1.currentLocation.isCollidingPosition(new Rectangle((int) this.position.X - Game1.tileSize, (int) this.position.Y + this.sprite.getHeight() - Game1.tileSize / 2, Game1.tileSize * 2 - Game1.tileSize / 4, Game1.tileSize / 2), Game1.viewport, (Character) this))
          {
            this.SetMovingLeft(true);
            if (this.facingDirection != 3)
            {
              this.position.X -= (float) Game1.tileSize;
              break;
            }
            break;
          }
          this.SetMovingUp(true);
          this.faceDirection(0);
          break;
      }
      this.faceDirection(Game1.player.facingDirection);
      this.sprite.UpdateSourceRect();
      this.pushAccumulator = 0;
    }

    public override Rectangle GetBoundingBox()
    {
      int width = this.facingDirection == 3 || this.facingDirection == 1 ? Game1.tileSize * 2 - Game1.tileSize / 4 : Game1.tileSize * 3 / 4;
      return new Rectangle((int) this.position.X + Game1.tileSize / 8, (int) this.position.Y + this.sprite.getHeight() - Game1.tileSize / 2, width, Game1.tileSize / 2);
    }

    public void dayUpdate()
    {
      this.age = this.age + 1;
      this.daysSinceLastLay = this.daysSinceLastLay + 1;
      if (this.age == this.ageWhenMature)
        this.sprite.Texture = Game1.content.Load<Texture2D>("Animals\\" + this.type);
      if (!this.wasPet)
      {
        this.friendshipTowardFarmer = Math.Max(0, this.friendshipTowardFarmer - (10 - this.friendshipTowardFarmer / 200));
        if (this.friendshipTowardFarmer <= 990)
          this.daysToLay = 3;
      }
      if (this.wasFed)
      {
        int val1 = 0;
        int daysSinceLastFed = this.daysSinceLastFed;
        this.daysSinceLastFed = daysSinceLastFed - 1;
        int val2 = daysSinceLastFed;
        this.daysSinceLastFed = Math.Max(val1, val2);
      }
      else
      {
        this.daysSinceLastFed = this.daysSinceLastFed + 1;
        this.hasProduce = false;
      }
      if (this.daysSinceLastFed <= 0 && this.daysSinceLastLay >= this.daysToLay && this.age >= this.ageWhenMature)
      {
        this.hasProduce = true;
        if (this.type.Equals("Pig") && Game1.random.NextDouble() < 0.75 - (double) Game1.player.LuckLevel * 0.015 - Game1.dailyLuck - (double) this.friendshipTowardFarmer / 10000.0)
          this.hasProduce = false;
        else if (this.type.Equals("Sheep"))
          this.sprite.Texture = Game1.content.Load<Texture2D>("Animals\\Sheep");
      }
      this.wasFed = false;
      this.wasPet = false;
    }

    public Object getProduce()
    {
      if (!this.hasProduce)
        return (Object) null;
      this.hasProduce = false;
      this.daysSinceLastLay = 0;
      if (this.type.Equals("Sheep"))
        this.sprite.Texture = Game1.content.Load<Texture2D>("Animals\\ShearedSheep");
      switch (this.defaultProduceIndex)
      {
        case 436:
          ++Game1.stats.GoatMilkProduced;
          break;
        case 440:
          ++Game1.stats.SheepWoolProduced;
          break;
        case 184:
          ++Game1.stats.CowMilkProduced;
          break;
        case 430:
          ++Game1.stats.TrufflesFound;
          break;
      }
      int defaultProduceIndex = this.defaultProduceIndex;
      if (Game1.random.NextDouble() < (double) this.friendshipTowardFarmer / 10.0 && (defaultProduceIndex == 184 || defaultProduceIndex == 436))
        defaultProduceIndex += 2;
      return new Object(Vector2.Zero, defaultProduceIndex, (string) null, false, false, false, false);
    }

    public void pet()
    {
      if (Game1.timeOfDay >= 1900)
      {
        Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:BarnDweller.cs.386", (object) this.displayName));
      }
      else
      {
        if (this.hasProduce)
          return;
        this.Halt();
        this.sprite.StopAnimation();
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
        }
        this.sprite.UpdateSourceRect();
        if (!this.wasPet)
        {
          this.wasPet = true;
          this.friendshipTowardFarmer = Math.Min(1000, this.friendshipTowardFarmer + 10);
          this.doEmote(20, true);
        }
        else if (this.daysSinceLastFed == 0)
          Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:BarnDweller.cs.387", (object) this.displayName));
        else
          Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:BarnDweller.cs.388", (object) this.displayName));
      }
    }

    public void setRandomPosition()
    {
      string[] strArray = Game1.getLocationFromName("Barn").getMapProperty("ProduceArea").Split(' ');
      int index1 = 0;
      int int32_1 = Convert.ToInt32(strArray[index1]);
      int index2 = 1;
      int int32_2 = Convert.ToInt32(strArray[index2]);
      int index3 = 2;
      int int32_3 = Convert.ToInt32(strArray[index3]);
      int index4 = 3;
      int int32_4 = Convert.ToInt32(strArray[index4]);
      this.position = new Vector2((float) (Game1.random.Next(int32_1, int32_1 + int32_3) * Game1.tileSize), (float) (Game1.random.Next(int32_2, int32_2 + int32_4) * Game1.tileSize));
      int num = 0;
      while (this.doesIntersectAnotherBarnDweller())
      {
        this.faceDirection(Game1.random.Next(4));
        this.position = new Vector2((float) (Game1.random.Next(int32_1, int32_1 + int32_3) * Game1.tileSize), (float) (Game1.random.Next(int32_2, int32_2 + int32_4) * Game1.tileSize));
        ++num;
        if (num > 5)
          break;
      }
    }

    public bool doesIntersectAnotherBarnDweller()
    {
      for (int index = 0; index < Game1.player.barnDwellers.Count; ++index)
      {
        if (!Game1.player.barnDwellers[index].name.Equals(this.name) && this.GetBoundingBox().Intersects(Game1.player.barnDwellers[index].GetBoundingBox()))
          return true;
      }
      return false;
    }

    public new void update(GameTime time, GameLocation location)
    {
      if (this.isEmoting)
        this.updateEmote(time);
      if (Game1.timeOfDay >= 1900)
      {
        this.facingDirection = 2;
        this.sprite.SourceRect = new Rectangle(Game1.tileSize * 4, 0, Game1.tileSize, Game1.tileSize * 3 / 2);
        if (this.isEmoting || Game1.random.NextDouble() >= 0.002)
          return;
        this.doEmote(24, true);
      }
      else
      {
        if (Game1.random.NextDouble() < 0.002 && this.age >= this.ageWhenMature && !Game1.eventUp && (!Game1.currentLocation.name.Equals("Forest") || Game1.random.NextDouble() < 0.001))
          Game1.playSound(this.sound);
        if (Game1.random.NextDouble() < 0.007)
        {
          int direction = Game1.random.Next(5);
          if (direction != (this.facingDirection + 2) % 4)
          {
            if (direction < 4)
            {
              int facingDirection = this.facingDirection;
              this.faceDirection(direction);
              if (location.isCollidingPosition(this.nextPosition(direction), Game1.viewport, (Character) this))
              {
                this.faceDirection(facingDirection);
                return;
              }
            }
            switch (direction)
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
        if (this.isMoving() && Game1.random.NextDouble() < 0.014)
        {
          this.Halt();
          this.sprite.StopAnimation();
        }
        if (this.moveUp)
        {
          if (!location.isCollidingPosition(this.nextPosition(0), Game1.viewport, (Character) this))
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
          if (!location.isCollidingPosition(this.nextPosition(1), Game1.viewport, (Character) this))
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
          if (!location.isCollidingPosition(this.nextPosition(2), Game1.viewport, (Character) this))
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
        else if (this.moveLeft)
        {
          if (!location.isCollidingPosition(this.nextPosition(3), Game1.viewport, (Character) this))
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
        this.sprite.UpdateSourceRect();
      }
    }
  }
}
