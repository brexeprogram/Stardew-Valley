// Decompiled with JetBrains decompiler
// Type: StardewValley.TerrainFeatures.HoeDirt
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Characters;
using StardewValley.Locations;
using StardewValley.Tools;
using System;
using System.Collections.Generic;

namespace StardewValley.TerrainFeatures
{
  public class HoeDirt : TerrainFeature
  {
    private Color c = Color.White;
    public const float defaultShakeRate = 0.03926991f;
    public const float maximumShake = 0.3926991f;
    public const float shakeDecayRate = 0.01047198f;
    public const int N = 1000;
    public const int E = 100;
    public const int S = 500;
    public const int W = 10;
    public const int dry = 0;
    public const int watered = 1;
    public const int invisible = 2;
    public const int noFertilizer = 0;
    public const int fertilizerLowQuality = 368;
    public const int fertilizerHighQuality = 369;
    public const int waterRetentionSoil = 370;
    public const int waterRetentionSoilQUality = 371;
    public const int speedGro = 465;
    public const int superSpeedGro = 466;
    public static Texture2D lightTexture;
    public static Texture2D darkTexture;
    public static Texture2D snowTexture;
    public Crop crop;
    public static Dictionary<int, int> drawGuide;
    public int state;
    public int fertilizer;
    private bool shakeLeft;
    private float shakeRotation;
    private float maxShake;
    private float shakeRate;

    public HoeDirt()
    {
      this.loadSprite();
      if (HoeDirt.drawGuide == null)
        HoeDirt.populateDrawGuide();
      if (!(Game1.currentLocation is MineShaft) || (Game1.currentLocation as MineShaft).getMineArea(-1) != 80)
        return;
      this.c = Color.MediumPurple * 0.4f;
    }

    public HoeDirt(int startingState)
      : this()
    {
      this.state = startingState;
    }

    public HoeDirt(int startingState, Crop crop)
      : this()
    {
      this.state = startingState;
      this.crop = crop;
    }

    public override Rectangle getBoundingBox(Vector2 tileLocation)
    {
      return new Rectangle((int) ((double) tileLocation.X * (double) Game1.tileSize), (int) ((double) tileLocation.Y * (double) Game1.tileSize), Game1.tileSize, Game1.tileSize);
    }

    public override void doCollisionAction(Rectangle positionOfCollider, int speedOfCollision, Vector2 tileLocation, Character who, GameLocation location)
    {
      if (this.crop != null && this.crop.currentPhase != 0 && (speedOfCollision > 0 && (double) this.maxShake == 0.0) && (positionOfCollider.Intersects(this.getBoundingBox(tileLocation)) && Utility.isOnScreen(Utility.Vector2ToPoint(tileLocation), Game1.tileSize, location)))
      {
        if (Game1.soundBank != null && (who == null || who.GetType() != typeof (FarmAnimal)) && !Grass.grassSound.IsPlaying)
        {
          Grass.grassSound = Game1.soundBank.GetCue("grassyStep");
          Grass.grassSound.Play();
        }
        this.shake((float) (0.392699092626572 / (double) ((5 + Game1.player.addedSpeed) / speedOfCollision) - (speedOfCollision > 2 ? (double) this.crop.currentPhase * 3.14159274101257 / 64.0 : 0.0)), (float) Math.PI / 80f / (float) ((5 + Game1.player.addedSpeed) / speedOfCollision), (double) positionOfCollider.Center.X > (double) tileLocation.X * (double) Game1.tileSize + (double) (Game1.tileSize / 2));
      }
      if (this.crop == null || this.crop.currentPhase == 0 || (!(who is Farmer) || !(who as Farmer).running))
        return;
      (who as Farmer).temporarySpeedBuff = -1f;
    }

    private void shake(float shake, float rate, bool left)
    {
      if (this.crop == null)
        return;
      this.maxShake = shake * (this.crop.raisedSeeds ? 0.6f : 1.5f);
      this.shakeRate = rate * 0.5f;
      this.shakeRotation = 0.0f;
      this.shakeLeft = left;
    }

    public bool needsWatering()
    {
      if (this.crop == null)
        return false;
      if (this.readyForHarvest())
        return this.crop.regrowAfterHarvest != -1;
      return true;
    }

