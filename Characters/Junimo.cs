// Decompiled with JetBrains decompiler
// Type: StardewValley.Characters.Junimo
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using StardewValley.Locations;
using System;
using System.Collections.Generic;

namespace StardewValley.Characters
{
  public class Junimo : NPC
  {
    private float alpha = 1f;
    private int farmerCloseCheckTimer = 100;
    private Vector2 motion = Vector2.Zero;
    private float alphaChange;
    public int whichArea;
    public bool friendly;
    public bool holdingStar;
    public bool holdingBundle;
    public bool temporaryJunimo;
    public bool stayPut;
    public new bool eventActor;
    private Rectangle nextPosition;
    private Color color;
    private Color bundleColor;
    private static int soundTimer;
    private bool sayingGoodbye;

    public Junimo()
    {
    }

    public Junimo(Vector2 position, int whichArea, bool temporary = false)
      : base(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Junimo"), 0, 16, 16), position, 2, nameof (Junimo), (LocalizedContentManager) null)
    {
      this.whichArea = whichArea;
      try
      {
        this.friendly = ((CommunityCenter) Game1.getLocationFromName("CommunityCenter")).areasComplete[whichArea];
      }
      catch (Exception ex)
      {
        this.friendly = true;
      }
      this.temporaryJunimo = temporary;
      this.nextPosition = this.GetBoundingBox();
      this.breather = false;
      this.speed = 3;
      this.forceUpdateTimer = 9999;
      this.collidesWithOtherCharacters = true;
      this.farmerPassesThrough = true;
      this.scale = 0.75f;
      if (this.temporaryJunimo)
      {
        if (Game1.random.NextDouble() < 0.01)
        {
          switch (Game1.random.Next(8))
          {
            case 0:
              this.color = Color.Red;
              break;
            case 1:
              this.color = Color.Goldenrod;
              break;
            case 2:
              this.color = Color.Yellow;
              break;
            case 3:
              this.color = Color.Lime;
              break;
            case 4:
              this.color = new Color(0, (int) byte.MaxValue, 180);
              break;
            case 5:
              this.color = new Color(0, 100, (int) byte.MaxValue);
              break;
            case 6:
              this.color = Color.MediumPurple;
              break;
            case 7:
              this.color = Color.Salmon;
              break;
          }
          if (Game1.random.NextDouble() >= 0.01)
            return;
          this.color = Color.White;
        }
        else
        {
          switch (Game1.random.Next(8))
          {
            case 0:
              this.color = Color.LimeGreen;
              break;
            case 1:
              this.color = Color.Orange;
              break;
            case 2:
              this.color = Color.LightGreen;
              break;
            case 3:
              this.color = Color.Tan;
              break;
            case 4:
              this.color = Color.GreenYellow;
              break;
            case 5:
              this.color = Color.LawnGreen;
              break;
            case 6:
              this.color = Color.PaleGreen;
              break;
            case 7:
              this.color = Color.Turquoise;
              break;
          }
        }
      }
      else
      {
        switch (whichArea)
        {
          case -1:
          case 0:
            this.color = Color.LimeGreen;
            break;
          case 1:
            this.color = Color.Orange;
            break;
          case 2:
            this.color = Color.Turquoise;
            break;
          case 3:
            this.color = Color.Tan;
            break;
          case 4:
            this.color = Color.Gold;
            break;
          case 5:
            this.color = Color.BlanchedAlmond;
            break;
        }
      }
    }

    public override bool canPassThroughActionTiles()
    {
      return false;
    }

    public override bool shouldCollideWithBuildingLayer(GameLocation location)
    {
      return true;
    }

    public void fadeAway()
    {
      this.collidesWithOtherCharacters = false;
      this.alphaChange = this.stayPut ? -0.005f : -0.015f;
    }

    public void setAlpha(float a)
    {
      this.alpha = a;
    }

    public void fadeBack()
    {
      this.alpha = 0.0f;
      this.alphaChange = 0.02f;
      this.isInvisible = false;
    }

    public void setMoving(int xSpeed, int ySpeed)
    {
      this.motion.X = (float) xSpeed;
      this.motion.Y = (float) ySpeed;
    }

    public void setMoving(Vector2 motion)
    {
      this.motion = motion;
    }

    public override void Halt()
    {
      base.Halt();
      this.motion = Vector2.Zero;
    }

    public void returnToJunimoHut(GameLocation location)
    {
      this.jump();
      this.collidesWithOtherCharacters = false;
      this.controller = new PathFindController((Character) this, location, new Point(25, 10), 0, new PathFindController.endBehavior(this.junimoReachedHut));
      Game1.playSound("junimoMeep1");
    }

    public void stayStill()
    {
      this.stayPut = true;
      this.motion = Vector2.Zero;
    }

    public void allowToMoveAgain()
    {
      this.stayPut = false;
    }

    public void returnToJunimoHutToFetchStar(GameLocation location)
    {
      this.friendly = true;
      if (((CommunityCenter) Game1.getLocationFromName("CommunityCenter")).areAllAreasComplete())
      {
        Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.finalCutscene), 0.005f);
        Game1.freezeControls = true;
        this.collidesWithOtherCharacters = false;
        this.farmerPassesThrough = false;
        this.stayStill();
        this.faceDirection(0);
        GameLocation locationFromName = Game1.getLocationFromName("CommunityCenter");
        if (!Game1.player.mailReceived.Contains("ccIsComplete"))
          Game1.player.mailReceived.Add("ccIsComplete");
        if (!Game1.currentLocation.Equals((object) locationFromName))
          return;
        Game1.flashAlpha = 1f;
        (locationFromName as CommunityCenter).addStarToPlaque();
      }
      else
      {
        this.fadeBack();
        DelayedAction.textAboveHeadAfterDelay(Game1.random.NextDouble() < 0.5 ? Game1.content.LoadString("Strings\\Characters:JunimoTextAboveHead1") : Game1.content.LoadString("Strings\\Characters:JunimoTextAboveHead2"), (NPC) this, Game1.random.Next(3000, 6000));
        this.controller = new PathFindController((Character) this, location, new Point(25, 10), 0, new PathFindController.endBehavior(this.junimoReachedHutToFetchStar));
        Game1.playSound("junimoMeep1");
        this.collidesWithOtherCharacters = false;
        this.farmerPassesThrough = false;
        this.holdingBundle = true;
      }
    }

