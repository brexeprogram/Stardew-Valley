// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.OptionsCheckbox
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Menus
{
  public class OptionsCheckbox : OptionsElement
  {
    public static Rectangle sourceRectUnchecked = new Rectangle(227, 425, 9, 9);
    public static Rectangle sourceRectChecked = new Rectangle(236, 425, 9, 9);
    public const int pixelsWide = 9;
    public bool isChecked;

    public OptionsCheckbox(string label, int whichOption, int x = -1, int y = -1)
      : base(label, x, y, 9 * Game1.pixelZoom, 9 * Game1.pixelZoom, whichOption)
    {
      Game1.options.setCheckBoxToProperValue(this);
    }

    public override void receiveLeftClick(int x, int y)
    {
      if (this.greyedOut)
        return;
      Game1.playSound("drumkit6");
      base.receiveLeftClick(x, y);
      this.isChecked = !this.isChecked;
      Game1.options.changeCheckBoxOption(this.whichOption, this.isChecked);
    }

    public override void draw(SpriteBatch b, int slotX, int slotY)
    {
      b.Draw(Game1.mouseCursors, new Vector2((float) (slotX + this.bounds.X), (float) (slotY + this.bounds.Y)), new Rectangle?(this.isChecked ? OptionsCheckbox.sourceRectChecked : OptionsCheckbox.sourceRectUnchecked), Color.White * (this.greyedOut ? 0.33f : 1f), 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.4f);
      base.draw(b, slotX, slotY);
    }
  }
}