    public static void populateDrawGuide()
    {
      HoeDirt.drawGuide = new Dictionary<int, int>();
      HoeDirt.drawGuide.Add(0, 0);
      HoeDirt.drawGuide.Add(10, 15);
      HoeDirt.drawGuide.Add(100, 13);
      HoeDirt.drawGuide.Add(1000, 12);
      HoeDirt.drawGuide.Add(500, 4);
      HoeDirt.drawGuide.Add(1010, 11);
      HoeDirt.drawGuide.Add(1100, 9);
      HoeDirt.drawGuide.Add(1500, 8);
      HoeDirt.drawGuide.Add(600, 1);
      HoeDirt.drawGuide.Add(510, 3);
      HoeDirt.drawGuide.Add(110, 14);
      HoeDirt.drawGuide.Add(1600, 5);
      HoeDirt.drawGuide.Add(1610, 6);
      HoeDirt.drawGuide.Add(1510, 7);
      HoeDirt.drawGuide.Add(1110, 10);
      HoeDirt.drawGuide.Add(610, 2);
    }

    public override void loadSprite()
    {
      if (HoeDirt.lightTexture == null)
      {
        try
        {
          HoeDirt.lightTexture = Game1.content.Load<Texture2D>("TerrainFeatures\\hoeDirt");
        }
        catch (Exception ex)
        {
        }
      }
      if (HoeDirt.darkTexture == null)
      {
        try
        {
          HoeDirt.darkTexture = Game1.content.Load<Texture2D>("TerrainFeatures\\hoeDirtDark");
        }
        catch (Exception ex)
        {
        }
      }
      if (HoeDirt.snowTexture != null)
        return;
      try
      {
        HoeDirt.snowTexture = Game1.content.Load<Texture2D>("TerrainFeatures\\hoeDirtSnow");
      }
      catch (Exception ex)
      {
      }
    }

    public override bool isPassable(Character c)
    {
      if (this.crop != null && this.crop.raisedSeeds)
        return c is JunimoHarvester;
      return true;
    }

    public bool readyForHarvest()
    {
      if (this.crop != null && (!this.crop.fullyGrown || this.crop.dayOfCurrentPhase <= 0) && this.crop.currentPhase >= this.crop.phaseDays.Count - 1)
        return !this.crop.dead;
      return false;
    }

    public override bool performUseAction(Vector2 tileLocation)
    {
      if (this.crop == null)
        return false;
      bool flag = this.crop.currentPhase >= this.crop.phaseDays.Count - 1 && (!this.crop.fullyGrown || this.crop.dayOfCurrentPhase <= 0);
      if (this.crop.harvestMethod == 0 && this.crop.harvest((int) tileLocation.X, (int) tileLocation.Y, this, (JunimoHarvester) null))
      {
        this.destroyCrop(tileLocation, false);
        return true;
      }
      if (this.crop.harvestMethod == 1 && this.readyForHarvest())
      {
        if (Game1.player.CurrentTool != null && Game1.player.CurrentTool is MeleeWeapon && (Game1.player.CurrentTool as MeleeWeapon).initialParentTileIndex == 47)
        {
          Game1.player.CanMove = false;
          Game1.player.UsingTool = true;
          Game1.player.canReleaseTool = true;
          Game1.player.Halt();
          try
          {
            Game1.player.CurrentTool.beginUsing(Game1.currentLocation, (int) Game1.player.lastClick.X, (int) Game1.player.lastClick.Y, Game1.player);
          }
          catch (Exception ex)
          {
          }
          ((MeleeWeapon) Game1.player.CurrentTool).setFarmerAnimating(Game1.player);
        }
        else
          Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:HoeDirt.cs.13915"));
      }
      return flag;
    }

