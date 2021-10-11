// Decompiled with JetBrains decompiler
// Type: StardewValley.Projectiles.Projectile
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.TerrainFeatures;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Projectiles
{
  public abstract class Projectile
  {
    public static int boundingBoxWidth = Game1.tileSize / 3;
    public static int boundingBoxHeight = Game1.tileSize / 3;
    protected int tailCounter = 50;
    private Queue<Vector2> tail = new Queue<Vector2>();
    public const int travelTimeBeforeCollisionPossible = 100;
    public const int goblinsCurseIndex = 0;
    public const int flameBallIndex = 1;
    public const int fearBolt = 2;
    public const int shadowBall = 3;
    public const int bone = 4;
    public const int throwingKnife = 5;
    public const int snowBall = 6;
    public const int shamanBolt = 7;
    public const int frostBall = 8;
    public const int frozenBolt = 9;
    public const int fireball = 10;
    public const int timePerTailUpdate = 50;
    public static Texture2D projectileSheet;
    protected int currentTileSheetIndex;
    protected Vector2 position;
    protected int tailLength;
    protected int bouncesLeft;
    protected int travelTime;
    protected float rotation;
    protected float rotationVelocity;
    protected float xVelocity;
    protected float yVelocity;
    protected bool damagesMonsters;
    protected bool spriteFromObjectSheet;
    protected Character theOneWhoFiredMe;
    public bool ignoreLocationCollision;
    public bool destroyMe;

    private bool behaviorOnCollision(GameLocation location)
    {
      foreach (Vector2 index in Utility.getListOfTileLocationsForBordersOfNonTileRectangle(this.getBoundingBox()))
      {
        if (!this.damagesMonsters && Game1.player.GetBoundingBox().Intersects(this.getBoundingBox()))
        {
          this.behaviorOnCollisionWithPlayer(location);
          return true;
        }
        if (location.terrainFeatures.ContainsKey(index) && !location.terrainFeatures[index].isPassable((Character) null))
        {
          this.behaviorOnCollisionWithTerrainFeature(location.terrainFeatures[index], index, location);
          return true;
        }
        if (this.damagesMonsters)
        {
          NPC n = location.doesPositionCollideWithCharacter(this.getBoundingBox(), false);
          if (n != null)
          {
            this.behaviorOnCollisionWithMonster(n, location);
            return true;
          }
        }
      }
      this.behaviorOnCollisionWithOther(location);
      return true;
    }

    public abstract void behaviorOnCollisionWithPlayer(GameLocation location);

    public abstract void behaviorOnCollisionWithTerrainFeature(TerrainFeature t, Vector2 tileLocation, GameLocation location);

    public abstract void behaviorOnCollisionWithMineWall(int tileX, int tileY);

    public abstract void behaviorOnCollisionWithOther(GameLocation location);

    public abstract void behaviorOnCollisionWithMonster(NPC n, GameLocation location);

    public bool update(GameTime time, GameLocation location)
    {
      this.rotation = this.rotation + this.rotationVelocity;
      this.travelTime = this.travelTime + time.ElapsedGameTime.Milliseconds;
      this.updatePosition(time);
      this.updateTail(time);
      if (this.isColliding(location) && this.travelTime > 100)
      {
        if (this.bouncesLeft <= 0 || Game1.player.GetBoundingBox().Intersects(this.getBoundingBox()))
          return this.behaviorOnCollision(location);
        this.bouncesLeft = this.bouncesLeft - 1;
        bool[] flagArray = Utility.horizontalOrVerticalCollisionDirections(this.getBoundingBox(), this.theOneWhoFiredMe, true);
        int index1 = 0;
        if (flagArray[index1])
          this.xVelocity = -this.xVelocity;
        int index2 = 1;
        if (flagArray[index2])
          this.yVelocity = -this.yVelocity;
      }
      return false;
    }

    private void updateTail(GameTime time)
    {
      this.tailCounter = this.tailCounter - time.ElapsedGameTime.Milliseconds;
      if (this.tailCounter > 0)
        return;
      this.tailCounter = 50;
      this.tail.Enqueue(this.position);
      if (this.tail.Count <= this.tailLength)
        return;
      this.tail.Dequeue();
    }

    public virtual bool isColliding(GameLocation location)
    {
      if (!location.isTileOnMap(this.position / (float) Game1.tileSize) || !this.ignoreLocationCollision && location.isCollidingPosition(this.getBoundingBox(), Game1.viewport, false, 0, true, this.theOneWhoFiredMe, false, true, false) || !this.damagesMonsters && Game1.player.GetBoundingBox().Intersects(this.getBoundingBox()))
        return true;
      if (this.damagesMonsters)
        return location.doesPositionCollideWithCharacter(this.getBoundingBox(), false) != null;
      return false;
    }

    public abstract void updatePosition(GameTime time);

    public virtual Rectangle getBoundingBox()
    {
      return new Rectangle((int) this.position.X + Game1.tileSize / 2 - (Projectile.boundingBoxWidth + (this.damagesMonsters ? 8 : 0)) / 2, (int) this.position.Y + Game1.tileSize / 2 - (Projectile.boundingBoxWidth + (this.damagesMonsters ? 8 : 0)) / 2, Projectile.boundingBoxWidth + (this.damagesMonsters ? 8 : 0), Projectile.boundingBoxWidth + (this.damagesMonsters ? 8 : 0));
    }

    public virtual void draw(SpriteBatch b)
    {
      b.Draw(this.spriteFromObjectSheet ? Game1.objectSpriteSheet : Projectile.projectileSheet, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(this.spriteFromObjectSheet ? Game1.objectSpriteSheet : Projectile.projectileSheet, this.currentTileSheetIndex, 16, 16)), Color.White, this.rotation, new Vector2(8f, 8f), (float) Game1.pixelZoom, SpriteEffects.None, (float) (((double) this.position.Y + (double) (Game1.tileSize * 3 / 2)) / 10000.0));
      float scale = (float) Game1.pixelZoom;
      float num = 1f;
      for (int index = this.tail.Count - 1; index >= 0; --index)
      {
        b.Draw(this.spriteFromObjectSheet ? Game1.objectSpriteSheet : Projectile.projectileSheet, Game1.GlobalToLocal(Game1.viewport, this.tail.ElementAt<Vector2>(index) + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(this.spriteFromObjectSheet ? Game1.objectSpriteSheet : Projectile.projectileSheet, this.currentTileSheetIndex, 16, 16)), Color.White * num, this.rotation, new Vector2(8f, 8f), scale, SpriteEffects.None, (float) (((double) this.tail.ElementAt<Vector2>(index).Y + (double) (Game1.tileSize * 3 / 2)) / 10000.0));
        scale = 0.8f * (float) (Game1.pixelZoom - Game1.pixelZoom / (index + Game1.pixelZoom));
        num -= 0.1f;
      }
    }
  }
}
