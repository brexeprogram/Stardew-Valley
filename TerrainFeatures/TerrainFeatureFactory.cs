// Decompiled with JetBrains decompiler
// Type: StardewValley.TerrainFeatures.TerrainFeatureFactory
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using System;

namespace StardewValley.TerrainFeatures
{
  public class TerrainFeatureFactory
  {
    public const byte grass1 = 0;
    public const byte grass2 = 1;
    public const byte grass3 = 2;
    public const byte grass4 = 3;
    public const byte bushyTree = 4;
    public const byte leafyTree = 5;
    public const byte pineTree = 6;
    public const byte winterTree1 = 7;
    public const byte winterTree2 = 8;
    public const byte palmTree = 9;
    public const byte quartzSmall = 10;
    public const byte quartzMedium = 11;
    public const byte hoeDirt = 12;
    public const byte cosmeticPlant = 13;
    public const byte floorWood = 14;
    public const byte floorStone = 15;
    public const byte mushroomTree = 16;
    public const byte floorIce = 17;
    public const byte floorGhost = 18;
    public const byte floorStraw = 19;
    public const byte stump = 20;

    public static TerrainFeature getNewTerrainFeatureFromIndex(byte index, int extraInfo)
    {
      switch (index)
      {
        case 0:
        case 1:
        case 2:
        case 3:
          return (TerrainFeature) new Grass((int) index + 1, extraInfo);
        case 4:
        case 5:
        case 6:
        case 7:
        case 8:
        case 9:
          return (TerrainFeature) new Tree((int) index - 3, extraInfo);
        case 10:
          return (TerrainFeature) new Quartz(1, Game1.mine != null ? Game1.mine.getCrystalColorForThisLevel() : Color.Green);
        case 11:
          return (TerrainFeature) new Quartz(2, Game1.mine != null ? Game1.mine.getCrystalColorForThisLevel() : Color.Green);
        case 12:
          return (TerrainFeature) new HoeDirt(Game1.isRaining ? 1 : 0);
        case 13:
          return (TerrainFeature) new CosmeticPlant(extraInfo);
        case 14:
          return (TerrainFeature) new Flooring(0);
        case 15:
          return (TerrainFeature) new Flooring(1);
        case 16:
          return (TerrainFeature) new Tree(7, extraInfo);
        case 17:
          return (TerrainFeature) new Flooring(3);
        case 18:
          return (TerrainFeature) new Flooring(2);
        case 19:
          return (TerrainFeature) new Flooring(4);
        case 20:
          return (TerrainFeature) new ResourceClump(600, 2, 2, Vector2.Zero);
        default:
          throw new MissingMethodException();
      }
    }

    public static TerrainFeatureDescription getIndexFromTerrainFeature(TerrainFeature f)
    {
      if (f.GetType() == typeof (CosmeticPlant))
        return new TerrainFeatureDescription((byte) 13, (int) ((Grass) f).grassType);
      if (f.GetType() == typeof (Grass))
      {
        switch (((Grass) f).grassType)
        {
          case 1:
            return new TerrainFeatureDescription((byte) 0, ((Grass) f).numberOfWeeds);
          case 2:
            return new TerrainFeatureDescription((byte) 1, ((Grass) f).numberOfWeeds);
          case 3:
            return new TerrainFeatureDescription((byte) 2, ((Grass) f).numberOfWeeds);
          case 4:
            return new TerrainFeatureDescription((byte) 3, ((Grass) f).numberOfWeeds);
        }
      }
      else if (f.GetType() == typeof (Tree))
      {
        switch (((Tree) f).treeType)
        {
          case 1:
            return new TerrainFeatureDescription((byte) 4, ((Tree) f).growthStage);
          case 2:
            return new TerrainFeatureDescription((byte) 5, ((Tree) f).growthStage);
          case 3:
            return new TerrainFeatureDescription((byte) 6, ((Tree) f).growthStage);
          case 4:
            return new TerrainFeatureDescription((byte) 7, ((Tree) f).growthStage);
          case 5:
            return new TerrainFeatureDescription((byte) 8, ((Tree) f).growthStage);
          case 7:
            return new TerrainFeatureDescription((byte) 16, ((Tree) f).growthStage);
        }
      }
      else if (f.GetType() == typeof (Quartz))
      {
        switch (((Quartz) f).bigness)
        {
          case 1:
            return new TerrainFeatureDescription((byte) 10, -1);
          case 2:
            return new TerrainFeatureDescription((byte) 11, -1);
        }
      }
      else
      {
        if (f.GetType() == typeof (HoeDirt))
          return new TerrainFeatureDescription((byte) 12, -1);
        if (f.GetType() == typeof (Flooring))
        {
          switch (((Flooring) f).whichFloor)
          {
            case 0:
              return new TerrainFeatureDescription((byte) 14, -1);
            case 1:
              return new TerrainFeatureDescription((byte) 15, -1);
          }
        }
        else if (f is ResourceClump && (f as ResourceClump).parentSheetIndex == 600)
          return new TerrainFeatureDescription((byte) 20, -1);
      }
      throw new MissingMethodException();
    }
  }
}
