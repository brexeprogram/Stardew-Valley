// Decompiled with JetBrains decompiler
// Type: StardewValley.Locations.FarmHouse
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using StardewValley.BellsAndWhistles;
using StardewValley.Characters;
using StardewValley.Monsters;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using xTile;
using xTile.Dimensions;
using xTile.ObjectModel;
using xTile.Tiles;

namespace StardewValley.Locations
{
  public class FarmHouse : DecoratableLocation
  {
    public Chest fridge = new Chest(true);
    public int upgradeLevel;
    public int farmerNumberOfOwner;
    public bool fireplaceOn;
    private int currentlyDisplayedUpgradeLevel;
    private bool displayingSpouseRoom;

    public FarmHouse()
    {
    }

    public FarmHouse(int ownerNumber = 1)
    {
      this.farmerNumberOfOwner = ownerNumber;
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
      foreach (NPC character in this.characters)
      {
        if (character.isMarried())
        {
          character.checkForMarriageDialogue(timeOfDay, (GameLocation) this);
          if (Game1.timeOfDay == 2200)
          {
            character.controller = (PathFindController) null;
            character.controller = new PathFindController((Character) character, (GameLocation) this, this.getSpouseBedSpot(), 0);
            if (character.controller.pathToEndPoint == null || !this.isTileOnMap(character.controller.pathToEndPoint.Last<Point>().X, character.controller.pathToEndPoint.Last<Point>().Y))
              character.controller = (PathFindController) null;
          }
        }
        if (character is Child)
          (character as Child).tenMinuteUpdate();
      }
    }

    public Point getPorchStandingSpot()
    {
      switch (this.farmerNumberOfOwner)
      {
        case 0:
        case 1:
          return new Point(66, 15);
        default:
          return new Point(-1000, -1000);
      }
    }

    public Point getKitchenStandingSpot()
    {
      switch (this.upgradeLevel)
      {
        case 1:
          return new Point(4, 5);
        case 2:
        case 3:
          return new Point(7, 14);
        default:
          return new Point(-1000, -1000);
      }
    }

    public Point getSpouseBedSpot()
    {
      switch (this.upgradeLevel)
      {
        case 1:
          return new Point(23, 4);
        case 2:
        case 3:
          return new Point(29, 13);
        default:
          return new Point(-1000, -1000);
      }
    }

    public Point getBedSpot()
    {
      switch (this.upgradeLevel)
      {
        case 0:
          return new Point(10, 9);
        case 1:
          return new Point(22, 4);
        case 2:
        case 3:
          return new Point(28, 13);
        default:
          return new Point(-1000, -1000);
      }
    }

    public Point getEntryLocation()
    {
      switch (this.upgradeLevel)
      {
        case 0:
          return new Point(3, 11);
        case 1:
          return new Point(9, 11);
        case 2:
        case 3:
          return new Point(12, 20);
        default:
          return new Point(-1000, -1000);
      }
    }

    public Point getChildBed(int gender)
    {
      if (gender == 0)
        return new Point(23, 5);
      if (gender == 1)
        return new Point(27, 5);
      return Point.Zero;
    }

    public Point getRandomOpenPointInHouse(Random r, int buffer = 0, int tries = 30)
    {
      Point point = Point.Zero;
      for (int index = 0; index < tries; ++index)
      {
        point = new Point(r.Next(this.map.Layers[0].LayerWidth), r.Next(this.map.Layers[0].LayerHeight));
        Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle(point.X - buffer, point.Y - buffer, 1 + buffer * 2, 1 + buffer * 2);
        bool flag = false;
        for (int x = rectangle.X; x < rectangle.Right; ++x)
        {
          for (int y = rectangle.Y; y < rectangle.Bottom; ++y)
          {
            flag = this.getTileIndexAt(x, y, "Back") == -1 || !this.isTileLocationTotallyClearAndPlaceable(x, y) || Utility.pointInRectangles(FarmHouse.getWalls(this.upgradeLevel), x, y);
            if (flag)
              break;
          }
          if (flag)
            break;
        }
        if (!flag)
          return point;
      }
      return Point.Zero;
    }

    public void setSpouseInKitchen()
    {
      Farmer fromFarmerNumber = Utility.getFarmerFromFarmerNumber(this.farmerNumberOfOwner);
      NPC characterFromName = Game1.getCharacterFromName(fromFarmerNumber.spouse, false);
      if (fromFarmerNumber == null || characterFromName == null)
        return;
      Game1.warpCharacter(characterFromName, this.name, this.getKitchenStandingSpot(), false, false);
      if (Game1.player.getSpouse().gender == 0)
        characterFromName.spouseObstacleCheck(((IEnumerable<string>) Game1.content.LoadString("Data\\ExtraDialogue:Spouse_KitchenBlocked").Split('/')).First<string>(), (GameLocation) this, false);
      else
        characterFromName.spouseObstacleCheck(((IEnumerable<string>) Game1.content.LoadString("Data\\ExtraDialogue:Spouse_KitchenBlocked").Split('/')).Last<string>(), (GameLocation) this, false);
    }

