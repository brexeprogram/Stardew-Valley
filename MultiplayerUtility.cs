// Decompiled with JetBrains decompiler
// Type: StardewValley.MultiplayerUtility
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Lidgren.Network;
using Lidgren.Network.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Buildings;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using xTile.Dimensions;

namespace StardewValley
{
  public class MultiplayerUtility
  {
    public static long latestID = long.MinValue + (long) Game1.random.Next(1000);
    public const byte movement = 0;
    public const byte position = 1;
    public const byte playerIntroduction = 2;
    public const byte animation = 3;
    public const byte objectAlteration = 4;
    public const byte warpFarmer = 5;
    public const byte switchHeldItem = 6;
    public const byte toolAction = 7;
    public const byte debrisPickup = 8;
    public const byte checkAction = 9;
    public const byte chatMessage = 10;
    public const byte nameChange = 11;
    public const byte tenMinSync = 12;
    public const byte building = 13;
    public const byte debrisCreate = 14;
    public const byte npcMove = 15;
    public const byte npcBehavior = 16;
    public const byte readyConfirmation = 17;
    public const byte serverToClientsMessage = 18;
    public const byte messageToEveryone = 19;
    public const byte addObject = 0;
    public const byte removeObject = 1;
    public const byte addTerrainFeature = 2;
    public const byte removeTerrainFeature = 3;
    public const byte addBuilding = 0;
    public const byte removeBuilding = 1;
    public const byte upgradeBuilding = 2;
    public static long recentMultiplayerEntityID;
    public const string MSG_START_FESTIVAL_EVENT = "festivalEvent";
    public const string MSG_END_FESTIVAL = "endFest";
    public const string MSG_PLACEHOLDER = "[replace me]";
    public const int DANCE_PARTNER = 0;
    public const int LUAU_ITEM = 1;
    public const int GRANGE_DISPLAY_USER = 2;
    public const int GRANGE_DISPLAY_CHANGE = 3;
    public const int MSGE_GRANGE_SCORE = 4;
    public const int MSGE_ADD_MAIL_RECEIVED = 5;
    public const int MSGE_BUNDLE_COMPLETE = 6;
    public const int MSGE_ADD_MAIL_FOR_TOMORROW = 7;

    public static long getNewID()
    {
      return MultiplayerUtility.latestID++;
    }

    public static void broadcastFarmerPosition(long f, Vector2 position, string currentLocation)
    {
      foreach (KeyValuePair<long, Farmer> otherFarmer in Game1.otherFarmers)
      {
        if (otherFarmer.Value.currentLocation.name.Equals(currentLocation))
        {
          for (int index = 0; index < otherFarmer.Value.multiplayerMessage.Count; ++index)
          {
            if ((int) (byte) otherFarmer.Value.multiplayerMessage[index][0] == 1 && (long) otherFarmer.Value.multiplayerMessage[index][1] == f)
            {
              otherFarmer.Value.multiplayerMessage.RemoveAt(index);
              break;
            }
          }
          otherFarmer.Value.multiplayerMessage.Add(new object[3]
          {
            (object) (byte) 1,
            (object) f,
            (object) position
          });
        }
      }
    }

    public static void broadcastFarmerAnimation(long f, int startingFrame, int numberOfFrames, float animationSpeed, bool backwards, string currentLocation, int currentToolIndex)
    {
      foreach (KeyValuePair<long, Farmer> otherFarmer in Game1.otherFarmers)
      {
        if (otherFarmer.Value.currentLocation.name.Equals(currentLocation) && otherFarmer.Value.uniqueMultiplayerID != f)
          otherFarmer.Value.multiplayerMessage.Add(new object[7]
          {
            (object) (byte) 3,
            (object) f,
            (object) currentToolIndex,
            (object) (byte) (backwards ? 1 : 0),
            (object) startingFrame,
            (object) animationSpeed,
            (object) (byte) numberOfFrames
          });
      }
    }

    public static void broadcastFarmerMovement(long f, byte command, string currentLocation)
    {
      foreach (KeyValuePair<long, Farmer> otherFarmer in Game1.otherFarmers)
      {
        if (otherFarmer.Value.currentLocation.name.Equals(currentLocation))
          otherFarmer.Value.multiplayerMessage.Add(new object[3]
          {
            (object) (byte) 0,
            (object) f,
            (object) command
          });
      }
    }

    public static void broadcastObjectChange(short x, short y, byte command, byte terrainFeatureIndex, int extraInfo, string location)
    {
      foreach (KeyValuePair<long, Farmer> otherFarmer in Game1.otherFarmers)
      {
        if (otherFarmer.Value.currentLocation.name.Equals(location))
          otherFarmer.Value.multiplayerMessage.Add(new object[6]
          {
            (object) (byte) 4,
            (object) x,
            (object) y,
            (object) command,
            (object) terrainFeatureIndex,
            (object) extraInfo
          });
      }
    }

