// Decompiled with JetBrains decompiler
// Type: StardewValley.Locations.DecoratableLocation
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using xTile;
using xTile.Dimensions;

namespace StardewValley.Locations
{
  public class DecoratableLocation : GameLocation
  {
    public List<int> wallPaper = new List<int>();
    public List<int> floor = new List<int>();
    public List<Furniture> furniture = new List<Furniture>();

    public DecoratableLocation()
    {
    }

    public DecoratableLocation(Map m, string name)
      : base(m, name)
    {
    }

    public override bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer, int damagesFarmer, bool glider, Character character, bool pathfinding, bool projectile = false, bool ignoreCharacterRequirement = false)
    {
      foreach (Furniture furniture in this.furniture)
      {
        if (furniture.furniture_type != 12 && furniture.getBoundingBox(furniture.tileLocation).Intersects(position))
          return true;
      }
      return base.isCollidingPosition(position, viewport, isFarmer, damagesFarmer, glider, character, pathfinding, false, false);
    }

    public override bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer, int damagesFarmer, bool glider, Character character)
    {
      if (character == null || character.willDestroyObjectsUnderfoot)
      {
        foreach (Furniture furniture in this.furniture)
        {
          if (furniture.furniture_type != 12 && furniture.getBoundingBox(furniture.tileLocation).Intersects(position))
            return true;
        }
      }
      return base.isCollidingPosition(position, viewport, isFarmer, damagesFarmer, glider, character);
    }

    public override bool isTileLocationTotallyClearAndPlaceable(Vector2 v)
    {
      Vector2 vector2 = v * (float) Game1.tileSize;
      vector2.X += (float) (Game1.tileSize / 2);
      vector2.Y += (float) (Game1.tileSize / 2);
      foreach (Furniture furniture in this.furniture)
      {
        if (furniture.furniture_type != 12 && furniture.getBoundingBox(furniture.tileLocation).Contains((int) vector2.X, (int) vector2.Y))
          return false;
      }
      return base.isTileLocationTotallyClearAndPlaceable(v);
    }

    public override void performTenMinuteUpdate(int timeOfDay)
    {
      base.performTenMinuteUpdate(timeOfDay);
      foreach (StardewValley.Object @object in this.furniture)
        @object.minutesElapsed(10, (GameLocation) this);
    }

    public override void DayUpdate(int dayOfMonth)
    {
      base.DayUpdate(dayOfMonth);
      foreach (Furniture furniture in this.furniture)
      {
        int minutes = 3000 - Game1.timeOfDay;
        furniture.minutesElapsed(minutes, (GameLocation) this);
        furniture.DayUpdate((GameLocation) this);
      }
    }

    public override bool leftClick(int x, int y, Farmer who)
    {
      if (Game1.activeClickableMenu != null)
        return false;
      for (int index1 = this.furniture.Count - 1; index1 >= 0; --index1)
      {
        if (this.furniture[index1].boundingBox.Contains(x, y) && this.furniture[index1].clicked(who))
        {
          if (this.furniture[index1].flaggedForPickUp && who.couldInventoryAcceptThisItem((Item) this.furniture[index1]))
          {
            this.furniture[index1].flaggedForPickUp = false;
            this.furniture[index1].performRemoveAction(new Vector2((float) (x / Game1.tileSize), (float) (y / Game1.tileSize)), (GameLocation) this);
            bool flag = false;
            for (int index2 = 0; index2 < 12; ++index2)
            {
              if (who.items[index2] == null)
              {
                who.items[index2] = (Item) this.furniture[index1];
                who.CurrentToolIndex = index2;
                flag = true;
                break;
              }
            }
            if (!flag)
            {
              Item inventory = who.addItemToInventory((Item) this.furniture[index1], 11);
              who.addItemToInventory(inventory);
              who.CurrentToolIndex = 11;
            }
            this.furniture.RemoveAt(index1);
            Game1.playSound("coin");
          }
          return true;
        }
      }
      return base.leftClick(x, y, who);
    }

    public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
    {
      if (base.checkAction(tileLocation, viewport, who))
        return true;
      Point point1 = new Point(tileLocation.X * Game1.tileSize, tileLocation.Y * Game1.tileSize);
      Point point2 = new Point(tileLocation.X * Game1.tileSize, (tileLocation.Y - 1) * Game1.tileSize);
      foreach (Furniture furniture in this.furniture)
      {
        if (furniture.boundingBox.Contains(point1) && furniture.furniture_type != 12)
        {
          if (who.ActiveObject == null)
            return furniture.checkForAction(who, false);
          return furniture.performObjectDropInAction(who.ActiveObject, false, who);
        }
        if (furniture.furniture_type == 6 && furniture.boundingBox.Contains(point2))
        {
          if (who.ActiveObject == null)
            return furniture.checkForAction(who, false);
          return furniture.performObjectDropInAction(who.ActiveObject, false, who);
        }
      }
      return false;
    }

    public override void UpdateWhenCurrentLocation(GameTime time)
    {
      if (this.wasUpdated)
        return;
      base.UpdateWhenCurrentLocation(time);
    }

    public override void cleanupBeforePlayerExit()
    {
      base.cleanupBeforePlayerExit();
    }

    public override void resetForPlayerEntry()
    {
      base.resetForPlayerEntry();
      if (!Game1.player.mailReceived.Contains("button_tut_1"))
      {
        Game1.player.mailReceived.Add("button_tut_1");
        Game1.onScreenMenus.Add((IClickableMenu) new ButtonTutorialMenu(0));
      }
      if (!(this is FarmHouse))
      {
        this.setWallpapers();
        this.setFloors();
      }
      if (this.getTileIndexAt(Game1.player.getTileX(), Game1.player.getTileY(), "Buildings") != -1)
        Game1.player.position.Y += (float) Game1.tileSize;
      foreach (Furniture furniture in this.furniture)
        furniture.resetOnPlayerEntry((GameLocation) this);
    }

    public override void shiftObjects(int dx, int dy)
    {
      base.shiftObjects(dx, dy);
      foreach (Furniture furniture in this.furniture)
      {
        furniture.tileLocation.X += (float) dx;
        furniture.tileLocation.Y += (float) dy;
        furniture.boundingBox.X += dx * Game1.tileSize;
        furniture.boundingBox.Y += dy * Game1.tileSize;
        furniture.updateDrawPosition();
      }
      SerializableDictionary<Vector2, TerrainFeature> serializableDictionary = new SerializableDictionary<Vector2, TerrainFeature>();
      foreach (Vector2 key in this.terrainFeatures.Keys)
        serializableDictionary.Add(new Vector2(key.X + (float) dx, key.Y + (float) dy), this.terrainFeatures[key]);
      this.terrainFeatures = serializableDictionary;
    }

    public override bool isObjectAt(int x, int y)
    {
      foreach (StardewValley.Object @object in this.furniture)
      {
        if (@object.boundingBox.Contains(x, y))
          return true;
      }
      return base.isObjectAt(x, y);
    }

    public override StardewValley.Object getObjectAt(int x, int y)
    {
      foreach (Furniture furniture in this.furniture)
      {
        if (furniture.boundingBox.Contains(x, y))
          return (StardewValley.Object) furniture;
      }
      return base.getObjectAt(x, y);
    }

    public void moveFurniture(int oldX, int oldY, int newX, int newY)
    {
      Vector2 index = new Vector2((float) oldX, (float) oldY);
      foreach (Furniture furniture in this.furniture)
      {
        if (furniture.tileLocation.Equals(index))
        {
          furniture.tileLocation = new Vector2((float) newX, (float) newY);
          furniture.boundingBox.X = newX * Game1.tileSize;
          furniture.boundingBox.Y = newY * Game1.tileSize;
          furniture.updateDrawPosition();
          return;
        }
      }
      if (!this.objects.ContainsKey(index))
        return;
      StardewValley.Object @object = this.objects[index];
      this.objects.Remove(index);
      @object.tileLocation = new Vector2((float) newX, (float) newY);
      this.objects.Add(new Vector2((float) newX, (float) newY), @object);
    }

    public bool isTileOnWall(int x, int y)
    {
      foreach (Microsoft.Xna.Framework.Rectangle wall in DecoratableLocation.getWalls())
      {
        if (wall.Contains(x, y))
          return true;
      }
      return false;
    }

    public static List<Microsoft.Xna.Framework.Rectangle> getWalls()
    {
      return new List<Microsoft.Xna.Framework.Rectangle>()
      {
        new Microsoft.Xna.Framework.Rectangle(1, 1, 11, 3)
      };
    }

    public void setFloors()
    {
      for (int whichRoom = 0; whichRoom < this.floor.Count; ++whichRoom)
        this.setFloor(this.floor[whichRoom], whichRoom, true);
    }

    public void setWallpapers()
    {
      for (int whichRoom = 0; whichRoom < this.wallPaper.Count; ++whichRoom)
        this.setWallpaper(this.wallPaper[whichRoom], whichRoom, true);
    }

    public virtual void setWallpaper(int which, int whichRoom = -1, bool persist = false)
    {
      List<Microsoft.Xna.Framework.Rectangle> walls = DecoratableLocation.getWalls();
      if (persist)
      {
        while (this.wallPaper.Count < walls.Count)
          this.wallPaper.Add(0);
        if (whichRoom == -1)
        {
          for (int index = 0; index < this.wallPaper.Count; ++index)
            this.wallPaper[index] = which;
        }
        else if (whichRoom <= this.wallPaper.Count - 1)
          this.wallPaper[whichRoom] = which;
      }
      int index1 = which % 16 + which / 16 * 48;
      if (whichRoom == -1)
      {
        foreach (Microsoft.Xna.Framework.Rectangle rectangle in walls)
        {
          for (int x = rectangle.X; x < rectangle.Right; ++x)
          {
            this.setMapTileIndex(x, rectangle.Y, index1, "Back", 0);
            this.setMapTileIndex(x, rectangle.Y + 1, index1 + 16, "Back", 0);
            if (rectangle.Height >= 3)
            {
              if (this.map.GetLayer("Buildings").Tiles[x, rectangle.Y + 2].TileSheet.Equals((object) this.map.TileSheets[2]))
                this.setMapTileIndex(x, rectangle.Y + 2, index1 + 32, "Buildings", 0);
              else
                this.setMapTileIndex(x, rectangle.Y + 2, index1 + 32, "Back", 0);
            }
          }
        }
      }
      else
      {
        Microsoft.Xna.Framework.Rectangle rectangle = walls[Math.Min(walls.Count - 1, whichRoom)];
        for (int x = rectangle.X; x < rectangle.Right; ++x)
        {
          this.setMapTileIndex(x, rectangle.Y, index1, "Back", 0);
          this.setMapTileIndex(x, rectangle.Y + 1, index1 + 16, "Back", 0);
          if (rectangle.Height >= 3)
          {
            if (this.map.GetLayer("Buildings").Tiles[x, rectangle.Y + 2].TileSheet.Equals((object) this.map.TileSheets[2]))
              this.setMapTileIndex(x, rectangle.Y + 2, index1 + 32, "Buildings", 0);
            else
              this.setMapTileIndex(x, rectangle.Y + 2, index1 + 32, "Back", 0);
          }
        }
      }
    }

    public override bool shouldShadowBeDrawnAboveBuildingsLayer(Vector2 p)
    {
      return this.getTileIndexAt((int) p.X, (int) p.Y, "Front") == -1;
    }

    public int getFloorAt(Point p)
    {
      List<Microsoft.Xna.Framework.Rectangle> floors = DecoratableLocation.getFloors();
      for (int index = 0; index < floors.Count; ++index)
      {
        if (floors[index].Contains(p))
          return index;
      }
      return -1;
    }

    public Furniture getRandomFurniture(Random r)
    {
      if (this.furniture.Count > 0)
        return this.furniture.ElementAt<Furniture>(r.Next(this.furniture.Count));
      return (Furniture) null;
    }

    public int getWallForRoomAt(Point p)
    {
      List<Microsoft.Xna.Framework.Rectangle> walls = DecoratableLocation.getWalls();
      for (int index1 = 0; index1 < 16; ++index1)
      {
        for (int index2 = 0; index2 < walls.Count; ++index2)
        {
          if (walls[index2].Contains(p))
            return index2;
        }
        --p.Y;
      }
      return -1;
    }

    public virtual void setFloor(int which, int whichRoom = -1, bool persist = false)
    {
      List<Microsoft.Xna.Framework.Rectangle> floors = DecoratableLocation.getFloors();
      if (persist)
      {
        while (this.floor.Count < floors.Count)
          this.floor.Add(0);
        if (whichRoom == -1)
        {
          for (int index = 0; index < this.floor.Count; ++index)
            this.floor[index] = which;
        }
        else
        {
          if (whichRoom > this.floor.Count - 1)
            return;
          this.floor[whichRoom] = which;
        }
      }
      int index1 = 336 + which % 8 * 2 + which / 8 * 32;
      if (whichRoom == -1)
      {
        foreach (Microsoft.Xna.Framework.Rectangle rectangle in floors)
        {
          int x = rectangle.X;
          while (x < rectangle.Right)
          {
            int y = rectangle.Y;
            while (y < rectangle.Bottom)
            {
              if (rectangle.Contains(x, y))
                this.setMapTileIndex(x, y, index1, "Back", 0);
              if (rectangle.Contains(x + 1, y))
                this.setMapTileIndex(x + 1, y, index1 + 1, "Back", 0);
              if (rectangle.Contains(x, y + 1))
                this.setMapTileIndex(x, y + 1, index1 + 16, "Back", 0);
              if (rectangle.Contains(x + 1, y + 1))
                this.setMapTileIndex(x + 1, y + 1, index1 + 17, "Back", 0);
              y += 2;
            }
            x += 2;
          }
        }
      }
      else
      {
        Microsoft.Xna.Framework.Rectangle rectangle = floors[whichRoom];
        int x = rectangle.X;
        while (x < rectangle.Right)
        {
          int y = rectangle.Y;
          while (y < rectangle.Bottom)
          {
            if (rectangle.Contains(x, y))
              this.setMapTileIndex(x, y, index1, "Back", 0);
            if (rectangle.Contains(x + 1, y))
              this.setMapTileIndex(x + 1, y, index1 + 1, "Back", 0);
            if (rectangle.Contains(x, y + 1))
              this.setMapTileIndex(x, y + 1, index1 + 16, "Back", 0);
            if (rectangle.Contains(x + 1, y + 1))
              this.setMapTileIndex(x + 1, y + 1, index1 + 17, "Back", 0);
            y += 2;
          }
          x += 2;
        }
      }
    }

    public static List<Microsoft.Xna.Framework.Rectangle> getFloors()
    {
      return new List<Microsoft.Xna.Framework.Rectangle>()
      {
        new Microsoft.Xna.Framework.Rectangle(1, 3, 11, 11)
      };
    }

    public override void draw(SpriteBatch b)
    {
      base.draw(b);
      foreach (StardewValley.Object @object in this.furniture)
        @object.draw(b, -1, -1, 1f);
    }
  }
}
