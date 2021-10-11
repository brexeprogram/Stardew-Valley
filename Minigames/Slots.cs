// Decompiled with JetBrains decompiler
// Type: StardewValley.Minigames.Slots
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Locations;
using StardewValley.Menus;
using System;
using System.Collections.Generic;

namespace StardewValley.Minigames
{
  public class Slots : IMinigame
  {
    public const float slotTurnRate = 0.008f;
    public const int numberOfIcons = 8;
    public const int defaultBet = 10;
    private string coinBuffer;
    private List<float> slots;
    private List<float> slotResults;
    private ClickableComponent spinButton10;
    private ClickableComponent spinButton100;
    private ClickableComponent doneButton;
    private Random r;
    private bool spinning;
    private bool showResult;
    private float payoutModifier;
    private int currentBet;
    private int spinsCount;
    private int slotsFinished;
    private int endTimer;
    public ClickableComponent currentlySnappedComponent;

    public Slots(int toBet = -1, bool highStakes = false)
    {
      this.coinBuffer = LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ru ? "     " : (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.zh ? "　　" : "  ");
      this.currentBet = toBet;
      if (this.currentBet == -1)
        this.currentBet = 10;
      this.slots = new List<float>();
      this.slots.Add(0.0f);
      this.slots.Add(0.0f);
      this.slots.Add(0.0f);
      this.slotResults = new List<float>();
      this.slotResults.Add(0.0f);
      this.slotResults.Add(0.0f);
      this.slotResults.Add(0.0f);
      Game1.playSound("newArtifact");
      this.r = new Random(Club.timesPlayedSlots + (int) Game1.stats.DaysPlayed + (int) Game1.uniqueIDForThisGame);
      this.setSlotResults(this.slots);
      Vector2 centeringOnScreen1 = Utility.getTopLeftPositionForCenteringOnScreen(26 * Game1.pixelZoom, 13 * Game1.pixelZoom, -4 * Game1.pixelZoom, Game1.tileSize / 2);
      this.spinButton10 = new ClickableComponent(new Rectangle((int) centeringOnScreen1.X, (int) centeringOnScreen1.Y, 26 * Game1.pixelZoom, 13 * Game1.pixelZoom), Game1.content.LoadString("Strings\\StringsFromCSFiles:Slots.cs.12117"));
      Vector2 centeringOnScreen2 = Utility.getTopLeftPositionForCenteringOnScreen(31 * Game1.pixelZoom, 13 * Game1.pixelZoom, -4 * Game1.pixelZoom, Game1.tileSize * 3 / 2);
      this.spinButton100 = new ClickableComponent(new Rectangle((int) centeringOnScreen2.X, (int) centeringOnScreen2.Y, 31 * Game1.pixelZoom, 13 * Game1.pixelZoom), Game1.content.LoadString("Strings\\StringsFromCSFiles:Slots.cs.12118"));
      Vector2 centeringOnScreen3 = Utility.getTopLeftPositionForCenteringOnScreen(24 * Game1.pixelZoom, 13 * Game1.pixelZoom, -4 * Game1.pixelZoom, Game1.tileSize * 5 / 2);
      this.doneButton = new ClickableComponent(new Rectangle((int) centeringOnScreen3.X, (int) centeringOnScreen3.Y, 24 * Game1.pixelZoom, 13 * Game1.pixelZoom), Game1.content.LoadString("Strings\\StringsFromCSFiles:NameSelect.cs.3864"));
      if (!Game1.isAnyGamePadButtonBeingPressed())
        return;
      Game1.setMousePosition(this.spinButton10.bounds.Center);
      if (!Game1.options.SnappyMenus)
        return;
      this.currentlySnappedComponent = this.spinButton10;
    }

