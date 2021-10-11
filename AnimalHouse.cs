// Decompiled with JetBrains decompiler
// Type: StardewValley.AnimalHouse
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Buildings;
using StardewValley.Events;
using StardewValley.Tools;
using System.Collections.Generic;
using System.Linq;
using xTile;
using xTile.Dimensions;

namespace StardewValley
{
  public class AnimalHouse : GameLocation
  {
    public SerializableDictionary<long, FarmAnimal> animals = new SerializableDictionary<long, FarmAnimal>();
    public int animalLimit = 4;
    public List<long> animalsThatLiveHere = new List<long>();
    private List<long> animalsToRemove = new List<long>();
    public Point incubatingEgg;

    public AnimalHouse()
    {
    }

    public AnimalHouse(Map m, string name)
      : base(m, name)
    {
    }

    public void updateWhenNotCurrentLocation(Building parentBuilding, GameTime time)
    {
      if (Game1.currentLocation.Equals((object) this))
        return;
      for (int index = this.animals.Count - 1; index >= 0; --index)
        this.animals.ElementAt<KeyValuePair<long, FarmAnimal>>(index).Value.updateWhenNotCurrentLocation(parentBuilding, time, (GameLocation) this);
    }

    public void incubator()
    {
      if (this.incubatingEgg.Y <= 0 && Game1.player.ActiveObject != null && Game1.player.ActiveObject.Category == -5)
      {
        this.incubatingEgg.X = 2;
        this.incubatingEgg.Y = Game1.player.ActiveObject.ParentSheetIndex;
        this.map.GetLayer("Front").Tiles[1, 2].TileIndex += Game1.player.ActiveObject.ParentSheetIndex == 180 || Game1.player.ActiveObject.ParentSheetIndex == 182 ? 2 : 1;
        Game1.throwActiveObjectDown();
      }
      else
      {
        if (Game1.player.ActiveObject != null || this.incubatingEgg.Y <= 0)
          return;
        this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:AnimalHouse_Incubator_RemoveEgg_Question"), this.createYesNoResponses(), "RemoveIncubatingEgg");
      }
    }

    public bool isFull()
    {
      return this.animalsThatLiveHere.Count >= this.animalLimit;
    }

