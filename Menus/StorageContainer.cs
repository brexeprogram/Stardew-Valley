// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.StorageContainer
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace StardewValley.Menus
{
  public class StorageContainer : MenuWithInventory
  {
    public InventoryMenu ItemsToGrabMenu;
    private TemporaryAnimatedSprite poof;
    private StorageContainer.behaviorOnItemChange itemChangeBehavior;

    public StorageContainer(List<Item> inventory, int capacity, int rows = 3, StorageContainer.behaviorOnItemChange itemChangeBehavior = null, InventoryMenu.highlightThisItem highlightMethod = null)
      : base(highlightMethod, true, true, 0, 0)
    {
      this.itemChangeBehavior = itemChangeBehavior;
      int num1 = Game1.tileSize * (capacity / rows);
      int tileSize = Game1.tileSize;
      int num2 = Game1.tileSize / 4;
      this.ItemsToGrabMenu = new InventoryMenu(Game1.viewport.Width / 2 - num1 / 2, this.yPositionOnScreen + Game1.tileSize, false, inventory, (InventoryMenu.highlightThisItem) null, capacity, rows, 0, 0, true);
      for (int index = 0; index < this.ItemsToGrabMenu.actualInventory.Count; ++index)
      {
        if (index >= this.ItemsToGrabMenu.actualInventory.Count - this.ItemsToGrabMenu.capacity / this.ItemsToGrabMenu.rows)
          this.ItemsToGrabMenu.inventory[index].downNeighborID = index + 53910;
      }
      for (int index = 0; index < this.inventory.inventory.Count; ++index)
      {
        this.inventory.inventory[index].myID = index + 53910;
        if (this.inventory.inventory[index].downNeighborID != -1)
          this.inventory.inventory[index].downNeighborID += 53910;
        if (this.inventory.inventory[index].rightNeighborID != -1)
          this.inventory.inventory[index].rightNeighborID += 53910;
        if (this.inventory.inventory[index].leftNeighborID != -1)
          this.inventory.inventory[index].leftNeighborID += 53910;
        if (this.inventory.inventory[index].upNeighborID != -1)
          this.inventory.inventory[index].upNeighborID += 53910;
        if (index < 12)
          this.inventory.inventory[index].upNeighborID = this.ItemsToGrabMenu.actualInventory.Count - this.ItemsToGrabMenu.capacity / this.ItemsToGrabMenu.rows;
      }
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
      Item heldItem = this.heldItem;
      int num = heldItem != null ? heldItem.Stack : -1;
      if (this.isWithinBounds(x, y))
      {
        base.receiveLeftClick(x, y, false);
        if (this.itemChangeBehavior == null && heldItem == null && (this.heldItem != null && Game1.oldKBState.IsKeyDown(Keys.LeftShift)))
          this.heldItem = this.ItemsToGrabMenu.tryToAddItem(this.heldItem, "Ship");
      }
      bool flag = true;
      if (this.ItemsToGrabMenu.isWithinBounds(x, y))
      {
        this.heldItem = this.ItemsToGrabMenu.leftClick(x, y, this.heldItem, false);
        if (this.heldItem != null && heldItem == null || this.heldItem != null && heldItem != null && !this.heldItem.Equals((object) heldItem))
        {
          if (this.itemChangeBehavior != null)
            flag = this.itemChangeBehavior(this.heldItem, this.ItemsToGrabMenu.getInventoryPositionOfClick(x, y), heldItem, this, true);
          if (flag)
            Game1.playSound("dwop");
        }
        if (this.heldItem == null && heldItem != null || this.heldItem != null && heldItem != null && !this.heldItem.Equals((object) heldItem))
        {
          Item old = this.heldItem;
          if (this.heldItem == null && this.ItemsToGrabMenu.getItemAt(x, y) != null && num < this.ItemsToGrabMenu.getItemAt(x, y).Stack)
          {
            old = heldItem.getOne();
            old.Stack = num;
          }
          if (this.itemChangeBehavior != null)
            flag = this.itemChangeBehavior(heldItem, this.ItemsToGrabMenu.getInventoryPositionOfClick(x, y), old, this, false);
          if (flag)
            Game1.playSound("Ship");
        }
        if (this.heldItem is StardewValley.Object && (this.heldItem as StardewValley.Object).isRecipe)
        {
          string key = this.heldItem.Name.Substring(0, this.heldItem.Name.IndexOf("Recipe") - 1);
          try
          {
            if ((this.heldItem as StardewValley.Object).category == -7)
              Game1.player.cookingRecipes.Add(key, 0);
            else
              Game1.player.craftingRecipes.Add(key, 0);
            this.poof = new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 320, 64, 64), 50f, 8, 0, new Vector2((float) (x - x % Game1.tileSize + Game1.tileSize / 4), (float) (y - y % Game1.tileSize + Game1.tileSize / 4)), false, false);
            Game1.playSound("newRecipe");
          }
          catch (Exception ex)
          {
          }
          this.heldItem = (Item) null;
        }
        else if (Game1.oldKBState.IsKeyDown(Keys.LeftShift) && Game1.player.addItemToInventoryBool(this.heldItem, false))
        {
          this.heldItem = (Item) null;
          if (this.itemChangeBehavior != null)
            flag = this.itemChangeBehavior(this.heldItem, this.ItemsToGrabMenu.getInventoryPositionOfClick(x, y), heldItem, this, true);
          if (flag)
            Game1.playSound("coin");
        }
      }
      if (this.okButton.containsPoint(x, y) && this.readyToClose())
      {
        Game1.playSound("bigDeSelect");
        Game1.exitActiveMenu();
      }
      if (!this.trashCan.containsPoint(x, y) || this.heldItem == null || !this.heldItem.canBeTrashed())
        return;
      if (this.heldItem is StardewValley.Object && Game1.player.specialItems.Contains((this.heldItem as StardewValley.Object).parentSheetIndex))
        Game1.player.specialItems.Remove((this.heldItem as StardewValley.Object).parentSheetIndex);
      this.heldItem = (Item) null;
      Game1.playSound("trashcan");
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
      int num1 = this.heldItem != null ? this.heldItem.Stack : 0;
      Item heldItem = this.heldItem;
      if (this.isWithinBounds(x, y))
      {
        base.receiveRightClick(x, y, true);
        if (this.itemChangeBehavior == null && heldItem == null && (this.heldItem != null && Game1.oldKBState.IsKeyDown(Keys.LeftShift)))
          this.heldItem = this.ItemsToGrabMenu.tryToAddItem(this.heldItem, "Ship");
      }
      if (!this.ItemsToGrabMenu.isWithinBounds(x, y))
        return;
      this.heldItem = this.ItemsToGrabMenu.rightClick(x, y, this.heldItem, false);
      if (this.heldItem != null && heldItem == null || this.heldItem != null && heldItem != null && !this.heldItem.Equals((object) heldItem) || this.heldItem != null && heldItem != null && (this.heldItem.Equals((object) heldItem) && this.heldItem.Stack != num1))
      {
        if (this.itemChangeBehavior != null)
        {
          int num2 = this.itemChangeBehavior(this.heldItem, this.ItemsToGrabMenu.getInventoryPositionOfClick(x, y), heldItem, this, true) ? 1 : 0;
        }
        Game1.playSound("dwop");
      }
      if (this.heldItem == null && heldItem != null || this.heldItem != null && heldItem != null && !this.heldItem.Equals((object) heldItem))
      {
        if (this.itemChangeBehavior != null)
        {
          int num2 = this.itemChangeBehavior(heldItem, this.ItemsToGrabMenu.getInventoryPositionOfClick(x, y), this.heldItem, this, false) ? 1 : 0;
        }
        Game1.playSound("Ship");
      }
      if (this.heldItem is StardewValley.Object && (this.heldItem as StardewValley.Object).isRecipe)
      {
        string key = this.heldItem.Name.Substring(0, this.heldItem.Name.IndexOf("Recipe") - 1);
        try
        {
          if ((this.heldItem as StardewValley.Object).category == -7)
            Game1.player.cookingRecipes.Add(key, 0);
          else
            Game1.player.craftingRecipes.Add(key, 0);
          this.poof = new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 320, 64, 64), 50f, 8, 0, new Vector2((float) (x - x % Game1.tileSize + Game1.tileSize / 4), (float) (y - y % Game1.tileSize + Game1.tileSize / 4)), false, false);
          Game1.playSound("newRecipe");
        }
        catch (Exception ex)
        {
        }
        this.heldItem = (Item) null;
      }
      else
      {
        if (!Game1.oldKBState.IsKeyDown(Keys.LeftShift) || !Game1.player.addItemToInventoryBool(this.heldItem, false))
          return;
        this.heldItem = (Item) null;
        Game1.playSound("coin");
        if (this.itemChangeBehavior == null)
          return;
        int num2 = this.itemChangeBehavior(this.heldItem, this.ItemsToGrabMenu.getInventoryPositionOfClick(x, y), heldItem, this, true) ? 1 : 0;
      }
    }

    public override void update(GameTime time)
    {
      base.update(time);
      if (this.poof == null || !this.poof.update(time))
        return;
      this.poof = (TemporaryAnimatedSprite) null;
    }

    public override void performHoverAction(int x, int y)
    {
      base.performHoverAction(x, y);
      this.ItemsToGrabMenu.hover(x, y, this.heldItem);
    }

    public override void draw(SpriteBatch b)
    {
      b.Draw(Game1.fadeToBlackRect, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), Color.Black * 0.5f);
      this.draw(b, false, false);
      Game1.drawDialogueBox(this.ItemsToGrabMenu.xPositionOnScreen - IClickableMenu.borderWidth - IClickableMenu.spaceToClearSideBorder, this.ItemsToGrabMenu.yPositionOnScreen - IClickableMenu.borderWidth - IClickableMenu.spaceToClearTopBorder, this.ItemsToGrabMenu.width + IClickableMenu.borderWidth * 2 + IClickableMenu.spaceToClearSideBorder * 2, this.ItemsToGrabMenu.height + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth * 2, false, true, (string) null, false);
      this.ItemsToGrabMenu.draw(b);
      if (this.poof != null)
        this.poof.draw(b, true, 0, 0);
      if (!this.hoverText.Equals(""))
        IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, -1, (string) null, -1, (string[]) null, (Item) null, 0, -1, -1, -1, -1, 1f, (CraftingRecipe) null);
      if (this.heldItem != null)
        this.heldItem.drawInMenu(b, new Vector2((float) (Game1.getOldMouseX() + Game1.pixelZoom * 4), (float) (Game1.getOldMouseY() + Game1.pixelZoom * 4)), 1f);
      this.drawMouse(b);
      if (this.ItemsToGrabMenu.descriptionTitle == null || this.ItemsToGrabMenu.descriptionTitle.Length <= 1)
        return;
      IClickableMenu.drawHoverText(b, this.ItemsToGrabMenu.descriptionTitle, Game1.smallFont, Game1.tileSize / 2 + (this.heldItem != null ? Game1.tileSize / 4 : -Game1.tileSize / 3), Game1.tileSize / 2 + (this.heldItem != null ? Game1.tileSize / 4 : -Game1.tileSize / 3), -1, (string) null, -1, (string[]) null, (Item) null, 0, -1, -1, -1, -1, 1f, (CraftingRecipe) null);
    }

    public delegate bool behaviorOnItemChange(Item i, int position, Item old, StorageContainer container, bool onRemoval = false);
  }
}
