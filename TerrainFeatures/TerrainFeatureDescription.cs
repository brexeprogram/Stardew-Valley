// Decompiled with JetBrains decompiler
// Type: StardewValley.TerrainFeatures.TerrainFeatureDescription
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

namespace StardewValley.TerrainFeatures
{
  public struct TerrainFeatureDescription
  {
    public byte index;
    public int extraInfo;

    public TerrainFeatureDescription(byte index, int extraInfo)
    {
      this.index = index;
      this.extraInfo = extraInfo;
    }
  }
}
