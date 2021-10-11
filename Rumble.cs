// Decompiled with JetBrains decompiler
// Type: StardewValley.Rumble
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework.Input;

namespace StardewValley
{
  public static class Rumble
  {
    private static float rumbleTimerMax;
    private static float rumbleTimerCurrent;
    private static float rumbleDuringFade;
    private static float maxRumbleDuringFade;
    private static bool isRumbling;
    private static bool fade;

    public static void update(float milliseconds)
    {
      if (!Rumble.isRumbling || !Game1.options.gamepadControls)
        return;
      Rumble.rumbleTimerCurrent += milliseconds;
      if (Rumble.fade)
      {
        if ((double) Rumble.rumbleTimerCurrent > (double) Rumble.rumbleTimerMax - 1000.0)
          Rumble.rumbleDuringFade = Rumble.maxRumbleDuringFade - Rumble.maxRumbleDuringFade * (float) (((double) Rumble.rumbleTimerMax - (double) Rumble.rumbleTimerCurrent) / 1000.0);
        GamePad.SetVibration(Game1.playerOneIndex, Rumble.rumbleDuringFade, Rumble.rumbleDuringFade);
      }
      if ((double) Rumble.rumbleTimerCurrent <= (double) Rumble.rumbleTimerMax)
        return;
      int num = 500;
      while (!GamePad.SetVibration(Game1.playerOneIndex, 0.0f, 0.0f) && num > 0)
        --num;
      Rumble.isRumbling = false;
    }

    public static void stopRumbling()
    {
      int num = 0;
      while (GamePad.GetState(Game1.playerOneIndex).IsConnected && !GamePad.SetVibration(Game1.playerOneIndex, 0.0f, 0.0f) && num < 5)
        ++num;
      Rumble.isRumbling = false;
    }

    public static void rumble(float leftPower, float rightPower, float milliseconds)
    {
      if (Rumble.isRumbling || !Game1.options.gamepadControls || !Game1.options.rumble)
        return;
      Rumble.rumbleTimerCurrent = 0.0f;
      Rumble.fade = false;
      Rumble.rumbleTimerMax = milliseconds;
      Rumble.isRumbling = true;
      GamePad.SetVibration(Game1.playerOneIndex, leftPower, rightPower);
    }

    public static void rumble(float power, float milliseconds)
    {
      if (Rumble.isRumbling || !Game1.options.gamepadControls || !Game1.options.rumble)
        return;
      Rumble.fade = false;
      Rumble.rumbleTimerCurrent = 0.0f;
      Rumble.rumbleTimerMax = milliseconds;
      Rumble.isRumbling = true;
      GamePad.SetVibration(Game1.playerOneIndex, power, power);
    }

    public static void rumbleAndFade(float power, float milliseconds)
    {
      if (Rumble.isRumbling || !Game1.options.gamepadControls || !Game1.options.rumble)
        return;
      Rumble.rumbleTimerCurrent = 0.0f;
      Rumble.rumbleTimerMax = milliseconds;
      Rumble.isRumbling = true;
      GamePad.SetVibration(Game1.playerOneIndex, power, power);
      Rumble.fade = true;
      Rumble.rumbleDuringFade = power;
      Rumble.maxRumbleDuringFade = power;
    }
  }
}
