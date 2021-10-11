// Decompiled with JetBrains decompiler
// Type: StardewValley.Locations.MineShaft
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using StardewValley.Menus;
using StardewValley.Monsters;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using xTile;
using xTile.Dimensions;
using xTile.ObjectModel;
using xTile.Tiles;

namespace StardewValley.Locations
{
  public class MineShaft : GameLocation
  {
    public List<ResourceClump> resourceClumps = new List<ResourceClump>();
    private LocalizedContentManager mineLoader = Game1.content.CreateTemporary();
    private int timeSinceLastMusic = 200000;
    private Color lighting = Color.White;
    private Microsoft.Xna.Framework.Rectangle fogSource = new Microsoft.Xna.Framework.Rectangle(640, 0, 64, 64);
    public const int mineFrostLevel = 40;
    public const int mineLavaLevel = 80;
    public const int upperArea = 0;
    public const int jungleArea = 10;
    public const int frostArea = 40;
    public const int lavaArea = 80;
    public const int desertArea = 121;
    public const int bottomOfMineLevel = 120;
    public const int numberOfLevelsPerArea = 40;
    public const int mineFeature_barrels = 0;
    public const int mineFeature_chests = 1;
    public const int mineFeature_coalCart = 2;
    public const int mineFeature_elevator = 3;
    public const double chanceForColoredGemstone = 0.008;
    public const double chanceForDiamond = 0.0005;
    public const double chanceForPrismaticShard = 0.0005;
    public const int monsterLimit = 30;
    public SerializableDictionary<int, MineInfo> permanentMineChanges;
    private Random mineRandom;
    public int mineLevel;
    public int nextLevel;
    public int lowestLevelReached;
    private Vector2 tileBeneathLadder;
    private Vector2 tileBeneathElevator;
    private int stonesLeftOnThisLevel;
    private int timeUntilElevatorLightUp;
    private int fogTime;
    private Point ElevatorLightSpot;
    private bool ladderHasSpawned;
    private bool ghostAdded;
    private bool loadedDarkArea;
    private bool isSlimeArea;
    private bool isMonsterArea;
    private bool isFallingDownShaft;
    private bool ambientFog;
    private Vector2 fogPos;
    private Color fogColor;
    private Point mostRecentLadder;
    private float fogAlpha;
    [XmlIgnore]
    public Cue bugLevelLoop;
    private bool rainbowLights;
    private bool isLightingDark;
    private int lastLevelsDownFallen;

    public MineShaft()
    {
      this.name = "UndergroundMine";
      this.permanentMineChanges = new SerializableDictionary<int, MineInfo>();
    }

    public override void UpdateWhenCurrentLocation(GameTime time)
    {
      if (this.wasUpdated)
        return;
      foreach (ResourceClump resourceClump in this.resourceClumps)
        resourceClump.tickUpdate(time, resourceClump.tile);
      if (Game1.currentSong != null && (!Game1.currentSong.IsPlaying || Game1.currentSong.Name.Contains("Ambient")) && Game1.random.NextDouble() < 0.00195)
        Game1.playSound("cavedrip");
      if (this.timeUntilElevatorLightUp > 0)
      {
        this.timeUntilElevatorLightUp = this.timeUntilElevatorLightUp - time.ElapsedGameTime.Milliseconds;
        if (this.timeUntilElevatorLightUp <= 0)
        {
          Game1.playSound("crystal");
          this.setMapTileIndex(this.ElevatorLightSpot.X, this.ElevatorLightSpot.Y, 48, "Buildings", 0);
          Game1.currentLightSources.Add(new LightSource(4, new Vector2((float) this.ElevatorLightSpot.X, (float) this.ElevatorLightSpot.Y) * (float) Game1.tileSize, 2f, Color.Black, this.ElevatorLightSpot.X + this.ElevatorLightSpot.Y * 1000));
        }
      }
      if (this.fogTime > 0 && Game1.shouldTimePass())
      {
        if (Game1.soundBank != null && (this.bugLevelLoop == null || this.bugLevelLoop.IsStopped))
        {
          this.bugLevelLoop = Game1.soundBank.GetCue("bugLevelLoop");
          this.bugLevelLoop.Play();
        }
        if ((double) this.fogAlpha < 1.0)
        {
          this.fogAlpha = this.fogAlpha + 0.01f;
          if (this.bugLevelLoop != null && Game1.soundBank != null)
          {
            this.bugLevelLoop.SetVariable("Volume", this.fogAlpha * 100f);
            this.bugLevelLoop.SetVariable("Frequency", this.fogAlpha * 25f);
          }
        }
        else if (this.bugLevelLoop != null && Game1.soundBank != null)
          this.bugLevelLoop.SetVariable("Frequency", Math.Max(0.0f, Math.Min(100f, (float) ((double) this.fogAlpha * 25.0 + Math.Max(0.0, Math.Min(100.0, Math.Sin((double) this.fogTime / 10000.0 % (200.0 * Math.PI)))) * 10.0))));
        int fogTime = this.fogTime;
        this.fogTime = this.fogTime - (int) time.ElapsedGameTime.TotalMilliseconds;
        if (this.fogTime > 5000 && fogTime % 4000 < this.fogTime % 4000)
          this.spawnFlyingMonsterOffScreen();
        this.fogPos = Game1.updateFloatingObjectPositionForMovement(this.fogPos, new Vector2((float) Game1.viewport.X, (float) Game1.viewport.Y), Game1.previousViewportPosition, -1f);
        this.fogPos.X = (this.fogPos.X + 0.5f) % (float) (64 * Game1.pixelZoom);
        this.fogPos.Y = (this.fogPos.Y + 0.5f) % (float) (64 * Game1.pixelZoom);
      }
      else if ((double) this.fogAlpha > 0.0)
      {
        this.fogAlpha = this.fogAlpha - 0.01f;
        if (this.bugLevelLoop != null)
        {
          this.bugLevelLoop.SetVariable("Volume", this.fogAlpha * 100f);
          this.bugLevelLoop.SetVariable("Frequency", Math.Max(0.0f, this.bugLevelLoop.GetVariable("Frequency") - 0.01f));
          if ((double) this.fogAlpha <= 0.0)
          {
            this.bugLevelLoop.Stop(AudioStopOptions.Immediate);
            this.bugLevelLoop = (Cue) null;
          }
        }
      }
      else if (this.ambientFog)
      {
        this.fogPos = Game1.updateFloatingObjectPositionForMovement(this.fogPos, new Vector2((float) Game1.viewport.X, (float) Game1.viewport.Y), Game1.previousViewportPosition, -1f);
        this.fogPos.X = (this.fogPos.X + 0.5f) % (float) (64 * Game1.pixelZoom);
        this.fogPos.Y = (this.fogPos.Y + 0.5f) % (float) (64 * Game1.pixelZoom);
      }
      base.UpdateWhenCurrentLocation(time);
    }

    public override void cleanupBeforePlayerExit()
    {
      base.cleanupBeforePlayerExit();
      if (this.bugLevelLoop == null)
        return;
      this.bugLevelLoop.Stop(AudioStopOptions.Immediate);
      this.bugLevelLoop = (Cue) null;
    }

    public void setNextLevel(int level)
    {
      this.nextLevel = level;
    }

    public override int getExtraMillisecondsPerInGameMinuteForThisLocation()
    {
      return this.getMineArea(-1) != 121 ? 0 : 2000;
    }

    public Vector2 enterMine(Farmer who, int mineLevel, bool ridingElevator)
    {
      this.mineRandom = new Random();
      this.ladderHasSpawned = false;
      this.loadLevel(this.nextLevel);
      this.chooseLevelType();
      this.findLadder();
      this.populateLevel();
      if (!who.ridingMineElevator || this.tileBeneathElevator.Equals(Vector2.Zero))
        return this.tileBeneathLadder;
      return this.tileBeneathElevator;
    }

    public void chooseLevelType()
    {
      this.fogTime = 0;
      if (this.bugLevelLoop != null)
      {
        this.bugLevelLoop.Stop(AudioStopOptions.Immediate);
        this.bugLevelLoop = (Cue) null;
      }
      this.ambientFog = false;
      this.rainbowLights = false;
      this.isLightingDark = false;
      Random random = new Random((int) Game1.stats.DaysPlayed + this.mineLevel + (int) Game1.uniqueIDForThisGame / 2);
      this.lighting = new Color(80, 80, 40);
      if (this.getMineArea(-1) == 80)
        this.lighting = new Color(100, 100, 50);
      if (random.NextDouble() < 0.3 && this.mineLevel > 2)
      {
        this.isLightingDark = true;
        this.lighting = new Color(120, 120, 60);
        if (random.NextDouble() < 0.3)
          this.lighting = new Color(150, 150, 120);
      }
      if (random.NextDouble() < 0.15 && this.mineLevel > 5 && this.mineLevel != 120)
      {
        this.isLightingDark = true;
        switch (this.getMineArea(-1))
        {
          case 40:
            this.lighting = Color.Black;
            break;
          case 80:
            this.lighting = new Color(90, 130, 70);
            break;
          case 0:
          case 10:
            this.lighting = new Color(110, 110, 70);
            break;
        }
      }
      if (random.NextDouble() < 0.035 && this.getMineArea(-1) == 80 && this.mineLevel % 5 != 0)
        this.rainbowLights = true;
      if (this.isDarkArea() && this.mineLevel < 120)
      {
        this.isLightingDark = true;
        this.lighting = this.getMineArea(-1) == 80 ? new Color(70, 100, 100) : new Color(150, 150, 120);
        if (this.getMineArea(-1) == 0)
          this.ambientFog = true;
      }
      if (this.mineLevel == 100)
        this.lighting = new Color(140, 140, 80);
      if (this.getMineArea(-1) == 121)
      {
        this.lighting = new Color(110, 110, 40);
        if (random.NextDouble() < 0.05)
          this.lighting = random.NextDouble() < 0.5 ? new Color(30, 30, 0) : new Color(150, 150, 50);
      }
      if (this.mineLevel <= 1 || this.mineLevel != 2 && (this.mineLevel % 5 == 0 || this.timeSinceLastMusic <= 150000 || Game1.random.NextDouble() >= 0.5))
        return;
      this.playMineSong();
    }

    private bool canAdd(int typeOfFeature, int numberSoFar)
    {
      if (this.permanentMineChanges.ContainsKey(this.mineLevel))
      {
        switch (typeOfFeature)
        {
          case 0:
            return this.permanentMineChanges[this.mineLevel].platformContainersLeft > numberSoFar;
          case 1:
            return this.permanentMineChanges[this.mineLevel].chestsLeft > numberSoFar;
          case 2:
            return this.permanentMineChanges[this.mineLevel].coalCartsLeft > numberSoFar;
          case 3:
            return this.permanentMineChanges[this.mineLevel].elevator == 0;
        }
      }
      return true;
    }

    public void updateMineLevelData(int feature, int amount = 1)
    {
      if (!this.permanentMineChanges.ContainsKey(this.mineLevel))
        this.permanentMineChanges.Add(this.mineLevel, new MineInfo());
      switch (feature)
      {
        case 0:
          this.permanentMineChanges[this.mineLevel].platformContainersLeft += amount;
          break;
        case 1:
          this.permanentMineChanges[this.mineLevel].chestsLeft += amount;
          break;
        case 2:
          this.permanentMineChanges[this.mineLevel].coalCartsLeft += amount;
          break;
        case 3:
          this.permanentMineChanges[this.mineLevel].elevator += amount;
          break;
      }
    }

