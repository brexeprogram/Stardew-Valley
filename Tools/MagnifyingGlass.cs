// Decompiled with JetBrains decompiler
// Type: StardewValley.Tools.MagnifyingGlass
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using StardewValley.Menus;
using System.Collections.Generic;

namespace StardewValley.Tools
{
  public class MagnifyingGlass : Tool
  {
    public MagnifyingGlass()
      : base("Magnifying Glass", -1, 5, 5, false, 0)
    {
      this.instantUse = true;
    }

    protected override string loadDisplayName()
    {
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:MagnifyingGlass.cs.14119");
    }

    protected override string loadDescription()
    {
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:MagnifyingGlass.cs.14120");
    }

    public override bool beginUsing(GameLocation location, int x, int y, Farmer who)
    {
      who.Halt();
      who.canMove = true;
      who.usingTool = false;
      this.DoFunction(location, Game1.getOldMouseX() + Game1.viewport.X, Game1.getOldMouseY() + Game1.viewport.Y, 0, who);
      return true;
    }

    public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
    {
      base.DoFunction(location, x, y, power, who);
      this.currentParentTileIndex = 5;
      this.indexOfMenuItemView = 5;
      Rectangle rectangle = new Rectangle(x / Game1.tileSize * Game1.tileSize, y / Game1.tileSize * Game1.tileSize, Game1.tileSize, Game1.tileSize);
      if (location is Farm)
      {
        foreach (KeyValuePair<long, FarmAnimal> animal in (Dictionary<long, FarmAnimal>) (location as Farm).animals)
        {
          if (animal.Value.GetBoundingBox().Intersects(rectangle))
          {
            Game1.activeClickableMenu = (IClickableMenu) new AnimalQueryMenu(animal.Value);
            break;
          }
        }
      }
      else
      {
        if (!(location is AnimalHouse))
          return;
        foreach (KeyValuePair<long, FarmAnimal> animal in (Dictionary<long, FarmAnimal>) (location as AnimalHouse).animals)
        {
          if (animal.Value.GetBoundingBox().Intersects(rectangle))
          {
            Game1.activeClickableMenu = (IClickableMenu) new AnimalQueryMenu(animal.Value);
            break;
          }
        }
      }
    }
  }
}