    public static void broadcastFarmerWarp(short x, short y, string nameOfNewLocation, bool isStructure, long id)
    {
      foreach (KeyValuePair<long, Farmer> otherFarmer in Game1.otherFarmers)
      {
        if (otherFarmer.Value.uniqueMultiplayerID != id)
          otherFarmer.Value.multiplayerMessage.Add(new object[6]
          {
            (object) (byte) 5,
            (object) x,
            (object) y,
            (object) nameOfNewLocation,
            (object) (byte) (isStructure ? 1 : 0),
            (object) id
          });
      }
    }

    public static void broadcastSwitchHeldItem(byte bigCraftable, short heldItem, long whichPlayer, string location)
    {
      foreach (KeyValuePair<long, Farmer> otherFarmer in Game1.otherFarmers)
      {
        if (otherFarmer.Value.currentLocation.name.Equals(location) && whichPlayer != otherFarmer.Value.uniqueMultiplayerID)
          otherFarmer.Value.multiplayerMessage.Add(new object[4]
          {
            (object) (byte) 6,
            (object) whichPlayer,
            (object) bigCraftable,
            (object) heldItem
          });
      }
    }

    public static void broadcastToolAction(Tool t, int tileX, int tileY, string location, byte facingDirection, short seed, long whichPlayer)
    {
      ToolDescription indexFromTool = ToolFactory.getIndexFromTool(t);
      foreach (KeyValuePair<long, Farmer> otherFarmer in Game1.otherFarmers)
      {
        if (otherFarmer.Value.currentLocation.name.Equals(location) && whichPlayer != otherFarmer.Value.uniqueMultiplayerID)
          otherFarmer.Value.multiplayerMessage.Add(new object[9]
          {
            (object) (byte) 7,
            (object) indexFromTool.index,
            (object) indexFromTool.upgradeLevel,
            (object) (short) tileX,
            (object) (short) tileY,
            (object) location,
            (object) facingDirection,
            (object) seed,
            (object) whichPlayer
          });
      }
    }

    public static NetOutgoingMessage writeData(NetOutgoingMessage sendMsg, byte which, object[] data)
    {
      sendMsg.Write(which);
      for (int index = 0; index < data.Length; ++index)
      {
        if (data[index].GetType() == typeof (Vector2))
          sendMsg.Write((Vector2) data[index]);
        else if (data[index] is byte)
          sendMsg.Write((byte) data[index]);
        else if (data[index] is int)
          sendMsg.Write((int) data[index]);
        else if (data[index] is short)
          sendMsg.Write((short) data[index]);
        else if (data[index] is float)
          sendMsg.Write((float) data[index]);
        else if (data[index] is long)
          sendMsg.Write((long) data[index]);
        else if (data[index] is string)
          sendMsg.Write((string) data[index]);
      }
      return sendMsg;
    }

    public static void broadcastGameClock()
    {
      foreach (KeyValuePair<long, Farmer> otherFarmer in Game1.otherFarmers)
        otherFarmer.Value.multiplayerMessage.Add(new object[2]
        {
          (object) (byte) 12,
          (object) (short) Game1.timeOfDay
        });
    }

    public static void receiveTenMinuteSync(short time)
    {
      Game1.timeOfDay = (int) time;
      Game1.performTenMinuteClockUpdate();
    }

    public static void sendAnimationMessageToServer(int startingFrame, int numberOfFrames, float animationSpeed, bool backwards, int currentToolIndex)
    {
      Game1.client.sendMessage((byte) 3, new object[5]
      {
        (object) currentToolIndex,
        (object) (byte) (backwards ? 1 : 0),
        (object) startingFrame,
        (object) animationSpeed,
        (object) (byte) numberOfFrames
      });
    }

    private static int translateObjectIndex(int index)
    {
      switch (index)
      {
        case -9:
          return 325;
        case -7:
          return 324;
        case -6:
          return 323;
        case -5:
          return 322;
        default:
          return index;
      }
    }

