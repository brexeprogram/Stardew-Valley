// Decompiled with JetBrains decompiler
// Type: StardewValley.Warp
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

namespace StardewValley
{
  public class Warp
  {
    private int x;
    private int y;
    private int targetX;
    private int targetY;
    public bool flipFarmer;
    private string targetName;

    public int X
    {
      get
      {
        return this.x;
      }
    }

    public int Y
    {
      get
      {
        return this.y;
      }
    }

    public int TargetX
    {
      get
      {
        return this.targetX;
      }
      set
      {
        this.targetX = value;
      }
    }

    public int TargetY
    {
      get
      {
        return this.targetY;
      }
      set
      {
        this.targetY = value;
      }
    }

    public string TargetName
    {
      get
      {
        return this.targetName;
      }
    }

    public Warp(int x, int y, string targetName, int targetX, int targetY, bool flipFarmer)
    {
      this.x = x;
      this.y = y;
      this.targetX = targetX;
      this.targetY = targetY;
      this.targetName = targetName;
      this.flipFarmer = flipFarmer;
    }
  }
}
