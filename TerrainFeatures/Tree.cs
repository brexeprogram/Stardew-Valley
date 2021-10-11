// Decompiled with JetBrains decompiler
// Type: StardewValley.TerrainFeatures.Tree
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using xTile.Dimensions;

namespace StardewValley.TerrainFeatures
{
  public class Tree : TerrainFeature
  {
    public static Microsoft.Xna.Framework.Rectangle treeTopSourceRect = new Microsoft.Xna.Framework.Rectangle(0, 0, 48, 96);
    public static Microsoft.Xna.Framework.Rectangle stumpSourceRect = new Microsoft.Xna.Framework.Rectangle(32, 96, 16, 32);
    public static Microsoft.Xna.Framework.Rectangle shadowSourceRect = new Microsoft.Xna.Framework.Rectangle(663, 1011, 41, 30);
    private float alpha = 1f;
    private List<Leaf> leaves = new List<Leaf>();
    public const float chanceForDailySeed = 0.05f;
    public const float shakeRate = 0.01570796f;
    public const float shakeDecayRate = 0.003067962f;
    public const int minWoodDebrisForFallenTree = 12;
    public const int minWoodDebrisForStump = 5;
    public const int startingHealth = 10;
    public const int leafFallRate = 3;
    public const int bushyTree = 1;
    public const int leafyTree = 2;
    public const int pineTree = 3;
    public const int winterTree1 = 4;
    public const int winterTree2 = 5;
    public const int palmTree = 6;
    public const int mushroomTree = 7;
    public const int seedStage = 0;
    public const int sproutStage = 1;
    public const int saplingStage = 2;
    public const int bushStage = 3;
    public const int treeStage = 5;
    private Texture2D texture;
    public int growthStage;
    public int treeType;
    public float health;
    public bool flipped;
    public bool stump;
    public bool tapped;
    public bool hasSeed;
    private bool shakeLeft;
    private bool falling;
    private bool destroy;
    private float shakeRotation;
    private float maxShake;
    private long lastPlayerToHit;
    private float shakeTimer;

    public Tree()
    {
    }

    public Tree(int which, int growthStage)
    {
      this.growthStage = growthStage;
      this.treeType = which;
      this.loadSprite();
      this.flipped = Game1.random.NextDouble() < 0.5;
      this.health = 10f;
    }

    public Tree(int which)
    {
      this.treeType = which;
      this.loadSprite();
      this.flipped = Game1.random.NextDouble() < 0.5;
      this.health = 10f;
    }

    public override void loadSprite()
    {
      try
      {
        if (this.treeType == 7)
          this.texture = Game1.content.Load<Texture2D>("TerrainFeatures\\mushroom_tree");
        else if (this.treeType == 6)
        {
          this.texture = Game1.content.Load<Texture2D>("TerrainFeatures\\tree_palm");
        }
        else
        {
          if (this.treeType == 4)
            this.treeType = 1;
          if (this.treeType == 5)
            this.treeType = 2;
          string str = Game1.currentSeason;
          if (this.treeType == 3 && str.Equals("summer"))
            str = "spring";
          this.texture = Game1.content.Load<Texture2D>("TerrainFeatures\\tree" + (object) Math.Max(1, this.treeType) + "_" + str);
        }
      }
      catch (Exception ex)
      {
      }
    }

