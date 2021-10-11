// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.TitleMenu
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Minigames;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StardewValley.Menus
{
  public class TitleMenu : IClickableMenu, IDisposable
  {
    private static int windowNumber = 3;
    public LocalizedContentManager menuContent = Game1.content.CreateTemporary();
    private List<float> bigClouds = new List<float>();
    private List<float> smallClouds = new List<float>();
    private List<TemporaryAnimatedSprite> tempSprites = new List<TemporaryAnimatedSprite>();
    public List<ClickableTextureComponent> buttons = new List<ClickableTextureComponent>();
    private List<TemporaryAnimatedSprite> birds = new List<TemporaryAnimatedSprite>();
    private float globalCloudAlpha = 1f;
    private int numFarmsSaved = -1;
    public string startupMessage = "";
    public Color startupMessageColor = Color.DeepSkyBlue;
    private string whichSubMenu = "";
    public const int region_muteMusic = 81111;
    public const int region_windowedButton = 81112;
    public const int region_aboutButton = 81113;
    public const int region_backButton = 81114;
    public const int region_newButton = 81115;
    public const int region_loadButton = 81116;
    public const int region_exitButton = 81117;
    public const int region_languagesButton = 81118;
    public const int fadeFromWhiteDuration = 2000;
    public const int viewportFinalPosition = -1000;
    public const int logoSwipeDuration = 1000;
    public const int numberOfButtons = 3;
    public const int spaceBetweenButtons = 8;
    public const float bigCloudDX = 0.1f;
    public const float mediumCloudDX = 0.2f;
    public const float smallCloudDX = 0.3f;
    public const float bgmountainsParallaxSpeed = 0.66f;
    public const float mountainsParallaxSpeed = 1f;
    public const float foregroundJungleParallaxSpeed = 2f;
    public const float cloudsParallaxSpeed = 0.5f;
    public const int pixelZoom = 3;
    private Texture2D cloudsTexture;
    private Texture2D titleButtonsTexture;
    public ClickableTextureComponent backButton;
    public ClickableTextureComponent muteMusicButton;
    public ClickableTextureComponent aboutButton;
    public ClickableTextureComponent languageButton;
    public ClickableTextureComponent windowedButton;
    public ClickableComponent skipButton;
    private Rectangle eRect;
    private List<Rectangle> leafRects;
    private static IClickableMenu _subMenu;
    private StartupPreferences startupPreferences;
    private int globalXOffset;
    private float viewportY;
    private float viewportDY;
    private float logoSwipeTimer;
    private int fadeFromWhiteTimer;
    private int pauseBeforeViewportRiseTimer;
    private int buttonsToShow;
    private int showButtonsTimer;
    private int logoFadeTimer;
    private int logoSurprisedTimer;
    private int clicksOnE;
    private int clicksOnLeaf;
    private int buttonsDX;
    private int chuckleFishTimer;
    private bool titleInPosition;
    private bool isTransitioningButtons;
    private bool shades;
    private bool transitioningCharacterCreationMenu;
    private int bCount;
    private int quitTimer;
    private bool transitioningFromLoadScreen;
    private bool disposedValue;

    public static IClickableMenu subMenu
    {
      get
      {
        return TitleMenu._subMenu;
      }
      set
      {
        if (TitleMenu._subMenu != null && TitleMenu._subMenu is IDisposable)
          (TitleMenu._subMenu as IDisposable).Dispose();
        TitleMenu._subMenu = value;
      }
    }

    private bool HasActiveUser
    {
      get
      {
        return true;
      }
    }

    public TitleMenu()
      : base(0, 0, Game1.viewport.Width, Game1.viewport.Height, false)
    {
      LocalizedContentManager.OnLanguageChange += new LocalizedContentManager.LanguageChangedHandler(this.OnLanguageChange);
      this.cloudsTexture = this.menuContent.Load<Texture2D>(Path.Combine("Minigames", "Clouds"));
      this.titleButtonsTexture = this.menuContent.Load<Texture2D>(Path.Combine("Minigames", "TitleButtons"));
      this.viewportY = 0.0f;
      this.fadeFromWhiteTimer = 4000;
      this.logoFadeTimer = 5000;
      this.chuckleFishTimer = 4000;
      this.bigClouds.Add(-750f);
      this.bigClouds.Add((float) (this.width * 3 / 4));
      this.shades = Game1.random.NextDouble() < 0.5;
      this.smallClouds.Add((float) (this.width / 2));
      this.smallClouds.Add((float) (this.width - 1));
      this.smallClouds.Add(1f);
      this.smallClouds.Add((float) (this.width / 3));
      this.smallClouds.Add((float) (this.width * 2 / 3));
      this.smallClouds.Add((float) (this.width * 3 / 4));
      this.smallClouds.Add((float) (this.width / 4));
      this.smallClouds.Add((float) (this.width / 2 + 300));
      this.smallClouds.Add((float) (this.width - 1 + 300));
      this.smallClouds.Add(301f);
      this.smallClouds.Add((float) (this.width / 3 + 300));
      this.smallClouds.Add((float) (this.width * 2 / 3 + 300));
      this.smallClouds.Add((float) (this.width * 3 / 4 + 300));
      this.smallClouds.Add((float) (this.width / 4 + 300));
      if (Game1.currentSong == null && Game1.nextMusicTrack != null)
      {
        int length = Game1.nextMusicTrack.Length;
      }
      List<TemporaryAnimatedSprite> birds1 = this.birds;
      TemporaryAnimatedSprite temporaryAnimatedSprite1 = new TemporaryAnimatedSprite(this.titleButtonsTexture, new Rectangle(296, 227, 26, 21), new Vector2((float) (this.width - 210), (float) (this.height - 390)), false, 0.0f, Color.White);
      temporaryAnimatedSprite1.scale = 3f;
      temporaryAnimatedSprite1.pingPong = true;
      temporaryAnimatedSprite1.animationLength = 4;
      temporaryAnimatedSprite1.interval = 100f;
      temporaryAnimatedSprite1.totalNumberOfLoops = 9999;
      temporaryAnimatedSprite1.local = true;
      Vector2 vector2_1 = new Vector2(-1f, 0.0f);
      temporaryAnimatedSprite1.motion = vector2_1;
      double num1 = 0.25;
      temporaryAnimatedSprite1.layerDepth = (float) num1;
      birds1.Add(temporaryAnimatedSprite1);
      List<TemporaryAnimatedSprite> birds2 = this.birds;
      TemporaryAnimatedSprite temporaryAnimatedSprite2 = new TemporaryAnimatedSprite(this.titleButtonsTexture, new Rectangle(296, 227, 26, 21), new Vector2((float) (this.width - 120), (float) (this.height - 360)), false, 0.0f, Color.White);
      temporaryAnimatedSprite2.scale = 3f;
      temporaryAnimatedSprite2.pingPong = true;
      temporaryAnimatedSprite2.animationLength = 4;
      temporaryAnimatedSprite2.interval = 100f;
      temporaryAnimatedSprite2.totalNumberOfLoops = 9999;
      temporaryAnimatedSprite2.local = true;
      temporaryAnimatedSprite2.delayBeforeAnimationStart = 100;
      Vector2 vector2_2 = new Vector2(-1f, 0.0f);
      temporaryAnimatedSprite2.motion = vector2_2;
      double num2 = 0.25;
      temporaryAnimatedSprite2.layerDepth = (float) num2;
      birds2.Add(temporaryAnimatedSprite2);
      this.setUpIcons();
      ClickableTextureComponent textureComponent1 = new ClickableTextureComponent(new Rectangle(Game1.tileSize / 4, Game1.tileSize / 4, 9 * Game1.pixelZoom, 9 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(128, 384, 9, 9), (float) Game1.pixelZoom, false);
      int num3 = 81111;
      textureComponent1.myID = num3;
      int num4 = 81115;
      textureComponent1.downNeighborID = num4;
      int num5 = 81112;
      textureComponent1.rightNeighborID = num5;
      this.muteMusicButton = textureComponent1;
      ClickableTextureComponent textureComponent2 = new ClickableTextureComponent(new Rectangle(Game1.viewport.Width - 9 * Game1.pixelZoom - Game1.tileSize / 4, Game1.tileSize / 4, 9 * Game1.pixelZoom, 9 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(Game1.options == null || Game1.options.isCurrentlyWindowed() ? 146 : 155, 384, 9, 9), (float) Game1.pixelZoom, false);
      int num6 = 81112;
      textureComponent2.myID = num6;
      int num7 = 81111;
      textureComponent2.leftNeighborID = num7;
      int num8 = 81113;
      textureComponent2.downNeighborID = num8;
      this.windowedButton = textureComponent2;
      this.startupPreferences = new StartupPreferences();
      this.startupPreferences.loadPreferences();
      this.applyPreferences();
      switch (this.startupPreferences.timesPlayed)
      {
        case 100:
          this.startupMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:TitleMenu.cs.11732");
          break;
        case 1000:
          this.startupMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:TitleMenu.cs.11733");
          break;
        case 10000:
          this.startupMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:TitleMenu.cs.11734");
          break;
        case 2:
          this.startupMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:TitleMenu.cs.11717");
          break;
        case 3:
          this.startupMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:TitleMenu.cs.11718");
          break;
        case 4:
          this.startupMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:TitleMenu.cs.11719");
          break;
        case 5:
          this.startupMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:TitleMenu.cs.11720");
          break;
        case 6:
          this.startupMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:TitleMenu.cs.11721");
          break;
        case 7:
          this.startupMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:TitleMenu.cs.11722");
          break;
        case 8:
          this.startupMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:TitleMenu.cs.11723");
          break;
        case 9:
          this.startupMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:TitleMenu.cs.11724");
          break;
        case 10:
          this.startupMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:TitleMenu.cs.11725");
          break;
        case 15:
          if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.en)
          {
            string randomNoun1 = Dialogue.getRandomNoun();
            string randomNoun2 = Dialogue.getRandomNoun();
            this.startupMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:TitleMenu.cs.11726") + Environment.NewLine + "The " + Dialogue.getRandomAdjective() + " " + randomNoun1 + " " + Dialogue.getRandomVerb() + " " + Dialogue.getRandomPositional() + " the " + (randomNoun1.Equals(randomNoun2) ? "other " + randomNoun2 : randomNoun2);
            break;
          }
          int num9 = new Random().Next(1, 15);
          this.startupMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:RandomSentence." + (object) num9);
          break;
        case 20:
          this.startupMessage = "<";
          break;
        case 30:
          this.startupMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:TitleMenu.cs.11731");
          break;
      }
      this.startupPreferences.savePreferences();
      Game1.setRichPresence("menus", (object) null);
      if (!Game1.options.snappyMenus || !Game1.options.gamepadControls)
        return;
      this.populateClickableComponentList();
      this.snapToDefaultClickableComponent();
    }

    private bool alternativeTitleGraphic()
    {
      return Game1.content.GetCurrentLanguage() == LocalizedContentManager.LanguageCode.zh;
    }

    public void applyPreferences()
    {
      if (this.startupPreferences.startMuted)
        this.muteMusicButton.sourceRect.X = !Utility.toggleMuteMusic() ? 128 : 137;
      if (this.startupPreferences.skipWindowPreparation && TitleMenu.windowNumber == 3)
        TitleMenu.windowNumber = -1;
      if (!Game1.options.gamepadControls || !Game1.options.snappyMenus)
        return;
      this.populateClickableComponentList();
      this.snapToDefaultClickableComponent();
    }

    private void OnLanguageChange(LocalizedContentManager.LanguageCode code)
    {
      this.titleButtonsTexture = Game1.content.Load<Texture2D>(Path.Combine("Minigames", "TitleButtons"));
      this.setUpIcons();
      this.tempSprites.Clear();
    }

    public void skipToTitleButtons()
    {
      this.logoFadeTimer = 0;
      this.logoSwipeTimer = 0.0f;
      this.titleInPosition = false;
      this.pauseBeforeViewportRiseTimer = 0;
      this.fadeFromWhiteTimer = 0;
      this.viewportY = -999f;
      this.viewportDY = -0.01f;
      this.birds.Clear();
      this.logoSwipeTimer = 1f;
      this.chuckleFishTimer = 0;
      Game1.changeMusicTrack("MainTheme");
      if (!Game1.options.SnappyMenus || !Game1.options.gamepadControls)
        return;
      this.snapToDefaultClickableComponent();
    }

    public void setUpIcons()
    {
      this.buttons.Clear();
      int num1 = 74;
      int x1 = this.width / 2 - (num1 * 3 * 3 + 48) / 2;
      List<ClickableTextureComponent> buttons1 = this.buttons;
      ClickableTextureComponent textureComponent1 = new ClickableTextureComponent("New", new Rectangle(x1, this.height - 174 - 24, num1 * 3, 174), (string) null, "", this.titleButtonsTexture, new Rectangle(0, 187, 74, 58), 3f, false);
      int num2 = 81115;
      textureComponent1.myID = num2;
      int num3 = 81116;
      textureComponent1.rightNeighborID = num3;
      int num4 = 81111;
      textureComponent1.upNeighborID = num4;
      buttons1.Add(textureComponent1);
      int x2 = x1 + (num1 + 8) * 3;
      List<ClickableTextureComponent> buttons2 = this.buttons;
      ClickableTextureComponent textureComponent2 = new ClickableTextureComponent("Load", new Rectangle(x2, this.height - 174 - 24, 222, 174), (string) null, "", this.titleButtonsTexture, new Rectangle(74, 187, 74, 58), 3f, false);
      int num5 = 81116;
      textureComponent2.myID = num5;
      int num6 = 81115;
      textureComponent2.leftNeighborID = num6;
      int num7 = -7777;
      textureComponent2.rightNeighborID = num7;
      int num8 = 81111;
      textureComponent2.upNeighborID = num8;
      buttons2.Add(textureComponent2);
      int x3 = x2 + (num1 + 8) * 3;
      List<ClickableTextureComponent> buttons3 = this.buttons;
      ClickableTextureComponent textureComponent3 = new ClickableTextureComponent("Exit", new Rectangle(x3, this.height - 174 - 24, 222, 174), (string) null, "", this.titleButtonsTexture, new Rectangle(222, 187, 74, 58), 3f, false);
      int num9 = 81117;
      textureComponent3.myID = num9;
      int num10 = 81116;
      textureComponent3.leftNeighborID = num10;
      int num11 = 81118;
      textureComponent3.rightNeighborID = num11;
      int num12 = 81111;
      textureComponent3.upNeighborID = num12;
      buttons3.Add(textureComponent3);
      int num13 = this.height < 800 ? 2 : 3;
      this.eRect = new Rectangle(this.width / 2 - 200 * num13 + 251 * num13, -300 * num13 - (int) ((double) this.viewportY / 3.0) * num13 + 26 * num13, 42 * num13, 68 * num13);
      this.populateLeafRects();
      ClickableTextureComponent textureComponent4 = new ClickableTextureComponent(Game1.content.LoadString("Strings\\StringsFromCSFiles:TitleMenu.cs.11739"), new Rectangle(this.width - 198 - 48, this.height - 81 - 24, 198, 81), (string) null, "", this.titleButtonsTexture, new Rectangle(296, 252, 66, 27), 3f, false);
      int num14 = 81114;
      textureComponent4.myID = num14;
      this.backButton = textureComponent4;
      ClickableTextureComponent textureComponent5 = new ClickableTextureComponent(Game1.content.LoadString("Strings\\StringsFromCSFiles:TitleMenu.cs.11740"), new Rectangle(this.width - 66 - 48, this.height - 75 - 24, 66, 75), (string) null, "", this.titleButtonsTexture, new Rectangle(8, 458, 22, 25), 3f, false);
      int num15 = 81113;
      textureComponent5.myID = num15;
      int num16 = 81112;
      textureComponent5.upNeighborID = num16;
      int num17 = 81118;
      textureComponent5.leftNeighborID = num17;
      this.aboutButton = textureComponent5;
      ClickableTextureComponent textureComponent6 = new ClickableTextureComponent(Game1.content.LoadString("Strings\\StringsFromCSFiles:TitleMenu.cs.11740"), new Rectangle(this.width - 132 - 96, this.height - 75 - 24, 81, 75), (string) null, "", this.titleButtonsTexture, new Rectangle(52, 458, 27, 25), 3f, false);
      int num18 = 81118;
      textureComponent6.myID = num18;
      int num19 = 81113;
      textureComponent6.rightNeighborID = num19;
      int num20 = -7777;
      textureComponent6.leftNeighborID = num20;
      int num21 = 81112;
      textureComponent6.upNeighborID = num21;
      this.languageButton = textureComponent6;
      this.skipButton = new ClickableComponent(new Rectangle(this.width / 2 - 261, this.height / 2 - 102, 249, 201), Game1.content.LoadString("Strings\\StringsFromCSFiles:TitleMenu.cs.11741"));
      if (!Game1.options.gamepadControls || !Game1.options.snappyMenus)
        return;
      this.populateClickableComponentList();
      this.snapToDefaultClickableComponent();
    }

    public override void snapToDefaultClickableComponent()
    {
      this.currentlySnappedComponent = this.getComponentWithID(this.startupPreferences == null || this.startupPreferences.timesPlayed <= 0 ? 81115 : 81116);
      this.snapCursorToCurrentSnappedComponent();
    }

    protected override void customSnapBehavior(int direction, int oldRegion, int oldID)
    {
      if (oldID == 81116 && direction == 1)
      {
        if (this.getComponentWithID(81117) != null)
        {
          this.setCurrentlySnappedComponentTo(81117);
          this.snapCursorToCurrentSnappedComponent();
        }
        else
        {
          this.setCurrentlySnappedComponentTo(81118);
          this.snapCursorToCurrentSnappedComponent();
        }
      }
      else
      {
        if (oldID != 81118 || direction != 3)
          return;
        if (this.getComponentWithID(81117) != null)
        {
          this.setCurrentlySnappedComponentTo(81117);
          this.snapCursorToCurrentSnappedComponent();
        }
        else
        {
          this.setCurrentlySnappedComponentTo(81116);
          this.snapCursorToCurrentSnappedComponent();
        }
      }
    }

    public void populateLeafRects()
    {
      int num = this.height < 800 ? 2 : 3;
      this.leafRects = new List<Rectangle>();
      this.leafRects.Add(new Rectangle(this.width / 2 - 200 * num + 251 * num - 196 * num, -300 * num - (int) ((double) this.viewportY / 3.0) * num + 26 * num + 109 * num, 17 * num, 30 * num));
      this.leafRects.Add(new Rectangle(this.width / 2 - 200 * num + 251 * num + 91 * num, -300 * num - (int) ((double) this.viewportY / 3.0) * num + 26 * num - 26 * num, 17 * num, 31 * num));
      this.leafRects.Add(new Rectangle(this.width / 2 - 200 * num + 251 * num + 79 * num, -300 * num - (int) ((double) this.viewportY / 3.0) * num + 26 * num + 83 * num, 25 * num, 17 * num));
      this.leafRects.Add(new Rectangle(this.width / 2 - 200 * num + 251 * num - 213 * num, -300 * num - (int) ((double) this.viewportY / 3.0) * num + 26 * num - 24 * num, 14 * num, 23 * num));
      this.leafRects.Add(new Rectangle(this.width / 2 - 200 * num + 251 * num - 234 * num, -300 * num - (int) ((double) this.viewportY / 3.0) * num + 26 * num - 11 * num, 18 * num, 12 * num));
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
      if (this.transitioningCharacterCreationMenu || TitleMenu.subMenu == null)
        return;
      TitleMenu.subMenu.receiveRightClick(x, y, true);
    }

    public override bool readyToClose()
    {
      return false;
    }

    public override bool overrideSnappyMenuCursorMovementBan()
    {
      return !this.titleInPosition;
    }

    public override void leftClickHeld(int x, int y)
    {
      if (this.transitioningCharacterCreationMenu)
        return;
      base.leftClickHeld(x, y);
      if (TitleMenu.subMenu == null)
        return;
      TitleMenu.subMenu.leftClickHeld(x, y);
    }

    public override void releaseLeftClick(int x, int y)
    {
      if (this.transitioningCharacterCreationMenu)
        return;
      base.releaseLeftClick(x, y);
      if (TitleMenu.subMenu == null)
        return;
      TitleMenu.subMenu.releaseLeftClick(x, y);
    }

    public override void receiveKeyPress(Keys key)
    {
      if (this.transitioningCharacterCreationMenu)
        return;
      if (!Program.releaseBuild && key == Keys.N && (Game1.oldKBState.IsKeyDown(Keys.RightShift) && Game1.oldKBState.IsKeyDown(Keys.LeftControl)))
      {
        Game1.loadForNewGame(false);
        Game1.saveOnNewDay = false;
        Game1.player.eventsSeen.Add(60367);
        Game1.player.currentLocation = (GameLocation) Utility.getHomeOfFarmer(Game1.player);
        Game1.player.position = new Vector2(7f, 9f) * (float) Game1.tileSize;
        Game1.player.FarmerSprite.setOwner(Game1.player);
        Game1.NewDay(0.0f);
        Game1.exitActiveMenu();
        Game1.setGameMode((byte) 3);
      }
      else
      {
        if (this.logoFadeTimer > 0 && (key == Keys.B || key == Keys.Escape))
        {
          this.bCount = this.bCount + 1;
          if (key == Keys.Escape)
            this.bCount = this.bCount + 3;
          if (this.bCount >= 3)
          {
            Game1.playSound("bigDeSelect");
            this.logoFadeTimer = 0;
            this.fadeFromWhiteTimer = 0;
            Game1.delayedActions.Clear();
            this.pauseBeforeViewportRiseTimer = 0;
            this.fadeFromWhiteTimer = 0;
            this.viewportY = -999f;
            this.viewportDY = -0.01f;
            this.birds.Clear();
            this.logoSwipeTimer = 1f;
            this.chuckleFishTimer = 0;
            Game1.changeMusicTrack("MainTheme");
          }
        }
        if (Game1.options.doesInputListContain(Game1.options.menuButton, key))
          return;
        if (TitleMenu.subMenu != null)
          TitleMenu.subMenu.receiveKeyPress(key);
        if (!Game1.options.snappyMenus || !Game1.options.gamepadControls || TitleMenu.subMenu != null)
          return;
        base.receiveKeyPress(key);
      }
    }

    public override void receiveGamePadButton(Buttons b)
    {
      base.receiveGamePadButton(b);
      bool flag = true;
      if (TitleMenu.subMenu != null)
      {
        if (TitleMenu.subMenu is LoadGameMenu && (TitleMenu.subMenu as LoadGameMenu).deleteConfirmationScreen)
          flag = false;
        TitleMenu.subMenu.receiveGamePadButton(b);
      }
      if (!flag || b != Buttons.B)
        return;
      this.backButtonPressed();
    }

    public override void gamePadButtonHeld(Buttons b)
    {
      if (TitleMenu.subMenu == null)
        return;
      TitleMenu.subMenu.gamePadButtonHeld(b);
    }

    public void backButtonPressed()
    {
      Game1.playSound("bigDeSelect");
      this.buttonsDX = -1;
      if (TitleMenu.subMenu is AboutMenu)
      {
        TitleMenu.subMenu = (IClickableMenu) null;
        this.buttonsDX = 0;
        if (!Game1.options.SnappyMenus)
          return;
        this.setCurrentlySnappedComponentTo(81113);
        this.snapCursorToCurrentSnappedComponent();
      }
      else
      {
        this.isTransitioningButtons = true;
        if (TitleMenu.subMenu is LoadGameMenu)
          this.transitioningFromLoadScreen = true;
        TitleMenu.subMenu = (IClickableMenu) null;
        Game1.changeMusicTrack("spring_day_ambient");
      }
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
      if (this.HasActiveUser && this.muteMusicButton.containsPoint(x, y))
      {
        this.startupPreferences.startMuted = Utility.toggleMuteMusic();
        this.muteMusicButton.sourceRect.X = this.muteMusicButton.sourceRect.X != 128 ? 128 : 137;
        Game1.playSound("drumkit6");
        this.startupPreferences.savePreferences();
      }
      else if (this.HasActiveUser && this.windowedButton.containsPoint(x, y))
      {
        if (!Game1.options.isCurrentlyWindowed())
        {
          Game1.options.setWindowedOption("Windowed");
          this.windowedButton.sourceRect.X = 146;
          this.startupPreferences.windowMode = 1;
        }
        else
        {
          Game1.options.setWindowedOption("Windowed Borderless");
          this.windowedButton.sourceRect.X = 155;
          this.startupPreferences.windowMode = 0;
        }
        this.startupPreferences.savePreferences();
        Game1.playSound("drumkit6");
      }
      else
      {
        if (this.logoFadeTimer > 0 && this.skipButton.containsPoint(x, y) && this.chuckleFishTimer <= 0)
        {
          if (this.logoSurprisedTimer <= 0)
          {
            this.logoSurprisedTimer = 1500;
            string cueName = "fishSlap";
            Game1.changeMusicTrack("none");
            switch (Game1.random.Next(2))
            {
              case 0:
                cueName = "Duck";
                break;
              case 1:
                cueName = "fishSlap";
                break;
            }
            Game1.playSound(cueName);
          }
          else if (this.logoSurprisedTimer > 1)
            this.logoSurprisedTimer = Math.Max(1, this.logoSurprisedTimer - 500);
        }
        if (this.chuckleFishTimer > 500)
          this.chuckleFishTimer = 500;
        if (this.logoFadeTimer > 0 || this.fadeFromWhiteTimer > 0 || this.transitioningCharacterCreationMenu)
          return;
        if (TitleMenu.subMenu != null)
        {
          if (!this.isTransitioningButtons)
            TitleMenu.subMenu.receiveLeftClick(x, y, true);
          if (TitleMenu.subMenu == null || !this.backButton.containsPoint(x, y) && !(TitleMenu.subMenu is TooManyFarmsMenu) && (!(TitleMenu.subMenu is LanguageSelectionMenu) || !TitleMenu.subMenu.readyToClose()))
            return;
          Game1.playSound("bigDeSelect");
          this.buttonsDX = -1;
          if (TitleMenu.subMenu is AboutMenu || TitleMenu.subMenu is LanguageSelectionMenu)
          {
            TitleMenu.subMenu = (IClickableMenu) null;
            this.buttonsDX = 0;
          }
          else
          {
            this.isTransitioningButtons = true;
            if (TitleMenu.subMenu is LoadGameMenu)
              this.transitioningFromLoadScreen = true;
            TitleMenu.subMenu = (IClickableMenu) null;
            Game1.changeMusicTrack("spring_day_ambient");
          }
        }
        else if (this.logoFadeTimer <= 0 && !this.titleInPosition && (double) this.logoSwipeTimer == 0.0)
        {
          this.pauseBeforeViewportRiseTimer = 0;
          this.fadeFromWhiteTimer = 0;
          this.viewportY = -999f;
          this.viewportDY = -0.01f;
          this.birds.Clear();
          this.logoSwipeTimer = 1f;
        }
        else
        {
          if (!this.alternativeTitleGraphic())
          {
            if (this.clicksOnLeaf >= 10 && Game1.random.NextDouble() < 0.001)
              Game1.playSound("junimoMeep1");
            if (this.titleInPosition && this.eRect.Contains(x, y) && this.clicksOnE < 10)
            {
              this.clicksOnE = this.clicksOnE + 1;
              Game1.playSound("woodyStep");
              if (this.clicksOnE == 10)
              {
                int num1 = this.height < 800 ? 2 : 3;
                Game1.playSound("openChest");
                List<TemporaryAnimatedSprite> tempSprites = this.tempSprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(this.titleButtonsTexture, new Rectangle(0, 491, 42, 68), new Vector2((float) (this.width / 2 - 200 * num1 + 251 * num1), (float) (-300 * num1 - (int) ((double) this.viewportY / 3.0) * num1 + 26 * num1)), false, 0.0f, Color.White);
                temporaryAnimatedSprite.scale = (float) num1;
                temporaryAnimatedSprite.animationLength = 9;
                temporaryAnimatedSprite.interval = 200f;
                int num2 = 1;
                temporaryAnimatedSprite.local = num2 != 0;
                int num3 = 1;
                temporaryAnimatedSprite.holdLastFrame = num3 != 0;
                tempSprites.Add(temporaryAnimatedSprite);
              }
            }
            else if (this.titleInPosition)
            {
              bool flag = false;
              foreach (Rectangle leafRect in this.leafRects)
              {
                if (leafRect.Contains(x, y))
                {
                  flag = true;
                  break;
                }
              }
              if (flag)
              {
                this.clicksOnLeaf = this.clicksOnLeaf + 1;
                if (this.clicksOnLeaf == 10)
                {
                  int num1 = this.height < 800 ? 2 : 3;
                  Game1.playSound("discoverMineral");
                  List<TemporaryAnimatedSprite> tempSprites1 = this.tempSprites;
                  TemporaryAnimatedSprite temporaryAnimatedSprite1 = new TemporaryAnimatedSprite(this.titleButtonsTexture, new Rectangle(264, 464, 16, 16), new Vector2((float) (this.width / 2 - 200 * num1 + 80 * num1), (float) (-300 * num1 - (int) ((double) this.viewportY / 3.0) * num1 + 10 * num1 + 2)), false, 0.0f, Color.White);
                  temporaryAnimatedSprite1.scale = (float) num1;
                  temporaryAnimatedSprite1.animationLength = 8;
                  temporaryAnimatedSprite1.interval = 80f;
                  temporaryAnimatedSprite1.totalNumberOfLoops = 999999;
                  int num2 = 1;
                  temporaryAnimatedSprite1.local = num2 != 0;
                  int num3 = 0;
                  temporaryAnimatedSprite1.holdLastFrame = num3 != 0;
                  int num4 = 200;
                  temporaryAnimatedSprite1.delayBeforeAnimationStart = num4;
                  tempSprites1.Add(temporaryAnimatedSprite1);
                  List<TemporaryAnimatedSprite> tempSprites2 = this.tempSprites;
                  TemporaryAnimatedSprite temporaryAnimatedSprite2 = new TemporaryAnimatedSprite(this.titleButtonsTexture, new Rectangle(136, 448, 16, 16), new Vector2((float) (this.width / 2 - 200 * num1 + 80 * num1), (float) (-300 * num1 - (int) ((double) this.viewportY / 3.0) * num1 + 10 * num1)), false, 0.0f, Color.White);
                  temporaryAnimatedSprite2.scale = (float) num1;
                  temporaryAnimatedSprite2.animationLength = 8;
                  temporaryAnimatedSprite2.interval = 50f;
                  int num5 = 1;
                  temporaryAnimatedSprite2.local = num5 != 0;
                  int num6 = 0;
                  temporaryAnimatedSprite2.holdLastFrame = num6 != 0;
                  tempSprites2.Add(temporaryAnimatedSprite2);
                  List<TemporaryAnimatedSprite> tempSprites3 = this.tempSprites;
                  TemporaryAnimatedSprite temporaryAnimatedSprite3 = new TemporaryAnimatedSprite(this.titleButtonsTexture, new Rectangle(200, 464, 16, 16), new Vector2((float) (this.width / 2 - 200 * num1 + 178 * num1), (float) (-300 * num1 - (int) ((double) this.viewportY / 3.0) * num1 + 141 * num1 + 2)), false, 0.0f, Color.White);
                  temporaryAnimatedSprite3.scale = (float) num1;
                  temporaryAnimatedSprite3.animationLength = 4;
                  temporaryAnimatedSprite3.interval = 150f;
                  temporaryAnimatedSprite3.totalNumberOfLoops = 999999;
                  int num7 = 1;
                  temporaryAnimatedSprite3.local = num7 != 0;
                  int num8 = 0;
                  temporaryAnimatedSprite3.holdLastFrame = num8 != 0;
                  int num9 = 400;
                  temporaryAnimatedSprite3.delayBeforeAnimationStart = num9;
                  tempSprites3.Add(temporaryAnimatedSprite3);
                  List<TemporaryAnimatedSprite> tempSprites4 = this.tempSprites;
                  TemporaryAnimatedSprite temporaryAnimatedSprite4 = new TemporaryAnimatedSprite(this.titleButtonsTexture, new Rectangle(136, 448, 16, 16), new Vector2((float) (this.width / 2 - 200 * num1 + 178 * num1), (float) (-300 * num1 - (int) ((double) this.viewportY / 3.0) * num1 + 141 * num1)), false, 0.0f, Color.White);
                  temporaryAnimatedSprite4.scale = (float) num1;
                  temporaryAnimatedSprite4.animationLength = 8;
                  temporaryAnimatedSprite4.interval = 50f;
                  int num10 = 1;
                  temporaryAnimatedSprite4.local = num10 != 0;
                  int num11 = 0;
                  temporaryAnimatedSprite4.holdLastFrame = num11 != 0;
                  int num12 = 200;
                  temporaryAnimatedSprite4.delayBeforeAnimationStart = num12;
                  tempSprites4.Add(temporaryAnimatedSprite4);
                  List<TemporaryAnimatedSprite> tempSprites5 = this.tempSprites;
                  TemporaryAnimatedSprite temporaryAnimatedSprite5 = new TemporaryAnimatedSprite(this.titleButtonsTexture, new Rectangle(136, 464, 16, 16), new Vector2((float) (this.width / 2 - 200 * num1 + 294 * num1), (float) (-300 * num1 - (int) ((double) this.viewportY / 3.0) * num1 + 89 * num1 + 2)), false, 0.0f, Color.White);
                  temporaryAnimatedSprite5.scale = (float) num1;
                  temporaryAnimatedSprite5.animationLength = 4;
                  temporaryAnimatedSprite5.interval = 150f;
                  temporaryAnimatedSprite5.totalNumberOfLoops = 999999;
                  int num13 = 1;
                  temporaryAnimatedSprite5.local = num13 != 0;
                  int num14 = 0;
                  temporaryAnimatedSprite5.holdLastFrame = num14 != 0;
                  int num15 = 600;
                  temporaryAnimatedSprite5.delayBeforeAnimationStart = num15;
                  tempSprites5.Add(temporaryAnimatedSprite5);
                  List<TemporaryAnimatedSprite> tempSprites6 = this.tempSprites;
                  TemporaryAnimatedSprite temporaryAnimatedSprite6 = new TemporaryAnimatedSprite(this.titleButtonsTexture, new Rectangle(136, 448, 16, 16), new Vector2((float) (this.width / 2 - 200 * num1 + 294 * num1), (float) (-300 * num1 - (int) ((double) this.viewportY / 3.0) * num1 + 89 * num1)), false, 0.0f, Color.White);
                  temporaryAnimatedSprite6.scale = (float) num1;
                  temporaryAnimatedSprite6.animationLength = 8;
                  temporaryAnimatedSprite6.interval = 50f;
                  int num16 = 1;
                  temporaryAnimatedSprite6.local = num16 != 0;
                  int num17 = 0;
                  temporaryAnimatedSprite6.holdLastFrame = num17 != 0;
                  int num18 = 400;
                  temporaryAnimatedSprite6.delayBeforeAnimationStart = num18;
                  tempSprites6.Add(temporaryAnimatedSprite6);
                }
                else
                {
                  Game1.playSound("leafrustle");
                  int num1 = this.height < 800 ? 2 : 3;
                  for (int index = 0; index < 2; ++index)
                  {
                    List<TemporaryAnimatedSprite> tempSprites = this.tempSprites;
                    TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(355, 1199 + Game1.random.Next(-1, 2) * 16, 16, 16), new Vector2((float) (x + Game1.random.Next(-8, 9)), (float) (y + Game1.random.Next(-8, 9))), Game1.random.NextDouble() < 0.5, 0.0f, Color.White);
                    temporaryAnimatedSprite.scale = (float) num1;
                    temporaryAnimatedSprite.animationLength = 11;
                    temporaryAnimatedSprite.interval = (float) (50 + Game1.random.Next(50));
                    temporaryAnimatedSprite.totalNumberOfLoops = 999;
                    temporaryAnimatedSprite.motion = new Vector2((float) Game1.random.Next(-100, 101) / 100f, (float) (1.0 + (double) Game1.random.Next(-100, 100) / 500.0));
                    int num2 = Game1.random.NextDouble() < 0.5 ? 1 : 0;
                    temporaryAnimatedSprite.xPeriodic = num2 != 0;
                    double num3 = (double) Game1.random.Next(6000, 16000);
                    temporaryAnimatedSprite.xPeriodicLoopTime = (float) num3;
                    double num4 = (double) Game1.random.Next(Game1.tileSize, Game1.tileSize * 3);
                    temporaryAnimatedSprite.xPeriodicRange = (float) num4;
                    double num5 = 1.0 / 1000.0;
                    temporaryAnimatedSprite.alphaFade = (float) num5;
                    int num6 = 1;
                    temporaryAnimatedSprite.local = num6 != 0;
                    int num7 = 0;
                    temporaryAnimatedSprite.holdLastFrame = num7 != 0;
                    int num8 = index * 20;
                    temporaryAnimatedSprite.delayBeforeAnimationStart = num8;
                    tempSprites.Add(temporaryAnimatedSprite);
                  }
                }
              }
            }
          }
          if (!this.HasActiveUser || TitleMenu.subMenu != null && !TitleMenu.subMenu.readyToClose() || this.isTransitioningButtons)
            return;
          foreach (ClickableTextureComponent button in this.buttons)
          {
            if (button.containsPoint(x, y))
              this.performButtonAction(button.name);
          }
          if (this.aboutButton.containsPoint(x, y))
          {
            TitleMenu.subMenu = (IClickableMenu) new AboutMenu();
            Game1.playSound("newArtifact");
          }
          if (!this.languageButton.visible || !this.languageButton.containsPoint(x, y))
            return;
          TitleMenu.subMenu = (IClickableMenu) new LanguageSelectionMenu();
          Game1.playSound("newArtifact");
        }
      }
    }

    public void performButtonAction(string which)
    {
      this.whichSubMenu = which;
      if (!(which == "New"))
      {
        if (!(which == "Load"))
        {
          if (which == "Co-op" || !(which == "Exit"))
            return;
          Game1.playSound("bigDeSelect");
          Game1.changeMusicTrack("none");
          this.quitTimer = 500;
        }
        else
        {
          this.buttonsDX = 1;
          this.isTransitioningButtons = true;
          Game1.playSound("select");
        }
      }
      else
      {
        this.buttonsDX = 1;
        this.isTransitioningButtons = true;
        Game1.playSound("select");
        lock (this)
          this.numFarmsSaved = -1;
        Game1.GetNumFarmsSavedAsync((ReportNumFarms) (num =>
        {
          lock (this)
            this.numFarmsSaved = num;
        }));
      }
    }

    private void addRightLeafGust()
    {
      if (this.isTransitioningButtons || this.tempSprites.Count<TemporaryAnimatedSprite>() > 0 || this.alternativeTitleGraphic())
        return;
      int num1 = this.height < 800 ? 2 : 3;
      List<TemporaryAnimatedSprite> tempSprites = this.tempSprites;
      TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(this.titleButtonsTexture, new Rectangle(296, 187, 27, 21), new Vector2((float) (this.width / 2 - 200 * num1 + 327 * num1), (float) (-300 * num1) - this.viewportY / 3f * (float) num1 + (float) (107 * num1)), false, 0.0f, Color.White);
      temporaryAnimatedSprite.scale = (float) num1;
      int num2 = 1;
      temporaryAnimatedSprite.pingPong = num2 != 0;
      int num3 = 3;
      temporaryAnimatedSprite.animationLength = num3;
      double num4 = 100.0;
      temporaryAnimatedSprite.interval = (float) num4;
      int num5 = 3;
      temporaryAnimatedSprite.totalNumberOfLoops = num5;
      int num6 = 1;
      temporaryAnimatedSprite.local = num6 != 0;
      tempSprites.Add(temporaryAnimatedSprite);
    }

    private void addLeftLeafGust()
    {
      if (this.isTransitioningButtons || this.tempSprites.Count<TemporaryAnimatedSprite>() > 0 || this.alternativeTitleGraphic())
        return;
      int num1 = this.height < 800 ? 2 : 3;
      List<TemporaryAnimatedSprite> tempSprites = this.tempSprites;
      TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(this.titleButtonsTexture, new Rectangle(296, 208, 22, 18), new Vector2((float) (this.width / 2 - 200 * num1 + 16 * num1), (float) (-300 * num1) - this.viewportY / 3f * (float) num1 + (float) (16 * num1)), false, 0.0f, Color.White);
      temporaryAnimatedSprite.scale = (float) num1;
      int num2 = 1;
      temporaryAnimatedSprite.pingPong = num2 != 0;
      int num3 = 3;
      temporaryAnimatedSprite.animationLength = num3;
      double num4 = 100.0;
      temporaryAnimatedSprite.interval = (float) num4;
      int num5 = 3;
      temporaryAnimatedSprite.totalNumberOfLoops = num5;
      int num6 = 1;
      temporaryAnimatedSprite.local = num6 != 0;
      tempSprites.Add(temporaryAnimatedSprite);
    }

    public void createdNewCharacter(bool skipIntro)
    {
      Game1.playSound("smallSelect");
      TitleMenu.subMenu = (IClickableMenu) null;
      this.transitioningCharacterCreationMenu = true;
      if (!skipIntro)
        return;
      Game1.loadForNewGame(false);
      Game1.saveOnNewDay = true;
      Game1.player.eventsSeen.Add(60367);
      Game1.player.currentLocation = (GameLocation) Utility.getHomeOfFarmer(Game1.player);
      Game1.player.position = new Vector2(7f, 9f) * (float) Game1.tileSize;
      Game1.NewDay(0.0f);
      Game1.exitActiveMenu();
      Game1.setGameMode((byte) 3);
    }

    public override void update(GameTime time)
    {
      if (TitleMenu.windowNumber > (this.startupPreferences.windowMode == 1 ? 0 : 1))
      {
        if (TitleMenu.windowNumber % 2 == 0)
          Game1.options.setWindowedOption("Windowed Borderless");
        else
          Game1.options.setWindowedOption("Windowed");
        --TitleMenu.windowNumber;
        if (TitleMenu.windowNumber == (this.startupPreferences.windowMode == 1 ? 0 : 1))
          Game1.options.setWindowedOption(this.startupPreferences.windowMode);
      }
      base.update(time);
      if (TitleMenu.subMenu != null)
        TitleMenu.subMenu.update(time);
      if (this.transitioningCharacterCreationMenu)
      {
        this.globalCloudAlpha = this.globalCloudAlpha - (float) time.ElapsedGameTime.Milliseconds * (1f / 1000f);
        if ((double) this.globalCloudAlpha <= 0.0)
        {
          this.transitioningCharacterCreationMenu = false;
          this.globalCloudAlpha = 0.0f;
          TitleMenu.subMenu = (IClickableMenu) null;
          Game1.currentMinigame = (IMinigame) new GrandpaStory();
          Game1.exitActiveMenu();
          Game1.setGameMode((byte) 3);
        }
      }
      if (this.quitTimer > 0)
      {
        this.quitTimer = this.quitTimer - time.ElapsedGameTime.Milliseconds;
        if (this.quitTimer <= 0)
        {
          Game1.quit = true;
          Game1.exitActiveMenu();
        }
      }
      TimeSpan elapsedGameTime;
      if (this.chuckleFishTimer > 0)
        this.chuckleFishTimer = this.chuckleFishTimer - time.ElapsedGameTime.Milliseconds;
      else if (this.logoFadeTimer > 0)
      {
        if (this.logoSurprisedTimer > 0)
        {
          this.logoSurprisedTimer = this.logoSurprisedTimer - time.ElapsedGameTime.Milliseconds;
          if (this.logoSurprisedTimer <= 0)
            this.logoFadeTimer = 1;
        }
        else
        {
          int logoFadeTimer1 = this.logoFadeTimer;
          int logoFadeTimer2 = this.logoFadeTimer;
          elapsedGameTime = time.ElapsedGameTime;
          int milliseconds = elapsedGameTime.Milliseconds;
          this.logoFadeTimer = logoFadeTimer2 - milliseconds;
          if (this.logoFadeTimer < 4000 & logoFadeTimer1 >= 4000)
            Game1.playSound("mouseClick");
          if (this.logoFadeTimer < 2500 & logoFadeTimer1 >= 2500)
            Game1.playSound("mouseClick");
          if (this.logoFadeTimer < 2000 & logoFadeTimer1 >= 2000)
            Game1.playSound("mouseClick");
          if (this.logoFadeTimer <= 0)
            Game1.changeMusicTrack("MainTheme");
        }
      }
      else if (this.fadeFromWhiteTimer > 0)
      {
        this.fadeFromWhiteTimer = this.fadeFromWhiteTimer - time.ElapsedGameTime.Milliseconds;
        if (this.fadeFromWhiteTimer <= 0)
          this.pauseBeforeViewportRiseTimer = 3500;
      }
      else if (this.pauseBeforeViewportRiseTimer > 0)
      {
        this.pauseBeforeViewportRiseTimer = this.pauseBeforeViewportRiseTimer - time.ElapsedGameTime.Milliseconds;
        if (this.pauseBeforeViewportRiseTimer <= 0)
          this.viewportDY = -0.05f;
      }
      this.viewportY = this.viewportY + this.viewportDY;
      if ((double) this.viewportDY < 0.0)
        this.viewportDY = this.viewportDY - 3f / 500f;
      if ((double) this.viewportY <= -1000.0)
      {
        if ((double) this.viewportDY != 0.0)
        {
          this.logoSwipeTimer = 1000f;
          this.showButtonsTimer = 250;
        }
        this.viewportDY = 0.0f;
      }
      if ((double) this.logoSwipeTimer > 0.0)
      {
        double logoSwipeTimer = (double) this.logoSwipeTimer;
        elapsedGameTime = time.ElapsedGameTime;
        double milliseconds = (double) elapsedGameTime.Milliseconds;
        this.logoSwipeTimer = (float) (logoSwipeTimer - milliseconds);
        if ((double) this.logoSwipeTimer <= 0.0)
        {
          this.addLeftLeafGust();
          this.addRightLeafGust();
          this.titleInPosition = true;
          int num = this.height < 800 ? 2 : 3;
          this.eRect = new Rectangle(this.width / 2 - 200 * num + 251 * num, -300 * num - (int) ((double) this.viewportY / 3.0) * num + 26 * num, 42 * num, 68 * num);
          this.populateLeafRects();
        }
      }
      if (this.showButtonsTimer > 0 && this.HasActiveUser)
      {
        int showButtonsTimer = this.showButtonsTimer;
        elapsedGameTime = time.ElapsedGameTime;
        int milliseconds = elapsedGameTime.Milliseconds;
        this.showButtonsTimer = showButtonsTimer - milliseconds;
        if (this.showButtonsTimer <= 0 && this.buttonsToShow < 3)
        {
          this.buttonsToShow = this.buttonsToShow + 1;
          Game1.playSound("Cowboy_gunshot");
          this.showButtonsTimer = 250;
        }
      }
      if (this.titleInPosition && !this.isTransitioningButtons && (this.globalXOffset == 0 && Game1.random.NextDouble() < 0.005))
      {
        if (Game1.random.NextDouble() < 0.5)
          this.addLeftLeafGust();
        else
          this.addRightLeafGust();
      }
      if (this.titleInPosition && this.isTransitioningButtons)
      {
        int buttonsDx = this.buttonsDX;
        elapsedGameTime = time.ElapsedGameTime;
        int totalMilliseconds = (int) elapsedGameTime.TotalMilliseconds;
        int dx = buttonsDx * totalMilliseconds;
        int num1 = this.globalXOffset + dx;
        int num2 = num1 - this.width;
        if (num2 > 0)
        {
          num1 -= num2;
          dx -= num2;
        }
        this.globalXOffset = num1;
        this.moveFeatures(dx, 0);
        if (this.buttonsDX > 0 && this.globalXOffset >= this.width)
        {
          if (TitleMenu.subMenu != null)
          {
            if (TitleMenu.subMenu.readyToClose())
            {
              this.isTransitioningButtons = false;
              this.buttonsDX = 0;
            }
          }
          else if (this.whichSubMenu.Equals("Load"))
          {
            TitleMenu.subMenu = (IClickableMenu) new LoadGameMenu();
            Game1.changeMusicTrack("title_night");
            this.buttonsDX = 0;
            this.isTransitioningButtons = false;
          }
          else if (this.whichSubMenu.Equals("New") && this.numFarmsSaved != -1)
          {
            if (this.numFarmsSaved >= Game1.GetMaxNumFarmsSaved())
            {
              TitleMenu.subMenu = (IClickableMenu) new TooManyFarmsMenu();
              Game1.playSound("newArtifact");
              this.buttonsDX = 0;
              this.isTransitioningButtons = false;
            }
            else
            {
              Game1.resetPlayer();
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
              int num3 = 0;
              TitleMenu.subMenu = (IClickableMenu) new CharacterCustomization(shirtOptions, hairStyleOptions, accessoryOptions, num3 != 0);
              Game1.playSound("select");
              Game1.changeMusicTrack("CloudCountry");
              Game1.player.favoriteThing = "";
              this.buttonsDX = 0;
              this.isTransitioningButtons = false;
            }
          }
          if (!this.isTransitioningButtons)
            this.whichSubMenu = "";
        }
        else if (this.buttonsDX < 0 && this.globalXOffset <= 0)
        {
          this.globalXOffset = 0;
          this.isTransitioningButtons = false;
          this.buttonsDX = 0;
          this.setUpIcons();
          this.whichSubMenu = "";
          this.transitioningFromLoadScreen = false;
        }
      }
      for (int index1 = this.bigClouds.Count - 1; index1 >= 0; --index1)
      {
        List<float> bigClouds1 = this.bigClouds;
        int index2 = index1;
        bigClouds1[index2] = bigClouds1[index2] - 0.1f;
        List<float> bigClouds2 = this.bigClouds;
        int index3 = index1;
        List<float> floatList = bigClouds2;
        int index4 = index3;
        double num1 = (double) bigClouds2[index3];
        int buttonsDx = this.buttonsDX;
        elapsedGameTime = time.ElapsedGameTime;
        int milliseconds = elapsedGameTime.Milliseconds;
        double num2 = (double) (buttonsDx * milliseconds / 2);
        double num3 = num1 + num2;
        floatList[index4] = (float) num3;
        if ((double) this.bigClouds[index1] < -1536.0)
          this.bigClouds[index1] = (float) this.width;
      }
      for (int index1 = this.smallClouds.Count - 1; index1 >= 0; --index1)
      {
        List<float> smallClouds1 = this.smallClouds;
        int index2 = index1;
        smallClouds1[index2] = smallClouds1[index2] - 0.3f;
        List<float> smallClouds2 = this.smallClouds;
        int index3 = index1;
        List<float> floatList = smallClouds2;
        int index4 = index3;
        double num1 = (double) smallClouds2[index3];
        int buttonsDx = this.buttonsDX;
        elapsedGameTime = time.ElapsedGameTime;
        int milliseconds = elapsedGameTime.Milliseconds;
        double num2 = (double) (buttonsDx * milliseconds / 2);
        double num3 = num1 + num2;
        floatList[index4] = (float) num3;
        if ((double) this.smallClouds[index1] < -384.0)
          this.smallClouds[index1] = (float) this.width;
      }
      for (int index = this.tempSprites.Count - 1; index >= 0; --index)
      {
        if (this.tempSprites[index].update(time))
          this.tempSprites.RemoveAt(index);
      }
      for (int index = this.birds.Count - 1; index >= 0; --index)
      {
        this.birds[index].position.Y -= this.viewportDY * 2f;
        if (this.birds[index].update(time))
          this.birds.RemoveAt(index);
      }
    }

    private void moveFeatures(int dx, int dy)
    {
      foreach (TemporaryAnimatedSprite tempSprite in this.tempSprites)
      {
        tempSprite.position.X += (float) dx;
        tempSprite.position.Y += (float) dy;
      }
      foreach (ClickableTextureComponent button in this.buttons)
      {
        button.bounds.X += dx;
        button.bounds.Y += dy;
      }
    }

    public override void receiveScrollWheelAction(int direction)
    {
      base.receiveScrollWheelAction(direction);
      if (TitleMenu.subMenu == null)
        return;
      TitleMenu.subMenu.receiveScrollWheelAction(direction);
    }

    public override void performHoverAction(int x, int y)
    {
      base.performHoverAction(x, y);
      this.muteMusicButton.tryHover(x, y, 0.1f);
      if (TitleMenu.subMenu != null)
      {
        TitleMenu.subMenu.performHoverAction(x, y);
        if (this.backButton.containsPoint(x, y))
        {
          if (this.backButton.sourceRect.Y == 252)
            Game1.playSound("Cowboy_Footstep");
          this.backButton.sourceRect.Y = 279;
        }
        else
          this.backButton.sourceRect.Y = 252;
        this.backButton.tryHover(x, y, 0.25f);
      }
      else
      {
        if (!this.titleInPosition || !this.HasActiveUser)
          return;
        foreach (ClickableTextureComponent button in this.buttons)
        {
          if (button.containsPoint(x, y))
          {
            if (button.sourceRect.Y == 187)
              Game1.playSound("Cowboy_Footstep");
            button.sourceRect.Y = 245;
          }
          else
            button.sourceRect.Y = 187;
          button.tryHover(x, y, 0.25f);
        }
        this.aboutButton.tryHover(x, y, 0.25f);
        if (this.aboutButton.containsPoint(x, y))
        {
          if (this.aboutButton.sourceRect.X == 8)
            Game1.playSound("Cowboy_Footstep");
          this.aboutButton.sourceRect.X = 30;
        }
        else
          this.aboutButton.sourceRect.X = 8;
        if (!this.languageButton.visible)
          return;
        this.languageButton.tryHover(x, y, 0.25f);
        if (this.languageButton.containsPoint(x, y))
        {
          if (this.languageButton.sourceRect.X == 52)
            Game1.playSound("Cowboy_Footstep");
          this.languageButton.sourceRect.X = 79;
        }
        else
          this.languageButton.sourceRect.X = 52;
      }
    }

    public override void draw(SpriteBatch b)
    {
      b.Draw(Game1.staminaRect, new Rectangle(0, 0, this.width, this.height), new Color(64, 136, 248));
      b.Draw(Game1.mouseCursors, new Rectangle(0, (int) (-900.0 - (double) this.viewportY * 0.660000026226044), this.width, 900 + this.height - 360), new Rectangle?(new Rectangle(703, 1912, 1, 264)), Color.White);
      if (!this.whichSubMenu.Equals("Load"))
        b.Draw(Game1.mouseCursors, new Vector2(-30f, (float) (-1080.0 - (double) this.viewportY * 0.660000026226044)), new Rectangle?(new Rectangle(0, 1453, 638, 195)), Color.White * (float) (1.0 - (double) this.globalXOffset / 1200.0), 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.8f);
      foreach (float bigCloud in this.bigClouds)
        b.Draw(this.cloudsTexture, new Vector2(bigCloud, (float) (this.height - 750) - this.viewportY * 0.5f), new Rectangle?(new Rectangle(0, 0, 512, 337)), Color.White * this.globalCloudAlpha, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 0.01f);
      b.Draw(Game1.mouseCursors, new Vector2(-90f, (float) (this.height - 474) - this.viewportY * 0.66f), new Rectangle?(new Rectangle(0, 886, 639, 148)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 0.08f);
      b.Draw(Game1.mouseCursors, new Vector2(1827f, (float) (this.height - 474) - this.viewportY * 0.66f), new Rectangle?(new Rectangle(0, 886, 640, 148)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 0.08f);
      for (int index = 0; index < this.smallClouds.Count; ++index)
        b.Draw(this.cloudsTexture, new Vector2(this.smallClouds[index], (float) (this.height - 900 - index * 16 * 3) - this.viewportY * 0.5f), new Rectangle?(index % 2 == 0 ? new Rectangle(152, 447, 123, 55) : new Rectangle(410, 467, 63, 37)), Color.White * this.globalCloudAlpha, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 0.01f);
      b.Draw(Game1.mouseCursors, new Vector2(0.0f, (float) (this.height - 444) - this.viewportY * 1f), new Rectangle?(new Rectangle(0, 737, 639, 148)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 0.1f);
      b.Draw(Game1.mouseCursors, new Vector2(1917f, (float) (this.height - 444) - this.viewportY * 1f), new Rectangle?(new Rectangle(0, 737, 640, 148)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 0.1f);
      foreach (TemporaryAnimatedSprite bird in this.birds)
        bird.draw(b, false, 0, 0);
      b.Draw(this.cloudsTexture, new Vector2(0.0f, (float) (this.height - 426) - this.viewportY * 2f), new Rectangle?(new Rectangle(0, 554, 165, 142)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 0.2f);
      b.Draw(this.cloudsTexture, new Vector2((float) (this.width - 366), (float) (this.height - 459) - this.viewportY * 2f), new Rectangle?(new Rectangle(390, 543, 122, 153)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 0.2f);
      int num = this.height < 800 ? 2 : 3;
      if (this.whichSubMenu.Equals("Load") || TitleMenu.subMenu != null && TitleMenu.subMenu is LoadGameMenu || this.transitioningFromLoadScreen)
      {
        b.Draw(Game1.mouseCursors, new Rectangle(0, 0, this.width, this.height), new Rectangle?(new Rectangle(639, 858, 1, 100)), Color.White * ((float) this.globalXOffset / 1200f));
        b.Draw(Game1.mouseCursors, Vector2.Zero, new Rectangle?(new Rectangle(0, 1453, 638, 195)), Color.White * ((float) this.globalXOffset / 1200f), 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.8f);
        b.Draw(Game1.mouseCursors, new Vector2(0.0f, (float) (195 * Game1.pixelZoom)), new Rectangle?(new Rectangle(0, 1453, 638, 195)), Color.White * ((float) this.globalXOffset / 1200f), 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.FlipHorizontally, 0.8f);
      }
      b.Draw(this.titleButtonsTexture, new Vector2((float) (this.globalXOffset + this.width / 2 - 200 * num), (float) (-300 * num) - this.viewportY / 3f * (float) num), new Rectangle?(new Rectangle(0, 0, 400, 187)), Color.White, 0.0f, Vector2.Zero, (float) num, SpriteEffects.None, 0.2f);
      if ((double) this.logoSwipeTimer > 0.0)
        b.Draw(this.titleButtonsTexture, new Vector2((float) (this.globalXOffset + this.width / 2), (float) (-300 * num) - this.viewportY / 3f * (float) num + (float) (93 * num)), new Rectangle?(new Rectangle(0, 0, 400, 187)), Color.White, 0.0f, new Vector2(200f, 93f), (float) num + (float) ((0.5 - (double) Math.Abs((float) ((double) this.logoSwipeTimer / 1000.0 - 0.5))) * 0.100000001490116), SpriteEffects.None, 0.2f);
      if (!this.HasActiveUser && this.titleInPosition)
      {
        SpriteText.drawStringWithScrollCenteredAt(b, Game1.content.LoadString("Strings\\UI:TitleMenu_PressAToStart"), Game1.viewport.Width / 2, Game1.viewport.Height / 2 + 270, "", 1f, -1, 0, 0.88f, false);
        b.Draw(Game1.controllerMaps, new Rectangle(Game1.viewport.Width / 2 - 64, Game1.viewport.Height / 2 + 273, 52, 52), new Rectangle?(new Rectangle(542, 260, 26, 26)), Color.White);
      }
      if (TitleMenu.subMenu != null && !this.isTransitioningButtons)
      {
        this.backButton.draw(b);
        TitleMenu.subMenu.draw(b);
        if (!(TitleMenu.subMenu is CharacterCustomization))
          this.backButton.draw(b);
      }
      else if (TitleMenu.subMenu == null && this.isTransitioningButtons && (this.whichSubMenu.Equals("Load") || this.whichSubMenu.Equals("New")))
      {
        int x = Game1.tileSize + 20;
        int y = Game1.viewport.Height - Game1.tileSize;
        int width = 0;
        int tileSize = Game1.tileSize;
        Utility.makeSafe(ref x, ref y, width, tileSize);
        SpriteText.drawStringWithScrollBackground(b, Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3689"), x, y, "", 1f, -1);
      }
      else if (TitleMenu.subMenu == null && !this.isTransitioningButtons && (this.titleInPosition && !this.transitioningCharacterCreationMenu) && this.HasActiveUser)
      {
        this.aboutButton.draw(b);
        this.languageButton.draw(b);
      }
      for (int index = 0; index < this.buttonsToShow; ++index)
      {
        if (this.buttons.Count > index)
          this.buttons[index].draw(b, TitleMenu.subMenu == null || !(TitleMenu.subMenu is AboutMenu) && !(TitleMenu.subMenu is LanguageSelectionMenu) ? Color.White : Color.LightGray * 0.1f, 1f);
      }
      if (TitleMenu.subMenu == null)
      {
        foreach (TemporaryAnimatedSprite tempSprite in this.tempSprites)
          tempSprite.draw(b, false, 0, 0);
      }
      if (this.chuckleFishTimer > 0)
      {
        b.Draw(Game1.staminaRect, new Rectangle(0, 0, this.width, this.height), Color.White);
        b.Draw(this.titleButtonsTexture, new Vector2((float) (this.width / 2 - 66 * Game1.pixelZoom), (float) (this.height / 2 - 48 * Game1.pixelZoom)), new Rectangle?(new Rectangle(this.chuckleFishTimer % 200 / 100 * 132, 559, 132, 96)), Color.White * Math.Min(1f, (float) this.chuckleFishTimer / 500f), 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.2f);
      }
      else if (this.logoFadeTimer > 0 || this.fadeFromWhiteTimer > 0)
      {
        b.Draw(Game1.staminaRect, new Rectangle(0, 0, this.width, this.height), Color.White * ((float) this.fadeFromWhiteTimer / 2000f));
        b.Draw(this.titleButtonsTexture, new Vector2((float) (this.width / 2), (float) (this.height / 2 - 90)), new Rectangle?(new Rectangle(171 + (this.logoFadeTimer / 100 % 2 != 0 || this.logoSurprisedTimer > 0 ? 0 : 111), 311, 111, 60)), Color.White * (this.logoFadeTimer < 500 ? (float) this.logoFadeTimer / 500f : (this.logoFadeTimer > 4500 ? (float) (1.0 - (double) (this.logoFadeTimer - 4500) / 500.0) : 1f)), 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 0.2f);
        if (this.logoSurprisedTimer <= 0)
          b.Draw(this.titleButtonsTexture, new Vector2((float) (this.width / 2 - 261), (float) (this.height / 2 - 102)), new Rectangle?(new Rectangle(this.logoFadeTimer / 100 % 2 == 0 ? 85 : 0, 306 + (this.shades ? 69 : 0), 85, 69)), Color.White * (this.logoFadeTimer < 500 ? (float) this.logoFadeTimer / 500f : (this.logoFadeTimer > 4500 ? (float) (1.0 - (double) (this.logoFadeTimer - 4500) / 500.0) : 1f)), 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 0.2f);
        if (this.logoSurprisedTimer > 0)
          b.Draw(this.titleButtonsTexture, new Vector2((float) (this.width / 2 - 261), (float) (this.height / 2 - 102)), new Rectangle?(new Rectangle(this.logoSurprisedTimer > 800 || this.logoSurprisedTimer < 400 ? 176 : 260, 375, 85, 69)), Color.White * (this.logoSurprisedTimer < 200 ? (float) this.logoSurprisedTimer / 200f : 1f), 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 0.22f);
        if (this.startupMessage.Length > 0 && this.logoFadeTimer > 0)
          b.DrawString(Game1.smallFont, Game1.parseText(this.startupMessage, Game1.smallFont, Game1.tileSize * 10), new Vector2((float) (Game1.pixelZoom * 2), (float) Game1.viewport.Height - Game1.smallFont.MeasureString(Game1.parseText(this.startupMessage, Game1.smallFont, Game1.tileSize * 10)).Y - (float) Game1.pixelZoom), this.startupMessageColor * (this.logoFadeTimer < 500 ? (float) this.logoFadeTimer / 500f : (this.logoFadeTimer > 4500 ? (float) (1.0 - (double) (this.logoFadeTimer - 4500) / 500.0) : 1f)));
      }
      if (this.logoFadeTimer > 0)
      {
        int logoSurprisedTimer = this.logoSurprisedTimer;
      }
      if (this.quitTimer > 0)
        b.Draw(Game1.staminaRect, new Rectangle(0, 0, this.width, this.height), Color.Black * (float) (1.0 - (double) this.quitTimer / 500.0));
      if (this.HasActiveUser)
      {
        this.muteMusicButton.draw(b);
        this.windowedButton.draw(b);
      }
      this.drawMouse(b);
    }

    public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
    {
      this.width = Game1.viewport.Width;
      this.height = Game1.viewport.Height;
      if (!this.isTransitioningButtons && TitleMenu.subMenu == null)
        this.setUpIcons();
      if (TitleMenu.subMenu != null)
        TitleMenu.subMenu.gameWindowSizeChanged(oldBounds, newBounds);
      ClickableTextureComponent textureComponent1 = new ClickableTextureComponent(Game1.content.LoadString("Strings\\StringsFromCSFiles:TitleMenu.cs.11739"), new Rectangle(this.width - 198 - 48, this.height - 81 - 24, 198, 81), (string) null, "", this.titleButtonsTexture, new Rectangle(296, 252, 66, 27), 3f, false);
      int num1 = 81114;
      textureComponent1.myID = num1;
      this.backButton = textureComponent1;
      this.tempSprites.Clear();
      if (this.birds.Count > 0 && !this.titleInPosition)
      {
        for (int index = 0; index < this.birds.Count; ++index)
          this.birds[index].position = index % 2 == 0 ? new Vector2((float) (this.width - 210), (float) (this.height - 360)) : new Vector2((float) (this.width - 120), (float) (this.height - 330));
      }
      ClickableTextureComponent textureComponent2 = new ClickableTextureComponent(new Rectangle(Game1.viewport.Width - 9 * Game1.pixelZoom - Game1.tileSize / 4, Game1.tileSize / 4, 9 * Game1.pixelZoom, 9 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(Game1.options == null || Game1.options.isCurrentlyWindowed() ? 146 : 155, 384, 9, 9), (float) Game1.pixelZoom, false);
      int num2 = 81112;
      textureComponent2.myID = num2;
      int num3 = 81111;
      textureComponent2.leftNeighborID = num3;
      int num4 = 81113;
      textureComponent2.downNeighborID = num4;
      this.windowedButton = textureComponent2;
      if (!Game1.options.SnappyMenus)
        return;
      int id = this.currentlySnappedComponent != null ? this.currentlySnappedComponent.myID : 81115;
      this.populateClickableComponentList();
      this.currentlySnappedComponent = this.getComponentWithID(id);
      this.snapCursorToCurrentSnappedComponent();
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposedValue)
        return;
      if (disposing)
      {
        if (this.tempSprites != null)
          this.tempSprites.Clear();
        if (this.menuContent != null)
        {
          this.menuContent.Dispose();
          this.menuContent = (LocalizedContentManager) null;
        }
        TitleMenu.subMenu = (IClickableMenu) null;
      }
      this.disposedValue = true;
    }

    ~TitleMenu()
    {
      this.Dispose(false);
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }
  }
}