    public bool isLevelSlimeArea()
    {
      return this.isSlimeArea;
    }

    public void checkForMapAlterations(int x, int y)
    {
      Tile tile = this.map.GetLayer("Buildings").Tiles[x, y];
      if (tile == null || tile.TileIndex != 194 || this.canAdd(2, 0))
        return;
      this.setMapTileIndex(x, y, 195, "Buildings", 0);
      this.setMapTileIndex(x, y - 1, 179, "Front", 0);
    }

    public void findLadder()
    {
      int num = 0;
      this.tileBeneathElevator = Vector2.Zero;
      bool flag1 = this.mineLevel % 20 == 0;
      if (flag1)
      {
        this.waterTiles = new bool[this.map.Layers[0].LayerWidth, this.map.Layers[0].LayerHeight];
        this.waterColor = this.getMineArea(-1) == 80 ? Color.Red * 0.8f : new Color(50, 100, 200) * 0.5f;
      }
      bool flag2 = false;
      this.lightGlows.Clear();
      for (int yTile = 0; yTile < this.map.GetLayer("Buildings").LayerHeight; ++yTile)
      {
        for (int xTile = 0; xTile < this.map.GetLayer("Buildings").LayerWidth; ++xTile)
        {
          if (this.map.GetLayer("Buildings").Tiles[xTile, yTile] != null)
          {
            int tileIndex = this.map.GetLayer("Buildings").Tiles[xTile, yTile].TileIndex;
            switch (tileIndex)
            {
              case 115:
                this.tileBeneathLadder = new Vector2((float) xTile, (float) (yTile + 1));
                Game1.currentLightSources.Add(new LightSource(4, new Vector2((float) xTile, (float) (yTile - 2)) * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), 0.0f), 0.25f, new Color(0, 20, 50), xTile + yTile * 999));
                Game1.currentLightSources.Add(new LightSource(4, new Vector2((float) xTile, (float) (yTile - 1)) * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), 0.0f), 0.5f, new Color(0, 20, 50), xTile + yTile * 998));
                Game1.currentLightSources.Add(new LightSource(4, new Vector2((float) xTile, (float) yTile) * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), 0.0f), 0.75f, new Color(0, 20, 50), xTile + yTile * 997));
                Game1.currentLightSources.Add(new LightSource(4, new Vector2((float) xTile, (float) (yTile + 1)) * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), 0.0f), 1f, new Color(0, 20, 50), xTile + yTile * 1000));
                ++num;
                break;
              case 112:
                this.tileBeneathElevator = new Vector2((float) xTile, (float) (yTile + 1));
                ++num;
                break;
            }
            if (this.lighting.Equals(Color.White) && num == 2 && !flag1)
              return;
            if (!this.lighting.Equals(Color.White) && (tileIndex == 97 || tileIndex == 113 || (tileIndex == 65 || tileIndex == 66) || (tileIndex == 81 || tileIndex == 82 || tileIndex == 48)))
            {
              Game1.currentLightSources.Add(new LightSource(4, new Vector2((float) xTile, (float) yTile) * (float) Game1.tileSize, 2.5f, new Color(0, 50, 100), xTile + yTile * 1000));
              if (tileIndex == 66)
                this.lightGlows.Add(new Vector2((float) xTile, (float) yTile) * (float) Game1.tileSize + new Vector2(0.0f, (float) Game1.tileSize));
              else if (tileIndex == 97 || tileIndex == 113)
                this.lightGlows.Add(new Vector2((float) xTile, (float) yTile) * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2)));
            }
          }
          if (this.doesTileHaveProperty(xTile, yTile, "Water", "Back") != null)
          {
            flag2 = true;
            this.waterTiles[xTile, yTile] = true;
            if (this.getMineArea(-1) == 80 && Game1.random.NextDouble() < 0.1)
              Game1.currentLightSources.Add(new LightSource(4, new Vector2((float) xTile, (float) yTile) * (float) Game1.tileSize, 2f, new Color(0, 220, 220), xTile + yTile * 1000));
          }
        }
      }
      if (!flag2)
        this.waterTiles = (bool[,]) null;
      if (this.isFallingDownShaft)
      {
        Vector2 v = new Vector2();
        while (!this.isTileClearForMineObjects(v))
        {
          v.X = (float) Game1.random.Next(1, this.map.Layers[0].LayerWidth);
          v.Y = (float) Game1.random.Next(1, this.map.Layers[0].LayerHeight);
        }
        this.tileBeneathLadder = v;
        Game1.player.showFrame(5, false);
      }
      this.isFallingDownShaft = false;
    }

    public override void performTenMinuteUpdate(int timeOfDay)
    {
      base.performTenMinuteUpdate(timeOfDay);
      if (this.mustKillAllMonstersToAdvance() && this.characters.Count == 0)
      {
        Vector2 vector2 = new Vector2((float) (int) this.tileBeneathLadder.X, (float) (int) this.tileBeneathLadder.Y);
        if (this.getTileIndexAt(Utility.Vector2ToPoint(vector2), "Buildings") == -1)
        {
          this.createLadderAt(vector2, "newArtifact");
          if (this.mustKillAllMonstersToAdvance())
            Game1.showGlobalMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:MineShaft.cs.9484"));
        }
      }
      while (this.map != null && this.Equals((object) Game1.currentLocation) && (this.mineLevel % 5 != 0 && Game1.random.NextDouble() < 0.1) && !Game1.player.hasBuff(23))
      {
        if (this.mineLevel > 10 && !this.mustKillAllMonstersToAdvance() && Game1.random.NextDouble() < 0.1)
        {
          this.fogTime = 35000 + Game1.random.Next(-5, 6) * 1000;
          Game1.changeMusicTrack("none");
          switch (this.getMineArea(-1))
          {
            case 40:
              this.fogColor = Color.Blue * 0.75f;
              continue;
            case 80:
              this.fogColor = Color.Red * 0.5f;
              continue;
            case 121:
              this.fogColor = Color.BlueViolet * 1f;
              continue;
            case 0:
            case 10:
              this.fogColor = this.isDarkArea() ? Color.Khaki : Color.Green * 0.75f;
              continue;
            default:
              continue;
          }
        }
        else
          this.spawnFlyingMonsterOffScreen();
      }
    }

    public void spawnFlyingMonsterOffScreen()
    {
      Vector2 zero = Vector2.Zero;
      switch (Game1.random.Next(4))
      {
        case 0:
          zero.X = (float) Game1.random.Next(this.map.Layers[0].LayerWidth);
          break;
        case 1:
          zero.X = (float) (this.map.Layers[0].LayerWidth - 1);
          zero.Y = (float) Game1.random.Next(this.map.Layers[0].LayerHeight);
          break;
        case 2:
          zero.Y = (float) (this.map.Layers[0].LayerHeight - 1);
          zero.X = (float) Game1.random.Next(this.map.Layers[0].LayerWidth);
          break;
        case 3:
          zero.Y = (float) Game1.random.Next(this.map.Layers[0].LayerHeight);
          break;
      }
      if (Utility.isOnScreen(zero * (float) Game1.tileSize, Game1.tileSize))
        zero.X -= (float) (Game1.viewport.Width / Game1.tileSize);
      switch (this.getMineArea(-1))
      {
        case 40:
          List<NPC> characters1 = this.characters;
          Bat bat1 = new Bat(zero * (float) Game1.tileSize, this.mineLevel);
          int num1 = 1;
          bat1.focusedOnFarmers = num1 != 0;
          characters1.Add((NPC) bat1);
          Game1.playSound("batScreech");
          break;
        case 80:
          List<NPC> characters2 = this.characters;
          Bat bat2 = new Bat(zero * (float) Game1.tileSize, this.mineLevel);
          int num2 = 1;
          bat2.focusedOnFarmers = num2 != 0;
          characters2.Add((NPC) bat2);
          Game1.playSound("batScreech");
          break;
        case 121:
          List<NPC> characters3 = this.characters;
          Serpent serpent = new Serpent(zero * (float) Game1.tileSize);
          int num3 = 1;
          serpent.focusedOnFarmers = num3 != 0;
          characters3.Add((NPC) serpent);
          Game1.playSound("serpentDie");
          break;
        case 0:
          if (this.mineLevel <= 10 || !this.isDarkArea())
            break;
          List<NPC> characters4 = this.characters;
          Bat bat3 = new Bat(zero * (float) Game1.tileSize, this.mineLevel);
          int num4 = 1;
          bat3.focusedOnFarmers = num4 != 0;
          characters4.Add((NPC) bat3);
          Game1.playSound("batScreech");
          break;
        case 10:
          List<NPC> characters5 = this.characters;
          Fly fly = new Fly(zero * (float) Game1.tileSize, false);
          int num5 = 1;
          fly.focusedOnFarmers = num5 != 0;
          characters5.Add((NPC) fly);
          break;
      }
    }

    public override void drawLightGlows(SpriteBatch b)
    {
      Color color;
      switch (this.getMineArea(-1))
      {
        case 80:
          color = this.isDarkArea() ? Color.Pink * 0.4f : Color.Red * 0.33f;
          break;
        case 121:
          color = Color.White * 0.8f;
          break;
        case 0:
          color = this.isDarkArea() ? Color.PaleGoldenrod * 0.5f : Color.PaleGoldenrod * 0.33f;
          break;
        case 40:
          color = Color.White * 0.65f;
          break;
        default:
          color = Color.PaleGoldenrod * 0.33f;
          break;
      }
      foreach (Vector2 lightGlow in this.lightGlows)
      {
        if (this.rainbowLights)
        {
          switch ((int) ((double) lightGlow.X / (double) Game1.tileSize + (double) lightGlow.Y / (double) Game1.tileSize) % 4)
          {
            case 0:
              color = Color.Red * 0.5f;
              break;
            case 1:
              color = Color.Yellow * 0.5f;
              break;
            case 2:
              color = Color.Cyan * 0.33f;
              break;
            case 3:
              color = Color.Lime * 0.45f;
              break;
          }
        }
        b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, lightGlow), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(88, 1779, 30, 30)), color, 0.0f, new Vector2(15f, 15f), (float) (Game1.pixelZoom * 2) + (float) ((double) (Game1.tileSize * 3 / 2) * Math.Sin((Game1.currentGameTime.TotalGameTime.TotalMilliseconds + (double) lightGlow.X * 777.0 + (double) lightGlow.Y * 9746.0) % 3140.0 / 1000.0) / 50.0), SpriteEffects.None, 1f);
      }
    }

    public override StardewValley.Object getFish(float millisecondsAfterNibble, int bait, int waterDepth, Farmer who, double baitPotency)
    {
      int parentSheetIndex = -1;
      double num1 = 1.0 + 0.4 * (double) who.FishingLevel + (double) waterDepth * 0.1;
      switch (this.getMineArea(-1))
      {
        case 40:
          double num2 = num1 + (bait == 682 ? 3.0 : 0.0);
          if (Game1.random.NextDouble() < 0.015 + 0.009 * num2)
          {
            parentSheetIndex = 161;
            break;
          }
          break;
        case 80:
          double num3 = num1 + (bait == 684 ? 3.0 : 0.0);
          if (Game1.random.NextDouble() < 0.01 + 0.008 * num3)
          {
            parentSheetIndex = 162;
            break;
          }
          break;
        case 0:
        case 10:
          double num4 = num1 + (bait == 689 ? 3.0 : 0.0);
          if (Game1.random.NextDouble() < 0.02 + 0.01 * num4)
          {
            parentSheetIndex = 158;
            break;
          }
          break;
      }
      int quality = 0;
      if (Game1.random.NextDouble() < (double) who.FishingLevel / 10.0)
        quality = 1;
      if (Game1.random.NextDouble() < (double) who.FishingLevel / 50.0 + (double) who.LuckLevel / 100.0)
        quality = 2;
      if (parentSheetIndex != -1)
        return new StardewValley.Object(parentSheetIndex, 1, false, -1, quality);
      if (this.getMineArea(-1) == 80)
        return new StardewValley.Object(Game1.random.Next(167, 173), 1, false, -1, 0);
      return base.getFish(millisecondsAfterNibble, bait, waterDepth, who, baitPotency);
    }

    private void adjustLevelChances(ref double stoneChance, ref double monsterChance, ref double itemChance, ref double gemStoneChance)
    {
      if (this.mineLevel == 1)
      {
        monsterChance = 0.0;
        itemChance = 0.0;
        gemStoneChance = 0.0;
      }
      else if (this.mineLevel % 5 == 0 && this.getMineArea(-1) != 121)
      {
        itemChance = 0.0;
        gemStoneChance = 0.0;
        if (this.mineLevel % 10 == 0)
          monsterChance = 0.0;
      }
      if (this.mustKillAllMonstersToAdvance())
      {
        monsterChance = 0.025;
        itemChance = 0.001;
        stoneChance = 0.0;
        gemStoneChance = 0.0;
      }
      if (Game1.player.hasBuff(23) && this.getMineArea(-1) != 121)
        monsterChance = 0.0;
      gemStoneChance = gemStoneChance / 2.0;
    }

    private void populateLevel()
    {
      this.objects.Clear();
      this.terrainFeatures.Clear();
      this.resourceClumps.Clear();
      this.debris.Clear();
      this.characters.Clear();
      this.ghostAdded = false;
      this.stonesLeftOnThisLevel = 0;
      double stoneChance = (double) this.mineRandom.Next(10, 30) / 100.0;
      double monsterChance = 0.002 + (double) this.mineRandom.Next(200) / 10000.0;
      double itemChance = 1.0 / 400.0;
      double gemStoneChance = 0.003;
      this.adjustLevelChances(ref stoneChance, ref monsterChance, ref itemChance, ref gemStoneChance);
      int numberSoFar = 0;
      bool firstTime = !this.permanentMineChanges.ContainsKey(this.mineLevel);
      if (this.mineLevel > 1 && this.mineLevel % 5 != 0 && (this.mineRandom.NextDouble() < 0.5 && !this.mustKillAllMonstersToAdvance()))
      {
        int num = this.mineRandom.Next(5) + (int) (Game1.dailyLuck * 20.0);
        for (int index = 0; index < num; ++index)
        {
          Point point1;
          Point point2;
          if (this.mineRandom.NextDouble() < 0.33)
          {
            point1 = new Point(this.mineRandom.Next(this.map.GetLayer("Back").LayerWidth), 0);
            point2 = new Point(0, 1);
          }
          else if (this.mineRandom.NextDouble() < 0.5)
          {
            point1 = new Point(0, this.mineRandom.Next(this.map.GetLayer("Back").LayerHeight));
            point2 = new Point(1, 0);
          }
          else
          {
            point1 = new Point(this.map.GetLayer("Back").LayerWidth - 1, this.mineRandom.Next(this.map.GetLayer("Back").LayerHeight));
            point2 = new Point(-1, 0);
          }
          while (this.isTileOnMap(point1.X, point1.Y))
          {
            point1.X += point2.X;
            point1.Y += point2.Y;
            if (this.isTileClearForMineObjects(point1.X, point1.Y))
            {
              Vector2 vector2 = new Vector2((float) point1.X, (float) point1.Y);
              this.objects.Add(vector2, (StardewValley.Object) new BreakableContainer(vector2, 118));
              break;
            }
          }
        }
      }
      this.addLevelUnique(firstTime);
      if (this.mineLevel % 10 != 0 || this.getMineArea(-1) == 121)
      {
        for (int index1 = 0; index1 < this.map.GetLayer("Back").LayerWidth; ++index1)
        {
          for (int index2 = 0; index2 < this.map.GetLayer("Back").LayerHeight; ++index2)
          {
            this.checkForMapAlterations(index1, index2);
            if (this.isTileClearForMineObjects(index1, index2))
            {
              if (this.mineRandom.NextDouble() <= stoneChance)
              {
                Vector2 vector2 = new Vector2((float) index1, (float) index2);
                if (!this.Objects.ContainsKey(vector2))
                {
                  if (this.getMineArea(-1) == 40 && this.mineRandom.NextDouble() < 0.15)
                    this.Objects.Add(vector2, new StardewValley.Object(vector2, this.mineRandom.Next(319, 322), "Weeds", true, false, false, false)
                    {
                      fragility = 2,
                      canBeGrabbed = true
                    });
                  else if (this.rainbowLights && this.mineRandom.NextDouble() < 0.55)
                  {
                    if (this.mineRandom.NextDouble() < 0.25)
                    {
                      int parentSheetIndex = 404;
                      switch (this.mineRandom.Next(5))
                      {
                        case 0:
                          parentSheetIndex = 422;
                          break;
                        case 1:
                          parentSheetIndex = 420;
                          break;
                        case 2:
                          parentSheetIndex = 420;
                          break;
                        case 3:
                          parentSheetIndex = 420;
                          break;
                        case 4:
                          parentSheetIndex = 420;
                          break;
                      }
                      this.Objects.Add(vector2, new StardewValley.Object(parentSheetIndex, 1, false, -1, 0)
                      {
                        isSpawnedObject = true
                      });
                    }
                  }
                  else
                  {
                    this.Objects.Add(vector2, this.chooseStoneType(0.001, 5E-05, gemStoneChance, vector2));
                    this.stonesLeftOnThisLevel = this.stonesLeftOnThisLevel + 1;
                  }
                }
              }
              else if (this.mineRandom.NextDouble() <= monsterChance && (double) Utility.distance(this.tileBeneathLadder.X, (float) index1, this.tileBeneathLadder.Y, (float) index2) > 5.0)
              {
                Monster monsterForThisLevel = this.getMonsterForThisLevel(this.mineLevel, index1, index2);
                if (monsterForThisLevel is Grub)
                {
                  if (this.mineRandom.NextDouble() < 0.4)
                    this.tryToAddMonster((Monster) new Grub(Vector2.Zero, false), index1 - 1, index2);
                  if (this.mineRandom.NextDouble() < 0.4)
                    this.tryToAddMonster((Monster) new Grub(Vector2.Zero, false), index1 + 1, index2);
                  if (this.mineRandom.NextDouble() < 0.4)
                    this.tryToAddMonster((Monster) new Grub(Vector2.Zero, false), index1, index2 - 1);
                  if (this.mineRandom.NextDouble() < 0.4)
                    this.tryToAddMonster((Monster) new Grub(Vector2.Zero, false), index1, index2 + 1);
                }
                else if (monsterForThisLevel is DustSpirit)
                {
                  if (this.mineRandom.NextDouble() < 0.6)
                    this.tryToAddMonster((Monster) new DustSpirit(Vector2.Zero), index1 - 1, index2);
                  if (this.mineRandom.NextDouble() < 0.6)
                    this.tryToAddMonster((Monster) new DustSpirit(Vector2.Zero), index1 + 1, index2);
                  if (this.mineRandom.NextDouble() < 0.6)
                    this.tryToAddMonster((Monster) new DustSpirit(Vector2.Zero), index1, index2 - 1);
                  if (this.mineRandom.NextDouble() < 0.6)
                    this.tryToAddMonster((Monster) new DustSpirit(Vector2.Zero), index1, index2 + 1);
                }
                if (this.mineRandom.NextDouble() < 0.00175)
                  monsterForThisLevel.hasSpecialItem = true;
                if (monsterForThisLevel.GetBoundingBox().Width <= Game1.tileSize || this.isTileClearForMineObjects(index1 + 1, index2))
                  this.characters.Add((NPC) monsterForThisLevel);
              }
              else if (this.mineRandom.NextDouble() <= itemChance)
                this.Objects.Add(new Vector2((float) index1, (float) index2), this.getRandomItemForThisLevel(this.mineLevel));
              else if (this.mineRandom.NextDouble() <= 0.005 && !this.isDarkArea() && (!this.mustKillAllMonstersToAdvance() && this.isTileClearForMineObjects(index1 + 1, index2)) && (this.isTileClearForMineObjects(index1, index2 + 1) && this.isTileClearForMineObjects(index1 + 1, index2 + 1)))
              {
                Vector2 tile = new Vector2((float) index1, (float) index2);
                int parentSheetIndex = this.mineRandom.NextDouble() < 0.5 ? 752 : 754;
                if (this.getMineArea(-1) == 40)
                  parentSheetIndex = this.mineRandom.NextDouble() < 0.5 ? 756 : 758;
                this.resourceClumps.Add(new ResourceClump(parentSheetIndex, 2, 2, tile));
              }
            }
            else if (this.isContainerPlatform(index1, index2) && this.isTileLocationTotallyClearAndPlaceable(index1, index2) && this.mineRandom.NextDouble() < 0.4 && (firstTime || this.canAdd(0, numberSoFar)))
            {
              Vector2 vector2 = new Vector2((float) index1, (float) index2);
              this.objects.Add(vector2, (StardewValley.Object) new BreakableContainer(vector2, 118));
              ++numberSoFar;
              if (firstTime)
                this.updateMineLevelData(0, 1);
            }
            else if (this.mineRandom.NextDouble() <= monsterChance && this.isTileLocationTotallyClearAndPlaceable(index1, index2) && this.isTileOnClearAndSolidGround(index1, index2) && (!Game1.player.hasBuff(23) || this.getMineArea(-1) == 121))
            {
              Monster monsterForThisLevel = this.getMonsterForThisLevel(this.mineLevel, index1, index2);
              if (this.mineRandom.NextDouble() < 0.01)
                monsterForThisLevel.hasSpecialItem = true;
              this.characters.Add((NPC) monsterForThisLevel);
            }
          }
        }
        if (this.stonesLeftOnThisLevel > 35)
        {
          int num1 = this.stonesLeftOnThisLevel / 35;
          for (int index1 = 0; index1 < num1; ++index1)
          {
            Vector2 index2 = this.objects.Keys.ElementAt<Vector2>(this.mineRandom.Next(this.objects.Count));
            if (this.objects[index2].name.Equals("Stone"))
            {
              int num2 = this.mineRandom.Next(3, 8);
              bool flag = this.mineRandom.NextDouble() < 0.1;
              for (int xTile = (int) index2.X - num2 / 2; (double) xTile < (double) index2.X + (double) (num2 / 2); ++xTile)
              {
                for (int yTile = (int) index2.Y - num2 / 2; (double) yTile < (double) index2.Y + (double) (num2 / 2); ++yTile)
                {
                  Vector2 key = new Vector2((float) xTile, (float) yTile);
                  if (this.objects.ContainsKey(key) && this.objects[key].name.Equals("Stone"))
                  {
                    this.objects.Remove(key);
                    this.stonesLeftOnThisLevel = this.stonesLeftOnThisLevel - 1;
                    if (flag && this.mineRandom.NextDouble() < 0.12)
                      this.characters.Add((NPC) this.getMonsterForThisLevel(this.mineLevel, xTile, yTile));
                  }
                }
              }
            }
          }
        }
        this.tryToAddAreaUniques();
        if (this.mineRandom.NextDouble() < 0.95 && !this.mustKillAllMonstersToAdvance() && (this.mineLevel > 1 && this.mineLevel % 5 != 0))
        {
          Vector2 v = new Vector2((float) this.mineRandom.Next(this.map.GetLayer("Back").LayerWidth), (float) this.mineRandom.Next(this.map.GetLayer("Back").LayerHeight));
          if (this.isTileClearForMineObjects(v))
            this.createLadderDown((int) v.X, (int) v.Y);
        }
        if (this.mustKillAllMonstersToAdvance() && this.characters.Count <= 1)
          this.characters.Add((NPC) new Bat(this.tileBeneathLadder * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize * 4), (float) (Game1.tileSize * 4))));
      }
      if (this.mustKillAllMonstersToAdvance() || this.mineLevel % 5 == 0 || this.mineLevel <= 2)
        return;
      this.tryToAddOreClumps();
      if (!this.isLightingDark)
        return;
      this.tryToAddOldMinerPath();
    }

    public void placeAppropriateOreAt(Vector2 tile)
    {
      if (!this.isTileLocationTotallyClearAndPlaceable(tile))
        return;
      this.objects.Add(tile, this.getAppropriateOre(tile));
    }

    public StardewValley.Object getAppropriateOre(Vector2 tile)
    {
      StardewValley.Object @object = new StardewValley.Object(tile, 751, "Stone", true, false, false, false)
      {
        minutesUntilReady = 3
      };
      switch (this.getMineArea(-1))
      {
        case 40:
          if (this.mineRandom.NextDouble() < 0.8)
          {
            @object = new StardewValley.Object(tile, 290, "Stone", true, false, false, false)
            {
              minutesUntilReady = 4
            };
            break;
          }
          break;
        case 80:
          if (this.mineRandom.NextDouble() < 0.8)
          {
            @object = new StardewValley.Object(tile, 764, "Stone", true, false, false, false)
            {
              minutesUntilReady = 8
            };
            break;
          }
          break;
        case 121:
          @object = new StardewValley.Object(tile, 764, "Stone", true, false, false, false)
          {
            minutesUntilReady = 8
          };
          if (this.mineRandom.NextDouble() < 0.02)
            return new StardewValley.Object(tile, 765, "Stone", true, false, false, false)
            {
              minutesUntilReady = 16
            };
          break;
      }
      if (this.mineRandom.NextDouble() < 0.25 && this.getMineArea(-1) != 40)
        @object = new StardewValley.Object(tile, this.mineRandom.NextDouble() < 0.5 ? 668 : 670, "Stone", true, false, false, false)
        {
          minutesUntilReady = 2
        };
      return @object;
    }

    public void tryToAddOreClumps()
    {
      if (this.mineRandom.NextDouble() >= 0.55 + Game1.dailyLuck)
        return;
      Vector2 randomTile = this.getRandomTile();
      for (int index = 0; index < 1 || this.mineRandom.NextDouble() < 0.25 + Game1.dailyLuck; ++index)
      {
        if (this.isTileLocationTotallyClearAndPlaceable(randomTile) && this.isTileOnClearAndSolidGround(randomTile) && this.doesTileHaveProperty((int) randomTile.X, (int) randomTile.Y, "Diggable", "Back") == null)
        {
          StardewValley.Object appropriateOre = this.getAppropriateOre(randomTile);
          if (appropriateOre.parentSheetIndex == 670)
            appropriateOre.parentSheetIndex = 668;
          Utility.recursiveObjectPlacement(appropriateOre, (int) randomTile.X, (int) randomTile.Y, 0.949999988079071, 0.300000011920929, (GameLocation) this, "Dirt", appropriateOre.parentSheetIndex == 668 ? 1 : 0, 0.0500000007450581, appropriateOre.parentSheetIndex == 668 ? 2 : 1);
        }
        randomTile = this.getRandomTile();
      }
    }

    public void tryToAddOldMinerPath()
    {
      Vector2 randomTile = this.getRandomTile();
      for (int index = 0; !this.isTileOnClearAndSolidGround(randomTile) && index < 8; ++index)
        randomTile = this.getRandomTile();
      if (!this.isTileOnClearAndSolidGround(randomTile))
        return;
      Stack<Point> path = PathFindController.findPath(Utility.Vector2ToPoint(this.tileBeneathLadder), Utility.Vector2ToPoint(randomTile), new PathFindController.isAtEnd(PathFindController.isAtEndPoint), (GameLocation) this, (Character) Game1.player, 500);
      if (path == null)
        return;
      while (path.Count > 0)
      {
        Point point = path.Pop();
        this.removeEverythingExceptCharactersFromThisTile(point.X, point.Y);
        if (path.Count > 0 && this.mineRandom.NextDouble() < 0.2)
        {
          Vector2 vector2 = Vector2.Zero;
          vector2 = path.Peek().X != point.X ? new Vector2((float) point.X, (float) (point.Y + (this.mineRandom.NextDouble() < 0.5 ? -1 : 1))) : new Vector2((float) (point.X + (this.mineRandom.NextDouble() < 0.5 ? -1 : 1)), (float) point.Y);
          if (!vector2.Equals(Vector2.Zero) && this.isTileLocationTotallyClearAndPlaceable(vector2) && this.isTileOnClearAndSolidGround(vector2))
          {
            if (this.mineRandom.NextDouble() < 0.5)
              new Torch(vector2, 1).placementAction((GameLocation) this, (int) vector2.X * Game1.tileSize, (int) vector2.Y * Game1.tileSize, (Farmer) null);
            else
              this.placeAppropriateOreAt(vector2);
          }
        }
      }
    }

    public void tryToAddAreaUniques()
    {
      if (this.getMineArea(-1) != 10 && this.getMineArea(-1) != 80 && (this.getMineArea(-1) != 40 || this.mineRandom.NextDouble() >= 0.1) || (this.isDarkArea() || this.mustKillAllMonstersToAdvance()))
        return;
      int num = this.mineRandom.Next(7, 24);
      int parentSheetIndex = this.getMineArea(-1) == 80 ? 316 : (this.getMineArea(-1) == 40 ? 319 : 313);
      for (int index = 0; index < num; ++index)
      {
        Vector2 tileLocation = new Vector2((float) this.mineRandom.Next(this.map.GetLayer("Back").LayerWidth), (float) this.mineRandom.Next(this.map.GetLayer("Back").LayerHeight));
        StardewValley.Object o = new StardewValley.Object(tileLocation, parentSheetIndex, "Weeds", true, false, false, false);
        o.fragility = 2;
        o.canBeGrabbed = true;
        int x = (int) tileLocation.X;
        int y = (int) tileLocation.Y;
        double growthRate = 1.0;
        double decay = (double) this.mineRandom.Next(10, 40) / 100.0;
        string terrainToExclude = "Dirt";
        int objectIndexAddRange = 2;
        double failChance = 0.29;
        int objectIndeAddRangeMultiplier = 1;
        Utility.recursiveObjectPlacement(o, x, y, growthRate, decay, (GameLocation) this, terrainToExclude, objectIndexAddRange, failChance, objectIndeAddRangeMultiplier);
      }
    }

    public void tryToAddMonster(Monster m, int tileX, int tileY)
    {
      if (!this.isTileClearForMineObjects(tileX, tileY) || this.isTileOccupied(new Vector2((float) tileX, (float) tileY), ""))
        return;
      m.setTilePosition(tileX, tileY);
      this.characters.Add((NPC) m);
    }

    public bool isContainerPlatform(int x, int y)
    {
      return this.map.GetLayer("Back").Tiles[x, y] != null && this.map.GetLayer("Back").Tiles[x, y].TileIndex == 257;
    }

    public bool mustKillAllMonstersToAdvance()
    {
      if (!this.isSlimeArea)
        return this.isMonsterArea;
      return true;
    }

    public void createLadderAt(Vector2 p, string sound = "hoeHit")
    {
      this.setMapTileIndex((int) p.X, (int) p.Y, 173, "Buildings", 0);
      Game1.playSound(sound);
      this.temporarySprites.Add(new TemporaryAnimatedSprite(5, p * (float) Game1.tileSize, Color.White * 0.5f, 8, false, 100f, 0, -1, -1f, -1, 0)
      {
        interval = 80f
      });
      this.temporarySprites.Add(new TemporaryAnimatedSprite(5, p * (float) Game1.tileSize - new Vector2((float) (Game1.tileSize / 4), (float) (Game1.tileSize / 4)), Color.White * 0.5f, 8, false, 100f, 0, -1, -1f, -1, 0)
      {
        delayBeforeAnimationStart = 150,
        interval = 80f,
        scale = 0.75f,
        startSound = "sandyStep"
      });
      this.temporarySprites.Add(new TemporaryAnimatedSprite(5, p * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 4)), Color.White * 0.5f, 8, false, 100f, 0, -1, -1f, -1, 0)
      {
        delayBeforeAnimationStart = 300,
        interval = 80f,
        scale = 0.75f,
        startSound = "sandyStep"
      });
      this.temporarySprites.Add(new TemporaryAnimatedSprite(5, p * (float) Game1.tileSize - new Vector2((float) (Game1.tileSize / 2), (float) (-Game1.tileSize / 4)), Color.White * 0.5f, 8, false, 100f, 0, -1, -1f, -1, 0)
      {
        delayBeforeAnimationStart = 450,
        interval = 80f,
        scale = 0.75f,
        startSound = "sandyStep"
      });
      this.temporarySprites.Add(new TemporaryAnimatedSprite(5, p * (float) Game1.tileSize - new Vector2((float) (-Game1.tileSize / 4), (float) (Game1.tileSize / 4)), Color.White * 0.5f, 8, false, 100f, 0, -1, -1f, -1, 0)
      {
        delayBeforeAnimationStart = 600,
        interval = 80f,
        scale = 0.75f,
        startSound = "sandyStep"
      });
      Game1.player.temporaryImpassableTile = new Microsoft.Xna.Framework.Rectangle((int) p.X * Game1.tileSize, (int) p.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
    }

    public bool recursiveTryToCreateLadderDown(Vector2 centerTile, string sound = "hoeHit", int maxIterations = 16)
    {
      int num = 0;
      Queue<Vector2> vector2Queue = new Queue<Vector2>();
      vector2Queue.Enqueue(centerTile);
      List<Vector2> vector2List = new List<Vector2>();
      for (; num < maxIterations && vector2Queue.Count > 0; ++num)
      {
        Vector2 vector2 = vector2Queue.Dequeue();
        vector2List.Add(vector2);
        if (!this.isTileOccupied(vector2, "ignoreMe") && this.isTileOnClearAndSolidGround(vector2) && (this.isTileOccupiedByFarmer(vector2) == null && this.doesTileHaveProperty((int) vector2.X, (int) vector2.Y, "Type", "Back") != null) && this.doesTileHaveProperty((int) vector2.X, (int) vector2.Y, "Type", "Back").Equals("Stone"))
        {
          this.createLadderAt(vector2, "hoeHit");
          return true;
        }
        foreach (Vector2 directionsTileVector in Utility.DirectionsTileVectors)
        {
          if (!vector2List.Contains(vector2 + directionsTileVector))
            vector2Queue.Enqueue(vector2 + directionsTileVector);
        }
      }
      return false;
    }

    public override void monsterDrop(Monster monster, int x, int y)
    {
      if (monster.hasSpecialItem)
        Game1.createItemDebris(MineShaft.getSpecialItemForThisMineLevel(this.mineLevel, x / Game1.tileSize, y / Game1.tileSize), monster.position, Game1.random.Next(4), (GameLocation) null);
      else
        base.monsterDrop(monster, x, y);
      if ((this.mustKillAllMonstersToAdvance() || Game1.random.NextDouble() >= 0.15) && (!this.mustKillAllMonstersToAdvance() || this.characters.Count > 1))
        return;
      Vector2 vector2 = new Vector2((float) x, (float) y) / (float) Game1.tileSize;
      vector2.X = (float) (int) vector2.X;
      vector2.Y = (float) (int) vector2.Y;
      monster.name = "ignoreMe";
      Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle((int) vector2.X * Game1.tileSize, (int) vector2.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
      if (!this.isTileOccupied(vector2, "ignoreMe") && this.isTileOnClearAndSolidGround(vector2) && (!Game1.player.GetBoundingBox().Intersects(rectangle) && this.doesTileHaveProperty((int) vector2.X, (int) vector2.Y, "Type", "Back") != null && this.doesTileHaveProperty((int) vector2.X, (int) vector2.Y, "Type", "Back").Equals("Stone")))
      {
        this.createLadderAt(vector2, "hoeHit");
      }
      else
      {
        if (!this.mustKillAllMonstersToAdvance() || this.characters.Count > 1)
          return;
        vector2 = new Vector2((float) (int) this.tileBeneathLadder.X, (float) (int) this.tileBeneathLadder.Y);
        this.createLadderAt(vector2, "newArtifact");
        if (!this.mustKillAllMonstersToAdvance())
          return;
        Game1.showGlobalMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:MineShaft.cs.9484"));
      }
    }

    public override bool performToolAction(Tool t, int tileX, int tileY)
    {
      for (int index = this.resourceClumps.Count - 1; index >= 0; --index)
      {
        if (this.resourceClumps[index] != null && this.resourceClumps[index].getBoundingBox(this.resourceClumps[index].tile).Contains(tileX * Game1.tileSize, tileY * Game1.tileSize))
        {
          if (this.resourceClumps[index].performToolAction(t, 1, this.resourceClumps[index].tile, (GameLocation) null))
            this.resourceClumps.RemoveAt(index);
          return true;
        }
      }
      return base.performToolAction(t, tileX, tileY);
    }

    private void addLevelUnique(bool firstTime)
    {
      List<Item> items = new List<Item>();
      Vector2 vector2 = new Vector2(9f, 9f);
      Color color = Color.White;
      if (this.mineLevel % 20 == 0 && this.mineLevel % 40 != 0)
        vector2.Y += 4f;
      switch (this.mineLevel)
      {
        case 100:
          items.Add((Item) new StardewValley.Object(434, 1, false, -1, 0));
          break;
        case 110:
          items.Add((Item) new Boots(514));
          break;
        case 120:
          Game1.player.completeQuest(18);
          Game1.getSteamAchievement("Achievement_TheBottom");
          if (!Game1.player.hasSkullKey)
            items.Add((Item) new SpecialItem(1, 4, ""));
          color = Color.Pink;
          break;
        case 70:
          items.Add((Item) new Slingshot(33));
          break;
        case 80:
          items.Add((Item) new Boots(512));
          break;
        case 90:
          items.Add((Item) new MeleeWeapon(8));
          break;
        case 40:
          Game1.player.completeQuest(17);
          items.Add((Item) new Slingshot());
          break;
        case 50:
          items.Add((Item) new Boots(509));
          break;
        case 60:
          items.Add((Item) new MeleeWeapon(21));
          break;
        case 5:
          Game1.player.completeQuest(14);
          if (!Game1.player.hasOrWillReceiveMail("guildQuest"))
          {
            Game1.addMailForTomorrow("guildQuest", false, false);
            break;
          }
          break;
        case 10:
          items.Add((Item) new Boots(506));
          break;
        case 20:
          items.Add((Item) new MeleeWeapon(11));
          break;
      }
      if (items.Count <= 0 || !this.canAdd(1, 0))
        return;
      this.objects.Add(vector2, (StardewValley.Object) new Chest(0, items, vector2, false)
      {
        tint = color
      });
      if (!firstTime)
        return;
      this.updateMineLevelData(1, 1);
    }

    public static Item getSpecialItemForThisMineLevel(int level, int x, int y)
    {
      Random random = new Random(level + (int) Game1.stats.DaysPlayed + x + y * 10000);
      if (level < 20)
      {
        switch (random.Next(6))
        {
          case 0:
            return (Item) new MeleeWeapon(16);
          case 1:
            return (Item) new MeleeWeapon(24);
          case 2:
            return (Item) new Boots(504);
          case 3:
            return (Item) new Boots(505);
          case 4:
            return (Item) new Ring(516);
          case 5:
            return (Item) new Ring(518);
        }
      }
      else if (level < 40)
      {
        switch (random.Next(7))
        {
          case 0:
            return (Item) new MeleeWeapon(22);
          case 1:
            return (Item) new MeleeWeapon(24);
          case 2:
            return (Item) new Boots(504);
          case 3:
            return (Item) new Boots(505);
          case 4:
            return (Item) new Ring(516);
          case 5:
            return (Item) new Ring(518);
          case 6:
            return (Item) new MeleeWeapon(15);
        }
      }
      else if (level < 60)
      {
        switch (random.Next(7))
        {
          case 0:
            return (Item) new MeleeWeapon(6);
          case 1:
            return (Item) new MeleeWeapon(26);
          case 2:
            return (Item) new MeleeWeapon(15);
          case 3:
            return (Item) new Boots(510);
          case 4:
            return (Item) new Ring(517);
          case 5:
            return (Item) new Ring(519);
          case 6:
            return (Item) new MeleeWeapon(27);
        }
      }
      else if (level < 160)
      {
        switch (random.Next(7))
        {
          case 0:
            return (Item) new MeleeWeapon(26);
          case 1:
            return (Item) new MeleeWeapon(26);
          case 2:
            return (Item) new Boots(508);
          case 3:
            return (Item) new Boots(510);
          case 4:
            return (Item) new Ring(517);
          case 5:
            return (Item) new Ring(519);
          case 6:
            return (Item) new MeleeWeapon(26);
        }
      }
      else if (level < 100)
      {
        switch (random.Next(7))
        {
          case 0:
            return (Item) new MeleeWeapon(48);
          case 1:
            return (Item) new MeleeWeapon(48);
          case 2:
            return (Item) new Boots(511);
          case 3:
            return (Item) new Boots(513);
          case 4:
            return (Item) new MeleeWeapon(18);
          case 5:
            return (Item) new MeleeWeapon(28);
          case 6:
            return (Item) new MeleeWeapon(52);
        }
      }
      else if (level < 120)
      {
        switch (random.Next(6))
        {
          case 0:
            return (Item) new MeleeWeapon(19);
          case 1:
            return (Item) new MeleeWeapon(50);
          case 2:
            return (Item) new Boots(511);
          case 3:
            return (Item) new Boots(513);
          case 4:
            return (Item) new MeleeWeapon(18);
          case 5:
            return (Item) new MeleeWeapon(46);
        }
      }
      else
      {
        switch (random.Next(8))
        {
          case 0:
            return (Item) new MeleeWeapon(45);
          case 1:
            return (Item) new MeleeWeapon(50);
          case 2:
            return (Item) new Boots(511);
          case 3:
            return (Item) new Boots(513);
          case 4:
            return (Item) new MeleeWeapon(18);
          case 5:
            return (Item) new MeleeWeapon(28);
          case 6:
            return (Item) new MeleeWeapon(52);
          case 7:
            return (Item) new StardewValley.Object(787, 1, false, -1, 0);
        }
      }
      return (Item) new StardewValley.Object(78, 1, false, -1, 0);
    }

    public override bool isTileOccupied(Vector2 tileLocation, string characterToIgnore = "")
    {
      foreach (ResourceClump resourceClump in this.resourceClumps)
      {
        if (resourceClump.occupiesTile((int) tileLocation.X, (int) tileLocation.Y))
          return true;
      }
      if (this.tileBeneathLadder.Equals(tileLocation))
        return true;
      return base.isTileOccupied(tileLocation, characterToIgnore);
    }

    public bool isDarkArea()
    {
      if (this.loadedDarkArea || this.mineLevel % 40 > 30)
        return this.getMineArea(-1) != 40;
      return false;
    }

    public bool isTileClearForMineObjects(Vector2 v)
    {
      if (this.tileBeneathLadder.Equals(v) || this.tileBeneathElevator.Equals(v) || !this.isTileLocationTotallyClearAndPlaceable(v))
        return false;
      string str = this.doesTileHaveProperty((int) v.X, (int) v.Y, "Type", "Back");
      return str != null && str.Equals("Stone") && (this.isTileOnClearAndSolidGround(v) && !this.objects.ContainsKey(v));
    }

    public bool isTileOnClearAndSolidGround(Vector2 v)
    {
      return this.map.GetLayer("Back").Tiles[(int) v.X, (int) v.Y] != null && this.map.GetLayer("Front").Tiles[(int) v.X, (int) v.Y] == null && (this.map.GetLayer("Buildings").Tiles[(int) v.X, (int) v.Y] == null && this.getTileIndexAt((int) v.X, (int) v.Y, "Back") != 77);
    }

    public bool isTileOnClearAndSolidGround(int x, int y)
    {
      return this.map.GetLayer("Back").Tiles[x, y] != null && this.map.GetLayer("Front").Tiles[x, y] == null && this.getTileIndexAt(x, y, "Back") != 77;
    }

    public bool isTileClearForMineObjects(int x, int y)
    {
      return this.isTileClearForMineObjects(new Vector2((float) x, (float) y));
    }

    public void loadLevel(int level)
    {
      this.isMonsterArea = false;
      this.isSlimeArea = false;
      this.loadedDarkArea = false;
      this.mineLoader.Unload();
      this.mineLoader.Dispose();
      this.mineLoader = Game1.content.CreateTemporary();
      int num = (level % 40 % 20 != 0 || level % 40 == 0 ? (level % 10 == 0 ? 10 : level) : 20) % 40;
      if (level == 120)
        num = 120;
      if (this.getMineArea(level) == 121)
      {
        num = this.mineRandom.Next(40);
        while (num % 5 == 0)
          num = this.mineRandom.Next(40);
      }
      this.map = this.mineLoader.Load<Map>("Maps\\Mines\\" + (object) num);
      Random random = new Random((int) Game1.stats.DaysPlayed + level + (int) Game1.uniqueIDForThisGame / 2);
      if ((!Game1.player.hasBuff(23) || this.getMineArea(-1) == 121) && (random.NextDouble() < 0.05 && num % 5 != 0) && (num % 40 > 5 && num % 40 < 30 && num % 40 != 19))
      {
        if (random.NextDouble() < 0.5)
          this.isMonsterArea = true;
        else
          this.isSlimeArea = true;
        Game1.showGlobalMessage(Game1.content.LoadString(random.NextDouble() < 0.5 ? "Strings\\Locations:Mines_Infested" : "Strings\\Locations:Mines_Overrun"));
      }
      if (this.getMineArea(this.nextLevel) != this.getMineArea(this.mineLevel) || this.mineLevel == 120)
        Game1.changeMusicTrack("none");
      if (this.isSlimeArea)
      {
        this.map.TileSheets[0].ImageSource = "Maps\\Mines\\mine_slime";
        this.map.LoadTileSheets(Game1.mapDisplayDevice);
      }
      else if (this.getMineArea(-1) == 0 || this.getMineArea(-1) == 10 || this.getMineArea(this.nextLevel) != 0 && this.getMineArea(this.nextLevel) != 10)
      {
        if (this.getMineArea(this.nextLevel) == 40)
        {
          this.map.TileSheets[0].ImageSource = "Maps\\Mines\\mine_frost";
          if (this.nextLevel >= 70)
          {
            this.map.TileSheets[0].ImageSource += "_dark";
            this.loadedDarkArea = true;
          }
          this.map.LoadTileSheets(Game1.mapDisplayDevice);
        }
        else if (this.getMineArea(this.nextLevel) == 80)
        {
          this.map.TileSheets[0].ImageSource = "Maps\\Mines\\mine_lava";
          if (this.nextLevel >= 110 && this.nextLevel != 120)
          {
            this.map.TileSheets[0].ImageSource += "_dark";
            this.loadedDarkArea = true;
          }
          this.map.LoadTileSheets(Game1.mapDisplayDevice);
        }
        else if (this.getMineArea(this.nextLevel) == 121)
        {
          this.map.TileSheets[0].ImageSource = "Maps\\Mines\\mine_desert";
          if (num % 40 >= 30)
          {
            this.map.TileSheets[0].ImageSource += "_dark";
            this.loadedDarkArea = true;
          }
          this.map.LoadTileSheets(Game1.mapDisplayDevice);
          if (this.nextLevel >= 145 && Game1.player.hasQuest(20) && !Game1.player.hasOrWillReceiveMail("QiChallengeComplete"))
          {
            Game1.player.completeQuest(20);
            Game1.addMailForTomorrow("QiChallengeComplete", false, false);
          }
        }
      }
      if (!this.map.TileSheets[0].TileIndexProperties[165].ContainsKey("Diggable"))
        this.map.TileSheets[0].TileIndexProperties[165].Add("Diggable", new PropertyValue("true"));
      if (!this.map.TileSheets[0].TileIndexProperties[181].ContainsKey("Diggable"))
        this.map.TileSheets[0].TileIndexProperties[181].Add("Diggable", new PropertyValue("true"));
      if (!this.map.TileSheets[0].TileIndexProperties[183].ContainsKey("Diggable"))
        this.map.TileSheets[0].TileIndexProperties[183].Add("Diggable", new PropertyValue("true"));
      this.mineLevel = this.nextLevel;
      if (this.nextLevel > this.lowestLevelReached)
      {
        this.lowestLevelReached = this.nextLevel;
        Game1.player.deepestMineLevel = this.nextLevel;
      }
      if (this.mineLevel % 5 == 0 && this.getMineArea(-1) != 121)
        this.prepareElevator();
      Utility.CollectGarbage("", 0);
    }

    private void prepareElevator()
    {
      Point tile = Utility.findTile((GameLocation) this, 80, "Buildings");
      this.ElevatorLightSpot = tile;
      if (tile.X < 0)
        return;
      if (this.canAdd(3, 0))
      {
        this.timeUntilElevatorLightUp = 1500;
        this.updateMineLevelData(3, 1);
      }
      else
        this.setMapTileIndex(tile.X, tile.Y, 48, "Buildings", 0);
    }

    public void enterMineShaft()
    {
      DelayedAction.playSoundAfterDelay("fallDown", 1200);
      DelayedAction.playSoundAfterDelay("clubSmash", 2200);
      int num = this.mineRandom.Next(3, 9);
      if (this.mineRandom.NextDouble() < 0.1)
        num = num * 2 - 1;
      this.lastLevelsDownFallen = num;
      Game1.player.health = Math.Max(1, Game1.player.health - num * 3);
      this.isFallingDownShaft = true;
      Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.afterFall), 0.025f);
      Game1.player.CanMove = false;
      Game1.player.jump();
    }

    private void afterFall()
    {
      Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:MineShaft.cs.9578", (object) this.lastLevelsDownFallen) + (this.lastLevelsDownFallen > 7 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:MineShaft.cs.9580") : ""));
      Game1.drawObjectDialogue(Game1.content.LoadString(this.lastLevelsDownFallen > 7 ? "Strings\\Locations:Mines_FallenFar" : "Strings\\Locations:Mines_Fallen", (object) this.lastLevelsDownFallen));
      this.setNextLevel(this.mineLevel + this.lastLevelsDownFallen);
      Game1.messagePause = true;
      Game1.warpFarmer("UndergroundMine", 0, 0, 2);
      Game1.player.faceDirection(2);
      Game1.player.showFrame(5, false);
      Game1.globalFadeToClear((Game1.afterFadeFunction) null, 0.01f);
    }

    public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
    {
      Tile tile = this.map.GetLayer("Buildings").PickTile(new Location(tileLocation.X * Game1.tileSize, tileLocation.Y * Game1.tileSize), viewport.Size);
      if (tile == null || !who.IsMainPlayer)
        return base.checkAction(tileLocation, viewport, who);
      switch (tile.TileIndex)
      {
        case 173:
          Game1.enterMine(false, this.mineLevel + 1, (string) null);
          Game1.playSound("stairsdown");
          break;
        case 174:
          Response[] answerChoices = new Response[2]
          {
            new Response("Jump", Game1.content.LoadString("Strings\\Locations:Mines_ShaftJumpIn")),
            new Response("Do", Game1.content.LoadString("Strings\\Locations:Mines_DoNothing"))
          };
          this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:Mines_Shaft"), answerChoices, "Shaft");
          break;
        case 194:
          Game1.playSound("openBox");
          Game1.playSound("Ship");
          ++this.map.GetLayer("Buildings").Tiles[tileLocation].TileIndex;
          ++this.map.GetLayer("Front").Tiles[tileLocation.X, tileLocation.Y - 1].TileIndex;
          Game1.createRadialDebris((GameLocation) this, 382, tileLocation.X, tileLocation.Y, 6, false, -1, true, -1);
          this.updateMineLevelData(2, -1);
          break;
        case 112:
          Game1.activeClickableMenu = (IClickableMenu) new MineElevatorMenu();
          break;
        case 115:
          this.createQuestionDialogue(" ", new Response[2]
          {
            new Response("Leave", Game1.content.LoadString("Strings\\Locations:Mines_LeaveMine")),
            new Response("Do", Game1.content.LoadString("Strings\\Locations:Mines_DoNothing"))
          }, "ExitMine");
          break;
      }
      return true;
    }

    public override string checkForBuriedItem(int xLocation, int yLocation, bool explosion, bool detectOnly)
    {
      if (Game1.random.NextDouble() >= 0.15)
        return "";
      int objectIndex = 330;
      if (Game1.random.NextDouble() < 0.07)
      {
        if (Game1.random.NextDouble() < 0.75)
        {
          switch (Game1.random.Next(5))
          {
            case 0:
              objectIndex = 96;
              break;
            case 1:
              objectIndex = Game1.player.archaeologyFound.ContainsKey(102) ? (Game1.player.archaeologyFound[102][0] < 21 ? 102 : 770) : 770;
              break;
            case 2:
              objectIndex = 110;
              break;
            case 3:
              objectIndex = 112;
              break;
            case 4:
              objectIndex = 585;
              break;
          }
        }
        else if (Game1.random.NextDouble() < 0.75)
        {
          switch (this.getMineArea(-1))
          {
            case 0:
              objectIndex = Game1.random.NextDouble() < 0.5 ? 121 : 97;
              break;
            case 40:
              objectIndex = Game1.random.NextDouble() < 0.5 ? 122 : 336;
              break;
            case 80:
              objectIndex = 99;
              break;
          }
        }
        else
          objectIndex = Game1.random.NextDouble() < 0.5 ? 126 : (int) sbyte.MaxValue;
      }
      else if (Game1.random.NextDouble() < 0.19)
      {
        objectIndex = Game1.random.NextDouble() < 0.5 ? 390 : this.getOreIndexForLevel(this.mineLevel, Game1.random);
      }
      else
      {
        if (Game1.random.NextDouble() < 0.08)
        {
          Game1.createRadialDebris((GameLocation) this, 8, xLocation, yLocation, Game1.random.Next(1, 5), true, -1, false, -1);
          return "";
        }
        if (Game1.random.NextDouble() < 0.45)
          objectIndex = 330;
        else if (Game1.random.NextDouble() < 0.12)
        {
          if (Game1.random.NextDouble() < 0.25)
          {
            objectIndex = 749;
          }
          else
          {
            switch (this.getMineArea(-1))
            {
              case 0:
                objectIndex = 535;
                break;
              case 40:
                objectIndex = 536;
                break;
              case 80:
                objectIndex = 537;
                break;
            }
          }
        }
        else
          objectIndex = 78;
      }
      Game1.createObjectDebris(objectIndex, xLocation, yLocation, Game1.player.uniqueMultiplayerID, (GameLocation) this);
      return "";
    }

    public override bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer, int damagesFarmer, bool glider, Character character)
    {
      foreach (ResourceClump resourceClump in this.resourceClumps)
      {
        if (!glider && resourceClump.getBoundingBox(resourceClump.tile).Intersects(position))
          return true;
      }
      return base.isCollidingPosition(position, viewport, isFarmer, damagesFarmer, glider, character);
    }

    public override void drawAboveAlwaysFrontLayer(SpriteBatch b)
    {
      base.drawAboveAlwaysFrontLayer(b);
      foreach (NPC character in this.characters)
      {
        if (character is Monster)
          (character as Monster).drawAboveAllLayers(b);
      }
      if ((double) this.fogAlpha > 0.0 || this.ambientFog)
      {
        Vector2 position = new Vector2();
        float num1 = (float) (-64 * Game1.pixelZoom + (int) ((double) this.fogPos.X % (double) (64 * Game1.pixelZoom)));
        while ((double) num1 < (double) Game1.graphics.GraphicsDevice.Viewport.Width)
        {
          float num2 = (float) (-64 * Game1.pixelZoom + (int) ((double) this.fogPos.Y % (double) (64 * Game1.pixelZoom)));
          while ((double) num2 < (double) Game1.graphics.GraphicsDevice.Viewport.Height)
          {
            position.X = (float) (int) num1;
            position.Y = (float) (int) num2;
            b.Draw(Game1.mouseCursors, position, new Microsoft.Xna.Framework.Rectangle?(this.fogSource), (double) this.fogAlpha > 0.0 ? this.fogColor * this.fogAlpha : Color.Black * 0.95f, 0.0f, Vector2.Zero, (float) Game1.pixelZoom + 1f / 1000f, SpriteEffects.None, 1f);
            num2 += (float) (64 * Game1.pixelZoom);
          }
          num1 += (float) (64 * Game1.pixelZoom);
        }
      }
      if (this.isMonsterArea)
        b.Draw(Game1.mouseCursors, new Vector2((float) (Game1.tileSize / 4), (float) (Game1.tileSize / 4)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(193, 324, 7, 10)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom + Game1.dialogueButtonScale / 25f, SpriteEffects.None, 1f);
      else
        SpriteText.drawString(b, string.Concat((object) (this.mineLevel + (this.getMineArea(-1) == 121 ? -120 : 0))), Game1.tileSize / 4, Game1.tileSize / 4, 999999, -1, 999999, 1f, 1f, false, 2, "", this.getMineArea(-1) == 0 || this.isDarkArea() ? 4 : (this.getMineArea(-1) == 10 ? 6 : (this.getMineArea(-1) == 40 ? 7 : (this.getMineArea(-1) == 80 ? 2 : 3))));
    }

    public override void checkForMusic(GameTime time)
    {
      if (Game1.player.freezePause > 0 || this.fogTime > 0 || this.mineLevel == 120)
        return;
      if (Game1.currentSong == null || !Game1.currentSong.IsPlaying)
      {
        string str = "";
        switch (this.getMineArea(-1))
        {
          case 40:
            str = "Frost";
            break;
          case 80:
            str = "Lava";
            break;
          case 121:
          case 0:
          case 10:
            str = "Upper";
            break;
        }
        Game1.changeMusicTrack(str + "_Ambient");
      }
      this.timeSinceLastMusic = Math.Min(335000, this.timeSinceLastMusic + time.ElapsedGameTime.Milliseconds);
    }

    public void playMineSong()
    {
      if (Game1.currentSong != null && Game1.currentSong.IsPlaying && !Game1.currentSong.Name.Contains("Ambient") || this.isDarkArea())
        return;
      this.timeSinceLastMusic = 0;
      if (Game1.player.isWearingRing(528))
        Game1.changeMusicTrack(Utility.getRandomNonLoopingSong());
      else if (this.mineLevel < 40)
        Game1.changeMusicTrack("EarthMine");
      else if (this.mineLevel < 80)
        Game1.changeMusicTrack("FrostMine");
      else
        Game1.changeMusicTrack("LavaMine");
    }

    public override void resetForPlayerEntry()
    {
      base.resetForPlayerEntry();
      this.forceViewportPlayerFollow = true;
    }

    public void createLadderDown(int x, int y)
    {
      if (this.getMineArea(-1) == 121 && !this.mustKillAllMonstersToAdvance() && this.mineRandom.NextDouble() < 0.2)
      {
        this.map.GetLayer("Buildings").Tiles[x, y] = (Tile) new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 174);
      }
      else
      {
        this.ladderHasSpawned = true;
        this.map.GetLayer("Buildings").Tiles[x, y] = (Tile) new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 173);
      }
      Game1.player.temporaryImpassableTile = new Microsoft.Xna.Framework.Rectangle(x * Game1.tileSize, y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
    }

    public void checkStoneForItems(int tileIndexOfStone, int x, int y, Farmer who)
    {
      if (who == null)
        who = Game1.player;
      double num1 = Game1.dailyLuck / 2.0 + (double) who.MiningLevel * 0.005 + (double) who.LuckLevel * 0.001;
      Random r = new Random(x * 1000 + y + this.mineLevel + (int) Game1.uniqueIDForThisGame / 2);
      r.NextDouble();
      double num2 = tileIndexOfStone == 40 || tileIndexOfStone == 42 ? 1.2 : 0.8;
      if (tileIndexOfStone != 34 && tileIndexOfStone != 36 && tileIndexOfStone != 50)
        ;
      this.stonesLeftOnThisLevel = this.stonesLeftOnThisLevel - 1;
      double num3 = 0.02 + 1.0 / (double) Math.Max(1, this.stonesLeftOnThisLevel) + (double) who.LuckLevel / 100.0 + Game1.dailyLuck / 5.0;
      if (this.characters.Count == 0)
        num3 += 0.04;
      if (!this.ladderHasSpawned && (this.stonesLeftOnThisLevel == 0 || r.NextDouble() < num3))
        this.createLadderDown(x, y);
      if (this.breakStone(tileIndexOfStone, x, y, who, r))
        return;
      if (tileIndexOfStone == 44)
      {
        int num4 = r.Next(59, 70);
        int objectIndex = num4 + num4 % 2;
        if (who.timesReachedMineBottom == 0)
        {
          if (this.mineLevel < 40 && objectIndex != 66 && objectIndex != 68)
            objectIndex = r.NextDouble() < 0.5 ? 66 : 68;
          else if (this.mineLevel < 80 && (objectIndex == 64 || objectIndex == 60))
            objectIndex = r.NextDouble() < 0.5 ? (r.NextDouble() < 0.5 ? 66 : 70) : (r.NextDouble() < 0.5 ? 68 : 62);
        }
        Game1.createObjectDebris(objectIndex, x, y, who.uniqueMultiplayerID, (GameLocation) this);
        ++Game1.stats.OtherPreciousGemsFound;
      }
      else
      {
        if (r.NextDouble() < 0.022 * (1.0 + num1) * (who.professions.Contains(22) ? 2.0 : 1.0))
        {
          int objectIndex = 535 + (this.getMineArea(-1) == 40 ? 1 : (this.getMineArea(-1) == 80 ? 2 : 0));
          if (this.getMineArea(-1) == 121)
            objectIndex = 749;
          if (who.professions.Contains(19) && r.NextDouble() < 0.5)
            Game1.createObjectDebris(objectIndex, x, y, who.uniqueMultiplayerID, (GameLocation) this);
          Game1.createObjectDebris(objectIndex, x, y, who.uniqueMultiplayerID, (GameLocation) this);
          who.gainExperience(5, 20 * this.getMineArea(-1));
        }
        if (this.mineLevel > 20 && r.NextDouble() < 0.005 * (1.0 + num1) * (who.professions.Contains(22) ? 2.0 : 1.0))
        {
          if (who.professions.Contains(19) && r.NextDouble() < 0.5)
            Game1.createObjectDebris(749, x, y, who.uniqueMultiplayerID, (GameLocation) this);
          Game1.createObjectDebris(749, x, y, who.uniqueMultiplayerID, (GameLocation) this);
          who.gainExperience(5, 40 * this.getMineArea(-1));
        }
        if (r.NextDouble() < 0.05 * (1.0 + num1) * num2)
        {
          r.Next(1, 3);
          r.NextDouble();
          double num4 = 0.1 * (1.0 + num1);
          if (r.NextDouble() < 0.25)
          {
            Game1.createObjectDebris(382, x, y, who.uniqueMultiplayerID, (GameLocation) this);
            this.temporarySprites.Add(new TemporaryAnimatedSprite(25, new Vector2((float) (Game1.tileSize * x), (float) (Game1.tileSize * y)), Color.White, 8, Game1.random.NextDouble() < 0.5, 80f, 0, -1, -1f, Game1.tileSize * 2, 0));
          }
          else
            Game1.createObjectDebris(this.getOreIndexForLevel(this.mineLevel, r), x, y, who.uniqueMultiplayerID, (GameLocation) this);
          who.gainExperience(3, 5);
        }
        else
        {
          if (r.NextDouble() >= 0.5)
            return;
          Game1.createDebris(14, x, y, 1, (GameLocation) this);
        }
      }
    }

    public int getOreIndexForLevel(int mineLevel, Random r)
    {
      if (mineLevel < 40)
        return mineLevel >= 20 && r.NextDouble() < 0.1 ? 380 : 378;
      if (mineLevel < 80)
      {
        if (mineLevel >= 60 && r.NextDouble() < 0.1)
          return 384;
        return r.NextDouble() >= 0.75 ? 378 : 380;
      }
      if (mineLevel < 120)
      {
        if (r.NextDouble() < 0.75)
          return 384;
        return r.NextDouble() >= 0.75 ? 378 : 380;
      }
      if (r.NextDouble() < 0.01 + (double) (mineLevel - 120) / 2000.0)
        return 386;
      if (r.NextDouble() < 0.75)
        return 384;
      return r.NextDouble() >= 0.75 ? 378 : 380;
    }

    public int getMineArea(int level = -1)
    {
      if (level == -1)
        level = this.mineLevel;
      if (level >= 80 && level <= 120)
        return 80;
      if (level > 120)
        return 121;
      if (level >= 40)
        return 40;
      return level > 10 && this.mineLevel < 30 ? 10 : 0;
    }

    public byte getWallAt(int x, int y)
    {
      return byte.MaxValue;
    }

    public Color getLightingColor(GameTime time)
    {
      return this.lighting;
    }

    public StardewValley.Object getRandomItemForThisLevel(int level)
    {
      int parentSheetIndex = 0;
      if (this.mineRandom.NextDouble() < 0.05 && level > 80)
        parentSheetIndex = 422;
      else if (this.mineRandom.NextDouble() < 0.1 && level > 20 && this.getMineArea(-1) != 40)
        parentSheetIndex = 420;
      else if (this.mineRandom.NextDouble() < 0.25)
      {
        switch (this.getMineArea(-1))
        {
          case 40:
            parentSheetIndex = 84;
            break;
          case 80:
            parentSheetIndex = 82;
            break;
          case 121:
            parentSheetIndex = this.mineRandom.NextDouble() < 0.3 ? 86 : (this.mineRandom.NextDouble() < 0.3 ? 84 : 82);
            break;
          case 0:
          case 10:
            parentSheetIndex = 86;
            break;
        }
      }
      else
        parentSheetIndex = 80;
      return new StardewValley.Object(parentSheetIndex, 1, false, -1, 0)
      {
        isSpawnedObject = true
      };
    }

    public int getRandomGemRichStoneForThisLevel(int level)
    {
      int num1 = this.mineRandom.Next(59, 70);
      int num2 = num1 + num1 % 2;
      if (Game1.player.timesReachedMineBottom == 0)
      {
        if (level < 40 && num2 != 66 && num2 != 68)
          num2 = this.mineRandom.NextDouble() < 0.5 ? 66 : 68;
        else if (level < 80 && (num2 == 64 || num2 == 60))
          num2 = this.mineRandom.NextDouble() < 0.5 ? (this.mineRandom.NextDouble() < 0.5 ? 66 : 70) : (this.mineRandom.NextDouble() < 0.5 ? 68 : 62);
      }
      switch (num2)
      {
        case 60:
          return 12;
        case 62:
          return 14;
        case 64:
          return 4;
        case 66:
          return 8;
        case 68:
          return 10;
        case 70:
          return 6;
        default:
          return 40;
      }
    }

    public Monster getMonsterForThisLevel(int level, int xTile, int yTile)
    {
      Vector2 vector2 = new Vector2((float) xTile, (float) yTile) * (float) Game1.tileSize;
      float num = Utility.distance((float) xTile, this.tileBeneathLadder.X, (float) yTile, this.tileBeneathLadder.Y);
      if (this.isSlimeArea)
      {
        if (this.mineRandom.NextDouble() < 0.2)
          return (Monster) new BigSlime(vector2, this.getMineArea(-1));
        return (Monster) new GreenSlime(vector2, this.mineLevel);
      }
      if (level < 40)
      {
        if (this.mineRandom.NextDouble() < 0.25 && !this.mustKillAllMonstersToAdvance())
          return (Monster) new Bug(vector2, this.mineRandom.Next(4));
        if (level < 15)
        {
          if (this.doesTileHaveProperty(xTile, yTile, "Diggable", "Back") != null)
            return (Monster) new Duggy(vector2);
          if (this.mineRandom.NextDouble() < 0.15)
            return (Monster) new RockCrab(vector2);
          return (Monster) new GreenSlime(vector2, level);
        }
        if (level <= 30)
        {
          if (this.doesTileHaveProperty(xTile, yTile, "Diggable", "Back") != null)
            return (Monster) new Duggy(vector2);
          if (this.mineRandom.NextDouble() < 0.15)
            return (Monster) new RockCrab(vector2);
          if (this.mineRandom.NextDouble() < 0.05 && (double) num > 10.0)
            return (Monster) new Fly(vector2, false);
          if (this.mineRandom.NextDouble() < 0.45)
            return (Monster) new GreenSlime(vector2, level);
          return (Monster) new Grub(vector2, false);
        }
        if (level <= 40)
        {
          if (this.mineRandom.NextDouble() < 0.1 && (double) num > 10.0)
            return (Monster) new Bat(vector2, level);
          return (Monster) new RockGolem(vector2);
        }
      }
      else if (this.getMineArea(-1) == 40)
      {
        if (this.mineLevel >= 70 && this.mineRandom.NextDouble() < 0.75)
          return (Monster) new Skeleton(vector2);
        if (this.mineRandom.NextDouble() < 0.3)
          return (Monster) new DustSpirit(vector2, this.mineRandom.NextDouble() < 0.8);
        if (this.mineRandom.NextDouble() < 0.3 && (double) num > 10.0)
          return (Monster) new Bat(vector2, this.mineLevel);
        if (!this.ghostAdded && this.mineLevel > 50 && (this.mineRandom.NextDouble() < 0.3 && (double) num > 10.0))
        {
          this.ghostAdded = true;
          return (Monster) new Ghost(vector2);
        }
      }
      else if (this.getMineArea(-1) == 80)
      {
        if (this.isDarkArea() && this.mineRandom.NextDouble() < 0.25)
          return (Monster) new Bat(vector2, this.mineLevel);
        if (this.mineRandom.NextDouble() < 0.15)
          return (Monster) new GreenSlime(vector2, this.getMineArea(-1));
        if (this.mineRandom.NextDouble() < 0.15)
          return (Monster) new MetalHead(vector2, this.getMineArea(-1));
        if (this.mineRandom.NextDouble() < 0.25)
          return (Monster) new ShadowBrute(vector2);
        if (this.mineRandom.NextDouble() < 0.25)
          return (Monster) new ShadowShaman(vector2);
        if (this.mineRandom.NextDouble() < 0.25)
          return (Monster) new RockCrab(vector2, "Lava Crab");
        if (this.mineRandom.NextDouble() < 0.2 && (double) num > 8.0 && this.mineLevel >= 90)
          return (Monster) new SquidKid(vector2);
      }
      else if (this.getMineArea(-1) == 121)
      {
        if (this.loadedDarkArea)
          return (Monster) new Mummy(vector2);
        if (this.mineLevel % 20 == 0 && (double) num > 10.0)
          return (Monster) new Bat(vector2, this.mineLevel);
        if (this.mineLevel % 16 == 0 && !this.mustKillAllMonstersToAdvance())
          return (Monster) new Bug(vector2, this.mineRandom.Next(4));
        if (this.mineRandom.NextDouble() < 0.33 && (double) num > 10.0)
          return (Monster) new Serpent(vector2);
        if (this.mineRandom.NextDouble() < 0.33 && !this.mustKillAllMonstersToAdvance())
          return (Monster) new Bug(vector2, this.mineRandom.Next(4));
        if (this.mineRandom.NextDouble() < 0.25)
          return (Monster) new GreenSlime(vector2, level);
        return (Monster) new BigSlime(vector2);
      }
      return (Monster) new GreenSlime(vector2, level);
    }

    public Color getCrystalColorForThisLevel()
    {
      Random random = new Random(this.mineLevel + Game1.player.timesReachedMineBottom);
      if (random.NextDouble() < 0.04 && this.mineLevel < 80)
      {
        Color color;
        for (color = new Color(this.mineRandom.Next(256), this.mineRandom.Next(256), this.mineRandom.Next(256)); (int) color.R + (int) color.G + (int) color.B < 500; color.B = (byte) Math.Min((int) byte.MaxValue, (int) color.B + 10))
        {
          color.R = (byte) Math.Min((int) byte.MaxValue, (int) color.R + 10);
          color.G = (byte) Math.Min((int) byte.MaxValue, (int) color.G + 10);
        }
        return color;
      }
      if (random.NextDouble() < 0.07)
        return new Color((int) byte.MaxValue - this.mineRandom.Next(20), (int) byte.MaxValue - this.mineRandom.Next(20), (int) byte.MaxValue - this.mineRandom.Next(20));
      if (this.mineLevel < 40)
      {
        switch (this.mineRandom.Next(2))
        {
          case 0:
            return new Color(58, 145, 72);
          case 1:
            return new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
        }
      }
      else if (this.mineLevel < 80)
      {
        switch (this.mineRandom.Next(4))
        {
          case 0:
            return new Color(120, 0, 210);
          case 1:
            return new Color(0, 100, 170);
          case 2:
            return new Color(0, 220, (int) byte.MaxValue);
          case 3:
            return new Color(0, (int) byte.MaxValue, 220);
        }
      }
      else
      {
        switch (this.mineRandom.Next(2))
        {
          case 0:
            return new Color(200, 100, 0);
          case 1:
            return new Color(220, 60, 0);
        }
      }
      return Color.White;
    }

    private StardewValley.Object chooseStoneType(double chanceForPurpleStone, double chanceForMysticStone, double gemStoneChance, Vector2 tile)
    {
      int num1 = 1;
      int num2;
      if (this.mineLevel < 40)
      {
        num2 = this.mineRandom.Next(31, 42);
        if (this.mineLevel % 40 < 30 && num2 >= 33 && num2 < 38)
          num2 = this.mineRandom.NextDouble() < 0.5 ? 32 : 38;
        else if (this.mineLevel % 40 >= 30)
          num2 = this.mineRandom.NextDouble() < 0.5 ? 34 : 36;
        if (this.mineLevel != 1 && this.mineLevel % 5 != 0 && this.mineRandom.NextDouble() < 0.029)
          return new StardewValley.Object(tile, 751, "Stone", true, false, false, false)
          {
            minutesUntilReady = 3
          };
      }
      else if (this.mineLevel < 80)
      {
        num2 = this.mineRandom.Next(47, 54);
        num1 = 3;
        if (this.mineLevel % 5 != 0 && this.mineRandom.NextDouble() < 0.029)
          return new StardewValley.Object(tile, 290, "Stone", true, false, false, false)
          {
            minutesUntilReady = 4
          };
      }
      else if (this.mineLevel < 120)
      {
        num1 = 4;
        num2 = this.mineRandom.NextDouble() >= 0.3 || this.isDarkArea() ? (this.mineRandom.NextDouble() >= 0.3 ? (this.mineRandom.NextDouble() >= 0.5 ? 762 : 760) : this.mineRandom.Next(55, 58)) : (this.mineRandom.NextDouble() >= 0.5 ? 32 : 38);
        if (this.mineLevel % 5 != 0 && this.mineRandom.NextDouble() < 0.029)
          return new StardewValley.Object(tile, 764, "Stone", true, false, false, false)
          {
            minutesUntilReady = 8
          };
      }
      else
      {
        num1 = 5;
        num2 = this.mineRandom.NextDouble() >= 0.5 ? (this.mineRandom.NextDouble() >= 0.5 ? 42 : 40) : (this.mineRandom.NextDouble() >= 0.5 ? 32 : 38);
        double num3 = 0.02 + (double) (this.mineLevel - 120) * 0.0005;
        if (this.mineLevel >= 130)
          num3 += 0.01 * ((double) (this.mineLevel - 120 - 10) / 10.0);
        double val1 = 0.0;
        if (this.mineLevel >= 130)
          val1 += 0.001 * ((double) (this.mineLevel - 120 - 10) / 10.0);
        double num4 = Math.Min(val1, 0.004);
        if (this.mineLevel % 5 != 0 && this.mineRandom.NextDouble() < num3)
        {
          double num5 = (double) (this.mineLevel - 120) * (0.0003 + num4);
          double num6 = 0.01 + (double) (this.mineLevel - 120) * 0.0005;
          double num7 = Math.Min(0.5, 0.1 + (double) (this.mineLevel - 120) * 0.005);
          if (this.mineRandom.NextDouble() < num5)
            return new StardewValley.Object(tile, 765, "Stone", true, false, false, false)
            {
              minutesUntilReady = 16
            };
          if (this.mineRandom.NextDouble() < num6)
            return new StardewValley.Object(tile, 764, "Stone", true, false, false, false)
            {
              minutesUntilReady = 8
            };
          if (this.mineRandom.NextDouble() < num7)
            return new StardewValley.Object(tile, 290, "Stone", true, false, false, false)
            {
              minutesUntilReady = 4
            };
          return new StardewValley.Object(tile, 751, "Stone", true, false, false, false)
          {
            minutesUntilReady = 2
          };
        }
      }
      double num8 = Game1.dailyLuck / 2.0 + (double) Game1.player.MiningLevel * 0.005;
      if (this.mineLevel > 50 && this.mineRandom.NextDouble() < 0.00025 + (double) this.mineLevel / 120000.0 + 0.0005 * num8 / 2.0)
      {
        num2 = 2;
        num1 = 10;
      }
      else if (gemStoneChance != 0.0 && this.mineRandom.NextDouble() < gemStoneChance + gemStoneChance * num8 + (double) this.mineLevel / 24000.0)
        return new StardewValley.Object(tile, this.getRandomGemRichStoneForThisLevel(this.mineLevel), "Stone", true, false, false, false)
        {
          minutesUntilReady = 5
        };
      if (this.mineRandom.NextDouble() < chanceForPurpleStone / 2.0 + chanceForPurpleStone * (double) Game1.player.MiningLevel * 0.008 + chanceForPurpleStone * (Game1.dailyLuck / 2.0))
        num2 = 44;
      if (this.mineLevel > 100 && this.mineRandom.NextDouble() < chanceForMysticStone + chanceForMysticStone * (double) Game1.player.MiningLevel * 0.008 + chanceForMysticStone * (Game1.dailyLuck / 2.0))
        num2 = 46;
      int parentSheetIndex = num2 + num2 % 2;
      if (this.mineRandom.NextDouble() < 0.1 && this.getMineArea(-1) != 40)
      {
        StardewValley.Object @object = new StardewValley.Object(tile, this.mineRandom.NextDouble() < 0.5 ? 668 : 670, "Stone", true, false, false, false);
        @object.minutesUntilReady = 2;
        int num3 = this.mineRandom.NextDouble() < 0.5 ? 1 : 0;
        @object.flipped = num3 != 0;
        return @object;
      }
      return new StardewValley.Object(tile, parentSheetIndex, "Stone", true, false, false, false)
      {
        minutesUntilReady = num1
      };
    }

    public override void draw(SpriteBatch b)
    {
      foreach (ResourceClump resourceClump in this.resourceClumps)
        resourceClump.draw(b, resourceClump.tile);
      base.draw(b);
    }
  }
}
