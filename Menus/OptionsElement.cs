// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.OptionsElement
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;

namespace StardewValley.Menus
{
  public class OptionsElement
  {
    public const int defaultX = 8;
    public const int defaultY = 4;
    public const int defaultPixelWidth = 9;
    public Rectangle bounds;
    public string label;
    public int whichOption;
    public bool greyedOut;

    public OptionsElement(string label)
    {
      this.label = label;
      this.bounds = new Rectangle(8 * Game1.pixelZoom, 4 * Game1.pixelZoom, 9 * Game1.pixelZoom, 9 * Game1.pixelZoom);
      this.whichOption = -1;
    }

    public OptionsElement(string label, int x, int y, int width, int height, int whichOption = -1)
    {
      if (x == -1)
        x = 8 * Game1.pixelZoom;
      if (y == -1)
        y = 4 * Game1.pixelZoom;
      this.bounds = new Rectangle(x, y, width, height);
      this.label = label;
      this.whichOption = whichOption;
    }

    public OptionsElement(string label, Rectangle bounds, int whichOption)
    {
      this.whichOption = whichOption;
      this.label = label;
      this.bounds = bounds;
    }

    public virtual void receiveLeftClick(int x, int y)
    {
    }

    public virtual void leftClickHeld(int x, int y)
    {
    }

    public virtual void leftClickReleased(int x, int y)
    {
    }

    public virtual void receiveKeyPress(Keys key)
    {
    }

    public virtual void draw(SpriteBatch b, int slotX, int slotY)
    {
      if (this.whichOption == -1)
        SpriteText.drawString(b, this.label, slotX + this.bounds.X, slotY + this.bounds.Y + Game1.pixelZoom * 3, 999, -1, 999, 1f, 0.1f, false, -1, "", -1);
      else
        Utility.drawTextWithShadow(b, this.label, Game1.dialogueFont, new Vector2((float) (slotX + this.bounds.X + this.bounds.Width + Game1.pixelZoom * 2), (float) (slotY + this.bounds.Y)), this.greyedOut ? Game1.textColor * 0.33f : Game1.textColor, 1f, 0.1f, -1, -1, 1f, 3);
    }
  }
}
