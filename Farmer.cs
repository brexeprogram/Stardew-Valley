// Decompiled with JetBrains decompiler
// Type: StardewValley.Farmer
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Characters;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Objects;
using StardewValley.Quests;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using xTile.Dimensions;
using xTile.ObjectModel;
using xTile.Tiles;

namespace StardewValley
{
  public class Farmer : Character, IComparable
  {
    public static int tileSlideThreshold = Game1.tileSize / 2;
    public List<Quest> questLog = new List<Quest>();
    public List<int> professions = new List<int>();
    public List<Point> newLevels = new List<Point>();
    private Queue<int> newLevelSparklingTexts = new Queue<int>();
    public int[] experiencePoints = new int[6];
    public List<int> dialogueQuestionsAnswered = new List<int>();
    public List<string> furnitureOwned = new List<string>();
    public SerializableDictionary<string, int> cookingRecipes = new SerializableDictionary<string, int>();
    public SerializableDictionary<string, int> craftingRecipes = new SerializableDictionary<string, int>();
    public SerializableDictionary<string, int> activeDialogueEvents = new SerializableDictionary<string, int>();
    public List<int> eventsSeen = new List<int>();
    public List<string> songsHeard = new List<string>();
    public List<int> achievements = new List<int>();
    public List<int> specialItems = new List<int>();
    public List<int> specialBigCraftables = new List<int>();
    public List<string> mailReceived = new List<string>();
    public List<string> mailForTomorrow = new List<string>();
    public List<string> blueprints = new List<string>();
    public List<CoopDweller> coopDwellers = new List<CoopDweller>();
    public List<BarnDweller> barnDwellers = new List<BarnDweller>();
    public Tool[] toolBox = new Tool[30];
    public Object[] cupboard = new Object[30];
    [XmlIgnore]
    public List<int> movementDirections = new List<int>();
    public string farmName = "";
    public string favoriteThing = "";
    [XmlIgnore]
    public List<Buff> buffs = new List<Buff>();
    [XmlIgnore]
    public List<object[]> multiplayerMessage = new List<object[]>();
    [XmlIgnore]
    public GameLocation currentLocation = Game1.getLocationFromName("FarmHouse");
    [XmlIgnore]
    public long uniqueMultiplayerID = -6666666;
    [XmlIgnore]
    public string _tmpLocationName = "FarmHouse";
    [XmlIgnore]
    public string previousLocationName = "";
    public bool catPerson = true;
    public int accessory = -1;
    public int facialHair = -1;
    public int maxStamina = 270;
    public int maxItems = 12;
    public float stamina = 270f;
    public int money = 500;
    public int daysUntilHouseUpgrade = -1;
    public bool showChestColorPicker = true;
    public int magneticRadius = Game1.tileSize * 2;
    private int craftingTime = 1000;
    private int raftPuddleCounter = 250;
    private int raftBobCounter = 1000;
    public int health = 100;
    public int maxHealth = 100;
    [XmlIgnore]
    public Vector2 jitter = Vector2.Zero;
    [XmlIgnore]
    public Vector2 lastGrabTile = Vector2.Zero;
    public bool isMale = true;
    [XmlIgnore]
    public bool canMove = true;
    [XmlIgnore]
    public Microsoft.Xna.Framework.Rectangle temporaryImpassableTile = Microsoft.Xna.Framework.Rectangle.Empty;
    public string bobber = "";
    public float movementMultiplier = 0.01f;
    public const int millisecondsPerSpeedUnit = 64;
    public const byte halt = 64;
    public const byte up = 1;
    public const byte right = 2;
    public const byte down = 4;
    public const byte left = 8;
    public const byte run = 16;
    public const byte release = 32;
    public const int FESTIVAL_WINNER = -9999;
    public const int farmingSkill = 0;
    public const int miningSkill = 3;
    public const int fishingSkill = 1;
    public const int foragingSkill = 2;
    public const int combatSkill = 4;
    public const int luckSkill = 5;
    public const float interpolationConstant = 0.5f;
    public const int runningSpeed = 5;
    public const int walkingSpeed = 2;
    public const int caveNothing = 0;
    public const int caveBats = 1;
    public const int caveMushrooms = 2;
    public const int millisecondsInvincibleAfterDamage = 1200;
    public const int millisecondsPerFlickerWhenInvincible = 50;
    public const int startingStamina = 270;
    public const int totalLevels = 35;
    public const int maxInventorySpace = 36;
    public const int hotbarSize = 12;
    public const int eyesOpen = 0;
    public const int eyesHalfShut = 4;
    public const int eyesClosed = 1;
    public const int eyesRight = 2;
    public const int eyesLeft = 3;
    public const int eyesWide = 5;
    public const int rancher = 0;
    public const int tiller = 1;
    public const int butcher = 2;
    public const int shepherd = 3;
    public const int artisan = 4;
    public const int agriculturist = 5;
    public const int fisher = 6;
    public const int trapper = 7;
    public const int angler = 8;
    public const int pirate = 9;
    public const int baitmaster = 10;
    public const int mariner = 11;
    public const int forester = 12;
    public const int gatherer = 13;
    public const int lumberjack = 14;
    public const int tapper = 15;
    public const int botanist = 16;
    public const int tracker = 17;
    public const int miner = 18;
    public const int geologist = 19;
    public const int blacksmith = 20;
    public const int burrower = 21;
    public const int excavator = 22;
    public const int gemologist = 23;
    public const int fighter = 24;
    public const int scout = 25;
    public const int brute = 26;
    public const int defender = 27;
    public const int acrobat = 28;
    public const int desperado = 29;
    private SparklingText sparklingText;
    [XmlIgnore]
    private Item activeObject;
    public List<Item> items;
    [XmlIgnore]
    public Item mostRecentlyGrabbedItem;
    [XmlIgnore]
    public Item itemToEat;
    private FarmerRenderer farmerRenderer;
    [XmlIgnore]
    public int toolPower;
    [XmlIgnore]
    public int toolHold;
    public Vector2 mostRecentBed;
    public int shirt;
    public int hair;
    public int skin;
    [XmlIgnore]
    public int currentEyes;
    [XmlIgnore]
    public int blinkTimer;
    [XmlIgnore]
    public int festivalScore;
    [XmlIgnore]
    public float temporarySpeedBuff;
    public Color hairstyleColor;
    public Color pantsColor;
    public Color newEyeColor;
    public Hat hat;
    public Boots boots;
    public Ring leftRing;
    public Ring rightRing;
    [XmlIgnore]
    public NPC dancePartner;
    [XmlIgnore]
    public bool ridingMineElevator;
    [XmlIgnore]
    public bool mineMovementDirectionWasUp;
    [XmlIgnore]
    public bool cameFromDungeon;
    [XmlIgnore]
    public bool readyConfirmation;
    [XmlIgnore]
    public bool exhausted;
    [XmlIgnore]
    public bool divorceTonight;
    [XmlIgnore]
    public AnimatedSprite.endOfAnimationBehavior toolOverrideFunction;
    public int deepestMineLevel;
    private int currentToolIndex;
    public int woodPieces;
    public int stonePieces;
    public int copperPieces;
    public int ironPieces;
    public int coalPieces;
    public int goldPieces;
    public int iridiumPieces;
    public int quartzPieces;
    public int caveChoice;
    public int feed;
    public int farmingLevel;
    public int miningLevel;
    public int combatLevel;
    public int foragingLevel;
    public int fishingLevel;
    public int luckLevel;
    public int newSkillPointsToSpend;
    public int addedFarmingLevel;
    public int addedMiningLevel;
    public int addedCombatLevel;
    public int addedForagingLevel;
    public int addedFishingLevel;
    public int addedLuckLevel;
    public int resilience;
    public int attack;
    public int immunity;
    public float attackIncreaseModifier;
    public float knockbackModifier;
    public float weaponSpeedModifier;
    public float critChanceModifier;
    public float critPowerModifier;
    public float weaponPrecisionModifier;
    public int clubCoins;
    public uint totalMoneyEarned;
    public uint millisecondsPlayed;
    public Tool toolBeingUpgraded;
    public int daysLeftForToolUpgrade;
    private float timeOfLastPositionPacket;
    private int numUpdatesSinceLastDraw;
    public int houseUpgradeLevel;
    public int coopUpgradeLevel;
    public int barnUpgradeLevel;
    public bool hasGreenhouse;
    public bool hasRustyKey;
    public bool hasSkullKey;
    public bool hasUnlockedSkullDoor;
    public bool hasDarkTalisman;
    public bool hasMagicInk;
    public int temporaryInvincibilityTimer;
    [XmlIgnore]
    public float rotation;
    public int timesReachedMineBottom;
    [XmlIgnore]
    public Vector2 lastPosition;
    [XmlIgnore]
    public float jitterStrength;
    [XmlIgnore]
    public float xOffset;
    [XmlIgnore]
    public bool running;
    [XmlIgnore]
    public bool usingTool;
    [XmlIgnore]
    public bool forceTimePass;
    [XmlIgnore]
    public bool isRafting;
    [XmlIgnore]
    public bool usingSlingshot;
    [XmlIgnore]
    public bool bathingClothes;
    [XmlIgnore]
    public bool canOnlyWalk;
    [XmlIgnore]
    public bool temporarilyInvincible;
    public bool hasBusTicket;
    public bool stardewHero;
    public bool hasClubCard;
    public bool hasSpecialCharm;
    [XmlIgnore]
    public bool canReleaseTool;
    [XmlIgnore]
    public bool isCrafting;
    public bool canUnderstandDwarves;
    public SerializableDictionary<int, int> basicShipped;
    public SerializableDictionary<int, int> mineralsFound;
    public SerializableDictionary<int, int> recipesCooked;
    public SerializableDictionary<int, int[]> archaeologyFound;
    public SerializableDictionary<int, int[]> fishCaught;
    public SerializableDictionary<string, int[]> friendships;
    [XmlIgnore]
    public Vector2 positionBeforeEvent;
    [XmlIgnore]
    public Vector2 remotePosition;
    [XmlIgnore]
    public int orientationBeforeEvent;
    [XmlIgnore]
    public int swimTimer;
    [XmlIgnore]
    public int timerSinceLastMovement;
    [XmlIgnore]
    public int noMovementPause;
    [XmlIgnore]
    public int freezePause;
    [XmlIgnore]
    public float yOffset;
    public BuildingUpgrade currentUpgrade;
    public string spouse;
    public string dateStringForSaveGame;
    public int? dayOfMonthForSaveGame;
    public int? seasonForSaveGame;
    public int? yearForSaveGame;
    public int overallsColor;
    public int shirtColor;
    public int skinColor;
    public int hairColor;
    public int eyeColor;
    [XmlIgnore]
    public Vector2 armOffset;
    private Horse mount;
    private LocalizedContentManager farmerTextureManager;
    public int saveTime;
    public int daysMarried;
    private int toolPitchAccumulator;
    private int charactercollisionTimer;
    private NPC collisionNPC;

    [XmlIgnore]
    public int MaxItems
    {
      get
      {
        return this.maxItems;
      }
      set
      {
        this.maxItems = value;
      }
    }

    [XmlIgnore]
    public int Level
    {
      get
      {
        return (this.farmingLevel + this.fishingLevel + this.foragingLevel + this.combatLevel + this.miningLevel + this.luckLevel) / 2;
      }
    }

    [XmlIgnore]
    public int CraftingTime
    {
      get
      {
        return this.craftingTime;
      }
      set
      {
        this.craftingTime = value;
      }
    }

    [XmlIgnore]
    public int NewSkillPointsToSpend
    {
      get
      {
        return this.newSkillPointsToSpend;
      }
      set
      {
        this.newSkillPointsToSpend = value;
      }
    }

    [XmlIgnore]
    public int FarmingLevel
    {
      get
      {
        return this.farmingLevel + this.addedFarmingLevel;
      }
      set
      {
        this.farmingLevel = value;
      }
    }

    [XmlIgnore]
    public int MiningLevel
    {
      get
      {
        return this.miningLevel + this.addedMiningLevel;
      }
      set
      {
        this.miningLevel = value;
      }
    }

    [XmlIgnore]
    public int CombatLevel
    {
      get
      {
        return this.combatLevel + this.addedCombatLevel;
      }
      set
      {
        this.combatLevel = value;
      }
    }

    [XmlIgnore]
    public int ForagingLevel
    {
      get
      {
        return this.foragingLevel + this.addedForagingLevel;
      }
      set
      {
        this.foragingLevel = value;
      }
    }

    [XmlIgnore]
    public int FishingLevel
    {
      get
      {
        return this.fishingLevel + this.addedFishingLevel;
      }
      set
      {
        this.fishingLevel = value;
      }
    }

    [XmlIgnore]
    public int LuckLevel
    {
      get
      {
        return this.luckLevel + this.addedLuckLevel;
      }
      set
      {
        this.luckLevel = value;
      }
    }

    [XmlIgnore]
    public int HouseUpgradeLevel
    {
      get
      {
        return this.houseUpgradeLevel;
      }
      set
      {
        this.houseUpgradeLevel = value;
      }
    }

    [XmlIgnore]
    public int CoopUpgradeLevel
    {
      get
      {
        return this.coopUpgradeLevel;
      }
      set
      {
        this.coopUpgradeLevel = value;
      }
    }

    [XmlIgnore]
    public int BarnUpgradeLevel
    {
      get
      {
        return this.barnUpgradeLevel;
      }
      set
      {
        this.barnUpgradeLevel = value;
      }
    }

    [XmlIgnore]
    public Microsoft.Xna.Framework.Rectangle TemporaryImpassableTile
    {
      get
      {
        return this.temporaryImpassableTile;
      }
      set
      {
        this.temporaryImpassableTile = value;
      }
    }

    [XmlIgnore]
    public List<Item> Items
    {
      get
      {
        return this.items;
      }
      set
      {
        this.items = value;
      }
    }

    [XmlIgnore]
    public int MagneticRadius
    {
      get
      {
        return this.magneticRadius;
      }
      set
      {
        this.magneticRadius = value;
      }
    }

    [XmlIgnore]
    public Object ActiveObject
    {
      get
      {
        if (this.currentToolIndex < this.items.Count && this.items[this.currentToolIndex] != null && this.items[this.currentToolIndex] is Object)
          return (Object) this.items[this.currentToolIndex];
        return (Object) null;
      }
      set
      {
        if (value == null)
          this.removeItemFromInventory((Item) this.ActiveObject);
        else
          this.addItemToInventory((Item) value, this.CurrentToolIndex);
      }
    }

    [XmlIgnore]
    public string Name
    {
      get
      {
        return this.name;
      }
      set
      {
        this.name = value;
      }
    }

    [XmlIgnore]
    public bool IsMale
    {
      get
      {
        return this.isMale;
      }
      set
      {
        this.isMale = value;
      }
    }

    [XmlIgnore]
    public List<int> DialogueQuestionsAnswered
    {
      get
      {
        return this.dialogueQuestionsAnswered;
      }
      set
      {
        this.dialogueQuestionsAnswered = value;
      }
    }

    [XmlIgnore]
    public int WoodPieces
    {
      get
      {
        return this.woodPieces;
      }
      set
      {
        this.woodPieces = value;
      }
    }

    [XmlIgnore]
    public int StonePieces
    {
      get
      {
        return this.stonePieces;
      }
      set
      {
        this.stonePieces = value;
      }
    }

    [XmlIgnore]
    public int CopperPieces
    {
      get
      {
        return this.copperPieces;
      }
      set
      {
        this.copperPieces = value;
      }
    }

    [XmlIgnore]
    public int IronPieces
    {
      get
      {
        return this.ironPieces;
      }
      set
      {
        this.ironPieces = value;
      }
    }

    [XmlIgnore]
    public int CoalPieces
    {
      get
      {
        return this.coalPieces;
      }
      set
      {
        this.coalPieces = value;
      }
    }

    [XmlIgnore]
    public int GoldPieces
    {
      get
      {
        return this.goldPieces;
      }
      set
      {
        this.goldPieces = value;
      }
    }

    [XmlIgnore]
    public int IridiumPieces
    {
      get
      {
        return this.iridiumPieces;
      }
      set
      {
        this.iridiumPieces = value;
      }
    }

    [XmlIgnore]
    public int QuartzPieces
    {
      get
      {
        return this.quartzPieces;
      }
      set
      {
        this.quartzPieces = value;
      }
    }

    [XmlIgnore]
    public int Feed
    {
      get
      {
        return this.feed;
      }
      set
      {
        this.feed = value;
      }
    }

    [XmlIgnore]
    public bool CanMove
    {
      get
      {
        return this.canMove;
      }
      set
      {
        this.canMove = value;
      }
    }

    [XmlIgnore]
    public bool UsingTool
    {
      get
      {
        return this.usingTool;
      }
      set
      {
        this.usingTool = value;
      }
    }

    [XmlIgnore]
    public Tool CurrentTool
    {
      get
      {
        if (this.CurrentItem != null && this.CurrentItem is Tool)
          return (Tool) this.CurrentItem;
        return (Tool) null;
      }
      set
      {
        this.items[this.CurrentToolIndex] = (Item) value;
      }
    }

    [XmlIgnore]
    public Item CurrentItem
    {
      get
      {
        if (this.currentToolIndex >= this.items.Count)
          return (Item) null;
        return this.items[this.currentToolIndex];
      }
    }

    [XmlIgnore]
    public int CurrentToolIndex
    {
      get
      {
        return this.currentToolIndex;
      }
      set
      {
        if (this.currentToolIndex >= 0 && this.CurrentItem != null && value != this.currentToolIndex)
          this.CurrentItem.actionWhenStopBeingHeld(this);
        this.currentToolIndex = value;
      }
    }

    [XmlIgnore]
    public float Stamina
    {
      get
      {
        return this.stamina;
      }
      set
      {
        this.stamina = Math.Min((float) this.maxStamina, Math.Max(value, -16f));
      }
    }

    [XmlIgnore]
    public int MaxStamina
    {
      get
      {
        return this.maxStamina;
      }
      set
      {
        this.maxStamina = value;
      }
    }

    [XmlIgnore]
    public bool IsMainPlayer
    {
      get
      {
        return this.uniqueMultiplayerID == Game1.player.uniqueMultiplayerID;
      }
    }

    [XmlIgnore]
    public FarmerSprite FarmerSprite
    {
      get
      {
        return (FarmerSprite) this.sprite;
      }
      set
      {
        this.sprite = (AnimatedSprite) value;
      }
    }

    [XmlIgnore]
    public FarmerRenderer FarmerRenderer
    {
      get
      {
        return this.farmerRenderer;
      }
      set
      {
        this.farmerRenderer = value;
      }
    }

    [XmlIgnore]
    public int Money
    {
      get
      {
        return this.money;
      }
      set
      {
        if (value > this.money)
        {
          this.totalMoneyEarned = this.totalMoneyEarned + (uint) (value - this.money);
          Game1.stats.checkForMoneyAchievements();
        }
        else
        {
          int money = this.money;
        }
        this.money = value;
      }
    }

    public Farmer()
    {
      this.farmerTextureManager = Game1.content.CreateTemporary();
      this.farmerRenderer = new FarmerRenderer(this.farmerTextureManager.Load<Texture2D>("Characters\\Farmer\\farmer_" + (this.isMale ? "" : "girl_") + "base"));
      this.currentLocation = Game1.getLocationFromName("FarmHouse");
      Game1.player.sprite = (AnimatedSprite) new FarmerSprite((Texture2D) null);
    }

    public Farmer(FarmerSprite sprite, Vector2 position, int speed, string name, List<Item> initialTools, bool isMale)
      : base((AnimatedSprite) sprite, position, speed, name)
    {
      this.farmerTextureManager = Game1.content.CreateTemporary();
      this.pantsColor = new Color(46, 85, 183);
      this.hairstyleColor = new Color(193, 90, 50);
      this.newEyeColor = new Color(122, 68, 52);
      this.name = name;
      this.displayName = name;
      this.currentToolIndex = 0;
      this.isMale = isMale;
      this.basicShipped = new SerializableDictionary<int, int>();
      this.fishCaught = new SerializableDictionary<int, int[]>();
      this.archaeologyFound = new SerializableDictionary<int, int[]>();
      this.mineralsFound = new SerializableDictionary<int, int>();
      this.recipesCooked = new SerializableDictionary<int, int>();
      this.friendships = new SerializableDictionary<string, int[]>();
      this.stamina = (float) this.maxStamina;
      this.items = initialTools;
      if (this.items == null)
        this.items = new List<Item>();
      for (int count = this.items.Count; count < this.maxItems; ++count)
        this.items.Add((Item) null);
      this.activeDialogueEvents.Add("Introduction", 6);
      name = "Cam";
      this.farmerRenderer = new FarmerRenderer(this.farmerTextureManager.Load<Texture2D>("Characters\\Farmer\\farmer_" + (isMale ? "" : "girl_") + "base"));
      this.currentLocation = Game1.getLocationFromName("FarmHouse");
      if (this.currentLocation != null)
        this.mostRecentBed = Utility.PointToVector2((this.currentLocation as FarmHouse).getBedSpot()) * (float) Game1.tileSize;
      else
        this.mostRecentBed = new Vector2(9f, 9f) * (float) Game1.tileSize;
    }

