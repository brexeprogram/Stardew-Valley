// Decompiled with JetBrains decompiler
// Type: StardewValley.Tools.ToolDescription
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

namespace StardewValley.Tools
{
  public struct ToolDescription
  {
    public byte index;
    public byte upgradeLevel;

    public ToolDescription(byte index, byte upgradeLevel)
    {
      this.index = index;
      this.upgradeLevel = upgradeLevel;
    }
  }
}