    public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
    {
      if (this.map.GetLayer("Buildings").Tiles[tileLocation] != null)
      {
        int tileIndex = this.map.GetLayer("Buildings").Tiles[tileLocation].TileIndex;
        switch (tileIndex)
        {
          case 173:
            this.fridge.fridge = true;
            this.fridge.checkForAction(who, false);
            return true;
          case 794:
          case 795:
          case 796:
          case 797:
            this.fireplaceOn = !this.fireplaceOn;
            this.setFireplace(this.fireplaceOn, tileLocation.X - (tileIndex == 795 || tileIndex == 797 ? 1 : 0), tileLocation.Y, true);
            return true;
          case 2173:
            if (Game1.player.eventsSeen.Contains(463391) && Game1.player.spouse != null && Game1.player.spouse.Equals("Emily"))
            {
              TemporaryAnimatedSprite temporarySpriteById = this.getTemporarySpriteByID(5858585);
              if (temporarySpriteById != null && temporarySpriteById is EmilysParrot)
                (temporarySpriteById as EmilysParrot).doAction();
            }
            return true;
        }
      }
      return base.checkAction(tileLocation, viewport, who);
    }

    public FarmHouse(Map m, string name)
      : base(m, name)
    {
      switch (Game1.whichFarm)
      {
        case 0:
          this.furniture.Add(new Furniture(1120, new Vector2(5f, 4f)));
          this.furniture.Last<Furniture>().heldObject = (StardewValley.Object) new Furniture(1364, new Vector2(5f, 4f));
          this.furniture.Add(new Furniture(1376, new Vector2(1f, 10f)));
          this.furniture.Add(new Furniture(0, new Vector2(4f, 4f)));
          this.furniture.Add((Furniture) new TV(1466, new Vector2(1f, 4f)));
          this.furniture.Add(new Furniture(1614, new Vector2(3f, 1f)));
          this.furniture.Add(new Furniture(1618, new Vector2(6f, 8f)));
          this.furniture.Add(new Furniture(1602, new Vector2(5f, 1f)));
          SerializableDictionary<Vector2, StardewValley.Object> objects1 = this.objects;
          Vector2 key1 = new Vector2(3f, 7f);
          int coins1 = 0;
          List<Item> items1 = new List<Item>();
          items1.Add((Item) new StardewValley.Object(472, 15, false, -1, 0));
          Vector2 location1 = new Vector2(3f, 7f);
          int num1 = 1;
          Chest chest1 = new Chest(coins1, items1, location1, num1 != 0);
          objects1.Add(key1, (StardewValley.Object) chest1);
          break;
        case 1:
          this.setWallpaper(11, -1, true);
          this.setFloor(1, -1, true);
          this.furniture.Add(new Furniture(1122, new Vector2(1f, 6f)));
          this.furniture.Last<Furniture>().heldObject = (StardewValley.Object) new Furniture(1367, new Vector2(1f, 6f));
          this.furniture.Add(new Furniture(3, new Vector2(1f, 5f)));
          this.furniture.Add((Furniture) new TV(1680, new Vector2(5f, 4f)));
          this.furniture.Add(new Furniture(1673, new Vector2(1f, 1f)));
          this.furniture.Add(new Furniture(1673, new Vector2(3f, 1f)));
          this.furniture.Add(new Furniture(1676, new Vector2(5f, 1f)));
          this.furniture.Add(new Furniture(1737, new Vector2(6f, 8f)));
          this.furniture.Add(new Furniture(1742, new Vector2(5f, 5f)));
          this.furniture.Add(new Furniture(1675, new Vector2(10f, 1f)));
          SerializableDictionary<Vector2, StardewValley.Object> objects2 = this.objects;
          Vector2 key2 = new Vector2(4f, 7f);
          int coins2 = 0;
          List<Item> items2 = new List<Item>();
          items2.Add((Item) new StardewValley.Object(472, 15, false, -1, 0));
          Vector2 location2 = new Vector2(4f, 7f);
          int num2 = 1;
          Chest chest2 = new Chest(coins2, items2, location2, num2 != 0);
          objects2.Add(key2, (StardewValley.Object) chest2);
          break;
        case 2:
          this.setWallpaper(92, -1, true);
          this.setFloor(34, -1, true);
          this.furniture.Add(new Furniture(1134, new Vector2(1f, 7f)));
          this.furniture.Last<Furniture>().heldObject = (StardewValley.Object) new Furniture(1748, new Vector2(1f, 7f));
          this.furniture.Add(new Furniture(3, new Vector2(1f, 6f)));
          this.furniture.Add((Furniture) new TV(1680, new Vector2(6f, 4f)));
          this.furniture.Add(new Furniture(1296, new Vector2(1f, 4f)));
          this.furniture.Add(new Furniture(1682, new Vector2(3f, 1f)));
          this.furniture.Add(new Furniture(1777, new Vector2(6f, 5f)));
          this.furniture.Add(new Furniture(1745, new Vector2(6f, 1f)));
          this.furniture.Add(new Furniture(1747, new Vector2(5f, 4f)));
          this.furniture.Add(new Furniture(1296, new Vector2(10f, 4f)));
          SerializableDictionary<Vector2, StardewValley.Object> objects3 = this.objects;
          Vector2 key3 = new Vector2(4f, 7f);
          int coins3 = 0;
          List<Item> items3 = new List<Item>();
          items3.Add((Item) new StardewValley.Object(472, 15, false, -1, 0));
          Vector2 location3 = new Vector2(4f, 7f);
          int num3 = 1;
          Chest chest3 = new Chest(coins3, items3, location3, num3 != 0);
          objects3.Add(key3, (StardewValley.Object) chest3);
          break;
        case 3:
          this.setWallpaper(12, -1, true);
          this.setFloor(18, -1, true);
          this.furniture.Add(new Furniture(1218, new Vector2(1f, 6f)));
          this.furniture.Last<Furniture>().heldObject = (StardewValley.Object) new Furniture(1368, new Vector2(1f, 6f));
          this.furniture.Add(new Furniture(1755, new Vector2(1f, 5f)));
          this.furniture.Add(new Furniture(1755, new Vector2(3f, 6f), 1));
          this.furniture.Add((Furniture) new TV(1680, new Vector2(5f, 4f)));
          this.furniture.Add(new Furniture(1751, new Vector2(5f, 10f)));
          this.furniture.Add(new Furniture(1749, new Vector2(3f, 1f)));
          this.furniture.Add(new Furniture(1753, new Vector2(5f, 1f)));
          this.furniture.Add(new Furniture(1742, new Vector2(5f, 5f)));
          SerializableDictionary<Vector2, StardewValley.Object> objects4 = this.objects;
          Vector2 key4 = new Vector2(2f, 9f);
          int coins4 = 0;
          List<Item> items4 = new List<Item>();
          items4.Add((Item) new StardewValley.Object(472, 15, false, -1, 0));
          Vector2 location4 = new Vector2(2f, 9f);
          int num4 = 1;
          Chest chest4 = new Chest(coins4, items4, location4, num4 != 0);
          objects4.Add(key4, (StardewValley.Object) chest4);
          break;
        case 4:
          this.setWallpaper(95, -1, true);
          this.setFloor(4, -1, true);
          this.furniture.Add((Furniture) new TV(1680, new Vector2(1f, 4f)));
          this.furniture.Add(new Furniture(1628, new Vector2(1f, 5f)));
          this.furniture.Add(new Furniture(1393, new Vector2(3f, 4f)));
          this.furniture.Last<Furniture>().heldObject = (StardewValley.Object) new Furniture(1369, new Vector2(3f, 4f));
          this.furniture.Add(new Furniture(1678, new Vector2(10f, 1f)));
          this.furniture.Add(new Furniture(1812, new Vector2(3f, 1f)));
          this.furniture.Add(new Furniture(1630, new Vector2(1f, 1f)));
          this.furniture.Add(new Furniture(1811, new Vector2(6f, 1f)));
          this.furniture.Add(new Furniture(1389, new Vector2(10f, 4f)));
          SerializableDictionary<Vector2, StardewValley.Object> objects5 = this.objects;
          Vector2 key5 = new Vector2(4f, 7f);
          int coins5 = 0;
          List<Item> items5 = new List<Item>();
          items5.Add((Item) new StardewValley.Object(472, 15, false, -1, 0));
          Vector2 location5 = new Vector2(4f, 7f);
          int num5 = 1;
          Chest chest5 = new Chest(coins5, items5, location5, num5 != 0);
          objects5.Add(key5, (StardewValley.Object) chest5);
          this.furniture.Add(new Furniture(1758, new Vector2(1f, 10f)));
          break;
      }
    }

