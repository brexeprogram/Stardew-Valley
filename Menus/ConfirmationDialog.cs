// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.ConfirmationDialog
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace StardewValley.Menus
{
  public class ConfirmationDialog : IClickableMenu
  {
    private bool active = true;
    public const int region_okButton = 101;
    public const int region_cancelButton = 102;
    private string message;
    public ClickableTextureComponent okButton;
    public ClickableTextureComponent cancelButton;
    private ConfirmationDialog.behavior onConfirm;
    private ConfirmationDialog.behavior onCancel;

    public ConfirmationDialog(string message, ConfirmationDialog.behavior onConfirm, ConfirmationDialog.behavior onCancel = null)
      : base(Game1.viewport.Width / 2 - (int) Game1.dialogueFont.MeasureString(message).X / 2 - IClickableMenu.borderWidth, Game1.viewport.Height / 2 - (int) Game1.dialogueFont.MeasureString(message).Y / 2, (int) Game1.dialogueFont.MeasureString(message).X + IClickableMenu.borderWidth * 2, (int) Game1.dialogueFont.MeasureString(message).Y + IClickableMenu.borderWidth * 2 + Game1.tileSize * 5 / 2, false)
    {
      if (onCancel == null)
        onCancel = new ConfirmationDialog.behavior(this.closeDialog);
      else
        this.onCancel = onCancel;
      this.onConfirm = onConfirm;
      this.message = message;
      ClickableTextureComponent textureComponent1 = new ClickableTextureComponent("OK", new Rectangle(this.xPositionOnScreen + this.width - IClickableMenu.borderWidth - IClickableMenu.spaceToClearSideBorder - Game1.tileSize * 2 - Game1.pixelZoom, this.yPositionOnScreen + this.height - IClickableMenu.borderWidth - IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 3, Game1.tileSize, Game1.tileSize), (string) null, (string) null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f, false);
      int num1 = 101;
      textureComponent1.myID = num1;
      int num2 = 102;
      textureComponent1.rightNeighborID = num2;
      this.okButton = textureComponent1;
      ClickableTextureComponent textureComponent2 = new ClickableTextureComponent("OK", new Rectangle(this.xPositionOnScreen + this.width - IClickableMenu.borderWidth - IClickableMenu.spaceToClearSideBorder - Game1.tileSize, this.yPositionOnScreen + this.height - IClickableMenu.borderWidth - IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 3, Game1.tileSize, Game1.tileSize), (string) null, (string) null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 47, -1, -1), 1f, false);
      int num3 = 102;
      textureComponent2.myID = num3;
      int num4 = 101;
      textureComponent2.leftNeighborID = num4;
      this.cancelButton = textureComponent2;
      if (!Game1.options.SnappyMenus)
        return;
      this.populateClickableComponentList();
      this.snapToDefaultClickableComponent();
    }

    public void closeDialog(Farmer who)
    {
      Game1.exitActiveMenu();
    }

    public override void snapToDefaultClickableComponent()
    {
      this.currentlySnappedComponent = this.getComponentWithID(102);
      this.snapCursorToCurrentSnappedComponent();
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
      if (!this.active)
        return;
      if (this.okButton.containsPoint(x, y))
      {
        if (this.onConfirm != null)
          this.onConfirm(Game1.player);
        Game1.playSound("smallSelect");
        this.active = false;
      }
      if (!this.cancelButton.containsPoint(x, y))
        return;
      if (this.onCancel != null)
        this.onCancel(Game1.player);
      else
        Game1.exitActiveMenu();
      Game1.playSound("bigDeSelect");
    }

    public override void receiveKeyPress(Keys key)
    {
      base.receiveKeyPress(key);
      if (!this.active || Game1.activeClickableMenu != null || this.onCancel == null)
        return;
      this.onCancel(Game1.player);
    }

    public override void update(GameTime time)
    {
      base.update(time);
    }

    public override void performHoverAction(int x, int y)
    {
      if (this.okButton.containsPoint(x, y))
        this.okButton.scale = Math.Min(this.okButton.scale + 0.02f, this.okButton.baseScale + 0.2f);
      else
        this.okButton.scale = Math.Max(this.okButton.scale - 0.02f, this.okButton.baseScale);
      if (this.cancelButton.containsPoint(x, y))
        this.cancelButton.scale = Math.Min(this.cancelButton.scale + 0.02f, this.cancelButton.baseScale + 0.2f);
      else
        this.cancelButton.scale = Math.Max(this.cancelButton.scale - 0.02f, this.cancelButton.baseScale);
    }

    public override void draw(SpriteBatch b)
    {
      if (!this.active)
        return;
      b.Draw(Game1.fadeToBlackRect, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), Color.Black * 0.5f);
      Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true, (string) null, false);
      b.DrawString(Game1.dialogueFont, this.message, new Vector2((float) (this.xPositionOnScreen + IClickableMenu.borderWidth), (float) (this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth / 2)), Game1.textColor);
      this.okButton.draw(b);
      this.cancelButton.draw(b);
      this.drawMouse(b);
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
    }

    public delegate void behavior(Farmer who);
  }
}
