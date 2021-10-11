// Decompiled with JetBrains decompiler
// Type: StardewValley.PathNode
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

namespace StardewValley
{
  public class PathNode
  {
    public int x;
    public int y;
    public byte g;
    public PathNode parent;

    public PathNode(int x, int y, PathNode parent)
    {
      this.x = x;
      this.y = y;
      this.parent = parent;
    }

    public PathNode(int x, int y, byte g, PathNode parent)
    {
      this.x = x;
      this.y = y;
      this.g = g;
      this.parent = parent;
    }

    public override bool Equals(object obj)
    {
      if (this.x == ((PathNode) obj).x)
        return this.y == ((PathNode) obj).y;
      return false;
    }

    public override int GetHashCode()
    {
      return 100000 * this.x + this.y;
    }
  }
}
