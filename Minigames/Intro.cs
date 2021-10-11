// Decompiled with JetBrains decompiler
// Type: StardewValley.Minigames.Intro
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Minigames
{
  public class Intro : IMinigame
  {
    public float speed = 0.1f;
    private int screenWidth = Game1.graphics.GraphicsDevice.Viewport.Width / 3;
    private int screenHeight = Game1.graphics.GraphicsDevice.Viewport.Height / 3;
    private int tileSize = 16;
    private List<Point> backClouds = new List<Point>();
    private List<int> road = new List<int>();
    private List<int> sky = new List<int>();
    private List<int> roadsideObjects = new List<int>();
    private List<int> roadsideFences = new List<int>();
    private bool cameraCenteredOnBus = true;
    private Vector2 birdPosition = Vector2.Zero;
    private List<Intro.Balloon> balloons = new List<Intro.Balloon>();
    private int fenceBuildStatus = -1;
    public const int pixelScale = 3;
    public const int valleyLoopWidth = 160;
    public const int skyLoopWidth = 112;
    public const int cloudLoopWidth = 170;
    public const int tilesBeyondViewportToSimulate = 6;
    public const int leftFence = 0;
    public const int centerFence = 1;
    public const int rightFence = 2;
    public const int busYRest = 240;
    public const int choosingCharacterState = 0;
    public const int panningDownFromCloudsState = 1;
    public const int panningDownToRoadState = 2;
    public const int drivingState = 3;
    public const int stardewInViewState = 4;
    private float valleyPosition;
    private float skyPosition;
    private float roadPosition;
    private float bigCloudPosition;
    private float frontCloudPosition;
    private float backCloudPosition;
    private float globalYPan;
    private float globalYPanDY;
    private float drivingTimer;
    private float fadeAlpha;
    private Matrix transformMatrix;
    private Texture2D texture;
    private Texture2D roadsideTexture;
    private Texture2D cloudTexture;
    private Color skyColor;
    private Color roadColor;
    private Color carColor;
    private bool addedSign;
    private Vector2 busPosition;
    private Vector2 carPosition;
    private CharacterCustomization characterCreateMenu;
    private int birdFrame;
    private int birdTimer;
    private int birdXTimer;
    public static Cue roadNoise;
    private int currentState;
    private bool quit;
    private bool hasQuit;

    public Intro()
    {
      List<int> intList1 = new List<int>();
      List<int> intList2 = new List<int>();
      List<int> intList3 = new List<int>();
      this.texture = Game1.content.Load<Texture2D>("Minigames\\Intro");
      this.roadsideTexture = Game1.content.Load<Texture2D>("Maps\\spring_outdoorsTileSheet");
      this.cloudTexture = Game1.content.Load<Texture2D>("Minigames\\Clouds");
      this.transformMatrix = Matrix.CreateScale(3f);
      this.skyColor = new Color(64, 136, 248);
      this.roadColor = new Color(130, 130, 130);
      this.createBeginningOfLevel();
      Game1.player.FarmerSprite.SourceRect = new Rectangle(0, 0, 16, 32);
      this.bigCloudPosition = (float) this.cloudTexture.Width;
      if (Game1.soundBank != null)
        Intro.roadNoise = Game1.soundBank.GetCue("roadnoise");
      this.currentState = 1;
      Game1.changeMusicTrack("spring_day_ambient");
    }

    public Intro(int startingGameMode)
    {
      this.texture = Game1.content.Load<Texture2D>("Minigames\\Intro");
      this.roadsideTexture = Game1.content.Load<Texture2D>("Maps\\spring_outdoorsTileSheet");
      this.cloudTexture = Game1.content.Load<Texture2D>("Minigames\\Clouds");
      this.transformMatrix = Matrix.CreateScale(3f);
      this.skyColor = new Color(102, 181, (int) byte.MaxValue);
      this.roadColor = new Color(130, 130, 130);
      this.createBeginningOfLevel();
      this.currentState = startingGameMode;
      if (this.currentState != 4)
        return;
      this.fadeAlpha = 1f;
    }

    public bool overrideFreeMouseMovement()
    {
      return false;
    }

    public void createBeginningOfLevel()
    {
      this.backClouds.Clear();
      this.road.Clear();
      this.sky.Clear();
      this.roadsideObjects.Clear();
      this.roadsideFences.Clear();
      for (int index = 0; index < this.screenWidth / this.tileSize + 6; ++index)
      {
        this.road.Add(Game1.random.NextDouble() < 0.7 ? 0 : Game1.random.Next(0, 3));
        this.roadsideObjects.Add(-1);
        this.roadsideFences.Add(-1);
      }
      for (int index = 0; index < this.screenWidth / 112 + 2; ++index)
        this.sky.Add(Game1.random.NextDouble() < 0.5 ? 1 : Game1.random.Next(2));
      for (int index = 0; index < this.screenWidth / 170 + 2; ++index)
        this.backClouds.Add(new Point(Game1.random.Next(3), Game1.random.Next(this.screenHeight / 2)));
      this.roadsideObjects.Add(-1);
      this.roadsideObjects.Add(-1);
      this.roadsideObjects.Add(-1);
      this.busPosition = new Vector2((float) (this.tileSize * 8), 240f);
    }

    public void updateRoad(GameTime time)
    {
      this.roadPosition = this.roadPosition + (float) time.ElapsedGameTime.Milliseconds * this.speed;
      if ((double) this.roadPosition >= (double) (this.tileSize * 3))
      {
        this.roadPosition = this.roadPosition - (float) (this.tileSize * 3);
        for (int index = 0; index < 3; ++index)
          this.road.Add(Game1.random.NextDouble() < 0.7 ? 0 : Game1.random.Next(0, 3));
        this.road.RemoveRange(0, 3);
        if (this.fenceBuildStatus != -1 || this.cameraCenteredOnBus && Game1.random.NextDouble() < 0.1)
        {
          for (int index1 = 0; index1 < 3; ++index1)
          {
            switch (this.fenceBuildStatus)
            {
              case -1:
                this.fenceBuildStatus = 0;
                this.roadsideFences.Add(0);
                break;
              case 0:
                this.fenceBuildStatus = 1;
                this.roadsideFences.Add(Game1.random.Next(3));
                break;
              case 1:
                if (Game1.random.NextDouble() < 0.1)
                {
                  this.roadsideFences.Add(2);
                  this.fenceBuildStatus = 2;
                  break;
                }
                this.fenceBuildStatus = 1;
                this.roadsideFences.Add(Game1.random.NextDouble() < 0.1 ? 3 : Game1.random.Next(3));
                break;
              case 2:
                this.fenceBuildStatus = -1;
                for (int index2 = index1; index2 < 3; ++index2)
                  this.roadsideFences.Add(-1);
                break;
            }
            if (this.fenceBuildStatus == -1)
              break;
          }
        }
        else
        {
          this.roadsideFences.Add(-1);
          this.roadsideFences.Add(-1);
          this.roadsideFences.Add(-1);
        }
        this.roadsideFences.RemoveRange(0, 3);
        if (this.cameraCenteredOnBus && !this.addedSign && Game1.random.NextDouble() < 0.25)
        {
          for (int index1 = 0; index1 < 3; ++index1)
          {
            if (index1 == 0 && Game1.random.NextDouble() < 0.3)
            {
              this.roadsideObjects.Add(Game1.random.Next(2));
              for (int index2 = index1; index2 < 3; ++index2)
                this.roadsideObjects.Add(-1);
              break;
            }
            if (Game1.random.NextDouble() < 0.5)
              this.roadsideObjects.Add(Game1.random.Next(2, 5));
            else
              this.roadsideObjects.Add(-1);
          }
        }
        else
        {
          this.roadsideObjects.Add(-1);
          this.roadsideObjects.Add(-1);
          this.roadsideObjects.Add(-1);
        }
        this.roadsideObjects.RemoveRange(0, 3);
      }
      this.skyPosition = this.skyPosition + (float) time.ElapsedGameTime.Milliseconds * (this.speed / 12f);
      if ((double) this.skyPosition >= 112.0)
      {
        this.skyPosition = this.skyPosition - 112f;
        this.sky.Add(Game1.random.Next(2));
        this.sky.RemoveAt(0);
      }
      this.valleyPosition = this.valleyPosition + (float) time.ElapsedGameTime.Milliseconds * (this.speed / 6f);
      if (this.carPosition.Equals(Vector2.Zero) && Game1.random.NextDouble() < 0.002 && !this.addedSign)
      {
        this.carPosition = new Vector2((float) this.screenWidth, 200f);
        this.carColor = new Color(Game1.random.Next(100, (int) byte.MaxValue), Game1.random.Next(100, (int) byte.MaxValue), Game1.random.Next(100, (int) byte.MaxValue));
      }
      else
      {
        if (this.carPosition.Equals(Vector2.Zero))
          return;
        this.carPosition.X -= (float) (0.100000001490116 * (double) time.ElapsedGameTime.Milliseconds * ((double) this.carColor.G / 60.0));
        if ((double) this.carPosition.X >= -200.0)
          return;
        this.carPosition = Vector2.Zero;
      }
    }

    public void updateUpperClouds(GameTime time)
    {
      this.bigCloudPosition = this.bigCloudPosition + (float) time.ElapsedGameTime.Milliseconds * (this.speed / 24f);
      if ((double) this.bigCloudPosition >= (double) (this.cloudTexture.Width * 3))
        this.bigCloudPosition = this.bigCloudPosition - (float) (this.cloudTexture.Width * 3);
      this.backCloudPosition = this.backCloudPosition + (float) time.ElapsedGameTime.Milliseconds * (this.speed / 36f);
      if ((double) this.backCloudPosition > 170.0)
      {
        this.backCloudPosition = this.backCloudPosition % 170f;
        this.backClouds.Add(new Point(Game1.random.Next(3), Game1.random.Next(this.screenHeight / 2)));
        this.backClouds.RemoveAt(0);
      }
      if (Game1.random.NextDouble() < 0.0002)
      {
        this.balloons.Add(new Intro.Balloon(this.screenWidth, this.screenHeight));
        if (Game1.random.NextDouble() < 0.1)
        {
          Vector2 vector2 = new Vector2((float) Game1.random.Next(this.screenWidth / 3, this.screenWidth), (float) this.screenHeight);
          this.balloons.Add(new Intro.Balloon(this.screenWidth, this.screenHeight));
          this.balloons.Last<Intro.Balloon>().position = new Vector2(vector2.X + (float) Game1.random.Next(-16, 16), vector2.Y + (float) Game1.random.Next(8));
          this.balloons.Add(new Intro.Balloon(this.screenWidth, this.screenHeight));
          this.balloons.Last<Intro.Balloon>().position = new Vector2(vector2.X + (float) Game1.random.Next(-16, 16), vector2.Y + (float) Game1.random.Next(8));
          this.balloons.Add(new Intro.Balloon(this.screenWidth, this.screenHeight));
          this.balloons.Last<Intro.Balloon>().position = new Vector2(vector2.X + (float) Game1.random.Next(-16, 16), vector2.Y + (float) Game1.random.Next(8));
          this.balloons.Add(new Intro.Balloon(this.screenWidth, this.screenHeight));
          this.balloons.Last<Intro.Balloon>().position = new Vector2(vector2.X + (float) Game1.random.Next(-16, 16), vector2.Y + (float) Game1.random.Next(8));
        }
      }
      for (int index = this.balloons.Count - 1; index >= 0; --index)
      {
        this.balloons[index].update(this.speed, time);
        if ((double) this.balloons[index].position.X < (double) -this.tileSize || (double) this.balloons[index].position.Y < (double) -this.tileSize)
          this.balloons.RemoveAt(index);
      }
    }

    public bool tick(GameTime time)
    {
      if (this.hasQuit)
        return true;
      if (this.quit && !this.hasQuit)
      {
        Game1.warpFarmer("BusStop", 12, 11, false);
        if (Intro.roadNoise != null)
          Intro.roadNoise.Stop(AudioStopOptions.Immediate);
        Game1.exitActiveMenu();
        this.hasQuit = true;
        return true;
      }
      switch (this.currentState)
      {
        case 0:
          this.updateUpperClouds(time);
          break;
        case 1:
          this.globalYPanDY = Math.Min(4f, this.globalYPanDY + (float) time.ElapsedGameTime.Milliseconds * (this.speed / 140f));
          this.globalYPan = this.globalYPan - this.globalYPanDY;
          this.updateUpperClouds(time);
          if ((double) this.globalYPan < -1.0)
          {
            this.globalYPan = (float) (this.screenHeight * 3);
            this.currentState = 2;
            this.transformMatrix = Matrix.CreateScale(3f);
            this.transformMatrix.Translation = new Vector3(0.0f, this.globalYPan, 0.0f);
            if (Game1.soundBank != null && Intro.roadNoise != null)
            {
              Intro.roadNoise.SetVariable("Volume", 0.0f);
              Intro.roadNoise.Play();
            }
            Game1.loadForNewGame(false);
            break;
          }
          break;
        case 2:
          this.globalYPanDY = Math.Max(0.5f, this.globalYPan / 100f);
          if (Game1.soundBank != null && Intro.roadNoise != null)
            Intro.roadNoise.SetVariable("Volume", Math.Max(90f, (float) (1.0 / ((double) this.globalYPan / (double) (this.screenHeight * 3)) * 90.0)));
          this.globalYPan = this.globalYPan - this.globalYPanDY;
          this.transformMatrix = Matrix.CreateScale(3f);
          this.transformMatrix.Translation = new Vector3(0.0f, this.globalYPan, 0.0f);
          this.updateRoad(time);
          double globalYpan = (double) this.globalYPan;
          int num1 = 0;
          int val1_1 = 0;
          int num2 = 900;
          Viewport viewport = Game1.graphics.GraphicsDevice.Viewport;
          int height1 = viewport.Height;
          int val2_1 = num2 - height1;
          int num3 = Math.Max(val1_1, val2_1);
          double num4 = (double) (num1 - num3);
          if (globalYpan <= num4)
          {
            int val1_2 = 0;
            int num5 = 900;
            viewport = Game1.graphics.GraphicsDevice.Viewport;
            int height2 = viewport.Height;
            int val2_2 = num5 - height2;
            this.globalYPan = (float) -Math.Max(val1_2, val2_2);
            this.currentState = 3;
            if (Game1.soundBank != null && Intro.roadNoise != null)
            {
              Intro.roadNoise.SetVariable("Volume", 100f);
              break;
            }
            break;
          }
          break;
        case 3:
          this.updateRoad(time);
          this.drivingTimer = this.drivingTimer + (float) time.ElapsedGameTime.Milliseconds;
          if ((double) this.drivingTimer > 5700.0)
          {
            this.drivingTimer = 0.0f;
            this.currentState = 4;
            break;
          }
          break;
        case 4:
          this.updateRoad(time);
          this.drivingTimer = this.drivingTimer + (float) time.ElapsedGameTime.Milliseconds;
          if ((double) this.drivingTimer > 2000.0)
          {
            this.busPosition.X += (float) time.ElapsedGameTime.Milliseconds / 8f;
            if (Game1.soundBank != null && Intro.roadNoise != null)
              Intro.roadNoise.SetVariable("Volume", Math.Max(0.0f, Intro.roadNoise.GetVariable("Volume") - 1f));
            double num5 = 0.0;
            double speed = (double) this.speed;
            TimeSpan elapsedGameTime = time.ElapsedGameTime;
            double num6 = (double) elapsedGameTime.Milliseconds / 70000.0;
            double num7 = speed - num6;
            this.speed = Math.Max((float) num5, (float) num7);
            if (!this.addedSign)
            {
              this.addedSign = true;
              this.roadsideObjects.RemoveAt(this.roadsideObjects.Count - 1);
              this.roadsideObjects.Add(5);
              Game1.playSound("busDriveOff");
            }
            if ((double) this.speed <= 0.0 && this.birdPosition.Equals(Vector2.Zero))
            {
              int num8 = 0;
              for (int index = 0; index < this.roadsideObjects.Count; ++index)
              {
                if (this.roadsideObjects[index] == 5)
                {
                  num8 = index;
                  break;
                }
              }
              this.birdPosition = new Vector2((float) ((double) (num8 * 16) - (double) this.roadPosition - 32.0 + 16.0), -16f);
              Game1.playSound("SpringBirds");
              this.fadeAlpha = 0.0f;
            }
            if (!this.birdPosition.Equals(Vector2.Zero) && (double) this.birdPosition.Y < 116.0)
            {
              float num8 = Math.Max(0.5f, (float) ((116.0 - (double) this.birdPosition.Y) / 116.0 * 2.0));
              this.birdPosition.Y += num8;
              this.birdPosition.X += (float) (Math.Sin((double) this.birdXTimer / (16.0 * Math.PI)) * (double) num8 / 2.0);
              int birdTimer = this.birdTimer;
              elapsedGameTime = time.ElapsedGameTime;
              int milliseconds1 = elapsedGameTime.Milliseconds;
              this.birdTimer = birdTimer + milliseconds1;
              int birdXtimer = this.birdXTimer;
              elapsedGameTime = time.ElapsedGameTime;
              int milliseconds2 = elapsedGameTime.Milliseconds;
              this.birdXTimer = birdXtimer + milliseconds2;
              if (this.birdTimer >= 100)
              {
                this.birdFrame = (this.birdFrame + 1) % 4;
                this.birdTimer = 0;
              }
            }
            else if (!this.birdPosition.Equals(Vector2.Zero))
            {
              this.birdFrame = this.birdTimer > 1500 ? 5 : 4;
              int birdTimer = this.birdTimer;
              elapsedGameTime = time.ElapsedGameTime;
              int milliseconds = elapsedGameTime.Milliseconds;
              this.birdTimer = birdTimer + milliseconds;
              if (this.birdTimer > 2400 || this.birdTimer > 1800 && Game1.random.NextDouble() < 0.006)
              {
                this.birdTimer = 0;
                if (Game1.random.NextDouble() < 0.5)
                {
                  Game1.playSound("SpringBirds");
                  this.birdPosition.Y -= 4f;
                }
              }
            }
            if ((double) this.drivingTimer > 14000.0)
            {
              double fadeAlpha = (double) this.fadeAlpha;
              elapsedGameTime = time.ElapsedGameTime;
              double num8 = (double) elapsedGameTime.Milliseconds * 0.100000001490116 / 128.0;
              this.fadeAlpha = (float) (fadeAlpha + num8);
              if ((double) this.fadeAlpha >= 1.0)
              {
                Game1.warpFarmer("BusStop", 12, 11, false);
                if (Intro.roadNoise != null)
                  Intro.roadNoise.Stop(AudioStopOptions.Immediate);
                Game1.exitActiveMenu();
                return true;
              }
              break;
            }
            break;
          }
          break;
      }
      return false;
    }

    public void doneCreatingCharacter()
    {
      this.characterCreateMenu = (CharacterCustomization) null;
      this.currentState = 1;
      Game1.changeMusicTrack("spring_day_ambient");
    }

    public void receiveLeftClick(int x, int y, bool playSound = true)
    {
      if (this.characterCreateMenu != null)
        this.characterCreateMenu.receiveLeftClick(x, y, true);
      for (int index = this.balloons.Count - 1; index >= 0; --index)
      {
        if (new Rectangle((int) this.balloons[index].position.X * 4 + 16, (int) this.balloons[index].position.Y * 4 + 16, 32, 32).Contains(x, y))
        {
          this.balloons.RemoveAt(index);
          Game1.playSound("coin");
        }
      }
    }

    public void receiveRightClick(int x, int y, bool playSound = true)
    {
      if (this.characterCreateMenu == null)
        return;
      this.characterCreateMenu.receiveRightClick(x, y, true);
    }

    public void releaseLeftClick(int x, int y)
    {
      if (this.characterCreateMenu == null)
        return;
      this.characterCreateMenu.releaseLeftClick(x, y);
    }

    public void leftClickHeld(int x, int y)
    {
      if (this.characterCreateMenu == null)
        return;
      this.characterCreateMenu.leftClickHeld(x, y);
    }

    public void releaseRightClick(int x, int y)
    {
    }

    public void receiveKeyPress(Keys k)
    {
      if (k != Keys.Escape || this.currentState == 1)
        return;
      if (!this.quit)
        Game1.playSound("bigDeSelect");
      this.quit = true;
    }

    public void receiveKeyRelease(Keys k)
    {
    }

    public void draw(SpriteBatch b)
    {
      switch (this.currentState)
      {
        case 1:
          b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
          b.GraphicsDevice.Clear(this.skyColor);
          int tileSize1 = Game1.tileSize;
          int y = Game1.viewport.Height - Game1.tileSize;
          int width = 0;
          int tileSize2 = Game1.tileSize;
          Utility.makeSafe(ref tileSize1, ref y, width, tileSize2);
          SpriteText.drawString(b, Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.3689"), tileSize1, y, 999, -1, 999, 1f, 1f, false, 0, "", -1);
          b.End();
          break;
        case 2:
        case 3:
        case 4:
          this.drawRoadArea(b);
          break;
      }
    }

    public void drawChoosingCharacterArea(SpriteBatch b)
    {
      b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
      b.GraphicsDevice.Clear(this.skyColor);
      if (this.characterCreateMenu != null)
        this.characterCreateMenu.draw(b);
      b.End();
    }

    public void drawRoadArea(SpriteBatch b)
    {
      b.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null, (Effect) null, this.transformMatrix);
      b.GraphicsDevice.Clear(this.roadColor);
      b.Draw(Game1.staminaRect, new Rectangle(0, -this.screenHeight * 2, this.screenWidth, this.screenHeight * 3), this.skyColor);
      b.Draw(Game1.staminaRect, new Rectangle(0, this.screenHeight / 2 - 32, this.screenWidth, this.screenHeight * 4), this.roadColor);
      for (int index = 0; index < this.screenWidth / 112 + 2; ++index)
      {
        if (this.sky[index] == 0)
          b.Draw(this.texture, new Vector2(-this.skyPosition + (float) (index * 112), -16f), new Rectangle?(new Rectangle(128, 0, 112, 96)), Color.White);
        else
          b.Draw(this.texture, new Rectangle((int) -(double) this.skyPosition - 1 + index * 112, -16, 114, 96), new Rectangle?(new Rectangle(128, 0, 1, 96)), Color.White);
      }
      for (int index = 0; index < 8; ++index)
      {
        b.Draw(Game1.mouseCursors, new Vector2((float) (-(double) this.valleyPosition / 2.0 - 10.0) + (float) (index * 639), 70f), new Rectangle?(new Rectangle(0, 886, 639, 148)), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.08f);
        b.Draw(Game1.mouseCursors, new Vector2(-this.valleyPosition + (float) (index * 639), 80f), new Rectangle?(new Rectangle(0, 737, 639, 120)), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.08f);
      }
      for (int index = 0; index < this.road.Count; ++index)
      {
        if (index % 3 == 0)
        {
          b.Draw(this.texture, new Vector2((float) (index * 16) - this.roadPosition, 160f), new Rectangle?(new Rectangle(0, 176, 48, 48)), Color.White);
          b.Draw(this.texture, new Vector2((float) (index * 16 + this.tileSize) - this.roadPosition, 272f), new Rectangle?(new Rectangle(0, 64, 16, 16)), Color.White);
        }
        b.Draw(this.texture, new Vector2((float) (index * 16) - this.roadPosition, 208f), new Rectangle?(new Rectangle(this.road[index] * 16, 240, 16, 16)), Color.White);
      }
      for (int index = 0; index < this.roadsideObjects.Count; ++index)
      {
        switch (this.roadsideObjects[index])
        {
          case 0:
            b.Draw(this.roadsideTexture, new Vector2((float) ((double) (index * 16) - (double) this.roadPosition - 32.0), 96f), new Rectangle?(new Rectangle(48, 0, 48, 96)), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            break;
          case 1:
            b.Draw(this.roadsideTexture, new Vector2((float) ((double) (index * 16) - (double) this.roadPosition - 32.0), 96f), new Rectangle?(new Rectangle(0, 0, 48, 64)), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            b.Draw(this.roadsideTexture, new Vector2((float) ((double) (index * 16) - (double) this.roadPosition - 16.0), 160f), new Rectangle?(new Rectangle(16, 64, 16, 32)), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            break;
          case 2:
            b.Draw(this.roadsideTexture, new Vector2((float) ((double) (index * 16) - (double) this.roadPosition - 32.0), 176f), new Rectangle?(new Rectangle(112, 144, 16, 16)), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            break;
          case 3:
            b.Draw(this.roadsideTexture, new Vector2((float) ((double) (index * 16) - (double) this.roadPosition - 32.0), 176f), new Rectangle?(new Rectangle(112, 160, 16, 16)), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            break;
          case 5:
            b.Draw(this.texture, new Vector2((float) ((double) (index * 16) - (double) this.roadPosition - 32.0), 128f), new Rectangle?(new Rectangle(48, 176, 64, 64)), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            break;
        }
      }
      for (int index = 0; index < this.roadsideFences.Count; ++index)
      {
        if (this.roadsideFences[index] != -1)
        {
          if (this.roadsideFences[index] == 3)
            b.Draw(this.roadsideTexture, new Vector2((float) (index * 16) - this.roadPosition, 176f), new Rectangle?(new Rectangle(144, 256, 16, 32)), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
          else
            b.Draw(this.roadsideTexture, new Vector2((float) (index * 16) - this.roadPosition, 176f), new Rectangle?(new Rectangle(128 + this.roadsideFences[index] * 16, 224, 16, 32)), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
        }
      }
      if (!this.carPosition.Equals(Vector2.Zero))
      {
        b.Draw(this.texture, this.carPosition, new Rectangle?(new Rectangle(160, 112, 80, 64)), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
        b.Draw(this.texture, this.carPosition, new Rectangle?(new Rectangle(160, 176, 80, 64)), this.carColor, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
      }
      b.Draw(this.texture, this.busPosition, new Rectangle?(new Rectangle(0, 0, 128, 64)), Color.White, 0.0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0.0f);
      b.Draw(this.texture, this.busPosition + new Vector2(23.5f, 56.5f) * 1.5f, new Rectangle?(new Rectangle(21, 54, 5, 5)), Color.White, (float) ((double) this.roadPosition / 3.0 / 16.0 * Math.PI * 2.0), new Vector2(2.5f, 2.5f), 1.5f, SpriteEffects.None, 0.0f);
      b.Draw(this.texture, this.busPosition + new Vector2(87.5f, 56.5f) * 1.5f, new Rectangle?(new Rectangle(21, 54, 5, 5)), Color.White, (float) (((double) this.roadPosition + 4.0) / 3.0 / 16.0 * Math.PI * 2.0), new Vector2(2.5f, 2.5f), 1.5f, SpriteEffects.None, 0.0f);
      if (!this.birdPosition.Equals(Vector2.Zero))
        b.Draw(this.texture, this.birdPosition, new Rectangle?(new Rectangle(16 + this.birdFrame * 16, 64, 16, 16)), Color.White);
      if ((double) this.fadeAlpha > 0.0)
        b.Draw(Game1.fadeToBlackRect, new Rectangle(0, 0, this.screenWidth + 2, this.screenHeight * 2), Color.Black * this.fadeAlpha);
      b.End();
    }

    public void changeScreenSize()
    {
      this.screenWidth = Game1.graphics.GraphicsDevice.Viewport.Width / 3;
      this.screenHeight = Game1.graphics.GraphicsDevice.Viewport.Height / 3;
      this.createBeginningOfLevel();
    }

    public void unload()
    {
    }

    public void receiveEventPoke(int data)
    {
      throw new NotImplementedException();
    }

    public string minigameId()
    {
      return (string) null;
    }

    public class Balloon
    {
      public Vector2 position;
      public Color color;

      public Balloon(int screenWidth, int screenHeight)
      {
        int g = Game1.random.Next((int) byte.MaxValue);
        int b = (int) byte.MaxValue - g;
        int r = Game1.random.NextDouble() < 0.5 ? (int) byte.MaxValue : 0;
        this.position = new Vector2((float) Game1.random.Next(screenWidth / 5, screenWidth), (float) screenHeight);
        this.color = new Color(r, g, b);
      }

      public void update(float speed, GameTime time)
      {
        this.position.Y -= (float) ((double) speed * (double) time.ElapsedGameTime.Milliseconds / 16.0);
        this.position.X -= (float) ((double) speed * (double) time.ElapsedGameTime.Milliseconds / 32.0);
      }
    }
  }
}
