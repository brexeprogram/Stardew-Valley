// Decompiled with JetBrains decompiler
// Type: StardewValley.TerrainFeatures.FruitTree
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.TerrainFeatures
{
  public class FruitTree : TerrainFeature
  {
    public int treeType = -1;
    private float alpha = 1f;
    private List<Leaf> leaves = new List<Leaf>();
    public const float shakeRate = 0.01570796f;
    public const float shakeDecayRate = 0.003067962f;
    public const int minWoodDebrisForFallenTree = 12;
    public const int minWoodDebrisForStump = 5;
    public const int startingHealth = 10;
    public const int leafFallRate = 3;
    public const int DaysUntilMaturity = 28;
    public const int maxFruitsOnTrees = 3;
    public const int seedStage = 0;
    public const int sproutStage = 1;
    public const int saplingStage = 2;
    public const int bushStage = 3;
    public const int treeStage = 4;
    public static Texture2D texture;
    public int growthStage;
    public int indexOfFruit;
    public int daysUntilMature;
    public int fruitsOnTree;
    public int struckByLightningCountdown;
    public float health;
    public bool flipped;
    public bool stump;
    public bool greenHouseTree;
    public bool greenHouseTileTree;
    private bool shakeLeft;
    private bool falling;
    private bool destroy;
    private float shakeRotation;
    private float maxShake;
    private long lastPlayerToHit;
    public string fruitSeason;
    private float shakeTimer;

    public FruitTree()
    {
    }

    public FruitTree(int saplingIndex, int growthStage)
    {
      this.growthStage = growthStage;
      this.loadSprite();
      this.flipped = Game1.random.NextDouble() < 0.5;
      this.health = 10f;
      this.loadData(saplingIndex);
      this.daysUntilMature = 28;
    }

    public FruitTree(int saplingIndex)
    {
      this.loadSprite();
      this.flipped = Game1.random.NextDouble() < 0.5;
      this.health = 10f;
      this.loadData(saplingIndex);
      this.daysUntilMature = 28;
    }

    private void loadData(int saplingIndex)
    {
      Dictionary<int, string> dictionary = Game1.content.Load<Dictionary<int, string>>("Data\\fruitTrees");
      if (!dictionary.ContainsKey(saplingIndex))
        return;
      string[] strArray = dictionary[saplingIndex].Split('/');
      this.treeType = Convert.ToInt32(strArray[0]);
      this.fruitSeason = strArray[1];
      this.indexOfFruit = Convert.ToInt32(strArray[2]);
    }

    public override void loadSprite()
    {
      try
      {
        if (FruitTree.texture != null)
          return;
        FruitTree.texture = Game1.content.Load<Texture2D>("TileSheets\\fruitTrees");
      }
      catch (Exception ex)
      {
      }
    }

    public override bool isActionable()
    {
      return true;
    }

    public override Rectangle getBoundingBox(Vector2 tileLocation)
    {
      switch (this.growthStage)
      {
        case 0:
        case 1:
        case 2:
          return new Rectangle((int) tileLocation.X * Game1.tileSize + Game1.tileSize / 5, (int) tileLocation.Y * Game1.tileSize + Game1.tileSize / 4, Game1.tileSize * 3 / 5, Game1.tileSize * 3 / 4);
        default:
          return new Rectangle((int) tileLocation.X * Game1.tileSize, (int) tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
      }
    }

    public override bool performUseAction(Vector2 tileLocation)
    {
      if ((double) this.maxShake == 0.0 && !this.stump && this.growthStage >= 3 && (!Game1.currentSeason.Equals("winter") || Game1.currentLocation.name.ToLower().Contains("greenhouse")))
        Game1.playSound("leafrustle");
      this.shake(tileLocation, false);
      return true;
    }

    public override bool tickUpdate(GameTime time, Vector2 tileLocation)
    {
      if (this.destroy)
        return true;
      this.alpha = Math.Min(1f, this.alpha + 0.05f);
      if ((double) this.shakeTimer > 0.0)
        this.shakeTimer = this.shakeTimer - (float) time.ElapsedGameTime.Milliseconds;
      if (this.growthStage >= 4 && !this.falling && !this.stump && Game1.player.GetBoundingBox().Intersects(new Rectangle(Game1.tileSize * ((int) tileLocation.X - 1), Game1.tileSize * ((int) tileLocation.Y - 4), 3 * Game1.tileSize, 3 * Game1.tileSize + Game1.tileSize / 2)))
        this.alpha = Math.Max(0.4f, this.alpha - 0.09f);
      if (!this.falling)
      {
        if ((double) Math.Abs(this.shakeRotation) > Math.PI / 2.0 && this.leaves.Count <= 0 && (double) this.health <= 0.0)
          return true;
        if ((double) this.maxShake > 0.0)
        {
          if (this.shakeLeft)
          {
            this.shakeRotation = this.shakeRotation - (this.growthStage >= 4 ? (float) Math.PI / 600f : (float) Math.PI / 200f);
            if ((double) this.shakeRotation <= -(double) this.maxShake)
              this.shakeLeft = false;
          }
          else
          {
            this.shakeRotation = this.shakeRotation + (this.growthStage >= 4 ? (float) Math.PI / 600f : (float) Math.PI / 200f);
            if ((double) this.shakeRotation >= (double) this.maxShake)
              this.shakeLeft = true;
          }
        }
        if ((double) this.maxShake > 0.0)
          this.maxShake = Math.Max(0.0f, this.maxShake - (this.growthStage >= 4 ? 0.001022654f : 0.003067962f));
        if (this.struckByLightningCountdown > 0 && Game1.random.NextDouble() < 0.01)
        {
          List<TemporaryAnimatedSprite> temporarySprites = Game1.currentLocation.temporarySprites;
          TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(372, 1956, 10, 10), new Vector2(tileLocation.X * (float) Game1.tileSize + (float) Game1.random.Next(-Game1.tileSize, Game1.tileSize * 3 / 2), tileLocation.Y * (float) Game1.tileSize - (float) (Game1.tileSize * 3) + (float) Game1.random.Next(-Game1.tileSize, Game1.tileSize * 2)), false, 1f / 500f, Color.Gray);
          temporaryAnimatedSprite.alpha = 0.75f;
          Vector2 vector2 = new Vector2(0.0f, -0.5f);
          temporaryAnimatedSprite.motion = vector2;
          double num1 = 99999.0;
          temporaryAnimatedSprite.interval = (float) num1;
          double num2 = 1.0;
          temporaryAnimatedSprite.layerDepth = (float) num2;
          double num3 = (double) (Game1.pixelZoom / 2);
          temporaryAnimatedSprite.scale = (float) num3;
          double num4 = 0.00999999977648258;
          temporaryAnimatedSprite.scaleChange = (float) num4;
          temporarySprites.Add(temporaryAnimatedSprite);
        }
      }
      else
      {
        this.shakeRotation = this.shakeRotation + (this.shakeLeft ? (float) -((double) this.maxShake * (double) this.maxShake) : this.maxShake * this.maxShake);
        this.maxShake = this.maxShake + 0.001533981f;
        if (Game1.random.NextDouble() < 0.01 && !Game1.currentSeason.Equals("winter"))
          Game1.playSound("leafrustle");
        if ((double) Math.Abs(this.shakeRotation) > Math.PI / 2.0)
        {
          this.falling = false;
          this.maxShake = 0.0f;
          Game1.playSound("treethud");
          int num = Game1.random.Next(90, 120);
          for (int index = 0; index < num; ++index)
            this.leaves.Add(new Leaf(new Vector2((float) (Game1.random.Next((int) ((double) tileLocation.X * (double) Game1.tileSize), (int) ((double) tileLocation.X * (double) Game1.tileSize + (double) (Game1.tileSize * 3))) + (this.shakeLeft ? -Game1.tileSize * 5 : Game1.tileSize * 4)), tileLocation.Y * (float) Game1.tileSize - (float) Game1.tileSize), (float) Game1.random.Next(-10, 10) / 100f, Game1.random.Next(4), (float) Game1.random.Next(10, 40) / 10f));
          Game1.createRadialDebris(Game1.currentLocation, 12, (int) tileLocation.X + (this.shakeLeft ? -4 : 4), (int) tileLocation.Y, 12, true, -1, false, -1);
          Game1.createRadialDebris(Game1.currentLocation, 12, (int) tileLocation.X + (this.shakeLeft ? -4 : 4), (int) tileLocation.Y, 12, false, -1, false, -1);
          if (Game1.IsMultiplayer)
          {
            Game1.recentMultiplayerRandom = new Random((int) tileLocation.X * 1000 + (int) tileLocation.Y);
            Random multiplayerRandom = Game1.recentMultiplayerRandom;
          }
          else
          {
            Random random = new Random((int) Game1.uniqueIDForThisGame + (int) Game1.stats.DaysPlayed + (int) tileLocation.X * 7 + (int) tileLocation.Y * 11);
          }
          if (Game1.IsMultiplayer)
            Game1.createMultipleObjectDebris(92, (int) tileLocation.X + (this.shakeLeft ? -4 : 4), (int) tileLocation.Y, 10, this.lastPlayerToHit);
          else
            Game1.createMultipleObjectDebris(92, (int) tileLocation.X + (this.shakeLeft ? -4 : 4), (int) tileLocation.Y, 10);
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

    public void shake(Vector2 tileLocation, bool doEvenIfStillShaking)
    {
      if ((double) this.maxShake == 0.0 | doEvenIfStillShaking && this.growthStage >= 3 && !this.stump)
      {
        this.shakeLeft = (double) Game1.player.getTileLocation().X > (double) tileLocation.X || (double) Game1.player.getTileLocation().X == (double) tileLocation.X && Game1.random.NextDouble() < 0.5;
        this.maxShake = this.growthStage >= 4 ? (float) Math.PI / 128f : (float) Math.PI / 64f;
        if (this.growthStage >= 4)
        {
          if (Game1.random.NextDouble() < 0.66)
          {
            int num = Game1.random.Next(1, 6);
            for (int index = 0; index < num; ++index)
              this.leaves.Add(new Leaf(new Vector2((float) Game1.random.Next((int) ((double) tileLocation.X * (double) Game1.tileSize - (double) Game1.tileSize), (int) ((double) tileLocation.X * (double) Game1.tileSize + (double) (Game1.tileSize * 2))), (float) Game1.random.Next((int) ((double) tileLocation.Y * (double) Game1.tileSize - (double) (Game1.tileSize * 4)), (int) ((double) tileLocation.Y * (double) Game1.tileSize - (double) (Game1.tileSize * 3)))), (float) Game1.random.Next(-10, 10) / 100f, Game1.random.Next(4), (float) Game1.random.Next(5) / 10f));
          }
          int num1 = 0;
          if (this.daysUntilMature <= -112)
            num1 = 1;
          if (this.daysUntilMature <= -224)
            num1 = 2;
          if (this.daysUntilMature <= -336)
            num1 = 4;
          if (this.struckByLightningCountdown > 0)
            num1 = 0;
          if (!Game1.currentLocation.terrainFeatures.ContainsKey(tileLocation) || !Game1.currentLocation.terrainFeatures[tileLocation].Equals((object) this))
            return;
          for (int index = 0; index < this.fruitsOnTree; ++index)
          {
            Vector2 vector2 = new Vector2(0.0f, 0.0f);
            switch (index)
            {
              case 0:
                vector2.X = (float) -Game1.tileSize;
                break;
              case 1:
                vector2.X = (float) Game1.tileSize;
                vector2.Y = (float) (-Game1.tileSize / 2);
                break;
              case 2:
                vector2.Y = (float) (Game1.tileSize / 2);
                break;
            }
            Debris debris = new Debris(this.struckByLightningCountdown > 0 ? 382 : this.indexOfFruit, new Vector2(tileLocation.X * (float) Game1.tileSize + (float) (Game1.tileSize / 2), (tileLocation.Y - 3f) * (float) Game1.tileSize + (float) (Game1.tileSize / 2)) + vector2, new Vector2((float) Game1.player.getStandingX(), (float) Game1.player.getStandingY()))
            {
              itemQuality = num1
            };
            debris.Chunks[0].xVelocity += (float) Game1.random.Next(-10, 11) / 10f;
            debris.chunkFinalYLevel = (int) ((double) tileLocation.Y * (double) Game1.tileSize + (double) Game1.tileSize);
            Game1.currentLocation.debris.Add(debris);
          }
          this.fruitsOnTree = 0;
        }
        else
        {
          if (Game1.random.NextDouble() >= 0.66)
            return;
          int num = Game1.random.Next(1, 3);
          for (int index = 0; index < num; ++index)
            this.leaves.Add(new Leaf(new Vector2((float) Game1.random.Next((int) ((double) tileLocation.X * (double) Game1.tileSize), (int) ((double) tileLocation.X * (double) Game1.tileSize + (double) (Game1.tileSize * 3 / 4))), tileLocation.Y * (float) Game1.tileSize - (float) (Game1.tileSize * 3 / 2)), (float) Game1.random.Next(-10, 10) / 100f, Game1.random.Next(4), (float) Game1.random.Next(30) / 10f));
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
      return (double) this.health <= -99.0;
    }

    public override void dayUpdate(GameLocation environment, Vector2 tileLocation)
    {
      if ((double) this.health <= -99.0)
        this.destroy = true;
      if (this.struckByLightningCountdown > 0)
      {
        this.struckByLightningCountdown = this.struckByLightningCountdown - 1;
        if (this.struckByLightningCountdown <= 0)
          this.fruitsOnTree = 0;
      }
      bool flag1 = false;
      foreach (Vector2 surroundingTileLocations in Utility.getSurroundingTileLocationsArray(tileLocation))
      {
        bool flag2 = environment.terrainFeatures.ContainsKey(surroundingTileLocations) && environment.terrainFeatures[surroundingTileLocations] is HoeDirt && (environment.terrainFeatures[surroundingTileLocations] as HoeDirt).crop == null;
        if (environment.isTileOccupied(surroundingTileLocations, "") && !flag2)
        {
          flag1 = true;
          break;
        }
      }
      if (!flag1)
      {
        if (this.daysUntilMature > 28)
          this.daysUntilMature = 28;
        this.daysUntilMature = this.daysUntilMature - 1;
        this.growthStage = this.daysUntilMature > 0 ? (this.daysUntilMature > 7 ? (this.daysUntilMature > 14 ? (this.daysUntilMature > 21 ? 0 : 1) : 2) : 3) : 4;
      }
      if (!this.stump && this.growthStage == 4 && (this.struckByLightningCountdown > 0 && !Game1.IsWinter || (Game1.currentSeason.Equals(this.fruitSeason) || environment.name.ToLower().Contains("greenhouse"))))
      {
        this.fruitsOnTree = Math.Min(3, this.fruitsOnTree + 1);
        if (environment.name.ToLower().Contains("greenhouse"))
          this.greenHouseTree = true;
      }
      if (!this.stump)
        return;
      this.fruitsOnTree = 0;
    }

    public override bool seasonUpdate(bool onLoad)
    {
      if (!Game1.currentSeason.Equals(this.fruitSeason) && !onLoad && !this.greenHouseTree)
        this.fruitsOnTree = 0;
      return false;
    }

    public override bool performToolAction(Tool t, int explosion, Vector2 tileLocation, GameLocation location = null)
    {
      Console.WriteLine("FRUIT TREE: IsClient:" + Game1.IsClient.ToString() + " randomOutput: " + (object) Game1.recentMultiplayerRandom.Next(9999));
      if ((double) this.health <= -99.0 || t != null && t is MeleeWeapon)
        return false;
      if (this.growthStage >= 4)
      {
        if (t != null && t is Axe)
        {
          Game1.playSound("axchop");
          Game1.currentLocation.debris.Add(new Debris(12, Game1.random.Next(t.upgradeLevel * 2, t.upgradeLevel * 4), t.getLastFarmerToUse().GetToolLocation(false) + new Vector2((float) (Game1.tileSize / 4), 0.0f), t.getLastFarmerToUse().position, 0, -1));
          this.lastPlayerToHit = t.getLastFarmerToUse().uniqueMultiplayerID;
          int num = 0;
          if (this.daysUntilMature <= -112)
            num = 1;
          if (this.daysUntilMature <= -224)
            num = 2;
          if (this.daysUntilMature <= -336)
            num = 4;
          if (this.struckByLightningCountdown > 0)
            num = 0;
          if (Game1.currentLocation.terrainFeatures.ContainsKey(tileLocation) && Game1.currentLocation.terrainFeatures[tileLocation].Equals((object) this))
          {
            for (int index = 0; index < this.fruitsOnTree; ++index)
            {
              Vector2 vector2 = new Vector2(0.0f, 0.0f);
              switch (index)
              {
                case 0:
                  vector2.X = (float) -Game1.tileSize;
                  break;
                case 1:
                  vector2.X = (float) Game1.tileSize;
                  vector2.Y = (float) (-Game1.tileSize / 2);
                  break;
                case 2:
                  vector2.Y = (float) (Game1.tileSize / 2);
                  break;
              }
              Debris debris = new Debris(this.struckByLightningCountdown > 0 ? 382 : this.indexOfFruit, new Vector2(tileLocation.X * (float) Game1.tileSize + (float) (Game1.tileSize / 2), (tileLocation.Y - 3f) * (float) Game1.tileSize + (float) (Game1.tileSize / 2)) + vector2, new Vector2((float) Game1.player.getStandingX(), (float) Game1.player.getStandingY()))
              {
                itemQuality = num
              };
              debris.Chunks[0].xVelocity += (float) Game1.random.Next(-10, 11) / 10f;
              debris.chunkFinalYLevel = (int) ((double) tileLocation.Y * (double) Game1.tileSize + (double) Game1.tileSize);
              Game1.currentLocation.debris.Add(debris);
            }
            this.fruitsOnTree = 0;
          }
        }
        else if (explosion <= 0)
          return false;
        this.shake(tileLocation, true);
        float num1 = 1f;
        if (explosion > 0)
        {
          num1 = (float) explosion;
        }
        else
        {
          if (t == null)
            return false;
          switch (t.upgradeLevel)
          {
            case 0:
              num1 = 1f;
              break;
            case 1:
              num1 = 1.25f;
              break;
            case 2:
              num1 = 1.67f;
              break;
            case 3:
              num1 = 2.5f;
              break;
            case 4:
              num1 = 5f;
              break;
          }
        }
        this.health = this.health - num1;
        if ((double) this.health <= 0.0)
        {
          if (!this.stump)
          {
            Game1.playSound("treecrack");
            this.stump = true;
            this.health = 5f;
            this.falling = true;
            this.shakeLeft = t == null || t.getLastFarmerToUse() == null || ((double) t.getLastFarmerToUse().getTileLocation().X > (double) tileLocation.X || (double) t.getLastFarmerToUse().getTileLocation().Y < (double) tileLocation.Y && (double) tileLocation.X % 2.0 == 0.0);
          }
          else
          {
            this.health = -100f;
            Game1.createRadialDebris(Game1.currentLocation, 12, (int) tileLocation.X, (int) tileLocation.Y, Game1.random.Next(30, 40), false, -1, false, -1);
            int index = 92;
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
              Game1.createMultipleObjectDebris(92, (int) tileLocation.X, (int) tileLocation.Y, 2);
            else if (Game1.IsMultiplayer)
            {
              Game1.createMultipleObjectDebris(index, (int) tileLocation.X, (int) tileLocation.Y, 1, this.lastPlayerToHit);
              Game1.createRadialDebris(Game1.currentLocation, 12, (int) tileLocation.X, (int) tileLocation.Y, 4, true, -1, false, -1);
            }
            else
            {
              Game1.createRadialDebris(Game1.currentLocation, 12, (int) tileLocation.X, (int) tileLocation.Y, 5, true, -1, false, -1);
              Game1.createMultipleObjectDebris(index, (int) tileLocation.X, (int) tileLocation.Y, 1);
            }
          }
        }
      }
      else if (this.growthStage >= 3)
      {
        if (t != null && t.name.Contains("Ax"))
        {
          Game1.playSound("axchop");
          Game1.playSound("leafrustle");
          Game1.currentLocation.debris.Add(new Debris(12, Game1.random.Next(t.upgradeLevel * 2, t.upgradeLevel * 4), t.getLastFarmerToUse().GetToolLocation(false) + new Vector2((float) (Game1.tileSize / 4), 0.0f), new Vector2((float) t.getLastFarmerToUse().GetBoundingBox().Center.X, (float) t.getLastFarmerToUse().GetBoundingBox().Center.Y), 0, -1));
        }
        else if (explosion <= 0)
          return false;
        this.shake(tileLocation, true);
        float num = 1f;
        Random random = !Game1.IsMultiplayer ? new Random((int) ((double) Game1.uniqueIDForThisGame + (double) tileLocation.X * 7.0 + (double) tileLocation.Y * 11.0 + (double) Game1.stats.DaysPlayed + (double) this.health)) : Game1.recentMultiplayerRandom;
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
        int numberOfChunks = 0;
        while (t != null && random.NextDouble() < (double) num * 0.08 + (double) t.getLastFarmerToUse().ForagingLevel / 200.0)
          ++numberOfChunks;
        this.health = this.health - num;
        if (numberOfChunks > 0)
          Game1.createDebris(12, (int) tileLocation.X, (int) tileLocation.Y, numberOfChunks, (GameLocation) null);
        if ((double) this.health <= 0.0)
        {
          Game1.createRadialDebris(Game1.currentLocation, 12, (int) tileLocation.X, (int) tileLocation.Y, Game1.random.Next(20, 30), false, -1, false, -1);
          return true;
        }
      }
      else if (this.growthStage >= 1)
      {
        if (explosion > 0)
          return true;
        if (t != null && t.name.Contains("Axe"))
        {
          Game1.playSound("axchop");
          Game1.createRadialDebris(Game1.currentLocation, 12, (int) tileLocation.X, (int) tileLocation.Y, Game1.random.Next(10, 20), false, -1, false, -1);
        }
        if (t is Axe || t is Pickaxe || (t is Hoe || t is MeleeWeapon))
        {
          Game1.createRadialDebris(Game1.currentLocation, 12, (int) tileLocation.X, (int) tileLocation.Y, Game1.random.Next(10, 20), false, -1, false, -1);
          if (t.name.Contains("Axe") && Game1.recentMultiplayerRandom.NextDouble() < (double) t.getLastFarmerToUse().ForagingLevel / 10.0)
            Game1.createDebris(12, (int) tileLocation.X, (int) tileLocation.Y, 1, (GameLocation) null);
          Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(17, tileLocation * (float) Game1.tileSize, Color.White, 8, false, 100f, 0, -1, -1f, -1, 0));
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
          Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(17, tileLocation * (float) Game1.tileSize, Color.White, 8, false, 100f, 0, -1, -1f, -1, 0));
          return true;
        }
      }
      return false;
    }

    public override void drawInMenu(SpriteBatch spriteBatch, Vector2 positionOnScreen, Vector2 tileLocation, float scale, float layerDepth)
    {
      layerDepth += positionOnScreen.X / 100000f;
      if (this.growthStage < 4)
      {
        Rectangle rectangle = Rectangle.Empty;
        switch (this.growthStage)
        {
          case 0:
            rectangle = new Rectangle(2 * Game1.tileSize, 8 * Game1.tileSize, Game1.tileSize, Game1.tileSize);
            break;
          case 1:
            rectangle = new Rectangle(0, 8 * Game1.tileSize, Game1.tileSize, Game1.tileSize);
            break;
          case 2:
            rectangle = new Rectangle(Game1.tileSize, 8 * Game1.tileSize, Game1.tileSize, Game1.tileSize);
            break;
          default:
            rectangle = new Rectangle(0, 6 * Game1.tileSize, Game1.tileSize, Game1.tileSize * 2);
            break;
        }
        spriteBatch.Draw(FruitTree.texture, positionOnScreen - new Vector2(0.0f, (float) rectangle.Height * scale), new Rectangle?(rectangle), Color.White, 0.0f, Vector2.Zero, scale, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth + (float) (((double) positionOnScreen.Y + (double) rectangle.Height * (double) scale) / 20000.0));
      }
      else
      {
        if (!this.falling)
          spriteBatch.Draw(FruitTree.texture, positionOnScreen + new Vector2(0.0f, (float) -Game1.tileSize * scale), new Rectangle?(new Rectangle(2 * Game1.tileSize, 6 * Game1.tileSize, Game1.tileSize, Game1.tileSize * 2)), Color.White, 0.0f, Vector2.Zero, scale, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth + (float) (((double) positionOnScreen.Y + (double) (7 * Game1.tileSize) * (double) scale - 1.0) / 20000.0));
        if (this.stump && !this.falling)
          return;
        spriteBatch.Draw(FruitTree.texture, positionOnScreen + new Vector2((float) -Game1.tileSize * scale, (float) (-5 * Game1.tileSize) * scale), new Rectangle?(new Rectangle(0, 0, 3 * Game1.tileSize, 6 * Game1.tileSize)), Color.White, this.shakeRotation, Vector2.Zero, scale, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth + (float) (((double) positionOnScreen.Y + (double) (7 * Game1.tileSize) * (double) scale) / 20000.0));
      }
    }

    public override void draw(SpriteBatch spriteBatch, Vector2 tileLocation)
    {
      if (this.greenHouseTileTree)
        spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float) Game1.tileSize, tileLocation.Y * (float) Game1.tileSize)), new Rectangle?(new Rectangle(669, 1957, 16, 16)), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 1E-08f);
      Rectangle boundingBox;
      if (this.growthStage < 4)
      {
        Vector2 vector2 = new Vector2((float) Math.Max(-8.0, Math.Min((double) Game1.tileSize, Math.Sin((double) tileLocation.X * 200.0 / (2.0 * Math.PI)) * -16.0)), (float) Math.Max(-8.0, Math.Min((double) Game1.tileSize, Math.Sin((double) tileLocation.X * 200.0 / (2.0 * Math.PI)) * -16.0)));
        Rectangle rectangle = Rectangle.Empty;
        switch (this.growthStage)
        {
          case 0:
            rectangle = new Rectangle(0, this.treeType * 5 * 16, 48, 80);
            break;
          case 1:
            rectangle = new Rectangle(48, this.treeType * 5 * 16, 48, 80);
            break;
          case 2:
            rectangle = new Rectangle(96, this.treeType * 5 * 16, 48, 80);
            break;
          default:
            rectangle = new Rectangle(144, this.treeType * 5 * 16, 48, 80);
            break;
        }
        SpriteBatch spriteBatch1 = spriteBatch;
        Texture2D texture = FruitTree.texture;
        Vector2 local = Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float) Game1.tileSize + (float) (Game1.tileSize / 2) + vector2.X, tileLocation.Y * (float) Game1.tileSize - (float) rectangle.Height + (float) (Game1.tileSize * 2) + vector2.Y));
        Rectangle? sourceRectangle = new Rectangle?(rectangle);
        Color white = Color.White;
        double shakeRotation = (double) this.shakeRotation;
        Vector2 origin = new Vector2(24f, 80f);
        double num1 = 4.0;
        int num2 = this.flipped ? 1 : 0;
        boundingBox = this.getBoundingBox(tileLocation);
        double num3 = (double) boundingBox.Bottom / 10000.0 - (double) tileLocation.X / 1000000.0;
        spriteBatch1.Draw(texture, local, sourceRectangle, white, (float) shakeRotation, origin, (float) num1, (SpriteEffects) num2, (float) num3);
      }
      else
      {
        if (!this.stump || this.falling)
        {
          if (!this.falling)
            spriteBatch.Draw(FruitTree.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float) Game1.tileSize + (float) (Game1.tileSize / 2), tileLocation.Y * (float) Game1.tileSize + (float) Game1.tileSize)), new Rectangle?(new Rectangle((12 + (this.greenHouseTree ? 1 : Utility.getSeasonNumber(Game1.currentSeason)) * 3) * 16, this.treeType * 5 * 16 + 64, 48, 16)), this.struckByLightningCountdown > 0 ? Color.Gray * this.alpha : Color.White * this.alpha, 0.0f, new Vector2(24f, 16f), 4f, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 1E-07f);
          SpriteBatch spriteBatch1 = spriteBatch;
          Texture2D texture = FruitTree.texture;
          Vector2 local = Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float) Game1.tileSize + (float) (Game1.tileSize / 2), tileLocation.Y * (float) Game1.tileSize + (float) Game1.tileSize));
          Rectangle? sourceRectangle = new Rectangle?(new Rectangle((12 + (this.greenHouseTree ? 1 : Utility.getSeasonNumber(Game1.currentSeason)) * 3) * 16, this.treeType * 5 * 16, 48, 64));
          Color color = this.struckByLightningCountdown > 0 ? Color.Gray * this.alpha : Color.White * this.alpha;
          double shakeRotation = (double) this.shakeRotation;
          Vector2 origin = new Vector2(24f, 80f);
          double num1 = 4.0;
          int num2 = this.flipped ? 1 : 0;
          boundingBox = this.getBoundingBox(tileLocation);
          double num3 = (double) boundingBox.Bottom / 10000.0 + 1.0 / 1000.0 - (double) tileLocation.X / 1000000.0;
          spriteBatch1.Draw(texture, local, sourceRectangle, color, (float) shakeRotation, origin, (float) num1, (SpriteEffects) num2, (float) num3);
        }
        if ((double) this.health >= 1.0 || !this.falling && (double) this.health > -99.0)
        {
          SpriteBatch spriteBatch1 = spriteBatch;
          Texture2D texture = FruitTree.texture;
          Vector2 local = Game1.GlobalToLocal(Game1.viewport, new Vector2((float) ((double) tileLocation.X * (double) Game1.tileSize + (double) (Game1.tileSize / 2) + ((double) this.shakeTimer > 0.0 ? Math.Sin(2.0 * Math.PI / (double) this.shakeTimer) * 2.0 : 0.0)), tileLocation.Y * (float) Game1.tileSize + (float) Game1.tileSize));
          Rectangle? sourceRectangle = new Rectangle?(new Rectangle(384, this.treeType * 5 * 16 + 48, 48, 32));
          Color color = this.struckByLightningCountdown > 0 ? Color.Gray * this.alpha : Color.White * this.alpha;
          double num1 = 0.0;
          Vector2 origin = new Vector2(24f, 32f);
          double num2 = 4.0;
          int num3 = this.flipped ? 1 : 0;
          double num4;
          if (!this.stump || this.falling)
          {
            boundingBox = this.getBoundingBox(tileLocation);
            num4 = (double) boundingBox.Bottom / 10000.0 - 1.0 / 1000.0 - (double) tileLocation.X / 1000000.0;
          }
          else
          {
            boundingBox = this.getBoundingBox(tileLocation);
            num4 = (double) boundingBox.Bottom / 10000.0;
          }
          spriteBatch1.Draw(texture, local, sourceRectangle, color, (float) num1, origin, (float) num2, (SpriteEffects) num3, (float) num4);
        }
        for (int index = 0; index < this.fruitsOnTree; ++index)
        {
          switch (index)
          {
            case 0:
              SpriteBatch spriteBatch1 = spriteBatch;
              Texture2D objectSpriteSheet1 = Game1.objectSpriteSheet;
              Vector2 local1 = Game1.GlobalToLocal(Game1.viewport, new Vector2((float) ((double) tileLocation.X * (double) Game1.tileSize - (double) Game1.tileSize + (double) tileLocation.X * 200.0 % (double) Game1.tileSize / 2.0), (float) ((double) tileLocation.Y * (double) Game1.tileSize - (double) (Game1.tileSize * 3) - (double) tileLocation.X % (double) Game1.tileSize / 3.0)));
              Rectangle? sourceRectangle1 = new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.struckByLightningCountdown > 0 ? 382 : this.indexOfFruit, 16, 16));
              Color white1 = Color.White;
              double num1 = 0.0;
              Vector2 zero1 = Vector2.Zero;
              double num2 = (double) Game1.pixelZoom * 1.0;
              int num3 = 0;
              boundingBox = this.getBoundingBox(tileLocation);
              double num4 = (double) boundingBox.Bottom / 10000.0 + 1.0 / 500.0 - (double) tileLocation.X / 1000000.0;
              spriteBatch1.Draw(objectSpriteSheet1, local1, sourceRectangle1, white1, (float) num1, zero1, (float) num2, (SpriteEffects) num3, (float) num4);
              break;
            case 1:
              SpriteBatch spriteBatch2 = spriteBatch;
              Texture2D objectSpriteSheet2 = Game1.objectSpriteSheet;
              Vector2 local2 = Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float) Game1.tileSize + (float) (Game1.tileSize / 2), (float) ((double) tileLocation.Y * (double) Game1.tileSize - (double) (Game1.tileSize * 4) + (double) tileLocation.X * 232.0 % (double) Game1.tileSize / 3.0)));
              Rectangle? sourceRectangle2 = new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.struckByLightningCountdown > 0 ? 382 : this.indexOfFruit, 16, 16));
              Color white2 = Color.White;
              double num5 = 0.0;
              Vector2 zero2 = Vector2.Zero;
              double num6 = (double) Game1.pixelZoom * 1.0;
              int num7 = 0;
              boundingBox = this.getBoundingBox(tileLocation);
              double num8 = (double) boundingBox.Bottom / 10000.0 + 1.0 / 500.0 - (double) tileLocation.X / 1000000.0;
              spriteBatch2.Draw(objectSpriteSheet2, local2, sourceRectangle2, white2, (float) num5, zero2, (float) num6, (SpriteEffects) num7, (float) num8);
              break;
            case 2:
              SpriteBatch spriteBatch3 = spriteBatch;
              Texture2D objectSpriteSheet3 = Game1.objectSpriteSheet;
              Vector2 local3 = Game1.GlobalToLocal(Game1.viewport, new Vector2((float) ((double) tileLocation.X * (double) Game1.tileSize + (double) tileLocation.X * 200.0 % (double) Game1.tileSize / 3.0), (float) ((double) tileLocation.Y * (double) Game1.tileSize - (double) Game1.tileSize * 2.5 + (double) tileLocation.X * 200.0 % (double) Game1.tileSize / 3.0)));
              Rectangle? sourceRectangle3 = new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.struckByLightningCountdown > 0 ? 382 : this.indexOfFruit, 16, 16));
              Color white3 = Color.White;
              double num9 = 0.0;
              Vector2 zero3 = Vector2.Zero;
              double num10 = (double) Game1.pixelZoom * 1.0;
              int num11 = 1;
              boundingBox = this.getBoundingBox(tileLocation);
              double num12 = (double) boundingBox.Bottom / 10000.0 + 1.0 / 500.0 - (double) tileLocation.X / 1000000.0;
              spriteBatch3.Draw(objectSpriteSheet3, local3, sourceRectangle3, white3, (float) num9, zero3, (float) num10, (SpriteEffects) num11, (float) num12);
              break;
          }
        }
      }
      foreach (Leaf leaf in this.leaves)
      {
        SpriteBatch spriteBatch1 = spriteBatch;
        Texture2D texture = FruitTree.texture;
        Vector2 local = Game1.GlobalToLocal(Game1.viewport, leaf.position);
        Rectangle? sourceRectangle = new Rectangle?(new Rectangle((24 + Utility.getSeasonNumber(Game1.currentSeason)) * 16, this.treeType * 5 * 16, Game1.tileSize / 8, Game1.tileSize / 8));
        Color white = Color.White;
        double rotation = (double) leaf.rotation;
        Vector2 zero = Vector2.Zero;
        double num1 = 4.0;
        int num2 = 0;
        boundingBox = this.getBoundingBox(tileLocation);
        double num3 = (double) boundingBox.Bottom / 10000.0 + 0.00999999977648258;
        spriteBatch1.Draw(texture, local, sourceRectangle, white, (float) rotation, zero, (float) num1, (SpriteEffects) num2, (float) num3);
      }
    }
  }
}
