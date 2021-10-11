// Decompiled with JetBrains decompiler
// Type: StardewValley.Tools.Blueprints
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using StardewValley.Menus;

namespace StardewValley.Tools
{
  public class Blueprints : Tool
  {
    public Blueprints()
      : base("Farmer's Catalogue", 0, 75, 75, false, 0)
    {
      this.upgradeLevel = 0;
      this.CurrentParentTileIndex = this.indexOfMenuItemView;
      this.instantUse = true;
    }

    protected override string loadDisplayName()
    {
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:Blueprints.cs.14039");
    }

    protected override string loadDescription()
    {
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:Blueprints.cs.14040");
    }

    public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
    {
      if (Game1.activeClickableMenu == null)
      {
        Game1.activeClickableMenu = (IClickableMenu) new BlueprintsMenu(Game1.viewport.Width / 2 - (Game1.viewport.Width / 2 + Game1.tileSize * 3 / 2) / 2, Game1.viewport.Height / 4);
        ((BlueprintsMenu) Game1.activeClickableMenu).changePosition(Game1.activeClickableMenu.xPositionOnScreen, Game1.viewport.Height / 2 - Game1.activeClickableMenu.height / 2);
      }
      this.CurrentParentTileIndex = this.indexOfMenuItemView;
    }

    public override void tickUpdate(GameTime time, Farmer who)
    {
    }
  }
}
