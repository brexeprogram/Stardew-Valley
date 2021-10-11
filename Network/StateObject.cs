// Decompiled with JetBrains decompiler
// Type: StardewValley.Network.StateObject
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using System.Net.Sockets;
using System.Text;

namespace StardewValley.Network
{
  public class StateObject
  {
    public byte[] buffer = new byte[1024];
    public StringBuilder sb = new StringBuilder();
    public Socket workSocket;
    public const int BufferSize = 1024;
  }
}