    public static void performObjectAlteration(short x, short y, byte command, byte terrainFeatureIndex, int extraInfo)
    {
      switch (command)
      {
        case 0:
          extraInfo = MultiplayerUtility.translateObjectIndex(extraInfo);
          ((int) terrainFeatureIndex != 0 ? new Object(Vector2.Zero, extraInfo, false) : new Object(Vector2.Zero, extraInfo, (string) null, true, false, false, false)).placementAction(Game1.currentLocation, (int) x * Game1.tileSize, (int) y * Game1.tileSize, (Farmer) null);
          break;
        case 1:
          Object @object;
          Game1.currentLocation.objects.TryGetValue(new Vector2((float) x, (float) y), out @object);
          if (@object == null)
            break;
          Game1.currentLocation.objects.Remove(new Vector2((float) x, (float) y));
          @object.performRemoveAction(new Vector2((float) x, (float) y), Game1.currentLocation);
          break;
        case 2:
          if (Game1.currentLocation.terrainFeatures.ContainsKey(new Vector2((float) x, (float) y)))
          {
            Game1.currentLocation.terrainFeatures[new Vector2((float) x, (float) y)] = TerrainFeatureFactory.getNewTerrainFeatureFromIndex(terrainFeatureIndex, extraInfo);
            break;
          }
          Game1.currentLocation.terrainFeatures.Add(new Vector2((float) x, (float) y), TerrainFeatureFactory.getNewTerrainFeatureFromIndex(terrainFeatureIndex, extraInfo));
          break;
        case 3:
          Game1.currentLocation.terrainFeatures.Remove(new Vector2((float) x, (float) y));
          break;
      }
    }

    public static void serverTryToPerformObjectAlteration(short x, short y, byte command, byte terrainFeatureIndex, int extraInfo, Farmer actionPerformer)
    {
      switch (command)
      {
        case 0:
          extraInfo = MultiplayerUtility.translateObjectIndex(extraInfo);
          Object object1 = (int) terrainFeatureIndex != 0 ? new Object(Vector2.Zero, extraInfo, false) : new Object(Vector2.Zero, extraInfo, (string) null, true, false, false, false);
          if (!Utility.playerCanPlaceItemHere(actionPerformer.currentLocation, (Item) object1, (int) x, (int) y, actionPerformer))
            break;
          object1.placementAction(Game1.currentLocation, (int) x, (int) y, (Farmer) null);
          break;
        case 1:
          Object object2 = Game1.currentLocation.objects[new Vector2((float) x, (float) y)];
          Game1.currentLocation.objects.Remove(new Vector2((float) x, (float) y));
          if (object2 == null)
            break;
          object2.performRemoveAction(new Vector2((float) x, (float) y), Game1.currentLocation);
          break;
        case 2:
          Game1.currentLocation.terrainFeatures.Add(new Vector2((float) x, (float) y), TerrainFeatureFactory.getNewTerrainFeatureFromIndex(terrainFeatureIndex, extraInfo));
          break;
        case 3:
          Game1.currentLocation.terrainFeatures.Remove(new Vector2((float) x, (float) y));
          break;
      }
    }

    public static void performSwitchHeldItem(long id, byte bigCraftable, int index)
    {
      if (index == -1)
      {
        Game1.otherFarmers[id].showNotCarrying();
        if (Game1.otherFarmers[id].ActiveObject != null)
          Game1.otherFarmers[id].ActiveObject.actionWhenStopBeingHeld(Game1.otherFarmers[id]);
        Game1.otherFarmers[id].items[Game1.otherFarmers[id].CurrentToolIndex] = (Item) null;
      }
      else
      {
        Game1.otherFarmers[id].showCarrying();
        Game1.otherFarmers[id].ActiveObject = (int) bigCraftable == 1 ? new Object(Vector2.Zero, index, false) : new Object(Vector2.Zero, index, 1);
      }
      if (!Game1.IsServer)
        return;
      MultiplayerUtility.sendSwitchHeldItemMessage(index, bigCraftable, id);
    }

    public static void sendSwitchHeldItemMessage(int heldItemIndex, byte bigCraftable, long whichPlayer)
    {
      if (Game1.IsClient)
      {
        Game1.client.sendMessage((byte) 6, new object[2]
        {
          (object) bigCraftable,
          (object) (short) heldItemIndex
        });
      }
      else
      {
        if (!Game1.IsServer)
          return;
        MultiplayerUtility.broadcastSwitchHeldItem(bigCraftable, (short) heldItemIndex, whichPlayer, Game1.currentLocation.name);
      }
    }

    public static void sendMessageToEveryone(int messageCategory, string message, long whichPlayer)
    {
      if (Game1.IsClient)
      {
        Game1.client.sendMessage((byte) 19, new object[2]
        {
          (object) messageCategory,
          (object) message
        });
      }
      else
      {
        if (!Game1.IsServer)
          return;
        foreach (KeyValuePair<long, Farmer> otherFarmer in Game1.otherFarmers)
          otherFarmer.Value.multiplayerMessage.Add(new object[4]
          {
            (object) (byte) 19,
            (object) messageCategory,
            (object) message,
            (object) whichPlayer
          });
      }
    }

