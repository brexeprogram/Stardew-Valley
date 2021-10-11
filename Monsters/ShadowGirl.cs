// Decompiled with JetBrains decompiler
// Type: StardewValley.Monsters.ShadowGirl
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley.Monsters
{
  public class ShadowGirl : Monster
  {
    private new Vector2 lastPosition = Vector2.Zero;
    public const int blockTimeBeforePathfinding = 500;
    private int howLongOnThisPosition;

    public ShadowGirl()
    {
    }

    public ShadowGirl(Vector2 position)
      : base("Shadow Girl", position)
    {
      this.IsWalkingTowardPlayer = false;
      this.moveTowardPlayerThreshold = 8;
      if (!Game1.player.friendships.ContainsKey("???") || Game1.player.friendships["???"][0] < 1250)
        return;
      this.damageToFarmer = 0;
    }

    public override void reloadSprite()
    {
      this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Monsters\\Shadow Girl"));
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
        if (Game1.player.CurrentTool.Name.Equals("Holy Sword") && !isBomb)
        {
          this.health = this.health - damage * 3 / 4;
          Game1.currentLocation.debris.Add(new Debris(string.Concat((object) (damage * 3 / 4)), 1, new Vector2((float) this.getStandingX(), (float) this.getStandingY()), Color.LightBlue, 1f, 0.0f));
        }
        this.health = this.health - num;
        this.setTrajectory(xTrajectory, yTrajectory);
        if (this.health <= 0)
          this.deathAnimation();
      }
      return num;
    }

    public override void deathAnimation()
    {
      Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(45, this.position, Color.White, 10, false, 100f, 0, -1, -1f, -1, 0));
      Game1.createRadialDebris(Game1.currentLocation, this.sprite.Texture, new Rectangle(this.sprite.SourceRect.X, this.sprite.SourceRect.Y, Game1.tileSize, Game1.tileSize / 3), Game1.tileSize, this.getStandingX(), this.getStandingY() - Game1.tileSize / 2, 1, this.getStandingY() / Game1.tileSize, Color.White);
      Game1.createRadialDebris(Game1.currentLocation, this.sprite.Texture, new Rectangle(this.sprite.SourceRect.X + Game1.tileSize / 6, this.sprite.SourceRect.Y + Game1.tileSize / 3, Game1.tileSize, Game1.tileSize / 3), Game1.tileSize * 2 / 3, this.getStandingX(), this.getStandingY() - Game1.tileSize / 2, 1, this.getStandingY() / Game1.tileSize, Color.White);
    }

    public override void update(GameTime time, GameLocation location)
    {
      if (!location.Equals((object) Game1.currentLocation))
        return;
      if (!Game1.player.isRafting || !this.withinPlayerThreshold(4))
      {
        this.updateGlow();
        this.updateEmote(time);
        if (this.controller == null)
          this.updateMovement(location, time);
        if (this.controller != null && this.controller.update(time))
          this.controller = (PathFindController) null;
      }
      this.behaviorAtGameTick(time);
      if ((double) this.position.X >= 0.0 && (double) this.position.X <= (double) (location.map.GetLayer("Back").LayerWidth * Game1.tileSize) && ((double) this.position.Y >= 0.0 && (double) this.position.Y <= (double) (location.map.GetLayer("Back").LayerHeight * Game1.tileSize)))
        return;
      location.characters.Remove((NPC) this);
    }

    public override void behaviorAtGameTick(GameTime time)
    {
      base.behaviorAtGameTick(time);
      this.addedSpeed = 0;
      this.speed = 3;
      if (this.howLongOnThisPosition > 500 && this.controller == null)
      {
        this.IsWalkingTowardPlayer = false;
        this.controller = new PathFindController((Character) this, Game1.currentLocation, new Point((int) Game1.player.getTileLocation().X, (int) Game1.player.getTileLocation().Y), Game1.random.Next(4), (PathFindController.endBehavior) null, 300);
        this.timeBeforeAIMovementAgain = 2000f;
        this.howLongOnThisPosition = 0;
      }
      else if (this.controller == null)
        this.IsWalkingTowardPlayer = true;
      this.howLongOnThisPosition = !this.position.Equals(this.lastPosition) ? 0 : this.howLongOnThisPosition + time.ElapsedGameTime.Milliseconds;
      this.lastPosition = this.position;
    }
  }
}
