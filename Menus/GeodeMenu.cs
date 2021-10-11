// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.GeodeMenu
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StardewValley.Menus
{
  public class GeodeMenu : MenuWithInventory
  {
    public const int region_geodeSpot = 998;
    public ClickableComponent geodeSpot;
    public AnimatedSprite clint;
    public TemporaryAnimatedSprite geodeDestructionAnimation;
    public TemporaryAnimatedSprite sparkle;
    public int geodeAnimationTimer;
    public int yPositionOfGem;
    public int alertTimer;
    public StardewValley.Object geodeTreasure;

    public GeodeMenu()
      : base((InventoryMenu.highlightThisItem) null, true, true, Game1.tileSize / 5, Game1.tileSize * 2 + Game1.pixelZoom)
    {
      this.inventory.highlightMethod = new InventoryMenu.highlightThisItem(this.highlightGeodes);
      this.geodeSpot = new ClickableComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth / 2 + 4, this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + 8, 560, 308), "")
      {
        myID = 998,
        downNeighborID = 0
      };
      this.clint = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Clint"), 8, 32, 48);
      if (this.inventory.inventory != null && this.inventory.inventory.Count >= 12)
      {
        for (int index = 0; index < 12; ++index)
        {
          if (this.inventory.inventory[index] != null)
            this.inventory.inventory[index].upNeighborID = 998;
        }
      }
      if (this.trashCan != null)
        this.trashCan.myID = 106;
      if (this.okButton != null)
        this.okButton.leftNeighborID = 11;
      if (!Game1.options.SnappyMenus)
        return;
      this.populateClickableComponentList();
      this.snapToDefaultClickableComponent();
    }

    public override void snapToDefaultClickableComponent()
    {
      this.currentlySnappedComponent = this.getComponentWithID(0);
      this.snapCursorToCurrentSnappedComponent();
    }

    public override bool readyToClose()
    {
      if (base.readyToClose() && this.geodeAnimationTimer <= 0)
        return this.heldItem == null;
      return false;
    }

    public bool highlightGeodes(Item i)
    {
      if (this.heldItem != null)
        return true;
      switch (i.parentSheetIndex)
      {
        case 535:
        case 536:
        case 537:
        case 749:
          return true;
        default:
          return false;
      }
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
      base.receiveLeftClick(x, y, true);
      if (!this.geodeSpot.containsPoint(x, y))
        return;
      if (this.heldItem != null && this.heldItem.Name.Contains("Geode") && (Game1.player.money >= 25 && this.geodeAnimationTimer <= 0))
      {
        if (Game1.player.freeSpotsInInventory() > 1 || Game1.player.freeSpotsInInventory() == 1 && this.heldItem.Stack == 1)
        {
          this.geodeSpot.item = this.heldItem.getOne();
          --this.heldItem.Stack;
          if (this.heldItem.Stack <= 0)
            this.heldItem = (Item) null;
          this.geodeAnimationTimer = 2700;
          Game1.player.money -= 25;
          Game1.playSound("stoneStep");
          this.clint.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
          {
            new FarmerSprite.AnimationFrame(8, 300),
            new FarmerSprite.AnimationFrame(9, 200),
            new FarmerSprite.AnimationFrame(10, 80),
            new FarmerSprite.AnimationFrame(11, 200),
            new FarmerSprite.AnimationFrame(12, 100),
            new FarmerSprite.AnimationFrame(8, 300)
          });
          this.clint.loop = false;
        }
        else
        {
          this.descriptionText = Game1.content.LoadString("Strings\\UI:GeodeMenu_InventoryFull");
          this.wiggleWordsTimer = 500;
          this.alertTimer = 1500;
        }
      }
      else
      {
        if (Game1.player.money >= 25)
          return;
        this.wiggleWordsTimer = 500;
        Game1.dayTimeMoneyBox.moneyShakeTimer = 1000;
      }
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
      base.receiveRightClick(x, y, true);
    }

    public override void performHoverAction(int x, int y)
    {
      if (this.alertTimer > 0)
        return;
      base.performHoverAction(x, y);
      if (!this.descriptionText.Equals(""))
        return;
      if (Game1.player.money < 25)
        this.descriptionText = Game1.content.LoadString("Strings\\UI:GeodeMenu_Description_NotEnoughMoney");
      else
        this.descriptionText = Game1.content.LoadString("Strings\\UI:GeodeMenu_Description");
    }

    public override void emergencyShutDown()
    {
      base.emergencyShutDown();
      if (this.heldItem == null)
        return;
      Game1.player.addItemToInventoryBool(this.heldItem, false);
    }

    public override void update(GameTime time)
    {
      base.update(time);
      TimeSpan elapsedGameTime;
      if (this.alertTimer > 0)
      {
        int alertTimer = this.alertTimer;
        elapsedGameTime = time.ElapsedGameTime;
        int milliseconds = elapsedGameTime.Milliseconds;
        this.alertTimer = alertTimer - milliseconds;
      }
      if (this.geodeAnimationTimer <= 0)
        return;
      Game1.changeMusicTrack("none");
      int geodeAnimationTimer = this.geodeAnimationTimer;
      elapsedGameTime = time.ElapsedGameTime;
      int milliseconds1 = elapsedGameTime.Milliseconds;
      this.geodeAnimationTimer = geodeAnimationTimer - milliseconds1;
      if (this.geodeAnimationTimer <= 0)
      {
        this.geodeDestructionAnimation = (TemporaryAnimatedSprite) null;
        this.geodeSpot.item = (Item) null;
        Game1.player.addItemToInventoryBool((Item) this.geodeTreasure, false);
        this.geodeTreasure = (StardewValley.Object) null;
        this.yPositionOfGem = 0;
      }
      else
      {
        int currentFrame = this.clint.CurrentFrame;
        this.clint.animateOnce(time);
        if (this.clint.CurrentFrame == 11 && currentFrame != 11)
        {
          ++Game1.stats.GeodesCracked;
          Game1.playSound("hammer");
          Game1.playSound("stoneCrack");
          int y = 448;
          if (this.geodeSpot.item != null)
          {
            switch ((this.geodeSpot.item as StardewValley.Object).parentSheetIndex)
            {
              case 536:
                y += Game1.tileSize;
                break;
              case 537:
                y += Game1.tileSize * 2;
                break;
            }
            this.geodeDestructionAnimation = new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, y, Game1.tileSize, Game1.tileSize), 100f, 8, 0, new Vector2((float) (this.geodeSpot.bounds.X + 392 - Game1.tileSize / 2), (float) (this.geodeSpot.bounds.Y + 192 - Game1.tileSize / 2)), false, false);
            this.geodeTreasure = Utility.getTreasureFromGeode(this.geodeSpot.item);
            if (this.geodeTreasure.Type.Contains("Mineral"))
              Game1.player.foundMineral(this.geodeTreasure.parentSheetIndex);
            else if (this.geodeTreasure.Type.Contains("Arch") && !Game1.player.hasOrWillReceiveMail("artifactFound"))
              this.geodeTreasure = new StardewValley.Object(390, 5, false, -1, 0);
          }
        }
        if (this.geodeDestructionAnimation != null && this.geodeDestructionAnimation.currentParentTileIndex < 7)
        {
          this.geodeDestructionAnimation.update(time);
          if (this.geodeDestructionAnimation.currentParentTileIndex < 3)
            this.yPositionOfGem = this.yPositionOfGem - 1;
          this.yPositionOfGem = this.yPositionOfGem - 1;
          if (this.geodeDestructionAnimation.currentParentTileIndex == 7 && this.geodeTreasure.price > 75)
          {
            this.sparkle = new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 640, Game1.tileSize, Game1.tileSize), 100f, 8, 0, new Vector2((float) (this.geodeSpot.bounds.X + 392 - Game1.tileSize / 2), (float) (this.geodeSpot.bounds.Y + 192 + this.yPositionOfGem - Game1.tileSize / 2)), false, false);
            Game1.playSound("discoverMineral");
          }
          else if (this.geodeDestructionAnimation.currentParentTileIndex == 7 && this.geodeTreasure.price <= 75)
            Game1.playSound("newArtifact");
        }
        if (this.sparkle == null || !this.sparkle.update(time))
          return;
        this.sparkle = (TemporaryAnimatedSprite) null;
      }
    }

    public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
    {
      base.gameWindowSizeChanged(oldBounds, newBounds);
      this.geodeSpot = new ClickableComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth / 2 + 4, this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + 8, 560, 308), "Anvil");
      int yPosition = this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth + Game1.tileSize * 3 - Game1.tileSize / 4 + Game1.tileSize * 2 + Game1.pixelZoom;
      this.inventory = new InventoryMenu(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + IClickableMenu.borderWidth / 2 + Game1.tileSize / 5, yPosition, false, (List<Item>) null, this.inventory.highlightMethod, -1, 3, 0, 0, true);
    }

    public override void draw(SpriteBatch b)
    {
      b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.4f);
      this.draw(b, true, true);
      Game1.dayTimeMoneyBox.drawMoneyBox(b, -1, -1);
      b.Draw(Game1.mouseCursors, new Vector2((float) this.geodeSpot.bounds.X, (float) this.geodeSpot.bounds.Y), new Rectangle?(new Rectangle(0, 512, 140, 77)), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 0.87f);
      if (this.geodeSpot.item != null)
      {
        if (this.geodeDestructionAnimation == null)
          this.geodeSpot.item.drawInMenu(b, new Vector2((float) (this.geodeSpot.bounds.X + 90 * Game1.pixelZoom), (float) (this.geodeSpot.bounds.Y + 40 * Game1.pixelZoom)), 1f);
        else
          this.geodeDestructionAnimation.draw(b, true, 0, 0);
        if (this.geodeTreasure != null)
          this.geodeTreasure.drawInMenu(b, new Vector2((float) (this.geodeSpot.bounds.X + 90 * Game1.pixelZoom), (float) (this.geodeSpot.bounds.Y + 40 * Game1.pixelZoom + this.yPositionOfGem)), 1f);
        if (this.sparkle != null)
          this.sparkle.draw(b, true, 0, 0);
      }
      this.clint.draw(b, new Vector2((float) (this.geodeSpot.bounds.X + 96 * Game1.pixelZoom), (float) (this.geodeSpot.bounds.Y + 16 * Game1.pixelZoom)), 0.877f);
      if (!this.hoverText.Equals(""))
        IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, -1, (string) null, -1, (string[]) null, (Item) null, 0, -1, -1, -1, -1, 1f, (CraftingRecipe) null);
      if (this.heldItem != null)
        this.heldItem.drawInMenu(b, new Vector2((float) (Game1.getOldMouseX() + 8), (float) (Game1.getOldMouseY() + 8)), 1f);
      if (Game1.options.hardwareCursor)
        return;
      this.drawMouse(b);
    }
  }
}
