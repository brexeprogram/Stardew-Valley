// Decompiled with JetBrains decompiler
// Type: StardewValley.Monsters.Monster
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace StardewValley.Monsters
{
  public class Monster : NPC
  {
    public int slipperiness = 2;
    public List<int> objectsToDrop = new List<int>();
    protected int defaultAnimationInterval = 175;
    public const int defaultInvincibleCountdown = 450;
    public const int seekPlayerIterationLimit = 80;
    public int damageToFarmer;
    public int health;
    public int maxHealth;
    public int coinsToDrop;
    public int durationOfRandomMovements;
    public int resilience;
    public int experienceGained;
    public double jitteriness;
    public double missChance;
    public bool isGlider;
    public bool mineMonster;
    public bool hasSpecialItem;
    protected int skipHorizontal;
    protected int invincibleCountdown;
    private bool skipHorizontalUp;
    [XmlIgnore]
    public bool focusedOnFarmers;
    [XmlIgnore]
    public bool wildernessFarmMonster;
    protected Monster.collisionBehavior onCollision;
    private int slideAnimationTimer;

    public Monster()
    {
    }

    public Monster(string name, Vector2 position)
      : this(name, position, 2)
    {
      this.breather = false;
    }

    public virtual List<Item> getExtraDropItems()
    {
      return new List<Item>();
    }

    public override bool withinPlayerThreshold()
    {
      if (!this.focusedOnFarmers)
        return this.withinPlayerThreshold(this.moveTowardPlayerThreshold);
      return true;
    }

    public override bool IsMonster
    {
      get
      {
        return true;
      }
    }

    public Monster(string name, Vector2 position, int facingDir)
      : base(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Monsters\\" + name)), position, facingDir, name, (LocalizedContentManager) null)
    {
      this.parseMonsterInfo(name);
      this.breather = false;
    }

    public virtual void drawAboveAllLayers(SpriteBatch b)
    {
    }

    public override void draw(SpriteBatch b)
    {
      if (this.isGlider)
        return;
      base.draw(b);
    }

    public bool isInvincible()
    {
      return this.invincibleCountdown > 0;
    }

    public void setInvincibleCountdown(int time)
    {
      this.invincibleCountdown = time;
      this.startGlowing(new Color((int) byte.MaxValue, 0, 0), false, 0.25f);
      this.glowingTransparency = 1f;
    }

    protected void parseMonsterInfo(string name)
    {
      string[] strArray1 = Game1.content.Load<Dictionary<string, string>>("Data\\Monsters")[name].Split('/');
      this.health = Convert.ToInt32(strArray1[0]);
      this.maxHealth = this.health;
      this.damageToFarmer = Convert.ToInt32(strArray1[1]);
      this.coinsToDrop = Game1.random.Next(Convert.ToInt32(strArray1[2]), Convert.ToInt32(strArray1[3]) + 1);
      this.isGlider = Convert.ToBoolean(strArray1[4]);
      this.durationOfRandomMovements = Convert.ToInt32(strArray1[5]);
      string[] strArray2 = strArray1[6].Split(' ');
      this.objectsToDrop.Clear();
      int index = 0;
      while (index < strArray2.Length)
      {
        if (Game1.random.NextDouble() < Convert.ToDouble(strArray2[index + 1]))
          this.objectsToDrop.Add(Convert.ToInt32(strArray2[index]));
        index += 2;
      }
      this.resilience = Convert.ToInt32(strArray1[7]);
      this.jitteriness = Convert.ToDouble(strArray1[8]);
      this.willDestroyObjectsUnderfoot = false;
      this.moveTowardPlayer(Convert.ToInt32(strArray1[9]));
      this.speed = Convert.ToInt32(strArray1[10]);
      this.missChance = Convert.ToDouble(strArray1[11]);
      this.mineMonster = Convert.ToBoolean(strArray1[12]);
      if (Game1.player.timesReachedMineBottom >= 1 && this.mineMonster)
      {
        this.resilience = this.resilience * 2;
        if (Game1.random.NextDouble() < 0.1)
          this.addedSpeed = 1;
        this.missChance = this.missChance * 2.0;
        this.health = this.health + Game1.random.Next(0, this.health);
        this.damageToFarmer = this.damageToFarmer + Game1.random.Next(0, this.damageToFarmer);
        this.coinsToDrop = this.coinsToDrop + Game1.random.Next(0, this.coinsToDrop + 1);
        if (Game1.random.NextDouble() < 0.008)
          this.objectsToDrop.Add(Game1.random.NextDouble() < 0.5 ? 72 : 74);
      }
      try
      {
        this.experienceGained = Convert.ToInt32(strArray1[13]);
      }
      catch (Exception ex)
      {
        this.experienceGained = 1;
      }
      if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.en)
        return;
      this.displayName = strArray1[strArray1.Length - 1];
    }

    public override void reloadSprite()
    {
      this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Monsters\\" + this.name), 0, 16, 16);
    }

    public virtual void shedChunks(int number)
    {
      this.shedChunks(number, 0.75f);
    }

    public virtual void shedChunks(int number, float scale)
    {
      if (this.sprite.Texture.Height <= this.sprite.getHeight() * 4)
        return;
      GameLocation currentLocation = Game1.currentLocation;
      Texture2D texture = this.sprite.Texture;
      Microsoft.Xna.Framework.Rectangle sourcerectangle = new Microsoft.Xna.Framework.Rectangle(0, this.sprite.getHeight() * 4 + 16, 16, 16);
      int sizeOfSourceRectSquares = 8;
      Microsoft.Xna.Framework.Rectangle boundingBox = this.GetBoundingBox();
      int x = boundingBox.Center.X;
      boundingBox = this.GetBoundingBox();
      int y1 = boundingBox.Center.Y;
      int numberOfChunks = number;
      int y2 = (int) this.getTileLocation().Y;
      Color white = Color.White;
      double num = 1.0 * (double) Game1.pixelZoom * (double) scale;
      Game1.createRadialDebris(currentLocation, texture, sourcerectangle, sizeOfSourceRectSquares, x, y1, numberOfChunks, y2, white, (float) num);
    }

    public virtual void deathAnimation()
    {
      this.shedChunks(Game1.random.Next(4, 9), 0.75f);
    }

    public virtual int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
    {
      return this.takeDamage(damage, xTrajectory, yTrajectory, isBomb, addedPrecision, "hitEnemy");
    }

    public int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision, string hitSound)
    {
      int num = Math.Max(1, damage - this.resilience);
      this.slideAnimationTimer = 0;
      if (Game1.random.NextDouble() < this.missChance - this.missChance * addedPrecision)
      {
        num = -1;
      }
      else
      {
        this.health = this.health - num;
        Game1.playSound(hitSound);
        this.setTrajectory(xTrajectory / 3, yTrajectory / 3);
        if (this.health <= 0)
          this.deathAnimation();
      }
      return num;
    }

    public virtual void behaviorAtGameTick(GameTime time)
    {
      if ((double) this.timeBeforeAIMovementAgain > 0.0)
        this.timeBeforeAIMovementAgain = this.timeBeforeAIMovementAgain - (float) time.ElapsedGameTime.Milliseconds;
      if (!Game1.player.isRafting || !this.withinPlayerThreshold(4))
        return;
      Microsoft.Xna.Framework.Rectangle boundingBox = Game1.player.GetBoundingBox();
      int y1 = boundingBox.Center.Y;
      boundingBox = this.GetBoundingBox();
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
    }

    public virtual bool passThroughCharacters()
    {
      return false;
    }

    public override bool shouldCollideWithBuildingLayer(GameLocation location)
    {
      return true;
    }

    public override void update(GameTime time, GameLocation location)
    {
      if (this.invincibleCountdown > 0)
      {
        this.invincibleCountdown = this.invincibleCountdown - time.ElapsedGameTime.Milliseconds;
        if (this.invincibleCountdown <= 0)
          this.stopGlowing();
      }
      if (!location.Equals((object) Game1.currentLocation))
        return;
      if (!Game1.player.isRafting || !this.withinPlayerThreshold(4))
        base.update(time, location);
      this.behaviorAtGameTick(time);
      if (this.controller != null && this.withinPlayerThreshold(3))
        this.controller = (PathFindController) null;
      if (!this.isGlider && ((double) this.position.X < 0.0 || (double) this.position.X > (double) (location.map.GetLayer("Back").LayerWidth * Game1.tileSize) || ((double) this.position.Y < 0.0 || (double) this.position.Y > (double) (location.map.GetLayer("Back").LayerHeight * Game1.tileSize))))
      {
        location.characters.Remove((NPC) this);
      }
      else
      {
        if (!this.isGlider || (double) this.position.X >= -2000.0)
          return;
        this.health = -500;
      }
    }

    private bool doHorizontalMovement(GameLocation location)
    {
      bool flag = false;
      if ((double) this.position.X > (double) Game1.player.position.X + (double) (Game1.pixelZoom * 2) || this.skipHorizontal > 0 && Game1.player.getStandingX() < this.getStandingX() - Game1.pixelZoom * 2)
      {
        this.SetMovingOnlyLeft();
        if (!location.isCollidingPosition(this.nextPosition(3), Game1.viewport, false, this.damageToFarmer, this.isGlider, (Character) this))
        {
          this.MovePosition(Game1.currentGameTime, Game1.viewport, location);
          flag = true;
        }
        else
        {
          this.faceDirection(3);
          if (this.durationOfRandomMovements > 0 && Game1.random.NextDouble() < this.jitteriness)
          {
            if (Game1.random.NextDouble() < 0.5)
              this.tryToMoveInDirection(2, false, this.damageToFarmer, this.isGlider);
            else
              this.tryToMoveInDirection(0, false, this.damageToFarmer, this.isGlider);
            this.timeBeforeAIMovementAgain = (float) this.durationOfRandomMovements;
          }
        }
      }
      else if ((double) this.position.X < (double) Game1.player.position.X - (double) (Game1.pixelZoom * 2))
      {
        this.SetMovingOnlyRight();
        if (!location.isCollidingPosition(this.nextPosition(1), Game1.viewport, false, this.damageToFarmer, this.isGlider, (Character) this))
        {
          this.MovePosition(Game1.currentGameTime, Game1.viewport, location);
          flag = true;
        }
        else
        {
          this.faceDirection(1);
          if (this.durationOfRandomMovements > 0 && Game1.random.NextDouble() < this.jitteriness)
          {
            if (Game1.random.NextDouble() < 0.5)
              this.tryToMoveInDirection(2, false, this.damageToFarmer, this.isGlider);
            else
              this.tryToMoveInDirection(0, false, this.damageToFarmer, this.isGlider);
            this.timeBeforeAIMovementAgain = (float) this.durationOfRandomMovements;
          }
        }
      }
      else
      {
        this.faceGeneralDirection(Game1.player.getStandingPosition(), 0);
        this.setMovingInFacingDirection();
        this.skipHorizontal = 500;
      }
      return flag;
    }

    private void checkHorizontalMovement(ref bool success, ref bool setMoving, ref bool scootSuccess, Farmer who, GameLocation location)
    {
      if ((double) who.position.X > (double) this.position.X + (double) (Game1.tileSize / 4))
      {
        this.SetMovingOnlyRight();
        setMoving = true;
        if (!location.isCollidingPosition(this.nextPosition(1), Game1.viewport, false, this.damageToFarmer, this.isGlider, (Character) this))
        {
          success = true;
        }
        else
        {
          this.MovePosition(Game1.currentGameTime, Game1.viewport, location);
          if (!this.position.Equals(this.lastPosition))
            scootSuccess = true;
        }
      }
      if (success || (double) who.position.X >= (double) this.position.X - (double) (Game1.tileSize / 4))
        return;
      this.SetMovingOnlyLeft();
      setMoving = true;
      if (!location.isCollidingPosition(this.nextPosition(3), Game1.viewport, false, this.damageToFarmer, this.isGlider, (Character) this))
      {
        success = true;
      }
      else
      {
        this.MovePosition(Game1.currentGameTime, Game1.viewport, location);
        if (this.position.Equals(this.lastPosition))
          return;
        scootSuccess = true;
      }
    }

    private void checkVerticalMovement(ref bool success, ref bool setMoving, ref bool scootSuccess, Farmer who, GameLocation location)
    {
      if (!success && (double) who.position.Y < (double) this.position.Y - (double) (Game1.tileSize / 4))
      {
        this.SetMovingOnlyUp();
        setMoving = true;
        if (!location.isCollidingPosition(this.nextPosition(0), Game1.viewport, false, this.damageToFarmer, this.isGlider, (Character) this))
        {
          success = true;
        }
        else
        {
          this.MovePosition(Game1.currentGameTime, Game1.viewport, location);
          if (!this.position.Equals(this.lastPosition))
            scootSuccess = true;
        }
      }
      if (success || (double) who.position.Y <= (double) this.position.Y + (double) (Game1.tileSize / 4))
        return;
      this.SetMovingOnlyDown();
      setMoving = true;
      if (!location.isCollidingPosition(this.nextPosition(2), Game1.viewport, false, this.damageToFarmer, this.isGlider, (Character) this))
      {
        success = true;
      }
      else
      {
        this.MovePosition(Game1.currentGameTime, Game1.viewport, location);
        if (this.position.Equals(this.lastPosition))
          return;
        scootSuccess = true;
      }
    }

    public override void updateMovement(GameLocation location, GameTime time)
    {
      if (this.IsWalkingTowardPlayer)
      {
        if ((this.moveTowardPlayerThreshold == -1 || this.withinPlayerThreshold()) && ((double) this.timeBeforeAIMovementAgain <= 0.0 && this.IsMonster) && (!this.isGlider && !location.map.GetLayer("Back").Tiles[(int) Game1.player.getTileLocation().X, (int) Game1.player.getTileLocation().Y].Properties.ContainsKey("NPCBarrier")))
        {
          if (this.skipHorizontal <= 0)
          {
            Farmer inCurrentLocation = Utility.getNearestFarmerInCurrentLocation(this.getTileLocation());
            if (this.lastPosition.Equals(this.position) && Game1.random.NextDouble() < 0.001)
            {
              switch (this.facingDirection)
              {
                case 0:
                case 2:
                  if (Game1.random.NextDouble() < 0.5)
                  {
                    this.SetMovingOnlyRight();
                    break;
                  }
                  this.SetMovingOnlyLeft();
                  break;
                case 1:
                case 3:
                  if (Game1.random.NextDouble() < 0.5)
                  {
                    this.SetMovingOnlyUp();
                    break;
                  }
                  this.SetMovingOnlyDown();
                  break;
              }
              this.skipHorizontal = 700;
              return;
            }
            bool success = false;
            bool setMoving = false;
            bool scootSuccess = false;
            if ((double) this.lastPosition.X == (double) this.position.X)
            {
              this.checkHorizontalMovement(ref success, ref setMoving, ref scootSuccess, inCurrentLocation, location);
              this.checkVerticalMovement(ref success, ref setMoving, ref scootSuccess, inCurrentLocation, location);
            }
            else
            {
              this.checkVerticalMovement(ref success, ref setMoving, ref scootSuccess, inCurrentLocation, location);
              this.checkHorizontalMovement(ref success, ref setMoving, ref scootSuccess, inCurrentLocation, location);
            }
            if (!success && !setMoving)
            {
              this.Halt();
              this.faceGeneralDirection(inCurrentLocation.getStandingPosition(), 0);
            }
            if (success)
              this.skipHorizontal = 500;
            if (scootSuccess)
              return;
          }
          else
            this.skipHorizontal = this.skipHorizontal - time.ElapsedGameTime.Milliseconds;
        }
      }
      else
        this.defaultMovementBehavior(time);
      this.MovePosition(time, Game1.viewport, location);
      if (!this.position.Equals(this.lastPosition) || !this.IsWalkingTowardPlayer || !this.withinPlayerThreshold())
        return;
      this.noMovementProgressNearPlayerBehavior();
    }

    public virtual void noMovementProgressNearPlayerBehavior()
    {
      this.Halt();
      this.faceGeneralDirection(Utility.getNearestFarmerInCurrentLocation(this.getTileLocation()).getStandingPosition(), 0);
    }

    public virtual void defaultMovementBehavior(GameTime time)
    {
      if (Game1.random.NextDouble() >= this.jitteriness * 1.8 || this.skipHorizontal > 0)
        return;
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

    public override void MovePosition(GameTime time, xTile.Dimensions.Rectangle viewport, GameLocation currentLocation)
    {
      this.lastPosition = this.position;
      if ((double) this.xVelocity != 0.0 || (double) this.yVelocity != 0.0)
      {
        if (double.IsNaN((double) this.xVelocity) || double.IsNaN((double) this.yVelocity))
        {
          this.xVelocity = 0.0f;
          this.yVelocity = 0.0f;
        }
        Microsoft.Xna.Framework.Rectangle boundingBox = this.GetBoundingBox();
        boundingBox.X += (int) this.xVelocity;
        boundingBox.Y -= (int) this.yVelocity;
        if (!currentLocation.isCollidingPosition(boundingBox, viewport, false, this.damageToFarmer, this.isGlider, (Character) this))
        {
          this.position.X += this.xVelocity;
          this.position.Y -= this.yVelocity;
          if (this.slipperiness < 1000)
          {
            this.xVelocity = this.xVelocity - this.xVelocity / (float) this.slipperiness;
            this.yVelocity = this.yVelocity - this.yVelocity / (float) this.slipperiness;
            if ((double) Math.Abs(this.xVelocity) <= 0.0500000007450581)
              this.xVelocity = 0.0f;
            if ((double) Math.Abs(this.yVelocity) <= 0.0500000007450581)
              this.yVelocity = 0.0f;
          }
          if (!this.isGlider && this.invincibleCountdown > 0)
          {
            this.slideAnimationTimer = this.slideAnimationTimer - time.ElapsedGameTime.Milliseconds;
            if (this.slideAnimationTimer < 0 && ((double) Math.Abs(this.xVelocity) >= 3.0 || (double) Math.Abs(this.yVelocity) >= 3.0))
            {
              this.slideAnimationTimer = 100 - (int) ((double) Math.Abs(this.xVelocity) * 2.0 + (double) Math.Abs(this.yVelocity) * 2.0);
              currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(6, this.getStandingPosition() + new Vector2((float) (-Game1.tileSize / 2), (float) (-Game1.tileSize / 2)), Color.White * 0.75f, 8, Game1.random.NextDouble() < 0.5, 20f, 0, -1, -1f, -1, 0)
              {
                scale = 0.75f
              });
            }
          }
        }
        else if (this.isGlider || this.slipperiness >= 8)
        {
          bool[] flagArray = Utility.horizontalOrVerticalCollisionDirections(boundingBox, (Character) this, false);
          int index1 = 0;
          if (flagArray[index1])
          {
            this.xVelocity = -this.xVelocity;
            this.position.X += (float) Math.Sign(this.xVelocity);
            this.rotation = this.rotation + (float) (Math.PI + (double) Game1.random.Next(-10, 11) * Math.PI / 500.0);
          }
          int index2 = 1;
          if (flagArray[index2])
          {
            this.yVelocity = -this.yVelocity;
            this.position.Y -= (float) Math.Sign(this.yVelocity);
            this.rotation = this.rotation + (float) (Math.PI + (double) Game1.random.Next(-10, 11) * Math.PI / 500.0);
          }
          if (this.slipperiness < 1000)
          {
            this.xVelocity = this.xVelocity - (float) ((double) this.xVelocity / (double) this.slipperiness / 4.0);
            this.yVelocity = this.yVelocity - (float) ((double) this.yVelocity / (double) this.slipperiness / 4.0);
            if ((double) Math.Abs(this.xVelocity) <= 0.0500000007450581)
              this.xVelocity = 0.0f;
            if ((double) Math.Abs(this.yVelocity) <= 0.0509999990463257)
              this.yVelocity = 0.0f;
          }
        }
        else
        {
          this.xVelocity = this.xVelocity - this.xVelocity / (float) this.slipperiness;
          this.yVelocity = this.yVelocity - this.yVelocity / (float) this.slipperiness;
          if ((double) Math.Abs(this.xVelocity) <= 0.0500000007450581)
            this.xVelocity = 0.0f;
          if ((double) Math.Abs(this.yVelocity) <= 0.0500000007450581)
            this.yVelocity = 0.0f;
        }
        if (this.isGlider)
          return;
      }
      if (this.moveUp)
      {
        if (!currentLocation.isCollidingPosition(this.nextPosition(0), viewport, false, this.damageToFarmer, this.isGlider, (Character) this) || this.isCharging)
        {
          this.position.Y -= (float) (this.speed + this.addedSpeed);
          if (!this.ignoreMovementAnimations)
            this.sprite.AnimateUp(time, 0, "");
          this.facingDirection = 0;
          this.faceDirection(0);
        }
        else
        {
          Microsoft.Xna.Framework.Rectangle position = this.nextPosition(0);
          position.Width /= 4;
          bool flag1 = currentLocation.isCollidingPosition(position, viewport, false, this.damageToFarmer, this.isGlider, (Character) this);
          position.X += position.Width * 3;
          bool flag2 = currentLocation.isCollidingPosition(position, viewport, false, this.damageToFarmer, this.isGlider, (Character) this);
          TimeSpan elapsedGameTime;
          if (flag1 && !flag2 && !currentLocation.isCollidingPosition(this.nextPosition(1), viewport, false, this.damageToFarmer, this.isGlider, (Character) this))
          {
            // ISSUE: explicit reference operation
            // ISSUE: variable of a reference type
            float& local = @this.position.X;
            // ISSUE: explicit reference operation
            double num1 = (double) ^local;
            double speed = (double) this.speed;
            elapsedGameTime = time.ElapsedGameTime;
            double num2 = (double) elapsedGameTime.Milliseconds / 64.0;
            double num3 = speed * num2;
            double num4 = num1 + num3;
            // ISSUE: explicit reference operation
            ^local = (float) num4;
          }
          else if (flag2 && !flag1 && !currentLocation.isCollidingPosition(this.nextPosition(3), viewport, false, this.damageToFarmer, this.isGlider, (Character) this))
          {
            // ISSUE: explicit reference operation
            // ISSUE: variable of a reference type
            float& local = @this.position.X;
            // ISSUE: explicit reference operation
            double num1 = (double) ^local;
            double speed = (double) this.speed;
            elapsedGameTime = time.ElapsedGameTime;
            double num2 = (double) elapsedGameTime.Milliseconds / 64.0;
            double num3 = speed * num2;
            double num4 = num1 - num3;
            // ISSUE: explicit reference operation
            ^local = (float) num4;
          }
          if (!currentLocation.isTilePassable(this.nextPosition(0), viewport) || !this.willDestroyObjectsUnderfoot)
            this.Halt();
          else if (this.willDestroyObjectsUnderfoot)
          {
            Vector2 vector2 = new Vector2((float) (this.getStandingX() / Game1.tileSize), (float) (this.getStandingY() / Game1.tileSize - 1));
            if (currentLocation.characterDestroyObjectWithinRectangle(this.nextPosition(0), true))
            {
              Game1.playSound("stoneCrack");
              this.position.Y -= (float) (this.speed + this.addedSpeed);
            }
            else
            {
              int blockedInterval = this.blockedInterval;
              elapsedGameTime = time.ElapsedGameTime;
              int milliseconds = elapsedGameTime.Milliseconds;
              this.blockedInterval = blockedInterval + milliseconds;
            }
          }
          if (this.onCollision != null)
            this.onCollision(currentLocation);
        }
      }
      else if (this.moveRight)
      {
        if (!currentLocation.isCollidingPosition(this.nextPosition(1), viewport, false, this.damageToFarmer, this.isGlider, (Character) this) || this.isCharging)
        {
          this.position.X += (float) (this.speed + this.addedSpeed);
          if (!this.ignoreMovementAnimations)
            this.sprite.AnimateRight(time, 0, "");
          this.facingDirection = 1;
          this.faceDirection(1);
        }
        else
        {
          Microsoft.Xna.Framework.Rectangle position = this.nextPosition(1);
          position.Height /= 4;
          bool flag1 = currentLocation.isCollidingPosition(position, viewport, false, this.damageToFarmer, this.isGlider, (Character) this);
          position.Y += position.Height * 3;
          bool flag2 = currentLocation.isCollidingPosition(position, viewport, false, this.damageToFarmer, this.isGlider, (Character) this);
          TimeSpan elapsedGameTime;
          if (flag1 && !flag2 && !currentLocation.isCollidingPosition(this.nextPosition(2), viewport, false, this.damageToFarmer, this.isGlider, (Character) this))
          {
            // ISSUE: explicit reference operation
            // ISSUE: variable of a reference type
            float& local = @this.position.Y;
            // ISSUE: explicit reference operation
            double num1 = (double) ^local;
            double speed = (double) this.speed;
            elapsedGameTime = time.ElapsedGameTime;
            double num2 = (double) elapsedGameTime.Milliseconds / 64.0;
            double num3 = speed * num2;
            double num4 = num1 + num3;
            // ISSUE: explicit reference operation
            ^local = (float) num4;
          }
          else if (flag2 && !flag1 && !currentLocation.isCollidingPosition(this.nextPosition(0), viewport, false, this.damageToFarmer, this.isGlider, (Character) this))
          {
            // ISSUE: explicit reference operation
            // ISSUE: variable of a reference type
            float& local = @this.position.Y;
            // ISSUE: explicit reference operation
            double num1 = (double) ^local;
            double speed = (double) this.speed;
            elapsedGameTime = time.ElapsedGameTime;
            double num2 = (double) elapsedGameTime.Milliseconds / 64.0;
            double num3 = speed * num2;
            double num4 = num1 - num3;
            // ISSUE: explicit reference operation
            ^local = (float) num4;
          }
          if (!currentLocation.isTilePassable(this.nextPosition(1), viewport) || !this.willDestroyObjectsUnderfoot)
            this.Halt();
          else if (this.willDestroyObjectsUnderfoot)
          {
            Vector2 vector2 = new Vector2((float) (this.getStandingX() / Game1.tileSize + 1), (float) (this.getStandingY() / Game1.tileSize));
            if (currentLocation.characterDestroyObjectWithinRectangle(this.nextPosition(1), true))
            {
              Game1.playSound("stoneCrack");
              this.position.X += (float) (this.speed + this.addedSpeed);
            }
            else
            {
              int blockedInterval = this.blockedInterval;
              elapsedGameTime = time.ElapsedGameTime;
              int milliseconds = elapsedGameTime.Milliseconds;
              this.blockedInterval = blockedInterval + milliseconds;
            }
          }
          if (this.onCollision != null)
            this.onCollision(currentLocation);
        }
      }
      else if (this.moveDown)
      {
        if (!currentLocation.isCollidingPosition(this.nextPosition(2), viewport, false, this.damageToFarmer, this.isGlider, (Character) this) || this.isCharging)
        {
          this.position.Y += (float) (this.speed + this.addedSpeed);
          if (!this.ignoreMovementAnimations)
            this.sprite.AnimateDown(time, 0, "");
          this.facingDirection = 2;
          this.faceDirection(2);
        }
        else
        {
          Microsoft.Xna.Framework.Rectangle position = this.nextPosition(2);
          position.Width /= 4;
          bool flag1 = currentLocation.isCollidingPosition(position, viewport, false, this.damageToFarmer, this.isGlider, (Character) this);
          position.X += position.Width * 3;
          bool flag2 = currentLocation.isCollidingPosition(position, viewport, false, this.damageToFarmer, this.isGlider, (Character) this);
          TimeSpan elapsedGameTime;
          if (flag1 && !flag2 && !currentLocation.isCollidingPosition(this.nextPosition(1), viewport, false, this.damageToFarmer, this.isGlider, (Character) this))
          {
            // ISSUE: explicit reference operation
            // ISSUE: variable of a reference type
            float& local = @this.position.X;
            // ISSUE: explicit reference operation
            double num1 = (double) ^local;
            double speed = (double) this.speed;
            elapsedGameTime = time.ElapsedGameTime;
            double num2 = (double) elapsedGameTime.Milliseconds / 64.0;
            double num3 = speed * num2;
            double num4 = num1 + num3;
            // ISSUE: explicit reference operation
            ^local = (float) num4;
          }
          else if (flag2 && !flag1 && !currentLocation.isCollidingPosition(this.nextPosition(3), viewport, false, this.damageToFarmer, this.isGlider, (Character) this))
          {
            // ISSUE: explicit reference operation
            // ISSUE: variable of a reference type
            float& local = @this.position.X;
            // ISSUE: explicit reference operation
            double num1 = (double) ^local;
            double speed = (double) this.speed;
            elapsedGameTime = time.ElapsedGameTime;
            double num2 = (double) elapsedGameTime.Milliseconds / 64.0;
            double num3 = speed * num2;
            double num4 = num1 - num3;
            // ISSUE: explicit reference operation
            ^local = (float) num4;
          }
          if (!currentLocation.isTilePassable(this.nextPosition(2), viewport) || !this.willDestroyObjectsUnderfoot)
            this.Halt();
          else if (this.willDestroyObjectsUnderfoot)
          {
            Vector2 vector2 = new Vector2((float) (this.getStandingX() / Game1.tileSize), (float) (this.getStandingY() / Game1.tileSize + 1));
            if (currentLocation.characterDestroyObjectWithinRectangle(this.nextPosition(2), true))
            {
              Game1.playSound("stoneCrack");
              this.position.Y += (float) (this.speed + this.addedSpeed);
            }
            else
            {
              int blockedInterval = this.blockedInterval;
              elapsedGameTime = time.ElapsedGameTime;
              int milliseconds = elapsedGameTime.Milliseconds;
              this.blockedInterval = blockedInterval + milliseconds;
            }
          }
          if (this.onCollision != null)
            this.onCollision(currentLocation);
        }
      }
      else if (this.moveLeft)
      {
        if (!currentLocation.isCollidingPosition(this.nextPosition(3), viewport, false, this.damageToFarmer, this.isGlider, (Character) this) || this.isCharging)
        {
          this.position.X -= (float) (this.speed + this.addedSpeed);
          this.facingDirection = 3;
          if (!this.ignoreMovementAnimations)
            this.sprite.AnimateLeft(time, 0, "");
          this.faceDirection(3);
        }
        else
        {
          Microsoft.Xna.Framework.Rectangle position = this.nextPosition(3);
          position.Height /= 4;
          bool flag1 = currentLocation.isCollidingPosition(position, viewport, false, this.damageToFarmer, this.isGlider, (Character) this);
          position.Y += position.Height * 3;
          bool flag2 = currentLocation.isCollidingPosition(position, viewport, false, this.damageToFarmer, this.isGlider, (Character) this);
          TimeSpan elapsedGameTime;
          if (flag1 && !flag2 && !currentLocation.isCollidingPosition(this.nextPosition(2), viewport, false, this.damageToFarmer, this.isGlider, (Character) this))
          {
            // ISSUE: explicit reference operation
            // ISSUE: variable of a reference type
            float& local = @this.position.Y;
            // ISSUE: explicit reference operation
            double num1 = (double) ^local;
            double speed = (double) this.speed;
            elapsedGameTime = time.ElapsedGameTime;
            double num2 = (double) elapsedGameTime.Milliseconds / 64.0;
            double num3 = speed * num2;
            double num4 = num1 + num3;
            // ISSUE: explicit reference operation
            ^local = (float) num4;
          }
          else if (flag2 && !flag1 && !currentLocation.isCollidingPosition(this.nextPosition(0), viewport, false, this.damageToFarmer, this.isGlider, (Character) this))
          {
            // ISSUE: explicit reference operation
            // ISSUE: variable of a reference type
            float& local = @this.position.Y;
            // ISSUE: explicit reference operation
            double num1 = (double) ^local;
            double speed = (double) this.speed;
            elapsedGameTime = time.ElapsedGameTime;
            double num2 = (double) elapsedGameTime.Milliseconds / 64.0;
            double num3 = speed * num2;
            double num4 = num1 - num3;
            // ISSUE: explicit reference operation
            ^local = (float) num4;
          }
          if (!currentLocation.isTilePassable(this.nextPosition(3), viewport) || !this.willDestroyObjectsUnderfoot)
            this.Halt();
          else if (this.willDestroyObjectsUnderfoot)
          {
            Vector2 vector2 = new Vector2((float) (this.getStandingX() / Game1.tileSize - 1), (float) (this.getStandingY() / Game1.tileSize));
            if (currentLocation.characterDestroyObjectWithinRectangle(this.nextPosition(3), true))
            {
              Game1.playSound("stoneCrack");
              this.position.X -= (float) (this.speed + this.addedSpeed);
            }
            else
            {
              int blockedInterval = this.blockedInterval;
              elapsedGameTime = time.ElapsedGameTime;
              int milliseconds = elapsedGameTime.Milliseconds;
              this.blockedInterval = blockedInterval + milliseconds;
            }
          }
          if (this.onCollision != null)
            this.onCollision(currentLocation);
        }
      }
      else if (!this.ignoreMovementAnimations)
      {
        if (this.moveUp)
          this.sprite.AnimateUp(time, 0, "");
        else if (this.moveRight)
          this.sprite.AnimateRight(time, 0, "");
        else if (this.moveDown)
          this.sprite.AnimateDown(time, 0, "");
        else if (this.moveLeft)
          this.sprite.AnimateLeft(time, 0, "");
      }
      if (!this.ignoreMovementAnimations)
        this.sprite.interval = (float) this.defaultAnimationInterval - (float) (this.speed + this.addedSpeed - 2) * 20f;
      if ((this.blockedInterval < 3000 || (double) this.blockedInterval > 3750.0) && this.blockedInterval >= 5000)
      {
        this.speed = 4;
        this.isCharging = true;
        this.blockedInterval = 0;
      }
      if (this.damageToFarmer <= 0 || Game1.random.NextDouble() >= 0.000333333333333333)
        return;
      if (this.name.Equals("Shadow Guy") && Game1.random.NextDouble() < 0.3)
      {
        if (Game1.random.NextDouble() < 0.5)
          Game1.playSound("grunt");
        else
          Game1.playSound("shadowpeep");
      }
      else
      {
        if (this.name.Equals("Shadow Girl"))
          return;
        if (this.name.Equals("Ghost"))
        {
          Game1.playSound("ghost");
        }
        else
        {
          if (this.name.Contains("Slime"))
            return;
          this.name.Contains("Jelly");
        }
      }
    }

    protected delegate void collisionBehavior(GameLocation location);
  }
}