    public static void warpCharacter(short x, short y, string name, byte isStructure, long id)
    {
      if (Game1.otherFarmers.ContainsKey(id))
      {
        if (Game1.otherFarmers[id].currentLocation == null)
        {
          Game1.otherFarmers[id]._tmpLocationName = name;
          return;
        }
        Game1.otherFarmers[id].currentLocation.farmers.Remove(Game1.otherFarmers[id]);
        Game1.otherFarmers[id].currentLocation = Game1.getLocationFromName(name, (int) isStructure == 1);
        Game1.otherFarmers[id].position.X = (float) ((int) x * Game1.tileSize);
        Game1.otherFarmers[id].position.Y = (float) ((int) y * Game1.tileSize - Game1.tileSize / 2);
        GameLocation locationFromName = Game1.getLocationFromName(name, (int) isStructure == 1);
        locationFromName.farmers.Add(Game1.otherFarmers[id]);
        if (locationFromName.farmers.Count.Equals(Game1.numberOfPlayers() - 1))
          locationFromName.checkForEvents();
      }
      if (!Game1.IsServer)
        return;
      MultiplayerUtility.broadcastFarmerWarp(x, y, name, (int) isStructure == 1, id);
    }

    public static void performToolAction(byte toolIndex, byte toolUpgradeLevel, short xTile, short yTile, string locationName, byte facingDirection, short seed, long who)
    {
      Tool toolFromDescription = ToolFactory.getToolFromDescription(toolIndex, (int) toolUpgradeLevel);
      GameLocation locationFromName = Game1.getLocationFromName(locationName);
      Game1.otherFarmers[who].CurrentTool = toolFromDescription;
      Game1.recentMultiplayerRandom = new Random((int) seed);
      if (locationFromName == null)
      {
        if (toolFromDescription is MeleeWeapon)
        {
          Game1.otherFarmers[who].faceDirection((int) facingDirection);
          (toolFromDescription as MeleeWeapon).DoDamage(Game1.currentLocation, (int) xTile, (int) yTile, Game1.otherFarmers[who].facingDirection, 1, Game1.otherFarmers[who]);
        }
        else
          toolFromDescription.DoFunction(Game1.currentLocation, (int) xTile, (int) yTile, 1, Game1.otherFarmers[who]);
      }
      else if (toolFromDescription is MeleeWeapon)
      {
        Game1.otherFarmers[who].faceDirection((int) facingDirection);
        (toolFromDescription as MeleeWeapon).DoDamage(locationFromName, (int) xTile, (int) yTile, Game1.otherFarmers[who].facingDirection, 1, Game1.otherFarmers[who]);
      }
      else
        toolFromDescription.DoFunction(locationFromName, (int) xTile, (int) yTile, 1, Game1.otherFarmers[who]);
      if (!Game1.IsServer)
        return;
      MultiplayerUtility.broadcastToolAction(toolFromDescription, (int) xTile, (int) yTile, locationName, facingDirection, seed, who);
    }

    public static void broadcastBuildingChange(byte whatChange, Vector2 tileLocation, string name, string locationName, long who)
    {
      if (Game1.IsClient)
      {
        Game1.client.sendMessage((byte) 13, new object[4]
        {
          (object) whatChange,
          (object) (short) tileLocation.X,
          (object) (short) tileLocation.Y,
          (object) name
        });
      }
      else
      {
        if (!Game1.IsServer)
          return;
        foreach (KeyValuePair<long, Farmer> otherFarmer in Game1.otherFarmers)
        {
          if (otherFarmer.Value.currentLocation.name.Equals(locationName) && ((int) whatChange != 2 || otherFarmer.Value.uniqueMultiplayerID != who))
            otherFarmer.Value.multiplayerMessage.Add(new object[7]
            {
              (object) (byte) 13,
              (object) whatChange,
              (object) (short) tileLocation.X,
              (object) (short) tileLocation.Y,
              (object) name,
              (object) who,
              (object) MultiplayerUtility.recentMultiplayerEntityID
            });
        }
      }
    }