    public void setSlotResults(List<float> toSet)
    {
      double num1 = this.r.NextDouble();
      double num2 = 0.858 + Game1.dailyLuck * 2.0 + (double) Game1.player.LuckLevel * 0.08;
      if (num1 < 5E-05 * num2)
      {
        this.set(toSet, 5);
        this.payoutModifier = 2500f;
      }
      else if (num1 < 0.0005 * num2)
      {
        this.set(toSet, 6);
        this.payoutModifier = 1000f;
      }
      else if (num1 < 0.001 * num2)
      {
        this.set(toSet, 7);
        this.payoutModifier = 500f;
      }
      else if (num1 < 0.002 * num2)
      {
        this.set(toSet, 4);
        this.payoutModifier = 200f;
      }
      else if (num1 < 0.004 * num2)
      {
        this.set(toSet, 3);
        this.payoutModifier = 120f;
      }
      else if (num1 < 0.006 * num2)
      {
        this.set(toSet, 2);
        this.payoutModifier = 80f;
      }
      else if (num1 < 0.01 * num2)
      {
        this.set(toSet, 1);
        this.payoutModifier = 30f;
      }
      else if (num1 < 0.03 * num2)
      {
        int num3 = this.r.Next(3);
        for (int index = 0; index < 3; ++index)
          toSet[index] = index == num3 ? (float) this.r.Next(7) : 7f;
        this.payoutModifier = 3f;
      }
      else if (num1 < 0.08 * num2)
      {
        this.set(toSet, 0);
        this.payoutModifier = 5f;
      }
      else if (num1 < 0.2 * num2)
      {
        int num3 = this.r.Next(3);
        for (int index = 0; index < 3; ++index)
          toSet[index] = index == num3 ? 7f : (float) this.r.Next(7);
        this.payoutModifier = 2f;
      }
      else
      {
        this.payoutModifier = 0.0f;
        int[] numArray = new int[8];
        for (int index1 = 0; index1 < 3; ++index1)
        {
          int index2 = this.r.Next(6);
          while (numArray[index2] > 1)
            index2 = this.r.Next(6);
          toSet[index1] = (float) index2;
          ++numArray[index2];
        }
      }
    }

    private void set(List<float> toSet, int number)
    {
      toSet[0] = (float) number;
      toSet[1] = (float) number;
      toSet[2] = (float) number;
    }

    public bool tick(GameTime time)
    {
      TimeSpan elapsedGameTime;
      if (this.spinning && this.endTimer <= 0)
      {
        for (int slotsFinished = this.slotsFinished; slotsFinished < this.slots.Count; ++slotsFinished)
        {
          float slot = this.slots[slotsFinished];
          List<float> slots1 = this.slots;
          int index1 = slotsFinished;
          List<float> floatList = slots1;
          int index2 = index1;
          double num1 = (double) slots1[index1];
          elapsedGameTime = time.ElapsedGameTime;
          double num2 = (double) elapsedGameTime.Milliseconds * 0.00800000037997961 * (1.0 - (double) slotsFinished * 0.0500000007450581);
          double num3 = num1 + num2;
          floatList[index2] = (float) num3;
          List<float> slots2 = this.slots;
          int index3 = slotsFinished;
          slots2[index3] = slots2[index3] % 8f;
          if (slotsFinished == 2)
          {
            if ((double) slot % (0.25 + (double) this.slotsFinished * 0.5) > (double) this.slots[slotsFinished] % (0.25 + (double) this.slotsFinished * 0.5))
              Game1.playSound("shiny4");
            if ((double) slot > (double) this.slots[slotsFinished])
              this.spinsCount = this.spinsCount + 1;
          }
          if (this.spinsCount > 0 && slotsFinished == this.slotsFinished)
          {
            double num4 = (double) Math.Abs(this.slots[slotsFinished] - this.slotResults[slotsFinished]);
            elapsedGameTime = time.ElapsedGameTime;
            double num5 = (double) elapsedGameTime.Milliseconds * 0.00800000037997961;
            if (num4 <= num5)
            {
              this.slots[slotsFinished] = this.slotResults[slotsFinished];
              this.slotsFinished = this.slotsFinished + 1;
              this.spinsCount = this.spinsCount - 1;
              Game1.playSound("Cowboy_gunshot");
            }
          }
        }
        if (this.slotsFinished >= 3)
          this.endTimer = (double) this.payoutModifier == 0.0 ? 600 : 1000;
      }
      if (this.endTimer > 0)
      {
        int endTimer = this.endTimer;
        elapsedGameTime = time.ElapsedGameTime;
        int milliseconds = elapsedGameTime.Milliseconds;
        this.endTimer = endTimer - milliseconds;
        if (this.endTimer <= 0)
        {
          this.spinning = false;
          this.spinsCount = 0;
          this.slotsFinished = 0;
          if ((double) this.payoutModifier > 0.0)
          {
            this.showResult = true;
            Game1.playSound((double) this.payoutModifier >= 5.0 ? ((double) this.payoutModifier >= 10.0 ? "reward" : "money") : "newArtifact");
          }
          else
            Game1.playSound("breathout");
          Game1.player.clubCoins += (int) ((double) this.currentBet * (double) this.payoutModifier);
        }
      }
      this.spinButton10.scale = this.spinning || !this.spinButton10.bounds.Contains(Game1.getOldMouseX(), Game1.getOldMouseY()) ? 1f : 1.05f;
      this.spinButton100.scale = this.spinning || !this.spinButton100.bounds.Contains(Game1.getOldMouseX(), Game1.getOldMouseY()) ? 1f : 1.05f;
      this.doneButton.scale = this.spinning || !this.doneButton.bounds.Contains(Game1.getOldMouseX(), Game1.getOldMouseY()) ? 1f : 1.05f;
      return false;
    }

