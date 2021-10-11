// Decompiled with JetBrains decompiler
// Type: StardewValley.Monsters.Spiker
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley.Monsters
{
  public class Spiker : Monster
  {
    public const int speedIncrement = 50;
    public int offset;
    public int movementDirection;
    public int pauseAtCollision;
    public int movementSpeed;
    private int pauseAtCollisionTimer;
    private int movementStartTimer;
    public bool mover;
    private new float rotation;
    public Rectangle room;
    private Vector2 previousPosition;

    public Spiker()
    {
    }

    public Spiker(Vector2 position, bool mover, Rectangle room, int startingDirection)
      : base(nameof (Spiker), position)
    {
      this.mover = mover;
      this.position = position;
      this.room = new Rectangle(room.X * Game1.tileSize - Game1.tileSize / 4, room.Y * Game1.tileSize - Game1.tileSize / 4, room.Width * Game1.tileSize + Game1.tileSize / 2, room.Height * Game1.tileSize + Game1.tileSize / 2);
      switch (Game1.mine.getMineArea(-1))
      {
        case 0:
          this.offset = 0;
          break;
        case 40:
          this.offset = 2;
          break;
        case 80:
          this.offset = 1;
          break;
      }
      this.movementSpeed = Game1.random.Next(5, 9);
      if (mover)
        this.offset = 4;
      this.sprite.spriteHeight = Game1.tileSize;
      this.sprite.CurrentFrame = this.offset;
      this.movementDirection = startingDirection;
      this.pauseAtCollision = Game1.random.Next(500, 2000);
      if (mover)
      {
        Vector2 velocityTowardPoint = Utility.getVelocityTowardPoint(new Point((int) position.X, (int) position.Y), new Vector2(position.X + 32f, position.Y + 32f), (float) this.movementSpeed);
        this.xVelocity = velocityTowardPoint.X;
        this.yVelocity = velocityTowardPoint.Y;
        this.slipperiness = 9999;
        this.isGlider = true;
      }
      else
      {
        this.facingDirection = this.movementDirection;
        this.setMovingInFacingDirection();
      }
      this.IsWalkingTowardPlayer = false;
    }

    public override void reloadSprite()
    {
      this.sprite.spriteHeight = Game1.tileSize;
      base.reloadSprite();
      this.sprite.CurrentFrame = this.offset;
    }

    public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
    {
      Game1.playSound("parry");
      return 0;
    }

    public override Rectangle GetBoundingBox()
    {
      return new Rectangle((int) this.position.X + 2, (int) this.position.Y + 2, Game1.tileSize - 4, Game1.tileSize - 4);
    }

    public override void behaviorAtGameTick(GameTime time)
    {
      if (this.mover)
      {
        this.rotation = (float) (((double) this.rotation + (double) time.ElapsedGameTime.Milliseconds * (Math.PI / 512.0)) % 6.28318548202515);
        if (!this.room.Contains((int) this.position.X, (int) this.position.Y) || (int) this.previousPosition.X == (int) this.position.X && (int) this.previousPosition.Y == (int) this.position.Y)
        {
          Vector2 velocityTowardPoint = Utility.getVelocityTowardPoint(new Point((int) this.position.X, (int) this.position.Y), new Vector2((float) this.room.Center.X, (float) this.room.Center.Y), (float) this.movementSpeed);
          if ((int) velocityTowardPoint.X == 0)
            velocityTowardPoint.X = 1f;
          if ((int) velocityTowardPoint.Y == 0)
            velocityTowardPoint.Y = 1f;
          this.xVelocity = velocityTowardPoint.X;
          this.yVelocity = -velocityTowardPoint.Y;
          this.position.X += this.xVelocity;
          this.position.Y -= this.yVelocity;
        }
      }
      else
      {
        if (this.movementStartTimer > 0)
        {
          this.movementStartTimer = this.movementStartTimer - time.ElapsedGameTime.Milliseconds;
          if (this.movementStartTimer <= 0)
          {
            this.addedSpeed = Math.Min(this.movementSpeed, this.addedSpeed + 1);
            if (this.addedSpeed < this.movementSpeed)
              this.movementStartTimer = 50;
          }
        }
        if (this.pauseAtCollisionTimer > 0)
        {
          this.pauseAtCollisionTimer = this.pauseAtCollisionTimer - time.ElapsedGameTime.Milliseconds;
          if (this.pauseAtCollisionTimer <= 0)
          {
            this.facingDirection = this.movementDirection;
            this.setMovingInFacingDirection();
            this.addedSpeed = 0;
            this.movementStartTimer = 50;
          }
        }
        else if (Game1.currentLocation.isCollidingPosition(this.nextPosition(this.movementDirection), Game1.viewport, false, 5, false, (Character) this) || !this.room.Contains(this.nextPosition(this.movementDirection)))
        {
          this.movementDirection = (this.movementDirection + 2) % 4;
          this.pauseAtCollisionTimer = this.pauseAtCollision;
          if (Utility.isOnScreen(this.position, 0))
            Game1.playSound("parry");
        }
      }
      this.previousPosition = new Vector2(this.position.X, this.position.Y);
    }

    public override bool passThroughCharacters()
    {
      return true;
    }

    public override void updateMovement(GameLocation location, GameTime time)
    {
      base.updateMovement(location, time);
    }

    public override void draw(SpriteBatch b)
    {
      if (this.mover)
      {
        b.Draw(this.Sprite.Texture, this.getLocalPosition(Game1.viewport) + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2)), new Rectangle?(new Rectangle(64, 64, 64, 64)), Color.White, this.rotation, new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2)), 1f, SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.991f : (float) (this.getStandingY() + Game1.tileSize) / 10000f));
        b.Draw(this.Sprite.Texture, this.getLocalPosition(Game1.viewport) + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2)), new Rectangle?(new Rectangle(88, 84, 16, 24)), Color.White, 0.0f, new Vector2(8f, 12f), 1f, SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.992f : (float) (this.getStandingY() + Game1.tileSize + 1) / 10000f));
      }
      else
      {
        this.sprite.CurrentFrame = this.offset;
        b.Draw(this.Sprite.Texture, this.getLocalPosition(Game1.viewport) + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2)), new Rectangle?(this.sprite.SourceRect), Color.White, this.rotation, new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2)), 1f, SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.991f : (float) (this.getStandingY() + Game1.tileSize / 2) / 10000f));
      }
    }
  }
}
