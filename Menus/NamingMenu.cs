// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.NamingMenu
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using System;

namespace StardewValley.Menus
{
  public class NamingMenu : IClickableMenu
  {
    protected int minLength = 1;
    public const int region_okButton = 101;
    public const int region_doneNamingButton = 102;
    public const int region_randomButton = 103;
    public const int region_namingBox = 104;
    public ClickableTextureComponent doneNamingButton;
    public ClickableTextureComponent randomButton;
    protected TextBox textBox;
    public ClickableComponent textBoxCC;
    private TextBoxEvent e;
    private NamingMenu.doneNamingBehavior doneNaming;
    private string title;

    public NamingMenu(NamingMenu.doneNamingBehavior b, string title, string defaultName = null)
    {
      this.doneNaming = b;
      this.xPositionOnScreen = 0;
      this.yPositionOnScreen = 0;
      this.width = Game1.viewport.Width;
      this.height = Game1.viewport.Height;
      this.title = title;
      this.randomButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize * 4 / 5 + Game1.tileSize, Game1.viewport.Height / 2, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, new Rectangle(381, 361, 10, 10), (float) Game1.pixelZoom, false);
      this.textBox = new TextBox((Texture2D) null, (Texture2D) null, Game1.dialogueFont, Game1.textColor);
      this.textBox.X = Game1.viewport.Width / 2 - Game1.tileSize * 3;
      this.textBox.Y = Game1.viewport.Height / 2;
      this.textBox.Width = Game1.tileSize * 4;
      this.textBox.Height = Game1.tileSize * 3;
      this.e = new TextBoxEvent(this.textBoxEnter);
      this.textBox.OnEnterPressed += this.e;
      Game1.keyboardDispatcher.Subscriber = (IKeyboardSubscriber) this.textBox;
      this.textBox.Text = defaultName != null ? defaultName : Dialogue.randomName();
      this.textBox.Selected = true;
      ClickableTextureComponent textureComponent1 = new ClickableTextureComponent(new Rectangle(this.textBox.X + this.textBox.Width + Game1.tileSize + Game1.tileSize * 3 / 4 - Game1.pixelZoom * 2, Game1.viewport.Height / 2 + Game1.pixelZoom, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, new Rectangle(381, 361, 10, 10), (float) Game1.pixelZoom, false);
      int num1 = 103;
      textureComponent1.myID = num1;
      int num2 = 102;
      textureComponent1.leftNeighborID = num2;
      this.randomButton = textureComponent1;
      ClickableTextureComponent textureComponent2 = new ClickableTextureComponent(new Rectangle(this.textBox.X + this.textBox.Width + Game1.tileSize / 2 + Game1.pixelZoom, Game1.viewport.Height / 2 - Game1.pixelZoom * 2, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f, false);
      int num3 = 102;
      textureComponent2.myID = num3;
      int num4 = 103;
      textureComponent2.rightNeighborID = num4;
      int num5 = 104;
      textureComponent2.leftNeighborID = num5;
      this.doneNamingButton = textureComponent2;
      this.textBoxCC = new ClickableComponent(new Rectangle(this.textBox.X, this.textBox.Y, 192, 48), "")
      {
        myID = 104,
        rightNeighborID = 102
      };
      if (!Game1.options.SnappyMenus)
        return;
      this.populateClickableComponentList();
      this.snapToDefaultClickableComponent();
    }

    public override void snapToDefaultClickableComponent()
    {
      this.currentlySnappedComponent = this.getComponentWithID(104);
      this.snapCursorToCurrentSnappedComponent();
    }

    public void textBoxEnter(TextBox sender)
    {
      if (sender.Text.Length < this.minLength)
        return;
      if (this.doneNaming != null)
      {
        this.doneNaming(sender.Text);
        this.textBox.Selected = false;
      }
      else
        Game1.exitActiveMenu();
    }

    public override void receiveGamePadButton(Buttons b)
    {
      base.receiveGamePadButton(b);
      if (!this.textBox.Selected)
        return;
      switch (b)
      {
        case Buttons.LeftThumbstickUp:
        case Buttons.LeftThumbstickDown:
        case Buttons.LeftThumbstickRight:
        case Buttons.DPadUp:
        case Buttons.DPadDown:
        case Buttons.DPadLeft:
        case Buttons.DPadRight:
        case Buttons.LeftThumbstickLeft:
          this.textBox.Selected = false;
          break;
      }
    }

    public override void receiveKeyPress(Keys key)
    {
      if (this.textBox.Selected || Game1.options.doesInputListContain(Game1.options.menuButton, key))
        return;
      base.receiveKeyPress(key);
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
    }

    public override void performHoverAction(int x, int y)
    {
      base.performHoverAction(x, y);
      if (this.doneNamingButton != null)
      {
        if (this.doneNamingButton.containsPoint(x, y))
          this.doneNamingButton.scale = Math.Min(1.1f, this.doneNamingButton.scale + 0.05f);
        else
          this.doneNamingButton.scale = Math.Max(1f, this.doneNamingButton.scale - 0.05f);
      }
      this.randomButton.tryHover(x, y, 0.5f);
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
      base.receiveLeftClick(x, y, playSound);
      this.textBox.Update();
      if (this.doneNamingButton.containsPoint(x, y))
      {
        this.textBoxEnter(this.textBox);
        Game1.playSound("smallSelect");
      }
      else if (this.randomButton.containsPoint(x, y))
      {
        this.textBox.Text = Dialogue.randomName();
        this.randomButton.scale = this.randomButton.baseScale;
        Game1.playSound("drumkit6");
      }
      this.textBox.Update();
    }

    public override void draw(SpriteBatch b)
    {
      base.draw(b);
      b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.75f);
      SpriteText.drawStringWithScrollCenteredAt(b, this.title, Game1.viewport.Width / 2, Game1.viewport.Height / 2 - Game1.tileSize * 2, this.title, 1f, -1, 0, 0.88f, false);
      this.textBox.Draw(b);
      this.doneNamingButton.draw(b);
      this.randomButton.draw(b);
      this.drawMouse(b);
    }

    public delegate void doneNamingBehavior(string s);
  }
}