    public Texture2D getTexture()
    {
      if (this.farmerTextureManager == null)
        this.farmerTextureManager = Game1.content.CreateTemporary();
      return this.farmerTextureManager.Load<Texture2D>("Characters\\Farmer\\farmer_" + (this.isMale ? "" : "girl_") + "base");
    }

    public void checkForLevelTenStatus()
    {
    }

    public void unload()
    {
      if (this.farmerTextureManager == null)
        return;
      this.farmerTextureManager.Unload();
      this.farmerTextureManager.Dispose();
      this.farmerTextureManager = (LocalizedContentManager) null;
    }

    public void setInventory(List<Item> newInventory)
    {
      this.items = newInventory;
      if (this.items == null)
        this.items = new List<Item>();
      for (int count = this.items.Count; count < this.maxItems; ++count)
        this.items.Add((Item) null);
    }

    public void makeThisTheActiveObject(Object o)
    {
      if (this.freeSpotsInInventory() <= 0)
        return;
      Item currentItem = this.CurrentItem;
      this.ActiveObject = o;
      this.addItemToInventory(currentItem);
    }

    public int getNumberOfChildren()
    {
      int num = 0;
      foreach (NPC character in Utility.getHomeOfFarmer(Game1.player).characters)
      {
        if (character is Child && (character as Child).isChildOf(Game1.player))
          ++num;
      }
      foreach (NPC character in Game1.getLocationFromName("Farm").characters)
      {
        if (character is Child && (character as Child).isChildOf(Game1.player))
          ++num;
      }
      return num;
    }

    public void mountUp(Horse mount)
    {
      this.mount = mount;
      this.xOffset = -11f;
      this.position = Utility.PointToVector2(mount.GetBoundingBox().Location);
      this.position.Y -= (float) (Game1.pixelZoom * 4);
      this.position.X -= (float) (Game1.pixelZoom * 2);
      this.speed = 2;
      this.showNotCarrying();
    }

    public Horse getMount()
    {
      return this.mount;
    }

    public void dismount()
    {
      if (this.mount != null)
        this.mount = (Horse) null;
      this.collisionNPC = (NPC) null;
      this.running = false;
      this.speed = !Game1.isOneOfTheseKeysDown(Keyboard.GetState(), Game1.options.runButton) || Game1.options.autoRun ? 2 : 5;
      this.running = this.speed == 5;
      if (this.running)
      {
        this.speed = 5;
      }
      else
      {
        this.speed = 2;
        this.Halt();
      }
      this.Halt();
      this.xOffset = 0.0f;
    }

    public bool isRidingHorse()
    {
      if (this.mount != null)
        return !Game1.eventUp;
      return false;
    }

    public List<Child> getChildren()
    {
      List<Child> childList = new List<Child>();
      foreach (NPC character in Utility.getHomeOfFarmer(Game1.player).characters)
      {
        if (character is Child && (character as Child).isChildOf(Game1.player))
          childList.Add(character as Child);
      }
      foreach (NPC character in Game1.getLocationFromName("Farm").characters)
      {
        if (character is Child && (character as Child).isChildOf(Game1.player))
          childList.Add(character as Child);
      }
      return childList;
    }

    public Tool getToolFromName(string name)
    {
      foreach (Item obj in this.items)
      {
        if (obj != null && obj is Tool && obj.Name.Contains(name))
          return (Tool) obj;
      }
      return (Tool) null;
    }

    public override void SetMovingDown(bool b)
    {
      this.setMoving((byte) (4 + (b ? 0 : 32)));
    }

    public override void SetMovingRight(bool b)
    {
      this.setMoving((byte) (2 + (b ? 0 : 32)));
    }

    public override void SetMovingUp(bool b)
    {
      this.setMoving((byte) (1 + (b ? 0 : 32)));
    }

    public override void SetMovingLeft(bool b)
    {
      this.setMoving((byte) (8 + (b ? 0 : 32)));
    }

    public int? tryGetFriendshipLevelForNPC(string name)
    {
      int[] numArray;
      if (this.friendships.TryGetValue(name, out numArray))
        return new int?(numArray[0]);
      return new int?();
    }

    public int getFriendshipLevelForNPC(string name)
    {
      int[] numArray;
      if (this.friendships.TryGetValue(name, out numArray))
        return numArray[0];
      return 0;
    }

    public int getFriendshipHeartLevelForNPC(string name)
    {
      return this.getFriendshipLevelForNPC(name) / 250;
    }

    public bool hasAFriendWithHeartLevel(int heartLevel, bool datablesOnly)
    {
      foreach (NPC allCharacter in Utility.getAllCharacters())
      {
        if ((!datablesOnly || allCharacter.datable) && this.getFriendshipHeartLevelForNPC(allCharacter.name) >= heartLevel)
          return true;
      }
      return false;
    }

    public int getTallyOfObject(int index, bool bigCraftable)
    {
      int num = 0;
      foreach (Item obj in this.items)
      {
        if (obj is Object && (obj as Object).ParentSheetIndex == index && (obj as Object).bigCraftable == bigCraftable)
          num += obj.Stack;
      }
      return num;
    }

    public bool areAllItemsNull()
    {
      for (int index = 0; index < this.items.Count; ++index)
      {
        if (this.items[index] != null)
          return false;
      }
      return true;
    }

    public void shipAll()
    {
      for (int index1 = 0; index1 < this.items.Count; ++index1)
      {
        if (this.items[index1] != null && this.items[index1] is Object)
        {
          this.shippedBasic(((Object) this.items[index1]).ParentSheetIndex, this.items[index1].Stack);
          for (int index2 = 0; index2 < this.items[index1].Stack; ++index2)
            Game1.shipObject((Object) this.items[index1].getOne());
          this.items[index1] = (Item) null;
        }
      }
      Game1.playSound("Ship");
    }

    public void shippedBasic(int index, int number)
    {
      if (this.basicShipped.ContainsKey(index))
      {
        SerializableDictionary<int, int> basicShipped = this.basicShipped;
        int index1 = index;
        basicShipped[index1] = basicShipped[index1] + number;
      }
      else
        this.basicShipped.Add(index, number);
    }

    public void shiftToolbar(bool right)
    {
      if (this.items == null || this.items.Count < 12 || (this.UsingTool || Game1.dialogueUp) || (!Game1.pickingTool && !Game1.player.CanMove || (this.areAllItemsNull() || Game1.eventUp)))
        return;
      Game1.playSound("shwip");
      if (right)
      {
        List<Item> range = this.items.GetRange(0, 12);
        this.items.RemoveRange(0, 12);
        this.items.AddRange((IEnumerable<Item>) range);
      }
      else
      {
        List<Item> range = this.items.GetRange(this.items.Count - 12, 12);
        for (int index = 0; index < this.items.Count - 12; ++index)
          range.Add(this.items[index]);
        this.items = range;
      }
      for (int index = 0; index < Game1.onScreenMenus.Count; ++index)
      {
        if (Game1.onScreenMenus[index] is Toolbar)
        {
          (Game1.onScreenMenus[index] as Toolbar).shifted(right);
          break;
        }
      }
    }

    public void foundArtifact(int index, int number)
    {
      if (this.archaeologyFound == null)
        this.archaeologyFound = new SerializableDictionary<int, int[]>();
      if (this.archaeologyFound.ContainsKey(index))
      {
        this.archaeologyFound[index][0] += number;
        this.archaeologyFound[index][1] += number;
      }
      else
      {
        if (this.archaeologyFound.Count == 0)
        {
          if (!this.eventsSeen.Contains(0) && index != 102)
            this.addQuest(23);
          this.mailReceived.Add("artifactFound");
          this.holdUpItemThenMessage((Item) new Object(index, 1, false, -1, 0), true);
        }
        this.archaeologyFound.Add(index, new int[2]
        {
          number,
          number
        });
      }
    }

    public void cookedRecipe(int index)
    {
      if (this.recipesCooked == null)
        this.recipesCooked = new SerializableDictionary<int, int>();
      if (this.recipesCooked.ContainsKey(index))
      {
        SerializableDictionary<int, int> recipesCooked = this.recipesCooked;
        int num1 = index;
        int index1 = num1;
        int num2 = recipesCooked[index1];
        int index2 = num1;
        int num3 = num2 + 1;
        recipesCooked[index2] = num3;
      }
      else
        this.recipesCooked.Add(index, 1);
    }

    public bool caughtFish(int index, int size)
    {
      if (this.fishCaught == null)
        this.fishCaught = new SerializableDictionary<int, int[]>();
      if (index >= 167 && index < 173)
        return false;
      bool flag = false;
      if (this.fishCaught.ContainsKey(index))
      {
        ++this.fishCaught[index][0];
        Game1.stats.checkForFishingAchievements();
        if (size > this.fishCaught[index][1])
        {
          this.fishCaught[index][1] = size;
          flag = true;
        }
      }
      else
      {
        this.fishCaught.Add(index, new int[2]{ 1, size });
        Game1.stats.checkForFishingAchievements();
      }
      this.checkForQuestComplete((NPC) null, index, -1, (Item) null, (string) null, 7, -1);
      return flag;
    }

    public void gainExperience(int which, int howMuch)
    {
      if (which == 5 || howMuch <= 0)
        return;
      int num1 = Farmer.checkForLevelGain(this.experiencePoints[which], this.experiencePoints[which] + howMuch);
      this.experiencePoints[which] += howMuch;
      int num2 = -1;
      if (num1 != -1)
      {
        switch (which)
        {
          case 0:
            num2 = this.farmingLevel;
            this.farmingLevel = num1;
            break;
          case 1:
            num2 = this.fishingLevel;
            this.fishingLevel = num1;
            break;
          case 2:
            num2 = this.foragingLevel;
            this.foragingLevel = num1;
            break;
          case 3:
            num2 = this.miningLevel;
            this.miningLevel = num1;
            break;
          case 4:
            num2 = this.combatLevel;
            this.combatLevel = num1;
            break;
          case 5:
            num2 = this.luckLevel;
            this.luckLevel = num1;
            break;
        }
      }
      if (num1 <= num2)
        return;
      for (int y = num2 + 1; y <= num1; ++y)
      {
        this.newLevels.Add(new Point(which, y));
        int count = this.newLevels.Count;
      }
    }

    public int getEffectiveSkillLevel(int whichSkill)
    {
      if (whichSkill < 0 || whichSkill > 5)
        return -1;
      int[] numArray = new int[6]
      {
        this.farmingLevel,
        this.fishingLevel,
        this.foragingLevel,
        this.miningLevel,
        this.combatLevel,
        this.luckLevel
      };
      for (int index = 0; index < this.newLevels.Count; ++index)
        numArray[this.newLevels[index].X] -= this.newLevels[index].Y;
      return numArray[whichSkill];
    }

    public static int checkForLevelGain(int oldXP, int newXP)
    {
      int num = -1;
      if (oldXP < 100 && newXP >= 100)
        num = 1;
      if (oldXP < 380 && newXP >= 380)
        num = 2;
      if (oldXP < 770 && newXP >= 770)
        num = 3;
      if (oldXP < 1300 && newXP >= 1300)
        num = 4;
      if (oldXP < 2150 && newXP >= 2150)
        num = 5;
      if (oldXP < 3300 && newXP >= 3300)
        num = 6;
      if (oldXP < 4800 && newXP >= 4800)
        num = 7;
      if (oldXP < 6900 && newXP >= 6900)
        num = 8;
      if (oldXP < 10000 && newXP >= 10000)
        num = 9;
      if (oldXP < 15000 && newXP >= 15000)
        num = 10;
      return num;
    }

    public void foundMineral(int index)
    {
      if (this.mineralsFound == null)
        this.mineralsFound = new SerializableDictionary<int, int>();
      if (this.mineralsFound.ContainsKey(index))
      {
        SerializableDictionary<int, int> mineralsFound = this.mineralsFound;
        int num1 = index;
        int index1 = num1;
        int num2 = mineralsFound[index1];
        int index2 = num1;
        int num3 = num2 + 1;
        mineralsFound[index2] = num3;
      }
      else
        this.mineralsFound.Add(index, 1);
      if (this.hasOrWillReceiveMail("artifactFound"))
        return;
      this.mailReceived.Add("artifactFound");
    }

    public void increaseBackpackSize(int howMuch)
    {
      this.MaxItems = this.MaxItems + howMuch;
      for (int index = 0; index < howMuch; ++index)
        this.items.Add((Item) null);
    }

    public void consumeObject(int index, int quantity)
    {
      for (int index1 = this.items.Count - 1; index1 >= 0; --index1)
      {
        if (this.items[index1] != null && this.items[index1] is Object && ((Object) this.items[index1]).parentSheetIndex == index)
        {
          int num = quantity;
          quantity -= this.items[index1].Stack;
          this.items[index1].Stack -= num;
          if (this.items[index1].Stack <= 0)
            this.items[index1] = (Item) null;
          if (quantity <= 0)
            break;
        }
      }
    }

    public bool hasItemInInventory(int itemIndex, int quantity, int minPrice = 0)
    {
      int num = 0;
      for (int index = 0; index < this.items.Count; ++index)
      {
        if (this.items[index] != null && (this.items[index] is Object && !(this.items[index] is Furniture) && (!(this.items[index] as Object).bigCraftable && ((Object) this.items[index]).ParentSheetIndex == itemIndex) || this.items[index] is Object && ((Object) this.items[index]).Category == itemIndex))
          num += this.items[index].Stack;
      }
      return num >= quantity;
    }

    public bool hasItemInList(List<Item> list, int itemIndex, int quantity, int minPrice = 0)
    {
      int num = 0;
      for (int index = 0; index < list.Count; ++index)
      {
        if (list[index] != null && (list[index] is Object && !(list[index] is Furniture) && (!(list[index] as Object).bigCraftable && ((Object) list[index]).ParentSheetIndex == itemIndex) || list[index] is Object && ((Object) list[index]).Category == itemIndex))
          num += list[index].Stack;
      }
      return num >= quantity;
    }

    public void addItemByMenuIfNecessaryElseHoldUp(Item item, ItemGrabMenu.behaviorOnItemSelect itemSelectedCallback = null)
    {
      this.mostRecentlyGrabbedItem = item;
      List<Item> itemsToAdd = new List<Item>();
      itemsToAdd.Add(item);
      ItemGrabMenu.behaviorOnItemSelect itemSelectedCallback1 = itemSelectedCallback;
      this.addItemsByMenuIfNecessary(itemsToAdd, itemSelectedCallback1);
      if (Game1.activeClickableMenu != null || this.mostRecentlyGrabbedItem.parentSheetIndex == 434)
        return;
      this.holdUpItemThenMessage(item, true);
    }

    public void addItemByMenuIfNecessary(Item item, ItemGrabMenu.behaviorOnItemSelect itemSelectedCallback = null)
    {
      List<Item> itemsToAdd = new List<Item>();
      itemsToAdd.Add(item);
      ItemGrabMenu.behaviorOnItemSelect itemSelectedCallback1 = itemSelectedCallback;
      this.addItemsByMenuIfNecessary(itemsToAdd, itemSelectedCallback1);
    }

    public void addItemsByMenuIfNecessary(List<Item> itemsToAdd, ItemGrabMenu.behaviorOnItemSelect itemSelectedCallback = null)
    {
      if (itemsToAdd == null)
        return;
      if (itemsToAdd.Count > 0 && itemsToAdd[0] is Object && (itemsToAdd[0] as Object).parentSheetIndex == 434)
      {
        Game1.playerEatObject(itemsToAdd[0] as Object, true);
        if (Game1.activeClickableMenu == null)
          return;
        Game1.activeClickableMenu.exitThisMenu(false);
      }
      else
      {
        for (int index = itemsToAdd.Count - 1; index >= 0; --index)
        {
          if (this.addItemToInventoryBool(itemsToAdd[index], false))
          {
            if (itemSelectedCallback != null)
              itemSelectedCallback(itemsToAdd[index], this);
            itemsToAdd.Remove(itemsToAdd[index]);
          }
        }
        if (itemsToAdd.Count <= 0)
          return;
        Game1.activeClickableMenu = (IClickableMenu) new ItemGrabMenu(itemsToAdd);
        (Game1.activeClickableMenu as ItemGrabMenu).inventory.showGrayedOutSlots = true;
        (Game1.activeClickableMenu as ItemGrabMenu).inventory.onAddItem = itemSelectedCallback;
        (Game1.activeClickableMenu as ItemGrabMenu).source = 2;
      }
    }

    public void showCarrying()
    {
      if (Game1.eventUp || this.isRidingHorse() || this.ActiveObject != null && (this.ActiveObject is Furniture || this.ActiveObject is Wallpaper))
        return;
      if (!this.FarmerSprite.pauseForSingleAnimation && !this.isMoving())
      {
        int currentAnimation = this.FarmerSprite.indexInCurrentAnimation;
        float timer = this.FarmerSprite.timer;
        switch (this.facingDirection)
        {
          case 0:
            this.FarmerSprite.setCurrentFrame(this.running ? 144 : 112);
            break;
          case 1:
            this.FarmerSprite.setCurrentFrame(this.running ? 136 : 104);
            break;
          case 2:
            this.FarmerSprite.setCurrentFrame(this.running ? 128 : 96);
            break;
          case 3:
            this.FarmerSprite.setCurrentFrame(this.running ? 152 : 120);
            break;
        }
        this.FarmerSprite.CurrentFrame = this.FarmerSprite.CurrentAnimation[currentAnimation].frame;
        this.FarmerSprite.indexInCurrentAnimation = currentAnimation;
        this.FarmerSprite.currentAnimationIndex = currentAnimation;
        this.FarmerSprite.timer = timer;
        if (this.IsMainPlayer && this.ActiveObject != null)
          MultiplayerUtility.sendSwitchHeldItemMessage(this.ActiveObject.ParentSheetIndex, this.ActiveObject.bigCraftable ? (byte) 1 : (byte) 0, this.uniqueMultiplayerID);
      }
      if (this.ActiveObject != null)
        this.mostRecentlyGrabbedItem = (Item) this.ActiveObject;
      if (this.mostRecentlyGrabbedItem == null || !(this.mostRecentlyGrabbedItem is Object) || (this.mostRecentlyGrabbedItem as Object).ParentSheetIndex != 434)
        return;
      Game1.eatHeldObject();
    }

    public void showNotCarrying()
    {
      if (this.FarmerSprite.pauseForSingleAnimation || this.isMoving())
        return;
      int currentAnimation = this.FarmerSprite.indexInCurrentAnimation;
      float timer = this.FarmerSprite.timer;
      switch (this.facingDirection)
      {
        case 0:
          this.FarmerSprite.setCurrentFrame(this.running ? 48 : 16);
          break;
        case 1:
          this.FarmerSprite.setCurrentFrame(this.running ? 40 : 8);
          break;
        case 2:
          this.FarmerSprite.setCurrentFrame(this.running ? 32 : 0);
          break;
        case 3:
          this.FarmerSprite.setCurrentFrame(this.running ? 56 : 24);
          break;
      }
      this.FarmerSprite.CurrentFrame = this.FarmerSprite.CurrentAnimation[Math.Min(currentAnimation, this.FarmerSprite.CurrentAnimation.Count - 1)].frame;
      this.FarmerSprite.indexInCurrentAnimation = currentAnimation;
      this.FarmerSprite.currentAnimationIndex = currentAnimation;
      this.FarmerSprite.timer = timer;
      if (!this.IsMainPlayer)
        return;
      MultiplayerUtility.sendSwitchHeldItemMessage(-1, (byte) 0, this.uniqueMultiplayerID);
    }

    public bool isThereALostItemQuestThatTakesThisItem(int index)
    {
      foreach (Quest quest in Game1.player.questLog)
      {
        if (quest is LostItemQuest && (quest as LostItemQuest).itemIndex == index)
          return true;
      }
      return false;
    }

    public bool hasDailyQuest()
    {
      for (int index = this.questLog.Count - 1; index >= 0; --index)
      {
        if (this.questLog[index].dailyQuest)
          return true;
      }
      return false;
    }