    public override void UpdateWhenCurrentLocation(GameTime time)
    {
      if (this.wasUpdated)
        return;
      base.UpdateWhenCurrentLocation(time);
      this.fridge.updateWhenCurrentLocation(time);
      if (!Game1.player.isMarried())
        return;
      NPC characterFromName = this.getCharacterFromName(Game1.player.spouse);
      if (characterFromName != null && Game1.timeOfDay < 1500 && (Game1.random.NextDouble() < 0.0006 && characterFromName.controller == null) && characterFromName.Schedule == null && (!characterFromName.getTileLocation().Equals(Utility.PointToVector2(this.getSpouseBedSpot())) && this.furniture.Count > 0))
      {
        Microsoft.Xna.Framework.Rectangle boundingBox = this.furniture[Game1.random.Next(this.furniture.Count)].boundingBox;
        Vector2 v = new Vector2((float) (boundingBox.X / Game1.tileSize), (float) (boundingBox.Y / Game1.tileSize));
        int num1 = 0;
        int finalFacingDirection = -3;
        for (; num1 < 3; ++num1)
        {
          int num2 = Game1.random.Next(-1, 2);
          int num3 = Game1.random.Next(-1, 2);
          v.X += (float) num2;
          if (num2 == 0)
            v.Y += (float) num3;
          if (num2 == -1)
            finalFacingDirection = 1;
          else if (num2 == 1)
            finalFacingDirection = 3;
          else if (num3 == -1)
            finalFacingDirection = 2;
          else if (num3 == 1)
            finalFacingDirection = 0;
          if (this.isTileLocationTotallyClearAndPlaceable(v))
            break;
        }
        if (num1 < 3)
          this.getCharacterFromName(Game1.player.spouse).controller = new PathFindController((Character) this.getCharacterFromName(Game1.player.spouse), (GameLocation) this, new Point((int) v.X, (int) v.Y), finalFacingDirection);
      }
      if (characterFromName == null || characterFromName.isEmoting)
        return;
      Vector2 tileLocation1 = characterFromName.getTileLocation();
      foreach (Vector2 adjacentTilesOffset in Character.AdjacentTilesOffsets)
      {
        Vector2 tileLocation2 = tileLocation1 + adjacentTilesOffset;
        NPC npc = this.isCharacterAtTile(tileLocation2);
        if (npc != null && npc.IsMonster && !npc.name.Equals("Cat"))
        {
          characterFromName.faceGeneralDirection(tileLocation2 * new Vector2((float) Game1.tileSize, (float) Game1.tileSize), 0);
          Game1.showSwordswipeAnimation(characterFromName.facingDirection, characterFromName.position, 60f, false);
          Game1.playSound("swordswipe");
          characterFromName.shake(500);
          characterFromName.showTextAboveHead(Game1.content.LoadString("Strings\\Locations:FarmHouse_SpouseAttacked" + (object) (Game1.random.Next(12) + 1)), -1, 2, 3000, 0);
          ((Monster) npc).takeDamage(50, (int) Utility.getAwayFromPositionTrajectory(npc.GetBoundingBox(), characterFromName.position).X, (int) Utility.getAwayFromPositionTrajectory(npc.GetBoundingBox(), characterFromName.position).Y, false, 1.0);
          if (((Monster) npc).health <= 0)
          {
            this.debris.Add(new Debris(npc.sprite.Texture, Game1.random.Next(6, 16), new Vector2((float) npc.getStandingX(), (float) npc.getStandingY())));
            this.monsterDrop((Monster) npc, npc.getStandingX(), npc.getStandingY());
            this.characters.Remove(npc);
            ++Game1.stats.MonstersKilled;
            Game1.player.changeFriendship(-10, characterFromName);
          }
          else
            ((Monster) npc).shedChunks(4);
          characterFromName.CurrentDialogue.Clear();
          characterFromName.CurrentDialogue.Push(new Dialogue(Game1.content.LoadString("Data\\ExtraDialogue:Spouse_MonstersInHouse"), characterFromName));
        }
      }
    }

