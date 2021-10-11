// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.AddNewItemMenu
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace StardewValley.Menus
{
  public class AddNewItemMenu : IClickableMenu
  {
    private InventoryMenu playerInventory;
    private ClickableComponent garbage;

    public AddNewItemMenu()
      : base(Game1.viewport.Width / 2 - (800 + IClickableMenu.borderWidth * 2) / 2, Game1.viewport.Height / 2 - (300 + IClickableMenu.borderWidth * 2) / 2, 800 + IClickableMenu.borderWidth * 2, 300 + IClickableMenu.borderWidth * 2, false)
    {
      this.playerInventory = new InventoryMenu(this.xPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearSideBorder, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearSideBorder, true, (List<Item>) null, (InventoryMenu.highlightThisItem) null, -1, 3, 0, 0, true);
      this.garbage = new ClickableComponent(new Rectangle(this.xPositionOnScreen + this.width + IClickableMenu.spaceToClearSideBorder, this.yPositionOnScreen + this.height - Game1.tileSize, Game1.tileSize, Game1.tileSize), "Garbage");
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

    public override void draw(SpriteBatch b)
    {
    }
  }
}
