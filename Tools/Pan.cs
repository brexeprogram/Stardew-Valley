// Decompiled with JetBrains decompiler
// Type: StardewValley.Tools.Pan
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;
using System;
using System.Collections.Generic;

namespace StardewValley.Tools
{
  public class Pan : Tool
  {
    public Pan()
      : base("Copper Pan", -1, 12, 12, false, 0)
    {
    }

    protected override string loadDisplayName()
    {
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:Pan.cs.14180");
    }

    protected override string loadDescription()
    {
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:Pan.cs.14181");
    }

    public override bool beginUsing(GameLocation location, int x, int y, Farmer who)
    {
      this.currentParentTileIndex = 12;
      this.indexOfMenuItemView = 12;
      bool flag = false;
      Rectangle rectangle = new Rectangle(location.orePanPoint.X * Game1.tileSize - Game1.tileSize, location.orePanPoint.Y * Game1.tileSize - Game1.tileSize, Game1.tileSize * 4, Game1.tileSize * 4);
      if (rectangle.Contains(x, y) && (double) Utility.distance((float) who.getStandingX(), (float) rectangle.Center.X, (float) who.getStandingY(), (float) rectangle.Center.Y) <= (double) (3 * Game1.tileSize))
        flag = true;
      who.lastClick = Vector2.Zero;
      x = (int) who.GetToolLocation(false).X;
      y = (int) who.GetToolLocation(false).Y;
      who.lastClick = new Vector2((float) x, (float) y);
      Point orePanPoint = location.orePanPoint;
      if (!location.orePanPoint.Equals(Point.Zero))
      {
        Rectangle boundingBox = who.GetBoundingBox();
        if (flag || boundingBox.Intersects(rectangle))
        {
          who.faceDirection(2);
          who.FarmerSprite.animateOnce(303, 50f, 4);
          return true;
        }
      }
      who.forceCanMove();
      return true;
    }

    public static void playSlosh(Farmer who)
    {
      Game1.playSound("slosh");
    }

    public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
    {
      base.DoFunction(location, x, y, power, who);
      x = (int) who.GetToolLocation(false).X;
      y = (int) who.GetToolLocation(false).Y;
      this.currentParentTileIndex = 12;
      this.indexOfMenuItemView = 12;
      Game1.playSound("coin");
      who.addItemsByMenuIfNecessary(this.getPanItems(location, who), (ItemGrabMenu.behaviorOnItemSelect) null);
      location.orePanPoint = Point.Zero;
      location.orePanAnimation = (TemporaryAnimatedSprite) null;
      who.CanMove = true;
      who.usingTool = false;
      who.canReleaseTool = true;
    }

    public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber)
    {
      this.indexOfMenuItemView = 12;
      base.drawInMenu(spriteBatch, location, scaleSize, transparency, layerDepth, drawStackNumber);
    }

    public List<Item> getPanItems(GameLocation location, Farmer who)
    {
      List<Item> objList = new List<Item>();
      int parentSheetIndex1 = 378;
      int parentSheetIndex2 = -1;
      Random random = new Random(location.orePanPoint.X + location.orePanPoint.Y * 1000 + (int) Game1.stats.DaysPlayed);
      double num1 = random.NextDouble() - (double) who.luckLevel * 0.001 - Game1.dailyLuck;
      if (num1 < 0.01)
        parentSheetIndex1 = 386;
      else if (num1 < 0.241)
        parentSheetIndex1 = 384;
      else if (num1 < 0.6)
        parentSheetIndex1 = 380;
      int initialStack1 = random.Next(5) + 1 + (int) ((random.NextDouble() + 0.1 + (double) who.luckLevel / 10.0 + Game1.dailyLuck) * 2.0);
      int initialStack2 = random.Next(5) + 1 + (int) ((random.NextDouble() + 0.1 + (double) who.luckLevel / 10.0) * 2.0);
      if (random.NextDouble() - Game1.dailyLuck < 0.4 + (double) who.LuckLevel * 0.04)
      {
        double num2 = random.NextDouble() - Game1.dailyLuck;
        parentSheetIndex2 = 382;
        if (num2 < 0.02 + (double) who.LuckLevel * 0.002)
        {
          parentSheetIndex2 = 72;
          initialStack2 = 1;
        }
        else if (num2 < 0.1)
        {
          parentSheetIndex2 = 60 + random.Next(5) * 2;
          initialStack2 = 1;
        }
        else if (num2 < 0.36)
        {
          parentSheetIndex2 = 749;
          initialStack2 = Math.Max(1, initialStack2 / 2);
        }
        else if (num2 < 0.5)
        {
          parentSheetIndex2 = random.NextDouble() < 0.3 ? 82 : (random.NextDouble() < 0.5 ? 84 : 86);
          initialStack2 = 1;
        }
      }
      objList.Add((Item) new StardewValley.Object(parentSheetIndex1, initialStack1, false, -1, 0));
      if (parentSheetIndex2 != -1)
        objList.Add((Item) new StardewValley.Object(parentSheetIndex2, initialStack2, false, -1, 0));
      return objList;
    }
  }
}