    public static void receiveBuildingChange(byte whatChange, short tileX, short tileY, string name, long who, long id)
    {
      MultiplayerUtility.recentMultiplayerEntityID = !Game1.IsClient ? MultiplayerUtility.getNewID() : id;
      if (!(Game1.currentLocation is Farm) && !Game1.IsServer)
        return;
      Farm currentLocation = (Farm) Game1.currentLocation;
      if (!(Game1.currentLocation is Farm))
        currentLocation = (Farm) Game1.otherFarmers[id].currentLocation;
      Farmer farmer = Game1.getFarmer(who);
      switch (whatChange)
      {
        case 0:
          BluePrint bluePrint = new BluePrint(name);
          if (bluePrint.blueprintType.Equals("Animals") && currentLocation.placeAnimal(bluePrint, new Vector2((float) tileX, (float) tileY), true, who) && farmer.IsMainPlayer)
            bluePrint.consumeResources();
          else if (!bluePrint.blueprintType.Equals("Animals") && currentLocation.buildStructure(bluePrint, new Vector2((float) tileX, (float) tileY), true, farmer, false) && farmer.IsMainPlayer)
            bluePrint.consumeResources();
          else if (farmer.IsMainPlayer)
          {
            Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:BlueprintsMenu.cs.10016"), Color.Red, 3500f));
            break;
          }
          if (bluePrint.blueprintType.Equals("Animals"))
            break;
          Game1.playSound("axe");
          break;
        case 1:
          Building buildingAt1 = currentLocation.getBuildingAt(new Vector2((float) tileX, (float) tileY));
          if (!currentLocation.destroyStructure(new Vector2((float) tileX, (float) tileY)))
            break;
          int num = buildingAt1.tileY + buildingAt1.tilesHigh;
          for (int index = 0; index < buildingAt1.texture.Bounds.Height / Game1.tileSize; ++index)
          {
            Farm farm = currentLocation;
            Texture2D texture = buildingAt1.texture;
            Microsoft.Xna.Framework.Rectangle bounds = buildingAt1.texture.Bounds;
            int x = bounds.Center.X;
            bounds = buildingAt1.texture.Bounds;
            int y = bounds.Center.Y;
            int width = Game1.tileSize / 16;
            int height = Game1.tileSize / 16;
            Microsoft.Xna.Framework.Rectangle sourcerectangle = new Microsoft.Xna.Framework.Rectangle(x, y, width, height);
            int xTile = buildingAt1.tileX + Game1.random.Next(buildingAt1.tilesWide);
            int yTile = buildingAt1.tileY + buildingAt1.tilesHigh - index;
            int numberOfChunks = Game1.random.Next(20, 45);
            int groundLevelTile = num;
            Game1.createRadialDebris((GameLocation) farm, texture, sourcerectangle, xTile, yTile, numberOfChunks, groundLevelTile);
          }
          Game1.playSound("explosion");
          Utility.spreadAnimalsAround(buildingAt1, currentLocation);
          break;
        case 2:
          BluePrint blueprint = new BluePrint(name);
          Building buildingAt2 = currentLocation.getBuildingAt(new Vector2((float) tileX, (float) tileY));
          currentLocation.tryToUpgrade(buildingAt2, blueprint);
          break;
      }
    }

    public static void broadcastDebrisPickup(int uniqueID, string locationName, long whichPlayer)
    {
      if (Game1.IsClient)
      {
        Game1.client.sendMessage((byte) 8, new object[2]
        {
          (object) uniqueID,
          (object) locationName
        });
      }
      else
      {
        if (!Game1.IsServer)
          return;
        foreach (KeyValuePair<long, Farmer> otherFarmer in Game1.otherFarmers)
        {
          if (otherFarmer.Value.currentLocation.name.Equals(locationName) && whichPlayer != otherFarmer.Value.uniqueMultiplayerID)
            otherFarmer.Value.multiplayerMessage.Add(new object[4]
            {
              (object) (byte) 8,
              (object) uniqueID,
              (object) locationName,
              (object) whichPlayer
            });
        }
      }
    }

    public static void receivePlayerIntroduction(long id, string name)
    {
      Farmer owner = new Farmer(new FarmerSprite(Game1.content.Load<Texture2D>("Characters\\farmer")), new Vector2((float) (Game1.tileSize * 5), (float) (Game1.tileSize * 5)), 2, name, new List<Item>(), true);
      owner.FarmerSprite.setOwner(owner);
      owner.currentLocation = Game1.getLocationFromName("FarmHouse");
      owner.uniqueMultiplayerID = id;
      Game1.otherFarmers.Add(id, owner);
    }

    public static void broadcastPlayerIntroduction(long id, string name)
    {
      foreach (KeyValuePair<long, Farmer> otherFarmer in Game1.otherFarmers)
      {
        if (id != otherFarmer.Value.uniqueMultiplayerID)
          otherFarmer.Value.multiplayerMessage.Add(new object[3]
          {
            (object) (byte) 2,
            (object) id,
            (object) name
          });
      }
    }

    public static void performCheckAction(short x, short y, string location, long who)
    {
      if (!Utility.canGrabSomethingFromHere((int) x * Game1.tileSize, (int) y * Game1.tileSize, Game1.otherFarmers[who]) || !Game1.getLocationFromName(location).objects.ContainsKey(new Vector2((float) x, (float) y)) || !Game1.getLocationFromName(location).objects[new Vector2((float) x, (float) y)].checkForAction(Game1.otherFarmers[who], false))
      {
        if (Game1.isFestival())
          Game1.currentLocation.checkAction(new Location((int) x, (int) y), Game1.viewport, Game1.otherFarmers[who]);
        else
          Game1.getLocationFromName(location).checkAction(new Location((int) x, (int) y), Game1.viewport, Game1.otherFarmers[who]);
      }
      if (!Game1.IsServer)
        return;
      MultiplayerUtility.broadcastCheckAction((int) x, (int) y, who, location);
    }

