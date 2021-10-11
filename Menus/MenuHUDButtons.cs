// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.MenuHUDButtons
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
  public class MenuHUDButtons : IClickableMenu
  {
    private List<ClickableComponent> buttons = new List<ClickableComponent>();
    private string hoverText = "";
    public new const int width = 70;
    public new const int height = 21;
    private Vector2 position;
    private Rectangle sourceRect;

    public MenuHUDButtons()
      : base(Game1.viewport.Width / 2 + Game1.tileSize * 12 / 2 + Game1.tileSize, Game1.viewport.Height - 21 * Game1.pixelZoom - Game1.pixelZoom * 4, 70 * Game1.pixelZoom, 21 * Game1.pixelZoom, false)
    {
      for (int index = 0; index < 7; ++index)
        this.buttons.Add(new ClickableComponent(new Rectangle(Game1.viewport.Width / 2 + Game1.tileSize * 12 / 2 + Game1.pixelZoom * 4 + index * 9 * Game1.pixelZoom, this.yPositionOnScreen + 5 * Game1.pixelZoom, 9 * Game1.pixelZoom, 11 * Game1.pixelZoom), string.Concat((object) index)));
      this.position = new Vector2((float) this.xPositionOnScreen, (float) this.yPositionOnScreen);
      this.sourceRect = new Rectangle(221, 362, 70, 21);
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
      if (Game1.player.usingTool)
        return;
      foreach (ClickableComponent button in this.buttons)
      {
        if (button.containsPoint(x, y))
        {
          Game1.activeClickableMenu = (IClickableMenu) new GameMenu(Convert.ToInt32(button.name), -1);
          Game1.playSound("bigSelect");
          break;
        }
      }
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
    }

    public override void performHoverAction(int x, int y)
    {
      this.hoverText = "";
      foreach (ClickableComponent button in this.buttons)
      {
        if (button.containsPoint(x, y))
          this.hoverText = GameMenu.getLabelOfTabFromIndex(Convert.ToInt32(button.name));
      }
    }

    public override void update(GameTime time)
    {
    }

    public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
    {
      this.xPositionOnScreen = Game1.viewport.Width / 2 + Game1.tileSize * 12 / 2 + Game1.tileSize;
      this.yPositionOnScreen = Game1.viewport.Height - 21 * Game1.pixelZoom - Game1.pixelZoom * 4;
      for (int index = 0; index < 7; ++index)
        this.buttons[index].bounds = new Rectangle(Game1.viewport.Width / 2 + Game1.tileSize * 12 / 2 + Game1.pixelZoom * 4 + index * 9 * Game1.pixelZoom, this.yPositionOnScreen + 5 * Game1.pixelZoom, 9 * Game1.pixelZoom, 11 * Game1.pixelZoom);
      this.position = new Vector2((float) this.xPositionOnScreen, (float) this.yPositionOnScreen);
    }

    public override bool isWithinBounds(int x, int y)
    {
      return new Rectangle(this.buttons.First<ClickableComponent>().bounds.X, this.buttons.First<ClickableComponent>().bounds.Y, this.buttons.Last<ClickableComponent>().bounds.X - this.buttons.First<ClickableComponent>().bounds.X + Game1.tileSize, Game1.tileSize).Contains(x, y);
    }

    public override void draw(SpriteBatch b)
    {
      if (Game1.activeClickableMenu != null)
        return;
      b.Draw(Game1.mouseCursors, this.position, new Rectangle?(this.sourceRect), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1f);
      if (this.hoverText.Equals("") || !this.isWithinBounds(Game1.getOldMouseX(), Game1.getOldMouseY()))
        return;
      IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, -1, (string) null, -1, (string[]) null, (Item) null, 0, -1, -1, -1, -1, 1f, (CraftingRecipe) null);
    }
  }
}