    public void dayupdate()
    {
      this.attack = 0;
      this.addedSpeed = 0;
      this.dancePartner = (NPC) null;
      this.festivalScore = 0;
      this.forceTimePass = false;
      if (this.daysLeftForToolUpgrade > 0)
        this.daysLeftForToolUpgrade = this.daysLeftForToolUpgrade - 1;
      if (this.daysUntilHouseUpgrade > 0)
      {
        this.daysUntilHouseUpgrade = this.daysUntilHouseUpgrade - 1;
        if (this.daysUntilHouseUpgrade <= 0)
        {
          this.daysUntilHouseUpgrade = -1;
          this.houseUpgradeLevel = this.houseUpgradeLevel + 1;
          Utility.getHomeOfFarmer(this).moveObjectsForHouseUpgrade(this.houseUpgradeLevel);
          ++Utility.getHomeOfFarmer(this).upgradeLevel;
          if (this.houseUpgradeLevel == 1)
            this.position = new Vector2(20f, 4f) * (float) Game1.tileSize;
          if (this.houseUpgradeLevel == 2)
            this.position = new Vector2(29f, 13f) * (float) Game1.tileSize;
          Game1.stats.checkForBuildingUpgradeAchievements();
        }
      }
      for (int index = this.questLog.Count - 1; index >= 0; --index)
      {
        if (this.questLog[index].dailyQuest)
        {
          --this.questLog[index].daysLeft;
          if (this.questLog[index].daysLeft <= 0 && !this.questLog[index].completed)
            this.questLog.RemoveAt(index);
        }
      }
      foreach (Buff buff in this.buffs)
        buff.removeBuff();
      Game1.buffsDisplay.clearAllBuffs();
      this.stopGlowing();
      this.buffs.Clear();
      this.addedCombatLevel = 0;
      this.addedFarmingLevel = 0;
      this.addedFishingLevel = 0;
      this.addedForagingLevel = 0;
      this.addedLuckLevel = 0;
      this.addedMiningLevel = 0;
      this.addedSpeed = 0;
      this.bobber = "";
      float stamina = this.Stamina;
      this.Stamina = (float) this.MaxStamina;
      if (this.exhausted)
      {
        this.exhausted = false;
        this.Stamina = (float) (this.MaxStamina / 2 + 1);
      }
      if (Game1.timeOfDay > 2400)
      {
        this.Stamina = this.Stamina - (float) (1.0 - (double) (2600 - Math.Min(2600, Game1.timeOfDay)) / 200.0) * (float) (this.MaxStamina / 2);
        if (Game1.timeOfDay > 2700)
          this.Stamina = this.Stamina / 2f;
      }
      if (Game1.timeOfDay < 2700 && (double) stamina > (double) this.Stamina)
        this.Stamina = stamina;
      this.health = this.maxHealth;
      List<string> stringList = new List<string>();
      foreach (string index1 in this.activeDialogueEvents.Keys.ToList<string>())
      {
        SerializableDictionary<string, int> activeDialogueEvents = this.activeDialogueEvents;
        string str = index1;
        string index2 = str;
        int num1 = activeDialogueEvents[index2];
        string index3 = str;
        int num2 = num1 - 1;
        activeDialogueEvents[index3] = num2;
        if (this.activeDialogueEvents[index1] < 0)
          stringList.Add(index1);
      }
      foreach (string key in stringList)
        this.activeDialogueEvents.Remove(key);
      if (this.isMarried())
        this.daysMarried = this.daysMarried + 1;
      if (!this.isMarried() || !this.divorceTonight)
        return;
      NPC spouse = this.getSpouse();
      if (spouse != null)
      {
        this.spouse = (string) null;
        string str = Game1.content.Load<Dictionary<string, string>>("Data\\NPCDispositions")[spouse.name].Split('/')[10];
        spouse.defaultMap = str.Split(' ')[0];
        spouse.DefaultPosition = new Vector2((float) Convert.ToInt32(str.Split(' ')[1]), (float) Convert.ToInt32(str.Split(' ')[2])) * (float) Game1.tileSize;
        spouse.datingFarmer = false;
        spouse.divorcedFromFarmer = true;
        spouse.setMarried(false);
        for (int index = this.specialItems.Count - 1; index >= 0; --index)
        {
          if (this.specialItems[index] == 460)
            this.specialItems.RemoveAt(index);
        }
        if (this.friendships.ContainsKey(spouse.name))
          this.friendships[spouse.name][0] = 0;
        Game1.warpCharacter(spouse, spouse.defaultMap, spouse.DefaultPosition, true, false);
        Utility.getHomeOfFarmer(this).showSpouseRoom();
        Game1.getFarm().addSpouseOutdoorArea("");
      }
      this.divorceTonight = false;
    }

    public static void showReceiveNewItemMessage(Farmer who)
    {
      string dialogue1 = who.mostRecentlyGrabbedItem.checkForSpecialItemHoldUpMeessage();
      if (dialogue1 != null)
        Game1.drawObjectDialogue(dialogue1);
      else if (who.mostRecentlyGrabbedItem.parentSheetIndex == 472 && who.mostRecentlyGrabbedItem.Stack == 15)
      {
        Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.1918"));
      }
      else
      {
        string dialogue2;
        if (who.mostRecentlyGrabbedItem.Stack <= 1)
          dialogue2 = Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.1919", (object) who.mostRecentlyGrabbedItem.DisplayName, (object) Game1.getProperArticleForWord(who.mostRecentlyGrabbedItem.DisplayName));
        else
          dialogue2 = Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.1922", (object) who.mostRecentlyGrabbedItem.Stack, (object) who.mostRecentlyGrabbedItem.DisplayName);
        Game1.drawObjectDialogue(dialogue2);
      }
      who.completelyStopAnimatingOrDoingAction();
    }

