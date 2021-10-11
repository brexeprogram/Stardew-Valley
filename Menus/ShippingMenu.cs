// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.ShippingMenu
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using System;
using System.Collections.Generic;

namespace StardewValley.Menus
{
  public class ShippingMenu : IClickableMenu
  {
    public int currentPage = -1;
    public List<ClickableTextureComponent> categories = new List<ClickableTextureComponent>();
    private List<int> categoryTotals = new List<int>();
    private List<MoneyDial> categoryDials = new List<MoneyDial>();
    private List<List<Item>> categoryItems = new List<List<Item>>();
    private int introTimer = 3500;
    public List<TemporaryAnimatedSprite> animations = new List<TemporaryAnimatedSprite>();
    public const int region_okbutton = 101;
    public const int region_forwardButton = 102;
    public const int region_backButton = 103;
    public const int farming_category = 0;
    public const int foraging_category = 1;
    public const int fishing_category = 2;
    public const int mining_category = 3;
    public const int other_category = 4;
    public const int total_category = 5;
    public const int timePerIntroCategory = 500;
    public const int outroFadeTime = 800;
    public const int smokeRate = 100;
    public const int categorylabelHeight = 25;
    public const int itemsPerCategoryPage = 9;
    public int currentTab;
    public ClickableTextureComponent okButton;
    public ClickableTextureComponent forwardButton;
    public ClickableTextureComponent backButton;
    private int categoryLabelsWidth;
    private int plusButtonWidth;
    private int itemSlotWidth;
    private int itemAndPlusButtonWidth;
    private int totalWidth;
    private int centerX;
    private int centerY;
    private int outroFadeTimer;
    private int outroPauseBeforeDateChange;
    private int finalOutroTimer;
    private int smokeTimer;
    private int dayPlaqueY;
    private float weatherX;
    private bool outro;
    private bool newDayPlaque;
    private bool savedYet;
    private SaveGameMenu saveGameMenu;