    private void finalCutscene()
    {
      this.collidesWithOtherCharacters = false;
      this.farmerPassesThrough = false;
      Game1.player.position = new Vector2(29f, 11f) * (float) Game1.tileSize;
      Game1.player.completelyStopAnimatingOrDoingAction();
      Game1.player.faceDirection(3);
      Game1.UpdateViewPort(true, new Point(Game1.player.getStandingX(), Game1.player.getStandingY()));
      Game1.viewport.X = Game1.player.getStandingX() - Game1.viewport.Width / 2;
      Game1.viewport.Y = Game1.player.getStandingY() - Game1.viewport.Height / 2;
      Game1.viewportTarget = Vector2.Zero;
      Game1.viewportCenter = new Point(Game1.player.getStandingX(), Game1.player.getStandingY());
      Game1.moveViewportTo(new Vector2(32.5f, 6f) * (float) Game1.tileSize, 2f, 999999, (Game1.afterFadeFunction) null, (Game1.afterFadeFunction) null);
      Game1.globalFadeToClear(new Game1.afterFadeFunction(this.goodbyeDance), 0.005f);
      Game1.pauseTime = 1000f;
      Game1.freezeControls = true;
    }

    public void bringBundleBackToHut(Color bundleColor, GameLocation location)
    {
      if (this.holdingBundle)
        return;
      this.position = Utility.getRandomAdjacentOpenTile(Game1.player.getTileLocation()) * (float) Game1.tileSize;
      int num;
      for (num = 0; location.isCollidingPosition(this.GetBoundingBox(), Game1.viewport, (Character) this) && num < 5; ++num)
        this.position = Utility.getRandomAdjacentOpenTile(Game1.player.getTileLocation()) * (float) Game1.tileSize;
      if (num >= 5)
        return;
      if (Game1.random.NextDouble() < 0.25)
        DelayedAction.textAboveHeadAfterDelay(Game1.random.NextDouble() < 0.5 ? Game1.content.LoadString("Strings\\Characters:JunimoThankYou1") : Game1.content.LoadString("Strings\\Characters:JunimoThankYou2"), (NPC) this, Game1.random.Next(3000, 6000));
      this.fadeBack();
      this.bundleColor = bundleColor;
      this.controller = new PathFindController((Character) this, location, new Point(25, 10), 0, new PathFindController.endBehavior(this.junimoReachedHutToReturnBundle));
      this.collidesWithOtherCharacters = false;
      this.farmerPassesThrough = false;
      this.holdingBundle = true;
      this.speed = 1;
    }

