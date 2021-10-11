// Decompiled with JetBrains decompiler
// Type: StardewValley.Monsters.DustSpirit
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley.Monsters
{
  public class DustSpirit : Monster
  {
    private bool seenFarmer;
    private bool runningAwayFromFarmer;
    private bool chargingFarmer;
    public byte voice;
    private Cue meep;

    public DustSpirit()
    {
    }

    public DustSpirit(Vector2 position)
      : base("Dust Spirit", position)
    {
      this.IsWalkingTowardPlayer = false;
      this.sprite.interval = 45f;
      this.scale = (float) Game1.random.Next(75, 101) / 100f;
      this.voice = (byte) Game1.random.Next(1, 24);
    }

    public DustSpirit(Vector2 position, bool chargingTowardFarmer)
      : base("Dust Spirit", position)
    {
      this.IsWalkingTowardPlayer = false;
      if (chargingTowardFarmer)
      {
        this.chargingFarmer = true;
        this.seenFarmer = true;
      }
      this.sprite.interval = 45f;
      this.scale = (float) Game1.random.Next(75, 101) / 100f;
    }

    public override void draw(SpriteBatch b)
    {
      if (this.isInvisible || !Utility.isOnScreen(this.position, 2 * Game1.tileSize))
        return;
      b.Draw(this.Sprite.Texture, this.getLocalPosition(Game1.viewport) + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize + this.yJumpOffset)), new Rectangle?(this.Sprite.SourceRect), Color.White, this.rotation, new Vector2(8f, 16f), new Vector2(this.scale + (float) Math.Max(-0.1, (double) (this.yJumpOffset + 32) / 128.0), this.scale - Math.Max(-0.1f, (float) this.yJumpOffset / 256f)) * (float) Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.991f : (float) this.getStandingY() / 10000f));
      if (this.isGlowing)
        b.Draw(this.Sprite.Texture, this.getLocalPosition(Game1.viewport) + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize + this.yJumpOffset)), new Rectangle?(this.Sprite.SourceRect), this.glowingColor * this.glowingTransparency, this.rotation, new Vector2(8f, 16f), Math.Max(0.2f, this.scale) * (float) Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.99f : (float) ((double) this.getStandingY() / 10000.0 + 1.0 / 1000.0)));
      b.Draw(Game1.shadowTexture, this.getLocalPosition(Game1.viewport) + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize * 5 / 4)), new Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0.0f, new Vector2((float) Game1.shadowTexture.Bounds.Center.X, (float) Game1.shadowTexture.Bounds.Center.Y), (float) (4.0 + (double) this.yJumpOffset / (double) Game1.tileSize), SpriteEffects.None, (float) (this.getStandingY() - 1) / 10000f);
    }

    public override void deathAnimation()
    {
      Game1.playSound("dustMeep");
      Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(44, this.position, new Color(50, 50, 80), 10, false, 100f, 0, -1, -1f, -1, 0));
      Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(44, this.position + new Vector2((float) Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize / 2), (float) Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize / 2)), new Color(50, 50, 80), 10, false, 100f, 0, -1, -1f, -1, 0)
      {
        delayBeforeAnimationStart = 150,
        scale = 0.5f
      });
      Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(44, this.position + new Vector2((float) Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize / 2), (float) Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize / 2)), new Color(50, 50, 80), 10, false, 100f, 0, -1, -1f, -1, 0)
      {
        delayBeforeAnimationStart = 300,
        scale = 0.5f
      });
      Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(44, this.position + new Vector2((float) Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize / 2), (float) Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize / 2)), new Color(50, 50, 80), 10, false, 100f, 0, -1, -1f, -1, 0)
      {
        delayBeforeAnimationStart = 450,
        scale = 0.5f
      });
    }

    public override void shedChunks(int number, float scale)
    {
      GameLocation currentLocation = Game1.currentLocation;
      Texture2D texture = this.sprite.Texture;
      Rectangle sourcerectangle = new Rectangle(0, 16, 16, 16);
      int sizeOfSourceRectSquares = 8;
      Rectangle boundingBox = this.GetBoundingBox();
      int x = boundingBox.Center.X;
      boundingBox = this.GetBoundingBox();
      int y1 = boundingBox.Center.Y;
      int numberOfChunks = number;
      int y2 = (int) this.getTileLocation().Y;
      Color white = Color.White;
      double num = this.health <= 0 ? (double) Game1.pixelZoom : (double) Game1.pixelZoom * 0.5;
      Game1.createRadialDebris(currentLocation, texture, sourcerectangle, sizeOfSourceRectSquares, x, y1, numberOfChunks, y2, white, (float) num);
    }

    public void offScreenBehavior(Character c, GameLocation l)
    {
    }

    public override void behaviorAtGameTick(GameTime time)
    {
      base.behaviorAtGameTick(time);
      this.sprite.AnimateDown(time, 0, "");
      if (this.yJumpOffset == 0)
      {
        this.jumpWithoutSound(8f);
        this.yJumpVelocity = (float) Game1.random.Next(50, 70) / 10f;
        if (Game1.random.NextDouble() < 0.1 && (this.meep == null || !this.meep.IsPlaying) && (Utility.isOnScreen(this.position, 64) && Game1.soundBank != null))
        {
          this.meep = Game1.soundBank.GetCue("dustMeep");
          this.meep.SetVariable("Pitch", (float) ((int) this.voice * 100 + Game1.random.Next(-100, 100)));
          this.meep.Play();
        }
        if (Game1.random.NextDouble() < 0.01)
        {
          Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 128, 64, 64), 40f, 4, 0, this.getStandingPosition(), false, false));
          foreach (Vector2 adjacentTileLocation in Utility.getAdjacentTileLocations(this.getTileLocation()))
          {
            if (Game1.currentLocation.objects.ContainsKey(adjacentTileLocation) && Game1.currentLocation.objects[adjacentTileLocation].Name.Contains("Stone"))
              Game1.currentLocation.destroyObject(adjacentTileLocation, (Farmer) null);
          }
          this.yJumpVelocity = this.yJumpVelocity * 2f;
        }
        if (!this.chargingFarmer)
          this.xVelocity = (float) Game1.random.Next(-20, 21) / 5f;
      }
      if (this.chargingFarmer)
      {
        this.slipperiness = 10;
        Vector2 playerTrajectory = Utility.getAwayFromPlayerTrajectory(this.GetBoundingBox());
        this.xVelocity = this.xVelocity + (float) (-(double) playerTrajectory.X / 150.0 + (Game1.random.NextDouble() < 0.01 ? (double) Game1.random.Next(-50, 50) / 10.0 : 0.0));
        if ((double) Math.Abs(this.xVelocity) > 5.0)
          this.xVelocity = (float) (Math.Sign(this.xVelocity) * 5);
        this.yVelocity = this.yVelocity + (float) (-(double) playerTrajectory.Y / 150.0 + (Game1.random.NextDouble() < 0.01 ? (double) Game1.random.Next(-50, 50) / 10.0 : 0.0));
        if ((double) Math.Abs(this.yVelocity) > 5.0)
          this.yVelocity = (float) (Math.Sign(this.yVelocity) * 5);
        if (Game1.random.NextDouble() >= 0.0001)
          return;
        this.controller = new PathFindController((Character) this, Game1.currentLocation, new Point((int) Game1.player.getTileLocation().X, (int) Game1.player.getTileLocation().Y), Game1.random.Next(4), (PathFindController.endBehavior) null, 300);
        this.chargingFarmer = false;
      }
      else if (!this.seenFarmer && Utility.doesPointHaveLineOfSightInMine(this.getStandingPosition() / (float) Game1.tileSize, Game1.player.getStandingPosition() / (float) Game1.tileSize, 8))
        this.seenFarmer = true;
      else if (this.seenFarmer && this.controller == null && !this.runningAwayFromFarmer)
      {
        this.addedSpeed = 2;
        this.controller = new PathFindController((Character) this, Game1.currentLocation, new PathFindController.isAtEnd(Utility.isOffScreenEndFunction), -1, false, new PathFindController.endBehavior(this.offScreenBehavior), 350, Point.Zero);
        this.runningAwayFromFarmer = true;
      }
      else
      {
        if (this.controller != null || !this.runningAwayFromFarmer)
          return;
        this.chargingFarmer = true;
      }
    }
  }
}
