// Decompiled with JetBrains decompiler
// Type: StardewValley.SaveGame
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Buildings;
using StardewValley.Characters;
using StardewValley.Locations;
using StardewValley.Monsters;
using StardewValley.Objects;
using StardewValley.Quests;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace StardewValley
{
  public class SaveGame
  {
    public static XmlSerializer serializer = new XmlSerializer(typeof (SaveGame), new Type[28]
    {
      typeof (Tool),
      typeof (GameLocation),
      typeof (Crow),
      typeof (Duggy),
      typeof (Bug),
      typeof (BigSlime),
      typeof (Fireball),
      typeof (Ghost),
      typeof (Child),
      typeof (Pet),
      typeof (Dog),
      typeof (StardewValley.Characters.Cat),
      typeof (Horse),
      typeof (GreenSlime),
      typeof (LavaCrab),
      typeof (RockCrab),
      typeof (ShadowGuy),
      typeof (SkeletonMage),
      typeof (SquidKid),
      typeof (Grub),
      typeof (Fly),
      typeof (DustSpirit),
      typeof (Quest),
      typeof (MetalHead),
      typeof (ShadowGirl),
      typeof (Monster),
      typeof (JunimoHarvester),
      typeof (TerrainFeature)
    });
    public static XmlSerializer farmerSerializer = new XmlSerializer(typeof (Farmer), new Type[1]
    {
      typeof (Tool)
    });
    public static XmlSerializer locationSerializer = new XmlSerializer(typeof (GameLocation), new Type[27]
    {
      typeof (Tool),
      typeof (Crow),
      typeof (Duggy),
      typeof (Fireball),
      typeof (Ghost),
      typeof (GreenSlime),
      typeof (LavaCrab),
      typeof (RockCrab),
      typeof (ShadowGuy),
      typeof (SkeletonWarrior),
      typeof (Child),
      typeof (Pet),
      typeof (Dog),
      typeof (StardewValley.Characters.Cat),
      typeof (Horse),
      typeof (SquidKid),
      typeof (Grub),
      typeof (Fly),
      typeof (DustSpirit),
      typeof (Bug),
      typeof (BigSlime),
      typeof (BreakableContainer),
      typeof (MetalHead),
      typeof (ShadowGirl),
      typeof (Monster),
      typeof (JunimoHarvester),
      typeof (TerrainFeature)
    });
    public List<ResourceClump> mine_resourceClumps = new List<ResourceClump>();
    public static bool IsProcessing;
    public static bool CancelToTitle;
    public Farmer player;
    public List<GameLocation> locations;
    public string currentSeason;
    public string samBandName;
    public string elliottBookName;
    public List<string> mailbox;
    public int dayOfMonth;
    public int year;
    public int farmerWallpaper;
    public int FarmerFloor;
    public int countdownToWedding;
    public int currentWallpaper;
    public int currentFloor;
    public int currentSongIndex;
    public Point incubatingEgg;
    public double chanceToRainTomorrow;
    public double dailyLuck;
    public ulong uniqueIDForThisGame;
    public bool weddingToday;
    public bool isRaining;
    public bool isDebrisWeather;
    public bool shippingTax;
    public bool bloomDay;
    public bool isLightning;
    public bool isSnowing;
    public bool shouldSpawnMonsters;
    public Stats stats;
    public static SaveGame loaded;
    public float musicVolume;
    public float soundVolume;
    public int[] cropsOfTheWeek;
    public Object dishOfTheDay;
    public long latestID;
    public Options options;
    public SerializableDictionary<int, MineInfo> mine_permanentMineChanges;
    public int mine_mineLevel;
    public int mine_nextLevel;
    public int mine_lowestLevelReached;
    public int minecartHighScore;
    public int weatherForTomorrow;
    public int whichFarm;

    public static IEnumerator<int> Save()
    {
      SaveGame.IsProcessing = true;
      yield return 1;
      IEnumerator<int> loader = SaveGame.getSaveEnumerator();
      Task task = new Task((Action) (() =>
      {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        if (loader == null)
          return;
        do
          ;
        while (loader.MoveNext() && loader.Current < 100);
      }));
      task.Start();
      while (!task.IsCanceled && !task.IsCompleted && !task.IsFaulted)
        yield return 1;
      SaveGame.IsProcessing = false;
      if (task.IsFaulted)
      {
        Exception baseException = task.Exception.GetBaseException();
        if (!(baseException is TaskCanceledException))
          throw baseException;
        Game1.ExitToTitle();
      }
      else
        yield return 100;
    }

    public static IEnumerator<int> getSaveEnumerator()
    {
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      if (SaveGame.CancelToTitle)
        throw new TaskCanceledException();
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E2__current = 1;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = 1;
      return true;
    }

    public static void ensureFolderStructureExists(string tmpString = "")
    {
      string str = Game1.player.Name;
      foreach (char c in str)
      {
        if (!char.IsLetterOrDigit(c))
          str = str.Replace(c.ToString() ?? "", "");
      }
      string path2 = str + "_" + (object) Game1.uniqueIDForThisGame + tmpString;
      FileInfo fileInfo1 = new FileInfo(Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StardewValley"), "Saves"), path2));
      if (!fileInfo1.Directory.Exists)
        fileInfo1.Directory.Create();
      FileInfo fileInfo2 = new FileInfo(Path.Combine(Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StardewValley"), "Saves"), path2), "dummy"));
      if (!fileInfo2.Directory.Exists)
        fileInfo2.Directory.Create();
    }

    public static void Load(string filename)
    {
      Game1.gameMode = (byte) 6;
      Game1.loadingMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:SaveGame.cs.4690");
      Game1.currentLoader = SaveGame.getLoadEnumerator(filename);
    }

    public static IEnumerator<int> getLoadEnumerator(string file)
    {
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      Console.WriteLine("getLoadEnumerator('{0}')", (object) file);
      Game1.loadingMessage = "Accessing save...";
      SaveGame saveGame = new SaveGame();
      SaveGame.IsProcessing = true;
      if (SaveGame.CancelToTitle)
        Game1.ExitToTitle();
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E2__current = 1;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = 1;
      return true;
    }

    public static void loadDataToFarmer(Farmer target)
    {
      Farmer farmer = target;
      target.items = farmer.items;
      target.canMove = true;
      target.sprite = (AnimatedSprite) new FarmerSprite((Texture2D) null);
      target.FarmerSprite.setOwner(target);
      target.reloadLivestockSprites();
      if (target.cookingRecipes == null || target.cookingRecipes.Count == 0)
        target.cookingRecipes.Add("Fried Egg", 0);
      if (target.craftingRecipes == null || target.craftingRecipes.Count == 0)
        target.craftingRecipes.Add("Lumber", 0);
      if (!target.songsHeard.Contains("title_day"))
        target.songsHeard.Add("title_day");
      if (!target.songsHeard.Contains("title_night"))
        target.songsHeard.Add("title_night");
      if (target.addedSpeed > 0)
        target.addedSpeed = 0;
      target.maxItems = farmer.maxItems;
      for (int index = 0; index < target.maxItems; ++index)
      {
        if (target.items.Count <= index)
          target.items.Add((Item) null);
      }
      if (target.FarmerRenderer == null)
        target.FarmerRenderer = new FarmerRenderer(target.getTexture());
      target.changeGender(farmer.isMale);
      target.changeAccessory(farmer.accessory);
      target.changeShirt(farmer.shirt);
      target.changePants(farmer.pantsColor);
      target.changeSkinColor(farmer.skin);
      target.changeHairColor(farmer.hairstyleColor);
      target.changeHairStyle(farmer.hair);
      if (target.boots != null)
        target.changeShoeColor(farmer.boots.indexInColorSheet);
      target.Stamina = farmer.Stamina;
      target.health = farmer.health;
      target.MaxStamina = farmer.MaxStamina;
      target.mostRecentBed = farmer.mostRecentBed;
      target.position = target.mostRecentBed;
      target.position.X -= (float) Game1.tileSize;
      target.checkForLevelTenStatus();
      if (!target.craftingRecipes.ContainsKey("Wood Path"))
        target.craftingRecipes.Add("Wood Path", 1);
      if (!target.craftingRecipes.ContainsKey("Gravel Path"))
        target.craftingRecipes.Add("Gravel Path", 1);
      if (target.craftingRecipes.ContainsKey("Cobblestone Path"))
        return;
      target.craftingRecipes.Add("Cobblestone Path", 1);
    }

    public static void loadDataToLocations(List<GameLocation> gamelocations)
    {
      foreach (GameLocation gamelocation in gamelocations)
      {
        if (gamelocation is FarmHouse)
        {
          GameLocation locationFromName = Game1.getLocationFromName(gamelocation.name);
          (Game1.getLocationFromName("FarmHouse") as FarmHouse).upgradeLevel = (gamelocation as FarmHouse).upgradeLevel;
          (locationFromName as FarmHouse).upgradeLevel = (gamelocation as FarmHouse).upgradeLevel;
          (locationFromName as FarmHouse).setMapForUpgradeLevel((locationFromName as FarmHouse).upgradeLevel, true);
          (locationFromName as FarmHouse).wallPaper = (gamelocation as FarmHouse).wallPaper;
          (locationFromName as FarmHouse).floor = (gamelocation as FarmHouse).floor;
          (locationFromName as FarmHouse).furniture = (gamelocation as FarmHouse).furniture;
          (locationFromName as FarmHouse).fireplaceOn = (gamelocation as FarmHouse).fireplaceOn;
          (locationFromName as FarmHouse).fridge = (gamelocation as FarmHouse).fridge;
          (locationFromName as FarmHouse).farmerNumberOfOwner = (gamelocation as FarmHouse).farmerNumberOfOwner;
          (locationFromName as FarmHouse).resetForPlayerEntry();
          foreach (Furniture furniture in (locationFromName as FarmHouse).furniture)
            furniture.updateDrawPosition();
          locationFromName.lastTouchActionLocation = Game1.player.getTileLocation();
        }
        if (gamelocation.name.Equals("Farm"))
        {
          GameLocation locationFromName = Game1.getLocationFromName(gamelocation.name);
          foreach (Building building in ((BuildableGameLocation) gamelocation).buildings)
            building.load();
          ((BuildableGameLocation) locationFromName).buildings = ((BuildableGameLocation) gamelocation).buildings;
          foreach (FarmAnimal farmAnimal in ((Farm) gamelocation).animals.Values)
            farmAnimal.reload();
        }
      }
      foreach (GameLocation gamelocation in gamelocations)
      {
        GameLocation locationFromName = Game1.getLocationFromName(gamelocation.name);
        gamelocation.name.Equals("Farm");
        for (int index = gamelocation.characters.Count - 1; index >= 0; --index)
        {
          if (!gamelocation.characters[index].DefaultPosition.Equals(Vector2.Zero))
            gamelocation.characters[index].position = gamelocation.characters[index].DefaultPosition;
          gamelocation.characters[index].currentLocation = locationFromName;
          if (index < gamelocation.characters.Count)
            gamelocation.characters[index].reloadSprite();
        }
        foreach (TerrainFeature terrainFeature in gamelocation.terrainFeatures.Values)
          terrainFeature.loadSprite();
        foreach (KeyValuePair<Vector2, Object> keyValuePair in (Dictionary<Vector2, Object>) gamelocation.objects)
        {
          keyValuePair.Value.initializeLightSource(keyValuePair.Key);
          keyValuePair.Value.reloadSprite();
        }
        if (gamelocation.name.Equals("Farm"))
        {
          ((BuildableGameLocation) locationFromName).buildings = ((BuildableGameLocation) gamelocation).buildings;
          foreach (FarmAnimal farmAnimal in ((Farm) gamelocation).animals.Values)
            farmAnimal.reload();
          foreach (Building building in Game1.getFarm().buildings)
          {
            Vector2 tile = new Vector2((float) building.tileX, (float) building.tileY);
            if (building.indoors is Shed)
            {
              (building.indoors as Shed).furniture = ((gamelocation as Farm).getBuildingAt(tile).indoors as Shed).furniture;
              (building.indoors as Shed).wallPaper = ((gamelocation as Farm).getBuildingAt(tile).indoors as Shed).wallPaper;
              (building.indoors as Shed).floor = ((gamelocation as Farm).getBuildingAt(tile).indoors as Shed).floor;
            }
            building.load();
            if (building.indoors is Shed)
            {
              (building.indoors as Shed).furniture = ((gamelocation as Farm).getBuildingAt(tile).indoors as Shed).furniture;
              (building.indoors as Shed).wallPaper = ((gamelocation as Farm).getBuildingAt(tile).indoors as Shed).wallPaper;
              (building.indoors as Shed).floor = ((gamelocation as Farm).getBuildingAt(tile).indoors as Shed).floor;
            }
          }
        }
        if (locationFromName != null)
        {
          locationFromName.characters = gamelocation.characters;
          locationFromName.objects = gamelocation.objects;
          locationFromName.numberOfSpawnedObjectsOnMap = gamelocation.numberOfSpawnedObjectsOnMap;
          locationFromName.terrainFeatures = gamelocation.terrainFeatures;
          locationFromName.largeTerrainFeatures = gamelocation.largeTerrainFeatures;
          if (locationFromName.name.Equals("Farm"))
          {
            ((Farm) locationFromName).animals = ((Farm) gamelocation).animals;
            (locationFromName as Farm).piecesOfHay = (gamelocation as Farm).piecesOfHay;
            (locationFromName as Farm).resourceClumps = (gamelocation as Farm).resourceClumps;
            (locationFromName as Farm).hasSeenGrandpaNote = (gamelocation as Farm).hasSeenGrandpaNote;
            (locationFromName as Farm).grandpaScore = (gamelocation as Farm).grandpaScore;
          }
          if (locationFromName is Sewer)
            (locationFromName as Sewer).populateShopStock(Game1.dayOfMonth);
          if (locationFromName is Beach)
            (locationFromName as Beach).bridgeFixed = (gamelocation as Beach).bridgeFixed;
          if (locationFromName is Woods)
          {
            (locationFromName as Woods).stumps = (gamelocation as Woods).stumps;
            (locationFromName as Woods).hasFoundStardrop = (gamelocation as Woods).hasFoundStardrop;
            (locationFromName as Woods).hasUnlockedStatue = (gamelocation as Woods).hasUnlockedStatue;
          }
          if (locationFromName is LibraryMuseum)
            (locationFromName as LibraryMuseum).museumPieces = (gamelocation as LibraryMuseum).museumPieces;
          if (locationFromName is CommunityCenter)
          {
            (locationFromName as CommunityCenter).bundleRewards = (gamelocation as CommunityCenter).bundleRewards;
            (locationFromName as CommunityCenter).bundles = (gamelocation as CommunityCenter).bundles;
            (locationFromName as CommunityCenter).areasComplete = (gamelocation as CommunityCenter).areasComplete;
          }
          if (locationFromName is SeedShop)
          {
            (locationFromName as SeedShop).itemsFromPlayerToSell = (gamelocation as SeedShop).itemsFromPlayerToSell;
            (locationFromName as SeedShop).itemsToStartSellingTomorrow = (gamelocation as SeedShop).itemsToStartSellingTomorrow;
          }
          if (locationFromName is Forest)
          {
            if (Game1.dayOfMonth % 7 % 5 == 0)
            {
              (locationFromName as Forest).travelingMerchantDay = true;
              (locationFromName as Forest).travelingMerchantBounds = new List<Rectangle>();
              (locationFromName as Forest).travelingMerchantBounds.Add(new Rectangle(23 * Game1.tileSize, 10 * Game1.tileSize, 123 * Game1.pixelZoom, 28 * Game1.pixelZoom));
              (locationFromName as Forest).travelingMerchantBounds.Add(new Rectangle(23 * Game1.tileSize + 45 * Game1.pixelZoom, 10 * Game1.tileSize + 26 * Game1.pixelZoom, 19 * Game1.pixelZoom, 12 * Game1.pixelZoom));
              (locationFromName as Forest).travelingMerchantBounds.Add(new Rectangle(23 * Game1.tileSize + 85 * Game1.pixelZoom, 10 * Game1.tileSize + 26 * Game1.pixelZoom, 26 * Game1.pixelZoom, 12 * Game1.pixelZoom));
              (locationFromName as Forest).travelingMerchantStock = Utility.getTravelingMerchantStock();
              foreach (Rectangle travelingMerchantBound in (locationFromName as Forest).travelingMerchantBounds)
                Utility.clearObjectsInArea(travelingMerchantBound, locationFromName);
            }
            (locationFromName as Forest).log = (gamelocation as Forest).log;
          }
        }
      }
      Game1.player.currentLocation = (GameLocation) Utility.getHomeOfFarmer(Game1.player);
    }
  }
}
