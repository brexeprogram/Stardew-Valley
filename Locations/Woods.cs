// Decompiled with JetBrains decompiler
// Type: StardewValley.Locations.Woods
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;
using StardewValley.Monsters;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using xTile;
using xTile.Dimensions;
using xTile.ObjectModel;
using xTile.Tiles;

namespace StardewValley.Locations
{
  public class Woods : GameLocation
  {
    public List<ResourceClump> stumps = new List<ResourceClump>();
    public const int numBaubles = 25;
    private List<Vector2> baubles;
    private List<WeatherDebris> weatherDebris;
    public bool hasUnlockedStatue;
    public bool hasFoundStardrop;
    private bool addedSlimesToday;
    private int statueTimer;

    public Woods()
    {
    }

    public Woods(Map map, string name)
      : base(map, name)
    {
      this.isOutdoors = true;
      this.ignoreDebrisWeather = true;
      this.ignoreOutdoorLighting = true;
    }

    public override void checkForMusic(GameTime time)
    {
      if (Game1.currentSong != null && Game1.currentSong.IsPlaying || Game1.nextMusicTrack != null && Game1.nextMusicTrack.Length != 0)
        return;
      if (Game1.isRaining)
        Game1.changeMusicTrack("rain");
      else
        Game1.changeMusicTrack(Game1.currentSeason + "_day_ambient");
    }

    public void statueAnimation(Farmer who)
    {
      this.hasUnlockedStatue = true;
      who.reduceActiveItemByOne();
      this.temporarySprites.Add(new TemporaryAnimatedSprite(10, new Vector2(8f, 7f) * (float) Game1.tileSize, Color.White, 9, false, 50f, 0, -1, -1f, -1, 0));
      this.temporarySprites.Add(new TemporaryAnimatedSprite(10, new Vector2(9f, 7f) * (float) Game1.tileSize, Color.Orange, 9, false, 70f, 0, -1, -1f, -1, 0));
      this.temporarySprites.Add(new TemporaryAnimatedSprite(10, new Vector2(8f, 6f) * (float) Game1.tileSize, Color.White, 9, false, 60f, 0, -1, -1f, -1, 0));
      this.temporarySprites.Add(new TemporaryAnimatedSprite(10, new Vector2(9f, 6f) * (float) Game1.tileSize, Color.OrangeRed, 9, false, 120f, 0, -1, -1f, -1, 0));
      this.temporarySprites.Add(new TemporaryAnimatedSprite(10, new Vector2(8f, 5f) * (float) Game1.tileSize, Color.Red, 9, false, 100f, 0, -1, -1f, -1, 0));
      this.temporarySprites.Add(new TemporaryAnimatedSprite(10, new Vector2(9f, 5f) * (float) Game1.tileSize, Color.White, 9, false, 170f, 0, -1, -1f, -1, 0));
      this.temporarySprites.Add(new TemporaryAnimatedSprite(11, new Vector2((float) (8 * Game1.tileSize + Game1.tileSize / 2), (float) (7 * Game1.tileSize + Game1.tileSize / 4)), Color.Orange, 9, false, 40f, 0, -1, -1f, -1, 0));
      this.temporarySprites.Add(new TemporaryAnimatedSprite(11, new Vector2((float) (9 * Game1.tileSize + Game1.tileSize / 2), (float) (7 * Game1.tileSize + Game1.tileSize / 4)), Color.White, 9, false, 90f, 0, -1, -1f, -1, 0));
      this.temporarySprites.Add(new TemporaryAnimatedSprite(11, new Vector2((float) (8 * Game1.tileSize + Game1.tileSize / 2), (float) (6 * Game1.tileSize + Game1.tileSize / 4)), Color.OrangeRed, 9, false, 190f, 0, -1, -1f, -1, 0));
      this.temporarySprites.Add(new TemporaryAnimatedSprite(11, new Vector2((float) (9 * Game1.tileSize + Game1.tileSize / 2), (float) (6 * Game1.tileSize + Game1.tileSize / 4)), Color.White, 9, false, 80f, 0, -1, -1f, -1, 0));
      this.temporarySprites.Add(new TemporaryAnimatedSprite(11, new Vector2((float) (8 * Game1.tileSize + Game1.tileSize / 2), (float) (5 * Game1.tileSize + Game1.tileSize / 4)), Color.Red, 9, false, 69f, 0, -1, -1f, -1, 0));
      this.temporarySprites.Add(new TemporaryAnimatedSprite(11, new Vector2((float) (9 * Game1.tileSize + Game1.tileSize / 2), (float) (5 * Game1.tileSize + Game1.tileSize / 4)), Color.OrangeRed, 9, false, 130f, 0, -1, -1f, -1, 0));
      this.temporarySprites.Add(new TemporaryAnimatedSprite(10, new Vector2((float) (7 * Game1.tileSize + Game1.tileSize / 2), (float) (7 * Game1.tileSize + Game1.tileSize / 4)), Color.Orange, 9, false, 40f, 0, -1, -1f, -1, 0));
      this.temporarySprites.Add(new TemporaryAnimatedSprite(11, new Vector2((float) (10 * Game1.tileSize + Game1.tileSize / 2), (float) (6 * Game1.tileSize - Game1.tileSize / 4)), Color.White, 9, false, 90f, 0, -1, -1f, -1, 0));
      this.temporarySprites.Add(new TemporaryAnimatedSprite(10, new Vector2((float) (7 * Game1.tileSize + Game1.tileSize / 2), (float) (7 * Game1.tileSize + Game1.tileSize / 4)), Color.Red, 9, false, 30f, 0, -1, -1f, -1, 0));
      this.temporarySprites.Add(new TemporaryAnimatedSprite(11, new Vector2((float) (10 * Game1.tileSize + Game1.tileSize / 2), (float) (6 * Game1.tileSize - Game1.tileSize / 4)), Color.White, 9, false, 180f, 0, -1, -1f, -1, 0));
      Game1.playSound("secret1");
      this.map.GetLayer("Front").Tiles[8, 6].TileIndex += 2;
      this.map.GetLayer("Front").Tiles[9, 6].TileIndex += 2;
    }