    public static void broadcastCheckAction(int x, int y, long who, string location)
    {
      if (Game1.IsClient)
      {
        Game1.client.sendMessage((byte) 9, new object[3]
        {
          (object) (short) x,
          (object) (short) y,
          (object) location
        });
      }
      else
      {
        if (!Game1.IsServer)
          return;
        Console.WriteLine("Server Received Check Action message @ X:" + (object) x + " Y:" + (object) y);
        foreach (KeyValuePair<long, Farmer> otherFarmer in Game1.otherFarmers)
        {
          if (otherFarmer.Value.currentLocation.name.Equals(location) && who != otherFarmer.Value.uniqueMultiplayerID)
            otherFarmer.Value.multiplayerMessage.Add(new object[5]
            {
              (object) (byte) 9,
              (object) (short) x,
              (object) (short) y,
              (object) location,
              (object) who
            });
        }
      }
    }

    public static void sendReadyConfirmation(long whichPlayer)
    {
      if (Game1.IsClient)
      {
        Game1.client.sendMessage((byte) 17, new object[1]
        {
          (object) (short) 0
        });
      }
      else
      {
        if (!Game1.IsServer)
          return;
        foreach (KeyValuePair<long, Farmer> otherFarmer in Game1.otherFarmers)
          otherFarmer.Value.multiplayerMessage.Add(new object[2]
          {
            (object) (byte) 17,
            (object) whichPlayer
          });
      }
    }

    public static void sendServerToClientsMessage(string message)
    {
      if (!Game1.IsServer)
        return;
      foreach (KeyValuePair<long, Farmer> otherFarmer in Game1.otherFarmers)
        otherFarmer.Value.multiplayerMessage.Add(new object[2]
        {
          (object) (byte) 18,
          (object) message
        });
    }

    public static void sendChatMessage(string message, long whichPlayer)
    {
      if (Game1.IsClient)
      {
        Game1.client.sendMessage((byte) 10, new object[1]
        {
          (object) message
        });
      }
      else
      {
        if (!Game1.IsServer)
          return;
        foreach (KeyValuePair<long, Farmer> otherFarmer in Game1.otherFarmers)
          otherFarmer.Value.multiplayerMessage.Add(new object[3]
          {
            (object) (byte) 10,
            (object) message,
            (object) whichPlayer
          });
      }
    }

    public static void sendNameChange(string name, long who)
    {
      if (who == Game1.player.uniqueMultiplayerID)
        Game1.player.name = name;
      if (Game1.IsClient)
      {
        Game1.client.sendMessage((byte) 11, new object[1]
        {
          (object) name
        });
      }
      else
      {
        if (!Game1.IsServer)
          return;
        foreach (KeyValuePair<long, Farmer> otherFarmer in Game1.otherFarmers)
        {
          if (who != otherFarmer.Value.uniqueMultiplayerID)
            otherFarmer.Value.multiplayerMessage.Add(new object[3]
            {
              (object) (byte) 11,
              (object) name,
              (object) who
            });
        }
      }
    }

    public static void receiveNameChange(string message, long who)
    {
      ChatBox chatBox = Game1.ChatBox;
      string message1;
      if (!Game1.otherFarmers[who].isMale)
        message1 = Game1.content.LoadString("Strings\\StringsFromCSFiles:MultiplayerUtility.cs.12479", (object) Game1.otherFarmers[who].displayName, (object) message);
      else
        message1 = Game1.content.LoadString("Strings\\StringsFromCSFiles:MultiplayerUtility.cs.12478", (object) Game1.otherFarmers[who].displayName, (object) message);
      long who1 = -1;
      chatBox.receiveChatMessage(message1, who1);
      Game1.otherFarmers[who].name = message;
      if (!Game1.IsServer)
        return;
      MultiplayerUtility.sendNameChange(message, who);
    }

    public static void receiveChatMessage(string message, long whichPlayer)
    {
      foreach (IClickableMenu onScreenMenu in Game1.onScreenMenus)
      {
        if (onScreenMenu is ChatBox)
        {
          ((ChatBox) onScreenMenu).receiveChatMessage(message, whichPlayer);
          break;
        }
      }
      if (!Game1.IsServer)
        return;
      MultiplayerUtility.sendChatMessage(message, whichPlayer);
    }

