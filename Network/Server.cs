// Decompiled with JetBrains decompiler
// Type: StardewValley.Network.Server
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;

namespace StardewValley.Network
{
  public abstract class Server
  {
    public const int messageSendDelay = 50;
    public const int defaultPort = 24642;
    public const int defaultMapServerPort = 24643;
    protected string serverName;

    public Server(string name)
    {
      this.serverName = name;
    }

    public abstract int connectionsCount { get; }

    public abstract void initializeConnection();

    public abstract void stopServer();

    public abstract void receiveMessages();

    public abstract void sendMessages(GameTime time);
  }
}
