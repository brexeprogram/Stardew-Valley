// Decompiled with JetBrains decompiler
// Type: StardewValley.Monsters.SquidKid
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Projectiles;
using System;

namespace StardewValley.Monsters
{
  public class SquidKid : Monster
  {
    private float lastFireball;
    private int yOffset;

    public SquidKid()
    {
    }

    public SquidKid(Vector2 position)
      : base("Squid Kid", position)
    {
      this.sprite.spriteHeight = 16;
      this.IsWalkingTowardPlayer = false;
      this.sprite.UpdateSourceRect();
    }

    public override void reloadSprite()
    {
      this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Monsters\\Squid Kid"));
    }

    public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
    {
      int num = Math.Max(1, damage - this.resilience);
      if (Game1.random.NextDouble() < this.missChance - this.missChance * addedPrecision)
      {
        num = -1;
      }
      else
      {
        this.health = this.health - num;
        this.setTrajectory(xTrajectory, yTrajectory);
        Game1.playSound("hitEnemy");
        this.sprite.CurrentFrame = this.sprite.CurrentFrame - this.sprite.CurrentFrame % 4 + 3;
        if (this.health <= 0)
          this.deathAnimation();
      }
      return num;
    }

    public override void deathAnimation()
    {
      Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(this.sprite.Texture, new Rectangle(0, 64, 16, 16), 70f, 7, 0, this.position + new Vector2(0.0f, (float) (-Game1.tileSize / 2)), false, false)
      {
        scale = (float) Game1.pixelZoom
      });
      Game1.playSound("fireball");
      Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(362, 30f, 6, 1, this.position + new Vector2((float) (-Game1.tileSize / 4 + Game1.random.Next(Game1.tileSize)), (float) (Game1.random.Next(Game1.tileSize) - Game1.tileSize / 2)), false, Game1.random.NextDouble() < 0.5)
      {
        delayBeforeAnimationStart = 100
      });
      Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(362, 30f, 6, 1, this.position + new Vector2((float) (-Game1.tileSize / 4 + Game1.random.Next(Game1.tileSize)), (float) (Game1.random.Next(Game1.tileSize) - Game1.tileSize / 2)), false, Game1.random.NextDouble() < 0.5)
      {
        delayBeforeAnimationStart = 200
      });
      Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(362, 30f, 6, 1, this.position + new Vector2((float) (-Game1.tileSize / 4 + Game1.random.Next(Game1.tileSize)), (float) (Game1.random.Next(Game1.tileSize) - Game1.tileSize / 2)), false, Game1.random.NextDouble() < 0.5)
      {
        delayBeforeAnimationStart = 300
      });
      Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(362, 30f, 6, 1, this.position + new Vector2((float) (-Game1.tileSize / 4 + Game1.random.Next(Game1.tileSize)), (float) (Game1.random.Next(Game1.tileSize) - Game1.tileSize / 2)), false, Game1.random.NextDouble() < 0.5)
      {
        delayBeforeAnimationStart = 400
      });
    }

    public override void drawAboveAllLayers(SpriteBatch b)
    {
      b.Draw(this.Sprite.Texture, this.getLocalPosition(Game1.viewport) + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 3 + this.yOffset)), new Rectangle?(this.Sprite.SourceRect), Color.White, 0.0f, new Vector2(8f, 16f), Math.Max(0.2f, this.scale) * (float) Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.991f : (float) this.getStandingY() / 10000f));
      SpriteBatch spriteBatch = b;
      Texture2D shadowTexture = Game1.shadowTexture;
      Vector2 position = this.getLocalPosition(Game1.viewport) + new Vector2((float) (Game1.tileSize / 2), (float) Game1.tileSize);
      Rectangle? sourceRectangle = new Rectangle?(Game1.shadowTexture.Bounds);
      Color white = Color.White;
      double num1 = 0.0;
      Rectangle bounds = Game1.shadowTexture.Bounds;
      double x = (double) bounds.Center.X;
      bounds = Game1.shadowTexture.Bounds;
      double y = (double) bounds.Center.Y;
      Vector2 origin = new Vector2((float) x, (float) y);
      double num2 = 3.0 + (double) this.yOffset / 20.0;
      int num3 = 0;
      double num4 = (double) (this.getStandingY() - 1) / 10000.0;
      spriteBatch.Draw(shadowTexture, position, sourceRectangle, white, (float) num1, origin, (float) num2, (SpriteEffects) num3, (float) num4);
    }

    public override void behaviorAtGameTick(GameTime time)
    {
      base.behaviorAtGameTick(time);
      this.faceGeneralDirection(Game1.player.position, 0);
      this.yOffset = (int) (Math.Sin((double) time.TotalGameTime.Milliseconds / 2000.0 * (2.0 * Math.PI)) * 15.0);
      if (this.sprite.CurrentFrame % 4 != 0 && Game1.random.NextDouble() < 0.1)
        this.sprite.CurrentFrame -= this.sprite.CurrentFrame % 4;
      if (Game1.random.NextDouble() < 0.01)
        ++this.sprite.CurrentFrame;
      this.lastFireball = Math.Max(0.0f, this.lastFireball - (float) time.ElapsedGameTime.Milliseconds);
      if (this.withinPlayerThreshold() && (double) this.lastFireball == 0.0 && Game1.random.NextDouble() < 0.01)
      {
        this.IsWalkingTowardPlayer = false;
        Vector2 vector2 = new Vector2(this.position.X, this.position.Y + (float) Game1.tileSize);
        this.Halt();
        switch (this.facingDirection)
        {
          case 0:
            this.sprite.CurrentFrame = 3;
            break;
          case 1:
            this.sprite.CurrentFrame = 7;
            vector2.X += (float) Game1.tileSize;
            break;
          case 2:
            this.sprite.CurrentFrame = 11;
            vector2.Y += (float) (Game1.tileSize / 2);
            break;
          case 3:
            this.sprite.CurrentFrame = 15;
            vector2.X -= (float) (Game1.tileSize / 2);
            break;
        }
        this.sprite.UpdateSourceRect();
        Vector2 velocityTowardPlayer = Utility.getVelocityTowardPlayer(Utility.Vector2ToPoint(this.getStandingPosition()), 8f, Game1.player);
        Game1.currentLocation.projectiles.Add((Projectile) new BasicProjectile(15, 10, 3, 4, 0.0f, velocityTowardPlayer.X, velocityTowardPlayer.Y, this.getStandingPosition(), "", "", true, false, (Character) this, false, (BasicProjectile.onCollisionBehavior) null));
        Game1.playSound("fireball");
        this.lastFireball = (float) Game1.random.Next(1200, 3500);
      }
      else
      {
        if ((double) this.lastFireball == 0.0 || Game1.random.NextDouble() >= 0.02)
          return;
        this.Halt();
        if (!this.withinPlayerThreshold())
          return;
        this.slipperiness = 8;
        this.setTrajectory((int) Utility.getVelocityTowardPlayer(Utility.Vector2ToPoint(this.getStandingPosition()), 8f, Game1.player).X, (int) -(double) Utility.getVelocityTowardPlayer(Utility.Vector2ToPoint(this.getStandingPosition()), 8f, Game1.player).Y);
      }
    }
  }
}