    public Point getFireplacePoint()
    {
      switch (this.upgradeLevel)
      {
        case 0:
          return new Point(8, 4);
        case 1:
          return new Point(26, 4);
        case 2:
        case 3:
          return new Point(2, 13);
        default:
          return new Point(-50, -50);
      }
    }

    public bool shouldShowSpouseRoom()
    {
      return Utility.getFarmerFromFarmerNumber(this.farmerNumberOfOwner) != null ? Utility.getFarmerFromFarmerNumber(this.farmerNumberOfOwner).isMarried() : Game1.player.isMarried();
    }

    public void showSpouseRoom()
    {
      int upgradeLevel = this.upgradeLevel;
      bool flag = Utility.getFarmerFromFarmerNumber(this.farmerNumberOfOwner) != null ? Utility.getFarmerFromFarmerNumber(this.farmerNumberOfOwner).isMarried() : Game1.player.isMarried();
      int num = this.displayingSpouseRoom ? 1 : 0;
      this.displayingSpouseRoom = flag;
      this.map = Game1.game1.xTileContent.Load<Map>("Maps\\FarmHouse" + (upgradeLevel == 0 ? "" : (upgradeLevel == 3 ? "2" : string.Concat((object) upgradeLevel))) + (flag ? "_marriage" : ""));
      this.map.LoadTileSheets(Game1.mapDisplayDevice);
      if (num != 0 && !this.displayingSpouseRoom)
      {
        Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle();
        switch (this.upgradeLevel)
        {
          case 1:
            rectangle = new Microsoft.Xna.Framework.Rectangle(29, 4, 6, 6);
            break;
          case 2:
          case 3:
            rectangle = new Microsoft.Xna.Framework.Rectangle(35, 13, 6, 6);
            break;
        }
        for (int x = rectangle.X; x <= rectangle.Right; ++x)
        {
          for (int y = rectangle.Y; y <= rectangle.Bottom; ++y)
          {
            Vector2 other = new Vector2((float) x, (float) y);
            for (int index = this.furniture.Count - 1; index >= 0; --index)
            {
              if (this.furniture[index].tileLocation.Equals(other))
              {
                Game1.createItemDebris((Item) this.furniture[index], new Vector2((float) rectangle.X, (float) rectangle.Center.Y) * (float) Game1.tileSize, 3, (GameLocation) null);
                this.furniture.RemoveAt(index);
              }
            }
          }
        }
      }
      this.loadObjects();
      if (upgradeLevel == 3)
      {
        this.setMapTileIndex(3, 22, 162, "Front", 0);
        this.removeTile(4, 22, "Front");
        this.removeTile(5, 22, "Front");
        this.setMapTileIndex(6, 22, 163, "Front", 0);
        this.setMapTileIndex(3, 23, 64, "Buildings", 0);
        this.setMapTileIndex(3, 24, 96, "Buildings", 0);
        this.setMapTileIndex(4, 24, 165, "Front", 0);
        this.setMapTileIndex(5, 24, 165, "Front", 0);
        this.removeTile(4, 23, "Back");
        this.removeTile(5, 23, "Back");
        this.setMapTileIndex(4, 23, 1043, "Back", 0);
        this.setMapTileIndex(5, 23, 1043, "Back", 0);
        this.setMapTileIndex(4, 24, 1075, "Back", 0);
        this.setMapTileIndex(5, 24, 1075, "Back", 0);
        this.setMapTileIndex(6, 23, 68, "Buildings", 0);
        this.setMapTileIndex(6, 24, 130, "Buildings", 0);
        this.setMapTileIndex(4, 25, 0, "Front", 0);
        this.setMapTileIndex(5, 25, 0, "Front", 0);
        this.removeTile(4, 23, "Buildings");
        this.removeTile(5, 23, "Buildings");
        this.warps.Add(new Warp(4, 25, "Cellar", 3, 2, false));
        this.warps.Add(new Warp(5, 25, "Cellar", 4, 2, false));
        if (!Game1.player.craftingRecipes.ContainsKey("Cask"))
          Game1.player.craftingRecipes.Add("Cask", 0);
      }
      if (!flag)
        return;
      this.loadSpouseRoom();
    }

