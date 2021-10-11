// Decompiled with JetBrains decompiler
// Type: StardewValley.Monsters.GoblinPeasant
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Tools;
using System;
using System.Collections.Generic;

namespace StardewValley.Monsters
{
  public class GoblinPeasant : Monster
  {
    public Rectangle hallway = Rectangle.Empty;
    private MeleeWeapon weapon = new MeleeWeapon(Game1.random.NextDouble() < 0.01 ? 22 : 16);
    public const int durationOfDaggerThrust = 200;
    public const int huntForPlayerUpdateTicks = 2000;
    public const int distanceToStopChasing = 13;
    public const int visionDistance = 8;
    private bool spottedPlayer;
    private bool actionGoblin;
    private bool attacking;
    private int controllerCountdown;
    private int actionCountdown;
    private Color clothesColor;
    private Color gogglesColor;
    private Color skinColor;

    public GoblinPeasant()
    {
      this.pickColors();
    }

    public GoblinPeasant(Vector2 position)
      : this(position, 2)
    {
    }

    public GoblinPeasant(Vector2 position, int facingDir, Rectangle hallway)
      : base("Goblin Warrior", position, facingDir)
    {
      this.facingDirection = facingDir;
      this.faceDirection(facingDir);
      this.sprite.faceDirection(facingDir);
      this.hallway = new Rectangle(hallway.X * Game1.tileSize, hallway.Y * Game1.tileSize, hallway.Width * Game1.tileSize, hallway.Height * Game1.tileSize);
      this.hallway.Inflate(Game1.tileSize / 2, Game1.tileSize / 2);
      this.moveTowardPlayerThreshold = 8;
      this.IsWalkingTowardPlayer = false;
      this.pickColors();
    }

    public GoblinPeasant(Vector2 position, int facingDirection)
      : this(position, facingDirection, false)
    {
      this.pickColors();
    }

    public GoblinPeasant(Vector2 position, int facingDir, bool actionGoblin)
      : base("Goblin Warrior", position, facingDir)
    {
      this.facingDirection = facingDir;
      this.IsWalkingTowardPlayer = false;
      this.faceDirection(facingDir);
      this.sprite.faceDirection(facingDir);
      this.moveTowardPlayerThreshold = 8;
      this.actionGoblin = actionGoblin;
      this.pickColors();
    }

    public override void reloadSprite()
    {
      base.reloadSprite();
      this.sprite.spriteHeight = Game1.tileSize * 3 / 2;
      this.sprite.UpdateSourceRect();
    }

    private void pickColors()
    {
      this.clothesColor = new Color(Game1.random.Next(80, 256), Game1.random.Next(80, 256), Game1.random.Next(80, 256));
      this.gogglesColor = new Color(Game1.random.Next(50, 256), Game1.random.Next(50, 256), Game1.random.Next(50, 256));
      this.skinColor = new Color(Game1.random.Next(100, Game1.random.NextDouble() < 0.08 ? 256 : 150), Game1.random.Next(100, Game1.random.NextDouble() < 0.08 ? 256 : 190), Game1.random.Next(100, Game1.random.NextDouble() < 0.08 ? 256 : 190));
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
        if (!this.spottedPlayer)
        {
          this.controller = (PathFindController) null;
          this.spottedPlayer = true;
          this.Halt();
          this.facePlayer(Game1.player);
          this.doEmote(16, false);
          Game1.playSound("goblinSpot");
          this.IsWalkingTowardPlayer = true;
          num *= 3;
          Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:GoblinPeasant.cs.12264"), Color.Yellow, 3500f));
        }
        this.health = this.health - num;
        this.setTrajectory(xTrajectory, yTrajectory);
        if (this.health <= 0)
        {
          this.deathAnimation();
          Game1.playSound("goblinDie");
          if (this.weapon.indexOfMenuItemView == 22 || Game1.random.NextDouble() < 0.18)
          {
            List<Debris> debris1 = Game1.currentLocation.debris;
            MeleeWeapon weapon = this.weapon;
            Rectangle boundingBox = this.GetBoundingBox();
            double x = (double) boundingBox.Center.X;
            boundingBox = this.GetBoundingBox();
            double y = (double) boundingBox.Center.Y;
            Vector2 debrisOrigin = new Vector2((float) x, (float) y);
            Debris debris2 = new Debris((Item) weapon, debrisOrigin);
            debris1.Add(debris2);
          }
        }
        else
          Game1.playSound("goblinHurt");
      }
      return num;
    }

