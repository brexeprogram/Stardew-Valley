// Decompiled with JetBrains decompiler
// Type: StardewValley.Monsters.Skeleton
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Projectiles;

namespace StardewValley.Monsters
{
  public class Skeleton : Monster
  {
    private bool spottedPlayer;
    private bool throwing;
    private int controllerAttemptTimer;

    public Skeleton()
    {
    }

    public Skeleton(Vector2 position)
      : base(nameof (Skeleton), position, Game1.random.Next(4))
    {
      this.sprite.spriteHeight = 32;
      this.sprite.UpdateSourceRect();
      this.IsWalkingTowardPlayer = false;
      this.jitteriness = 0.0;
    }

    public override void reloadSprite()
    {
      this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Monsters\\Skeleton"));
      this.sprite.spriteHeight = 32;
      this.sprite.UpdateSourceRect();
    }

    public override int takeDamage(int damage, int xTrajectory, int yTrajectory, bool isBomb, double addedPrecision)
    {
      Game1.playSound("skeletonHit");
      this.slipperiness = 3;
      if (this.throwing)
      {
        this.throwing = false;
        this.Halt();
      }
      if (this.health - damage <= 0)
      {
        Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(46, this.position, Color.White, 10, false, 70f, 0, -1, -1f, -1, 0));
        Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(46, this.position + new Vector2((float) (-Game1.tileSize / 4), 0.0f), Color.White, 10, false, 70f, 0, -1, -1f, -1, 0)
        {
          delayBeforeAnimationStart = 100
        });
        Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(46, this.position + new Vector2((float) (Game1.tileSize / 4), 0.0f), Color.White, 10, false, 70f, 0, -1, -1f, -1, 0)
        {
          delayBeforeAnimationStart = 200
        });
      }
      return base.takeDamage(damage, xTrajectory, yTrajectory, isBomb, addedPrecision);
    }

    public override void shedChunks(int number)
    {
      GameLocation currentLocation = Game1.currentLocation;
      Texture2D texture = this.sprite.Texture;
      Rectangle sourcerectangle = new Rectangle(0, 128, 16, 16);
      int sizeOfSourceRectSquares = 8;
      Rectangle boundingBox = this.GetBoundingBox();
      int x = boundingBox.Center.X;
      boundingBox = this.GetBoundingBox();
      int y1 = boundingBox.Center.Y;
      int numberOfChunks = number;
      int y2 = (int) this.getTileLocation().Y;
      Color white = Color.White;
      double pixelZoom = (double) Game1.pixelZoom;
      Game1.createRadialDebris(currentLocation, texture, sourcerectangle, sizeOfSourceRectSquares, x, y1, numberOfChunks, y2, white, (float) pixelZoom);
    }

    public override void deathAnimation()
    {
      Game1.playSound("skeletonDie");
      Game1.random.Next(5, 13);
      this.shedChunks(20);
      Game1.createRadialDebris(Game1.currentLocation, this.sprite.Texture, new Rectangle(3, Game1.random.NextDouble() < 0.5 ? 3 : 35, 10, 10), 11, this.GetBoundingBox().Center.X, this.GetBoundingBox().Center.Y, 1, (int) this.getTileLocation().Y, Color.White, (float) Game1.pixelZoom);
    }

    public override void update(GameTime time, GameLocation location)
    {
      if (!this.throwing)
        base.update(time, location);
      else
        this.behaviorAtGameTick(time);
    }

    public override void behaviorAtGameTick(GameTime time)
    {
      if (!this.throwing)
        base.behaviorAtGameTick(time);
      if (!this.spottedPlayer && !this.wildernessFarmMonster && Utility.doesPointHaveLineOfSightInMine(this.getTileLocation(), Game1.player.getTileLocation(), 8))
      {
        this.controller = new PathFindController((Character) this, Game1.currentLocation, new Point(Game1.player.getStandingX() / Game1.tileSize, Game1.player.getStandingY() / Game1.tileSize), Game1.random.Next(4), (PathFindController.endBehavior) null, 200);
        this.spottedPlayer = true;
        this.Halt();
        this.facePlayer(Game1.player);
        Game1.playSound("skeletonStep");
        this.IsWalkingTowardPlayer = true;
      }
      else if (this.throwing)
      {
        if (this.invincibleCountdown > 0)
        {
          this.invincibleCountdown = this.invincibleCountdown - time.ElapsedGameTime.Milliseconds;
          if (this.invincibleCountdown <= 0)
            this.stopGlowing();
        }
        this.sprite.Animate(time, 20, 5, 150f);
        if (this.sprite.currentFrame == 24)
        {
          this.throwing = false;
          this.sprite.CurrentFrame = 0;
          this.faceDirection(2);
          Vector2 velocityTowardPlayer = Utility.getVelocityTowardPlayer(new Point((int) this.position.X, (int) this.position.Y), 8f, Game1.player);
          Game1.currentLocation.projectiles.Add((Projectile) new BasicProjectile(this.damageToFarmer, 4, 0, 0, 0.1963495f, velocityTowardPlayer.X, velocityTowardPlayer.Y, new Vector2(this.position.X, this.position.Y), "skeletonHit", "skeletonStep", false, false, (Character) this, false, (BasicProjectile.onCollisionBehavior) null));
        }
      }
      else if (this.spottedPlayer && this.controller == null && (Game1.random.NextDouble() < 0.002 && !this.wildernessFarmMonster) && Utility.doesPointHaveLineOfSightInMine(this.getTileLocation(), Game1.player.getTileLocation(), 8))
      {
        this.throwing = true;
        this.Halt();
        this.sprite.CurrentFrame = 20;
        this.shake(750);
      }
      else if (this.withinPlayerThreshold(2))
        this.controller = (PathFindController) null;
      else if (this.spottedPlayer && this.controller == null && this.controllerAttemptTimer <= 0)
      {
        this.controller = new PathFindController((Character) this, Game1.currentLocation, new Point(Game1.player.getStandingX() / Game1.tileSize, Game1.player.getStandingY() / Game1.tileSize), Game1.random.Next(4), (PathFindController.endBehavior) null, 200);
        this.Halt();
        this.facePlayer(Game1.player);
        this.controllerAttemptTimer = this.wildernessFarmMonster ? 2000 : 1000;
      }
      else if (this.wildernessFarmMonster)
      {
        this.spottedPlayer = true;
        this.IsWalkingTowardPlayer = true;
      }
      this.controllerAttemptTimer = this.controllerAttemptTimer - time.ElapsedGameTime.Milliseconds;
    }
  }
}
