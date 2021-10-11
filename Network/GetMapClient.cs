// Decompiled with JetBrains decompiler
// Type: StardewValley.Network.GetMapClient
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using StardewValley.Buildings;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml;

namespace StardewValley.Network
{
  public class GetMapClient
  {
    public static void receiveMapFromServer(GameLocation map, bool isStructure)
    {
      long num = DateTime.Now.Ticks / 10000L;
      byte[] buffer = new byte[4];
      IPEndPoint ipEndPoint = new IPEndPoint(Game1.client.serverAddress, 24643);
      Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
      socket.Connect((EndPoint) ipEndPoint);
      Console.WriteLine("Socket connected to {0}", (object) socket.RemoteEndPoint.ToString());
      byte[] bytes = Encoding.ASCII.GetBytes((isStructure ? map.uniqueName : map.name) + "_" + isStructure.ToString() + "_<EOF>");
      socket.Send(bytes);
      socket.Receive(buffer);
      byte[] numArray = new byte[BitConverter.ToInt32(buffer, 0)];
      socket.Receive(numArray);
      GameLocation gameLocation = (GameLocation) GetMapClient.FromXml(Encoding.ASCII.GetString(numArray), typeof (GameLocation));
      map.terrainFeatures = gameLocation.terrainFeatures;
      foreach (TerrainFeature terrainFeature in map.terrainFeatures.Values)
        terrainFeature.loadSprite();
      map.objects = gameLocation.objects;
      foreach (StardewValley.Object @object in map.objects.Values)
        @object.reloadSprite();
      map.characters = gameLocation.characters;
      if (gameLocation is Farm && map is Farm)
      {
        (map as Farm).buildings = (gameLocation as Farm).buildings;
        foreach (Building building in (map as Farm).buildings)
          building.load();
        (map as Farm).animals = (gameLocation as Farm).animals;
        foreach (KeyValuePair<long, FarmAnimal> animal in (Dictionary<long, FarmAnimal>) (map as Farm).animals)
          animal.Value.reload();
      }
      foreach (NPC character in map.characters)
        character.reloadSprite();
      Game1.player.remotePosition = Game1.player.position;
      Console.WriteLine("Time: " + (object) (DateTime.Now.Ticks / 10000L - num));
      socket.Close();
    }

    public static object FromXml(string Xml, Type ObjType)
    {
      StringReader stringReader = new StringReader(Xml);
      XmlTextReader xmlTextReader = new XmlTextReader((TextReader) stringReader);
      object obj = SaveGame.locationSerializer.Deserialize((XmlReader) xmlTextReader);
      xmlTextReader.Close();
      stringReader.Close();
      return obj;
    }
  }
}
