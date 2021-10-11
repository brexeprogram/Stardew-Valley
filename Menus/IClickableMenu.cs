// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.IClickableMenu
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.Objects;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace StardewValley.Menus
{
  public abstract class IClickableMenu
  {
    public static int borderWidth = Game1.tileSize / 2 + Game1.tileSize / 8;
    public static int tabYPositionRelativeToMenuY = -Game1.tileSize * 3 / 4;
    public static int spaceToClearTopBorder = Game1.tileSize * 3 / 2;
    public static int spaceToClearSideBorder = Game1.tileSize / 4;
    public const int currency_g = 0;
    public const int currency_starTokens = 1;
    public const int currency_qiCoins = 2;
    public const int greyedOutSpotIndex = 57;
    public const int outerBorderWithUpArrow = 61;
    public const int lvlMarkerRedIndex = 54;
    public const int lvlMarkerGreyIndex = 55;
    public const int borderWithDownArrowIndex = 46;
    public const int borderWithUpArrowIndex = 47;
    public const int littleHeartIndex = 49;
    public const int uncheckedBoxIndex = 50;
    public const int checkedBoxIndex = 51;
    public const int presentIconIndex = 58;
    public const int itemSpotIndex = 10;
    public const int spaceBetweenTabs = 4;
    public int width;
    public int height;
    public int xPositionOnScreen;
    public int yPositionOnScreen;
    public int currentRegion;
    public IClickableMenu.onExit exitFunction;
    public ClickableTextureComponent upperRightCloseButton;
    public bool destroy;
    public bool gamePadControlsImplemented;
    public List<ClickableComponent> allClickableComponents;
    public ClickableComponent currentlySnappedComponent;

    public IClickableMenu()
    {
    }

    public IClickableMenu(int x, int y, int width, int height, bool showUpperRightCloseButton = false)
    {
      this.initialize(x, y, width, height, showUpperRightCloseButton);
      if ((int) Game1.gameMode != 3 || Game1.player == null || Game1.eventUp)
        return;
      Game1.player.Halt();
    }

    public void initialize(int x, int y, int width, int height, bool showUpperRightCloseButton = false)
    {
      if (Game1.player != null && !Game1.player.UsingTool && !Game1.eventUp)
        Game1.player.forceCanMove();
      this.xPositionOnScreen = x;
      this.yPositionOnScreen = y;
      this.width = width;
      this.height = height;
      if (showUpperRightCloseButton)
        this.upperRightCloseButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + width - 9 * Game1.pixelZoom, this.yPositionOnScreen - Game1.pixelZoom * 2, 12 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(337, 494, 12, 12), (float) Game1.pixelZoom, false);
      for (int index = 0; index < 4; ++index)
        Game1.directionKeyPolling[index] = 250;
    }

    public virtual bool areGamePadControlsImplemented()
    {
      return false;
    }

    public ClickableComponent getLastClickableComponentInThisListThatContainsThisXCoord(List<ClickableComponent> ccList, int xCoord)
    {
      for (int index = ccList.Count - 1; index >= 0; --index)
      {
        if (ccList[index].bounds.Contains(xCoord, ccList[index].bounds.Center.Y))
          return ccList[index];
      }
      return (ClickableComponent) null;
    }

    public ClickableComponent getFirstClickableComponentInThisListThatContainsThisXCoord(List<ClickableComponent> ccList, int xCoord)
    {
      for (int index = 0; index < ccList.Count; ++index)
      {
        if (ccList[index].bounds.Contains(xCoord, ccList[index].bounds.Center.Y))
          return ccList[index];
      }
      return (ClickableComponent) null;
    }

    public ClickableComponent getLastClickableComponentInThisListThatContainsThisYCoord(List<ClickableComponent> ccList, int yCoord)
    {
      for (int index = ccList.Count - 1; index >= 0; --index)
      {
        if (ccList[index].bounds.Contains(ccList[index].bounds.Center.X, yCoord))
          return ccList[index];
      }
      return (ClickableComponent) null;
    }

    public ClickableComponent getFirstClickableComponentInThisListThatContainsThisYCoord(List<ClickableComponent> ccList, int yCoord)
    {
      for (int index = 0; index < ccList.Count; ++index)
      {
        if (ccList[index].bounds.Contains(ccList[index].bounds.Center.X, yCoord))
          return ccList[index];
      }
      return (ClickableComponent) null;
    }

    public virtual void receiveGamePadButton(Buttons b)
    {
      if (!Game1.options.snappyMenus || !Game1.options.gamepadControls || b == Buttons.A)
        ;
    }

    public void drawMouse(SpriteBatch b)
    {
      if (Game1.options.hardwareCursor)
        return;
      b.Draw(Game1.mouseCursors, new Vector2((float) Game1.getMouseX(), (float) Game1.getMouseY()), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, Game1.options.SnappyMenus ? 44 : 0, 16, 16)), Color.White * Game1.mouseCursorTransparency, 0.0f, Vector2.Zero, (float) Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
    }

    public void populateClickableComponentList()
    {
      this.allClickableComponents = new List<ClickableComponent>();
      foreach (FieldInfo field in this.GetType().GetFields())
      {
        if (field.FieldType.IsSubclassOf(typeof (ClickableComponent)) || field.FieldType == typeof (ClickableComponent))
        {
          if (field.GetValue((object) this) != null)
            this.allClickableComponents.Add((ClickableComponent) field.GetValue((object) this));
        }
        else if (field.FieldType == typeof (List<ClickableComponent>))
        {
          List<ClickableComponent> clickableComponentList = (List<ClickableComponent>) field.GetValue((object) this);
          if (clickableComponentList != null)
          {
            for (int index = clickableComponentList.Count - 1; index >= 0; --index)
            {
              if (clickableComponentList[index] != null)
                this.allClickableComponents.Add(clickableComponentList[index]);
            }
          }
        }
        else if (field.FieldType == typeof (List<ClickableTextureComponent>))
        {
          List<ClickableTextureComponent> textureComponentList = (List<ClickableTextureComponent>) field.GetValue((object) this);
          if (textureComponentList != null)
          {
            for (int index = textureComponentList.Count - 1; index >= 0; --index)
            {
              if (textureComponentList[index] != null)
                this.allClickableComponents.Add((ClickableComponent) textureComponentList[index]);
            }
          }
        }
        else if (field.FieldType == typeof (List<ClickableAnimatedComponent>))
        {
          List<ClickableAnimatedComponent> animatedComponentList = (List<ClickableAnimatedComponent>) field.GetValue((object) this);
          for (int index = animatedComponentList.Count - 1; index >= 0; --index)
          {
            if (animatedComponentList[index] != null)
              this.allClickableComponents.Add((ClickableComponent) animatedComponentList[index]);
          }
        }
        else if (field.FieldType == typeof (List<Bundle>))
        {
          List<Bundle> bundleList = (List<Bundle>) field.GetValue((object) this);
          for (int index = bundleList.Count - 1; index >= 0; --index)
          {
            if (bundleList[index] != null)
              this.allClickableComponents.Add((ClickableComponent) bundleList[index]);
          }
        }
        else if (field.FieldType == typeof (InventoryMenu))
          this.allClickableComponents.AddRange((IEnumerable<ClickableComponent>) ((InventoryMenu) field.GetValue((object) this)).inventory);
        else if (field.FieldType == typeof (List<Dictionary<ClickableTextureComponent, CraftingRecipe>>))
        {
          foreach (Dictionary<ClickableTextureComponent, CraftingRecipe> dictionary in (List<Dictionary<ClickableTextureComponent, CraftingRecipe>>) field.GetValue((object) this))
            this.allClickableComponents.AddRange((IEnumerable<ClickableComponent>) dictionary.Keys);
        }
        else if (field.FieldType == typeof (Dictionary<int, List<List<ClickableTextureComponent>>>))
        {
          foreach (List<List<ClickableTextureComponent>> textureComponentListList in ((Dictionary<int, List<List<ClickableTextureComponent>>>) field.GetValue((object) this)).Values)
          {
            foreach (IEnumerable<ClickableComponent> collection in textureComponentListList)
              this.allClickableComponents.AddRange(collection);
          }
        }
      }
    }

    public virtual void applyMovementKey(int direction)
    {
      if (this.allClickableComponents == null)
        this.populateClickableComponentList();
      this.moveCursorInDirection(direction);
    }

    public virtual void snapToDefaultClickableComponent()
    {
    }

    public void applyMovementKey(Keys key)
    {
      if (Game1.options.doesInputListContain(Game1.options.moveUpButton, key))
        this.applyMovementKey(0);
      else if (Game1.options.doesInputListContain(Game1.options.moveRightButton, key))
        this.applyMovementKey(1);
      else if (Game1.options.doesInputListContain(Game1.options.moveDownButton, key))
      {
        this.applyMovementKey(2);
      }
      else
      {
        if (!Game1.options.doesInputListContain(Game1.options.moveLeftButton, key))
          return;
        this.applyMovementKey(3);
      }
    }

    public virtual void setCurrentlySnappedComponentTo(int id)
    {
      this.currentlySnappedComponent = this.getComponentWithID(id);
    }

    public void moveCursorInDirection(int direction)
    {
      if (this.currentlySnappedComponent == null && this.allClickableComponents != null && this.allClickableComponents.Count<ClickableComponent>() > 0)
      {
        this.snapToDefaultClickableComponent();
        if (this.currentlySnappedComponent == null)
          this.currentlySnappedComponent = this.allClickableComponents.First<ClickableComponent>();
      }
      if (this.currentlySnappedComponent == null)
        return;
      ClickableComponent snappedComponent = this.currentlySnappedComponent;
      switch (direction)
      {
        case 0:
          if (this.currentlySnappedComponent.upNeighborID == -99999)
            this.snapToDefaultClickableComponent();
          else if (this.currentlySnappedComponent.upNeighborID == -7777)
            this.customSnapBehavior(0, this.currentlySnappedComponent.region, this.currentlySnappedComponent.myID);
          else
            this.currentlySnappedComponent = this.getComponentWithID(this.currentlySnappedComponent.upNeighborID);
          if (this.currentlySnappedComponent != null && (snappedComponent == null || snappedComponent.upNeighborID != -7777) && (!this.currentlySnappedComponent.downNeighborImmutable && !this.currentlySnappedComponent.fullyImmutable))
            this.currentlySnappedComponent.downNeighborID = snappedComponent.myID;
          if (this.currentlySnappedComponent == null)
          {
            this.noSnappedComponentFound(0, snappedComponent.region, snappedComponent.myID);
            break;
          }
          break;
        case 1:
          if (this.currentlySnappedComponent.rightNeighborID == -99999)
            this.snapToDefaultClickableComponent();
          else if (this.currentlySnappedComponent.rightNeighborID == -7777)
            this.customSnapBehavior(1, this.currentlySnappedComponent.region, this.currentlySnappedComponent.myID);
          else
            this.currentlySnappedComponent = this.getComponentWithID(this.currentlySnappedComponent.rightNeighborID);
          if (this.currentlySnappedComponent != null && (snappedComponent == null || snappedComponent.rightNeighborID != -7777) && (!this.currentlySnappedComponent.leftNeighborImmutable && !this.currentlySnappedComponent.fullyImmutable))
            this.currentlySnappedComponent.leftNeighborID = snappedComponent.myID;
          if (this.currentlySnappedComponent == null && snappedComponent.tryDefaultIfNoRightNeighborExists)
          {
            this.snapToDefaultClickableComponent();
            break;
          }
          if (this.currentlySnappedComponent == null)
          {
            this.noSnappedComponentFound(1, snappedComponent.region, snappedComponent.myID);
            break;
          }
          break;
        case 2:
          if (this.currentlySnappedComponent.downNeighborID == -99999)
            this.snapToDefaultClickableComponent();
          else if (this.currentlySnappedComponent.downNeighborID == -7777)
            this.customSnapBehavior(2, this.currentlySnappedComponent.region, this.currentlySnappedComponent.myID);
          else
            this.currentlySnappedComponent = this.getComponentWithID(this.currentlySnappedComponent.downNeighborID);
          if (this.currentlySnappedComponent != null && (snappedComponent == null || snappedComponent.downNeighborID != -7777) && (!this.currentlySnappedComponent.upNeighborImmutable && !this.currentlySnappedComponent.fullyImmutable))
            this.currentlySnappedComponent.upNeighborID = snappedComponent.myID;
          if (this.currentlySnappedComponent == null && snappedComponent.tryDefaultIfNoDownNeighborExists)
          {
            this.snapToDefaultClickableComponent();
            break;
          }
          if (this.currentlySnappedComponent == null)
          {
            this.noSnappedComponentFound(2, snappedComponent.region, snappedComponent.myID);
            break;
          }
          break;
        case 3:
          if (this.currentlySnappedComponent.leftNeighborID == -99999)
            this.snapToDefaultClickableComponent();
          else if (this.currentlySnappedComponent.leftNeighborID == -7777)
            this.customSnapBehavior(3, this.currentlySnappedComponent.region, this.currentlySnappedComponent.myID);
          else
            this.currentlySnappedComponent = this.getComponentWithID(this.currentlySnappedComponent.leftNeighborID);
          if (this.currentlySnappedComponent != null && (snappedComponent == null || snappedComponent.leftNeighborID != -7777) && (!this.currentlySnappedComponent.rightNeighborImmutable && !this.currentlySnappedComponent.fullyImmutable))
            this.currentlySnappedComponent.rightNeighborID = snappedComponent.myID;
          if (this.currentlySnappedComponent == null)
          {
            this.noSnappedComponentFound(3, snappedComponent.region, snappedComponent.myID);
            break;
          }
          break;
      }
      if (this.currentlySnappedComponent != null && snappedComponent != null && this.currentlySnappedComponent.region != snappedComponent.region)
        this.actionOnRegionChange(snappedComponent.region, this.currentlySnappedComponent.region);
      if (this.currentlySnappedComponent == null)
        this.currentlySnappedComponent = snappedComponent;
      this.snapCursorToCurrentSnappedComponent();
      Game1.playSound("shiny4");
      Game1.debugOutput = "snapped Component ID: " + (object) this.currentlySnappedComponent.myID ?? "";
    }

    public virtual void snapCursorToCurrentSnappedComponent()
    {
      if (this.currentlySnappedComponent == null)
        return;
      Game1.setMousePosition(this.currentlySnappedComponent.bounds.Right - this.currentlySnappedComponent.bounds.Width / 4, this.currentlySnappedComponent.bounds.Bottom - this.currentlySnappedComponent.bounds.Height / 4);
    }

    protected virtual void noSnappedComponentFound(int direction, int oldRegion, int oldID)
    {
    }

    protected virtual void customSnapBehavior(int direction, int oldRegion, int oldID)
    {
    }

    protected virtual void actionOnRegionChange(int oldRegion, int newRegion)
    {
    }

    public ClickableComponent getComponentWithID(int id)
    {
      if (this.allClickableComponents != null)
      {
        for (int index = 0; index < this.allClickableComponents.Count; ++index)
        {
          if (this.allClickableComponents[index] != null && this.allClickableComponents[index].myID == id && this.allClickableComponents[index].visible)
            return this.allClickableComponents[index];
        }
        for (int index = 0; index < this.allClickableComponents.Count; ++index)
        {
          if (this.allClickableComponents[index] != null && this.allClickableComponents[index].myAlternateID == id && this.allClickableComponents[index].visible)
            return this.allClickableComponents[index];
        }
      }
      return (ClickableComponent) null;
    }

    public void initializeUpperRightCloseButton()
    {
      this.upperRightCloseButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width - 9 * Game1.pixelZoom, this.yPositionOnScreen - Game1.pixelZoom * 2, 12 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(337, 494, 12, 12), (float) Game1.pixelZoom, false);
    }

    public virtual void drawBackground(SpriteBatch b)
    {
      if (this is ShopMenu)
      {
        int num1 = 0;
        while (num1 < Game1.viewport.Width)
        {
          int num2 = 0;
          while (num2 < Game1.viewport.Height)
          {
            b.Draw(Game1.mouseCursors, new Vector2((float) num1, (float) num2), new Rectangle?(new Rectangle(527, 0, 100, 96)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.08f);
            num2 += 96 * Game1.pixelZoom;
          }
          num1 += 100 * Game1.pixelZoom;
        }
      }
      else
      {
        if (Game1.isDarkOut())
          b.Draw(Game1.mouseCursors, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), new Rectangle?(new Rectangle(639, 858, 1, 144)), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.9f);
        else if (Game1.isRaining)
          b.Draw(Game1.mouseCursors, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), new Rectangle?(new Rectangle(640, 858, 1, 184)), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.9f);
        else
          b.Draw(Game1.mouseCursors, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), new Rectangle?(new Rectangle(639 + Utility.getSeasonNumber(Game1.currentSeason), 1051, 1, 400)), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.9f);
        b.Draw(Game1.mouseCursors, new Vector2((float) (-30 * Game1.pixelZoom), (float) (Game1.viewport.Height - 148 * Game1.pixelZoom)), new Rectangle?(new Rectangle(0, Game1.currentSeason.Equals("winter") ? 1035 : (Game1.isRaining || Game1.isDarkOut() ? 886 : 737), 639, 148)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.08f);
        b.Draw(Game1.mouseCursors, new Vector2((float) (-30 * Game1.pixelZoom + 639 * Game1.pixelZoom), (float) (Game1.viewport.Height - 148 * Game1.pixelZoom)), new Rectangle?(new Rectangle(0, Game1.currentSeason.Equals("winter") ? 1035 : (Game1.isRaining || Game1.isDarkOut() ? 886 : 737), 639, 148)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.08f);
        if (!Game1.isRaining)
          return;
        b.Draw(Game1.staminaRect, Utility.xTileToMicrosoftRectangle(Game1.viewport), Color.Blue * 0.2f);
      }
    }

    public virtual bool showWithoutTransparencyIfOptionIsSet()
    {
      return this is GameMenu || this is ShopMenu || (this is WheelSpinGame || this is ItemGrabMenu);
    }

    public virtual void clickAway()
    {
    }

    public virtual void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
    {
      this.xPositionOnScreen = (int) ((double) newBounds.Width * ((double) this.xPositionOnScreen / (double) oldBounds.Width));
      this.yPositionOnScreen = (int) ((double) newBounds.Height * ((double) this.yPositionOnScreen / (double) oldBounds.Height));
    }

    public virtual void setUpForGamePadMode()
    {
    }

    public virtual void releaseLeftClick(int x, int y)
    {
    }

    public virtual void leftClickHeld(int x, int y)
    {
    }

    public virtual void receiveLeftClick(int x, int y, bool playSound = true)
    {
      if (this.upperRightCloseButton == null || !this.readyToClose() || !this.upperRightCloseButton.containsPoint(x, y))
        return;
      if (playSound)
        Game1.playSound("bigDeSelect");
      this.exitThisMenu(true);
    }

    public virtual bool overrideSnappyMenuCursorMovementBan()
    {
      return false;
    }

    public abstract void receiveRightClick(int x, int y, bool playSound = true);

    public virtual void receiveKeyPress(Keys key)
    {
      if (key == Keys.None)
        return;
      if (Game1.options.doesInputListContain(Game1.options.menuButton, key) && this.readyToClose())
      {
        this.exitThisMenu(true);
      }
      else
      {
        if (!Game1.options.snappyMenus || !Game1.options.gamepadControls || this.overrideSnappyMenuCursorMovementBan())
          return;
        this.applyMovementKey(key);
      }
    }

    public virtual void gamePadButtonHeld(Buttons b)
    {
    }

    public virtual ClickableComponent getCurrentlySnappedComponent()
    {
      return this.currentlySnappedComponent;
    }

    public virtual void receiveScrollWheelAction(int direction)
    {
    }

    public virtual void performHoverAction(int x, int y)
    {
      if (this.upperRightCloseButton == null)
        return;
      this.upperRightCloseButton.tryHover(x, y, 0.5f);
    }

    public virtual void draw(SpriteBatch b)
    {
      if (this.upperRightCloseButton == null)
        return;
      this.upperRightCloseButton.draw(b);
    }

    public virtual bool isWithinBounds(int x, int y)
    {
      if (x - this.xPositionOnScreen < this.width && x - this.xPositionOnScreen >= 0 && y - this.yPositionOnScreen < this.height)
        return y - this.yPositionOnScreen >= 0;
      return false;
    }

    public virtual void update(GameTime time)
    {
    }

    public void exitThisMenuNoSound()
    {
      Game1.exitActiveMenu();
      if (this.exitFunction == null)
        return;
      this.exitFunction();
    }

    public void exitThisMenu(bool playSound = true)
    {
      if (playSound)
        Game1.playSound("bigDeSelect");
      Game1.exitActiveMenu();
      if (this.exitFunction == null)
        return;
      this.exitFunction();
    }

    public virtual bool autoCenterMouseCursorForGamepad()
    {
      return true;
    }

    public virtual void emergencyShutDown()
    {
    }

    public virtual bool readyToClose()
    {
      return true;
    }

    protected void drawHorizontalPartition(SpriteBatch b, int yPosition, bool small = false)
    {
      if (small)
      {
        b.Draw(Game1.menuTexture, new Rectangle(this.xPositionOnScreen + Game1.tileSize / 2, yPosition, this.width - Game1.tileSize, Game1.tileSize), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 25, -1, -1)), Color.White);
      }
      else
      {
        b.Draw(Game1.menuTexture, new Vector2((float) this.xPositionOnScreen, (float) yPosition), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 4, -1, -1)), Color.White);
        b.Draw(Game1.menuTexture, new Rectangle(this.xPositionOnScreen + Game1.tileSize, yPosition, this.width - Game1.tileSize * 2, Game1.tileSize), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 6, -1, -1)), Color.White);
        b.Draw(Game1.menuTexture, new Vector2((float) (this.xPositionOnScreen + this.width - Game1.tileSize), (float) yPosition), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 7, -1, -1)), Color.White);
      }
    }

    protected void drawVerticalPartition(SpriteBatch b, int xPosition, bool small = false)
    {
      if (small)
      {
        b.Draw(Game1.menuTexture, new Rectangle(xPosition, this.yPositionOnScreen + Game1.tileSize + Game1.tileSize / 2, Game1.tileSize, this.height - Game1.tileSize * 2), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 26, -1, -1)), Color.White);
      }
      else
      {
        b.Draw(Game1.menuTexture, new Vector2((float) xPosition, (float) (this.yPositionOnScreen + Game1.tileSize)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 1, -1, -1)), Color.White);
        b.Draw(Game1.menuTexture, new Rectangle(xPosition, this.yPositionOnScreen + Game1.tileSize * 2, Game1.tileSize, this.height - Game1.tileSize * 3), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 5, -1, -1)), Color.White);
        b.Draw(Game1.menuTexture, new Vector2((float) xPosition, (float) (this.yPositionOnScreen + this.height - Game1.tileSize)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 13, -1, -1)), Color.White);
      }
    }

    protected void drawVerticalIntersectingPartition(SpriteBatch b, int xPosition, int yPosition)
    {
      b.Draw(Game1.menuTexture, new Vector2((float) xPosition, (float) yPosition), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 59, -1, -1)), Color.White);
      b.Draw(Game1.menuTexture, new Rectangle(xPosition, yPosition + Game1.tileSize, Game1.tileSize, this.yPositionOnScreen + this.height - Game1.tileSize - yPosition - Game1.tileSize), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 63, -1, -1)), Color.White);
      b.Draw(Game1.menuTexture, new Vector2((float) xPosition, (float) (this.yPositionOnScreen + this.height - Game1.tileSize)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 62, -1, -1)), Color.White);
    }

    protected void drawVerticalUpperIntersectingPartition(SpriteBatch b, int xPosition, int partitionHeight)
    {
      b.Draw(Game1.menuTexture, new Vector2((float) xPosition, (float) (this.yPositionOnScreen + Game1.tileSize)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 44, -1, -1)), Color.White);
      b.Draw(Game1.menuTexture, new Rectangle(xPosition, this.yPositionOnScreen + Game1.tileSize * 2, Game1.tileSize, partitionHeight - Game1.tileSize / 2), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 63, -1, -1)), Color.White);
      b.Draw(Game1.menuTexture, new Vector2((float) xPosition, (float) (this.yPositionOnScreen + partitionHeight + Game1.tileSize)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 39, -1, -1)), Color.White);
    }

    public static void drawTextureBox(SpriteBatch b, int x, int y, int width, int height, Color color)
    {
      IClickableMenu.drawTextureBox(b, Game1.menuTexture, new Rectangle(0, 256, 60, 60), x, y, width, height, color, 1f, true);
    }

    public static void drawTextureBox(SpriteBatch b, Texture2D texture, Rectangle sourceRect, int x, int y, int width, int height, Color color, float scale = 1f, bool drawShadow = true)
    {
      int num = sourceRect.Width / 3;
      if (drawShadow)
      {
        b.Draw(texture, new Vector2((float) (x + width - (int) ((double) num * (double) scale) - Game1.pixelZoom * 2), (float) (y + Game1.pixelZoom * 2)), new Rectangle?(new Rectangle(sourceRect.X + num * 2, sourceRect.Y, num, num)), Color.Black * 0.4f, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.77f);
        b.Draw(texture, new Vector2((float) (x - Game1.pixelZoom * 2), (float) (y + height - (int) ((double) num * (double) scale) + Game1.pixelZoom * 2)), new Rectangle?(new Rectangle(sourceRect.X, num * 2 + sourceRect.Y, num, num)), Color.Black * 0.4f, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.77f);
        b.Draw(texture, new Vector2((float) (x + width - (int) ((double) num * (double) scale) - Game1.pixelZoom * 2), (float) (y + height - (int) ((double) num * (double) scale) + Game1.pixelZoom * 2)), new Rectangle?(new Rectangle(sourceRect.X + num * 2, num * 2 + sourceRect.Y, num, num)), Color.Black * 0.4f, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.77f);
        b.Draw(texture, new Rectangle(x + (int) ((double) num * (double) scale) - Game1.pixelZoom * 2, y + Game1.pixelZoom * 2, width - (int) ((double) num * (double) scale) * 2, (int) ((double) num * (double) scale)), new Rectangle?(new Rectangle(sourceRect.X + num, sourceRect.Y, num, num)), Color.Black * 0.4f, 0.0f, Vector2.Zero, SpriteEffects.None, 0.77f);
        b.Draw(texture, new Rectangle(x + (int) ((double) num * (double) scale) - Game1.pixelZoom * 2, y + height - (int) ((double) num * (double) scale) + Game1.pixelZoom * 2, width - (int) ((double) num * (double) scale) * 2, (int) ((double) num * (double) scale)), new Rectangle?(new Rectangle(sourceRect.X + num, num * 2 + sourceRect.Y, num, num)), Color.Black * 0.4f, 0.0f, Vector2.Zero, SpriteEffects.None, 0.77f);
        b.Draw(texture, new Rectangle(x - Game1.pixelZoom * 2, y + (int) ((double) num * (double) scale) + Game1.pixelZoom * 2, (int) ((double) num * (double) scale), height - (int) ((double) num * (double) scale) * 2), new Rectangle?(new Rectangle(sourceRect.X, num + sourceRect.Y, num, num)), Color.Black * 0.4f, 0.0f, Vector2.Zero, SpriteEffects.None, 0.77f);
        b.Draw(texture, new Rectangle(x + width - (int) ((double) num * (double) scale) - Game1.pixelZoom * 2, y + (int) ((double) num * (double) scale) + Game1.pixelZoom * 2, (int) ((double) num * (double) scale), height - (int) ((double) num * (double) scale) * 2), new Rectangle?(new Rectangle(sourceRect.X + num * 2, num + sourceRect.Y, num, num)), Color.Black * 0.4f, 0.0f, Vector2.Zero, SpriteEffects.None, 0.77f);
        b.Draw(texture, new Rectangle((int) ((double) num * (double) scale / 2.0) + x - Game1.pixelZoom * 2, (int) ((double) num * (double) scale / 2.0) + y + Game1.pixelZoom * 2, width - (int) ((double) num * (double) scale), height - (int) ((double) num * (double) scale)), new Rectangle?(new Rectangle(num + sourceRect.X, num + sourceRect.Y, num, num)), Color.Black * 0.4f, 0.0f, Vector2.Zero, SpriteEffects.None, 0.77f);
      }
      b.Draw(texture, new Rectangle((int) ((double) num * (double) scale) + x, (int) ((double) num * (double) scale) + y, width - (int) ((double) num * (double) scale * 2.0), height - (int) ((double) num * (double) scale * 2.0)), new Rectangle?(new Rectangle(num + sourceRect.X, num + sourceRect.Y, num, num)), color, 0.0f, Vector2.Zero, SpriteEffects.None, (float) (0.800000011920929 - (double) y * 9.99999997475243E-07));
      b.Draw(texture, new Vector2((float) x, (float) y), new Rectangle?(new Rectangle(sourceRect.X, sourceRect.Y, num, num)), color, 0.0f, Vector2.Zero, scale, SpriteEffects.None, (float) (0.800000011920929 - (double) y * 9.99999997475243E-07));
      b.Draw(texture, new Vector2((float) (x + width - (int) ((double) num * (double) scale)), (float) y), new Rectangle?(new Rectangle(sourceRect.X + num * 2, sourceRect.Y, num, num)), color, 0.0f, Vector2.Zero, scale, SpriteEffects.None, (float) (0.800000011920929 - (double) y * 9.99999997475243E-07));
      b.Draw(texture, new Vector2((float) x, (float) (y + height - (int) ((double) num * (double) scale))), new Rectangle?(new Rectangle(sourceRect.X, num * 2 + sourceRect.Y, num, num)), color, 0.0f, Vector2.Zero, scale, SpriteEffects.None, (float) (0.800000011920929 - (double) y * 9.99999997475243E-07));
      b.Draw(texture, new Vector2((float) (x + width - (int) ((double) num * (double) scale)), (float) (y + height - (int) ((double) num * (double) scale))), new Rectangle?(new Rectangle(sourceRect.X + num * 2, num * 2 + sourceRect.Y, num, num)), color, 0.0f, Vector2.Zero, scale, SpriteEffects.None, (float) (0.800000011920929 - (double) y * 9.99999997475243E-07));
      b.Draw(texture, new Rectangle(x + (int) ((double) num * (double) scale), y, width - (int) ((double) num * (double) scale) * 2, (int) ((double) num * (double) scale)), new Rectangle?(new Rectangle(sourceRect.X + num, sourceRect.Y, num, num)), color, 0.0f, Vector2.Zero, SpriteEffects.None, (float) (0.800000011920929 - (double) y * 9.99999997475243E-07));
      b.Draw(texture, new Rectangle(x + (int) ((double) num * (double) scale), y + height - (int) ((double) num * (double) scale), width - (int) ((double) num * (double) scale) * 2, (int) ((double) num * (double) scale)), new Rectangle?(new Rectangle(sourceRect.X + num, num * 2 + sourceRect.Y, num, num)), color, 0.0f, Vector2.Zero, SpriteEffects.None, (float) (0.800000011920929 - (double) y * 9.99999997475243E-07));
      b.Draw(texture, new Rectangle(x, y + (int) ((double) num * (double) scale), (int) ((double) num * (double) scale), height - (int) ((double) num * (double) scale) * 2), new Rectangle?(new Rectangle(sourceRect.X, num + sourceRect.Y, num, num)), color, 0.0f, Vector2.Zero, SpriteEffects.None, (float) (0.800000011920929 - (double) y * 9.99999997475243E-07));
      b.Draw(texture, new Rectangle(x + width - (int) ((double) num * (double) scale), y + (int) ((double) num * (double) scale), (int) ((double) num * (double) scale), height - (int) ((double) num * (double) scale) * 2), new Rectangle?(new Rectangle(sourceRect.X + num * 2, num + sourceRect.Y, num, num)), color, 0.0f, Vector2.Zero, SpriteEffects.None, (float) (0.800000011920929 - (double) y * 9.99999997475243E-07));
    }

    public void drawBorderLabel(SpriteBatch b, string text, SpriteFont font, int x, int y)
    {
      int x1 = (int) font.MeasureString(text).X;
      y += Game1.tileSize - Game1.pixelZoom * 3;
      b.Draw(Game1.mouseCursors, new Vector2((float) x, (float) y), new Rectangle?(new Rectangle(256, 267, 6, 16)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.87f);
      b.Draw(Game1.mouseCursors, new Vector2((float) (x + 6 * Game1.pixelZoom), (float) y), new Rectangle?(new Rectangle(262, 267, 1, 16)), Color.White, 0.0f, Vector2.Zero, new Vector2((float) x1, (float) Game1.pixelZoom), SpriteEffects.None, 0.87f);
      b.Draw(Game1.mouseCursors, new Vector2((float) (x + 6 * Game1.pixelZoom + x1), (float) y), new Rectangle?(new Rectangle(263, 267, 6, 16)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.87f);
      Utility.drawTextWithShadow(b, text, font, new Vector2((float) (x + 6 * Game1.pixelZoom), (float) (y + Game1.pixelZoom * 5)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
    }

    public static void drawToolTip(SpriteBatch b, string hoverText, string hoverTitle, Item hoveredItem, bool heldItem = false, int healAmountToDisplay = -1, int currencySymbol = 0, int extraItemToShowIndex = -1, int extraItemToShowAmount = -1, CraftingRecipe craftingIngredients = null, int moneyAmountToShowAtBottom = -1)
    {
      bool flag = hoveredItem != null && hoveredItem is StardewValley.Object && (hoveredItem as StardewValley.Object).edibility != -300;
      SpriteBatch b1 = b;
      string text = hoverText;
      SpriteFont smallFont = Game1.smallFont;
      int xOffset = heldItem ? Game1.tileSize / 2 + 8 : 0;
      int yOffset = heldItem ? Game1.tileSize / 2 + 8 : 0;
      int moneyAmountToDisplayAtBottom = moneyAmountToShowAtBottom;
      string boldTitleText = hoverTitle;
      int healAmountToDisplay1 = flag ? (hoveredItem as StardewValley.Object).edibility : -1;
      string[] buffIconsToDisplay;
      if (flag)
      {
        if (Game1.objectInformation[(hoveredItem as StardewValley.Object).parentSheetIndex].Split('/').Length > 7)
        {
          buffIconsToDisplay = Game1.objectInformation[(hoveredItem as StardewValley.Object).parentSheetIndex].Split('/')[7].Split(' ');
          goto label_4;
        }
      }
      buffIconsToDisplay = (string[]) null;
label_4:
      Item hoveredItem1 = hoveredItem;
      int currencySymbol1 = currencySymbol;
      int extraItemToShowIndex1 = extraItemToShowIndex;
      int extraItemToShowAmount1 = extraItemToShowAmount;
      int overrideX = -1;
      int overrideY = -1;
      double num = 1.0;
      CraftingRecipe craftingIngredients1 = craftingIngredients;
      IClickableMenu.drawHoverText(b1, text, smallFont, xOffset, yOffset, moneyAmountToDisplayAtBottom, boldTitleText, healAmountToDisplay1, buffIconsToDisplay, hoveredItem1, currencySymbol1, extraItemToShowIndex1, extraItemToShowAmount1, overrideX, overrideY, (float) num, craftingIngredients1);
    }

    public static void drawHoverText(SpriteBatch b, string text, SpriteFont font, int xOffset = 0, int yOffset = 0, int moneyAmountToDisplayAtBottom = -1, string boldTitleText = null, int healAmountToDisplay = -1, string[] buffIconsToDisplay = null, Item hoveredItem = null, int currencySymbol = 0, int extraItemToShowIndex = -1, int extraItemToShowAmount = -1, int overrideX = -1, int overrideY = -1, float alpha = 1f, CraftingRecipe craftingIngredients = null)
    {
      if (text == null || text.Length == 0)
        return;
      if (boldTitleText != null && boldTitleText.Length == 0)
        boldTitleText = (string) null;
      int num1 = 20;
      int num2 = Math.Max(healAmountToDisplay != -1 ? (int) font.MeasureString(healAmountToDisplay.ToString() + "+ Energy" + (object) (Game1.tileSize / 2)).X : 0, Math.Max((int) font.MeasureString(text).X, boldTitleText != null ? (int) Game1.dialogueFont.MeasureString(boldTitleText).X : 0)) + Game1.tileSize / 2;
      int height = Math.Max(num1 * 3, (int) font.MeasureString(text).Y + Game1.tileSize / 2 + (moneyAmountToDisplayAtBottom > -1 ? (int) ((double) font.MeasureString(string.Concat((object) moneyAmountToDisplayAtBottom)).Y + 4.0) : 0) + (boldTitleText != null ? (int) ((double) Game1.dialogueFont.MeasureString(boldTitleText).Y + (double) (Game1.tileSize / 4)) : 0) + (healAmountToDisplay != -1 ? 38 : 0));
      if (extraItemToShowIndex != -1)
      {
        string[] strArray = Game1.objectInformation[extraItemToShowIndex].Split('/');
        string str = strArray[0];
        if (LocalizedContentManager.CurrentLanguageCode != LocalizedContentManager.LanguageCode.en)
          str = strArray[strArray.Length - 1];
        string text1 = Game1.content.LoadString("Strings\\UI:ItemHover_Requirements", (object) extraItemToShowAmount, (object) str);
        int num3 = Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, extraItemToShowIndex, 16, 16).Width * 2 * Game1.pixelZoom;
        num2 = Math.Max(num2, num3 + (int) font.MeasureString(text1).X);
      }
      if (buffIconsToDisplay != null)
      {
        foreach (string str in buffIconsToDisplay)
        {
          if (!str.Equals("0"))
            height += 34;
        }
        height += 4;
      }
      string text2 = (string) null;
      if (hoveredItem != null)
      {
        height += (Game1.tileSize + 4) * hoveredItem.attachmentSlots();
        text2 = hoveredItem.getCategoryName();
        if (text2.Length > 0)
        {
          num2 = Math.Max(num2, (int) font.MeasureString(text2).X + Game1.tileSize / 2);
          height += (int) font.MeasureString("T").Y;
        }
        int num3 = 9999;
        int num4 = 15 * Game1.pixelZoom + Game1.tileSize / 2;
        if (hoveredItem is MeleeWeapon)
        {
          height = Math.Max(num1 * 3, (boldTitleText != null ? (int) ((double) Game1.dialogueFont.MeasureString(boldTitleText).Y + (double) (Game1.tileSize / 4)) : 0) + Game1.tileSize / 2) + (int) font.MeasureString("T").Y + (moneyAmountToDisplayAtBottom > -1 ? (int) ((double) font.MeasureString(string.Concat((object) moneyAmountToDisplayAtBottom)).Y + 4.0) : 0) + (hoveredItem.Name == "Scythe" ? 0 : (hoveredItem as MeleeWeapon).getNumberOfDescriptionCategories() * Game1.pixelZoom * 12) + (int) font.MeasureString(Game1.parseText((hoveredItem as MeleeWeapon).description, Game1.smallFont, Game1.tileSize * 4 + Game1.tileSize / 4)).Y;
          num2 = (int) Math.Max((float) num2, Math.Max((float) ((double) font.MeasureString(Game1.content.LoadString("Strings\\UI:ItemHover_Damage", (object) num3, (object) num3)).X + (double) num4), Math.Max((float) ((double) font.MeasureString(Game1.content.LoadString("Strings\\UI:ItemHover_Speed", (object) num3)).X + (double) num4), Math.Max((float) ((double) font.MeasureString(Game1.content.LoadString("Strings\\UI:ItemHover_DefenseBonus", (object) num3)).X + (double) num4), Math.Max((float) ((double) font.MeasureString(Game1.content.LoadString("Strings\\UI:ItemHover_CritChanceBonus", (object) num3)).X + (double) num4), Math.Max((float) ((double) font.MeasureString(Game1.content.LoadString("Strings\\UI:ItemHover_CritPowerBonus", (object) num3)).X + (double) num4), (float) ((double) font.MeasureString(Game1.content.LoadString("Strings\\UI:ItemHover_Weight", (object) num3)).X + (double) num4)))))));
        }
        else if (hoveredItem is Boots)
        {
          height = height - (int) font.MeasureString(text).Y + (int) ((double) ((hoveredItem as Boots).getNumberOfDescriptionCategories() * Game1.pixelZoom * 12) + (double) font.MeasureString(Game1.parseText((hoveredItem as Boots).description, Game1.smallFont, Game1.tileSize * 4 + Game1.tileSize / 4)).Y);
          num2 = (int) Math.Max((float) num2, Math.Max((float) ((double) font.MeasureString(Game1.content.LoadString("Strings\\UI:ItemHover_DefenseBonus", (object) num3)).X + (double) num4), (float) ((double) font.MeasureString(Game1.content.LoadString("Strings\\UI:ItemHover_ImmunityBonus", (object) num3)).X + (double) num4)));
        }
        else if (hoveredItem is StardewValley.Object && (hoveredItem as StardewValley.Object).edibility != -300)
        {
          if (healAmountToDisplay == -1)
            height += (Game1.tileSize / 2 + Game1.pixelZoom * 2) * (healAmountToDisplay > 0 ? 2 : 1);
          else
            height += Game1.tileSize / 2 + Game1.pixelZoom * 2;
          healAmountToDisplay = (int) Math.Ceiling((double) (hoveredItem as StardewValley.Object).Edibility * 2.5) + (hoveredItem as StardewValley.Object).quality * (hoveredItem as StardewValley.Object).Edibility;
          num2 = (int) Math.Max((float) num2, Math.Max((float) ((double) font.MeasureString(Game1.content.LoadString("Strings\\UI:ItemHover_Energy", (object) num3)).X + (double) num4), (float) ((double) font.MeasureString(Game1.content.LoadString("Strings\\UI:ItemHover_Health", (object) num3)).X + (double) num4)));
        }
        if (buffIconsToDisplay != null)
        {
          for (int index = 0; index < buffIconsToDisplay.Length; ++index)
          {
            if (!buffIconsToDisplay[index].Equals("0") && index <= 11)
              num2 = (int) Math.Max((float) num2, (float) ((double) font.MeasureString(Game1.content.LoadString("Strings\\UI:ItemHover_Buff" + (object) index, (object) num3)).X + (double) num4));
          }
        }
      }
      if (craftingIngredients != null)
      {
        num2 = Math.Max((int) Game1.dialogueFont.MeasureString(boldTitleText).X + Game1.pixelZoom * 3, Game1.tileSize * 6);
        height += craftingIngredients.getDescriptionHeight(num2 - Game1.pixelZoom * 2) + (healAmountToDisplay == -1 ? -Game1.tileSize / 2 : 0) + Game1.pixelZoom * 3;
      }
      if (hoveredItem is FishingRod && moneyAmountToDisplayAtBottom > -1)
        height += (int) font.MeasureString("T").Y;
      int x = Game1.getOldMouseX() + Game1.tileSize / 2 + xOffset;
      int y1 = Game1.getOldMouseY() + Game1.tileSize / 2 + yOffset;
      if (overrideX != -1)
        x = overrideX;
      if (overrideY != -1)
        y1 = overrideY;
      int num5 = x + num2;
      Rectangle safeArea = Utility.getSafeArea();
      int right1 = safeArea.Right;
      if (num5 > right1)
      {
        safeArea = Utility.getSafeArea();
        x = safeArea.Right - num2;
        y1 += Game1.tileSize / 4;
      }
      int num6 = y1 + height;
      safeArea = Utility.getSafeArea();
      int bottom = safeArea.Bottom;
      if (num6 > bottom)
      {
        x += Game1.tileSize / 4;
        int num3 = x + num2;
        safeArea = Utility.getSafeArea();
        int right2 = safeArea.Right;
        if (num3 > right2)
        {
          safeArea = Utility.getSafeArea();
          x = safeArea.Right - num2;
        }
        safeArea = Utility.getSafeArea();
        y1 = safeArea.Bottom - height;
      }
      IClickableMenu.drawTextureBox(b, Game1.menuTexture, new Rectangle(0, 256, 60, 60), x, y1, num2 + (craftingIngredients != null ? Game1.tileSize / 3 : 0), height, Color.White * alpha, 1f, true);
      if (boldTitleText != null)
      {
        IClickableMenu.drawTextureBox(b, Game1.menuTexture, new Rectangle(0, 256, 60, 60), x, y1, num2 + (craftingIngredients != null ? Game1.tileSize / 3 : 0), (int) Game1.dialogueFont.MeasureString(boldTitleText).Y + Game1.tileSize / 2 + (hoveredItem == null || text2.Length <= 0 ? 0 : (int) font.MeasureString("asd").Y) - Game1.pixelZoom, Color.White * alpha, 1f, false);
        b.Draw(Game1.menuTexture, new Rectangle(x + Game1.pixelZoom * 3, y1 + (int) Game1.dialogueFont.MeasureString(boldTitleText).Y + Game1.tileSize / 2 + (hoveredItem == null || text2.Length <= 0 ? 0 : (int) font.MeasureString("asd").Y) - Game1.pixelZoom, num2 - Game1.pixelZoom * (craftingIngredients == null ? 6 : 1), Game1.pixelZoom), new Rectangle?(new Rectangle(44, 300, 4, 4)), Color.White);
        b.DrawString(Game1.dialogueFont, boldTitleText, new Vector2((float) (x + Game1.tileSize / 4), (float) (y1 + Game1.tileSize / 4 + 4)) + new Vector2(2f, 2f), Game1.textShadowColor);
        b.DrawString(Game1.dialogueFont, boldTitleText, new Vector2((float) (x + Game1.tileSize / 4), (float) (y1 + Game1.tileSize / 4 + 4)) + new Vector2(0.0f, 2f), Game1.textShadowColor);
        b.DrawString(Game1.dialogueFont, boldTitleText, new Vector2((float) (x + Game1.tileSize / 4), (float) (y1 + Game1.tileSize / 4 + 4)), Game1.textColor);
        y1 += (int) Game1.dialogueFont.MeasureString(boldTitleText).Y;
      }
      int y2;
      if (hoveredItem != null && text2.Length > 0)
      {
        int num3 = y1 - 4;
        Utility.drawTextWithShadow(b, text2, font, new Vector2((float) (x + Game1.tileSize / 4), (float) (num3 + Game1.tileSize / 4 + 4)), hoveredItem.getCategoryColor(), 1f, -1f, 2, 2, 1f, 3);
        y2 = num3 + ((int) font.MeasureString("T").Y + (boldTitleText != null ? Game1.tileSize / 4 : 0) + Game1.pixelZoom);
      }
      else
        y2 = y1 + (boldTitleText != null ? Game1.tileSize / 4 : 0);
      if (hoveredItem != null && hoveredItem is Boots)
      {
        Boots boots = hoveredItem as Boots;
        Utility.drawTextWithShadow(b, Game1.parseText(boots.description, Game1.smallFont, Game1.tileSize * 4 + Game1.tileSize / 4), font, new Vector2((float) (x + Game1.tileSize / 4), (float) (y2 + Game1.tileSize / 4 + 4)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
        y2 += (int) font.MeasureString(Game1.parseText(boots.description, Game1.smallFont, Game1.tileSize * 4 + Game1.tileSize / 4)).Y;
        if (boots.defenseBonus > 0)
        {
          Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float) (x + Game1.tileSize / 4 + Game1.pixelZoom), (float) (y2 + Game1.tileSize / 4 + 4)), new Rectangle(110, 428, 10, 10), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
          Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:ItemHover_DefenseBonus", (object) boots.defenseBonus), font, new Vector2((float) (x + Game1.tileSize / 4 + Game1.pixelZoom * 13), (float) (y2 + Game1.tileSize / 4 + Game1.pixelZoom * 3)), Game1.textColor * 0.9f * alpha, 1f, -1f, -1, -1, 1f, 3);
          y2 += (int) Math.Max(font.MeasureString("TT").Y, (float) (12 * Game1.pixelZoom));
        }
        if (boots.immunityBonus > 0)
        {
          Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float) (x + Game1.tileSize / 4 + Game1.pixelZoom), (float) (y2 + Game1.tileSize / 4 + 4)), new Rectangle(150, 428, 10, 10), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
          Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:ItemHover_ImmunityBonus", (object) boots.immunityBonus), font, new Vector2((float) (x + Game1.tileSize / 4 + Game1.pixelZoom * 13), (float) (y2 + Game1.tileSize / 4 + Game1.pixelZoom * 3)), Game1.textColor * 0.9f * alpha, 1f, -1f, -1, -1, 1f, 3);
          y2 += (int) Math.Max(font.MeasureString("TT").Y, (float) (12 * Game1.pixelZoom));
        }
      }
      else if (hoveredItem != null && hoveredItem is MeleeWeapon)
      {
        MeleeWeapon meleeWeapon = hoveredItem as MeleeWeapon;
        Utility.drawTextWithShadow(b, Game1.parseText(meleeWeapon.description, Game1.smallFont, Game1.tileSize * 4 + Game1.tileSize / 4), font, new Vector2((float) (x + Game1.tileSize / 4), (float) (y2 + Game1.tileSize / 4 + 4)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
        y2 += (int) font.MeasureString(Game1.parseText(meleeWeapon.description, Game1.smallFont, Game1.tileSize * 4 + Game1.tileSize / 4)).Y;
        if (meleeWeapon.indexOfMenuItemView != 47)
        {
          Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float) (x + Game1.tileSize / 4 + Game1.pixelZoom), (float) (y2 + Game1.tileSize / 4 + 4)), new Rectangle(120, 428, 10, 10), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
          Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:ItemHover_Damage", (object) meleeWeapon.minDamage, (object) meleeWeapon.maxDamage), font, new Vector2((float) (x + Game1.tileSize / 4 + Game1.pixelZoom * 13), (float) (y2 + Game1.tileSize / 4 + Game1.pixelZoom * 3)), Game1.textColor * 0.9f * alpha, 1f, -1f, -1, -1, 1f, 3);
          y2 += (int) Math.Max(font.MeasureString("TT").Y, (float) (12 * Game1.pixelZoom));
          if (meleeWeapon.speed != (meleeWeapon.type == 2 ? -8 : 0))
          {
            Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float) (x + Game1.tileSize / 4 + Game1.pixelZoom), (float) (y2 + Game1.tileSize / 4 + 4)), new Rectangle(130, 428, 10, 10), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
            bool flag = meleeWeapon.type == 2 && meleeWeapon.speed < -8 || meleeWeapon.type != 2 && meleeWeapon.speed < 0;
            Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:ItemHover_Speed", (object) (((meleeWeapon.type == 2 ? meleeWeapon.speed - -8 : meleeWeapon.speed) > 0 ? (object) "+" : (object) "").ToString() + (object) ((meleeWeapon.type == 2 ? meleeWeapon.speed - -8 : meleeWeapon.speed) / 2))), font, new Vector2((float) (x + Game1.tileSize / 4 + Game1.pixelZoom * 13), (float) (y2 + Game1.tileSize / 4 + Game1.pixelZoom * 3)), flag ? Color.DarkRed : Game1.textColor * 0.9f * alpha, 1f, -1f, -1, -1, 1f, 3);
            y2 += (int) Math.Max(font.MeasureString("TT").Y, (float) (12 * Game1.pixelZoom));
          }
          if (meleeWeapon.addedDefense > 0)
          {
            Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float) (x + Game1.tileSize / 4 + Game1.pixelZoom), (float) (y2 + Game1.tileSize / 4 + 4)), new Rectangle(110, 428, 10, 10), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
            Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:ItemHover_DefenseBonus", (object) meleeWeapon.addedDefense), font, new Vector2((float) (x + Game1.tileSize / 4 + Game1.pixelZoom * 13), (float) (y2 + Game1.tileSize / 4 + Game1.pixelZoom * 3)), Game1.textColor * 0.9f * alpha, 1f, -1f, -1, -1, 1f, 3);
            y2 += (int) Math.Max(font.MeasureString("TT").Y, (float) (12 * Game1.pixelZoom));
          }
          if ((double) meleeWeapon.critChance / 0.02 >= 2.0)
          {
            Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float) (x + Game1.tileSize / 4 + Game1.pixelZoom), (float) (y2 + Game1.tileSize / 4 + 4)), new Rectangle(40, 428, 10, 10), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
            Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:ItemHover_CritChanceBonus", (object) (int) ((double) meleeWeapon.critChance / 0.02)), font, new Vector2((float) (x + Game1.tileSize / 4 + Game1.pixelZoom * 13), (float) (y2 + Game1.tileSize / 4 + Game1.pixelZoom * 3)), Game1.textColor * 0.9f * alpha, 1f, -1f, -1, -1, 1f, 3);
            y2 += (int) Math.Max(font.MeasureString("TT").Y, (float) (12 * Game1.pixelZoom));
          }
          if (((double) meleeWeapon.critMultiplier - 3.0) / 0.02 >= 1.0)
          {
            Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float) (x + Game1.tileSize / 4), (float) (y2 + Game1.tileSize / 4 + 4)), new Rectangle(160, 428, 10, 10), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
            Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:ItemHover_CritPowerBonus", (object) (int) (((double) meleeWeapon.critMultiplier - 3.0) / 0.02)), font, new Vector2((float) (x + Game1.tileSize / 4 + Game1.pixelZoom * 11), (float) (y2 + Game1.tileSize / 4 + Game1.pixelZoom * 3)), Game1.textColor * 0.9f * alpha, 1f, -1f, -1, -1, 1f, 3);
            y2 += (int) Math.Max(font.MeasureString("TT").Y, (float) (12 * Game1.pixelZoom));
          }
          if ((double) meleeWeapon.knockback != (double) meleeWeapon.defaultKnockBackForThisType(meleeWeapon.type))
          {
            Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float) (x + Game1.tileSize / 4 + Game1.pixelZoom), (float) (y2 + Game1.tileSize / 4 + 4)), new Rectangle(70, 428, 10, 10), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
            Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:ItemHover_Weight", (object) (((double) (int) Math.Ceiling((double) Math.Abs(meleeWeapon.knockback - meleeWeapon.defaultKnockBackForThisType(meleeWeapon.type)) * 10.0) > (double) meleeWeapon.defaultKnockBackForThisType(meleeWeapon.type) ? (object) "+" : (object) "").ToString() + (object) (int) Math.Ceiling((double) Math.Abs(meleeWeapon.knockback - meleeWeapon.defaultKnockBackForThisType(meleeWeapon.type)) * 10.0))), font, new Vector2((float) (x + Game1.tileSize / 4 + Game1.pixelZoom * 13), (float) (y2 + Game1.tileSize / 4 + Game1.pixelZoom * 3)), Game1.textColor * 0.9f * alpha, 1f, -1f, -1, -1, 1f, 3);
            y2 += (int) Math.Max(font.MeasureString("TT").Y, (float) (12 * Game1.pixelZoom));
          }
        }
      }
      else if (!string.IsNullOrEmpty(text) && text != " ")
      {
        b.DrawString(font, text, new Vector2((float) (x + Game1.tileSize / 4), (float) (y2 + Game1.tileSize / 4 + 4)) + new Vector2(2f, 2f), Game1.textShadowColor * alpha);
        b.DrawString(font, text, new Vector2((float) (x + Game1.tileSize / 4), (float) (y2 + Game1.tileSize / 4 + 4)) + new Vector2(0.0f, 2f), Game1.textShadowColor * alpha);
        b.DrawString(font, text, new Vector2((float) (x + Game1.tileSize / 4), (float) (y2 + Game1.tileSize / 4 + 4)) + new Vector2(2f, 0.0f), Game1.textShadowColor * alpha);
        b.DrawString(font, text, new Vector2((float) (x + Game1.tileSize / 4), (float) (y2 + Game1.tileSize / 4 + 4)), Game1.textColor * 0.9f * alpha);
        y2 += (int) font.MeasureString(text).Y + 4;
      }
      if (craftingIngredients != null)
      {
        craftingIngredients.drawRecipeDescription(b, new Vector2((float) (x + Game1.tileSize / 4), (float) (y2 - Game1.pixelZoom * 2)), num2);
        y2 += craftingIngredients.getDescriptionHeight(num2);
      }
      if (healAmountToDisplay != -1)
      {
        if (healAmountToDisplay > 0)
        {
          Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float) (x + Game1.tileSize / 4 + Game1.pixelZoom), (float) (y2 + Game1.tileSize / 4)), new Rectangle(healAmountToDisplay < 0 ? 140 : 0, 428, 10, 10), Color.White, 0.0f, Vector2.Zero, 3f, false, 0.95f, -1, -1, 0.35f);
          Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:ItemHover_Energy", (object) ((healAmountToDisplay > 0 ? (object) "+" : (object) "").ToString() + (object) healAmountToDisplay)), font, new Vector2((float) (x + Game1.tileSize / 4 + 34 + Game1.pixelZoom), (float) (y2 + Game1.tileSize / 4 + 8)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
          int num3 = y2 + 34;
          Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float) (x + Game1.tileSize / 4 + Game1.pixelZoom), (float) (num3 + Game1.tileSize / 4)), new Rectangle(0, 438, 10, 10), Color.White, 0.0f, Vector2.Zero, 3f, false, 0.95f, -1, -1, 0.35f);
          Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:ItemHover_Health", (object) ((healAmountToDisplay > 0 ? (object) "+" : (object) "").ToString() + (object) (int) ((double) healAmountToDisplay * 0.400000005960464))), font, new Vector2((float) (x + Game1.tileSize / 4 + 34 + Game1.pixelZoom), (float) (num3 + Game1.tileSize / 4 + 8)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
          y2 = num3 + 34;
        }
        else if (healAmountToDisplay != -300)
        {
          Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float) (x + Game1.tileSize / 4 + Game1.pixelZoom), (float) (y2 + Game1.tileSize / 4)), new Rectangle(140, 428, 10, 10), Color.White, 0.0f, Vector2.Zero, 3f, false, 0.95f, -1, -1, 0.35f);
          Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:ItemHover_Energy", (object) string.Concat((object) healAmountToDisplay)), font, new Vector2((float) (x + Game1.tileSize / 4 + 34 + Game1.pixelZoom), (float) (y2 + Game1.tileSize / 4 + 8)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
          y2 += 34;
        }
      }
      if (buffIconsToDisplay != null)
      {
        for (int index = 0; index < buffIconsToDisplay.Length; ++index)
        {
          if (!buffIconsToDisplay[index].Equals("0"))
          {
            Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float) (x + Game1.tileSize / 4 + Game1.pixelZoom), (float) (y2 + Game1.tileSize / 4)), new Rectangle(10 + index * 10, 428, 10, 10), Color.White, 0.0f, Vector2.Zero, 3f, false, 0.95f, -1, -1, 0.35f);
            string text1 = (Convert.ToInt32(buffIconsToDisplay[index]) > 0 ? "+" : "") + buffIconsToDisplay[index] + " ";
            if (index <= 11)
              text1 = Game1.content.LoadString("Strings\\UI:ItemHover_Buff" + (object) index, (object) text1);
            Utility.drawTextWithShadow(b, text1, font, new Vector2((float) (x + Game1.tileSize / 4 + 34 + Game1.pixelZoom), (float) (y2 + Game1.tileSize / 4 + 8)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
            y2 += 34;
          }
        }
      }
      if (hoveredItem != null && hoveredItem.attachmentSlots() > 0)
      {
        y2 += 16;
        hoveredItem.drawAttachments(b, x + Game1.tileSize / 4, y2);
        if (moneyAmountToDisplayAtBottom > -1)
          y2 += Game1.tileSize * hoveredItem.attachmentSlots();
      }
      if (moneyAmountToDisplayAtBottom > -1)
      {
        b.DrawString(font, string.Concat((object) moneyAmountToDisplayAtBottom), new Vector2((float) (x + Game1.tileSize / 4), (float) (y2 + Game1.tileSize / 4 + 4)) + new Vector2(2f, 2f), Game1.textShadowColor);
        b.DrawString(font, string.Concat((object) moneyAmountToDisplayAtBottom), new Vector2((float) (x + Game1.tileSize / 4), (float) (y2 + Game1.tileSize / 4 + 4)) + new Vector2(0.0f, 2f), Game1.textShadowColor);
        b.DrawString(font, string.Concat((object) moneyAmountToDisplayAtBottom), new Vector2((float) (x + Game1.tileSize / 4), (float) (y2 + Game1.tileSize / 4 + 4)) + new Vector2(2f, 0.0f), Game1.textShadowColor);
        b.DrawString(font, string.Concat((object) moneyAmountToDisplayAtBottom), new Vector2((float) (x + Game1.tileSize / 4), (float) (y2 + Game1.tileSize / 4 + 4)), Game1.textColor);
        if (currencySymbol == 0)
          b.Draw(Game1.debrisSpriteSheet, new Vector2((float) ((double) (x + Game1.tileSize / 4) + (double) font.MeasureString(string.Concat((object) moneyAmountToDisplayAtBottom)).X + 20.0), (float) (y2 + Game1.tileSize / 4 + 16)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 8, 16, 16)), Color.White, 0.0f, new Vector2(8f, 8f), (float) Game1.pixelZoom, SpriteEffects.None, 0.95f);
        else if (currencySymbol == 1)
          b.Draw(Game1.mouseCursors, new Vector2((float) ((double) (x + Game1.tileSize / 8) + (double) font.MeasureString(string.Concat((object) moneyAmountToDisplayAtBottom)).X + 20.0), (float) (y2 + Game1.tileSize / 4 - 5)), new Rectangle?(new Rectangle(338, 400, 8, 8)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1f);
        else if (currencySymbol == 2)
          b.Draw(Game1.mouseCursors, new Vector2((float) ((double) (x + Game1.tileSize / 8) + (double) font.MeasureString(string.Concat((object) moneyAmountToDisplayAtBottom)).X + 20.0), (float) (y2 + Game1.tileSize / 4 - 7)), new Rectangle?(new Rectangle(211, 373, 9, 10)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1f);
        y2 += Game1.tileSize * 3 / 4;
      }
      if (extraItemToShowIndex == -1)
        return;
      IClickableMenu.drawTextureBox(b, Game1.menuTexture, new Rectangle(0, 256, 60, 60), x, y2 + Game1.pixelZoom, num2, Game1.tileSize * 3 / 2, Color.White, 1f, true);
      int num7 = y2 + Game1.pixelZoom * 5;
      string str1 = Game1.objectInformation[extraItemToShowIndex].Split('/')[4];
      string text3 = Game1.content.LoadString("Strings\\UI:ItemHover_Requirements", (object) extraItemToShowAmount, (object) str1);
      b.DrawString(font, text3, new Vector2((float) (x + Game1.tileSize / 4), (float) (num7 + Game1.pixelZoom)) + new Vector2(2f, 2f), Game1.textShadowColor);
      b.DrawString(font, text3, new Vector2((float) (x + Game1.tileSize / 4), (float) (num7 + Game1.pixelZoom)) + new Vector2(0.0f, 2f), Game1.textShadowColor);
      b.DrawString(font, text3, new Vector2((float) (x + Game1.tileSize / 4), (float) (num7 + Game1.pixelZoom)) + new Vector2(2f, 0.0f), Game1.textShadowColor);
      b.DrawString(Game1.smallFont, text3, new Vector2((float) (x + Game1.tileSize / 4), (float) (num7 + Game1.pixelZoom)), Game1.textColor);
      b.Draw(Game1.objectSpriteSheet, new Vector2((float) (x + Game1.tileSize / 4 + (int) font.MeasureString(text3).X + Game1.tileSize / 3), (float) num7), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, extraItemToShowIndex, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1f);
    }

    public delegate void onExit();
  }
}
