// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.MuseumMenu
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Locations;
using xTile.Dimensions;

namespace StardewValley.Menus
{
  public class MuseumMenu : MenuWithInventory
  {
    public const int startingState = 0;
    public const int placingInMuseumState = 1;
    public const int exitingState = 2;
    public int fadeTimer;
    public int state;
    public int menuPositionOffset;
    public bool fadeIntoBlack;
    public bool menuMovingDown;
    public float blackFadeAlpha;
    public SparklingText sparkleText;
    public Vector2 globalLocationOfSparklingArtifact;
    private bool holdingMuseumPiece;

    public MuseumMenu()
      : base(new InventoryMenu.highlightThisItem((Game1.currentLocation as LibraryMuseum).isItemSuitableForDonation), true, false, 0, 0)
    {
      this.fadeTimer = 800;
      this.fadeIntoBlack = true;
      this.movePosition(0, Game1.viewport.Height - this.yPositionOnScreen - this.height);
      Game1.player.forceCanMove();
      if (!Game1.options.SnappyMenus)
        return;
      if (this.okButton != null)
        this.okButton.myID = 106;
      this.populateClickableComponentList();
      this.currentlySnappedComponent = this.getComponentWithID(0);
      this.snapCursorToCurrentSnappedComponent();
    }

    public override void receiveKeyPress(Keys key)
    {
      if (this.fadeTimer > 0)
        return;
      if (Game1.options.doesInputListContain(Game1.options.menuButton, key) && this.readyToClose())
      {
        this.state = 2;
        this.fadeTimer = 500;
        this.fadeIntoBlack = true;
      }
      else if (Game1.options.SnappyMenus)
        base.receiveKeyPress(key);
      if (Game1.options.SnappyMenus)
        return;
      if (Game1.options.doesInputListContain(Game1.options.moveDownButton, key))
        Game1.panScreen(0, 4);
      else if (Game1.options.doesInputListContain(Game1.options.moveRightButton, key))
        Game1.panScreen(4, 0);
      else if (Game1.options.doesInputListContain(Game1.options.moveUpButton, key))
      {
        Game1.panScreen(0, -4);
      }
      else
      {
        if (!Game1.options.doesInputListContain(Game1.options.moveLeftButton, key))
          return;
        Game1.panScreen(-4, 0);
      }
    }

    public override bool overrideSnappyMenuCursorMovementBan()
    {
      return this.heldItem != null;
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
      if (this.fadeTimer > 0)
        return;
      Item heldItem = this.heldItem;
      if (!this.holdingMuseumPiece)
        this.heldItem = this.inventory.leftClick(x, y, this.heldItem, true);
      if (heldItem != null && this.heldItem != null && (y < Game1.viewport.Height - (this.height - (IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + 3 * Game1.tileSize)) || this.menuMovingDown))
      {
        int x1 = (x + Game1.viewport.X) / Game1.tileSize;
        int y1 = (y + Game1.viewport.Y) / Game1.tileSize;
        if ((Game1.currentLocation as LibraryMuseum).isTileSuitableForMuseumPiece(x1, y1) && (Game1.currentLocation as LibraryMuseum).isItemSuitableForDonation(this.heldItem))
        {
          int count1 = (Game1.currentLocation as LibraryMuseum).getRewardsForPlayer(Game1.player).Count;
          (Game1.currentLocation as LibraryMuseum).museumPieces.Add(new Vector2((float) x1, (float) y1), (this.heldItem as Object).parentSheetIndex);
          Game1.playSound("stoneStep");
          this.holdingMuseumPiece = false;
          if ((Game1.currentLocation as LibraryMuseum).getRewardsForPlayer(Game1.player).Count > count1)
          {
            this.sparkleText = new SparklingText(Game1.dialogueFont, Game1.content.LoadString("Strings\\StringsFromCSFiles:NewReward"), Color.MediumSpringGreen, Color.White, false, 0.1, 2500, -1, 500);
            Game1.playSound("reward");
            this.globalLocationOfSparklingArtifact = new Vector2((float) (x1 * Game1.tileSize + Game1.tileSize / 2) - this.sparkleText.textWidth / 2f, (float) (y1 * Game1.tileSize - Game1.tileSize * 3 / 4));
          }
          else
            Game1.playSound("newArtifact");
          Game1.player.completeQuest(24);
          --this.heldItem.Stack;
          if (this.heldItem.Stack <= 0)
            this.heldItem = (Item) null;
          this.menuMovingDown = false;
          int count2 = (Game1.currentLocation as LibraryMuseum).museumPieces.Count;
          if (count2 >= 95)
            Game1.getAchievement(5);
          else if (count2 >= 40)
            Game1.getAchievement(28);
        }
      }
      else if (this.heldItem == null && !this.inventory.isWithinBounds(x, y))
      {
        Vector2 key = new Vector2((float) ((x + Game1.viewport.X) / Game1.tileSize), (float) ((y + Game1.viewport.Y) / Game1.tileSize));
        if ((Game1.currentLocation as LibraryMuseum).museumPieces.ContainsKey(key))
        {
          this.heldItem = (Item) new Object((Game1.currentLocation as LibraryMuseum).museumPieces[key], 1, false, -1, 0);
          (Game1.currentLocation as LibraryMuseum).museumPieces.Remove(key);
          this.holdingMuseumPiece = true;
        }
      }
      if (this.heldItem != null && heldItem == null)
        this.menuMovingDown = true;
      if (this.okButton == null || !this.okButton.containsPoint(x, y) || !this.readyToClose())
        return;
      this.state = 2;
      this.fadeTimer = 800;
      this.fadeIntoBlack = true;
      Game1.playSound("bigDeSelect");
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
      Item heldItem = this.heldItem;
      if (this.fadeTimer <= 0)
        base.receiveRightClick(x, y, true);
      if (this.heldItem == null || heldItem != null)
        return;
      this.menuMovingDown = true;
    }