    public void receiveLeftClick(int x, int y, bool playSound = true)
    {
      if (!this.spinning && Game1.player.clubCoins >= 10 && this.spinButton10.bounds.Contains(x, y))
      {
        ++Club.timesPlayedSlots;
        this.r = new Random(Club.timesPlayedSlots + (int) Game1.stats.DaysPlayed + (int) Game1.uniqueIDForThisGame);
        this.setSlotResults(this.slotResults);
        this.spinning = true;
        Game1.playSound("bigSelect");
        this.currentBet = 10;
        this.slotsFinished = 0;
        this.spinsCount = 0;
        this.showResult = false;
        Game1.player.clubCoins -= 10;
      }
      if (!this.spinning && Game1.player.clubCoins >= 100 && this.spinButton100.bounds.Contains(x, y))
      {
        ++Club.timesPlayedSlots;
        this.r = new Random(Club.timesPlayedSlots + (int) Game1.stats.DaysPlayed + (int) Game1.uniqueIDForThisGame);
        this.setSlotResults(this.slotResults);
        Game1.playSound("bigSelect");
        this.spinning = true;
        this.slotsFinished = 0;
        this.spinsCount = 0;
        this.showResult = false;
        this.currentBet = 100;
        Game1.player.clubCoins -= 100;
      }
      if (this.spinning || !this.doneButton.bounds.Contains(x, y))
        return;
      Game1.playSound("bigDeSelect");
      Game1.currentMinigame = (IMinigame) null;
    }

    public void leftClickHeld(int x, int y)
    {
    }

    public void receiveRightClick(int x, int y, bool playSound = true)
    {
    }

    public void releaseLeftClick(int x, int y)
    {
    }

    public void releaseRightClick(int x, int y)
    {
    }

    public bool overrideFreeMouseMovement()
    {
      return Game1.options.SnappyMenus;
    }

    public void receiveKeyPress(Keys k)
    {
      if (!this.spinning && (k.Equals((object) Keys.Escape) || Game1.options.doesInputListContain(Game1.options.menuButton, k)))
      {
        this.unload();
        Game1.playSound("bigDeSelect");
        Game1.currentMinigame = (IMinigame) null;
      }
      else
      {
        if (this.spinning || this.currentlySnappedComponent == null)
          return;
        if (Game1.options.doesInputListContain(Game1.options.moveDownButton, k))
        {
          if (this.currentlySnappedComponent.Equals((object) this.spinButton10))
          {
            this.currentlySnappedComponent = this.spinButton100;
            Game1.setMousePosition(this.currentlySnappedComponent.bounds.Center);
          }
          else
          {
            if (!this.currentlySnappedComponent.Equals((object) this.spinButton100))
              return;
            this.currentlySnappedComponent = this.doneButton;
            Game1.setMousePosition(this.currentlySnappedComponent.bounds.Center);
          }
        }
        else
        {
          if (!Game1.options.doesInputListContain(Game1.options.moveUpButton, k))
            return;
          if (this.currentlySnappedComponent.Equals((object) this.doneButton))
          {
            this.currentlySnappedComponent = this.spinButton100;
            Game1.setMousePosition(this.currentlySnappedComponent.bounds.Center);
          }
          else
          {
            if (!this.currentlySnappedComponent.Equals((object) this.spinButton100))
              return;
            this.currentlySnappedComponent = this.spinButton10;
            Game1.setMousePosition(this.currentlySnappedComponent.bounds.Center);
          }
        }
      }
    }

