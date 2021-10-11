// Decompiled with JetBrains decompiler
// Type: StardewValley.Minigames.GrandpaStory
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
using xTile;
using xTile.Display;

namespace StardewValley.Minigames
{
  public class GrandpaStory : IMinigame
  {
    private float letterScale = 0.5f;
    private Vector2 letterPosition = new Vector2(477f, 345f);
    public const int sceneWidth = 1294;
    public const int sceneHeight = 730;
    public const int scene_beforeGrandpa = 0;
    public const int scene_grandpaSpeech = 1;
    public const int scene_fadeOutFromGrandpa = 2;
    public const int scene_timePass = 3;
    public const int scene_jojaCorpOverhead = 4;
    public const int scene_jojaCorpPan = 5;
    public const int scene_desk = 6;
    private LocalizedContentManager content;
    private Texture2D texture;
    private float foregroundFade;
    private float backgroundFade;
    private float foregroundFadeChange;
    private float backgroundFadeChange;
    private float panX;
    private float letterDy;
    private float letterDyDy;
    private int scene;
    private int totalMilliseconds;
    private int grandpaSpeechTimer;
    private int parallaxPan;
    private int letterOpenTimer;
    private bool drawGrandpa;
    private bool letterReceived;
    private bool mouseActive;
    private bool clickedLetter;
    private bool quit;
    private bool fadingToQuit;
    private Queue<string> grandpaSpeech;
    private LetterViewerMenu letterView;

    public GrandpaStory()
    {
      Game1.changeMusicTrack("none");
      this.content = Game1.content.CreateTemporary();
      this.texture = this.content.Load<Texture2D>("Minigames\\jojacorps");
      this.backgroundFadeChange = 0.0003f;
      this.grandpaSpeech = new Queue<string>();
      this.grandpaSpeech.Enqueue(Game1.player.isMale ? Game1.content.LoadString("Strings\\StringsFromCSFiles:GrandpaStory.cs.12026") : Game1.content.LoadString("Strings\\StringsFromCSFiles:GrandpaStory.cs.12028"));
      this.grandpaSpeech.Enqueue(Game1.content.LoadString("Strings\\StringsFromCSFiles:GrandpaStory.cs.12029"));
      this.grandpaSpeech.Enqueue(Game1.content.LoadString("Strings\\StringsFromCSFiles:GrandpaStory.cs.12030"));
      this.grandpaSpeech.Enqueue(Game1.content.LoadString("Strings\\StringsFromCSFiles:GrandpaStory.cs.12031"));
      this.grandpaSpeech.Enqueue(Game1.content.LoadString("Strings\\StringsFromCSFiles:GrandpaStory.cs.12034"));
      this.grandpaSpeech.Enqueue(Game1.content.LoadString("Strings\\StringsFromCSFiles:GrandpaStory.cs.12035"));
      this.grandpaSpeech.Enqueue(Game1.player.isMale ? Game1.content.LoadString("Strings\\StringsFromCSFiles:GrandpaStory.cs.12036") : Game1.content.LoadString("Strings\\StringsFromCSFiles:GrandpaStory.cs.12038"));
      this.grandpaSpeech.Enqueue(Game1.content.LoadString("Strings\\StringsFromCSFiles:GrandpaStory.cs.12040"));
      Game1.player.position = new Vector2(this.panX, (float) (Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 180 * Game1.pixelZoom / 2)) + new Vector2(3000f, 376f);
      Game1.viewport.X = 0;
      Game1.viewport.Y = 0;
      Map m = this.content.Load<Map>("Maps\\FarmHouse");
      IDisplayDevice mapDisplayDevice = Game1.mapDisplayDevice;
      m.LoadTileSheets(mapDisplayDevice);
      string name = "FarmHouse";
      Game1.currentLocation = (GameLocation) new FarmHouse(m, name);
      Game1.player.currentLocation = Game1.currentLocation;
    }

