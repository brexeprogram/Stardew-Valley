// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.AboutMenu
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using System;
using System.Diagnostics;

namespace StardewValley.Menus
{
  public class AboutMenu : IClickableMenu
  {
    public const int region_linkToTwitter = 91111;
    public const int region_linkToSVSite = 92222;
    public const int region_linkToChucklefish = 93333;
    public new const int width = 950;
    public new const int height = 700;
    public ClickableComponent linkToTwitter;
    public ClickableComponent linkToSVSite;
    public ClickableComponent linkToChucklefish;
    public ClickableComponent backButton;

    public AboutMenu()
    {
      Vector2 centeringOnScreen = Utility.getTopLeftPositionForCenteringOnScreen(950, 700, 0, 0);
      this.linkToSVSite = new ClickableComponent(new Rectangle((int) centeringOnScreen.X + Game1.tileSize / 2, (int) centeringOnScreen.Y + 700 - 100 - Game1.tileSize * 3 - Game1.pixelZoom * 4, 950 - Game1.tileSize, Game1.tileSize), "", Game1.content.LoadString("Strings\\UI:About_Website"))
      {
        myID = 92222,
        downNeighborID = 91111
      };
      this.linkToTwitter = new ClickableComponent(new Rectangle((int) centeringOnScreen.X + Game1.tileSize / 2, (int) centeringOnScreen.Y + 700 - 100 - Game1.tileSize * 2 - Game1.pixelZoom * 4, 950 - Game1.tileSize, Game1.tileSize), "", Game1.content.LoadString("Strings\\UI:About_ConcernedApe"))
      {
        myID = 91111,
        upNeighborID = 92222,
        downNeighborID = 93333
      };
      this.linkToChucklefish = new ClickableComponent(new Rectangle((int) centeringOnScreen.X + Game1.tileSize / 2, (int) centeringOnScreen.Y + 700 - 100 - Game1.tileSize - Game1.pixelZoom * 4, 950 - Game1.tileSize, Game1.tileSize), "", Game1.content.LoadString("Strings\\UI:About_Chucklefish"))
      {
        myID = 93333,
        upNeighborID = 91111,
        downNeighborID = 81114
      };
      this.backButton = new ClickableComponent(new Rectangle(Game1.viewport.Width - 198 - 48, Game1.viewport.Height - 81 - 24, 198, 81), "")
      {
        myID = 81114,
        upNeighborID = 93333
      };
      if (!Game1.options.snappyMenus || !Game1.options.gamepadControls)
        return;
      this.populateClickableComponentList();
      this.snapToDefaultClickableComponent();
    }

    public override void snapToDefaultClickableComponent()
    {
      this.currentlySnappedComponent = this.getComponentWithID(81114);
      this.snapCursorToCurrentSnappedComponent();
    }

    private static void LaunchBrowser(string url)
    {
      Game1.playSound("bigSelect");
      try
      {
        Process.Start(url);
      }
      catch (Exception ex)
      {
      }
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
      base.receiveLeftClick(x, y, playSound);
      if (this.linkToSVSite.containsPoint(x, y) && this.linkToSVSite.visible)
        AboutMenu.LaunchBrowser("http://www.stardewvalley.net");
      else if (this.linkToTwitter.containsPoint(x, y) && this.linkToTwitter.visible)
        AboutMenu.LaunchBrowser("http://www.twitter.com/ConcernedApe");
      else if (this.linkToChucklefish.containsPoint(x, y) && this.linkToChucklefish.visible)
        AboutMenu.LaunchBrowser("http://blog.chucklefish.org/");
      else
        this.isWithinBounds(x, y);
    }

    public override void performHoverAction(int x, int y)
    {
      base.performHoverAction(x, y);
      this.linkToSVSite.scale = 1f;
      this.linkToTwitter.scale = 1f;
      this.linkToChucklefish.scale = 1f;
      if (this.linkToSVSite.containsPoint(x, y))
        this.linkToSVSite.scale = 2f;
      else if (this.linkToTwitter.containsPoint(x, y))
      {
        this.linkToTwitter.scale = 2f;
      }
      else
      {
        if (!this.linkToChucklefish.containsPoint(x, y))
          return;
        this.linkToChucklefish.scale = 2f;
      }
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
    }

