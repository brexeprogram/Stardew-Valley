// Decompiled with JetBrains decompiler
// Type: StardewValley.Monsters.BigSlime
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley.Monsters
{
  public class BigSlime : Monster
  {
    public Color c;

    public BigSlime()
    {
    }

    public BigSlime(Vector2 position)
      : this(position, Game1.mine.getMineArea(-1))
    {
      this.sprite.ignoreStopAnimation = true;
      this.ignoreMovementAnimations = true;
      this.hideShadow = true;
    }

    public BigSlime(Vector2 position, int mineArea)
      : base("Big Slime", position)
    {
      this.ignoreMovementAnimations = true;
      this.sprite.ignoreStopAnimation = true;
      this.sprite.spriteWidth = 32;
      this.sprite.spriteHeight = 32;
      this.sprite.UpdateSourceRect();
      this.sprite.framesPerAnimation = 8;
      this.c = Color.White;
      switch (Game1.mine.getMineArea(-1))
      {
        case 80:
          this.c = Color.Red;
          this.health = this.health * 3;
          this.damageToFarmer = this.damageToFarmer * 2;
          this.experienceGained = this.experienceGained * 3;
          break;
        case 121:
          this.c = Color.BlueViolet;
          this.health = this.health * 4;
          this.damageToFarmer = this.damageToFarmer * 3;
          this.experienceGained = this.experienceGained * 3;
          break;
        case 0:
          this.c = Color.Green;
          break;
        case 40:
          this.c = Color.Turquoise;
          this.health = this.health * 2;
          this.experienceGained = this.experienceGained * 2;
          break;
      }
      int r = (int) this.c.R;
      int g = (int) this.c.G;
      int b = (int) this.c.B;
      int val2_1 = r + Game1.random.Next(-20, 21);
      int val2_2 = g + Game1.random.Next(-20, 21);
      int val2_3 = b + Game1.random.Next(-20, 21);
      this.c.R = (byte) Math.Max(Math.Min((int) byte.MaxValue, val2_1), 0);
      this.c.G = (byte) Math.Max(Math.Min((int) byte.MaxValue, val2_2), 0);
      this.c.B = (byte) Math.Max(Math.Min((int) byte.MaxValue, val2_3), 0);
      this.c = this.c * ((float) Game1.random.Next(7, 11) / 10f);
      this.sprite.interval = 300f;
      this.hideShadow = true;
    }

    public override void reloadSprite()
    {
      base.reloadSprite();
      this.sprite.spriteWidth = 32;
      this.sprite.spriteHeight = 32;
      this.sprite.interval = 300f;
      this.sprite.ignoreStopAnimation = true;
      this.ignoreMovementAnimations = true;
      this.hideShadow = true;
      this.sprite.UpdateSourceRect();
      this.sprite.framesPerAnimation = 8;
    }

    public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
    {
      int num1 = Math.Max(1, damage - this.resilience);
      if (Game1.random.NextDouble() < this.missChance - this.missChance * addedPrecision)
      {
        num1 = -1;
      }
      else
      {
        this.slipperiness = 3;
        this.health = this.health - num1;
        this.setTrajectory(xTrajectory, yTrajectory);
        Game1.playSound("hitEnemy");
        this.IsWalkingTowardPlayer = true;
        if (this.health <= 0)
        {
          this.deathAnimation();
          ++Game1.stats.SlimesKilled;
          if ((int) Game1.gameMode == 3 && Game1.random.NextDouble() < 0.75)
          {
            int num2 = Game1.random.Next(2, 5);
            for (int index = 0; index < num2; ++index)
            {
              Game1.currentLocation.characters.Add((NPC) new GreenSlime(this.position, Game1.mine.mineLevel));
              Game1.currentLocation.characters[Game1.currentLocation.characters.Count - 1].setTrajectory(xTrajectory / 8 + Game1.random.Next(-2, 3), yTrajectory / 8 + Game1.random.Next(-2, 3));
              Game1.currentLocation.characters[Game1.currentLocation.characters.Count - 1].willDestroyObjectsUnderfoot = false;
              Game1.currentLocation.characters[Game1.currentLocation.characters.Count - 1].moveTowardPlayer(4);
              Game1.currentLocation.characters[Game1.currentLocation.characters.Count - 1].scale = (float) (0.75 + (double) Game1.random.Next(-5, 10) / 100.0);
            }
          }
        }
      }
      return num1;
    }

    public override void deathAnimation()
    {
      Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(44, this.position, this.c, 10, false, 70f, 0, -1, -1f, -1, 0));
      Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(44, this.position + new Vector2((float) (-Game1.tileSize / 2), 0.0f), this.c, 10, false, 70f, 0, -1, -1f, -1, 0)
      {
        delayBeforeAnimationStart = 100
      });
      Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(44, this.position + new Vector2((float) (Game1.tileSize / 2), 0.0f), this.c, 10, false, 70f, 0, -1, -1f, -1, 0)
      {
        delayBeforeAnimationStart = 200
      });
      Game1.playSound("slimedead");
      Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(44, this.position + new Vector2(0.0f, (float) (-Game1.tileSize / 2)), this.c, 10, false, 100f, 0, -1, -1f, -1, 0)
      {
        delayBeforeAnimationStart = 300
      });
    }

    public override void behaviorAtGameTick(GameTime time)
    {
      base.behaviorAtGameTick(time);
      int currentFrame = this.sprite.CurrentFrame;
      this.sprite.AnimateDown(time, 0, "");
      if (this.isMoving())
        this.sprite.interval = 100f;
      else
        this.sprite.interval = 200f;
      if (!Utility.isOnScreen(this.position, Game1.tileSize * 2) || this.sprite.CurrentFrame != 0 || currentFrame != 7)
        return;
      Game1.playSound("slimeHit");
    }

    public override void draw(SpriteBatch b)
    {
      if (this.isInvisible || !Utility.isOnScreen(this.position, 2 * Game1.tileSize))
        return;
      b.Draw(this.Sprite.Texture, this.getLocalPosition(Game1.viewport) + new Vector2((float) (Game1.tileSize * 3 / 4 + Game1.pixelZoom * 2), (float) (Game1.tileSize / 4 + this.yJumpOffset)), new Rectangle?(this.Sprite.SourceRect), this.c, this.rotation, new Vector2(16f, 16f), Math.Max(0.2f, this.scale) * (float) Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.991f : (float) this.getStandingY() / 10000f));
      if (!this.isGlowing)
        return;
      b.Draw(this.Sprite.Texture, this.getLocalPosition(Game1.viewport) + new Vector2((float) (Game1.tileSize * 3 / 4 + Game1.pixelZoom * 2), (float) (Game1.tileSize / 4 + this.yJumpOffset)), new Rectangle?(this.Sprite.SourceRect), this.glowingColor * this.glowingTransparency, 0.0f, new Vector2(16f, 16f), (float) Game1.pixelZoom * Math.Max(0.2f, this.scale), this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.991f : (float) ((double) this.getStandingY() / 10000.0 + 1.0 / 1000.0)));
    }

    public override Rectangle GetBoundingBox()
    {
      return new Rectangle((int) this.position.X + Game1.tileSize / 8, (int) this.position.Y, this.sprite.spriteWidth * Game1.pixelZoom * 3 / 4, Game1.tileSize);
    }

    public override void shedChunks(int number, float scale)
    {
    }
  }
}