    public static void allFarmersReadyCheck()
    {
      if (Game1.IsServer)
      {
        foreach (Farmer farmer in Game1.otherFarmers.Values)
        {
          if (!farmer.readyConfirmation)
            return;
        }
        if (!Game1.player.readyConfirmation)
          return;
        MultiplayerUtility.sendReadyConfirmation(Game1.player.uniqueMultiplayerID);
        foreach (Farmer farmer in Game1.otherFarmers.Values)
          farmer.readyConfirmation = false;
        Game1.player.readyConfirmation = false;
        if (Game1.currentLocation.currentEvent == null)
          return;
        ++Game1.currentLocation.currentEvent.CurrentCommand;
      }
      else
      {
        if (Game1.isFestival())
          Game1.currentLocation.currentEvent.allPlayersReady = true;
        foreach (Farmer farmer in Game1.otherFarmers.Values)
          farmer.readyConfirmation = false;
      }
    }

    public static void parseServerToClientsMessage(string message)
    {
      if (!Game1.IsClient)
        return;
      if (!(message == "festivalEvent"))
      {
        if (!(message == "endFest") || Game1.CurrentEvent == null)
          return;
        Game1.CurrentEvent.forceEndFestival(Game1.player);
      }
      else
      {
        if (Game1.currentLocation.currentEvent == null)
          return;
        Game1.currentLocation.currentEvent.forceFestivalContinue();
      }
    }

    public static void interpretMessageToEveryone(int messageCategory, string message, long who)
    {
      switch (messageCategory)
      {
        case 0:
          if (Game1.isFestival())
            Game1.otherFarmers[who].dancePartner = Game1.currentLocation.currentEvent.getActorByName(message);
          Game1.currentLocation.currentEvent.getActorByName(message).hasPartnerForDance = true;
          break;
        case 1:
          if (Game1.isFestival())
          {
            Game1.currentLocation.currentEvent.addItemToLuauSoup((Item) new Object(Convert.ToInt32(message.Split(' ')[0]), 1, false, -1, Convert.ToInt32(message.Split(' ')[1])), Game1.otherFarmers[who]);
            break;
          }
          break;
        case 2:
          if (Game1.isFestival())
          {
            Game1.CurrentEvent.setGrangeDisplayUser(message.Equals("null") ? (Farmer) null : Game1.getFarmer(who));
            break;
          }
          break;
        case 3:
          if (Game1.isFestival())
          {
            string[] strArray = message.Split(' ');
            int int32 = Convert.ToInt32(strArray[0]);
            if (strArray[1].Equals("null"))
            {
              Game1.CurrentEvent.addItemToGrangeDisplay((Item) null, int32, true);
              break;
            }
            Game1.CurrentEvent.addItemToGrangeDisplay((Item) new Object(Convert.ToInt32(strArray[1]), Convert.ToInt32(strArray[2]), false, -1, 0), int32, true);
            break;
          }
          break;
        case 4:
          Game1.CurrentEvent.grangeScore = Convert.ToInt32(message);
          Game1.ChatBox.receiveChatMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:MultiplayerUtility.cs.12488"), -1L);
          Game1.CurrentEvent.interpretGrangeResults();
          break;
        case 5:
          if (!Game1.player.mailReceived.Contains(message))
          {
            Game1.player.mailReceived.Add(message);
            break;
          }
          break;
        case 6:
          (Game1.getLocationFromName("CommunityCenter") as CommunityCenter).completeBundle(Convert.ToInt32(message));
          break;
        case 7:
          Game1.addMailForTomorrow(message, false, false);
          break;
      }
      if (!Game1.IsServer)
        return;
      MultiplayerUtility.sendMessageToEveryone(messageCategory, message, who);
    }

    public static void broadcastDebrisCreate(short seed, Vector2 position, int facingDirection, Item i, long who)
    {
      ItemDescription descriptionFromItem = ObjectFactory.getDescriptionFromItem(i);
      if (Game1.IsClient)
      {
        Game1.client.sendMessage((byte) 14, new object[7]
        {
          (object) seed,
          (object) (int) position.X,
          (object) (int) position.Y,
          (object) (byte) facingDirection,
          (object) descriptionFromItem.type,
          (object) (short) descriptionFromItem.index,
          (object) (short) descriptionFromItem.stack
        });
      }
      else
      {
        if (!Game1.IsServer)
          return;
        foreach (KeyValuePair<long, Farmer> otherFarmer in Game1.otherFarmers)
        {
          if (who != otherFarmer.Value.uniqueMultiplayerID && (who == Game1.player.uniqueMultiplayerID || Game1.otherFarmers[who].currentLocation.Equals((object) otherFarmer.Value.currentLocation)))
            otherFarmer.Value.multiplayerMessage.Add(new object[8]
            {
              (object) (byte) 14,
              (object) seed,
              (object) (int) position.X,
              (object) (int) position.Y,
              (object) (byte) facingDirection,
              (object) descriptionFromItem.type,
              (object) (short) descriptionFromItem.index,
              (object) (short) descriptionFromItem.stack
            });
        }
      }
    }

