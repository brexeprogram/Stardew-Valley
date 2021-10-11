// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.MineElevatorMenu
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StardewValley.Menus
{
  public class MineElevatorMenu : IClickableMenu
  {
    public List<ClickableComponent> elevators = new List<ClickableComponent>();

    public MineElevatorMenu()
      : base(0, 0, 0, 0, true)
    {
      int num1 = Math.Min(Game1.mine.lowestLevelReached, 120) / 5;
      this.width = num1 > 50 ? (Game1.tileSize * 3 / 4 - 4) * 11 + IClickableMenu.borderWidth * 2 : Math.Min((Game1.tileSize * 3 / 4 - 4) * 5 + IClickableMenu.borderWidth * 2, num1 * (Game1.tileSize * 3 / 4 - 4) + IClickableMenu.borderWidth * 2);
      this.height = Math.Max(Game1.tileSize + IClickableMenu.borderWidth * 3, num1 * (Game1.tileSize * 3 / 4 - 4) / (this.width - IClickableMenu.borderWidth) * (Game1.tileSize * 3 / 4 - 4) + Game1.tileSize + IClickableMenu.borderWidth * 3);
      this.xPositionOnScreen = Game1.viewport.Width / 2 - this.width / 2;
      this.yPositionOnScreen = Game1.viewport.Height / 2 - this.height / 2;
      Game1.playSound("crystal");
      int num2 = this.width / (Game1.tileSize - 20) - 1;
      int x1 = this.xPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearSideBorder * 3 / 4;
      int y = this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.borderWidth / 3;
      this.elevators.Add(new ClickableComponent(new Rectangle(x1, y, Game1.tileSize * 3 / 4 - 4, Game1.tileSize * 3 / 4 - 4), string.Concat((object) 0))
      {
        myID = 0,
        rightNeighborID = 1,
        downNeighborID = num2
      });
      int x2 = x1 + Game1.tileSize - 20;
      if (x2 > this.xPositionOnScreen + this.width - IClickableMenu.borderWidth)
      {
        x2 = this.xPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearSideBorder * 3 / 4;
        y += Game1.tileSize - 20;
      }
      for (int index = 1; index <= num1; ++index)
      {
        List<ClickableComponent> elevators = this.elevators;
        ClickableComponent clickableComponent = new ClickableComponent(new Rectangle(x2, y, Game1.tileSize * 3 / 4 - 4, Game1.tileSize * 3 / 4 - 4), string.Concat((object) (index * 5)));
        clickableComponent.myID = index;
        int num3 = index % num2 == num2 - 1 ? -1 : index + 1;
        clickableComponent.rightNeighborID = num3;
        int num4 = index % num2 == 0 ? -1 : index - 1;
        clickableComponent.leftNeighborID = num4;
        int num5 = index + num2;
        clickableComponent.downNeighborID = num5;
        int num6 = index - num2;
        clickableComponent.upNeighborID = num6;
        elevators.Add(clickableComponent);
        x2 = x2 + Game1.tileSize - 20;
        if (x2 > this.xPositionOnScreen + this.width - IClickableMenu.borderWidth)
        {
          x2 = this.xPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearSideBorder * 3 / 4;
          y += Game1.tileSize - 20;
        }
      }
      this.initializeUpperRightCloseButton();
      if (!Game1.options.snappyMenus || !Game1.options.gamepadControls)
        return;
      this.populateClickableComponentList();
      this.snapToDefaultClickableComponent();
    }

    public override void snapToDefaultClickableComponent()
    {
      this.currentlySnappedComponent = this.getComponentWithID(0);
      this.snapCursorToCurrentSnappedComponent();
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
      if (this.isWithinBounds(x, y))
      {
        foreach (ClickableComponent elevator in this.elevators)
        {
          if (elevator.containsPoint(x, y))
          {
            Game1.playSound("smallSelect");
            if (Convert.ToInt32(elevator.name) == 0)
            {
              if (!Game1.currentLocation.Equals((object) Game1.mine))
                return;
              Game1.warpFarmer("Mine", 17, 4, true);
              Game1.exitActiveMenu();
              Game1.changeMusicTrack("none");
            }
            else
            {
              if (Game1.currentLocation.Equals((object) Game1.mine) && Convert.ToInt32(elevator.name) == Game1.mine.mineLevel)
                return;
              Game1.player.ridingMineElevator = true;
              Game1.enterMine(false, Convert.ToInt32(elevator.name), (string) null);
              Game1.exitActiveMenu();
            }
          }
        }
        base.receiveLeftClick(x, y, true);
      }
      else
        Game1.exitActiveMenu();
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
    }

    public override void performHoverAction(int x, int y)
    {
      base.performHoverAction(x, y);
      foreach (ClickableComponent elevator in this.elevators)
        elevator.scale = !elevator.containsPoint(x, y) ? 1f : 2f;
    }

    public override void draw(SpriteBatch b)
    {
      b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.4f);
      Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen - Game1.tileSize + Game1.tileSize / 8, this.width + Game1.tileSize / 3, this.height + Game1.tileSize, false, true, (string) null, false);
      foreach (ClickableComponent elevator in this.elevators)
      {
        b.Draw(Game1.mouseCursors, new Vector2((float) (elevator.bounds.X - Game1.pixelZoom), (float) (elevator.bounds.Y + Game1.pixelZoom)), new Rectangle?(new Rectangle((double) elevator.scale > 1.0 ? 267 : 256, 256, 10, 10)), Color.Black * 0.5f, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.865f);
        b.Draw(Game1.mouseCursors, new Vector2((float) elevator.bounds.X, (float) elevator.bounds.Y), new Rectangle?(new Rectangle((double) elevator.scale > 1.0 ? 267 : 256, 256, 10, 10)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.868f);
        Vector2 position = new Vector2((float) (elevator.bounds.X + 16 + NumberSprite.numberOfDigits(Convert.ToInt32(elevator.name)) * 6), (float) (elevator.bounds.Y + Game1.pixelZoom * 6 - NumberSprite.getHeight() / 4));
        NumberSprite.draw(Convert.ToInt32(elevator.name), b, position, Game1.mine.mineLevel == Convert.ToInt32(elevator.name) && Game1.currentLocation.Equals((object) Game1.mine) || Convert.ToInt32(elevator.name) == 0 && !Game1.currentLocation.Equals((object) Game1.mine) ? Color.Gray * 0.75f : Color.Gold, 0.5f, 0.86f, 1f, 0, 0);
      }
      this.drawMouse(b);
      base.draw(b);
    }
  }
}
