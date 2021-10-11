// Decompiled with JetBrains decompiler
// Type: StardewValley.Network.AsynchronousSocketListener
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Xml;

namespace StardewValley.Network
{
  public class AsynchronousSocketListener
  {
    public static ManualResetEvent allDone = new ManualResetEvent(false);
    public static bool active = true;

    public static void StartListening()
    {
      byte[] numArray = new byte[1024];
      IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 24643);
      Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
      try
      {
        socket.Bind((EndPoint) ipEndPoint);
        socket.Listen(16);
        while (AsynchronousSocketListener.active)
        {
          AsynchronousSocketListener.allDone.Reset();
          Console.WriteLine("Waiting for a connection...");
          socket.BeginAccept(new AsyncCallback(AsynchronousSocketListener.AcceptCallback), (object) socket);
          AsynchronousSocketListener.allDone.WaitOne();
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
      Console.WriteLine("\nPress ENTER to continue...");
      Console.Read();
    }

    public static void AcceptCallback(IAsyncResult ar)
    {
      AsynchronousSocketListener.allDone.Set();
      Socket socket = ((Socket) ar.AsyncState).EndAccept(ar);
      StateObject stateObject = new StateObject();
      stateObject.workSocket = socket;
      socket.BeginReceive(stateObject.buffer, 0, 1024, SocketFlags.None, new AsyncCallback(AsynchronousSocketListener.ReadCallback), (object) stateObject);
    }

    public static void ReadCallback(IAsyncResult ar)
    {
      string empty = string.Empty;
      StateObject asyncState = (StateObject) ar.AsyncState;
      Socket workSocket = asyncState.workSocket;
      int count = workSocket.EndReceive(ar);
      if (count <= 0)
        return;
      asyncState.sb.Append(Encoding.ASCII.GetString(asyncState.buffer, 0, count));
      string data = asyncState.sb.ToString();
      if (data.IndexOf("<EOF>") > -1)
      {
        Console.WriteLine("Read {0} bytes from socket. \n Data : {1}", (object) data.Length, (object) data);
        AsynchronousSocketListener.Send(workSocket, data);
      }
      else
        workSocket.BeginReceive(asyncState.buffer, 0, 1024, SocketFlags.None, new AsyncCallback(AsynchronousSocketListener.ReadCallback), (object) asyncState);
    }

    private static void Send(Socket handler, string data)
    {
      string str = data.Remove(data.IndexOf("<EOF>"));
      data = AsynchronousSocketListener.ToXml((object) Game1.getLocationFromName(str.Split('_')[0], (Convert.ToBoolean(str.Split('_')[1]) ? 1 : 0) != 0), typeof (GameLocation));
      byte[] bytes1 = Encoding.ASCII.GetBytes(data);
      byte[] bytes2 = BitConverter.GetBytes(bytes1.Length);
      handler.Send(bytes2);
      handler.BeginSend(bytes1, 0, bytes1.Length, SocketFlags.None, new AsyncCallback(AsynchronousSocketListener.SendCallback), (object) handler);
    }

    public static string ToXml(object Obj, Type ObjType)
    {
      MemoryStream memoryStream = new MemoryStream();
      XmlWriter xmlWriter = XmlWriter.Create((Stream) memoryStream, new XmlWriterSettings()
      {
        CloseOutput = true
      });
      xmlWriter.WriteStartDocument();
      SaveGame.locationSerializer.Serialize(xmlWriter, Obj);
      xmlWriter.WriteEndDocument();
      xmlWriter.Flush();
      xmlWriter.Close();
      memoryStream.Close();
      string str1 = Encoding.UTF8.GetString(memoryStream.GetBuffer());
      string str2 = str1.Substring(str1.IndexOf(Convert.ToChar(60)));
      return str2.Substring(0, str2.LastIndexOf(Convert.ToChar(62)) + 1);
    }

    private static void SendCallback(IAsyncResult ar)
    {
      try
      {
        Socket asyncState = (Socket) ar.AsyncState;
        IAsyncResult asyncResult = ar;
        Console.WriteLine("Sent {0} bytes to client.", (object) asyncState.EndSend(asyncResult));
        int num = 2;
        asyncState.Shutdown((SocketShutdown) num);
        asyncState.Close();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
    }
  }
}
