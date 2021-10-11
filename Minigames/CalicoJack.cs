// Decompiled with JetBrains decompiler
// Type: StardewValley.Minigames.CalicoJack
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
using System.Linq;

namespace StardewValley.Minigames
{
  public class CalicoJack : IMinigame
  {
    private int dealerTurnTimer = -1;
    private string endMessage = "";
    private string endTitle = "";
    public const int cardState_flipped = -1;
    public const int cardState_up = 0;
    public const int cardState_transitioning = 400;
    public const int bet = 100;
    public const int cardWidth = 96;
    public const int dealTime = 1000;
    public const int playingTo = 21;
    public const int passNumber = 18;
    public const int dealerTurnDelay = 1000;
    public List<int[]> playerCards;
    public List<int[]> dealerCards;
    private Random r;
    private int currentBet;
    private int startTimer;
    private int bustTimer;
    private ClickableComponent hit;
    private ClickableComponent stand;
    private ClickableComponent doubleOrNothing;
    private ClickableComponent playAgain;
    private ClickableComponent quit;
    private ClickableComponent currentlySnappedComponent;
    private bool showingResultsScreen;
    private bool playerWon;
    private bool highStakes;
    private string coinBuffer;

    public CalicoJack(int toBet = -1, bool highStakes = false)
    {
      this.coinBuffer = LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ru ? "     " : (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.zh ? "　　" : "  ");
      this.highStakes = highStakes;
      this.startTimer = 1000;
      this.playerCards = new List<int[]>();
      this.dealerCards = new List<int[]>();
      this.currentBet = toBet != -1 ? toBet : (highStakes ? 1000 : 100);
      ++Club.timesPlayedCalicoJack;
      this.r = new Random(Club.timesPlayedCalicoJack + (int) Game1.stats.DaysPlayed + (int) Game1.uniqueIDForThisGame);
      Viewport viewport = Game1.graphics.GraphicsDevice.Viewport;
      int x1 = viewport.Width - Game1.tileSize * 2 - SpriteText.getWidthOfString(Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11924"));
      viewport = Game1.graphics.GraphicsDevice.Viewport;
      int y1 = viewport.Height / 2 - Game1.tileSize;
      int widthOfString1 = SpriteText.getWidthOfString(Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11924") + "  ");
      int tileSize1 = Game1.tileSize;
      this.hit = new ClickableComponent(new Rectangle(x1, y1, widthOfString1, tileSize1), "", " " + Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11924") + " ");
      viewport = Game1.graphics.GraphicsDevice.Viewport;
      int x2 = viewport.Width - Game1.tileSize * 2 - SpriteText.getWidthOfString(Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11927"));
      viewport = Game1.graphics.GraphicsDevice.Viewport;
      int y2 = viewport.Height / 2 + Game1.tileSize / 2;
      int widthOfString2 = SpriteText.getWidthOfString(Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11927") + "  ");
      int tileSize2 = Game1.tileSize;
      this.stand = new ClickableComponent(new Rectangle(x2, y2, widthOfString2, tileSize2), "", " " + Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11927") + " ");
      viewport = Game1.graphics.GraphicsDevice.Viewport;
      int x3 = viewport.Width / 2 - SpriteText.getWidthOfString(Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11930")) / 2;
      viewport = Game1.graphics.GraphicsDevice.Viewport;
      int y3 = viewport.Height / 2;
      int width1 = SpriteText.getWidthOfString(Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11930")) + Game1.tileSize;
      int tileSize3 = Game1.tileSize;
      this.doubleOrNothing = new ClickableComponent(new Rectangle(x3, y3, width1, tileSize3), "", Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11930"));
      viewport = Game1.graphics.GraphicsDevice.Viewport;
      int x4 = viewport.Width / 2 - SpriteText.getWidthOfString(Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11933")) / 2;
      viewport = Game1.graphics.GraphicsDevice.Viewport;
      int y4 = viewport.Height / 2 + Game1.tileSize + Game1.tileSize / 4;
      int width2 = SpriteText.getWidthOfString(Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11933")) + Game1.tileSize;
      int tileSize4 = Game1.tileSize;
      this.playAgain = new ClickableComponent(new Rectangle(x4, y4, width2, tileSize4), "", Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11933"));
      viewport = Game1.graphics.GraphicsDevice.Viewport;
      int x5 = viewport.Width / 2 - SpriteText.getWidthOfString(Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11936")) / 2;
      viewport = Game1.graphics.GraphicsDevice.Viewport;
      int y5 = viewport.Height / 2 + Game1.tileSize + Game1.tileSize * 3 / 2;
      int width3 = SpriteText.getWidthOfString(Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11936")) + Game1.tileSize;
      int tileSize5 = Game1.tileSize;
      this.quit = new ClickableComponent(new Rectangle(x5, y5, width3, tileSize5), "", Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11936"));
      if (!Game1.options.SnappyMenus)
        return;
      this.currentlySnappedComponent = this.hit;
      this.currentlySnappedComponent.snapMouseCursorToCenter();
    }

    public bool overrideFreeMouseMovement()
    {
      return Game1.options.SnappyMenus;
    }

    public bool playButtonsActive()
    {
      if (this.startTimer <= 0 && this.dealerTurnTimer < 0)
        return !this.showingResultsScreen;
      return false;
    }

    public bool tick(GameTime time)
    {
      TimeSpan elapsedGameTime;
      for (int index = 0; index < this.playerCards.Count; ++index)
      {
        if (this.playerCards[index][1] > 0)
        {
          // ISSUE: explicit reference operation
          // ISSUE: variable of a reference type
          int& local = @this.playerCards[index][1];
          // ISSUE: explicit reference operation
          int num1 = ^local;
          elapsedGameTime = time.ElapsedGameTime;
          int milliseconds = elapsedGameTime.Milliseconds;
          int num2 = num1 - milliseconds;
          // ISSUE: explicit reference operation
          ^local = num2;
          if (this.playerCards[index][1] <= 0)
            this.playerCards[index][1] = 0;
        }
      }
      for (int index = 0; index < this.dealerCards.Count; ++index)
      {
        if (this.dealerCards[index][1] > 0)
        {
          // ISSUE: explicit reference operation
          // ISSUE: variable of a reference type
          int& local = @this.dealerCards[index][1];
          // ISSUE: explicit reference operation
          int num1 = ^local;
          elapsedGameTime = time.ElapsedGameTime;
          int milliseconds = elapsedGameTime.Milliseconds;
          int num2 = num1 - milliseconds;
          // ISSUE: explicit reference operation
          ^local = num2;
          if (this.dealerCards[index][1] <= 0)
            this.dealerCards[index][1] = 0;
        }
      }
      if (this.startTimer > 0)
      {
        int startTimer1 = this.startTimer;
        int startTimer2 = this.startTimer;
        elapsedGameTime = time.ElapsedGameTime;
        int milliseconds = elapsedGameTime.Milliseconds;
        this.startTimer = startTimer2 - milliseconds;
        if (startTimer1 % 250 < this.startTimer % 250)
        {
          switch (startTimer1 / 250)
          {
            case 1:
              this.playerCards.Add(new int[2]
              {
                this.r.Next(1, 10),
                400
              });
              break;
            case 2:
              this.playerCards.Add(new int[2]
              {
                this.r.Next(1, 12),
                400
              });
              break;
            case 3:
              this.dealerCards.Add(new int[2]
              {
                this.r.Next(1, 10),
                400
              });
              break;
            case 4:
              this.dealerCards.Add(new int[2]
              {
                this.r.Next(1, 12),
                -1
              });
              break;
          }
          Game1.playSound("shwip");
        }
      }
      else if (this.bustTimer > 0)
      {
        int bustTimer = this.bustTimer;
        elapsedGameTime = time.ElapsedGameTime;
        int milliseconds = elapsedGameTime.Milliseconds;
        this.bustTimer = bustTimer - milliseconds;
        if (this.bustTimer <= 0)
          this.endGame();
      }
      else if (this.dealerTurnTimer > 0 && !this.showingResultsScreen)
      {
        int dealerTurnTimer = this.dealerTurnTimer;
        elapsedGameTime = time.ElapsedGameTime;
        int milliseconds = elapsedGameTime.Milliseconds;
        this.dealerTurnTimer = dealerTurnTimer - milliseconds;
        if (this.dealerTurnTimer <= 0)
        {
          int num1 = 0;
          foreach (int[] dealerCard in this.dealerCards)
            num1 += dealerCard[0];
          int num2 = 0;
          foreach (int[] playerCard in this.playerCards)
            num2 += playerCard[0];
          if (this.dealerCards[0][1] == -1)
          {
            this.dealerCards[0][1] = 400;
            Game1.playSound("shwip");
          }
          else if (num1 < 18 || num1 < num2 && num2 <= 21)
          {
            this.dealerCards.Add(new int[2]
            {
              this.r.Next(1, 10),
              400
            });
            int num3 = num1 + this.dealerCards.Last<int[]>()[0];
            Game1.playSound("shwip");
            if (num3 > 21)
              this.bustTimer = 2000;
          }
          else
            this.bustTimer = 50;
          this.dealerTurnTimer = 1000;
        }
      }
      if (this.playButtonsActive())
      {
        this.hit.scale = this.hit.bounds.Contains(Game1.getOldMouseX(), Game1.getOldMouseY()) ? 1.25f : 1f;
        this.stand.scale = this.stand.bounds.Contains(Game1.getOldMouseX(), Game1.getOldMouseY()) ? 1.25f : 1f;
      }
      else if (this.showingResultsScreen)
      {
        this.doubleOrNothing.scale = this.doubleOrNothing.bounds.Contains(Game1.getOldMouseX(), Game1.getOldMouseY()) ? 1.25f : 1f;
        this.playAgain.scale = this.playAgain.bounds.Contains(Game1.getOldMouseX(), Game1.getOldMouseY()) ? 1.25f : 1f;
        this.quit.scale = this.quit.bounds.Contains(Game1.getOldMouseX(), Game1.getOldMouseY()) ? 1.25f : 1f;
      }
      return false;
    }

    public void endGame()
    {
      if (Game1.options.SnappyMenus)
      {
        this.currentlySnappedComponent = this.quit;
        this.currentlySnappedComponent.snapMouseCursorToCenter();
      }
      this.showingResultsScreen = true;
      int num1 = 0;
      foreach (int[] playerCard in this.playerCards)
        num1 += playerCard[0];
      if (num1 == 21)
      {
        Game1.playSound("reward");
        this.playerWon = true;
        this.endTitle = Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11943");
        this.endMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11944");
        Game1.player.clubCoins += this.currentBet;
      }
      else if (num1 > 21)
      {
        Game1.playSound("fishEscape");
        this.endTitle = Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11946");
        this.endMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11947");
        Game1.player.clubCoins -= this.currentBet;
      }
      else
      {
        int num2 = 0;
        foreach (int[] dealerCard in this.dealerCards)
          num2 += dealerCard[0];
        if (num2 > 21)
        {
          Game1.playSound("reward");
          this.playerWon = true;
          this.endTitle = Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11943");
          this.endMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11950");
          Game1.player.clubCoins += this.currentBet;
        }
        else if (num1 == num2)
        {
          this.endTitle = Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11951");
          this.endMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11952");
        }
        else if (num1 > num2)
        {
          Game1.playSound("reward");
          this.endTitle = Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11943");
          this.endMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11955", (object) 21);
          Game1.player.clubCoins += this.currentBet;
          this.playerWon = true;
        }
        else
        {
          Game1.playSound("fishEscape");
          this.endTitle = Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11946");
          this.endMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11958", (object) 21);
          Game1.player.clubCoins -= this.currentBet;
        }
      }
    }

    public void receiveLeftClick(int x, int y, bool playSound = true)
    {
      if (this.playButtonsActive() && this.bustTimer <= 0)
      {
        if (this.hit.bounds.Contains(x, y))
        {
          this.playerCards.Add(new int[2]
          {
            this.r.Next(1, 10),
            400
          });
          Game1.playSound("shwip");
          int num = 0;
          foreach (int[] playerCard in this.playerCards)
            num += playerCard[0];
          if (num == 21)
            this.bustTimer = 1000;
          else if (num > 21)
            this.bustTimer = 1000;
        }
        if (!this.stand.bounds.Contains(x, y))
          return;
        this.dealerTurnTimer = 1000;
        Game1.playSound("coin");
      }
      else
      {
        if (!this.showingResultsScreen)
          return;
        if (this.playerWon && this.doubleOrNothing.containsPoint(x, y))
        {
          Game1.currentMinigame = (IMinigame) new CalicoJack(this.currentBet * 2, this.highStakes);
          Game1.playSound("bigSelect");
        }
        if (Game1.player.clubCoins >= this.currentBet && this.playAgain.containsPoint(x, y))
        {
          Game1.currentMinigame = (IMinigame) new CalicoJack(-1, this.highStakes);
          Game1.playSound("smallSelect");
        }
        if (!this.quit.containsPoint(x, y))
          return;
        Game1.currentMinigame = (IMinigame) null;
        Game1.playSound("bigDeSelect");
      }
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

    public void receiveKeyPress(Keys k)
    {
      if (!Game1.options.SnappyMenus)
        return;
      if (Game1.options.doesInputListContain(Game1.options.moveUpButton, k))
      {
        if (this.currentlySnappedComponent.Equals((object) this.stand))
          this.currentlySnappedComponent = this.hit;
        else if (this.currentlySnappedComponent.Equals((object) this.playAgain) && this.playerWon)
          this.currentlySnappedComponent = this.doubleOrNothing;
        else if (this.currentlySnappedComponent.Equals((object) this.quit) && Game1.player.clubCoins >= this.currentBet)
          this.currentlySnappedComponent = this.playAgain;
      }
      else if (Game1.options.doesInputListContain(Game1.options.moveDownButton, k))
      {
        if (this.currentlySnappedComponent.Equals((object) this.hit))
          this.currentlySnappedComponent = this.stand;
        else if (this.currentlySnappedComponent.Equals((object) this.doubleOrNothing))
          this.currentlySnappedComponent = this.playAgain;
        else if (this.currentlySnappedComponent.Equals((object) this.playAgain))
          this.currentlySnappedComponent = this.quit;
      }
      if (this.currentlySnappedComponent == null)
        return;
      this.currentlySnappedComponent.snapMouseCursorToCenter();
    }

    public void receiveKeyRelease(Keys k)
    {
    }

    public void draw(SpriteBatch b)
    {
      b.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
      SpriteBatch spriteBatch = b;
      Texture2D staminaRect = Game1.staminaRect;
      int x1 = 0;
      int y1 = 0;
      Viewport viewport1 = Game1.graphics.GraphicsDevice.Viewport;
      int width = viewport1.Width;
      viewport1 = Game1.graphics.GraphicsDevice.Viewport;
      int height1 = viewport1.Height;
      Rectangle destinationRectangle = new Rectangle(x1, y1, width, height1);
      Color color1 = this.highStakes ? new Color(130, 0, 82) : Color.DarkGreen;
      spriteBatch.Draw(staminaRect, destinationRectangle, color1);
      if (this.showingResultsScreen)
      {
        SpriteBatch b1 = b;
        string endMessage = this.endMessage;
        Viewport viewport2 = Game1.graphics.GraphicsDevice.Viewport;
        int x2 = viewport2.Width / 2;
        int y2 = Game1.tileSize * 3 / 4;
        string placeHolderWidthText1 = "";
        double num1 = 1.0;
        int color2 = -1;
        int scrollType1 = 0;
        double num2 = 0.879999995231628;
        int num3 = 0;
        SpriteText.drawStringWithScrollCenteredAt(b1, endMessage, x2, y2, placeHolderWidthText1, (float) num1, color2, scrollType1, (float) num2, num3 != 0);
        SpriteBatch b2 = b;
        string endTitle = this.endTitle;
        viewport2 = Game1.graphics.GraphicsDevice.Viewport;
        int x3 = viewport2.Width / 2;
        int y3 = Game1.tileSize * 2;
        string placeHolderWidthText2 = "";
        double num4 = 1.0;
        int color3 = -1;
        int scrollType2 = 0;
        double num5 = 0.879999995231628;
        int num6 = 0;
        SpriteText.drawStringWithScrollCenteredAt(b2, endTitle, x3, y3, placeHolderWidthText2, (float) num4, color3, scrollType2, (float) num5, num6 != 0);
        if (!this.endTitle.Equals(Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11951")))
        {
          SpriteBatch b3 = b;
          string s = Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11965", (object) ((this.playerWon ? (object) "" : (object) "-").ToString() + (object) this.currentBet + "   "));
          viewport2 = Game1.graphics.GraphicsDevice.Viewport;
          int x4 = viewport2.Width / 2;
          int y4 = Game1.tileSize * 4;
          string placeHolderWidthText3 = "";
          double num7 = 1.0;
          int color4 = -1;
          int scrollType3 = 0;
          double num8 = 0.879999995231628;
          int num9 = 0;
          SpriteText.drawStringWithScrollCenteredAt(b3, s, x4, y4, placeHolderWidthText3, (float) num7, color4, scrollType3, (float) num8, num9 != 0);
          SpriteBatch b4 = b;
          Texture2D mouseCursors = Game1.mouseCursors;
          viewport2 = Game1.graphics.GraphicsDevice.Viewport;
          Vector2 position = new Vector2((float) (viewport2.Width / 2 - Game1.tileSize / 2 + SpriteText.getWidthOfString(Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11965", (object) ((this.playerWon ? (object) "" : (object) "-").ToString() + (object) this.currentBet + "   "))) / 2), (float) (Game1.tileSize * 4 + Game1.pixelZoom)) + new Vector2((float) (Game1.pixelZoom * 2), 0.0f);
          Rectangle sourceRect = new Rectangle(211, 373, 9, 10);
          Color white = Color.White;
          double num10 = 0.0;
          Vector2 zero = Vector2.Zero;
          double pixelZoom = (double) Game1.pixelZoom;
          int num11 = 0;
          double num12 = 1.0;
          int horizontalShadowOffset = -1;
          int verticalShadowOffset = -1;
          double num13 = 0.349999994039536;
          Utility.drawWithShadow(b4, mouseCursors, position, sourceRect, white, (float) num10, zero, (float) pixelZoom, num11 != 0, (float) num12, horizontalShadowOffset, verticalShadowOffset, (float) num13);
        }
        if (this.playerWon)
        {
          IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(403, 373, 9, 9), this.doubleOrNothing.bounds.X, this.doubleOrNothing.bounds.Y, this.doubleOrNothing.bounds.Width, this.doubleOrNothing.bounds.Height, Color.White, (float) Game1.pixelZoom * this.doubleOrNothing.scale, true);
          SpriteText.drawString(b, this.doubleOrNothing.label, this.doubleOrNothing.bounds.X + Game1.pixelZoom * 8, this.doubleOrNothing.bounds.Y + Game1.pixelZoom * 2, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
        }
        if (Game1.player.clubCoins >= this.currentBet)
        {
          IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(403, 373, 9, 9), this.playAgain.bounds.X, this.playAgain.bounds.Y, this.playAgain.bounds.Width, this.playAgain.bounds.Height, Color.White, (float) Game1.pixelZoom * this.playAgain.scale, true);
          SpriteText.drawString(b, this.playAgain.label, this.playAgain.bounds.X + Game1.pixelZoom * 8, this.playAgain.bounds.Y + Game1.pixelZoom * 2, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
        }
        IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(403, 373, 9, 9), this.quit.bounds.X, this.quit.bounds.Y, this.quit.bounds.Width, this.quit.bounds.Height, Color.White, (float) Game1.pixelZoom * this.quit.scale, true);
        SpriteText.drawString(b, this.quit.label, this.quit.bounds.X + Game1.pixelZoom * 8, this.quit.bounds.Y + Game1.pixelZoom * 2, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
      }
      else
      {
        Vector2 vector2 = new Vector2((float) (Game1.tileSize * 2), (float) (Game1.graphics.GraphicsDevice.Viewport.Height - Game1.tileSize * 5));
        int num1 = 0;
        foreach (int[] playerCard in this.playerCards)
        {
          int height2 = 144;
          if (playerCard[1] > 0)
            height2 = (int) ((double) Math.Abs((float) playerCard[1] - 200f) / 200.0 * 144.0);
          IClickableMenu.drawTextureBox(b, Game1.mouseCursors, playerCard[1] > 200 || playerCard[1] == -1 ? new Rectangle(399, 396, 15, 15) : new Rectangle(384, 396, 15, 15), (int) vector2.X, (int) vector2.Y + 72 - height2 / 2, 96, height2, Color.White, (float) Game1.pixelZoom, true);
          if (playerCard[1] == 0)
            SpriteText.drawStringHorizontallyCenteredAt(b, string.Concat((object) playerCard[0]), (int) vector2.X + 48 - Game1.tileSize / 8 + Game1.pixelZoom, (int) vector2.Y + 72 - Game1.tileSize / 4, 999999, -1, 999999, 1f, 0.88f, false, -1);
          vector2.X += (float) (96 + Game1.tileSize / 4);
          if (playerCard[1] == 0)
            num1 += playerCard[0];
        }
        SpriteText.drawStringWithScrollBackground(b, Game1.player.name + ": " + (object) num1, Game1.tileSize * 2 + Game1.tileSize / 2, (int) vector2.Y + 144 + Game1.tileSize / 2, "", 1f, -1);
        vector2.X = (float) (Game1.tileSize * 2);
        vector2.Y = (float) (Game1.tileSize * 2);
        int num2 = 0;
        foreach (int[] dealerCard in this.dealerCards)
        {
          int height2 = 144;
          if (dealerCard[1] > 0)
            height2 = (int) ((double) Math.Abs((float) dealerCard[1] - 200f) / 200.0 * 144.0);
          IClickableMenu.drawTextureBox(b, Game1.mouseCursors, dealerCard[1] > 200 || dealerCard[1] == -1 ? new Rectangle(399, 396, 15, 15) : new Rectangle(384, 396, 15, 15), (int) vector2.X, (int) vector2.Y + 72 - height2 / 2, 96, height2, Color.White, (float) Game1.pixelZoom, true);
          if (dealerCard[1] == 0)
            SpriteText.drawStringHorizontallyCenteredAt(b, string.Concat((object) dealerCard[0]), (int) vector2.X + 48 - Game1.tileSize / 8 + Game1.pixelZoom, (int) vector2.Y + 72 - Game1.tileSize / 4, 999999, -1, 999999, 1f, 0.88f, false, -1);
          vector2.X += (float) (96 + Game1.tileSize / 4);
          if (dealerCard[1] == 0)
            num2 += dealerCard[0];
          else if (dealerCard[1] == -1)
            num2 = -99999;
        }
        SpriteText.drawStringWithScrollBackground(b, Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11970", (object) (num2 > 0 ? string.Concat((object) num2) : "?")), Game1.tileSize * 2 + Game1.tileSize / 2, Game1.tileSize / 2, "", 1f, -1);
        SpriteText.drawStringWithScrollBackground(b, Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11972", (object) (this.currentBet.ToString() + this.coinBuffer)), Game1.tileSize * 3, Game1.graphics.GraphicsDevice.Viewport.Height / 2, "", 1f, -1);
        Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float) (Game1.tileSize * 3 + Game1.pixelZoom * 3 + SpriteText.getWidthOfString(Game1.content.LoadString("Strings\\StringsFromCSFiles:CalicoJack.cs.11972", (object) this.currentBet))), (float) (Game1.graphics.GraphicsDevice.Viewport.Height / 2 + Game1.pixelZoom)), new Rectangle(211, 373, 9, 10), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
        if (this.playButtonsActive())
        {
          IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(403, 373, 9, 9), this.hit.bounds.X, this.hit.bounds.Y, this.hit.bounds.Width, this.hit.bounds.Height, Color.White, (float) Game1.pixelZoom * this.hit.scale, true);
          SpriteText.drawString(b, this.hit.label, this.hit.bounds.X + Game1.pixelZoom * 2, this.hit.bounds.Y + Game1.pixelZoom * 2, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
          IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(403, 373, 9, 9), this.stand.bounds.X, this.stand.bounds.Y, this.stand.bounds.Width, this.stand.bounds.Height, Color.White, (float) Game1.pixelZoom * this.stand.scale, true);
          SpriteText.drawString(b, this.stand.label, this.stand.bounds.X + Game1.pixelZoom * 2, this.stand.bounds.Y + Game1.pixelZoom * 2, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
        }
      }
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
      return nameof (CalicoJack);
    }
  }
}
