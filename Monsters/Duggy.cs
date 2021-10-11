// Decompiled with JetBrains decompiler
// Type: StardewValley.Monsters.Duggy
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using xTile.Dimensions;

namespace StardewValley.Monsters
{
  public class Duggy : Monster
  {
    private double chanceToDisappear = 0.03;
    private bool hasDugForTreasure;

    public Duggy()
    {
      this.hideShadow = true;
    }

    public Duggy(Vector2 position)
      : base(nameof (Duggy), position)
    {
      this.IsWalkingTowardPlayer = false;
      this.isInvisible = true;
      this.damageToFarmer = 0;
      this.sprite.loop = false;
      this.sprite.CurrentFrame = 0;
      this.hideShadow = true;
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
        Game1.playSound("hitEnemy");
        if (this.health <= 0)
          this.deathAnimation();
      }
      return num;
    }

    public override void deathAnimation()
    {
      Game1.playSound("monsterdead");
      TemporaryAnimatedSprite t = new TemporaryAnimatedSprite(44, this.position, Color.DarkRed, 10, false, 100f, 0, -1, -1f, -1, 0);
      t.holdLastFrame = true;
      t.alphaFade = 0.01f;
      t.interval = 70f;
      GameLocation currentLocation = Game1.currentLocation;
      int numAddOns = 4;
      int xRange = 64;
      int yRange = 64;
      Utility.makeTemporarySpriteJuicier(t, currentLocation, numAddOns, xRange, yRange);
    }

    public override void update(GameTime time, GameLocation location)
    {
      if (this.invincibleCountdown > 0)
      {
        this.glowingColor = Color.Cyan;
        this.invincibleCountdown = this.invincibleCountdown - time.ElapsedGameTime.Milliseconds;
        if (this.invincibleCountdown <= 0)
          this.stopGlowing();
      }
      if (!location.Equals((object) Game1.currentLocation))
        return;
      this.behaviorAtGameTick(time);
      if ((double) this.position.X < 0.0 || (double) this.position.X > (double) (location.map.GetLayer("Back").LayerWidth * Game1.tileSize) || ((double) this.position.Y < 0.0 || (double) this.position.Y > (double) (location.map.GetLayer("Back").LayerHeight * Game1.tileSize)))
        location.characters.Remove((NPC) this);
      this.updateGlow();
    }

