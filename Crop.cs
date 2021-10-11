// Decompiled with JetBrains decompiler
// Type: StardewValley.Crop
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Characters;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;

namespace StardewValley
{
  public class Crop
  {
    public List<int> phaseDays = new List<int>();
    public int phaseToShow = -1;
    public List<string> seasonsToGrowIn = new List<string>();
    public const int mixedSeedIndex = 770;
    public const int seedPhase = 0;
    public const int grabHarvest = 0;
    public const int sickleHarvest = 1;
    public const int rowOfWildSeeds = 23;
    public const int finalPhaseLength = 99999;
    public const int forageCrop_springOnion = 1;
    public int rowInSpriteSheet;
    public int currentPhase;
    public int harvestMethod;
    public int indexOfHarvest;
    public int regrowAfterHarvest;
    public int dayOfCurrentPhase;
    public int minHarvest;
    public int maxHarvest;
    public int maxHarvestIncreasePerFarmingLevel;
    public int daysOfUnclutteredGrowth;
    public int whichForageCrop;
    public Color tintColor;
    public bool flip;
    public bool fullyGrown;
    public bool raisedSeeds;
    public bool programColored;
    public bool dead;
    public bool forageCrop;
    public double chanceForExtraCrops;

    public Crop()
    {
    }

    public Crop(bool forageCrop, int which, int tileX, int tileY)
    {
      this.forageCrop = forageCrop;
      this.whichForageCrop = which;
      this.fullyGrown = true;
      this.currentPhase = 5;
    }

    public Crop(int seedIndex, int tileX, int tileY)
    {
      Dictionary<int, string> dictionary = Game1.content.Load<Dictionary<int, string>>("Data\\Crops");
      if (seedIndex == 770)
      {
        seedIndex = Crop.getRandomLowGradeCropForThisSeason(Game1.currentSeason);
        if (seedIndex == 473)
          --seedIndex;
      }
      if (dictionary.ContainsKey(seedIndex))
      {
        string[] strArray1 = dictionary[seedIndex].Split('/');
        string str1 = strArray1[0];
        char[] chArray1 = new char[1]{ ' ' };
        foreach (string str2 in str1.Split(chArray1))
          this.phaseDays.Add(Convert.ToInt32(str2));
        this.phaseDays.Add(99999);
        string str3 = strArray1[1];
        char[] chArray2 = new char[1]{ ' ' };
        foreach (string str2 in str3.Split(chArray2))
          this.seasonsToGrowIn.Add(str2);
        this.rowInSpriteSheet = Convert.ToInt32(strArray1[2]);
        this.indexOfHarvest = Convert.ToInt32(strArray1[3]);
        this.regrowAfterHarvest = Convert.ToInt32(strArray1[4]);
        this.harvestMethod = Convert.ToInt32(strArray1[5]);
        string[] strArray2 = strArray1[6].Split(' ');
        if (strArray2.Length != 0 && strArray2[0].Equals("true"))
        {
          this.minHarvest = Convert.ToInt32(strArray2[1]);
          this.maxHarvest = Convert.ToInt32(strArray2[2]);
          this.maxHarvestIncreasePerFarmingLevel = Convert.ToInt32(strArray2[3]);
          this.chanceForExtraCrops = Convert.ToDouble(strArray2[4]);
        }
        this.raisedSeeds = Convert.ToBoolean(strArray1[7]);
        string[] strArray3 = strArray1[8].Split(' ');
        if (strArray3.Length != 0 && strArray3[0].Equals("true"))
        {
          List<Color> colorList = new List<Color>();
          int index = 1;
          while (index < strArray3.Length)
          {
            colorList.Add(new Color((int) Convert.ToByte(strArray3[index]), (int) Convert.ToByte(strArray3[index + 1]), (int) Convert.ToByte(strArray3[index + 2])));
            index += 3;
          }
          Random random = new Random(tileX * 1000 + tileY + Game1.dayOfMonth);
          this.tintColor = colorList[random.Next(colorList.Count)];
          this.programColored = true;
        }
        this.flip = Game1.random.NextDouble() < 0.5;
      }
      if (this.rowInSpriteSheet != 23)
        return;
      this.whichForageCrop = seedIndex;
    }

