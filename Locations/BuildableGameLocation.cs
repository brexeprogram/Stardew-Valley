// Decompiled with JetBrains decompiler
// Type: StardewValley.Locations.BuildableGameLocation
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Buildings;
using StardewValley.Characters;
using System.Collections.Generic;
using xTile;
using xTile.Dimensions;

namespace StardewValley.Locations
{
  public class BuildableGameLocation : GameLocation
  {
    public List<Building> buildings = new List<Building>();
    private Microsoft.Xna.Framework.Rectangle caveNoBuildRect = new Microsoft.Xna.Framework.Rectangle(32, 8, 5, 3);
    private Microsoft.Xna.Framework.Rectangle shippingAreaNoBuildRect = new Microsoft.Xna.Framework.Rectangle(69, 10, 4, 4);

    public BuildableGameLocation()
    {
    }

    public BuildableGameLocation(Map m, string name)
      : base(m, name)
    {
    }

    public override void DayUpdate(int dayOfMonth)
    {
      base.DayUpdate(dayOfMonth);
      foreach (Building building in this.buildings)
        building.dayUpdate(dayOfMonth);
    }

    public virtual void timeUpdate(int timeElapsed)
    {
      foreach (Building building in this.buildings)
      {
        if (building.indoors != null && building.indoors.GetType() == typeof (AnimalHouse))
        {
          foreach (KeyValuePair<long, FarmAnimal> animal in (Dictionary<long, FarmAnimal>) ((AnimalHouse) building.indoors).animals)
            animal.Value.updatePerTenMinutes(Game1.timeOfDay, building.indoors);
        }
      }
    }

    public Building getBuildingAt(Vector2 tile)
    {
      for (int index = this.buildings.Count - 1; index >= 0; --index)
      {
        if (!this.buildings[index].isTilePassable(tile))
          return this.buildings[index];
      }
      return (Building) null;
    }

    public bool destroyStructure(Vector2 tile)
    {
      for (int index = this.buildings.Count - 1; index >= 0; --index)
      {
        if (!this.buildings[index].isTilePassable(tile))
        {
          this.buildings[index].performActionOnDemolition((GameLocation) this);
          this.buildings.RemoveAt(index);
          return true;
        }
      }
      return false;
    }

    public bool destroyStructure(Building b)
    {
      b.performActionOnDemolition((GameLocation) this);
      return this.buildings.Remove(b);
    }

    public override bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer, int damagesFarmer, bool glider, Character character, bool pathfinding, bool projectile = false, bool ignoreCharacterRequirement = false)
    {
      if (!glider)
      {
        foreach (Building building in this.buildings)
        {
          if (building.intersects(position))
          {
            if (character != null && character.GetType() == typeof (FarmAnimal))
            {
              Microsoft.Xna.Framework.Rectangle rectForAnimalDoor = building.getRectForAnimalDoor();
              rectForAnimalDoor.Height += Game1.tileSize;
              if (rectForAnimalDoor.Contains(position) && building.buildingType.Contains((character as FarmAnimal).buildingTypeILiveIn))
                continue;
            }
            else if (character != null && character is JunimoHarvester)
            {
              Microsoft.Xna.Framework.Rectangle rectForAnimalDoor = building.getRectForAnimalDoor();
              rectForAnimalDoor.Height += Game1.tileSize;
              if (rectForAnimalDoor.Contains(position))
                continue;
            }
            return true;
          }
        }
      }
      return base.isCollidingPosition(position, viewport, isFarmer, damagesFarmer, glider, character, pathfinding, projectile, ignoreCharacterRequirement);
    }

    public override bool isActionableTile(int xTile, int yTile, Farmer who)
    {
      foreach (Building building in this.buildings)
      {
        if (building.isActionableTile(xTile, yTile, who))
          return true;
      }
      return base.isActionableTile(xTile, yTile, who);
    }

