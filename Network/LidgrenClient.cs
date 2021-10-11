// Decompiled with JetBrains decompiler
// Type: StardewValley.Network.LidgrenClient
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Lidgren.Network;
using Lidgren.Network.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Net;

namespace StardewValley.Network
{
  public class LidgrenClient : Client
  {
    private NetClient client;

    public override bool isConnected
    {
      get
      {
        if (this.client != null)
          return this.client.ConnectionStatus == NetConnectionStatus.Connected;
        return false;
      }
    }

    public override float averageRoundtripTime
    {
      get
      {
        return this.client.ServerConnection.AverageRoundtripTime;
      }
    }

    public override IPAddress serverAddress
    {
      get
      {
        return this.client.ServerConnection.RemoteEndPoint.Address;
      }
    }

    public override void initializeConnection(string address)
    {
      NetPeerConfiguration config = new NetPeerConfiguration("StardewValley");
      config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
      config.ConnectionTimeout = 120f;
      config.PingInterval = 5f;
      this.client = new NetClient(config);
      this.client.Start();
      int serverPort = 24642;
      if (address.Contains(":"))
      {
        string[] strArray = address.Split(':');
        int index1 = 0;
        address = strArray[index1];
        int index2 = 1;
        serverPort = Convert.ToInt32(strArray[index2]);
      }
      this.client.DiscoverKnownPeer(address, serverPort);
    }

    public override void receiveMessages()
    {
      NetIncomingMessage msg;
      while ((msg = this.client.ReadMessage()) != null)
      {
        switch (msg.MessageType)
        {
          case NetIncomingMessageType.WarningMessage:
            Game1.debugOutput = msg.ReadString();
            continue;
          case NetIncomingMessageType.ErrorMessage:
            Game1.debugOutput = msg.ReadString();
            continue;
          case NetIncomingMessageType.Data:
            LidgrenClient.parseDataMessageFromServer(msg);
            continue;
          case NetIncomingMessageType.DiscoveryResponse:
            Console.WriteLine("Found server at " + (object) msg.SenderEndPoint);
            this.serverName = msg.ReadString();
            this.receiveHandshake(msg);
            continue;
          default:
            continue;
        }
      }
      if (this.client.ServerConnection == null || DateTime.Now.Second % 2 != 0)
        return;
      Game1.debugOutput = "Ping: " + (object) (float) ((double) this.client.ServerConnection.AverageRoundtripTime * 1000.0) + "ms";
    }

    private void receiveHandshake(NetIncomingMessage msg)
    {
      while ((long) msg.LengthBits - msg.Position >= 8L)
      {
        if ((int) msg.ReadByte() == 2)
        {
          long key = msg.ReadInt64();
          Farmer owner = new Farmer(new FarmerSprite(Game1.content.Load<Texture2D>("Characters\\Farmer\\farmer_base")), new Vector2((float) (Game1.tileSize * 10), (float) (Game1.tileSize * 15)), 2, msg.ReadString(), new List<Item>(), true);
          owner.FarmerSprite.setOwner(owner);
          Game1.serverHost = owner;
          Game1.otherFarmers.Add(key, owner);
          Game1.otherFarmers[key]._tmpLocationName = msg.ReadString();
          Game1.otherFarmers[key].uniqueMultiplayerID = key;
        }
      }
      this.client.Connect(msg.SenderEndPoint.Address.ToString(), msg.SenderEndPoint.Port);
      this.hasHandshaked = true;
      if (Game1.otherFarmers.ContainsKey(this.client.UniqueIdentifier))
        return;
      Game1.otherFarmers.Add(this.client.UniqueIdentifier, Game1.player);
      Game1.player.uniqueMultiplayerID = this.client.UniqueIdentifier;
      Game1.player._tmpLocationName = "BusStop";
    }

    public override void sendMessage(byte which, object[] data)
    {
      NetOutgoingMessage message = this.client.CreateMessage();
      MultiplayerUtility.writeData(message, which, data);
      int num = (int) this.client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
    }

