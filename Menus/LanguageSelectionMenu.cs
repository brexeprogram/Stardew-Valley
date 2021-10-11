// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.LanguageSelectionMenu
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace StardewValley.Menus
{
  public class LanguageSelectionMenu : IClickableMenu
  {
    public List<ClickableComponent> languages = new List<ClickableComponent>();
    public new const int width = 500;
    public new const int height = 650;
    private Texture2D texture;
    private bool isReadyToClose;

    public LanguageSelectionMenu()
    {
      this.texture = Game1.temporaryContent.Load<Texture2D>("LooseSprites\\LanguageButtons");
      Vector2 centeringOnScreen = Utility.getTopLeftPositionForCenteringOnScreen(500, 650, 0, 0);
      this.languages.Clear();
      int height = (int) ((double) Game1.tileSize * 1.3);
      this.languages.Add(new ClickableComponent(new Rectangle((int) centeringOnScreen.X + Game1.tileSize, (int) centeringOnScreen.Y + 650 - 30 - height * 7 - Game1.pixelZoom * 4, 500 - Game1.tileSize * 2, height), "English", (string) null)
      {
        myID = 0,
        downNeighborID = 1
      });
      this.languages.Add(new ClickableComponent(new Rectangle((int) centeringOnScreen.X + Game1.tileSize, (int) centeringOnScreen.Y + 650 - 30 - height * 6 - Game1.pixelZoom * 4, 500 - Game1.tileSize * 2, height), "German", (string) null)
      {
        myID = 1,
        upNeighborID = 0,
        downNeighborID = 2
      });
      this.languages.Add(new ClickableComponent(new Rectangle((int) centeringOnScreen.X + Game1.tileSize, (int) centeringOnScreen.Y + 650 - 30 - height * 4 - Game1.pixelZoom * 4, 500 - Game1.tileSize * 2, height), "Russian", (string) null)
      {
        myID = 3,
        upNeighborID = 2,
        downNeighborID = 4
      });
      this.languages.Add(new ClickableComponent(new Rectangle((int) centeringOnScreen.X + Game1.tileSize, (int) centeringOnScreen.Y + 650 - 30 - height - Game1.pixelZoom * 4, 500 - Game1.tileSize * 2, height), "Chinese", (string) null)
      {
        myID = 6,
        upNeighborID = 5
      });
      this.languages.Add(new ClickableComponent(new Rectangle((int) centeringOnScreen.X + Game1.tileSize, (int) centeringOnScreen.Y + 650 - 30 - height * 2 - Game1.pixelZoom * 4, 500 - Game1.tileSize * 2, height), "Japanese", (string) null)
      {
        myID = 5,
        upNeighborID = 4,
        downNeighborID = 6
      });
      this.languages.Add(new ClickableComponent(new Rectangle((int) centeringOnScreen.X + Game1.tileSize, (int) centeringOnScreen.Y + 650 - 30 - height * 5 - Game1.pixelZoom * 4, 500 - Game1.tileSize * 2, height), "Spanish", (string) null)
      {
        myID = 2,
        upNeighborID = 1,
        downNeighborID = 3
      });
      this.languages.Add(new ClickableComponent(new Rectangle((int) centeringOnScreen.X + Game1.tileSize, (int) centeringOnScreen.Y + 650 - 30 - height * 3 - Game1.pixelZoom * 4, 500 - Game1.tileSize * 2, height), "Portuguese", (string) null)
      {
        myID = 4,
        upNeighborID = 3,
        downNeighborID = 5
      });
      if (!Game1.options.SnappyMenus)
        return;
      this.populateClickableComponentList();
      this.snapToDefaultClickableComponent();
    }

    public override void snapToDefaultClickableComponent()
    {
      this.currentlySnappedComponent = this.getComponentWithID(0);
      this.snapCursorToCurrentSnappedComponent();
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
      base.receiveLeftClick(x, y, playSound);
      foreach (ClickableComponent language in this.languages)
      {
        if (language.containsPoint(x, y))
        {
          Game1.playSound("select");
          string name = language.name;
          // ISSUE: reference to a compiler-generated method
          uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(name);
          if (stringHash <= 1197024134U)
          {
            if ((int) stringHash != 286263347)
            {
              if ((int) stringHash != 463134907)
              {
                if ((int) stringHash == 1197024134 && name == "Russian")
                {
                  LocalizedContentManager.CurrentLanguageCode = LocalizedContentManager.LanguageCode.ru;
                  goto label_24;
                }
              }
              else if (name == "English")
              {
                LocalizedContentManager.CurrentLanguageCode = LocalizedContentManager.LanguageCode.en;
                goto label_24;
              }
            }
            else if (name == "German")
            {
              LocalizedContentManager.CurrentLanguageCode = LocalizedContentManager.LanguageCode.de;
              goto label_24;
            }
          }
          else if (stringHash <= 2483826186U)
          {
            if ((int) stringHash != 2115103848)
            {
              if ((int) stringHash == -1811141110 && name == "Japanese")
              {
                LocalizedContentManager.CurrentLanguageCode = LocalizedContentManager.LanguageCode.ja;
                goto label_24;
              }
            }
            else if (name == "Chinese")
            {
              LocalizedContentManager.CurrentLanguageCode = LocalizedContentManager.LanguageCode.zh;
              goto label_24;
            }
          }
          else if ((int) stringHash != -1206287781)
          {
            if ((int) stringHash == -422150820 && name == "Portuguese")
            {
              LocalizedContentManager.CurrentLanguageCode = LocalizedContentManager.LanguageCode.pt;
              goto label_24;
            }
          }
          else if (name == "Spanish")
          {
            LocalizedContentManager.CurrentLanguageCode = LocalizedContentManager.LanguageCode.es;
            goto label_24;
          }
          LocalizedContentManager.CurrentLanguageCode = LocalizedContentManager.LanguageCode.en;
label_24:
          this.isReadyToClose = true;
          if (Game1.options.SnappyMenus)
          {
            Game1.activeClickableMenu.setCurrentlySnappedComponentTo(81118);
            Game1.activeClickableMenu.snapCursorToCurrentSnappedComponent();
          }
        }
      }
      this.isWithinBounds(x, y);
    }

    public override void performHoverAction(int x, int y)
    {
      base.performHoverAction(x, y);
      foreach (ClickableComponent language in this.languages)
      {
        if (language.containsPoint(x, y))
        {
          if (language.label == null)
          {
            Game1.playSound("Cowboy_Footstep");
            language.label = "hovered";
          }
        }
        else
          language.label = (string) null;
      }
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
    }

    public override void draw(SpriteBatch b)
    {
      Vector2 centeringOnScreen = Utility.getTopLeftPositionForCenteringOnScreen(500, 550, 0, 0);
      b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.6f);
      IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(473, 36, 24, 24), (int) centeringOnScreen.X + Game1.tileSize / 2, (int) centeringOnScreen.Y - 55, 500 - Game1.tileSize, 640, Color.White, (float) Game1.pixelZoom, true);
      foreach (ClickableComponent language in this.languages)
      {
        int num = 0;
        string name = language.name;
        // ISSUE: reference to a compiler-generated method
        uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(name);
        if (stringHash <= 1197024134U)
        {
          if ((int) stringHash != 286263347)
          {
            if ((int) stringHash != 463134907)
            {
              if ((int) stringHash == 1197024134 && name == "Russian")
                num = 3;
            }
            else if (name == "English")
              num = 0;
          }
          else if (name == "German")
            num = 6;
        }
        else if (stringHash <= 2483826186U)
        {
          if ((int) stringHash != 2115103848)
          {
            if ((int) stringHash == -1811141110 && name == "Japanese")
              num = 5;
          }
          else if (name == "Chinese")
            num = 4;
        }
        else if ((int) stringHash != -1206287781)
        {
          if ((int) stringHash == -422150820 && name == "Portuguese")
            num = 2;
        }
        else if (name == "Spanish")
          num = 1;
        int y = num * 78 + (language.label != null ? 39 : 0);
        b.Draw(this.texture, language.bounds, new Rectangle?(new Rectangle(0, y, 174, 40)), Color.White, 0.0f, new Vector2(0.0f, 0.0f), SpriteEffects.None, 0.0f);
      }
    }

    public override bool readyToClose()
    {
      return this.isReadyToClose;
    }
  }
}
