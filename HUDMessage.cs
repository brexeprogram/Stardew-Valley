// Decompiled with JetBrains decompiler
// Type: StardewValley.HUDMessage
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;
using System;

namespace StardewValley
{
  public class HUDMessage
  {
    public float transparency = 1f;
    public int number = -1;
    public const float defaultTime = 3500f;
    public const int achievement_type = 1;
    public const int newQuest_type = 2;
    public const int error_type = 3;
    public const int stamina_type = 4;
    public const int health_type = 5;
    public string message;
    public string type;
    public Color color;
    public float timeLeft;
    public int whatType;
    public bool add;
    public bool achievement;
    public bool fadeIn;
    public bool noIcon;
    private Item messageSubject;

    public string Message
    {
      get
      {
        if (this.type == null)
          return this.message;
        if (this.type.Equals("Money"))
          return (this.add ? (object) "+ " : (object) "- ").ToString() + (object) this.number + "g";
        return (this.add ? "+ " : "- ") + (object) this.number + " " + this.type;
      }
      set
      {
        this.message = value;
      }
    }

    public HUDMessage(string message)
    {
      this.message = message;
      this.color = Color.SeaGreen;
      this.timeLeft = 3500f;
    }

    public HUDMessage(string message, bool achievement)
    {
      if (!achievement)
        return;
      this.message = Game1.content.LoadString("Strings\\StringsFromCSFiles:HUDMessage.cs.3824") + message;
      this.color = Color.OrangeRed;
      this.timeLeft = 5250f;
      this.achievement = true;
      this.whatType = 1;
    }

    public HUDMessage(string message, int whatType)
    {
      this.message = message;
      this.color = Color.OrangeRed;
      this.timeLeft = 5250f;
      this.achievement = true;
      this.whatType = whatType;
    }

    public HUDMessage(string type, int number, bool add, Color color, Item messageSubject = null)
    {
      this.type = type;
      this.add = add;
      this.color = color;
      this.timeLeft = 3500f;
      this.number = number;
      this.messageSubject = messageSubject;
    }

    public HUDMessage(string message, Color color, float timeLeft)
      : this(message, color, timeLeft, false)
    {
    }

    public HUDMessage(string message, string leaveMeNull)
    {
      this.message = Game1.parseText(message, Game1.dialogueFont, Game1.tileSize * 6);
      this.timeLeft = 3500f;
      this.color = Game1.textColor;
      this.noIcon = true;
    }

    public HUDMessage(string message, Color color, float timeLeft, bool fadeIn)
    {
      this.message = message;
      this.color = color;
      this.timeLeft = timeLeft;
      this.fadeIn = fadeIn;
      if (!fadeIn)
        return;
      this.transparency = 0.0f;
    }

    public bool update(GameTime time)
    {
      this.timeLeft = this.timeLeft - (float) time.ElapsedGameTime.Milliseconds;
      if ((double) this.timeLeft < 0.0)
      {
        this.transparency = this.transparency - 0.02f;
        if ((double) this.transparency < 0.0)
          return true;
      }
      else if (this.fadeIn)
        this.transparency = Math.Min(this.transparency + 0.02f, 1f);
      return false;
    }

