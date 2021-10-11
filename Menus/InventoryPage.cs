// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.InventoryPage
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.Objects;
using System;
using System.Collections.Generic;

namespace StardewValley.Menus
{
  public class InventoryPage : IClickableMenu
  {
    private string descriptionText = "";
    private string hoverText = "";
    private string descriptionTitle = "";
    private string hoverTitle = "";
    public List<ClickableComponent> equipmentIcons = new List<ClickableComponent>();
    private string horseName = "";
    public const int region_inventory = 100;
    public const int region_hat = 101;
    public const int region_ring1 = 102;
    public const int region_ring2 = 103;
    public const int region_boots = 104;
    public const int region_trashCan = 105;
    public const int region_organizeButton = 106;
    public InventoryMenu inventory;
    private Item heldItem;
    private Item hoveredItem;
    public ClickableComponent portrait;
    public ClickableTextureComponent trashCan;
    public ClickableTextureComponent organizeButton;
    private float trashCanLidRotation;

    public InventoryPage(int x, int y, int width, int height)
      : base(x, y, width, height, false)
    {
      this.inventory = new InventoryMenu(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth, this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth, true, (List<Item>) null, (InventoryMenu.highlightThisItem) null, -1, 3, 0, 0, true);
      List<ClickableComponent> equipmentIcons = this.equipmentIcons;
      ClickableComponent clickableComponent = new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 3 / 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + 4 * Game1.tileSize - 12, Game1.tileSize, Game1.tileSize), "Hat");
      clickableComponent.myID = 101;
      clickableComponent.downNeighborID = 102;
      int num1 = Game1.player.MaxItems - 12;
      clickableComponent.upNeighborID = num1;
      int num2 = 105;
      clickableComponent.rightNeighborID = num2;
      int num3 = 1;
      clickableComponent.upNeighborImmutable = num3 != 0;
      equipmentIcons.Add(clickableComponent);
      this.equipmentIcons.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 3 / 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + 5 * Game1.tileSize - 12, Game1.tileSize, Game1.tileSize), "Left Ring")
      {
        myID = 102,
        downNeighborID = 103,
        rightNeighborID = 105
      });
      this.equipmentIcons.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 3 / 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + 6 * Game1.tileSize - 12, Game1.tileSize, Game1.tileSize), "Right Ring")
      {
        myID = 103,
        downNeighborID = 104,
        rightNeighborID = 105
      });
      this.equipmentIcons.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 3 / 4, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + 7 * Game1.tileSize - 12, Game1.tileSize, Game1.tileSize), "Boots")
      {
        myID = 104,
        rightNeighborID = 105
      });
      this.portrait = new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 3 - Game1.tileSize + Game1.tileSize / 2, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + 4 * Game1.tileSize - 8 + Game1.tileSize, Game1.tileSize, Game1.tileSize * 3 / 2), "32");
      ClickableTextureComponent textureComponent1 = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + width / 3 + Game1.tileSize * 9 + Game1.tileSize / 2, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + 3 * Game1.tileSize + Game1.tileSize, Game1.tileSize, 104), Game1.mouseCursors, new Rectangle(669, 261, 16, 26), (float) Game1.pixelZoom, false);
      int num4 = 105;
      textureComponent1.myID = num4;
      int num5 = 106;
      textureComponent1.upNeighborID = num5;
      int num6 = 101;
      textureComponent1.leftNeighborID = num6;
      this.trashCan = textureComponent1;
      ClickableTextureComponent textureComponent2 = new ClickableTextureComponent("", new Rectangle(this.xPositionOnScreen + width, this.yPositionOnScreen + height / 3 - Game1.tileSize + Game1.pixelZoom * 2, Game1.tileSize, Game1.tileSize), "", Game1.content.LoadString("Strings\\UI:ItemGrab_Organize"), Game1.mouseCursors, new Rectangle(162, 440, 16, 16), (float) Game1.pixelZoom, false);
      int num7 = 106;
      textureComponent2.myID = num7;
      int num8 = 105;
      textureComponent2.downNeighborID = num8;
      int num9 = 11;
      textureComponent2.leftNeighborID = num9;
      int num10 = 898;
      textureComponent2.upNeighborID = num10;
      this.organizeButton = textureComponent2;
      if (Utility.findHorse() == null)
        return;
      this.horseName = Utility.findHorse().displayName;
    }

    public override void receiveKeyPress(Keys key)
    {
      base.receiveKeyPress(key);
      if (Game1.isAnyGamePadButtonBeingPressed() && Game1.options.doesInputListContain(Game1.options.menuButton, key) && this.heldItem != null)
        Game1.setMousePosition(this.trashCan.bounds.Center);
      if (key.Equals((object) Keys.Delete) && this.heldItem != null && this.heldItem.canBeTrashed())
      {
        if (this.heldItem is StardewValley.Object && Game1.player.specialItems.Contains((this.heldItem as StardewValley.Object).parentSheetIndex))
          Game1.player.specialItems.Remove((this.heldItem as StardewValley.Object).parentSheetIndex);
        this.heldItem = (Item) null;
        Game1.playSound("trashcan");
      }
      if (Game1.options.doesInputListContain(Game1.options.inventorySlot1, key))
      {
        Game1.player.CurrentToolIndex = 0;
        Game1.playSound("toolSwap");
      }
      else if (Game1.options.doesInputListContain(Game1.options.inventorySlot2, key))
      {
        Game1.player.CurrentToolIndex = 1;
        Game1.playSound("toolSwap");
      }
      else if (Game1.options.doesInputListContain(Game1.options.inventorySlot3, key))
      {
        Game1.player.CurrentToolIndex = 2;
        Game1.playSound("toolSwap");
      }
      else if (Game1.options.doesInputListContain(Game1.options.inventorySlot4, key))
      {
        Game1.player.CurrentToolIndex = 3;
        Game1.playSound("toolSwap");
      }
      else if (Game1.options.doesInputListContain(Game1.options.inventorySlot5, key))
      {
        Game1.player.CurrentToolIndex = 4;
        Game1.playSound("toolSwap");
      }
      else if (Game1.options.doesInputListContain(Game1.options.inventorySlot6, key))
      {
        Game1.player.CurrentToolIndex = 5;
        Game1.playSound("toolSwap");
      }
      else if (Game1.options.doesInputListContain(Game1.options.inventorySlot7, key))
      {
        Game1.player.CurrentToolIndex = 6;
        Game1.playSound("toolSwap");
      }
      else if (Game1.options.doesInputListContain(Game1.options.inventorySlot8, key))
      {
        Game1.player.CurrentToolIndex = 7;
        Game1.playSound("toolSwap");
      }
      else if (Game1.options.doesInputListContain(Game1.options.inventorySlot9, key))
      {
        Game1.player.CurrentToolIndex = 8;
        Game1.playSound("toolSwap");
      }
      else if (Game1.options.doesInputListContain(Game1.options.inventorySlot10, key))
      {
        Game1.player.CurrentToolIndex = 9;
        Game1.playSound("toolSwap");
      }
      else if (Game1.options.doesInputListContain(Game1.options.inventorySlot11, key))
      {
        Game1.player.CurrentToolIndex = 10;
        Game1.playSound("toolSwap");
      }
      else
      {
        if (!Game1.options.doesInputListContain(Game1.options.inventorySlot12, key))
          return;
        Game1.player.CurrentToolIndex = 11;
        Game1.playSound("toolSwap");
      }
    }

    public override void setUpForGamePadMode()
    {
      base.setUpForGamePadMode();
      if (this.inventory != null)
        this.inventory.setUpForGamePadMode();
      this.currentRegion = 100;
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
      foreach (ClickableComponent equipmentIcon in this.equipmentIcons)
      {
        if (equipmentIcon.containsPoint(x, y))
        {
          bool flag = this.heldItem == null;
          string name = equipmentIcon.name;
          if (!(name == "Hat"))
          {
            if (!(name == "Left Ring"))
            {
              if (!(name == "Right Ring"))
              {
                if (name == "Boots" && (this.heldItem == null || this.heldItem is Boots))
                {
                  Boots heldItem = (Boots) this.heldItem;
                  this.heldItem = (Item) Game1.player.boots;
                  Game1.player.boots = heldItem;
                  if (this.heldItem != null)
                    (this.heldItem as Boots).onUnequip();
                  if (Game1.player.boots != null)
                  {
                    Game1.player.boots.onEquip();
                    Game1.playSound("sandyStep");
                    DelayedAction.playSoundAfterDelay("sandyStep", 150);
                  }
                  else if (this.heldItem != null)
                    Game1.playSound("dwop");
                }
              }
              else if (this.heldItem == null || this.heldItem is Ring)
              {
                Ring heldItem = (Ring) this.heldItem;
                this.heldItem = (Item) Game1.player.rightRing;
                Game1.player.rightRing = heldItem;
                if (this.heldItem != null)
                  (this.heldItem as Ring).onUnequip(Game1.player);
                if (Game1.player.rightRing != null)
                {
                  Game1.player.rightRing.onEquip(Game1.player);
                  Game1.playSound("crit");
                }
                else if (this.heldItem != null)
                  Game1.playSound("dwop");
              }
            }
            else if (this.heldItem == null || this.heldItem is Ring)
            {
              Ring heldItem = (Ring) this.heldItem;
              this.heldItem = (Item) Game1.player.leftRing;
              Game1.player.leftRing = heldItem;
              if (this.heldItem != null)
                (this.heldItem as Ring).onUnequip(Game1.player);
              if (Game1.player.leftRing != null)
              {
                Game1.player.leftRing.onEquip(Game1.player);
                Game1.playSound("crit");
              }
              else if (this.heldItem != null)
                Game1.playSound("dwop");
            }
          }
          else if (this.heldItem == null || this.heldItem is Hat)
          {
            Hat heldItem = (Hat) this.heldItem;
            this.heldItem = (Item) Game1.player.hat;
            Game1.player.hat = heldItem;
            if (Game1.player.hat != null)
              Game1.playSound("grassyStep");
            else if (this.heldItem != null)
              Game1.playSound("dwop");
          }
          if (flag && this.heldItem != null && Game1.oldKBState.IsKeyDown(Keys.LeftShift))
          {
            for (int position = 0; position < Game1.player.items.Count; ++position)
            {
              if (Game1.player.items[position] == null || Game1.player.items[position].canStackWith(this.heldItem))
              {
                if (Game1.player.CurrentToolIndex == position && this.heldItem != null)
                  this.heldItem.actionWhenBeingHeld(Game1.player);
                this.heldItem = Utility.addItemToInventory(this.heldItem, position, this.inventory.actualInventory, (ItemGrabMenu.behaviorOnItemSelect) null);
                if (Game1.player.CurrentToolIndex == position && this.heldItem != null)
                  this.heldItem.actionWhenStopBeingHeld(Game1.player);
                Game1.playSound("stoneStep");
                return;
              }
            }
          }
        }
      }
      this.heldItem = this.inventory.leftClick(x, y, this.heldItem, !Game1.oldKBState.IsKeyDown(Keys.LeftShift));
      if (this.heldItem != null && this.heldItem is StardewValley.Object && (this.heldItem as StardewValley.Object).ParentSheetIndex == 434)
      {
        Game1.playSound("smallSelect");
        Game1.playerEatObject(this.heldItem as StardewValley.Object, true);
        this.heldItem = (Item) null;
        Game1.exitActiveMenu();
      }
      else if (this.heldItem != null && Game1.oldKBState.IsKeyDown(Keys.LeftShift))
      {
        if (this.heldItem is Ring)
        {
          if (Game1.player.leftRing == null)
          {
            Game1.player.leftRing = this.heldItem as Ring;
            (this.heldItem as Ring).onEquip(Game1.player);
            this.heldItem = (Item) null;
            Game1.playSound("crit");
            return;
          }
          if (Game1.player.rightRing == null)
          {
            Game1.player.rightRing = this.heldItem as Ring;
            (this.heldItem as Ring).onEquip(Game1.player);
            this.heldItem = (Item) null;
            Game1.playSound("crit");
            return;
          }
        }
        else if (this.heldItem is Hat)
        {
          if (Game1.player.hat == null)
          {
            Game1.player.hat = this.heldItem as Hat;
            Game1.playSound("grassyStep");
            this.heldItem = (Item) null;
            return;
          }
        }
        else if (this.heldItem is Boots && Game1.player.boots == null)
        {
          Game1.player.boots = this.heldItem as Boots;
          (this.heldItem as Boots).onEquip();
          Game1.playSound("sandyStep");
          DelayedAction.playSoundAfterDelay("sandyStep", 150);
          this.heldItem = (Item) null;
          return;
        }
        if (this.inventory.getInventoryPositionOfClick(x, y) >= 12)
        {
          for (int position = 0; position < 12; ++position)
          {
            if (Game1.player.items[position] == null || Game1.player.items[position].canStackWith(this.heldItem))
            {
              if (Game1.player.CurrentToolIndex == position && this.heldItem != null)
                this.heldItem.actionWhenBeingHeld(Game1.player);
              this.heldItem = Utility.addItemToInventory(this.heldItem, position, this.inventory.actualInventory, (ItemGrabMenu.behaviorOnItemSelect) null);
              if (this.heldItem != null)
                this.heldItem.actionWhenStopBeingHeld(Game1.player);
              Game1.playSound("stoneStep");
              return;
            }
          }
        }
      }
      if (this.portrait.containsPoint(x, y))
        this.portrait.name = this.portrait.name.Equals("32") ? "8" : "32";
      if (this.heldItem != null && this.trashCan.containsPoint(x, y) && this.heldItem.canBeTrashed())
      {
        if (this.heldItem is StardewValley.Object && Game1.player.specialItems.Contains((this.heldItem as StardewValley.Object).parentSheetIndex))
          Game1.player.specialItems.Remove((this.heldItem as StardewValley.Object).parentSheetIndex);
        this.heldItem = (Item) null;
        Game1.playSound("trashcan");
      }
      else if (this.heldItem != null && !this.isWithinBounds(x, y) && this.heldItem.canBeTrashed())
      {
        Game1.playSound("throwDownITem");
        Game1.createItemDebris(this.heldItem, Game1.player.getStandingPosition(), Game1.player.FacingDirection, (GameLocation) null);
        this.heldItem = (Item) null;
      }
      if (this.organizeButton == null || !this.organizeButton.containsPoint(x, y))
        return;
      ItemGrabMenu.organizeItemsInList(Game1.player.items);
      Game1.playSound("Ship");
    }

    public override void receiveGamePadButton(Buttons b)
    {
      if (b != Buttons.Back || this.organizeButton == null)
        return;
      ItemGrabMenu.organizeItemsInList(Game1.player.items);
      Game1.playSound("Ship");
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
      this.heldItem = this.inventory.rightClick(x, y, this.heldItem, true);
    }

    public override void performHoverAction(int x, int y)
    {
      this.descriptionText = "";
      this.descriptionTitle = "";
      this.hoveredItem = this.inventory.hover(x, y, this.heldItem);
      this.hoverText = this.inventory.hoverText;
      this.hoverTitle = this.inventory.hoverTitle;
      foreach (ClickableComponent equipmentIcon in this.equipmentIcons)
      {
        if (equipmentIcon.containsPoint(x, y))
        {
          string name = equipmentIcon.name;
          if (!(name == "Hat"))
          {
            if (!(name == "Right Ring"))
            {
              if (!(name == "Left Ring"))
              {
                if (name == "Boots" && Game1.player.boots != null)
                {
                  this.hoveredItem = (Item) Game1.player.boots;
                  this.hoverText = Game1.player.boots.getDescription();
                  this.hoverTitle = Game1.player.boots.DisplayName;
                }
              }
              else if (Game1.player.leftRing != null)
              {
                this.hoveredItem = (Item) Game1.player.leftRing;
                this.hoverText = Game1.player.leftRing.getDescription();
                this.hoverTitle = Game1.player.leftRing.DisplayName;
              }
            }
            else if (Game1.player.rightRing != null)
            {
              this.hoveredItem = (Item) Game1.player.rightRing;
              this.hoverText = Game1.player.rightRing.getDescription();
              this.hoverTitle = Game1.player.rightRing.DisplayName;
            }
          }
          else if (Game1.player.hat != null)
          {
            this.hoveredItem = (Item) Game1.player.hat;
            this.hoverText = Game1.player.hat.getDescription();
            this.hoverTitle = Game1.player.hat.DisplayName;
          }
          equipmentIcon.scale = Math.Min(equipmentIcon.scale + 0.05f, 1.1f);
        }
        equipmentIcon.scale = Math.Max(1f, equipmentIcon.scale - 0.025f);
      }
      if (this.portrait.containsPoint(x, y))
      {
        this.portrait.scale += 0.2f;
        this.hoverText = Game1.content.LoadString("Strings\\UI:Inventory_PortraitHover_Level", (object) Game1.player.Level) + Environment.NewLine + Game1.player.getTitle();
      }
      else
        this.portrait.scale = 0.0f;
      if (this.trashCan.containsPoint(x, y))
      {
        if ((double) this.trashCanLidRotation <= 0.0)
          Game1.playSound("trashcanlid");
        this.trashCanLidRotation = Math.Min(this.trashCanLidRotation + (float) Math.PI / 48f, 1.570796f);
      }
      else if ((double) this.trashCanLidRotation != 0.0)
      {
        this.trashCanLidRotation = Math.Max(this.trashCanLidRotation - 0.1308997f, 0.0f);
        if ((double) this.trashCanLidRotation == 0.0)
          Game1.playSound("thudStep");
      }
      if (this.organizeButton == null)
        return;
      this.organizeButton.tryHover(x, y, 0.1f);
      if (!this.organizeButton.containsPoint(x, y))
        return;
      this.hoverText = this.organizeButton.hoverText;
    }

    public override void snapToDefaultClickableComponent()
    {
      this.currentlySnappedComponent = this.getComponentWithID(0);
      this.snapCursorToCurrentSnappedComponent();
    }

    public override bool readyToClose()
    {
      return this.heldItem == null;
    }

    public override void draw(SpriteBatch b)
    {
      this.drawHorizontalPartition(b, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + 3 * Game1.tileSize, false);
      this.inventory.draw(b);
      foreach (ClickableComponent equipmentIcon in this.equipmentIcons)
      {
        string name = equipmentIcon.name;
        if (!(name == "Hat"))
        {
          if (!(name == "Right Ring"))
          {
            if (!(name == "Left Ring"))
            {
              if (name == "Boots")
              {
                if (Game1.player.boots != null)
                {
                  b.Draw(Game1.menuTexture, equipmentIcon.bounds, new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 10, -1, -1)), Color.White);
                  Game1.player.boots.drawInMenu(b, new Vector2((float) equipmentIcon.bounds.X, (float) equipmentIcon.bounds.Y), equipmentIcon.scale);
                }
                else
                  b.Draw(Game1.menuTexture, equipmentIcon.bounds, new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 40, -1, -1)), Color.White);
              }
            }
            else if (Game1.player.leftRing != null)
            {
              b.Draw(Game1.menuTexture, equipmentIcon.bounds, new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 10, -1, -1)), Color.White);
              Game1.player.leftRing.drawInMenu(b, new Vector2((float) equipmentIcon.bounds.X, (float) equipmentIcon.bounds.Y), equipmentIcon.scale);
            }
            else
              b.Draw(Game1.menuTexture, equipmentIcon.bounds, new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 41, -1, -1)), Color.White);
          }
          else if (Game1.player.rightRing != null)
          {
            b.Draw(Game1.menuTexture, equipmentIcon.bounds, new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 10, -1, -1)), Color.White);
            Game1.player.rightRing.drawInMenu(b, new Vector2((float) equipmentIcon.bounds.X, (float) equipmentIcon.bounds.Y), equipmentIcon.scale);
          }
          else
            b.Draw(Game1.menuTexture, equipmentIcon.bounds, new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 41, -1, -1)), Color.White);
        }
        else if (Game1.player.hat != null)
        {
          b.Draw(Game1.menuTexture, equipmentIcon.bounds, new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 10, -1, -1)), Color.White);
          Game1.player.hat.drawInMenu(b, new Vector2((float) equipmentIcon.bounds.X, (float) equipmentIcon.bounds.Y), equipmentIcon.scale, 1f, 0.866f, false);
        }
        else
          b.Draw(Game1.menuTexture, equipmentIcon.bounds, new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 42, -1, -1)), Color.White);
      }
      b.Draw(Game1.timeOfDay >= 1900 ? Game1.nightbg : Game1.daybg, new Vector2((float) (this.xPositionOnScreen + Game1.tileSize * 3 - Game1.tileSize), (float) (this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + 4 * Game1.tileSize - 8)), Color.White);
      Game1.player.FarmerRenderer.draw(b, new FarmerSprite.AnimationFrame(0, Game1.player.bathingClothes ? 108 : 0, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false), Game1.player.bathingClothes ? 108 : 0, new Rectangle(0, Game1.player.bathingClothes ? 576 : 0, 16, 32), new Vector2((float) (this.xPositionOnScreen + Game1.tileSize * 3 - Game1.tileSize / 2), (float) (this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + 5 * Game1.tileSize - Game1.tileSize / 2)), Vector2.Zero, 0.8f, 2, Color.White, 0.0f, 1f, Game1.player);
      if (Game1.timeOfDay >= 1900)
        Game1.player.FarmerRenderer.draw(b, new FarmerSprite.AnimationFrame(0, 0, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false), 0, new Rectangle(0, 0, 16, 32), new Vector2((float) (this.xPositionOnScreen + Game1.tileSize * 3 - Game1.tileSize / 2), (float) (this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + 5 * Game1.tileSize - Game1.tileSize / 2)), Vector2.Zero, 0.8f, 2, Color.DarkBlue * 0.3f, 0.0f, 1f, Game1.player);
      Utility.drawTextWithShadow(b, Game1.player.name, Game1.dialogueFont, new Vector2((float) (this.xPositionOnScreen + Game1.tileSize * 3) - Math.Min((float) Game1.tileSize, Game1.dialogueFont.MeasureString(Game1.player.name).X / 2f), (float) (this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + 7 * Game1.tileSize + Game1.pixelZoom * 2)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
      string text1 = Game1.content.LoadString("Strings\\UI:Inventory_FarmName", (object) Game1.player.farmName);
      Utility.drawTextWithShadow(b, text1, Game1.dialogueFont, new Vector2((float) (this.xPositionOnScreen + Game1.tileSize * 8 + Game1.tileSize / 2) - Game1.dialogueFont.MeasureString(text1).X / 2f, (float) (this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + 4 * Game1.tileSize + Game1.pixelZoom)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
      string text2 = Game1.content.LoadString("Strings\\UI:Inventory_CurrentFunds", (object) Utility.getNumberWithCommas(Game1.player.Money));
      Utility.drawTextWithShadow(b, text2, Game1.dialogueFont, new Vector2((float) (this.xPositionOnScreen + Game1.tileSize * 8 + Game1.tileSize / 2) - Game1.dialogueFont.MeasureString(text2).X / 2f, (float) (this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + 5 * Game1.tileSize)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
      string text3 = Game1.content.LoadString("Strings\\UI:Inventory_TotalEarnings", (object) Utility.getNumberWithCommas((int) Game1.player.totalMoneyEarned));
      Utility.drawTextWithShadow(b, text3, Game1.dialogueFont, new Vector2((float) (this.xPositionOnScreen + Game1.tileSize * 8 + Game1.tileSize / 2) - Game1.dialogueFont.MeasureString(text3).X / 2f, (float) (this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + 6 * Game1.tileSize - Game1.pixelZoom)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
      if (Game1.player.hasPet())
      {
        string petDisplayName = Game1.player.getPetDisplayName();
        Utility.drawTextWithShadow(b, petDisplayName, Game1.dialogueFont, new Vector2((float) (this.xPositionOnScreen + Game1.tileSize * 5) + Math.Max((float) Game1.tileSize, Game1.dialogueFont.MeasureString(Game1.player.name).X / 2f), (float) (this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + 7 * Game1.tileSize + Game1.pixelZoom * 2)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
        Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float) (this.xPositionOnScreen + Game1.tileSize * 4) + Math.Max((float) Game1.tileSize, Game1.dialogueFont.MeasureString(Game1.player.name).X / 2f), (float) (this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + 7 * Game1.tileSize - Game1.pixelZoom)), new Rectangle(160 + (Game1.player.catPerson ? 0 : 16), 192, 16, 16), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, false, -1f, -1, -1, 0.35f);
      }
      if (this.horseName.Length > 0)
      {
        Utility.drawTextWithShadow(b, this.horseName, Game1.dialogueFont, new Vector2((float) ((double) (this.xPositionOnScreen + Game1.tileSize * 6) + (double) Math.Max((float) Game1.tileSize, Game1.dialogueFont.MeasureString(Game1.player.name).X / 2f) + (Game1.player.getPetDisplayName() != null ? (double) Math.Max((float) Game1.tileSize, Game1.dialogueFont.MeasureString(Game1.player.getPetDisplayName()).X) : 0.0)), (float) (this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + 7 * Game1.tileSize + Game1.pixelZoom * 2)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
        Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float) ((double) (this.xPositionOnScreen + Game1.tileSize * 5 + Game1.pixelZoom * 2) + (double) Math.Max((float) Game1.tileSize, Game1.dialogueFont.MeasureString(Game1.player.name).X / 2f) + (Game1.player.getPetDisplayName() != null ? (double) Math.Max((float) Game1.tileSize, Game1.dialogueFont.MeasureString(Game1.player.getPetDisplayName()).X) : 0.0)), (float) (this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + 7 * Game1.tileSize - Game1.pixelZoom)), new Rectangle(193, 192, 16, 16), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, false, -1f, -1, -1, 0.35f);
      }
      int positionOnScreen = this.xPositionOnScreen;
      int num = this.width / 3;
      int tileSize1 = Game1.tileSize;
      int tileSize2 = Game1.tileSize;
      if (this.organizeButton != null)
        this.organizeButton.draw(b);
      this.trashCan.draw(b);
      b.Draw(Game1.mouseCursors, new Vector2((float) (this.trashCan.bounds.X + 60), (float) (this.trashCan.bounds.Y + 40)), new Rectangle?(new Rectangle(686, 256, 18, 10)), Color.White, this.trashCanLidRotation, new Vector2(16f, 10f), (float) Game1.pixelZoom, SpriteEffects.None, 0.86f);
      if (this.heldItem != null)
        this.heldItem.drawInMenu(b, new Vector2((float) (Game1.getOldMouseX() + 16), (float) (Game1.getOldMouseY() + 16)), 1f);
      if (this.hoverText == null || this.hoverText.Equals(""))
        return;
      IClickableMenu.drawToolTip(b, this.hoverText, this.hoverTitle, this.hoveredItem, this.heldItem != null, -1, 0, -1, -1, (CraftingRecipe) null, -1);
    }
  }
}
