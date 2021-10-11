// Decompiled with JetBrains decompiler
// Type: StardewValley.Events.SoundInTheNightEvent
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Buildings;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;

namespace StardewValley.Events
{
  public class SoundInTheNightEvent : FarmEvent
  {
    public const int cropCircle = 0;
    public const int meteorite = 1;
    public const int dogs = 2;
    public const int owl = 3;
    public const int earthquake = 4;
    private int behavior;
    private int timer;
    private string soundName;
    private string message;
    private bool playedSound;
    private bool showedMessage;
    private Vector2 targetLocation;
    private Building targetBuilding;

    public SoundInTheNightEvent(int which)
    {
      this.behavior = which;
    }

    public bool setUp()
    {
      Random random = new Random((int) Game1.uniqueIDForThisGame + (int) Game1.stats.DaysPlayed);
      Farm locationFromName = Game1.getLocationFromName("Farm") as Farm;
      switch (this.behavior)
      {
        case 0:
          this.soundName = "UFO";
          this.message = Game1.content.LoadString("Strings\\Events:SoundInTheNight_UFO");
          for (int index = 50; index > 0; --index)
          {
            this.targetLocation = new Vector2((float) random.Next(5, locationFromName.map.GetLayer("Back").TileWidth - 4), (float) random.Next(5, locationFromName.map.GetLayer("Back").TileHeight - 4));
            if (!locationFromName.isTileLocationTotallyClearAndPlaceable(this.targetLocation))
              return true;
          }
          break;
        case 1:
          this.soundName = "Meteorite";
          this.message = Game1.content.LoadString("Strings\\Events:SoundInTheNight_Meteorite");
          this.targetLocation = new Vector2((float) random.Next(5, locationFromName.map.GetLayer("Back").TileWidth - 20), (float) random.Next(5, locationFromName.map.GetLayer("Back").TileHeight - 4));
          for (int x = (int) this.targetLocation.X; (double) x <= (double) this.targetLocation.X + 1.0; ++x)
          {
            for (int y = (int) this.targetLocation.Y; (double) y <= (double) this.targetLocation.Y + 1.0; ++y)
            {
              Vector2 tile = new Vector2((float) x, (float) y);
              if (!locationFromName.isTileOpenBesidesTerrainFeatures(tile) || !locationFromName.isTileOpenBesidesTerrainFeatures(new Vector2(tile.X + 1f, tile.Y)) || (!locationFromName.isTileOpenBesidesTerrainFeatures(new Vector2(tile.X + 1f, tile.Y - 1f)) || !locationFromName.isTileOpenBesidesTerrainFeatures(new Vector2(tile.X, tile.Y - 1f))) || (locationFromName.doesTileHaveProperty((int) tile.X, (int) tile.Y, "Water", "Back") != null || locationFromName.doesTileHaveProperty((int) tile.X + 1, (int) tile.Y, "Water", "Back") != null))
                return true;
            }
          }
          break;
        case 2:
          this.soundName = "dogs";
          if (random.NextDouble() < 0.5)
            return true;
          foreach (Building building in locationFromName.buildings)
          {
            if (building.indoors != null && building.indoors is AnimalHouse && (!building.animalDoorOpen && (building.indoors as AnimalHouse).animalsThatLiveHere.Count > (building.indoors as AnimalHouse).animals.Count) && random.NextDouble() < 1.0 / (double) locationFromName.buildings.Count)
            {
              this.targetBuilding = building;
              break;
            }
          }
          return this.targetBuilding == null;
        case 3:
          this.soundName = "owl";
          for (int index = 50; index > 0; --index)
          {
            this.targetLocation = new Vector2((float) random.Next(5, locationFromName.map.GetLayer("Back").TileWidth - 4), (float) random.Next(5, locationFromName.map.GetLayer("Back").TileHeight - 4));
            if (!locationFromName.isTileLocationTotallyClearAndPlaceable(this.targetLocation))
              return true;
          }
          break;
        case 4:
          this.soundName = "thunder_small";
          this.message = Game1.content.LoadString("Strings\\Events:SoundInTheNight_Earthquake");
          break;
      }
      Game1.freezeControls = true;
      return false;
    }

