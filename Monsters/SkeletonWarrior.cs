// Decompiled with JetBrains decompiler
// Type: StardewValley.Monsters.SkeletonWarrior
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley.Monsters
{
  public class SkeletonWarrior : Monster
  {
    public SkeletonWarrior()
    {
    }

    public SkeletonWarrior(Vector2 position)
      : base("Skeleton Warrior", position)
    {
      this.slipperiness = 1;
    }

    public override void reloadSprite()
    {
      this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\Monsters\\Skeleton Warrior"));
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
        if (this.sprite.CurrentFrame == 16 && (double) Game1.player.position.Y > (double) this.position.Y + (double) (Game1.tileSize / 2) || this.sprite.CurrentFrame == 17 && (double) Game1.player.position.X > (double) this.position.X + (double) (Game1.tileSize / 2) || (this.sprite.CurrentFrame == 18 && (double) Game1.player.position.Y < (double) this.position.Y - (double) Game1.tileSize || this.sprite.CurrentFrame == 19 && (double) Game1.player.position.X < (double) this.position.X - (double) (Game1.tileSize / 2)))
        {
          num = 0;
          Game1.playSound("crafting");
        }
        if (Game1.random.NextDouble() < 0.25)
        {
          this.Halt();
          num = 0;
          Game1.playSound("crafting");
          switch (this.facingDirection)
          {
            case 0:
              this.sprite.CurrentFrame = 18;
              break;
            case 1:
              this.sprite.CurrentFrame = 17;
              break;
            case 2:
              this.sprite.CurrentFrame = 16;
              break;
            case 3:
              this.sprite.CurrentFrame = 19;
              break;
          }
          this.timeBeforeAIMovementAgain = 400f;
        }
        this.health = this.health - num;
        if (num > 0 && Game1.player.CurrentTool.Name.Equals("Holy Sword") && !isBomb)
        {
          this.health = this.health - damage * 3 / 4;
          Game1.currentLocation.debris.Add(new Debris(string.Concat((object) (damage * 3 / 4)), 1, new Vector2((float) this.getStandingX(), (float) this.getStandingY()), Color.LightBlue, 1f, 0.0f));
        }
        if (num > 0)
          this.setTrajectory(xTrajectory, yTrajectory);
      }
      if (this.health <= 0)
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
      return num;
    }

    public override void behaviorAtGameTick(GameTime time)
    {
      base.behaviorAtGameTick(time);
      if (!this.withinPlayerThreshold())
        return;
      if (Game1.random.NextDouble() < 0.005)
        this.willDestroyObjectsUnderfoot = true;
      if (Game1.random.NextDouble() < 0.01)
        this.willDestroyObjectsUnderfoot = false;
      if (!this.withinPlayerThreshold(2) || Game1.random.NextDouble() >= 0.01)
        return;
      Game1.playSound("swordswipe");
      switch (this.facingDirection)
      {
        case 0:
          Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(-1, 25f, 5, 1, new Vector2(this.position.X + (float) (Game1.tileSize / 2), this.position.Y - (float) (Game1.tileSize / 4)), false, false, true, -1.570796f));
          break;
        case 1:
          Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(-1, 25f, 5, 1, new Vector2(this.position.X + (float) Game1.tileSize, this.position.Y + (float) (Game1.tileSize * 3 / 4)), false, false));
          break;
        case 2:
          Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(-1, 25f, 5, 1, new Vector2(this.position.X + (float) (Game1.tileSize / 2), this.position.Y + (float) (Game1.tileSize * 3 / 2)), false, false, true, 1.570796f));
          break;
        case 3:
          Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(-1, 25f, 5, 1, new Vector2(this.position.X - (float) (Game1.tileSize / 4), this.position.Y + (float) (Game1.tileSize * 3 / 4)), false, true));
          break;
      }
      int x = (int) this.GetToolLocation(false).X;
      int y = (int) this.GetToolLocation(false).Y;
      Rectangle position = Rectangle.Empty;
      Rectangle boundingBox = this.GetBoundingBox();
      switch (this.facingDirection)
      {
        case 0:
          position = new Rectangle(x - Game1.tileSize, boundingBox.Y - Game1.tileSize, Game1.tileSize * 2, Game1.tileSize);
          break;
        case 1:
          position = new Rectangle(boundingBox.Right, y - Game1.tileSize, Game1.tileSize, Game1.tileSize * 2);
          break;
        case 2:
          position = new Rectangle(x - Game1.tileSize, boundingBox.Bottom, Game1.tileSize * 2, Game1.tileSize);
          break;
        case 3:
          position = new Rectangle(boundingBox.Left - Game1.tileSize, y - Game1.tileSize, Game1.tileSize, Game1.tileSize * 2);
          break;
      }
      Game1.currentLocation.isCollidingPosition(position, Game1.viewport, false, Game1.random.Next(2, 10), true);
    }
  }
}
