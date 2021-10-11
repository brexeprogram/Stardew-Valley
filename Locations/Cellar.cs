// Decompiled with JetBrains decompiler
// Type: StardewValley.Locations.Cellar
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using StardewValley.Objects;
using xTile;

namespace StardewValley.Locations
{
  public class Cellar : GameLocation
  {
    public Cellar()
    {
    }

    public Cellar(Map map, string name)
      : base(map, name)
    {
      this.setUpAgingBoards();
    }

    public override void DayUpdate(int dayOfMonth)
    {
      base.DayUpdate(dayOfMonth);
    }

    public void setUpAgingBoards()
    {
      for (int index = 6; index < 17; ++index)
      {
        Vector2 vector2 = new Vector2((float) index, 8f);
        if (!this.objects.ContainsKey(vector2))
          this.objects.Add(vector2, (Object) new Cask(vector2));
        vector2 = new Vector2((float) index, 10f);
        if (!this.objects.ContainsKey(vector2))
          this.objects.Add(vector2, (Object) new Cask(vector2));
        vector2 = new Vector2((float) index, 12f);
        if (!this.objects.ContainsKey(vector2))
          this.objects.Add(vector2, (Object) new Cask(vector2));
      }
    }
  }
}
