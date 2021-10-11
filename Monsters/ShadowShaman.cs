// Decompiled with JetBrains decompiler
// Type: StardewValley.Monsters.ShadowShaman
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Projectiles;
using System;
using System.Collections.Generic;

namespace StardewValley.Monsters
{
  public class ShadowShaman : Monster
  {
    private int coolDown = 1500;
    public const int visionDistance = 8;
    public const int spellCooldown = 1500;
    private bool spottedPlayer;
    private bool casting;
    private float rotationTimer;

    public ShadowShaman()
    {
    }

    public ShadowShaman(Vector2 position)
      : base("Shadow Shaman", position)
    {
      if (!Game1.player.friendships.ContainsKey("???") || Game1.player.friendships["???"][0] < 1250)
        return;
      this.damageToFarmer = 0;
    }

    public override void reloadSprite()
    {
      this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Monsters\\Shadow Shaman"));
    }

    public override void draw(SpriteBatch b)
    {
      base.draw(b);
      if (!this.casting)
        return;
      for (int index = 0; index < 8; ++index)
        b.Draw(Projectile.projectileSheet, Game1.GlobalToLocal(Game1.viewport, this.getStandingPosition()), new Rectangle?(new Rectangle(119, 6, 3, 3)), Color.White * 0.7f, this.rotationTimer + (float) ((double) index * 3.14159274101257 / 4.0), new Vector2(8f, 48f), 1.5f * (float) Game1.pixelZoom, SpriteEffects.None, 0.95f);
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
        if (Game1.player.CurrentTool != null && Game1.player.CurrentTool.Name != null && (Game1.player.CurrentTool.Name.Equals("Holy Sword") && !isBomb))
        {
          this.health = this.health - damage * 3 / 4;
          Game1.currentLocation.debris.Add(new Debris(string.Concat((object) (damage * 3 / 4)), 1, new Vector2((float) this.getStandingX(), (float) this.getStandingY()), Color.LightBlue, 1f, 0.0f));
        }
        this.health = this.health - num;
        if (this.casting && Game1.random.NextDouble() < 0.5)
        {
          this.coolDown = this.coolDown + 200;
        }
        else
        {
          this.setTrajectory(xTrajectory, yTrajectory);
          Game1.playSound("shadowHit");
        }
        if (this.health <= 0)
        {
          Game1.playSound("shadowDie");
          this.deathAnimation();
        }
      }
      return num;
    }