    private static void parseDataMessageFromServer(NetIncomingMessage msg)
    {
      while ((long) msg.LengthBits - msg.Position >= 8L)
      {
        switch (msg.ReadByte())
        {
          case 0:
            Game1.otherFarmers[msg.ReadInt64()].setMoving(msg.ReadByte());
            continue;
          case 1:
            Game1.otherFarmers[msg.ReadInt64()].updatePositionFromServer(msg.ReadVector2());
            continue;
          case 2:
            MultiplayerUtility.receivePlayerIntroduction(msg.ReadInt64(), msg.ReadString());
            continue;
          case 3:
            long index = msg.ReadInt64();
            Game1.otherFarmers[index].FarmerSprite.CurrentToolIndex = msg.ReadInt32();
            if ((int) msg.ReadByte() == 1)
            {
              ((FarmerSprite) Game1.otherFarmers[index].sprite).animateBackwardsOnce(msg.ReadInt32(), msg.ReadFloat());
              int num = (int) msg.ReadByte();
              continue;
            }
            ((FarmerSprite) Game1.otherFarmers[index].sprite).animateOnce(msg.ReadInt32(), msg.ReadFloat(), (int) msg.ReadByte());
            continue;
          case 4:
            MultiplayerUtility.performObjectAlteration(msg.ReadInt16(), msg.ReadInt16(), msg.ReadByte(), msg.ReadByte(), msg.ReadInt32());
            continue;
          case 5:
            MultiplayerUtility.warpCharacter(msg.ReadInt16(), msg.ReadInt16(), msg.ReadString(), msg.ReadByte(), msg.ReadInt64());
            continue;
          case 6:
            MultiplayerUtility.performSwitchHeldItem(msg.ReadInt64(), msg.ReadByte(), (int) msg.ReadInt16());
            continue;
          case 7:
            MultiplayerUtility.performToolAction(msg.ReadByte(), msg.ReadByte(), msg.ReadInt16(), msg.ReadInt16(), msg.ReadString(), msg.ReadByte(), msg.ReadInt16(), msg.ReadInt64());
            continue;
          case 8:
            MultiplayerUtility.performDebrisPickup(msg.ReadInt32(), msg.ReadString(), msg.ReadInt64());
            continue;
          case 9:
            MultiplayerUtility.performCheckAction(msg.ReadInt16(), msg.ReadInt16(), msg.ReadString(), msg.ReadInt64());
            continue;
          case 10:
            MultiplayerUtility.receiveChatMessage(msg.ReadString(), msg.ReadInt64());
            continue;
          case 11:
            MultiplayerUtility.receiveNameChange(msg.ReadString(), msg.ReadInt64());
            continue;
          case 12:
            MultiplayerUtility.receiveTenMinuteSync(msg.ReadInt16());
            continue;
          case 13:
            MultiplayerUtility.receiveBuildingChange(msg.ReadByte(), msg.ReadInt16(), msg.ReadInt16(), msg.ReadString(), msg.ReadInt64(), msg.ReadInt64());
            continue;
          case 14:
            MultiplayerUtility.performDebrisCreate(msg.ReadInt16(), msg.ReadInt32(), msg.ReadInt32(), msg.ReadByte(), msg.ReadByte(), msg.ReadInt16(), msg.ReadInt16(), 0L);
            continue;
          case 15:
            MultiplayerUtility.performNPCMove(msg.ReadInt32(), msg.ReadInt32(), msg.ReadInt64());
            continue;
          case 16:
            MultiplayerUtility.performNPCBehavior(msg.ReadInt64(), msg.ReadByte());
            continue;
          case 17:
            MultiplayerUtility.allFarmersReadyCheck();
            continue;
          case 18:
            MultiplayerUtility.parseServerToClientsMessage(msg.ReadString());
            continue;
          case 19:
            MultiplayerUtility.interpretMessageToEveryone(msg.ReadInt32(), msg.ReadString(), msg.ReadInt64());
            continue;
          default:
            continue;
        }
      }
    }
  }
}
