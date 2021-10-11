// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.SocialPage
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Characters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Menus
{
  public class SocialPage : IClickableMenu
  {
    private string descriptionText = "";
    private string hoverText = "";
    private List<string> kidsNames = new List<string>();
    public const int slotsOnPage = 5;
    private ClickableTextureComponent upButton;
    private ClickableTextureComponent downButton;
    private ClickableTextureComponent scrollBar;
    private Rectangle scrollBarRunner;
    private List<ClickableTextureComponent> friendNames;
    private int slotPosition;
    private Dictionary<string, string> npcNames;
    private bool scrolling;

    public SocialPage(int x, int y, int width, int height)
      : base(x, y, width, height, false)
    {
      this.friendNames = new List<ClickableTextureComponent>();
      this.npcNames = new Dictionary<string, string>();
      foreach (NPC allCharacter in Utility.getAllCharacters())
      {
        if ((!allCharacter.name.Equals("Sandy") || Game1.player.mailReceived.Contains("ccVault")) && (!allCharacter.name.Equals("???") && !allCharacter.name.Equals("Bouncer")) && (!allCharacter.name.Equals("Marlon") && !allCharacter.name.Equals("Gil") && (!allCharacter.name.Equals("Gunther") && !allCharacter.name.Equals("Henchman"))) && (!allCharacter.IsMonster && !(allCharacter is Horse) && (!(allCharacter is Pet) && !(allCharacter is JunimoHarvester))))
        {
          if (Game1.player.friendships.ContainsKey(allCharacter.name))
          {
            string hoverText = (allCharacter.datingFarmer ? "true" : "false") + (allCharacter.datable ? "_true" : "_false");
            this.friendNames.Add(new ClickableTextureComponent(allCharacter.name, new Rectangle(x + IClickableMenu.borderWidth + Game1.pixelZoom, 0, width, Game1.tileSize), (string) null, hoverText, allCharacter.sprite.Texture, allCharacter.getMugShotSourceRect(), (float) Game1.pixelZoom, false));
            this.npcNames[allCharacter.name] = allCharacter.getName();
            if (allCharacter is Child)
              this.kidsNames.Add(allCharacter.name);
          }
          else if (!allCharacter.name.Equals("Dwarf") && !allCharacter.name.Contains("Qi") && (!(allCharacter is Pet) && !(allCharacter is Horse)) && (!(allCharacter is Junimo) && !(allCharacter is Child)))
          {
            this.friendNames.Add(new ClickableTextureComponent(allCharacter.name, new Rectangle(x + IClickableMenu.borderWidth + Game1.pixelZoom, 0, width, Game1.tileSize), (string) null, "false_false", allCharacter.sprite.Texture, allCharacter.getMugShotSourceRect(), (float) Game1.pixelZoom, false));
            this.npcNames[allCharacter.name] = "???";
            if (allCharacter is Child)
              this.kidsNames.Add(allCharacter.name);
          }
        }
      }
      this.friendNames = this.friendNames.OrderBy<ClickableTextureComponent, int>((Func<ClickableTextureComponent, int>) (i => -Game1.player.getFriendshipLevelForNPC(i.name))).ToList<ClickableTextureComponent>();
      this.upButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + width + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 459, 11, 12), (float) Game1.pixelZoom, false);
      this.downButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + width + Game1.tileSize / 4, this.yPositionOnScreen + height - Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 472, 11, 12), (float) Game1.pixelZoom, false);
      this.scrollBar = new ClickableTextureComponent(new Rectangle(this.upButton.bounds.X + Game1.pixelZoom * 3, this.upButton.bounds.Y + this.upButton.bounds.Height + Game1.pixelZoom, 6 * Game1.pixelZoom, 10 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(435, 463, 6, 10), (float) Game1.pixelZoom, false);
      this.scrollBarRunner = new Rectangle(this.scrollBar.bounds.X, this.upButton.bounds.Y + this.upButton.bounds.Height + Game1.pixelZoom, this.scrollBar.bounds.Width, height - Game1.tileSize * 2 - this.upButton.bounds.Height - Game1.pixelZoom * 2);
      this.updateSlots();
    }

    public void updateSlots()
    {
      int num1 = 0;
      for (int slotPosition = this.slotPosition; slotPosition < this.slotPosition + 5; ++slotPosition)
      {
        if (this.friendNames.Count > slotPosition)
        {
          int num2 = this.yPositionOnScreen + IClickableMenu.borderWidth + Game1.tileSize / 2 + (Game1.tileSize + Game1.tileSize * 3 / 4) * num1 + Game1.pixelZoom * 8;
          this.friendNames[slotPosition].bounds.Y = num2;
        }
        ++num1;
      }
    }

    public override void applyMovementKey(int direction)
    {
      if (direction == 0 && this.slotPosition > 0)
        this.upArrowPressed();
      else if (direction == 2 && this.slotPosition < this.friendNames.Count - 5)
      {
        this.downArrowPressed();
      }
      else
      {
        if (direction != 3 && direction != 1)
          return;
        base.applyMovementKey(direction);
      }
    }

    public override void leftClickHeld(int x, int y)
    {
      base.leftClickHeld(x, y);
      if (!this.scrolling)
        return;
      int y1 = this.scrollBar.bounds.Y;
      this.scrollBar.bounds.Y = Math.Min(this.yPositionOnScreen + this.height - Game1.tileSize - Game1.pixelZoom * 3 - this.scrollBar.bounds.Height, Math.Max(y, this.yPositionOnScreen + this.upButton.bounds.Height + Game1.pixelZoom * 5));
      this.slotPosition = Math.Min(this.friendNames.Count - 5, Math.Max(0, (int) ((double) this.friendNames.Count * (double) ((float) (y - this.scrollBarRunner.Y) / (float) this.scrollBarRunner.Height))));
      this.setScrollBarToCurrentIndex();
      int y2 = this.scrollBar.bounds.Y;
      if (y1 == y2)
        return;
      Game1.playSound("shiny4");
    }

    public override void releaseLeftClick(int x, int y)
    {
      base.releaseLeftClick(x, y);
      this.scrolling = false;
    }

    private void setScrollBarToCurrentIndex()
    {
      if (this.friendNames.Count > 0)
      {
        this.scrollBar.bounds.Y = this.scrollBarRunner.Height / Math.Max(1, this.friendNames.Count - 5 + 1) * this.slotPosition + this.upButton.bounds.Bottom + Game1.pixelZoom;
        if (this.slotPosition == this.friendNames.Count - 5)
          this.scrollBar.bounds.Y = this.downButton.bounds.Y - this.scrollBar.bounds.Height - Game1.pixelZoom;
      }
      this.updateSlots();
    }

    public override void receiveScrollWheelAction(int direction)
    {
      base.receiveScrollWheelAction(direction);
      if (direction > 0 && this.slotPosition > 0)
      {
        this.upArrowPressed();
        Game1.playSound("shiny4");
      }
      else
      {
        if (direction >= 0 || this.slotPosition >= Math.Max(0, this.friendNames.Count - 5))
          return;
        this.downArrowPressed();
        Game1.playSound("shiny4");
      }
    }

    public void upArrowPressed()
    {
      this.slotPosition = this.slotPosition - 1;
      this.updateSlots();
      this.upButton.scale = 3.5f;
      this.setScrollBarToCurrentIndex();
    }

    public void downArrowPressed()
    {
      this.slotPosition = this.slotPosition + 1;
      this.updateSlots();
      this.downButton.scale = 3.5f;
      this.setScrollBarToCurrentIndex();
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
      if (this.upButton.containsPoint(x, y) && this.slotPosition > 0)
      {
        this.upArrowPressed();
        Game1.playSound("shwip");
      }
      else if (this.downButton.containsPoint(x, y) && this.slotPosition < this.friendNames.Count - 5)
      {
        this.downArrowPressed();
        Game1.playSound("shwip");
      }
      else if (this.scrollBar.containsPoint(x, y))
        this.scrolling = true;
      else if (!this.downButton.containsPoint(x, y) && x > this.xPositionOnScreen + this.width - Game1.tileSize * 3 / 2 && (x < this.xPositionOnScreen + this.width + Game1.tileSize * 2 && y > this.yPositionOnScreen) && y < this.yPositionOnScreen + this.height)
      {
        this.scrolling = true;
        this.leftClickHeld(x, y);
        this.releaseLeftClick(x, y);
      }
      this.slotPosition = Math.Max(0, Math.Min(this.friendNames.Count - 5, this.slotPosition));
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
    }

    public override void performHoverAction(int x, int y)
    {
      this.descriptionText = "";
      this.hoverText = "";
      this.upButton.tryHover(x, y, 0.1f);
      this.downButton.tryHover(x, y, 0.1f);
    }

    public override void draw(SpriteBatch b)
    {
      this.drawHorizontalPartition(b, this.yPositionOnScreen + IClickableMenu.borderWidth + Game1.tileSize * 2 + Game1.pixelZoom, true);
      this.drawHorizontalPartition(b, this.yPositionOnScreen + IClickableMenu.borderWidth + Game1.tileSize * 3 + Game1.tileSize / 2 + Game1.pixelZoom * 5, true);
      this.drawHorizontalPartition(b, this.yPositionOnScreen + IClickableMenu.borderWidth + Game1.tileSize * 5 + Game1.pixelZoom * 9, true);
      this.drawHorizontalPartition(b, this.yPositionOnScreen + IClickableMenu.borderWidth + Game1.tileSize * 6 + Game1.tileSize / 2 + Game1.pixelZoom * 13, true);
      this.drawVerticalPartition(b, this.xPositionOnScreen + Game1.tileSize * 4 + Game1.pixelZoom * 3, true);
      this.drawVerticalPartition(b, this.xPositionOnScreen + Game1.tileSize * 4 + Game1.pixelZoom * 3 + 85 * Game1.pixelZoom, true);
      for (int slotPosition = this.slotPosition; slotPosition < this.slotPosition + 5; ++slotPosition)
      {
        if (slotPosition < this.friendNames.Count)
        {
          this.friendNames[slotPosition].draw(b);
          int heartLevelForNpc = Game1.player.getFriendshipHeartLevelForNPC(this.friendNames[slotPosition].name);
          bool flag1 = this.friendNames[slotPosition].hoverText.Split('_')[1].Equals("true");
          bool flag2 = Game1.player.spouse != null && Game1.player.spouse.Equals(this.friendNames[slotPosition].name);
          float y = Game1.smallFont.MeasureString("W").Y;
          float num1 = LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ru ? (float) (-(double) y / 2.0) : 0.0f;
          b.DrawString(Game1.dialogueFont, this.npcNames[this.friendNames[slotPosition].name], new Vector2((float) (this.xPositionOnScreen + IClickableMenu.borderWidth * 3 / 2 + Game1.tileSize - Game1.pixelZoom * 5 + Game1.tileSize * 3 / 2) - Game1.dialogueFont.MeasureString(this.npcNames[this.friendNames[slotPosition].name]).X / 2f, (float) ((double) (this.friendNames[slotPosition].bounds.Y + Game1.tileSize * 3 / 4) + (double) num1 - (flag1 ? (double) (Game1.pixelZoom * 6) : (double) (Game1.pixelZoom * 5)))), Game1.textColor);
          for (int index = 0; index < 10 + (this.friendNames[slotPosition].name.Equals(Game1.player.spouse) ? 2 : 0); ++index)
          {
            int x = index < heartLevelForNpc ? 211 : 218;
            if (flag1)
            {
              if (!this.friendNames[slotPosition].hoverText.Split('_')[0].Equals("true") && !flag2 && index >= 8)
                x = 211;
            }
            if (index < 10)
            {
              SpriteBatch spriteBatch = b;
              Texture2D mouseCursors = Game1.mouseCursors;
              Vector2 position = new Vector2((float) (this.xPositionOnScreen + Game1.tileSize * 5 - Game1.pixelZoom * 2 + index * (8 * Game1.pixelZoom)), (float) (this.friendNames[slotPosition].bounds.Y + Game1.tileSize - Game1.pixelZoom * 7));
              Rectangle? sourceRectangle = new Rectangle?(new Rectangle(x, 428, 7, 6));
              Color color;
              if (flag1)
              {
                if (!this.friendNames[slotPosition].hoverText.Split('_')[0].Equals("true") && !flag2 && index >= 8)
                {
                  color = Color.Black * 0.35f;
                  goto label_11;
                }
              }
              color = Color.White;
label_11:
              double num2 = 0.0;
              Vector2 zero = Vector2.Zero;
              double pixelZoom = (double) Game1.pixelZoom;
              int num3 = 0;
              double num4 = 0.879999995231628;
              spriteBatch.Draw(mouseCursors, position, sourceRectangle, color, (float) num2, zero, (float) pixelZoom, (SpriteEffects) num3, (float) num4);
            }
            else
              b.Draw(Game1.mouseCursors, new Vector2((float) (this.xPositionOnScreen + Game1.tileSize * 5 - Game1.pixelZoom * 2 + (index - 10) * (8 * Game1.pixelZoom)), (float) (this.friendNames[slotPosition].bounds.Y + Game1.tileSize)), new Rectangle?(new Rectangle(x, 428, 7, 6)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.88f);
          }
          if (flag1)
          {
            string str;
            if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.pt)
            {
              if (Game1.getCharacterFromName(this.friendNames[slotPosition].name, false).gender != 0)
                str = ((IEnumerable<string>) Game1.content.LoadString("Strings\\StringsFromCSFiles:SocialPage.cs.11635").Split('/')).Last<string>();
              else
                str = ((IEnumerable<string>) Game1.content.LoadString("Strings\\StringsFromCSFiles:SocialPage.cs.11635").Split('/')).First<string>();
            }
            else
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:SocialPage.cs.11635");
            string text1 = str;
            if (flag2 && (object) Game1.getCharacterFromName(this.friendNames[slotPosition].name, false) != null)
            {
              text1 = Game1.getCharacterFromName(this.friendNames[slotPosition].name, false).gender == 0 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:SocialPage.cs.11636") : Game1.content.LoadString("Strings\\StringsFromCSFiles:SocialPage.cs.11637");
            }
            else
            {
              if (!Game1.player.isMarried())
              {
                if (this.friendNames[slotPosition].hoverText.Split('_')[0].Equals("true") && (object) Game1.getCharacterFromName(this.friendNames[slotPosition].name, false) != null)
                {
                  text1 = Game1.getCharacterFromName(this.friendNames[slotPosition].name, false).gender == 0 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:SocialPage.cs.11639") : Game1.content.LoadString("Strings\\StringsFromCSFiles:SocialPage.cs.11640");
                  goto label_28;
                }
              }
              if (Game1.getCharacterFromName(this.friendNames[slotPosition].name, false).divorcedFromFarmer)
                text1 = Game1.getCharacterFromName(this.friendNames[slotPosition].name, false).gender == 0 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:SocialPage.cs.11642") : Game1.content.LoadString("Strings\\StringsFromCSFiles:SocialPage.cs.11643");
            }
label_28:
            int width = (IClickableMenu.borderWidth * 3 + Game1.tileSize * 2 - Game1.pixelZoom * 10 + Game1.tileSize * 3) / 2;
            string text2 = Game1.parseText(text1, Game1.smallFont, width);
            Vector2 vector2 = Game1.smallFont.MeasureString(text2);
            b.DrawString(Game1.smallFont, text2, new Vector2((float) (this.xPositionOnScreen + Game1.tileSize * 3 + Game1.pixelZoom * 2) - vector2.X / 2f, (float) this.friendNames[slotPosition].bounds.Bottom - (vector2.Y - y)), Game1.textColor);
          }
          if (Game1.player.friendships.ContainsKey(this.friendNames[slotPosition].name) && (Game1.player.spouse == null || !this.friendNames[slotPosition].name.Equals(Game1.player.spouse)) && !this.kidsNames.Contains(this.friendNames[slotPosition].name))
          {
            b.Draw(Game1.mouseCursors, new Vector2((float) (this.xPositionOnScreen + Game1.tileSize * 6 + 66 * Game1.pixelZoom), (float) (this.friendNames[slotPosition].bounds.Y + Game1.tileSize / 2 - Game1.pixelZoom * 3)), new Rectangle?(new Rectangle(229, 410, 14, 14)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.88f);
            b.Draw(Game1.mouseCursors, new Vector2((float) (this.xPositionOnScreen + Game1.tileSize * 6 + 81 * Game1.pixelZoom), (float) (this.friendNames[slotPosition].bounds.Y + Game1.tileSize / 2)), new Rectangle?(new Rectangle(227 + (Game1.player.friendships[this.friendNames[slotPosition].name][1] == 2 ? 9 : 0), 425, 9, 9)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.88f);
            b.Draw(Game1.mouseCursors, new Vector2((float) (this.xPositionOnScreen + Game1.tileSize * 6 + 91 * Game1.pixelZoom), (float) (this.friendNames[slotPosition].bounds.Y + Game1.tileSize / 2)), new Rectangle?(new Rectangle(227 + (Game1.player.friendships[this.friendNames[slotPosition].name][1] >= 1 ? 9 : 0), 425, 9, 9)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.88f);
          }
          if (Game1.player.spouse != null && this.friendNames[slotPosition].name.Equals(Game1.player.spouse))
            b.Draw(Game1.objectSpriteSheet, new Vector2((float) (this.xPositionOnScreen + IClickableMenu.borderWidth * 7 / 4 + Game1.tileSize * 3), (float) this.friendNames[slotPosition].bounds.Y), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 460, 16, 16)), Color.White, 0.0f, Vector2.Zero, 2f, SpriteEffects.None, 0.88f);
          else if (this.friendNames[slotPosition].hoverText.Split('_')[0].Equals("true"))
            b.Draw(Game1.objectSpriteSheet, new Vector2((float) (this.xPositionOnScreen + IClickableMenu.borderWidth * 7 / 4 + Game1.tileSize * 3), (float) this.friendNames[slotPosition].bounds.Y), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 458, 16, 16)), Color.White, 0.0f, Vector2.Zero, 2f, SpriteEffects.None, 0.88f);
        }
      }
      this.upButton.draw(b);
      this.downButton.draw(b);
      IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(403, 383, 6, 6), this.scrollBarRunner.X, this.scrollBarRunner.Y, this.scrollBarRunner.Width, this.scrollBarRunner.Height, Color.White, (float) Game1.pixelZoom, true);
      this.scrollBar.draw(b);
      if (this.hoverText.Equals(""))
        return;
      IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, -1, (string) null, -1, (string[]) null, (Item) null, 0, -1, -1, -1, -1, 1f, (CraftingRecipe) null);
    }
  }
}