    public override void resetForPlayerEntry()
    {
      base.resetForPlayerEntry();
      if (this.fireplaceOn)
      {
        Point fireplacePoint = this.getFireplacePoint();
        this.setFireplace(true, fireplacePoint.X, fireplacePoint.Y, false);
      }
      if (Game1.player.isMarried() && Game1.player.spouse.Equals("Emily") && Game1.player.eventsSeen.Contains(463391))
      {
        Vector2 location = new Vector2((float) (32 * Game1.tileSize + Game1.pixelZoom * 4), (float) (3 * Game1.tileSize - Game1.pixelZoom * 8));
        switch (this.upgradeLevel)
        {
          case 2:
          case 3:
            location = new Vector2((float) (38 * Game1.tileSize + Game1.pixelZoom * 4), (float) (12 * Game1.tileSize - Game1.pixelZoom * 8));
            break;
        }
        this.temporarySprites.Add((TemporaryAnimatedSprite) new EmilysParrot(location));
      }
      if (this.currentlyDisplayedUpgradeLevel != this.upgradeLevel)
        this.setMapForUpgradeLevel(this.upgradeLevel, false);
      if (!this.displayingSpouseRoom && this.shouldShowSpouseRoom() || this.displayingSpouseRoom && !this.shouldShowSpouseRoom())
        this.showSpouseRoom();
      this.setWallpapers();
      this.setFloors();
      if (Game1.player.currentLocation == null || !Game1.player.currentLocation.Equals((object) this) && !Game1.player.currentLocation.name.Equals("Cellar"))
      {
        switch (this.upgradeLevel)
        {
          case 1:
            Game1.player.position = new Vector2(9f, 11f) * (float) Game1.tileSize;
            break;
          case 2:
          case 3:
            Game1.player.position = new Vector2(12f, 20f) * (float) Game1.tileSize;
            break;
        }
        Game1.xLocationAfterWarp = Game1.player.getTileX();
        Game1.yLocationAfterWarp = Game1.player.getTileY();
        Game1.player.currentLocation = (GameLocation) this;
      }
      if (Game1.timeOfDay >= 2200 && this.getCharacterFromName(Game1.player.spouse) != null && !Game1.player.spouse.Contains("engaged"))
      {
        NPC npc = Game1.removeCharacterFromItsLocation(Game1.player.spouse);
        npc.position = new Vector2((float) (this.getSpouseBedSpot().X * Game1.tileSize), (float) (this.getSpouseBedSpot().Y * Game1.tileSize));
        npc.faceDirection(0);
        this.characters.Add(npc);
      }
      for (int index = this.characters.Count - 1; index >= 0; --index)
      {
        if (this.characters[index] is Pet && (!this.isTileOnMap(this.characters[index].getTileX(), this.characters[index].getTileY()) || this.getTileIndexAt(this.characters[index].getTileLocationPoint(), "Buildings") != -1 || this.getTileIndexAt(this.characters[index].getTileX() + 1, this.characters[index].getTileY(), "Buildings") != -1))
        {
          this.characters[index].faceDirection(2);
          Game1.warpCharacter(this.characters[index], "Farm", new Vector2(54f, 8f), false, false);
          break;
        }
      }
      Farm farm = Game1.getFarm();
      for (int index1 = this.characters.Count - 1; index1 >= 0; --index1)
      {
        for (int index2 = index1 - 1; index2 >= 0; --index2)
        {
          if (index1 < this.characters.Count && index2 < this.characters.Count && (this.characters[index2].Equals((object) this.characters[index1]) || this.characters[index2].name.Equals(this.characters[index1].name) && this.characters[index2].isVillager() && this.characters[index1].isVillager()) && index2 != index1)
            this.characters.RemoveAt(index2);
        }
        for (int index2 = farm.characters.Count - 1; index2 >= 0; --index2)
        {
          if (index1 < this.characters.Count && index2 < this.characters.Count && farm.characters[index2].Equals((object) this.characters[index1]))
            farm.characters.RemoveAt(index2);
        }
      }
      if (Game1.timeOfDay >= 1800)
      {
        foreach (NPC character in this.characters)
          character.isMarried();
      }
      foreach (NPC character in this.characters)
      {
        if (character is Child)
          (character as Child).resetForPlayerEntry((GameLocation) this);
        if (Game1.timeOfDay >= 2000)
        {
          character.controller = (PathFindController) null;
          character.Halt();
        }
      }
    }

