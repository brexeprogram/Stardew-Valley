// Decompiled with JetBrains decompiler
// Type: StardewValley.Debris
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace StardewValley
{
  public class Debris
  {
    private List<Chunk> chunks = new List<Chunk>();
    public int sizeOfSourceRectSquares = 8;
    public float scale = 1f;
    public bool visible = true;
    public string debrisMessage = "";
    public Color nonSpriteChunkColor = Color.White;
    public const int copperDebris = 0;
    public const int ironDebris = 2;
    public const int coalDebris = 4;
    public const int goldDebris = 6;
    public const int coinsDebris = 8;
    public const int iridiumDebris = 10;
    public const int woodDebris = 12;
    public const int stoneDebris = 14;
    public const int fuelDebris = 28;
    public const int quartzDebris = 30;
    public const int bigStoneDebris = 32;
    public const int bigWoodDebris = 34;
    public const int timesToBounce = 2;
    public const int minMoneyPerCoin = 10;
    public const int maxMoneyPerCoin = 40;
    public const float gravity = 0.4f;
    public const float timeToWaitBeforeRemoval = 600f;
    public const int marginForChunkPickup = 64;
    public const int white = 10000;
    public const int green = 100001;
    public const int blue = 100002;
    public const int red = 100003;
    public const int yellow = 100004;
    public const int black = 100005;
    public const int charcoal = 100007;
    public const int gray = 100006;
    public int chunkType;
    public int itemQuality;
    public int chunkFinalYLevel;
    public int chunkFinalYTarget;
    public float timeSinceDoneBouncing;
    private bool chunksMoveTowardPlayer;
    private bool movingUp;
    public bool itemDebris;
    public bool floppingFish;
    public bool isFishable;
    public bool movingFinalYLevel;
    public Debris.DebrisType debrisType;
    public Color chunksColor;
    [XmlIgnore]
    public Texture2D spriteChunkSheet;
    public Item item;
    public int uniqueID;
    public Character toHover;
    private float relativeXPosition;

    public Debris()
    {
    }

    public List<Chunk> Chunks
    {
      get
      {
        return this.chunks;
      }
    }

    public Debris(int objectIndex, Vector2 debrisOrigin, Vector2 playerPosition)
      : this(objectIndex, 1, debrisOrigin, playerPosition)
    {
      string str;
      if (objectIndex <= 0)
        str = "Crafting";
      else
        str = Game1.objectInformation[objectIndex].Split('/')[3].Split(' ')[0];
      this.debrisType = !str.Equals("Arch") ? Debris.DebrisType.OBJECT : Debris.DebrisType.ARCHAEOLOGY;
      if (objectIndex == 92)
        this.debrisType = Debris.DebrisType.RESOURCE;
      if (Game1.player.speed >= 5 && !Game1.IsMultiplayer)
      {
        for (int index = 0; index < this.chunks.Count; ++index)
          this.chunks[index].xVelocity *= Game1.player.FacingDirection == 1 || Game1.player.FacingDirection == 3 ? 1f : 1f;
      }
      this.chunks[0].debrisType = objectIndex;
    }

    public Debris(int number, Vector2 debrisOrigin, Color messageColor, float scale, Character toHover)
      : this(-1, 1, debrisOrigin, Game1.player.Position)
    {
      this.chunkType = number;
      this.debrisType = Debris.DebrisType.NUMBERS;
      this.nonSpriteChunkColor = messageColor;
      this.chunks[0].scale = scale;
      this.toHover = toHover;
      this.chunks[0].xVelocity = (float) Game1.random.Next(-1, 2);
    }

    public Debris(Item item, Vector2 debrisOrigin)
    {
      int debrisType = -2;
      int numberOfChunks = 1;
      Vector2 debrisOrigin1 = debrisOrigin;
      Rectangle boundingBox = Game1.player.GetBoundingBox();
      double x = (double) boundingBox.Center.X;
      boundingBox = Game1.player.GetBoundingBox();
      double y = (double) boundingBox.Center.Y;
      Vector2 playerPosition = new Vector2((float) x, (float) y);
      // ISSUE: explicit constructor call
      this.\u002Ector(debrisType, numberOfChunks, debrisOrigin1, playerPosition);
      this.item = item;
    }

    public Debris(Item item, Vector2 debrisOrigin, Vector2 targetLocation)
      : this(-2, 1, debrisOrigin, targetLocation)
    {
      this.item = item;
    }

    public Debris(string message, int numberOfChunks, Vector2 debrisOrigin, Color messageColor, float scale, float rotation)
      : this(-1, numberOfChunks, debrisOrigin, Game1.player.Position)
    {
      this.debrisType = Debris.DebrisType.LETTERS;
      this.debrisMessage = message;
      this.nonSpriteChunkColor = messageColor;
      this.chunks[0].rotation = rotation;
      this.chunks[0].scale = scale;
    }

    public Debris(Texture2D spriteSheet, int numberOfChunks, Vector2 debrisOrigin)
      : this(-1, numberOfChunks, debrisOrigin, Game1.player.Position)
    {
      this.debrisType = Debris.DebrisType.SPRITECHUNKS;
      this.spriteChunkSheet = spriteSheet;
      for (int index = 0; index < this.chunks.Count; ++index)
      {
        this.chunks[index].xSpriteSheet = Game1.random.Next(0, Game1.tileSize - 8);
        this.chunks[index].ySpriteSheet = Game1.random.Next(0, Game1.tileSize * 3 / 2 - 8);
        this.chunks[index].scale = 1f;
      }
    }

    public Debris(Texture2D spriteSheet, Rectangle sourceRect, int numberOfChunks, Vector2 debrisOrigin)
      : this(-1, numberOfChunks, debrisOrigin, Game1.player.Position)
    {
      this.debrisType = Debris.DebrisType.SPRITECHUNKS;
      this.spriteChunkSheet = spriteSheet;
      for (int index = 0; index < this.chunks.Count; ++index)
      {
        this.chunks[index].xSpriteSheet = Game1.random.Next(sourceRect.X, sourceRect.X + sourceRect.Width - 4);
        this.chunks[index].ySpriteSheet = Game1.random.Next(sourceRect.Y, sourceRect.Y + sourceRect.Width - 4);
        this.chunks[index].scale = 1f;
      }
    }

    public Debris(Texture2D spriteSheet, Rectangle sourceRect, int numberOfChunks, Vector2 debrisOrigin, Vector2 playerPosition, int groundLevel, int sizeOfSourceRectSquares)
      : this(-1, numberOfChunks, debrisOrigin, Game1.player.Position)
    {
      this.sizeOfSourceRectSquares = sizeOfSourceRectSquares;
      this.debrisType = Debris.DebrisType.SPRITECHUNKS;
      this.spriteChunkSheet = spriteSheet;
      for (int index = 0; index < this.chunks.Count; ++index)
      {
        this.chunks[index].xSpriteSheet = Game1.random.Next(2) * sizeOfSourceRectSquares + sourceRect.X;
        this.chunks[index].ySpriteSheet = Game1.random.Next(2) * sizeOfSourceRectSquares + sourceRect.Y;
        this.chunks[index].rotationVelocity = Game1.random.NextDouble() < 0.5 ? 3.141593f / (float) Game1.random.Next(-32, -16) : 3.141593f / (float) Game1.random.Next(16, 32);
        this.chunks[index].xVelocity *= 1.2f;
        this.chunks[index].yVelocity *= 1.2f;
        this.chunks[index].scale = (float) Game1.pixelZoom;
      }
    }

    public Debris(Texture2D spriteSheet, Rectangle sourceRect, int numberOfChunks, Vector2 debrisOrigin, Vector2 playerPosition, int groundLevel)
      : this(-1, numberOfChunks, debrisOrigin, playerPosition)
    {
      this.debrisType = Debris.DebrisType.SPRITECHUNKS;
      this.spriteChunkSheet = spriteSheet;
      for (int index = 0; index < this.chunks.Count; ++index)
      {
        this.chunks[index].xSpriteSheet = Game1.random.Next(sourceRect.X, sourceRect.X + sourceRect.Width - 4);
        this.chunks[index].ySpriteSheet = Game1.random.Next(sourceRect.Y, sourceRect.Y + sourceRect.Width - 4);
        this.chunks[index].scale = 1f;
      }
      this.chunkFinalYLevel = groundLevel;
    }

    public Debris(int type, int numberOfChunks, Vector2 debrisOrigin, Vector2 playerPosition, int groundLevel, int color = -1)
      : this(-1, numberOfChunks, debrisOrigin, playerPosition)
    {
      this.debrisType = Debris.DebrisType.CHUNKS;
      for (int index = 0; index < this.chunks.Count; ++index)
        this.chunks[index].debrisType = type;
      this.chunkType = type;
      this.chunksColor = this.getColorForDebris(color == -1 ? type : color);
    }

    public Color getColorForDebris(int type)
    {
      switch (type)
      {
        case 12:
          return new Color(170, 106, 46);
        case 100001:
        case 100006:
          return Color.LightGreen;
        case 100002:
          return Color.LightBlue;
        case 100003:
          return Color.Red;
        case 100004:
          return Color.Yellow;
        case 100005:
          return Color.Black;
        case 100007:
          return Color.DimGray;
        default:
          return Color.White;
      }
    }

    public Debris(int debrisType, int numberOfChunks, Vector2 debrisOrigin, Vector2 playerPosition)
      : this(debrisType, numberOfChunks, debrisOrigin, playerPosition, 1f)
    {
    }

    public Debris(int debrisType, int numberOfChunks, Vector2 debrisOrigin, Vector2 playerPosition, float velocityMultiplyer)
    {
      switch (debrisType)
      {
        case 0:
        case 378:
          debrisType = 378;
          this.debrisType = Debris.DebrisType.RESOURCE;
          break;
        case 2:
        case 380:
          debrisType = 380;
          this.debrisType = Debris.DebrisType.RESOURCE;
          break;
        case 4:
        case 382:
          debrisType = 382;
          this.debrisType = Debris.DebrisType.RESOURCE;
          break;
        case 6:
        case 384:
          debrisType = 384;
          this.debrisType = Debris.DebrisType.RESOURCE;
          break;
        case 8:
          this.debrisType = Debris.DebrisType.CHUNKS;
          break;
        case 10:
        case 386:
          debrisType = 386;
          this.debrisType = Debris.DebrisType.RESOURCE;
          break;
        case 12:
        case 388:
          debrisType = 388;
          this.debrisType = Debris.DebrisType.RESOURCE;
          break;
        case 14:
        case 390:
          debrisType = 390;
          this.debrisType = Debris.DebrisType.RESOURCE;
          break;
        default:
          this.debrisType = Debris.DebrisType.OBJECT;
          break;
      }
      if (debrisType != -1)
        playerPosition -= (playerPosition - debrisOrigin) * 2f;
      this.chunkType = debrisType;
      int num1;
      if (Game1.objectInformation.ContainsKey(debrisType))
        num1 = Game1.objectInformation[debrisType].Split('/')[3].Contains("-4") ? 1 : 0;
      else
        num1 = 0;
      this.floppingFish = num1 != 0;
      int num2;
      if (Game1.objectInformation.ContainsKey(debrisType))
        num2 = Game1.objectInformation[debrisType].Split('/')[3].Contains("Fish") ? 1 : 0;
      else
        num2 = 0;
      this.isFishable = num2 != 0;
      int num3;
      int num4;
      int num5;
      int num6;
      if ((double) playerPosition.Y >= (double) debrisOrigin.Y - (double) (Game1.tileSize / 2) && (double) playerPosition.Y <= (double) debrisOrigin.Y + (double) (Game1.tileSize / 2))
      {
        this.chunkFinalYLevel = (int) debrisOrigin.Y - Game1.tileSize / 2;
        num3 = 220;
        num4 = 250;
        if ((double) playerPosition.X < (double) debrisOrigin.X)
        {
          num5 = 20;
          num6 = 140;
        }
        else
        {
          num5 = -140;
          num6 = -20;
        }
      }
      else if ((double) playerPosition.Y < (double) debrisOrigin.Y - (double) (Game1.tileSize / 2))
      {
        this.chunkFinalYLevel = (int) debrisOrigin.Y + (int) ((double) (Game1.tileSize / 2) * (double) velocityMultiplyer);
        num3 = 150;
        num4 = 200;
        num5 = -50;
        num6 = 50;
      }
      else
      {
        this.movingFinalYLevel = true;
        this.chunkFinalYLevel = (int) debrisOrigin.Y - 1;
        this.chunkFinalYTarget = (int) debrisOrigin.Y - (int) ((double) (Game1.tileSize * 3 / 2) * (double) velocityMultiplyer);
        this.movingUp = true;
        num3 = 350;
        num4 = 400;
        num5 = -50;
        num6 = 50;
      }
      debrisOrigin.X -= (float) (Game1.tileSize / 2);
      debrisOrigin.Y -= (float) (Game1.tileSize / 2);
      int minValue1 = (int) ((double) num5 * (double) velocityMultiplyer);
      int maxValue1 = (int) ((double) num6 * (double) velocityMultiplyer);
      int minValue2 = (int) ((double) num3 * (double) velocityMultiplyer);
      int maxValue2 = (int) ((double) num4 * (double) velocityMultiplyer);
      this.uniqueID = Game1.recentMultiplayerRandom.Next(int.MinValue, int.MaxValue);
      for (int index = 0; index < numberOfChunks; ++index)
        this.chunks.Add(new Chunk(debrisOrigin, (float) Game1.recentMultiplayerRandom.Next(minValue1, maxValue1) / 40f, (float) Game1.recentMultiplayerRandom.Next(minValue2, maxValue2) / 40f, Game1.recentMultiplayerRandom.Next(debrisType, debrisType + 2)));
    }

    public bool updateChunks(GameTime time)
    {
      this.timeSinceDoneBouncing = this.timeSinceDoneBouncing + (float) time.ElapsedGameTime.Milliseconds;
      if ((double) this.timeSinceDoneBouncing >= (this.floppingFish ? 2500.0 : (this.debrisType == Debris.DebrisType.SPRITECHUNKS || this.debrisType == Debris.DebrisType.NUMBERS ? 1800.0 : 600.0)))
      {
        if (this.debrisType == Debris.DebrisType.LETTERS || this.debrisType == Debris.DebrisType.NUMBERS || (this.debrisType == Debris.DebrisType.SQUARES || this.debrisType == Debris.DebrisType.SPRITECHUNKS) || this.debrisType == Debris.DebrisType.CHUNKS && this.chunks[0].debrisType - this.chunks[0].debrisType % 2 != 8)
          return true;
        if (this.debrisType == Debris.DebrisType.ARCHAEOLOGY || this.debrisType == Debris.DebrisType.OBJECT || (this.debrisType == Debris.DebrisType.RESOURCE || this.debrisType == Debris.DebrisType.CHUNKS))
          this.chunksMoveTowardPlayer = true;
        this.timeSinceDoneBouncing = 0.0f;
      }
      for (int index = this.chunks.Count - 1; index >= 0; --index)
      {
        if ((double) this.chunks[index].alpha > 0.100000001490116 && (this.debrisType == Debris.DebrisType.SPRITECHUNKS || this.debrisType == Debris.DebrisType.NUMBERS) && (double) this.timeSinceDoneBouncing > 600.0)
          this.chunks[index].alpha = (float) ((1800.0 - (double) this.timeSinceDoneBouncing) / 1000.0);
        if ((double) this.chunks[index].position.X < (double) (-Game1.tileSize * 2) || (double) this.chunks[index].position.Y < (double) -Game1.tileSize || ((double) this.chunks[index].position.X >= (double) (Game1.currentLocation.map.DisplayWidth + Game1.tileSize) || (double) this.chunks[index].position.Y >= (double) (Game1.currentLocation.map.DisplayHeight + Game1.tileSize)))
        {
          this.chunks.RemoveAt(index);
        }
        else
        {
          bool flag1 = (double) Math.Abs(this.chunks[index].position.X + (float) (Game1.tileSize / 2) - (float) Game1.player.getStandingX()) <= (double) Game1.player.MagneticRadius && (double) Math.Abs(this.chunks[index].position.Y + (float) (Game1.tileSize / 2) - (float) Game1.player.getStandingY()) <= (double) Game1.player.MagneticRadius;
          if (flag1)
          {
            switch (this.debrisType)
            {
              case Debris.DebrisType.ARCHAEOLOGY:
              case Debris.DebrisType.OBJECT:
                if (this.item != null)
                {
                  flag1 = Game1.player.couldInventoryAcceptThisItem(this.item);
                  break;
                }
                flag1 = Game1.player.couldInventoryAcceptThisObject(this.chunks[index].debrisType, 1, this.itemQuality);
                if (this.chunks[index].debrisType == 102 && Game1.activeClickableMenu != null)
                {
                  flag1 = false;
                  break;
                }
                break;
              case Debris.DebrisType.RESOURCE:
                flag1 = Game1.player.couldInventoryAcceptThisObject(this.chunks[index].debrisType - this.chunks[index].debrisType % 2, 1, 0);
                break;
              default:
                flag1 = true;
                break;
            }
          }
          if (((this.chunksMoveTowardPlayer ? 1 : (this.isFishable ? 1 : 0)) & (flag1 ? 1 : 0)) != 0)
          {
            if ((double) this.chunks[index].position.X < (double) Game1.player.position.X - 12.0)
              this.chunks[index].xVelocity = Math.Min(this.chunks[index].xVelocity + 0.8f, 8f);
            else if ((double) this.chunks[index].position.X > (double) Game1.player.position.X + 12.0)
              this.chunks[index].xVelocity = Math.Max(this.chunks[index].xVelocity - 0.8f, -8f);
            if ((double) this.chunks[index].position.Y + (double) (Game1.tileSize / 2) < (double) (Game1.player.getStandingY() - 12))
              this.chunks[index].yVelocity = Math.Max(this.chunks[index].yVelocity - 0.8f, -8f);
            else if ((double) this.chunks[index].position.Y + (double) (Game1.tileSize / 2) > (double) (Game1.player.getStandingY() + 12))
              this.chunks[index].yVelocity = Math.Min(this.chunks[index].yVelocity + 0.8f, 8f);
            this.chunks[index].position.X += this.chunks[index].xVelocity;
            this.chunks[index].position.Y -= this.chunks[index].yVelocity;
            if ((double) Math.Abs(this.chunks[index].position.X + (float) (Game1.tileSize / 2) - (float) Game1.player.getStandingX()) <= 64.0 && (double) Math.Abs(this.chunks[index].position.Y + (float) (Game1.tileSize / 2) - (float) Game1.player.getStandingY()) <= 64.0)
            {
              int num1 = this.debrisType == Debris.DebrisType.ARCHAEOLOGY || this.debrisType == Debris.DebrisType.OBJECT ? this.chunks[index].debrisType : this.chunks[index].debrisType - this.chunks[index].debrisType % 2;
              if (this.debrisType == Debris.DebrisType.ARCHAEOLOGY)
                Game1.farmerFindsArtifact(this.chunks[index].debrisType);
              else if (this.item != null)
              {
                if (!Game1.player.addItemToInventoryBool(this.item, false))
                  continue;
              }
              else if (this.debrisType != Debris.DebrisType.CHUNKS || num1 != 8)
              {
                if (num1 <= -10000)
                {
                  if (!Game1.player.addItemToInventoryBool((Item) new MeleeWeapon(num1), false))
                    continue;
                }
                else if (num1 <= 0)
                {
                  if (!Game1.player.addItemToInventoryBool((Item) new Object(Vector2.Zero, -num1, false), false))
                    continue;
                }
                else
                {
                  Farmer player = Game1.player;
                  Object @object;
                  if (num1 != 93 && num1 != 94)
                    @object = new Object(Vector2.Zero, num1, 1)
                    {
                      quality = this.itemQuality
                    };
                  else
                    @object = (Object) new Torch(Vector2.Zero, 1, num1);
                  int num2 = 0;
                  if (!player.addItemToInventoryBool((Item) @object, num2 != 0))
                    continue;
                }
              }
              if ((double) Game1.debrisSoundInterval <= 0.0)
              {
                Game1.debrisSoundInterval = 10f;
                Game1.playSound("coin");
              }
              if (Game1.IsMultiplayer)
                MultiplayerUtility.broadcastDebrisPickup(this.uniqueID, Game1.currentLocation.name, Game1.player.uniqueMultiplayerID);
              this.chunks.RemoveAt(index);
            }
          }
          else
          {
            if (this.debrisType == Debris.DebrisType.NUMBERS && this.toHover != null)
            {
              this.relativeXPosition = this.relativeXPosition + this.chunks[index].xVelocity;
              this.chunks[index].position.X = this.toHover.position.X + (float) (Game1.tileSize / 2) + this.relativeXPosition;
              this.chunks[index].scale = Math.Min(2f, Math.Max(1f, (float) (0.899999976158142 + (double) Math.Abs(this.chunks[index].position.Y - (float) this.chunkFinalYLevel) / ((double) Game1.tileSize * 2.0))));
              this.chunkFinalYLevel = this.toHover.getStandingY() + 8;
              if ((double) this.timeSinceDoneBouncing > 250.0)
                this.chunks[index].alpha = Math.Max(0.0f, this.chunks[index].alpha - 0.033f);
              if (!this.toHover.Equals((object) Game1.player) && !this.nonSpriteChunkColor.Equals(Color.Yellow) && !this.nonSpriteChunkColor.Equals(Color.Green))
              {
                this.nonSpriteChunkColor.R = (byte) Math.Max((double) Math.Min((int) byte.MaxValue, 200 + this.chunkType), Math.Min((double) Math.Min((int) byte.MaxValue, 220 + this.chunkType), 400.0 * Math.Sin((double) this.timeSinceDoneBouncing / (256.0 * Math.PI) + Math.PI / 12.0)));
                this.nonSpriteChunkColor.G = (byte) Math.Max((double) (150 - this.chunkType), Math.Min((double) ((int) byte.MaxValue - this.chunkType), (int) this.nonSpriteChunkColor.R > 220 ? 300.0 * Math.Sin((double) this.timeSinceDoneBouncing / (256.0 * Math.PI) + Math.PI / 12.0) : 0.0));
                this.nonSpriteChunkColor.B = (byte) Math.Max(0, Math.Min((int) byte.MaxValue, (int) this.nonSpriteChunkColor.G > 200 ? (int) this.nonSpriteChunkColor.G - 20 : 0));
              }
            }
            this.chunks[index].position.X += this.chunks[index].xVelocity;
            this.chunks[index].position.Y -= this.chunks[index].yVelocity;
            if (this.movingFinalYLevel)
            {
              this.chunkFinalYLevel = this.chunkFinalYLevel - (int) Math.Ceiling((double) this.chunks[index].yVelocity / 2.0);
              if (this.chunkFinalYLevel <= this.chunkFinalYTarget)
              {
                this.chunkFinalYLevel = this.chunkFinalYTarget;
                this.movingFinalYLevel = false;
              }
            }
            if (this.debrisType == Debris.DebrisType.SQUARES && (double) this.chunks[index].position.Y < (double) (this.chunkFinalYLevel - Game1.tileSize * 3 / 2) && Game1.random.NextDouble() < 0.1)
            {
              this.chunks[index].position.Y = (float) (this.chunkFinalYLevel - Game1.random.Next(1, Game1.tileSize / 3));
              this.chunks[index].yVelocity = (float) Game1.random.Next(30, 80) / 40f;
              this.chunks[index].position.X = (float) Game1.random.Next((int) ((double) this.chunks[index].position.X - (double) this.chunks[index].position.X % (double) Game1.tileSize + 1.0), (int) ((double) this.chunks[index].position.X - (double) this.chunks[index].position.X % (double) Game1.tileSize + 64.0));
            }
            if (this.debrisType != Debris.DebrisType.SQUARES)
              this.chunks[index].yVelocity -= 0.4f;
            bool flag2 = false;
            if ((double) this.chunks[index].position.Y >= (double) this.chunkFinalYLevel && this.chunks[index].hasPassedRestingLineOnce && this.chunks[index].bounces <= (this.floppingFish ? 65 : 2))
            {
              if (this.debrisType != Debris.DebrisType.LETTERS && this.debrisType != Debris.DebrisType.NUMBERS && this.debrisType != Debris.DebrisType.SPRITECHUNKS && (this.debrisType != Debris.DebrisType.CHUNKS || this.chunks[index].debrisType - this.chunks[index].debrisType % 2 == 8))
                Game1.playSound("shiny4");
              ++this.chunks[index].bounces;
              if (this.floppingFish)
              {
                this.chunks[index].yVelocity = Math.Abs(this.chunks[index].yVelocity) * (!this.movingUp || this.chunks[index].bounces >= 2 ? 0.9f : 0.6f);
                this.chunks[index].xVelocity = (float) Game1.random.Next(-250, 250) / 100f;
              }
              else
              {
                this.chunks[index].yVelocity = Math.Abs((float) ((double) this.chunks[index].yVelocity * 2.0 / 3.0));
                this.chunks[index].rotationVelocity = Game1.random.NextDouble() < 0.5 ? this.chunks[index].rotationVelocity / 2f : (float) (-(double) this.chunks[index].rotationVelocity * 2.0 / 3.0);
                this.chunks[index].xVelocity -= this.chunks[index].xVelocity / 2f;
              }
              if (this.debrisType != Debris.DebrisType.LETTERS && this.debrisType != Debris.DebrisType.SPRITECHUNKS && (this.debrisType != Debris.DebrisType.NUMBERS && Game1.currentLocation.doesTileHaveProperty((int) (((double) this.chunks[index].position.X + (double) (Game1.tileSize / 2)) / (double) Game1.tileSize), (int) (((double) this.chunks[index].position.Y + (double) (Game1.tileSize / 2)) / (double) Game1.tileSize), "Water", "Back") != null))
              {
                Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(28, 300f, 2, 1, this.chunks[index].position, false, false));
                Game1.playSound("dropItemInWater");
                flag2 = true;
              }
            }
            if (!this.chunks[index].hitWall && Game1.currentLocation.Map.GetLayer("Buildings").Tiles[(int) (((double) this.chunks[index].position.X + (double) (Game1.tileSize / 2)) / (double) Game1.tileSize), (int) (((double) this.chunks[index].position.Y + (double) (Game1.tileSize / 2)) / (double) Game1.tileSize)] != null || Game1.currentLocation.Map.GetLayer("Back").Tiles[(int) (((double) this.chunks[index].position.X + (double) (Game1.tileSize / 2)) / (double) Game1.tileSize), (int) (((double) this.chunks[index].position.Y + (double) (Game1.tileSize / 2)) / (double) Game1.tileSize)] == null)
            {
              this.chunks[index].xVelocity = -this.chunks[index].xVelocity;
              this.chunks[index].hitWall = true;
            }
            if ((double) this.chunks[index].position.Y < (double) this.chunkFinalYLevel)
              this.chunks[index].hasPassedRestingLineOnce = true;
            if (this.chunks[index].bounces > (this.floppingFish ? 65 : 2))
            {
              this.chunks[index].yVelocity = 0.0f;
              this.chunks[index].xVelocity = 0.0f;
              this.chunks[index].rotationVelocity = 0.0f;
            }
            this.chunks[index].rotation += this.chunks[index].rotationVelocity;
            if (flag2)
              this.chunks.RemoveAt(index);
          }
        }
      }
      return this.chunks.Count == 0;
    }

    public static string getNameOfDebrisTypeFromIntId(int id)
    {
      switch (id)
      {
        case 0:
        case 1:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Debris.cs.621");
        case 2:
        case 3:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Debris.cs.622");
        case 4:
        case 5:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Debris.cs.623");
        case 6:
        case 7:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Debris.cs.624");
        case 8:
        case 9:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Debris.cs.625");
        case 10:
        case 11:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Debris.cs.626");
        case 12:
        case 13:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Debris.cs.627");
        case 14:
        case 15:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Debris.cs.628");
        case 28:
        case 29:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Debris.cs.629");
        case 30:
        case 31:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Debris.cs.630");
        default:
          return "???";
      }
    }

    public static bool getDebris(int which, int howMuch)
    {
      switch (which)
      {
        case 0:
          Game1.player.CopperPieces += howMuch;
          Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Debris.cs.621"), howMuch, true, Color.Sienna, (Item) null));
          if (howMuch > 0)
          {
            Game1.stats.CopperFound += (uint) howMuch;
            break;
          }
          break;
        case 2:
          Game1.player.IronPieces += howMuch;
          Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Debris.cs.622"), howMuch, true, Color.LightSlateGray, (Item) null));
          if (howMuch > 0)
          {
            Game1.stats.IronFound += (uint) howMuch;
            break;
          }
          break;
        case 4:
          Game1.player.CoalPieces += howMuch;
          Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Debris.cs.623"), howMuch, true, Color.DimGray, (Item) null));
          if (howMuch > 0)
          {
            Game1.stats.CoalFound += (uint) howMuch;
            break;
          }
          break;
        case 6:
          Game1.player.GoldPieces += howMuch;
          Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Debris.cs.624"), howMuch, true, Color.Gold, (Item) null));
          if (howMuch > 0)
          {
            Game1.stats.GoldFound += (uint) howMuch;
            break;
          }
          break;
        case 8:
          int num = Game1.random.Next(10, 50) * howMuch;
          Game1.player.Money += num - num % 5;
          if (howMuch > 0)
          {
            Game1.stats.CoinsFound += (uint) howMuch;
            break;
          }
          break;
        case 10:
          Game1.player.IridiumPieces += howMuch;
          Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Debris.cs.626"), howMuch, true, Color.Purple, (Item) null));
          if (howMuch > 0)
          {
            Game1.stats.IridiumFound += (uint) howMuch;
            break;
          }
          break;
        case 12:
          Game1.player.WoodPieces += howMuch;
          Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Debris.cs.627"), howMuch, true, Color.Tan, (Item) null));
          if (howMuch > 0)
          {
            Game1.stats.SticksChopped += (uint) howMuch;
            break;
          }
          break;
        case 28:
          Game1.player.fuelLantern(howMuch * 2);
          Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Debris.cs.629"), howMuch * 2, true, Color.Goldenrod, (Item) null));
          break;
        default:
          return false;
      }
      if (Game1.questOfTheDay != null && Game1.questOfTheDay.accepted && (!Game1.questOfTheDay.completed && Game1.questOfTheDay.GetType().Name.Equals("ResourceCollectionQuest")))
        Game1.questOfTheDay.checkIfComplete((NPC) null, which, howMuch, (Item) null, (string) null);
      return true;
    }

    public enum DebrisType
    {
      CHUNKS,
      LETTERS,
      SQUARES,
      ARCHAEOLOGY,
      OBJECT,
      SPRITECHUNKS,
      RESOURCE,
      NUMBERS,
    }
  }
}
