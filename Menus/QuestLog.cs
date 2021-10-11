// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.QuestLog
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Quests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Menus
{
  public class QuestLog : IClickableMenu
  {
    private int questPage = -1;
    private string hoverText = "";
    public const int questsPerPage = 6;
    public const int region_forwardButton = 101;
    public const int region_backButton = 102;
    public const int region_rewardBox = 103;
    public const int region_cancelQuestButton = 104;
    private List<List<Quest>> pages;
    public List<ClickableComponent> questLogButtons;
    private int currentPage;
    public ClickableTextureComponent forwardButton;
    public ClickableTextureComponent backButton;
    public ClickableTextureComponent rewardBox;
    public ClickableTextureComponent cancelQuestButton;

    public QuestLog()
      : base(0, 0, 0, 0, true)
    {
      Game1.playSound("bigSelect");
      this.paginateQuests();
      this.width = Game1.tileSize * 13;
      this.height = Game1.tileSize * 9;
      Vector2 centeringOnScreen = Utility.getTopLeftPositionForCenteringOnScreen(this.width, this.height, 0, 0);
      this.xPositionOnScreen = (int) centeringOnScreen.X;
      this.yPositionOnScreen = (int) centeringOnScreen.Y + Game1.tileSize / 2;
      this.questLogButtons = new List<ClickableComponent>();
      for (int index = 0; index < 6; ++index)
      {
        List<ClickableComponent> questLogButtons = this.questLogButtons;
        ClickableComponent clickableComponent = new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize / 4 + index * ((this.height - Game1.tileSize / 2) / 6), this.width - Game1.tileSize / 2, (this.height - Game1.tileSize / 2) / 6 + Game1.pixelZoom), string.Concat((object) index));
        clickableComponent.myID = index;
        clickableComponent.downNeighborID = -7777;
        int num1 = index > 0 ? index - 1 : -1;
        clickableComponent.upNeighborID = num1;
        int num2 = -7777;
        clickableComponent.rightNeighborID = num2;
        int num3 = -7777;
        clickableComponent.leftNeighborID = num3;
        int num4 = 1;
        clickableComponent.fullyImmutable = num4 != 0;
        questLogButtons.Add(clickableComponent);
      }
      this.upperRightCloseButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width - 5 * Game1.pixelZoom, this.yPositionOnScreen - 2 * Game1.pixelZoom, 12 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(337, 494, 12, 12), (float) Game1.pixelZoom, false);
      ClickableTextureComponent textureComponent1 = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen - Game1.tileSize, this.yPositionOnScreen + Game1.pixelZoom * 2, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(352, 495, 12, 11), (float) Game1.pixelZoom, false);
      int num5 = 102;
      textureComponent1.myID = num5;
      int num6 = -7777;
      textureComponent1.rightNeighborID = num6;
      this.backButton = textureComponent1;
      ClickableTextureComponent textureComponent2 = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize - 12 * Game1.pixelZoom, this.yPositionOnScreen + this.height - 12 * Game1.pixelZoom, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(365, 495, 12, 11), (float) Game1.pixelZoom, false);
      int num7 = 101;
      textureComponent2.myID = num7;
      this.forwardButton = textureComponent2;
      ClickableTextureComponent textureComponent3 = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width / 2 - Game1.pixelZoom * 20, this.yPositionOnScreen + this.height - Game1.tileSize / 2 - 24 * Game1.pixelZoom, 24 * Game1.pixelZoom, 24 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(293, 360, 24, 24), (float) Game1.pixelZoom, true);
      int num8 = 103;
      textureComponent3.myID = num8;
      this.rewardBox = textureComponent3;
      ClickableTextureComponent textureComponent4 = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + Game1.pixelZoom, this.yPositionOnScreen + this.height + Game1.pixelZoom, 12 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(322, 498, 12, 12), (float) Game1.pixelZoom, true);
      int num9 = 104;
      textureComponent4.myID = num9;
      this.cancelQuestButton = textureComponent4;
      if (!Game1.options.SnappyMenus)
        return;
      this.populateClickableComponentList();
      this.snapToDefaultClickableComponent();
    }

    protected override void customSnapBehavior(int direction, int oldRegion, int oldID)
    {
      if (oldID >= 0 && oldID < 6 && this.questPage == -1)
      {
        if (direction == 2)
        {
          if (oldID < 5 && this.pages[this.currentPage].Count - 1 > oldID)
            this.currentlySnappedComponent = this.getComponentWithID(oldID + 1);
        }
        else if (direction == 1)
        {
          if (this.currentPage < this.pages.Count - 1)
          {
            this.currentlySnappedComponent = this.getComponentWithID(101);
            this.currentlySnappedComponent.leftNeighborID = oldID;
          }
        }
        else if (direction == 3 && this.currentPage > 0)
        {
          this.currentlySnappedComponent = this.getComponentWithID(102);
          this.currentlySnappedComponent.rightNeighborID = oldID;
        }
      }
      else if (oldID == 102)
      {
        if (this.questPage != -1)
          return;
        this.currentlySnappedComponent = this.getComponentWithID(0);
      }
      this.snapCursorToCurrentSnappedComponent();
    }

    public override void snapToDefaultClickableComponent()
    {
      this.currentlySnappedComponent = this.getComponentWithID(0);
      this.snapCursorToCurrentSnappedComponent();
    }

    public override void receiveGamePadButton(Buttons b)
    {
      if (b == Buttons.RightTrigger && this.questPage == -1 && this.currentPage < this.pages.Count - 1)
      {
        this.nonQuestPageForwardButton();
      }
      else
      {
        if (b != Buttons.LeftTrigger || this.questPage != -1 || this.currentPage <= 0)
          return;
        this.nonQuestPageBackButton();
      }
    }

    private void paginateQuests()
    {
      this.pages = new List<List<Quest>>();
      for (int index = Game1.player.questLog.Count - 1; index >= 0; --index)
      {
        if (Game1.player.questLog[index] == null || Game1.player.questLog[index].destroy)
        {
          Game1.player.questLog.RemoveAt(index);
        }
        else
        {
          int num = Game1.player.questLog.Count - 1 - index;
          if (this.pages.Count <= num / 6)
            this.pages.Add(new List<Quest>());
          this.pages[num / 6].Add(Game1.player.questLog[index]);
        }
      }
      this.currentPage = Math.Min(Math.Max(this.currentPage, 0), this.pages.Count - 1);
      this.questPage = -1;
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
    }

    public override void performHoverAction(int x, int y)
    {
      this.hoverText = "";
      base.performHoverAction(x, y);
      if (this.questPage == -1)
      {
        for (int index = 0; index < this.questLogButtons.Count; ++index)
        {
          if (this.pages.Count > 0 && this.pages[0].Count > index && (this.questLogButtons[index].containsPoint(x, y) && !this.questLogButtons[index].containsPoint(Game1.getOldMouseX(), Game1.getOldMouseY())))
            Game1.playSound("Cowboy_gunshot");
        }
      }
      else if (this.pages[this.currentPage][this.questPage].canBeCancelled && this.cancelQuestButton.containsPoint(x, y))
        this.hoverText = Game1.content.LoadString("Strings\\StringsFromCSFiles:QuestLog.cs.11364");
      this.forwardButton.tryHover(x, y, 0.2f);
      this.backButton.tryHover(x, y, 0.2f);
      this.cancelQuestButton.tryHover(x, y, 0.2f);
    }

    public override void receiveKeyPress(Keys key)
    {
      base.receiveKeyPress(key);
      if (!Game1.options.doesInputListContain(Game1.options.journalButton, key) || !this.readyToClose())
        return;
      Game1.exitActiveMenu();
      Game1.playSound("bigDeSelect");
    }

    private void nonQuestPageForwardButton()
    {
      this.currentPage = this.currentPage + 1;
      Game1.playSound("shwip");
      if (!Game1.options.SnappyMenus || this.currentPage != this.pages.Count - 1)
        return;
      this.currentlySnappedComponent = this.getComponentWithID(0);
      this.snapCursorToCurrentSnappedComponent();
    }

    private void nonQuestPageBackButton()
    {
      this.currentPage = this.currentPage - 1;
      Game1.playSound("shwip");
      if (!Game1.options.SnappyMenus || this.currentPage != 0)
        return;
      this.currentlySnappedComponent = this.getComponentWithID(0);
      this.snapCursorToCurrentSnappedComponent();
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
      base.receiveLeftClick(x, y, playSound);
      if (Game1.activeClickableMenu == null)
        return;
      if (this.questPage == -1)
      {
        for (int index = 0; index < this.questLogButtons.Count; ++index)
        {
          if (this.pages.Count > 0 && this.pages[this.currentPage].Count > index && this.questLogButtons[index].containsPoint(x, y))
          {
            Game1.playSound("smallSelect");
            this.questPage = index;
            this.pages[this.currentPage][index].showNew = false;
            if (!Game1.options.SnappyMenus)
              return;
            this.currentlySnappedComponent = this.getComponentWithID(102);
            this.currentlySnappedComponent.downNeighborID = !this.pages[this.currentPage][this.questPage].completed || this.pages[this.currentPage][this.questPage].moneyReward <= 0 ? (this.pages[this.currentPage][this.questPage].canBeCancelled ? 104 : -1) : 103;
            this.snapCursorToCurrentSnappedComponent();
            return;
          }
        }
        if (this.currentPage < this.pages.Count - 1 && this.forwardButton.containsPoint(x, y))
          this.nonQuestPageForwardButton();
        else if (this.currentPage > 0 && this.backButton.containsPoint(x, y))
        {
          this.nonQuestPageBackButton();
        }
        else
        {
          Game1.playSound("bigDeSelect");
          this.exitThisMenu(true);
        }
      }
      else if (this.questPage != -1 && this.pages[this.currentPage][this.questPage].completed && (this.pages[this.currentPage][this.questPage].moneyReward > 0 && this.rewardBox.containsPoint(x, y)))
      {
        Game1.player.Money += this.pages[this.currentPage][this.questPage].moneyReward;
        Game1.playSound("purchaseRepeat");
        this.pages[this.currentPage][this.questPage].moneyReward = 0;
        this.pages[this.currentPage][this.questPage].destroy = true;
      }
      else if (this.questPage != -1 && !this.pages[this.currentPage][this.questPage].completed && (this.pages[this.currentPage][this.questPage].canBeCancelled && this.cancelQuestButton.containsPoint(x, y)))
      {
        this.pages[this.currentPage][this.questPage].accepted = false;
        Game1.player.questLog.Remove(this.pages[this.currentPage][this.questPage]);
        this.pages[this.currentPage].RemoveAt(this.questPage);
        this.questPage = -1;
        Game1.playSound("trashcan");
        if (!Game1.options.SnappyMenus || this.currentPage != 0)
          return;
        this.currentlySnappedComponent = this.getComponentWithID(0);
        this.snapCursorToCurrentSnappedComponent();
      }
      else
        this.exitQuestPage();
    }

    public void exitQuestPage()
    {
      if (this.pages[this.currentPage][this.questPage].completed && this.pages[this.currentPage][this.questPage].moneyReward <= 0)
        this.pages[this.currentPage][this.questPage].destroy = true;
      if (this.pages[this.currentPage][this.questPage].destroy)
      {
        Game1.player.questLog.Remove(this.pages[this.currentPage][this.questPage]);
        this.pages[this.currentPage].RemoveAt(this.questPage);
      }
      this.questPage = -1;
      this.paginateQuests();
      Game1.playSound("shwip");
      if (!Game1.options.SnappyMenus)
        return;
      this.snapToDefaultClickableComponent();
    }

    public override void update(GameTime time)
    {
      base.update(time);
      if (this.questPage == -1 || !this.pages[this.currentPage][this.questPage].hasReward())
        return;
      this.rewardBox.scale = this.rewardBox.baseScale + Game1.dialogueButtonScale / 20f;
    }

    public override void draw(SpriteBatch b)
    {
      b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.75f);
      SpriteText.drawStringWithScrollCenteredAt(b, Game1.content.LoadString("Strings\\StringsFromCSFiles:QuestLog.cs.11373"), this.xPositionOnScreen + this.width / 2, this.yPositionOnScreen - Game1.tileSize, "", 1f, -1, 0, 0.88f, false);
      IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(384, 373, 18, 18), this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, Color.White, (float) Game1.pixelZoom, true);
      if (this.questPage == -1)
      {
        for (int index = 0; index < this.questLogButtons.Count; ++index)
        {
          if (this.pages.Count<List<Quest>>() > 0 && this.pages[this.currentPage].Count<Quest>() > index)
          {
            IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(384, 396, 15, 15), this.questLogButtons[index].bounds.X, this.questLogButtons[index].bounds.Y, this.questLogButtons[index].bounds.Width, this.questLogButtons[index].bounds.Height, this.questLogButtons[index].containsPoint(Game1.getOldMouseX(), Game1.getOldMouseY()) ? Color.Wheat : Color.White, (float) Game1.pixelZoom, false);
            if (this.pages[this.currentPage][index].showNew || this.pages[this.currentPage][index].completed)
              Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float) (this.questLogButtons[index].bounds.X + Game1.tileSize + Game1.pixelZoom), (float) (this.questLogButtons[index].bounds.Y + Game1.pixelZoom * 11)), new Rectangle(this.pages[this.currentPage][index].completed ? 341 : 317, 410, 23, 9), Color.White, 0.0f, new Vector2(11f, 4f), (float) Game1.pixelZoom + (float) ((double) Game1.dialogueButtonScale * 10.0 / 250.0), false, 0.99f, -1, -1, 0.35f);
            else
              Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float) (this.questLogButtons[index].bounds.X + Game1.tileSize / 2), (float) (this.questLogButtons[index].bounds.Y + Game1.pixelZoom * 7)), this.pages[this.currentPage][index].dailyQuest ? new Rectangle(410, 501, 9, 9) : new Rectangle(395 + (this.pages[this.currentPage][index].dailyQuest ? 3 : 0), 497, 3, 8), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, false, 0.99f, -1, -1, 0.35f);
            int num = this.pages[this.currentPage][index].dailyQuest ? 1 : 0;
            SpriteText.drawString(b, this.pages[this.currentPage][index].questTitle, this.questLogButtons[index].bounds.X + Game1.tileSize * 2 + Game1.pixelZoom, this.questLogButtons[index].bounds.Y + Game1.pixelZoom * 5, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
          }
        }
      }
      else
      {
        SpriteText.drawStringHorizontallyCenteredAt(b, this.pages[this.currentPage][this.questPage].questTitle, this.xPositionOnScreen + this.width / 2 + (!this.pages[this.currentPage][this.questPage].dailyQuest || this.pages[this.currentPage][this.questPage].daysLeft <= 0 ? 0 : Math.Max(8 * Game1.pixelZoom, SpriteText.getWidthOfString(this.pages[this.currentPage][this.questPage].questTitle) / 3) - 8 * Game1.pixelZoom), this.yPositionOnScreen + Game1.tileSize / 2, 999999, -1, 999999, 1f, 0.88f, false, -1);
        if (this.pages[this.currentPage][this.questPage].dailyQuest && this.pages[this.currentPage][this.questPage].daysLeft > 0)
        {
          Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float) (this.xPositionOnScreen + Game1.pixelZoom * 8), (float) (this.yPositionOnScreen + Game1.tileSize * 3 / 4 - Game1.pixelZoom * 2)), new Rectangle(410, 501, 9, 9), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, false, 0.99f, -1, -1, 0.35f);
          SpriteBatch b1 = b;
          string text1;
          if (this.pages[this.currentPage][this.questPage].daysLeft <= 1)
            text1 = Game1.content.LoadString("Strings\\StringsFromCSFiles:QuestLog.cs.11375", (object) this.pages[this.currentPage][this.questPage].daysLeft);
          else
            text1 = Game1.content.LoadString("Strings\\StringsFromCSFiles:QuestLog.cs.11374", (object) this.pages[this.currentPage][this.questPage].daysLeft);
          SpriteFont dialogueFont1 = Game1.dialogueFont;
          int width = this.width - Game1.tileSize * 2;
          string text2 = Game1.parseText(text1, dialogueFont1, width);
          SpriteFont dialogueFont2 = Game1.dialogueFont;
          Vector2 position = new Vector2((float) (this.xPositionOnScreen + 20 * Game1.pixelZoom), (float) (this.yPositionOnScreen + Game1.tileSize * 3 / 4 - Game1.pixelZoom * 2));
          Color textColor = Game1.textColor;
          double num1 = 1.0;
          double num2 = -1.0;
          int horizontalShadowOffset = -1;
          int verticalShadowOffset = -1;
          double num3 = 1.0;
          int numShadows = 3;
          Utility.drawTextWithShadow(b1, text2, dialogueFont2, position, textColor, (float) num1, (float) num2, horizontalShadowOffset, verticalShadowOffset, (float) num3, numShadows);
        }
        Utility.drawTextWithShadow(b, Game1.parseText(this.pages[this.currentPage][this.questPage].questDescription, Game1.dialogueFont, this.width - Game1.tileSize * 2), Game1.dialogueFont, new Vector2((float) (this.xPositionOnScreen + Game1.tileSize), (float) (this.yPositionOnScreen + Game1.tileSize * 3 / 2)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
        float y = (float) (this.yPositionOnScreen + Game1.tileSize * 3 / 2) + Game1.dialogueFont.MeasureString(Game1.parseText(this.pages[this.currentPage][this.questPage].questDescription, Game1.dialogueFont, this.width - Game1.tileSize * 2)).Y + (float) (Game1.tileSize / 2);
        if (this.pages[this.currentPage][this.questPage].completed)
        {
          SpriteText.drawString(b, Game1.content.LoadString("Strings\\StringsFromCSFiles:QuestLog.cs.11376"), this.xPositionOnScreen + Game1.tileSize / 2 + Game1.pixelZoom, this.rewardBox.bounds.Y + Game1.tileSize / 3 + Game1.pixelZoom, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
          this.rewardBox.draw(b);
          if (this.pages[this.currentPage][this.questPage].moneyReward > 0)
          {
            b.Draw(Game1.mouseCursors, new Vector2((float) (this.rewardBox.bounds.X + Game1.pixelZoom * 4), (float) (this.rewardBox.bounds.Y + Game1.pixelZoom * 4) - Game1.dialogueButtonScale / 2f), new Rectangle?(new Rectangle(280, 410, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1f);
            SpriteText.drawString(b, Game1.content.LoadString("Strings\\StringsFromCSFiles:LoadGameMenu.cs.11020", (object) this.pages[this.currentPage][this.questPage].moneyReward), this.xPositionOnScreen + Game1.tileSize * 7, this.rewardBox.bounds.Y + Game1.tileSize / 3 + Game1.pixelZoom, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
          }
        }
        else
        {
          Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float) (this.xPositionOnScreen + Game1.tileSize * 3 / 2) + (float) ((double) (Game1.pixelZoom * 2) * (double) Game1.dialogueButtonScale / 10.0), y), new Rectangle(412, 495, 5, 4), Color.White, 1.570796f, Vector2.Zero, -1f, false, -1f, -1, -1, 0.35f);
          Utility.drawTextWithShadow(b, Game1.parseText(this.pages[this.currentPage][this.questPage].currentObjective, Game1.dialogueFont, this.width - Game1.tileSize * 4), Game1.dialogueFont, new Vector2((float) (this.xPositionOnScreen + Game1.tileSize * 8 / 4), y - (float) (Game1.pixelZoom * 2)), Color.DarkBlue, 1f, -1f, -1, -1, 1f, 3);
          if (this.pages[this.currentPage][this.questPage].canBeCancelled)
            this.cancelQuestButton.draw(b);
        }
      }
      if (this.currentPage < this.pages.Count - 1 && this.questPage == -1)
        this.forwardButton.draw(b);
      if (this.currentPage > 0 || this.questPage != -1)
        this.backButton.draw(b);
      base.draw(b);
      Game1.mouseCursorTransparency = 1f;
      this.drawMouse(b);
      if (this.hoverText.Length <= 0)
        return;
      IClickableMenu.drawHoverText(b, this.hoverText, Game1.dialogueFont, 0, 0, -1, (string) null, -1, (string[]) null, (Item) null, 0, -1, -1, -1, -1, 1f, (CraftingRecipe) null);
    }
  }
}