    public override Microsoft.Xna.Framework.Rectangle getBoundingBox(Vector2 tileLocation)
    {
      switch (this.growthStage)
      {
        case 0:
        case 1:
        case 2:
          return new Microsoft.Xna.Framework.Rectangle((int) tileLocation.X * Game1.tileSize + Game1.tileSize / 5, (int) tileLocation.Y * Game1.tileSize + Game1.tileSize / 4, Game1.tileSize * 3 / 5, Game1.tileSize * 3 / 4);
        default:
          return new Microsoft.Xna.Framework.Rectangle((int) tileLocation.X * Game1.tileSize, (int) tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
      }
    }

    public override bool performUseAction(Vector2 tileLocation)
    {
      if (!this.tapped)
      {
        if ((double) this.maxShake == 0.0 && !this.stump && this.growthStage >= 3 && (!Game1.currentSeason.Equals("winter") || this.treeType == 3))
          Game1.playSound("leafrustle");
        this.shake(tileLocation, false);
      }
      return false;
    }

    private int extraWoodCalculator(Vector2 tileLocation)
    {
      Random random = new Random((int) Game1.uniqueIDForThisGame + (int) Game1.stats.DaysPlayed + (int) tileLocation.X * 7 + (int) tileLocation.Y * 11);
      int num = 0;
      if (random.NextDouble() < Game1.dailyLuck)
        ++num;
      if (random.NextDouble() < (double) Game1.player.ForagingLevel / 12.5)
        ++num;
      if (random.NextDouble() < (double) Game1.player.ForagingLevel / 12.5)
        ++num;
      if (random.NextDouble() < (double) Game1.player.LuckLevel / 25.0)
        ++num;
      return num;
    }

    public override bool tickUpdate(GameTime time, Vector2 tileLocation)
    {
      if ((double) this.shakeTimer > 0.0)
        this.shakeTimer = this.shakeTimer - (float) time.ElapsedGameTime.Milliseconds;
      if (this.destroy)
        return true;
      this.alpha = Math.Min(1f, this.alpha + 0.05f);
      if (this.growthStage >= 5 && !this.falling && !this.stump && Game1.player.GetBoundingBox().Intersects(new Microsoft.Xna.Framework.Rectangle(Game1.tileSize * ((int) tileLocation.X - 1), Game1.tileSize * ((int) tileLocation.Y - 5), 3 * Game1.tileSize, 4 * Game1.tileSize + Game1.tileSize / 2)))
        this.alpha = Math.Max(0.4f, this.alpha - 0.09f);
      if (!this.falling)
      {
        if ((double) Math.Abs(this.shakeRotation) > Math.PI / 2.0 && this.leaves.Count <= 0 && (double) this.health <= 0.0)
          return true;
        if ((double) this.maxShake > 0.0)
        {
          if (this.shakeLeft)
          {
            this.shakeRotation = this.shakeRotation - (this.growthStage >= 5 ? (float) Math.PI / 600f : (float) Math.PI / 200f);
            if ((double) this.shakeRotation <= -(double) this.maxShake)
              this.shakeLeft = false;
          }
          else
          {
            this.shakeRotation = this.shakeRotation + (this.growthStage >= 5 ? (float) Math.PI / 600f : (float) Math.PI / 200f);
            if ((double) this.shakeRotation >= (double) this.maxShake)
              this.shakeLeft = true;
          }
        }
        if ((double) this.maxShake > 0.0)
          this.maxShake = Math.Max(0.0f, this.maxShake - (this.growthStage >= 5 ? 0.001022654f : 0.003067962f));
      }
      else
      {
        this.shakeRotation = this.shakeRotation + (this.shakeLeft ? (float) -((double) this.maxShake * (double) this.maxShake) : this.maxShake * this.maxShake);
        this.maxShake = this.maxShake + 0.001533981f;
        if (Game1.random.NextDouble() < 0.01 && this.treeType != 7)
          Game1.playSound("leafrustle");
        if ((double) Math.Abs(this.shakeRotation) > Math.PI / 2.0)
        {
          this.falling = false;
          this.maxShake = 0.0f;
          Game1.playSound("treethud");
          int num = Game1.random.Next(90, 120);
          if (Game1.currentLocation.Objects.ContainsKey(tileLocation))
            Game1.currentLocation.Objects.Remove(tileLocation);
          for (int index = 0; index < num; ++index)
            this.leaves.Add(new Leaf(new Vector2((float) (Game1.random.Next((int) ((double) tileLocation.X * (double) Game1.tileSize), (int) ((double) tileLocation.X * (double) Game1.tileSize + (double) (Game1.tileSize * 3))) + (this.shakeLeft ? -Game1.tileSize * 5 : Game1.tileSize * 4)), tileLocation.Y * (float) Game1.tileSize - (float) Game1.tileSize), (float) Game1.random.Next(-10, 10) / 100f, Game1.random.Next(4), (float) Game1.random.Next(10, 40) / 10f));
          if (this.treeType != 7)
          {
            Game1.createRadialDebris(Game1.currentLocation, 12, (int) tileLocation.X + (this.shakeLeft ? -4 : 4), (int) tileLocation.Y, 12 + this.extraWoodCalculator(tileLocation), true, -1, false, -1);
            Game1.createRadialDebris(Game1.currentLocation, 12, (int) tileLocation.X + (this.shakeLeft ? -4 : 4), (int) tileLocation.Y, 12 + this.extraWoodCalculator(tileLocation), false, -1, false, -1);
            Random random;
            if (Game1.IsMultiplayer)
            {
              Game1.recentMultiplayerRandom = new Random((int) tileLocation.X * 1000 + (int) tileLocation.Y);
              random = Game1.recentMultiplayerRandom;
            }
            else
              random = new Random((int) Game1.uniqueIDForThisGame + (int) Game1.stats.DaysPlayed + (int) tileLocation.X * 7 + (int) tileLocation.Y * 11);
            if (Game1.IsMultiplayer)
            {
              Game1.createMultipleObjectDebris(92, (int) tileLocation.X + (this.shakeLeft ? -4 : 4), (int) tileLocation.Y, 5, this.lastPlayerToHit);
              int number = 0;
              if (Game1.getFarmer(this.lastPlayerToHit) != null)
              {
                while (Game1.getFarmer(this.lastPlayerToHit).professions.Contains(14) && random.NextDouble() < 0.4)
                  ++number;
              }
              if (number > 0)
                Game1.createMultipleObjectDebris(709, (int) tileLocation.X + (this.shakeLeft ? -4 : 4), (int) tileLocation.Y, number, this.lastPlayerToHit);
              if (Game1.getFarmer(this.lastPlayerToHit).getEffectiveSkillLevel(2) >= 1 && random.NextDouble() < 0.75)
                Game1.createMultipleObjectDebris(308 + this.treeType, (int) tileLocation.X + (this.shakeLeft ? -4 : 4), (int) tileLocation.Y, random.Next(1, 3), this.lastPlayerToHit);
            }
            else
            {
              Game1.createMultipleObjectDebris(92, (int) tileLocation.X + (this.shakeLeft ? -4 : 4), (int) tileLocation.Y, 5);
              int number = 0;
              if (Game1.getFarmer(this.lastPlayerToHit) != null)
              {
                while (Game1.getFarmer(this.lastPlayerToHit).professions.Contains(14) && random.NextDouble() < 0.4)
                  ++number;
              }
              if (number > 0)
                Game1.createMultipleObjectDebris(709, (int) tileLocation.X + (this.shakeLeft ? -4 : 4), (int) tileLocation.Y, number);
              if (this.lastPlayerToHit != 0L && Game1.getFarmer(this.lastPlayerToHit).getEffectiveSkillLevel(2) >= 1 && (random.NextDouble() < 0.75 && this.treeType < 4))
                Game1.createMultipleObjectDebris(308 + this.treeType, (int) tileLocation.X + (this.shakeLeft ? -4 : 4), (int) tileLocation.Y, random.Next(1, 3));
            }
          }
          else if (!Game1.IsMultiplayer)
            Game1.createMultipleObjectDebris(420, (int) tileLocation.X + (this.shakeLeft ? -4 : 4), (int) tileLocation.Y, 5);
          if ((double) this.health == -100.0)
            return true;
          if ((double) this.health <= 0.0)
            this.health = -100f;
        }
      }
      for (int index = this.leaves.Count - 1; index >= 0; --index)
      {
        this.leaves.ElementAt<Leaf>(index).position.Y -= this.leaves.ElementAt<Leaf>(index).yVelocity - 3f;
        this.leaves.ElementAt<Leaf>(index).yVelocity = Math.Max(0.0f, this.leaves.ElementAt<Leaf>(index).yVelocity - 0.01f);
        this.leaves.ElementAt<Leaf>(index).rotation += this.leaves.ElementAt<Leaf>(index).rotationRate;
        if ((double) this.leaves.ElementAt<Leaf>(index).position.Y >= (double) tileLocation.Y * (double) Game1.tileSize + (double) Game1.tileSize)
          this.leaves.RemoveAt(index);
      }
      return false;
    }

    private void shake(Vector2 tileLocation, bool doEvenIfStillShaking)
    {
      if ((double) this.maxShake == 0.0 | doEvenIfStillShaking && this.growthStage >= 3 && !this.stump)
      {
        this.shakeLeft = (double) Game1.player.getTileLocation().X > (double) tileLocation.X || (double) Game1.player.getTileLocation().X == (double) tileLocation.X && Game1.random.NextDouble() < 0.5;
        this.maxShake = this.growthStage >= 5 ? (float) Math.PI / 128f : (float) Math.PI / 64f;
        if (this.growthStage >= 5)
        {
          if (Game1.random.NextDouble() < 0.66)
          {
            int num = Game1.random.Next(1, 6);
            for (int index = 0; index < num; ++index)
              this.leaves.Add(new Leaf(new Vector2((float) Game1.random.Next((int) ((double) tileLocation.X * (double) Game1.tileSize - (double) Game1.tileSize), (int) ((double) tileLocation.X * (double) Game1.tileSize + (double) (Game1.tileSize * 2))), (float) Game1.random.Next((int) ((double) tileLocation.Y * (double) Game1.tileSize - (double) (Game1.tileSize * 4)), (int) ((double) tileLocation.Y * (double) Game1.tileSize - (double) (Game1.tileSize * 3)))), (float) Game1.random.Next(-10, 10) / 100f, Game1.random.Next(4), (float) Game1.random.Next(5) / 10f));
          }
          if (Game1.random.NextDouble() < 0.01 && (Game1.currentSeason.Equals("spring") || Game1.currentSeason.Equals("summer")))
          {
            while (Game1.random.NextDouble() < 0.8)
              Game1.currentLocation.addCritter((Critter) new Butterfly(new Vector2(tileLocation.X + (float) Game1.random.Next(1, 3), tileLocation.Y - 2f + (float) Game1.random.Next(-1, 2))));
          }
          if (!this.hasSeed || !Game1.IsMultiplayer && Game1.player.ForagingLevel < 1)
            return;
          int objectIndex = -1;
          switch (this.treeType)
          {
            case 1:
              objectIndex = 309;
              break;
            case 2:
              objectIndex = 310;
              break;
            case 3:
              objectIndex = 311;
              break;
            case 6:
              objectIndex = 88;
              break;
          }
          if (Game1.currentSeason.Equals("fall") && this.treeType == 2 && Game1.dayOfMonth >= 14)
            objectIndex = 408;
          if (objectIndex != -1)
            Game1.createObjectDebris(objectIndex, (int) tileLocation.X, (int) tileLocation.Y - 3, ((int) tileLocation.Y + 1) * Game1.tileSize, 0, 1f, (GameLocation) null);
          this.hasSeed = false;
        }
        else
        {
          if (Game1.random.NextDouble() >= 0.66)
            return;
          int num = Game1.random.Next(1, 3);
          for (int index = 0; index < num; ++index)
            this.leaves.Add(new Leaf(new Vector2((float) Game1.random.Next((int) ((double) tileLocation.X * (double) Game1.tileSize), (int) ((double) tileLocation.X * (double) Game1.tileSize + (double) (Game1.tileSize * 3 / 4))), tileLocation.Y * (float) Game1.tileSize - (float) (Game1.tileSize / 2)), (float) Game1.random.Next(-10, 10) / 100f, Game1.random.Next(4), (float) Game1.random.Next(30) / 10f));
        }
      }
      else
      {
        if (!this.stump)
          return;
        this.shakeTimer = 100f;
      }
    }

    public override bool isPassable(Character c = null)
    {
      return (double) this.health <= -99.0 || this.growthStage == 0;
    }

    public override void dayUpdate(GameLocation environment, Vector2 tileLocation)
    {
      Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle((int) (((double) tileLocation.X - 1.0) * (double) Game1.tileSize), (int) (((double) tileLocation.Y - 1.0) * (double) Game1.tileSize), Game1.tileSize * 3, Game1.tileSize * 3);
      if ((double) this.health <= -100.0)
        this.destroy = true;
      if (!Game1.currentSeason.Equals("winter") || this.treeType == 6 || environment.Name.ToLower().Contains("greenhouse"))
      {
        string str = environment.doesTileHaveProperty((int) tileLocation.X, (int) tileLocation.Y, "NoSpawn", "Back");
        if (str != null && (str.Equals("All") || str.Equals(nameof (Tree)) || str.Equals("True")))
          return;
        if (this.growthStage == 4)
        {
          foreach (KeyValuePair<Vector2, TerrainFeature> terrainFeature in (Dictionary<Vector2, TerrainFeature>) environment.terrainFeatures)
          {
            if (terrainFeature.Value is Tree && !terrainFeature.Value.Equals((object) this) && ((Tree) terrainFeature.Value).growthStage >= 5 && terrainFeature.Value.getBoundingBox(terrainFeature.Key).Intersects(rectangle))
              return;
          }
        }
        else if (this.growthStage == 0 && environment.objects.ContainsKey(tileLocation))
          return;
        if (Game1.random.NextDouble() < 0.2)
          this.growthStage = this.growthStage + 1;
      }
      if (Game1.currentSeason.Equals("winter") && this.treeType == 7)
        this.stump = true;
      else if (this.treeType == 7 && Game1.dayOfMonth <= 1 && Game1.currentSeason.Equals("spring"))
      {
        this.stump = false;
        this.health = 10f;
      }
      if (this.growthStage >= 5 && environment is Farm && Game1.random.NextDouble() < 0.15)
      {
        int xTile = Game1.random.Next(-3, 4) + (int) tileLocation.X;
        int yTile = Game1.random.Next(-3, 4) + (int) tileLocation.Y;
        Vector2 vector2 = new Vector2((float) xTile, (float) yTile);
        string str = environment.doesTileHaveProperty(xTile, yTile, "NoSpawn", "Back");
        if ((str == null || !str.Equals(nameof (Tree)) && !str.Equals("All") && !str.Equals("True")) && (environment.isTileLocationOpen(new Location(xTile * Game1.tileSize, yTile * Game1.tileSize)) && !environment.isTileOccupied(vector2, "") && (environment.doesTileHaveProperty(xTile, yTile, "Water", "Back") == null && environment.isTileOnMap(vector2))))
          environment.terrainFeatures.Add(vector2, (TerrainFeature) new Tree(this.treeType, 0));
      }
      this.hasSeed = false;
      if (this.growthStage < 5 || Game1.random.NextDouble() >= 0.0500000007450581)
        return;
      this.hasSeed = true;
    }

    public override bool seasonUpdate(bool onLoad)
    {
      this.loadSprite();
      return false;
    }

    public override bool isActionable()
    {
      if (!this.tapped)
        return this.growthStage >= 3;
      return false;
    }

    public override bool performToolAction(Tool t, int explosion, Vector2 tileLocation, GameLocation location = null)
    {
      if (location == null)
        location = Game1.currentLocation;
      if (explosion > 0)
        this.tapped = false;
      if (this.tapped)
        return false;
      Console.WriteLine("TREE: IsClient:" + Game1.IsClient.ToString() + " randomOutput: " + (object) Game1.recentMultiplayerRandom.Next(9999));
      if ((double) this.health <= -99.0)
        return false;
      if (this.growthStage >= 5)
      {
        if (t != null && t is Axe)
        {
          Game1.playSound("axchop");
          location.debris.Add(new Debris(12, Game1.random.Next(1, 3), t.getLastFarmerToUse().GetToolLocation(false) + new Vector2((float) (Game1.tileSize / 4), 0.0f), t.getLastFarmerToUse().position, 0, -1));
          this.lastPlayerToHit = t.getLastFarmerToUse().uniqueMultiplayerID;
        }
        else if (explosion <= 0)
          return false;
        this.shake(tileLocation, true);
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
            if ((t != null || explosion > 0) && location.Equals((object) Game1.currentLocation))
              Game1.playSound("treecrack");
            this.stump = true;
            this.health = 5f;
            this.falling = true;
            if (t != null)
              t.getLastFarmerToUse().gainExperience(2, 12);
            this.shakeLeft = t == null || t.getLastFarmerToUse() == null || ((double) t.getLastFarmerToUse().getTileLocation().X > (double) tileLocation.X || (double) t.getLastFarmerToUse().getTileLocation().Y < (double) tileLocation.Y && (double) tileLocation.X % 2.0 == 0.0);
          }
          else
          {
            this.health = -100f;
            Game1.createRadialDebris(location, 12, (int) tileLocation.X, (int) tileLocation.Y, Game1.random.Next(30, 40), false, -1, false, -1);
            int index = this.treeType != 7 || (double) tileLocation.X % 7.0 != 0.0 ? (this.treeType == 7 ? 420 : 92) : 422;
            if (Game1.IsMultiplayer)
            {
              Game1.recentMultiplayerRandom = new Random((int) tileLocation.X * 2000 + (int) tileLocation.Y);
              Random multiplayerRandom = Game1.recentMultiplayerRandom;
            }
            else
            {
              Random random = new Random((int) Game1.uniqueIDForThisGame + (int) Game1.stats.DaysPlayed + (int) tileLocation.X * 7 + (int) tileLocation.Y * 11);
            }
            if (t == null || t.getLastFarmerToUse() == null)
            {
              if (location.Equals((object) Game1.currentLocation))
              {
                Game1.createMultipleObjectDebris(92, (int) tileLocation.X, (int) tileLocation.Y, 2);
              }
              else
              {
                Game1.createItemDebris((Item) new StardewValley.Object(92, 1, false, -1, 0), tileLocation * (float) Game1.tileSize, 2, location);
                Game1.createItemDebris((Item) new StardewValley.Object(92, 1, false, -1, 0), tileLocation * (float) Game1.tileSize, 2, location);
              }
            }
            else if (Game1.IsMultiplayer)
            {
              Game1.createMultipleObjectDebris(index, (int) tileLocation.X, (int) tileLocation.Y, 1, this.lastPlayerToHit);
              if (this.treeType != 7)
                Game1.createRadialDebris(location, 12, (int) tileLocation.X, (int) tileLocation.Y, 4, true, -1, false, -1);
            }
            else
            {
              if (this.treeType != 7)
                Game1.createRadialDebris(location, 12, (int) tileLocation.X, (int) tileLocation.Y, 5 + this.extraWoodCalculator(tileLocation), true, -1, false, -1);
              Game1.createMultipleObjectDebris(index, (int) tileLocation.X, (int) tileLocation.Y, 1);
            }
            if (location.Equals((object) Game1.currentLocation))
              Game1.playSound("treethud");
            if (!this.falling)
              return true;
          }
        }
      }
      else if (this.growthStage >= 3)
      {
        if (t != null && t.name.Contains("Ax"))
        {
          Game1.playSound("axchop");
          if (this.treeType != 7)
            Game1.playSound("leafrustle");
          location.debris.Add(new Debris(12, Game1.random.Next(t.upgradeLevel * 2, t.upgradeLevel * 4), t.getLastFarmerToUse().GetToolLocation(false) + new Vector2((float) (Game1.tileSize / 4), 0.0f), new Vector2((float) t.getLastFarmerToUse().GetBoundingBox().Center.X, (float) t.getLastFarmerToUse().GetBoundingBox().Center.Y), 0, -1));
        }
        else if (explosion <= 0)
          return false;
        this.shake(tileLocation, true);
        float num = 1f;
        if (Game1.IsMultiplayer)
        {
          Random multiplayerRandom = Game1.recentMultiplayerRandom;
        }
        else
        {
          Random random = new Random((int) ((double) Game1.uniqueIDForThisGame + (double) tileLocation.X * 7.0 + (double) tileLocation.Y * 11.0 + (double) Game1.stats.DaysPlayed + (double) this.health));
        }
        if (explosion > 0)
        {
          num = (float) explosion;
        }
        else
        {
          switch (t.upgradeLevel)
          {
            case 0:
              num = 2f;
              break;
            case 1:
              num = 2.5f;
              break;
            case 2:
              num = 3.34f;
              break;
            case 3:
              num = 5f;
              break;
            case 4:
              num = 10f;
              break;
          }
        }
        this.health = this.health - num;
        if ((double) this.health <= 0.0)
        {
          Game1.createDebris(12, (int) tileLocation.X, (int) tileLocation.Y, 4, (GameLocation) null);
          Game1.createRadialDebris(location, 12, (int) tileLocation.X, (int) tileLocation.Y, Game1.random.Next(20, 30), false, -1, false, -1);
          return true;
        }
      }
      else if (this.growthStage >= 1)
      {
        if (explosion > 0)
          return true;
        if (location.Equals((object) Game1.currentLocation))
          Game1.playSound("cut");
        if (t != null && t.name.Contains("Axe"))
        {
          Game1.playSound("axchop");
          Game1.createRadialDebris(location, 12, (int) tileLocation.X, (int) tileLocation.Y, Game1.random.Next(10, 20), false, -1, false, -1);
        }
        if (t is Axe || t is Pickaxe || (t is Hoe || t is MeleeWeapon))
        {
          Game1.createRadialDebris(location, 12, (int) tileLocation.X, (int) tileLocation.Y, Game1.random.Next(10, 20), false, -1, false, -1);
          if (t.name.Contains("Axe") && Game1.recentMultiplayerRandom.NextDouble() < (double) t.getLastFarmerToUse().ForagingLevel / 10.0)
            Game1.createDebris(12, (int) tileLocation.X, (int) tileLocation.Y, 1, (GameLocation) null);
          location.temporarySprites.Add(new TemporaryAnimatedSprite(17, tileLocation * (float) Game1.tileSize, Color.White, 8, false, 100f, 0, -1, -1f, -1, 0));
          return true;
        }
      }
      else
      {
        if (explosion > 0)
          return true;
        if (t.name.Contains("Axe") || t.name.Contains("Pick") || t.name.Contains("Hoe"))
        {
          Game1.playSound("woodyHit");
          Game1.playSound("axchop");
          location.temporarySprites.Add(new TemporaryAnimatedSprite(17, tileLocation * (float) Game1.tileSize, Color.White, 8, false, 100f, 0, -1, -1f, -1, 0));
          if (this.lastPlayerToHit != 0L && Game1.getFarmer(this.lastPlayerToHit).getEffectiveSkillLevel(2) >= 1)
            Game1.createMultipleObjectDebris(308 + this.treeType, (int) tileLocation.X, (int) tileLocation.Y, 1, t.getLastFarmerToUse().uniqueMultiplayerID, location);
          else if (!Game1.IsMultiplayer && Game1.player.getEffectiveSkillLevel(2) >= 1)
            Game1.createMultipleObjectDebris(308 + this.treeType, (int) tileLocation.X, (int) tileLocation.Y, 1, t.getLastFarmerToUse().uniqueMultiplayerID, location);
          return true;
        }
      }
      return false;
    }

