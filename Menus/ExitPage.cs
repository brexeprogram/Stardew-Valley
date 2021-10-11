// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.ExitPage
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley.Menus
{
  public class ExitPage : IClickableMenu
  {
    public ClickableComponent exitToTitle;
    public ClickableComponent exitToDesktop;

    public ExitPage(int x, int y, int width, int height)
      : base(x, y, width, height, false)
    {
      Vector2 vector2 = new Vector2((float) (this.xPositionOnScreen + width / 2 - (int) (((double) Game1.dialogueFont.MeasureString(Game1.content.LoadString("Strings\\UI:ExitToTitle")).X + (double) Game1.tileSize) / 2.0)), (float) (this.yPositionOnScreen + Game1.tileSize * 4 - Game1.tileSize / 2));
      this.exitToTitle = new ClickableComponent(new Rectangle((int) vector2.X, (int) vector2.Y, (int) Game1.dialogueFont.MeasureString(Game1.content.LoadString("Strings\\UI:ExitToTitle")).X + Game1.tileSize, Game1.tileSize * 3 / 2), "", Game1.content.LoadString("Strings\\UI:ExitToTitle"))
      {
        myID = 535,
        upNeighborID = 12347,
        downNeighborID = 536
      };
      vector2 = new Vector2((float) (this.xPositionOnScreen + width / 2 - (int) (((double) Game1.dialogueFont.MeasureString(Game1.content.LoadString("Strings\\UI:ExitToDesktop")).X + (double) Game1.tileSize) / 2.0)), (float) (this.yPositionOnScreen + Game1.tileSize * 6 + Game1.pixelZoom * 2 - Game1.tileSize / 2));
      this.exitToDesktop = new ClickableComponent(new Rectangle((int) vector2.X, (int) vector2.Y, (int) Game1.dialogueFont.MeasureString(Game1.content.LoadString("Strings\\UI:ExitToDesktop")).X + Game1.tileSize, Game1.tileSize * 3 / 2), "", Game1.content.LoadString("Strings\\UI:ExitToDesktop"))
      {
        myID = 536,
        upNeighborID = 535
      };
      if (!Game1.options.SnappyMenus)
        return;
      this.populateClickableComponentList();
      this.snapToDefaultClickableComponent();
    }

    public override void snapToDefaultClickableComponent()
    {
      this.currentlySnappedComponent = this.getComponentWithID(12347);
      this.snapCursorToCurrentSnappedComponent();
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
      if (Game1.conventionMode)
        return;
      if (this.exitToTitle.containsPoint(x, y) && this.exitToTitle.visible)
      {
        Game1.playSound("bigDeSelect");
        Game1.ExitToTitle();
      }
      if (!this.exitToDesktop.containsPoint(x, y) || !this.exitToDesktop.visible)
        return;
      Game1.playSound("bigDeSelect");
      Game1.quit = true;
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
    }

    public override void performHoverAction(int x, int y)
    {
      if (this.exitToTitle.containsPoint(x, y) && this.exitToTitle.visible)
      {
        if ((double) this.exitToTitle.scale == 0.0)
          Game1.playSound("Cowboy_gunshot");
        this.exitToTitle.scale = 1f;
      }
      else
        this.exitToTitle.scale = 0.0f;
      if (this.exitToDesktop.containsPoint(x, y) && this.exitToDesktop.visible)
      {
        if ((double) this.exitToDesktop.scale == 0.0)
          Game1.playSound("Cowboy_gunshot");
        this.exitToDesktop.scale = 1f;
      }
      else
        this.exitToDesktop.scale = 0.0f;
    }

    public override void draw(SpriteBatch b)
    {
      if (this.exitToTitle.visible)
      {
        IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(432, 439, 9, 9), this.exitToTitle.bounds.X, this.exitToTitle.bounds.Y, this.exitToTitle.bounds.Width, this.exitToTitle.bounds.Height, (double) this.exitToTitle.scale > 0.0 ? Color.Wheat : Color.White, (float) Game1.pixelZoom, true);
        Utility.drawTextWithShadow(b, this.exitToTitle.label, Game1.dialogueFont, new Vector2((float) this.exitToTitle.bounds.Center.X, (float) (this.exitToTitle.bounds.Center.Y + Game1.pixelZoom)) - Game1.dialogueFont.MeasureString(this.exitToTitle.label) / 2f, Game1.textColor, 1f, -1f, -1, -1, 0.0f, 3);
      }
      if (!this.exitToDesktop.visible)
        return;
      IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(432, 439, 9, 9), this.exitToDesktop.bounds.X, this.exitToDesktop.bounds.Y, this.exitToDesktop.bounds.Width, this.exitToDesktop.bounds.Height, (double) this.exitToDesktop.scale > 0.0 ? Color.Wheat : Color.White, (float) Game1.pixelZoom, true);
      Utility.drawTextWithShadow(b, this.exitToDesktop.label, Game1.dialogueFont, new Vector2((float) this.exitToDesktop.bounds.Center.X, (float) (this.exitToDesktop.bounds.Center.Y + Game1.pixelZoom)) - Game1.dialogueFont.MeasureString(this.exitToDesktop.label) / 2f, Game1.textColor, 1f, -1f, -1, -1, 0.0f, 3);
    }
  }
}