    public bool tick(GameTime time)
    {
      if (this.quit)
      {
        this.unload();
        Game1.currentMinigame = (IMinigame) new Intro();
        return false;
      }
      if (this.letterView != null)
        this.letterView.update(time);
      int totalMilliseconds = this.totalMilliseconds;
      TimeSpan elapsedGameTime = time.ElapsedGameTime;
      int milliseconds1 = elapsedGameTime.Milliseconds;
      this.totalMilliseconds = totalMilliseconds + milliseconds1;
      this.totalMilliseconds = this.totalMilliseconds % 9000000;
      double backgroundFade = (double) this.backgroundFade;
      double backgroundFadeChange = (double) this.backgroundFadeChange;
      elapsedGameTime = time.ElapsedGameTime;
      double milliseconds2 = (double) elapsedGameTime.Milliseconds;
      double num1 = backgroundFadeChange * milliseconds2;
      this.backgroundFade = (float) (backgroundFade + num1);
      this.backgroundFade = Math.Max(0.0f, Math.Min(1f, this.backgroundFade));
      double foregroundFade = (double) this.foregroundFade;
      double foregroundFadeChange = (double) this.foregroundFadeChange;
      elapsedGameTime = time.ElapsedGameTime;
      double milliseconds3 = (double) elapsedGameTime.Milliseconds;
      double num2 = foregroundFadeChange * milliseconds3;
      this.foregroundFade = (float) (foregroundFade + num2);
      this.foregroundFade = Math.Max(0.0f, Math.Min(1f, this.foregroundFade));
      int grandpaSpeechTimer1 = this.grandpaSpeechTimer;
      if ((double) this.foregroundFade >= 1.0 && this.fadingToQuit)
      {
        this.unload();
        Game1.currentMinigame = (IMinigame) new Intro();
        return false;
      }
      switch (this.scene)
      {
        case 0:
          if ((double) this.backgroundFade == 1.0)
          {
            if (!this.drawGrandpa)
            {
              this.foregroundFade = 1f;
              this.foregroundFadeChange = -0.0005f;
              this.drawGrandpa = true;
            }
            if ((double) this.foregroundFade == 0.0)
            {
              this.scene = 1;
              Game1.changeMusicTrack("grandpas_theme");
              break;
            }
            break;
          }
          break;
        case 1:
          int grandpaSpeechTimer2 = this.grandpaSpeechTimer;
          elapsedGameTime = time.ElapsedGameTime;
          int milliseconds4 = elapsedGameTime.Milliseconds;
          this.grandpaSpeechTimer = grandpaSpeechTimer2 + milliseconds4;
          if (this.grandpaSpeechTimer >= 60000)
            this.foregroundFadeChange = 0.0005f;
          if ((double) this.foregroundFade >= 1.0)
          {
            this.drawGrandpa = false;
            this.scene = 3;
            this.grandpaSpeechTimer = 0;
            this.foregroundFade = 0.0f;
            this.foregroundFadeChange = 0.0f;
          }
          if (grandpaSpeechTimer1 % 10000 > this.grandpaSpeechTimer % 10000 && this.grandpaSpeech.Count > 0)
            this.grandpaSpeech.Dequeue();
          if (grandpaSpeechTimer1 < 25000 && this.grandpaSpeechTimer > 25000 && this.grandpaSpeech.Count > 0)
            this.grandpaSpeech.Dequeue();
          if (grandpaSpeechTimer1 < 17000 && this.grandpaSpeechTimer >= 17000)
          {
            Game1.playSound("newRecipe");
            this.letterReceived = true;
            this.letterDy = -0.6f;
            this.letterDyDy = 1f / 1000f;
          }
          if (this.letterReceived && (double) this.letterPosition.Y <= (double) Game1.graphics.GraphicsDevice.Viewport.Height)
          {
            double letterDy1 = (double) this.letterDy;
            double letterDyDy = (double) this.letterDyDy;
            elapsedGameTime = time.ElapsedGameTime;
            double milliseconds5 = (double) elapsedGameTime.Milliseconds;
            double num3 = letterDyDy * milliseconds5;
            this.letterDy = (float) (letterDy1 + num3);
            // ISSUE: explicit reference operation
            // ISSUE: variable of a reference type
            float& local1 = @this.letterPosition.Y;
            // ISSUE: explicit reference operation
            double num4 = (double) ^local1;
            double letterDy2 = (double) this.letterDy;
            elapsedGameTime = time.ElapsedGameTime;
            double milliseconds6 = (double) elapsedGameTime.Milliseconds;
            double num5 = letterDy2 * milliseconds6;
            double num6 = num4 + num5;
            // ISSUE: explicit reference operation
            ^local1 = (float) num6;
            // ISSUE: explicit reference operation
            // ISSUE: variable of a reference type
            float& local2 = @this.letterPosition.X;
            // ISSUE: explicit reference operation
            double num7 = (double) ^local2;
            double num8 = 0.00999999977648258;
            elapsedGameTime = time.ElapsedGameTime;
            double milliseconds7 = (double) elapsedGameTime.Milliseconds;
            double num9 = num8 * milliseconds7;
            double num10 = num7 + num9;
            // ISSUE: explicit reference operation
            ^local2 = (float) num10;
            double letterScale = (double) this.letterScale;
            double num11 = 1.0 / 800.0;
            elapsedGameTime = time.ElapsedGameTime;
            double milliseconds8 = (double) elapsedGameTime.Milliseconds;
            double num12 = num11 * milliseconds8;
            this.letterScale = (float) (letterScale + num12);
            if ((double) this.letterPosition.Y > (double) Game1.graphics.GraphicsDevice.Viewport.Height)
            {
              Game1.playSound("coin");
              break;
            }
            break;
          }
          break;
        case 3:
          int grandpaSpeechTimer3 = this.grandpaSpeechTimer;
          elapsedGameTime = time.ElapsedGameTime;
          int milliseconds9 = elapsedGameTime.Milliseconds;
          this.grandpaSpeechTimer = grandpaSpeechTimer3 + milliseconds9;
          if (this.grandpaSpeechTimer > 2600 && grandpaSpeechTimer1 <= 2600)
          {
            Game1.changeMusicTrack("jojaOfficeSoundscape");
            break;
          }
          if (this.grandpaSpeechTimer > 4000)
          {
            this.grandpaSpeechTimer = 0;
            this.scene = 4;
            break;
          }
          break;
        case 4:
          int grandpaSpeechTimer4 = this.grandpaSpeechTimer;
          elapsedGameTime = time.ElapsedGameTime;
          int milliseconds10 = elapsedGameTime.Milliseconds;
          this.grandpaSpeechTimer = grandpaSpeechTimer4 + milliseconds10;
          if (this.grandpaSpeechTimer >= 9000)
          {
            this.grandpaSpeechTimer = 0;
            this.scene = 5;
            Game1.player.faceDirection(1);
            Game1.player.currentEyes = 1;
          }
          if (this.grandpaSpeechTimer >= 7000)
          {
            Game1.viewport.X = 0;
            Game1.viewport.Y = 0;
            double panX = (double) this.panX;
            double num3 = 0.200000002980232;
            elapsedGameTime = time.ElapsedGameTime;
            double milliseconds5 = (double) elapsedGameTime.Milliseconds;
            double num4 = num3 * milliseconds5;
            this.panX = (float) (panX - num4);
            Game1.player.position = new Vector2(this.panX, (float) (Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 180 * Game1.pixelZoom / 2)) + new Vector2(3612f, 572f);
            break;
          }
          break;
        case 5:
          if ((double) this.panX > (double) (-1200 * Game1.pixelZoom + Math.Max(400 * Game1.pixelZoom, Game1.graphics.GraphicsDevice.Viewport.Width)))
          {
            Game1.viewport.X = 0;
            Game1.viewport.Y = 0;
            double panX = (double) this.panX;
            double num3 = 0.200000002980232;
            elapsedGameTime = time.ElapsedGameTime;
            double milliseconds5 = (double) elapsedGameTime.Milliseconds;
            double num4 = num3 * milliseconds5;
            this.panX = (float) (panX - num4);
            Game1.player.position = new Vector2(this.panX, (float) (Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 180 * Game1.pixelZoom / 2)) + new Vector2(3612f, 572f);
            break;
          }
          int grandpaSpeechTimer5 = this.grandpaSpeechTimer;
          elapsedGameTime = time.ElapsedGameTime;
          int milliseconds11 = elapsedGameTime.Milliseconds;
          this.grandpaSpeechTimer = grandpaSpeechTimer5 + milliseconds11;
          if (grandpaSpeechTimer1 < 2000 && this.grandpaSpeechTimer >= 2000)
            Game1.player.currentEyes = 4;
          if (grandpaSpeechTimer1 < 3000 && this.grandpaSpeechTimer >= 3000)
          {
            Game1.player.currentEyes = 1;
            Game1.player.jitterStrength = 1f;
          }
          if (grandpaSpeechTimer1 < 3500 && this.grandpaSpeechTimer >= 3500)
            Game1.player.stopJittering();
          if (grandpaSpeechTimer1 < 4000 && this.grandpaSpeechTimer >= 4000)
          {
            Game1.player.currentEyes = 1;
            Game1.player.jitterStrength = 1f;
          }
          if (grandpaSpeechTimer1 < 4500 && this.grandpaSpeechTimer >= 4500)
          {
            Game1.player.stopJittering();
            Game1.player.doEmote(28);
          }
          if (grandpaSpeechTimer1 < 7000 && this.grandpaSpeechTimer >= 7000)
            Game1.player.currentEyes = 4;
          if (grandpaSpeechTimer1 < 8000 && this.grandpaSpeechTimer >= 8000)
            Game1.player.showFrame(33, false);
          if (this.grandpaSpeechTimer >= 10000)
          {
            this.scene = 6;
            this.grandpaSpeechTimer = 0;
          }
          Game1.player.position = new Vector2(this.panX, (float) (Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 180 * Game1.pixelZoom / 2)) + new Vector2(3612f, 572f);
          break;
        case 6:
          int grandpaSpeechTimer6 = this.grandpaSpeechTimer;
          elapsedGameTime = time.ElapsedGameTime;
          int milliseconds12 = elapsedGameTime.Milliseconds;
          this.grandpaSpeechTimer = grandpaSpeechTimer6 + milliseconds12;
          if (this.grandpaSpeechTimer >= 2000)
          {
            int parallaxPan = this.parallaxPan;
            double num3 = 0.1;
            elapsedGameTime = time.ElapsedGameTime;
            double milliseconds5 = (double) elapsedGameTime.Milliseconds;
            int num4 = (int) Math.Ceiling(num3 * milliseconds5);
            this.parallaxPan = parallaxPan + num4;
            if (this.parallaxPan >= 107)
              this.parallaxPan = 107;
          }
          if (grandpaSpeechTimer1 < 3500 && this.grandpaSpeechTimer >= 3500)
            Game1.changeMusicTrack("none");
          if (grandpaSpeechTimer1 < 5000 && this.grandpaSpeechTimer >= 5000)
            Game1.playSound("doorCreak");
          if (grandpaSpeechTimer1 < 6000 && this.grandpaSpeechTimer >= 6000)
          {
            this.mouseActive = true;
            Point center = this.clickableGrandpaLetterRect().Center;
            Mouse.SetPosition(center.X, center.Y);
            Game1.lastCursorMotionWasMouse = false;
          }
          if (this.clickedLetter)
          {
            int letterOpenTimer = this.letterOpenTimer;
            elapsedGameTime = time.ElapsedGameTime;
            int milliseconds5 = elapsedGameTime.Milliseconds;
            this.letterOpenTimer = letterOpenTimer + milliseconds5;
            break;
          }
          break;
      }
      Game1.player.updateEmote(time);
      if ((double) Game1.player.jitterStrength > 0.0)
        Game1.player.jitter = new Vector2((float) Game1.random.Next(-(int) ((double) Game1.player.jitterStrength * 100.0), (int) (((double) Game1.player.jitterStrength + 1.0) * 100.0)) / 100f, (float) Game1.random.Next(-(int) ((double) Game1.player.jitterStrength * 100.0), (int) (((double) Game1.player.jitterStrength + 1.0) * 100.0)) / 100f);
      return false;
    }