    public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
    {
      Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle(tileLocation.X * Game1.tileSize, tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
      if (who.ActiveObject != null && who.ActiveObject.Name.Equals("Hay") && (this.doesTileHaveProperty(tileLocation.X, tileLocation.Y, "Trough", "Back") != null && !this.objects.ContainsKey(new Vector2((float) tileLocation.X, (float) tileLocation.Y))))
      {
        this.objects.Add(new Vector2((float) tileLocation.X, (float) tileLocation.Y), (Object) who.ActiveObject.getOne());
        who.reduceActiveItemByOne();
        Game1.playSound("coin");
        return false;
      }
      bool flag = base.checkAction(tileLocation, viewport, who);
      if (!flag)
      {
        foreach (KeyValuePair<long, FarmAnimal> animal in (Dictionary<long, FarmAnimal>) this.animals)
        {
          if (animal.Value.GetBoundingBox().Intersects(rectangle) && !animal.Value.wasPet)
          {
            animal.Value.pet(who);
            return true;
          }
        }
        foreach (KeyValuePair<long, FarmAnimal> animal in (Dictionary<long, FarmAnimal>) this.animals)
        {
          if (animal.Value.GetBoundingBox().Intersects(rectangle))
          {
            animal.Value.pet(who);
            return true;
          }
        }
      }
      return flag;
    }

    public override bool isTileOccupiedForPlacement(Vector2 tileLocation, Object toPlace = null)
    {
      foreach (KeyValuePair<long, FarmAnimal> animal in (Dictionary<long, FarmAnimal>) this.animals)
      {
        if (animal.Value.getTileLocation().Equals(tileLocation))
          return true;
      }
      return base.isTileOccupiedForPlacement(tileLocation, toPlace);
    }

    public override void resetForPlayerEntry()
    {
      this.resetPositionsOfAllAnimals();
      foreach (Object @object in this.objects.Values)
      {
        if (@object.bigCraftable && @object.Name.Contains("Incubator") && (@object.heldObject != null && @object.minutesUntilReady <= 0) && !this.isFull())
        {
          string str = "??";
          switch (@object.heldObject.ParentSheetIndex)
          {
            case 305:
              str = Game1.content.LoadString("Strings\\Locations:AnimalHouse_Incubator_Hatch_VoidEgg");
              break;
            case 442:
              str = Game1.content.LoadString("Strings\\Locations:AnimalHouse_Incubator_Hatch_DuckEgg");
              break;
            case 180:
            case 182:
            case 174:
            case 176:
              str = Game1.content.LoadString("Strings\\Locations:AnimalHouse_Incubator_Hatch_RegularEgg");
              break;
            case 107:
              str = Game1.content.LoadString("Strings\\Locations:AnimalHouse_Incubator_Hatch_DinosaurEgg");
              break;
          }
          this.currentEvent = new Event("none/-1000 -1000/farmer 2 9 0/pause 250/message \"" + str + "\"/pause 500/animalNaming/pause 500/end", -1);
          break;
        }
      }
      base.resetForPlayerEntry();
    }

    public Building getBuilding()
    {
      foreach (Building building in Game1.getFarm().buildings)
      {
        if (building.indoors != null && building.indoors.Equals((object) this))
          return building;
      }
      return (Building) null;
    }

    public void addNewHatchedAnimal(string name)
    {
      if (this.getBuilding() is Coop)
      {
        foreach (Object @object in this.objects.Values)
        {
          if (@object.bigCraftable && @object.Name.Contains("Incubator") && (@object.heldObject != null && @object.minutesUntilReady <= 0) && !this.isFull())
          {
            string type = "??";
            if (@object.heldObject == null)
            {
              type = "White Chicken";
            }
            else
            {
              switch (@object.heldObject.ParentSheetIndex)
              {
                case 305:
                  type = "Void Chicken";
                  break;
                case 442:
                  type = "Duck";
                  break;
                case 180:
                case 182:
                  type = "Brown Chicken";
                  break;
                case 107:
                  type = "Dinosaur";
                  break;
                case 174:
                case 176:
                  type = "White Chicken";
                  break;
              }
            }
            FarmAnimal farmAnimal = new FarmAnimal(type, MultiplayerUtility.getNewID(), Game1.player.uniqueMultiplayerID);
            farmAnimal.name = name;
            farmAnimal.displayName = name;
            Building building = this.getBuilding();
            farmAnimal.home = building;
            farmAnimal.homeLocation = new Vector2((float) building.tileX, (float) building.tileY);
            farmAnimal.setRandomPosition(farmAnimal.home.indoors);
            (building.indoors as AnimalHouse).animals.Add(farmAnimal.myID, farmAnimal);
            (building.indoors as AnimalHouse).animalsThatLiveHere.Add(farmAnimal.myID);
            @object.heldObject = (Object) null;
            @object.ParentSheetIndex = 101;
            break;
          }
        }
      }
      else if (Game1.farmEvent != null && Game1.farmEvent is QuestionEvent)
      {
        FarmAnimal farmAnimal = new FarmAnimal((Game1.farmEvent as QuestionEvent).animal.type, MultiplayerUtility.getNewID(), Game1.player.uniqueMultiplayerID);
        farmAnimal.name = name;
        farmAnimal.displayName = name;
        farmAnimal.parentId = (Game1.farmEvent as QuestionEvent).animal.myID;
        Building building = this.getBuilding();
        farmAnimal.home = building;
        farmAnimal.homeLocation = new Vector2((float) building.tileX, (float) building.tileY);
        (Game1.farmEvent as QuestionEvent).forceProceed = true;
        farmAnimal.setRandomPosition(farmAnimal.home.indoors);
        (building.indoors as AnimalHouse).animals.Add(farmAnimal.myID, farmAnimal);
        (building.indoors as AnimalHouse).animalsThatLiveHere.Add(farmAnimal.myID);
      }
      if (Game1.currentLocation.currentEvent != null)
        ++Game1.currentLocation.currentEvent.CurrentCommand;
      Game1.exitActiveMenu();
    }

    public override bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer, int damagesFarmer, bool glider, Character character, bool pathfinding, bool projectile = false, bool ignoreCharacterRequirement = false)
    {
      foreach (KeyValuePair<long, FarmAnimal> animal in (Dictionary<long, FarmAnimal>) this.animals)
      {
        if (character != null && !character.Equals((object) animal.Value) && position.Intersects(animal.Value.GetBoundingBox()))
        {
          animal.Value.farmerPushing();
          return true;
        }
      }
      return base.isCollidingPosition(position, viewport, isFarmer, damagesFarmer, glider, character, pathfinding, false, false);
    }

