// Decompiled with JetBrains decompiler
// Type: StardewValley.Shed
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using StardewValley.Buildings;
using StardewValley.Locations;
using System.Collections.Generic;
using xTile;

namespace StardewValley
{
  public class Shed : DecoratableLocation
  {
    public Shed()
    {
    }

    public Shed(Map m, string name)
      : base(m, name)
    {
      List<Rectangle> walls = DecoratableLocation.getWalls();
      while (this.wallPaper.Count < walls.Count)
        this.wallPaper.Add(0);
      List<Rectangle> floors = DecoratableLocation.getFloors();
      while (this.floor.Count < floors.Count)
        this.floor.Add(0);
    }

    public void updateWhenNotCurrentLocation(Building parentBuilding, GameTime time)
    {
    }

    public override void resetForPlayerEntry()
    {
      base.resetForPlayerEntry();
      if (!Game1.isDarkOut())
        return;
      Game1.ambientLight = new Color(180, 180, 0);
    }

    public Building getBuilding()
    {
      foreach (Building building in Game1.getFarm().buildings)
      {
        if (building.indoors != null && building.indoors.Equals((object) this))
          return building;
      }
      return (Building) null;
    }
  }
}
