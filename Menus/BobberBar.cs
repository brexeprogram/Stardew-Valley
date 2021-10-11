// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.BobberBar
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Menus
{
  public class BobberBar : IClickableMenu
  {
    private float bobberPosition = 548f;
    private float distanceFromCatching = 0.3f;
    public const int timePerFishSizeReduction = 800;
    public const int bobberTrackHeight = 548;
    public const int bobberBarTrackHeight = 568;
    public const int xOffsetToBobberTrack = 64;
    public const int yOffsetToBobberTrack = 12;
    public const int mixed = 0;
    public const int dart = 1;
    public const int smooth = 2;
    public const int sink = 3;
    public const int floater = 4;
    private float difficulty;
    private int motionType;
    private int whichFish;
    private float bobberSpeed;
    private float bobberAcceleration;
    private float bobberTargetPosition;
    private float scale;
    private float everythingShakeTimer;
    private float floaterSinkerAcceleration;
    private float treasurePosition;
    private float treasureCatchLevel;
    private float treasureAppearTimer;
    private float treasureScale;
    private bool bobberInBar;
    private bool buttonPressed;
    private bool flipBubble;
    private bool fadeIn;
    private bool fadeOut;
    private bool treasure;
    private bool treasureCaught;
    private bool perfect;
    private bool bossFish;
    private int bobberBarHeight;
    private int fishSize;
    private int fishQuality;
    private int minFishSize;
    private int maxFishSize;
    private int fishSizeReductionTimer;
    private int whichBobber;
    private Vector2 barShake;
    private Vector2 fishShake;
    private Vector2 everythingShake;
    private Vector2 treasureShake;
    private float reelRotation;
    private SparklingText sparkleText;
    private float bobberBarPos;
    private float bobberBarSpeed;
    private float bobberBarAcceleration;
    public static Cue reelSound;
    public static Cue unReelSound;

    public BobberBar(int whichFish, float fishSize, bool treasure, int bobber)
      : base(0, 0, 96, 636, false)
    {
      this.treasure = treasure;
      this.treasureAppearTimer = (float) Game1.random.Next(1000, 3000);
      this.fadeIn = true;
      this.scale = 0.0f;
      this.whichFish = whichFish;
      Dictionary<int, string> dictionary = Game1.content.Load<Dictionary<int, string>>("Data\\Fish");
      this.bobberBarHeight = Game1.tileSize * 3 / 2 + Game1.player.FishingLevel * 8;
      this.bossFish = FishingRod.isFishBossFish(whichFish);
      if (Game1.player.fishCaught != null && Game1.player.fishCaught.Count == 0)
        this.distanceFromCatching = 0.1f;
      if (dictionary.ContainsKey(whichFish))
      {
        string[] strArray = dictionary[whichFish].Split('/');
        this.difficulty = (float) Convert.ToInt32(strArray[1]);
        string lower = strArray[2].ToLower();
        if (!(lower == nameof (mixed)))
        {
          if (!(lower == nameof (dart)))
          {
            if (!(lower == nameof (smooth)))
            {
              if (!(lower == nameof (floater)))
              {
                if (lower == "sinker")
                  this.motionType = 3;
              }
              else
                this.motionType = 4;
            }
            else
              this.motionType = 2;
          }
          else
            this.motionType = 1;
        }
        else
          this.motionType = 0;
        this.minFishSize = Convert.ToInt32(strArray[3]);
        this.maxFishSize = Convert.ToInt32(strArray[4]);
        this.fishSize = (int) ((double) this.minFishSize + (double) (this.maxFishSize - this.minFishSize) * (double) fishSize);
        this.fishSize = this.fishSize + 1;
        this.perfect = true;
        this.fishQuality = (double) fishSize < 0.33 ? 0 : ((double) fishSize < 0.66 ? 1 : 2);
        this.fishSizeReductionTimer = 800;
      }
      switch (Game1.player.FacingDirection)
      {
        case 0:
          this.xPositionOnScreen = (int) Game1.player.position.X - Game1.tileSize - 132;
          this.yPositionOnScreen = (int) Game1.player.position.Y - 274;
          break;
        case 1:
          this.xPositionOnScreen = (int) Game1.player.position.X - Game1.tileSize - 132;
          this.yPositionOnScreen = (int) Game1.player.position.Y - 274;
          break;
        case 2:
          this.xPositionOnScreen = (int) Game1.player.position.X - Game1.tileSize - 132;
          this.yPositionOnScreen = (int) Game1.player.position.Y - 274;
          break;
        case 3:
          this.xPositionOnScreen = (int) Game1.player.position.X + Game1.tileSize * 2;
          this.yPositionOnScreen = (int) Game1.player.position.Y - 274;
          this.flipBubble = true;
          break;
      }
      this.xPositionOnScreen = this.xPositionOnScreen - Game1.viewport.X;
      this.yPositionOnScreen = this.yPositionOnScreen - (Game1.viewport.Y + Game1.tileSize);
      if (this.xPositionOnScreen + 96 > Game1.viewport.Width)
        this.xPositionOnScreen = Game1.viewport.Width - 96;
      else if (this.xPositionOnScreen < 0)
        this.xPositionOnScreen = 0;
      if (this.yPositionOnScreen < 0)
        this.yPositionOnScreen = 0;
      else if (this.yPositionOnScreen + 636 > Game1.viewport.Height)
        this.yPositionOnScreen = Game1.viewport.Height - 636;
      if (bobber == 695)
        this.bobberBarHeight = this.bobberBarHeight + 24;
      this.bobberBarPos = (float) (568 - this.bobberBarHeight);
      this.bobberPosition = 508f;
      this.bobberTargetPosition = (float) ((100.0 - (double) this.difficulty) / 100.0 * 548.0);
      if (Game1.soundBank != null)
        BobberBar.reelSound = Game1.soundBank.GetCue("fastReel");
      this.whichBobber = bobber;
      Game1.setRichPresence("fishing", (object) Game1.currentLocation.Name);
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
    }

    public override void performHoverAction(int x, int y)
    {
    }

    public override void update(GameTime time)
    {
      if (this.sparkleText != null && this.sparkleText.update(time))
        this.sparkleText = (SparklingText) null;
      if ((double) this.everythingShakeTimer > 0.0)
      {
        this.everythingShakeTimer = this.everythingShakeTimer - (float) time.ElapsedGameTime.Milliseconds;
        this.everythingShake = new Vector2((float) Game1.random.Next(-10, 11) / 10f, (float) Game1.random.Next(-10, 11) / 10f);
        if ((double) this.everythingShakeTimer <= 0.0)
          this.everythingShake = Vector2.Zero;
      }
      if (this.fadeIn)
      {
        this.scale = this.scale + 0.05f;
        if ((double) this.scale >= 1.0)
        {
          this.scale = 1f;
          this.fadeIn = false;
        }
      }
      else if (this.fadeOut)
      {
        if ((double) this.everythingShakeTimer > 0.0 || this.sparkleText != null)
          return;
        this.scale = this.scale - 0.05f;
        if ((double) this.scale <= 0.0)
        {
          this.scale = 0.0f;
          this.fadeOut = false;
          if ((double) this.distanceFromCatching > 0.899999976158142 && Game1.player.CurrentTool is FishingRod)
          {
            (Game1.player.CurrentTool as FishingRod).pullFishFromWater(this.whichFish, this.fishSize, this.fishQuality, (int) this.difficulty, this.treasureCaught, this.perfect);
          }
          else
          {
            if (Game1.player.CurrentTool != null && Game1.player.CurrentTool is FishingRod)
              (Game1.player.CurrentTool as FishingRod).doneFishing(Game1.player, true);
            Game1.player.completelyStopAnimatingOrDoingAction();
          }
          Game1.exitActiveMenu();
          Game1.setRichPresence("location", (object) Game1.currentLocation.Name);
        }
      }
      else
      {
        if (Game1.random.NextDouble() < (double) this.difficulty * (this.motionType == 2 ? 20.0 : 1.0) / 4000.0 && (this.motionType != 2 || (double) this.bobberTargetPosition == -1.0))
        {
          float num1 = 548f - this.bobberPosition;
          float bobberPosition = this.bobberPosition;
          float num2 = Math.Min(99f, this.difficulty + (float) Game1.random.Next(10, 45)) / 100f;
          this.bobberTargetPosition = this.bobberPosition + (float) Game1.random.Next(-(int) bobberPosition, (int) num1) * num2;
        }
        if (this.motionType == 4)
          this.floaterSinkerAcceleration = Math.Max(this.floaterSinkerAcceleration - 0.01f, -1.5f);
        else if (this.motionType == 3)
          this.floaterSinkerAcceleration = Math.Min(this.floaterSinkerAcceleration + 0.01f, 1.5f);
        if ((double) Math.Abs(this.bobberPosition - this.bobberTargetPosition) > 3.0 && (double) this.bobberTargetPosition != -1.0)
        {
          this.bobberAcceleration = (float) (((double) this.bobberTargetPosition - (double) this.bobberPosition) / ((double) Game1.random.Next(10, 30) + (100.0 - (double) Math.Min(100f, this.difficulty))));
          this.bobberSpeed = this.bobberSpeed + (float) (((double) this.bobberAcceleration - (double) this.bobberSpeed) / 5.0);
        }
        else
          this.bobberTargetPosition = this.motionType == 2 || Game1.random.NextDouble() >= (double) this.difficulty / 2000.0 ? -1f : this.bobberPosition + (Game1.random.NextDouble() < 0.5 ? (float) Game1.random.Next(-100, -51) : (float) Game1.random.Next(50, 101));
        if (this.motionType == 1 && Game1.random.NextDouble() < (double) this.difficulty / 1000.0)
          this.bobberTargetPosition = this.bobberPosition + (Game1.random.NextDouble() < 0.5 ? (float) Game1.random.Next(-100 - (int) this.difficulty * 2, -51) : (float) Game1.random.Next(50, 101 + (int) this.difficulty * 2));
        this.bobberTargetPosition = Math.Max(-1f, Math.Min(this.bobberTargetPosition, 548f));
        this.bobberPosition = this.bobberPosition + (this.bobberSpeed + this.floaterSinkerAcceleration);
        if ((double) this.bobberPosition > 532.0)
          this.bobberPosition = 532f;
        else if ((double) this.bobberPosition < 0.0)
          this.bobberPosition = 0.0f;
        this.bobberInBar = (double) this.bobberPosition + 16.0 <= (double) this.bobberBarPos - 32.0 + (double) this.bobberBarHeight && (double) this.bobberPosition - 16.0 >= (double) this.bobberBarPos - 32.0;
        if ((double) this.bobberPosition >= (double) (548 - this.bobberBarHeight) && (double) this.bobberBarPos >= (double) (568 - this.bobberBarHeight - 4))
          this.bobberInBar = true;
        int num3 = this.buttonPressed ? 1 : 0;
        this.buttonPressed = Game1.oldMouseState.LeftButton == ButtonState.Pressed || Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.useToolButton) || Game1.options.gamepadControls && (Game1.oldPadState.IsButtonDown(Buttons.X) || Game1.oldPadState.IsButtonDown(Buttons.A));
        if (num3 == 0 && this.buttonPressed)
          Game1.playSound("fishingRodBend");
        float num4 = this.buttonPressed ? -0.25f : 0.25f;
        if (this.buttonPressed && (double) num4 < 0.0 && ((double) this.bobberBarPos == 0.0 || (double) this.bobberBarPos == (double) (568 - this.bobberBarHeight)))
          this.bobberBarSpeed = 0.0f;
        if (this.bobberInBar)
        {
          num4 *= this.whichBobber == 691 ? 0.3f : 0.6f;
          if (this.whichBobber == 691)
            this.bobberBarSpeed = (double) this.bobberPosition + 16.0 >= (double) this.bobberBarPos + (double) (this.bobberBarHeight / 2) ? this.bobberBarSpeed + 0.2f : this.bobberBarSpeed - 0.2f;
        }
        float bobberBarPos = this.bobberBarPos;
        this.bobberBarSpeed = this.bobberBarSpeed + num4;
        this.bobberBarPos = this.bobberBarPos + this.bobberBarSpeed;
        if ((double) this.bobberBarPos + (double) this.bobberBarHeight > 568.0)
        {
          this.bobberBarPos = (float) (568 - this.bobberBarHeight);
          this.bobberBarSpeed = (float) (-(double) this.bobberBarSpeed * 2.0 / 3.0 * (this.whichBobber == 692 ? 0.100000001490116 : 1.0));
          if ((double) bobberBarPos + (double) this.bobberBarHeight < 568.0)
            Game1.playSound("shiny4");
        }
        else if ((double) this.bobberBarPos < 0.0)
        {
          this.bobberBarPos = 0.0f;
          this.bobberBarSpeed = (float) (-(double) this.bobberBarSpeed * 2.0 / 3.0);
          if ((double) bobberBarPos > 0.0)
            Game1.playSound("shiny4");
        }
        bool flag = false;
        if (this.treasure)
        {
          float treasureAppearTimer = this.treasureAppearTimer;
          this.treasureAppearTimer = this.treasureAppearTimer - (float) time.ElapsedGameTime.Milliseconds;
          if ((double) this.treasureAppearTimer <= 0.0)
          {
            if ((double) this.treasureScale < 1.0 && !this.treasureCaught)
            {
              if ((double) treasureAppearTimer > 0.0)
              {
                this.treasurePosition = (double) this.bobberBarPos > 274.0 ? (float) Game1.random.Next(8, (int) this.bobberBarPos - 20) : (float) Game1.random.Next(Math.Min(528, (int) this.bobberBarPos + this.bobberBarHeight), 500);
                Game1.playSound("dwop");
              }
              this.treasureScale = Math.Min(1f, this.treasureScale + 0.1f);
            }
            flag = (double) this.treasurePosition + 16.0 <= (double) this.bobberBarPos - 32.0 + (double) this.bobberBarHeight && (double) this.treasurePosition - 16.0 >= (double) this.bobberBarPos - 32.0;
            if (flag && !this.treasureCaught)
            {
              this.treasureCatchLevel = this.treasureCatchLevel + 0.0135f;
              this.treasureShake = new Vector2((float) Game1.random.Next(-2, 3), (float) Game1.random.Next(-2, 3));
              if ((double) this.treasureCatchLevel >= 1.0)
              {
                Game1.playSound("newArtifact");
                this.treasureCaught = true;
              }
            }
            else if (this.treasureCaught)
            {
              this.treasureScale = Math.Max(0.0f, this.treasureScale - 0.1f);
            }
            else
            {
              this.treasureShake = Vector2.Zero;
              this.treasureCatchLevel = Math.Max(0.0f, this.treasureCatchLevel - 0.01f);
            }
          }
        }
        if (this.bobberInBar)
        {
          this.distanceFromCatching = this.distanceFromCatching + 1f / 500f;
          this.reelRotation = this.reelRotation + 0.3926991f;
          this.fishShake.X = (float) Game1.random.Next(-10, 11) / 10f;
          this.fishShake.Y = (float) Game1.random.Next(-10, 11) / 10f;
          this.barShake = Vector2.Zero;
          Rumble.rumble(0.1f, 1000f);
          if (BobberBar.unReelSound != null)
            BobberBar.unReelSound.Stop(AudioStopOptions.Immediate);
          if (Game1.soundBank != null && (BobberBar.reelSound == null || BobberBar.reelSound.IsStopped || BobberBar.reelSound.IsStopping))
            BobberBar.reelSound = Game1.soundBank.GetCue("fastReel");
          if (BobberBar.reelSound != null && !BobberBar.reelSound.IsPlaying && !BobberBar.reelSound.IsStopping)
            BobberBar.reelSound.Play();
        }
        else if (!flag || this.treasureCaught || this.whichBobber != 693)
        {
          if (!this.fishShake.Equals(Vector2.Zero))
          {
            Game1.playSound("tinyWhip");
            this.perfect = false;
            Rumble.stopRumbling();
          }
          this.fishSizeReductionTimer = this.fishSizeReductionTimer - time.ElapsedGameTime.Milliseconds;
          if (this.fishSizeReductionTimer <= 0)
          {
            this.fishSize = Math.Max(this.minFishSize, this.fishSize - 1);
            this.fishSizeReductionTimer = 800;
          }
          if (Game1.player.fishCaught != null && Game1.player.fishCaught.Count != 0 || Game1.currentMinigame != null)
            this.distanceFromCatching = this.distanceFromCatching - (this.whichBobber == 694 ? 1f / 500f : 3f / 1000f);
          this.reelRotation = this.reelRotation - 3.141593f / Math.Max(10f, 200f - Math.Abs(this.bobberPosition - (this.bobberBarPos + (float) (this.bobberBarHeight / 2))));
          this.barShake.X = (float) Game1.random.Next(-10, 11) / 10f;
          this.barShake.Y = (float) Game1.random.Next(-10, 11) / 10f;
          this.fishShake = Vector2.Zero;
          if (BobberBar.reelSound != null)
            BobberBar.reelSound.Stop(AudioStopOptions.Immediate);
          if (Game1.soundBank != null && (BobberBar.unReelSound == null || BobberBar.unReelSound.IsStopped))
          {
            BobberBar.unReelSound = Game1.soundBank.GetCue("slowReel");
            BobberBar.unReelSound.SetVariable("Pitch", 600f);
          }
          if (BobberBar.unReelSound != null && !BobberBar.unReelSound.IsPlaying && !BobberBar.unReelSound.IsStopping)
            BobberBar.unReelSound.Play();
        }
        this.distanceFromCatching = Math.Max(0.0f, Math.Min(1f, this.distanceFromCatching));
        if (Game1.player.CurrentTool != null)
          Game1.player.CurrentTool.tickUpdate(time, Game1.player);
        if ((double) this.distanceFromCatching <= 0.0)
        {
          this.fadeOut = true;
          this.everythingShakeTimer = 500f;
          Game1.playSound("fishEscape");
          if (BobberBar.unReelSound != null)
            BobberBar.unReelSound.Stop(AudioStopOptions.Immediate);
          if (BobberBar.reelSound != null)
            BobberBar.reelSound.Stop(AudioStopOptions.Immediate);
        }
        else if ((double) this.distanceFromCatching >= 1.0)
        {
          this.everythingShakeTimer = 500f;
          Game1.playSound("jingle1");
          this.fadeOut = true;
          if (BobberBar.unReelSound != null)
            BobberBar.unReelSound.Stop(AudioStopOptions.Immediate);
          if (BobberBar.reelSound != null)
            BobberBar.reelSound.Stop(AudioStopOptions.Immediate);
          if (this.perfect)
          {
            this.sparkleText = new SparklingText(Game1.dialogueFont, Game1.content.LoadString("Strings\\UI:BobberBar_Perfect"), Color.Yellow, Color.White, false, 0.1, 1500, -1, 500);
            if (Game1.isFestival())
              Game1.CurrentEvent.perfectFishing();
          }
          else if (this.fishSize == this.maxFishSize)
            this.fishSize = this.fishSize - 1;
        }
      }
      if ((double) this.bobberPosition < 0.0)
        this.bobberPosition = 0.0f;
      if ((double) this.bobberPosition <= 548.0)
        return;
      this.bobberPosition = 548f;
    }

    public override bool readyToClose()
    {
      return false;
    }

    public override void emergencyShutDown()
    {
      base.emergencyShutDown();
      if (BobberBar.unReelSound != null)
        BobberBar.unReelSound.Stop(AudioStopOptions.Immediate);
      if (BobberBar.reelSound != null)
        BobberBar.reelSound.Stop(AudioStopOptions.Immediate);
      this.fadeOut = true;
      this.everythingShakeTimer = 500f;
      this.distanceFromCatching = -1f;
      Game1.playSound("fishEscape");
    }

    public override void receiveKeyPress(Keys key)
    {
      if (!((IEnumerable<InputButton>) Game1.options.menuButton).Contains<InputButton>(new InputButton(key)))
        return;
      this.emergencyShutDown();
    }

    public override void draw(SpriteBatch b)
    {
      b.Draw(Game1.mouseCursors, new Vector2((float) (this.xPositionOnScreen - (this.flipBubble ? 44 : 20) + 104), (float) (this.yPositionOnScreen - 16 + 314)) + this.everythingShake, new Rectangle?(new Rectangle(652, 1685, 52, 157)), Color.White * 0.6f * this.scale, 0.0f, new Vector2(26f, 78.5f) * this.scale, (float) Game1.pixelZoom * this.scale, this.flipBubble ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 1f / 1000f);
      b.Draw(Game1.mouseCursors, new Vector2((float) (this.xPositionOnScreen + 70), (float) (this.yPositionOnScreen + 296)) + this.everythingShake, new Rectangle?(new Rectangle(644, 1999, 37, 150)), Color.White * this.scale, 0.0f, new Vector2(18.5f, 74f) * this.scale, (float) Game1.pixelZoom * this.scale, SpriteEffects.None, 0.01f);
      if ((double) this.scale == 1.0)
      {
        SpriteBatch spriteBatch1 = b;
        Texture2D mouseCursors1 = Game1.mouseCursors;
        Vector2 position1 = new Vector2((float) (this.xPositionOnScreen + 64), (float) (this.yPositionOnScreen + 12 + (int) this.bobberBarPos)) + this.barShake + this.everythingShake;
        Rectangle? sourceRectangle1 = new Rectangle?(new Rectangle(682, 2078, 9, 2));
        TimeSpan timeOfDay;
        Color color1;
        if (!this.bobberInBar)
        {
          Color color2 = Color.White * 0.25f;
          timeOfDay = DateTime.Now.TimeOfDay;
          double num = Math.Round(Math.Sin(timeOfDay.TotalMilliseconds / 100.0), 2) + 2.0;
          color1 = color2 * (float) num;
        }
        else
          color1 = Color.White;
        double num1 = 0.0;
        Vector2 zero1 = Vector2.Zero;
        double num2 = 4.0;
        int num3 = 0;
        double num4 = 0.889999985694885;
        spriteBatch1.Draw(mouseCursors1, position1, sourceRectangle1, color1, (float) num1, zero1, (float) num2, (SpriteEffects) num3, (float) num4);
        SpriteBatch spriteBatch2 = b;
        Texture2D mouseCursors2 = Game1.mouseCursors;
        Vector2 position2 = new Vector2((float) (this.xPositionOnScreen + 64), (float) (this.yPositionOnScreen + 12 + (int) this.bobberBarPos + 8)) + this.barShake + this.everythingShake;
        Rectangle? sourceRectangle2 = new Rectangle?(new Rectangle(682, 2081, 9, 1));
        Color color3;
        if (!this.bobberInBar)
        {
          Color color2 = Color.White * 0.25f;
          timeOfDay = DateTime.Now.TimeOfDay;
          double num5 = Math.Round(Math.Sin(timeOfDay.TotalMilliseconds / 100.0), 2) + 2.0;
          color3 = color2 * (float) num5;
        }
        else
          color3 = Color.White;
        double num6 = 0.0;
        Vector2 zero2 = Vector2.Zero;
        Vector2 scale = new Vector2(4f, (float) (this.bobberBarHeight - 16));
        int num7 = 0;
        double num8 = 0.889999985694885;
        spriteBatch2.Draw(mouseCursors2, position2, sourceRectangle2, color3, (float) num6, zero2, scale, (SpriteEffects) num7, (float) num8);
        SpriteBatch spriteBatch3 = b;
        Texture2D mouseCursors3 = Game1.mouseCursors;
        Vector2 position3 = new Vector2((float) (this.xPositionOnScreen + 64), (float) (this.yPositionOnScreen + 12 + (int) this.bobberBarPos + this.bobberBarHeight - 8)) + this.barShake + this.everythingShake;
        Rectangle? sourceRectangle3 = new Rectangle?(new Rectangle(682, 2085, 9, 2));
        Color color4;
        if (!this.bobberInBar)
        {
          Color color2 = Color.White * 0.25f;
          timeOfDay = DateTime.Now.TimeOfDay;
          double num5 = Math.Round(Math.Sin(timeOfDay.TotalMilliseconds / 100.0), 2) + 2.0;
          color4 = color2 * (float) num5;
        }
        else
          color4 = Color.White;
        double num9 = 0.0;
        Vector2 zero3 = Vector2.Zero;
        double num10 = 4.0;
        int num11 = 0;
        double num12 = 0.889999985694885;
        spriteBatch3.Draw(mouseCursors3, position3, sourceRectangle3, color4, (float) num9, zero3, (float) num10, (SpriteEffects) num11, (float) num12);
        b.Draw(Game1.staminaRect, new Rectangle(this.xPositionOnScreen + 124, this.yPositionOnScreen + 4 + (int) (580.0 * (1.0 - (double) this.distanceFromCatching)), 16, (int) (580.0 * (double) this.distanceFromCatching)), Utility.getRedToGreenLerpColor(this.distanceFromCatching));
        b.Draw(Game1.mouseCursors, new Vector2((float) (this.xPositionOnScreen + 18), (float) (this.yPositionOnScreen + 514)) + this.everythingShake, new Rectangle?(new Rectangle(257, 1990, 5, 10)), Color.White, this.reelRotation, new Vector2(2f, 10f), 4f, SpriteEffects.None, 0.9f);
        b.Draw(Game1.mouseCursors, new Vector2((float) (this.xPositionOnScreen + 64 + 18), (float) (this.yPositionOnScreen + 12 + 24) + this.treasurePosition) + this.treasureShake + this.everythingShake, new Rectangle?(new Rectangle(638, 1865, 20, 24)), Color.White, 0.0f, new Vector2(10f, 10f), 2f * this.treasureScale, SpriteEffects.None, 0.85f);
        if ((double) this.treasureCatchLevel > 0.0 && !this.treasureCaught)
        {
          b.Draw(Game1.staminaRect, new Rectangle(this.xPositionOnScreen + 64, this.yPositionOnScreen + 12 + (int) this.treasurePosition, 40, 8), Color.DimGray * 0.5f);
          b.Draw(Game1.staminaRect, new Rectangle(this.xPositionOnScreen + 64, this.yPositionOnScreen + 12 + (int) this.treasurePosition, (int) ((double) this.treasureCatchLevel * 40.0), 8), Color.Orange);
        }
        b.Draw(Game1.mouseCursors, new Vector2((float) (this.xPositionOnScreen + 64 + 18), (float) (this.yPositionOnScreen + 12 + 24) + this.bobberPosition) + this.fishShake + this.everythingShake, new Rectangle?(new Rectangle(614 + (this.bossFish ? 20 : 0), 1840, 20, 20)), Color.White, 0.0f, new Vector2(10f, 10f), 2f, SpriteEffects.None, 0.88f);
        if (this.sparkleText != null)
          this.sparkleText.draw(b, new Vector2((float) (this.xPositionOnScreen - Game1.tileSize / 4), (float) (this.yPositionOnScreen - Game1.tileSize)));
      }
      if (Game1.player.fishCaught == null || Game1.player.fishCaught.Count != 0)
        return;
      Vector2 position = new Vector2((float) (this.xPositionOnScreen + (this.flipBubble ? this.width + Game1.tileSize + Game1.pixelZoom * 2 : -Game1.pixelZoom * 2 - 48 * Game1.pixelZoom)), (float) (this.yPositionOnScreen + Game1.tileSize * 3));
      if (!Game1.options.gamepadControls)
        b.Draw(Game1.mouseCursors, position, new Rectangle?(new Rectangle(644, 1330, 48, 69)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.88f);
      else
        b.Draw(Game1.controllerMaps, position, new Rectangle?(Utility.controllerMapSourceRect(new Rectangle(681, 0, 96, 138))), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom / 2f, SpriteEffects.None, 0.88f);
    }
  }
}
