// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.Toolbar
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Menus
{
  public class Toolbar : IClickableMenu
  {
    private List<ClickableComponent> buttons = new List<ClickableComponent>();
    private string hoverTitle = "";
    private float transparency = 1f;
    public Rectangle toolbarTextSource = new Rectangle(0, 256, 60, 60);
    private new int yPositionOnScreen;
    private Item hoverItem;

    public Toolbar()
      : base(Game1.viewport.Width / 2 - Game1.tileSize * 12 / 2 - Game1.tileSize, Game1.viewport.Height, Game1.tileSize * 14, Game1.tileSize * 3 + Game1.tileSize / 4, false)
    {
      for (int index = 0; index < 12; ++index)
        this.buttons.Add(new ClickableComponent(new Rectangle(Game1.viewport.Width / 2 - Game1.tileSize * 12 / 2 + index * Game1.tileSize, this.yPositionOnScreen - Game1.tileSize * 3 / 2 + 8, Game1.tileSize, Game1.tileSize), string.Concat((object) index)));
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
      if (Game1.player.usingTool)
        return;
      foreach (ClickableComponent button in this.buttons)
      {
        if (button.containsPoint(x, y))
        {
          Game1.player.CurrentToolIndex = Convert.ToInt32(button.name);
          if (Game1.player.ActiveObject != null)
          {
            Game1.player.showCarrying();
            Game1.playSound("pickUpItem");
            break;
          }
          Game1.player.showNotCarrying();
          Game1.playSound("stoneStep");
          break;
        }
      }
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
    }

    public override void performHoverAction(int x, int y)
    {
      this.hoverItem = (Item) null;
      foreach (ClickableComponent button in this.buttons)
      {
        if (button.containsPoint(x, y))
        {
          int int32 = Convert.ToInt32(button.name);
          if (int32 < Game1.player.items.Count && Game1.player.items[int32] != null)
          {
            button.scale = Math.Min(button.scale + 0.05f, 1.1f);
            this.hoverTitle = Game1.player.items[int32].DisplayName;
            this.hoverItem = Game1.player.items[int32];
          }
        }
        else
          button.scale = Math.Max(button.scale - 0.025f, 1f);
      }
    }

    public void shifted(bool right)
    {
      if (right)
      {
        for (int index = 0; index < this.buttons.Count; ++index)
          this.buttons[index].scale = (float) (1.0 + (double) index * 0.0299999993294477);
      }
      else
      {
        for (int index = this.buttons.Count - 1; index >= 0; --index)
          this.buttons[index].scale = (float) (1.0 + (double) (11 - index) * 0.0299999993294477);
      }
    }

    public override void update(GameTime time)
    {
    }

    public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
    {
      for (int index = 0; index < 12; ++index)
        this.buttons[index].bounds = new Rectangle(Game1.viewport.Width / 2 - Game1.tileSize * 12 / 2 + index * Game1.tileSize, this.yPositionOnScreen - Game1.tileSize * 3 / 2 + 8, Game1.tileSize, Game1.tileSize);
    }

    public override bool isWithinBounds(int x, int y)
    {
      return new Rectangle(this.buttons.First<ClickableComponent>().bounds.X, this.buttons.First<ClickableComponent>().bounds.Y, this.buttons.Last<ClickableComponent>().bounds.X - this.buttons.First<ClickableComponent>().bounds.X + Game1.tileSize, Game1.tileSize).Contains(x, y);
    }

    public override void draw(SpriteBatch b)
    {
      if (Game1.activeClickableMenu != null)
        return;
      Point center = Game1.player.GetBoundingBox().Center;
      Vector2 globalPosition = new Vector2((float) center.X, (float) center.Y);
      Vector2 local = Game1.GlobalToLocal(Game1.viewport, globalPosition);
      bool flag;
      if (Game1.options.pinToolbarToggle)
      {
        flag = false;
        this.transparency = Math.Min(1f, this.transparency + 0.075f);
        if ((double) local.Y > (double) (Game1.viewport.Height - Game1.tileSize * 3))
          this.transparency = Math.Max(0.33f, this.transparency - 0.15f);
      }
      else
      {
        flag = (double) local.Y > (double) (Game1.viewport.Height / 2 + Game1.tileSize);
        this.transparency = 1f;
      }
      int num = Utility.makeSafeMarginY(8);
      int positionOnScreen1 = this.yPositionOnScreen;
      if (!flag)
      {
        this.yPositionOnScreen = Game1.viewport.Height;
        this.yPositionOnScreen = this.yPositionOnScreen + 8;
        this.yPositionOnScreen = this.yPositionOnScreen - num;
      }
      else
      {
        this.yPositionOnScreen = Game1.tileSize + Game1.tileSize * 3 / 4;
        this.yPositionOnScreen = this.yPositionOnScreen - 8;
        this.yPositionOnScreen = this.yPositionOnScreen + num;
      }
      int positionOnScreen2 = this.yPositionOnScreen;
      if (positionOnScreen1 != positionOnScreen2)
      {
        for (int index = 0; index < 12; ++index)
          this.buttons[index].bounds.Y = this.yPositionOnScreen - Game1.tileSize * 3 / 2 + 8;
      }
      IClickableMenu.drawTextureBox(b, Game1.menuTexture, this.toolbarTextSource, Game1.viewport.Width / 2 - Game1.tileSize * 12 / 2 - Game1.pixelZoom * 4, this.yPositionOnScreen - Game1.tileSize * 3 / 2 - Game1.pixelZoom * 2, Game1.tileSize * 12 + Game1.tileSize / 2, Game1.tileSize + Game1.tileSize / 2, Color.White * this.transparency, 1f, false);
      for (int index = 0; index < 12; ++index)
      {
        Vector2 position = new Vector2((float) (Game1.viewport.Width / 2 - Game1.tileSize * 12 / 2 + index * Game1.tileSize), (float) (this.yPositionOnScreen - Game1.tileSize * 3 / 2 + 8));
        b.Draw(Game1.menuTexture, position, new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, Game1.player.CurrentToolIndex == index ? 56 : 10, -1, -1)), Color.White * this.transparency);
        string text = index == 9 ? "0" : (index == 10 ? "-" : (index == 11 ? "=" : string.Concat((object) (index + 1))));
        b.DrawString(Game1.tinyFont, text, position + new Vector2(4f, -8f), Color.DimGray * this.transparency);
      }
      for (int index = 0; index < 12; ++index)
      {
        this.buttons[index].scale = Math.Max(1f, this.buttons[index].scale - 0.025f);
        Vector2 location = new Vector2((float) (Game1.viewport.Width / 2 - Game1.tileSize * 12 / 2 + index * Game1.tileSize), (float) (this.yPositionOnScreen - Game1.tileSize * 3 / 2 + 8));
        if (Game1.player.items.Count > index && Game1.player.items.ElementAt<Item>(index) != null)
          Game1.player.items[index].drawInMenu(b, location, Game1.player.CurrentToolIndex == index ? 0.9f : this.buttons.ElementAt<ClickableComponent>(index).scale * 0.8f, this.transparency, 0.88f);
      }
      if (this.hoverItem == null)
        return;
      IClickableMenu.drawToolTip(b, this.hoverItem.getDescription(), this.hoverItem.DisplayName, this.hoverItem, false, -1, 0, -1, -1, (CraftingRecipe) null, -1);
      this.hoverItem = (Item) null;
    }
  }
}