    public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
    {
      foreach (Building building in this.buildings)
      {
        if (building.doAction(new Vector2((float) tileLocation.X, (float) tileLocation.Y), who))
          return true;
      }
      return base.checkAction(tileLocation, viewport, who);
    }

    public override bool isTileOccupied(Vector2 tileLocation, string characterToIngore = "")
    {
      foreach (Building building in this.buildings)
      {
        if (!building.isTilePassable(tileLocation))
          return true;
      }
      return base.isTileOccupied(tileLocation, "");
    }

    public override bool isTileOccupiedForPlacement(Vector2 tileLocation, Object toPlace = null)
    {
      foreach (Building building in this.buildings)
      {
        if (!building.isTilePassable(tileLocation))
          return true;
      }
      return base.isTileOccupiedForPlacement(tileLocation, toPlace);
    }

    public override void updateEvenIfFarmerIsntHere(GameTime time, bool skipWasUpdatedFlush = false)
    {
      base.updateEvenIfFarmerIsntHere(time, skipWasUpdatedFlush);
      foreach (Building building in this.buildings)
        building.updateWhenFarmNotCurrentLocation(time);
    }

    public override void UpdateWhenCurrentLocation(GameTime time)
    {
      if (this.wasUpdated && (int) Game1.gameMode != 0)
        return;
      base.UpdateWhenCurrentLocation(time);
      foreach (Building building in this.buildings)
        building.Update(time);
    }

    public override void draw(SpriteBatch b)
    {
      base.draw(b);
      foreach (Building building in this.buildings)
        building.draw(b);
    }

    public void tryToUpgrade(Building toUpgrade, BluePrint blueprint)
    {
      if (toUpgrade != null && blueprint.name != null && toUpgrade.buildingType.Equals(blueprint.nameOfBuildingToUpgrade))
      {
        if (toUpgrade.indoors.farmers.Count > 0)
        {
          Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\Locations:BuildableLocation_CantUpgrade_SomeoneInside"), Color.Red, 3500f));
        }
        else
        {
          toUpgrade.indoors.map = Game1.game1.xTileContent.Load<Map>("Maps\\" + blueprint.mapToWarpTo);
          toUpgrade.indoors.name = blueprint.mapToWarpTo;
          toUpgrade.indoors.isStructure = true;
          toUpgrade.nameOfIndoorsWithoutUnique = blueprint.mapToWarpTo;
          toUpgrade.buildingType = blueprint.name;
          toUpgrade.texture = blueprint.texture;
          if (toUpgrade.indoors.GetType() == typeof (AnimalHouse))
            ((AnimalHouse) toUpgrade.indoors).resetPositionsOfAllAnimals();
          Game1.playSound("axe");
          blueprint.consumeResources();
          toUpgrade.performActionOnUpgrade((GameLocation) this);
          toUpgrade.color = Color.White;
          Game1.exitActiveMenu();
          if (!Game1.IsMultiplayer)
            return;
          MultiplayerUtility.broadcastBuildingChange((byte) 2, new Vector2((float) toUpgrade.tileX, (float) toUpgrade.tileY), blueprint.name, Game1.currentLocation.name, Game1.player.uniqueMultiplayerID);
        }
      }
      else
      {
        if (toUpgrade == null)
          return;
        Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\Locations:BuildableLocation_CantUpgrade_IncorrectBuildingType"), Color.Red, 3500f));
      }
    }

    public override void resetForPlayerEntry()
    {
      base.resetForPlayerEntry();
      foreach (Building building in this.buildings)
        building.performActionOnPlayerLocationEntry();
    }

    public bool isBuildingConstructed(string name)
    {
      foreach (Building building in this.buildings)
      {
        if (building.buildingType.Equals(name) && building.daysOfConstructionLeft <= 0 && building.daysUntilUpgrade <= 0)
          return true;
      }
      return false;
    }

