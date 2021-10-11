// Decompiled with JetBrains decompiler
// Type: StardewValley.LevelBuilder
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using StardewValley.Monsters;
using xTile.Dimensions;

namespace StardewValley
{
  public class LevelBuilder
  {
    public static bool tryToAddObject(int index, bool bigCraftable, Vector2 position)
    {
      if (Game1.mine.isTileOccupiedForPlacement(position, (Object) null))
        return false;
      if (bigCraftable)
        Game1.mine.objects.Add(position, new Object(position, index, false));
      else
        Game1.mine.Objects.Add(position, new Object(position, index, (string) null, false, false, false, false));
      return true;
    }

    public static bool tryToAddMonster(Monster m, Vector2 position)
    {
      if (Game1.mine.isTileOccupiedForPlacement(position, (Object) null) || !Game1.mine.isTileLocationOpen(new Location((int) position.X, (int) position.Y)) || (!Game1.mine.isTileOnMap(position) || !Game1.mine.isTileOnClearAndSolidGround(position)))
        return false;
      m.position = new Vector2(position.X * (float) Game1.tileSize, position.Y * (float) Game1.tileSize - (float) (m.sprite.spriteHeight - Game1.tileSize));
      Game1.mine.characters.Add((NPC) m);
      return true;
    }

    public static bool tryToAddFence(int which, Vector2 position, bool gate)
    {
      if (!Game1.mine.isTileOccupiedForPlacement(position, (Object) null))
        Game1.mine.objects.Add(position, (Object) new Fence(position, which, gate));
      return false;
    }

    public static bool addTorch(Vector2 position)
    {
      if (Game1.mine.isTileOccupiedForPlacement(position, (Object) null))
        return false;
      Game1.mine.Objects.Add(position, (Object) new Torch(position, 1));
      return true;
    }

    public static bool tryToAddObject(Object obj, Vector2 position)
    {
      if (Game1.mine.isTileOccupiedForPlacement(position, (Object) null))
        return false;
      Game1.mine.Objects.Add(position, obj);
      return true;
    }

    public static bool tryToAddObject(int index, bool bigCraftable, Vector2 position, int heldItem)
    {
      if (LevelBuilder.tryToAddObject(index, bigCraftable, position))
        Game1.mine.objects[position].heldObject = new Object(position, heldItem, (string) null, false, false, false, false);
      return false;
    }
  }
}
