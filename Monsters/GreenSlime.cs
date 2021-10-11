// Decompiled with JetBrains decompiler
// Type: StardewValley.Monsters.GreenSlime
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StardewValley.Monsters
{
  public class GreenSlime : Monster
  {
    public static int matingRange = Game1.tileSize * 3;
    public bool cute = true;
    private int readyToJump = -1;
    public int readyToMate = 120000;
    public const float mutationFactor = 0.25f;
    public const int matingInterval = 120000;
    public const int childhoodLength = 120000;
    public const int durationOfMating = 2000;
    public const double chanceToMate = 0.001;
    public bool leftDrift;
    private int matingCountdown;
    private int yOffset;
    private int wagTimer;
    public int ageUntilFullGrown;
    public int animateTimer;
    public int timeSinceLastJump;
    public int specialNumber;
    public bool firstGeneration;
    public Color color;
    private GreenSlime mateToPursue;
    private GreenSlime mateToAvoid;
    private Vector2 facePosition;
    private Vector2 faceTargetPosition;

    public GreenSlime()
    {
    }

    public GreenSlime(Vector2 position)
      : base("Green Slime", position)
    {
      if (Game1.random.NextDouble() < 0.5)
        this.leftDrift = true;
      this.slipperiness = 4;
      this.readyToMate = Game1.random.Next(1000, 120000);
      int num = Game1.random.Next(200, 256);
      this.color = new Color(num / Game1.random.Next(2, 10), Game1.random.Next(180, 256), Game1.random.NextDouble() < 0.1 ? (int) byte.MaxValue : (int) byte.MaxValue - num);
      this.firstGeneration = true;
      this.flip = Game1.random.NextDouble() < 0.5;
      this.cute = Game1.random.NextDouble() < 0.49;
      this.hideShadow = true;
    }

    public GreenSlime(Vector2 position, int mineLevel)
      : base("Green Slime", position)
    {
      this.cute = Game1.random.NextDouble() < 0.49;
      this.flip = Game1.random.NextDouble() < 0.5;
      this.specialNumber = Game1.random.Next(100);
      if (mineLevel < 40)
      {
        this.parseMonsterInfo("Green Slime");
        int g = Game1.random.Next(200, 256);
        this.color = new Color(g / Game1.random.Next(2, 10), g, Game1.random.NextDouble() < 0.01 ? (int) byte.MaxValue : (int) byte.MaxValue - g);
        if (Game1.random.NextDouble() < 0.01 && mineLevel % 5 != 0 && mineLevel % 5 != 1)
        {
          this.color = new Color(205, (int) byte.MaxValue, 0) * 0.7f;
          this.hasSpecialItem = true;
          this.health = this.health * 3;
          this.damageToFarmer = this.damageToFarmer * 2;
        }
        if (Game1.random.NextDouble() < 0.01 && Game1.player.mailReceived.Contains("slimeHutchBuilt"))
          this.objectsToDrop.Add(680);
      }
      else if (mineLevel < 80)
      {
        this.name = "Frost Jelly";
        this.parseMonsterInfo("Frost Jelly");
        int b = Game1.random.Next(200, 256);
        this.color = new Color(Game1.random.NextDouble() < 0.01 ? 180 : b / Game1.random.Next(2, 10), Game1.random.NextDouble() < 0.1 ? (int) byte.MaxValue : (int) byte.MaxValue - b / 3, b);
        if (Game1.random.NextDouble() < 0.01 && mineLevel % 5 != 0 && mineLevel % 5 != 1)
        {
          this.color = new Color(0, 0, 0) * 0.7f;
          this.hasSpecialItem = true;
          this.health = this.health * 3;
          this.damageToFarmer = this.damageToFarmer * 2;
        }
        if (Game1.random.NextDouble() < 0.01 && Game1.player.mailReceived.Contains("slimeHutchBuilt"))
          this.objectsToDrop.Add(413);
      }
      else if (mineLevel > 120)
      {
        this.name = "Sludge";
        this.parseMonsterInfo("Sludge");
        this.color = Color.BlueViolet;
        this.health = this.health * 2;
        int r = (int) this.color.R;
        int g = (int) this.color.G;
        int b = (int) this.color.B;
        int val2_1 = r + Game1.random.Next(-20, 21);
        int val2_2 = g + Game1.random.Next(-20, 21);
        int val2_3 = b + Game1.random.Next(-20, 21);
        this.color.R = (byte) Math.Max(Math.Min((int) byte.MaxValue, val2_1), 0);
        this.color.G = (byte) Math.Max(Math.Min((int) byte.MaxValue, val2_2), 0);
        this.color.B = (byte) Math.Max(Math.Min((int) byte.MaxValue, val2_3), 0);
        while (Game1.random.NextDouble() < 0.08)
          this.objectsToDrop.Add(386);
        if (Game1.random.NextDouble() < 0.009)
          this.objectsToDrop.Add(337);
        if (Game1.random.NextDouble() < 0.01 && Game1.player.mailReceived.Contains("slimeHutchBuilt"))
          this.objectsToDrop.Add(439);
      }
      else
      {
        this.name = "Sludge";
        this.parseMonsterInfo("Sludge");
        int r = Game1.random.Next(200, 256);
        this.color = new Color(r, Game1.random.NextDouble() < 0.01 ? (int) byte.MaxValue : (int) byte.MaxValue - r, r / Game1.random.Next(2, 10));
        if (Game1.random.NextDouble() < 0.01 && mineLevel % 5 != 0 && mineLevel % 5 != 1)
        {
          this.color = new Color(50, 10, 50) * 0.7f;
          this.hasSpecialItem = true;
          this.health = this.health * 3;
          this.damageToFarmer = this.damageToFarmer * 2;
        }
        if (Game1.random.NextDouble() < 0.01 && Game1.player.mailReceived.Contains("slimeHutchBuilt"))
          this.objectsToDrop.Add(437);
      }
      if (this.cute)
      {
        this.health = this.health + this.health / 4;
        this.damageToFarmer = this.damageToFarmer + 1;
      }
      if (Game1.random.NextDouble() < 0.5)
        this.leftDrift = true;
      this.slipperiness = 3;
      this.readyToMate = Game1.random.Next(1000, 120000);
      if (Game1.random.NextDouble() < 0.001)
      {
        this.color = new Color((int) byte.MaxValue, (int) byte.MaxValue, 50);
        this.coinsToDrop = 10;
      }
      this.firstGeneration = true;
      this.hideShadow = true;
    }

    public GreenSlime(Vector2 position, Color color)
      : base("Green Slime", position)
    {
      this.color = color;
      this.firstGeneration = true;
      this.hideShadow = true;
    }

    public override void reloadSprite()
    {
      this.hideShadow = true;
      string name = this.name;
      this.name = "Green Slime";
      base.reloadSprite();
      this.name = name;
      this.sprite.spriteHeight = 24;
      this.sprite.UpdateSourceRect();
    }

    public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
    {
      int num1 = Math.Max(1, damage - this.resilience);
      if (Game1.random.NextDouble() < this.missChance - this.missChance * addedPrecision)
      {
        num1 = -1;
      }
      else
      {
        if (Game1.random.NextDouble() < 0.025 && this.cute)
        {
          if (!this.focusedOnFarmers)
          {
            this.damageToFarmer = this.damageToFarmer + this.damageToFarmer / 2;
            this.shake(1000);
          }
          this.focusedOnFarmers = true;
        }
        this.slipperiness = 3;
        this.health = this.health - num1;
        this.setTrajectory(xTrajectory, yTrajectory);
        Game1.playSound("slimeHit");
        this.readyToJump = -1;
        this.IsWalkingTowardPlayer = true;
        if (this.health <= 0)
        {
          Game1.playSound("slimedead");
          ++Game1.stats.SlimesKilled;
          if (this.mateToPursue != null)
            this.mateToPursue.mateToAvoid = (GreenSlime) null;
          if (this.mateToAvoid != null)
            this.mateToAvoid.mateToPursue = (GreenSlime) null;
          if ((int) Game1.gameMode == 3 && (double) this.scale > 1.79999995231628)
          {
            this.health = 10;
            int num2 = (double) this.scale > 1.79999995231628 ? Game1.random.Next(3, 5) : 1;
            this.scale = this.scale * 0.6666667f;
            for (int index = 0; index < num2; ++index)
            {
              Game1.currentLocation.characters.Add((NPC) new GreenSlime(this.position + new Vector2((float) (index * this.GetBoundingBox().Width), 0.0f), Game1.mine.mineLevel));
              Game1.currentLocation.characters[Game1.currentLocation.characters.Count - 1].setTrajectory(xTrajectory + Game1.random.Next(-20, 20), yTrajectory + Game1.random.Next(-20, 20));
              Game1.currentLocation.characters[Game1.currentLocation.characters.Count - 1].willDestroyObjectsUnderfoot = false;
              Game1.currentLocation.characters[Game1.currentLocation.characters.Count - 1].moveTowardPlayer(4);
              Game1.currentLocation.characters[Game1.currentLocation.characters.Count - 1].scale = (float) (0.75 + (double) Game1.random.Next(-5, 10) / 100.0);
            }
          }
          else
          {
            Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(44, this.position, this.color * 0.66f, 10, false, 100f, 0, -1, -1f, -1, 0)
            {
              interval = 70f,
              holdLastFrame = true,
              alphaFade = 0.01f
            });
            Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(44, this.position + new Vector2((float) (-Game1.tileSize / 4), 0.0f), this.color * 0.66f, 10, false, 100f, 0, -1, -1f, -1, 0)
            {
              interval = 70f,
              delayBeforeAnimationStart = 0,
              holdLastFrame = true,
              alphaFade = 0.01f
            });
            Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(44, this.position + new Vector2(0.0f, (float) (Game1.tileSize / 4)), this.color * 0.66f, 10, false, 100f, 0, -1, -1f, -1, 0)
            {
              interval = 70f,
              delayBeforeAnimationStart = 100,
              holdLastFrame = true,
              alphaFade = 0.01f
            });
            Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(44, this.position + new Vector2((float) (Game1.tileSize / 4), 0.0f), this.color * 0.66f, 10, false, 100f, 0, -1, -1f, -1, 0)
            {
              interval = 70f,
              delayBeforeAnimationStart = 200,
              holdLastFrame = true,
              alphaFade = 0.01f
            });
          }
        }
      }
      return num1;
    }

    public override void shedChunks(int number, float scale)
    {
      GameLocation currentLocation = Game1.currentLocation;
      Texture2D texture = this.sprite.Texture;
      Rectangle sourcerectangle = new Rectangle(0, 120, 16, 16);
      int sizeOfSourceRectSquares = 8;
      Rectangle boundingBox = this.GetBoundingBox();
      int xPosition = boundingBox.Center.X + Game1.tileSize / 2;
      boundingBox = this.GetBoundingBox();
      int y1 = boundingBox.Center.Y;
      int numberOfChunks = number;
      int y2 = (int) this.getTileLocation().Y;
      Color color = this.color;
      double num = 4.0 * (double) scale;
      Game1.createRadialDebris(currentLocation, texture, sourcerectangle, sizeOfSourceRectSquares, xPosition, y1, numberOfChunks, y2, color, (float) num);
    }

    public override void collisionWithFarmerBehavior()
    {
      if (Game1.random.NextDouble() < 0.3 && !Game1.player.temporarilyInvincible && (!Game1.player.isWearingRing(520) && Game1.buffsDisplay.addOtherBuff(new Buff(13))))
        Game1.playSound("slime");
      this.farmerPassesThrough = Game1.player.isWearingRing(520);
    }

    public override void draw(SpriteBatch b)
    {
      if (this.isInvisible || !Utility.isOnScreen(this.position, 2 * Game1.tileSize))
        return;
      b.Draw(this.Sprite.Texture, this.getLocalPosition(Game1.viewport) + new Vector2((float) (Game1.tileSize / 2), (float) (this.GetBoundingBox().Height / 2 + this.yOffset)), new Rectangle?(this.Sprite.SourceRect), this.color, 0.0f, new Vector2(8f, 16f), (float) Game1.pixelZoom * Math.Max(0.2f, this.scale - (float) (0.400000005960464 * ((double) this.ageUntilFullGrown / 120000.0))), SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.991f : (float) this.getStandingY() / 10000f));
      if (this.ageUntilFullGrown <= 0)
      {
        if (this.cute || this.hasSpecialItem)
        {
          TimeSpan totalGameTime;
          int num1;
          if (!this.isMoving() && this.wagTimer <= 0)
          {
            num1 = 48;
          }
          else
          {
            int num2 = 16;
            int val1 = 7;
            int num3;
            if (this.wagTimer <= 0)
            {
              totalGameTime = Game1.currentGameTime.TotalGameTime;
              num3 = totalGameTime.Milliseconds % 992;
            }
            else
              num3 = 992 - this.wagTimer;
            int num4 = 496;
            int val2 = Math.Abs(num3 - num4) / 62;
            int num5 = Math.Min(val1, val2);
            num1 = num2 * num5 % 64;
          }
          int x = num1;
          int num6;
          if (!this.isMoving() && this.wagTimer <= 0)
          {
            num6 = 24;
          }
          else
          {
            int num2 = 24;
            int val1_1 = 1;
            int val1_2 = 1;
            int num3;
            if (this.wagTimer <= 0)
            {
              totalGameTime = Game1.currentGameTime.TotalGameTime;
              num3 = totalGameTime.Milliseconds % 992;
            }
            else
              num3 = 992 - this.wagTimer;
            int num4 = 496;
            int val2_1 = Math.Abs(num3 - num4) / 62;
            int val2_2 = Math.Max(val1_2, val2_1) / 4;
            int num5 = Math.Min(val1_1, val2_2);
            num6 = num2 * num5;
          }
          int num7 = num6;
          if (this.hasSpecialItem)
            num7 += 48;
          b.Draw(this.Sprite.Texture, this.getLocalPosition(Game1.viewport) + new Vector2((float) (Game1.tileSize / 2), (float) (this.GetBoundingBox().Height - Game1.pixelZoom * 4 + (this.readyToJump <= 0 ? Game1.pixelZoom * (Math.Abs(this.sprite.currentFrame % 4 - 2) - 2) : Game1.pixelZoom + Game1.pixelZoom * (this.sprite.currentFrame % 4 % 3)) + this.yOffset)) * this.scale, new Rectangle?(new Rectangle(x, 168 + num7, 16, 24)), this.hasSpecialItem ? Color.White : this.color, 0.0f, new Vector2(8f, 16f), (float) Game1.pixelZoom * Math.Max(0.2f, this.scale - (float) (0.400000005960464 * ((double) this.ageUntilFullGrown / 120000.0))), this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.991f : (float) ((double) this.getStandingY() / 10000.0 + 9.99999974737875E-05)));
        }
        b.Draw(this.Sprite.Texture, this.getLocalPosition(Game1.viewport) + (new Vector2((float) (Game1.tileSize / 2), (float) (this.GetBoundingBox().Height / 2 + (this.readyToJump <= 0 ? Game1.pixelZoom * (Math.Abs(this.sprite.currentFrame % 4 - 2) - 2) : Game1.pixelZoom - Game1.pixelZoom * (this.sprite.currentFrame % 4 % 3)) + this.yOffset)) + this.facePosition) * Math.Max(0.2f, this.scale - (float) (0.400000005960464 * ((double) this.ageUntilFullGrown / 120000.0))), new Rectangle?(new Rectangle(32 + (this.readyToJump > 0 || this.focusedOnFarmers ? 16 : 0), 120 + (this.readyToJump >= 0 || !this.focusedOnFarmers && this.invincibleCountdown <= 0 ? 0 : 24), 16, 24)), Color.White * (this.facingDirection == 0 ? 0.5f : 1f), 0.0f, new Vector2(8f, 16f), (float) Game1.pixelZoom * Math.Max(0.2f, this.scale - (float) (0.400000005960464 * ((double) this.ageUntilFullGrown / 120000.0))), SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.991f : (float) ((double) this.getStandingY() / 10000.0 + 9.99999974737875E-05)));
      }
      if (this.isGlowing)
        b.Draw(this.Sprite.Texture, this.getLocalPosition(Game1.viewport) + new Vector2((float) (Game1.tileSize / 2), (float) (this.GetBoundingBox().Height / 2 + this.yOffset)), new Rectangle?(this.Sprite.SourceRect), this.glowingColor * this.glowingTransparency, 0.0f, new Vector2(8f, 16f), (float) Game1.pixelZoom * Math.Max(0.2f, this.scale), SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.99f : (float) ((double) this.getStandingY() / 10000.0 + 1.0 / 1000.0)));
      if (this.mateToPursue != null)
        b.Draw(this.Sprite.Texture, this.getLocalPosition(Game1.viewport) + new Vector2((float) (Game1.tileSize / 2), (float) (-Game1.tileSize / 2 + this.yOffset)), new Rectangle?(new Rectangle(16, 120, 8, 8)), Color.White, 0.0f, new Vector2(3f, 3f), (float) Game1.pixelZoom, SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.991f : (float) this.getStandingY() / 10000f));
      else if (this.mateToAvoid != null)
        b.Draw(this.Sprite.Texture, this.getLocalPosition(Game1.viewport) + new Vector2((float) (Game1.tileSize / 2), (float) (-Game1.tileSize / 2 + this.yOffset)), new Rectangle?(new Rectangle(24, 120, 8, 8)), Color.White, 0.0f, new Vector2(4f, 4f), (float) Game1.pixelZoom, SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.991f : (float) this.getStandingY() / 10000f));
      SpriteBatch spriteBatch = b;
      Texture2D shadowTexture = Game1.shadowTexture;
      Vector2 position = this.getLocalPosition(Game1.viewport) + new Vector2((float) (Game1.tileSize / 2), (float) ((double) (this.GetBoundingBox().Height / 2 * 7) / 4.0 + (double) this.yOffset + (double) (Game1.pixelZoom * 2) * (double) this.scale - (this.ageUntilFullGrown > 0 ? (double) (Game1.pixelZoom * 2) : 0.0)));
      Rectangle? sourceRectangle = new Rectangle?(Game1.shadowTexture.Bounds);
      Color white = Color.White;
      double num8 = 0.0;
      Rectangle bounds = Game1.shadowTexture.Bounds;
      double x1 = (double) bounds.Center.X;
      bounds = Game1.shadowTexture.Bounds;
      double y = (double) bounds.Center.Y;
      Vector2 origin = new Vector2((float) x1, (float) y);
      double num9 = 3.0 + (double) this.scale - (double) this.ageUntilFullGrown / 120000.0 - (this.Sprite.CurrentFrame % 4 % 3 != 0 ? 1.0 : 0.0) + (double) this.yOffset / 30.0;
      int num10 = 0;
      double num11 = (double) (this.getStandingY() - 1) / 10000.0;
      spriteBatch.Draw(shadowTexture, position, sourceRectangle, white, (float) num8, origin, (float) num9, (SpriteEffects) num10, (float) num11);
    }

    public void moveTowardOtherSlime(GreenSlime other, bool moveAway, GameTime time)
    {
      int num1 = Math.Abs(other.getStandingX() - this.getStandingX());
      int num2 = Math.Abs(other.getStandingY() - this.getStandingY());
      if (num1 > 4 || num2 > 4)
      {
        int num3 = other.getStandingX() > this.getStandingX() ? 1 : -1;
        int num4 = other.getStandingY() > this.getStandingY() ? 1 : -1;
        if (moveAway)
        {
          num3 = -num3;
          num4 = -num4;
        }
        double num5 = (double) num1 / (double) (num1 + num2);
        if (Game1.random.NextDouble() < num5)
          this.tryToMoveInDirection(num3 > 0 ? 1 : 3, false, this.damageToFarmer, false);
        else
          this.tryToMoveInDirection(num4 > 0 ? 2 : 0, false, this.damageToFarmer, false);
      }
      this.sprite.AnimateDown(time, 0, "");
      if (this.invincibleCountdown <= 0)
        return;
      this.invincibleCountdown = this.invincibleCountdown - time.ElapsedGameTime.Milliseconds;
      if (this.invincibleCountdown > 0)
        return;
      this.stopGlowing();
    }

    public void doneMating()
    {
      this.readyToMate = 120000;
      this.matingCountdown = 2000;
      this.mateToPursue = (GreenSlime) null;
      this.mateToAvoid = (GreenSlime) null;
    }

    public override void update(GameTime time, GameLocation location)
    {
      if (this.mateToPursue == null && this.mateToAvoid == null)
      {
        base.update(time, location);
      }
      else
      {
        if (this.currentLocation == null)
          this.currentLocation = location;
        this.behaviorAtGameTick(time);
      }
    }

    public override void noMovementProgressNearPlayerBehavior()
    {
      this.faceGeneralDirection(Utility.getNearestFarmerInCurrentLocation(this.getTileLocation()).getStandingPosition(), 0);
    }

    public void mateWith(GreenSlime mateToPursue, GameLocation location)
    {
      if (location.canSlimeMateHere())
      {
        GreenSlime greenSlime = new GreenSlime(Vector2.Zero);
        Utility.recursiveFindPositionForCharacter((NPC) greenSlime, location, this.getTileLocation(), 30);
        Random random = new Random((int) Game1.stats.DaysPlayed + (int) Game1.uniqueIDForThisGame / 10 + (int) ((double) this.scale * 100.0) + (int) ((double) mateToPursue.scale * 100.0));
        switch (random.Next(4))
        {
          case 0:
            greenSlime.color = new Color(Math.Min((int) byte.MaxValue, Math.Max(0, (int) this.color.R + random.Next((int) ((double) -this.color.R * 0.25), (int) ((double) this.color.R * 0.25)))), Math.Min((int) byte.MaxValue, Math.Max(0, (int) this.color.G + random.Next((int) ((double) -this.color.G * 0.25), (int) ((double) this.color.G * 0.25)))), Math.Min((int) byte.MaxValue, Math.Max(0, (int) this.color.B + random.Next((int) ((double) -this.color.B * 0.25), (int) ((double) this.color.B * 0.25)))));
            break;
          case 1:
          case 2:
            greenSlime.color = Utility.getBlendedColor(this.color, mateToPursue.color);
            break;
          case 3:
            greenSlime.color = new Color(Math.Min((int) byte.MaxValue, Math.Max(0, (int) mateToPursue.color.R + random.Next((int) ((double) -mateToPursue.color.R * 0.25), (int) ((double) mateToPursue.color.R * 0.25)))), Math.Min((int) byte.MaxValue, Math.Max(0, (int) mateToPursue.color.G + random.Next((int) ((double) -mateToPursue.color.G * 0.25), (int) ((double) mateToPursue.color.G * 0.25)))), Math.Min((int) byte.MaxValue, Math.Max(0, (int) mateToPursue.color.B + random.Next((int) ((double) -mateToPursue.color.B * 0.25), (int) ((double) mateToPursue.color.B * 0.25)))));
            break;
        }
        int r = (int) greenSlime.color.R;
        int g = (int) greenSlime.color.G;
        int b = (int) greenSlime.color.B;
        if (r > 100 && b > 100 && g < 50)
        {
          greenSlime.parseMonsterInfo("Sludge");
          while (random.NextDouble() < 0.1)
            greenSlime.objectsToDrop.Add(386);
          if (random.NextDouble() < 0.01)
            greenSlime.objectsToDrop.Add(337);
        }
        else if (r >= 200 && g < 75)
          greenSlime.parseMonsterInfo("Sludge");
        else if (b >= 200 && r < 100)
          greenSlime.parseMonsterInfo("Frost Jelly");
        greenSlime.health = random.NextDouble() < 0.5 ? this.health : mateToPursue.health;
        greenSlime.health = Math.Max(1, this.health + random.Next(-4, 5));
        greenSlime.damageToFarmer = random.NextDouble() < 0.5 ? this.damageToFarmer : mateToPursue.damageToFarmer;
        greenSlime.damageToFarmer = Math.Max(0, this.damageToFarmer + random.Next(-1, 2));
        greenSlime.resilience = random.NextDouble() < 0.5 ? this.resilience : mateToPursue.resilience;
        greenSlime.resilience = Math.Max(0, this.resilience + random.Next(-1, 2));
        greenSlime.missChance = random.NextDouble() < 0.5 ? this.missChance : mateToPursue.missChance;
        greenSlime.missChance = Math.Max(0.0, this.missChance + (double) random.Next(-1, 2) / 100.0);
        greenSlime.scale = random.NextDouble() < 0.5 ? this.scale : mateToPursue.scale;
        greenSlime.scale = Math.Max(0.6f, Math.Min(1.5f, this.scale + (float) random.Next(-2, 3) / 100f));
        greenSlime.slipperiness = 8;
        this.speed = random.NextDouble() < 0.5 ? this.speed : mateToPursue.speed;
        if (random.NextDouble() < 0.015)
          this.speed = Math.Max(1, Math.Min(6, this.speed + random.Next(-1, 2)));
        greenSlime.setTrajectory(Utility.getAwayFromPositionTrajectory(greenSlime.GetBoundingBox(), this.getStandingPosition()) / 2f);
        greenSlime.ageUntilFullGrown = 120000;
        greenSlime.Halt();
        greenSlime.firstGeneration = false;
        if (Utility.isOnScreen(this.position, 128))
          Game1.playSound("slime");
      }
      mateToPursue.doneMating();
      this.doneMating();
    }

    public override List<Item> getExtraDropItems()
    {
      List<Item> objList = new List<Item>();
      if ((int) this.color.R < 80 && (int) this.color.G < 80 && (int) this.color.B < 80)
      {
        objList.Add((Item) new StardewValley.Object(382, 1, false, -1, 0));
        Random random = new Random((int) this.position.X * 777 + (int) this.position.Y * 77 + (int) Game1.stats.DaysPlayed);
        if (random.NextDouble() < 0.05)
          objList.Add((Item) new StardewValley.Object(553, 1, false, -1, 0));
        if (random.NextDouble() < 0.05)
          objList.Add((Item) new StardewValley.Object(539, 1, false, -1, 0));
      }
      else if ((int) this.color.R > 200 && (int) this.color.G > 180 && (int) this.color.B < 50)
        objList.Add((Item) new StardewValley.Object(384, 2, false, -1, 0));
      else if ((int) this.color.R > 220 && (int) this.color.G > 90 && ((int) this.color.G < 150 && (int) this.color.B < 50))
        objList.Add((Item) new StardewValley.Object(378, 2, false, -1, 0));
      else if ((int) this.color.R > 230 && (int) this.color.G > 230 && (int) this.color.B > 230)
      {
        objList.Add((Item) new StardewValley.Object(380, 1, false, -1, 0));
        if ((int) this.color.R % 2 == 0 && (int) this.color.G % 2 == 0 && (int) this.color.B % 2 == 0)
          objList.Add((Item) new StardewValley.Object(72, 1, false, -1, 0));
      }
      else if ((int) this.color.R > 150 && (int) this.color.G > 150 && (int) this.color.B > 150)
        objList.Add((Item) new StardewValley.Object(390, 2, false, -1, 0));
      else if ((int) this.color.R > 150 && (int) this.color.B > 180 && ((int) this.color.G < 50 && this.specialNumber % 4 == 0))
        objList.Add((Item) new StardewValley.Object(386, 2, false, -1, 0));
      if (Game1.player.mailReceived.Contains("slimeHutchBuilt") && this.specialNumber == 1)
      {
        string name = this.name;
        if (!(name == "Green Slime"))
        {
          if (name == "Frost Jelly")
            objList.Add((Item) new StardewValley.Object(413, 1, false, -1, 0));
        }
        else
          objList.Add((Item) new StardewValley.Object(680, 1, false, -1, 0));
      }
      return objList;
    }

    public override void dayUpdate(int dayOfMonth)
    {
      if (this.ageUntilFullGrown > 0)
        this.ageUntilFullGrown = this.ageUntilFullGrown / 2;
      if (this.readyToMate > 0)
        this.readyToMate = this.readyToMate / 2;
      base.dayUpdate(dayOfMonth);
    }

    public override void behaviorAtGameTick(GameTime time)
    {
      TimeSpan elapsedGameTime;
      if (this.wagTimer > 0)
      {
        int wagTimer = this.wagTimer;
        elapsedGameTime = time.ElapsedGameTime;
        int totalMilliseconds = (int) elapsedGameTime.TotalMilliseconds;
        this.wagTimer = wagTimer - totalMilliseconds;
      }
      switch (this.FacingDirection)
      {
        case 0:
          if ((double) this.facePosition.X > 0.0)
            this.facePosition.X -= 2f;
          else if ((double) this.facePosition.X < 0.0)
            this.facePosition.X += 2f;
          if ((double) this.facePosition.Y > (double) (-Game1.pixelZoom * 2))
          {
            this.facePosition.Y -= 2f;
            break;
          }
          break;
        case 1:
          if ((double) this.facePosition.X < (double) (Game1.pixelZoom * 2))
            this.facePosition.X += 2f;
          if ((double) this.facePosition.Y < 0.0)
          {
            this.facePosition.Y += 2f;
            break;
          }
          break;
        case 2:
          if ((double) this.facePosition.X > 0.0)
            this.facePosition.X -= 2f;
          else if ((double) this.facePosition.X < 0.0)
            this.facePosition.X += 2f;
          if ((double) this.facePosition.Y < 0.0)
          {
            this.facePosition.Y += 2f;
            break;
          }
          break;
        case 3:
          if ((double) this.facePosition.X > (double) (-Game1.pixelZoom * 2))
            this.facePosition.X -= 2f;
          if ((double) this.facePosition.Y < 0.0)
          {
            this.facePosition.Y += 2f;
            break;
          }
          break;
      }
      if (this.ageUntilFullGrown <= 0)
      {
        int readyToMate = this.readyToMate;
        elapsedGameTime = time.ElapsedGameTime;
        int milliseconds = elapsedGameTime.Milliseconds;
        this.readyToMate = readyToMate - milliseconds;
      }
      else
      {
        int ageUntilFullGrown = this.ageUntilFullGrown;
        elapsedGameTime = time.ElapsedGameTime;
        int milliseconds = elapsedGameTime.Milliseconds;
        this.ageUntilFullGrown = ageUntilFullGrown - milliseconds;
      }
      if (this.mateToPursue != null)
      {
        if (this.readyToMate <= -35000)
        {
          this.mateToPursue.doneMating();
          this.doneMating();
        }
        else
        {
          this.moveTowardOtherSlime(this.mateToPursue, false, time);
          if (this.mateToPursue.mateToAvoid == null && this.mateToPursue.mateToPursue != null && !this.mateToPursue.mateToPursue.Equals((object) this))
            this.doneMating();
          else if ((double) Vector2.Distance(this.getStandingPosition(), this.mateToPursue.getStandingPosition()) < (double) (this.GetBoundingBox().Width + 4))
          {
            if (this.mateToPursue.mateToAvoid != null && this.mateToPursue.mateToAvoid.Equals((object) this))
            {
              this.mateToPursue.mateToAvoid = (GreenSlime) null;
              this.mateToPursue.matingCountdown = 2000;
              this.mateToPursue.mateToPursue = this;
            }
            int matingCountdown = this.matingCountdown;
            elapsedGameTime = time.ElapsedGameTime;
            int milliseconds = elapsedGameTime.Milliseconds;
            this.matingCountdown = matingCountdown - milliseconds;
            if (this.currentLocation == null || this.matingCountdown > 0 || this.mateToPursue == null || this.currentLocation.isOutdoors && Utility.getNumberOfCharactersInRadius(this.currentLocation, Utility.Vector2ToPoint(this.position), 1) > 4)
              return;
            this.mateWith(this.mateToPursue, this.currentLocation);
          }
          else
          {
            if ((double) Vector2.Distance(this.getStandingPosition(), this.mateToPursue.getStandingPosition()) <= (double) (GreenSlime.matingRange * 2))
              return;
            this.mateToPursue.mateToAvoid = (GreenSlime) null;
            this.mateToPursue = (GreenSlime) null;
          }
        }
      }
      else if (this.mateToAvoid != null)
      {
        this.moveTowardOtherSlime(this.mateToAvoid, true, time);
      }
      else
      {
        if (this.readyToMate < 0 && this.cute)
        {
          this.readyToMate = -1;
          if (Game1.random.NextDouble() < 0.001)
          {
            GreenSlime greenSlime = (GreenSlime) Utility.checkForCharacterWithinArea(this.GetType(), this.position, Game1.currentLocation, new Rectangle(this.getStandingX() - GreenSlime.matingRange, this.getStandingY() - GreenSlime.matingRange, GreenSlime.matingRange * 2, GreenSlime.matingRange * 2));
            if (greenSlime != null && greenSlime.readyToMate <= 0 && !greenSlime.cute)
            {
              this.matingCountdown = 2000;
              this.mateToPursue = greenSlime;
              greenSlime.mateToAvoid = this;
              this.addedSpeed = 1;
              this.mateToPursue.addedSpeed = 1;
              return;
            }
          }
        }
        else if (!this.isGlowing)
          this.addedSpeed = 0;
        this.yOffset = Math.Max(this.yOffset - (int) Math.Abs(this.xVelocity + this.yVelocity) / 2, -Game1.tileSize);
        base.behaviorAtGameTick(time);
        if (this.yOffset < 0)
          this.yOffset = Math.Min(0, this.yOffset + 4 + (this.yOffset <= -Game1.tileSize ? (int) ((double) -this.yOffset / 8.0) : (int) ((double) -this.yOffset / 16.0)));
        int timeSinceLastJump = this.timeSinceLastJump;
        elapsedGameTime = time.ElapsedGameTime;
        int milliseconds1 = elapsedGameTime.Milliseconds;
        this.timeSinceLastJump = timeSinceLastJump + milliseconds1;
        if (this.readyToJump != -1)
        {
          this.Halt();
          this.IsWalkingTowardPlayer = false;
          int readyToJump = this.readyToJump;
          elapsedGameTime = time.ElapsedGameTime;
          int milliseconds2 = elapsedGameTime.Milliseconds;
          this.readyToJump = readyToJump - milliseconds2;
          this.sprite.CurrentFrame = 16 + (800 - this.readyToJump) / 200;
          if (this.readyToJump <= 0)
          {
            this.timeSinceLastJump = this.timeSinceLastJump;
            this.slipperiness = 10;
            this.IsWalkingTowardPlayer = true;
            this.readyToJump = -1;
            if (Utility.isOnScreen(this.position, 128))
              Game1.playSound("slime");
            Vector2 playerTrajectory = Utility.getAwayFromPlayerTrajectory(this.GetBoundingBox());
            playerTrajectory.X = (float) (-(double) playerTrajectory.X / 2.0);
            playerTrajectory.Y = (float) (-(double) playerTrajectory.Y / 2.0);
            this.setTrajectory((int) playerTrajectory.X, (int) playerTrajectory.Y);
            this.sprite.CurrentFrame = 1;
            this.invincibleCountdown = 0;
          }
        }
        else if (Game1.random.NextDouble() < 0.1 && !this.focusedOnFarmers)
        {
          if (this.facingDirection == 0 || this.facingDirection == 2)
          {
            if (this.leftDrift && !Game1.currentLocation.isCollidingPosition(this.nextPosition(3), Game1.viewport, false, 1, false, (Character) this))
              this.position.X -= (float) this.speed;
            else if (!this.leftDrift && !Game1.currentLocation.isCollidingPosition(this.nextPosition(1), Game1.viewport, false, 1, false, (Character) this))
              this.position.X += (float) this.speed;
          }
          else if (this.leftDrift && !Game1.currentLocation.isCollidingPosition(this.nextPosition(0), Game1.viewport, false, 1, false, (Character) this))
            this.position.Y -= (float) this.speed;
          else if (!this.leftDrift && !Game1.currentLocation.isCollidingPosition(this.nextPosition(2), Game1.viewport, false, 1, false, (Character) this))
            this.position.Y += (float) this.speed;
          if (Game1.random.NextDouble() < 0.08)
            this.leftDrift = !this.leftDrift;
        }
        else if (this.withinPlayerThreshold() && (this.timeSinceLastJump > (this.focusedOnFarmers ? 1000 : 4000) && Game1.random.NextDouble() < 0.01))
        {
          if (this.name.Equals("Frost Jelly") && Game1.random.NextDouble() < 0.25)
          {
            this.addedSpeed = 2;
            this.startGlowing(Color.Cyan, false, 0.15f);
          }
          else
          {
            this.addedSpeed = 0;
            this.stopGlowing();
            this.readyToJump = 800;
          }
        }
        if (Game1.random.NextDouble() < 0.01 && this.wagTimer <= 0)
          this.wagTimer = 992;
        if ((double) Math.Abs(this.xVelocity) >= 0.5 || (double) Math.Abs(this.yVelocity) >= 0.5)
          this.sprite.AnimateDown(time, 0, "");
        else if (!this.position.Equals(this.lastPosition))
          this.animateTimer = 500;
        if (this.animateTimer <= 0 || this.readyToJump > 0)
          return;
        int animateTimer = this.animateTimer;
        elapsedGameTime = time.ElapsedGameTime;
        int milliseconds3 = elapsedGameTime.Milliseconds;
        this.animateTimer = animateTimer - milliseconds3;
        this.sprite.AnimateDown(time, 0, "");
      }
    }
  }
}
