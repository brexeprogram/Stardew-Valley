// Decompiled with JetBrains decompiler
// Type: StardewValley.Tools.WateringCan
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Locations;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Tools
{
  public class WateringCan : Tool
  {
    public int waterCanMax = 40;
    private int waterLeft = 40;

    public int WaterLeft
    {
      get
      {
        return this.waterLeft;
      }
      set
      {
        this.waterLeft = value;
      }
    }

    public override int UpgradeLevel
    {
      get
      {
        return this.upgradeLevel;
      }
      set
      {
        this.upgradeLevel = value;
        this.setNewTileIndexForUpgradeLevel();
      }
    }

    public WateringCan()
      : base("Watering Can", 0, 273, 296, false, 0)
    {
      this.upgradeLevel = 0;
    }

    protected override string loadDisplayName()
    {
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:WateringCan.cs.14324");
    }

    protected override string loadDescription()
    {
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:WateringCan.cs.14325");
    }

    public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber)
    {
      base.drawInMenu(spriteBatch, location + new Vector2(0.0f, (float) (-Game1.tileSize / 4 + 4)), scaleSize, transparency, layerDepth, drawStackNumber);
      if (!drawStackNumber)
        return;
      spriteBatch.Draw(Game1.mouseCursors, location + new Vector2(4f, (float) (Game1.tileSize - 20)), new Rectangle?(new Rectangle(297, 420, 14, 5)), Color.White * transparency, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, layerDepth + 0.0001f);
      spriteBatch.Draw(Game1.staminaRect, new Rectangle((int) location.X + 8, (int) location.Y + Game1.tileSize - 16, (int) ((double) this.waterLeft / (double) this.waterCanMax * 48.0), 8), Color.DodgerBlue * 0.7f * transparency);
    }

    public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
    {
      base.DoFunction(location, x, y, power, who);
      power = who.toolPower;
      who.stopJittering();
      List<Vector2> source = this.tilesAffected(new Vector2((float) (x / Game1.tileSize), (float) (y / Game1.tileSize)), power, who);
      if (location.doesTileHaveProperty(x / Game1.tileSize, y / Game1.tileSize, "Water", "Back") != null || location.doesTileHaveProperty(x / Game1.tileSize, y / Game1.tileSize, "WaterSource", "Back") != null || location is BuildableGameLocation && (location as BuildableGameLocation).getBuildingAt(source.First<Vector2>()) != null && ((location as BuildableGameLocation).getBuildingAt(source.First<Vector2>()).buildingType.Equals("Well") && (location as BuildableGameLocation).getBuildingAt(source.First<Vector2>()).daysOfConstructionLeft <= 0))
      {
        who.jitterStrength = 0.5f;
        switch (this.upgradeLevel)
        {
          case 0:
            this.waterCanMax = 40;
            break;
          case 1:
            this.waterCanMax = 55;
            break;
          case 2:
            this.waterCanMax = 70;
            break;
          case 3:
            this.waterCanMax = 85;
            break;
          case 4:
            this.waterCanMax = 100;
            break;
        }
        this.waterLeft = this.waterCanMax;
        Game1.playSound("slosh");
        DelayedAction.playSoundAfterDelay("glug", 250);
      }
      else if (this.waterLeft > 0)
      {
        who.Stamina = who.Stamina - ((float) (2 * (power + 1)) - (float) who.FarmingLevel * 0.1f);
        int num = 0;
        foreach (Vector2 index in source)
        {
          if (location.terrainFeatures.ContainsKey(index))
            location.terrainFeatures[index].performToolAction((Tool) this, 0, index, (GameLocation) null);
          if (location.objects.ContainsKey(index))
            location.Objects[index].performToolAction((Tool) this);
          location.performToolAction((Tool) this, (int) index.X, (int) index.Y);
          location.temporarySprites.Add(new TemporaryAnimatedSprite(13, new Vector2(index.X * (float) Game1.tileSize, index.Y * (float) Game1.tileSize), Color.White, 10, Game1.random.NextDouble() < 0.5, 70f, 0, Game1.tileSize, (float) (((double) index.Y * (double) Game1.tileSize + (double) (Game1.tileSize / 2)) / 10000.0 - 0.00999999977648258), -1, 0)
          {
            delayBeforeAnimationStart = 200 + num * 10
          });
          ++num;
        }
        this.waterLeft = this.waterLeft - (power + 1);
        Vector2 vector2 = new Vector2(who.position.X - (float) (Game1.tileSize / 2) - (float) Game1.pixelZoom, who.position.Y - (float) (Game1.tileSize / 4) - (float) Game1.pixelZoom);
        switch (who.facingDirection)
        {
          case 0:
            vector2 = Vector2.Zero;
            break;
          case 1:
            vector2.X += (float) (Game1.tileSize * 2 + Game1.pixelZoom * 2);
            break;
          case 2:
            vector2.X += (float) (Game1.tileSize + Game1.pixelZoom * 2);
            vector2.Y += (float) (Game1.tileSize / 2 + Game1.pixelZoom * 3);
            break;
        }
        if (vector2.Equals(Vector2.Zero))
          return;
        for (int index = 0; index < 30; ++index)
          location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.staminaRect, new Rectangle(0, 0, 1, 1), 999f, 1, 999, vector2 + new Vector2((float) (Game1.random.Next(-3, 0) * Game1.pixelZoom), (float) (Game1.random.Next(2) * Game1.pixelZoom)), false, false, (float) (who.GetBoundingBox().Bottom + Game1.tileSize / 2) / 10000f, 0.04f, Game1.random.NextDouble() < 0.5 ? Color.DeepSkyBlue : Color.LightBlue, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
          {
            delayBeforeAnimationStart = index * 15,
            motion = new Vector2((float) Game1.random.Next(-10, 11) / 100f, 0.5f),
            acceleration = new Vector2(0.0f, 0.1f)
          });
      }
      else
      {
        who.doEmote(4);
        Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:WateringCan.cs.14335"));
      }
    }
  }
}
