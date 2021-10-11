// Decompiled with JetBrains decompiler
// Type: StardewValley.TerrainFeatures.Stalagmite
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StardewValley.TerrainFeatures
{
  public class Stalagmite : TerrainFeature
  {
    private List<Leaf> leaves = new List<Leaf>();
    public const float shakeRate = 0.01227185f;
    public const float shakeDecayRate = 0.006135923f;
    public const int minWoodDebrisForFallenTree = 8;
    public const int minWoodDebrisForStump = 4;
    public const int startingHealth = 10;
    public const int leafFallRate = 3;
    public const int bushyTree = 1;
    public const int leafyTree = 2;
    public const int pineTree = 3;
    public const int winterTree1 = 4;
    public const int winterTree2 = 5;
    public const int palmTree = 6;
    public const int seedStage = 0;
    public const int sproutStage = 1;
    public const int saplingStage = 2;
    public const int bushStage = 3;
    public const int treeStage = 5;
    private Texture2D texture;
    public float health;
    public bool stump;
    private bool shakeLeft;
    private bool falling;
    private bool tall;
    private bool drop;
    private float shakeRotation;
    private float maxShake;
    private float dropY;

    public Stalagmite()
    {
    }

    public Stalagmite(bool tall)
    {
      this.loadSprite();
      this.health = 10f;
      this.tall = tall;
    }

    public override void loadSprite()
    {
      try
      {
        string str = Game1.mine.mineLevel < 40 || Game1.mine.mineLevel >= 80 ? (Game1.mine.mineLevel < 40 ? "" : "_Lava") : "_Frost";
        this.texture = Game1.content.Load<Texture2D>("TerrainFeatures\\Stalagmite" + str);
      }
      catch (Exception ex)
      {
        this.texture = Game1.content.Load<Texture2D>("TerrainFeatures\\Stalagmite");
      }
    }

    public override Rectangle getBoundingBox(Vector2 tileLocation)
    {
      if ((double) this.health > 0.0)
        return new Rectangle((int) tileLocation.X * Game1.tileSize, (int) tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
      return Rectangle.Empty;
    }

    public override bool performUseAction(Vector2 tileLocation)
    {
      this.shake(tileLocation);
      return false;
    }

    private int extraStoneCalculator(Vector2 tileLocation)
    {
      Random random = new Random((int) Game1.uniqueIDForThisGame + (int) Game1.stats.DaysPlayed + (int) tileLocation.X * 7 + (int) tileLocation.Y * 11);
      int num = 0;
      if (random.NextDouble() < Game1.dailyLuck)
        ++num;
      if (random.NextDouble() < (double) Game1.player.MiningLevel / 12.5)
        ++num;
      if (random.NextDouble() < (double) Game1.player.MiningLevel / 12.5)
        ++num;
      if (random.NextDouble() < (double) Game1.player.LuckLevel / 25.0)
        ++num;
      return num;
    }

    public override bool tickUpdate(GameTime time, Vector2 tileLocation)
    {
      if (!this.falling)
      {
        if ((double) this.maxShake > 0.0)
        {
          if (this.shakeLeft)
          {
            this.shakeRotation = this.shakeRotation - (float) Math.PI / 256f;
            if ((double) this.shakeRotation <= -(double) this.maxShake)
              this.shakeLeft = false;
          }
          else
          {
            this.shakeRotation = this.shakeRotation + (float) Math.PI / 256f;
            if ((double) this.shakeRotation >= (double) this.maxShake)
              this.shakeLeft = true;
          }
        }
        if ((double) this.maxShake > 0.0)
          this.maxShake = Math.Max(0.0f, this.maxShake - (float) Math.PI / 512f);
        if (this.drop)
        {
          this.dropY = this.dropY + 10f;
          if ((double) this.dropY >= (double) tileLocation.Y * (double) Game1.tileSize - (double) (Game1.tileSize * 2))
          {
            this.drop = false;
            Game1.playSound("cavedrip");
            Game1.createWaterDroplets(this.texture, new Rectangle(Game1.tileSize, 0, 4, 4), (int) tileLocation.X * Game1.tileSize + Game1.tileSize, (int) ((double) tileLocation.Y - 2.0) * Game1.tileSize, Game1.random.Next(4, 5), (int) ((double) tileLocation.Y + 1.0));
          }
        }
        if (!this.drop && Game1.random.NextDouble() < 0.005)
        {
          this.drop = true;
          this.dropY = tileLocation.Y * (float) Game1.tileSize - (float) Game1.viewport.Height;
        }
      }
      else
      {
        this.shakeRotation = this.shakeRotation + (this.shakeLeft ? (float) -((double) this.maxShake * (double) this.maxShake) : this.maxShake * this.maxShake);
        this.maxShake = this.maxShake + 0.002045308f;
        if ((double) Math.Abs(this.shakeRotation) > Math.PI / 2.0)
        {
          this.falling = false;
          this.maxShake = 0.0f;
          Game1.playSound("stoneCrack");
          Game1.createRadialDebris(Game1.currentLocation, 14, (int) tileLocation.X + (this.shakeLeft ? -2 : 2), (int) tileLocation.Y, 8 + this.extraStoneCalculator(tileLocation), true, -1, false, -1);
          Game1.createRadialDebris(Game1.currentLocation, this.texture, new Rectangle(Game1.tileSize / 4, Game1.tileSize, Game1.tileSize / 4, Game1.tileSize / 4), (int) tileLocation.X + (this.shakeLeft ? -2 : 2), (int) tileLocation.Y, Game1.random.Next(40, 60));
          if ((double) this.health <= 0.0)
            return true;
        }
      }
      return false;
    }

    private void shake(Vector2 tileLocation)
    {
      if ((double) this.maxShake != 0.0 || this.stump)
        return;
      this.shakeLeft = (double) Game1.player.getTileLocation().X > (double) tileLocation.X || (double) Game1.player.getTileLocation().X == (double) tileLocation.X && Game1.random.NextDouble() < 0.5;
      this.maxShake = (float) Math.PI / 64f;
    }

    public override bool isPassable(Character c = null)
    {
      return (double) this.health <= -99.0;
    }

    public override void dayUpdate(GameLocation environment, Vector2 tileLocation)
    {
    }

    public override bool seasonUpdate(bool onLoad)
    {
      return false;
    }

    public override bool performToolAction(Tool t, int explosion, Vector2 tileLocation, GameLocation location = null)
    {
      if ((double) this.health <= -99.0)
        return false;
      if (t != null && t.name.Contains("Pickaxe"))
      {
        Game1.playSound("hammer");
        Game1.currentLocation.debris.Add(new Debris(this.texture, new Rectangle(Game1.tileSize / 4, Game1.tileSize, Game1.tileSize / 2, Game1.tileSize / 2), Game1.random.Next(t.upgradeLevel * 2, t.upgradeLevel * 4), Game1.player.GetToolLocation(false) + new Vector2((float) (Game1.tileSize / 4), 0.0f)));
      }
      else if (explosion <= 0)
        return false;
      this.shake(tileLocation);
      float num = 1f;
      if (explosion > 0)
      {
        num = (float) explosion;
      }
      else
      {
        if (t == null)
          return false;
        switch (t.upgradeLevel)
        {
          case 0:
            num = 1f;
            break;
          case 1:
            num = 1.25f;
            break;
          case 2:
            num = 1.67f;
            break;
          case 3:
            num = 2.5f;
            break;
          case 4:
            num = 5f;
            break;
        }
      }
      this.health = this.health - num;
      if ((double) this.health <= 0.0)
      {
        if (!this.stump)
        {
          Game1.playSound("treecrack");
          this.maxShake = 0.0f;
          this.stump = true;
          this.health = 1f;
          this.falling = true;
          this.shakeLeft = (double) Game1.player.getTileLocation().X >= (double) tileLocation.X && ((double) Game1.player.getTileLocation().X != (double) tileLocation.X || Game1.random.NextDouble() >= 0.5);
        }
        else
        {
          this.health = -100f;
          Game1.createRadialDebris(Game1.currentLocation, this.texture, new Rectangle(2 * Game1.tileSize + Game1.tileSize / 4, Game1.tileSize * 7, Game1.tileSize / 2, Game1.tileSize / 2), (int) tileLocation.X, (int) tileLocation.Y, Game1.random.Next(30, 40));
          Game1.createRadialDebris(Game1.currentLocation, 14, (int) tileLocation.X, (int) tileLocation.Y, 1, true, -1, false, -1);
          if (!this.falling)
            return true;
        }
      }
      return false;
    }

    public override void draw(SpriteBatch spriteBatch, Vector2 tileLocation)
    {
      Rectangle boundingBox;
      if (!this.stump || this.falling)
      {
        if (this.tall)
        {
          SpriteBatch spriteBatch1 = spriteBatch;
          Texture2D texture = this.texture;
          Vector2 local = Game1.GlobalToLocal(Game1.viewport, new Vector2((float) ((double) tileLocation.X * (double) Game1.tileSize + (!this.falling || !this.shakeLeft ? (this.falling ? (double) (Game1.tileSize * 3 / 4) : (double) (Game1.tileSize / 2)) : (double) (Game1.tileSize / 4))), tileLocation.Y * (float) Game1.tileSize));
          Rectangle? sourceRectangle = new Rectangle?(new Rectangle(Game1.tileSize, Game1.tileSize, Game1.tileSize, Game1.tileSize * 3));
          Color white = Color.White;
          double shakeRotation = (double) this.shakeRotation;
          Vector2 origin = new Vector2(!this.falling || !this.shakeLeft ? (this.falling ? (float) (Game1.tileSize * 3 / 4) : (float) (Game1.tileSize / 2)) : (float) (Game1.tileSize / 4), (float) (Game1.tileSize * 2));
          double num1 = 1.0;
          int num2 = 0;
          boundingBox = this.getBoundingBox(tileLocation);
          double num3 = (double) boundingBox.Bottom / 10000.0 + 9.99999997475243E-07;
          spriteBatch1.Draw(texture, local, sourceRectangle, white, (float) shakeRotation, origin, (float) num1, (SpriteEffects) num2, (float) num3);
        }
        else
        {
          SpriteBatch spriteBatch1 = spriteBatch;
          Texture2D texture = this.texture;
          Vector2 local = Game1.GlobalToLocal(Game1.viewport, new Vector2((float) ((double) tileLocation.X * (double) Game1.tileSize + (!this.falling || !this.shakeLeft ? (this.falling ? (double) (Game1.tileSize * 3 / 4) : (double) (Game1.tileSize / 2)) : (double) (Game1.tileSize / 4))), tileLocation.Y * (float) Game1.tileSize));
          Rectangle? sourceRectangle = new Rectangle?(new Rectangle(0, 0, Game1.tileSize, Game1.tileSize * 3));
          Color white = Color.White;
          double shakeRotation = (double) this.shakeRotation;
          Vector2 origin = new Vector2(!this.falling || !this.shakeLeft ? (this.falling ? (float) (Game1.tileSize * 3 / 4) : (float) (Game1.tileSize / 2)) : (float) (Game1.tileSize / 4), (float) (Game1.tileSize * 2));
          double num1 = 1.0;
          int num2 = 0;
          boundingBox = this.getBoundingBox(tileLocation);
          double num3 = (double) boundingBox.Bottom / 10000.0 + 9.99999997475243E-07;
          spriteBatch1.Draw(texture, local, sourceRectangle, white, (float) shakeRotation, origin, (float) num1, (SpriteEffects) num2, (float) num3);
        }
      }
      if ((double) this.health > 0.0 || !this.falling)
      {
        SpriteBatch spriteBatch1 = spriteBatch;
        Texture2D texture = this.texture;
        Vector2 local = Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float) Game1.tileSize, tileLocation.Y * (float) Game1.tileSize));
        Rectangle? sourceRectangle = new Rectangle?(new Rectangle(0, 3 * Game1.tileSize, Game1.tileSize, Game1.tileSize));
        Color white = Color.White;
        double num1 = 0.0;
        Vector2 zero = Vector2.Zero;
        double num2 = 1.0;
        int num3 = 0;
        boundingBox = this.getBoundingBox(tileLocation);
        double num4 = (double) boundingBox.Bottom / 10000.0;
        spriteBatch1.Draw(texture, local, sourceRectangle, white, (float) num1, zero, (float) num2, (SpriteEffects) num3, (float) num4);
      }
      if (!this.drop)
        return;
      spriteBatch.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) Game1.tileSize * tileLocation.X + (float) (Game1.tileSize / 2), this.dropY)), new Rectangle?(new Rectangle(Game1.tileSize, 0, 4, 8)), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9999f);
    }
  }
}