    public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
    {
      Tile tile = this.map.GetLayer("Buildings").PickTile(new Location(tileLocation.X * Game1.tileSize, tileLocation.Y * Game1.tileSize), viewport.Size);
      if (tile != null && who.IsMainPlayer)
      {
        switch (tile.TileIndex)
        {
          case 1140:
          case 1141:
            if (!this.hasUnlockedStatue)
            {
              if (who.ActiveObject != null && who.ActiveObject.ParentSheetIndex == 417)
              {
                this.statueTimer = 1000;
                who.freezePause = 1000;
                who.FarmerSprite.ignoreDefaultActionThisTime = true;
                Game1.changeMusicTrack("none");
                Game1.playSound("newArtifact");
              }
              else
                Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Woods_Statue").Replace('\n', '^'));
            }
            if (this.hasUnlockedStatue && !this.hasFoundStardrop && who.freeSpotsInInventory() > 0)
            {
              who.addItemByMenuIfNecessaryElseHoldUp((Item) new StardewValley.Object(434, 1, false, -1, 0), (ItemGrabMenu.behaviorOnItemSelect) null);
              this.hasFoundStardrop = true;
              if (!Game1.player.mailReceived.Contains("CF_Statue"))
                Game1.player.mailReceived.Add("CF_Statue");
            }
            return true;
        }
      }
      return base.checkAction(tileLocation, viewport, who);
    }

    public override bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer, int damagesFarmer, bool glider, Character character)
    {
      foreach (ResourceClump stump in this.stumps)
      {
        Vector2 tile = stump.tile;
        if (stump.getBoundingBox(tile).Intersects(position))
          return true;
      }
      return base.isCollidingPosition(position, viewport, isFarmer, damagesFarmer, glider, character);
    }

    public override bool performToolAction(Tool t, int tileX, int tileY)
    {
      if (t is Axe)
      {
        Point point = new Point(tileX * Game1.tileSize + Game1.tileSize / 2, tileY * Game1.tileSize + Game1.tileSize / 2);
        for (int index = this.stumps.Count - 1; index >= 0; --index)
        {
          if (this.stumps[index].getBoundingBox(this.stumps[index].tile).Contains(point))
          {
            if (this.stumps[index].performToolAction(t, 1, this.stumps[index].tile, (GameLocation) null))
              this.stumps.RemoveAt(index);
            return true;
          }
        }
      }
      return false;
    }

