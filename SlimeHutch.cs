// Decompiled with JetBrains decompiler
// Type: StardewValley.SlimeHutch
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using StardewValley.Buildings;
using StardewValley.Monsters;
using StardewValley.Tools;
using System;
using xTile;

namespace StardewValley
{
  public class SlimeHutch : GameLocation
  {
    public bool[] waterSpots = new bool[4];
    private int slimeMatingsLeft;

    public SlimeHutch()
    {
    }

    public SlimeHutch(Map m, string name)
      : base(m, name)
    {
    }

    public void updateWhenNotCurrentLocation(Building parentBuilding, GameTime time)
    {
    }

    public bool isFull()
    {
      return this.characters.Count >= 20;
    }

    public override void resetForPlayerEntry()
    {
      base.resetForPlayerEntry();
      for (int index = 0; index < this.waterSpots.Length; ++index)
      {
        if (this.waterSpots[index])
          this.setMapTileIndex(16, 6 + index, 2135, "Buildings", 0);
        else
          this.setMapTileIndex(16, 6 + index, 2134, "Buildings", 0);
      }
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

    public override bool canSlimeMateHere()
    {
      int slimeMatingsLeft = this.slimeMatingsLeft;
      this.slimeMatingsLeft = this.slimeMatingsLeft - 1;
      if (!this.isFull())
        return slimeMatingsLeft > 0;
      return false;
    }

    public override bool canSlimeHatchHere()
    {
      return !this.isFull();
    }

    public override void DayUpdate(int dayOfMonth)
    {
      int val2 = 0;
      for (int index = 0; index < this.waterSpots.Length; ++index)
      {
        if (this.waterSpots[index] && val2 * 5 < this.characters.Count)
        {
          ++val2;
          this.waterSpots[index] = false;
          this.setMapTileIndex(16, 6 + index, 2134, "Buildings", 0);
        }
      }
      for (int index = Math.Min(this.characters.Count / 5, val2); index > 0; --index)
      {
        int num = 50;
        Vector2 randomTile;
        for (randomTile = this.getRandomTile(); (!this.isTileLocationTotallyClearAndPlaceable(randomTile) || this.doesTileHaveProperty((int) randomTile.X, (int) randomTile.Y, "NPCBarrier", "Back") != null || (double) randomTile.Y >= 12.0) && num > 0; --num)
          randomTile = this.getRandomTile();
        if (num > 0)
          this.objects.Add(randomTile, new Object(randomTile, 56, false));
      }
      for (; this.slimeMatingsLeft > 0; this.slimeMatingsLeft = this.slimeMatingsLeft - 1)
      {
        if (this.characters.Count > 1 && !this.isFull())
        {
          NPC character = this.characters[Game1.random.Next(this.characters.Count)];
          if (character is GreenSlime)
          {
            GreenSlime greenSlime = character as GreenSlime;
            if (greenSlime.ageUntilFullGrown <= 0)
            {
              for (int index = 1; index < 10; ++index)
              {
                GreenSlime mateToPursue = (GreenSlime) Utility.checkForCharacterWithinArea(greenSlime.GetType(), character.position, (GameLocation) this, new Rectangle((int) greenSlime.position.X - Game1.tileSize * index, (int) greenSlime.position.Y - Game1.tileSize * index, Game1.tileSize * (index * 2 + 1), Game1.tileSize * (index * 2 + 1)));
                if (mateToPursue != null && mateToPursue.cute != greenSlime.cute && mateToPursue.ageUntilFullGrown <= 0)
                {
                  greenSlime.mateWith(mateToPursue, (GameLocation) this);
                  break;
                }
              }
            }
          }
        }
      }
      this.slimeMatingsLeft = this.characters.Count / 5 + 1;
      base.DayUpdate(dayOfMonth);
    }

    public override bool performToolAction(Tool t, int tileX, int tileY)
    {
      if (t is WateringCan && tileX == 16)
      {
        switch (tileY)
        {
          case 6:
            this.setMapTileIndex(tileX, tileY, 2135, "Buildings", 0);
            this.waterSpots[0] = true;
            break;
          case 7:
            this.setMapTileIndex(tileX, tileY, 2135, "Buildings", 0);
            this.waterSpots[1] = true;
            break;
          case 8:
            this.setMapTileIndex(tileX, tileY, 2135, "Buildings", 0);
            this.waterSpots[2] = true;
            break;
          case 9:
            this.setMapTileIndex(tileX, tileY, 2135, "Buildings", 0);
            this.waterSpots[3] = true;
            break;
        }
      }
      return false;
    }
  }
}