    public static void performDebrisCreate(short seed, int xPosition, int yPosition, byte facingDirection, byte type, short index, short stack, long who)
    {
      Game1.recentMultiplayerRandom = new Random((int) seed);
      Vector2 vector2 = new Vector2((float) xPosition, (float) yPosition);
      Vector2 debrisOrigin = new Vector2((float) xPosition, (float) yPosition);
      Item itemFromDescription = ObjectFactory.getItemFromDescription(type, (int) index, (int) stack);
      switch (facingDirection)
      {
        case 0:
          debrisOrigin.X -= (float) (Game1.tileSize / 2);
          debrisOrigin.Y -= (float) (Game1.tileSize * 2 + Game1.recentMultiplayerRandom.Next(Game1.tileSize / 2));
          vector2.Y -= (float) (Game1.tileSize * 3);
          break;
        case 1:
          debrisOrigin.X += (float) (Game1.tileSize * 2 / 3);
          debrisOrigin.Y -= (float) (Game1.tileSize / 2 - Game1.recentMultiplayerRandom.Next(Game1.tileSize / 8));
          vector2.X += (float) (Game1.tileSize * 4);
          break;
        case 2:
          debrisOrigin.X -= (float) (Game1.tileSize / 2);
          debrisOrigin.Y += (float) Game1.recentMultiplayerRandom.Next(Game1.tileSize / 2);
          vector2.Y += (float) (Game1.tileSize * 3 / 2);
          break;
        case 3:
          debrisOrigin.X -= (float) Game1.tileSize;
          debrisOrigin.Y -= (float) (Game1.tileSize / 2 - Game1.recentMultiplayerRandom.Next(Game1.tileSize / 8));
          vector2.X -= (float) (Game1.tileSize * 4);
          break;
      }
      if (Game1.IsClient)
      {
        Game1.currentLocation.debris.Add(new Debris(itemFromDescription, debrisOrigin, vector2));
      }
      else
      {
        if (!Game1.IsServer)
          return;
        Game1.otherFarmers[who].currentLocation.debris.Add(new Debris(itemFromDescription, debrisOrigin, vector2));
        MultiplayerUtility.broadcastDebrisCreate(seed, vector2, (int) facingDirection, itemFromDescription, who);
      }
    }

    public static void performDebrisPickup(int uniqueID, string locationName, long whichPlayer)
    {
      GameLocation locationFromName = Game1.getLocationFromName(locationName);
      for (int index = 0; index < locationFromName.debris.Count; ++index)
      {
        if (locationFromName.debris[index].uniqueID == uniqueID)
        {
          locationFromName.debris.RemoveAt(index);
          break;
        }
      }
      if (!Game1.IsServer)
        return;
      MultiplayerUtility.broadcastDebrisPickup(uniqueID, locationName, whichPlayer);
    }

    public static void broadcastNPCMove(int x, int y, long id, GameLocation location)
    {
      foreach (KeyValuePair<long, Farmer> otherFarmer in Game1.otherFarmers)
      {
        if (otherFarmer.Value.currentLocation.Equals((object) location))
          otherFarmer.Value.multiplayerMessage.Add(new object[4]
          {
            (object) (byte) 15,
            (object) x,
            (object) y,
            (object) id
          });
      }
    }

    public static void broadcastNPCBehavior(long npcID, GameLocation location, byte behavior)
    {
      foreach (KeyValuePair<long, Farmer> otherFarmer in Game1.otherFarmers)
      {
        if (otherFarmer.Value.currentLocation.Equals((object) location))
          otherFarmer.Value.multiplayerMessage.Add(new object[3]
          {
            (object) (byte) 16,
            (object) npcID,
            (object) behavior
          });
      }
    }

    public static void performNPCBehavior(long npcID, byte behavior)
    {
      Character characterFromId = MultiplayerUtility.getCharacterFromID(npcID);
      if (characterFromId == null || characterFromId.ignoreMultiplayerUpdates)
        return;
      characterFromId.performBehavior(behavior);
    }

    public static void performNPCMove(int x, int y, long id)
    {
      Character characterFromId = MultiplayerUtility.getCharacterFromID(id);
      if (characterFromId == null || characterFromId.ignoreMultiplayerUpdates)
        return;
      characterFromId.updatePositionFromServer(new Vector2((float) x, (float) y));
    }

    public static Character getCharacterFromID(long id)
    {
      if (Game1.currentLocation is Farm)
      {
        if ((Game1.currentLocation as Farm).animals.ContainsKey(id))
          return (Character) (Game1.currentLocation as Farm).animals[id];
        foreach (Building building in (Game1.currentLocation as Farm).buildings)
        {
          if (building.indoors is AnimalHouse && (building.indoors as AnimalHouse).animals.ContainsKey(id))
            return (Character) (building.indoors as AnimalHouse).animals[id];
        }
      }
      return (Character) null;
    }
  }
}