    public override void DayUpdate(int dayOfMonth)
    {
      base.DayUpdate(dayOfMonth);
      this.characters.Clear();
      this.addedSlimesToday = false;
      PropertyValue propertyValue;
      this.map.Properties.TryGetValue("Stumps", out propertyValue);
      if (propertyValue == null)
        return;
      string[] strArray = propertyValue.ToString().Split(' ');
      int index = 0;
      while (index < strArray.Length)
      {
        Vector2 vector2 = new Vector2((float) Convert.ToInt32(strArray[index]), (float) Convert.ToInt32(strArray[index + 1]));
        bool flag = false;
        foreach (ResourceClump stump in this.stumps)
        {
          if (stump.tile.Equals(vector2))
          {
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          this.stumps.Add(new ResourceClump(600, 2, 2, vector2));
          this.removeObject(vector2, false);
          this.removeObject(vector2 + new Vector2(1f, 0.0f), false);
          this.removeObject(vector2 + new Vector2(1f, 1f), false);
          this.removeObject(vector2 + new Vector2(0.0f, 1f), false);
        }
        index += 3;
      }
    }

    public override void cleanupBeforePlayerExit()
    {
      base.cleanupBeforePlayerExit();
      Game1.changeMusicTrack("none");
      if (this.baubles != null)
        this.baubles.Clear();
      if (this.weatherDebris == null)
        return;
      this.weatherDebris.Clear();
    }

    public override bool isTileLocationTotallyClearAndPlaceable(Vector2 v)
    {
      foreach (ResourceClump stump in this.stumps)
      {
        if (stump.occupiesTile((int) v.X, (int) v.Y))
          return false;
      }
      return base.isTileLocationTotallyClearAndPlaceable(v);
    }

    public override void resetForPlayerEntry()
    {
      if (!Game1.player.mailReceived.Contains("beenToWoods"))
        Game1.player.mailReceived.Add("beenToWoods");
      if (!this.addedSlimesToday)
      {
        this.addedSlimesToday = true;
        Random random = new Random((int) Game1.stats.DaysPlayed + (int) Game1.uniqueIDForThisGame + 12);
        for (int index = 50; index > 0; --index)
        {
          Vector2 randomTile = this.getRandomTile();
          if (random.NextDouble() < 0.25 && this.isTileLocationTotallyClearAndPlaceable(randomTile))
          {
            string currentSeason = Game1.currentSeason;
            if (!(currentSeason == "spring"))
            {
              if (!(currentSeason == "summer"))
              {
                if (!(currentSeason == "fall"))
                {
                  if (currentSeason == "winter")
                    this.characters.Add((NPC) new GreenSlime(randomTile * (float) Game1.tileSize, 40));
                }
                else
                  this.characters.Add((NPC) new GreenSlime(randomTile * (float) Game1.tileSize, random.NextDouble() < 0.5 ? 0 : 40));
              }
              else
                this.characters.Add((NPC) new GreenSlime(randomTile * (float) Game1.tileSize, 0));
            }
            else
              this.characters.Add((NPC) new GreenSlime(randomTile * (float) Game1.tileSize, 0));
          }
        }
      }
      if (Game1.timeOfDay > 1600)
      {
        this.ignoreOutdoorLighting = false;
        this.ignoreLights = true;
      }
      else
      {
        this.ignoreOutdoorLighting = true;
        this.ignoreLights = false;
      }
      base.resetForPlayerEntry();
      int num = 25 + new Random((int) Game1.stats.DaysPlayed + (int) Game1.uniqueIDForThisGame / 2).Next(0, 75);
      if (!Game1.isRaining)
      {
        this.baubles = new List<Vector2>();
        for (int index = 0; index < num; ++index)
          this.baubles.Add(new Vector2((float) Game1.random.Next(0, this.map.DisplayWidth), (float) Game1.random.Next(0, this.map.DisplayHeight)));
        if (!Game1.currentSeason.Equals("winter"))
        {
          this.weatherDebris = new List<WeatherDebris>();
          int maxValue = Game1.tileSize * 3;
          for (int index = 0; index < num; ++index)
            this.weatherDebris.Add(new WeatherDebris(new Vector2((float) (index * maxValue % Game1.graphics.GraphicsDevice.Viewport.Width + Game1.random.Next(maxValue)), (float) (index * maxValue / Game1.graphics.GraphicsDevice.Viewport.Width * maxValue + Game1.random.Next(maxValue))), 1, (float) Game1.random.Next(15) / 500f, (float) Game1.random.Next(-10, 0) / 50f, (float) Game1.random.Next(10) / 50f));
        }
      }
      if (Game1.timeOfDay < 1800)
        Game1.changeMusicTrack("woodsTheme");
      if (!this.hasUnlockedStatue || this.hasFoundStardrop)
        return;
      this.map.GetLayer("Front").Tiles[8, 6].TileIndex += 2;
      this.map.GetLayer("Front").Tiles[9, 6].TileIndex += 2;
    }

    public override void UpdateWhenCurrentLocation(GameTime time)
    {
      base.UpdateWhenCurrentLocation(time);
      TimeSpan timeSpan;
      if (this.statueTimer > 0)
      {
        int statueTimer = this.statueTimer;
        timeSpan = time.ElapsedGameTime;
        int milliseconds = timeSpan.Milliseconds;
        this.statueTimer = statueTimer - milliseconds;
        if (this.statueTimer <= 0)
          this.statueAnimation(Game1.player);
      }
      if (this.baubles != null)
      {
        for (int index = 0; index < this.baubles.Count; ++index)
        {
          Vector2 vector2 = new Vector2();
          // ISSUE: explicit reference operation
          // ISSUE: variable of a reference type
          Vector2& local = @vector2;
          double num1 = (double) this.baubles[index].X - (double) Math.Max(0.4f, Math.Min(1f, (float) index * 0.01f));
          double num2 = (double) index * 0.00999999977648258;
          double num3 = 2.0 * Math.PI;
          timeSpan = time.TotalGameTime;
          double milliseconds = (double) timeSpan.Milliseconds;
          double num4 = Math.Sin(num3 * milliseconds / 8000.0);
          double num5 = num2 * num4;
          double num6 = num1 - num5;
          // ISSUE: explicit reference operation
          (^local).X = (float) num6;
          vector2.Y = this.baubles[index].Y + Math.Max(0.5f, Math.Min(1.2f, (float) index * 0.02f));
          if ((double) vector2.Y > (double) this.map.DisplayHeight || (double) vector2.X < 0.0)
          {
            vector2.X = (float) Game1.random.Next(0, this.map.DisplayWidth);
            vector2.Y = (float) -Game1.tileSize;
          }
          this.baubles[index] = vector2;
        }
      }
      if (this.weatherDebris != null)
      {
        foreach (WeatherDebris weatherDebri in this.weatherDebris)
          weatherDebri.update();
        Game1.updateDebrisWeatherForMovement(this.weatherDebris);
      }
      foreach (ResourceClump stump in this.stumps)
        stump.tickUpdate(time, stump.tile);
    }

    public override void draw(SpriteBatch b)
    {
      base.draw(b);
      if (Game1.eventUp && (this.currentEvent == null || !this.currentEvent.showGroundObjects))
        return;
      foreach (ResourceClump stump in this.stumps)
        stump.draw(b, stump.tile);
    }

    public override void drawAboveAlwaysFrontLayer(SpriteBatch b)
    {
      base.drawAboveAlwaysFrontLayer(b);
      if (this.baubles != null)
      {
        for (int index = 0; index < this.baubles.Count; ++index)
          b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.baubles[index]), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(346 + (int) ((Game1.currentGameTime.TotalGameTime.TotalMilliseconds + (double) (index * 25)) % 600.0) / 150 * 5, 1971, 5, 5)), Color.White, (float) index * 0.3926991f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1f);
      }
      if (this.weatherDebris == null || this.currentEvent != null)
        return;
      foreach (WeatherDebris weatherDebri in this.weatherDebris)
        weatherDebri.draw(b);
    }
  }
}
