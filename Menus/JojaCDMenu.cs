// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.JojaCDMenu
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.Locations;
using System;
using System.Collections.Generic;

namespace StardewValley.Menus
{
  public class JojaCDMenu : IClickableMenu
  {
    public List<ClickableComponent> checkboxes = new List<ClickableComponent>();
    private int exitTimer = -1;
    public new const int width = 1280;
    public new const int height = 576;
    public const int buttonWidth = 147;
    public const int buttonHeight = 30;
    private Texture2D noteTexture;
    private string hoverText;
    private bool boughtSomething;

    public JojaCDMenu(Texture2D noteTexture)
      : base(Game1.viewport.Width / 2 - 640, Game1.viewport.Height / 2 - 288, 1280, 576, true)
    {
      Game1.player.forceCanMove();
      this.noteTexture = noteTexture;
      int x = this.xPositionOnScreen + Game1.pixelZoom;
      int y = this.yPositionOnScreen + 52 * Game1.pixelZoom;
      for (int index = 0; index < 5; ++index)
      {
        List<ClickableComponent> checkboxes = this.checkboxes;
        ClickableComponent clickableComponent = new ClickableComponent(new Rectangle(x, y, 147 * Game1.pixelZoom, 30 * Game1.pixelZoom), string.Concat((object) index));
        clickableComponent.myID = index;
        int num1 = index % 2 != 0 || index == 4 ? -1 : index + 1;
        clickableComponent.rightNeighborID = num1;
        int num2 = index % 2 == 0 ? -1 : index - 1;
        clickableComponent.leftNeighborID = num2;
        int num3 = index + 2;
        clickableComponent.downNeighborID = num3;
        int num4 = index - 2;
        clickableComponent.upNeighborID = num4;
        checkboxes.Add(clickableComponent);
        x += 148 * Game1.pixelZoom;
        if (x > this.xPositionOnScreen + 148 * Game1.pixelZoom * 2)
        {
          x = this.xPositionOnScreen + Game1.pixelZoom;
          y += 30 * Game1.pixelZoom;
        }
      }
      if (Game1.player.hasOrWillReceiveMail("ccVault"))
        this.checkboxes[0].name = "complete";
      if (Game1.player.hasOrWillReceiveMail("ccBoilerRoom"))
        this.checkboxes[1].name = "complete";
      if (Game1.player.hasOrWillReceiveMail("ccCraftsRoom"))
        this.checkboxes[2].name = "complete";
      if (Game1.player.hasOrWillReceiveMail("ccPantry"))
        this.checkboxes[3].name = "complete";
      if (Game1.player.hasOrWillReceiveMail("ccFishTank"))
        this.checkboxes[4].name = "complete";
      this.exitFunction = new IClickableMenu.onExit(this.onExitFunction);
      if (!Game1.options.SnappyMenus)
        return;
      this.populateClickableComponentList();
      this.snapToDefaultClickableComponent();
      Game1.mouseCursorTransparency = 1f;
    }

    public override void snapToDefaultClickableComponent()
    {
      this.currentlySnappedComponent = this.getComponentWithID(0);
      this.snapCursorToCurrentSnappedComponent();
    }

    private void onExitFunction()
    {
      if (!this.boughtSomething)
        return;
      JojaMart.Morris.setNewDialogue(Game1.content.LoadString("Data\\ExtraDialogue:Morris_JojaCDConfirm"), false, false);
      Game1.drawDialogue(JojaMart.Morris);
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
      if (this.exitTimer >= 0)
        return;
      base.receiveLeftClick(x, y, true);
      foreach (ClickableComponent checkbox in this.checkboxes)
      {
        if (checkbox.containsPoint(x, y) && !checkbox.name.Equals("complete"))
        {
          int int32 = Convert.ToInt32(checkbox.name);
          int fromButtonNumber = this.getPriceFromButtonNumber(int32);
          if (Game1.player.money >= fromButtonNumber)
          {
            Game1.player.money -= fromButtonNumber;
            Game1.playSound("reward");
            checkbox.name = "complete";
            this.boughtSomething = true;
            switch (int32)
            {
              case 0:
                Game1.addMailForTomorrow("jojaVault", true, true);
                Game1.addMailForTomorrow("ccVault", true, true);
                break;
              case 1:
                Game1.addMailForTomorrow("jojaBoilerRoom", true, true);
                Game1.addMailForTomorrow("ccBoilerRoom", true, true);
                break;
              case 2:
                Game1.addMailForTomorrow("jojaCraftsRoom", true, true);
                Game1.addMailForTomorrow("ccCraftsRoom", true, true);
                break;
              case 3:
                Game1.addMailForTomorrow("jojaPantry", true, true);
                Game1.addMailForTomorrow("ccPantry", true, true);
                break;
              case 4:
                Game1.addMailForTomorrow("jojaFishTank", true, true);
                Game1.addMailForTomorrow("ccFishTank", true, true);
                break;
            }
            this.exitTimer = 1000;
          }
          else
            Game1.dayTimeMoneyBox.moneyShakeTimer = 1000;
        }
      }
    }