    public void moveObjectsForHouseUpgrade(int whichUpgrade)
    {
      switch (whichUpgrade)
      {
        case 0:
          if (this.upgradeLevel != 1)
            break;
          this.shiftObjects(-6, 0);
          break;
        case 1:
          if (this.upgradeLevel == 0)
            this.shiftObjects(6, 0);
          if (this.upgradeLevel != 2)
            break;
          this.shiftObjects(-3, 0);
          break;
        case 2:
        case 3:
          if (this.upgradeLevel == 1)
          {
            this.shiftObjects(3, 9);
            foreach (Furniture furniture in this.furniture)
            {
              if ((double) furniture.tileLocation.X >= 10.0 && (double) furniture.tileLocation.X <= 13.0 && ((double) furniture.tileLocation.Y >= 10.0 && (double) furniture.tileLocation.Y <= 11.0))
              {
                furniture.tileLocation.X -= 3f;
                furniture.boundingBox.X -= 3 * Game1.tileSize;
                furniture.tileLocation.Y -= 9f;
                furniture.boundingBox.Y -= 9 * Game1.tileSize;
                furniture.updateDrawPosition();
              }
            }
            this.moveFurniture(27, 13, 1, 4);
            this.moveFurniture(28, 13, 2, 4);
            this.moveFurniture(29, 13, 3, 4);
            this.moveFurniture(28, 14, 7, 4);
            this.moveFurniture(29, 14, 8, 4);
            this.moveFurniture(27, 14, 4, 4);
            this.moveFurniture(28, 15, 5, 4);
            this.moveFurniture(29, 16, 6, 4);
          }
          if (this.upgradeLevel != 0)
            break;
          this.shiftObjects(9, 9);
          break;
      }
    }

    public void setMapForUpgradeLevel(int level, bool persist = false)
    {
      if (persist)
        this.upgradeLevel = level;
      this.currentlyDisplayedUpgradeLevel = level;
      bool flag = Utility.getFarmerFromFarmerNumber(this.farmerNumberOfOwner) != null ? Utility.getFarmerFromFarmerNumber(this.farmerNumberOfOwner).isMarried() : Game1.player.isMarried();
      if (this.displayingSpouseRoom && !flag)
        this.displayingSpouseRoom = false;
      this.map = Game1.game1.xTileContent.Load<Map>("Maps\\FarmHouse" + (level == 0 ? "" : (level == 3 ? "2" : string.Concat((object) level))) + (flag ? "_marriage" : ""));
      this.map.LoadTileSheets(Game1.mapDisplayDevice);
      if (flag)
        this.showSpouseRoom();
      this.loadObjects();
      if (level == 3)
      {
        this.setMapTileIndex(3, 22, 162, "Front", 0);
        this.removeTile(4, 22, "Front");
        this.removeTile(5, 22, "Front");
        this.setMapTileIndex(6, 22, 163, "Front", 0);
        this.setMapTileIndex(3, 23, 64, "Buildings", 0);
        this.setMapTileIndex(3, 24, 96, "Buildings", 0);
        this.setMapTileIndex(4, 24, 165, "Front", 0);
        this.setMapTileIndex(5, 24, 165, "Front", 0);
        this.removeTile(4, 23, "Back");
        this.removeTile(5, 23, "Back");
        this.setMapTileIndex(4, 23, 1043, "Back", 0);
        this.setMapTileIndex(5, 23, 1043, "Back", 0);
        this.setTileProperty(4, 23, "Back", "NoFurniture", "t");
        this.setTileProperty(5, 23, "Back", "NoFurniture", "t");
        this.setTileProperty(4, 23, "Back", "NPCBarrier", "t");
        this.setTileProperty(5, 23, "Back", "NPCBarrier", "t");
        this.setMapTileIndex(4, 24, 1075, "Back", 0);
        this.setMapTileIndex(5, 24, 1075, "Back", 0);
        this.setTileProperty(4, 24, "Back", "NoFurniture", "t");
        this.setTileProperty(5, 24, "Back", "NoFurniture", "t");
        this.setMapTileIndex(6, 23, 68, "Buildings", 0);
        this.setMapTileIndex(6, 24, 130, "Buildings", 0);
        this.setMapTileIndex(4, 25, 0, "Front", 0);
        this.setMapTileIndex(5, 25, 0, "Front", 0);
        this.removeTile(4, 23, "Buildings");
        this.removeTile(5, 23, "Buildings");
        this.warps.Add(new Warp(4, 25, "Cellar", 3, 2, false));
        this.warps.Add(new Warp(5, 25, "Cellar", 4, 2, false));
        if (!Game1.player.craftingRecipes.ContainsKey("Cask"))
          Game1.player.craftingRecipes.Add("Cask", 0);
      }
      if (this.wallPaper.Count > 0 && this.floor.Count > 0)
      {
        List<Microsoft.Xna.Framework.Rectangle> walls = FarmHouse.getWalls(this.upgradeLevel);
        if (persist)
        {
          while (this.wallPaper.Count < walls.Count)
            this.wallPaper.Add(0);
        }
        List<Microsoft.Xna.Framework.Rectangle> floors = FarmHouse.getFloors(this.upgradeLevel);
        if (persist)
        {
          while (this.floor.Count < floors.Count)
            this.floor.Add(0);
        }
        if (this.upgradeLevel == 1)
        {
          this.setFloor(this.floor[0], 1, true);
          this.setFloor(this.floor[0], 2, true);
          this.setFloor(this.floor[0], 3, true);
          this.setFloor(22, 0, true);
        }
        if (this.upgradeLevel == 2)
        {
          this.setWallpaper(this.wallPaper[0], 4, true);
          this.setWallpaper(this.wallPaper[2], 6, true);
          this.setWallpaper(this.wallPaper[1], 5, true);
          this.setWallpaper(11, 0, true);
          this.setWallpaper(61, 1, true);
          this.setWallpaper(61, 2, true);
          int which = this.floor[3];
          this.setFloor(this.floor[2], 5, true);
          this.setFloor(this.floor[0], 3, true);
          this.setFloor(this.floor[1], 4, true);
          this.setFloor(which, 6, true);
          this.setFloor(1, 0, true);
          this.setFloor(31, 1, true);
          this.setFloor(31, 2, true);
        }
      }
      this.lightGlows.Clear();
    }

