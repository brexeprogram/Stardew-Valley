// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.InventoryMenu
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
  public class InventoryMenu : IClickableMenu
  {
    public string hoverText = "";
    public string hoverTitle = "";
    public string descriptionTitle = "";
    public string descriptionText = "";
    public List<ClickableComponent> inventory = new List<ClickableComponent>();
    public const int region_inventorySlot0 = 0;
    public const int region_inventorySlot1 = 1;
    public const int region_inventorySlot2 = 2;
    public const int region_inventorySlot3 = 3;
    public const int region_inventorySlot4 = 4;
    public const int region_inventorySlot5 = 5;
    public const int region_inventorySlot6 = 6;
    public const int region_inventorySlot7 = 7;
    public const int region_inventorySlot8 = 8;
    public const int region_inventorySlot9 = 9;
    public const int region_inventorySlot10 = 10;
    public const int region_inventorySlot11 = 11;
    public const int region_inventorySlot12 = 12;
    public const int region_inventorySlot13 = 13;
    public const int region_inventorySlot14 = 14;
    public const int region_inventorySlot15 = 15;
    public const int region_inventorySlot16 = 16;
    public const int region_inventorySlot17 = 17;
    public const int region_inventorySlot18 = 18;
    public const int region_inventorySlot19 = 19;
    public const int region_inventorySlot20 = 20;
    public const int region_inventorySlot21 = 21;
    public const int region_inventorySlot22 = 22;
    public const int region_inventorySlot23 = 23;
    public const int region_inventorySlot24 = 24;
    public const int region_inventorySlot25 = 25;
    public const int region_inventorySlot26 = 26;
    public const int region_inventorySlot27 = 27;
    public const int region_inventorySlot28 = 28;
    public const int region_inventorySlot29 = 29;
    public const int region_inventorySlot30 = 30;
    public const int region_inventorySlot31 = 31;
    public const int region_inventorySlot32 = 32;
    public const int region_inventorySlot33 = 33;
    public const int region_inventorySlot34 = 34;
    public const int region_inventorySlot35 = 35;
    public const int region_inventoryArea = 9000;
    public List<Item> actualInventory;
    public InventoryMenu.highlightThisItem highlightMethod;
    public ItemGrabMenu.behaviorOnItemSelect onAddItem;
    public bool playerInventory;
    public bool drawSlots;
    public bool showGrayedOutSlots;
    public int capacity;
    public int rows;
    public int horizontalGap;
    public int verticalGap;

    public InventoryMenu(int xPosition, int yPosition, bool playerInventory, List<Item> actualInventory = null, InventoryMenu.highlightThisItem highlightMethod = null, int capacity = -1, int rows = 3, int horizontalGap = 0, int verticalGap = 0, bool drawSlots = true)
      : base(xPosition, yPosition, Game1.tileSize * ((capacity == -1 ? 36 : capacity) / rows), Game1.tileSize * rows + Game1.tileSize / 4, false)
    {
      this.drawSlots = drawSlots;
      this.horizontalGap = horizontalGap;
      this.verticalGap = verticalGap;
      this.rows = rows;
      this.capacity = capacity == -1 ? 36 : capacity;
      this.playerInventory = playerInventory;
      this.actualInventory = actualInventory;
      if (actualInventory == null)
        this.actualInventory = Game1.player.items;
      for (int index = 0; index < Game1.player.maxItems; ++index)
      {
        if (Game1.player.items.Count <= index)
          Game1.player.items.Add((Item) null);
      }
      for (int index = 0; index < this.actualInventory.Count; ++index)
      {
        List<ClickableComponent> inventory = this.inventory;
        ClickableComponent clickableComponent = new ClickableComponent(new Rectangle(xPosition + index % (this.capacity / rows) * Game1.tileSize + horizontalGap * (index % (this.capacity / rows)), this.yPositionOnScreen + index / (this.capacity / rows) * (Game1.tileSize + verticalGap) + (index / (this.capacity / rows) - 1) * Game1.pixelZoom - (index > this.capacity / rows || !playerInventory || verticalGap != 0 ? 0 : Game1.tileSize / 5), Game1.tileSize, Game1.tileSize), string.Concat((object) index));
        clickableComponent.myID = index;
        int num1 = index % (this.capacity / rows) != 0 ? index - 1 : -1;
        clickableComponent.leftNeighborID = num1;
        int num2 = (index + 1) % (this.capacity / rows) != 0 ? index + 1 : 106;
        clickableComponent.rightNeighborID = num2;
        int num3 = index >= this.actualInventory.Count - this.capacity / rows ? 101 : index + this.capacity / rows;
        clickableComponent.downNeighborID = num3;
        int num4 = index < this.capacity / rows ? 12340 + index : index - this.capacity / rows;
        clickableComponent.upNeighborID = num4;
        int num5 = 9000;
        clickableComponent.region = num5;
        inventory.Add(clickableComponent);
      }
      this.highlightMethod = highlightMethod;
      if (highlightMethod != null)
        return;
      this.highlightMethod = new InventoryMenu.highlightThisItem(InventoryMenu.highlightAllItems);
    }

    public static bool highlightAllItems(Item i)
    {
      return true;
    }

    public void movePosition(int x, int y)
    {
      this.xPositionOnScreen = this.xPositionOnScreen + x;
      this.yPositionOnScreen = this.yPositionOnScreen + y;
      foreach (ClickableComponent clickableComponent in this.inventory)
      {
        clickableComponent.bounds.X += x;
        clickableComponent.bounds.Y += y;
      }
    }

    public Item tryToAddItem(Item toPlace, string sound = "coin")
    {
      if (toPlace == null)
        return (Item) null;
      int stack = toPlace.Stack;
      foreach (ClickableComponent clickableComponent in this.inventory)
      {
        int int32 = Convert.ToInt32(clickableComponent.name);
        if (int32 < this.actualInventory.Count && this.actualInventory[int32] != null && (this.highlightMethod(this.actualInventory[int32]) && this.actualInventory[int32].canStackWith(toPlace)))
        {
          toPlace.Stack = this.actualInventory[int32].addToStack(toPlace.Stack);
          if (toPlace.Stack <= 0)
          {
            try
            {
              Game1.playSound(sound);
              if (this.onAddItem != null)
                this.onAddItem(toPlace, this.playerInventory ? Game1.player : (Farmer) null);
            }
            catch (Exception ex)
            {
            }
            return (Item) null;
          }
        }
      }
      foreach (ClickableComponent clickableComponent in this.inventory)
      {
        int int32 = Convert.ToInt32(clickableComponent.name);
        if (int32 < this.actualInventory.Count && (this.actualInventory[int32] == null || this.highlightMethod(this.actualInventory[int32])))
        {
          if (this.actualInventory[int32] == null)
          {
            try
            {
              Game1.playSound(sound);
            }
            catch (Exception ex)
            {
            }
            return Utility.addItemToInventory(toPlace, int32, this.actualInventory, this.onAddItem);
          }
        }
      }
      if (toPlace.Stack < stack)
        Game1.playSound(sound);
      return toPlace;
    }

    public int getInventoryPositionOfClick(int x, int y)
    {
      for (int index = 0; index < this.inventory.Count; ++index)
      {
        if (this.inventory[index] != null && this.inventory[index].bounds.Contains(x, y))
          return Convert.ToInt32(this.inventory[index].name);
      }
      return -1;
    }

    public Item leftClick(int x, int y, Item toPlace, bool playSound = true)
    {
      foreach (ClickableComponent clickableComponent in this.inventory)
      {
        if (clickableComponent.containsPoint(x, y))
        {
          int int32 = Convert.ToInt32(clickableComponent.name);
          if (int32 < this.actualInventory.Count && (this.actualInventory[int32] == null || this.highlightMethod(this.actualInventory[int32]) || this.actualInventory[int32].canStackWith(toPlace)))
          {
            if (this.actualInventory[int32] != null)
            {
              if (toPlace != null)
              {
                if (playSound)
                  Game1.playSound("stoneStep");
                return Utility.addItemToInventory(toPlace, int32, this.actualInventory, this.onAddItem);
              }
              if (playSound)
                Game1.playSound("dwop");
              return Utility.removeItemFromInventory(int32, this.actualInventory);
            }
            if (toPlace != null)
            {
              if (playSound)
                Game1.playSound("stoneStep");
              return Utility.addItemToInventory(toPlace, int32, this.actualInventory, this.onAddItem);
            }
          }
        }
      }
      return toPlace;
    }

    public Vector2 snapToClickableComponent(int x, int y)
    {
      foreach (ClickableComponent clickableComponent in this.inventory)
      {
        if (clickableComponent.containsPoint(x, y))
          return new Vector2((float) clickableComponent.bounds.X, (float) clickableComponent.bounds.Y);
      }
      return new Vector2((float) x, (float) y);
    }

    public Item getItemAt(int x, int y)
    {
      foreach (ClickableComponent c in this.inventory)
      {
        if (c.containsPoint(x, y))
          return this.getItemFromClickableComponent(c);
      }
      return (Item) null;
    }

    public Item getItemFromClickableComponent(ClickableComponent c)
    {
      if (c != null)
      {
        int int32 = Convert.ToInt32(c.name);
        if (int32 < this.actualInventory.Count)
          return this.actualInventory[int32];
      }
      return (Item) null;
    }

    public Item rightClick(int x, int y, Item toAddTo, bool playSound = true)
    {
      foreach (ClickableComponent clickableComponent in this.inventory)
      {
        int int32 = Convert.ToInt32(clickableComponent.name);
        int x1 = x;
        int y1 = y;
        if (clickableComponent.containsPoint(x1, y1) && (this.actualInventory[int32] == null || this.highlightMethod(this.actualInventory[int32])) && (int32 < this.actualInventory.Count && this.actualInventory[int32] != null))
        {
          if (this.actualInventory[int32] is Tool && (toAddTo == null || toAddTo is StardewValley.Object) && (this.actualInventory[int32] as Tool).canThisBeAttached((StardewValley.Object) toAddTo))
            return (Item) (this.actualInventory[int32] as Tool).attach(toAddTo == null ? (StardewValley.Object) null : (StardewValley.Object) toAddTo);
          if (toAddTo == null)
          {
            if (this.actualInventory[int32].maximumStackSize() != -1)
            {
              if (int32 == Game1.player.CurrentToolIndex && this.actualInventory[int32] != null && this.actualInventory[int32].Stack == 1)
                this.actualInventory[int32].actionWhenStopBeingHeld(Game1.player);
              Item one = this.actualInventory[int32].getOne();
              if (this.actualInventory[int32].Stack > 1)
              {
                if (Game1.isOneOfTheseKeysDown(Game1.oldKBState, new InputButton[1]
                {
                  new InputButton(Keys.LeftShift)
                }))
                {
                  one.Stack = (int) Math.Ceiling((double) this.actualInventory[int32].Stack / 2.0);
                  this.actualInventory[int32].Stack = this.actualInventory[int32].Stack / 2;
                  goto label_15;
                }
              }
              if (this.actualInventory[int32].Stack == 1)
                this.actualInventory[int32] = (Item) null;
              else
                --this.actualInventory[int32].Stack;
label_15:
              if (this.actualInventory[int32] != null && this.actualInventory[int32].Stack <= 0)
                this.actualInventory[int32] = (Item) null;
              if (playSound)
                Game1.playSound("dwop");
              return one;
            }
          }
          else if (this.actualInventory[int32].canStackWith(toAddTo) && toAddTo.Stack < toAddTo.maximumStackSize())
          {
            if (Game1.isOneOfTheseKeysDown(Game1.oldKBState, new InputButton[1]
            {
              new InputButton(Keys.LeftShift)
            }))
            {
              toAddTo.Stack += (int) Math.Ceiling((double) this.actualInventory[int32].Stack / 2.0);
              this.actualInventory[int32].Stack = this.actualInventory[int32].Stack / 2;
            }
            else
            {
              ++toAddTo.Stack;
              --this.actualInventory[int32].Stack;
            }
            if (playSound)
              Game1.playSound("dwop");
            if (this.actualInventory[int32].Stack <= 0)
            {
              if (int32 == Game1.player.CurrentToolIndex)
                this.actualInventory[int32].actionWhenStopBeingHeld(Game1.player);
              this.actualInventory[int32] = (Item) null;
            }
            return toAddTo;
          }
        }
      }
      return toAddTo;
    }

    public Item hover(int x, int y, Item heldItem)
    {
      this.descriptionText = "";
      this.descriptionTitle = "";
      this.hoverText = "";
      this.hoverTitle = "";
      Item obj = (Item) null;
      foreach (ClickableComponent clickableComponent in this.inventory)
      {
        int int32 = Convert.ToInt32(clickableComponent.name);
        clickableComponent.scale = Math.Max(1f, clickableComponent.scale - 0.025f);
        if (clickableComponent.containsPoint(x, y) && (this.actualInventory[int32] == null || this.highlightMethod(this.actualInventory[int32])) && (int32 < this.actualInventory.Count && this.actualInventory[int32] != null))
        {
          this.descriptionTitle = this.actualInventory[int32].DisplayName;
          this.descriptionText = Environment.NewLine + this.actualInventory[int32].getDescription();
          clickableComponent.scale = Math.Min(clickableComponent.scale + 0.05f, 1.1f);
          string hoverBoxText = this.actualInventory[int32].getHoverBoxText(heldItem);
          if (hoverBoxText != null)
          {
            this.hoverText = hoverBoxText;
          }
          else
          {
            this.hoverText = this.actualInventory[int32].getDescription();
            this.hoverTitle = this.actualInventory[int32].DisplayName;
          }
          if (obj == null)
            obj = this.actualInventory[int32];
        }
      }
      return obj;
    }

    public override void setUpForGamePadMode()
    {
      base.setUpForGamePadMode();
      if (this.inventory == null || this.inventory.Count <= 0)
        return;
      Game1.setMousePosition(this.inventory[0].bounds.Right - this.inventory[0].bounds.Width / 8, this.inventory[0].bounds.Bottom - this.inventory[0].bounds.Height / 8);
    }

    public override void draw(SpriteBatch b)
    {
      if (this.drawSlots)
      {
        for (int index = 0; index < this.capacity; ++index)
        {
          Vector2 vector2_1 = new Vector2((float) (this.xPositionOnScreen + index % (this.capacity / this.rows) * Game1.tileSize + this.horizontalGap * (index % (this.capacity / this.rows))), (float) (this.yPositionOnScreen + index / (this.capacity / this.rows) * (Game1.tileSize + this.verticalGap) + (index / (this.capacity / this.rows) - 1) * Game1.pixelZoom - (index >= this.capacity / this.rows || !this.playerInventory || this.verticalGap != 0 ? 0 : Game1.tileSize / 5)));
          b.Draw(Game1.menuTexture, vector2_1, new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 10, -1, -1)), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
          if ((this.playerInventory || this.showGrayedOutSlots) && index >= Game1.player.maxItems)
            b.Draw(Game1.menuTexture, vector2_1, new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 57, -1, -1)), Color.White * 0.5f, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
          if (index < 12 && this.playerInventory)
          {
            string text = index == 9 ? "0" : (index == 10 ? "-" : (index == 11 ? "=" : string.Concat((object) (index + 1))));
            Vector2 vector2_2 = Game1.tinyFont.MeasureString(text);
            b.DrawString(Game1.tinyFont, text, vector2_1 + new Vector2((float) ((double) Game1.tileSize / 2.0 - (double) vector2_2.X / 2.0), -vector2_2.Y), index == Game1.player.CurrentToolIndex ? Color.Red : Color.DimGray);
          }
          if (this.actualInventory.Count > index && this.actualInventory.ElementAt<Item>(index) != null)
            this.actualInventory[index].drawInMenu(b, vector2_1, this.inventory.Count > index ? this.inventory[index].scale : 1f, !this.highlightMethod(this.actualInventory[index]) ? 0.2f : 1f, 0.865f);
        }
      }
      for (int index = 0; index < this.capacity; ++index)
      {
        Vector2 location = new Vector2((float) (this.xPositionOnScreen + index % (this.capacity / this.rows) * Game1.tileSize + this.horizontalGap * (index % (this.capacity / this.rows))), (float) (this.yPositionOnScreen + index / (this.capacity / this.rows) * (Game1.tileSize + this.verticalGap) + (index / (this.capacity / this.rows) - 1) * Game1.pixelZoom - (index >= this.capacity / this.rows || !this.playerInventory || this.verticalGap != 0 ? 0 : Game1.tileSize / 5)));
        if (this.actualInventory.Count > index && this.actualInventory.ElementAt<Item>(index) != null)
          this.actualInventory[index].drawInMenu(b, location, this.inventory.Count > index ? this.inventory[index].scale : 1f, !this.highlightMethod(this.actualInventory[index]) ? 0.2f : 1f, 0.865f);
      }
    }

    public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
    {
      base.gameWindowSizeChanged(oldBounds, newBounds);
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
    }

    public override void performHoverAction(int x, int y)
    {
    }

    public delegate bool highlightThisItem(Item i);
  }
}
