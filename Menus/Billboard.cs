// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.Billboard
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Locations;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Menus
{
  public class Billboard : IClickableMenu
  {
    private string hoverText = "";
    private Texture2D billboardTexture;
    public const int basewidth = 338;
    public const int baseWidth_calendar = 301;
    public const int baseheight = 198;
    private bool dailyQuestBoard;
    public ClickableComponent acceptQuestButton;
    public List<ClickableTextureComponent> calendarDays;

    public Billboard(bool dailyQuest = false)
      : base(0, 0, 0, 0, true)
    {
      if (!Game1.player.hasOrWillReceiveMail("checkedBulletinOnce"))
      {
        Game1.player.mailReceived.Add("checkedBulletinOnce");
        (Game1.getLocationFromName("Town") as Town).checkedBoard();
      }
      this.dailyQuestBoard = dailyQuest;
      this.billboardTexture = Game1.temporaryContent.Load<Texture2D>("LooseSprites\\Billboard");
      this.width = (dailyQuest ? 338 : 301) * Game1.pixelZoom;
      this.height = 198 * Game1.pixelZoom;
      Vector2 centeringOnScreen = Utility.getTopLeftPositionForCenteringOnScreen(this.width, this.height, 0, 0);
      this.xPositionOnScreen = (int) centeringOnScreen.X;
      this.yPositionOnScreen = (int) centeringOnScreen.Y;
      this.acceptQuestButton = new ClickableComponent(new Rectangle(this.xPositionOnScreen + this.width / 2 - Game1.tileSize * 2, this.yPositionOnScreen + this.height - Game1.tileSize * 2, (int) Game1.dialogueFont.MeasureString(Game1.content.LoadString("Strings\\UI:AcceptQuest")).X + Game1.pixelZoom * 6, (int) Game1.dialogueFont.MeasureString(Game1.content.LoadString("Strings\\UI:AcceptQuest")).Y + Game1.pixelZoom * 6), "")
      {
        myID = 0
      };
      this.upperRightCloseButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width - 5 * Game1.pixelZoom, this.yPositionOnScreen, 12 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(337, 494, 12, 12), (float) Game1.pixelZoom, false);
      Game1.playSound("bigSelect");
      if (!dailyQuest)
      {
        this.calendarDays = new List<ClickableTextureComponent>();
        Dictionary<int, NPC> dictionary = new Dictionary<int, NPC>();
        foreach (NPC allCharacter in Utility.getAllCharacters())
        {
          if (allCharacter.birthday_Season != null && allCharacter.birthday_Season.Equals(Game1.currentSeason) && !dictionary.ContainsKey(allCharacter.birthday_Day) && (Game1.player.friendships.ContainsKey(allCharacter.name) || !allCharacter.name.Equals("Dwarf") && !allCharacter.name.Equals("Sandy") && !allCharacter.name.Equals("Krobus")))
            dictionary.Add(allCharacter.birthday_Day, allCharacter);
        }
        for (int index = 1; index <= 28; ++index)
        {
          string str = "";
          string hoverText = "";
          NPC npc = dictionary.ContainsKey(index) ? dictionary[index] : (NPC) null;
          if (Utility.isFestivalDay(index, Game1.currentSeason))
            str = Game1.temporaryContent.Load<Dictionary<string, string>>("Data\\Festivals\\" + Game1.currentSeason + (object) index)["name"];
          else if ((object) npc != null)
          {
            if ((int) npc.name.Last<char>() == 115 || LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.de && ((int) npc.name.Last<char>() == 120 || (int) npc.name.Last<char>() == 223 || (int) npc.name.Last<char>() == 122))
              hoverText = Game1.content.LoadString("Strings\\UI:Billboard_SBirthday", (object) npc.displayName);
            else
              hoverText = Game1.content.LoadString("Strings\\UI:Billboard_Birthday", (object) npc.displayName);
          }
          List<ClickableTextureComponent> calendarDays = this.calendarDays;
          ClickableTextureComponent textureComponent = new ClickableTextureComponent(str, new Rectangle(this.xPositionOnScreen + 38 * Game1.pixelZoom + (index - 1) % 7 * 32 * Game1.pixelZoom, this.yPositionOnScreen + 50 * Game1.pixelZoom + (index - 1) / 7 * 32 * Game1.pixelZoom, 31 * Game1.pixelZoom, 31 * Game1.pixelZoom), str, hoverText, (object) npc != null ? npc.sprite.Texture : (Texture2D) null, (object) npc != null ? new Rectangle(0, 0, 16, 24) : Rectangle.Empty, 1f, false);
          int num1 = index;
          textureComponent.myID = num1;
          int num2 = index % 7 != 0 ? index + 1 : -1;
          textureComponent.rightNeighborID = num2;
          int num3 = index % 7 != 1 ? index - 1 : -1;
          textureComponent.leftNeighborID = num3;
          int num4 = index + 7;
          textureComponent.downNeighborID = num4;
          int num5 = index > 7 ? index - 7 : -1;
          textureComponent.upNeighborID = num5;
          calendarDays.Add(textureComponent);
        }
      }
      if (!Game1.options.SnappyMenus)
        return;
      this.populateClickableComponentList();
      this.snapToDefaultClickableComponent();
    }

    public override void snapToDefaultClickableComponent()
    {
      this.currentlySnappedComponent = this.getComponentWithID(this.dailyQuestBoard ? 0 : 1);
      this.snapCursorToCurrentSnappedComponent();
    }

    public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
    {
      base.gameWindowSizeChanged(oldBounds, newBounds);
      Game1.activeClickableMenu = (IClickableMenu) new Billboard(this.dailyQuestBoard);
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
      Game1.playSound("bigDeSelect");
      this.exitThisMenu(true);
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
      base.receiveLeftClick(x, y, playSound);
      if (!this.dailyQuestBoard || Game1.questOfTheDay == null || Game1.questOfTheDay.accepted && Game1.questOfTheDay.currentObjective != null && Game1.questOfTheDay.currentObjective.Length != 0 || !this.acceptQuestButton.containsPoint(x, y))
        return;
      Game1.playSound("newArtifact");
      Game1.questOfTheDay.dailyQuest = true;
      Game1.questOfTheDay.accepted = true;
      Game1.questOfTheDay.canBeCancelled = true;
      Game1.questOfTheDay.daysLeft = 2;
      Game1.player.questLog.Add(Game1.questOfTheDay);
    }

    public override void performHoverAction(int x, int y)
    {
      base.performHoverAction(x, y);
      this.hoverText = "";
      if (this.dailyQuestBoard && Game1.questOfTheDay != null && !Game1.questOfTheDay.accepted)
      {
        float scale = this.acceptQuestButton.scale;
        this.acceptQuestButton.scale = this.acceptQuestButton.bounds.Contains(x, y) ? 1.5f : 1f;
        if ((double) this.acceptQuestButton.scale > (double) scale)
          Game1.playSound("Cowboy_gunshot");
      }
      if (this.calendarDays == null)
        return;
      foreach (ClickableTextureComponent calendarDay in this.calendarDays)
      {
        if (calendarDay.bounds.Contains(x, y))
          this.hoverText = calendarDay.hoverText.Length <= 0 ? calendarDay.label : calendarDay.hoverText;
      }
    }

    public override void draw(SpriteBatch b)
    {
      b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.75f);
      b.Draw(this.billboardTexture, new Vector2((float) this.xPositionOnScreen, (float) this.yPositionOnScreen), new Rectangle?(this.dailyQuestBoard ? new Rectangle(0, 0, 338, 198) : new Rectangle(0, 198, 301, 198)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1f);
      if (!this.dailyQuestBoard)
      {
        b.DrawString(Game1.dialogueFont, Utility.getSeasonNameFromNumber(Utility.getSeasonNumber(Game1.currentSeason)), new Vector2((float) (this.xPositionOnScreen + Game1.tileSize * 5 / 2), (float) (this.yPositionOnScreen + Game1.tileSize * 5 / 4)), Game1.textColor);
        b.DrawString(Game1.dialogueFont, Game1.content.LoadString("Strings\\UI:Billboard_Year", (object) Game1.year), new Vector2((float) (this.xPositionOnScreen + Game1.tileSize * 7), (float) (this.yPositionOnScreen + Game1.tileSize * 5 / 4)), Game1.textColor);
        for (int index = 0; index < this.calendarDays.Count; ++index)
        {
          if (this.calendarDays[index].name.Length > 0)
            Utility.drawWithShadow(b, this.billboardTexture, new Vector2((float) (this.calendarDays[index].bounds.X + Game1.pixelZoom * 10), (float) (this.calendarDays[index].bounds.Y + Game1.pixelZoom * 14) - Game1.dialogueButtonScale / 2f), new Rectangle(1 + (int) (Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 600.0 / 100.0) * 14, 398, 14, 12), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
          else if (this.calendarDays[index].hoverText.Length > 0)
            b.Draw(this.calendarDays[index].texture, new Vector2((float) (this.calendarDays[index].bounds.X + Game1.pixelZoom * 10), (float) (this.calendarDays[index].bounds.Y + 7 * Game1.pixelZoom)), new Rectangle?(this.calendarDays[index].sourceRect), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1f);
          if (Game1.dayOfMonth > index + 1)
            b.Draw(Game1.staminaRect, this.calendarDays[index].bounds, Color.Gray * 0.25f);
          else if (Game1.dayOfMonth == index + 1)
          {
            int num = (int) ((double) Game1.pixelZoom * (double) Game1.dialogueButtonScale / 8.0);
            IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(379, 357, 3, 3), this.calendarDays[index].bounds.X - num, this.calendarDays[index].bounds.Y - num, this.calendarDays[index].bounds.Width + num * 2, this.calendarDays[index].bounds.Height + num * 2, Color.Blue, (float) Game1.pixelZoom, false);
          }
        }
      }
      else if (Game1.questOfTheDay == null || Game1.questOfTheDay.currentObjective == null || Game1.questOfTheDay.currentObjective.Length == 0)
      {
        b.DrawString(Game1.dialogueFont, Game1.content.LoadString("Strings\\UI:Billboard_NothingPosted"), new Vector2((float) (this.xPositionOnScreen + Game1.tileSize * 6), (float) (this.yPositionOnScreen + Game1.tileSize * 5)), Game1.textColor);
      }
      else
      {
        string text = Game1.parseText(Game1.questOfTheDay.questDescription, Game1.dialogueFont, Game1.tileSize * 10);
        Utility.drawTextWithShadow(b, text, Game1.dialogueFont, new Vector2((float) (this.xPositionOnScreen + Game1.tileSize * 5 + Game1.tileSize / 2), (float) (this.yPositionOnScreen + Game1.tileSize * 4)), Game1.textColor, 1f, -1f, -1, -1, 0.5f, 3);
        if (!Game1.questOfTheDay.accepted)
        {
          IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(403, 373, 9, 9), this.acceptQuestButton.bounds.X, this.acceptQuestButton.bounds.Y, this.acceptQuestButton.bounds.Width, this.acceptQuestButton.bounds.Height, (double) this.acceptQuestButton.scale > 1.0 ? Color.LightPink : Color.White, (float) Game1.pixelZoom * this.acceptQuestButton.scale, true);
          Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:AcceptQuest"), Game1.dialogueFont, new Vector2((float) (this.acceptQuestButton.bounds.X + Game1.pixelZoom * 3), (float) (this.acceptQuestButton.bounds.Y + (LocalizedContentManager.CurrentLanguageLatin ? Game1.pixelZoom * 4 : Game1.pixelZoom * 3))), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
        }
      }
      base.draw(b);
      Game1.mouseCursorTransparency = 1f;
      this.drawMouse(b);
      if (this.hoverText.Length <= 0)
        return;
      IClickableMenu.drawHoverText(b, this.hoverText, Game1.dialogueFont, 0, 0, -1, (string) null, -1, (string[]) null, (Item) null, 0, -1, -1, -1, -1, 1f, (CraftingRecipe) null);
    }
  }
}