    public bool isThereABuildingUnderConstruction()
    {
      foreach (Building building in this.buildings)
      {
        if (building.daysOfConstructionLeft > 0 || building.daysUntilUpgrade > 0)
          return true;
      }
      return false;
    }

    public Building getBuildingUnderConstruction()
    {
      foreach (Building building in this.buildings)
      {
        if (building.daysOfConstructionLeft > 0 || building.daysUntilUpgrade > 0)
          return building;
      }
      return (Building) null;
    }

    public bool buildStructure(Building b, Vector2 tileLocation, bool serverMessage, Farmer who)
    {
      if ((!serverMessage ? 0 : (Game1.IsClient ? 1 : 0)) == 0)
      {
        for (int index1 = 0; index1 < b.tilesHigh; ++index1)
        {
          for (int index2 = 0; index2 < b.tilesWide; ++index2)
          {
            if (!this.isBuildable(new Vector2(tileLocation.X + (float) index2, tileLocation.Y + (float) index1)))
              return false;
            for (int index3 = 0; index3 < this.farmers.Count; ++index3)
            {
              if (this.farmers[index3].GetBoundingBox().Intersects(new Microsoft.Xna.Framework.Rectangle(index2 * Game1.tileSize, index1 * Game1.tileSize, Game1.tileSize, Game1.tileSize)))
                return false;
            }
          }
        }
        if (Game1.IsMultiplayer)
        {
          MultiplayerUtility.broadcastBuildingChange((byte) 0, tileLocation, b.buildingType, this.name, who.uniqueMultiplayerID);
          if (Game1.IsClient)
            return false;
        }
      }
      string message = b.isThereAnythingtoPreventConstruction((GameLocation) this);
      if (message != null)
      {
        Game1.addHUDMessage(new HUDMessage(message, Color.Red, 3500f));
        return false;
      }
      b.tileX = (int) tileLocation.X;
      b.tileY = (int) tileLocation.Y;
      if (b.indoors != null && b.indoors is AnimalHouse)
      {
        foreach (long index in (b.indoors as AnimalHouse).animalsThatLiveHere)
        {
          FarmAnimal animal1 = Utility.getAnimal(index);
          if (animal1 != null)
          {
            animal1.homeLocation = tileLocation;
            animal1.home = b;
          }
          else if (animal1 == null && (b.indoors as AnimalHouse).animals.ContainsKey(index))
          {
            FarmAnimal animal2 = (b.indoors as AnimalHouse).animals[index];
            animal2.homeLocation = tileLocation;
            animal2.home = b;
          }
        }
      }
      if (b.indoors != null)
      {
        foreach (Warp warp in b.indoors.warps)
        {
          int num1 = b.humanDoor.X + b.tileX;
          warp.TargetX = num1;
          int num2 = b.humanDoor.Y + b.tileY + 1;
          warp.TargetY = num2;
        }
      }
      this.buildings.Add(b);
      return true;
    }

    public bool isBuildable(Vector2 tileLocation)
    {
      return (!Game1.player.getTileLocation().Equals(tileLocation) || !Game1.player.currentLocation.Equals((object) this)) && (!this.isTileOccupied(tileLocation, "") && this.isTilePassable(new Location((int) tileLocation.X, (int) tileLocation.Y), Game1.viewport)) && (this.doesTileHaveProperty((int) tileLocation.X, (int) tileLocation.Y, "NoFurniture", "Back") == null && !this.caveNoBuildRect.Contains(Utility.Vector2ToPoint(tileLocation)) && !this.shippingAreaNoBuildRect.Contains(Utility.Vector2ToPoint(tileLocation))) && (Game1.currentLocation.doesTileHavePropertyNoNull((int) tileLocation.X, (int) tileLocation.Y, "Buildable", "Back").ToLower().Equals("t") || Game1.currentLocation.doesTileHavePropertyNoNull((int) tileLocation.X, (int) tileLocation.Y, "Buildable", "Back").ToLower().Equals("true") || Game1.currentLocation.doesTileHaveProperty((int) tileLocation.X, (int) tileLocation.Y, "Diggable", "Back") != null && !Game1.currentLocation.doesTileHavePropertyNoNull((int) tileLocation.X, (int) tileLocation.Y, "Buildable", "Back").ToLower().Equals("f"));
    }