    public override void deathAnimation()
    {
      Utility.makeTemporarySpriteJuicier(new TemporaryAnimatedSprite(45, this.position, Color.White, 10, false, 100f, 0, -1, -1f, -1, 0), Game1.currentLocation, 4, 64, 64);
      Game1.createRadialDebris(Game1.currentLocation, this.sprite.Texture, new Rectangle(this.sprite.SourceRect.X, this.sprite.SourceRect.Y, 16, 5), 16, this.getStandingX(), this.getStandingY() - Game1.tileSize / 2, 1, this.getStandingY() / Game1.tileSize, Color.White);
      Game1.createRadialDebris(Game1.currentLocation, this.sprite.Texture, new Rectangle(this.sprite.SourceRect.X + 2, this.sprite.SourceRect.Y + 5, 16, 5), 10, this.getStandingX(), this.getStandingY() - Game1.tileSize / 2, 1, this.getStandingY() / Game1.tileSize, Color.White);
      Game1.createRadialDebris(Game1.currentLocation, this.sprite.Texture, new Rectangle(0, 10, 16, 5), 16, this.getStandingX(), this.getStandingY() - Game1.tileSize / 2, 1, this.getStandingY() / Game1.tileSize, Color.White);
      for (int index = 1; index < 3; ++index)
      {
        Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(6, this.position + new Vector2(1f, 1f) * (float) Game1.tileSize * (float) index, Color.Gray * 0.75f, 10, false, 100f, 0, -1, -1f, -1, 0)
        {
          delayBeforeAnimationStart = index * 159
        });
        Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(6, this.position + new Vector2(1f, -1f) * (float) Game1.tileSize * (float) index, Color.Gray * 0.75f, 10, false, 100f, 0, -1, -1f, -1, 0)
        {
          delayBeforeAnimationStart = index * 159
        });
        Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(6, this.position + new Vector2(-1f, 1f) * (float) Game1.tileSize * (float) index, Color.Gray * 0.75f, 10, false, 100f, 0, -1, -1f, -1, 0)
        {
          delayBeforeAnimationStart = index * 159
        });
        Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(6, this.position + new Vector2(-1f, -1f) * (float) Game1.tileSize * (float) index, Color.Gray * 0.75f, 10, false, 100f, 0, -1, -1f, -1, 0)
        {
          delayBeforeAnimationStart = index * 159
        });
      }
    }

    public override void behaviorAtGameTick(GameTime time)
    {
      base.behaviorAtGameTick(time);
      if ((double) this.timeBeforeAIMovementAgain <= 0.0)
        this.isInvisible = false;
      if (!this.spottedPlayer && Utility.couldSeePlayerInPeripheralVision((Character) this) && Utility.doesPointHaveLineOfSightInMine(this.getTileLocation(), Game1.player.getTileLocation(), 8))
      {
        this.controller = (PathFindController) null;
        this.spottedPlayer = true;
        this.Halt();
        this.facePlayer(Game1.player);
        if (Game1.random.NextDouble() >= 0.3)
          return;
        Game1.playSound("shadowpeep");
      }
      else if (this.casting)
      {
        this.IsWalkingTowardPlayer = false;
        this.sprite.Animate(time, 16, 4, 200f);
        TimeSpan timeSpan = time.TotalGameTime;
        this.rotationTimer = (float) ((double) timeSpan.Milliseconds * 0.0245436932891607 / 24.0 % (1024.0 * Math.PI));
        int coolDown = this.coolDown;
        timeSpan = time.ElapsedGameTime;
        int milliseconds = timeSpan.Milliseconds;
        this.coolDown = coolDown - milliseconds;
        if (this.coolDown > 0)
          return;
        this.scale = 1f;
        Vector2 velocityTowardPlayer = Utility.getVelocityTowardPlayer(this.GetBoundingBox().Center, 15f, Game1.player);
        if (Game1.player.attack >= 0 && Game1.random.NextDouble() < 0.6)
        {
          Game1.currentLocation.projectiles.Add((Projectile) new DebuffingProjectile(new Buff(14), 7, 4, 4, 0.1963495f, velocityTowardPlayer.X, velocityTowardPlayer.Y, new Vector2((float) this.GetBoundingBox().X, (float) this.GetBoundingBox().Y), (Character) this));
        }
        else
        {
          List<Monster> monsterList = new List<Monster>();
          foreach (NPC character in Game1.currentLocation.characters)
          {
            if (character is Monster && (character as Monster).withinPlayerThreshold(6))
              monsterList.Add((Monster) character);
          }
          Monster monster1 = (Monster) null;
          double num = 1.0;
          foreach (Monster monster2 in monsterList)
          {
            if ((double) monster2.health / (double) monster2.maxHealth <= num)
            {
              monster1 = monster2;
              num = (double) monster2.health / (double) monster2.maxHealth;
            }
          }
          if (monster1 != null)
          {
            int number = 60;
            monster1.health = Math.Min(monster1.maxHealth, monster1.health + number);
            Game1.playSound("healSound");
            Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 256, 64, 64), 40f, 8, 0, monster1.position + new Vector2((float) (Game1.tileSize / 2), (float) Game1.tileSize), false, false));
            Game1.currentLocation.debris.Add(new Debris(number, new Vector2((float) monster1.GetBoundingBox().Center.X, (float) monster1.GetBoundingBox().Center.Y), Color.Green, 1f, (Character) monster1));
          }
        }
        this.casting = false;
        this.coolDown = 1500;
        this.IsWalkingTowardPlayer = true;
      }
      else if (this.spottedPlayer && this.withinPlayerThreshold(8))
      {
        if (this.health < 30)
        {
          int y1 = Game1.player.GetBoundingBox().Center.Y;
          Rectangle boundingBox = this.GetBoundingBox();
          int y2 = boundingBox.Center.Y;
          if (Math.Abs(y1 - y2) > Game1.tileSize * 3)
          {
            boundingBox = Game1.player.GetBoundingBox();
            int x1 = boundingBox.Center.X;
            boundingBox = this.GetBoundingBox();
            int x2 = boundingBox.Center.X;
            if (x1 - x2 > 0)
              this.SetMovingLeft(true);
            else
              this.SetMovingRight(true);
          }
          else
          {
            boundingBox = Game1.player.GetBoundingBox();
            int y3 = boundingBox.Center.Y;
            boundingBox = this.GetBoundingBox();
            int y4 = boundingBox.Center.Y;
            if (y3 - y4 > 0)
              this.SetMovingUp(true);
            else
              this.SetMovingDown(true);
          }
        }
        else if (this.controller == null && !Utility.doesPointHaveLineOfSightInMine(this.getTileLocation(), Game1.player.getTileLocation(), 8))
        {
          this.controller = new PathFindController((Character) this, Game1.currentLocation, new Point((int) Game1.player.getTileLocation().X, (int) Game1.player.getTileLocation().Y), -1, (PathFindController.endBehavior) null, 300);
          if (this.controller == null || this.controller.pathToEndPoint == null || this.controller.pathToEndPoint.Count == 0)
          {
            this.spottedPlayer = false;
            this.Halt();
            this.controller = (PathFindController) null;
            this.addedSpeed = 0;
          }
        }
        else if (this.coolDown <= 0 && Game1.random.NextDouble() < 0.02)
        {
          this.casting = true;
          this.Halt();
          this.coolDown = 500;
        }
        this.coolDown = this.coolDown - time.ElapsedGameTime.Milliseconds;
      }
      else if (this.spottedPlayer)
      {
        this.IsWalkingTowardPlayer = false;
        this.spottedPlayer = false;
        this.controller = (PathFindController) null;
        this.addedSpeed = 0;
      }
      else
        this.defaultMovementBehavior(time);
    }
  }
}
