// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.DebugInputMenu
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace StardewValley.Menus
{
  internal class DebugInputMenu : NamingMenu
  {
    public DebugInputMenu(NamingMenu.doneNamingBehavior b)
      : base(b, "Debug Input:", "")
    {
      this.textBox.limitWidth = false;
      this.textBox.Width = Game1.tileSize * 8;
      this.textBox.X -= Game1.tileSize * 2;
      this.randomButton.bounds.X += Game1.tileSize * 2;
      this.doneNamingButton.bounds.X += Game1.tileSize * 2;
      this.minLength = 0;
    }

    public override void update(GameTime time)
    {
      GamePadState state1 = GamePad.GetState(Game1.playerOneIndex);
      KeyboardState state2 = Keyboard.GetState();
      if (Game1.IsPressEvent(ref state1, Buttons.B) || Game1.IsPressEvent(ref state2, Keys.Escape))
      {
        Game1.exitActiveMenu();
        Game1.lastDebugInput = this.textBox.Text;
      }
      base.update(time);
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
      if (this.randomButton.containsPoint(x, y))
      {
        this.textBox.Text = Game1.lastDebugInput;
        this.randomButton.scale = this.randomButton.baseScale;
        Game1.playSound("drumkit6");
      }
      else
        base.receiveLeftClick(x, y, playSound);
    }
  }
}
