// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.GameMenu
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Menus
{
  public class GameMenu : IClickableMenu
  {
    private string hoverText = "";
    private string descriptionText = "";
    private List<ClickableComponent> tabs = new List<ClickableComponent>();
    private List<IClickableMenu> pages = new List<IClickableMenu>();
    public const int inventoryTab = 0;
    public const int skillsTab = 1;
    public const int socialTab = 2;
    public const int mapTab = 3;
    public const int craftingTab = 4;
    public const int collectionsTab = 5;
    public const int optionsTab = 6;
    public const int exitTab = 7;
    public const int region_inventoryTab = 12340;
    public const int region_skillsTab = 12341;
    public const int region_socialTab = 12342;
    public const int region_mapTab = 12343;
    public const int region_craftingTab = 12344;
    public const int region_collectionsTab = 12345;
    public const int region_optionsTab = 12346;
    public const int region_exitTab = 12347;
    public const int numberOfTabs = 7;
    public int currentTab;
    public bool invisible;
    public static bool forcePreventClose;
    public ClickableTextureComponent junimoNoteIcon;

    public GameMenu()
      : base(Game1.viewport.Width / 2 - (800 + IClickableMenu.borderWidth * 2) / 2, Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2, 800 + IClickableMenu.borderWidth * 2, 600 + IClickableMenu.borderWidth * 2, true)
    {
      this.tabs.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize, this.yPositionOnScreen + IClickableMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "inventory", Game1.content.LoadString("Strings\\UI:GameMenu_Inventory"))
      {
        myID = 12340,
        downNeighborID = 0,
        rightNeighborID = 12341,
        tryDefaultIfNoDownNeighborExists = true,
        fullyImmutable = true
      });
      this.pages.Add((IClickableMenu) new InventoryPage(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height));
      this.tabs.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 2, this.yPositionOnScreen + IClickableMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "skills", Game1.content.LoadString("Strings\\UI:GameMenu_Skills"))
      {
        myID = 12341,
        downNeighborID = 1,
        rightNeighborID = 12342,
        leftNeighborID = 12340,
        tryDefaultIfNoDownNeighborExists = true,
        fullyImmutable = true
      });
      this.pages.Add((IClickableMenu) new SkillsPage(this.xPositionOnScreen, this.yPositionOnScreen, this.width + (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ru ? Game1.tileSize : 0), this.height));
      this.tabs.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 3, this.yPositionOnScreen + IClickableMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "social", Game1.content.LoadString("Strings\\UI:GameMenu_Social"))
      {
        myID = 12342,
        downNeighborID = 2,
        rightNeighborID = 12343,
        leftNeighborID = 12341,
        tryDefaultIfNoDownNeighborExists = true,
        fullyImmutable = true
      });
      this.pages.Add((IClickableMenu) new SocialPage(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height));
      this.tabs.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 4, this.yPositionOnScreen + IClickableMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "map", Game1.content.LoadString("Strings\\UI:GameMenu_Map"))
      {
        myID = 12343,
        downNeighborID = 3,
        rightNeighborID = 12344,
        leftNeighborID = 12342,
        tryDefaultIfNoDownNeighborExists = true,
        fullyImmutable = true
      });
      this.pages.Add((IClickableMenu) new MapPage(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height));
      this.tabs.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 5, this.yPositionOnScreen + IClickableMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "crafting", Game1.content.LoadString("Strings\\UI:GameMenu_Crafting"))
      {
        myID = 12344,
        downNeighborID = 4,
        rightNeighborID = 12345,
        leftNeighborID = 12343,
        tryDefaultIfNoDownNeighborExists = true,
        fullyImmutable = true
      });
      this.pages.Add((IClickableMenu) new CraftingPage(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false));
      this.tabs.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 6, this.yPositionOnScreen + IClickableMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "collections", Game1.content.LoadString("Strings\\UI:GameMenu_Collections"))
      {
        myID = 12345,
        downNeighborID = 5,
        rightNeighborID = 12346,
        leftNeighborID = 12344,
        tryDefaultIfNoDownNeighborExists = true,
        fullyImmutable = true
      });
      this.pages.Add((IClickableMenu) new CollectionsPage(this.xPositionOnScreen, this.yPositionOnScreen, this.width - Game1.tileSize - Game1.tileSize / 4, this.height));
      this.tabs.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 7, this.yPositionOnScreen + IClickableMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "options", Game1.content.LoadString("Strings\\UI:GameMenu_Options"))
      {
        myID = 12346,
        downNeighborID = 6,
        rightNeighborID = 12347,
        leftNeighborID = 12345,
        tryDefaultIfNoDownNeighborExists = true,
        fullyImmutable = true
      });
      this.pages.Add((IClickableMenu) new OptionsPage(this.xPositionOnScreen, this.yPositionOnScreen, this.width + (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ru ? Game1.tileSize * 3 / 2 : Game1.tileSize / 2), this.height));
      this.tabs.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize * 8, this.yPositionOnScreen + IClickableMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "exit", Game1.content.LoadString("Strings\\UI:GameMenu_Exit"))
      {
        myID = 12347,
        downNeighborID = 7,
        leftNeighborID = 12346,
        tryDefaultIfNoDownNeighborExists = true,
        fullyImmutable = true
      });
      this.pages.Add((IClickableMenu) new ExitPage(this.xPositionOnScreen, this.yPositionOnScreen, this.width - Game1.tileSize - Game1.tileSize / 4, this.height));
      if (Game1.activeClickableMenu == null)
        Game1.playSound("bigSelect");
      if (Game1.player.hasOrWillReceiveMail("canReadJunimoText") && !Game1.player.hasOrWillReceiveMail("JojaMember") && !Game1.player.hasCompletedCommunityCenter())
      {
        ClickableTextureComponent textureComponent = new ClickableTextureComponent("", new Rectangle(this.xPositionOnScreen + this.width, this.yPositionOnScreen + Game1.tileSize * 3 / 2, Game1.tileSize, Game1.tileSize), "", Game1.content.LoadString("Strings\\UI:GameMenu_JunimoNote_Hover"), Game1.mouseCursors, new Rectangle(331, 374, 15, 14), (float) Game1.pixelZoom, false);
        int num1 = 898;
        textureComponent.myID = num1;
        int num2 = 11;
        textureComponent.leftNeighborID = num2;
        int num3 = 106;
        textureComponent.downNeighborID = num3;
        this.junimoNoteIcon = textureComponent;
      }
      GameMenu.forcePreventClose = false;
      if (!Game1.options.SnappyMenus)
        return;
      this.pages[this.currentTab].populateClickableComponentList();
      this.pages[this.currentTab].allClickableComponents.AddRange((IEnumerable<ClickableComponent>) this.tabs);
      this.snapToDefaultClickableComponent();
    }

    public GameMenu(int startingTab, int extra = -1)
      : this()
    {
      this.changeTab(startingTab);
      if (startingTab != 6 || extra == -1)
        return;
      (this.pages[6] as OptionsPage).currentItemIndex = extra;
    }

    public override void snapToDefaultClickableComponent()
    {
      if (this.currentTab < this.pages.Count)
        this.pages[this.currentTab].snapToDefaultClickableComponent();
      if (this.junimoNoteIcon == null || this.currentTab >= this.pages.Count || this.pages[this.currentTab].allClickableComponents.Contains((ClickableComponent) this.junimoNoteIcon))
        return;
      this.pages[this.currentTab].allClickableComponents.Add((ClickableComponent) this.junimoNoteIcon);
    }

    public override void receiveGamePadButton(Buttons b)
    {
      base.receiveGamePadButton(b);
      if (b == Buttons.RightTrigger)
      {
        if (this.currentTab == 3)
        {
          Game1.activeClickableMenu = (IClickableMenu) new GameMenu(4, -1);
        }
        else
        {
          if (this.currentTab >= 7 || !this.pages[this.currentTab].readyToClose())
            return;
          this.changeTab(this.currentTab + 1);
        }
      }
      else if (b == Buttons.LeftTrigger)
      {
        if (this.currentTab == 3)
        {
          Game1.activeClickableMenu = (IClickableMenu) new GameMenu(2, -1);
        }
        else
        {
          if (this.currentTab <= 0 || !this.pages[this.currentTab].readyToClose())
            return;
          this.changeTab(this.currentTab - 1);
        }
      }
      else
      {
        if (b != Buttons.Back || this.currentTab != 0)
          return;
        this.pages[this.currentTab].receiveGamePadButton(b);
      }
    }

    public override void setUpForGamePadMode()
    {
      base.setUpForGamePadMode();
      if (this.pages.Count <= this.currentTab)
        return;
      this.pages[this.currentTab].setUpForGamePadMode();
    }

    public override ClickableComponent getCurrentlySnappedComponent()
    {
      return this.pages[this.currentTab].getCurrentlySnappedComponent();
    }

    public override void setCurrentlySnappedComponentTo(int id)
    {
      this.pages[this.currentTab].setCurrentlySnappedComponentTo(id);
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
      base.receiveLeftClick(x, y, playSound);
      if (!this.invisible && !GameMenu.forcePreventClose)
      {
        for (int index = 0; index < this.tabs.Count; ++index)
        {
          if (this.tabs[index].containsPoint(x, y) && this.currentTab != index && this.pages[this.currentTab].readyToClose())
          {
            this.changeTab(this.getTabNumberFromName(this.tabs[index].name));
            return;
          }
        }
        if (this.junimoNoteIcon != null && this.junimoNoteIcon.containsPoint(x, y) && this.pages[this.currentTab].readyToClose())
          Game1.activeClickableMenu = (IClickableMenu) new JunimoNoteMenu(true, 1, false);
      }
      this.pages[this.currentTab].receiveLeftClick(x, y, true);
    }

    public static string getLabelOfTabFromIndex(int index)
    {
      switch (index)
      {
        case 0:
          return Game1.content.LoadString("Strings\\UI:GameMenu_Inventory");
        case 1:
          return Game1.content.LoadString("Strings\\UI:GameMenu_Skills");
        case 2:
          return Game1.content.LoadString("Strings\\UI:GameMenu_Social");
        case 3:
          return Game1.content.LoadString("Strings\\UI:GameMenu_Map");
        case 4:
          return Game1.content.LoadString("Strings\\UI:GameMenu_Crafting");
        case 5:
          return Game1.content.LoadString("Strings\\UI:GameMenu_Collections");
        case 6:
          return Game1.content.LoadString("Strings\\UI:GameMenu_Options");
        case 7:
          return Game1.content.LoadString("Strings\\UI:GameMenu_Exit");
        default:
          return "";
      }
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
      this.pages[this.currentTab].receiveRightClick(x, y, true);
    }

    public override void receiveScrollWheelAction(int direction)
    {
      base.receiveScrollWheelAction(direction);
      this.pages[this.currentTab].receiveScrollWheelAction(direction);
    }

    public override void performHoverAction(int x, int y)
    {
      base.performHoverAction(x, y);
      this.hoverText = "";
      this.pages[this.currentTab].performHoverAction(x, y);
      foreach (ClickableComponent tab in this.tabs)
      {
        if (tab.containsPoint(x, y))
        {
          this.hoverText = tab.label;
          return;
        }
      }
      if (this.junimoNoteIcon == null)
        return;
      this.junimoNoteIcon.tryHover(x, y, 0.1f);
      if (!this.junimoNoteIcon.containsPoint(x, y))
        return;
      this.hoverText = this.junimoNoteIcon.hoverText;
    }

    public int getTabNumberFromName(string name)
    {
      int num = -1;
      // ISSUE: reference to a compiler-generated method
      uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(name);
      if (stringHash <= 3454868101U)
      {
        if (stringHash <= 3001865938U)
        {
          if ((int) stringHash != 1700191391)
          {
            if ((int) stringHash == -1293101358 && name == "social")
              num = 2;
          }
          else if (name == "skills")
            num = 1;
        }
        else if ((int) stringHash != -1246894561)
        {
          if ((int) stringHash == -840099195 && name == "exit")
            num = 7;
        }
        else if (name == "crafting")
          num = 4;
      }
      else if (stringHash <= 3760730054U)
      {
        if ((int) stringHash != -542969935)
        {
          if ((int) stringHash == -534237242 && name == "collections")
            num = 5;
        }
        else if (name == "map")
          num = 3;
      }
      else if ((int) stringHash != -282563419)
      {
        if ((int) stringHash == -50478017 && name == "inventory")
          num = 0;
      }
      else if (name == "options")
        num = 6;
      return num;
    }

    public override void releaseLeftClick(int x, int y)
    {
      base.releaseLeftClick(x, y);
      this.pages[this.currentTab].releaseLeftClick(x, y);
    }

    public override void leftClickHeld(int x, int y)
    {
      base.leftClickHeld(x, y);
      this.pages[this.currentTab].leftClickHeld(x, y);
    }

    public override bool readyToClose()
    {
      if (!GameMenu.forcePreventClose)
        return this.pages[this.currentTab].readyToClose();
      return false;
    }

    public void changeTab(int whichTab)
    {
      if (this.currentTab == 2)
      {
        if (this.junimoNoteIcon != null)
          this.junimoNoteIcon = new ClickableTextureComponent("", new Rectangle(this.xPositionOnScreen + this.width, this.yPositionOnScreen + Game1.tileSize * 3 / 2, Game1.tileSize, Game1.tileSize), "", Game1.content.LoadString("Strings\\UI:GameMenu_JunimoNote_Hover"), Game1.mouseCursors, new Rectangle(331, 374, 15, 14), (float) Game1.pixelZoom, false);
      }
      else if (whichTab == 2 && this.junimoNoteIcon != null)
        this.junimoNoteIcon.bounds.X += Game1.tileSize;
      this.currentTab = this.getTabNumberFromName(this.tabs[whichTab].name);
      if (this.currentTab == 3)
      {
        this.invisible = true;
        this.width = this.width + Game1.tileSize * 2;
        this.initializeUpperRightCloseButton();
      }
      else
      {
        this.width = 800 + IClickableMenu.borderWidth * 2;
        this.initializeUpperRightCloseButton();
        this.invisible = false;
      }
      Game1.playSound("smallSelect");
      if (!Game1.options.SnappyMenus)
        return;
      this.pages[this.currentTab].populateClickableComponentList();
      this.pages[this.currentTab].allClickableComponents.AddRange((IEnumerable<ClickableComponent>) this.tabs);
      this.setTabNeighborsForCurrentPage();
      this.snapToDefaultClickableComponent();
      if (whichTab == 2)
      {
        this.pages[this.currentTab].currentlySnappedComponent = this.tabs[2];
        this.snapCursorToCurrentSnappedComponent();
      }
      if (this.currentTab != 0 || this.junimoNoteIcon == null)
        return;
      this.junimoNoteIcon.leftNeighborID = 11;
      this.junimoNoteIcon.downNeighborID = 105;
    }

    public void setTabNeighborsForCurrentPage()
    {
      switch (this.currentTab)
      {
        case 0:
          for (int index = 0; index < this.tabs.Count; ++index)
            this.tabs[index].downNeighborID = index;
          break;
        case 7:
          for (int index = 0; index < this.tabs.Count; ++index)
            this.tabs[index].downNeighborID = 535;
          break;
        default:
          for (int index = 0; index < this.tabs.Count; ++index)
            this.tabs[index].downNeighborID = -99999;
          break;
      }
    }

    public override void draw(SpriteBatch b)
    {
      if (!this.invisible)
      {
        if (!Game1.options.showMenuBackground)
          b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.4f);
        Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.pages[this.currentTab].width, this.pages[this.currentTab].height, false, true, (string) null, false);
        this.pages[this.currentTab].draw(b);
        b.End();
        b.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
        if (!GameMenu.forcePreventClose)
        {
          foreach (ClickableComponent tab in this.tabs)
          {
            int num = 0;
            string name = tab.name;
            // ISSUE: reference to a compiler-generated method
            uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(name);
            if (stringHash <= 3048072735U)
            {
              if (stringHash <= 2237694710U)
              {
                if ((int) stringHash != 1700191391)
                {
                  if ((int) stringHash == -2057272586 && name == "catalogue")
                    num = 7;
                }
                else if (name == "skills")
                  num = 1;
              }
              else if ((int) stringHash != -1293101358)
              {
                if ((int) stringHash == -1246894561 && name == "crafting")
                  num = 4;
              }
              else if (name == "social")
                num = 2;
            }
            else if (stringHash <= 3751997361U)
            {
              if ((int) stringHash != -840099195)
              {
                if ((int) stringHash == -542969935 && name == "map")
                  num = 3;
              }
              else if (name == "exit")
                num = 7;
            }
            else if ((int) stringHash != -534237242)
            {
              if ((int) stringHash != -282563419)
              {
                if ((int) stringHash == -50478017 && name == "inventory")
                  num = 0;
              }
              else if (name == "options")
                num = 6;
            }
            else if (name == "collections")
              num = 5;
            b.Draw(Game1.mouseCursors, new Vector2((float) tab.bounds.X, (float) (tab.bounds.Y + (this.currentTab == this.getTabNumberFromName(tab.name) ? 8 : 0))), new Rectangle?(new Rectangle(num * 16, 368, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.0001f);
            if (tab.name.Equals("skills"))
              Game1.player.FarmerRenderer.drawMiniPortrat(b, new Vector2((float) (tab.bounds.X + 8), (float) (tab.bounds.Y + 12 + (this.currentTab == this.getTabNumberFromName(tab.name) ? 8 : 0))), 0.00011f, 3f, 2, Game1.player);
          }
          b.End();
          b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
          if (this.junimoNoteIcon != null)
            this.junimoNoteIcon.draw(b);
          if (!this.hoverText.Equals(""))
            IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, -1, (string) null, -1, (string[]) null, (Item) null, 0, -1, -1, -1, -1, 1f, (CraftingRecipe) null);
        }
      }
      else
        this.pages[this.currentTab].draw(b);
      if (!GameMenu.forcePreventClose)
        base.draw(b);
      if (Game1.options.hardwareCursor)
        return;
      b.Draw(Game1.mouseCursors, new Vector2((float) Game1.getOldMouseX(), (float) Game1.getOldMouseY()), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, Game1.options.gamepadControls ? 44 : 0, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
    }

    public override bool areGamePadControlsImplemented()
    {
      return false;
    }

    public override void receiveKeyPress(Keys key)
    {
      if (((IEnumerable<InputButton>) Game1.options.menuButton).Contains<InputButton>(new InputButton(key)) && this.readyToClose())
      {
        Game1.exitActiveMenu();
        Game1.playSound("bigDeSelect");
      }
      this.pages[this.currentTab].receiveKeyPress(key);
    }
  }
}