    public void afterFade()
    {
    }

    private Rectangle clickableGrandpaLetterRect()
    {
      return new Rectangle((int) Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0).X + (286 - this.parallaxPan) * 4, (int) Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0).Y + 218 + Math.Max(0, Math.Min(60, (this.grandpaSpeechTimer - 5000) / 8)), 524, 344);
    }

    public void receiveLeftClick(int x, int y, bool playSound = true)
    {
      if (!this.clickedLetter && this.mouseActive && (this.clickableGrandpaLetterRect().Contains(x, y) || Game1.options.SnappyMenus))
      {
        this.clickedLetter = true;
        Game1.playSound("newRecipe");
        Game1.changeMusicTrack("musicboxsong");
        string text;
        if (!Game1.player.isMale)
          text = Game1.content.LoadString("Strings\\StringsFromCSFiles:GrandpaStory.cs.12055", (object) Game1.player.name, (object) Game1.player.farmName);
        else
          text = Game1.content.LoadString("Strings\\StringsFromCSFiles:GrandpaStory.cs.12051", (object) Game1.player.name, (object) Game1.player.farmName);
        this.letterView = new LetterViewerMenu(text);
        this.letterView.exitFunction = new IClickableMenu.onExit(this.onLetterExit);
      }
      if (this.letterView == null)
        return;
      this.letterView.receiveLeftClick(x, y, true);
    }

    public void onLetterExit()
    {
      this.mouseActive = false;
      this.foregroundFadeChange = 0.0003f;
      this.fadingToQuit = true;
      if (this.letterView != null)
      {
        this.letterView.unload();
        this.letterView = (LetterViewerMenu) null;
      }
      Game1.playSound("newRecipe");
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
      if (k == Keys.Escape || Game1.options.doesInputListContain(Game1.options.menuButton, k))
      {
        if (!this.quit && !this.fadingToQuit)
          Game1.playSound("bigDeSelect");
        if (this.letterView != null)
        {
          this.letterView.unload();
          this.letterView = (LetterViewerMenu) null;
        }
        this.quit = true;
      }
      else
      {
        if (this.letterView == null)
          return;
        this.letterView.receiveKeyPress(k);
        if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.RightTrigger) && !Game1.oldPadState.IsButtonDown(Buttons.RightTrigger))
          this.letterView.receiveGamePadButton(Buttons.RightTrigger);
        if (!GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftTrigger) || !Game1.oldPadState.IsButtonUp(Buttons.LeftTrigger))
          return;
        this.letterView.receiveGamePadButton(Buttons.LeftTrigger);
      }
    }

    public bool overrideFreeMouseMovement()
    {
      return Game1.options.SnappyMenus;
    }

    public void receiveKeyRelease(Keys k)
    {
    }

    public void draw(SpriteBatch b)
    {
      b.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
      SpriteBatch spriteBatch1 = b;
      Texture2D staminaRect1 = Game1.staminaRect;
      int x1 = 0;
      int y1 = 0;
      Viewport viewport1 = Game1.graphics.GraphicsDevice.Viewport;
      int width1 = viewport1.Width;
      viewport1 = Game1.graphics.GraphicsDevice.Viewport;
      int height1 = viewport1.Height;
      Rectangle destinationRectangle1 = new Rectangle(x1, y1, width1, height1);
      Color color1 = new Color(64, 136, 248);
      spriteBatch1.Draw(staminaRect1, destinationRectangle1, color1);
      SpriteBatch spriteBatch2 = b;
      Texture2D staminaRect2 = Game1.staminaRect;
      int x2 = 0;
      int y2 = 0;
      Viewport viewport2 = Game1.graphics.GraphicsDevice.Viewport;
      int width2 = viewport2.Width;
      viewport2 = Game1.graphics.GraphicsDevice.Viewport;
      int height2 = viewport2.Height;
      Rectangle destinationRectangle2 = new Rectangle(x2, y2, width2, height2);
      Color color2 = Color.Black * this.backgroundFade;
      spriteBatch2.Draw(staminaRect2, destinationRectangle2, color2);
      if (this.drawGrandpa)
      {
        b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0), new Rectangle?(new Rectangle(427, this.totalMilliseconds % 300 < 150 ? 240 : 0, 427, 240)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
        b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0) + new Vector2(317f, 74f) * 3f, new Rectangle?(new Rectangle(427 + 74 * (this.totalMilliseconds % 400 / 100), 480, 74, 42)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
        b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0) + new Vector2(320f, 75f) * 3f, new Rectangle?(new Rectangle(427, 522, 70, 32)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
        if (this.grandpaSpeechTimer > 8000 && this.grandpaSpeechTimer % 10000 < 5000)
          b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0) + new Vector2(189f, 69f) * 3f, new Rectangle?(new Rectangle(497 + 18 * (this.totalMilliseconds % 400 / 200), 523, 18, 18)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
        if (this.grandpaSpeech.Count > 0 && this.grandpaSpeechTimer > 3000)
        {
          b.DrawString(Game1.dialogueFont, this.grandpaSpeech.Peek(), new Vector2((float) ((double) (Game1.graphics.GraphicsDevice.Viewport.Width / 2) - (double) Game1.dialogueFont.MeasureString(this.grandpaSpeech.Peek()).X / 2.0 - 3.0), (float) ((int) Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0).Y + 669) + 3f), Color.White * 0.25f, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
          b.DrawString(Game1.dialogueFont, this.grandpaSpeech.Peek(), new Vector2((float) (Game1.graphics.GraphicsDevice.Viewport.Width / 2) - Game1.dialogueFont.MeasureString(this.grandpaSpeech.Peek()).X / 2f, (float) ((int) Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0).Y + 669)), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        }
        if (this.letterReceived)
        {
          b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0) + new Vector2(157f, 113f) * 3f, new Rectangle?(new Rectangle(463, 556, 37, 17)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
          if (this.grandpaSpeechTimer > 8000 && this.grandpaSpeechTimer % 10000 > 7000 && (this.grandpaSpeechTimer % 10000 < 9000 && this.totalMilliseconds % 600 < 300))
            b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0) + new Vector2(157f, 113f) * 3f, new Rectangle?(new Rectangle(500, 556, 37, 17)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
          b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0) + this.letterPosition, new Rectangle?(new Rectangle(729, 524, 131, 63)), Color.White, 0.0f, Vector2.Zero, this.letterScale, SpriteEffects.None, 1f);
        }
      }
      else if (this.scene == 3)
        SpriteText.drawString(b, Game1.content.LoadString("Strings\\StringsFromCSFiles:GrandpaStory.cs.12059"), (int) Utility.getTopLeftPositionForCenteringOnScreen(0, 0, -200, 0).X, (int) Utility.getTopLeftPositionForCenteringOnScreen(0, 0, 0, -50).Y, 999, -1, 999, 1f, 1f, false, -1, "", 4);
      else if (this.scene == 4)
      {
        float num = (float) (1.0 - ((double) this.grandpaSpeechTimer - 7000.0) / 2000.0);
        b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0), new Rectangle?(new Rectangle(0, 0, 427, 240)), Color.White * num, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
        b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0) + new Vector2(22f, 211f) * 3f, new Rectangle?(new Rectangle(264 + this.totalMilliseconds % 500 / 250 * 19, 581, 19, 17)), Color.White * num, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
        b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0) + new Vector2(332f, 215f) * 3f, new Rectangle?(new Rectangle(305 + this.totalMilliseconds % 600 / 200 * 12, 581, 12, 12)), Color.White * num, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
        b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0) + new Vector2(414f, 211f) * 3f, new Rectangle?(new Rectangle(460 + this.totalMilliseconds % 400 / 200 * 13, 581, 13, 17)), Color.White * num, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
        b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0) + new Vector2(189f, 81f) * 3f, new Rectangle?(new Rectangle(426 + this.totalMilliseconds % 800 / 400 * 16, 581, 16, 16)), Color.White * num, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
      }
      if (this.scene == 4 && this.grandpaSpeechTimer >= 5000 || this.scene == 5)
      {
        b.Draw(this.texture, new Vector2(this.panX, (float) (Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 180 * Game1.pixelZoom / 2)), new Rectangle?(new Rectangle(0, 600, 1200, 180)), Color.White * (this.scene == 5 ? 1f : (float) (((double) this.grandpaSpeechTimer - 7000.0) / 2000.0)), 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
        b.Draw(this.texture, new Vector2(this.panX, (float) (Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 180 * Game1.pixelZoom / 2)) + new Vector2(1080f, 524f), new Rectangle?(new Rectangle(350 + this.totalMilliseconds % 800 / 400 * 14, 581, 14, 9)), Color.White * (this.scene == 5 ? 1f : (float) (((double) this.grandpaSpeechTimer - 7000.0) / 2000.0)), 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
        b.Draw(this.texture, new Vector2(this.panX, (float) (Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 180 * Game1.pixelZoom / 2)) + new Vector2(1564f, 520f), new Rectangle?(new Rectangle(383 + this.totalMilliseconds % 400 / 200 * 9, 581, 9, 7)), Color.White * (this.scene == 5 ? 1f : (float) (((double) this.grandpaSpeechTimer - 7000.0) / 2000.0)), 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
        b.Draw(this.texture, new Vector2(this.panX, (float) (Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 180 * Game1.pixelZoom / 2)) + new Vector2(2632f, 520f), new Rectangle?(new Rectangle(403 + this.totalMilliseconds % 600 / 300 * 8, 582, 8, 8)), Color.White * (this.scene == 5 ? 1f : (float) (((double) this.grandpaSpeechTimer - 7000.0) / 2000.0)), 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
        b.Draw(this.texture, new Vector2(this.panX, (float) (Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 180 * Game1.pixelZoom / 2)) + new Vector2(2604f, 504f), new Rectangle?(new Rectangle(364 + this.totalMilliseconds % 1100 / 100 * 5, 594, 5, 3)), Color.White * (this.scene == 5 ? 1f : (float) (((double) this.grandpaSpeechTimer - 7000.0) / 2000.0)), 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
        b.Draw(this.texture, new Vector2(this.panX, (float) (Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 180 * Game1.pixelZoom / 2)) + new Vector2(3116f, 492f), new Rectangle?(new Rectangle(343 + this.totalMilliseconds % 3000 / 1000 * 6, 593, 6, 5)), Color.White * (this.scene == 5 ? 1f : (float) (((double) this.grandpaSpeechTimer - 7000.0) / 2000.0)), 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
        if (this.scene == 5)
          Game1.player.draw(b);
        b.Draw(this.texture, new Vector2(this.panX, (float) (Game1.graphics.GraphicsDevice.Viewport.Height / 2 - 180 * Game1.pixelZoom / 2)) + new Vector2(3580f, 540f), new Rectangle?(new Rectangle(895, 735, 29, 36)), Color.White * (this.scene == 5 ? 1f : (float) (((double) this.grandpaSpeechTimer - 7000.0) / 2000.0)), 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
      }
      if (this.scene == 6)
      {
        b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0) + new Vector2((float) (261 - this.parallaxPan), 145f) * 4f, new Rectangle?(new Rectangle(550, 540, 56 + this.parallaxPan, 35)), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
        b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0) + new Vector2((float) (261 - this.parallaxPan), 4f + (float) Math.Max(0, Math.Min(60, (this.grandpaSpeechTimer - 5000) / 8))) * 4f, new Rectangle?(new Rectangle(264, 434, 56 + this.parallaxPan, 141)), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
        if (this.grandpaSpeechTimer > 3000)
          b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0) + new Vector2((float) (286 - this.parallaxPan), 32f + (float) Math.Max(0, Math.Min(60, (this.grandpaSpeechTimer - 5000) / 8)) + Math.Min(30f, (float) this.letterOpenTimer / 4f)) * 4f, new Rectangle?(new Rectangle(729 + Math.Min(2, this.letterOpenTimer / 200) * 131, 508, 131, 79)), Color.White, 0.0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);
        b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0), new Rectangle?(new Rectangle(this.parallaxPan, 240, 320, 180)), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
        b.Draw(this.texture, Utility.getTopLeftPositionForCenteringOnScreen(1294, 730, 0, 0) + new Vector2((float) (187.0 - (double) this.parallaxPan * 2.5), 8f) * 4f, new Rectangle?(new Rectangle(20, 428, 232, 172)), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
      }
      if (this.letterView != null)
        this.letterView.draw(b);
      if (this.mouseActive)
        b.Draw(Game1.mouseCursors, new Vector2((float) Game1.getOldMouseX(), (float) Game1.getOldMouseY()), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 0, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
      SpriteBatch spriteBatch3 = b;
      Texture2D staminaRect3 = Game1.staminaRect;
      int x3 = 0;
      int y3 = 0;
      Viewport viewport3 = Game1.graphics.GraphicsDevice.Viewport;
      int width3 = viewport3.Width;
      viewport3 = Game1.graphics.GraphicsDevice.Viewport;
      int height3 = viewport3.Height;
      Rectangle destinationRectangle3 = new Rectangle(x3, y3, width3, height3);
      Color color3 = this.fadingToQuit ? new Color(64, 136, 248) * this.foregroundFade : Color.Black * this.foregroundFade;
      spriteBatch3.Draw(staminaRect3, destinationRectangle3, color3);
      b.End();
    }

    public void changeScreenSize()
    {
      Game1.viewport.X = 0;
      Game1.viewport.Y = 0;
    }

    public void unload()
    {
      this.content.Unload();
      this.content = (LocalizedContentManager) null;
    }

    public void receiveEventPoke(int data)
    {
    }

    public string minigameId()
    {
      return (string) null;
    }
  }
}
