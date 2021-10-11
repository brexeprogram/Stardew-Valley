// Decompiled with JetBrains decompiler
// Type: StardewValley.Network.Client
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using System.Net;

namespace StardewValley.Network
{
  public abstract class Client
  {
    public string serverName = "???";
    public bool hasHandshaked;

    public abstract bool isConnected { get; }

    public abstract float averageRoundtripTime { get; }

    public abstract IPAddress serverAddress { get; }

    public abstract void initializeConnection(string address);

    public abstract void receiveMessages();

    public abstract void sendMessage(byte which, object[] data);
  }
}
