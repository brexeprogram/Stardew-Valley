// Decompiled with JetBrains decompiler
// Type: StardewValley.Farm
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using StardewValley.Buildings;
using StardewValley.Characters;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Monsters;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using xTile;
using xTile.Dimensions;

namespace StardewValley
{
  public class Farm : BuildableGameLocation
  {
    public SerializableDictionary<long, FarmAnimal> animals = new SerializableDictionary<long, FarmAnimal>();
    [XmlIgnore]
    public Texture2D houseTextures = Game1.content.Load<Texture2D>("Buildings\\houses");
    public List<ResourceClump> resourceClumps = new List<ResourceClump>();
    private Microsoft.Xna.Framework.Rectangle shippingBinLidOpenArea = new Microsoft.Xna.Framework.Rectangle(70 * Game1.tileSize, 13 * Game1.tileSize, Game1.tileSize * 4, Game1.tileSize * 3);
    [XmlIgnore]
    public List<Item> shippingBin = new List<Item>();
    private int chimneyTimer = 500;
    private List<long> animalsToRemove = new List<long>();
    public const int default_layout = 0;
    public const int riverlands_layout = 1;
    public const int forest_layout = 2;
    public const int mountains_layout = 3;
    public const int combat_layout = 4;
    public int piecesOfHay;
    public int grandpaScore;
    private TemporaryAnimatedSprite shippingBinLid;
    [XmlIgnore]
    public Item lastItemShipped;
    public bool hasSeenGrandpaNote;
    public const int numCropsForCrow = 16;
    private Microsoft.Xna.Framework.Rectangle houseSource;
    private Microsoft.Xna.Framework.Rectangle greenhouseSource;

    public Farm()
    {
    }

    public Farm(Map m, string name)
      : base(m, name)
    {
    }

    public static string getMapNameFromTypeInt(int type)
    {
      switch (type)
      {
        case 0:
          return nameof (Farm);
        case 1:
          return "Farm_Fishing";
        case 2:
          return "Farm_Foraging";
        case 3:
          return "Farm_Mining";
        case 4:
          return "Farm_Combat";
        default:
          return nameof (Farm);
      }
    }

    public void setMap(int which)
    {
      this.map = Game1.game1.xTileContent.Load<Map>("Maps\\" + Farm.getMapNameFromTypeInt(which));
    }

    public override void DayUpdate(int dayOfMonth)
    {
      base.DayUpdate(dayOfMonth);
      if (Game1.whichFarm == 4 && !Game1.player.mailReceived.Contains("henchmanGone"))
        Game1.spawnMonstersAtNight = true;
      this.lastItemShipped = (Item) null;
      for (int index = this.animals.Count - 1; index >= 0; --index)
        this.animals.ElementAt<KeyValuePair<long, FarmAnimal>>(index).Value.dayUpdate((GameLocation) this);
      for (int index = this.characters.Count - 1; index >= 0; --index)
      {
        if (this.characters[index] is JunimoHarvester)
          this.characters.RemoveAt(index);
      }
      for (int index = this.characters.Count - 1; index >= 0; --index)
      {
        if (this.characters[index] is Monster && (this.characters[index] as Monster).wildernessFarmMonster)
          this.characters.RemoveAt(index);
      }
      if (this.characters.Count > 5)
      {
        int num = 0;
        for (int index = this.characters.Count - 1; index >= 0; --index)
        {
          if (this.characters[index] is GreenSlime && Game1.random.NextDouble() < 0.035)
          {
            this.characters.RemoveAt(index);
            ++num;
          }
        }
        if (num > 0)
          Game1.showGlobalMessage(Game1.content.LoadString(num == 1 ? "Strings\\Locations:Farm_1SlimeEscaped" : "Strings\\Locations:Farm_NSlimesEscaped", (object) num));
      }
      if (Game1.whichFarm == 2)
      {
        for (int x = 0; x < 20; ++x)
        {
          for (int y = 0; y < this.map.Layers[0].LayerHeight; ++y)
          {
            if (this.map.GetLayer("Paths").Tiles[x, y] != null && this.map.GetLayer("Paths").Tiles[x, y].TileIndex == 21 && (this.isTileLocationTotallyClearAndPlaceable(x, y) && this.isTileLocationTotallyClearAndPlaceable(x + 1, y)) && (this.isTileLocationTotallyClearAndPlaceable(x + 1, y + 1) && this.isTileLocationTotallyClearAndPlaceable(x, y + 1)))
              this.resourceClumps.Add(new ResourceClump(600, 2, 2, new Vector2((float) x, (float) y)));
          }
        }
        if (!Game1.IsWinter)
        {
          while (Game1.random.NextDouble() < 0.75)
          {
            Vector2 vector2 = new Vector2((float) Game1.random.Next(18), (float) Game1.random.Next(this.map.Layers[0].LayerHeight));
            if (Game1.random.NextDouble() < 0.5)
              vector2 = this.getRandomTile();
            if (this.isTileLocationTotallyClearAndPlaceable(vector2) && this.getTileIndexAt((int) vector2.X, (int) vector2.Y, "AlwaysFront") == -1 && ((double) vector2.X < 18.0 || this.doesTileHavePropertyNoNull((int) vector2.X, (int) vector2.Y, "Type", "Back").Equals("Grass")))
            {
              int parentSheetIndex = 792;
              string currentSeason = Game1.currentSeason;
              if (!(currentSeason == "spring"))
              {
                if (!(currentSeason == "summer"))
                {
                  if (currentSeason == "fall")
                  {
                    switch (Game1.random.Next(4))
                    {
                      case 0:
                        parentSheetIndex = 281;
                        break;
                      case 1:
                        parentSheetIndex = 420;
                        break;
                      case 2:
                        parentSheetIndex = 422;
                        break;
                      case 3:
                        parentSheetIndex = 404;
                        break;
                    }
                  }
                }
                else
                {
                  switch (Game1.random.Next(4))
                  {
                    case 0:
                      parentSheetIndex = 402;
                      break;
                    case 1:
                      parentSheetIndex = 396;
                      break;
                    case 2:
                      parentSheetIndex = 398;
                      break;
                    case 3:
                      parentSheetIndex = 404;
                      break;
                  }
                }
              }
              else
              {
                switch (Game1.random.Next(4))
                {
                  case 0:
                    parentSheetIndex = 16;
                    break;
                  case 1:
                    parentSheetIndex = 22;
                    break;
                  case 2:
                    parentSheetIndex = 20;
                    break;
                  case 3:
                    parentSheetIndex = 257;
                    break;
                }
              }
              this.dropObject(new Object(vector2, parentSheetIndex, (string) null, false, true, false, true), vector2 * (float) Game1.tileSize, Game1.viewport, true, (Farmer) null);
            }
          }
          if (this.objects.Count > 0)
          {
            for (int index = 0; index < 6; ++index)
            {
              Object @object = this.objects.ElementAt<KeyValuePair<Vector2, Object>>(Game1.random.Next(this.objects.Count)).Value;
              if (@object.name.Equals("Weeds"))
                @object.parentSheetIndex = 792 + Utility.getSeasonNumber(Game1.currentSeason);
            }
          }
        }
      }
      if (Game1.whichFarm == 3)
        this.doDailyMountainFarmUpdate();
      Dictionary<Vector2, TerrainFeature>.KeyCollection keys = this.terrainFeatures.Keys;
      for (int index = keys.Count - 1; index >= 0; --index)
      {
        if (this.terrainFeatures[keys.ElementAt<Vector2>(index)] is HoeDirt && (this.terrainFeatures[keys.ElementAt<Vector2>(index)] as HoeDirt).crop == null && Game1.random.NextDouble() <= 0.1)
          this.terrainFeatures.Remove(keys.ElementAt<Vector2>(index));
      }
      if (this.terrainFeatures.Count > 0 && Game1.currentSeason.Equals("fall") && (Game1.dayOfMonth > 1 && Game1.random.NextDouble() < 0.05))
      {
        for (int index = 0; index < 10; ++index)
        {
          TerrainFeature terrainFeature = this.terrainFeatures.ElementAt<KeyValuePair<Vector2, TerrainFeature>>(Game1.random.Next(this.terrainFeatures.Count)).Value;
          if (terrainFeature is Tree && (terrainFeature as Tree).growthStage >= 5 && !(terrainFeature as Tree).tapped)
          {
            (terrainFeature as Tree).treeType = 7;
            (terrainFeature as Tree).loadSprite();
            break;
          }
        }
      }
      this.addCrows();
      if (!Game1.currentSeason.Equals("winter"))
        this.spawnWeedsAndStones(Game1.currentSeason.Equals("summer") ? 30 : 20, false, true);
      this.spawnWeeds(false);
      if (dayOfMonth == 1)
      {
        for (int index = this.terrainFeatures.Count - 1; index >= 0; --index)
        {
          KeyValuePair<Vector2, TerrainFeature> keyValuePair = this.terrainFeatures.ElementAt<KeyValuePair<Vector2, TerrainFeature>>(index);
          if (keyValuePair.Value is HoeDirt)
          {
            keyValuePair = this.terrainFeatures.ElementAt<KeyValuePair<Vector2, TerrainFeature>>(index);
            if ((keyValuePair.Value as HoeDirt).crop == null && Game1.random.NextDouble() < 0.8)
            {
              SerializableDictionary<Vector2, TerrainFeature> terrainFeatures = this.terrainFeatures;
              keyValuePair = this.terrainFeatures.ElementAt<KeyValuePair<Vector2, TerrainFeature>>(index);
              Vector2 key = keyValuePair.Key;
              terrainFeatures.Remove(key);
            }
          }
        }
        this.spawnWeedsAndStones(20, false, false);
        if (Game1.currentSeason.Equals("spring") && Game1.stats.DaysPlayed > 1U)
        {
          this.spawnWeedsAndStones(40, false, false);
          this.spawnWeedsAndStones(40, true, false);
          for (int index = 0; index < 15; ++index)
          {
            int xTile = Game1.random.Next(this.map.DisplayWidth / Game1.tileSize);
            int yTile = Game1.random.Next(this.map.DisplayHeight / Game1.tileSize);
            Vector2 vector2 = new Vector2((float) xTile, (float) yTile);
            Object @object;
            this.objects.TryGetValue(vector2, out @object);
            if (@object == null && this.doesTileHaveProperty(xTile, yTile, "Diggable", "Back") != null && (this.isTileLocationOpen(new Location(xTile * Game1.tileSize, yTile * Game1.tileSize)) && !this.isTileOccupied(vector2, "")) && this.doesTileHaveProperty(xTile, yTile, "Water", "Back") == null)
              this.terrainFeatures.Add(vector2, (TerrainFeature) new Grass(1, 4));
          }
          this.growWeedGrass(40);
        }
      }
      this.growWeedGrass(1);
    }