    private void junimoReachedHutToReturnBundle(Character c, GameLocation l)
    {
      this.holdingBundle = false;
      this.collidesWithOtherCharacters = true;
      this.farmerPassesThrough = true;
      Game1.playSound("Ship");
    }

    private void junimoReachedHutToFetchStar(Character c, GameLocation l)
    {
      this.holdingStar = true;
      this.holdingBundle = false;
      this.speed = 1;
      this.collidesWithOtherCharacters = false;
      this.farmerPassesThrough = false;
      this.controller = new PathFindController((Character) this, l, new Point(32, 9), 2, new PathFindController.endBehavior(this.placeStar));
      Game1.playSound("dwop");
      this.farmerPassesThrough = false;
    }

    private void placeStar(Character c, GameLocation l)
    {
      this.collidesWithOtherCharacters = false;
      this.farmerPassesThrough = true;
      this.holdingStar = false;
      Game1.playSound("tinyWhip");
      this.friendly = true;
      this.speed = 3;
      List<TemporaryAnimatedSprite> temporarySprites = l.temporarySprites;
      TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(this.sprite.Texture, new Rectangle(0, 109, 16, 19), 40f, 8, 10, this.position + new Vector2(0.0f, (float) -Game1.tileSize), false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom * this.scale, 0.0f, 0.0f, 0.0f, false);
      temporaryAnimatedSprite.endFunction = new TemporaryAnimatedSprite.endBehavior(this.starDoneSpinning);
      temporaryAnimatedSprite.endSound = "yoba";
      Vector2 vector2_1 = new Vector2(0.22f, -2f);
      temporaryAnimatedSprite.motion = vector2_1;
      Vector2 vector2_2 = new Vector2(0.0f, 0.01f);
      temporaryAnimatedSprite.acceleration = vector2_2;
      double num = 777.0;
      temporaryAnimatedSprite.id = (float) num;
      temporarySprites.Add(temporaryAnimatedSprite);
      if (!(l is CommunityCenter) || !(l as CommunityCenter).areAllAreasComplete())
        return;
      Game1.player.faceDirection(0);
      this.fadeAway();
      Game1.pauseThenDoFunction(2000, new Game1.afterFadeFunction(this.goodbyeDance));
    }

    public void sayGoodbye()
    {
      this.sayingGoodbye = true;
      this.farmerPassesThrough = true;
    }

    private void goodbyeDance()
    {
      Game1.player.faceDirection(3);
      (Game1.getLocationFromName("CommunityCenter") as CommunityCenter).junimoGoodbyeDance();
    }

    private void starDoneSpinning(int extraInfo)
    {
      GameLocation locationFromName = Game1.getLocationFromName("CommunityCenter");
      if (!Game1.currentLocation.Equals((object) locationFromName))
        return;
      Game1.flashAlpha = 1f;
      (locationFromName as CommunityCenter).addStarToPlaque();
    }

    public override void drawAboveAlwaysFrontLayer(SpriteBatch b)
    {
      if (this.textAboveHeadTimer <= 0 || this.textAboveHead == null)
        return;
      Vector2 local = Game1.GlobalToLocal(new Vector2((float) this.getStandingX(), (float) this.getStandingY() - (float) Game1.tileSize * 2f + (float) this.yJumpOffset));
      if (this.textAboveHeadStyle == 0)
        local += new Vector2((float) Game1.random.Next(-1, 2), (float) Game1.random.Next(-1, 2));
      SpriteText.drawStringWithScrollCenteredAt(b, this.textAboveHead, (int) local.X, (int) local.Y, "", this.textAboveHeadAlpha, this.textAboveHeadColor, 1, (float) ((double) (this.getTileY() * Game1.tileSize) / 10000.0 + 1.0 / 1000.0 + (double) this.getTileX() / 10000.0), !this.sayingGoodbye);
    }

    public void junimoReachedHut(Character c, GameLocation l)
    {
      this.fadeAway();
      this.controller = (PathFindController) null;
      this.motion.X = 0.0f;
      this.motion.Y = -1f;
    }