    public static int getRandomLowGradeCropForThisSeason(string season)
    {
      if (season.Equals("winter"))
        season = Game1.random.NextDouble() < 0.33 ? "spring" : (Game1.random.NextDouble() < 0.5 ? "summer" : "fall");
      if (season == "spring")
        return Game1.random.Next(472, 476);
      if (!(season == "summer"))
      {
        if (season == "fall")
          return Game1.random.Next(487, 491);
      }
      else
      {
        switch (Game1.random.Next(4))
        {
          case 0:
            return 487;
          case 1:
            return 483;
          case 2:
            return 482;
          case 3:
            return 484;
        }
      }
      return -1;
    }

    public void growCompletely()
    {
      this.currentPhase = this.phaseDays.Count - 1;
      this.dayOfCurrentPhase = 0;
      if (this.regrowAfterHarvest == -1)
        return;
      this.fullyGrown = true;
    }

    public bool harvest(int xTile, int yTile, HoeDirt soil, JunimoHarvester junimoHarvester = null)
    {
      if (this.dead)
        return junimoHarvester != null;
      if (this.forageCrop)
      {
        Object @object = (Object) null;
        int howMuch = 3;
        if (this.whichForageCrop == 1)
          @object = new Object(399, 1, false, -1, 0);
        if (Game1.player.professions.Contains(16))
          @object.quality = 4;
        else if (Game1.random.NextDouble() < (double) Game1.player.ForagingLevel / 30.0)
          @object.quality = 2;
        else if (Game1.random.NextDouble() < (double) Game1.player.ForagingLevel / 15.0)
          @object.quality = 1;
        Game1.stats.ItemsForaged += (uint) @object.Stack;
        if (junimoHarvester != null)
        {
          junimoHarvester.tryToAddItemToHut((Item) @object);
          return true;
        }
        if (Game1.player.addItemToInventoryBool((Item) @object, false))
        {
          Vector2 vector2 = new Vector2((float) xTile, (float) yTile);
          Game1.player.animateOnce(279 + Game1.player.facingDirection);
          Game1.player.canMove = false;
          Game1.playSound(nameof (harvest));
          DelayedAction.playSoundAfterDelay("coin", 260);
          if (this.regrowAfterHarvest == -1)
          {
            Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(17, new Vector2(vector2.X * (float) Game1.tileSize, vector2.Y * (float) Game1.tileSize), Color.White, 7, Game1.random.NextDouble() < 0.5, 125f, 0, -1, -1f, -1, 0));
            Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(14, new Vector2(vector2.X * (float) Game1.tileSize, vector2.Y * (float) Game1.tileSize), Color.White, 7, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, -1, 0));
          }
          Game1.player.gainExperience(2, howMuch);
          return true;
        }
        Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Crop.cs.588"));
      }
      else if (this.currentPhase >= this.phaseDays.Count - 1 && (!this.fullyGrown || this.dayOfCurrentPhase <= 0))
      {
        int num1 = 1;
        int num2 = 0;
        int num3 = 0;
        if (this.indexOfHarvest == 0)
          return true;
        Random random = new Random(xTile * 7 + yTile * 11 + (int) Game1.stats.DaysPlayed + (int) Game1.uniqueIDForThisGame);
        switch (soil.fertilizer)
        {
          case 368:
            num3 = 1;
            break;
          case 369:
            num3 = 2;
            break;
        }
        double num4 = 0.2 * ((double) Game1.player.FarmingLevel / 10.0) + 0.2 * (double) num3 * (((double) Game1.player.FarmingLevel + 2.0) / 12.0) + 0.01;
        double num5 = Math.Min(0.75, num4 * 2.0);
        if (random.NextDouble() < num4)
          num2 = 2;
        else if (random.NextDouble() < num5)
          num2 = 1;
        if (this.minHarvest > 1 || this.maxHarvest > 1)
          num1 = random.Next(this.minHarvest, Math.Min(this.minHarvest + 1, this.maxHarvest + 1 + Game1.player.FarmingLevel / this.maxHarvestIncreasePerFarmingLevel));
        if (this.chanceForExtraCrops > 0.0)
        {
          while (random.NextDouble() < Math.Min(0.9, this.chanceForExtraCrops))
            ++num1;
        }
        if (this.harvestMethod == 1)
        {
          if (junimoHarvester == null)
            DelayedAction.playSoundAfterDelay("daggerswipe", 150);
          if (junimoHarvester != null && Utility.isOnScreen(junimoHarvester.getTileLocationPoint(), Game1.tileSize, junimoHarvester.currentLocation))
            Game1.playSound(nameof (harvest));
          if (junimoHarvester != null && Utility.isOnScreen(junimoHarvester.getTileLocationPoint(), Game1.tileSize, junimoHarvester.currentLocation))
            DelayedAction.playSoundAfterDelay("coin", 260);
          for (int index = 0; index < num1; ++index)
          {
            if (junimoHarvester != null)
              junimoHarvester.tryToAddItemToHut((Item) new Object(this.indexOfHarvest, 1, false, -1, num2));
            else
              Game1.createObjectDebris(this.indexOfHarvest, xTile, yTile, -1, num2, 1f, (GameLocation) null);
          }
          if (this.regrowAfterHarvest == -1)
            return true;
          this.dayOfCurrentPhase = this.regrowAfterHarvest;
          this.fullyGrown = true;
        }
        else
        {
          if (junimoHarvester == null)
          {
            Farmer player = Game1.player;
            Object @object;
            if (!this.programColored)
            {
              @object = new Object(this.indexOfHarvest, 1, false, -1, num2);
            }
            else
            {
              @object = (Object) new ColoredObject(this.indexOfHarvest, 1, this.tintColor);
              int num6 = num2;
              @object.quality = num6;
            }
            int num7 = 0;
            if (!player.addItemToInventoryBool((Item) @object, num7 != 0))
            {
              Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Crop.cs.588"));
              goto label_86;
            }
          }
          Vector2 vector2 = new Vector2((float) xTile, (float) yTile);
          if (junimoHarvester == null)
          {
            Game1.player.animateOnce(279 + Game1.player.facingDirection);
            Game1.player.canMove = false;
          }
          else
          {
            JunimoHarvester junimoHarvester1 = junimoHarvester;
            Object @object;
            if (!this.programColored)
            {
              @object = new Object(this.indexOfHarvest, 1, false, -1, num2);
            }
            else
            {
              @object = (Object) new ColoredObject(this.indexOfHarvest, 1, this.tintColor);
              int num6 = num2;
              @object.quality = num6;
            }
            junimoHarvester1.tryToAddItemToHut((Item) @object);
          }
          if (random.NextDouble() < (double) Game1.player.LuckLevel / 1500.0 + Game1.dailyLuck / 1200.0 + 9.99999974737875E-05)
          {
            num1 *= 2;
            if (junimoHarvester == null || Utility.isOnScreen(junimoHarvester.getTileLocationPoint(), Game1.tileSize, junimoHarvester.currentLocation))
              Game1.playSound("dwoop");
          }
          else if (this.harvestMethod == 0)
          {
            if (junimoHarvester == null || Utility.isOnScreen(junimoHarvester.getTileLocationPoint(), Game1.tileSize, junimoHarvester.currentLocation))
              Game1.playSound(nameof (harvest));
            if (junimoHarvester == null || Utility.isOnScreen(junimoHarvester.getTileLocationPoint(), Game1.tileSize, junimoHarvester.currentLocation))
              DelayedAction.playSoundAfterDelay("coin", 260);
            if (this.regrowAfterHarvest == -1 && (junimoHarvester == null || junimoHarvester.currentLocation.Equals((object) Game1.currentLocation)))
            {
              Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(17, new Vector2(vector2.X * (float) Game1.tileSize, vector2.Y * (float) Game1.tileSize), Color.White, 7, Game1.random.NextDouble() < 0.5, 125f, 0, -1, -1f, -1, 0));
              Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(14, new Vector2(vector2.X * (float) Game1.tileSize, vector2.Y * (float) Game1.tileSize), Color.White, 7, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, -1, 0));
            }
          }
          if (this.indexOfHarvest == 421)
          {
            this.indexOfHarvest = 431;
            num1 = random.Next(1, 4);
          }
          for (int index = 0; index < num1 - 1; ++index)
          {
            if (junimoHarvester == null)
              Game1.createObjectDebris(this.indexOfHarvest, xTile, yTile, -1, 0, 1f, (GameLocation) null);
            else
              junimoHarvester.tryToAddItemToHut((Item) new Object(this.indexOfHarvest, 1, false, -1, 0));
          }
          float num8 = (float) (16.0 * Math.Log(0.018 * (double) Convert.ToInt32(Game1.objectInformation[this.indexOfHarvest].Split('/')[1]) + 1.0, Math.E));
          if (junimoHarvester == null)
            Game1.player.gainExperience(0, (int) Math.Round((double) num8));
          if (this.regrowAfterHarvest == -1)
            return true;
          this.dayOfCurrentPhase = this.regrowAfterHarvest;
          this.fullyGrown = true;
        }
      }