    public bool buildStructure(BluePrint structureForPlacement, Vector2 tileLocation, bool serverMessage, Farmer who, bool magicalConstruction = false)
    {
      if ((!serverMessage ? 0 : (Game1.IsClient ? 1 : 0)) == 0)
      {
        for (int index1 = 0; index1 < structureForPlacement.tilesHeight; ++index1)
        {
          for (int index2 = 0; index2 < structureForPlacement.tilesWidth; ++index2)
          {
            if (!this.isBuildable(new Vector2(tileLocation.X + (float) index2, tileLocation.Y + (float) index1)))
              return false;
            for (int index3 = 0; index3 < this.farmers.Count; ++index3)
            {
              if (this.farmers[index3].GetBoundingBox().Intersects(new Microsoft.Xna.Framework.Rectangle(index2 * Game1.tileSize, index1 * Game1.tileSize, Game1.tileSize, Game1.tileSize)))
                return false;
            }
          }
        }
        if (Game1.IsMultiplayer)
        {
          MultiplayerUtility.broadcastBuildingChange((byte) 0, tileLocation, structureForPlacement.name, this.name, who.uniqueMultiplayerID);
          if (Game1.IsClient)
            return false;
        }
      }
      string name = structureForPlacement.name;
      // ISSUE: reference to a compiler-generated method
      uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(name);
      Building building;
      if (stringHash <= 1972213674U)
      {
        if (stringHash <= 846075854U)
        {
          if ((int) stringHash != 45101750)
          {
            if ((int) stringHash != 846075854 || !(name == "Big Barn"))
              goto label_39;
            else
              goto label_36;
          }
          else if (name == "Stable")
          {
            building = (Building) new Stable(structureForPlacement, tileLocation);
            goto label_40;
          }
          else
            goto label_39;
        }
        else if ((int) stringHash != 1684694008)
        {
          if ((int) stringHash != 1972213674 || !(name == "Big Coop"))
            goto label_39;
        }
        else if (!(name == "Coop"))
          goto label_39;
      }
      else if (stringHash <= 2601855023U)
      {
        if ((int) stringHash != -1719902568)
        {
          if ((int) stringHash != -1693112273 || !(name == "Deluxe Barn"))
            goto label_39;
          else
            goto label_36;
        }
        else if (name == "Junimo Hut")
        {
          building = (Building) new JunimoHut(structureForPlacement, tileLocation);
          goto label_40;
        }
        else
          goto label_39;
      }
      else if ((int) stringHash != -1111878468)
      {
        if ((int) stringHash != -560689829)
        {
          if ((int) stringHash == -361784093 && name == "Mill")
          {
            building = (Building) new Mill(structureForPlacement, tileLocation);
            goto label_40;
          }
          else
            goto label_39;
        }
        else if (!(name == "Deluxe Coop"))
          goto label_39;
      }
      else if (name == "Barn")
        goto label_36;
      else
        goto label_39;
      building = (Building) new Coop(structureForPlacement, tileLocation);
      goto label_40;
label_36:
      building = (Building) new Barn(structureForPlacement, tileLocation);
      goto label_40;
label_39:
      building = new Building(structureForPlacement, tileLocation);
label_40:
      building.owner = who.uniqueMultiplayerID;
      string message = building.isThereAnythingtoPreventConstruction((GameLocation) this);
      if (message != null)
      {
        Game1.addHUDMessage(new HUDMessage(message, Color.Red, 3500f));
        return false;
      }
      this.buildings.Add(building);
      building.performActionOnConstruction((GameLocation) this);
      return true;
    }
  }
}
