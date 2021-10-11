// Decompiled with JetBrains decompiler
// Type: StardewValley.Tools.Hoe
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using StardewValley.TerrainFeatures;
using System.Collections.Generic;
using xTile.Dimensions;

namespace StardewValley.Tools
{
  public class Hoe : Tool
  {
    public Hoe()
      : base(nameof (Hoe), 0, 21, 47, false, 0)
    {
      this.upgradeLevel = 0;
    }

    protected override string loadDisplayName()
    {
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:Hoe.cs.14101");
    }

    protected override string loadDescription()
    {
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:Hoe.cs.14102");
    }

    public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
    {
      base.DoFunction(location, x, y, power, who);
      if (location.Name.Equals("UndergroundMine"))
        power = 1;
      who.Stamina = who.Stamina - ((float) (2 * power) - (float) who.FarmingLevel * 0.1f);
      power = who.toolPower;
      who.stopJittering();
      Game1.playSound("woodyHit");
      Vector2 vector2 = new Vector2((float) (x / Game1.tileSize), (float) (y / Game1.tileSize));
      List<Vector2> vector2List = this.tilesAffected(vector2, power, who);
      foreach (Vector2 index in vector2List)
      {
        index.Equals(vector2);
        if (location.terrainFeatures.ContainsKey(index))
        {
          if (location.terrainFeatures[index].performToolAction((Tool) this, 0, index, (GameLocation) null))
            location.terrainFeatures.Remove(index);
        }
        else
        {
          if (location.objects.ContainsKey(index) && location.Objects[index].performToolAction((Tool) this))
          {
            if (location.Objects[index].type.Equals("Crafting") && location.Objects[index].fragility != 2)
            {
              List<Debris> debris1 = location.debris;
              int objectIndex = location.Objects[index].bigCraftable ? -location.Objects[index].ParentSheetIndex : location.Objects[index].ParentSheetIndex;
              Vector2 toolLocation = who.GetToolLocation(false);
              Microsoft.Xna.Framework.Rectangle boundingBox = who.GetBoundingBox();
              double x1 = (double) boundingBox.Center.X;
              boundingBox = who.GetBoundingBox();
              double y1 = (double) boundingBox.Center.Y;
              Vector2 playerPosition = new Vector2((float) x1, (float) y1);
              Debris debris2 = new Debris(objectIndex, toolLocation, playerPosition);
              debris1.Add(debris2);
            }
            location.Objects[index].performRemoveAction(index, location);
            location.Objects.Remove(index);
          }
          if (location.doesTileHaveProperty((int) index.X, (int) index.Y, "Diggable", "Back") != null)
          {
            if (location.Name.Equals("UndergroundMine") && !location.isTileOccupied(index, ""))
            {
              if (Game1.mine.mineLevel < 40 || Game1.mine.mineLevel >= 80)
              {
                location.terrainFeatures.Add(index, (TerrainFeature) new HoeDirt());
                Game1.playSound("hoeHit");
              }
              else if (Game1.mine.mineLevel < 80)
              {
                location.terrainFeatures.Add(index, (TerrainFeature) new HoeDirt());
                Game1.playSound("hoeHit");
              }
              Game1.removeSquareDebrisFromTile((int) index.X, (int) index.Y);
              location.checkForBuriedItem((int) index.X, (int) index.Y, false, false);
              location.temporarySprites.Add(new TemporaryAnimatedSprite(12, new Vector2(vector2.X * (float) Game1.tileSize, vector2.Y * (float) Game1.tileSize), Color.White, 8, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, -1, 0));
              if (vector2List.Count > 2)
                location.temporarySprites.Add(new TemporaryAnimatedSprite(6, new Vector2(index.X * (float) Game1.tileSize, index.Y * (float) Game1.tileSize), Color.White, 8, Game1.random.NextDouble() < 0.5, Vector2.Distance(vector2, index) * 30f, 0, -1, -1f, -1, 0));
            }
            else if (!location.isTileOccupied(index, "") && location.isTilePassable(new Location((int) index.X, (int) index.Y), Game1.viewport))
            {
              location.makeHoeDirt(index);
              Game1.playSound("hoeHit");
              Game1.removeSquareDebrisFromTile((int) index.X, (int) index.Y);
              location.temporarySprites.Add(new TemporaryAnimatedSprite(12, new Vector2(index.X * (float) Game1.tileSize, index.Y * (float) Game1.tileSize), Color.White, 8, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, -1, 0));
              if (vector2List.Count > 2)
                location.temporarySprites.Add(new TemporaryAnimatedSprite(6, new Vector2(index.X * (float) Game1.tileSize, index.Y * (float) Game1.tileSize), Color.White, 8, Game1.random.NextDouble() < 0.5, Vector2.Distance(vector2, index) * 30f, 0, -1, -1f, -1, 0));
              location.checkForBuriedItem((int) index.X, (int) index.Y, false, false);
            }
            ++Game1.stats.DirtHoed;
          }
        }
      }
    }
  }
}
