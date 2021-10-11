// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.ChooseFromListMenu
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using System.Collections.Generic;

namespace StardewValley.Menus
{
  public class ChooseFromListMenu : IClickableMenu
  {
    private List<string> options = new List<string>();
    public const int region_backButton = 101;
    public const int region_forwardButton = 102;
    public const int region_okButton = 103;
    public const int region_cancelButton = 104;
    public const int w = 640;
    public const int h = 192;
    public ClickableTextureComponent backButton;
    public ClickableTextureComponent forwardButton;
    public ClickableTextureComponent okButton;
    public ClickableTextureComponent cancelButton;
    private int index;
    private ChooseFromListMenu.actionOnChoosingListOption chooseAction;
    private bool isJukebox;

    public ChooseFromListMenu(List<string> options, ChooseFromListMenu.actionOnChoosingListOption chooseAction, bool isJukebox = false)
      : base(Game1.viewport.Width / 2 - 320, Game1.viewport.Height - Game1.tileSize - 192, 640, 192, false)
    {
      this.chooseAction = chooseAction;
      ClickableTextureComponent textureComponent1 = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen - Game1.tileSize * 2 - Game1.pixelZoom, this.yPositionOnScreen + Game1.tileSize * 4 / 3, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(352, 495, 12, 11), (float) Game1.pixelZoom, false);
      int num1 = 101;
      textureComponent1.myID = num1;
      int num2 = 102;
      textureComponent1.rightNeighborID = num2;
      this.backButton = textureComponent1;
      ClickableTextureComponent textureComponent2 = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + 640 + Game1.pixelZoom * 4 + Game1.tileSize, this.yPositionOnScreen + Game1.tileSize * 4 / 3, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(365, 495, 12, 11), (float) Game1.pixelZoom, false);
      int num3 = 102;
      textureComponent2.myID = num3;
      int num4 = 101;
      textureComponent2.leftNeighborID = num4;
      int num5 = 103;
      textureComponent2.rightNeighborID = num5;
      this.forwardButton = textureComponent2;
      ClickableTextureComponent textureComponent3 = new ClickableTextureComponent("OK", new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize * 2 + Game1.pixelZoom * 2, this.yPositionOnScreen + 192 - Game1.tileSize * 2, Game1.tileSize, Game1.tileSize), (string) null, (string) null, Game1.mouseCursors, new Rectangle(175, 379, 16, 15), (float) Game1.pixelZoom, false);
      int num6 = 103;
      textureComponent3.myID = num6;
      int num7 = 102;
      textureComponent3.leftNeighborID = num7;
      int num8 = 104;
      textureComponent3.rightNeighborID = num8;
      this.okButton = textureComponent3;
      ClickableTextureComponent textureComponent4 = new ClickableTextureComponent("OK", new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize * 3 + Game1.pixelZoom * 3, this.yPositionOnScreen + 192 - Game1.tileSize * 2, Game1.tileSize, Game1.tileSize), (string) null, (string) null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 47, -1, -1), 1f, false);
      int num9 = 104;
      textureComponent4.myID = num9;
      int num10 = 103;
      textureComponent4.leftNeighborID = num10;
      this.cancelButton = textureComponent4;
      Game1.playSound("bigSelect");
      this.isJukebox = isJukebox;
      if (isJukebox)
      {
        for (int index = options.Count - 1; index >= 0; --index)
        {
          if (options[index].ToLower().Contains("ambient") || options[index].ToLower().Contains("bigdrums") || options[index].ToLower().Contains("clubloop"))
          {
            options.RemoveAt(index);
          }
          else
          {
            string option = options[index];
            // ISSUE: reference to a compiler-generated method
            uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(option);
            if (stringHash <= 1611928003U)
            {
              if ((int) stringHash != 575982768)
              {
                if ((int) stringHash != 1176080900)
                {
                  if ((int) stringHash == 1611928003 && option == "buglevelloop")
                    options.RemoveAt(index);
                }
                else if (option == "jojaOfficeSoundscape")
                  options.RemoveAt(index);
              }
              else if (option == "title_day")
              {
                options.RemoveAt(index);
                options.Add("MainTheme");
              }
            }
            else if (stringHash <= 3528263180U)
            {
              if ((int) stringHash != -962254472)
              {
                if ((int) stringHash == -766704116 && option == "coin")
                  options.RemoveAt(index);
              }
              else if (option == "nightTime")
                options.RemoveAt(index);
            }
            else if ((int) stringHash != -730834543)
            {
              if ((int) stringHash == -475385117 && option == "communityCenter")
                options.RemoveAt(index);
            }
            else if (option == "ocean")
              options.RemoveAt(index);
          }
        }
      }
      this.options = options;
      if (!Game1.options.SnappyMenus)
        return;
      this.populateClickableComponentList();
      this.snapToDefaultClickableComponent();
    }

    public override void snapToDefaultClickableComponent()
    {
      this.currentlySnappedComponent = this.getComponentWithID(103);
      this.snapCursorToCurrentSnappedComponent();
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
    }

    public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
    {
      base.gameWindowSizeChanged(oldBounds, newBounds);
      this.xPositionOnScreen = Game1.viewport.Width / 2 - 320;
      this.yPositionOnScreen = Game1.viewport.Height - Game1.tileSize - 192;
      this.backButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen - Game1.tileSize * 2 - Game1.pixelZoom, this.yPositionOnScreen + Game1.tileSize * 4 / 3, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(352, 495, 12, 11), (float) Game1.pixelZoom, false);
      this.forwardButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + 640 + Game1.pixelZoom * 4 + Game1.tileSize, this.yPositionOnScreen + Game1.tileSize * 4 / 3, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(365, 495, 12, 11), (float) Game1.pixelZoom, false);
      this.okButton = new ClickableTextureComponent("OK", new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize * 2 + Game1.pixelZoom * 2, this.yPositionOnScreen + 192 - Game1.tileSize * 2, Game1.tileSize, Game1.tileSize), (string) null, (string) null, Game1.mouseCursors, new Rectangle(175, 379, 16, 15), (float) Game1.pixelZoom, false);
      this.cancelButton = new ClickableTextureComponent("OK", new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize * 3 + Game1.pixelZoom * 3, this.yPositionOnScreen + 192 - Game1.tileSize * 2, Game1.tileSize, Game1.tileSize), (string) null, (string) null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 47, -1, -1), 1f, false);
    }

    public static void playSongAction(string s)
    {
      Game1.changeMusicTrack(s);
    }

    public override void performHoverAction(int x, int y)
    {
      base.performHoverAction(x, y);
      this.okButton.tryHover(x, y, 0.1f);
      this.cancelButton.tryHover(x, y, 0.1f);
      this.backButton.tryHover(x, y, 0.1f);
      this.forwardButton.tryHover(x, y, 0.1f);
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
      base.receiveLeftClick(x, y, playSound);
      if (this.okButton.containsPoint(x, y) && this.chooseAction != null)
      {
        this.chooseAction(this.options[this.index]);
        Game1.playSound("select");
      }
      if (this.cancelButton.containsPoint(x, y))
        this.exitThisMenu(true);
      if (this.backButton.containsPoint(x, y))
      {
        this.index = this.index - 1;
        if (this.index < 0)
          this.index = this.options.Count - 1;
        this.backButton.scale = this.backButton.baseScale - 1f;
        Game1.playSound("shwip");
      }
      if (!this.forwardButton.containsPoint(x, y))
        return;
      this.index = this.index + 1;
      this.index = this.index % this.options.Count;
      Game1.playSound("shwip");
      this.forwardButton.scale = this.forwardButton.baseScale - 1f;
    }

    public override void draw(SpriteBatch b)
    {
      base.draw(b);
      string str = "Summer (The Sun Can Bend An Orange Sky)";
      int x = (int) Game1.dialogueFont.MeasureString(this.isJukebox ? str : this.options[this.index]).X;
      IClickableMenu.drawTextureBox(b, this.xPositionOnScreen + this.width / 2 - x / 2 - Game1.pixelZoom * 4, this.yPositionOnScreen + Game1.tileSize - Game1.pixelZoom, x + Game1.tileSize / 2, Game1.tileSize + Game1.tileSize / 4, Color.White);
      if (this.index < this.options.Count)
        Utility.drawTextWithShadow(b, this.isJukebox ? Utility.getSongTitleFromCueName(this.options[this.index]) : this.options[this.index], Game1.dialogueFont, new Vector2((float) (this.xPositionOnScreen + this.width / 2) - Game1.dialogueFont.MeasureString(this.isJukebox ? Utility.getSongTitleFromCueName(this.options[this.index]) : this.options[this.index]).X / 2f, (float) (this.yPositionOnScreen + this.height / 2 - Game1.pixelZoom * 4)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
      this.okButton.draw(b);
      this.cancelButton.draw(b);
      this.forwardButton.draw(b);
      this.backButton.draw(b);
      if (this.isJukebox)
        SpriteText.drawStringWithScrollCenteredAt(b, Game1.content.LoadString("Strings\\UI:JukeboxMenu_Title"), this.xPositionOnScreen + this.width / 2, this.yPositionOnScreen - Game1.tileSize / 2, "", 1f, -1, 0, 0.88f, false);
      this.drawMouse(b);
    }

    public delegate void actionOnChoosingListOption(string s);
  }
}
