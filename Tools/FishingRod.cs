// Decompiled with JetBrains decompiler
// Type: StardewValley.Tools.FishingRod
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Menus;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace StardewValley.Tools
{
  public class FishingRod : Tool
  {
    public static int minFishingBiteTime = 600;
    public static int maxFishingBiteTime = 30000;
    public static int minTimeToNibble = 340;
    public static int maxTimeToNibble = 800;
    public static double baseChanceForTreasure = 0.15;
    [XmlIgnore]
    public float timePerBobberBob = 2000f;
    [XmlIgnore]
    public float timeUntilFishingBite = -1f;
    [XmlIgnore]
    public float timeUntilFishingNibbleDone = -1f;
    [XmlIgnore]
    public float castingTimerSpeed = 1f / 1000f;
    [XmlIgnore]
    public List<TemporaryAnimatedSprite> animations = new List<TemporaryAnimatedSprite>();
    public const int sizeOfLandCheckRectangle = 11;
    private Vector2 bobber;
    private int bobberBob;
    [XmlIgnore]
    public float bobberTimeAccumulator;
    [XmlIgnore]
    public float fishingBiteAccumulator;
    [XmlIgnore]
    public float fishingNibbleAccumulator;
    [XmlIgnore]
    public float castingPower;
    [XmlIgnore]
    public float castingChosenCountdown;
    [XmlIgnore]
    public float fishWiggle;
    [XmlIgnore]
    public float fishWiggleIntensity;
    [XmlIgnore]
    public bool isFishing;
    [XmlIgnore]
    public bool hit;
    [XmlIgnore]
    public bool isNibbling;
    [XmlIgnore]
    public bool favBait;
    [XmlIgnore]
    public bool isTimingCast;
    [XmlIgnore]
    public bool isCasting;
    [XmlIgnore]
    public bool castedButBobberStillInAir;
    [XmlIgnore]
    public bool doneWithAnimation;
    [XmlIgnore]
    public bool hasDoneFucntionYet;
    [XmlIgnore]
    public bool pullingOutOfWater;
    [XmlIgnore]
    public bool isReeling;
    [XmlIgnore]
    public bool fishCaught;
    [XmlIgnore]
    public bool recordSize;
    [XmlIgnore]
    public bool treasureCaught;
    [XmlIgnore]
    public bool showingTreasure;
    [XmlIgnore]
    public bool hadBobber;
    [XmlIgnore]
    public bool bossFish;
    [XmlIgnore]
    public SparklingText sparklingText;
    [XmlIgnore]
    private int fishSize;
    [XmlIgnore]
    private int whichFish;
    [XmlIgnore]
    private int fishQuality;
    [XmlIgnore]
    private int clearWaterDistance;
    [XmlIgnore]
    private int originalFacingDirection;
    public static Cue chargeSound;
    public static Cue reelSound;
    private bool usedGamePadToCast;

    public FishingRod()
      : base("Fishing Rod", 0, 189, 8, false, 0)
    {
      this.numAttachmentSlots = 2;
      this.attachments = new StardewValley.Object[this.numAttachmentSlots];
      this.indexOfMenuItemView = 8 + this.upgradeLevel;
    }

    protected override string loadDisplayName()
    {
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingRod.cs.14041");
    }

    protected override string loadDescription()
    {
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingRod.cs.14042");
    }

    public override int salePrice()
    {
      switch (this.upgradeLevel)
      {
        case 0:
          return 500;
        case 1:
          return 2000;
        case 2:
          return 5000;
        case 3:
          return 15000;
        default:
          return 500;
      }
    }

    public override int attachmentSlots()
    {
      if (this.upgradeLevel > 2)
        return 2;
      return this.upgradeLevel <= 0 ? 0 : 1;
    }

    public FishingRod(int upgradeLevel)
      : base("Fishing Rod", upgradeLevel, 189, 8, false, 0)
    {
      this.numAttachmentSlots = 2;
      this.attachments = new StardewValley.Object[this.numAttachmentSlots];
      this.indexOfMenuItemView = 8 + upgradeLevel;
      this.upgradeLevel = upgradeLevel;
    }

    public override string DisplayName
    {
      get
      {
        switch (this.upgradeLevel)
        {
          case 0:
            return Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingRod.cs.14045");
          case 1:
            return Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingRod.cs.14046");
          case 2:
            return Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingRod.cs.14047");
          case 3:
            return Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingRod.cs.14048");
          default:
            return this.displayName;
        }
      }
    }

    public override string Name
    {
      get
      {
        switch (this.upgradeLevel)
        {
          case 0:
            return "Bamboo Pole";
          case 1:
            return "Yew Rod";
          case 2:
            return "Fiberglass Rod";
          case 3:
            return "Iridium Rod";
          default:
            return this.name;
        }
      }
    }

    private int getAddedDistance(Farmer who)
    {
      if (who.FishingLevel >= 8)
        return 3;
      if (who.FishingLevel >= 4)
        return 2;
      return who.FishingLevel >= 1 ? 1 : 0;
    }

    public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
    {
      if (this.fishCaught)
        return;
      this.hasDoneFucntionYet = true;
      int num1 = (int) ((double) this.bobber.X / (double) Game1.tileSize);
      int num2 = (int) (((double) this.bobber.Y - (double) (Game1.tileSize / 2)) / (double) Game1.tileSize);
      base.DoFunction(location, x, y, power, who);
      if (this.doneWithAnimation && who.IsMainPlayer)
        who.canReleaseTool = true;
      if (Game1.isAnyGamePadButtonBeingPressed())
        Game1.lastCursorMotionWasMouse = false;
      if (!this.isFishing && !this.castedButBobberStillInAir && (!this.pullingOutOfWater && !this.isNibbling) && !this.hit)
      {
        if (!Game1.eventUp)
        {
          float stamina = who.Stamina;
          who.Stamina = who.Stamina - (float) (8.0 - (double) who.FishingLevel * 0.100000001490116);
          who.checkForExhaustion(stamina);
        }
        if (location.doesTileHaveProperty(num1, num2, "Water", "Back") != null && location.doesTileHaveProperty(num1, num2, "NoFishing", "Back") == null && location.getTileIndexAt(num1, num2, "Buildings") == -1 || location.doesTileHaveProperty(num1, num2, "Water", "Buildings") != null)
        {
          this.isFishing = true;
          location.TemporarySprites.Add(new TemporaryAnimatedSprite(28, 100f, 2, 1, new Vector2((float) ((double) this.bobber.X - (double) Game1.tileSize - 16.0), (float) ((double) this.bobber.Y - (double) Game1.tileSize - 16.0)), false, false));
          Game1.playSound("dropItemInWater");
          this.timeUntilFishingBite = (float) Game1.random.Next(FishingRod.minFishingBiteTime, FishingRod.maxFishingBiteTime - 250 * who.FishingLevel - (this.attachments[1] == null || this.attachments[1].ParentSheetIndex != 686 ? (this.attachments[1] == null || this.attachments[1].ParentSheetIndex != 687 ? 0 : 10000) : 5000));
          this.timeUntilFishingBite = this.timeUntilFishingBite * 0.75f;
          if (this.attachments[0] != null)
          {
            this.timeUntilFishingBite = this.timeUntilFishingBite * 0.5f;
            if (this.attachments[0].parentSheetIndex == 774)
              this.timeUntilFishingBite = this.timeUntilFishingBite * 0.75f;
          }
          this.timeUntilFishingBite = Math.Max(500f, this.timeUntilFishingBite);
          ++Game1.stats.TimesFished;
          double num3 = ((double) this.bobber.X - (double) (Game1.tileSize / 2)) / (double) Game1.tileSize;
          double num4 = ((double) this.bobber.Y - (double) (Game1.tileSize / 2)) / (double) Game1.tileSize;
          Point fishSplashPoint = location.fishSplashPoint;
          Rectangle rectangle = new Rectangle(location.fishSplashPoint.X * Game1.tileSize, location.fishSplashPoint.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
          if (new Rectangle((int) this.bobber.X - Game1.tileSize * 5 / 4, (int) this.bobber.Y - Game1.tileSize * 5 / 4, Game1.tileSize, Game1.tileSize).Intersects(rectangle))
          {
            this.timeUntilFishingBite = this.timeUntilFishingBite / 4f;
            location.temporarySprites.Add(new TemporaryAnimatedSprite(10, this.bobber - new Vector2((float) Game1.tileSize, (float) (Game1.tileSize * 2)), Color.Cyan, 8, false, 100f, 0, -1, -1f, -1, 0));
          }
          if (!who.IsMainPlayer)
          {
            who.Halt();
            who.FarmerSprite.PauseForSingleAnimation = false;
          }
          who.UsingTool = true;
          if (!who.IsMainPlayer)
            return;
          who.canMove = false;
        }
        else
        {
          if (this.doneWithAnimation && who.IsMainPlayer)
            who.usingTool = false;
          if (!this.doneWithAnimation || !who.IsMainPlayer)
            return;
          who.canMove = true;
        }
      }
      else
      {
        if (this.isCasting || this.pullingOutOfWater)
          return;
        who.FarmerSprite.pauseForSingleAnimation = false;
        switch (who.FacingDirection)
        {
          case 0:
            who.FarmerSprite.animateBackwardsOnce(299, 35f);
            break;
          case 1:
            who.FarmerSprite.animateBackwardsOnce(300, 35f);
            break;
          case 2:
            who.FarmerSprite.animateBackwardsOnce(301, 35f);
            break;
          case 3:
            who.FarmerSprite.animateBackwardsOnce(302, 35f);
            break;
        }
        if (this.isNibbling)
        {
          double num3 = this.attachments[0] != null ? (double) this.attachments[0].Price / 10.0 : 0.0;
          Point fishSplashPoint = location.fishSplashPoint;
          bool flag = new Rectangle(location.fishSplashPoint.X * Game1.tileSize, location.fishSplashPoint.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize).Intersects(new Rectangle((int) this.bobber.X - Game1.tileSize * 5 / 4, (int) this.bobber.Y - Game1.tileSize * 5 / 4, Game1.tileSize, Game1.tileSize));
          StardewValley.Object @object = location.getFish(this.fishingNibbleAccumulator, this.attachments[0] != null ? this.attachments[0].ParentSheetIndex : -1, this.clearWaterDistance + (flag ? 1 : 0), this.lastUser, num3 + (flag ? 0.4 : 0.0));
          if (@object == null || @object.ParentSheetIndex <= 0)
            @object = new StardewValley.Object(Game1.random.Next(167, 173), 1, false, -1, 0);
          if ((double) @object.scale.X == 1.0)
            this.favBait = true;
          if (@object.Category == -20 || @object.ParentSheetIndex == 152 || (@object.ParentSheetIndex == 153 || @object.parentSheetIndex == 157))
          {
            this.pullFishFromWater(@object.ParentSheetIndex, -1, 0, 0, false, false);
          }
          else
          {
            if (this.hit)
              return;
            this.hit = true;
            List<TemporaryAnimatedSprite> overlayTempSprites = Game1.screenOverlayTempSprites;
            TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(612, 1913, 74, 30), 1500f, 1, 0, Game1.GlobalToLocal(Game1.viewport, this.bobber + new Vector2(-140f, (float) (-Game1.tileSize * 5 / 2))), false, false, 1f, 0.005f, Color.White, 4f, 0.075f, 0.0f, 0.0f, true);
            temporaryAnimatedSprite.scaleChangeChange = -0.005f;
            Vector2 vector2 = new Vector2(0.0f, -0.1f);
            temporaryAnimatedSprite.motion = vector2;
            TemporaryAnimatedSprite.endBehavior endBehavior = new TemporaryAnimatedSprite.endBehavior(this.startMinigameEndFunction);
            temporaryAnimatedSprite.endFunction = endBehavior;
            int parentSheetIndex = @object.ParentSheetIndex;
            temporaryAnimatedSprite.extraInfoForEndBehavior = parentSheetIndex;
            overlayTempSprites.Add(temporaryAnimatedSprite);
            Game1.playSound("FishHit");
          }
        }
        else
        {
          Game1.playSound("pullItemFromWater");
          this.isFishing = false;
          this.pullingOutOfWater = true;
          if (this.lastUser.FacingDirection == 1 || this.lastUser.FacingDirection == 3)
          {
            double num3 = (double) Math.Abs(this.bobber.X - (float) this.lastUser.getStandingX());
            float y1 = 0.005f;
            double num4 = (double) y1;
            float num5 = -(float) Math.Sqrt(num3 * num4 / 2.0);
            float animationInterval = (float) (2.0 * ((double) Math.Abs(num5 - 0.5f) / (double) y1)) * 1.2f;
            List<TemporaryAnimatedSprite> animations = this.animations;
            TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(170, 1903, 7, 8), animationInterval, 1, 0, this.bobber + new Vector2((float) (-Game1.tileSize / 2), (float) (-Game1.tileSize * 3 / 4)), false, false, (float) who.getStandingY() / 10000f, 0.0f, Color.White, 4f, 0.0f, 0.0f, (float) Game1.random.Next(-20, 20) / 100f, false);
            temporaryAnimatedSprite.motion = new Vector2((who.FacingDirection == 3 ? -1f : 1f) * (num5 + 0.2f), num5 - 0.8f);
            temporaryAnimatedSprite.acceleration = new Vector2(0.0f, y1);
            temporaryAnimatedSprite.endFunction = new TemporaryAnimatedSprite.endBehavior(this.donefishingEndFunction);
            int num6 = 1;
            temporaryAnimatedSprite.timeBasedMotion = num6 != 0;
            double num7 = 1.0 / 1000.0;
            temporaryAnimatedSprite.alphaFade = (float) num7;
            animations.Add(temporaryAnimatedSprite);
          }
          else
          {
            float num3 = this.bobber.Y - (float) this.lastUser.getStandingY();
            float num4 = Math.Abs(num3 + (float) (Game1.tileSize * 4));
            float y1 = 0.005f;
            float num5 = (float) Math.Sqrt(2.0 * (double) y1 * (double) num4);
            float animationInterval = (float) (Math.Sqrt(2.0 * ((double) num4 - (double) num3) / (double) y1) + (double) num5 / (double) y1);
            List<TemporaryAnimatedSprite> animations = this.animations;
            TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(170, 1903, 7, 8), animationInterval, 1, 0, this.bobber + new Vector2((float) (-Game1.tileSize / 2), (float) (-Game1.tileSize * 3 / 4)), false, false, this.bobber.Y / 10000f, 0.0f, Color.White, 4f, 0.0f, 0.0f, (float) Game1.random.Next(-20, 20) / 100f, false);
            temporaryAnimatedSprite.motion = new Vector2(0.0f, -num5);
            temporaryAnimatedSprite.acceleration = new Vector2(0.0f, y1);
            temporaryAnimatedSprite.endFunction = new TemporaryAnimatedSprite.endBehavior(this.donefishingEndFunction);
            int num6 = 1;
            temporaryAnimatedSprite.timeBasedMotion = num6 != 0;
            double num7 = 1.0 / 1000.0;
            temporaryAnimatedSprite.alphaFade = (float) num7;
            animations.Add(temporaryAnimatedSprite);
          }
          who.UsingTool = true;
          who.canReleaseTool = false;
        }
      }
    }

    public Color getColor()
    {
      switch (this.upgradeLevel)
      {
        case 0:
          return Color.Goldenrod;
        case 1:
          return Color.RosyBrown;
        case 2:
          return Color.White;
        case 3:
          return Color.Violet;
        default:
          return Color.White;
      }
    }

    public static int distanceToLand(int tileX, int tileY, GameLocation location)
    {
      Rectangle r = new Rectangle(tileX - 1, tileY - 1, 3, 3);
      bool flag = false;
      int num = 1;
      while (!flag && r.Width <= 11)
      {
        foreach (Vector2 position in Utility.getBorderOfThisRectangle(r))
        {
          if (location.isTileOnMap(position) && location.doesTileHaveProperty((int) position.X, (int) position.Y, "Water", "Back") == null)
          {
            flag = true;
            num = r.Width / 2;
            break;
          }
        }
        r.Inflate(1, 1);
      }
      if (r.Width > 11)
        num = 6;
      return num - 1;
    }

    public void startMinigameEndFunction(int extra)
    {
      this.isReeling = true;
      this.hit = false;
      switch (this.lastUser.FacingDirection)
      {
        case 1:
          this.lastUser.FarmerSprite.setCurrentSingleFrame(48, (short) 32000, false, false);
          break;
        case 3:
          this.lastUser.FarmerSprite.setCurrentSingleFrame(48, (short) 32000, false, true);
          break;
      }
      this.lastUser.FarmerSprite.pauseForSingleAnimation = true;
      this.clearWaterDistance = FishingRod.distanceToLand((int) ((double) this.bobber.X / (double) Game1.tileSize - 1.0), (int) ((double) this.bobber.Y / (double) Game1.tileSize - 1.0), this.lastUser.currentLocation);
      float num = 1f * ((float) this.clearWaterDistance / 5f) * ((float) Game1.random.Next(1 + Math.Min(10, this.lastUser.FishingLevel) / 2, 6) / 5f);
      if (this.favBait)
        num *= 1.2f;
      float fishSize = Math.Max(0.0f, Math.Min(1f, num * (float) (1.0 + (double) Game1.random.Next(-10, 10) / 100.0)));
      bool treasure = !Game1.isFestival() && this.lastUser.fishCaught != null && this.lastUser.fishCaught.Count > 1 && Game1.random.NextDouble() < FishingRod.baseChanceForTreasure + (double) this.lastUser.LuckLevel * 0.005 + (this.getBaitAttachmentIndex() == 703 ? FishingRod.baseChanceForTreasure : 0.0) + (this.getBobberAttachmentIndex() == 693 ? FishingRod.baseChanceForTreasure / 3.0 : 0.0) + Game1.dailyLuck / 2.0 + (this.lastUser.professions.Contains(9) ? FishingRod.baseChanceForTreasure : 0.0);
      Game1.activeClickableMenu = (IClickableMenu) new BobberBar(extra, fishSize, treasure, this.attachments[1] != null ? this.attachments[1].ParentSheetIndex : -1);
    }

    public int getBobberAttachmentIndex()
    {
      if (this.attachments[1] == null)
        return -1;
      return this.attachments[1].ParentSheetIndex;
    }

    public int getBaitAttachmentIndex()
    {
      if (this.attachments[0] == null)
        return -1;
      return this.attachments[0].ParentSheetIndex;
    }

    public bool inUse()
    {
      if (!this.isFishing && !this.isCasting && (!this.isTimingCast && !this.isNibbling) && !this.isReeling)
        return this.fishCaught;
      return true;
    }

    public void donefishingEndFunction(int extra)
    {
      this.isFishing = false;
      this.isReeling = false;
      this.lastUser.canReleaseTool = true;
      this.lastUser.canMove = true;
      this.lastUser.usingTool = false;
      this.lastUser.FarmerSprite.pauseForSingleAnimation = false;
      this.pullingOutOfWater = false;
      this.doneFishing(this.lastUser, false);
    }

    public static void endOfAnimationBehavior(Farmer f)
    {
    }

    public override StardewValley.Object attach(StardewValley.Object o)
    {
      if (o != null && o.Category == -21 && this.upgradeLevel > 0)
      {
        StardewValley.Object @object = this.attachments[0];
        if (@object != null && @object.canStackWith((Item) o))
        {
          @object.Stack = o.addToStack(@object.Stack);
          if (@object.Stack <= 0)
            @object = (StardewValley.Object) null;
        }
        this.attachments[0] = o;
        Game1.playSound("button1");
        return @object;
      }
      if (o != null && o.Category == -22 && this.upgradeLevel > 2)
      {
        StardewValley.Object attachment = this.attachments[1];
        this.attachments[1] = o;
        Game1.playSound("button1");
        return attachment;
      }
      if (o == null)
      {
        if (this.attachments[0] != null)
        {
          StardewValley.Object attachment = this.attachments[0];
          this.attachments[0] = (StardewValley.Object) null;
          Game1.playSound("dwop");
          return attachment;
        }
        if (this.attachments[1] != null)
        {
          StardewValley.Object attachment = this.attachments[1];
          this.attachments[1] = (StardewValley.Object) null;
          Game1.playSound("dwop");
          return attachment;
        }
      }
      return (StardewValley.Object) null;
    }

    public override void drawAttachments(SpriteBatch b, int x, int y)
    {
      if (this.upgradeLevel > 0)
      {
        if (this.attachments[0] == null)
        {
          b.Draw(Game1.menuTexture, new Vector2((float) x, (float) y), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 36, -1, -1)), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.86f);
        }
        else
        {
          b.Draw(Game1.menuTexture, new Vector2((float) x, (float) y), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 10, -1, -1)), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.86f);
          this.attachments[0].drawInMenu(b, new Vector2((float) x, (float) y), 1f);
        }
      }
      if (this.upgradeLevel <= 2)
        return;
      if (this.attachments[1] == null)
      {
        b.Draw(Game1.menuTexture, new Vector2((float) x, (float) (y + Game1.tileSize + 4)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 37, -1, -1)), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.86f);
      }
      else
      {
        b.Draw(Game1.menuTexture, new Vector2((float) x, (float) (y + Game1.tileSize + 4)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 10, -1, -1)), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.86f);
        this.attachments[1].drawInMenu(b, new Vector2((float) x, (float) (y + Game1.tileSize + 4)), 1f);
      }
    }

    public override bool canThisBeAttached(StardewValley.Object o)
    {
      if (o == null || o.category == -21 && this.upgradeLevel > 0)
        return true;
      if (o.Category == -22)
        return this.upgradeLevel > 2;
      return false;
    }

    public void playerCaughtFishEndFunction(int extraData)
    {
      this.lastUser.Halt();
      this.lastUser.armOffset = Vector2.Zero;
      this.castedButBobberStillInAir = false;
      this.fishCaught = true;
      this.isReeling = false;
      this.isFishing = false;
      this.pullingOutOfWater = false;
      this.lastUser.canReleaseTool = false;
      if (!Game1.isFestival())
      {
        this.recordSize = this.lastUser.caughtFish(this.whichFish, this.fishSize);
        this.lastUser.faceDirection(2);
      }
      else
      {
        Game1.currentLocation.currentEvent.caughtFish(this.whichFish, this.fishSize, this.lastUser);
        this.fishCaught = false;
        this.doneFishing(Game1.player, false);
      }
      if (FishingRod.isFishBossFish(this.whichFish))
        Game1.showGlobalMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingRod.cs.14068"));
      else if (this.recordSize)
      {
        this.sparklingText = new SparklingText(Game1.dialogueFont, Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingRod.cs.14069"), Color.LimeGreen, Color.Azure, false, 0.1, 2500, -1, 500);
        Game1.playSound("newRecord");
      }
      else
        Game1.playSound("fishSlap");
    }

    public static bool isFishBossFish(int index)
    {
      switch (index)
      {
        case 159:
        case 160:
        case 163:
        case 682:
        case 775:
          return true;
        default:
          return false;
      }
    }

    public void pullFishFromWater(int whichFish, int fishSize, int fishQuality, int fishDifficulty, bool treasureCaught, bool wasPerfect = false)
    {
      this.treasureCaught = treasureCaught;
      this.fishSize = fishSize;
      this.fishQuality = fishQuality;
      this.whichFish = whichFish;
      if (!Game1.isFestival())
      {
        this.bossFish = FishingRod.isFishBossFish(whichFish);
        int howMuch = Math.Max(1, (fishQuality + 1) * 3 + fishDifficulty / 3);
        if (treasureCaught)
          howMuch += (int) ((double) howMuch * 1.20000004768372);
        if (wasPerfect)
          howMuch += (int) ((double) howMuch * 1.39999997615814);
        if (this.bossFish)
          howMuch *= 5;
        this.lastUser.gainExperience(1, howMuch);
      }
      float animationInterval;
      if (this.lastUser.FacingDirection == 1 || this.lastUser.FacingDirection == 3)
      {
        float num1 = Vector2.Distance(this.bobber, this.lastUser.position);
        float y = 1f / 1000f;
        float num2 = (float) (Game1.tileSize * 2) - (float) ((double) this.lastUser.position.Y - (double) this.bobber.Y + 10.0);
        double a = 4.0 * Math.PI / 11.0;
        float f = (float) ((double) num1 * (double) y * Math.Tan(a) / Math.Sqrt(2.0 * (double) num1 * (double) y * Math.Tan(a) - 2.0 * (double) y * (double) num2));
        if (float.IsNaN(f))
          f = 0.6f;
        float num3 = f * (float) (1.0 / Math.Tan(a));
        animationInterval = num1 / num3;
        List<TemporaryAnimatedSprite> animations = this.animations;
        TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, whichFish, 16, 16), animationInterval, 1, 0, this.bobber, false, false, this.bobber.Y / 10000f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
        temporaryAnimatedSprite.motion = new Vector2((this.lastUser.FacingDirection == 3 ? -1f : 1f) * -num3, -f);
        temporaryAnimatedSprite.acceleration = new Vector2(0.0f, y);
        int num4 = 1;
        temporaryAnimatedSprite.timeBasedMotion = num4 != 0;
        TemporaryAnimatedSprite.endBehavior endBehavior = new TemporaryAnimatedSprite.endBehavior(this.playerCaughtFishEndFunction);
        temporaryAnimatedSprite.endFunction = endBehavior;
        int num5 = whichFish;
        temporaryAnimatedSprite.extraInfoForEndBehavior = num5;
        string str = "tinyWhip";
        temporaryAnimatedSprite.endSound = str;
        animations.Add(temporaryAnimatedSprite);
      }
      else
      {
        float num1 = this.bobber.Y - (float) this.lastUser.getStandingY();
        float num2 = Math.Abs(num1 + (float) (Game1.tileSize * 4) + (float) (Game1.tileSize / 2));
        if (this.lastUser.FacingDirection == 0)
          num2 += (float) (Game1.tileSize * 3 / 2);
        float y = 0.005f;
        float num3 = (float) Math.Sqrt(2.0 * (double) y * (double) num2);
        animationInterval = (float) (Math.Sqrt(2.0 * ((double) num2 - (double) num1) / (double) y) + (double) num3 / (double) y);
        float x = 0.0f;
        if ((double) animationInterval != 0.0)
          x = (this.lastUser.position.X - this.bobber.X) / animationInterval;
        List<TemporaryAnimatedSprite> animations = this.animations;
        TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, whichFish, 16, 16), animationInterval, 1, 0, new Vector2(this.bobber.X, this.bobber.Y), false, false, this.bobber.Y / 10000f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
        temporaryAnimatedSprite.motion = new Vector2(x, -num3);
        temporaryAnimatedSprite.acceleration = new Vector2(0.0f, y);
        int num4 = 1;
        temporaryAnimatedSprite.timeBasedMotion = num4 != 0;
        TemporaryAnimatedSprite.endBehavior endBehavior = new TemporaryAnimatedSprite.endBehavior(this.playerCaughtFishEndFunction);
        temporaryAnimatedSprite.endFunction = endBehavior;
        int num5 = whichFish;
        temporaryAnimatedSprite.extraInfoForEndBehavior = num5;
        string str = "tinyWhip";
        temporaryAnimatedSprite.endSound = str;
        animations.Add(temporaryAnimatedSprite);
      }
      Game1.playSound("pullItemFromWater");
      Game1.playSound("dwop");
      this.castedButBobberStillInAir = false;
      this.pullingOutOfWater = true;
      this.isFishing = false;
      this.isReeling = false;
      switch (this.lastUser.FacingDirection)
      {
        case 0:
          this.lastUser.FarmerSprite.animateBackwardsOnce(299, animationInterval);
          break;
        case 1:
          this.lastUser.FarmerSprite.animateBackwardsOnce(300, animationInterval);
          break;
        case 2:
          this.lastUser.FarmerSprite.animateBackwardsOnce(301, animationInterval);
          break;
        case 3:
          this.lastUser.FarmerSprite.animateBackwardsOnce(302, animationInterval);
          break;
      }
    }

    public override void draw(SpriteBatch b)
    {
      base.draw(b);
      if (!this.bobber.Equals(Vector2.Zero) && this.isFishing)
      {
        Vector2 bobber = this.bobber;
        float scale = 4f;
        if ((double) this.bobberTimeAccumulator > (double) this.timePerBobberBob)
        {
          if (!this.isNibbling && !this.isReeling || Game1.random.NextDouble() < 0.05)
          {
            Game1.playSound("waterSlosh");
            this.lastUser.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 0, Game1.tileSize, Game1.tileSize), 150f, 8, 0, new Vector2((float) ((double) this.bobber.X - (double) Game1.tileSize - 14.0), (float) ((double) this.bobber.Y - (double) Game1.tileSize - 10.0)), false, Game1.random.NextDouble() < 0.5, 1f / 1000f, 0.01f, Color.White, 0.75f, 3f / 1000f, 0.0f, 0.0f, false));
          }
          this.timePerBobberBob = this.bobberBob == 0 ? (float) Game1.random.Next(1500, 3500) : (float) Game1.random.Next(350, 750);
          this.bobberTimeAccumulator = 0.0f;
          if (this.isNibbling || this.isReeling)
          {
            this.timePerBobberBob = (float) Game1.random.Next(25, 75);
            bobber.X += (float) Game1.random.Next(-5, 5);
            bobber.Y += (float) Game1.random.Next(-5, 5);
            if (!this.isReeling)
              scale += (float) Game1.random.Next(-20, 20) / 100f;
          }
          else if (Game1.random.NextDouble() < 0.1)
            Game1.playSound("bob");
        }
        b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, bobber), new Rectangle?(new Rectangle(179 + this.bobberBob * 9, 1903, 9, 9)), Color.White, 0.0f, new Vector2(4f, 4f) * scale, scale, SpriteEffects.None, 0.1f);
      }
      else if (this.isTimingCast || (double) this.castingChosenCountdown > 0.0)
      {
        int num1 = (int) (-(double) Math.Abs(this.castingChosenCountdown / 2f - this.castingChosenCountdown) / 50.0);
        float num2 = (double) this.castingChosenCountdown <= 0.0 || (double) this.castingChosenCountdown >= 100.0 ? 1f : this.castingChosenCountdown / 100f;
        b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.getLastFarmerToUse().position + new Vector2((float) (-Game1.tileSize / 2 - 16), (float) (-Game1.tileSize * 2 - Game1.tileSize / 2 + num1))), new Rectangle?(new Rectangle(193, 1868, 47, 12)), Color.White * num2, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 0.885f);
        b.Draw(Game1.staminaRect, new Rectangle((int) Game1.GlobalToLocal(Game1.viewport, this.getLastFarmerToUse().position).X - Game1.tileSize / 2 - 4, (int) Game1.GlobalToLocal(Game1.viewport, this.getLastFarmerToUse().position).Y + num1 - Game1.tileSize * 2 - Game1.tileSize / 2 + 12, (int) (164.0 * (double) this.castingPower), 25), new Rectangle?(Game1.staminaRect.Bounds), Utility.getRedToGreenLerpColor(this.castingPower) * num2, 0.0f, Vector2.Zero, SpriteEffects.None, 0.887f);
      }
      for (int index = this.animations.Count - 1; index >= 0; --index)
        this.animations[index].draw(b, false, 0, 0);
      if (this.sparklingText != null && !this.fishCaught)
        this.sparklingText.draw(b, Game1.GlobalToLocal(Game1.viewport, this.getLastFarmerToUse().position + new Vector2((float) (-Game1.tileSize / 2 + 8), (float) (-Game1.tileSize * 2 - Game1.tileSize))));
      else if (this.sparklingText != null && this.fishCaught)
        this.sparklingText.draw(b, Game1.GlobalToLocal(Game1.viewport, this.getLastFarmerToUse().position + new Vector2((float) (-Game1.tileSize / 2 - 32), (float) (-Game1.tileSize * 4) - (float) Game1.tileSize * 1.5f)));
      if ((this.isFishing || this.pullingOutOfWater || this.castedButBobberStillInAir) && this.lastUser.FarmerSprite.CurrentFrame != 57 && (this.lastUser.FacingDirection != 0 || !this.pullingOutOfWater || this.whichFish == -1))
      {
        Vector2 vector2_1 = this.isFishing ? this.bobber : (this.animations.Count > 0 ? this.animations[0].position + new Vector2((float) (Game1.tileSize / 2 + 8), (float) (Game1.tileSize + 4)) : Vector2.Zero);
        if (this.whichFish != -1)
          vector2_1 += new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2));
        Vector2 vector2_2 = Vector2.Zero;
        if (this.castedButBobberStillInAir)
        {
          switch (this.lastUser.FacingDirection)
          {
            case 0:
              switch (this.lastUser.FarmerSprite.indexInCurrentAnimation)
              {
                case 0:
                  vector2_2 = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(22f, (float) ((double) this.lastUser.armOffset.Y - (double) (Game1.tileSize * 3 / 2) + 4.0)));
                  break;
                case 1:
                  vector2_2 = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(32f, (float) ((double) this.lastUser.armOffset.Y - (double) (Game1.tileSize * 3 / 2) + 4.0)));
                  break;
                case 2:
                  vector2_2 = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(36f, (float) ((double) this.lastUser.armOffset.Y - (double) Game1.tileSize + 40.0)));
                  break;
                case 3:
                  vector2_2 = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(36f, this.lastUser.armOffset.Y - 16f));
                  break;
                case 4:
                  vector2_2 = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(36f, this.lastUser.armOffset.Y - 32f));
                  break;
                case 5:
                  vector2_2 = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(36f, this.lastUser.armOffset.Y - 32f));
                  break;
                default:
                  vector2_2 = Vector2.Zero;
                  break;
              }
            case 1:
              switch (this.lastUser.FarmerSprite.indexInCurrentAnimation)
              {
                case 0:
                  vector2_2 = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(-48f, (float) ((double) this.lastUser.armOffset.Y - (double) (Game1.tileSize * 3 / 2) - 8.0)));
                  break;
                case 1:
                  vector2_2 = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(-16f, (float) ((double) this.lastUser.armOffset.Y - (double) (Game1.tileSize * 3 / 2) - 20.0)));
                  break;
                case 2:
                  vector2_2 = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float) (Game1.tileSize + 20), (float) ((double) this.lastUser.armOffset.Y - (double) (Game1.tileSize * 3 / 2) - 20.0)));
                  break;
                case 3:
                  vector2_2 = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float) (Game1.tileSize * 2 - 16), (float) ((double) this.lastUser.armOffset.Y - (double) (Game1.tileSize / 2) - 20.0)));
                  break;
                case 4:
                  vector2_2 = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float) (Game1.tileSize * 2 - 8), (float) ((double) this.lastUser.armOffset.Y - (double) (Game1.tileSize / 2) + 8.0)));
                  break;
                case 5:
                  vector2_2 = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float) (Game1.tileSize * 2 - 8), (float) ((double) this.lastUser.armOffset.Y - (double) (Game1.tileSize / 2) + 8.0)));
                  break;
                default:
                  vector2_2 = Vector2.Zero;
                  break;
              }
            case 2:
              switch (this.lastUser.FarmerSprite.indexInCurrentAnimation)
              {
                case 0:
                  vector2_2 = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(8f, (float) ((double) this.lastUser.armOffset.Y - (double) (Game1.tileSize * 3 / 2) + 4.0)));
                  break;
                case 1:
                  vector2_2 = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(22f, (float) ((double) this.lastUser.armOffset.Y - (double) (Game1.tileSize * 3 / 2) + 4.0)));
                  break;
                case 2:
                  vector2_2 = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(28f, (float) ((double) this.lastUser.armOffset.Y - (double) Game1.tileSize + 40.0)));
                  break;
                case 3:
                  vector2_2 = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(28f, this.lastUser.armOffset.Y - 8f));
                  break;
                case 4:
                  vector2_2 = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(28f, this.lastUser.armOffset.Y + 32f));
                  break;
                case 5:
                  vector2_2 = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(28f, this.lastUser.armOffset.Y + 32f));
                  break;
                default:
                  vector2_2 = Vector2.Zero;
                  break;
              }
            case 3:
              switch (this.lastUser.FarmerSprite.indexInCurrentAnimation)
              {
                case 0:
                  vector2_2 = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(112f, (float) ((double) this.lastUser.armOffset.Y - (double) (Game1.tileSize * 3 / 2) - 8.0)));
                  break;
                case 1:
                  vector2_2 = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(80f, (float) ((double) this.lastUser.armOffset.Y - (double) (Game1.tileSize * 3 / 2) - 20.0)));
                  break;
                case 2:
                  vector2_2 = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float) (32 + (32 - (Game1.tileSize + 20))), (float) ((double) this.lastUser.armOffset.Y - (double) (Game1.tileSize * 3 / 2) - 20.0)));
                  break;
                case 3:
                  vector2_2 = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float) (32 + (32 - (Game1.tileSize * 2 - 16))), (float) ((double) this.lastUser.armOffset.Y - (double) (Game1.tileSize / 2) - 20.0)));
                  break;
                case 4:
                  vector2_2 = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float) (32 + (32 - (Game1.tileSize * 2 - 8))), (float) ((double) this.lastUser.armOffset.Y - (double) (Game1.tileSize / 2) + 8.0)));
                  break;
                case 5:
                  vector2_2 = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float) (32 + (32 - (Game1.tileSize * 2 - 8))), (float) ((double) this.lastUser.armOffset.Y - (double) (Game1.tileSize / 2) + 8.0)));
                  break;
              }
            default:
              vector2_2 = Vector2.Zero;
              break;
          }
        }
        else if (this.isReeling)
        {
          if (Game1.didPlayerJustClickAtAll())
          {
            switch (this.lastUser.FacingDirection)
            {
              case 0:
                vector2_2 = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(24f, (float) ((double) this.lastUser.armOffset.Y - (double) (Game1.tileSize * 3 / 2) + 12.0)));
                break;
              case 1:
                vector2_2 = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(20f, (float) ((double) this.lastUser.armOffset.Y - (double) (Game1.tileSize * 3 / 2) - 12.0)));
                break;
              case 2:
                vector2_2 = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(12f, (float) ((double) this.lastUser.armOffset.Y - (double) (Game1.tileSize * 3 / 2) + 8.0)));
                break;
              case 3:
                vector2_2 = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(48f, (float) ((double) this.lastUser.armOffset.Y - (double) (Game1.tileSize * 3 / 2) - 12.0)));
                break;
            }
          }
          else
          {
            switch (this.lastUser.FacingDirection)
            {
              case 0:
                vector2_2 = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(25f, (float) ((double) this.lastUser.armOffset.Y - (double) (Game1.tileSize * 3 / 2) + 4.0)));
                break;
              case 1:
                vector2_2 = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(28f, (float) ((double) this.lastUser.armOffset.Y - (double) (Game1.tileSize * 3 / 2) - 8.0)));
                break;
              case 2:
                vector2_2 = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(12f, (float) ((double) this.lastUser.armOffset.Y - (double) (Game1.tileSize * 3 / 2) + 4.0)));
                break;
              case 3:
                vector2_2 = Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(36f, (float) ((double) this.lastUser.armOffset.Y - (double) (Game1.tileSize * 3 / 2) - 8.0)));
                break;
            }
          }
        }
        else
        {
          switch (this.lastUser.FacingDirection)
          {
            case 0:
              vector2_2 = this.pullingOutOfWater ? Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(22f, (float) ((double) this.lastUser.armOffset.Y - (double) (Game1.tileSize * 3 / 2) + 4.0))) : Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(28f, (float) ((double) this.lastUser.armOffset.Y - (double) Game1.tileSize - 12.0)));
              break;
            case 1:
              vector2_2 = this.pullingOutOfWater ? Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(-48f, (float) ((double) this.lastUser.armOffset.Y - (double) (Game1.tileSize * 3 / 2) - 8.0))) : Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float) (Game1.tileSize * 2 - 8), (float) ((double) this.lastUser.armOffset.Y - (double) Game1.tileSize + 16.0)));
              break;
            case 2:
              vector2_2 = this.pullingOutOfWater ? Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(8f, (float) ((double) this.lastUser.armOffset.Y - (double) (Game1.tileSize * 3 / 2) + 4.0))) : Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(28f, (float) ((double) this.lastUser.armOffset.Y + (double) Game1.tileSize - 12.0)));
              break;
            case 3:
              vector2_2 = this.pullingOutOfWater ? Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2(112f, (float) ((double) this.lastUser.armOffset.Y - (double) (Game1.tileSize * 3 / 2) - 8.0))) : Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float) (32 + (32 - (Game1.tileSize * 2 - 8))), (float) ((double) this.lastUser.armOffset.Y - (double) Game1.tileSize + 16.0)));
              break;
            default:
              vector2_2 = Vector2.Zero;
              break;
          }
        }
        Vector2 local = Game1.GlobalToLocal(Game1.viewport, vector2_1 + new Vector2(-36f, (float) (-Game1.tileSize / 2 - 24 + (this.bobberBob == 1 ? 4 : 0))));
        if (this.isReeling)
        {
          Utility.drawLineWithScreenCoordinates((int) vector2_2.X, (int) vector2_2.Y, (int) local.X, (int) local.Y, b, Color.White * 0.5f, 1f);
        }
        else
        {
          Vector2 p0 = vector2_2;
          Vector2 p1 = new Vector2(vector2_2.X + (float) (((double) local.X - (double) vector2_2.X) / 3.0), vector2_2.Y + (float) (((double) local.Y - (double) vector2_2.Y) * 2.0 / 3.0));
          Vector2 p2 = new Vector2(vector2_2.X + (float) (((double) local.X - (double) vector2_2.X) * 2.0 / 3.0), vector2_2.Y + (float) (((double) local.Y - (double) vector2_2.Y) * (this.isFishing ? 6.0 : 2.0) / 5.0));
          Vector2 p3 = local;
          float t = 0.0f;
          while ((double) t < 1.0)
          {
            Vector2 curvePoint = Utility.GetCurvePoint(t, p0, p1, p2, p3);
            Utility.drawLineWithScreenCoordinates((int) vector2_2.X, (int) vector2_2.Y, (int) curvePoint.X, (int) curvePoint.Y, b, Color.White * 0.5f, (float) ((double) this.lastUser.getStandingY() / 10000.0 + (this.lastUser.FacingDirection != 0 ? 0.00499999988824129 : -1.0 / 1000.0)));
            vector2_2 = curvePoint;
            t += 0.025f;
          }
        }
      }
      else
      {
        if (!this.fishCaught)
          return;
        float num = (float) (4.0 * Math.Round(Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 250.0), 2));
        b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float) (-Game1.tileSize * 2 + 8), (float) (-Game1.tileSize * 5 + Game1.tileSize / 2) + num)), new Rectangle?(new Rectangle(31, 1870, 73, 49)), Color.White * 0.8f, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, (float) ((double) this.lastUser.getStandingY() / 10000.0 + 0.0599999986588955));
        b.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float) (-Game1.tileSize * 2 + 4), (float) (-Game1.tileSize * 5 + Game1.tileSize / 2 + 4) + num) + new Vector2(44f, 68f)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.whichFish, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, (float) ((double) this.lastUser.getStandingY() / 10000.0 + 9.99999974737875E-05 + 0.0599999986588955));
        b.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float) (Game1.tileSize / 8 - 8), (float) (-Game1.tileSize / 4 - Game1.tileSize / 2 - Game1.pixelZoom * 2))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.whichFish, 16, 16)), Color.White, this.fishSize == -1 ? 0.0f : 2.356194f, new Vector2(8f, 8f), (float) Game1.pixelZoom * 0.75f, SpriteEffects.None, (float) ((double) this.lastUser.getStandingY() / 10000.0 + 1.0 / 500.0 + 0.0599999986588955));
        string text = Game1.objectInformation[this.whichFish].Split('/')[4];
        b.DrawString(Game1.smallFont, text, Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float) (-Game1.tileSize * 2 + 8 + 146) - Game1.smallFont.MeasureString(text).X / 2f, (float) (-Game1.tileSize * 5 + Game1.tileSize * 2 / 3) + num)), this.bossFish ? new Color(126, 61, 237) : Game1.textColor, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, (float) ((double) this.lastUser.getStandingY() / 10000.0 + 1.0 / 500.0 + 0.0599999986588955));
        if (this.fishSize == -1)
          return;
        b.DrawString(Game1.smallFont, Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingRod.cs.14082"), Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float) (-Game1.tileSize * 2 + 8 + 140), (float) (-Game1.tileSize * 5 + Game1.tileSize * 5 / 3) + num)), Game1.textColor, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, (float) ((double) this.lastUser.getStandingY() / 10000.0 + 1.0 / 500.0 + 0.0599999986588955));
        b.DrawString(Game1.smallFont, Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingRod.cs.14083", (object) (LocalizedContentManager.CurrentLanguageCode != LocalizedContentManager.LanguageCode.en ? Math.Round((double) this.fishSize * 2.54) : (double) this.fishSize)), Game1.GlobalToLocal(Game1.viewport, this.lastUser.position + new Vector2((float) ((double) (-Game1.tileSize * 2 + 8 + 205) - (double) Game1.smallFont.MeasureString(Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingRod.cs.14083", (object) (LocalizedContentManager.CurrentLanguageCode != LocalizedContentManager.LanguageCode.en ? Math.Round((double) this.fishSize * 2.54) : (double) this.fishSize))).X / 2.0), (float) (-Game1.tileSize * 5 + Game1.tileSize * 7 / 3 - 8) + num)), this.recordSize ? Color.Blue * Math.Min(1f, (float) ((double) num / 8.0 + 1.5)) : Game1.textColor, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, (float) ((double) this.lastUser.getStandingY() / 10000.0 + 1.0 / 500.0 + 0.0599999986588955));
      }
    }

    public override bool beginUsing(GameLocation location, int x, int y, Farmer who)
    {
      if ((double) who.Stamina <= 1.0)
      {
        if (!who.isEmoting)
          who.doEmote(36);
        who.CanMove = !Game1.eventUp;
        who.UsingTool = false;
        who.canReleaseTool = false;
        this.doneFishing((Farmer) null, false);
        return true;
      }
      this.usedGamePadToCast = false;
      if (GamePad.GetState(Game1.playerOneIndex).IsButtonDown(Buttons.X))
        this.usedGamePadToCast = true;
      this.bossFish = false;
      this.originalFacingDirection = who.FacingDirection;
      who.Halt();
      this.treasureCaught = false;
      this.showingTreasure = false;
      this.isFishing = false;
      this.hit = false;
      this.favBait = false;
      if (this.attachments != null && this.attachments.Length > 1 && this.attachments[1] != null)
        this.hadBobber = true;
      this.isNibbling = false;
      this.lastUser = who;
      this.isTimingCast = true;
      who.usingTool = true;
      this.whichFish = -1;
      who.canMove = false;
      this.fishCaught = false;
      this.doneWithAnimation = false;
      who.canReleaseTool = false;
      this.hasDoneFucntionYet = false;
      this.isReeling = false;
      this.pullingOutOfWater = false;
      this.castingPower = 0.0f;
      this.castingChosenCountdown = 0.0f;
      this.animations.Clear();
      this.sparklingText = (SparklingText) null;
      switch (who.FacingDirection)
      {
        case 0:
          who.FarmerSprite.setCurrentFrame(295);
          Game1.player.CurrentTool.Update(0, 0);
          break;
        case 1:
          who.FarmerSprite.setCurrentFrame(296);
          Game1.player.CurrentTool.Update(1, 0);
          break;
        case 2:
          who.FarmerSprite.setCurrentFrame(297);
          Game1.player.CurrentTool.Update(2, 0);
          break;
        case 3:
          who.FarmerSprite.setCurrentFrame(298);
          Game1.player.CurrentTool.Update(3, 0);
          break;
      }
      return true;
    }

    public void doneFishing(Farmer who, bool consumeBaitAndTackle = false)
    {
      if (consumeBaitAndTackle)
      {
        if (this.attachments[0] != null)
        {
          --this.attachments[0].Stack;
          if (this.attachments[0].Stack <= 0)
          {
            this.attachments[0] = (StardewValley.Object) null;
            Game1.showGlobalMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingRod.cs.14085"));
          }
        }
        if (this.attachments[1] != null)
        {
          this.attachments[1].scale.Y -= 0.05f;
          if ((double) this.attachments[1].scale.Y <= 0.0)
          {
            this.attachments[1] = (StardewValley.Object) null;
            Game1.showGlobalMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingRod.cs.14086"));
          }
        }
      }
      this.bobber = Vector2.Zero;
      this.isNibbling = false;
      this.fishCaught = false;
      this.isFishing = false;
      this.isReeling = false;
      this.doneWithAnimation = false;
      this.pullingOutOfWater = false;
      this.fishingBiteAccumulator = 0.0f;
      this.fishingNibbleAccumulator = 0.0f;
      this.timeUntilFishingBite = -1f;
      this.timeUntilFishingNibbleDone = -1f;
      this.bobberTimeAccumulator = 0.0f;
      if (FishingRod.chargeSound != null && FishingRod.chargeSound.IsPlaying)
        FishingRod.chargeSound.Stop(AudioStopOptions.Immediate);
      if (FishingRod.reelSound != null && FishingRod.reelSound.IsPlaying)
        FishingRod.reelSound.Stop(AudioStopOptions.Immediate);
      if (who == null)
        return;
      who.UsingTool = false;
      who.Halt();
      who.faceDirection(this.originalFacingDirection);
    }

    public static void doneWithCastingAnimation(Farmer who)
    {
      if (who.CurrentTool == null || !(who.CurrentTool is FishingRod))
        return;
      (who.CurrentTool as FishingRod).doneWithAnimation = true;
      if (!(who.CurrentTool as FishingRod).hasDoneFucntionYet)
        return;
      who.canReleaseTool = true;
      who.usingTool = false;
      who.canMove = true;
      Farmer.canMoveNow(who);
    }

    public void castingEndFunction(int extraInfo)
    {
      this.castedButBobberStillInAir = false;
      if (this.lastUser == null)
        return;
      float stamina = this.lastUser.Stamina;
      this.lastUser.CurrentTool.DoFunction(this.lastUser.currentLocation, (int) this.bobber.X, (int) this.bobber.Y, 1, this.lastUser);
      this.lastUser.lastClick = Vector2.Zero;
      if (FishingRod.reelSound != null)
        FishingRod.reelSound.Stop(AudioStopOptions.Immediate);
      FishingRod.reelSound = (Cue) null;
      if ((double) this.lastUser.Stamina <= 0.0 && (double) stamina > 0.0)
        this.lastUser.doEmote(36);
      Game1.toolHold = 0.0f;
      if (this.isFishing || !this.doneWithAnimation)
        return;
      Farmer.canMoveNow(this.lastUser);
    }

    public override void tickUpdate(GameTime time, Farmer who)
    {
      if (Game1.paused)
        return;
      if (who.CurrentTool != null && who.CurrentTool.Equals((object) this) && who.usingTool)
        who.CanMove = false;
      else if (Game1.currentMinigame == null && (who.CurrentTool == null || !(who.CurrentTool is FishingRod) || !who.usingTool))
      {
        if (FishingRod.chargeSound == null || !FishingRod.chargeSound.IsPlaying)
          return;
        FishingRod.chargeSound.Stop(AudioStopOptions.Immediate);
        FishingRod.chargeSound = (Cue) null;
        return;
      }
      for (int index = this.animations.Count - 1; index >= 0; --index)
      {
        if (this.animations[index].update(time))
          this.animations.RemoveAt(index);
      }
      if (this.sparklingText != null && this.sparklingText.update(time))
        this.sparklingText = (SparklingText) null;
      if ((double) this.castingChosenCountdown > 0.0)
      {
        this.castingChosenCountdown = this.castingChosenCountdown - (float) time.ElapsedGameTime.Milliseconds;
        if ((double) this.castingChosenCountdown <= 0.0)
        {
          switch (who.FacingDirection)
          {
            case 0:
              who.FarmerSprite.animateOnce(295, 1f, 1);
              Game1.player.CurrentTool.Update(0, 0);
              break;
            case 1:
              who.FarmerSprite.animateOnce(296, 1f, 1);
              Game1.player.CurrentTool.Update(1, 0);
              break;
            case 2:
              who.FarmerSprite.animateOnce(297, 1f, 1);
              Game1.player.CurrentTool.Update(2, 0);
              break;
            case 3:
              who.FarmerSprite.animateOnce(298, 1f, 1);
              Game1.player.CurrentTool.Update(3, 0);
              break;
          }
          if (who.FacingDirection == 1 || who.FacingDirection == 3)
          {
            float num1 = Math.Max((float) (Game1.tileSize * 2), this.castingPower * (float) (this.getAddedDistance(who) + 4) * (float) Game1.tileSize);
            if (who.FacingDirection == 3)
              num1 = Math.Max((float) (Game1.tileSize * 2), num1 - (float) Game1.tileSize);
            float num2 = num1 - 8f;
            float y = 0.005f;
            float num3 = num2 * (float) Math.Sqrt((double) y / (2.0 * ((double) num2 + (double) (Game1.tileSize * 3 / 2))));
            float animationInterval = (float) (2.0 * ((double) num3 / (double) y)) + ((float) Math.Sqrt((double) num3 * (double) num3 + 2.0 * (double) y * (double) (Game1.tileSize * 3 / 2)) - num3) / y;
            this.bobber = new Vector2((float) who.getStandingX() + (who.FacingDirection == 3 ? -1f : 1f) * num2 + (float) (Game1.tileSize / 2), (float) (who.getStandingY() + Game1.tileSize / 2));
            List<TemporaryAnimatedSprite> animations = this.animations;
            TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(170, 1903, 7, 8), animationInterval, 1, 0, who.position + new Vector2(0.0f, (float) (-Game1.tileSize * 3 / 2)), false, false, (float) who.getStandingY() / 10000f, 0.0f, Color.White, 4f, 0.0f, 0.0f, (float) Game1.random.Next(-20, 20) / 100f, false);
            temporaryAnimatedSprite.motion = new Vector2((who.FacingDirection == 3 ? -1f : 1f) * num3, -num3);
            temporaryAnimatedSprite.acceleration = new Vector2(0.0f, y);
            temporaryAnimatedSprite.endFunction = new TemporaryAnimatedSprite.endBehavior(this.castingEndFunction);
            int num4 = 1;
            temporaryAnimatedSprite.timeBasedMotion = num4 != 0;
            animations.Add(temporaryAnimatedSprite);
          }
          else
          {
            float num1 = -Math.Max((float) (Game1.tileSize * 2), this.castingPower * (float) (this.getAddedDistance(who) + 3) * (float) Game1.tileSize);
            float num2 = Math.Abs(num1 - (float) Game1.tileSize);
            if (this.lastUser.FacingDirection == 0)
            {
              num1 = -num1;
              num2 += (float) Game1.tileSize;
            }
            float y = 0.005f;
            float num3 = (float) Math.Sqrt(2.0 * (double) y * (double) num2);
            float animationInterval = (float) (Math.Sqrt(2.0 * ((double) num2 - (double) num1) / (double) y) + (double) num3 / (double) y) * 1.05f;
            if (this.lastUser.FacingDirection == 0)
              animationInterval *= 1.05f;
            this.bobber = new Vector2((float) (who.getStandingX() + Game1.random.Next(48)), (float) (who.getStandingY() + Game1.tileSize / 2) - num1);
            List<TemporaryAnimatedSprite> animations = this.animations;
            TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(170, 1903, 7, 8), animationInterval, 1, 0, who.position + new Vector2(0.0f, (float) (-Game1.tileSize * 3 / 2)), false, false, this.bobber.Y / 10000f, 0.0f, Color.White, 4f, 0.0f, 0.0f, (float) Game1.random.Next(-20, 20) / 100f, false);
            temporaryAnimatedSprite.alphaFade = 0.0001f;
            Vector2 vector2_1 = new Vector2(0.0f, -num3);
            temporaryAnimatedSprite.motion = vector2_1;
            Vector2 vector2_2 = new Vector2(0.0f, y);
            temporaryAnimatedSprite.acceleration = vector2_2;
            TemporaryAnimatedSprite.endBehavior endBehavior = new TemporaryAnimatedSprite.endBehavior(this.castingEndFunction);
            temporaryAnimatedSprite.endFunction = endBehavior;
            int num4 = 1;
            temporaryAnimatedSprite.timeBasedMotion = num4 != 0;
            animations.Add(temporaryAnimatedSprite);
          }
          this.castedButBobberStillInAir = true;
          this.isCasting = false;
          Game1.playSound("cast");
          if (Game1.soundBank != null)
          {
            FishingRod.reelSound = Game1.soundBank.GetCue("slowReel");
            FishingRod.reelSound.SetVariable("Pitch", 1600f);
            FishingRod.reelSound.Play();
          }
        }
      }
      else if (!this.isTimingCast && (double) this.castingChosenCountdown <= 0.0)
        who.jitterStrength = 0.0f;
      if (this.isTimingCast)
      {
        if (FishingRod.chargeSound == null && Game1.soundBank != null)
          FishingRod.chargeSound = Game1.soundBank.GetCue("SinWave");
        if ((double) this.castingPower > 0.0 && FishingRod.chargeSound != null && (!FishingRod.chargeSound.IsPlaying && !FishingRod.chargeSound.IsStopped))
          FishingRod.chargeSound.Play();
        double num1 = 0.0;
        double num2 = 1.0;
        double castingPower = (double) this.castingPower;
        double castingTimerSpeed = (double) this.castingTimerSpeed;
        TimeSpan timeSpan = time.ElapsedGameTime;
        double milliseconds = (double) timeSpan.Milliseconds;
        double num3 = castingTimerSpeed * milliseconds;
        double num4 = castingPower + num3;
        double num5 = (double) Math.Min((float) num2, (float) num4);
        this.castingPower = Math.Max((float) num1, (float) num5);
        if (FishingRod.chargeSound != null)
          FishingRod.chargeSound.SetVariable("Pitch", 2400f * this.castingPower);
        if ((double) this.castingPower == 1.0 || (double) this.castingPower == 0.0)
          this.castingTimerSpeed = -this.castingTimerSpeed;
        // ISSUE: explicit reference operation
        // ISSUE: variable of a reference type
        Vector2& local = @who.armOffset;
        double num6 = 2.0;
        timeSpan = DateTime.Now.TimeOfDay;
        double num7 = Math.Round(Math.Sin(timeSpan.TotalMilliseconds / 250.0), 2);
        double num8 = num6 * num7;
        // ISSUE: explicit reference operation
        (^local).Y = (float) num8;
        who.jitterStrength = Math.Max(0.0f, this.castingPower - 0.5f);
        if ((this.usedGamePadToCast || Mouse.GetState().LeftButton != ButtonState.Released) && (!this.usedGamePadToCast || !Game1.options.gamepadControls || !GamePad.GetState(Game1.playerOneIndex).IsButtonUp(Buttons.X)) || !Game1.areAllOfTheseKeysUp(Keyboard.GetState(), Game1.options.useToolButton))
          return;
        if (FishingRod.chargeSound != null)
        {
          FishingRod.chargeSound.Stop(AudioStopOptions.Immediate);
          FishingRod.chargeSound = (Cue) null;
        }
        Game1.playSound("button1");
        Rumble.rumble(0.5f, 150f);
        this.isTimingCast = false;
        this.isCasting = true;
        this.castingChosenCountdown = 350f;
        who.armOffset.Y = 0.0f;
        if ((double) this.castingPower <= 0.990000009536743)
          return;
        Game1.screenOverlayTempSprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(545, 1921, 53, 19), 800f, 1, 0, Game1.GlobalToLocal(Game1.viewport, Game1.player.position + new Vector2(0.0f, (float) (-Game1.tileSize * 3))), false, false, 1f, 0.01f, Color.White, 2f, 0.0f, 0.0f, 0.0f, true)
        {
          motion = new Vector2(0.0f, -4f),
          acceleration = new Vector2(0.0f, 0.2f),
          delayBeforeAnimationStart = 200
        });
        DelayedAction.playSoundAfterDelay("crit", 200);
      }
      else if (this.isReeling)
      {
        if (Game1.didPlayerJustClickAtAll())
        {
          if (Game1.isAnyGamePadButtonBeingPressed())
            Game1.lastCursorMotionWasMouse = false;
          switch (who.FacingDirection)
          {
            case 0:
              who.FarmerSprite.setCurrentSingleFrame(76, (short) 32000, false, false);
              break;
            case 1:
              who.FarmerSprite.setCurrentSingleFrame(72, (short) 100, false, false);
              break;
            case 2:
              who.FarmerSprite.setCurrentSingleFrame(75, (short) 32000, false, false);
              break;
            case 3:
              who.FarmerSprite.setCurrentSingleFrame(72, (short) 100, false, true);
              break;
          }
          who.armOffset.Y = (float) Math.Round(Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 250.0), 2);
          who.jitterStrength = 1f;
        }
        else
        {
          switch (who.FacingDirection)
          {
            case 0:
              who.FarmerSprite.setCurrentSingleFrame(36, (short) 32000, false, false);
              break;
            case 1:
              who.FarmerSprite.setCurrentSingleFrame(48, (short) 100, false, false);
              break;
            case 2:
              who.FarmerSprite.setCurrentSingleFrame(66, (short) 32000, false, false);
              break;
            case 3:
              who.FarmerSprite.setCurrentSingleFrame(48, (short) 100, false, true);
              break;
          }
          who.stopJittering();
        }
        who.armOffset = new Vector2((float) Game1.random.Next(-10, 11) / 10f, (float) Game1.random.Next(-10, 11) / 10f);
        this.bobberTimeAccumulator = this.bobberTimeAccumulator + (float) time.ElapsedGameTime.Milliseconds;
      }
      else if (this.isFishing)
      {
        this.bobber.Y += (float) (0.100000001490116 * Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 250.0));
        who.canReleaseTool = true;
        double bobberTimeAccumulator = (double) this.bobberTimeAccumulator;
        TimeSpan timeSpan = time.ElapsedGameTime;
        double milliseconds1 = (double) timeSpan.Milliseconds;
        this.bobberTimeAccumulator = (float) (bobberTimeAccumulator + milliseconds1);
        switch (who.FacingDirection)
        {
          case 0:
            who.FarmerSprite.setCurrentFrame(44);
            break;
          case 1:
            who.FarmerSprite.setCurrentFrame(89);
            break;
          case 2:
            who.FarmerSprite.setCurrentFrame(70);
            break;
          case 3:
            who.FarmerSprite.setCurrentFrame(89, 0, 10, 1, true, false);
            break;
        }
        // ISSUE: explicit reference operation
        // ISSUE: variable of a reference type
        Vector2& local = @who.armOffset;
        timeSpan = DateTime.Now.TimeOfDay;
        double num = Math.Round(Math.Sin(timeSpan.TotalMilliseconds / 250.0), 2) + (who.FacingDirection == 1 || who.FacingDirection == 3 ? 1.0 : -1.0);
        // ISSUE: explicit reference operation
        (^local).Y = (float) num;
        if (!who.IsMainPlayer)
          return;
        if ((double) this.timeUntilFishingBite != -1.0)
        {
          double fishingBiteAccumulator = (double) this.fishingBiteAccumulator;
          timeSpan = time.ElapsedGameTime;
          double milliseconds2 = (double) timeSpan.Milliseconds;
          this.fishingBiteAccumulator = (float) (fishingBiteAccumulator + milliseconds2);
          if ((double) this.fishingBiteAccumulator > (double) this.timeUntilFishingBite)
          {
            this.fishingBiteAccumulator = 0.0f;
            this.timeUntilFishingBite = -1f;
            this.isNibbling = true;
            Game1.playSound("fishBite");
            Rumble.rumble(0.75f, 250f);
            this.timeUntilFishingNibbleDone = (float) FishingRod.maxTimeToNibble;
            if (Game1.currentMinigame == null)
              Game1.screenOverlayTempSprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(395, 497, 3, 8), new Vector2((float) (this.lastUser.getStandingX() - Game1.viewport.X), (float) (this.lastUser.getStandingY() - Game1.tileSize * 2 - Game1.pixelZoom * 2 - Game1.viewport.Y)), false, 0.02f, Color.White)
              {
                scale = (float) (Game1.pixelZoom + 1),
                scaleChange = -0.01f,
                motion = new Vector2(0.0f, -0.5f),
                shakeIntensityChange = -0.005f,
                shakeIntensity = 1f
              });
            this.timePerBobberBob = 1f;
          }
        }
        if ((double) this.timeUntilFishingNibbleDone == -1.0)
          return;
        double nibbleAccumulator = (double) this.fishingNibbleAccumulator;
        timeSpan = time.ElapsedGameTime;
        double milliseconds3 = (double) timeSpan.Milliseconds;
        this.fishingNibbleAccumulator = (float) (nibbleAccumulator + milliseconds3);
        if ((double) this.fishingNibbleAccumulator <= (double) this.timeUntilFishingNibbleDone)
          return;
        this.fishingNibbleAccumulator = 0.0f;
        this.timeUntilFishingNibbleDone = -1f;
        this.isNibbling = false;
        this.timeUntilFishingBite = (float) Game1.random.Next(FishingRod.minFishingBiteTime, FishingRod.maxFishingBiteTime);
      }
      else if (who.usingTool && this.castedButBobberStillInAir)
      {
        Vector2 zero = Vector2.Zero;
        if (Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.moveDownButton) && who.FacingDirection != 2 && who.FacingDirection != 0)
          zero.Y += 4f;
        if (Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.moveRightButton) && who.FacingDirection != 1 && who.FacingDirection != 3)
          zero.X += 2f;
        if (Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.moveUpButton) && who.FacingDirection != 0 && who.FacingDirection != 2)
          zero.Y -= 4f;
        if (Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.moveLeftButton) && who.FacingDirection != 3 && who.FacingDirection != 1)
          zero.X -= 2f;
        this.bobber = this.bobber + zero;
        if (this.animations.Count <= 0)
          return;
        this.animations[0].position += zero;
      }
      else if (this.showingTreasure)
        who.FarmerSprite.setCurrentSingleFrame(0, (short) 32000, false, false);
      else if (this.fishCaught)
      {
        if (!Game1.isFestival())
        {
          who.faceDirection(2);
          who.FarmerSprite.setCurrentFrame(84);
        }
        if (Game1.random.NextDouble() < 0.025)
          who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(653, 858, 1, 1), 9999f, 1, 1, who.position + new Vector2((float) (Game1.random.Next(-3, 2) * 4), (float) (-Game1.tileSize / 2)), false, false, (float) ((double) who.getStandingY() / 10000.0 + 1.0 / 500.0), 0.04f, Color.LightBlue, 5f, 0.0f, 0.0f, 0.0f, false)
          {
            acceleration = new Vector2(0.0f, 0.25f)
          });
        if (Mouse.GetState().LeftButton != ButtonState.Pressed && !Game1.didPlayerJustClickAtAll() && !Game1.isOneOfTheseKeysDown(Game1.oldKBState, Game1.options.useToolButton))
          return;
        Game1.playSound("coin");
        if (!this.treasureCaught)
        {
          this.doneFishing(this.lastUser, true);
          this.lastUser.completelyStopAnimatingOrDoingAction();
          if (Game1.isFestival() || this.lastUser.addItemToInventoryBool((Item) new StardewValley.Object(this.whichFish, 1, false, -1, this.fishQuality), false))
            return;
          Game1.activeClickableMenu = (IClickableMenu) new ItemGrabMenu(new List<Item>()
          {
            (Item) new StardewValley.Object(this.whichFish, 1, false, -1, this.fishQuality)
          });
        }
        else
        {
          this.fishCaught = false;
          this.showingTreasure = true;
          bool inventoryBool = this.lastUser.addItemToInventoryBool((Item) new StardewValley.Object(this.whichFish, 1, false, -1, this.fishQuality), false);
          List<TemporaryAnimatedSprite> animations = this.animations;
          TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(64, 1920, 32, 32), 500f, 1, 0, this.lastUser.position + new Vector2(-32f, -160f), false, false, (float) ((double) this.lastUser.getStandingY() / 10000.0 + 1.0 / 1000.0), 0.0f, Color.White, 4f, 0.0f, 0.0f, 0.0f, false);
          temporaryAnimatedSprite.motion = new Vector2(0.0f, -0.128f);
          int num1 = 1;
          temporaryAnimatedSprite.timeBasedMotion = num1 != 0;
          TemporaryAnimatedSprite.endBehavior endBehavior = new TemporaryAnimatedSprite.endBehavior(this.openChestEndFunction);
          temporaryAnimatedSprite.endFunction = endBehavior;
          int num2 = inventoryBool ? 0 : 1;
          temporaryAnimatedSprite.extraInfoForEndBehavior = num2;
          double num3 = 0.0;
          temporaryAnimatedSprite.alpha = (float) num3;
          double num4 = -1.0 / 500.0;
          temporaryAnimatedSprite.alphaFade = (float) num4;
          animations.Add(temporaryAnimatedSprite);
        }
      }
      else if (who.usingTool && this.castedButBobberStillInAir && this.doneWithAnimation)
      {
        switch (who.FacingDirection)
        {
          case 0:
            who.FarmerSprite.setCurrentFrame(39);
            break;
          case 1:
            who.FarmerSprite.setCurrentFrame(89);
            break;
          case 2:
            who.FarmerSprite.setCurrentFrame(28);
            break;
          case 3:
            who.FarmerSprite.setCurrentFrame(89, 0, 10, 1, true, false);
            break;
        }
        who.armOffset.Y = (float) Math.Round(Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 250.0), 2);
      }
      else
      {
        if (this.castedButBobberStillInAir || this.whichFish == -1 || (this.animations.Count <= 0 || (double) this.animations[0].timer <= 500.0) || Game1.eventUp)
          return;
        this.lastUser.faceDirection(2);
        this.lastUser.FarmerSprite.setCurrentFrame(57);
      }
    }

    public void openChestEndFunction(int extra)
    {
      Game1.playSound("openChest");
      this.animations.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(64, 1920, 32, 32), 200f, 4, 0, this.lastUser.position + new Vector2(-32f, -228f), false, false, (float) ((double) this.lastUser.getStandingY() / 10000.0 + 1.0 / 1000.0), 0.0f, Color.White, 4f, 0.0f, 0.0f, 0.0f, false)
      {
        endFunction = new TemporaryAnimatedSprite.endBehavior(this.openTreasureMenuEndFunction),
        extraInfoForEndBehavior = extra
      });
      this.sparklingText = (SparklingText) null;
    }

    public override bool doesShowTileLocationMarker()
    {
      return false;
    }

    public void openTreasureMenuEndFunction(int extra)
    {
      this.lastUser.gainExperience(5, 10 * (this.clearWaterDistance + 1));
      this.doneFishing(this.lastUser, true);
      this.lastUser.completelyStopAnimatingOrDoingAction();
      List<Item> objList1 = new List<Item>();
      if (extra == 1)
        objList1.Add((Item) new StardewValley.Object(this.whichFish, 1, false, -1, this.fishQuality));
      float num1 = 1f;
      while (Game1.random.NextDouble() <= (double) num1)
      {
        num1 *= 0.4f;
        switch (Game1.random.Next(4))
        {
          case 0:
            if (this.clearWaterDistance >= 5 && Game1.random.NextDouble() < 0.03)
            {
              objList1.Add((Item) new StardewValley.Object(386, Game1.random.Next(1, 3), false, -1, 0));
              continue;
            }
            List<int> source = new List<int>();
            if (this.clearWaterDistance >= 4)
              source.Add(384);
            if (this.clearWaterDistance >= 3 && (source.Count == 0 || Game1.random.NextDouble() < 0.6))
              source.Add(380);
            if (source.Count == 0 || Game1.random.NextDouble() < 0.6)
              source.Add(378);
            if (source.Count == 0 || Game1.random.NextDouble() < 0.6)
              source.Add(388);
            if (source.Count == 0 || Game1.random.NextDouble() < 0.6)
              source.Add(390);
            source.Add(382);
            objList1.Add((Item) new StardewValley.Object(source.ElementAt<int>(Game1.random.Next(source.Count)), Game1.random.Next(2, 7) * (Game1.random.NextDouble() < 0.05 + (double) this.lastUser.luckLevel * 0.015 ? 2 : 1), false, -1, 0));
            if (Game1.random.NextDouble() < 0.05 + (double) this.lastUser.LuckLevel * 0.03)
            {
              objList1.Last<Item>().Stack *= 2;
              continue;
            }
            continue;
          case 1:
            if (this.clearWaterDistance >= 4 && Game1.random.NextDouble() < 0.1 && this.lastUser.FishingLevel >= 6)
            {
              objList1.Add((Item) new StardewValley.Object(687, 1, false, -1, 0));
              continue;
            }
            if (this.lastUser.FishingLevel >= 6)
            {
              objList1.Add((Item) new StardewValley.Object(685, 1, false, -1, 0));
              continue;
            }
            objList1.Add((Item) new StardewValley.Object(685, 10, false, -1, 0));
            continue;
          case 2:
            if (Game1.random.NextDouble() < 0.1 && this.lastUser != null && (this.lastUser.archaeologyFound != null && this.lastUser.archaeologyFound.ContainsKey(102)) && this.lastUser.archaeologyFound[102][0] < 21)
            {
              objList1.Add((Item) new StardewValley.Object(102, 1, false, -1, 0));
              Game1.showGlobalMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingRod.cs.14100"));
              continue;
            }
            if (Game1.player.archaeologyFound.Count > 0)
            {
              if (Game1.random.NextDouble() < 0.25 && this.lastUser.FishingLevel > 1)
              {
                objList1.Add((Item) new StardewValley.Object(Game1.random.Next(585, 588), 1, false, -1, 0));
                continue;
              }
              if (Game1.random.NextDouble() < 0.5 && this.lastUser.FishingLevel > 1)
              {
                objList1.Add((Item) new StardewValley.Object(Game1.random.Next(103, 120), 1, false, -1, 0));
                continue;
              }
              objList1.Add((Item) new StardewValley.Object(535, 1, false, -1, 0));
              continue;
            }
            objList1.Add((Item) new StardewValley.Object(382, Game1.random.Next(1, 3), false, -1, 0));
            continue;
          case 3:
            switch (Game1.random.Next(3))
            {
              case 0:
                if (this.clearWaterDistance >= 4)
                  objList1.Add((Item) new StardewValley.Object(537 + (Game1.random.NextDouble() < 0.4 ? Game1.random.Next(-2, 0) : 0), Game1.random.Next(1, 4), false, -1, 0));
                else if (this.clearWaterDistance >= 3)
                  objList1.Add((Item) new StardewValley.Object(536 + (Game1.random.NextDouble() < 0.4 ? -1 : 0), Game1.random.Next(1, 4), false, -1, 0));
                else
                  objList1.Add((Item) new StardewValley.Object(535, Game1.random.Next(1, 4), false, -1, 0));
                if (Game1.random.NextDouble() < 0.05 + (double) this.lastUser.LuckLevel * 0.03)
                {
                  objList1.Last<Item>().Stack *= 2;
                  continue;
                }
                continue;
              case 1:
                if (this.lastUser.FishingLevel < 2)
                {
                  objList1.Add((Item) new StardewValley.Object(382, Game1.random.Next(1, 4), false, -1, 0));
                  continue;
                }
                if (this.clearWaterDistance >= 4)
                  objList1.Add((Item) new StardewValley.Object(Game1.random.NextDouble() < 0.3 ? 82 : (Game1.random.NextDouble() < 0.5 ? 64 : 60), Game1.random.Next(1, 3), false, -1, 0));
                else if (this.clearWaterDistance >= 3)
                  objList1.Add((Item) new StardewValley.Object(Game1.random.NextDouble() < 0.3 ? 84 : (Game1.random.NextDouble() < 0.5 ? 70 : 62), Game1.random.Next(1, 3), false, -1, 0));
                else
                  objList1.Add((Item) new StardewValley.Object(Game1.random.NextDouble() < 0.3 ? 86 : (Game1.random.NextDouble() < 0.5 ? 66 : 68), Game1.random.Next(1, 3), false, -1, 0));
                if (Game1.random.NextDouble() < 0.028 * ((double) this.clearWaterDistance / 5.0))
                  objList1.Add((Item) new StardewValley.Object(72, 1, false, -1, 0));
                if (Game1.random.NextDouble() < 0.05)
                {
                  objList1.Last<Item>().Stack *= 2;
                  continue;
                }
                continue;
              case 2:
                if (this.lastUser.FishingLevel < 2)
                {
                  objList1.Add((Item) new StardewValley.Object(770, Game1.random.Next(1, 4), false, -1, 0));
                  continue;
                }
                float num2 = (float) ((1.0 + Game1.dailyLuck) * ((double) this.clearWaterDistance / 5.0));
                if (Game1.random.NextDouble() < 0.05 * (double) num2 && !this.lastUser.specialItems.Contains(14))
                {
                  List<Item> objList2 = objList1;
                  MeleeWeapon meleeWeapon = new MeleeWeapon(14);
                  int num3 = 1;
                  meleeWeapon.specialItem = num3 != 0;
                  objList2.Add((Item) meleeWeapon);
                }
                if (Game1.random.NextDouble() < 0.05 * (double) num2 && !this.lastUser.specialItems.Contains(51))
                {
                  List<Item> objList2 = objList1;
                  MeleeWeapon meleeWeapon = new MeleeWeapon(51);
                  int num3 = 1;
                  meleeWeapon.specialItem = num3 != 0;
                  objList2.Add((Item) meleeWeapon);
                }
                if (Game1.random.NextDouble() < 0.07 * (double) num2)
                {
                  switch (Game1.random.Next(3))
                  {
                    case 0:
                      objList1.Add((Item) new Ring(516 + (Game1.random.NextDouble() < (double) this.lastUser.LuckLevel / 11.0 ? 1 : 0)));
                      break;
                    case 1:
                      objList1.Add((Item) new Ring(518 + (Game1.random.NextDouble() < (double) this.lastUser.LuckLevel / 11.0 ? 1 : 0)));
                      break;
                    case 2:
                      objList1.Add((Item) new Ring(Game1.random.Next(529, 535)));
                      break;
                  }
                }
                if (Game1.random.NextDouble() < 0.02 * (double) num2)
                  objList1.Add((Item) new StardewValley.Object(166, 1, false, -1, 0));
                if (this.lastUser.FishingLevel > 5 && Game1.random.NextDouble() < 0.001 * (double) num2)
                  objList1.Add((Item) new StardewValley.Object(74, 1, false, -1, 0));
                if (Game1.random.NextDouble() < 0.01 * (double) num2)
                  objList1.Add((Item) new StardewValley.Object((int) sbyte.MaxValue, 1, false, -1, 0));
                if (Game1.random.NextDouble() < 0.01 * (double) num2)
                  objList1.Add((Item) new StardewValley.Object(126, 1, false, -1, 0));
                if (Game1.random.NextDouble() < 0.01 * (double) num2)
                  objList1.Add((Item) new Ring(527));
                if (Game1.random.NextDouble() < 0.01 * (double) num2)
                  objList1.Add((Item) new Boots(Game1.random.Next(504, 514)));
                if (objList1.Count == 1)
                {
                  objList1.Add((Item) new StardewValley.Object(72, 1, false, -1, 0));
                  continue;
                }
                continue;
              default:
                continue;
            }
          default:
            continue;
        }
      }
      if (objList1.Count == 0)
        objList1.Add((Item) new StardewValley.Object(685, Game1.random.Next(1, 4) * 5, false, -1, 0));
      Game1.activeClickableMenu = (IClickableMenu) new ItemGrabMenu(objList1);
      (Game1.activeClickableMenu as ItemGrabMenu).source = 3;
      this.lastUser.completelyStopAnimatingOrDoingAction();
    }
  }
}
