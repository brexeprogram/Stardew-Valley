// Decompiled with JetBrains decompiler
// Type: StardewValley.MoneyMadeScreen
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley
{
  public class MoneyMadeScreen
  {
    private Dictionary<ShippedItem, int> shippingItems = new Dictionary<ShippedItem, int>();
    private float starScale = 1f;
    private const int timeToDisplayEachItem = 200;
    public bool complete;
    public bool canProceed;
    public bool throbUp;
    public bool day;
    private int currentItemIndex;
    private int timeOnCurrentItem;
    private int total;

    public MoneyMadeScreen(List<Object> shippingItems, int timeOfDay)
    {
      if (timeOfDay < 2000)
        this.day = true;
      int randomItemFromSeason = Utility.getRandomItemFromSeason(Game1.currentSeason, 0, false);
      int num1 = Game1.cropsOfTheWeek[(Game1.dayOfMonth - 1) / 7];
      foreach (Object shippingItem in shippingItems)
      {
        ShippedItem key = new ShippedItem(shippingItem.ParentSheetIndex, shippingItem.Price, shippingItem.name);
        int num2 = shippingItem.Price * shippingItem.Stack;
        if (shippingItem.ParentSheetIndex == randomItemFromSeason)
          num2 = (int) ((double) num2 * 1.20000004768372);
        if (shippingItem.ParentSheetIndex == num1)
          num2 = (int) ((double) num2 * 1.10000002384186);
        if (shippingItem.Name.Contains("="))
          num2 += num2 / 2;
        int num3 = num2 - num2 % 5;
        if (this.shippingItems.ContainsKey(key))
        {
          Dictionary<ShippedItem, int> shippingItems1 = this.shippingItems;
          ShippedItem shippedItem = key;
          ShippedItem index1 = shippedItem;
          int num4 = shippingItems1[index1];
          ShippedItem index2 = shippedItem;
          int num5 = num4 + 1;
          shippingItems1[index2] = num5;
        }
        else
          this.shippingItems.Add(key, shippingItem.Stack);
        this.total = this.total + num3;
      }
    }

    public void update(int milliseconds, bool keyDown)
    {
      if (!this.complete)
      {
        this.timeOnCurrentItem = this.timeOnCurrentItem + (keyDown ? milliseconds * 2 : milliseconds);
        if (this.timeOnCurrentItem >= 200)
        {
          this.currentItemIndex = this.currentItemIndex + 1;
          Game1.playSound("shiny4");
          this.timeOnCurrentItem = 0;
          if (this.currentItemIndex == this.shippingItems.Count)
            this.complete = true;
        }
      }
      else
      {
        this.timeOnCurrentItem = this.timeOnCurrentItem + (keyDown ? milliseconds * 2 : milliseconds);
        if (this.timeOnCurrentItem >= 1000)
          this.canProceed = true;
      }
      this.starScale = !this.throbUp ? this.starScale - 0.01f : this.starScale + 0.01f;
      if ((double) this.starScale >= 1.20000004768372)
      {
        this.throbUp = false;
      }
      else
      {
        if ((double) this.starScale > 1.0)
          return;
        this.throbUp = true;
      }
    }

    public void draw(GameTime gametime)
    {
      if (this.day)
        Game1.graphics.GraphicsDevice.Clear(Utility.getSkyColorForSeason(Game1.currentSeason));
      Game1.drawTitleScreenBackground(gametime, this.day ? "_day" : "_night", Utility.weatherDebrisOffsetForSeason(Game1.currentSeason));
      Viewport viewport = Game1.graphics.GraphicsDevice.Viewport;
      int height = viewport.TitleSafeArea.Height - Game1.tileSize * 2;
      viewport = Game1.graphics.GraphicsDevice.Viewport;
      int x1 = viewport.TitleSafeArea.X;
      viewport = Game1.graphics.GraphicsDevice.Viewport;
      int num1 = viewport.Width / 2;
      int x2 = x1 + num1 - (int) ((double) ((this.shippingItems.Count / (height / Game1.tileSize - 4) + 1) * Game1.tileSize) * 3.0);
      viewport = Game1.graphics.GraphicsDevice.Viewport;
      int y1 = viewport.TitleSafeArea.Y + Game1.tileSize;
      int width = (int) ((double) ((this.shippingItems.Count / (height / Game1.tileSize - 4) + 1) * Game1.tileSize) * 6.0);
      Game1.drawDialogueBox(x2, y1, width, height, false, true, (string) null, false);
      int num2 = height - Game1.tileSize * 3;
      Point point = new Point(x2 + Game1.tileSize, y1 + Game1.tileSize);
      for (int index = 0; index < this.currentItemIndex; ++index)
      {
        SpriteBatch spriteBatch1 = Game1.spriteBatch;
        Texture2D objectSpriteSheet = Game1.objectSpriteSheet;
        double num3 = (double) (point.X + index * Game1.tileSize / (num2 - Game1.tileSize * 2) * Game1.tileSize * 4 + Game1.tileSize / 2);
        int num4 = index * Game1.tileSize % (num2 - Game1.tileSize * 2) - index * Game1.tileSize % (num2 - Game1.tileSize * 2) % Game1.tileSize;
        viewport = Game1.graphics.GraphicsDevice.Viewport;
        int y2 = viewport.TitleSafeArea.Y;
        double num5 = (double) (num4 + y2 + Game1.tileSize * 3 + Game1.tileSize / 2);
        Vector2 position1 = new Vector2((float) num3, (float) num5);
        Rectangle? sourceRectangle = new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.shippingItems.Keys.ElementAt<ShippedItem>(index).index, -1, -1));
        Color white = Color.White;
        double num6 = 0.0;
        Vector2 origin = new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2));
        double num7 = this.shippingItems.Keys.ElementAt<ShippedItem>(index).name.Contains("=") ? (double) this.starScale : 1.0;
        int num8 = 0;
        double num9 = 0.999998986721039;
        spriteBatch1.Draw(objectSpriteSheet, position1, sourceRectangle, white, (float) num6, origin, (float) num7, (SpriteEffects) num8, (float) num9);
        SpriteBatch spriteBatch2 = Game1.spriteBatch;
        SpriteFont dialogueFont = Game1.dialogueFont;
        string text = "x" + (object) this.shippingItems[this.shippingItems.Keys.ElementAt<ShippedItem>(index)] + " : " + (object) (this.shippingItems.Keys.ElementAt<ShippedItem>(index).price * this.shippingItems[this.shippingItems.Keys.ElementAt<ShippedItem>(index)]) + "g";
        double num10 = (double) (point.X + index * Game1.tileSize / (num2 - Game1.tileSize * 2) * Game1.tileSize * 4 + Game1.tileSize);
        double num11 = (double) (index * Game1.tileSize % (num2 - Game1.tileSize * 2) - index * Game1.tileSize % (num2 - Game1.tileSize * 2) % Game1.tileSize + Game1.tileSize / 2) - (double) Game1.dialogueFont.MeasureString("9").Y / 2.0;
        viewport = Game1.graphics.GraphicsDevice.Viewport;
        double y3 = (double) viewport.TitleSafeArea.Y;
        double num12 = num11 + y3 + (double) (Game1.tileSize * 3);
        Vector2 position2 = new Vector2((float) num10, (float) num12);
        Color textColor = Game1.textColor;
        spriteBatch2.DrawString(dialogueFont, text, position2, textColor);
      }
      if (!this.complete)
        return;
      SpriteBatch spriteBatch = Game1.spriteBatch;
      SpriteFont dialogueFont1 = Game1.dialogueFont;
      string text1 = Game1.content.LoadString("Strings\\StringsFromCSFiles:MoneyMadeScreen.cs.3854", (object) this.total);
      double num13 = (double) (x2 + width - Game1.tileSize) - (double) Game1.dialogueFont.MeasureString("Total: " + (object) this.total).X;
      viewport = Game1.graphics.GraphicsDevice.Viewport;
      double num14 = (double) (viewport.TitleSafeArea.Bottom - Game1.tileSize * 5 / 2);
      Vector2 position = new Vector2((float) num13, (float) num14);
      Color textColor1 = Game1.textColor;
      spriteBatch.DrawString(dialogueFont1, text1, position, textColor1);
    }
  }
}
