// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.OptionsSlider
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace StardewValley.Menus
{
  public class OptionsSlider : OptionsElement
  {
    public static Rectangle sliderBGSource = new Rectangle(403, 383, 6, 6);
    public static Rectangle sliderButtonRect = new Rectangle(420, 441, 10, 6);
    public const int pixelsWide = 48;
    public const int pixelsHigh = 6;
    public const int sliderButtonWidth = 10;
    public const int sliderMaxValue = 100;
    public int value;

    public OptionsSlider(string label, int whichOption, int x = -1, int y = -1)
      : base(label, x, y, 48 * Game1.pixelZoom, 6 * Game1.pixelZoom, whichOption)
    {
      Game1.options.setSliderToProperValue(this);
    }

    public override void leftClickHeld(int x, int y)
    {
      if (this.greyedOut)
        return;
      base.leftClickHeld(x, y);
      this.value = x >= this.bounds.X ? (x <= this.bounds.Right - 10 * Game1.pixelZoom ? (int) ((double) ((float) (x - this.bounds.X) / (float) (this.bounds.Width - 10 * Game1.pixelZoom)) * 100.0) : 100) : 0;
      Game1.options.changeSliderOption(this.whichOption, this.value);
    }

    public override void receiveLeftClick(int x, int y)
    {
      if (this.greyedOut)
        return;
      base.receiveLeftClick(x, y);
      this.leftClickHeld(x, y);
    }

    public override void receiveKeyPress(Keys key)
    {
      base.receiveKeyPress(key);
      if (!Game1.options.snappyMenus || !Game1.options.gamepadControls)
        return;
      if (Game1.options.doesInputListContain(Game1.options.moveRightButton, key))
      {
        this.value = Math.Min(this.value + 10, 100);
        Game1.options.changeSliderOption(this.whichOption, this.value);
      }
      else
      {
        if (!Game1.options.doesInputListContain(Game1.options.moveLeftButton, key))
          return;
        this.value = Math.Max(this.value - 10, 0);
        Game1.options.changeSliderOption(this.whichOption, this.value);
      }
    }

    public override void draw(SpriteBatch b, int slotX, int slotY)
    {
      base.draw(b, slotX, slotY);
      IClickableMenu.drawTextureBox(b, Game1.mouseCursors, OptionsSlider.sliderBGSource, slotX + this.bounds.X, slotY + this.bounds.Y, this.bounds.Width, this.bounds.Height, Color.White, (float) Game1.pixelZoom, false);
      b.Draw(Game1.mouseCursors, new Vector2((float) (slotX + this.bounds.X) + (float) (this.bounds.Width - 10 * Game1.pixelZoom) * ((float) this.value / 100f), (float) (slotY + this.bounds.Y)), new Rectangle?(OptionsSlider.sliderButtonRect), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.9f);
    }
  }
}
