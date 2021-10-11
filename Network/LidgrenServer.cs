// Decompiled with JetBrains decompiler
// Type: StardewValley.Network.LidgrenServer
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace StardewValley.Network
{
  public class LidgrenServer : Server
  {
    private static int messageSendCounter = 50;
    private NetServer server;
    private Thread mapServerThread;

    public override int connectionsCount
    {
      get
      {
        if (this.server == null)
          return 0;
        return this.server.ConnectionsCount;
      }
    }

    public LidgrenServer(string name)
      : base(name)
    {
    }

    public override void initializeConnection()
    {
      NetPeerConfiguration config = new NetPeerConfiguration("StardewValley");
      config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
      config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
      config.Port = 24642;
      config.ConnectionTimeout = 120f;
      config.PingInterval = 5f;
      config.MaximumConnections = 4;
      config.EnableUPnP = true;
      this.server = new NetServer(config);
      this.server.Start();
      this.server.UPnP.ForwardPort(24642, "Stardew Valley Server");
      this.mapServerThread = new Thread(new ThreadStart(AsynchronousSocketListener.StartListening));
      this.mapServerThread.Start();
      Game1.player.uniqueMultiplayerID = this.server.UniqueIdentifier;
      Game1.serverHost = Game1.player;
    }

    public override void stopServer()
    {
      this.server.Shutdown("Server shutting down...");
      AsynchronousSocketListener.allDone.Close();
      AsynchronousSocketListener.active = false;
    }

    public override void receiveMessages()
    {
      NetIncomingMessage netIncomingMessage;
      while ((netIncomingMessage = this.server.ReadMessage()) != null)
      {
        switch (netIncomingMessage.MessageType)
        {
          case NetIncomingMessageType.DiscoveryRequest:
            this.handshakeWithPlayer(netIncomingMessage);
            break;
          case NetIncomingMessageType.ErrorMessage:
            Game1.debugOutput = netIncomingMessage.ToString();
            break;
          case NetIncomingMessageType.ConnectionApproval:
            netIncomingMessage.SenderConnection.Approve();
            this.addNewFarmer(netIncomingMessage.SenderConnection.RemoteUniqueIdentifier);
            break;
          case NetIncomingMessageType.Data:
            LidgrenServer.parseDataMessageFromClient(netIncomingMessage);
            break;
          default:
            Game1.debugOutput = netIncomingMessage.ToString();
            break;
        }
        this.server.Recycle(netIncomingMessage);
      }
    }

    private void addNewFarmer(long id)
    {
      Farmer owner = new Farmer(new FarmerSprite(Game1.content.Load<Texture2D>("Characters\\Farmer\\farmer_base")), new Vector2((float) (Game1.tileSize * 10), (float) (Game1.tileSize * 15)), 2, "Max", new List<Item>(), true);
      owner.FarmerSprite.setOwner(owner);
      owner.currentLocation = Game1.getLocationFromName("BusStop");
      owner.uniqueMultiplayerID = id;
      Game1.getLocationFromName(owner.currentLocation.name).farmers.Add(owner);
      Game1.otherFarmers.Add(id, owner);
      MultiplayerUtility.broadcastPlayerIntroduction(id, "Max");
    }

    private void handshakeWithPlayer(NetIncomingMessage message)
    {
      NetOutgoingMessage message1 = this.server.CreateMessage();
      message1.Write(this.serverName);
      message1.Write((byte) 2);
      message1.Write(this.server.UniqueIdentifier);
      message1.Write(Game1.player.name);
      message1.Write(Game1.player.currentLocation.name);
      foreach (KeyValuePair<long, Farmer> otherFarmer in Game1.otherFarmers)
      {
        message1.Write((byte) 2);
        message1.Write(otherFarmer.Key);
        message1.Write(otherFarmer.Value.name);
        message1.Write(otherFarmer.Value.currentLocation.name);
      }
      this.server.SendDiscoveryResponse(message1, message.SenderEndPoint);
    }

    private static void parseDataMessageFromClient(NetIncomingMessage msg)
    {
      switch (msg.ReadByte())
      {
        case 0:
          Game1.otherFarmers[msg.SenderConnection.RemoteUniqueIdentifier].setMoving(msg.ReadByte());
          break;
        case 3:
          ((FarmerSprite) Game1.otherFarmers[msg.SenderConnection.RemoteUniqueIdentifier].sprite).CurrentToolIndex = msg.ReadInt32();
          if ((int) msg.ReadByte() == 1)
          {
            ((FarmerSprite) Game1.otherFarmers[msg.SenderConnection.RemoteUniqueIdentifier].sprite).animateBackwardsOnce(msg.ReadInt32(), msg.ReadFloat());
            int num = (int) msg.ReadByte();
            break;
          }
          ((FarmerSprite) Game1.otherFarmers[msg.SenderConnection.RemoteUniqueIdentifier].sprite).animateOnce(msg.ReadInt32(), msg.ReadFloat(), (int) msg.ReadByte());
          break;
        case 4:
          MultiplayerUtility.serverTryToPerformObjectAlteration(msg.ReadInt16(), msg.ReadInt16(), msg.ReadByte(), msg.ReadByte(), msg.ReadInt32(), Game1.otherFarmers[msg.SenderConnection.RemoteUniqueIdentifier]);
          break;
        case 5:
          MultiplayerUtility.warpCharacter(msg.ReadInt16(), msg.ReadInt16(), msg.ReadString(), msg.ReadByte(), msg.SenderConnection.RemoteUniqueIdentifier);
          break;
        case 6:
          MultiplayerUtility.performSwitchHeldItem(msg.SenderConnection.RemoteUniqueIdentifier, msg.ReadByte(), (int) msg.ReadInt16());
          break;
        case 7:
          MultiplayerUtility.performToolAction(msg.ReadByte(), msg.ReadByte(), msg.ReadInt16(), msg.ReadInt16(), msg.ReadString(), msg.ReadByte(), msg.ReadInt16(), msg.SenderConnection.RemoteUniqueIdentifier);
          break;
        case 8:
          MultiplayerUtility.performDebrisPickup(msg.ReadInt32(), msg.ReadString(), msg.SenderConnection.RemoteUniqueIdentifier);
          break;
        case 9:
          MultiplayerUtility.performCheckAction(msg.ReadInt16(), msg.ReadInt16(), msg.ReadString(), msg.SenderConnection.RemoteUniqueIdentifier);
          break;
        case 10:
          MultiplayerUtility.receiveChatMessage(msg.ReadString(), msg.SenderConnection.RemoteUniqueIdentifier);
          break;
        case 11:
          MultiplayerUtility.receiveNameChange(msg.ReadString(), msg.SenderConnection.RemoteUniqueIdentifier);
          break;
        case 13:
          MultiplayerUtility.receiveBuildingChange(msg.ReadByte(), msg.ReadInt16(), msg.ReadInt16(), msg.ReadString(), msg.SenderConnection.RemoteUniqueIdentifier, 0L);
          break;
        case 14:
          MultiplayerUtility.performDebrisCreate(msg.ReadInt16(), msg.ReadInt32(), msg.ReadInt32(), msg.ReadByte(), msg.ReadByte(), msg.ReadInt16(), msg.ReadInt16(), msg.SenderConnection.RemoteUniqueIdentifier);
          break;
        case 17:
          Game1.otherFarmers[msg.SenderConnection.RemoteUniqueIdentifier].readyConfirmation = true;
          MultiplayerUtility.allFarmersReadyCheck();
          break;
        case 19:
          MultiplayerUtility.interpretMessageToEveryone(msg.ReadInt32(), msg.ReadString(), msg.SenderConnection.RemoteUniqueIdentifier);
          break;
      }
    }

    public override void sendMessages(GameTime time)
    {
      LidgrenServer.messageSendCounter -= time.ElapsedGameTime.Milliseconds;
      if (LidgrenServer.messageSendCounter >= 0)
        return;
      LidgrenServer.messageSendCounter = 50;
      foreach (NetConnection connection in this.server.Connections)
      {
        if (Game1.otherFarmers.ContainsKey(connection.RemoteUniqueIdentifier) && Game1.otherFarmers[connection.RemoteUniqueIdentifier].multiplayerMessage.Count > 0)
        {
          NetOutgoingMessage message = this.server.CreateMessage();
          for (int index = 0; index < Game1.otherFarmers[connection.RemoteUniqueIdentifier].multiplayerMessage.Count; ++index)
            MultiplayerUtility.writeData(message, (byte) Game1.otherFarmers[connection.RemoteUniqueIdentifier].multiplayerMessage[index][0], ((IEnumerable<object>) Game1.otherFarmers[connection.RemoteUniqueIdentifier].multiplayerMessage[index]).Skip<object>(1).ToArray<object>());
          int num = (int) this.server.SendMessage(message, connection, NetDeliveryMethod.ReliableOrdered);
        }
      }
      foreach (KeyValuePair<long, Farmer> otherFarmer in Game1.otherFarmers)
        otherFarmer.Value.multiplayerMessage.Clear();
    }
  }
}
