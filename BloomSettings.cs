// Decompiled with JetBrains decompiler
// Type: StardewValley.BloomSettings
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

namespace StardewValley
{
  public class BloomSettings
  {
    public static BloomSettings[] PresetSettings = new BloomSettings[8]
    {
      new BloomSettings("Default", 0.25f, 4f, 1.25f, 1f, 1f, 1f, false),
      new BloomSettings("Soft", 0.0f, 3f, 1f, 1f, 1f, 1f, false),
      new BloomSettings("Desaturated", 0.5f, 8f, 2f, 1f, 0.0f, 1f, false),
      new BloomSettings("Saturated", 0.25f, 4f, 2f, 1f, 2f, 0.0f, false),
      new BloomSettings("RainyDay", 0.0f, 2f, 0.7f, 1f, 0.5f, 0.5f, false),
      new BloomSettings("SunnyDay", 0.6f, 4f, 0.7f, 1f, 1f, 1f, false),
      new BloomSettings("B&W", 0.0f, 0.0f, 0.0f, 1f, 0.0f, 0.0f, false),
      new BloomSettings("NightTimeLights", 0.0f, 3f, 7f, 1f, 4f, 1.2f, true)
    };
    public const int nightTimeLights = 7;
    public string Name;
    public float BloomThreshold;
    public float BlurAmount;
    public float BloomIntensity;
    public float BaseIntensity;
    public float BloomSaturation;
    public float BaseSaturation;
    public bool brightWhiteOnly;

    public BloomSettings(string name, float bloomThreshold, float blurAmount, float bloomIntensity, float baseIntensity, float bloomSaturation, float baseSaturation, bool brightWhiteOnly = false)
    {
      this.Name = name;
      this.BloomThreshold = bloomThreshold;
      this.BlurAmount = blurAmount;
      this.BloomIntensity = bloomIntensity;
      this.BaseIntensity = baseIntensity;
      this.BloomSaturation = bloomSaturation;
      this.BaseSaturation = baseSaturation;
      this.brightWhiteOnly = brightWhiteOnly;
    }
  }
}
