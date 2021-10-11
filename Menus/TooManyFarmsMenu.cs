// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.TooManyFarmsMenu
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;

namespace StardewValley.Menus
{
  public class TooManyFarmsMenu : IClickableMenu
  {
    public const int cWidth = 800;
    public const int cHeight = 180;

    public TooManyFarmsMenu()
    {
      Vector2 centeringOnScreen = Utility.getTopLeftPositionForCenteringOnScreen(800, 180, 0, 0);
      this.initialize((int) centeringOnScreen.X, (int) centeringOnScreen.Y, 800, 180, false);
    }

    public override bool readyToClose()
    {
      return true;
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
      base.receiveLeftClick(x, y, playSound);
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
    }

    public void drawBox(SpriteBatch b, int xPos, int yPos, int boxWidth, int boxHeight)
    {
      b.Draw(Game1.mouseCursors, new Rectangle(xPos, yPos, boxWidth, boxHeight), new Rectangle?(new Rectangle(306, 320, 16, 16)), Color.White);
      b.Draw(Game1.mouseCursors, new Rectangle(xPos, yPos - 5 * Game1.pixelZoom, boxWidth, 6 * Game1.pixelZoom), new Rectangle?(new Rectangle(275, 313, 1, 6)), Color.White);
      b.Draw(Game1.mouseCursors, new Rectangle(xPos + 3 * Game1.pixelZoom, yPos + boxHeight, boxWidth - 5 * Game1.pixelZoom, 8 * Game1.pixelZoom), new Rectangle?(new Rectangle(275, 328, 1, 8)), Color.White);
      b.Draw(Game1.mouseCursors, new Rectangle(xPos - 8 * Game1.pixelZoom, yPos + 6 * Game1.pixelZoom, 8 * Game1.pixelZoom, boxHeight - 7 * Game1.pixelZoom), new Rectangle?(new Rectangle(264, 325, 8, 1)), Color.White);
      b.Draw(Game1.mouseCursors, new Rectangle(xPos + boxWidth, yPos, 7 * Game1.pixelZoom, boxHeight), new Rectangle?(new Rectangle(293, 324, 7, 1)), Color.White);
      b.Draw(Game1.mouseCursors, new Vector2((float) (xPos - 11 * Game1.pixelZoom), (float) (yPos - 7 * Game1.pixelZoom)), new Rectangle?(new Rectangle(261, 311, 14, 13)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.87f);
      b.Draw(Game1.mouseCursors, new Vector2((float) (xPos + boxWidth - Game1.pixelZoom * 2), (float) (yPos - 7 * Game1.pixelZoom)), new Rectangle?(new Rectangle(291, 311, 12, 11)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.87f);
      b.Draw(Game1.mouseCursors, new Vector2((float) (xPos + boxWidth - Game1.pixelZoom * 2), (float) (yPos + boxHeight - 2 * Game1.pixelZoom)), new Rectangle?(new Rectangle(291, 326, 12, 12)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.87f);
      b.Draw(Game1.mouseCursors, new Vector2((float) (xPos - 11 * Game1.pixelZoom), (float) (yPos + boxHeight - Game1.pixelZoom)), new Rectangle?(new Rectangle(261, 327, 14, 11)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.87f);
    }

    public override void update(GameTime time)
    {
      base.update(time);
    }

    public override void draw(SpriteBatch b)
    {
      b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.75f);
      this.drawBox(b, this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height);
      int num = 35;
      string s = Game1.content.LoadString("Strings\\UI:TooManyFarmsMenu_TooManyFarms");
      SpriteText.drawString(b, s, this.xPositionOnScreen + num, this.yPositionOnScreen + num, 999999, this.width, this.height, 1f, 0.88f, false, -1, "", -1);
      int y = 260;
      if (!Game1.options.gamepadControls)
        return;
      b.Draw(Game1.controllerMaps, new Rectangle(this.xPositionOnScreen + this.width - 14 - 52, this.yPositionOnScreen + this.height - 14 - 52, 52, 52), new Rectangle?(new Rectangle(542, y, 26, 26)), Color.White);
    }
  }
}