    public bool plant(int index, int tileX, int tileY, Farmer who, bool isFertilizer = false)
    {
      if (isFertilizer)
      {
        if (this.crop != null && (index == 368 || index == 369))
        {
          Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:HoeDirt.cs.13916"));
          return false;
        }
        if (this.fertilizer == 0)
        {
          this.fertilizer = index;
          Game1.playSound("dirtyHit");
        }
        return true;
      }
      Crop crop = new Crop(index, tileX, tileY);
      if (crop.seasonsToGrowIn.Count == 0)
        return false;
      if (this.fertilizer == 465 || this.fertilizer == 466 || who.professions.Contains(5))
      {
        int num1 = 0;
        for (int index1 = 0; index1 < crop.phaseDays.Count - 1; ++index1)
          num1 += crop.phaseDays[index1];
        float num2 = this.fertilizer == 465 ? 0.1f : (this.fertilizer == 466 ? 0.25f : 0.0f);
        if (who.professions.Contains(5))
          num2 += 0.1f;
        int num3 = (int) Math.Ceiling((double) num1 * (double) num2);
        for (int index1 = 0; num3 > 0 && index1 < 3; ++index1)
        {
          for (int index2 = 0; index2 < crop.phaseDays.Count; ++index2)
          {
            if (index2 > 0 || crop.phaseDays[index2] > 1)
            {
              List<int> phaseDays = crop.phaseDays;
              int num4 = index2;
              int index3 = num4;
              int num5 = phaseDays[index3];
              int index4 = num4;
              int num6 = num5 - 1;
              phaseDays[index4] = num6;
              --num3;
            }
            if (num3 <= 0)
              break;
          }
        }
      }
      if (!who.currentLocation.isFarm && !who.currentLocation.name.Equals("Greenhouse"))
      {
        Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:HoeDirt.cs.13919"));
        return false;
      }
      if (who.currentLocation.name.Equals("Greenhouse") || crop.seasonsToGrowIn.Contains(Game1.currentSeason))
      {
        this.crop = crop;
        if (crop.raisedSeeds)
          Game1.playSound("stoneStep");
        Game1.playSound("dirtyHit");
        ++Game1.stats.SeedsSown;
        return true;
      }
      if (crop.seasonsToGrowIn.Count > 0 && !crop.seasonsToGrowIn.Contains(Game1.currentSeason))
        Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:HoeDirt.cs.13924"));
      else
        Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:HoeDirt.cs.13925"));
      return false;
    }

