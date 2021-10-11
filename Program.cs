// Decompiled with JetBrains decompiler
// Type: StardewValley.Program
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using StardewValley.SDKs;
using System;
using System.IO;

namespace StardewValley
{
  public static class Program
  {
    public static bool GameTesterMode = false;
    public static bool releaseBuild = true;
    public const int build_steam = 0;
    public const int build_gog = 1;
    public const int build_rail = 2;
    public const int buildType = 0;
    private static SDKHelper _sdk;
    public static Game1 gamePtr;
    public static bool handlingException;
    public static bool hasTriedToPrintLog;
    public static bool successfullyPrintedLog;

    public static SDKHelper sdk
    {
      get
      {
        if (Program._sdk == null)
          Program._sdk = (SDKHelper) new SteamHelper();
        return Program._sdk;
      }
    }

    public static void Main(string[] args)
    {
      Program.GameTesterMode = true;
      AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(Program.handleException);
      using (Game1 game1 = new Game1())
      {
        Program.gamePtr = game1;
        game1.Run();
      }
    }

    public static void handleException(object sender, UnhandledExceptionEventArgs args)
    {
      if (Program.handlingException || !Program.GameTesterMode)
        return;
      Game1.gameMode = (byte) 11;
      Program.handlingException = true;
      Exception exceptionObject = (Exception) args.ExceptionObject;
      Game1.errorMessage = "Message: " + exceptionObject.Message + Environment.NewLine + "InnerException: " + (object) exceptionObject.InnerException + Environment.NewLine + "Stack Trace: " + exceptionObject.StackTrace;
      long num1 = DateTime.Now.Ticks / 10000L + 25000L;
      if (!Program.hasTriedToPrintLog)
      {
        Program.hasTriedToPrintLog = true;
        string path2 = (Game1.player != null ? Game1.player.name : "NullPlayer") + "_" + (object) Game1.uniqueIDForThisGame + "_" + (object) (Game1.player != null ? (int) Game1.player.millisecondsPlayed : Game1.random.Next(999999)) + ".txt";
        int num2 = Environment.OSVersion.Platform != PlatformID.Unix ? 26 : 28;
        string path = Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath((Environment.SpecialFolder) num2), "StardewValley"), "ErrorLogs"), path2);
        FileInfo fileInfo = new FileInfo(Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath((Environment.SpecialFolder) num2), "StardewValley"), "ErrorLogs"), "asdfasdf"));
        if (!fileInfo.Directory.Exists)
          fileInfo.Directory.Create();
        if (File.Exists(path))
          File.Delete(path);
        try
        {
          File.WriteAllText(path, Game1.errorMessage);
          Program.successfullyPrintedLog = true;
          Game1.errorMessage = "(Error Report created at " + path + ")" + Environment.NewLine + Game1.errorMessage;
        }
        catch (Exception ex)
        {
        }
      }
      Game1.gameMode = (byte) 3;
    }
  }
}
