// Decompiled with JetBrains decompiler
// Type: StardewValley.Tools.Axe
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using xTile.ObjectModel;

namespace StardewValley.Tools
{
  public class Axe : Tool
  {
    public const int StumpStrength = 4;
    private int stumpTileX;
    private int stumpTileY;
    private int hitsToStump;

    public Axe()
      : base(nameof (Axe), 0, 189, 215, false, 0)
    {
      this.upgradeLevel = 0;
    }

    protected override string loadDisplayName()
    {
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:Axe.cs.1");
    }

    protected override string loadDescription()
    {
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:Axe.cs.14019");
    }

    public override bool beginUsing(GameLocation location, int x, int y, Farmer who)
    {
      this.Update(who.facingDirection, 0, who);
      if (who.IsMainPlayer)
      {
        Game1.releaseUseToolButton();
        return true;
      }
      switch (who.FacingDirection)
      {
        case 0:
          who.FarmerSprite.setCurrentFrame(176);
          who.CurrentTool.Update(0, 0);
          break;
        case 1:
          who.FarmerSprite.setCurrentFrame(168);
          who.CurrentTool.Update(1, 0);
          break;
        case 2:
          who.FarmerSprite.setCurrentFrame(160);
          who.CurrentTool.Update(2, 0);
          break;
        case 3:
          who.FarmerSprite.setCurrentFrame(184);
          who.CurrentTool.Update(3, 0);
          break;
      }
      return true;
    }

    public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
    {
      base.DoFunction(location, x, y, power, who);
      who.Stamina = who.Stamina - ((float) (2 * power) - (float) who.ForagingLevel * 0.1f);
      int tileX = x / Game1.tileSize;
      int tileY = y / Game1.tileSize;
      Rectangle rectangle = new Rectangle(tileX * Game1.tileSize, tileY * Game1.tileSize, Game1.tileSize, Game1.tileSize);
      Vector2 index1 = new Vector2((float) tileX, (float) tileY);
      if (location.map.GetLayer("Buildings").Tiles[tileX, tileY] != null)
      {
        PropertyValue propertyValue = (PropertyValue) null;
        location.map.GetLayer("Buildings").Tiles[tileX, tileY].TileIndexProperties.TryGetValue("TreeStump", out propertyValue);
        if (propertyValue != null)
        {
          Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Axe.cs.14023"));
          return;
        }
      }
      location.performToolAction((Tool) this, tileX, tileY);
      if (location.terrainFeatures.ContainsKey(index1) && location.terrainFeatures[index1].performToolAction((Tool) this, 0, index1, (GameLocation) null))
        location.terrainFeatures.Remove(index1);
      Rectangle boundingBox;
      if (location.largeTerrainFeatures != null)
      {
        for (int index2 = location.largeTerrainFeatures.Count - 1; index2 >= 0; --index2)
        {
          boundingBox = location.largeTerrainFeatures[index2].getBoundingBox();
          if (boundingBox.Intersects(rectangle) && location.largeTerrainFeatures[index2].performToolAction((Tool) this, 0, index1, (GameLocation) null))
            location.largeTerrainFeatures.RemoveAt(index2);
        }
      }
      Vector2 index3 = new Vector2((float) tileX, (float) tileY);
      if (!location.Objects.ContainsKey(index3) || location.Objects[index3].Type == null || !location.Objects[index3].performToolAction((Tool) this))
        return;
      if (location.Objects[index3].type.Equals("Crafting") && location.Objects[index3].fragility != 2)
      {
        List<Debris> debris1 = location.debris;
        int objectIndex = location.Objects[index3].bigCraftable ? -location.Objects[index3].ParentSheetIndex : location.Objects[index3].ParentSheetIndex;
        Vector2 toolLocation = who.GetToolLocation(false);
        boundingBox = who.GetBoundingBox();
        double x1 = (double) boundingBox.Center.X;
        boundingBox = who.GetBoundingBox();
        double y1 = (double) boundingBox.Center.Y;
        Vector2 playerPosition = new Vector2((float) x1, (float) y1);
        Debris debris2 = new Debris(objectIndex, toolLocation, playerPosition);
        debris1.Add(debris2);
      }
      location.Objects[index3].performRemoveAction(index3, location);
      location.Objects.Remove(index3);
    }
  }
}
