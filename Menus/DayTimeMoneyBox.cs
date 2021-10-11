// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.DayTimeMoneyBox
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using System;

namespace StardewValley.Menus
{
  public class DayTimeMoneyBox : IClickableMenu
  {
    public MoneyDial moneyDial = new MoneyDial(8, true);
    private string hoverText = "";
    public new const int width = 300;
    public new const int height = 284;
    public Vector2 position;
    private Rectangle sourceRect;
    public int timeShakeTimer;
    public int moneyShakeTimer;
    public int questPulseTimer;
    public int whenToPulseTimer;
    public ClickableTextureComponent questButton;
    public ClickableTextureComponent zoomOutButton;
    public ClickableTextureComponent zoomInButton;

    public DayTimeMoneyBox()
      : base(Game1.viewport.Width - 300 + Game1.tileSize / 2, Game1.tileSize / 8, 300, 284, false)
    {
      this.position = new Vector2((float) this.xPositionOnScreen, (float) this.yPositionOnScreen);
      this.sourceRect = new Rectangle(333, 431, 71, 43);
      this.questButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + 220, this.yPositionOnScreen + 240, 44, 46), Game1.mouseCursors, new Rectangle(383, 493, 11, 14), 4f, false);
      this.zoomOutButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + Game1.pixelZoom * 23, this.yPositionOnScreen + 244, 7 * Game1.pixelZoom, 8 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(177, 345, 7, 8), 4f, false);
      this.zoomInButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + Game1.pixelZoom * 31, this.yPositionOnScreen + 244, 7 * Game1.pixelZoom, 8 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(184, 345, 7, 8), 4f, false);
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
      this.updatePosition();
      if (Game1.player.questLog.Count > 0 && this.questButton.containsPoint(x, y))
        Game1.activeClickableMenu = (IClickableMenu) new QuestLog();
      if (!Game1.options.zoomButtons)
        return;
      if (this.zoomInButton.containsPoint(x, y) && (double) Game1.options.zoomLevel < 1.25)
      {
        int num1 = (int) Math.Round((double) Game1.options.zoomLevel * 100.0);
        int num2 = num1 - num1 % 5 + 5;
        Game1.options.zoomLevel = Math.Min(1.25f, (float) num2 / 100f);
        Program.gamePtr.refreshWindowSettings();
        Game1.playSound("drumkit6");
        Game1.setMousePosition(this.zoomInButton.bounds.Center);
      }
      else
      {
        if (!this.zoomOutButton.containsPoint(x, y) || (double) Game1.options.zoomLevel <= 0.75)
          return;
        int num1 = (int) Math.Round((double) Game1.options.zoomLevel * 100.0);
        int num2 = num1 - num1 % 5 - 5;
        Game1.options.zoomLevel = Math.Max(0.75f, (float) num2 / 100f);
        Program.gamePtr.refreshWindowSettings();
        Game1.playSound("drumkit6");
        Game1.setMousePosition(this.zoomOutButton.bounds.Center);
      }
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
      this.updatePosition();
    }

    public void questIconPulse()
    {
      this.questPulseTimer = 2000;
    }

    public override void performHoverAction(int x, int y)
    {
      this.updatePosition();
      this.hoverText = "";
      if (Game1.player.questLog.Count > 0 && this.questButton.containsPoint(x, y))
        this.hoverText = Game1.content.LoadString("Strings\\UI:QuestButton_Hover", (object) Game1.options.journalButton[0].ToString());
      if (!Game1.options.zoomButtons)
        return;
      if (this.zoomInButton.containsPoint(x, y))
        this.hoverText = Game1.content.LoadString("Strings\\UI:ZoomInButton_Hover");
      if (!this.zoomOutButton.containsPoint(x, y))
        return;
      this.hoverText = Game1.content.LoadString("Strings\\UI:ZoomOutButton_Hover");
    }

    public void drawMoneyBox(SpriteBatch b, int overrideX = -1, int overrideY = -1)
    {
      this.updatePosition();
      b.Draw(Game1.mouseCursors, (overrideY != -1 ? new Vector2(overrideX == -1 ? this.position.X : (float) overrideX, (float) (overrideY - 172)) : this.position) + new Vector2((float) (28 + (this.moneyShakeTimer > 0 ? Game1.random.Next(-3, 4) : 0)), (float) (172 + (this.moneyShakeTimer > 0 ? Game1.random.Next(-3, 4) : 0))), new Rectangle?(new Rectangle(340, 472, 65, 17)), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 0.9f);
      this.moneyDial.draw(b, (overrideY != -1 ? new Vector2(overrideX == -1 ? this.position.X : (float) overrideX, (float) (overrideY - 172)) : this.position) + new Vector2((float) (68 + (this.moneyShakeTimer > 0 ? Game1.random.Next(-3, 4) : 0)), (float) (196 + (this.moneyShakeTimer > 0 ? Game1.random.Next(-3, 4) : 0))), Game1.player.money);
      if (this.moneyShakeTimer <= 0)
        return;
      this.moneyShakeTimer = this.moneyShakeTimer - Game1.currentGameTime.ElapsedGameTime.Milliseconds;
    }

    public override void draw(SpriteBatch b)
    {
      this.updatePosition();
      TimeSpan timeSpan;
      if (this.timeShakeTimer > 0)
      {
        int timeShakeTimer = this.timeShakeTimer;
        timeSpan = Game1.currentGameTime.ElapsedGameTime;
        int milliseconds = timeSpan.Milliseconds;
        this.timeShakeTimer = timeShakeTimer - milliseconds;
      }
      if (this.questPulseTimer > 0)
      {
        int questPulseTimer = this.questPulseTimer;
        timeSpan = Game1.currentGameTime.ElapsedGameTime;
        int milliseconds = timeSpan.Milliseconds;
        this.questPulseTimer = questPulseTimer - milliseconds;
      }
      if (this.whenToPulseTimer >= 0)
      {
        int whenToPulseTimer = this.whenToPulseTimer;
        timeSpan = Game1.currentGameTime.ElapsedGameTime;
        int milliseconds = timeSpan.Milliseconds;
        this.whenToPulseTimer = whenToPulseTimer - milliseconds;
        if (this.whenToPulseTimer <= 0)
        {
          this.whenToPulseTimer = 3000;
          if (Game1.player.hasNewQuestActivity())
            this.questPulseTimer = 1000;
        }
      }
      b.Draw(Game1.mouseCursors, this.position, new Rectangle?(this.sourceRect), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 0.9f);
      string str1;
      if (LocalizedContentManager.CurrentLanguageCode != LocalizedContentManager.LanguageCode.ja)
      {
        if (LocalizedContentManager.CurrentLanguageCode != LocalizedContentManager.LanguageCode.zh)
          str1 = Game1.shortDayDisplayNameFromDayOfSeason(Game1.dayOfMonth) + ". " + (object) Game1.dayOfMonth;
        else
          str1 = Game1.shortDayDisplayNameFromDayOfSeason(Game1.dayOfMonth) + " " + (object) Game1.dayOfMonth + "日";
      }
      else
        str1 = Game1.dayOfMonth.ToString() + "日 (" + Game1.shortDayDisplayNameFromDayOfSeason(Game1.dayOfMonth) + ")";
      string text1 = str1;
      Vector2 vector2_1 = Game1.dialogueFont.MeasureString(text1);
      Vector2 vector2_2 = new Vector2((float) ((double) this.sourceRect.X * 0.550000011920929 - (double) vector2_1.X / 2.0), (float) ((double) this.sourceRect.Y * (LocalizedContentManager.CurrentLanguageLatin ? 0.100000001490116 : 0.100000001490116) - (double) vector2_1.Y / 2.0));
      Utility.drawTextWithShadow(b, text1, Game1.dialogueFont, this.position + vector2_2, Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
      b.Draw(Game1.mouseCursors, this.position + new Vector2(212f, 68f), new Rectangle?(new Rectangle(406, 441 + Utility.getSeasonNumber(Game1.currentSeason) * 8, 12, 8)), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 0.9f);
      b.Draw(Game1.mouseCursors, this.position + new Vector2(116f, 68f), new Rectangle?(new Rectangle(317 + 12 * Game1.weatherIcon, 421, 12, 8)), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 0.9f);
      string str2 = Game1.timeOfDay % 100 == 0 ? "0" : "";
      string str3 = Game1.timeOfDay / 100 % 12 == 0 ? "12" : string.Concat((object) (Game1.timeOfDay / 100 % 12));
      switch (LocalizedContentManager.CurrentLanguageCode)
      {
        case LocalizedContentManager.LanguageCode.en:
        case LocalizedContentManager.LanguageCode.ja:
          str3 = Game1.timeOfDay / 100 % 12 == 0 ? "12" : string.Concat((object) (Game1.timeOfDay / 100 % 12));
          break;
        case LocalizedContentManager.LanguageCode.ru:
        case LocalizedContentManager.LanguageCode.pt:
        case LocalizedContentManager.LanguageCode.es:
        case LocalizedContentManager.LanguageCode.de:
        case LocalizedContentManager.LanguageCode.th:
          string str4 = string.Concat((object) (Game1.timeOfDay / 100 % 24));
          str3 = Game1.timeOfDay / 100 % 24 <= 9 ? "0" + str4 : str4;
          break;
        case LocalizedContentManager.LanguageCode.zh:
          str3 = Game1.timeOfDay / 100 % 24 == 0 ? "00" : (Game1.timeOfDay / 100 % 12 == 0 ? "12" : string.Concat((object) (Game1.timeOfDay / 100 % 12)));
          break;
      }
      string text2 = str3 + ":" + (object) (Game1.timeOfDay % 100) + str2;
      if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.en)
        text2 = text2 + " " + (Game1.timeOfDay < 1200 || Game1.timeOfDay >= 2400 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:DayTimeMoneyBox.cs.10370") : Game1.content.LoadString("Strings\\StringsFromCSFiles:DayTimeMoneyBox.cs.10371"));
      else if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ja)
        text2 = Game1.timeOfDay < 1200 || Game1.timeOfDay >= 2400 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:DayTimeMoneyBox.cs.10370") + " " + text2 : Game1.content.LoadString("Strings\\StringsFromCSFiles:DayTimeMoneyBox.cs.10371") + " " + text2;
      else if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.zh)
        text2 = Game1.timeOfDay < 600 || Game1.timeOfDay >= 2400 ? "凌晨 " + text2 : (Game1.timeOfDay < 1200 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:DayTimeMoneyBox.cs.10370") + " " + text2 : (Game1.timeOfDay < 1300 ? "中午  " + text2 : (Game1.timeOfDay < 1900 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:DayTimeMoneyBox.cs.10371") + " " + text2 : "晚上  " + text2)));
      Vector2 vector2_3 = Game1.dialogueFont.MeasureString(text2);
      Vector2 vector2_4 = new Vector2((float) ((double) this.sourceRect.X * 0.550000011920929 - (double) vector2_3.X / 2.0 + (this.timeShakeTimer > 0 ? (double) Game1.random.Next(-2, 3) : 0.0)), (float) ((double) this.sourceRect.Y * (LocalizedContentManager.CurrentLanguageLatin ? 0.310000002384186 : 0.310000002384186) - (double) vector2_3.Y / 2.0 + (this.timeShakeTimer > 0 ? (double) Game1.random.Next(-2, 3) : 0.0)));
      int num1;
      if (!Game1.shouldTimePass() && !Game1.fadeToBlack)
      {
        timeSpan = Game1.currentGameTime.TotalGameTime;
        num1 = timeSpan.TotalMilliseconds % 2000.0 > 1000.0 ? 1 : 0;
      }
      else
        num1 = 1;
      bool flag = num1 != 0;
      Utility.drawTextWithShadow(b, text2, Game1.dialogueFont, this.position + vector2_4, Game1.timeOfDay >= 2400 ? Color.Red : Game1.textColor * (flag ? 1f : 0.5f), 1f, -1f, -1, -1, 1f, 3);
      int num2 = (int) ((double) (Game1.timeOfDay - Game1.timeOfDay % 100) + (double) (Game1.timeOfDay % 100 / 10) * 16.6599998474121);
      if (Game1.player.questLog.Count > 0)
      {
        this.questButton.draw(b);
        if (this.questPulseTimer > 0)
        {
          float num3 = (float) (1.0 / ((double) Math.Max(300f, (float) Math.Abs(this.questPulseTimer % 1000 - 500)) / 500.0));
          b.Draw(Game1.mouseCursors, new Vector2((float) (this.questButton.bounds.X + 6 * Game1.pixelZoom), (float) (this.questButton.bounds.Y + 8 * Game1.pixelZoom)) + ((double) num3 > 1.0 ? new Vector2((float) Game1.random.Next(-1, 2), (float) Game1.random.Next(-1, 2)) : Vector2.Zero), new Rectangle?(new Rectangle(395, 497, 3, 8)), Color.White, 0.0f, new Vector2(2f, 4f), (float) Game1.pixelZoom * num3, SpriteEffects.None, 0.99f);
        }
      }
      if (Game1.options.zoomButtons)
      {
        this.zoomInButton.draw(b, Color.White * ((double) Game1.options.zoomLevel >= 1.25 ? 0.5f : 1f), 1f);
        this.zoomOutButton.draw(b, Color.White * ((double) Game1.options.zoomLevel <= 0.75 ? 0.5f : 1f), 1f);
      }
      this.drawMoneyBox(b, -1, -1);
      if (!this.hoverText.Equals("") && this.isWithinBounds(Game1.getOldMouseX(), Game1.getOldMouseY()))
        IClickableMenu.drawHoverText(b, this.hoverText, Game1.dialogueFont, 0, 0, -1, (string) null, -1, (string[]) null, (Item) null, 0, -1, -1, -1, -1, 1f, (CraftingRecipe) null);
      b.Draw(Game1.mouseCursors, this.position + new Vector2(88f, 88f), new Rectangle?(new Rectangle(324, 477, 7, 19)), Color.White, (float) (Math.PI + Math.Min(Math.PI, ((double) num2 + (double) Game1.gameTimeInterval / 7000.0 * 16.6000003814697 - 600.0) / 2000.0 * Math.PI)), new Vector2(3f, 17f), 4f, SpriteEffects.None, 0.9f);
    }

    private void updatePosition()
    {
      this.position = new Vector2((float) (Game1.viewport.Width - 300), (float) (Game1.tileSize / 8));
      if (Game1.isOutdoorMapSmallerThanViewport())
        this.position = new Vector2(Math.Min(this.position.X, (float) (-Game1.viewport.X + Game1.currentLocation.map.Layers[0].LayerWidth * Game1.tileSize - 300)), (float) (Game1.tileSize / 8));
      Utility.makeSafe(ref this.position, 300, 284);
      Game1.debugOutput = "position = " + (object) this.position.X;
      this.xPositionOnScreen = (int) this.position.X;
      this.yPositionOnScreen = (int) this.position.Y;
      this.questButton.bounds = new Rectangle(this.xPositionOnScreen + 212, this.yPositionOnScreen + 240, 44, 46);
      this.zoomOutButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + Game1.pixelZoom * 23, this.yPositionOnScreen + 244, 7 * Game1.pixelZoom, 8 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(177, 345, 7, 8), 4f, false);
      this.zoomInButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + Game1.pixelZoom * 31, this.yPositionOnScreen + 244, 7 * Game1.pixelZoom, 8 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(184, 345, 7, 8), 4f, false);
    }
  }
}
