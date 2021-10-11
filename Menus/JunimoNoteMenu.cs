// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.JunimoNoteMenu
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Characters;
using StardewValley.Locations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Menus
{
  public class JunimoNoteMenu : IClickableMenu
  {
    public static bool canClick = true;
    public static string hoverText = "";
    public static List<TemporaryAnimatedSprite> tempSprites = new List<TemporaryAnimatedSprite>();
    public List<Bundle> bundles = new List<Bundle>();
    public List<ClickableTextureComponent> ingredientSlots = new List<ClickableTextureComponent>();
    public List<ClickableTextureComponent> ingredientList = new List<ClickableTextureComponent>();
    public List<ClickableTextureComponent> otherClickableComponents = new List<ClickableTextureComponent>();
    public const int region_ingredientSlotModifier = 250;
    public const int region_bundleModifier = 505;
    public const int region_areaNextButton = 101;
    public const int region_areaBackButton = 102;
    public const int region_backButton = 103;
    public const int region_purchaseButton = 104;
    public const int region_presentButton = 105;
    public Texture2D noteTexture;
    private bool specificBundlePage;
    public const int baseWidth = 320;
    public const int baseHeight = 180;
    public InventoryMenu inventory;
    private Item heldItem;
    private Item hoveredItem;
    private int whichArea;
    public static ScreenSwipe screenSwipe;
    public bool fromGameMenu;
    public bool scrambledText;
    public ClickableTextureComponent backButton;
    public ClickableTextureComponent purchaseButton;
    public ClickableTextureComponent areaNextButton;
    public ClickableTextureComponent areaBackButton;
    public ClickableAnimatedComponent presentButton;
    private Bundle currentPageBundle;

    public JunimoNoteMenu(bool fromGameMenu, int area = 1, bool fromThisMenu = false)
      : base(Game1.viewport.Width / 2 - 320 * Game1.pixelZoom / 2, Game1.viewport.Height / 2 - 180 * Game1.pixelZoom / 2, 320 * Game1.pixelZoom, 180 * Game1.pixelZoom, true)
    {
      CommunityCenter locationFromName = Game1.getLocationFromName("CommunityCenter") as CommunityCenter;
      if (fromGameMenu && !fromThisMenu)
      {
        for (int area1 = 0; area1 < locationFromName.areasComplete.Length; ++area1)
        {
          if (locationFromName.shouldNoteAppearInArea(area1) && !locationFromName.areasComplete[area1])
          {
            area = area1;
            this.whichArea = area;
            break;
          }
        }
      }
      this.setUpMenu(area, (Dictionary<int, bool[]>) locationFromName.bundles);
      Game1.player.forceCanMove();
      ClickableTextureComponent textureComponent1 = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width - Game1.tileSize * 2, this.yPositionOnScreen, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(365, 495, 12, 11), (float) Game1.pixelZoom, false);
      int num1 = 0;
      textureComponent1.visible = num1 != 0;
      int num2 = 101;
      textureComponent1.myID = num2;
      int num3 = 102;
      textureComponent1.leftNeighborID = num3;
      this.areaNextButton = textureComponent1;
      ClickableTextureComponent textureComponent2 = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize, this.yPositionOnScreen, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(352, 495, 12, 11), (float) Game1.pixelZoom, false);
      int num4 = 0;
      textureComponent2.visible = num4 != 0;
      int num5 = 102;
      textureComponent2.myID = num5;
      int num6 = 101;
      textureComponent2.rightNeighborID = num6;
      this.areaBackButton = textureComponent2;
      for (int index = 0; index < 6; ++index)
      {
        if (locationFromName.shouldNoteAppearInArea((area + index) % 6))
          this.areaNextButton.visible = true;
      }
      for (int index = 0; index < 6; ++index)
      {
        int area1 = area - index;
        if (area1 == -1)
          area1 = 5;
        if (locationFromName.shouldNoteAppearInArea(area1))
          this.areaBackButton.visible = true;
      }
      this.fromGameMenu = fromGameMenu;
      foreach (Bundle bundle in this.bundles)
        bundle.depositsAllowed = false;
      if (!Game1.options.SnappyMenus)
        return;
      this.populateClickableComponentList();
      this.snapToDefaultClickableComponent();
    }

    public JunimoNoteMenu(int whichArea, Dictionary<int, bool[]> bundlesComplete)
      : base(Game1.viewport.Width / 2 - 320 * Game1.pixelZoom / 2, Game1.viewport.Height / 2 - 180 * Game1.pixelZoom / 2, 320 * Game1.pixelZoom, 180 * Game1.pixelZoom, true)
    {
      this.setUpMenu(whichArea, bundlesComplete);
      if (!Game1.options.SnappyMenus)
        return;
      this.populateClickableComponentList();
      this.snapToDefaultClickableComponent();
    }

    public override void snapToDefaultClickableComponent()
    {
      this.currentlySnappedComponent = this.getComponentWithID(505);
      this.snapCursorToCurrentSnappedComponent();
    }

    protected override void customSnapBehavior(int direction, int oldRegion, int oldID)
    {
      if (oldID - 505 < 0 || oldID - 505 >= 10 || this.currentlySnappedComponent == null)
        return;
      int num1 = -1;
      int num2 = 999999;
      Point center1 = this.currentlySnappedComponent.bounds.Center;
      for (int index = 0; index < this.bundles.Count; ++index)
      {
        if (this.bundles[index].myID != oldID)
        {
          int num3 = 999999;
          Point center2 = this.bundles[index].bounds.Center;
          switch (direction)
          {
            case 0:
              if (center2.Y < center1.Y)
              {
                num3 = center1.Y - center2.Y + Math.Abs(center1.X - center2.X) * 3;
                break;
              }
              break;
            case 1:
              if (center2.X > center1.X)
              {
                num3 = center2.X - center1.X + Math.Abs(center1.Y - center2.Y) * 3;
                break;
              }
              break;
            case 2:
              if (center2.Y > center1.Y)
              {
                num3 = center2.Y - center1.Y + Math.Abs(center1.X - center2.X) * 3;
                break;
              }
              break;
            case 3:
              if (center2.X < center1.X)
              {
                num3 = center1.X - center2.X + Math.Abs(center1.Y - center2.Y) * 3;
                break;
              }
              break;
          }
          if (num3 < 10000 && num3 < num2)
          {
            num2 = num3;
            num1 = index;
          }
        }
      }
      if (num1 != -1)
      {
        this.currentlySnappedComponent = this.getComponentWithID(num1 + 505);
        this.snapCursorToCurrentSnappedComponent();
      }
      else
      {
        switch (direction)
        {
          case 1:
            if (this.areaNextButton == null)
              break;
            this.currentlySnappedComponent = (ClickableComponent) this.areaNextButton;
            this.snapCursorToCurrentSnappedComponent();
            this.areaNextButton.leftNeighborID = oldID;
            break;
          case 2:
            if (this.presentButton == null)
              break;
            this.currentlySnappedComponent = (ClickableComponent) this.presentButton;
            this.snapCursorToCurrentSnappedComponent();
            this.presentButton.upNeighborID = oldID;
            break;
          case 3:
            if (this.areaBackButton == null)
              break;
            this.currentlySnappedComponent = (ClickableComponent) this.areaBackButton;
            this.snapCursorToCurrentSnappedComponent();
            this.areaBackButton.rightNeighborID = oldID;
            break;
        }
      }
    }

    public void setUpMenu(int whichArea, Dictionary<int, bool[]> bundlesComplete)
    {
      this.noteTexture = Game1.temporaryContent.Load<Texture2D>("LooseSprites\\JunimoNote");
      if (!Game1.player.hasOrWillReceiveMail("seenJunimoNote"))
      {
        Game1.player.removeQuest(26);
        Game1.player.mailReceived.Add("seenJunimoNote");
      }
      if (!Game1.player.hasOrWillReceiveMail("wizardJunimoNote"))
        Game1.addMailForTomorrow("wizardJunimoNote", false, false);
      this.scrambledText = !Game1.player.hasOrWillReceiveMail("canReadJunimoText");
      JunimoNoteMenu.tempSprites.Clear();
      this.whichArea = whichArea;
      this.inventory = new InventoryMenu(this.xPositionOnScreen + 32 * Game1.pixelZoom, this.yPositionOnScreen + 35 * Game1.pixelZoom, true, (List<Item>) null, new InventoryMenu.highlightThisItem(Utility.highlightSmallObjects), 36, 6, Game1.pixelZoom * 2, 2 * Game1.pixelZoom, false)
      {
        capacity = 36
      };
      Dictionary<string, string> dictionary = Game1.content.Load<Dictionary<string, string>>("Data\\Bundles");
      string areaNameFromNumber = CommunityCenter.getAreaNameFromNumber(whichArea);
      int whichBundle = 0;
      foreach (string key in dictionary.Keys)
      {
        if (key.Contains(areaNameFromNumber))
        {
          int int32 = Convert.ToInt32(key.Split('/')[1]);
          List<Bundle> bundles = this.bundles;
          Bundle bundle = new Bundle(int32, dictionary[key], bundlesComplete[int32], this.getBundleLocationFromNumber(whichBundle), this.noteTexture, this);
          int num1 = whichBundle + 505;
          bundle.myID = num1;
          int num2 = -7777;
          bundle.rightNeighborID = num2;
          int num3 = -7777;
          bundle.leftNeighborID = num3;
          int num4 = -7777;
          bundle.upNeighborID = num4;
          int num5 = -7777;
          bundle.downNeighborID = num5;
          int num6 = 1;
          bundle.fullyImmutable = num6 != 0;
          bundles.Add(bundle);
          ++whichBundle;
        }
      }
      ClickableTextureComponent textureComponent = new ClickableTextureComponent("Back", new Rectangle(this.xPositionOnScreen + IClickableMenu.borderWidth * 2 + Game1.pixelZoom * 2, this.yPositionOnScreen + IClickableMenu.borderWidth * 2 + Game1.pixelZoom, Game1.tileSize, Game1.tileSize), (string) null, (string) null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 44, -1, -1), 1f, false);
      int num = 103;
      textureComponent.myID = num;
      this.backButton = textureComponent;
      this.checkForRewards();
      JunimoNoteMenu.canClick = true;
      Game1.playSound("shwip");
      bool flag = false;
      foreach (Bundle bundle in this.bundles)
      {
        if (!bundle.complete && !bundle.Equals((object) this.currentPageBundle))
        {
          flag = true;
          break;
        }
      }
      if (flag)
        return;
      ((CommunityCenter) Game1.getLocationFromName("CommunityCenter")).areasComplete[whichArea] = true;
      this.exitFunction = new IClickableMenu.onExit(this.restoreAreaOnExit);
      ((CommunityCenter) Game1.getLocationFromName("CommunityCenter")).areaCompleteReward(whichArea);
    }

    public override bool readyToClose()
    {
      if (this.heldItem == null)
        return !this.specificBundlePage;
      return false;
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
      if (!JunimoNoteMenu.canClick)
        return;
      base.receiveLeftClick(x, y, playSound);
      if (this.scrambledText)
        return;
      if (this.specificBundlePage)
      {
        this.heldItem = this.inventory.leftClick(x, y, this.heldItem, true);
        if (this.backButton.containsPoint(x, y) && this.heldItem == null)
        {
          this.takeDownBundleSpecificPage(this.currentPageBundle);
          Game1.playSound("shwip");
        }
        if (this.heldItem != null)
        {
          if (Game1.oldKBState.IsKeyDown(Keys.LeftShift))
          {
            for (int index = 0; index < this.ingredientSlots.Count; ++index)
            {
              if (this.ingredientSlots[index].item == null)
              {
                this.heldItem = this.currentPageBundle.tryToDepositThisItem(this.heldItem, this.ingredientSlots[index], this.noteTexture);
                this.checkIfBundleIsComplete();
                return;
              }
            }
          }
          for (int index = 0; index < this.ingredientSlots.Count; ++index)
          {
            if (this.ingredientSlots[index].containsPoint(x, y))
            {
              this.heldItem = this.currentPageBundle.tryToDepositThisItem(this.heldItem, this.ingredientSlots[index], this.noteTexture);
              this.checkIfBundleIsComplete();
            }
          }
        }
        if (this.purchaseButton != null && this.purchaseButton.containsPoint(x, y))
        {
          int stack = this.currentPageBundle.ingredients.Last<BundleIngredientDescription>().stack;
          if (Game1.player.Money >= stack)
          {
            Game1.player.Money -= stack;
            Game1.playSound("select");
            this.currentPageBundle.completionAnimation(this, true, 0);
            if (this.purchaseButton != null)
              this.purchaseButton.scale = this.purchaseButton.baseScale * 0.75f;
            ((CommunityCenter) Game1.getLocationFromName("CommunityCenter")).bundleRewards[this.currentPageBundle.bundleIndex] = true;
            (Game1.getLocationFromName("CommunityCenter") as CommunityCenter).bundles[this.currentPageBundle.bundleIndex][0] = true;
            this.checkForRewards();
            bool flag = false;
            foreach (Bundle bundle in this.bundles)
            {
              if (!bundle.complete && !bundle.Equals((object) this.currentPageBundle))
              {
                flag = true;
                break;
              }
            }
            if (!flag)
            {
              ((CommunityCenter) Game1.getLocationFromName("CommunityCenter")).areasComplete[this.whichArea] = true;
              this.exitFunction = new IClickableMenu.onExit(this.restoreAreaOnExit);
              ((CommunityCenter) Game1.getLocationFromName("CommunityCenter")).areaCompleteReward(this.whichArea);
            }
            else
            {
              Junimo junimoForArea = ((CommunityCenter) Game1.getLocationFromName("CommunityCenter")).getJunimoForArea(this.whichArea);
              if (junimoForArea != null)
                junimoForArea.bringBundleBackToHut(Bundle.getColorFromColorIndex(this.currentPageBundle.bundleColor), Game1.getLocationFromName("CommunityCenter"));
            }
          }
          else
            Game1.dayTimeMoneyBox.moneyShakeTimer = 600;
        }
        if (this.upperRightCloseButton != null && !this.readyToClose() && this.upperRightCloseButton.containsPoint(x, y))
          this.closeBundlePage();
      }
      else
      {
        foreach (Bundle bundle in this.bundles)
        {
          if (bundle.canBeClicked() && bundle.containsPoint(x, y))
          {
            this.setUpBundleSpecificPage(bundle);
            Game1.playSound("shwip");
            return;
          }
        }
        if (this.presentButton != null && this.presentButton.containsPoint(x, y))
          this.openRewardsMenu();
        if (this.fromGameMenu)
        {
          CommunityCenter locationFromName = Game1.getLocationFromName("CommunityCenter") as CommunityCenter;
          if (this.areaNextButton.containsPoint(x, y))
          {
            for (int index = 1; index < 7; ++index)
            {
              if (locationFromName.shouldNoteAppearInArea((this.whichArea + index) % 6))
              {
                Game1.activeClickableMenu = (IClickableMenu) new JunimoNoteMenu(true, (this.whichArea + index) % 6, true);
                return;
              }
            }
          }
          else if (this.areaBackButton.containsPoint(x, y))
          {
            int area = this.whichArea;
            for (int index = 1; index < 7; ++index)
            {
              --area;
              if (area == -1)
                area = 5;
              if (locationFromName.shouldNoteAppearInArea(area))
              {
                Game1.activeClickableMenu = (IClickableMenu) new JunimoNoteMenu(true, area, true);
                return;
              }
            }
          }
        }
      }
      if (this.heldItem == null || this.isWithinBounds(x, y) || !this.heldItem.canBeTrashed())
        return;
      Game1.playSound("throwDownITem");
      Game1.createItemDebris(this.heldItem, Game1.player.getStandingPosition(), Game1.player.FacingDirection, (GameLocation) null);
      this.heldItem = (Item) null;
    }

    public override void receiveGamePadButton(Buttons b)
    {
      base.receiveGamePadButton(b);
      if (!this.fromGameMenu)
        return;
      CommunityCenter locationFromName = Game1.getLocationFromName("CommunityCenter") as CommunityCenter;
      if (b == Buttons.RightTrigger)
      {
        for (int index = 1; index < 7; ++index)
        {
          if (locationFromName.shouldNoteAppearInArea((this.whichArea + index) % 6))
          {
            Game1.activeClickableMenu = (IClickableMenu) new JunimoNoteMenu(true, (this.whichArea + index) % 6, true);
            break;
          }
        }
      }
      else
      {
        if (b != Buttons.LeftTrigger)
          return;
        int area = this.whichArea;
        for (int index = 1; index < 7; ++index)
        {
          --area;
          if (area == -1)
            area = 5;
          if (locationFromName.shouldNoteAppearInArea(area))
          {
            Game1.activeClickableMenu = (IClickableMenu) new JunimoNoteMenu(true, area, true);
            break;
          }
        }
      }
    }

    public override void receiveKeyPress(Keys key)
    {
      base.receiveKeyPress(key);
      if (key.Equals((object) Keys.Delete) && this.heldItem != null && this.heldItem.canBeTrashed())
      {
        if (this.heldItem is StardewValley.Object && Game1.player.specialItems.Contains((this.heldItem as StardewValley.Object).parentSheetIndex))
          Game1.player.specialItems.Remove((this.heldItem as StardewValley.Object).parentSheetIndex);
        this.heldItem = (Item) null;
        Game1.playSound("trashcan");
      }
      if (!Game1.options.doesInputListContain(Game1.options.menuButton, key) || this.readyToClose())
        return;
      this.closeBundlePage();
    }

    private void closeBundlePage()
    {
      if (!this.specificBundlePage)
        return;
      if (this.heldItem == null)
      {
        this.takeDownBundleSpecificPage(this.currentPageBundle);
        Game1.playSound("shwip");
      }
      else
        this.heldItem = this.inventory.tryToAddItem(this.heldItem, "coin");
    }

    private void reOpenThisMenu()
    {
      Game1.activeClickableMenu = (IClickableMenu) new JunimoNoteMenu(this.whichArea, (Dictionary<int, bool[]>) ((CommunityCenter) Game1.getLocationFromName("CommunityCenter")).bundles);
    }

    private void updateIngredientSlots()
    {
      int index1 = 0;
      for (int index2 = 0; index2 < this.currentPageBundle.ingredients.Count; ++index2)
      {
        if (this.currentPageBundle.ingredients[index2].completed)
        {
          this.ingredientSlots[index1].item = (Item) new StardewValley.Object(this.currentPageBundle.ingredients[index2].index, this.currentPageBundle.ingredients[index2].stack, false, -1, this.currentPageBundle.ingredients[index2].quality);
          this.currentPageBundle.ingredientDepositAnimation(this.ingredientSlots[index1], this.noteTexture, true);
          ++index1;
        }
      }
    }

    private void openRewardsMenu()
    {
      Game1.playSound("smallSelect");
      List<Item> inventory = new List<Item>();
      Dictionary<string, string> dictionary = Game1.content.Load<Dictionary<string, string>>("Data\\Bundles");
      foreach (string key in dictionary.Keys)
      {
        if (key.Contains(CommunityCenter.getAreaNameFromNumber(this.whichArea)))
        {
          int int32 = Convert.ToInt32(key.Split('/')[1]);
          if (((CommunityCenter) Game1.getLocationFromName("CommunityCenter")).bundleRewards[int32])
          {
            Item standardTextDescription = Utility.getItemFromStandardTextDescription(dictionary[key].Split('/')[1], Game1.player, ' ');
            standardTextDescription.specialVariable = int32;
            inventory.Add(standardTextDescription);
          }
        }
      }
      Game1.activeClickableMenu = (IClickableMenu) new ItemGrabMenu(inventory, false, true, (InventoryMenu.highlightThisItem) null, (ItemGrabMenu.behaviorOnItemSelect) null, (string) null, new ItemGrabMenu.behaviorOnItemSelect(this.rewardGrabbed), false, true, true, true, false, 0, (Item) null, -1, (object) null);
      Game1.activeClickableMenu.exitFunction = this.exitFunction != null ? this.exitFunction : new IClickableMenu.onExit(this.reOpenThisMenu);
    }

    private void rewardGrabbed(Item item, Farmer who)
    {
      ((CommunityCenter) Game1.getLocationFromName("CommunityCenter")).bundleRewards[item.specialVariable] = false;
    }

    private void checkIfBundleIsComplete()
    {
      if (!this.specificBundlePage || this.currentPageBundle == null)
        return;
      int num = 0;
      foreach (ClickableComponent ingredientSlot in this.ingredientSlots)
      {
        if (ingredientSlot.item != null)
          ++num;
      }
      if (num < this.currentPageBundle.numberOfIngredientSlots)
        return;
      if (this.heldItem != null)
      {
        Game1.player.addItemToInventory(this.heldItem);
        this.heldItem = (Item) null;
      }
      for (int index = 0; index < ((CommunityCenter) Game1.getLocationFromName("CommunityCenter")).bundles[this.currentPageBundle.bundleIndex].Length; ++index)
        ((CommunityCenter) Game1.getLocationFromName("CommunityCenter")).bundles[this.currentPageBundle.bundleIndex][index] = true;
      ((CommunityCenter) Game1.getLocationFromName("CommunityCenter")).checkForNewJunimoNotes();
      JunimoNoteMenu.screenSwipe = new ScreenSwipe(0, -1f, -1);
      this.currentPageBundle.completionAnimation(this, true, 400);
      JunimoNoteMenu.canClick = false;
      ((CommunityCenter) Game1.getLocationFromName("CommunityCenter")).bundleRewards[this.currentPageBundle.bundleIndex] = true;
      bool flag = false;
      foreach (Bundle bundle in this.bundles)
      {
        if (!bundle.complete && !bundle.Equals((object) this.currentPageBundle))
        {
          flag = true;
          break;
        }
      }
      if (!flag)
      {
        ((CommunityCenter) Game1.getLocationFromName("CommunityCenter")).areasComplete[this.whichArea] = true;
        this.exitFunction = new IClickableMenu.onExit(this.restoreAreaOnExit);
        ((CommunityCenter) Game1.getLocationFromName("CommunityCenter")).areaCompleteReward(this.whichArea);
      }
      else
      {
        Junimo junimoForArea = ((CommunityCenter) Game1.getLocationFromName("CommunityCenter")).getJunimoForArea(this.whichArea);
        if (junimoForArea != null)
          junimoForArea.bringBundleBackToHut(Bundle.getColorFromColorIndex(this.currentPageBundle.bundleColor), Game1.getLocationFromName("CommunityCenter"));
      }
      this.checkForRewards();
      if (!Game1.IsMultiplayer)
        return;
      MultiplayerUtility.sendMessageToEveryone(6, string.Concat((object) this.currentPageBundle.bundleIndex), Game1.player.uniqueMultiplayerID);
    }

    private void restoreAreaOnExit()
    {
      if (this.fromGameMenu)
        return;
      ((CommunityCenter) Game1.getLocationFromName("CommunityCenter")).restoreAreaCutscene(this.whichArea);
    }

    public void checkForRewards()
    {
      foreach (string key in Game1.content.Load<Dictionary<string, string>>("Data\\Bundles").Keys)
      {
        if (key.Contains(CommunityCenter.getAreaNameFromNumber(this.whichArea)))
        {
          if (((CommunityCenter) Game1.getLocationFromName("CommunityCenter")).bundleRewards[Convert.ToInt32(key.Split('/')[1])])
          {
            this.presentButton = new ClickableAnimatedComponent(new Rectangle(this.xPositionOnScreen + 148 * Game1.pixelZoom, this.yPositionOnScreen + 128 * Game1.pixelZoom, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom), "", Game1.content.LoadString("Strings\\StringsFromCSFiles:JunimoNoteMenu.cs.10783"), new TemporaryAnimatedSprite(this.noteTexture, new Rectangle(548, 262, 18, 20), 70f, 4, 99999, new Vector2((float) -Game1.tileSize, (float) -Game1.tileSize), false, false, 0.5f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, true));
            break;
          }
        }
      }
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
      if (!JunimoNoteMenu.canClick)
        return;
      if (this.specificBundlePage)
        this.heldItem = this.inventory.rightClick(x, y, this.heldItem, true);
      if (this.specificBundlePage || !this.readyToClose())
        return;
      this.exitThisMenu(true);
    }

    public override void update(GameTime time)
    {
      foreach (Bundle bundle in this.bundles)
        bundle.update(time);
      for (int index = JunimoNoteMenu.tempSprites.Count - 1; index >= 0; --index)
      {
        if (JunimoNoteMenu.tempSprites[index].update(time))
          JunimoNoteMenu.tempSprites.RemoveAt(index);
      }
      if (this.presentButton != null)
        this.presentButton.update(time);
      if (JunimoNoteMenu.screenSwipe == null)
        return;
      JunimoNoteMenu.canClick = false;
      if (!JunimoNoteMenu.screenSwipe.update(time))
        return;
      JunimoNoteMenu.screenSwipe = (ScreenSwipe) null;
      JunimoNoteMenu.canClick = true;
    }

    public override void performHoverAction(int x, int y)
    {
      base.performHoverAction(x, y);
      if (this.scrambledText)
        return;
      JunimoNoteMenu.hoverText = "";
      if (this.specificBundlePage)
      {
        this.backButton.tryHover(x, y, 0.1f);
        this.hoveredItem = this.inventory.hover(x, y, this.heldItem);
        foreach (ClickableTextureComponent ingredient in this.ingredientList)
        {
          if (ingredient.bounds.Contains(x, y))
          {
            JunimoNoteMenu.hoverText = ingredient.hoverText;
            break;
          }
        }
        if (this.heldItem != null)
        {
          foreach (ClickableTextureComponent ingredientSlot in this.ingredientSlots)
          {
            if (ingredientSlot.bounds.Contains(x, y) && this.currentPageBundle.canAcceptThisItem(this.heldItem, ingredientSlot))
            {
              ingredientSlot.sourceRect.X = 530;
              ingredientSlot.sourceRect.Y = 262;
            }
            else
            {
              ingredientSlot.sourceRect.X = 512;
              ingredientSlot.sourceRect.Y = 244;
            }
          }
        }
        if (this.purchaseButton == null)
          return;
        this.purchaseButton.tryHover(x, y, 0.1f);
      }
      else
      {
        if (this.presentButton != null)
          JunimoNoteMenu.hoverText = this.presentButton.tryHover(x, y);
        foreach (Bundle bundle in this.bundles)
          bundle.tryHoverAction(x, y);
        if (!this.fromGameMenu)
          return;
        Game1.getLocationFromName("CommunityCenter");
        this.areaNextButton.tryHover(x, y, 0.1f);
        this.areaBackButton.tryHover(x, y, 0.1f);
      }
    }

    public override void draw(SpriteBatch b)
    {
      b.Draw(Game1.fadeToBlackRect, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), Color.Black * 0.5f);
      if (!this.specificBundlePage)
      {
        b.Draw(this.noteTexture, new Vector2((float) this.xPositionOnScreen, (float) this.yPositionOnScreen), new Rectangle?(new Rectangle(0, 0, 320, 180)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.1f);
        SpriteText.drawStringHorizontallyCenteredAt(b, this.scrambledText ? CommunityCenter.getAreaEnglishDisplayNameFromNumber(this.whichArea) : CommunityCenter.getAreaDisplayNameFromNumber(this.whichArea), this.xPositionOnScreen + this.width / 2 + Game1.pixelZoom * 4, this.yPositionOnScreen + Game1.pixelZoom * 3, 999999, -1, 99999, 0.88f, 0.88f, this.scrambledText, -1);
        if (this.scrambledText)
        {
          SpriteText.drawString(b, LocalizedContentManager.CurrentLanguageLatin ? Game1.content.LoadString("Strings\\StringsFromCSFiles:JunimoNoteMenu.cs.10786") : Game1.content.LoadBaseString("Strings\\StringsFromCSFiles:JunimoNoteMenu.cs.10786"), this.xPositionOnScreen + Game1.tileSize * 3 / 2, this.yPositionOnScreen + Game1.tileSize * 3 / 2, 999999, this.width - Game1.tileSize * 3, 99999, 0.88f, 0.88f, true, -1, "", -1);
          base.draw(b);
          if (!JunimoNoteMenu.canClick)
            return;
          this.drawMouse(b);
          return;
        }
        foreach (Bundle bundle in this.bundles)
          bundle.draw(b);
        if (this.presentButton != null)
          this.presentButton.draw(b);
        foreach (TemporaryAnimatedSprite tempSprite in JunimoNoteMenu.tempSprites)
          tempSprite.draw(b, true, 0, 0);
        if (this.fromGameMenu)
        {
          if (this.areaNextButton.visible)
            this.areaNextButton.draw(b);
          if (this.areaBackButton.visible)
            this.areaBackButton.draw(b);
        }
      }
      else
      {
        b.Draw(this.noteTexture, new Vector2((float) this.xPositionOnScreen, (float) this.yPositionOnScreen), new Rectangle?(new Rectangle(320, 0, 320, 180)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.1f);
        if (this.currentPageBundle != null)
        {
          b.Draw(this.noteTexture, new Vector2((float) (this.xPositionOnScreen + 218 * Game1.pixelZoom), (float) (this.yPositionOnScreen + 22 * Game1.pixelZoom)), new Rectangle?(new Rectangle(this.currentPageBundle.bundleIndex * 16 * 2 % this.noteTexture.Width, 180 + 32 * (this.currentPageBundle.bundleIndex * 16 * 2 / this.noteTexture.Width), 32, 32)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.15f);
          SpriteFont dialogueFont1 = Game1.dialogueFont;
          string text1;
          if (Game1.player.hasOrWillReceiveMail("canReadJunimoText"))
            text1 = Game1.content.LoadString("Strings\\UI:JunimoNote_BundleName", (object) this.currentPageBundle.label);
          else
            text1 = "???";
          float x = dialogueFont1.MeasureString(text1).X;
          b.Draw(this.noteTexture, new Vector2((float) (this.xPositionOnScreen + 234 * Game1.pixelZoom - (int) x / 2 - Game1.pixelZoom * 4), (float) (this.yPositionOnScreen + 57 * Game1.pixelZoom)), new Rectangle?(new Rectangle(517, 266, 4, 17)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.1f);
          b.Draw(this.noteTexture, new Rectangle(this.xPositionOnScreen + 234 * Game1.pixelZoom - (int) x / 2, this.yPositionOnScreen + 57 * Game1.pixelZoom, (int) x, 17 * Game1.pixelZoom), new Rectangle?(new Rectangle(520, 266, 1, 17)), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.1f);
          b.Draw(this.noteTexture, new Vector2((float) (this.xPositionOnScreen + 234 * Game1.pixelZoom + (int) x / 2), (float) (this.yPositionOnScreen + 57 * Game1.pixelZoom)), new Rectangle?(new Rectangle(524, 266, 4, 17)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.1f);
          SpriteBatch spriteBatch1 = b;
          SpriteFont dialogueFont2 = Game1.dialogueFont;
          string text2;
          if (Game1.player.hasOrWillReceiveMail("canReadJunimoText"))
            text2 = Game1.content.LoadString("Strings\\UI:JunimoNote_BundleName", (object) this.currentPageBundle.label);
          else
            text2 = "???";
          Vector2 position1 = new Vector2((float) (this.xPositionOnScreen + 234 * Game1.pixelZoom) - x / 2f, (float) (this.yPositionOnScreen + 61 * Game1.pixelZoom)) + new Vector2(2f, 2f);
          Color textShadowColor1 = Game1.textShadowColor;
          spriteBatch1.DrawString(dialogueFont2, text2, position1, textShadowColor1);
          SpriteBatch spriteBatch2 = b;
          SpriteFont dialogueFont3 = Game1.dialogueFont;
          string text3;
          if (Game1.player.hasOrWillReceiveMail("canReadJunimoText"))
            text3 = Game1.content.LoadString("Strings\\UI:JunimoNote_BundleName", (object) this.currentPageBundle.label);
          else
            text3 = "???";
          Vector2 position2 = new Vector2((float) (this.xPositionOnScreen + 234 * Game1.pixelZoom) - x / 2f, (float) (this.yPositionOnScreen + 61 * Game1.pixelZoom)) + new Vector2(0.0f, 2f);
          Color textShadowColor2 = Game1.textShadowColor;
          spriteBatch2.DrawString(dialogueFont3, text3, position2, textShadowColor2);
          SpriteBatch spriteBatch3 = b;
          SpriteFont dialogueFont4 = Game1.dialogueFont;
          string text4;
          if (Game1.player.hasOrWillReceiveMail("canReadJunimoText"))
            text4 = Game1.content.LoadString("Strings\\UI:JunimoNote_BundleName", (object) this.currentPageBundle.label);
          else
            text4 = "???";
          Vector2 position3 = new Vector2((float) (this.xPositionOnScreen + 234 * Game1.pixelZoom) - x / 2f, (float) (this.yPositionOnScreen + 61 * Game1.pixelZoom)) + new Vector2(2f, 0.0f);
          Color textShadowColor3 = Game1.textShadowColor;
          spriteBatch3.DrawString(dialogueFont4, text4, position3, textShadowColor3);
          SpriteBatch spriteBatch4 = b;
          SpriteFont dialogueFont5 = Game1.dialogueFont;
          string text5;
          if (Game1.player.hasOrWillReceiveMail("canReadJunimoText"))
            text5 = Game1.content.LoadString("Strings\\UI:JunimoNote_BundleName", (object) this.currentPageBundle.label);
          else
            text5 = "???";
          Vector2 position4 = new Vector2((float) (this.xPositionOnScreen + 234 * Game1.pixelZoom) - x / 2f, (float) (this.yPositionOnScreen + 61 * Game1.pixelZoom));
          Color color = Game1.textColor * 0.9f;
          spriteBatch4.DrawString(dialogueFont5, text5, position4, color);
        }
        this.backButton.draw(b);
        if (this.purchaseButton != null)
        {
          this.purchaseButton.draw(b);
          Game1.dayTimeMoneyBox.drawMoneyBox(b, -1, -1);
        }
        foreach (TemporaryAnimatedSprite tempSprite in JunimoNoteMenu.tempSprites)
          tempSprite.draw(b, true, 0, 0);
        foreach (ClickableTextureComponent ingredientSlot in this.ingredientSlots)
        {
          if (ingredientSlot.item == null)
            ingredientSlot.draw(b, this.fromGameMenu ? Color.LightGray * 0.5f : Color.White, 0.89f);
          ingredientSlot.drawItem(b, Game1.pixelZoom, Game1.pixelZoom);
        }
        foreach (ClickableTextureComponent ingredient in this.ingredientList)
        {
          b.Draw(Game1.shadowTexture, new Vector2((float) (ingredient.bounds.Center.X - Game1.shadowTexture.Bounds.Width * Game1.pixelZoom / 2 - Game1.pixelZoom), (float) (ingredient.bounds.Center.Y + Game1.pixelZoom)), new Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.1f);
          ingredient.drawItem(b, 0, 0);
        }
        this.inventory.draw(b);
      }
      SpriteText.drawStringWithScrollCenteredAt(b, this.getRewardNameForArea(this.whichArea), this.xPositionOnScreen + this.width / 2, Math.Min(this.yPositionOnScreen + this.height + Game1.pixelZoom * 5, Game1.viewport.Height - Game1.tileSize - Game1.pixelZoom * 2), "", 1f, -1, 0, 0.88f, false);
      base.draw(b);
      Game1.mouseCursorTransparency = 1f;
      if (JunimoNoteMenu.canClick)
        this.drawMouse(b);
      if (this.heldItem != null)
        this.heldItem.drawInMenu(b, new Vector2((float) (Game1.getOldMouseX() + 16), (float) (Game1.getOldMouseY() + 16)), 1f);
      if (this.inventory.descriptionText.Length > 0)
      {
        if (this.hoveredItem != null)
          IClickableMenu.drawToolTip(b, this.hoveredItem.getDescription(), this.hoveredItem.DisplayName, this.hoveredItem, false, -1, 0, -1, -1, (CraftingRecipe) null, -1);
      }
      else
        IClickableMenu.drawHoverText(b, Game1.player.hasOrWillReceiveMail("canReadJunimoText") || JunimoNoteMenu.hoverText.Length <= 0 ? JunimoNoteMenu.hoverText : "???", Game1.dialogueFont, 0, 0, -1, (string) null, -1, (string[]) null, (Item) null, 0, -1, -1, -1, -1, 1f, (CraftingRecipe) null);
      if (JunimoNoteMenu.screenSwipe == null)
        return;
      JunimoNoteMenu.screenSwipe.draw(b);
    }

    public string getRewardNameForArea(int whichArea)
    {
      switch (whichArea)
      {
        case 0:
          return Game1.content.LoadString("Strings\\UI:JunimoNote_RewardPantry");
        case 1:
          return Game1.content.LoadString("Strings\\UI:JunimoNote_RewardCrafts");
        case 2:
          return Game1.content.LoadString("Strings\\UI:JunimoNote_RewardFishTank");
        case 3:
          return Game1.content.LoadString("Strings\\UI:JunimoNote_RewardBoiler");
        case 4:
          return Game1.content.LoadString("Strings\\UI:JunimoNote_RewardVault");
        case 5:
          return Game1.content.LoadString("Strings\\UI:JunimoNote_RewardBulletin");
        default:
          return "???";
      }
    }

    public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
    {
      base.gameWindowSizeChanged(oldBounds, newBounds);
      this.xPositionOnScreen = Game1.viewport.Width / 2 - 320 * Game1.pixelZoom / 2;
      this.yPositionOnScreen = Game1.viewport.Height / 2 - 180 * Game1.pixelZoom / 2;
      this.backButton = new ClickableTextureComponent("Back", new Rectangle(this.xPositionOnScreen + IClickableMenu.borderWidth * 2 + Game1.pixelZoom * 2, this.yPositionOnScreen + IClickableMenu.borderWidth * 2 + Game1.pixelZoom, Game1.tileSize, Game1.tileSize), (string) null, (string) null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 44, -1, -1), 1f, false);
      if (this.fromGameMenu)
      {
        ClickableTextureComponent textureComponent1 = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width - Game1.tileSize * 2, this.yPositionOnScreen, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(365, 495, 12, 11), (float) Game1.pixelZoom, false);
        int num1 = 0;
        textureComponent1.visible = num1 != 0;
        this.areaNextButton = textureComponent1;
        ClickableTextureComponent textureComponent2 = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize, this.yPositionOnScreen, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(352, 495, 12, 11), (float) Game1.pixelZoom, false);
        int num2 = 0;
        textureComponent2.visible = num2 != 0;
        this.areaBackButton = textureComponent2;
      }
      this.inventory = new InventoryMenu(this.xPositionOnScreen + 32 * Game1.pixelZoom, this.yPositionOnScreen + 35 * Game1.pixelZoom, true, (List<Item>) null, new InventoryMenu.highlightThisItem(Utility.highlightSmallObjects), Game1.player.maxItems, 6, Game1.pixelZoom * 2, 2 * Game1.pixelZoom, false);
      for (int whichBundle = 0; whichBundle < this.bundles.Count; ++whichBundle)
      {
        Point locationFromNumber = this.getBundleLocationFromNumber(whichBundle);
        this.bundles[whichBundle].bounds.X = locationFromNumber.X;
        this.bundles[whichBundle].bounds.Y = locationFromNumber.Y;
        this.bundles[whichBundle].sprite.position = new Vector2((float) locationFromNumber.X, (float) locationFromNumber.Y);
      }
      if (!this.specificBundlePage)
        return;
      int ofIngredientSlots = this.currentPageBundle.numberOfIngredientSlots;
      List<Rectangle> toAddTo1 = new List<Rectangle>();
      this.addRectangleRowsToList(toAddTo1, ofIngredientSlots, 233 * Game1.pixelZoom, 135 * Game1.pixelZoom);
      this.ingredientSlots.Clear();
      for (int index = 0; index < toAddTo1.Count; ++index)
        this.ingredientSlots.Add(new ClickableTextureComponent(toAddTo1[index], this.noteTexture, new Rectangle(512, 244, 18, 18), (float) Game1.pixelZoom, false));
      List<Rectangle> toAddTo2 = new List<Rectangle>();
      this.ingredientList.Clear();
      this.addRectangleRowsToList(toAddTo2, this.currentPageBundle.ingredients.Count, 233 * Game1.pixelZoom, 91 * Game1.pixelZoom);
      for (int index = 0; index < toAddTo2.Count; ++index)
      {
        if (Game1.objectInformation.ContainsKey(this.currentPageBundle.ingredients[index].index))
        {
          List<ClickableTextureComponent> ingredientList = this.ingredientList;
          ClickableTextureComponent textureComponent = new ClickableTextureComponent("", toAddTo2[index], "", Game1.objectInformation[this.currentPageBundle.ingredients[index].index].Split('/')[0], Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.currentPageBundle.ingredients[index].index, 16, 16), (float) Game1.pixelZoom, false);
          StardewValley.Object @object = new StardewValley.Object(this.currentPageBundle.ingredients[index].index, this.currentPageBundle.ingredients[index].stack, false, -1, this.currentPageBundle.ingredients[index].quality);
          textureComponent.item = (Item) @object;
          ingredientList.Add(textureComponent);
        }
      }
      this.updateIngredientSlots();
    }

    private void setUpBundleSpecificPage(Bundle b)
    {
      JunimoNoteMenu.tempSprites.Clear();
      this.currentPageBundle = b;
      this.specificBundlePage = true;
      if (this.whichArea == 4)
      {
        if (this.fromGameMenu)
          return;
        ClickableTextureComponent textureComponent = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + 200 * Game1.pixelZoom, this.yPositionOnScreen + 126 * Game1.pixelZoom, 65 * Game1.pixelZoom, 18 * Game1.pixelZoom), this.noteTexture, new Rectangle(517, 286, 65, 20), (float) Game1.pixelZoom, false);
        int num1 = 797;
        textureComponent.myID = num1;
        int num2 = 103;
        textureComponent.leftNeighborID = num2;
        this.purchaseButton = textureComponent;
        if (!Game1.options.SnappyMenus)
          return;
        this.currentlySnappedComponent = (ClickableComponent) this.purchaseButton;
        this.snapCursorToCurrentSnappedComponent();
      }
      else
      {
        int ofIngredientSlots = b.numberOfIngredientSlots;
        List<Rectangle> toAddTo1 = new List<Rectangle>();
        this.addRectangleRowsToList(toAddTo1, ofIngredientSlots, 233 * Game1.pixelZoom, 135 * Game1.pixelZoom);
        for (int index = 0; index < toAddTo1.Count; ++index)
        {
          List<ClickableTextureComponent> ingredientSlots = this.ingredientSlots;
          ClickableTextureComponent textureComponent = new ClickableTextureComponent(toAddTo1[index], this.noteTexture, new Rectangle(512, 244, 18, 18), (float) Game1.pixelZoom, false);
          int num1 = index + 250;
          textureComponent.myID = num1;
          int num2 = index < toAddTo1.Count - 1 ? index + 250 + 1 : -1;
          textureComponent.rightNeighborID = num2;
          int num3 = index > 0 ? index + 250 - 1 : -1;
          textureComponent.leftNeighborID = num3;
          ingredientSlots.Add(textureComponent);
        }
        List<Rectangle> toAddTo2 = new List<Rectangle>();
        this.addRectangleRowsToList(toAddTo2, b.ingredients.Count, 233 * Game1.pixelZoom, 91 * Game1.pixelZoom);
        for (int index = 0; index < toAddTo2.Count; ++index)
        {
          if (Game1.objectInformation.ContainsKey(b.ingredients[index].index))
          {
            string hoverText = Game1.objectInformation[b.ingredients[index].index].Split('/')[4];
            List<ClickableTextureComponent> ingredientList = this.ingredientList;
            ClickableTextureComponent textureComponent = new ClickableTextureComponent("", toAddTo2[index], "", hoverText, Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, b.ingredients[index].index, 16, 16), (float) Game1.pixelZoom, false);
            StardewValley.Object @object = new StardewValley.Object(b.ingredients[index].index, b.ingredients[index].stack, false, -1, b.ingredients[index].quality);
            textureComponent.item = (Item) @object;
            ingredientList.Add(textureComponent);
          }
        }
        this.updateIngredientSlots();
        if (!Game1.options.SnappyMenus)
          return;
        this.populateClickableComponentList();
        if (this.inventory != null && this.inventory.inventory != null)
        {
          for (int index = 0; index < this.inventory.inventory.Count; ++index)
          {
            if (this.inventory.inventory[index] != null)
            {
              if (this.inventory.inventory[index].downNeighborID == 101)
                this.inventory.inventory[index].downNeighborID = -1;
              if (this.inventory.inventory[index].rightNeighborID == 106)
                this.inventory.inventory[index].rightNeighborID = 250;
              if (this.inventory.inventory[index].leftNeighborID == -1)
                this.inventory.inventory[index].leftNeighborID = 103;
              if (this.inventory.inventory[index].upNeighborID >= 1000)
                this.inventory.inventory[index].upNeighborID = 103;
            }
          }
        }
        this.currentlySnappedComponent = this.getComponentWithID(0);
        this.snapCursorToCurrentSnappedComponent();
      }
    }

    private void addRectangleRowsToList(List<Rectangle> toAddTo, int numberOfItems, int centerX, int centerY)
    {
      switch (numberOfItems)
      {
        case 1:
          toAddTo.AddRange((IEnumerable<Rectangle>) this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY, 1, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
          break;
        case 2:
          toAddTo.AddRange((IEnumerable<Rectangle>) this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY, 2, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
          break;
        case 3:
          toAddTo.AddRange((IEnumerable<Rectangle>) this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY, 3, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
          break;
        case 4:
          toAddTo.AddRange((IEnumerable<Rectangle>) this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY, 4, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
          break;
        case 5:
          toAddTo.AddRange((IEnumerable<Rectangle>) this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY - 9 * Game1.pixelZoom, 3, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
          toAddTo.AddRange((IEnumerable<Rectangle>) this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY + 10 * Game1.pixelZoom, 2, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
          break;
        case 6:
          toAddTo.AddRange((IEnumerable<Rectangle>) this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY - 9 * Game1.pixelZoom, 3, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
          toAddTo.AddRange((IEnumerable<Rectangle>) this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY + 10 * Game1.pixelZoom, 3, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
          break;
        case 7:
          toAddTo.AddRange((IEnumerable<Rectangle>) this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY - 9 * Game1.pixelZoom, 4, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
          toAddTo.AddRange((IEnumerable<Rectangle>) this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY + 10 * Game1.pixelZoom, 3, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
          break;
        case 8:
          toAddTo.AddRange((IEnumerable<Rectangle>) this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY - 9 * Game1.pixelZoom, 4, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
          toAddTo.AddRange((IEnumerable<Rectangle>) this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY + 10 * Game1.pixelZoom, 4, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
          break;
        case 9:
          toAddTo.AddRange((IEnumerable<Rectangle>) this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY - 9 * Game1.pixelZoom, 5, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
          toAddTo.AddRange((IEnumerable<Rectangle>) this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY + 10 * Game1.pixelZoom, 4, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
          break;
        case 10:
          toAddTo.AddRange((IEnumerable<Rectangle>) this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY - 9 * Game1.pixelZoom, 5, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
          toAddTo.AddRange((IEnumerable<Rectangle>) this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY + 10 * Game1.pixelZoom, 5, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
          break;
        case 11:
          toAddTo.AddRange((IEnumerable<Rectangle>) this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY - 9 * Game1.pixelZoom, 6, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
          toAddTo.AddRange((IEnumerable<Rectangle>) this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY + 10 * Game1.pixelZoom, 5, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
          break;
        case 12:
          toAddTo.AddRange((IEnumerable<Rectangle>) this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY - 9 * Game1.pixelZoom, 6, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
          toAddTo.AddRange((IEnumerable<Rectangle>) this.createRowOfBoxesCenteredAt(this.xPositionOnScreen + centerX, this.yPositionOnScreen + centerY + 10 * Game1.pixelZoom, 6, 18 * Game1.pixelZoom, 18 * Game1.pixelZoom, 3 * Game1.pixelZoom));
          break;
      }
    }

    private List<Rectangle> createRowOfBoxesCenteredAt(int xStart, int yStart, int numBoxes, int boxWidth, int boxHeight, int horizontalGap)
    {
      List<Rectangle> rectangleList = new List<Rectangle>();
      int num = xStart - numBoxes * (boxWidth + horizontalGap) / 2;
      int y = yStart - boxHeight / 2;
      for (int index = 0; index < numBoxes; ++index)
        rectangleList.Add(new Rectangle(num + index * (boxWidth + horizontalGap), y, boxWidth, boxHeight));
      return rectangleList;
    }

    public void takeDownBundleSpecificPage(Bundle b = null)
    {
      if (!this.specificBundlePage)
        return;
      if (b == null)
        b = this.currentPageBundle;
      this.specificBundlePage = false;
      this.ingredientSlots.Clear();
      this.ingredientList.Clear();
      JunimoNoteMenu.tempSprites.Clear();
      this.purchaseButton = (ClickableTextureComponent) null;
      if (!Game1.options.SnappyMenus)
        return;
      this.snapToDefaultClickableComponent();
    }

    private Point getBundleLocationFromNumber(int whichBundle)
    {
      Point point = new Point(this.xPositionOnScreen, this.yPositionOnScreen);
      switch (whichBundle)
      {
        case 0:
          point.X += 148 * Game1.pixelZoom;
          point.Y += 34 * Game1.pixelZoom;
          break;
        case 1:
          point.X += 98 * Game1.pixelZoom;
          point.Y += 96 * Game1.pixelZoom;
          break;
        case 2:
          point.X += 196 * Game1.pixelZoom;
          point.Y += 97 * Game1.pixelZoom;
          break;
        case 3:
          point.X += 76 * Game1.pixelZoom;
          point.Y += 63 * Game1.pixelZoom;
          break;
        case 4:
          point.X += 223 * Game1.pixelZoom;
          point.Y += 63 * Game1.pixelZoom;
          break;
        case 5:
          point.X += 147 * Game1.pixelZoom;
          point.Y += 69 * Game1.pixelZoom;
          break;
        case 6:
          point.X += 147 * Game1.pixelZoom;
          point.Y += 95 * Game1.pixelZoom;
          break;
        case 7:
          point.X += 110 * Game1.pixelZoom;
          point.Y += 41 * Game1.pixelZoom;
          break;
        case 8:
          point.X += 194 * Game1.pixelZoom;
          point.Y += 41 * Game1.pixelZoom;
          break;
      }
      return point;
    }
  }
}