    public void receiveKeyRelease(Keys k)
    {
    }

    public int getIconIndex(int index)
    {
      switch (index)
      {
        case 0:
          return 24;
        case 1:
          return 186;
        case 2:
          return 138;
        case 3:
          return 392;
        case 4:
          return 254;
        case 5:
          return 434;
        case 6:
          return 72;
        case 7:
          return 638;
        default:
          return 24;
      }
    }

    public void draw(SpriteBatch b)
    {
      b.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
      SpriteBatch spriteBatch1 = b;
      Texture2D staminaRect = Game1.staminaRect;
      int x1 = 0;
      int y1 = 0;
      Viewport viewport1 = Game1.graphics.GraphicsDevice.Viewport;
      int width1 = viewport1.Width;
      viewport1 = Game1.graphics.GraphicsDevice.Viewport;
      int height1 = viewport1.Height;
      Rectangle destinationRectangle = new Rectangle(x1, y1, width1, height1);
      Color color1 = new Color(38, 0, 7);
      spriteBatch1.Draw(staminaRect, destinationRectangle, color1);
      b.Draw(Game1.mouseCursors, Utility.getTopLeftPositionForCenteringOnScreen(57 * Game1.pixelZoom, 13 * Game1.pixelZoom, 0, -4 * Game1.tileSize), new Rectangle?(new Rectangle(441, 424, 57, 13)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.99f);
      Viewport viewport2;
      for (int index = 0; index < 3; ++index)
      {
        SpriteBatch spriteBatch2 = b;
        Texture2D mouseCursors1 = Game1.mouseCursors;
        viewport2 = Game1.graphics.GraphicsDevice.Viewport;
        double num1 = (double) (viewport2.Width / 2 - 28 * Game1.pixelZoom + index * 26 * Game1.pixelZoom);
        viewport2 = Game1.graphics.GraphicsDevice.Viewport;
        double num2 = (double) (viewport2.Height / 2 - 32 * Game1.pixelZoom);
        Vector2 position1 = new Vector2((float) num1, (float) num2);
        Rectangle? sourceRectangle1 = new Rectangle?(new Rectangle(306, 320, 16, 16));
        Color white1 = Color.White;
        double num3 = 0.0;
        Vector2 zero1 = Vector2.Zero;
        double pixelZoom1 = (double) Game1.pixelZoom;
        int num4 = 0;
        double num5 = 0.990000009536743;
        spriteBatch2.Draw(mouseCursors1, position1, sourceRectangle1, white1, (float) num3, zero1, (float) pixelZoom1, (SpriteEffects) num4, (float) num5);
        float num6 = (float) (((double) this.slots[index] + 1.0) % 8.0);
        int iconIndex1 = this.getIconIndex(((int) num6 + 8 - 1) % 8);
        int iconIndex2 = this.getIconIndex((iconIndex1 + 1) % 8);
        SpriteBatch spriteBatch3 = b;
        Texture2D objectSpriteSheet1 = Game1.objectSpriteSheet;
        viewport2 = Game1.graphics.GraphicsDevice.Viewport;
        double num7 = (double) (viewport2.Width / 2 - 28 * Game1.pixelZoom + index * 26 * Game1.pixelZoom);
        viewport2 = Game1.graphics.GraphicsDevice.Viewport;
        double num8 = (double) (viewport2.Height / 2 - 32 * Game1.pixelZoom);
        Vector2 position2 = new Vector2((float) num7, (float) num8) - new Vector2(0.0f, (float) -Game1.tileSize * (num6 % 1f));
        Rectangle? sourceRectangle2 = new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, iconIndex1, 16, 16));
        Color white2 = Color.White;
        double num9 = 0.0;
        Vector2 zero2 = Vector2.Zero;
        double pixelZoom2 = (double) Game1.pixelZoom;
        int num10 = 0;
        double num11 = 0.990000009536743;
        spriteBatch3.Draw(objectSpriteSheet1, position2, sourceRectangle2, white2, (float) num9, zero2, (float) pixelZoom2, (SpriteEffects) num10, (float) num11);
        SpriteBatch spriteBatch4 = b;
        Texture2D objectSpriteSheet2 = Game1.objectSpriteSheet;
        viewport2 = Game1.graphics.GraphicsDevice.Viewport;
        double num12 = (double) (viewport2.Width / 2 - 28 * Game1.pixelZoom + index * 26 * Game1.pixelZoom);
        viewport2 = Game1.graphics.GraphicsDevice.Viewport;
        double num13 = (double) (viewport2.Height / 2 - 32 * Game1.pixelZoom);
        Vector2 position3 = new Vector2((float) num12, (float) num13) - new Vector2(0.0f, (float) Game1.tileSize - (float) Game1.tileSize * (num6 % 1f));
        Rectangle? sourceRectangle3 = new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, iconIndex2, 16, 16));
        Color white3 = Color.White;
        double num14 = 0.0;
        Vector2 zero3 = Vector2.Zero;
        double pixelZoom3 = (double) Game1.pixelZoom;
        int num15 = 0;
        double num16 = 0.990000009536743;
        spriteBatch4.Draw(objectSpriteSheet2, position3, sourceRectangle3, white3, (float) num14, zero3, (float) pixelZoom3, (SpriteEffects) num15, (float) num16);
        SpriteBatch spriteBatch5 = b;
        Texture2D mouseCursors2 = Game1.mouseCursors;
        viewport2 = Game1.graphics.GraphicsDevice.Viewport;
        double num17 = (double) (viewport2.Width / 2 - 33 * Game1.pixelZoom + index * 26 * Game1.pixelZoom);
        viewport2 = Game1.graphics.GraphicsDevice.Viewport;
        double num18 = (double) (viewport2.Height / 2 - 48 * Game1.pixelZoom);
        Vector2 position4 = new Vector2((float) num17, (float) num18);
        Rectangle? sourceRectangle4 = new Rectangle?(new Rectangle(415, 385, 26, 48));
        Color white4 = Color.White;
        double num19 = 0.0;
        Vector2 zero4 = Vector2.Zero;
        double pixelZoom4 = (double) Game1.pixelZoom;
        int num20 = 0;
        double num21 = 0.990000009536743;
        spriteBatch5.Draw(mouseCursors2, position4, sourceRectangle4, white4, (float) num19, zero4, (float) pixelZoom4, (SpriteEffects) num20, (float) num21);
      }
      if (this.showResult)
      {
        SpriteBatch b1 = b;
        string s = "+" + (object) (float) ((double) this.payoutModifier * (double) this.currentBet);
        viewport2 = Game1.graphics.GraphicsDevice.Viewport;
        int x2 = viewport2.Width / 2 - 93 * Game1.pixelZoom;
        int y2 = this.spinButton10.bounds.Y - Game1.tileSize + Game1.pixelZoom * 2;
        int characterPosition = 9999;
        int width2 = -1;
        int height2 = 9999;
        double num1 = 1.0;
        double num2 = 1.0;
        int num3 = 0;
        int drawBGScroll = -1;
        string placeHolderScrollWidthText = "";
        int color2 = 4;
        SpriteText.drawString(b1, s, x2, y2, characterPosition, width2, height2, (float) num1, (float) num2, num3 != 0, drawBGScroll, placeHolderScrollWidthText, color2);
      }
      b.Draw(Game1.mouseCursors, new Vector2((float) this.spinButton10.bounds.X, (float) this.spinButton10.bounds.Y), new Rectangle?(new Rectangle(441, 385, 26, 13)), Color.White * (this.spinning || Game1.player.clubCoins < 10 ? 0.5f : 1f), 0.0f, Vector2.Zero, (float) Game1.pixelZoom * this.spinButton10.scale, SpriteEffects.None, 0.99f);
      b.Draw(Game1.mouseCursors, new Vector2((float) this.spinButton100.bounds.X, (float) this.spinButton100.bounds.Y), new Rectangle?(new Rectangle(441, 398, 31, 13)), Color.White * (this.spinning || Game1.player.clubCoins < 100 ? 0.5f : 1f), 0.0f, Vector2.Zero, (float) Game1.pixelZoom * this.spinButton100.scale, SpriteEffects.None, 0.99f);
      b.Draw(Game1.mouseCursors, new Vector2((float) this.doneButton.bounds.X, (float) this.doneButton.bounds.Y), new Rectangle?(new Rectangle(441, 411, 24, 13)), Color.White * (!this.spinning ? 1f : 0.5f), 0.0f, Vector2.Zero, (float) Game1.pixelZoom * this.doneButton.scale, SpriteEffects.None, 0.99f);
      SpriteBatch b2 = b;
      string s1 = this.coinBuffer + (object) Game1.player.clubCoins;
      viewport2 = Game1.graphics.GraphicsDevice.Viewport;
      int x3 = viewport2.Width / 2 - 94 * Game1.pixelZoom;
      viewport2 = Game1.graphics.GraphicsDevice.Viewport;
      int y3 = viewport2.Height / 2 - 30 * Game1.pixelZoom;
      string placeHolderWidthText = "";
      double num22 = 1.0;
      int color3 = -1;
      SpriteText.drawStringWithScrollBackground(b2, s1, x3, y3, placeHolderWidthText, (float) num22, color3);
      SpriteBatch b3 = b;
      Texture2D mouseCursors = Game1.mouseCursors;
      viewport2 = Game1.graphics.GraphicsDevice.Viewport;
      double num23 = (double) (viewport2.Width / 2 - 94 * Game1.pixelZoom + Game1.pixelZoom);
      viewport2 = Game1.graphics.GraphicsDevice.Viewport;
      double num24 = (double) (viewport2.Height / 2 - 30 * Game1.pixelZoom + Game1.pixelZoom);
      Vector2 position = new Vector2((float) num23, (float) num24);
      Rectangle sourceRect = new Rectangle(211, 373, 9, 10);
      Color white = Color.White;
      double num25 = 0.0;
      Vector2 zero = Vector2.Zero;
      double pixelZoom = (double) Game1.pixelZoom;
      int num26 = 0;
      double num27 = 1.0;
      int horizontalShadowOffset = -1;
      int verticalShadowOffset = -1;
      double num28 = 0.349999994039536;
      Utility.drawWithShadow(b3, mouseCursors, position, sourceRect, white, (float) num25, zero, (float) pixelZoom, num26 != 0, (float) num27, horizontalShadowOffset, verticalShadowOffset, (float) num28);
      Vector2 vector2;
      // ISSUE: explicit reference operation
      // ISSUE: variable of a reference type
      Vector2& local = @vector2;
      viewport2 = Game1.graphics.GraphicsDevice.Viewport;
      double num29 = (double) (viewport2.Width / 2 + 50 * Game1.pixelZoom);
      viewport2 = Game1.graphics.GraphicsDevice.Viewport;
      double num30 = (double) (viewport2.Height / 2 - 88 * Game1.pixelZoom);
      // ISSUE: explicit reference operation
      ^local = new Vector2((float) num29, (float) num30);
      IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(375, 357, 3, 3), (int) vector2.X, (int) vector2.Y, Game1.tileSize * 6, Game1.tileSize * 11, Color.White, (float) Game1.pixelZoom, true);
      b.Draw(Game1.objectSpriteSheet, vector2 + new Vector2((float) (Game1.pixelZoom * 2), (float) (Game1.pixelZoom * 2)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.getIconIndex(7), 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.99f);
      SpriteText.drawString(b, "x2", (int) vector2.X + Game1.tileSize * 3 + Game1.pixelZoom * 4, (int) vector2.Y + Game1.pixelZoom * 6, 9999, -1, 99999, 1f, 0.88f, false, -1, "", 4);
      b.Draw(Game1.objectSpriteSheet, vector2 + new Vector2((float) (Game1.pixelZoom * 2), (float) (Game1.pixelZoom * 2 + (Game1.tileSize + Game1.pixelZoom))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.getIconIndex(7), 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.99f);
      b.Draw(Game1.objectSpriteSheet, vector2 + new Vector2((float) (Game1.pixelZoom * 2 + (Game1.tileSize + Game1.pixelZoom)), (float) (Game1.pixelZoom * 2 + (Game1.tileSize + Game1.pixelZoom))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.getIconIndex(7), 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.99f);
      SpriteText.drawString(b, "x3", (int) vector2.X + Game1.tileSize * 3 + Game1.pixelZoom * 4, (int) vector2.Y + (Game1.tileSize + Game1.pixelZoom) + Game1.pixelZoom * 6, 9999, -1, 99999, 1f, 0.88f, false, -1, "", 4);
      for (int index1 = 0; index1 < 8; ++index1)
      {
        int index2 = index1;
        if (index1 == 5)
          index2 = 7;
        else if (index1 == 7)
          index2 = 5;
        b.Draw(Game1.objectSpriteSheet, vector2 + new Vector2((float) (Game1.pixelZoom * 2), (float) (Game1.pixelZoom * 2 + (index1 + 2) * (Game1.tileSize + Game1.pixelZoom))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.getIconIndex(index2), 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.99f);
        b.Draw(Game1.objectSpriteSheet, vector2 + new Vector2((float) (Game1.pixelZoom * 2 + (Game1.tileSize + Game1.pixelZoom)), (float) (Game1.pixelZoom * 2 + (index1 + 2) * (Game1.tileSize + Game1.pixelZoom))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.getIconIndex(index2), 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.99f);
        b.Draw(Game1.objectSpriteSheet, vector2 + new Vector2((float) (Game1.pixelZoom * 2 + 2 * (Game1.tileSize + Game1.pixelZoom)), (float) (Game1.pixelZoom * 2 + (index1 + 2) * (Game1.tileSize + Game1.pixelZoom))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.getIconIndex(index2), 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.99f);
        int num1 = 0;
        switch (index1)
        {
          case 0:
            num1 = 5;
            break;
          case 1:
            num1 = 30;
            break;
          case 2:
            num1 = 80;
            break;
          case 3:
            num1 = 120;
            break;
          case 4:
            num1 = 200;
            break;
          case 5:
            num1 = 500;
            break;
          case 6:
            num1 = 1000;
            break;
          case 7:
            num1 = 2500;
            break;
        }
        SpriteText.drawString(b, "x" + (object) num1, (int) vector2.X + Game1.tileSize * 3 + Game1.pixelZoom * 4, (int) vector2.Y + (index1 + 2) * (Game1.tileSize + Game1.pixelZoom) + Game1.pixelZoom * 6, 9999, -1, 99999, 1f, 0.88f, false, -1, "", 4);
      }
      IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(379, 357, 3, 3), (int) vector2.X - Game1.tileSize * 10, (int) vector2.Y, Game1.tileSize * 16, Game1.tileSize * 11, Color.Red, (float) Game1.pixelZoom, false);
      for (int index = 1; index < 8; ++index)
        IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(379, 357, 3, 3), (int) vector2.X - Game1.tileSize * 10 - Game1.pixelZoom * index, (int) vector2.Y - Game1.pixelZoom * index, Game1.tileSize * 16 + Game1.pixelZoom * 2 * index, Game1.tileSize * 11 + Game1.pixelZoom * 2 * index, Color.Red * (float) (1.0 - (double) index * 0.150000005960464), (float) Game1.pixelZoom, false);
      for (int index = 0; index < 17; ++index)
        IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(147, 472, 3, 3), (int) vector2.X - Game1.tileSize * 10 + Game1.pixelZoom * 2, (int) vector2.Y + index * Game1.pixelZoom * 3 + Game1.pixelZoom * 3, (int) ((double) Game1.tileSize * 9.5 - (double) (index * Game1.tileSize) * 1.20000004768372 + (double) (index * index * Game1.pixelZoom) * 0.699999988079071), Game1.pixelZoom, new Color(index * 25, index > 8 ? index * 10 : 0, (int) byte.MaxValue - index * 25), (float) Game1.pixelZoom, false);
      if (!Game1.options.hardwareCursor)
        b.Draw(Game1.mouseCursors, new Vector2((float) Game1.getMouseX(), (float) Game1.getMouseY()), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 0, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
      b.End();
    }

    public void changeScreenSize()
    {
    }

    public void unload()
    {
    }

    public void receiveEventPoke(int data)
    {
    }

    public string minigameId()
    {
      return nameof (Slots);
    }
  }
}