    public void doDailyMountainFarmUpdate()
    {
      double num1 = 1.0;
      while (Game1.random.NextDouble() < num1)
      {
        Vector2 positionInThisRectangle = Utility.getRandomPositionInThisRectangle(new Microsoft.Xna.Framework.Rectangle(5, 37, 22, 8), Game1.random);
        if (this.doesTileHavePropertyNoNull((int) positionInThisRectangle.X, (int) positionInThisRectangle.Y, "Type", "Back").Equals("Dirt") && this.isTileLocationTotallyClearAndPlaceable(positionInThisRectangle))
        {
          int parentSheetIndex = 668;
          int num2 = 2;
          if (Game1.random.NextDouble() < 0.15)
          {
            this.objects.Add(positionInThisRectangle, new Object(positionInThisRectangle, 590, 1));
            continue;
          }
          if (Game1.random.NextDouble() < 0.5)
            parentSheetIndex = 670;
          if (Game1.random.NextDouble() < 0.1)
          {
            if (Game1.player.MiningLevel >= 8 && Game1.random.NextDouble() < 0.33)
            {
              parentSheetIndex = 77;
              num2 = 7;
            }
            else if (Game1.player.MiningLevel >= 5 && Game1.random.NextDouble() < 0.5)
            {
              parentSheetIndex = 76;
              num2 = 5;
            }
            else
            {
              parentSheetIndex = 75;
              num2 = 3;
            }
          }
          if (Game1.random.NextDouble() < 0.21)
          {
            parentSheetIndex = 751;
            num2 = 3;
          }
          if (Game1.player.MiningLevel >= 4 && Game1.random.NextDouble() < 0.15)
          {
            parentSheetIndex = 290;
            num2 = 4;
          }
          if (Game1.player.MiningLevel >= 7 && Game1.random.NextDouble() < 0.1)
          {
            parentSheetIndex = 764;
            num2 = 8;
          }
          if (Game1.player.MiningLevel >= 10 && Game1.random.NextDouble() < 0.01)
          {
            parentSheetIndex = 765;
            num2 = 16;
          }
          this.objects.Add(positionInThisRectangle, new Object(positionInThisRectangle, parentSheetIndex, 10)
          {
            minutesUntilReady = num2
          });
        }
        num1 *= 0.66;
      }
    }

    public void addCrows()
    {
      int num1 = 0;
      foreach (KeyValuePair<Vector2, TerrainFeature> terrainFeature in (Dictionary<Vector2, TerrainFeature>) this.terrainFeatures)
      {
        if (terrainFeature.Value is HoeDirt && (terrainFeature.Value as HoeDirt).crop != null)
          ++num1;
      }
      List<Vector2> vector2List = new List<Vector2>();
      foreach (KeyValuePair<Vector2, Object> keyValuePair in (Dictionary<Vector2, Object>) this.objects)
      {
        if (keyValuePair.Value.bigCraftable && keyValuePair.Value.Name.Contains("arecrow"))
          vector2List.Add(keyValuePair.Key);
      }
      int num2 = Math.Min(4, num1 / 16);
      for (int index1 = 0; index1 < num2; ++index1)
      {
        if (Game1.random.NextDouble() < 0.3)
        {
          for (int index2 = 0; index2 < 10; ++index2)
          {
            Vector2 key = this.terrainFeatures.ElementAt<KeyValuePair<Vector2, TerrainFeature>>(Game1.random.Next(this.terrainFeatures.Count)).Key;
            if (this.terrainFeatures[key] is HoeDirt && (this.terrainFeatures[key] as HoeDirt).crop != null && (this.terrainFeatures[key] as HoeDirt).crop.currentPhase > 1)
            {
              bool flag = false;
              foreach (Vector2 index3 in vector2List)
              {
                if ((double) Vector2.Distance(index3, key) < 9.0)
                {
                  flag = true;
                  ++this.objects[index3].specialVariable;
                  break;
                }
              }
              if (!flag)
              {
                (this.terrainFeatures[key] as HoeDirt).crop = (Crop) null;
                this.critters.Add((Critter) new StardewValley.BellsAndWhistles.Crow((int) key.X, (int) key.Y));
                break;
              }
              break;
            }
          }
        }
      }
    }

    public static Point getFrontDoorPositionForFarmer(Farmer who)
    {
      if (Utility.getFarmerNumberFromFarmer(who) == 1)
        return new Point(64, 14);
      throw new Exception();
    }

    public override void performTenMinuteUpdate(int timeOfDay)
    {
      base.performTenMinuteUpdate(timeOfDay);
      if (timeOfDay == 1300)
      {
        foreach (NPC character in this.characters)
        {
          if (character.isMarried())
            character.returnHomeFromFarmPosition(this);
        }
      }
      foreach (NPC character in this.characters)
      {
        if (character.isMarried())
          character.checkForMarriageDialogue(timeOfDay, (GameLocation) this);
        if (character is Child)
          (character as Child).tenMinuteUpdate();
      }
      if (!Game1.spawnMonstersAtNight || Game1.farmEvent != null || (Game1.timeOfDay < 1900 || Game1.random.NextDouble() >= 0.25 - Game1.dailyLuck / 2.0))
        return;
      if (Game1.random.NextDouble() < 0.25)
      {
        if (!this.Equals((object) Game1.currentLocation))
          return;
        this.spawnFlyingMonstersOffScreen();
      }
      else
        this.spawnGroundMonsterOffScreen();
    }

