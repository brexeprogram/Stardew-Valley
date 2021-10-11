// Decompiled with JetBrains decompiler
// Type: StardewValley.Minigames.FantasyBoardGame
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace StardewValley.Minigames
{
  public class FantasyBoardGame : IMinigame
  {
    public int borderSourceWidth = 138;
    public int borderSourceHeight = 74;
    public int slideSourceWidth = 128;
    public int slideSourceHeight = 64;
    private string grade = "";
    private LocalizedContentManager content;
    private Texture2D slides;
    private Texture2D border;
    public int whichSlide;
    public int shakeTimer;
    public int endTimer;

    public FantasyBoardGame()
    {
      this.content = Game1.content.CreateTemporary();
      this.slides = this.content.Load<Texture2D>("LooseSprites\\boardGame");
      this.border = this.content.Load<Texture2D>("LooseSprites\\boardGameBorder");
      Game1.globalFadeToClear((Game1.afterFadeFunction) null, 0.02f);
    }

    public bool overrideFreeMouseMovement()
    {
      return false;
    }

    public bool tick(GameTime time)
    {
      TimeSpan elapsedGameTime;
      if (this.shakeTimer > 0)
      {
        int shakeTimer = this.shakeTimer;
        elapsedGameTime = time.ElapsedGameTime;
        int milliseconds = elapsedGameTime.Milliseconds;
        this.shakeTimer = shakeTimer - milliseconds;
      }
      Game1.currentLocation.currentEvent.checkForNextCommand(Game1.currentLocation, time);
      if (Game1.activeClickableMenu != null)
        Game1.activeClickableMenu.update(time);
      if (this.endTimer > 0)
      {
        int endTimer = this.endTimer;
        elapsedGameTime = time.ElapsedGameTime;
        int milliseconds = elapsedGameTime.Milliseconds;
        this.endTimer = endTimer - milliseconds;
        if (this.endTimer <= 0 && this.whichSlide == -1)
          Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.end), 0.02f);
      }
      if (Game1.activeClickableMenu != null)
        Game1.activeClickableMenu.performHoverAction(Game1.getOldMouseX(), Game1.getOldMouseY());
      return false;
    }

    public void end()
    {
      this.unload();
      ++Game1.currentLocation.currentEvent.CurrentCommand;
      Game1.currentMinigame = (IMinigame) null;
    }

    public void receiveLeftClick(int x, int y, bool playSound = true)
    {
      if (Game1.activeClickableMenu == null)
        return;
      Game1.activeClickableMenu.receiveLeftClick(x, y, true);
    }

    public void leftClickHeld(int x, int y)
    {
    }

    public void receiveRightClick(int x, int y, bool playSound = true)
    {
      Game1.pressActionButton(Keyboard.GetState(), Mouse.GetState(), GamePad.GetState(Game1.playerOneIndex));
      if (Game1.activeClickableMenu == null)
        return;
      Game1.activeClickableMenu.receiveRightClick(x, y, true);
    }

    public void releaseLeftClick(int x, int y)
    {
    }

    public void releaseRightClick(int x, int y)
    {
    }

    public void receiveKeyPress(Keys k)
    {
      if (!Game1.isQuestion)
        return;
      if (Game1.options.doesInputListContain(Game1.options.moveUpButton, k))
      {
        Game1.currentQuestionChoice = Math.Max(Game1.currentQuestionChoice - 1, 0);
        Game1.playSound("toolSwap");
      }
      else
      {
        if (!Game1.options.doesInputListContain(Game1.options.moveDownButton, k))
          return;
        Game1.currentQuestionChoice = Math.Min(Game1.currentQuestionChoice + 1, Game1.questionChoices.Count - 1);
        Game1.playSound("toolSwap");
      }
    }

    public void receiveKeyRelease(Keys k)
    {
    }

    public void draw(SpriteBatch b)
    {
      b.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
      if (this.whichSlide >= 0)
      {
        Vector2 vector2 = new Vector2();
        if (this.shakeTimer > 0)
          vector2 = new Vector2((float) Game1.random.Next(-2, 2), (float) Game1.random.Next(-2, 2));
        b.Draw(this.border, vector2 + new Vector2((float) (Game1.graphics.GraphicsDevice.Viewport.Width / 2 - this.borderSourceWidth * Game1.pixelZoom / 2), (float) (Game1.graphics.GraphicsDevice.Viewport.Height / 2 - this.borderSourceHeight * Game1.pixelZoom / 2 - Game1.tileSize * 2)), new Rectangle?(new Rectangle(0, 0, this.borderSourceWidth, this.borderSourceHeight)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.0f);
        b.Draw(this.slides, vector2 + new Vector2((float) (Game1.graphics.GraphicsDevice.Viewport.Width / 2 - this.slideSourceWidth * Game1.pixelZoom / 2), (float) (Game1.graphics.GraphicsDevice.Viewport.Height / 2 - this.slideSourceHeight * Game1.pixelZoom / 2 - Game1.tileSize * 2)), new Rectangle?(new Rectangle(this.whichSlide % 2 * this.slideSourceWidth, this.whichSlide / 2 * this.slideSourceHeight, this.slideSourceWidth, this.slideSourceHeight)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.01f);
      }
      else
      {
        string text = Game1.content.LoadString("Strings\\StringsFromCSFiles:FantasyBoardGame.cs.11980", (object) this.grade);
        float num1 = (float) Math.Sin((double) (this.endTimer / 1000)) * (float) (Game1.pixelZoom * 2);
        string message = text;
        Color textColor = Game1.textColor;
        Color purple = Color.Purple;
        Viewport viewport = Game1.graphics.GraphicsDevice.Viewport;
        double num2 = (double) (viewport.Width / 2) - (double) Game1.dialogueFont.MeasureString(text).X / 2.0;
        double num3 = (double) num1;
        viewport = Game1.graphics.GraphicsDevice.Viewport;
        double num4 = (double) (viewport.Height / 2);
        double num5 = num3 + num4;
        Vector2 position = new Vector2((float) num2, (float) num5);
        Game1.drawWithBorder(message, textColor, purple, position);
      }
      if (Game1.activeClickableMenu != null)
        Game1.activeClickableMenu.draw(b);
      b.End();
    }

    public void changeScreenSize()
    {
    }

    public void unload()
    {
      this.content.Unload();
    }

    public void afterFade()
    {
      this.whichSlide = -1;
      int num = 0;
      if (Game1.player.mailReceived.Contains("savedFriends"))
        ++num;
      if (Game1.player.mailReceived.Contains("destroyedPods"))
        ++num;
      if (Game1.player.mailReceived.Contains("killedSkeleton"))
        ++num;
      switch (num)
      {
        case 0:
          this.grade = "D";
          break;
        case 1:
          this.grade = "C";
          break;
        case 2:
          this.grade = "B";
          break;
        case 3:
          this.grade = "A";
          break;
      }
      Game1.playSound("newArtifact");
      this.endTimer = 5500;
    }

    public void receiveEventPoke(int data)
    {
      if (data == -1)
        this.shakeTimer = 1000;
      else if (data == -2)
        Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.afterFade), 0.02f);
      else
        this.whichSlide = data;
    }

    public string minigameId()
    {
      return nameof (FantasyBoardGame);
    }
  }
}
