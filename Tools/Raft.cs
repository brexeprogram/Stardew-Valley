// Decompiled with JetBrains decompiler
// Type: StardewValley.Tools.Raft
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;

namespace StardewValley.Tools
{
  public class Raft : Tool
  {
    public Raft()
      : base(nameof (Raft), 0, 1, 1, false, 0)
    {
      this.upgradeLevel = 0;
      this.CurrentParentTileIndex = this.indexOfMenuItemView;
      this.instantUse = true;
    }

    protected override string loadDisplayName()
    {
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:Raft.cs.14204");
    }

    protected override string loadDescription()
    {
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:Raft.cs.14205");
    }

    public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
    {
      base.DoFunction(location, x, y, power, who);
      if (!who.isRafting && location.doesTileHaveProperty(x / Game1.tileSize, y / Game1.tileSize, "Water", "Back") != null)
      {
        who.isRafting = true;
        Rectangle position = new Rectangle(x - Game1.tileSize / 2, y - Game1.tileSize / 2, Game1.tileSize, Game1.tileSize);
        if (location.isCollidingPosition(position, Game1.viewport, true))
        {
          who.isRafting = false;
          return;
        }
        who.xVelocity = who.facingDirection == 1 ? 3f : (who.facingDirection == 3 ? -3f : 0.0f);
        who.yVelocity = who.facingDirection == 2 ? 3f : (who.facingDirection == 0 ? -3f : 0.0f);
        who.position = new Vector2((float) (x - Game1.tileSize / 2), (float) (y - Game1.tileSize / 2 - Game1.tileSize / 2 - (y < who.getStandingY() ? Game1.tileSize : 0)));
        Game1.playSound("dropItemInWater");
      }
      this.CurrentParentTileIndex = this.indexOfMenuItemView;
    }
  }
}
