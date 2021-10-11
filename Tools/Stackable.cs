// Decompiled with JetBrains decompiler
// Type: StardewValley.Tools.Stackable
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

namespace StardewValley.Tools
{
  public abstract class Stackable : Tool
  {
    private int numberInStack;

    public int NumberInStack
    {
      get
      {
        return this.numberInStack;
      }
      set
      {
        this.numberInStack = value;
      }
    }

    public Stackable()
    {
    }

    public Stackable(string name, int upgradeLevel, int initialParentTileIndex, int indexOfMenuItemView, bool stackable)
      : base(name, upgradeLevel, initialParentTileIndex, indexOfMenuItemView, stackable, 0)
    {
    }
  }
}