    public override void update(GameTime time)
    {
      base.update(time);
      if (this.sparkleText != null && this.sparkleText.update(time))
        this.sparkleText = (SparklingText) null;
      if (this.fadeTimer > 0)
      {
        this.fadeTimer = this.fadeTimer - time.ElapsedGameTime.Milliseconds;
        this.blackFadeAlpha = !this.fadeIntoBlack ? (float) (1.0 - (1500.0 - (double) this.fadeTimer) / 1500.0) : (float) (0.0 + (1500.0 - (double) this.fadeTimer) / 1500.0);
        if (this.fadeTimer <= 0)
        {
          switch (this.state)
          {
            case 0:
              this.state = 1;
              Game1.viewportFreeze = true;
              Game1.viewport.Location = new Location(18 * Game1.tileSize, 2 * Game1.tileSize);
              Game1.clampViewportToGameMap();
              this.fadeTimer = 800;
              this.fadeIntoBlack = false;
              break;
            case 2:
              Game1.viewportFreeze = false;
              this.fadeIntoBlack = false;
              this.fadeTimer = 800;
              this.state = 3;
              break;
            case 3:
              Game1.exitActiveMenu();
              break;
          }
        }
      }
      if (this.menuMovingDown && this.menuPositionOffset < this.height / 3)
      {
        this.menuPositionOffset = this.menuPositionOffset + 8;
        this.movePosition(0, 8);
      }
      else if (!this.menuMovingDown && this.menuPositionOffset > 0)
      {
        this.menuPositionOffset = this.menuPositionOffset - 8;
        this.movePosition(0, -8);
      }
      int num1 = Game1.getOldMouseX() + Game1.viewport.X;
      int num2 = Game1.getOldMouseY() + Game1.viewport.Y;
      if (num1 - Game1.viewport.X < Game1.tileSize)
        Game1.panScreen(-4, 0);
      else if (num1 - (Game1.viewport.X + Game1.viewport.Width) >= -Game1.tileSize)
        Game1.panScreen(4, 0);
      if (num2 - Game1.viewport.Y < Game1.tileSize)
        Game1.panScreen(0, -4);
      else if (num2 - (Game1.viewport.Y + Game1.viewport.Height) >= -Game1.tileSize)
      {
        Game1.panScreen(0, 4);
        if (this.menuMovingDown)
          this.menuMovingDown = false;
      }
      foreach (Keys pressedKey in Game1.oldKBState.GetPressedKeys())
        this.receiveKeyPress(pressedKey);
    }

    public override void gameWindowSizeChanged(Microsoft.Xna.Framework.Rectangle oldBounds, Microsoft.Xna.Framework.Rectangle newBounds)
    {
      base.gameWindowSizeChanged(oldBounds, newBounds);
      this.movePosition(0, Game1.viewport.Height - this.yPositionOnScreen - this.height);
      Game1.player.forceCanMove();
    }

    public override void draw(SpriteBatch b)
    {
      if ((this.fadeTimer <= 0 || !this.fadeIntoBlack) && this.state != 3)
      {
        if (this.heldItem != null)
        {
          for (int y = Game1.viewport.Y / Game1.tileSize - 1; y < (Game1.viewport.Y + Game1.viewport.Height) / Game1.tileSize + 2; ++y)
          {
            for (int x = Game1.viewport.X / Game1.tileSize - 1; x < (Game1.viewport.X + Game1.viewport.Width) / Game1.tileSize + 1; ++x)
            {
              if ((Game1.currentLocation as LibraryMuseum).isTileSuitableForMuseumPiece(x, y))
                b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) x, (float) y) * (float) Game1.tileSize), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 29, -1, -1)), Color.LightGreen);
            }
          }
        }
        if (!this.holdingMuseumPiece)
          this.draw(b, false, false);
        if (!this.hoverText.Equals(""))
          IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, -1, (string) null, -1, (string[]) null, (Item) null, 0, -1, -1, -1, -1, 1f, (CraftingRecipe) null);
        if (this.heldItem != null)
          this.heldItem.drawInMenu(b, new Vector2((float) (Game1.getOldMouseX() + 8), (float) (Game1.getOldMouseY() + 8)), 1f);
        this.drawMouse(b);
        if (this.sparkleText != null)
          this.sparkleText.draw(b, Game1.GlobalToLocal(Game1.viewport, this.globalLocationOfSparklingArtifact));
      }
      b.Draw(Game1.fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), Color.Black * this.blackFadeAlpha);
    }
  }
}