    public ShippingMenu(List<Item> items)
      : base(Game1.viewport.Width / 2 - 640, Game1.viewport.Height / 2 - 360, 1280, 720, false)
    {
      this.parseItems(items);
      if (!Game1.wasRainingYesterday)
        Game1.changeMusicTrack(Game1.currentSeason.Equals("summer") ? "nightTime" : "none");
      this.categoryLabelsWidth = Game1.tileSize * 8;
      this.plusButtonWidth = 10 * Game1.pixelZoom;
      this.itemSlotWidth = 24 * Game1.pixelZoom;
      this.itemAndPlusButtonWidth = this.plusButtonWidth + this.itemSlotWidth + 2 * Game1.pixelZoom;
      this.totalWidth = this.categoryLabelsWidth + this.itemAndPlusButtonWidth;
      this.centerX = Game1.viewport.Width / 2;
      this.centerY = Game1.viewport.Height / 2;
      int num1 = -1;
      for (int index = 0; index < 6; ++index)
      {
        List<ClickableTextureComponent> categories = this.categories;
        ClickableTextureComponent textureComponent = new ClickableTextureComponent("", new Rectangle(this.centerX + this.totalWidth / 2 - this.plusButtonWidth, this.centerY - 25 * Game1.pixelZoom * 3 + index * 27 * Game1.pixelZoom, this.plusButtonWidth, 11 * Game1.pixelZoom), "", this.getCategoryName(index), Game1.mouseCursors, new Rectangle(392, 361, 10, 11), (float) Game1.pixelZoom, false);
        int num2 = index >= 5 ? 0 : (this.categoryItems[index].Count > 0 ? 1 : 0);
        textureComponent.visible = num2 != 0;
        int num3 = index;
        textureComponent.myID = num3;
        int num4 = index < 4 ? index + 1 : 101;
        textureComponent.downNeighborID = num4;
        int num5 = index > 0 ? num1 : -1;
        textureComponent.upNeighborID = num5;
        int num6 = 1;
        textureComponent.upNeighborImmutable = num6 != 0;
        categories.Add(textureComponent);
        num1 = index >= 5 || this.categoryItems[index].Count <= 0 ? num1 : index;
      }
      this.dayPlaqueY = this.categories[0].bounds.Y - Game1.tileSize * 2;
      Rectangle bounds = new Rectangle(this.centerX + this.totalWidth / 2 - this.itemAndPlusButtonWidth + Game1.tileSize / 2, this.centerY + 25 * Game1.pixelZoom * 3 - Game1.tileSize, Game1.tileSize, Game1.tileSize);
      ClickableTextureComponent textureComponent1 = new ClickableTextureComponent(Game1.content.LoadString("Strings\\StringsFromCSFiles:ShippingMenu.cs.11382"), bounds, (string) null, Game1.content.LoadString("Strings\\StringsFromCSFiles:ShippingMenu.cs.11382"), Game1.mouseCursors, new Rectangle(128, 256, 64, 64), 1f, false);
      int num7 = 101;
      textureComponent1.myID = num7;
      int num8 = num1;
      textureComponent1.upNeighborID = num8;
      this.okButton = textureComponent1;
      if (Game1.options.gamepadControls)
      {
        Mouse.SetPosition(bounds.Center.X, bounds.Center.Y);
        Game1.lastCursorMotionWasMouse = false;
      }
      ClickableTextureComponent textureComponent2 = new ClickableTextureComponent("", new Rectangle(this.xPositionOnScreen + Game1.tileSize / 2, this.yPositionOnScreen + this.height - 16 * Game1.pixelZoom, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), (string) null, "", Game1.mouseCursors, new Rectangle(352, 495, 12, 11), (float) Game1.pixelZoom, false);
      int num9 = 103;
      textureComponent2.myID = num9;
      int num10 = -7777;
      textureComponent2.rightNeighborID = num10;
      this.backButton = textureComponent2;
      ClickableTextureComponent textureComponent3 = new ClickableTextureComponent("", new Rectangle(this.xPositionOnScreen + this.width - Game1.tileSize / 2 - 12 * Game1.pixelZoom, this.yPositionOnScreen + this.height - 16 * Game1.pixelZoom, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), (string) null, "", Game1.mouseCursors, new Rectangle(365, 495, 12, 11), (float) Game1.pixelZoom, false);
      int num11 = 102;
      textureComponent3.myID = num11;
      int num12 = 103;
      textureComponent3.leftNeighborID = num12;
      this.forwardButton = textureComponent3;
      if (Game1.dayOfMonth == 25 && Game1.currentSeason.Equals("winter"))
      {
        Vector2 position = new Vector2((float) Game1.viewport.Width, (float) Game1.random.Next(0, 200));
        Rectangle sourceRect = new Rectangle(640, 800, 32, 16);
        int numberOfLoops = 1000;
        this.animations.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, sourceRect, 80f, 2, numberOfLoops, position, false, false, 0.01f, 0.0f, Color.White, 4f, 0.0f, 0.0f, 0.0f, true)
        {
          motion = new Vector2(-4f, 0.0f),
          delayBeforeAnimationStart = 3000
        });
      }
      Game1.stats.checkForShippingAchievements();
      if (!Game1.player.achievements.Contains(34) && Utility.hasFarmerShippedAllItems())
        Game1.getAchievement(34);
      if (!Game1.options.SnappyMenus)
        return;
      this.populateClickableComponentList();
      this.snapToDefaultClickableComponent();
    }

    protected override void customSnapBehavior(int direction, int oldRegion, int oldID)
    {
      if (oldID != 103 || direction != 1 || !this.showForwardButton())
        return;
      this.currentlySnappedComponent = this.getComponentWithID(102);
      this.snapCursorToCurrentSnappedComponent();
    }

    public override void snapToDefaultClickableComponent()
    {
      this.currentlySnappedComponent = this.getComponentWithID(101);
      this.snapCursorToCurrentSnappedComponent();
    }

    public void parseItems(List<Item> items)
    {
      Utility.consolidateStacks(items);
      for (int index = 0; index < 6; ++index)
      {
        this.categoryItems.Add(new List<Item>());
        this.categoryTotals.Add(0);
        this.categoryDials.Add(new MoneyDial(7, index == 5));
      }
      foreach (Item obj in items)
      {
        if (obj is StardewValley.Object)
        {
          StardewValley.Object o = obj as StardewValley.Object;
          int categoryIndexForObject = this.getCategoryIndexForObject(o);
          this.categoryItems[categoryIndexForObject].Add((Item) o);
          List<int> categoryTotals = this.categoryTotals;
          int index = categoryIndexForObject;
          categoryTotals[index] = categoryTotals[index] + o.sellToStorePrice() * o.Stack;
          Game1.stats.itemsShipped += (uint) o.Stack;
          if (o.Category == -75 || o.Category == -79)
            Game1.stats.CropsShipped += (uint) o.Stack;
          if (o.countsForShippedCollection())
            Game1.player.shippedBasic(o.parentSheetIndex, o.stack);
        }
      }
      for (int index = 0; index < 5; ++index)
      {
        List<int> categoryTotals = this.categoryTotals;
        categoryTotals[5] = categoryTotals[5] + this.categoryTotals[index];
        this.categoryItems[5].AddRange((IEnumerable<Item>) this.categoryItems[index]);
        this.categoryDials[index].currentValue = this.categoryTotals[index];
        this.categoryDials[index].previousTargetValue = this.categoryDials[index].currentValue;
      }
      this.categoryDials[5].currentValue = this.categoryTotals[5];
      Game1.player.Money += this.categoryTotals[5];
      Game1.setRichPresence("earnings", (object) this.categoryTotals[5]);
    }

    public int getCategoryIndexForObject(StardewValley.Object o)
    {
      switch (o.parentSheetIndex)
      {
        case 414:
        case 418:
        case 406:
        case 410:
        case 296:
        case 396:
        case 402:
          return 1;
        default:
          switch (o.category)
          {
            case -20:
            case -4:
              return 2;
            case -15:
            case -12:
            case -2:
              return 3;
            case -14:
            case -6:
            case -5:
            case -80:
            case -79:
            case -75:
            case -26:
              return 0;
            case -81:
            case -27:
            case -23:
              return 1;
            default:
              return 4;
          }
      }
    }

    public string getCategoryName(int index)
    {
      switch (index)
      {
        case 0:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:ShippingMenu.cs.11389");
        case 1:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:ShippingMenu.cs.11390");
        case 2:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:ShippingMenu.cs.11391");
        case 3:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:ShippingMenu.cs.11392");
        case 4:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:ShippingMenu.cs.11393");
        case 5:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:ShippingMenu.cs.11394");
        default:
          return "";
      }
    }

    public override void update(GameTime time)
    {
      base.update(time);
      if (this.saveGameMenu != null)
      {
        this.saveGameMenu.update(time);
        if (this.saveGameMenu.quit)
        {
          this.saveGameMenu = (SaveGameMenu) null;
          this.savedYet = true;
        }
      }
      this.weatherX = this.weatherX + (float) time.ElapsedGameTime.Milliseconds * 0.03f;
      for (int index = this.animations.Count - 1; index >= 0; --index)
      {
        if (this.animations[index].update(time))
          this.animations.RemoveAt(index);
      }
      if (this.outro)
      {
        if (this.outroFadeTimer > 0)
          this.outroFadeTimer = this.outroFadeTimer - time.ElapsedGameTime.Milliseconds;
        else if (this.outroFadeTimer <= 0 && this.dayPlaqueY < this.centerY - Game1.tileSize)
        {
          if (this.animations.Count > 0)
            this.animations.Clear();
          this.dayPlaqueY = this.dayPlaqueY + (int) Math.Ceiling((double) time.ElapsedGameTime.Milliseconds * 0.349999994039536);
          if (this.dayPlaqueY >= this.centerY - Game1.tileSize)
            this.outroPauseBeforeDateChange = 700;
        }
        else if (this.outroPauseBeforeDateChange > 0)
        {
          this.outroPauseBeforeDateChange = this.outroPauseBeforeDateChange - time.ElapsedGameTime.Milliseconds;
          if (this.outroPauseBeforeDateChange <= 0)
          {
            this.newDayPlaque = true;
            Game1.playSound("newRecipe");
            if (!Game1.currentSeason.Equals("winter"))
              DelayedAction.playSoundAfterDelay(Game1.isRaining ? "rainsound" : "rooster", 1500);
            this.finalOutroTimer = 2000;
            this.animations.Clear();
            if (!this.savedYet)
            {
              if (this.saveGameMenu != null)
                return;
              this.saveGameMenu = new SaveGameMenu();
              return;
            }
          }
        }
        else if (this.finalOutroTimer > 0 && this.savedYet)
        {
          this.finalOutroTimer = this.finalOutroTimer - time.ElapsedGameTime.Milliseconds;
          if (this.finalOutroTimer <= 0)
            this.exitThisMenu(false);
        }
      }
      if (this.introTimer >= 0)
      {
        int introTimer = this.introTimer;
        this.introTimer = this.introTimer - time.ElapsedGameTime.Milliseconds * (Game1.oldMouseState.LeftButton == ButtonState.Pressed ? 3 : 1);
        int num = 500;
        if (introTimer % num < this.introTimer % 500 && this.introTimer <= 3000)
        {
          int which = 4 - this.introTimer / 500;
          if (which < 6 && which > -1)
          {
            if (this.categoryItems[which].Count > 0)
            {
              Game1.playSound(this.getCategorySound(which));
              this.categoryDials[which].currentValue = 0;
              this.categoryDials[which].previousTargetValue = 0;
            }
            else
              Game1.playSound("stoneStep");
          }
        }
        if (this.introTimer >= 0)
          return;
        Game1.playSound("money");
        this.categoryDials[5].currentValue = 0;
        this.categoryDials[5].previousTargetValue = 0;
      }
      else
      {
        if (Game1.dayOfMonth == 28 || this.outro)
          return;
        if (!Game1.wasRainingYesterday)
        {
          Vector2 position = new Vector2((float) Game1.viewport.Width, (float) Game1.random.Next(200));
          Rectangle sourceRect = new Rectangle(640, 752, 16, 16);
          int num1 = Game1.random.Next(1, 4);
          if (Game1.random.NextDouble() < 0.001)
          {
            bool flipped = Game1.random.NextDouble() < 0.5;
            if (Game1.random.NextDouble() < 0.5)
            {
              List<TemporaryAnimatedSprite> animations = this.animations;
              TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(640, 826, 16, 8), 40f, 4, 0, new Vector2((float) Game1.random.Next(this.centerX * 2), (float) Game1.random.Next(this.centerY)), false, flipped);
              temporaryAnimatedSprite.rotation = 3.141593f;
              double pixelZoom = (double) Game1.pixelZoom;
              temporaryAnimatedSprite.scale = (float) pixelZoom;
              Vector2 vector2 = new Vector2(flipped ? -8f : 8f, 8f);
              temporaryAnimatedSprite.motion = vector2;
              int num2 = 1;
              temporaryAnimatedSprite.local = num2 != 0;
              animations.Add(temporaryAnimatedSprite);
            }
            else
            {
              List<TemporaryAnimatedSprite> animations = this.animations;
              TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(258, 1680, 16, 16), 40f, 4, 0, new Vector2((float) Game1.random.Next(this.centerX * 2), (float) Game1.random.Next(this.centerY)), false, flipped);
              temporaryAnimatedSprite.scale = (float) Game1.pixelZoom;
              temporaryAnimatedSprite.motion = new Vector2(flipped ? -8f : 8f, 8f);
              int num2 = 1;
              temporaryAnimatedSprite.local = num2 != 0;
              animations.Add(temporaryAnimatedSprite);
            }
          }
          else if (Game1.random.NextDouble() < 0.0002)
          {
            position = new Vector2((float) Game1.viewport.Width, (float) Game1.random.Next(4, Game1.tileSize * 4));
            this.animations.Add(new TemporaryAnimatedSprite(Game1.staminaRect, new Rectangle(0, 0, 1, 1), 9999f, 1, 10000, position, false, false, 0.01f, 0.0f, Color.White * (0.25f + (float) Game1.random.NextDouble()), 4f, 0.0f, 0.0f, 0.0f, true)
            {
              motion = new Vector2(-0.25f, 0.0f)
            });
          }
          else if (Game1.random.NextDouble() < 5E-05)
          {
            position = new Vector2((float) Game1.viewport.Width, (float) (Game1.viewport.Height - Game1.tileSize * 3));
            for (int index = 0; index < num1; ++index)
            {
              this.animations.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, sourceRect, (float) Game1.random.Next(60, 101), 4, 100, position + new Vector2((float) ((index + 1) * Game1.random.Next(15, 18)), (float) ((index + 1) * -20)), false, false, 0.01f, 0.0f, Color.Black, 4f, 0.0f, 0.0f, 0.0f, true)
              {
                motion = new Vector2(-1f, 0.0f)
              });
              this.animations.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, sourceRect, (float) Game1.random.Next(60, 101), 4, 100, position + new Vector2((float) ((index + 1) * Game1.random.Next(15, 18)), (float) ((index + 1) * 20)), false, false, 0.01f, 0.0f, Color.Black, 4f, 0.0f, 0.0f, 0.0f, true)
              {
                motion = new Vector2(-1f, 0.0f)
              });
            }
          }
          else if (Game1.random.NextDouble() < 1E-05)
          {
            sourceRect = new Rectangle(640, 784, 16, 16);
            this.animations.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, sourceRect, 75f, 4, 1000, position, false, false, 0.01f, 0.0f, Color.White, 4f, 0.0f, 0.0f, 0.0f, true)
            {
              motion = new Vector2(-3f, 0.0f),
              yPeriodic = true,
              yPeriodicLoopTime = 1000f,
              yPeriodicRange = (float) (Game1.tileSize / 8),
              shakeIntensity = 0.5f
            });
          }
        }
        this.smokeTimer = this.smokeTimer - time.ElapsedGameTime.Milliseconds;
        if (this.smokeTimer > 0)
          return;
        this.smokeTimer = 50;
        this.animations.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(684, 1075, 1, 1), 1000f, 1, 1000, new Vector2((float) (Game1.tileSize * 2 + Game1.tileSize * 3 / 4 + Game1.pixelZoom * 3), (float) (Game1.viewport.Height - Game1.tileSize * 2 + Game1.pixelZoom * 5)), false, false)
        {
          color = Game1.wasRainingYesterday ? Color.SlateGray : Color.White,
          scale = (float) Game1.pixelZoom,
          scaleChange = 0.0f,
          alphaFade = 1f / 400f,
          motion = new Vector2(0.0f, (float) ((double) -Game1.random.Next(25, 75) / 100.0 / 4.0)),
          acceleration = new Vector2(-1f / 1000f, 0.0f)
        });
      }
    }

    public string getCategorySound(int which)
    {
      switch (which)
      {
        case 0:
          return !(this.categoryItems[0][0] as StardewValley.Object).isAnimalProduct() ? "harvest" : "cluck";
        case 1:
          return "leafrustle";
        case 2:
          return "button1";
        case 3:
          return "hammer";
        case 4:
          return "coin";
        case 5:
          return "money";
        default:
          return "stoneStep";
      }
    }

    public override void performHoverAction(int x, int y)
    {
      base.performHoverAction(x, y);
      if (this.currentPage == -1)
      {
        this.okButton.tryHover(x, y, 0.1f);
        foreach (ClickableTextureComponent category in this.categories)
          category.sourceRect.X = !category.containsPoint(x, y) ? 392 : 402;
      }
      else
      {
        this.backButton.tryHover(x, y, 0.5f);
        this.forwardButton.tryHover(x, y, 0.5f);
      }
    }

    public override void receiveKeyPress(Keys key)
    {
      if (this.introTimer <= 0 && !Game1.options.gamepadControls && (key.Equals((object) Keys.Escape) || Game1.options.doesInputListContain(Game1.options.menuButton, key)))
      {
        this.receiveLeftClick(this.okButton.bounds.Center.X, this.okButton.bounds.Center.Y, true);
      }
      else
      {
        if (this.introTimer > 0 || Game1.options.gamepadControls && Game1.options.doesInputListContain(Game1.options.menuButton, key))
          return;
        base.receiveKeyPress(key);
      }
    }

    public override void receiveGamePadButton(Buttons b)
    {
      base.receiveGamePadButton(b);
      if (b == Buttons.B && this.currentPage != -1)
      {
        if (this.currentTab == 0)
        {
          if (Game1.options.SnappyMenus)
          {
            this.currentlySnappedComponent = this.getComponentWithID(this.currentPage);
            this.snapCursorToCurrentSnappedComponent();
          }
          this.currentPage = -1;
        }
        else
          this.currentTab = this.currentTab - 1;
        Game1.playSound("shwip");
      }
      else
      {
        if (b != Buttons.Start && b != Buttons.B || (this.currentPage != -1 || this.outro))
          return;
        if (this.introTimer <= 0)
          this.okClicked();
        else
          this.introTimer = this.introTimer - Game1.currentGameTime.ElapsedGameTime.Milliseconds * 2;
      }
    }

    private void okClicked()
    {
      this.outro = true;
      this.outroFadeTimer = 800;
      Game1.playSound("bigDeSelect");
      Game1.changeMusicTrack("none");
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
      if (this.outro && !this.savedYet)
      {
        SaveGameMenu saveGameMenu = this.saveGameMenu;
      }
      else
      {
        if (this.savedYet)
          return;
        base.receiveLeftClick(x, y, playSound);
        if (this.currentPage == -1 && this.introTimer <= 0 && this.okButton.containsPoint(x, y))
          this.okClicked();
        if (this.currentPage == -1)
        {
          for (int index = 0; index < this.categories.Count; ++index)
          {
            if (this.categories[index].visible && this.categories[index].containsPoint(x, y))
            {
              this.currentPage = index;
              Game1.playSound("shwip");
              if (!Game1.options.SnappyMenus)
                break;
              this.currentlySnappedComponent = this.getComponentWithID(103);
              this.snapCursorToCurrentSnappedComponent();
              break;
            }
          }
        }
        else if (this.backButton.containsPoint(x, y))
        {
          if (this.currentTab == 0)
          {
            if (Game1.options.SnappyMenus)
            {
              this.currentlySnappedComponent = this.getComponentWithID(this.currentPage);
              this.snapCursorToCurrentSnappedComponent();
            }
            this.currentPage = -1;
          }
          else
            this.currentTab = this.currentTab - 1;
          Game1.playSound("shwip");
        }
        else
        {
          if (!this.showForwardButton() || !this.forwardButton.containsPoint(x, y))
            return;
          this.currentTab = this.currentTab + 1;
          Game1.playSound("shwip");
        }
      }
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
    }

    public bool showForwardButton()
    {
      return this.categoryItems[this.currentPage].Count > 9 * (this.currentTab + 1);
    }

    public override void draw(SpriteBatch b)
    {
      if (Game1.wasRainingYesterday)
      {
        b.Draw(Game1.mouseCursors, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), new Rectangle?(new Rectangle(639, 858, 1, 184)), Game1.currentSeason.Equals("winter") ? Color.LightSlateGray : Color.SlateGray * (float) (1.0 - (double) this.introTimer / 3500.0));
        b.Draw(Game1.mouseCursors, new Rectangle(639 * Game1.pixelZoom, 0, Game1.viewport.Width, Game1.viewport.Height), new Rectangle?(new Rectangle(639, 858, 1, 184)), Game1.currentSeason.Equals("winter") ? Color.LightSlateGray : Color.SlateGray * (float) (1.0 - (double) this.introTimer / 3500.0));
        int num1 = -61 * Game1.pixelZoom;
        while (num1 < Game1.viewport.Width + 61 * Game1.pixelZoom)
        {
          b.Draw(Game1.mouseCursors, new Vector2((float) num1 + this.weatherX / 2f % (float) (61 * Game1.pixelZoom), (float) (Game1.tileSize / 2)), new Rectangle?(new Rectangle(643, 1142, 61, 53)), Color.DarkSlateGray * 1f * (float) (1.0 - (double) this.introTimer / 3500.0), 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1f);
          num1 += 61 * Game1.pixelZoom;
        }
        b.Draw(Game1.mouseCursors, new Vector2(0.0f, (float) (Game1.viewport.Height - Game1.tileSize * 3)), new Rectangle?(new Rectangle(0, Game1.currentSeason.Equals("winter") ? 1034 : 737, 639, 48)), (Game1.currentSeason.Equals("winter") ? Color.White * 0.25f : new Color(30, 62, 50)) * (float) (0.5 - (double) this.introTimer / 3500.0), 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.FlipHorizontally, 1f);
        b.Draw(Game1.mouseCursors, new Vector2((float) (639 * Game1.pixelZoom), (float) (Game1.viewport.Height - Game1.tileSize * 3)), new Rectangle?(new Rectangle(0, Game1.currentSeason.Equals("winter") ? 1034 : 737, 639, 48)), (Game1.currentSeason.Equals("winter") ? Color.White * 0.25f : new Color(30, 62, 50)) * (float) (0.5 - (double) this.introTimer / 3500.0), 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.FlipHorizontally, 1f);
        b.Draw(Game1.mouseCursors, new Vector2(0.0f, (float) (Game1.viewport.Height - Game1.tileSize * 2)), new Rectangle?(new Rectangle(0, Game1.currentSeason.Equals("winter") ? 1034 : 737, 639, 32)), (Game1.currentSeason.Equals("winter") ? Color.White * 0.5f : new Color(30, 62, 50)) * (float) (1.0 - (double) this.introTimer / 3500.0), 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1f);
        b.Draw(Game1.mouseCursors, new Vector2((float) (639 * Game1.pixelZoom), (float) (Game1.viewport.Height - Game1.tileSize * 2)), new Rectangle?(new Rectangle(0, Game1.currentSeason.Equals("winter") ? 1034 : 737, 639, 32)), (Game1.currentSeason.Equals("winter") ? Color.White * 0.5f : new Color(30, 62, 50)) * (float) (1.0 - (double) this.introTimer / 3500.0), 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1f);
        b.Draw(Game1.mouseCursors, new Vector2((float) (Game1.tileSize * 2 + Game1.tileSize / 2), (float) (Game1.viewport.Height - Game1.tileSize * 2 + Game1.tileSize / 4 + Game1.pixelZoom * 2)), new Rectangle?(new Rectangle(653, 880, 10, 10)), Color.White * (float) (1.0 - (double) this.introTimer / 3500.0), 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1f);
        int num2 = -61 * Game1.pixelZoom;
        while (num2 < Game1.viewport.Width + 61 * Game1.pixelZoom)
        {
          b.Draw(Game1.mouseCursors, new Vector2((float) num2 + this.weatherX % (float) (61 * Game1.pixelZoom), (float) (-Game1.tileSize / 2)), new Rectangle?(new Rectangle(643, 1142, 61, 53)), Color.SlateGray * 0.85f * (float) (1.0 - (double) this.introTimer / 3500.0), 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.9f);
          num2 += 61 * Game1.pixelZoom;
        }
        foreach (TemporaryAnimatedSprite animation in this.animations)
          animation.draw(b, true, 0, 0);
        int num3 = -61 * Game1.pixelZoom;
        while (num3 < Game1.viewport.Width + 61 * Game1.pixelZoom)
        {
          b.Draw(Game1.mouseCursors, new Vector2((float) num3 + this.weatherX * 1.5f % (float) (61 * Game1.pixelZoom), (float) (-Game1.tileSize * 2)), new Rectangle?(new Rectangle(643, 1142, 61, 53)), Color.LightSlateGray * (float) (1.0 - (double) this.introTimer / 3500.0), 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.9f);
          num3 += 61 * Game1.pixelZoom;
        }
      }
      else
      {
        b.Draw(Game1.mouseCursors, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), new Rectangle?(new Rectangle(639, 858, 1, 184)), Color.White * (float) (1.0 - (double) this.introTimer / 3500.0));
        b.Draw(Game1.mouseCursors, new Rectangle(639 * Game1.pixelZoom, 0, Game1.viewport.Width, Game1.viewport.Height), new Rectangle?(new Rectangle(639, 858, 1, 184)), Color.White * (float) (1.0 - (double) this.introTimer / 3500.0));
        b.Draw(Game1.mouseCursors, new Vector2(0.0f, 0.0f), new Rectangle?(new Rectangle(0, 1453, 639, 195)), Color.White * (float) (1.0 - (double) this.introTimer / 3500.0), 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1f);
        b.Draw(Game1.mouseCursors, new Vector2((float) (639 * Game1.pixelZoom), 0.0f), new Rectangle?(new Rectangle(0, 1453, 639, 195)), Color.White * (float) (1.0 - (double) this.introTimer / 3500.0), 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1f);
        if (Game1.dayOfMonth == 28)
          b.Draw(Game1.mouseCursors, new Vector2((float) (Game1.viewport.Width - 44 * Game1.pixelZoom), (float) Game1.pixelZoom), new Rectangle?(new Rectangle(642, 835, 43, 43)), Color.White * (float) (1.0 - (double) this.introTimer / 3500.0), 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1f);
        b.Draw(Game1.mouseCursors, new Vector2(0.0f, (float) (Game1.viewport.Height - Game1.tileSize * 3)), new Rectangle?(new Rectangle(0, Game1.currentSeason.Equals("winter") ? 1034 : 737, 639, 48)), (Game1.currentSeason.Equals("winter") ? Color.White * 0.25f : new Color(0, 20, 40)) * (float) (0.649999976158142 - (double) this.introTimer / 3500.0), 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.FlipHorizontally, 1f);
        b.Draw(Game1.mouseCursors, new Vector2((float) (639 * Game1.pixelZoom), (float) (Game1.viewport.Height - Game1.tileSize * 3)), new Rectangle?(new Rectangle(0, Game1.currentSeason.Equals("winter") ? 1034 : 737, 639, 48)), (Game1.currentSeason.Equals("winter") ? Color.White * 0.25f : new Color(0, 20, 40)) * (float) (0.649999976158142 - (double) this.introTimer / 3500.0), 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.FlipHorizontally, 1f);
        b.Draw(Game1.mouseCursors, new Vector2(0.0f, (float) (Game1.viewport.Height - Game1.tileSize * 2)), new Rectangle?(new Rectangle(0, Game1.currentSeason.Equals("winter") ? 1034 : 737, 639, 32)), (Game1.currentSeason.Equals("winter") ? Color.White * 0.5f : new Color(0, 32, 20)) * (float) (1.0 - (double) this.introTimer / 3500.0), 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1f);
        b.Draw(Game1.mouseCursors, new Vector2((float) (639 * Game1.pixelZoom), (float) (Game1.viewport.Height - Game1.tileSize * 2)), new Rectangle?(new Rectangle(0, Game1.currentSeason.Equals("winter") ? 1034 : 737, 639, 32)), (Game1.currentSeason.Equals("winter") ? Color.White * 0.5f : new Color(0, 32, 20)) * (float) (1.0 - (double) this.introTimer / 3500.0), 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1f);
        b.Draw(Game1.mouseCursors, new Vector2((float) (Game1.tileSize * 2 + Game1.tileSize / 2), (float) (Game1.viewport.Height - Game1.tileSize * 2 + Game1.tileSize / 4 + Game1.pixelZoom * 2)), new Rectangle?(new Rectangle(653, 880, 10, 10)), Color.White * (float) (1.0 - (double) this.introTimer / 3500.0), 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1f);
      }
      if (!this.outro && !Game1.wasRainingYesterday)
      {
        foreach (TemporaryAnimatedSprite animation in this.animations)
          animation.draw(b, true, 0, 0);
      }
      if (this.currentPage == -1)
      {
        SpriteText.drawStringWithScrollCenteredAt(b, Utility.getYesterdaysDate(), Game1.viewport.Width / 2, this.categories[0].bounds.Y - Game1.tileSize * 2, "", 1f, -1, 0, 0.88f, false);
        int num = -5 * Game1.pixelZoom;
        int index1 = 0;
        foreach (ClickableTextureComponent category in this.categories)
        {
          if (this.introTimer < 2500 - index1 * 500)
          {
            Vector2 vector2 = category.getVector2() + new Vector2((float) (Game1.pixelZoom * 3), (float) (-Game1.pixelZoom * 2));
            if (category.visible)
            {
              category.draw(b);
              b.Draw(Game1.mouseCursors, vector2 + new Vector2((float) (-26 * Game1.pixelZoom), (float) (num + Game1.pixelZoom)), new Rectangle?(new Rectangle(293, 360, 24, 24)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.88f);
              this.categoryItems[index1][0].drawInMenu(b, vector2 + new Vector2((float) (-22 * Game1.pixelZoom), (float) (num + Game1.pixelZoom * 4)), 1f, 1f, 0.9f, false);
            }
            IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(384, 373, 18, 18), (int) ((double) vector2.X + (double) -this.itemSlotWidth - (double) this.categoryLabelsWidth - (double) (Game1.pixelZoom * 3)), (int) ((double) vector2.Y + (double) num), this.categoryLabelsWidth, 26 * Game1.pixelZoom, Color.White, (float) Game1.pixelZoom, false);
            SpriteText.drawString(b, category.hoverText, (int) vector2.X - this.itemSlotWidth - this.categoryLabelsWidth + Game1.pixelZoom * 2, (int) vector2.Y + Game1.pixelZoom, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
            for (int index2 = 0; index2 < 6; ++index2)
              b.Draw(Game1.mouseCursors, vector2 + new Vector2((float) (-this.itemSlotWidth - Game1.tileSize * 3 - Game1.pixelZoom * 6 + index2 * 6 * Game1.pixelZoom), (float) (3 * Game1.pixelZoom)), new Rectangle?(new Rectangle(355, 476, 7, 11)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.88f);
            this.categoryDials[index1].draw(b, vector2 + new Vector2((float) (-this.itemSlotWidth - Game1.tileSize * 3 - Game1.pixelZoom * 12 + Game1.pixelZoom), (float) (5 * Game1.pixelZoom)), this.categoryTotals[index1]);
            b.Draw(Game1.mouseCursors, vector2 + new Vector2((float) (-this.itemSlotWidth - Game1.tileSize - Game1.pixelZoom), (float) (3 * Game1.pixelZoom)), new Rectangle?(new Rectangle(408, 476, 9, 11)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.88f);
          }
          ++index1;
        }
        if (this.introTimer <= 0)
          this.okButton.draw(b);
      }
      else
      {
        IClickableMenu.drawTextureBox(b, Game1.viewport.Width / 2 - 640, Game1.viewport.Height / 2 - 360, 1280, 720, Color.White);
        Vector2 location = new Vector2((float) (this.xPositionOnScreen + Game1.tileSize / 2), (float) (this.yPositionOnScreen + Game1.tileSize / 2));
        for (int index = this.currentTab * 9; index < this.currentTab * 9 + 9; ++index)
        {
          if (this.categoryItems[this.currentPage].Count > index)
          {
            this.categoryItems[this.currentPage][index].drawInMenu(b, location, 1f, 1f, 1f, true);
            if (LocalizedContentManager.CurrentLanguageLatin)
            {
              SpriteText.drawString(b, this.categoryItems[this.currentPage][index].DisplayName + (this.categoryItems[this.currentPage][index].Stack > 1 ? " x" + (object) this.categoryItems[this.currentPage][index].Stack : ""), (int) location.X + Game1.tileSize + Game1.pixelZoom * 3, (int) location.Y + Game1.pixelZoom * 3, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
              string s = ".";
              int num = 0;
              while (true)
              {
                if (num < this.width - Game1.tileSize * 3 / 2 - SpriteText.getWidthOfString(this.categoryItems[this.currentPage][index].DisplayName + (this.categoryItems[this.currentPage][index].Stack > 1 ? " x" + (object) this.categoryItems[this.currentPage][index].Stack : "") + Game1.content.LoadString("Strings\\StringsFromCSFiles:LoadGameMenu.cs.11020", (object) ((this.categoryItems[this.currentPage][index] as StardewValley.Object).sellToStorePrice() * (this.categoryItems[this.currentPage][index] as StardewValley.Object).Stack))))
                {
                  s += " .";
                  num += SpriteText.getWidthOfString(" .");
                }
                else
                  break;
              }
              SpriteText.drawString(b, s, (int) location.X + Game1.tileSize * 5 / 4 + SpriteText.getWidthOfString(this.categoryItems[this.currentPage][index].DisplayName + (this.categoryItems[this.currentPage][index].Stack > 1 ? " x" + (object) this.categoryItems[this.currentPage][index].Stack : "")), (int) location.Y + Game1.tileSize / 8, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
              SpriteText.drawString(b, Game1.content.LoadString("Strings\\StringsFromCSFiles:LoadGameMenu.cs.11020", (object) ((this.categoryItems[this.currentPage][index] as StardewValley.Object).sellToStorePrice() * (this.categoryItems[this.currentPage][index] as StardewValley.Object).Stack)), (int) location.X + this.width - Game1.tileSize - SpriteText.getWidthOfString(Game1.content.LoadString("Strings\\StringsFromCSFiles:LoadGameMenu.cs.11020", (object) ((this.categoryItems[this.currentPage][index] as StardewValley.Object).sellToStorePrice() * (this.categoryItems[this.currentPage][index] as StardewValley.Object).Stack))), (int) location.Y + Game1.pixelZoom * 3, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
            }
            else
            {
              string s1 = this.categoryItems[this.currentPage][index].DisplayName + (this.categoryItems[this.currentPage][index].Stack > 1 ? " x" + (object) this.categoryItems[this.currentPage][index].Stack : ".");
              string s2 = Game1.content.LoadString("Strings\\StringsFromCSFiles:LoadGameMenu.cs.11020", (object) ((this.categoryItems[this.currentPage][index] as StardewValley.Object).sellToStorePrice() * (this.categoryItems[this.currentPage][index] as StardewValley.Object).Stack));
              int x = (int) location.X + this.width - Game1.tileSize - SpriteText.getWidthOfString(Game1.content.LoadString("Strings\\StringsFromCSFiles:LoadGameMenu.cs.11020", (object) ((this.categoryItems[this.currentPage][index] as StardewValley.Object).sellToStorePrice() * (this.categoryItems[this.currentPage][index] as StardewValley.Object).Stack)));
              SpriteText.getWidthOfString(s1 + s2);
              while (SpriteText.getWidthOfString(s1 + s2) < 1155 - Game1.tileSize / 2)
                s1 += " .";
              if (SpriteText.getWidthOfString(s1 + s2) >= 1155)
                s1 = s1.Remove(s1.Length - 1);
              SpriteText.drawString(b, s1, (int) location.X + Game1.tileSize + Game1.pixelZoom * 3, (int) location.Y + Game1.pixelZoom * 3, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
              SpriteText.drawString(b, s2, x, (int) location.Y + Game1.pixelZoom * 3, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
            }
            location.Y += (float) (Game1.tileSize + Game1.pixelZoom);
          }
        }
        this.backButton.draw(b);
        if (this.showForwardButton())
          this.forwardButton.draw(b);
      }
      if (this.outro)
      {
        b.Draw(Game1.mouseCursors, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), new Rectangle?(new Rectangle(639, 858, 1, 184)), Color.Black * (float) (1.0 - (double) this.outroFadeTimer / 800.0));
        SpriteText.drawStringWithScrollCenteredAt(b, this.newDayPlaque ? Utility.getDateString(0) : Utility.getYesterdaysDate(), Game1.viewport.Width / 2, this.dayPlaqueY, "", 1f, -1, 0, 0.88f, false);
        foreach (TemporaryAnimatedSprite animation in this.animations)
          animation.draw(b, true, 0, 0);
        if (this.finalOutroTimer > 0)
          b.Draw(Game1.staminaRect, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), new Rectangle?(new Rectangle(0, 0, 1, 1)), Color.Black * (float) (1.0 - (double) this.finalOutroTimer / 2000.0));
      }
      if (this.saveGameMenu != null)
        this.saveGameMenu.draw(b);
      if (Game1.options.SnappyMenus && (this.introTimer > 0 || this.outro))
        return;
      this.drawMouse(b);
    }
  }
}