    public override void draw(SpriteBatch b)
    {
      if (this.isInvisible || !Utility.isOnScreen(this.position, 2 * Game1.tileSize))
        return;
      b.Draw(this.Sprite.Texture, this.getLocalPosition(Game1.viewport) + new Vector2((float) (Game1.tileSize / 2), (float) (this.GetBoundingBox().Height / 2 + this.yJumpOffset)), new Microsoft.Xna.Framework.Rectangle?(this.Sprite.SourceRect), Color.White, this.rotation, new Vector2(8f, 16f), Math.Max(0.2f, this.scale) * (float) Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.991f : (float) this.getStandingY() / 10000f));
      if (!this.isGlowing)
        return;
      b.Draw(this.Sprite.Texture, this.getLocalPosition(Game1.viewport) + new Vector2((float) (Game1.tileSize / 2), (float) (this.GetBoundingBox().Height / 2 + this.yJumpOffset)), new Microsoft.Xna.Framework.Rectangle?(this.Sprite.SourceRect), this.glowingColor * this.glowingTransparency, this.rotation, new Vector2(8f, 16f), Math.Max(0.2f, this.scale) * (float) Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0.0f, this.drawOnTop ? 0.991f : (float) ((double) this.getStandingY() / 10000.0 + 1.0 / 1000.0)));
    }

    public override void behaviorAtGameTick(GameTime time)
    {
      base.behaviorAtGameTick(time);
      this.isEmoting = false;
      Microsoft.Xna.Framework.Rectangle boundingBox = this.GetBoundingBox();
      if (this.sprite.currentFrame < 4)
      {
        boundingBox.Inflate(Game1.tileSize * 2, Game1.tileSize * 2);
        if (!this.isInvisible || boundingBox.Contains(Game1.player.getStandingX(), Game1.player.getStandingY()))
        {
          if (this.isInvisible)
          {
            if (Game1.currentLocation.map.GetLayer("Back").Tiles[(int) Game1.player.getTileLocation().X, (int) Game1.player.getTileLocation().Y].Properties.ContainsKey("NPCBarrier") || !Game1.currentLocation.map.GetLayer("Back").Tiles[(int) Game1.player.getTileLocation().X, (int) Game1.player.getTileLocation().Y].TileIndexProperties.ContainsKey("Diggable") && Game1.currentLocation.map.GetLayer("Back").Tiles[(int) Game1.player.getTileLocation().X, (int) Game1.player.getTileLocation().Y].TileIndex != 0)
              return;
            this.position = new Vector2(Game1.player.position.X, Game1.player.position.Y + (float) Game1.player.sprite.spriteHeight - (float) this.sprite.spriteHeight);
            Game1.playSound(nameof (Duggy));
            this.sprite.interval = 100f;
            this.position = Game1.player.getTileLocation() * (float) Game1.tileSize;
          }
          this.isInvisible = false;
          this.sprite.AnimateDown(time, 0, "");
        }
      }
      if (this.sprite.currentFrame >= 4 && this.sprite.CurrentFrame < 8)
      {
        if (!this.hasDugForTreasure)
          this.getTileLocation();
        boundingBox.Inflate(-Game1.tileSize * 2, -Game1.tileSize * 2);
        Game1.currentLocation.isCollidingPosition(boundingBox, Game1.viewport, false, 8, false, (Character) this);
        this.sprite.AnimateRight(time, 0, "");
        this.sprite.interval = 220f;
      }
      if (this.sprite.currentFrame >= 8)
        this.sprite.AnimateUp(time, 0, "");
      if (this.sprite.currentFrame < 10)
        return;
      this.isInvisible = true;
      this.sprite.currentFrame = 0;
      this.hasDugForTreasure = false;
      int num = 0;
      Vector2 tileLocation1 = this.getTileLocation();
      Game1.currentLocation.map.GetLayer("Back").Tiles[(int) tileLocation1.X, (int) tileLocation1.Y].TileIndex = 0;
      Game1.currentLocation.removeEverythingExceptCharactersFromThisTile((int) tileLocation1.X, (int) tileLocation1.Y);
      for (Vector2 tileLocation2 = new Vector2((float) (Game1.player.GetBoundingBox().Center.X / Game1.tileSize + Game1.random.Next(-12, 12)), (float) (Game1.player.GetBoundingBox().Center.Y / Game1.tileSize + Game1.random.Next(-12, 12))); num < 4 && ((double) tileLocation2.X <= 0.0 || (double) tileLocation2.X >= (double) Game1.currentLocation.map.Layers[0].LayerWidth || ((double) tileLocation2.Y <= 0.0 || (double) tileLocation2.Y >= (double) Game1.currentLocation.map.Layers[0].LayerHeight) || (Game1.currentLocation.map.GetLayer("Back").Tiles[(int) tileLocation2.X, (int) tileLocation2.Y] == null || Game1.currentLocation.isTileOccupied(tileLocation2, "") || (!Game1.currentLocation.isTilePassable(new Location((int) tileLocation2.X, (int) tileLocation2.Y), Game1.viewport) || tileLocation2.Equals(new Vector2((float) (Game1.player.getStandingX() / Game1.tileSize), (float) (Game1.player.getStandingY() / Game1.tileSize))))) || (Game1.currentLocation.map.GetLayer("Back").Tiles[(int) tileLocation2.X, (int) tileLocation2.Y].Properties.ContainsKey("NPCBarrier") || !Game1.currentLocation.map.GetLayer("Back").Tiles[(int) tileLocation2.X, (int) tileLocation2.Y].TileIndexProperties.ContainsKey("Diggable") && Game1.currentLocation.map.GetLayer("Back").Tiles[(int) tileLocation2.X, (int) tileLocation2.Y].TileIndex != 0)); ++num)
        tileLocation2 = new Vector2((float) (Game1.player.GetBoundingBox().Center.X / Game1.tileSize + Game1.random.Next(-2, 2)), (float) (Game1.player.GetBoundingBox().Center.Y / Game1.tileSize + Game1.random.Next(-2, 2)));
    }
  }
}