    public bool tickUpdate(GameTime time)
    {
      this.timer = this.timer + time.ElapsedGameTime.Milliseconds;
      if (this.timer > 1500 && !this.playedSound)
      {
        if (this.soundName != null && !this.soundName.Equals(""))
        {
          Game1.playSound(this.soundName);
          this.playedSound = true;
        }
        if (!this.playedSound && this.message != null)
        {
          Game1.drawObjectDialogue(this.message);
          Game1.globalFadeToClear((Game1.afterFadeFunction) null, 0.02f);
          this.showedMessage = true;
        }
      }
      if (this.timer > 7000 && !this.showedMessage)
      {
        Game1.pauseThenMessage(10, this.message, false);
        this.showedMessage = true;
      }
      if (!this.showedMessage || !this.playedSound)
        return false;
      Game1.freezeControls = false;
      return true;
    }

    public void draw(SpriteBatch b)
    {
      SpriteBatch spriteBatch = b;
      Texture2D staminaRect = Game1.staminaRect;
      int x = 0;
      int y = 0;
      Viewport viewport = Game1.graphics.GraphicsDevice.Viewport;
      int width = viewport.Width;
      viewport = Game1.graphics.GraphicsDevice.Viewport;
      int height = viewport.Height;
      Rectangle destinationRectangle = new Rectangle(x, y, width, height);
      Color black = Color.Black;
      spriteBatch.Draw(staminaRect, destinationRectangle, black);
    }

    public void makeChangesToLocation()
    {
      Farm locationFromName = Game1.getLocationFromName("Farm") as Farm;
      switch (this.behavior)
      {
        case 0:
          locationFromName.objects.Add(this.targetLocation, new StardewValley.Object(this.targetLocation, 96, false)
          {
            minutesUntilReady = 24000 - Game1.timeOfDay
          });
          break;
        case 1:
          if (locationFromName.terrainFeatures.ContainsKey(this.targetLocation))
            locationFromName.terrainFeatures.Remove(this.targetLocation);
          if (locationFromName.terrainFeatures.ContainsKey(this.targetLocation + new Vector2(1f, 0.0f)))
            locationFromName.terrainFeatures.Remove(this.targetLocation + new Vector2(1f, 0.0f));
          if (locationFromName.terrainFeatures.ContainsKey(this.targetLocation + new Vector2(1f, 1f)))
            locationFromName.terrainFeatures.Remove(this.targetLocation + new Vector2(1f, 1f));
          if (locationFromName.terrainFeatures.ContainsKey(this.targetLocation + new Vector2(0.0f, 1f)))
            locationFromName.terrainFeatures.Remove(this.targetLocation + new Vector2(0.0f, 1f));
          locationFromName.resourceClumps.Add(new ResourceClump(622, 2, 2, this.targetLocation));
          break;
        case 2:
          AnimalHouse indoors = this.targetBuilding.indoors as AnimalHouse;
          long key1 = 0;
          foreach (long key2 in indoors.animalsThatLiveHere)
          {
            if (!indoors.animals.ContainsKey(key2))
            {
              key1 = key2;
              break;
            }
          }
          if (!Game1.getFarm().animals.ContainsKey(key1))
            break;
          Game1.getFarm().animals.Remove(key1);
          indoors.animalsThatLiveHere.Remove(key1);
          using (Dictionary<long, FarmAnimal>.Enumerator enumerator = Game1.getFarm().animals.GetEnumerator())
          {
            while (enumerator.MoveNext())
              enumerator.Current.Value.moodMessage = (short) 5;
            break;
          }
        case 3:
          locationFromName.objects.Add(this.targetLocation, new StardewValley.Object(this.targetLocation, 95, false));
          break;
      }
    }

    public void drawAboveEverything(SpriteBatch b)
    {
    }
  }
}
