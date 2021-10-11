// Decompiled with JetBrains decompiler
// Type: StardewValley.Projectiles.BasicProjectile
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using StardewValley.Monsters;
using StardewValley.TerrainFeatures;

namespace StardewValley.Projectiles
{
  public class BasicProjectile : Projectile
  {
    public int damageToFarmer;
    private string collisionSound;
    private bool explode;
    private BasicProjectile.onCollisionBehavior collisionBehavior;

    public BasicProjectile(int damageToFarmer, int parentSheetIndex, int bouncesTillDestruct, int tailLength, float rotationVelocity, float xVelocity, float yVelocity, Vector2 startingPosition, string collisionSound, string firingSound, bool explode, bool damagesMonsters = false, Character firer = null, bool spriteFromObjectSheet = false, BasicProjectile.onCollisionBehavior collisionBehavior = null)
    {
      this.damageToFarmer = damageToFarmer;
      this.currentTileSheetIndex = parentSheetIndex;
      this.bouncesLeft = bouncesTillDestruct;
      this.tailLength = tailLength;
      this.rotationVelocity = rotationVelocity;
      this.xVelocity = xVelocity;
      this.yVelocity = yVelocity;
      this.position = startingPosition;
      if (firingSound != null && !firingSound.Equals(""))
        Game1.playSound(firingSound);
      this.explode = explode;
      this.collisionSound = collisionSound;
      this.damagesMonsters = damagesMonsters;
      this.theOneWhoFiredMe = firer;
      this.spriteFromObjectSheet = spriteFromObjectSheet;
      this.collisionBehavior = collisionBehavior;
    }

    public BasicProjectile(int damageToFarmer, int parentSheetIndex, int bouncesTillDestruct, int tailLength, float rotationVelocity, float xVelocity, float yVelocity, Vector2 startingPosition)
      : this(damageToFarmer, parentSheetIndex, bouncesTillDestruct, tailLength, rotationVelocity, xVelocity, yVelocity, startingPosition, "flameSpellHit", "flameSpell", true, false, (Character) null, false, (BasicProjectile.onCollisionBehavior) null)
    {
    }

    public override void updatePosition(GameTime time)
    {
      this.position.X += this.xVelocity;
      this.position.Y += this.yVelocity;
    }

    public override void behaviorOnCollisionWithPlayer(GameLocation location)
    {
      if (this.damagesMonsters)
        return;
      Game1.farmerTakeDamage(this.damageToFarmer, false, (Monster) null);
      this.explosionAnimation(location);
    }

    public override void behaviorOnCollisionWithTerrainFeature(TerrainFeature t, Vector2 tileLocation, GameLocation location)
    {
      t.performUseAction(tileLocation);
      this.explosionAnimation(location);
    }

    public override void behaviorOnCollisionWithMineWall(int tileX, int tileY)
    {
      this.explosionAnimation((GameLocation) Game1.mine);
    }

    public override void behaviorOnCollisionWithOther(GameLocation location)
    {
      this.explosionAnimation(location);
    }

    public override void behaviorOnCollisionWithMonster(NPC n, GameLocation location)
    {
      if (!this.damagesMonsters)
        return;
      this.explosionAnimation(location);
      if (n is Monster)
        location.damageMonster(n.GetBoundingBox(), this.damageToFarmer, this.damageToFarmer + 1, false, this.theOneWhoFiredMe is Farmer ? this.theOneWhoFiredMe as Farmer : Game1.player);
      else
        n.getHitByPlayer(this.theOneWhoFiredMe == null || !(this.theOneWhoFiredMe is Farmer) ? Game1.player : this.theOneWhoFiredMe as Farmer, location);
    }

    private void explosionAnimation(GameLocation location)
    {
      Rectangle standardTileSheet = Game1.getSourceRectForStandardTileSheet(this.spriteFromObjectSheet ? Game1.objectSpriteSheet : Projectile.projectileSheet, this.currentTileSheetIndex, -1, -1);
      standardTileSheet.X += Game1.tileSize / 2 - 4;
      standardTileSheet.Y += Game1.tileSize / 2 - 4;
      standardTileSheet.Width = 8;
      standardTileSheet.Height = 8;
      int debrisType = 12;
      switch (this.currentTileSheetIndex)
      {
        case 378:
          debrisType = 0;
          break;
        case 380:
          debrisType = 2;
          break;
        case 382:
          debrisType = 4;
          break;
        case 384:
          debrisType = 6;
          break;
        case 386:
          debrisType = 10;
          break;
        case 390:
          debrisType = 14;
          break;
      }
      if (this.spriteFromObjectSheet)
        Game1.createRadialDebris(location, debrisType, (int) ((double) this.position.X + (double) (Game1.tileSize / 2)) / Game1.tileSize, (int) ((double) this.position.Y + (double) (Game1.tileSize / 2)) / Game1.tileSize, 6, false, -1, false, -1);
      else
        Game1.createRadialDebris(location, Projectile.projectileSheet, standardTileSheet, 4, (int) this.position.X + Game1.tileSize / 2, (int) this.position.Y + Game1.tileSize / 2, 12, (int) ((double) this.position.Y / (double) Game1.tileSize) + 1);
      if (this.collisionSound != null && !this.collisionSound.Equals(""))
        Game1.playSound(this.collisionSound);
      if (this.explode)
        location.temporarySprites.Add(new TemporaryAnimatedSprite(362, (float) Game1.random.Next(30, 90), 6, 1, this.position, false, Game1.random.NextDouble() < 0.5));
      if (this.collisionBehavior != null)
        this.collisionBehavior(location, this.getBoundingBox().Center.X, this.getBoundingBox().Center.Y, this.theOneWhoFiredMe);
      this.destroyMe = true;
    }

    public static void explodeOnImpact(GameLocation location, int x, int y, Character who)
    {
      location.explode(new Vector2((float) (x / Game1.tileSize), (float) (y / Game1.tileSize)), 3, who is Farmer ? (Farmer) who : (Farmer) null);
    }

    public delegate void onCollisionBehavior(GameLocation location, int xPosition, int yPosition, Character who);
  }
}