    public void draw(SpriteBatch b, int i)
    {
      if (this.noIcon)
      {
        IClickableMenu.drawHoverText(b, this.message, Game1.dialogueFont, 0, 0, -1, (string) null, -1, (string[]) null, (Item) null, 0, -1, -1, Game1.tileSize / 4, (Game1.viewport.Width < 1400 ? -Game1.tileSize : 0) + Game1.graphics.GraphicsDevice.Viewport.Height - (i + 1) * Game1.tileSize * 7 / 4 - Game1.tileSize / 3 - (int) Game1.dialogueFont.MeasureString(this.message).Y, this.transparency, (CraftingRecipe) null);
      }
      else
      {
        Vector2 vector2 = new Vector2((float) (Game1.tileSize / 4), (float) (Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Bottom - (i + 1) * Game1.tileSize * 7 / 4 - Game1.tileSize));
        if (Game1.isOutdoorMapSmallerThanViewport())
          vector2.X = (float) Math.Max(Game1.tileSize / 4, -Game1.viewport.X + Game1.tileSize / 4);
        if (Game1.viewport.Width < 1400)
          vector2.Y -= (float) (Game1.tileSize * 3 / 4);
        b.Draw(Game1.mouseCursors, vector2, new Rectangle?(this.messageSubject == null || !(this.messageSubject is Object) || (this.messageSubject as Object).sellToStorePrice() <= 500 ? new Rectangle(293, 360, 26, 24) : new Rectangle(163, 399, 26, 24)), Color.White * this.transparency, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1f);
        float x = Game1.smallFont.MeasureString(this.messageSubject == null || this.messageSubject.DisplayName == null ? (this.message == null ? "" : this.message) : this.messageSubject.DisplayName).X;
        b.Draw(Game1.mouseCursors, new Vector2(vector2.X + (float) (26 * Game1.pixelZoom), vector2.Y), new Rectangle?(new Rectangle(319, 360, 1, 24)), Color.White * this.transparency, 0.0f, Vector2.Zero, new Vector2(x, (float) Game1.pixelZoom), SpriteEffects.None, 1f);
        b.Draw(Game1.mouseCursors, new Vector2(vector2.X + (float) (26 * Game1.pixelZoom) + x, vector2.Y), new Rectangle?(new Rectangle(323, 360, 6, 24)), Color.White * this.transparency, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1f);
        vector2.X += (float) (Game1.pixelZoom * 4);
        vector2.Y += (float) (Game1.pixelZoom * 4);
        if (this.messageSubject == null)
        {
          switch (this.whatType)
          {
            case 1:
              b.Draw(Game1.mouseCursors, vector2 + new Vector2(8f, 8f) * (float) Game1.pixelZoom, new Rectangle?(new Rectangle(294, 392, 16, 16)), Color.White * this.transparency, 0.0f, new Vector2(8f, 8f), (float) Game1.pixelZoom + Math.Max(0.0f, (float) (((double) this.timeLeft - 3000.0) / 900.0)), SpriteEffects.None, 1f);
              break;
            case 2:
              b.Draw(Game1.mouseCursors, vector2 + new Vector2(8f, 8f) * (float) Game1.pixelZoom, new Rectangle?(new Rectangle(403, 496, 5, 14)), Color.White * this.transparency, 0.0f, new Vector2(3f, 7f), (float) Game1.pixelZoom + Math.Max(0.0f, (float) (((double) this.timeLeft - 3000.0) / 900.0)), SpriteEffects.None, 1f);
              break;
            case 3:
              b.Draw(Game1.mouseCursors, vector2 + new Vector2(8f, 8f) * (float) Game1.pixelZoom, new Rectangle?(new Rectangle(268, 470, 16, 16)), Color.White * this.transparency, 0.0f, new Vector2(8f, 8f), (float) Game1.pixelZoom + Math.Max(0.0f, (float) (((double) this.timeLeft - 3000.0) / 900.0)), SpriteEffects.None, 1f);
              break;
            case 4:
              b.Draw(Game1.mouseCursors, vector2 + new Vector2(8f, 8f) * (float) Game1.pixelZoom, new Rectangle?(new Rectangle(0, 411, 16, 16)), Color.White * this.transparency, 0.0f, new Vector2(8f, 8f), (float) Game1.pixelZoom + Math.Max(0.0f, (float) (((double) this.timeLeft - 3000.0) / 900.0)), SpriteEffects.None, 1f);
              break;
            case 5:
              b.Draw(Game1.mouseCursors, vector2 + new Vector2(8f, 8f) * (float) Game1.pixelZoom, new Rectangle?(new Rectangle(16, 411, 16, 16)), Color.White * this.transparency, 0.0f, new Vector2(8f, 8f), (float) Game1.pixelZoom + Math.Max(0.0f, (float) (((double) this.timeLeft - 3000.0) / 900.0)), SpriteEffects.None, 1f);
              break;
          }
        }
        else
          this.messageSubject.drawInMenu(b, vector2, 1f + Math.Max(0.0f, (float) (((double) this.timeLeft - 3000.0) / 900.0)), this.transparency, 1f, false);
        vector2.X += (float) (Game1.tileSize * 4 / 5);
        vector2.Y += (float) (Game1.tileSize * 4 / 5);
        if (this.number > 1)
          Utility.drawTinyDigits(this.number, b, vector2, 3f, 1f, Color.White * this.transparency);
        vector2.X += (float) (Game1.tileSize / 2);
        vector2.Y -= (float) (Game1.tileSize * 2 / 5 + Game1.pixelZoom * 2);
        Utility.drawTextWithShadow(b, this.messageSubject == null ? this.message : this.messageSubject.DisplayName, Game1.smallFont, vector2, Game1.textColor * this.transparency, 1f, 1f, -1, -1, this.transparency, 3);
      }
    }
  }
}
