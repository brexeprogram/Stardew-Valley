// Decompiled with JetBrains decompiler
// Type: StardewValley.TemporaryAnimatedSprite
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley
{
  public class TemporaryAnimatedSprite
  {
    public float interval = 200f;
    public int xStopCoordinate = -1;
    public int yStopCoordinate = -1;
    public int pingPongMotion = 1;
    public bool destroyable = true;
    public float alpha = 1f;
    public float layerDepth = -1f;
    public float scale = 1f;
    public float pulseAmount = 1.1f;
    public Color color = Color.White;
    public Color lightcolor = Color.White;
    public Vector2 motion = Vector2.Zero;
    public Vector2 acceleration = Vector2.Zero;
    public Vector2 accelerationChange = Vector2.Zero;
    public float timer;
    public int currentParentTileIndex;
    public int oldCurrentParentTileIndex;
    public int initialParentTileIndex;
    public int totalNumberOfLoops;
    public int currentNumberOfLoops;
    public int animationLength;
    public int bombRadius;
    public bool flicker;
    public bool timeBasedMotion;
    public bool overrideLocationDestroy;
    public bool pingPong;
    public bool holdLastFrame;
    public bool pulse;
    public int extraInfoForEndBehavior;
    public int lightID;
    public bool bigCraftable;
    public bool swordswipe;
    public bool flash;
    public bool flipped;
    public bool verticalFlipped;
    public bool local;
    public bool light;
    public bool hasLit;
    public bool xPeriodic;
    public bool yPeriodic;
    public bool paused;
    public float rotation;
    public float alphaFade;
    public float scaleChange;
    public float scaleChangeChange;
    public float rotationChange;
    public float id;
    public float lightRadius;
    public float xPeriodicRange;
    public float yPeriodicRange;
    public float xPeriodicLoopTime;
    public float yPeriodicLoopTime;
    public float shakeIntensityChange;
    public float shakeIntensity;
    public float pulseTime;
    public Vector2 position;
    public Vector2 sourceRectStartingPos;
    protected GameLocation parent;
    private Texture2D texture;
    public Rectangle sourceRect;
    protected Farmer owner;
    public Vector2 initialPosition;
    public int delayBeforeAnimationStart;
    public string startSound;
    public string endSound;
    public string text;
    public TemporaryAnimatedSprite.endBehavior endFunction;
    public TemporaryAnimatedSprite.endBehavior reachedStopCoordinate;
    public TemporaryAnimatedSprite parentSprite;
    public Character attachedCharacter;
    private float pulseTimer;
    private float originalScale;
    private float totalTimer;

    public Vector2 Position
    {
      get
      {
        return this.position;
      }
      set
      {
        this.position = value;
      }
    }

    public Texture2D Texture
    {
      get
      {
        return this.texture;
      }
      set
      {
        this.texture = value;
      }
    }

    public TemporaryAnimatedSprite getClone()
    {
      TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite();
      temporaryAnimatedSprite.texture = this.texture;
      temporaryAnimatedSprite.interval = this.interval;
      temporaryAnimatedSprite.currentParentTileIndex = this.currentParentTileIndex;
      temporaryAnimatedSprite.oldCurrentParentTileIndex = this.oldCurrentParentTileIndex;
      temporaryAnimatedSprite.initialParentTileIndex = this.initialParentTileIndex;
      temporaryAnimatedSprite.totalNumberOfLoops = this.totalNumberOfLoops;
      temporaryAnimatedSprite.currentNumberOfLoops = this.currentNumberOfLoops;
      temporaryAnimatedSprite.xStopCoordinate = this.xStopCoordinate;
      temporaryAnimatedSprite.yStopCoordinate = this.yStopCoordinate;
      temporaryAnimatedSprite.animationLength = this.animationLength;
      temporaryAnimatedSprite.bombRadius = this.bombRadius;
      temporaryAnimatedSprite.pingPongMotion = this.pingPongMotion;
      int num1 = this.flicker ? 1 : 0;
      temporaryAnimatedSprite.flicker = num1 != 0;
      int num2 = this.timeBasedMotion ? 1 : 0;
      temporaryAnimatedSprite.timeBasedMotion = num2 != 0;
      int num3 = this.overrideLocationDestroy ? 1 : 0;
      temporaryAnimatedSprite.overrideLocationDestroy = num3 != 0;
      int num4 = this.pingPong ? 1 : 0;
      temporaryAnimatedSprite.pingPong = num4 != 0;
      int num5 = this.holdLastFrame ? 1 : 0;
      temporaryAnimatedSprite.holdLastFrame = num5 != 0;
      int infoForEndBehavior = this.extraInfoForEndBehavior;
      temporaryAnimatedSprite.extraInfoForEndBehavior = infoForEndBehavior;
      int lightId = this.lightID;
      temporaryAnimatedSprite.lightID = lightId;
      Vector2 acceleration = this.acceleration;
      temporaryAnimatedSprite.acceleration = acceleration;
      Vector2 accelerationChange = this.accelerationChange;
      temporaryAnimatedSprite.accelerationChange = accelerationChange;
      double alpha = (double) this.alpha;
      temporaryAnimatedSprite.alpha = (float) alpha;
      double alphaFade = (double) this.alphaFade;
      temporaryAnimatedSprite.alphaFade = (float) alphaFade;
      Character attachedCharacter = this.attachedCharacter;
      temporaryAnimatedSprite.attachedCharacter = attachedCharacter;
      int num6 = this.bigCraftable ? 1 : 0;
      temporaryAnimatedSprite.bigCraftable = num6 != 0;
      Color color = this.color;
      temporaryAnimatedSprite.color = color;
      int beforeAnimationStart = this.delayBeforeAnimationStart;
      temporaryAnimatedSprite.delayBeforeAnimationStart = beforeAnimationStart;
      int num7 = this.destroyable ? 1 : 0;
      temporaryAnimatedSprite.destroyable = num7 != 0;
      TemporaryAnimatedSprite.endBehavior endFunction = this.endFunction;
      temporaryAnimatedSprite.endFunction = endFunction;
      string endSound = this.endSound;
      temporaryAnimatedSprite.endSound = endSound;
      int num8 = this.flash ? 1 : 0;
      temporaryAnimatedSprite.flash = num8 != 0;
      int num9 = this.flipped ? 1 : 0;
      temporaryAnimatedSprite.flipped = num9 != 0;
      int num10 = this.hasLit ? 1 : 0;
      temporaryAnimatedSprite.hasLit = num10 != 0;
      double id = (double) this.id;
      temporaryAnimatedSprite.id = (float) id;
      Vector2 initialPosition = this.initialPosition;
      temporaryAnimatedSprite.initialPosition = initialPosition;
      int num11 = this.light ? 1 : 0;
      temporaryAnimatedSprite.light = num11 != 0;
      int num12 = this.local ? 1 : 0;
      temporaryAnimatedSprite.local = num12 != 0;
      Vector2 motion = this.motion;
      temporaryAnimatedSprite.motion = motion;
      Farmer owner = this.owner;
      temporaryAnimatedSprite.owner = owner;
      GameLocation parent = this.parent;
      temporaryAnimatedSprite.parent = parent;
      TemporaryAnimatedSprite parentSprite = this.parentSprite;
      temporaryAnimatedSprite.parentSprite = parentSprite;
      Vector2 position = this.position;
      temporaryAnimatedSprite.position = position;
      double rotation = (double) this.rotation;
      temporaryAnimatedSprite.rotation = (float) rotation;
      double rotationChange = (double) this.rotationChange;
      temporaryAnimatedSprite.rotationChange = (float) rotationChange;
      double scale = (double) this.scale;
      temporaryAnimatedSprite.scale = (float) scale;
      double scaleChange = (double) this.scaleChange;
      temporaryAnimatedSprite.scaleChange = (float) scaleChange;
      double scaleChangeChange = (double) this.scaleChangeChange;
      temporaryAnimatedSprite.scaleChangeChange = (float) scaleChangeChange;
      double shakeIntensity = (double) this.shakeIntensity;
      temporaryAnimatedSprite.shakeIntensity = (float) shakeIntensity;
      double shakeIntensityChange = (double) this.shakeIntensityChange;
      temporaryAnimatedSprite.shakeIntensityChange = (float) shakeIntensityChange;
      Rectangle sourceRect = this.sourceRect;
      temporaryAnimatedSprite.sourceRect = sourceRect;
      Vector2 sourceRectStartingPos = this.sourceRectStartingPos;
      temporaryAnimatedSprite.sourceRectStartingPos = sourceRectStartingPos;
      string startSound = this.startSound;
      temporaryAnimatedSprite.startSound = startSound;
      int num13 = this.timeBasedMotion ? 1 : 0;
      temporaryAnimatedSprite.timeBasedMotion = num13 != 0;
      int num14 = this.verticalFlipped ? 1 : 0;
      temporaryAnimatedSprite.verticalFlipped = num14 != 0;
      int num15 = this.xPeriodic ? 1 : 0;
      temporaryAnimatedSprite.xPeriodic = num15 != 0;
      double periodicLoopTime1 = (double) this.xPeriodicLoopTime;
      temporaryAnimatedSprite.xPeriodicLoopTime = (float) periodicLoopTime1;
      double xPeriodicRange = (double) this.xPeriodicRange;
      temporaryAnimatedSprite.xPeriodicRange = (float) xPeriodicRange;
      int num16 = this.yPeriodic ? 1 : 0;
      temporaryAnimatedSprite.yPeriodic = num16 != 0;
      double periodicLoopTime2 = (double) this.yPeriodicLoopTime;
      temporaryAnimatedSprite.yPeriodicLoopTime = (float) periodicLoopTime2;
      double yPeriodicRange = (double) this.yPeriodicRange;
      temporaryAnimatedSprite.yPeriodicRange = (float) yPeriodicRange;
      int yStopCoordinate = this.yStopCoordinate;
      temporaryAnimatedSprite.yStopCoordinate = yStopCoordinate;
      int totalNumberOfLoops = this.totalNumberOfLoops;
      temporaryAnimatedSprite.totalNumberOfLoops = totalNumberOfLoops;
      return temporaryAnimatedSprite;
    }

    public static void addMoneyToFarmerEndBehavior(int extraInfo)
    {
      Game1.player.money += extraInfo;
    }

    public TemporaryAnimatedSprite()
    {
    }

    public TemporaryAnimatedSprite(int initialParentTileIndex, float animationInterval, int animationLength, int numberOfLoops, Vector2 position, bool flicker, bool flipped)
    {
      if (initialParentTileIndex == -1)
      {
        this.swordswipe = true;
        this.currentParentTileIndex = 0;
      }
      else
        this.currentParentTileIndex = initialParentTileIndex;
      this.initialParentTileIndex = initialParentTileIndex;
      this.interval = animationInterval;
      this.totalNumberOfLoops = numberOfLoops;
      this.position = position;
      this.animationLength = animationLength;
      this.flicker = flicker;
      this.flipped = flipped;
    }

    public TemporaryAnimatedSprite(int rowInAnimationTexture, Vector2 position, Color color, int animationLength = 8, bool flipped = false, float animationInterval = 100f, int numberOfLoops = 0, int sourceRectWidth = -1, float layerDepth = -1f, int sourceRectHeight = -1, int delay = 0)
      : this(Game1.animations, new Rectangle(0, rowInAnimationTexture * Game1.tileSize, sourceRectWidth, sourceRectHeight), animationInterval, animationLength, numberOfLoops, position, false, flipped, layerDepth, 0.0f, color, 1f, 0.0f, 0.0f, 0.0f, false)
    {
      if (sourceRectWidth == -1)
      {
        sourceRectWidth = Game1.tileSize;
        this.sourceRect.Width = Game1.tileSize;
      }
      if (sourceRectHeight == -1)
      {
        sourceRectHeight = Game1.tileSize;
        this.sourceRect.Height = Game1.tileSize;
      }
      if ((double) layerDepth == -1.0)
        layerDepth = (float) (((double) position.Y + (double) (Game1.tileSize / 2)) / 10000.0);
      this.delayBeforeAnimationStart = delay;
    }

    public TemporaryAnimatedSprite(int initialParentTileIndex, float animationInterval, int animationLength, int numberOfLoops, Vector2 position, bool flicker, bool flipped, bool verticalFlipped, float rotation)
      : this(initialParentTileIndex, animationInterval, animationLength, numberOfLoops, position, flicker, flipped)
    {
      this.rotation = rotation;
      this.verticalFlipped = verticalFlipped;
    }

    public TemporaryAnimatedSprite(int initialParentTileIndex, float animationInterval, int animationLength, int numberOfLoops, Vector2 position, bool flicker, bool bigCraftable, bool flipped)
      : this(initialParentTileIndex, animationInterval, animationLength, numberOfLoops, position, flicker, flipped)
    {
      this.bigCraftable = bigCraftable;
      if (!bigCraftable)
        return;
      this.position.Y -= (float) Game1.tileSize;
    }

    public TemporaryAnimatedSprite(Texture2D texture, Rectangle sourceRect, float animationInterval, int animationLength, int numberOfLoops, Vector2 position, bool flicker, bool flipped)
      : this(0, animationInterval, animationLength, numberOfLoops, position, flicker, flipped)
    {
      this.Texture = texture;
      this.sourceRect = sourceRect;
      this.sourceRectStartingPos = new Vector2((float) sourceRect.X, (float) sourceRect.Y);
      this.initialPosition = position;
    }

    public TemporaryAnimatedSprite(Texture2D texture, Rectangle sourceRect, float animationInterval, int animationLength, int numberOfLoops, Vector2 position, bool flicker, bool flipped, float layerDepth, float alphaFade, Color color, float scale, float scaleChange, float rotation, float rotationChange, bool local = false)
      : this(0, animationInterval, animationLength, numberOfLoops, position, flicker, flipped)
    {
      this.Texture = texture;
      this.sourceRect = sourceRect;
      this.sourceRectStartingPos = new Vector2((float) sourceRect.X, (float) sourceRect.Y);
      this.layerDepth = layerDepth;
      this.alphaFade = Math.Max(0.0f, alphaFade);
      this.color = color;
      this.scale = scale;
      this.scaleChange = scaleChange;
      this.rotation = rotation;
      this.rotationChange = rotationChange;
      this.local = local;
      this.initialPosition = position;
    }

    public TemporaryAnimatedSprite(Texture2D texture, Rectangle sourceRect, Vector2 position, bool flipped, float alphaFade, Color color)
      : this(0, 999999f, 1, 0, position, false, flipped)
    {
      this.Texture = texture;
      this.sourceRect = sourceRect;
      this.sourceRectStartingPos = new Vector2((float) sourceRect.X, (float) sourceRect.Y);
      this.initialPosition = position;
      this.alphaFade = Math.Max(0.0f, alphaFade);
      this.color = color;
    }

    public TemporaryAnimatedSprite(int initialParentTileIndex, float animationInterval, int animationLength, int numberOfLoops, Vector2 position, bool flicker, bool flipped, GameLocation parent, Farmer owner)
      : this(initialParentTileIndex, animationInterval, animationLength, numberOfLoops, position, flicker, flipped)
    {
      this.position.X = (float) (int) this.position.X;
      this.position.Y = (float) (int) this.position.Y;
      this.parent = parent;
      switch (initialParentTileIndex)
      {
        case 286:
          this.bombRadius = 3;
          break;
        case 287:
          this.bombRadius = 5;
          break;
        case 288:
          this.bombRadius = 7;
          break;
      }
      this.owner = owner;
    }

    public virtual void draw(SpriteBatch spriteBatch, bool localPosition = false, int xOffset = 0, int yOffset = 0)
    {
      if (this.local)
        localPosition = true;
      if (this.currentParentTileIndex < 0 || this.delayBeforeAnimationStart > 0)
        return;
      if (this.text != null)
        spriteBatch.DrawString(Game1.dialogueFont, this.text, localPosition ? this.Position : Game1.GlobalToLocal(Game1.viewport, this.Position), this.color * this.alpha, this.rotation, Vector2.Zero, this.scale, SpriteEffects.None, this.layerDepth);
      else if (this.Texture != null)
        spriteBatch.Draw(this.Texture, (localPosition ? this.Position : Game1.GlobalToLocal(Game1.viewport, new Vector2((float) ((int) this.Position.X + xOffset), (float) ((int) this.Position.Y + yOffset)))) + new Vector2((float) (this.sourceRect.Width / 2), (float) (this.sourceRect.Height / 2)) * this.scale + new Vector2((double) this.shakeIntensity > 0.0 ? (float) Game1.random.Next(-(int) this.shakeIntensity, (int) this.shakeIntensity + 1) : 0.0f, (double) this.shakeIntensity > 0.0 ? (float) Game1.random.Next(-(int) this.shakeIntensity, (int) this.shakeIntensity + 1) : 0.0f), new Rectangle?(this.sourceRect), this.color * this.alpha, this.rotation, new Vector2((float) (this.sourceRect.Width / 2), (float) (this.sourceRect.Height / 2)), this.scale, this.flipped ? SpriteEffects.FlipHorizontally : (this.verticalFlipped ? SpriteEffects.FlipVertically : SpriteEffects.None), (double) this.layerDepth >= 0.0 ? this.layerDepth : (float) (((double) this.Position.Y + (double) this.sourceRect.Height) / 10000.0));
      else if (this.bigCraftable)
      {
        spriteBatch.Draw(Game1.bigCraftableSpriteSheet, localPosition ? this.Position : Game1.GlobalToLocal(Game1.viewport, new Vector2((float) ((int) this.Position.X + xOffset), (float) ((int) this.Position.Y + yOffset))) + new Vector2((float) (this.sourceRect.Width / 2), (float) (this.sourceRect.Height / 2)), new Rectangle?(Object.getSourceRectForBigCraftable(this.currentParentTileIndex)), Color.White, 0.0f, new Vector2((float) (this.sourceRect.Width / 2), (float) (this.sourceRect.Height / 2)), this.scale, SpriteEffects.None, (float) (((double) this.Position.Y + (double) (Game1.tileSize / 2)) / 10000.0));
      }
      else
      {
        if (this.swordswipe)
          return;
        spriteBatch.Draw(Game1.objectSpriteSheet, localPosition ? this.Position : Game1.GlobalToLocal(Game1.viewport, new Vector2((float) ((int) this.Position.X + xOffset), (float) ((int) this.Position.Y + yOffset))) + new Vector2(8f, 8f) * (float) Game1.pixelZoom + new Vector2((double) this.shakeIntensity > 0.0 ? (float) Game1.random.Next(-(int) this.shakeIntensity, (int) this.shakeIntensity + 1) : 0.0f, (double) this.shakeIntensity > 0.0 ? (float) Game1.random.Next(-(int) this.shakeIntensity, (int) this.shakeIntensity + 1) : 0.0f), new Rectangle?(Game1.currentLocation.getSourceRectForObject(this.currentParentTileIndex)), (this.flash ? Color.LightBlue * 0.85f : Color.White) * this.alpha, this.rotation, new Vector2(8f, 8f), (float) Game1.pixelZoom * this.scale, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (double) this.layerDepth >= 0.0 ? this.layerDepth : (float) (((double) this.Position.Y + (double) (Game1.tileSize / 2)) / 10000.0));
      }
    }

    public void unload()
    {
      if (this.endSound != null)
        Game1.playSound(this.endSound);
      if (this.endFunction != null)
        this.endFunction(this.extraInfoForEndBehavior);
      if (!this.hasLit)
        return;
      Utility.removeLightSource(this.lightID);
    }

    public void reset()
    {
      this.sourceRect.X = (int) this.sourceRectStartingPos.X;
      this.sourceRect.Y = (int) this.sourceRectStartingPos.Y;
      this.currentParentTileIndex = 0;
      this.oldCurrentParentTileIndex = 0;
      this.timer = 0.0f;
      this.totalTimer = 0.0f;
      this.currentNumberOfLoops = 0;
      this.pingPongMotion = 1;
    }

    public virtual bool update(GameTime time)
    {
      if (this.paused || this.bombRadius > 0 && Game1.activeClickableMenu != null)
        return false;
      if (this.delayBeforeAnimationStart > 0)
      {
        this.delayBeforeAnimationStart = this.delayBeforeAnimationStart - time.ElapsedGameTime.Milliseconds;
        if (this.delayBeforeAnimationStart <= 0 && this.startSound != null)
          Game1.playSound(this.startSound);
        if (this.delayBeforeAnimationStart <= 0 && this.parentSprite != null)
          this.position = this.parentSprite.position + this.position;
        return false;
      }
      this.timer = this.timer + (float) time.ElapsedGameTime.Milliseconds;
      this.totalTimer = this.totalTimer + (float) time.ElapsedGameTime.Milliseconds;
      this.alpha = this.alpha - this.alphaFade * (this.timeBasedMotion ? (float) time.ElapsedGameTime.Milliseconds : 1f);
      if ((double) this.alphaFade > 0.0 && this.light && (double) this.alpha < 1.0)
      {
        if ((double) this.alpha >= 0.0)
        {
          try
          {
            Utility.getLightSource(this.lightID).color.A = (byte) ((double) byte.MaxValue * (double) this.alpha);
          }
          catch (Exception ex)
          {
          }
        }
      }
      double shakeIntensity = (double) this.shakeIntensity;
      double shakeIntensityChange = (double) this.shakeIntensityChange;
      TimeSpan elapsedGameTime = time.ElapsedGameTime;
      double milliseconds1 = (double) elapsedGameTime.Milliseconds;
      double num1 = shakeIntensityChange * milliseconds1;
      this.shakeIntensity = (float) (shakeIntensity + num1);
      double scale1 = (double) this.scale;
      double scaleChange1 = (double) this.scaleChange;
      int num2;
      if (!this.timeBasedMotion)
      {
        num2 = 1;
      }
      else
      {
        elapsedGameTime = time.ElapsedGameTime;
        num2 = elapsedGameTime.Milliseconds;
      }
      double num3 = (double) num2;
      double num4 = scaleChange1 * num3;
      this.scale = (float) (scale1 + num4);
      double scaleChange2 = (double) this.scaleChange;
      double scaleChangeChange = (double) this.scaleChangeChange;
      int num5;
      if (!this.timeBasedMotion)
      {
        num5 = 1;
      }
      else
      {
        elapsedGameTime = time.ElapsedGameTime;
        num5 = elapsedGameTime.Milliseconds;
      }
      double num6 = (double) num5;
      double num7 = scaleChangeChange * num6;
      this.scaleChange = (float) (scaleChange2 + num7);
      this.rotation = this.rotation + this.rotationChange;
      if (this.xPeriodic)
      {
        this.position.X = this.initialPosition.X + this.xPeriodicRange * (float) Math.Sin(2.0 * Math.PI / (double) this.xPeriodicLoopTime * (double) this.totalTimer);
      }
      else
      {
        // ISSUE: explicit reference operation
        // ISSUE: variable of a reference type
        float& local = @this.position.X;
        // ISSUE: explicit reference operation
        double num8 = (double) ^local;
        double x = (double) this.motion.X;
        int num9;
        if (!this.timeBasedMotion)
        {
          num9 = 1;
        }
        else
        {
          elapsedGameTime = time.ElapsedGameTime;
          num9 = elapsedGameTime.Milliseconds;
        }
        double num10 = (double) num9;
        double num11 = x * num10;
        double num12 = num8 + num11;
        // ISSUE: explicit reference operation
        ^local = (float) num12;
      }
      if (this.yPeriodic)
      {
        this.position.Y = this.initialPosition.Y + this.yPeriodicRange * (float) Math.Sin(2.0 * Math.PI / (double) this.yPeriodicLoopTime * (double) (this.totalTimer + this.yPeriodicLoopTime / 2f));
      }
      else
      {
        // ISSUE: explicit reference operation
        // ISSUE: variable of a reference type
        float& local = @this.position.Y;
        // ISSUE: explicit reference operation
        double num8 = (double) ^local;
        double y = (double) this.motion.Y;
        int num9;
        if (!this.timeBasedMotion)
        {
          num9 = 1;
        }
        else
        {
          elapsedGameTime = time.ElapsedGameTime;
          num9 = elapsedGameTime.Milliseconds;
        }
        double num10 = (double) num9;
        double num11 = y * num10;
        double num12 = num8 + num11;
        // ISSUE: explicit reference operation
        ^local = (float) num12;
      }
      if (this.attachedCharacter != null)
      {
        if (this.xPeriodic)
        {
          this.attachedCharacter.position.X = this.initialPosition.X + this.xPeriodicRange * (float) Math.Sin(2.0 * Math.PI / (double) this.xPeriodicLoopTime * (double) this.totalTimer);
        }
        else
        {
          // ISSUE: explicit reference operation
          // ISSUE: variable of a reference type
          float& local = @this.attachedCharacter.position.X;
          // ISSUE: explicit reference operation
          double num8 = (double) ^local;
          double x = (double) this.motion.X;
          int num9;
          if (!this.timeBasedMotion)
          {
            num9 = 1;
          }
          else
          {
            elapsedGameTime = time.ElapsedGameTime;
            num9 = elapsedGameTime.Milliseconds;
          }
          double num10 = (double) num9;
          double num11 = x * num10;
          double num12 = num8 + num11;
          // ISSUE: explicit reference operation
          ^local = (float) num12;
        }
        if (this.yPeriodic)
        {
          this.attachedCharacter.position.Y = this.initialPosition.Y + this.yPeriodicRange * (float) Math.Sin(2.0 * Math.PI / (double) this.yPeriodicLoopTime * (double) this.totalTimer);
        }
        else
        {
          // ISSUE: explicit reference operation
          // ISSUE: variable of a reference type
          float& local = @this.attachedCharacter.position.Y;
          // ISSUE: explicit reference operation
          double num8 = (double) ^local;
          double y = (double) this.motion.Y;
          int num9;
          if (!this.timeBasedMotion)
          {
            num9 = 1;
          }
          else
          {
            elapsedGameTime = time.ElapsedGameTime;
            num9 = elapsedGameTime.Milliseconds;
          }
          double num10 = (double) num9;
          double num11 = y * num10;
          double num12 = num8 + num11;
          // ISSUE: explicit reference operation
          ^local = (float) num12;
        }
      }
      // ISSUE: explicit reference operation
      // ISSUE: variable of a reference type
      float& local1 = @this.motion.X;
      // ISSUE: explicit reference operation
      double num13 = (double) ^local1;
      double x1 = (double) this.acceleration.X;
      int num14;
      if (!this.timeBasedMotion)
      {
        num14 = 1;
      }
      else
      {
        elapsedGameTime = time.ElapsedGameTime;
        num14 = elapsedGameTime.Milliseconds;
      }
      double num15 = (double) num14;
      double num16 = x1 * num15;
      double num17 = num13 + num16;
      // ISSUE: explicit reference operation
      ^local1 = (float) num17;
      // ISSUE: explicit reference operation
      // ISSUE: variable of a reference type
      float& local2 = @this.motion.Y;
      // ISSUE: explicit reference operation
      double num18 = (double) ^local2;
      double y1 = (double) this.acceleration.Y;
      int num19;
      if (!this.timeBasedMotion)
      {
        num19 = 1;
      }
      else
      {
        elapsedGameTime = time.ElapsedGameTime;
        num19 = elapsedGameTime.Milliseconds;
      }
      double num20 = (double) num19;
      double num21 = y1 * num20;
      double num22 = num18 + num21;
      // ISSUE: explicit reference operation
      ^local2 = (float) num22;
      this.acceleration.X += this.accelerationChange.X;
      this.acceleration.Y += this.accelerationChange.Y;
      if (this.xStopCoordinate != -1 || this.yStopCoordinate != -1)
      {
        if (this.xStopCoordinate != -1 && (double) Math.Abs(this.position.X - (float) this.xStopCoordinate) <= (double) Math.Abs(this.motion.X))
        {
          this.motion.X = 0.0f;
          this.acceleration.X = 0.0f;
          this.xStopCoordinate = -1;
        }
        if (this.yStopCoordinate != -1 && (double) Math.Abs(this.position.Y - (float) this.yStopCoordinate) <= (double) Math.Abs(this.motion.Y))
        {
          this.motion.Y = 0.0f;
          this.acceleration.Y = 0.0f;
          this.yStopCoordinate = -1;
        }
        if (this.xStopCoordinate == -1 && this.yStopCoordinate == -1)
        {
          this.rotationChange = 0.0f;
          if (this.reachedStopCoordinate != null)
            this.reachedStopCoordinate(0);
        }
      }
      if (!this.pingPong)
        this.pingPongMotion = 1;
      if (this.pulse)
      {
        double pulseTimer = (double) this.pulseTimer;
        elapsedGameTime = time.ElapsedGameTime;
        double milliseconds2 = (double) elapsedGameTime.Milliseconds;
        this.pulseTimer = (float) (pulseTimer - milliseconds2);
        if ((double) this.originalScale == 0.0)
          this.originalScale = this.scale;
        if ((double) this.pulseTimer <= 0.0)
        {
          this.pulseTimer = this.pulseTime;
          this.scale = this.originalScale * this.pulseAmount;
        }
        if ((double) this.scale > (double) this.originalScale)
        {
          double scale2 = (double) this.scale;
          double num8 = (double) this.pulseAmount / 100.0;
          elapsedGameTime = time.ElapsedGameTime;
          double milliseconds3 = (double) elapsedGameTime.Milliseconds;
          double num9 = num8 * milliseconds3;
          this.scale = (float) (scale2 - num9);
        }
      }
      if (this.light)
      {
        if (!this.hasLit)
        {
          this.hasLit = true;
          this.lightID = Game1.random.Next(int.MinValue, int.MaxValue);
          Game1.currentLightSources.Add(new LightSource(4, this.position + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2)), this.lightRadius, this.lightcolor.Equals(Color.White) ? new Color(0, 131, (int) byte.MaxValue) : this.lightcolor, this.lightID));
        }
        else
          Utility.repositionLightSource(this.lightID, this.position + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2)));
      }
      if ((double) this.alpha <= 0.0 || (double) this.position.X < -2000.0 && !this.overrideLocationDestroy || (double) this.scale <= 0.0)
      {
        this.unload();
        return this.destroyable;
      }
      if ((double) this.timer > (double) this.interval)
      {
        this.currentParentTileIndex = this.currentParentTileIndex + this.pingPongMotion;
        this.sourceRect.X += this.sourceRect.Width * this.pingPongMotion;
        if (this.Texture != null)
        {
          if (!this.pingPong && this.sourceRect.X >= this.Texture.Width)
            this.sourceRect.Y += this.sourceRect.Height;
          if (!this.pingPong)
            this.sourceRect.X %= this.Texture.Width;
          if (this.pingPong)
          {
            if ((double) this.sourceRect.X + ((double) this.sourceRect.Y - (double) this.sourceRectStartingPos.Y) / (double) this.sourceRect.Height * (double) this.Texture.Width >= (double) this.sourceRectStartingPos.X + (double) (this.sourceRect.Width * this.animationLength))
            {
              this.pingPongMotion = -1;
              this.sourceRect.X -= this.sourceRect.Width * 2;
              this.currentParentTileIndex = this.currentParentTileIndex - 1;
              if (this.sourceRect.X < 0)
                this.sourceRect.X = this.Texture.Width + this.sourceRect.X;
            }
            else if ((double) this.sourceRect.X < (double) this.sourceRectStartingPos.X && (double) this.sourceRect.Y == (double) this.sourceRectStartingPos.Y)
            {
              this.pingPongMotion = 1;
              this.sourceRect.X = (int) this.sourceRectStartingPos.X + this.sourceRect.Width;
              this.currentParentTileIndex = this.currentParentTileIndex + 1;
              this.currentNumberOfLoops = this.currentNumberOfLoops + 1;
              if (this.endFunction != null)
              {
                this.endFunction(this.extraInfoForEndBehavior);
                this.endFunction = (TemporaryAnimatedSprite.endBehavior) null;
              }
              if (this.currentNumberOfLoops >= this.totalNumberOfLoops)
              {
                this.unload();
                return this.destroyable;
              }
            }
          }
          else if (this.totalNumberOfLoops >= 1 && (double) this.sourceRect.X + ((double) this.sourceRect.Y - (double) this.sourceRectStartingPos.Y) / (double) this.sourceRect.Height * (double) this.Texture.Width >= (double) this.sourceRectStartingPos.X + (double) (this.sourceRect.Width * this.animationLength))
          {
            this.sourceRect.X = (int) this.sourceRectStartingPos.X;
            this.sourceRect.Y = (int) this.sourceRectStartingPos.Y;
          }
        }
        this.timer = 0.0f;
        if (this.flicker)
        {
          if (this.currentParentTileIndex < 0 || this.flash)
          {
            this.currentParentTileIndex = this.oldCurrentParentTileIndex;
            this.flash = false;
          }
          else
          {
            this.oldCurrentParentTileIndex = this.currentParentTileIndex;
            if (this.bombRadius > 0)
              this.flash = true;
            else
              this.currentParentTileIndex = -100;
          }
        }
        if (this.currentParentTileIndex - this.initialParentTileIndex >= this.animationLength)
        {
          this.currentNumberOfLoops = this.currentNumberOfLoops + 1;
          if (this.holdLastFrame)
          {
            this.currentParentTileIndex = this.initialParentTileIndex + this.animationLength - 1;
            this.setSourceRectToCurrentTileIndex();
            if (this.endFunction != null)
            {
              this.endFunction(this.extraInfoForEndBehavior);
              this.endFunction = (TemporaryAnimatedSprite.endBehavior) null;
            }
            return false;
          }
          this.currentParentTileIndex = this.initialParentTileIndex;
          if (this.currentNumberOfLoops >= this.totalNumberOfLoops)
          {
            if (this.bombRadius > 0)
            {
              if (Game1.fuseSound != null)
              {
                Game1.fuseSound.Stop(AudioStopOptions.AsAuthored);
                Game1.fuseSound = Game1.soundBank.GetCue("fuse");
              }
              Game1.playSound("explosion");
              Game1.flashAlpha = 1f;
              this.parent.explode(new Vector2((float) (int) ((double) this.position.X / (double) Game1.tileSize), (float) (int) ((double) this.position.Y / (double) Game1.tileSize)), this.bombRadius, this.owner);
            }
            this.unload();
            return this.destroyable;
          }
          if (this.bombRadius > 0 && this.currentNumberOfLoops == this.totalNumberOfLoops - 5)
            this.interval = this.interval - this.interval / 3f;
        }
      }
      return false;
    }

    private void setSourceRectToCurrentTileIndex()
    {
      this.sourceRect.X = (int) ((double) this.sourceRectStartingPos.X + (double) (this.currentParentTileIndex * this.sourceRect.Width)) % this.texture.Width;
      if (this.sourceRect.X < 0)
        this.sourceRect.X = 0;
      this.sourceRect.Y = (int) this.sourceRectStartingPos.Y;
    }

    public delegate void endBehavior(int extraInfo);
  }
}