    public override void update(GameTime time, GameLocation location)
    {
      base.update(time, location);
      this.forceUpdateTimer = 99999;
      if (this.eventActor)
        return;
      this.alpha = this.alpha + this.alphaChange;
      if ((double) this.alpha > 1.0)
      {
        this.alpha = 1f;
        this.hideShadow = false;
      }
      else if ((double) this.alpha < 0.0)
      {
        this.alpha = 0.0f;
        this.isInvisible = true;
        this.hideShadow = true;
      }
      --Junimo.soundTimer;
      this.farmerCloseCheckTimer = this.farmerCloseCheckTimer - time.ElapsedGameTime.Milliseconds;
      if (this.sayingGoodbye)
      {
        this.flip = false;
        if (this.whichArea % 2 == 0)
          this.sprite.Animate(time, 16, 8, 50f);
        else
          this.sprite.Animate(time, 28, 4, 80f);
        if (this.isInvisible || Game1.random.NextDouble() >= 0.00999999977648258 || this.yJumpOffset != 0)
          return;
        this.jump();
        if (Game1.random.NextDouble() >= 0.15 || Game1.player.getTileX() != 29 || Game1.player.getTileY() != 11)
          return;
        this.showTextAboveHead(Game1.random.NextDouble() < 0.5 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Junimo.cs.6625") : Game1.content.LoadString("Strings\\StringsFromCSFiles:Junimo.cs.6626"), -1, 2, 3000, 0);
      }
      else if (this.temporaryJunimo)
      {
        this.sprite.Animate(time, 12, 4, 100f);
        if (Game1.random.NextDouble() >= 0.001)
          return;
        this.jumpWithoutSound(8f);
        Game1.playSound("junimoMeep1");
      }
      else
      {
        if (!this.isInvisible && this.farmerCloseCheckTimer <= 0 && (this.controller == null && (double) this.alpha >= 1.0) && !this.stayPut)
        {
          this.farmerCloseCheckTimer = 100;
          Farmer farmer = Utility.isThereAFarmerWithinDistance(this.getTileLocation(), 5);
          if (farmer != null)
          {
            if (this.friendly && (double) Vector2.Distance(this.position, farmer.position) > (double) (this.speed * 4))
            {
              if (this.motion.Equals(Vector2.Zero) && Junimo.soundTimer <= 0)
              {
                this.jump();
                Game1.playSound("junimoMeep1");
                Junimo.soundTimer = 400;
              }
              if (Game1.random.NextDouble() < 0.007)
                this.jumpWithoutSound((float) Game1.random.Next(6, 9));
              this.setMoving(Utility.getVelocityTowardPlayer(new Point((int) this.position.X, (int) this.position.Y), (float) this.speed, farmer));
            }
            else if (!this.friendly)
            {
              this.fadeAway();
              Vector2 playerTrajectory = Utility.getAwayFromPlayerTrajectory(this.GetBoundingBox(), farmer);
              playerTrajectory.Normalize();
              playerTrajectory.Y *= -1f;
              this.setMoving(playerTrajectory * (float) this.speed);
            }
            else if ((double) this.alpha >= 1.0)
              this.motion = Vector2.Zero;
          }
          else if ((double) this.alpha >= 1.0)
            this.motion = Vector2.Zero;
        }
        if (!this.isInvisible && this.controller == null)
        {
          this.nextPosition = this.GetBoundingBox();
          this.nextPosition.X += (int) this.motion.X;
          bool flag = false;
          if (!location.isCollidingPosition(this.nextPosition, Game1.viewport, (Character) this))
          {
            this.position.X += (float) (int) this.motion.X;
            flag = true;
          }
          this.nextPosition.X -= (int) this.motion.X;
          this.nextPosition.Y += (int) this.motion.Y;
          if (!location.isCollidingPosition(this.nextPosition, Game1.viewport, (Character) this))
          {
            this.position.Y += (float) (int) this.motion.Y;
            flag = true;
          }
          if (!this.motion.Equals(Vector2.Zero) & flag && Game1.random.NextDouble() < 0.005)
            location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.random.NextDouble() < 0.5 ? 10 : 11, this.position, this.color, 8, false, 100f, 0, -1, -1f, -1, 0)
            {
              motion = this.motion / 4f,
              alphaFade = 0.01f,
              layerDepth = 0.8f,
              scale = 0.75f,
              alpha = 0.75f
            });
        }
        if (this.controller != null || !this.motion.Equals(Vector2.Zero))
        {
          if (this.holdingStar || this.holdingBundle)
            this.sprite.Animate(time, 44, 4, 200f);
          else if (this.moveRight || (double) Math.Abs(this.motion.X) > (double) Math.Abs(this.motion.Y) && (double) this.motion.X > 0.0)
          {
            this.flip = false;
            this.sprite.Animate(time, 16, 8, 50f);
          }
          else if (this.moveLeft || (double) Math.Abs(this.motion.X) > (double) Math.Abs(this.motion.Y) && (double) this.motion.X < 0.0)
          {
            this.sprite.Animate(time, 16, 8, 50f);
            this.flip = true;
          }
          else if (this.moveUp || (double) Math.Abs(this.motion.Y) > (double) Math.Abs(this.motion.X) && (double) this.motion.Y < 0.0)
            this.sprite.Animate(time, 32, 8, 50f);
          else
            this.sprite.Animate(time, 0, 8, 50f);
        }
        else
          this.sprite.Animate(time, 8, 4, 100f);
      }
    }

    public override void draw(SpriteBatch b, float alpha = 1f)
    {
      if (this.isInvisible)
        return;
      b.Draw(this.Sprite.Texture, this.getLocalPosition(Game1.viewport) + new Vector2((float) (this.sprite.spriteWidth * Game1.pixelZoom / 2), (float) ((double) this.sprite.spriteHeight * 3.0 / 4.0 * (double) Game1.pixelZoom / Math.Pow((double) (this.sprite.spriteHeight / 16), 2.0)) + (float) this.yJumpOffset - (float) (Game1.pixelZoom * 2)) + (this.shakeTimer > 0 ? new Vector2((float) Game1.random.Next(-1, 2), (float) Game1.random.Next(-1, 2)) : Vector2.Zero), new Rectangle?(this.Sprite.SourceRect), this.color * this.alpha, this.rotation, new Vector2((float) (this.sprite.spriteWidth * Game1.pixelZoom / 2), (float) ((double) (this.sprite.spriteHeight * Game1.pixelZoom) * 3.0 / 4.0)) / (float) Game1.pixelZoom, Math.Max(0.2f, this.scale) * (float) Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.991f : (float) this.getStandingY() / 10000f));
      if (!this.swimming && !this.hideShadow)
        b.Draw(Game1.shadowTexture, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2((float) (this.sprite.spriteWidth * Game1.pixelZoom) / 2f, (float) (Game1.tileSize * 3) / 4f - (float) Game1.pixelZoom)), new Rectangle?(Game1.shadowTexture.Bounds), this.color * this.alpha, 0.0f, new Vector2((float) Game1.shadowTexture.Bounds.Center.X, (float) Game1.shadowTexture.Bounds.Center.Y), ((float) Game1.pixelZoom + (float) this.yJumpOffset / 40f) * this.scale, SpriteEffects.None, Math.Max(0.0f, (float) this.getStandingY() / 10000f) - 1E-06f);
      if (this.holdingStar)
      {
        b.Draw(this.sprite.Texture, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2((float) (Game1.pixelZoom * 2), (float) -Game1.tileSize * this.scale + (float) Game1.pixelZoom + (float) this.yJumpOffset)), new Rectangle?(new Rectangle(0, 109, 16, 19)), Color.White * this.alpha, 0.0f, Vector2.Zero, (float) Game1.pixelZoom * this.scale, SpriteEffects.None, (float) ((double) this.position.Y / 10000.0 + 9.99999974737875E-05));
      }
      else
      {
        if (!this.holdingBundle)
          return;
        b.Draw(this.sprite.Texture, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2((float) (Game1.pixelZoom * 2), (float) -Game1.tileSize * this.scale + (float) (Game1.pixelZoom * 5) + (float) this.yJumpOffset)), new Rectangle?(new Rectangle(0, 96, 16, 13)), this.bundleColor * this.alpha, 0.0f, Vector2.Zero, (float) Game1.pixelZoom * this.scale, SpriteEffects.None, (float) ((double) this.position.Y / 10000.0 + 9.99999974737875E-05));
      }
    }
  }
}
