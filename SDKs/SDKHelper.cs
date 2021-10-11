// Decompiled with JetBrains decompiler
// Type: StardewValley.SDKs.SDKHelper
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

namespace StardewValley.SDKs
{
  public interface SDKHelper
  {
    void EarlyInitialize();

    void Initialize();

    void GetAchievement(string achieve);

    void ResetAchievements();

    void Update();

    void Shutdown();

    void DebugInfo();

    string FilterDirtyWords(string words);
  }
}
