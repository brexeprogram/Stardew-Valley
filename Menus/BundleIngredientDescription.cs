// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.BundleIngredientDescription
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

namespace StardewValley.Menus
{
  public struct BundleIngredientDescription
  {
    public int index;
    public int stack;
    public int quality;
    public bool completed;

    public BundleIngredientDescription(int index, int stack, int quality, bool completed)
    {
      this.completed = completed;
      this.index = index;
      this.stack = stack;
      this.quality = quality;
    }
  }
}