    public override void draw(SpriteBatch b)
    {
      Vector2 centeringOnScreen = Utility.getTopLeftPositionForCenteringOnScreen(950, 600, 0, 0);
      b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.75f);
      IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(473, 36, 24, 24), (int) centeringOnScreen.X, (int) centeringOnScreen.Y, 950, 550, Color.White, (float) Game1.pixelZoom, true);
      SpriteText.drawString(b, Game1.content.LoadString("Strings\\UI:About_Title"), (int) centeringOnScreen.X + Game1.tileSize / 2, (int) centeringOnScreen.Y + Game1.tileSize / 2, 9999, -1, 9999, 1f, 0.88f, false, -1, "", 6);
      string str = Game1.content.LoadString("Strings\\UI:About_Credit");
      SpriteText.drawString(b, str.Replace('\n', '^'), (int) centeringOnScreen.X + Game1.tileSize / 2, (int) centeringOnScreen.Y + Game1.tileSize / 2, 9999, -1, 9999, 1f, 0.88f, false, -1, "", 4);
      if (this.linkToSVSite.visible)
        SpriteText.drawString(b, "= " + this.linkToSVSite.label, (int) centeringOnScreen.X + Game1.tileSize / 2, this.linkToSVSite.bounds.Y, 999, -1, 999, 1f, 1f, false, -1, "", (double) this.linkToSVSite.scale == 1.0 ? 3 : 7);
      if (this.linkToTwitter.visible)
        SpriteText.drawString(b, "= " + this.linkToTwitter.label, (int) centeringOnScreen.X + Game1.tileSize / 2, this.linkToTwitter.bounds.Y, 999, -1, 999, 1f, 1f, false, -1, "", (double) this.linkToTwitter.scale == 1.0 ? 3 : 7);
      if (this.linkToChucklefish.visible)
        SpriteText.drawString(b, "< " + this.linkToChucklefish.label, (int) centeringOnScreen.X + Game1.tileSize / 2, this.linkToChucklefish.bounds.Y, 999, -1, 999, 1f, 1f, false, -1, "", (double) this.linkToChucklefish.scale == 1.0 ? 3 : 7);
      if ((double) this.linkToChucklefish.scale > 1.0)
        b.Draw(Game1.objectSpriteSheet, new Vector2((float) (this.linkToChucklefish.bounds.Right - Game1.tileSize * 5), (float) (this.linkToChucklefish.bounds.Y - Game1.pixelZoom)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 128, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.89f);
      else if ((double) this.linkToSVSite.scale <= 1.0)
      {
        double scale = (double) this.linkToTwitter.scale;
      }
      b.Draw(Game1.mouseCursors, new Vector2(centeringOnScreen.X + 950f - (float) (Game1.tileSize * 3 / 2), centeringOnScreen.Y + (float) (Game1.tileSize * 2)), new Rectangle?(new Rectangle(540 + 13 * (int) (Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 600.0 / 150.0), 333, 13, 13)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1f);
      if (this.linkToSVSite.visible || this.linkToTwitter.visible || this.linkToChucklefish.visible)
        b.Draw(Game1.mouseCursors, new Vector2(centeringOnScreen.X + 950f - (float) (Game1.tileSize * 3 / 2), centeringOnScreen.Y + 700f - (float) (Game1.tileSize * 4)), new Rectangle?(new Rectangle(592 + 13 * (int) (Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 600.0 / 150.0), 333, 13, 13)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1f);
      if (Game1.activeClickableMenu != null && Game1.activeClickableMenu is TitleMenu && (Game1.activeClickableMenu as TitleMenu).startupMessage.Length > 0)
        b.DrawString(Game1.smallFont, Game1.parseText((Game1.activeClickableMenu as TitleMenu).startupMessage, Game1.smallFont, Game1.tileSize * 10), new Vector2((float) (Game1.pixelZoom * 2), (float) Game1.viewport.Height - Game1.smallFont.MeasureString(Game1.parseText((Game1.activeClickableMenu as TitleMenu).startupMessage, Game1.smallFont, Game1.tileSize * 10)).Y - (float) Game1.pixelZoom), Color.White);
      else
        b.DrawString(Game1.smallFont, "v1.2.33", new Vector2((float) (Game1.tileSize / 4), (float) Game1.viewport.Height - Game1.smallFont.MeasureString("v1.2.33").Y - (float) (Game1.pixelZoom * 2)), Color.White);
    }
  }
}