    public static void showEatingItem(Farmer who)
    {
      TemporaryAnimatedSprite temporaryAnimatedSprite1 = (TemporaryAnimatedSprite) null;
      if (who.itemToEat == null)
        return;
      switch (who.FarmerSprite.indexInCurrentAnimation)
      {
        case 1:
          temporaryAnimatedSprite1 = who.itemToEat == null || !(who.itemToEat is Object) || (who.itemToEat as Object).ParentSheetIndex != 434 ? new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, (who.itemToEat as Object).parentSheetIndex, 16, 16), 254f, 1, 0, who.position + new Vector2((float) (-Game1.tileSize / 3), (float) (-Game1.tileSize * 2 + Game1.tileSize / 4)), false, false, (float) ((double) who.getStandingY() / 10000.0 + 0.00999999977648258), 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false) : new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(368, 16, 16, 16), 62.75f, 8, 2, who.position + new Vector2((float) (-Game1.tileSize / 3), (float) (-Game1.tileSize * 2 + Game1.tileSize / 4)), false, false, (float) ((double) who.getStandingY() / 10000.0 + 0.00999999977648258), 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
          break;
        case 2:
          if (who.itemToEat != null && who.itemToEat is Object && (who.itemToEat as Object).ParentSheetIndex == 434)
          {
            temporaryAnimatedSprite1 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(368, 16, 16, 16), 81.25f, 8, 0, who.position + new Vector2((float) (-Game1.tileSize / 3), (float) (-Game1.tileSize * 2 + 4 + Game1.tileSize / 4)), false, false, (float) ((double) who.getStandingY() / 10000.0 + 0.00999999977648258), 0.0f, Color.White, (float) Game1.pixelZoom, -0.01f, 0.0f, 0.0f, false)
            {
              motion = new Vector2(0.8f, -11f),
              acceleration = new Vector2(0.0f, 0.5f)
            };
            break;
          }
          Game1.playSound("dwop");
          temporaryAnimatedSprite1 = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, (who.itemToEat as Object).parentSheetIndex, 16, 16), 650f, 1, 0, who.position + new Vector2((float) (-Game1.tileSize / 3), (float) (-Game1.tileSize * 2 + 4 + Game1.tileSize / 4)), false, false, (float) ((double) who.getStandingY() / 10000.0 + 0.00999999977648258), 0.0f, Color.White, (float) Game1.pixelZoom, -0.01f, 0.0f, 0.0f, false)
          {
            motion = new Vector2(0.8f, -11f),
            acceleration = new Vector2(0.0f, 0.5f)
          };
          break;
        case 3:
          who.yJumpVelocity = 6f;
          who.yJumpOffset = 1;
          break;
        case 4:
          Game1.playSound("eat");
          for (int index = 0; index < 8; ++index)
          {
            Microsoft.Xna.Framework.Rectangle standardTileSheet = Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, (who.itemToEat as Object).parentSheetIndex, 16, 16);
            standardTileSheet.X += 8;
            standardTileSheet.Y += 8;
            standardTileSheet.Width = Game1.pixelZoom;
            standardTileSheet.Height = Game1.pixelZoom;
            TemporaryAnimatedSprite temporaryAnimatedSprite2 = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, standardTileSheet, 400f, 1, 0, who.position + new Vector2(24f, -48f), false, false, (float) ((double) who.getStandingY() / 10000.0 + 0.00999999977648258), 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
            {
              motion = new Vector2((float) Game1.random.Next(-30, 31) / 10f, (float) Game1.random.Next(-6, -3)),
              acceleration = new Vector2(0.0f, 0.5f)
            };
            who.currentLocation.temporarySprites.Add(temporaryAnimatedSprite2);
          }
          return;
        default:
          who.freezePause = 0;
          break;
      }
      if (temporaryAnimatedSprite1 == null)
        return;
      who.currentLocation.temporarySprites.Add(temporaryAnimatedSprite1);
    }

    public static void eatItem(Farmer who)
    {
    }

    public bool hasBuff(int whichBuff)
    {
      foreach (Buff buff in this.buffs)
      {
        if (buff.which == whichBuff)
          return true;
      }
      foreach (Buff otherBuff in Game1.buffsDisplay.otherBuffs)
      {
        if (otherBuff.which == whichBuff)
          return true;
      }
      return false;
    }

    public bool hasOrWillReceiveMail(string id)
    {
      if (!this.mailReceived.Contains(id) && !this.mailForTomorrow.Contains(id) && !Game1.mailbox.Contains(id))
        return this.mailForTomorrow.Contains(id + "%&NL&%");
      return true;
    }

    public static void showHoldingItem(Farmer who)
    {
      if (who.mostRecentlyGrabbedItem is SpecialItem)
      {
        TemporaryAnimatedSprite spriteForHoldingUp = (who.mostRecentlyGrabbedItem as SpecialItem).getTemporarySpriteForHoldingUp(who.position + new Vector2(0.0f, (float) (-Game1.tileSize * 2 + Game1.pixelZoom)));
        spriteForHoldingUp.motion = new Vector2(0.0f, -0.1f);
        spriteForHoldingUp.scale = (float) Game1.pixelZoom;
        spriteForHoldingUp.interval = 2500f;
        spriteForHoldingUp.totalNumberOfLoops = 0;
        spriteForHoldingUp.animationLength = 1;
        Game1.currentLocation.temporarySprites.Add(spriteForHoldingUp);
      }
      else if (who.mostRecentlyGrabbedItem is Slingshot)
        Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Tool.weaponsTexture, Game1.getSquareSourceRectForNonStandardTileSheet(Tool.weaponsTexture, 16, 16, (who.mostRecentlyGrabbedItem as Slingshot).indexOfMenuItemView), 2500f, 1, 0, who.position + new Vector2(0.0f, (float) (-Game1.tileSize * 2 + Game1.pixelZoom)), false, false, 1f, 0.0f, Color.White, 4f, 0.0f, 0.0f, 0.0f, false)
        {
          motion = new Vector2(0.0f, -0.1f)
        });
      else if (who.mostRecentlyGrabbedItem is MeleeWeapon)
        Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Tool.weaponsTexture, Game1.getSquareSourceRectForNonStandardTileSheet(Tool.weaponsTexture, 16, 16, (who.mostRecentlyGrabbedItem as MeleeWeapon).indexOfMenuItemView), 2500f, 1, 0, who.position + new Vector2(0.0f, (float) (-Game1.tileSize * 2 + Game1.pixelZoom)), false, false, 1f, 0.0f, Color.White, 4f, 0.0f, 0.0f, 0.0f, false)
        {
          motion = new Vector2(0.0f, -0.1f)
        });
      else if (who.mostRecentlyGrabbedItem is Boots)
        Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSquareSourceRectForNonStandardTileSheet(Game1.objectSpriteSheet, Game1.tileSize / 4, Game1.tileSize / 4, (who.mostRecentlyGrabbedItem as Boots).indexInTileSheet), 2500f, 1, 0, who.position + new Vector2(0.0f, (float) (-Game1.tileSize * 2 + Game1.pixelZoom)), false, false, 1f, 0.0f, Color.White, 4f, 0.0f, 0.0f, 0.0f, false)
        {
          motion = new Vector2(0.0f, -0.1f)
        });
      else if (who.mostRecentlyGrabbedItem is Tool)
        Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.toolSpriteSheet, Game1.getSquareSourceRectForNonStandardTileSheet(Game1.toolSpriteSheet, Game1.tileSize / 4, Game1.tileSize / 4, (who.mostRecentlyGrabbedItem as Tool).indexOfMenuItemView), 2500f, 1, 0, who.position + new Vector2(0.0f, (float) (-Game1.tileSize * 2 + Game1.pixelZoom)), false, false, 1f, 0.0f, Color.White, 4f, 0.0f, 0.0f, 0.0f, false)
        {
          motion = new Vector2(0.0f, -0.1f)
        });
      else if (who.mostRecentlyGrabbedItem is Furniture)
        Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Furniture.furnitureTexture, (who.mostRecentlyGrabbedItem as Furniture).sourceRect, 2500f, 1, 0, who.position + new Vector2(0.0f, (float) (-Game1.tileSize * 3 + 4)), false, false)
        {
          motion = new Vector2(0.0f, -0.1f),
          scale = (float) Game1.pixelZoom,
          layerDepth = 1f
        });
      else if (who.mostRecentlyGrabbedItem is Object && !(who.mostRecentlyGrabbedItem as Object).bigCraftable)
      {
        Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, who.mostRecentlyGrabbedItem.parentSheetIndex, 16, 16), 2500f, 1, 0, who.position + new Vector2(0.0f, (float) (-Game1.tileSize * 2 + 4)), false, false)
        {
          motion = new Vector2(0.0f, -0.1f),
          scale = (float) Game1.pixelZoom,
          layerDepth = 1f
        });
        if (who.mostRecentlyGrabbedItem.parentSheetIndex == 434)
          Game1.eatHeldObject();
      }
      else if (who.mostRecentlyGrabbedItem is Object)
        Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.bigCraftableSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.bigCraftableSpriteSheet, who.mostRecentlyGrabbedItem.parentSheetIndex, 16, 32), 2500f, 1, 0, who.position + new Vector2(0.0f, (float) (-Game1.tileSize * 3 + 4)), false, false)
        {
          motion = new Vector2(0.0f, -0.1f),
          scale = (float) Game1.pixelZoom,
          layerDepth = 1f
        });
      if (who.mostRecentlyGrabbedItem == null)
        Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(420, 489, 25, 18), 2500f, 1, 0, who.position + new Vector2(0.0f, (float) (-Game1.tileSize * 2 - Game1.pixelZoom * 6)), false, false)
        {
          motion = new Vector2(0.0f, -0.1f),
          scale = (float) Game1.pixelZoom,
          layerDepth = 1f
        });
      else
        Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(10, who.position + new Vector2((float) (Game1.tileSize / 2), (float) (-Game1.tileSize * 3 / 2)), Color.White, 8, false, 100f, 0, -1, -1f, -1, 0)
        {
          motion = new Vector2(0.0f, -0.1f)
        });
    }

    public void holdUpItemThenMessage(Item item, bool showMessage = true)
    {
      this.completelyStopAnimatingOrDoingAction();
      if (showMessage)
        DelayedAction.playSoundAfterDelay("getNewSpecialItem", 750);
      Game1.player.faceDirection(2);
      this.freezePause = 4000;
      this.FarmerSprite.animateOnce(new FarmerSprite.AnimationFrame[3]
      {
        new FarmerSprite.AnimationFrame(57, 0),
        new FarmerSprite.AnimationFrame(57, 2500, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showHoldingItem), false),
        showMessage ? new FarmerSprite.AnimationFrame((int) (short) this.FarmerSprite.currentFrame, 500, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showReceiveNewItemMessage), true) : new FarmerSprite.AnimationFrame((int) (short) this.FarmerSprite.currentFrame, 500, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false)
      });
      this.mostRecentlyGrabbedItem = item;
      this.canMove = false;
    }

    private void checkForLevelUp()
    {
      int num1 = 600;
      int num2 = 0;
      int level = this.Level;
      for (int index = 0; index <= 35; ++index)
      {
        if (level <= index && (long) this.totalMoneyEarned >= (long) num1)
        {
          this.NewSkillPointsToSpend = this.NewSkillPointsToSpend + 2;
          Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.1925"), Color.Violet, 3500f));
        }
        else if ((long) this.totalMoneyEarned < (long) num1)
          break;
        int num3 = num1;
        num1 += (int) ((double) (num1 - num2) * 1.2);
        num2 = num3;
      }
    }

    public void clearBackpack()
    {
      for (int index = 0; index < this.items.Count; ++index)
        this.items[index] = (Item) null;
    }

    public int numberOfItemsInInventory()
    {
      int num = 0;
      foreach (Item obj in this.items)
      {
        if (obj != null && obj is Object)
          ++num;
      }
      return num;
    }

    public void resetFriendshipsForNewDay()
    {
      foreach (string name in this.friendships.Keys.ToArray<string>())
      {
        this.friendships[name][3] = 0;
        bool flag = false;
        NPC characterFromName = Game1.getCharacterFromName(name, false);
        if (characterFromName != null && characterFromName.datable && (!characterFromName.datingFarmer && !characterFromName.isMarried()))
          flag = true;
        if (this.spouse != null && name.Equals(this.spouse) && !this.hasPlayerTalkedToNPC(name))
          this.friendships[name][0] = Math.Max(this.friendships[name][0] - 20, 0);
        else if (characterFromName != null && characterFromName.datingFarmer && (!this.hasPlayerTalkedToNPC(name) && this.friendships[name][0] < 2500))
          this.friendships[name][0] = Math.Max(this.friendships[name][0] - 8, 0);
        if (this.hasPlayerTalkedToNPC(name))
          this.friendships[name][2] = 0;
        else if (!flag && this.friendships[name][0] < 2500 || flag && this.friendships[name][0] < 2000)
          this.friendships[name][0] = Math.Max(this.friendships[name][0] - 2, 0);
        if (Game1.dayOfMonth % 7 == 0)
        {
          if (this.friendships[name][1] == 2)
            this.friendships[name][0] = Math.Min(this.friendships[name][0] + 10, 2749);
          this.friendships[name][1] = 0;
        }
      }
    }

    public bool hasPlayerTalkedToNPC(string name)
    {
      if (!this.friendships.ContainsKey(name) && Game1.NPCGiftTastes.ContainsKey(name))
        this.friendships.Add(name, new int[4]);
      return this.friendships.ContainsKey(name) && this.friendships[name][2] == 1;
    }

    public void fuelLantern(int units)
    {
      Tool toolFromName = this.getToolFromName("Lantern");
      if (toolFromName == null)
        return;
      ((Lantern) toolFromName).fuelLeft = Math.Min(100, ((Lantern) toolFromName).fuelLeft + units);
    }

    public bool tryToCraftItem(List<int[]> ingredients, double successRate, int itemToCraft, bool bigCraftable, string craftingOrCooking)
    {
      List<int[]> locationOfIngredients = new List<int[]>();
      foreach (int[] ingredient in ingredients)
      {
        if (ingredient[0] <= -100)
        {
          int num = 0;
          switch (ingredient[0])
          {
            case -106:
              num = this.IridiumPieces;
              break;
            case -105:
              num = this.GoldPieces;
              break;
            case -104:
              num = this.CoalPieces;
              break;
            case -103:
              num = this.IronPieces;
              break;
            case -102:
              num = this.CopperPieces;
              break;
            case -101:
              num = this.stonePieces;
              break;
            case -100:
              num = this.WoodPieces;
              break;
          }
          if (num < ingredient[1])
            return false;
          locationOfIngredients.Add(ingredient);
        }
        else
        {
          for (int index1 = 0; index1 < ingredient[1]; ++index1)
          {
            int[] numArray = new int[2]{ 99999, -1 };
            for (int index2 = 0; index2 < this.items.Count; ++index2)
            {
              if (this.items[index2] != null && this.items[index2] is Object && (((Object) this.items[index2]).ParentSheetIndex == ingredient[0] && !Farmer.containsIndex(locationOfIngredients, index2)))
              {
                locationOfIngredients.Add(new int[2]
                {
                  index2,
                  1
                });
                break;
              }
              if (this.items[index2] != null && this.items[index2] is Object && (((Object) this.items[index2]).Category == ingredient[0] && !Farmer.containsIndex(locationOfIngredients, index2)) && ((Object) this.items[index2]).Price < numArray[0])
              {
                numArray[0] = ((Object) this.items[index2]).Price;
                numArray[1] = index2;
              }
              if (index2 == this.items.Count - 1)
              {
                if (numArray[1] == -1)
                  return false;
                locationOfIngredients.Add(new int[2]
                {
                  numArray[1],
                  ingredient[1]
                });
                break;
              }
            }
          }
        }
      }
      string str = "";
      if (itemToCraft == 291)
        str = this.items[locationOfIngredients[0][0]].Name;
      else if (itemToCraft == 216 && Game1.random.NextDouble() < 0.5)
        ++itemToCraft;
      Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.1927", (object) craftingOrCooking));
      this.isCrafting = true;
      Game1.playSound("crafting");
      int index3 = -1;
      string message = Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.1930");
      if (bigCraftable)
      {
        Game1.player.ActiveObject = new Object(Vector2.Zero, itemToCraft, false);
        Game1.player.showCarrying();
      }
      else if (itemToCraft < 0)
      {
        if (!true)
          message = Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.1935");
      }
      else
      {
        index3 = locationOfIngredients[0][0];
        if (locationOfIngredients[0][0] < 0)
        {
          for (int index1 = 0; index1 < this.items.Count; ++index1)
          {
            if (this.items[index1] == null)
            {
              index3 = index1;
              break;
            }
            if (index1 == this.maxItems - 1)
            {
              Game1.pauseThenMessage(this.craftingTime + ingredients.Count<int[]>() * 500, Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.1936"), true);
              return false;
            }
          }
        }
        this.items[index3] = str.Equals("") ? (Item) new Object(Vector2.Zero, itemToCraft, (string) null, true, true, false, false) : (Item) new Object(Vector2.Zero, itemToCraft, str + " Bobber", true, true, false, false);
      }
      Game1.pauseThenMessage(this.craftingTime + ingredients.Count * 500, message, true);
      string lower = craftingOrCooking.ToLower();
      if (!(lower == "crafting"))
      {
        if (lower == "cooking")
          ++Game1.stats.ItemsCooked;
      }
      else
        ++Game1.stats.ItemsCrafted;
      foreach (int[] numArray in locationOfIngredients)
      {
        if (numArray[0] <= -100)
        {
          switch (numArray[0])
          {
            case -106:
              this.IridiumPieces = this.IridiumPieces - numArray[1];
              continue;
            case -105:
              this.GoldPieces = this.GoldPieces - numArray[1];
              continue;
            case -104:
              this.CoalPieces = this.CoalPieces - numArray[1];
              continue;
            case -103:
              this.IronPieces = this.IronPieces - numArray[1];
              continue;
            case -102:
              this.CopperPieces = this.CopperPieces - numArray[1];
              continue;
            case -101:
              this.stonePieces = this.stonePieces - numArray[1];
              continue;
            case -100:
              this.WoodPieces = this.WoodPieces - numArray[1];
              continue;
            default:
              continue;
          }
        }
        else if (numArray[0] != index3)
          this.items[numArray[0]] = (Item) null;
      }
      return true;
    }

    private static bool containsIndex(List<int[]> locationOfIngredients, int index)
    {
      for (int index1 = 0; index1 < locationOfIngredients.Count; ++index1)
      {
        if (locationOfIngredients[index1][0] == index)
          return true;
      }
      return false;
    }

    public override bool collideWith(Object o)
    {
      base.collideWith(o);
      if (this.isRidingHorse() && o is Fence)
      {
        this.mount.squeezeForGate();
        switch (this.facingDirection)
        {
          case 1:
            if ((double) o.tileLocation.X < (double) this.getTileX())
              return false;
            break;
          case 3:
            if ((double) o.tileLocation.X > (double) this.getTileX())
              return false;
            break;
        }
      }
      return true;
    }

    public void changeIntoSwimsuit()
    {
      this.bathingClothes = true;
      this.Halt();
      this.setRunning(false, false);
      this.canOnlyWalk = true;
    }

    public void changeOutOfSwimSuit()
    {
      this.bathingClothes = false;
      this.canOnlyWalk = false;
      this.Halt();
      this.FarmerSprite.StopAnimation();
      if (!Game1.options.autoRun)
        return;
      this.setRunning(true, false);
    }

    public bool ownsFurniture(string name)
    {
      foreach (string str in this.furnitureOwned)
      {
        if (str.Equals(name))
          return true;
      }
      return false;
    }

    public void showFrame(int frame, bool flip = false)
    {
      this.FarmerSprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
      {
        new FarmerSprite.AnimationFrame(Convert.ToInt32(frame), 100, false, flip, (AnimatedSprite.endOfAnimationBehavior) null, false)
      }.ToArray());
      this.FarmerSprite.loopThisAnimation = true;
      this.FarmerSprite.PauseForSingleAnimation = true;
      this.sprite.CurrentFrame = Convert.ToInt32(frame);
    }

    public void stopShowingFrame()
    {
      this.FarmerSprite.loopThisAnimation = false;
      this.FarmerSprite.PauseForSingleAnimation = false;
      this.completelyStopAnimatingOrDoingAction();
    }

    public Item addItemToInventory(Item item)
    {
      if (item == null)
        return (Item) null;
      if (item is SpecialItem)
        return item;
      for (int index = 0; index < this.maxItems; ++index)
      {
        if (index < this.items.Count && this.items[index] != null && (this.items[index].maximumStackSize() != -1 && this.items[index].getStack() < this.items[index].maximumStackSize()) && this.items[index].Name.Equals(item.Name) && ((!(item is Object) || !(this.items[index] is Object) || (item as Object).quality == (this.items[index] as Object).quality && (item as Object).parentSheetIndex == (this.items[index] as Object).parentSheetIndex) && item.canStackWith(this.items[index])))
        {
          int stack = this.items[index].addToStack(item.getStack());
          if (stack <= 0)
            return (Item) null;
          item.Stack = stack;
        }
      }
      for (int index = 0; index < this.maxItems; ++index)
      {
        if (this.items.Count > index && this.items[index] == null)
        {
          this.items[index] = item;
          return (Item) null;
        }
      }
      return item;
    }

    public bool isInventoryFull()
    {
      for (int index = 0; index < this.maxItems; ++index)
      {
        if (this.items.Count > index && this.items[index] == null)
          return false;
      }
      return true;
    }

    public bool couldInventoryAcceptThisItem(Item item)
    {
      for (int index = 0; index < this.maxItems; ++index)
      {
        if (this.items.Count > index && (this.items[index] == null || item is Object && this.items[index] is Object && (this.items[index].Stack + item.Stack <= this.items[index].maximumStackSize() && (this.items[index] as Object).canStackWith(item))))
          return true;
      }
      if (this.isInventoryFull() && Game1.hudMessages.Count<HUDMessage>() == 0)
        Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Crop.cs.588"));
      return false;
    }

    public bool couldInventoryAcceptThisObject(int index, int stack, int quality = 0)
    {
      for (int index1 = 0; index1 < this.maxItems; ++index1)
      {
        if (this.items.Count > index1 && (this.items[index1] == null || this.items[index1] is Object && this.items[index1].Stack + stack <= this.items[index1].maximumStackSize() && ((this.items[index1] as Object).ParentSheetIndex == index && (this.items[index1] as Object).quality == quality)))
          return true;
      }
      if (this.isInventoryFull() && Game1.hudMessages.Count<HUDMessage>() == 0)
        Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Crop.cs.588"));
      return false;
    }

    public bool hasItemOfType(string type)
    {
      for (int index = 0; index < this.maxItems; ++index)
      {
        if (this.items.Count > index && this.items[index] is Object && (this.items[index] as Object).type.Equals(type))
          return true;
      }
      return false;
    }

    public NPC getSpouse()
    {
      if (this.isMarried())
        return Game1.getCharacterFromName(this.spouse, false);
      return (NPC) null;
    }

    public int freeSpotsInInventory()
    {
      int num = 0;
      for (int index = 0; index < this.maxItems; ++index)
      {
        if (index < this.items.Count && this.items[index] == null)
          ++num;
      }
      return num;
    }

    public Item hasItemWithNameThatContains(string name)
    {
      for (int index = 0; index < this.maxItems; ++index)
      {
        if (index < this.items.Count && this.items[index] != null && this.items[index].Name.Contains(name))
          return this.items[index];
      }
      return (Item) null;
    }

    public bool addItemToInventoryBool(Item item, bool makeActiveObject = false)
    {
      if (item == null)
        return false;
      int stack = item.Stack;
      Item obj1 = this.IsMainPlayer ? this.addItemToInventory(item) : (Item) null;
      bool flag = obj1 == null || obj1.Stack != item.Stack || item is SpecialItem;
      if (item is Object)
        (item as Object).reloadSprite();
      if (!flag || !this.IsMainPlayer)
        return false;
      if (item != null)
      {
        if (this.IsMainPlayer && !item.hasBeenInInventory)
        {
          if (item is SpecialItem)
          {
            (item as SpecialItem).actionWhenReceived(this);
            return true;
          }
          if (item is Object && (item as Object).specialItem)
          {
            if ((item as Object).bigCraftable || item is Furniture)
            {
              if (!this.specialBigCraftables.Contains((item as Object).parentSheetIndex))
                this.specialBigCraftables.Add((item as Object).parentSheetIndex);
            }
            else if (!this.specialItems.Contains((item as Object).parentSheetIndex))
              this.specialItems.Add((item as Object).parentSheetIndex);
          }
          if (item is Object && (item as Object).Category == -2 && !(item as Object).hasBeenPickedUpByFarmer)
            this.foundMineral((item as Object).parentSheetIndex);
          else if (!(item is Furniture) && item is Object && ((item as Object).type != null && (item as Object).type.Contains("Arch")) && !(item as Object).hasBeenPickedUpByFarmer)
            this.foundArtifact((item as Object).parentSheetIndex, 1);
          if (item.parentSheetIndex == 102)
          {
            this.foundArtifact((item as Object).parentSheetIndex, 1);
            this.removeItemFromInventory(item);
          }
          else
          {
            switch (item.parentSheetIndex)
            {
              case 384:
                Game1.stats.GoldFound += (uint) item.Stack;
                break;
              case 386:
                Game1.stats.IridiumFound += (uint) item.Stack;
                break;
              case 378:
                Game1.stats.CopperFound += (uint) item.Stack;
                break;
              case 380:
                Game1.stats.IronFound += (uint) item.Stack;
                break;
            }
          }
        }
        if (item is Object && !item.hasBeenInInventory)
        {
          if (!(item is Furniture) && !(item as Object).bigCraftable && !(item as Object).hasBeenPickedUpByFarmer)
            this.checkForQuestComplete((NPC) null, (item as Object).parentSheetIndex, (item as Object).stack, item, (string) null, 9, -1);
          (item as Object).hasBeenPickedUpByFarmer = true;
          if ((item as Object).questItem)
            return true;
          if (Game1.activeClickableMenu == null)
          {
            switch ((item as Object).parentSheetIndex)
            {
              case 390:
                ++Game1.stats.StoneGathered;
                if (Game1.stats.StoneGathered >= 100U && !Game1.player.hasOrWillReceiveMail("robinWell"))
                {
                  Game1.addMailForTomorrow("robinWell", false, false);
                  break;
                }
                break;
              case 535:
                if (!Game1.player.hasOrWillReceiveMail("geodeFound"))
                {
                  this.mailReceived.Add("geodeFound");
                  this.holdUpItemThenMessage(item, true);
                  break;
                }
                break;
              case 102:
                ++Game1.stats.NotesFound;
                Game1.playSound("newRecipe");
                this.holdUpItemThenMessage(item, true);
                return true;
              case 378:
                if (!Game1.player.hasOrWillReceiveMail("copperFound"))
                {
                  Game1.addMailForTomorrow("copperFound", true, false);
                  break;
                }
                break;
            }
          }
        }
        Color color = Color.WhiteSmoke;
        string displayName = item.DisplayName;
        if (item is Object)
        {
          string type = (item as Object).type;
          if (!(type == "Arch"))
          {
            if (!(type == "Fish"))
            {
              if (!(type == "Mineral"))
              {
                if (!(type == "Vegetable"))
                {
                  if (type == "Fruit")
                    color = Color.Pink;
                }
                else
                  color = Color.PaleGreen;
              }
              else
                color = Color.PaleVioletRed;
            }
            else
              color = Color.SkyBlue;
          }
          else
          {
            color = Color.Tan;
            displayName += Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.1954");
          }
        }
        if (Game1.activeClickableMenu == null || !(Game1.activeClickableMenu is ItemGrabMenu))
          Game1.addHUDMessage(new HUDMessage(displayName, Math.Max(1, item.Stack), true, color, item));
        this.mostRecentlyGrabbedItem = item;
        if (obj1 != null & makeActiveObject && item.Stack <= 1)
        {
          int indexOfInventoryItem = this.getIndexOfInventoryItem(item);
          Item obj2 = this.items[this.currentToolIndex];
          this.items[this.currentToolIndex] = this.items[indexOfInventoryItem];
          this.items[indexOfInventoryItem] = obj2;
        }
      }
      if (item is Object && !item.hasBeenInInventory)
        this.checkForQuestComplete((NPC) null, item.parentSheetIndex, item.Stack, item, "", 10, -1);
      item.hasBeenInInventory = true;
      return flag;
    }

    public int getIndexOfInventoryItem(Item item)
    {
      for (int index = 0; index < this.items.Count; ++index)
      {
        if (this.items[index] == item || this.items[index] != null && item != null && item.canStackWith(this.items[index]))
          return index;
      }
      return -1;
    }

    public void reduceActiveItemByOne()
    {
      if (this.CurrentItem == null)
        return;
      --this.CurrentItem.Stack;
      if (this.CurrentItem.Stack > 0)
        return;
      this.removeItemFromInventory(this.CurrentItem);
      this.showNotCarrying();
    }

    public bool removeItemsFromInventory(int index, int stack)
    {
      if (this.hasItemInInventory(index, stack, 0))
      {
        for (int index1 = 0; index1 < this.items.Count; ++index1)
        {
          if (this.items[index1] != null && this.items[index1] is Object && (this.items[index1] as Object).parentSheetIndex == index)
          {
            if (this.items[index1].Stack > stack)
            {
              this.items[index1].Stack -= stack;
              return true;
            }
            stack -= this.items[index1].Stack;
            this.items[index1] = (Item) null;
          }
          if (stack <= 0)
            return true;
        }
      }
      return false;
    }

    public Item addItemToInventory(Item item, int position)
    {
      if (item != null && item is Object && (item as Object).specialItem)
      {
        if ((item as Object).bigCraftable)
        {
          if (!this.specialBigCraftables.Contains((item as Object).parentSheetIndex))
            this.specialBigCraftables.Add((item as Object).parentSheetIndex);
        }
        else if (!this.specialItems.Contains((item as Object).parentSheetIndex))
          this.specialItems.Add((item as Object).parentSheetIndex);
      }
      if (position < 0 || position >= this.items.Count)
        return item;
      if (this.items[position] == null)
      {
        this.items[position] = item;
        return (Item) null;
      }
      if (item != null && this.items[position].maximumStackSize() != -1 && this.items[position].Name.Equals(item.Name) && (!(item is Object) || !(this.items[position] is Object) || (item as Object).quality == (this.items[position] as Object).quality))
      {
        int stack = this.items[position].addToStack(item.getStack());
        if (stack <= 0)
          return (Item) null;
        item.Stack = stack;
        return item;
      }
      Item obj = this.items[position];
      this.items[position] = item;
      return obj;
    }

    public void removeItemFromInventory(Item which)
    {
      int num = this.items.IndexOf(which);
      if (num < 0 || num >= this.items.Count)
        return;
      this.items[this.items.IndexOf(which)] = (Item) null;
    }

    public Item removeItemFromInventory(int whichItemIndex)
    {
      if (whichItemIndex < 0 || whichItemIndex >= this.items.Count || this.items[whichItemIndex] == null)
        return (Item) null;
      Item obj = this.items[whichItemIndex];
      this.items[whichItemIndex] = (Item) null;
      return obj;
    }

    public bool isMarried()
    {
      if (this.spouse != null)
        return !this.spouse.Contains("engaged");
      return false;
    }

    public void removeFirstOfThisItemFromInventory(int parentSheetIndexOfItem)
    {
      if (this.ActiveObject != null && this.ActiveObject.ParentSheetIndex == parentSheetIndexOfItem)
      {
        --this.ActiveObject.Stack;
        if (this.ActiveObject.Stack > 0)
          return;
        this.ActiveObject = (Object) null;
        this.showNotCarrying();
      }
      else
      {
        for (int index = 0; index < this.items.Count; ++index)
        {
          if (this.items[index] != null && this.items[index] is Object && ((Object) this.items[index]).ParentSheetIndex == parentSheetIndexOfItem)
          {
            --this.items[index].Stack;
            if (this.items[index].Stack > 0)
              break;
            this.items[index] = (Item) null;
            break;
          }
        }
      }
    }

    public bool hasCoopDweller(string type)
    {
      foreach (CoopDweller coopDweller in this.coopDwellers)
      {
        if (coopDweller.type.Equals(type))
          return true;
      }
      return false;
    }

    public void changeShirt(int whichShirt)
    {
      if (whichShirt < 0)
        whichShirt = FarmerRenderer.shirtsTexture.Height / 32 * (FarmerRenderer.shirtsTexture.Width / 8) - 1;
      else if (whichShirt > FarmerRenderer.shirtsTexture.Height / 32 * (FarmerRenderer.shirtsTexture.Width / 8) - 1)
        whichShirt = 0;
      this.shirt = whichShirt;
      this.FarmerRenderer.changeShirt(whichShirt);
    }

    public void changeHairStyle(int whichHair)
    {
      if (whichHair < 0)
        whichHair = FarmerRenderer.hairStylesTexture.Height / 96 * 8 - 1;
      else if (whichHair > FarmerRenderer.hairStylesTexture.Height / 96 * 8 - 1)
        whichHair = 0;
      this.hair = whichHair;
    }

    public void changeShoeColor(int which)
    {
      this.FarmerRenderer.recolorShoes(which);
    }

    public void changeHairColor(Color c)
    {
      this.hairstyleColor = c;
    }

    public void changePants(Color color)
    {
      this.pantsColor = color;
    }

    public void changeHat(int newHat)
    {
      if (newHat < 0)
        this.hat = (Hat) null;
      else
        this.hat = new Hat(newHat);
    }

    public void changeAccessory(int which)
    {
      if (which < -1)
        which = 18;
      if (which < -1)
        return;
      if (which >= 19)
        which = -1;
      this.accessory = which;
    }

    public void changeSkinColor(int which)
    {
      this.skin = this.FarmerRenderer.recolorSkin(which);
    }

    public bool hasDarkSkin()
    {
      return this.skin >= 4 && this.skin <= 8 || this.skin == 14;
    }

    public void changeEyeColor(Color c)
    {
      this.newEyeColor = c;
      this.FarmerRenderer.recolorEyes(c);
    }

    public int getHair()
    {
      if (this.hat == null || this.hat.skipHairDraw || this.bathingClothes)
        return this.hair;
      switch (this.hair)
      {
        case 1:
        case 5:
        case 6:
        case 9:
        case 11:
          return this.hair;
        case 3:
          return 11;
        case 17:
        case 20:
        case 23:
        case 24:
        case 25:
        case 27:
        case 28:
        case 29:
        case 30:
          return this.hair;
        case 18:
        case 19:
        case 21:
        case 31:
          return 23;
        default:
          return this.hair >= 16 ? 30 : 7;
      }
    }

    public void changeGender(bool male)
    {
      if (male)
      {
        this.isMale = true;
        this.FarmerRenderer.baseTexture = this.getTexture();
        this.FarmerRenderer.heightOffset = 0;
      }
      else
      {
        this.isMale = false;
        this.FarmerRenderer.heightOffset = 4;
        this.FarmerRenderer.baseTexture = this.getTexture();
      }
      this.changeShirt(this.shirt);
      this.changeEyeColor(this.newEyeColor);
    }

    public bool hasBarnDweller(string type)
    {
      foreach (BarnDweller barnDweller in this.barnDwellers)
      {
        if (barnDweller.type.Equals(type))
          return true;
      }
      return false;
    }

    public void changeFriendship(int amount, NPC n)
    {
      if (amount > 0 && n.name.Equals("Dwarf") && !this.canUnderstandDwarves)
        return;
      if (this.friendships.ContainsKey(n.name))
      {
        if (!n.datable || n.datingFarmer || this.spouse != null && this.spouse.Equals(n.name) || this.friendships[n.name][0] < 2000)
        {
          this.friendships[n.name][0] = Math.Max(0, Math.Min(this.friendships[n.name][0] + amount, (this.spouse == null || !n.name.Equals(this.spouse) ? 11 : 14) * 250 - 1));
          if (n.datable && !n.datingFarmer && (this.spouse == null || !this.spouse.Equals(n.name)))
            this.friendships[n.name][0] = Math.Min(2498, this.friendships[n.name][0]);
        }
        if (n.datable && this.friendships[n.name][0] >= 2000 && !this.hasOrWillReceiveMail("Bouquet"))
          Game1.addMailForTomorrow("Bouquet", false, false);
        if (!n.datable || this.friendships[n.name][0] < 2500 || this.hasOrWillReceiveMail("SeaAmulet"))
          return;
        Game1.addMailForTomorrow("SeaAmulet", false, false);
      }
      else
        Game1.debugOutput = "Tried to change friendship for a friend that wasn't there.";
    }

    public bool knowsRecipe(string name)
    {
      if (!this.craftingRecipes.Keys.Contains<string>(name.Replace(" Recipe", "")))
        return this.cookingRecipes.Keys.Contains<string>(name.Replace(" Recipe", ""));
      return true;
    }

    public Vector2 getUniformPositionAwayFromBox(int direction, int distance)
    {
      switch (this.facingDirection)
      {
        case 0:
          return new Vector2((float) this.GetBoundingBox().Center.X, (float) (this.GetBoundingBox().Y - distance));
        case 1:
          return new Vector2((float) (this.GetBoundingBox().Right + distance), (float) this.GetBoundingBox().Center.Y);
        case 2:
          return new Vector2((float) this.GetBoundingBox().Center.X, (float) (this.GetBoundingBox().Bottom + distance));
        case 3:
          return new Vector2((float) (this.GetBoundingBox().X - distance), (float) this.GetBoundingBox().Center.Y);
        default:
          return Vector2.Zero;
      }
    }

    public bool hasTalkedToFriendToday(string npcName)
    {
      return this.friendships.ContainsKey(npcName) && this.friendships[npcName][2] == 1;
    }

    public void talkToFriend(NPC n, int friendshipPointChange = 20)
    {
      if (!this.friendships.ContainsKey(n.name) || this.friendships[n.name][2] != 0)
        return;
      this.changeFriendship(friendshipPointChange, n);
      this.friendships[n.name][2] = 1;
    }

    public void moveRaft(GameLocation currentLocation, GameTime time)
    {
      float num = 0.2f;
      if (this.CanMove && Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.moveUpButton))
      {
        this.yVelocity = Math.Max(this.yVelocity - num, (float) ((double) Math.Abs(this.xVelocity) / 2.0 - 3.0));
        this.faceDirection(0);
      }
      if (this.CanMove && Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.moveRightButton))
      {
        this.xVelocity = Math.Min(this.xVelocity + num, (float) (3.0 - (double) Math.Abs(this.yVelocity) / 2.0));
        this.faceDirection(1);
      }
      if (this.CanMove && Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.moveDownButton))
      {
        this.yVelocity = Math.Min(this.yVelocity + num, (float) (3.0 - (double) Math.Abs(this.xVelocity) / 2.0));
        this.faceDirection(2);
      }
      if (this.CanMove && Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.moveLeftButton))
      {
        this.xVelocity = Math.Max(this.xVelocity - num, (float) ((double) Math.Abs(this.yVelocity) / 2.0 - 3.0));
        this.faceDirection(3);
      }
      Microsoft.Xna.Framework.Rectangle position = new Microsoft.Xna.Framework.Rectangle((int) this.position.X, (int) ((double) this.position.Y + (double) Game1.tileSize + (double) (Game1.tileSize / 4)), Game1.tileSize, Game1.tileSize);
      position.X += (int) Math.Ceiling((double) this.xVelocity);
      if (!currentLocation.isCollidingPosition(position, Game1.viewport, true))
        this.position.X += this.xVelocity;
      position.X -= (int) Math.Ceiling((double) this.xVelocity);
      position.Y += (int) Math.Floor((double) this.yVelocity);
      if (!currentLocation.isCollidingPosition(position, Game1.viewport, true))
        this.position.Y += this.yVelocity;
      if ((double) this.xVelocity != 0.0 || (double) this.yVelocity != 0.0)
      {
        this.raftPuddleCounter = this.raftPuddleCounter - time.ElapsedGameTime.Milliseconds;
        if (this.raftPuddleCounter <= 0)
        {
          this.raftPuddleCounter = 250;
          currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(0, 0, Game1.tileSize, Game1.tileSize), (float) (150.0 - ((double) Math.Abs(this.xVelocity) + (double) Math.Abs(this.yVelocity)) * 3.0), 8, 0, new Vector2((float) position.X, (float) (position.Y - Game1.tileSize)), false, Game1.random.NextDouble() < 0.5, 1f / 1000f, 0.01f, Color.White, 1f, 3f / 1000f, 0.0f, 0.0f, false));
          if (Game1.random.NextDouble() < 0.6)
            Game1.playSound("wateringCan");
          if (Game1.random.NextDouble() < 0.6)
            this.raftBobCounter = this.raftBobCounter / 2;
        }
      }
      this.raftBobCounter = this.raftBobCounter - time.ElapsedGameTime.Milliseconds;
      if (this.raftBobCounter <= 0)
      {
        this.raftBobCounter = Game1.random.Next(15, 28) * 100;
        if ((double) this.yOffset <= 0.0)
        {
          this.yOffset = 4f;
          currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(0, 0, Game1.tileSize, Game1.tileSize), (float) (150.0 - ((double) Math.Abs(this.xVelocity) + (double) Math.Abs(this.yVelocity)) * 3.0), 8, 0, new Vector2((float) position.X, (float) (position.Y - Game1.tileSize)), false, Game1.random.NextDouble() < 0.5, 1f / 1000f, 0.01f, Color.White, 1f, 3f / 1000f, 0.0f, 0.0f, false));
        }
        else
          this.yOffset = 0.0f;
      }
      if ((double) this.xVelocity > 0.0)
        this.xVelocity = Math.Max(0.0f, this.xVelocity - num / 2f);
      else if ((double) this.xVelocity < 0.0)
        this.xVelocity = Math.Min(0.0f, this.xVelocity + num / 2f);
      if ((double) this.yVelocity > 0.0)
      {
        this.yVelocity = Math.Max(0.0f, this.yVelocity - num / 2f);
      }
      else
      {
        if ((double) this.yVelocity >= 0.0)
          return;
        this.yVelocity = Math.Min(0.0f, this.yVelocity + num / 2f);
      }
    }

    public void warpFarmer(Warp w)
    {
      if (w == null || Game1.eventUp)
        return;
      this.Halt();
      Game1.warpFarmer(w.TargetName, w.TargetX, w.TargetY, w.flipFarmer);
      if (!Game1.currentLocation.Name.Equals("Town") && !Game1.jukeboxPlaying || (!Game1.getLocationFromName(w.TargetName).IsOutdoors || Game1.currentSong == null) || !Game1.currentSong.Name.Contains("town") && !Game1.jukeboxPlaying)
        return;
      Game1.changeMusicTrack("none");
    }

    public static void passOutFromTired(Farmer who)
    {
      if (who.isRidingHorse())
        who.getMount().dismount();
      if (Game1.activeClickableMenu != null)
      {
        Game1.activeClickableMenu.emergencyShutDown();
        Game1.exitActiveMenu();
      }
      Game1.warpFarmer((GameLocation) Utility.getHomeOfFarmer(who), (int) who.mostRecentBed.X / Game1.tileSize, (int) who.mostRecentBed.Y / Game1.tileSize, 2, false);
      Game1.newDay = true;
      who.currentLocation.lastTouchActionLocation = new Vector2((float) ((int) who.mostRecentBed.X / Game1.tileSize), (float) ((int) who.mostRecentBed.Y / Game1.tileSize));
      who.completelyStopAnimatingOrDoingAction();
      if (who.bathingClothes)
        who.changeOutOfSwimSuit();
      who.swimming = false;
      Game1.player.CanMove = false;
      Game1.changeMusicTrack("none");
      if (!(who.currentLocation is FarmHouse) && !(who.currentLocation is Cellar))
      {
        int num = Math.Min(1000, who.Money / 10);
        who.Money -= num;
        who.mailForTomorrow.Add("passedOut " + (object) num);
      }
      who.FarmerSprite.setCurrentSingleFrame(5, (short) 3000, false, false);
    }

    public static void doSleepEmote(Farmer who)
    {
      who.doEmote(24);
      who.yJumpVelocity = -2f;
    }

    public override Microsoft.Xna.Framework.Rectangle GetBoundingBox()
    {
      if (this.mount != null && !this.mount.dismounting)
        return this.mount.GetBoundingBox();
      return new Microsoft.Xna.Framework.Rectangle((int) this.position.X + Game1.tileSize / 8, (int) this.position.Y + this.sprite.getHeight() - Game1.tileSize / 2, Game1.tileSize * 3 / 4, Game1.tileSize / 2);
    }

    public string getPetName()
    {
      foreach (NPC character in Game1.getFarm().characters)
      {
        if (character is Pet)
          return character.name;
      }
      foreach (NPC character in Utility.getHomeOfFarmer(this).characters)
      {
        if (character is Pet)
          return character.name;
      }
      return "the Farm";
    }

    public string getPetDisplayName()
    {
      foreach (NPC character in Game1.getFarm().characters)
      {
        if (character is Pet)
          return character.displayName;
      }
      foreach (NPC character in Utility.getHomeOfFarmer(this).characters)
      {
        if (character is Pet)
          return character.displayName;
      }
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.1972");
    }

    public bool hasPet()
    {
      foreach (NPC character in Game1.getFarm().characters)
      {
        if (character is Pet)
          return true;
      }
      foreach (NPC character in Utility.getHomeOfFarmer(this).characters)
      {
        if (character is Pet)
          return true;
      }
      return false;
    }

    public bool movedDuringLastTick()
    {
      return !this.position.Equals(this.lastPosition);
    }

    public int CompareTo(object obj)
    {
      return ((Farmer) obj).saveTime - this.saveTime;
    }

    public override void draw(SpriteBatch b)
    {
      if (this.currentLocation == null || !this.currentLocation.Equals((object) Game1.currentLocation) && !this.IsMainPlayer)
        return;
      Vector2 origin = new Vector2(this.xOffset, (this.yOffset + (float) (Game1.tileSize * 2) - (float) (this.GetBoundingBox().Height / 2)) / (float) Game1.pixelZoom + (float) Game1.pixelZoom);
      this.numUpdatesSinceLastDraw = 0;
      PropertyValue propertyValue = (PropertyValue) null;
      Tile tile = Game1.currentLocation.Map.GetLayer("Buildings").PickTile(new Location(this.getStandingX(), this.getStandingY()), Game1.viewport.Size);
      if (this.isGlowing && this.coloredBorder)
        b.Draw(this.Sprite.Texture, new Vector2(this.getLocalPosition(Game1.viewport).X - (float) Game1.pixelZoom, this.getLocalPosition(Game1.viewport).Y - (float) Game1.pixelZoom), new Microsoft.Xna.Framework.Rectangle?(this.Sprite.SourceRect), this.glowingColor * this.glowingTransparency, 0.0f, Vector2.Zero, 1.1f, SpriteEffects.None, Math.Max(0.0f, (float) ((double) this.getStandingY() / 10000.0 - 1.0 / 1000.0)));
      else if (this.isGlowing && !this.coloredBorder)
        this.farmerRenderer.draw(b, this.FarmerSprite, this.FarmerSprite.SourceRect, this.getLocalPosition(Game1.viewport), origin, Math.Max(0.0f, (float) ((double) this.getStandingY() / 10000.0 + 0.000110000000859145)), this.glowingColor * this.glowingTransparency, this.rotation, this);
      if (tile != null)
        tile.TileIndexProperties.TryGetValue("Shadow", out propertyValue);
      if (propertyValue == null)
      {
        if (!this.temporarilyInvincible || this.temporaryInvincibilityTimer % 100 < 50)
          this.farmerRenderer.draw(b, this.FarmerSprite, this.FarmerSprite.SourceRect, this.getLocalPosition(Game1.viewport) + this.jitter + new Vector2(0.0f, (float) this.yJumpOffset), origin, Math.Max(0.0f, (float) ((double) this.getStandingY() / 10000.0 + 9.99999974737875E-05)), Color.White, this.rotation, this);
      }
      else
      {
        this.farmerRenderer.draw(b, this.FarmerSprite, this.FarmerSprite.SourceRect, this.getLocalPosition(Game1.viewport), origin, Math.Max(0.0f, (float) ((double) this.getStandingY() / 10000.0 + 9.99999974737875E-05)), Color.White, this.rotation, this);
        this.farmerRenderer.draw(b, this.FarmerSprite, this.FarmerSprite.SourceRect, this.getLocalPosition(Game1.viewport), origin, Math.Max(0.0f, (float) ((double) this.getStandingY() / 10000.0 + 0.000199999994947575)), Color.Black * 0.25f, this.rotation, this);
      }
      if (this.isRafting)
        b.Draw(Game1.toolSpriteSheet, this.getLocalPosition(Game1.viewport) + new Vector2(0.0f, this.yOffset), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.toolSpriteSheet, 1, -1, -1)), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, (float) ((double) this.getStandingY() / 10000.0 - 1.0 / 1000.0));
      if (Game1.activeClickableMenu == null && !Game1.eventUp && (this.IsMainPlayer && this.CurrentTool != null) && ((Game1.oldKBState.IsKeyDown(Keys.LeftShift) || Game1.options.alwaysShowToolHitLocation) && this.CurrentTool.doesShowTileLocationMarker()) && (!Game1.options.hideToolHitLocationWhenInMotion || !this.isMoving()))
      {
        Vector2 local = Game1.GlobalToLocal(Game1.viewport, !Utility.withinRadiusOfPlayer(Game1.getOldMouseX() + Game1.viewport.X, Game1.getOldMouseY() + Game1.viewport.Y, 1, this) || Game1.options.gamepadControls ? Utility.clampToTile(this.GetToolLocation(false)) : new Vector2((float) ((Game1.getOldMouseX() + Game1.viewport.X) / Game1.tileSize), (float) ((Game1.getOldMouseY() + Game1.viewport.Y) / Game1.tileSize)) * (float) Game1.tileSize);
        if (!Game1.wasMouseVisibleThisFrame || Game1.isAnyGamePadButtonBeingPressed())
          local = Game1.GlobalToLocal(Game1.viewport, Utility.clampToTile(this.GetToolLocation(false)));
        b.Draw(Game1.mouseCursors, local, new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 29, -1, -1)), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, (float) (((double) this.GetToolLocation(false).Y + (double) Game1.tileSize) / 10000.0));
      }
      if (this.IsEmoting)
      {
        Vector2 localPosition = this.getLocalPosition(Game1.viewport);
        localPosition.Y -= (float) (Game1.tileSize * 2 + Game1.tileSize / 2);
        b.Draw(Game1.emoteSpriteSheet, localPosition, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(this.CurrentEmoteIndex * (Game1.tileSize / 4) % Game1.emoteSpriteSheet.Width, this.CurrentEmoteIndex * (Game1.tileSize / 4) / Game1.emoteSpriteSheet.Width * (Game1.tileSize / 4), Game1.tileSize / 4, Game1.tileSize / 4)), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, (float) this.getStandingY() / 10000f);
      }
      if (this.ActiveObject != null)
        Game1.drawPlayerHeldObject(this);
      if (!this.IsMainPlayer)
      {
        if (this.FarmerSprite.isOnToolAnimation())
          Game1.drawTool(this, this.FarmerSprite.CurrentToolIndex);
        if (new Microsoft.Xna.Framework.Rectangle((int) this.position.X - Game1.viewport.X, (int) this.position.Y - Game1.viewport.Y, Game1.tileSize, Game1.tileSize * 3 / 2).Contains(new Point(Game1.getOldMouseX(), Game1.getOldMouseY())))
          Game1.drawWithBorder(this.displayName, Color.Black, Color.White, this.getLocalPosition(Game1.viewport) + new Vector2((float) (Game1.tileSize / 2) - Game1.dialogueFont.MeasureString(this.displayName).X / 2f, -Game1.dialogueFont.MeasureString(this.displayName).Y));
      }
      if (this.sparklingText == null)
        return;
      this.sparklingText.draw(b, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2((float) (Game1.tileSize / 2) - this.sparklingText.textWidth / 2f, (float) (-Game1.tileSize * 2))));
    }

    public static void drinkGlug(Farmer who)
    {
      Color color = Color.LightBlue;
      if (who.itemToEat != null)
      {
        string s = ((IEnumerable<string>) who.itemToEat.Name.Split(' ')).Last<string>();
        // ISSUE: reference to a compiler-generated method
        uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(s);
        if (stringHash <= 2470525844U)
        {
          if (stringHash <= 948615682U)
          {
            if ((int) stringHash != 154965655)
            {
              if ((int) stringHash == 948615682 && s == "Tonic")
              {
                color = Color.Red;
                goto label_23;
              }
              else
                goto label_23;
            }
            else if (s == "Beer")
            {
              color = Color.Orange;
              goto label_23;
            }
            else
              goto label_23;
          }
          else if ((int) stringHash != 1702016080)
          {
            if ((int) stringHash == -1824441452 && s == "Wine")
            {
              color = Color.Purple;
              goto label_23;
            }
            else
              goto label_23;
          }
          else if (!(s == "Cola"))
            goto label_23;
        }
        else if (stringHash <= 3224132511U)
        {
          if ((int) stringHash != -1615951475)
          {
            if ((int) stringHash == -1070834785 && s == "Juice")
            {
              color = Color.LightGreen;
              goto label_23;
            }
            else
              goto label_23;
          }
          else if (s == "Remedy")
          {
            color = Color.LimeGreen;
            goto label_23;
          }
          else
            goto label_23;
        }
        else if ((int) stringHash != -734006079)
        {
          if ((int) stringHash == -277895998 && s == "Milk")
          {
            color = Color.White;
            goto label_23;
          }
          else
            goto label_23;
        }
        else if (!(s == "Coffee"))
          goto label_23;
        color = new Color(46, 20, 0);
      }
label_23:
      Game1.playSound("gulp");
      who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(653, 858, 1, 1), 9999f, 1, 1, who.position + new Vector2((float) (32 + Game1.random.Next(-2, 3) * 4), -48f), false, false, (float) ((double) who.getStandingY() / 10000.0 + 1.0 / 1000.0), 0.04f, color, 5f, 0.0f, 0.0f, 0.0f, false)
      {
        acceleration = new Vector2(0.0f, 0.5f)
      });
    }

    public bool isDivorced()
    {
      foreach (NPC allCharacter in Utility.getAllCharacters())
      {
        if (allCharacter.divorcedFromFarmer)
          return true;
      }
      return false;
    }

    public void wipeExMemories()
    {
      foreach (NPC allCharacter in Utility.getAllCharacters())
      {
        if (allCharacter.divorcedFromFarmer)
        {
          allCharacter.divorcedFromFarmer = false;
          allCharacter.datingFarmer = false;
          allCharacter.daysMarried = 0;
          try
          {
            this.friendships[allCharacter.name][0] = 0;
            this.friendships[allCharacter.name][1] = 0;
            this.friendships[allCharacter.name][2] = 0;
            this.friendships[allCharacter.name][3] = 0;
            this.friendships[allCharacter.name][4] = 0;
          }
          catch (Exception ex)
          {
          }
          allCharacter.CurrentDialogue.Clear();
          allCharacter.CurrentDialogue.Push(new Dialogue(Game1.content.LoadString("Strings\\Characters:WipedMemory"), allCharacter));
        }
      }
    }

    public void getRidOfChildren()
    {
      for (int index = Utility.getHomeOfFarmer(this).characters.Count<NPC>() - 1; index >= 0; --index)
      {
        if (Utility.getHomeOfFarmer(this).characters[index] is Child && (Utility.getHomeOfFarmer(this).characters[index] as Child).isChildOf(this))
          Utility.getHomeOfFarmer(this).characters.RemoveAt(index);
      }
      for (int index = Game1.getLocationFromName("Farm").characters.Count<NPC>() - 1; index >= 0; --index)
      {
        if (Game1.getLocationFromName("Farm").characters[index] is Child && (Game1.getLocationFromName("Farm").characters[index] as Child).isChildOf(this))
          Game1.getLocationFromName("Farm").characters.RemoveAt(index);
      }
    }

    public void animateOnce(int whichAnimation)
    {
      this.FarmerSprite.animateOnce(whichAnimation, 100f, 6);
      this.CanMove = false;
    }

    public static void showItemIntake(Farmer who)
    {
      TemporaryAnimatedSprite temporaryAnimatedSprite = (TemporaryAnimatedSprite) null;
      Object @object = who.mostRecentlyGrabbedItem == null || !(who.mostRecentlyGrabbedItem is Object) ? (who.ActiveObject == null ? (Object) null : who.ActiveObject) : (Object) who.mostRecentlyGrabbedItem;
      if (@object == null)
        return;
      switch (who.facingDirection)
      {
        case 0:
          switch (who.FarmerSprite.indexInCurrentAnimation)
          {
            case 1:
              temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, @object.parentSheetIndex, 16, 16), 100f, 1, 0, who.position + new Vector2(0.0f, (float) (-Game1.tileSize + Game1.tileSize / 2)), false, false, (float) ((double) who.getStandingY() / 10000.0 - 1.0 / 1000.0), 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
              break;
            case 2:
              temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, @object.parentSheetIndex, 16, 16), 100f, 1, 0, who.position + new Vector2(0.0f, (float) (-Game1.tileSize + Game1.tileSize / 3)), false, false, (float) ((double) who.getStandingY() / 10000.0 - 1.0 / 1000.0), 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
              break;
            case 3:
              temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, @object.parentSheetIndex, 16, 16), 100f, 1, 0, who.position + new Vector2(0.0f, (float) (-Game1.tileSize * 2)), false, false, (float) ((double) who.getStandingY() / 10000.0 - 1.0 / 1000.0), 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
              break;
            case 4:
              temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, @object.parentSheetIndex, 16, 16), 200f, 1, 0, who.position + new Vector2(0.0f, (float) (-Game1.tileSize * 2 + 8)), false, false, (float) ((double) who.getStandingY() / 10000.0 - 1.0 / 1000.0), 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
              break;
            case 5:
              temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, @object.parentSheetIndex, 16, 16), 200f, 1, 0, who.position + new Vector2(0.0f, (float) (-Game1.tileSize * 2 + 8)), false, false, (float) ((double) who.getStandingY() / 10000.0 - 1.0 / 1000.0), 0.02f, Color.White, (float) Game1.pixelZoom, -0.02f, 0.0f, 0.0f, false);
              break;
          }
        case 1:
          switch (who.FarmerSprite.indexInCurrentAnimation)
          {
            case 1:
              temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, @object.parentSheetIndex, 16, 16), 100f, 1, 0, who.position + new Vector2((float) (Game1.tileSize / 2 - 4), (float) -Game1.tileSize), false, false, (float) ((double) who.getStandingY() / 10000.0 + 0.00999999977648258), 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
              break;
            case 2:
              temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, @object.parentSheetIndex, 16, 16), 100f, 1, 0, who.position + new Vector2((float) (Game1.tileSize / 2 - 8), (float) (-Game1.tileSize - 8)), false, false, (float) ((double) who.getStandingY() / 10000.0 + 0.00999999977648258), 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
              break;
            case 3:
              temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, @object.parentSheetIndex, 16, 16), 100f, 1, 0, who.position + new Vector2(4f, (float) (-Game1.tileSize * 2)), false, false, (float) ((double) who.getStandingY() / 10000.0 + 0.00999999977648258), 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
              break;
            case 4:
              temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, @object.parentSheetIndex, 16, 16), 200f, 1, 0, who.position + new Vector2(0.0f, (float) (-Game1.tileSize * 2 + 4)), false, false, (float) ((double) who.getStandingY() / 10000.0 + 0.00999999977648258), 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
              break;
            case 5:
              temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, @object.parentSheetIndex, 16, 16), 200f, 1, 0, who.position + new Vector2(0.0f, (float) (-Game1.tileSize * 2 + 4)), false, false, (float) ((double) who.getStandingY() / 10000.0 + 0.00999999977648258), 0.02f, Color.White, (float) Game1.pixelZoom, -0.02f, 0.0f, 0.0f, false);
              break;
          }
        case 2:
          switch (who.FarmerSprite.indexInCurrentAnimation)
          {
            case 1:
              temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, @object.parentSheetIndex, 16, 16), 100f, 1, 0, who.position + new Vector2(0.0f, (float) (-Game1.tileSize + Game1.tileSize / 2)), false, false, (float) ((double) who.getStandingY() / 10000.0 + 0.00999999977648258), 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
              break;
            case 2:
              temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, @object.parentSheetIndex, 16, 16), 100f, 1, 0, who.position + new Vector2(0.0f, (float) (-Game1.tileSize + Game1.tileSize / 3)), false, false, (float) ((double) who.getStandingY() / 10000.0 + 0.00999999977648258), 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
              break;
            case 3:
              temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, @object.parentSheetIndex, 16, 16), 100f, 1, 0, who.position + new Vector2(0.0f, (float) (-Game1.tileSize * 2)), false, false, (float) ((double) who.getStandingY() / 10000.0 + 0.00999999977648258), 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
              break;
            case 4:
              temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, @object.parentSheetIndex, 16, 16), 200f, 1, 0, who.position + new Vector2(0.0f, (float) (-Game1.tileSize * 2 + 8)), false, false, (float) ((double) who.getStandingY() / 10000.0 + 0.00999999977648258), 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
              break;
            case 5:
              temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, @object.parentSheetIndex, 16, 16), 200f, 1, 0, who.position + new Vector2(0.0f, (float) (-Game1.tileSize * 2 + 8)), false, false, (float) ((double) who.getStandingY() / 10000.0 + 0.00999999977648258), 0.02f, Color.White, (float) Game1.pixelZoom, -0.02f, 0.0f, 0.0f, false);
              break;
          }
        case 3:
          switch (who.FarmerSprite.indexInCurrentAnimation)
          {
            case 1:
              temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, @object.parentSheetIndex, 16, 16), 100f, 1, 0, who.position + new Vector2((float) (-Game1.tileSize / 2), (float) -Game1.tileSize), false, false, (float) ((double) who.getStandingY() / 10000.0 + 0.00999999977648258), 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
              break;
            case 2:
              temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, @object.parentSheetIndex, 16, 16), 100f, 1, 0, who.position + new Vector2((float) (-Game1.tileSize / 2 + 4), (float) (-Game1.tileSize - 12)), false, false, (float) ((double) who.getStandingY() / 10000.0 + 0.00999999977648258), 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
              break;
            case 3:
              temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, @object.parentSheetIndex, 16, 16), 100f, 1, 0, who.position + new Vector2((float) (-Game1.tileSize / 4), (float) (-Game1.tileSize * 2)), false, false, (float) ((double) who.getStandingY() / 10000.0 + 0.00999999977648258), 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
              break;
            case 4:
              temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, @object.parentSheetIndex, 16, 16), 200f, 1, 0, who.position + new Vector2(0.0f, (float) (-Game1.tileSize * 2 + 4)), false, false, (float) ((double) who.getStandingY() / 10000.0 + 0.00999999977648258), 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
              break;
            case 5:
              temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, @object.parentSheetIndex, 16, 16), 200f, 1, 0, who.position + new Vector2(0.0f, (float) (-Game1.tileSize * 2 + 4)), false, false, (float) ((double) who.getStandingY() / 10000.0 + 0.00999999977648258), 0.02f, Color.White, (float) Game1.pixelZoom, -0.02f, 0.0f, 0.0f, false);
              break;
          }
      }
      if ((@object.Equals((object) who.ActiveObject) || who.ActiveObject != null && @object != null && @object.ParentSheetIndex == who.ActiveObject.parentSheetIndex) && who.FarmerSprite.indexInCurrentAnimation == 5)
        temporaryAnimatedSprite = (TemporaryAnimatedSprite) null;
      if (temporaryAnimatedSprite != null)
        who.currentLocation.temporarySprites.Add(temporaryAnimatedSprite);
      if (who.mostRecentlyGrabbedItem is ColoredObject && temporaryAnimatedSprite != null)
        who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, @object.parentSheetIndex + 1, 16, 16), temporaryAnimatedSprite.interval, 1, 0, temporaryAnimatedSprite.Position, false, false, temporaryAnimatedSprite.layerDepth + 0.0001f, temporaryAnimatedSprite.alphaFade, (who.mostRecentlyGrabbedItem as ColoredObject).color, (float) Game1.pixelZoom, temporaryAnimatedSprite.scaleChange, 0.0f, 0.0f, false));
      if (who.FarmerSprite.indexInCurrentAnimation != 5)
        return;
      who.Halt();
      who.FarmerSprite.CurrentAnimation = (List<FarmerSprite.AnimationFrame>) null;
    }

    public static void showSwordSwipe(Farmer who)
    {
      TemporaryAnimatedSprite temporaryAnimatedSprite1 = (TemporaryAnimatedSprite) null;
      bool flag = who.CurrentTool != null && who.CurrentTool is MeleeWeapon && (who.CurrentTool as MeleeWeapon).type == 1;
      Vector2 toolLocation = who.GetToolLocation(true);
      if (who.CurrentTool != null && who.CurrentTool is MeleeWeapon)
        (who.CurrentTool as MeleeWeapon).DoDamage(who.currentLocation, (int) toolLocation.X, (int) toolLocation.Y, who.facingDirection, 1, who);
      switch (who.facingDirection)
      {
        case 0:
          switch (who.FarmerSprite.indexInCurrentAnimation)
          {
            case 0:
              if (flag)
              {
                who.yVelocity = 0.6f;
                break;
              }
              break;
            case 1:
              who.yVelocity = flag ? -0.5f : 0.5f;
              break;
            case 5:
              who.yVelocity = -0.3f;
              temporaryAnimatedSprite1 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(518, 274, 23, 31), who.position + new Vector2(0.0f, -32f) * (float) Game1.pixelZoom, false, 0.07f, Color.White)
              {
                scale = (float) Game1.pixelZoom,
                animationLength = 1,
                interval = (float) who.FarmerSprite.CurrentAnimationFrame.milliseconds,
                alpha = 0.5f,
                rotation = 3.926991f
              };
              break;
          }
        case 1:
          switch (who.FarmerSprite.indexInCurrentAnimation)
          {
            case 0:
              if (flag)
              {
                who.xVelocity = 0.6f;
                break;
              }
              break;
            case 1:
              who.xVelocity = flag ? -0.5f : 0.5f;
              break;
            case 5:
              who.xVelocity = -0.3f;
              temporaryAnimatedSprite1 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(518, 274, 23, 31), who.position + new Vector2(4f, -12f) * (float) Game1.pixelZoom, false, 0.07f, Color.White)
              {
                scale = (float) Game1.pixelZoom,
                animationLength = 1,
                interval = (float) who.FarmerSprite.CurrentAnimationFrame.milliseconds,
                alpha = 0.5f
              };
              break;
          }
        case 2:
          switch (who.FarmerSprite.indexInCurrentAnimation)
          {
            case 0:
              if (flag)
              {
                who.yVelocity = -0.6f;
                break;
              }
              break;
            case 1:
              who.yVelocity = flag ? 0.5f : -0.5f;
              break;
            case 5:
              who.yVelocity = 0.3f;
              temporaryAnimatedSprite1 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(503, 256, 42, 17), who.position + new Vector2(-16f, -2f) * (float) Game1.pixelZoom, false, 0.07f, Color.White)
              {
                scale = (float) Game1.pixelZoom,
                animationLength = 1,
                interval = (float) who.FarmerSprite.CurrentAnimationFrame.milliseconds,
                alpha = 0.5f,
                layerDepth = (float) (((double) who.position.Y + (double) Game1.tileSize) / 10000.0)
              };
              break;
          }
        case 3:
          switch (who.FarmerSprite.indexInCurrentAnimation)
          {
            case 0:
              if (flag)
              {
                who.xVelocity = -0.6f;
                break;
              }
              break;
            case 1:
              who.xVelocity = flag ? 0.5f : -0.5f;
              break;
            case 5:
              who.xVelocity = 0.3f;
              TemporaryAnimatedSprite temporaryAnimatedSprite2 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(518, 274, 23, 31), who.position + new Vector2(-15f, -12f) * (float) Game1.pixelZoom, false, 0.07f, Color.White);
              temporaryAnimatedSprite2.scale = (float) Game1.pixelZoom;
              temporaryAnimatedSprite2.animationLength = 1;
              temporaryAnimatedSprite2.interval = (float) who.FarmerSprite.CurrentAnimationFrame.milliseconds;
              int num1 = 1;
              temporaryAnimatedSprite2.flipped = num1 != 0;
              double num2 = 0.5;
              temporaryAnimatedSprite2.alpha = (float) num2;
              temporaryAnimatedSprite1 = temporaryAnimatedSprite2;
              break;
          }
      }
      if (temporaryAnimatedSprite1 == null)
        return;
      if (who.CurrentTool != null && who.CurrentTool is MeleeWeapon && who.CurrentTool.initialParentTileIndex == 4)
        temporaryAnimatedSprite1.color = Color.HotPink;
      who.currentLocation.temporarySprites.Add(temporaryAnimatedSprite1);
    }

    public static void showToolSwipeEffect(Farmer who)
    {
      if (who.CurrentTool != null && who.CurrentTool is WateringCan)
      {
        int facingDirection = who.FacingDirection;
      }
      else
      {
        switch (who.FacingDirection)
        {
          case 0:
            who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(18, who.position + new Vector2(0.0f, (float) (-Game1.tileSize * 2 - 4)), Color.White, 4, false, (double) who.stamina <= 0.0 ? 100f : 50f, 0, Game1.tileSize, 1f, Game1.tileSize, 0)
            {
              layerDepth = (float) ((double) (who.getStandingY() - Game1.tileSize / 7) / 10000.0)
            });
            break;
          case 1:
            who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(15, who.position + new Vector2(20f, (float) (-Game1.tileSize * 2 - 4)), Color.White, 4, false, (double) who.stamina <= 0.0 ? 80f : 40f, 0, Game1.tileSize * 2, 1f, Game1.tileSize * 2, 0)
            {
              layerDepth = (float) ((double) (who.GetBoundingBox().Bottom + 1) / 10000.0)
            });
            break;
          case 2:
            who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(19, who.position + new Vector2(-4f, (float) (-Game1.tileSize * 2)), Color.White, 4, false, (double) who.stamina <= 0.0 ? 80f : 40f, 0, Game1.tileSize * 2, 1f, Game1.tileSize * 2, 0)
            {
              layerDepth = (float) ((double) (who.GetBoundingBox().Bottom + 1) / 10000.0)
            });
            break;
          case 3:
            who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(15, who.position + new Vector2((float) (-Game1.tileSize - 28), (float) (-Game1.tileSize * 2 - 4)), Color.White, 4, true, (double) who.stamina <= 0.0 ? 80f : 40f, 0, Game1.tileSize * 2, 1f, Game1.tileSize * 2, 0)
            {
              layerDepth = (float) ((double) (who.GetBoundingBox().Bottom + 1) / 10000.0)
            });
            break;
        }
      }
    }

    public static void canMoveNow(Farmer who)
    {
      who.CanMove = true;
      who.usingTool = false;
      who.usingSlingshot = false;
      who.FarmerSprite.pauseForSingleAnimation = false;
      who.yVelocity = 0.0f;
      who.xVelocity = 0.0f;
    }

    public static void useTool(Farmer who)
    {
      if (who.toolOverrideFunction != null)
      {
        who.toolOverrideFunction(who);
      }
      else
      {
        if (who.CurrentTool == null)
          return;
        float stamina = who.stamina;
        who.CurrentTool.DoFunction(who.currentLocation, (int) who.GetToolLocation(false).X, (int) who.GetToolLocation(false).Y, 1, who);
        who.lastClick = Vector2.Zero;
        who.checkForExhaustion(stamina);
        Game1.toolHold = 0.0f;
      }
    }

    public void checkForExhaustion(float oldStamina)
    {
      if ((double) this.stamina <= 0.0 && (double) oldStamina > 0.0)
      {
        if (!this.exhausted)
          Game1.showGlobalMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.1986"));
        this.setRunning(false, false);
        this.doEmote(36);
      }
      else if ((double) this.stamina <= 15.0 && (double) oldStamina > 15.0)
        Game1.showGlobalMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.1987"));
      if ((double) this.stamina <= 0.0)
        this.exhausted = true;
      if ((double) this.stamina > -15.0)
        return;
      Game1.farmerShouldPassOut = true;
    }

    public void setMoving(byte command)
    {
      bool flag = false;
      if (this.movementDirections.Count < 2)
      {
        if ((int) command == 1 && !this.movementDirections.Contains(0) && !this.movementDirections.Contains(2))
        {
          this.movementDirections.Insert(0, 0);
          flag = true;
        }
        if ((int) command == 2 && !this.movementDirections.Contains(1) && !this.movementDirections.Contains(3))
        {
          this.movementDirections.Insert(0, 1);
          flag = true;
        }
        if ((int) command == 4 && !this.movementDirections.Contains(2) && !this.movementDirections.Contains(0))
        {
          this.movementDirections.Insert(0, 2);
          flag = true;
        }
        if ((int) command == 8 && !this.movementDirections.Contains(3) && !this.movementDirections.Contains(1))
        {
          this.movementDirections.Insert(0, 3);
          flag = true;
        }
      }
      if ((int) command == 33)
      {
        this.movementDirections.Remove(0);
        flag = true;
      }
      if ((int) command == 34)
      {
        this.movementDirections.Remove(1);
        flag = true;
      }
      if ((int) command == 36)
      {
        this.movementDirections.Remove(2);
        flag = true;
      }
      if ((int) command == 40)
      {
        this.movementDirections.Remove(3);
        flag = true;
      }
      if ((int) command == 16)
      {
        this.setRunning(true, false);
        flag = true;
      }
      else if ((int) command == 48)
      {
        this.setRunning(false, false);
        flag = true;
      }
      if (((int) command & 64) == 64)
      {
        this.Halt();
        this.running = false;
        flag = true;
      }
      if (Game1.IsClient & flag && ((int) command & 32) != 32)
        this.timeOfLastPositionPacket = 60f;
      if (!(Game1.IsServer & flag))
        return;
      MultiplayerUtility.broadcastFarmerMovement(this.uniqueMultiplayerID, command, this.currentLocation.name);
    }

    public void toolPowerIncrease()
    {
      if (this.toolPower == 0)
        this.toolPitchAccumulator = 0;
      this.toolPower = this.toolPower + 1;
      if (this.CurrentTool is Pickaxe && this.toolPower == 1)
        this.toolPower = this.toolPower + 2;
      Color color = Color.White;
      int num1 = this.FacingDirection == 0 ? 4 : (this.FacingDirection == 2 ? 2 : 0);
      switch (this.toolPower)
      {
        case 1:
          color = Color.Orange;
          if (!(this.CurrentTool is WateringCan))
            this.FarmerSprite.CurrentFrame = 72 + num1;
          this.jitterStrength = 0.25f;
          break;
        case 2:
          color = Color.LightSteelBlue;
          if (!(this.CurrentTool is WateringCan))
            ++this.FarmerSprite.CurrentFrame;
          this.jitterStrength = 0.5f;
          break;
        case 3:
          color = Color.Gold;
          this.jitterStrength = 1f;
          break;
        case 4:
          color = Color.Violet;
          this.jitterStrength = 2f;
          break;
      }
      int num2 = this.FacingDirection == 1 ? Game1.tileSize : (this.FacingDirection == 3 ? -Game1.tileSize : (this.FacingDirection == 2 ? Game1.tileSize / 2 : 0));
      int num3 = Game1.tileSize * 3;
      if (this.CurrentTool is WateringCan)
      {
        num2 = -num2;
        num3 = Game1.tileSize * 2;
      }
      Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(21, this.position - new Vector2((float) num2, (float) num3), color, 8, false, 70f, 0, Game1.tileSize, (float) ((double) this.getStandingY() / 10000.0 + 0.00499999988824129), Game1.tileSize * 2, 0));
      Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(192, 1152, Game1.tileSize, Game1.tileSize), 50f, 4, 0, this.position - new Vector2(this.FacingDirection == 1 ? 0.0f : (float) -Game1.tileSize, (float) (Game1.tileSize * 2)), false, this.FacingDirection == 1, (float) this.getStandingY() / 10000f, 0.01f, Color.White, 1f, 0.0f, 0.0f, 0.0f, false));
      if (Game1.soundBank == null)
        return;
      Cue cue = Game1.soundBank.GetCue("toolCharge");
      Random random = new Random(Game1.dayOfMonth + (int) this.position.X * 1000 + (int) this.position.Y);
      string name = "Pitch";
      double num4 = (double) (random.Next(12, 16) * 100 + this.toolPower * 100);
      cue.SetVariable(name, (float) num4);
      cue.Play();
    }

    public override void updatePositionFromServer(Vector2 position)
    {
      if (Game1.eventUp && !Game1.currentLocation.currentEvent.playerControlSequence)
        return;
      this.remotePosition = position;
      if (!Game1.IsClient || !Game1.client.isConnected)
        return;
      float num = (float) ((double) Game1.client.averageRoundtripTime / 2.0 * 60.0);
      if (this.movementDirections.Contains(0))
        this.remotePosition.Y -= num / 64f * this.getMovementSpeed();
      if (this.movementDirections.Contains(1))
        this.remotePosition.X += num / 64f * this.getMovementSpeed();
      if (this.movementDirections.Contains(2))
        this.remotePosition.Y += num / 64f * this.getMovementSpeed();
      if (!this.movementDirections.Contains(3))
        return;
      this.remotePosition.X -= num / 64f * this.getMovementSpeed();
    }

    public override void lerpPosition(Vector2 target)
    {
      if (target.Equals(Vector2.Zero))
        return;
      int num1 = (int) ((double) target.X - (double) this.position.X);
      if (Math.Abs(num1) > Game1.tileSize * 8)
        this.position.X = target.X;
      else
        this.position.X += (float) num1 * this.getMovementSpeed() * Math.Min(0.04f, this.timeOfLastPositionPacket / 40000f);
      int num2 = (int) ((double) target.Y - (double) this.position.Y);
      if (Math.Abs(num2) > Game1.tileSize * 8)
        this.position.Y = target.Y;
      else
        this.position.Y += (float) num2 * this.getMovementSpeed() * Math.Min(0.04f, this.timeOfLastPositionPacket / 40000f);
    }

    public void UpdateIfOtherPlayer(GameTime time)
    {
      if (this.currentLocation == null)
        return;
      this.FarmerSprite.setOwner(this);
      this.FarmerSprite.checkForSingleAnimation(time);
      this.timeOfLastPositionPacket = this.timeOfLastPositionPacket + (float) time.ElapsedGameTime.Milliseconds;
      if (!Game1.eventUp || Game1.currentLocation.currentEvent.playerControlSequence)
        this.lerpPosition(this.remotePosition);
      Vector2 position = this.position;
      this.MovePosition(time, Game1.viewport, this.currentLocation);
      this.rotation = 0.0f;
      if (this.movementDirections.Count == 0 && !this.FarmerSprite.pauseForSingleAnimation && !this.UsingTool)
        this.sprite.StopAnimation();
      if (Game1.IsServer && this.movementDirections.Count > 0)
        MultiplayerUtility.broadcastFarmerPosition(this.uniqueMultiplayerID, this.position, this.currentLocation.name);
      Game1.debugOutput = this.movementDirections.Count <= 0 ? "no movemement" : this.position.ToString();
      if (this.CurrentTool != null)
        this.CurrentTool.tickUpdate(time, this);
      else if (this.ActiveObject != null)
        this.ActiveObject.actionWhenBeingHeld(this);
      this.updateEmote(time);
      this.updateGlow();
    }

    public void forceCanMove()
    {
      this.forceTimePass = false;
      this.movementDirections.Clear();
      Game1.isEating = false;
      this.CanMove = true;
      Game1.freezeControls = false;
      this.freezePause = 0;
      this.usingTool = false;
      this.usingSlingshot = false;
      this.FarmerSprite.pauseForSingleAnimation = false;
      if (!(this.CurrentTool is FishingRod))
        return;
      (this.CurrentTool as FishingRod).isFishing = false;
    }

    public void dropItem(Item i)
    {
      if (i == null || !i.canBeDropped())
        return;
      Game1.createItemDebris(i.getOne(), this.getStandingPosition(), this.FacingDirection, (GameLocation) null);
    }

    public bool addEvent(string eventName, int daysActive)
    {
      if (this.activeDialogueEvents.ContainsKey(eventName))
        return false;
      this.activeDialogueEvents.Add(eventName, daysActive);
      return true;
    }

    public void dropObjectFromInventory(int parentSheetIndex, int quantity)
    {
      for (int index = 0; index < this.items.Count; ++index)
      {
        if (this.items[index] != null && this.items[index] is Object && (this.items[index] as Object).parentSheetIndex == parentSheetIndex)
        {
          while (quantity > 0)
          {
            this.dropItem(this.items[index].getOne());
            --this.items[index].Stack;
            --quantity;
            if (this.items[index].Stack <= 0)
            {
              this.items[index] = (Item) null;
              break;
            }
          }
          if (quantity <= 0)
            break;
        }
      }
    }

    public Vector2 getMostRecentMovementVector()
    {
      return new Vector2(this.position.X - this.lastPosition.X, this.position.Y - this.lastPosition.Y);
    }

    public void dropActiveItem()
    {
      if (this.CurrentItem == null || !this.CurrentItem.canBeDropped())
        return;
      Game1.createItemDebris(this.CurrentItem.getOne(), this.getStandingPosition(), this.FacingDirection, (GameLocation) null);
      this.reduceActiveItemByOne();
    }

    public static string getSkillNameFromIndex(int index)
    {
      switch (index)
      {
        case 0:
          return "Farming";
        case 1:
          return "Fishing";
        case 2:
          return "Foraging";
        case 3:
          return "Mining";
        case 4:
          return "Combat";
        case 5:
          return "Luck";
        default:
          return "";
      }
    }

    public static string getSkillDisplayNameFromIndex(int index)
    {
      switch (index)
      {
        case 0:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.1991");
        case 1:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.1993");
        case 2:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.1994");
        case 3:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.1992");
        case 4:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.1996");
        case 5:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.1995");
        default:
          return "";
      }
    }

    public override bool isMoving()
    {
      return this.movementDirections.Count > 0;
    }

    public bool hasCompletedCommunityCenter()
    {
      if (this.mailReceived.Contains("ccBoilerRoom") && this.mailReceived.Contains("ccCraftsRoom") && (this.mailReceived.Contains("ccPantry") && this.mailReceived.Contains("ccFishTank")) && this.mailReceived.Contains("ccVault"))
        return this.mailReceived.Contains("ccBulletin");
      return false;
    }

    public void Update(GameTime time, GameLocation location)
    {
      if (Game1.CurrentEvent == null && !this.bathingClothes)
        this.canOnlyWalk = false;
      if (this.exhausted && (double) this.stamina <= 1.0)
      {
        this.currentEyes = 4;
        this.blinkTimer = -1000;
      }
      if (this.noMovementPause > 0)
      {
        this.CanMove = false;
        this.noMovementPause = this.noMovementPause - time.ElapsedGameTime.Milliseconds;
        if (this.noMovementPause <= 0)
          this.CanMove = true;
      }
      if (this.freezePause > 0)
      {
        this.CanMove = false;
        this.freezePause = this.freezePause - time.ElapsedGameTime.Milliseconds;
        if (this.freezePause <= 0)
          this.CanMove = true;
      }
      if (this.sparklingText != null && this.sparklingText.update(time))
        this.sparklingText = (SparklingText) null;
      if (this.newLevelSparklingTexts.Count > 0 && this.sparklingText == null && (!this.usingTool && this.CanMove) && Game1.activeClickableMenu == null)
      {
        this.sparklingText = new SparklingText(Game1.dialogueFont, Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.2003", (object) Farmer.getSkillDisplayNameFromIndex(this.newLevelSparklingTexts.Peek())), Color.White, Color.White, true, 0.1, 2500, -1, 500);
        this.newLevelSparklingTexts.Dequeue();
      }
      if ((double) this.jitterStrength > 0.0)
        this.jitter = new Vector2((float) Game1.random.Next(-(int) ((double) this.jitterStrength * 100.0), (int) (((double) this.jitterStrength + 1.0) * 100.0)) / 100f, (float) Game1.random.Next(-(int) ((double) this.jitterStrength * 100.0), (int) (((double) this.jitterStrength + 1.0) * 100.0)) / 100f);
      this.blinkTimer = this.blinkTimer + time.ElapsedGameTime.Milliseconds;
      if (this.blinkTimer > 2200 && Game1.random.NextDouble() < 0.01)
      {
        this.blinkTimer = -150;
        this.currentEyes = 4;
      }
      else if (this.blinkTimer > -100)
        this.currentEyes = this.blinkTimer >= -50 ? (this.blinkTimer >= 0 ? 0 : 4) : 1;
      TimeSpan timeSpan;
      if (this.swimming)
      {
        timeSpan = time.TotalGameTime;
        this.yOffset = (float) Math.Cos(timeSpan.TotalMilliseconds / 2000.0) * (float) Game1.pixelZoom;
        int swimTimer1 = this.swimTimer;
        int swimTimer2 = this.swimTimer;
        timeSpan = time.ElapsedGameTime;
        int milliseconds = timeSpan.Milliseconds;
        this.swimTimer = swimTimer2 - milliseconds;
        if (this.timerSinceLastMovement == 0)
        {
          if (swimTimer1 > 400 && this.swimTimer <= 400)
            this.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(0, 0, Game1.tileSize, Game1.tileSize), (float) (150.0 - ((double) Math.Abs(this.xVelocity) + (double) Math.Abs(this.yVelocity)) * 3.0), 8, 0, new Vector2(this.position.X, (float) (this.getStandingY() - Game1.tileSize / 2)), false, Game1.random.NextDouble() < 0.5, 0.01f, 0.01f, Color.White, 1f, 3f / 1000f, 0.0f, 0.0f, false));
          if (this.swimTimer < 0)
          {
            this.swimTimer = 800;
            Game1.playSound("slosh");
            this.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(0, 0, Game1.tileSize, Game1.tileSize), (float) (150.0 - ((double) Math.Abs(this.xVelocity) + (double) Math.Abs(this.yVelocity)) * 3.0), 8, 0, new Vector2(this.position.X, (float) (this.getStandingY() - Game1.tileSize / 2)), false, Game1.random.NextDouble() < 0.5, 0.01f, 0.01f, Color.White, 1f, 3f / 1000f, 0.0f, 0.0f, false));
          }
        }
        else if (!Game1.eventUp && Game1.activeClickableMenu == null && !Game1.paused)
        {
          if (this.timerSinceLastMovement > 700)
            this.currentEyes = 4;
          if (this.timerSinceLastMovement > 800)
            this.currentEyes = 1;
          if (this.swimTimer < 0)
          {
            this.swimTimer = 100;
            if ((double) this.stamina < (double) this.maxStamina)
              this.stamina = this.stamina + 1f;
            if (this.health < this.maxHealth)
              this.health = this.health + 1;
          }
        }
      }
      this.FarmerSprite.setOwner(this);
      this.FarmerSprite.checkForSingleAnimation(time);
      if (Game1.IsClient && (!Game1.eventUp || location.currentEvent != null && location.currentEvent.playerControlSequence))
      {
        this.lerpPosition(this.remotePosition);
        double lastPositionPacket = (double) this.timeOfLastPositionPacket;
        timeSpan = time.ElapsedGameTime;
        double milliseconds = (double) timeSpan.Milliseconds;
        this.timeOfLastPositionPacket = (float) (lastPositionPacket + milliseconds);
      }
      if (this.CanMove)
      {
        this.rotation = 0.0f;
        if (this.health <= 0 && !Game1.killScreen)
        {
          this.CanMove = false;
          Game1.screenGlowOnce(Color.Red, true, 0.005f, 0.3f);
          Game1.killScreen = true;
          this.FarmerSprite.setCurrentFrame(5);
          this.jitterStrength = 1f;
          Game1.pauseTime = 3000f;
          Rumble.rumbleAndFade(0.75f, 1500f);
          this.freezePause = 8000;
          if (Game1.currentSong != null && Game1.currentSong.IsPlaying)
            Game1.currentSong.Stop(AudioStopOptions.Immediate);
          Game1.playSound("death");
          Game1.dialogueUp = false;
          ++Game1.stats.TimesUnconscious;
        }
        switch (this.getDirection())
        {
          case 0:
            location.isCollidingWithWarp(this.nextPosition(0));
            break;
          case 1:
            location.isCollidingWithWarp(this.nextPosition(1));
            break;
          case 2:
            location.isCollidingWithWarp(this.nextPosition(2));
            break;
          case 3:
            location.isCollidingWithWarp(this.nextPosition(3));
            break;
        }
        if (this.collisionNPC != null)
          this.collisionNPC.farmerPassesThrough = true;
        if (this.isMoving() && !this.isRidingHorse() && location.isCollidingWithCharacter(this.nextPosition(this.facingDirection)) != null)
        {
          int charactercollisionTimer = this.charactercollisionTimer;
          timeSpan = time.ElapsedGameTime;
          int milliseconds = timeSpan.Milliseconds;
          this.charactercollisionTimer = charactercollisionTimer + milliseconds;
          if (this.charactercollisionTimer > 400)
            location.isCollidingWithCharacter(this.nextPosition(this.facingDirection)).shake(50);
          if (this.charactercollisionTimer >= 1500 && this.collisionNPC == null)
          {
            this.collisionNPC = location.isCollidingWithCharacter(this.nextPosition(this.facingDirection));
            if (this.collisionNPC.name.Equals("Bouncer") && this.currentLocation != null && this.currentLocation.name.Equals("SandyHouse"))
            {
              this.collisionNPC.showTextAboveHead(Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.2010"), -1, 2, 3000, 0);
              this.collisionNPC = (NPC) null;
              this.charactercollisionTimer = 0;
            }
            else if (this.collisionNPC.name.Equals("Henchman") && this.currentLocation != null && this.currentLocation.name.Equals("WitchSwamp"))
            {
              this.collisionNPC = (NPC) null;
              this.charactercollisionTimer = 0;
            }
          }
        }
        else
        {
          this.charactercollisionTimer = 0;
          if (this.collisionNPC != null && location.isCollidingWithCharacter(this.nextPosition(this.facingDirection)) == null)
          {
            this.collisionNPC.farmerPassesThrough = false;
            this.collisionNPC = (NPC) null;
          }
        }
      }
      MeleeWeapon.weaponsTypeUpdate(time);
      if (!Game1.eventUp || !this.isMoving() || (this.currentLocation.currentEvent == null || this.currentLocation.currentEvent.playerControlSequence))
      {
        this.lastPosition = this.position;
        if (this.controller != null)
        {
          if (this.controller.update(time))
            this.controller = (PathFindController) null;
        }
        else if (this.controller == null)
          this.MovePosition(time, Game1.viewport, location);
      }
      if (this.lastPosition.Equals(this.position))
      {
        int sinceLastMovement = this.timerSinceLastMovement;
        timeSpan = time.ElapsedGameTime;
        int milliseconds = timeSpan.Milliseconds;
        this.timerSinceLastMovement = sinceLastMovement + milliseconds;
      }
      else
        this.timerSinceLastMovement = 0;
      if (Game1.IsServer && this.movementDirections.Count > 0)
        MultiplayerUtility.broadcastFarmerPosition(this.uniqueMultiplayerID, this.position, this.currentLocation.name);
      if (this.yJumpOffset != 0)
      {
        this.yJumpVelocity = this.yJumpVelocity - 0.5f;
        this.yJumpOffset = this.yJumpOffset - (int) this.yJumpVelocity;
        if (this.yJumpOffset >= 0)
        {
          this.yJumpOffset = 0;
          this.yJumpVelocity = 0.0f;
        }
      }
      this.updateEmote(time);
      this.updateGlow();
      for (int index = this.items.Count - 1; index >= 0; --index)
      {
        if (this.items[index] != null && this.items[index] is Tool)
          ((Tool) this.items[index]).tickUpdate(time, this);
      }
      if (this.rightRing != null)
        this.rightRing.update(time, location, this);
      if (this.leftRing == null)
        return;
      this.leftRing.update(time, location, this);
    }

    public void addQuest(int questID)
    {
      if (this.hasQuest(questID))
        return;
      this.questLog.Add(Quest.getQuestFromId(questID));
      Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.2011"), 2));
    }

    public void removeQuest(int questID)
    {
      for (int index = this.questLog.Count - 1; index >= 0; --index)
      {
        if (this.questLog[index].id == questID)
          this.questLog.RemoveAt(index);
      }
    }

    public void completeQuest(int questID)
    {
      for (int index = this.questLog.Count - 1; index >= 0; --index)
      {
        if (this.questLog[index].id == questID)
          this.questLog[index].questComplete();
      }
    }

    public bool hasQuest(int id)
    {
      for (int index = this.questLog.Count - 1; index >= 0; --index)
      {
        if (this.questLog[index].id == id)
          return true;
      }
      return false;
    }

    public bool hasNewQuestActivity()
    {
      foreach (Quest quest in this.questLog)
      {
        if (quest.showNew || quest.completed && !quest.destroy)
          return true;
      }
      return false;
    }

    public float getMovementSpeed()
    {
      float num;
      if (Game1.CurrentEvent == null || Game1.CurrentEvent.playerControlSequence)
      {
        this.movementMultiplier = 0.066f;
        num = Math.Max(1f, ((float) this.speed + (Game1.eventUp ? 0.0f : (float) this.addedSpeed + (this.isRidingHorse() ? 4.6f : this.temporarySpeedBuff))) * this.movementMultiplier * (float) Game1.currentGameTime.ElapsedGameTime.Milliseconds);
        if (this.movementDirections.Count > 1)
          num = 0.7f * num;
      }
      else
      {
        num = Math.Max(1f, (float) this.speed + (Game1.eventUp ? (float) Math.Max(0, Game1.CurrentEvent.farmerAddedSpeed - 2) : (float) this.addedSpeed + (this.isRidingHorse() ? 5f : this.temporarySpeedBuff)));
        if (this.movementDirections.Count > 1)
          num = (float) Math.Max(1, (int) Math.Sqrt(2.0 * ((double) num * (double) num)) / 2);
      }
      return num;
    }

    public bool isWearingRing(int ringIndex)
    {
      if (this.rightRing != null && this.rightRing.indexInTileSheet == ringIndex)
        return true;
      if (this.leftRing != null)
        return this.leftRing.indexInTileSheet == ringIndex;
      return false;
    }

    public override void Halt()
    {
      if (!this.FarmerSprite.pauseForSingleAnimation)
        base.Halt();
      this.movementDirections.Clear();
      this.stopJittering();
      this.armOffset = Vector2.Zero;
      if (!this.isRidingHorse())
        return;
      this.mount.Halt();
      this.mount.sprite.CurrentAnimation = (List<FarmerSprite.AnimationFrame>) null;
    }

    public void stopJittering()
    {
      this.jitterStrength = 0.0f;
      this.jitter = Vector2.Zero;
    }

    public override Microsoft.Xna.Framework.Rectangle nextPosition(int direction)
    {
      Microsoft.Xna.Framework.Rectangle boundingBox = this.GetBoundingBox();
      switch (direction)
      {
        case 0:
          boundingBox.Y -= (int) Math.Ceiling((double) this.getMovementSpeed());
          break;
        case 1:
          boundingBox.X += (int) Math.Ceiling((double) this.getMovementSpeed());
          break;
        case 2:
          boundingBox.Y += (int) Math.Ceiling((double) this.getMovementSpeed());
          break;
        case 3:
          boundingBox.X -= (int) Math.Ceiling((double) this.getMovementSpeed());
          break;
      }
      return boundingBox;
    }

    public Microsoft.Xna.Framework.Rectangle nextPositionHalf(int direction)
    {
      Microsoft.Xna.Framework.Rectangle boundingBox = this.GetBoundingBox();
      switch (direction)
      {
        case 0:
          boundingBox.Y -= (int) Math.Ceiling((double) this.getMovementSpeed() / 2.0);
          break;
        case 1:
          boundingBox.X += (int) Math.Ceiling((double) this.getMovementSpeed() / 2.0);
          break;
        case 2:
          boundingBox.Y += (int) Math.Ceiling((double) this.getMovementSpeed() / 2.0);
          break;
        case 3:
          boundingBox.X -= (int) Math.Ceiling((double) this.getMovementSpeed() / 2.0);
          break;
      }
      return boundingBox;
    }

    public int getProfessionForSkill(int skillType, int skillLevel)
    {
      if (skillLevel == 5)
      {
        switch (skillType)
        {
          case 0:
            if (this.professions.Contains(0))
              return 0;
            if (this.professions.Contains(1))
              return 1;
            break;
          case 1:
            if (this.professions.Contains(6))
              return 6;
            if (this.professions.Contains(7))
              return 7;
            break;
          case 2:
            if (this.professions.Contains(12))
              return 12;
            if (this.professions.Contains(13))
              return 13;
            break;
          case 3:
            if (this.professions.Contains(18))
              return 18;
            if (this.professions.Contains(19))
              return 19;
            break;
          case 4:
            if (this.professions.Contains(24))
              return 24;
            if (this.professions.Contains(25))
              return 25;
            break;
        }
      }
      else if (skillLevel == 10)
      {
        switch (skillType)
        {
          case 0:
            if (this.professions.Contains(1))
            {
              if (this.professions.Contains(4))
                return 4;
              if (this.professions.Contains(5))
                return 5;
              break;
            }
            if (this.professions.Contains(2))
              return 2;
            if (this.professions.Contains(3))
              return 3;
            break;
          case 1:
            if (this.professions.Contains(6))
            {
              if (this.professions.Contains(8))
                return 8;
              if (this.professions.Contains(9))
                return 9;
              break;
            }
            if (this.professions.Contains(10))
              return 10;
            if (this.professions.Contains(11))
              return 11;
            break;
          case 2:
            if (this.professions.Contains(12))
            {
              if (this.professions.Contains(14))
                return 14;
              if (this.professions.Contains(15))
                return 15;
              break;
            }
            if (this.professions.Contains(16))
              return 16;
            if (this.professions.Contains(17))
              return 17;
            break;
          case 3:
            if (this.professions.Contains(18))
            {
              if (this.professions.Contains(20))
                return 20;
              if (this.professions.Contains(21))
                return 21;
              break;
            }
            if (this.professions.Contains(23))
              return 23;
            if (this.professions.Contains(22))
              return 22;
            break;
          case 4:
            if (this.professions.Contains(24))
            {
              if (this.professions.Contains(26))
                return 26;
              if (this.professions.Contains(27))
                return 27;
              break;
            }
            if (this.professions.Contains(28))
              return 28;
            if (this.professions.Contains(29))
              return 29;
            break;
        }
      }
      return -1;
    }

    public void behaviorOnMovement(int direction)
    {
    }

    public override void MovePosition(GameTime time, xTile.Dimensions.Rectangle viewport, GameLocation currentLocation)
    {
      if (Game1.activeClickableMenu != null && (!Game1.eventUp || Game1.CurrentEvent.playerControlSequence))
        return;
      if (this.isRafting)
      {
        this.moveRaft(currentLocation, time);
      }
      else
      {
        if ((double) this.xVelocity != 0.0 || (double) this.yVelocity != 0.0)
        {
          if (double.IsNaN((double) this.xVelocity) || double.IsNaN((double) this.yVelocity))
          {
            this.xVelocity = 0.0f;
            this.yVelocity = 0.0f;
          }
          Microsoft.Xna.Framework.Rectangle boundingBox = this.GetBoundingBox();
          boundingBox.X += (int) this.xVelocity;
          boundingBox.Y -= (int) this.yVelocity;
          if (!currentLocation.isCollidingPosition(boundingBox, viewport, true, -1, false, (Character) this))
          {
            this.position.X += this.xVelocity;
            this.position.Y -= this.yVelocity;
            this.xVelocity = this.xVelocity - this.xVelocity / 16f;
            this.yVelocity = this.yVelocity - this.yVelocity / 16f;
            if ((double) Math.Abs(this.xVelocity) <= 0.0500000007450581)
              this.xVelocity = 0.0f;
            if ((double) Math.Abs(this.yVelocity) <= 0.0500000007450581)
              this.yVelocity = 0.0f;
          }
          else
          {
            this.xVelocity = this.xVelocity - this.xVelocity / 16f;
            this.yVelocity = this.yVelocity - this.yVelocity / 16f;
            if ((double) Math.Abs(this.xVelocity) <= 0.0500000007450581)
              this.xVelocity = 0.0f;
            if ((double) Math.Abs(this.yVelocity) <= 0.0500000007450581)
              this.yVelocity = 0.0f;
          }
        }
        if (this.CanMove || Game1.eventUp || this.controller != null)
        {
          if (!this.temporaryImpassableTile.Intersects(this.GetBoundingBox()))
            this.temporaryImpassableTile = Microsoft.Xna.Framework.Rectangle.Empty;
          float movementSpeed = this.getMovementSpeed();
          this.temporarySpeedBuff = 0.0f;
          if (this.movementDirections.Contains(0))
          {
            this.facingDirection = 0;
            Warp w = Game1.currentLocation.isCollidingWithWarp(this.nextPosition(0));
            if (w != null && this.IsMainPlayer)
            {
              this.warpFarmer(w);
              return;
            }
            if (this.isRidingHorse())
              currentLocation.isCollidingPosition(this.nextPosition(0), viewport, true, 0, false, (Character) this);
            if (!currentLocation.isCollidingPosition(this.nextPosition(0), viewport, true, 0, false, (Character) this))
            {
              this.position.Y -= movementSpeed;
              this.behaviorOnMovement(0);
            }
            else if (!this.isRidingHorse() && !currentLocation.isCollidingPosition(this.nextPositionHalf(0), viewport, true, 0, false, (Character) this))
            {
              this.position.Y -= movementSpeed / 2f;
              this.behaviorOnMovement(0);
            }
            else if (this.movementDirections.Count == 1)
            {
              Microsoft.Xna.Framework.Rectangle position = this.nextPosition(0);
              position.Width /= 4;
              bool flag1 = currentLocation.isCollidingPosition(position, viewport, true, 0, false, (Character) this);
              position.X += position.Width * 3;
              bool flag2 = currentLocation.isCollidingPosition(position, viewport, true, 0, false, (Character) this);
              if (flag1 && !flag2 && !currentLocation.isCollidingPosition(this.nextPosition(1), viewport, true, 0, false, (Character) this))
                this.position.X += (float) this.speed * ((float) time.ElapsedGameTime.Milliseconds / 64f);
              else if (flag2 && !flag1 && !currentLocation.isCollidingPosition(this.nextPosition(3), viewport, true, 0, false, (Character) this))
                this.position.X -= (float) this.speed * ((float) time.ElapsedGameTime.Milliseconds / 64f);
            }
            if (this.movementDirections.Count == 1)
            {
              if (this.ActiveObject == null || Game1.eventUp)
              {
                if (this.running)
                  ((FarmerSprite) this.sprite).animate(48, time);
                else
                  ((FarmerSprite) this.sprite).animate(16, time);
              }
              else if (this.running)
                ((FarmerSprite) this.sprite).animate(144, time);
              else
                ((FarmerSprite) this.sprite).animate(112, time);
            }
          }
          if (this.movementDirections.Contains(2))
          {
            this.facingDirection = 2;
            Warp w = Game1.currentLocation.isCollidingWithWarp(this.nextPosition(2));
            if (w != null && this.IsMainPlayer)
            {
              this.warpFarmer(w);
              return;
            }
            if (this.isRidingHorse())
              currentLocation.isCollidingPosition(this.nextPosition(2), viewport, true, 0, false, (Character) this);
            if (!currentLocation.isCollidingPosition(this.nextPosition(2), viewport, true, 0, false, (Character) this))
            {
              this.position.Y += movementSpeed;
              this.behaviorOnMovement(2);
            }
            else if (!this.isRidingHorse() && !currentLocation.isCollidingPosition(this.nextPositionHalf(2), viewport, true, 0, false, (Character) this))
            {
              this.position.Y += movementSpeed / 2f;
              this.behaviorOnMovement(0);
            }
            else if (this.movementDirections.Count == 1)
            {
              Microsoft.Xna.Framework.Rectangle position = this.nextPosition(2);
              position.Width /= 4;
              bool flag1 = currentLocation.isCollidingPosition(position, viewport, true, 0, false, (Character) this);
              position.X += position.Width * 3;
              bool flag2 = currentLocation.isCollidingPosition(position, viewport, true, 0, false, (Character) this);
              if (flag1 && !flag2 && !currentLocation.isCollidingPosition(this.nextPosition(1), viewport, true, 0, false, (Character) this))
                this.position.X += (float) this.speed * ((float) time.ElapsedGameTime.Milliseconds / 64f);
              else if (flag2 && !flag1 && !currentLocation.isCollidingPosition(this.nextPosition(3), viewport, true, 0, false, (Character) this))
                this.position.X -= (float) this.speed * ((float) time.ElapsedGameTime.Milliseconds / 64f);
            }
            if (this.movementDirections.Count == 1)
            {
              if (this.ActiveObject == null || Game1.eventUp)
              {
                if (this.running)
                  ((FarmerSprite) this.sprite).animate(32, time);
                else
                  ((FarmerSprite) this.sprite).animate(0, time);
              }
              else if (this.running)
                ((FarmerSprite) this.sprite).animate(128, time);
              else
                ((FarmerSprite) this.sprite).animate(96, time);
            }
          }
          if (this.movementDirections.Contains(1))
          {
            this.facingDirection = 1;
            Warp w = Game1.currentLocation.isCollidingWithWarp(this.nextPosition(1));
            if (w != null && this.IsMainPlayer)
            {
              this.warpFarmer(w);
              return;
            }
            if (!currentLocation.isCollidingPosition(this.nextPosition(1), viewport, true, 0, false, (Character) this))
            {
              this.position.X += movementSpeed;
              this.behaviorOnMovement(1);
            }
            else if (!this.isRidingHorse() && !currentLocation.isCollidingPosition(this.nextPositionHalf(1), viewport, true, 0, false, (Character) this))
            {
              this.position.X += movementSpeed / 2f;
              this.behaviorOnMovement(0);
            }
            else if (this.movementDirections.Count == 1)
            {
              Microsoft.Xna.Framework.Rectangle position = this.nextPosition(1);
              position.Height /= 4;
              bool flag1 = currentLocation.isCollidingPosition(position, viewport, true, 0, false, (Character) this);
              position.Y += position.Height * 3;
              bool flag2 = currentLocation.isCollidingPosition(position, viewport, true, 0, false, (Character) this);
              if (flag1 && !flag2 && !currentLocation.isCollidingPosition(this.nextPosition(2), viewport, true, 0, false, (Character) this))
                this.position.Y += (float) this.speed * ((float) time.ElapsedGameTime.Milliseconds / 64f);
              else if (flag2 && !flag1 && !currentLocation.isCollidingPosition(this.nextPosition(0), viewport, true, 0, false, (Character) this))
                this.position.Y -= (float) this.speed * ((float) time.ElapsedGameTime.Milliseconds / 64f);
            }
            if (this.movementDirections.Contains(1))
            {
              if (this.ActiveObject == null || Game1.eventUp)
              {
                if (this.running)
                  this.FarmerSprite.animate(40, time);
                else
                  ((FarmerSprite) this.sprite).animate(8, time);
              }
              else if (this.running)
                ((FarmerSprite) this.sprite).animate(136, time);
              else
                ((FarmerSprite) this.sprite).animate(104, time);
            }
          }
          if (this.movementDirections.Contains(3))
          {
            this.facingDirection = 3;
            Warp w = Game1.currentLocation.isCollidingWithWarp(this.nextPosition(3));
            if (w != null && this.IsMainPlayer)
            {
              this.warpFarmer(w);
              return;
            }
            if (!currentLocation.isCollidingPosition(this.nextPosition(3), viewport, true, 0, false, (Character) this))
            {
              this.position.X -= movementSpeed;
              this.behaviorOnMovement(3);
            }
            else if (!this.isRidingHorse() && !currentLocation.isCollidingPosition(this.nextPositionHalf(3), viewport, true, 0, false, (Character) this))
            {
              this.position.X -= movementSpeed / 2f;
              this.behaviorOnMovement(0);
            }
            else if (this.movementDirections.Count == 1)
            {
              Microsoft.Xna.Framework.Rectangle position = this.nextPosition(3);
              position.Height /= 4;
              bool flag1 = currentLocation.isCollidingPosition(position, viewport, true, 0, false, (Character) this);
              position.Y += position.Height * 3;
              bool flag2 = currentLocation.isCollidingPosition(position, viewport, true, 0, false, (Character) this);
              if (flag1 && !flag2 && !currentLocation.isCollidingPosition(this.nextPosition(2), viewport, true, 0, false, (Character) this))
                this.position.Y += (float) this.speed * ((float) time.ElapsedGameTime.Milliseconds / 64f);
              else if (flag2 && !flag1 && !currentLocation.isCollidingPosition(this.nextPosition(0), viewport, true, 0, false, (Character) this))
                this.position.Y -= (float) this.speed * ((float) time.ElapsedGameTime.Milliseconds / 64f);
            }
            if (this.movementDirections.Contains(3))
            {
              if (this.ActiveObject == null || Game1.eventUp)
              {
                if (this.running)
                  ((FarmerSprite) this.sprite).animate(56, time);
                else
                  ((FarmerSprite) this.sprite).animate(24, time);
              }
              else if (this.running)
                ((FarmerSprite) this.sprite).animate(152, time);
              else
                ((FarmerSprite) this.sprite).animate(120, time);
            }
          }
          else if (this.moveUp && this.running && this.ActiveObject == null)
            ((FarmerSprite) this.sprite).animate(48, time);
          else if (this.moveRight && this.running && this.ActiveObject == null)
            ((FarmerSprite) this.sprite).animate(40, time);
          else if (this.moveDown && this.running && this.ActiveObject == null)
            ((FarmerSprite) this.sprite).animate(32, time);
          else if (this.moveLeft && this.running && this.ActiveObject == null)
            ((FarmerSprite) this.sprite).animate(56, time);
          else if (this.moveUp && this.running)
            ((FarmerSprite) this.sprite).animate(144, time);
          else if (this.moveRight && this.running)
            ((FarmerSprite) this.sprite).animate(136, time);
          else if (this.moveDown && this.running)
            ((FarmerSprite) this.sprite).animate(128, time);
          else if (this.moveLeft && this.running)
            ((FarmerSprite) this.sprite).animate(152, time);
          else if (this.moveUp && this.ActiveObject == null)
            ((FarmerSprite) this.sprite).animate(16, time);
          else if (this.moveRight && this.ActiveObject == null)
            ((FarmerSprite) this.sprite).animate(8, time);
          else if (this.moveDown && this.ActiveObject == null)
            ((FarmerSprite) this.sprite).animate(0, time);
          else if (this.moveLeft && this.ActiveObject == null)
            ((FarmerSprite) this.sprite).animate(24, time);
          else if (this.moveUp)
            ((FarmerSprite) this.sprite).animate(112, time);
          else if (this.moveRight)
            ((FarmerSprite) this.sprite).animate(104, time);
          else if (this.moveDown)
            ((FarmerSprite) this.sprite).animate(96, time);
          else if (this.moveLeft)
            ((FarmerSprite) this.sprite).animate(120, time);
        }
        TimeSpan elapsedGameTime;
        if (this.isMoving() && !this.usingTool)
        {
          FarmerSprite farmerSprite = this.FarmerSprite;
          double num1 = 1.0;
          double num2 = this.running ? 0.0299999993294477 : 0.025000000372529;
          double num3 = 1.0;
          double num4 = ((double) this.speed + (Game1.eventUp ? 0.0 : (double) this.addedSpeed + (this.isRidingHorse() ? 4.59999990463257 : 0.0))) * (double) this.movementMultiplier;
          elapsedGameTime = Game1.currentGameTime.ElapsedGameTime;
          double milliseconds = (double) elapsedGameTime.Milliseconds;
          double num5 = num4 * milliseconds;
          double num6 = (double) Math.Max((float) num3, (float) num5) * 1.25;
          double num7 = num2 * num6;
          double num8 = num1 - num7;
          farmerSprite.intervalModifier = (float) num8;
        }
        else
          this.FarmerSprite.intervalModifier = 1f;
        if (this.moveUp)
          this.facingDirection = 0;
        else if (this.moveRight)
          this.facingDirection = 1;
        else if (this.moveDown)
          this.facingDirection = 2;
        else if (this.moveLeft)
          this.facingDirection = 3;
        if (this.temporarilyInvincible)
        {
          int invincibilityTimer = this.temporaryInvincibilityTimer;
          elapsedGameTime = time.ElapsedGameTime;
          int milliseconds = elapsedGameTime.Milliseconds;
          this.temporaryInvincibilityTimer = invincibilityTimer + milliseconds;
          if (this.temporaryInvincibilityTimer > 1200)
          {
            this.temporarilyInvincible = false;
            this.temporaryInvincibilityTimer = 0;
          }
        }
        if (currentLocation != null && currentLocation.isFarmerCollidingWithAnyCharacter())
          this.temporaryImpassableTile = new Microsoft.Xna.Framework.Rectangle((int) this.getTileLocation().X * Game1.tileSize, (int) this.getTileLocation().Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
        if (!this.isRidingHorse() || this.mount.dismounting)
          return;
        this.speed = 2;
        if (this.movementDirections.Count > 0 && (this.mount.facingDirection != this.movementDirections.First<int>() || this.mount.facingDirection != 1 && this.movementDirections.Contains(1) || this.mount.facingDirection != 3 && this.movementDirections.Contains(3)) && ((this.movementDirections.Count <= 1 || !this.movementDirections.Contains(1) || this.mount.facingDirection != 1) && (this.movementDirections.Count <= 1 || !this.movementDirections.Contains(3) || this.mount.facingDirection != 3)))
          this.mount.sprite.currentAnimation = (List<FarmerSprite.AnimationFrame>) null;
        if (this.movementDirections.Count > 0)
        {
          if (this.movementDirections.Contains(1))
            this.mount.faceDirection(1);
          else if (this.movementDirections.Contains(3))
            this.mount.faceDirection(3);
          else
            this.mount.faceDirection(this.movementDirections.First<int>());
        }
        if (this.isMoving() && this.mount.sprite.currentAnimation == null)
        {
          if (this.movementDirections.Contains(1))
          {
            this.faceDirection(1);
            this.mount.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
            {
              new FarmerSprite.AnimationFrame(8, 70),
              new FarmerSprite.AnimationFrame(9, 70, false, false, new AnimatedSprite.endOfAnimationBehavior(FarmerSprite.checkForFootstep), false),
              new FarmerSprite.AnimationFrame(10, 70, false, false, new AnimatedSprite.endOfAnimationBehavior(FarmerSprite.checkForFootstep), false),
              new FarmerSprite.AnimationFrame(11, 70, false, false, new AnimatedSprite.endOfAnimationBehavior(FarmerSprite.checkForFootstep), false),
              new FarmerSprite.AnimationFrame(12, 70),
              new FarmerSprite.AnimationFrame(13, 70)
            });
          }
          else if (this.movementDirections.Contains(3))
          {
            this.faceDirection(3);
            this.mount.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
            {
              new FarmerSprite.AnimationFrame(8, 70, false, true, (AnimatedSprite.endOfAnimationBehavior) null, false),
              new FarmerSprite.AnimationFrame(9, 70, false, true, new AnimatedSprite.endOfAnimationBehavior(FarmerSprite.checkForFootstep), false),
              new FarmerSprite.AnimationFrame(10, 70, false, true, new AnimatedSprite.endOfAnimationBehavior(FarmerSprite.checkForFootstep), false),
              new FarmerSprite.AnimationFrame(11, 70, false, true, new AnimatedSprite.endOfAnimationBehavior(FarmerSprite.checkForFootstep), false),
              new FarmerSprite.AnimationFrame(12, 70, false, true, (AnimatedSprite.endOfAnimationBehavior) null, false),
              new FarmerSprite.AnimationFrame(13, 70, false, true, (AnimatedSprite.endOfAnimationBehavior) null, false)
            });
          }
          else if (this.movementDirections.First<int>().Equals(0))
          {
            this.faceDirection(0);
            this.mount.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
            {
              new FarmerSprite.AnimationFrame(15, 70),
              new FarmerSprite.AnimationFrame(16, 70, false, false, new AnimatedSprite.endOfAnimationBehavior(FarmerSprite.checkForFootstep), false),
              new FarmerSprite.AnimationFrame(17, 70, false, false, new AnimatedSprite.endOfAnimationBehavior(FarmerSprite.checkForFootstep), false),
              new FarmerSprite.AnimationFrame(18, 70, false, false, new AnimatedSprite.endOfAnimationBehavior(FarmerSprite.checkForFootstep), false),
              new FarmerSprite.AnimationFrame(19, 70),
              new FarmerSprite.AnimationFrame(20, 70)
            });
          }
          else if (this.movementDirections.First<int>().Equals(2))
          {
            this.faceDirection(2);
            this.mount.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
            {
              new FarmerSprite.AnimationFrame(1, 70),
              new FarmerSprite.AnimationFrame(2, 70, false, false, new AnimatedSprite.endOfAnimationBehavior(FarmerSprite.checkForFootstep), false),
              new FarmerSprite.AnimationFrame(3, 70, false, false, new AnimatedSprite.endOfAnimationBehavior(FarmerSprite.checkForFootstep), false),
              new FarmerSprite.AnimationFrame(4, 70, false, false, new AnimatedSprite.endOfAnimationBehavior(FarmerSprite.checkForFootstep), false),
              new FarmerSprite.AnimationFrame(5, 70),
              new FarmerSprite.AnimationFrame(6, 70)
            });
          }
        }
        else if (!this.isMoving())
        {
          this.mount.Halt();
          this.mount.sprite.currentAnimation = (List<FarmerSprite.AnimationFrame>) null;
        }
        this.mount.position = this.position;
      }
    }

    public bool checkForQuestComplete(NPC n, int number1, int number2, Item item, string str, int questType = -1, int questTypeToIgnore = -1)
    {
      bool flag = false;
      for (int index = this.questLog.Count - 1; index >= 0; --index)
      {
        if (this.questLog[index] != null && (questType == -1 || this.questLog[index].questType == questType) && ((questTypeToIgnore == -1 || this.questLog[index].questType != questTypeToIgnore) && this.questLog[index].checkIfComplete(n, number1, number2, item, str)))
          flag = true;
      }
      return flag;
    }

    public static void completelyStopAnimating(Farmer who)
    {
      who.completelyStopAnimatingOrDoingAction();
    }

    public void completelyStopAnimatingOrDoingAction()
    {
      this.CanMove = !Game1.eventUp;
      this.usingTool = false;
      this.FarmerSprite.pauseForSingleAnimation = false;
      this.usingSlingshot = false;
      this.canReleaseTool = false;
      this.Halt();
      this.sprite.StopAnimation();
      if (this.CurrentTool != null && this.CurrentTool is MeleeWeapon)
        (this.CurrentTool as MeleeWeapon).isOnSpecial = false;
      this.stopJittering();
    }

    public void doEmote(int whichEmote)
    {
      if (this.isEmoting)
        return;
      this.isEmoting = true;
      this.currentEmote = whichEmote;
      this.currentEmoteFrame = 0;
      this.emoteInterval = 0.0f;
    }

    public void reloadLivestockSprites()
    {
      foreach (CoopDweller coopDweller in this.coopDwellers)
        coopDweller.reload();
      foreach (BarnDweller barnDweller in this.barnDwellers)
        barnDweller.reload();
    }

    public void performTenMinuteUpdate()
    {
      if (this.addedSpeed <= 0 || this.buffs.Count != 0 || (Game1.buffsDisplay.otherBuffs.Count != 0 || Game1.buffsDisplay.food != null) || Game1.buffsDisplay.drink != null)
        return;
      this.addedSpeed = 0;
    }

    public void setRunning(bool isRunning, bool force = false)
    {
      if (this.canOnlyWalk || this.bathingClothes && !this.running || Game1.CurrentEvent != null & isRunning && !Game1.CurrentEvent.isFestival && !Game1.CurrentEvent.playerControlSequence)
        return;
      if (this.isRidingHorse())
        this.running = true;
      else if ((double) this.stamina <= 0.0)
      {
        this.speed = 2;
        if (this.running)
          this.Halt();
        this.running = false;
      }
      else if (force || this.CanMove && !Game1.isEating && (Game1.currentLocation.currentEvent == null || Game1.currentLocation.currentEvent.playerControlSequence) && ((isRunning || !this.usingTool) && (isRunning || !Game1.pickingTool)) && (this.sprite == null || !((FarmerSprite) this.sprite).pauseForSingleAnimation))
      {
        this.running = isRunning;
        if (this.running)
          this.speed = 5;
        else
          this.speed = 2;
      }
      else
      {
        if (!this.usingTool)
          return;
        this.running = isRunning;
        if (this.running)
          this.speed = 5;
        else
          this.speed = 2;
      }
    }

    public void addSeenResponse(int id)
    {
      this.dialogueQuestionsAnswered.Add(id);
    }

    public void grabObject(Object obj)
    {
      if (obj == null)
        return;
      this.CanMove = false;
      this.activeObject = (Item) obj;
      switch (this.facingDirection)
      {
        case 0:
          ((FarmerSprite) this.sprite).animateOnce(80, 50f, 8);
          break;
        case 1:
          ((FarmerSprite) this.sprite).animateOnce(72, 50f, 8);
          break;
        case 2:
          ((FarmerSprite) this.sprite).animateOnce(64, 50f, 8);
          break;
        case 3:
          ((FarmerSprite) this.sprite).animateOnce(88, 50f, 8);
          break;
      }
      Game1.playSound("pickUpItem");
    }

    public string getTitle()
    {
      int level = this.Level;
      if (level >= 30)
        return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.2016");
      if (level > 28)
        return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.2017");
      if (level > 26)
        return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.2018");
      if (level > 24)
        return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.2019");
      if (level > 22)
        return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.2020");
      if (level > 20)
        return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.2021");
      if (level > 18)
        return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.2022");
      if (level > 16)
      {
        if (!this.isMale)
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.2024");
        return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.2023");
      }
      if (level > 14)
        return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.2025");
      if (level > 12)
        return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.2026");
      if (level > 10)
        return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.2027");
      if (level > 8)
        return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.2028");
      if (level > 6)
        return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.2029");
      if (level > 4)
        return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.2030");
      if (level > 2)
        return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.2031");
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:Farmer.cs.2032");
    }
  }
}
