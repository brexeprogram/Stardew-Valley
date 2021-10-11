// Decompiled with JetBrains decompiler
// Type: StardewValley.Game1
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Buildings;
using StardewValley.Characters;
using StardewValley.Events;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Minigames;
using StardewValley.Monsters;
using StardewValley.Network;
using StardewValley.Objects;
using StardewValley.Projectiles;
using StardewValley.Quests;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using xTile;
using xTile.Dimensions;
using xTile.Display;
using xTile.Layers;
using xTile.ObjectModel;
using xTile.Tiles;

namespace StardewValley
{
  public class Game1 : Game
  {
    public static int pixelZoom = 4;
    public static int tileSize = 64;
    public static float thumbStickSensitivity = 0.1f;
    public static float runThreshold = 0.5f;
    private static StringBuilder _debugStringBuilder = new StringBuilder();
    public static List<GameLocation> locations = new List<GameLocation>();
    private static Texture2D _toolSpriteSheet = (Texture2D) null;
    public static float screenGlowAlpha = 0.0f;
    public static float flashAlpha = 0.0f;
    public static bool fadeToBlack = false;
    public static bool fadeIn = true;
    public static bool dialogueUp = false;
    public static bool dialogueTyping = false;
    public static bool pickingTool = false;
    public static bool isQuestion = false;
    public static bool nonWarpFade = false;
    public static bool particleRaining = false;
    public static bool newDay = false;
    public static bool inMine = false;
    public static bool isEating = false;
    public static bool menuUp = false;
    public static bool eventUp = false;
    public static bool viewportFreeze = false;
    public static bool eventOver = false;
    public static bool nameSelectUp = false;
    public static bool screenGlow = false;
    public static bool screenGlowHold = false;
    public static bool progressBar = false;
    public static bool isRaining = false;
    public static bool isSnowing = false;
    public static bool killScreen = false;
    public static bool displayHUD = true;
    public static bool displayFarmer = true;
    public static bool spawnMonstersAtNight = false;
    public static bool showingHealth = false;
    public static string currentSeason = "spring";
    public static string nextMusicTrack = "";
    public static string messageAfterPause = "";
    public static string fertilizer = "";
    public static string samBandName = "The Alfalfas";
    public static string elliottBookName = "Blue Tower";
    public static string keyHelpString = "";
    public static string lastDebugInput = "";
    public static string loadingMessage = "";
    public static string errorMessage = "";
    public static Queue<string> currentObjectDialogue = new Queue<string>();
    public static Queue<string> mailbox = new Queue<string>();
    public static List<Response> questionChoices = new List<Response>();
    public static int dayOfMonth = 0;
    public static int year = 1;
    public static int timeOfDay = 600;
    public static int numberOfSelectedItems = -1;
    public static int farmerWallpaper = 22;
    public static int wallpaperPrice = 75;
    public static int currentFloor = 3;
    public static int FarmerFloor = 29;
    public static int floorPrice = 75;
    public static int tvStation = -1;
    public static int percentageToWinStardewHero = 70;
    public static int currentSongIndex = 3;
    public static Color morningColor = Color.LightBlue;
    public static Color eveningColor = new Color((int) byte.MaxValue, (int) byte.MaxValue, 0);
    public static Color unselectedOptionColor = new Color(100, 100, 100);
    public static Random random = new Random(DateTime.Now.Millisecond);
    public static Random recentMultiplayerRandom = new Random();
    public static List<Object> shippingBin = new List<Object>();
    public static List<HUDMessage> hudMessages = new List<HUDMessage>();
    public static float dialogueButtonScale = 1f;
    public static PlayerIndex playerOneIndex = PlayerIndex.One;
    public static Vector2 shiny = Vector2.Zero;
    public static Vector2 lastCursorTile = Vector2.Zero;
    public static RainDrop[] rainDrops = new RainDrop[70];
    public static double chanceToRainTomorrow = 0.0;
    public static double dailyLuck = 0.001;
    public static List<WeatherDebris> debrisWeather = new List<WeatherDebris>();
    public static List<TemporaryAnimatedSprite> screenOverlayTempSprites = new List<TemporaryAnimatedSprite>();
    public static ulong uniqueIDForThisGame = (ulong) (DateTime.UtcNow - new DateTime(2012, 6, 22)).TotalSeconds;
    public static Stats stats = new Stats();
    public static int[] directionKeyPolling = new int[4];
    public static HashSet<LightSource> currentLightSources = new HashSet<LightSource>();
    public static Color outdoorLight = new Color((int) byte.MaxValue, (int) byte.MaxValue, 0);
    public static Color textColor = new Color(34, 17, 34);
    public static Color textShadowColor = new Color(206, 156, 95);
    private static IMinigame _currentMinigame = (IMinigame) null;
    public static List<IClickableMenu> onScreenMenus = new List<IClickableMenu>();
    private static List<float> _fpsList = new List<float>(120);
    private static Stopwatch _fpsStopwatch = new Stopwatch();
    private static float _fps = 0.0f;
    public static List<DelayedAction> delayedActions = new List<DelayedAction>();
    public static Stack<IClickableMenu> endOfNightMenus = new Stack<IClickableMenu>();
    public static Vector2 viewportTarget = new Vector2((float) int.MinValue, (float) int.MinValue);
    public static float viewportSpeed = 2f;
    private static bool _cursorDragEnabled = false;
    private static bool _cursorDragPrevEnabled = false;
    private static bool _cursorSpeedDirty = true;
    private static float _cursorSpeed = 16f;
    private static float _cursorSpeedScale = 1f;
    private static float _cursorUpdateElapsedSec = 0.0f;
    public static float screenGlowRate = 0.005f;
    public static bool haltAfterCheck = false;
    public static bool conventionMode = false;
    public static float thumbstickMotionAccell = 1f;
    public static int mouseCursor = 0;
    private static float _mouseCursorTransparency = 1f;
    public static bool wasMouseVisibleThisFrame = true;
    private readonly Color bgColor = new Color(5, 3, 4);
    private readonly BlendState lightingBlend = new BlendState()
    {
      ColorBlendFunction = BlendFunction.ReverseSubtract,
      ColorDestinationBlend = Blend.One,
      ColorSourceBlend = Blend.SourceColor
    };
    public const int defaultResolutionX = 1280;
    public const int defaultResolutionY = 720;
    public const int smallestTileSize = 16;
    public const int up = 0;
    public const int right = 1;
    public const int down = 2;
    public const int left = 3;
    public const int spriteIndexForOveralls = 3854;
    public const int colorToleranceForOveralls = 60;
    public const int spriteIndexForOverallsBorder = 3846;
    public const int colorToloranceForOverallsBorder = 20;
    public const int dialogueBoxTileHeight = 5;
    public const int realMilliSecondsPerGameTenMinutes = 7000;
    public const int rainDensity = 70;
    public const int millisecondsPerDialogueLetterType = 30;
    public const float pickToolDelay = 500f;
    public const int defaultMinFishingBiteTime = 600;
    public const int defaultMaxFishingBiteTime = 30000;
    public const int defaultMinFishingNibbleTime = 340;
    public const int defaultMaxFishingNibbleTime = 800;
    public const int minWallpaperPrice = 75;
    public const int maxWallpaperPrice = 500;
    public const int rainLoopLength = 70;
    public const int weather_sunny = 0;
    public const int weather_rain = 1;
    public const int weather_debris = 2;
    public const int weather_lightning = 3;
    public const int weather_festival = 4;
    public const int weather_snow = 5;
    public const int weather_wedding = 6;
    public const byte singlePlayer = 0;
    public const byte multiplayerClient = 1;
    public const byte multiplayerServer = 2;
    public const byte logoScreenGameMode = 4;
    public const byte titleScreenGameMode = 0;
    public const byte loadScreenGameMode = 1;
    public const byte newGameMode = 2;
    public const byte playingGameMode = 3;
    public const byte characterSelectMode = 5;
    public const byte loadingMode = 6;
    public const byte saveMode = 7;
    public const byte saveCompleteMode = 8;
    public const byte selectGameScreen = 9;
    public const byte creditsMode = 10;
    public const byte errorLogMode = 11;
    public const string version = "1.2.33";
    public const float keyPollingThreshold = 650f;
    public const float toolHoldPerPowerupLevel = 600f;
    public const float startingMusicVolume = 1f;
    public LocalizedContentManager xTileContent;
    private static LocalizedContentManager _temporaryContent;
    public static GraphicsDeviceManager graphics;
    public static LocalizedContentManager content;
    public static SpriteBatch spriteBatch;
    public static GamePadState oldPadState;
    public static KeyboardState oldKBState;
    public static MouseState oldMouseState;
    private static Farmer _player;
    private static Farmer _serverHost;
    public static GameLocation currentLocation;
    public static GameLocation locationAfterWarp;
    public static IDisplayDevice mapDisplayDevice;
    public static xTile.Dimensions.Rectangle viewport;
    public static Texture2D objectSpriteSheet;
    public static Texture2D cropSpriteSheet;
    public static Texture2D mailboxTexture;
    public static Texture2D emoteSpriteSheet;
    public static Texture2D debrisSpriteSheet;
    public static Texture2D toolIconBox;
    public static Texture2D rainTexture;
    public static Texture2D bigCraftableSpriteSheet;
    public static Texture2D swordSwipe;
    public static Texture2D swordSwipeDark;
    public static Texture2D buffsIcons;
    public static Texture2D daybg;
    public static Texture2D nightbg;
    public static Texture2D logoScreenTexture;
    public static Texture2D tvStationTexture;
    public static Texture2D cloud;
    public static Texture2D menuTexture;
    public static Texture2D lantern;
    public static Texture2D windowLight;
    public static Texture2D sconceLight;
    public static Texture2D cauldronLight;
    public static Texture2D shadowTexture;
    public static Texture2D mouseCursors;
    public static Texture2D controllerMaps;
    public static Texture2D indoorWindowLight;
    public static Texture2D animations;
    public static Texture2D titleScreenBG;
    public static Texture2D logo;
    private static RenderTarget2D _lightmap;
    public static Texture2D fadeToBlackRect;
    public static Texture2D staminaRect;
    public static Texture2D currentCoopTexture;
    public static Texture2D currentBarnTexture;
    public static Texture2D currentHouseTexture;
    public static Texture2D greenhouseTexture;
    public static Texture2D littleEffect;
    public static SpriteFont dialogueFont;
    public static SpriteFont smallFont;
    public static SpriteFont tinyFont;
    public static SpriteFont tinyFontBorder;
    public static float fadeToBlackAlpha;
    public static float pickToolInterval;
    public static float starCropShimmerPause;
    public static float noteBlockTimer;
    public static float globalFadeSpeed;
    public static bool screenGlowUp;
    public static bool coopDwellerBorn;
    public static bool messagePause;
    public static bool isDebrisWeather;
    public static bool boardingBus;
    public static bool listeningForKeyControlDefinitions;
    public static bool weddingToday;
    public static bool exitToTitle;
    public static bool debugMode;
    public static bool isLightning;
    public static bool showKeyHelp;
    public static bool shippingTax;
    public static bool dialogueButtonShrinking;
    public static bool jukeboxPlaying;
    public static bool drawLighting;
    public static bool bloomDay;
    public static bool quit;
    public static bool isChatting;
    public static bool globalFade;
    public static bool drawGrid;
    public static bool freezeControls;
    public static bool saveOnNewDay;
    public static bool panMode;
    public static bool showingEndOfNightStuff;
    public static bool wasRainingYesterday;
    public static bool hasLoadedGame;
    public static bool isActionAtCurrentCursorTile;
    public static bool isInspectionAtCurrentCursorTile;
    public static bool paused;
    public static bool lastCursorMotionWasMouse;
    public static string debugOutput;
    public static string selectedItemsType;
    public static string nameSelectType;
    public static string slotResult;
    public static int xLocationAfterWarp;
    public static int yLocationAfterWarp;
    public static int gameTimeInterval;
    public static int currentQuestionChoice;
    public static int currentDialogueCharacterIndex;
    public static int dialogueTypingInterval;
    public static int priceOfSelectedItem;
    public static int currentWallpaper;
    public static int dialogueWidth;
    public static int countdownToWedding;
    public static int menuChoice;
    public static int currentBillboard;
    public static int facingDirectionAfterWarp;
    public static int tmpTimeOfDay;
    public static int mouseClickPolling;
    public static int gamePadXButtonPolling;
    public static int gamePadAButtonPolling;
    public static int weatherIcon;
    public static int hitShakeTimer;
    public static int staminaShakeTimer;
    public static int pauseThenDoFunctionTimer;
    public static int weatherForTomorrow;
    public static int cursorTileHintCheckTimer;
    public static int timerUntilMouseFade;
    public static int minecartHighScore;
    public static int whichFarm;
    public static List<int> dealerCalicoJackTotal;
    public static Color screenGlowColor;
    public static NPC currentSpeaker;
    public static Dictionary<int, string> objectInformation;
    public static Dictionary<int, string> bigCraftablesInformation;
    public static MineShaft mine;
    public static Dictionary<string, string> NPCGiftTastes;
    public static float musicPlayerVolume;
    public static float ambientPlayerVolume;
    public static float pauseAccumulator;
    public static float pauseTime;
    public static float upPolling;
    public static float downPolling;
    public static float rightPolling;
    public static float leftPolling;
    public static float debrisSoundInterval;
    public static float toolHold;
    public static float windGust;
    public static float creditsTimer;
    public static float globalOutdoorLighting;
    public static Cue currentSong;
    public static AudioCategory musicCategory;
    public static AudioCategory soundCategory;
    public static AudioCategory ambientCategory;
    public static AudioCategory footstepCategory;
    public static AudioEngine audioEngine;
    public static WaveBank waveBank;
    public static SoundBank soundBank;
    public static Vector2 previousViewportPosition;
    public static Vector2 currentCursorTile;
    public static Cue fuseSound;
    public static Cue chargeUpSound;
    public static Cue wind;
    private static byte _gameMode;
    private bool _isSaving;
    public static byte multiplayerMode;
    public static IEnumerator<int> currentLoader;
    public static int[] cropsOfTheWeek;
    public static Quest questOfTheDay;
    public static MoneyMadeScreen moneyMadeScreen;
    public static Color ambientLight;
    public static IClickableMenu overlayMenu;
    private static IClickableMenu _activeClickableMenu;
    private const int _fpsHistory = 120;
    public static BloomComponent bloom;
    public static Dictionary<int, string> achievements;
    public static Object dishOfTheDay;
    public static BuffsDisplay buffsDisplay;
    public static DayTimeMoneyBox dayTimeMoneyBox;
    public static Dictionary<long, Farmer> otherFarmers;
    public static Server server;
    public static Client client;
    public static KeyboardDispatcher keyboardDispatcher;
    public static Background background;
    public static FarmEvent farmEvent;
    public static Game1.afterFadeFunction afterFade;
    public static Game1.afterFadeFunction afterDialogues;
    public static Game1.afterFadeFunction afterViewport;
    public static Game1.afterFadeFunction viewportReachedTarget;
    public static Game1.afterFadeFunction afterPause;
    public static GameTime currentGameTime;
    public static Options options;
    public static Game1 game1;
    public static Point lastMousePositionBeforeFade;
    private static string debugPresenceString;
    public static bool overrideGameMenuReset;
    public static Point viewportCenter;
    public static int viewportHold;
    private const float CursorBaseSpeed = 16f;
    private static int thumbstickPollingTimer;
    public static bool toggleFullScreen;
    public static string whereIsTodaysFest;
    public static bool farmerShouldPassOut;
    public const string NO_LETTER_MAIL = "%&NL&%";
    private static Task _newDayTask;
    private static Action _afterNewDayAction;
    public static Vector2 currentViewportTarget;
    public static Vector2 viewportPositionLerp;
    public static float screenGlowMax;
    private string panModeString;
    private bool panFacingDirectionWait;
    public static int thumbstickMotionMargin;
    public static int triggerPolling;
    public static int rightClickPolling;
    private RenderTarget2D _screen;
    public static NPC objectDialoguePortraitPerson;

    public void CleanupReturningToTitle()
    {
      Console.WriteLine("CleanupReturningToTitle()");
      SaveGame.CancelToTitle = false;
      Game1.overlayMenu = (IClickableMenu) null;
      Game1._afterNewDayAction = (Action) null;
      Game1._currentMinigame = (IMinigame) null;
      Game1.gameMode = (byte) 0;
      this._isSaving = false;
      Game1._mouseCursorTransparency = 1f;
      Game1._newDayTask = (Task) null;
      Game1.resetPlayer();
      Game1.serverHost = (Farmer) null;
      Game1.afterDialogues = (Game1.afterFadeFunction) null;
      Game1.afterFade = (Game1.afterFadeFunction) null;
      Game1.afterPause = (Game1.afterFadeFunction) null;
      Game1.afterViewport = (Game1.afterFadeFunction) null;
      Game1.ambientLight = new Color(0, 0, 0, 0);
      Game1.background = (Background) null;
      Game1.bloom = (BloomComponent) null;
      Game1.bloomDay = false;
      Game1.boardingBus = false;
      Game1.chanceToRainTomorrow = 0.0;
      Game1.client = (Client) null;
      Game1.cloud = (Texture2D) null;
      Game1.conventionMode = false;
      Game1.coopDwellerBorn = false;
      Game1.countdownToWedding = 0;
      Game1.creditsTimer = 0.0f;
      Game1.cropsOfTheWeek = (int[]) null;
      Game1.currentBarnTexture = (Texture2D) null;
      Game1.currentBillboard = 0;
      Game1.currentCoopTexture = (Texture2D) null;
      Game1.currentCursorTile = Vector2.Zero;
      Game1.currentDialogueCharacterIndex = 0;
      Game1.currentFloor = 3;
      Game1.currentHouseTexture = (Texture2D) null;
      Game1.currentLightSources.Clear();
      Game1.currentLoader = (IEnumerator<int>) null;
      Game1.currentLocation = (GameLocation) null;
      Game1.currentObjectDialogue.Clear();
      Game1.currentQuestionChoice = 0;
      Game1.currentSeason = "spring";
      Game1.currentSongIndex = 3;
      Game1.currentSpeaker = (NPC) null;
      Game1.currentViewportTarget = Vector2.Zero;
      Game1.currentWallpaper = 0;
      Game1.cursorTileHintCheckTimer = 0;
      Game1.dailyLuck = 0.001;
      Game1.dayOfMonth = 0;
      Game1.dealerCalicoJackTotal = (List<int>) null;
      Game1.debrisSoundInterval = 0.0f;
      Game1.debrisWeather.Clear();
      Game1.debugMode = false;
      Game1.debugOutput = (string) null;
      Game1.debugPresenceString = "In menus";
      Game1.delayedActions.Clear();
      Game1.dialogueButtonScale = 1f;
      Game1.dialogueButtonShrinking = false;
      Game1.dialogueTyping = false;
      Game1.dialogueTypingInterval = 0;
      Game1.dialogueUp = false;
      Game1.dialogueWidth = 1024;
      Game1.dishOfTheDay = (Object) null;
      Game1.displayFarmer = true;
      Game1.displayHUD = true;
      Game1.downPolling = 0.0f;
      Game1.drawGrid = false;
      Game1.drawLighting = false;
      Game1.elliottBookName = "Blue Tower";
      Game1.endOfNightMenus.Clear();
      Game1.errorMessage = "";
      Game1.eveningColor = new Color((int) byte.MaxValue, (int) byte.MaxValue, 0, (int) byte.MaxValue);
      Game1.eventOver = false;
      Game1.eventUp = false;
      Game1.exitToTitle = false;
      Game1.facingDirectionAfterWarp = 0;
      Game1.fadeIn = true;
      Game1.fadeToBlack = false;
      Game1.fadeToBlackAlpha = 1.02f;
      Game1.FarmerFloor = 29;
      Game1.farmerShouldPassOut = false;
      Game1.farmerWallpaper = 22;
      Game1.farmEvent = (FarmEvent) null;
      Game1.fertilizer = "";
      Game1.flashAlpha = 0.0f;
      Game1.floorPrice = 75;
      Game1.freezeControls = false;
      Game1.gamePadAButtonPolling = 0;
      Game1.gameTimeInterval = 0;
      Game1.globalFade = false;
      Game1.globalFadeSpeed = 0.0f;
      Game1.globalOutdoorLighting = 0.0f;
      Game1.greenhouseTexture = (Texture2D) null;
      Game1.haltAfterCheck = false;
      Game1.hasLoadedGame = false;
      Game1.hitShakeTimer = 0;
      Game1.hudMessages.Clear();
      Game1.inMine = false;
      Game1.isActionAtCurrentCursorTile = false;
      Game1.isChatting = false;
      Game1.isDebrisWeather = false;
      Game1.isEating = false;
      Game1.isInspectionAtCurrentCursorTile = false;
      Game1.isLightning = false;
      Game1.isQuestion = false;
      Game1.isRaining = false;
      Game1.isSnowing = false;
      Game1.jukeboxPlaying = false;
      Game1.keyHelpString = "";
      Game1.killScreen = false;
      Game1.lastCursorMotionWasMouse = true;
      Game1.lastCursorTile = Vector2.Zero;
      Game1.lastDebugInput = "";
      Game1.lastMousePositionBeforeFade = Point.Zero;
      Game1.leftPolling = 0.0f;
      Game1.listeningForKeyControlDefinitions = false;
      Game1.loadingMessage = "";
      Game1.locationAfterWarp = (GameLocation) null;
      Game1.locations.Clear();
      Game1.logo = (Texture2D) null;
      Game1.logoScreenTexture = (Texture2D) null;
      Game1.mailbox.Clear();
      Game1.mailboxTexture = (Texture2D) null;
      Game1.mapDisplayDevice = (IDisplayDevice) new XnaDisplayDevice(this.Content, this.GraphicsDevice);
      Game1.menuChoice = 0;
      Game1.menuUp = false;
      Game1.messageAfterPause = "";
      Game1.messagePause = false;
      Game1.mine = (MineShaft) null;
      Game1.minecartHighScore = 0;
      Game1.moneyMadeScreen = (MoneyMadeScreen) null;
      Game1.mouseClickPolling = 0;
      Game1.mouseCursor = 0;
      Game1.multiplayerMode = (byte) 0;
      Game1.nameSelectType = (string) null;
      Game1.nameSelectUp = false;
      Game1.newDay = false;
      Game1.nonWarpFade = false;
      Game1.noteBlockTimer = 0.0f;
      Game1.numberOfSelectedItems = -1;
      Game1.objectDialoguePortraitPerson = (NPC) null;
      Game1.onScreenMenus.Clear();
      Game1.onScreenMenus.Add((IClickableMenu) new Toolbar());
      Game1.dayTimeMoneyBox = new DayTimeMoneyBox();
      Game1.onScreenMenus.Add((IClickableMenu) Game1.dayTimeMoneyBox);
      Game1.buffsDisplay = new BuffsDisplay();
      Game1.onScreenMenus.Add((IClickableMenu) Game1.buffsDisplay);
      Game1.options = new Options();
      for (int index = 0; index < Game1.otherFarmers.Count; ++index)
        Game1.otherFarmers[(long) index].unload();
      Game1.otherFarmers.Clear();
      Game1.outdoorLight = new Color((int) byte.MaxValue, (int) byte.MaxValue, 0, (int) byte.MaxValue);
      Game1.overlayMenu = (IClickableMenu) null;
      Game1.overrideGameMenuReset = false;
      this.panFacingDirectionWait = false;
      Game1.panMode = false;
      this.panModeString = (string) null;
      Game1.particleRaining = false;
      Game1.pauseAccumulator = 0.0f;
      Game1.paused = false;
      Game1.pauseThenDoFunctionTimer = 0;
      Game1.pauseTime = 0.0f;
      Game1.percentageToWinStardewHero = 70;
      Game1.pickingTool = false;
      Game1.pickToolInterval = 0.0f;
      Game1.pixelZoom = 4;
      Game1.previousViewportPosition = Vector2.Zero;
      Game1.priceOfSelectedItem = 0;
      Game1.progressBar = false;
      Game1.questionChoices.Clear();
      Game1.questOfTheDay = (Quest) null;
      Game1.quit = false;
      Game1.rightClickPolling = 0;
      Game1.rightPolling = 0.0f;
      Game1.runThreshold = 0.5f;
      Game1.samBandName = "The Alfalfas";
      Game1.saveOnNewDay = true;
      Game1.screenGlow = false;
      Game1.screenGlowAlpha = 0.0f;
      Game1.screenGlowColor = new Color(0, 0, 0, 0);
      Game1.screenGlowHold = false;
      Game1.screenGlowMax = 0.0f;
      Game1.screenGlowRate = 0.005f;
      Game1.screenGlowUp = false;
      Game1.screenOverlayTempSprites.Clear();
      Game1.selectedItemsType = (string) null;
      Game1.server = (Server) null;
      Game1.shiny = Vector2.Zero;
      Game1.shippingBin.Clear();
      Game1.shippingTax = false;
      Game1.showingEndOfNightStuff = false;
      Game1.showKeyHelp = false;
      Game1.slotResult = (string) null;
      Game1.spawnMonstersAtNight = false;
      Game1.staminaShakeTimer = 0;
      Game1.starCropShimmerPause = 0.0f;
      Game1.stats = new Stats();
      Game1.swordSwipe = (Texture2D) null;
      Game1.swordSwipeDark = (Texture2D) null;
      Game1.textColor = new Color(34, 17, 34, (int) byte.MaxValue);
      Game1.textShadowColor = new Color(206, 156, 95, (int) byte.MaxValue);
      Game1.thumbstickMotionAccell = 1f;
      Game1.thumbstickMotionMargin = 0;
      Game1.thumbstickPollingTimer = 0;
      Game1.thumbStickSensitivity = 0.1f;
      Game1.tileSize = 64;
      Game1.timeOfDay = 600;
      Game1.timerUntilMouseFade = 0;
      Game1.titleScreenBG = (Texture2D) null;
      Game1.tmpTimeOfDay = 0;
      Game1.toggleFullScreen = false;
      Game1.toolHold = 0.0f;
      Game1.toolIconBox = (Texture2D) null;
      Game1.ResetToolSpriteSheet();
      Game1.triggerPolling = 0;
      Game1.tvStation = -1;
      Game1.tvStationTexture = (Texture2D) null;
      Game1.uniqueIDForThisGame = (ulong) (DateTime.UtcNow - new DateTime(2012, 6, 22)).TotalSeconds;
      Game1.upPolling = 0.0f;
      Game1.viewportFreeze = false;
      Game1.viewportHold = 0;
      Game1.viewportPositionLerp = Vector2.Zero;
      Game1.viewportReachedTarget = (Game1.afterFadeFunction) null;
      Game1.viewportSpeed = 2f;
      Game1.viewportTarget = new Vector2((float) int.MinValue, (float) int.MinValue);
      Game1.wallpaperPrice = 75;
      Game1.wasMouseVisibleThisFrame = true;
      Game1.wasRainingYesterday = false;
      Game1.weatherForTomorrow = 0;
      Game1.weatherIcon = 0;
      Game1.weddingToday = false;
      Game1.whereIsTodaysFest = (string) null;
      Game1.whichFarm = 0;
      Game1.windGust = 0.0f;
      Game1.xLocationAfterWarp = 0;
      Game1.game1.xTileContent.Dispose();
      Game1.game1.xTileContent = this.CreateContentManager(Game1.content.ServiceProvider, Game1.content.RootDirectory);
      Game1.year = 1;
      Game1.yLocationAfterWarp = 0;
      JojaMart.Morris = (NPC) null;
      AmbientLocationSounds.onLocationLeave();
      WeatherDebris.globalWind = -0.25f;
      Utility.killAllStaticLoopingSoundCues();
      TitleMenu.subMenu = (IClickableMenu) null;
      OptionsDropDown.selected = (OptionsDropDown) null;
      JunimoNoteMenu.tempSprites.Clear();
      JunimoNoteMenu.screenSwipe = (ScreenSwipe) null;
      JunimoNoteMenu.canClick = true;
      GameMenu.forcePreventClose = false;
      Club.timesPlayedCalicoJack = 0;
      if (GamePad.GetState(PlayerIndex.One).IsConnected)
        Game1.options.gamepadControls = true;
      Game1.game1.refreshWindowSettings();
      if (Game1.activeClickableMenu == null || !(Game1.activeClickableMenu is TitleMenu))
        return;
      (Game1.activeClickableMenu as TitleMenu).applyPreferences();
      Game1.activeClickableMenu.gameWindowSizeChanged(Game1.graphics.GraphicsDevice.Viewport.Bounds, Game1.graphics.GraphicsDevice.Viewport.Bounds);
    }

    public static void GetNumFarmsSavedAsync(ReportNumFarms callback)
    {
      new Task((Action) (() =>
      {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        callback(Game1.GetNumFarmsSaved());
      })).Start();
    }

    public static int GetNumFarmsSaved()
    {
      int num = 0;
      string str = Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StardewValley"), "Saves"));
      if (Directory.Exists(str))
      {
        foreach (string directory in Directory.GetDirectories(str))
        {
          string path = Path.Combine(str, directory, "SaveGameInfo");
          try
          {
            using (File.Open(path, FileMode.Open))
              ++num;
          }
          catch (Exception ex)
          {
          }
        }
      }
      return num;
    }

    public static int GetMaxNumFarmsSaved()
    {
      return int.MaxValue;
    }

    private static string GameModeToString(byte mode)
    {
      switch (mode)
      {
        case 0:
          return string.Format("titleScreenGameMode ({0})", (object) mode);
        case 1:
          return string.Format("loadScreenGameMode ({0})", (object) mode);
        case 2:
          return string.Format("newGameMode ({0})", (object) mode);
        case 3:
          return string.Format("playingGameMode ({0})", (object) mode);
        case 4:
          return string.Format("logoScreenGameMode ({0})", (object) mode);
        case 5:
          return string.Format("characterSelectMode ({0})", (object) mode);
        case 6:
          return string.Format("loadingMode ({0})", (object) mode);
        case 7:
          return string.Format("saveMode ({0})", (object) mode);
        case 8:
          return string.Format("saveCompleteMode ({0})", (object) mode);
        case 9:
          return string.Format("selectGameScreen ({0})", (object) mode);
        case 10:
          return string.Format("creditsMode ({0})", (object) mode);
        case 11:
          return string.Format("errorLogMode ({0})", (object) mode);
        default:
          return string.Format("unknown ({0})", (object) mode);
      }
    }

    public static LocalizedContentManager temporaryContent
    {
      get
      {
        if (Game1._temporaryContent == null)
          Game1._temporaryContent = Game1.content.CreateTemporary();
        return Game1._temporaryContent;
      }
    }

    public static Farmer player
    {
      get
      {
        return Game1._player;
      }
      set
      {
        if (Game1._player != null)
        {
          Game1._player.unload();
          Game1._player = (Farmer) null;
        }
        Game1._player = value;
      }
    }

    public static Farmer serverHost
    {
      get
      {
        return Game1._serverHost;
      }
      set
      {
        if (Game1._serverHost != null)
        {
          if (Game1._serverHost != Game1._player)
            Game1._serverHost.unload();
          Game1._serverHost = (Farmer) null;
        }
        Game1._serverHost = value;
      }
    }

    public static Texture2D toolSpriteSheet
    {
      get
      {
        if (Game1._toolSpriteSheet == null)
          Game1.ResetToolSpriteSheet();
        return Game1._toolSpriteSheet;
      }
    }

    public static void ResetToolSpriteSheet()
    {
      if (Game1._toolSpriteSheet != null)
      {
        Game1._toolSpriteSheet.Dispose();
        Game1._toolSpriteSheet = (Texture2D) null;
      }
      Texture2D texture2D1 = Game1.game1.Content.Load<Texture2D>("TileSheets\\tools");
      int width = texture2D1.Width;
      int height = texture2D1.Height;
      int levelCount = texture2D1.LevelCount;
      Texture2D texture2D2 = new Texture2D(Game1.game1.GraphicsDevice, width, height, false, SurfaceFormat.Color);
      Color[] data1 = new Color[width * height];
      texture2D1.GetData<Color>(data1);
      Color[] data2 = data1;
      texture2D2.SetData<Color>(data2);
      Game1._toolSpriteSheet = texture2D2;
    }

    public static RenderTarget2D lightmap
    {
      get
      {
        return Game1._lightmap;
      }
    }

    private static void allocateLightmap()
    {
      int num1 = 32;
      float num2 = 1f;
      if (Game1.options != null)
      {
        num1 = Game1.options.lightingQuality;
        num2 = Game1.options.zoomLevel;
      }
      int width = (int) ((double) Game1.graphics.GraphicsDevice.Viewport.Width * (1.0 / (double) num2) + (double) Game1.tileSize) / (num1 / 2);
      int height = (int) ((double) Game1.graphics.GraphicsDevice.Viewport.Height * (1.0 / (double) num2) + (double) Game1.tileSize) / (num1 / 2);
      if (Game1.lightmap != null && Game1.lightmap.Width == width && Game1.lightmap.Height == height)
        return;
      if (Game1._lightmap != null)
        Game1._lightmap.Dispose();
      Game1._lightmap = new RenderTarget2D(Game1.graphics.GraphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
    }

    public static string CurrentSeasonDisplayName
    {
      get
      {
        return Game1.content.LoadString("Strings\\StringsFromCSFiles:" + Game1.currentSeason);
      }
    }

    public static byte gameMode
    {
      get
      {
        return Game1._gameMode;
      }
      set
      {
        if ((int) Game1._gameMode == (int) value)
          return;
        Console.WriteLine("gameMode was '{0}', set to '{1}'.", (object) Game1.GameModeToString(Game1._gameMode), (object) Game1.GameModeToString(value));
        Game1._gameMode = value;
      }
    }

    public bool IsSaving
    {
      get
      {
        return this._isSaving;
      }
      set
      {
        this._isSaving = value;
      }
    }

    public static IClickableMenu activeClickableMenu
    {
      get
      {
        return Game1._activeClickableMenu;
      }
      set
      {
        if (Game1._activeClickableMenu is IDisposable)
          (Game1._activeClickableMenu as IDisposable).Dispose();
        Game1._activeClickableMenu = value;
      }
    }

    public static IMinigame currentMinigame
    {
      get
      {
        return Game1._currentMinigame;
      }
      set
      {
        Game1._currentMinigame = value;
        if (value == null)
        {
          if (Game1.currentLocation == null)
            return;
          Game1.setRichPresence("location", (object) Game1.currentLocation.Name);
        }
        else
        {
          if (value.minigameId() == null)
            return;
          Game1.setRichPresence("minigame", (object) value.minigameId());
        }
      }
    }

    public static void ExitToTitle()
    {
      Game1.changeMusicTrack("none");
      Game1.setGameMode((byte) 0);
      Game1.exitToTitle = true;
    }

    public static bool IsMultiplayer
    {
      get
      {
        return (uint) Game1.multiplayerMode > 0U;
      }
    }

    public static bool IsClient
    {
      get
      {
        return (int) Game1.multiplayerMode == 1;
      }
    }

    public static bool IsServer
    {
      get
      {
        return (int) Game1.multiplayerMode == 2;
      }
    }

    public static bool IsMasterGame
    {
      get
      {
        if ((int) Game1.multiplayerMode != 0)
          return (int) Game1.multiplayerMode == 2;
        return true;
      }
    }

    public static ChatBox ChatBox
    {
      get
      {
        foreach (IClickableMenu onScreenMenu in Game1.onScreenMenus)
        {
          if (onScreenMenu is ChatBox)
            return (ChatBox) onScreenMenu;
        }
        return (ChatBox) null;
      }
    }

    public static Event CurrentEvent
    {
      get
      {
        if (Game1.currentLocation == null)
          return (Event) null;
        return Game1.currentLocation.currentEvent;
      }
    }

    public Game1()
    {
      Program.gamePtr = this;
      Game1.game1 = this;
      Program.sdk.EarlyInitialize();
      Game1.graphics = new GraphicsDeviceManager((Game) this);
      Game1.graphics.PreferredBackBufferWidth = 1280;
      Game1.graphics.PreferredBackBufferHeight = 720;
      Game1.viewport = new xTile.Dimensions.Rectangle(new Size(1280, 720));
      this.Window.AllowUserResizing = true;
      this.Content.RootDirectory = "Content";
      Game1._temporaryContent = this.CreateContentManager(this.Content.ServiceProvider, this.Content.RootDirectory);
      this.Window.ClientSizeChanged += new EventHandler<EventArgs>(this.Window_ClientSizeChanged);
      this.Exiting += new EventHandler<EventArgs>(this.exitEvent);
      Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
      LocalizedContentManager.OnLanguageChange += (LocalizedContentManager.LanguageChangedHandler) (code => this.TranslateFields());
    }

    private void TranslateFields()
    {
      Game1.samBandName = Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2156");
      Game1.elliottBookName = Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2157");
      Game1.objectSpriteSheet = Game1.content.Load<Texture2D>("Maps\\springobjects");
      Game1.dialogueFont = Game1.content.Load<SpriteFont>("Fonts\\SpriteFont1");
      Game1.dialogueFont.LineSpacing = 42;
      Game1.smallFont = Game1.content.Load<SpriteFont>("Fonts\\SmallFont");
      Game1.smallFont.LineSpacing = 26;
      Game1.tinyFont = Game1.content.Load<SpriteFont>("Fonts\\tinyFont");
      Game1.tinyFontBorder = Game1.content.Load<SpriteFont>("Fonts\\tinyFontBorder");
      Game1.objectInformation = Game1.content.Load<Dictionary<int, string>>("Data\\ObjectInformation");
      Game1.bigCraftablesInformation = Game1.content.Load<Dictionary<int, string>>("Data\\BigCraftablesInformation");
      Game1.achievements = Game1.content.Load<Dictionary<int, string>>("Data\\Achievements");
      CraftingRecipe.craftingRecipes = Game1.content.Load<Dictionary<string, string>>("Data//CraftingRecipes");
      CraftingRecipe.cookingRecipes = Game1.content.Load<Dictionary<string, string>>("Data//CookingRecipes");
      Game1.mouseCursors = Game1.content.Load<Texture2D>("LooseSprites\\Cursors");
      Game1.controllerMaps = Game1.content.Load<Texture2D>("LooseSprites\\ControllerMaps");
      Game1.NPCGiftTastes = Game1.content.Load<Dictionary<string, string>>("Data\\NPCGiftTastes");
    }

    public void exitEvent(object sender, EventArgs e)
    {
      if (Game1.IsServer && Game1.server != null)
        Game1.server.stopServer();
      Process.GetCurrentProcess().Kill();
    }

    public void refreshWindowSettings()
    {
      this.Window_ClientSizeChanged((object) null, (EventArgs) null);
    }

    private void Window_ClientSizeChanged(object sender, EventArgs e)
    {
      if (Game1.options == null)
        return;
      Microsoft.Xna.Framework.Rectangle oldBounds = new Microsoft.Xna.Framework.Rectangle(Game1.viewport.X, Game1.viewport.Y, Game1.viewport.Width, Game1.viewport.Height);
      this.Window.ClientSizeChanged -= new EventHandler<EventArgs>(this.Window_ClientSizeChanged);
      int width = Game1.graphics.IsFullScreen ? Game1.graphics.PreferredBackBufferWidth : this.Window.ClientBounds.Width;
      int height = Game1.graphics.IsFullScreen ? Game1.graphics.PreferredBackBufferHeight : this.Window.ClientBounds.Height;
      try
      {
        Viewport viewport = Game1.graphics.GraphicsDevice.Viewport;
        if (width < 1280)
        {
          Game1.graphics.PreferredBackBufferWidth = 1280;
          width = 1280;
        }
        if (height < 720)
        {
          Game1.graphics.PreferredBackBufferHeight = 720;
          height = 720;
        }
      }
      catch (Exception ex)
      {
        Game1.graphics.PreferredBackBufferWidth = 1280;
        Game1.graphics.PreferredBackBufferHeight = 720;
      }
      Game1.updateViewportForScreenSizeChange(false, width, height);
      if (Game1.bloom != null)
        Game1.bloom.reload();
      Game1.graphics.ApplyChanges();
      try
      {
        this.screen = new RenderTarget2D(Game1.graphics.GraphicsDevice, Math.Min(4096, (int) ((double) this.Window.ClientBounds.Width * (1.0 / (double) Game1.options.zoomLevel))), Math.Min(4096, (int) ((double) this.Window.ClientBounds.Height * (1.0 / (double) Game1.options.zoomLevel))));
        Game1.viewport = new xTile.Dimensions.Rectangle((int) Game1.player.position.X - Game1.viewport.Width / 2, (int) Game1.player.position.Y - Game1.viewport.Height / 2, (int) ((double) this.Window.ClientBounds.Width * (1.0 / (double) Game1.options.zoomLevel)), (int) ((double) this.Window.ClientBounds.Height * (1.0 / (double) Game1.options.zoomLevel)));
        Game1.previousViewportPosition = new Vector2(Game1.player.position.X - (float) (Game1.viewport.Width / 2), Game1.player.position.Y - (float) (Game1.viewport.Height / 2));
        Game1.allocateLightmap();
      }
      catch (Exception ex)
      {
      }
      try
      {
        Game1.graphics.GraphicsDevice.Viewport = !Game1.graphics.IsFullScreen ? new Viewport(new Microsoft.Xna.Framework.Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height)) : new Viewport(new Microsoft.Xna.Framework.Rectangle(0, 0, Game1.graphics.PreferredBackBufferWidth, Game1.graphics.PreferredBackBufferHeight));
      }
      catch (Exception ex)
      {
      }
      foreach (IClickableMenu onScreenMenu in Game1.onScreenMenus)
        onScreenMenu.gameWindowSizeChanged(oldBounds, new Microsoft.Xna.Framework.Rectangle(Game1.viewport.X, Game1.viewport.Y, Game1.viewport.Width, Game1.viewport.Height));
      if (Game1.currentMinigame != null)
        Game1.currentMinigame.changeScreenSize();
      if (Game1.activeClickableMenu != null)
        Game1.activeClickableMenu.gameWindowSizeChanged(oldBounds, new Microsoft.Xna.Framework.Rectangle(Game1.viewport.X, Game1.viewport.Y, Game1.viewport.Width, Game1.viewport.Height));
      if (Game1.activeClickableMenu is GameMenu && !Game1.overrideGameMenuReset)
        Game1.activeClickableMenu = (IClickableMenu) new GameMenu((Game1.activeClickableMenu as GameMenu).currentTab, -1);
      this.Window.ClientSizeChanged += new EventHandler<EventArgs>(this.Window_ClientSizeChanged);
    }

    private void Game1_Exiting(object sender, EventArgs e)
    {
      Program.sdk.Shutdown();
    }

    public static void setGameMode(byte mode)
    {
      Console.WriteLine("setGameMode( '{0}' )", (object) Game1.GameModeToString(mode));
      try
      {
        Game1._gameMode = mode;
        if (Game1.temporaryContent != null)
          Game1.temporaryContent.Unload();
        if ((int) mode != 0)
          return;
        int num = Game1.activeClickableMenu == null ? 0 : (Game1.currentGameTime.TotalGameTime.Seconds > 10 ? 1 : 0);
        Game1.activeClickableMenu = (IClickableMenu) new TitleMenu();
        if (num == 0)
          return;
        (Game1.activeClickableMenu as TitleMenu).skipToTitleButtons();
      }
      catch (Exception ex)
      {
      }
    }

    public static void updateViewportForScreenSizeChange(bool fullscreenChange, int width, int height)
    {
      Point centerPoint = new Point(Game1.viewport.X + Game1.viewport.Width / 2, Game1.viewport.Y + Game1.viewport.Height / 2);
      Game1.viewport = new xTile.Dimensions.Rectangle(centerPoint.X - width / 2, centerPoint.Y - height / 2, width + width % Game1.tileSize, height + height % Game1.tileSize);
      if (Game1.graphics.GraphicsDevice != null)
        Game1.allocateLightmap();
      if (Game1.currentLocation == null || ((Game1.viewport.X >= 0 ? 1 : (!Game1.currentLocation.IsOutdoors ? 1 : 0)) | (fullscreenChange ? 1 : 0)) == 0)
        return;
      if (Game1.eventUp)
      {
        if (Game1.currentLocation.map.DisplayHeight < height && Game1.currentLocation.map.DisplayWidth < width)
        {
          Game1.viewport = new xTile.Dimensions.Rectangle(Game1.graphics.GraphicsDevice.Viewport.X, Game1.graphics.GraphicsDevice.Viewport.Y, width + width % Game1.tileSize, height + height % Game1.tileSize);
          Game1.UpdateViewPort(true, new Point(Game1.player.getStandingX(), Game1.player.getStandingY()));
        }
        else
        {
          centerPoint = new Point(Game1.viewport.X + Game1.viewport.Width / 2, Game1.viewport.Y + Game1.viewport.Height / 2);
          Game1.viewport = new xTile.Dimensions.Rectangle(centerPoint.X - width / 2, centerPoint.Y - height / 2, width + width % Game1.tileSize, height + height % Game1.tileSize);
          Game1.UpdateViewPort(true, centerPoint);
        }
      }
      else
      {
        centerPoint = new Point(Game1.viewport.X + Game1.viewport.Width / 2, Game1.viewport.Y + Game1.viewport.Height / 2);
        Game1.viewport = new xTile.Dimensions.Rectangle(centerPoint.X - width / 2, centerPoint.Y - height / 2, width + width % Game1.tileSize, height + height % Game1.tileSize);
        Game1.UpdateViewPort(true, centerPoint);
      }
    }

    protected override void Initialize()
    {
      Game1.viewport = new xTile.Dimensions.Rectangle(new Size(1280, 720));
      Game1.keyboardDispatcher = new KeyboardDispatcher(this.Window);
      Game1.mapDisplayDevice = (IDisplayDevice) new XnaDisplayDevice(this.Content, this.GraphicsDevice);
      this.IsFixedTimeStep = true;
      string str = this.Content.RootDirectory;
      if (!File.Exists(Path.Combine(str, "XACT", "FarmerSounds.xgs")))
      {
        str = "C:\\Program Files (x86)\\Steam\\SteamApps\\common\\Stardew Valley\\Content";
        if (!Directory.Exists(str))
          str = "C:\\Program Files\\Steam\\SteamApps\\common\\Stardew Valley\\Content";
      }
      Game1.audioEngine = new AudioEngine(Path.Combine(str, "XACT", "FarmerSounds.xgs"));
      Game1.waveBank = new WaveBank(Game1.audioEngine, Path.Combine(str, "XACT", "Wave Bank.xwb"));
      Game1.soundBank = new SoundBank(Game1.audioEngine, Path.Combine(str, "XACT", "Sound Bank.xsb"));
      Game1.audioEngine.Update();
      Game1.musicCategory = Game1.audioEngine.GetCategory("Music");
      Game1.soundCategory = Game1.audioEngine.GetCategory("Sound");
      Game1.ambientCategory = Game1.audioEngine.GetCategory("Ambient");
      Game1.footstepCategory = Game1.audioEngine.GetCategory("Footsteps");
      Game1.currentSong = (Cue) null;
      if (Game1.soundBank != null)
      {
        Game1.fuseSound = Game1.soundBank.GetCue("fuse");
        Game1.wind = Game1.soundBank.GetCue("wind");
        Game1.chargeUpSound = Game1.soundBank.GetCue("toolCharge");
      }
      base.Initialize();
      Game1.graphics.SynchronizeWithVerticalRetrace = true;
      this.screen = new RenderTarget2D(Game1.graphics.GraphicsDevice, Game1.graphics.GraphicsDevice.Viewport.Width, Game1.graphics.GraphicsDevice.Viewport.Height);
      Game1.allocateLightmap();
      AmbientLocationSounds.InitShared();
      Game1.setGameMode((byte) 0);
      Program.sdk.Initialize();
      Game1.previousViewportPosition = Vector2.Zero;
      Game1.setRichPresence("menus", (object) null);
    }

    public static void pauseThenDoFunction(int pauseTime, Game1.afterFadeFunction function)
    {
      Game1.afterPause = function;
      Game1.pauseThenDoFunctionTimer = pauseTime;
    }

    public static string dayOrNight()
    {
      string str = "_day";
      if (DateTime.Now.TimeOfDay.TotalHours >= (double) (int) (1.75 * Math.Sin(2.0 * Math.PI / 365.0 * (double) DateTime.Now.DayOfYear - 79.0) + 18.75) || DateTime.Now.TimeOfDay.TotalHours < 5.0)
        str = "_night";
      return str;
    }

    public void dummyLoad()
    {
      Game1.content.Unload();
      Game1.content.Dispose();
      Game1.game1.Content = (ContentManager) this.CreateContentManager(Game1.content.ServiceProvider, Game1.content.RootDirectory);
      Game1.game1.xTileContent = this.CreateContentManager(Game1.content.ServiceProvider, Game1.content.RootDirectory);
      Game1._temporaryContent.Unload();
      Game1._temporaryContent.Dispose();
      Game1._temporaryContent = ((LocalizedContentManager) Game1.game1.Content).CreateTemporary();
      Game1.mapDisplayDevice = (IDisplayDevice) new XnaDisplayDevice(this.Content, this.GraphicsDevice);
      this.LoadContent();
      Game1.exitActiveMenu();
      Game1.setGameMode((byte) 3);
    }

    protected virtual LocalizedContentManager CreateContentManager(IServiceProvider serviceProvider, string rootDirectory)
    {
      return new LocalizedContentManager(serviceProvider, rootDirectory);
    }

    protected override void LoadContent()
    {
      Game1.options = new Options();
      Game1.options.musicVolumeLevel = 1f;
      Game1.options.soundVolumeLevel = 1f;
      Game1.content = this.CreateContentManager(this.Content.ServiceProvider, this.Content.RootDirectory);
      this.xTileContent = this.CreateContentManager(Game1.content.ServiceProvider, Game1.content.RootDirectory);
      LocalizedContentManager.OnLanguageChange += (LocalizedContentManager.LanguageChangedHandler) (code => this.TranslateFields());
      CraftingRecipe.InitShared();
      Critter.InitShared();
      Game1.spriteBatch = new SpriteBatch(this.GraphicsDevice);
      Game1.otherFarmers = new Dictionary<long, Farmer>();
      Game1.daybg = this.Content.Load<Texture2D>("LooseSprites\\daybg");
      Game1.nightbg = this.Content.Load<Texture2D>("LooseSprites\\nightbg");
      Game1.menuTexture = this.Content.Load<Texture2D>("Maps\\MenuTiles");
      Game1.lantern = this.Content.Load<Texture2D>("LooseSprites\\Lighting\\lantern");
      Game1.windowLight = this.Content.Load<Texture2D>("LooseSprites\\Lighting\\windowLight");
      Game1.sconceLight = this.Content.Load<Texture2D>("LooseSprites\\Lighting\\sconceLight");
      Game1.cauldronLight = this.Content.Load<Texture2D>("LooseSprites\\Lighting\\greenLight");
      Game1.indoorWindowLight = this.Content.Load<Texture2D>("LooseSprites\\Lighting\\indoorWindowLight");
      Game1.shadowTexture = this.Content.Load<Texture2D>("LooseSprites\\shadow");
      Game1.mouseCursors = this.Content.Load<Texture2D>("LooseSprites\\Cursors");
      Game1.controllerMaps = Game1.content.Load<Texture2D>("LooseSprites\\ControllerMaps");
      Game1.animations = this.Content.Load<Texture2D>("TileSheets\\animations");
      Game1.achievements = Game1.content.Load<Dictionary<int, string>>("Data\\Achievements");
      if (Game1.bloom != null)
        Game1.bloom.Visible = false;
      Game1.fadeToBlackRect = new Texture2D(this.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
      Color[] data1 = new Color[1]{ Color.White };
      Game1.fadeToBlackRect.SetData<Color>(data1);
      Game1.dialogueWidth = Math.Min(1280 - Game1.tileSize * 4, Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Width - Game1.tileSize * 4);
      NameSelect.load();
      Game1.NPCGiftTastes = Game1.content.Load<Dictionary<string, string>>("Data\\NPCGiftTastes");
      Color[] data2 = new Color[1];
      Game1.staminaRect = new Texture2D(this.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
      Game1.onScreenMenus.Clear();
      Game1.onScreenMenus.Add((IClickableMenu) new Toolbar());
      for (int index = 0; index < data2.Length; ++index)
        data2[index] = new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      Game1.staminaRect.SetData<Color>(data2);
      Game1.saveOnNewDay = true;
      Game1.littleEffect = new Texture2D(this.GraphicsDevice, 4, 4, false, SurfaceFormat.Color);
      Color[] data3 = new Color[16];
      for (int index = 0; index < data3.Length; ++index)
        data3[index] = new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      Game1.littleEffect.SetData<Color>(data3);
      for (int index = 0; index < 70; ++index)
        Game1.rainDrops[index] = new RainDrop(Game1.random.Next(Game1.viewport.Width), Game1.random.Next(Game1.viewport.Height), Game1.random.Next(4), Game1.random.Next(70));
      Game1.dayTimeMoneyBox = new DayTimeMoneyBox();
      Game1.onScreenMenus.Add((IClickableMenu) Game1.dayTimeMoneyBox);
      Game1.buffsDisplay = new BuffsDisplay();
      Game1.onScreenMenus.Add((IClickableMenu) Game1.buffsDisplay);
      Game1.dialogueFont = Game1.content.Load<SpriteFont>("Fonts\\SpriteFont1");
      Game1.dialogueFont.LineSpacing = 42;
      Game1.smallFont = Game1.content.Load<SpriteFont>("Fonts\\SmallFont");
      Game1.smallFont.LineSpacing = 26;
      Game1.tinyFont = Game1.content.Load<SpriteFont>("Fonts\\tinyFont");
      Game1.tinyFontBorder = Game1.content.Load<SpriteFont>("Fonts\\tinyFontBorder");
      Game1.objectSpriteSheet = Game1.content.Load<Texture2D>("Maps\\springobjects");
      Game1.cropSpriteSheet = Game1.content.Load<Texture2D>("TileSheets\\crops");
      Game1.emoteSpriteSheet = Game1.content.Load<Texture2D>("TileSheets\\emotes");
      Game1.debrisSpriteSheet = Game1.content.Load<Texture2D>("TileSheets\\debris");
      Game1.bigCraftableSpriteSheet = Game1.content.Load<Texture2D>("TileSheets\\Craftables");
      Game1.rainTexture = Game1.content.Load<Texture2D>("TileSheets\\rain");
      Game1.buffsIcons = Game1.content.Load<Texture2D>("TileSheets\\BuffsIcons");
      Game1.objectInformation = Game1.content.Load<Dictionary<int, string>>("Data\\ObjectInformation");
      Game1.bigCraftablesInformation = Game1.content.Load<Dictionary<int, string>>("Data\\BigCraftablesInformation");
      if ((int) Game1.gameMode == 4)
      {
        Game1.fadeToBlackAlpha = -0.5f;
        Game1.fadeIn = true;
      }
      if (Game1.random.NextDouble() < 0.7)
      {
        Game1.isDebrisWeather = true;
        Game1.populateDebrisWeatherArray();
      }
      FarmerRenderer.hairStylesTexture = Game1.content.Load<Texture2D>("Characters\\Farmer\\hairstyles");
      FarmerRenderer.shirtsTexture = Game1.content.Load<Texture2D>("Characters\\Farmer\\shirts");
      FarmerRenderer.hatsTexture = Game1.content.Load<Texture2D>("Characters\\Farmer\\hats");
      FarmerRenderer.accessoriesTexture = Game1.content.Load<Texture2D>("Characters\\Farmer\\accessories");
      Furniture.furnitureTexture = Game1.content.Load<Texture2D>("TileSheets\\furniture");
      SpriteText.spriteTexture = Game1.content.Load<Texture2D>("LooseSprites\\font_bold");
      SpriteText.coloredTexture = Game1.content.Load<Texture2D>("LooseSprites\\font_colored");
      Tool.weaponsTexture = Game1.content.Load<Texture2D>("TileSheets\\weapons");
      Projectile.projectileSheet = Game1.content.Load<Texture2D>("TileSheets\\Projectiles");
      Game1.resetPlayer();
    }

    public static void resetPlayer()
    {
      Game1.player = new Farmer(new FarmerSprite((Texture2D) null), new Vector2(192f, 192f), 1, "Max", new List<Item>()
      {
        (Item) new Axe(),
        (Item) new Hoe(),
        (Item) new WateringCan(),
        (Item) new Pickaxe(),
        (Item) new MeleeWeapon(47),
        (Item) null
      }, true);
      Game1.player.Name = "";
      long uniqueMultiplayerId = Game1.player.uniqueMultiplayerID;
      Game1.player.FarmerSprite.setOwner(Game1.player);
      Game1.player.uniqueMultiplayerID = uniqueMultiplayerId;
      Game1.player.craftingRecipes.Add("Chest", 0);
      Game1.player.craftingRecipes.Add("Wood Fence", 0);
      Game1.player.craftingRecipes.Add("Gate", 0);
      Game1.player.craftingRecipes.Add("Torch", 0);
      Game1.player.craftingRecipes.Add("Campfire", 0);
      Game1.player.craftingRecipes.Add("Wood Path", 0);
      Game1.player.craftingRecipes.Add("Cobblestone Path", 0);
      Game1.player.craftingRecipes.Add("Gravel Path", 0);
      Game1.player.cookingRecipes.Add("Fried Egg", 0);
      Game1.player.songsHeard.Add("title_day");
      Game1.player.songsHeard.Add("title_night");
      Game1.player.changeShirt(0);
      Game1.player.changeSkinColor(0);
    }

    public static void resetVariables()
    {
      Game1.xLocationAfterWarp = 0;
      Game1.yLocationAfterWarp = 0;
      Game1.gameTimeInterval = 0;
      Game1.currentQuestionChoice = 0;
      Game1.currentDialogueCharacterIndex = 0;
      Game1.dialogueTypingInterval = 0;
      Game1.dayOfMonth = 0;
      Game1.year = 1;
      Game1.timeOfDay = 600;
      Game1.numberOfSelectedItems = -1;
      Game1.priceOfSelectedItem = 0;
      Game1.currentWallpaper = 0;
      Game1.farmerWallpaper = 22;
      Game1.wallpaperPrice = 75;
      Game1.currentFloor = 3;
      Game1.FarmerFloor = 29;
      Game1.floorPrice = 75;
      Game1.countdownToWedding = 0;
      Game1.facingDirectionAfterWarp = 0;
      Game1.dialogueWidth = 0;
      Game1.menuChoice = 0;
      Game1.tvStation = -1;
      Game1.currentBillboard = 0;
      Game1.facingDirectionAfterWarp = 0;
      Game1.tmpTimeOfDay = 0;
      Game1.percentageToWinStardewHero = 70;
      Game1.mouseClickPolling = 0;
      Game1.weatherIcon = 0;
      Game1.hitShakeTimer = 0;
      Game1.staminaShakeTimer = 0;
      Game1.pauseThenDoFunctionTimer = 0;
      Game1.weatherForTomorrow = 0;
      Game1.currentSongIndex = 3;
    }

    public static void playSound(string cueName)
    {
      if (Game1.soundBank == null)
        return;
      try
      {
        Game1.soundBank.PlayCue(cueName);
      }
      catch (Exception ex)
      {
        Game1.debugOutput = Game1.parseText(ex.Message);
        Console.WriteLine((object) ex);
      }
    }

    public static void setRichPresence(string friendlyName, object argument = null)
    {
      // ISSUE: reference to a compiler-generated method
      uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(friendlyName);
      if (stringHash <= 2353551851U)
      {
        if (stringHash <= 819128320U)
        {
          if ((int) stringHash != 200649126)
          {
            if ((int) stringHash != 819128320 || !(friendlyName == "giantcrop"))
              return;
            Game1.debugPresenceString = string.Format("Just harvested a Giant {0}", argument);
          }
          else
          {
            if (!(friendlyName == "location"))
              return;
            Game1.debugPresenceString = string.Format("At {0}", argument);
          }
        }
        else if ((int) stringHash != 1266391031)
        {
          if ((int) stringHash != -1941415445 || !(friendlyName == "menus"))
            return;
          Game1.debugPresenceString = "In menus";
        }
        else
        {
          if (!(friendlyName == "wedding"))
            return;
          Game1.debugPresenceString = string.Format("Getting married to {0}", argument);
        }
      }
      else if (stringHash <= 2899391285U)
      {
        if ((int) stringHash != -1845998632)
        {
          if ((int) stringHash != -1395576011 || !(friendlyName == "fishing"))
            return;
          Game1.debugPresenceString = string.Format("Fishing at {0}", argument);
        }
        else
        {
          if (!(friendlyName == "minigame"))
            return;
          Game1.debugPresenceString = string.Format("Playing {0}", argument);
        }
      }
      else if ((int) stringHash != -762514426)
      {
        if ((int) stringHash != -493073483 || !(friendlyName == "festival"))
          return;
        Game1.debugPresenceString = string.Format("At {0}", argument);
      }
      else
      {
        if (!(friendlyName == "earnings"))
          return;
        Game1.debugPresenceString = string.Format("Made {0}g last night", argument);
      }
    }

    public static void loadForNewGame(bool loadedGame = false)
    {
      Game1.locations.Clear();
      Game1.mailbox.Clear();
      Game1.currentLightSources.Clear();
      if (Game1.dealerCalicoJackTotal != null)
        Game1.dealerCalicoJackTotal.Clear();
      Game1.questionChoices.Clear();
      Game1.hudMessages.Clear();
      Game1.weddingToday = false;
      Game1.countdownToWedding = 0;
      Game1.timeOfDay = 600;
      Game1.currentSeason = "spring";
      if (!loadedGame)
        Game1.year = 1;
      Game1.dayOfMonth = 0;
      Game1.pickingTool = false;
      Game1.isQuestion = false;
      Game1.nonWarpFade = false;
      Game1.particleRaining = false;
      Game1.newDay = false;
      Game1.inMine = false;
      Game1.isEating = false;
      Game1.menuUp = false;
      Game1.eventUp = false;
      Game1.viewportFreeze = false;
      Game1.eventOver = false;
      Game1.nameSelectUp = false;
      Game1.screenGlow = false;
      Game1.screenGlowHold = false;
      Game1.screenGlowUp = false;
      Game1.progressBar = false;
      Game1.isRaining = false;
      Game1.killScreen = false;
      Game1.coopDwellerBorn = false;
      Game1.messagePause = false;
      Game1.isDebrisWeather = false;
      Game1.boardingBus = false;
      Game1.listeningForKeyControlDefinitions = false;
      Game1.weddingToday = false;
      Game1.exitToTitle = false;
      Game1.isRaining = false;
      Game1.dialogueUp = false;
      Game1.currentBillboard = 0;
      Game1.farmerShouldPassOut = false;
      Game1.displayHUD = true;
      Game1.messageAfterPause = "";
      Game1.fertilizer = "";
      Game1.samBandName = Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2156");
      Game1.slotResult = "";
      Game1.background = (Background) null;
      Game1.currentCursorTile = Vector2.Zero;
      Game1.resetVariables();
      Game1.chanceToRainTomorrow = 0.0;
      Game1.dailyLuck = 0.001;
      if (!loadedGame)
      {
        Game1.stats = new Stats();
        Game1.options = new Options();
      }
      Game1.cropsOfTheWeek = Utility.cropsOfTheWeek();
      if (Game1.IsMultiplayer)
        Game1.onScreenMenus.Add((IClickableMenu) new ChatBox());
      Game1.outdoorLight = Color.White;
      Game1.ambientLight = Color.White;
      int parentSheetIndex = Game1.random.Next(194, 240);
      if (parentSheetIndex == 217)
        parentSheetIndex = 216;
      Game1.dishOfTheDay = new Object(Vector2.Zero, parentSheetIndex, Game1.random.Next(1, 4 + (Game1.random.NextDouble() < 0.08 ? 10 : 0)));
      Game1.locations.Clear();
      Map m = Game1.game1.xTileContent.Load<Map>("Maps\\FarmHouse");
      IDisplayDevice mapDisplayDevice = Game1.mapDisplayDevice;
      m.LoadTileSheets(mapDisplayDevice);
      string name = "FarmHouse";
      Game1.currentLocation = (GameLocation) new FarmHouse(m, name);
      Game1.locations.Add(Game1.currentLocation);
      Game1.locations.Add((GameLocation) new Farm(Game1.game1.xTileContent.Load<Map>("Maps\\" + Farm.getMapNameFromTypeInt(Game1.whichFarm)), "Farm"));
      if (Game1.whichFarm == 3)
      {
        for (int index = 0; index < 28; ++index)
          Game1.getFarm().doDailyMountainFarmUpdate();
      }
      Game1.locations.Add((GameLocation) new FarmCave(Game1.game1.xTileContent.Load<Map>("Maps\\FarmCave"), "FarmCave"));
      Game1.locations.Add((GameLocation) new Town(Game1.game1.xTileContent.Load<Map>("Maps\\Town"), "Town"));
      Game1.locations.Add(new GameLocation(Game1.game1.xTileContent.Load<Map>("Maps\\JoshHouse"), "JoshHouse"));
      Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\George"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float) (16 * Game1.tileSize), (float) (22 * Game1.tileSize)), "JoshHouse", 0, "George", false, (Dictionary<int, int[]>) null, Game1.content.Load<Texture2D>("Portraits\\George")));
      Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Evelyn"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float) (2 * Game1.tileSize), (float) (17 * Game1.tileSize)), "JoshHouse", 1, "Evelyn", false, (Dictionary<int, int[]>) null, Game1.content.Load<Texture2D>("Portraits\\Evelyn")));
      Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Alex"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float) (19 * Game1.tileSize), (float) (5 * Game1.tileSize)), "JoshHouse", 3, "Alex", true, (Dictionary<int, int[]>) null, Game1.content.Load<Texture2D>("Portraits\\Alex")));
      Game1.locations.Add(new GameLocation(Game1.game1.xTileContent.Load<Map>("Maps\\HaleyHouse"), "HaleyHouse"));
      Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Emily"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float) (16 * Game1.tileSize), (float) (5 * Game1.tileSize)), "HaleyHouse", 2, "Emily", true, (Dictionary<int, int[]>) null, Game1.content.Load<Texture2D>("Portraits\\Emily")));
      Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Haley"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float) (8 * Game1.tileSize), (float) (7 * Game1.tileSize)), "HaleyHouse", 1, "Haley", true, (Dictionary<int, int[]>) null, Game1.content.Load<Texture2D>("Portraits\\Haley")));
      Game1.locations.Add(new GameLocation(Game1.game1.xTileContent.Load<Map>("Maps\\SamHouse"), "SamHouse"));
      Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Jodi"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float) (4 * Game1.tileSize), (float) (5 * Game1.tileSize)), "SamHouse", 0, "Jodi", false, (Dictionary<int, int[]>) null, Game1.content.Load<Texture2D>("Portraits\\Jodi")));
      Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Sam"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float) (22 * Game1.tileSize), (float) (13 * Game1.tileSize)), "SamHouse", 1, "Sam", true, (Dictionary<int, int[]>) null, Game1.content.Load<Texture2D>("Portraits\\Sam")));
      Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Vincent"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float) (10 * Game1.tileSize), (float) (23 * Game1.tileSize)), "SamHouse", 2, "Vincent", false, (Dictionary<int, int[]>) null, Game1.content.Load<Texture2D>("Portraits\\Vincent")));
      if (Game1.year > 1)
        Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Kent"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float) (8 * Game1.tileSize), (float) (13 * Game1.tileSize)), "SamHouse", 2, "Kent", false, (Dictionary<int, int[]>) null, Game1.content.Load<Texture2D>("Portraits\\Kent")));
      Game1.locations.Add(new GameLocation(Game1.game1.xTileContent.Load<Map>("Maps\\Blacksmith"), "Blacksmith"));
      Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Clint"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float) (3 * Game1.tileSize), (float) (13 * Game1.tileSize)), "Blacksmith", 2, "Clint", false, (Dictionary<int, int[]>) null, Game1.content.Load<Texture2D>("Portraits\\Clint")));
      Game1.locations.Add(new GameLocation(Game1.game1.xTileContent.Load<Map>("Maps\\ManorHouse"), "ManorHouse"));
      Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Lewis"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float) (8 * Game1.tileSize), (float) (5 * Game1.tileSize)), "ManorHouse", 0, "Lewis", false, (Dictionary<int, int[]>) null, Game1.content.Load<Texture2D>("Portraits\\Lewis")));
      Game1.locations.Add((GameLocation) new SeedShop(Game1.game1.xTileContent.Load<Map>("Maps\\SeedShop"), "SeedShop"));
      Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Caroline"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float) (22 * Game1.tileSize), (float) (5 * Game1.tileSize)), "SeedShop", 2, "Caroline", false, (Dictionary<int, int[]>) null, Game1.content.Load<Texture2D>("Portraits\\Caroline")));
      Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Abigail"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float) Game1.tileSize, (float) (9 * Game1.tileSize + Game1.pixelZoom)), "SeedShop", 3, "Abigail", true, (Dictionary<int, int[]>) null, Game1.content.Load<Texture2D>("Portraits\\Abigail")));
      Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Pierre"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float) (4 * Game1.tileSize), (float) (17 * Game1.tileSize)), "SeedShop", 2, "Pierre", false, (Dictionary<int, int[]>) null, Game1.content.Load<Texture2D>("Portraits\\Pierre")));
      Game1.locations.Add(new GameLocation(Game1.game1.xTileContent.Load<Map>("Maps\\Saloon"), "Saloon"));
      Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Gus"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float) (18 * Game1.tileSize), (float) (6 * Game1.tileSize)), "Saloon", 2, "Gus", false, (Dictionary<int, int[]>) null, Game1.content.Load<Texture2D>("Portraits\\Gus")));
      Game1.locations.Add(new GameLocation(Game1.game1.xTileContent.Load<Map>("Maps\\Trailer"), "Trailer"));
      Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Pam"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float) (15 * Game1.tileSize), (float) (4 * Game1.tileSize)), "Trailer", 2, "Pam", false, (Dictionary<int, int[]>) null, Game1.content.Load<Texture2D>("Portraits\\Pam")));
      Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Penny"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float) (4 * Game1.tileSize), (float) (9 * Game1.tileSize)), "Trailer", 1, "Penny", true, (Dictionary<int, int[]>) null, Game1.content.Load<Texture2D>("Portraits\\Penny")));
      Game1.locations.Add(new GameLocation(Game1.game1.xTileContent.Load<Map>("Maps\\Hospital"), "Hospital"));
      Game1.locations.Add(new GameLocation(Game1.game1.xTileContent.Load<Map>("Maps\\HarveyRoom"), "HarveyRoom"));
      Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Harvey"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float) (13 * Game1.tileSize), (float) (4 * Game1.tileSize)), "HarveyRoom", 1, "Harvey", true, (Dictionary<int, int[]>) null, Game1.content.Load<Texture2D>("Portraits\\Harvey")));
      Game1.locations.Add((GameLocation) new Beach(Game1.game1.xTileContent.Load<Map>("Maps\\Beach"), "Beach"));
      Game1.locations.Add(new GameLocation(Game1.game1.xTileContent.Load<Map>("Maps\\ElliottHouse"), "ElliottHouse"));
      Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Elliott"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float) Game1.tileSize, (float) (5 * Game1.tileSize)), "ElliottHouse", 0, "Elliott", true, (Dictionary<int, int[]>) null, Game1.content.Load<Texture2D>("Portraits\\Elliott")));
      Game1.locations.Add((GameLocation) new Mountain(Game1.game1.xTileContent.Load<Map>("Maps\\Mountain"), "Mountain"));
      Game1.locations.Add(new GameLocation(Game1.game1.xTileContent.Load<Map>("Maps\\ScienceHouse"), "ScienceHouse"));
      Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Maru"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float) (2 * Game1.tileSize), (float) (4 * Game1.tileSize)), "ScienceHouse", 3, "Maru", true, (Dictionary<int, int[]>) null, Game1.content.Load<Texture2D>("Portraits\\Maru")));
      Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Robin"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float) (21 * Game1.tileSize), (float) (4 * Game1.tileSize)), "ScienceHouse", 1, "Robin", false, (Dictionary<int, int[]>) null, Game1.content.Load<Texture2D>("Portraits\\Robin")));
      Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Demetrius"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float) (19 * Game1.tileSize), (float) (4 * Game1.tileSize)), "ScienceHouse", 1, "Demetrius", false, (Dictionary<int, int[]>) null, Game1.content.Load<Texture2D>("Portraits\\Demetrius")));
      Game1.locations.Add(new GameLocation(Game1.game1.xTileContent.Load<Map>("Maps\\SebastianRoom"), "SebastianRoom"));
      Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Sebastian"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float) (10 * Game1.tileSize), (float) (9 * Game1.tileSize)), "SebastianRoom", 1, "Sebastian", true, (Dictionary<int, int[]>) null, Game1.content.Load<Texture2D>("Portraits\\Sebastian")));
      GameLocation gameLocation = new GameLocation(Game1.game1.xTileContent.Load<Map>("Maps\\Tent"), "Tent");
      gameLocation.addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Linus"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2(2f, 2f) * (float) Game1.tileSize, "Tent", 2, "Linus", false, (Dictionary<int, int[]>) null, Game1.content.Load<Texture2D>("Portraits\\Linus")));
      Game1.locations.Add(gameLocation);
      Game1.locations.Add((GameLocation) new Forest(Game1.game1.xTileContent.Load<Map>("Maps\\Forest"), "Forest"));
      Game1.locations.Add((GameLocation) new WizardHouse(Game1.game1.xTileContent.Load<Map>("Maps\\WizardHouse"), "WizardHouse"));
      Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Wizard"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float) (3 * Game1.tileSize), (float) (17 * Game1.tileSize)), "WizardHouse", 2, "Wizard", false, (Dictionary<int, int[]>) null, Game1.content.Load<Texture2D>("Portraits\\Wizard")));
      Game1.locations.Add(new GameLocation(Game1.game1.xTileContent.Load<Map>("Maps\\AnimalShop"), "AnimalShop"));
      Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Marnie"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float) (12 * Game1.tileSize), (float) (14 * Game1.tileSize)), "AnimalShop", 2, "Marnie", false, (Dictionary<int, int[]>) null, Game1.content.Load<Texture2D>("Portraits\\Marnie")));
      Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Shane"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float) (25 * Game1.tileSize), (float) (6 * Game1.tileSize)), "AnimalShop", 3, "Shane", true, (Dictionary<int, int[]>) null, Game1.content.Load<Texture2D>("Portraits\\Shane")));
      Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Jas"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float) (4 * Game1.tileSize), (float) (6 * Game1.tileSize)), "AnimalShop", 2, "Jas", false, (Dictionary<int, int[]>) null, Game1.content.Load<Texture2D>("Portraits\\Jas")));
      Game1.locations.Add(new GameLocation(Game1.game1.xTileContent.Load<Map>("Maps\\LeahHouse"), "LeahHouse"));
      Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Leah"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float) (3 * Game1.tileSize), (float) (7 * Game1.tileSize)), "LeahHouse", 3, "Leah", true, (Dictionary<int, int[]>) null, Game1.content.Load<Texture2D>("Portraits\\Leah")));
      Game1.locations.Add((GameLocation) new BusStop(Game1.game1.xTileContent.Load<Map>("Maps\\BusStop"), "BusStop"));
      Game1.locations.Add(new GameLocation(Game1.game1.xTileContent.Load<Map>("Maps\\Mine"), "Mine"));
      Game1.locations[Game1.locations.Count - 1].objects.Add(new Vector2(27f, 8f), new Object(Vector2.Zero, 78, false));
      Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Dwarf"), 0, Game1.tileSize / 4, 24), new Vector2((float) (43 * Game1.tileSize), (float) (6 * Game1.tileSize)), "Mine", 2, "Dwarf", false, (Dictionary<int, int[]>) null, Game1.content.Load<Texture2D>("Portraits\\Dwarf"))
      {
        breather = false
      });
      Game1.locations.Add((GameLocation) new Sewer(Game1.game1.xTileContent.Load<Map>("Maps\\Sewer"), "Sewer"));
      Game1.locations.Add(new GameLocation(Game1.game1.xTileContent.Load<Map>("Maps\\BugLand"), "BugLand"));
      Game1.locations.Add((GameLocation) new Desert(Game1.game1.xTileContent.Load<Map>("Maps\\Desert"), "Desert"));
      Game1.locations.Add((GameLocation) new Club(Game1.game1.xTileContent.Load<Map>("Maps\\Club"), "Club"));
      Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\MrQi"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float) (8 * Game1.tileSize), (float) (4 * Game1.tileSize)), "Club", 0, "Mister Qi", false, (Dictionary<int, int[]>) null, Game1.content.Load<Texture2D>("Portraits\\MrQi")));
      Game1.locations.Add(new GameLocation(Game1.game1.xTileContent.Load<Map>("Maps\\SandyHouse"), "SandyHouse"));
      Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Sandy"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float) (2 * Game1.tileSize), (float) (5 * Game1.tileSize)), "SandyHouse", 2, "Sandy", false, (Dictionary<int, int[]>) null, Game1.content.Load<Texture2D>("Portraits\\Sandy")));
      Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Bouncer"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float) (17 * Game1.tileSize), (float) (3 * Game1.tileSize)), "SandyHouse", 2, "Bouncer", false, (Dictionary<int, int[]>) null, Game1.content.Load<Texture2D>("Portraits\\Bouncer")));
      Game1.locations.Add((GameLocation) new LibraryMuseum(Game1.game1.xTileContent.Load<Map>("Maps\\ArchaeologyHouse"), "ArchaeologyHouse"));
      Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Gunther"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float) (3 * Game1.tileSize), (float) (8 * Game1.tileSize)), "ArchaeologyHouse", 2, "Gunther", false, (Dictionary<int, int[]>) null, Game1.content.Load<Texture2D>("Portraits\\Gunther")));
      Game1.locations.Add(new GameLocation(Game1.game1.xTileContent.Load<Map>("Maps\\WizardHouseBasement"), "WizardHouseBasement"));
      Game1.locations.Add((GameLocation) new AdventureGuild(Game1.game1.xTileContent.Load<Map>("Maps\\AdventureGuild"), "AdventureGuild"));
      Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Marlon"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float) (5 * Game1.tileSize), (float) (11 * Game1.tileSize)), "AdventureGuild", 2, "Marlon", false, (Dictionary<int, int[]>) null, Game1.content.Load<Texture2D>("Portraits\\Marlon")));
      Game1.locations.Add((GameLocation) new Woods(Game1.game1.xTileContent.Load<Map>("Maps\\Woods"), "Woods"));
      Game1.locations.Add((GameLocation) new Railroad(Game1.game1.xTileContent.Load<Map>("Maps\\Railroad"), "Railroad"));
      Game1.locations.Add(new GameLocation(Game1.game1.xTileContent.Load<Map>("Maps\\WitchSwamp"), "WitchSwamp"));
      Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Henchman"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float) (20 * Game1.tileSize), (float) (29 * Game1.tileSize)), "WitchSwamp", 2, "Henchman", false, (Dictionary<int, int[]>) null, Game1.content.Load<Texture2D>("Portraits\\Henchman")));
      Game1.locations.Add(new GameLocation(Game1.game1.xTileContent.Load<Map>("Maps\\WitchHut"), "WitchHut"));
      Game1.locations.Add(new GameLocation(Game1.game1.xTileContent.Load<Map>("Maps\\WitchWarpCave"), "WitchWarpCave"));
      Game1.locations.Add((GameLocation) new Summit(Game1.game1.xTileContent.Load<Map>("Maps\\Summit"), "Summit"));
      Game1.locations.Add(new GameLocation(Game1.game1.xTileContent.Load<Map>("Maps\\FishShop"), "FishShop"));
      Game1.locations[Game1.locations.Count - 1].addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Willy"), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float) (5 * Game1.tileSize), (float) (4 * Game1.tileSize)), "FishShop", 2, "Willy", false, (Dictionary<int, int[]>) null, Game1.content.Load<Texture2D>("Portraits\\Willy")));
      Game1.locations.Add(new GameLocation(Game1.game1.xTileContent.Load<Map>("Maps\\BathHouse_Entry"), "BathHouse_Entry"));
      Game1.locations.Add(new GameLocation(Game1.game1.xTileContent.Load<Map>("Maps\\BathHouse_MensLocker"), "BathHouse_MensLocker"));
      Game1.locations.Add(new GameLocation(Game1.game1.xTileContent.Load<Map>("Maps\\BathHouse_WomensLocker"), "BathHouse_WomensLocker"));
      Game1.locations.Add((GameLocation) new BathHousePool(Game1.game1.xTileContent.Load<Map>("Maps\\BathHouse_Pool"), "BathHouse_Pool"));
      Game1.locations.Add((GameLocation) new CommunityCenter("CommunityCenter"));
      Game1.locations.Add((GameLocation) new JojaMart(Game1.game1.xTileContent.Load<Map>("Maps\\JojaMart"), "JojaMart"));
      Game1.locations.Add(new GameLocation(Game1.game1.xTileContent.Load<Map>("Maps\\Greenhouse"), "Greenhouse"));
      Game1.locations.Add(new GameLocation(Game1.game1.xTileContent.Load<Map>("Maps\\SkullCave"), "SkullCave"));
      Game1.locations.Add(new GameLocation(Game1.game1.xTileContent.Load<Map>("Maps\\Backwoods"), "Backwoods"));
      Game1.locations.Add(new GameLocation(Game1.game1.xTileContent.Load<Map>("Maps\\Tunnel"), "Tunnel"));
      Game1.locations.Add((GameLocation) new Cellar(Game1.game1.xTileContent.Load<Map>("Maps\\Cellar"), "Cellar"));
      NPC.populateRoutesFromLocationToLocationList();
      Game1.player.addQuest(9);
      Game1.player.currentLocation = Game1.getLocationFromName("FarmHouse");
      Game1.hudMessages.Clear();
      Game1.hasLoadedGame = true;
      Game1.setGraphicsForSeason();
    }

    protected override void UnloadContent()
    {
      base.UnloadContent();
      Game1.spriteBatch.Dispose();
      Game1.content.Unload();
      this.xTileContent.Unload();
      if (Game1.server == null)
        return;
      Game1.server.stopServer();
    }

    public void errorUpdateLoop()
    {
      if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.B))
      {
        Program.GameTesterMode = false;
        Game1.gameMode = (byte) 3;
      }
      if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
      {
        Program.gamePtr.Exit();
        Environment.Exit(1);
      }
      this.Update(new GameTime());
      this.BeginDraw();
      this.Draw(new GameTime());
      this.EndDraw();
    }

    public static void showRedMessage(string message)
    {
      Game1.addHUDMessage(new HUDMessage(message, 3));
      if (!message.Contains("Inventory"))
      {
        Game1.playSound("cancel");
      }
      else
      {
        if (Game1.player.mailReceived.Contains("BackpackTip"))
          return;
        Game1.player.mailReceived.Add("BackpackTip");
        Game1.addMailForTomorrow("pierreBackpack", false, false);
      }
    }

    public static void showRedMessageUsingLoadString(string loadString)
    {
      Game1.showRedMessage(Game1.content.LoadString(loadString));
    }

    public static bool didPlayerJustLeftClick()
    {
      return Mouse.GetState().LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && Game1.oldMouseState.LeftButton != Microsoft.Xna.Framework.Input.ButtonState.Pressed || GamePad.GetState(Game1.playerOneIndex).Buttons.X == Microsoft.Xna.Framework.Input.ButtonState.Pressed;
    }

    public static bool didPlayerJustRightClick()
    {
      return Mouse.GetState().RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && Game1.oldMouseState.RightButton != Microsoft.Xna.Framework.Input.ButtonState.Pressed || GamePad.GetState(Game1.playerOneIndex).Buttons.A == Microsoft.Xna.Framework.Input.ButtonState.Pressed;
    }

    public static bool didPlayerJustClickAtAll()
    {
      if (!Game1.didPlayerJustLeftClick())
        return Game1.didPlayerJustRightClick();
      return true;
    }

    public static void showGlobalMessage(string message)
    {
      Game1.addHUDMessage(new HUDMessage(message, ""));
    }

    public static void globalFadeToBlack(Game1.afterFadeFunction afterFade = null, float fadeSpeed = 0.02f)
    {
      Game1.globalFade = true;
      Game1.fadeIn = false;
      Game1.afterFade = afterFade;
      Game1.globalFadeSpeed = fadeSpeed;
      Game1.fadeToBlackAlpha = 0.0f;
    }

    public static void globalFadeToClear(Game1.afterFadeFunction afterFade = null, float fadeSpeed = 0.02f)
    {
      Game1.globalFade = true;
      Game1.fadeIn = true;
      Game1.afterFade = afterFade;
      Game1.globalFadeSpeed = fadeSpeed;
      Game1.fadeToBlackAlpha = 1f;
    }

    protected override void Update(GameTime gameTime)
    {
      if (Game1.graphics.GraphicsDevice == null)
        return;
      Game1.options.reApplySetOptions();
      if (Game1.toggleFullScreen)
      {
        Game1.toggleFullscreen();
        Game1.toggleFullScreen = false;
      }
      if (Game1._newDayTask != null)
      {
        if (Game1._newDayTask.Status >= TaskStatus.RanToCompletion)
        {
          if (Game1._newDayTask.IsFaulted)
            throw Game1._newDayTask.Exception.GetBaseException();
          Game1._newDayTask = (Task) null;
          Action afterNewDayAction = Game1._afterNewDayAction;
          if (afterNewDayAction != null)
          {
            Game1._afterNewDayAction = (Action) null;
            afterNewDayAction();
          }
          Utility.CollectGarbage("", 0);
        }
        base.Update(gameTime);
      }
      else if (this.IsSaving)
      {
        IClickableMenu activeClickableMenu = Game1.activeClickableMenu;
        if (activeClickableMenu != null)
          activeClickableMenu.update(gameTime);
        base.Update(gameTime);
      }
      else
      {
        if (Game1.exitToTitle)
        {
          Game1.exitToTitle = false;
          this.CleanupReturningToTitle();
          Utility.CollectGarbage("", 0);
        }
        TimeSpan elapsedGameTime = gameTime.ElapsedGameTime;
        Game1.SetFreeCursorElapsed((float) elapsedGameTime.TotalSeconds);
        Program.sdk.Update();
        if ((Game1.paused || !this.IsActive) && (Game1.options == null || Game1.options.pauseWhenOutOfFocus || Game1.paused))
          return;
        if (Game1.quit)
          this.Exit();
        Game1.currentGameTime = gameTime;
        if ((int) Game1.gameMode != 11)
        {
          if (Game1.IsMultiplayer && (int) Game1.gameMode == 3)
          {
            if ((int) Game1.multiplayerMode == 2)
              Game1.server.receiveMessages();
            else
              Game1.client.receiveMessages();
          }
          if (this.IsActive)
            this.checkForEscapeKeys();
          Game1.updateMusic();
          Game1.updateRaindropPosition();
          if (Game1.bloom != null)
            Game1.bloom.tick(gameTime);
          if (Game1.globalFade)
          {
            if (!Game1.dialogueUp)
            {
              if (Game1.fadeIn)
              {
                Game1.fadeToBlackAlpha = Math.Max(0.0f, Game1.fadeToBlackAlpha - Game1.globalFadeSpeed);
                if ((double) Game1.fadeToBlackAlpha <= 0.0)
                {
                  Game1.globalFade = false;
                  if (Game1.afterFade != null)
                  {
                    Game1.afterFadeFunction afterFade = Game1.afterFade;
                    Game1.afterFade();
                    if (Game1.afterFade != null && Game1.afterFade.Equals((object) afterFade))
                      Game1.afterFade = (Game1.afterFadeFunction) null;
                    if (Game1.nonWarpFade)
                      Game1.fadeToBlack = false;
                  }
                }
              }
              else
              {
                Game1.fadeToBlackAlpha = Math.Min(1f, Game1.fadeToBlackAlpha + Game1.globalFadeSpeed);
                if ((double) Game1.fadeToBlackAlpha >= 1.0)
                {
                  Game1.globalFade = false;
                  if (Game1.afterFade != null)
                  {
                    Game1.afterFadeFunction afterFade = Game1.afterFade;
                    Game1.afterFade();
                    if (Game1.afterFade != null && Game1.afterFade.Equals((object) afterFade))
                      Game1.afterFade = (Game1.afterFadeFunction) null;
                    if (Game1.nonWarpFade)
                      Game1.fadeToBlack = false;
                  }
                }
              }
            }
            else if (Game1.farmEvent == null)
              this.UpdateControlInput(gameTime);
          }
          else if (Game1.pauseThenDoFunctionTimer > 0)
          {
            Game1.freezeControls = true;
            int thenDoFunctionTimer = Game1.pauseThenDoFunctionTimer;
            elapsedGameTime = gameTime.ElapsedGameTime;
            int milliseconds = elapsedGameTime.Milliseconds;
            Game1.pauseThenDoFunctionTimer = thenDoFunctionTimer - milliseconds;
            if (Game1.pauseThenDoFunctionTimer <= 0)
            {
              Game1.freezeControls = false;
              if (Game1.afterPause != null)
                Game1.afterPause();
            }
          }
          if ((int) Game1.gameMode == 3 || (int) Game1.gameMode == 2)
          {
            Farmer player = Game1.player;
            int millisecondsPlayed = (int) player.millisecondsPlayed;
            elapsedGameTime = gameTime.ElapsedGameTime;
            int milliseconds1 = elapsedGameTime.Milliseconds;
            int num1 = millisecondsPlayed + milliseconds1;
            player.millisecondsPlayed = (uint) num1;
            bool flag = true;
            if (Game1.currentMinigame != null)
            {
              if ((double) Game1.pauseTime > 0.0)
                Game1.updatePause(gameTime);
              if (Game1.fadeToBlack)
              {
                Game1.updateScreenFade(gameTime);
                if ((double) Game1.fadeToBlackAlpha >= 1.0)
                  Game1.fadeToBlack = false;
              }
              else
              {
                if (Game1.thumbstickMotionMargin > 0)
                {
                  int thumbstickMotionMargin = Game1.thumbstickMotionMargin;
                  elapsedGameTime = gameTime.ElapsedGameTime;
                  int milliseconds2 = elapsedGameTime.Milliseconds;
                  Game1.thumbstickMotionMargin = thumbstickMotionMargin - milliseconds2;
                }
                if (this.IsActive)
                {
                  KeyboardState state1 = Keyboard.GetState();
                  MouseState state2 = Mouse.GetState();
                  GamePadState state3 = GamePad.GetState(Game1.playerOneIndex);
                  foreach (Microsoft.Xna.Framework.Input.Keys pressedKey in state1.GetPressedKeys())
                  {
                    if (!Game1.oldKBState.IsKeyDown(pressedKey))
                      Game1.currentMinigame.receiveKeyPress(pressedKey);
                  }
                  if (Game1.options.gamepadControls)
                  {
                    if (Game1.currentMinigame == null)
                    {
                      Game1.oldMouseState = state2;
                      Game1.oldKBState = state1;
                      Game1.oldPadState = state3;
                      return;
                    }
                    foreach (Buttons pressedButton in Utility.getPressedButtons(state3, Game1.oldPadState))
                      Game1.currentMinigame.receiveKeyPress(Utility.mapGamePadButtonToKey(pressedButton));
                    if (Game1.currentMinigame == null)
                    {
                      Game1.oldMouseState = state2;
                      Game1.oldKBState = state1;
                      Game1.oldPadState = state3;
                      return;
                    }
                    GamePadThumbSticks thumbSticks = state3.ThumbSticks;
                    if ((double) thumbSticks.Right.Y < -0.200000002980232)
                    {
                      thumbSticks = Game1.oldPadState.ThumbSticks;
                      if ((double) thumbSticks.Right.Y >= -0.200000002980232)
                        Game1.currentMinigame.receiveKeyPress(Microsoft.Xna.Framework.Input.Keys.Down);
                    }
                    thumbSticks = state3.ThumbSticks;
                    if ((double) thumbSticks.Right.Y > 0.200000002980232)
                    {
                      thumbSticks = Game1.oldPadState.ThumbSticks;
                      if ((double) thumbSticks.Right.Y <= 0.200000002980232)
                        Game1.currentMinigame.receiveKeyPress(Microsoft.Xna.Framework.Input.Keys.Up);
                    }
                    thumbSticks = state3.ThumbSticks;
                    if ((double) thumbSticks.Right.X < -0.200000002980232)
                    {
                      thumbSticks = Game1.oldPadState.ThumbSticks;
                      if ((double) thumbSticks.Right.X >= -0.200000002980232)
                        Game1.currentMinigame.receiveKeyPress(Microsoft.Xna.Framework.Input.Keys.Left);
                    }
                    thumbSticks = state3.ThumbSticks;
                    if ((double) thumbSticks.Right.X > 0.200000002980232)
                    {
                      thumbSticks = Game1.oldPadState.ThumbSticks;
                      if ((double) thumbSticks.Right.X <= 0.200000002980232)
                        Game1.currentMinigame.receiveKeyPress(Microsoft.Xna.Framework.Input.Keys.Right);
                    }
                    thumbSticks = Game1.oldPadState.ThumbSticks;
                    if ((double) thumbSticks.Right.Y < -0.200000002980232)
                    {
                      thumbSticks = state3.ThumbSticks;
                      if ((double) thumbSticks.Right.Y >= -0.200000002980232)
                        Game1.currentMinigame.receiveKeyRelease(Microsoft.Xna.Framework.Input.Keys.Down);
                    }
                    thumbSticks = Game1.oldPadState.ThumbSticks;
                    if ((double) thumbSticks.Right.Y > 0.200000002980232)
                    {
                      thumbSticks = state3.ThumbSticks;
                      if ((double) thumbSticks.Right.Y <= 0.200000002980232)
                        Game1.currentMinigame.receiveKeyRelease(Microsoft.Xna.Framework.Input.Keys.Up);
                    }
                    thumbSticks = Game1.oldPadState.ThumbSticks;
                    if ((double) thumbSticks.Right.X < -0.200000002980232)
                    {
                      thumbSticks = state3.ThumbSticks;
                      if ((double) thumbSticks.Right.X >= -0.200000002980232)
                        Game1.currentMinigame.receiveKeyRelease(Microsoft.Xna.Framework.Input.Keys.Left);
                    }
                    thumbSticks = Game1.oldPadState.ThumbSticks;
                    if ((double) thumbSticks.Right.X > 0.200000002980232)
                    {
                      thumbSticks = state3.ThumbSticks;
                      if ((double) thumbSticks.Right.X <= 0.200000002980232)
                        Game1.currentMinigame.receiveKeyRelease(Microsoft.Xna.Framework.Input.Keys.Right);
                    }
                    if (Game1.isGamePadThumbstickInMotion(0.2) && Game1.currentMinigame != null && !Game1.currentMinigame.overrideFreeMouseMovement())
                    {
                      int mouseX = Game1.getMouseX();
                      thumbSticks = state3.ThumbSticks;
                      int num2 = (int) ((double) thumbSticks.Left.X * (double) Game1.thumbstickToMouseModifier);
                      int x = mouseX + num2;
                      int mouseY = Game1.getMouseY();
                      thumbSticks = state3.ThumbSticks;
                      int num3 = (int) ((double) thumbSticks.Left.Y * (double) Game1.thumbstickToMouseModifier);
                      int y = mouseY - num3;
                      Game1.setMousePosition(x, y);
                    }
                    else if (Game1.getMousePosition().X != Game1.getOldMouseX() || Game1.getMousePosition().Y != Game1.getOldMouseY())
                      Game1.lastCursorMotionWasMouse = true;
                  }
                  foreach (Microsoft.Xna.Framework.Input.Keys pressedKey in Game1.oldKBState.GetPressedKeys())
                  {
                    if (!state1.IsKeyDown(pressedKey) && Game1.currentMinigame != null)
                      Game1.currentMinigame.receiveKeyRelease(pressedKey);
                  }
                  if (Game1.options.gamepadControls)
                  {
                    if (Game1.currentMinigame == null)
                    {
                      Game1.oldMouseState = state2;
                      Game1.oldKBState = state1;
                      Game1.oldPadState = state3;
                      return;
                    }
                    if (state3.IsConnected && state3.IsButtonDown(Buttons.X) && !Game1.oldPadState.IsButtonDown(Buttons.X))
                      Game1.currentMinigame.receiveRightClick(Game1.getMouseX(), Game1.getMouseY(), true);
                    else if (state3.IsConnected && state3.IsButtonDown(Buttons.A) && !Game1.oldPadState.IsButtonDown(Buttons.A))
                      Game1.currentMinigame.receiveLeftClick(Game1.getMouseX(), Game1.getMouseY(), true);
                    else if (state3.IsConnected && !state3.IsButtonDown(Buttons.X) && Game1.oldPadState.IsButtonDown(Buttons.X))
                      Game1.currentMinigame.releaseRightClick(Game1.getMouseX(), Game1.getMouseY());
                    else if (state3.IsConnected && !state3.IsButtonDown(Buttons.A) && Game1.oldPadState.IsButtonDown(Buttons.A))
                      Game1.currentMinigame.releaseLeftClick(Game1.getMouseX(), Game1.getMouseY());
                    foreach (Buttons pressedButton in Utility.getPressedButtons(Game1.oldPadState, state3))
                      Game1.currentMinigame.receiveKeyRelease(Utility.mapGamePadButtonToKey(pressedButton));
                    if (state3.IsConnected && state3.IsButtonDown(Buttons.A) && Game1.currentMinigame != null)
                      Game1.currentMinigame.leftClickHeld(0, 0);
                  }
                  if (Game1.currentMinigame == null)
                  {
                    Game1.oldMouseState = state2;
                    Game1.oldKBState = state1;
                    Game1.oldPadState = state3;
                    return;
                  }
                  if (Game1.currentMinigame != null && state2.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && Game1.oldMouseState.LeftButton != Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    Game1.currentMinigame.receiveLeftClick(Game1.getMouseX(), Game1.getMouseY(), true);
                  if (Game1.currentMinigame != null && state2.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && Game1.oldMouseState.RightButton != Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    Game1.currentMinigame.receiveRightClick(Game1.getMouseX(), Game1.getMouseY(), true);
                  if (Game1.currentMinigame != null && state2.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && Game1.oldMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    Game1.currentMinigame.releaseLeftClick(Game1.getMouseX(), Game1.getMouseY());
                  if (Game1.currentMinigame != null && state2.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Released && Game1.oldMouseState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    Game1.currentMinigame.releaseLeftClick(Game1.getMouseX(), Game1.getMouseY());
                  if (Game1.currentMinigame != null && state2.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && Game1.oldMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    Game1.currentMinigame.leftClickHeld(Game1.getMouseX(), Game1.getMouseY());
                  Game1.oldMouseState = state2;
                  Game1.oldKBState = state1;
                  Game1.oldPadState = state3;
                }
                if (Game1.currentMinigame != null && Game1.currentMinigame.tick(gameTime))
                {
                  Game1.currentMinigame.unload();
                  Game1.currentMinigame = (IMinigame) null;
                  Game1.fadeIn = true;
                  Game1.fadeToBlackAlpha = 1f;
                  return;
                }
              }
              flag = Game1.IsMultiplayer;
            }
            else if (Game1.farmEvent != null && Game1.farmEvent.tickUpdate(gameTime))
            {
              Game1.farmEvent.makeChangesToLocation();
              Game1.timeOfDay = 600;
              Game1.UpdateOther(gameTime);
              Game1.displayHUD = true;
              Game1.farmEvent = (FarmEvent) null;
              Game1.currentLocation = Game1.getLocationFromName("FarmHouse");
              Game1.player.position = Utility.PointToVector2(Utility.getHomeOfFarmer(Game1.player).getBedSpot()) * (float) Game1.tileSize;
              Game1.player.position.X -= (float) Game1.tileSize;
              Game1.changeMusicTrack("none");
              Game1.currentLocation.resetForPlayerEntry();
              Game1.player.forceCanMove();
              Game1.freezeControls = false;
              Game1.displayFarmer = true;
              Game1.outdoorLight = Color.White;
              Game1.viewportFreeze = false;
              Game1.fadeToBlackAlpha = 0.0f;
              Game1.fadeToBlack = false;
              Game1.globalFadeToClear((Game1.afterFadeFunction) null, 0.02f);
              Game1.player.mailForTomorrow.Clear();
              Game1.showEndOfNightStuff();
            }
            if (flag)
            {
              if (Game1.endOfNightMenus.Count > 0 && Game1.activeClickableMenu == null)
              {
                Game1.activeClickableMenu = Game1.endOfNightMenus.Pop();
                if (Game1.activeClickableMenu != null && Game1.options.SnappyMenus)
                  Game1.activeClickableMenu.snapToDefaultClickableComponent();
              }
              if (Game1.activeClickableMenu != null)
              {
                Game1.updateActiveMenu(gameTime);
              }
              else
              {
                if ((double) Game1.pauseTime > 0.0)
                  Game1.updatePause(gameTime);
                if (!Game1.globalFade && !Game1.freezeControls && (Game1.activeClickableMenu == null && this.IsActive))
                  this.UpdateControlInput(gameTime);
              }
              if (Game1.showingEndOfNightStuff && Game1.endOfNightMenus.Count == 0 && Game1.activeClickableMenu == null)
              {
                Game1.showingEndOfNightStuff = false;
                Game1.globalFadeToClear(new Game1.afterFadeFunction(Game1.playMorningSong), 0.02f);
              }
              if (!Game1.showingEndOfNightStuff)
              {
                if (Game1.IsMultiplayer || Game1.activeClickableMenu == null && Game1.currentMinigame == null)
                  Game1.UpdateGameClock(gameTime);
                this.UpdateCharacters(gameTime);
                this.UpdateLocations(gameTime);
                Game1.UpdateViewPort(false, this.getViewportCenter());
              }
              Game1.UpdateOther(gameTime);
              if (Game1.messagePause)
              {
                KeyboardState state1 = Keyboard.GetState();
                MouseState state2 = Mouse.GetState();
                GamePadState state3 = GamePad.GetState(Game1.playerOneIndex);
                if (Game1.isOneOfTheseKeysDown(state1, Game1.options.actionButton) && !Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.actionButton))
                  Game1.pressActionButton(state1, state2, state3);
                Game1.oldKBState = state1;
                Game1.oldPadState = state3;
              }
            }
          }
          else
          {
            if (Game1.thumbstickMotionMargin > 0)
            {
              int thumbstickMotionMargin = Game1.thumbstickMotionMargin;
              elapsedGameTime = gameTime.ElapsedGameTime;
              int milliseconds = elapsedGameTime.Milliseconds;
              Game1.thumbstickMotionMargin = thumbstickMotionMargin - milliseconds;
            }
            this.UpdateTitleScreen(gameTime);
            if (Game1.activeClickableMenu != null)
              Game1.updateActiveMenu(gameTime);
            if ((int) Game1.gameMode == 10)
              Game1.UpdateOther(gameTime);
          }
          if (Game1.audioEngine != null)
            Game1.audioEngine.Update();
          if ((int) Game1.multiplayerMode == 2 && (int) Game1.gameMode == 3)
            Game1.server.sendMessages(gameTime);
        }
        base.Update(gameTime);
      }
    }

    public static bool isDarkOut()
    {
      return Game1.timeOfDay >= Game1.getTrulyDarkTime();
    }

    public static bool isStartingToGetDarkOut()
    {
      return Game1.timeOfDay >= Game1.getStartingToGetDarkTime();
    }

    public static int getStartingToGetDarkTime()
    {
      string currentSeason = Game1.currentSeason;
      if (currentSeason == "spring" || currentSeason == "summer")
        return 1800;
      if (currentSeason == "fall")
        return 1700;
      return currentSeason == "winter" ? 1600 : 1800;
    }

    public static int getModeratelyDarkTime()
    {
      return (Game1.getTrulyDarkTime() + Game1.getStartingToGetDarkTime()) / 2;
    }

    public static int getTrulyDarkTime()
    {
      return Game1.getStartingToGetDarkTime() + 200;
    }

    public static void playMorningSong()
    {
      if (Game1.isRaining || Game1.isLightning || (Game1.eventUp || Game1.dayOfMonth <= 0) || Game1.currentLocation.Name.Equals("Desert"))
        return;
      DelayedAction.playMusicAfterDelay(Game1.currentSeason + (object) Math.Max(1, Game1.currentSongIndex), 500);
    }

    private Point getViewportCenter()
    {
      if ((double) Game1.viewportTarget.X != (double) int.MinValue)
      {
        if ((double) Math.Abs((float) Game1.viewportCenter.X - Game1.viewportTarget.X) > (double) Game1.viewportSpeed || (double) Math.Abs((float) Game1.viewportCenter.Y - Game1.viewportTarget.Y) > (double) Game1.viewportSpeed)
        {
          Vector2 velocityTowardPoint = Utility.getVelocityTowardPoint(Game1.viewportCenter, Game1.viewportTarget, Game1.viewportSpeed);
          Game1.viewportCenter.X += (int) Math.Round((double) velocityTowardPoint.X);
          Game1.viewportCenter.Y += (int) Math.Round((double) velocityTowardPoint.Y);
        }
        else
        {
          if (Game1.viewportReachedTarget != null)
          {
            Game1.viewportReachedTarget();
            Game1.viewportReachedTarget = (Game1.afterFadeFunction) null;
          }
          Game1.viewportHold -= Game1.currentGameTime.ElapsedGameTime.Milliseconds;
          if (Game1.viewportHold <= 0)
          {
            Game1.viewportTarget = new Vector2((float) int.MinValue, (float) int.MinValue);
            if (Game1.afterViewport != null)
              Game1.afterViewport();
          }
        }
        return Game1.viewportCenter;
      }
      Game1.viewportCenter.X = Game1.player.getStandingX();
      Game1.viewportCenter.Y = Game1.player.getStandingY();
      return Game1.viewportCenter;
    }

    public static void afterFadeReturnViewportToPlayer()
    {
      Game1.viewportTarget = new Vector2((float) int.MinValue, (float) int.MinValue);
      Game1.viewportHold = 0;
      Game1.viewportFreeze = false;
      Game1.viewportCenter.X = Game1.player.getStandingX();
      Game1.viewportCenter.Y = Game1.player.getStandingY();
      Game1.globalFadeToClear((Game1.afterFadeFunction) null, 0.02f);
    }

    public static bool isViewportOnCustomPath()
    {
      return (double) Game1.viewportTarget.X != (double) int.MinValue;
    }

    public static void moveViewportTo(Vector2 target, float speed, int holdTimer = 0, Game1.afterFadeFunction reachedTarget = null, Game1.afterFadeFunction endFunction = null)
    {
      Game1.viewportTarget = target;
      Game1.viewportSpeed = speed;
      Game1.viewportHold = holdTimer;
      Game1.afterViewport = endFunction;
      Game1.viewportReachedTarget = reachedTarget;
    }

    public static Farm getFarm()
    {
      return Game1.getLocationFromName("Farm") as Farm;
    }

    public static void setMousePosition(int x, int y)
    {
      Mouse.SetPosition((int) ((double) x * (double) Game1.options.zoomLevel), (int) ((double) y * (double) Game1.options.zoomLevel));
      Game1.lastCursorMotionWasMouse = false;
    }

    public static void setMousePosition(Point position)
    {
      Mouse.SetPosition((int) ((double) position.X * (double) Game1.options.zoomLevel), (int) ((double) position.Y * (double) Game1.options.zoomLevel));
      Game1.lastCursorMotionWasMouse = false;
    }

    public static Point getMousePosition()
    {
      return new Point(Game1.getMouseX(), Game1.getMouseY());
    }

    private static float thumbstickToMouseModifier
    {
      get
      {
        if (Game1._cursorSpeedDirty)
          Game1.ComputeCursorSpeed();
        return (float) ((double) Game1._cursorSpeed / 720.0 * (double) Game1.viewport.Height * Game1.currentGameTime.ElapsedGameTime.TotalSeconds);
      }
    }

    private static void ComputeCursorSpeed()
    {
      Game1._cursorSpeedDirty = false;
      GamePadState state = GamePad.GetState(Game1.playerOneIndex);
      float num1 = 0.9f;
      bool flag = false;
      Vector2 vector2 = state.ThumbSticks.Left;
      double num2 = (double) vector2.Length();
      vector2 = state.ThumbSticks.Right;
      float num3 = vector2.Length();
      double num4 = (double) num1;
      if (num2 > num4 || (double) num3 > (double) num1)
        flag = true;
      float min = 0.7f;
      float max = 2f;
      float num5 = 1f;
      if (Game1._cursorDragEnabled)
      {
        min = 0.5f;
        max = 2f;
        num5 = 1f;
      }
      if (!flag)
        num5 = -5f;
      if (Game1._cursorDragPrevEnabled != Game1._cursorDragEnabled)
        Game1._cursorSpeedScale *= 0.5f;
      Game1._cursorDragPrevEnabled = Game1._cursorDragEnabled;
      Game1._cursorSpeedScale += Game1._cursorUpdateElapsedSec * num5;
      Game1._cursorSpeedScale = MathHelper.Clamp(Game1._cursorSpeedScale, min, max);
      double num6 = 16.0 / Game1.game1.TargetElapsedTime.TotalSeconds * (double) Game1._cursorSpeedScale;
      double cursorSpeed = (double) Game1._cursorSpeed;
      float num7 = (float) (num6 - cursorSpeed);
      Game1._cursorSpeed = (float) num6;
      Game1._cursorUpdateElapsedSec = 0.0f;
      if (!Game1.debugMode)
        return;
      Console.WriteLine("_cursorSpeed={0}, _cursorSpeedScale={1}, deltaSpeed={2}", (object) Game1._cursorSpeed.ToString("0.0"), (object) Game1._cursorSpeedScale.ToString("0.0"), (object) num7.ToString("0.0"));
    }

    private static void SetFreeCursorElapsed(float elapsedSec)
    {
      if ((double) elapsedSec == (double) Game1._cursorUpdateElapsedSec)
        return;
      Game1._cursorUpdateElapsedSec = elapsedSec;
      Game1._cursorSpeedDirty = true;
    }

    public static void ResetFreeCursorDrag()
    {
      if (Game1._cursorDragEnabled)
        Game1._cursorSpeedDirty = true;
      Game1._cursorDragEnabled = false;
    }

    public static void SetFreeCursorDrag()
    {
      if (!Game1._cursorDragEnabled)
        Game1._cursorSpeedDirty = true;
      Game1._cursorDragEnabled = true;
    }

    public static void updateActiveMenu(GameTime gameTime)
    {
      if (!Program.gamePtr.IsActive)
        return;
      MouseState state1 = Mouse.GetState();
      KeyboardState state2 = Keyboard.GetState();
      GamePadState state3 = GamePad.GetState(Game1.playerOneIndex);
      if (state3.IsConnected && !Game1.options.gamepadControls)
      {
        Game1.options.gamepadControls = true;
        Game1.showGlobalMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2574"));
        if (Game1.activeClickableMenu != null)
          Game1.activeClickableMenu.setUpForGamePadMode();
      }
      else if (!state3.IsConnected && Game1.options.gamepadControls)
      {
        Game1.options.gamepadControls = false;
        Game1.showGlobalMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2575"));
        if (Game1.activeClickableMenu == null)
          Game1.activeClickableMenu = (IClickableMenu) new GameMenu();
      }
      if (Game1.CurrentEvent != null && (state1.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && Game1.oldMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released || Game1.options.gamepadControls && state3.IsButtonDown(Buttons.A) && Game1.oldPadState.IsButtonUp(Buttons.A)))
      {
        Game1.CurrentEvent.receiveMouseClick(Game1.getMouseX(), Game1.getMouseY());
        if (Game1.CurrentEvent != null && Game1.CurrentEvent.skipped)
        {
          Game1.oldMouseState = state1;
          Game1.oldKBState = state2;
          Game1.oldPadState = state3;
          return;
        }
      }
      GamePadThumbSticks thumbSticks;
      ButtonCollection buttonCollection;
      if (state3.IsConnected && Game1.activeClickableMenu != null)
      {
        if (Game1.getMousePosition().Equals(Point.Zero) && Game1.activeClickableMenu.autoCenterMouseCursorForGamepad())
          Game1.setMousePosition(Game1.graphics.GraphicsDevice.Viewport.Width / 2, Game1.graphics.GraphicsDevice.Viewport.Height / 2);
        if (Game1.isGamePadThumbstickInMotion(0.2) && (!Game1.options.snappyMenus || Game1.activeClickableMenu.overrideSnappyMenuCursorMovementBan()))
        {
          int x = (int) ((double) state1.X + (double) state3.ThumbSticks.Left.X * (double) Game1.thumbstickToMouseModifier);
          double y1 = (double) state1.Y;
          thumbSticks = state3.ThumbSticks;
          double num = (double) thumbSticks.Left.Y * (double) Game1.thumbstickToMouseModifier;
          int y2 = (int) (y1 - num);
          Mouse.SetPosition(x, y2);
          Game1.lastCursorMotionWasMouse = false;
        }
        if (Game1.activeClickableMenu != null)
        {
          foreach (Buttons pressedButton in Utility.getPressedButtons(state3, Game1.oldPadState))
          {
            Game1.activeClickableMenu.receiveGamePadButton(pressedButton);
            if (Game1.activeClickableMenu == null)
              break;
          }
          buttonCollection = Utility.getHeldButtons(state3);
          foreach (Buttons b in buttonCollection)
          {
            if (Game1.activeClickableMenu != null)
              Game1.activeClickableMenu.gamePadButtonHeld(b);
            if (Game1.activeClickableMenu == null)
              break;
          }
        }
      }
      if ((Game1.getMouseX() != Game1.getOldMouseX() || Game1.getMouseY() != Game1.getOldMouseY()) && !Game1.isGamePadThumbstickInMotion(0.2))
        Game1.lastCursorMotionWasMouse = true;
      Game1.ResetFreeCursorDrag();
      if (Game1.activeClickableMenu != null)
        Game1.activeClickableMenu.performHoverAction(Game1.getMouseX(), Game1.getMouseY());
      if (Game1.activeClickableMenu != null)
        Game1.activeClickableMenu.update(gameTime);
      if (Game1.activeClickableMenu != null && state1.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && Game1.oldMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
        Game1.activeClickableMenu.receiveLeftClick(Game1.getMouseX(), Game1.getMouseY(), true);
      else if (Game1.activeClickableMenu != null && state1.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && (Game1.oldMouseState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Released || (double) Game1.mouseClickPolling > 650.0 && !(Game1.activeClickableMenu is DialogueBox)))
      {
        Game1.activeClickableMenu.receiveRightClick(Game1.getMouseX(), Game1.getMouseY(), true);
        if ((double) Game1.mouseClickPolling > 650.0)
          Game1.mouseClickPolling = 600;
        if (Game1.activeClickableMenu == null)
          Game1.rightClickPolling = 500;
      }
      if (state1.ScrollWheelValue != Game1.oldMouseState.ScrollWheelValue && Game1.activeClickableMenu != null)
        Game1.activeClickableMenu.receiveScrollWheelAction(state1.ScrollWheelValue - Game1.oldMouseState.ScrollWheelValue);
      if (Game1.options.gamepadControls && Game1.activeClickableMenu != null)
      {
        Game1.thumbstickPollingTimer -= Game1.currentGameTime.ElapsedGameTime.Milliseconds;
        if (Game1.thumbstickPollingTimer <= 0)
        {
          thumbSticks = state3.ThumbSticks;
          if ((double) thumbSticks.Right.Y > 0.200000002980232)
          {
            Game1.activeClickableMenu.receiveScrollWheelAction(1);
          }
          else
          {
            thumbSticks = state3.ThumbSticks;
            if ((double) thumbSticks.Right.Y < -0.200000002980232)
              Game1.activeClickableMenu.receiveScrollWheelAction(-1);
          }
        }
        if (Game1.thumbstickPollingTimer <= 0)
        {
          int num1 = 220;
          thumbSticks = state3.ThumbSticks;
          int num2 = (int) ((double) Math.Abs(thumbSticks.Right.Y) * 170.0);
          Game1.thumbstickPollingTimer = num1 - num2;
        }
        thumbSticks = state3.ThumbSticks;
        if ((double) Math.Abs(thumbSticks.Right.Y) < 0.200000002980232)
          Game1.thumbstickPollingTimer = 0;
      }
      if (Game1.activeClickableMenu != null && state1.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && Game1.oldMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
        Game1.activeClickableMenu.releaseLeftClick(Game1.getMouseX(), Game1.getMouseY());
      else if (Game1.activeClickableMenu != null && state1.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && Game1.oldMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
        Game1.activeClickableMenu.leftClickHeld(Game1.getMouseX(), Game1.getMouseY());
      foreach (Microsoft.Xna.Framework.Input.Keys pressedKey in state2.GetPressedKeys())
      {
        if (Game1.activeClickableMenu != null && !((IEnumerable<Microsoft.Xna.Framework.Input.Keys>) Game1.oldKBState.GetPressedKeys()).Contains<Microsoft.Xna.Framework.Input.Keys>(pressedKey))
          Game1.activeClickableMenu.receiveKeyPress(pressedKey);
      }
      if (!Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.moveUpButton))
      {
        if (Game1.options.snappyMenus && Game1.options.gamepadControls)
        {
          thumbSticks = state3.ThumbSticks;
          double num = (double) Math.Abs(thumbSticks.Left.X);
          thumbSticks = state3.ThumbSticks;
          double y = (double) thumbSticks.Left.Y;
          if (num < y || state3.IsButtonDown(Buttons.DPadUp))
            goto label_61;
        }
        if (!Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.moveRightButton))
        {
          if (Game1.options.snappyMenus && Game1.options.gamepadControls)
          {
            thumbSticks = state3.ThumbSticks;
            double x = (double) thumbSticks.Left.X;
            thumbSticks = state3.ThumbSticks;
            double num = (double) Math.Abs(thumbSticks.Left.Y);
            if (x > num || state3.IsButtonDown(Buttons.DPadRight))
              goto label_65;
          }
          if (!Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.moveDownButton))
          {
            if (Game1.options.snappyMenus && Game1.options.gamepadControls)
            {
              thumbSticks = state3.ThumbSticks;
              double num1 = (double) Math.Abs(thumbSticks.Left.X);
              thumbSticks = state3.ThumbSticks;
              double num2 = (double) Math.Abs(thumbSticks.Left.Y);
              if (num1 < num2 || state3.IsButtonDown(Buttons.DPadDown))
                goto label_69;
            }
            if (!Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.moveLeftButton))
            {
              if (Game1.options.snappyMenus && Game1.options.gamepadControls)
              {
                thumbSticks = state3.ThumbSticks;
                double num1 = (double) Math.Abs(thumbSticks.Left.X);
                thumbSticks = state3.ThumbSticks;
                double num2 = (double) Math.Abs(thumbSticks.Left.Y);
                if (num1 <= num2 && !state3.IsButtonDown(Buttons.DPadLeft))
                  goto label_74;
              }
              else
                goto label_74;
            }
            Game1.directionKeyPolling[3] -= Game1.currentGameTime.ElapsedGameTime.Milliseconds;
            goto label_74;
          }
label_69:
          Game1.directionKeyPolling[2] -= Game1.currentGameTime.ElapsedGameTime.Milliseconds;
          goto label_74;
        }
label_65:
        Game1.directionKeyPolling[1] -= Game1.currentGameTime.ElapsedGameTime.Milliseconds;
        goto label_74;
      }
label_61:
      Game1.directionKeyPolling[0] -= Game1.currentGameTime.ElapsedGameTime.Milliseconds;
label_74:
      if (Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.moveUpButton))
      {
        if (Game1.options.snappyMenus && Game1.options.gamepadControls)
        {
          thumbSticks = state3.ThumbSticks;
          if ((double) thumbSticks.Left.Y >= 0.1 || !state3.IsButtonUp(Buttons.DPadUp))
            goto label_78;
        }
        Game1.directionKeyPolling[0] = 250;
      }
label_78:
      if (Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.moveRightButton))
      {
        if (Game1.options.snappyMenus && Game1.options.gamepadControls)
        {
          thumbSticks = state3.ThumbSticks;
          if ((double) thumbSticks.Left.X >= 0.1 || !state3.IsButtonUp(Buttons.DPadRight))
            goto label_82;
        }
        Game1.directionKeyPolling[1] = 250;
      }
label_82:
      if (Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.moveDownButton))
      {
        if (Game1.options.snappyMenus && Game1.options.gamepadControls)
        {
          thumbSticks = state3.ThumbSticks;
          if ((double) thumbSticks.Left.Y <= -0.1 || !state3.IsButtonUp(Buttons.DPadDown))
            goto label_86;
        }
        Game1.directionKeyPolling[2] = 250;
      }
label_86:
      if (Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.moveLeftButton))
      {
        if (Game1.options.snappyMenus && Game1.options.gamepadControls)
        {
          thumbSticks = state3.ThumbSticks;
          if ((double) thumbSticks.Left.X <= -0.1 || !state3.IsButtonUp(Buttons.DPadLeft))
            goto label_90;
        }
        Game1.directionKeyPolling[3] = 250;
      }
label_90:
      if (Game1.directionKeyPolling[0] <= 0 && Game1.activeClickableMenu != null)
      {
        Game1.activeClickableMenu.receiveKeyPress(Game1.options.getFirstKeyboardKeyFromInputButtonList(Game1.options.moveUpButton));
        Game1.directionKeyPolling[0] = 70;
      }
      if (Game1.directionKeyPolling[1] <= 0 && Game1.activeClickableMenu != null)
      {
        Game1.activeClickableMenu.receiveKeyPress(Game1.options.getFirstKeyboardKeyFromInputButtonList(Game1.options.moveRightButton));
        Game1.directionKeyPolling[1] = 70;
      }
      if (Game1.directionKeyPolling[2] <= 0 && Game1.activeClickableMenu != null)
      {
        Game1.activeClickableMenu.receiveKeyPress(Game1.options.getFirstKeyboardKeyFromInputButtonList(Game1.options.moveDownButton));
        Game1.directionKeyPolling[2] = 70;
      }
      if (Game1.directionKeyPolling[3] <= 0 && Game1.activeClickableMenu != null)
      {
        Game1.activeClickableMenu.receiveKeyPress(Game1.options.getFirstKeyboardKeyFromInputButtonList(Game1.options.moveLeftButton));
        Game1.directionKeyPolling[3] = 70;
      }
      if (Game1.options.gamepadControls && state3.IsConnected && Game1.activeClickableMenu != null)
      {
        if (!Game1.activeClickableMenu.areGamePadControlsImplemented() && state3.IsButtonDown(Buttons.A) && (!Game1.oldPadState.IsButtonDown(Buttons.A) || (double) Game1.gamePadAButtonPolling > 650.0 && !(Game1.activeClickableMenu is DialogueBox)))
        {
          Game1.activeClickableMenu.receiveLeftClick(Game1.getMousePosition().X, Game1.getMousePosition().Y, true);
          if ((double) Game1.gamePadAButtonPolling > 650.0)
            Game1.gamePadAButtonPolling = 600;
        }
        else if (!Game1.activeClickableMenu.areGamePadControlsImplemented() && !state3.IsButtonDown(Buttons.A) && Game1.oldPadState.IsButtonDown(Buttons.A))
          Game1.activeClickableMenu.releaseLeftClick(Game1.getMousePosition().X, Game1.getMousePosition().Y);
        else if (!Game1.activeClickableMenu.areGamePadControlsImplemented() && state3.IsButtonDown(Buttons.X) && (!Game1.oldPadState.IsButtonDown(Buttons.X) || (double) Game1.gamePadXButtonPolling > 650.0 && !(Game1.activeClickableMenu is DialogueBox)))
        {
          Game1.activeClickableMenu.receiveRightClick(Game1.getMousePosition().X, Game1.getMousePosition().Y, true);
          if ((double) Game1.gamePadXButtonPolling > 650.0)
            Game1.gamePadXButtonPolling = 600;
        }
        buttonCollection = Utility.getPressedButtons(state3, Game1.oldPadState);
        foreach (Buttons b in buttonCollection)
        {
          if (Game1.activeClickableMenu != null)
            Game1.activeClickableMenu.receiveKeyPress(Utility.mapGamePadButtonToKey(b));
          else
            break;
        }
        if (Game1.activeClickableMenu != null && !Game1.activeClickableMenu.areGamePadControlsImplemented() && (state3.IsButtonDown(Buttons.A) && Game1.oldPadState.IsButtonDown(Buttons.A)))
          Game1.activeClickableMenu.leftClickHeld(Game1.getMousePosition().X, Game1.getMousePosition().Y);
        if (state3.IsButtonDown(Buttons.X))
          Game1.gamePadXButtonPolling += gameTime.ElapsedGameTime.Milliseconds;
        else
          Game1.gamePadXButtonPolling = 0;
        if (state3.IsButtonDown(Buttons.A))
          Game1.gamePadAButtonPolling += gameTime.ElapsedGameTime.Milliseconds;
        else
          Game1.gamePadAButtonPolling = 0;
      }
      if (state1.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
        Game1.mouseClickPolling += gameTime.ElapsedGameTime.Milliseconds;
      else
        Game1.mouseClickPolling = 0;
      Game1.oldMouseState = state1;
      Game1.oldKBState = state2;
      Game1.oldPadState = state3;
    }

    public static string DateCompiled()
    {
      Version version = Assembly.GetExecutingAssembly().GetName().Version;
      return version.Major.ToString() + "." + (object) version.Minor + "." + (object) version.Build + "." + (object) version.Revision;
    }

    public static void updatePause(GameTime gameTime)
    {
      Game1.pauseTime -= (float) gameTime.ElapsedGameTime.Milliseconds;
      if (Game1.player.isCrafting && Game1.random.NextDouble() < 0.007)
        Game1.playSound("crafting");
      if ((double) Game1.pauseTime > 0.0)
        return;
      if (Game1.currentObjectDialogue.Count == 0)
        Game1.messagePause = false;
      Game1.pauseTime = 0.0f;
      if (Game1.messageAfterPause != null && !Game1.messageAfterPause.Equals(""))
      {
        Game1.player.isCrafting = false;
        Game1.drawObjectDialogue(Game1.messageAfterPause);
        Game1.messageAfterPause = "";
        if (Game1.player.ActiveObject != null)
        {
          int num = Game1.player.ActiveObject.bigCraftable ? 1 : 0;
        }
        if (Game1.killScreen)
        {
          Game1.killScreen = false;
          Game1.player.health = 10;
        }
      }
      else if (Game1.killScreen)
      {
        Game1.screenGlow = false;
        if (Game1.currentLocation.Name.Equals("UndergroundMine") && Game1.mine.getMineArea(-1) != 121)
          Game1.warpFarmer("Mine", 22, 9, false);
        else
          Game1.warpFarmer("Hospital", 20, 12, false);
      }
      Game1.progressBar = false;
      if (Game1.currentLocation.currentEvent == null)
        return;
      ++Game1.currentLocation.currentEvent.CurrentCommand;
    }

    public static void initializeMultiplayerServer()
    {
      Game1.player.currentLocation = Game1.getLocationFromName("FarmHouse");
      Game1.server.initializeConnection();
    }

    public static void initializeMultiplayerClient()
    {
      Game1.client.receiveMessages();
      foreach (KeyValuePair<long, Farmer> otherFarmer in Game1.otherFarmers)
      {
        otherFarmer.Value.currentLocation = Game1.getLocationFromName(otherFarmer.Value._tmpLocationName);
        otherFarmer.Value.currentLocation.farmers.Add(otherFarmer.Value);
      }
    }

    public static void toggleNonBorderlessWindowedFullscreen()
    {
      int preferredResolutionX = Game1.options.preferredResolutionX;
      int preferredResolutionY = Game1.options.preferredResolutionY;
      Game1.graphics.PreferredBackBufferWidth = preferredResolutionX;
      Game1.graphics.PreferredBackBufferHeight = preferredResolutionY;
      Game1.graphics.ToggleFullScreen();
      Game1.graphics.ApplyChanges();
      Game1.updateViewportForScreenSizeChange(true, Game1.graphics.PreferredBackBufferWidth, Game1.graphics.PreferredBackBufferHeight);
      Form form = Control.FromHandle(Program.gamePtr.Window.Handle).FindForm();
      int num1 = Game1.isFullscreen ? 0 : 4;
      form.FormBorderStyle = (FormBorderStyle) num1;
      int num2 = 0;
      form.WindowState = (FormWindowState) num2;
      Program.gamePtr.Window_ClientSizeChanged((object) null, (EventArgs) null);
    }

    public static void toggleFullscreen()
    {
      Form form = Control.FromHandle(Program.gamePtr.Window.Handle).FindForm();
      if (Game1.options.windowedBorderlessFullscreen || form.WindowState == FormWindowState.Maximized)
      {
        if (form.WindowState != FormWindowState.Maximized || form.FormBorderStyle != FormBorderStyle.None)
        {
          form.FormBorderStyle = FormBorderStyle.None;
          form.WindowState = FormWindowState.Maximized;
        }
        else
        {
          form.FormBorderStyle = FormBorderStyle.Sizable;
          form.WindowState = FormWindowState.Normal;
          if (Game1.options.fullscreen && !Game1.options.windowedBorderlessFullscreen)
          {
            Program.gamePtr.Window_ClientSizeChanged((object) null, (EventArgs) null);
            Game1.graphics.PreferredBackBufferWidth = Game1.options.preferredResolutionX;
            Game1.graphics.PreferredBackBufferHeight = Game1.options.preferredResolutionY;
            Game1.toggleNonBorderlessWindowedFullscreen();
            return;
          }
        }
      }
      else
        Game1.toggleNonBorderlessWindowedFullscreen();
      Program.gamePtr.Window_ClientSizeChanged((object) null, (EventArgs) null);
    }

    public static bool isFullscreen
    {
      get
      {
        if (Game1.graphics.IsFullScreen)
          return true;
        Form form = Control.FromHandle(Program.gamePtr.Window.Handle).FindForm();
        if (form.WindowState == FormWindowState.Maximized)
          return form.FormBorderStyle == FormBorderStyle.None;
        return false;
      }
    }

    private void checkForEscapeKeys()
    {
      KeyboardState state = Keyboard.GetState();
      if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftAlt) && state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter) && (Game1.oldKBState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.LeftAlt) || Game1.oldKBState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Enter)))
      {
        if (Game1.options.isCurrentlyFullscreen() || Game1.options.isCurrentlyWindowedBorderless())
          Game1.options.setWindowedOption(1);
        else
          Game1.options.setWindowedOption(0);
        if (Game1.activeClickableMenu != null && Game1.activeClickableMenu is GameMenu)
        {
          Game1.exitActiveMenu();
          Game1.activeClickableMenu = (IClickableMenu) new GameMenu(6, -1);
        }
      }
      if (!Game1.player.UsingTool && !Game1.freezeControls || (!state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightShift) || !state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.R)) || !state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Delete))
        return;
      Game1.freezeControls = false;
      Game1.player.forceCanMove();
      Game1.player.completelyStopAnimatingOrDoingAction();
      Game1.player.usingTool = false;
    }

    public static bool IsPressEvent(ref KeyboardState state, Microsoft.Xna.Framework.Input.Keys key)
    {
      return state.IsKeyDown(key) && !Game1.oldKBState.IsKeyUp(key);
    }

    public static bool IsPressEvent(ref GamePadState state, Buttons btn)
    {
      return state.IsConnected && state.IsButtonDown(btn) && (!Game1.oldPadState.IsConnected || Game1.oldPadState.IsButtonUp(btn));
    }

    public static bool isOneOfTheseKeysDown(KeyboardState state, InputButton[] keys)
    {
      foreach (InputButton key in keys)
      {
        if (key.key != Microsoft.Xna.Framework.Input.Keys.None && state.IsKeyDown(key.key))
          return true;
      }
      return false;
    }

    public static bool areAllOfTheseKeysUp(KeyboardState state, InputButton[] keys)
    {
      foreach (InputButton key in keys)
      {
        if (key.key != Microsoft.Xna.Framework.Input.Keys.None && !state.IsKeyUp(key.key))
          return false;
      }
      return true;
    }

    private void UpdateTitleScreen(GameTime time)
    {
      if (Game1.quit)
      {
        this.Exit();
        Game1.changeMusicTrack("none");
      }
      if ((int) Game1.gameMode == 6)
      {
        Game1.nextMusicTrack = "none";
        if (Game1.currentLoader.MoveNext())
          return;
        if ((int) Game1.gameMode == 3)
        {
          Game1.setGameMode((byte) 3);
          Game1.fadeIn = true;
          Game1.fadeToBlackAlpha = 0.99f;
        }
        else
          Game1.ExitToTitle();
      }
      else if ((int) Game1.gameMode == 7)
        Game1.currentLoader.MoveNext();
      else if ((int) Game1.gameMode == 8)
      {
        Game1.pauseAccumulator -= (float) time.ElapsedGameTime.Milliseconds;
        if ((double) Game1.pauseAccumulator > 0.0)
          return;
        Game1.pauseAccumulator = 0.0f;
        Game1.setGameMode((byte) 3);
        if (Game1.currentObjectDialogue.Count <= 0)
          return;
        Game1.messagePause = true;
        Game1.pauseTime = 1E+10f;
        Game1.fadeToBlackAlpha = 1f;
        Game1.player.CanMove = false;
      }
      else
      {
        if ((double) Game1.fadeToBlackAlpha < 1.0 && Game1.fadeIn)
          Game1.fadeToBlackAlpha += 0.02f;
        else if ((double) Game1.fadeToBlackAlpha > 0.0 && Game1.fadeToBlack)
          Game1.fadeToBlackAlpha -= 0.02f;
        if ((double) Game1.pauseTime > 0.0)
          Game1.pauseTime = Math.Max(0.0f, Game1.pauseTime - (float) time.ElapsedGameTime.Milliseconds);
        if ((int) Game1.gameMode == 0 && (double) Game1.fadeToBlackAlpha >= 0.98)
        {
          double fadeToBlackAlpha = (double) Game1.fadeToBlackAlpha;
        }
        if ((double) Game1.fadeToBlackAlpha >= 1.0)
        {
          if ((int) Game1.gameMode == 4 && !Game1.fadeToBlack)
          {
            Game1.fadeIn = false;
            Game1.fadeToBlack = true;
            Game1.fadeToBlackAlpha = 2.5f;
          }
          else if ((int) Game1.gameMode == 0 && Game1.currentSong == null && (Game1.soundBank != null && (double) Game1.pauseTime <= 0.0) && Game1.soundBank != null)
          {
            Game1.currentSong = Game1.soundBank.GetCue("spring_day_ambient");
            Game1.currentSong.Play();
          }
          if ((int) Game1.gameMode != 0 || Game1.activeClickableMenu != null || Game1.quit)
            return;
          Game1.activeClickableMenu = (IClickableMenu) new TitleMenu();
        }
        else
        {
          if ((double) Game1.fadeToBlackAlpha > 0.0)
            return;
          if ((int) Game1.gameMode == 4 && Game1.fadeToBlack)
          {
            Game1.fadeIn = true;
            Game1.fadeToBlack = false;
            Game1.setGameMode((byte) 0);
            Game1.pauseTime = 2000f;
          }
          else
          {
            if ((int) Game1.gameMode != 0 || !Game1.fadeToBlack || Game1.menuChoice != 0)
              return;
            Game1.currentLoader = Utility.generateNewFarm(Game1.IsClient);
            Game1.setGameMode((byte) 6);
            string str;
            if (!Game1.IsClient)
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2575");
            else
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2574", (object) Game1.client.serverName);
            Game1.loadingMessage = str;
            Game1.exitActiveMenu();
          }
        }
      }
    }

    public static void colorFarmer()
    {
      Utility.changeFarmerEyeColor(Utility.getEyeColors()[Game1.player.eyeColor]);
      Utility.changeFarmerHairColor(Utility.getHairColors()[Game1.player.hairColor]);
      Utility.changeFarmerOverallsColor(Utility.getOverallsColors()[Game1.player.overallsColor]);
      Utility.changeFarmerShirtColor(Utility.getShirtColors()[Game1.player.shirtColor]);
      Utility.changeFarmerSkinColor(Utility.getSkinColors()[Game1.player.skinColor]);
    }

    private void UpdateLocations(GameTime time)
    {
      if (Game1.menuUp)
        return;
      Game1.currentLocation.UpdateWhenCurrentLocation(time);
      if (Game1.IsServer)
      {
        foreach (Farmer farmer in Game1.otherFarmers.Values)
          farmer.currentLocation.UpdateWhenCurrentLocation(time);
      }
      for (int index = 0; index < Game1.locations.Count; ++index)
        Game1.locations[index].updateEvenIfFarmerIsntHere(time, false);
      if (Game1.currentLocation.Name.Equals("Temp"))
        Game1.currentLocation.updateEvenIfFarmerIsntHere(time, false);
      if (Game1.mine == null)
        return;
      Game1.mine.updateEvenIfFarmerIsntHere(time, false);
    }

    public static void performTenMinuteClockUpdate()
    {
      if (Game1.IsServer)
        MultiplayerUtility.broadcastGameClock();
      int trulyDarkTime = Game1.getTrulyDarkTime();
      Game1.gameTimeInterval = 0;
      Game1.timeOfDay += 10;
      if (Game1.timeOfDay % 100 >= 60)
        Game1.timeOfDay = Game1.timeOfDay - Game1.timeOfDay % 100 + 100;
      if (Game1.isLightning && Game1.timeOfDay < 2400)
        Utility.performLightningUpdate();
      if (Game1.timeOfDay == trulyDarkTime)
        Game1.currentLocation.switchOutNightTiles();
      else if (Game1.timeOfDay == Game1.getModeratelyDarkTime())
      {
        if (Game1.currentLocation.IsOutdoors && !Game1.isRaining)
          Game1.ambientLight = Color.White;
        if (!Game1.isRaining && !(Game1.currentLocation is MineShaft) && (Game1.currentSong != null && !Game1.currentSong.Name.Contains("ambient")) && Game1.currentLocation is Town)
          Game1.changeMusicTrack("none");
      }
      if (Game1.currentLocation.isOutdoors && !Game1.isRaining && (!Game1.eventUp && Game1.currentSong != null) && (Game1.currentSong.Name.Contains("day") && Game1.isDarkOut()))
        Game1.changeMusicTrack("none");
      if (Game1.weatherIcon == 1)
      {
        int int32 = Convert.ToInt32(Game1.temporaryContent.Load<Dictionary<string, string>>("Data\\Festivals\\" + Game1.currentSeason + (object) Game1.dayOfMonth)["conditions"].Split('/')[1].Split(' ')[0]);
        if (Game1.whereIsTodaysFest == null)
          Game1.whereIsTodaysFest = Game1.temporaryContent.Load<Dictionary<string, string>>("Data\\Festivals\\" + Game1.currentSeason + (object) Game1.dayOfMonth)["conditions"].Split('/')[0];
        if (Game1.timeOfDay == int32)
        {
          string str = Game1.temporaryContent.Load<Dictionary<string, string>>("Data\\Festivals\\" + Game1.currentSeason + (object) Game1.dayOfMonth)["conditions"].Split('/')[0];
          if (!(str == "Forest"))
          {
            if (!(str == "Town"))
            {
              if (str == "Beach")
                str = Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2639");
            }
            else
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2637");
          }
          else
            str = Game1.currentSeason.Equals("winter") ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2634") : Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2635");
          Game1.showGlobalMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2640", (object) Game1.temporaryContent.Load<Dictionary<string, string>>("Data\\Festivals\\" + Game1.currentSeason + (object) Game1.dayOfMonth)["name"]) + str);
        }
      }
      Game1.player.performTenMinuteUpdate();
      switch (Game1.timeOfDay)
      {
        case 2500:
          Game1.dayTimeMoneyBox.timeShakeTimer = 2000;
          Game1.player.doEmote(24);
          break;
        case 2600:
          Game1.dayTimeMoneyBox.timeShakeTimer = 2000;
          Game1.farmerShouldPassOut = true;
          if (Game1.player.getMount() != null)
          {
            Game1.player.getMount().dismount();
            break;
          }
          break;
        case 2800:
          Game1.exitActiveMenu();
          Game1.player.faceDirection(2);
          Game1.player.completelyStopAnimatingOrDoingAction();
          Game1.player.animateOnce(293);
          if (Game1.player.getMount() != null)
          {
            Game1.player.getMount().dismount();
            break;
          }
          break;
        case 1200:
          if (Game1.currentLocation.isOutdoors && !Game1.isRaining && (Game1.currentSong == null || Game1.currentSong.IsStopped || Game1.currentSong.Name.ToLower().Contains("ambient")))
          {
            Game1.playMorningSong();
            break;
          }
          break;
        case 2000:
          if (!Game1.isRaining && Game1.currentLocation is Town)
          {
            Game1.changeMusicTrack("none");
            break;
          }
          break;
        case 2400:
          Game1.dayTimeMoneyBox.timeShakeTimer = 2000;
          Game1.player.doEmote(24);
          Game1.showGlobalMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2652"));
          break;
      }
      if (Game1.timeOfDay >= 2600)
        Game1.farmerShouldPassOut = true;
      foreach (GameLocation location in Game1.locations)
      {
        location.performTenMinuteUpdate(Game1.timeOfDay);
        if (location.GetType() == typeof (Farm))
          ((BuildableGameLocation) location).timeUpdate(10);
      }
      if (Game1.mine == null)
        return;
      Game1.mine.performTenMinuteUpdate(Game1.timeOfDay);
    }

    public static void UpdateGameClock(GameTime time)
    {
      if (Game1.shouldTimePass())
        Game1.gameTimeInterval += Game1.IsClient ? 0 : time.ElapsedGameTime.Milliseconds;
      if (Game1.timeOfDay >= Game1.getTrulyDarkTime())
      {
        float num = Math.Min(0.93f, (float) (0.75 + ((double) ((int) ((double) (Game1.timeOfDay - Game1.timeOfDay % 100) + (double) (Game1.timeOfDay % 100 / 10) * 16.6599998474121) - Game1.getTrulyDarkTime()) + (double) Game1.gameTimeInterval / 7000.0 * 16.6000003814697) * 0.000624999986030161));
        Game1.outdoorLight = (Game1.isRaining ? Game1.ambientLight : Game1.eveningColor) * num;
      }
      else if (Game1.timeOfDay >= Game1.getStartingToGetDarkTime())
      {
        float num = Math.Min(0.93f, (float) (0.300000011920929 + ((double) ((int) ((double) (Game1.timeOfDay - Game1.timeOfDay % 100) + (double) (Game1.timeOfDay % 100 / 10) * 16.6599998474121) - Game1.getStartingToGetDarkTime()) + (double) Game1.gameTimeInterval / 7000.0 * 16.6000003814697) * 0.00224999990314245));
        Game1.outdoorLight = (Game1.isRaining ? Game1.ambientLight : Game1.eveningColor) * num;
      }
      else if (Game1.bloom != null && Game1.timeOfDay >= Game1.getStartingToGetDarkTime() - 100 && Game1.bloom.Visible)
        Game1.bloom.Settings.BloomThreshold = Math.Min(1f, Game1.bloom.Settings.BloomThreshold + 0.0004f);
      else if (Game1.isRaining)
        Game1.outdoorLight = Game1.ambientLight * 0.3f;
      if (Game1.gameTimeInterval <= 7000 + Game1.currentLocation.getExtraMillisecondsPerInGameMinuteForThisLocation())
        return;
      if (Game1.panMode)
        Game1.gameTimeInterval = 0;
      else
        Game1.performTenMinuteClockUpdate();
    }

    public static void checkForWedding()
    {
      if (!Game1.weddingToday)
        return;
      if (Game1.weddingToday)
        Game1.setRichPresence("wedding", (object) Game1.getCharacterFromName(Game1.player.spouse, false).name);
      Game1.player.faceDirection(2);
      Game1.currentLocation = Game1.getLocationFromName("Town");
      Game1.currentLocation.resetForPlayerEntry();
      Game1.getLocationFromName("Town").currentEvent = new Event(Utility.getWeddingEvent(), -1);
      Game1.eventUp = true;
      Game1.player.CanMove = false;
      Game1.currentLocation.Map.LoadTileSheets(Game1.mapDisplayDevice);
      Game1.player.position = new Vector2((float) (22 * Game1.tileSize), (float) (19 * Game1.tileSize));
      Game1.locationAfterWarp = (GameLocation) null;
    }

    public static void checkForNewLevelPerks()
    {
      Dictionary<string, string> dictionary1 = Game1.content.Load<Dictionary<string, string>>("Data\\CookingRecipes");
      int level = Game1.player.Level;
      foreach (string key in dictionary1.Keys)
      {
        string[] strArray = dictionary1[key].Split('/')[3].Split(' ');
        if (strArray[0].Equals("l") && Convert.ToInt32(strArray[1]) <= level && !Game1.player.cookingRecipes.ContainsKey(key))
        {
          Game1.player.cookingRecipes.Add(key, 0);
          Game1.currentObjectDialogue.Enqueue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2666") + key));
          Game1.currentDialogueCharacterIndex = 1;
          Game1.dialogueUp = true;
          Game1.dialogueTyping = true;
        }
        else if (strArray[0].Equals("s"))
        {
          int int32 = Convert.ToInt32(strArray[2]);
          bool flag = false;
          string str = strArray[1];
          if (!(str == "Farming"))
          {
            if (!(str == "Fishing"))
            {
              if (!(str == "Mining"))
              {
                if (!(str == "Combat"))
                {
                  if (!(str == "Foraging"))
                  {
                    if (str == "Luck" && Game1.player.LuckLevel >= int32)
                      flag = true;
                  }
                  else if (Game1.player.ForagingLevel >= int32)
                    flag = true;
                }
                else if (Game1.player.CombatLevel >= int32)
                  flag = true;
              }
              else if (Game1.player.MiningLevel >= int32)
                flag = true;
            }
            else if (Game1.player.FishingLevel >= int32)
              flag = true;
          }
          else if (Game1.player.FarmingLevel >= int32)
            flag = true;
          if (flag && !Game1.player.cookingRecipes.ContainsKey(key))
          {
            Game1.player.cookingRecipes.Add(key, 0);
            Game1.currentObjectDialogue.Enqueue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2666") + key));
            Game1.currentDialogueCharacterIndex = 1;
            Game1.dialogueUp = true;
            Game1.dialogueTyping = true;
          }
        }
      }
      Dictionary<string, string> dictionary2 = Game1.content.Load<Dictionary<string, string>>("Data\\CraftingRecipes");
      foreach (string key in dictionary2.Keys)
      {
        string[] strArray = dictionary2[key].Split('/')[4].Split(' ');
        if (strArray[0].Equals("l") && Convert.ToInt32(strArray[1]) <= level && !Game1.player.craftingRecipes.ContainsKey(key))
        {
          Game1.player.craftingRecipes.Add(key, 0);
          Game1.currentObjectDialogue.Enqueue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2677") + key));
          Game1.currentDialogueCharacterIndex = 1;
          Game1.dialogueUp = true;
          Game1.dialogueTyping = true;
        }
        else if (strArray[0].Equals("s"))
        {
          int int32 = Convert.ToInt32(strArray[2]);
          bool flag = false;
          string str = strArray[1];
          if (!(str == "Farming"))
          {
            if (!(str == "Fishing"))
            {
              if (!(str == "Mining"))
              {
                if (!(str == "Combat"))
                {
                  if (!(str == "Foraging"))
                  {
                    if (str == "Luck" && Game1.player.LuckLevel >= int32)
                      flag = true;
                  }
                  else if (Game1.player.ForagingLevel >= int32)
                    flag = true;
                }
                else if (Game1.player.CombatLevel >= int32)
                  flag = true;
              }
              else if (Game1.player.MiningLevel >= int32)
                flag = true;
            }
            else if (Game1.player.FishingLevel >= int32)
              flag = true;
          }
          else if (Game1.player.FarmingLevel >= int32)
            flag = true;
          if (flag && !Game1.player.craftingRecipes.ContainsKey(key))
          {
            Game1.player.craftingRecipes.Add(key, 0);
            Game1.currentObjectDialogue.Enqueue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2677") + key));
            Game1.currentDialogueCharacterIndex = 1;
            Game1.dialogueUp = true;
            Game1.dialogueTyping = true;
          }
        }
      }
    }

    public static void exitActiveMenu()
    {
      Game1.activeClickableMenu = (IClickableMenu) null;
    }

    public static void receiveNewLocationInfoFromServer(GameLocation location)
    {
      if (Game1.locationAfterWarp != null)
        Game1.client.sendMessage((byte) 5, new object[4]
        {
          (object) (short) Game1.xLocationAfterWarp,
          (object) (short) Game1.yLocationAfterWarp,
          (object) (Game1.currentLocation.isStructure ? Game1.currentLocation.uniqueName : Game1.currentLocation.name),
          (object) (byte) (location.isStructure ? 1 : 0)
        });
      GetMapClient.receiveMapFromServer(location, location.isStructure);
      Game1.player.currentLocation = location;
    }

    public static void updateScreenFade(GameTime time)
    {
      if (Game1.fadeIn)
      {
        Game1.fadeToBlackAlpha += (Game1.eventUp || Game1.farmEvent != null ? 0.0008f : 0.0019f) * (float) time.ElapsedGameTime.Milliseconds;
      }
      else
      {
        if (Game1.menuUp || Game1.messagePause || Game1.dialogueUp)
          return;
        Game1.fadeToBlackAlpha -= (Game1.eventUp || Game1.farmEvent != null ? 0.0008f : 0.0019f) * (float) time.ElapsedGameTime.Milliseconds;
      }
    }

    public static void fadeBlack()
    {
      Game1.fadeToBlack = true;
      Game1.fadeIn = true;
      Game1.fadeToBlackAlpha = 0.0f;
    }

    public static void fadeClear()
    {
      Game1.fadeIn = false;
      Game1.fadeToBlack = true;
      Game1.fadeToBlackAlpha = 1f;
    }

    public static void UpdateOther(GameTime time)
    {
      if (Game1.fadeToBlack && ((double) Game1.pauseTime == 0.0 || Game1.eventUp))
      {
        Game1.updateScreenFade(time);
        if ((double) Game1.fadeToBlackAlpha > 1.0 && !Game1.messagePause)
        {
          if (Game1.killScreen)
          {
            Game1.viewportFreeze = true;
            Game1.viewport.X = -10000;
          }
          if (Game1.exitToTitle)
          {
            Game1.menuUp = false;
            Game1.setGameMode((byte) 4);
            Game1.menuChoice = 0;
            Game1.fadeIn = false;
            Game1.fadeToBlack = true;
            Game1.fadeToBlackAlpha = 0.01f;
            Game1.exitToTitle = false;
            Game1.changeMusicTrack("none");
            Game1.debrisWeather.Clear();
            return;
          }
          if (!Game1.nonWarpFade && Game1.locationAfterWarp != null && !Game1.menuUp)
          {
            Game1.currentLocation.cleanupBeforePlayerExit();
            Game1.displayFarmer = true;
            if (Game1.eventOver)
            {
              Game1.eventFinished();
              if (Game1.dayOfMonth != 0)
                return;
              Game1.newDayAfterFade((Action) (() => Game1.player.position = new Vector2((float) (5 * Game1.tileSize), (float) (5 * Game1.tileSize))));
              return;
            }
            if (Game1.locationAfterWarp.Equals((object) Game1.currentLocation) && !Game1.eventUp && !Game1.currentLocation.Name.Equals("UndergroundMine"))
            {
              Game1.player.Position = new Vector2((float) (Game1.xLocationAfterWarp * Game1.tileSize), (float) (Game1.yLocationAfterWarp * Game1.tileSize - (Game1.player.Sprite.getHeight() - Game1.tileSize / 2) + Game1.tileSize / 4));
              Game1.viewportFreeze = false;
              Game1.currentLocation.resetForPlayerEntry();
            }
            else
            {
              if (Game1.locationAfterWarp.Name.Equals("UndergroundMine"))
              {
                if (!Game1.currentLocation.Name.Equals("UndergroundMine"))
                  Game1.changeMusicTrack("none");
                Vector2 vector2;
                if (Game1.currentLocation.Name.Equals("UndergroundMine") && Game1.mine.mineLevel < 120)
                {
                  vector2 = Game1.mine.enterMine(Game1.player, Game1.mine.mineLevel, Game1.player.ridingMineElevator);
                  Game1.player.ridingMineElevator = false;
                }
                else
                {
                  vector2 = Game1.mine.enterMine(Game1.player, Game1.mine.mineLevel, Game1.player.ridingMineElevator);
                  Game1.player.ridingMineElevator = false;
                }
                Game1.player.Halt();
                Game1.player.forceCanMove();
                Game1.xLocationAfterWarp = (int) vector2.X;
                Game1.yLocationAfterWarp = (int) vector2.Y;
                Game1.currentLocation = (GameLocation) Game1.mine;
                Game1.currentLocation.resetForPlayerEntry();
                Game1.currentLocation.Map.LoadTileSheets(Game1.mapDisplayDevice);
                Game1.checkForRunButton(Keyboard.GetState(), false);
              }
              if (!Game1.eventUp && !Game1.menuUp)
                Game1.player.Position = new Vector2((float) (Game1.xLocationAfterWarp * Game1.tileSize), (float) (Game1.yLocationAfterWarp * Game1.tileSize - (Game1.player.Sprite.getHeight() - Game1.tileSize / 2) + Game1.tileSize / 4));
              if (!Game1.locationAfterWarp.name.Equals("UndergroundMine") && Game1.locationAfterWarp != null)
              {
                Game1.currentLocation = Game1.locationAfterWarp;
                Game1.currentLocation.resetForPlayerEntry();
                Game1.currentLocation.Map.LoadTileSheets(Game1.mapDisplayDevice);
                if (!Game1.viewportFreeze && Game1.currentLocation.Map.DisplayWidth <= Game1.viewport.Width)
                  Game1.viewport.X = (Game1.currentLocation.Map.DisplayWidth - Game1.viewport.Width) / 2;
                if (!Game1.viewportFreeze && Game1.currentLocation.Map.DisplayHeight <= Game1.viewport.Height)
                  Game1.viewport.Y = (Game1.currentLocation.Map.DisplayHeight - Game1.viewport.Height) / 2;
                if (Game1.player.isRidingHorse())
                {
                  if ((double) Game1.player.position.X / (double) Game1.tileSize >= (double) (Game1.currentLocation.map.Layers[0].LayerWidth - 1))
                    Game1.player.position.X -= (float) Game1.tileSize;
                  else if ((double) Game1.player.position.Y / (double) Game1.tileSize >= (double) (Game1.currentLocation.map.Layers[0].LayerHeight - 1))
                    Game1.player.position.Y -= (float) (Game1.tileSize / 2);
                  if ((double) Game1.player.position.Y / (double) Game1.tileSize >= (double) (Game1.currentLocation.map.Layers[0].LayerHeight - 2))
                    Game1.player.position.X -= (float) (Game1.tileSize * 3 / 4);
                  Game1.warpCharacter((NPC) Game1.player.getMount(), Game1.currentLocation.Name, new Point(Game1.xLocationAfterWarp, Game1.yLocationAfterWarp), false, true);
                  Game1.player.Halt();
                }
                Game1.checkForRunButton(Keyboard.GetState(), true);
              }
              if (!Game1.eventUp)
                Game1.viewportFreeze = false;
            }
            Game1.player.faceDirection(Game1.facingDirectionAfterWarp);
            if (Game1.player.ActiveObject != null)
              Game1.player.showCarrying();
            else
              Game1.player.showNotCarrying();
            if (Game1.IsClient)
              Game1.receiveNewLocationInfoFromServer(Game1.locationAfterWarp);
            else if (Game1.IsServer)
            {
              Game1.player.currentLocation = Game1.currentLocation;
              MultiplayerUtility.broadcastFarmerWarp((short) Game1.xLocationAfterWarp, (short) Game1.yLocationAfterWarp, Game1.currentLocation.isStructure ? Game1.currentLocation.uniqueName : Game1.currentLocation.name, Game1.currentLocation.isStructure, Game1.player.uniqueMultiplayerID);
            }
            else
              Game1.player.currentLocation = Game1.currentLocation;
          }
          if (Game1.newDay)
          {
            Game1.newDayAfterFade((Action) (() =>
            {
              Game1.checkForWedding();
              if (Game1.eventOver)
              {
                Game1.eventFinished();
                if (Game1.dayOfMonth == 0)
                  Game1.newDayAfterFade((Action) (() => Game1.player.position = new Vector2((float) (5 * Game1.tileSize), (float) (5 * Game1.tileSize))));
              }
              Game1.nonWarpFade = false;
              Game1.fadeIn = false;
            }));
            return;
          }
          if (Game1.eventOver)
          {
            Game1.eventFinished();
            if (Game1.dayOfMonth != 0)
              return;
            Game1.newDayAfterFade((Action) (() =>
            {
              Game1.currentLocation.resetForPlayerEntry();
              Game1.nonWarpFade = false;
              Game1.fadeIn = false;
            }));
            return;
          }
          Game1.nonWarpFade = false;
          Game1.fadeIn = false;
          if (Game1.boardingBus)
          {
            Game1.boardingBus = false;
            Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2694") + (Game1.currentLocation.Name.Equals("Desert") ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2696") : Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2697")));
            Game1.messagePause = true;
            Game1.viewportFreeze = false;
          }
          if (Game1.isRaining && Game1.currentSong != null && (Game1.currentSong != null && Game1.currentSong.Name.Equals("rain")))
          {
            if (Game1.currentLocation.IsOutdoors)
              Game1.currentSong.SetVariable("Frequency", 100f);
            else if (!Game1.currentLocation.Name.Equals("UndergroundMine"))
              Game1.currentSong.SetVariable("Frequency", 15f);
          }
        }
        if ((double) Game1.fadeToBlackAlpha < 0.0)
        {
          Game1.fadeToBlack = false;
          if (Game1.killScreen)
            Game1.pauseThenMessage(1500, "..." + Game1.player.Name + "?", false);
          else if (!Game1.eventUp)
            Game1.player.CanMove = true;
          Game1.checkForRunButton(Game1.oldKBState, true);
        }
      }
      if (Game1.dialogueUp || Game1.currentBillboard != 0)
        Game1.player.CanMove = false;
      else
        Game1.player.FarmerSprite.freezeUntilDialogueIsOver = false;
      for (int index = Game1.delayedActions.Count - 1; index >= 0; --index)
      {
        if (Game1.delayedActions[index].update(time))
          Game1.delayedActions.RemoveAt(index);
      }
      if (Game1.farmerShouldPassOut && Game1.player.canMove && (Game1.player.freezePause <= 0 && !Game1.player.UsingTool))
      {
        Game1.exitActiveMenu();
        Game1.player.faceDirection(2);
        Game1.player.completelyStopAnimatingOrDoingAction();
        Game1.player.animateOnce(293);
        Game1.farmerShouldPassOut = false;
        Game1.player.freezePause = 7000;
      }
      for (int index = Game1.screenOverlayTempSprites.Count - 1; index >= 0; --index)
      {
        if (Game1.screenOverlayTempSprites[index].update(time))
          Game1.screenOverlayTempSprites.RemoveAt(index);
      }
      TimeSpan timeSpan;
      if (Game1.pickingTool)
      {
        double pickToolInterval = (double) Game1.pickToolInterval;
        timeSpan = time.ElapsedGameTime;
        double milliseconds = (double) timeSpan.Milliseconds;
        Game1.pickToolInterval = (float) (pickToolInterval + milliseconds);
        if ((double) Game1.pickToolInterval > 500.0)
        {
          Game1.pickingTool = false;
          Game1.pickToolInterval = 0.0f;
          if (!Game1.eventUp)
            Game1.player.CanMove = true;
          Game1.player.UsingTool = false;
          switch (Game1.player.FacingDirection)
          {
            case 0:
              Game1.player.Sprite.CurrentFrame = 16;
              break;
            case 1:
              Game1.player.Sprite.CurrentFrame = 8;
              break;
            case 2:
              Game1.player.Sprite.CurrentFrame = 0;
              break;
            case 3:
              Game1.player.Sprite.CurrentFrame = 24;
              break;
          }
          if (!Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift))
            Game1.player.setRunning(Game1.options.autoRun, false);
        }
        else if ((double) Game1.pickToolInterval > 83.3333358764648)
        {
          switch (Game1.player.FacingDirection)
          {
            case 0:
              Game1.player.FarmerSprite.setCurrentFrame(196);
              break;
            case 1:
              Game1.player.FarmerSprite.setCurrentFrame(194);
              break;
            case 2:
              Game1.player.FarmerSprite.setCurrentFrame(192);
              break;
            case 3:
              Game1.player.FarmerSprite.setCurrentFrame(198);
              break;
          }
        }
      }
      if ((Game1.player.CanMove || Game1.player.UsingTool) && Game1.shouldTimePass())
        Game1.buffsDisplay.update(time);
      if (Game1.player.CurrentItem != null)
        Game1.player.CurrentItem.actionWhenBeingHeld(Game1.player);
      float dialogueButtonScale = Game1.dialogueButtonScale;
      double num1 = (double) (Game1.tileSize / 4);
      timeSpan = time.TotalGameTime;
      double num2 = Math.Sin(timeSpan.TotalMilliseconds % 1570.0 / 500.0);
      Game1.dialogueButtonScale = (float) (num1 * num2);
      if ((double) dialogueButtonScale > (double) Game1.dialogueButtonScale && !Game1.dialogueButtonShrinking)
        Game1.dialogueButtonShrinking = true;
      else if ((double) dialogueButtonScale < (double) Game1.dialogueButtonScale && Game1.dialogueButtonShrinking)
        Game1.dialogueButtonShrinking = false;
      if (Game1.player.currentUpgrade != null && Game1.currentLocation.Name.Equals("Farm") && Game1.player.currentUpgrade.daysLeftTillUpgradeDone <= 3)
      {
        BuildingUpgrade currentUpgrade = Game1.player.currentUpgrade;
        timeSpan = time.ElapsedGameTime;
        double milliseconds = (double) timeSpan.Milliseconds;
        currentUpgrade.update((float) milliseconds);
      }
      if (Game1.screenGlow)
      {
        if (Game1.screenGlowUp || Game1.screenGlowHold)
        {
          if (Game1.screenGlowHold)
          {
            Game1.screenGlowAlpha = Math.Min(Game1.screenGlowAlpha + Game1.screenGlowRate, Game1.screenGlowMax);
          }
          else
          {
            Game1.screenGlowAlpha = Math.Min(Game1.screenGlowAlpha + 0.03f, 0.6f);
            if ((double) Game1.screenGlowAlpha >= 0.600000023841858)
              Game1.screenGlowUp = false;
          }
        }
        else
        {
          Game1.screenGlowAlpha -= 0.01f;
          if ((double) Game1.screenGlowAlpha <= 0.0)
            Game1.screenGlow = false;
        }
      }
      for (int index = Game1.hudMessages.Count - 1; index >= 0; --index)
      {
        if (Game1.hudMessages.ElementAt<HUDMessage>(index).update(time))
          Game1.hudMessages.RemoveAt(index);
      }
      if (Game1.isRaining && Game1.currentLocation.IsOutdoors)
      {
        for (int index = 0; index < Game1.rainDrops.Length; ++index)
        {
          if (Game1.rainDrops[index].frame == 0)
          {
            // ISSUE: explicit reference operation
            // ISSUE: variable of a reference type
            int& local = @Game1.rainDrops[index].accumulator;
            // ISSUE: explicit reference operation
            int num3 = ^local;
            timeSpan = time.ElapsedGameTime;
            int milliseconds = timeSpan.Milliseconds;
            int num4 = num3 + milliseconds;
            // ISSUE: explicit reference operation
            ^local = num4;
            if (Game1.rainDrops[index].accumulator >= 70)
            {
              Game1.rainDrops[index].position += new Vector2((float) (-Game1.tileSize / 4 + index * 8 / Game1.rainDrops.Length), (float) (Game1.tileSize / 2 - index * 8 / Game1.rainDrops.Length));
              Game1.rainDrops[index].accumulator = 0;
              if (Game1.random.NextDouble() < 0.1)
                ++Game1.rainDrops[index].frame;
              if ((double) Game1.rainDrops[index].position.Y > (double) (Game1.viewport.Height + Game1.tileSize))
                Game1.rainDrops[index].position.Y = (float) -Game1.tileSize;
            }
          }
          else
          {
            // ISSUE: explicit reference operation
            // ISSUE: variable of a reference type
            int& local = @Game1.rainDrops[index].accumulator;
            // ISSUE: explicit reference operation
            int num3 = ^local;
            timeSpan = time.ElapsedGameTime;
            int milliseconds = timeSpan.Milliseconds;
            int num4 = num3 + milliseconds;
            // ISSUE: explicit reference operation
            ^local = num4;
            if (Game1.rainDrops[index].accumulator > 70)
            {
              Game1.rainDrops[index].frame = (Game1.rainDrops[index].frame + 1) % 4;
              Game1.rainDrops[index].accumulator = 0;
              if (Game1.rainDrops[index].frame == 0)
                Game1.rainDrops[index].position = new Vector2((float) Game1.random.Next(Game1.viewport.Width), (float) Game1.random.Next(Game1.viewport.Height));
            }
          }
        }
      }
      else if (Game1.isDebrisWeather && Game1.currentLocation.IsOutdoors && !Game1.currentLocation.ignoreDebrisWeather)
      {
        if (Game1.currentSeason.Equals("fall") && Game1.random.NextDouble() < 0.001 && ((double) Game1.windGust == 0.0 && (double) WeatherDebris.globalWind >= -0.5))
        {
          Game1.windGust += (float) Game1.random.Next(-10, -1) / 100f;
          if (Game1.soundBank != null)
          {
            Game1.wind = Game1.soundBank.GetCue("wind");
            Game1.wind.Play();
          }
        }
        else if ((double) Game1.windGust != 0.0)
        {
          Game1.windGust = Math.Max(-5f, Game1.windGust * 1.02f);
          WeatherDebris.globalWind = Game1.windGust - 0.5f;
          if ((double) Game1.windGust < -0.200000002980232 && Game1.random.NextDouble() < 0.007)
            Game1.windGust = 0.0f;
        }
        foreach (WeatherDebris weatherDebris in Game1.debrisWeather)
          weatherDebris.update();
      }
      if ((double) WeatherDebris.globalWind < -0.5 && Game1.wind != null)
      {
        WeatherDebris.globalWind = Math.Min(-0.5f, WeatherDebris.globalWind + 0.015f);
        Game1.wind.SetVariable("Volume", (float) (-(double) WeatherDebris.globalWind * 20.0));
        Game1.wind.SetVariable("Frequency", (float) (-(double) WeatherDebris.globalWind * 20.0));
        if ((double) WeatherDebris.globalWind == -0.5)
          Game1.wind.Stop(AudioStopOptions.AsAuthored);
      }
      if (!Game1.fadeToBlack)
        Game1.currentLocation.checkForMusic(time);
      if ((double) Game1.debrisSoundInterval > 0.0)
      {
        double debrisSoundInterval = (double) Game1.debrisSoundInterval;
        timeSpan = time.ElapsedGameTime;
        double milliseconds = (double) timeSpan.Milliseconds;
        Game1.debrisSoundInterval = (float) (debrisSoundInterval - milliseconds);
      }
      double noteBlockTimer = (double) Game1.noteBlockTimer;
      timeSpan = time.ElapsedGameTime;
      double milliseconds1 = (double) timeSpan.Milliseconds;
      Game1.noteBlockTimer = (float) (noteBlockTimer + milliseconds1);
      if ((double) Game1.noteBlockTimer > 1000.0)
      {
        Game1.noteBlockTimer = 0.0f;
        if (Game1.player.health < 20 && Game1.CurrentEvent == null)
        {
          Game1.hitShakeTimer = 250;
          if (Game1.player.health <= 10)
          {
            Game1.hitShakeTimer = 500;
            for (int index = 0; index < 3; ++index)
            {
              List<TemporaryAnimatedSprite> overlayTempSprites = Game1.screenOverlayTempSprites;
              TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(366, 412, 5, 6), new Vector2((float) (Game1.random.Next(Game1.tileSize / 2) + Game1.viewport.Width - (48 + Game1.tileSize / 8) * 2), (float) (Game1.viewport.Height - 224 - (Game1.player.maxHealth - 100) - Game1.tileSize / 4 + 4)), false, 0.017f, Color.Red);
              temporaryAnimatedSprite.motion = new Vector2(-1.5f, (float) (Game1.random.Next(-1, 2) - 8));
              temporaryAnimatedSprite.acceleration = new Vector2(0.0f, 0.5f);
              int num3 = 1;
              temporaryAnimatedSprite.local = num3 != 0;
              double pixelZoom = (double) Game1.pixelZoom;
              temporaryAnimatedSprite.scale = (float) pixelZoom;
              int num4 = index * 150;
              temporaryAnimatedSprite.delayBeforeAnimationStart = num4;
              overlayTempSprites.Add(temporaryAnimatedSprite);
            }
          }
        }
      }
      if (Game1.showKeyHelp && !Game1.eventUp)
      {
        Game1.keyHelpString = "";
        if (Game1.dialogueUp)
          Game1.keyHelpString += Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2716");
        else if (Game1.menuUp)
        {
          Game1.keyHelpString += Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2719");
          Game1.keyHelpString += Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2720");
        }
        else if (Game1.player.ActiveObject != null)
        {
          Game1.keyHelpString += Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2727");
          Game1.keyHelpString = Game1.keyHelpString + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2728");
          if (Game1.player.numberOfItemsInInventory() < Game1.player.maxItems)
            Game1.keyHelpString = Game1.keyHelpString + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2729");
          if (Game1.player.numberOfItemsInInventory() > 0)
            Game1.keyHelpString = Game1.keyHelpString + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2730");
          Game1.keyHelpString = Game1.keyHelpString + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2731");
          Game1.keyHelpString = Game1.keyHelpString + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2732");
        }
        else
        {
          Game1.keyHelpString += Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2733");
          if (Game1.player.CurrentTool != null)
            Game1.keyHelpString = Game1.keyHelpString + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2734", (object) Game1.player.CurrentTool.DisplayName);
          if (Game1.player.numberOfItemsInInventory() > 0)
            Game1.keyHelpString = Game1.keyHelpString + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2735");
          Game1.keyHelpString = Game1.keyHelpString + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2731");
          Game1.keyHelpString = Game1.keyHelpString + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2732");
        }
      }
      Game1.drawLighting = Game1.currentLocation.IsOutdoors && !Game1.outdoorLight.Equals(Color.White) || (!Game1.ambientLight.Equals(Color.White) || Game1.currentLocation is MineShaft && !((MineShaft) Game1.currentLocation).getLightingColor(time).Equals(Color.White));
      if (Game1.hitShakeTimer > 0)
      {
        int hitShakeTimer = Game1.hitShakeTimer;
        timeSpan = time.ElapsedGameTime;
        int milliseconds2 = timeSpan.Milliseconds;
        Game1.hitShakeTimer = hitShakeTimer - milliseconds2;
      }
      if (Game1.staminaShakeTimer > 0)
      {
        int staminaShakeTimer = Game1.staminaShakeTimer;
        timeSpan = time.ElapsedGameTime;
        int milliseconds2 = timeSpan.Milliseconds;
        Game1.staminaShakeTimer = staminaShakeTimer - milliseconds2;
      }
      if (Game1.background != null)
        Game1.background.update(Game1.viewport);
      int tileHintCheckTimer = Game1.cursorTileHintCheckTimer;
      timeSpan = time.ElapsedGameTime;
      int totalMilliseconds = (int) timeSpan.TotalMilliseconds;
      Game1.cursorTileHintCheckTimer = tileHintCheckTimer - totalMilliseconds;
      Game1.currentCursorTile.X = (float) ((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize);
      Game1.currentCursorTile.Y = (float) ((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize);
      if (Game1.cursorTileHintCheckTimer <= 0 || !Game1.currentCursorTile.Equals(Game1.lastCursorTile))
      {
        Game1.cursorTileHintCheckTimer = 250;
        Game1.updateCursorTileHint();
        if (Game1.player.CanMove)
          Game1.checkForRunButton(Game1.oldKBState, true);
      }
      if (Game1.options.gamepadControls)
      {
        timeSpan = time.ElapsedGameTime;
        Rumble.update((float) timeSpan.Milliseconds);
        if (Game1.thumbstickMotionMargin > 0)
        {
          int thumbstickMotionMargin = Game1.thumbstickMotionMargin;
          timeSpan = time.ElapsedGameTime;
          int milliseconds2 = timeSpan.Milliseconds;
          Game1.thumbstickMotionMargin = thumbstickMotionMargin - milliseconds2;
        }
      }
      if (Game1.activeClickableMenu != null || Game1.farmEvent != null || Game1.keyboardDispatcher == null)
        return;
      Game1.keyboardDispatcher.Subscriber = (IKeyboardSubscriber) null;
    }

    public static void updateCursorTileHint()
    {
      if (Game1.activeClickableMenu != null)
        return;
      Game1.mouseCursorTransparency = 1f;
      Game1.isActionAtCurrentCursorTile = false;
      Game1.isInspectionAtCurrentCursorTile = false;
      int xTile = (Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize;
      int yTile = (Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize;
      if (Game1.currentLocation != null)
      {
        Game1.isActionAtCurrentCursorTile = Game1.currentLocation.isActionableTile(xTile, yTile, Game1.player);
        if (!Game1.isActionAtCurrentCursorTile)
          Game1.isActionAtCurrentCursorTile = Game1.currentLocation.isActionableTile(xTile, yTile + 1, Game1.player);
      }
      Game1.lastCursorTile = Game1.currentCursorTile;
    }

    public static void updateMusic()
    {
      if (Game1.soundBank == null)
        return;
      if (!Game1.nextMusicTrack.Equals(""))
      {
        Game1.musicPlayerVolume = Math.Max(0.0f, Game1.musicPlayerVolume - 0.01f);
        Game1.ambientPlayerVolume = Math.Max(0.0f, Game1.ambientPlayerVolume - 0.01f);
        Game1.musicCategory.SetVolume(Game1.musicPlayerVolume);
        Game1.ambientCategory.SetVolume(Game1.ambientPlayerVolume);
        if ((double) Game1.musicPlayerVolume != 0.0 || (double) Game1.ambientPlayerVolume != 0.0 || Game1.currentSong == null)
          return;
        if (Game1.nextMusicTrack.Equals("none"))
        {
          Game1.jukeboxPlaying = false;
          Game1.currentSong.Stop(AudioStopOptions.Immediate);
        }
        else if (((double) Game1.options.musicVolumeLevel != 0.0 || (double) Game1.options.ambientVolumeLevel != 0.0) && (!Game1.nextMusicTrack.Equals("rain") || Game1.endOfNightMenus.Count == 0))
        {
          Game1.currentSong.Stop(AudioStopOptions.Immediate);
          Game1.currentSong.Dispose();
          Game1.currentSong = Game1.soundBank.GetCue(Game1.nextMusicTrack);
          Game1.currentSong.Play();
          if (Game1.isRaining && Game1.currentSong != null && Game1.currentSong.Name.Equals("rain"))
          {
            if (Game1.currentLocation.IsOutdoors)
              Game1.currentSong.SetVariable("Frequency", 100f);
            else if (!Game1.currentLocation.Name.Equals("UndergroundMine"))
              Game1.currentSong.SetVariable("Frequency", 15f);
          }
        }
        else
          Game1.currentSong.Stop(AudioStopOptions.Immediate);
        Game1.nextMusicTrack = "";
      }
      else if ((double) Game1.musicPlayerVolume < (double) Game1.options.musicVolumeLevel || (double) Game1.ambientPlayerVolume < (double) Game1.options.ambientVolumeLevel)
      {
        if ((double) Game1.musicPlayerVolume < (double) Game1.options.musicVolumeLevel)
        {
          Game1.musicPlayerVolume = Math.Min(1f, Game1.musicPlayerVolume += 0.01f);
          Game1.musicCategory.SetVolume(Game1.options.musicVolumeLevel);
        }
        if ((double) Game1.ambientPlayerVolume >= (double) Game1.options.ambientVolumeLevel)
          return;
        Game1.ambientPlayerVolume = Math.Min(1f, Game1.ambientPlayerVolume += 0.015f);
        Game1.ambientCategory.SetVolume(Game1.ambientPlayerVolume);
      }
      else
      {
        if (Game1.currentSong == null || Game1.currentSong.IsPlaying || Game1.currentSong.IsStopped)
          return;
        Game1.currentSong = Game1.soundBank.GetCue(Game1.currentSong.Name);
        Game1.currentSong.Play();
      }
    }

    public static void updateRainDropPositionForPlayerMovement(int direction)
    {
      Game1.updateRainDropPositionForPlayerMovement(direction, false);
    }

    public static void updateRainDropPositionForPlayerMovement(int direction, bool overrideConstraints)
    {
      Game1.updateRainDropPositionForPlayerMovement(direction, overrideConstraints, (float) Game1.player.speed);
    }

    public static void updateRainDropPositionForPlayerMovement(int direction, bool overrideConstraints, float speed)
    {
      if (!overrideConstraints && (!Game1.isRaining && !Game1.isDebrisWeather || !Game1.currentLocation.IsOutdoors || direction != 0 && direction != 2 && (Game1.player.getStandingX() < Game1.viewport.Width / 2 || Game1.player.getStandingX() > Game1.currentLocation.Map.DisplayWidth - Game1.viewport.Width / 2) || direction != 1 && direction != 3 && (Game1.player.getStandingY() < Game1.viewport.Height / 2 || Game1.player.getStandingY() > Game1.currentLocation.Map.DisplayHeight - Game1.viewport.Height / 2)))
        return;
      if (Game1.isRaining)
      {
        for (int index = 0; index < Game1.rainDrops.Length; ++index)
        {
          if (direction == 0)
          {
            Game1.rainDrops[index].position.Y += speed;
            if ((double) Game1.rainDrops[index].position.Y > (double) (Game1.viewport.Height + Game1.tileSize))
              Game1.rainDrops[index].position.Y = (float) -Game1.tileSize;
          }
          else if (direction == 1)
          {
            Game1.rainDrops[index].position.X -= speed;
            if ((double) Game1.rainDrops[index].position.X < (double) -Game1.tileSize)
              Game1.rainDrops[index].position.X = (float) Game1.viewport.Width;
          }
          else if (direction == 2)
          {
            Game1.rainDrops[index].position.Y -= speed;
            if ((double) Game1.rainDrops[index].position.Y < (double) -Game1.tileSize)
              Game1.rainDrops[index].position.Y = (float) Game1.viewport.Height;
          }
          else if (direction == 3)
          {
            Game1.rainDrops[index].position.X += speed;
            if ((double) Game1.rainDrops[index].position.X > (double) (Game1.viewport.Width + Game1.tileSize))
              Game1.rainDrops[index].position.X = (float) -Game1.tileSize;
          }
        }
      }
      else
        Game1.updateDebrisWeatherForMovement(Game1.debrisWeather, direction, overrideConstraints, speed);
    }

    public static void updateDebrisWeatherForMovement(List<WeatherDebris> debris, int direction, bool overrideConstraints, float speed)
    {
      if ((double) Game1.fadeToBlackAlpha > 0.0 || debris == null)
        return;
      foreach (WeatherDebris debri in debris)
      {
        if (direction == 0)
        {
          debri.position.Y += speed;
          if ((double) debri.position.Y > (double) (Game1.viewport.Height + Game1.tileSize))
            debri.position.Y = (float) -Game1.tileSize;
        }
        else if (direction == 1)
        {
          debri.position.X -= speed;
          if ((double) debri.position.X < (double) -Game1.tileSize)
            debri.position.X = (float) Game1.viewport.Width;
        }
        else if (direction == 2)
        {
          debri.position.Y -= speed;
          if ((double) debri.position.Y < (double) -Game1.tileSize)
            debri.position.Y = (float) Game1.viewport.Height;
        }
        else if (direction == 3)
        {
          debri.position.X += speed;
          if ((double) debri.position.X > (double) (Game1.viewport.Width + Game1.tileSize))
            debri.position.X = (float) -Game1.tileSize;
        }
      }
    }

    public static Vector2 updateFloatingObjectPositionForMovement(Vector2 w, Vector2 current, Vector2 previous, float speed)
    {
      if ((double) current.Y < (double) previous.Y)
        w.Y -= Math.Abs(current.Y - previous.Y) * speed;
      else if ((double) current.Y > (double) previous.Y)
        w.Y += Math.Abs(current.Y - previous.Y) * speed;
      if ((double) current.X > (double) previous.X)
        w.X += Math.Abs(current.X - previous.X) * speed;
      else if ((double) current.X < (double) previous.X)
        w.X -= Math.Abs(current.X - previous.X) * speed;
      return w;
    }

    public static void updateRaindropPosition()
    {
      if (Game1.isRaining)
      {
        int num1 = Game1.viewport.X - (int) Game1.previousViewportPosition.X;
        int num2 = Game1.viewport.Y - (int) Game1.previousViewportPosition.Y;
        for (int index = 0; index < Game1.rainDrops.Length; ++index)
        {
          Game1.rainDrops[index].position.X -= (float) num1 * 1f;
          Game1.rainDrops[index].position.Y -= (float) num2 * 1f;
          if ((double) Game1.rainDrops[index].position.Y > (double) (Game1.viewport.Height + Game1.tileSize))
            Game1.rainDrops[index].position.Y = (float) -Game1.tileSize;
          else if ((double) Game1.rainDrops[index].position.X < (double) -Game1.tileSize)
            Game1.rainDrops[index].position.X = (float) Game1.viewport.Width;
          else if ((double) Game1.rainDrops[index].position.Y < (double) -Game1.tileSize)
            Game1.rainDrops[index].position.Y = (float) Game1.viewport.Height;
          else if ((double) Game1.rainDrops[index].position.X > (double) (Game1.viewport.Width + Game1.tileSize))
            Game1.rainDrops[index].position.X = (float) -Game1.tileSize;
        }
      }
      else
        Game1.updateDebrisWeatherForMovement(Game1.debrisWeather);
    }

    public static void updateDebrisWeatherForMovement(List<WeatherDebris> debris)
    {
      if (Game1.fadeToBlack || (double) Game1.fadeToBlackAlpha > 0.0 || debris == null)
        return;
      int num1 = Game1.viewport.X - (int) Game1.previousViewportPosition.X;
      int num2 = Game1.viewport.Y - (int) Game1.previousViewportPosition.Y;
      foreach (WeatherDebris debri in debris)
      {
        debri.position.X -= (float) num1 * 1f;
        debri.position.Y -= (float) num2 * 1f;
        if ((double) debri.position.Y > (double) (Game1.viewport.Height + Game1.tileSize))
          debri.position.Y = (float) -Game1.tileSize;
        else if ((double) debri.position.X < (double) -Game1.tileSize)
          debri.position.X = (float) Game1.viewport.Width;
        else if ((double) debri.position.Y < (double) -Game1.tileSize)
          debri.position.Y = (float) Game1.viewport.Height;
        else if ((double) debri.position.X > (double) (Game1.viewport.Width + Game1.tileSize))
          debri.position.X = (float) -Game1.tileSize;
      }
    }

    public static void randomizeDebrisWeatherPositions(List<WeatherDebris> debris)
    {
      if (debris == null)
        return;
      foreach (WeatherDebris debri in debris)
        debri.position = Utility.getRandomPositionOnScreen();
    }

    public static void eventFinished()
    {
      Game1.player.canOnlyWalk = false;
      bool flag = Game1.currentLocation.currentEvent != null && Game1.currentLocation.currentEvent.isFestival;
      Game1.eventOver = false;
      Game1.eventUp = false;
      Game1.player.CanMove = true;
      Game1.displayHUD = true;
      Game1.player.position = new Vector2(Game1.player.positionBeforeEvent.X * (float) Game1.tileSize, Game1.player.positionBeforeEvent.Y * (float) Game1.tileSize - (float) (Game1.tileSize / 2));
      if (Game1.locationAfterWarp == null || Game1.locationAfterWarp.Equals((object) Game1.currentLocation))
      {
        Game1.xLocationAfterWarp = (int) Game1.player.positionBeforeEvent.X;
        Game1.yLocationAfterWarp = (int) Game1.player.positionBeforeEvent.Y;
      }
      Game1.player.faceDirection(Game1.player.orientationBeforeEvent);
      Game1.player.completelyStopAnimatingOrDoingAction();
      Game1.viewportFreeze = false;
      if (Game1.currentLocation.currentEvent != null)
      {
        Game1.currentLocation.currentEvent.cleanup();
        Game1.currentLocation.currentEvent = (Event) null;
      }
      if (Game1.player.ActiveObject != null)
        Game1.player.showCarrying();
      if (Game1.isRaining && (Game1.currentSong == null || !Game1.currentSong.Name.Equals("rain")) && !Game1.currentLocation.Name.Equals("UndergroundMine"))
        Game1.changeMusicTrack("rain");
      else if (!Game1.isRaining && (Game1.currentSong == null || Game1.currentSong.Name == null || !Game1.currentSong.Name.Contains(Game1.currentSeason)))
        Game1.changeMusicTrack("none");
      if (Game1.dayOfMonth != 0)
      {
        Game1.currentLightSources.Clear();
        Game1.currentLocation.resetForPlayerEntry();
      }
      if (Game1.locationAfterWarp == null)
        return;
      if (flag)
      {
        foreach (Farmer farmer in Game1.otherFarmers.Values)
        {
          if (!farmer.IsMainPlayer)
          {
            farmer.Position = new Vector2((float) (Game1.xLocationAfterWarp * Game1.tileSize), (float) (Game1.yLocationAfterWarp * Game1.tileSize - (Game1.player.Sprite.getHeight() - Game1.tileSize / 2) + Game1.tileSize / 4));
            farmer.currentLocation.farmers.Remove(farmer);
            farmer.currentLocation = Game1.locationAfterWarp;
            farmer.currentLocation.farmers.Add(farmer);
          }
        }
      }
      Game1.player.Position = new Vector2((float) (Game1.xLocationAfterWarp * Game1.tileSize), (float) (Game1.yLocationAfterWarp * Game1.tileSize - (Game1.player.Sprite.getHeight() - Game1.tileSize / 2) + Game1.tileSize / 4));
      Game1.currentLocation.cleanupBeforePlayerExit();
      Game1.currentLocation = Game1.locationAfterWarp;
      Game1.currentLocation.resetForPlayerEntry();
      Game1.player.currentLocation = Game1.currentLocation;
      Game1.currentLocation.Map.LoadTileSheets(Game1.mapDisplayDevice);
      if (Game1.currentLocation.Map.DisplayWidth <= Game1.viewport.Width)
        Game1.viewport.X = (Game1.currentLocation.Map.DisplayWidth - Game1.viewport.Width) / 2;
      if (Game1.currentLocation.Map.DisplayHeight <= Game1.viewport.Height)
        Game1.viewport.Y = (Game1.currentLocation.Map.DisplayHeight - Game1.viewport.Height) / 2;
      Game1.currentLocation.currentEvent = (Event) null;
      Game1.eventUp = false;
      Game1.displayHUD = true;
      if (Game1.timeOfDay < 2000)
        return;
      Game1.currentLocation.switchOutNightTiles();
      Game1.player.canMove = true;
      Game1.UpdateGameClock(Game1.currentGameTime);
    }

    public static void populateDebrisWeatherArray()
    {
      Game1.debrisWeather.Clear();
      Game1.isDebrisWeather = true;
      int num = Game1.random.Next(16, 64);
      int which = Game1.currentSeason.Equals("spring") ? 0 : (Game1.currentSeason.Equals("winter") ? 3 : 2);
      for (int index = 0; index < num; ++index)
        Game1.debrisWeather.Add(new WeatherDebris(new Vector2((float) Game1.random.Next(0, Game1.viewport.Width), (float) Game1.random.Next(0, Game1.viewport.Height)), which, (float) Game1.random.Next(15) / 500f, (float) Game1.random.Next(-10, 0) / 50f, (float) Game1.random.Next(10) / 50f));
    }

    private static void newSeason()
    {
      string currentSeason = Game1.currentSeason;
      if (!(currentSeason == "spring"))
      {
        if (!(currentSeason == "summer"))
        {
          if (!(currentSeason == "fall"))
          {
            if (currentSeason == "winter")
              Game1.currentSeason = "spring";
          }
          else
            Game1.currentSeason = "winter";
        }
        else
          Game1.currentSeason = "fall";
      }
      else
        Game1.currentSeason = "summer";
      Game1.setGraphicsForSeason();
      Game1.dayOfMonth = 1;
      foreach (GameLocation location in Game1.locations)
        location.seasonUpdate(Game1.currentSeason, false);
    }

    public static void playItemNumberSelectSound()
    {
      if (Game1.selectedItemsType.Equals("flutePitch"))
      {
        if (Game1.soundBank == null)
          return;
        Cue cue = Game1.soundBank.GetCue("flute");
        string name = "Pitch";
        double num = (double) (100 * Game1.numberOfSelectedItems);
        cue.SetVariable(name, (float) num);
        cue.Play();
      }
      else if (Game1.selectedItemsType.Equals("drumTone"))
        Game1.playSound("drumkit" + (object) Game1.numberOfSelectedItems);
      else
        Game1.playSound("toolSwap");
    }

    public static void slotsDone()
    {
      Response[] answerChoices = new Response[2]
      {
        new Response("Play", Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2766")),
        new Response("Leave", Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2768"))
      };
      if ((int) Game1.slotResult[3] == 120)
      {
        Game1.currentLocation.createQuestionDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2769", (object) Game1.player.clubCoins), answerChoices, Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2771") + Game1.currentLocation.map.GetLayer("Buildings").PickTile(new Location((int) ((double) Game1.player.GetGrabTile().X * (double) Game1.tileSize), (int) ((double) Game1.player.GetGrabTile().Y * (double) Game1.tileSize)), Game1.viewport.Size).Properties["Action"].ToString().Split(' ')[1]);
        Game1.currentDialogueCharacterIndex = Game1.currentObjectDialogue.Peek().Length - 1;
      }
      else
      {
        Game1.playSound("money");
        string str = Game1.slotResult.Substring(0, 3).Equals("===") ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2776") : "";
        Game1.player.clubCoins += Convert.ToInt32(Game1.slotResult.Substring(3));
        Game1.currentLocation.createQuestionDialogue(Game1.parseText(str + Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2777", (object) Game1.slotResult.Substring(3))), answerChoices, Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2771") + Game1.currentLocation.map.GetLayer("Buildings").PickTile(new Location((int) ((double) Game1.player.GetGrabTile().X * (double) Game1.tileSize), (int) ((double) Game1.player.GetGrabTile().Y * (double) Game1.tileSize)), Game1.viewport.Size).Properties["Action"].ToString().Split(' ')[1]);
        Game1.currentDialogueCharacterIndex = Game1.currentObjectDialogue.Peek().Length - 1;
      }
    }

    public static void prepareSpouseForWedding()
    {
      Game1.weddingToday = true;
      NPC characterFromName = Game1.getCharacterFromName(Game1.player.spouse, false);
      characterFromName.Schedule = (Dictionary<int, SchedulePathDescription>) null;
      characterFromName.CurrentDialogue.Clear();
      characterFromName.CurrentDialogue.Push(new Dialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2782"), characterFromName));
      characterFromName.DefaultMap = "FarmHouse";
      characterFromName.DefaultPosition = Utility.PointToVector2((Game1.getLocationFromName("FarmHouse") as FarmHouse).getSpouseBedSpot()) * (float) Game1.tileSize;
      characterFromName.DefaultFacingDirection = 2;
      characterFromName.setMarried(true);
      Game1.weatherForTomorrow = 6;
    }

    public static void fixProblems()
    {
      List<NPC> allCharacters = Utility.getAllCharacters();
      Dictionary<string, string> dictionary1 = Game1.content.Load<Dictionary<string, string>>("Data\\NPCDispositions");
      foreach (string key in dictionary1.Keys)
      {
        bool flag = false;
        if (Game1.player.friendships.ContainsKey(key))
        {
          foreach (NPC npc in allCharacters)
          {
            if (npc.isVillager() && npc.name.Equals(key))
            {
              flag = true;
              if (dictionary1[key].Split('/')[5].Equals("datable"))
              {
                npc.datable = true;
                break;
              }
              break;
            }
          }
          if (!flag)
          {
            try
            {
              Game1.getLocationFromName(dictionary1[key].Split('/')[10].Split(' ')[0]).addCharacter(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\" + key), 0, Game1.tileSize / 4, Game1.tileSize * 2 / 4), new Vector2((float) (Convert.ToInt32(dictionary1[key].Split('/')[10].Split(' ')[1]) * Game1.tileSize), (float) (Convert.ToInt32(dictionary1[key].Split('/')[10].Split(' ')[2]) * Game1.tileSize)), dictionary1[key].Split('/')[10].Split(' ')[0], 0, key, (Dictionary<int, int[]>) null, Game1.content.Load<Texture2D>("Portraits\\" + key), false));
            }
            catch (Exception ex)
            {
            }
          }
        }
      }
      Dictionary<Type, bool> source = new Dictionary<Type, bool>();
      source.Add(new Axe().GetType(), false);
      source.Add(new Pickaxe().GetType(), false);
      source.Add(new Hoe().GetType(), false);
      source.Add(new WateringCan().GetType(), false);
      if (Game1.player.hasOrWillReceiveMail("ReturnScepter"))
        source.Add(new Wand().GetType(), false);
      bool flag1 = false;
      KeyValuePair<Type, bool> keyValuePair1;
      for (int index1 = 0; index1 < Game1.player.items.Count; ++index1)
      {
        if (Game1.player.items[index1] != null)
        {
          for (int index2 = 0; index2 < source.Count; ++index2)
          {
            Type type = Game1.player.items[index1].GetType();
            keyValuePair1 = source.ElementAt<KeyValuePair<Type, bool>>(index2);
            Type key1 = keyValuePair1.Key;
            if (type == key1)
            {
              Dictionary<Type, bool> dictionary2 = source;
              keyValuePair1 = source.ElementAt<KeyValuePair<Type, bool>>(index2);
              Type key2 = keyValuePair1.Key;
              int num = 1;
              dictionary2[key2] = num != 0;
            }
            else if (Game1.player.items[index1] is MeleeWeapon && (Game1.player.items[index1] as MeleeWeapon).Name.Equals("Scythe"))
              flag1 = true;
          }
        }
      }
      bool flag2 = true;
      for (int index = 0; index < source.Count; ++index)
      {
        keyValuePair1 = source.ElementAt<KeyValuePair<Type, bool>>(index);
        if (!keyValuePair1.Value)
        {
          flag2 = false;
          break;
        }
      }
      if (!flag1)
        flag2 = false;
      if (flag2)
        return;
      foreach (GameLocation location in Game1.locations)
      {
        foreach (Object @object in location.objects.Values)
        {
          if (@object is Chest)
          {
            for (int index = 0; index < source.Count; ++index)
            {
              foreach (Item obj in (@object as Chest).items)
              {
                Type type = obj.GetType();
                keyValuePair1 = source.ElementAt<KeyValuePair<Type, bool>>(index);
                Type key1 = keyValuePair1.Key;
                if (type == key1)
                {
                  Dictionary<Type, bool> dictionary2 = source;
                  keyValuePair1 = source.ElementAt<KeyValuePair<Type, bool>>(index);
                  Type key2 = keyValuePair1.Key;
                  int num = 1;
                  dictionary2[key2] = num != 0;
                }
                else if (obj is MeleeWeapon && (obj as MeleeWeapon).Name.Equals("Scythe"))
                  flag1 = true;
              }
            }
          }
        }
        if (location is FarmHouse)
        {
          for (int index = 0; index < source.Count; ++index)
          {
            foreach (Item obj in (location as FarmHouse).fridge.items)
            {
              Type type = obj.GetType();
              KeyValuePair<Type, bool> keyValuePair2 = source.ElementAt<KeyValuePair<Type, bool>>(index);
              Type key1 = keyValuePair2.Key;
              if (type == key1)
              {
                Dictionary<Type, bool> dictionary2 = source;
                keyValuePair2 = source.ElementAt<KeyValuePair<Type, bool>>(index);
                Type key2 = keyValuePair2.Key;
                int num = 1;
                dictionary2[key2] = num != 0;
              }
              else if (obj is MeleeWeapon && (obj as MeleeWeapon).Name.Equals("Scythe"))
                flag1 = true;
            }
          }
        }
        if (location is BuildableGameLocation)
        {
          foreach (Building building in (location as BuildableGameLocation).buildings)
          {
            if (building.indoors != null)
            {
              foreach (Object @object in building.indoors.objects.Values)
              {
                if (@object is Chest)
                {
                  for (int index = 0; index < source.Count; ++index)
                  {
                    foreach (Item obj in (@object as Chest).items)
                    {
                      Type type = obj.GetType();
                      KeyValuePair<Type, bool> keyValuePair2 = source.ElementAt<KeyValuePair<Type, bool>>(index);
                      Type key1 = keyValuePair2.Key;
                      if (type == key1)
                      {
                        Dictionary<Type, bool> dictionary2 = source;
                        keyValuePair2 = source.ElementAt<KeyValuePair<Type, bool>>(index);
                        Type key2 = keyValuePair2.Key;
                        int num = 1;
                        dictionary2[key2] = num != 0;
                      }
                      else if (obj is MeleeWeapon && (obj as MeleeWeapon).Name.Equals("Scythe"))
                        flag1 = true;
                    }
                  }
                }
              }
            }
            else if (building is Mill)
            {
              for (int index = 0; index < source.Count; ++index)
              {
                foreach (Item obj in (building as Mill).output.items)
                {
                  Type type = obj.GetType();
                  KeyValuePair<Type, bool> keyValuePair2 = source.ElementAt<KeyValuePair<Type, bool>>(index);
                  Type key1 = keyValuePair2.Key;
                  if (type == key1)
                  {
                    Dictionary<Type, bool> dictionary2 = source;
                    keyValuePair2 = source.ElementAt<KeyValuePair<Type, bool>>(index);
                    Type key2 = keyValuePair2.Key;
                    int num = 1;
                    dictionary2[key2] = num != 0;
                  }
                  else if (obj is MeleeWeapon && (obj as MeleeWeapon).Name.Equals("Scythe"))
                    flag1 = true;
                }
              }
            }
            else if (building is JunimoHut)
            {
              for (int index = 0; index < source.Count; ++index)
              {
                foreach (Item obj in (building as JunimoHut).output.items)
                {
                  Type type = obj.GetType();
                  KeyValuePair<Type, bool> keyValuePair2 = source.ElementAt<KeyValuePair<Type, bool>>(index);
                  Type key1 = keyValuePair2.Key;
                  if (type == key1)
                  {
                    Dictionary<Type, bool> dictionary2 = source;
                    keyValuePair2 = source.ElementAt<KeyValuePair<Type, bool>>(index);
                    Type key2 = keyValuePair2.Key;
                    int num = 1;
                    dictionary2[key2] = num != 0;
                  }
                  else if (obj is MeleeWeapon && (obj as MeleeWeapon).Name.Equals("Scythe"))
                    flag1 = true;
                }
              }
            }
          }
        }
      }
      if (Game1.player.toolBeingUpgraded != null)
        source[Game1.player.toolBeingUpgraded.GetType()] = true;
      List<string> stringList1 = new List<string>();
      for (int index = 0; index < source.Count; ++index)
      {
        KeyValuePair<Type, bool> keyValuePair2 = source.ElementAt<KeyValuePair<Type, bool>>(index);
        if (!keyValuePair2.Value)
        {
          List<string> stringList2 = stringList1;
          keyValuePair2 = source.ElementAt<KeyValuePair<Type, bool>>(index);
          string str = keyValuePair2.Key.ToString();
          stringList2.Add(str);
        }
      }
      if (!flag1)
        stringList1.Add("Scythe");
      if (stringList1.Count > 0)
        Game1.addMailForTomorrow("foundLostTools", false, false);
      for (int index = 0; index < stringList1.Count; ++index)
      {
        Item obj = (Item) null;
        string str = stringList1[index];
        if (!(str == "StardewValley.Tools.Axe"))
        {
          if (!(str == "StardewValley.Tools.Hoe"))
          {
            if (!(str == "StardewValley.Tools.WateringCan"))
            {
              if (!(str == "Scythe"))
              {
                if (!(str == "StardewValley.Tools.Pickaxe"))
                {
                  if (str == "StardewValley.Tools.Wand")
                    obj = (Item) new Wand();
                }
                else
                  obj = (Item) new Pickaxe();
              }
              else
                obj = (Item) new MeleeWeapon(47);
            }
            else
              obj = (Item) new WateringCan();
          }
          else
            obj = (Item) new Hoe();
        }
        else
          obj = (Item) new Axe();
        if (obj != null)
          Utility.getHomeOfFarmer(Game1.player).debris.Add(new Debris(obj, Game1.player.position + new Vector2((float) -Game1.tileSize, 0.0f)));
      }
    }

    public static void newDayAfterFade(Action after)
    {
      Game1._afterNewDayAction = after;
      if (Game1._newDayTask != null)
        return;
      Game1._newDayTask = new Task((Action) (() =>
      {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        Game1._newDayAfterFade();
      }));
      Game1._newDayTask.Start();
    }

    private static void _newDayAfterFade()
    {
      Game1.farmerShouldPassOut = false;
      try
      {
        Game1.fixProblems();
      }
      catch (Exception ex)
      {
      }
      Game1.whereIsTodaysFest = (string) null;
      if (Game1.wind != null)
      {
        Game1.wind.Stop(AudioStopOptions.Immediate);
        Game1.wind = (Cue) null;
      }
      Game1.player.currentEyes = 0;
      Game1.random = new Random((int) Game1.uniqueIDForThisGame / 100 + (int) Game1.stats.DaysPlayed * 10 + 1 + (int) Game1.stats.StepsTaken);
      for (int index = 0; index < Game1.dayOfMonth; ++index)
        Game1.random.Next();
      Game1.gameTimeInterval = 0;
      Game1.player.CanMove = true;
      Game1.player.FarmerSprite.pauseForSingleAnimation = false;
      Game1.player.FarmerSprite.StopAnimation();
      Game1.player.completelyStopAnimatingOrDoingAction();
      Game1.changeMusicTrack("none");
      Game1.dishOfTheDay = new Object(Vector2.Zero, Game1.random.Next(194, 240), Game1.random.Next(1, 4 + (Game1.random.NextDouble() < 0.08 ? 10 : 0)));
      if (Game1.dishOfTheDay.parentSheetIndex == 217)
        Game1.dishOfTheDay = new Object(Vector2.Zero, 216, Game1.random.Next(1, 4 + (Game1.random.NextDouble() < 0.08 ? 10 : 0)));
      foreach (NPC allCharacter in Utility.getAllCharacters())
        allCharacter.updatedDialogueYet = false;
      foreach (GameLocation location in Game1.locations)
      {
        location.currentEvent = (Event) null;
        for (int index = location.objects.Count - 1; index >= 0; --index)
        {
          if (location.objects[location.objects.Keys.ElementAt<Vector2>(index)].minutesElapsed(3000 - Game1.timeOfDay, location))
            location.objects.Remove(location.objects.Keys.ElementAt<Vector2>(index));
        }
      }
      foreach (Building building in Game1.getFarm().buildings)
      {
        if (building.indoors != null)
        {
          for (int index = building.indoors.objects.Count - 1; index >= 0; --index)
          {
            if (building.indoors.objects[building.indoors.objects.Keys.ElementAt<Vector2>(index)].minutesElapsed(3000 - Game1.timeOfDay, building.indoors))
              building.indoors.objects.Remove(building.indoors.objects.Keys.ElementAt<Vector2>(index));
          }
        }
      }
      Game1.globalOutdoorLighting = 0.0f;
      Game1.outdoorLight = Color.White;
      Game1.ambientLight = Color.White;
      if (Game1.isLightning)
        Utility.overnightLightning();
      Game1.tmpTimeOfDay = Game1.timeOfDay;
      Game1.weddingToday = false;
      if (Game1.player.spouse != null && Game1.player.spouse.Contains("engaged"))
      {
        --Game1.countdownToWedding;
        if (Game1.countdownToWedding == 0)
        {
          Game1.player.spouse = Game1.player.spouse.Replace("engaged", "");
          Game1.prepareSpouseForWedding();
        }
      }
      foreach (string str1 in Game1.player.mailForTomorrow)
      {
        if (str1.Contains("%&NL&%"))
        {
          string str2 = str1.Replace("%&NL&%", "");
          if (!Game1.player.mailReceived.Contains(str2))
            Game1.player.mailReceived.Add(str2);
        }
        else
          Game1.mailbox.Enqueue(str1);
      }
      if (Game1.player.friendships.Count > 0)
      {
        string key = Game1.player.friendships.Keys.ElementAt<string>(Game1.random.Next(Game1.player.friendships.Keys.Count));
        if (Game1.random.NextDouble() < (double) (Game1.player.friendships[key][0] / 250) * 0.1 && (Game1.player.spouse == null || !Game1.player.spouse.Equals(key)) && Game1.content.Load<Dictionary<string, string>>("Data\\mail").ContainsKey(key))
          Game1.mailbox.Enqueue(key);
      }
      Game1.player.dayupdate();
      Game1.dailyLuck = !Game1.player.hasSpecialCharm || Game1.random.NextDouble() >= 0.8 ? (double) Game1.random.Next(-100, 101) / 1000.0 : 0.1;
      ++Game1.dayOfMonth;
      ++Game1.stats.DaysPlayed;
      Game1.player.dayOfMonthForSaveGame = new int?(Game1.dayOfMonth);
      Game1.player.seasonForSaveGame = new int?(Utility.getSeasonNumber(Game1.currentSeason));
      Game1.player.yearForSaveGame = new int?(Game1.year);
      if (Game1.dayOfMonth == 27 && Game1.currentSeason.Equals("spring"))
      {
        int year1 = Game1.year;
      }
      if (Game1.dayOfMonth == 29)
      {
        Game1.newSeason();
        if (!Game1.currentSeason.Equals("winter"))
          Game1.cropsOfTheWeek = Utility.cropsOfTheWeek();
        if (Game1.currentSeason.Equals("spring"))
        {
          ++Game1.year;
          if (Game1.year == 2)
          {
            Game1.getLocationFromName("SamHouse").characters.Add(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Kent"), 0, 16, 32), new Vector2((float) (8 * Game1.tileSize), (float) (13 * Game1.tileSize)), "SamHouse", 3, "Kent", false, (Dictionary<int, int[]>) null, Game1.content.Load<Texture2D>("Portraits\\Kent")));
            if (!Game1.player.friendships.ContainsKey("Kent"))
              Game1.player.friendships.Add("Kent", new int[5]);
          }
        }
        int year2 = Game1.year;
      }
      if (Game1.content.Load<Dictionary<string, string>>("Data\\mail").ContainsKey(Game1.currentSeason + "_" + (object) Game1.dayOfMonth + "_" + (object) Game1.year))
        Game1.mailbox.Enqueue(Game1.currentSeason + "_" + (object) Game1.dayOfMonth + "_" + (object) Game1.year);
      else if (Game1.content.Load<Dictionary<string, string>>("Data\\mail").ContainsKey(Game1.currentSeason + "_" + (object) Game1.dayOfMonth))
        Game1.mailbox.Enqueue(Game1.currentSeason + "_" + (object) Game1.dayOfMonth);
      Game1.questOfTheDay = Utility.getQuestOfTheDay();
      if (Utility.isFestivalDay(Game1.dayOfMonth + 1, Game1.currentSeason))
        Game1.questOfTheDay = (Quest) null;
      Game1.player.resetFriendshipsForNewDay();
      if (Game1.dayOfMonth == 1 || Game1.stats.DaysPlayed <= 4U)
        Game1.weatherForTomorrow = 0;
      if (Game1.bloom != null)
        Game1.bloomDay = false;
      if ((int) Game1.stats.DaysPlayed == 3)
        Game1.weatherForTomorrow = 1;
      if (Game1.currentSeason.Equals("summer") && Game1.dayOfMonth % 13 == 0)
        Game1.weatherForTomorrow = 3;
      if (Utility.isFestivalDay(Game1.dayOfMonth, Game1.currentSeason))
      {
        Game1.weatherForTomorrow = 4;
        Game1.questOfTheDay = (Quest) null;
      }
      if (Game1.weddingToday)
        Game1.weatherForTomorrow = 6;
      Game1.wasRainingYesterday = Game1.isRaining || Game1.isLightning;
      if (Game1.weatherForTomorrow == 1 || Game1.weatherForTomorrow == 3)
        Game1.isRaining = true;
      if (Game1.weatherForTomorrow == 3)
        Game1.isLightning = true;
      if (Game1.weatherForTomorrow == 0 || Game1.weatherForTomorrow == 2 || (Game1.weatherForTomorrow == 4 || Game1.weatherForTomorrow == 5) || Game1.weatherForTomorrow == 6)
      {
        Game1.isRaining = false;
        Game1.isLightning = false;
        Game1.isSnowing = false;
        Game1.changeMusicTrack("none");
        if (Game1.weatherForTomorrow == 5)
          Game1.isSnowing = true;
      }
      if (!Game1.isRaining && !Game1.isLightning)
      {
        ++Game1.currentSongIndex;
        if (Game1.currentSongIndex > 3 || Game1.dayOfMonth == 1)
          Game1.currentSongIndex = 1;
      }
      Game1.debrisWeather.Clear();
      Game1.isDebrisWeather = false;
      if (Game1.bloom != null)
        Game1.bloom.Visible = false;
      if (Game1.weatherForTomorrow == 2)
        Game1.populateDebrisWeatherArray();
      else if (!Game1.isRaining && Game1.chanceToRainTomorrow <= 0.1 && Game1.bloom != null)
      {
        Game1.bloomDay = true;
        Game1.bloom.Settings = BloomSettings.PresetSettings[5];
      }
      Game1.chanceToRainTomorrow = !Game1.currentSeason.Equals("summer") ? (!Game1.currentSeason.Equals("winter") ? 0.183 : 0.63) : (Game1.dayOfMonth > 1 ? 0.12 + (double) Game1.dayOfMonth * (3.0 / 1000.0) : 0.0);
      if (Game1.random.NextDouble() < Game1.chanceToRainTomorrow)
      {
        Game1.weatherForTomorrow = 1;
        if (Game1.currentSeason.Equals("summer") && Game1.random.NextDouble() < 0.85 || !Game1.currentSeason.Equals("winter") && Game1.random.NextDouble() < 0.25 && (Game1.dayOfMonth > 2 && Game1.stats.DaysPlayed > 27U))
          Game1.weatherForTomorrow = 3;
        if (Game1.currentSeason.Equals("winter"))
          Game1.weatherForTomorrow = 5;
      }
      else
        Game1.weatherForTomorrow = Game1.stats.DaysPlayed <= 2U || (!Game1.currentSeason.Equals("spring") || Game1.random.NextDouble() >= 0.2) && (!Game1.currentSeason.Equals("fall") || Game1.random.NextDouble() >= 0.6) || Game1.weddingToday ? 0 : 2;
      if (Utility.isFestivalDay(Game1.dayOfMonth + 1, Game1.currentSeason))
        Game1.weatherForTomorrow = 4;
      if ((int) Game1.stats.DaysPlayed == 2)
        Game1.weatherForTomorrow = 1;
      foreach (GameLocation location in Game1.locations)
      {
        location.UpdateCharacterDialogues();
        int dayOfMonth = Game1.dayOfMonth;
        location.DayUpdate(dayOfMonth);
      }
      foreach (NPC allCharacter in Utility.getAllCharacters())
        allCharacter.dayUpdate(Game1.dayOfMonth);
      GameLocation locationFromName = Game1.getLocationFromName("SeedShop");
      if (locationFromName != null)
      {
        SeedShop seedShop = locationFromName as SeedShop;
        for (int index1 = seedShop.itemsFromPlayerToSell.Count - 1; index1 >= 0; --index1)
        {
          for (int index2 = 0; index2 < seedShop.itemsFromPlayerToSell[index1].Stack; ++index2)
          {
            if (Game1.random.NextDouble() < 0.04 && seedShop.itemsFromPlayerToSell[index1] is Object && (seedShop.itemsFromPlayerToSell[index1] as Object).edibility != -300)
            {
              NPC randomTownNpc = Utility.getRandomTownNPC();
              if (randomTownNpc.age != 2)
              {
                randomTownNpc.addExtraDialogues(seedShop.getPurchasedItemDialogueForNPC(seedShop.itemsFromPlayerToSell[index1] as Object, randomTownNpc));
                --seedShop.itemsFromPlayerToSell[index1].Stack;
              }
            }
            else if (Game1.random.NextDouble() < 0.15)
              --seedShop.itemsFromPlayerToSell[index1].Stack;
            if (seedShop.itemsFromPlayerToSell[index1].Stack <= 0)
            {
              seedShop.itemsFromPlayerToSell.RemoveAt(index1);
              break;
            }
          }
        }
      }
      if (Game1.isRaining)
      {
        foreach (KeyValuePair<Vector2, TerrainFeature> terrainFeature in (Dictionary<Vector2, TerrainFeature>) Game1.getLocationFromName("Farm").terrainFeatures)
        {
          if (terrainFeature.Value.GetType() == typeof (HoeDirt))
            ((HoeDirt) terrainFeature.Value).state = 1;
        }
      }
      if (Game1.player.currentUpgrade != null)
      {
        --Game1.player.currentUpgrade.daysLeftTillUpgradeDone;
        if (Game1.getLocationFromName("Farm").objects.ContainsKey(new Vector2(Game1.player.currentUpgrade.positionOfCarpenter.X / (float) Game1.tileSize, Game1.player.currentUpgrade.positionOfCarpenter.Y / (float) Game1.tileSize)))
          Game1.getLocationFromName("Farm").objects.Remove(new Vector2(Game1.player.currentUpgrade.positionOfCarpenter.X / (float) Game1.tileSize, Game1.player.currentUpgrade.positionOfCarpenter.Y / (float) Game1.tileSize));
        if (Game1.player.currentUpgrade.daysLeftTillUpgradeDone == 0)
        {
          string whichBuilding = Game1.player.currentUpgrade.whichBuilding;
          if (!(whichBuilding == "House"))
          {
            if (!(whichBuilding == "Coop"))
            {
              if (!(whichBuilding == "Barn"))
              {
                if (whichBuilding == "Greenhouse")
                {
                  Game1.player.hasGreenhouse = true;
                  Game1.greenhouseTexture = Game1.content.Load<Texture2D>("BuildingUpgrades\\Greenhouse");
                }
              }
              else
              {
                ++Game1.player.BarnUpgradeLevel;
                Game1.currentBarnTexture = Game1.content.Load<Texture2D>("BuildingUpgrades\\Barn" + (object) Game1.player.BarnUpgradeLevel);
              }
            }
            else
            {
              ++Game1.player.CoopUpgradeLevel;
              Game1.currentCoopTexture = Game1.content.Load<Texture2D>("BuildingUpgrades\\Coop" + (object) Game1.player.CoopUpgradeLevel);
            }
          }
          else
          {
            ++Game1.player.HouseUpgradeLevel;
            Game1.currentHouseTexture = Game1.content.Load<Texture2D>("Buildings\\House" + (object) Game1.player.HouseUpgradeLevel);
          }
          Game1.stats.checkForBuildingUpgradeAchievements();
          Game1.removeFrontLayerForFarmBuildings();
          Game1.addNewFarmBuildingMaps();
          Game1.player.currentUpgrade = (BuildingUpgrade) null;
          Game1.changeInvisibility("Robin", false);
        }
        else if (Game1.player.currentUpgrade.daysLeftTillUpgradeDone == 3)
          Game1.changeInvisibility("Robin", true);
      }
      Game1.stats.AverageBedtime = (uint) Game1.timeOfDay;
      Game1.timeOfDay = 600;
      Game1.newDay = false;
      if (Game1.player.currentLocation != null && Game1.player.currentLocation is FarmHouse)
      {
        Game1.player.position = Utility.PointToVector2((Game1.getLocationFromName("FarmHouse") as FarmHouse).getBedSpot()) * (float) Game1.tileSize;
        Game1.player.position.Y += (float) (Game1.tileSize / 2);
        Game1.player.position.X -= (float) (Game1.tileSize / 2);
        Game1.player.faceDirection(1);
      }
      int currentWallpaper = Game1.currentWallpaper;
      Game1.wallpaperPrice = Game1.random.Next(75, 500) + Game1.player.HouseUpgradeLevel * 100;
      Game1.wallpaperPrice -= Game1.wallpaperPrice % 5;
      int currentFloor = Game1.currentFloor;
      Game1.floorPrice = Game1.random.Next(75, 500) + Game1.player.HouseUpgradeLevel * 100;
      Game1.floorPrice -= Game1.floorPrice % 5;
      Game1.updateWeatherIcon();
      Game1.freezeControls = false;
      if (Game1.stats.DaysPlayed > 1U)
      {
        Game1.farmEvent = Utility.pickFarmEvent();
        if (Game1.farmEvent != null && Game1.farmEvent.setUp())
          Game1.farmEvent = (FarmEvent) null;
      }
      Game1.player.mailForTomorrow.Clear();
      if (Game1.farmEvent != null)
        return;
      Game1.showEndOfNightStuff();
    }

    public static void updateWeatherIcon()
    {
      Game1.weatherIcon = !Game1.isSnowing ? (!Game1.isRaining ? (!Game1.isDebrisWeather || !Game1.currentSeason.Equals("spring") ? (!Game1.isDebrisWeather || !Game1.currentSeason.Equals("fall") ? (!Game1.isDebrisWeather || !Game1.currentSeason.Equals("winter") ? (!Game1.weddingToday ? 2 : 0) : 7) : 6) : 3) : 4) : 7;
      if (Game1.isLightning)
        Game1.weatherIcon = 5;
      if (!Utility.isFestivalDay(Game1.dayOfMonth, Game1.currentSeason))
        return;
      Game1.weatherIcon = 1;
    }

    public static void showEndOfNightStuff()
    {
      bool flag1 = false;
      if (Game1.getFarm().shippingBin.Count > 0)
      {
        Game1.endOfNightMenus.Push((IClickableMenu) new ShippingMenu(Game1.getFarm().shippingBin));
        Game1.getFarm().shippingBin.Clear();
        flag1 = true;
      }
      bool flag2 = false;
      if (Game1.player.newLevels.Count > 0 && !flag1)
        Game1.endOfNightMenus.Push((IClickableMenu) new SaveGameMenu());
      while (Game1.player.newLevels.Count > 0)
      {
        Game1.endOfNightMenus.Push((IClickableMenu) new LevelUpMenu(Game1.player.newLevels.Last<Point>().X, Game1.player.newLevels.Last<Point>().Y));
        Game1.player.newLevels.RemoveAt(Game1.player.newLevels.Count - 1);
        flag2 = true;
      }
      if (flag2)
        Game1.playSound("newRecord");
      if (Game1.endOfNightMenus.Count > 0)
      {
        Game1.showingEndOfNightStuff = true;
        Game1.activeClickableMenu = Game1.endOfNightMenus.Pop();
      }
      else if (Game1.saveOnNewDay)
      {
        Game1.showingEndOfNightStuff = true;
        Game1.activeClickableMenu = (IClickableMenu) new SaveGameMenu();
      }
      else
      {
        Game1.currentLocation.resetForPlayerEntry();
        Game1.globalFadeToClear(new Game1.afterFadeFunction(Game1.playMorningSong), 0.02f);
      }
    }

    public static void playerEatObject(Object o, bool overrideFullness = false)
    {
      if (o.ParentSheetIndex == 434)
        Game1.changeMusicTrack("none");
      if (Game1.player.getFacingDirection() != 2)
        Game1.player.faceDirection(2);
      Game1.player.itemToEat = (Item) o;
      Game1.player.mostRecentlyGrabbedItem = (Item) o;
      string[] strArray = Game1.objectInformation[o.ParentSheetIndex].Split('/');
      Game1.player.forceCanMove();
      Game1.player.completelyStopAnimatingOrDoingAction();
      if (strArray.Length > 6 && strArray[6].Equals("drink"))
      {
        if (Game1.buffsDisplay.hasBuff(7) && !overrideFullness)
        {
          Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2898"), Color.OrangeRed, 3500f));
          return;
        }
        ((FarmerSprite) Game1.player.Sprite).animateOnce(294, 80f, 8);
      }
      else if (Convert.ToInt32(strArray[2]) != -300)
      {
        if (Game1.buffsDisplay.hasBuff(6) && !overrideFullness)
        {
          Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2899"), Color.OrangeRed, 3500f));
          return;
        }
        ((FarmerSprite) Game1.player.Sprite).animateOnce(216, 80f, 8);
      }
      Game1.player.freezePause = 20000;
      Game1.player.CanMove = false;
      Game1.isEating = true;
    }

    public static void eatHeldObject()
    {
      if (Game1.fadeToBlack)
        return;
      if (Game1.player.ActiveObject == null)
        Game1.player.ActiveObject = (Object) Game1.player.mostRecentlyGrabbedItem;
      Game1.playerEatObject(Game1.player.ActiveObject, false);
      if (!Game1.isEating)
        return;
      Game1.player.reduceActiveItemByOne();
      Game1.player.CanMove = false;
    }

    private static void updateWallpaperInSeedShop()
    {
      GameLocation locationFromName = Game1.getLocationFromName("SeedShop");
      for (int index = 9; index < 12; ++index)
      {
        locationFromName.Map.GetLayer("Back").Tiles[index, 15] = (Tile) new StaticTile(locationFromName.Map.GetLayer("Back"), locationFromName.Map.GetTileSheet("Wallpapers"), BlendMode.Alpha, Game1.currentWallpaper);
        locationFromName.Map.GetLayer("Back").Tiles[index, 16] = (Tile) new StaticTile(locationFromName.Map.GetLayer("Back"), locationFromName.Map.GetTileSheet("Wallpapers"), BlendMode.Alpha, Game1.currentWallpaper);
      }
    }

    public static void setGraphicsForSeason()
    {
      foreach (GameLocation location in Game1.locations)
      {
        location.seasonUpdate(Game1.currentSeason, true);
        if (location.IsOutdoors)
        {
          if (!location.Name.Equals("Desert"))
          {
            for (int index = 0; index < location.Map.TileSheets.Count; ++index)
            {
              if (!location.Map.TileSheets[index].ImageSource.Contains("path") && !location.Map.TileSheets[index].ImageSource.Contains("object"))
              {
                location.Map.TileSheets[index].ImageSource = "Maps\\" + Game1.currentSeason + "_" + location.Map.TileSheets[index].ImageSource.Split('_')[1];
                location.Map.DisposeTileSheets(Game1.mapDisplayDevice);
                location.Map.LoadTileSheets(Game1.mapDisplayDevice);
              }
            }
          }
          if (Game1.currentSeason.Equals("spring"))
          {
            foreach (KeyValuePair<Vector2, Object> keyValuePair in (Dictionary<Vector2, Object>) location.Objects)
            {
              if ((keyValuePair.Value.Name.Contains("Stump") || keyValuePair.Value.Name.Contains("Boulder") || (keyValuePair.Value.Name.Equals("Stick") || keyValuePair.Value.Name.Equals("Stone"))) && (keyValuePair.Value.ParentSheetIndex >= 378 && keyValuePair.Value.ParentSheetIndex <= 391))
                keyValuePair.Value.ParentSheetIndex -= 376;
            }
            Game1.eveningColor = new Color((int) byte.MaxValue, (int) byte.MaxValue, 0);
          }
          else if (Game1.currentSeason.Equals("summer"))
          {
            foreach (KeyValuePair<Vector2, Object> keyValuePair in (Dictionary<Vector2, Object>) location.Objects)
            {
              if (keyValuePair.Value.Name.Contains("Weed"))
              {
                if (keyValuePair.Value.parentSheetIndex == 792)
                  ++keyValuePair.Value.ParentSheetIndex;
                else if (Game1.random.NextDouble() < 0.3)
                  keyValuePair.Value.ParentSheetIndex = 676;
                else if (Game1.random.NextDouble() < 0.3)
                  keyValuePair.Value.ParentSheetIndex = 677;
              }
            }
            Game1.eveningColor = new Color((int) byte.MaxValue, (int) byte.MaxValue, 0);
          }
          else if (Game1.currentSeason.Equals("fall"))
          {
            foreach (KeyValuePair<Vector2, Object> keyValuePair in (Dictionary<Vector2, Object>) location.Objects)
            {
              if (keyValuePair.Value.Name.Contains("Weed"))
              {
                if (keyValuePair.Value.parentSheetIndex == 793)
                  ++keyValuePair.Value.ParentSheetIndex;
                else
                  keyValuePair.Value.ParentSheetIndex = Game1.random.NextDouble() >= 0.5 ? 679 : 678;
              }
            }
            Game1.eveningColor = new Color((int) byte.MaxValue, (int) byte.MaxValue, 0);
            foreach (WeatherDebris weatherDebris in Game1.debrisWeather)
              weatherDebris.which = 2;
          }
          else if (Game1.currentSeason.Equals("winter"))
          {
            for (int index = location.Objects.Count - 1; index >= 0; --index)
            {
              Object @object = location.Objects[location.Objects.Keys.ElementAt<Vector2>(index)];
              if (@object.Name.Contains("Weed"))
                location.Objects.Remove(location.Objects.Keys.ElementAt<Vector2>(index));
              else if ((!@object.Name.Contains("Stump") && !@object.Name.Contains("Boulder") && (!@object.Name.Equals("Stick") && !@object.Name.Equals("Stone")) || @object.ParentSheetIndex > 100) && (location.IsOutdoors && !@object.isHoedirt))
                @object.name.Equals("HoeDirt");
            }
            foreach (WeatherDebris weatherDebris in Game1.debrisWeather)
              weatherDebris.which = 3;
            Game1.eveningColor = new Color(245, 225, 170);
          }
        }
      }
    }

    private static void updateFloorInSeedShop()
    {
      GameLocation locationFromName = Game1.getLocationFromName("SeedShop");
      for (int index = 9; index < 12; ++index)
      {
        locationFromName.Map.GetLayer("Back").Tiles[index, 17] = (Tile) new StaticTile(locationFromName.Map.GetLayer("Back"), locationFromName.Map.GetTileSheet("Floors"), BlendMode.Alpha, Game1.currentFloor);
        locationFromName.Map.GetLayer("Back").Tiles[index, 18] = (Tile) new StaticTile(locationFromName.Map.GetLayer("Back"), locationFromName.Map.GetTileSheet("Floors"), BlendMode.Alpha, Game1.currentFloor);
      }
    }

    public static void pauseThenMessage(int millisecondsPause, string message, bool showProgressBar)
    {
      Game1.messageAfterPause = message;
      Game1.pauseTime = (float) millisecondsPause;
      Game1.progressBar = showProgressBar;
    }

    public static void updateWallpaperInFarmHouse(int wallpaper)
    {
      GameLocation locationFromName = Game1.getLocationFromName("FarmHouse");
      PropertyValue propertyValue;
      locationFromName.Map.Properties.TryGetValue("Wallpaper", out propertyValue);
      if (propertyValue == null)
        return;
      string[] strArray = propertyValue.ToString().Split(' ');
      int index1 = 0;
      while (index1 < strArray.Length)
      {
        int int32_1 = Convert.ToInt32(strArray[index1]);
        int int32_2 = Convert.ToInt32(strArray[index1 + 1]);
        int int32_3 = Convert.ToInt32(strArray[index1 + 2]);
        int int32_4 = Convert.ToInt32(strArray[index1 + 3]);
        for (int index2 = int32_1; index2 < int32_1 + int32_3; ++index2)
        {
          for (int index3 = int32_2; index3 < int32_2 + int32_4; ++index3)
            locationFromName.Map.GetLayer("Back").Tiles[index2, index3] = (Tile) new StaticTile(locationFromName.Map.GetLayer("Back"), locationFromName.Map.GetTileSheet("Wallpapers"), BlendMode.Alpha, wallpaper);
        }
        index1 += 4;
      }
    }

    public static void updateFloorInFarmHouse(int floor)
    {
      GameLocation locationFromName = Game1.getLocationFromName("FarmHouse");
      PropertyValue propertyValue;
      locationFromName.Map.Properties.TryGetValue("Floor", out propertyValue);
      if (propertyValue == null)
        return;
      string[] strArray = propertyValue.ToString().Split(' ');
      int index1 = 0;
      while (index1 < strArray.Length)
      {
        int int32_1 = Convert.ToInt32(strArray[index1]);
        int int32_2 = Convert.ToInt32(strArray[index1 + 1]);
        int int32_3 = Convert.ToInt32(strArray[index1 + 2]);
        int int32_4 = Convert.ToInt32(strArray[index1 + 3]);
        for (int index2 = int32_1; index2 < int32_1 + int32_3; ++index2)
        {
          for (int index3 = int32_2; index3 < int32_2 + int32_4; ++index3)
            locationFromName.Map.GetLayer("Back").Tiles[index2, index3] = (Tile) new StaticTile(locationFromName.Map.GetLayer("Back"), locationFromName.Map.GetTileSheet("Floors"), BlendMode.Alpha, floor);
        }
        index1 += 4;
      }
    }

    public static bool shouldTimePass()
    {
      if ((Game1.IsMultiplayer || Game1.player.CanMove || (Game1.player.usingTool || Game1.player.forceTimePass)) && (!Game1.paused && !Game1.isFestival() && !Game1.freezeControls) && (Game1.activeClickableMenu == null || Game1.activeClickableMenu is BobberBar))
        return Game1.overlayMenu == null;
      return false;
    }

    public static void UpdateViewPort(bool overrideFreeze, Point centerPoint)
    {
      Game1.previousViewportPosition.X = (float) Game1.viewport.X;
      Game1.previousViewportPosition.Y = (float) Game1.viewport.Y;
      if (!Game1.viewportFreeze | overrideFreeze)
      {
        bool flag = (double) Math.Abs(Game1.currentViewportTarget.X + (float) (Game1.viewport.Width / 2) - (float) Game1.player.getStandingX()) > (double) Game1.tileSize || (double) Math.Abs(Game1.currentViewportTarget.Y + (float) (Game1.viewport.Height / 2) - (float) Game1.player.getStandingY()) > (double) Game1.tileSize;
        if (centerPoint.X >= Game1.viewport.Width / 2 && centerPoint.X <= Game1.currentLocation.Map.DisplayWidth - Game1.viewport.Width / 2)
        {
          if (Game1.player.isRafting | flag)
            Game1.currentViewportTarget.X = (float) (centerPoint.X - Game1.viewport.Width / 2);
          else if ((double) Math.Abs(Game1.currentViewportTarget.X - (Game1.currentViewportTarget.X = (float) (centerPoint.X - Game1.viewport.Width / 2))) > (double) Game1.player.getMovementSpeed())
            Game1.currentViewportTarget.X += (float) Math.Sign(Game1.currentViewportTarget.X - (Game1.currentViewportTarget.X = (float) (centerPoint.X - Game1.viewport.Width / 2))) * Game1.player.getMovementSpeed();
        }
        else if (centerPoint.X < Game1.viewport.Width / 2 && Game1.viewport.Width <= Game1.currentLocation.Map.DisplayWidth)
        {
          if (Game1.player.isRafting | flag)
            Game1.currentViewportTarget.X = 0.0f;
          else if ((double) Math.Abs(Game1.currentViewportTarget.X - 0.0f) > (double) Game1.player.getMovementSpeed())
            Game1.currentViewportTarget.X -= (float) Math.Sign(Game1.currentViewportTarget.X - 0.0f) * Game1.player.getMovementSpeed();
        }
        else if (Game1.viewport.Width <= Game1.currentLocation.Map.DisplayWidth)
        {
          if (Game1.player.isRafting | flag)
            Game1.currentViewportTarget.X = (float) (Game1.currentLocation.Map.DisplayWidth - Game1.viewport.Width);
          else if ((double) Math.Abs(Game1.currentViewportTarget.X - (float) (Game1.currentLocation.Map.DisplayWidth - Game1.viewport.Width)) > (double) Game1.player.getMovementSpeed())
            Game1.currentViewportTarget.X += (float) Math.Sign(Game1.currentViewportTarget.X - (float) (Game1.currentLocation.Map.DisplayWidth - Game1.viewport.Width)) * Game1.player.getMovementSpeed();
        }
        else if (Game1.currentLocation.Map.DisplayWidth < Game1.viewport.Width)
        {
          if (Game1.player.isRafting | flag)
            Game1.currentViewportTarget.X = (float) ((Game1.currentLocation.Map.DisplayWidth - Game1.viewport.Width) / 2);
          else if ((double) Math.Abs(Game1.currentViewportTarget.X - (float) ((Game1.currentLocation.Map.DisplayWidth - Game1.viewport.Width) / 2)) > (double) Game1.player.getMovementSpeed())
            Game1.currentViewportTarget.X -= (float) Math.Sign(Game1.currentViewportTarget.X - (float) ((Game1.currentLocation.Map.DisplayWidth - Game1.viewport.Width) / 2)) * Game1.player.getMovementSpeed();
        }
        if (centerPoint.Y >= Game1.viewport.Height / 2 && centerPoint.Y <= Game1.currentLocation.Map.DisplayHeight - Game1.viewport.Height / 2)
        {
          if (Game1.player.isRafting | flag)
            Game1.currentViewportTarget.Y = (float) (centerPoint.Y - Game1.viewport.Height / 2);
          else if ((double) Math.Abs(Game1.currentViewportTarget.Y - (float) (centerPoint.Y - Game1.viewport.Height / 2)) >= (double) Game1.player.getMovementSpeed())
            Game1.currentViewportTarget.Y -= (float) Math.Sign(Game1.currentViewportTarget.Y - (float) (centerPoint.Y - Game1.viewport.Height / 2)) * Game1.player.getMovementSpeed();
        }
        else if (centerPoint.Y < Game1.viewport.Height / 2 && Game1.viewport.Height <= Game1.currentLocation.Map.DisplayHeight)
        {
          if (Game1.player.isRafting | flag)
            Game1.currentViewportTarget.Y = 0.0f;
          else if ((double) Math.Abs(Game1.currentViewportTarget.Y - 0.0f) > (double) Game1.player.getMovementSpeed())
            Game1.currentViewportTarget.Y -= (float) Math.Sign(Game1.currentViewportTarget.Y - 0.0f) * Game1.player.getMovementSpeed();
          Game1.currentViewportTarget.Y = 0.0f;
        }
        else if (Game1.viewport.Height <= Game1.currentLocation.Map.DisplayHeight)
        {
          if (Game1.player.isRafting | flag)
            Game1.currentViewportTarget.Y = (float) (Game1.currentLocation.Map.DisplayHeight - Game1.viewport.Height);
          else if ((double) Math.Abs(Game1.currentViewportTarget.Y - (float) (Game1.currentLocation.Map.DisplayHeight - Game1.viewport.Height)) > (double) Game1.player.getMovementSpeed())
            Game1.currentViewportTarget.Y -= (float) Math.Sign(Game1.currentViewportTarget.Y - (float) (Game1.currentLocation.Map.DisplayHeight - Game1.viewport.Height)) * Game1.player.getMovementSpeed();
        }
        else if (Game1.currentLocation.Map.DisplayHeight < Game1.viewport.Height)
        {
          if (Game1.player.isRafting | flag)
            Game1.currentViewportTarget.Y = (float) ((Game1.currentLocation.Map.DisplayHeight - Game1.viewport.Height) / 2);
          else if ((double) Math.Abs(Game1.currentViewportTarget.Y - (float) ((Game1.currentLocation.Map.DisplayHeight - Game1.viewport.Height) / 2)) > (double) Game1.player.getMovementSpeed())
            Game1.currentViewportTarget.Y -= (float) Math.Sign(Game1.currentViewportTarget.Y - (float) ((Game1.currentLocation.Map.DisplayHeight - Game1.viewport.Height) / 2)) * Game1.player.getMovementSpeed();
        }
      }
      if (Game1.currentLocation.forceViewportPlayerFollow)
      {
        Game1.currentViewportTarget.X = Game1.player.position.X - (float) (Game1.viewport.Width / 2);
        Game1.currentViewportTarget.Y = Game1.player.position.Y - (float) (Game1.viewport.Height / 2);
      }
      if ((double) Game1.currentViewportTarget.X == (double) int.MinValue || Game1.viewportFreeze && !overrideFreeze)
        return;
      int num1 = (int) ((double) Game1.currentViewportTarget.X - (double) Game1.viewport.X);
      if (Math.Abs(num1) > Game1.tileSize * 2)
        Game1.viewportPositionLerp.X = Game1.currentViewportTarget.X;
      else
        Game1.viewportPositionLerp.X += (float) ((double) num1 * (double) Game1.player.getMovementSpeed() * 0.0299999993294477);
      int num2 = (int) ((double) Game1.currentViewportTarget.Y - (double) Game1.viewport.Y);
      if (Math.Abs(num2) > Game1.tileSize * 2)
        Game1.viewportPositionLerp.Y = (float) (int) Game1.currentViewportTarget.Y;
      else
        Game1.viewportPositionLerp.Y += (float) ((double) num2 * (double) Game1.player.getMovementSpeed() * 0.0299999993294477);
      Game1.viewport.X = (int) Game1.viewportPositionLerp.X;
      Game1.viewport.Y = (int) Game1.viewportPositionLerp.Y;
    }

    private void UpdateCharacters(GameTime time)
    {
      Game1.player.Update(time, Game1.currentLocation);
      foreach (KeyValuePair<long, Farmer> otherFarmer in Game1.otherFarmers)
      {
        if (otherFarmer.Key != Game1.player.uniqueMultiplayerID)
          otherFarmer.Value.UpdateIfOtherPlayer(time);
      }
    }

    public static void addMailForTomorrow(string mailName, bool noLetter = false, bool sendToEveryone = false)
    {
      mailName = mailName.Trim();
      mailName = mailName.Replace(Environment.NewLine, "");
      if (Game1.player.hasOrWillReceiveMail(mailName))
        return;
      if (noLetter)
        mailName += "%&NL&%";
      Game1.player.mailForTomorrow.Add(mailName);
      if (!sendToEveryone || !Game1.IsMultiplayer)
        return;
      MultiplayerUtility.sendMessageToEveryone(7, mailName, Game1.player.uniqueMultiplayerID);
    }

    public static void fadeScreenToBlack()
    {
      Game1.fadeToBlack = true;
      Game1.fadeIn = true;
      Game1.fadeToBlackAlpha = 0.0f;
      Game1.player.CanMove = false;
    }

    public static void drawDialogue(NPC speaker)
    {
      Game1.activeClickableMenu = (IClickableMenu) new DialogueBox(speaker.CurrentDialogue.Peek());
      Game1.dialogueUp = true;
      if (!Game1.eventUp)
      {
        Game1.player.Halt();
        Game1.player.CanMove = false;
      }
      if (speaker == null)
        return;
      Game1.currentSpeaker = speaker;
    }

    public static void drawDialogueNoTyping(NPC speaker, string dialogue)
    {
      if (speaker == null)
        Game1.currentObjectDialogue.Enqueue(dialogue);
      else if (dialogue != null)
        speaker.CurrentDialogue.Push(new Dialogue(dialogue, speaker));
      Game1.activeClickableMenu = (IClickableMenu) new DialogueBox(speaker.CurrentDialogue.Peek());
      Game1.dialogueUp = true;
      Game1.player.CanMove = false;
      if (speaker == null)
        return;
      Game1.currentSpeaker = speaker;
    }

    public static void multipleDialogues(string[] messages)
    {
      Game1.activeClickableMenu = (IClickableMenu) new DialogueBox(((IEnumerable<string>) messages).ToList<string>());
      Game1.dialogueUp = true;
      Game1.player.CanMove = false;
    }

    public static void drawDialogueNoTyping(string dialogue)
    {
      Game1.drawObjectDialogue(dialogue);
      if (Game1.activeClickableMenu == null || !(Game1.activeClickableMenu is DialogueBox))
        return;
      (Game1.activeClickableMenu as DialogueBox).finishTyping();
    }

    public static void drawDialogue(NPC speaker, string dialogue)
    {
      speaker.CurrentDialogue.Push(new Dialogue(dialogue, speaker));
      Game1.drawDialogue(speaker);
    }

    public static void drawItemNumberSelection(string itemType, int price)
    {
      Game1.selectedItemsType = itemType;
      Game1.numberOfSelectedItems = 0;
      Game1.priceOfSelectedItem = price;
      if (itemType.Equals("calicoJackBet"))
        Game1.currentObjectDialogue.Enqueue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2946", (object) Game1.player.clubCoins));
      else if (itemType.Equals("flutePitch"))
      {
        Game1.currentObjectDialogue.Enqueue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2949"));
        Game1.numberOfSelectedItems = (int) Game1.currentLocation.actionObjectForQuestionDialogue.scale.X / 100;
      }
      else if (itemType.Equals("drumTone"))
      {
        Game1.currentObjectDialogue.Enqueue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2951"));
        Game1.numberOfSelectedItems = (int) Game1.currentLocation.actionObjectForQuestionDialogue.scale.X;
      }
      else if (itemType.Equals("jukebox"))
        Game1.currentObjectDialogue.Enqueue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2953"));
      else if (itemType.Equals("Fuel"))
        Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2955"));
      else if (Game1.currentSpeaker != null)
        Game1.setDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2956"), false);
      else
        Game1.currentObjectDialogue.Enqueue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2957"));
    }

    public static void setDialogue(string dialogue, bool typing)
    {
      if (Game1.currentSpeaker != null)
      {
        Game1.currentSpeaker.CurrentDialogue.Peek().setCurrentDialogue(dialogue);
        if (typing)
          Game1.drawDialogue(Game1.currentSpeaker);
        else
          Game1.drawDialogueNoTyping(Game1.currentSpeaker, (string) null);
      }
      else if (typing)
        Game1.drawObjectDialogue(dialogue);
      else
        Game1.drawDialogueNoTyping(dialogue);
    }

    private static void checkIfDialogueIsQuestion()
    {
      if (Game1.currentSpeaker == null || Game1.currentSpeaker.CurrentDialogue.Count <= 0 || !Game1.currentSpeaker.CurrentDialogue.Peek().isCurrentDialogueAQuestion())
        return;
      Game1.questionChoices.Clear();
      Game1.isQuestion = true;
      List<NPCDialogueResponse> npcResponseOptions = Game1.currentSpeaker.CurrentDialogue.Peek().getNPCResponseOptions();
      for (int index = 0; index < npcResponseOptions.Count; ++index)
        Game1.questionChoices.Add((Response) npcResponseOptions[index]);
    }

    public static void drawLetterMessage(string message)
    {
      Game1.activeClickableMenu = (IClickableMenu) new LetterViewerMenu(message);
    }

    public static void drawObjectDialogue(string dialogue)
    {
      if (Game1.activeClickableMenu != null)
        Game1.activeClickableMenu.emergencyShutDown();
      Game1.activeClickableMenu = (IClickableMenu) new DialogueBox(dialogue);
      Game1.player.CanMove = false;
      Game1.dialogueUp = true;
    }

    public static void drawObjectQuestionDialogue(string dialogue, List<Response> choices, int width)
    {
      Game1.activeClickableMenu = (IClickableMenu) new DialogueBox(dialogue, choices, width);
      Game1.dialogueUp = true;
      Game1.player.CanMove = false;
    }

    public static void drawObjectQuestionDialogue(string dialogue, List<Response> choices)
    {
      Game1.activeClickableMenu = (IClickableMenu) new DialogueBox(dialogue, choices, 1200);
      Game1.dialogueUp = true;
      Game1.player.CanMove = false;
    }

    public static bool IsSummer
    {
      get
      {
        return Game1.currentSeason.Equals("summer");
      }
    }

    public static bool IsSpring
    {
      get
      {
        return Game1.currentSeason.Equals("spring");
      }
    }

    public static bool IsFall
    {
      get
      {
        return Game1.currentSeason.Equals("fall");
      }
    }

    public static bool IsWinter
    {
      get
      {
        return Game1.currentSeason.Equals("winter");
      }
    }

    public static void removeThisCharacterFromAllLocations(NPC toDelete)
    {
      for (int index = 0; index < Game1.locations.Count; ++index)
      {
        if (Game1.locations[index].characters.Contains(toDelete))
          Game1.locations[index].characters.Remove(toDelete);
      }
    }

    public static void warpCharacter(NPC character, string targetLocationName, Point location, bool returnToDefault, bool wasOutdoors)
    {
      Game1.warpCharacter(character, targetLocationName, new Vector2((float) location.X, (float) location.Y), returnToDefault, wasOutdoors);
    }

    public static void warpCharacter(NPC character, string targetLocationName, Vector2 location, bool returnToDefault, bool wasOutdoors)
    {
      for (int index = 0; index < Game1.locations.Count; ++index)
      {
        if (Game1.locations[index].Name.Equals(targetLocationName))
        {
          if (!Game1.locations[index].characters.Contains(character))
            Game1.locations[index].addCharacter(character);
          character.isCharging = false;
          character.speed = 2;
          character.blockedInterval = 0;
          string str = character.name;
          bool flag = false;
          if (character.name.Equals("Maru"))
          {
            if (targetLocationName.Equals("Hospital"))
            {
              str = character.name + "_" + Game1.locations[index].Name;
              flag = true;
            }
            else if (targetLocationName.Equals("Town"))
            {
              str = character.name;
              flag = true;
            }
          }
          else if (character.name.Equals("Shane"))
          {
            if (targetLocationName.Equals("JojaMart"))
            {
              str = character.name + "_" + Game1.locations[index].Name;
              flag = true;
            }
            else if (targetLocationName.Equals("Town"))
            {
              str = character.name;
              flag = true;
            }
          }
          if (flag)
            character.Sprite.Texture = Game1.content.Load<Texture2D>("Characters\\" + str);
          character.position.X = location.X * (float) Game1.tileSize;
          character.position.Y = location.Y * (float) Game1.tileSize;
          if (character.CurrentDialogue.Count > 0 && character.CurrentDialogue.Peek().removeOnNextMove && !character.getTileLocation().Equals(character.DefaultPosition / (float) Game1.tileSize))
            character.CurrentDialogue.Pop();
          if (Game1.locations[index] is FarmHouse)
            character.arriveAtFarmHouse();
          else
            character.arriveAt(Game1.locations[index]);
          if (character.currentLocation != null && !character.currentLocation.Equals((object) Game1.locations[index]))
            character.currentLocation.characters.Remove(character);
          character.currentLocation = Game1.locations[index];
          break;
        }
      }
    }

    public static void warpFarmer(string locationName, int tileX, int tileY, bool flip)
    {
      Game1.warpFarmer(Game1.getLocationFromName(locationName), tileX, tileY, flip ? (Game1.player.facingDirection + 2) % 4 : Game1.player.facingDirection, false);
    }

    public static void warpFarmer(string locationName, int tileX, int tileY, int facingDirectionAfterWarp)
    {
      Game1.warpFarmer(Game1.getLocationFromName(locationName), tileX, tileY, facingDirectionAfterWarp, false);
    }

    public static void warpFarmer(GameLocation locationAfterWarp, int tileX, int tileY, int facingDirectionAfterWarp, bool isStructure)
    {
      if (Game1.weatherIcon == 1 && Game1.whereIsTodaysFest != null && locationAfterWarp.name.Equals(Game1.whereIsTodaysFest))
      {
        if (Game1.timeOfDay < Convert.ToInt32(Game1.temporaryContent.Load<Dictionary<string, string>>("Data\\Festivals\\" + Game1.currentSeason + (object) Game1.dayOfMonth)["conditions"].Split('/')[1].Split(' ')[0]))
        {
          Game1.player.position = Game1.player.lastPosition;
          Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2973"));
          return;
        }
      }
      Game1.player.previousLocationName = Game1.player.currentLocation.name;
      Game1.locationAfterWarp = locationAfterWarp;
      Game1.xLocationAfterWarp = tileX;
      Game1.yLocationAfterWarp = tileY;
      Game1.facingDirectionAfterWarp = facingDirectionAfterWarp;
      Game1.fadeScreenToBlack();
      Game1.setRichPresence("location", (object) locationAfterWarp.Name);
    }

    public static void changeInvisibility(string name, bool invisibility)
    {
      Game1.getCharacterFromName(name, false).isInvisible = invisibility;
    }

    public static NPC getCharacterFromName(string name, bool mustBeVillager = false)
    {
      if (Game1.currentLocation != null)
      {
        foreach (NPC character in Game1.currentLocation.getCharacters())
        {
          if (character.name.Equals(name) && (!mustBeVillager || character.isVillager()))
            return character;
        }
      }
      for (int index = 0; index < Game1.locations.Count; ++index)
      {
        foreach (NPC character in Game1.locations[index].getCharacters())
        {
          if (character.name.Equals(name) && (!mustBeVillager || character.isVillager()))
            return character;
        }
      }
      if (Game1.mine != null)
      {
        foreach (NPC character in Game1.mine.getCharacters())
        {
          if (character.name.Equals(name) && (!mustBeVillager || character.isVillager()))
            return character;
        }
      }
      if (Game1.getFarm() != null)
      {
        foreach (Building building in Game1.getFarm().buildings)
        {
          if (building.indoors != null)
          {
            foreach (NPC character in building.indoors.characters)
            {
              if (character.name.Equals(name) && (!mustBeVillager || character.isVillager()))
                return character;
            }
          }
        }
      }
      return (NPC) null;
    }

    public static NPC removeCharacterFromItsLocation(string name)
    {
      for (int index1 = 0; index1 < Game1.locations.Count; ++index1)
      {
        for (int index2 = 0; index2 < Game1.locations[index1].getCharacters().Count; ++index2)
        {
          if (Game1.locations[index1].getCharacters()[index2].name.Equals(name))
          {
            NPC character = Game1.locations[index1].characters[index2];
            Game1.locations[index1].characters.RemoveAt(index2);
            return character;
          }
        }
      }
      return (NPC) null;
    }

    public static GameLocation getLocationFromName(string name)
    {
      return Game1.getLocationFromName(name, false);
    }

    public static GameLocation getLocationFromName(string name, bool isStructure)
    {
      if (name == null)
        return (GameLocation) null;
      for (int index1 = 0; index1 < Game1.locations.Count; ++index1)
      {
        if (!isStructure)
        {
          if (string.Equals(Game1.locations[index1].Name, name, StringComparison.OrdinalIgnoreCase))
            return Game1.locations[index1];
        }
        else if (Game1.locations[index1] is Farm)
        {
          for (int index2 = 0; index2 < (Game1.locations[index1] as Farm).buildings.Count; ++index2)
          {
            if ((Game1.locations[index1] as Farm).buildings.ElementAt<Building>(index2).nameOfIndoors.Equals(name))
              return (Game1.locations[index1] as Farm).buildings.ElementAt<Building>(index2).indoors;
          }
        }
      }
      if (string.Equals(name, "UndergroundMine", StringComparison.OrdinalIgnoreCase))
        return (GameLocation) Game1.mine;
      if (!isStructure)
        return Game1.getLocationFromName(name, true);
      return (GameLocation) null;
    }

    public static void addNewFarmBuildingMaps()
    {
      if (Game1.player.CoopUpgradeLevel >= 1 && Game1.getLocationFromName("Coop") == null)
      {
        Game1.locations.Add(new GameLocation(Game1.content.Load<Map>("Maps\\Coop" + (object) Game1.player.CoopUpgradeLevel), "Coop"));
        Game1.getLocationFromName("Farm").setTileProperty(21, 10, "Buildings", "Action", "Warp 2 9 Coop");
        Game1.currentCoopTexture = Game1.content.Load<Texture2D>("BuildingUpgrades\\Coop" + (object) Game1.player.coopUpgradeLevel);
      }
      else if (Game1.getLocationFromName("Coop") != null)
      {
        Game1.getLocationFromName("Coop").map = Game1.content.Load<Map>("Maps\\Coop" + (object) Game1.player.CoopUpgradeLevel);
        Game1.currentCoopTexture = Game1.content.Load<Texture2D>("BuildingUpgrades\\Coop" + (object) Game1.player.coopUpgradeLevel);
      }
      if (Game1.player.BarnUpgradeLevel >= 1 && Game1.getLocationFromName("Barn") == null)
      {
        Game1.locations.Add(new GameLocation(Game1.content.Load<Map>("Maps\\Barn" + (object) Game1.player.BarnUpgradeLevel), "Barn"));
        Game1.getLocationFromName("Farm").warps.Add(new Warp(14, 9, "Barn", 11, 14, false));
        Game1.currentBarnTexture = Game1.content.Load<Texture2D>("BuildingUpgrades\\Barn" + (object) Game1.player.barnUpgradeLevel);
      }
      else if (Game1.getLocationFromName("Barn") != null)
      {
        Game1.getLocationFromName("Barn").map = Game1.content.Load<Map>("Maps\\Barn" + (object) Game1.player.BarnUpgradeLevel);
        Game1.currentBarnTexture = Game1.content.Load<Texture2D>("BuildingUpgrades\\Barn" + (object) Game1.player.barnUpgradeLevel);
      }
      if (Game1.player.HouseUpgradeLevel >= 1 && Game1.getLocationFromName("FarmHouse").Map.Id.Equals("FarmHouse"))
      {
        Game1.getLocationFromName("FarmHouse").Map = Game1.content.Load<Map>("Maps\\FarmHouse" + (object) Game1.player.HouseUpgradeLevel);
        Game1.getLocationFromName("FarmHouse").Map.LoadTileSheets(Game1.mapDisplayDevice);
        int currentWallpaper = Game1.currentWallpaper;
        int currentFloor = Game1.currentFloor;
        Game1.currentWallpaper = Game1.farmerWallpaper;
        Game1.currentFloor = Game1.FarmerFloor;
        Game1.updateFloorInFarmHouse(Game1.currentFloor);
        Game1.updateWallpaperInFarmHouse(Game1.currentWallpaper);
        Game1.currentWallpaper = currentWallpaper;
        Game1.currentFloor = currentFloor;
      }
      if (!Game1.player.hasGreenhouse || Game1.getLocationFromName("FarmGreenHouse") != null)
        return;
      Game1.locations.Add(new GameLocation(Game1.content.Load<Map>("Maps\\FarmGreenHouse"), "FarmGreenHouse"));
      Game1.getLocationFromName("Farm").setTileProperty(3, 10, "Buildings", "Action", "Warp 5 15 FarmGreenHouse");
      Game1.greenhouseTexture = Game1.content.Load<Texture2D>("BuildingUpgrades\\Greenhouse");
    }

    public static void NewDay(float timeToPause)
    {
      Game1.nonWarpFade = true;
      Game1.fadeScreenToBlack();
      Game1.newDay = true;
      Game1.player.Halt();
      Game1.player.currentEyes = 1;
      Game1.player.blinkTimer = -4000;
      Game1.player.CanMove = false;
      Game1.pauseTime = timeToPause;
    }

    public static void setUpSpouse()
    {
      NPC characterFromName = Game1.getCharacterFromName(Game1.player.spouse, false);
      Game1.getLocationFromName(characterFromName.DefaultMap).characters.Remove(Game1.getLocationFromName(characterFromName.DefaultMap).getCharacterFromName(Game1.player.spouse));
      characterFromName.Schedule = (Dictionary<int, SchedulePathDescription>) null;
      characterFromName.CurrentDialogue.Clear();
      characterFromName.DefaultMap = "FarmHouse";
      characterFromName.DefaultPosition = new Vector2((float) (9 * Game1.tileSize), (float) (4 * Game1.tileSize - Game1.tileSize));
      characterFromName.DefaultFacingDirection = 0;
      Game1.getLocationFromName("FarmHouse").characters.Add(characterFromName);
      characterFromName.position = new Vector2((float) (9 * Game1.tileSize), (float) (4 * Game1.tileSize - Game1.tileSize));
      characterFromName.faceDirection(2);
    }

    public static void screenGlowOnce(Color glowColor, bool hold, float rate = 0.005f, float maxAlpha = 0.3f)
    {
      Game1.screenGlowMax = maxAlpha;
      Game1.screenGlowRate = rate;
      Game1.screenGlowAlpha = 0.0f;
      Game1.screenGlowUp = true;
      Game1.screenGlowColor = glowColor;
      Game1.screenGlow = true;
      Game1.screenGlowHold = hold;
    }

    public static void removeTilesFromLayer(GameLocation l, string layer, Microsoft.Xna.Framework.Rectangle area)
    {
      for (int x = area.X; x < area.Right; ++x)
      {
        for (int y = area.Y; y < area.Bottom; ++y)
          l.Map.GetLayer(layer).Tiles[x, y] = (Tile) null;
      }
    }

    public static void removeFrontLayerForFarmBuildings()
    {
      GameLocation locationFromName = Game1.getLocationFromName("Farm");
      if (Game1.player.CoopUpgradeLevel > 0)
        Game1.removeTilesFromLayer(locationFromName, "Front", new Microsoft.Xna.Framework.Rectangle(20, 5, 4, 6));
      switch (Game1.player.BarnUpgradeLevel)
      {
        case 1:
          Game1.removeTilesFromLayer(locationFromName, "Front", new Microsoft.Xna.Framework.Rectangle(12, 5, 5, 6));
          break;
        case 2:
          Game1.removeTilesFromLayer(locationFromName, "Front", new Microsoft.Xna.Framework.Rectangle(9, 4, 8, 7));
          break;
      }
      switch (Game1.player.HouseUpgradeLevel)
      {
        case 1:
          Game1.removeTilesFromLayer(locationFromName, "Front", new Microsoft.Xna.Framework.Rectangle(31, 6, 5, 5));
          break;
        case 2:
          Game1.removeTilesFromLayer(locationFromName, "Front", new Microsoft.Xna.Framework.Rectangle(31, 5, 5, 6));
          break;
        case 3:
          Game1.removeTilesFromLayer(locationFromName, "Front", new Microsoft.Xna.Framework.Rectangle(31, 5, 5, 6));
          break;
      }
    }

    public static string shortDayNameFromDayOfSeason(int dayOfSeason)
    {
      switch (dayOfSeason % 7)
      {
        case 0:
          return "Sun";
        case 1:
          return "Mon";
        case 2:
          return "Tue";
        case 3:
          return "Wed";
        case 4:
          return "Thu";
        case 5:
          return "Fri";
        case 6:
          return "Sat";
        default:
          return "";
      }
    }

    public static string shortDayDisplayNameFromDayOfSeason(int dayOfSeason)
    {
      switch (dayOfSeason % 7)
      {
        case 0:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3042");
        case 1:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3043");
        case 2:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3044");
        case 3:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3045");
        case 4:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3046");
        case 5:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3047");
        case 6:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3048");
        default:
          return "";
      }
    }

    public static void shipObject(Object item)
    {
      bool flag = false;
      for (int index = 0; index < Game1.shippingBin.Count; ++index)
      {
        if (Game1.shippingBin[index].Name.Equals(item.Name))
        {
          ++Game1.shippingBin[index].Stack;
          flag = true;
          break;
        }
      }
      if (!flag)
        Game1.shippingBin.Add(item);
      if (!item.type.Equals("Basic"))
        return;
      Game1.player.shippedBasic(item.ParentSheetIndex, 1);
    }

    public static void shipHeldItem()
    {
      if (Game1.player.ActiveObject != null)
      {
        Game1.shipObject((Object) Game1.player.ActiveObject.getOne());
        Game1.playSound("Ship");
      }
      else if (Game1.player.numberOfItemsInInventory() > 0)
        Game1.currentLocation.createQuestionDialogue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3051")), Game1.currentLocation.createYesNoResponses(), "Shipping");
      else
        Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3053"));
    }

    public static void showNameSelectScreen(string type)
    {
      Game1.nameSelectType = type;
      Game1.nameSelectUp = true;
    }

    public static void nameSelectionDone()
    {
    }

    public static void tryToBuySelectedItems()
    {
      if (Game1.selectedItemsType.Equals("flutePitch"))
      {
        Game1.currentObjectDialogue.Clear();
        Game1.currentLocation.actionObjectForQuestionDialogue.scale.X = (float) (Game1.numberOfSelectedItems * 100);
        Game1.dialogueUp = false;
        Game1.player.CanMove = true;
        Game1.numberOfSelectedItems = -1;
      }
      else if (Game1.selectedItemsType.Equals("drumTone"))
      {
        Game1.currentObjectDialogue.Clear();
        Game1.currentLocation.actionObjectForQuestionDialogue.scale.X = (float) Game1.numberOfSelectedItems;
        Game1.dialogueUp = false;
        Game1.player.CanMove = true;
        Game1.numberOfSelectedItems = -1;
      }
      else if (Game1.selectedItemsType.Equals("jukebox"))
      {
        Game1.changeMusicTrack(Game1.player.songsHeard.ElementAt<string>(Game1.numberOfSelectedItems));
        Game1.dialogueUp = false;
        Game1.player.CanMove = true;
        Game1.numberOfSelectedItems = -1;
      }
      else if (Game1.player.Money >= Game1.priceOfSelectedItem * Game1.numberOfSelectedItems && Game1.numberOfSelectedItems > 0)
      {
        bool flag = true;
        string selectedItemsType = Game1.selectedItemsType;
        if (!(selectedItemsType == "Animal Food"))
        {
          if (!(selectedItemsType == "Fuel"))
          {
            if (selectedItemsType == "Star Token")
            {
              Game1.player.festivalScore += Game1.numberOfSelectedItems;
              Game1.dialogueUp = false;
              Game1.player.canMove = true;
            }
          }
          else
            ((Lantern) Game1.player.getToolFromName("Lantern")).fuelLeft += Game1.numberOfSelectedItems;
        }
        else
        {
          Game1.player.Feed += Game1.numberOfSelectedItems;
          Game1.setDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3072"), false);
        }
        if (!flag)
          return;
        Game1.player.Money -= Game1.priceOfSelectedItem * Game1.numberOfSelectedItems;
        Game1.numberOfSelectedItems = -1;
        Game1.playSound("purchase");
      }
      else
      {
        if (Game1.player.Money >= Game1.priceOfSelectedItem * Game1.numberOfSelectedItems)
          return;
        Game1.currentObjectDialogue.Dequeue();
        Game1.setDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3078"), false);
        Game1.numberOfSelectedItems = -1;
      }
    }

    public static void throwActiveObjectDown()
    {
      Game1.player.CanMove = false;
      switch (Game1.player.FacingDirection)
      {
        case 0:
          ((FarmerSprite) Game1.player.Sprite).animateBackwardsOnce(80, 50f);
          break;
        case 1:
          ((FarmerSprite) Game1.player.Sprite).animateBackwardsOnce(72, 50f);
          break;
        case 2:
          ((FarmerSprite) Game1.player.Sprite).animateBackwardsOnce(64, 50f);
          break;
        case 3:
          ((FarmerSprite) Game1.player.Sprite).animateBackwardsOnce(88, 50f);
          break;
      }
      Game1.player.reduceActiveItemByOne();
      Game1.playSound("throwDownITem");
    }

    public static void changeMusicTrack(string newTrackName)
    {
      if (!Game1.player.songsHeard.Contains(newTrackName))
        Utility.farmerHeardSong(newTrackName);
      if (Game1.currentSong != null && !Game1.currentSong.IsStopped && Game1.currentSong.Name.Equals(newTrackName))
        return;
      Game1.nextMusicTrack = newTrackName;
    }

    public static void doneEating()
    {
      Game1.isEating = false;
      Game1.player.completelyStopAnimatingOrDoingAction();
      Game1.player.forceCanMove();
      if (Game1.player.mostRecentlyGrabbedItem == null)
        return;
      Object itemToEat = Game1.player.itemToEat as Object;
      if (itemToEat.ParentSheetIndex == 434)
      {
        if (Utility.foundAllStardrops())
          Game1.getSteamAchievement("Achievement_Stardrop");
        Game1.player.yOffset = 0.0f;
        Game1.player.yJumpOffset = 0;
        Game1.changeMusicTrack("none");
        Game1.playSound("stardrop");
        string str = Game1.random.NextDouble() < 0.5 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3094") : Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3095");
        if (Game1.player.favoriteThing.Contains("Stardew"))
          str = Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3097");
        if (Game1.player.favoriteThing.Equals("ConcernedApe"))
          str = Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3099");
        DelayedAction.showDialogueAfterDelay(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3100") + str + Game1.player.favoriteThing + Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3101"), 6000);
        Game1.player.MaxStamina += 34;
        Game1.player.Stamina = (float) Game1.player.MaxStamina;
        Game1.player.FarmerSprite.animateOnce(new FarmerSprite.AnimationFrame[1]
        {
          new FarmerSprite.AnimationFrame(57, 6000)
        });
        Game1.player.startGlowing(new Color(200, 0, (int) byte.MaxValue), false, 0.1f);
        Game1.player.jitterStrength = 1f;
        Game1.staminaShakeTimer = 12000;
        Game1.screenGlowOnce(new Color(200, 0, (int) byte.MaxValue), true, 0.005f, 0.3f);
        Game1.player.CanMove = false;
        Game1.player.freezePause = 8000;
        List<TemporaryAnimatedSprite> temporarySprites = Game1.currentLocation.temporarySprites;
        TemporaryAnimatedSprite temporaryAnimatedSprite1 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(368, 16, 16, 16), 60f, 8, 40, Game1.player.position + new Vector2((float) (-Game1.pixelZoom * 2), (float) (-Game1.tileSize * 2)), false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0075f, 0.0f, 0.0f, false);
        temporaryAnimatedSprite1.alpha = 0.75f;
        temporaryAnimatedSprite1.alphaFade = 1f / 400f;
        Vector2 vector2 = new Vector2(0.0f, -0.25f);
        temporaryAnimatedSprite1.motion = vector2;
        temporarySprites.Add(temporaryAnimatedSprite1);
        for (int index = 0; index < 40; ++index)
        {
          List<TemporaryAnimatedSprite> overlayTempSprites = Game1.screenOverlayTempSprites;
          int rowInAnimationTexture = Game1.random.Next(10, 12);
          Viewport viewport = Game1.graphics.GraphicsDevice.Viewport;
          double num1 = (double) (viewport.TitleSafeArea.Right - 48 - Game1.tileSize / 8 - Game1.random.Next(Game1.tileSize));
          int num2 = Game1.random.Next(-Game1.tileSize, Game1.tileSize);
          viewport = Game1.graphics.GraphicsDevice.Viewport;
          int bottom = viewport.TitleSafeArea.Bottom;
          double num3 = (double) (num2 + bottom - 224 - Game1.tileSize / 4 - (int) ((double) (Game1.player.MaxStamina - 270) * 0.715));
          Vector2 position = new Vector2((float) num1, (float) num3);
          Color color = Game1.random.NextDouble() < 0.5 ? Color.White : Color.Lime;
          int animationLength = 8;
          int num4 = 0;
          double num5 = 50.0;
          int numberOfLoops = 0;
          int sourceRectWidth = -1;
          double num6 = -1.0;
          int sourceRectHeight = -1;
          int delay = 0;
          TemporaryAnimatedSprite temporaryAnimatedSprite2 = new TemporaryAnimatedSprite(rowInAnimationTexture, position, color, animationLength, num4 != 0, (float) num5, numberOfLoops, sourceRectWidth, (float) num6, sourceRectHeight, delay);
          temporaryAnimatedSprite2.layerDepth = 1f;
          int num7 = 200 * index;
          temporaryAnimatedSprite2.delayBeforeAnimationStart = num7;
          double num8 = 100.0;
          temporaryAnimatedSprite2.interval = (float) num8;
          int num9 = 1;
          temporaryAnimatedSprite2.local = num9 != 0;
          overlayTempSprites.Add(temporaryAnimatedSprite2);
        }
        Utility.addSprinklesToLocation(Game1.currentLocation, Game1.player.getTileX(), Game1.player.getTileY(), 9, 9, 6000, 100, new Color(200, 0, (int) byte.MaxValue), (string) null, true);
        DelayedAction.stopFarmerGlowing(6000);
        Utility.addSprinklesToLocation(Game1.currentLocation, Game1.player.getTileX(), Game1.player.getTileY(), 9, 9, 6000, 300, Color.Cyan, (string) null, true);
        Game1.player.mostRecentlyGrabbedItem = (Item) null;
      }
      else
      {
        string[] strArray1 = Game1.objectInformation[itemToEat.ParentSheetIndex].Split('/');
        if (Convert.ToInt32(strArray1[2]) > 0)
        {
          string[] strArray2;
          if (strArray1.Length <= 7)
            strArray2 = new string[12]
            {
              "0",
              "0",
              "0",
              "0",
              "0",
              "0",
              "0",
              "0",
              "0",
              "0",
              "0",
              "0"
            };
          else
            strArray2 = strArray1[7].Split(' ');
          string[] strArray3 = strArray2;
          if (strArray1.Length > 6 && strArray1[6].Equals("drink"))
          {
            if (!Game1.buffsDisplay.tryToAddDrinkBuff(new Buff(Convert.ToInt32(strArray3[0]), Convert.ToInt32(strArray3[1]), Convert.ToInt32(strArray3[2]), Convert.ToInt32(strArray3[3]), Convert.ToInt32(strArray3[4]), Convert.ToInt32(strArray3[5]), Convert.ToInt32(strArray3[6]), Convert.ToInt32(strArray3[7]), Convert.ToInt32(strArray3[8]), Convert.ToInt32(strArray3[9]), Convert.ToInt32(strArray3[10]), strArray3.Length > 10 ? Convert.ToInt32(strArray3[10]) : 0, strArray1.Length > 8 ? Convert.ToInt32(strArray1[8]) : -1, strArray1[0], strArray1[4])))
              ;
          }
          else if (Convert.ToInt32(strArray1[2]) > 0)
            Game1.buffsDisplay.tryToAddFoodBuff(new Buff(Convert.ToInt32(strArray3[0]), Convert.ToInt32(strArray3[1]), Convert.ToInt32(strArray3[2]), Convert.ToInt32(strArray3[3]), Convert.ToInt32(strArray3[4]), Convert.ToInt32(strArray3[5]), Convert.ToInt32(strArray3[6]), Convert.ToInt32(strArray3[7]), Convert.ToInt32(strArray3[8]), Convert.ToInt32(strArray3[9]), Convert.ToInt32(strArray3[10]), strArray3.Length > 11 ? Convert.ToInt32(strArray3[11]) : 0, strArray1.Length > 8 ? Convert.ToInt32(strArray1[8]) : -1, strArray1[0], strArray1[4]), Math.Min(120000, (int) ((double) Convert.ToInt32(strArray1[2]) / 20.0 * 30000.0)));
        }
        float stamina = Game1.player.Stamina;
        int health = Game1.player.health;
        int num = (int) Math.Ceiling((double) itemToEat.Edibility * 2.5) + itemToEat.quality * itemToEat.Edibility;
        Game1.player.Stamina = Math.Min((float) Game1.player.MaxStamina, Game1.player.Stamina + (float) num);
        Game1.player.health = Math.Min(Game1.player.maxHealth, Game1.player.health + (itemToEat.Edibility < 0 ? 0 : (int) ((double) num * 0.449999988079071)));
        if ((double) stamina < (double) Game1.player.Stamina)
          Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3116", (object) (int) ((double) Game1.player.Stamina - (double) stamina)), 4));
        if (health < Game1.player.health)
          Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3118", (object) (Game1.player.health - health)), 5));
      }
      if (itemToEat.Edibility >= 0)
        return;
      Game1.player.CanMove = false;
      ((FarmerSprite) Game1.player.Sprite).animateOnce(224, 350f, 4);
      Game1.player.doEmote(12);
    }

    public static void enterMine(bool isEnteringFromTopFloor, int whatLevel, string forceLevelType)
    {
      Game1.inMine = true;
      if (Game1.mine == null)
        Game1.mine = new MineShaft();
      Game1.mine.setNextLevel(whatLevel);
      Game1.warpFarmer("UndergroundMine", 6, 6, 2);
    }

    public static void getSteamAchievement(string which)
    {
      if (which.Equals("0"))
        which = "a0";
      Program.sdk.GetAchievement(which);
    }

    public static void getAchievement(int which)
    {
      if (Game1.player.achievements.Contains(which) || (int) Game1.gameMode != 3)
        return;
      Dictionary<int, string> dictionary = Game1.content.Load<Dictionary<int, string>>("Data\\Achievements");
      if (!dictionary.ContainsKey(which))
        return;
      string message = dictionary[which].Split('^')[0];
      Game1.player.achievements.Add(which);
      Game1.playSound("achievement");
      Program.sdk.GetAchievement(string.Concat((object) which));
      int num = 1;
      Game1.addHUDMessage(new HUDMessage(message, num != 0));
      if (Game1.player.hasOrWillReceiveMail("hatter"))
        return;
      Game1.addMailForTomorrow("hatter", false, false);
    }

    public static void createMultipleObjectDebris(int index, int xTile, int yTile, int number)
    {
      for (int index1 = 0; index1 < number; ++index1)
        Game1.createObjectDebris(index, xTile, yTile, -1, 0, 1f, (GameLocation) null);
    }

    public static void createMultipleObjectDebris(int index, int xTile, int yTile, int number, float velocityMultiplier)
    {
      for (int index1 = 0; index1 < number; ++index1)
        Game1.createObjectDebris(index, xTile, yTile, -1, 0, velocityMultiplier, (GameLocation) null);
    }

    public static void createMultipleObjectDebris(int index, int xTile, int yTile, int number, long who)
    {
      for (int index1 = 0; index1 < number; ++index1)
        Game1.createObjectDebris(index, xTile, yTile, who);
    }

    public static void createMultipleObjectDebris(int index, int xTile, int yTile, int number, long who, GameLocation location)
    {
      for (int index1 = 0; index1 < number; ++index1)
        Game1.createObjectDebris(index, xTile, yTile, who, location);
    }

    public static void createDebris(int debrisType, int xTile, int yTile, int numberOfChunks, GameLocation location = null)
    {
      if (location == null)
        location = Game1.currentLocation;
      location.debris.Add(new Debris(debrisType, numberOfChunks, new Vector2((float) (xTile * Game1.tileSize + Game1.tileSize / 2), (float) (yTile * Game1.tileSize + Game1.tileSize / 2)), new Vector2((float) Game1.player.getStandingX(), (float) Game1.player.getStandingY())));
    }

    public static void createItemDebris(Item item, Vector2 origin, int direction, GameLocation location = null)
    {
      if (location == null)
        location = Game1.currentLocation;
      if (Game1.IsMultiplayer)
      {
        int Seed = (int) (short) Game1.random.Next((int) short.MinValue, (int) short.MaxValue);
        Game1.recentMultiplayerRandom = new Random(Seed);
        Vector2 position = origin;
        int facingDirection = direction;
        Item i = item;
        long uniqueMultiplayerId = Game1.player.uniqueMultiplayerID;
        MultiplayerUtility.broadcastDebrisCreate((short) Seed, position, facingDirection, i, uniqueMultiplayerId);
      }
      Vector2 targetLocation = new Vector2(origin.X, origin.Y);
      switch (direction)
      {
        case -1:
          targetLocation = Game1.player.getStandingPosition();
          break;
        case 0:
          origin.X -= (float) (Game1.tileSize / 2);
          origin.Y -= (float) (Game1.tileSize * 2 + Game1.recentMultiplayerRandom.Next(Game1.tileSize / 2));
          targetLocation.Y -= (float) (Game1.tileSize * 3);
          break;
        case 1:
          origin.X += (float) (Game1.tileSize * 2 / 3);
          origin.Y -= (float) (Game1.tileSize / 2 - Game1.recentMultiplayerRandom.Next(Game1.tileSize / 8));
          targetLocation.X += (float) (Game1.tileSize * 4);
          break;
        case 2:
          origin.X -= (float) (Game1.tileSize / 2);
          origin.Y += (float) Game1.recentMultiplayerRandom.Next(Game1.tileSize / 2);
          targetLocation.Y += (float) (Game1.tileSize * 3 / 2);
          break;
        case 3:
          origin.X -= (float) Game1.tileSize;
          origin.Y -= (float) (Game1.tileSize / 2 - Game1.recentMultiplayerRandom.Next(Game1.tileSize / 8));
          targetLocation.X -= (float) (Game1.tileSize * 4);
          break;
      }
      location.debris.Add(new Debris(item, origin, targetLocation));
    }

    public static void createRadialDebris(GameLocation location, int debrisType, int xTile, int yTile, int numberOfChunks, bool resource, int groundLevel = -1, bool item = false, int color = -1)
    {
      if (groundLevel == -1)
        groundLevel = yTile * Game1.tileSize + Game1.tileSize / 2;
      Vector2 debrisOrigin = new Vector2((float) (xTile * Game1.tileSize + Game1.tileSize), (float) (yTile * Game1.tileSize + Game1.tileSize));
      if (item)
      {
        for (; numberOfChunks > 0; --numberOfChunks)
        {
          switch (Game1.random.Next(4))
          {
            case 0:
              location.debris.Add(new Debris((Item) new Object(Vector2.Zero, debrisType, 1), debrisOrigin, debrisOrigin + new Vector2((float) -Game1.tileSize, 0.0f)));
              break;
            case 1:
              location.debris.Add(new Debris((Item) new Object(Vector2.Zero, debrisType, 1), debrisOrigin, debrisOrigin + new Vector2((float) Game1.tileSize, 0.0f)));
              break;
            case 2:
              location.debris.Add(new Debris((Item) new Object(Vector2.Zero, debrisType, 1), debrisOrigin, debrisOrigin + new Vector2(0.0f, (float) Game1.tileSize)));
              break;
            case 3:
              location.debris.Add(new Debris((Item) new Object(Vector2.Zero, debrisType, 1), debrisOrigin, debrisOrigin + new Vector2(0.0f, (float) -Game1.tileSize)));
              break;
          }
        }
      }
      if (resource)
      {
        location.debris.Add(new Debris(debrisType, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2((float) -Game1.tileSize, 0.0f)));
        ++numberOfChunks;
        location.debris.Add(new Debris(debrisType, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2((float) Game1.tileSize, 0.0f)));
        ++numberOfChunks;
        location.debris.Add(new Debris(debrisType, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2(0.0f, (float) -Game1.tileSize)));
        ++numberOfChunks;
        location.debris.Add(new Debris(debrisType, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2(0.0f, (float) Game1.tileSize)));
      }
      else
      {
        location.debris.Add(new Debris(debrisType, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2((float) -Game1.tileSize, 0.0f), groundLevel, color));
        ++numberOfChunks;
        location.debris.Add(new Debris(debrisType, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2((float) Game1.tileSize, 0.0f), groundLevel, color));
        ++numberOfChunks;
        location.debris.Add(new Debris(debrisType, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2(0.0f, (float) -Game1.tileSize), groundLevel, color));
        ++numberOfChunks;
        location.debris.Add(new Debris(debrisType, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2(0.0f, (float) Game1.tileSize), groundLevel, color));
      }
    }

    public static void createRadialDebris(GameLocation location, Texture2D texture, Microsoft.Xna.Framework.Rectangle sourcerectangle, int xTile, int yTile, int numberOfChunks)
    {
      Game1.createRadialDebris(location, texture, sourcerectangle, xTile, yTile, numberOfChunks, yTile);
    }

    public static void createWaterDroplets(Texture2D texture, Microsoft.Xna.Framework.Rectangle sourcerectangle, int xPosition, int yPosition, int numberOfChunks, int groundLevelTile)
    {
      Vector2 debrisOrigin = new Vector2((float) xPosition, (float) yPosition);
      Game1.currentLocation.debris.Add(new Debris(texture, sourcerectangle, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2((float) -Game1.tileSize, 0.0f), groundLevelTile * Game1.tileSize));
      Game1.currentLocation.debris.Add(new Debris(texture, sourcerectangle, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2((float) Game1.tileSize, 0.0f), groundLevelTile * Game1.tileSize));
      Game1.currentLocation.debris.Add(new Debris(texture, sourcerectangle, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2(0.0f, (float) -Game1.tileSize), groundLevelTile * Game1.tileSize));
      Game1.currentLocation.debris.Add(new Debris(texture, sourcerectangle, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2(0.0f, (float) Game1.tileSize), groundLevelTile * Game1.tileSize));
    }

    public static void createRadialDebris(GameLocation location, Texture2D texture, Microsoft.Xna.Framework.Rectangle sourcerectangle, int xTile, int yTile, int numberOfChunks, int groundLevelTile)
    {
      Game1.createRadialDebris(location, texture, sourcerectangle, 8, xTile * Game1.tileSize + Game1.tileSize / 2 + Game1.random.Next(Game1.tileSize / 2), yTile * Game1.tileSize + Game1.tileSize / 2 + Game1.random.Next(Game1.tileSize / 2), numberOfChunks, groundLevelTile);
    }

    public static void createRadialDebris(GameLocation location, Texture2D texture, Microsoft.Xna.Framework.Rectangle sourcerectangle, int sizeOfSourceRectSquares, int xPosition, int yPosition, int numberOfChunks, int groundLevelTile)
    {
      Vector2 debrisOrigin = new Vector2((float) xPosition, (float) yPosition);
      location.debris.Add(new Debris(texture, sourcerectangle, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2((float) -Game1.tileSize, 0.0f), groundLevelTile * Game1.tileSize, sizeOfSourceRectSquares));
      location.debris.Add(new Debris(texture, sourcerectangle, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2((float) Game1.tileSize, 0.0f), groundLevelTile * Game1.tileSize, sizeOfSourceRectSquares));
      location.debris.Add(new Debris(texture, sourcerectangle, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2(0.0f, (float) -Game1.tileSize), groundLevelTile * Game1.tileSize, sizeOfSourceRectSquares));
      location.debris.Add(new Debris(texture, sourcerectangle, numberOfChunks / 4, debrisOrigin, debrisOrigin + new Vector2(0.0f, (float) Game1.tileSize), groundLevelTile * Game1.tileSize, sizeOfSourceRectSquares));
    }

    public static void createRadialDebris(GameLocation location, Texture2D texture, Microsoft.Xna.Framework.Rectangle sourcerectangle, int sizeOfSourceRectSquares, int xPosition, int yPosition, int numberOfChunks, int groundLevelTile, Color color)
    {
      Game1.createRadialDebris(location, texture, sourcerectangle, sizeOfSourceRectSquares, xPosition, yPosition, numberOfChunks, groundLevelTile, color, 1f);
    }

    public static void createRadialDebris(GameLocation location, Texture2D texture, Microsoft.Xna.Framework.Rectangle sourcerectangle, int sizeOfSourceRectSquares, int xPosition, int yPosition, int numberOfChunks, int groundLevelTile, Color color, float scale)
    {
      Vector2 debrisOrigin = new Vector2((float) xPosition, (float) yPosition);
      for (; numberOfChunks > 0; --numberOfChunks)
      {
        switch (Game1.random.Next(4))
        {
          case 0:
            Debris debris1 = new Debris(texture, sourcerectangle, 1, debrisOrigin, debrisOrigin + new Vector2((float) -Game1.tileSize, 0.0f), groundLevelTile * Game1.tileSize, sizeOfSourceRectSquares);
            debris1.nonSpriteChunkColor = color;
            location.debris.Add(debris1);
            debris1.Chunks[0].scale = scale;
            break;
          case 1:
            Debris debris2 = new Debris(texture, sourcerectangle, 1, debrisOrigin, debrisOrigin + new Vector2((float) Game1.tileSize, 0.0f), groundLevelTile * Game1.tileSize, sizeOfSourceRectSquares);
            debris2.nonSpriteChunkColor = color;
            location.debris.Add(debris2);
            debris2.Chunks[0].scale = scale;
            break;
          case 2:
            Debris debris3 = new Debris(texture, sourcerectangle, 1, debrisOrigin, debrisOrigin + new Vector2((float) Game1.random.Next(-Game1.tileSize, Game1.tileSize), (float) -Game1.tileSize), groundLevelTile * Game1.tileSize, sizeOfSourceRectSquares);
            debris3.nonSpriteChunkColor = color;
            location.debris.Add(debris3);
            debris3.Chunks[0].scale = scale;
            break;
          case 3:
            Debris debris4 = new Debris(texture, sourcerectangle, 1, debrisOrigin, debrisOrigin + new Vector2((float) Game1.random.Next(-Game1.tileSize, Game1.tileSize), (float) Game1.tileSize), groundLevelTile * Game1.tileSize, sizeOfSourceRectSquares);
            debris4.nonSpriteChunkColor = color;
            location.debris.Add(debris4);
            debris4.Chunks[0].scale = scale;
            break;
        }
      }
    }

    public static void createObjectDebris(int objectIndex, int xTile, int yTile, long whichPlayer)
    {
      Game1.currentLocation.debris.Add(new Debris(objectIndex, new Vector2((float) (xTile * Game1.tileSize + Game1.tileSize / 2), (float) (yTile * Game1.tileSize + Game1.tileSize / 2)), Game1.getFarmer(whichPlayer).getStandingPosition()));
    }

    public static void createObjectDebris(int objectIndex, int xTile, int yTile, long whichPlayer, GameLocation location)
    {
      location.debris.Add(new Debris(objectIndex, new Vector2((float) (xTile * Game1.tileSize + Game1.tileSize / 2), (float) (yTile * Game1.tileSize + Game1.tileSize / 2)), Game1.getFarmer(whichPlayer).getStandingPosition()));
    }

    public static void createObjectDebris(int objectIndex, int xTile, int yTile, int groundLevel = -1, int itemQuality = 0, float velocityMultiplyer = 1f, GameLocation location = null)
    {
      Debris debris = new Debris(objectIndex, new Vector2((float) (xTile * Game1.tileSize + Game1.tileSize / 2), (float) (yTile * Game1.tileSize + Game1.tileSize / 2)), new Vector2((float) Game1.player.getStandingX(), (float) Game1.player.getStandingY()))
      {
        itemQuality = itemQuality
      };
      foreach (Chunk chunk in debris.Chunks)
      {
        double num1 = (double) chunk.xVelocity * (double) velocityMultiplyer;
        chunk.xVelocity = (float) num1;
        double num2 = (double) chunk.yVelocity * (double) velocityMultiplyer;
        chunk.yVelocity = (float) num2;
      }
      if (groundLevel != -1)
        debris.chunkFinalYLevel = groundLevel;
      (location == null ? Game1.currentLocation : location).debris.Add(debris);
    }

    public static Farmer getFarmer(long id)
    {
      if (Game1.player.uniqueMultiplayerID == id)
        return Game1.player;
      foreach (Farmer farmer in Game1.otherFarmers.Values)
      {
        if (farmer.uniqueMultiplayerID == id)
          return farmer;
      }
      return (Farmer) null;
    }

    public static List<Farmer> getAllFarmers()
    {
      List<Farmer> farmerList = new List<Farmer>();
      farmerList.Add(Game1.player);
      if (Game1.otherFarmers != null)
        farmerList.AddRange((IEnumerable<Farmer>) Game1.otherFarmers.Values);
      return farmerList;
    }

    public static void farmerFindsArtifact(int objectIndex)
    {
      Game1.player.addItemToInventoryBool((Item) new Object(objectIndex, 1, false, -1, 0), false);
    }

    public static void addHUDMessage(HUDMessage message)
    {
      if (message.type != null || message.whatType != 0)
      {
        for (int index = 0; index < Game1.hudMessages.Count; ++index)
        {
          if (message.type != null && Game1.hudMessages[index].type != null && (Game1.hudMessages[index].type.Equals(message.type) && Game1.hudMessages[index].add == message.add))
          {
            Game1.hudMessages[index].number = message.add ? Game1.hudMessages[index].number + message.number : Game1.hudMessages[index].number - message.number;
            Game1.hudMessages[index].timeLeft = 3500f;
            Game1.hudMessages[index].transparency = 1f;
            return;
          }
          if (message.whatType == Game1.hudMessages[index].whatType && message.whatType != 1 && (message.message != null && message.message.Equals(Game1.hudMessages[index].message)))
          {
            Game1.hudMessages[index].timeLeft = message.timeLeft;
            Game1.hudMessages[index].transparency = 1f;
            return;
          }
        }
      }
      Game1.hudMessages.Add(message);
      for (int index = Game1.hudMessages.Count - 1; index >= 0; --index)
      {
        if (Game1.hudMessages[index].noIcon)
        {
          HUDMessage hudMessage = Game1.hudMessages[index];
          Game1.hudMessages.RemoveAt(index);
          Game1.hudMessages.Add(hudMessage);
        }
      }
    }

    public static void nextMineLevel()
    {
      Game1.warpFarmer("UndergroundMine", 16, 16, false);
    }

    public static void swordswipe(int direction, float animationSpeed, bool flip)
    {
    }

    public static void showSwordswipeAnimation(int direction, Vector2 source, float animationSpeed, bool flip)
    {
      switch (direction)
      {
        case 0:
          Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(-1, animationSpeed, 5, 1, new Vector2(source.X + (float) (Game1.tileSize / 2), source.Y), false, false, !flip, -1.570796f));
          break;
        case 1:
          Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(-1, animationSpeed, 5, 1, new Vector2(source.X + (float) (Game1.tileSize * 3 / 2) + (float) (Game1.tileSize / 4), source.Y + (float) (Game1.tileSize * 3 / 4)), false, flip, false, flip ? -3.141593f : 0.0f));
          break;
        case 2:
          Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(-1, animationSpeed, 5, 1, new Vector2(source.X + (float) (Game1.tileSize / 2), source.Y + (float) (Game1.tileSize * 2)), false, false, !flip, 1.570796f));
          break;
        case 3:
          Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(-1, animationSpeed, 5, 1, new Vector2(source.X - (float) (Game1.tileSize / 2) - (float) (Game1.tileSize / 4), source.Y + (float) (Game1.tileSize * 3 / 4)), false, !flip, false, flip ? -3.141593f : 0.0f));
          break;
      }
    }

    public static void farmerTakeDamage(int damage, bool overrideParry, Monster damager)
    {
      if (damager != null && !damager.isInvincible() && (!overrideParry && Game1.player.CurrentTool != null) && (Game1.player.CurrentTool is MeleeWeapon && ((MeleeWeapon) Game1.player.CurrentTool).isOnSpecial && ((MeleeWeapon) Game1.player.CurrentTool).type == 3))
      {
        Rumble.rumble(0.75f, 150f);
        Game1.playSound("parry");
        float xVelocity = damager.xVelocity;
        float yVelocity = damager.yVelocity;
        if ((double) damager.xVelocity != 0.0 || (double) damager.yVelocity != 0.0)
          Game1.currentLocation.damageMonster(damager.GetBoundingBox(), damage / 2, damage / 2 + 1, false, 0.0f, 0, 0.0f, 0.0f, false, Game1.player);
        damager.xVelocity = -xVelocity;
        damager.yVelocity = -yVelocity;
        damager.xVelocity *= (float) (damager.isGlider ? 2.0 : 3.5);
        damager.yVelocity *= (float) (damager.isGlider ? 2.0 : 3.5);
        int num = damager.isGlider ? 1 : 0;
        damager.setInvincibleCountdown(450);
      }
      else
      {
        if (Game1.player.temporarilyInvincible || damager != null && damager.isInvincible() || (Game1.isEating || Game1.fadeToBlack || Game1.buffsDisplay.hasBuff(21)) || damager != null && (damager is GreenSlime || damager is BigSlime) && Game1.player.isWearingRing(520))
          return;
        if (Game1.player.isWearingRing(524) && !Game1.buffsDisplay.hasBuff(21) && Game1.random.NextDouble() < (0.9 - (double) Game1.player.health / 100.0) / (double) (3 - Game1.player.LuckLevel / 10) + (Game1.player.health <= 15 ? 0.2 : 0.0))
        {
          Game1.playSound("yoba");
          Game1.buffsDisplay.addOtherBuff(new Buff(21));
        }
        else
        {
          Rumble.rumble(0.75f, 150f);
          damage += Game1.random.Next(Math.Min(-1, -damage / 8), Math.Max(1, damage / 8));
          damage = Math.Max(1, damage - Game1.player.resilience);
          Game1.player.health = Math.Max(0, Game1.player.health - damage);
          Game1.player.temporarilyInvincible = true;
          Game1.currentLocation.debris.Add(new Debris(damage, new Vector2((float) (Game1.player.getStandingX() + 8), (float) Game1.player.getStandingY()), Color.Red, 1f, (Character) Game1.player));
          Game1.playSound("ow");
          Game1.hitShakeTimer = 100 * damage;
        }
      }
    }

    public static void removeSquareDebrisFromTile(int tileX, int tileY)
    {
      for (int index = Game1.currentLocation.debris.Count - 1; index >= 0; --index)
      {
        if (Game1.currentLocation.debris[index].debrisType == Debris.DebrisType.SQUARES && (int) ((double) Game1.currentLocation.debris[index].Chunks[0].position.X / (double) Game1.tileSize) == tileX && Game1.currentLocation.debris[index].chunkFinalYLevel / Game1.tileSize == tileY)
          Game1.currentLocation.debris.RemoveAt(index);
      }
    }

    public static void removeDebris(Debris.DebrisType type)
    {
      for (int index = Game1.currentLocation.debris.Count - 1; index >= 0; --index)
      {
        if (Game1.currentLocation.debris[index].debrisType == type)
          Game1.currentLocation.debris.RemoveAt(index);
      }
    }

    public static void toolAnimationDone()
    {
      Game1.toolAnimationDone(Game1.player);
    }

    public static void toolAnimationDone(Farmer who)
    {
      float stamina = Game1.player.Stamina;
      if (who.CurrentTool == null)
        return;
      if ((double) who.Stamina > 0.0)
      {
        int power = (int) (((double) Game1.toolHold + 20.0) / 600.0) + 1;
        Vector2 toolLocation = who.GetToolLocation(false);
        if (who.CurrentTool.GetType() == typeof (FishingRod) && ((FishingRod) who.CurrentTool).isFishing)
          who.canReleaseTool = false;
        else if (who.CurrentTool.GetType() != typeof (FishingRod))
        {
          who.UsingTool = false;
          if (who.CurrentTool.Name.Contains("Seeds"))
          {
            if (!Game1.eventUp)
            {
              who.CurrentTool.DoFunction(Game1.currentLocation, who.getStandingX(), who.getStandingY(), power, who);
              if (((Seeds) who.CurrentTool).NumberInStack <= 0)
                who.removeItemFromInventory((Item) who.CurrentTool);
            }
          }
          else if (who.CurrentTool.Name.Equals("Watering Can"))
          {
            switch (who.FacingDirection)
            {
              case 0:
              case 2:
                who.CurrentTool.DoFunction(Game1.currentLocation, (int) toolLocation.X, (int) toolLocation.Y, power, who);
                break;
              case 1:
              case 3:
                who.CurrentTool.DoFunction(Game1.currentLocation, (int) toolLocation.X, (int) toolLocation.Y, power, who);
                break;
            }
          }
          else if (who.CurrentTool is MeleeWeapon)
          {
            who.CurrentTool.CurrentParentTileIndex = who.CurrentTool.indexOfMenuItemView;
          }
          else
          {
            if (who.CurrentTool.Name.Equals("Wand"))
              who.CurrentTool.CurrentParentTileIndex = who.CurrentTool.indexOfMenuItemView;
            who.CurrentTool.DoFunction(Game1.currentLocation, (int) toolLocation.X, (int) toolLocation.Y, power, who);
          }
        }
        else
          who.usingTool = false;
      }
      else if (who.CurrentTool.instantUse)
        who.CurrentTool.DoFunction(Game1.currentLocation, 0, 0, 0, who);
      else
        who.UsingTool = false;
      who.lastClick = Vector2.Zero;
      Game1.toolHold = 0.0f;
      if (who.IsMainPlayer && !Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift))
        who.setRunning(Game1.options.autoRun, false);
      if (!who.UsingTool)
      {
        switch (who.FacingDirection)
        {
          case 0:
            who.Sprite.CurrentFrame = 16;
            break;
          case 1:
            who.Sprite.CurrentFrame = 8;
            break;
          case 2:
            who.Sprite.CurrentFrame = 0;
            break;
          case 3:
            who.Sprite.CurrentFrame = 24;
            break;
        }
      }
      if ((double) Game1.player.Stamina > 0.0 || (double) stamina <= 0.0)
        return;
      Game1.player.doEmote(36);
    }

    public static bool pressActionButton(KeyboardState currentKBState, MouseState currentMouseState, GamePadState currentPadState)
    {
      if (Game1.dialogueTyping)
      {
        bool flag = true;
        Game1.dialogueTyping = false;
        if (Game1.currentSpeaker != null)
          Game1.currentDialogueCharacterIndex = Game1.currentSpeaker.CurrentDialogue.Peek().getCurrentDialogue().Length;
        else if (Game1.currentObjectDialogue.Count > 0)
          Game1.currentDialogueCharacterIndex = Game1.currentObjectDialogue.Peek().Length;
        else
          flag = false;
        Game1.dialogueTypingInterval = 0;
        Game1.oldKBState = currentKBState;
        Game1.oldMouseState = currentMouseState;
        Game1.oldPadState = currentPadState;
        if (flag)
        {
          Game1.playSound("dialogueCharacterClose");
          return false;
        }
      }
      if (Game1.dialogueUp && Game1.numberOfSelectedItems == -1)
      {
        if (Game1.isQuestion)
        {
          Game1.isQuestion = false;
          if (Game1.currentSpeaker != null)
          {
            if (Game1.currentSpeaker.CurrentDialogue.Peek().chooseResponse(Game1.questionChoices[Game1.currentQuestionChoice]))
            {
              Game1.currentDialogueCharacterIndex = 1;
              Game1.dialogueTyping = true;
              Game1.oldKBState = currentKBState;
              Game1.oldMouseState = currentMouseState;
              Game1.oldPadState = currentPadState;
              return false;
            }
          }
          else
          {
            Game1.dialogueUp = false;
            if (Game1.eventUp)
            {
              Game1.currentLocation.currentEvent.answerDialogue(Game1.currentLocation.lastQuestionKey, Game1.currentQuestionChoice);
              Game1.currentQuestionChoice = 0;
              Game1.oldKBState = currentKBState;
              Game1.oldMouseState = currentMouseState;
              Game1.oldPadState = currentPadState;
            }
            else if (Game1.currentLocation.answerDialogue(Game1.questionChoices[Game1.currentQuestionChoice]))
            {
              Game1.currentQuestionChoice = 0;
              Game1.oldKBState = currentKBState;
              Game1.oldMouseState = currentMouseState;
              Game1.oldPadState = currentPadState;
              return false;
            }
            if (Game1.dialogueUp)
            {
              Game1.currentDialogueCharacterIndex = 1;
              Game1.dialogueTyping = true;
              Game1.oldKBState = currentKBState;
              Game1.oldMouseState = currentMouseState;
              Game1.oldPadState = currentPadState;
              return false;
            }
          }
          Game1.currentQuestionChoice = 0;
        }
        string str = (string) null;
        if (Game1.currentSpeaker != null)
        {
          if (!Game1.currentSpeaker.immediateSpeak)
          {
            str = Game1.currentSpeaker.CurrentDialogue.Count > 0 ? Game1.currentSpeaker.CurrentDialogue.Peek().exitCurrentDialogue() : (string) null;
          }
          else
          {
            Game1.currentSpeaker.immediateSpeak = false;
            return false;
          }
        }
        if (str == null)
        {
          if (Game1.currentSpeaker != null && Game1.currentSpeaker.CurrentDialogue.Count > 0 && (Game1.currentSpeaker.CurrentDialogue.Peek().isOnFinalDialogue() && Game1.currentSpeaker.CurrentDialogue.Count > 0))
            Game1.currentSpeaker.CurrentDialogue.Pop();
          Game1.dialogueUp = false;
          if (Game1.messagePause)
            Game1.pauseTime = 500f;
          if (Game1.currentObjectDialogue.Count > 0)
            Game1.currentObjectDialogue.Dequeue();
          Game1.currentDialogueCharacterIndex = 0;
          if (Game1.currentObjectDialogue.Count > 0)
          {
            Game1.dialogueUp = true;
            Game1.questionChoices.Clear();
            Game1.oldKBState = currentKBState;
            Game1.oldMouseState = currentMouseState;
            Game1.oldPadState = currentPadState;
            Game1.dialogueTyping = true;
            return false;
          }
          Game1.tvStation = -1;
          if (Game1.currentSpeaker != null && !Game1.currentSpeaker.name.Equals("Gunther") && (!Game1.eventUp && !Game1.currentSpeaker.doingEndOfRouteAnimation))
            Game1.currentSpeaker.doneFacingPlayer(Game1.player);
          Game1.currentSpeaker = (NPC) null;
          if (!Game1.eventUp)
            Game1.player.CanMove = true;
          else if (Game1.currentLocation.currentEvent.CurrentCommand > 0 || Game1.currentLocation.currentEvent.specialEventVariable1)
          {
            if (!Game1.isFestival() || !Game1.currentLocation.currentEvent.canMoveAfterDialogue())
              ++Game1.currentLocation.currentEvent.CurrentCommand;
            else
              Game1.player.CanMove = true;
          }
          Game1.questionChoices.Clear();
          Game1.playSound("smallSelect");
        }
        else
        {
          Game1.playSound("smallSelect");
          Game1.currentDialogueCharacterIndex = 0;
          Game1.dialogueTyping = true;
          Game1.checkIfDialogueIsQuestion();
        }
        Game1.oldKBState = currentKBState;
        Game1.oldMouseState = currentMouseState;
        Game1.oldPadState = currentPadState;
        if (Game1.questOfTheDay != null && Game1.questOfTheDay.accepted && Game1.questOfTheDay.GetType().Name.Equals("SocializeQuest"))
          Game1.questOfTheDay.checkIfComplete((NPC) null, -1, -1, (Item) null, (string) null);
        Game1.afterFadeFunction afterDialogues = Game1.afterDialogues;
        return false;
      }
      if (Game1.currentBillboard != 0)
      {
        Game1.currentBillboard = 0;
        Game1.player.CanMove = true;
        Game1.oldKBState = currentKBState;
        Game1.oldMouseState = currentMouseState;
        Game1.oldPadState = currentPadState;
        return false;
      }
      if (!Game1.player.UsingTool && !Game1.pickingTool && !Game1.menuUp && ((!Game1.eventUp || Game1.currentLocation.currentEvent.playerControlSequence) && (!Game1.nameSelectUp && Game1.numberOfSelectedItems == -1)) && !Game1.fadeToBlack)
      {
        Vector2 vector2 = new Vector2((float) (Game1.getOldMouseX() + Game1.viewport.X), (float) (Game1.getOldMouseY() + Game1.viewport.Y)) / (float) Game1.tileSize;
        if ((double) Game1.mouseCursorTransparency == 0.0 || !Game1.wasMouseVisibleThisFrame || !Game1.lastCursorMotionWasMouse && (Game1.player.ActiveObject == null || !Game1.player.ActiveObject.isPlaceable() && Game1.player.ActiveObject.category != -74))
        {
          vector2 = Game1.player.GetGrabTile();
          if (vector2.Equals(Game1.player.getTileLocation()))
            vector2 = Utility.getTranslatedVector2(vector2, Game1.player.facingDirection, 1f);
        }
        if (!Utility.tileWithinRadiusOfPlayer((int) vector2.X, (int) vector2.Y, 1, Game1.player))
        {
          vector2 = Game1.player.GetGrabTile();
          if (vector2.Equals(Game1.player.getTileLocation()) && Game1.isAnyGamePadButtonBeingPressed())
            vector2 = Utility.getTranslatedVector2(vector2, Game1.player.facingDirection, 1f);
        }
        if (!Game1.eventUp || Game1.isFestival())
        {
          if (Game1.tryToCheckAt(vector2, Game1.player))
            return false;
          if (Game1.player.isRidingHorse())
          {
            Game1.player.getMount().checkAction(Game1.player, Game1.player.currentLocation);
            return false;
          }
          if (!Game1.player.canMove)
            return false;
          bool flag = false;
          if (Game1.player.ActiveObject != null && !(Game1.player.ActiveObject is Furniture))
          {
            if (Game1.player.ActiveObject.performUseAction())
            {
              Game1.player.reduceActiveItemByOne();
              Game1.oldKBState = currentKBState;
              Game1.oldMouseState = currentMouseState;
              Game1.oldPadState = currentPadState;
              return false;
            }
            int stack = Game1.player.ActiveObject.Stack;
            Utility.tryToPlaceItem(Game1.currentLocation, (Item) Game1.player.ActiveObject, (int) vector2.X * Game1.tileSize + Game1.tileSize / 2, (int) vector2.Y * Game1.tileSize + Game1.tileSize / 2);
            if (Game1.player.ActiveObject == null || Game1.player.ActiveObject.Stack < stack || Game1.player.ActiveObject.isPlaceable())
              flag = true;
          }
          if (!flag)
          {
            ++vector2.Y;
            if (Game1.tryToCheckAt(vector2, Game1.player))
              return false;
            if (Game1.player.ActiveObject != null && Game1.player.ActiveObject is Furniture)
            {
              (Game1.player.ActiveObject as Furniture).rotate();
              Game1.playSound("dwoop");
              Game1.oldKBState = currentKBState;
              Game1.oldMouseState = currentMouseState;
              Game1.oldPadState = currentPadState;
              return false;
            }
            vector2 = Game1.player.getTileLocation();
            if (Game1.tryToCheckAt(vector2, Game1.player))
              return false;
            if (Game1.player.ActiveObject != null && Game1.player.ActiveObject is Furniture)
            {
              (Game1.player.ActiveObject as Furniture).rotate();
              Game1.playSound("dwoop");
              Game1.oldKBState = currentKBState;
              Game1.oldMouseState = currentMouseState;
              Game1.oldPadState = currentPadState;
              return false;
            }
          }
          if (!Game1.isEating && Game1.player.ActiveObject != null && (!Game1.dialogueUp && !Game1.eventUp) && (!Game1.player.canOnlyWalk && !Game1.player.FarmerSprite.pauseForSingleAnimation && (!Game1.fadeToBlack && Game1.player.ActiveObject.Edibility != -300)))
          {
            Game1.player.faceDirection(2);
            Game1.isEating = true;
            Game1.player.itemToEat = (Item) Game1.player.ActiveObject;
            Game1.player.FarmerSprite.setCurrentSingleAnimation(304);
            GameLocation currentLocation = Game1.currentLocation;
            string question;
            if (Game1.objectInformation[Game1.player.ActiveObject.parentSheetIndex].Split('/').Length > 6)
            {
              if (Game1.objectInformation[Game1.player.ActiveObject.parentSheetIndex].Split('/')[6].Equals("drink"))
              {
                question = Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3159", (object) Game1.player.ActiveObject.DisplayName);
                goto label_81;
              }
            }
            question = Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3160", (object) Game1.player.ActiveObject.DisplayName);
label_81:
            Response[] yesNoResponses = Game1.currentLocation.createYesNoResponses();
            string dialogKey = "Eat";
            currentLocation.createQuestionDialogue(question, yesNoResponses, dialogKey);
            Game1.oldKBState = currentKBState;
            Game1.oldMouseState = currentMouseState;
            Game1.oldPadState = currentPadState;
            return false;
          }
        }
        else
        {
          Game1.currentLocation.currentEvent.receiveActionPress((int) vector2.X, (int) vector2.Y);
          Game1.oldKBState = currentKBState;
          Game1.oldMouseState = currentMouseState;
          Game1.oldPadState = currentPadState;
          return false;
        }
      }
      else if (Game1.numberOfSelectedItems != -1)
      {
        Game1.tryToBuySelectedItems();
        Game1.playSound("smallSelect");
        Game1.oldKBState = currentKBState;
        Game1.oldMouseState = currentMouseState;
        Game1.oldPadState = currentPadState;
        return false;
      }
      if (Game1.player.CurrentTool != null && Game1.player.CurrentTool is MeleeWeapon && (Game1.player.CanMove && !Game1.player.canOnlyWalk) && !Game1.eventUp)
        ((MeleeWeapon) Game1.player.CurrentTool).animateSpecialMove(Game1.player);
      return true;
    }

    public static bool tryToCheckAt(Vector2 grabTile, Farmer who)
    {
      Game1.haltAfterCheck = true;
      if (!Utility.tileWithinRadiusOfPlayer((int) grabTile.X, (int) grabTile.Y, 1, Game1.player) || !Game1.currentLocation.checkAction(new Location((int) grabTile.X, (int) grabTile.Y), Game1.viewport, who))
        return false;
      Game1.updateCursorTileHint();
      who.lastGrabTile = grabTile;
      if (who.CanMove && Game1.haltAfterCheck)
      {
        who.faceGeneralDirection(grabTile * (float) Game1.tileSize, 0);
        who.Halt();
      }
      MultiplayerUtility.broadcastCheckAction((int) grabTile.X, (int) grabTile.Y, who.uniqueMultiplayerID, Game1.currentLocation.name);
      Game1.oldKBState = Keyboard.GetState();
      Game1.oldMouseState = Mouse.GetState();
      Game1.oldPadState = GamePad.GetState(Game1.playerOneIndex);
      return true;
    }

    public static void pressSwitchToolButton()
    {
      int num = Mouse.GetState().ScrollWheelValue > Game1.oldMouseState.ScrollWheelValue ? -1 : (Mouse.GetState().ScrollWheelValue < Game1.oldMouseState.ScrollWheelValue ? 1 : 0);
      if (Game1.options.gamepadControls && num == 0)
        num = GamePad.GetState(Game1.playerOneIndex).IsButtonDown(Buttons.LeftTrigger) ? -1 : 1;
      if (Game1.options.invertScrollDirection)
        num *= -1;
      Game1.player.CurrentToolIndex = (Game1.player.CurrentToolIndex + num) % 12;
      if (Game1.player.CurrentToolIndex < 0)
        Game1.player.CurrentToolIndex = 11;
      for (int index = 0; index < 12 && Game1.player.CurrentItem == null; ++index)
      {
        Game1.player.CurrentToolIndex = (num + Game1.player.CurrentToolIndex) % 12;
        if (Game1.player.CurrentToolIndex < 0)
          Game1.player.CurrentToolIndex = 11;
      }
      Game1.playSound("toolSwap");
      if (Game1.player.ActiveObject != null)
        Game1.player.showCarrying();
      else
        Game1.player.showNotCarrying();
      if (Game1.player.CurrentTool == null || Game1.player.CurrentTool.Name.Equals("Seeds") || (Game1.player.CurrentTool.Name.Contains("Sword") || Game1.player.CurrentTool.instantUse))
        return;
      Game1.player.CurrentTool.CurrentParentTileIndex = Game1.player.CurrentTool.CurrentParentTileIndex - Game1.player.CurrentTool.CurrentParentTileIndex % 8 + 2;
    }

    public static void switchToolAnimation()
    {
      Game1.pickToolInterval = 0.0f;
      Game1.player.CanMove = false;
      Game1.pickingTool = true;
      Game1.playSound("toolSwap");
      switch (Game1.player.FacingDirection)
      {
        case 0:
          Game1.player.FarmerSprite.setCurrentFrame(196);
          break;
        case 1:
          Game1.player.FarmerSprite.setCurrentFrame(194);
          break;
        case 2:
          Game1.player.FarmerSprite.setCurrentFrame(192);
          break;
        case 3:
          Game1.player.FarmerSprite.setCurrentFrame(198);
          break;
      }
      if (Game1.player.CurrentTool != null && !Game1.player.CurrentTool.Name.Equals("Seeds") && (!Game1.player.CurrentTool.Name.Contains("Sword") && !Game1.player.CurrentTool.instantUse))
        Game1.player.CurrentTool.CurrentParentTileIndex = Game1.player.CurrentTool.CurrentParentTileIndex - Game1.player.CurrentTool.CurrentParentTileIndex % 8 + 2;
      if (Game1.player.ActiveObject == null)
        return;
      Game1.player.showCarrying();
    }

    public static bool pressUseToolButton()
    {
      if (Game1.fadeToBlack)
        return false;
      Game1.player.toolPower = 0;
      Game1.player.toolHold = 0;
      if (Game1.player.CurrentTool == null && Game1.player.ActiveObject == null)
      {
        Vector2 key = Game1.player.GetToolLocation(false) / (float) Game1.tileSize;
        key.X = (float) (int) key.X;
        key.Y = (float) (int) key.Y;
        if (Game1.currentLocation.Objects.ContainsKey(key))
        {
          Object @object = Game1.currentLocation.Objects[key];
          if (!@object.readyForHarvest && @object.heldObject == null && (!(@object is Fence) && !(@object is CrabPot)) && (@object.type != null && (@object.type.Equals("Crafting") || @object.type.Equals("interactive"))) && !@object.name.Equals("Twig"))
          {
            @object.setHealth(@object.getHealth() - 1);
            @object.shakeTimer = 300;
            Game1.playSound("hammer");
            if (@object.getHealth() < 2)
            {
              Game1.playSound("hammer");
              if (@object.getHealth() < 1)
              {
                Tool t = (Tool) new Pickaxe();
                t.DoFunction(Game1.currentLocation, -1, -1, 0, Game1.player);
                if (@object.performToolAction(t))
                {
                  @object.performRemoveAction(@object.tileLocation, Game1.currentLocation);
                  if (@object.type.Equals("Crafting") && @object.fragility != 2)
                  {
                    List<Debris> debris1 = Game1.currentLocation.debris;
                    int objectIndex = @object.bigCraftable ? -@object.ParentSheetIndex : @object.ParentSheetIndex;
                    Vector2 toolLocation = Game1.player.GetToolLocation(false);
                    Microsoft.Xna.Framework.Rectangle boundingBox = Game1.player.GetBoundingBox();
                    double x = (double) boundingBox.Center.X;
                    boundingBox = Game1.player.GetBoundingBox();
                    double y = (double) boundingBox.Center.Y;
                    Vector2 playerPosition = new Vector2((float) x, (float) y);
                    Debris debris2 = new Debris(objectIndex, toolLocation, playerPosition);
                    debris1.Add(debris2);
                  }
                  Game1.currentLocation.Objects.Remove(key);
                  return true;
                }
              }
            }
          }
        }
      }
      if (Game1.currentMinigame == null && !Game1.player.usingTool && (Game1.player.isRidingHorse() || Game1.dialogueUp || Game1.eventUp && !Game1.CurrentEvent.canPlayerUseTool() && (!Game1.currentLocation.currentEvent.playerControlSequence || Game1.activeClickableMenu == null && Game1.currentMinigame == null) || Game1.player.CurrentTool != null && (object) Game1.currentLocation.doesPositionCollideWithCharacter(Utility.getRectangleCenteredAt(Game1.player.GetToolLocation(false), Game1.tileSize), true) != null && Game1.currentLocation.doesPositionCollideWithCharacter(Utility.getRectangleCenteredAt(Game1.player.GetToolLocation(false), Game1.tileSize), true).isVillager()))
      {
        Game1.pressActionButton(Keyboard.GetState(), Mouse.GetState(), GamePad.GetState(Game1.playerOneIndex));
        return false;
      }
      if (Game1.player.canOnlyWalk)
        return true;
      Vector2 vector2 = !Game1.wasMouseVisibleThisFrame ? Game1.player.GetToolLocation(false) : new Vector2((float) (Game1.getOldMouseX() + Game1.viewport.X), (float) (Game1.getOldMouseY() + Game1.viewport.Y));
      if (Utility.canGrabSomethingFromHere((int) vector2.X, (int) vector2.Y, Game1.player))
      {
        Vector2 index = new Vector2((float) ((Game1.getOldMouseX() + Game1.viewport.X) / Game1.tileSize), (float) ((Game1.getOldMouseY() + Game1.viewport.Y) / Game1.tileSize));
        if (Game1.currentLocation.checkAction(new Location((int) index.X, (int) index.Y), Game1.viewport, Game1.player))
        {
          Game1.updateCursorTileHint();
          return true;
        }
        if (Game1.currentLocation.terrainFeatures.ContainsKey(index))
        {
          Game1.currentLocation.terrainFeatures[index].performUseAction(index);
          return true;
        }
        if (Game1.IsMultiplayer)
          MultiplayerUtility.broadcastCheckAction((int) index.X, (int) index.Y, Game1.player.uniqueMultiplayerID, Game1.currentLocation.name);
        return false;
      }
      if (Game1.currentLocation.leftClick((int) vector2.X, (int) vector2.Y, Game1.player))
        return true;
      if (Game1.player.ActiveObject != null)
      {
        if (Utility.withinRadiusOfPlayer((int) vector2.X, (int) vector2.Y, 1, Game1.player) && Game1.currentLocation.checkAction(new Location((int) vector2.X / Game1.tileSize, (int) vector2.Y / Game1.tileSize), Game1.viewport, Game1.player))
        {
          if (Game1.IsMultiplayer)
            MultiplayerUtility.broadcastCheckAction((Game1.getOldMouseX() + Game1.viewport.X) / Game1.tileSize, (Game1.getOldMouseY() + Game1.viewport.Y) / Game1.tileSize, Game1.player.uniqueMultiplayerID, Game1.currentLocation.name);
          return true;
        }
        Utility.tryToPlaceItem(Game1.currentLocation, (Item) Game1.player.ActiveObject, (int) vector2.X, (int) vector2.Y);
      }
      if (Game1.player.UsingTool)
      {
        Game1.player.lastClick = new Vector2((float) (int) vector2.X, (float) (int) vector2.Y);
        Game1.player.CurrentTool.DoFunction(Game1.player.currentLocation, (int) Game1.player.lastClick.X, (int) Game1.player.lastClick.Y, 1, Game1.player);
        return true;
      }
      if (Game1.player.ActiveObject == null && !Game1.isEating && Game1.player.CurrentTool != null)
      {
        if ((double) Game1.player.Stamina <= 20.0 && Game1.player.CurrentTool != null && !(Game1.player.CurrentTool is MeleeWeapon))
        {
          Game1.staminaShakeTimer = 1000;
          for (int index = 0; index < 4; ++index)
          {
            List<TemporaryAnimatedSprite> overlayTempSprites = Game1.screenOverlayTempSprites;
            TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(366, 412, 5, 6), new Vector2((float) (Game1.random.Next(Game1.tileSize / 2) + Game1.viewport.Width - (48 + Game1.tileSize / 8)), (float) (Game1.viewport.Height - 224 - Game1.tileSize / 4 - (int) ((double) (Game1.player.MaxStamina - 270) * 0.715))), false, 0.012f, Color.SkyBlue);
            temporaryAnimatedSprite.motion = new Vector2(-2f, -10f);
            temporaryAnimatedSprite.acceleration = new Vector2(0.0f, 0.5f);
            int num1 = 1;
            temporaryAnimatedSprite.local = num1 != 0;
            double num2 = (double) (Game1.pixelZoom + Game1.random.Next(-1, 0));
            temporaryAnimatedSprite.scale = (float) num2;
            int num3 = index * 30;
            temporaryAnimatedSprite.delayBeforeAnimationStart = num3;
            overlayTempSprites.Add(temporaryAnimatedSprite);
          }
        }
        Game1.player.CanMove = false;
        Game1.player.UsingTool = true;
        Game1.player.canReleaseTool = true;
        if (Utility.withinRadiusOfPlayer((int) vector2.X, (int) vector2.Y, 1, Game1.player) && (Game1.player.CurrentTool is WateringCan || (double) Math.Abs(vector2.X - (float) Game1.player.getStandingX()) >= (double) (Game1.tileSize / 2) || (double) Math.Abs(vector2.Y - (float) Game1.player.getStandingY()) >= (double) (Game1.tileSize / 2)))
        {
          Game1.player.Halt();
          if ((double) Game1.mouseCursorTransparency != 0.0 && !Game1.isAnyGamePadButtonBeingHeld())
            Game1.player.faceGeneralDirection(new Vector2((float) (int) vector2.X, (float) (int) vector2.Y), 0);
          Game1.player.lastClick = new Vector2((float) (int) vector2.X, (float) (int) vector2.Y);
        }
        try
        {
          if (Game1.player.CurrentTool.beginUsing(Game1.currentLocation, (int) Game1.player.lastClick.X, (int) Game1.player.lastClick.Y, Game1.player))
            return false;
        }
        catch (Exception ex)
        {
        }
        if (!Game1.player.CurrentTool.instantUse)
        {
          Game1.player.Halt();
          Game1.player.CurrentTool.Update(Game1.player.FacingDirection, 0, Game1.player);
          if (!(Game1.player.CurrentTool is FishingRod) && Game1.player.CurrentTool.upgradeLevel <= 0 && !(Game1.player.CurrentTool is MeleeWeapon) || Game1.player.CurrentTool is Pickaxe)
          {
            Game1.releaseUseToolButton();
            return false;
          }
        }
        if (Game1.player.CurrentTool.Name.Equals("Wand"))
        {
          if (((Wand) Game1.player.CurrentTool).charged)
          {
            Game1.toolAnimationDone();
            Game1.player.canReleaseTool = false;
            if (!Game1.fadeToBlack)
            {
              Game1.player.CanMove = true;
              Game1.player.UsingTool = false;
            }
          }
          else
          {
            Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3180")));
            Game1.player.UsingTool = false;
            Game1.player.canReleaseTool = false;
          }
        }
        else if (Game1.player.CurrentTool.instantUse)
        {
          Game1.toolAnimationDone();
          Game1.player.canReleaseTool = false;
          Game1.player.UsingTool = false;
        }
        else if (Game1.player.CurrentTool.Name.Equals("Seeds"))
        {
          switch (Game1.player.FacingDirection)
          {
            case 0:
              Game1.player.Sprite.CurrentFrame = 208;
              Game1.player.CurrentTool.Update(0, 0);
              break;
            case 1:
              Game1.player.Sprite.CurrentFrame = 204;
              Game1.player.CurrentTool.Update(1, 0);
              break;
            case 2:
              Game1.player.Sprite.CurrentFrame = 200;
              Game1.player.CurrentTool.Update(2, 0);
              break;
            case 3:
              Game1.player.Sprite.CurrentFrame = 212;
              Game1.player.CurrentTool.Update(3, 0);
              break;
          }
        }
        else if (Game1.player.CurrentTool is WateringCan && Game1.currentLocation.doesTileHaveProperty(((int) Game1.player.GetToolLocation(false).X + Game1.tileSize / 2) / Game1.tileSize, (int) Game1.player.GetToolLocation(false).Y / Game1.tileSize, "Water", "Back") != null)
        {
          switch (Game1.player.FacingDirection)
          {
            case 0:
              ((FarmerSprite) Game1.player.Sprite).animateOnce(182, 250f, 2);
              Game1.player.CurrentTool.Update(0, 1);
              break;
            case 1:
              ((FarmerSprite) Game1.player.Sprite).animateOnce(174, 250f, 2);
              Game1.player.CurrentTool.Update(1, 0);
              break;
            case 2:
              ((FarmerSprite) Game1.player.Sprite).animateOnce(166, 250f, 2);
              Game1.player.CurrentTool.Update(2, 1);
              break;
            case 3:
              ((FarmerSprite) Game1.player.Sprite).animateOnce(190, 250f, 2);
              Game1.player.CurrentTool.Update(3, 0);
              break;
          }
          Game1.player.canReleaseTool = false;
        }
        else if (Game1.player.CurrentTool is WateringCan && ((WateringCan) Game1.player.CurrentTool).WaterLeft <= 0)
        {
          Game1.toolAnimationDone();
          Game1.player.CanMove = true;
          Game1.player.canReleaseTool = false;
        }
        else if (Game1.player.CurrentTool is WateringCan)
        {
          Game1.player.jitterStrength = 0.25f;
          switch (Game1.player.FacingDirection)
          {
            case 0:
              Game1.player.FarmerSprite.setCurrentFrame(180);
              Game1.player.CurrentTool.Update(0, 0);
              break;
            case 1:
              Game1.player.FarmerSprite.setCurrentFrame(172);
              Game1.player.CurrentTool.Update(1, 0);
              break;
            case 2:
              Game1.player.FarmerSprite.setCurrentFrame(164);
              Game1.player.CurrentTool.Update(2, 0);
              break;
            case 3:
              Game1.player.FarmerSprite.setCurrentFrame(188);
              Game1.player.CurrentTool.Update(3, 0);
              break;
          }
        }
        else if (Game1.player.CurrentTool is FishingRod)
        {
          switch (Game1.player.FacingDirection)
          {
            case 0:
              ((FarmerSprite) Game1.player.Sprite).animateOnce(295, 35f, 8, new AnimatedSprite.endOfAnimationBehavior(FishingRod.endOfAnimationBehavior));
              Game1.player.CurrentTool.Update(0, 0);
              break;
            case 1:
              ((FarmerSprite) Game1.player.Sprite).animateOnce(296, 35f, 8, new AnimatedSprite.endOfAnimationBehavior(FishingRod.endOfAnimationBehavior));
              Game1.player.CurrentTool.Update(1, 0);
              break;
            case 2:
              ((FarmerSprite) Game1.player.Sprite).animateOnce(297, 35f, 8, new AnimatedSprite.endOfAnimationBehavior(FishingRod.endOfAnimationBehavior));
              Game1.player.CurrentTool.Update(2, 0);
              break;
            case 3:
              ((FarmerSprite) Game1.player.Sprite).animateOnce(298, 35f, 8, new AnimatedSprite.endOfAnimationBehavior(FishingRod.endOfAnimationBehavior));
              Game1.player.CurrentTool.Update(3, 0);
              break;
          }
          Game1.player.canReleaseTool = false;
        }
        else if (Game1.player.CurrentTool is MeleeWeapon)
        {
          ((MeleeWeapon) Game1.player.CurrentTool).setFarmerAnimating(Game1.player);
        }
        else
        {
          switch (Game1.player.FacingDirection)
          {
            case 0:
              Game1.player.FarmerSprite.setCurrentFrame(176);
              Game1.player.CurrentTool.Update(0, 0);
              break;
            case 1:
              Game1.player.FarmerSprite.setCurrentFrame(168);
              Game1.player.CurrentTool.Update(1, 0);
              break;
            case 2:
              Game1.player.FarmerSprite.setCurrentFrame(160);
              Game1.player.CurrentTool.Update(2, 0);
              break;
            case 3:
              Game1.player.FarmerSprite.setCurrentFrame(184);
              Game1.player.CurrentTool.Update(3, 0);
              break;
          }
        }
      }
      return false;
    }

    public static void releaseUseToolButton()
    {
      Game1.player.stopJittering();
      Game1.player.canReleaseTool = false;
      int num = (double) Game1.player.Stamina <= 0.0 ? 2 : 1;
      if (Game1.isAnyGamePadButtonBeingPressed())
        Game1.player.lastClick = Game1.player.GetToolLocation(false);
      if (Game1.player.CurrentTool.Name.Equals("Seeds"))
      {
        switch (Game1.player.FacingDirection)
        {
          case 0:
            ((FarmerSprite) Game1.player.Sprite).animateOnce(208, 150f, 4);
            break;
          case 1:
            ((FarmerSprite) Game1.player.Sprite).animateOnce(204, 150f, 4);
            break;
          case 2:
            ((FarmerSprite) Game1.player.Sprite).animateOnce(200, 150f, 4);
            break;
          case 3:
            ((FarmerSprite) Game1.player.Sprite).animateOnce(212, 150f, 4);
            break;
        }
      }
      else if (Game1.player.CurrentTool is WateringCan)
      {
        if ((Game1.player.CurrentTool as WateringCan).WaterLeft > 0)
          Game1.playSound("wateringCan");
        switch (Game1.player.FacingDirection)
        {
          case 0:
            ((FarmerSprite) Game1.player.Sprite).animateOnce(180, 125f * (float) num, 3);
            break;
          case 1:
            ((FarmerSprite) Game1.player.Sprite).animateOnce(172, 125f * (float) num, 3);
            break;
          case 2:
            ((FarmerSprite) Game1.player.Sprite).animateOnce(164, 125f * (float) num, 3);
            break;
          case 3:
            ((FarmerSprite) Game1.player.Sprite).animateOnce(188, 125f * (float) num, 3);
            break;
        }
      }
      else if (Game1.player.CurrentTool.GetType() == typeof (FishingRod) && Game1.activeClickableMenu == null)
      {
        if ((Game1.player.CurrentTool as FishingRod).hit)
          return;
        Game1.player.CurrentTool.DoFunction(Game1.player.currentLocation, (int) Game1.player.lastClick.X, (int) Game1.player.lastClick.Y, 1, Game1.player);
      }
      else
      {
        Game1.player.FarmerSprite.nextOffset = 0;
        switch (Game1.player.FacingDirection)
        {
          case 0:
            ((FarmerSprite) Game1.player.Sprite).animateOnce(176, 60f * (float) num, 8);
            break;
          case 1:
            ((FarmerSprite) Game1.player.Sprite).animateOnce(168, 60f * (float) num, 8);
            break;
          case 2:
            ((FarmerSprite) Game1.player.Sprite).animateOnce(160, 60f * (float) num, 8);
            break;
          case 3:
            ((FarmerSprite) Game1.player.Sprite).animateOnce(184, 60f * (float) num, 8);
            break;
        }
      }
    }

    public static int getMouseX()
    {
      return (int) ((double) Mouse.GetState().X * (1.0 / (double) Game1.options.zoomLevel));
    }

    public static int getOldMouseX()
    {
      return (int) ((double) Game1.oldMouseState.X * (1.0 / (double) Game1.options.zoomLevel));
    }

    public static int getMouseY()
    {
      return (int) ((double) Mouse.GetState().Y * (1.0 / (double) Game1.options.zoomLevel));
    }

    public static int getOldMouseY()
    {
      return (int) ((double) Game1.oldMouseState.Y * (1.0 / (double) Game1.options.zoomLevel));
    }

    public static void pressAddItemToInventoryButton()
    {
    }

    public static int numberOfPlayers()
    {
      if (Game1.IsServer)
        return Game1.otherFarmers.Count + 1;
      if (!Game1.IsMultiplayer)
        return 1;
      return Game1.otherFarmers.Count;
    }

    public static bool isFestival()
    {
      if (Game1.currentLocation != null && Game1.currentLocation.currentEvent != null)
        return Game1.currentLocation.currentEvent.isFestival;
      return false;
    }

    public void parseDebugInput(string debugInput)
    {
      Game1.exitActiveMenu();
      Game1.lastDebugInput = debugInput;
      debugInput = debugInput.Trim();
      string[] strArray = debugInput.Split(' ');
      try
      {
        if (Game1.panMode)
        {
          if (strArray[0].Equals("exit") || strArray[0].ToLower().Equals("panmode"))
          {
            Game1.panMode = false;
            Game1.viewportFreeze = false;
            this.panModeString = "";
            Game1.debugMode = false;
            Game1.debugOutput = "";
            this.panFacingDirectionWait = false;
          }
          else if (strArray[0].Equals("clear"))
          {
            this.panModeString = "";
            Game1.debugOutput = "";
            this.panFacingDirectionWait = false;
          }
          else
          {
            if (this.panFacingDirectionWait)
              return;
            int result = 0;
            if (!int.TryParse(strArray[0], out result))
              return;
            this.panModeString = this.panModeString + (this.panModeString.Length > 0 ? "/" : "") + (object) result + " ";
            Game1.debugOutput = this.panModeString + Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3191");
          }
        }
        else
        {
          string s = strArray[0];
          // ISSUE: reference to a compiler-generated method
          uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(s);
          if (stringHash <= 2072037248U)
          {
            if (stringHash <= 1147755170U)
            {
              if (stringHash <= 654091955U)
              {
                if (stringHash <= 304701189U)
                {
                  if (stringHash <= 108289031U)
                  {
                    if (stringHash <= 60638767U)
                    {
                      if (stringHash <= 11649459U)
                      {
                        if ((int) stringHash != 9940504)
                        {
                          if ((int) stringHash != 11649459 || !(s == "removeObjects"))
                            return;
                          Game1.currentLocation.objects.Clear();
                          return;
                        }
                        if (!(s == "customize"))
                          return;
                        goto label_1088;
                      }
                      else if ((int) stringHash != 39742388)
                      {
                        if ((int) stringHash != 60638767 || !(s == "mainmenu"))
                          return;
                        goto label_1082;
                      }
                      else
                      {
                        if (!(s == "pickaxe"))
                          return;
                        goto label_1204;
                      }
                    }
                    else if (stringHash <= 90969176U)
                    {
                      if ((int) stringHash != 68193782)
                      {
                        if ((int) stringHash != 90969176 || !(s == "where"))
                          return;
                        goto label_947;
                      }
                      else
                      {
                        if (!(s == "bloomDay"))
                          return;
                        Game1.bloomDay = !Game1.bloomDay;
                        Game1.bloom.Visible = Game1.bloomDay;
                        Game1.bloom.reload();
                        return;
                      }
                    }
                    else
                    {
                      if ((int) stringHash != 105537319)
                      {
                        if ((int) stringHash != 108289031 || !(s == "cat"))
                          return;
                        Game1.currentLocation.characters.Add((NPC) new StardewValley.Characters.Cat(Convert.ToInt32(strArray[1]), Convert.ToInt32(strArray[2])));
                        return;
                      }
                      if (!(s == "museumloot"))
                        return;
                      using (Dictionary<int, string>.Enumerator enumerator = Game1.objectInformation.GetEnumerator())
                      {
                        while (enumerator.MoveNext())
                        {
                          KeyValuePair<int, string> current = enumerator.Current;
                          string str = current.Value.Split('/')[3];
                          if ((str.Contains("Arch") || str.Contains("Minerals")) && (!Game1.player.mineralsFound.ContainsKey(current.Key) && !Game1.player.archaeologyFound.ContainsKey(current.Key)))
                          {
                            if (str.Contains("Arch"))
                              Game1.player.foundArtifact(current.Key, 1);
                            else
                              Game1.player.addItemToInventoryBool((Item) new Object(current.Key, 1, false, -1, 0), false);
                          }
                          if (Game1.player.freeSpotsInInventory() == 0)
                            break;
                        }
                        return;
                      }
                    }
                  }
                  else if (stringHash <= 217793883U)
                  {
                    if (stringHash <= 164679485U)
                    {
                      if ((int) stringHash != 114665541)
                      {
                        if ((int) stringHash != 164679485 || !(s == "quest"))
                          return;
                        Game1.player.questLog.Add(Quest.getQuestFromId(Convert.ToInt32(strArray[1])));
                        return;
                      }
                      if (!(s == "question"))
                        return;
                      Game1.player.dialogueQuestionsAnswered.Add(Convert.ToInt32(strArray[1]));
                      return;
                    }
                    if ((int) stringHash != 166771958)
                    {
                      if ((int) stringHash != 217793883 || !(s == "house"))
                        return;
                      goto label_941;
                    }
                    else if (!(s == "scissors"))
                      return;
                  }
                  else if (stringHash <= 268356225U)
                  {
                    if ((int) stringHash != 250384600)
                    {
                      if ((int) stringHash != 268356225 || !(s == "houseUpgrade"))
                        return;
                      goto label_941;
                    }
                    else
                    {
                      if (!(s == "growgrass"))
                        return;
                      Game1.currentLocation.spawnWeeds(false);
                      Game1.currentLocation.growWeedGrass(Convert.ToInt32(strArray[1]));
                      return;
                    }
                  }
                  else if ((int) stringHash != 269790383)
                  {
                    if ((int) stringHash != 292255708)
                    {
                      if ((int) stringHash != 304701189 || !(s == "bundle"))
                        return;
                      for (int index = 0; index < (Game1.getLocationFromName("CommunityCenter") as CommunityCenter).bundles[Convert.ToInt32(strArray[1])].Length; ++index)
                        (Game1.getLocationFromName("CommunityCenter") as CommunityCenter).bundles[Convert.ToInt32(strArray[1])][index] = true;
                      Game1.playSound("crystal");
                      return;
                    }
                    if (!(s == "face"))
                      return;
                    goto label_1208;
                  }
                  else
                  {
                    if (!(s == "petToFarm"))
                      return;
                    (Game1.getCharacterFromName(Game1.player.getPetName(), false) as Pet).setAtFarmPosition();
                    return;
                  }
                }
                else if (stringHash <= 453405023U)
                {
                  if (stringHash <= 408921239U)
                  {
                    if (stringHash <= 341878775U)
                    {
                      if ((int) stringHash != 307718101)
                      {
                        if ((int) stringHash != 341878775 || !(s == "cooking"))
                          return;
                        using (Dictionary<string, string>.KeyCollection.Enumerator enumerator = CraftingRecipe.cookingRecipes.Keys.GetEnumerator())
                        {
                          while (enumerator.MoveNext())
                          {
                            string current = enumerator.Current;
                            if (!Game1.player.cookingRecipes.ContainsKey(current))
                              Game1.player.cookingRecipes.Add(current, 0);
                          }
                          return;
                        }
                      }
                      else
                      {
                        if (!(s == "junimoStar"))
                          return;
                        ((Game1.getLocationFromName("CommunityCenter") as CommunityCenter).characters[0] as Junimo).returnToJunimoHutToFetchStar((GameLocation) (Game1.getLocationFromName("CommunityCenter") as CommunityCenter));
                        return;
                      }
                    }
                    else if ((int) stringHash != 360098152)
                    {
                      if ((int) stringHash != 408921239 || !(s == "growAnimals"))
                        return;
                      using (Dictionary<long, FarmAnimal>.ValueCollection.Enumerator enumerator = (Game1.currentLocation as AnimalHouse).animals.Values.GetEnumerator())
                      {
                        while (enumerator.MoveNext())
                        {
                          FarmAnimal current = enumerator.Current;
                          int num = (int) current.ageWhenMature - 1;
                          current.age = num;
                          GameLocation currentLocation = Game1.currentLocation;
                          current.dayUpdate(currentLocation);
                        }
                        return;
                      }
                    }
                    else
                    {
                      if (!(s == "leaveEvent"))
                        return;
                      goto label_1010;
                    }
                  }
                  else
                  {
                    if (stringHash <= 434279500U)
                    {
                      if ((int) stringHash != 417438434)
                      {
                        if ((int) stringHash != 434279500 || !(s == "completeQuest"))
                          return;
                        Game1.player.completeQuest(Convert.ToInt32(strArray[1]));
                        return;
                      }
                      if (!(s == "stoprafting"))
                        return;
                      Game1.player.isRafting = false;
                      return;
                    }
                    if ((int) stringHash != 445690992)
                    {
                      if ((int) stringHash != 453405023 || !(s == "shears"))
                        return;
                    }
                    else
                    {
                      if (!(s == "bluebook"))
                        return;
                      Game1.player.items.Add((Item) new Blueprints());
                      return;
                    }
                  }
                }
                else if (stringHash <= 501437184U)
                {
                  if (stringHash <= 486219809U)
                  {
                    if ((int) stringHash != 474674083)
                    {
                      if ((int) stringHash != 486219809 || !(s == "removeTerrainFeatures"))
                        return;
                      goto label_1055;
                    }
                    else
                    {
                      if (!(s == "caughtFish"))
                        return;
                      goto label_1039;
                    }
                  }
                  else
                  {
                    if ((int) stringHash != 499086637)
                    {
                      if ((int) stringHash != 501437184 || !(s == "pregnant"))
                        return;
                      Game1.player.getSpouse().daysUntilBirthing = 1;
                      Game1.player.getRidOfChildren();
                      return;
                    }
                    if (!(s == "removeFurniture"))
                      return;
                    (Game1.currentLocation as DecoratableLocation).furniture.Clear();
                    return;
                  }
                }
                else
                {
                  if (stringHash <= 605731668U)
                  {
                    if ((int) stringHash != 604705968)
                    {
                      if ((int) stringHash != 605731668 || !(s == "localInfo"))
                        return;
                      Game1.debugOutput = "";
                      int num1 = 0;
                      int num2 = 0;
                      int num3 = 0;
                      foreach (TerrainFeature terrainFeature in Game1.currentLocation.terrainFeatures.Values)
                      {
                        if (terrainFeature is Grass)
                          ++num1;
                        else if (terrainFeature is Tree)
                          ++num2;
                        else
                          ++num3;
                      }
                      Game1.debugOutput = Game1.debugOutput + "Grass:" + (object) num1 + ",  ";
                      Game1.debugOutput = Game1.debugOutput + "Trees:" + (object) num2 + ",  ";
                      Game1.debugOutput = Game1.debugOutput + "Other Terrain Features:" + (object) num3 + ",  ";
                      Game1.debugOutput = Game1.debugOutput + "Objects: " + (object) Game1.currentLocation.objects.Count + ",  ";
                      Game1.debugOutput = Game1.debugOutput + "temporarySprites: " + (object) Game1.currentLocation.temporarySprites.Count + ",  ";
                      Game1.drawObjectDialogue(Game1.debugOutput);
                      return;
                    }
                    if (!(s == "completeCC"))
                      return;
                    Game1.player.mailReceived.Add("ccCraftsRoom");
                    Game1.player.mailReceived.Add("ccVault");
                    Game1.player.mailReceived.Add("ccFishTank");
                    Game1.player.mailReceived.Add("ccBoilerRoom");
                    Game1.player.mailReceived.Add("ccPantry");
                    Game1.player.mailReceived.Add("ccBulletin");
                    CommunityCenter locationFromName = Game1.getLocationFromName("CommunityCenter") as CommunityCenter;
                    for (int index = 0; index < locationFromName.areasComplete.Length; ++index)
                      locationFromName.areasComplete[index] = true;
                    return;
                  }
                  if ((int) stringHash != 627798748)
                  {
                    if ((int) stringHash != 635243468)
                    {
                      if ((int) stringHash != 654091955 || !(s == "rfh"))
                        return;
                      goto label_837;
                    }
                    else
                    {
                      if (!(s == "dayUpdate"))
                        return;
                      Game1.currentLocation.DayUpdate(Game1.dayOfMonth);
                      if (strArray.Length <= 1)
                        return;
                      for (int index = 0; index < Convert.ToInt32(strArray[1]) - 1; ++index)
                        Game1.currentLocation.DayUpdate(Game1.dayOfMonth);
                      return;
                    }
                  }
                  else
                  {
                    if (!(s == "friendAll"))
                      return;
                    using (List<NPC>.Enumerator enumerator = Utility.getAllCharacters().GetEnumerator())
                    {
                      while (enumerator.MoveNext())
                      {
                        NPC current = enumerator.Current;
                        if ((object) current != null && !Game1.player.friendships.ContainsKey(current.name))
                          Game1.player.friendships.Add(current.name, new int[6]);
                        Game1.player.changeFriendship(2500, current);
                      }
                      return;
                    }
                  }
                }
                Game1.player.addItemToInventoryBool((Item) new Shears(), false);
                return;
              }
              if (stringHash <= 945450609U)
              {
                if (stringHash <= 795343909U)
                {
                  if (stringHash <= 730356610U)
                  {
                    if (stringHash <= 702513141U)
                    {
                      if ((int) stringHash != 680058905)
                      {
                        if ((int) stringHash != 702513141 || !(s == "resource"))
                          return;
                        Debris.getDebris(Convert.ToInt32(strArray[1]), Convert.ToInt32(strArray[2]));
                        return;
                      }
                      if (!(s == "craftingrecipe"))
                        return;
                      Game1.player.craftingRecipes.Add(debugInput.Substring(debugInput.IndexOf(' ')).Trim(), 0);
                      return;
                    }
                    if ((int) stringHash != 720728591)
                    {
                      if ((int) stringHash != 730356610 || !(s == "clone"))
                        return;
                      Game1.currentLocation.characters.Add(Game1.getCharacterFromName(strArray[1], false));
                      return;
                    }
                    if (!(s == "TV"))
                      return;
                    Game1.player.addItemToInventoryBool((Item) new TV(Game1.random.NextDouble() < 0.5 ? 1466 : 1468, Vector2.Zero), false);
                    return;
                  }
                  if (stringHash <= 760682489U)
                  {
                    if ((int) stringHash != 759299508)
                    {
                      if ((int) stringHash != 760682489 || !(s == "setstat"))
                        return;
                      Game1.stats.GetType().GetProperty(strArray[1]).SetValue((object) Game1.stats, (object) Convert.ToUInt32(strArray[2]), (object[]) null);
                      return;
                    }
                    if (!(s == "canmove"))
                      return;
                    goto label_1166;
                  }
                  else if ((int) stringHash != 788059154)
                  {
                    if ((int) stringHash != 795343909 || !(s == "growCrops"))
                      return;
                    using (Dictionary<Vector2, TerrainFeature>.Enumerator enumerator = Game1.currentLocation.terrainFeatures.GetEnumerator())
                    {
                      while (enumerator.MoveNext())
                      {
                        KeyValuePair<Vector2, TerrainFeature> current = enumerator.Current;
                        if (current.Value is HoeDirt && (current.Value as HoeDirt).crop != null)
                        {
                          for (int index = 0; index < Convert.ToInt32(strArray[1]); ++index)
                          {
                            if ((current.Value as HoeDirt).crop != null)
                              (current.Value as HoeDirt).crop.newDay(1, -1, (int) current.Key.X, (int) current.Key.Y, Game1.getLocationFromName("Farm"));
                          }
                        }
                      }
                      return;
                    }
                  }
                  else
                  {
                    if (!(s == "upgradebarn"))
                      return;
                    goto label_1142;
                  }
                }
                else if (stringHash <= 847060761U)
                {
                  if (stringHash <= 821490340U)
                  {
                    if ((int) stringHash != 814732343)
                    {
                      if ((int) stringHash != 821490340 || !(s == "busDriveBack"))
                        return;
                      (Game1.getLocationFromName("BusStop") as BusStop).busDriveBack();
                      return;
                    }
                    if (!(s == "removeTF"))
                      return;
                  }
                  else
                  {
                    if ((int) stringHash != 838844761)
                    {
                      if ((int) stringHash != 847060761 || !(s == "conventionMode"))
                        return;
                      Game1.conventionMode = !Game1.conventionMode;
                      return;
                    }
                    if (!(s == "junimoNote"))
                      return;
                    goto label_992;
                  }
                }
                else if (stringHash <= 894566304U)
                {
                  if ((int) stringHash != 893774473)
                  {
                    if ((int) stringHash != 894566304 || !(s == "sb"))
                      return;
                    Game1.getCharacterFromName(strArray[1], false).showTextAboveHead(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3206"), -1, 2, 3000, 0);
                    return;
                  }
                  if (!(s == "resetJunimoNotes"))
                    return;
                  using (Dictionary<int, bool[]>.ValueCollection.Enumerator enumerator = (Game1.getLocationFromName("CommunityCenter") as CommunityCenter).bundles.Values.GetEnumerator())
                  {
                    while (enumerator.MoveNext())
                    {
                      foreach (bool flag in enumerator.Current)
                        flag = false;
                    }
                    return;
                  }
                }
                else
                {
                  if ((int) stringHash != 908429225)
                  {
                    if ((int) stringHash != 941250399)
                    {
                      if ((int) stringHash != 945450609 || !(s == "upgradehouse"))
                        return;
                      Game1.player.HouseUpgradeLevel = Math.Min(3, Game1.player.HouseUpgradeLevel + 1);
                      Game1.removeFrontLayerForFarmBuildings();
                      Game1.addNewFarmBuildingMaps();
                      return;
                    }
                    if (!(s == "ee"))
                      return;
                    Game1.pauseTime = 0.0f;
                    Game1.nonWarpFade = true;
                    Game1.eventFinished();
                    Game1.fadeScreenToBlack();
                    Game1.viewportFreeze = false;
                    return;
                  }
                  if (!(s == "hurry"))
                    return;
                  Game1.getCharacterFromName(strArray[1], false).warpToPathControllerDestination();
                  return;
                }
              }
              else if (stringHash <= 1094220446U)
              {
                if (stringHash <= 1043048946U)
                {
                  if (stringHash <= 979982427U)
                  {
                    if ((int) stringHash != 961676780)
                    {
                      if ((int) stringHash != 979982427 || !(s == "heal"))
                        return;
                      Game1.player.health = Game1.player.maxHealth;
                      return;
                    }
                    if (!(s == "sf"))
                      return;
                    goto label_1009;
                  }
                  else if ((int) stringHash != 1001919309)
                  {
                    if ((int) stringHash != 1043048946 || !(s == "bc"))
                      return;
                    goto label_886;
                  }
                  else
                  {
                    if (!(s == "makeInedible") || Game1.player.ActiveObject == null)
                      return;
                    Game1.player.ActiveObject.edibility = -300;
                    return;
                  }
                }
                else if (stringHash <= 1051169212U)
                {
                  if ((int) stringHash != 1049719538)
                  {
                    if ((int) stringHash != 1051169212 || !(s == "fenceDecay"))
                      return;
                    using (Dictionary<Vector2, Object>.ValueCollection.Enumerator enumerator = Game1.currentLocation.objects.Values.GetEnumerator())
                    {
                      while (enumerator.MoveNext())
                      {
                        object current = (object) enumerator.Current;
                        if (current is Fence)
                          (current as Fence).health -= (float) Convert.ToInt32(strArray[1]);
                      }
                      return;
                    }
                  }
                  else
                  {
                    if (!(s == "toss"))
                      return;
                    List<TemporaryAnimatedSprite> temporarySprites = Game1.currentLocation.TemporarySprites;
                    TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(738, 2700f, 1, 0, Game1.player.getTileLocation() * (float) Game1.tileSize, false, false);
                    temporaryAnimatedSprite.rotationChange = (float) Math.PI / 32f;
                    Vector2 vector2_1 = new Vector2(0.0f, -6f);
                    temporaryAnimatedSprite.motion = vector2_1;
                    Vector2 vector2_2 = new Vector2(0.0f, 0.08f);
                    temporaryAnimatedSprite.acceleration = vector2_2;
                    temporarySprites.Add(temporaryAnimatedSprite);
                    return;
                  }
                }
                else if ((int) stringHash != 1059385280)
                {
                  if ((int) stringHash != 1094220446 || !(s == "in"))
                    return;
                  goto label_1100;
                }
                else
                {
                  if (!(s == "al"))
                    return;
                  goto label_897;
                }
              }
              else if (stringHash <= 1130187492U)
              {
                if (stringHash <= 1101462628U)
                {
                  if ((int) stringHash != 1097324755)
                  {
                    if ((int) stringHash != 1101462628 || !(s == "completeJoja"))
                      return;
                    Game1.player.mailReceived.Add("ccCraftsRoom");
                    Game1.player.mailReceived.Add("ccVault");
                    Game1.player.mailReceived.Add("ccFishTank");
                    Game1.player.mailReceived.Add("ccBoilerRoom");
                    Game1.player.mailReceived.Add("ccPantry");
                    Game1.player.mailReceived.Add("jojaCraftsRoom");
                    Game1.player.mailReceived.Add("jojaVault");
                    Game1.player.mailReceived.Add("jojaFishTank");
                    Game1.player.mailReceived.Add("jojaBoilerRoom");
                    Game1.player.mailReceived.Add("jojaPantry");
                    Game1.player.mailReceived.Add("JojaMember");
                    return;
                  }
                  if (!(s == "zl"))
                    return;
                  goto label_780;
                }
                else if ((int) stringHash != 1129452970)
                {
                  if ((int) stringHash != 1130187492 || !(s == "eventseen"))
                    return;
                  goto label_1136;
                }
                else
                {
                  if (!(s == "sl"))
                    return;
                  Game1.player.shiftToolbar(false);
                  return;
                }
              }
              else
              {
                if (stringHash <= 1141880953U)
                {
                  if ((int) stringHash != 1138976003)
                  {
                    if ((int) stringHash != 1141880953 || !(s == "specials"))
                      return;
                    Game1.player.hasRustyKey = true;
                    Game1.player.hasSkullKey = true;
                    Game1.player.hasSpecialCharm = true;
                    Game1.player.hasDarkTalisman = true;
                    Game1.player.hasMagicInk = true;
                    Game1.player.hasClubCard = true;
                    Game1.player.canUnderstandDwarves = true;
                    return;
                  }
                  if (!(s == "shirt"))
                    return;
                  Game1.player.changeShirt(Convert.ToInt32(strArray[1]));
                  return;
                }
                if ((int) stringHash != 1143714660)
                {
                  if ((int) stringHash != 1145980326)
                  {
                    if ((int) stringHash != 1147755170 || !(s == "removeDebris"))
                      return;
                    goto label_1082;
                  }
                  else
                  {
                    if (!(s == "pm"))
                      return;
                    goto label_948;
                  }
                }
                else
                {
                  if (!(s == "bi"))
                    return;
                  goto label_1164;
                }
              }
label_1055:
              Game1.currentLocation.terrainFeatures.Clear();
              return;
            }
            if (stringHash <= 1553132714U)
            {
              if (stringHash <= 1331031788U)
              {
                if (stringHash <= 1226617017U)
                {
                  if (stringHash <= 1174793874U)
                  {
                    if (stringHash <= 1161685201U)
                    {
                      if ((int) stringHash != 1160050994)
                      {
                        if ((int) stringHash != 1161685201 || !(s == "facePlayer"))
                          return;
                        Game1.getCharacterFromName(strArray[1], false).faceTowardFarmer = true;
                        return;
                      }
                      if (!(s == "aj"))
                        return;
                      goto label_984;
                    }
                    else
                    {
                      if ((int) stringHash != 1163008208)
                      {
                        if ((int) stringHash != 1174793874 || !(s == "deleteArch"))
                          return;
                        Game1.player.archaeologyFound.Clear();
                        Game1.player.fishCaught.Clear();
                        Game1.player.mineralsFound.Clear();
                        Game1.player.mailReceived.Clear();
                        return;
                      }
                      if (!(s == "sr"))
                        return;
                      Game1.player.shiftToolbar(true);
                      return;
                    }
                  }
                  else if (stringHash <= 1181855383U)
                  {
                    if ((int) stringHash != 1177961446)
                    {
                      if ((int) stringHash != 1181855383 || !(s == "version"))
                        return;
                      Game1.debugOutput = string.Concat((object) typeof (Game1).Assembly.GetName().Version);
                      return;
                    }
                    if (!(s == "ns"))
                      return;
                    goto label_834;
                  }
                  else
                  {
                    if ((int) stringHash != 1198881991)
                    {
                      if ((int) stringHash != 1226617017 || !(s == "dp"))
                        return;
                      Game1.stats.daysPlayed = (uint) Convert.ToInt32(strArray[1]);
                      return;
                    }
                    if (!(s == "killMonsterStat"))
                      return;
                  }
                }
                else if (stringHash <= 1278921350U)
                {
                  if (stringHash <= 1266391031U)
                  {
                    if ((int) stringHash != 1237752336)
                    {
                      if ((int) stringHash != 1266391031 || !(s == "wedding"))
                        return;
                      Game1.player.spouse = strArray[1];
                      Game1.prepareSpouseForWedding();
                      Game1.checkForWedding();
                      return;
                    }
                    if (!(s == "water"))
                      return;
                    using (Dictionary<Vector2, TerrainFeature>.ValueCollection.Enumerator enumerator = Game1.currentLocation.terrainFeatures.Values.GetEnumerator())
                    {
                      while (enumerator.MoveNext())
                      {
                        TerrainFeature current = enumerator.Current;
                        if (current is HoeDirt)
                          (current as HoeDirt).state = 1;
                      }
                      return;
                    }
                  }
                  else if ((int) stringHash != 1274636741)
                  {
                    if ((int) stringHash != 1278921350 || !(s == "hu"))
                      return;
                    goto label_941;
                  }
                  else
                  {
                    if (!(s == "eventOver"))
                      return;
                    Game1.eventFinished();
                    return;
                  }
                }
                else if (stringHash <= 1298661701U)
                {
                  if ((int) stringHash != 1279593123)
                  {
                    if ((int) stringHash != 1298661701 || !(s == "festivalScore"))
                      return;
                    Game1.player.festivalScore += Convert.ToInt32(strArray[1]);
                    return;
                  }
                  if (!(s == "growAnimalsFarm"))
                    return;
                  using (Dictionary<long, FarmAnimal>.ValueCollection.Enumerator enumerator = (Game1.currentLocation as Farm).animals.Values.GetEnumerator())
                  {
                    while (enumerator.MoveNext())
                    {
                      FarmAnimal current = enumerator.Current;
                      if (current.isBaby())
                      {
                        current.age = (int) current.ageWhenMature - 1;
                        current.dayUpdate(Game1.currentLocation);
                      }
                    }
                    return;
                  }
                }
                else if ((int) stringHash != 1302939849)
                {
                  if ((int) stringHash != 1330092850)
                  {
                    if ((int) stringHash != 1331031788 || !(s == "pan"))
                      return;
                    Game1.player.addItemToInventoryBool((Item) new Pan(), false);
                    return;
                  }
                  if (!(s == "wp"))
                    return;
                  goto label_932;
                }
                else
                {
                  if (!(s == "seenmail"))
                    return;
                  Game1.player.mailReceived.Add(strArray[1]);
                  return;
                }
              }
              else if (stringHash <= 1461503683U)
              {
                if (stringHash <= 1395526040U)
                {
                  if (stringHash <= 1383527311U)
                  {
                    if ((int) stringHash != 1366625991)
                    {
                      if ((int) stringHash != 1383527311 || !(s == "panmode"))
                        return;
                      goto label_948;
                    }
                    else
                    {
                      if (!(s == "tool"))
                        return;
                      Game1.player.getToolFromName(strArray[1]).UpgradeLevel = Convert.ToInt32(strArray[2]);
                      return;
                    }
                  }
                  else
                  {
                    if ((int) stringHash != 1394937660)
                    {
                      if ((int) stringHash != 1395526040 || !(s == "mp"))
                        return;
                      Game1.player.addItemToInventoryBool((Item) new MilkPail(), false);
                      return;
                    }
                    if (!(s == "ax"))
                      return;
                    Game1.player.addItemToInventoryBool((Item) new Axe(), false);
                    Game1.playSound("coin");
                    return;
                  }
                }
                else if (stringHash <= 1448470768U)
                {
                  if ((int) stringHash != 1405799865)
                  {
                    if ((int) stringHash != 1448470768 || !(s == "whereis"))
                      return;
                    goto label_947;
                  }
                  else
                  {
                    if (!(s == "big"))
                      return;
                    goto label_1164;
                  }
                }
                else
                {
                  if ((int) stringHash != 1457159580)
                  {
                    if ((int) stringHash != 1461503683 || !(s == "db"))
                      return;
                    Game1.activeClickableMenu = (IClickableMenu) new DialogueBox(Game1.getCharacterFromName(strArray.Length > 1 ? strArray[1] : "Pierre", false).CurrentDialogue.Peek());
                    return;
                  }
                  if (!(s == "barn"))
                    return;
                  goto label_1142;
                }
              }
              else if (stringHash <= 1520724356U)
              {
                if (stringHash <= 1490091188U)
                {
                  if ((int) stringHash != 1475248161)
                  {
                    if ((int) stringHash != 1490091188 || !(s == "refuel") || Game1.player.getToolFromName("Lantern") == null)
                      return;
                    ((Lantern) Game1.player.getToolFromName("Lantern")).fuelLeft = 100;
                    return;
                  }
                  if (!(s == "frameOffset"))
                    return;
                  goto label_841;
                }
                else
                {
                  if ((int) stringHash != 1496191754)
                  {
                    if ((int) stringHash != 1520724356 || !(s == "resetAchievements"))
                      return;
                    Program.sdk.ResetAchievements();
                    return;
                  }
                  if (!(s == "mv"))
                    return;
                  goto label_1095;
                }
              }
              else if (stringHash <= 1534898856U)
              {
                if ((int) stringHash != 1523588291)
                {
                  if ((int) stringHash != 1534898856 || !(s == "readyForHarvest"))
                    return;
                  goto label_837;
                }
                else
                {
                  if (!(s == "emote"))
                    return;
                  Game1.player.doEmote(Convert.ToInt32(strArray[1]));
                  return;
                }
              }
              else
              {
                if ((int) stringHash != 1546774874)
                {
                  if ((int) stringHash != 1547417870)
                  {
                    if ((int) stringHash != 1553132714 || !(s == "removeLargeTF"))
                      return;
                    Game1.currentLocation.largeTerrainFeatures.Clear();
                    return;
                  }
                  if (!(s == "spawnweeds"))
                    return;
                  for (int index = 0; index < Convert.ToInt32(strArray[1]); ++index)
                    Game1.currentLocation.spawnWeedsAndStones(1, false, true);
                  return;
                }
                if (!(s == "lu"))
                  return;
                goto label_974;
              }
            }
            else if (stringHash <= 1763898183U)
            {
              if (stringHash <= 1646454850U)
              {
                if (stringHash <= 1582198420U)
                {
                  if (stringHash <= 1573653271U)
                  {
                    if ((int) stringHash != 1564253156)
                    {
                      if ((int) stringHash != 1573653271 || !(s == "speech"))
                        return;
                      Game1.getCharacterFromName(strArray[1], false).CurrentDialogue.Push(new Dialogue(debugInput.Substring(debugInput.IndexOf("0") + 1), Game1.getCharacterFromName(strArray[1], false)));
                      Game1.drawDialogue(Game1.getCharacterFromName(strArray[1], false));
                      return;
                    }
                    if (!(s == "time"))
                      return;
                    Game1.timeOfDay = Convert.ToInt32(strArray[1]);
                    Game1.outdoorLight = Color.White;
                    return;
                  }
                  if ((int) stringHash != 1581757135)
                  {
                    if ((int) stringHash != 1582198420 || !(s == "ps"))
                      return;
                    goto label_1025;
                  }
                  else
                  {
                    if (!(s == "wc"))
                      return;
                    goto label_1184;
                  }
                }
                else if (stringHash <= 1638190062U)
                {
                  if ((int) stringHash != 1629900385)
                  {
                    if ((int) stringHash != 1638190062 || !(s == "fruitTrees"))
                      return;
                    using (Dictionary<Vector2, TerrainFeature>.Enumerator enumerator = Game1.currentLocation.terrainFeatures.GetEnumerator())
                    {
                      while (enumerator.MoveNext())
                      {
                        KeyValuePair<Vector2, TerrainFeature> current = enumerator.Current;
                        if (current.Value is FruitTree)
                        {
                          (current.Value as FruitTree).daysUntilMature -= 27;
                          current.Value.dayUpdate(Game1.currentLocation, current.Key);
                        }
                      }
                      return;
                    }
                  }
                  else
                  {
                    if (!(s == "skullkey"))
                      return;
                    Game1.player.hasSkullKey = true;
                    return;
                  }
                }
                else if ((int) stringHash != 1644312386)
                {
                  if ((int) stringHash != 1646454850 || !(s == "fo"))
                    return;
                  goto label_841;
                }
                else if (!(s == "kms"))
                  return;
              }
              else if (stringHash <= 1697079146U)
              {
                if (stringHash <= 1680451373U)
                {
                  if ((int) stringHash != 1665100777)
                  {
                    if ((int) stringHash != 1680451373 || !(s == "cm"))
                      return;
                    goto label_1166;
                  }
                  else
                  {
                    if (!(s == "jn"))
                      return;
                    goto label_992;
                  }
                }
                else
                {
                  if ((int) stringHash != 1684539335)
                  {
                    if ((int) stringHash != 1697079146 || !(s == "toggleCatPerson"))
                      return;
                    Game1.player.catPerson = !Game1.player.catPerson;
                    return;
                  }
                  if (!(s == "nosave"))
                    return;
                  goto label_834;
                }
              }
              else if (stringHash <= 1730342945U)
              {
                if ((int) stringHash != 1697114278)
                {
                  if ((int) stringHash != 1730342945 || !(s == "fb"))
                    return;
                  goto label_853;
                }
                else
                {
                  if (!(s == "upgradecoop"))
                    return;
                  goto label_1141;
                }
              }
              else if ((int) stringHash != 1735922541)
              {
                if ((int) stringHash != 1758887219)
                {
                  if ((int) stringHash != 1763898183 || !(s == "fd"))
                    return;
                  goto label_1208;
                }
                else
                {
                  if (!(s == "pole"))
                    return;
                  Game1.player.addItemToInventoryBool((Item) new FishingRod(strArray.Length > 1 ? Convert.ToInt32(strArray[1]) : 0), false);
                  return;
                }
              }
              else
              {
                if (!(s == "animalInfo"))
                  return;
                Game1.showGlobalMessage(string.Concat((object) Game1.getFarm().getAllFarmAnimals().Count));
                return;
              }
            }
            else if (stringHash <= 1883325193U)
            {
              if (stringHash <= 1811284518U)
              {
                if (stringHash <= 1787721130U)
                {
                  if ((int) stringHash != 1769407897)
                  {
                    if ((int) stringHash != 1787721130 || !(s == "end"))
                      return;
                    Game1.warpFarmer("Town", 20, 20, false);
                    Game1.getLocationFromName("Town").currentEvent = new Event(Utility.getStardewHeroCelebrationEventString(90), -1);
                    this.makeCelebrationWeatherDebris();
                    Utility.perpareDayForStardewCelebration(90);
                    return;
                  }
                  if (!(s == "pickax"))
                    return;
                  goto label_1204;
                }
                else if ((int) stringHash != 1797453421)
                {
                  if ((int) stringHash != 1811284518 || !(s == "musicvolume"))
                    return;
                  goto label_1095;
                }
                else
                {
                  if (!(s == "ff"))
                    return;
                  goto label_934;
                }
              }
              else if (stringHash <= 1840121496U)
              {
                if ((int) stringHash != 1828144719)
                {
                  if ((int) stringHash != 1840121496 || !(s == "pixelZoom"))
                    return;
                  Game1.pixelZoom = Convert.ToInt32(strArray[1]);
                  Game1.tileSize = Game1.pixelZoom * 16;
                  Layer.m_tileSize = new Size(Game1.tileSize, Game1.tileSize);
                  Projectile.boundingBoxWidth = Game1.tileSize / 4;
                  Projectile.boundingBoxHeight = Game1.tileSize / 4;
                  GreenSlime.matingRange = Game1.tileSize * 3;
                  SliderBar.defaultWidth = Game1.tileSize * 2;
                  IClickableMenu.borderWidth = Game1.tileSize / 2 + Game1.tileSize / 8;
                  IClickableMenu.tabYPositionRelativeToMenuY = -Game1.tileSize * 3 / 4;
                  IClickableMenu.spaceToClearTopBorder = Game1.tileSize * 3 / 2;
                  IClickableMenu.spaceToClearSideBorder = Game1.tileSize / 4;
                  BuffsDisplay.width = Game1.tileSize * 4 + Game1.tileSize / 2;
                  BuffsDisplay.sideSpace = Game1.tileSize / 2;
                  Farmer.tileSlideThreshold = Game1.tileSize / 2;
                  Coop.openAnimalDoorPosition = -Game1.tileSize + Game1.pixelZoom * 3;
                  Barn.openAnimalDoorPosition = -Game1.tileSize - 24 + Game1.pixelZoom * 3;
                  return;
                }
                if (!(s == "mainMenu"))
                  return;
                goto label_1082;
              }
              else if ((int) stringHash != 1848911934)
              {
                if ((int) stringHash != 1865621569)
                {
                  if ((int) stringHash != 1883325193 || !(s == "wallpaper"))
                    return;
                  goto label_932;
                }
                else
                {
                  if (!(s == "weapon"))
                    return;
                  Game1.player.addItemToInventoryBool((Item) new MeleeWeapon(Convert.ToInt32(strArray[1])), false);
                  return;
                }
              }
              else
              {
                if (!(s == "darkTalisman"))
                  return;
                Game1.player.hasDarkTalisman = true;
                Game1.getLocationFromName("Railroad").setMapTile(54, 35, 287, "Buildings", "", 1);
                Game1.getLocationFromName("Railroad").setMapTile(54, 34, 262, "Front", "", 1);
                Game1.getLocationFromName("WitchHut").setMapTile(4, 11, 114, "Buildings", "", 1);
                Game1.getLocationFromName("WitchHut").setTileProperty(4, 11, "Buildings", "Action", "MagicInk");
                Game1.player.hasMagicInk = false;
                Game1.player.mailReceived.Clear();
                return;
              }
            }
            else if (stringHash <= 2018864233U)
            {
              if (stringHash <= 1975629332U)
              {
                if ((int) stringHash != 1916524223)
                {
                  if ((int) stringHash != 1975629332 || !(s == "achieve"))
                    return;
                  Program.sdk.GetAchievement(strArray[1]);
                  return;
                }
                if (!(s == "sdkinfo"))
                  return;
                goto label_823;
              }
              else
              {
                if ((int) stringHash != 2016676370)
                {
                  if ((int) stringHash != 2018864233 || !(s == "train"))
                    return;
                  (Game1.getLocationFromName("Railroad") as Railroad).setTrainComing(7500);
                  return;
                }
                if (!(s == "bigitem"))
                  return;
                goto label_1164;
              }
            }
            else
            {
              if (stringHash <= 2042762978U)
              {
                if ((int) stringHash != 2033203167)
                {
                  if ((int) stringHash != 2042762978 || !(s == "morepollen"))
                    return;
                  for (int index = 0; index < Convert.ToInt32(strArray[1]); ++index)
                    Game1.debrisWeather.Add(new WeatherDebris(new Vector2((float) Game1.random.Next(0, Game1.graphics.GraphicsDevice.Viewport.Width), (float) Game1.random.Next(0, Game1.graphics.GraphicsDevice.Viewport.Height)), 0, (float) Game1.random.Next(15) / 500f, (float) Game1.random.Next(-10, 0) / 50f, (float) Game1.random.Next(10) / 50f));
                  return;
                }
                if (!(s == "specialItem"))
                  return;
                Game1.player.specialItems.Add(Convert.ToInt32(strArray[1]));
                return;
              }
              if ((int) stringHash != 2044027800)
              {
                if ((int) stringHash != 2066479103)
                {
                  if ((int) stringHash != 2072037248 || !(s == "speed"))
                    return;
                  Game1.player.addedSpeed = Convert.ToInt32(strArray[1]);
                  return;
                }
                if (!(s == "wand"))
                  return;
                Game1.player.addItemToInventoryBool((Item) new Wand(), false);
                Game1.playSound("coin");
                return;
              }
              if (!(s == "levelup"))
                return;
              Game1.activeClickableMenu = (IClickableMenu) new LevelUpMenu(Convert.ToInt32(strArray[1]), Convert.ToInt32(strArray[2]));
              return;
            }
            string key = strArray[1].Replace("0", " ");
            int int32 = Convert.ToInt32(strArray[2]);
            if (Game1.stats.specificMonstersKilled.ContainsKey(key))
              Game1.stats.specificMonstersKilled[key] = int32;
            else
              Game1.stats.specificMonstersKilled.Add(key, int32);
            Game1.debugOutput = Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3159", (object) key, (object) int32);
            return;
label_841:
            int num4 = strArray[2].Contains<char>('s') ? -1 : 1;
            FarmerRenderer.featureXOffsetPerFrame[Convert.ToInt32(strArray[1])] = (int) (short) (num4 * Convert.ToInt32(strArray[2].Last<char>().ToString() ?? ""));
            int num5 = strArray[3].Contains<char>('s') ? -1 : 1;
            FarmerRenderer.featureYOffsetPerFrame[Convert.ToInt32(strArray[1])] = (int) (short) (num5 * Convert.ToInt32(strArray[3].Last<char>().ToString() ?? ""));
            if (strArray.Length <= 4)
              return;
            int num6 = strArray[4].Contains<char>('s') ? -1 : 1;
            return;
label_932:
            bool isFloor = Game1.random.NextDouble() < 0.5;
            Game1.player.addItemToInventoryBool((Item) new Wallpaper(isFloor ? Game1.random.Next(40) : Game1.random.Next(112), isFloor), false);
            return;
label_837:
            Game1.currentLocation.objects[new Vector2((float) Convert.ToInt32(strArray[1]), (float) Convert.ToInt32(strArray[2]))].minutesUntilReady = 1;
            return;
label_941:
            (Game1.getLocationFromName("FarmHouse") as FarmHouse).moveObjectsForHouseUpgrade(Convert.ToInt32(strArray[1]));
            (Game1.getLocationFromName("FarmHouse") as FarmHouse).setMapForUpgradeLevel(Convert.ToInt32(strArray[1]), true);
            Game1.player.HouseUpgradeLevel = Convert.ToInt32(strArray[1]);
            Game1.removeFrontLayerForFarmBuildings();
            Game1.addNewFarmBuildingMaps();
            return;
label_947:
            Game1.debugOutput = strArray[1] + " is at " + Utility.getGameLocationOfCharacter(Game1.getCharacterFromName(strArray[1], false)).Name + ", " + (object) Game1.getCharacterFromName(strArray[1], false).getTileX() + "," + (object) Game1.getCharacterFromName(strArray[1], false).getTileY();
            return;
label_992:
            (Game1.getLocationFromName("CommunityCenter") as CommunityCenter).addJunimoNote(Convert.ToInt32(strArray[1]));
            return;
label_1082:
            Game1.currentLocation.debris.Clear();
            return;
label_1142:
            Game1.player.BarnUpgradeLevel = Math.Min(3, Game1.player.BarnUpgradeLevel + 1);
            Game1.removeFrontLayerForFarmBuildings();
            Game1.addNewFarmBuildingMaps();
            return;
          }
          if (stringHash <= 3429009752U)
          {
            if (stringHash <= 2686971291U)
            {
              if (stringHash <= 2448968664U)
              {
                if (stringHash <= 2268828259U)
                {
                  if (stringHash <= 2105453515U)
                  {
                    if (stringHash <= 2095482337U)
                    {
                      if ((int) stringHash != 2075513521)
                      {
                        if ((int) stringHash != 2095482337 || !(s == "fillbin"))
                          return;
                        goto label_853;
                      }
                      else
                      {
                        if (!(s == "fixAnimals"))
                          return;
                        Farm farm = Game1.getFarm();
                        for (int index1 = farm.buildings.Count - 1; index1 >= 0; --index1)
                        {
                          if (farm.buildings[index1].indoors != null && farm.buildings[index1].indoors is AnimalHouse)
                          {
                            foreach (FarmAnimal farmAnimal in (farm.buildings[index1].indoors as AnimalHouse).animals.Values)
                            {
                              for (int index2 = farm.buildings.Count - 1; index2 >= 0; --index2)
                              {
                                if (farm.buildings[index2].indoors != null && farm.buildings[index2].indoors is AnimalHouse && ((farm.buildings[index2].indoors as AnimalHouse).animalsThatLiveHere.Contains(farmAnimal.myID) && !farm.buildings[index2].Equals((object) farmAnimal.home)))
                                {
                                  for (int index3 = (farm.buildings[index2].indoors as AnimalHouse).animalsThatLiveHere.Count - 1; index3 >= 0; --index3)
                                  {
                                    if ((farm.buildings[index2].indoors as AnimalHouse).animalsThatLiveHere[index3] == farmAnimal.myID)
                                    {
                                      (farm.buildings[index2].indoors as AnimalHouse).animalsThatLiveHere.RemoveAt(index3);
                                      Game1.playSound("crystal");
                                    }
                                  }
                                }
                              }
                            }
                            for (int index2 = (farm.buildings[index1].indoors as AnimalHouse).animalsThatLiveHere.Count - 1; index2 >= 0; --index2)
                            {
                              if (Utility.getAnimal((farm.buildings[index1].indoors as AnimalHouse).animalsThatLiveHere[index2]) == null)
                              {
                                (farm.buildings[index1].indoors as AnimalHouse).animalsThatLiveHere.RemoveAt(index2);
                                Game1.playSound("crystal");
                              }
                            }
                          }
                        }
                        return;
                      }
                    }
                    else
                    {
                      if ((int) stringHash != 2096616551)
                      {
                        if ((int) stringHash != 2105453515 || !(s == "backpack"))
                          return;
                        Game1.player.increaseBackpackSize(Math.Min(20 - Game1.player.maxItems, Convert.ToInt32(strArray[1])));
                        return;
                      }
                      if (!(s == "kid"))
                        return;
                      goto label_907;
                    }
                  }
                  else
                  {
                    if (stringHash <= 2160778853U)
                    {
                      if ((int) stringHash != 2137427461)
                      {
                        if ((int) stringHash != -2134188443 || !(s == "allbundles"))
                          return;
                        foreach (KeyValuePair<int, bool[]> bundle in (Dictionary<int, bool[]>) (Game1.getLocationFromName("CommunityCenter") as CommunityCenter).bundles)
                        {
                          for (int index = 0; index < bundle.Value.Length; ++index)
                            bundle.Value[index] = true;
                        }
                        Game1.playSound("crystal");
                        return;
                      }
                      if (!(s == "divorce"))
                        return;
                      Game1.player.divorceTonight = true;
                      return;
                    }
                    if ((int) stringHash != -2082919916)
                    {
                      if ((int) stringHash != -2026139037 || !(s == "spreadDirt"))
                        return;
                      Farm farm = Game1.getFarm();
                      for (int xTile = 0; xTile < farm.map.Layers[0].LayerWidth; ++xTile)
                      {
                        for (int yTile = 0; yTile < farm.map.Layers[0].LayerHeight; ++yTile)
                        {
                          if (!farm.terrainFeatures.ContainsKey(new Vector2((float) xTile, (float) yTile)) && farm.doesTileHaveProperty(xTile, yTile, "Diggable", "Back") != null && farm.isTileLocationTotallyClearAndPlaceable(new Vector2((float) xTile, (float) yTile)))
                            farm.terrainFeatures.Add(new Vector2((float) xTile, (float) yTile), (TerrainFeature) new HoeDirt());
                        }
                      }
                      return;
                    }
                    if (!(s == "movebuilding"))
                      return;
                    Game1.getFarm().getBuildingAt(new Vector2((float) Convert.ToInt32(strArray[1]), (float) Convert.ToInt32(strArray[2]))).tileX = Convert.ToInt32(strArray[3]);
                    Game1.getFarm().getBuildingAt(new Vector2((float) Convert.ToInt32(strArray[1]), (float) Convert.ToInt32(strArray[2]))).tileY = Convert.ToInt32(strArray[4]);
                    return;
                  }
                }
                else if (stringHash <= 2382613700U)
                {
                  if (stringHash <= 2309207507U)
                  {
                    if ((int) stringHash != -2019873658)
                    {
                      if ((int) stringHash != -1985759789 || !(s == "rain"))
                        return;
                      Game1.isRaining = !Game1.isRaining;
                      Game1.isDebrisWeather = false;
                      return;
                    }
                    if (!(s == "mailForTomorrow"))
                      return;
                    goto label_946;
                  }
                  else
                  {
                    if ((int) stringHash != -1958008074)
                    {
                      if ((int) stringHash != -1912353596 || !(s == "minelevel") || Game1.mine == null)
                        return;
                      Game1.enterMine(false, Convert.ToInt32(strArray[1]), (string) null);
                      return;
                    }
                    if (!(s == "warpCharacter"))
                      return;
                    goto label_1184;
                  }
                }
                else
                {
                  if (stringHash <= 2398780079U)
                  {
                    if ((int) stringHash != -1901452349)
                    {
                      if ((int) stringHash != -1896187217 || !(s == "warp"))
                        return;
                      Game1.warpFarmer(strArray[1], Convert.ToInt32(strArray[2]), Convert.ToInt32(strArray[3]), false);
                      return;
                    }
                    if (!(s == "ccload"))
                      return;
                    (Game1.getLocationFromName("CommunityCenter") as CommunityCenter).loadArea(Convert.ToInt32(strArray[1]), true);
                    (Game1.getLocationFromName("CommunityCenter") as CommunityCenter).areasComplete[Convert.ToInt32(strArray[1])] = true;
                    return;
                  }
                  if ((int) stringHash != -1852175299)
                  {
                    if ((int) stringHash != -1849220622)
                    {
                      if ((int) stringHash != -1845998632 || !(s == "minigame"))
                        return;
                      string str = strArray[1];
                      if (!(str == "cowboy"))
                      {
                        if (!(str == "blastoff"))
                        {
                          if (!(str == "minecart"))
                          {
                            if (!(str == "grandpa"))
                              return;
                            Game1.currentMinigame = (IMinigame) new GrandpaStory();
                            return;
                          }
                          Game1.currentMinigame = (IMinigame) new MineCart(5, 4);
                          return;
                        }
                        Game1.currentMinigame = (IMinigame) new RobotBlastoff();
                        return;
                      }
                      Game1.updateViewportForScreenSizeChange(false, Game1.graphics.PreferredBackBufferWidth, Game1.graphics.PreferredBackBufferHeight);
                      Game1.currentMinigame = (IMinigame) new AbigailGame(false);
                      return;
                    }
                    if (!(s == "clearMuseum"))
                      return;
                    (Game1.getLocationFromName("ArchaeologyHouse") as LibraryMuseum).museumPieces.Clear();
                    return;
                  }
                  if (!(s == "note"))
                    return;
                  if (!Game1.player.archaeologyFound.ContainsKey(102))
                    Game1.player.archaeologyFound.Add(102, new int[2]);
                  Game1.player.archaeologyFound[102][0] = 18;
                  Game1.currentLocation.readNote(Convert.ToInt32(strArray[1]));
                  return;
                }
              }
              else if (stringHash <= 2565552814U)
              {
                if (stringHash <= 2528307764U)
                {
                  if (stringHash <= 2513359314U)
                  {
                    if ((int) stringHash != -1829637335)
                    {
                      if ((int) stringHash != -1781607982 || !(s == "makeEx"))
                        return;
                      Game1.getCharacterFromName(strArray[1], false).divorcedFromFarmer = true;
                      return;
                    }
                    if (!(s == "experience"))
                      return;
                    Game1.player.gainExperience(Convert.ToInt32(strArray[1]), Convert.ToInt32(strArray[2]));
                    return;
                  }
                  if ((int) stringHash != -1779656205)
                  {
                    if ((int) stringHash != -1766659532 || !(s == "killNPC"))
                      return;
                    for (int index1 = Game1.locations.Count - 1; index1 >= 0; --index1)
                    {
                      for (int index2 = 0; index2 < Game1.locations[index1].characters.Count; ++index2)
                      {
                        if (Game1.locations[index1].characters[index2].name.Equals(strArray[1]))
                        {
                          Game1.locations[index1].characters.RemoveAt(index2);
                          break;
                        }
                      }
                    }
                    return;
                  }
                  if (!(s == "itemnamed"))
                    return;
                  goto label_1100;
                }
                else if (stringHash <= 2537565969U)
                {
                  if ((int) stringHash != -1761238774)
                  {
                    if ((int) stringHash != -1757401327 || !(s == "befriendAnimals") || !(Game1.currentLocation is AnimalHouse))
                      return;
                    using (Dictionary<long, FarmAnimal>.ValueCollection.Enumerator enumerator = (Game1.currentLocation as AnimalHouse).animals.Values.GetEnumerator())
                    {
                      while (enumerator.MoveNext())
                        enumerator.Current.friendshipTowardFarmer = strArray.Length > 1 ? Convert.ToInt32(strArray[1]) : 1000;
                      return;
                    }
                  }
                  else
                  {
                    if (!(s == "setUpFarm"))
                      return;
                    Game1.getFarm().buildings.Clear();
                    for (int x = 0; x < Game1.getFarm().map.Layers[0].LayerWidth; ++x)
                    {
                      for (int y = 0; y < 16 + (strArray.Length > 1 ? 32 : 0); ++y)
                        Game1.getFarm().removeEverythingExceptCharactersFromThisTile(x, y);
                    }
                    for (int x = 56; x < 71; ++x)
                    {
                      for (int y = 17; y < 34; ++y)
                      {
                        Game1.getFarm().removeEverythingExceptCharactersFromThisTile(x, y);
                        if (x > 57 && y > 18 && (x < 70 && y < 29))
                          Game1.getFarm().terrainFeatures.Add(new Vector2((float) x, (float) y), (TerrainFeature) new HoeDirt());
                      }
                    }
                    Game1.getFarm().buildStructure(new BluePrint("Coop"), new Vector2(52f, 11f), false, Game1.player, false);
                    Game1.getFarm().buildings.Last<Building>().daysOfConstructionLeft = 0;
                    Game1.getFarm().buildStructure(new BluePrint("Silo"), new Vector2(36f, 9f), false, Game1.player, false);
                    Game1.getFarm().buildings.Last<Building>().daysOfConstructionLeft = 0;
                    Game1.getFarm().buildStructure(new BluePrint("Barn"), new Vector2(42f, 10f), false, Game1.player, false);
                    Game1.getFarm().buildings.Last<Building>().daysOfConstructionLeft = 0;
                    Game1.player.getToolFromName("Ax").UpgradeLevel = 4;
                    Game1.player.getToolFromName("Watering Can").UpgradeLevel = 4;
                    Game1.player.getToolFromName("Hoe").UpgradeLevel = 4;
                    Game1.player.getToolFromName("Pickaxe").UpgradeLevel = 4;
                    Game1.player.Money += 20000;
                    Game1.player.addItemToInventoryBool((Item) new Shears(), false);
                    Game1.player.addItemToInventoryBool((Item) new MilkPail(), false);
                    Game1.player.addItemToInventoryBool((Item) new Object(472, 999, false, -1, 0), false);
                    Game1.player.addItemToInventoryBool((Item) new Object(473, 999, false, -1, 0), false);
                    Game1.player.addItemToInventoryBool((Item) new Object(322, 999, false, -1, 0), false);
                    Game1.player.addItemToInventoryBool((Item) new Object(388, 999, false, -1, 0), false);
                    Game1.player.addItemToInventoryBool((Item) new Object(390, 999, false, -1, 0), false);
                    return;
                  }
                }
                else if ((int) stringHash != -1732058027)
                {
                  if ((int) stringHash != -1729414482 || !(s == "fillbackpack"))
                    return;
                  goto label_734;
                }
                else
                {
                  if (!(s == "ambientLight"))
                    return;
                  goto label_897;
                }
              }
              else if (stringHash <= 2606942237U)
              {
                if (stringHash <= 2573284398U)
                {
                  if ((int) stringHash != -1726178819)
                  {
                    if ((int) stringHash != -1721682898 || !(s == "slingshot"))
                      return;
                    Game1.player.addItemToInventoryBool((Item) new Slingshot(), false);
                    Game1.playSound("coin");
                    return;
                  }
                  if (!(s == "addOtherFarmer"))
                    return;
                  Farmer owner = new Farmer(new FarmerSprite(Game1.content.Load<Texture2D>("Characters\\Farmer\\farmer_base")), new Vector2(Game1.player.position.X - (float) Game1.tileSize, Game1.player.position.Y), 2, Dialogue.randomName(), (List<Item>) null, true);
                  owner.changeShirt(Game1.random.Next(40));
                  owner.changePants(new Color(Game1.random.Next((int) byte.MaxValue), Game1.random.Next((int) byte.MaxValue), Game1.random.Next((int) byte.MaxValue)));
                  owner.changeHairStyle(Game1.random.Next(FarmerRenderer.hairStylesTexture.Height / 96 * 8));
                  if (Game1.random.NextDouble() < 0.5)
                    owner.changeHat(Game1.random.Next(-1, FarmerRenderer.hatsTexture.Height / 80 * 12));
                  else
                    Game1.player.changeHat(-1);
                  owner.changeHairColor(new Color(Game1.random.Next((int) byte.MaxValue), Game1.random.Next((int) byte.MaxValue), Game1.random.Next((int) byte.MaxValue)));
                  owner.changeSkinColor(Game1.random.Next(16));
                  owner.FarmerSprite.setOwner(owner);
                  owner.currentLocation = Game1.currentLocation;
                  Game1.currentLocation.farmers.Add(owner);
                  Game1.otherFarmers.Add((long) Game1.random.Next(), owner);
                  return;
                }
                if ((int) stringHash != -1700198083)
                {
                  if ((int) stringHash != -1688025059 || !(s == "addAllCrafting"))
                    return;
                  using (Dictionary<string, string>.KeyCollection.Enumerator enumerator = CraftingRecipe.craftingRecipes.Keys.GetEnumerator())
                  {
                    while (enumerator.MoveNext())
                      Game1.player.craftingRecipes.Add(enumerator.Current, 0);
                    return;
                  }
                }
                else
                {
                  if (!(s == "plaque"))
                    return;
                  (Game1.getLocationFromName("CommunityCenter") as CommunityCenter).addStarToPlaque();
                  return;
                }
              }
              else if (stringHash <= 2609467814U)
              {
                if ((int) stringHash != -1685598123)
                {
                  if ((int) stringHash != -1685499482 || !(s == "horse"))
                    return;
                  Game1.currentLocation.characters.Add((NPC) new Horse(Convert.ToInt32(strArray[1]), Convert.ToInt32(strArray[2])));
                  return;
                }
                if (!(s == "cmenu"))
                  return;
                goto label_1088;
              }
              else if ((int) stringHash != -1626576569)
              {
                if ((int) stringHash != -1623706650)
                {
                  if ((int) stringHash != -1607996005 || !(s == "sprinkle"))
                    return;
                  Utility.addSprinklesToLocation(Game1.currentLocation, Game1.player.getTileX(), Game1.player.getTileY(), 7, 7, 2000, 100, Color.White, (string) null, false);
                  return;
                }
                if (!(s == "item"))
                  return;
                goto label_1098;
              }
              else if (!(s == "friendship"))
                return;
            }
            else if (stringHash <= 2949673445U)
            {
              if (stringHash <= 2885078319U)
              {
                if (stringHash <= 2805947405U)
                {
                  if (stringHash <= 2803217125U)
                  {
                    if ((int) stringHash != -1545342491)
                    {
                      if ((int) stringHash != -1491750171 || !(s == "owl"))
                        return;
                      Game1.currentLocation.addOwl();
                      return;
                    }
                    if (!(s == "removeQuest"))
                      return;
                    Game1.player.removeQuest(Convert.ToInt32(strArray[1]));
                    return;
                  }
                  if ((int) stringHash != -1490670315)
                  {
                    if ((int) stringHash != -1489019891 || !(s == "jump"))
                      return;
                    float jumpVelocity = 8f;
                    if (strArray.Length > 2)
                      jumpVelocity = (float) Convert.ToDouble(strArray[2]);
                    if (strArray[1].Equals("farmer"))
                    {
                      Game1.player.jump(jumpVelocity);
                      return;
                    }
                    Game1.getCharacterFromName(strArray[1], false).jump(jumpVelocity);
                    return;
                  }
                  if (!(s == "wall"))
                    return;
                  goto label_942;
                }
                else if (stringHash <= 2850206306U)
                {
                  if ((int) stringHash != -1469294289)
                  {
                    if ((int) stringHash != -1444760990 || !(s == "KillAllHorses"))
                      return;
                    using (List<GameLocation>.Enumerator enumerator = Game1.locations.GetEnumerator())
                    {
                      while (enumerator.MoveNext())
                      {
                        GameLocation current = enumerator.Current;
                        for (int index = current.characters.Count - 1; index >= 0; --index)
                        {
                          if (current.characters[index] is Horse)
                          {
                            current.characters.RemoveAt(index);
                            Game1.playSound("drumkit0");
                          }
                        }
                      }
                      return;
                    }
                  }
                  else
                  {
                    if (!(s == "lantern"))
                      return;
                    Game1.player.items.Add((Item) new Lantern());
                    return;
                  }
                }
                else
                {
                  if ((int) stringHash != -1440480390)
                  {
                    if ((int) stringHash != -1409888977 || !(s == "skinColor"))
                      return;
                    Game1.player.changeSkinColor(Convert.ToInt32(strArray[1]));
                    return;
                  }
                  if (!(s == "debrisWeather"))
                    return;
                  Game1.isDebrisWeather = !Game1.isDebrisWeather;
                  return;
                }
              }
              else
              {
                if (stringHash <= 2900856178U)
                {
                  if (stringHash <= 2896420465U)
                  {
                    if ((int) stringHash != -1406514591)
                    {
                      if ((int) stringHash != -1398546831 || !(s == "clearMail"))
                        return;
                      Game1.player.mailReceived.Clear();
                      return;
                    }
                    if (!(s == "cookingrecipe"))
                      return;
                    Game1.player.cookingRecipes.Add(debugInput.Substring(debugInput.IndexOf(' ')).Trim(), 0);
                    return;
                  }
                  if ((int) stringHash != -1395576011)
                  {
                    if ((int) stringHash != -1394111118 || !(s == "clearCharacters"))
                      return;
                    Game1.currentLocation.characters.Clear();
                    return;
                  }
                  if (!(s == "fishing"))
                    return;
                  Game1.player.FishingLevel = Convert.ToInt32(strArray[1]);
                  return;
                }
                if (stringHash <= 2923112787U)
                {
                  if ((int) stringHash != -1392488573)
                  {
                    if ((int) stringHash != -1371854509 || !(s == "dialogue"))
                      return;
                    Game1.getCharacterFromName(strArray[1], false).CurrentDialogue.Push(new Dialogue(debugInput.Substring(debugInput.IndexOf("0") + 1), Game1.getCharacterFromName(strArray[1], false)));
                    return;
                  }
                  if (!(s == "fbp"))
                    return;
                  goto label_734;
                }
                else
                {
                  if ((int) stringHash != -1367388900)
                  {
                    if ((int) stringHash != -1347581597)
                    {
                      if ((int) stringHash != -1345293851 || !(s == "test"))
                        return;
                      Game1.currentMinigame = (IMinigame) new Test();
                      return;
                    }
                    if (!(s == "fish"))
                      return;
                    Game1.activeClickableMenu = (IClickableMenu) new BobberBar(Convert.ToInt32(strArray[1]), 0.5f, true, (Game1.player.CurrentTool as FishingRod).attachments[1] != null ? (Game1.player.CurrentTool as FishingRod).attachments[1].ParentSheetIndex : -1);
                    return;
                  }
                  if (!(s == "year"))
                    return;
                  Game1.year = Convert.ToInt32(strArray[1]);
                  return;
                }
              }
            }
            else if (stringHash <= 3251403663U)
            {
              if (stringHash <= 3102149661U)
              {
                if (stringHash <= 3048072735U)
                {
                  if ((int) stringHash != -1310039480)
                  {
                    if ((int) stringHash != -1246894561 || !(s == "crafting"))
                      return;
                    using (Dictionary<string, string>.KeyCollection.Enumerator enumerator = CraftingRecipe.craftingRecipes.Keys.GetEnumerator())
                    {
                      while (enumerator.MoveNext())
                      {
                        string current = enumerator.Current;
                        if (!Game1.player.craftingRecipes.ContainsKey(current))
                          Game1.player.craftingRecipes.Add(current, 0);
                      }
                      return;
                    }
                  }
                  else
                  {
                    if (!(s == "fill"))
                      return;
                    goto label_734;
                  }
                }
                else if ((int) stringHash != -1237134848)
                {
                  if ((int) stringHash != -1192817635 || !(s == "floor"))
                    return;
                  goto label_943;
                }
                else
                {
                  if (!(s == "killAll"))
                    return;
                  string str = strArray[1];
                  using (List<GameLocation>.Enumerator enumerator = Game1.locations.GetEnumerator())
                  {
                    while (enumerator.MoveNext())
                    {
                      GameLocation current = enumerator.Current;
                      if (!current.Equals((object) Game1.currentLocation))
                      {
                        current.characters.Clear();
                      }
                      else
                      {
                        for (int index = current.characters.Count - 1; index >= 0; --index)
                        {
                          if (!current.characters[index].name.Equals(str))
                            current.characters.RemoveAt(index);
                        }
                      }
                    }
                    return;
                  }
                }
              }
              else
              {
                if (stringHash <= 3171639549U)
                {
                  if ((int) stringHash != -1191033768)
                  {
                    if ((int) stringHash != -1123327747 || !(s == "beachBridge"))
                      return;
                    (Game1.getLocationFromName("Beach") as Beach).bridgeFixed = !(Game1.getLocationFromName("Beach") as Beach).bridgeFixed;
                    if ((Game1.getLocationFromName("Beach") as Beach).bridgeFixed)
                      return;
                    (Game1.getLocationFromName("Beach") as Beach).setMapTile(58, 13, 284, "Buildings", (string) null, 1);
                    return;
                  }
                  if (!(s == "achievement"))
                    return;
                  Game1.getAchievement(Convert.ToInt32(strArray[1]));
                  return;
                }
                if ((int) stringHash != -1084398268)
                {
                  if ((int) stringHash != -1043563633 || !(s == "slimecraft"))
                    return;
                  Game1.player.craftingRecipes.Add("Slime Incubator", 0);
                  Game1.player.craftingRecipes.Add("Slime Egg-Press", 0);
                  Game1.playSound("crystal");
                  return;
                }
                if (!(s == "LSD"))
                  return;
                Game1.bloom.startShifting((float) Convert.ToDouble(strArray[1]), (float) Convert.ToDouble(strArray[2]), (float) Convert.ToDouble(strArray[3]) / 1000f, (float) Convert.ToDouble(strArray[4]) / 100f, (float) Convert.ToDouble(strArray[5]) / 100f, (float) Convert.ToDouble(strArray[6]) / 100f, (float) Convert.ToDouble(strArray[7]) / 100f, (float) Convert.ToDouble(strArray[8]) / 100f, (float) Convert.ToDouble(strArray[9]) / 100f, (float) Convert.ToDouble(strArray[10]) / 100f, (float) Convert.ToDouble(strArray[11]), true);
                return;
              }
            }
            else if (stringHash <= 3313734240U)
            {
              if (stringHash <= 3281777315U)
              {
                if ((int) stringHash != -1017083499)
                {
                  if ((int) stringHash != -1013189981 || !(s == "build"))
                    return;
                  Game1.getFarm().buildStructure(new BluePrint(strArray[1].Replace('9', ' ')), strArray.Length > 3 ? new Vector2((float) Convert.ToInt32(strArray[2]), (float) Convert.ToInt32(strArray[3])) : new Vector2((float) (Game1.player.getTileX() + 1), (float) Game1.player.getTileY()), false, Game1.player, false);
                  Game1.getFarm().buildings.Last<Building>().daysOfConstructionLeft = 0;
                  return;
                }
                if (!(s == "farmMap"))
                  return;
                for (int index = 0; index < Game1.locations.Count; ++index)
                {
                  if (Game1.locations[index] is Farm)
                    Game1.locations.RemoveAt(index);
                  if (Game1.locations[index] is FarmHouse)
                    Game1.locations.RemoveAt(index);
                }
                Game1.whichFarm = Convert.ToInt32(strArray[1]);
                Game1.locations.Add((GameLocation) new Farm(Game1.game1.xTileContent.Load<Map>("Maps\\" + Farm.getMapNameFromTypeInt(Game1.whichFarm)), "Farm"));
                Game1.locations.Add((GameLocation) new FarmHouse(Game1.game1.xTileContent.Load<Map>("Maps\\FarmHouse"), "FarmHouse"));
                return;
              }
              if ((int) stringHash != -999257139)
              {
                if ((int) stringHash != -981233056 || !(s == "blueprint"))
                  return;
                Game1.player.blueprints.Add(debugInput.Substring(debugInput.IndexOf(' ')).Trim());
                return;
              }
              if (!(s == "faceDirection"))
                return;
              goto label_1208;
            }
            else if (stringHash <= 3385614082U)
            {
              if ((int) stringHash != -937973262)
              {
                if ((int) stringHash != -909353214 || !(s == "playSound"))
                  return;
                goto label_1025;
              }
              else
              {
                if (!(s == "upgradeBarn"))
                  return;
                using (List<Building>.Enumerator enumerator = Game1.getFarm().buildings.GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    Building current = enumerator.Current;
                    if (current is Barn)
                      current.daysUntilUpgrade = 1;
                  }
                  return;
                }
              }
            }
            else if ((int) stringHash != -901437576)
            {
              if ((int) stringHash != -878665843)
              {
                if ((int) stringHash != -865957544 || !(s == "season"))
                  return;
                Game1.currentSeason = strArray[1];
                Game1.setGraphicsForSeason();
                return;
              }
              if (!(s == "friend"))
                return;
            }
            else
            {
              if (!(s == "energize"))
                return;
              Game1.player.Stamina = (float) Game1.player.MaxStamina;
              if (strArray.Length <= 1)
                return;
              Game1.player.Stamina = (float) Convert.ToInt32(strArray[1]);
              return;
            }
            NPC characterFromName = Game1.getCharacterFromName(strArray[1], false);
            if ((object) characterFromName != null && !Game1.player.friendships.ContainsKey(characterFromName.name))
              Game1.player.friendships.Add(characterFromName.name, new int[6]);
            Game1.player.friendships[strArray[1]][0] = Convert.ToInt32(strArray[2]);
            return;
          }
          if (stringHash <= 3840843484U)
          {
            if (stringHash <= 3655829174U)
            {
              if (stringHash <= 3552237772U)
              {
                if (stringHash <= 3482252359U)
                {
                  if (stringHash <= 3439296072U)
                  {
                    if ((int) stringHash != -865276856)
                    {
                      if ((int) stringHash != -855671224 || !(s == "save"))
                        return;
                      Game1.saveOnNewDay = !Game1.saveOnNewDay;
                      if (Game1.saveOnNewDay)
                      {
                        Game1.playSound("bigSelect");
                        return;
                      }
                      Game1.playSound("bigDeSelect");
                      return;
                    }
                    if (!(s == "gamePad"))
                      return;
                    Game1.options.gamepadControls = !Game1.options.gamepadControls;
                    Game1.options.mouseControls = !Game1.options.gamepadControls;
                    Game1.showGlobalMessage(Game1.options.gamepadControls ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3209") : Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3210"));
                    return;
                  }
                  if ((int) stringHash != -821524801)
                  {
                    if ((int) stringHash != -812714937 || !(s == "steaminfo"))
                      return;
                    goto label_823;
                  }
                  else
                  {
                    if (!(s == "customizeMenu"))
                      return;
                    goto label_1088;
                  }
                }
                else if (stringHash <= 3517781723U)
                {
                  if ((int) stringHash != -795499352)
                  {
                    if ((int) stringHash != -777185573 || !(s == "coopDweller"))
                      return;
                    Game1.player.coopDwellers.Add(new CoopDweller(strArray[1], strArray[2]));
                    return;
                  }
                  if (!(s == "addJunimo"))
                    return;
                  goto label_984;
                }
                else
                {
                  if ((int) stringHash != -759009398)
                  {
                    if ((int) stringHash != -742729524 || !(s == "characterInfo"))
                      return;
                    Game1.showGlobalMessage(Game1.currentLocation.characters.Count.ToString() + " characters on this map");
                    return;
                  }
                  if (!(s == "hairStyle"))
                    return;
                  Game1.player.changeHairStyle(Convert.ToInt32(strArray[1]));
                  return;
                }
              }
              else if (stringHash <= 3602710866U)
              {
                if (stringHash <= 3570692901U)
                {
                  if ((int) stringHash != -725547140)
                  {
                    if ((int) stringHash != -724274395 || !(s == "daysPlayed"))
                      return;
                  }
                  else
                  {
                    if (!(s == "spreadSeeds"))
                      return;
                    using (Dictionary<Vector2, TerrainFeature>.Enumerator enumerator = Game1.getFarm().terrainFeatures.GetEnumerator())
                    {
                      while (enumerator.MoveNext())
                      {
                        KeyValuePair<Vector2, TerrainFeature> current = enumerator.Current;
                        if (current.Value is HoeDirt)
                          (current.Value as HoeDirt).crop = new Crop(Convert.ToInt32(strArray[1]), (int) current.Key.X, (int) current.Key.Y);
                      }
                      return;
                    }
                  }
                }
                else if ((int) stringHash != -717587494)
                {
                  if ((int) stringHash != -692256430 || !(s == "buildcoop"))
                    return;
                  goto label_886;
                }
                else
                {
                  if (!(s == "removeBuildings"))
                    return;
                  Game1.getFarm().buildings.Clear();
                  return;
                }
              }
              else
              {
                if (stringHash <= 3609007298U)
                {
                  if ((int) stringHash != -692054739)
                  {
                    if ((int) stringHash != -685959998 || !(s == "addKent"))
                      return;
                    Game1.getLocationFromName("SamHouse").characters.Add(new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Kent")), new Vector2((float) (8 * Game1.tileSize), (float) (13 * Game1.tileSize)), "SamHouse", 3, "Kent", false, (Dictionary<int, int[]>) null, Game1.content.Load<Texture2D>("Portraits\\Kent")));
                    Game1.player.friendships.Add("Kent", new int[5]);
                    return;
                  }
                  if (!(s == "getstat"))
                    return;
                  Game1.debugOutput = Game1.stats.GetType().GetProperty(strArray[1]).GetValue((object) Game1.stats, (object[]) null).ToString();
                  return;
                }
                if ((int) stringHash != -682620571)
                {
                  if ((int) stringHash != -664627503)
                  {
                    if ((int) stringHash != -639138122 || !(s == "doesItemExist"))
                      return;
                    Game1.showGlobalMessage(Utility.doesItemWithThisIndexExistAnywhere(Convert.ToInt32(strArray[1]), strArray.Length > 2) ? "Yes" : "No");
                    return;
                  }
                  if (!(s == "die"))
                    return;
                  Game1.player.health = 0;
                  return;
                }
                if (!(s == "furniture"))
                  return;
                goto label_934;
              }
            }
            else if (stringHash <= 3780611722U)
            {
              if (stringHash <= 3710806992U)
              {
                if (stringHash <= 3679930478U)
                {
                  if ((int) stringHash != -615574574)
                  {
                    if ((int) stringHash != -615036818 || !(s == "clearSpecials"))
                      return;
                    Game1.player.hasRustyKey = false;
                    Game1.player.hasSkullKey = false;
                    Game1.player.hasSpecialCharm = false;
                    Game1.player.hasDarkTalisman = false;
                    Game1.player.hasMagicInk = false;
                    Game1.player.hasClubCard = false;
                    Game1.player.canUnderstandDwarves = false;
                    return;
                  }
                  if (!(s == "dap"))
                    return;
                }
                else
                {
                  if ((int) stringHash != -597210560)
                  {
                    if ((int) stringHash != -584160304 || !(s == "clearLightGlows"))
                      return;
                    Game1.currentLocation.lightGlows.Clear();
                    return;
                  }
                  if (!(s == "zoomLevel"))
                    return;
                  goto label_780;
                }
              }
              else
              {
                if (stringHash <= 3726850017U)
                {
                  if ((int) stringHash != -571614112)
                  {
                    if ((int) stringHash != -568117279 || !(s == "pants"))
                      return;
                    Game1.player.changePants(new Color(Convert.ToInt32(strArray[1]), Convert.ToInt32(strArray[2]), Convert.ToInt32(strArray[3])));
                    return;
                  }
                  if (!(s == "gamemode"))
                    return;
                  Game1.setGameMode(Convert.ToByte(strArray[1]));
                  return;
                }
                if ((int) stringHash != -514799281)
                {
                  if ((int) stringHash != -514355574 || !(s == "wateringcan"))
                    return;
                  goto label_1203;
                }
                else
                {
                  if (!(s == "money"))
                    return;
                  Game1.player.Money = Convert.ToInt32(strArray[1]);
                  return;
                }
              }
            }
            else if (stringHash <= 3820166566U)
            {
              if (stringHash <= 3786391242U)
              {
                if ((int) stringHash != -512226265)
                {
                  if ((int) stringHash != -508576054 || !(s == "mft"))
                    return;
                  goto label_946;
                }
                else
                {
                  if (!(s == "barnDweller"))
                    return;
                  Game1.player.barnDwellers.Add(new BarnDweller(strArray[1], strArray[2]));
                  return;
                }
              }
              else
              {
                if ((int) stringHash != -485742695)
                {
                  if ((int) stringHash != -474800730 || !(s == "marry"))
                    return;
                  Game1.player.changeFriendship(2500, Game1.getCharacterFromName(strArray[1], false));
                  Game1.player.spouse = strArray[1];
                  Game1.prepareSpouseForWedding();
                  return;
                }
                if (!(s == "f"))
                  return;
                goto label_943;
              }
            }
            else if (stringHash <= 3829274584U)
            {
              if ((int) stringHash != -465950553)
              {
                if ((int) stringHash != -465692712 || !(s == "coop"))
                  return;
                goto label_1141;
              }
              else
              {
                if (!(s == "noSave"))
                  return;
                goto label_834;
              }
            }
            else
            {
              if ((int) stringHash != -464576003)
              {
                if ((int) stringHash != -458507581)
                {
                  if ((int) stringHash != -454123812 || !(s == "bloom"))
                    return;
                  Game1.bloomDay = true;
                  Game1.bloom.Visible = true;
                  Game1.bloom.Settings.BloomThreshold = (float) (Convert.ToDouble(strArray[1]) / 10.0);
                  Game1.bloom.Settings.BlurAmount = (float) (Convert.ToDouble(strArray[2]) / 10.0);
                  Game1.bloom.Settings.BloomIntensity = (float) (Convert.ToDouble(strArray[3]) / 10.0);
                  Game1.bloom.Settings.BaseIntensity = (float) (Convert.ToDouble(strArray[4]) / 10.0);
                  Game1.bloom.Settings.BloomSaturation = (float) (Convert.ToDouble(strArray[5]) / 10.0);
                  Game1.bloom.Settings.BaseSaturation = (float) (Convert.ToDouble(strArray[6]) / 10.0);
                  Game1.bloom.Settings.brightWhiteOnly = strArray.Length > 7;
                  return;
                }
                if (!(s == "viewport"))
                  return;
                Game1.viewport.X = Convert.ToInt32(strArray[1]) * Game1.tileSize;
                Game1.viewport.Y = Convert.ToInt32(strArray[2]) * Game1.tileSize;
                return;
              }
              if (!(s == "day"))
                return;
              Game1.stats.DaysPlayed = (uint) ((Utility.getSeasonNumber(Game1.currentSeason) * 28 + Convert.ToInt32(strArray[1])) * Game1.year);
              Game1.dayOfMonth = Convert.ToInt32(strArray[1]);
              return;
            }
            Game1.showGlobalMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3332", (object) (int) Game1.stats.DaysPlayed));
            return;
          }
          if (stringHash <= 4022571063U)
          {
            if (stringHash <= 3893112696U)
            {
              if (stringHash <= 3865623817U)
              {
                if (stringHash <= 3852476509U)
                {
                  if ((int) stringHash != -453272442)
                  {
                    if ((int) stringHash != -442490787 || !(s == "child"))
                      return;
                    goto label_907;
                  }
                  else
                  {
                    if (!(s == "upgradeCoop"))
                      return;
                    using (List<Building>.Enumerator enumerator = Game1.getFarm().buildings.GetEnumerator())
                    {
                      while (enumerator.MoveNext())
                      {
                        Building current = enumerator.Current;
                        if (current is Coop)
                          current.daysUntilUpgrade = 1;
                      }
                      return;
                    }
                  }
                }
                else
                {
                  if ((int) stringHash != -435409838)
                  {
                    if ((int) stringHash != -429343479 || !(s == "dog"))
                      return;
                    Game1.currentLocation.characters.Add((NPC) new Dog(Convert.ToInt32(strArray[1]), Convert.ToInt32(strArray[2])));
                    return;
                  }
                  if (!(s == "c"))
                    return;
                  goto label_1166;
                }
              }
              else if (stringHash <= 3876335077U)
              {
                if ((int) stringHash != -425207780)
                {
                  if ((int) stringHash != -418632219 || !(s == "b"))
                    return;
                  goto label_1164;
                }
                else
                {
                  if (!(s == "removeLights"))
                    return;
                  Game1.currentLightSources.Clear();
                  return;
                }
              }
              else if ((int) stringHash != -406386072)
              {
                if ((int) stringHash != -401854600 || !(s == "m"))
                  return;
                goto label_1095;
              }
              else
              {
                if (!(s == "busDriveOff"))
                  return;
                (Game1.getLocationFromName("BusStop") as BusStop).busDriveOff();
                return;
              }
            }
            else if (stringHash <= 3966162835U)
            {
              if (stringHash <= 3952565359U)
              {
                if ((int) stringHash != -367849795)
                {
                  if ((int) stringHash != -342401937 || !(s == "panMode"))
                    return;
                  goto label_948;
                }
                else
                {
                  if (!(s == "child2"))
                    return;
                  if (Game1.player.getChildren().Count > 1)
                  {
                    ++Game1.player.getChildren()[1].age;
                    Game1.player.getChildren()[1].reloadSprite();
                    return;
                  }
                  (Game1.getLocationFromName("FarmHouse") as FarmHouse).characters.Add((NPC) new Child("Baby2", Game1.random.NextDouble() < 0.5, Game1.random.NextDouble() < 0.5, Game1.player));
                  return;
                }
              }
              else
              {
                if ((int) stringHash != -334744124)
                {
                  if ((int) stringHash != -328804461 || !(s == "gold"))
                    return;
                  Game1.player.Money += 1000000;
                  return;
                }
                if (!(s == "i"))
                  return;
                goto label_1098;
              }
            }
            else if (stringHash <= 3970545608U)
            {
              if ((int) stringHash != -328220253)
              {
                if ((int) stringHash != -324421688 || !(s == "setFrame"))
                  return;
                goto label_1009;
              }
              else
              {
                if (!(s == "fishCaught"))
                  return;
                goto label_1039;
              }
            }
            else
            {
              if ((int) stringHash != -284411267)
              {
                if ((int) stringHash != -274818850)
                {
                  if ((int) stringHash != -272396233 || !(s == "hoe"))
                    return;
                  Game1.player.addItemToInventoryBool((Item) new Hoe(), false);
                  Game1.playSound("coin");
                  return;
                }
                if (!(s == "playMusic"))
                  return;
                Game1.changeMusicTrack(strArray[1]);
                return;
              }
              if (!(s == "j"))
                return;
              goto label_984;
            }
          }
          else if (stringHash <= 4176079303U)
          {
            if (stringHash <= 4072609730U)
            {
              if (stringHash <= 4037672760U)
              {
                if ((int) stringHash != -260818587)
                {
                  if ((int) stringHash != -257294536 || !(s == "resetMines"))
                    return;
                  (Game1.getLocationFromName("UndergroundMine") as MineShaft).permanentMineChanges.Clear();
                  Game1.playSound("jingle1");
                  return;
                }
                if (!(s == "can"))
                  return;
              }
              else
              {
                if ((int) stringHash != -234078410)
                {
                  if ((int) stringHash != -222357566 || !(s == "hat"))
                    return;
                  Game1.player.changeHat(Convert.ToInt32(strArray[1]));
                  Game1.playSound("coin");
                  return;
                }
                if (!(s == "w"))
                  return;
                goto label_942;
              }
            }
            else
            {
              if (stringHash <= 4144776981U)
              {
                if ((int) stringHash != -169622948)
                {
                  if ((int) stringHash != -150190315 || !(s == "r"))
                    return;
                  Game1.currentLocation.cleanupBeforePlayerExit();
                  Game1.currentLocation.resetForPlayerEntry();
                  return;
                }
                if (!(s == "hairColor"))
                  return;
                Game1.player.changeHairColor(new Color(Convert.ToInt32(strArray[1]), Convert.ToInt32(strArray[2]), Convert.ToInt32(strArray[3])));
                return;
              }
              if ((int) stringHash != -145193589)
              {
                if ((int) stringHash != -145032341)
                {
                  if ((int) stringHash != -118887993 || !(s == "waterColor"))
                    return;
                  Game1.currentLocation.waterColor = new Color(Convert.ToInt32(strArray[1]), Convert.ToInt32(strArray[2]), Convert.ToInt32(strArray[3])) * 0.5f;
                  return;
                }
                if (!(s == "ring"))
                  return;
                Game1.player.addItemToInventoryBool((Item) new Ring(Convert.ToInt32(strArray[1])), false);
                Game1.playSound("coin");
                return;
              }
              if (!(s == "lookup"))
                return;
              goto label_974;
            }
          }
          else if (stringHash <= 4216511432U)
          {
            if (stringHash <= 4183284832U)
            {
              if ((int) stringHash != -114250746)
              {
                if ((int) stringHash != -111682464 || !(s == "clearFurniture"))
                  return;
                (Game1.currentLocation as FarmHouse).furniture.Clear();
                return;
              }
              if (!(s == "seenevent"))
                return;
              goto label_1136;
            }
            else
            {
              if ((int) stringHash != -96342536)
              {
                if ((int) stringHash != -78455864 || !(s == "removeDirt"))
                  return;
                for (int index = Game1.currentLocation.terrainFeatures.Count - 1; index >= 0; --index)
                {
                  KeyValuePair<Vector2, TerrainFeature> keyValuePair = Game1.currentLocation.terrainFeatures.ElementAt<KeyValuePair<Vector2, TerrainFeature>>(index);
                  if (keyValuePair.Value is HoeDirt)
                  {
                    SerializableDictionary<Vector2, TerrainFeature> terrainFeatures = Game1.currentLocation.terrainFeatures;
                    keyValuePair = Game1.currentLocation.terrainFeatures.ElementAt<KeyValuePair<Vector2, TerrainFeature>>(index);
                    Vector2 key = keyValuePair.Key;
                    terrainFeatures.Remove(key);
                  }
                }
                return;
              }
              if (!(s == "pick"))
                return;
              goto label_1204;
            }
          }
          else if (stringHash <= 4237905830U)
          {
            if ((int) stringHash != -62995158)
            {
              if ((int) stringHash != -57061466 || !(s == "endEvent"))
                return;
              goto label_1010;
            }
            else
            {
              if (!(s == "fillbp"))
                return;
              goto label_734;
            }
          }
          else
          {
            if ((int) stringHash != -30355297)
            {
              if ((int) stringHash != -21101361)
              {
                if ((int) stringHash != -362414 || !(s == "boots"))
                  return;
                Game1.player.addItemToInventoryBool((Item) new Boots(Convert.ToInt32(strArray[1])), false);
                Game1.playSound("coin");
                return;
              }
              if (!(s == "ccloadcutscene"))
                return;
              (Game1.getLocationFromName("CommunityCenter") as CommunityCenter).restoreAreaCutscene(Convert.ToInt32(strArray[1]));
              return;
            }
            if (!(s == "event"))
              return;
            if (strArray.Length <= 3)
              Game1.player.eventsSeen.Clear();
            string str = strArray[1];
            if (str == "Pool")
              str = "BathHouse_Pool";
            if (!Game1.content.Load<Dictionary<string, string>>("Data\\Events\\" + str).ElementAt<KeyValuePair<string, string>>(Convert.ToInt32(strArray[2])).Key.Contains<char>('/'))
              return;
            Game1.getLocationFromName(str).currentEvent = new Event(Game1.content.Load<Dictionary<string, string>>("Data\\Events\\" + str).ElementAt<KeyValuePair<string, string>>(Convert.ToInt32(strArray[2])).Value, -1);
            Game1.warpFarmer(str, 8, 8, false);
            return;
          }
label_1203:
          Game1.player.addItemToInventoryBool((Item) new WateringCan(), false);
          Game1.playSound("coin");
          return;
label_734:
          for (int index = 0; index < Game1.player.items.Count; ++index)
          {
            if (Game1.player.items[index] == null)
            {
              int num = -1;
              while (!Game1.objectInformation.ContainsKey(num))
                num = Game1.random.Next(1000);
              bool flag = false;
              if (num >= 516 && num <= 534)
                flag = true;
              Game1.player.items[index] = !flag ? (Item) new Object(num, 1, false, -1, 0) : (Item) new Ring(num);
            }
          }
          return;
label_907:
          if (Game1.player.getChildren().Count > 0)
          {
            ++Game1.player.getChildren()[0].age;
            Game1.player.getChildren()[0].reloadSprite();
            return;
          }
          (Game1.getLocationFromName("FarmHouse") as FarmHouse).characters.Add((NPC) new Child("Baby", Game1.random.NextDouble() < 0.5, Game1.random.NextDouble() < 0.5, Game1.player));
          return;
label_942:
          (Game1.getLocationFromName("FarmHouse") as FarmHouse).setWallpaper(strArray.Length > 1 ? Convert.ToInt32(strArray[1]) : (Game1.getLocationFromName("FarmHouse") as FarmHouse).wallPaper[0] + 1, -1, true);
          return;
label_943:
          (Game1.getLocationFromName("FarmHouse") as FarmHouse).setFloor(strArray.Length > 1 ? Convert.ToInt32(strArray[1]) : (Game1.getLocationFromName("FarmHouse") as FarmHouse).floor[0] + 1, -1, true);
          return;
label_946:
          Game1.addMailForTomorrow(strArray[1].Replace('0', '_'), strArray.Length > 2, false);
          return;
label_1098:
          if (!Game1.objectInformation.ContainsKey(Convert.ToInt32(strArray[1])))
            return;
          Game1.playSound("coin");
          Game1.player.addItemToInventoryBool((Item) new Object(Convert.ToInt32(strArray[1]), strArray.Length >= 3 ? Convert.ToInt32(strArray[2]) : 1, false, -1, strArray.Length >= 4 ? Convert.ToInt32(strArray[3]) : 0), false);
          return;
label_780:
          Game1.options.zoomLevel = (float) Convert.ToInt32(strArray[1]) / 100f;
          this.Window_ClientSizeChanged((object) null, (EventArgs) null);
          return;
label_823:
          Program.sdk.DebugInfo();
          return;
label_834:
          Game1.saveOnNewDay = !Game1.saveOnNewDay;
          if (!Game1.saveOnNewDay)
          {
            Game1.playSound("bigDeSelect");
            return;
          }
          Game1.playSound("bigSelect");
          return;
label_853:
          Game1.getFarm().shippingBin.Add((Item) new Object(24, 1, false, -1, 0));
          Game1.getFarm().shippingBin.Add((Item) new Object(82, 1, false, -1, 0));
          Game1.getFarm().shippingBin.Add((Item) new Object(136, 1, false, -1, 0));
          Game1.getFarm().shippingBin.Add((Item) new Object(16, 1, false, -1, 0));
          Game1.getFarm().shippingBin.Add((Item) new Object(388, 1, false, -1, 0));
          return;
label_886:
          Game1.getFarm().buildStructure(new BluePrint("Coop"), new Vector2((float) Convert.ToInt32(strArray[1]), (float) Convert.ToInt32(strArray[2])), false, Game1.player, false);
          Game1.getFarm().buildings.Last<Building>().daysOfConstructionLeft = 0;
          return;
label_897:
          Game1.ambientLight = new Color(Convert.ToInt32(strArray[1]), Convert.ToInt32(strArray[2]), Convert.ToInt32(strArray[3]));
          return;
label_934:
          if (strArray.Length < 2)
          {
            Furniture furniture = (Furniture) null;
            while (furniture == null)
            {
              try
              {
                furniture = new Furniture(Game1.random.Next(1613), Vector2.Zero);
              }
              catch (Exception ex)
              {
              }
            }
            Game1.player.addItemToInventoryBool((Item) furniture, false);
            return;
          }
          Game1.player.addItemToInventoryBool((Item) new Furniture(Convert.ToInt32(strArray[1]), Vector2.Zero), false);
          return;
label_948:
          Game1.panMode = true;
          Game1.viewportFreeze = true;
          Game1.debugMode = true;
          this.panFacingDirectionWait = false;
          this.panModeString = "";
          return;
label_974:
          using (Dictionary<int, string>.KeyCollection.Enumerator enumerator = Game1.objectInformation.Keys.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              int current = enumerator.Current;
              if (Game1.objectInformation[current].Substring(0, Game1.objectInformation[current].IndexOf('/')).ToLower().Equals(debugInput.Substring(debugInput.IndexOf(' ') + 1)))
                Game1.debugOutput = strArray[1] + " " + (object) current;
            }
            return;
          }
label_984:
          (Game1.getLocationFromName("CommunityCenter") as CommunityCenter).addCharacter((NPC) new Junimo(new Vector2((float) Convert.ToInt32(strArray[1]), (float) Convert.ToInt32(strArray[2])) * (float) Game1.tileSize, Convert.ToInt32(strArray[3]), false));
          return;
label_1009:
          Game1.player.FarmerSprite.pauseForSingleAnimation = true;
          Game1.player.FarmerSprite.setCurrentSingleAnimation(Convert.ToInt32(strArray[1]));
          return;
label_1010:
          Game1.pauseTime = 0.0f;
          Game1.player.eventsSeen.Clear();
          Game1.player.dialogueQuestionsAnswered.Clear();
          Game1.player.mailReceived.Clear();
          Game1.nonWarpFade = true;
          Game1.eventFinished();
          Game1.fadeScreenToBlack();
          Game1.viewportFreeze = false;
          return;
label_1025:
          Game1.playSound(strArray[1]);
          return;
label_1039:
          Game1.stats.FishCaught = (uint) Convert.ToInt32(strArray[1]);
          return;
label_1088:
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
          int num7 = 0;
          Game1.activeClickableMenu = (IClickableMenu) new CharacterCustomization(shirtOptions, hairStyleOptions, accessoryOptions, num7 != 0);
          return;
label_1095:
          Game1.musicPlayerVolume = (float) Convert.ToDouble(strArray[1]);
          Game1.options.musicVolumeLevel = (float) Convert.ToDouble(strArray[1]);
          Game1.musicCategory.SetVolume(Game1.options.musicVolumeLevel);
          return;
label_1100:
          using (Dictionary<int, string>.KeyCollection.Enumerator enumerator = Game1.objectInformation.Keys.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              int current = enumerator.Current;
              if (Game1.objectInformation[current].Substring(0, Game1.objectInformation[current].IndexOf('/')).ToLower().Replace(" ", "").Equals(strArray[1]))
              {
                Game1.player.addItemToInventory((Item) new Object(current, strArray.Length >= 3 ? Convert.ToInt32(strArray[2]) : 1, false, -1, strArray.Length >= 4 ? Convert.ToInt32(strArray[3]) : 0));
                Game1.playSound("coin");
              }
            }
            return;
          }
label_1136:
          Game1.player.eventsSeen.Add(Convert.ToInt32(strArray[1]));
          return;
label_1141:
          Game1.player.CoopUpgradeLevel = Math.Min(3, Game1.player.CoopUpgradeLevel + 1);
          Game1.removeFrontLayerForFarmBuildings();
          Game1.addNewFarmBuildingMaps();
          return;
label_1164:
          if (!Game1.bigCraftablesInformation.ContainsKey(Convert.ToInt32(strArray[1])))
            return;
          Game1.playSound("coin");
          Game1.player.addItemToInventory((Item) new Object(Vector2.Zero, Convert.ToInt32(strArray[1]), false));
          return;
label_1166:
          Game1.isEating = false;
          Game1.player.CanMove = true;
          Game1.player.usingTool = false;
          Game1.player.usingSlingshot = false;
          Game1.player.FarmerSprite.pauseForSingleAnimation = false;
          if (Game1.player.CurrentTool is FishingRod)
            (Game1.player.CurrentTool as FishingRod).isFishing = false;
          if (Game1.player.getMount() == null)
            return;
          Game1.player.getMount().dismount();
          return;
label_1184:
          if ((object) Game1.getCharacterFromName(strArray[1], false) == null)
            return;
          Game1.warpCharacter(Game1.getCharacterFromName(strArray[1], false), Game1.currentLocation.Name, new Vector2((float) Convert.ToInt32(strArray[2]), (float) Convert.ToInt32(strArray[3])), false, false);
          Game1.getCharacterFromName(strArray[1], false).faceDirection(Convert.ToInt32(strArray[4]));
          Game1.getCharacterFromName(strArray[1], false).controller = (PathFindController) null;
          Game1.getCharacterFromName(strArray[1], false).Halt();
          return;
label_1204:
          Game1.player.addItemToInventoryBool((Item) new Pickaxe(), false);
          Game1.playSound("coin");
          return;
label_1208:
          if (strArray[1].Equals("farmer"))
          {
            Game1.player.Halt();
            Game1.player.completelyStopAnimatingOrDoingAction();
            Game1.player.faceDirection(Convert.ToInt32(strArray[2]));
          }
          else
            Game1.getCharacterFromName(strArray[1], false).faceDirection(Convert.ToInt32(strArray[2]));
        }
      }
      catch (Exception ex)
      {
        Game1.debugOutput = Game1.parseText("Input parsing error... did you type your command correctly? - " + ex.Message);
      }
    }

    public void requestDebugInput()
    {
      Game1.activeClickableMenu = (IClickableMenu) new DebugInputMenu(new NamingMenu.doneNamingBehavior(this.parseDebugInput));
    }

    private void makeCelebrationWeatherDebris()
    {
      Game1.debrisWeather.Clear();
      Game1.isDebrisWeather = true;
      int num1 = Game1.random.Next(80, 100);
      int num2 = 22;
      for (int index = 0; index < num1; ++index)
        Game1.debrisWeather.Add(new WeatherDebris(new Vector2((float) Game1.random.Next(0, Game1.graphics.GraphicsDevice.Viewport.Width), (float) Game1.random.Next(0, Game1.graphics.GraphicsDevice.Viewport.Height)), num2 + Game1.random.Next(2), (float) Game1.random.Next(15) / 500f, (float) Game1.random.Next(-10, 0) / 50f, (float) Game1.random.Next(10) / 50f));
    }

    private void panModeSuccess(KeyboardState currentKBState)
    {
      this.panFacingDirectionWait = false;
      Game1.playSound("smallSelect");
      if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift))
        this.panModeString = this.panModeString + " (animation_name_here)";
      Thread thread = new Thread((ThreadStart) (() => Clipboard.SetText(this.panModeString)));
      int num = 0;
      thread.SetApartmentState((ApartmentState) num);
      thread.Start();
      thread.Join();
      Game1.debugOutput = this.panModeString;
    }

    private void updatePanModeControls(MouseState currentMouseState, KeyboardState currentKBState)
    {
      if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F8) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F8))
      {
        this.requestDebugInput();
      }
      else
      {
        if (!this.panFacingDirectionWait)
        {
          if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W))
            Game1.viewport.Y -= 16;
          if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
            Game1.viewport.X -= 16;
          if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S))
            Game1.viewport.Y += 16;
          if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
            Game1.viewport.X += 16;
        }
        else
        {
          if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W))
          {
            this.panModeString = this.panModeString + "0";
            this.panModeSuccess(currentKBState);
          }
          if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
          {
            this.panModeString = this.panModeString + "3";
            this.panModeSuccess(currentKBState);
          }
          if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S))
          {
            this.panModeString = this.panModeString + "2";
            this.panModeSuccess(currentKBState);
          }
          if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
          {
            this.panModeString = this.panModeString + "1";
            this.panModeSuccess(currentKBState);
          }
        }
        if (Game1.getMouseX() < Game1.tileSize * 3)
        {
          Game1.viewport.X -= 8;
          Game1.viewport.X -= (Game1.tileSize * 3 - Game1.getMouseX()) / 8;
        }
        if (Game1.getMouseX() > Game1.viewport.Width - Game1.tileSize * 3)
        {
          Game1.viewport.X += 8;
          Game1.viewport.X += (Game1.getMouseX() - Game1.viewport.Width + Game1.tileSize * 3) / 8;
        }
        if (Game1.getMouseY() < Game1.tileSize * 3)
        {
          Game1.viewport.Y -= 8;
          Game1.viewport.Y -= (Game1.tileSize * 3 - Game1.getMouseY()) / 8;
        }
        if (Game1.getMouseY() > Game1.viewport.Height - Game1.tileSize * 3)
        {
          Game1.viewport.Y += 8;
          Game1.viewport.Y += (Game1.getMouseY() - Game1.viewport.Height + Game1.tileSize * 3) / 8;
        }
        if (currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && Game1.oldMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && (this.panModeString != null && this.panModeString.Length > 0))
        {
          int num1 = (Game1.getMouseX() + Game1.viewport.X) / Game1.tileSize;
          int num2 = (Game1.getMouseY() + Game1.viewport.Y) / Game1.tileSize;
          this.panModeString = this.panModeString + Game1.currentLocation.Name + " " + (object) num1 + " " + (object) num2 + " ";
          this.panFacingDirectionWait = true;
          Game1.currentLocation.playTerrainSound(new Vector2((float) num1, (float) num2), (Character) null, true);
          Game1.debugOutput = this.panModeString;
        }
        if (currentMouseState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && Game1.oldMouseState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
        {
          int x = Game1.getMouseX() + Game1.viewport.X;
          int y = Game1.getMouseY() + Game1.viewport.Y;
          Warp warp = Game1.currentLocation.isCollidingWithWarpOrDoor(new Microsoft.Xna.Framework.Rectangle(x, y, 1, 1));
          if (warp != null)
          {
            Game1.currentLocation = Game1.getLocationFromName(warp.TargetName);
            Game1.currentLocation.map.LoadTileSheets(Game1.mapDisplayDevice);
            Game1.viewport.X = warp.TargetX * Game1.tileSize - Game1.viewport.Width / 2;
            Game1.viewport.Y = warp.TargetY * Game1.tileSize - Game1.viewport.Height / 2;
            Game1.playSound("dwop");
          }
        }
        if (currentKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
        {
          Warp warp = Game1.currentLocation.warps[0];
          Game1.currentLocation = Game1.getLocationFromName(warp.TargetName);
          Game1.currentLocation.map.LoadTileSheets(Game1.mapDisplayDevice);
          Game1.viewport.X = warp.TargetX * Game1.tileSize - Game1.viewport.Width / 2;
          Game1.viewport.Y = warp.TargetY * Game1.tileSize - Game1.viewport.Height / 2;
          Game1.playSound("dwop");
        }
        if (Game1.viewport.X < -Game1.tileSize)
          Game1.viewport.X = -Game1.tileSize;
        if (Game1.viewport.X + Game1.viewport.Width > Game1.currentLocation.Map.Layers[0].LayerWidth * Game1.tileSize + Game1.tileSize * 2)
          Game1.viewport.X = Game1.currentLocation.Map.Layers[0].LayerWidth * Game1.tileSize + Game1.tileSize * 2 - Game1.viewport.Width;
        if (Game1.viewport.Y < -Game1.tileSize)
          Game1.viewport.Y = -Game1.tileSize;
        if (Game1.viewport.Y + Game1.viewport.Height > Game1.currentLocation.Map.Layers[0].LayerHeight * Game1.tileSize + Game1.tileSize * 2)
          Game1.viewport.Y = Game1.currentLocation.Map.Layers[0].LayerHeight * Game1.tileSize + Game1.tileSize * 2 - Game1.viewport.Height;
        Game1.oldMouseState = currentMouseState;
        Game1.oldKBState = currentKBState;
      }
    }

    public static bool isLocationAccessible(string locationName)
    {
      if (!(locationName == "CommunityCenter"))
      {
        if (!(locationName == "JojaMart"))
        {
          if (locationName == "Railroad" && Game1.stats.DaysPlayed > 31U)
            return true;
        }
        else if (!Game1.player.eventsSeen.Contains(191393))
          return true;
      }
      else if (Game1.player.eventsSeen.Contains(191393))
        return true;
      return false;
    }

    public static bool isGamePadThumbstickInMotion(double threshold = 0.2)
    {
      bool flag = false;
      GamePadState state = GamePad.GetState(Game1.playerOneIndex);
      if ((double) state.ThumbSticks.Left.X < -threshold || state.IsButtonDown(Buttons.LeftThumbstickLeft))
        flag = true;
      if ((double) state.ThumbSticks.Left.X > threshold || state.IsButtonDown(Buttons.LeftThumbstickRight))
        flag = true;
      if ((double) state.ThumbSticks.Left.Y < -threshold || state.IsButtonDown(Buttons.LeftThumbstickUp))
        flag = true;
      if ((double) state.ThumbSticks.Left.Y > threshold || state.IsButtonDown(Buttons.LeftThumbstickDown))
        flag = true;
      if ((double) state.ThumbSticks.Right.X < -threshold)
        flag = true;
      if ((double) state.ThumbSticks.Right.X > threshold)
        flag = true;
      if ((double) state.ThumbSticks.Right.Y < -threshold)
        flag = true;
      if ((double) state.ThumbSticks.Right.Y > threshold)
        flag = true;
      if (flag)
        Game1.thumbstickMotionMargin = 50;
      return Game1.thumbstickMotionMargin > 0;
    }

    public static bool isAnyGamePadButtonBeingPressed()
    {
      return Utility.getPressedButtons(GamePad.GetState(Game1.playerOneIndex), Game1.oldPadState).Count > 0;
    }

    public static bool isAnyGamePadButtonBeingHeld()
    {
      return Utility.getHeldButtons(GamePad.GetState(Game1.playerOneIndex)).Count > 0;
    }

    private void UpdateControlInput(GameTime time)
    {
      if (Game1.paused)
        return;
      KeyboardState state1 = Keyboard.GetState();
      MouseState state2 = Mouse.GetState();
      GamePadState state3 = GamePad.GetState(Game1.playerOneIndex);
      if (state3.IsConnected && !Game1.options.gamepadControls)
      {
        Game1.options.gamepadControls = true;
        Game1.showGlobalMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2574"));
        if (Game1.activeClickableMenu != null && Game1.options.SnappyMenus)
        {
          Game1.activeClickableMenu.populateClickableComponentList();
          Game1.activeClickableMenu.snapToDefaultClickableComponent();
        }
      }
      else if (!state3.IsConnected && Game1.options.gamepadControls)
      {
        Game1.options.gamepadControls = false;
        Game1.showGlobalMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2575"));
        if (Game1.activeClickableMenu == null)
          Game1.activeClickableMenu = (IClickableMenu) new GameMenu();
      }
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      bool flag4 = false;
      bool flag5 = false;
      bool flag6 = false;
      bool flag7 = false;
      bool flag8 = false;
      bool flag9 = false;
      bool flag10 = false;
      bool flag11 = false;
      bool flag12 = false;
      bool flag13 = false;
      bool flag14 = false;
      bool flag15 = false;
      bool flag16 = false;
      bool flag17 = false;
      bool flag18 = false;
      bool flag19 = false;
      bool flag20 = false;
      if (Game1.isOneOfTheseKeysDown(state1, Game1.options.actionButton) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.actionButton) || state2.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && Game1.oldMouseState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
      {
        flag1 = true;
        Game1.rightClickPolling = 250;
      }
      if (Game1.isOneOfTheseKeysDown(state1, Game1.options.useToolButton) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.useToolButton) || state2.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && Game1.oldMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
        flag3 = true;
      if (Game1.areAllOfTheseKeysUp(state1, Game1.options.useToolButton) && Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.useToolButton) || state2.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && Game1.oldMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
        flag4 = true;
      if (Game1.isOneOfTheseKeysDown(state1, Game1.options.toolSwapButton) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.toolSwapButton) || state2.ScrollWheelValue != Game1.oldMouseState.ScrollWheelValue)
        flag2 = true;
      if (Game1.isOneOfTheseKeysDown(state1, Game1.options.cancelButton) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.cancelButton) || state2.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && Game1.oldMouseState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
        flag6 = true;
      if (Game1.isOneOfTheseKeysDown(state1, Game1.options.moveUpButton) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.moveUpButton))
        flag7 = true;
      if (Game1.isOneOfTheseKeysDown(state1, Game1.options.moveRightButton) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.moveRightButton))
        flag8 = true;
      if (Game1.isOneOfTheseKeysDown(state1, Game1.options.moveDownButton) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.moveDownButton))
        flag10 = true;
      if (Game1.isOneOfTheseKeysDown(state1, Game1.options.moveLeftButton) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.moveLeftButton))
        flag9 = true;
      if (Game1.areAllOfTheseKeysUp(state1, Game1.options.moveUpButton) && Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.moveUpButton))
        flag11 = true;
      if (Game1.areAllOfTheseKeysUp(state1, Game1.options.moveRightButton) && Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.moveRightButton))
        flag12 = true;
      if (Game1.areAllOfTheseKeysUp(state1, Game1.options.moveDownButton) && Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.moveDownButton))
        flag13 = true;
      if (Game1.areAllOfTheseKeysUp(state1, Game1.options.moveLeftButton) && Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.moveLeftButton))
        flag14 = true;
      if (Game1.isOneOfTheseKeysDown(state1, Game1.options.moveUpButton))
        flag15 = true;
      if (Game1.isOneOfTheseKeysDown(state1, Game1.options.moveRightButton))
        flag16 = true;
      if (Game1.isOneOfTheseKeysDown(state1, Game1.options.moveDownButton))
        flag17 = true;
      if (Game1.isOneOfTheseKeysDown(state1, Game1.options.moveLeftButton))
        flag18 = true;
      if (Game1.isOneOfTheseKeysDown(state1, Game1.options.useToolButton) || state2.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
        flag19 = true;
      if (Game1.isOneOfTheseKeysDown(state1, Game1.options.chatButton))
        flag20 = true;
      TimeSpan elapsedGameTime1;
      if (Game1.isOneOfTheseKeysDown(state1, Game1.options.actionButton) || state2.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
      {
        int rightClickPolling = Game1.rightClickPolling;
        elapsedGameTime1 = time.ElapsedGameTime;
        int milliseconds = elapsedGameTime1.Milliseconds;
        Game1.rightClickPolling = rightClickPolling - milliseconds;
        if (Game1.rightClickPolling <= 0)
        {
          Game1.rightClickPolling = 100;
          flag1 = true;
        }
      }
      if (Game1.options.gamepadControls)
      {
        GamePadThumbSticks thumbSticks;
        if (Game1.isGamePadThumbstickInMotion(0.2))
        {
          double x1 = (double) state2.X;
          thumbSticks = state3.ThumbSticks;
          double num1 = (double) thumbSticks.Right.X * (double) Game1.thumbstickToMouseModifier;
          int x2 = (int) (x1 + num1);
          double y1 = (double) state2.Y;
          thumbSticks = state3.ThumbSticks;
          double num2 = (double) thumbSticks.Right.Y * (double) Game1.thumbstickToMouseModifier;
          int y2 = (int) (y1 - num2);
          Mouse.SetPosition(x2, y2);
          Game1.lastCursorMotionWasMouse = false;
        }
        if (Game1.getMouseX() == Game1.getOldMouseX() && Game1.getMouseY() == Game1.getOldMouseY() || (Game1.getMouseX() == 0 || Game1.getMouseY() == 0))
        {
          thumbSticks = state3.ThumbSticks;
          if ((double) Math.Abs(thumbSticks.Right.X) <= 0.0)
          {
            thumbSticks = state3.ThumbSticks;
            if ((double) Math.Abs(thumbSticks.Right.Y) <= 0.0)
              goto label_60;
          }
        }
        if (Game1.timerUntilMouseFade <= 0 && this.IsActive)
        {
          Mouse.SetPosition(Game1.lastMousePositionBeforeFade.X, Game1.lastMousePositionBeforeFade.Y);
          Game1.lastCursorMotionWasMouse = false;
        }
        else if (!Game1.isGamePadThumbstickInMotion(0.0))
          Game1.lastCursorMotionWasMouse = true;
        Game1.timerUntilMouseFade = 4000;
label_60:
        if (state1.GetPressedKeys().Length != 0 || state2.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed || state2.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
          Game1.timerUntilMouseFade = 4000;
        if (state3.IsButtonDown(Buttons.A) && !Game1.oldPadState.IsButtonDown(Buttons.A))
        {
          flag1 = true;
          Game1.lastCursorMotionWasMouse = false;
          Game1.rightClickPolling = 250;
        }
        if (state3.IsButtonDown(Buttons.X) && !Game1.oldPadState.IsButtonDown(Buttons.X))
        {
          flag3 = true;
          Game1.lastCursorMotionWasMouse = false;
        }
        if (!state3.IsButtonDown(Buttons.X) && Game1.oldPadState.IsButtonDown(Buttons.X))
          flag4 = true;
        if (state3.IsButtonDown(Buttons.RightTrigger) && !Game1.oldPadState.IsButtonDown(Buttons.RightTrigger))
        {
          flag2 = true;
          Game1.triggerPolling = 300;
        }
        else if (state3.IsButtonDown(Buttons.LeftTrigger) && !Game1.oldPadState.IsButtonDown(Buttons.LeftTrigger))
        {
          flag2 = true;
          Game1.triggerPolling = 300;
        }
        if (state3.IsButtonDown(Buttons.X))
          flag19 = true;
        if (state3.IsButtonDown(Buttons.A))
        {
          int rightClickPolling = Game1.rightClickPolling;
          elapsedGameTime1 = time.ElapsedGameTime;
          int milliseconds = elapsedGameTime1.Milliseconds;
          Game1.rightClickPolling = rightClickPolling - milliseconds;
          if (Game1.rightClickPolling <= 0)
          {
            Game1.rightClickPolling = 100;
            flag1 = true;
          }
        }
        if (state3.IsButtonDown(Buttons.RightTrigger) || state3.IsButtonDown(Buttons.LeftTrigger))
        {
          int triggerPolling = Game1.triggerPolling;
          elapsedGameTime1 = time.ElapsedGameTime;
          int milliseconds = elapsedGameTime1.Milliseconds;
          Game1.triggerPolling = triggerPolling - milliseconds;
          if (Game1.triggerPolling <= 0)
          {
            Game1.triggerPolling = 100;
            flag2 = true;
          }
        }
        if (state3.IsButtonDown(Buttons.RightShoulder) && !Game1.oldPadState.IsButtonDown(Buttons.RightShoulder))
          Game1.player.shiftToolbar(true);
        if (state3.IsButtonDown(Buttons.LeftShoulder) && !Game1.oldPadState.IsButtonDown(Buttons.LeftShoulder))
          Game1.player.shiftToolbar(false);
        if (state3.IsButtonDown(Buttons.DPadUp) && !Game1.oldPadState.IsButtonDown(Buttons.DPadUp))
          flag7 = true;
        else if (!state3.IsButtonDown(Buttons.DPadUp) && Game1.oldPadState.IsButtonDown(Buttons.DPadUp))
          flag11 = true;
        if (state3.IsButtonDown(Buttons.DPadRight) && !Game1.oldPadState.IsButtonDown(Buttons.DPadRight))
          flag8 = true;
        else if (!state3.IsButtonDown(Buttons.DPadRight) && Game1.oldPadState.IsButtonDown(Buttons.DPadRight))
          flag12 = true;
        if (state3.IsButtonDown(Buttons.DPadDown) && !Game1.oldPadState.IsButtonDown(Buttons.DPadDown))
          flag10 = true;
        else if (!state3.IsButtonDown(Buttons.DPadDown) && Game1.oldPadState.IsButtonDown(Buttons.DPadDown))
          flag13 = true;
        if (state3.IsButtonDown(Buttons.DPadLeft) && !Game1.oldPadState.IsButtonDown(Buttons.DPadLeft))
          flag9 = true;
        else if (!state3.IsButtonDown(Buttons.DPadLeft) && Game1.oldPadState.IsButtonDown(Buttons.DPadLeft))
          flag14 = true;
        if (state3.IsButtonDown(Buttons.DPadUp))
          flag15 = true;
        if (state3.IsButtonDown(Buttons.DPadRight))
          flag16 = true;
        if (state3.IsButtonDown(Buttons.DPadDown))
          flag17 = true;
        if (state3.IsButtonDown(Buttons.DPadLeft))
          flag18 = true;
        thumbSticks = state3.ThumbSticks;
        if ((double) thumbSticks.Left.X < -0.2)
        {
          flag9 = true;
          flag18 = true;
        }
        else
        {
          thumbSticks = state3.ThumbSticks;
          if ((double) thumbSticks.Left.X > 0.2)
          {
            flag8 = true;
            flag16 = true;
          }
        }
        thumbSticks = state3.ThumbSticks;
        if ((double) thumbSticks.Left.Y < -0.2)
        {
          flag10 = true;
          flag17 = true;
        }
        else
        {
          thumbSticks = state3.ThumbSticks;
          if ((double) thumbSticks.Left.Y > 0.2)
          {
            flag7 = true;
            flag15 = true;
          }
        }
        thumbSticks = Game1.oldPadState.ThumbSticks;
        if ((double) thumbSticks.Left.X < -0.2 && !flag18)
          flag14 = true;
        thumbSticks = Game1.oldPadState.ThumbSticks;
        if ((double) thumbSticks.Left.X > 0.2 && !flag16)
          flag12 = true;
        thumbSticks = Game1.oldPadState.ThumbSticks;
        if ((double) thumbSticks.Left.Y < -0.2 && !flag17)
          flag13 = true;
        thumbSticks = Game1.oldPadState.ThumbSticks;
        if ((double) thumbSticks.Left.Y > 0.2 && !flag15)
          flag11 = true;
      }
      Game1.ResetFreeCursorDrag();
      if (flag19)
      {
        int mouseClickPolling = Game1.mouseClickPolling;
        elapsedGameTime1 = time.ElapsedGameTime;
        int milliseconds = elapsedGameTime1.Milliseconds;
        Game1.mouseClickPolling = mouseClickPolling + milliseconds;
      }
      else
        Game1.mouseClickPolling = 0;
      if (Game1.mouseClickPolling > 250 && (Game1.player.CurrentTool == null || !(Game1.player.CurrentTool is MeleeWeapon)) && (Game1.player.CurrentTool == null || Game1.player.CurrentTool.GetType() != typeof (FishingRod) || Game1.player.CurrentTool.upgradeLevel <= 0))
      {
        flag3 = true;
        Game1.mouseClickPolling = 100;
      }
      if (Game1.displayHUD)
      {
        foreach (IClickableMenu onScreenMenu in Game1.onScreenMenus)
        {
          if (Game1.wasMouseVisibleThisFrame && onScreenMenu.isWithinBounds(Game1.getMouseX(), Game1.getMouseY()))
            onScreenMenu.performHoverAction(Game1.getMouseX(), Game1.getMouseY());
        }
        if (flag20)
        {
          foreach (IClickableMenu onScreenMenu in Game1.onScreenMenus)
          {
            if (onScreenMenu is ChatBox)
            {
              ((ChatBox) onScreenMenu).chatBox.Selected = true;
              Game1.isChatting = true;
              if (!state1.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.OemQuestion))
                return;
              ((ChatBox) onScreenMenu).chatBox.Text = "/";
              return;
            }
          }
        }
        else if (Game1.isChatting && state1.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
        {
          Game1.ChatBox.chatBox.Selected = false;
          Game1.isChatting = false;
          Game1.oldKBState = state1;
          return;
        }
      }
      if (Game1.panMode)
        this.updatePanModeControls(state2, state1);
      else if (flag4 && Game1.player.CurrentTool != null && (Game1.CurrentEvent == null && (double) Game1.pauseTime <= 0.0) && Game1.player.CurrentTool.onRelease(Game1.currentLocation, Game1.getMouseX(), Game1.getMouseY(), Game1.player))
      {
        Game1.oldMouseState = state2;
        Game1.oldKBState = state1;
        Game1.oldPadState = state3;
        Game1.player.usingSlingshot = false;
        Game1.player.canReleaseTool = true;
        Game1.player.usingTool = false;
        Game1.player.CanMove = true;
      }
      else
      {
        if ((flag3 && !Game1.isAnyGamePadButtonBeingPressed() || flag1 && Game1.isAnyGamePadButtonBeingPressed()) && ((double) Game1.pauseTime <= 0.0 && Game1.displayHUD && Game1.wasMouseVisibleThisFrame))
        {
          foreach (IClickableMenu onScreenMenu in Game1.onScreenMenus)
          {
            if (!(onScreenMenu is ChatBox) && (!(onScreenMenu is LevelUpMenu) || (onScreenMenu as LevelUpMenu).informationUp) && onScreenMenu.isWithinBounds(Game1.getMouseX(), Game1.getMouseY()))
            {
              onScreenMenu.receiveLeftClick(Game1.getMouseX(), Game1.getMouseY(), true);
              Game1.oldMouseState = state2;
              if (!Game1.isAnyGamePadButtonBeingPressed())
                return;
            }
            onScreenMenu.clickAway();
          }
        }
        if (Game1.isChatting || Game1.player.freezePause > 0)
        {
          Game1.oldMouseState = state2;
          Game1.oldKBState = state1;
          Game1.oldPadState = state3;
        }
        else
        {
          if (Game1.eventUp && flag1 | flag3)
            Game1.currentLocation.currentEvent.receiveMouseClick(Game1.getMouseX(), Game1.getMouseY());
          if (flag1 || Game1.dialogueUp & flag3)
          {
            foreach (IClickableMenu onScreenMenu in Game1.onScreenMenus)
            {
              if (Game1.wasMouseVisibleThisFrame && Game1.displayHUD && onScreenMenu.isWithinBounds(Game1.getMouseX(), Game1.getMouseY()) && (!(onScreenMenu is LevelUpMenu) || (onScreenMenu as LevelUpMenu).informationUp))
              {
                onScreenMenu.receiveRightClick(Game1.getMouseX(), Game1.getMouseY(), true);
                Game1.oldMouseState = state2;
                if (!Game1.isAnyGamePadButtonBeingPressed())
                  return;
              }
            }
            if (!Game1.pressActionButton(state1, state2, state3))
              return;
          }
          else
          {
            if (flag3 && (!Game1.player.UsingTool || Game1.player.CurrentTool != null && Game1.player.CurrentTool is MeleeWeapon) && (!Game1.pickingTool && !Game1.dialogueUp && !Game1.menuUp) && (Game1.player.CanMove || Game1.player.CurrentTool != null && (Game1.player.CurrentTool.Name.Equals("Fishing Rod") || Game1.player.CurrentTool is MeleeWeapon)))
            {
              if (Game1.player.CurrentTool != null)
                Game1.player.CurrentTool.leftClick(Game1.player);
              if (!Game1.pressUseToolButton() && Game1.player.canReleaseTool && Game1.player.usingTool)
              {
                Tool currentTool = Game1.player.CurrentTool;
              }
              Game1.oldMouseState = state2;
              if (Game1.mouseClickPolling < 100)
                Game1.oldKBState = state1;
              Game1.oldPadState = state3;
              return;
            }
            if (flag4 && Game1.player.canReleaseTool && (Game1.player.usingTool && Game1.player.CurrentTool != null))
              Game1.releaseUseToolButton();
            else if (flag2 && !Game1.player.UsingTool && !Game1.dialogueUp && ((Game1.pickingTool || Game1.player.CanMove) && (!Game1.player.areAllItemsNull() && !Game1.eventUp)))
              Game1.pressSwitchToolButton();
            else if ((!flag2 || Game1.player.ActiveObject == null || (Game1.dialogueUp || Game1.eventUp)) && (flag5 && !Game1.pickingTool && (!Game1.eventUp && !Game1.player.UsingTool)) && Game1.player.CanMove)
              Game1.pressAddItemToInventoryButton();
          }
          if (flag6)
          {
            if (Game1.numberOfSelectedItems != -1)
            {
              Game1.numberOfSelectedItems = -1;
              Game1.dialogueUp = false;
              Game1.player.CanMove = true;
            }
            else if (Game1.nameSelectUp && NameSelect.cancel())
            {
              Game1.nameSelectUp = false;
              Game1.playSound("bigDeSelect");
            }
          }
          TimeSpan elapsedGameTime2;
          if (Game1.player.CurrentTool != null & flag19 && Game1.player.canReleaseTool && (!Game1.eventUp && !Game1.dialogueUp) && (!Game1.menuUp && (double) Game1.player.Stamina >= 1.0 && !(Game1.player.CurrentTool is FishingRod)))
          {
            if (Game1.player.toolHold <= 0 && Game1.player.CurrentTool.upgradeLevel > Game1.player.toolPower)
              Game1.player.toolHold = 600;
            else if (Game1.player.CurrentTool.upgradeLevel > Game1.player.toolPower)
            {
              Farmer player = Game1.player;
              int toolHold = player.toolHold;
              elapsedGameTime2 = time.ElapsedGameTime;
              int milliseconds = elapsedGameTime2.Milliseconds;
              int num = toolHold - milliseconds;
              player.toolHold = num;
              if (Game1.player.toolHold <= 0)
                Game1.player.toolPowerIncrease();
            }
          }
          if ((double) Game1.upPolling >= 650.0)
            Game1.upPolling -= 100f;
          else if ((double) Game1.downPolling >= 650.0)
            Game1.downPolling -= 100f;
          else if ((double) Game1.rightPolling >= 650.0)
            Game1.rightPolling -= 100f;
          else if ((double) Game1.leftPolling >= 650.0)
            Game1.leftPolling -= 100f;
          else if (!Game1.nameSelectUp && (double) Game1.pauseTime <= 0.0 && (!Game1.eventUp || Game1.CurrentEvent != null && Game1.CurrentEvent.playerControlSequence && !Game1.player.usingTool))
          {
            if (Game1.player.movementDirections.Count < 2)
            {
              int count = Game1.player.movementDirections.Count;
              if (flag15)
              {
                Game1.player.setMoving((byte) 1);
                if (Game1.IsClient)
                  Game1.client.sendMessage((byte) 0, new object[1]
                  {
                    (object) (byte) 1
                  });
              }
              if (flag16)
              {
                Game1.player.setMoving((byte) 2);
                if (Game1.IsClient)
                  Game1.client.sendMessage((byte) 0, new object[1]
                  {
                    (object) (byte) 2
                  });
              }
              if (flag17)
              {
                Game1.player.setMoving((byte) 4);
                if (Game1.IsClient)
                  Game1.client.sendMessage((byte) 0, new object[1]
                  {
                    (object) (byte) 4
                  });
              }
              if (flag18)
              {
                Game1.player.setMoving((byte) 8);
                if (Game1.IsClient)
                  Game1.client.sendMessage((byte) 0, new object[1]
                  {
                    (object) (byte) 8
                  });
              }
              if (count == 0 && Game1.player.movementDirections.Count > 0 && Game1.player.running)
                Game1.player.FarmerSprite.nextOffset = 1;
            }
            if (flag11 || Game1.player.movementDirections.Contains(0) && !flag15)
            {
              Game1.player.setMoving((byte) 33);
              if (Game1.IsClient)
                Game1.client.sendMessage((byte) 0, new object[1]
                {
                  (object) 33
                });
              else if (Game1.IsServer && Game1.player.movementDirections.Count == 0)
                Game1.player.setMoving((byte) 64);
            }
            if (flag12 || Game1.player.movementDirections.Contains(1) && !flag16)
            {
              Game1.player.setMoving((byte) 34);
              if (Game1.IsClient)
                Game1.client.sendMessage((byte) 0, new object[1]
                {
                  (object) 34
                });
              else if (Game1.IsServer && Game1.player.movementDirections.Count == 0)
                Game1.player.setMoving((byte) 64);
            }
            if (flag13 || Game1.player.movementDirections.Contains(2) && !flag17)
            {
              Game1.player.setMoving((byte) 36);
              if (Game1.IsClient)
                Game1.client.sendMessage((byte) 0, new object[1]
                {
                  (object) 36
                });
              else if (Game1.IsServer && Game1.player.movementDirections.Count == 0)
                Game1.player.setMoving((byte) 64);
            }
            if (flag14 || Game1.player.movementDirections.Contains(3) && !flag18)
            {
              Game1.player.setMoving((byte) 40);
              if (Game1.IsClient)
                Game1.client.sendMessage((byte) 0, new object[1]
                {
                  (object) 40
                });
              else if (Game1.IsServer && Game1.player.movementDirections.Count == 0)
                Game1.player.setMoving((byte) 64);
            }
            if (!flag15 && !flag16 && (!flag17 && !flag18) && !Game1.player.UsingTool)
              Game1.player.Halt();
          }
          else if (Game1.isQuestion)
          {
            if (flag7)
            {
              Game1.currentQuestionChoice = Math.Max(Game1.currentQuestionChoice - 1, 0);
              Game1.playSound("toolSwap");
            }
            else if (flag10)
            {
              Game1.currentQuestionChoice = Math.Min(Game1.currentQuestionChoice + 1, Game1.questionChoices.Count - 1);
              Game1.playSound("toolSwap");
            }
          }
          else if (Game1.numberOfSelectedItems != -1 && !Game1.dialogueTyping)
          {
            int val2 = 99;
            if (Game1.selectedItemsType.Equals("Animal Food"))
              val2 = 999 - Game1.player.Feed;
            else if (Game1.selectedItemsType.Equals("calicoJackBet"))
              val2 = Math.Min(Game1.player.clubCoins, 999);
            else if (Game1.selectedItemsType.Equals("flutePitch"))
              val2 = 26;
            else if (Game1.selectedItemsType.Equals("drumTone"))
              val2 = 6;
            else if (Game1.selectedItemsType.Equals("jukebox"))
              val2 = Game1.player.songsHeard.Count - 1;
            else if (Game1.selectedItemsType.Equals("Fuel"))
              val2 = 100 - ((Lantern) Game1.player.getToolFromName("Lantern")).fuelLeft;
            if (flag8)
            {
              Game1.numberOfSelectedItems = Math.Min(Game1.numberOfSelectedItems + 1, val2);
              Game1.playItemNumberSelectSound();
            }
            else if (flag9)
            {
              Game1.numberOfSelectedItems = Math.Max(Game1.numberOfSelectedItems - 1, 0);
              Game1.playItemNumberSelectSound();
            }
            else if (flag7)
            {
              Game1.numberOfSelectedItems = Math.Min(Game1.numberOfSelectedItems + 10, val2);
              Game1.playItemNumberSelectSound();
            }
            else if (flag10)
            {
              Game1.numberOfSelectedItems = Math.Max(Game1.numberOfSelectedItems - 10, 0);
              Game1.playItemNumberSelectSound();
            }
          }
          if (flag15 && !Game1.player.CanMove)
          {
            double upPolling = (double) Game1.upPolling;
            elapsedGameTime2 = time.ElapsedGameTime;
            double milliseconds = (double) elapsedGameTime2.Milliseconds;
            Game1.upPolling = (float) (upPolling + milliseconds);
          }
          else if (flag17 && !Game1.player.CanMove)
          {
            double downPolling = (double) Game1.downPolling;
            elapsedGameTime2 = time.ElapsedGameTime;
            double milliseconds = (double) elapsedGameTime2.Milliseconds;
            Game1.downPolling = (float) (downPolling + milliseconds);
          }
          else if (flag16 && !Game1.player.CanMove)
          {
            double rightPolling = (double) Game1.rightPolling;
            elapsedGameTime2 = time.ElapsedGameTime;
            double milliseconds = (double) elapsedGameTime2.Milliseconds;
            Game1.rightPolling = (float) (rightPolling + milliseconds);
          }
          else if (flag18 && !Game1.player.CanMove)
          {
            double leftPolling = (double) Game1.leftPolling;
            elapsedGameTime2 = time.ElapsedGameTime;
            double milliseconds = (double) elapsedGameTime2.Milliseconds;
            Game1.leftPolling = (float) (leftPolling + milliseconds);
          }
          else if (flag11)
            Game1.upPolling = 0.0f;
          else if (flag13)
            Game1.downPolling = 0.0f;
          else if (flag12)
            Game1.rightPolling = 0.0f;
          else if (flag14)
            Game1.leftPolling = 0.0f;
          if (Game1.debugMode)
          {
            if (state1.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Q))
              Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Q);
            if (state1.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.P) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.P))
              Game1.NewDay(0.0f);
            if (state1.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.M) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.M))
            {
              Game1.dayOfMonth = 28;
              Game1.NewDay(0.0f);
            }
            if (state1.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.T) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.T))
            {
              Game1.timeOfDay += 100;
              foreach (GameLocation location in Game1.locations)
              {
                for (int index = 0; index < location.getCharacters().Count; ++index)
                {
                  location.getCharacters()[index].checkSchedule(Game1.timeOfDay);
                  location.getCharacters()[index].checkSchedule(Game1.timeOfDay - 50);
                  location.getCharacters()[index].checkSchedule(Game1.timeOfDay - 60);
                  location.getCharacters()[index].checkSchedule(Game1.timeOfDay - 70);
                  location.getCharacters()[index].checkSchedule(Game1.timeOfDay - 80);
                  location.getCharacters()[index].checkSchedule(Game1.timeOfDay - 90);
                }
              }
              switch (Game1.timeOfDay)
              {
                case 2100:
                  Game1.globalOutdoorLighting = 0.9f;
                  break;
                case 2200:
                  Game1.globalOutdoorLighting = 1f;
                  break;
                case 1900:
                  Game1.globalOutdoorLighting = 0.5f;
                  Game1.currentLocation.switchOutNightTiles();
                  break;
                case 2000:
                  Game1.globalOutdoorLighting = 0.7f;
                  if (!Game1.isRaining)
                  {
                    Game1.changeMusicTrack("none");
                    break;
                  }
                  break;
              }
            }
            if (state1.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Y) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Y))
            {
              if (state1.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift))
                Game1.timeOfDay -= 10;
              else
                Game1.timeOfDay += 10;
              if (Game1.timeOfDay % 100 == 60)
                Game1.timeOfDay += 40;
              if (Game1.timeOfDay % 100 == 90)
                Game1.timeOfDay -= 40;
              Game1.currentLocation.performTenMinuteUpdate(Game1.timeOfDay);
              foreach (GameLocation location in Game1.locations)
              {
                for (int index = 0; index < location.getCharacters().Count; ++index)
                  location.getCharacters()[index].checkSchedule(Game1.timeOfDay);
              }
              if (Game1.isLightning)
                Utility.performLightningUpdate();
              switch (Game1.timeOfDay)
              {
                case 2000:
                  Game1.globalOutdoorLighting = 0.7f;
                  if (!Game1.isRaining)
                  {
                    Game1.changeMusicTrack("none");
                    break;
                  }
                  break;
                case 2100:
                  Game1.globalOutdoorLighting = 0.9f;
                  break;
                case 2200:
                  Game1.globalOutdoorLighting = 1f;
                  break;
                case 1750:
                  Game1.globalOutdoorLighting = 0.0f;
                  Game1.outdoorLight = Color.White;
                  break;
                case 1900:
                  Game1.globalOutdoorLighting = 0.5f;
                  Game1.currentLocation.switchOutNightTiles();
                  break;
              }
            }
            if (state1.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D1) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D1))
              Game1.warpFarmer("Mountain", 15, 35, false);
            if (state1.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D2) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D2))
              Game1.warpFarmer("Town", 35, 35, false);
            if (state1.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D3) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D3))
              Game1.warpFarmer("Farm", 64, 15, false);
            if (state1.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D4) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D4))
              Game1.warpFarmer("Forest", 34, 13, false);
            if (state1.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D5) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D4))
              Game1.warpFarmer("Beach", 34, 10, false);
            if (state1.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D6) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D6))
              Game1.warpFarmer("Mine", 18, 12, false);
            if (state1.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D7) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D7))
              Game1.warpFarmer("SandyHouse", 16, 3, false);
            if (state1.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.K) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.K))
            {
              if (Game1.mine == null)
                Game1.mine = new MineShaft();
              Game1.enterMine(false, Game1.mine.mineLevel + 1, (string) null);
            }
            if (state1.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.H) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.H))
              Game1.player.changeHat(Game1.random.Next(FarmerRenderer.hatsTexture.Height / 80 * 12));
            if (state1.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.I) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.I))
              Game1.player.changeHairStyle(Game1.random.Next(FarmerRenderer.hairStylesTexture.Height / 96 * 8));
            if (state1.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.J) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.J))
            {
              Game1.player.changeShirt(Game1.random.Next(40));
              Game1.player.changePants(new Color(Game1.random.Next((int) byte.MaxValue), Game1.random.Next((int) byte.MaxValue), Game1.random.Next((int) byte.MaxValue)));
            }
            if (state1.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.L) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.L))
            {
              Game1.player.changeShirt(Game1.random.Next(40));
              Game1.player.changePants(new Color(Game1.random.Next((int) byte.MaxValue), Game1.random.Next((int) byte.MaxValue), Game1.random.Next((int) byte.MaxValue)));
              Game1.player.changeHairStyle(Game1.random.Next(FarmerRenderer.hairStylesTexture.Height / 96 * 8));
              if (Game1.random.NextDouble() < 0.5)
                Game1.player.changeHat(Game1.random.Next(-1, FarmerRenderer.hatsTexture.Height / 80 * 12));
              else
                Game1.player.changeHat(-1);
              Game1.player.changeHairColor(new Color(Game1.random.Next((int) byte.MaxValue), Game1.random.Next((int) byte.MaxValue), Game1.random.Next((int) byte.MaxValue)));
              Game1.player.changeSkinColor(Game1.random.Next(16));
            }
            if (state1.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.U) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.U))
            {
              (Game1.getLocationFromName("FarmHouse") as FarmHouse).setWallpaper(Game1.random.Next(112), -1, true);
              (Game1.getLocationFromName("FarmHouse") as FarmHouse).setFloor(Game1.random.Next(40), -1, true);
            }
            if (state1.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F2))
              Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F2);
            if (state1.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F5) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F5))
              Game1.displayFarmer = !Game1.displayFarmer;
            if (state1.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F6))
              Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F6);
            if (state1.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F7) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F7))
              Game1.drawGrid = !Game1.drawGrid;
            if (state1.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.B) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.B))
              Game1.player.shiftToolbar(false);
            if (state1.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.N) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.N))
              Game1.player.shiftToolbar(true);
            if (state1.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F10) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F10))
            {
              if (Game1.server == null)
              {
                Game1.multiplayerMode = (byte) 2;
                Game1.server = (Server) new LidgrenServer("server");
                Game1.server.initializeConnection();
              }
              if (Game1.ChatBox == null)
                Game1.onScreenMenus.Add((IClickableMenu) new ChatBox());
            }
          }
          else if (!Game1.player.UsingTool)
          {
            if (Game1.isOneOfTheseKeysDown(state1, Game1.options.inventorySlot1) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.inventorySlot1))
              Game1.player.CurrentToolIndex = 0;
            else if (Game1.isOneOfTheseKeysDown(state1, Game1.options.inventorySlot2) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.inventorySlot2))
              Game1.player.CurrentToolIndex = 1;
            else if (Game1.isOneOfTheseKeysDown(state1, Game1.options.inventorySlot3) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.inventorySlot3))
              Game1.player.CurrentToolIndex = 2;
            else if (Game1.isOneOfTheseKeysDown(state1, Game1.options.inventorySlot4) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.inventorySlot4))
              Game1.player.CurrentToolIndex = 3;
            else if (Game1.isOneOfTheseKeysDown(state1, Game1.options.inventorySlot5) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.inventorySlot5))
              Game1.player.CurrentToolIndex = 4;
            else if (Game1.isOneOfTheseKeysDown(state1, Game1.options.inventorySlot6) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.inventorySlot6))
              Game1.player.CurrentToolIndex = 5;
            else if (Game1.isOneOfTheseKeysDown(state1, Game1.options.inventorySlot7) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.inventorySlot7))
              Game1.player.CurrentToolIndex = 6;
            else if (Game1.isOneOfTheseKeysDown(state1, Game1.options.inventorySlot8) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.inventorySlot8))
              Game1.player.CurrentToolIndex = 7;
            else if (Game1.isOneOfTheseKeysDown(state1, Game1.options.inventorySlot9) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.inventorySlot9))
              Game1.player.CurrentToolIndex = 8;
            else if (Game1.isOneOfTheseKeysDown(state1, Game1.options.inventorySlot10) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.inventorySlot10))
              Game1.player.CurrentToolIndex = 9;
            else if (Game1.isOneOfTheseKeysDown(state1, Game1.options.inventorySlot11) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.inventorySlot11))
              Game1.player.CurrentToolIndex = 10;
            else if (Game1.isOneOfTheseKeysDown(state1, Game1.options.inventorySlot12) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.inventorySlot12))
              Game1.player.CurrentToolIndex = 11;
          }
          if (!Program.releaseBuild)
          {
            if (Game1.IsPressEvent(ref state1, Microsoft.Xna.Framework.Input.Keys.F3) || Game1.IsPressEvent(ref state3, Buttons.LeftStick))
            {
              Game1.debugMode = !Game1.debugMode;
              if ((int) Game1.gameMode == 11)
                Game1.gameMode = (byte) 3;
            }
            if (Game1.IsPressEvent(ref state1, Microsoft.Xna.Framework.Input.Keys.F8) || Game1.IsPressEvent(ref state3, Buttons.RightStick))
              this.requestDebugInput();
          }
          if (state1.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F4) && !Game1.oldKBState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F4))
          {
            Game1.displayHUD = !Game1.displayHUD;
            Game1.playSound("smallSelect");
            if (!Game1.displayHUD)
              Game1.showGlobalMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3666"));
          }
          bool flag21 = Game1.isOneOfTheseKeysDown(state1, Game1.options.menuButton) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.menuButton);
          bool flag22 = Game1.isOneOfTheseKeysDown(state1, Game1.options.journalButton) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.journalButton);
          bool flag23 = Game1.isOneOfTheseKeysDown(state1, Game1.options.mapButton) && Game1.areAllOfTheseKeysUp(Game1.oldKBState, Game1.options.mapButton);
          if (Game1.options.gamepadControls && !flag21)
            flag21 = state3.IsButtonDown(Buttons.Start) && !Game1.oldPadState.IsButtonDown(Buttons.Start) || state3.IsButtonDown(Buttons.B) && !Game1.oldPadState.IsButtonDown(Buttons.B);
          if (Game1.options.gamepadControls && !flag22)
            flag22 = state3.IsButtonDown(Buttons.Back) && !Game1.oldPadState.IsButtonDown(Buttons.Back);
          if (Game1.options.gamepadControls && !flag23)
            flag23 = state3.IsButtonDown(Buttons.Y) && !Game1.oldPadState.IsButtonDown(Buttons.Y);
          if (((Game1.dayOfMonth <= 0 ? 0 : (Game1.player.CanMove ? 1 : 0)) & (flag21 ? 1 : 0)) != 0 && !Game1.dialogueUp && (!Game1.eventUp || Game1.isFestival() && Game1.CurrentEvent.festivalTimer <= 0) && Game1.currentMinigame == null)
          {
            if (Game1.activeClickableMenu == null)
              Game1.activeClickableMenu = (IClickableMenu) new GameMenu();
            else if (Game1.activeClickableMenu.readyToClose())
              Game1.exitActiveMenu();
          }
          if (((Game1.dayOfMonth <= 0 ? 0 : (Game1.player.CanMove ? 1 : 0)) & (flag22 ? 1 : 0)) != 0 && !Game1.dialogueUp && !Game1.eventUp)
          {
            if (Game1.activeClickableMenu == null)
              Game1.activeClickableMenu = (IClickableMenu) new QuestLog();
          }
          else if (((!Game1.eventUp ? 0 : (Game1.CurrentEvent != null ? 1 : 0)) & (flag22 ? 1 : 0)) != 0 && !Game1.CurrentEvent.skipped && Game1.CurrentEvent.skippable)
          {
            Game1.CurrentEvent.skipped = true;
            Game1.CurrentEvent.skipEvent();
            Game1.freezeControls = false;
          }
          if (((!Game1.options.gamepadControls || Game1.dayOfMonth <= 0 || !Game1.player.CanMove ? 0 : (Game1.isAnyGamePadButtonBeingPressed() ? 1 : 0)) & (flag23 ? 1 : 0)) != 0 && !Game1.dialogueUp && !Game1.eventUp)
          {
            if (Game1.activeClickableMenu == null)
              Game1.activeClickableMenu = (IClickableMenu) new GameMenu(4, -1);
          }
          else if (((Game1.dayOfMonth <= 0 ? 0 : (Game1.player.CanMove ? 1 : 0)) & (flag23 ? 1 : 0)) != 0 && !Game1.dialogueUp && (!Game1.eventUp && Game1.activeClickableMenu == null))
            Game1.activeClickableMenu = (IClickableMenu) new GameMenu(3, -1);
          Game1.checkForRunButton(state1, false);
          Game1.oldKBState = state1;
          Game1.oldMouseState = state2;
          Game1.oldPadState = state3;
        }
      }
    }

    public static void checkForRunButton(KeyboardState kbState, bool ignoreKeyPressQualifier = false)
    {
      bool running = Game1.player.running;
      bool flag1 = Game1.isOneOfTheseKeysDown(kbState, Game1.options.runButton) && !Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.runButton) | ignoreKeyPressQualifier;
      bool flag2 = !Game1.isOneOfTheseKeysDown(kbState, Game1.options.runButton) && Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.runButton) | ignoreKeyPressQualifier;
      if (Game1.options.gamepadControls)
      {
        if (!Game1.options.autoRun && (double) Math.Abs(Vector2.Distance(GamePad.GetState(Game1.playerOneIndex).ThumbSticks.Left, Vector2.Zero)) > 0.899999976158142)
          flag1 = true;
        else if ((double) Math.Abs(Vector2.Distance(Game1.oldPadState.ThumbSticks.Left, Vector2.Zero)) > 0.899999976158142 && (double) Math.Abs(Vector2.Distance(GamePad.GetState(Game1.playerOneIndex).ThumbSticks.Left, Vector2.Zero)) <= 0.899999976158142)
          flag2 = true;
      }
      if (flag1 && !Game1.player.canOnlyWalk)
      {
        Game1.player.setRunning(!Game1.options.autoRun, false);
        if (Game1.IsClient)
          Game1.client.sendMessage((byte) 0, new object[1]
          {
            (object) (Game1.player.running ? 16 : 48)
          });
        else if (Game1.IsServer)
          Game1.player.setMoving(Game1.player.running ? (byte) 16 : (byte) 48);
      }
      else if (flag2 && !Game1.player.canOnlyWalk)
      {
        Game1.player.setRunning(Game1.options.autoRun, false);
        if (Game1.IsClient)
          Game1.client.sendMessage((byte) 0, new object[1]
          {
            (object) (Game1.player.running ? 16 : 48)
          });
        else if (Game1.IsServer)
          Game1.player.setMoving(Game1.player.running ? (byte) 16 : (byte) 48);
      }
      if (Game1.player.running == running || Game1.player.usingTool)
        return;
      Game1.player.Halt();
    }

    public static void drawTitleScreenBackground(GameTime gameTime, string dayNight, int weatherDebrisOffsetDay)
    {
    }

    public static Vector2 getMostRecentViewportMotion()
    {
      return new Vector2((float) Game1.viewport.X - Game1.previousViewportPosition.X, (float) Game1.viewport.Y - Game1.previousViewportPosition.Y);
    }

    private RenderTarget2D screen
    {
      get
      {
        return this._screen;
      }
      set
      {
        if (this._screen != null)
        {
          this._screen.Dispose();
          this._screen = (RenderTarget2D) null;
        }
        this._screen = value;
      }
    }

    protected override void Draw(GameTime gameTime)
    {
      if (Game1.debugMode)
      {
        if (Game1._fpsStopwatch.IsRunning)
        {
          float totalSeconds = (float) Game1._fpsStopwatch.Elapsed.TotalSeconds;
          Game1._fpsList.Add(totalSeconds);
          while (Game1._fpsList.Count >= 120)
            Game1._fpsList.RemoveAt(0);
          float num = 0.0f;
          foreach (float fps in Game1._fpsList)
            num += fps;
          Game1._fps = (float) (1.0 / ((double) num / (double) Game1._fpsList.Count));
        }
        Game1._fpsStopwatch.Restart();
      }
      else
      {
        if (Game1._fpsStopwatch.IsRunning)
          Game1._fpsStopwatch.Reset();
        Game1._fps = 0.0f;
        Game1._fpsList.Clear();
      }
      if (Game1._newDayTask != null)
      {
        this.GraphicsDevice.Clear(this.bgColor);
        base.Draw(gameTime);
      }
      else
      {
        if ((double) Game1.options.zoomLevel != 1.0)
          this.GraphicsDevice.SetRenderTarget(this.screen);
        if (this.IsSaving)
        {
          this.GraphicsDevice.Clear(this.bgColor);
          IClickableMenu activeClickableMenu = Game1.activeClickableMenu;
          if (activeClickableMenu != null)
          {
            Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
            activeClickableMenu.draw(Game1.spriteBatch);
            Game1.spriteBatch.End();
          }
          base.Draw(gameTime);
          this.renderScreenBuffer();
        }
        else
        {
          this.GraphicsDevice.Clear(this.bgColor);
          if (Game1.activeClickableMenu != null && Game1.options.showMenuBackground && Game1.activeClickableMenu.showWithoutTransparencyIfOptionIsSet())
          {
            Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
            Game1.activeClickableMenu.drawBackground(Game1.spriteBatch);
            Game1.activeClickableMenu.draw(Game1.spriteBatch);
            Game1.spriteBatch.End();
            if ((double) Game1.options.zoomLevel != 1.0)
            {
              this.GraphicsDevice.SetRenderTarget((RenderTarget2D) null);
              this.GraphicsDevice.Clear(this.bgColor);
              Game1.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);
              Game1.spriteBatch.Draw((Texture2D) this.screen, Vector2.Zero, new Microsoft.Xna.Framework.Rectangle?(this.screen.Bounds), Color.White, 0.0f, Vector2.Zero, Game1.options.zoomLevel, SpriteEffects.None, 1f);
              Game1.spriteBatch.End();
            }
            if (Game1.overlayMenu == null)
              return;
            Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
            Game1.overlayMenu.draw(Game1.spriteBatch);
            Game1.spriteBatch.End();
          }
          else if ((int) Game1.gameMode == 11)
          {
            Game1.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
            Game1.spriteBatch.DrawString(Game1.dialogueFont, Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3685"), new Vector2(16f, 16f), Color.HotPink);
            Game1.spriteBatch.DrawString(Game1.dialogueFont, Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3686"), new Vector2(16f, 32f), new Color(0, (int) byte.MaxValue, 0));
            Game1.spriteBatch.DrawString(Game1.dialogueFont, Game1.parseText(Game1.errorMessage, Game1.dialogueFont, Game1.graphics.GraphicsDevice.Viewport.Width), new Vector2(16f, 48f), Color.White);
            Game1.spriteBatch.End();
          }
          else if (Game1.currentMinigame != null)
          {
            Game1.currentMinigame.draw(Game1.spriteBatch);
            if (Game1.globalFade && !Game1.menuUp && (!Game1.nameSelectUp || Game1.messagePause))
            {
              Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
              Game1.spriteBatch.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * ((int) Game1.gameMode == 0 ? 1f - Game1.fadeToBlackAlpha : Game1.fadeToBlackAlpha));
              Game1.spriteBatch.End();
            }
            if ((double) Game1.options.zoomLevel != 1.0)
            {
              this.GraphicsDevice.SetRenderTarget((RenderTarget2D) null);
              this.GraphicsDevice.Clear(this.bgColor);
              Game1.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);
              Game1.spriteBatch.Draw((Texture2D) this.screen, Vector2.Zero, new Microsoft.Xna.Framework.Rectangle?(this.screen.Bounds), Color.White, 0.0f, Vector2.Zero, Game1.options.zoomLevel, SpriteEffects.None, 1f);
              Game1.spriteBatch.End();
            }
            if (Game1.overlayMenu == null)
              return;
            Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
            Game1.overlayMenu.draw(Game1.spriteBatch);
            Game1.spriteBatch.End();
          }
          else if (Game1.showingEndOfNightStuff)
          {
            Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
            if (Game1.activeClickableMenu != null)
              Game1.activeClickableMenu.draw(Game1.spriteBatch);
            Game1.spriteBatch.End();
            if ((double) Game1.options.zoomLevel != 1.0)
            {
              this.GraphicsDevice.SetRenderTarget((RenderTarget2D) null);
              this.GraphicsDevice.Clear(this.bgColor);
              Game1.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);
              Game1.spriteBatch.Draw((Texture2D) this.screen, Vector2.Zero, new Microsoft.Xna.Framework.Rectangle?(this.screen.Bounds), Color.White, 0.0f, Vector2.Zero, Game1.options.zoomLevel, SpriteEffects.None, 1f);
              Game1.spriteBatch.End();
            }
            if (Game1.overlayMenu == null)
              return;
            Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
            Game1.overlayMenu.draw(Game1.spriteBatch);
            Game1.spriteBatch.End();
          }
          else if ((int) Game1.gameMode == 6)
          {
            Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
            string str1 = "";
            for (int index = 0; (double) index < gameTime.TotalGameTime.TotalMilliseconds % 999.0 / 333.0; ++index)
              str1 += ".";
            string str2 = Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3688");
            string str3 = str1;
            string s = str2 + str3;
            string str4 = "... ";
            string str5 = str2 + str4;
            int widthOfString = SpriteText.getWidthOfString(str5);
            int height = 64;
            int x = 64;
            int y = Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Bottom - height;
            SpriteText.drawString(Game1.spriteBatch, s, x, y, 999999, widthOfString, height, 1f, 0.88f, false, 0, str5, -1);
            Game1.spriteBatch.End();
            if ((double) Game1.options.zoomLevel != 1.0)
            {
              this.GraphicsDevice.SetRenderTarget((RenderTarget2D) null);
              this.GraphicsDevice.Clear(this.bgColor);
              Game1.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);
              Game1.spriteBatch.Draw((Texture2D) this.screen, Vector2.Zero, new Microsoft.Xna.Framework.Rectangle?(this.screen.Bounds), Color.White, 0.0f, Vector2.Zero, Game1.options.zoomLevel, SpriteEffects.None, 1f);
              Game1.spriteBatch.End();
            }
            if (Game1.overlayMenu == null)
              return;
            Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
            Game1.overlayMenu.draw(Game1.spriteBatch);
            Game1.spriteBatch.End();
          }
          else
          {
            Microsoft.Xna.Framework.Rectangle rectangle;
            if ((int) Game1.gameMode == 0)
            {
              Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
            }
            else
            {
              if (Game1.drawLighting)
              {
                this.GraphicsDevice.SetRenderTarget(Game1.lightmap);
                this.GraphicsDevice.Clear(Color.White * 0.0f);
                Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
                Game1.spriteBatch.Draw(Game1.staminaRect, Game1.lightmap.Bounds, Game1.currentLocation.name.Equals("UndergroundMine") ? Game1.mine.getLightingColor(gameTime) : (Game1.ambientLight.Equals(Color.White) || Game1.isRaining && Game1.currentLocation.isOutdoors ? Game1.outdoorLight : Game1.ambientLight));
                for (int index = 0; index < Game1.currentLightSources.Count; ++index)
                {
                  if (Utility.isOnScreen(Game1.currentLightSources.ElementAt<LightSource>(index).position, (int) ((double) Game1.currentLightSources.ElementAt<LightSource>(index).radius * (double) Game1.tileSize * 4.0)))
                    Game1.spriteBatch.Draw(Game1.currentLightSources.ElementAt<LightSource>(index).lightTexture, Game1.GlobalToLocal(Game1.viewport, Game1.currentLightSources.ElementAt<LightSource>(index).position) / (float) (Game1.options.lightingQuality / 2), new Microsoft.Xna.Framework.Rectangle?(Game1.currentLightSources.ElementAt<LightSource>(index).lightTexture.Bounds), Game1.currentLightSources.ElementAt<LightSource>(index).color, 0.0f, new Vector2((float) Game1.currentLightSources.ElementAt<LightSource>(index).lightTexture.Bounds.Center.X, (float) Game1.currentLightSources.ElementAt<LightSource>(index).lightTexture.Bounds.Center.Y), Game1.currentLightSources.ElementAt<LightSource>(index).radius / (float) (Game1.options.lightingQuality / 2), SpriteEffects.None, 0.9f);
                }
                Game1.spriteBatch.End();
                this.GraphicsDevice.SetRenderTarget((double) Game1.options.zoomLevel == 1.0 ? (RenderTarget2D) null : this.screen);
              }
              if (Game1.bloomDay && Game1.bloom != null)
                Game1.bloom.BeginDraw();
              this.GraphicsDevice.Clear(this.bgColor);
              Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
              if (Game1.background != null)
                Game1.background.draw(Game1.spriteBatch);
              Game1.mapDisplayDevice.BeginScene(Game1.spriteBatch);
              Game1.currentLocation.Map.GetLayer("Back").Draw(Game1.mapDisplayDevice, Game1.viewport, Location.Origin, false, Game1.pixelZoom);
              Game1.currentLocation.drawWater(Game1.spriteBatch);
              if (Game1.CurrentEvent == null)
              {
                foreach (NPC character in Game1.currentLocation.characters)
                {
                  if (!character.swimming && !character.hideShadow && (!character.isInvisible && !Game1.currentLocation.shouldShadowBeDrawnAboveBuildingsLayer(character.getTileLocation())))
                    Game1.spriteBatch.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, character.position + new Vector2((float) (character.sprite.spriteWidth * Game1.pixelZoom) / 2f, (float) (character.GetBoundingBox().Height + (character.IsMonster ? 0 : Game1.pixelZoom * 3)))), new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0.0f, new Vector2((float) Game1.shadowTexture.Bounds.Center.X, (float) Game1.shadowTexture.Bounds.Center.Y), ((float) Game1.pixelZoom + (float) character.yJumpOffset / 40f) * character.scale, SpriteEffects.None, Math.Max(0.0f, (float) character.getStandingY() / 10000f) - 1E-06f);
                }
              }
              else
              {
                foreach (NPC actor in Game1.CurrentEvent.actors)
                {
                  if (!actor.swimming && !actor.hideShadow && !Game1.currentLocation.shouldShadowBeDrawnAboveBuildingsLayer(actor.getTileLocation()))
                    Game1.spriteBatch.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, actor.position + new Vector2((float) (actor.sprite.spriteWidth * Game1.pixelZoom) / 2f, (float) (actor.GetBoundingBox().Height + (actor.IsMonster ? 0 : (actor.sprite.spriteHeight <= 16 ? -Game1.pixelZoom : Game1.pixelZoom * 3))))), new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0.0f, new Vector2((float) Game1.shadowTexture.Bounds.Center.X, (float) Game1.shadowTexture.Bounds.Center.Y), ((float) Game1.pixelZoom + (float) actor.yJumpOffset / 40f) * actor.scale, SpriteEffects.None, Math.Max(0.0f, (float) actor.getStandingY() / 10000f) - 1E-06f);
                }
              }
              Microsoft.Xna.Framework.Rectangle bounds;
              if (Game1.displayFarmer && !Game1.player.swimming && (!Game1.player.isRidingHorse() && !Game1.currentLocation.shouldShadowBeDrawnAboveBuildingsLayer(Game1.player.getTileLocation())))
              {
                SpriteBatch spriteBatch = Game1.spriteBatch;
                Texture2D shadowTexture = Game1.shadowTexture;
                Vector2 local = Game1.GlobalToLocal(Game1.player.position + new Vector2(32f, 24f));
                Microsoft.Xna.Framework.Rectangle? sourceRectangle = new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds);
                Color white = Color.White;
                double num1 = 0.0;
                double x = (double) Game1.shadowTexture.Bounds.Center.X;
                bounds = Game1.shadowTexture.Bounds;
                double y = (double) bounds.Center.Y;
                Vector2 origin = new Vector2((float) x, (float) y);
                double num2 = 4.0 - (!Game1.player.running && !Game1.player.usingTool || Game1.player.FarmerSprite.indexInCurrentAnimation <= 1 ? 0.0 : (double) Math.Abs(FarmerRenderer.featureYOffsetPerFrame[Game1.player.FarmerSprite.CurrentFrame]) * 0.5);
                int num3 = 0;
                double num4 = 0.0;
                spriteBatch.Draw(shadowTexture, local, sourceRectangle, white, (float) num1, origin, (float) num2, (SpriteEffects) num3, (float) num4);
              }
              Game1.currentLocation.Map.GetLayer("Buildings").Draw(Game1.mapDisplayDevice, Game1.viewport, Location.Origin, false, Game1.pixelZoom);
              Game1.mapDisplayDevice.EndScene();
              Game1.spriteBatch.End();
              Game1.spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
              if (Game1.CurrentEvent == null)
              {
                foreach (NPC character in Game1.currentLocation.characters)
                {
                  if (!character.swimming && !character.hideShadow && Game1.currentLocation.shouldShadowBeDrawnAboveBuildingsLayer(character.getTileLocation()))
                  {
                    SpriteBatch spriteBatch = Game1.spriteBatch;
                    Texture2D shadowTexture = Game1.shadowTexture;
                    Vector2 local = Game1.GlobalToLocal(Game1.viewport, character.position + new Vector2((float) (character.sprite.spriteWidth * Game1.pixelZoom) / 2f, (float) (character.GetBoundingBox().Height + (character.IsMonster ? 0 : Game1.pixelZoom * 3))));
                    Microsoft.Xna.Framework.Rectangle? sourceRectangle = new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds);
                    Color white = Color.White;
                    double num1 = 0.0;
                    bounds = Game1.shadowTexture.Bounds;
                    double x = (double) bounds.Center.X;
                    bounds = Game1.shadowTexture.Bounds;
                    double y = (double) bounds.Center.Y;
                    Vector2 origin = new Vector2((float) x, (float) y);
                    double num2 = ((double) Game1.pixelZoom + (double) character.yJumpOffset / 40.0) * (double) character.scale;
                    int num3 = 0;
                    double num4 = (double) Math.Max(0.0f, (float) character.getStandingY() / 10000f) - 9.99999997475243E-07;
                    spriteBatch.Draw(shadowTexture, local, sourceRectangle, white, (float) num1, origin, (float) num2, (SpriteEffects) num3, (float) num4);
                  }
                }
              }
              else
              {
                foreach (NPC actor in Game1.CurrentEvent.actors)
                {
                  if (!actor.swimming && !actor.hideShadow && Game1.currentLocation.shouldShadowBeDrawnAboveBuildingsLayer(actor.getTileLocation()))
                  {
                    SpriteBatch spriteBatch = Game1.spriteBatch;
                    Texture2D shadowTexture = Game1.shadowTexture;
                    Vector2 local = Game1.GlobalToLocal(Game1.viewport, actor.position + new Vector2((float) (actor.sprite.spriteWidth * Game1.pixelZoom) / 2f, (float) (actor.GetBoundingBox().Height + (actor.IsMonster ? 0 : Game1.pixelZoom * 3))));
                    Microsoft.Xna.Framework.Rectangle? sourceRectangle = new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds);
                    Color white = Color.White;
                    double num1 = 0.0;
                    bounds = Game1.shadowTexture.Bounds;
                    double x = (double) bounds.Center.X;
                    bounds = Game1.shadowTexture.Bounds;
                    double y = (double) bounds.Center.Y;
                    Vector2 origin = new Vector2((float) x, (float) y);
                    double num2 = ((double) Game1.pixelZoom + (double) actor.yJumpOffset / 40.0) * (double) actor.scale;
                    int num3 = 0;
                    double num4 = (double) Math.Max(0.0f, (float) actor.getStandingY() / 10000f) - 9.99999997475243E-07;
                    spriteBatch.Draw(shadowTexture, local, sourceRectangle, white, (float) num1, origin, (float) num2, (SpriteEffects) num3, (float) num4);
                  }
                }
              }
              if (Game1.displayFarmer && !Game1.player.swimming && (!Game1.player.isRidingHorse() && Game1.currentLocation.shouldShadowBeDrawnAboveBuildingsLayer(Game1.player.getTileLocation())))
              {
                SpriteBatch spriteBatch = Game1.spriteBatch;
                Texture2D shadowTexture = Game1.shadowTexture;
                Vector2 local = Game1.GlobalToLocal(Game1.player.position + new Vector2(32f, 24f));
                Microsoft.Xna.Framework.Rectangle? sourceRectangle = new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds);
                Color white = Color.White;
                double num1 = 0.0;
                double x = (double) Game1.shadowTexture.Bounds.Center.X;
                rectangle = Game1.shadowTexture.Bounds;
                double y = (double) rectangle.Center.Y;
                Vector2 origin = new Vector2((float) x, (float) y);
                double num2 = 4.0 - (!Game1.player.running && !Game1.player.usingTool || Game1.player.FarmerSprite.indexInCurrentAnimation <= 1 ? 0.0 : (double) Math.Abs(FarmerRenderer.featureYOffsetPerFrame[Game1.player.FarmerSprite.CurrentFrame]) * 0.5);
                int num3 = 0;
                double num4 = (double) Math.Max(0.0001f, (float) ((double) Game1.player.getStandingY() / 10000.0 + 0.000110000000859145)) - 9.99999974737875E-05;
                spriteBatch.Draw(shadowTexture, local, sourceRectangle, white, (float) num1, origin, (float) num2, (SpriteEffects) num3, (float) num4);
              }
              if (Game1.displayFarmer)
                Game1.player.draw(Game1.spriteBatch);
              if ((Game1.eventUp || Game1.killScreen) && (!Game1.killScreen && Game1.currentLocation.currentEvent != null))
                Game1.currentLocation.currentEvent.draw(Game1.spriteBatch);
              if (Game1.player.currentUpgrade != null && Game1.player.currentUpgrade.daysLeftTillUpgradeDone <= 3 && Game1.currentLocation.Name.Equals("Farm"))
                Game1.spriteBatch.Draw(Game1.player.currentUpgrade.workerTexture, Game1.GlobalToLocal(Game1.viewport, Game1.player.currentUpgrade.positionOfCarpenter), new Microsoft.Xna.Framework.Rectangle?(Game1.player.currentUpgrade.getSourceRectangle()), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, (float) (((double) Game1.player.currentUpgrade.positionOfCarpenter.Y + (double) (Game1.tileSize * 3 / 4)) / 10000.0));
              Game1.currentLocation.draw(Game1.spriteBatch);
              if (Game1.eventUp && Game1.currentLocation.currentEvent != null)
              {
                string messageToScreen = Game1.currentLocation.currentEvent.messageToScreen;
              }
              if (Game1.player.ActiveObject == null && (Game1.player.UsingTool || Game1.pickingTool) && (Game1.player.CurrentTool != null && (!Game1.player.CurrentTool.Name.Equals("Seeds") || Game1.pickingTool)))
                Game1.drawTool(Game1.player);
              if (Game1.currentLocation.Name.Equals("Farm"))
                this.drawFarmBuildings();
              if (Game1.tvStation >= 0)
                Game1.spriteBatch.Draw(Game1.tvStationTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (6 * Game1.tileSize + Game1.tileSize / 4), (float) (2 * Game1.tileSize + Game1.tileSize / 2))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(Game1.tvStation * 24, 0, 24, 15)), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 1E-08f);
              if (Game1.panMode)
              {
                Game1.spriteBatch.Draw(Game1.fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle((int) Math.Floor((double) (Game1.getOldMouseX() + Game1.viewport.X) / (double) Game1.tileSize) * Game1.tileSize - Game1.viewport.X, (int) Math.Floor((double) (Game1.getOldMouseY() + Game1.viewport.Y) / (double) Game1.tileSize) * Game1.tileSize - Game1.viewport.Y, Game1.tileSize, Game1.tileSize), Color.Lime * 0.75f);
                foreach (Warp warp in Game1.currentLocation.warps)
                  Game1.spriteBatch.Draw(Game1.fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle(warp.X * Game1.tileSize - Game1.viewport.X, warp.Y * Game1.tileSize - Game1.viewport.Y, Game1.tileSize, Game1.tileSize), Color.Red * 0.75f);
              }
              Game1.mapDisplayDevice.BeginScene(Game1.spriteBatch);
              Game1.currentLocation.Map.GetLayer("Front").Draw(Game1.mapDisplayDevice, Game1.viewport, Location.Origin, false, Game1.pixelZoom);
              Game1.mapDisplayDevice.EndScene();
              Game1.currentLocation.drawAboveFrontLayer(Game1.spriteBatch);
              Game1.spriteBatch.End();
              Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
              if (Game1.currentLocation.Name.Equals("Farm") && Game1.stats.SeedsSown >= 200U)
              {
                Game1.spriteBatch.Draw(Game1.debrisSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (3 * Game1.tileSize + Game1.tileSize / 4), (float) (Game1.tileSize + Game1.tileSize / 3))), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 16, -1, -1)), Color.White);
                Game1.spriteBatch.Draw(Game1.debrisSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (4 * Game1.tileSize + Game1.tileSize), (float) (2 * Game1.tileSize + Game1.tileSize))), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 16, -1, -1)), Color.White);
                Game1.spriteBatch.Draw(Game1.debrisSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (5 * Game1.tileSize), (float) (2 * Game1.tileSize))), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 16, -1, -1)), Color.White);
                Game1.spriteBatch.Draw(Game1.debrisSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (3 * Game1.tileSize + Game1.tileSize / 2), (float) (3 * Game1.tileSize))), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 16, -1, -1)), Color.White);
                Game1.spriteBatch.Draw(Game1.debrisSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (5 * Game1.tileSize - Game1.tileSize / 4), (float) Game1.tileSize)), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 16, -1, -1)), Color.White);
                Game1.spriteBatch.Draw(Game1.debrisSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (4 * Game1.tileSize), (float) (3 * Game1.tileSize + Game1.tileSize / 6))), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 16, -1, -1)), Color.White);
                Game1.spriteBatch.Draw(Game1.debrisSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (4 * Game1.tileSize + Game1.tileSize / 5), (float) (2 * Game1.tileSize + Game1.tileSize / 3))), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 16, -1, -1)), Color.White);
              }
              if (Game1.displayFarmer && Game1.player.ActiveObject != null && (Game1.player.ActiveObject.bigCraftable && this.checkBigCraftableBoundariesForFrontLayer()) && Game1.currentLocation.Map.GetLayer("Front").PickTile(new Location(Game1.player.getStandingX(), Game1.player.getStandingY()), Game1.viewport.Size) == null)
                Game1.drawPlayerHeldObject(Game1.player);
              else if (Game1.displayFarmer && Game1.player.ActiveObject != null)
              {
                if (Game1.currentLocation.Map.GetLayer("Front").PickTile(new Location((int) Game1.player.position.X, (int) Game1.player.position.Y - Game1.tileSize * 3 / 5), Game1.viewport.Size) == null || Game1.currentLocation.Map.GetLayer("Front").PickTile(new Location((int) Game1.player.position.X, (int) Game1.player.position.Y - Game1.tileSize * 3 / 5), Game1.viewport.Size).TileIndexProperties.ContainsKey("FrontAlways"))
                {
                  Layer layer1 = Game1.currentLocation.Map.GetLayer("Front");
                  rectangle = Game1.player.GetBoundingBox();
                  Location mapDisplayLocation1 = new Location(rectangle.Right, (int) Game1.player.position.Y - Game1.tileSize * 3 / 5);
                  Size size1 = Game1.viewport.Size;
                  if (layer1.PickTile(mapDisplayLocation1, size1) != null)
                  {
                    Layer layer2 = Game1.currentLocation.Map.GetLayer("Front");
                    rectangle = Game1.player.GetBoundingBox();
                    Location mapDisplayLocation2 = new Location(rectangle.Right, (int) Game1.player.position.Y - Game1.tileSize * 3 / 5);
                    Size size2 = Game1.viewport.Size;
                    if (layer2.PickTile(mapDisplayLocation2, size2).TileIndexProperties.ContainsKey("FrontAlways"))
                      goto label_127;
                  }
                  else
                    goto label_127;
                }
                Game1.drawPlayerHeldObject(Game1.player);
              }
label_127:
              if ((Game1.player.UsingTool || Game1.pickingTool) && Game1.player.CurrentTool != null && ((!Game1.player.CurrentTool.Name.Equals("Seeds") || Game1.pickingTool) && (Game1.currentLocation.Map.GetLayer("Front").PickTile(new Location(Game1.player.getStandingX(), (int) Game1.player.position.Y - Game1.tileSize * 3 / 5), Game1.viewport.Size) != null && Game1.currentLocation.Map.GetLayer("Front").PickTile(new Location(Game1.player.getStandingX(), Game1.player.getStandingY()), Game1.viewport.Size) == null)))
                Game1.drawTool(Game1.player);
              if (Game1.currentLocation.Map.GetLayer("AlwaysFront") != null)
              {
                Game1.mapDisplayDevice.BeginScene(Game1.spriteBatch);
                Game1.currentLocation.Map.GetLayer("AlwaysFront").Draw(Game1.mapDisplayDevice, Game1.viewport, Location.Origin, false, Game1.pixelZoom);
                Game1.mapDisplayDevice.EndScene();
              }
              if ((double) Game1.toolHold > 400.0 && Game1.player.CurrentTool.UpgradeLevel >= 1 && Game1.player.canReleaseTool)
              {
                Color color = Color.White;
                switch ((int) ((double) Game1.toolHold / 600.0) + 2)
                {
                  case 1:
                    color = Tool.copperColor;
                    break;
                  case 2:
                    color = Tool.steelColor;
                    break;
                  case 3:
                    color = Tool.goldColor;
                    break;
                  case 4:
                    color = Tool.iridiumColor;
                    break;
                }
                Game1.spriteBatch.Draw(Game1.littleEffect, new Microsoft.Xna.Framework.Rectangle((int) Game1.player.getLocalPosition(Game1.viewport).X - 2, (int) Game1.player.getLocalPosition(Game1.viewport).Y - (Game1.player.CurrentTool.Name.Equals("Watering Can") ? 0 : Game1.tileSize) - 2, (int) ((double) Game1.toolHold % 600.0 * 0.0799999982118607) + 4, Game1.tileSize / 8 + 4), Color.Black);
                Game1.spriteBatch.Draw(Game1.littleEffect, new Microsoft.Xna.Framework.Rectangle((int) Game1.player.getLocalPosition(Game1.viewport).X, (int) Game1.player.getLocalPosition(Game1.viewport).Y - (Game1.player.CurrentTool.Name.Equals("Watering Can") ? 0 : Game1.tileSize), (int) ((double) Game1.toolHold % 600.0 * 0.0799999982118607), Game1.tileSize / 8), color);
              }
              if (Game1.isDebrisWeather && Game1.currentLocation.IsOutdoors && (!Game1.currentLocation.ignoreDebrisWeather && !Game1.currentLocation.Name.Equals("Desert")) && Game1.viewport.X > -10)
              {
                foreach (WeatherDebris weatherDebris in Game1.debrisWeather)
                  weatherDebris.draw(Game1.spriteBatch);
              }
              if (Game1.farmEvent != null)
                Game1.farmEvent.draw(Game1.spriteBatch);
              if ((double) Game1.currentLocation.LightLevel > 0.0 && Game1.timeOfDay < 2000)
                Game1.spriteBatch.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * Game1.currentLocation.LightLevel);
              if (Game1.screenGlow)
                Game1.spriteBatch.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Game1.screenGlowColor * Game1.screenGlowAlpha);
              Game1.currentLocation.drawAboveAlwaysFrontLayer(Game1.spriteBatch);
              if (Game1.player.CurrentTool != null && Game1.player.CurrentTool is FishingRod && ((Game1.player.CurrentTool as FishingRod).isTimingCast || (double) (Game1.player.CurrentTool as FishingRod).castingChosenCountdown > 0.0 || ((Game1.player.CurrentTool as FishingRod).fishCaught || (Game1.player.CurrentTool as FishingRod).showingTreasure)))
                Game1.player.CurrentTool.draw(Game1.spriteBatch);
              if (Game1.isRaining && Game1.currentLocation.IsOutdoors && (!Game1.currentLocation.Name.Equals("Desert") && !(Game1.currentLocation is Summit)) && (!Game1.eventUp || Game1.currentLocation.isTileOnMap(new Vector2((float) (Game1.viewport.X / Game1.tileSize), (float) (Game1.viewport.Y / Game1.tileSize)))))
              {
                for (int index = 0; index < Game1.rainDrops.Length; ++index)
                  Game1.spriteBatch.Draw(Game1.rainTexture, Game1.rainDrops[index].position, new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.rainTexture, Game1.rainDrops[index].frame, -1, -1)), Color.White);
              }
              Game1.spriteBatch.End();
              base.Draw(gameTime);
              Game1.spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
              if (Game1.eventUp && Game1.currentLocation.currentEvent != null)
              {
                foreach (NPC actor in Game1.currentLocation.currentEvent.actors)
                {
                  if (actor.isEmoting)
                  {
                    Vector2 localPosition = actor.getLocalPosition(Game1.viewport);
                    localPosition.Y -= (float) (Game1.tileSize * 2 + Game1.pixelZoom * 3);
                    if (actor.age == 2)
                      localPosition.Y += (float) (Game1.tileSize / 2);
                    else if (actor.gender == 1)
                      localPosition.Y += (float) (Game1.tileSize / 6);
                    Game1.spriteBatch.Draw(Game1.emoteSpriteSheet, localPosition, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(actor.CurrentEmoteIndex * (Game1.tileSize / 4) % Game1.emoteSpriteSheet.Width, actor.CurrentEmoteIndex * (Game1.tileSize / 4) / Game1.emoteSpriteSheet.Width * (Game1.tileSize / 4), Game1.tileSize / 4, Game1.tileSize / 4)), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, (float) actor.getStandingY() / 10000f);
                  }
                }
              }
              Game1.spriteBatch.End();
              if (Game1.drawLighting)
              {
                Game1.spriteBatch.Begin(SpriteSortMode.Deferred, this.lightingBlend, SamplerState.LinearClamp, (DepthStencilState) null, (RasterizerState) null);
                Game1.spriteBatch.Draw((Texture2D) Game1.lightmap, Vector2.Zero, new Microsoft.Xna.Framework.Rectangle?(Game1.lightmap.Bounds), Color.White, 0.0f, Vector2.Zero, (float) (Game1.options.lightingQuality / 2), SpriteEffects.None, 1f);
                if (Game1.isRaining && Game1.currentLocation.isOutdoors && !(Game1.currentLocation is Desert))
                  Game1.spriteBatch.Draw(Game1.staminaRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.OrangeRed * 0.45f);
                Game1.spriteBatch.End();
              }
              Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
              if (Game1.drawGrid)
              {
                int x1 = -Game1.viewport.X % Game1.tileSize;
                float num1 = (float) (-Game1.viewport.Y % Game1.tileSize);
                int x2 = x1;
                while (x2 < Game1.graphics.GraphicsDevice.Viewport.Width)
                {
                  Game1.spriteBatch.Draw(Game1.staminaRect, new Microsoft.Xna.Framework.Rectangle(x2, (int) num1, 1, Game1.graphics.GraphicsDevice.Viewport.Height), Color.Red * 0.5f);
                  x2 += Game1.tileSize;
                }
                float num2 = num1;
                while ((double) num2 < (double) Game1.graphics.GraphicsDevice.Viewport.Height)
                {
                  Game1.spriteBatch.Draw(Game1.staminaRect, new Microsoft.Xna.Framework.Rectangle(x1, (int) num2, Game1.graphics.GraphicsDevice.Viewport.Width, 1), Color.Red * 0.5f);
                  num2 += (float) Game1.tileSize;
                }
              }
              if (Game1.currentBillboard != 0)
                this.drawBillboard();
              if ((Game1.displayHUD || Game1.eventUp) && (Game1.currentBillboard == 0 && (int) Game1.gameMode == 3) && (!Game1.freezeControls && !Game1.panMode))
                this.drawHUD();
              else if (Game1.activeClickableMenu == null && Game1.farmEvent == null)
                Game1.spriteBatch.Draw(Game1.mouseCursors, new Vector2((float) Game1.getOldMouseX(), (float) Game1.getOldMouseY()), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 0, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) (4.0 + (double) Game1.dialogueButtonScale / 150.0), SpriteEffects.None, 1f);
              if (Game1.hudMessages.Count > 0 && (!Game1.eventUp || Game1.isFestival()))
              {
                for (int i = Game1.hudMessages.Count - 1; i >= 0; --i)
                  Game1.hudMessages[i].draw(Game1.spriteBatch, i);
              }
            }
            if (Game1.farmEvent != null)
              Game1.farmEvent.draw(Game1.spriteBatch);
            if (Game1.dialogueUp && !Game1.nameSelectUp && !Game1.messagePause && (Game1.activeClickableMenu == null || !(Game1.activeClickableMenu is DialogueBox)))
              this.drawDialogueBox();
            Viewport viewport;
            if (Game1.progressBar)
            {
              SpriteBatch spriteBatch1 = Game1.spriteBatch;
              Texture2D fadeToBlackRect = Game1.fadeToBlackRect;
              int x1 = (Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Width - Game1.dialogueWidth) / 2;
              rectangle = Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea;
              int y1 = rectangle.Bottom - Game1.tileSize * 2;
              int dialogueWidth = Game1.dialogueWidth;
              int height1 = Game1.tileSize / 2;
              Microsoft.Xna.Framework.Rectangle destinationRectangle1 = new Microsoft.Xna.Framework.Rectangle(x1, y1, dialogueWidth, height1);
              Color lightGray = Color.LightGray;
              spriteBatch1.Draw(fadeToBlackRect, destinationRectangle1, lightGray);
              SpriteBatch spriteBatch2 = Game1.spriteBatch;
              Texture2D staminaRect = Game1.staminaRect;
              viewport = Game1.graphics.GraphicsDevice.Viewport;
              int x2 = (viewport.TitleSafeArea.Width - Game1.dialogueWidth) / 2;
              viewport = Game1.graphics.GraphicsDevice.Viewport;
              rectangle = viewport.TitleSafeArea;
              int y2 = rectangle.Bottom - Game1.tileSize * 2;
              int width = (int) ((double) Game1.pauseAccumulator / (double) Game1.pauseTime * (double) Game1.dialogueWidth);
              int height2 = Game1.tileSize / 2;
              Microsoft.Xna.Framework.Rectangle destinationRectangle2 = new Microsoft.Xna.Framework.Rectangle(x2, y2, width, height2);
              Color dimGray = Color.DimGray;
              spriteBatch2.Draw(staminaRect, destinationRectangle2, dimGray);
            }
            if (Game1.eventUp && Game1.currentLocation != null && Game1.currentLocation.currentEvent != null)
              Game1.currentLocation.currentEvent.drawAfterMap(Game1.spriteBatch);
            if (Game1.isRaining && Game1.currentLocation != null && (Game1.currentLocation.isOutdoors && !(Game1.currentLocation is Desert)))
            {
              SpriteBatch spriteBatch = Game1.spriteBatch;
              Texture2D staminaRect = Game1.staminaRect;
              viewport = Game1.graphics.GraphicsDevice.Viewport;
              Microsoft.Xna.Framework.Rectangle bounds = viewport.Bounds;
              Color color = Color.Blue * 0.2f;
              spriteBatch.Draw(staminaRect, bounds, color);
            }
            if ((Game1.fadeToBlack || Game1.globalFade) && !Game1.menuUp && (!Game1.nameSelectUp || Game1.messagePause))
            {
              SpriteBatch spriteBatch = Game1.spriteBatch;
              Texture2D fadeToBlackRect = Game1.fadeToBlackRect;
              viewport = Game1.graphics.GraphicsDevice.Viewport;
              Microsoft.Xna.Framework.Rectangle bounds = viewport.Bounds;
              Color color = Color.Black * ((int) Game1.gameMode == 0 ? 1f - Game1.fadeToBlackAlpha : Game1.fadeToBlackAlpha);
              spriteBatch.Draw(fadeToBlackRect, bounds, color);
            }
            else if ((double) Game1.flashAlpha > 0.0)
            {
              if (Game1.options.screenFlash)
              {
                SpriteBatch spriteBatch = Game1.spriteBatch;
                Texture2D fadeToBlackRect = Game1.fadeToBlackRect;
                viewport = Game1.graphics.GraphicsDevice.Viewport;
                Microsoft.Xna.Framework.Rectangle bounds = viewport.Bounds;
                Color color = Color.White * Math.Min(1f, Game1.flashAlpha);
                spriteBatch.Draw(fadeToBlackRect, bounds, color);
              }
              Game1.flashAlpha -= 0.1f;
            }
            if ((Game1.messagePause || Game1.globalFade) && Game1.dialogueUp)
              this.drawDialogueBox();
            foreach (TemporaryAnimatedSprite overlayTempSprite in Game1.screenOverlayTempSprites)
              overlayTempSprite.draw(Game1.spriteBatch, true, 0, 0);
            if (Game1.debugMode)
            {
              SpriteBatch spriteBatch = Game1.spriteBatch;
              SpriteFont smallFont = Game1.smallFont;
              object[] objArray = new object[10];
              int index1 = 0;
              string str1;
              if (!Game1.panMode)
                str1 = "player: " + (object) (Game1.player.getStandingX() / Game1.tileSize) + ", " + (object) (Game1.player.getStandingY() / Game1.tileSize);
              else
                str1 = ((Game1.getOldMouseX() + Game1.viewport.X) / Game1.tileSize).ToString() + "," + (object) ((Game1.getOldMouseY() + Game1.viewport.Y) / Game1.tileSize);
              objArray[index1] = (object) str1;
              int index2 = 1;
              string str2 = " mouseTransparency: ";
              objArray[index2] = (object) str2;
              int index3 = 2;
              // ISSUE: variable of a boxed type
              __Boxed<float> cursorTransparency = (ValueType) Game1.mouseCursorTransparency;
              objArray[index3] = (object) cursorTransparency;
              int index4 = 3;
              string str3 = " mousePosition: ";
              objArray[index4] = (object) str3;
              int index5 = 4;
              // ISSUE: variable of a boxed type
              __Boxed<int> mouseX = (ValueType) Game1.getMouseX();
              objArray[index5] = (object) mouseX;
              int index6 = 5;
              string str4 = ",";
              objArray[index6] = (object) str4;
              int index7 = 6;
              // ISSUE: variable of a boxed type
              __Boxed<int> mouseY = (ValueType) Game1.getMouseY();
              objArray[index7] = (object) mouseY;
              int index8 = 7;
              string newLine = Environment.NewLine;
              objArray[index8] = (object) newLine;
              int index9 = 8;
              string str5 = "debugOutput: ";
              objArray[index9] = (object) str5;
              int index10 = 9;
              string debugOutput = Game1.debugOutput;
              objArray[index10] = (object) debugOutput;
              string text = string.Concat(objArray);
              Vector2 position = new Vector2((float) this.GraphicsDevice.Viewport.TitleSafeArea.X, (float) this.GraphicsDevice.Viewport.TitleSafeArea.Y);
              Color red = Color.Red;
              double num1 = 0.0;
              Vector2 zero = Vector2.Zero;
              double num2 = 1.0;
              int num3 = 0;
              double num4 = 0.99999988079071;
              spriteBatch.DrawString(smallFont, text, position, red, (float) num1, zero, (float) num2, (SpriteEffects) num3, (float) num4);
            }
            if (Game1.showKeyHelp)
              Game1.spriteBatch.DrawString(Game1.smallFont, Game1.keyHelpString, new Vector2((float) Game1.tileSize, (float) (Game1.viewport.Height - Game1.tileSize - (Game1.dialogueUp ? Game1.tileSize * 3 + (Game1.isQuestion ? Game1.questionChoices.Count * Game1.tileSize : 0) : 0)) - Game1.smallFont.MeasureString(Game1.keyHelpString).Y), Color.LightGray, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9999999f);
            if (Game1.activeClickableMenu != null)
              Game1.activeClickableMenu.draw(Game1.spriteBatch);
            else if (Game1.farmEvent != null)
              Game1.farmEvent.drawAboveEverything(Game1.spriteBatch);
            Game1.spriteBatch.End();
            if (Game1.overlayMenu != null)
            {
              Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
              Game1.overlayMenu.draw(Game1.spriteBatch);
              Game1.spriteBatch.End();
            }
            this.renderScreenBuffer();
          }
        }
      }
    }

    private void renderScreenBuffer()
    {
      if ((double) Game1.options.zoomLevel == 1.0)
        return;
      this.GraphicsDevice.SetRenderTarget((RenderTarget2D) null);
      this.GraphicsDevice.Clear(this.bgColor);
      Game1.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);
      Game1.spriteBatch.Draw((Texture2D) this.screen, Vector2.Zero, new Microsoft.Xna.Framework.Rectangle?(this.screen.Bounds), Color.White, 0.0f, Vector2.Zero, Game1.options.zoomLevel, SpriteEffects.None, 1f);
      Game1.spriteBatch.End();
    }

    public static void drawWithBorder(string message, Color borderColor, Color insideColor, Vector2 position)
    {
      Game1.drawWithBorder(message, borderColor, insideColor, position, 0.0f, 1f, 1f, false);
    }

    public static void drawWithBorder(string message, Color borderColor, Color insideColor, Vector2 position, float rotate, float scale, float layerDepth)
    {
      Game1.drawWithBorder(message, borderColor, insideColor, position, rotate, scale, layerDepth, false);
    }

    public static void drawWithBorder(string message, Color borderColor, Color insideColor, Vector2 position, float rotate, float scale, float layerDepth, bool tiny)
    {
      string[] strArray = message.Split(Utility.CharSpace);
      int num = 0;
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (strArray[index].Contains("="))
        {
          Game1.spriteBatch.DrawString(tiny ? Game1.tinyFont : Game1.dialogueFont, strArray[index], new Vector2(position.X + (float) num, position.Y), Color.Purple, rotate, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
          num += (int) ((double) (tiny ? Game1.tinyFont : Game1.dialogueFont).MeasureString(strArray[index]).X + 8.0);
        }
        else
        {
          Game1.spriteBatch.DrawString(tiny ? Game1.tinyFont : Game1.dialogueFont, strArray[index], new Vector2(position.X + (float) num, position.Y), insideColor, rotate, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
          num += (int) ((double) (tiny ? Game1.tinyFont : Game1.dialogueFont).MeasureString(strArray[index]).X + 8.0);
        }
      }
    }

    public static bool isOutdoorMapSmallerThanViewport()
    {
      if (Game1.currentLocation != null && Game1.currentLocation.IsOutdoors)
        return Game1.currentLocation.map.Layers[0].LayerWidth * Game1.tileSize < Game1.viewport.Width;
      return false;
    }

    private void drawHUD()
    {
      if (!Game1.eventUp)
      {
        float num1 = 0.625f;
        Vector2 position1 = new Vector2((float) (Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Right - 48 - Game1.tileSize / 8), (float) (Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Bottom - 224 - Game1.tileSize / 4 - (int) ((double) (Game1.player.MaxStamina - 270) * (double) num1)));
        if (Game1.isOutdoorMapSmallerThanViewport())
          position1.X = Math.Min(position1.X, (float) (-Game1.viewport.X + Game1.currentLocation.map.Layers[0].LayerWidth * Game1.tileSize - 48));
        if (Game1.staminaShakeTimer > 0)
        {
          position1.X += (float) Game1.random.Next(-3, 4);
          position1.Y += (float) Game1.random.Next(-3, 4);
        }
        Game1.spriteBatch.Draw(Game1.mouseCursors, position1, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(256, 408, 12, 16)), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
        Game1.spriteBatch.Draw(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle((int) position1.X, (int) ((double) position1.Y + (double) Game1.tileSize), 48, Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Bottom - Game1.tileSize - Game1.tileSize / 4 - (int) ((double) position1.Y + (double) Game1.tileSize - 8.0)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(256, 424, 12, 16)), Color.White);
        Game1.spriteBatch.Draw(Game1.mouseCursors, new Vector2(position1.X, position1.Y + 224f + (float) (int) ((double) (Game1.player.MaxStamina - 270) * (double) num1) - (float) Game1.tileSize), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(256, 448, 12, 16)), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
        Microsoft.Xna.Framework.Rectangle destinationRectangle1 = new Microsoft.Xna.Framework.Rectangle((int) position1.X + 12, (int) position1.Y + 16 + Game1.tileSize / 2 + (int) ((double) (Game1.player.MaxStamina - (int) Math.Max(0.0f, Game1.player.Stamina)) * (double) num1), 24, (int) ((double) Game1.player.Stamina * (double) num1));
        if ((double) Game1.getOldMouseX() >= (double) position1.X && (double) Game1.getOldMouseY() >= (double) position1.Y)
          Game1.drawWithBorder(((int) Math.Max(0.0f, Game1.player.Stamina)).ToString() + "/" + (object) Game1.player.MaxStamina, Color.Black * 0.0f, Color.White, position1 + new Vector2((float) (-(double) Game1.dialogueFont.MeasureString("999/999").X - (double) (Game1.tileSize / 4) - (Game1.showingHealth ? (double) Game1.tileSize : 0.0)), (float) Game1.tileSize));
        Color toGreenLerpColor = Utility.getRedToGreenLerpColor(Game1.player.stamina / (float) Game1.player.maxStamina);
        Game1.spriteBatch.Draw(Game1.staminaRect, destinationRectangle1, toGreenLerpColor);
        destinationRectangle1.Height = Game1.pixelZoom;
        toGreenLerpColor.R = (byte) Math.Max(0, (int) toGreenLerpColor.R - 50);
        toGreenLerpColor.G = (byte) Math.Max(0, (int) toGreenLerpColor.G - 50);
        Game1.spriteBatch.Draw(Game1.staminaRect, destinationRectangle1, toGreenLerpColor);
        if (Game1.player.exhausted)
        {
          Game1.spriteBatch.Draw(Game1.mouseCursors, position1 - new Vector2(0.0f, 11f) * (float) Game1.pixelZoom, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(191, 406, 12, 11)), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
          if ((double) Game1.getOldMouseX() >= (double) position1.X && (double) Game1.getOldMouseY() >= (double) position1.Y - (double) (11 * Game1.pixelZoom))
            Game1.drawWithBorder(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3747"), Color.Black * 0.0f, Color.White, position1 + new Vector2((float) (-(double) Game1.dialogueFont.MeasureString(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3747")).X - (double) (Game1.tileSize / 4) - (Game1.showingHealth ? (double) Game1.tileSize : 0.0)), (float) (Game1.tileSize * 3 / 2)));
        }
        if (Game1.currentLocation is MineShaft || Game1.currentLocation is Woods || (Game1.currentLocation is SlimeHutch || Game1.player.health < Game1.player.maxHealth))
        {
          Game1.showingHealth = true;
          position1.X -= (float) (48 + Game1.tileSize / 8 + (Game1.hitShakeTimer > 0 ? Game1.random.Next(-3, 4) : 0));
          // ISSUE: explicit reference operation
          // ISSUE: variable of a reference type
          Vector2& local = @position1;
          Microsoft.Xna.Framework.Rectangle titleSafeArea = Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea;
          double num2 = (double) (titleSafeArea.Bottom - 224 - (Game1.player.maxHealth - 100) - Game1.tileSize / 4 + 4);
          // ISSUE: explicit reference operation
          (^local).Y = (float) num2;
          SpriteBatch spriteBatch1 = Game1.spriteBatch;
          Texture2D mouseCursors1 = Game1.mouseCursors;
          Vector2 position2 = position1;
          Microsoft.Xna.Framework.Rectangle? sourceRectangle1 = new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(268, 408, 12, 16));
          DateTime now;
          Color color1;
          if (Game1.player.health >= 20)
          {
            color1 = Color.White;
          }
          else
          {
            Color pink = Color.Pink;
            now = DateTime.Now;
            double num3 = Math.Sin(now.TimeOfDay.TotalMilliseconds / ((double) Game1.player.health * 50.0)) / 4.0 + 0.899999976158142;
            color1 = pink * (float) num3;
          }
          double num4 = 0.0;
          Vector2 zero1 = Vector2.Zero;
          double num5 = 4.0;
          int num6 = 0;
          double num7 = 1.0;
          spriteBatch1.Draw(mouseCursors1, position2, sourceRectangle1, color1, (float) num4, zero1, (float) num5, (SpriteEffects) num6, (float) num7);
          SpriteBatch spriteBatch2 = Game1.spriteBatch;
          Texture2D mouseCursors2 = Game1.mouseCursors;
          int x1 = (int) position1.X;
          int y1 = (int) ((double) position1.Y + (double) Game1.tileSize);
          int width1 = 48;
          titleSafeArea = Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea;
          int height1 = titleSafeArea.Bottom - Game1.tileSize - Game1.tileSize / 4 - (int) ((double) position1.Y + (double) Game1.tileSize);
          Microsoft.Xna.Framework.Rectangle destinationRectangle2 = new Microsoft.Xna.Framework.Rectangle(x1, y1, width1, height1);
          Microsoft.Xna.Framework.Rectangle? sourceRectangle2 = new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(268, 424, 12, 16));
          Color color2;
          if (Game1.player.health >= 20)
          {
            color2 = Color.White;
          }
          else
          {
            Color pink = Color.Pink;
            now = DateTime.Now;
            double num3 = Math.Sin(now.TimeOfDay.TotalMilliseconds / ((double) Game1.player.health * 50.0)) / 4.0 + 0.899999976158142;
            color2 = pink * (float) num3;
          }
          spriteBatch2.Draw(mouseCursors2, destinationRectangle2, sourceRectangle2, color2);
          SpriteBatch spriteBatch3 = Game1.spriteBatch;
          Texture2D mouseCursors3 = Game1.mouseCursors;
          Vector2 position3 = new Vector2(position1.X, (float) ((double) position1.Y + 220.0 + (double) (Game1.player.maxHealth - 100) - 64.0));
          Microsoft.Xna.Framework.Rectangle? sourceRectangle3 = new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(268, 448, 12, 16));
          Color color3;
          if (Game1.player.health >= 20)
          {
            color3 = Color.White;
          }
          else
          {
            Color pink = Color.Pink;
            now = DateTime.Now;
            double num3 = Math.Sin(now.TimeOfDay.TotalMilliseconds / ((double) Game1.player.health * 50.0)) / 4.0 + 0.899999976158142;
            color3 = pink * (float) num3;
          }
          double num8 = 0.0;
          Vector2 zero2 = Vector2.Zero;
          double num9 = 4.0;
          int num10 = 0;
          double num11 = 1.0;
          spriteBatch3.Draw(mouseCursors3, position3, sourceRectangle3, color3, (float) num8, zero2, (float) num9, (SpriteEffects) num10, (float) num11);
          int num12 = (int) ((double) Game1.player.health / (double) Game1.player.maxHealth * (double) (168 + (Game1.player.maxHealth - 100)));
          toGreenLerpColor = Utility.getRedToGreenLerpColor((float) Game1.player.health / (float) Game1.player.maxHealth);
          SpriteBatch spriteBatch4 = Game1.spriteBatch;
          Texture2D staminaRect1 = Game1.staminaRect;
          int x2 = (int) position1.X + 12;
          int num13 = (int) position1.Y + Game1.tileSize / 2;
          titleSafeArea = Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea;
          int num14 = titleSafeArea.Bottom - Game1.tileSize - Game1.tileSize / 4 - (int) position1.Y + 24 - num12;
          int y2 = num13 + num14;
          int width2 = 24;
          int height2 = num12;
          Microsoft.Xna.Framework.Rectangle destinationRectangle3 = new Microsoft.Xna.Framework.Rectangle(x2, y2, width2, height2);
          Microsoft.Xna.Framework.Rectangle? sourceRectangle4 = new Microsoft.Xna.Framework.Rectangle?(Game1.staminaRect.Bounds);
          Color color4 = toGreenLerpColor;
          double num15 = 0.0;
          Vector2 zero3 = Vector2.Zero;
          int num16 = 0;
          double num17 = 1.0;
          spriteBatch4.Draw(staminaRect1, destinationRectangle3, sourceRectangle4, color4, (float) num15, zero3, (SpriteEffects) num16, (float) num17);
          toGreenLerpColor.R = (byte) Math.Max(0, (int) toGreenLerpColor.R - 50);
          toGreenLerpColor.G = (byte) Math.Max(0, (int) toGreenLerpColor.G - 50);
          if ((double) Game1.getOldMouseX() >= (double) position1.X && (double) Game1.getOldMouseY() >= (double) position1.Y && (double) Game1.getOldMouseX() < (double) position1.X + (double) (Game1.tileSize / 2))
            Game1.drawWithBorder(Math.Max(0, Game1.player.health).ToString() + "/" + (object) Game1.player.maxHealth, Color.Black * 0.0f, Color.Red, position1 + new Vector2(-Game1.dialogueFont.MeasureString("999/999").X - (float) (Game1.tileSize / 2), (float) Game1.tileSize));
          SpriteBatch spriteBatch5 = Game1.spriteBatch;
          Texture2D staminaRect2 = Game1.staminaRect;
          int x3 = (int) position1.X + 12;
          int num18 = (int) position1.Y + Game1.tileSize / 2;
          titleSafeArea = Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea;
          int num19 = titleSafeArea.Bottom - Game1.tileSize - Game1.tileSize / 4 - (int) position1.Y + 24 - num12;
          int y3 = num18 + num19;
          int width3 = 24;
          int pixelZoom = Game1.pixelZoom;
          Microsoft.Xna.Framework.Rectangle destinationRectangle4 = new Microsoft.Xna.Framework.Rectangle(x3, y3, width3, pixelZoom);
          Microsoft.Xna.Framework.Rectangle? sourceRectangle5 = new Microsoft.Xna.Framework.Rectangle?(Game1.staminaRect.Bounds);
          Color color5 = toGreenLerpColor;
          double num20 = 0.0;
          Vector2 zero4 = Vector2.Zero;
          int num21 = 0;
          double num22 = 1.0;
          spriteBatch5.Draw(staminaRect2, destinationRectangle4, sourceRectangle5, color5, (float) num20, zero4, (SpriteEffects) num21, (float) num22);
        }
        else
          Game1.showingHealth = false;
        Object activeObject = Game1.player.ActiveObject;
        if (Game1.isOutdoorMapSmallerThanViewport())
        {
          SpriteBatch spriteBatch1 = Game1.spriteBatch;
          Texture2D fadeToBlackRect1 = Game1.fadeToBlackRect;
          int x1 = 0;
          int y1 = 0;
          int width1 = -Math.Min(Game1.viewport.X, 4096);
          Viewport viewport = Game1.graphics.GraphicsDevice.Viewport;
          int height1 = viewport.Height;
          Microsoft.Xna.Framework.Rectangle destinationRectangle2 = new Microsoft.Xna.Framework.Rectangle(x1, y1, width1, height1);
          Color black1 = Color.Black;
          spriteBatch1.Draw(fadeToBlackRect1, destinationRectangle2, black1);
          SpriteBatch spriteBatch2 = Game1.spriteBatch;
          Texture2D fadeToBlackRect2 = Game1.fadeToBlackRect;
          int x2 = -Game1.viewport.X + Game1.currentLocation.map.Layers[0].LayerWidth * Game1.tileSize;
          int y2 = 0;
          int val1 = 4096;
          viewport = Game1.graphics.GraphicsDevice.Viewport;
          int val2 = viewport.Width - (-Game1.viewport.X + Game1.currentLocation.map.Layers[0].LayerWidth * Game1.tileSize);
          int width2 = Math.Min(val1, val2);
          int height2 = Game1.graphics.GraphicsDevice.Viewport.Height;
          Microsoft.Xna.Framework.Rectangle destinationRectangle3 = new Microsoft.Xna.Framework.Rectangle(x2, y2, width2, height2);
          Color black2 = Color.Black;
          spriteBatch2.Draw(fadeToBlackRect2, destinationRectangle3, black2);
        }
        foreach (IClickableMenu onScreenMenu in Game1.onScreenMenus)
        {
          GameTime currentGameTime = Game1.currentGameTime;
          onScreenMenu.update(currentGameTime);
          SpriteBatch spriteBatch = Game1.spriteBatch;
          onScreenMenu.draw(spriteBatch);
        }
        if (Game1.player.professions.Contains(17) && Game1.currentLocation.IsOutdoors)
        {
          foreach (KeyValuePair<Vector2, Object> keyValuePair in (Dictionary<Vector2, Object>) Game1.currentLocation.objects)
          {
            if ((keyValuePair.Value.isSpawnedObject || keyValuePair.Value.ParentSheetIndex == 590) && !Utility.isOnScreen(keyValuePair.Key * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2)), 64))
            {
              Vector2 position2 = new Vector2();
              float rotation = 0.0f;
              Microsoft.Xna.Framework.Rectangle bounds;
              if ((double) keyValuePair.Key.X * (double) Game1.tileSize > (double) (Game1.viewport.MaxCorner.X - 64))
              {
                // ISSUE: explicit reference operation
                // ISSUE: variable of a reference type
                Vector2& local = @position2;
                bounds = Game1.graphics.GraphicsDevice.Viewport.Bounds;
                double num2 = (double) (bounds.Right - 8);
                // ISSUE: explicit reference operation
                (^local).X = (float) num2;
                rotation = 1.570796f;
              }
              else if ((double) keyValuePair.Key.X * (double) Game1.tileSize < (double) Game1.viewport.X)
              {
                position2.X = 8f;
                rotation = -1.570796f;
              }
              else
                position2.X = keyValuePair.Key.X * (float) Game1.tileSize - (float) Game1.viewport.X;
              if ((double) keyValuePair.Key.Y * (double) Game1.tileSize > (double) (Game1.viewport.MaxCorner.Y - 64))
              {
                // ISSUE: explicit reference operation
                // ISSUE: variable of a reference type
                Vector2& local = @position2;
                bounds = Game1.graphics.GraphicsDevice.Viewport.Bounds;
                double num2 = (double) (bounds.Bottom - 8);
                // ISSUE: explicit reference operation
                (^local).Y = (float) num2;
                rotation = 3.141593f;
              }
              else
                position2.Y = (double) keyValuePair.Key.Y * (double) Game1.tileSize >= (double) Game1.viewport.Y ? keyValuePair.Key.Y * (float) Game1.tileSize - (float) Game1.viewport.Y : 8f;
              if ((double) position2.X == 8.0 && (double) position2.Y == 8.0)
                rotation += 0.7853982f;
              if ((double) position2.X == 8.0)
              {
                double y = (double) position2.Y;
                bounds = Game1.graphics.GraphicsDevice.Viewport.Bounds;
                double num2 = (double) (bounds.Bottom - 8);
                if (y == num2)
                  rotation += 0.7853982f;
              }
              double x1 = (double) position2.X;
              bounds = Game1.graphics.GraphicsDevice.Viewport.Bounds;
              double num3 = (double) (bounds.Right - 8);
              if (x1 == num3 && (double) position2.Y == 8.0)
                rotation -= 0.7853982f;
              double x2 = (double) position2.X;
              bounds = Game1.graphics.GraphicsDevice.Viewport.Bounds;
              double num4 = (double) (bounds.Right - 8);
              if (x2 == num4)
              {
                double y = (double) position2.Y;
                bounds = Game1.graphics.GraphicsDevice.Viewport.Bounds;
                double num2 = (double) (bounds.Bottom - 8);
                if (y == num2)
                  rotation -= 0.7853982f;
              }
              Game1.spriteBatch.Draw(Game1.mouseCursors, position2, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(412, 495, 5, 4)), Color.White, rotation, new Vector2(2f, 2f), 4f, SpriteEffects.None, 1f);
            }
          }
        }
      }
      if (Game1.timerUntilMouseFade > 0)
      {
        Game1.timerUntilMouseFade -= Game1.currentGameTime.ElapsedGameTime.Milliseconds;
        if (Game1.timerUntilMouseFade <= 0)
          Game1.lastMousePositionBeforeFade = Game1.getMousePosition();
      }
      if (Game1.options.gamepadControls && Game1.timerUntilMouseFade <= 0 && Game1.activeClickableMenu == null)
      {
        Game1.mouseCursorTransparency = 0.0f;
        if (this.IsActive)
        {
          IClickableMenu activeClickableMenu = Game1.activeClickableMenu;
        }
      }
      if (Game1.activeClickableMenu == null && Game1.mouseCursor > -1 && (Mouse.GetState().X != 0 || Mouse.GetState().Y != 0) && (Game1.getOldMouseX() != 0 || Game1.getOldMouseY() != 0))
      {
        if ((double) Game1.mouseCursorTransparency <= 0.0 || !Utility.canGrabSomethingFromHere(Game1.getOldMouseX() + Game1.viewport.X, Game1.getOldMouseY() + Game1.viewport.Y, Game1.player) || Game1.mouseCursor == 3)
        {
          if (Game1.player.ActiveObject != null && Game1.mouseCursor != 3 && !Game1.eventUp)
          {
            if ((double) Game1.mouseCursorTransparency > 0.0 || Game1.options.showPlacementTileForGamepad)
            {
              Game1.player.ActiveObject.drawPlacementBounds(Game1.spriteBatch, Game1.currentLocation);
              if ((double) Game1.mouseCursorTransparency > 0.0)
              {
                bool flag = Utility.playerCanPlaceItemHere(Game1.currentLocation, Game1.player.CurrentItem, Game1.getMouseX() + Game1.viewport.X, Game1.getMouseY() + Game1.viewport.Y, Game1.player) || Utility.isThereAnObjectHereWhichAcceptsThisItem(Game1.currentLocation, Game1.player.CurrentItem, Game1.getMouseX() + Game1.viewport.X, Game1.getMouseY() + Game1.viewport.Y) && Utility.withinRadiusOfPlayer(Game1.getMouseX() + Game1.viewport.X, Game1.getMouseY() + Game1.viewport.Y, 1, Game1.player);
                Game1.player.CurrentItem.drawInMenu(Game1.spriteBatch, new Vector2((float) (Game1.getMouseX() + Game1.tileSize / 4), (float) (Game1.getMouseY() + Game1.tileSize / 4)), flag ? (float) ((double) Game1.dialogueButtonScale / 75.0 + 1.0) : 1f, flag ? 1f : 0.5f, 0.999f);
              }
            }
          }
          else if (Game1.mouseCursor == 0 && Game1.isActionAtCurrentCursorTile)
            Game1.mouseCursor = Game1.isInspectionAtCurrentCursorTile ? 5 : 2;
        }
        if (!Game1.options.hardwareCursor)
          Game1.spriteBatch.Draw(Game1.mouseCursors, new Vector2((float) Game1.getMouseX(), (float) Game1.getMouseY()), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, Game1.mouseCursor, 16, 16)), Color.White * Game1.mouseCursorTransparency, 0.0f, Vector2.Zero, (float) Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
        Game1.wasMouseVisibleThisFrame = (double) Game1.mouseCursorTransparency > 0.0;
      }
      Game1.mouseCursor = 0;
      if (Game1.isActionAtCurrentCursorTile || Game1.activeClickableMenu != null)
        return;
      Game1.mouseCursorTransparency = 1f;
    }

    public static float mouseCursorTransparency
    {
      get
      {
        return Game1._mouseCursorTransparency;
      }
      set
      {
        Game1._mouseCursorTransparency = value;
      }
    }

    public static void panScreen(int x, int y)
    {
      Game1.previousViewportPosition.X = (float) Game1.viewport.Location.X;
      Game1.previousViewportPosition.Y = (float) Game1.viewport.Location.Y;
      Game1.viewport.X += x;
      Game1.viewport.Y += y;
      Game1.clampViewportToGameMap();
      Game1.updateRaindropPosition();
    }

    public static void clampViewportToGameMap()
    {
      if (Game1.viewport.X < 0)
        Game1.viewport.X = 0;
      if (Game1.viewport.X > Game1.currentLocation.map.DisplayWidth - Game1.viewport.Width)
        Game1.viewport.X = Game1.currentLocation.map.DisplayWidth - Game1.viewport.Width;
      if (Game1.viewport.Y < 0)
        Game1.viewport.Y = 0;
      if (Game1.viewport.Y <= Game1.currentLocation.map.DisplayHeight - Game1.viewport.Height)
        return;
      Game1.viewport.Y = Game1.currentLocation.map.DisplayHeight - Game1.viewport.Height;
    }

    public void drawBillboard()
    {
    }

    private void drawDialogueBox()
    {
      int num1 = 5 * Game1.tileSize;
      if (Game1.currentSpeaker != null)
      {
        int num2 = Math.Max((int) Game1.dialogueFont.MeasureString(Game1.currentSpeaker.CurrentDialogue.Peek().getCurrentDialogue()).Y, 5 * Game1.tileSize);
        int x = (this.GraphicsDevice.Viewport.TitleSafeArea.Width - Math.Min(1280, this.GraphicsDevice.Viewport.TitleSafeArea.Width - Game1.tileSize * 2)) / 2;
        Viewport viewport = this.GraphicsDevice.Viewport;
        int y = viewport.TitleSafeArea.Height - num2;
        int val1 = 1280;
        viewport = this.GraphicsDevice.Viewport;
        int val2 = viewport.TitleSafeArea.Width - Game1.tileSize * 2;
        int width = Math.Min(val1, val2);
        int height = num2;
        int num3 = 1;
        int num4 = 0;
        // ISSUE: variable of the null type
        __Null local = null;
        int num5 = Game1.objectDialoguePortraitPerson == null ? 0 : (Game1.currentSpeaker == null ? 1 : 0);
        Game1.drawDialogueBox(x, y, width, height, num3 != 0, num4 != 0, (string) local, num5 != 0);
      }
      else
      {
        int count = Game1.currentObjectDialogue.Count;
      }
    }

    public static void drawDialogueBox(string message)
    {
      Game1.drawDialogueBox(Game1.viewport.Width / 2, Game1.viewport.Height / 2, false, false, message);
    }

    public static void drawDialogueBox(int centerX, int centerY, bool speaker, bool drawOnlyBox, string message)
    {
      string text = (string) null;
      if (speaker && Game1.currentSpeaker != null)
        text = Game1.currentSpeaker.CurrentDialogue.Peek().getCurrentDialogue();
      else if (message != null)
        text = message;
      else if (Game1.currentObjectDialogue.Count > 0)
        text = Game1.currentObjectDialogue.Peek();
      if (text == null)
        return;
      Vector2 vector2 = Game1.dialogueFont.MeasureString(text);
      int width = (int) vector2.X + Game1.tileSize * 2;
      int height = (int) vector2.Y + Game1.tileSize * 2;
      Game1.drawDialogueBox(centerX - width / 2, centerY - height / 2, width, height, speaker, drawOnlyBox, message, Game1.objectDialoguePortraitPerson != null && !speaker);
    }

    public static void drawDialogueBox(int x, int y, int width, int height, bool speaker, bool drawOnlyBox, string message = null, bool objectDialogueWithPortrait = false)
    {
      if (!drawOnlyBox)
        return;
      int height1 = Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Height;
      int width1 = Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Width;
      int dialogueX = 0;
      int num1 = y > Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Y ? 0 : Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Y;
      int num2 = 0;
      width = Math.Min(Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Width, width);
      if (!Game1.isQuestion && Game1.currentSpeaker == null && (Game1.currentObjectDialogue.Count > 0 && !drawOnlyBox))
      {
        width = (int) Game1.dialogueFont.MeasureString(Game1.currentObjectDialogue.Peek()).X + Game1.tileSize * 2;
        height = (int) Game1.dialogueFont.MeasureString(Game1.currentObjectDialogue.Peek()).Y + Game1.tileSize;
        x = width1 / 2 - width / 2;
        num2 = height > Game1.tileSize * 4 ? -(height - Game1.tileSize * 4) : 0;
      }
      Microsoft.Xna.Framework.Rectangle rectangle1 = new Microsoft.Xna.Framework.Rectangle(0, 0, Game1.tileSize, Game1.tileSize);
      int addedTileHeightForQuestions = -1;
      if (Game1.questionChoices.Count >= 3)
        addedTileHeightForQuestions = Game1.questionChoices.Count - 3;
      if (!drawOnlyBox && Game1.currentObjectDialogue.Count > 0)
      {
        if ((double) Game1.dialogueFont.MeasureString(Game1.currentObjectDialogue.Peek()).Y >= (double) (height - Game1.tileSize * 2))
        {
          addedTileHeightForQuestions -= (int) (((double) (height - Game1.tileSize * 2) - (double) Game1.dialogueFont.MeasureString(Game1.currentObjectDialogue.Peek()).Y) / (double) Game1.tileSize) - 1;
        }
        else
        {
          height += (int) Game1.dialogueFont.MeasureString(Game1.currentObjectDialogue.Peek()).Y / 2;
          num2 -= (int) Game1.dialogueFont.MeasureString(Game1.currentObjectDialogue.Peek()).Y / 2;
          if ((int) Game1.dialogueFont.MeasureString(Game1.currentObjectDialogue.Peek()).Y / 2 > Game1.tileSize)
            addedTileHeightForQuestions = 0;
        }
      }
      if (Game1.currentSpeaker != null && Game1.isQuestion && Game1.currentSpeaker.CurrentDialogue.Peek().getCurrentDialogue().Substring(0, Game1.currentDialogueCharacterIndex).Contains(Environment.NewLine))
        ++addedTileHeightForQuestions;
      rectangle1.Width = Game1.tileSize;
      rectangle1.Height = Game1.tileSize;
      rectangle1.X = Game1.tileSize;
      rectangle1.Y = Game1.tileSize * 2;
      Game1.spriteBatch.Draw(Game1.menuTexture, new Microsoft.Xna.Framework.Rectangle(28 + x + dialogueX, 28 + y - Game1.tileSize * addedTileHeightForQuestions + num1 + num2, width - Game1.tileSize, height - Game1.tileSize + addedTileHeightForQuestions * Game1.tileSize), new Microsoft.Xna.Framework.Rectangle?(rectangle1), Color.White);
      rectangle1.Y = 0;
      rectangle1.X = 0;
      Game1.spriteBatch.Draw(Game1.menuTexture, new Vector2((float) (x + dialogueX), (float) (y - Game1.tileSize * addedTileHeightForQuestions + num1 + num2)), new Microsoft.Xna.Framework.Rectangle?(rectangle1), Color.White);
      rectangle1.X = Game1.tileSize * 3;
      Game1.spriteBatch.Draw(Game1.menuTexture, new Vector2((float) (x + width + dialogueX - Game1.tileSize), (float) (y - Game1.tileSize * addedTileHeightForQuestions + num1 + num2)), new Microsoft.Xna.Framework.Rectangle?(rectangle1), Color.White);
      rectangle1.Y = Game1.tileSize * 3;
      Game1.spriteBatch.Draw(Game1.menuTexture, new Vector2((float) (x + width + dialogueX - Game1.tileSize), (float) (y + height + num1 - Game1.tileSize + num2)), new Microsoft.Xna.Framework.Rectangle?(rectangle1), Color.White);
      rectangle1.X = 0;
      Game1.spriteBatch.Draw(Game1.menuTexture, new Vector2((float) (x + dialogueX), (float) (y + height + num1 - Game1.tileSize + num2)), new Microsoft.Xna.Framework.Rectangle?(rectangle1), Color.White);
      rectangle1.X = Game1.tileSize * 2;
      rectangle1.Y = 0;
      Game1.spriteBatch.Draw(Game1.menuTexture, new Microsoft.Xna.Framework.Rectangle(Game1.tileSize + x + dialogueX, y - Game1.tileSize * addedTileHeightForQuestions + num1 + num2, width - Game1.tileSize * 2, Game1.tileSize), new Microsoft.Xna.Framework.Rectangle?(rectangle1), Color.White);
      rectangle1.Y = 3 * Game1.tileSize;
      Game1.spriteBatch.Draw(Game1.menuTexture, new Microsoft.Xna.Framework.Rectangle(Game1.tileSize + x + dialogueX, y + height + num1 - Game1.tileSize + num2, width - Game1.tileSize * 2, Game1.tileSize), new Microsoft.Xna.Framework.Rectangle?(rectangle1), Color.White);
      rectangle1.Y = Game1.tileSize * 2;
      rectangle1.X = 0;
      Game1.spriteBatch.Draw(Game1.menuTexture, new Microsoft.Xna.Framework.Rectangle(x + dialogueX, y - Game1.tileSize * addedTileHeightForQuestions + num1 + Game1.tileSize + num2, Game1.tileSize, height - Game1.tileSize * 2 + addedTileHeightForQuestions * Game1.tileSize), new Microsoft.Xna.Framework.Rectangle?(rectangle1), Color.White);
      rectangle1.X = 3 * Game1.tileSize;
      Game1.spriteBatch.Draw(Game1.menuTexture, new Microsoft.Xna.Framework.Rectangle(x + width + dialogueX - Game1.tileSize, y - Game1.tileSize * addedTileHeightForQuestions + num1 + Game1.tileSize + num2, Game1.tileSize, height - Game1.tileSize * 2 + addedTileHeightForQuestions * Game1.tileSize), new Microsoft.Xna.Framework.Rectangle?(rectangle1), Color.White);
      if (objectDialogueWithPortrait && Game1.objectDialoguePortraitPerson != null || speaker && Game1.currentSpeaker != null && (Game1.currentSpeaker.CurrentDialogue.Count > 0 && Game1.currentSpeaker.CurrentDialogue.Peek().showPortrait))
      {
        Microsoft.Xna.Framework.Rectangle rectangle2 = new Microsoft.Xna.Framework.Rectangle(0, 0, 64, 64);
        NPC npc = objectDialogueWithPortrait ? Game1.objectDialoguePortraitPerson : Game1.currentSpeaker;
        string s = objectDialogueWithPortrait ? (Game1.objectDialoguePortraitPerson.name.Equals(Game1.player.spouse) ? "$l" : "$neutral") : npc.CurrentDialogue.Peek().CurrentEmotion;
        // ISSUE: reference to a compiler-generated method
        uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(s);
        if (stringHash <= 1488727062U)
        {
          if ((int) stringHash != 1186729920)
          {
            if ((int) stringHash != 1287395634)
            {
              if ((int) stringHash == 1488727062 && s == "$a")
              {
                rectangle2 = new Microsoft.Xna.Framework.Rectangle(64, 128, 64, 64);
                goto label_35;
              }
            }
            else if (s == "$u")
            {
              rectangle2 = new Microsoft.Xna.Framework.Rectangle(64, 64, 64, 64);
              goto label_35;
            }
          }
          else if (s == "$s")
          {
            rectangle2 = new Microsoft.Xna.Framework.Rectangle(0, 64, 64, 64);
            goto label_35;
          }
        }
        else
        {
          if (stringHash <= 1639725633U)
          {
            if ((int) stringHash != 1589392776)
            {
              if ((int) stringHash == 1639725633 && s == "$h")
              {
                rectangle2 = new Microsoft.Xna.Framework.Rectangle(64, 0, 64, 64);
                goto label_35;
              }
              else
                goto label_34;
            }
            else if (!(s == "$k"))
              goto label_34;
          }
          else if ((int) stringHash != 1706836109)
          {
            if ((int) stringHash != -1610031950 || !(s == "$neutral"))
              goto label_34;
          }
          else if (s == "$l")
          {
            rectangle2 = new Microsoft.Xna.Framework.Rectangle(0, 128, 64, 64);
            goto label_35;
          }
          else
            goto label_34;
          rectangle2 = new Microsoft.Xna.Framework.Rectangle(0, 0, 64, 64);
          goto label_35;
        }
label_34:
        rectangle2 = Game1.getSourceRectForStandardTileSheet(npc.Portrait, Convert.ToInt32(npc.CurrentDialogue.Peek().CurrentEmotion.Substring(1)), -1, -1);
label_35:
        Game1.spriteBatch.End();
        Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
        if (npc.Portrait != null)
        {
          Game1.spriteBatch.Draw(Game1.mouseCursors, new Vector2((float) (dialogueX + x + Game1.tileSize * 12), (float) (height1 - 5 * Game1.tileSize - Game1.tileSize * addedTileHeightForQuestions - 256 + num1 + Game1.tileSize / 4 - 60 + num2)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(333, 305, 80, 87)), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 0.98f);
          Game1.spriteBatch.Draw(npc.Portrait, new Vector2((float) (dialogueX + x + Game1.tileSize * 12 + 32), (float) (height1 - 5 * Game1.tileSize - Game1.tileSize * addedTileHeightForQuestions - 256 + num1 + Game1.tileSize / 4 - 60 + num2)), new Microsoft.Xna.Framework.Rectangle?(rectangle2), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 0.99f);
        }
        Game1.spriteBatch.End();
        Game1.spriteBatch.Begin();
        if (Game1.isQuestion)
          Game1.spriteBatch.DrawString(Game1.dialogueFont, npc.displayName, new Vector2((float) (Game1.tileSize * 14 + Game1.tileSize / 2) - Game1.dialogueFont.MeasureString(npc.displayName).X / 2f + (float) dialogueX + (float) x, (float) (height1 - 5 * Game1.tileSize - Game1.tileSize * addedTileHeightForQuestions) - Game1.dialogueFont.MeasureString(npc.displayName).Y + (float) num1 + (float) (Game1.tileSize / 3) + (float) num2) + new Vector2(2f, 2f), new Color(150, 150, 150));
        Game1.spriteBatch.DrawString(Game1.dialogueFont, npc.name.Equals("DwarfKing") ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3754") : (npc.name.Equals("Lewis") ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3756") : npc.displayName), new Vector2((float) (dialogueX + x + Game1.tileSize * 14 + Game1.tileSize / 2) - Game1.dialogueFont.MeasureString(npc.name.Equals("Lewis") ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3756") : npc.displayName).X / 2f, (float) (height1 - 5 * Game1.tileSize - Game1.tileSize * addedTileHeightForQuestions) - Game1.dialogueFont.MeasureString(npc.name.Equals("Lewis") ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3756") : npc.displayName).Y + (float) num1 + (float) (Game1.tileSize / 3) + (float) (Game1.tileSize / 8) + (float) num2), Game1.textColor);
      }
      if (drawOnlyBox || Game1.nameSelectUp && (!Game1.messagePause || Game1.currentObjectDialogue == null))
        return;
      string text = "";
      if (Game1.currentSpeaker != null && Game1.currentSpeaker.CurrentDialogue.Count > 0)
      {
        if (Game1.currentSpeaker.CurrentDialogue.Peek() == null || Game1.currentSpeaker.CurrentDialogue.Peek().getCurrentDialogue().Length < Game1.currentDialogueCharacterIndex - 1)
        {
          Game1.dialogueUp = false;
          Game1.currentDialogueCharacterIndex = 0;
          Game1.playSound("dialogueCharacterClose");
          Game1.player.forceCanMove();
          return;
        }
        text = Game1.currentSpeaker.CurrentDialogue.Peek().getCurrentDialogue().Substring(0, Game1.currentDialogueCharacterIndex);
      }
      else if (message != null)
        text = message;
      else if (Game1.currentObjectDialogue.Count > 0)
        text = Game1.currentObjectDialogue.Peek().Length <= 1 ? "" : Game1.currentObjectDialogue.Peek().Substring(0, Game1.currentDialogueCharacterIndex);
      Vector2 position = (double) Game1.dialogueFont.MeasureString(text).X <= (double) (width1 - Game1.tileSize * 4 - dialogueX) ? (Game1.currentSpeaker == null || Game1.currentSpeaker.CurrentDialogue.Count <= 0 ? (message == null ? (!Game1.isQuestion ? new Vector2((float) (width1 / 2) - Game1.dialogueFont.MeasureString(Game1.currentObjectDialogue.Count == 0 ? "" : Game1.currentObjectDialogue.Peek()).X / 2f + (float) dialogueX, (float) (y + Game1.pixelZoom + num2)) : new Vector2((float) (width1 / 2) - Game1.dialogueFont.MeasureString(Game1.currentObjectDialogue.Count == 0 ? "" : Game1.currentObjectDialogue.Peek()).X / 2f + (float) dialogueX, (float) (height1 - Game1.tileSize * addedTileHeightForQuestions - 4 * Game1.tileSize - (Game1.tileSize / 4 + (Game1.questionChoices.Count - 2) * Game1.tileSize) + num1 + num2))) : new Vector2((float) (width1 / 2) - Game1.dialogueFont.MeasureString(text).X / 2f + (float) dialogueX, (float) (y + Game1.tileSize * 3 / 2 + Game1.pixelZoom))) : new Vector2((float) (width1 / 2) - Game1.dialogueFont.MeasureString(Game1.currentSpeaker.CurrentDialogue.Peek().getCurrentDialogue()).X / 2f + (float) dialogueX, (float) (height1 - Game1.tileSize * addedTileHeightForQuestions - 4 * Game1.tileSize - Game1.tileSize / 4 + num1 + num2))) : new Vector2((float) (Game1.tileSize * 2 + dialogueX), (float) (height1 - Game1.tileSize * addedTileHeightForQuestions - 4 * Game1.tileSize - Game1.tileSize / 4 + num1 + num2));
      if (!drawOnlyBox)
      {
        Game1.spriteBatch.DrawString(Game1.dialogueFont, text, position + new Vector2(3f, 0.0f), Game1.textShadowColor);
        Game1.spriteBatch.DrawString(Game1.dialogueFont, text, position + new Vector2(3f, 3f), Game1.textShadowColor);
        Game1.spriteBatch.DrawString(Game1.dialogueFont, text, position + new Vector2(0.0f, 3f), Game1.textShadowColor);
        Game1.spriteBatch.DrawString(Game1.dialogueFont, text, position, Game1.textColor);
      }
      if ((double) Game1.dialogueFont.MeasureString(text).Y <= (double) Game1.tileSize)
        num1 += Game1.tileSize;
      if (Game1.isQuestion && !Game1.dialogueTyping)
      {
        for (int index = 0; index < Game1.questionChoices.Count; ++index)
        {
          if (Game1.currentQuestionChoice == index)
          {
            position.X = (float) (Game1.tileSize * 5 / 4 + dialogueX + x);
            position.Y = (float) (height1 - (5 + addedTileHeightForQuestions + 1) * Game1.tileSize) + (text.Trim().Length > 0 ? Game1.dialogueFont.MeasureString(text).Y : 0.0f) + (float) (Game1.tileSize * 2) + (float) ((Game1.tileSize / 2 + Game1.tileSize / 4) * index) - (float) (Game1.tileSize / 4 + (Game1.questionChoices.Count - 2) * Game1.tileSize) + (float) num1 + (float) num2;
            Game1.spriteBatch.End();
            Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
            Game1.spriteBatch.Draw(Game1.objectSpriteSheet, position + new Vector2((float) Math.Cos((double) Game1.currentGameTime.TotalGameTime.Milliseconds * Math.PI / 512.0) * 3f, 0.0f), new Microsoft.Xna.Framework.Rectangle?(Game1.currentLocation.getSourceRectForObject(26)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1f);
            Game1.spriteBatch.End();
            Game1.spriteBatch.Begin();
            position.X = (float) (Game1.tileSize * 5 / 2 + dialogueX + x);
            position.Y = (float) (height1 - (5 + addedTileHeightForQuestions + 1) * Game1.tileSize) + (text.Trim().Length > 1 ? Game1.dialogueFont.MeasureString(text).Y : 0.0f) + (float) (Game1.tileSize * 3 / 2 + Game1.tileSize / 2) - (float) ((Game1.questionChoices.Count - 2) * Game1.tileSize) + (float) ((Game1.tileSize / 2 + Game1.tileSize / 4) * index) + (float) num1 + (float) num2;
            Game1.spriteBatch.DrawString(Game1.dialogueFont, Game1.questionChoices[index].responseText, position, Game1.textColor);
          }
          else
          {
            position.X = (float) (Game1.tileSize * 2 + dialogueX + x);
            position.Y = (float) (height1 - (5 + addedTileHeightForQuestions + 1) * Game1.tileSize) + (text.Trim().Length > 1 ? Game1.dialogueFont.MeasureString(text).Y : 0.0f) + (float) (Game1.tileSize * 3 / 2 + Game1.tileSize / 2) - (float) ((Game1.questionChoices.Count - 2) * Game1.tileSize) + (float) ((Game1.tileSize / 2 + Game1.tileSize / 4) * index) + (float) num1 + (float) num2;
            Game1.spriteBatch.DrawString(Game1.dialogueFont, Game1.questionChoices[index].responseText, position, Game1.unselectedOptionColor);
          }
        }
      }
      else if (Game1.numberOfSelectedItems != -1 && !Game1.dialogueTyping)
        Game1.drawItemSelectDialogue(x, y, dialogueX, num1 + num2, height1, addedTileHeightForQuestions, text);
      if (drawOnlyBox || Game1.dialogueTyping || message != null)
        return;
      Game1.spriteBatch.Draw(Game1.mouseCursors, new Vector2((float) (x + dialogueX + width - Game1.tileSize * 3 / 2), (float) (y + height + num1 + num2 - Game1.tileSize * 3 / 2) - Game1.dialogueButtonScale), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, Game1.dialogueButtonShrinking || (double) Game1.dialogueButtonScale >= (double) (Game1.tileSize / 8) ? 2 : 3, -1, -1)), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9999999f);
    }

    private static void drawItemSelectDialogue(int x, int y, int dialogueX, int dialogueY, int screenHeight, int addedTileHeightForQuestions, string text)
    {
      string selectedItemsType = Game1.selectedItemsType;
      string text1;
      if (!(selectedItemsType == "flutePitch") && !(selectedItemsType == "drumTome"))
      {
        if (selectedItemsType == "jukebox")
          text1 = "@ " + Game1.player.songsHeard.ElementAt<string>(Game1.numberOfSelectedItems) + " >  ";
        else
          text1 = "@ " + (object) Game1.numberOfSelectedItems + " >  " + (object) (Game1.priceOfSelectedItem * Game1.numberOfSelectedItems) + "g";
      }
      else
        text1 = "@ " + (object) Game1.numberOfSelectedItems + " >  ";
      if (Game1.currentLocation.Name.Equals("Club"))
        text1 = "@ " + (object) Game1.numberOfSelectedItems + " >  ";
      Game1.spriteBatch.DrawString(Game1.dialogueFont, text1, new Vector2((float) (dialogueX + x + Game1.tileSize), (float) (screenHeight - (5 + addedTileHeightForQuestions + 1) * Game1.tileSize) + Game1.dialogueFont.MeasureString(text).Y + (float) (Game1.tileSize * 3 / 2 + Game1.tileSize / 8) + (float) dialogueY), Game1.textColor);
    }

    private void drawFarmBuildings()
    {
      if (Game1.player.CoopUpgradeLevel > 0)
        Game1.spriteBatch.Draw(Game1.currentCoopTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (20 * Game1.tileSize), (float) (5 * Game1.tileSize))), new Microsoft.Xna.Framework.Rectangle?(Game1.currentCoopTexture.Bounds), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, Math.Max(0.0f, (float) (9 * Game1.tileSize) / 10000f));
      switch (Game1.player.BarnUpgradeLevel)
      {
        case 1:
          Game1.spriteBatch.Draw(Game1.currentBarnTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (12 * Game1.tileSize), (float) (5 * Game1.tileSize))), new Microsoft.Xna.Framework.Rectangle?(Game1.currentBarnTexture.Bounds), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, Math.Max(0.0f, (float) (9 * Game1.tileSize) / 10000f));
          break;
        case 2:
          Game1.spriteBatch.Draw(Game1.currentBarnTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (10 * Game1.tileSize), (float) (4 * Game1.tileSize))), new Microsoft.Xna.Framework.Rectangle?(Game1.currentBarnTexture.Bounds), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, Math.Max(0.0f, (float) (9 * Game1.tileSize) / 10000f));
          break;
      }
      if (!Game1.player.hasGreenhouse)
        return;
      Game1.spriteBatch.Draw(Game1.greenhouseTexture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) Game1.tileSize, (float) (5 * Game1.tileSize))), new Microsoft.Xna.Framework.Rectangle?(Game1.greenhouseTexture.Bounds), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, Math.Max(0.0f, (float) (9 * Game1.tileSize) / 10000f));
    }

    public static void drawPlayerHeldObject(Farmer f)
    {
      if (Game1.eventUp && (Game1.currentLocation.currentEvent == null || !Game1.currentLocation.currentEvent.showActiveObject) || (f.FarmerSprite.pauseForSingleAnimation || f.isRidingHorse() || f.bathingClothes))
        return;
      float num1 = f.getLocalPosition(Game1.viewport).X + ((double) f.rotation < 0.0 ? -8f : ((double) f.rotation > 0.0 ? 8f : 0.0f)) + (float) (f.FarmerSprite.CurrentAnimationFrame.xOffset * Game1.pixelZoom);
      float num2 = f.getLocalPosition(Game1.viewport).Y - (float) (Game1.tileSize * 2) + (float) (f.FarmerSprite.CurrentAnimationFrame.positionOffset * 4) + (float) (FarmerRenderer.featureYOffsetPerFrame[f.FarmerSprite.CurrentAnimationFrame.frame] * 4);
      if (f.ActiveObject.bigCraftable)
        num2 -= (float) Game1.tileSize;
      if (Game1.isEating)
      {
        num1 = f.getLocalPosition(Game1.viewport).X - (float) (Game1.tileSize / 3);
        num2 = f.getLocalPosition(Game1.viewport).Y - (float) (Game1.tileSize * 2) + (float) (Game1.pixelZoom * 3);
      }
      if (Game1.isEating && (!Game1.isEating || f.Sprite.CurrentFrame > 218))
        return;
      f.ActiveObject.drawWhenHeld(Game1.spriteBatch, new Vector2((float) (int) num1, (float) (int) num2), f);
    }

    public static void drawTool(Farmer f)
    {
      Game1.drawTool(f, f.CurrentTool.currentParentTileIndex);
    }

    public static void drawTool(Farmer f, int currentToolIndex)
    {
      Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle(currentToolIndex * (Game1.tileSize / 4) % Game1.toolSpriteSheet.Width, currentToolIndex * (Game1.tileSize / 4) / Game1.toolSpriteSheet.Width * (Game1.tileSize / 4), Game1.tileSize / 4, Game1.tileSize / 2);
      Vector2 playerPosition = f.getLocalPosition(Game1.viewport) + f.jitter + f.armOffset;
      if (Game1.pickingTool)
      {
        int num = (int) playerPosition.Y - Game1.tileSize * 2;
        Game1.spriteBatch.Draw(Game1.toolSpriteSheet, new Vector2(playerPosition.X, (float) num), new Microsoft.Xna.Framework.Rectangle?(rectangle), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, Math.Max(0.0f, (float) (f.getStandingY() + Game1.tileSize / 2) / 10000f));
      }
      else if (f.CurrentTool is MeleeWeapon)
        ((MeleeWeapon) f.CurrentTool).drawDuringUse(((FarmerSprite) f.Sprite).animatingBackwards ? 5 - ((FarmerSprite) f.Sprite).indexInCurrentAnimation : ((FarmerSprite) f.Sprite).indexInCurrentAnimation, f.FacingDirection, Game1.spriteBatch, playerPosition, f);
      else if (f.FarmerSprite.isUsingWeapon())
      {
        MeleeWeapon.drawDuringUse(((FarmerSprite) f.Sprite).indexInCurrentAnimation, f.FacingDirection, Game1.spriteBatch, playerPosition, f, MeleeWeapon.getSourceRect(f.FarmerSprite.CurrentToolIndex), f.FarmerSprite.getWeaponTypeFromAnimation(), false);
      }
      else
      {
        if (f.CurrentTool is FishingRod)
        {
          if ((f.CurrentTool as FishingRod).fishCaught || (f.CurrentTool as FishingRod).showingTreasure)
          {
            f.CurrentTool.draw(Game1.spriteBatch);
            return;
          }
          rectangle = new Microsoft.Xna.Framework.Rectangle(((FarmerSprite) f.Sprite).indexInCurrentAnimation * 48, 288, 48, 48);
          if (f.FacingDirection == 2 || f.FacingDirection == 0)
            rectangle.Y += 48;
          else if ((f.CurrentTool as FishingRod).isFishing && (!(f.CurrentTool as FishingRod).isReeling || (f.CurrentTool as FishingRod).hit))
            playerPosition.Y += (float) (Game1.pixelZoom * 2);
          if ((f.CurrentTool as FishingRod).isFishing)
            rectangle.X += (5 - ((FarmerSprite) f.Sprite).indexInCurrentAnimation) * 48;
          if ((f.CurrentTool as FishingRod).isReeling)
          {
            if (f.FacingDirection == 2 || f.FacingDirection == 0)
            {
              rectangle.X = 288;
              if (Game1.didPlayerJustClickAtAll())
                rectangle.X = 0;
            }
            else
            {
              rectangle.X = 288;
              rectangle.Y = 240;
              if (Game1.didPlayerJustClickAtAll())
                rectangle.Y += 48;
            }
          }
          if (f.FarmerSprite.CurrentFrame == 57)
            rectangle.Height = 0;
          if (f.FacingDirection == 0)
            playerPosition.X += (float) (Game1.tileSize / 4);
        }
        if (f.CurrentTool != null)
          f.CurrentTool.draw(Game1.spriteBatch);
        if (f.CurrentTool is Slingshot || f.CurrentTool is Shears || (f.CurrentTool is MilkPail || f.CurrentTool is Pan))
          return;
        int num1 = 0;
        int num2 = 0;
        if (f.CurrentTool is WateringCan)
        {
          num1 += Game1.tileSize + Game1.tileSize / 4;
          num2 = f.FacingDirection == 1 ? Game1.tileSize / 2 : (f.FacingDirection == 3 ? -Game1.tileSize / 2 : 0);
          if (((FarmerSprite) f.Sprite).indexInCurrentAnimation == 0 || ((FarmerSprite) f.Sprite).indexInCurrentAnimation == 1)
            num2 = num2 * 3 / 2;
        }
        Microsoft.Xna.Framework.Rectangle boundingBox;
        if (f.FacingDirection == 1)
        {
          int num3 = 0;
          if (((FarmerSprite) f.Sprite).indexInCurrentAnimation > 2)
          {
            Point tileLocationPoint = f.getTileLocationPoint();
            ++tileLocationPoint.X;
            --tileLocationPoint.Y;
            if (!(f.CurrentTool is WateringCan) && f.currentLocation.getTileIndexAt(tileLocationPoint, "Front") != -1)
              return;
            ++tileLocationPoint.Y;
            if (f.currentLocation.getTileIndexAt(tileLocationPoint, "Front") == -1)
              num3 += Game1.tileSize / 4;
          }
          else if (f.CurrentTool is WateringCan && ((FarmerSprite) f.Sprite).indexInCurrentAnimation == 1)
          {
            Point tileLocationPoint = f.getTileLocationPoint();
            --tileLocationPoint.X;
            --tileLocationPoint.Y;
            if (f.currentLocation.getTileIndexAt(tileLocationPoint, "Front") != -1 && (double) f.position.Y % (double) Game1.tileSize < (double) (Game1.tileSize / 2))
              return;
          }
          if (f.CurrentTool != null && f.CurrentTool is FishingRod)
          {
            Color color = (f.CurrentTool as FishingRod).getColor();
            switch (((FarmerSprite) f.Sprite).indexInCurrentAnimation)
            {
              case 0:
                if ((f.CurrentTool as FishingRod).isReeling)
                {
                  Game1.spriteBatch.Draw(Game1.toolSpriteSheet, new Vector2(playerPosition.X - (float) Game1.tileSize + (float) num2, playerPosition.Y - (float) (Game1.tileSize * 5 / 2) + (float) num1), new Microsoft.Xna.Framework.Rectangle?(rectangle), color, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0.0f, (float) (f.getStandingY() + Game1.tileSize) / 10000f));
                  break;
                }
                if ((f.CurrentTool as FishingRod).isFishing || (f.CurrentTool as FishingRod).doneWithAnimation)
                {
                  Game1.spriteBatch.Draw(Game1.toolSpriteSheet, new Vector2(playerPosition.X - (float) Game1.tileSize + (float) num2, playerPosition.Y - (float) (Game1.tileSize * 5 / 2) + (float) num1), new Microsoft.Xna.Framework.Rectangle?(rectangle), color, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0.0f, (float) (f.getStandingY() + Game1.tileSize) / 10000f));
                  break;
                }
                if (!(f.CurrentTool as FishingRod).hasDoneFucntionYet || (f.CurrentTool as FishingRod).pullingOutOfWater)
                {
                  Game1.spriteBatch.Draw(Game1.toolSpriteSheet, new Vector2(playerPosition.X - (float) Game1.tileSize + (float) num2, playerPosition.Y - (float) (Game1.tileSize * 5 / 2) + (float) num1), new Microsoft.Xna.Framework.Rectangle?(rectangle), color, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0.0f, (float) (f.getStandingY() + Game1.tileSize) / 10000f));
                  break;
                }
                break;
              case 1:
                Game1.spriteBatch.Draw(Game1.toolSpriteSheet, new Vector2(playerPosition.X - (float) Game1.tileSize + (float) num2, (float) ((double) playerPosition.Y - (double) (Game1.tileSize * 5 / 2) + 8.0) + (float) num1), new Microsoft.Xna.Framework.Rectangle?(rectangle), color, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0.0f, (float) (f.getStandingY() + Game1.tileSize) / 10000f));
                break;
              case 2:
                Game1.spriteBatch.Draw(Game1.toolSpriteSheet, new Vector2((float) ((double) playerPosition.X - (double) (Game1.tileSize * 3 / 2) + 32.0) + (float) num2, (float) ((double) playerPosition.Y - (double) (Game1.tileSize * 2) - 24.0) + (float) num1), new Microsoft.Xna.Framework.Rectangle?(rectangle), color, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0.0f, (float) (f.getStandingY() + Game1.tileSize) / 10000f));
                break;
              case 3:
                Game1.spriteBatch.Draw(Game1.toolSpriteSheet, new Vector2((float) ((double) playerPosition.X - (double) (Game1.tileSize * 3 / 2) + 24.0) + (float) num2, (float) ((double) playerPosition.Y - (double) (Game1.tileSize * 2) - 32.0) + (float) num1), new Microsoft.Xna.Framework.Rectangle?(rectangle), color, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0.0f, (float) (f.getStandingY() + Game1.tileSize) / 10000f));
                break;
              case 4:
                if ((f.CurrentTool as FishingRod).isFishing || (f.CurrentTool as FishingRod).doneWithAnimation)
                {
                  Game1.spriteBatch.Draw(Game1.toolSpriteSheet, new Vector2(playerPosition.X - (float) Game1.tileSize + (float) num2, playerPosition.Y - (float) (Game1.tileSize * 5 / 2) + (float) num1), new Microsoft.Xna.Framework.Rectangle?(rectangle), color, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0.0f, (float) (f.getStandingY() + Game1.tileSize) / 10000f));
                  break;
                }
                Game1.spriteBatch.Draw(Game1.toolSpriteSheet, new Vector2(playerPosition.X - (float) Game1.tileSize + (float) num2, (float) ((double) playerPosition.Y - (double) (Game1.tileSize * 5 / 2) + 4.0) + (float) num1), new Microsoft.Xna.Framework.Rectangle?(rectangle), color, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0.0f, (float) (f.getStandingY() + Game1.tileSize) / 10000f));
                break;
              case 5:
                Game1.spriteBatch.Draw(Game1.toolSpriteSheet, new Vector2(playerPosition.X - (float) Game1.tileSize + (float) num2, playerPosition.Y - (float) (Game1.tileSize * 5 / 2) + (float) num1), new Microsoft.Xna.Framework.Rectangle?(rectangle), color, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0.0f, (float) (f.getStandingY() + Game1.tileSize) / 10000f));
                break;
            }
          }
          else if (f.CurrentTool != null && f.CurrentTool.Name.Contains("Sword"))
          {
            switch (((FarmerSprite) f.Sprite).indexInCurrentAnimation)
            {
              case 0:
                Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2((float) ((double) playerPosition.X + (double) Game1.tileSize - 20.0), playerPosition.Y + 28f)), new Microsoft.Xna.Framework.Rectangle?(rectangle), Color.White, -0.3926991f, new Vector2(4f, (float) (Game1.tileSize - 4)), 1f, SpriteEffects.None, Math.Max(0.0f, (float) (f.getStandingY() + Game1.tileSize) / 10000f));
                break;
              case 1:
                Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2((float) ((double) playerPosition.X + (double) Game1.tileSize - 12.0), (float) ((double) playerPosition.Y + (double) Game1.tileSize - 8.0))), new Microsoft.Xna.Framework.Rectangle?(rectangle), Color.White, 0.0f, new Vector2(4f, (float) (Game1.tileSize - 4)), 1f, SpriteEffects.None, Math.Max(0.0f, (float) (f.getStandingY() + Game1.tileSize) / 10000f));
                break;
              case 2:
                Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2((float) ((double) playerPosition.X + (double) Game1.tileSize - 12.0), (float) ((double) playerPosition.Y + (double) Game1.tileSize - 4.0))), new Microsoft.Xna.Framework.Rectangle?(rectangle), Color.White, 0.3926991f, new Vector2(4f, (float) (Game1.tileSize - 4)), 1f, SpriteEffects.None, Math.Max(0.0f, (float) (f.getStandingY() + Game1.tileSize) / 10000f));
                break;
              case 3:
                Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2((float) ((double) playerPosition.X + (double) Game1.tileSize - 12.0), playerPosition.Y + (float) Game1.tileSize)), new Microsoft.Xna.Framework.Rectangle?(rectangle), Color.White, 0.7853981f, new Vector2(4f, (float) (Game1.tileSize - 4)), 1f, SpriteEffects.None, Math.Max(0.0f, (float) (f.getStandingY() + Game1.tileSize) / 10000f));
                break;
              case 4:
                Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2((float) ((double) playerPosition.X + (double) Game1.tileSize - 16.0), (float) ((double) playerPosition.Y + (double) Game1.tileSize + 4.0))), new Microsoft.Xna.Framework.Rectangle?(rectangle), Color.White, 3f * (float) Math.PI / 8f, new Vector2(4f, (float) (Game1.tileSize - 4)), 1f, SpriteEffects.None, Math.Max(0.0f, (float) (f.getStandingY() + Game1.tileSize) / 10000f));
                break;
              case 5:
                Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2((float) ((double) playerPosition.X + (double) Game1.tileSize - 16.0), (float) ((double) playerPosition.Y + (double) Game1.tileSize + 8.0))), new Microsoft.Xna.Framework.Rectangle?(rectangle), Color.White, 1.570796f, new Vector2(4f, (float) (Game1.tileSize - 4)), 1f, SpriteEffects.None, Math.Max(0.0f, (float) (f.getStandingY() + Game1.tileSize) / 10000f));
                break;
              case 6:
                Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2((float) ((double) playerPosition.X + (double) Game1.tileSize - 16.0), (float) ((double) playerPosition.Y + (double) Game1.tileSize + 12.0))), new Microsoft.Xna.Framework.Rectangle?(rectangle), Color.White, 1.963495f, new Vector2(4f, (float) (Game1.tileSize - 4)), 1f, SpriteEffects.None, Math.Max(0.0f, (float) (f.getStandingY() + Game1.tileSize) / 10000f));
                break;
              case 7:
                Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2((float) ((double) playerPosition.X + (double) Game1.tileSize - 16.0), (float) ((double) playerPosition.Y + (double) Game1.tileSize + 12.0))), new Microsoft.Xna.Framework.Rectangle?(rectangle), Color.White, 1.963495f, new Vector2(4f, (float) (Game1.tileSize - 4)), 1f, SpriteEffects.None, Math.Max(0.0f, (float) (f.getStandingY() + Game1.tileSize) / 10000f));
                break;
            }
          }
          else if (f.CurrentTool != null && f.CurrentTool is WateringCan)
          {
            switch (((FarmerSprite) f.Sprite).indexInCurrentAnimation)
            {
              case 0:
              case 1:
                Game1.spriteBatch.Draw(Game1.toolSpriteSheet, new Vector2((float) (int) ((double) playerPosition.X + (double) num2 - 4.0), (float) (int) ((double) playerPosition.Y - (double) (Game1.tileSize * 2) + 8.0 + (double) num1)), new Microsoft.Xna.Framework.Rectangle?(rectangle), Color.White, 0.0f, new Vector2(0.0f, (float) Game1.tileSize / 4f), 4f, SpriteEffects.None, Math.Max(0.0f, (float) (f.GetBoundingBox().Bottom + num3) / 10000f));
                break;
              case 2:
                Game1.spriteBatch.Draw(Game1.toolSpriteSheet, new Vector2((float) ((int) playerPosition.X + num2 + 24), (float) (int) ((double) playerPosition.Y - (double) (Game1.tileSize * 2) - 8.0 + (double) num1)), new Microsoft.Xna.Framework.Rectangle?(rectangle), Color.White, 0.2617994f, new Vector2(0.0f, (float) Game1.tileSize / 4f), 4f, SpriteEffects.None, Math.Max(0.0f, (float) (f.GetBoundingBox().Bottom + num3) / 10000f));
                break;
              case 3:
                rectangle.X += Game1.tileSize / 4;
                SpriteBatch spriteBatch1 = Game1.spriteBatch;
                Texture2D toolSpriteSheet1 = Game1.toolSpriteSheet;
                Vector2 position1 = new Vector2((float) (int) ((double) playerPosition.X + (double) num2 + 8.0), (float) (int) ((double) playerPosition.Y - (double) (Game1.tileSize * 2) - 24.0 + (double) num1));
                Microsoft.Xna.Framework.Rectangle? sourceRectangle1 = new Microsoft.Xna.Framework.Rectangle?(rectangle);
                Color white1 = Color.White;
                double num4 = 0.0;
                Vector2 origin1 = new Vector2(0.0f, (float) Game1.tileSize / 4f);
                double num5 = 4.0;
                int num6 = 0;
                double num7 = 0.0;
                boundingBox = f.GetBoundingBox();
                double num8 = (double) (boundingBox.Bottom + num3) / 10000.0;
                double num9 = (double) Math.Max((float) num7, (float) num8);
                spriteBatch1.Draw(toolSpriteSheet1, position1, sourceRectangle1, white1, (float) num4, origin1, (float) num5, (SpriteEffects) num6, (float) num9);
                break;
            }
          }
          else
          {
            switch (((FarmerSprite) f.Sprite).indexInCurrentAnimation)
            {
              case 0:
                Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2((float) ((double) playerPosition.X - (double) (Game1.tileSize / 2) - 4.0) + (float) num2 - (float) Math.Min(8, f.toolPower * 4), (float) ((double) playerPosition.Y - (double) (Game1.tileSize * 2) + 24.0) + (float) num1 + (float) Math.Min(8, f.toolPower * 4))), new Microsoft.Xna.Framework.Rectangle?(rectangle), Color.White, (float) (-0.261799395084381 - (double) Math.Min(f.toolPower, 2) * 0.0490873865783215), new Vector2(0.0f, (float) Game1.tileSize / 4f), 4f, SpriteEffects.None, Math.Max(0.0f, (float) (f.GetBoundingBox().Bottom + num3) / 10000f));
                break;
              case 1:
                Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2((float) ((double) playerPosition.X + (double) (Game1.tileSize / 2) - 24.0) + (float) num2, playerPosition.Y - 124f + (float) num1 + (float) Game1.tileSize)), new Microsoft.Xna.Framework.Rectangle?(rectangle), Color.White, 0.2617994f, new Vector2(0.0f, (float) (Game1.tileSize * 2) / 4f), 4f, SpriteEffects.None, Math.Max(0.0f, (float) (f.GetBoundingBox().Bottom + num3) / 10000f));
                break;
              case 2:
                Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2((float) ((double) playerPosition.X + (double) (Game1.tileSize / 2) + (double) num2 - 4.0), playerPosition.Y - 132f + (float) num1 + (float) Game1.tileSize)), new Microsoft.Xna.Framework.Rectangle?(rectangle), Color.White, 0.7853982f, new Vector2(0.0f, (float) (Game1.tileSize * 2) / 4f), 4f, SpriteEffects.None, Math.Max(0.0f, (float) (f.GetBoundingBox().Bottom + num3) / 10000f));
                break;
              case 3:
                Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2((float) ((double) playerPosition.X + (double) (Game1.tileSize / 2) + 28.0) + (float) num2, playerPosition.Y - (float) Game1.tileSize + (float) num1)), new Microsoft.Xna.Framework.Rectangle?(rectangle), Color.White, 1.832596f, new Vector2(0.0f, (float) (Game1.tileSize * 2) / 4f), 4f, SpriteEffects.None, Math.Max(0.0f, (float) (f.GetBoundingBox().Bottom + num3) / 10000f));
                break;
              case 4:
                Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2((float) ((double) playerPosition.X + (double) (Game1.tileSize / 2) + 28.0) + (float) num2, (float) ((double) playerPosition.Y - (double) Game1.tileSize + 4.0) + (float) num1)), new Microsoft.Xna.Framework.Rectangle?(rectangle), Color.White, 1.832596f, new Vector2(0.0f, (float) (Game1.tileSize * 2) / 4f), 4f, SpriteEffects.None, Math.Max(0.0f, (float) (f.GetBoundingBox().Bottom + num3) / 10000f));
                break;
              case 5:
                SpriteBatch spriteBatch2 = Game1.spriteBatch;
                Texture2D toolSpriteSheet2 = Game1.toolSpriteSheet;
                Vector2 position2 = Utility.snapToInt(new Vector2((float) ((double) playerPosition.X + (double) Game1.tileSize + 12.0) + (float) num2, (float) ((double) playerPosition.Y - (double) (Game1.tileSize * 2) + 32.0) + (float) num1 + (float) (Game1.tileSize * 2)));
                Microsoft.Xna.Framework.Rectangle? sourceRectangle2 = new Microsoft.Xna.Framework.Rectangle?(rectangle);
                Color white2 = Color.White;
                double num10 = 0.785398185253143;
                Vector2 origin2 = new Vector2(0.0f, (float) (Game1.tileSize * 2) / 4f);
                double num11 = 4.0;
                int num12 = 0;
                double num13 = 0.0;
                boundingBox = f.GetBoundingBox();
                double num14 = (double) (boundingBox.Bottom + num3) / 10000.0;
                double num15 = (double) Math.Max((float) num13, (float) num14);
                spriteBatch2.Draw(toolSpriteSheet2, position2, sourceRectangle2, white2, (float) num10, origin2, (float) num11, (SpriteEffects) num12, (float) num15);
                break;
              case 6:
                SpriteBatch spriteBatch3 = Game1.spriteBatch;
                Texture2D toolSpriteSheet3 = Game1.toolSpriteSheet;
                Vector2 position3 = Utility.snapToInt(new Vector2((float) ((double) playerPosition.X + (double) (Game1.tileSize * 2 / 3) + 8.0) + (float) num2, (float) ((double) playerPosition.Y - (double) Game1.tileSize + 24.0) + (float) num1 + (float) (Game1.tileSize * 2)));
                Microsoft.Xna.Framework.Rectangle? sourceRectangle3 = new Microsoft.Xna.Framework.Rectangle?(rectangle);
                Color white3 = Color.White;
                double num16 = 0.0;
                Vector2 origin3 = new Vector2(0.0f, (float) (Game1.tileSize * 2));
                double num17 = 4.0;
                int num18 = 0;
                double num19 = 0.0;
                boundingBox = f.GetBoundingBox();
                double num20 = (double) (boundingBox.Bottom + num3) / 10000.0;
                double num21 = (double) Math.Max((float) num19, (float) num20);
                spriteBatch3.Draw(toolSpriteSheet3, position3, sourceRectangle3, white3, (float) num16, origin3, (float) num17, (SpriteEffects) num18, (float) num21);
                break;
            }
          }
        }
        else if (f.FacingDirection == 3)
        {
          int num3 = 0;
          if (((FarmerSprite) f.Sprite).indexInCurrentAnimation > 2)
          {
            Point tileLocationPoint = f.getTileLocationPoint();
            --tileLocationPoint.X;
            --tileLocationPoint.Y;
            if (!(f.CurrentTool is WateringCan) && f.currentLocation.getTileIndexAt(tileLocationPoint, "Front") != -1 && (double) f.position.Y % (double) Game1.tileSize < (double) (Game1.tileSize / 2))
              return;
            ++tileLocationPoint.Y;
            if (f.currentLocation.getTileIndexAt(tileLocationPoint, "Front") == -1)
              num3 += Game1.tileSize / 4;
          }
          else if (f.CurrentTool is WateringCan && ((FarmerSprite) f.Sprite).indexInCurrentAnimation == 1)
          {
            Point tileLocationPoint = f.getTileLocationPoint();
            --tileLocationPoint.X;
            --tileLocationPoint.Y;
            if (f.currentLocation.getTileIndexAt(tileLocationPoint, "Front") != -1 && (double) f.position.Y % (double) Game1.tileSize < (double) (Game1.tileSize / 2))
              return;
          }
          if (f.CurrentTool != null && f.CurrentTool is FishingRod || currentToolIndex >= 48 && currentToolIndex <= 55)
          {
            Color color = (f.CurrentTool as FishingRod).getColor();
            switch (((FarmerSprite) f.Sprite).indexInCurrentAnimation)
            {
              case 0:
                if ((f.CurrentTool as FishingRod).isReeling)
                {
                  Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(playerPosition.X - (float) Game1.tileSize + (float) num2, playerPosition.Y - (float) (Game1.tileSize * 5 / 2) + (float) num1)), new Microsoft.Xna.Framework.Rectangle?(rectangle), color, 0.0f, Vector2.Zero, 4f, SpriteEffects.FlipHorizontally, Math.Max(0.0f, (float) (f.getStandingY() + Game1.tileSize) / 10000f));
                  break;
                }
                if ((f.CurrentTool as FishingRod).isFishing || (f.CurrentTool as FishingRod).doneWithAnimation)
                {
                  Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(playerPosition.X - (float) Game1.tileSize + (float) num2, playerPosition.Y - (float) (Game1.tileSize * 5 / 2) + (float) num1)), new Microsoft.Xna.Framework.Rectangle?(rectangle), color, 0.0f, Vector2.Zero, 4f, SpriteEffects.FlipHorizontally, Math.Max(0.0f, (float) (f.getStandingY() + Game1.tileSize) / 10000f));
                  break;
                }
                if (!(f.CurrentTool as FishingRod).hasDoneFucntionYet || (f.CurrentTool as FishingRod).pullingOutOfWater)
                {
                  Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(playerPosition.X - (float) Game1.tileSize + (float) num2, playerPosition.Y - (float) (Game1.tileSize * 5 / 2) + (float) num1)), new Microsoft.Xna.Framework.Rectangle?(rectangle), color, 0.0f, Vector2.Zero, 4f, SpriteEffects.FlipHorizontally, Math.Max(0.0f, (float) (f.getStandingY() + Game1.tileSize) / 10000f));
                  break;
                }
                break;
              case 1:
                Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(playerPosition.X - (float) Game1.tileSize + (float) num2, (float) ((double) playerPosition.Y - (double) (Game1.tileSize * 5 / 2) + 8.0) + (float) num1)), new Microsoft.Xna.Framework.Rectangle?(rectangle), color, 0.0f, Vector2.Zero, 4f, SpriteEffects.FlipHorizontally, Math.Max(0.0f, (float) (f.getStandingY() + Game1.tileSize) / 10000f));
                break;
              case 2:
                Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2((float) ((double) playerPosition.X - (double) (Game1.tileSize * 3 / 2) + 32.0) + (float) num2, (float) ((double) playerPosition.Y - (double) (Game1.tileSize * 2) - 24.0) + (float) num1)), new Microsoft.Xna.Framework.Rectangle?(rectangle), color, 0.0f, Vector2.Zero, 4f, SpriteEffects.FlipHorizontally, Math.Max(0.0f, (float) (f.getStandingY() + Game1.tileSize) / 10000f));
                break;
              case 3:
                Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2((float) ((double) playerPosition.X - (double) (Game1.tileSize * 3 / 2) + 24.0) + (float) num2, (float) ((double) playerPosition.Y - (double) (Game1.tileSize * 2) - 32.0) + (float) num1)), new Microsoft.Xna.Framework.Rectangle?(rectangle), color, 0.0f, Vector2.Zero, 4f, SpriteEffects.FlipHorizontally, Math.Max(0.0f, (float) (f.getStandingY() + Game1.tileSize) / 10000f));
                break;
              case 4:
                if ((f.CurrentTool as FishingRod).isFishing || (f.CurrentTool as FishingRod).doneWithAnimation)
                {
                  Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(playerPosition.X - (float) Game1.tileSize + (float) num2, playerPosition.Y - (float) (Game1.tileSize * 5 / 2) + (float) num1)), new Microsoft.Xna.Framework.Rectangle?(rectangle), color, 0.0f, Vector2.Zero, 4f, SpriteEffects.FlipHorizontally, Math.Max(0.0f, (float) (f.getStandingY() + Game1.tileSize) / 10000f));
                  break;
                }
                Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(playerPosition.X - (float) Game1.tileSize + (float) num2, (float) ((double) playerPosition.Y - (double) (Game1.tileSize * 5 / 2) + 4.0) + (float) num1)), new Microsoft.Xna.Framework.Rectangle?(rectangle), color, 0.0f, Vector2.Zero, 4f, SpriteEffects.FlipHorizontally, Math.Max(0.0f, (float) (f.getStandingY() + Game1.tileSize) / 10000f));
                break;
              case 5:
                Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(playerPosition.X - (float) Game1.tileSize + (float) num2, playerPosition.Y - (float) (Game1.tileSize * 5 / 2) + (float) num1)), new Microsoft.Xna.Framework.Rectangle?(rectangle), color, 0.0f, Vector2.Zero, 4f, SpriteEffects.FlipHorizontally, Math.Max(0.0f, (float) (f.getStandingY() + Game1.tileSize) / 10000f));
                break;
            }
          }
          else if (f.CurrentTool != null && f.CurrentTool is WateringCan)
          {
            switch (((FarmerSprite) f.Sprite).indexInCurrentAnimation)
            {
              case 0:
              case 1:
                SpriteBatch spriteBatch1 = Game1.spriteBatch;
                Texture2D toolSpriteSheet1 = Game1.toolSpriteSheet;
                Vector2 position1 = Utility.snapToInt(new Vector2((float) ((double) playerPosition.X + (double) num2 - 4.0), (float) ((double) playerPosition.Y - (double) (Game1.tileSize * 2) + 8.0) + (float) num1));
                Microsoft.Xna.Framework.Rectangle? sourceRectangle1 = new Microsoft.Xna.Framework.Rectangle?(rectangle);
                Color white1 = Color.White;
                double num4 = 0.0;
                Vector2 origin1 = new Vector2(0.0f, (float) Game1.tileSize / 4f);
                double num5 = 4.0;
                int num6 = 1;
                double num7 = 0.0;
                boundingBox = f.GetBoundingBox();
                double num8 = (double) (boundingBox.Bottom + num3) / 10000.0;
                double num9 = (double) Math.Max((float) num7, (float) num8);
                spriteBatch1.Draw(toolSpriteSheet1, position1, sourceRectangle1, white1, (float) num4, origin1, (float) num5, (SpriteEffects) num6, (float) num9);
                break;
              case 2:
                SpriteBatch spriteBatch2 = Game1.spriteBatch;
                Texture2D toolSpriteSheet2 = Game1.toolSpriteSheet;
                Vector2 position2 = Utility.snapToInt(new Vector2((float) ((double) playerPosition.X + (double) num2 - 16.0), playerPosition.Y - (float) (Game1.tileSize * 2) + (float) num1));
                Microsoft.Xna.Framework.Rectangle? sourceRectangle2 = new Microsoft.Xna.Framework.Rectangle?(rectangle);
                Color white2 = Color.White;
                double num10 = -0.261799395084381;
                Vector2 origin2 = new Vector2(0.0f, (float) Game1.tileSize / 4f);
                double num11 = 4.0;
                int num12 = 1;
                double num13 = 0.0;
                boundingBox = f.GetBoundingBox();
                double num14 = (double) (boundingBox.Bottom + num3) / 10000.0;
                double num15 = (double) Math.Max((float) num13, (float) num14);
                spriteBatch2.Draw(toolSpriteSheet2, position2, sourceRectangle2, white2, (float) num10, origin2, (float) num11, (SpriteEffects) num12, (float) num15);
                break;
              case 3:
                rectangle.X += Game1.tileSize / 4;
                SpriteBatch spriteBatch3 = Game1.spriteBatch;
                Texture2D toolSpriteSheet3 = Game1.toolSpriteSheet;
                Vector2 position3 = Utility.snapToInt(new Vector2((float) ((double) playerPosition.X + (double) num2 - 16.0), (float) ((double) playerPosition.Y - (double) (Game1.tileSize * 2) - 24.0) + (float) num1));
                Microsoft.Xna.Framework.Rectangle? sourceRectangle3 = new Microsoft.Xna.Framework.Rectangle?(rectangle);
                Color white3 = Color.White;
                double num16 = 0.0;
                Vector2 origin3 = new Vector2(0.0f, (float) Game1.tileSize / 4f);
                double num17 = 4.0;
                int num18 = 1;
                double num19 = 0.0;
                boundingBox = f.GetBoundingBox();
                double num20 = (double) (boundingBox.Bottom + num3) / 10000.0;
                double num21 = (double) Math.Max((float) num19, (float) num20);
                spriteBatch3.Draw(toolSpriteSheet3, position3, sourceRectangle3, white3, (float) num16, origin3, (float) num17, (SpriteEffects) num18, (float) num21);
                break;
            }
          }
          else
          {
            switch (((FarmerSprite) f.Sprite).indexInCurrentAnimation)
            {
              case 0:
                Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2((float) ((double) playerPosition.X + (double) (Game1.tileSize / 2) + 8.0) + (float) num2 + (float) Math.Min(8, f.toolPower * 4), (float) ((double) playerPosition.Y - (double) (Game1.tileSize * 2) + 8.0) + (float) num1 + (float) Math.Min(8, f.toolPower * 4))), new Microsoft.Xna.Framework.Rectangle?(rectangle), Color.White, (float) (0.261799395084381 + (double) Math.Min(f.toolPower, 2) * 0.0490873865783215), new Vector2(0.0f, (float) Game1.tileSize / 4f), 4f, SpriteEffects.FlipHorizontally, Math.Max(0.0f, (float) (f.GetBoundingBox().Bottom + num3) / 10000f));
                break;
              case 1:
                Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(playerPosition.X - (float) (Game1.tileSize / 4) + (float) num2, (float) ((double) playerPosition.Y - (double) (Game1.tileSize * 2) + 16.0) + (float) num1)), new Microsoft.Xna.Framework.Rectangle?(rectangle), Color.White, -0.2617994f, new Vector2(0.0f, (float) Game1.tileSize / 4f), 4f, SpriteEffects.FlipHorizontally, Math.Max(0.0f, (float) (f.GetBoundingBox().Bottom + num3) / 10000f));
                break;
              case 2:
                Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2((float) ((double) playerPosition.X - (double) Game1.tileSize + 4.0) + (float) num2, (float) ((double) playerPosition.Y - (double) (Game1.tileSize * 2) + 60.0) + (float) num1)), new Microsoft.Xna.Framework.Rectangle?(rectangle), Color.White, -0.7853982f, new Vector2(0.0f, (float) Game1.tileSize / 4f), 4f, SpriteEffects.FlipHorizontally, Math.Max(0.0f, (float) (f.GetBoundingBox().Bottom + num3) / 10000f));
                break;
              case 3:
                Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2((float) ((double) playerPosition.X - (double) Game1.tileSize + 20.0) + (float) num2, (float) ((double) playerPosition.Y - (double) Game1.tileSize + 76.0) + (float) num1)), new Microsoft.Xna.Framework.Rectangle?(rectangle), Color.White, -1.832596f, new Vector2(0.0f, (float) Game1.tileSize / 4f), 4f, SpriteEffects.FlipHorizontally, Math.Max(0.0f, (float) (f.GetBoundingBox().Bottom + num3) / 10000f));
                break;
              case 4:
                SpriteBatch spriteBatch4 = Game1.spriteBatch;
                Texture2D toolSpriteSheet4 = Game1.toolSpriteSheet;
                Vector2 position4 = Utility.snapToInt(new Vector2((float) ((double) playerPosition.X - (double) Game1.tileSize + 24.0) + (float) num2, playerPosition.Y + 24f + (float) num1));
                Microsoft.Xna.Framework.Rectangle? sourceRectangle4 = new Microsoft.Xna.Framework.Rectangle?(rectangle);
                Color white4 = Color.White;
                double num22 = -1.83259582519531;
                Vector2 origin4 = new Vector2(0.0f, (float) Game1.tileSize / 4f);
                double num23 = 4.0;
                int num24 = 1;
                double num25 = 0.0;
                boundingBox = f.GetBoundingBox();
                double num26 = (double) (boundingBox.Bottom + num3) / 10000.0;
                double num27 = (double) Math.Max((float) num25, (float) num26);
                spriteBatch4.Draw(toolSpriteSheet4, position4, sourceRectangle4, white4, (float) num22, origin4, (float) num23, (SpriteEffects) num24, (float) num27);
                break;
            }
          }
        }
        else if (!(f.CurrentTool is MeleeWeapon) || f.FacingDirection != 0)
        {
          if (((FarmerSprite) f.Sprite).indexInCurrentAnimation > 2 && (!(f.CurrentTool is FishingRod) || (f.CurrentTool as FishingRod).isCasting || ((f.CurrentTool as FishingRod).castedButBobberStillInAir || (f.CurrentTool as FishingRod).isTimingCast)))
          {
            Point tileLocationPoint = f.getTileLocationPoint();
            if (f.currentLocation.getTileIndexAt(tileLocationPoint, "Front") != -1 && (double) f.position.Y % (double) Game1.tileSize < (double) (Game1.tileSize / 2) && (double) f.position.Y % (double) Game1.tileSize > (double) (Game1.tileSize / 4))
              return;
          }
          else if (f.CurrentTool is FishingRod && ((FarmerSprite) f.Sprite).indexInCurrentAnimation <= 2)
          {
            Point tileLocationPoint = f.getTileLocationPoint();
            --tileLocationPoint.Y;
            if (f.currentLocation.getTileIndexAt(tileLocationPoint, "Front") != -1)
              return;
          }
          if (f.CurrentTool != null && f.CurrentTool is FishingRod || currentToolIndex >= 48 && currentToolIndex <= 55 && !(f.CurrentTool as FishingRod).fishCaught)
          {
            Color color = (f.CurrentTool as FishingRod).getColor();
            switch (((FarmerSprite) f.Sprite).indexInCurrentAnimation)
            {
              case 0:
                if (!(f.CurrentTool as FishingRod).showingTreasure && !(f.CurrentTool as FishingRod).fishCaught && (f.FacingDirection != 0 || !(f.CurrentTool as FishingRod).isFishing || (f.CurrentTool as FishingRod).isReeling))
                {
                  Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(playerPosition.X - 64f, (float) ((double) playerPosition.Y - (double) (Game1.tileSize * 2) + 4.0))), new Microsoft.Xna.Framework.Rectangle?(rectangle), color, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0.0f, (float) (f.getStandingY() + (f.FacingDirection == 0 ? 0 : Game1.tileSize * 2)) / 10000f));
                  break;
                }
                break;
              case 1:
                Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(playerPosition.X - 64f, (float) ((double) playerPosition.Y - (double) (Game1.tileSize * 2) + 4.0))), new Microsoft.Xna.Framework.Rectangle?(rectangle), color, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0.0f, (float) (f.getStandingY() + (f.FacingDirection == 0 ? 0 : Game1.tileSize * 2)) / 10000f));
                break;
              case 2:
                Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(playerPosition.X - 64f, (float) ((double) playerPosition.Y - (double) (Game1.tileSize * 2) + 4.0))), new Microsoft.Xna.Framework.Rectangle?(rectangle), color, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0.0f, (float) (f.getStandingY() + (f.FacingDirection == 0 ? 0 : Game1.tileSize * 2)) / 10000f));
                break;
              case 3:
                if (f.FacingDirection == 2)
                {
                  Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(playerPosition.X - 64f, (float) ((double) playerPosition.Y - (double) (Game1.tileSize * 2) + 4.0))), new Microsoft.Xna.Framework.Rectangle?(rectangle), color, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0.0f, (float) (f.getStandingY() + (f.FacingDirection == 0 ? 0 : Game1.tileSize * 2)) / 10000f));
                  break;
                }
                break;
              case 4:
                if (f.FacingDirection == 0 && (f.CurrentTool as FishingRod).isFishing)
                {
                  Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(playerPosition.X - 80f, playerPosition.Y - 96f)), new Microsoft.Xna.Framework.Rectangle?(rectangle), color, 0.0f, Vector2.Zero, 4f, SpriteEffects.FlipVertically, Math.Max(0.0f, (float) (f.getStandingY() + Game1.tileSize * 2) / 10000f));
                  break;
                }
                if (f.FacingDirection == 2)
                {
                  Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(playerPosition.X - 64f, (float) ((double) playerPosition.Y - (double) (Game1.tileSize * 2) + 4.0))), new Microsoft.Xna.Framework.Rectangle?(rectangle), color, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0.0f, (float) (f.getStandingY() + (f.FacingDirection == 0 ? 0 : Game1.tileSize * 2)) / 10000f));
                  break;
                }
                break;
              case 5:
                if (f.FacingDirection == 2 && !(f.CurrentTool as FishingRod).showingTreasure && !(f.CurrentTool as FishingRod).fishCaught)
                {
                  Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(playerPosition.X - 64f, (float) ((double) playerPosition.Y - (double) (Game1.tileSize * 2) + 4.0))), new Microsoft.Xna.Framework.Rectangle?(rectangle), color, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, Math.Max(0.0f, (float) (f.getStandingY() + (f.FacingDirection == 0 ? 0 : Game1.tileSize * 2)) / 10000f));
                  break;
                }
                break;
            }
          }
          else if (f.CurrentTool != null && f.CurrentTool is WateringCan)
          {
            switch (((FarmerSprite) f.Sprite).indexInCurrentAnimation)
            {
              case 0:
              case 1:
                SpriteBatch spriteBatch = Game1.spriteBatch;
                Texture2D toolSpriteSheet = Game1.toolSpriteSheet;
                Vector2 position = Utility.snapToInt(new Vector2(playerPosition.X + (float) num2, (float) ((double) playerPosition.Y - (double) (Game1.tileSize * 2) + 16.0) + (float) num1));
                Microsoft.Xna.Framework.Rectangle? sourceRectangle = new Microsoft.Xna.Framework.Rectangle?(rectangle);
                Color white = Color.White;
                double num3 = 0.0;
                Vector2 origin = new Vector2(0.0f, (float) Game1.tileSize / 4f);
                double num4 = 4.0;
                int num5 = 0;
                double num6 = 0.0;
                boundingBox = f.GetBoundingBox();
                double num7 = (double) boundingBox.Bottom / 10000.0;
                double num8 = (double) Math.Max((float) num6, (float) num7);
                spriteBatch.Draw(toolSpriteSheet, position, sourceRectangle, white, (float) num3, origin, (float) num4, (SpriteEffects) num5, (float) num8);
                break;
              case 2:
                Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(playerPosition.X + (float) num2, (float) ((double) playerPosition.Y - (double) (Game1.tileSize * 2) - (f.FacingDirection == 2 ? -4.0 : 32.0)) + (float) num1)), new Microsoft.Xna.Framework.Rectangle?(rectangle), Color.White, 0.0f, new Vector2(0.0f, (float) Game1.tileSize / 4f), 4f, SpriteEffects.None, Math.Max(0.0f, (float) f.GetBoundingBox().Bottom / 10000f));
                break;
              case 3:
                if (f.FacingDirection == 2)
                  rectangle.X += Game1.tileSize / 4;
                Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2((float) ((double) playerPosition.X + (double) num2 - (f.FacingDirection == 2 ? 4.0 : 0.0)), (float) ((double) playerPosition.Y - (double) (Game1.tileSize * 2) - (f.FacingDirection == 2 ? -24.0 : 64.0)) + (float) num1)), new Microsoft.Xna.Framework.Rectangle?(rectangle), Color.White, 0.0f, new Vector2(0.0f, (float) Game1.tileSize / 4f), 4f, SpriteEffects.None, Math.Max(0.0f, (float) f.GetBoundingBox().Bottom / 10000f));
                break;
            }
          }
          else
          {
            switch (((FarmerSprite) f.Sprite).indexInCurrentAnimation)
            {
              case 0:
                if (f.FacingDirection == 0)
                {
                  Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(playerPosition.X + (float) num2, (float) ((double) playerPosition.Y - (double) (Game1.tileSize * 2) - 8.0) + (float) num1 + (float) Math.Min(8, f.toolPower * 4))), new Microsoft.Xna.Framework.Rectangle?(rectangle), Color.White, 0.0f, new Vector2(0.0f, (float) Game1.tileSize / 4f), 4f, SpriteEffects.None, Math.Max(0.0f, (float) (f.getStandingY() - Game1.tileSize / 8) / 10000f));
                  break;
                }
                Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2((float) ((double) playerPosition.X + (double) num2 - 20.0), (float) ((double) playerPosition.Y - (double) (Game1.tileSize * 2) + 12.0) + (float) num1 + (float) Math.Min(8, f.toolPower * 4))), new Microsoft.Xna.Framework.Rectangle?(rectangle), Color.White, 0.0f, new Vector2(0.0f, (float) Game1.tileSize / 4f), 4f, SpriteEffects.None, Math.Max(0.0f, (float) (f.getStandingY() + Game1.tileSize / 8) / 10000f));
                break;
              case 1:
                if (f.FacingDirection == 0)
                {
                  Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(playerPosition.X + (float) num2 + (float) Game1.pixelZoom, (float) ((double) playerPosition.Y - (double) (Game1.tileSize * 2) + 40.0) + (float) num1)), new Microsoft.Xna.Framework.Rectangle?(rectangle), Color.White, 0.0f, new Vector2(0.0f, (float) Game1.tileSize / 4f), 4f, SpriteEffects.None, Math.Max(0.0f, (float) (f.getStandingY() - Game1.tileSize / 8) / 10000f));
                  break;
                }
                Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2((float) ((double) playerPosition.X + (double) num2 - 12.0), (float) ((double) playerPosition.Y - (double) (Game1.tileSize * 2) + 32.0) + (float) num1)), new Microsoft.Xna.Framework.Rectangle?(rectangle), Color.White, -0.1308997f, new Vector2(0.0f, (float) Game1.tileSize / 4f), 4f, SpriteEffects.None, Math.Max(0.0f, (float) (f.getStandingY() + Game1.tileSize / 8) / 10000f));
                break;
              case 2:
                Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(playerPosition.X + (float) num2, (float) ((double) playerPosition.Y - (double) (Game1.tileSize * 2) + 64.0) + (float) num1)), new Microsoft.Xna.Framework.Rectangle?(rectangle), Color.White, 0.0f, new Vector2(0.0f, (float) Game1.tileSize / 4f), 4f, SpriteEffects.None, Math.Max(0.0f, (float) ((f.getStandingY() + f.FacingDirection == 0 ? (double) (-Game1.tileSize / 8) : (double) (Game1.tileSize / 8)) / 10000.0)));
                break;
              case 3:
                if (f.FacingDirection != 0)
                {
                  Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(playerPosition.X + (float) num2, (float) ((double) playerPosition.Y - (double) Game1.tileSize + 44.0) + (float) num1)), new Microsoft.Xna.Framework.Rectangle?(rectangle), Color.White, 0.0f, new Vector2(0.0f, (float) Game1.tileSize / 4f), 4f, SpriteEffects.None, Math.Max(0.0f, (float) (f.getStandingY() + Game1.tileSize / 8) / 10000f));
                  break;
                }
                break;
              case 4:
                if (f.FacingDirection != 0)
                {
                  Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(playerPosition.X + (float) num2, (float) ((double) playerPosition.Y - (double) Game1.tileSize + 48.0) + (float) num1)), new Microsoft.Xna.Framework.Rectangle?(rectangle), Color.White, 0.0f, new Vector2(0.0f, (float) Game1.tileSize / 4f), 4f, SpriteEffects.None, Math.Max(0.0f, (float) (f.getStandingY() + Game1.tileSize / 8) / 10000f));
                  break;
                }
                break;
              case 5:
                Game1.spriteBatch.Draw(Game1.toolSpriteSheet, Utility.snapToInt(new Vector2(playerPosition.X + (float) num2, (float) ((double) playerPosition.Y - (double) Game1.tileSize + 32.0) + (float) num1)), new Microsoft.Xna.Framework.Rectangle?(rectangle), Color.White, 0.0f, new Vector2(0.0f, (float) Game1.tileSize / 4f), 4f, SpriteEffects.None, Math.Max(0.0f, (float) (f.getStandingY() + Game1.tileSize / 8) / 10000f));
                break;
            }
          }
        }
        if (f.FacingDirection != 0)
          return;
        FarmerRenderer farmerRenderer = f.FarmerRenderer;
        SpriteBatch spriteBatch5 = Game1.spriteBatch;
        FarmerSprite farmerSprite = f.FarmerSprite;
        Microsoft.Xna.Framework.Rectangle sourceRect = f.FarmerSprite.SourceRect;
        Vector2 position5 = f.getLocalPosition(Game1.viewport) + f.jitter;
        Vector2 origin5 = new Vector2(0.0f, (float) (((double) f.yOffset + (double) (Game1.tileSize * 2) - (double) (f.GetBoundingBox().Height / 2)) / 4.0 + 4.0));
        double num28 = 0.0;
        boundingBox = f.GetBoundingBox();
        double num29 = ((double) boundingBox.Bottom + 1.0) / 10000.0;
        double num30 = (double) Math.Max((float) num28, (float) num29);
        Color white5 = Color.White;
        double num31 = 0.0;
        Farmer who = f;
        farmerRenderer.draw(spriteBatch5, farmerSprite, sourceRect, position5, origin5, (float) num30, white5, (float) num31, who);
      }
    }

    public static Vector2 GlobalToLocal(xTile.Dimensions.Rectangle viewport, Vector2 globalPosition)
    {
      return new Vector2(globalPosition.X - (float) viewport.X, globalPosition.Y - (float) viewport.Y);
    }

    public static Vector2 GlobalToLocal(Vector2 globalPosition)
    {
      return new Vector2(globalPosition.X - (float) Game1.viewport.X, globalPosition.Y - (float) Game1.viewport.Y);
    }

    public static Microsoft.Xna.Framework.Rectangle GlobalToLocal(xTile.Dimensions.Rectangle viewport, Microsoft.Xna.Framework.Rectangle globalPosition)
    {
      return new Microsoft.Xna.Framework.Rectangle(globalPosition.X - viewport.X, globalPosition.Y - viewport.Y, globalPosition.Width, globalPosition.Height);
    }

    public static string parseText(string text, SpriteFont whichFont, int width)
    {
      if (text == null)
        return "";
      string str1 = string.Empty;
      string str2 = string.Empty;
      if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ja || LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.zh || LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.th)
      {
        foreach (char ch in text)
        {
          if ((double) whichFont.MeasureString(str1 + ch.ToString()).Length() > (double) width || ch.Equals((object) Environment.NewLine))
          {
            str2 = str2 + str1 + Environment.NewLine;
            str1 = string.Empty;
          }
          if (!ch.Equals((object) Environment.NewLine))
            str1 += ch.ToString();
        }
        return str2 + str1;
      }
      string str3 = text;
      char[] chArray = new char[1]{ ' ' };
      foreach (string str4 in str3.Split(chArray))
      {
        try
        {
          if ((double) whichFont.MeasureString(str1 + str4).Length() > (double) width || str4.Equals(Environment.NewLine))
          {
            str2 = str2 + str1 + Environment.NewLine;
            str1 = string.Empty;
          }
          if (!str4.Equals(Environment.NewLine))
            str1 = str1 + str4 + " ";
        }
        catch (Exception ex)
        {
          Console.WriteLine("Exception measuring string: " + (object) ex);
        }
      }
      return str2 + str1;
    }

    public static string LoadStringByGender(int npcGender, string key)
    {
      if (npcGender == 0)
        return ((IEnumerable<string>) Game1.content.LoadString(key).Split('/')).First<string>();
      return ((IEnumerable<string>) Game1.content.LoadString(key).Split('/')).Last<string>();
    }

    public static string LoadStringByGender(int npcGender, string key, params object[] substitutions)
    {
      if (npcGender == 0)
      {
        string format = ((IEnumerable<string>) Game1.content.LoadString(key).Split('/')).First<string>();
        if (substitutions.Length != 0)
        {
          try
          {
            return string.Format(format, substitutions);
          }
          catch (Exception ex)
          {
            return format;
          }
        }
      }
      string format1 = ((IEnumerable<string>) Game1.content.LoadString(key).Split('/')).Last<string>();
      if (substitutions.Length == 0)
        return format1;
      try
      {
        return string.Format(format1, substitutions);
      }
      catch (Exception ex)
      {
        return format1;
      }
    }

    public static string parseText(string text)
    {
      return Game1.parseText(text, Game1.dialogueFont, Game1.dialogueWidth);
    }

    public static bool isThisPositionVisibleToPlayer(string locationName, Vector2 position)
    {
      return locationName.Equals(Game1.currentLocation.Name) && new Microsoft.Xna.Framework.Rectangle((int) ((double) Game1.player.position.X - (double) (Game1.viewport.Width / 2)), (int) ((double) Game1.player.position.Y - (double) (Game1.viewport.Height / 2)), Game1.viewport.Width, Game1.viewport.Height).Contains(new Point((int) position.X, (int) position.Y));
    }

    public static string getProperArticleForWord(string word)
    {
      string str = "a";
      if (word != null)
      {
        char ch = word.ToLower()[0];
        if ((uint) ch <= 101U)
        {
          if ((int) ch != 97)
          {
            if ((int) ch == 101)
              str += "n";
          }
          else
            str += "n";
        }
        else if ((int) ch != 105)
        {
          if ((int) ch != 111)
          {
            if ((int) ch == 117)
              str += "n";
          }
          else
            str += "n";
        }
        else
          str += "n";
      }
      return str;
    }

    public static Microsoft.Xna.Framework.Rectangle getSourceRectForStandardTileSheet(Texture2D tileSheet, int tilePosition, int width = -1, int height = -1)
    {
      if (width == -1)
        width = Game1.tileSize;
      if (height == -1)
        height = Game1.tileSize;
      return new Microsoft.Xna.Framework.Rectangle(tilePosition * width % tileSheet.Width, tilePosition * width / tileSheet.Width * height, width, height);
    }

    public static Microsoft.Xna.Framework.Rectangle getSquareSourceRectForNonStandardTileSheet(Texture2D tileSheet, int tileWidth, int tileHeight, int tilePosition)
    {
      return new Microsoft.Xna.Framework.Rectangle(tilePosition * tileWidth % tileSheet.Width, tilePosition * tileWidth / tileSheet.Width * tileHeight, tileWidth, tileHeight);
    }

    public static Microsoft.Xna.Framework.Rectangle getArbitrarySourceRect(Texture2D tileSheet, int tileWidth, int tileHeight, int tilePosition)
    {
      if (tileSheet != null)
        return new Microsoft.Xna.Framework.Rectangle(tilePosition * tileWidth % tileSheet.Width, tilePosition * tileWidth / tileSheet.Width * tileHeight, tileWidth, tileHeight);
      return Microsoft.Xna.Framework.Rectangle.Empty;
    }

    public static string getTimeOfDayString(int time)
    {
      string str1 = time % 100 == 0 ? "0" : "";
      string str2 = time / 100 % 12 == 0 ? "12" : string.Concat((object) (time / 100 % 12));
      switch (LocalizedContentManager.CurrentLanguageCode)
      {
        case LocalizedContentManager.LanguageCode.en:
        case LocalizedContentManager.LanguageCode.ja:
          str2 = time / 100 % 12 == 0 ? "12" : string.Concat((object) (time / 100 % 12));
          break;
        case LocalizedContentManager.LanguageCode.ru:
        case LocalizedContentManager.LanguageCode.pt:
        case LocalizedContentManager.LanguageCode.es:
        case LocalizedContentManager.LanguageCode.de:
        case LocalizedContentManager.LanguageCode.th:
          string str3 = string.Concat((object) (time / 100 % 24));
          str2 = time / 100 % 24 <= 9 ? "0" + str3 : str3;
          break;
        case LocalizedContentManager.LanguageCode.zh:
          str2 = time / 100 % 24 == 0 ? "00" : (time / 100 % 12 == 0 ? "12" : string.Concat((object) (time / 100 % 12)));
          break;
      }
      string str4 = str2 + ":" + (object) (time % 100) + str1;
      if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.en)
        str4 = str4 + " " + (time < 1200 || time >= 2400 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:DayTimeMoneyBox.cs.10370") : Game1.content.LoadString("Strings\\StringsFromCSFiles:DayTimeMoneyBox.cs.10371"));
      else if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ja)
        str4 = time < 1200 || time >= 2400 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:DayTimeMoneyBox.cs.10370") + " " + str4 : Game1.content.LoadString("Strings\\StringsFromCSFiles:DayTimeMoneyBox.cs.10371") + " " + str4;
      else if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.zh)
        str4 = time < 600 || time >= 2400 ? "凌晨 " + str4 : (time < 1200 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:DayTimeMoneyBox.cs.10370") + " " + str4 : (time < 1300 ? "中午  " + str4 : (time < 1900 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:DayTimeMoneyBox.cs.10371") + " " + str4 : "晚上  " + str4)));
      return str4;
    }

    public bool checkBigCraftableBoundariesForFrontLayer()
    {
      if (Game1.currentLocation.Map.GetLayer("Front").PickTile(new Location(Game1.player.getStandingX() - Game1.tileSize / 2, (int) Game1.player.position.Y - Game1.tileSize * 3 / 5), Game1.viewport.Size) == null && Game1.currentLocation.Map.GetLayer("Front").PickTile(new Location(Game1.player.getStandingX() + Game1.tileSize / 2, (int) Game1.player.position.Y - Game1.tileSize * 3 / 5), Game1.viewport.Size) == null && Game1.currentLocation.Map.GetLayer("Front").PickTile(new Location(Game1.player.getStandingX() - Game1.tileSize / 2, (int) Game1.player.position.Y - Game1.tileSize * 3 / 5 - Game1.tileSize), Game1.viewport.Size) == null)
        return Game1.currentLocation.Map.GetLayer("Front").PickTile(new Location(Game1.player.getStandingX() + Game1.tileSize / 2, (int) Game1.player.position.Y - Game1.tileSize * 3 / 5 - Game1.tileSize), Game1.viewport.Size) != null;
      return true;
    }

    public static bool[,] getCircleOutlineGrid(int radius)
    {
      bool[,] flagArray = new bool[radius * 2 + 1, radius * 2 + 1];
      int num1 = 1 - radius;
      int num2 = 1;
      int num3 = -2 * radius;
      int num4 = 0;
      int num5 = radius;
      int index1 = radius;
      int index2 = radius;
      flagArray[index1, index2 + radius] = true;
      flagArray[index1, index2 - radius] = true;
      flagArray[index1 + radius, index2] = true;
      flagArray[index1 - radius, index2] = true;
      while (num4 < num5)
      {
        if (num1 >= 0)
        {
          --num5;
          num3 += 2;
          num1 += num3;
        }
        ++num4;
        num2 += 2;
        num1 += num2;
        flagArray[index1 + num4, index2 + num5] = true;
        flagArray[index1 - num4, index2 + num5] = true;
        flagArray[index1 + num4, index2 - num5] = true;
        flagArray[index1 - num4, index2 - num5] = true;
        flagArray[index1 + num5, index2 + num4] = true;
        flagArray[index1 - num5, index2 + num4] = true;
        flagArray[index1 + num5, index2 - num4] = true;
        flagArray[index1 - num5, index2 - num4] = true;
      }
      return flagArray;
    }

    public static Color getColorForTreasureType(string type)
    {
      // ISSUE: reference to a compiler-generated method
      uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(type);
      if (stringHash <= 872197005U)
      {
        if ((int) stringHash != 116937720)
        {
          if ((int) stringHash != 849800425)
          {
            if ((int) stringHash == 872197005 && type == "Coins")
              return Color.Yellow;
          }
          else if (type == "Arch")
            return Color.White;
        }
        else if (type == "Iridium")
          return Color.Purple;
      }
      else if (stringHash <= 1952841722U)
      {
        if ((int) stringHash != 1821685427)
        {
          if ((int) stringHash == 1952841722 && type == "Coal")
            return Color.Black;
        }
        else if (type == "Gold")
          return Color.Gold;
      }
      else if ((int) stringHash != -874770789)
      {
        if ((int) stringHash == -473546124 && type == "Copper")
          return Color.Sienna;
      }
      else if (type == "Iron")
        return Color.LightSlateGray;
      return Color.SaddleBrown;
    }

    public delegate void afterFadeFunction();
  }
}
