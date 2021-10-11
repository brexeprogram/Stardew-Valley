// Decompiled with JetBrains decompiler
// Type: StardewValley.SDKs.SteamHelper
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Steamworks;
using System;

namespace StardewValley.SDKs
{
  public class SteamHelper : SDKHelper
  {
    public Callback<GameOverlayActivated_t> m_GameOverlayActivated;
    public bool active;

    public void EarlyInitialize()
    {
    }

    public void Initialize()
    {
      try
      {
        this.active = SteamAPI.Init();
      }
      catch (Exception ex)
      {
      }
      if (!this.active)
        return;
      this.m_GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(new Callback<GameOverlayActivated_t>.DispatchDelegate(this.OnGameOverlayActivated));
    }

    private void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
    {
      if (!this.active)
        return;
      if ((int) pCallback.m_bActive != 0)
        Game1.paused = !Game1.IsMultiplayer;
      else
        Game1.paused = false;
    }

    public void GetAchievement(string achieve)
    {
      if (!this.active || !SteamAPI.IsSteamRunning())
        return;
      if (achieve.Equals("0"))
        achieve = "a0";
      try
      {
        SteamUserStats.SetAchievement(achieve);
        SteamUserStats.StoreStats();
      }
      catch (Exception ex)
      {
      }
    }

    public void ResetAchievements()
    {
      if (!this.active)
        return;
      if (!SteamAPI.IsSteamRunning())
        return;
      try
      {
        SteamUserStats.ResetAllStats(true);
      }
      catch (Exception ex)
      {
      }
    }

    public void Update()
    {
      if (!this.active)
        return;
      SteamAPI.RunCallbacks();
    }

    public void Shutdown()
    {
      SteamAPI.Shutdown();
    }

    public void DebugInfo()
    {
      if (SteamAPI.IsSteamRunning())
      {
        Game1.debugOutput = "steam is running";
        if (!SteamUser.BLoggedOn())
          return;
        Game1.debugOutput += ", user logged on";
      }
      else
      {
        Game1.debugOutput = "steam is not running";
        SteamAPI.Init();
      }
    }

    public string FilterDirtyWords(string words)
    {
      return words;
    }
  }
}