    public override bool readyToClose()
    {
      return true;
    }

    public override void update(GameTime time)
    {
      base.update(time);
      if (this.exitTimer >= 0)
      {
        this.exitTimer = this.exitTimer - time.ElapsedGameTime.Milliseconds;
        if (this.exitTimer <= 0)
          this.exitThisMenu(true);
      }
      Game1.mouseCursorTransparency = 1f;
    }

    public int getPriceFromButtonNumber(int buttonNumber)
    {
      switch (buttonNumber)
      {
        case 0:
          return 40000;
        case 1:
          return 15000;
        case 2:
          return 25000;
        case 3:
          return 35000;
        case 4:
          return 20000;
        default:
          return -1;
      }
    }

    public string getDescriptionFromButtonNumber(int buttonNumber)
    {
      return Game1.content.LoadString("Strings\\UI:JojaCDMenu_Hover" + (object) buttonNumber);
    }

    public override void performHoverAction(int x, int y)
    {
      base.performHoverAction(x, y);
      this.hoverText = "";
      foreach (ClickableComponent checkbox in this.checkboxes)
      {
        if (checkbox.containsPoint(x, y))
          this.hoverText = checkbox.name.Equals("complete") ? "" : Game1.parseText(this.getDescriptionFromButtonNumber(Convert.ToInt32(checkbox.name)), Game1.dialogueFont, Game1.tileSize * 6);
      }
    }

    public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
    {
      base.gameWindowSizeChanged(oldBounds, newBounds);
      this.xPositionOnScreen = Game1.viewport.Width / 2 - 640;
      this.yPositionOnScreen = Game1.viewport.Height / 2 - 288;
      int x = this.xPositionOnScreen + Game1.pixelZoom;
      int y = this.yPositionOnScreen + 52 * Game1.pixelZoom;
      this.checkboxes.Clear();
      for (int index = 0; index < 5; ++index)
      {
        this.checkboxes.Add(new ClickableComponent(new Rectangle(x, y, 147 * Game1.pixelZoom, 30 * Game1.pixelZoom), string.Concat((object) index)));
        x += 148 * Game1.pixelZoom;
        if (x > this.xPositionOnScreen + 148 * Game1.pixelZoom * 2)
        {
          x = this.xPositionOnScreen + Game1.pixelZoom;
          y += 30 * Game1.pixelZoom;
        }
      }
    }

    public override void receiveKeyPress(Keys key)
    {
      base.receiveKeyPress(key);
    }

    public override void draw(SpriteBatch b)
    {
      b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.75f);
      b.Draw(this.noteTexture, Utility.getTopLeftPositionForCenteringOnScreen(1280, 576, 0, 0), new Rectangle?(new Rectangle(0, 0, 320, 144)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.79f);
      base.draw(b);
      foreach (ClickableComponent checkbox in this.checkboxes)
      {
        if (checkbox.name.Equals("complete"))
          b.Draw(this.noteTexture, new Vector2((float) (checkbox.bounds.Left + 4 * Game1.pixelZoom), (float) (checkbox.bounds.Y + 4 * Game1.pixelZoom)), new Rectangle?(new Rectangle(0, 144, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.8f);
      }
      Game1.dayTimeMoneyBox.drawMoneyBox(b, Game1.viewport.Width - 300 - IClickableMenu.spaceToClearSideBorder * 2, Game1.pixelZoom);
      Game1.mouseCursorTransparency = 1f;
      this.drawMouse(b);
      if (this.hoverText == null || this.hoverText.Equals(""))
        return;
      IClickableMenu.drawHoverText(b, this.hoverText, Game1.dialogueFont, 0, 0, -1, (string) null, -1, (string[]) null, (Item) null, 0, -1, -1, -1, -1, 1f, (CraftingRecipe) null);
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
    }
  }
}