    public override void UpdateWhenCurrentLocation(GameTime time)
    {
      base.UpdateWhenCurrentLocation(time);
      foreach (KeyValuePair<long, FarmAnimal> animal in (Dictionary<long, FarmAnimal>) this.animals)
      {
        if (animal.Value.updateWhenCurrentLocation(time, (GameLocation) this))
          this.animalsToRemove.Add(animal.Key);
      }
      for (int index = 0; index < this.animalsToRemove.Count; ++index)
        this.animals.Remove(this.animalsToRemove[index]);
      this.animalsToRemove.Clear();
    }

    public void resetPositionsOfAllAnimals()
    {
      foreach (KeyValuePair<long, FarmAnimal> animal in (Dictionary<long, FarmAnimal>) this.animals)
        animal.Value.setRandomPosition((GameLocation) this);
    }

    public override bool dropObject(Object obj, Vector2 location, xTile.Dimensions.Rectangle viewport, bool initialPlacement, Farmer who = null)
    {
      Vector2 key = new Vector2((float) (int) ((double) location.X / (double) Game1.tileSize), (float) (int) ((double) location.Y / (double) Game1.tileSize));
      if (!obj.Name.Equals("Hay") || this.doesTileHaveProperty((int) key.X, (int) key.Y, "Trough", "Back") == null)
        return base.dropObject(obj, location, viewport, initialPlacement, (Farmer) null);
      if (this.objects.ContainsKey(key))
        return false;
      this.objects.Add(key, obj);
      return true;
    }

    public void feedAllAnimals()
    {
      int num = 0;
      for (int xTile = 0; xTile < this.map.Layers[0].LayerWidth; ++xTile)
      {
        for (int yTile = 0; yTile < this.map.Layers[0].LayerHeight; ++yTile)
        {
          if (this.doesTileHaveProperty(xTile, yTile, "Trough", "Back") != null)
          {
            Vector2 key = new Vector2((float) xTile, (float) yTile);
            if (!this.objects.ContainsKey(key) && Game1.getFarm().piecesOfHay > 0)
            {
              this.objects.Add(key, new Object(178, 1, false, -1, 0));
              ++num;
              --Game1.getFarm().piecesOfHay;
            }
            if (num >= this.animalLimit)
              return;
          }
        }
      }
    }

    public override void DayUpdate(int dayOfMonth)
    {
      base.DayUpdate(dayOfMonth);
      foreach (KeyValuePair<long, FarmAnimal> animal in (Dictionary<long, FarmAnimal>) this.animals)
        animal.Value.dayUpdate((GameLocation) this);
    }

    public override bool performToolAction(Tool t, int tileX, int tileY)
    {
      if (t is MeleeWeapon)
      {
        foreach (FarmAnimal farmAnimal in this.animals.Values)
        {
          if (farmAnimal.GetBoundingBox().Intersects((t as MeleeWeapon).mostRecentArea))
            farmAnimal.hitWithWeapon(t as MeleeWeapon);
        }
      }
      return false;
    }

    public override void draw(SpriteBatch b)
    {
      base.draw(b);
      foreach (KeyValuePair<long, FarmAnimal> animal in (Dictionary<long, FarmAnimal>) this.animals)
        animal.Value.draw(b);
    }
  }
}
