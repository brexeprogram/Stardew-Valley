// Decompiled with JetBrains decompiler
// Type: StardewValley.GameLocation
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Characters;
using StardewValley.Events;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Minigames;
using StardewValley.Monsters;
using StardewValley.Objects;
using StardewValley.Projectiles;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using xTile;
using xTile.Dimensions;
using xTile.ObjectModel;
using xTile.Tiles;

namespace StardewValley
{
  [XmlInclude(typeof (Farm))]
  [XmlInclude(typeof (Beach))]
  [XmlInclude(typeof (AnimalHouse))]
  [XmlInclude(typeof (SlimeHutch))]
  [XmlInclude(typeof (Shed))]
  [XmlInclude(typeof (LibraryMuseum))]
  [XmlInclude(typeof (AdventureGuild))]
  [XmlInclude(typeof (Woods))]
  [XmlInclude(typeof (Railroad))]
  [XmlInclude(typeof (Summit))]
  [XmlInclude(typeof (Forest))]
  [XmlInclude(typeof (SeedShop))]
  [XmlInclude(typeof (BathHousePool))]
  [XmlInclude(typeof (FarmHouse))]
  [XmlInclude(typeof (Club))]
  [XmlInclude(typeof (BusStop))]
  [XmlInclude(typeof (CommunityCenter))]
  [XmlInclude(typeof (Desert))]
  [XmlInclude(typeof (FarmCave))]
  [XmlInclude(typeof (JojaMart))]
  [XmlInclude(typeof (MineShaft))]
  [XmlInclude(typeof (Mountain))]
  [XmlInclude(typeof (Sewer))]
  [XmlInclude(typeof (WizardHouse))]
  [XmlInclude(typeof (Town))]
  [XmlInclude(typeof (Cellar))]
  public class GameLocation
  {
    private static readonly char[] ForwardSlash = new char[1]
    {
      '/'
    };
    public List<NPC> characters = new List<NPC>();
    public SerializableDictionary<Vector2, Object> objects = new SerializableDictionary<Vector2, Object>();
    [XmlIgnore]
    public List<TemporaryAnimatedSprite> temporarySprites = new List<TemporaryAnimatedSprite>();
    [XmlIgnore]
    public List<Warp> warps = new List<Warp>();
    [XmlIgnore]
    public Dictionary<Point, string> doors = new Dictionary<Point, string>();
    [XmlIgnore]
    public List<Farmer> farmers = new List<Farmer>();
    [XmlIgnore]
    public List<Projectile> projectiles = new List<Projectile>();
    public SerializableDictionary<Vector2, TerrainFeature> terrainFeatures = new SerializableDictionary<Vector2, TerrainFeature>();
    public List<Debris> debris = new List<Debris>();
    [XmlIgnore]
    public Point fishSplashPoint = Point.Zero;
    [XmlIgnore]
    public Point orePanPoint = Point.Zero;
    public Color waterColor = Color.White * 0.33f;
    [XmlIgnore]
    public Vector2 lastTouchActionLocation = Vector2.Zero;
    private List<Vector2> terrainFeaturesToRemoveList = new List<Vector2>();
    [XmlIgnore]
    public List<Vector2> lightGlows = new List<Vector2>();
    public const int minDailyWeeds = 5;
    public const int maxDailyWeeds = 12;
    public const int minDailyObjectSpawn = 1;
    public const int maxDailyObjectSpawn = 4;
    public const int maxSpawnedObjectsAtOnce = 6;
    public const int maxTriesForDebrisPlacement = 3;
    public const int maxTriesForObjectSpawn = 6;
    public const double chanceForStumpOrBoulderRespawn = 0.2;
    public const double chanceForClay = 0.03;
    public const int forageDataIndex = 0;
    public const int fishDataIndex = 4;
    public const int diggablesDataIndex = 8;
    private GameLocation.afterQuestionBehavior afterQuestion;
    [XmlIgnore]
    public Map map;
    [XmlIgnore]
    public Dictionary<Point, TemporaryAnimatedSprite> doorSprites;
    public List<LargeTerrainFeature> largeTerrainFeatures;
    protected List<Critter> critters;
    [XmlIgnore]
    public TemporaryAnimatedSprite fishSplashAnimation;
    [XmlIgnore]
    public TemporaryAnimatedSprite orePanAnimation;
    [XmlIgnore]
    public bool[,] waterTiles;
    [XmlIgnore]
    public string uniqueName;
    public string name;
    [XmlIgnore]
    public string lastQuestionKey;
    protected float lightLevel;
    public bool isFarm;
    public bool isOutdoors;
    public bool isStructure;
    public bool ignoreDebrisWeather;
    public bool ignoreOutdoorLighting;
    public bool ignoreLights;
    public bool treatAsOutdoors;
    protected bool wasUpdated;
    public int numberOfSpawnedObjectsOnMap;
    [XmlIgnore]
    public Event currentEvent;
    [XmlIgnore]
    public Object actionObjectForQuestionDialogue;
    [XmlIgnore]
    public int waterAnimationIndex;
    [XmlIgnore]
    public int waterAnimationTimer;
    [XmlIgnore]
    public bool waterTileFlip;
    [XmlIgnore]
    public bool forceViewportPlayerFollow;
    private float waterPosition;
    private Vector2 snowPos;
    private const int fireIDBase = 944468;

    [XmlIgnore]
    public float LightLevel
    {
      get
      {
        return this.lightLevel;
      }
      set
      {
        this.lightLevel = value;
      }
    }

    [XmlIgnore]
    public Map Map
    {
      get
      {
        return this.map;
      }
      set
      {
        this.map = value;
      }
    }

    [XmlIgnore]
    public SerializableDictionary<Vector2, Object> Objects
    {
      get
      {
        return this.objects;
      }
    }

    [XmlIgnore]
    public List<TemporaryAnimatedSprite> TemporarySprites
    {
      get
      {
        return this.temporarySprites;
      }
    }

    public string Name
    {
      get
      {
        return this.name;
      }
    }

    [XmlIgnore]
    public bool IsFarm
    {
      get
      {
        return this.isFarm;
      }
      set
      {
        this.isFarm = value;
      }
    }

    [XmlIgnore]
    public bool IsOutdoors
    {
      get
      {
        return this.isOutdoors;
      }
      set
      {
        this.isOutdoors = value;
      }
    }

    public GameLocation()
    {
    }

    public GameLocation(Map map, string name)
    {
      this.map = map;
      this.name = name;
      if (name.Contains("Farm") || name.Contains("Coop") || (name.Contains("Barn") || name.Equals("SlimeHutch")))
        this.isFarm = true;
      this.loadObjects();
      this.objects.CollectionChanged += new NotifyCollectionChangedEventHandler(this.objectCollectionChanged);
      this.terrainFeatures.CollectionChanged += new NotifyCollectionChangedEventHandler(this.terrainFeaturesCollectionChanged);
      if ((this.isOutdoors || this is Sewer) && !(this is Desert))
      {
        this.waterTiles = new bool[map.Layers[0].LayerWidth, map.Layers[0].LayerHeight];
        bool flag = false;
        for (int xTile = 0; xTile < map.Layers[0].LayerWidth; ++xTile)
        {
          for (int yTile = 0; yTile < map.Layers[0].LayerHeight; ++yTile)
          {
            if (this.doesTileHaveProperty(xTile, yTile, "Water", "Back") != null)
            {
              flag = true;
              this.waterTiles[xTile, yTile] = true;
            }
          }
        }
        if (!flag)
          this.waterTiles = (bool[,]) null;
      }
      if (!this.isOutdoors)
        return;
      this.critters = new List<Critter>();
    }

    public virtual bool canSlimeMateHere()
    {
      return true;
    }

    public virtual bool canSlimeHatchHere()
    {
      return true;
    }

    public void objectCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (!Game1.IsServer || Game1.server.connectionsCount <= 0)
        return;
      switch (e.Action)
      {
        case NotifyCollectionChangedAction.Add:
          Vector2 newItem = (Vector2) e.NewItems[0];
          MultiplayerUtility.broadcastObjectChange((short) newItem.X, (short) newItem.Y, (byte) 0, this.objects[newItem].bigCraftable ? (byte) 1 : (byte) 0, (int) (short) this.objects[newItem].ParentSheetIndex, this.name);
          break;
        case NotifyCollectionChangedAction.Remove:
          Vector2 oldItem = (Vector2) e.OldItems[0];
          MultiplayerUtility.broadcastObjectChange((short) oldItem.X, (short) oldItem.Y, (byte) 1, (byte) 0, -1, this.name);
          break;
      }
    }

    public void terrainFeaturesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (!Game1.IsServer || Game1.server.connectionsCount <= 0)
        return;
      switch (e.Action)
      {
        case NotifyCollectionChangedAction.Add:
          Vector2 newItem = (Vector2) e.NewItems[0];
          TerrainFeatureDescription fromTerrainFeature = TerrainFeatureFactory.getIndexFromTerrainFeature(this.terrainFeatures[newItem]);
          MultiplayerUtility.broadcastObjectChange((short) newItem.X, (short) newItem.Y, (byte) 2, fromTerrainFeature.index, fromTerrainFeature.extraInfo, this.name);
          break;
        case NotifyCollectionChangedAction.Remove:
          Vector2 oldItem = (Vector2) e.OldItems[0];
          MultiplayerUtility.broadcastObjectChange((short) oldItem.X, (short) oldItem.Y, (byte) 3, (byte) 0, -1, this.name);
          break;
      }
    }

    public void addCharacter(NPC character)
    {
      this.characters.Add(character);
    }

    public List<NPC> getCharacters()
    {
      return this.characters;
    }

    public Microsoft.Xna.Framework.Rectangle getSourceRectForObject(int tileIndex)
    {
      return new Microsoft.Xna.Framework.Rectangle(tileIndex * 16 % Game1.objectSpriteSheet.Width, tileIndex * 16 / Game1.objectSpriteSheet.Width * 16, 16, 16);
    }

    public Warp isCollidingWithWarp(Microsoft.Xna.Framework.Rectangle position)
    {
      foreach (Warp warp in this.warps)
      {
        if ((warp.X == (int) Math.Floor((double) position.Left / (double) Game1.tileSize) || warp.X == (int) Math.Floor((double) position.Right / (double) Game1.tileSize)) && (warp.Y == (int) Math.Floor((double) position.Top / (double) Game1.tileSize) || warp.Y == (int) Math.Floor((double) position.Bottom / (double) Game1.tileSize)))
          return warp;
      }
      return (Warp) null;
    }

    public Warp isCollidingWithWarpOrDoor(Microsoft.Xna.Framework.Rectangle position)
    {
      return this.isCollidingWithWarp(position) ?? this.isCollidingWithDoors(position);
    }

    public Warp isCollidingWithDoors(Microsoft.Xna.Framework.Rectangle position)
    {
      for (int corner = 0; corner < 4; ++corner)
      {
        Vector2 cornersOfThisRectangle = Utility.getCornersOfThisRectangle(ref position, corner);
        Point point = new Point((int) cornersOfThisRectangle.X / Game1.tileSize, (int) cornersOfThisRectangle.Y / Game1.tileSize);
        foreach (KeyValuePair<Point, string> door in this.doors)
        {
          Point key = door.Key;
          if (point.Equals(key))
            return this.getWarpFromDoor(key);
        }
      }
      return (Warp) null;
    }

    public Warp getWarpFromDoor(Point door)
    {
      string[] strArray = this.doesTileHaveProperty(door.X, door.Y, "Action", "Buildings").Split(' ');
      if (strArray[0].Equals("WarpCommunityCenter"))
        return new Warp(door.X, door.Y, "CommunityCenter", 32, 23, false);
      return new Warp(door.X, door.Y, strArray[3], Convert.ToInt32(strArray[1]), Convert.ToInt32(strArray[2]), false);
    }

    public bool isFarmerCollidingWithAnyCharacter()
    {
      foreach (NPC character in this.characters)
      {
        if (character != null && Game1.player.GetBoundingBox().Intersects(character.GetBoundingBox()))
          return true;
      }
      return false;
    }

    public bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer)
    {
      return this.isCollidingPosition(position, viewport, isFarmer, 0, false, (Character) null, false, false, false);
    }

    public bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, Character character)
    {
      return this.isCollidingPosition(position, viewport, false, 0, false, character, false, false, false);
    }

    public bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer, int damagesFarmer, bool glider)
    {
      return this.isCollidingPosition(position, viewport, isFarmer, damagesFarmer, glider, (Character) null, false, false, false);
    }

    public virtual bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer, int damagesFarmer, bool glider, Character character)
    {
      return this.isCollidingPosition(position, viewport, isFarmer, damagesFarmer, glider, character, false, false, false);
    }

    public virtual bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer, int damagesFarmer, bool glider, Character character, bool pathfinding, bool projectile = false, bool ignoreCharacterRequirement = false)
    {
      if (position.Right < 0 || position.X > this.map.Layers[0].DisplayWidth || (position.Bottom < 0 || position.Top > this.map.Layers[0].DisplayHeight))
        return false;
      Vector2 index1 = new Vector2((float) (position.Right / Game1.tileSize), (float) (position.Top / Game1.tileSize));
      Vector2 index2 = new Vector2((float) (position.Left / Game1.tileSize), (float) (position.Top / Game1.tileSize));
      Vector2 index3 = new Vector2((float) (position.Right / Game1.tileSize), (float) (position.Bottom / Game1.tileSize));
      Vector2 index4 = new Vector2((float) (position.Left / Game1.tileSize), (float) (position.Bottom / Game1.tileSize));
      bool flag = position.Width > Game1.tileSize;
      Vector2 index5 = new Vector2((float) (position.Center.X / Game1.tileSize), (float) (position.Bottom / Game1.tileSize));
      Vector2 index6 = new Vector2((float) (position.Center.X / Game1.tileSize), (float) (position.Top / Game1.tileSize));
      if (character == null && !ignoreCharacterRequirement)
        return true;
      Microsoft.Xna.Framework.Rectangle boundingBox;
      if (!glider && (!Game1.eventUp || character != null && character.GetType() != typeof (Character) && !isFarmer && (!pathfinding || !character.willDestroyObjectsUnderfoot)))
      {
        Object o;
        this.objects.TryGetValue(index1, out o);
        if (o != null && !o.IsHoeDirt && (!o.isPassable() && !Game1.player.temporaryImpassableTile.Intersects(o.getBoundingBox(index1))))
        {
          boundingBox = o.getBoundingBox(index1);
          if (boundingBox.Intersects(position) && character != null && (character.GetType() != typeof (FarmAnimal) || !o.isAnimalProduct()) && character.collideWith(o))
            return true;
        }
        this.objects.TryGetValue(index3, out o);
        if (o != null && !o.IsHoeDirt && (!o.isPassable() && !Game1.player.temporaryImpassableTile.Intersects(o.getBoundingBox(index3))))
        {
          boundingBox = o.getBoundingBox(index3);
          if (boundingBox.Intersects(position) && character != null && (character.GetType() != typeof (FarmAnimal) || !o.isAnimalProduct()) && character.collideWith(o))
            return true;
        }
        this.objects.TryGetValue(index2, out o);
        if (o != null && !o.IsHoeDirt && (!o.isPassable() && !Game1.player.temporaryImpassableTile.Intersects(o.getBoundingBox(index2))))
        {
          boundingBox = o.getBoundingBox(index2);
          if (boundingBox.Intersects(position) && character != null && (character.GetType() != typeof (FarmAnimal) || !o.isAnimalProduct()) && character.collideWith(o))
            return true;
        }
        this.objects.TryGetValue(index4, out o);
        if (o != null && !o.IsHoeDirt && (!o.isPassable() && !Game1.player.temporaryImpassableTile.Intersects(o.getBoundingBox(index4))))
        {
          boundingBox = o.getBoundingBox(index4);
          if (boundingBox.Intersects(position) && character != null && (character.GetType() != typeof (FarmAnimal) || !o.isAnimalProduct()) && character.collideWith(o))
            return true;
        }
        if (flag)
        {
          this.objects.TryGetValue(index5, out o);
          if (o != null && !o.IsHoeDirt && (!o.isPassable() && !Game1.player.temporaryImpassableTile.Intersects(o.getBoundingBox(index5))))
          {
            boundingBox = o.getBoundingBox(index5);
            if (boundingBox.Intersects(position) && (character.GetType() != typeof (FarmAnimal) || !o.isAnimalProduct()) && character.collideWith(o))
              return true;
          }
          this.objects.TryGetValue(index6, out o);
          if (o != null && !o.IsHoeDirt && (!o.isPassable() && !Game1.player.temporaryImpassableTile.Intersects(o.getBoundingBox(index6))))
          {
            boundingBox = o.getBoundingBox(index6);
            if (boundingBox.Intersects(position) && (character.GetType() != typeof (FarmAnimal) || !o.isAnimalProduct()) && character.collideWith(o))
              return true;
          }
        }
      }
      if (this.largeTerrainFeatures != null && !glider)
      {
        foreach (LargeTerrainFeature largeTerrainFeature in this.largeTerrainFeatures)
        {
          boundingBox = largeTerrainFeature.getBoundingBox();
          if (boundingBox.Intersects(position))
            return true;
        }
      }
      if (!Game1.eventUp && !glider)
      {
        if (this.terrainFeatures.ContainsKey(index1))
        {
          boundingBox = this.terrainFeatures[index1].getBoundingBox(index1);
          if (boundingBox.Intersects(position))
          {
            if (!pathfinding)
              this.terrainFeatures[index1].doCollisionAction(position, Game1.player.speed + Game1.player.addedSpeed, index1, character, this);
            if (this.terrainFeatures.ContainsKey(index1) && !this.terrainFeatures[index1].isPassable(character))
              return true;
          }
        }
        if (this.terrainFeatures.ContainsKey(index2))
        {
          boundingBox = this.terrainFeatures[index2].getBoundingBox(index2);
          if (boundingBox.Intersects(position))
          {
            if (!pathfinding)
              this.terrainFeatures[index2].doCollisionAction(position, Game1.player.speed + Game1.player.addedSpeed, index2, character, this);
            if (this.terrainFeatures.ContainsKey(index2) && !this.terrainFeatures[index2].isPassable(character))
              return true;
          }
        }
        if (this.terrainFeatures.ContainsKey(index3))
        {
          boundingBox = this.terrainFeatures[index3].getBoundingBox(index3);
          if (boundingBox.Intersects(position))
          {
            if (!pathfinding)
              this.terrainFeatures[index3].doCollisionAction(position, Game1.player.speed + Game1.player.addedSpeed, index3, character, this);
            if (this.terrainFeatures.ContainsKey(index3) && !this.terrainFeatures[index3].isPassable(character))
              return true;
          }
        }
        if (this.terrainFeatures.ContainsKey(index4))
        {
          boundingBox = this.terrainFeatures[index4].getBoundingBox(index4);
          if (boundingBox.Intersects(position))
          {
            if (!pathfinding)
              this.terrainFeatures[index4].doCollisionAction(position, Game1.player.speed + Game1.player.addedSpeed, index4, character, this);
            if (this.terrainFeatures.ContainsKey(index4) && !this.terrainFeatures[index4].isPassable(character))
              return true;
          }
        }
        if (flag)
        {
          if (this.terrainFeatures.ContainsKey(index5))
          {
            boundingBox = this.terrainFeatures[index5].getBoundingBox(index5);
            if (boundingBox.Intersects(position))
            {
              if (!pathfinding)
                this.terrainFeatures[index5].doCollisionAction(position, Game1.player.speed + Game1.player.addedSpeed, index5, character, this);
              if (this.terrainFeatures.ContainsKey(index5) && !this.terrainFeatures[index5].isPassable((Character) null))
                return true;
            }
          }
          if (this.terrainFeatures.ContainsKey(index6))
          {
            boundingBox = this.terrainFeatures[index6].getBoundingBox(index6);
            if (boundingBox.Intersects(position))
            {
              if (!pathfinding)
                this.terrainFeatures[index6].doCollisionAction(position, Game1.player.speed + Game1.player.addedSpeed, index6, character, this);
              if (this.terrainFeatures.ContainsKey(index6) && !this.terrainFeatures[index6].isPassable((Character) null))
                return true;
            }
          }
        }
      }
      if (character != null && character.hasSpecialCollisionRules() && (character.isColliding(this, index1) || character.isColliding(this, index2) || (character.isColliding(this, index3) || character.isColliding(this, index4))))
        return true;
      if (isFarmer || character != null && character.collidesWithOtherCharacters)
      {
        for (int index7 = this.characters.Count - 1; index7 >= 0; --index7)
        {
          if (this.characters[index7] != null && (character == null || !character.Equals((object) this.characters[index7])))
          {
            boundingBox = this.characters[index7].GetBoundingBox();
            if (boundingBox.Intersects(position) && !Game1.player.temporarilyInvincible)
              this.characters[index7].behaviorOnFarmerPushing();
            if (isFarmer && !Game1.eventUp && !this.characters[index7].farmerPassesThrough)
            {
              boundingBox = this.characters[index7].GetBoundingBox();
              if (boundingBox.Intersects(position) && !Game1.player.temporarilyInvincible && Game1.player.temporaryImpassableTile.Equals(Microsoft.Xna.Framework.Rectangle.Empty))
              {
                if (this.characters[index7].IsMonster)
                {
                  if (!((Monster) this.characters[index7]).isGlider)
                  {
                    boundingBox = Game1.player.GetBoundingBox();
                    if (boundingBox.Intersects(this.characters[index7].GetBoundingBox()))
                      goto label_83;
                  }
                  else
                    goto label_83;
                }
                if (!this.characters[index7].isInvisible)
                  return true;
              }
            }
label_83:
            if (!isFarmer)
            {
              boundingBox = this.characters[index7].GetBoundingBox();
              if (boundingBox.Intersects(position))
                return true;
            }
          }
        }
      }
      if (isFarmer)
      {
        if (this.currentEvent != null && this.currentEvent.checkForCollision(position, character != null ? character as Farmer : Game1.player) || Game1.player.currentUpgrade != null && Game1.player.currentUpgrade.daysLeftTillUpgradeDone <= 3 && (this.name.Equals("Farm") && position.Intersects(new Microsoft.Xna.Framework.Rectangle((int) Game1.player.currentUpgrade.positionOfCarpenter.X, (int) Game1.player.currentUpgrade.positionOfCarpenter.Y + Game1.tileSize, Game1.tileSize, Game1.tileSize / 2))))
          return true;
      }
      else
      {
        if (position.Intersects(Game1.player.GetBoundingBox()))
        {
          if (damagesFarmer > 0)
          {
            if (Game1.player.CurrentTool != null && Game1.player.CurrentTool is MeleeWeapon && (((MeleeWeapon) Game1.player.CurrentTool).isOnSpecial && ((MeleeWeapon) Game1.player.CurrentTool).type == 3))
            {
              Game1.farmerTakeDamage(damagesFarmer, false, character as Monster);
              return true;
            }
            if (character != null)
              character.collisionWithFarmerBehavior();
            Game1.farmerTakeDamage(Math.Max(1, damagesFarmer + Game1.random.Next(-damagesFarmer / 4, damagesFarmer / 4)), false, character as Monster);
            if (!glider && (character == null || !(character as Monster).passThroughCharacters()))
              return true;
          }
          else if (!glider && (!pathfinding || !(character is Monster)))
            return true;
        }
        if (damagesFarmer > 0 && !glider)
        {
          for (int index7 = 0; index7 < this.characters.Count; ++index7)
          {
            if (!this.characters[index7].Equals((object) character) && position.Intersects(this.characters[index7].GetBoundingBox()) && (!(character is GreenSlime) && !(character is DustSpirit)))
              return true;
          }
        }
        if ((this.isFarm || this.name.Equals("UndergroundMine")) && (character != null && !character.name.Contains("NPC")) && (!character.eventActor && !glider))
        {
          PropertyValue propertyValue = (PropertyValue) null;
          Tile tile1 = this.map.GetLayer("Back").PickTile(new Location(position.Right, position.Top), viewport.Size);
          if (tile1 != null)
            tile1.Properties.TryGetValue("NPCBarrier", out propertyValue);
          if (propertyValue != null)
            return true;
          Tile tile2 = this.map.GetLayer("Back").PickTile(new Location(position.Right, position.Bottom), viewport.Size);
          if (tile2 != null)
            tile2.Properties.TryGetValue("NPCBarrier", out propertyValue);
          if (propertyValue != null)
            return true;
          Tile tile3 = this.map.GetLayer("Back").PickTile(new Location(position.Left, position.Top), viewport.Size);
          if (tile3 != null)
            tile3.Properties.TryGetValue("NPCBarrier", out propertyValue);
          if (propertyValue != null)
            return true;
          Tile tile4 = this.map.GetLayer("Back").PickTile(new Location(position.Left, position.Bottom), viewport.Size);
          if (tile4 != null)
            tile4.Properties.TryGetValue("NPCBarrier", out propertyValue);
          if (propertyValue != null)
            return true;
        }
        if (glider && !projectile)
          return false;
      }
      if (!isFarmer || !Game1.player.isRafting)
      {
        PropertyValue propertyValue = (PropertyValue) null;
        Tile tile1 = this.map.GetLayer("Back").PickTile(new Location(position.Right, position.Top), viewport.Size);
        if (tile1 != null)
          tile1.TileIndexProperties.TryGetValue("Passable", out propertyValue);
        if (propertyValue != null && (!isFarmer || !Game1.player.temporaryImpassableTile.Contains(position.Right, position.Top)))
          return true;
        Tile tile2 = this.map.GetLayer("Back").PickTile(new Location(position.Right, position.Bottom), viewport.Size);
        if (tile2 != null)
          tile2.TileIndexProperties.TryGetValue("Passable", out propertyValue);
        if (propertyValue != null && (!isFarmer || !Game1.player.temporaryImpassableTile.Contains(position.Right, position.Bottom)))
          return true;
        Tile tile3 = this.map.GetLayer("Back").PickTile(new Location(position.Left, position.Top), viewport.Size);
        if (tile3 != null)
          tile3.TileIndexProperties.TryGetValue("Passable", out propertyValue);
        if (propertyValue != null && (!isFarmer || !Game1.player.temporaryImpassableTile.Contains(position.Left, position.Top)))
          return true;
        Tile tile4 = this.map.GetLayer("Back").PickTile(new Location(position.Left, position.Bottom), viewport.Size);
        if (tile4 != null)
          tile4.TileIndexProperties.TryGetValue("Passable", out propertyValue);
        if (propertyValue != null && (!isFarmer || !Game1.player.temporaryImpassableTile.Contains(position.Left, position.Bottom)))
          return true;
        if (flag)
        {
          Tile tile5 = this.map.GetLayer("Back").PickTile(new Location(position.Center.X, position.Bottom), viewport.Size);
          if (tile5 != null)
            tile5.TileIndexProperties.TryGetValue("Passable", out propertyValue);
          if (propertyValue != null && (!isFarmer || !Game1.player.temporaryImpassableTile.Contains(position.Center.X, position.Bottom)))
            return true;
          Tile tile6 = this.map.GetLayer("Back").PickTile(new Location(position.Center.X, position.Top), viewport.Size);
          if (tile6 != null)
            tile6.TileIndexProperties.TryGetValue("Passable", out propertyValue);
          if (propertyValue != null && (!isFarmer || !Game1.player.temporaryImpassableTile.Contains(position.Center.X, position.Top)))
            return true;
        }
        Tile tile7 = this.map.GetLayer("Buildings").PickTile(new Location(position.Right, position.Top), viewport.Size);
        if (tile7 != null)
        {
          tile7.TileIndexProperties.TryGetValue("Shadow", out propertyValue);
          if (propertyValue == null)
            tile7.TileIndexProperties.TryGetValue("Passable", out propertyValue);
          if (propertyValue == null && !isFarmer)
            tile7.TileIndexProperties.TryGetValue("NPCPassable", out propertyValue);
          if (propertyValue == null && !isFarmer && (character != null && character.canPassThroughActionTiles()))
            tile7.Properties.TryGetValue("Action", out propertyValue);
          if ((propertyValue == null || propertyValue.ToString().Length == 0) && (!isFarmer || !Game1.player.temporaryImpassableTile.Contains(position.Right, position.Top)))
          {
            if (character == null)
              return true;
            return character.shouldCollideWithBuildingLayer(this);
          }
        }
        Tile tile8 = this.map.GetLayer("Buildings").PickTile(new Location(position.Right, position.Bottom), viewport.Size);
        if (tile8 != null && propertyValue == null | isFarmer)
        {
          tile8.TileIndexProperties.TryGetValue("Shadow", out propertyValue);
          if (propertyValue == null)
            tile8.TileIndexProperties.TryGetValue("Passable", out propertyValue);
          if (propertyValue == null && !isFarmer)
            tile8.TileIndexProperties.TryGetValue("NPCPassable", out propertyValue);
          if (propertyValue == null && !isFarmer && (character != null && character.canPassThroughActionTiles()))
            tile8.Properties.TryGetValue("Action", out propertyValue);
          if ((propertyValue == null || propertyValue.ToString().Length == 0) && (!isFarmer || !Game1.player.temporaryImpassableTile.Contains(position.Right, position.Bottom)))
          {
            if (character == null)
              return true;
            return character.shouldCollideWithBuildingLayer(this);
          }
        }
        Tile tile9 = this.map.GetLayer("Buildings").PickTile(new Location(position.Left, position.Top), viewport.Size);
        if (tile9 != null && propertyValue == null | isFarmer)
        {
          tile9.TileIndexProperties.TryGetValue("Shadow", out propertyValue);
          if (propertyValue == null)
            tile9.TileIndexProperties.TryGetValue("Passable", out propertyValue);
          if (propertyValue == null && !isFarmer)
            tile9.TileIndexProperties.TryGetValue("NPCPassable", out propertyValue);
          if (propertyValue == null && !isFarmer && (character != null && character.canPassThroughActionTiles()))
            tile9.Properties.TryGetValue("Action", out propertyValue);
          if ((propertyValue == null || propertyValue.ToString().Length == 0) && (!isFarmer || !Game1.player.temporaryImpassableTile.Contains(position.Left, position.Top)))
          {
            if (character == null)
              return true;
            return character.shouldCollideWithBuildingLayer(this);
          }
        }
        Tile tile10 = this.map.GetLayer("Buildings").PickTile(new Location(position.Left, position.Bottom), viewport.Size);
        if (tile10 != null && propertyValue == null | isFarmer)
        {
          tile10.TileIndexProperties.TryGetValue("Shadow", out propertyValue);
          if (propertyValue == null)
            tile10.TileIndexProperties.TryGetValue("Passable", out propertyValue);
          if (propertyValue == null && !isFarmer)
            tile10.TileIndexProperties.TryGetValue("NPCPassable", out propertyValue);
          if (propertyValue == null && !isFarmer && (character != null && character.canPassThroughActionTiles()))
            tile10.Properties.TryGetValue("Action", out propertyValue);
          if ((propertyValue == null || propertyValue.ToString().Length == 0) && (!isFarmer || !Game1.player.temporaryImpassableTile.Contains(position.Left, position.Bottom)))
          {
            if (character == null)
              return true;
            return character.shouldCollideWithBuildingLayer(this);
          }
        }
        if (flag)
        {
          Tile tile5 = this.map.GetLayer("Buildings").PickTile(new Location(position.Center.X, position.Top), viewport.Size);
          if (tile5 != null && propertyValue == null | isFarmer)
          {
            tile5.TileIndexProperties.TryGetValue("Shadow", out propertyValue);
            if (propertyValue == null)
              tile5.TileIndexProperties.TryGetValue("Passable", out propertyValue);
            if (propertyValue == null && !isFarmer)
              tile5.TileIndexProperties.TryGetValue("NPCPassable", out propertyValue);
            if (propertyValue == null && !isFarmer && (character != null && character.canPassThroughActionTiles()))
              tile5.Properties.TryGetValue("Action", out propertyValue);
            if ((propertyValue == null || propertyValue.ToString().Length == 0) && (!isFarmer || !Game1.player.temporaryImpassableTile.Contains(position.Center.X, position.Top)))
            {
              if (character == null)
                return true;
              return character.shouldCollideWithBuildingLayer(this);
            }
          }
          Tile tile6 = this.map.GetLayer("Buildings").PickTile(new Location(position.Center.X, position.Bottom), viewport.Size);
          if (tile6 != null && propertyValue == null | isFarmer)
          {
            tile6.TileIndexProperties.TryGetValue("Shadow", out propertyValue);
            if (propertyValue == null)
              tile6.TileIndexProperties.TryGetValue("Passable", out propertyValue);
            if (propertyValue == null && !isFarmer)
              tile6.TileIndexProperties.TryGetValue("NPCPassable", out propertyValue);
            if (propertyValue == null && !isFarmer && (character != null && character.canPassThroughActionTiles()))
              tile6.Properties.TryGetValue("Action", out propertyValue);
            if ((propertyValue == null || propertyValue.ToString().Length == 0) && (!isFarmer || !Game1.player.temporaryImpassableTile.Contains(position.Center.X, position.Bottom)))
            {
              if (character == null)
                return true;
              return character.shouldCollideWithBuildingLayer(this);
            }
          }
        }
        if (!isFarmer && propertyValue != null)
        {
          if (propertyValue.ToString().Split(' ')[0] == "Door")
          {
            this.openDoor(new Location(position.Center.X / Game1.tileSize, position.Bottom / Game1.tileSize), false);
            this.openDoor(new Location(position.Center.X / Game1.tileSize, position.Top / Game1.tileSize), Game1.currentLocation.Equals((object) this));
          }
        }
        return false;
      }
      PropertyValue propertyValue1 = (PropertyValue) null;
      Tile tile11 = this.map.GetLayer("Back").PickTile(new Location(position.Right, position.Top), viewport.Size);
      if (tile11 != null)
        tile11.TileIndexProperties.TryGetValue("Water", out propertyValue1);
      if (propertyValue1 == null)
      {
        if (this.isTileLocationOpen(new Location(position.Right, position.Top)) && !this.isTileOccupiedForPlacement(new Vector2((float) (position.Right / Game1.tileSize), (float) (position.Top / Game1.tileSize)), (Object) null))
        {
          Game1.player.isRafting = false;
          Game1.player.position = new Vector2((float) (position.Right / Game1.tileSize * Game1.tileSize), (float) (position.Top / Game1.tileSize * Game1.tileSize - Game1.tileSize / 2));
          Game1.player.setTrajectory(0, 0);
        }
        return true;
      }
      Tile tile12 = this.map.GetLayer("Back").PickTile(new Location(position.Right, position.Bottom), viewport.Size);
      if (tile12 != null)
        tile12.TileIndexProperties.TryGetValue("Water", out propertyValue1);
      if (propertyValue1 == null)
      {
        if (this.isTileLocationOpen(new Location(position.Right, position.Bottom)) && !this.isTileOccupiedForPlacement(new Vector2((float) (position.Right / Game1.tileSize), (float) (position.Bottom / Game1.tileSize)), (Object) null))
        {
          Game1.player.isRafting = false;
          Game1.player.position = new Vector2((float) (position.Right / Game1.tileSize * Game1.tileSize), (float) (position.Bottom / Game1.tileSize * Game1.tileSize - Game1.tileSize / 2));
          Game1.player.setTrajectory(0, 0);
        }
        return true;
      }
      Tile tile13 = this.map.GetLayer("Back").PickTile(new Location(position.Left, position.Top), viewport.Size);
      if (tile13 != null)
        tile13.TileIndexProperties.TryGetValue("Water", out propertyValue1);
      if (propertyValue1 == null)
      {
        if (this.isTileLocationOpen(new Location(position.Left, position.Top)) && !this.isTileOccupiedForPlacement(new Vector2((float) (position.Left / Game1.tileSize), (float) (position.Top / Game1.tileSize)), (Object) null))
        {
          Game1.player.isRafting = false;
          Game1.player.position = new Vector2((float) (position.Left / Game1.tileSize * Game1.tileSize), (float) (position.Top / Game1.tileSize * Game1.tileSize - Game1.tileSize / 2));
          Game1.player.setTrajectory(0, 0);
        }
        return true;
      }
      Tile tile14 = this.map.GetLayer("Back").PickTile(new Location(position.Left, position.Bottom), viewport.Size);
      if (tile14 != null)
        tile14.TileIndexProperties.TryGetValue("Water", out propertyValue1);
      if (propertyValue1 != null)
        return false;
      if (this.isTileLocationOpen(new Location(position.Left, position.Bottom)) && !this.isTileOccupiedForPlacement(new Vector2((float) (position.Left / Game1.tileSize), (float) (position.Bottom / Game1.tileSize)), (Object) null))
      {
        Game1.player.isRafting = false;
        Game1.player.position = new Vector2((float) (position.Left / Game1.tileSize * Game1.tileSize), (float) (position.Bottom / Game1.tileSize * Game1.tileSize - Game1.tileSize / 2));
        Game1.player.setTrajectory(0, 0);
      }
      return true;
    }

    public bool isTilePassable(Location tileLocation, xTile.Dimensions.Rectangle viewport)
    {
      PropertyValue propertyValue = (PropertyValue) null;
      Tile tile1 = this.map.GetLayer("Back").PickTile(new Location(tileLocation.X * Game1.tileSize, tileLocation.Y * Game1.tileSize), viewport.Size);
      if (tile1 != null)
        tile1.TileIndexProperties.TryGetValue("Passable", out propertyValue);
      Tile tile2 = this.map.GetLayer("Buildings").PickTile(new Location(tileLocation.X * Game1.tileSize, tileLocation.Y * Game1.tileSize), viewport.Size);
      if (propertyValue == null && tile2 == null)
        return tile1 != null;
      return false;
    }

    public bool isPointPassable(Location location, xTile.Dimensions.Rectangle viewport)
    {
      PropertyValue propertyValue1 = (PropertyValue) null;
      PropertyValue propertyValue2 = (PropertyValue) null;
      Tile tile1 = this.map.GetLayer("Back").PickTile(new Location(location.X, location.Y), viewport.Size);
      if (tile1 != null)
        tile1.TileIndexProperties.TryGetValue("Passable", out propertyValue1);
      Tile tile2 = this.map.GetLayer("Buildings").PickTile(new Location(location.X, location.Y), viewport.Size);
      if (tile2 != null)
        tile2.TileIndexProperties.TryGetValue("Shadow", out propertyValue2);
      if (propertyValue1 != null)
        return false;
      if (tile2 != null)
        return propertyValue2 != null;
      return true;
    }

    public bool isTilePassable(Microsoft.Xna.Framework.Rectangle nextPosition, xTile.Dimensions.Rectangle viewport)
    {
      if (this.isPointPassable(new Location(nextPosition.Left, nextPosition.Top), viewport) && this.isPointPassable(new Location(nextPosition.Left, nextPosition.Bottom), viewport) && this.isPointPassable(new Location(nextPosition.Right, nextPosition.Top), viewport))
        return this.isPointPassable(new Location(nextPosition.Right, nextPosition.Bottom), viewport);
      return false;
    }

    public bool isTileOnMap(Vector2 position)
    {
      if ((double) position.X >= 0.0 && (double) position.X < (double) this.map.Layers[0].LayerWidth && (double) position.Y >= 0.0)
        return (double) position.Y < (double) this.map.Layers[0].LayerHeight;
      return false;
    }

    public bool isTileOnMap(int x, int y)
    {
      if (x >= 0 && x < this.map.Layers[0].LayerWidth && y >= 0)
        return y < this.map.Layers[0].LayerHeight;
      return false;
    }

    public void busLeave()
    {
      NPC npc = (NPC) null;
      for (int index = this.characters.Count - 1; index >= 0; --index)
      {
        if (this.characters[index].name.Equals("Pam"))
        {
          npc = this.characters[index];
          this.characters.RemoveAt(index);
          break;
        }
      }
      if (npc == null)
        return;
      Game1.changeMusicTrack("none");
      Game1.playSound("openBox");
      if (this.name.Equals("BusStop"))
      {
        Game1.warpFarmer("Desert", 32, 27, true);
        npc.followSchedule = false;
        npc.position = new Vector2((float) (31 * Game1.tileSize), (float) (28 * Game1.tileSize - Game1.tileSize / 2 - Game1.tileSize / 8));
        npc.faceDirection(2);
        npc.CurrentDialogue.Peek().temporaryDialogue = Game1.parseText(Game1.content.LoadString("Strings\\Locations:Desert_BusArrived"));
        Game1.getLocationFromName("Desert").characters.Add(npc);
      }
      else
      {
        npc.CurrentDialogue.Peek().temporaryDialogue = (string) null;
        Game1.warpFarmer("BusStop", 9, 9, true);
        if (Game1.timeOfDay >= 2300)
        {
          npc.position = new Vector2((float) (18 * Game1.tileSize), (float) (7 * Game1.tileSize - Game1.tileSize / 2 - Game1.tileSize / 8));
          npc.faceDirection(2);
          Game1.getLocationFromName("Trailer").characters.Add(npc);
        }
        else if (Game1.timeOfDay >= 1700)
        {
          npc.position = new Vector2((float) (7 * Game1.tileSize), (float) (18 * Game1.tileSize - Game1.tileSize / 2 - Game1.tileSize / 8));
          npc.faceDirection(1);
          Game1.getLocationFromName("Saloon").characters.Add(npc);
        }
        else
        {
          npc.position = new Vector2((float) (8 * Game1.tileSize), (float) (10 * Game1.tileSize - Game1.tileSize / 2 - Game1.tileSize / 8));
          npc.faceDirection(2);
          Game1.getLocationFromName("BusStop").characters.Add(npc);
          npc.sprite.CurrentFrame = 0;
        }
        npc.DirectionsToNewLocation = (SchedulePathDescription) null;
        npc.followSchedule = true;
      }
    }

    public int numberOfObjectsWithName(string name)
    {
      int num = 0;
      foreach (Item obj in this.objects.Values)
      {
        if (obj.Name.Equals(name))
          ++num;
      }
      return num;
    }

    public Point getWarpPointTo(string location)
    {
      foreach (Warp warp in this.warps)
      {
        if (warp.TargetName.Equals(location))
          return new Point(warp.X, warp.Y);
      }
      foreach (KeyValuePair<Point, string> door in this.doors)
      {
        if (door.Value.Equals(location))
          return door.Key;
      }
      return Point.Zero;
    }

    public Point getWarpPointTarget(Point warpPointLocation)
    {
      foreach (Warp warp in this.warps)
      {
        if (warp.X == warpPointLocation.X && warp.Y == warpPointLocation.Y)
          return new Point(warp.TargetX, warp.TargetY);
      }
      foreach (KeyValuePair<Point, string> door in this.doors)
      {
        if (door.Key.Equals(warpPointLocation))
        {
          string str = this.doesTileHaveProperty(warpPointLocation.X, warpPointLocation.Y, "Action", "Buildings");
          if (str != null && str.Contains("Warp"))
          {
            string[] strArray = str.Split(' ');
            if (strArray[0].Equals("WarpCommunityCenter"))
              return new Point(32, 23);
            return new Point(Convert.ToInt32(strArray[1]), Convert.ToInt32(strArray[2]));
          }
        }
      }
      return Point.Zero;
    }

    public void boardBus(Vector2 playerTileLocation)
    {
      if (!Game1.player.hasBusTicket && !this.name.Equals("Desert"))
        return;
      bool flag = false;
      int num = this.characters.Count - 1;
      while (num >= 0)
        --num;
      if (flag)
      {
        Game1.player.hasBusTicket = false;
        Game1.player.CanMove = false;
        Game1.viewportFreeze = true;
        Game1.player.position.X = -99999f;
        Game1.boardingBus = true;
      }
      else
        Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Bus_NoDriver"));
    }

    public NPC doesPositionCollideWithCharacter(float x, float y)
    {
      foreach (NPC character in this.characters)
      {
        if (character.GetBoundingBox().Contains((int) x, (int) y))
          return character;
      }
      return (NPC) null;
    }

    public NPC doesPositionCollideWithCharacter(Microsoft.Xna.Framework.Rectangle r, bool ignoreMonsters = false)
    {
      foreach (NPC character in this.characters)
      {
        if (character.GetBoundingBox().Intersects(r) && (!character.IsMonster || !ignoreMonsters))
          return character;
      }
      return (NPC) null;
    }

    public void switchOutNightTiles()
    {
      try
      {
        PropertyValue propertyValue;
        this.map.Properties.TryGetValue("NightTiles", out propertyValue);
        if (propertyValue != null)
        {
          string[] strArray = propertyValue.ToString().Split(' ');
          int index = 0;
          while (index < strArray.Length)
          {
            this.map.GetLayer(strArray[index]).Tiles[Convert.ToInt32(strArray[index + 1]), Convert.ToInt32(strArray[index + 2])].TileIndex = Convert.ToInt32(strArray[index + 3]);
            index += 4;
          }
        }
      }
      catch (Exception ex)
      {
      }
      if (this is MineShaft)
        return;
      this.lightGlows.Clear();
    }

    public virtual void checkForMusic(GameTime time)
    {
      if (this.IsOutdoors && Game1.currentSong != null && (!Game1.currentSong.IsPlaying && !Game1.isRaining) && !Game1.eventUp)
      {
        if (!Game1.isDarkOut())
        {
          string currentSeason = Game1.currentSeason;
          if (!(currentSeason == "spring"))
          {
            if (!(currentSeason == "summer"))
            {
              if (!(currentSeason == "fall"))
              {
                if (currentSeason == "winter")
                  Game1.changeMusicTrack("winter_day_ambient");
              }
              else
                Game1.changeMusicTrack("fall_day_ambient");
            }
            else
              Game1.changeMusicTrack("summer_day_ambient");
          }
          else
            Game1.changeMusicTrack("spring_day_ambient");
        }
        else if (Game1.isDarkOut() && Game1.timeOfDay < 2500)
        {
          string currentSeason = Game1.currentSeason;
          if (!(currentSeason == "spring"))
          {
            if (!(currentSeason == "summer"))
            {
              if (currentSeason == "fall")
                Game1.changeMusicTrack("spring_night_ambient");
            }
            else
              Game1.changeMusicTrack("spring_night_ambient");
          }
          else
            Game1.changeMusicTrack("spring_night_ambient");
        }
      }
      else if ((Game1.currentSong == null || !Game1.currentSong.IsPlaying) && (Game1.isRaining && !Game1.showingEndOfNightStuff))
        Game1.changeMusicTrack("rain");
      if (Game1.currentSong != null && !Game1.isRaining && (!Game1.currentSeason.Equals("fall") || !Game1.isDebrisWeather) && (!Game1.currentSeason.Equals("winter") && !Game1.eventUp && (Game1.timeOfDay < 1800 && this.name.Equals("Town"))) && ((!Game1.currentSong.IsPlaying || Game1.currentSong.Name.Contains("ambient")) && (Game1.locationAfterWarp != null && Game1.locationAfterWarp.name.Equals("Town"))))
      {
        Game1.changeMusicTrack("springtown");
      }
      else
      {
        if (!this.name.Equals("AnimalShop") && !this.name.Equals("ScienceHouse") || (Game1.currentSong == null || Game1.nextMusicTrack.Contains("marnie")) || (Game1.currentSong.IsPlaying && !Game1.currentSong.Name.Contains("ambient") || this.currentEvent != null))
          return;
        Game1.changeMusicTrack("marnieShop");
      }
    }

    public NPC isCollidingWithCharacter(Microsoft.Xna.Framework.Rectangle box)
    {
      foreach (NPC character in this.characters)
      {
        if (character.GetBoundingBox().Intersects(box))
          return character;
      }
      return (NPC) null;
    }

    public virtual void drawAboveAlwaysFrontLayer(SpriteBatch b)
    {
      if (this.critters != null)
      {
        for (int index = 0; index < this.critters.Count; ++index)
          this.critters[index].drawAboveFrontLayer(b);
      }
      if (Game1.isSnowing && this.isOutdoors && !(this is Desert))
      {
        this.snowPos = Game1.updateFloatingObjectPositionForMovement(this.snowPos, new Vector2((float) Game1.viewport.X, (float) Game1.viewport.Y), Game1.previousViewportPosition, -1f);
        this.snowPos.X = this.snowPos.X % (float) (16 * Game1.pixelZoom);
        Vector2 position = new Vector2();
        float num1 = (float) (-16 * Game1.pixelZoom) + this.snowPos.X % (float) (16 * Game1.pixelZoom);
        while ((double) num1 < (double) Game1.viewport.Width)
        {
          float num2 = (float) (-16 * Game1.pixelZoom) + this.snowPos.Y % (float) (16 * Game1.pixelZoom);
          while ((double) num2 < (double) Game1.viewport.Height)
          {
            position.X = (float) (int) num1;
            position.Y = (float) (int) num2;
            b.Draw(Game1.mouseCursors, position, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(368 + (int) (Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 1200.0) / 75 * 16, 192, 16, 16)), Color.White * Game1.options.snowTransparency, 0.0f, Vector2.Zero, (float) Game1.pixelZoom + 1f / 1000f, SpriteEffects.None, 1f);
            num2 += (float) (16 * Game1.pixelZoom);
          }
          num1 += (float) (16 * Game1.pixelZoom);
        }
      }
      foreach (Character character in this.characters)
        character.drawAboveAlwaysFrontLayer(b);
    }

    public bool moveObject(int oldX, int oldY, int newX, int newY)
    {
      Vector2 key1 = new Vector2((float) oldX, (float) oldY);
      Vector2 key2 = new Vector2((float) newX, (float) newY);
      if (!this.objects.ContainsKey(key1) || this.objects.ContainsKey(key2))
        return false;
      Object @object = this.objects[key1];
      @object.tileLocation = key2;
      this.objects.Remove(key1);
      this.objects.Add(key2, @object);
      return true;
    }

    private void getGalaxySword()
    {
      Game1.flashAlpha = 1f;
      Game1.player.holdUpItemThenMessage((Item) new MeleeWeapon(4), true);
      Game1.player.CurrentTool = (Tool) new MeleeWeapon(4);
      Game1.player.mailReceived.Add("galaxySword");
      Game1.player.jitterStrength = 0.0f;
      Game1.screenGlowHold = false;
    }

    public virtual void performTouchAction(string fullActionString, Vector2 playerStandingPosition)
    {
      if (Game1.eventUp)
        return;
      try
      {
        string s = fullActionString.Split(' ')[0];
        // ISSUE: reference to a compiler-generated method
        uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(s);
        if (stringHash <= 1817135690U)
        {
          if (stringHash <= 799419560U)
          {
            if ((int) stringHash != 327122275)
            {
              if ((int) stringHash != 764036487)
              {
                if ((int) stringHash != 799419560 || !(s == "Sleep") || (Game1.newDay || !Game1.shouldTimePass()) || (Game1.timeOfDay <= 600 && Game1.gameTimeInterval <= 2000 || Game1.player.ActiveObject != null))
                  return;
                this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:FarmHouse_Bed_GoToSleep"), this.createYesNoResponses(), "Sleep", (Object) null);
              }
              else
              {
                if (!(s == "legendarySword"))
                  return;
                if (Game1.player.ActiveObject != null && Game1.player.ActiveObject.parentSheetIndex == 74 && !Game1.player.mailReceived.Contains("galaxySword"))
                {
                  Game1.player.Halt();
                  Game1.player.faceDirection(2);
                  Game1.player.showCarrying();
                  Game1.player.jitterStrength = 1f;
                  Game1.pauseThenDoFunction(7000, new Game1.afterFadeFunction(this.getGalaxySword));
                  Game1.changeMusicTrack("none");
                  Game1.playSound("crit");
                  Game1.screenGlowOnce(new Color(30, 0, 150), true, 0.01f, 0.999f);
                  DelayedAction.playSoundAfterDelay("stardrop", 1500);
                  Game1.screenOverlayTempSprites.AddRange((IEnumerable<TemporaryAnimatedSprite>) Utility.sparkleWithinArea(new Microsoft.Xna.Framework.Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), 500, Color.White, 10, 2000, ""));
                }
                else
                {
                  if (Game1.player.mailReceived.Contains("galaxySword"))
                    return;
                  Game1.playSound("SpringBirds");
                }
              }
            }
            else
            {
              if (!(s == "Emote"))
                return;
              this.getCharacterFromName(fullActionString.Split(' ')[1]).doEmote(Convert.ToInt32(fullActionString.Split(' ')[2]), true);
            }
          }
          else if ((int) stringHash != 1301151257)
          {
            if ((int) stringHash != 1421563949)
            {
              if ((int) stringHash != 1817135690 || !(s == "WomensLocker") || !Game1.player.isMale)
                return;
              Game1.player.position.Y += (float) ((Game1.player.Speed + Game1.player.addedSpeed) * 2);
              Game1.player.Halt();
              Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:WomensLocker_WrongGender"));
            }
            else
            {
              if (!(s == "FaceDirection"))
                return;
              if ((object) this.getCharacterFromName(fullActionString.Split(' ')[1]) == null)
                return;
              this.getCharacterFromName(fullActionString.Split(' ')[1]).faceDirection(Convert.ToInt32(fullActionString.Split(' ')[2]));
            }
          }
          else
          {
            if (!(s == "PoolEntrance"))
              return;
            if (!Game1.player.swimming)
            {
              Game1.player.swimTimer = 800;
              Game1.player.swimming = true;
              Game1.player.position.Y += (float) (Game1.pixelZoom * 4);
              Game1.player.yVelocity = -8f;
              Game1.playSound("pullItemFromWater");
              List<TemporaryAnimatedSprite> temporarySprites = this.temporarySprites;
              TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(27, 100f, 4, 0, new Vector2(Game1.player.position.X, (float) (Game1.player.getStandingY() - Game1.tileSize * 5 / 8)), false, false);
              temporaryAnimatedSprite.layerDepth = 1f;
              Vector2 vector2 = new Vector2(0.0f, 2f);
              temporaryAnimatedSprite.motion = vector2;
              temporarySprites.Add(temporaryAnimatedSprite);
            }
            else
            {
              Game1.player.jump();
              Game1.player.swimTimer = 800;
              Game1.player.position.X = playerStandingPosition.X * (float) Game1.tileSize;
              Game1.playSound("pullItemFromWater");
              Game1.player.yVelocity = 8f;
              Game1.player.swimming = false;
            }
            Game1.player.noMovementPause = 500;
          }
        }
        else if (stringHash <= 3302649497U)
        {
          if ((int) stringHash != -1999286711)
          {
            if ((int) stringHash != -1698547726)
            {
              if ((int) stringHash != -992317799 || !(s == "Bus"))
                return;
              this.boardBus(playerStandingPosition);
            }
            else
            {
              if (!(s == "ChangeIntoSwimsuit"))
                return;
              Game1.player.changeIntoSwimsuit();
            }
          }
          else
          {
            if (!(s == "Door"))
              return;
            int index = 1;
            while (true)
            {
              if (index < fullActionString.Split(' ').Length)
              {
                if (Game1.player.getFriendshipHeartLevelForNPC(fullActionString.Split(' ')[index]) < 2)
                {
                  if (index == fullActionString.Split(' ').Length - 1)
                    break;
                }
                if (index != fullActionString.Split(' ').Length - 1)
                {
                  if (Game1.player.getFriendshipHeartLevelForNPC(fullActionString.Split(' ')[index]) >= 2)
                    goto label_68;
                }
                if (index == fullActionString.Split(' ').Length - 1)
                {
                  if (Game1.player.getFriendshipHeartLevelForNPC(fullActionString.Split(' ')[index]) >= 2)
                    goto label_73;
                }
                ++index;
              }
              else
                goto label_112;
            }
            Farmer player = Game1.player;
            Vector2 vector2 = player.position - Game1.player.getMostRecentMovementVector() * 2f;
            player.position = vector2;
            Game1.player.yVelocity = 0.0f;
            Game1.player.Halt();
            Game1.player.temporaryImpassableTile = Microsoft.Xna.Framework.Rectangle.Empty;
            if (Game1.player.getTileLocation().Equals(this.lastTouchActionLocation))
            {
              if ((double) Game1.player.position.Y > (double) this.lastTouchActionLocation.Y * (double) Game1.tileSize + (double) (Game1.tileSize / 2))
                Game1.player.position.Y += (float) Game1.pixelZoom;
              else
                Game1.player.position.Y -= (float) Game1.pixelZoom;
              this.lastTouchActionLocation = Vector2.Zero;
            }
            if (Game1.player.mailReceived.Contains("doorUnlock" + fullActionString.Split(' ')[1]))
            {
              if (fullActionString.Split(' ').Length == 2)
                return;
              if (Game1.player.mailReceived.Contains("doorUnlock" + fullActionString.Split(' ')[2]))
                return;
            }
            if (fullActionString.Split(' ').Length == 3)
            {
              if (Game1.player.mailReceived.Contains("doorUnlock" + fullActionString.Split(' ')[2]))
                return;
            }
            if (fullActionString.Split(' ').Length == 2)
            {
              NPC characterFromName = Game1.getCharacterFromName(fullActionString.Split(' ')[1], false);
              string str = characterFromName.gender == 0 ? "Male" : "Female";
              Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:DoorUnlock_NotFriend_" + str, (object) characterFromName.displayName));
              return;
            }
            NPC characterFromName1 = Game1.getCharacterFromName(fullActionString.Split(' ')[1], false);
            NPC characterFromName2 = Game1.getCharacterFromName(fullActionString.Split(' ')[2], false);
            Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:DoorUnlock_NotFriend_Couple", (object) characterFromName1.displayName, (object) characterFromName2.displayName));
            return;
label_68:
            if (Game1.player.mailReceived.Contains("doorUnlock" + fullActionString.Split(' ')[index]))
              return;
            Game1.player.mailReceived.Add("doorUnlock" + fullActionString.Split(' ')[index]);
            return;
label_73:
            if (Game1.player.mailReceived.Contains("doorUnlock" + fullActionString.Split(' ')[index]))
              return;
            Game1.player.mailReceived.Add("doorUnlock" + fullActionString.Split(' ')[index]);
            return;
label_112:;
          }
        }
        else if (stringHash <= 3711744508U)
        {
          if ((int) stringHash != -715021196)
          {
            if ((int) stringHash != -583222788 || !(s == "MagicalSeal") || Game1.player.mailReceived.Contains("krobusUnseal"))
              return;
            Farmer player = Game1.player;
            Vector2 vector2 = player.position - Game1.player.getMostRecentMovementVector() * 2f;
            player.position = vector2;
            Game1.player.yVelocity = 0.0f;
            Game1.player.Halt();
            Game1.player.temporaryImpassableTile = Microsoft.Xna.Framework.Rectangle.Empty;
            if (Game1.player.getTileLocation().Equals(this.lastTouchActionLocation))
            {
              if ((double) Game1.player.position.Y > (double) this.lastTouchActionLocation.Y * (double) Game1.tileSize + (double) (Game1.tileSize / 2))
                Game1.player.position.Y += (float) Game1.pixelZoom;
              else
                Game1.player.position.Y -= (float) Game1.pixelZoom;
              this.lastTouchActionLocation = Vector2.Zero;
            }
            Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Sewer_MagicSeal"));
            for (int index = 0; index < 40; ++index)
            {
              List<TemporaryAnimatedSprite> temporarySprites1 = this.temporarySprites;
              TemporaryAnimatedSprite temporaryAnimatedSprite1 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(666, 1851, 8, 8), 25f, 4, 2, new Vector2(3f, 19f) * (float) Game1.tileSize + new Vector2((float) (-Game1.pixelZoom * 2 + index % 4 * 16), (float) (-(index / 4) * Game1.tileSize / 4)), false, false);
              temporaryAnimatedSprite1.layerDepth = (float) ((double) (18 * Game1.tileSize) / 10000.0 + (double) index / 10000.0);
              temporaryAnimatedSprite1.color = new Color(100 + index * 4, index * 5, 120 + index * 4);
              int num1 = 1;
              temporaryAnimatedSprite1.pingPong = num1 != 0;
              int num2 = index * 10;
              temporaryAnimatedSprite1.delayBeforeAnimationStart = num2;
              double pixelZoom1 = (double) Game1.pixelZoom;
              temporaryAnimatedSprite1.scale = (float) pixelZoom1;
              double num3 = 0.00999999977648258;
              temporaryAnimatedSprite1.alphaFade = (float) num3;
              temporarySprites1.Add(temporaryAnimatedSprite1);
              List<TemporaryAnimatedSprite> temporarySprites2 = this.temporarySprites;
              TemporaryAnimatedSprite temporaryAnimatedSprite2 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(666, 1851, 8, 8), 25f, 4, 2, new Vector2(3f, 17f) * (float) Game1.tileSize + new Vector2((float) (-Game1.pixelZoom * 2 + index % 4 * 16), (float) (index / 4 * Game1.tileSize / 4)), false, false);
              temporaryAnimatedSprite2.layerDepth = (float) ((double) (18 * Game1.tileSize) / 10000.0 + (double) index / 10000.0);
              temporaryAnimatedSprite2.color = new Color(232 - index * 4, 192 - index * 6, (int) byte.MaxValue - index * 4);
              int num4 = 1;
              temporaryAnimatedSprite2.pingPong = num4 != 0;
              int num5 = 320 + index * 10;
              temporaryAnimatedSprite2.delayBeforeAnimationStart = num5;
              double pixelZoom2 = (double) Game1.pixelZoom;
              temporaryAnimatedSprite2.scale = (float) pixelZoom2;
              double num6 = 0.00999999977648258;
              temporaryAnimatedSprite2.alphaFade = (float) num6;
              temporarySprites2.Add(temporaryAnimatedSprite2);
              List<TemporaryAnimatedSprite> temporarySprites3 = this.temporarySprites;
              TemporaryAnimatedSprite temporaryAnimatedSprite3 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(666, 1851, 8, 8), 25f, 4, 2, new Vector2(3f, 19f) * (float) Game1.tileSize + new Vector2((float) (-Game1.pixelZoom * 2 + index % 4 * 16), (float) (-(index / 4) * Game1.tileSize / 4)), false, false);
              temporaryAnimatedSprite3.layerDepth = (float) ((double) (18 * Game1.tileSize) / 10000.0 + (double) index / 10000.0);
              temporaryAnimatedSprite3.color = new Color(100 + index * 4, index * 6, 120 + index * 4);
              int num7 = 1;
              temporaryAnimatedSprite3.pingPong = num7 != 0;
              int num8 = 640 + index * 10;
              temporaryAnimatedSprite3.delayBeforeAnimationStart = num8;
              double pixelZoom3 = (double) Game1.pixelZoom;
              temporaryAnimatedSprite3.scale = (float) pixelZoom3;
              double num9 = 0.00999999977648258;
              temporaryAnimatedSprite3.alphaFade = (float) num9;
              temporarySprites3.Add(temporaryAnimatedSprite3);
            }
            Game1.player.jitterStrength = 2f;
            Game1.player.freezePause = 500;
            Game1.playSound("debuffHit");
          }
          else
          {
            if (!(s == "MensLocker") || Game1.player.isMale)
              return;
            Game1.player.position.Y += (float) ((Game1.player.Speed + Game1.player.addedSpeed) * 2);
            Game1.player.Halt();
            Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:MensLocker_WrongGender"));
          }
        }
        else if ((int) stringHash != -296825893)
        {
          if ((int) stringHash != -23098446 || !(s == "MagicWarp"))
            return;
          string nameToWarpTo = fullActionString.Split(' ')[1];
          int int32_1 = Convert.ToInt32(fullActionString.Split(' ')[2]);
          int int32_2 = Convert.ToInt32(fullActionString.Split(' ')[3]);
          string str1;
          if (fullActionString.Split(' ').Length <= 4)
            str1 = (string) null;
          else
            str1 = fullActionString.Split(' ')[4];
          string str2 = str1;
          if (str2 != null && !Game1.player.mailReceived.Contains(str2))
            return;
          for (int index = 0; index < 12; ++index)
            this.temporarySprites.Add(new TemporaryAnimatedSprite(354, (float) Game1.random.Next(25, 75), 6, 1, new Vector2((float) Game1.random.Next((int) Game1.player.position.X - Game1.tileSize * 4, (int) Game1.player.position.X + Game1.tileSize * 3), (float) Game1.random.Next((int) Game1.player.position.Y - Game1.tileSize * 4, (int) Game1.player.position.Y + Game1.tileSize * 3)), false, Game1.random.NextDouble() < 0.5));
          Game1.playSound("wand");
          Game1.displayFarmer = false;
          Game1.player.freezePause = 1000;
          Game1.flashAlpha = 1f;
          DelayedAction.warpAfterDelay(nameToWarpTo, new Point(int32_1, int32_2), 1000);
          new Microsoft.Xna.Framework.Rectangle(Game1.player.GetBoundingBox().X, Game1.player.GetBoundingBox().Y, Game1.tileSize, Game1.tileSize).Inflate(Game1.tileSize * 3, Game1.tileSize * 3);
          int num1 = 0;
          for (int index = Game1.player.getTileX() + 8; index >= Game1.player.getTileX() - 8; --index)
          {
            List<TemporaryAnimatedSprite> temporarySprites = Game1.player.currentLocation.temporarySprites;
            TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(6, new Vector2((float) index, (float) Game1.player.getTileY()) * (float) Game1.tileSize, Color.White, 8, false, 50f, 0, -1, -1f, -1, 0);
            temporaryAnimatedSprite.layerDepth = 1f;
            int num2 = num1 * 25;
            temporaryAnimatedSprite.delayBeforeAnimationStart = num2;
            Vector2 vector2 = new Vector2(-0.25f, 0.0f);
            temporaryAnimatedSprite.motion = vector2;
            temporarySprites.Add(temporaryAnimatedSprite);
            ++num1;
          }
        }
        else
        {
          if (!(s == "ChangeOutOfSwimsuit"))
            return;
          Game1.player.changeOutOfSwimSuit();
        }
      }
      catch (Exception ex)
      {
      }
    }

    public virtual void UpdateWhenCurrentLocation(GameTime time)
    {
      if (this.wasUpdated)
        return;
      this.wasUpdated = true;
      AmbientLocationSounds.update(time);
      if (this.critters != null)
      {
        for (int index = this.critters.Count - 1; index >= 0; --index)
        {
          if (this.critters[index].update(time, this))
            this.critters.RemoveAt(index);
        }
      }
      if (this.fishSplashAnimation != null)
      {
        this.fishSplashAnimation.update(time);
        if (Game1.random.NextDouble() < 0.02)
          this.temporarySprites.Add(new TemporaryAnimatedSprite(0, this.fishSplashAnimation.position + new Vector2((float) Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize / 2), (float) Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize / 2)), Color.White * 0.3f, 8, false, 100f, 0, -1, -1f, -1, 0));
      }
      if (this.orePanAnimation != null)
      {
        this.orePanAnimation.update(time);
        if (Game1.random.NextDouble() < 0.05)
          this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(432, 1435, 16, 16), this.orePanAnimation.position + new Vector2((float) Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize / 2), (float) Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize / 2)), false, 0.02f, Color.White * 0.8f)
          {
            scale = (float) (Game1.pixelZoom / 2),
            animationLength = 6,
            interval = 100f
          });
      }
      if (this.doorSprites != null)
      {
        foreach (KeyValuePair<Point, TemporaryAnimatedSprite> doorSprite in this.doorSprites)
          doorSprite.Value.update(time);
      }
      int waterAnimationTimer = this.waterAnimationTimer;
      TimeSpan timeSpan = time.ElapsedGameTime;
      int milliseconds1 = timeSpan.Milliseconds;
      this.waterAnimationTimer = waterAnimationTimer - milliseconds1;
      if (this.waterAnimationTimer <= 0)
      {
        this.waterAnimationIndex = (this.waterAnimationIndex + 1) % 10;
        this.waterAnimationTimer = 200;
      }
      if (!this.isFarm)
      {
        double waterPosition = (double) this.waterPosition;
        timeSpan = time.TotalGameTime;
        double num = (Math.Sin((double) timeSpan.Milliseconds / 1000.0) + 1.0) * 0.150000005960464;
        this.waterPosition = (float) (waterPosition + num);
      }
      else
        this.waterPosition = this.waterPosition + 0.1f;
      if ((double) this.waterPosition >= (double) Game1.tileSize)
      {
        this.waterPosition = this.waterPosition - (float) Game1.tileSize;
        this.waterTileFlip = !this.waterTileFlip;
      }
      Map map = this.map;
      timeSpan = time.ElapsedGameTime;
      long milliseconds2 = (long) timeSpan.Milliseconds;
      map.Update(milliseconds2);
      for (int index = this.debris.Count - 1; index >= 0; --index)
      {
        if (this.debris[index].updateChunks(time))
          this.debris.RemoveAt(index);
      }
      if (Game1.shouldTimePass() || Game1.isFestival())
      {
        for (int index = this.projectiles.Count - 1; index >= 0; --index)
        {
          if (this.projectiles[index].update(time, this))
            this.projectiles.RemoveAt(index);
        }
      }
      foreach (KeyValuePair<Vector2, TerrainFeature> terrainFeature in (Dictionary<Vector2, TerrainFeature>) this.terrainFeatures)
      {
        if (terrainFeature.Value.tickUpdate(time, terrainFeature.Key))
          this.terrainFeaturesToRemoveList.Add(terrainFeature.Key);
      }
      foreach (Vector2 featuresToRemove in this.terrainFeaturesToRemoveList)
        this.terrainFeatures.Remove(featuresToRemove);
      this.terrainFeaturesToRemoveList.Clear();
      if (this.largeTerrainFeatures != null)
      {
        foreach (LargeTerrainFeature largeTerrainFeature in this.largeTerrainFeatures)
          largeTerrainFeature.tickUpdate(time);
      }
      if (Game1.timeOfDay >= 2000 && (double) this.lightLevel > 0.0 && this.name.Equals("FarmHouse"))
        Game1.currentLightSources.Add(new LightSource(4, new Vector2((float) Game1.tileSize, (float) (7 * Game1.tileSize)), 2f));
      if (this.currentEvent != null)
        this.currentEvent.checkForNextCommand(this, time);
      foreach (KeyValuePair<Vector2, Object> keyValuePair in (Dictionary<Vector2, Object>) this.objects)
        keyValuePair.Value.updateWhenCurrentLocation(time);
      if ((int) Game1.gameMode != 3)
        return;
      if (this.isOutdoors && !Game1.isRaining && (Game1.random.NextDouble() < 0.002 && Game1.currentSong != null) && (!Game1.currentSong.IsPlaying && Game1.timeOfDay < 2000 && (!Game1.currentSeason.Equals("winter") && !this.name.Equals("Desert"))))
        Game1.playSound("SpringBirds");
      else if (!Game1.isRaining && this.isOutdoors && (Game1.timeOfDay > 2100 && Game1.currentSeason.Equals("summer")) && (Game1.random.NextDouble() < 0.0005 && !(this is Beach) && !this.name.Equals("temp")))
        Game1.playSound("crickets");
      else if (Game1.isRaining && this.isOutdoors && (!this.name.Equals("Town") && !Game1.eventUp) && ((double) Game1.options.musicVolumeLevel > 0.0 && Game1.random.NextDouble() < 0.00015))
        Game1.playSound("rainsound");
      Vector2 vector2 = new Vector2((float) (Game1.player.getStandingX() / Game1.tileSize), (float) (Game1.player.getStandingY() / Game1.tileSize));
      if (this.lastTouchActionLocation.Equals(Vector2.Zero))
      {
        string fullActionString = this.doesTileHaveProperty((int) vector2.X, (int) vector2.Y, "TouchAction", "Back");
        this.lastTouchActionLocation = new Vector2((float) (Game1.player.getStandingX() / Game1.tileSize), (float) (Game1.player.getStandingY() / Game1.tileSize));
        if (fullActionString != null)
          this.performTouchAction(fullActionString, vector2);
      }
      else if (!this.lastTouchActionLocation.Equals(vector2))
      {
        this.lastTouchActionLocation = Vector2.Zero;
        Game1.noteBlockTimer = 0.0f;
      }
      Vector2 tileLocation = Game1.player.getTileLocation();
      foreach (Vector2 adjacentTilesOffset in Character.AdjacentTilesOffsets)
      {
        Object @object;
        if (this.objects.TryGetValue(tileLocation + adjacentTilesOffset, out @object))
          @object.farmerAdjacentAction();
      }
      if (!Game1.boardingBus)
        return;
      NPC characterFromName = this.getCharacterFromName("Pam");
      if ((object) characterFromName == null || this.doesTileHaveProperty(characterFromName.getStandingX() / Game1.tileSize, characterFromName.getStandingY() / Game1.tileSize, "TouchAction", "Back") == null)
        return;
      this.busLeave();
    }

    public NPC getCharacterFromName(string name)
    {
      NPC npc = (NPC) null;
      foreach (NPC character in this.characters)
      {
        if (character.name.Equals(name))
          return character;
      }
      return npc;
    }

    public virtual void updateEvenIfFarmerIsntHere(GameTime time, bool ignoreWasUpdatedFlush = false)
    {
      if (!ignoreWasUpdatedFlush)
        this.wasUpdated = false;
      for (int index = this.characters.Count - 1; index >= 0; --index)
      {
        if (this.characters[index] != null && (Game1.shouldTimePass() || this.characters[index] is Horse || this.characters[index].forceUpdateTimer > 0))
        {
          this.characters[index].update(time, this);
          if (index < this.characters.Count && this.characters[index] is Monster && ((Monster) this.characters[index]).health <= 0)
            this.characters.RemoveAt(index);
        }
        else if (this.characters[index] != null)
          this.characters[index].updateEmote(time);
      }
      for (int index = this.temporarySprites.Count - 1; index >= 0; --index)
      {
        if (this.temporarySprites[index] != null && this.temporarySprites[index].update(time) && index < this.temporarySprites.Count)
          this.temporarySprites.RemoveAt(index);
      }
    }

    public Response[] createYesNoResponses()
    {
      return new Response[2]
      {
        new Response("Yes", Game1.content.LoadString("Strings\\Lexicon:QuestionDialogue_Yes")),
        new Response("No", Game1.content.LoadString("Strings\\Lexicon:QuestionDialogue_No"))
      };
    }

    public void createQuestionDialogue(string question, Response[] answerChoices, string dialogKey)
    {
      this.lastQuestionKey = dialogKey;
      Game1.drawObjectQuestionDialogue(question, ((IEnumerable<Response>) answerChoices).ToList<Response>());
    }

    public void createQuestionDialogueWithCustomWidth(string question, Response[] answerChoices, string dialogKey)
    {
      int width = SpriteText.getWidthOfString(question) + Game1.tileSize;
      this.lastQuestionKey = dialogKey;
      Game1.drawObjectQuestionDialogue(question, ((IEnumerable<Response>) answerChoices).ToList<Response>(), width);
    }

    public void createQuestionDialogue(string question, Response[] answerChoices, GameLocation.afterQuestionBehavior afterDialogueBehavior, NPC speaker = null)
    {
      this.afterQuestion = afterDialogueBehavior;
      Game1.drawObjectQuestionDialogue(question, ((IEnumerable<Response>) answerChoices).ToList<Response>());
      if (speaker == null)
        return;
      Game1.objectDialoguePortraitPerson = speaker;
    }

    public void createQuestionDialogue(string question, Response[] answerChoices, string dialogKey, Object actionObject)
    {
      this.lastQuestionKey = dialogKey;
      Game1.drawObjectQuestionDialogue(question, ((IEnumerable<Response>) answerChoices).ToList<Response>());
      this.actionObjectForQuestionDialogue = actionObject;
    }

    public virtual void monsterDrop(Monster monster, int x, int y)
    {
      int coinsToDrop = monster.coinsToDrop;
      List<int> objectsToDrop = monster.objectsToDrop;
      Vector2 vector2;
      // ISSUE: explicit reference operation
      // ISSUE: variable of a reference type
      Vector2& local = @vector2;
      Microsoft.Xna.Framework.Rectangle boundingBox = Game1.player.GetBoundingBox();
      double x1 = (double) boundingBox.Center.X;
      boundingBox = Game1.player.GetBoundingBox();
      double y1 = (double) boundingBox.Center.Y;
      // ISSUE: explicit reference operation
      ^local = new Vector2((float) x1, (float) y1);
      List<Item> extraDropItems = monster.getExtraDropItems();
      if (Game1.player.isWearingRing(526))
      {
        string[] strArray = Game1.content.Load<Dictionary<string, string>>("Data\\Monsters")[monster.name].Split('/')[6].Split(' ');
        int index = 0;
        while (index < strArray.Length)
        {
          if (Game1.random.NextDouble() < Convert.ToDouble(strArray[index + 1]))
            objectsToDrop.Add(Convert.ToInt32(strArray[index]));
          index += 2;
        }
      }
      for (int index = 0; index < objectsToDrop.Count; ++index)
      {
        int objectIndex = objectsToDrop[index];
        if (objectIndex < 0)
          this.debris.Add(new Debris(Math.Abs(objectIndex), Game1.random.Next(1, 4), new Vector2((float) x, (float) y), vector2));
        else
          this.debris.Add(new Debris(objectIndex, new Vector2((float) x, (float) y), vector2));
      }
      for (int index = 0; index < extraDropItems.Count; ++index)
        this.debris.Add(new Debris(extraDropItems[index], new Vector2((float) x, (float) y), vector2));
    }

    public bool damageMonster(Microsoft.Xna.Framework.Rectangle areaOfEffect, int minDamage, int maxDamage, bool isBomb, Farmer who)
    {
      return this.damageMonster(areaOfEffect, minDamage, maxDamage, isBomb, 1f, 0, 0.0f, 1f, false, who);
    }

    private bool isMonsterDamageApplicable(Farmer who, Monster monster, bool horizontalBias = true)
    {
      if (!monster.isGlider && !(who.CurrentTool is Slingshot))
      {
        Point tileLocationPoint1 = who.getTileLocationPoint();
        Point tileLocationPoint2 = monster.getTileLocationPoint();
        if (Math.Abs(tileLocationPoint1.X - tileLocationPoint2.X) + Math.Abs(tileLocationPoint1.Y - tileLocationPoint2.Y) > 1)
        {
          int num1 = tileLocationPoint2.X - tileLocationPoint1.X;
          int num2 = tileLocationPoint2.Y - tileLocationPoint1.Y;
          Vector2 key = new Vector2((float) tileLocationPoint1.X, (float) tileLocationPoint1.Y);
          while (num1 != 0 || num2 != 0)
          {
            if (horizontalBias)
            {
              if (Math.Abs(num1) >= Math.Abs(num2))
              {
                key.X += (float) Math.Sign(num1);
                num1 -= Math.Sign(num1);
              }
              else
              {
                key.Y += (float) Math.Sign(num2);
                num2 -= Math.Sign(num2);
              }
            }
            else if (Math.Abs(num2) >= Math.Abs(num1))
            {
              key.Y += (float) Math.Sign(num2);
              num2 -= Math.Sign(num2);
            }
            else
            {
              key.X += (float) Math.Sign(num1);
              num1 -= Math.Sign(num1);
            }
            if (this.objects.ContainsKey(key) || this.getTileIndexAt((int) key.X, (int) key.Y, "Buildings") != -1)
              return false;
          }
        }
      }
      return true;
    }

    public bool damageMonster(Microsoft.Xna.Framework.Rectangle areaOfEffect, int minDamage, int maxDamage, bool isBomb, float knockBackModifier, int addedPrecision, float critChance, float critMultiplier, bool triggerMonsterInvincibleTimer, Farmer who)
    {
      bool flag1 = false;
      for (int index1 = this.characters.Count - 1; index1 >= 0; --index1)
      {
        if (this.characters[index1].GetBoundingBox().Intersects(areaOfEffect) && this.characters[index1].IsMonster && (!this.characters[index1].isInvisible && !(this.characters[index1] as Monster).isInvincible()) && !(this.characters[index1] as Monster).isInvincible() && (isBomb || this.isMonsterDamageApplicable(who, this.characters[index1] as Monster, true) || this.isMonsterDamageApplicable(who, this.characters[index1] as Monster, false)))
        {
          bool flag2 = who != null && who.CurrentTool != null && who.CurrentTool is MeleeWeapon && (who.CurrentTool as MeleeWeapon).type == 1;
          flag1 = true;
          Rumble.rumble(0.1f + (float) (Game1.random.NextDouble() / 4.0), (float) (200 + Game1.random.Next(-50, 50)));
          Microsoft.Xna.Framework.Rectangle boundingBox = this.characters[index1].GetBoundingBox();
          Vector2 trajectory = Utility.getAwayFromPlayerTrajectory(boundingBox, who);
          if ((double) knockBackModifier > 0.0)
            trajectory *= knockBackModifier;
          else
            trajectory = new Vector2(this.characters[index1].xVelocity, this.characters[index1].yVelocity);
          if ((this.characters[index1] as Monster).slipperiness == -1)
            trajectory = Vector2.Zero;
          bool flag3 = false;
          if (who != null && who.CurrentTool != null && this.characters[index1].hitWithTool(who.CurrentTool))
            return false;
          if (who.professions.Contains(25))
            critChance += critChance * 0.5f;
          int number;
          if (maxDamage >= 0)
          {
            int num = Game1.random.Next(minDamage, maxDamage + 1);
            if (who != null && Game1.random.NextDouble() < (double) critChance + (double) who.LuckLevel * ((double) critChance / 40.0))
            {
              flag3 = true;
              Game1.playSound("crit");
            }
            int damage = Math.Max(1, (flag3 ? (int) ((double) num * (double) critMultiplier) : num) + (who != null ? who.attack * 3 : 0));
            if (who != null && who.professions.Contains(24))
              damage = (int) Math.Ceiling((double) damage * 1.10000002384186);
            if (who != null && who.professions.Contains(26))
              damage = (int) Math.Ceiling((double) damage * 1.14999997615814);
            if (who != null & flag3 && who.professions.Contains(29))
              damage *= 3;
            number = ((Monster) this.characters[index1]).takeDamage(damage, (int) trajectory.X, (int) trajectory.Y, isBomb, (double) addedPrecision / 10.0);
            if (number == -1)
            {
              this.debris.Add(new Debris("Miss", 1, new Vector2((float) boundingBox.Center.X, (float) boundingBox.Center.Y), Color.LightGray, 1f, 0.0f));
            }
            else
            {
              for (int index2 = this.debris.Count - 1; index2 >= 0; --index2)
              {
                if (this.debris[index2].toHover != null && this.debris[index2].toHover.Equals((object) this.characters[index1]) && (!this.debris[index2].nonSpriteChunkColor.Equals(Color.Yellow) && (double) this.debris[index2].timeSinceDoneBouncing > 900.0))
                  this.debris.RemoveAt(index2);
              }
              this.debris.Add(new Debris(number, new Vector2((float) (boundingBox.Center.X + Game1.pixelZoom * 4), (float) boundingBox.Center.Y), flag3 ? Color.Yellow : new Color((int) byte.MaxValue, 130, 0), flag3 ? (float) (1.0 + (double) number / 300.0) : 1f, (Character) this.characters[index1]));
            }
            if (triggerMonsterInvincibleTimer)
              (this.characters[index1] as Monster).setInvincibleCountdown(450 / (flag2 ? 3 : 2));
          }
          else
          {
            number = -2;
            this.characters[index1].setTrajectory(trajectory);
            if (((Monster) this.characters[index1]).slipperiness > 10)
            {
              this.characters[index1].xVelocity /= 2f;
              this.characters[index1].yVelocity /= 2f;
            }
          }
          if (who != null && who.CurrentTool != null && who.CurrentTool.Name.Equals("Galaxy Sword"))
            this.temporarySprites.Add(new TemporaryAnimatedSprite(362, (float) Game1.random.Next(50, 120), 6, 1, new Vector2((float) (boundingBox.Center.X - Game1.tileSize / 2), (float) (boundingBox.Center.Y - Game1.tileSize / 2)), false, false));
          if (((Monster) this.characters[index1]).health <= 0)
          {
            if (!this.isFarm)
              who.checkForQuestComplete((NPC) null, 1, 1, (Item) null, this.characters[index1].name, 4, -1);
            if (who != null && who.leftRing != null)
              who.leftRing.onMonsterSlay((Monster) this.characters[index1]);
            if (who != null && who.rightRing != null)
              who.rightRing.onMonsterSlay((Monster) this.characters[index1]);
            if (who != null && who.IsMainPlayer && !this.isFarm && (!(this.characters[index1] is GreenSlime) || (this.characters[index1] as GreenSlime).firstGeneration))
              Game1.stats.monsterKilled(this.characters[index1].name);
            this.monsterDrop((Monster) this.characters[index1], boundingBox.Center.X, boundingBox.Center.Y);
            if (who != null && !this.isFarm)
              who.gainExperience(4, ((Monster) this.characters[index1]).experienceGained);
            this.characters.RemoveAt(index1);
            ++Game1.stats.MonstersKilled;
          }
          else if (number > 0)
          {
            ((Monster) this.characters[index1]).shedChunks(Game1.random.Next(1, 3));
            if (flag3)
            {
              List<TemporaryAnimatedSprite> temporarySprites1 = this.temporarySprites;
              TemporaryAnimatedSprite temporaryAnimatedSprite1 = new TemporaryAnimatedSprite(362, (float) Game1.random.Next(15, 50), 6, 1, this.characters[index1].getStandingPosition() - new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2)), false, Game1.random.NextDouble() < 0.5);
              temporaryAnimatedSprite1.scale = 0.75f;
              double num1 = flag3 ? 0.75 : 0.5;
              temporaryAnimatedSprite1.alpha = (float) num1;
              temporarySprites1.Add(temporaryAnimatedSprite1);
              List<TemporaryAnimatedSprite> temporarySprites2 = this.temporarySprites;
              TemporaryAnimatedSprite temporaryAnimatedSprite2 = new TemporaryAnimatedSprite(362, (float) Game1.random.Next(15, 50), 6, 1, this.characters[index1].getStandingPosition() - new Vector2((float) (Game1.tileSize / 2 + Game1.random.Next(-Game1.tileSize / 3, Game1.tileSize / 3) + Game1.tileSize / 2), (float) (Game1.tileSize / 2 + Game1.random.Next(-Game1.tileSize / 3, Game1.tileSize / 3))), false, Game1.random.NextDouble() < 0.5);
              temporaryAnimatedSprite2.scale = 0.5f;
              temporaryAnimatedSprite2.delayBeforeAnimationStart = 50;
              double num2 = flag3 ? 0.75 : 0.5;
              temporaryAnimatedSprite2.alpha = (float) num2;
              temporarySprites2.Add(temporaryAnimatedSprite2);
              List<TemporaryAnimatedSprite> temporarySprites3 = this.temporarySprites;
              TemporaryAnimatedSprite temporaryAnimatedSprite3 = new TemporaryAnimatedSprite(362, (float) Game1.random.Next(15, 50), 6, 1, this.characters[index1].getStandingPosition() - new Vector2((float) (Game1.tileSize / 2 + Game1.random.Next(-Game1.tileSize / 3, Game1.tileSize / 3) - Game1.tileSize / 2), (float) (Game1.tileSize / 2 + Game1.random.Next(-Game1.tileSize / 3, Game1.tileSize / 3))), false, Game1.random.NextDouble() < 0.5);
              temporaryAnimatedSprite3.scale = 0.5f;
              temporaryAnimatedSprite3.delayBeforeAnimationStart = 100;
              double num3 = flag3 ? 0.75 : 0.5;
              temporaryAnimatedSprite3.alpha = (float) num3;
              temporarySprites3.Add(temporaryAnimatedSprite3);
              List<TemporaryAnimatedSprite> temporarySprites4 = this.temporarySprites;
              TemporaryAnimatedSprite temporaryAnimatedSprite4 = new TemporaryAnimatedSprite(362, (float) Game1.random.Next(15, 50), 6, 1, this.characters[index1].getStandingPosition() - new Vector2((float) (Game1.tileSize / 2 + Game1.random.Next(-Game1.tileSize / 3, Game1.tileSize / 3) + Game1.tileSize / 2), (float) (Game1.tileSize / 2 + Game1.random.Next(-Game1.tileSize / 3, Game1.tileSize / 3))), false, Game1.random.NextDouble() < 0.5);
              temporaryAnimatedSprite4.scale = 0.5f;
              temporaryAnimatedSprite4.delayBeforeAnimationStart = 150;
              double num4 = flag3 ? 0.75 : 0.5;
              temporaryAnimatedSprite4.alpha = (float) num4;
              temporarySprites4.Add(temporaryAnimatedSprite4);
              List<TemporaryAnimatedSprite> temporarySprites5 = this.temporarySprites;
              TemporaryAnimatedSprite temporaryAnimatedSprite5 = new TemporaryAnimatedSprite(362, (float) Game1.random.Next(15, 50), 6, 1, this.characters[index1].getStandingPosition() - new Vector2((float) (Game1.tileSize / 2 + Game1.random.Next(-Game1.tileSize / 3, Game1.tileSize / 3) - Game1.tileSize / 2), (float) (Game1.tileSize / 2 + Game1.random.Next(-Game1.tileSize / 3, Game1.tileSize / 3))), false, Game1.random.NextDouble() < 0.5);
              temporaryAnimatedSprite5.scale = 0.5f;
              temporaryAnimatedSprite5.delayBeforeAnimationStart = 200;
              double num5 = flag3 ? 0.75 : 0.5;
              temporaryAnimatedSprite5.alpha = (float) num5;
              temporarySprites5.Add(temporaryAnimatedSprite5);
            }
          }
          if (number > 0 && who != null && (number > 1 && Game1.player.CurrentTool != null) && (Game1.player.CurrentTool.Name.Equals("Dark Sword") && Game1.random.NextDouble() < 0.08))
          {
            who.health = Math.Min(who.maxHealth, Game1.player.health + number / 2);
            this.debris.Add(new Debris(number / 2, new Vector2((float) Game1.player.getStandingX(), (float) Game1.player.getStandingY()), Color.Lime, 1f, (Character) who));
            Game1.playSound("healSound");
          }
        }
      }
      return flag1;
    }

    public void moveCharacters(GameTime time)
    {
      for (int index = this.characters.Count - 1; index >= 0; --index)
      {
        if (!this.characters[index].isInvisible)
          this.characters[index].updateMovement(this, time);
      }
    }

    public List<Farmer> getFarmers()
    {
      List<Farmer> farmerList = new List<Farmer>((IEnumerable<Farmer>) this.farmers);
      if (Game1.player.currentLocation != null && Game1.player.currentLocation.Equals((object) this))
        farmerList.Add(Game1.player);
      return farmerList;
    }

    public int getFarmersCount()
    {
      int count = this.farmers.Count;
      if (Game1.player.currentLocation != null && Game1.player.currentLocation.Equals((object) this))
        ++count;
      return count;
    }

    public void growWeedGrass(int iterations)
    {
      for (int index1 = 0; index1 < iterations; ++index1)
      {
        for (int index2 = this.terrainFeatures.Count - 1; index2 >= 0; --index2)
        {
          KeyValuePair<Vector2, TerrainFeature> keyValuePair = this.terrainFeatures.ElementAt<KeyValuePair<Vector2, TerrainFeature>>(index2);
          if (keyValuePair.Value.GetType() == typeof (Grass) && Game1.random.NextDouble() < 0.65)
          {
            if (((Grass) keyValuePair.Value).numberOfWeeds < 4)
              ((Grass) keyValuePair.Value).numberOfWeeds = (int) (byte) Math.Min(4, ((Grass) keyValuePair.Value).numberOfWeeds + (int) (byte) Game1.random.Next(3));
            else if (((Grass) keyValuePair.Value).numberOfWeeds >= 4)
            {
              int x = (int) keyValuePair.Key.X;
              int y = (int) keyValuePair.Key.Y;
              if (this.isTileOnMap(x, y) && !this.isTileOccupied(keyValuePair.Key + new Vector2(-1f, 0.0f), "") && (this.isTileLocationOpenIgnoreFrontLayers(new Location((x - 1) * Game1.tileSize, y * Game1.tileSize)) && this.doesTileHaveProperty(x - 1, y, "Diggable", "Back") != null) && Game1.random.NextDouble() < 0.25)
                this.terrainFeatures.Add(keyValuePair.Key + new Vector2(-1f, 0.0f), (TerrainFeature) new Grass((int) ((Grass) keyValuePair.Value).grassType, Game1.random.Next(1, 3)));
              if (this.isTileOnMap(x, y) && !this.isTileOccupied(keyValuePair.Key + new Vector2(1f, 0.0f), "") && (this.isTileLocationOpenIgnoreFrontLayers(new Location((x + 1) * Game1.tileSize, y * Game1.tileSize)) && this.doesTileHaveProperty(x + 1, y, "Diggable", "Back") != null) && Game1.random.NextDouble() < 0.25)
                this.terrainFeatures.Add(keyValuePair.Key + new Vector2(1f, 0.0f), (TerrainFeature) new Grass((int) ((Grass) keyValuePair.Value).grassType, Game1.random.Next(1, 3)));
              if (this.isTileOnMap(x, y) && !this.isTileOccupied(keyValuePair.Key + new Vector2(0.0f, 1f), "") && (this.isTileLocationOpenIgnoreFrontLayers(new Location(x * Game1.tileSize, (y + 1) * Game1.tileSize)) && this.doesTileHaveProperty(x, y + 1, "Diggable", "Back") != null) && Game1.random.NextDouble() < 0.25)
                this.terrainFeatures.Add(keyValuePair.Key + new Vector2(0.0f, 1f), (TerrainFeature) new Grass((int) ((Grass) keyValuePair.Value).grassType, Game1.random.Next(1, 3)));
              if (this.isTileOnMap(x, y) && !this.isTileOccupied(keyValuePair.Key + new Vector2(0.0f, -1f), "") && (this.isTileLocationOpenIgnoreFrontLayers(new Location(x * Game1.tileSize, (y - 1) * Game1.tileSize)) && this.doesTileHaveProperty(x, y - 1, "Diggable", "Back") != null) && Game1.random.NextDouble() < 0.25)
                this.terrainFeatures.Add(keyValuePair.Key + new Vector2(0.0f, -1f), (TerrainFeature) new Grass((int) ((Grass) keyValuePair.Value).grassType, Game1.random.Next(1, 3)));
            }
          }
        }
      }
    }

    public void spawnWeeds(bool weedsOnly)
    {
      int num1 = Game1.random.Next(this.isFarm ? 5 : 2, this.isFarm ? 12 : 6);
      if (Game1.dayOfMonth == 1 && Game1.currentSeason.Equals("spring"))
        num1 *= 15;
      if (this.name.Equals("Desert"))
        num1 = Game1.random.NextDouble() < 0.1 ? 1 : 0;
      for (int index = 0; index < num1; ++index)
      {
        int num2 = 0;
        while (num2 < 3)
        {
          int xTile = Game1.random.Next(this.map.DisplayWidth / Game1.tileSize);
          int yTile = Game1.random.Next(this.map.DisplayHeight / Game1.tileSize);
          Vector2 vector2 = new Vector2((float) xTile, (float) yTile);
          Object @object;
          this.objects.TryGetValue(vector2, out @object);
          int which = -1;
          int num3 = -1;
          if (this.name.Equals("Desert"))
          {
            if (Game1.random.NextDouble() >= 0.5)
              ;
          }
          else if (Game1.random.NextDouble() < 0.15 + (weedsOnly ? 0.05 : 0.0))
            which = 1;
          else if (!weedsOnly && Game1.random.NextDouble() < 0.35)
            num3 = 1;
          else if (!weedsOnly && !this.isFarm && Game1.random.NextDouble() < 0.35)
            num3 = 2;
          if (num3 != -1)
          {
            if (this is Farm && Game1.random.NextDouble() < 0.25)
              return;
          }
          else if (@object == null && this.doesTileHaveProperty(xTile, yTile, "Diggable", "Back") != null && (this.isTileLocationOpen(new Location(xTile * Game1.tileSize, yTile * Game1.tileSize)) && !this.isTileOccupied(vector2, "")) && this.doesTileHaveProperty(xTile, yTile, "Water", "Back") == null)
          {
            string str = this.doesTileHaveProperty(xTile, yTile, "NoSpawn", "Back");
            if (str == null || !str.Equals("Grass") && !str.Equals("All") && !str.Equals("True"))
            {
              if (which != -1 && !Game1.currentSeason.Equals("winter") && this.name.Equals("Farm"))
              {
                int numberOfWeeds = Game1.random.Next(1, 3);
                this.terrainFeatures.Add(vector2, (TerrainFeature) new Grass(which, numberOfWeeds));
              }
            }
            else
              continue;
          }
          ++num2;
        }
      }
    }

    public bool addCharacterAtRandomLocation(NPC n)
    {
      Vector2 tileLocation = new Vector2((float) Game1.random.Next(0, this.map.GetLayer("Back").LayerWidth), (float) Game1.random.Next(0, this.map.GetLayer("Back").LayerHeight));
      int num;
      for (num = 0; num < 6 && (this.isTileOccupied(tileLocation, "") || !this.isTilePassable(new Location((int) tileLocation.X, (int) tileLocation.Y), Game1.viewport) || (this.map.GetLayer("Back").Tiles[(int) tileLocation.X, (int) tileLocation.Y] == null || this.map.GetLayer("Back").Tiles[(int) tileLocation.X, (int) tileLocation.Y].Properties.ContainsKey("NPCBarrier"))); ++num)
        tileLocation = new Vector2((float) Game1.random.Next(0, this.map.GetLayer("Back").LayerWidth), (float) Game1.random.Next(0, this.map.GetLayer("Back").LayerHeight));
      if (num >= 6)
        return false;
      n.position = tileLocation * new Vector2((float) Game1.tileSize, (float) Game1.tileSize) - new Vector2(0.0f, (float) (n.sprite.spriteHeight - Game1.tileSize));
      this.addCharacter(n);
      return true;
    }

    public virtual void DayUpdate(int dayOfMonth)
    {
      Random random = new Random((int) Game1.uniqueIDForThisGame + (int) Game1.stats.DaysPlayed);
      this.temporarySprites.Clear();
      for (int index = this.terrainFeatures.Count - 1; index >= 0; --index)
      {
        KeyValuePair<Vector2, TerrainFeature> keyValuePair = this.terrainFeatures.ElementAt<KeyValuePair<Vector2, TerrainFeature>>(index);
        if (!this.isTileOnMap(keyValuePair.Key))
        {
          SerializableDictionary<Vector2, TerrainFeature> terrainFeatures = this.terrainFeatures;
          keyValuePair = this.terrainFeatures.ElementAt<KeyValuePair<Vector2, TerrainFeature>>(index);
          Vector2 key = keyValuePair.Key;
          terrainFeatures.Remove(key);
        }
        else
        {
          keyValuePair = this.terrainFeatures.ElementAt<KeyValuePair<Vector2, TerrainFeature>>(index);
          TerrainFeature terrainFeature = keyValuePair.Value;
          keyValuePair = this.terrainFeatures.ElementAt<KeyValuePair<Vector2, TerrainFeature>>(index);
          Vector2 key = keyValuePair.Key;
          terrainFeature.dayUpdate(this, key);
        }
      }
      if (this.largeTerrainFeatures != null)
      {
        foreach (LargeTerrainFeature largeTerrainFeature in this.largeTerrainFeatures)
          largeTerrainFeature.dayUpdate(this);
      }
      foreach (Object @object in this.objects.Values)
        @object.DayUpdate(this);
      if (!(this is FarmHouse))
        this.debris.Clear();
      if (this.isOutdoors)
      {
        if (Game1.dayOfMonth % 7 == 0 && !(this is Farm))
        {
          for (int index = this.objects.Count - 1; index >= 0; --index)
          {
            if (this.objects.ElementAt<KeyValuePair<Vector2, Object>>(index).Value.isSpawnedObject)
              this.objects.Remove(this.objects.ElementAt<KeyValuePair<Vector2, Object>>(index).Key);
          }
          this.numberOfSpawnedObjectsOnMap = 0;
          this.spawnObjects();
          this.spawnObjects();
        }
        this.spawnObjects();
        if (Game1.dayOfMonth == 1)
          this.spawnObjects();
        if (Game1.stats.DaysPlayed < 4U)
          this.spawnObjects();
        bool flag = false;
        foreach (Component layer in this.map.Layers)
        {
          if (layer.Id.Equals("Paths"))
          {
            flag = true;
            break;
          }
        }
        if (flag && !(this is Farm))
        {
          for (int index1 = 0; index1 < this.map.Layers[0].LayerWidth; ++index1)
          {
            for (int index2 = 0; index2 < this.map.Layers[0].LayerHeight; ++index2)
            {
              if (this.map.GetLayer("Paths").Tiles[index1, index2] != null && Game1.random.NextDouble() < 0.5)
              {
                Vector2 key = new Vector2((float) index1, (float) index2);
                int which = -1;
                switch (this.map.GetLayer("Paths").Tiles[index1, index2].TileIndex)
                {
                  case 9:
                    which = 1;
                    if (Game1.currentSeason.Equals("winter"))
                    {
                      which += 3;
                      break;
                    }
                    break;
                  case 10:
                    which = 2;
                    if (Game1.currentSeason.Equals("winter"))
                    {
                      which += 3;
                      break;
                    }
                    break;
                  case 11:
                    which = 3;
                    break;
                  case 12:
                    which = 6;
                    break;
                }
                if (which != -1 && !this.terrainFeatures.ContainsKey(key) && !this.objects.ContainsKey(key))
                  this.terrainFeatures.Add(key, (TerrainFeature) new Tree(which, 2));
              }
            }
          }
        }
      }
      if (!this.isFarm)
      {
        Dictionary<Vector2, TerrainFeature>.KeyCollection keys = this.terrainFeatures.Keys;
        for (int index = keys.Count - 1; index >= 0; --index)
        {
          if (this.terrainFeatures[keys.ElementAt<Vector2>(index)] is HoeDirt && ((this.terrainFeatures[keys.ElementAt<Vector2>(index)] as HoeDirt).crop == null || (this.terrainFeatures[keys.ElementAt<Vector2>(index)] as HoeDirt).crop.forageCrop))
            this.terrainFeatures.Remove(keys.ElementAt<Vector2>(index));
        }
      }
      int num = this.characters.Count - 1;
      while (num >= 0)
        --num;
      this.lightLevel = 0.0f;
      if (this.name.Equals("BugLand"))
      {
        for (int index1 = 0; index1 < this.map.Layers[0].LayerWidth; ++index1)
        {
          for (int index2 = 0; index2 < this.map.Layers[0].LayerHeight; ++index2)
          {
            if (Game1.random.NextDouble() < 0.33)
            {
              Tile tile = this.map.GetLayer("Paths").Tiles[index1, index2];
              if (tile != null)
              {
                Vector2 vector2 = new Vector2((float) index1, (float) index2);
                switch (tile.TileIndex)
                {
                  case 13:
                  case 14:
                  case 15:
                    if (!this.objects.ContainsKey(vector2))
                    {
                      this.objects.Add(vector2, new Object(vector2, GameLocation.getWeedForSeason(Game1.random, "spring"), 1));
                      continue;
                    }
                    continue;
                  case 16:
                    if (!this.objects.ContainsKey(vector2))
                    {
                      this.objects.Add(vector2, new Object(vector2, Game1.random.NextDouble() < 0.5 ? 343 : 450, 1));
                      continue;
                    }
                    continue;
                  case 17:
                    if (!this.objects.ContainsKey(vector2))
                    {
                      this.objects.Add(vector2, new Object(vector2, Game1.random.NextDouble() < 0.5 ? 343 : 450, 1));
                      continue;
                    }
                    continue;
                  case 18:
                    if (!this.objects.ContainsKey(vector2))
                    {
                      this.objects.Add(vector2, new Object(vector2, Game1.random.NextDouble() < 0.5 ? 294 : 295, 1));
                      continue;
                    }
                    continue;
                  case 28:
                    if (this.isTileLocationTotallyClearAndPlaceable(vector2) && this.characters.Count < 50)
                    {
                      this.characters.Add((NPC) new Grub(new Vector2(vector2.X * (float) Game1.tileSize, vector2.Y * (float) Game1.tileSize), true));
                      continue;
                    }
                    continue;
                  default:
                    continue;
                }
              }
            }
          }
        }
      }
      this.addLightGlows();
    }

    public void addLightGlows()
    {
      if (this.isOutdoors || Game1.timeOfDay >= 1900 && !Game1.newDay)
        return;
      this.lightGlows.Clear();
      PropertyValue propertyValue;
      this.map.Properties.TryGetValue("DayTiles", out propertyValue);
      if (propertyValue == null)
        return;
      string[] strArray = propertyValue.ToString().Trim().Split(' ');
      int index = 0;
      while (index < strArray.Length)
      {
        if (this.map.GetLayer(strArray[index]).PickTile(new Location(Convert.ToInt32(strArray[index + 1]) * Game1.tileSize, Convert.ToInt32(strArray[index + 2]) * Game1.tileSize), new Size(Game1.viewport.Width, Game1.viewport.Height)) != null)
        {
          this.map.GetLayer(strArray[index]).Tiles[Convert.ToInt32(strArray[index + 1]), Convert.ToInt32(strArray[index + 2])].TileIndex = Convert.ToInt32(strArray[index + 3]);
          switch (Convert.ToInt32(strArray[index + 3]))
          {
            case 405:
              this.lightGlows.Add(new Vector2((float) Convert.ToInt32(strArray[index + 1]), (float) Convert.ToInt32(strArray[index + 2])) * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2)));
              this.lightGlows.Add(new Vector2((float) Convert.ToInt32(strArray[index + 1]), (float) Convert.ToInt32(strArray[index + 2])) * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize * 3 / 2), (float) (Game1.tileSize / 2)));
              break;
            case 469:
              this.lightGlows.Add(new Vector2((float) Convert.ToInt32(strArray[index + 1]), (float) Convert.ToInt32(strArray[index + 2])) * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2 + Game1.pixelZoom)));
              break;
            case 1224:
              this.lightGlows.Add(new Vector2((float) Convert.ToInt32(strArray[index + 1]), (float) Convert.ToInt32(strArray[index + 2])) * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2)));
              break;
            case 256:
              this.lightGlows.Add(new Vector2((float) Convert.ToInt32(strArray[index + 1]), (float) Convert.ToInt32(strArray[index + 2])) * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), (float) Game1.tileSize));
              break;
            case 257:
              this.lightGlows.Add(new Vector2((float) Convert.ToInt32(strArray[index + 1]), (float) Convert.ToInt32(strArray[index + 2])) * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), (float) -Game1.pixelZoom));
              break;
          }
        }
        index += 4;
      }
    }

    public NPC isCharacterAtTile(Vector2 tileLocation)
    {
      NPC npc = (NPC) null;
      tileLocation.X = (float) (int) tileLocation.X;
      tileLocation.Y = (float) (int) tileLocation.Y;
      if (this.currentEvent == null)
      {
        foreach (NPC character in this.characters)
        {
          if (character.getTileLocation().Equals(tileLocation))
            return character;
        }
      }
      else
      {
        foreach (NPC actor in this.currentEvent.actors)
        {
          if (actor.getTileLocation().Equals(tileLocation))
            return actor;
        }
      }
      return npc;
    }

    public void UpdateCharacterDialogues()
    {
      for (int index = this.characters.Count - 1; index >= 0; --index)
        this.characters[index].updateDialogue();
    }

    public string getMapProperty(string propertyName)
    {
      PropertyValue propertyValue = (PropertyValue) null;
      this.map.Properties.TryGetValue(propertyName, out propertyValue);
      return propertyValue.ToString();
    }

    public void tryToAddCritters(bool onlyIfOnScreen = false)
    {
      if (Game1.CurrentEvent != null)
        return;
      double num1 = (double) (this.map.Layers[0].LayerWidth * this.map.Layers[0].LayerHeight);
      double num2;
      double chance1 = num2 = Math.Max(0.15, Math.Min(0.5, num1 / 15000.0));
      double chance2 = num2;
      double num3 = 2.0;
      double chance3 = num2 / num3;
      double num4 = 2.0;
      double chance4 = num2 / num4;
      double num5 = 8.0;
      double chance5 = num2 / num5;
      double num6 = 2.0;
      double num7 = num2 * num6;
      if (Game1.isRaining)
        return;
      this.addClouds(num7 / (onlyIfOnScreen ? 2.0 : 1.0), onlyIfOnScreen);
      if (this is Beach || this.critters == null || this.critters.Count > (Game1.currentSeason.Equals("summer") ? 20 : 10))
        return;
      this.addBirdies(chance1, onlyIfOnScreen);
      this.addButterflies(chance2, onlyIfOnScreen);
      this.addBunnies(chance3, onlyIfOnScreen);
      this.addSquirrels(chance4, onlyIfOnScreen);
      this.addWoodpecker(chance5, onlyIfOnScreen);
      if (!Game1.isDarkOut() || Game1.random.NextDouble() >= 0.01)
        return;
      this.addOwl();
    }

    public void addClouds(double chance, bool onlyIfOnScreen = false)
    {
      if (!Game1.currentSeason.Equals("summer") || Game1.isRaining || (Game1.weatherIcon == 4 || Game1.timeOfDay >= Game1.getStartingToGetDarkTime() - 100))
        return;
      while (Game1.random.NextDouble() < Math.Min(0.9, chance))
      {
        Vector2 position = this.getRandomTile();
        if (onlyIfOnScreen)
          position = Game1.random.NextDouble() < 0.5 ? new Vector2((float) this.map.Layers[0].LayerWidth, (float) Game1.random.Next(this.map.Layers[0].LayerHeight)) : new Vector2((float) Game1.random.Next(this.map.Layers[0].LayerWidth), (float) this.map.Layers[0].LayerHeight);
        if (onlyIfOnScreen || !Utility.isOnScreen(position * (float) Game1.tileSize, Game1.tileSize * 20))
        {
          Cloud cloud = new Cloud(position);
          bool flag = true;
          if (this.critters != null)
          {
            foreach (Critter critter in this.critters)
            {
              if (critter is Cloud && critter.getBoundingBox(0, 0).Intersects(cloud.getBoundingBox(0, 0)))
              {
                flag = false;
                break;
              }
            }
          }
          if (flag)
          {
            Game1.debugOutput = "added CLOUD at " + (object) position.X + "," + (object) position.Y;
            this.addCritter((Critter) cloud);
          }
        }
      }
    }

    public void addOwl()
    {
      this.critters.Add((Critter) new Owl(new Vector2((float) Game1.random.Next(Game1.tileSize, this.map.Layers[0].LayerWidth * Game1.tileSize - Game1.tileSize), (float) (-Game1.tileSize * 2))));
    }

    public void setFireplace(bool on, int tileLocationX, int tileLocationY, bool playSound = true)
    {
      int num1 = 944468 + tileLocationX * 1000 + tileLocationY;
      if (on)
      {
        List<TemporaryAnimatedSprite> temporarySprites1 = this.temporarySprites;
        TemporaryAnimatedSprite temporaryAnimatedSprite1 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(276, 1985, 12, 11), new Vector2((float) tileLocationX, (float) tileLocationY) * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), (float) (-Game1.tileSize / 2)), false, 0.0f, Color.White);
        temporaryAnimatedSprite1.interval = 50f;
        temporaryAnimatedSprite1.totalNumberOfLoops = 99999;
        temporaryAnimatedSprite1.animationLength = 4;
        temporaryAnimatedSprite1.light = true;
        temporaryAnimatedSprite1.lightID = num1;
        double num2 = (double) num1;
        temporaryAnimatedSprite1.id = (float) num2;
        double num3 = 2.0;
        temporaryAnimatedSprite1.lightRadius = (float) num3;
        double pixelZoom1 = (double) Game1.pixelZoom;
        temporaryAnimatedSprite1.scale = (float) pixelZoom1;
        double num4 = ((double) tileLocationY + 1.10000002384186) * (double) Game1.tileSize / 10000.0;
        temporaryAnimatedSprite1.layerDepth = (float) num4;
        temporarySprites1.Add(temporaryAnimatedSprite1);
        List<TemporaryAnimatedSprite> temporarySprites2 = this.temporarySprites;
        TemporaryAnimatedSprite temporaryAnimatedSprite2 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(276, 1985, 12, 11), new Vector2((float) (tileLocationX + 1), (float) tileLocationY) * (float) Game1.tileSize + new Vector2((float) (-Game1.tileSize / 4), (float) (-Game1.tileSize / 2)), false, 0.0f, Color.White);
        temporaryAnimatedSprite2.delayBeforeAnimationStart = 10;
        temporaryAnimatedSprite2.interval = 50f;
        temporaryAnimatedSprite2.totalNumberOfLoops = 99999;
        temporaryAnimatedSprite2.animationLength = 4;
        temporaryAnimatedSprite2.light = true;
        temporaryAnimatedSprite2.lightID = num1;
        double num5 = (double) num1;
        temporaryAnimatedSprite2.id = (float) num5;
        double num6 = 2.0;
        temporaryAnimatedSprite2.lightRadius = (float) num6;
        double pixelZoom2 = (double) Game1.pixelZoom;
        temporaryAnimatedSprite2.scale = (float) pixelZoom2;
        double num7 = ((double) tileLocationY + 1.10000002384186) * (double) Game1.tileSize / 10000.0;
        temporaryAnimatedSprite2.layerDepth = (float) num7;
        temporarySprites2.Add(temporaryAnimatedSprite2);
        if (playSound)
          Game1.playSound("fireball");
        AmbientLocationSounds.addSound(new Vector2((float) tileLocationX, (float) tileLocationY), 1);
      }
      else
      {
        this.removeTemporarySpritesWithID(num1);
        Utility.removeLightSource(num1);
        if (playSound)
          Game1.playSound("fireball");
        AmbientLocationSounds.removeSound(new Vector2((float) tileLocationX, (float) tileLocationY));
      }
    }

    public void addWoodpecker(double chance, bool onlyIfOnScreen = false)
    {
      if (Game1.isStartingToGetDarkOut() || onlyIfOnScreen || (this is Town || this is Desert) || Game1.random.NextDouble() >= chance)
        return;
      for (int index1 = 0; index1 < 3; ++index1)
      {
        int index2 = Game1.random.Next(this.terrainFeatures.Count);
        if (this.terrainFeatures.Count > 0 && this.terrainFeatures.ElementAt<KeyValuePair<Vector2, TerrainFeature>>(index2).Value is Tree && ((this.terrainFeatures.ElementAt<KeyValuePair<Vector2, TerrainFeature>>(index2).Value as Tree).treeType != 2 && (this.terrainFeatures.ElementAt<KeyValuePair<Vector2, TerrainFeature>>(index2).Value as Tree).growthStage >= 5))
        {
          List<Critter> critters = this.critters;
          KeyValuePair<Vector2, TerrainFeature> keyValuePair = this.terrainFeatures.ElementAt<KeyValuePair<Vector2, TerrainFeature>>(index2);
          Tree tree = keyValuePair.Value as Tree;
          keyValuePair = this.terrainFeatures.ElementAt<KeyValuePair<Vector2, TerrainFeature>>(index2);
          Vector2 key = keyValuePair.Key;
          Woodpecker woodpecker = new Woodpecker(tree, key);
          critters.Add((Critter) woodpecker);
          break;
        }
      }
    }

    public void addSquirrels(double chance, bool onlyIfOnScreen = false)
    {
      if (Game1.isStartingToGetDarkOut() || onlyIfOnScreen || (this is Farm || this is Town) || (this is Desert || Game1.random.NextDouble() >= chance))
        return;
      for (int index1 = 0; index1 < 3; ++index1)
      {
        int index2 = Game1.random.Next(this.terrainFeatures.Count);
        if (this.terrainFeatures.Count > 0 && this.terrainFeatures.ElementAt<KeyValuePair<Vector2, TerrainFeature>>(index2).Value is Tree && ((this.terrainFeatures.ElementAt<KeyValuePair<Vector2, TerrainFeature>>(index2).Value as Tree).growthStage >= 5 && !(this.terrainFeatures.ElementAt<KeyValuePair<Vector2, TerrainFeature>>(index2).Value as Tree).stump))
        {
          Vector2 key = this.terrainFeatures.ElementAt<KeyValuePair<Vector2, TerrainFeature>>(index2).Key;
          int num = Game1.random.Next(4, 7);
          bool flip = Game1.random.NextDouble() < 0.5;
          bool flag = true;
          for (int index3 = 0; index3 < num; ++index3)
          {
            key.X += flip ? 1f : -1f;
            if (!this.isTileLocationTotallyClearAndPlaceable(key))
            {
              flag = false;
              break;
            }
          }
          if (flag)
          {
            this.critters.Add((Critter) new Squirrel(key, flip));
            break;
          }
        }
      }
    }

    public void addBunnies(double chance, bool onlyIfOnScreen = false)
    {
      if (onlyIfOnScreen || this is Farm || (this is Desert || Game1.random.NextDouble() >= chance) || this.largeTerrainFeatures == null)
        return;
      for (int index1 = 0; index1 < 3; ++index1)
      {
        int index2 = Game1.random.Next(this.largeTerrainFeatures.Count);
        if (this.largeTerrainFeatures.Count > 0 && this.largeTerrainFeatures[index2] is Bush)
        {
          Vector2 tilePosition = this.largeTerrainFeatures[index2].tilePosition;
          int num = Game1.random.Next(5, 12);
          bool flip = Game1.random.NextDouble() < 0.5;
          bool flag = true;
          for (int index3 = 0; index3 < num; ++index3)
          {
            tilePosition.X += flip ? 1f : -1f;
            if (!this.largeTerrainFeatures[index2].getBoundingBox().Intersects(new Microsoft.Xna.Framework.Rectangle((int) tilePosition.X * Game1.tileSize, (int) tilePosition.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize)) && !this.isTileLocationTotallyClearAndPlaceable(tilePosition))
            {
              flag = false;
              break;
            }
          }
          if (flag)
          {
            this.critters.Add((Critter) new Rabbit(tilePosition, flip));
            break;
          }
        }
      }
    }

    public void addCritter(Critter c)
    {
      if (this.critters == null)
        return;
      this.critters.Add(c);
    }

    public void addButterflies(double chance, bool onlyIfOnScreen = false)
    {
      bool flag = Game1.currentSeason.Equals("summer") && Game1.isDarkOut();
      if (Game1.timeOfDay >= 1500 && !flag || !Game1.currentSeason.Equals("spring") && !Game1.currentSeason.Equals("summer"))
        return;
      chance = Math.Min(0.8, chance * 1.5);
      while (Game1.random.NextDouble() < chance)
      {
        Vector2 randomTile = this.getRandomTile();
        if (!onlyIfOnScreen || !Utility.isOnScreen(randomTile * (float) Game1.tileSize, Game1.tileSize))
        {
          if (flag)
            this.critters.Add((Critter) new Firefly(randomTile));
          else
            this.critters.Add((Critter) new Butterfly(randomTile));
          while (Game1.random.NextDouble() < 0.4)
          {
            if (flag)
              this.critters.Add((Critter) new Firefly(randomTile + new Vector2((float) Game1.random.Next(-2, 3), (float) Game1.random.Next(-2, 3))));
            else
              this.critters.Add((Critter) new Butterfly(randomTile + new Vector2((float) Game1.random.Next(-2, 3), (float) Game1.random.Next(-2, 3))));
          }
        }
      }
    }

    public void addBirdies(double chance, bool onlyIfOnScreen = false)
    {
      if (Game1.timeOfDay >= 1500 || this is Desert || (this is Railroad || this is Farm) || Game1.currentSeason.Equals("summer"))
        return;
label_10:
      while (Game1.random.NextDouble() < chance)
      {
        int num1 = Game1.random.Next(1, 4);
        bool flag = false;
        int num2 = 0;
        while (true)
        {
          if (!flag && num2 < 5)
          {
            Vector2 randomTile = this.getRandomTile();
            if ((!onlyIfOnScreen || !Utility.isOnScreen(randomTile * (float) Game1.tileSize, Game1.tileSize)) && this.isAreaClear(new Microsoft.Xna.Framework.Rectangle((int) randomTile.X - 2, (int) randomTile.Y - 2, 5, 5)))
            {
              List<Critter> crittersToAdd = new List<Critter>();
              for (int index = 0; index < num1; ++index)
                crittersToAdd.Add((Critter) new Birdie(-100, -100, Game1.currentSeason.Equals("fall") ? 45 : 25));
              this.addCrittersStartingAtTile(randomTile, crittersToAdd);
              flag = true;
            }
            ++num2;
          }
          else
            goto label_10;
        }
      }
    }

    public void addJumperFrog(Vector2 tileLocation)
    {
      if (this.critters == null)
        return;
      this.critters.Add((Critter) new Frog(tileLocation, false, false));
    }

    public void addFrog()
    {
      if (!Game1.isRaining || Game1.currentSeason.Equals("winter"))
        return;
      for (int index1 = 0; index1 < 3; ++index1)
      {
        Vector2 randomTile = this.getRandomTile();
        if (this.doesTileHaveProperty((int) randomTile.X, (int) randomTile.Y, "Water", "Back") != null)
        {
          int num = 10;
          bool forceFlip = Game1.random.NextDouble() < 0.5;
          for (int index2 = 0; index2 < num; ++index2)
          {
            randomTile.X += forceFlip ? 1f : -1f;
            if (this.doesTileHaveProperty((int) randomTile.X, (int) randomTile.Y, "Water", "Back") == null && this.isTileOnMap((int) randomTile.X, (int) randomTile.Y))
            {
              this.critters.Add((Critter) new Frog(randomTile, true, forceFlip));
              return;
            }
          }
        }
      }
    }

    public void checkForSpecialCharacterIconAtThisTile(Vector2 tileLocation)
    {
      if (this.currentEvent == null)
        return;
      this.currentEvent.checkForSpecialCharacterIconAtThisTile(tileLocation);
    }

    private void addCrittersStartingAtTile(Vector2 tile, List<Critter> crittersToAdd)
    {
      if (crittersToAdd == null)
        return;
      for (int index = 0; crittersToAdd.Count > 0 && index < 20; ++index)
      {
        if (this.isTileLocationTotallyClearAndPlaceable(tile))
        {
          crittersToAdd.Last<Critter>().position = tile * (float) Game1.tileSize;
          crittersToAdd.Last<Critter>().startingPosition = tile * (float) Game1.tileSize;
          this.critters.Add(crittersToAdd.Last<Critter>());
          crittersToAdd.RemoveAt(crittersToAdd.Count - 1);
          tile = Utility.getTranslatedVector2(tile, Game1.random.Next(4), 1f);
        }
      }
    }

    public bool isAreaClear(Microsoft.Xna.Framework.Rectangle area)
    {
      for (int left = area.Left; left < area.Right; ++left)
      {
        for (int top = area.Top; top < area.Bottom; ++top)
        {
          if (!this.isTileLocationTotallyClearAndPlaceable(new Vector2((float) left, (float) top)))
            return false;
        }
      }
      return true;
    }

    public Vector2 getRandomTile()
    {
      return new Vector2((float) Game1.random.Next(this.map.Layers[0].LayerWidth), (float) Game1.random.Next(this.map.Layers[0].LayerHeight));
    }

    public void setUpLocationSpecificFlair()
    {
      if (!this.isOutdoors && !(this is FarmHouse))
      {
        PropertyValue propertyValue;
        this.map.Properties.TryGetValue("AmbientLight", out propertyValue);
        if (propertyValue == null)
          Game1.ambientLight = new Color(100, 120, 30);
      }
      string name = this.name;
      // ISSUE: reference to a compiler-generated method
      uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(name);
      if (stringHash <= 1840909614U)
      {
        if (stringHash <= 807500499U)
        {
          if (stringHash <= 636013712U)
          {
            if ((int) stringHash != 524243468)
            {
              if ((int) stringHash != 636013712 || !(name == "HaleyHouse") || !Game1.player.eventsSeen.Contains(463391) || Game1.player.spouse != null && Game1.player.spouse.Equals("Emily"))
                return;
              this.setMapTileIndex(14, 4, 2173, "Buildings", 0);
              this.setMapTileIndex(14, 3, 2141, "Buildings", 0);
              this.setMapTileIndex(14, 3, 219, "Back", 0);
              this.temporarySprites.Add((TemporaryAnimatedSprite) new EmilysParrot(new Vector2((float) (14 * Game1.tileSize + Game1.pixelZoom * 4), (float) (3 * Game1.tileSize - Game1.pixelZoom * 8))));
            }
            else
            {
              if (!(name == "BugLand"))
                return;
              if (!Game1.player.hasDarkTalisman && this.isTileLocationTotallyClearAndPlaceable(31, 5))
              {
                SerializableDictionary<Vector2, Object> objects = this.objects;
                Vector2 key = new Vector2(31f, 5f);
                int coins = 0;
                List<Item> items = new List<Item>();
                items.Add((Item) new SpecialItem(1, 6, ""));
                Vector2 location = new Vector2(31f, 5f);
                int num = 0;
                objects.Add(key, (Object) new Chest(coins, items, location, num != 0)
                {
                  tint = Color.Gray
                });
              }
              foreach (NPC character in this.characters)
              {
                if (character is Grub)
                  (character as Grub).setHard();
                else if (character is Fly)
                  (character as Fly).setHard();
              }
            }
          }
          else if ((int) stringHash != 720888915)
          {
            if ((int) stringHash != 746089795)
            {
              if ((int) stringHash != 807500499 || !(name == "Hospital"))
                return;
              if (!Game1.isRaining)
                Game1.changeMusicTrack("distantBanjo");
              Game1.ambientLight = new Color(100, 100, 60);
              if (Game1.random.NextDouble() >= 0.5)
                return;
              NPC characterFromName = Game1.getCharacterFromName("Maru", false);
              if ((object) characterFromName == null)
                return;
              string path1 = "";
              switch (Game1.random.Next(5))
              {
                case 0:
                  path1 = "Strings\\SpeechBubbles:Hospital_Maru_Greeting1";
                  break;
                case 1:
                  path1 = "Strings\\SpeechBubbles:Hospital_Maru_Greeting2";
                  break;
                case 2:
                  path1 = "Strings\\SpeechBubbles:Hospital_Maru_Greeting3";
                  break;
                case 3:
                  path1 = "Strings\\SpeechBubbles:Hospital_Maru_Greeting4";
                  break;
                case 4:
                  path1 = "Strings\\SpeechBubbles:Hospital_Maru_Greeting5";
                  break;
              }
              if (Game1.player.spouse != null && Game1.player.spouse.Equals("Maru"))
              {
                string path2 = "Strings\\SpeechBubbles:Hospital_Maru_Spouse";
                characterFromName.showTextAboveHead(Game1.content.LoadString(path2), 2, 2, 3000, 0);
              }
              else
                characterFromName.showTextAboveHead(Game1.content.LoadString(path1), -1, 2, 3000, 0);
            }
            else
            {
              if (!(name == "ScienceHouse") || Game1.random.NextDouble() >= 0.5 || !Game1.player.currentLocation.isOutdoors)
                return;
              NPC characterFromName = Game1.getCharacterFromName("Robin", false);
              if ((object) characterFromName == null || characterFromName.getTileY() != 18)
                return;
              string path = "";
              switch (Game1.random.Next(4))
              {
                case 0:
                  path = Game1.isRaining ? "Strings\\SpeechBubbles:ScienceHouse_Robin_Raining1" : "Strings\\SpeechBubbles:ScienceHouse_Robin_NotRaining1";
                  break;
                case 1:
                  path = Game1.isSnowing ? "Strings\\SpeechBubbles:ScienceHouse_Robin_Snowing" : "Strings\\SpeechBubbles:ScienceHouse_Robin_NotSnowing";
                  break;
                case 2:
                  path = Game1.player.getFriendshipHeartLevelForNPC("Robin") > 4 ? "Strings\\SpeechBubbles:ScienceHouse_Robin_CloseFriends" : "Strings\\SpeechBubbles:ScienceHouse_Robin_NotCloseFriends";
                  break;
                case 3:
                  path = Game1.isRaining ? "Strings\\SpeechBubbles:ScienceHouse_Robin_Raining2" : "Strings\\SpeechBubbles:ScienceHouse_Robin_NotRaining2";
                  break;
                case 4:
                  path = "Strings\\SpeechBubbles:ScienceHouse_Robin_Greeting";
                  break;
              }
              if (Game1.random.NextDouble() < 0.001)
                path = "Strings\\SpeechBubbles:ScienceHouse_Robin_RareGreeting";
              characterFromName.showTextAboveHead(Game1.content.LoadString(path, (object) Game1.player.name), -1, 2, 3000, 0);
            }
          }
          else
          {
            if (!(name == "JojaMart"))
              return;
            Game1.changeMusicTrack("Hospital_Ambient");
            Game1.ambientLight = new Color(0, 0, 0);
            if (Game1.random.NextDouble() >= 0.5)
              return;
            NPC characterFromName = Game1.getCharacterFromName("Morris", false);
            if ((object) characterFromName == null)
              return;
            string path = "Strings\\SpeechBubbles:JojaMart_Morris_Greeting";
            characterFromName.showTextAboveHead(Game1.content.LoadString(path), -1, 2, 3000, 0);
          }
        }
        else if (stringHash <= 1367472567U)
        {
          if ((int) stringHash != 1167876998)
          {
            if ((int) stringHash != 1367472567 || !(name == "Blacksmith"))
              return;
            AmbientLocationSounds.addSound(new Vector2(9f, 10f), 2);
            AmbientLocationSounds.changeSpecificVariable("Frequency", 2f, 2);
            Game1.changeMusicTrack("none");
          }
          else
          {
            if (!(name == "ManorHouse"))
              return;
            Game1.ambientLight = new Color(150, 120, 50);
            NPC characterFromName = Game1.getCharacterFromName("Lewis", false);
            if ((object) characterFromName == null)
              return;
            string str = Game1.timeOfDay < 1200 ? "Morning" : (Game1.timeOfDay < 1700 ? "Afternoon" : "Evening");
            characterFromName.faceTowardFarmerForPeriod(3000, 15, false, Game1.player);
            characterFromName.showTextAboveHead(Game1.content.LoadString("Strings\\SpeechBubbles:ManorHouse_Lewis_" + str), -1, 2, 3000, 0);
          }
        }
        else if ((int) stringHash != 1428365440)
        {
          if ((int) stringHash != 1446049731)
          {
            if ((int) stringHash != 1840909614 || !(name == "SandyHouse"))
              return;
            Game1.changeMusicTrack("distantBanjo");
            Game1.ambientLight = new Color(0, 0, 0);
            if (Game1.random.NextDouble() >= 0.5)
              return;
            NPC characterFromName = Game1.getCharacterFromName("Sandy", false);
            if ((object) characterFromName == null)
              return;
            string path = "";
            switch (Game1.random.Next(5))
            {
              case 0:
                path = "Strings\\SpeechBubbles:SandyHouse_Sandy_Greeting1";
                break;
              case 1:
                path = "Strings\\SpeechBubbles:SandyHouse_Sandy_Greeting2";
                break;
              case 2:
                path = "Strings\\SpeechBubbles:SandyHouse_Sandy_Greeting3";
                break;
              case 3:
                path = "Strings\\SpeechBubbles:SandyHouse_Sandy_Greeting4";
                break;
              case 4:
                path = "Strings\\SpeechBubbles:SandyHouse_Sandy_Greeting5";
                break;
            }
            characterFromName.showTextAboveHead(Game1.content.LoadString(path), -1, 2, 3000, 0);
          }
          else
          {
            if (!(name == "CommunityCenter") || !(this is CommunityCenter) || !Game1.isLocationAccessible("CommunityCenter"))
              return;
            this.setFireplace(true, 31, 8, false);
            this.setFireplace(true, 32, 8, false);
            this.setFireplace(true, 33, 8, false);
          }
        }
        else
        {
          if (!(name == "SeedShop"))
            return;
          this.setFireplace(true, 25, 13, false);
          if (Game1.random.NextDouble() >= 0.5)
            return;
          NPC characterFromName = Game1.getCharacterFromName("Pierre", false);
          if ((object) characterFromName == null || characterFromName.getTileY() != 17)
            return;
          string str = "";
          switch (Game1.random.Next(5))
          {
            case 0:
              str = Game1.IsWinter ? "Winter" : "NotWinter";
              break;
            case 1:
              str = Game1.IsSummer ? "Summer" : "NotSummer";
              break;
            case 2:
              str = "Greeting1";
              break;
            case 3:
              str = "Greeting2";
              break;
            case 4:
              str = Game1.isRaining ? "Raining" : "NotRaining";
              break;
          }
          if (Game1.random.NextDouble() < 0.001)
            str = "RareGreeting";
          characterFromName.showTextAboveHead(Game1.content.LoadString("Strings\\SpeechBubbles:SeedShop_Pierre_" + str, (object) Game1.player.Name), -1, 2, 3000, 0);
        }
      }
      else if (stringHash <= 2844260897U)
      {
        if (stringHash <= 2233558176U)
        {
          if ((int) stringHash != 1919215024)
          {
            if ((int) stringHash != -2061409120 || !(name == "Greenhouse") || !Game1.isDarkOut())
              return;
            Game1.ambientLight = Game1.outdoorLight;
          }
          else
          {
            if (!(name == "ElliottHouse"))
              return;
            Game1.changeMusicTrack("communityCenter");
            NPC characterFromName = Game1.getCharacterFromName("Elliott", false);
            if ((object) characterFromName == null)
              return;
            string path = "";
            switch (Game1.random.Next(3))
            {
              case 0:
                path = "Strings\\SpeechBubbles:ElliottHouse_Elliott_Greeting1";
                break;
              case 1:
                path = "Strings\\SpeechBubbles:ElliottHouse_Elliott_Greeting2";
                break;
              case 2:
                path = "Strings\\SpeechBubbles:ElliottHouse_Elliott_Greeting3";
                break;
            }
            characterFromName.faceTowardFarmerForPeriod(3000, 15, false, Game1.player);
            characterFromName.showTextAboveHead(Game1.content.LoadString(path, (object) Game1.player.name), -1, 2, 3000, 0);
          }
        }
        else if ((int) stringHash != -1585981025)
        {
          if ((int) stringHash != -1453563620)
          {
            if ((int) stringHash != -1450706399 || !(name == "Woods"))
              return;
            Game1.ambientLight = new Color(150, 120, 50);
          }
          else
          {
            if (!(name == "WitchSwamp"))
              return;
            if (Game1.player.mailReceived.Contains("henchmanGone"))
              this.removeTile(20, 29, "Buildings");
            else
              this.setMapTileIndex(20, 29, 10, "Buildings", 0);
          }
        }
        else
        {
          if (!(name == "ArchaeologyHouse"))
            return;
          this.setFireplace(true, 43, 4, false);
          if (Game1.random.NextDouble() >= 0.5 || !Game1.player.hasOrWillReceiveMail("artifactFound"))
            return;
          NPC characterFromName = Game1.getCharacterFromName("Gunther", false);
          if ((object) characterFromName == null)
            return;
          string str = "";
          switch (Game1.random.Next(5))
          {
            case 0:
              str = "Greeting1";
              break;
            case 1:
              str = "Greeting2";
              break;
            case 2:
              str = "Greeting3";
              break;
            case 3:
              str = "Greeting4";
              break;
            case 4:
              str = "Greeting5";
              break;
          }
          if (Game1.random.NextDouble() < 0.001)
            str = "RareGreeting";
          characterFromName.showTextAboveHead(Game1.content.LoadString("Strings\\SpeechBubbles:ArchaeologyHouse_Gunther_" + str), -1, 2, 3000, 0);
        }
      }
      else if (stringHash <= 3030632101U)
      {
        if ((int) stringHash != -1385590711)
        {
          if ((int) stringHash != -1264335195 || !(name == "LeahHouse"))
            return;
          Game1.changeMusicTrack("distantBanjo");
          NPC characterFromName = Game1.getCharacterFromName("Leah", false);
          if (Game1.IsFall || Game1.IsWinter || Game1.isRaining)
            this.setFireplace(true, 11, 4, false);
          if ((object) characterFromName == null)
            return;
          string path = "";
          switch (Game1.random.Next(3))
          {
            case 0:
              path = "Strings\\SpeechBubbles:LeahHouse_Leah_Greeting1";
              break;
            case 1:
              path = "Strings\\SpeechBubbles:LeahHouse_Leah_Greeting2";
              break;
            case 2:
              path = "Strings\\SpeechBubbles:LeahHouse_Leah_Greeting3";
              break;
          }
          characterFromName.faceTowardFarmerForPeriod(3000, 15, false, Game1.player);
          characterFromName.showTextAboveHead(Game1.content.LoadString(path, (object) Game1.player.name), -1, 2, 3000, 0);
        }
        else
        {
          if (!(name == "Saloon"))
            return;
          if (Game1.timeOfDay >= 1700)
          {
            this.setFireplace(true, 22, 17, false);
            Game1.changeMusicTrack("Saloon1");
          }
          if (Game1.random.NextDouble() >= 0.25)
            return;
          NPC characterFromName = Game1.getCharacterFromName("Gus", false);
          if ((object) characterFromName == null || characterFromName.getTileY() != 18)
            return;
          string str = "";
          switch (Game1.random.Next(5))
          {
            case 0:
              str = "Greeting";
              break;
            case 1:
              str = Game1.IsSummer ? "Summer" : "NotSummer";
              break;
            case 2:
              str = Game1.isSnowing ? "Snowing1" : "NotSnowing1";
              break;
            case 3:
              str = Game1.isRaining ? "Raining" : "NotRaining";
              break;
            case 4:
              str = Game1.isSnowing ? "Snowing2" : "NotSnowing2";
              break;
          }
          if (Game1.random.NextDouble() < 0.001)
            str = "RareGreeting";
          characterFromName.showTextAboveHead(Game1.content.LoadString("Strings\\SpeechBubbles:Saloon_Gus_" + str), -1, 2, 3000, 0);
        }
      }
      else if ((int) stringHash != -1199265098)
      {
        if ((int) stringHash != -539377511)
        {
          if ((int) stringHash != -316155903 || !(name == "AnimalShop"))
            return;
          this.setFireplace(true, 3, 14, false);
          if (Game1.random.NextDouble() >= 0.5)
            return;
          NPC characterFromName = Game1.getCharacterFromName("Marnie", false);
          if ((object) characterFromName == null || characterFromName.getTileY() != 14)
            return;
          string path = "";
          switch (Game1.random.Next(4))
          {
            case 0:
              path = "Strings\\SpeechBubbles:AnimalShop_Marnie_Greeting1";
              break;
            case 1:
              path = "Strings\\SpeechBubbles:AnimalShop_Marnie_Greeting2";
              break;
            case 2:
              path = Game1.player.getFriendshipHeartLevelForNPC("Marnie") > 4 ? "Strings\\SpeechBubbles:AnimalShop_Marnie_CloseFriends" : "Strings\\SpeechBubbles:AnimalShop_Marnie_NotCloseFriends";
              break;
            case 3:
              path = Game1.isRaining ? "Strings\\SpeechBubbles:AnimalShop_Marnie_Raining" : "Strings\\SpeechBubbles:AnimalShop_Marnie_NotRaining";
              break;
            case 4:
              path = "Strings\\SpeechBubbles:AnimalShop_Marnie_Greeting3";
              break;
          }
          if (Game1.random.NextDouble() < 0.001)
            path = "Strings\\SpeechBubbles:AnimalShop_Marnie_RareGreeting";
          characterFromName.showTextAboveHead(Game1.content.LoadString(path, (object) Game1.player.name, (object) Game1.player.farmName), -1, 2, 3000, 0);
        }
        else
        {
          if (!(name == "WitchHut") || !Game1.player.mailReceived.Contains("hasPickedUpMagicInk"))
            return;
          this.setMapTileIndex(4, 11, 113, "Buildings", 0);
          ((IDictionary<string, PropertyValue>) this.map.GetLayer("Buildings").Tiles[4, 11].Properties).Remove("Action");
        }
      }
      else
      {
        if (!(name == "AdventureGuild"))
          return;
        this.setFireplace(true, 9, 11, false);
        if (Game1.random.NextDouble() >= 0.5)
          return;
        NPC characterFromName = Game1.getCharacterFromName("Marlon", false);
        if ((object) characterFromName == null)
          return;
        string path = "";
        switch (Game1.random.Next(5))
        {
          case 0:
            path = "Strings\\SpeechBubbles:AdventureGuild_Marlon_Greeting_" + (Game1.player.isMale ? "Male" : "Female");
            break;
          case 1:
            path = "Strings\\SpeechBubbles:AdventureGuild_Marlon_Greeting1";
            break;
          case 2:
            path = "Strings\\SpeechBubbles:AdventureGuild_Marlon_Greeting2";
            break;
          case 3:
            path = "Strings\\SpeechBubbles:AdventureGuild_Marlon_Greeting3";
            break;
          case 4:
            path = "Strings\\SpeechBubbles:AdventureGuild_Marlon_Greeting4";
            break;
        }
        characterFromName.showTextAboveHead(Game1.content.LoadString(path), -1, 2, 3000, 0);
      }
    }

    public virtual void resetForPlayerEntry()
    {
      Utility.killAllStaticLoopingSoundCues();
      if (Game1.CurrentEvent == null && !this.Name.ToLower().Contains("bath"))
        Game1.player.canOnlyWalk = false;
      if (!(this is Farm))
        this.temporarySprites.Clear();
      if (Game1.options != null)
      {
        if (Game1.isOneOfTheseKeysDown(Keyboard.GetState(), Game1.options.runButton))
          Game1.player.setRunning(!Game1.options.autoRun, true);
        else
          Game1.player.setRunning(Game1.options.autoRun, true);
      }
      Game1.UpdateViewPort(false, new Point(Game1.player.getStandingX(), Game1.player.getStandingY()));
      Game1.previousViewportPosition = new Vector2((float) Game1.viewport.X, (float) Game1.viewport.Y);
      foreach (IClickableMenu onScreenMenu in Game1.onScreenMenus)
        onScreenMenu.gameWindowSizeChanged(new Microsoft.Xna.Framework.Rectangle(Game1.viewport.X, Game1.viewport.Y, Game1.viewport.Width, Game1.viewport.Height), new Microsoft.Xna.Framework.Rectangle(Game1.viewport.X, Game1.viewport.Y, Game1.viewport.Width, Game1.viewport.Height));
      if (Game1.player.rightRing != null)
        Game1.player.rightRing.onNewLocation(Game1.player, this);
      if (Game1.player.leftRing != null)
        Game1.player.leftRing.onNewLocation(Game1.player, this);
      this.forceViewportPlayerFollow = this.map.Properties.ContainsKey("ViewportFollowPlayer");
      this.lastTouchActionLocation = Game1.player.getTileLocation();
      for (int index = Game1.player.questLog.Count - 1; index >= 0; --index)
        Game1.player.questLog[index].adjustGameLocation(this);
      for (int index = this.characters.Count - 1; index >= 0; --index)
        this.characters[index].behaviorOnFarmerLocationEntry(this, Game1.player);
      if (!this.isOutdoors)
      {
        Game1.player.FarmerSprite.currentStep = "thudStep";
        if (this.doorSprites != null)
        {
          foreach (KeyValuePair<Point, TemporaryAnimatedSprite> doorSprite in this.doorSprites)
          {
            doorSprite.Value.reset();
            doorSprite.Value.paused = true;
          }
        }
      }
      if (!this.isOutdoors || this.ignoreOutdoorLighting)
      {
        PropertyValue propertyValue;
        this.map.Properties.TryGetValue("AmbientLight", out propertyValue);
        if (propertyValue != null)
        {
          string[] strArray = propertyValue.ToString().Split(' ');
          Game1.ambientLight = new Color(Convert.ToInt32(strArray[0]), Convert.ToInt32(strArray[1]), Convert.ToInt32(strArray[2]));
        }
        else
          Game1.ambientLight = Game1.isDarkOut() || (double) this.lightLevel > 0.0 ? new Color(180, 180, 0) : Color.White;
        if (Game1.bloom != null)
          Game1.bloom.Visible = false;
        if (Game1.currentSong != null && Game1.currentSong.Name.Contains("ambient"))
          Game1.changeMusicTrack("none");
      }
      else if (!(this is Desert))
      {
        Game1.ambientLight = Game1.isRaining ? new Color((int) byte.MaxValue, 200, 80) : Color.White;
        if (Game1.bloom != null)
          Game1.bloom.Visible = Game1.bloomDay;
      }
      this.setUpLocationSpecificFlair();
      PropertyValue propertyValue1;
      this.map.Properties.TryGetValue("UniqueSprite", out propertyValue1);
      if (propertyValue1 != null)
      {
        string str = propertyValue1.ToString();
        char[] chArray = new char[1]{ ' ' };
        foreach (string name in str.Split(chArray))
        {
          NPC characterFromName = Game1.getCharacterFromName(name, false);
          if (this.characters.Contains(characterFromName))
          {
            try
            {
              characterFromName.sprite.Texture = Game1.content.Load<Texture2D>("Characters\\" + characterFromName.name + "_" + this.name);
              characterFromName.uniqueSpriteActive = true;
            }
            catch (Exception ex)
            {
            }
          }
        }
      }
      PropertyValue propertyValue2;
      this.map.Properties.TryGetValue("UniquePortrait", out propertyValue2);
      if (propertyValue2 != null)
      {
        string str = propertyValue2.ToString();
        char[] chArray = new char[1]{ ' ' };
        foreach (string name in str.Split(chArray))
        {
          NPC characterFromName = Game1.getCharacterFromName(name, false);
          if (this.characters.Contains(characterFromName))
          {
            try
            {
              characterFromName.Portrait = Game1.content.Load<Texture2D>("Portraits\\" + characterFromName.name + "_" + this.name);
              characterFromName.uniquePortraitActive = true;
            }
            catch (Exception ex)
            {
            }
          }
        }
      }
      PropertyValue propertyValue3;
      this.map.Properties.TryGetValue("Light", out propertyValue3);
      if (propertyValue3 != null && !this.ignoreLights)
      {
        string[] strArray = propertyValue3.ToString().Split(' ');
        int index = 0;
        while (index < strArray.Length)
        {
          Game1.currentLightSources.Add(new LightSource(Convert.ToInt32(strArray[index + 2]), new Vector2((float) (Convert.ToInt32(strArray[index]) * Game1.tileSize + Game1.tileSize / 2), (float) (Convert.ToInt32(strArray[index + 1]) * Game1.tileSize + Game1.tileSize / 2)), 1f));
          index += 3;
        }
      }
      if (this.isOutdoors)
      {
        PropertyValue propertyValue4;
        this.map.Properties.TryGetValue("BrookSounds", out propertyValue4);
        if (propertyValue4 != null)
        {
          string[] strArray = propertyValue4.ToString().Split(' ');
          int index = 0;
          while (index < strArray.Length)
          {
            AmbientLocationSounds.addSound(new Vector2((float) Convert.ToInt32(strArray[index]), (float) Convert.ToInt32(strArray[index + 1])), Convert.ToInt32(strArray[index + 2]));
            index += 3;
          }
        }
        Game1.randomizeDebrisWeatherPositions(Game1.debrisWeather);
      }
      foreach (KeyValuePair<Vector2, TerrainFeature> terrainFeature in (Dictionary<Vector2, TerrainFeature>) this.terrainFeatures)
        terrainFeature.Value.performPlayerEntryAction(terrainFeature.Key);
      if (this.largeTerrainFeatures != null)
      {
        foreach (LargeTerrainFeature largeTerrainFeature in this.largeTerrainFeatures)
        {
          Vector2 tilePosition = largeTerrainFeature.tilePosition;
          largeTerrainFeature.performPlayerEntryAction(tilePosition);
        }
      }
      foreach (KeyValuePair<Vector2, Object> keyValuePair in (Dictionary<Vector2, Object>) this.objects)
        keyValuePair.Value.actionOnPlayerEntry();
      if (this.isOutdoors && !Game1.eventUp && ((double) Game1.options.musicVolumeLevel > 0.025000000372529 && Game1.timeOfDay < 1200) && (Game1.currentSong == null || Game1.currentSong.Name.ToLower().Contains("ambient")))
        Game1.playMorningSong();
      if (!(this is MineShaft))
      {
        string lower = Game1.currentSeason.ToLower();
        if (!(lower == "spring"))
        {
          if (!(lower == "summer"))
          {
            if (!(lower == "fall"))
            {
              if (lower == "winter")
                this.waterColor = new Color(130, 80, (int) byte.MaxValue) * 0.5f;
            }
            else
              this.waterColor = new Color((int) byte.MaxValue, 130, 200) * 0.5f;
          }
          else
            this.waterColor = new Color(60, 240, (int) byte.MaxValue) * 0.5f;
        }
        else
          this.waterColor = new Color(120, 200, (int) byte.MaxValue) * 0.5f;
      }
      PropertyValue propertyValue5 = (PropertyValue) null;
      this.map.Properties.TryGetValue("Music", out propertyValue5);
      if (propertyValue5 != null)
      {
        string[] strArray = propertyValue5.ToString().Split(' ');
        if (strArray.Length > 1)
        {
          if (Game1.timeOfDay >= Convert.ToInt32(strArray[0]) && Game1.timeOfDay < Convert.ToInt32(strArray[1]) && !strArray[2].Equals(Game1.currentSong.Name))
            Game1.changeMusicTrack(strArray[2]);
        }
        else if (Game1.currentSong == null || Game1.currentSong.IsStopped || !strArray[0].Equals(Game1.currentSong.Name))
          Game1.changeMusicTrack(strArray[0]);
      }
      if (this.isOutdoors)
      {
        ((FarmerSprite) Game1.player.Sprite).currentStep = "sandyStep";
        this.tryToAddCritters(false);
      }
      PropertyValue propertyValue6 = (PropertyValue) null;
      this.map.Properties.TryGetValue("Doors", out propertyValue6);
      if (propertyValue6 != null)
      {
        string[] strArray = propertyValue6.ToString().Split(' ');
        int index1 = 0;
        while (index1 < strArray.Length)
        {
          int int32_1 = Convert.ToInt32(strArray[index1]);
          int int32_2 = Convert.ToInt32(strArray[index1 + 1]);
          Location index2 = new Location(int32_1, int32_2);
          if (this.map.GetLayer("Buildings").Tiles[index2] == null)
          {
            Tile tile = (Tile) new StaticTile(this.map.GetLayer("Buildings"), this.map.GetTileSheet(strArray[index1 + 2]), BlendMode.Alpha, Convert.ToInt32(strArray[index1 + 3]));
            string str = (string) null;
            if (this.doorSprites != null)
            {
              TemporaryAnimatedSprite temporaryAnimatedSprite = (TemporaryAnimatedSprite) null;
              if (this.doorSprites.TryGetValue(new Point(int32_1, int32_2), out temporaryAnimatedSprite))
                str = temporaryAnimatedSprite.endSound;
            }
            if (string.IsNullOrEmpty(str))
              tile.Properties.Add("Action", new PropertyValue("Door"));
            else
              tile.Properties.Add("Action", new PropertyValue("Door " + str));
            this.map.GetLayer("Buildings").Tiles[index2] = tile;
            --index2.Y;
            this.map.GetLayer("Front").Tiles[index2] = (Tile) new StaticTile(this.map.GetLayer("Front"), this.map.GetTileSheet(strArray[index1 + 2]), BlendMode.Alpha, Convert.ToInt32(strArray[index1 + 3]) - this.map.GetTileSheet(strArray[index1 + 2]).SheetWidth);
            --index2.Y;
            this.map.GetLayer("Front").Tiles[index2] = (Tile) new StaticTile(this.map.GetLayer("Front"), this.map.GetTileSheet(strArray[index1 + 2]), BlendMode.Alpha, Convert.ToInt32(strArray[index1 + 3]) - this.map.GetTileSheet(strArray[index1 + 2]).SheetWidth * 2);
          }
          index1 += 4;
        }
      }
      if (Game1.timeOfDay < 1900 && (!Game1.isRaining || this.name.Equals("SandyHouse")))
      {
        PropertyValue propertyValue4;
        this.map.Properties.TryGetValue("DayTiles", out propertyValue4);
        if (propertyValue4 != null)
        {
          string[] strArray = propertyValue4.ToString().Trim().Split(' ');
          int index = 0;
          while (index < strArray.Length)
          {
            if (this.map.GetLayer(strArray[index]).Tiles[Convert.ToInt32(strArray[index + 1]), Convert.ToInt32(strArray[index + 2])] != null)
              this.map.GetLayer(strArray[index]).Tiles[Convert.ToInt32(strArray[index + 1]), Convert.ToInt32(strArray[index + 2])].TileIndex = Convert.ToInt32(strArray[index + 3]);
            index += 4;
          }
        }
      }
      else if (Game1.timeOfDay >= 1900 || Game1.isRaining && !this.name.Equals("SandyHouse"))
      {
        if (!(this is MineShaft))
          this.lightGlows.Clear();
        PropertyValue propertyValue4;
        this.map.Properties.TryGetValue("NightTiles", out propertyValue4);
        if (propertyValue4 != null)
        {
          string[] strArray = propertyValue4.ToString().Split(' ');
          int index = 0;
          while (index < strArray.Length)
          {
            if (this.map.GetLayer(strArray[index]).Tiles[Convert.ToInt32(strArray[index + 1]), Convert.ToInt32(strArray[index + 2])] != null)
              this.map.GetLayer(strArray[index]).Tiles[Convert.ToInt32(strArray[index + 1]), Convert.ToInt32(strArray[index + 2])].TileIndex = Convert.ToInt32(strArray[index + 3]);
            index += 4;
          }
        }
      }
      if (this.name.Equals("Coop"))
      {
        foreach (CoopDweller coopDweller in Game1.player.coopDwellers)
          coopDweller.setRandomPosition();
        string[] strArray = this.getMapProperty("Feed").Split(' ');
        this.map.GetLayer("Buildings").Tiles[Convert.ToInt32(strArray[0]), Convert.ToInt32(strArray[1])].TileIndex = Game1.player.Feed > 0 ? 31 : 35;
      }
      else if (this.name.Equals("Barn"))
      {
        foreach (BarnDweller barnDweller in Game1.player.barnDwellers)
          barnDweller.setRandomPosition();
        string[] strArray = this.getMapProperty("Feed").Split(' ');
        this.map.GetLayer("Buildings").Tiles[Convert.ToInt32(strArray[0]), Convert.ToInt32(strArray[1])].TileIndex = Game1.player.Feed > 0 ? 31 : 35;
      }
      if (this.name.Equals("Mountain") && (Game1.timeOfDay < 2000 || !Game1.currentSeason.Equals("summer") || Game1.random.NextDouble() >= 0.3) && (Game1.isRaining && !Game1.currentSeason.Equals("winter")))
        Game1.random.NextDouble();
      if (this.name.Equals("Club"))
        Game1.changeMusicTrack("clubloop");
      else if (Game1.currentSong != null && Game1.currentSong.Name.Equals("clubloop") && (Game1.nextMusicTrack == null || Game1.nextMusicTrack.Count<char>() == 0))
        Game1.changeMusicTrack("none");
      if (Game1.activeClickableMenu != null)
        return;
      this.checkForEvents();
    }

    public virtual bool isTileOccupiedForPlacement(Vector2 tileLocation, Object toPlace = null)
    {
      Object @object;
      this.objects.TryGetValue(tileLocation, out @object);
      Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle((int) tileLocation.X * Game1.tileSize, (int) tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
      Microsoft.Xna.Framework.Rectangle boundingBox;
      for (int index = 0; index < this.characters.Count; ++index)
      {
        if (this.characters[index] != null)
        {
          boundingBox = this.characters[index].GetBoundingBox();
          if (boundingBox.Intersects(rectangle))
            return true;
        }
      }
      if (this.isTileOccupiedByFarmer(tileLocation) != null && (toPlace == null || !toPlace.isPassable()))
        return true;
      if (this.largeTerrainFeatures != null)
      {
        foreach (LargeTerrainFeature largeTerrainFeature in this.largeTerrainFeatures)
        {
          boundingBox = largeTerrainFeature.getBoundingBox();
          if (boundingBox.Intersects(rectangle))
            return true;
        }
      }
      if (this.terrainFeatures.ContainsKey(tileLocation) && rectangle.Intersects(this.terrainFeatures[tileLocation].getBoundingBox(tileLocation)) && (!this.terrainFeatures[tileLocation].isPassable((Character) null) || this.terrainFeatures[tileLocation].GetType() == typeof (HoeDirt) && ((HoeDirt) this.terrainFeatures[tileLocation]).crop != null) || !this.isTilePassable(new Location((int) tileLocation.X, (int) tileLocation.Y), Game1.viewport) && (toPlace == null || !(toPlace is Wallpaper)))
        return true;
      return @object != null;
    }

    public Farmer isTileOccupiedByFarmer(Vector2 tileLocation)
    {
      foreach (Farmer farmer in this.getFarmers())
      {
        if (farmer.getTileLocation().Equals(tileLocation))
          return farmer;
      }
      return (Farmer) null;
    }

    public virtual bool isTileOccupied(Vector2 tileLocation, string characterToIgnore = "")
    {
      Object @object;
      this.objects.TryGetValue(tileLocation, out @object);
      Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle((int) tileLocation.X * Game1.tileSize + 1, (int) tileLocation.Y * Game1.tileSize + 1, Game1.tileSize - 2, Game1.tileSize - 2);
      Microsoft.Xna.Framework.Rectangle boundingBox;
      for (int index = 0; index < this.characters.Count; ++index)
      {
        if (this.characters[index] != null && !this.characters[index].name.Equals(characterToIgnore))
        {
          boundingBox = this.characters[index].GetBoundingBox();
          if (boundingBox.Intersects(rectangle))
            return true;
        }
      }
      if (this.terrainFeatures.ContainsKey(tileLocation) && rectangle.Intersects(this.terrainFeatures[tileLocation].getBoundingBox(tileLocation)))
        return true;
      if (this.largeTerrainFeatures != null)
      {
        foreach (LargeTerrainFeature largeTerrainFeature in this.largeTerrainFeatures)
        {
          boundingBox = largeTerrainFeature.getBoundingBox();
          if (boundingBox.Intersects(rectangle))
            return true;
        }
      }
      return @object != null;
    }

    public virtual bool isTileOccupiedIgnoreFloors(Vector2 tileLocation, string characterToIgnore = "")
    {
      Object @object;
      this.objects.TryGetValue(tileLocation, out @object);
      Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle((int) tileLocation.X * Game1.tileSize + 1, (int) tileLocation.Y * Game1.tileSize + 1, Game1.tileSize - 2, Game1.tileSize - 2);
      Microsoft.Xna.Framework.Rectangle boundingBox;
      for (int index = 0; index < this.characters.Count; ++index)
      {
        if (this.characters[index] != null && !this.characters[index].name.Equals(characterToIgnore))
        {
          boundingBox = this.characters[index].GetBoundingBox();
          if (boundingBox.Intersects(rectangle))
            return true;
        }
      }
      if (this.terrainFeatures.ContainsKey(tileLocation) && rectangle.Intersects(this.terrainFeatures[tileLocation].getBoundingBox(tileLocation)) && !this.terrainFeatures[tileLocation].isPassable((Character) null))
        return true;
      if (this.largeTerrainFeatures != null)
      {
        foreach (LargeTerrainFeature largeTerrainFeature in this.largeTerrainFeatures)
        {
          boundingBox = largeTerrainFeature.getBoundingBox();
          if (boundingBox.Intersects(rectangle))
            return true;
        }
      }
      return @object != null;
    }

    public bool isTileHoeDirt(Vector2 tileLocation)
    {
      return this.terrainFeatures.ContainsKey(tileLocation) && this.terrainFeatures[tileLocation] is HoeDirt;
    }

    public void playTerrainSound(Vector2 tileLocation, Character who = null, bool showTerrainDisturbAnimation = true)
    {
      string cueName = "thudStep";
      if (Game1.currentLocation.IsOutdoors || Game1.currentLocation.Name.ToLower().Contains("mine"))
      {
        switch (Game1.currentLocation.doesTileHaveProperty((int) tileLocation.X, (int) tileLocation.Y, "Type", "Back"))
        {
          case "Dirt":
            cueName = "sandyStep";
            break;
          case "Stone":
            cueName = "stoneStep";
            break;
          case "Grass":
            cueName = Game1.currentSeason.Equals("winter") ? "snowyStep" : "grassyStep";
            break;
          case "Wood":
            cueName = "woodyStep";
            break;
          case null:
            if (Game1.currentLocation.doesTileHaveProperty((int) tileLocation.X, (int) tileLocation.Y, "Water", "Back") != null)
            {
              cueName = "waterSlosh";
              break;
            }
            break;
        }
      }
      else
        cueName = "thudStep";
      if (Game1.currentLocation.terrainFeatures.ContainsKey(tileLocation) && Game1.currentLocation.terrainFeatures[tileLocation].GetType() == typeof (Flooring))
        cueName = ((Flooring) Game1.currentLocation.terrainFeatures[tileLocation]).getFootstepSound();
      if (who != null & showTerrainDisturbAnimation && cueName.Equals("sandyStep"))
        Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(0, Game1.tileSize, Game1.tileSize, Game1.tileSize), 50f, 4, 1, new Vector2(who.position.X + (float) Game1.random.Next(-8, 8), who.position.Y + (float) Game1.random.Next(-16, 0)), false, Game1.random.NextDouble() < 0.5, 0.0001f, 0.0f, Color.White, 1f, 0.01f, 0.0f, (float) ((double) Game1.random.Next(-5, 6) * 3.14159274101257 / 128.0), false));
      else if (who != null & showTerrainDisturbAnimation && Game1.currentSeason.Equals("winter") && cueName.Equals("grassyStep"))
        Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(247, 407, 6, 6), 2000f, 1, 10000, new Vector2(who.position.X, who.position.Y), false, false, 0.0001f, 1f / 1000f, Color.White, 1f, 0.01f, 0.0f, 0.0f, false));
      if (cueName.Length <= 0)
        return;
      Game1.playSound(cueName);
    }

    public virtual bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
    {
      if (this.currentEvent != null && this.currentEvent.isFestival)
        return this.currentEvent.checkAction(tileLocation, viewport, who);
      foreach (NPC character in this.characters)
      {
        if (character != null && !character.IsMonster && (!who.isRidingHorse() || !(character is Horse)) && character.GetBoundingBox().Intersects(new Microsoft.Xna.Framework.Rectangle(tileLocation.X * Game1.tileSize, tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize)))
          return character.checkAction(who, this);
      }
      if (who.IsMainPlayer && who.currentUpgrade != null && (this.name.Equals("Farm") && tileLocation.Equals((object) new Location((int) ((double) who.currentUpgrade.positionOfCarpenter.X + (double) (Game1.tileSize / 2)) / Game1.tileSize, (int) ((double) who.currentUpgrade.positionOfCarpenter.Y + (double) (Game1.tileSize / 2)) / Game1.tileSize))))
      {
        if (who.currentUpgrade.daysLeftTillUpgradeDone == 1)
          Game1.drawDialogue(Game1.getCharacterFromName("Robin", false), Game1.content.LoadString("Data\\ExtraDialogue:Farm_RobinWorking_ReadyTomorrow"));
        else
          Game1.drawDialogue(Game1.getCharacterFromName("Robin", false), Game1.content.LoadString("Data\\ExtraDialogue:Farm_RobinWorking" + (object) (Game1.random.Next(2) + 1)));
      }
      Vector2 key = new Vector2((float) tileLocation.X, (float) tileLocation.Y);
      if (this.objects.ContainsKey(key) && this.objects[key].Type != null)
      {
        if (who.isRidingHorse() && !(this.objects[key] is Fence))
          return false;
        if (key.Equals(who.getTileLocation()) && !this.objects[key].isPassable())
        {
          Tool t1 = (Tool) new Pickaxe();
          t1.DoFunction(Game1.currentLocation, -1, -1, 0, who);
          if (this.objects[key].performToolAction(t1))
          {
            this.objects[key].performRemoveAction(this.objects[key].tileLocation, Game1.currentLocation);
            if ((this.objects[key].type.Equals("Crafting") || this.objects[key].Type.Equals("interactive")) && this.objects[key].fragility != 2)
            {
              List<Debris> debris1 = Game1.currentLocation.debris;
              int objectIndex = this.objects[key].bigCraftable ? -this.objects[key].ParentSheetIndex : this.objects[key].ParentSheetIndex;
              Vector2 toolLocation = who.GetToolLocation(false);
              Microsoft.Xna.Framework.Rectangle boundingBox = who.GetBoundingBox();
              double x = (double) boundingBox.Center.X;
              boundingBox = who.GetBoundingBox();
              double y = (double) boundingBox.Center.Y;
              Vector2 playerPosition = new Vector2((float) x, (float) y);
              Debris debris2 = new Debris(objectIndex, toolLocation, playerPosition);
              debris1.Add(debris2);
            }
            Game1.currentLocation.Objects.Remove(key);
            return true;
          }
          Tool t2 = (Tool) new Axe();
          t2.DoFunction(Game1.currentLocation, -1, -1, 0, who);
          if (this.objects.ContainsKey(key) && this.objects[key].performToolAction(t2))
          {
            this.objects[key].performRemoveAction(this.objects[key].tileLocation, Game1.currentLocation);
            if ((this.objects[key].type.Equals("Crafting") || this.objects[key].Type.Equals("interactive")) && this.objects[key].fragility != 2)
            {
              List<Debris> debris1 = Game1.currentLocation.debris;
              int objectIndex = this.objects[key].bigCraftable ? -this.objects[key].ParentSheetIndex : this.objects[key].ParentSheetIndex;
              Vector2 toolLocation = who.GetToolLocation(false);
              Microsoft.Xna.Framework.Rectangle boundingBox = who.GetBoundingBox();
              double x = (double) boundingBox.Center.X;
              boundingBox = who.GetBoundingBox();
              double y = (double) boundingBox.Center.Y;
              Vector2 playerPosition = new Vector2((float) x, (float) y);
              Debris debris2 = new Debris(objectIndex, toolLocation, playerPosition);
              debris1.Add(debris2);
            }
            Game1.currentLocation.Objects.Remove(key);
            return true;
          }
          if (!this.objects.ContainsKey(key))
            return true;
        }
        if (this.objects.ContainsKey(key) && (this.objects[key].Type.Equals("Crafting") || this.objects[key].Type.Equals("interactive")))
        {
          if (who.ActiveObject == null)
            return this.objects[key].checkForAction(who, false);
          if (!this.objects[key].performObjectDropInAction(who.ActiveObject, false, who))
            return this.objects[key].checkForAction(who, false);
          who.reduceActiveItemByOne();
          return true;
        }
        if (this.objects.ContainsKey(key) && this.objects[key].isSpawnedObject)
        {
          int quality = this.objects[key].quality;
          Random random = new Random((int) Game1.uniqueIDForThisGame / 2 + (int) Game1.stats.DaysPlayed + (int) key.X + (int) key.Y * 777);
          if (who.professions.Contains(16) && this.objects[key].isForage(this))
            this.objects[key].quality = 4;
          else if (this.objects[key].isForage(this))
          {
            if (random.NextDouble() < (double) who.ForagingLevel / 30.0)
              this.objects[key].quality = 2;
            else if (random.NextDouble() < (double) who.ForagingLevel / 15.0)
              this.objects[key].quality = 1;
          }
          if (who.couldInventoryAcceptThisItem((Item) this.objects[key]))
          {
            if (who.IsMainPlayer)
            {
              Game1.playSound("pickUpItem");
              DelayedAction.playSoundAfterDelay("coin", 300);
            }
            who.animateOnce(279 + who.FacingDirection);
            if (!this.isFarmBuildingInterior())
            {
              if (this.objects[key].isForage(this))
                who.gainExperience(2, 7);
            }
            else
              who.gainExperience(0, 5);
            who.addItemToInventoryBool(this.objects[key].getOne(), false);
            ++Game1.stats.ItemsForaged;
            if (who.professions.Contains(13) && random.NextDouble() < 0.2 && (!this.objects[key].questItem && who.couldInventoryAcceptThisItem((Item) this.objects[key])) && !this.isFarmBuildingInterior())
            {
              who.addItemToInventoryBool(this.objects[key].getOne(), false);
              who.gainExperience(2, 7);
            }
            this.objects.Remove(key);
            return true;
          }
          this.objects[key].quality = quality;
        }
      }
      if (who.isRidingHorse())
      {
        who.getMount().checkAction(who, this);
        return true;
      }
      if (this.terrainFeatures.ContainsKey(key) && this.terrainFeatures[key].GetType() == typeof (HoeDirt) && who.ActiveObject != null && ((who.ActiveObject.Category == -74 || who.ActiveObject.Category == -19) && ((HoeDirt) this.terrainFeatures[key]).canPlantThisSeedHere(who.ActiveObject.ParentSheetIndex, tileLocation.X, tileLocation.Y, who.ActiveObject.Category == -19)))
      {
        if (((HoeDirt) this.terrainFeatures[key]).plant(who.ActiveObject.ParentSheetIndex, tileLocation.X, tileLocation.Y, who, who.ActiveObject.Category == -19) && who.IsMainPlayer)
          who.reduceActiveItemByOne();
        Game1.haltAfterCheck = false;
        return true;
      }
      Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle(tileLocation.X * Game1.tileSize, tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
      foreach (KeyValuePair<Vector2, TerrainFeature> terrainFeature in (Dictionary<Vector2, TerrainFeature>) this.terrainFeatures)
      {
        if (terrainFeature.Value.getBoundingBox(terrainFeature.Key).Intersects(rectangle))
        {
          Game1.haltAfterCheck = false;
          return terrainFeature.Value.performUseAction(terrainFeature.Key);
        }
      }
      if (this.largeTerrainFeatures != null)
      {
        foreach (LargeTerrainFeature largeTerrainFeature in this.largeTerrainFeatures)
        {
          if (largeTerrainFeature.getBoundingBox().Intersects(rectangle))
          {
            Game1.haltAfterCheck = false;
            return largeTerrainFeature.performUseAction(largeTerrainFeature.tilePosition);
          }
        }
      }
      PropertyValue propertyValue = (PropertyValue) null;
      Tile tile = this.map.GetLayer("Buildings").PickTile(new Location(tileLocation.X * Game1.tileSize, tileLocation.Y * Game1.tileSize), viewport.Size);
      if (tile != null)
        tile.Properties.TryGetValue("Action", out propertyValue);
      if (propertyValue != null && (this.currentEvent != null || this.isCharacterAtTile(key + new Vector2(0.0f, 1f)) == null))
        return this.performAction((string) propertyValue, who, tileLocation);
      return false;
    }

    public virtual bool leftClick(int x, int y, Farmer who)
    {
      Vector2 key = new Vector2((float) (x / Game1.tileSize), (float) (y / Game1.tileSize));
      if (!this.objects.ContainsKey(key) || !this.objects[key].clicked(who))
        return false;
      this.objects.Remove(key);
      return true;
    }

    public virtual int getExtraMillisecondsPerInGameMinuteForThisLocation()
    {
      return 0;
    }

    public bool isTileLocationTotallyClearAndPlaceable(int x, int y)
    {
      return this.isTileLocationTotallyClearAndPlaceable(new Vector2((float) x, (float) y));
    }

    public virtual bool isTileLocationTotallyClearAndPlaceableIgnoreFloors(Vector2 v)
    {
      if (this.isTileOnMap(v) && !this.isTileOccupiedIgnoreFloors(v, "") && this.isTilePassable(new Location((int) v.X, (int) v.Y), Game1.viewport))
        return this.isTilePlaceable(v, (Item) null);
      return false;
    }

    public virtual bool isTileLocationTotallyClearAndPlaceable(Vector2 v)
    {
      if (this.isTileOnMap(v) && !this.isTileOccupied(v, "") && this.isTilePassable(new Location((int) v.X, (int) v.Y), Game1.viewport))
        return this.isTilePlaceable(v, (Item) null);
      return false;
    }

    public virtual bool isTilePlaceable(Vector2 v, Item item = null)
    {
      if (this.doesTileHaveProperty((int) v.X, (int) v.Y, "NoFurniture", "Back") != null && (item == null || !(item is Object) || (!(item as Object).isPassable() || !Game1.currentLocation.IsOutdoors) || this.doesTileHavePropertyNoNull((int) v.X, (int) v.Y, "NoFurniture", "Back").Equals("total")))
        return false;
      if (this.doesTileHaveProperty((int) v.X, (int) v.Y, "Water", "Back") == null)
        return true;
      if (item != null)
        return item.canBePlacedInWater();
      return false;
    }

    public virtual bool shouldShadowBeDrawnAboveBuildingsLayer(Vector2 p)
    {
      Object @object;
      TerrainFeature terrainFeature;
      return this.objects.TryGetValue(p, out @object) && @object.isPassable() || this.terrainFeatures.TryGetValue(p, out terrainFeature) && terrainFeature.isPassable((Character) null);
    }

    public void openDoor(Location tileLocation, bool playSound)
    {
      try
      {
        int tileIndexAt = this.getTileIndexAt(tileLocation.X, tileLocation.Y, "Buildings");
        if (this.doorSprites.ContainsKey(new Point(tileLocation.X, tileLocation.Y)) && this.doorSprites[new Point(tileLocation.X, tileLocation.Y)].paused)
        {
          this.doorSprites[new Point(tileLocation.X, tileLocation.Y)].paused = false;
          if (tileIndexAt == 824)
            this.openDoor(new Location(tileLocation.X + 1, tileLocation.Y), false);
          else if (tileIndexAt == 825)
            this.openDoor(new Location(tileLocation.X - 1, tileLocation.Y), false);
        }
        DelayedAction.removeTileAfterDelay(tileLocation.X, tileLocation.Y, 400, this, "Buildings");
        this.removeTile(new Location(tileLocation.X, tileLocation.Y - 1), "Front");
        this.removeTile(new Location(tileLocation.X, tileLocation.Y - 2), "Front");
        if (!playSound)
          return;
        if (tileIndexAt == 120)
          Game1.playSound("doorOpen");
        else
          Game1.playSound("doorCreak");
      }
      catch (Exception ex)
      {
      }
    }

    public void doStarpoint(string which)
    {
      if (!(which == "3"))
      {
        if (!(which == "4") || Game1.player.ActiveObject == null || (Game1.player.ActiveObject == null || Game1.player.ActiveObject.bigCraftable) || Game1.player.ActiveObject.parentSheetIndex != 203)
          return;
        Game1.player.ActiveObject = new Object(Vector2.Zero, 162, false);
        Game1.playSound("croak");
        Game1.flashAlpha = 1f;
      }
      else
      {
        if (Game1.player.ActiveObject == null || Game1.player.ActiveObject == null || (Game1.player.ActiveObject.bigCraftable || Game1.player.ActiveObject.parentSheetIndex != 307))
          return;
        Game1.player.ActiveObject = new Object(Vector2.Zero, 161, false);
        Game1.playSound("discoverMineral");
        Game1.flashAlpha = 1f;
      }
    }

    public bool performAction(string action, Farmer who, Location tileLocation)
    {
      if (action != null && who.IsMainPlayer)
      {
        string[] strArray = action.Split(' ');
        string s = strArray[0];
        // ISSUE: reference to a compiler-generated method
        uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(s);
        if (stringHash <= 2413466880U)
        {
          if (stringHash <= 1135412759U)
          {
            if (stringHash <= 297990791U)
            {
              if (stringHash <= 139067618U)
              {
                if (stringHash <= 48641340U)
                {
                  if ((int) stringHash != 4774130)
                  {
                    if ((int) stringHash == 48641340 && s == "SpiritAltar" && (who.ActiveObject != null && Game1.dailyLuck != -0.12) && Game1.dailyLuck != 0.12)
                    {
                      if (who.ActiveObject.Price >= 60)
                      {
                        this.temporarySprites.Add(new TemporaryAnimatedSprite(352, 70f, 2, 2, new Vector2((float) (tileLocation.X * Game1.tileSize), (float) (tileLocation.Y * Game1.tileSize)), false, false));
                        Game1.dailyLuck = 0.12;
                        Game1.playSound("money");
                      }
                      else
                      {
                        this.temporarySprites.Add(new TemporaryAnimatedSprite(362, 50f, 6, 1, new Vector2((float) (tileLocation.X * Game1.tileSize), (float) (tileLocation.Y * Game1.tileSize)), false, false));
                        Game1.dailyLuck = -0.12;
                        Game1.playSound("thunder");
                      }
                      who.ActiveObject = (Object) null;
                      who.showNotCarrying();
                      goto label_369;
                    }
                    else
                      goto label_369;
                  }
                  else if (s == "EvilShrineRight")
                  {
                    if (Game1.spawnMonstersAtNight)
                    {
                      this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:WitchHut_EvilShrineRightDeActivate"), this.createYesNoResponses(), "evilShrineRightDeActivate");
                      goto label_369;
                    }
                    else
                    {
                      this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:WitchHut_EvilShrineRightActivate"), this.createYesNoResponses(), "evilShrineRightActivate");
                      goto label_369;
                    }
                  }
                  else
                    goto label_369;
                }
                else if ((int) stringHash != 49977355)
                {
                  if ((int) stringHash != 135767117)
                  {
                    if ((int) stringHash == 139067618 && s == "IceCreamStand")
                    {
                      if ((object) this.isCharacterAtTile(new Vector2((float) tileLocation.X, (float) (tileLocation.Y - 2))) != null || (object) this.isCharacterAtTile(new Vector2((float) tileLocation.X, (float) (tileLocation.Y - 1))) != null || (object) this.isCharacterAtTile(new Vector2((float) tileLocation.X, (float) (tileLocation.Y - 3))) != null)
                      {
                        Game1.activeClickableMenu = (IClickableMenu) new ShopMenu(new Dictionary<Item, int[]>()
                        {
                          {
                            (Item) new Object(233, 1, false, -1, 0),
                            new int[2]{ 250, int.MaxValue }
                          }
                        }, 0, (string) null);
                        goto label_369;
                      }
                      else if (Game1.currentSeason.Equals("summer"))
                      {
                        Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:IceCreamStand_ComeBackLater"));
                        goto label_369;
                      }
                      else
                      {
                        Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:IceCreamStand_NotSummer"));
                        goto label_369;
                      }
                    }
                    else
                      goto label_369;
                  }
                  else if (s == "Jukebox")
                  {
                    Game1.activeClickableMenu = (IClickableMenu) new ChooseFromListMenu(Game1.player.songsHeard, new ChooseFromListMenu.actionOnChoosingListOption(ChooseFromListMenu.playSongAction), true);
                    goto label_369;
                  }
                  else
                    goto label_369;
                }
                else if (s == "Crib")
                {
                  using (List<NPC>.Enumerator enumerator = this.characters.GetEnumerator())
                  {
                    while (enumerator.MoveNext())
                    {
                      NPC current = enumerator.Current;
                      if (current is Child && (current as Child).age == 1)
                        (current as Child).toss(who);
                      else if (current is Child && (current as Child).age == 0)
                        Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\Locations:FarmHouse_Crib_NewbornSleeping", (object) current.displayName)));
                    }
                    goto label_369;
                  }
                }
                else
                  goto label_369;
              }
              else if (stringHash <= 234320812U)
              {
                if ((int) stringHash != 183343509)
                {
                  if ((int) stringHash == 234320812 && s == "SandDragon")
                  {
                    if (who.ActiveObject != null && who.ActiveObject.parentSheetIndex == 768 && (!who.hasOrWillReceiveMail("TH_SandDragon") && who.hasOrWillReceiveMail("TH_MayorFridge")))
                    {
                      who.reduceActiveItemByOne();
                      Game1.player.CanMove = false;
                      Game1.playSound("eat");
                      Game1.player.mailReceived.Add("TH_SandDragon");
                      Game1.multipleDialogues(new string[2]
                      {
                        Game1.content.LoadString("Strings\\Locations:Desert_SandDragon_ConsumeEssence"),
                        Game1.content.LoadString("Strings\\Locations:Desert_SandDragon_MrQiNote")
                      });
                      Game1.player.removeQuest(4);
                      Game1.player.addQuest(5);
                      goto label_369;
                    }
                    else if (who.hasOrWillReceiveMail("TH_SandDragon"))
                    {
                      Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Desert_SandDragon_MrQiNote"));
                      goto label_369;
                    }
                    else
                    {
                      Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Desert_SandDragon_Initial"));
                      goto label_369;
                    }
                  }
                  else
                    goto label_369;
                }
                else if (s == "ColaMachine")
                {
                  this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:Saloon_ColaMachine_Question"), this.createYesNoResponses(), "buyJojaCola");
                  goto label_369;
                }
                else
                  goto label_369;
              }
              else if ((int) stringHash != 267393898)
              {
                if ((int) stringHash != 295776207)
                {
                  if ((int) stringHash == 297990791 && s == "NPCMessage")
                  {
                    NPC characterFromName = Game1.getCharacterFromName(strArray[1], false);
                    if ((object) characterFromName != null)
                    {
                      if (characterFromName.withinPlayerThreshold(14))
                      {
                        try
                        {
                          characterFromName.setNewDialogue(Game1.content.LoadString(action.Substring(action.IndexOf('"') + 1).Split('/')[0]), true, false);
                          Game1.drawDialogue(characterFromName);
                          return false;
                        }
                        catch (Exception ex)
                        {
                          return false;
                        }
                      }
                    }
                    try
                    {
                      Game1.drawDialogueNoTyping(Game1.content.LoadString(action.Substring(action.IndexOf('"')).Split('/')[1].Replace("\"", "")));
                      return false;
                    }
                    catch (Exception ex)
                    {
                      return false;
                    }
                  }
                  else
                    goto label_369;
                }
                else if (s == "Warp")
                {
                  who.faceGeneralDirection(new Vector2((float) tileLocation.X, (float) tileLocation.Y) * (float) Game1.tileSize, 0);
                  Rumble.rumble(0.15f, 200f);
                  Game1.warpFarmer(strArray[3], Convert.ToInt32(strArray[1]), Convert.ToInt32(strArray[2]), false);
                  if (strArray.Length < 5)
                  {
                    Game1.playSound("doorClose");
                    goto label_369;
                  }
                  else
                    goto label_369;
                }
                else
                  goto label_369;
              }
              else if (s == "Notes")
              {
                this.readNote(Convert.ToInt32(strArray[1]));
                goto label_369;
              }
              else
                goto label_369;
            }
            else if (stringHash <= 837292325U)
            {
              if (stringHash <= 414528787U)
              {
                if ((int) stringHash != 371676316)
                {
                  if ((int) stringHash == 414528787 && s == "QiCoins")
                  {
                    if (who.clubCoins > 0)
                    {
                      Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Club_QiCoins", (object) who.clubCoins));
                      goto label_369;
                    }
                    else
                    {
                      this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:Club_QiCoins_BuyStarter"), this.createYesNoResponses(), "BuyClubCoins");
                      goto label_369;
                    }
                  }
                  else
                    goto label_369;
                }
                else if (s == "MineElevator")
                {
                  if (Game1.mine == null || Game1.mine.lowestLevelReached < 5)
                  {
                    Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\Locations:Mines_MineElevator_NotWorking")));
                    goto label_369;
                  }
                  else
                  {
                    Game1.activeClickableMenu = (IClickableMenu) new MineElevatorMenu();
                    goto label_369;
                  }
                }
                else
                  goto label_369;
              }
              else if ((int) stringHash != 570160120)
              {
                if ((int) stringHash != 634795166)
                {
                  if ((int) stringHash == 837292325 && s == "BuyBackpack")
                  {
                    Response response1 = new Response("Purchase", Game1.content.LoadString("Strings\\Locations:SeedShop_BuyBackpack_Response2000"));
                    Response response2 = new Response("Purchase", Game1.content.LoadString("Strings\\Locations:SeedShop_BuyBackpack_Response10000"));
                    Response response3 = new Response("Not", Game1.content.LoadString("Strings\\Locations:SeedShop_BuyBackpack_ResponseNo"));
                    if (Game1.player.maxItems == 12)
                    {
                      this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:SeedShop_BuyBackpack_Question24"), new Response[2]
                      {
                        response1,
                        response3
                      }, "Backpack");
                      goto label_369;
                    }
                    else if (Game1.player.maxItems < 36)
                    {
                      this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:SeedShop_BuyBackpack_Question36"), new Response[2]
                      {
                        response2,
                        response3
                      }, "Backpack");
                      goto label_369;
                    }
                    else
                      goto label_369;
                  }
                  else
                    goto label_369;
                }
                else if (s == "ClubSlots")
                {
                  Game1.currentMinigame = (IMinigame) new Slots(-1, false);
                  goto label_369;
                }
                else
                  goto label_369;
              }
              else if (s == "MagicInk" && !who.mailReceived.Contains("hasPickedUpMagicInk"))
              {
                who.mailReceived.Add("hasPickedUpMagicInk");
                who.hasMagicInk = true;
                this.setMapTileIndex(4, 11, 113, "Buildings", 0);
                who.addItemByMenuIfNecessaryElseHoldUp((Item) new SpecialItem(1, 7, ""), (ItemGrabMenu.behaviorOnItemSelect) null);
                goto label_369;
              }
              else
                goto label_369;
            }
            else if (stringHash <= 895720287U)
            {
              if ((int) stringHash != 870733615)
              {
                if ((int) stringHash == 895720287 && s == "HospitalShop" && ((object) this.isCharacterAtTile(who.getTileLocation() + new Vector2(0.0f, -2f)) != null || (object) this.isCharacterAtTile(who.getTileLocation() + new Vector2(-1f, -2f)) != null))
                {
                  Game1.activeClickableMenu = (IClickableMenu) new ShopMenu(Utility.getHospitalStock(), 0, (string) null);
                  goto label_369;
                }
                else
                  goto label_369;
              }
              else if (s == "kitchen")
              {
                Vector2 centeringOnScreen = Utility.getTopLeftPositionForCenteringOnScreen(800 + IClickableMenu.borderWidth * 2, 600 + IClickableMenu.borderWidth * 2, 0, 0);
                Game1.activeClickableMenu = (IClickableMenu) new CraftingPage((int) centeringOnScreen.X, (int) centeringOnScreen.Y, 800 + IClickableMenu.borderWidth * 2, 600 + IClickableMenu.borderWidth * 2, true);
                goto label_369;
              }
              else
                goto label_369;
            }
            else if ((int) stringHash != 908820861)
            {
              if ((int) stringHash != 1094091226)
              {
                if ((int) stringHash == 1135412759 && s == "Carpenter" && who.getTileY() > tileLocation.Y)
                {
                  this.carpenters(tileLocation);
                  goto label_369;
                }
                else
                  goto label_369;
              }
              else if (s == "Arcade_Prairie")
              {
                Game1.currentMinigame = (IMinigame) new AbigailGame(false);
                goto label_369;
              }
              else
                goto label_369;
            }
            else if (s == "Letter")
            {
              Game1.drawLetterMessage(Game1.content.LoadString("Strings\\StringsFromMaps:" + strArray[1].Replace("\"", "")));
              goto label_369;
            }
            else
              goto label_369;
          }
          else if (stringHash <= 1719994463U)
          {
            if (stringHash <= 1555723527U)
            {
              if (stringHash <= 1288111488U)
              {
                if ((int) stringHash != 1140116675)
                {
                  if ((int) stringHash == 1288111488 && s == "ShippingBin")
                  {
                    Game1.shipHeldItem();
                    goto label_369;
                  }
                  else
                    goto label_369;
                }
                else if (s == "JojaShop")
                {
                  Game1.activeClickableMenu = (IClickableMenu) new ShopMenu(Utility.getJojaStock(), 0, (string) null);
                  goto label_369;
                }
                else
                  goto label_369;
              }
              else if ((int) stringHash != 1367472567)
              {
                if ((int) stringHash != 1379459566)
                {
                  if ((int) stringHash != 1555723527 || !(s == "BlackJack"))
                    goto label_369;
                }
                else if (s == "WarpMensLocker")
                {
                  if (!who.isMale)
                  {
                    if (who.IsMainPlayer)
                      Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:MensLocker_WrongGender"));
                    return false;
                  }
                  who.faceGeneralDirection(new Vector2((float) tileLocation.X, (float) tileLocation.Y) * (float) Game1.tileSize, 0);
                  Game1.warpFarmer(strArray[3], Convert.ToInt32(strArray[1]), Convert.ToInt32(strArray[2]), false);
                  if (strArray.Length < 5)
                  {
                    Game1.playSound("doorClose");
                    goto label_369;
                  }
                  else
                    goto label_369;
                }
                else
                  goto label_369;
              }
              else if (s == "Blacksmith" && who.getTileY() > tileLocation.Y)
              {
                this.blacksmith(tileLocation);
                goto label_369;
              }
              else
                goto label_369;
            }
            else if (stringHash <= 1673854597U)
            {
              if ((int) stringHash != 1573286044)
              {
                if ((int) stringHash == 1673854597 && s == "WizardShrine")
                {
                  this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:WizardTower_WizardShrine").Replace('\n', '^'), this.createYesNoResponses(), "WizardShrine");
                  goto label_369;
                }
                else
                  goto label_369;
              }
              else if (!(s == "ClubCards"))
                goto label_369;
            }
            else if ((int) stringHash != 1687261568)
            {
              if ((int) stringHash != 1716769139)
              {
                if ((int) stringHash == 1719994463 && s == "WizardBook" && (who.mailReceived.Contains("hasPickedUpMagicInk") || who.hasMagicInk))
                {
                  Game1.activeClickableMenu = (IClickableMenu) new CarpenterMenu(true);
                  goto label_369;
                }
                else
                  goto label_369;
              }
              else if (s == "Dialogue")
              {
                Game1.drawDialogueNoTyping(this.actionParamsToString(strArray));
                goto label_369;
              }
              else
                goto label_369;
            }
            else if (s == "ClubComputer")
              goto label_346;
            else
              goto label_369;
            if (strArray.Length > 1 && strArray[1].Equals("1000"))
            {
              this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:Club_CalicoJack_HS"), new Response[2]
              {
                new Response("Play", Game1.content.LoadString("Strings\\Locations:Club_CalicoJack_Play")),
                new Response("Leave", Game1.content.LoadString("Strings\\Locations:Club_CalicoJack_Leave"))
              }, "CalicoJackHS");
              goto label_369;
            }
            else
            {
              this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:Club_CalicoJack"), new Response[3]
              {
                new Response("Play", Game1.content.LoadString("Strings\\Locations:Club_CalicoJack_Play")),
                new Response("Leave", Game1.content.LoadString("Strings\\Locations:Club_CalicoJack_Leave")),
                new Response("Rules", Game1.content.LoadString("Strings\\Locations:Club_CalicoJack_Rules"))
              }, "CalicoJack");
              goto label_369;
            }
          }
          else if (stringHash <= 1959071256U)
          {
            if (stringHash <= 1806134029U)
            {
              if ((int) stringHash != 1722787773)
              {
                if ((int) stringHash == 1806134029 && s == "BuyQiCoins")
                {
                  this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:Club_Buy100Coins"), this.createYesNoResponses(), "BuyQiCoins");
                  goto label_369;
                }
                else
                  goto label_369;
              }
              else if (s == "MessageOnce" && !who.eventsSeen.Contains(Convert.ToInt32(strArray[1])))
              {
                who.eventsSeen.Add(Convert.ToInt32(strArray[1]));
                Game1.drawObjectDialogue(Game1.parseText(this.actionParamsToString(strArray).Substring(this.actionParamsToString(strArray).IndexOf(' '))));
                goto label_369;
              }
              else
                goto label_369;
            }
            else if ((int) stringHash != 1852246243)
            {
              if ((int) stringHash != 1856350152)
              {
                if ((int) stringHash == 1959071256 && s == "WizardHatch")
                {
                  if (who.friendships.ContainsKey("Wizard") && who.friendships["Wizard"][0] >= 1000)
                  {
                    Game1.warpFarmer("WizardHouseBasement", 4, 4, true);
                    Game1.playSound("doorClose");
                    goto label_369;
                  }
                  else
                  {
                    NPC character = this.characters[0];
                    character.CurrentDialogue.Push(new Dialogue(Game1.content.LoadString("Data\\ExtraDialogue:Wizard_Hatch"), character));
                    Game1.drawDialogue(character);
                    goto label_369;
                  }
                }
                else
                  goto label_369;
              }
              else if (s == "Yoba")
              {
                Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:SeedShop_Yoba"));
                goto label_369;
              }
              else
                goto label_369;
            }
            else if (!(s == "NextMineLevel"))
              goto label_369;
          }
          else if (stringHash <= 2250926415U)
          {
            if ((int) stringHash != 2039622173)
            {
              if ((int) stringHash != 2050472952)
              {
                if ((int) stringHash == -2044040881 && s == "Tutorial")
                {
                  Game1.activeClickableMenu = (IClickableMenu) new TutorialMenu();
                  goto label_369;
                }
                else
                  goto label_369;
              }
              else if (s == "DivorceBook")
              {
                if (Game1.player.divorceTonight)
                {
                  this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:ManorHouse_DivorceBook_CancelQuestion"), this.createYesNoResponses(), "divorceCancel");
                  goto label_369;
                }
                else if (Game1.player.isMarried())
                {
                  this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:ManorHouse_DivorceBook_Question"), this.createYesNoResponses(), "divorce");
                  goto label_369;
                }
                else
                {
                  Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:ManorHouse_DivorceBook_NoSpouse"));
                  goto label_369;
                }
              }
              else
                goto label_369;
            }
            else if (s == "LockedDoorWarp")
            {
              who.faceGeneralDirection(new Vector2((float) tileLocation.X, (float) tileLocation.Y) * (float) Game1.tileSize, 0);
              this.lockedDoorWarp(strArray);
              goto label_369;
            }
            else
              goto label_369;
          }
          else if ((int) stringHash != -2015468874)
          {
            if ((int) stringHash != -1999286711)
            {
              if ((int) stringHash == -1881500416 && s == "RailroadBox")
              {
                if (who.ActiveObject != null && who.ActiveObject.parentSheetIndex == 394 && (!who.hasOrWillReceiveMail("TH_Railroad") && who.hasOrWillReceiveMail("TH_Tunnel")))
                {
                  who.reduceActiveItemByOne();
                  Game1.player.CanMove = false;
                  Game1.playSound("Ship");
                  Game1.player.mailReceived.Add("TH_Railroad");
                  Game1.multipleDialogues(new string[2]
                  {
                    Game1.content.LoadString("Strings\\Locations:Railroad_Box_ConsumeShell"),
                    Game1.content.LoadString("Strings\\Locations:Railroad_Box_MrQiNote")
                  });
                  Game1.player.removeQuest(2);
                  Game1.player.addQuest(3);
                  goto label_369;
                }
                else if (who.hasOrWillReceiveMail("TH_Railroad"))
                {
                  Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Railroad_Box_MrQiNote"));
                  goto label_369;
                }
                else
                {
                  Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Railroad_Box_Initial"));
                  goto label_369;
                }
              }
              else
                goto label_369;
            }
            else if (s == "Door")
            {
              if (strArray.Length > 1 && !Game1.eventUp)
              {
                for (int index = 1; index < strArray.Length; ++index)
                {
                  if (who.getFriendshipHeartLevelForNPC(strArray[index]) >= 2 || Game1.player.mailReceived.Contains("doorUnlock" + strArray[index]))
                  {
                    Rumble.rumble(0.1f, 100f);
                    if (!Game1.player.mailReceived.Contains("doorUnlock" + strArray[index]))
                      Game1.player.mailReceived.Add("doorUnlock" + strArray[index]);
                    this.openDoor(tileLocation, true);
                    return false;
                  }
                }
                if (strArray.Length == 2 && (object) Game1.getCharacterFromName(strArray[1], false) != null)
                {
                  NPC characterFromName = Game1.getCharacterFromName(strArray[1], false);
                  Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:DoorUnlock_NotFriend_" + (characterFromName.gender == 0 ? "Male" : "Female"), (object) characterFromName.displayName));
                  goto label_369;
                }
                else if ((object) Game1.getCharacterFromName(strArray[1], false) != null && (object) Game1.getCharacterFromName(strArray[2], false) != null)
                {
                  NPC characterFromName1 = Game1.getCharacterFromName(strArray[1], false);
                  NPC characterFromName2 = Game1.getCharacterFromName(strArray[2], false);
                  Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:DoorUnlock_NotFriend_Couple", (object) characterFromName1.displayName, (object) characterFromName2.displayName));
                  goto label_369;
                }
                else if ((object) Game1.getCharacterFromName(strArray[1], false) != null)
                {
                  NPC characterFromName = Game1.getCharacterFromName(strArray[1], false);
                  Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:DoorUnlock_NotFriend_" + (characterFromName.gender == 0 ? "Male" : "Female"), (object) characterFromName.displayName));
                  goto label_369;
                }
                else
                  goto label_369;
              }
              else
              {
                this.openDoor(tileLocation, true);
                return false;
              }
            }
            else
              goto label_369;
          }
          else if (s == "WarpGreenhouse")
          {
            if (Game1.player.mailReceived.Contains("ccPantry"))
            {
              who.faceGeneralDirection(new Vector2((float) tileLocation.X, (float) tileLocation.Y) * (float) Game1.tileSize, 0);
              Game1.warpFarmer("Greenhouse", 10, 23, false);
              Game1.playSound("doorClose");
              goto label_369;
            }
            else
            {
              Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Farm_GreenhouseRuins"));
              goto label_369;
            }
          }
          else
            goto label_369;
        }
        else if (stringHash <= 3371180897U)
        {
          if (stringHash <= 2909376585U)
          {
            if (stringHash <= 2764184545U)
            {
              if (stringHash <= 2510785065U)
              {
                if ((int) stringHash != -1823855148)
                {
                  if ((int) stringHash == -1784182231 && s == "EvilShrineCenter")
                  {
                    if (who.isDivorced())
                    {
                      this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:WitchHut_EvilShrineCenter"), this.createYesNoResponses(), "evilShrineCenter");
                      goto label_369;
                    }
                    else
                    {
                      Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:WitchHut_EvilShrineCenterInactive"));
                      goto label_369;
                    }
                  }
                  else
                    goto label_369;
                }
                else if (s == "FarmerFile")
                  goto label_346;
                else
                  goto label_369;
              }
              else if ((int) stringHash != -1766050189)
              {
                if ((int) stringHash != -1556035170)
                {
                  if ((int) stringHash == -1530782751 && s == "MinecartTransport")
                  {
                    if (Game1.player.mailReceived.Contains("ccBoilerRoom"))
                    {
                      Response[] answerChoices;
                      if (Game1.player.mailReceived.Contains("ccCraftsRoom"))
                        answerChoices = new Response[4]
                        {
                          new Response("Town", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Town")),
                          new Response("Bus", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_BusStop")),
                          new Response("Quarry", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Quarry")),
                          new Response("Cancel", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Cancel"))
                        };
                      else
                        answerChoices = new Response[3]
                        {
                          new Response("Town", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Town")),
                          new Response("Bus", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_BusStop")),
                          new Response("Cancel", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Cancel"))
                        };
                      this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:MineCart_ChooseDestination"), answerChoices, "Minecart");
                      goto label_369;
                    }
                    else
                    {
                      Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:MineCart_OutOfOrder"));
                      goto label_369;
                    }
                  }
                  else
                    goto label_369;
                }
                else if (s == "ClubSeller")
                {
                  this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:Club_ClubSeller"), new Response[2]
                  {
                    new Response("I'll", Game1.content.LoadString("Strings\\Locations:Club_ClubSeller_Yes")),
                    new Response("No", Game1.content.LoadString("Strings\\Locations:Club_ClubSeller_No"))
                  }, "ClubSeller");
                  goto label_369;
                }
                else
                  goto label_369;
              }
              else if (s == "StorageBox")
              {
                this.openStorageBox(this.actionParamsToString(strArray));
                goto label_369;
              }
              else
                goto label_369;
            }
            else if (stringHash <= 2815316590U)
            {
              if ((int) stringHash != -1492825596)
              {
                if ((int) stringHash == -1479650706 && s == "MayorFridge")
                {
                  int num1 = 0;
                  for (int index = 0; index < who.items.Count; ++index)
                  {
                    if (who.items[index] != null && who.items[index].parentSheetIndex == 284)
                      num1 += who.items[index].Stack;
                  }
                  if (num1 >= 10 && !who.hasOrWillReceiveMail("TH_MayorFridge") && who.hasOrWillReceiveMail("TH_Railroad"))
                  {
                    int num2 = 0;
                    for (int index = 0; index < who.items.Count; ++index)
                    {
                      if (who.items[index] != null && who.items[index].parentSheetIndex == 284)
                      {
                        while (num2 < 10)
                        {
                          --who.items[index].Stack;
                          ++num2;
                          if (who.items[index].Stack == 0)
                          {
                            who.items[index] = (Item) null;
                            break;
                          }
                        }
                        if (num2 >= 10)
                          break;
                      }
                    }
                    Game1.player.CanMove = false;
                    Game1.playSound("coin");
                    Game1.player.mailReceived.Add("TH_MayorFridge");
                    Game1.multipleDialogues(new string[2]
                    {
                      Game1.content.LoadString("Strings\\Locations:ManorHouse_MayorFridge_ConsumeBeets"),
                      Game1.content.LoadString("Strings\\Locations:ManorHouse_MayorFridge_MrQiNote")
                    });
                    Game1.player.removeQuest(3);
                    Game1.player.addQuest(4);
                    goto label_369;
                  }
                  else if (who.hasOrWillReceiveMail("TH_MayorFridge"))
                  {
                    Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:ManorHouse_MayorFridge_MrQiNote"));
                    goto label_369;
                  }
                  else
                  {
                    Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:ManorHouse_MayorFridge_Initial"));
                    goto label_369;
                  }
                }
                else
                  goto label_369;
              }
              else if (s == "Billboard")
              {
                Game1.activeClickableMenu = (IClickableMenu) new Billboard(strArray[1].Equals("3"));
                goto label_369;
              }
              else
                goto label_369;
            }
            else if ((int) stringHash != -1477872992)
            {
              if ((int) stringHash != -1461978761)
              {
                if ((int) stringHash == -1385590711 && s == "Saloon" && who.getTileY() > tileLocation.Y)
                {
                  this.saloon(tileLocation);
                  goto label_369;
                }
                else
                  goto label_369;
              }
              else if (s == "AdventureShop")
              {
                this.adventureShop();
                goto label_369;
              }
              else
                goto label_369;
            }
            else if (s == "Incubator")
            {
              (this as AnimalHouse).incubator();
              goto label_369;
            }
            else
              goto label_369;
          }
          else if (stringHash <= 3158608557U)
          {
            if (stringHash <= 2959114096U)
            {
              if ((int) stringHash != -1374758524)
              {
                if ((int) stringHash == -1335853200 && s == "GetLumber")
                {
                  this.GetLumber();
                  goto label_369;
                }
                else
                  goto label_369;
              }
              else if (s == "Message")
              {
                Game1.drawDialogueNoTyping(Game1.content.LoadString("Strings\\StringsFromMaps:" + strArray[1].Replace("\"", "")));
                goto label_369;
              }
              else
                goto label_369;
            }
            else if ((int) stringHash != -1307486613)
            {
              if ((int) stringHash != -1139903097)
              {
                if ((int) stringHash == -1136358739)
                {
                  if (s == "Starpoint")
                  {
                    try
                    {
                      this.doStarpoint(strArray[1]);
                      goto label_369;
                    }
                    catch (Exception ex)
                    {
                      goto label_369;
                    }
                  }
                  else
                    goto label_369;
                }
                else
                  goto label_369;
              }
              else if (s == "farmstand")
              {
                Game1.shipHeldItem();
                goto label_369;
              }
              else
                goto label_369;
            }
            else if (s == "Mailbox")
            {
              this.mailbox();
              goto label_369;
            }
            else
              goto label_369;
          }
          else if (stringHash <= 3244018497U)
          {
            if ((int) stringHash != -1132692925)
            {
              if ((int) stringHash != -1083763529)
              {
                if ((int) stringHash == -1050948799 && s == "WarpCommunityCenter")
                {
                  if (who.mailReceived.Contains("ccDoorUnlock") || who.mailReceived.Contains("JojaMember"))
                  {
                    Game1.warpFarmer("CommunityCenter", 32, 23, false);
                    Game1.playSound("doorClose");
                    goto label_369;
                  }
                  else
                  {
                    Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:GameLocation.cs.8175"));
                    goto label_369;
                  }
                }
                else
                  goto label_369;
              }
              else if (s == "HMTGF" && who.ActiveObject != null && (who.ActiveObject != null && !who.ActiveObject.bigCraftable) && who.ActiveObject.parentSheetIndex == 155)
              {
                who.ActiveObject = new Object(Vector2.Zero, 155, false);
                Game1.playSound("discoverMineral");
                Game1.flashAlpha = 1f;
                goto label_369;
              }
              else
                goto label_369;
            }
            else if (s == "MineSign")
            {
              Game1.drawObjectDialogue(Game1.parseText(this.actionParamsToString(strArray)));
              goto label_369;
            }
            else
              goto label_369;
          }
          else if ((int) stringHash != -966994542)
          {
            if ((int) stringHash != -965964524)
            {
              if ((int) stringHash == -923786399 && s == "Craft")
              {
                GameLocation.openCraftingMenu(this.actionParamsToString(strArray));
                goto label_369;
              }
              else
                goto label_369;
            }
            else if (s == "Minecart")
            {
              this.openChest(tileLocation, 4, Game1.random.Next(3, 7));
              goto label_369;
            }
            else
              goto label_369;
          }
          else if (s == "TunnelSafe")
          {
            if (who.ActiveObject != null && who.ActiveObject.parentSheetIndex == 787 && !who.hasOrWillReceiveMail("TH_Tunnel"))
            {
              who.reduceActiveItemByOne();
              Game1.player.CanMove = false;
              Game1.playSound("openBox");
              DelayedAction.playSoundAfterDelay("doorCreakReverse", 500);
              Game1.player.mailReceived.Add("TH_Tunnel");
              Game1.multipleDialogues(new string[2]
              {
                Game1.content.LoadString("Strings\\Locations:Tunnel_TunnelSafe_ConsumeBattery"),
                Game1.content.LoadString("Strings\\Locations:Tunnel_TunnelSafe_MrQiNote")
              });
              Game1.player.addQuest(2);
              goto label_369;
            }
            else if (who.hasOrWillReceiveMail("TH_Tunnel"))
            {
              Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Tunnel_TunnelSafe_MrQiNote"));
              goto label_369;
            }
            else
            {
              Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Tunnel_TunnelSafe_Initial"));
              goto label_369;
            }
          }
          else
            goto label_369;
        }
        else if (stringHash <= 3912414904U)
        {
          if (stringHash <= 3424064554U)
          {
            if (stringHash <= 3385614082U)
            {
              if ((int) stringHash != -922490522)
              {
                if ((int) stringHash == -909353214 && s == "playSound")
                {
                  Game1.playSound(strArray[1]);
                  goto label_369;
                }
                else
                  goto label_369;
              }
              else if (s == "LumberPile" && !who.hasOrWillReceiveMail("TH_LumberPile") && who.hasOrWillReceiveMail("TH_SandDragon"))
              {
                Game1.player.hasClubCard = true;
                Game1.player.CanMove = false;
                Game1.player.mailReceived.Add("TH_LumberPile");
                Game1.player.addItemByMenuIfNecessaryElseHoldUp((Item) new SpecialItem(1, 2, ""), (ItemGrabMenu.behaviorOnItemSelect) null);
                Game1.player.removeQuest(5);
                goto label_369;
              }
              else
                goto label_369;
            }
            else if ((int) stringHash != -891652085)
            {
              if ((int) stringHash != -875212928)
              {
                if ((int) stringHash == -870902742 && s == "Gunther")
                {
                  this.gunther();
                  goto label_369;
                }
                else
                  goto label_369;
              }
              else if (s == "Material")
              {
                Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:GameLocation.cs.8205", (object) who.WoodPieces, (object) who.StonePieces).Replace("\n", "^"));
                goto label_369;
              }
              else
                goto label_369;
            }
            else if (s == "Buy" && who.getTileY() >= tileLocation.Y)
            {
              this.openShopMenu(strArray[1]);
              goto label_369;
            }
            else
              goto label_369;
          }
          else if (stringHash <= 3642125430U)
          {
            if ((int) stringHash != -691218215)
            {
              if ((int) stringHash == -652841866 && s == "ExitMine")
              {
                this.createQuestionDialogue(" ", new Response[3]
                {
                  new Response("Leave", Game1.content.LoadString("Strings\\Locations:Mines_LeaveMine")),
                  new Response("Go", Game1.content.LoadString("Strings\\Locations:Mines_GoUp")),
                  new Response("Do", Game1.content.LoadString("Strings\\Locations:Mines_DoNothing"))
                }, "ExitMine");
                goto label_369;
              }
              else
                goto label_369;
            }
            else if (s == "EnterSewer")
            {
              if (who.hasRustyKey && !who.mailReceived.Contains("OpenedSewer"))
              {
                Game1.playSound("openBox");
                Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\Locations:Forest_OpenedSewer")));
                who.mailReceived.Add("OpenedSewer");
                goto label_369;
              }
              else if (who.mailReceived.Contains("OpenedSewer"))
              {
                Game1.warpFarmer("Sewer", 16, 11, 2);
                Game1.playSound("stairsdown");
                goto label_369;
              }
              else
              {
                Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:LockedDoor"));
                goto label_369;
              }
            }
            else
              goto label_369;
          }
          else if ((int) stringHash != -540509786)
          {
            if ((int) stringHash != -446069546)
            {
              if ((int) stringHash == -382552392 && s == "DwarfGrave")
              {
                if (who.canUnderstandDwarves)
                {
                  Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Town_DwarfGrave_Translated").Replace('\n', '^'));
                  goto label_369;
                }
                else
                {
                  Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:GameLocation.cs.8214"));
                  goto label_369;
                }
              }
              else
                goto label_369;
            }
            else if (!(s == "Mine"))
              goto label_369;
          }
          else if (s == "SkullDoor")
          {
            if (who.hasSkullKey)
            {
              if (!who.hasUnlockedSkullDoor)
              {
                Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\Locations:SkullCave_SkullDoor_Unlock")));
                DelayedAction.playSoundAfterDelay("openBox", 500);
                DelayedAction.playSoundAfterDelay("openBox", 700);
                Game1.addMailForTomorrow("skullCave", false, false);
                who.hasUnlockedSkullDoor = true;
                who.completeQuest(19);
                goto label_369;
              }
              else
              {
                who.completelyStopAnimatingOrDoingAction();
                Game1.playSound("doorClose");
                DelayedAction.playSoundAfterDelay("stairsdown", 500);
                Game1.enterMine(true, 121, (string) null);
                goto label_369;
              }
            }
            else
            {
              Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:SkullCave_SkullDoor_Locked"));
              goto label_369;
            }
          }
          else
            goto label_369;
        }
        else if (stringHash <= 4067857873U)
        {
          if (stringHash <= 3970342769U)
          {
            if ((int) stringHash != -333628581)
            {
              if ((int) stringHash == -324624527 && s == "ClubShop")
              {
                Game1.activeClickableMenu = (IClickableMenu) new ShopMenu(Utility.getQiShopStock(), 2, (string) null);
                goto label_369;
              }
              else
                goto label_369;
            }
            else if (s == "Lamp")
            {
              this.lightLevel = (double) this.lightLevel != 0.0 ? 0.0f : 0.6f;
              Game1.playSound("openBox");
              goto label_369;
            }
            else
              goto label_369;
          }
          else if ((int) stringHash != -316155903)
          {
            if ((int) stringHash != -282875293)
            {
              if ((int) stringHash == -227109423 && s == "EvilShrineLeft")
              {
                if (who.getChildren().Count<Child>() == 0)
                {
                  Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:WitchHut_EvilShrineLeftInactive"));
                  goto label_369;
                }
                else
                {
                  this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:WitchHut_EvilShrineLeft"), this.createYesNoResponses(), "evilShrineLeft");
                  goto label_369;
                }
              }
              else
                goto label_369;
            }
            else if (s == "Arcade_Minecart")
            {
              if (who.hasSkullKey)
              {
                Response[] answerChoices = new Response[3]
                {
                  new Response("Progress", Game1.content.LoadString("Strings\\Locations:Saloon_Arcade_Minecart_ProgressMode")),
                  new Response("Endless", Game1.content.LoadString("Strings\\Locations:Saloon_Arcade_Minecart_EndlessMode")),
                  new Response("Exit", Game1.content.LoadString("Strings\\Locations:Saloon_Arcade_Minecart_Exit"))
                };
                this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:Saloon_Arcade_Minecart_Menu"), answerChoices, "MinecartGame");
                goto label_369;
              }
              else
              {
                Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Saloon_Arcade_Minecart_Inactive"));
                goto label_369;
              }
            }
            else
              goto label_369;
          }
          else if (s == "AnimalShop" && who.getTileY() > tileLocation.Y)
          {
            this.animalShop(tileLocation);
            goto label_369;
          }
          else
            goto label_369;
        }
        else if (stringHash <= 4097465949U)
        {
          if ((int) stringHash != -221313449)
          {
            if ((int) stringHash != -204497970)
            {
              if ((int) stringHash == -197501347 && s == "EmilyRoomObject")
              {
                if (Game1.player.eventsSeen.Contains(463391) && (Game1.player.spouse == null || !Game1.player.spouse.Equals("Emily")))
                {
                  TemporaryAnimatedSprite temporarySpriteById = this.getTemporarySpriteByID(5858585);
                  if (temporarySpriteById != null && temporarySpriteById is EmilysParrot)
                  {
                    (temporarySpriteById as EmilysParrot).doAction();
                    goto label_369;
                  }
                  else
                    goto label_369;
                }
                else
                {
                  Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:HaleyHouse_EmilyRoomObject"));
                  goto label_369;
                }
              }
              else
                goto label_369;
            }
            else if (s == "RemoveChest")
            {
              Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:RemoveChest"));
              this.map.GetLayer("Buildings").Tiles[tileLocation.X, tileLocation.Y] = (Tile) null;
              goto label_369;
            }
            else
              goto label_369;
          }
          else if (s == "ItemChest")
          {
            this.openItemChest(tileLocation, Convert.ToInt32(strArray[1]));
            goto label_369;
          }
          else
            goto label_369;
        }
        else if ((int) stringHash != -190714015)
        {
          if ((int) stringHash != -82074636)
          {
            if ((int) stringHash == -10374061 && s == "MineWallDecor")
            {
              this.getWallDecorItem(tileLocation);
              goto label_369;
            }
            else
              goto label_369;
          }
          else if (s == "WarpWomensLocker")
          {
            if (who.isMale)
            {
              if (who.IsMainPlayer)
                Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:WomensLocker_WrongGender"));
              return false;
            }
            who.faceGeneralDirection(new Vector2((float) tileLocation.X, (float) tileLocation.Y) * (float) Game1.tileSize, 0);
            Game1.warpFarmer(strArray[3], Convert.ToInt32(strArray[1]), Convert.ToInt32(strArray[2]), false);
            if (strArray.Length < 5)
            {
              Game1.playSound("doorClose");
              goto label_369;
            }
            else
              goto label_369;
          }
          else
            goto label_369;
        }
        else if (s == "ElliottBook")
        {
          if (who.eventsSeen.Contains(41))
          {
            Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\Locations:ElliottHouse_ElliottBook_Filled", (object) Game1.elliottBookName, (object) who.displayName)));
            goto label_369;
          }
          else
          {
            Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:ElliottHouse_ElliottBook_Blank"));
            goto label_369;
          }
        }
        else
          goto label_369;
        Game1.playSound("stairsdown");
        Game1.enterMine(this.name.Equals("Mine"), 1, (string) null);
        goto label_369;
label_346:
        this.farmerFile();
label_369:
        return true;
      }
      if (action != null && !who.IsMainPlayer)
      {
        string str = action.ToString().Split(' ')[0];
        if (!(str == "Minecart"))
        {
          if (!(str == "RemoveChest"))
          {
            if (!(str == "Door"))
            {
              if (str == "TV")
                Game1.tvStation = Game1.random.Next(2);
            }
            else
              this.openDoor(tileLocation, true);
          }
          else
            this.map.GetLayer("Buildings").Tiles[tileLocation.X, tileLocation.Y] = (Tile) null;
        }
        else
          this.openChest(tileLocation, 4, Game1.random.Next(3, 7));
      }
      return false;
    }

    public Vector2 findNearestObject(Vector2 startingPoint, int objectIndex, bool bigCraftable)
    {
      int num = 0;
      Queue<Vector2> vector2Queue1 = new Queue<Vector2>();
      vector2Queue1.Enqueue(startingPoint);
      HashSet<Vector2> vector2Set1 = new HashSet<Vector2>();
      List<Vector2> vector2List = new List<Vector2>();
      Queue<Vector2> vector2Queue2;
      HashSet<Vector2> vector2Set2;
      while (num < 1000)
      {
        if (this.objects.ContainsKey(startingPoint) && this.objects[startingPoint].parentSheetIndex == objectIndex && this.objects[startingPoint].bigCraftable == bigCraftable)
        {
          vector2Queue2 = (Queue<Vector2>) null;
          vector2Set2 = (HashSet<Vector2>) null;
          return startingPoint;
        }
        ++num;
        vector2Set1.Add(startingPoint);
        List<Vector2> adjacentTileLocations = Utility.getAdjacentTileLocations(startingPoint);
        for (int index = 0; index < adjacentTileLocations.Count; ++index)
        {
          if (!vector2Set1.Contains(adjacentTileLocations[index]))
            vector2Queue1.Enqueue(adjacentTileLocations[index]);
        }
        startingPoint = vector2Queue1.Dequeue();
      }
      vector2Queue2 = (Queue<Vector2>) null;
      vector2Set2 = (HashSet<Vector2>) null;
      return Vector2.Zero;
    }

    public void lockedDoorWarp(string[] actionParams)
    {
      if (Utility.isFestivalDay(Game1.dayOfMonth, Game1.currentSeason) && Utility.getStartTimeOfFestival() < 1900)
        Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\Locations:FestivalDay_DoorLocked")));
      else if (actionParams[3].Equals("SeedShop") && Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth).Equals("Wed") && !Game1.player.eventsSeen.Contains(191393))
        Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\Locations:SeedShop_LockedWed")));
      else if (Game1.timeOfDay >= Convert.ToInt32(actionParams[4]) && Game1.timeOfDay < Convert.ToInt32(actionParams[5]) && (actionParams.Length < 7 || Game1.currentSeason.Equals("winter") || Game1.player.friendships.ContainsKey(actionParams[6]) && Game1.player.friendships[actionParams[6]][0] >= Convert.ToInt32(actionParams[7])))
      {
        Rumble.rumble(0.15f, 200f);
        Game1.player.completelyStopAnimatingOrDoingAction();
        Game1.warpFarmer(actionParams[3], Convert.ToInt32(actionParams[1]), Convert.ToInt32(actionParams[2]), false);
        Game1.playSound("doorClose");
      }
      else if (actionParams.Length < 7)
      {
        string str1 = Game1.getTimeOfDayString(Convert.ToInt32(actionParams[4])).Replace(" ", "");
        string str2 = Game1.getTimeOfDayString(Convert.ToInt32(actionParams[5])).Replace(" ", "");
        Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:LockedDoor_OpenRange", (object) str1, (object) str2));
      }
      else if (Game1.timeOfDay < Convert.ToInt32(actionParams[4]) || Game1.timeOfDay >= Convert.ToInt32(actionParams[5]))
      {
        Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:LockedDoor"));
      }
      else
      {
        NPC characterFromName = Game1.getCharacterFromName(actionParams[6], false);
        Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:LockedDoor_FriendsOnly", (object) characterFromName.displayName));
      }
    }

    public void readNote(int which)
    {
      if (Game1.player.archaeologyFound.ContainsKey(102) && Game1.player.archaeologyFound[102][0] >= which)
      {
        string message = Game1.content.LoadString("Strings\\Notes:" + (object) which).Replace('\n', '^');
        if (!Game1.player.mailReceived.Contains("lb_" + (object) which))
          Game1.player.mailReceived.Add("lb_" + (object) which);
        this.removeTemporarySpritesWithID(which);
        Game1.drawLetterMessage(message);
      }
      else
        Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\Notes:Missing")));
    }

    public void mailbox()
    {
      if (Game1.mailbox.Count > 0 && Game1.player.ActiveObject == null)
      {
        if (!Game1.player.mailReceived.Contains(Game1.mailbox.Peek()) && !Game1.mailbox.Peek().Contains("passedOut") && !Game1.mailbox.Peek().Contains("Cooking"))
          Game1.player.mailReceived.Add(Game1.mailbox.Peek());
        string index = Game1.mailbox.Dequeue();
        Dictionary<string, string> dictionary = Game1.content.Load<Dictionary<string, string>>("Data\\mail");
        string str = dictionary.ContainsKey(index) ? dictionary[index] : "";
        if (index.Contains("passedOut"))
        {
          int int32 = Convert.ToInt32(index.Split(' ')[1]);
          switch (new Random(int32).Next(Game1.player.getSpouse() == null || !Game1.player.getSpouse().name.Equals("Harvey") ? 3 : 2))
          {
            case 0:
              str = string.Format(dictionary["passedOut1_" + (int32 > 0 ? "Billed" : "NotBilled") + "_" + (Game1.player.isMale ? "Male" : "Female")], (object) int32);
              break;
            case 1:
              str = string.Format(dictionary["passedOut2"], (object) int32);
              break;
            case 2:
              str = string.Format(dictionary["passedOut3_" + (int32 > 0 ? "Billed" : "NotBilled")], (object) int32);
              break;
          }
        }
        if (str.Length == 0)
          return;
        string mail = str.Replace("@", Game1.player.Name);
        if (mail.Contains("%update"))
          mail = mail.Replace("%update", Utility.getStardewHeroStandingsString());
        Game1.activeClickableMenu = (IClickableMenu) new LetterViewerMenu(mail, index);
      }
      else
      {
        if (Game1.mailbox.Count != 0)
          return;
        Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:GameLocation.cs.8429"));
      }
    }

    public void farmerFile()
    {
      Game1.multipleDialogues(new string[2]
      {
        Game1.content.LoadString("Strings\\UI:FarmerFile_1", (object) Game1.player.name, (object) Game1.stats.StepsTaken, (object) Game1.stats.GiftsGiven, (object) Game1.stats.DaysPlayed, (object) Game1.stats.DirtHoed, (object) Game1.stats.ItemsCrafted, (object) Game1.stats.ItemsCooked, (object) Game1.stats.PiecesOfTrashRecycled).Replace('\n', '^'),
        Game1.content.LoadString("Strings\\UI:FarmerFile_2", (object) Game1.stats.MonstersKilled, (object) Game1.stats.FishCaught, (object) Game1.stats.TimesFished, (object) Game1.stats.SeedsSown, (object) Game1.stats.ItemsShipped).Replace('\n', '^')
      });
    }

    public void openItemChest(Location location, int whichObject)
    {
      Game1.playSound("openBox");
      if (Game1.player.ActiveObject != null)
        return;
      if (whichObject == 434)
      {
        Game1.player.ActiveObject = new Object(Vector2.Zero, 434, "Cosmic Fruit", false, false, false, false);
        Game1.eatHeldObject();
      }
      else
        this.debris.Add(new Debris(whichObject, new Vector2((float) (location.X * Game1.tileSize), (float) (location.Y * Game1.tileSize)), Game1.player.position));
      ++this.map.GetLayer("Buildings").Tiles[location.X, location.Y].TileIndex;
      this.map.GetLayer("Buildings").Tiles[location].Properties["Action"] = new PropertyValue("RemoveChest");
    }

    public void getWallDecorItem(Location location)
    {
    }

    public static string getFavoriteItemName(string character)
    {
      string str = "???";
      if (Game1.NPCGiftTastes.ContainsKey(character))
      {
        string[] strArray = Game1.NPCGiftTastes[character].Split('/')[1].Split(' ');
        str = Game1.objectInformation[Convert.ToInt32(strArray[Game1.random.Next(strArray.Length)])].Split('/')[0];
      }
      return str;
    }

    public static void openCraftingMenu(string nameOfCraftingDevice)
    {
      Game1.activeClickableMenu = (IClickableMenu) new GameMenu(4, -1);
    }

    private void openStorageBox(string which)
    {
    }

    private void adventureShop()
    {
      Dictionary<Item, int[]> itemPriceAndStock = new Dictionary<Item, int[]>();
      int maxValue = int.MaxValue;
      itemPriceAndStock.Add((Item) new MeleeWeapon(12), new int[2]
      {
        250,
        maxValue
      });
      if (Game1.mine != null)
      {
        if (Game1.mine.lowestLevelReached >= 15)
          itemPriceAndStock.Add((Item) new MeleeWeapon(17), new int[2]
          {
            500,
            maxValue
          });
        if (Game1.mine.lowestLevelReached >= 20)
          itemPriceAndStock.Add((Item) new MeleeWeapon(1), new int[2]
          {
            750,
            maxValue
          });
        if (Game1.mine.lowestLevelReached >= 25)
        {
          itemPriceAndStock.Add((Item) new MeleeWeapon(43), new int[2]
          {
            850,
            maxValue
          });
          itemPriceAndStock.Add((Item) new MeleeWeapon(44), new int[2]
          {
            1500,
            maxValue
          });
        }
        if (Game1.mine.lowestLevelReached >= 40)
          itemPriceAndStock.Add((Item) new MeleeWeapon(27), new int[2]
          {
            2000,
            maxValue
          });
        if (Game1.mine.lowestLevelReached >= 45)
          itemPriceAndStock.Add((Item) new MeleeWeapon(10), new int[2]
          {
            2000,
            maxValue
          });
        if (Game1.mine.lowestLevelReached >= 55)
          itemPriceAndStock.Add((Item) new MeleeWeapon(7), new int[2]
          {
            4000,
            maxValue
          });
        if (Game1.mine.lowestLevelReached >= 75)
          itemPriceAndStock.Add((Item) new MeleeWeapon(5), new int[2]
          {
            6000,
            maxValue
          });
        if (Game1.mine.lowestLevelReached >= 90)
          itemPriceAndStock.Add((Item) new MeleeWeapon(50), new int[2]
          {
            9000,
            maxValue
          });
        if (Game1.mine.lowestLevelReached >= 120)
          itemPriceAndStock.Add((Item) new MeleeWeapon(9), new int[2]
          {
            25000,
            maxValue
          });
        if (Game1.player.mailReceived.Contains("galaxySword"))
        {
          itemPriceAndStock.Add((Item) new MeleeWeapon(4), new int[2]
          {
            50000,
            maxValue
          });
          itemPriceAndStock.Add((Item) new MeleeWeapon(23), new int[2]
          {
            35000,
            maxValue
          });
          itemPriceAndStock.Add((Item) new MeleeWeapon(29), new int[2]
          {
            75000,
            maxValue
          });
        }
      }
      itemPriceAndStock.Add((Item) new Boots(504), new int[2]
      {
        500,
        maxValue
      });
      if (Game1.mine != null && Game1.mine.lowestLevelReached >= 40)
        itemPriceAndStock.Add((Item) new Boots(508), new int[2]
        {
          1250,
          maxValue
        });
      if (Game1.mine != null && Game1.mine.lowestLevelReached >= 80)
        itemPriceAndStock.Add((Item) new Boots(511), new int[2]
        {
          2500,
          maxValue
        });
      itemPriceAndStock.Add((Item) new Ring(529), new int[2]
      {
        1000,
        maxValue
      });
      itemPriceAndStock.Add((Item) new Ring(530), new int[2]
      {
        1000,
        maxValue
      });
      if (Game1.mine != null && Game1.mine.lowestLevelReached >= 40)
      {
        itemPriceAndStock.Add((Item) new Ring(531), new int[2]
        {
          2500,
          maxValue
        });
        itemPriceAndStock.Add((Item) new Ring(532), new int[2]
        {
          2500,
          maxValue
        });
      }
      if (Game1.mine != null && Game1.mine.lowestLevelReached >= 80)
      {
        itemPriceAndStock.Add((Item) new Ring(533), new int[2]
        {
          5000,
          maxValue
        });
        itemPriceAndStock.Add((Item) new Ring(534), new int[2]
        {
          5000,
          maxValue
        });
      }
      if (Game1.mine != null)
      {
        int lowestLevelReached = Game1.mine.lowestLevelReached;
      }
      if (Game1.player.hasItemWithNameThatContains("Slingshot") != null)
        itemPriceAndStock.Add((Item) new Object(441, int.MaxValue, false, -1, 0), new int[2]
        {
          100,
          maxValue
        });
      if (Game1.player.mailReceived.Contains("Gil_Slime Charmer Ring"))
        itemPriceAndStock.Add((Item) new Ring(520), new int[2]
        {
          25000,
          maxValue
        });
      if (Game1.player.mailReceived.Contains("Gil_Savage Ring"))
        itemPriceAndStock.Add((Item) new Ring(523), new int[2]
        {
          25000,
          maxValue
        });
      if (Game1.player.mailReceived.Contains("Gil_Burglar's Ring"))
        itemPriceAndStock.Add((Item) new Ring(526), new int[2]
        {
          20000,
          maxValue
        });
      if (Game1.player.mailReceived.Contains("Gil_Vampire Ring"))
        itemPriceAndStock.Add((Item) new Ring(522), new int[2]
        {
          15000,
          maxValue
        });
      if (Game1.player.mailReceived.Contains("Gil_Skeleton Mask"))
        itemPriceAndStock.Add((Item) new Hat(8), new int[2]
        {
          20000,
          maxValue
        });
      if (Game1.player.mailReceived.Contains("Gil_Hard Hat"))
        itemPriceAndStock.Add((Item) new Hat(27), new int[2]
        {
          20000,
          maxValue
        });
      if (Game1.player.mailReceived.Contains("Gil_Insect Head"))
        itemPriceAndStock.Add((Item) new MeleeWeapon(13), new int[2]
        {
          10000,
          maxValue
        });
      Game1.activeClickableMenu = (IClickableMenu) new ShopMenu(itemPriceAndStock, 0, "Marlon");
    }

    private void openShopMenu(string which)
    {
      if (which.Equals("Fish"))
      {
        if (this.getCharacterFromName("Willy") == null || (double) this.getCharacterFromName("Willy").getTileLocation().Y >= (double) Game1.player.getTileY())
          return;
        Game1.activeClickableMenu = (IClickableMenu) new ShopMenu(Utility.getFishShopStock(Game1.player), 0, "Willy");
      }
      else if (this is SeedShop)
      {
        if (this.getCharacterFromName("Pierre") != null && (this.getCharacterFromName("Pierre").getTileLocation().Equals(new Vector2(4f, 17f)) && Game1.player.getTileY() > this.getCharacterFromName("Pierre").getTileY()))
          Game1.activeClickableMenu = (IClickableMenu) new ShopMenu((this as SeedShop).shopStock(), 0, "Pierre");
        else
          Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:GameLocation.cs.8525"));
      }
      else
      {
        if (!this.name.Equals("SandyHouse"))
          return;
        Game1.activeClickableMenu = (IClickableMenu) new ShopMenu(this.sandyShopStock(), 0, "Sandy");
      }
    }

    public virtual bool isObjectAt(int x, int y)
    {
      return this.objects.ContainsKey(new Vector2((float) (x / Game1.tileSize), (float) (y / Game1.tileSize)));
    }

    public virtual Object getObjectAt(int x, int y)
    {
      Vector2 key = new Vector2((float) (x / Game1.tileSize), (float) (y / Game1.tileSize));
      if (this.objects.ContainsKey(key))
        return this.objects[key];
      return (Object) null;
    }

    private List<Item> sandyShopStock()
    {
      List<Item> objList = new List<Item>();
      objList.Add((Item) new Object(478, int.MaxValue, false, -1, 0));
      objList.Add((Item) new Object(486, int.MaxValue, false, -1, 0));
      objList.Add((Item) new Object(494, int.MaxValue, false, -1, 0));
      switch (Game1.dayOfMonth % 7)
      {
        case 0:
          objList.Add((Item) new Object(233, int.MaxValue, false, -1, 0));
          break;
        case 1:
          objList.Add((Item) new Object(88, int.MaxValue, false, -1, 0));
          break;
        case 2:
          objList.Add((Item) new Object(90, int.MaxValue, false, -1, 0));
          break;
        case 3:
          objList.Add((Item) new Object(749, int.MaxValue, false, 500, 0));
          break;
        case 4:
          objList.Add((Item) new Object(466, int.MaxValue, false, -1, 0));
          break;
        case 5:
          objList.Add((Item) new Object(340, int.MaxValue, false, -1, 0));
          break;
        case 6:
          objList.Add((Item) new Object(371, int.MaxValue, false, 100, 0));
          break;
      }
      return objList;
    }

    private void saloon(Location tileLocation)
    {
      foreach (NPC character in this.characters)
      {
        if (character.name.Equals("Gus"))
        {
          if (character.getTileY() != Game1.player.getTileY() - 1 && character.getTileY() != Game1.player.getTileY() - 2)
            break;
          character.facePlayer(Game1.player);
          Game1.activeClickableMenu = (IClickableMenu) new ShopMenu(Utility.getSaloonStock(), 0, "Gus");
          break;
        }
      }
    }

    private void carpenters(Location tileLocation)
    {
      if (Game1.player.currentUpgrade != null)
        return;
      foreach (NPC character in this.characters)
      {
        if (character.name.Equals("Robin"))
        {
          if ((double) Vector2.Distance(character.getTileLocation(), new Vector2((float) tileLocation.X, (float) tileLocation.Y)) > 3.0)
            return;
          character.faceDirection(2);
          if (Game1.player.daysUntilHouseUpgrade < 0 && !Game1.getFarm().isThereABuildingUnderConstruction())
          {
            Response[] answerChoices;
            if (Game1.player.houseUpgradeLevel < 3)
              answerChoices = new Response[4]
              {
                new Response("Shop", Game1.content.LoadString("Strings\\Locations:ScienceHouse_CarpenterMenu_Shop")),
                new Response("Upgrade", Game1.content.LoadString("Strings\\Locations:ScienceHouse_CarpenterMenu_UpgradeHouse")),
                new Response("Construct", Game1.content.LoadString("Strings\\Locations:ScienceHouse_CarpenterMenu_Construct")),
                new Response("Leave", Game1.content.LoadString("Strings\\Locations:ScienceHouse_CarpenterMenu_Leave"))
              };
            else
              answerChoices = new Response[3]
              {
                new Response("Shop", Game1.content.LoadString("Strings\\Locations:ScienceHouse_CarpenterMenu_Shop")),
                new Response("Construct", Game1.content.LoadString("Strings\\Locations:ScienceHouse_CarpenterMenu_Construct")),
                new Response("Leave", Game1.content.LoadString("Strings\\Locations:ScienceHouse_CarpenterMenu_Leave"))
              };
            this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:ScienceHouse_CarpenterMenu"), answerChoices, "carpenter");
            return;
          }
          Game1.activeClickableMenu = (IClickableMenu) new ShopMenu(Utility.getCarpenterStock(), 0, "Robin");
          return;
        }
      }
      if (!Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth).Equals("Tue"))
        return;
      Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:ScienceHouse_RobinAbsent").Replace('\n', '^'));
    }

    private void blacksmith(Location tileLocation)
    {
      foreach (NPC character in this.characters)
      {
        if (character.name.Equals("Clint"))
        {
          Vector2 tileLocation1 = character.getTileLocation();
          if (!tileLocation1.Equals(new Vector2((float) tileLocation.X, (float) (tileLocation.Y - 1))))
          {
            tileLocation1 = character.getTileLocation();
            tileLocation1.Equals(new Vector2((float) (tileLocation.X - 1), (float) (tileLocation.Y - 1)));
          }
          character.faceDirection(2);
          if (Game1.player.toolBeingUpgraded == null)
          {
            Response[] answerChoices;
            if (Game1.player.hasItemInInventory(535, 1, 0) || Game1.player.hasItemInInventory(536, 1, 0) || (Game1.player.hasItemInInventory(537, 1, 0) || Game1.player.hasItemInInventory(749, 1, 0)))
              answerChoices = new Response[4]
              {
                new Response("Shop", Game1.content.LoadString("Strings\\Locations:Blacksmith_Clint_Shop")),
                new Response("Upgrade", Game1.content.LoadString("Strings\\Locations:Blacksmith_Clint_Upgrade")),
                new Response("Process", Game1.content.LoadString("Strings\\Locations:Blacksmith_Clint_Geodes")),
                new Response("Leave", Game1.content.LoadString("Strings\\Locations:Blacksmith_Clint_Leave"))
              };
            else
              answerChoices = new Response[3]
              {
                new Response("Shop", Game1.content.LoadString("Strings\\Locations:Blacksmith_Clint_Shop")),
                new Response("Upgrade", Game1.content.LoadString("Strings\\Locations:Blacksmith_Clint_Upgrade")),
                new Response("Leave", Game1.content.LoadString("Strings\\Locations:Blacksmith_Clint_Leave"))
              };
            this.createQuestionDialogue("", answerChoices, "Blacksmith");
            break;
          }
          if (Game1.player.daysLeftForToolUpgrade <= 0)
          {
            if (Game1.player.freeSpotsInInventory() > 0)
            {
              Game1.player.holdUpItemThenMessage((Item) Game1.player.toolBeingUpgraded, true);
              Game1.player.addItemToInventoryBool((Item) Game1.player.toolBeingUpgraded, false);
              Game1.player.toolBeingUpgraded = (Tool) null;
              break;
            }
            Game1.drawDialogue(character, Game1.content.LoadString("Data\\ExtraDialogue:Clint_NoInventorySpace"));
            break;
          }
          Game1.drawDialogue(character, Game1.content.LoadString("Data\\ExtraDialogue:Clint_StillWorking", (object) Game1.player.toolBeingUpgraded.DisplayName));
          break;
        }
      }
    }

    private void animalShop(Location tileLocation)
    {
      foreach (NPC character in this.characters)
      {
        if (character.name.Equals("Marnie"))
        {
          if (!character.getTileLocation().Equals(new Vector2((float) tileLocation.X, (float) (tileLocation.Y - 1))))
            return;
          character.faceDirection(2);
          this.createQuestionDialogue("", new Response[3]
          {
            new Response("Supplies", Game1.content.LoadString("Strings\\Locations:AnimalShop_Marnie_Supplies")),
            new Response("Purchase", Game1.content.LoadString("Strings\\Locations:AnimalShop_Marnie_Animals")),
            new Response("Leave", Game1.content.LoadString("Strings\\Locations:AnimalShop_Marnie_Leave"))
          }, "Marnie");
          return;
        }
      }
      if (!Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth).Equals("Tue"))
        return;
      Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:AnimalShop_Marnie_Absent").Replace('\n', '^'));
    }

    private void gunther()
    {
      if ((this as LibraryMuseum).doesFarmerHaveAnythingToDonate(Game1.player))
      {
        Response[] answerChoices;
        if ((this as LibraryMuseum).getRewardsForPlayer(Game1.player).Count > 0)
          answerChoices = new Response[3]
          {
            new Response("Donate", Game1.content.LoadString("Strings\\Locations:ArchaeologyHouse_Gunther_Donate")),
            new Response("Collect", Game1.content.LoadString("Strings\\Locations:ArchaeologyHouse_Gunther_Collect")),
            new Response("Leave", Game1.content.LoadString("Strings\\Locations:ArchaeologyHouse_Gunther_Leave"))
          };
        else
          answerChoices = new Response[2]
          {
            new Response("Donate", Game1.content.LoadString("Strings\\Locations:ArchaeologyHouse_Gunther_Donate")),
            new Response("Leave", Game1.content.LoadString("Strings\\Locations:ArchaeologyHouse_Gunther_Leave"))
          };
        this.createQuestionDialogue("", answerChoices, "Museum");
      }
      else if ((this as LibraryMuseum).getRewardsForPlayer(Game1.player).Count > 0)
        this.createQuestionDialogue("", new Response[2]
        {
          new Response("Collect", Game1.content.LoadString("Strings\\Locations:ArchaeologyHouse_Gunther_Collect")),
          new Response("Leave", Game1.content.LoadString("Strings\\Locations:ArchaeologyHouse_Gunther_Leave"))
        }, "Museum");
      else if (Game1.player.achievements.Contains(5))
        Game1.drawDialogue(Game1.getCharacterFromName("Gunther", false), Game1.parseText(Game1.content.LoadString("Data\\ExtraDialogue:Gunther_MuseumComplete")));
      else
        Game1.drawDialogue(Game1.getCharacterFromName("Gunther", false), Game1.player.mailReceived.Contains("artifactFound") ? Game1.parseText(Game1.content.LoadString("Data\\ExtraDialogue:Gunther_NothingToDonate")) : Game1.content.LoadString("Data\\ExtraDialogue:Gunther_NoArtifactsFound"));
    }

    public void openChest(Location location, int debrisType, int numberOfChunks)
    {
      int[] debrisType1 = new int[1]{ debrisType };
      this.openChest(location, debrisType1, numberOfChunks);
    }

    public void openChest(Location location, int[] debrisType, int numberOfChunks)
    {
      Game1.playSound("openBox");
      ++this.map.GetLayer("Buildings").Tiles[location.X, location.Y].TileIndex;
      for (int index = 0; index < debrisType.Length; ++index)
        Game1.createDebris(debrisType[index], location.X, location.Y, numberOfChunks, (GameLocation) null);
      this.map.GetLayer("Buildings").Tiles[location].Properties["Action"] = new PropertyValue("RemoveChest");
    }

    public string actionParamsToString(string[] actionparams)
    {
      string str = actionparams[1];
      for (int index = 2; index < actionparams.Length; ++index)
        str = str + " " + actionparams[index];
      return str;
    }

    private void GetLumber()
    {
      if (Game1.player.ActiveObject == null && Game1.player.WoodPieces > 0)
      {
        Game1.player.grabObject(new Object(Vector2.Zero, 30, "Lumber", true, true, false, false));
        --Game1.player.WoodPieces;
      }
      else
      {
        if (Game1.player.ActiveObject == null || !Game1.player.ActiveObject.Name.Equals("Lumber"))
          return;
        Game1.player.CanMove = false;
        switch (Game1.player.FacingDirection)
        {
          case 0:
            ((FarmerSprite) Game1.player.Sprite).animateBackwardsOnce(80, 75f);
            break;
          case 1:
            ((FarmerSprite) Game1.player.Sprite).animateBackwardsOnce(72, 75f);
            break;
          case 2:
            ((FarmerSprite) Game1.player.Sprite).animateBackwardsOnce(64, 75f);
            break;
          case 3:
            ((FarmerSprite) Game1.player.Sprite).animateBackwardsOnce(88, 75f);
            break;
        }
        Game1.player.ActiveObject = (Object) null;
        ++Game1.player.WoodPieces;
      }
    }

    public void removeTile(Location tileLocation, string layer)
    {
      this.map.GetLayer(layer).Tiles[tileLocation.X, tileLocation.Y] = (Tile) null;
    }

    public void removeTile(int x, int y, string layer)
    {
      this.map.GetLayer(layer).Tiles[x, y] = (Tile) null;
    }

    public bool characterDestroyObjectWithinRectangle(Microsoft.Xna.Framework.Rectangle rect, bool showDestroyedObject)
    {
      if (this is FarmHouse || rect.Intersects(Game1.player.GetBoundingBox()))
        return false;
      Vector2 key = new Vector2((float) (rect.X / Game1.tileSize), (float) (rect.Y / Game1.tileSize));
      Object @object;
      this.objects.TryGetValue(key, out @object);
      if (@object != null && !@object.IsHoeDirt && (!@object.isPassable() && !this.map.GetLayer("Back").Tiles[(int) key.X, (int) key.Y].Properties.ContainsKey("NPCBarrier")))
      {
        if (!@object.IsHoeDirt)
        {
          if (@object.IsSpawnedObject)
            this.numberOfSpawnedObjectsOnMap = this.numberOfSpawnedObjectsOnMap - 1;
          if (showDestroyedObject && !@object.bigCraftable)
            this.temporarySprites.Add(new TemporaryAnimatedSprite(@object.ParentSheetIndex, 150f, 1, 3, new Vector2(key.X * (float) Game1.tileSize, key.Y * (float) Game1.tileSize), false, @object.flipped)
            {
              alphaFade = 0.01f
            });
          @object.performToolAction((Tool) null);
          if (!(@object is Chest) && this.objects.ContainsKey(key))
            this.objects.Remove(key);
        }
        return true;
      }
      key.X = (float) (rect.Right / Game1.tileSize);
      this.objects.TryGetValue(key, out @object);
      if (@object != null && !@object.IsHoeDirt && (!@object.isPassable() && !this.map.GetLayer("Back").Tiles[(int) key.X, (int) key.Y].Properties.ContainsKey("NPCBarrier")))
      {
        if (!@object.IsHoeDirt)
        {
          if (@object.IsSpawnedObject)
            this.numberOfSpawnedObjectsOnMap = this.numberOfSpawnedObjectsOnMap - 1;
          if (showDestroyedObject && !@object.bigCraftable)
            this.temporarySprites.Add(new TemporaryAnimatedSprite(@object.ParentSheetIndex, 150f, 1, 3, new Vector2(key.X * (float) Game1.tileSize, key.Y * (float) Game1.tileSize), false, @object.flipped)
            {
              alphaFade = 0.01f
            });
          @object.performToolAction((Tool) null);
          if (this.objects.ContainsKey(key))
            this.objects.Remove(key);
        }
        return true;
      }
      key.X = (float) (rect.X / Game1.tileSize);
      key.Y = (float) (rect.Bottom / Game1.tileSize);
      this.objects.TryGetValue(key, out @object);
      if (@object != null && !@object.IsHoeDirt && (!@object.isPassable() && !this.map.GetLayer("Back").Tiles[(int) key.X, (int) key.Y].Properties.ContainsKey("NPCBarrier")))
      {
        if (!@object.IsHoeDirt)
        {
          if (@object.IsSpawnedObject)
            this.numberOfSpawnedObjectsOnMap = this.numberOfSpawnedObjectsOnMap - 1;
          if (showDestroyedObject && !@object.bigCraftable)
            this.temporarySprites.Add(new TemporaryAnimatedSprite(@object.ParentSheetIndex, 150f, 1, 3, new Vector2(key.X * (float) Game1.tileSize, key.Y * (float) Game1.tileSize), false, @object.flipped)
            {
              alphaFade = 0.01f
            });
          @object.performToolAction((Tool) null);
          if (this.objects.ContainsKey(key))
            this.objects.Remove(key);
        }
        return true;
      }
      key.X = (float) (rect.Right / Game1.tileSize);
      this.objects.TryGetValue(key, out @object);
      if (@object == null || @object.IsHoeDirt || (@object.isPassable() || this.map.GetLayer("Back").Tiles[(int) key.X, (int) key.Y].Properties.ContainsKey("NPCBarrier")))
        return false;
      if (!@object.IsHoeDirt)
      {
        if (@object.IsSpawnedObject)
          this.numberOfSpawnedObjectsOnMap = this.numberOfSpawnedObjectsOnMap - 1;
        if (showDestroyedObject && !@object.bigCraftable)
          this.temporarySprites.Add(new TemporaryAnimatedSprite(@object.ParentSheetIndex, 150f, 1, 3, new Vector2(key.X * (float) Game1.tileSize, key.Y * (float) Game1.tileSize), false, @object.flipped)
          {
            alphaFade = 0.01f
          });
        @object.performToolAction((Tool) null);
        if (this.objects.ContainsKey(key))
          this.objects.Remove(key);
      }
      return true;
    }

    public Object removeObject(Vector2 location, bool showDestroyedObject)
    {
      Object object1;
      this.objects.TryGetValue(location, out object1);
      if (object1 == null || !(object1.CanBeGrabbed | showDestroyedObject))
        return (Object) null;
      if (object1.IsSpawnedObject)
        this.numberOfSpawnedObjectsOnMap = this.numberOfSpawnedObjectsOnMap - 1;
      Object object2 = this.objects[location];
      this.objects.Remove(location);
      if (showDestroyedObject)
        this.temporarySprites.Add(new TemporaryAnimatedSprite(object2.Type.Equals("Crafting") ? object2.ParentSheetIndex : object2.ParentSheetIndex + 1, 150f, 1, 3, new Vector2(location.X * (float) Game1.tileSize, location.Y * (float) Game1.tileSize), true, object2.bigCraftable, object2.flipped));
      if (object1.Name.Contains("Weed"))
      {
        ++Game1.stats.WeedsEliminated;
        if (Game1.questOfTheDay != null && Game1.questOfTheDay.accepted && (!Game1.questOfTheDay.completed && Game1.questOfTheDay.GetType().Name.Equals("WeedingQuest")))
          Game1.questOfTheDay.checkIfComplete((NPC) null, -1, -1, (Item) null, (string) null);
      }
      return object2;
    }

    public void setTileProperty(int tileX, int tileY, string layer, string key, string value)
    {
      try
      {
        if (!this.map.GetLayer(layer).Tiles[tileX, tileY].Properties.ContainsKey(key))
          this.map.GetLayer(layer).Tiles[tileX, tileY].Properties.Add(key, new PropertyValue(value));
        else
          this.map.GetLayer(layer).Tiles[tileX, tileY].Properties[key] = (PropertyValue) value;
      }
      catch (Exception ex)
      {
      }
    }

    private void removeDirt(Vector2 location)
    {
      Object @object;
      this.objects.TryGetValue(location, out @object);
      if (@object == null || !@object.IsHoeDirt)
        return;
      this.objects.Remove(location);
    }

    public void removeBatch(List<Vector2> locations)
    {
      foreach (Vector2 location in locations)
        this.objects.Remove(location);
    }

    public void setObjectAt(float x, float y, Object o)
    {
      Vector2 key = new Vector2(x, y);
      if (this.objects.ContainsKey(key))
        this.objects[key] = o;
      else
        this.objects.Add(key, o);
    }

    public virtual void cleanupBeforePlayerExit()
    {
      Game1.currentLightSources.Clear();
      if (this.critters != null)
        this.critters.Clear();
      for (int index = Game1.onScreenMenus.Count - 1; index >= 0; --index)
      {
        IClickableMenu onScreenMenu = Game1.onScreenMenus[index];
        if (onScreenMenu.destroy)
        {
          Game1.onScreenMenus.RemoveAt(index);
          if (onScreenMenu is IDisposable)
            (onScreenMenu as IDisposable).Dispose();
        }
      }
      if (Game1.currentSong != null && !Game1.currentSong.Name.ToLower().Contains(Game1.currentSeason) && (!Game1.currentSong.Name.Contains("ambient") && !Game1.currentSong.Name.Equals("rain")) && !Game1.eventUp && (Game1.locationAfterWarp == null || this.isOutdoors && Game1.locationAfterWarp.isOutdoors))
        Game1.changeMusicTrack("none");
      AmbientLocationSounds.onLocationLeave();
      if ((this.name.Equals("AnimalShop") || this.name.Equals("ScienceHouse")) && (Game1.currentSong != null && Game1.currentSong.Name.Equals("marnieShop")) && (Game1.locationAfterWarp == null || Game1.locationAfterWarp.IsOutdoors))
        Game1.changeMusicTrack("none");
      if (this.name.Equals("Saloon") && Game1.currentSong != null)
        Game1.changeMusicTrack("none");
      if ((this is LibraryMuseum || this is JojaMart) && Game1.currentSong != null)
        Game1.changeMusicTrack("none");
      if (this.name.Equals("Hospital") && Game1.currentSong != null && Game1.currentSong.Name.Equals("distantBanjo") && (Game1.locationAfterWarp == null || Game1.locationAfterWarp.IsOutdoors))
        Game1.changeMusicTrack("none");
      if (Game1.player.rightRing != null)
        Game1.player.rightRing.onLeaveLocation(Game1.player, this);
      if (Game1.player.leftRing != null)
        Game1.player.leftRing.onLeaveLocation(Game1.player, this);
      foreach (NPC character in this.characters)
      {
        if (character.uniqueSpriteActive)
        {
          character.sprite.Texture = Game1.content.Load<Texture2D>("Characters\\" + character.name);
          character.uniqueSpriteActive = false;
        }
        if (character.uniquePortraitActive)
        {
          character.Portrait = Game1.content.Load<Texture2D>("Portraits\\" + character.name);
          character.uniquePortraitActive = false;
        }
      }
      Game1.temporaryContent.Unload();
      Utility.CollectGarbage("", 0);
    }

    public static int getWeedForSeason(Random r, string season)
    {
      if (!(season == "spring"))
      {
        if (!(season == " summer"))
        {
          if (!(season == "fall"))
          {
            if (season == "winter")
              ;
            return 674;
          }
          if (r.NextDouble() < 0.33)
            return 786;
          return r.NextDouble() >= 0.5 ? 679 : 678;
        }
        if (r.NextDouble() < 0.33)
          return 785;
        return r.NextDouble() >= 0.5 ? 677 : 676;
      }
      if (r.NextDouble() < 0.33)
        return 784;
      return r.NextDouble() >= 0.5 ? 675 : 674;
    }

    public virtual bool answerDialogueAction(string questionAndAnswer, string[] questionParams)
    {
      if (questionAndAnswer == null)
        return false;
      if (questionAndAnswer.Equals("Mine_Return to level " + (object) Game1.player.deepestMineLevel))
        Game1.mine.enterMine(Game1.player, Game1.player.deepestMineLevel, false);
      // ISSUE: reference to a compiler-generated method
      uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(questionAndAnswer);
      if (stringHash <= 1823658541U)
      {
        if (stringHash <= 799649041U)
        {
          if (stringHash <= 455134809U)
          {
            if (stringHash <= 247303129U)
            {
              if (stringHash <= 134883726U)
              {
                if ((int) stringHash != 112637151)
                {
                  if ((int) stringHash != 134883726 || !(questionAndAnswer == "ExitMine_Yes"))
                    goto label_294;
                }
                else if (questionAndAnswer == "Mine_Enter")
                {
                  Game1.enterMine(false, 1, (string) null);
                  goto label_295;
                }
                else
                  goto label_294;
              }
              else if ((int) stringHash != 245425821)
              {
                if ((int) stringHash == 247303129 && questionAndAnswer == "divorce_Yes")
                {
                  if (Game1.player.Money >= 50000)
                  {
                    Game1.player.Money -= 50000;
                    Game1.player.divorceTonight = true;
                    Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:ManorHouse_DivorceBook_Filed"));
                    goto label_295;
                  }
                  else
                  {
                    Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\UI:NotEnoughMoney1"));
                    goto label_295;
                  }
                }
                else
                  goto label_294;
              }
              else if (questionAndAnswer == "Eat_No")
              {
                Game1.isEating = false;
                Game1.player.completelyStopAnimatingOrDoingAction();
                goto label_295;
              }
              else
                goto label_294;
            }
            else if (stringHash <= 348312305U)
            {
              if ((int) stringHash != 279582718)
              {
                if ((int) stringHash == 348312305 && questionAndAnswer == "Quest_Yes")
                {
                  Game1.questOfTheDay.dailyQuest = true;
                  Game1.questOfTheDay.accept();
                  Game1.currentBillboard = 0;
                  Game1.player.questLog.Add(Game1.questOfTheDay);
                  goto label_295;
                }
                else
                  goto label_294;
              }
              else if (questionAndAnswer == "taxvote_Against")
              {
                Game1.addMailForTomorrow("taxRejected", false, false);
                ++this.currentEvent.currentCommand;
                goto label_295;
              }
              else
                goto label_294;
            }
            else if ((int) stringHash != 365638282)
            {
              if ((int) stringHash == 455134809 && questionAndAnswer == "Marnie_Supplies")
              {
                Game1.activeClickableMenu = (IClickableMenu) new ShopMenu(Utility.getAnimalShopStock(), 0, "Marnie");
                goto label_295;
              }
              else
                goto label_294;
            }
            else if (questionAndAnswer == "ClearHouse_Yes")
            {
              Vector2 tileLocation = Game1.player.getTileLocation();
              foreach (Vector2 adjacentTilesOffset in Character.AdjacentTilesOffsets)
              {
                Vector2 key = tileLocation + adjacentTilesOffset;
                if (this.objects.ContainsKey(key))
                  this.objects.Remove(key);
              }
              goto label_295;
            }
            else
              goto label_294;
          }
          else if (stringHash <= 706840292U)
          {
            if (stringHash <= 602270239U)
            {
              if ((int) stringHash != 564255041)
              {
                if ((int) stringHash == 602270239 && questionAndAnswer == "Shipping_Yes")
                {
                  Game1.player.shipAll();
                  goto label_295;
                }
                else
                  goto label_294;
              }
              else if (questionAndAnswer == "BuyQiCoins_Yes")
              {
                if (Game1.player.Money >= 1000)
                {
                  Game1.player.Money -= 1000;
                  Game1.playSound("Pickup_Coin15");
                  Game1.player.clubCoins += 100;
                  goto label_295;
                }
                else
                {
                  Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:GameLocation.cs.8715"));
                  goto label_295;
                }
              }
              else
                goto label_294;
            }
            else if ((int) stringHash != 606529510)
            {
              if ((int) stringHash == 706840292 && questionAndAnswer == "Dungeon_Go")
              {
                Game1.enterMine(false, Game1.mine.mineLevel + 1, "Dungeon");
                goto label_295;
              }
              else
                goto label_294;
            }
            else if (questionAndAnswer == "taxvote_For")
            {
              Game1.shippingTax = true;
              Game1.addMailForTomorrow("taxPassed", false, false);
              ++this.currentEvent.currentCommand;
              goto label_295;
            }
            else
              goto label_294;
          }
          else if (stringHash <= 767799469U)
          {
            if ((int) stringHash != 761217684)
            {
              if ((int) stringHash == 767799469 && questionAndAnswer == "CalicoJackHS_Play")
              {
                if (Game1.player.clubCoins >= 1000)
                {
                  Game1.currentMinigame = (IMinigame) new CalicoJack(-1, true);
                  goto label_295;
                }
                else
                {
                  Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Club_CalicoJackHS_NotEnoughCoins"));
                  goto label_295;
                }
              }
              else
                goto label_294;
            }
            else if (questionAndAnswer == "Blacksmith_Upgrade")
            {
              Game1.activeClickableMenu = (IClickableMenu) new ShopMenu(Utility.getBlacksmithUpgradeStock(Game1.player), 0, "ClintUpgrade");
              goto label_295;
            }
            else
              goto label_294;
          }
          else if ((int) stringHash != 778541444)
          {
            if ((int) stringHash == 799649041 && questionAndAnswer == "ExitMine_Go")
            {
              Game1.enterMine(false, Game1.mine.mineLevel - 1, (string) null);
              goto label_295;
            }
            else
              goto label_294;
          }
          else if (questionAndAnswer == "carpenter_Shop")
          {
            Game1.player.forceCanMove();
            Game1.activeClickableMenu = (IClickableMenu) new ShopMenu(Utility.getCarpenterStock(), 0, "Robin");
            goto label_295;
          }
          else
            goto label_294;
        }
        else if (stringHash <= 1098628691U)
        {
          if (stringHash <= 970402917U)
          {
            if (stringHash <= 877311563U)
            {
              if ((int) stringHash != 830669632)
              {
                if ((int) stringHash == 877311563 && questionAndAnswer == "Smelt_Iron")
                {
                  Game1.player.IronPieces -= 10;
                  this.smeltBar(new Object(Vector2.Zero, 335, "Iron Bar", false, true, false, false), 120);
                  goto label_295;
                }
                else
                  goto label_294;
              }
              else if (questionAndAnswer == "mariner_Buy")
              {
                if (Game1.player.Money >= 5000)
                {
                  Game1.player.Money -= 5000;
                  Farmer player = Game1.player;
                  Object @object = new Object(460, 1, false, -1, 0);
                  int num = 1;
                  @object.specialItem = num != 0;
                  // ISSUE: variable of the null type
                  __Null local = null;
                  player.addItemByMenuIfNecessary((Item) @object, (ItemGrabMenu.behaviorOnItemSelect) local);
                  if (Game1.activeClickableMenu == null)
                  {
                    Game1.player.holdUpItemThenMessage((Item) new Object(460, 1, false, -1, 0), true);
                    goto label_295;
                  }
                  else
                    goto label_295;
                }
                else
                {
                  Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\UI:NotEnoughMoney1"));
                  goto label_295;
                }
              }
              else
                goto label_294;
            }
            else if ((int) stringHash != 956781401)
            {
              if ((int) stringHash == 970402917 && questionAndAnswer == "Minecart_Bus")
              {
                Game1.player.Halt();
                Game1.player.freezePause = 700;
                Game1.warpFarmer("BusStop", 4, 4, 2);
                goto label_295;
              }
              else
                goto label_294;
            }
            else if (questionAndAnswer == "Mine_Return")
            {
              Game1.enterMine(false, Game1.player.deepestMineLevel, (string) null);
              goto label_295;
            }
            else
              goto label_294;
          }
          else if (stringHash <= 1034734869U)
          {
            if ((int) stringHash != 993094382)
            {
              if ((int) stringHash == 1034734869 && questionAndAnswer == "Blacksmith_Process")
              {
                Game1.activeClickableMenu = (IClickableMenu) new GeodeMenu();
                goto label_295;
              }
              else
                goto label_294;
            }
            else if (questionAndAnswer == "Mine_Yes")
            {
              if (Game1.mine != null && Game1.mine.mineLevel > 120)
              {
                Game1.warpFarmer("SkullCave", 3, 4, 2);
                goto label_295;
              }
              else
              {
                Game1.warpFarmer("UndergroundMine", 16, 16, false);
                goto label_295;
              }
            }
            else
              goto label_294;
          }
          else if ((int) stringHash != 1091116656)
          {
            if ((int) stringHash == 1098628691 && questionAndAnswer == "BuyClubCoins_Yes")
            {
              if (Game1.player.Money >= 1000)
              {
                Game1.player.Money -= 1000;
                Game1.player.clubCoins += 10;
                goto label_295;
              }
              else
              {
                Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\UI:NotEnoughMoney1"));
                goto label_295;
              }
            }
            else
              goto label_294;
          }
          else if (questionAndAnswer == "Museum_Collect")
          {
            Game1.activeClickableMenu = (IClickableMenu) new ItemGrabMenu((this as LibraryMuseum).getRewardsForPlayer(Game1.player), false, true, (InventoryMenu.highlightThisItem) null, (ItemGrabMenu.behaviorOnItemSelect) null, "Rewards", new ItemGrabMenu.behaviorOnItemSelect((this as LibraryMuseum).collectedReward), false, false, false, false, false, 0, (Item) null, -1, (object) null);
            goto label_295;
          }
          else
            goto label_294;
        }
        else
        {
          if (stringHash <= 1446971618U)
          {
            if (stringHash <= 1299599336U)
            {
              if ((int) stringHash != 1157692071)
              {
                if ((int) stringHash == 1299599336 && questionAndAnswer == "evilShrineRightDeActivate_Yes")
                {
                  if (Game1.player.removeItemsFromInventory(203, 1))
                  {
                    List<TemporaryAnimatedSprite> temporarySprites1 = this.temporarySprites;
                    TemporaryAnimatedSprite temporaryAnimatedSprite1 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(536, 1945, 8, 8), new Vector2((float) (12 * Game1.tileSize + 3 * Game1.pixelZoom), (float) (6 * Game1.tileSize + Game1.pixelZoom)), false, 0.0f, Color.White);
                    temporaryAnimatedSprite1.interval = 50f;
                    temporaryAnimatedSprite1.totalNumberOfLoops = 99999;
                    temporaryAnimatedSprite1.animationLength = 7;
                    double num1 = (double) (6 * Game1.tileSize) / 10000.0 + 9.99999974737875E-05;
                    temporaryAnimatedSprite1.layerDepth = (float) num1;
                    double pixelZoom = (double) Game1.pixelZoom;
                    temporaryAnimatedSprite1.scale = (float) pixelZoom;
                    temporarySprites1.Add(temporaryAnimatedSprite1);
                    Game1.playSound("fireball");
                    for (int index = 0; index < 20; ++index)
                    {
                      List<TemporaryAnimatedSprite> temporarySprites2 = this.temporarySprites;
                      TemporaryAnimatedSprite temporaryAnimatedSprite2 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(372, 1956, 10, 10), new Vector2(12f, 6f) * (float) Game1.tileSize + new Vector2((float) Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize), (float) Game1.random.Next(Game1.tileSize / 4)), false, 1f / 500f, Color.DarkSlateBlue);
                      temporaryAnimatedSprite2.alpha = 0.75f;
                      Vector2 vector2_1 = new Vector2(0.0f, -0.5f);
                      temporaryAnimatedSprite2.motion = vector2_1;
                      Vector2 vector2_2 = new Vector2(-1f / 500f, 0.0f);
                      temporaryAnimatedSprite2.acceleration = vector2_2;
                      double num2 = 99999.0;
                      temporaryAnimatedSprite2.interval = (float) num2;
                      double num3 = (double) (6 * Game1.tileSize) / 10000.0 + (double) Game1.random.Next(100) / 10000.0;
                      temporaryAnimatedSprite2.layerDepth = (float) num3;
                      double num4 = (double) (Game1.pixelZoom * 3) / 4.0;
                      temporaryAnimatedSprite2.scale = (float) num4;
                      double num5 = 0.00999999977648258;
                      temporaryAnimatedSprite2.scaleChange = (float) num5;
                      double num6 = (double) Game1.random.Next(-5, 6) * 3.14159274101257 / 256.0;
                      temporaryAnimatedSprite2.rotationChange = (float) num6;
                      int num7 = index * 25;
                      temporaryAnimatedSprite2.delayBeforeAnimationStart = num7;
                      temporarySprites2.Add(temporaryAnimatedSprite2);
                    }
                    Game1.spawnMonstersAtNight = false;
                    goto label_295;
                  }
                  else
                  {
                    Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:WitchHut_NoOffering"));
                    goto label_295;
                  }
                }
                else
                  goto label_294;
              }
              else if (!(questionAndAnswer == "ClubCard_Yes."))
                goto label_294;
            }
            else if ((int) stringHash != 1326189771)
            {
              if ((int) stringHash == 1446971618 && questionAndAnswer == "Drum_Change")
              {
                Game1.drawItemNumberSelection("drumTone", -1);
                goto label_295;
              }
              else
                goto label_294;
            }
            else if (questionAndAnswer == "ClubSeller_I'll")
            {
              if (Game1.player.Money >= 1000000)
              {
                Game1.player.Money -= 1000000;
                Game1.exitActiveMenu();
                Game1.player.forceCanMove();
                Game1.player.addItemByMenuIfNecessaryElseHoldUp((Item) new Object(Vector2.Zero, (int) sbyte.MaxValue, false), (ItemGrabMenu.behaviorOnItemSelect) null);
                goto label_295;
              }
              else
              {
                Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Club_ClubSeller_NotEnoughMoney"));
                goto label_295;
              }
            }
            else
              goto label_294;
          }
          else if (stringHash <= 1766532371U)
          {
            if ((int) stringHash != 1712806090)
            {
              if ((int) stringHash != 1766532371 || !(questionAndAnswer == "ClubCard_That's"))
                goto label_294;
            }
            else if (questionAndAnswer == "ExitToTitle_Yes")
            {
              Game1.fadeScreenToBlack();
              Game1.exitToTitle = true;
              goto label_295;
            }
            else
              goto label_294;
          }
          else if ((int) stringHash != 1775651564)
          {
            if ((int) stringHash != 1815784496)
            {
              if ((int) stringHash == 1823658541 && questionAndAnswer == "evilShrineCenter_Yes")
              {
                if (Game1.player.Money >= 30000)
                {
                  Game1.player.Money -= 30000;
                  Game1.player.wipeExMemories();
                  List<TemporaryAnimatedSprite> temporarySprites1 = this.temporarySprites;
                  TemporaryAnimatedSprite temporaryAnimatedSprite1 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(536, 1945, 8, 8), new Vector2((float) (7 * Game1.tileSize + 5 * Game1.pixelZoom), (float) (5 * Game1.tileSize + 2 * Game1.pixelZoom)), false, 0.0f, Color.White);
                  temporaryAnimatedSprite1.interval = 50f;
                  temporaryAnimatedSprite1.totalNumberOfLoops = 99999;
                  temporaryAnimatedSprite1.animationLength = 7;
                  double num1 = (double) (6 * Game1.tileSize) / 10000.0 + 9.99999974737875E-05;
                  temporaryAnimatedSprite1.layerDepth = (float) num1;
                  double pixelZoom1 = (double) Game1.pixelZoom;
                  temporaryAnimatedSprite1.scale = (float) pixelZoom1;
                  temporarySprites1.Add(temporaryAnimatedSprite1);
                  Game1.playSound("fireball");
                  DelayedAction.playSoundAfterDelay("debuffHit", 500);
                  int num2 = 0;
                  Game1.player.faceDirection(2);
                  Game1.player.FarmerSprite.animateOnce(new FarmerSprite.AnimationFrame[2]
                  {
                    new FarmerSprite.AnimationFrame(94, 1500),
                    new FarmerSprite.AnimationFrame(0, 1)
                  });
                  Game1.player.freezePause = 1500;
                  Game1.player.jitterStrength = 1f;
                  for (int index = 0; index < 20; ++index)
                  {
                    List<TemporaryAnimatedSprite> temporarySprites2 = this.temporarySprites;
                    TemporaryAnimatedSprite temporaryAnimatedSprite2 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(372, 1956, 10, 10), new Vector2(7f, 5f) * (float) Game1.tileSize + new Vector2((float) Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize), (float) Game1.random.Next(Game1.tileSize / 4)), false, 1f / 500f, Color.SlateGray);
                    temporaryAnimatedSprite2.alpha = 0.75f;
                    Vector2 vector2_1 = new Vector2(0.0f, -0.5f);
                    temporaryAnimatedSprite2.motion = vector2_1;
                    Vector2 vector2_2 = new Vector2(-1f / 500f, 0.0f);
                    temporaryAnimatedSprite2.acceleration = vector2_2;
                    double num3 = 99999.0;
                    temporaryAnimatedSprite2.interval = (float) num3;
                    double num4 = (double) (5 * Game1.tileSize) / 10000.0 + (double) Game1.random.Next(100) / 10000.0;
                    temporaryAnimatedSprite2.layerDepth = (float) num4;
                    double num5 = (double) (Game1.pixelZoom * 3) / 4.0;
                    temporaryAnimatedSprite2.scale = (float) num5;
                    double num6 = 0.00999999977648258;
                    temporaryAnimatedSprite2.scaleChange = (float) num6;
                    double num7 = (double) Game1.random.Next(-5, 6) * 3.14159274101257 / 256.0;
                    temporaryAnimatedSprite2.rotationChange = (float) num7;
                    int num8 = index * 25;
                    temporaryAnimatedSprite2.delayBeforeAnimationStart = num8;
                    temporarySprites2.Add(temporaryAnimatedSprite2);
                  }
                  for (int index = 0; index < 16; ++index)
                  {
                    foreach (Vector2 vector2_1 in Utility.getBorderOfThisRectangle(Utility.getRectangleCenteredAt(new Vector2(7f, 5f), 2 + index * 2)))
                    {
                      if (num2 % 2 == 0)
                      {
                        List<TemporaryAnimatedSprite> temporarySprites2 = this.temporarySprites;
                        TemporaryAnimatedSprite temporaryAnimatedSprite2 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(692, 1853, 4, 4), 25f, 1, 16, vector2_1 * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2)), false, false);
                        temporaryAnimatedSprite2.layerDepth = 1f;
                        int num3 = index * 50;
                        temporaryAnimatedSprite2.delayBeforeAnimationStart = num3;
                        double pixelZoom2 = (double) Game1.pixelZoom;
                        temporaryAnimatedSprite2.scale = (float) pixelZoom2;
                        double num4 = 1.0;
                        temporaryAnimatedSprite2.scaleChange = (float) num4;
                        int maxValue1 = (int) byte.MaxValue;
                        Color toGreenLerpColor = Utility.getRedToGreenLerpColor(1f / (float) (index + 1));
                        int r1 = (int) toGreenLerpColor.R;
                        int r2 = maxValue1 - r1;
                        int maxValue2 = (int) byte.MaxValue;
                        toGreenLerpColor = Utility.getRedToGreenLerpColor(1f / (float) (index + 1));
                        int g1 = (int) toGreenLerpColor.G;
                        int g2 = maxValue2 - g1;
                        int maxValue3 = (int) byte.MaxValue;
                        toGreenLerpColor = Utility.getRedToGreenLerpColor(1f / (float) (index + 1));
                        int b1 = (int) toGreenLerpColor.B;
                        int b2 = maxValue3 - b1;
                        Color color = new Color(r2, g2, b2);
                        temporaryAnimatedSprite2.color = color;
                        Vector2 vector2_2 = new Vector2(-0.1f, 0.0f);
                        temporaryAnimatedSprite2.acceleration = vector2_2;
                        temporarySprites2.Add(temporaryAnimatedSprite2);
                      }
                      ++num2;
                    }
                  }
                  goto label_295;
                }
                else
                {
                  Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:WitchHut_NoOffering"));
                  goto label_295;
                }
              }
              else
                goto label_294;
            }
            else if (questionAndAnswer == "Mine_No")
            {
              Response[] answerChoices = new Response[2]
              {
                new Response("No", Game1.content.LoadString("Strings\\Lexicon:QuestionDialogue_No")),
                new Response("Yes", Game1.content.LoadString("Strings\\Lexicon:QuestionDialogue_Yes"))
              };
              this.createQuestionDialogue(Game1.parseText(Game1.content.LoadString("Strings\\Locations:Mines_ResetMine")), answerChoices, "ResetMine");
              goto label_295;
            }
            else
              goto label_294;
          }
          else if (questionAndAnswer == "MinecartGame_Progress")
          {
            Game1.currentMinigame = (IMinigame) new MineCart(5, 3);
            goto label_295;
          }
          else
            goto label_294;
          Game1.playSound("explosion");
          Game1.flashAlpha = 5f;
          this.characters.Remove(this.getCharacterFromName("Bouncer"));
          if (this.characters.Count > 0)
          {
            this.characters[0].faceDirection(1);
            this.characters[0].setNewDialogue(Game1.content.LoadString("Data\\ExtraDialogue:Sandy_PlayerClubMember"), false, false);
            this.characters[0].doEmote(16, true);
          }
          Game1.pauseThenMessage(500, Game1.content.LoadString("Strings\\Locations:Club_Bouncer_PlayerClubMember"), false);
          Game1.player.Halt();
          Game1.getCharacterFromName("Mister Qi", false).setNewDialogue(Game1.content.LoadString("Data\\ExtraDialogue:MisterQi_PlayerClubMember"), false, false);
          goto label_295;
        }
      }
      else if (stringHash <= 2908868305U)
      {
        if (stringHash <= 2433686860U)
        {
          if (stringHash <= 2009433326U)
          {
            if (stringHash <= 1874877668U)
            {
              if ((int) stringHash != 1843424219)
              {
                if ((int) stringHash == 1874877668 && questionAndAnswer == "Smelt_Copper")
                {
                  Game1.player.CopperPieces -= 10;
                  this.smeltBar(new Object(Vector2.Zero, 334, "Copper Bar", false, true, false, false), 60);
                  goto label_295;
                }
                else
                  goto label_294;
              }
              else if (questionAndAnswer == "Minecart_Mines")
              {
                Game1.player.Halt();
                Game1.player.freezePause = 700;
                Game1.warpFarmer("Mine", 13, 9, 1);
                goto label_295;
              }
              else
                goto label_294;
            }
            else if ((int) stringHash != 1970483540)
            {
              if ((int) stringHash != 2009433326 || !(questionAndAnswer == "ExitMine_Leave"))
                goto label_294;
            }
            else if (questionAndAnswer == "carpenter_Upgrade")
            {
              this.houseUpgradeOffer();
              goto label_295;
            }
            else
              goto label_294;
          }
          else if (stringHash <= 2250286071U)
          {
            if ((int) stringHash != 2136192597)
            {
              if ((int) stringHash == -2044681225 && questionAndAnswer == "divorceCancel_Yes")
              {
                if (Game1.player.divorceTonight)
                {
                  Game1.player.divorceTonight = false;
                  Game1.player.money += 50000;
                  Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:ManorHouse_DivorceBook_Cancelled"));
                  goto label_295;
                }
                else
                  goto label_295;
              }
              else
                goto label_294;
            }
            else if (questionAndAnswer == "evilShrineLeft_Yes")
            {
              if (Game1.player.removeItemsFromInventory(74, 1))
              {
                List<TemporaryAnimatedSprite> temporarySprites1 = this.temporarySprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite1 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(536, 1945, 8, 8), new Vector2((float) (2 * Game1.tileSize + 7 * Game1.pixelZoom), (float) (6 * Game1.tileSize + Game1.pixelZoom)), false, 0.0f, Color.White);
                temporaryAnimatedSprite1.interval = 50f;
                temporaryAnimatedSprite1.totalNumberOfLoops = 99999;
                temporaryAnimatedSprite1.animationLength = 7;
                double num1 = (double) (6 * Game1.tileSize) / 10000.0 + 9.99999974737875E-05;
                temporaryAnimatedSprite1.layerDepth = (float) num1;
                double pixelZoom = (double) Game1.pixelZoom;
                temporaryAnimatedSprite1.scale = (float) pixelZoom;
                temporarySprites1.Add(temporaryAnimatedSprite1);
                for (int index = 0; index < 20; ++index)
                {
                  List<TemporaryAnimatedSprite> temporarySprites2 = this.temporarySprites;
                  TemporaryAnimatedSprite temporaryAnimatedSprite2 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(372, 1956, 10, 10), new Vector2(2f, 6f) * (float) Game1.tileSize + new Vector2((float) Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize), (float) Game1.random.Next(Game1.tileSize / 4)), false, 1f / 500f, Color.LightGray);
                  temporaryAnimatedSprite2.alpha = 0.75f;
                  Vector2 vector2_1 = new Vector2(1f, -0.5f);
                  temporaryAnimatedSprite2.motion = vector2_1;
                  Vector2 vector2_2 = new Vector2(-1f / 500f, 0.0f);
                  temporaryAnimatedSprite2.acceleration = vector2_2;
                  double num2 = 99999.0;
                  temporaryAnimatedSprite2.interval = (float) num2;
                  double num3 = (double) (6 * Game1.tileSize) / 10000.0 + (double) Game1.random.Next(100) / 10000.0;
                  temporaryAnimatedSprite2.layerDepth = (float) num3;
                  double num4 = (double) (Game1.pixelZoom * 3) / 4.0;
                  temporaryAnimatedSprite2.scale = (float) num4;
                  double num5 = 0.00999999977648258;
                  temporaryAnimatedSprite2.scaleChange = (float) num5;
                  double num6 = (double) Game1.random.Next(-5, 6) * 3.14159274101257 / 256.0;
                  temporaryAnimatedSprite2.rotationChange = (float) num6;
                  int num7 = index * 25;
                  temporaryAnimatedSprite2.delayBeforeAnimationStart = num7;
                  temporarySprites2.Add(temporaryAnimatedSprite2);
                }
                Game1.playSound("fireball");
                this.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(388, 1894, 24, 22), 100f, 6, 9999, new Vector2(2f, 5f) * (float) Game1.tileSize, false, true, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
                {
                  motion = new Vector2(4f, -2f)
                });
                if (Game1.player.getChildren().Count<Child>() > 1)
                  this.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(388, 1894, 24, 22), 100f, 6, 9999, new Vector2(2f, 5f) * (float) Game1.tileSize, false, true, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
                  {
                    motion = new Vector2(4f, -1.5f),
                    delayBeforeAnimationStart = 50
                  });
                string message = "";
                foreach (NPC child in Game1.player.getChildren())
                  message += Game1.content.LoadString("Strings\\Locations:WitchHut_Goodbye", (object) child.getName());
                Game1.showGlobalMessage(message);
                Game1.player.getRidOfChildren();
                goto label_295;
              }
              else
              {
                Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:WitchHut_NoOffering"));
                goto label_295;
              }
            }
            else
              goto label_294;
          }
          else if ((int) stringHash != -1978044793)
          {
            if ((int) stringHash == -1861280436 && questionAndAnswer == "diary_...I")
            {
              Game1.player.friendships[(Game1.farmEvent as DiaryEvent).NPCname][5] = 1;
              goto label_295;
            }
            else
              goto label_294;
          }
          else if (questionAndAnswer == "Backpack_Yes")
          {
            this.tryToBuyNewBackpack();
            goto label_295;
          }
          else
            goto label_294;
        }
        else if (stringHash <= 2764458791U)
        {
          if (stringHash <= 2714077224U)
          {
            if ((int) stringHash != -1746580989)
            {
              if ((int) stringHash == -1580890072 && questionAndAnswer == "Smelt_Iridium")
              {
                Game1.player.IridiumPieces -= 10;
                this.smeltBar(new Object(Vector2.Zero, 337, "Iridium Bar", false, true, false, false), 1440);
                goto label_295;
              }
              else
                goto label_294;
            }
            else if (questionAndAnswer == "upgrade_Yes")
            {
              this.houseUpgradeAccept();
              goto label_295;
            }
            else
              goto label_294;
          }
          else if ((int) stringHash != -1546664032)
          {
            if ((int) stringHash == -1530508505 && questionAndAnswer == "Backpack_Purchase")
            {
              if (Game1.player.maxItems == 12 && Game1.player.Money >= 2000)
              {
                Game1.player.Money -= 2000;
                Game1.player.maxItems += 12;
                for (int index = 0; index < Game1.player.maxItems; ++index)
                {
                  if (Game1.player.items.Count <= index)
                    Game1.player.items.Add((Item) null);
                }
                Game1.player.holdUpItemThenMessage((Item) new SpecialItem(-1, 99, Game1.content.LoadString("Strings\\StringsFromCSFiles:GameLocation.cs.8708")), true);
                goto label_295;
              }
              else if (Game1.player.maxItems < 36 && Game1.player.Money >= 10000)
              {
                Game1.player.Money -= 10000;
                Game1.player.maxItems += 12;
                Game1.player.holdUpItemThenMessage((Item) new SpecialItem(-1, 99, Game1.content.LoadString("Strings\\StringsFromCSFiles:GameLocation.cs.8709")), true);
                for (int index = 0; index < Game1.player.maxItems; ++index)
                {
                  if (Game1.player.items.Count <= index)
                    Game1.player.items.Add((Item) null);
                }
                goto label_295;
              }
              else if (Game1.player.maxItems != 36)
              {
                Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\UI:NotEnoughMoney2"));
                goto label_295;
              }
              else
                goto label_295;
            }
            else
              goto label_294;
          }
          else if (questionAndAnswer == "Sleep_Yes")
          {
            if ((double) this.lightLevel == 0.0 && Game1.timeOfDay < 2000)
            {
              this.lightLevel = 0.6f;
              Game1.playSound("openBox");
              Game1.NewDay(600f);
            }
            else if ((double) this.lightLevel > 0.0 && Game1.timeOfDay >= 2000)
            {
              this.lightLevel = 0.0f;
              Game1.playSound("openBox");
              Game1.NewDay(600f);
            }
            else
              Game1.NewDay(0.0f);
            Game1.player.mostRecentBed = Game1.player.position;
            Game1.player.doEmote(24);
            Game1.player.freezePause = 2000;
            goto label_295;
          }
          else
            goto label_294;
        }
        else if (stringHash <= 2784483468U)
        {
          if ((int) stringHash != -1515449041)
          {
            if ((int) stringHash == -1510483828 && questionAndAnswer == "Shaft_Jump")
            {
              if (this is MineShaft)
              {
                (this as MineShaft).enterMineShaft();
                goto label_295;
              }
              else
                goto label_295;
            }
            else
              goto label_294;
          }
          else if (questionAndAnswer == "CalicoJack_Rules")
          {
            Game1.multipleDialogues(new string[2]
            {
              Game1.content.LoadString("Strings\\Locations:Club_CalicoJack_Rules1"),
              Game1.content.LoadString("Strings\\Locations:Club_CalicoJack_Rules2")
            });
            goto label_295;
          }
          else
            goto label_294;
        }
        else if ((int) stringHash != -1481696103)
        {
          if ((int) stringHash == -1386098991 && questionAndAnswer == "Minecart_Town")
          {
            Game1.player.Halt();
            Game1.player.freezePause = 700;
            Game1.warpFarmer("Town", 105, 80, 1);
            goto label_295;
          }
          else
            goto label_294;
        }
        else if (questionAndAnswer == "Eat_Yes")
        {
          Game1.isEating = false;
          Game1.eatHeldObject();
          goto label_295;
        }
        else
          goto label_294;
      }
      else if (stringHash <= 3389677340U)
      {
        if (stringHash <= 3048358753U)
        {
          if (stringHash <= 2994182275U)
          {
            if ((int) stringHash != -1311702434)
            {
              if ((int) stringHash == -1300785021 && questionAndAnswer == "Smelt_Gold")
              {
                Game1.player.GoldPieces -= 10;
                this.smeltBar(new Object(Vector2.Zero, 336, "Gold Bar", false, true, false, false), 300);
                goto label_295;
              }
              else
                goto label_294;
            }
            else if (questionAndAnswer == "Flute_Change")
            {
              Game1.drawItemNumberSelection("flutePitch", -1);
              goto label_295;
            }
            else
              goto label_294;
          }
          else if ((int) stringHash != -1261107073)
          {
            if ((int) stringHash == -1246608543 && questionAndAnswer == "jukebox_Yes")
            {
              Game1.drawItemNumberSelection("jukebox", -1);
              Game1.jukeboxPlaying = true;
              goto label_295;
            }
            else
              goto label_294;
          }
          else if (questionAndAnswer == "evilShrineRightActivate_Yes")
          {
            if (Game1.player.removeItemsFromInventory(203, 1))
            {
              List<TemporaryAnimatedSprite> temporarySprites1 = this.temporarySprites;
              TemporaryAnimatedSprite temporaryAnimatedSprite1 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(536, 1945, 8, 8), new Vector2((float) (12 * Game1.tileSize + 3 * Game1.pixelZoom), (float) (6 * Game1.tileSize + Game1.pixelZoom)), false, 0.0f, Color.White);
              temporaryAnimatedSprite1.interval = 50f;
              temporaryAnimatedSprite1.totalNumberOfLoops = 99999;
              temporaryAnimatedSprite1.animationLength = 7;
              double num1 = (double) (6 * Game1.tileSize) / 10000.0 + 9.99999974737875E-05;
              temporaryAnimatedSprite1.layerDepth = (float) num1;
              double pixelZoom = (double) Game1.pixelZoom;
              temporaryAnimatedSprite1.scale = (float) pixelZoom;
              temporarySprites1.Add(temporaryAnimatedSprite1);
              Game1.playSound("fireball");
              DelayedAction.playSoundAfterDelay("batScreech", 500);
              for (int index = 0; index < 20; ++index)
              {
                List<TemporaryAnimatedSprite> temporarySprites2 = this.temporarySprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite2 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(372, 1956, 10, 10), new Vector2(12f, 6f) * (float) Game1.tileSize + new Vector2((float) Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize), (float) Game1.random.Next(Game1.tileSize / 4)), false, 1f / 500f, Color.DarkSlateBlue);
                temporaryAnimatedSprite2.alpha = 0.75f;
                Vector2 vector2_1 = new Vector2(-0.1f, -0.5f);
                temporaryAnimatedSprite2.motion = vector2_1;
                Vector2 vector2_2 = new Vector2(-1f / 500f, 0.0f);
                temporaryAnimatedSprite2.acceleration = vector2_2;
                double num2 = 99999.0;
                temporaryAnimatedSprite2.interval = (float) num2;
                double num3 = (double) (6 * Game1.tileSize) / 10000.0 + (double) Game1.random.Next(100) / 10000.0;
                temporaryAnimatedSprite2.layerDepth = (float) num3;
                double num4 = (double) (Game1.pixelZoom * 3) / 4.0;
                temporaryAnimatedSprite2.scale = (float) num4;
                double num5 = 0.00999999977648258;
                temporaryAnimatedSprite2.scaleChange = (float) num5;
                double num6 = (double) Game1.random.Next(-5, 6) * 3.14159274101257 / 256.0;
                temporaryAnimatedSprite2.rotationChange = (float) num6;
                int num7 = index * 60;
                temporaryAnimatedSprite2.delayBeforeAnimationStart = num7;
                temporarySprites2.Add(temporaryAnimatedSprite2);
              }
              Game1.player.freezePause = 1501;
              for (int index = 0; index < 28; ++index)
                this.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(540, 347, 13, 13), 50f, 4, 9999, new Vector2(12f, 5f) * (float) Game1.tileSize, false, true, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
                {
                  delayBeforeAnimationStart = 500 + index * 25,
                  motion = new Vector2((float) (Game1.random.Next(1, 5) * (Game1.random.NextDouble() < 0.5 ? -1 : 1)), (float) (Game1.random.Next(1, 5) * (Game1.random.NextDouble() < 0.5 ? -1 : 1)))
                });
              Game1.spawnMonstersAtNight = true;
              goto label_295;
            }
            else
            {
              Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:WitchHut_NoOffering"));
              goto label_295;
            }
          }
          else
            goto label_294;
        }
        else if (stringHash <= 3125820853U)
        {
          if ((int) stringHash != -1242923351)
          {
            if ((int) stringHash == -1169146443 && questionAndAnswer == "Minecart_Quarry")
            {
              Game1.player.Halt();
              Game1.player.freezePause = 700;
              Game1.warpFarmer("Mountain", 124, 12, 2);
              goto label_295;
            }
            else
              goto label_294;
          }
          else if (questionAndAnswer == "MinecartGame_Endless")
          {
            Game1.currentMinigame = (IMinigame) new MineCart(5, 2);
            goto label_295;
          }
          else
            goto label_294;
        }
        else if ((int) stringHash != -962610348)
        {
          if ((int) stringHash == -905289956 && questionAndAnswer == "RemoveIncubatingEgg_Yes")
          {
            Game1.player.ActiveObject = new Object(Vector2.Zero, (this as AnimalHouse).incubatingEgg.Y, (string) null, false, true, false, false);
            Game1.player.showCarrying();
            (this as AnimalHouse).incubatingEgg.Y = -1;
            this.map.GetLayer("Front").Tiles[1, 2].TileIndex = 45;
            goto label_295;
          }
          else
            goto label_294;
        }
        else if (questionAndAnswer == "buyJojaCola_Yes")
        {
          if (Game1.player.Money >= 75)
          {
            Game1.player.Money -= 75;
            Game1.player.addItemByMenuIfNecessary((Item) new Object(167, 1, false, -1, 0), (ItemGrabMenu.behaviorOnItemSelect) null);
            goto label_295;
          }
          else
          {
            Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\UI:NotEnoughMoney1"));
            goto label_295;
          }
        }
        else
          goto label_294;
      }
      else if (stringHash <= 3654804704U)
      {
        if (stringHash <= 3491527061U)
        {
          if ((int) stringHash != -860715943)
          {
            if ((int) stringHash == -803440235 && questionAndAnswer == "Museum_Donate")
            {
              Game1.activeClickableMenu = (IClickableMenu) new MuseumMenu();
              goto label_295;
            }
            else
              goto label_294;
          }
          else if (questionAndAnswer == "WizardShrine_Yes")
          {
            if (Game1.player.Money >= 500)
            {
              List<int> shirtOptions = new List<int>();
              shirtOptions.Add(0);
              shirtOptions.Add(1);
              shirtOptions.Add(2);
              shirtOptions.Add(3);
              shirtOptions.Add(4);
              shirtOptions.Add(5);
              List<int> hairStyleOptions = new List<int>();
              hairStyleOptions.Add(0);
              hairStyleOptions.Add(1);
              hairStyleOptions.Add(2);
              hairStyleOptions.Add(3);
              hairStyleOptions.Add(4);
              hairStyleOptions.Add(5);
              List<int> accessoryOptions = new List<int>();
              accessoryOptions.Add(0);
              accessoryOptions.Add(1);
              accessoryOptions.Add(2);
              accessoryOptions.Add(3);
              accessoryOptions.Add(4);
              accessoryOptions.Add(5);
              int num = 1;
              Game1.activeClickableMenu = (IClickableMenu) new CharacterCustomization(shirtOptions, hairStyleOptions, accessoryOptions, num != 0);
              Game1.player.Money -= 500;
              goto label_295;
            }
            else
            {
              Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\UI:NotEnoughMoney2"));
              goto label_295;
            }
          }
          else
            goto label_294;
        }
        else if ((int) stringHash != -795145835)
        {
          if ((int) stringHash == -640162592 && questionAndAnswer == "Mariner_Buy")
          {
            if (Game1.player.Money >= 5000)
            {
              Game1.player.Money -= 5000;
              Game1.player.grabObject(new Object(Vector2.Zero, 460, (string) null, false, true, false, false));
              return true;
            }
            Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\UI:NotEnoughMoney1"));
            goto label_295;
          }
          else
            goto label_294;
        }
        else if (questionAndAnswer == "Marnie_Purchase")
        {
          Game1.player.forceCanMove();
          Game1.activeClickableMenu = (IClickableMenu) new PurchaseAnimalsMenu(Utility.getPurchaseAnimalStock());
          goto label_295;
        }
        else
          goto label_294;
      }
      else if (stringHash <= 3694948413U)
      {
        if ((int) stringHash != -610169867)
        {
          if ((int) stringHash == -600018883 && questionAndAnswer == "carpenter_Construct")
          {
            Game1.activeClickableMenu = (IClickableMenu) new CarpenterMenu(false);
            goto label_295;
          }
          else
            goto label_294;
        }
        else if (questionAndAnswer == "Quest_No")
        {
          Game1.currentBillboard = 0;
          goto label_295;
        }
        else
          goto label_294;
      }
      else if ((int) stringHash != -348975462)
      {
        if ((int) stringHash != -137711420)
        {
          if ((int) stringHash == -62266474 && questionAndAnswer == "Bouquet_Yes")
          {
            if (Game1.player.Money >= 500)
            {
              if (Game1.player.ActiveObject == null)
              {
                Game1.player.Money -= 500;
                Game1.player.grabObject(new Object(Vector2.Zero, 458, (string) null, false, true, false, false));
                return true;
              }
              goto label_295;
            }
            else
            {
              Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\UI:NotEnoughMoney1"));
              goto label_295;
            }
          }
          else
            goto label_294;
        }
        else if (questionAndAnswer == "Blacksmith_Shop")
        {
          Game1.activeClickableMenu = (IClickableMenu) new ShopMenu(Utility.getBlacksmithStock(), 0, "Clint");
          goto label_295;
        }
        else
          goto label_294;
      }
      else if (questionAndAnswer == "CalicoJack_Play")
      {
        if (Game1.player.clubCoins >= 100)
        {
          Game1.currentMinigame = (IMinigame) new CalicoJack(-1, false);
          goto label_295;
        }
        else
        {
          Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Club_CalicoJack_NotEnoughCoins"));
          goto label_295;
        }
      }
      else
        goto label_294;
      if (Game1.mine != null && Game1.mine.mineLevel > 120)
        Game1.warpFarmer("SkullCave", 3, 4, 2);
      else
        Game1.warpFarmer("Mine", 23, 8, false);
      Game1.changeMusicTrack("none");
      goto label_295;
label_294:
      return false;
label_295:
      return true;
    }

    public virtual bool answerDialogue(Response answer)
    {
      string[] strArray;
      if (this.lastQuestionKey == null)
        strArray = (string[]) null;
      else
        strArray = this.lastQuestionKey.Split(' ');
      string[] questionParams = strArray;
      string questionAndAnswer = questionParams != null ? questionParams[0] + "_" + answer.responseKey : (string) null;
      if (answer.responseKey.Equals("Move"))
      {
        Game1.player.grabObject(this.actionObjectForQuestionDialogue);
        this.removeObject(this.actionObjectForQuestionDialogue.TileLocation, false);
        this.actionObjectForQuestionDialogue = (Object) null;
        return true;
      }
      if (this.afterQuestion != null)
      {
        this.afterQuestion(Game1.player, answer.responseKey);
        this.afterQuestion = (GameLocation.afterQuestionBehavior) null;
        Game1.objectDialoguePortraitPerson = (NPC) null;
        return true;
      }
      if (questionAndAnswer == null)
        return false;
      return this.answerDialogueAction(questionAndAnswer, questionParams);
    }

    public void setObject(Vector2 v, Object o)
    {
      if (this.objects.ContainsKey(v))
        this.objects[v] = o;
      else
        this.objects.Add(v, o);
    }

    private void houseUpgradeOffer()
    {
      switch (Game1.player.houseUpgradeLevel)
      {
        case 0:
          this.createQuestionDialogue(Game1.parseText(Game1.content.LoadString("Strings\\Locations:ScienceHouse_Carpenter_UpgradeHouse1")), this.createYesNoResponses(), "upgrade");
          break;
        case 1:
          this.createQuestionDialogue(Game1.parseText(Game1.content.LoadString("Strings\\Locations:ScienceHouse_Carpenter_UpgradeHouse2")), this.createYesNoResponses(), "upgrade");
          break;
        case 2:
          this.createQuestionDialogue(Game1.parseText(Game1.content.LoadString("Strings\\Locations:ScienceHouse_Carpenter_UpgradeHouse3")), this.createYesNoResponses(), "upgrade");
          break;
      }
    }

    private void houseUpgradeAccept()
    {
      switch (Game1.player.houseUpgradeLevel)
      {
        case 0:
          if (Game1.player.Money >= 10000 && Game1.player.hasItemInInventory(388, 450, 0))
          {
            Game1.player.daysUntilHouseUpgrade = 3;
            Game1.player.Money -= 10000;
            Game1.player.removeItemsFromInventory(388, 450);
            Game1.getCharacterFromName("Robin", false).setNewDialogue(Game1.content.LoadString("Data\\ExtraDialogue:Robin_HouseUpgrade_Accepted"), false, false);
            Game1.drawDialogue(Game1.getCharacterFromName("Robin", false));
            break;
          }
          if (Game1.player.Money < 10000)
          {
            Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\UI:NotEnoughMoney3"));
            break;
          }
          Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:ScienceHouse_Carpenter_NotEnoughWood1"));
          break;
        case 1:
          if (Game1.player.Money >= 50000 && Game1.player.hasItemInInventory(709, 150, 0))
          {
            Game1.player.daysUntilHouseUpgrade = 3;
            Game1.player.Money -= 50000;
            Game1.player.removeItemsFromInventory(709, 150);
            Game1.getCharacterFromName("Robin", false).setNewDialogue(Game1.content.LoadString("Data\\ExtraDialogue:Robin_HouseUpgrade_Accepted"), false, false);
            Game1.drawDialogue(Game1.getCharacterFromName("Robin", false));
            break;
          }
          if (Game1.player.Money < 50000)
          {
            Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\UI:NotEnoughMoney3"));
            break;
          }
          Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:ScienceHouse_Carpenter_NotEnoughWood2"));
          break;
        case 2:
          if (Game1.player.Money >= 100000)
          {
            Game1.player.daysUntilHouseUpgrade = 3;
            Game1.player.Money -= 100000;
            Game1.getCharacterFromName("Robin", false).setNewDialogue(Game1.content.LoadString("Data\\ExtraDialogue:Robin_HouseUpgrade_Accepted"), false, false);
            Game1.drawDialogue(Game1.getCharacterFromName("Robin", false));
            break;
          }
          if (Game1.player.Money >= 100000)
            break;
          Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\UI:NotEnoughMoney3"));
          break;
      }
    }

    private void smeltBar(Object bar, int minutesUntilReady)
    {
      --Game1.player.CoalPieces;
      this.actionObjectForQuestionDialogue.heldObject = bar;
      this.actionObjectForQuestionDialogue.minutesUntilReady = minutesUntilReady;
      this.actionObjectForQuestionDialogue.showNextIndex = true;
      this.actionObjectForQuestionDialogue = (Object) null;
      Game1.playSound("openBox");
      Game1.playSound("furnace");
      ++Game1.stats.BarsSmelted;
    }

    public void tryToBuyNewBackpack()
    {
      int num = 0;
      switch (Game1.player.MaxItems)
      {
        case 4:
          num = 3500;
          break;
        case 9:
          num = 7500;
          break;
        case 14:
          num = 15000;
          break;
      }
      if (Game1.player.Money >= num)
      {
        Game1.player.increaseBackpackSize(5);
        Game1.player.Money -= num;
        Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\UI:Backpack_Bought", (object) Game1.player.MaxItems));
        this.checkForMapChanges();
      }
      else
        Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\UI:NotEnoughMoney1"));
    }

    public void checkForMapChanges()
    {
      if (!this.name.Equals("SeedShop") || Game1.player.MaxItems != 19)
        return;
      this.map.GetLayer("Front").Tiles[10, 21] = (Tile) new StaticTile(this.map.GetLayer("Front"), this.map.GetTileSheet("TownHouseIndoors"), BlendMode.Alpha, 203);
    }

    public void removeStumpOrBoulder(int tileX, int tileY, Object o)
    {
      List<Vector2> locations = new List<Vector2>();
      string name = o.Name;
      // ISSUE: reference to a compiler-generated method
      uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(name);
      if (stringHash <= 3002255887U)
      {
        if (stringHash <= 1478161722U)
        {
          if ((int) stringHash != 1443354848)
          {
            if ((int) stringHash != 1478161722 || !(name == "Stump3/4"))
              goto label_19;
          }
          else if (!(name == "Boulder3/4"))
            goto label_19;
          locations.Add(new Vector2((float) tileX, (float) tileY));
          locations.Add(new Vector2((float) (tileX + 1), (float) tileY));
          locations.Add(new Vector2((float) tileX, (float) (tileY - 1)));
          locations.Add(new Vector2((float) (tileX + 1), (float) (tileY - 1)));
        }
        else
        {
          if ((int) stringHash != 2100107977)
          {
            if ((int) stringHash != -1292711409 || !(name == "Stump4/4"))
              goto label_19;
          }
          else if (!(name == "Boulder4/4"))
            goto label_19;
          locations.Add(new Vector2((float) tileX, (float) tileY));
          locations.Add(new Vector2((float) (tileX - 1), (float) tileY));
          locations.Add(new Vector2((float) tileX, (float) (tileY - 1)));
          locations.Add(new Vector2((float) (tileX - 1), (float) (tileY - 1)));
        }
      }
      else
      {
        if (stringHash <= 3696585573U)
        {
          if ((int) stringHash != -1269408830)
          {
            if ((int) stringHash != -598381723 || !(name == "Stump2/4"))
              goto label_19;
            else
              goto label_16;
          }
          else if (!(name == "Boulder1/4"))
            goto label_19;
        }
        else if ((int) stringHash != -98819016)
        {
          if ((int) stringHash != -80739317 || !(name == "Boulder2/4"))
            goto label_19;
          else
            goto label_16;
        }
        else if (!(name == "Stump1/4"))
          goto label_19;
        locations.Add(new Vector2((float) tileX, (float) tileY));
        locations.Add(new Vector2((float) (tileX + 1), (float) tileY));
        locations.Add(new Vector2((float) tileX, (float) (tileY + 1)));
        locations.Add(new Vector2((float) (tileX + 1), (float) (tileY + 1)));
        goto label_19;
label_16:
        locations.Add(new Vector2((float) tileX, (float) tileY));
        locations.Add(new Vector2((float) (tileX - 1), (float) tileY));
        locations.Add(new Vector2((float) tileX, (float) (tileY + 1)));
        locations.Add(new Vector2((float) (tileX - 1), (float) (tileY + 1)));
      }
label_19:
      int initialParentTileIndex = o.Name.Contains("Stump") ? 5 : 3;
      if (Game1.currentSeason.Equals("winter"))
        initialParentTileIndex += 376;
      for (int index = 0; index < locations.Count; ++index)
        this.TemporarySprites.Add(new TemporaryAnimatedSprite(initialParentTileIndex, (float) Game1.random.Next(150, 400), 1, 3, new Vector2(locations[index].X * (float) Game1.tileSize, locations[index].Y * (float) Game1.tileSize), true, o.flipped));
      this.removeBatch(locations);
    }

    public void destroyObject(Vector2 tileLocation, Farmer who)
    {
      this.destroyObject(tileLocation, false, who);
    }

    public void destroyObject(Vector2 tileLocation, bool hardDestroy, Farmer who)
    {
      if (!this.objects.ContainsKey(tileLocation) || this.objects[tileLocation].IsHoeDirt || (this.objects[tileLocation].fragility == 2 || this.objects[tileLocation] is Chest))
        return;
      Object o = this.objects[tileLocation];
      bool flag = false;
      if (o.Type != null && (o.Type.Equals("Fish") || o.Type.Equals("Cooking") || o.Type.Equals("Crafting")))
      {
        this.temporarySprites.Add(new TemporaryAnimatedSprite(o.ParentSheetIndex, 150f, 1, 3, new Vector2(tileLocation.X * (float) Game1.tileSize, tileLocation.Y * (float) Game1.tileSize), true, o.bigCraftable, o.flipped));
        flag = true;
      }
      else if (o.Name.Contains("Stump") || o.Name.Contains("Boulder"))
      {
        flag = true;
        this.removeStumpOrBoulder((int) tileLocation.X, (int) tileLocation.Y, o);
      }
      else if (o.CanBeGrabbed | hardDestroy)
        flag = true;
      if (this.Name.Equals("UndergroundMine") && !o.Name.Contains("Lumber"))
      {
        flag = true;
        Game1.mine.checkStoneForItems(o.ParentSheetIndex, (int) tileLocation.X, (int) tileLocation.Y, who);
      }
      if (flag)
        this.objects.Remove(tileLocation);
      if (!this.name.Equals("Town") || !o.Name.Contains("Weed") || (Game1.questOfTheDay == null || !Game1.questOfTheDay.accepted) || (Game1.questOfTheDay.completed || !Game1.questOfTheDay.GetType().Name.Equals("WeedingQuest")))
        return;
      Game1.questOfTheDay.checkIfComplete((NPC) null, -1, -1, (Item) null, (string) null);
    }

    public string doesTileHaveProperty(int xTile, int yTile, string propertyName, string layerName)
    {
      PropertyValue propertyValue = (PropertyValue) null;
      if (this.map.GetLayer(layerName) != null)
      {
        Tile tile = this.map.GetLayer(layerName).PickTile(new Location(xTile * Game1.tileSize, yTile * Game1.tileSize), Game1.viewport.Size);
        if (tile != null)
          tile.TileIndexProperties.TryGetValue(propertyName, out propertyValue);
        if (propertyValue == null && tile != null)
          this.map.GetLayer(layerName).PickTile(new Location(xTile * Game1.tileSize, yTile * Game1.tileSize), Game1.viewport.Size).Properties.TryGetValue(propertyName, out propertyValue);
      }
      if (propertyValue != null)
        return propertyValue.ToString();
      return (string) null;
    }

    public string doesTileHavePropertyNoNull(int xTile, int yTile, string propertyName, string layerName)
    {
      PropertyValue propertyValue1 = (PropertyValue) null;
      PropertyValue propertyValue2 = (PropertyValue) null;
      if (this.map.GetLayer(layerName) != null)
      {
        Tile tile = this.map.GetLayer(layerName).PickTile(new Location(xTile * Game1.tileSize, yTile * Game1.tileSize), Game1.viewport.Size);
        if (tile != null)
          tile.TileIndexProperties.TryGetValue(propertyName, out propertyValue1);
        if (tile != null)
          this.map.GetLayer(layerName).PickTile(new Location(xTile * Game1.tileSize, yTile * Game1.tileSize), Game1.viewport.Size).Properties.TryGetValue(propertyName, out propertyValue2);
        if (propertyValue2 != null)
          propertyValue1 = propertyValue2;
      }
      if (propertyValue1 != null)
        return propertyValue1.ToString();
      return "";
    }

    public bool isOpenWater(int xTile, int yTile)
    {
      if (this.doesTileHaveProperty(xTile, yTile, "Water", "Back") != null && this.doesTileHaveProperty(xTile, yTile, "Passable", "Buildings") == null)
        return !this.objects.ContainsKey(new Vector2((float) xTile, (float) yTile));
      return false;
    }

    public bool isCropAtTile(int tileX, int tileY)
    {
      Vector2 key = new Vector2((float) tileX, (float) tileY);
      if (this.terrainFeatures.ContainsKey(key) && this.terrainFeatures[key].GetType() == typeof (HoeDirt))
        return ((HoeDirt) this.terrainFeatures[key]).crop != null;
      return false;
    }

    public virtual bool dropObject(Object obj, Vector2 dropLocation, xTile.Dimensions.Rectangle viewport, bool initialPlacement, Farmer who = null)
    {
      obj.isSpawnedObject = true;
      Vector2 vector2 = new Vector2((float) ((int) dropLocation.X / Game1.tileSize), (float) ((int) dropLocation.Y / Game1.tileSize));
      if (!this.isTileOnMap(vector2) || this.map.GetLayer("Back").PickTile(new Location((int) dropLocation.X, (int) dropLocation.Y), Game1.viewport.Size) == null || this.map.GetLayer("Back").Tiles[(int) vector2.X, (int) vector2.Y].TileIndexProperties.ContainsKey("Unplaceable"))
        return false;
      if (obj.bigCraftable)
      {
        obj.tileLocation = vector2;
        if (!this.isFarm || !obj.setOutdoors && this.isOutdoors || (!obj.setIndoors && !this.isOutdoors || obj.performDropDownAction(who)))
          return false;
      }
      else if (obj.Type != null && obj.Type.Equals("Crafting") && obj.performDropDownAction(who))
        obj.CanBeSetDown = false;
      bool flag = this.isTilePassable(new Location((int) vector2.X, (int) vector2.Y), viewport) && !this.isTileOccupiedForPlacement(vector2, (Object) null);
      if ((obj.CanBeSetDown | initialPlacement) & flag && !this.isTileHoeDirt(vector2))
      {
        obj.TileLocation = vector2;
        if (this.objects.ContainsKey(vector2))
          return false;
        this.objects.Add(vector2, obj);
      }
      else if (this.doesTileHaveProperty((int) vector2.X, (int) vector2.Y, "Water", "Back") != null)
      {
        this.temporarySprites.Add(new TemporaryAnimatedSprite(28, 300f, 2, 1, dropLocation, false, obj.flipped));
        Game1.playSound("dropItemInWater");
      }
      else
      {
        if (obj.CanBeSetDown && !flag)
          return false;
        if (obj.ParentSheetIndex >= 0 && obj.Type != null)
        {
          if (obj.Type.Equals("Fish") || obj.Type.Equals("Cooking") || obj.Type.Equals("Crafting"))
            this.temporarySprites.Add(new TemporaryAnimatedSprite(obj.ParentSheetIndex, 150f, 1, 3, dropLocation, true, obj.flipped));
          else
            this.temporarySprites.Add(new TemporaryAnimatedSprite(obj.ParentSheetIndex + 1, 150f, 1, 3, dropLocation, true, obj.flipped));
        }
      }
      return true;
    }

    public void detectTreasures(double successRate, int radius, Vector2 tileLocation)
    {
      bool flag = false;
      Vector2 tileLocation1 = new Vector2(Math.Min((float) (this.map.Layers[0].LayerWidth - 1), Math.Max(0.0f, tileLocation.X - (float) radius)), Math.Min((float) (this.map.Layers[0].LayerHeight - 1), Math.Max(0.0f, tileLocation.Y - (float) radius)));
      bool[,] circleOutlineGrid = Game1.getCircleOutlineGrid(radius);
      for (int index1 = 0; index1 < radius * 2 + 1; ++index1)
      {
        for (int index2 = 0; index2 < radius * 2 + 1; ++index2)
        {
          if (index1 == 0 || index2 == 0 || (index1 == radius * 2 || index2 == radius * 2))
            flag = circleOutlineGrid[index1, index2];
          else if (circleOutlineGrid[index1, index2])
          {
            flag = !flag;
            if (!flag)
            {
              if (!this.isTileOccupied(tileLocation1, "") && this.isTilePassable(new Location((int) tileLocation1.X, (int) tileLocation1.Y), Game1.viewport) && (this.doesTileHaveProperty((int) tileLocation1.X, (int) tileLocation1.Y, "Diggable", "Back") != null && Game1.random.NextDouble() < successRate))
                this.checkForBuriedItem((int) tileLocation1.X, (int) tileLocation1.Y, false, true);
              if (Game1.random.NextDouble() < 0.2)
                this.temporarySprites.Add(new TemporaryAnimatedSprite(352, (float) Game1.random.Next(100, 150), 2, 1, new Vector2(tileLocation1.X * (float) Game1.tileSize, tileLocation1.Y * (float) Game1.tileSize), false, false));
            }
          }
          if (flag)
          {
            if (!this.isTileOccupied(tileLocation1, "") && this.isTilePassable(new Location((int) tileLocation1.X, (int) tileLocation1.Y), Game1.viewport) && (this.doesTileHaveProperty((int) tileLocation1.X, (int) tileLocation1.Y, "Diggable", "Back") != null && Game1.random.NextDouble() < successRate))
              this.checkForBuriedItem((int) tileLocation1.X, (int) tileLocation1.Y, false, true);
            if (Game1.random.NextDouble() < 0.2)
              this.temporarySprites.Add(new TemporaryAnimatedSprite(352, (float) Game1.random.Next(100, 150), 2, 1, new Vector2(tileLocation1.X * (float) Game1.tileSize, tileLocation1.Y * (float) Game1.tileSize), false, false));
          }
          ++tileLocation1.Y;
          tileLocation1.Y = Math.Min((float) (this.map.Layers[0].LayerHeight - 1), Math.Max(0.0f, tileLocation1.Y));
        }
        ++tileLocation1.X;
        tileLocation1.Y = Math.Min((float) (this.map.Layers[0].LayerWidth - 1), Math.Max(0.0f, tileLocation1.X));
        tileLocation1.Y = tileLocation.Y - (float) radius;
        tileLocation1.Y = Math.Min((float) (this.map.Layers[0].LayerHeight - 1), Math.Max(0.0f, tileLocation1.Y));
      }
    }

    public void explode(Vector2 tileLocation, int radius, Farmer who)
    {
      bool flag = false;
      Vector2 index1 = new Vector2(Math.Min((float) (this.map.Layers[0].LayerWidth - 1), Math.Max(0.0f, tileLocation.X - (float) radius)), Math.Min((float) (this.map.Layers[0].LayerHeight - 1), Math.Max(0.0f, tileLocation.Y - (float) radius)));
      bool[,] circleOutlineGrid1 = Game1.getCircleOutlineGrid(radius);
      Microsoft.Xna.Framework.Rectangle areaOfEffect = new Microsoft.Xna.Framework.Rectangle((int) ((double) tileLocation.X - (double) radius - 1.0) * Game1.tileSize, (int) ((double) tileLocation.Y - (double) radius - 1.0) * Game1.tileSize, (radius * 2 + 1) * Game1.tileSize, (radius * 2 + 1) * Game1.tileSize);
      this.damageMonster(areaOfEffect, radius * 6, radius * 8, true, who);
      List<TemporaryAnimatedSprite> temporarySprites = this.temporarySprites;
      TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(23, 9999f, 6, 1, new Vector2(index1.X * (float) Game1.tileSize, index1.Y * (float) Game1.tileSize), false, Game1.random.NextDouble() < 0.5);
      temporaryAnimatedSprite.light = true;
      double num1 = (double) radius;
      temporaryAnimatedSprite.lightRadius = (float) num1;
      Color black = Color.Black;
      temporaryAnimatedSprite.lightcolor = black;
      double num2 = 0.0299999993294477 - (double) radius * (3.0 / 1000.0);
      temporaryAnimatedSprite.alphaFade = (float) num2;
      temporarySprites.Add(temporaryAnimatedSprite);
      Rumble.rumbleAndFade(1f, (float) (300 + radius * 100));
      Microsoft.Xna.Framework.Rectangle boundingBox = Game1.player.GetBoundingBox();
      if (boundingBox.Intersects(areaOfEffect))
        Game1.farmerTakeDamage(radius * 3, true, (Monster) null);
      for (int index2 = this.terrainFeatures.Count - 1; index2 >= 0; --index2)
      {
        KeyValuePair<Vector2, TerrainFeature> keyValuePair = this.terrainFeatures.ElementAt<KeyValuePair<Vector2, TerrainFeature>>(index2);
        boundingBox = keyValuePair.Value.getBoundingBox(keyValuePair.Key);
        if (boundingBox.Intersects(areaOfEffect) && keyValuePair.Value.performToolAction((Tool) null, radius / 2, keyValuePair.Key, (GameLocation) null))
          this.terrainFeatures.Remove(keyValuePair.Key);
      }
      for (int index2 = 0; index2 < radius * 2 + 1; ++index2)
      {
        for (int index3 = 0; index3 < radius * 2 + 1; ++index3)
        {
          if (index2 == 0 || index3 == 0 || (index2 == radius * 2 || index3 == radius * 2))
            flag = circleOutlineGrid1[index2, index3];
          else if (circleOutlineGrid1[index2, index3])
          {
            flag = !flag;
            if (!flag)
            {
              if (this.objects.ContainsKey(index1) && this.objects[index1].onExplosion(who, this))
                this.destroyObject(index1, who);
              if (Game1.random.NextDouble() < 0.45)
              {
                if (Game1.random.NextDouble() < 0.5)
                  this.temporarySprites.Add(new TemporaryAnimatedSprite(362, (float) Game1.random.Next(30, 90), 6, 1, new Vector2(index1.X * (float) Game1.tileSize, index1.Y * (float) Game1.tileSize), false, Game1.random.NextDouble() < 0.5)
                  {
                    delayBeforeAnimationStart = Game1.random.Next(700)
                  });
                else
                  this.temporarySprites.Add(new TemporaryAnimatedSprite(5, new Vector2(index1.X * (float) Game1.tileSize, index1.Y * (float) Game1.tileSize), Color.White, 8, false, 50f, 0, -1, -1f, -1, 0)
                  {
                    delayBeforeAnimationStart = Game1.random.Next(200),
                    scale = (float) ((double) Game1.random.Next(5, 15) / 10.0)
                  });
              }
            }
          }
          if (flag)
          {
            if (this.objects.ContainsKey(index1) && this.objects[index1].onExplosion(who, this))
              this.destroyObject(index1, who);
            if (Game1.random.NextDouble() < 0.45)
            {
              if (Game1.random.NextDouble() < 0.5)
                this.temporarySprites.Add(new TemporaryAnimatedSprite(362, (float) Game1.random.Next(30, 90), 6, 1, new Vector2(index1.X * (float) Game1.tileSize, index1.Y * (float) Game1.tileSize), false, Game1.random.NextDouble() < 0.5)
                {
                  delayBeforeAnimationStart = Game1.random.Next(700)
                });
              else
                this.temporarySprites.Add(new TemporaryAnimatedSprite(5, new Vector2(index1.X * (float) Game1.tileSize, index1.Y * (float) Game1.tileSize), Color.White, 8, false, 50f, 0, -1, -1f, -1, 0)
                {
                  delayBeforeAnimationStart = Game1.random.Next(200),
                  scale = (float) ((double) Game1.random.Next(5, 15) / 10.0)
                });
            }
            this.temporarySprites.Add(new TemporaryAnimatedSprite(6, new Vector2(index1.X * (float) Game1.tileSize, index1.Y * (float) Game1.tileSize), Color.White, 8, Game1.random.NextDouble() < 0.5, Vector2.Distance(index1, tileLocation) * 20f, 0, -1, -1f, -1, 0));
          }
          ++index1.Y;
          index1.Y = Math.Min((float) (this.map.Layers[0].LayerHeight - 1), Math.Max(0.0f, index1.Y));
        }
        ++index1.X;
        index1.Y = Math.Min((float) (this.map.Layers[0].LayerWidth - 1), Math.Max(0.0f, index1.X));
        index1.Y = tileLocation.Y - (float) radius;
        index1.Y = Math.Min((float) (this.map.Layers[0].LayerHeight - 1), Math.Max(0.0f, index1.Y));
      }
      radius /= 2;
      bool[,] circleOutlineGrid2 = Game1.getCircleOutlineGrid(radius);
      index1 = new Vector2((float) (int) ((double) tileLocation.X - (double) radius), (float) (int) ((double) tileLocation.Y - (double) radius));
      for (int index2 = 0; index2 < radius * 2 + 1; ++index2)
      {
        for (int index3 = 0; index3 < radius * 2 + 1; ++index3)
        {
          if (index2 == 0 || index3 == 0 || (index2 == radius * 2 || index3 == radius * 2))
            flag = circleOutlineGrid2[index2, index3];
          else if (circleOutlineGrid2[index2, index3])
          {
            flag = !flag;
            if (!flag && !this.objects.ContainsKey(index1) && (Game1.random.NextDouble() < 0.9 && this.doesTileHaveProperty((int) index1.X, (int) index1.Y, "Diggable", "Back") != null) && !this.isTileHoeDirt(index1))
            {
              this.checkForBuriedItem((int) index1.X, (int) index1.Y, true, false);
              this.makeHoeDirt(index1);
            }
          }
          if (flag && !this.objects.ContainsKey(index1) && (Game1.random.NextDouble() < 0.9 && this.doesTileHaveProperty((int) index1.X, (int) index1.Y, "Diggable", "Back") != null) && !this.isTileHoeDirt(index1))
          {
            this.checkForBuriedItem((int) index1.X, (int) index1.Y, true, false);
            this.makeHoeDirt(index1);
          }
          ++index1.Y;
          index1.Y = Math.Min((float) (this.map.Layers[0].LayerHeight - 1), Math.Max(0.0f, index1.Y));
        }
        ++index1.X;
        index1.Y = Math.Min((float) (this.map.Layers[0].LayerWidth - 1), Math.Max(0.0f, index1.X));
        index1.Y = tileLocation.Y - (float) radius;
        index1.Y = Math.Min((float) (this.map.Layers[0].LayerHeight - 1), Math.Max(0.0f, index1.Y));
      }
    }

    public void removeTemporarySpritesWithID(int id)
    {
      this.removeTemporarySpritesWithID((float) id);
    }

    public void removeTemporarySpritesWithID(float id)
    {
      for (int index = this.temporarySprites.Count - 1; index >= 0; --index)
      {
        if ((double) this.temporarySprites[index].id == (double) id)
        {
          if (this.temporarySprites[index].hasLit)
            Utility.removeLightSource(this.temporarySprites[index].lightID);
          this.temporarySprites.RemoveAt(index);
        }
      }
    }

    public void makeHoeDirt(Vector2 tileLocation)
    {
      if (this.doesTileHaveProperty((int) tileLocation.X, (int) tileLocation.Y, "Diggable", "Back") == null || this.isTileOccupied(tileLocation, "") || !this.isTilePassable(new Location((int) tileLocation.X, (int) tileLocation.Y), Game1.viewport))
        return;
      this.terrainFeatures.Add(tileLocation, (TerrainFeature) new HoeDirt(!Game1.isRaining || !this.isOutdoors ? 0 : 1));
    }

    public int numberOfObjectsOfType(int index, bool bigCraftable)
    {
      int num = 0;
      foreach (KeyValuePair<Vector2, Object> keyValuePair in (Dictionary<Vector2, Object>) this.Objects)
      {
        if (keyValuePair.Value.parentSheetIndex == index && keyValuePair.Value.bigCraftable == bigCraftable)
          ++num;
      }
      return num;
    }

    public virtual void performTenMinuteUpdate(int timeOfDay)
    {
      for (int index = 0; index < this.characters.Count; ++index)
      {
        if (!this.characters[index].isInvisible)
        {
          this.characters[index].checkSchedule(timeOfDay);
          this.characters[index].performTenMinuteUpdate(timeOfDay, this);
        }
      }
      for (int index = this.objects.Count - 1; index >= 0; --index)
      {
        if (this.objects[this.objects.Keys.ElementAt<Vector2>(index)].minutesElapsed(10, this))
          this.objects.Remove(this.objects.Keys.ElementAt<Vector2>(index));
      }
      if (this.isOutdoors)
      {
        Random random = new Random(timeOfDay + (int) Game1.uniqueIDForThisGame / 2 + (int) Game1.stats.DaysPlayed);
        if (this.Equals((object) Game1.currentLocation))
          this.tryToAddCritters(true);
        if (this.fishSplashPoint.Equals(Point.Zero) && random.NextDouble() < 0.5 && (!(this is Farm) || Game1.whichFarm == 1))
        {
          for (int index = 0; index < 2; ++index)
          {
            Point point = new Point(random.Next(0, this.map.GetLayer("Back").LayerWidth), random.Next(0, this.map.GetLayer("Back").LayerHeight));
            if (this.isOpenWater(point.X, point.Y))
            {
              int land = FishingRod.distanceToLand(point.X, point.Y, this);
              if (land > 1 && land <= 5)
              {
                if (Game1.player.currentLocation.Equals((object) this))
                  Game1.playSound("waterSlosh");
                this.fishSplashPoint = point;
                this.fishSplashAnimation = new TemporaryAnimatedSprite(51, new Vector2((float) (point.X * Game1.tileSize), (float) (point.Y * Game1.tileSize)), Color.White, 10, false, 80f, 999999, -1, -1f, -1, 0);
                break;
              }
            }
          }
        }
        else if (!this.fishSplashPoint.Equals(Point.Zero) && random.NextDouble() < 0.1)
        {
          this.fishSplashPoint = Point.Zero;
          this.fishSplashAnimation = (TemporaryAnimatedSprite) null;
        }
        if (Game1.player.mailReceived.Contains("ccFishTank") && !(this is Beach) && (this.orePanPoint.Equals(Point.Zero) && random.NextDouble() < 0.5))
        {
          for (int index = 0; index < 6; ++index)
          {
            Point p = new Point(random.Next(0, this.map.GetLayer("Back").LayerWidth), random.Next(0, this.map.GetLayer("Back").LayerHeight));
            if (this.isOpenWater(p.X, p.Y) && FishingRod.distanceToLand(p.X, p.Y, this) <= 1 && this.getTileIndexAt(p, "Buildings") == -1)
            {
              if (Game1.player.currentLocation.Equals((object) this))
                Game1.playSound("slosh");
              this.orePanPoint = p;
              TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(432, 1435, 16, 16), new Vector2((float) (p.X * Game1.tileSize + Game1.tileSize / 2), (float) (p.Y * Game1.tileSize + Game1.tileSize / 2)), false, 0.0f, Color.White);
              temporaryAnimatedSprite.totalNumberOfLoops = 9999999;
              temporaryAnimatedSprite.interval = 100f;
              double num1 = (double) (Game1.pixelZoom * 3) / 4.0;
              temporaryAnimatedSprite.scale = (float) num1;
              int num2 = 6;
              temporaryAnimatedSprite.animationLength = num2;
              this.orePanAnimation = temporaryAnimatedSprite;
              break;
            }
          }
        }
        else if (!this.orePanPoint.Equals(Point.Zero) && random.NextDouble() < 0.1)
        {
          this.orePanPoint = Point.Zero;
          this.orePanAnimation = (TemporaryAnimatedSprite) null;
        }
      }
      if (!this.name.Equals("BugLand") || Game1.random.NextDouble() > 0.2 || !Game1.currentLocation.Equals((object) this))
        return;
      this.characters.Add((NPC) new Fly(this.getRandomTile() * (float) Game1.tileSize, true));
    }

    public bool dropObject(Object obj)
    {
      return this.dropObject(obj, obj.TileLocation, Game1.viewport, false, (Farmer) null);
    }

    public virtual int getFishingLocation(Vector2 tile)
    {
      return -1;
    }

    public virtual Object getFish(float millisecondsAfterNibble, int bait, int waterDepth, Farmer who, double baitPotency)
    {
      return this.getFish(millisecondsAfterNibble, bait, waterDepth, who, baitPotency, (string) null);
    }

    public virtual Object getFish(float millisecondsAfterNibble, int bait, int waterDepth, Farmer who, double baitPotency, string locationName = null)
    {
      int parentSheetIndex = -1;
      Dictionary<string, string> dictionary1 = Game1.content.Load<Dictionary<string, string>>("Data\\Locations");
      bool flag1 = false;
      string key = locationName == null ? this.name : locationName;
      if (this.name.Equals("WitchSwamp") && !Game1.player.mailReceived.Contains("henchmanGone") && (Game1.random.NextDouble() < 0.25 && !Game1.player.hasItemInInventory(308, 1, 0)))
        return new Object(308, 1, false, -1, 0);
      if (dictionary1.ContainsKey(key))
      {
        string[] strArray1 = dictionary1[key].Split('/')[4 + Utility.getSeasonNumber(Game1.currentSeason)].Split(' ');
        Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
        if (strArray1.Length > 1)
        {
          int index = 0;
          while (index < strArray1.Length)
          {
            dictionary2.Add(strArray1[index], strArray1[index + 1]);
            index += 2;
          }
        }
        string[] array = dictionary2.Keys.ToArray<string>();
        Dictionary<int, string> dictionary3 = Game1.content.Load<Dictionary<int, string>>("Data\\Fish");
        Utility.Shuffle<string>(Game1.random, array);
        for (int index1 = 0; index1 < array.Length; ++index1)
        {
          bool flag2 = true;
          string[] strArray2 = dictionary3[Convert.ToInt32(array[index1])].Split('/');
          string[] strArray3 = strArray2[5].Split(' ');
          int int32 = Convert.ToInt32(dictionary2[array[index1]]);
          if (int32 == -1 || this.getFishingLocation(who.getTileLocation()) == int32)
          {
            int index2 = 0;
            while (index2 < strArray3.Length)
            {
              if (Game1.timeOfDay >= Convert.ToInt32(strArray3[index2]) && Game1.timeOfDay < Convert.ToInt32(strArray3[index2 + 1]))
              {
                flag2 = false;
                break;
              }
              index2 += 2;
            }
          }
          if (!strArray2[7].Equals("both"))
          {
            if (strArray2[7].Equals("rainy") && !Game1.isRaining)
              flag2 = true;
            else if (strArray2[7].Equals("sunny") && Game1.isRaining)
              flag2 = true;
          }
          if (who.FishingLevel < Convert.ToInt32(strArray2[12]))
            flag2 = true;
          if (!flag2)
          {
            double num1 = Convert.ToDouble(strArray2[10]);
            double num2 = Convert.ToDouble(strArray2[11]) * num1;
            double num3 = Math.Min(num1 - (double) Math.Max(0, Convert.ToInt32(strArray2[9]) - waterDepth) * num2 + (double) who.FishingLevel / 50.0, 0.899999976158142);
            if (Game1.random.NextDouble() <= num3)
            {
              parentSheetIndex = Convert.ToInt32(array[index1]);
              break;
            }
          }
        }
      }
      if (parentSheetIndex == -1)
        parentSheetIndex = Game1.random.Next(167, 173);
      if ((who.fishCaught == null || who.fishCaught.Count == 0) && parentSheetIndex >= 152)
        parentSheetIndex = 145;
      Object @object = new Object(parentSheetIndex, 1, false, -1, 0);
      if (flag1)
        @object.scale.X = 1f;
      return @object;
    }

    public virtual bool isActionableTile(int xTile, int yTile, Farmer who)
    {
      bool flag = false;
      if (this.doesTileHaveProperty(xTile, yTile, "Action", "Buildings") != null)
      {
        flag = true;
        if (this.doesTileHaveProperty(xTile, yTile, "Action", "Buildings").Contains("Message"))
          Game1.isInspectionAtCurrentCursorTile = true;
      }
      if (this.objects.ContainsKey(new Vector2((float) xTile, (float) yTile)) && this.objects[new Vector2((float) xTile, (float) yTile)].isActionable(who))
        flag = true;
      if (this.terrainFeatures.ContainsKey(new Vector2((float) xTile, (float) yTile)) && this.terrainFeatures[new Vector2((float) xTile, (float) yTile)].isActionable())
        flag = true;
      if (flag && !Utility.tileWithinRadiusOfPlayer(xTile, yTile, 1, who))
        Game1.mouseCursorTransparency = 0.5f;
      return flag;
    }

    public void digUpArtifactSpot(int xLocation, int yLocation, Farmer who)
    {
      Random random = new Random(xLocation * 2000 + yLocation + (int) Game1.uniqueIDForThisGame / 2 + (int) Game1.stats.DaysPlayed);
      int objectIndex = -1;
      foreach (KeyValuePair<int, string> keyValuePair in Game1.objectInformation)
      {
        string[] strArray1 = keyValuePair.Value.Split('/');
        if (strArray1[3].Contains("Arch"))
        {
          string[] strArray2 = strArray1[6].Split(' ');
          int index = 0;
          while (index < strArray2.Length)
          {
            if (strArray2[index].Equals(this.name) && random.NextDouble() < Convert.ToDouble(strArray2[index + 1], (IFormatProvider) CultureInfo.InvariantCulture))
            {
              objectIndex = keyValuePair.Key;
              break;
            }
            index += 2;
          }
        }
        if (objectIndex != -1)
          break;
      }
      if (random.NextDouble() < 0.2 && !(this is Farm))
        objectIndex = 102;
      if (objectIndex == 102 && who.archaeologyFound.ContainsKey(102) && who.archaeologyFound[102][0] >= 21)
        objectIndex = 770;
      if (objectIndex != -1)
      {
        Game1.createObjectDebris(objectIndex, xLocation, yLocation, who.uniqueMultiplayerID);
        who.gainExperience(5, 25);
      }
      else if (Game1.currentSeason.Equals("winter") && random.NextDouble() < 0.5 && !(this is Desert))
      {
        if (random.NextDouble() < 0.4)
          Game1.createObjectDebris(416, xLocation, yLocation, who.uniqueMultiplayerID);
        else
          Game1.createObjectDebris(412, xLocation, yLocation, who.uniqueMultiplayerID);
      }
      else
      {
        Dictionary<string, string> dictionary = Game1.content.Load<Dictionary<string, string>>("Data\\Locations");
        if (!dictionary.ContainsKey(this.name))
          return;
        string[] strArray = dictionary[this.name].Split('/')[8].Split(' ');
        if (strArray.Length == 0 || strArray[0].Equals("-1"))
          return;
        int index1 = 0;
        while (index1 < strArray.Length)
        {
          if (random.NextDouble() <= Convert.ToDouble(strArray[index1 + 1]))
          {
            int index2 = Convert.ToInt32(strArray[index1]);
            if (Game1.objectInformation.ContainsKey(index2))
            {
              if (Game1.objectInformation[index2].Split('/')[3].Contains("Arch") || index2 == 102)
              {
                if (index2 == 102 && who.archaeologyFound.ContainsKey(102) && who.archaeologyFound[102][0] >= 21)
                  index2 = 770;
                Game1.createObjectDebris(index2, xLocation, yLocation, who.uniqueMultiplayerID);
                break;
              }
            }
            Game1.createMultipleObjectDebris(index2, xLocation, yLocation, random.Next(1, 4), who.uniqueMultiplayerID);
            break;
          }
          index1 += 2;
        }
      }
    }

    public virtual string checkForBuriedItem(int xLocation, int yLocation, bool explosion, bool detectOnly)
    {
      Random random = new Random(xLocation * 2000 + yLocation * 77 + (int) Game1.uniqueIDForThisGame / 2 + (int) Game1.stats.DaysPlayed + (int) Game1.stats.DirtHoed);
      string str = this.doesTileHaveProperty(xLocation, yLocation, "Treasure", "Back");
      if (str != null)
      {
        string[] strArray = str.Split(' ');
        if (detectOnly)
          return strArray[0];
        string s = strArray[0];
        // ISSUE: reference to a compiler-generated method
        uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(s);
        if (stringHash <= 1821685427U)
        {
          if (stringHash <= 849800425U)
          {
            if ((int) stringHash != 116937720)
            {
              if ((int) stringHash == 849800425 && s == "Arch")
                Game1.createObjectDebris(Convert.ToInt32(strArray[1]), xLocation, yLocation, -1, 0, 1f, (GameLocation) null);
            }
            else if (s == "Iridium")
              Game1.createDebris(10, xLocation, yLocation, Convert.ToInt32(strArray[1]), (GameLocation) null);
          }
          else if ((int) stringHash != 872197005)
          {
            if ((int) stringHash == 1821685427 && s == "Gold")
              Game1.createDebris(6, xLocation, yLocation, Convert.ToInt32(strArray[1]), (GameLocation) null);
          }
          else if (s == "Coins")
            Game1.createObjectDebris(330, xLocation, yLocation, -1, 0, 1f, (GameLocation) null);
        }
        else if (stringHash <= 2535266071U)
        {
          if ((int) stringHash != 1952841722)
          {
            if ((int) stringHash == -1759701225 && s == "CaveCarrot")
              Game1.createObjectDebris(78, xLocation, yLocation, -1, 0, 1f, (GameLocation) null);
          }
          else if (s == "Coal")
            Game1.createDebris(4, xLocation, yLocation, Convert.ToInt32(strArray[1]), (GameLocation) null);
        }
        else if ((int) stringHash != -874770789)
        {
          if ((int) stringHash != -473546124)
          {
            if ((int) stringHash == -443652902 && s == "Object")
            {
              Game1.createObjectDebris(Convert.ToInt32(strArray[1]), xLocation, yLocation, -1, 0, 1f, (GameLocation) null);
              if (Convert.ToInt32(strArray[1]) == 78)
                ++Game1.stats.CaveCarrotsFound;
            }
          }
          else if (s == "Copper")
            Game1.createDebris(0, xLocation, yLocation, Convert.ToInt32(strArray[1]), (GameLocation) null);
        }
        else if (s == "Iron")
          Game1.createDebris(2, xLocation, yLocation, Convert.ToInt32(strArray[1]), (GameLocation) null);
        this.map.GetLayer("Back").Tiles[xLocation, yLocation].Properties["Treasure"] = (PropertyValue) null;
      }
      else
      {
        if (!this.isFarm && this.isOutdoors && (Game1.currentSeason.Equals("winter") && random.NextDouble() < 0.08) && (!explosion && !detectOnly))
        {
          Game1.createObjectDebris(random.NextDouble() < 0.5 ? 412 : 416, xLocation, yLocation, -1, 0, 1f, (GameLocation) null);
          return "";
        }
        if (this.isOutdoors && random.NextDouble() < 0.03 && !explosion)
        {
          if (detectOnly)
          {
            this.map.GetLayer("Back").Tiles[xLocation, yLocation].Properties.Add("Treasure", new PropertyValue("Object " + (object) 330));
            return "Object";
          }
          Game1.createObjectDebris(330, xLocation, yLocation, -1, 0, 1f, (GameLocation) null);
          return "";
        }
      }
      return "";
    }

    public void setMapTile(int tileX, int tileY, int index, string layer, string action, int whichTileSheet = 0)
    {
      this.map.GetLayer(layer).Tiles[tileX, tileY] = (Tile) new StaticTile(this.map.GetLayer(layer), this.map.TileSheets[whichTileSheet], BlendMode.Alpha, index);
      if (action == null || layer == null || !layer.Equals("Buildings"))
        return;
      this.map.GetLayer("Buildings").Tiles[tileX, tileY].Properties.Add("Action", new PropertyValue(action));
    }

    public void setMapTileIndex(int tileX, int tileY, int index, string layer, int whichTileSheet = 0)
    {
      try
      {
        if (this.map.GetLayer(layer).Tiles[tileX, tileY] != null)
          this.map.GetLayer(layer).Tiles[tileX, tileY].TileIndex = index;
        else
          this.map.GetLayer(layer).Tiles[tileX, tileY] = (Tile) new StaticTile(this.map.GetLayer(layer), this.map.TileSheets[whichTileSheet], BlendMode.Alpha, index);
      }
      catch (Exception ex)
      {
      }
    }

    public virtual void shiftObjects(int dx, int dy)
    {
      SerializableDictionary<Vector2, Object> serializableDictionary = new SerializableDictionary<Vector2, Object>();
      foreach (KeyValuePair<Vector2, Object> keyValuePair in (Dictionary<Vector2, Object>) this.objects)
      {
        keyValuePair.Value.tileLocation = new Vector2(keyValuePair.Key.X + (float) dx, keyValuePair.Key.Y + (float) dy);
        serializableDictionary.Add(keyValuePair.Value.tileLocation, keyValuePair.Value);
      }
      this.objects = (SerializableDictionary<Vector2, Object>) null;
      this.objects = serializableDictionary;
    }

    public int getTileIndexAt(Point p, string layer)
    {
      Tile tile = this.map.GetLayer(layer).Tiles[p.X, p.Y];
      if (tile != null)
        return tile.TileIndex;
      return -1;
    }

    public int getTileIndexAt(int x, int y, string layer)
    {
      if (this.map.GetLayer(layer) == null)
        return -1;
      Tile tile = this.map.GetLayer(layer).Tiles[x, y];
      if (tile != null)
        return tile.TileIndex;
      return -1;
    }

    public bool breakStone(int indexOfStone, int x, int y, Farmer who, Random r)
    {
      int howMuch = 0;
      int num1 = who.professions.Contains(18) ? 1 : 0;
      switch (indexOfStone)
      {
        case 764:
          Game1.createMultipleObjectDebris(384, x, y, num1 + r.Next(1, 4) + (r.NextDouble() < (double) who.LuckLevel / 100.0 ? 1 : 0) + (r.NextDouble() < (double) who.MiningLevel / 100.0 ? 1 : 0), who.uniqueMultiplayerID, this);
          howMuch = 18;
          this.temporarySprites.AddRange((IEnumerable<TemporaryAnimatedSprite>) Utility.sparkleWithinArea(new Microsoft.Xna.Framework.Rectangle(x * Game1.tileSize, (y - 1) * Game1.tileSize, Game1.tileSize / 2, Game1.tileSize * 3 / 2), 3, Color.Yellow * 0.5f, 175, 100, ""));
          break;
        case 765:
          Game1.createMultipleObjectDebris(386, x, y, num1 + r.Next(1, 4) + (r.NextDouble() < (double) who.LuckLevel / 100.0 ? 1 : 0) + (r.NextDouble() < (double) who.MiningLevel / 100.0 ? 1 : 0), who.uniqueMultiplayerID, this);
          this.temporarySprites.AddRange((IEnumerable<TemporaryAnimatedSprite>) Utility.sparkleWithinArea(new Microsoft.Xna.Framework.Rectangle(x * Game1.tileSize, (y - 1) * Game1.tileSize, Game1.tileSize / 2, Game1.tileSize * 3 / 2), 6, Color.BlueViolet * 0.5f, 175, 100, ""));
          if (r.NextDouble() < 0.04)
            Game1.createMultipleObjectDebris(74, x, y, 1);
          howMuch = 50;
          break;
        case 670:
        case 668:
          Game1.createMultipleObjectDebris(390, x, y, num1 + r.Next(1, 3) + (r.NextDouble() < (double) who.LuckLevel / 100.0 ? 1 : 0) + (r.NextDouble() < (double) who.MiningLevel / 100.0 ? 1 : 0), who.uniqueMultiplayerID, this);
          howMuch = 3;
          if (r.NextDouble() < 0.08)
          {
            Game1.createMultipleObjectDebris(382, x, y, 1 + num1, who.uniqueMultiplayerID, this);
            howMuch = 4;
            break;
          }
          break;
        case 751:
          Game1.createMultipleObjectDebris(378, x, y, num1 + r.Next(1, 4) + (r.NextDouble() < (double) who.LuckLevel / 100.0 ? 1 : 0) + (r.NextDouble() < (double) who.MiningLevel / 100.0 ? 1 : 0), who.uniqueMultiplayerID, this);
          howMuch = 5;
          this.temporarySprites.AddRange((IEnumerable<TemporaryAnimatedSprite>) Utility.sparkleWithinArea(new Microsoft.Xna.Framework.Rectangle(x * Game1.tileSize, (y - 1) * Game1.tileSize, Game1.tileSize / 2, Game1.tileSize * 3 / 2), 3, Color.Orange * 0.5f, 175, 100, ""));
          break;
        case 290:
          Game1.createMultipleObjectDebris(380, x, y, num1 + r.Next(1, 4) + (r.NextDouble() < (double) who.LuckLevel / 100.0 ? 1 : 0) + (r.NextDouble() < (double) who.MiningLevel / 100.0 ? 1 : 0), who.uniqueMultiplayerID, this);
          howMuch = 12;
          this.temporarySprites.AddRange((IEnumerable<TemporaryAnimatedSprite>) Utility.sparkleWithinArea(new Microsoft.Xna.Framework.Rectangle(x * Game1.tileSize, (y - 1) * Game1.tileSize, Game1.tileSize / 2, Game1.tileSize * 3 / 2), 3, Color.White * 0.5f, 175, 100, ""));
          break;
        case 2:
          Game1.createObjectDebris(72, x, y, who.uniqueMultiplayerID, this);
          howMuch = 150;
          break;
        case 4:
          Game1.createObjectDebris(64, x, y, who.uniqueMultiplayerID, this);
          howMuch = 80;
          break;
        case 6:
          Game1.createObjectDebris(70, x, y, who.uniqueMultiplayerID, this);
          howMuch = 40;
          break;
        case 8:
          Game1.createObjectDebris(66, x, y, who.uniqueMultiplayerID, this);
          howMuch = 16;
          break;
        case 10:
          Game1.createObjectDebris(68, x, y, who.uniqueMultiplayerID, this);
          howMuch = 16;
          break;
        case 12:
          Game1.createObjectDebris(60, x, y, who.uniqueMultiplayerID, this);
          howMuch = 80;
          break;
        case 14:
          Game1.createObjectDebris(62, x, y, who.uniqueMultiplayerID, this);
          howMuch = 40;
          break;
        case 75:
          Game1.createObjectDebris(535, x, y, who.uniqueMultiplayerID, this);
          howMuch = 8;
          break;
        case 76:
          Game1.createObjectDebris(536, x, y, who.uniqueMultiplayerID, this);
          howMuch = 16;
          break;
        case 77:
          Game1.createObjectDebris(537, x, y, who.uniqueMultiplayerID, this);
          howMuch = 32;
          break;
      }
      if (who.professions.Contains(19) && r.NextDouble() < 0.5)
      {
        switch (indexOfStone)
        {
          case 2:
            Game1.createObjectDebris(72, x, y, who.uniqueMultiplayerID, this);
            howMuch = 100;
            break;
          case 4:
            Game1.createObjectDebris(64, x, y, who.uniqueMultiplayerID, this);
            howMuch = 50;
            break;
          case 6:
            Game1.createObjectDebris(70, x, y, who.uniqueMultiplayerID, this);
            howMuch = 20;
            break;
          case 8:
            Game1.createObjectDebris(66, x, y, who.uniqueMultiplayerID, this);
            howMuch = 8;
            break;
          case 10:
            Game1.createObjectDebris(68, x, y, who.uniqueMultiplayerID, this);
            howMuch = 8;
            break;
          case 12:
            Game1.createObjectDebris(60, x, y, who.uniqueMultiplayerID, this);
            howMuch = 50;
            break;
          case 14:
            Game1.createObjectDebris(62, x, y, who.uniqueMultiplayerID, this);
            howMuch = 20;
            break;
        }
      }
      if (indexOfStone == 46)
      {
        Game1.createDebris(10, x, y, r.Next(1, 4), (GameLocation) null);
        Game1.createDebris(6, x, y, r.Next(1, 5), (GameLocation) null);
        if (r.NextDouble() < 0.25)
          Game1.createMultipleObjectDebris(74, x, y, 1, who.uniqueMultiplayerID, this);
        howMuch = 50;
        ++Game1.stats.MysticStonesCrushed;
      }
      if ((this.isOutdoors || this.treatAsOutdoors) && howMuch == 0)
      {
        double num2 = Game1.dailyLuck / 2.0 + (double) who.MiningLevel * 0.005 + (double) who.LuckLevel * 0.001;
        Random random = new Random(x * 1000 + y + (int) Game1.stats.DaysPlayed + (int) Game1.uniqueIDForThisGame / 2);
        Game1.createDebris(14, x, y, 1, this);
        who.gainExperience(3, 1);
        if (who.professions.Contains(21) && random.NextDouble() < 0.05 * (1.0 + num2))
          Game1.createObjectDebris(382, x, y, who.uniqueMultiplayerID, this);
        if (random.NextDouble() < 0.05 * (1.0 + num2))
        {
          random.Next(1, 3);
          random.NextDouble();
          double num3 = 0.1 * (1.0 + num2);
          Game1.createObjectDebris(382, x, y, who.uniqueMultiplayerID, this);
          this.temporarySprites.Add(new TemporaryAnimatedSprite(25, new Vector2((float) (Game1.tileSize * x), (float) (Game1.tileSize * y)), Color.White, 8, Game1.random.NextDouble() < 0.5, 80f, 0, -1, -1f, Game1.tileSize * 2, 0));
          who.gainExperience(3, 5);
        }
      }
      who.gainExperience(3, howMuch);
      return howMuch > 0;
    }

    public bool isBehindBush(Vector2 Tile)
    {
      if (this.largeTerrainFeatures != null)
      {
        Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle((int) Tile.X * Game1.tileSize, (int) ((double) Tile.Y + 1.0) * Game1.tileSize, Game1.tileSize, Game1.tileSize * 2);
        foreach (LargeTerrainFeature largeTerrainFeature in this.largeTerrainFeatures)
        {
          if (largeTerrainFeature.getBoundingBox().Intersects(rectangle))
            return true;
        }
      }
      return false;
    }

    public bool isBehindTree(Vector2 Tile)
    {
      if (this.terrainFeatures != null)
      {
        Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle((int) ((double) Tile.X - 1.0) * Game1.tileSize, (int) Tile.Y * Game1.tileSize, Game1.tileSize * 3, Game1.tileSize * 4);
        foreach (KeyValuePair<Vector2, TerrainFeature> terrainFeature in (Dictionary<Vector2, TerrainFeature>) this.terrainFeatures)
        {
          if (terrainFeature.Value is Tree && terrainFeature.Value.getBoundingBox(terrainFeature.Key).Intersects(rectangle))
            return true;
        }
      }
      return false;
    }

    public virtual void spawnObjects()
    {
      Random random = new Random((int) Game1.uniqueIDForThisGame / 2 + (int) Game1.stats.DaysPlayed);
      Dictionary<string, string> dictionary = Game1.content.Load<Dictionary<string, string>>("Data\\Locations");
      if (dictionary.ContainsKey(this.name))
      {
        string str = dictionary[this.name].Split('/')[Utility.getSeasonNumber(Game1.currentSeason)];
        if (!str.Equals("-1") && this.numberOfSpawnedObjectsOnMap < 6)
        {
          string[] strArray = str.Split(' ');
          int num1 = random.Next(1, Math.Min(5, 7 - this.numberOfSpawnedObjectsOnMap));
          for (int index1 = 0; index1 < num1; ++index1)
          {
            for (int index2 = 0; index2 < 11; ++index2)
            {
              int num2 = random.Next(this.map.DisplayWidth / Game1.tileSize);
              int num3 = random.Next(this.map.DisplayHeight / Game1.tileSize);
              Vector2 vector2 = new Vector2((float) num2, (float) num3);
              Object @object;
              this.objects.TryGetValue(vector2, out @object);
              int index3 = random.Next(strArray.Length / 2) * 2;
              if (@object == null && this.doesTileHaveProperty(num2, num3, "Spawnable", "Back") != null && (random.NextDouble() < Convert.ToDouble(strArray[index3 + 1], (IFormatProvider) CultureInfo.InvariantCulture) && this.isTileLocationTotallyClearAndPlaceable(num2, num3)) && (this.getTileIndexAt(num2, num3, "AlwaysFront") == -1 && !this.isBehindBush(vector2) && (Game1.random.NextDouble() < 0.1 || !this.isBehindTree(vector2))) && this.dropObject(new Object(vector2, Convert.ToInt32(strArray[index3]), (string) null, false, true, false, true), new Vector2((float) (num2 * Game1.tileSize), (float) (num3 * Game1.tileSize)), Game1.viewport, true, (Farmer) null))
              {
                this.numberOfSpawnedObjectsOnMap = this.numberOfSpawnedObjectsOnMap + 1;
                break;
              }
            }
          }
        }
      }
      List<Vector2> source = new List<Vector2>();
      foreach (KeyValuePair<Vector2, Object> keyValuePair in (Dictionary<Vector2, Object>) this.objects)
      {
        if (keyValuePair.Value.parentSheetIndex == 590)
          source.Add(keyValuePair.Key);
      }
      if (!(this is Farm))
        this.spawnWeedsAndStones(-1, false, true);
      for (int index = source.Count - 1; index >= 0; --index)
      {
        if (Game1.random.NextDouble() < 0.15)
        {
          this.objects.Remove(source.ElementAt<Vector2>(index));
          source.RemoveAt(index);
        }
      }
      if (source.Count > (this is Farm ? 0 : 1) && (!Game1.currentSeason.Equals("winter") || source.Count > 4))
        return;
      double num4 = 1.0;
      while (random.NextDouble() < num4)
      {
        int num1 = random.Next(this.map.DisplayWidth / Game1.tileSize);
        int num2 = random.Next(this.map.DisplayHeight / Game1.tileSize);
        Vector2 vector2 = new Vector2((float) num1, (float) num2);
        if (this.isTileLocationTotallyClearAndPlaceable(vector2) && this.getTileIndexAt(num1, num2, "AlwaysFront") == -1 && (this.getTileIndexAt(num1, num2, "Front") == -1 && !this.isBehindBush(vector2)) && (this.doesTileHaveProperty(num1, num2, "Diggable", "Back") != null || Game1.currentSeason.Equals("winter") && this.doesTileHaveProperty(num1, num2, "Type", "Back") != null && this.doesTileHaveProperty(num1, num2, "Type", "Back").Equals("Grass")))
        {
          if (!this.name.Equals("Forest") || num1 < 93 || num2 > 22)
            this.objects.Add(vector2, new Object(vector2, 590, 1));
          else
            continue;
        }
        num4 *= 0.75;
        if (Game1.currentSeason.Equals("winter"))
          num4 += 0.100000001490116;
      }
    }

    public bool isTileLocationOpen(Location location)
    {
      if (this.map.GetLayer("Buildings").PickTile(location, Game1.viewport.Size) != null || this.doesTileHaveProperty(location.X, location.X, "Water", "Back") != null || this.map.GetLayer("Front").PickTile(location, Game1.viewport.Size) != null)
        return false;
      if (this.map.GetLayer("AlwaysFront") != null)
        return this.map.GetLayer("AlwaysFront").PickTile(location, Game1.viewport.Size) == null;
      return true;
    }

    public bool isTileLocationOpenIgnoreFrontLayers(Location location)
    {
      if (this.map.GetLayer("Buildings").PickTile(location, Game1.viewport.Size) == null)
        return this.doesTileHaveProperty(location.X, location.X, "Water", "Back") == null;
      return false;
    }

    public void spawnWeedsAndStones(int numDebris = -1, bool weedsOnly = false, bool spawnFromOldWeeds = true)
    {
      if (this is Farm && (this as Farm).isBuildingConstructed("Gold Clock") || (this is Beach || Game1.currentSeason.Equals("winter")) || this is Desert)
        return;
      int num = numDebris != -1 ? numDebris : (Game1.random.NextDouble() < 0.95 ? (Game1.random.NextDouble() < 0.25 ? Game1.random.Next(10, 21) : Game1.random.Next(5, 11)) : 0);
      if (Game1.isRaining)
        num *= 2;
      if (Game1.dayOfMonth == 1)
        num *= 5;
      if (this.objects.Count <= 0 & spawnFromOldWeeds)
        return;
      if (!(this is Farm))
        num /= 2;
      for (int index = 0; index < num; ++index)
      {
        Vector2 vector2_1 = spawnFromOldWeeds ? new Vector2((float) Game1.random.Next(-1, 2), (float) Game1.random.Next(-1, 2)) : new Vector2((float) Game1.random.Next(this.map.Layers[0].LayerWidth), (float) Game1.random.Next(this.map.Layers[0].LayerHeight));
        while (spawnFromOldWeeds && vector2_1.Equals(Vector2.Zero))
          vector2_1 = new Vector2((float) Game1.random.Next(-1, 2), (float) Game1.random.Next(-1, 2));
        KeyValuePair<Vector2, Object> keyValuePair = new KeyValuePair<Vector2, Object>(Vector2.Zero, (Object) null);
        if (spawnFromOldWeeds)
          keyValuePair = this.objects.ElementAt<KeyValuePair<Vector2, Object>>(Game1.random.Next(this.objects.Count));
        Vector2 vector2_2 = spawnFromOldWeeds ? keyValuePair.Key : Vector2.Zero;
        bool flag1 = this is Farm;
        if ((flag1 && this.doesTileHaveProperty((int) ((double) vector2_1.X + (double) vector2_2.X), (int) ((double) vector2_1.Y + (double) vector2_2.Y), "Diggable", "Back") != null || !flag1 && this.doesTileHaveProperty((int) ((double) vector2_1.X + (double) vector2_2.X), (int) ((double) vector2_1.Y + (double) vector2_2.Y), "Diggable", "Back") == null) && (this.doesTileHaveProperty((int) ((double) vector2_1.X + (double) vector2_2.X), (int) ((double) vector2_1.Y + (double) vector2_2.Y), "Type", "Back") == null || !this.doesTileHaveProperty((int) ((double) vector2_1.X + (double) vector2_2.X), (int) ((double) vector2_1.Y + (double) vector2_2.Y), "Type", "Back").Equals("Wood")) && (this.isTileLocationTotallyClearAndPlaceable(vector2_1 + vector2_2) || spawnFromOldWeeds && (this.objects.ContainsKey(vector2_1 + vector2_2) && this.objects[vector2_1 + vector2_2].parentSheetIndex != 105 || this.terrainFeatures.ContainsKey(vector2_1 + vector2_2) && (this.terrainFeatures[vector2_1 + vector2_2] is HoeDirt || this.terrainFeatures[vector2_1 + vector2_2] is Flooring))) && (this.doesTileHaveProperty((int) ((double) vector2_1.X + (double) vector2_2.X), (int) ((double) vector2_1.Y + (double) vector2_2.Y), "NoSpawn", "Back") == null && (spawnFromOldWeeds || !this.objects.ContainsKey(vector2_1 + vector2_2))))
        {
          int parentSheetIndex = -1;
          if (this is Desert)
          {
            parentSheetIndex = 750;
          }
          else
          {
            if (Game1.random.NextDouble() < 0.5 && !weedsOnly && (!spawnFromOldWeeds || keyValuePair.Value.Name.Equals("Stone") || keyValuePair.Value.Name.Contains("Twig")))
              parentSheetIndex = Game1.random.NextDouble() >= 0.5 ? (Game1.random.NextDouble() < 0.5 ? 343 : 450) : (Game1.random.NextDouble() < 0.5 ? 294 : 295);
            else if (!spawnFromOldWeeds || keyValuePair.Value.Name.Contains("Weed"))
              parentSheetIndex = GameLocation.getWeedForSeason(Game1.random, Game1.currentSeason);
            if (this is Farm && !spawnFromOldWeeds && Game1.random.NextDouble() < 0.05)
            {
              this.terrainFeatures.Add(vector2_1 + vector2_2, (TerrainFeature) new Tree(Game1.random.Next(3) + 1, Game1.random.Next(3)));
              continue;
            }
          }
          if (parentSheetIndex != -1)
          {
            bool flag2 = false;
            if (this.objects.ContainsKey(vector2_1 + vector2_2))
            {
              Object @object = this.objects[vector2_1 + vector2_2];
              if (!(@object is Fence) && !(@object is Chest))
              {
                if (@object.name != null && !@object.Name.Contains("Weed") && (!@object.Name.Equals("Stone") && !@object.name.Contains("Twig")) && @object.name.Length > 0)
                {
                  flag2 = true;
                  Game1.debugOutput = @object.Name + " was destroyed";
                }
                this.objects.Remove(vector2_1 + vector2_2);
              }
              else
                continue;
            }
            else if (this.terrainFeatures.ContainsKey(vector2_1 + vector2_2))
            {
              try
              {
                flag2 = this.terrainFeatures[vector2_1 + vector2_2] is HoeDirt || this.terrainFeatures[vector2_1 + vector2_2] is Flooring;
              }
              catch (Exception ex)
              {
              }
              if (!flag2)
                break;
              this.terrainFeatures.Remove(vector2_1 + vector2_2);
            }
            if (flag2 && this is Farm)
              Game1.showGlobalMessage(Game1.content.LoadString("Strings\\Locations:Farm_WeedsDestruction"));
            this.objects.Add(vector2_1 + vector2_2, new Object(vector2_1 + vector2_2, parentSheetIndex, 1));
          }
        }
      }
    }

    public virtual void removeEverythingExceptCharactersFromThisTile(int x, int y)
    {
      Vector2 key = new Vector2((float) x, (float) y);
      if (this.terrainFeatures.ContainsKey(key))
        this.terrainFeatures.Remove(key);
      if (!this.objects.ContainsKey(key))
        return;
      this.objects.Remove(key);
    }

    public virtual void removeEverythingFromThisTile(int x, int y)
    {
      Vector2 vector2 = new Vector2((float) x, (float) y);
      if (this.terrainFeatures.ContainsKey(vector2))
        this.terrainFeatures.Remove(vector2);
      if (this.objects.ContainsKey(vector2))
        this.objects.Remove(vector2);
      for (int index = this.characters.Count - 1; index >= 0; --index)
      {
        if (this.characters[index].getTileLocation().Equals(vector2) && this.characters[index] is Monster)
          this.characters.RemoveAt(index);
      }
    }

    public void checkForEvents()
    {
      if (Game1.killScreen && !Game1.eventUp)
      {
        if (this.name.Equals("Mine"))
        {
          string str1;
          string str2;
          switch (Game1.random.Next(7))
          {
            case 0:
              str1 = "Robin";
              str2 = "Data\\ExtraDialogue:Mines_PlayerKilled_Robin";
              break;
            case 1:
              str1 = "Clint";
              str2 = "Data\\ExtraDialogue:Mines_PlayerKilled_Clint";
              break;
            case 2:
              str1 = "Maru";
              str2 = Game1.player.spouse == null || !Game1.player.spouse.Equals("Maru") ? "Data\\ExtraDialogue:Mines_PlayerKilled_Maru_NotSpouse" : "Data\\ExtraDialogue:Mines_PlayerKilled_Maru_Spouse";
              break;
            default:
              str1 = "Linus";
              str2 = "Data\\ExtraDialogue:Mines_PlayerKilled_Linus";
              break;
          }
          if (Game1.random.NextDouble() < 0.1 && Game1.player.spouse != null && (!Game1.player.spouse.Contains("engaged") && Game1.player.spouse.Length > 1))
          {
            str1 = Game1.player.spouse;
            str2 = Game1.player.isMale ? "Data\\ExtraDialogue:Mines_PlayerKilled_Spouse_PlayerMale" : "Data\\ExtraDialogue:Mines_PlayerKilled_Spouse_PlayerFemale";
          }
          this.currentEvent = new Event(Game1.content.LoadString("Data\\Events\\Mine:PlayerKilled", (object) str1, (object) str2, (object) Game1.player.name), -1);
        }
        else if (this.name.Equals("Hospital"))
          this.currentEvent = new Event(Game1.content.LoadString("Data\\Events\\Hospital:PlayerKilled", (object) Game1.player.name), -1);
        Game1.eventUp = true;
        Game1.killScreen = false;
        Game1.player.health = 10;
      }
      else
      {
        if (Game1.eventUp || Game1.farmEvent != null)
          return;
        string festival = Game1.currentSeason + (object) Game1.dayOfMonth;
        try
        {
          Event @event = new Event();
          if (@event.tryToLoadFestival(festival))
            this.currentEvent = @event;
        }
        catch (Exception ex)
        {
        }
        if (!Game1.eventUp && this.currentEvent == null)
        {
          Dictionary<string, string> dictionary;
          try
          {
            dictionary = Game1.content.Load<Dictionary<string, string>>("Data\\Events\\" + this.name);
          }
          catch (Exception ex)
          {
            return;
          }
          if (dictionary != null)
          {
            string[] array = dictionary.Keys.ToArray<string>();
            for (int index = 0; index < array.Length; ++index)
            {
              int eventID = this.checkEventPrecondition(array[index]);
              if (eventID != -1)
              {
                this.currentEvent = new Event(dictionary[array[index]], eventID);
                break;
              }
            }
            if (this.currentEvent == null && this.name.Equals("Farm") && (!Game1.player.mailReceived.Contains("rejectedPet") && Game1.stats.DaysPlayed >= 20U) && !Game1.player.hasPet())
            {
              for (int index = 0; index < array.Length; ++index)
              {
                if (array[index].Contains("dog") && !Game1.player.catPerson || array[index].Contains("cat") && Game1.player.catPerson)
                {
                  this.currentEvent = new Event(dictionary[array[index]], -1);
                  Game1.player.eventsSeen.Add(Convert.ToInt32(array[index].Split('/')[0]));
                  break;
                }
              }
            }
          }
        }
        if (this.currentEvent == null)
          return;
        if (Game1.player.getMount() != null)
        {
          this.currentEvent.playerWasMounted = true;
          Game1.player.getMount().dismount();
        }
        foreach (NPC character in this.characters)
          character.clearTextAboveHead();
        Game1.eventUp = true;
        Game1.displayHUD = false;
        Game1.player.CanMove = false;
        Game1.player.showNotCarrying();
        if (this.critters == null)
          return;
        this.critters.Clear();
      }
    }

    public virtual void drawWater(SpriteBatch b)
    {
      if (this.currentEvent != null)
        this.currentEvent.drawUnderWater(b);
      if (this.waterTiles == null)
        return;
      for (int index1 = Math.Max(0, Game1.viewport.Y / Game1.tileSize - 1); index1 < Math.Min(this.map.Layers[0].LayerHeight, (Game1.viewport.Y + Game1.viewport.Height) / Game1.tileSize + 2); ++index1)
      {
        for (int index2 = Math.Max(0, Game1.viewport.X / Game1.tileSize - 1); index2 < Math.Min(this.map.Layers[0].LayerWidth, (Game1.viewport.X + Game1.viewport.Width) / Game1.tileSize + 1); ++index2)
        {
          if (this.waterTiles[index2, index1])
          {
            int num = index1 == this.map.Layers[0].LayerHeight - 1 ? 1 : (!this.waterTiles[index2, index1 + 1] ? 1 : 0);
            bool flag = index1 == 0 || !this.waterTiles[index2, index1 - 1];
            b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (index2 * Game1.tileSize), (float) (index1 * Game1.tileSize - (!flag ? (int) this.waterPosition : 0)))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(this.waterAnimationIndex * 64, 2064 + ((index2 + index1) % 2 == 0 ? (this.waterTileFlip ? 128 : 0) : (this.waterTileFlip ? 0 : 128)) + (flag ? (int) this.waterPosition : 0), 64, 64 + (flag ? (int) -(double) this.waterPosition : 0))), this.waterColor, 0.0f, Vector2.Zero, (float) (1.0 * ((double) Game1.pixelZoom / 4.0)), SpriteEffects.None, 0.56f);
            if (num != 0)
              b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (index2 * Game1.tileSize), (float) ((index1 + 1) * Game1.tileSize - (int) this.waterPosition))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(this.waterAnimationIndex * 64, 2064 + ((index2 + (index1 + 1)) % 2 == 0 ? (this.waterTileFlip ? 128 : 0) : (this.waterTileFlip ? 0 : 128)), 64, 64 - (int) (64.0 - (double) this.waterPosition) - 1)), this.waterColor, 0.0f, Vector2.Zero, (float) (1.0 * ((double) Game1.pixelZoom / 4.0)), SpriteEffects.None, 0.56f);
          }
        }
      }
    }

    public TemporaryAnimatedSprite getTemporarySpriteByID(int id)
    {
      for (int index = 0; index < this.temporarySprites.Count; ++index)
      {
        if ((double) this.temporarySprites[index].id == (double) id)
          return this.temporarySprites[index];
      }
      return (TemporaryAnimatedSprite) null;
    }

    protected void drawDebris(SpriteBatch b)
    {
      int num1 = 0;
      foreach (Debris debri in this.debris)
      {
        ++num1;
        if (debri.item != null)
          debri.item.drawInMenu(b, Game1.GlobalToLocal(Game1.viewport, debri.Chunks[0].position + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2))), (float) (0.800000011920929 + (double) debri.itemQuality * 0.100000001490116), 1f, (float) (((double) (debri.chunkFinalYLevel + Game1.tileSize) + (double) debri.Chunks[0].position.X / 10000.0) / 10000.0), false);
        else if (debri.debrisType == Debris.DebrisType.LETTERS)
          Game1.drawWithBorder(debri.debrisMessage, Color.Black, debri.nonSpriteChunkColor, Game1.GlobalToLocal(Game1.viewport, debri.Chunks[0].position), debri.Chunks[0].rotation, debri.Chunks[0].scale, (float) (((double) debri.Chunks[0].position.Y + (double) Game1.tileSize) / 10000.0));
        else if (debri.debrisType == Debris.DebrisType.NUMBERS)
          NumberSprite.draw(debri.chunkType, b, Game1.GlobalToLocal(Game1.viewport, new Vector2(debri.Chunks[0].position.X, (float) debri.chunkFinalYLevel - ((float) debri.chunkFinalYLevel - debri.Chunks[0].position.Y))), debri.nonSpriteChunkColor, debri.Chunks[0].scale * 0.75f, (float) (0.980000019073486 + 9.99999974737875E-05 * (double) num1), debri.Chunks[0].alpha, -1 * (int) ((double) debri.chunkFinalYLevel - (double) debri.Chunks[0].position.Y) / 2, 0);
        else if (debri.debrisType == Debris.DebrisType.SPRITECHUNKS)
        {
          for (int index = 0; index < debri.Chunks.Count; ++index)
            b.Draw(debri.spriteChunkSheet, Game1.GlobalToLocal(Game1.viewport, debri.Chunks[index].position), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(debri.Chunks[index].xSpriteSheet, debri.Chunks[index].ySpriteSheet, Math.Min(debri.sizeOfSourceRectSquares, debri.spriteChunkSheet.Bounds.Width), Math.Min(debri.sizeOfSourceRectSquares, debri.spriteChunkSheet.Bounds.Height))), debri.nonSpriteChunkColor * debri.Chunks[index].alpha, debri.Chunks[index].rotation, new Vector2((float) (debri.sizeOfSourceRectSquares / 2), (float) (debri.sizeOfSourceRectSquares / 2)), debri.Chunks[index].scale, SpriteEffects.None, (float) (((double) (debri.chunkFinalYLevel + Game1.tileSize / 4) + (double) debri.Chunks[index].position.X / 10000.0) / 10000.0));
        }
        else if (debri.debrisType == Debris.DebrisType.SQUARES)
        {
          for (int index = 0; index < debri.Chunks.Count; ++index)
            b.Draw(Game1.littleEffect, Game1.GlobalToLocal(Game1.viewport, debri.Chunks[index].position), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, 4, 4)), debri.nonSpriteChunkColor, 0.0f, Vector2.Zero, (float) (1.0 + (double) debri.Chunks[index].yVelocity / 2.0), SpriteEffects.None, (float) (((double) debri.Chunks[index].position.Y + (double) Game1.tileSize) / 10000.0));
        }
        else if (debri.debrisType != Debris.DebrisType.CHUNKS)
        {
          for (int index = 0; index < debri.Chunks.Count; ++index)
          {
            if (debri.Chunks[index].debrisType <= 0)
            {
              b.Draw(Game1.bigCraftableSpriteSheet, Game1.GlobalToLocal(Game1.viewport, debri.Chunks[index].position + new Vector2((float) (Game1.tileSize / 2), (float) Game1.tileSize)), new Microsoft.Xna.Framework.Rectangle?(Game1.getArbitrarySourceRect(Game1.bigCraftableSpriteSheet, 16, 32, -debri.Chunks[index].debrisType)), Color.White, 0.0f, new Vector2(8f, 32f), 3.2f, SpriteEffects.None, (float) (((double) debri.Chunks[index].position.Y + (double) (Game1.tileSize / 4) + (double) debri.Chunks[index].position.X / 20000.0) / 10000.0));
              SpriteBatch spriteBatch = b;
              Texture2D shadowTexture = Game1.shadowTexture;
              Vector2 local = Game1.GlobalToLocal(Game1.viewport, new Vector2(debri.Chunks[index].position.X + (float) Game1.tileSize * 0.4f, (float) (debri.chunkFinalYLevel + Game1.tileSize / 2)));
              Microsoft.Xna.Framework.Rectangle? sourceRectangle = new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds);
              Color white = Color.White;
              double num2 = 0.0;
              Microsoft.Xna.Framework.Rectangle bounds = Game1.shadowTexture.Bounds;
              double x = (double) bounds.Center.X;
              bounds = Game1.shadowTexture.Bounds;
              double y = (double) bounds.Center.Y;
              Vector2 origin = new Vector2((float) x, (float) y);
              double num3 = 3.0 - ((double) debri.chunkFinalYLevel - (double) debri.Chunks[index].position.Y) / 128.0;
              int num4 = 0;
              double num5 = (double) debri.chunkFinalYLevel / 10000.0;
              spriteBatch.Draw(shadowTexture, local, sourceRectangle, white, (float) num2, origin, (float) num3, (SpriteEffects) num4, (float) num5);
            }
            else
            {
              b.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, debri.Chunks[index].position), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, debri.Chunks[index].debrisType, 16, 16)), Color.White, 0.0f, Vector2.Zero, debri.debrisType == Debris.DebrisType.RESOURCE || debri.floppingFish ? (float) Game1.pixelZoom : (float) Game1.pixelZoom * (float) (0.800000011920929 + (double) debri.itemQuality * 0.100000001490116), !debri.floppingFish || debri.Chunks[index].bounces % 2 != 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, (float) (((double) (debri.chunkFinalYLevel + Game1.tileSize / 2) + (double) debri.Chunks[index].position.X / 10000.0) / 10000.0));
              b.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2(debri.Chunks[index].position.X + (float) Game1.tileSize * 0.4f, (float) (debri.chunkFinalYLevel + Game1.tileSize / 2 + Game1.tileSize / 5 * debri.itemQuality))), new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds), Color.White * 0.75f, 0.0f, new Vector2((float) Game1.shadowTexture.Bounds.Center.X, (float) Game1.shadowTexture.Bounds.Center.Y), (float) (3.0 - ((double) debri.chunkFinalYLevel - (double) debri.Chunks[index].position.Y) / 96.0), SpriteEffects.None, (float) debri.chunkFinalYLevel / 10000f);
            }
          }
        }
        else
        {
          for (int index = 0; index < debri.Chunks.Count; ++index)
            b.Draw(Game1.debrisSpriteSheet, Game1.GlobalToLocal(Game1.viewport, debri.Chunks[index].position), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, debri.Chunks[index].debrisType, 16, 16)), debri.chunksColor, 0.0f, Vector2.Zero, (float) Game1.pixelZoom * debri.scale, SpriteEffects.None, (float) (((double) debri.Chunks[index].position.Y + (double) (Game1.tileSize * 2) + (double) debri.Chunks[index].position.X / 10000.0) / 10000.0));
        }
      }
    }

    public virtual void draw(SpriteBatch b)
    {
      if (!Game1.eventUp)
      {
        for (int index = 0; index < this.characters.Count; ++index)
        {
          if (this.characters[index] != null)
            this.characters[index].draw(b);
        }
      }
      for (int index = 0; index < this.projectiles.Count; ++index)
        this.projectiles[index].draw(b);
      for (int index = 0; index < this.farmers.Count; ++index)
      {
        if (!this.farmers[index].uniqueMultiplayerID.Equals(Game1.player.uniqueMultiplayerID))
          this.farmers[index].draw(b);
      }
      if (this.critters != null)
      {
        for (int index = 0; index < this.critters.Count; ++index)
          this.critters[index].draw(b);
      }
      this.drawDebris(b);
      if (!Game1.eventUp || this.currentEvent != null && this.currentEvent.showGroundObjects)
      {
        foreach (KeyValuePair<Vector2, Object> keyValuePair in (Dictionary<Vector2, Object>) this.objects)
          keyValuePair.Value.draw(b, (int) keyValuePair.Key.X, (int) keyValuePair.Key.Y, 1f);
      }
      foreach (TemporaryAnimatedSprite temporarySprite in this.TemporarySprites)
        temporarySprite.draw(b, false, 0, 0);
      if (this.doorSprites != null)
      {
        foreach (KeyValuePair<Point, TemporaryAnimatedSprite> doorSprite in this.doorSprites)
          doorSprite.Value.draw(b, false, 0, 0);
      }
      if (this.largeTerrainFeatures != null)
      {
        foreach (LargeTerrainFeature largeTerrainFeature in this.largeTerrainFeatures)
          largeTerrainFeature.draw(b);
      }
      if (this.fishSplashAnimation != null)
        this.fishSplashAnimation.draw(b, false, 0, 0);
      if (this.orePanAnimation == null)
        return;
      this.orePanAnimation.draw(b, false, 0, 0);
    }

    public virtual void drawAboveFrontLayer(SpriteBatch b)
    {
      if (!Game1.isFestival())
      {
        Vector2 vector2 = new Vector2();
        for (int index1 = Game1.viewport.Y / Game1.tileSize - 1; index1 < (Game1.viewport.Y + Game1.viewport.Height) / Game1.tileSize + 7; ++index1)
        {
          for (int index2 = Game1.viewport.X / Game1.tileSize - 1; index2 < (Game1.viewport.X + Game1.viewport.Width) / Game1.tileSize + 3; ++index2)
          {
            vector2.X = (float) index2;
            vector2.Y = (float) index1;
            TerrainFeature terrainFeature;
            if (this.terrainFeatures.TryGetValue(vector2, out terrainFeature))
              terrainFeature.draw(b, vector2);
          }
        }
      }
      if (this.lightGlows.Count <= 0)
        return;
      this.drawLightGlows(b);
    }

    public virtual void drawLightGlows(SpriteBatch b)
    {
      foreach (Vector2 lightGlow in this.lightGlows)
        b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, lightGlow), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(21, 1695, 41, 67)), Color.White, 0.0f, new Vector2(19f, 22f), (float) Game1.pixelZoom, SpriteEffects.None, 1f);
    }

    public virtual bool performToolAction(Tool t, int tileX, int tileY)
    {
      return false;
    }

    public virtual void seasonUpdate(string season, bool onLoad = false)
    {
      for (int index = this.terrainFeatures.Count - 1; index >= 0; --index)
      {
        if (this.terrainFeatures.Values.ElementAt<TerrainFeature>(index).seasonUpdate(onLoad))
          this.terrainFeatures.Remove(this.terrainFeatures.Keys.ElementAt<Vector2>(index));
      }
      if (this.largeTerrainFeatures != null)
      {
        for (int index = this.largeTerrainFeatures.Count - 1; index >= 0; --index)
        {
          if (this.largeTerrainFeatures.ElementAt<LargeTerrainFeature>(index).seasonUpdate(onLoad))
            this.largeTerrainFeatures.Remove(this.largeTerrainFeatures.ElementAt<LargeTerrainFeature>(index));
        }
      }
      foreach (NPC character in this.getCharacters())
      {
        if (!character.IsMonster)
          character.loadSeasonalDialogue();
      }
      if (this.IsOutdoors && !onLoad)
      {
        for (int index = this.objects.Count - 1; index >= 0; --index)
        {
          KeyValuePair<Vector2, Object> keyValuePair = this.objects.ElementAt<KeyValuePair<Vector2, Object>>(index);
          if (keyValuePair.Value.IsSpawnedObject)
          {
            keyValuePair = this.objects.ElementAt<KeyValuePair<Vector2, Object>>(index);
            if (!keyValuePair.Value.Name.Equals("Stone"))
            {
              SerializableDictionary<Vector2, Object> objects = this.objects;
              keyValuePair = this.objects.ElementAt<KeyValuePair<Vector2, Object>>(index);
              Vector2 key = keyValuePair.Key;
              objects.Remove(key);
            }
          }
        }
        this.numberOfSpawnedObjectsOnMap = 0;
      }
      string lower = season.ToLower();
      if (!(lower == "spring"))
      {
        if (!(lower == "summer"))
        {
          if (!(lower == "fall"))
          {
            if (!(lower == "winter"))
              return;
            this.waterColor = new Color(130, 80, (int) byte.MaxValue) * 0.5f;
          }
          else
            this.waterColor = new Color((int) byte.MaxValue, 130, 200) * 0.5f;
        }
        else
          this.waterColor = new Color(60, 240, (int) byte.MaxValue) * 0.5f;
      }
      else
        this.waterColor = new Color(120, 200, (int) byte.MaxValue) * 0.5f;
    }

    private int checkEventPrecondition(string precondition)
    {
      string[] strArray1 = precondition.Split(GameLocation.ForwardSlash);
      int result;
      if (!int.TryParse(strArray1[0], out result) || Game1.player.eventsSeen.Contains(result))
        return -1;
      for (int index1 = 1; index1 < strArray1.Length; ++index1)
      {
        if ((int) strArray1[index1][0] == 101)
        {
          if (this.checkEventsSeenPreconditions(strArray1[index1].Split(' ')))
            return -1;
        }
        else
        {
          if ((int) strArray1[index1][0] == 104)
          {
            if (Game1.player.hasPet())
              return -1;
            if (Game1.player.catPerson)
            {
              if (!strArray1[index1].Split(' ')[1].ToString().ToLower().Equals("cat"))
                goto label_13;
            }
            if (!Game1.player.catPerson)
            {
              if (strArray1[index1].Split(' ')[1].ToString().ToLower().Equals("dog"))
                continue;
            }
            else
              continue;
label_13:
            return -1;
          }
          if ((int) strArray1[index1][0] == 109)
          {
            if ((long) Game1.player.totalMoneyEarned < (long) Convert.ToInt32(strArray1[index1].Split(' ')[1]))
              return -1;
          }
          else if ((int) strArray1[index1][0] == 77)
          {
            if (Game1.player.money < Convert.ToInt32(strArray1[index1].Split(' ')[1]))
              return -1;
          }
          else if ((int) strArray1[index1][0] == 99)
          {
            if (Game1.player.freeSpotsInInventory() < Convert.ToInt32(strArray1[index1].Split(' ')[1]))
              return -1;
          }
          else if ((int) strArray1[index1][0] == 67)
          {
            if (!Game1.player.eventsSeen.Contains(191393) && !Game1.player.eventsSeen.Contains(502261) && !Game1.player.hasCompletedCommunityCenter())
              return -1;
          }
          else if ((int) strArray1[index1][0] == 68)
          {
            if (!Game1.getCharacterFromName(strArray1[index1].Split(' ')[1], true).datingFarmer)
              return -1;
          }
          else if ((int) strArray1[index1][0] == 106)
          {
            if ((long) Game1.stats.DaysPlayed <= (long) Convert.ToInt32(strArray1[index1].Split(' ')[1]))
              return -1;
          }
          else if ((int) strArray1[index1][0] == 74)
          {
            if (!this.checkJojaCompletePrerequisite())
              return -1;
          }
          else if ((int) strArray1[index1][0] == 102)
          {
            if (!this.checkFriendshipPrecondition(strArray1[index1].Split(' ')))
              return -1;
          }
          else if ((int) strArray1[index1][0] == 70)
          {
            if (Utility.isFestivalDay(Game1.dayOfMonth, Game1.currentSeason))
              return -1;
          }
          else if ((int) strArray1[index1][0] == 114)
          {
            string[] strArray2 = strArray1[index1].Split(' ');
            if (Game1.random.NextDouble() > Convert.ToDouble(strArray2[1]))
              return -1;
          }
          else if ((int) strArray1[index1][0] == 115)
          {
            if (!this.checkItemsPrecondition(strArray1[index1].Split(' ')))
              return -1;
          }
          else if ((int) strArray1[index1][0] == 113)
          {
            if (!this.checkDialoguePrecondition(strArray1[index1].Split(' ')))
              return -1;
          }
          else if ((int) strArray1[index1][0] == 110)
          {
            if (!Game1.player.mailReceived.Contains(strArray1[index1].Split(' ')[1]))
              return -1;
          }
          else if ((int) strArray1[index1][0] == 108)
          {
            if (Game1.player.mailReceived.Contains(strArray1[index1].Split(' ')[1]))
              return -1;
          }
          else if ((int) strArray1[index1][0] == 116)
          {
            string[] strArray2 = strArray1[index1].Split(' ');
            if (Game1.timeOfDay < Convert.ToInt32(strArray2[1]) || Game1.timeOfDay > Convert.ToInt32(strArray2[2]))
              return -1;
          }
          else if ((int) strArray1[index1][0] == 119)
          {
            string[] strArray2 = strArray1[index1].Split(' ');
            if (strArray2[1].Equals("rainy") && !Game1.isRaining || strArray2[1].Equals("sunny") && Game1.isRaining)
              return -1;
          }
          else if ((int) strArray1[index1][0] == 100)
          {
            if (((IEnumerable<string>) strArray1[index1].Split(' ')).Contains<string>(Game1.shortDayNameFromDayOfSeason(Game1.dayOfMonth)))
              return -1;
          }
          else if ((int) strArray1[index1][0] == 111)
          {
            if (Game1.player.spouse != null)
            {
              if (Game1.player.spouse.Equals(strArray1[index1].Split(' ')[1]))
                return -1;
            }
          }
          else if ((int) strArray1[index1][0] == 118)
          {
            if (Game1.getCharacterFromName(strArray1[index1].Split(' ')[1], false).isInvisible)
              return -1;
          }
          else if ((int) strArray1[index1][0] == 112)
          {
            if (!this.isCharacterHere(strArray1[index1].Split(' ')[1]))
              return -1;
          }
          else if ((int) strArray1[index1][0] == 122)
          {
            string[] strArray2 = strArray1[index1].Split(' ');
            if (Game1.currentSeason.Equals(strArray2[1]))
              return -1;
          }
          else if ((int) strArray1[index1][0] == 98)
          {
            if (Game1.player.timesReachedMineBottom < Convert.ToInt32(strArray1[index1].Split(' ')[1]))
              return -1;
          }
          else
          {
            if ((int) strArray1[index1][0] == 121)
            {
              if (Game1.year >= Convert.ToInt32(strArray1[index1].Split(' ')[1]))
              {
                if (Convert.ToInt32(strArray1[index1].Split(' ')[1]) != 1 || Game1.year == 1)
                  continue;
              }
              return -1;
            }
            if ((int) strArray1[index1][0] == 103)
            {
              if (!(Game1.player.isMale ? "male" : "female").Equals(strArray1[index1].Split(' ')[1].ToLower()))
                return -1;
            }
            else if ((int) strArray1[index1][0] == 105)
            {
              if (!Game1.player.hasItemInInventory(Convert.ToInt32(strArray1[index1].Split(' ')[1]), 1, 0))
              {
                if (Game1.player.ActiveObject != null)
                {
                  if (Game1.player.ActiveObject.ParentSheetIndex == Convert.ToInt32(strArray1[index1].Split(' ')[1]))
                    continue;
                }
                return -1;
              }
            }
            else if ((int) strArray1[index1][0] == 107)
            {
              if (!this.checkEventsSeenPreconditions(strArray1[index1].Split(' ')))
                return -1;
            }
            else
            {
              if ((int) strArray1[index1][0] == 97)
              {
                if ((double) Game1.player.getTileLocation().X == (double) Convert.ToInt32(strArray1[index1].Split(' ')[1]))
                {
                  if ((double) Game1.player.getTileLocation().Y == (double) Convert.ToInt32(strArray1[index1].Split(' ')[2]))
                    continue;
                }
                return -1;
              }
              if ((int) strArray1[index1][0] == 120)
              {
                Game1.addMailForTomorrow(strArray1[index1].Split(' ')[1], false, false);
                Game1.player.eventsSeen.Add(Convert.ToInt32(strArray1[0]));
                return -1;
              }
              if ((int) strArray1[index1][0] != 117)
                return -1;
              bool flag = false;
              string[] strArray2 = strArray1[index1].Split(' ');
              for (int index2 = 1; index2 < strArray2.Length; ++index2)
              {
                if (Game1.dayOfMonth == Convert.ToInt32(strArray2[index2]))
                {
                  flag = true;
                  break;
                }
              }
              if (!flag)
                return -1;
            }
          }
        }
      }
      return result;
    }

    private bool isCharacterHere(string name)
    {
      foreach (Character character in this.characters)
      {
        if (character.name.Equals(name))
          return true;
      }
      return false;
    }

    private bool checkJojaCompletePrerequisite()
    {
      bool flag = false;
      if (Game1.player.mailReceived.Contains("jojaVault"))
        flag = true;
      else if (!Game1.player.mailReceived.Contains("ccVault"))
        return false;
      if (Game1.player.mailReceived.Contains("jojaPantry"))
        flag = true;
      else if (!Game1.player.mailReceived.Contains("ccPantry"))
        return false;
      if (Game1.player.mailReceived.Contains("jojaBoilerRoom"))
        flag = true;
      else if (!Game1.player.mailReceived.Contains("ccBoilerRoom"))
        return false;
      if (Game1.player.mailReceived.Contains("jojaCraftsRoom"))
        flag = true;
      else if (!Game1.player.mailReceived.Contains("ccCraftsRoom"))
        return false;
      if (Game1.player.mailReceived.Contains("jojaFishTank"))
        flag = true;
      else if (!Game1.player.mailReceived.Contains("ccFishTank"))
        return false;
      return flag || Game1.player.mailReceived.Contains("JojaMember");
    }

    private bool checkEventsSeenPreconditions(string[] eventIDs)
    {
      for (int index = 1; index < eventIDs.Length; ++index)
      {
        int result;
        if (int.TryParse(eventIDs[index], out result) && Game1.player.eventsSeen.Contains(Convert.ToInt32(eventIDs[index])))
          return false;
      }
      return true;
    }

    private bool checkFriendshipPrecondition(string[] friendshipString)
    {
      int index = 1;
      while (index < friendshipString.Length)
      {
        if (!Game1.player.friendships.ContainsKey(friendshipString[index]) || Game1.player.friendships[friendshipString[index]][0] < Convert.ToInt32(friendshipString[index + 1]))
          return false;
        index += 2;
      }
      return true;
    }

    private bool checkItemsPrecondition(string[] itemString)
    {
      int index = 1;
      while (index < itemString.Length)
      {
        if (!Game1.player.basicShipped.ContainsKey(Convert.ToInt32(itemString[index])) || Game1.player.basicShipped[Convert.ToInt32(itemString[index])] < Convert.ToInt32(itemString[index + 1]))
          return false;
        index += 2;
      }
      return true;
    }

    private bool checkDialoguePrecondition(string[] dialogueString)
    {
      int index = 1;
      while (index < dialogueString.Length)
      {
        if (!Game1.player.DialogueQuestionsAnswered.Contains(Convert.ToInt32(dialogueString[index])))
          return false;
        index += 2;
      }
      return true;
    }

    public void loadObjects()
    {
      if (this.map == null)
        return;
      this.warps.Clear();
      PropertyValue propertyValue1;
      this.map.Properties.TryGetValue("Warp", out propertyValue1);
      if (propertyValue1 != null)
      {
        string[] strArray = propertyValue1.ToString().Split(' ');
        int index = 0;
        while (index < strArray.Length)
        {
          this.warps.Add(new Warp(Convert.ToInt32(strArray[index]), Convert.ToInt32(strArray[index + 1]), strArray[index + 2], Convert.ToInt32(strArray[index + 3]), Convert.ToInt32(strArray[index + 4]), false));
          index += 5;
        }
      }
      PropertyValue propertyValue2;
      this.map.Properties.TryGetValue("Outdoors", out propertyValue2);
      if (propertyValue2 != null)
        this.isOutdoors = true;
      if (this.isOutdoors)
        this.largeTerrainFeatures = new List<LargeTerrainFeature>();
      PropertyValue propertyValue3;
      this.map.Properties.TryGetValue("TreatAsOutdoors", out propertyValue3);
      if (propertyValue3 != null)
        this.treatAsOutdoors = true;
      PropertyValue propertyValue4;
      this.map.Properties.TryGetValue(Game1.currentSeason.Substring(0, 1).ToUpper() + Game1.currentSeason.Substring(1) + "_Objects", out propertyValue4);
      if (propertyValue4 != null && !Game1.eventUp)
        this.spawnObjects();
      bool flag = false;
      foreach (Component layer in this.map.Layers)
      {
        if (layer.Id.Equals("Paths"))
        {
          flag = true;
          break;
        }
      }
      PropertyValue propertyValue5;
      this.map.Properties.TryGetValue("Trees", out propertyValue5);
      if (propertyValue5 != null)
      {
        string[] strArray = propertyValue5.ToString().Split(' ');
        int index = 0;
        while (index < strArray.Length)
        {
          this.terrainFeatures.Add(new Vector2((float) Convert.ToInt32(strArray[index]), (float) Convert.ToInt32(strArray[index + 1])), (TerrainFeature) new Tree(Convert.ToInt32(strArray[index + 2]) + 1, 5));
          index += 3;
        }
      }
      if (((this.isOutdoors || this.name.Equals("BathHouse_Entry") ? 1 : (this.treatAsOutdoors ? 1 : 0)) & (flag ? 1 : 0)) != 0)
      {
        for (int x = 0; x < this.map.Layers[0].LayerWidth; ++x)
        {
          for (int y = 0; y < this.map.Layers[0].LayerHeight; ++y)
          {
            Tile tile = this.map.GetLayer("Paths").Tiles[x, y];
            if (tile != null)
            {
              Vector2 vector2 = new Vector2((float) x, (float) y);
              int which = -1;
              switch (tile.TileIndex)
              {
                case 9:
                  which = 1;
                  if (Game1.currentSeason.Equals("winter"))
                  {
                    which += 3;
                    break;
                  }
                  break;
                case 10:
                  which = 2;
                  if (Game1.currentSeason.Equals("winter"))
                  {
                    which += 3;
                    break;
                  }
                  break;
                case 11:
                  which = 3;
                  break;
                case 12:
                  which = 6;
                  break;
              }
              if (which != -1)
              {
                if (!this.terrainFeatures.ContainsKey(vector2) && !this.objects.ContainsKey(vector2))
                  this.terrainFeatures.Add(vector2, (TerrainFeature) new Tree(which, 5));
              }
              else
              {
                switch (tile.TileIndex)
                {
                  case 13:
                  case 14:
                  case 15:
                    if (!this.objects.ContainsKey(vector2))
                    {
                      this.objects.Add(vector2, new Object(vector2, GameLocation.getWeedForSeason(Game1.random, Game1.currentSeason), 1));
                      break;
                    }
                    break;
                  case 16:
                    if (!this.objects.ContainsKey(vector2))
                    {
                      this.objects.Add(vector2, new Object(vector2, Game1.random.NextDouble() < 0.5 ? 343 : 450, 1));
                      break;
                    }
                    break;
                  case 17:
                    if (!this.objects.ContainsKey(vector2))
                    {
                      this.objects.Add(vector2, new Object(vector2, Game1.random.NextDouble() < 0.5 ? 343 : 450, 1));
                      break;
                    }
                    break;
                  case 18:
                    if (!this.objects.ContainsKey(vector2))
                    {
                      this.objects.Add(vector2, new Object(vector2, Game1.random.NextDouble() < 0.5 ? 294 : 295, 1));
                      break;
                    }
                    break;
                  case 19:
                    if (this is Farm)
                    {
                      (this as Farm).addResourceClumpAndRemoveUnderlyingTerrain(602, 2, 2, vector2);
                      break;
                    }
                    break;
                  case 20:
                    if (this is Farm)
                    {
                      (this as Farm).addResourceClumpAndRemoveUnderlyingTerrain(672, 2, 2, vector2);
                      break;
                    }
                    break;
                  case 21:
                    if (this is Farm)
                    {
                      (this as Farm).addResourceClumpAndRemoveUnderlyingTerrain(600, 2, 2, vector2);
                      break;
                    }
                    break;
                  case 22:
                    if (!this.terrainFeatures.ContainsKey(vector2))
                    {
                      this.terrainFeatures.Add(vector2, (TerrainFeature) new Grass(1, 3));
                      break;
                    }
                    break;
                  case 23:
                    if (!this.terrainFeatures.ContainsKey(vector2))
                    {
                      this.terrainFeatures.Add(vector2, (TerrainFeature) new Tree(Game1.random.Next(1, 4), Game1.random.Next(2, 4)));
                      break;
                    }
                    break;
                  case 24:
                    if (!this.terrainFeatures.ContainsKey(vector2))
                    {
                      this.largeTerrainFeatures.Add((LargeTerrainFeature) new Bush(vector2, 2, this));
                      break;
                    }
                    break;
                  case 25:
                    if (!this.terrainFeatures.ContainsKey(vector2))
                    {
                      this.largeTerrainFeatures.Add((LargeTerrainFeature) new Bush(vector2, 1, this));
                      break;
                    }
                    break;
                  case 26:
                    if (!this.terrainFeatures.ContainsKey(vector2))
                    {
                      this.largeTerrainFeatures.Add((LargeTerrainFeature) new Bush(vector2, 0, this));
                      break;
                    }
                    break;
                  case 27:
                    this.changeMapProperties("BrookSounds", ((double) vector2.X).ToString() + " " + (object) vector2.Y + " 0");
                    break;
                  case 28:
                    if (this.name == "BugLand")
                    {
                      this.characters.Add((NPC) new Grub(new Vector2(vector2.X * (float) Game1.tileSize, vector2.Y * (float) Game1.tileSize), true));
                      break;
                    }
                    break;
                }
              }
            }
            if (this.map.GetLayer("Buildings").Tiles[x, y] != null)
            {
              PropertyValue propertyValue6 = (PropertyValue) null;
              this.map.GetLayer("Buildings").Tiles[x, y].Properties.TryGetValue("Action", out propertyValue6);
              if (propertyValue6 != null && propertyValue6.ToString().Contains("Warp"))
              {
                string[] strArray = propertyValue6.ToString().Split(' ');
                if (strArray[0].Equals("WarpCommunityCenter"))
                  this.doors.Add(new Point(x, y), "CommunityCenter");
                else if ((!this.name.Equals("Mountain") || x != 8 || y != 20) && strArray.Length > 2)
                  this.doors.Add(new Point(x, y), strArray[3]);
              }
            }
          }
        }
      }
      this.loadLights();
    }

    public bool isTerrainFeatureAt(int x, int y)
    {
      Vector2 key = new Vector2((float) x, (float) y);
      if (this.terrainFeatures.ContainsKey(key) && !this.terrainFeatures[key].isPassable((Character) null))
        return true;
      if (this.largeTerrainFeatures != null)
      {
        Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle(x * Game1.tileSize, y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
        foreach (LargeTerrainFeature largeTerrainFeature in this.largeTerrainFeatures)
        {
          if (largeTerrainFeature.getBoundingBox().Intersects(rectangle))
            return true;
        }
      }
      return false;
    }

    public void loadLights()
    {
      if (this.isOutdoors && !Game1.isFestival() || this is FarmHouse)
        return;
      bool flag = false;
      foreach (Component layer in this.map.Layers)
      {
        if (layer.Id.Equals("Paths"))
        {
          flag = true;
          break;
        }
      }
      if (this.doorSprites == null)
        this.doorSprites = new Dictionary<Point, TemporaryAnimatedSprite>();
      else
        this.doorSprites.Clear();
      for (int x = 0; x < this.map.Layers[0].LayerWidth; ++x)
      {
        for (int y = 0; y < this.map.Layers[0].LayerHeight; ++y)
        {
          if (!this.isOutdoors)
          {
            Tile tile1 = this.map.GetLayer("Front").Tiles[x, y];
            if (tile1 != null)
              this.adjustMapLightPropertiesForLamp(tile1.TileIndex, x, y, "Front");
            Tile tile2 = this.map.GetLayer("Buildings").Tiles[x, y];
            if (tile2 != null)
            {
              this.adjustMapLightPropertiesForLamp(tile2.TileIndex, x, y, "Buildings");
              PropertyValue propertyValue = (PropertyValue) null;
              this.map.GetLayer("Buildings").Tiles[x, y].Properties.TryGetValue("Action", out propertyValue);
              if (propertyValue != null && propertyValue.ToString().Contains("Door"))
              {
                int tileIndex = this.map.GetLayer("Buildings").Tiles[x, y].TileIndex;
                Microsoft.Xna.Framework.Rectangle sourceRect = new Microsoft.Xna.Framework.Rectangle();
                bool flipped = false;
                if (tileIndex <= 824)
                {
                  if (tileIndex != 120)
                  {
                    if (tileIndex == 824)
                      sourceRect = new Microsoft.Xna.Framework.Rectangle(640, 144, 16, 48);
                  }
                  else
                    sourceRect = new Microsoft.Xna.Framework.Rectangle(512, 144, 16, 48);
                }
                else if (tileIndex != 825)
                {
                  if (tileIndex == 838)
                  {
                    sourceRect = new Microsoft.Xna.Framework.Rectangle(576, 144, 16, 48);
                    if (x == 10 && y == 5)
                      flipped = true;
                  }
                }
                else
                {
                  sourceRect = new Microsoft.Xna.Framework.Rectangle(640, 144, 16, 48);
                  flipped = true;
                }
                this.doorSprites.Add(new Point(x, y), new TemporaryAnimatedSprite(Game1.mouseCursors, sourceRect, 100f, 4, 1, new Vector2((float) x, (float) (y - 2)) * (float) Game1.tileSize, false, flipped, (float) ((y + 1) * Game1.tileSize - Game1.pixelZoom * 3) / 10000f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
                {
                  holdLastFrame = true,
                  paused = true,
                  endSound = propertyValue.ToString().Split(' ').Length > 1 ? propertyValue.ToString().Substring(propertyValue.ToString().IndexOf(' ') + 1) : (string) null
                });
                if (propertyValue.ToString().Split(' ').Length > 1 && !this.map.GetLayer("Back").Tiles[x, y].Properties.ContainsKey("TouchAction"))
                  this.map.GetLayer("Back").Tiles[x, y].Properties.Add("TouchAction", new PropertyValue("Door " + propertyValue.ToString().Substring(propertyValue.ToString().IndexOf(' ') + 1)));
              }
            }
          }
          if (flag)
          {
            Tile tile = this.map.GetLayer("Paths").Tiles[x, y];
            if (tile != null)
              this.adjustMapLightPropertiesForLamp(tile.TileIndex, x, y, "Paths");
          }
        }
      }
    }

    public bool isFarmBuildingInterior()
    {
      return this is AnimalHouse;
    }

    protected void adjustMapLightPropertiesForLamp(int tile, int x, int y, string layer)
    {
      if (this.isFarmBuildingInterior())
      {
        if (tile != 24)
        {
          if (tile != 25)
          {
            if (tile != 46)
              return;
            this.changeMapProperties("DayTiles", layer + " " + (object) x + " " + (object) y + " " + (object) tile);
            this.changeMapProperties("NightTiles", layer + " " + (object) x + " " + (object) y + " " + (object) 53);
          }
          else
          {
            this.changeMapProperties("DayTiles", layer + " " + (object) x + " " + (object) y + " " + (object) tile);
            this.changeMapProperties("NightTiles", layer + " " + (object) x + " " + (object) y + " " + (object) 12);
          }
        }
        else
        {
          this.changeMapProperties("DayTiles", layer + " " + (object) x + " " + (object) y + " " + (object) tile);
          this.changeMapProperties("NightTiles", layer + " " + (object) x + " " + (object) y + " " + (object) 26);
          this.changeMapProperties("Light", x.ToString() + " " + (object) (y + 1) + " 4");
          this.changeMapProperties("Light", x.ToString() + " " + (object) (y + 3) + " 4");
        }
      }
      else if (tile <= 256)
      {
        if (tile != 8)
        {
          if (tile != 225)
          {
            if (tile != 256)
              return;
            this.changeMapProperties("DayTiles", layer + " " + (object) x + " " + (object) y + " " + (object) tile);
            this.changeMapProperties("NightTiles", layer + " " + (object) x + " " + (object) y + " " + (object) 1253);
            this.changeMapProperties("DayTiles", layer + " " + (object) x + " " + (object) (y + 1) + " " + (object) 288);
            this.changeMapProperties("NightTiles", layer + " " + (object) x + " " + (object) (y + 1) + " " + (object) 1285);
            this.changeMapProperties("Light", x.ToString() + " " + (object) y + " 4");
            this.changeMapProperties("Light", x.ToString() + " " + (object) (y + 1) + " 4");
          }
          else
          {
            if (this.name.Contains("BathHouse") || this.name.Contains("Club"))
              return;
            this.changeMapProperties("DayTiles", layer + " " + (object) x + " " + (object) y + " " + (object) tile);
            this.changeMapProperties("NightTiles", layer + " " + (object) x + " " + (object) y + " " + (object) 1222);
            this.changeMapProperties("DayTiles", layer + " " + (object) x + " " + (object) (y + 1) + " " + (object) 257);
            this.changeMapProperties("NightTiles", layer + " " + (object) x + " " + (object) (y + 1) + " " + (object) 1254);
            this.changeMapProperties("Light", x.ToString() + " " + (object) y + " 4");
            this.changeMapProperties("Light", x.ToString() + " " + (object) (y + 1) + " 4");
          }
        }
        else
          this.changeMapProperties("Light", x.ToString() + " " + (object) y + " 4");
      }
      else if (tile <= 826)
      {
        if (tile != 480)
        {
          if (tile != 826)
            return;
          this.changeMapProperties("DayTiles", layer + " " + (object) x + " " + (object) y + " " + (object) tile);
          this.changeMapProperties("NightTiles", layer + " " + (object) x + " " + (object) y + " " + (object) 827);
          this.changeMapProperties("Light", x.ToString() + " " + (object) y + " 4");
        }
        else
        {
          this.changeMapProperties("DayTiles", layer + " " + (object) x + " " + (object) y + " " + (object) tile);
          this.changeMapProperties("NightTiles", layer + " " + (object) x + " " + (object) y + " " + (object) 809);
          this.changeMapProperties("Light", x.ToString() + " " + (object) y + " 4");
        }
      }
      else if (tile != 1344)
      {
        if (tile != 1346)
          return;
        this.changeMapProperties("DayTiles", "Front " + (object) x + " " + (object) y + " " + (object) tile);
        this.changeMapProperties("NightTiles", "Front " + (object) x + " " + (object) y + " " + (object) 1347);
        this.changeMapProperties("DayTiles", "Buildings " + (object) x + " " + (object) (y + 1) + " " + (object) 452);
        this.changeMapProperties("NightTiles", "Buildings " + (object) x + " " + (object) (y + 1) + " " + (object) 453);
        this.changeMapProperties("Light", x.ToString() + " " + (object) y + " 4");
      }
      else
      {
        this.changeMapProperties("DayTiles", layer + " " + (object) x + " " + (object) y + " " + (object) tile);
        this.changeMapProperties("NightTiles", layer + " " + (object) x + " " + (object) y + " " + (object) 1345);
        this.changeMapProperties("Light", x.ToString() + " " + (object) y + " 4");
      }
    }

    private void changeMapProperties(string propertyName, string toAdd)
    {
      try
      {
        bool flag = true;
        if (!this.map.Properties.ContainsKey(propertyName))
        {
          this.map.Properties.Add(propertyName, new PropertyValue(string.Empty));
          flag = false;
        }
        if (this.map.Properties[propertyName].ToString().Contains(toAdd))
          return;
        StringBuilder stringBuilder = new StringBuilder(this.map.Properties[propertyName].ToString());
        if (flag)
          stringBuilder.Append(" ");
        stringBuilder.Append(toAdd);
        this.map.Properties[propertyName] = new PropertyValue(stringBuilder.ToString());
      }
      catch (Exception ex)
      {
      }
    }

    public delegate void afterQuestionBehavior(Farmer who, string whichAnswer);
  }
}
