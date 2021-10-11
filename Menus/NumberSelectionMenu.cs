// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.NumberSelectionMenu
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace StardewValley.Menus
{
  public class NumberSelectionMenu : IClickableMenu
  {
    public const int region_leftButton = 101;
    public const int region_rightButton = 102;
    public const int region_okButton = 103;
    public const int region_cancelButton = 104;
    private string message;
    private int price;
    private int minValue;
    private int maxValue;
    private int currentValue;
    private int priceShake;
    private int heldTimer;
    private NumberSelectionMenu.behaviorOnNumberSelect behaviorFunction;
    private TextBox numberSelectedBox;
    public ClickableTextureComponent leftButton;
    public ClickableTextureComponent rightButton;
    public ClickableTextureComponent okButton;
    public ClickableTextureComponent cancelButton;

    public NumberSelectionMenu(string message, NumberSelectionMenu.behaviorOnNumberSelect behaviorOnSelection, int price = -1, int minValue = 0, int maxValue = 99, int defaultNumber = 0)
    {
      Vector2 vector2 = Game1.dialogueFont.MeasureString(message);
      int width = Math.Max((int) vector2.X, 600) + IClickableMenu.borderWidth * 2;
      int height = (int) vector2.Y + IClickableMenu.borderWidth * 2 + Game1.tileSize * 5 / 2;
      this.initialize(Game1.viewport.Width / 2 - width / 2, Game1.viewport.Height / 2 - height / 2, width, height, false);
      this.message = message;
      this.price = price;
      this.minValue = minValue;
      this.maxValue = maxValue;
      this.currentValue = defaultNumber;
      this.behaviorFunction = behaviorOnSelection;
      TextBox textBox = new TextBox(Game1.content.Load<Texture2D>("LooseSprites\\textBox"), (Texture2D) null, Game1.smallFont, Game1.textColor);
      textBox.X = this.xPositionOnScreen + IClickableMenu.borderWidth + 14 * Game1.pixelZoom;
      textBox.Y = this.yPositionOnScreen + IClickableMenu.borderWidth + this.height / 2;
      textBox.Text = string.Concat((object) this.currentValue);
      int num1 = 1;
      textBox.numbersOnly = num1 != 0;
      int length = string.Concat((object) maxValue).Length;
      textBox.textLimit = length;
      this.numberSelectedBox = textBox;
      this.numberSelectedBox.SelectMe();
      ClickableTextureComponent textureComponent1 = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.borderWidth, this.yPositionOnScreen + IClickableMenu.borderWidth + this.height / 2, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(352, 495, 12, 11), (float) Game1.pixelZoom, false);
      int num2 = 101;
      textureComponent1.myID = num2;
      int num3 = 102;
      textureComponent1.rightNeighborID = num3;
      this.leftButton = textureComponent1;
      ClickableTextureComponent textureComponent2 = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + IClickableMenu.borderWidth + 16 * Game1.pixelZoom + this.numberSelectedBox.Width, this.yPositionOnScreen + IClickableMenu.borderWidth + this.height / 2, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(365, 495, 12, 11), (float) Game1.pixelZoom, false);
      int num4 = 102;
      textureComponent2.myID = num4;
      int num5 = 101;
      textureComponent2.leftNeighborID = num5;
      int num6 = 103;
      textureComponent2.rightNeighborID = num6;
      this.rightButton = textureComponent2;
      ClickableTextureComponent textureComponent3 = new ClickableTextureComponent("OK", new Rectangle(this.xPositionOnScreen + this.width - IClickableMenu.borderWidth - IClickableMenu.spaceToClearSideBorder - Game1.tileSize * 2, this.yPositionOnScreen + this.height - IClickableMenu.borderWidth - IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 3, Game1.tileSize, Game1.tileSize), (string) null, (string) null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f, false);
      int num7 = 103;
      textureComponent3.myID = num7;
      int num8 = 102;
      textureComponent3.leftNeighborID = num8;
      int num9 = 104;
      textureComponent3.rightNeighborID = num9;
      this.okButton = textureComponent3;
      ClickableTextureComponent textureComponent4 = new ClickableTextureComponent("OK", new Rectangle(this.xPositionOnScreen + this.width - IClickableMenu.borderWidth - IClickableMenu.spaceToClearSideBorder - Game1.tileSize, this.yPositionOnScreen + this.height - IClickableMenu.borderWidth - IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 3, Game1.tileSize, Game1.tileSize), (string) null, (string) null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 47, -1, -1), 1f, false);
      int num10 = 104;
      textureComponent4.myID = num10;
      int num11 = 103;
      textureComponent4.leftNeighborID = num11;
      this.cancelButton = textureComponent4;
      if (!Game1.options.SnappyMenus)
        return;
      this.populateClickableComponentList();
      this.snapToDefaultClickableComponent();
    }

    public override void snapToDefaultClickableComponent()
    {
      this.currentlySnappedComponent = this.getComponentWithID(102);
      this.snapCursorToCurrentSnappedComponent();
    }

    public override void gamePadButtonHeld(Buttons b)
    {
      base.gamePadButtonHeld(b);
      if (b != Buttons.A || this.currentlySnappedComponent == null)
        return;
      this.heldTimer = this.heldTimer + Game1.currentGameTime.ElapsedGameTime.Milliseconds;
      if (this.heldTimer <= 300)
        return;
      if (this.currentlySnappedComponent.myID == 102)
      {
        int num = this.currentValue + 1;
        if (num > this.maxValue || this.price != -1 && num * this.price > Game1.player.Money)
          return;
        this.rightButton.scale = this.rightButton.baseScale;
        this.currentValue = num;
        this.numberSelectedBox.Text = string.Concat((object) this.currentValue);
      }
      else
      {
        if (this.currentlySnappedComponent.myID != 101)
          return;
        int num = this.currentValue - 1;
        if (num < this.minValue)
          return;
        this.leftButton.scale = this.leftButton.baseScale;
        this.currentValue = num;
        this.numberSelectedBox.Text = string.Concat((object) this.currentValue);
      }
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
      if (this.leftButton.containsPoint(x, y))
      {
        int num = this.currentValue - 1;
        if (num >= this.minValue)
        {
          this.leftButton.scale = this.leftButton.baseScale;
          this.currentValue = num;
          this.numberSelectedBox.Text = string.Concat((object) this.currentValue);
          Game1.playSound("smallSelect");
        }
      }
      if (this.rightButton.containsPoint(x, y))
      {
        int num = this.currentValue + 1;
        if (num <= this.maxValue && (this.price == -1 || num * this.price <= Game1.player.Money))
        {
          this.rightButton.scale = this.rightButton.baseScale;
          this.currentValue = num;
          this.numberSelectedBox.Text = string.Concat((object) this.currentValue);
          Game1.playSound("smallSelect");
        }
      }
      if (this.okButton.containsPoint(x, y))
      {
        if (this.currentValue > this.maxValue || this.currentValue < this.minValue)
        {
          this.currentValue = Math.Max(this.minValue, Math.Min(this.maxValue, this.currentValue));
          this.numberSelectedBox.Text = string.Concat((object) this.currentValue);
        }
        else
          this.behaviorFunction(this.currentValue, this.price, Game1.player);
        Game1.playSound("smallSelect");
      }
      if (this.cancelButton.containsPoint(x, y))
      {
        Game1.exitActiveMenu();
        Game1.playSound("bigDeSelect");
        Game1.player.canMove = true;
      }
      this.numberSelectedBox.Update();
    }

    public override void receiveKeyPress(Keys key)
    {
      base.receiveKeyPress(key);
      if (key != Keys.Enter)
        return;
      this.receiveLeftClick(this.okButton.bounds.Center.X, this.okButton.bounds.Center.Y, true);
    }

    public override void update(GameTime time)
    {
      base.update(time);
      this.currentValue = 0;
      if (this.numberSelectedBox.Text != null)
        int.TryParse(this.numberSelectedBox.Text, out this.currentValue);
      if (this.priceShake > 0)
        this.priceShake = this.priceShake - time.ElapsedGameTime.Milliseconds;
      if (!Game1.options.SnappyMenus)
        return;
      GamePadState oldPadState = Game1.oldPadState;
      if (Game1.oldPadState.IsButtonDown(Buttons.A))
        return;
      this.heldTimer = 0;
    }

    public override void performHoverAction(int x, int y)
    {
      if (this.okButton.containsPoint(x, y) && (this.price == -1 || this.currentValue > this.minValue))
        this.okButton.scale = Math.Min(this.okButton.scale + 0.02f, this.okButton.baseScale + 0.2f);
      else
        this.okButton.scale = Math.Max(this.okButton.scale - 0.02f, this.okButton.baseScale);
      if (this.cancelButton.containsPoint(x, y))
        this.cancelButton.scale = Math.Min(this.cancelButton.scale + 0.02f, this.cancelButton.baseScale + 0.2f);
      else
        this.cancelButton.scale = Math.Max(this.cancelButton.scale - 0.02f, this.cancelButton.baseScale);
      if (this.leftButton.containsPoint(x, y))
        this.leftButton.scale = Math.Min(this.leftButton.scale + 0.02f, this.leftButton.baseScale + 0.2f);
      else
        this.leftButton.scale = Math.Max(this.leftButton.scale - 0.02f, this.leftButton.baseScale);
      if (this.rightButton.containsPoint(x, y))
        this.rightButton.scale = Math.Min(this.rightButton.scale + 0.02f, this.rightButton.baseScale + 0.2f);
      else
        this.rightButton.scale = Math.Max(this.rightButton.scale - 0.02f, this.rightButton.baseScale);
    }

    public override void draw(SpriteBatch b)
    {
      b.Draw(Game1.fadeToBlackRect, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), Color.Black * 0.5f);
      Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true, (string) null, false);
      b.DrawString(Game1.dialogueFont, this.message, new Vector2((float) (this.xPositionOnScreen + IClickableMenu.borderWidth), (float) (this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth / 2)), Game1.textColor);
      this.okButton.draw(b);
      this.cancelButton.draw(b);
      this.leftButton.draw(b);
      this.rightButton.draw(b);
      if (this.price != -1)
        b.DrawString(Game1.dialogueFont, Game1.content.LoadString("Strings\\StringsFromCSFiles:LoadGameMenu.cs.11020", (object) (this.price * this.currentValue)), new Vector2((float) (this.rightButton.bounds.Right + Game1.tileSize / 2 + (this.priceShake > 0 ? Game1.random.Next(-1, 2) : 0)), (float) (this.rightButton.bounds.Y + (this.priceShake > 0 ? Game1.random.Next(-1, 2) : 0))), this.currentValue * this.price > Game1.player.Money ? Color.Red : Game1.textColor);
      this.numberSelectedBox.Draw(b);
      this.drawMouse(b);
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
    }

    public delegate void behaviorOnNumberSelect(int number, int price, Farmer who);
  }
}