    public override void drawInMenu(SpriteBatch spriteBatch, Vector2 positionOnScreen, Vector2 tileLocation, float scale, float layerDepth)
    {
      layerDepth += positionOnScreen.X / 100000f;
      if (this.growthStage < 5)
      {
        Microsoft.Xna.Framework.Rectangle rectangle = Microsoft.Xna.Framework.Rectangle.Empty;
        switch (this.growthStage)
        {
          case 0:
            rectangle = new Microsoft.Xna.Framework.Rectangle(32, 128, 16, 16);
            break;
          case 1:
            rectangle = new Microsoft.Xna.Framework.Rectangle(0, 128, 16, 16);
            break;
          case 2:
            rectangle = new Microsoft.Xna.Framework.Rectangle(16, 128, 16, 16);
            break;
          default:
            rectangle = new Microsoft.Xna.Framework.Rectangle(0, 96, 16, 32);
            break;
        }
        spriteBatch.Draw(this.texture, positionOnScreen - new Vector2(0.0f, (float) rectangle.Height * scale), new Microsoft.Xna.Framework.Rectangle?(rectangle), Color.White, 0.0f, Vector2.Zero, scale, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth + (float) (((double) positionOnScreen.Y + (double) rectangle.Height * (double) scale) / 20000.0));
      }
      else
      {
        if (!this.falling)
          spriteBatch.Draw(this.texture, positionOnScreen + new Vector2(0.0f, (float) -Game1.tileSize * scale), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(32, 96, 16, 32)), Color.White, 0.0f, Vector2.Zero, scale, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth + (float) (((double) positionOnScreen.Y + (double) (7 * Game1.tileSize) * (double) scale - 1.0) / 20000.0));
        if (this.stump && !this.falling)
          return;
        spriteBatch.Draw(this.texture, positionOnScreen + new Vector2((float) -Game1.tileSize * scale, (float) (-5 * Game1.tileSize) * scale), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, 48, 96)), Color.White, this.shakeRotation, Vector2.Zero, scale, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth + (float) (((double) positionOnScreen.Y + (double) (7 * Game1.tileSize) * (double) scale) / 20000.0));
      }
    }

    public override void draw(SpriteBatch spriteBatch, Vector2 tileLocation)
    {
      if (this.growthStage < 5)
      {
        Microsoft.Xna.Framework.Rectangle rectangle = Microsoft.Xna.Framework.Rectangle.Empty;
        switch (this.growthStage)
        {
          case 0:
            rectangle = new Microsoft.Xna.Framework.Rectangle(32, 128, 16, 16);
            break;
          case 1:
            rectangle = new Microsoft.Xna.Framework.Rectangle(0, 128, 16, 16);
            break;
          case 2:
            rectangle = new Microsoft.Xna.Framework.Rectangle(16, 128, 16, 16);
            break;
          default:
            rectangle = new Microsoft.Xna.Framework.Rectangle(0, 96, 16, 32);
            break;
        }
        spriteBatch.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float) Game1.tileSize + (float) (Game1.tileSize / 2), (float) ((double) tileLocation.Y * (double) Game1.tileSize - (double) (rectangle.Height * Game1.pixelZoom - Game1.tileSize) + (this.growthStage >= 3 ? (double) (Game1.tileSize * 2) : (double) Game1.tileSize)))), new Microsoft.Xna.Framework.Rectangle?(rectangle), Color.White, this.shakeRotation, new Vector2(8f, this.growthStage >= 3 ? 32f : 16f), (float) Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, this.growthStage == 0 ? 0.0001f : (float) this.getBoundingBox(tileLocation).Bottom / 10000f);
      }
      else
      {
        if (!this.stump || this.falling)
        {
          spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float) Game1.tileSize - (float) (Game1.tileSize * 4 / 5), tileLocation.Y * (float) Game1.tileSize - (float) (Game1.tileSize / 4))), new Microsoft.Xna.Framework.Rectangle?(Tree.shadowSourceRect), Color.White * (1.570796f - Math.Abs(this.shakeRotation)), 0.0f, Vector2.Zero, (float) Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 1E-06f);
          spriteBatch.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float) Game1.tileSize + (float) (Game1.tileSize / 2), tileLocation.Y * (float) Game1.tileSize + (float) Game1.tileSize)), new Microsoft.Xna.Framework.Rectangle?(Tree.treeTopSourceRect), Color.White * this.alpha, this.shakeRotation, new Vector2(24f, 96f), (float) Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (float) ((double) (this.getBoundingBox(tileLocation).Bottom + 2) / 10000.0 - (double) tileLocation.X / 1000000.0));
        }
        if ((double) this.health >= 1.0 || !this.falling && (double) this.health > -99.0)
          spriteBatch.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) ((double) tileLocation.X * (double) Game1.tileSize + ((double) this.shakeTimer > 0.0 ? Math.Sin(2.0 * Math.PI / (double) this.shakeTimer) * 3.0 : 0.0)), tileLocation.Y * (float) Game1.tileSize - (float) Game1.tileSize)), new Microsoft.Xna.Framework.Rectangle?(Tree.stumpSourceRect), Color.White * this.alpha, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (float) this.getBoundingBox(tileLocation).Bottom / 10000f);
        if (this.stump && (double) this.health < 4.0 && (double) this.health > -99.0)
          spriteBatch.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) ((double) tileLocation.X * (double) Game1.tileSize + ((double) this.shakeTimer > 0.0 ? Math.Sin(2.0 * Math.PI / (double) this.shakeTimer) * 3.0 : 0.0)), tileLocation.Y * (float) Game1.tileSize)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(Math.Min(2, (int) (3.0 - (double) this.health)) * 16, 144, 16, 16)), Color.White * this.alpha, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (float) (this.getBoundingBox(tileLocation).Bottom + 1) / 10000f);
      }
      foreach (Leaf leaf in this.leaves)
        spriteBatch.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, leaf.position), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(16 + leaf.type % 2 * 8, 112 + leaf.type / 2 * 8, 8, 8)), Color.White, leaf.rotation, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, (float) ((double) this.getBoundingBox(tileLocation).Bottom / 10000.0 + 0.00999999977648258));
    }
  }
}
