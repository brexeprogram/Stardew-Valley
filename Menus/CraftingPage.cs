// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.CraftingPage
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Menus
{
  public class CraftingPage : IClickableMenu
  {
    private string descriptionText = "";
    private string hoverText = "";
    public List<Dictionary<ClickableTextureComponent, CraftingRecipe>> pagesOfCraftingRecipes = new List<Dictionary<ClickableTextureComponent, CraftingRecipe>>();
    private string hoverTitle = "";
    public const int howManyRecipesFitOnPage = 40;
    public const int region_upArrow = 88;
    public const int region_downArrow = 89;
    public const int region_craftingSelectionArea = 8000;
    public const int region_craftingModifier = 200;
    private Item hoverItem;
    private Item lastCookingHover;
    public InventoryMenu inventory;
    private Item heldItem;
    private int currentCraftingPage;
    private CraftingRecipe hoverRecipe;
    public ClickableTextureComponent upButton;
    public ClickableTextureComponent downButton;
    private bool cooking;
    public ClickableTextureComponent trashCan;
    public float trashCanLidRotation;

    public CraftingPage(int x, int y, int width, int height, bool cooking = false)
      : base(x, y, width, height, false)
    {
      this.cooking = cooking;
      this.inventory = new InventoryMenu(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth, this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth + Game1.tileSize * 5 - Game1.tileSize / 4, false, (List<Item>) null, (InventoryMenu.highlightThisItem) null, -1, 3, 0, 0, true);
      this.inventory.showGrayedOutSlots = true;
      int num1 = this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth - Game1.tileSize / 4;
      int y1 = this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth - Game1.tileSize / 4;
      int tileSize = Game1.tileSize;
      int num2 = 8;
      int num3 = 10;
      int num4 = -1;
      if (cooking)
        this.initializeUpperRightCloseButton();
      SerializableDictionary<string, int> serializableDictionary = new SerializableDictionary<string, int>();
      foreach (string key in CraftingRecipe.craftingRecipes.Keys)
      {
        if (Game1.player.craftingRecipes.ContainsKey(key))
          serializableDictionary.Add(key, Game1.player.craftingRecipes[key]);
      }
      Game1.player.craftingRecipes = serializableDictionary;
      ClickableTextureComponent textureComponent1 = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + width + 4, this.yPositionOnScreen + height - Game1.tileSize * 3 - Game1.tileSize / 2 - IClickableMenu.borderWidth - 104, Game1.tileSize, 104), Game1.mouseCursors, new Rectangle(669, 261, 16, 26), (float) Game1.pixelZoom, false);
      int num5 = 106;
      textureComponent1.myID = num5;
      this.trashCan = textureComponent1;
      List<string> stringList = new List<string>();
      if (!cooking)
      {
        foreach (string key in Game1.player.craftingRecipes.Keys)
          stringList.Add(new string(key.ToCharArray()));
      }
      else
      {
        Game1.playSound("bigSelect");
        foreach (string key in CraftingRecipe.cookingRecipes.Keys)
          stringList.Add(new string(key.ToCharArray()));
      }
      int count1 = stringList.Count;
      int index1 = 0;
      while (stringList.Count > 0)
      {
        CraftingRecipe craftingRecipe;
        int index2;
        ClickableTextureComponent key1;
        bool flag;
        do
        {
          ++num4;
          if (num4 % 40 == 0)
            this.pagesOfCraftingRecipes.Add(new Dictionary<ClickableTextureComponent, CraftingRecipe>());
          int num6 = num4 / num3 % (40 / num3);
          craftingRecipe = new CraftingRecipe(stringList[index1], cooking);
          int count2 = stringList.Count;
          while (craftingRecipe.bigCraftable && num6 == 40 / num3 - 1 && count2 > 0)
          {
            index1 = (index1 + 1) % stringList.Count;
            --count2;
            craftingRecipe = new CraftingRecipe(stringList[index1], false);
            if (count2 == 0)
            {
              num4 += 40 - num4 % 40;
              num6 = num4 / num3 % (40 / num3);
              this.pagesOfCraftingRecipes.Add(new Dictionary<ClickableTextureComponent, CraftingRecipe>());
            }
          }
          index2 = num4 / 40;
          ClickableTextureComponent textureComponent2 = new ClickableTextureComponent("", new Rectangle(num1 + num4 % num3 * (Game1.tileSize + num2), y1 + num6 * (Game1.tileSize + 8), Game1.tileSize, craftingRecipe.bigCraftable ? Game1.tileSize * 2 : Game1.tileSize), (string) null, !cooking || Game1.player.cookingRecipes.ContainsKey(craftingRecipe.name) ? "" : "ghosted", craftingRecipe.bigCraftable ? Game1.bigCraftableSpriteSheet : Game1.objectSpriteSheet, craftingRecipe.bigCraftable ? Game1.getArbitrarySourceRect(Game1.bigCraftableSpriteSheet, 16, 32, craftingRecipe.getIndexOfMenuView()) : Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, craftingRecipe.getIndexOfMenuView(), 16, 16), (float) Game1.pixelZoom, false);
          int num7 = 200 + num4;
          textureComponent2.myID = num7;
          int num8 = craftingRecipe.bigCraftable ? 200 + num4 + num3 : -500;
          textureComponent2.myAlternateID = num8;
          int num9 = num4 % num3 < num3 - 1 ? 200 + num4 + 1 : (num6 >= 2 || index2 <= 0 ? 89 : 88);
          textureComponent2.rightNeighborID = num9;
          int num10 = num4 % num3 > 0 ? 200 + num4 - 1 : -1;
          textureComponent2.leftNeighborID = num10;
          int num11 = num6 == 0 ? 12344 : 200 + num4 - num3;
          textureComponent2.upNeighborID = num11;
          int num12 = num6 == 40 / num3 - 1 || num6 == 40 / num3 - 2 && craftingRecipe.bigCraftable || stringList.Count <= 10 ? num4 % num3 : 200 + num4 + (craftingRecipe.bigCraftable ? num3 * 2 : num3);
          textureComponent2.downNeighborID = num12;
          int num13 = 1;
          textureComponent2.fullyImmutable = num13 != 0;
          int num14 = 8000;
          textureComponent2.region = num14;
          key1 = textureComponent2;
          flag = false;
          foreach (ClickableComponent key2 in this.pagesOfCraftingRecipes[index2].Keys)
          {
            if (key2.bounds.Intersects(key1.bounds))
            {
              flag = true;
              break;
            }
          }
        }
        while (flag);
        this.pagesOfCraftingRecipes[index2].Add(key1, craftingRecipe);
        stringList.RemoveAt(index1);
        index1 = 0;
      }
      if (this.pagesOfCraftingRecipes.Count <= 1)
        return;
      ClickableTextureComponent textureComponent3 = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 12 + Game1.tileSize / 2, y1, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 12, -1, -1), 0.8f, false);
      int num15 = 88;
      textureComponent3.myID = num15;
      int num16 = 89;
      textureComponent3.downNeighborID = num16;
      int num17 = 106;
      textureComponent3.rightNeighborID = num17;
      this.upButton = textureComponent3;
      ClickableTextureComponent textureComponent4 = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 12 + Game1.tileSize / 2, y1 + Game1.tileSize * 3 + Game1.tileSize / 2, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 11, -1, -1), 0.8f, false);
      int num18 = 89;
      textureComponent4.myID = num18;
      int num19 = 88;
      textureComponent4.upNeighborID = num19;
      int num20 = 106;
      textureComponent4.rightNeighborID = num20;
      this.downButton = textureComponent4;
    }

    protected override void noSnappedComponentFound(int direction, int oldRegion, int oldID)
    {
      base.noSnappedComponentFound(direction, oldRegion, oldID);
      if (oldRegion != 8000 || direction != 2)
        return;
      this.currentlySnappedComponent = this.getComponentWithID(oldID % 10);
      this.currentlySnappedComponent.upNeighborID = oldID;
    }

    public override void snapToDefaultClickableComponent()
    {
      this.currentlySnappedComponent = this.currentCraftingPage < this.pagesOfCraftingRecipes.Count ? (ClickableComponent) this.pagesOfCraftingRecipes[this.currentCraftingPage].First<KeyValuePair<ClickableTextureComponent, CraftingRecipe>>().Key : (ClickableComponent) null;
      this.snapCursorToCurrentSnappedComponent();
    }

    protected override void actionOnRegionChange(int oldRegion, int newRegion)
    {
      base.actionOnRegionChange(oldRegion, newRegion);
      if (newRegion != 9000 || oldRegion == 0)
        return;
      for (int index = 0; index < 10; ++index)
      {
        if (this.inventory.inventory.Count > index)
          this.inventory.inventory[index].upNeighborID = this.currentlySnappedComponent.upNeighborID;
      }
    }

    public override void receiveKeyPress(Keys key)
    {
      base.receiveKeyPress(key);
      if (!key.Equals((object) Keys.Delete) || this.heldItem == null || !this.heldItem.canBeTrashed())
        return;
      if (this.heldItem is StardewValley.Object && Game1.player.specialItems.Contains((this.heldItem as StardewValley.Object).parentSheetIndex))
        Game1.player.specialItems.Remove((this.heldItem as StardewValley.Object).parentSheetIndex);
      this.heldItem = (Item) null;
      Game1.playSound("trashcan");
    }

    public override void receiveScrollWheelAction(int direction)
    {
      base.receiveScrollWheelAction(direction);
      if (direction > 0 && this.currentCraftingPage > 0)
      {
        this.currentCraftingPage = this.currentCraftingPage - 1;
        Game1.playSound("shwip");
        if (!Game1.options.SnappyMenus)
          return;
        ClickableTextureComponent upButton = this.upButton;
        KeyValuePair<ClickableTextureComponent, CraftingRecipe> keyValuePair = this.pagesOfCraftingRecipes[this.currentCraftingPage].Last<KeyValuePair<ClickableTextureComponent, CraftingRecipe>>();
        int id1 = keyValuePair.Key.myID;
        upButton.leftNeighborID = id1;
        this.setCurrentlySnappedComponentTo(88);
        this.snapCursorToCurrentSnappedComponent();
        ClickableTextureComponent downButton = this.downButton;
        keyValuePair = this.pagesOfCraftingRecipes[this.currentCraftingPage].Last<KeyValuePair<ClickableTextureComponent, CraftingRecipe>>();
        int id2 = keyValuePair.Key.myID;
        downButton.leftNeighborID = id2;
      }
      else
      {
        if (direction >= 0 || this.currentCraftingPage >= this.pagesOfCraftingRecipes.Count - 1)
          return;
        this.currentCraftingPage = this.currentCraftingPage + 1;
        Game1.playSound("shwip");
        if (!Game1.options.SnappyMenus)
          return;
        ClickableTextureComponent downButton = this.downButton;
        KeyValuePair<ClickableTextureComponent, CraftingRecipe> keyValuePair = this.pagesOfCraftingRecipes[this.currentCraftingPage].Last<KeyValuePair<ClickableTextureComponent, CraftingRecipe>>();
        int id1 = keyValuePair.Key.myID;
        downButton.leftNeighborID = id1;
        this.setCurrentlySnappedComponentTo(89);
        this.snapCursorToCurrentSnappedComponent();
        ClickableTextureComponent upButton = this.upButton;
        keyValuePair = this.pagesOfCraftingRecipes[this.currentCraftingPage].Last<KeyValuePair<ClickableTextureComponent, CraftingRecipe>>();
        int id2 = keyValuePair.Key.myID;
        upButton.leftNeighborID = id2;
      }
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
      base.receiveLeftClick(x, y, true);
      this.heldItem = this.inventory.leftClick(x, y, this.heldItem, true);
      if (this.upButton != null && this.upButton.containsPoint(x, y) && this.currentCraftingPage > 0)
      {
        Game1.playSound("coin");
        this.currentCraftingPage = Math.Max(0, this.currentCraftingPage - 1);
        this.upButton.scale = this.upButton.baseScale;
        this.upButton.leftNeighborID = this.pagesOfCraftingRecipes[this.currentCraftingPage].Last<KeyValuePair<ClickableTextureComponent, CraftingRecipe>>().Key.myID;
      }
      if (this.downButton != null && this.downButton.containsPoint(x, y) && this.currentCraftingPage < this.pagesOfCraftingRecipes.Count - 1)
      {
        Game1.playSound("coin");
        this.currentCraftingPage = Math.Min(this.pagesOfCraftingRecipes.Count - 1, this.currentCraftingPage + 1);
        this.downButton.scale = this.downButton.baseScale;
        this.downButton.leftNeighborID = this.pagesOfCraftingRecipes[this.currentCraftingPage].Last<KeyValuePair<ClickableTextureComponent, CraftingRecipe>>().Key.myID;
      }
      foreach (ClickableTextureComponent key in this.pagesOfCraftingRecipes[this.currentCraftingPage].Keys)
      {
        int num = Game1.oldKBState.IsKeyDown(Keys.LeftShift) ? 5 : 1;
        for (int index = 0; index < num; ++index)
        {
          if (key.containsPoint(x, y) && !key.hoverText.Equals("ghosted") && this.pagesOfCraftingRecipes[this.currentCraftingPage][key].doesFarmerHaveIngredientsInInventory(this.cooking ? Utility.getHomeOfFarmer(Game1.player).fridge.items : (List<Item>) null))
            this.clickCraftingRecipe(key, index == 0);
        }
      }
      if (this.trashCan != null && this.trashCan.containsPoint(x, y) && (this.heldItem != null && this.heldItem.canBeTrashed()))
      {
        if (this.heldItem is StardewValley.Object && Game1.player.specialItems.Contains((this.heldItem as StardewValley.Object).parentSheetIndex))
          Game1.player.specialItems.Remove((this.heldItem as StardewValley.Object).parentSheetIndex);
        this.heldItem = (Item) null;
        Game1.playSound("trashcan");
      }
      else
      {
        if (this.heldItem == null || this.isWithinBounds(x, y) || !this.heldItem.canBeTrashed())
          return;
        Game1.playSound("throwDownITem");
        Game1.createItemDebris(this.heldItem, Game1.player.getStandingPosition(), Game1.player.FacingDirection, (GameLocation) null);
        this.heldItem = (Item) null;
      }
    }

    private void clickCraftingRecipe(ClickableTextureComponent c, bool playSound = true)
    {
      Item obj = this.pagesOfCraftingRecipes[this.currentCraftingPage][c].createItem();
      Game1.player.checkForQuestComplete((NPC) null, -1, -1, obj, (string) null, 2, -1);
      if (this.heldItem == null)
      {
        this.pagesOfCraftingRecipes[this.currentCraftingPage][c].consumeIngredients();
        this.heldItem = obj;
        if (playSound)
          Game1.playSound("coin");
      }
      else if (this.heldItem.Name.Equals(obj.Name) && this.heldItem.Stack + this.pagesOfCraftingRecipes[this.currentCraftingPage][c].numberProducedPerCraft - 1 < this.heldItem.maximumStackSize())
      {
        this.heldItem.Stack += this.pagesOfCraftingRecipes[this.currentCraftingPage][c].numberProducedPerCraft;
        this.pagesOfCraftingRecipes[this.currentCraftingPage][c].consumeIngredients();
        if (playSound)
          Game1.playSound("coin");
      }
      if (!this.cooking && Game1.player.craftingRecipes.ContainsKey(this.pagesOfCraftingRecipes[this.currentCraftingPage][c].name))
      {
        SerializableDictionary<string, int> craftingRecipes = Game1.player.craftingRecipes;
        string name = this.pagesOfCraftingRecipes[this.currentCraftingPage][c].name;
        craftingRecipes[name] = craftingRecipes[name] + this.pagesOfCraftingRecipes[this.currentCraftingPage][c].numberProducedPerCraft;
      }
      if (this.cooking)
        Game1.player.cookedRecipe(this.heldItem.parentSheetIndex);
      if (!this.cooking)
        Game1.stats.checkForCraftingAchievements();
      else
        Game1.stats.checkForCookingAchievements();
      if (!Game1.options.gamepadControls || this.heldItem == null || !Game1.player.couldInventoryAcceptThisItem(this.heldItem))
        return;
      Game1.player.addItemToInventoryBool(this.heldItem, false);
      this.heldItem = (Item) null;
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
      this.heldItem = this.inventory.rightClick(x, y, this.heldItem, true);
      foreach (ClickableTextureComponent key in this.pagesOfCraftingRecipes[this.currentCraftingPage].Keys)
      {
        if (key.containsPoint(x, y) && !key.hoverText.Equals("ghosted") && this.pagesOfCraftingRecipes[this.currentCraftingPage][key].doesFarmerHaveIngredientsInInventory(this.cooking ? Utility.getHomeOfFarmer(Game1.player).fridge.items : (List<Item>) null))
          this.clickCraftingRecipe(key, true);
      }
    }

    public override void performHoverAction(int x, int y)
    {
      base.performHoverAction(x, y);
      this.hoverTitle = "";
      this.descriptionText = "";
      this.hoverText = "";
      this.hoverRecipe = (CraftingRecipe) null;
      this.hoverItem = this.inventory.hover(x, y, this.hoverItem);
      if (this.hoverItem != null)
      {
        this.hoverTitle = this.inventory.hoverTitle;
        this.hoverText = this.inventory.hoverText;
      }
      foreach (ClickableTextureComponent key in this.pagesOfCraftingRecipes[this.currentCraftingPage].Keys)
      {
        if (key.containsPoint(x, y))
        {
          if (key.hoverText.Equals("ghosted"))
          {
            this.hoverText = "???";
          }
          else
          {
            this.hoverRecipe = this.pagesOfCraftingRecipes[this.currentCraftingPage][key];
            if (this.lastCookingHover == null || !this.lastCookingHover.Name.Equals(this.hoverRecipe.name))
              this.lastCookingHover = this.hoverRecipe.createItem();
            key.scale = Math.Min(key.scale + 0.02f, key.baseScale + 0.1f);
          }
        }
        else
          key.scale = Math.Max(key.scale - 0.02f, key.baseScale);
      }
      if (this.upButton != null)
      {
        if (this.upButton.containsPoint(x, y))
          this.upButton.scale = Math.Min(this.upButton.scale + 0.02f, this.upButton.baseScale + 0.1f);
        else
          this.upButton.scale = Math.Max(this.upButton.scale - 0.02f, this.upButton.baseScale);
      }
      if (this.downButton != null)
      {
        if (this.downButton.containsPoint(x, y))
          this.downButton.scale = Math.Min(this.downButton.scale + 0.02f, this.downButton.baseScale + 0.1f);
        else
          this.downButton.scale = Math.Max(this.downButton.scale - 0.02f, this.downButton.baseScale);
      }
      if (this.trashCan == null)
        return;
      if (this.trashCan.containsPoint(x, y))
      {
        if ((double) this.trashCanLidRotation <= 0.0)
          Game1.playSound("trashcanlid");
        this.trashCanLidRotation = Math.Min(this.trashCanLidRotation + (float) Math.PI / 48f, 1.570796f);
      }
      else
        this.trashCanLidRotation = Math.Max(this.trashCanLidRotation - (float) Math.PI / 48f, 0.0f);
    }

    public override bool readyToClose()
    {
      return this.heldItem == null;
    }

    public override void draw(SpriteBatch b)
    {
      if (this.cooking)
        Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true, (string) null, false);
      this.drawHorizontalPartition(b, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + 4 * Game1.tileSize, false);
      this.inventory.draw(b);
      if (this.trashCan != null)
      {
        this.trashCan.draw(b);
        b.Draw(Game1.mouseCursors, new Vector2((float) (this.trashCan.bounds.X + 60), (float) (this.trashCan.bounds.Y + 40)), new Rectangle?(new Rectangle(686, 256, 18, 10)), Color.White, this.trashCanLidRotation, new Vector2(16f, 10f), (float) Game1.pixelZoom, SpriteEffects.None, 0.86f);
      }
      b.End();
      b.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
      foreach (ClickableTextureComponent key in this.pagesOfCraftingRecipes[this.currentCraftingPage].Keys)
      {
        if (key.hoverText.Equals("ghosted"))
          key.draw(b, Color.Black * 0.35f, 0.89f);
        else if (!this.pagesOfCraftingRecipes[this.currentCraftingPage][key].doesFarmerHaveIngredientsInInventory(this.cooking ? Utility.getHomeOfFarmer(Game1.player).fridge.items : (List<Item>) null))
        {
          key.draw(b, Color.LightGray * 0.4f, 0.89f);
        }
        else
        {
          key.draw(b);
          if (this.pagesOfCraftingRecipes[this.currentCraftingPage][key].numberProducedPerCraft > 1)
            NumberSprite.draw(this.pagesOfCraftingRecipes[this.currentCraftingPage][key].numberProducedPerCraft, b, new Vector2((float) (key.bounds.X + Game1.tileSize - 2), (float) (key.bounds.Y + Game1.tileSize - 2)), Color.Red, (float) (0.5 * ((double) key.scale / (double) Game1.pixelZoom)), 0.97f, 1f, 0, 0);
        }
      }
      b.End();
      b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
      if (this.hoverItem != null)
        IClickableMenu.drawToolTip(b, this.hoverText, this.hoverTitle, this.hoverItem, this.heldItem != null, -1, 0, -1, -1, (CraftingRecipe) null, -1);
      else if (!string.IsNullOrEmpty(this.hoverText))
        IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, this.heldItem != null ? Game1.tileSize : 0, this.heldItem != null ? Game1.tileSize : 0, -1, (string) null, -1, (string[]) null, (Item) null, 0, -1, -1, -1, -1, 1f, (CraftingRecipe) null);
      if (this.heldItem != null)
        this.heldItem.drawInMenu(b, new Vector2((float) (Game1.getOldMouseX() + Game1.tileSize / 4), (float) (Game1.getOldMouseY() + Game1.tileSize / 4)), 1f);
      base.draw(b);
      if (this.downButton != null && this.currentCraftingPage < this.pagesOfCraftingRecipes.Count - 1)
        this.downButton.draw(b);
      if (this.upButton != null && this.currentCraftingPage > 0)
        this.upButton.draw(b);
      if (this.cooking)
        this.drawMouse(b);
      if (this.hoverRecipe == null)
        return;
      SpriteBatch b1 = b;
      string text = " ";
      SpriteFont smallFont = Game1.smallFont;
      int xOffset = this.heldItem != null ? Game1.tileSize * 3 / 4 : 0;
      int yOffset = this.heldItem != null ? Game1.tileSize * 3 / 4 : 0;
      int moneyAmountToDisplayAtBottom = -1;
      string displayName = this.hoverRecipe.DisplayName;
      int healAmountToDisplay = -1;
      string[] buffIconsToDisplay;
      if (this.cooking && this.lastCookingHover != null)
      {
        if (Game1.objectInformation[(this.lastCookingHover as StardewValley.Object).parentSheetIndex].Split('/').Length > 7)
        {
          buffIconsToDisplay = Game1.objectInformation[(this.lastCookingHover as StardewValley.Object).parentSheetIndex].Split('/')[7].Split(' ');
          goto label_32;
        }
      }
      buffIconsToDisplay = (string[]) null;
label_32:
      Item lastCookingHover = this.lastCookingHover;
      int currencySymbol = 0;
      int extraItemToShowIndex = -1;
      int extraItemToShowAmount = -1;
      int overrideX = -1;
      int overrideY = -1;
      double num = 1.0;
      CraftingRecipe hoverRecipe = this.hoverRecipe;
      IClickableMenu.drawHoverText(b1, text, smallFont, xOffset, yOffset, moneyAmountToDisplayAtBottom, displayName, healAmountToDisplay, buffIconsToDisplay, lastCookingHover, currencySymbol, extraItemToShowIndex, extraItemToShowAmount, overrideX, overrideY, (float) num, hoverRecipe);
    }
  }
}
