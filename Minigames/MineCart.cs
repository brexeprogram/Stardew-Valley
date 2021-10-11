// Decompiled with JetBrains decompiler
// Type: StardewValley.Minigames.MineCart
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Minigames
{
  public class MineCart : IMinigame
  {
    public int pixelScale = 4;
    private int noiseSeed = Game1.random.Next(0, int.MaxValue);
    private int waterfallWidth = 1;
    private int distanceToTravel = -1;
    private List<Point> track = new List<Point>();
    private List<Point> lakeDecor = new List<Point>();
    private List<Point> ceiling = new List<Point>();
    private List<Point> obstacles = new List<Point>();
    private List<MineCart.Spark> sparkShower = new List<MineCart.Spark>();
    public const int track1 = 1;
    public const int track2 = 2;
    public const int noTrack = 0;
    public const int trackSlopeLeft = 3;
    public const int trackSlopeRight = 4;
    public const int minecartObstacle = 1;
    public const int coinObstacle = 2;
    public const int maxTrackDeviationFromZero = 6;
    public const int tilesBeyondViewportToSimulate = 4;
    public const int bgLoopWidth = 96;
    public const int tileOfMineCart = 6;
    public const int gapsBeforeForcedTrack = 4;
    public const int tracksBeforeConsideredObstacle = 2;
    public const float gravity = 0.21f;
    public const float snapMinecartToTrackThreshold = 6f;
    public const float maxDY = 4.5f;
    public float maxJumpHeight;
    public const float jumpStrengthPerTick = 0.6f;
    public const float dyThreshAtWhichJumpIsImpossible = 1f;
    public const int frostArea = 0;
    public const int lavaArea = 1;
    public const int waterArea = 2;
    public const int darkArea = 3;
    public const int heavenlyArea = 4;
    public const int brownArea = 5;
    public const int noSlope = 0;
    public const int slopingUp = 1;
    public const int slopingDown = 2;
    public const int mineLevelMode = 0;
    public const int arcadeTitleScreenMode = 1;
    public const int infiniteMode = 2;
    public const int progressMode = 3;
    public const int highScoreMode = 4;
    public const int respawnTime = 1400;
    public const int distanceToTravelInMineMode = 350;
    public const double ceilingHeightFluctuation = 0.15;
    public const double coinOccurance = 0.01;
    private float speed;
    private float speedAccumulator;
    private float lakeSpeedAccumulator;
    private float backBGPosition;
    private float midBGPosition;
    private float waterFallPosition;
    private int currentTrackY;
    private int screenWidth;
    private int screenHeight;
    private int tileSize;
    private int ytileOffset;
    private int totalMotion;
    private int movingOnSlope;
    private int levelsBeat;
    private int gameMode;
    private int livesLeft;
    private int respawnCounter;
    private int currentTheme;
    private float mineCartYPosition;
    private float mineCartXOffset;
    private float minecartDY;
    private float minecartPositionBeforeJump;
    private float minecartBumpOffset;
    private double lastNoiseValue;
    private double heightChangeThreshold;
    private double obstacleOccurance;
    private double heightFluctuationsThreshold;
    private bool isJumping;
    private bool reachedJumpApex;
    private bool reachedFinish;
    private float screenDarkness;
    private Cue minecartLoop;
    private Texture2D texture;
    private Color backBGTint;
    private Color midBGTint;
    private Color caveTint;
    private Color lakeTint;
    private Color waterfallTint;
    private Color trackShadowTint;
    private Color trackTint;
    private Matrix transformMatrix;

    public MineCart(int whichTheme, int mode)
    {
      this.changeScreenSize();
      this.maxJumpHeight = (float) (Game1.tileSize / this.pixelScale) * 5f;
      this.texture = Game1.content.Load<Texture2D>("Minigames\\MineCart");
      if (Game1.soundBank != null)
      {
        this.minecartLoop = Game1.soundBank.GetCue(nameof (minecartLoop));
        this.minecartLoop.Play();
      }
      this.ytileOffset = this.screenHeight / 2 / this.tileSize;
      this.gameMode = mode;
      this.setGameModeParameters();
      this.setUpTheme(whichTheme);
      this.createBeginningOfLevel();
      this.screenDarkness = 1f;
    }

    public bool overrideFreeMouseMovement()
    {
      return false;
    }

    public bool tick(GameTime time)
    {
      TimeSpan timeSpan;
      if (!this.reachedFinish && (this.livesLeft > 0 || this.gameMode == 2) && (double) this.screenDarkness > 0.0)
      {
        double screenDarkness = (double) this.screenDarkness;
        timeSpan = time.ElapsedGameTime;
        double num = (double) timeSpan.Milliseconds * (1.0 / 500.0);
        this.screenDarkness = (float) (screenDarkness - num);
      }
      int num1 = this.track.ElementAt<Point>(6).X == 0 ? 9999 : this.track.ElementAt<Point>(6).Y * this.tileSize + (this.track.ElementAt<Point>(6).X == 3 ? (int) -(double) this.speedAccumulator : (this.track.ElementAt<Point>(6).X == 4 ? (int) ((double) this.speedAccumulator - 16.0) : 0));
      if (this.respawnCounter <= 0 || this.track[6].X == 0 || (this.obstacles[6].X == 1 || this.obstacles[7].X == 1))
      {
        double speedAccumulator1 = (double) this.speedAccumulator;
        timeSpan = time.ElapsedGameTime;
        double num2 = (double) timeSpan.Milliseconds * (double) this.speed;
        this.speedAccumulator = (float) (speedAccumulator1 + num2);
        num1 = this.track.ElementAt<Point>(6).X == 0 ? 9999 : this.track.ElementAt<Point>(6).Y * this.tileSize + (this.track.ElementAt<Point>(6).X == 3 ? (int) -(double) this.speedAccumulator : (this.track.ElementAt<Point>(6).X == 4 ? (int) ((double) this.speedAccumulator - 16.0) : 0));
        if ((double) this.speedAccumulator >= (double) this.tileSize)
        {
          if (!this.isJumping && this.movingOnSlope == 0 && Game1.random.NextDouble() < 0.5)
            this.minecartBumpOffset = (float) Game1.random.Next(1, 3);
          if ((this.totalMotion + 1) % 1000 == 0)
            Game1.playSound("newArtifact");
          else if ((this.totalMotion + 1) % 100 == 0)
            Game1.playSound("Pickup_Coin15");
          this.totalMotion = this.totalMotion + 1;
          if (this.totalMotion > Game1.minecartHighScore)
            Game1.minecartHighScore = this.totalMotion;
          if (this.distanceToTravel != -1 && this.totalMotion >= this.distanceToTravel + this.screenWidth / this.tileSize)
          {
            if (!this.reachedFinish)
              Game1.playSound("reward");
            this.reachedFinish = true;
          }
          this.track.RemoveAt(0);
          this.ceiling.RemoveAt(0);
          this.obstacles.RemoveAt(0);
          if (this.distanceToTravel == -1 || this.totalMotion < this.distanceToTravel)
          {
            double num3 = NoiseGenerator.Noise(this.totalMotion, this.noiseSeed);
            Point point = Point.Zero;
            if (num3 > this.heightChangeThreshold && this.lastNoiseValue <= this.heightChangeThreshold)
              this.currentTrackY = Math.Max(this.currentTrackY - Game1.random.Next(1, 2), -6);
            else if (num3 < this.heightChangeThreshold && this.lastNoiseValue >= this.heightChangeThreshold)
              this.currentTrackY = Math.Min(this.currentTrackY + Game1.random.Next(1, this.currentTrackY <= -3 ? 6 : 3), 4);
            else if (Math.Abs(num3 - this.lastNoiseValue) > this.heightFluctuationsThreshold)
              this.currentTrackY = this.track[this.track.Count - 1].X != 0 ? Math.Max(Math.Min(4, this.currentTrackY + Game1.random.Next(-3, 3)), -6) : Math.Max(-6, Math.Min(6, this.currentTrackY + Game1.random.Next(1, 1)));
            if (num3 < -0.5)
            {
              bool flag = false;
              for (int index = 0; index < 4 - (Game1.random.NextDouble() < 0.1 ? 1 : 0); ++index)
              {
                if (this.track[this.track.Count - 1 - index].X != 0)
                {
                  flag = true;
                  break;
                }
              }
              point = !flag ? new Point(Game1.random.Next(1, 3), Game1.random.NextDouble() >= 0.5 || this.currentTrackY >= 6 ? this.currentTrackY + 1 : this.currentTrackY) : new Point(0, 999);
            }
            else
              point = new Point(Game1.random.Next(1, 3), this.currentTrackY);
            if (this.track[this.track.Count - 1].X == 0 && point.X != 0)
              point.Y = Math.Min(6, point.Y + 1);
            if (point.Y == this.track[this.track.Count - 1].Y - 1)
            {
              this.track.RemoveAt(this.track.Count - 1);
              this.track.Add(new Point(3, this.currentTrackY + 1));
            }
            else if (point.Y == this.track[this.track.Count - 1].Y + 1)
              point.X = 4;
            this.track.Add(point);
            this.ceiling.Add(new Point(Game1.random.Next(200), Math.Min(this.currentTrackY - 5 + this.ytileOffset, Math.Max(0, this.ceiling.Last<Point>().Y + (Game1.random.NextDouble() < 0.15 ? Game1.random.Next(-1, 2) : 0)))));
            bool flag1 = false;
            for (int index = 0; index < 2; ++index)
            {
              if (this.track[this.track.Count - 1 - index].X == 0 || this.track[this.track.Count - 1 - index - 1].Y != this.track[this.track.Count - 1 - index].Y)
              {
                flag1 = true;
                break;
              }
            }
            if (!flag1 && Game1.random.NextDouble() < this.obstacleOccurance && (this.currentTrackY > -2 && this.track.Last<Point>().X != 3) && this.track.Last<Point>().X != 4)
              this.obstacles.Add(new Point(1, this.currentTrackY));
            else
              this.obstacles.Add(Point.Zero);
            this.lastNoiseValue = num3;
          }
          else
          {
            this.track.Add(new Point(Game1.random.Next(1, 3), this.currentTrackY));
            this.ceiling.Add(new Point(Game1.random.Next(200), this.ceiling.Last<Point>().Y));
            this.obstacles.Add(Point.Zero);
            this.lakeDecor.Add(new Point(Game1.random.Next(2), Game1.random.Next(this.ytileOffset + 1, this.screenHeight / this.tileSize)));
          }
          this.speedAccumulator = this.speedAccumulator % (float) this.tileSize;
        }
        double speedAccumulator2 = (double) this.lakeSpeedAccumulator;
        timeSpan = time.ElapsedGameTime;
        double num4 = (double) timeSpan.Milliseconds * ((double) this.speed / 4.0);
        this.lakeSpeedAccumulator = (float) (speedAccumulator2 + num4);
        if ((double) this.lakeSpeedAccumulator >= (double) this.tileSize)
        {
          this.lakeSpeedAccumulator = this.lakeSpeedAccumulator % (float) this.tileSize;
          this.lakeDecor.RemoveAt(0);
          this.lakeDecor.Add(new Point(Game1.random.Next(2), Game1.random.Next(this.ytileOffset + 3, this.screenHeight / this.tileSize)));
        }
        double backBgPosition = (double) this.backBGPosition;
        timeSpan = time.ElapsedGameTime;
        double num5 = (double) timeSpan.Milliseconds * ((double) this.speed / 5.0);
        this.backBGPosition = (float) (backBgPosition + num5);
        this.backBGPosition = this.backBGPosition % 96f;
        double midBgPosition = (double) this.midBGPosition;
        timeSpan = time.ElapsedGameTime;
        double num6 = (double) timeSpan.Milliseconds * ((double) this.speed / 4.0);
        this.midBGPosition = (float) (midBgPosition + num6);
        this.midBGPosition = this.midBGPosition % 96f;
        double waterFallPosition = (double) this.waterFallPosition;
        timeSpan = time.ElapsedGameTime;
        double num7 = (double) timeSpan.Milliseconds * ((double) this.speed * 6.0 / 5.0);
        this.waterFallPosition = (float) (waterFallPosition + num7);
        if ((double) this.waterFallPosition > (double) (this.screenWidth * 3 / 2))
        {
          this.waterFallPosition = this.waterFallPosition % (float) (this.screenWidth * 3 / 2);
          this.waterfallWidth = Game1.random.Next(6);
        }
      }
      else
      {
        int respawnCounter = this.respawnCounter;
        timeSpan = time.ElapsedGameTime;
        int milliseconds = timeSpan.Milliseconds;
        this.respawnCounter = respawnCounter - milliseconds;
        this.mineCartYPosition = (float) num1;
      }
      if ((double) Math.Abs(this.mineCartYPosition - (float) num1) <= 6.0 && (double) this.minecartDY >= 0.0 && this.movingOnSlope == 0)
      {
        if ((double) this.minecartDY > 0.0)
        {
          this.mineCartYPosition = (float) num1;
          this.minecartDY = 0.0f;
          if (Game1.soundBank != null)
          {
            Game1.soundBank.GetCue("parry").Play();
            this.minecartLoop = Game1.soundBank.GetCue("minecartLoop");
            this.minecartLoop.Play();
          }
          this.isJumping = false;
          this.reachedJumpApex = false;
          this.createSparkShower();
        }
        if (this.track[6].X == 3)
        {
          this.movingOnSlope = 1;
          this.createSparkShower();
        }
        else if (this.track[6].X == 4)
        {
          this.movingOnSlope = 2;
          this.createSparkShower();
        }
      }
      else if (!this.isJumping && (double) Math.Abs(this.mineCartYPosition - (float) num1) <= 6.0 && (this.track[6].X == 3 || this.track[6].X == 4))
      {
        this.mineCartYPosition = (float) num1;
        if ((double) this.mineCartYPosition == (double) num1 && this.track[6].X == 3)
        {
          this.movingOnSlope = 1;
          if (this.respawnCounter <= 0)
            this.createSparkShower(Game1.random.Next(2));
        }
        else if ((double) this.mineCartYPosition == (double) num1 && this.track[6].X == 4)
        {
          this.movingOnSlope = 2;
          if (this.respawnCounter <= 0)
            this.createSparkShower(Game1.random.Next(2));
        }
        this.minecartDY = 0.0f;
      }
      else
      {
        this.movingOnSlope = 0;
        this.minecartDY = this.minecartDY + (!this.reachedJumpApex && this.isJumping || (double) this.mineCartYPosition == (double) num1 ? 0.0f : 0.21f);
        if ((double) this.minecartDY > 0.0)
          this.minecartDY = Math.Min(this.minecartDY, 9f);
        if ((double) this.minecartDY > 0.0 || (double) this.minecartPositionBeforeJump - (double) this.mineCartYPosition <= (double) this.maxJumpHeight)
          this.mineCartYPosition = this.mineCartYPosition + this.minecartDY;
      }
      if ((double) this.minecartDY > 0.0 && this.minecartLoop != null && this.minecartLoop.IsPlaying)
        this.minecartLoop.Stop(AudioStopOptions.Immediate);
      if (this.reachedFinish)
      {
        double mineCartXoffset = (double) this.mineCartXOffset;
        double speed = (double) this.speed;
        timeSpan = time.ElapsedGameTime;
        double milliseconds = (double) timeSpan.Milliseconds;
        double num2 = speed * milliseconds;
        this.mineCartXOffset = (float) (mineCartXoffset + num2);
        if (Game1.random.NextDouble() < 0.25)
          this.createSparkShower();
      }
      if ((double) this.mineCartXOffset > (double) (this.screenWidth - 6 * this.tileSize + this.tileSize))
      {
        switch (this.gameMode)
        {
          case 0:
            double screenDarkness1 = (double) this.screenDarkness;
            timeSpan = time.ElapsedGameTime;
            double num8 = (double) timeSpan.Milliseconds / 2000.0;
            this.screenDarkness = (float) (screenDarkness1 + num8);
            if ((double) this.screenDarkness >= 1.0)
            {
              if (Game1.mine != null)
              {
                Game1.mine.mineLevel += 3;
                Game1.warpFarmer("UndergroundMine", 16, 16, false);
                Game1.fadeToBlackAlpha = 1f;
              }
              return true;
            }
            break;
          case 3:
            double screenDarkness2 = (double) this.screenDarkness;
            timeSpan = time.ElapsedGameTime;
            double num9 = (double) timeSpan.Milliseconds / 2000.0;
            this.screenDarkness = (float) (screenDarkness2 + num9);
            if ((double) this.screenDarkness >= 1.0)
            {
              this.reachedFinish = false;
              this.currentTheme = (this.currentTheme + 1) % 6;
              this.levelsBeat = this.levelsBeat + 1;
              if (this.levelsBeat == 6)
              {
                if (!Game1.player.hasOrWillReceiveMail("JunimoKart"))
                  Game1.addMailForTomorrow("JunimoKart", false, false);
                this.unload();
                Game1.currentMinigame = (IMinigame) null;
                DelayedAction.playSoundAfterDelay("discoverMineral", 1000);
                Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:MineCart.cs.12106"));
                break;
              }
              this.setUpTheme(this.currentTheme);
              this.restartLevel();
              break;
            }
            break;
        }
      }
      if ((double) this.speedAccumulator >= (double) (this.tileSize / 2) && ((int) ((double) this.mineCartYPosition / (double) this.tileSize) == this.obstacles[7].Y || (int) ((double) this.mineCartYPosition / (double) this.tileSize - (double) (this.tileSize - 1)) == this.obstacles[7].Y))
      {
        switch (this.obstacles[7].X)
        {
          case 1:
            Game1.playSound("woodWhack");
            this.mineCartYPosition = (float) this.screenHeight;
            break;
          case 2:
            Game1.playSound("money");
            this.obstacles.RemoveAt(6);
            this.obstacles.Insert(6, Point.Zero);
            break;
        }
      }
      if ((double) this.mineCartYPosition > (double) this.screenHeight)
      {
        this.mineCartYPosition = -999999f;
        this.livesLeft = this.livesLeft - 1;
        Game1.playSound("fishEscape");
        if (this.gameMode == 0 && (double) this.livesLeft < 0.0)
        {
          this.mineCartYPosition = 999999f;
          this.livesLeft = this.livesLeft + 1;
          double screenDarkness3 = (double) this.screenDarkness;
          timeSpan = time.ElapsedGameTime;
          double num2 = (double) timeSpan.Milliseconds * (1.0 / 1000.0);
          this.screenDarkness = (float) (screenDarkness3 + num2);
          if ((double) this.screenDarkness >= 1.0)
          {
            if (Game1.player.health > 1)
            {
              Game1.player.health = 1;
              Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:MineCart.cs.12108"));
            }
            else
              Game1.player.health = 0;
            return true;
          }
        }
        else if (this.gameMode == 4 || this.gameMode == 3 && this.livesLeft < 0)
        {
          if (this.gameMode == 3)
          {
            this.levelsBeat = 0;
            this.setUpTheme(5);
          }
          this.restartLevel();
        }
        else
        {
          this.respawnCounter = 1400;
          this.minecartDY = 0.0f;
          this.isJumping = false;
          this.reachedJumpApex = false;
          if (this.gameMode == 2)
            this.totalMotion = 0;
        }
      }
      this.minecartBumpOffset = Math.Max(0.0f, this.minecartBumpOffset - 0.5f);
      for (int index = this.sparkShower.Count - 1; index >= 0; --index)
      {
        this.sparkShower[index].dy += 0.105f;
        this.sparkShower[index].x += this.sparkShower[index].dx;
        this.sparkShower[index].y += this.sparkShower[index].dy;
        // ISSUE: explicit reference operation
        // ISSUE: variable of a reference type
        Color& local1 = @this.sparkShower[index].c;
        double num2 = 0.0;
        double val1_1 = 0.0;
        timeSpan = time.TotalGameTime;
        double val2_1 = Math.Sin((double) timeSpan.Milliseconds / (20.0 * Math.PI / (double) this.sparkShower[index].dx)) * (double) byte.MaxValue;
        double num3 = Math.Max(val1_1, val2_1);
        int num4 = (int) (byte) (num2 + num3);
        // ISSUE: explicit reference operation
        (^local1).B = (byte) num4;
        if (this.reachedFinish)
        {
          // ISSUE: explicit reference operation
          // ISSUE: variable of a reference type
          Color& local2 = @this.sparkShower[index].c;
          double num5 = 0.0;
          double val1_2 = 0.0;
          timeSpan = time.TotalGameTime;
          double val2_2 = Math.Sin((double) (timeSpan.Milliseconds + 50) / (20.0 * Math.PI / (double) this.sparkShower[index].dx)) * (double) byte.MaxValue;
          double num6 = Math.Max(val1_2, val2_2);
          int num7 = (int) (byte) (num5 + num6);
          // ISSUE: explicit reference operation
          (^local2).R = (byte) num7;
          // ISSUE: explicit reference operation
          // ISSUE: variable of a reference type
          Color& local3 = @this.sparkShower[index].c;
          double num10 = 0.0;
          double val1_3 = 0.0;
          timeSpan = time.TotalGameTime;
          double val2_3 = Math.Sin((double) (timeSpan.Milliseconds + 100) / (20.0 * Math.PI / (double) this.sparkShower[index].dx)) * (double) byte.MaxValue;
          double num11 = Math.Max(val1_3, val2_3);
          int num12 = (int) (byte) (num10 + num11);
          // ISSUE: explicit reference operation
          (^local3).G = (byte) num12;
          if ((int) this.sparkShower[index].c.R == 0)
            this.sparkShower[index].c.R = byte.MaxValue;
          if ((int) this.sparkShower[index].c.G == 0)
            this.sparkShower[index].c.G = byte.MaxValue;
        }
        if ((double) this.sparkShower[index].y > (double) this.screenHeight)
          this.sparkShower.RemoveAt(index);
      }
      return false;
    }

    public void receiveLeftClick(int x, int y, bool playSound = true)
    {
      this.jump();
    }

    public void releaseLeftClick(int x, int y)
    {
      this.releaseJump();
    }

    public void releaseRightClick(int x, int y)
    {
    }

    public void receiveRightClick(int x, int y, bool playSound = true)
    {
    }

    public void receiveKeyPress(Keys k)
    {
      if (!k.Equals((object) Keys.Escape) && !Game1.options.doesInputListContain(Game1.options.menuButton, k) || Game1.isAnyGamePadButtonBeingPressed() && !GamePad.GetState(Game1.playerOneIndex).IsButtonDown(Buttons.Start))
        return;
      this.unload();
      Game1.playSound("bigDeSelect");
      Game1.currentMinigame = (IMinigame) null;
    }

    public void receiveKeyRelease(Keys k)
    {
    }

    private void restartLevel()
    {
      this.track.Clear();
      this.ceiling.Clear();
      this.lakeDecor.Clear();
      this.obstacles.Clear();
      this.totalMotion = 0;
      this.speedAccumulator = 0.0f;
      this.currentTrackY = 0;
      this.mineCartYPosition = 0.0f;
      this.minecartDY = 0.0f;
      this.isJumping = false;
      this.reachedJumpApex = false;
      this.reachedFinish = false;
      this.movingOnSlope = 0;
      this.mineCartXOffset = 0.0f;
      this.createBeginningOfLevel();
      this.setGameModeParameters();
    }

    public void createSparkShower()
    {
      int num = Game1.random.Next(3, 7);
      for (int index = 0; index < num; ++index)
        this.sparkShower.Add(new MineCart.Spark((float) (6 * this.tileSize - 3) + this.mineCartXOffset, (float) ((double) this.mineCartYPosition + (double) (this.ytileOffset * this.tileSize) + (double) this.tileSize - 4.0), (float) Game1.random.Next(-200, 5) / 100f, (float) -Game1.random.Next(5, 150) / 100f));
    }

    public void createSparkShower(int number)
    {
      for (int index = 0; index < number; ++index)
        this.sparkShower.Add(new MineCart.Spark((float) (6 * this.tileSize - 3) + this.mineCartXOffset, (float) ((double) this.mineCartYPosition + (double) (this.ytileOffset * this.tileSize) + (double) this.tileSize - 4.0), (float) Game1.random.Next(-200, 5) / 100f, (float) -Game1.random.Next(5, 150) / 100f));
    }

    public void createBeginningOfLevel()
    {
      for (int index = 0; index < this.screenWidth / this.tileSize + 4; ++index)
      {
        this.track.Add(new Point(Game1.random.Next(1, 3), 0));
        this.ceiling.Add(new Point(Game1.random.Next(200), 0));
        this.obstacles.Add(Point.Zero);
        this.lakeDecor.Add(new Point(Game1.random.Next(2), Game1.random.Next(this.ytileOffset + 3, this.screenHeight / this.tileSize)));
      }
    }

    public void setGameModeParameters()
    {
      switch (this.gameMode)
      {
        case 0:
          this.distanceToTravel = 200;
          this.livesLeft = 3;
          break;
        case 3:
          this.distanceToTravel = 200;
          this.livesLeft = 3;
          break;
      }
    }

    public void setUpTheme(int whichTheme)
    {
      switch (whichTheme)
      {
        case 0:
          this.backBGTint = new Color(254, 254, 254);
          this.midBGTint = new Color(254, 254, 254);
          this.caveTint = new Color(230, 244, 254);
          this.lakeTint = new Color(150, 210, (int) byte.MaxValue);
          this.waterfallTint = Color.LightCyan * 0.5f;
          this.trackTint = Color.LightCyan;
          this.speed = 0.085f;
          NoiseGenerator.Amplitude = 2.8;
          NoiseGenerator.Frequency = 0.18;
          this.heightChangeThreshold = 0.85;
          this.obstacleOccurance = 0.05;
          this.heightFluctuationsThreshold = 0.35;
          this.trackShadowTint = Color.DarkSlateBlue;
          break;
        case 1:
          this.backBGTint = Color.DarkRed;
          this.midBGTint = Color.DarkSalmon;
          this.caveTint = Color.DarkRed;
          this.lakeTint = Color.DarkRed;
          this.trackTint = Color.DarkGray;
          this.waterfallTint = Color.Red * 0.9f;
          this.trackShadowTint = Color.DarkOrange;
          this.speed = 0.12f;
          this.heightChangeThreshold = 0.8;
          NoiseGenerator.Amplitude = 3.0;
          NoiseGenerator.Frequency = 0.18;
          this.obstacleOccurance = 0.05;
          this.heightFluctuationsThreshold = 0.2;
          break;
        case 2:
          this.backBGTint = new Color(50, 150, 225);
          this.midBGTint = new Color(120, 170, 225);
          this.caveTint = Color.SlateGray;
          this.lakeTint = new Color(30, 120, 215);
          this.waterfallTint = Color.White * 0.5f;
          this.trackTint = Color.Gray;
          this.speed = 0.085f;
          NoiseGenerator.Amplitude = 3.0;
          NoiseGenerator.Frequency = 0.15;
          this.heightChangeThreshold = 0.9;
          this.obstacleOccurance = 0.05;
          this.heightFluctuationsThreshold = 0.4;
          this.trackShadowTint = Color.DarkSlateBlue;
          break;
        case 3:
          this.backBGTint = new Color(60, 60, 60);
          this.midBGTint = new Color(60, 60, 60);
          this.caveTint = new Color(70, 70, 70);
          this.lakeTint = new Color(60, 70, 80);
          this.trackTint = Color.DimGray;
          this.waterfallTint = Color.Black * 0.0f;
          this.trackShadowTint = Color.Black;
          this.speed = 0.1f;
          this.heightChangeThreshold = 0.7;
          NoiseGenerator.Amplitude = 3.0;
          NoiseGenerator.Frequency = 0.2;
          this.obstacleOccurance = 0.0;
          this.heightFluctuationsThreshold = 0.2;
          break;
        case 4:
          this.backBGTint = Color.SeaGreen;
          this.midBGTint = Color.Green;
          this.caveTint = new Color((int) byte.MaxValue, 200, 60);
          this.lakeTint = Color.Lime;
          this.trackTint = Color.LightSlateGray;
          this.waterfallTint = Color.ForestGreen * 0.5f;
          this.trackShadowTint = new Color(0, 180, 50);
          this.speed = 0.08f;
          this.heightChangeThreshold = 0.6;
          NoiseGenerator.Amplitude = 3.1;
          NoiseGenerator.Frequency = 0.24;
          this.obstacleOccurance = 0.05;
          this.heightFluctuationsThreshold = 0.15;
          break;
        case 5:
          this.backBGTint = Color.DarkKhaki;
          this.midBGTint = Color.SandyBrown;
          this.caveTint = Color.SandyBrown;
          this.lakeTint = Color.MediumAquamarine;
          this.trackTint = Color.Beige;
          this.waterfallTint = Color.MediumAquamarine * 0.9f;
          this.trackShadowTint = new Color(60, 60, 60);
          this.speed = 0.085f;
          this.heightChangeThreshold = 0.8;
          NoiseGenerator.Amplitude = 2.0;
          NoiseGenerator.Frequency = 0.12;
          this.obstacleOccurance = 0.05;
          this.heightFluctuationsThreshold = 0.25;
          break;
      }
      this.currentTheme = whichTheme;
    }

    private void jump()
    {
      if ((double) this.minecartDY >= 1.0 || this.respawnCounter > 0)
        return;
      if (!this.isJumping)
      {
        this.movingOnSlope = 0;
        this.minecartPositionBeforeJump = this.mineCartYPosition;
        this.isJumping = true;
        if (this.minecartLoop != null)
          this.minecartLoop.Stop(AudioStopOptions.Immediate);
        if (Game1.soundBank != null)
        {
          Cue cue = Game1.soundBank.GetCue("pickUpItem");
          string name = "Pitch";
          double num = 200.0;
          cue.SetVariable(name, (float) num);
          cue.Play();
        }
      }
      if (this.reachedJumpApex)
        return;
      this.minecartDY = Math.Max(-4.5f, this.minecartDY - 0.6f);
      if ((double) this.minecartDY > -4.5)
        return;
      this.reachedJumpApex = true;
    }

    private void releaseJump()
    {
      if (!this.isJumping)
        return;
      this.reachedJumpApex = true;
    }

    public void draw(SpriteBatch b)
    {
      b.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null, (Effect) null, this.transformMatrix);
      for (int index = 0; index <= this.screenWidth / this.tileSize + 1; ++index)
        b.Draw(this.texture, new Rectangle(index * this.tileSize - (int) this.lakeSpeedAccumulator, this.tileSize * 9, this.tileSize, this.screenHeight - 96), new Rectangle?(new Rectangle(0, 80, 16, 97)), this.lakeTint);
      for (int index = 0; index < this.lakeDecor.Count; ++index)
        b.Draw(this.texture, new Vector2((float) (index * this.tileSize) - this.lakeSpeedAccumulator, (float) (this.lakeDecor.ElementAt<Point>(index).Y * this.tileSize)), new Rectangle?(new Rectangle(32 + this.lakeDecor.ElementAt<Point>(index).X * this.tileSize, 0, 16, 16)), this.lakeDecor.ElementAt<Point>(index).X == 0 ? this.midBGTint : this.lakeTint);
      for (int index = 0; index <= this.screenWidth / 96 + 2; ++index)
        b.Draw(this.texture, new Vector2(-this.backBGPosition + (float) (index * 96), (float) (this.tileSize * 2)), new Rectangle?(new Rectangle(64, 162, 96, 111)), this.backBGTint);
      for (int index = 0; index < this.screenWidth / 96 + 2; ++index)
        b.Draw(this.texture, new Vector2(-this.midBGPosition + (float) (index * 96), 0.0f), new Rectangle?(new Rectangle(64, 0, 96, 162)), this.midBGTint);
      for (int index1 = 0; index1 < this.track.Count; ++index1)
      {
        if (this.track.ElementAt<Point>(index1).X != 0)
        {
          b.Draw(this.texture, new Vector2(-this.speedAccumulator + (float) (index1 * this.tileSize), (float) ((this.track.ElementAt<Point>(index1).Y + this.ytileOffset) * this.tileSize - this.tileSize)), new Rectangle?(new Rectangle((this.track.ElementAt<Point>(index1).X - 1) * 16, 180, 16, 32)), this.trackTint);
          float num = 0.0f;
          for (int index2 = this.track.ElementAt<Point>(index1).Y + 1; index2 < this.screenHeight / this.tileSize; ++index2)
          {
            b.Draw(this.texture, new Vector2(-this.speedAccumulator + (float) (index1 * this.tileSize), (float) ((index2 + this.ytileOffset) * this.tileSize)), new Rectangle?(new Rectangle(16 + (index2 % 2 == 0 ? (this.track.ElementAt<Point>(index1).X + 1) % 2 : this.track.ElementAt<Point>(index1).X % 2) * 16, 32, 16, 16)), this.trackTint);
            b.Draw(this.texture, new Vector2(-this.speedAccumulator + (float) (index1 * this.tileSize), (float) ((index2 + this.ytileOffset) * this.tileSize)), new Rectangle?(new Rectangle(16 + (index2 % 2 == 0 ? (this.track.ElementAt<Point>(index1).X + 1) % 2 : this.track.ElementAt<Point>(index1).X % 2) * 16, 32, 16, 16)), this.trackShadowTint * num);
            num += 0.1f;
          }
        }
      }
      for (int index = 0; index < this.obstacles.Count; ++index)
      {
        switch (this.obstacles[index].X)
        {
          case 1:
            b.Draw(this.texture, new Vector2(-this.speedAccumulator + (float) (index * this.tileSize), (float) ((this.obstacles[index].Y + this.ytileOffset) * this.tileSize)), new Rectangle?(new Rectangle(16, 0, 16, 16)), Color.White);
            break;
          case 2:
            b.Draw(Game1.debrisSpriteSheet, new Vector2(-this.speedAccumulator + (float) (index * this.tileSize), (float) ((this.obstacles[index].Y + this.ytileOffset) * this.tileSize)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 8, -1, -1)), Color.White, 0.0f, new Vector2((float) (Game1.tileSize / 2), 0.0f), 0.25f, SpriteEffects.None, 0.0f);
            break;
        }
      }
      if (this.respawnCounter / 200 % 2 == 0)
      {
        b.Draw(this.texture, new Vector2((float) (6 * this.tileSize + this.tileSize / 2) + this.mineCartXOffset, (float) ((double) (this.ytileOffset * this.tileSize) + (double) this.mineCartYPosition + (double) this.tileSize - (double) this.minecartBumpOffset - 4.0)), new Rectangle?(new Rectangle(0, 0, 16, 16)), Color.White, (double) this.minecartDY < 0.0 ? (float) ((double) this.minecartDY / 3.14159274101257 / 2.0) : 0.0f, new Vector2(16f, 16f), 1f, SpriteEffects.None, 0.0f);
        Game1.player.faceDirection(1);
        b.Draw(Game1.mouseCursors, new Vector2((float) (6 * this.tileSize - 2) + this.mineCartXOffset, (float) ((double) (this.ytileOffset * this.tileSize - 22) + (double) this.mineCartYPosition + (double) this.tileSize + (double) this.tileSize - (double) this.minecartBumpOffset - 8.0)), new Rectangle?(new Rectangle(294 + (int) (Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 400.0) / 100 * 16, 1432, 16, 16)), Color.Lime, (double) this.minecartDY < 0.0 ? (float) ((double) this.minecartDY / 3.14159274101257 / 4.0) : 0.0f, new Vector2(8f, 8f), 0.6666667f, SpriteEffects.None, 0.1f);
        b.Draw(this.texture, new Vector2((float) (6 * this.tileSize + this.tileSize / 2) + this.mineCartXOffset, (float) ((double) (this.ytileOffset * this.tileSize) + (double) this.mineCartYPosition + (double) this.tileSize - (double) this.minecartBumpOffset - 4.0 + 8.0)), new Rectangle?(new Rectangle(0, 8, 16, 8)), Color.White, (double) this.minecartDY < 0.0 ? (float) ((double) this.minecartDY / 3.14159274101257 / 4.0) : 0.0f, new Vector2(16f, 16f), 1f, SpriteEffects.None, 0.1f);
      }
      foreach (MineCart.Spark spark in this.sparkShower)
        b.Draw(Game1.staminaRect, new Rectangle((int) spark.x, (int) spark.y, this.pixelScale / 4, this.pixelScale / 4), spark.c);
      int num1 = 0;
      while (num1 < this.waterfallWidth)
      {
        for (int index = -2; index <= this.screenHeight / this.tileSize + 1; ++index)
          b.Draw(this.texture, new Vector2((float) (this.screenWidth + this.tileSize * num1) - this.waterFallPosition, (float) (index * this.tileSize) + this.lakeSpeedAccumulator * 2f), new Rectangle?(new Rectangle(48, 32, 16, 16)), this.waterfallTint);
        num1 += 2;
      }
      if (this.gameMode != 2 && this.totalMotion < this.distanceToTravel + this.screenWidth / this.tileSize)
      {
        if (this.gameMode != 4)
        {
          for (int index = 0; index < this.livesLeft; ++index)
            b.Draw(this.texture, new Vector2((float) (this.screenWidth - index * (this.tileSize + 2) - this.tileSize), 0.0f), new Rectangle?(new Rectangle(0, 0, 16, 16)), Color.White);
        }
        b.Draw(Game1.staminaRect, new Rectangle(this.pixelScale, this.pixelScale, this.tileSize * 8, this.pixelScale), Color.LightGray);
        b.Draw(Game1.staminaRect, new Rectangle(this.pixelScale + (int) ((double) this.totalMotion / (double) (this.distanceToTravel + this.screenWidth / this.tileSize) * (double) (this.tileSize * 8 - this.pixelScale)), this.pixelScale, this.pixelScale, this.pixelScale), Color.Aquamarine);
        for (int index = 0; index < 4; ++index)
        {
          b.Draw(Game1.staminaRect, new Rectangle(this.pixelScale + this.tileSize * 8, this.pixelScale + index * (this.pixelScale / 4), this.pixelScale / 4, this.pixelScale / 4), index % 2 == 0 ? Color.White : Color.Black);
          b.Draw(Game1.staminaRect, new Rectangle(this.pixelScale + this.tileSize * 8 + this.pixelScale / 4, this.pixelScale + index * (this.pixelScale / 4), this.pixelScale / 4, this.pixelScale / 4), index % 2 == 0 ? Color.Black : Color.White);
        }
        b.DrawString(Game1.dialogueFont, string.Concat((object) (this.levelsBeat + 1)), new Vector2((float) (this.pixelScale * 2 + this.tileSize * 8), (float) this.pixelScale / 2f), Color.Orange, 0.0f, Vector2.Zero, 1f / (float) Game1.pixelZoom, SpriteEffects.None, 0.0f);
      }
      else if (this.gameMode == 2)
      {
        string str = Game1.content.LoadString("Strings\\StringsFromCSFiles:MineCart.cs.12115");
        b.DrawString(Game1.dialogueFont, Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingGame.cs.10444", (object) this.totalMotion), new Vector2(1f, 1f), Color.White, 0.0f, Vector2.Zero, (float) (1.0 / (double) Game1.pixelZoom), SpriteEffects.None, 0.0f);
        b.DrawString(Game1.dialogueFont, str + (object) Game1.minecartHighScore, new Vector2(128f, 1f), Color.White, 0.0f, Vector2.Zero, 1f / (float) Game1.pixelZoom, SpriteEffects.None, 0.0f);
      }
      if ((double) this.screenDarkness > 0.0)
        b.Draw(Game1.staminaRect, new Rectangle(0, 0, this.screenWidth, this.screenHeight + this.tileSize), Color.Black * this.screenDarkness);
      b.End();
    }

    public void changeScreenSize()
    {
      this.pixelScale = 4;
      int num = Game1.viewport.Height < 1000 ? 3 : 4;
      this.screenWidth = Game1.viewport.Width / num;
      this.screenHeight = Game1.viewport.Height / num;
      this.tileSize = Game1.tileSize / this.pixelScale;
      this.ytileOffset = this.screenHeight / 2 / this.tileSize;
      this.maxJumpHeight = (float) (Game1.tileSize / this.pixelScale) * 5f;
      this.transformMatrix = Matrix.CreateScale((float) num);
    }

    public void unload()
    {
      Game1.player.faceDirection(0);
      if (this.minecartLoop == null || !this.minecartLoop.IsPlaying)
        return;
      this.minecartLoop.Stop(AudioStopOptions.Immediate);
    }

    public void leftClickHeld(int x, int y)
    {
      if (!this.isJumping || this.reachedJumpApex)
        return;
      this.minecartDY = Math.Max(-4.5f, this.minecartDY - 0.6f);
      if ((double) this.minecartDY != -4.5)
        return;
      this.reachedJumpApex = true;
    }

    public void receiveEventPoke(int data)
    {
      throw new NotImplementedException();
    }

    public string minigameId()
    {
      return nameof (MineCart);
    }

    private class Spark
    {
      public float x;
      public float y;
      public Color c;
      public float dx;
      public float dy;

      public Spark(float x, float y, float dx, float dy)
      {
        this.x = x;
        this.y = y;
        this.dx = dx;
        this.dy = dy;
        this.c = Color.Yellow;
      }
    }
  }
}
