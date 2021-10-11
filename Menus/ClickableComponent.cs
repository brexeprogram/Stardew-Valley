// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.ClickableComponent
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;

namespace StardewValley.Menus
{
  public class ClickableComponent
  {
    public float scale = 1f;
    public bool visible = true;
    public int myID = -500;
    public int leftNeighborID = -1;
    public int rightNeighborID = -1;
    public int upNeighborID = -1;
    public int downNeighborID = -1;
    public int myAlternateID = -500;
    public const int ID_ignore = -500;
    public const int CUSTOM_SNAP_BEHAVIOR = -7777;
    public const int SNAP_TO_DEFAULT = -99999;
    public Rectangle bounds;
    public string name;
    public string label;
    public Item item;
    public bool leftNeighborImmutable;
    public bool rightNeighborImmutable;
    public bool upNeighborImmutable;
    public bool downNeighborImmutable;
    public bool tryDefaultIfNoDownNeighborExists;
    public bool tryDefaultIfNoRightNeighborExists;
    public bool fullyImmutable;
    public int region;

    public ClickableComponent(Rectangle bounds, string name)
    {
      this.bounds = bounds;
      this.name = name;
    }

    public ClickableComponent(Rectangle bounds, string name, string label)
    {
      this.bounds = bounds;
      this.name = name;
      this.label = label;
    }

    public ClickableComponent(Rectangle bounds, Item item)
    {
      this.bounds = bounds;
      this.item = item;
    }

    public virtual bool containsPoint(int x, int y)
    {
      if (!this.visible || !this.bounds.Contains(x, y))
        return false;
      Game1.SetFreeCursorDrag();
      return true;
    }

    public virtual void snapMouseCursor()
    {
      Game1.setMousePosition(this.bounds.Right - this.bounds.Width / 8, this.bounds.Bottom - this.bounds.Height / 8);
    }

    public void snapMouseCursorToCenter()
    {
      Game1.setMousePosition(this.bounds.Center.X, this.bounds.Center.Y);
    }
  }
}