    public void loadSpouseRoom()
    {
      NPC npc = Utility.getFarmerFromFarmerNumber(this.farmerNumberOfOwner) != null ? Utility.getFarmerFromFarmerNumber(this.farmerNumberOfOwner).getSpouse() : Game1.player.getSpouse();
      if (npc == null)
        return;
      int num = -1;
      string name = npc.name;
      // ISSUE: reference to a compiler-generated method
      uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(name);
      if (stringHash <= 1866496948U)
      {
        if (stringHash <= 1067922812U)
        {
          if ((int) stringHash != 161540545)
          {
            if ((int) stringHash != 587846041)
            {
              if ((int) stringHash == 1067922812 && name == "Sam")
                num = 9;
            }
            else if (name == "Penny")
              num = 1;
          }
          else if (name == "Sebastian")
            num = 5;
        }
        else if ((int) stringHash != 1281010426)
        {
          if ((int) stringHash != 1708213605)
          {
            if ((int) stringHash == 1866496948 && name == "Shane")
              num = 10;
          }
          else if (name == "Alex")
            num = 6;
        }
        else if (name == "Maru")
          num = 4;
      }
      else if (stringHash <= 2571828641U)
      {
        if ((int) stringHash != 2010304804)
        {
          if ((int) stringHash != -1860673204)
          {
            if ((int) stringHash == -1723138655 && name == "Emily")
              num = 11;
          }
          else if (name == "Haley")
            num = 3;
        }
        else if (name == "Harvey")
          num = 7;
      }
      else if ((int) stringHash != -1562053956)
      {
        if ((int) stringHash != -1468719973)
        {
          if ((int) stringHash == -1228790996 && name == "Elliott")
            num = 8;
        }
        else if (name == "Leah")
          num = 2;
      }
      else if (name == "Abigail")
        num = 0;
      Microsoft.Xna.Framework.Rectangle rectangle = this.upgradeLevel == 1 ? new Microsoft.Xna.Framework.Rectangle(29, 1, 6, 9) : new Microsoft.Xna.Framework.Rectangle(35, 10, 6, 9);
      Map map = Game1.game1.xTileContent.Load<Map>("Maps\\spouseRooms");
      Point point = new Point(num % 5 * 6, num / 5 * 9);
      ((IDictionary<string, PropertyValue>) this.map.Properties).Remove("DayTiles");
      ((IDictionary<string, PropertyValue>) this.map.Properties).Remove("NightTiles");
      for (int index1 = 0; index1 < rectangle.Width; ++index1)
      {
        for (int index2 = 0; index2 < rectangle.Height; ++index2)
        {
          if (map.GetLayer("Back").Tiles[point.X + index1, point.Y + index2] != null)
            this.map.GetLayer("Back").Tiles[rectangle.X + index1, rectangle.Y + index2] = (Tile) new StaticTile(this.map.GetLayer("Back"), this.map.TileSheets[0], BlendMode.Alpha, map.GetLayer("Back").Tiles[point.X + index1, point.Y + index2].TileIndex);
          if (map.GetLayer("Buildings").Tiles[point.X + index1, point.Y + index2] != null)
          {
            this.map.GetLayer("Buildings").Tiles[rectangle.X + index1, rectangle.Y + index2] = (Tile) new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, map.GetLayer("Buildings").Tiles[point.X + index1, point.Y + index2].TileIndex);
            this.adjustMapLightPropertiesForLamp(map.GetLayer("Buildings").Tiles[point.X + index1, point.Y + index2].TileIndex, rectangle.X + index1, rectangle.Y + index2, "Buildings");
          }
          else
            this.map.GetLayer("Buildings").Tiles[rectangle.X + index1, rectangle.Y + index2] = (Tile) null;
          if (index2 < rectangle.Height - 1 && map.GetLayer("Front").Tiles[point.X + index1, point.Y + index2] != null)
          {
            this.map.GetLayer("Front").Tiles[rectangle.X + index1, rectangle.Y + index2] = (Tile) new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, map.GetLayer("Front").Tiles[point.X + index1, point.Y + index2].TileIndex);
            this.adjustMapLightPropertiesForLamp(map.GetLayer("Front").Tiles[point.X + index1, point.Y + index2].TileIndex, rectangle.X + index1, rectangle.Y + index2, "Front");
          }
          else if (index2 < rectangle.Height - 1)
            this.map.GetLayer("Front").Tiles[rectangle.X + index1, rectangle.Y + index2] = (Tile) null;
          if (index1 == 4 && index2 == 4)
            this.map.GetLayer("Back").Tiles[rectangle.X + index1, rectangle.Y + index2].Properties.Add(new KeyValuePair<string, PropertyValue>("NoFurniture", new PropertyValue("T")));
        }
      }
    }

    public void playerDivorced()
    {
      this.displayingSpouseRoom = false;
    }

    public new bool isTileOnWall(int x, int y)
    {
      foreach (Microsoft.Xna.Framework.Rectangle wall in FarmHouse.getWalls(this.upgradeLevel))
      {
        if (wall.Contains(x, y))
          return true;
      }
      return false;
    }

    public static List<Microsoft.Xna.Framework.Rectangle> getWalls(int upgradeLevel)
    {
      List<Microsoft.Xna.Framework.Rectangle> rectangleList = new List<Microsoft.Xna.Framework.Rectangle>();
      switch (upgradeLevel)
      {
        case 0:
          rectangleList.Add(new Microsoft.Xna.Framework.Rectangle(1, 1, 10, 3));
          break;
        case 1:
          rectangleList.Add(new Microsoft.Xna.Framework.Rectangle(1, 1, 17, 3));
          rectangleList.Add(new Microsoft.Xna.Framework.Rectangle(18, 6, 2, 2));
          rectangleList.Add(new Microsoft.Xna.Framework.Rectangle(20, 1, 9, 3));
          break;
        case 2:
        case 3:
          rectangleList.Add(new Microsoft.Xna.Framework.Rectangle(1, 1, 12, 3));
          rectangleList.Add(new Microsoft.Xna.Framework.Rectangle(15, 1, 13, 3));
          rectangleList.Add(new Microsoft.Xna.Framework.Rectangle(13, 3, 2, 2));
          rectangleList.Add(new Microsoft.Xna.Framework.Rectangle(1, 10, 10, 3));
          rectangleList.Add(new Microsoft.Xna.Framework.Rectangle(13, 10, 8, 3));
          rectangleList.Add(new Microsoft.Xna.Framework.Rectangle(21, 15, 2, 2));
          rectangleList.Add(new Microsoft.Xna.Framework.Rectangle(23, 10, 11, 3));
          break;
      }
      return rectangleList;
    }

    public override void setWallpaper(int which, int whichRoom = -1, bool persist = false)
    {
      List<Microsoft.Xna.Framework.Rectangle> walls = FarmHouse.getWalls(this.upgradeLevel);
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

    public new int getFloorAt(Point p)
    {
      List<Microsoft.Xna.Framework.Rectangle> floors = FarmHouse.getFloors(this.upgradeLevel);
      for (int index = 0; index < floors.Count; ++index)
      {
        if (floors[index].Contains(p))
          return index;
      }
      return -1;
    }

    public new int getWallForRoomAt(Point p)
    {
      List<Microsoft.Xna.Framework.Rectangle> walls = FarmHouse.getWalls(this.upgradeLevel);
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

    public override void setFloor(int which, int whichRoom = -1, bool persist = false)
    {
      List<Microsoft.Xna.Framework.Rectangle> floors = FarmHouse.getFloors(this.upgradeLevel);
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

    public static List<Microsoft.Xna.Framework.Rectangle> getFloors(int upgradeLevel)
    {
      List<Microsoft.Xna.Framework.Rectangle> rectangleList = new List<Microsoft.Xna.Framework.Rectangle>();
      switch (upgradeLevel)
      {
        case 0:
          rectangleList.Add(new Microsoft.Xna.Framework.Rectangle(1, 3, 10, 9));
          break;
        case 1:
          rectangleList.Add(new Microsoft.Xna.Framework.Rectangle(1, 3, 6, 9));
          rectangleList.Add(new Microsoft.Xna.Framework.Rectangle(7, 3, 11, 9));
          rectangleList.Add(new Microsoft.Xna.Framework.Rectangle(18, 8, 2, 2));
          rectangleList.Add(new Microsoft.Xna.Framework.Rectangle(20, 3, 9, 8));
          break;
        case 2:
        case 3:
          rectangleList.Add(new Microsoft.Xna.Framework.Rectangle(1, 3, 12, 6));
          rectangleList.Add(new Microsoft.Xna.Framework.Rectangle(15, 3, 13, 6));
          rectangleList.Add(new Microsoft.Xna.Framework.Rectangle(13, 5, 2, 2));
          rectangleList.Add(new Microsoft.Xna.Framework.Rectangle(0, 12, 10, 11));
          rectangleList.Add(new Microsoft.Xna.Framework.Rectangle(10, 12, 11, 9));
          rectangleList.Add(new Microsoft.Xna.Framework.Rectangle(21, 17, 2, 2));
          rectangleList.Add(new Microsoft.Xna.Framework.Rectangle(23, 12, 11, 11));
          break;
      }
      return rectangleList;
    }
  }
}