    public void spawnGroundMonsterOffScreen()
    {
      for (int index = 0; index < 15; ++index)
      {
        Vector2 zero = Vector2.Zero;
        Vector2 randomTile = this.getRandomTile();
        if (Utility.isOnScreen(Utility.Vector2ToPoint(randomTile), Game1.tileSize, (GameLocation) this))
          randomTile.X -= (float) (Game1.viewport.Width / Game1.tileSize);
        if (this.isTileLocationTotallyClearAndPlaceable(randomTile))
        {
          bool flag;
          if (Game1.player.CombatLevel >= 8 && Game1.random.NextDouble() < 0.15)
          {
            List<NPC> characters = this.characters;
            ShadowBrute shadowBrute = new ShadowBrute(randomTile * (float) Game1.tileSize);
            int num1 = 1;
            shadowBrute.focusedOnFarmers = num1 != 0;
            int num2 = 1;
            shadowBrute.wildernessFarmMonster = num2 != 0;
            characters.Add((NPC) shadowBrute);
            flag = true;
          }
          else if (Game1.random.NextDouble() < 0.65 && this.isTileLocationTotallyClearAndPlaceable(randomTile))
          {
            List<NPC> characters = this.characters;
            RockGolem rockGolem = new RockGolem(randomTile * (float) Game1.tileSize, Game1.player.CombatLevel);
            int num = 1;
            rockGolem.wildernessFarmMonster = num != 0;
            characters.Add((NPC) rockGolem);
            flag = true;
          }
          else
          {
            int mineLevel = 1;
            if (Game1.player.CombatLevel >= 10)
              mineLevel = 140;
            else if (Game1.player.CombatLevel >= 8)
              mineLevel = 100;
            else if (Game1.player.CombatLevel >= 4)
              mineLevel = 41;
            List<NPC> characters = this.characters;
            GreenSlime greenSlime = new GreenSlime(randomTile * (float) Game1.tileSize, mineLevel);
            int num = 1;
            greenSlime.wildernessFarmMonster = num != 0;
            characters.Add((NPC) greenSlime);
            flag = true;
          }
          if (!flag || !Game1.currentLocation.Equals((object) this))
            break;
          using (Dictionary<Vector2, Object>.Enumerator enumerator = this.objects.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              KeyValuePair<Vector2, Object> current = enumerator.Current;
              if (current.Value != null && current.Value.bigCraftable && current.Value.parentSheetIndex == 83)
              {
                current.Value.shakeTimer = 1000;
                current.Value.showNextIndex = true;
                Game1.currentLightSources.Add(new LightSource(4, current.Key * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), 0.0f), 1f, Color.Cyan * 0.75f, (int) ((double) current.Key.X * 797.0 + (double) current.Key.Y * 13.0 + 666.0)));
              }
            }
            break;
          }
        }
      }
    }

    public void spawnFlyingMonstersOffScreen()
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
        zero.X -= (float) Game1.viewport.Width;
      bool flag;
      if (Game1.player.CombatLevel >= 10 && Game1.random.NextDouble() < 0.25)
      {
        List<NPC> characters = this.characters;
        Serpent serpent = new Serpent(zero * (float) Game1.tileSize);
        int num1 = 1;
        serpent.focusedOnFarmers = num1 != 0;
        int num2 = 1;
        serpent.wildernessFarmMonster = num2 != 0;
        characters.Add((NPC) serpent);
        flag = true;
      }
      else if (Game1.player.CombatLevel >= 8 && Game1.random.NextDouble() < 0.5)
      {
        List<NPC> characters = this.characters;
        Bat bat = new Bat(zero * (float) Game1.tileSize, 81);
        int num1 = 1;
        bat.focusedOnFarmers = num1 != 0;
        int num2 = 1;
        bat.wildernessFarmMonster = num2 != 0;
        characters.Add((NPC) bat);
        flag = true;
      }
      else if (Game1.player.CombatLevel >= 5 && Game1.random.NextDouble() < 0.5)
      {
        List<NPC> characters = this.characters;
        Bat bat = new Bat(zero * (float) Game1.tileSize, 41);
        int num1 = 1;
        bat.focusedOnFarmers = num1 != 0;
        int num2 = 1;
        bat.wildernessFarmMonster = num2 != 0;
        characters.Add((NPC) bat);
        flag = true;
      }
      else
      {
        List<NPC> characters = this.characters;
        Bat bat = new Bat(zero * (float) Game1.tileSize, 1);
        int num1 = 1;
        bat.focusedOnFarmers = num1 != 0;
        int num2 = 1;
        bat.wildernessFarmMonster = num2 != 0;
        characters.Add((NPC) bat);
        flag = true;
      }
      if (!flag || !Game1.currentLocation.Equals((object) this))
        return;
      foreach (KeyValuePair<Vector2, Object> keyValuePair in (Dictionary<Vector2, Object>) this.objects)
      {
        if (keyValuePair.Value != null && keyValuePair.Value.bigCraftable && keyValuePair.Value.parentSheetIndex == 83)
        {
          keyValuePair.Value.shakeTimer = 1000;
          keyValuePair.Value.showNextIndex = true;
          Game1.currentLightSources.Add(new LightSource(4, keyValuePair.Key * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), 0.0f), 1f, Color.Cyan * 0.75f, (int) ((double) keyValuePair.Key.X * 797.0 + (double) keyValuePair.Key.Y * 13.0 + 666.0)));
        }
      }
    }

    public override bool performToolAction(Tool t, int tileX, int tileY)
    {
      Point point = new Point(tileX * Game1.tileSize + Game1.tileSize / 2, tileY * Game1.tileSize + Game1.tileSize / 2);
      for (int index = this.resourceClumps.Count - 1; index >= 0; --index)
      {
        if (this.resourceClumps[index].getBoundingBox(this.resourceClumps[index].tile).Contains(point) && this.resourceClumps[index].performToolAction(t, 1, this.resourceClumps[index].tile, (GameLocation) null))
          this.resourceClumps.RemoveAt(index);
      }
      if (t is MeleeWeapon)
      {
        foreach (FarmAnimal farmAnimal in this.animals.Values)
        {
          if (farmAnimal.GetBoundingBox().Intersects((t as MeleeWeapon).mostRecentArea))
            farmAnimal.hitWithWeapon(t as MeleeWeapon);
        }
      }
      if (t is WateringCan && (t as WateringCan).WaterLeft > 0 && this.getTileIndexAt(tileX, tileY, "Buildings") == 1938)
        this.setMapTileIndex(tileX, tileY, 1939, "Buildings", 0);
      return false;
    }

    public override void timeUpdate(int timeElapsed)
    {
      base.timeUpdate(timeElapsed);
      foreach (FarmAnimal farmAnimal in this.animals.Values)
        farmAnimal.updatePerTenMinutes(Game1.timeOfDay, (GameLocation) this);
      foreach (Building building in this.buildings)
      {
        if (building.indoors != null)
          building.indoors.performTenMinuteUpdate(timeElapsed);
        building.performTenMinuteAction(timeElapsed);
      }
    }

    public bool placeAnimal(BluePrint blueprint, Vector2 tileLocation, bool serverCommand, long ownerID)
    {
      for (int index1 = 0; index1 < blueprint.tilesHeight; ++index1)
      {
        for (int index2 = 0; index2 < blueprint.tilesWidth; ++index2)
        {
          Vector2 vector2 = new Vector2(tileLocation.X + (float) index2, tileLocation.Y + (float) index1);
          if (Game1.player.getTileLocation().Equals(vector2) || this.isTileOccupied(vector2, "") || !this.isTilePassable(new Location((int) vector2.X, (int) vector2.Y), Game1.viewport))
            return false;
        }
      }
      long num = 0;
      if (Game1.IsMultiplayer)
      {
        if (Game1.IsServer)
        {
          MultiplayerUtility.recentMultiplayerEntityID = MultiplayerUtility.getNewID();
          num = MultiplayerUtility.recentMultiplayerEntityID;
        }
        MultiplayerUtility.broadcastBuildingChange((byte) 0, tileLocation, blueprint.name, this.name, Game1.player.uniqueMultiplayerID);
        if (Game1.IsClient && !serverCommand)
          return false;
        if (Game1.IsClient & serverCommand)
          num = MultiplayerUtility.recentMultiplayerEntityID;
      }
      else
        num = MultiplayerUtility.getNewID();
      FarmAnimal farmAnimal = new FarmAnimal(blueprint.name, num, ownerID);
      farmAnimal.position = new Vector2((float) ((double) tileLocation.X * (double) Game1.tileSize + 4.0), (float) ((double) tileLocation.Y * (double) Game1.tileSize + (double) Game1.tileSize - (double) farmAnimal.sprite.getHeight() - 4.0));
      this.animals.Add(num, farmAnimal);
      if (farmAnimal.sound != null && !farmAnimal.sound.Equals(""))
        Game1.playSound(farmAnimal.sound);
      return true;
    }

    public int tryToAddHay(int num)
    {
      int num1 = Math.Min(Utility.numSilos() * 240 - this.piecesOfHay, num);
      this.piecesOfHay = this.piecesOfHay + num1;
      return num - num1;
    }

    public override bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer, int damagesFarmer, bool glider, Character character, bool pathfinding, bool projectile = false, bool ignoreCharacterRequirement = false)
    {
      if (!glider)
      {
        foreach (ResourceClump resourceClump in this.resourceClumps)
        {
          Vector2 tile = resourceClump.tile;
          if (resourceClump.getBoundingBox(tile).Intersects(position))
            return true;
        }
        foreach (KeyValuePair<long, FarmAnimal> animal in (Dictionary<long, FarmAnimal>) this.animals)
        {
          if (character != null && !character.Equals((object) animal.Value) && (!(character is FarmAnimal) && position.Intersects(animal.Value.GetBoundingBox())))
          {
            if (isFarmer)
            {
              if ((character as Farmer).temporaryImpassableTile.Intersects(position))
                break;
            }
            animal.Value.farmerPushing();
            return true;
          }
        }
      }
      return base.isCollidingPosition(position, viewport, isFarmer, damagesFarmer, glider, character, pathfinding, projectile, ignoreCharacterRequirement);
    }

    public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
    {
      Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle(tileLocation.X * Game1.tileSize, tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
      if (!this.objects.ContainsKey(new Vector2((float) tileLocation.X, (float) tileLocation.Y)))
      {
        foreach (KeyValuePair<long, FarmAnimal> animal in (Dictionary<long, FarmAnimal>) this.animals)
        {
          if (animal.Value.GetBoundingBox().Intersects(rectangle) && !animal.Value.wasPet)
          {
            animal.Value.pet(who);
            return true;
          }
        }
        foreach (KeyValuePair<long, FarmAnimal> animal in (Dictionary<long, FarmAnimal>) this.animals)
        {
          if (animal.Value.GetBoundingBox().Intersects(rectangle))
          {
            animal.Value.pet(who);
            return true;
          }
        }
      }
      foreach (ResourceClump resourceClump in this.resourceClumps)
      {
        if (resourceClump.getBoundingBox(resourceClump.tile).Intersects(rectangle))
        {
          resourceClump.performUseAction(new Vector2((float) tileLocation.X, (float) tileLocation.Y));
          return true;
        }
      }
      if (tileLocation.X >= 71 && tileLocation.X <= 72 && (tileLocation.Y >= 13 && tileLocation.Y <= 14))
      {
        ItemGrabMenu itemGrabMenu = new ItemGrabMenu((List<Item>) null, true, false, new InventoryMenu.highlightThisItem(Utility.highlightShippableObjects), new ItemGrabMenu.behaviorOnItemSelect(this.shipItem), "", (ItemGrabMenu.behaviorOnItemSelect) null, true, true, false, true, false, 0, (Item) null, -1, (object) null);
        itemGrabMenu.initializeUpperRightCloseButton();
        int num1 = 0;
        itemGrabMenu.setBackgroundTransparency(num1 != 0);
        int num2 = 1;
        itemGrabMenu.setDestroyItemOnClick(num2 != 0);
        itemGrabMenu.initializeShippingBin();
        Game1.activeClickableMenu = (IClickableMenu) itemGrabMenu;
        Game1.playSound("shwip");
        if (Game1.player.facingDirection == 1)
          Game1.player.Halt();
        Game1.player.showCarrying();
        return true;
      }
      switch (this.map.GetLayer("Buildings").Tiles[tileLocation] != null ? this.map.GetLayer("Buildings").Tiles[tileLocation].TileIndex : -1)
      {
        case 1956:
        case 1957:
        case 1958:
          if (!this.hasSeenGrandpaNote)
          {
            this.hasSeenGrandpaNote = true;
            Game1.activeClickableMenu = (IClickableMenu) new LetterViewerMenu(Game1.content.LoadString("Strings\\Locations:Farm_GrandpaNote", (object) Game1.player.name).Replace('\n', '^'));
            return true;
          }
          if (Game1.year >= 3 && this.grandpaScore > 0 && this.grandpaScore < 4)
          {
            if (who.ActiveObject != null && who.ActiveObject.parentSheetIndex == 72 && this.grandpaScore < 4)
            {
              who.reduceActiveItemByOne();
              this.grandpaScore = 0;
              Game1.playSound("stoneStep");
              Game1.playSound("fireball");
              DelayedAction.playSoundAfterDelay("yoba", 800);
              who.eventsSeen.Remove(558292);
              who.eventsSeen.Add(321777);
              who.freezePause = 1200;
              this.removeTemporarySpritesWithID(6666);
              DelayedAction.showDialogueAfterDelay(Game1.content.LoadString("Strings\\Locations:Farm_GrandpaShrine_PlaceDiamond"), 1200);
              return true;
            }
            if (who.ActiveObject == null || who.ActiveObject.parentSheetIndex != 72)
            {
              Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Farm_GrandpaShrine_DiamondSlot"));
              return true;
            }
            break;
          }
          if (this.grandpaScore >= 4 && !Utility.doesItemWithThisIndexExistAnywhere(160, true))
          {
            who.addItemByMenuIfNecessaryElseHoldUp((Item) new Object(Vector2.Zero, 160, false), new ItemGrabMenu.behaviorOnItemSelect(this.grandpaStatueCallback));
            return true;
          }
          if (this.grandpaScore == 0 && Game1.year >= 3)
          {
            Game1.player.eventsSeen.Remove(558292);
            if (!Game1.player.eventsSeen.Contains(321777))
            {
              Game1.player.eventsSeen.Add(321777);
              break;
            }
            break;
          }
          break;
      }
      return base.checkAction(tileLocation, viewport, who);
    }

    public void grandpaStatueCallback(Item item, Farmer who)
    {
      if (item == null || !(item is Object) || (!(item as Object).bigCraftable || (item as Object).parentSheetIndex != 160) || who == null)
        return;
      who.mailReceived.Add("grandpaPerfect");
    }

    private void shipItem(Item i, Farmer who)
    {
      if (i == null)
        return;
      this.shippingBin.Add(i);
      if (i is Object)
        this.showShipment(i as Object, false);
      this.lastItemShipped = i;
      who.removeItemFromInventory(i);
      if (Game1.player.ActiveObject != null)
        return;
      Game1.player.showNotCarrying();
      Game1.player.Halt();
    }

    public void shipItem(Item i)
    {
      this.shippingBin.Add(i);
    }

    public override bool leftClick(int x, int y, Farmer who)
    {
      if (who.ActiveObject == null || x / Game1.tileSize < 71 || (x / Game1.tileSize > 72 || y / Game1.tileSize < 13) || (y / Game1.tileSize > 14 || !who.ActiveObject.canBeShipped() || (double) Vector2.Distance(who.getTileLocation(), new Vector2(71.5f, 14f)) > 2.0))
        return base.leftClick(x, y, who);
      this.shippingBin.Add((Item) who.ActiveObject);
      this.lastItemShipped = (Item) who.ActiveObject;
      who.showNotCarrying();
      this.showShipment(who.ActiveObject, true);
      who.ActiveObject = (Object) null;
      return true;
    }

    public void showShipment(Object o, bool playThrowSound = true)
    {
      if (playThrowSound)
        Game1.playSound("backpackIN");
      DelayedAction.playSoundAfterDelay("Ship", playThrowSound ? 250 : 0);
      int num1 = Game1.random.Next();
      List<TemporaryAnimatedSprite> temporarySprites1 = this.temporarySprites;
      TemporaryAnimatedSprite temporaryAnimatedSprite1 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(524, 218, 34, 22), new Vector2(71f, 13f) * (float) Game1.tileSize + new Vector2(0.0f, 5f) * (float) Game1.pixelZoom, false, 0.0f, Color.White);
      temporaryAnimatedSprite1.interval = 100f;
      temporaryAnimatedSprite1.totalNumberOfLoops = 1;
      temporaryAnimatedSprite1.animationLength = 3;
      temporaryAnimatedSprite1.pingPong = true;
      double pixelZoom1 = (double) Game1.pixelZoom;
      temporaryAnimatedSprite1.scale = (float) pixelZoom1;
      double num2 = (double) (15 * Game1.tileSize) / 10000.0 + 9.99999974737875E-06;
      temporaryAnimatedSprite1.layerDepth = (float) num2;
      double num3 = (double) num1;
      temporaryAnimatedSprite1.id = (float) num3;
      int num4 = num1;
      temporaryAnimatedSprite1.extraInfoForEndBehavior = num4;
      TemporaryAnimatedSprite.endBehavior endBehavior = new TemporaryAnimatedSprite.endBehavior(((GameLocation) this).removeTemporarySpritesWithID);
      temporaryAnimatedSprite1.endFunction = endBehavior;
      temporarySprites1.Add(temporaryAnimatedSprite1);
      List<TemporaryAnimatedSprite> temporarySprites2 = this.temporarySprites;
      TemporaryAnimatedSprite temporaryAnimatedSprite2 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(524, 230, 34, 10), new Vector2(71f, 13f) * (float) Game1.tileSize + new Vector2(0.0f, 17f) * (float) Game1.pixelZoom, false, 0.0f, Color.White);
      temporaryAnimatedSprite2.interval = 100f;
      temporaryAnimatedSprite2.totalNumberOfLoops = 1;
      temporaryAnimatedSprite2.animationLength = 3;
      temporaryAnimatedSprite2.pingPong = true;
      double pixelZoom2 = (double) Game1.pixelZoom;
      temporaryAnimatedSprite2.scale = (float) pixelZoom2;
      double num5 = (double) (15 * Game1.tileSize) / 10000.0 + 0.000300000014249235;
      temporaryAnimatedSprite2.layerDepth = (float) num5;
      double num6 = (double) num1;
      temporaryAnimatedSprite2.id = (float) num6;
      int num7 = num1;
      temporaryAnimatedSprite2.extraInfoForEndBehavior = num7;
      temporarySprites2.Add(temporaryAnimatedSprite2);
      List<TemporaryAnimatedSprite> temporarySprites3 = this.temporarySprites;
      TemporaryAnimatedSprite temporaryAnimatedSprite3 = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, o.parentSheetIndex, 16, 16), new Vector2(71f, 13f) * (float) Game1.tileSize + new Vector2((float) (8 + Game1.random.Next(6)), 2f) * (float) Game1.pixelZoom, false, 0.0f, Color.White);
      temporaryAnimatedSprite3.interval = 9999f;
      double pixelZoom3 = (double) Game1.pixelZoom;
      temporaryAnimatedSprite3.scale = (float) pixelZoom3;
      double num8 = 0.0450000017881393;
      temporaryAnimatedSprite3.alphaFade = (float) num8;
      double num9 = (double) (15 * Game1.tileSize) / 10000.0 + 0.000224999996135011;
      temporaryAnimatedSprite3.layerDepth = (float) num9;
      Vector2 vector2_1 = new Vector2(0.0f, 0.3f);
      temporaryAnimatedSprite3.motion = vector2_1;
      Vector2 vector2_2 = new Vector2(0.0f, 0.2f);
      temporaryAnimatedSprite3.acceleration = vector2_2;
      double num10 = -0.0500000007450581;
      temporaryAnimatedSprite3.scaleChange = (float) num10;
      temporarySprites3.Add(temporaryAnimatedSprite3);
    }

    public override int getFishingLocation(Vector2 tile)
    {
      switch (Game1.whichFarm)
      {
        case 1:
        case 2:
          return 1;
        case 3:
          return 0;
        default:
          return -1;
      }
    }

    public override Object getFish(float millisecondsAfterNibble, int bait, int waterDepth, Farmer who, double baitPotency)
    {
      if (Game1.whichFarm == 1)
      {
        if (Game1.random.NextDouble() < 0.3)
          return this.getFish(millisecondsAfterNibble, bait, waterDepth, who, baitPotency, "Forest");
        return this.getFish(millisecondsAfterNibble, bait, waterDepth, who, baitPotency, "Town");
      }
      if (Game1.whichFarm == 3)
      {
        if (Game1.random.NextDouble() < 0.5)
          return this.getFish(millisecondsAfterNibble, bait, waterDepth, who, baitPotency, "Forest");
      }
      else if (Game1.whichFarm == 2)
      {
        if (Game1.random.NextDouble() < 0.05 + Game1.dailyLuck)
          return new Object(734, 1, false, -1, 0);
        if (Game1.random.NextDouble() < 0.45)
          return this.getFish(millisecondsAfterNibble, bait, waterDepth, who, baitPotency, "Forest");
      }
      else if (Game1.whichFarm == 4 && Game1.random.NextDouble() <= 0.35)
        return this.getFish(millisecondsAfterNibble, bait, waterDepth, who, baitPotency, "Mountain");
      return base.getFish(millisecondsAfterNibble, bait, waterDepth, who, baitPotency);
    }

    public List<FarmAnimal> getAllFarmAnimals()
    {
      List<FarmAnimal> list = this.animals.Values.ToList<FarmAnimal>();
      foreach (Building building in this.buildings)
      {
        if (building.indoors != null && building.indoors.GetType() == typeof (AnimalHouse))
          list.AddRange((IEnumerable<FarmAnimal>) ((AnimalHouse) building.indoors).animals.Values.ToList<FarmAnimal>());
      }
      return list;
    }

    public override bool isTileOccupied(Vector2 tileLocation, string characterToIgnore = "")
    {
      foreach (KeyValuePair<long, FarmAnimal> animal in (Dictionary<long, FarmAnimal>) this.animals)
      {
        if (animal.Value.getTileLocation().Equals(tileLocation))
          return true;
      }
      Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle((int) tileLocation.X * Game1.tileSize, (int) tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
      foreach (ResourceClump resourceClump in this.resourceClumps)
      {
        Vector2 tile = resourceClump.tile;
        if (resourceClump.getBoundingBox(tile).Intersects(rectangle))
          return true;
      }
      return base.isTileOccupied(tileLocation, "");
    }

    public override void resetForPlayerEntry()
    {
      base.resetForPlayerEntry();
      if (!Game1.player.mailReceived.Contains("button_tut_2"))
      {
        Game1.player.mailReceived.Add("button_tut_2");
        Game1.onScreenMenus.Add((IClickableMenu) new ButtonTutorialMenu(1));
      }
      this.houseSource = new Microsoft.Xna.Framework.Rectangle(0, 144 * (Game1.player.houseUpgradeLevel == 3 ? 2 : Game1.player.houseUpgradeLevel), 160, 144);
      this.greenhouseSource = new Microsoft.Xna.Framework.Rectangle(160, 160 * (Game1.player.mailReceived.Contains("ccPantry") ? 1 : 0), 112, 160);
      if (Game1.player.isMarried())
        this.addSpouseOutdoorArea(Game1.player.spouse);
      for (int index = this.characters.Count - 1; index >= 0; --index)
      {
        if (this.characters[index] is Child)
          (this.characters[index] as Child).resetForPlayerEntry((GameLocation) this);
        if (this.characters[index].isVillager() && this.characters[index].name.Equals(Game1.player.spouse))
          this.setMapTileIndex(54, 7, 1939, "Buildings", 0);
        if (this.characters[index] is Pet && (this.getTileIndexAt(this.characters[index].getTileLocationPoint(), "Buildings") != -1 || this.getTileIndexAt(this.characters[index].getTileX() + 1, this.characters[index].getTileY(), "Buildings") != -1 || (!this.isTileLocationTotallyClearAndPlaceable(this.characters[index].getTileLocation()) || !this.isTileLocationTotallyClearAndPlaceable(new Vector2((float) (this.characters[index].getTileX() + 1), (float) this.characters[index].getTileY())))))
        {
          this.characters[index].setTilePosition(new Point(54, 8));
          this.characters[index].position.X -= (float) Game1.tileSize;
        }
        if (Game1.timeOfDay >= 1300 && this.characters[index].isMarried() && this.characters[index].controller == null)
        {
          this.characters[index].drawOffset = Vector2.Zero;
          this.characters[index].sprite.StopAnimation();
          FarmHouse locationFromName = (FarmHouse) Game1.getLocationFromName("FarmHouse");
          Game1.warpCharacter(this.characters[index], "FarmHouse", locationFromName.getEntryLocation(), false, true);
          break;
        }
      }
      if (Game1.timeOfDay >= 1830)
      {
        for (int index = this.animals.Count - 1; index >= 0; --index)
          this.animals.ElementAt<KeyValuePair<long, FarmAnimal>>(index).Value.warpHome(this, this.animals.ElementAt<KeyValuePair<long, FarmAnimal>>(index).Value);
      }
      TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(134, 226, 30, 25), new Vector2(71f, 13f) * (float) Game1.tileSize + new Vector2(2f, -7f) * (float) Game1.pixelZoom, false, 0.0f, Color.White);
      temporaryAnimatedSprite.holdLastFrame = true;
      temporaryAnimatedSprite.destroyable = false;
      temporaryAnimatedSprite.interval = 20f;
      temporaryAnimatedSprite.animationLength = 13;
      temporaryAnimatedSprite.paused = true;
      double pixelZoom = (double) Game1.pixelZoom;
      temporaryAnimatedSprite.scale = (float) pixelZoom;
      double num1 = (double) (15 * Game1.tileSize) / 10000.0 + 9.99999974737875E-05;
      temporaryAnimatedSprite.layerDepth = (float) num1;
      int num2 = 1;
      temporaryAnimatedSprite.pingPong = num2 != 0;
      int num3 = 0;
      temporaryAnimatedSprite.pingPongMotion = num3;
      this.shippingBinLid = temporaryAnimatedSprite;
      if (this.isThereABuildingUnderConstruction() && this.getBuildingUnderConstruction().daysOfConstructionLeft > 0 && Game1.getCharacterFromName("Robin", false).currentLocation.Equals((object) this))
      {
        Building underConstruction = this.getBuildingUnderConstruction();
        this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(399, 262, underConstruction.daysOfConstructionLeft == 1 ? 29 : 9, 43), new Vector2((float) (underConstruction.tileX + underConstruction.tilesWide / 2), (float) (underConstruction.tileY + underConstruction.tilesHigh / 2)) * (float) Game1.tileSize + new Vector2((float) (-Game1.tileSize / 4), (float) (-Game1.tileSize * 2 - Game1.tileSize / 4)), false, 0.0f, Color.White)
        {
          scale = (float) Game1.pixelZoom,
          interval = 999999f,
          animationLength = 1,
          totalNumberOfLoops = 99999,
          layerDepth = (float) ((double) ((underConstruction.tileY + underConstruction.tilesHigh / 2) * Game1.tileSize + Game1.tileSize / 2) / 10000.0)
        });
      }
      this.addGrandpaCandles();
    }

    public void addSpouseOutdoorArea(string spouseName)
    {
      this.removeTile(70, 9, "Buildings");
      this.removeTile(71, 9, "Buildings");
      this.removeTile(72, 9, "Buildings");
      this.removeTile(69, 9, "Buildings");
      this.removeTile(70, 8, "Buildings");
      this.removeTile(71, 8, "Buildings");
      this.removeTile(72, 8, "Buildings");
      this.removeTile(69, 8, "Buildings");
      this.removeTile(70, 7, "Front");
      this.removeTile(71, 7, "Front");
      this.removeTile(72, 7, "Front");
      this.removeTile(69, 7, "Front");
      this.removeTile(70, 6, "AlwaysFront");
      this.removeTile(71, 6, "AlwaysFront");
      this.removeTile(72, 6, "AlwaysFront");
      this.removeTile(69, 6, "AlwaysFront");
      // ISSUE: reference to a compiler-generated method
      uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(spouseName);
      if (stringHash <= 1866496948U)
      {
        if (stringHash <= 1067922812U)
        {
          if ((int) stringHash != 161540545)
          {
            if ((int) stringHash != 587846041)
            {
              if ((int) stringHash != 1067922812 || !(spouseName == "Sam"))
                return;
              this.setMapTileIndex(69, 8, 1173, "Buildings", 1);
              this.setMapTileIndex(72, 8, 1174, "Buildings", 1);
              this.setMapTileIndex(70, 8, 1198, "Buildings", 1);
              this.setMapTileIndex(71, 8, 1199, "Buildings", 1);
              this.setMapTileIndex(69, 7, 1148, "Front", 1);
              this.setMapTileIndex(72, 7, 1149, "Front", 1);
            }
            else
            {
              if (!(spouseName == "Penny"))
                return;
              this.setMapTileIndex(69, 8, 1098, "Buildings", 1);
              this.setMapTileIndex(70, 8, 1123, "Buildings", 1);
              this.setMapTileIndex(72, 8, 1098, "Buildings", 1);
            }
          }
          else
          {
            if (!(spouseName == "Sebastian"))
              return;
            this.setMapTileIndex(70, 8, 1927, "Buildings", 1);
            this.setMapTileIndex(71, 8, 1928, "Buildings", 1);
            this.setMapTileIndex(72, 8, 1929, "Buildings", 1);
            this.setMapTileIndex(70, 7, 1902, "Front", 1);
            this.setMapTileIndex(71, 7, 1903, "Front", 1);
          }
        }
        else if ((int) stringHash != 1281010426)
        {
          if ((int) stringHash != 1708213605)
          {
            if ((int) stringHash != 1866496948 || !(spouseName == "Shane"))
              return;
            this.setMapTileIndex(70, 9, 1940, "Buildings", 1);
            this.setMapTileIndex(71, 9, 1941, "Buildings", 1);
            this.setMapTileIndex(72, 9, 1942, "Buildings", 1);
            this.setMapTileIndex(70, 8, 1915, "Buildings", 1);
            this.setMapTileIndex(71, 8, 1916, "Buildings", 1);
            this.setMapTileIndex(72, 8, 1917, "Buildings", 1);
            this.setMapTileIndex(70, 7, 1772, "Front", 1);
            this.setMapTileIndex(71, 7, 1773, "Front", 1);
            this.setMapTileIndex(72, 7, 1774, "Front", 1);
            this.setMapTileIndex(70, 6, 1747, "AlwaysFront", 1);
            this.setMapTileIndex(71, 6, 1748, "AlwaysFront", 1);
            this.setMapTileIndex(72, 6, 1749, "AlwaysFront", 1);
          }
          else
          {
            if (!(spouseName == "Alex"))
              return;
            this.setMapTileIndex(69, 8, 1099, "Buildings", 1);
          }
        }
        else
        {
          if (!(spouseName == "Maru"))
            return;
          this.setMapTileIndex(71, 8, 1124, "Buildings", 1);
        }
      }
      else if (stringHash <= 2571828641U)
      {
        if ((int) stringHash != 2010304804)
        {
          if ((int) stringHash != -1860673204)
          {
            if ((int) stringHash != -1723138655 || !(spouseName == "Emily"))
              return;
            this.setMapTileIndex(69, 8, 1867, "Buildings", 1);
            this.setMapTileIndex(72, 8, 1867, "Buildings", 1);
            this.setMapTileIndex(69, 7, 1842, "Front", 1);
            this.setMapTileIndex(72, 7, 1842, "Front", 1);
            this.setMapTileIndex(69, 9, 1866, "Buildings", 1);
            this.setMapTileIndex(71, 8, 1866, "Buildings", 1);
            this.setMapTileIndex(72, 9, 1967, "Buildings", 1);
            this.setMapTileIndex(70, 8, 1967, "Buildings", 1);
          }
          else
          {
            if (!(spouseName == "Haley"))
              return;
            this.setMapTileIndex(69, 8, 1074, "Buildings", 1);
            this.setMapTileIndex(69, 7, 1049, "Front", 1);
            this.setMapTileIndex(69, 6, 1024, "AlwaysFront", 1);
            this.setMapTileIndex(72, 8, 1074, "Buildings", 1);
            this.setMapTileIndex(72, 7, 1049, "Front", 1);
            this.setMapTileIndex(72, 6, 1024, "AlwaysFront", 1);
          }
        }
        else
        {
          if (!(spouseName == "Harvey"))
            return;
          this.setMapTileIndex(69, 8, 1098, "Buildings", 1);
          this.setMapTileIndex(70, 8, 1123, "Buildings", 1);
          this.setMapTileIndex(72, 8, 1098, "Buildings", 1);
        }
      }
      else if ((int) stringHash != -1562053956)
      {
        if ((int) stringHash != -1468719973)
        {
          if ((int) stringHash != -1228790996 || !(spouseName == "Elliott"))
            return;
          this.setMapTileIndex(69, 8, 1098, "Buildings", 1);
          this.setMapTileIndex(70, 8, 1123, "Buildings", 1);
          this.setMapTileIndex(72, 8, 1098, "Buildings", 1);
        }
        else
        {
          if (!(spouseName == "Leah"))
            return;
          this.setMapTileIndex(70, 8, 1122, "Buildings", 1);
          this.setMapTileIndex(70, 7, 1097, "Front", 1);
        }
      }
      else
      {
        if (!(spouseName == "Abigail"))
          return;
        this.setMapTileIndex(69, 8, 1098, "Buildings", 1);
        this.setMapTileIndex(70, 8, 1123, "Buildings", 1);
        this.setMapTileIndex(72, 8, 1098, "Buildings", 1);
      }
    }

    public void addGrandpaCandles()
    {
      if (this.grandpaScore <= 0)
        return;
      Microsoft.Xna.Framework.Rectangle sourceRect = new Microsoft.Xna.Framework.Rectangle(577, 1985, 2, 5);
      this.removeTemporarySpritesWithID(6666);
      this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, sourceRect, 99999f, 1, 9999, new Vector2((float) (7 * Game1.tileSize + 5 * Game1.pixelZoom), (float) (6 * Game1.tileSize + 5 * Game1.pixelZoom)), false, false, (float) (6 * Game1.tileSize) / 10000f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false));
      List<TemporaryAnimatedSprite> temporarySprites1 = this.temporarySprites;
      TemporaryAnimatedSprite temporaryAnimatedSprite1 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(536, 1945, 8, 8), new Vector2((float) (7 * Game1.tileSize + 3 * Game1.pixelZoom), (float) (6 * Game1.tileSize - Game1.pixelZoom)), false, 0.0f, Color.White);
      temporaryAnimatedSprite1.interval = 50f;
      temporaryAnimatedSprite1.totalNumberOfLoops = 99999;
      temporaryAnimatedSprite1.animationLength = 7;
      temporaryAnimatedSprite1.light = true;
      temporaryAnimatedSprite1.id = 6666f;
      temporaryAnimatedSprite1.lightRadius = 1f;
      double num1 = (double) (Game1.pixelZoom * 3) / 4.0;
      temporaryAnimatedSprite1.scale = (float) num1;
      double num2 = (double) (6 * Game1.tileSize) / 10000.0 + 9.99999974737875E-05;
      temporaryAnimatedSprite1.layerDepth = (float) num2;
      int num3 = 0;
      temporaryAnimatedSprite1.delayBeforeAnimationStart = num3;
      temporarySprites1.Add(temporaryAnimatedSprite1);
      if (this.grandpaScore > 1)
      {
        this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, sourceRect, 99999f, 1, 9999, new Vector2((float) (7 * Game1.tileSize + 10 * Game1.pixelZoom), (float) (5 * Game1.tileSize + 6 * Game1.pixelZoom)), false, false, (float) (6 * Game1.tileSize) / 10000f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false));
        List<TemporaryAnimatedSprite> temporarySprites2 = this.temporarySprites;
        TemporaryAnimatedSprite temporaryAnimatedSprite2 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(536, 1945, 8, 8), new Vector2((float) (7 * Game1.tileSize + 9 * Game1.pixelZoom), (float) (5 * Game1.tileSize)), false, 0.0f, Color.White);
        temporaryAnimatedSprite2.interval = 50f;
        temporaryAnimatedSprite2.totalNumberOfLoops = 99999;
        temporaryAnimatedSprite2.animationLength = 7;
        temporaryAnimatedSprite2.light = true;
        temporaryAnimatedSprite2.id = 6666f;
        temporaryAnimatedSprite2.lightRadius = 1f;
        double num4 = (double) (Game1.pixelZoom * 3) / 4.0;
        temporaryAnimatedSprite2.scale = (float) num4;
        double num5 = (double) (6 * Game1.tileSize) / 10000.0 + 9.99999974737875E-05;
        temporaryAnimatedSprite2.layerDepth = (float) num5;
        int num6 = 50;
        temporaryAnimatedSprite2.delayBeforeAnimationStart = num6;
        temporarySprites2.Add(temporaryAnimatedSprite2);
      }
      if (this.grandpaScore > 2)
      {
        this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, sourceRect, 99999f, 1, 9999, new Vector2((float) (9 * Game1.tileSize + 5 * Game1.pixelZoom), (float) (5 * Game1.tileSize + 6 * Game1.pixelZoom)), false, false, (float) (6 * Game1.tileSize) / 10000f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false));
        List<TemporaryAnimatedSprite> temporarySprites2 = this.temporarySprites;
        TemporaryAnimatedSprite temporaryAnimatedSprite2 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(536, 1945, 8, 8), new Vector2((float) (9 * Game1.tileSize + 4 * Game1.pixelZoom), (float) (5 * Game1.tileSize)), false, 0.0f, Color.White);
        temporaryAnimatedSprite2.interval = 50f;
        temporaryAnimatedSprite2.totalNumberOfLoops = 99999;
        temporaryAnimatedSprite2.animationLength = 7;
        temporaryAnimatedSprite2.light = true;
        temporaryAnimatedSprite2.id = 6666f;
        temporaryAnimatedSprite2.lightRadius = 1f;
        double num4 = (double) (Game1.pixelZoom * 3) / 4.0;
        temporaryAnimatedSprite2.scale = (float) num4;
        double num5 = (double) (6 * Game1.tileSize) / 10000.0 + 9.99999974737875E-05;
        temporaryAnimatedSprite2.layerDepth = (float) num5;
        int num6 = 100;
        temporaryAnimatedSprite2.delayBeforeAnimationStart = num6;
        temporarySprites2.Add(temporaryAnimatedSprite2);
      }
      if (this.grandpaScore <= 3)
        return;
      this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, sourceRect, 99999f, 1, 9999, new Vector2((float) (9 * Game1.tileSize + 10 * Game1.pixelZoom), (float) (6 * Game1.tileSize + 5 * Game1.pixelZoom)), false, false, (float) (6 * Game1.tileSize) / 10000f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false));
      List<TemporaryAnimatedSprite> temporarySprites3 = this.temporarySprites;
      TemporaryAnimatedSprite temporaryAnimatedSprite3 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(536, 1945, 8, 8), new Vector2((float) (9 * Game1.tileSize + 9 * Game1.pixelZoom), (float) (6 * Game1.tileSize - Game1.pixelZoom)), false, 0.0f, Color.White);
      temporaryAnimatedSprite3.interval = 50f;
      temporaryAnimatedSprite3.totalNumberOfLoops = 99999;
      temporaryAnimatedSprite3.animationLength = 7;
      temporaryAnimatedSprite3.light = true;
      temporaryAnimatedSprite3.id = 6666f;
      temporaryAnimatedSprite3.lightRadius = 1f;
      double num7 = (double) (Game1.pixelZoom * 3) / 4.0;
      temporaryAnimatedSprite3.scale = (float) num7;
      double num8 = (double) (6 * Game1.tileSize) / 10000.0 + 9.99999974737875E-05;
      temporaryAnimatedSprite3.layerDepth = (float) num8;
      int num9 = 150;
      temporaryAnimatedSprite3.delayBeforeAnimationStart = num9;
      temporarySprites3.Add(temporaryAnimatedSprite3);
    }

    private void openShippingBinLid()
    {
      if (this.shippingBinLid == null)
        return;
      if (this.shippingBinLid.pingPongMotion != 1)
        Game1.playSound("doorCreak");
      this.shippingBinLid.pingPongMotion = 1;
      this.shippingBinLid.paused = false;
    }

    private void closeShippingBinLid()
    {
      if (this.shippingBinLid == null || this.shippingBinLid.currentParentTileIndex <= 0)
        return;
      if (this.shippingBinLid.pingPongMotion != -1)
        Game1.playSound("doorCreakReverse");
      this.shippingBinLid.pingPongMotion = -1;
      this.shippingBinLid.paused = false;
    }

    private void updateShippingBinLid(GameTime time)
    {
      if (this.isShippingBinLidOpen(true) && this.shippingBinLid.pingPongMotion == 1)
        this.shippingBinLid.paused = true;
      else if (this.shippingBinLid.currentParentTileIndex == 0 && this.shippingBinLid.pingPongMotion == -1)
      {
        if (!this.shippingBinLid.paused)
          Game1.playSound("woodyStep");
        this.shippingBinLid.paused = true;
      }
      this.shippingBinLid.update(time);
    }

    private bool isShippingBinLidOpen(bool requiredToBeFullyOpen = false)
    {
      return this.shippingBinLid != null && this.shippingBinLid.currentParentTileIndex >= (requiredToBeFullyOpen ? this.shippingBinLid.animationLength - 1 : 1);
    }

    public override bool shouldShadowBeDrawnAboveBuildingsLayer(Vector2 p)
    {
      if (this.doesTileHavePropertyNoNull((int) p.X, (int) p.Y, "Type", "Back").Length > 0)
        return true;
      return base.shouldShadowBeDrawnAboveBuildingsLayer(p);
    }

    public override bool isTileOccupiedForPlacement(Vector2 tileLocation, Object toPlace = null)
    {
      foreach (KeyValuePair<long, FarmAnimal> animal in (Dictionary<long, FarmAnimal>) this.animals)
      {
        if (animal.Value.getTileLocation().Equals(tileLocation))
          return true;
      }
      foreach (ResourceClump resourceClump in this.resourceClumps)
      {
        if (resourceClump.occupiesTile((int) tileLocation.X, (int) tileLocation.Y))
          return true;
      }
      return base.isTileOccupiedForPlacement(tileLocation, toPlace);
    }

    public override void draw(SpriteBatch b)
    {
      base.draw(b);
      foreach (ResourceClump resourceClump in this.resourceClumps)
        resourceClump.draw(b, resourceClump.tile);
      foreach (KeyValuePair<long, FarmAnimal> animal in (Dictionary<long, FarmAnimal>) this.animals)
        animal.Value.draw(b);
      b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2(3776f, 1088f)), new Microsoft.Xna.Framework.Rectangle?(Building.leftShadow), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1E-05f);
      for (int index = 1; index < 8; ++index)
        b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (3776 + index * 64), 1088f)), new Microsoft.Xna.Framework.Rectangle?(Building.middleShadow), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1E-05f);
      b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2(4288f, 1088f)), new Microsoft.Xna.Framework.Rectangle?(Building.rightShadow), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1E-05f);
      b.Draw(this.houseTextures, Game1.GlobalToLocal(Game1.viewport, new Vector2(3712f, 520f)), new Microsoft.Xna.Framework.Rectangle?(this.houseSource), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 0.075f);
      b.Draw(this.houseTextures, Game1.GlobalToLocal(Game1.viewport, new Vector2(1600f, 384f)), new Microsoft.Xna.Framework.Rectangle?(this.greenhouseSource), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 0.0704f);
      if (Game1.mailbox.Count > 0)
      {
        float num = (float) (4.0 * Math.Round(Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 250.0), 2));
        b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (68 * Game1.tileSize), (float) (16 * Game1.tileSize - Game1.tileSize * 3 / 2 - 48) + num)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(141, 465, 20, 24)), Color.White * 0.75f, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, (float) ((double) (17 * Game1.tileSize) / 10000.0 + 9.99999997475243E-07 + 0.00680000009015203));
        b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (68 * Game1.tileSize + Game1.tileSize / 2 + Game1.pixelZoom), (float) (16 * Game1.tileSize - Game1.tileSize - 24 - Game1.tileSize / 8) + num)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(189, 423, 15, 13)), Color.White, 0.0f, new Vector2(7f, 6f), 4f, SpriteEffects.None, (float) ((double) (17 * Game1.tileSize) / 10000.0 + 9.99999974737875E-06 + 0.00680000009015203));
      }
      if (this.shippingBinLid != null)
        this.shippingBinLid.draw(b, false, 0, 0);
      if (this.hasSeenGrandpaNote)
        return;
      b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (9 * Game1.tileSize), (float) (7 * Game1.tileSize))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(575, 1972, 11, 8)), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, (float) ((double) (7 * Game1.tileSize) / 10000.0 + 9.99999997475243E-07));
    }

    public override void drawAboveAlwaysFrontLayer(SpriteBatch b)
    {
      base.drawAboveAlwaysFrontLayer(b);
      if (!Game1.spawnMonstersAtNight)
        return;
      foreach (Character character in this.characters)
      {
        if (character is Monster)
          (character as Monster).drawAboveAllLayers(b);
      }
    }

    public override void updateEvenIfFarmerIsntHere(GameTime time, bool skipWasUpdatedFlush = false)
    {
      base.updateEvenIfFarmerIsntHere(time, skipWasUpdatedFlush);
      if (Game1.currentLocation.Equals((object) this))
        return;
      for (int index = this.animals.Count - 1; index >= 0; --index)
        this.animals.ElementAt<KeyValuePair<long, FarmAnimal>>(index).Value.updateWhenNotCurrentLocation((Building) null, time, (GameLocation) this);
    }

    public bool isTileOpenBesidesTerrainFeatures(Vector2 tile)
    {
      Microsoft.Xna.Framework.Rectangle boundingBox = new Microsoft.Xna.Framework.Rectangle((int) tile.X * Game1.tileSize, (int) tile.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
      foreach (Building building in this.buildings)
      {
        if (building.intersects(boundingBox))
          return false;
      }
      foreach (ResourceClump resourceClump in this.resourceClumps)
      {
        Vector2 tile1 = resourceClump.tile;
        if (resourceClump.getBoundingBox(tile1).Intersects(boundingBox))
          return false;
      }
      foreach (KeyValuePair<long, FarmAnimal> animal in (Dictionary<long, FarmAnimal>) this.animals)
      {
        if (animal.Value.getTileLocation().Equals(tile))
          return true;
      }
      if (!this.objects.ContainsKey(tile))
        return this.isTilePassable(new Location((int) tile.X, (int) tile.Y), Game1.viewport);
      return false;
    }

    public void addResourceClumpAndRemoveUnderlyingTerrain(int resourceClumpIndex, int width, int height, Vector2 tile)
    {
      for (int index1 = 0; index1 < width; ++index1)
      {
        for (int index2 = 0; index2 < height; ++index2)
          this.removeEverythingExceptCharactersFromThisTile((int) tile.X + index1, (int) tile.Y + index2);
      }
      this.resourceClumps.Add(new ResourceClump(resourceClumpIndex, width, height, tile));
    }

    public override void UpdateWhenCurrentLocation(GameTime time)
    {
      if (this.wasUpdated && (int) Game1.gameMode != 0)
        return;
      base.UpdateWhenCurrentLocation(time);
      if (Utility.getHomeOfFarmer(Game1.player).fireplaceOn)
      {
        this.chimneyTimer = this.chimneyTimer - time.ElapsedGameTime.Milliseconds;
        if (this.chimneyTimer <= 0)
        {
          Point porchStandingSpot = Utility.getHomeOfFarmer(Game1.player).getPorchStandingSpot();
          List<TemporaryAnimatedSprite> temporarySprites = this.temporarySprites;
          TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(372, 1956, 10, 10), new Vector2((float) (porchStandingSpot.X * Game1.tileSize + Game1.pixelZoom * (Utility.getHomeOfFarmer(Game1.player).upgradeLevel >= 2 ? 9 : -5)), (float) (porchStandingSpot.Y * Game1.tileSize - Game1.pixelZoom * 105)), false, 1f / 500f, Color.Gray);
          temporaryAnimatedSprite.alpha = 0.75f;
          Vector2 vector2_1 = new Vector2(0.0f, -0.5f);
          temporaryAnimatedSprite.motion = vector2_1;
          Vector2 vector2_2 = new Vector2(1f / 500f, 0.0f);
          temporaryAnimatedSprite.acceleration = vector2_2;
          double num1 = 99999.0;
          temporaryAnimatedSprite.interval = (float) num1;
          double num2 = 1.0;
          temporaryAnimatedSprite.layerDepth = (float) num2;
          double num3 = (double) (Game1.pixelZoom / 2);
          temporaryAnimatedSprite.scale = (float) num3;
          double num4 = 0.0199999995529652;
          temporaryAnimatedSprite.scaleChange = (float) num4;
          double num5 = (double) Game1.random.Next(-5, 6) * 3.14159274101257 / 256.0;
          temporaryAnimatedSprite.rotationChange = (float) num5;
          temporarySprites.Add(temporaryAnimatedSprite);
          this.chimneyTimer = 500;
        }
      }
      foreach (ResourceClump resourceClump in this.resourceClumps)
        resourceClump.tickUpdate(time, resourceClump.tile);
      foreach (KeyValuePair<long, FarmAnimal> animal in (Dictionary<long, FarmAnimal>) this.animals)
      {
        if (animal.Value.updateWhenCurrentLocation(time, (GameLocation) this))
          this.animalsToRemove.Add(animal.Key);
      }
      for (int index = 0; index < this.animalsToRemove.Count; ++index)
        this.animals.Remove(this.animalsToRemove[index]);
      this.animalsToRemove.Clear();
      if (this.shippingBinLid == null)
        return;
      bool flag = false;
      foreach (Character farmer in this.getFarmers())
      {
        if (farmer.GetBoundingBox().Intersects(this.shippingBinLidOpenArea))
        {
          this.openShippingBinLid();
          flag = true;
        }
      }
      if (!flag)
        this.closeShippingBinLid();
      this.updateShippingBinLid(time);
    }
  }
}
