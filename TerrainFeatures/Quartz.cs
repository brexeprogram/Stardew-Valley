// Decompiled with JetBrains decompiler
// Type: StardewValley.TerrainFeatures.Quartz
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StardewValley.TerrainFeatures
{
  public class Quartz : TerrainFeature
  {
    public const float shakeRate = 0.01570796f;
    public const float shakeDecayRate = 0.003067962f;
    public const double chanceForDiamond = 0.02;
    public const double chanceForPrismaticShard = 0.005;
    public const double chanceForIridium = 0.007;
    public const double chanceForLevelUnique = 0.03;
    public const double chanceForRefinedQuartz = 0.04;
    public const int startingHealth = 10;
    public const int large = 3;
    public const int medium = 2;
    public const int small = 1;
    public const int tiny = 0;
    public const int pointingLeft = 0;
    public const int pointingUp = 1;
    public const int pointingRight = 2;
    private Texture2D texture;
    public float health;
    public bool flipped;
    private bool shakeLeft;
    private bool falling;
    private float shakeRotation;
    private float maxShake;
    private float glow;
    public int bigness;
    private int identifier;
    private Color color;

    public Quartz()
    {
    }

    public Quartz(int bigness, Color color)
    {
      this.loadSprite();
      this.health = (float) (10 - (3 - bigness) * 2);
      this.bigness = bigness;
      if (bigness >= 3)
        this.bigness = 2;
      this.color = color;
    }

    public override void loadSprite()
    {
      try
      {
        this.texture = Game1.content.Load<Texture2D>("TerrainFeatures\\Quartz");
      }
      catch (Exception ex)
      {
      }
      this.identifier = Game1.random.Next(-999999, 999999);
    }

    public override Rectangle getBoundingBox(Vector2 tileLocation)
    {
      int bigness = this.bigness;
      return new Rectangle((int) ((double) tileLocation.X * (double) Game1.tileSize), (int) ((double) tileLocation.Y * (double) Game1.tileSize), Game1.tileSize, Game1.tileSize);
    }

    public override bool tickUpdate(GameTime time, Vector2 tileLocation)
    {
      if ((double) this.glow > 0.0)
        this.glow = this.glow - 0.01f;
      if ((double) this.maxShake > 0.0)
      {
        if (this.shakeLeft)
        {
          this.shakeRotation = this.shakeRotation - (float) Math.PI / 200f;
          if ((double) this.shakeRotation <= -(double) this.maxShake)
            this.shakeLeft = false;
        }
        else
        {
          this.shakeRotation = this.shakeRotation + (float) Math.PI / 200f;
          if ((double) this.shakeRotation >= (double) this.maxShake)
            this.shakeLeft = true;
        }
      }
      if ((double) this.maxShake > 0.0)
        this.maxShake = Math.Max(0.0f, this.maxShake - 0.003067962f);
      return (double) this.health <= 0.0;
    }

    public override void performPlayerEntryAction(Vector2 tileLocation)
    {
      Color color = ((double) this.glow > 0.0 ? new Color((float) this.color.R + this.glow * 50f, (float) this.color.G + this.glow * 50f, (float) this.color.B + this.glow * 50f) : this.color) * (0.3f + this.glow);
      if (this.bigness < 2)
      {
        Game1.currentLightSources.Add(new LightSource(4, new Vector2(tileLocation.X * (float) Game1.tileSize + (float) (Game1.tileSize / 2), tileLocation.Y * (float) Game1.tileSize + (float) (Game1.tileSize / 2)), 1f, Utility.getOppositeColor(color), (int) ((double) tileLocation.X * 1000.0 + (double) tileLocation.Y)));
      }
      else
      {
        if (this.bigness != 2)
          return;
        Game1.currentLightSources.Add(new LightSource(4, new Vector2(tileLocation.X * (float) Game1.tileSize + (float) (Game1.tileSize / 2), tileLocation.Y * (float) Game1.tileSize + (float) (Game1.tileSize / 2)), 1f, Utility.getOppositeColor(color), (int) ((double) tileLocation.X * 1000.0 + (double) tileLocation.Y)));
        Game1.currentLightSources.Add(new LightSource(4, new Vector2(tileLocation.X * (float) Game1.tileSize + (float) (Game1.tileSize / 2), tileLocation.Y * (float) Game1.tileSize - (float) (Game1.tileSize / 2)), 1f, Utility.getOppositeColor(color), (int) ((double) tileLocation.X * 1000.0 + (double) tileLocation.Y)));
      }
    }

    private void shake(Vector2 tileLocation)
    {
      if ((double) this.maxShake != 0.0)
        return;
      this.shakeLeft = (double) Game1.player.getTileLocation().X > (double) tileLocation.X || (double) Game1.player.getTileLocation().X == (double) tileLocation.X && Game1.random.NextDouble() < 0.5;
      this.maxShake = (float) Math.PI / 128f;
    }

    public override bool performUseAction(Vector2 tileLocation)
    {
      if (Game1.soundBank != null)
      {
        Random random = new Random((int) ((double) Game1.uniqueIDForThisGame + (double) tileLocation.X * 7.0 + (double) tileLocation.Y * 11.0 + (double) Game1.mine.mineLevel));
        Cue cue = Game1.soundBank.GetCue("crystal");
        int maxValue = 2400;
        int num1 = random.Next(maxValue);
        int num2 = num1 - num1 % 100;
        cue.SetVariable("Pitch", (float) num2);
        cue.Play();
      }
      this.glow = 0.7f;
      return false;
    }

    public override bool isPassable(Character c = null)
    {
      return (double) this.health <= 0.0;
    }

    public override void dayUpdate(GameLocation environment, Vector2 tileLocation)
    {
    }

    public override bool seasonUpdate(bool onLoad)
    {
      return false;
    }

    private Rectangle getSourceRect(int size)
    {
      switch (size)
      {
        case 0:
          return new Rectangle(Game1.tileSize, 0, Game1.tileSize, Game1.tileSize);
        case 1:
          return new Rectangle(4 * Game1.tileSize + ((double) this.health <= 3.0 ? Game1.tileSize : 0), Game1.tileSize, Game1.tileSize, Game1.tileSize);
        case 2:
          return new Rectangle((int) ((8.0 - (double) this.health) / 2.0) * Game1.tileSize, 0, Game1.tileSize, Game1.tileSize * 2);
        default:
          return Rectangle.Empty;
      }
    }

    public override bool performToolAction(Tool t, int explosion, Vector2 tileLocation, GameLocation location = null)
    {
      if ((double) this.health > 0.0)
      {
        float num1 = 0.0f;
        if (t == null && explosion > 0)
          num1 = (float) explosion;
        else if (t.name.Contains("Pickaxe"))
        {
          switch (t.upgradeLevel)
          {
            case 0:
              num1 = 2f;
              break;
            case 1:
              num1 = 2.5f;
              break;
            case 2:
              num1 = 3.34f;
              break;
            case 3:
              num1 = 5f;
              break;
            case 4:
              num1 = 10f;
              break;
          }
          Game1.playSound("hammer");
        }
        if ((double) num1 > 0.0)
        {
          this.glow = 0.7f;
          this.shake(tileLocation);
          this.health = this.health - num1;
          if ((double) this.health <= 0.0)
          {
            Random random1 = new Random((int) ((double) Game1.uniqueIDForThisGame + (double) tileLocation.X * 7.0 + (double) tileLocation.Y * 11.0 + (double) Game1.mine.mineLevel + (double) Game1.player.timesReachedMineBottom));
            double num2 = 1.0 + Game1.dailyLuck + (double) Game1.player.LuckLevel / 100.0 + (double) Game1.player.miningLevel / 50.0;
            if (random1.NextDouble() < 0.005 * num2)
              Game1.createObjectDebris(74, (int) tileLocation.X, (int) tileLocation.Y, -1, 0, 1f, (GameLocation) null);
            else if (random1.NextDouble() < 0.007 * num2)
              Game1.createDebris(10, (int) tileLocation.X, (int) tileLocation.Y, 2, (GameLocation) null);
            else if (random1.NextDouble() < 0.02 * num2)
              Game1.createObjectDebris(72, (int) tileLocation.X, (int) tileLocation.Y, -1, 0, 1f, (GameLocation) null);
            else if (random1.NextDouble() < 0.03 * num2)
              Game1.createObjectDebris(Game1.mine.mineLevel < 40 ? 86 : (Game1.mine.mineLevel < 80 ? 84 : 82), (int) tileLocation.X, (int) tileLocation.Y, -1, 0, 1f, (GameLocation) null);
            else if (random1.NextDouble() < 0.04 * num2)
              Game1.createObjectDebris(338, (int) tileLocation.X, (int) tileLocation.Y, -1, 0, 1f, (GameLocation) null);
            for (int index = 0; index < this.bigness * 3; ++index)
            {
              Random random2 = Game1.random;
              int x1 = this.getBoundingBox(tileLocation).X;
              Rectangle boundingBox = this.getBoundingBox(tileLocation);
              int right = boundingBox.Right;
              int num3 = random2.Next(x1, right);
              Random random3 = Game1.random;
              int y = this.getBoundingBox(tileLocation).Y;
              boundingBox = this.getBoundingBox(tileLocation);
              int bottom = boundingBox.Bottom;
              int num4 = random3.Next(y, bottom);
              List<TemporaryAnimatedSprite> temporarySprites = Game1.currentLocation.TemporarySprites;
              Texture2D texture = this.texture;
              Vector2 startingPosition = new Vector2((float) num3, (float) num4);
              double num5 = (double) Game1.random.Next(-25, 25) / 100.0;
              int num6 = num3;
              boundingBox = this.getBoundingBox(tileLocation);
              int x2 = boundingBox.Center.X;
              double num7 = (double) (num6 - x2) / 30.0;
              double num8 = (double) Game1.random.Next(-800, -100) / 100.0;
              int groundYLevel = (int) tileLocation.Y * Game1.tileSize + Game1.tileSize;
              Rectangle sourceRect = new Rectangle(Game1.random.Next(4, 8) * Game1.tileSize, 0, Game1.tileSize, Game1.tileSize);
              Color color = this.color;
              Cue tapSound = Game1.soundBank != null ? Game1.soundBank.GetCue("boulderCrack") : (Cue) null;
              LightSource light = new LightSource(4, Vector2.Zero, 0.1f, Utility.getOppositeColor(this.color));
              int lightTailLength = 24;
              int disappearTime = 1000;
              CosmeticDebris cosmeticDebris = new CosmeticDebris(texture, startingPosition, (float) num5, (float) num7, (float) num8, groundYLevel, sourceRect, color, tapSound, light, lightTailLength, disappearTime);
              temporarySprites.Add((TemporaryAnimatedSprite) cosmeticDebris);
            }
            Utility.removeLightSource((int) ((double) tileLocation.X * 1000.0 + (double) tileLocation.Y));
          }
        }
      }
      return false;
    }

    private Vector2 getPivot()
    {
      switch (this.bigness)
      {
        case 1:
          return new Vector2((float) (Game1.tileSize / 2), (float) Game1.tileSize);
        case 2:
          return new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize * 2));
        case 3:
          return new Vector2((float) Game1.tileSize, (float) (Game1.tileSize * 3));
        default:
          return Vector2.Zero;
      }
    }

    public override void draw(SpriteBatch spriteBatch, Vector2 tileLocation)
    {
      if ((double) this.health <= 0.0)
        return;
      spriteBatch.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) this.getBoundingBox(tileLocation).Center.X, (float) this.getBoundingBox(tileLocation).Bottom)), new Rectangle?(this.getSourceRect(this.bigness)), this.color, this.shakeRotation, this.getPivot(), 1f, SpriteEffects.None, (float) (((double) tileLocation.Y * (double) Game1.tileSize + (double) Game1.tileSize) / 10000.0));
    }
  }
}