label_86:
      return false;
    }

    public int getRandomWildCropForSeason(string season)
    {
      if (season == "spring")
        return 16 + Game1.random.Next(4) * 2;
      if (!(season == "summer"))
      {
        if (season == "fall")
          return 404 + Game1.random.Next(4) * 2;
        if (season == "winter")
          return 412 + Game1.random.Next(4) * 2;
        return 22;
      }
      if (Game1.random.NextDouble() < 0.33)
        return 396;
      return Game1.random.NextDouble() >= 0.5 ? 402 : 398;
    }

    private Rectangle getSourceRect(int number)
    {
      if (this.dead)
        return new Rectangle(192 + number % 4 * 16, 384, 16, 32);
      return new Rectangle(Math.Min(240, (this.fullyGrown ? (this.dayOfCurrentPhase <= 0 ? 6 : 7) : (this.phaseToShow != -1 ? this.phaseToShow : this.currentPhase) + ((this.phaseToShow != -1 ? this.phaseToShow : this.currentPhase) != 0 || number % 2 != 0 ? 0 : -1) + 1) * 16 + (this.rowInSpriteSheet % 2 != 0 ? 128 : 0)), this.rowInSpriteSheet / 2 * 16 * 2, 16, 32);
    }

    public void newDay(int state, int fertilizer, int xTile, int yTile, GameLocation environment)
    {
      if (!environment.name.Equals("Greenhouse") && (this.dead || !this.seasonsToGrowIn.Contains(Game1.currentSeason)))
      {
        this.dead = true;
      }
      else
      {
        if (state == 1)
        {
          this.dayOfCurrentPhase = this.fullyGrown ? this.dayOfCurrentPhase - 1 : Math.Min(this.dayOfCurrentPhase + 1, this.phaseDays.Count > 0 ? this.phaseDays[Math.Min(this.phaseDays.Count - 1, this.currentPhase)] : 0);
          if (this.dayOfCurrentPhase >= (this.phaseDays.Count > 0 ? this.phaseDays[Math.Min(this.phaseDays.Count - 1, this.currentPhase)] : 0) && this.currentPhase < this.phaseDays.Count - 1)
          {
            this.currentPhase = this.currentPhase + 1;
            this.dayOfCurrentPhase = 0;
          }
          while (this.currentPhase < this.phaseDays.Count - 1 && this.phaseDays.Count > 0 && this.phaseDays[this.currentPhase] <= 0)
            this.currentPhase = this.currentPhase + 1;
          if (this.rowInSpriteSheet == 23 && this.phaseToShow == -1 && this.currentPhase > 0)
            this.phaseToShow = Game1.random.Next(1, 7);
          if (environment is Farm && this.currentPhase == this.phaseDays.Count - 1 && (this.indexOfHarvest == 276 || this.indexOfHarvest == 190 || this.indexOfHarvest == 254) && new Random((int) Game1.uniqueIDForThisGame + (int) Game1.stats.DaysPlayed + xTile * 2000 + yTile).NextDouble() < 0.01)
          {
            for (int index1 = xTile - 1; index1 <= xTile + 1; ++index1)
            {
              for (int index2 = yTile - 1; index2 <= yTile + 1; ++index2)
              {
                Vector2 key = new Vector2((float) index1, (float) index2);
                if (!environment.terrainFeatures.ContainsKey(key) || !(environment.terrainFeatures[key] is HoeDirt) || ((environment.terrainFeatures[key] as HoeDirt).crop == null || (environment.terrainFeatures[key] as HoeDirt).crop.indexOfHarvest != this.indexOfHarvest))
                  return;
              }
            }
            for (int index1 = xTile - 1; index1 <= xTile + 1; ++index1)
            {
              for (int index2 = yTile - 1; index2 <= yTile + 1; ++index2)
              {
                Vector2 index3 = new Vector2((float) index1, (float) index2);
                (environment.terrainFeatures[index3] as HoeDirt).crop = (Crop) null;
              }
            }
            (environment as Farm).resourceClumps.Add((ResourceClump) new GiantCrop(this.indexOfHarvest, new Vector2((float) (xTile - 1), (float) (yTile - 1))));
          }
        }
        if (this.fullyGrown && this.dayOfCurrentPhase > 0 || (this.currentPhase < this.phaseDays.Count - 1 || this.rowInSpriteSheet != 23))
          return;
        Vector2 index = new Vector2((float) xTile, (float) yTile);
        environment.objects.Remove(index);
        string season = Game1.currentSeason;
        switch (this.whichForageCrop)
        {
          case 495:
            season = "spring";
            break;
          case 496:
            season = "summer";
            break;
          case 497:
            season = "fall";
            break;
          case 498:
            season = "winter";
            break;
        }
        environment.objects.Add(index, new Object(index, this.getRandomWildCropForSeason(season), 1)
        {
          isSpawnedObject = true,
          canBeGrabbed = true
        });
        if (environment.terrainFeatures[index] == null || !(environment.terrainFeatures[index] is HoeDirt))
          return;
        (environment.terrainFeatures[index] as HoeDirt).crop = (Crop) null;
      }
    }

    public bool isWildSeedCrop()
    {
      return this.rowInSpriteSheet == 23;
    }

    public void draw(SpriteBatch b, Vector2 tileLocation, Color toTint, float rotation)
    {
      if (this.forageCrop)
      {
        b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) ((double) tileLocation.X * (double) Game1.tileSize + (((double) tileLocation.X * 11.0 + (double) tileLocation.Y * 7.0) % 10.0 - 5.0)) + (float) (Game1.tileSize / 2), (float) ((double) tileLocation.Y * (double) Game1.tileSize + (((double) tileLocation.Y * 11.0 + (double) tileLocation.X * 7.0) % 10.0 - 5.0)) + (float) (Game1.tileSize / 2))), new Rectangle?(new Rectangle((int) ((double) tileLocation.X * 51.0 + (double) tileLocation.Y * 77.0) % 3 * 16, 128 + this.whichForageCrop * 16, 16, 16)), Color.White, 0.0f, new Vector2(8f, 8f), (float) Game1.pixelZoom, SpriteEffects.None, (float) (((double) tileLocation.Y * (double) Game1.tileSize + (double) (Game1.tileSize / 2) + (((double) tileLocation.Y * 11.0 + (double) tileLocation.X * 7.0) % 10.0 - 5.0)) / 10000.0));
      }
      else
      {
        b.Draw(Game1.cropSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) ((double) tileLocation.X * (double) Game1.tileSize + (this.raisedSeeds || this.currentPhase >= this.phaseDays.Count - 1 ? 0.0 : ((double) tileLocation.X * 11.0 + (double) tileLocation.Y * 7.0) % 10.0 - 5.0)) + (float) (Game1.tileSize / 2), (float) ((double) tileLocation.Y * (double) Game1.tileSize + (this.raisedSeeds || this.currentPhase >= this.phaseDays.Count - 1 ? 0.0 : ((double) tileLocation.Y * 11.0 + (double) tileLocation.X * 7.0) % 10.0 - 5.0)) + (float) (Game1.tileSize / 2))), new Rectangle?(this.getSourceRect((int) tileLocation.X * 7 + (int) tileLocation.Y * 11)), toTint, rotation, new Vector2(8f, 24f), (float) Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (float) (((double) tileLocation.Y * (double) Game1.tileSize + (double) (Game1.tileSize / 2) + (this.raisedSeeds || this.currentPhase >= this.phaseDays.Count - 1 ? 0.0 : ((double) tileLocation.Y * 11.0 + (double) tileLocation.X * 7.0) % 10.0 - 5.0)) / 10000.0 / (this.currentPhase != 0 || this.raisedSeeds ? 1.0 : 2.0)));
        if (this.tintColor.Equals(Color.White) || this.currentPhase != this.phaseDays.Count - 1 || this.dead)
          return;
        b.Draw(Game1.cropSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) ((double) tileLocation.X * (double) Game1.tileSize + (this.raisedSeeds || this.currentPhase >= this.phaseDays.Count - 1 ? 0.0 : ((double) tileLocation.X * 11.0 + (double) tileLocation.Y * 7.0) % 10.0 - 5.0)) + (float) (Game1.tileSize / 2), (float) ((double) tileLocation.Y * (double) Game1.tileSize + (this.raisedSeeds || this.currentPhase >= this.phaseDays.Count - 1 ? 0.0 : ((double) tileLocation.Y * 11.0 + (double) tileLocation.X * 7.0) % 10.0 - 5.0)) + (float) (Game1.tileSize / 2))), new Rectangle?(new Rectangle((this.fullyGrown ? (this.dayOfCurrentPhase <= 0 ? 6 : 7) : this.currentPhase + 1 + 1) * 16 + (this.rowInSpriteSheet % 2 != 0 ? 128 : 0), this.rowInSpriteSheet / 2 * 16 * 2, 16, 32)), this.tintColor, rotation, new Vector2(8f, 24f), (float) Game1.pixelZoom, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (float) (((double) tileLocation.Y * (double) Game1.tileSize + (double) (Game1.tileSize / 2) + (((double) tileLocation.Y * 11.0 + (double) tileLocation.X * 7.0) % 10.0 - 5.0)) / 10000.0 / (this.currentPhase != 0 || this.raisedSeeds ? 1.0 : 2.0)));
      }
    }

    public void drawInMenu(SpriteBatch b, Vector2 screenPosition, Color toTint, float rotation, float scale, float layerDepth)
    {
      b.Draw(Game1.cropSpriteSheet, screenPosition, new Rectangle?(this.getSourceRect(0)), toTint, rotation, new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize + Game1.tileSize / 2)), scale, this.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth);
    }
  }
}
