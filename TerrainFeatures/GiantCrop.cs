// Decompiled with JetBrains decompiler
// Type: StardewValley.TerrainFeatures.GiantCrop
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Tools;
using System;

namespace StardewValley.TerrainFeatures
{
  public class GiantCrop : ResourceClump
  {
    public const int cauliflower = 0;
    public const int melon = 1;
    public const int pumpkin = 2;
    public int which;
    public bool forSale;

    public GiantCrop()
    {
    }

    public GiantCrop(int indexOfSmallerVersion, Vector2 tile)
    {
      this.tile = tile;
      this.parentSheetIndex = indexOfSmallerVersion;
      if (indexOfSmallerVersion != 190)
      {
        if (indexOfSmallerVersion != 254)
        {
          if (indexOfSmallerVersion == 276)
            this.which = 2;
        }
        else
          this.which = 1;
      }
      else
        this.which = 0;
      this.width = 3;
      this.height = 3;
      this.health = 3f;
    }

    public override void draw(SpriteBatch spriteBatch, Vector2 tileLocation)
    {
      spriteBatch.Draw(Game1.cropSpriteSheet, Game1.GlobalToLocal(Game1.viewport, tileLocation * (float) Game1.tileSize - new Vector2((double) this.shakeTimer > 0.0 ? (float) (Math.Sin(2.0 * Math.PI / (double) this.shakeTimer) * 2.0) : 0.0f, (float) Game1.tileSize)), new Rectangle?(new Rectangle(112 + this.which * 48, 512, 48, 63)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, (float) (((double) tileLocation.Y + 2.0) * (double) Game1.tileSize / 10000.0));
    }

    public override bool performToolAction(Tool t, int damage, Vector2 tileLocation, GameLocation location = null)
    {
      if (t == null || !(t is Axe))
        return false;
      Game1.playSound("axchop");
      this.health = this.health - (float) (t.getLastFarmerToUse().toolPower + 1);
      Game1.createRadialDebris(Game1.currentLocation, 12, (int) tileLocation.X, (int) tileLocation.Y, Game1.random.Next(4, 9), false, -1, false, -1);
      if ((double) this.shakeTimer <= 0.0)
        this.shakeTimer = 100f;
      if ((double) this.health > 0.0)
        return false;
      t.getLastFarmerToUse().gainExperience(5, 50 * ((t.getLastFarmerToUse().luckLevel + 1) / 2));
      Random random;
      if (Game1.IsMultiplayer)
      {
        Game1.recentMultiplayerRandom = new Random((int) tileLocation.X * 1000 + (int) tileLocation.Y);
        random = Game1.recentMultiplayerRandom;
      }
      else
        random = new Random((int) Game1.uniqueIDForThisGame + (int) Game1.stats.DaysPlayed + (int) tileLocation.X * 7 + (int) tileLocation.Y * 11);
      int num = random.Next(15, 22);
      if (Game1.IsMultiplayer)
        Game1.createMultipleObjectDebris(this.parentSheetIndex, (int) tileLocation.X + 1, (int) tileLocation.Y + 1, num, t.getLastFarmerToUse().uniqueMultiplayerID);
      else
        Game1.createRadialDebris(Game1.currentLocation, this.parentSheetIndex, (int) tileLocation.X, (int) tileLocation.Y, num, false, -1, true, -1);
      Game1.setRichPresence("giantcrop", (object) new StardewValley.Object(Vector2.Zero, this.parentSheetIndex, 1).Name);
      Game1.createRadialDebris(Game1.currentLocation, 12, (int) tileLocation.X, (int) tileLocation.Y, Game1.random.Next(4, 9), false, -1, false, -1);
      Game1.playSound("stumpCrack");
      Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, tileLocation * (float) Game1.tileSize, Color.White, 8, false, 100f, 0, -1, -1f, -1, 0));
      Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, (tileLocation + new Vector2(1f, 0.0f)) * (float) Game1.tileSize, Color.White, 8, false, 110f, 0, -1, -1f, -1, 0));
      Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, (tileLocation + new Vector2(1f, 1f)) * (float) Game1.tileSize, Color.White, 8, true, 80f, 0, -1, -1f, -1, 0));
      Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, (tileLocation + new Vector2(0.0f, 1f)) * (float) Game1.tileSize, Color.White, 8, false, 90f, 0, -1, -1f, -1, 0));
      Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, tileLocation * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2)), Color.White, 8, false, 70f, 0, -1, -1f, -1, 0));
      Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, tileLocation * (float) Game1.tileSize, Color.White, 8, false, 100f, 0, -1, -1f, -1, 0));
      Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, (tileLocation + new Vector2(2f, 0.0f)) * (float) Game1.tileSize, Color.White, 8, false, 110f, 0, -1, -1f, -1, 0));
      Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, (tileLocation + new Vector2(2f, 1f)) * (float) Game1.tileSize, Color.White, 8, true, 80f, 0, -1, -1f, -1, 0));
      Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, (tileLocation + new Vector2(2f, 2f)) * (float) Game1.tileSize, Color.White, 8, false, 90f, 0, -1, -1f, -1, 0));
      Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, tileLocation * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize * 3 / 2), (float) (Game1.tileSize * 3 / 2)), Color.White, 8, false, 70f, 0, -1, -1f, -1, 0));
      Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, (tileLocation + new Vector2(0.0f, 2f)) * (float) Game1.tileSize, Color.White, 8, false, 110f, 0, -1, -1f, -1, 0));
      Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, (tileLocation + new Vector2(1f, 2f)) * (float) Game1.tileSize, Color.White, 8, true, 80f, 0, -1, -1f, -1, 0));
      return true;
    }
  }
}
