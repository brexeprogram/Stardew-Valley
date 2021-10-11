// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.DiscreteColorPicker
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Objects;
using System;

namespace StardewValley.Menus
{
  public class DiscreteColorPicker : IClickableMenu
  {
    public bool visible = true;
    public const int sizeOfEachSwatch = 7;
    public Item itemToDrawColored;
    public bool showExample;
    public int colorSelection;
    public int totalColors;

    public DiscreteColorPicker(int xPosition, int yPosition, int startingColor = 0, Item itemToDrawColored = null)
    {
      this.totalColors = 21;
      this.xPositionOnScreen = xPosition;
      this.yPositionOnScreen = yPosition;
      this.width = this.totalColors * 9 * Game1.pixelZoom + IClickableMenu.borderWidth;
      this.height = 7 * Game1.pixelZoom + IClickableMenu.borderWidth;
      this.itemToDrawColored = itemToDrawColored;
      this.visible = Game1.player.showChestColorPicker;
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
    }

    public int getSelectionFromColor(Color c)
    {
      for (int selection = 0; selection < this.totalColors; ++selection)
      {
        if (this.getColorFromSelection(selection).Equals(c))
          return selection;
      }
      return -1;
    }

    public Color getCurrentColor()
    {
      return this.getColorFromSelection(this.colorSelection);
    }

    public override void performHoverAction(int x, int y)
    {
    }

    public override void update(GameTime time)
    {
      base.update(time);
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
      if (!this.visible)
        return;
      base.receiveLeftClick(x, y, playSound);
      Rectangle rectangle = new Rectangle(this.xPositionOnScreen + IClickableMenu.borderWidth / 2, this.yPositionOnScreen + IClickableMenu.borderWidth / 2, 9 * Game1.pixelZoom * this.totalColors, 7 * Game1.pixelZoom);
      if (!rectangle.Contains(x, y))
        return;
      this.colorSelection = (x - rectangle.X) / (9 * Game1.pixelZoom);
      try
      {
        Game1.playSound("coin");
      }
      catch (Exception ex)
      {
      }
      if (!(this.itemToDrawColored is Chest))
        return;
      (this.itemToDrawColored as Chest).playerChoiceColor = this.getColorFromSelection(this.colorSelection);
      (this.itemToDrawColored as Chest).currentLidFrame = 131;
    }

    public Color getColorFromSelection(int selection)
    {
      switch (selection)
      {
        case 1:
          return new Color(85, 85, (int) byte.MaxValue);
        case 2:
          return new Color(119, 191, (int) byte.MaxValue);
        case 3:
          return new Color(0, 170, 170);
        case 4:
          return new Color(0, 234, 175);
        case 5:
          return new Color(0, 170, 0);
        case 6:
          return new Color(159, 236, 0);
        case 7:
          return new Color((int) byte.MaxValue, 234, 18);
        case 8:
          return new Color((int) byte.MaxValue, 167, 18);
        case 9:
          return new Color((int) byte.MaxValue, 105, 18);
        case 10:
          return new Color((int) byte.MaxValue, 0, 0);
        case 11:
          return new Color(135, 0, 35);
        case 12:
          return new Color((int) byte.MaxValue, 173, 199);
        case 13:
          return new Color((int) byte.MaxValue, 117, 195);
        case 14:
          return new Color(172, 0, 198);
        case 15:
          return new Color(143, 0, (int) byte.MaxValue);
        case 16:
          return new Color(89, 11, 142);
        case 17:
          return new Color(64, 64, 64);
        case 18:
          return new Color(100, 100, 100);
        case 19:
          return new Color(200, 200, 200);
        case 20:
          return new Color(254, 254, 254);
        default:
          return Color.Black;
      }
    }

    public override void draw(SpriteBatch b)
    {
      if (!this.visible)
        return;
      IClickableMenu.drawTextureBox(b, this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, Color.LightGray);
      for (int selection = 0; selection < this.totalColors; ++selection)
      {
        if (selection == 0)
          b.Draw(Game1.mouseCursors, new Vector2((float) (this.xPositionOnScreen + IClickableMenu.borderWidth / 2), (float) (this.yPositionOnScreen + IClickableMenu.borderWidth / 2)), new Rectangle?(new Rectangle(295, 503, 7, 7)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.88f);
        else
          b.Draw(Game1.staminaRect, new Rectangle(this.xPositionOnScreen + IClickableMenu.borderWidth / 2 + selection * 9 * Game1.pixelZoom, this.yPositionOnScreen + IClickableMenu.borderWidth / 2, 7 * Game1.pixelZoom, 7 * Game1.pixelZoom), this.getColorFromSelection(selection));
        if (selection == this.colorSelection)
          IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(375, 357, 3, 3), this.xPositionOnScreen + IClickableMenu.borderWidth / 2 - Game1.pixelZoom + selection * 9 * Game1.pixelZoom, this.yPositionOnScreen + IClickableMenu.borderWidth / 2 - Game1.pixelZoom, 9 * Game1.pixelZoom, 9 * Game1.pixelZoom, Color.Black, (float) Game1.pixelZoom, false);
      }
      if (this.itemToDrawColored == null || !(this.itemToDrawColored is Chest))
        return;
      (this.itemToDrawColored as Chest).draw(b, this.xPositionOnScreen + this.width + IClickableMenu.borderWidth / 2, this.yPositionOnScreen + Game1.pixelZoom * 4, 1f, true);
    }
  }
}