    public override void deathAnimation()
    {
      Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(this.sprite.Texture, new Rectangle(0, Game1.tileSize * 3 / 2 * 5, Game1.tileSize, Game1.tileSize * 3 / 2), (float) Game1.random.Next(150, 200), 4, 0, this.position, false, false));
    }

    public void meetUpWithNeighborEndFunction(Character c, GameLocation location)
    {
      List<Vector2> adjacentTileLocations = Utility.getAdjacentTileLocations(this.getTileLocation());
      bool flag = false;
      foreach (Vector2 vector2 in adjacentTileLocations)
      {
        foreach (Character character in location.characters)
        {
          if (character.getTileLocation().Equals(vector2))
          {
            character.faceGeneralDirection(this.getTileLocation(), 0);
            this.faceGeneralDirection(vector2, 0);
            flag = true;
          }
        }
      }
      if (!flag)
        return;
      this.doEmote(Game1.random.NextDouble() < 0.5 ? 20 : (Game1.random.NextDouble() < 0.5 ? 32 : 8), true);
    }

    private void tryToMeetUpWithNeighbor()
    {
      Character character = (Character) Game1.currentLocation.characters[Game1.random.Next(Game1.currentLocation.characters.Count)];
      if (this.Equals((object) character) || character.controller == null || (!(character is GoblinPeasant) || ((GoblinPeasant) character).hallway.Equals(Rectangle.Empty)))
        return;
      Vector2 adjacentOpenTile = Utility.getRandomAdjacentOpenTile(character.getTileLocation());
      if (adjacentOpenTile.Equals(Vector2.Zero))
        return;
      this.controller = new PathFindController((Character) this, Game1.currentLocation, new Point((int) adjacentOpenTile.X, (int) adjacentOpenTile.Y), Game1.random.Next(4), new PathFindController.endBehavior(this.meetUpWithNeighborEndFunction));
    }

    public override void draw(SpriteBatch b)
    {
      this.sprite.draw(b, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2(0.0f, (float) (-Game1.tileSize * 4 / 5))), (float) this.GetBoundingBox().Center.Y / 10000f, 0, 0, this.skinColor, false, (float) Game1.pixelZoom, 0.0f, false);
      if (this.attacking)
        this.weapon.drawDuringUse((200 - this.actionCountdown) / 100, this.FacingDirection, b, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2(0.0f, (float) (-Game1.tileSize * 4 / 5))) + new Vector2(this.FacingDirection == 1 ? -16f : (this.FacingDirection == 3 ? 16f : 0.0f), (float) (Game1.tileSize + (this.FacingDirection == 1 || this.FacingDirection == 3 ? Game1.tileSize / 2 : 0))), Game1.player);
      this.sprite.draw(b, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2(0.0f, (float) (-Game1.tileSize * 4 / 5))), (float) ((double) this.GetBoundingBox().Center.Y / 10000.0 + 9.99999974737875E-06), 0, 144, this.clothesColor, false, (float) Game1.pixelZoom, 0.0f, false);
      if ((int) this.clothesColor.R % 2 != 0 && (int) this.clothesColor.G % 2 != 0 && (int) this.clothesColor.B % 2 != 0)
        return;
      this.sprite.draw(b, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2(0.0f, (float) (-Game1.tileSize * 4 / 5))), (float) ((double) this.GetBoundingBox().Center.Y / 10000.0 + 9.99999997475243E-07), 0, 264, this.gogglesColor, false, (float) Game1.pixelZoom, 0.0f, false);
    }

    public override void update(GameTime time, GameLocation location)
    {
      if (!this.attacking)
        base.update(time, location);
      else
        this.behaviorAtGameTick(time);
    }

    public override void behaviorAtGameTick(GameTime time)
    {
      base.behaviorAtGameTick(time);
      if (!this.spottedPlayer && this.controller == null)
      {
        if (this.actionGoblin)
        {
          this.actionCountdown = this.actionCountdown - time.ElapsedGameTime.Milliseconds;
          if (this.actionCountdown <= 0)
          {
            if (this.sprite.CurrentFrame >= 16)
            {
              this.actionCountdown = Game1.random.Next(500, 2000);
              this.sprite.CurrentFrame = (this.sprite.CurrentFrame - 16) * 4;
              this.sprite.UpdateSourceRect();
            }
            else
            {
              this.sprite.CurrentFrame = 16 + (this.facingDirection % 2 == 0 ? (this.facingDirection + 2) % 4 : this.facingDirection);
              this.sprite.UpdateSourceRect();
              this.actionCountdown = Game1.random.Next(100, 500);
              if (Utility.isOnScreen(this.position, Game1.tileSize * 4))
              {
                Game1.playSound("hammer");
                Game1.createRadialDebris(Game1.currentLocation, 14, (int) this.getTileLocation().X + (this.facingDirection == 1 ? 1 : (this.facingDirection == 3 ? -1 : 0)), (int) this.getTileLocation().Y + (this.facingDirection == 0 ? -1 : (this.facingDirection == 2 ? 1 : 0)), Game1.random.Next(4, 6), false, -1, false, -1);
              }
            }
          }
        }
        else if (!this.hallway.Equals(Rectangle.Empty))
        {
          if (this.hallway.Contains(this.GetBoundingBox().Center))
          {
            if (this.hallway.Height > this.hallway.Width && (this.facingDirection == 1 || this.facingDirection == 3))
              this.facingDirection = Game1.random.NextDouble() < 0.5 ? 0 : 2;
            else if (this.hallway.Width > this.hallway.Height && (this.facingDirection == 0 || this.facingDirection == 2))
              this.facingDirection = Game1.random.NextDouble() < 0.5 ? 1 : 3;
            this.setMovingInFacingDirection();
          }
          else
            this.controller = new PathFindController((Character) this, Game1.currentLocation, new Point(this.hallway.Center.X / Game1.tileSize, this.hallway.Center.Y / Game1.tileSize), Game1.random.Next(4));
          if (Game1.currentLocation.isCollidingPosition(this.nextPosition(this.facingDirection), Game1.viewport, false, 0, false, (Character) this))
          {
            this.faceDirection((this.facingDirection + 2) % 4);
            this.setMovingInFacingDirection();
            this.MovePosition(time, Game1.viewport, Game1.currentLocation);
            this.MovePosition(time, Game1.viewport, Game1.currentLocation);
          }
        }
        else if (Game1.random.NextDouble() < 1.0 / 400.0)
          this.tryToMeetUpWithNeighbor();
        else
          Game1.random.NextDouble();
      }
      if (!this.spottedPlayer && Utility.couldSeePlayerInPeripheralVision((Character) this) && Utility.doesPointHaveLineOfSightInMine(this.getTileLocation(), Game1.player.getTileLocation(), 8))
      {
        this.controller = (PathFindController) null;
        this.spottedPlayer = true;
        this.Halt();
        this.facePlayer(Game1.player);
        this.doEmote(16, false);
        Game1.playSound("goblinSpot");
        this.IsWalkingTowardPlayer = true;
        this.actionGoblin = false;
      }
      else if (this.spottedPlayer && this.withinPlayerThreshold(13) || this.attacking)
      {
        if (!this.withinPlayerThreshold(2) && !this.attacking)
          return;
        if (!this.attacking && Game1.random.NextDouble() < 0.04)
        {
          this.actionCountdown = 200;
          this.attacking = true;
          Game1.playSound("daggerswipe");
          Vector2 zero1 = Vector2.Zero;
          Vector2 zero2 = Vector2.Zero;
          Rectangle areaOfEffect = this.weapon.getAreaOfEffect((int) this.GetToolLocation(false).X, (int) this.GetToolLocation(false).Y, this.getFacingDirection(), ref zero1, ref zero2, this.GetBoundingBox(), 0);
          if (this.facingDirection == 1 || this.facingDirection == 3)
            areaOfEffect.Inflate(-12, -12);
          if (!areaOfEffect.Intersects(Game1.player.GetBoundingBox()))
            return;
          int health = Game1.player.health;
          Game1.farmerTakeDamage(Game1.random.Next(this.weapon.minDamage, this.weapon.maxDamage + 1), false, (Monster) this);
          if (Game1.player.health != health)
            return;
          this.setTrajectory(Utility.getAwayFromPlayerTrajectory(this.GetBoundingBox()) / 2f);
        }
        else
        {
          if (!this.attacking)
            return;
          this.Halt();
          this.sprite.CurrentFrame = 16 + (this.facingDirection == 0 ? 2 : (this.facingDirection == 2 ? 0 : this.facingDirection));
          this.actionCountdown = this.actionCountdown - time.ElapsedGameTime.Milliseconds;
          if (this.actionCountdown > 0)
            return;
          this.attacking = false;
          this.sprite.CurrentFrame = (this.sprite.CurrentFrame - 16) * 4;
        }
      }
      else
      {
        if (!this.spottedPlayer)
          return;
        this.IsWalkingTowardPlayer = false;
        this.spottedPlayer = false;
        this.controllerCountdown = 0;
        this.controller = (PathFindController) null;
        this.Halt();
        this.tryToMeetUpWithNeighbor();
        this.addedSpeed = 0;
        this.doEmote(8, false);
      }
    }
  }
}