    public void destroyCrop(Vector2 tileLocation, bool showAnimation = true)
    {
      if (this.crop != null & showAnimation)
      {
        if (this.crop.currentPhase < 1 && !this.crop.dead)
        {
          Game1.player.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(12, tileLocation * (float) Game1.tileSize, Color.White, 8, false, 100f, 0, -1, -1f, -1, 0));
          Game1.playSound("dirtyHit");
        }
        else
          Game1.player.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(50, tileLocation * (float) Game1.tileSize, this.crop.dead ? new Color(207, 193, 43) : Color.ForestGreen, 8, false, 100f, 0, -1, -1f, -1, 0));
      }
      this.crop = (Crop) null;
    }

    public override bool performToolAction(Tool t, int damage, Vector2 tileLocation, GameLocation location = null)
    {
      if (t != null)
      {
        if (t.GetType() == typeof (Pickaxe) && this.crop == null)
          return true;
        if (t.GetType() == typeof (WateringCan))
          this.state = 1;
        else if (t is MeleeWeapon && (t as MeleeWeapon).name.Equals("Scythe"))
        {
          if (this.crop != null && this.crop.harvestMethod == 1 && this.crop.harvest((int) tileLocation.X, (int) tileLocation.Y, this, (JunimoHarvester) null))
            this.destroyCrop(tileLocation, true);
          if (this.crop != null && this.crop.dead)
            this.destroyCrop(tileLocation, true);
        }
        else if (t.isHeavyHitter() && t.GetType() != typeof (Hoe) && (!(t is MeleeWeapon) && this.crop != null))
          this.destroyCrop(tileLocation, true);
        this.shake((float) Math.PI / 32f, (float) Math.PI / 40f, (double) tileLocation.X * (double) Game1.tileSize < (double) Game1.player.position.X);
      }
      else if (damage > 0 && this.crop != null)
      {
        if (damage == 50)
          this.crop.dead = true;
        else
          this.destroyCrop(tileLocation, true);
      }
      return false;
    }

    public bool canPlantThisSeedHere(int objectIndex, int tileX, int tileY, bool isFertilizer = false)
    {
      if (isFertilizer)
      {
        if (this.fertilizer == 0)
          return true;
      }
      else if (this.crop == null)
      {
        Crop crop = new Crop(objectIndex, tileX, tileY);
        if (crop.seasonsToGrowIn.Count == 0)
          return false;
        if (Game1.currentLocation.name.Equals("Greenhouse") || crop.seasonsToGrowIn.Contains(Game1.currentSeason))
          return !crop.raisedSeeds || !Utility.doesRectangleIntersectTile(Game1.player.GetBoundingBox(), tileX, tileY);
        if (objectIndex == 309 || objectIndex == 310 || objectIndex == 311)
          return true;
        if (Game1.didPlayerJustClickAtAll())
          Game1.playSound("cancel");
      }
      return false;
    }

    public override bool tickUpdate(GameTime time, Vector2 tileLocation)
    {
      if ((double) this.maxShake > 0.0)
      {
        if (this.shakeLeft)
        {
          this.shakeRotation = this.shakeRotation - this.shakeRate;
          if ((double) Math.Abs(this.shakeRotation) >= (double) this.maxShake)
            this.shakeLeft = false;
        }
        else
        {
          this.shakeRotation = this.shakeRotation + this.shakeRate;
          if ((double) this.shakeRotation >= (double) this.maxShake)
          {
            this.shakeLeft = true;
            this.shakeRotation = this.shakeRotation - this.shakeRate;
          }
        }
        this.maxShake = Math.Max(0.0f, this.maxShake - (float) Math.PI / 300f);
      }
      else
        this.shakeRotation = this.shakeRotation / 2f;
      if (this.state == 2)
        return this.crop == null;
      return false;
    }

    public override void dayUpdate(GameLocation environment, Vector2 tileLocation)
    {
      if (this.crop != null)
      {
        this.crop.newDay(this.state, this.fertilizer, (int) tileLocation.X, (int) tileLocation.Y, environment);
        if (!environment.name.Equals("Greenhouse") && Game1.currentSeason.Equals("winter") && (this.crop != null && !this.crop.isWildSeedCrop()))
          this.destroyCrop(tileLocation, false);
      }
      if (this.fertilizer == 370 && Game1.random.NextDouble() < 0.33 || this.fertilizer == 371 && Game1.random.NextDouble() < 0.66)
        return;
      this.state = 0;
    }

    public override bool seasonUpdate(bool onLoad)
    {
      if (!onLoad && (this.crop == null || this.crop.dead || !this.crop.seasonsToGrowIn.Contains(Game1.currentSeason)))
        this.fertilizer = 0;
      return false;
    }

    public override void drawInMenu(SpriteBatch spriteBatch, Vector2 positionOnScreen, Vector2 tileLocation, float scale, float layerDepth)
    {
      int index = 0;
      Vector2 key = tileLocation;
      ++key.X;
      GameLocation locationFromName = Game1.getLocationFromName("Farm");
      if (locationFromName.terrainFeatures.ContainsKey(key) && locationFromName.terrainFeatures[key].GetType() == typeof (HoeDirt))
        index += 100;
      key.X -= 2f;
      if (locationFromName.terrainFeatures.ContainsKey(key) && locationFromName.terrainFeatures[key].GetType() == typeof (HoeDirt))
        index += 10;
      ++key.X;
      ++key.Y;
      if (Game1.currentLocation.terrainFeatures.ContainsKey(key) && locationFromName.terrainFeatures[key].GetType() == typeof (HoeDirt))
        index += 500;
      key.Y -= 2f;
      if (locationFromName.terrainFeatures.ContainsKey(key) && locationFromName.terrainFeatures[key].GetType() == typeof (HoeDirt))
        index += 1000;
      int num = HoeDirt.drawGuide[index];
      spriteBatch.Draw(HoeDirt.lightTexture, positionOnScreen, new Rectangle?(new Rectangle(num % 4 * Game1.tileSize, num / 4 * Game1.tileSize, Game1.tileSize, Game1.tileSize)), Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, layerDepth + positionOnScreen.Y / 20000f);
      if (this.crop == null)
        return;
      this.crop.drawInMenu(spriteBatch, positionOnScreen + new Vector2((float) Game1.tileSize * scale, (float) Game1.tileSize * scale), Color.White, 0.0f, scale, layerDepth + (float) (((double) positionOnScreen.Y + (double) Game1.tileSize * (double) scale) / 20000.0));
    }

    public override void draw(SpriteBatch spriteBatch, Vector2 tileLocation)
    {
      if (this.state != 2)
      {
        int index1 = 0;
        int index2 = 0;
        Vector2 key = tileLocation;
        ++key.X;
        if (Game1.currentLocation.terrainFeatures.ContainsKey(key) && Game1.currentLocation.terrainFeatures[key].GetType() == typeof (HoeDirt))
        {
          index1 += 100;
          if (((HoeDirt) Game1.currentLocation.terrainFeatures[key]).state == this.state)
            index2 += 100;
        }
        key.X -= 2f;
        if (Game1.currentLocation.terrainFeatures.ContainsKey(key) && Game1.currentLocation.terrainFeatures[key].GetType() == typeof (HoeDirt))
        {
          index1 += 10;
          if (((HoeDirt) Game1.currentLocation.terrainFeatures[key]).state == this.state)
            index2 += 10;
        }
        ++key.X;
        ++key.Y;
        if (Game1.currentLocation.terrainFeatures.ContainsKey(key) && Game1.currentLocation.terrainFeatures[key].GetType() == typeof (HoeDirt))
        {
          index1 += 500;
          if (((HoeDirt) Game1.currentLocation.terrainFeatures[key]).state == this.state)
            index2 += 500;
        }
        key.Y -= 2f;
        if (Game1.currentLocation.terrainFeatures.ContainsKey(key) && Game1.currentLocation.terrainFeatures[key].GetType() == typeof (HoeDirt))
        {
          index1 += 1000;
          if (((HoeDirt) Game1.currentLocation.terrainFeatures[key]).state == this.state)
            index2 += 1000;
        }
        int num1 = HoeDirt.drawGuide[index1];
        int num2 = HoeDirt.drawGuide[index2];
        Texture2D texture = Game1.currentLocation.Name.Equals("Mountain") || Game1.currentLocation.Name.Equals("Mine") || Game1.currentLocation is MineShaft && Game1.mine.getMineArea(-1) != 121 ? HoeDirt.darkTexture : HoeDirt.lightTexture;
        if (Game1.currentSeason.Equals("winter") && !(Game1.currentLocation is Desert) && (!Game1.currentLocation.Name.Equals("Greenhouse") && !(Game1.currentLocation is MineShaft)) || Game1.currentLocation is MineShaft && Game1.mine.getMineArea(-1) == 40 && !Game1.mine.isLevelSlimeArea())
          texture = HoeDirt.snowTexture;
        spriteBatch.Draw(texture, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float) Game1.tileSize, tileLocation.Y * (float) Game1.tileSize)), new Rectangle?(new Rectangle(num1 % 4 * 16, num1 / 4 * 16, 16, 16)), this.c, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1E-08f);
        if (this.state == 1)
          spriteBatch.Draw(texture, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float) Game1.tileSize, tileLocation.Y * (float) Game1.tileSize)), new Rectangle?(new Rectangle(num2 % 4 * 16 + 64, num2 / 4 * 16, 16, 16)), this.c, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1.2E-08f);
        if (this.fertilizer != 0)
        {
          int num3 = 0;
          switch (this.fertilizer)
          {
            case 369:
              num3 = 1;
              break;
            case 370:
              num3 = 2;
              break;
            case 371:
              num3 = 3;
              break;
            case 465:
              num3 = 4;
              break;
            case 466:
              num3 = 5;
              break;
          }
          spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float) Game1.tileSize, tileLocation.Y * (float) Game1.tileSize)), new Rectangle?(new Rectangle(173 + num3 / 2 * 16, 466 + num3 % 2 * 16, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1.9E-08f);
        }
      }
      if (this.crop == null)
        return;
      this.crop.draw(spriteBatch, tileLocation, this.state != 1 || this.crop.currentPhase != 0 || this.crop.raisedSeeds ? Color.White : new Color(180, 100, 200) * 1f, this.shakeRotation);
    }
  }
}
