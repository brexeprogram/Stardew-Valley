// Decompiled with JetBrains decompiler
// Type: StardewValley.TerrainFeatures.CosmeticPlant
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Tools;
using System;

namespace StardewValley.TerrainFeatures
{
  public class CosmeticPlant : Grass
  {
    public float scale = 1f;
    public bool flipped;
    private int xOffset;
    private int yOffset;

    public CosmeticPlant()
    {
    }

    public CosmeticPlant(int which)
      : base(which, 1)
    {
      this.flipped = Game1.random.NextDouble() < 0.5;
      this.scale = (float) (1.0 - (Game1.random.NextDouble() < 0.5 ? (double) Game1.random.Next(which == 0 ? 10 : 51) / 100.0 : 0.0));
    }

    public override Rectangle getBoundingBox(Vector2 tileLocation)
    {
      return new Rectangle((int) ((double) tileLocation.X * (double) Game1.tileSize + (double) (Game1.tileSize / 4)), (int) (((double) tileLocation.Y + 1.0) * (double) Game1.tileSize - (double) (Game1.tileSize / 8) - 4.0), Game1.tileSize / 8, Game1.tileSize / 16 + 4);
    }

    public override bool seasonUpdate(bool onLoad)
    {
      return false;
    }

    public override void loadSprite()
    {
      try
      {
        this.texture = Game1.content.Load<Texture2D>("TerrainFeatures\\upperCavePlants");
      }
      catch (Exception ex)
      {
      }
      this.xOffset = Game1.random.Next(-2, 3) * 4;
      this.yOffset = Game1.random.Next(-2, 1) * 4;
    }

    public override bool performToolAction(Tool t, int explosion, Vector2 tileLocation, GameLocation location = null)
    {
      if (t != null && t is MeleeWeapon && ((MeleeWeapon) t).type != 2 || explosion > 0)
      {
        this.shake(3f * (float) Math.PI / 32f, (float) Math.PI / 40f, Game1.random.NextDouble() < 0.5);
        int num = explosion <= 0 ? (t.upgradeLevel == 3 ? 3 : t.upgradeLevel + 1) : Math.Max(1, explosion + 2 - Game1.random.Next(2));
        Game1.createRadialDebris(Game1.currentLocation, this.texture, new Rectangle(28 + (int) this.grassType * Game1.tileSize, 24, 28, 24), (int) tileLocation.X, (int) tileLocation.Y, Game1.random.Next(6, 14));
        this.numberOfWeeds = this.numberOfWeeds - num;
        if (this.numberOfWeeds <= 0)
        {
          Random random = new Random((int) ((double) Game1.uniqueIDForThisGame + (double) tileLocation.X * 7.0 + (double) tileLocation.Y * 11.0 + (double) Game1.mine.mineLevel + (double) Game1.player.timesReachedMineBottom));
          if (random.NextDouble() < 0.005)
            Game1.createObjectDebris(114, (int) tileLocation.X, (int) tileLocation.Y, -1, 0, 1f, (GameLocation) null);
          else if (random.NextDouble() < 0.01)
            Game1.createDebris(random.NextDouble() < 0.5 ? 4 : 8, (int) tileLocation.X, (int) tileLocation.Y, random.Next(1, 2), (GameLocation) null);
          else if (random.NextDouble() < 0.02)
            Game1.createDebris(92, (int) tileLocation.X, (int) tileLocation.Y, random.Next(2, 4), (GameLocation) null);
          return true;
        }
      }
      return false;
    }

    public override void draw(SpriteBatch spriteBatch, Vector2 tileLocation)
    {
      spriteBatch.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float) Game1.tileSize, tileLocation.Y * (float) Game1.tileSize) + new Vector2((float) (Game1.tileSize / 2 + this.xOffset), (float) (Game1.tileSize - 4 + this.yOffset))), new Rectangle?(new Rectangle((int) this.grassType * Game1.tileSize, 0, Game1.tileSize, Game1.tileSize * 3 / 2)), Color.White, this.shakeRotation, new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize * 3 / 2 - 4)), this.scale, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (float) (((double) (this.getBoundingBox(tileLocation).Y - 4) + (double) tileLocation.X / 900.0 + (double) this.scale / 100.0) / 10000.0));
    }
  }
}
