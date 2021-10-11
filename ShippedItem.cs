// Decompiled with JetBrains decompiler
// Type: StardewValley.ShippedItem
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

namespace StardewValley
{
  internal struct ShippedItem
  {
    public int index;
    public int price;
    public string name;

    public ShippedItem(int index, int price, string name)
    {
      this.index = index;
      this.price = price;
      this.name = name;
    }
  }
}
