// Decompiled with JetBrains decompiler
// Type: StardewValley.Monsters.Bat
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley.Monsters
{
  public class Bat : Monster
  {
    public const float rotationIncrement = 0.04908739f;
    private int wasHitCounter;
    private float targetRotation;
    private bool turningRight;
    private bool seenPlayer;
    private Cue batFlap;

    public Bat()
    {
    }

    public Bat(Vector2 position)
      : base(nameof (Bat), position)
    {
      this.slipperiness = 24 + Game1.random.Next(-10, 11);
      this.Halt();
      this.IsWalkingTowardPlayer = false;
      this.hideShadow = true;
    }

    public Bat(Vector2 position, int mineLevel)
      : base(nameof (Bat), position)
    {
      if (mineLevel >= 40 && mineLevel < 80)
      {
        this.name = "Frost Bat";
        this.parseMonsterInfo("Frost Bat");
        this.reloadSprite();
      }
      else if (mineLevel >= 80)
      {
        this.name = "Lava Bat";
        this.parseMonsterInfo("Lava Bat");
        this.reloadSprite();
      }
      this.slipperiness = 20 + Game1.random.Next(-5, 6);
      this.Halt();
      this.IsWalkingTowardPlayer = false;
      this.hideShadow = true;
    }

    public override void reloadSprite()
    {
      this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Monsters\\" + this.name));
      this.hideShadow = true;
    }

    public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
    {
      int num = Math.Max(1, damage - this.resilience);
      this.seenPlayer = true;
      if (Game1.random.NextDouble() < this.missChance - this.missChance * addedPrecision)
      {
        num = -1;
      }
      else
      {
        this.health = this.health - num;
        this.setTrajectory(xTrajectory / 3, yTrajectory / 3);
        this.wasHitCounter = 500;
        Game1.playSound("hitEnemy");
        if (this.health <= 0)
        {
          this.deathAnimation();
          Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(44, this.position, Color.DarkMagenta, 10, false, 100f, 0, -1, -1f, -1, 0));
          Game1.playSound("batScreech");
        }
      }
      this.addedSpeed = Game1.random.Next(-1, 1);
      return num;
    }

    public override void shedChunks(int number, float scale)
    {
      GameLocation currentLocation = Game1.currentLocation;
      Texture2D texture = this.sprite.Texture;
      Rectangle sourcerectangle = new Rectangle(0, 384, 64, 64);
      int sizeOfSourceRectSquares = 32;
      Rectangle boundingBox = this.GetBoundingBox();
      int x = boundingBox.Center.X;
      boundingBox = this.GetBoundingBox();
      int y1 = boundingBox.Center.Y;
      int numberOfChunks = number;
      int y2 = (int) this.getTileLocation().Y;
      Color white = Color.White;
      double num = (double) scale;
      Game1.createRadialDebris(currentLocation, texture, sourcerectangle, sizeOfSourceRectSquares, x, y1, numberOfChunks, y2, white, (float) num);
    }

    public override void drawAboveAllLayers(SpriteBatch b)
    {
      if (!Utility.isOnScreen(this.position, 2 * Game1.tileSize))
        return;
      b.Draw(this.Sprite.Texture, this.getLocalPosition(Game1.viewport) + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize - Game1.tileSize / 2)), new Rectangle?(this.Sprite.SourceRect), Color.White, 0.0f, new Vector2(8f, 16f), Math.Max(0.2f, this.scale) * (float) Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0.92f);
      b.Draw(Game1.shadowTexture, this.getLocalPosition(Game1.viewport) + new Vector2((float) (Game1.tileSize / 2), (float) Game1.tileSize), new Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0.0f, new Vector2((float) Game1.shadowTexture.Bounds.Center.X, (float) Game1.shadowTexture.Bounds.Center.Y), (float) Game1.pixelZoom, SpriteEffects.None, this.wildernessFarmMonster ? 0.0001f : (float) (this.getStandingY() - 1) / 10000f);
      if (!this.isGlowing)
        return;
      b.Draw(this.Sprite.Texture, this.getLocalPosition(Game1.viewport) + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize - Game1.tileSize / 2)), new Rectangle?(this.Sprite.SourceRect), this.glowingColor * this.glowingTransparency, 0.0f, new Vector2(8f, 16f), Math.Max(0.2f, this.scale) * (float) Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.99f : (float) ((double) this.getStandingY() / 10000.0 + 1.0 / 1000.0)));
    }

    public override void behaviorAtGameTick(GameTime time)
    {
      base.behaviorAtGameTick(time);
      if (this.wasHitCounter >= 0)
        this.wasHitCounter = this.wasHitCounter - time.ElapsedGameTime.Milliseconds;
      if (double.IsNaN((double) this.xVelocity) || double.IsNaN((double) this.yVelocity) || ((double) this.position.X < -2000.0 || (double) this.position.Y < -2000.0))
        this.health = -500;
      if ((double) this.position.X <= -640.0 || (double) this.position.Y <= -640.0 || ((double) this.position.X >= (double) (Game1.currentLocation.Map.Layers[0].LayerWidth * Game1.tileSize + 640) || (double) this.position.Y >= (double) (Game1.currentLocation.Map.Layers[0].LayerHeight * Game1.tileSize + 640)))
        this.health = -500;
      if (this.focusedOnFarmers || this.withinPlayerThreshold(6) || this.seenPlayer)
      {
        this.seenPlayer = true;
        this.sprite.Animate(time, 0, 4, 80f);
        if (this.sprite.CurrentFrame % 3 == 0 && Utility.isOnScreen(this.position, Game1.tileSize * 8) && (this.batFlap == null || !this.batFlap.IsPlaying) && Game1.soundBank != null)
        {
          this.batFlap = Game1.soundBank.GetCue("batFlap");
          this.batFlap.Play();
        }
        if (this.invincibleCountdown > 0)
        {
          if (!this.name.Equals("Lava Bat"))
            return;
          this.glowingColor = Color.Cyan;
        }
        else
        {
          float num1 = (float) -(Game1.player.GetBoundingBox().Center.X - this.GetBoundingBox().Center.X);
          float num2 = (float) (Game1.player.GetBoundingBox().Center.Y - this.GetBoundingBox().Center.Y);
          float num3 = Math.Max(1f, Math.Abs(num1) + Math.Abs(num2));
          if ((double) num3 < (double) Game1.tileSize)
          {
            this.xVelocity = Math.Max(-5f, Math.Min(5f, this.xVelocity * 1.05f));
            this.yVelocity = Math.Max(-5f, Math.Min(5f, this.yVelocity * 1.05f));
          }
          float num4 = num1 / num3;
          float num5 = num2 / num3;
          if (this.wasHitCounter <= 0)
          {
            this.targetRotation = (float) Math.Atan2(-(double) num5, (double) num4) - 1.570796f;
            if ((double) Math.Abs(this.targetRotation) - (double) Math.Abs(this.rotation) > 7.0 * Math.PI / 8.0 && Game1.random.NextDouble() < 0.5)
              this.turningRight = true;
            else if ((double) Math.Abs(this.targetRotation) - (double) Math.Abs(this.rotation) < Math.PI / 8.0)
              this.turningRight = false;
            if (this.turningRight)
              this.rotation = this.rotation - (float) Math.Sign(this.targetRotation - this.rotation) * ((float) Math.PI / 64f);
            else
              this.rotation = this.rotation + (float) Math.Sign(this.targetRotation - this.rotation) * ((float) Math.PI / 64f);
            this.rotation = this.rotation % 6.283185f;
            this.wasHitCounter = 0;
          }
          float num6 = Math.Min(5f, Math.Max(1f, (float) (5.0 - (double) num3 / (double) Game1.tileSize / 2.0)));
          float num7 = (float) Math.Cos((double) this.rotation + Math.PI / 2.0);
          float num8 = -(float) Math.Sin((double) this.rotation + Math.PI / 2.0);
          this.xVelocity = this.xVelocity + (float) (-(double) num7 * (double) num6 / 6.0 + (double) Game1.random.Next(-10, 10) / 100.0);
          this.yVelocity = this.yVelocity + (float) (-(double) num8 * (double) num6 / 6.0 + (double) Game1.random.Next(-10, 10) / 100.0);
          if ((double) Math.Abs(this.xVelocity) > (double) Math.Abs((float) (-(double) num7 * 5.0)))
            this.xVelocity = this.xVelocity - (float) (-(double) num7 * (double) num6 / 6.0);
          if ((double) Math.Abs(this.yVelocity) <= (double) Math.Abs((float) (-(double) num8 * 5.0)))
            return;
          this.yVelocity = this.yVelocity - (float) (-(double) num8 * (double) num6 / 6.0);
        }
      }
      else
      {
        this.sprite.CurrentFrame = 4;
        this.Halt();
      }
    }
  }
}
