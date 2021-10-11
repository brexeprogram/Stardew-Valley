// Decompiled with JetBrains decompiler
// Type: StardewValley.Locations.FarmCave
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using xTile;

namespace StardewValley.Locations
{
  public class FarmCave : GameLocation
  {
    public FarmCave()
    {
    }

    public FarmCave(Map map, string name)
      : base(map, name)
    {
    }

    public override void resetForPlayerEntry()
    {
      base.resetForPlayerEntry();
      if (Game1.player.caveChoice != 1)
        return;
      List<TemporaryAnimatedSprite> temporarySprites1 = this.temporarySprites;
      TemporaryAnimatedSprite temporaryAnimatedSprite1 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(374, 358, 1, 1), new Vector2(0.0f, 0.0f), false, 0.0f, Color.White);
      temporaryAnimatedSprite1.interval = 3000f;
      temporaryAnimatedSprite1.animationLength = 3;
      temporaryAnimatedSprite1.totalNumberOfLoops = 99999;
      double pixelZoom1 = (double) Game1.pixelZoom;
      temporaryAnimatedSprite1.scale = (float) pixelZoom1;
      double num1 = 1.0;
      temporaryAnimatedSprite1.layerDepth = (float) num1;
      int num2 = 1;
      temporaryAnimatedSprite1.light = num2 != 0;
      double num3 = 0.5;
      temporaryAnimatedSprite1.lightRadius = (float) num3;
      temporarySprites1.Add(temporaryAnimatedSprite1);
      List<TemporaryAnimatedSprite> temporarySprites2 = this.temporarySprites;
      TemporaryAnimatedSprite temporaryAnimatedSprite2 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(374, 358, 1, 1), new Vector2((float) (Game1.pixelZoom * 2), 0.0f), false, 0.0f, Color.White);
      temporaryAnimatedSprite2.interval = 3000f;
      temporaryAnimatedSprite2.animationLength = 3;
      temporaryAnimatedSprite2.totalNumberOfLoops = 99999;
      double pixelZoom2 = (double) Game1.pixelZoom;
      temporaryAnimatedSprite2.scale = (float) pixelZoom2;
      double num4 = 1.0;
      temporaryAnimatedSprite2.layerDepth = (float) num4;
      temporarySprites2.Add(temporaryAnimatedSprite2);
      List<TemporaryAnimatedSprite> temporarySprites3 = this.temporarySprites;
      TemporaryAnimatedSprite temporaryAnimatedSprite3 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(374, 358, 1, 1), new Vector2((float) (Game1.tileSize * 5), (float) -Game1.tileSize), false, 0.0f, Color.White);
      temporaryAnimatedSprite3.interval = 2000f;
      temporaryAnimatedSprite3.animationLength = 3;
      temporaryAnimatedSprite3.totalNumberOfLoops = 99999;
      double pixelZoom3 = (double) Game1.pixelZoom;
      temporaryAnimatedSprite3.scale = (float) pixelZoom3;
      int num5 = 500;
      temporaryAnimatedSprite3.delayBeforeAnimationStart = num5;
      double num6 = 1.0;
      temporaryAnimatedSprite3.layerDepth = (float) num6;
      int num7 = 1;
      temporaryAnimatedSprite3.light = num7 != 0;
      double num8 = 0.5;
      temporaryAnimatedSprite3.lightRadius = (float) num8;
      temporarySprites3.Add(temporaryAnimatedSprite3);
      List<TemporaryAnimatedSprite> temporarySprites4 = this.temporarySprites;
      TemporaryAnimatedSprite temporaryAnimatedSprite4 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(374, 358, 1, 1), new Vector2((float) (Game1.tileSize * 5 + Game1.pixelZoom * 2), (float) -Game1.tileSize), false, 0.0f, Color.White);
      temporaryAnimatedSprite4.interval = 2000f;
      temporaryAnimatedSprite4.animationLength = 3;
      temporaryAnimatedSprite4.totalNumberOfLoops = 99999;
      double pixelZoom4 = (double) Game1.pixelZoom;
      temporaryAnimatedSprite4.scale = (float) pixelZoom4;
      int num9 = 500;
      temporaryAnimatedSprite4.delayBeforeAnimationStart = num9;
      double num10 = 1.0;
      temporaryAnimatedSprite4.layerDepth = (float) num10;
      temporarySprites4.Add(temporaryAnimatedSprite4);
      List<TemporaryAnimatedSprite> temporarySprites5 = this.temporarySprites;
      TemporaryAnimatedSprite temporaryAnimatedSprite5 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(374, 358, 1, 1), new Vector2((float) (Game1.tileSize * 2), (float) (this.map.Layers[0].LayerHeight * Game1.tileSize - Game1.tileSize)), false, 0.0f, Color.White);
      temporaryAnimatedSprite5.interval = 1600f;
      temporaryAnimatedSprite5.animationLength = 3;
      temporaryAnimatedSprite5.totalNumberOfLoops = 99999;
      double pixelZoom5 = (double) Game1.pixelZoom;
      temporaryAnimatedSprite5.scale = (float) pixelZoom5;
      int num11 = 250;
      temporaryAnimatedSprite5.delayBeforeAnimationStart = num11;
      double num12 = 1.0;
      temporaryAnimatedSprite5.layerDepth = (float) num12;
      int num13 = 1;
      temporaryAnimatedSprite5.light = num13 != 0;
      double num14 = 0.5;
      temporaryAnimatedSprite5.lightRadius = (float) num14;
      temporarySprites5.Add(temporaryAnimatedSprite5);
      List<TemporaryAnimatedSprite> temporarySprites6 = this.temporarySprites;
      TemporaryAnimatedSprite temporaryAnimatedSprite6 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(374, 358, 1, 1), new Vector2((float) (Game1.tileSize * 2 + Game1.pixelZoom * 2), (float) (this.map.Layers[0].LayerHeight * Game1.tileSize - Game1.tileSize)), false, 0.0f, Color.White);
      temporaryAnimatedSprite6.interval = 1600f;
      temporaryAnimatedSprite6.animationLength = 3;
      temporaryAnimatedSprite6.totalNumberOfLoops = 99999;
      double pixelZoom6 = (double) Game1.pixelZoom;
      temporaryAnimatedSprite6.scale = (float) pixelZoom6;
      int num15 = 250;
      temporaryAnimatedSprite6.delayBeforeAnimationStart = num15;
      double num16 = 1.0;
      temporaryAnimatedSprite6.layerDepth = (float) num16;
      temporarySprites6.Add(temporaryAnimatedSprite6);
      List<TemporaryAnimatedSprite> temporarySprites7 = this.temporarySprites;
      TemporaryAnimatedSprite temporaryAnimatedSprite7 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(374, 358, 1, 1), new Vector2((float) ((this.map.Layers[0].LayerWidth + 1) * Game1.tileSize + Game1.pixelZoom), (float) (Game1.tileSize * 3)), false, 0.0f, Color.White);
      temporaryAnimatedSprite7.interval = 2800f;
      temporaryAnimatedSprite7.animationLength = 3;
      temporaryAnimatedSprite7.totalNumberOfLoops = 99999;
      double pixelZoom7 = (double) Game1.pixelZoom;
      temporaryAnimatedSprite7.scale = (float) pixelZoom7;
      int num17 = 750;
      temporaryAnimatedSprite7.delayBeforeAnimationStart = num17;
      double num18 = 1.0;
      temporaryAnimatedSprite7.layerDepth = (float) num18;
      int num19 = 1;
      temporaryAnimatedSprite7.light = num19 != 0;
      double num20 = 0.5;
      temporaryAnimatedSprite7.lightRadius = (float) num20;
      temporarySprites7.Add(temporaryAnimatedSprite7);
      List<TemporaryAnimatedSprite> temporarySprites8 = this.temporarySprites;
      TemporaryAnimatedSprite temporaryAnimatedSprite8 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(374, 358, 1, 1), new Vector2((float) ((this.map.Layers[0].LayerWidth + 1) * Game1.tileSize + Game1.pixelZoom * 3), (float) (Game1.tileSize * 3)), false, 0.0f, Color.White);
      temporaryAnimatedSprite8.interval = 2800f;
      temporaryAnimatedSprite8.animationLength = 3;
      temporaryAnimatedSprite8.totalNumberOfLoops = 99999;
      double pixelZoom8 = (double) Game1.pixelZoom;
      temporaryAnimatedSprite8.scale = (float) pixelZoom8;
      int num21 = 750;
      temporaryAnimatedSprite8.delayBeforeAnimationStart = num21;
      double num22 = 1.0;
      temporaryAnimatedSprite8.layerDepth = (float) num22;
      temporarySprites8.Add(temporaryAnimatedSprite8);
      List<TemporaryAnimatedSprite> temporarySprites9 = this.temporarySprites;
      TemporaryAnimatedSprite temporaryAnimatedSprite9 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(374, 358, 1, 1), new Vector2((float) ((this.map.Layers[0].LayerWidth + 1) * Game1.tileSize + Game1.pixelZoom), (float) (Game1.tileSize * 9)), false, 0.0f, Color.White);
      temporaryAnimatedSprite9.interval = 2200f;
      temporaryAnimatedSprite9.animationLength = 3;
      temporaryAnimatedSprite9.totalNumberOfLoops = 99999;
      double pixelZoom9 = (double) Game1.pixelZoom;
      temporaryAnimatedSprite9.scale = (float) pixelZoom9;
      int num23 = 750;
      temporaryAnimatedSprite9.delayBeforeAnimationStart = num23;
      double num24 = 1.0;
      temporaryAnimatedSprite9.layerDepth = (float) num24;
      int num25 = 1;
      temporaryAnimatedSprite9.light = num25 != 0;
      double num26 = 0.5;
      temporaryAnimatedSprite9.lightRadius = (float) num26;
      temporarySprites9.Add(temporaryAnimatedSprite9);
      List<TemporaryAnimatedSprite> temporarySprites10 = this.temporarySprites;
      TemporaryAnimatedSprite temporaryAnimatedSprite10 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(374, 358, 1, 1), new Vector2((float) ((this.map.Layers[0].LayerWidth + 1) * Game1.tileSize + Game1.pixelZoom * 3), (float) (Game1.tileSize * 9)), false, 0.0f, Color.White);
      temporaryAnimatedSprite10.interval = 2200f;
      temporaryAnimatedSprite10.animationLength = 3;
      temporaryAnimatedSprite10.totalNumberOfLoops = 99999;
      double pixelZoom10 = (double) Game1.pixelZoom;
      temporaryAnimatedSprite10.scale = (float) pixelZoom10;
      int num27 = 750;
      temporaryAnimatedSprite10.delayBeforeAnimationStart = num27;
      double num28 = 1.0;
      temporaryAnimatedSprite10.layerDepth = (float) num28;
      temporarySprites10.Add(temporaryAnimatedSprite10);
      List<TemporaryAnimatedSprite> temporarySprites11 = this.temporarySprites;
      TemporaryAnimatedSprite temporaryAnimatedSprite11 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(374, 358, 1, 1), new Vector2((float) (-Game1.tileSize + Game1.pixelZoom), (float) (Game1.tileSize * 2)), false, 0.0f, Color.White);
      temporaryAnimatedSprite11.interval = 2600f;
      temporaryAnimatedSprite11.animationLength = 3;
      temporaryAnimatedSprite11.totalNumberOfLoops = 99999;
      double pixelZoom11 = (double) Game1.pixelZoom;
      temporaryAnimatedSprite11.scale = (float) pixelZoom11;
      int num29 = 750;
      temporaryAnimatedSprite11.delayBeforeAnimationStart = num29;
      double num30 = 1.0;
      temporaryAnimatedSprite11.layerDepth = (float) num30;
      int num31 = 1;
      temporaryAnimatedSprite11.light = num31 != 0;
      double num32 = 0.5;
      temporaryAnimatedSprite11.lightRadius = (float) num32;
      temporarySprites11.Add(temporaryAnimatedSprite11);
      List<TemporaryAnimatedSprite> temporarySprites12 = this.temporarySprites;
      TemporaryAnimatedSprite temporaryAnimatedSprite12 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(374, 358, 1, 1), new Vector2((float) (-Game1.tileSize + Game1.pixelZoom * 3), (float) (Game1.tileSize * 2)), false, 0.0f, Color.White);
      temporaryAnimatedSprite12.interval = 2600f;
      temporaryAnimatedSprite12.animationLength = 3;
      temporaryAnimatedSprite12.totalNumberOfLoops = 99999;
      double pixelZoom12 = (double) Game1.pixelZoom;
      temporaryAnimatedSprite12.scale = (float) pixelZoom12;
      int num33 = 750;
      temporaryAnimatedSprite12.delayBeforeAnimationStart = num33;
      double num34 = 1.0;
      temporaryAnimatedSprite12.layerDepth = (float) num34;
      temporarySprites12.Add(temporaryAnimatedSprite12);
      List<TemporaryAnimatedSprite> temporarySprites13 = this.temporarySprites;
      TemporaryAnimatedSprite temporaryAnimatedSprite13 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(374, 358, 1, 1), new Vector2((float) -Game1.tileSize, (float) (Game1.tileSize * 6)), false, 0.0f, Color.White);
      temporaryAnimatedSprite13.interval = 3400f;
      temporaryAnimatedSprite13.animationLength = 3;
      temporaryAnimatedSprite13.totalNumberOfLoops = 99999;
      double pixelZoom13 = (double) Game1.pixelZoom;
      temporaryAnimatedSprite13.scale = (float) pixelZoom13;
      int num35 = 650;
      temporaryAnimatedSprite13.delayBeforeAnimationStart = num35;
      double num36 = 1.0;
      temporaryAnimatedSprite13.layerDepth = (float) num36;
      int num37 = 1;
      temporaryAnimatedSprite13.light = num37 != 0;
      double num38 = 0.5;
      temporaryAnimatedSprite13.lightRadius = (float) num38;
      temporarySprites13.Add(temporaryAnimatedSprite13);
      List<TemporaryAnimatedSprite> temporarySprites14 = this.temporarySprites;
      TemporaryAnimatedSprite temporaryAnimatedSprite14 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(374, 358, 1, 1), new Vector2((float) (-Game1.tileSize + Game1.pixelZoom * 3), (float) (Game1.tileSize * 6)), false, 0.0f, Color.White);
      temporaryAnimatedSprite14.interval = 3400f;
      temporaryAnimatedSprite14.animationLength = 3;
      temporaryAnimatedSprite14.totalNumberOfLoops = 99999;
      double pixelZoom14 = (double) Game1.pixelZoom;
      temporaryAnimatedSprite14.scale = (float) pixelZoom14;
      int num39 = 650;
      temporaryAnimatedSprite14.delayBeforeAnimationStart = num39;
      double num40 = 1.0;
      temporaryAnimatedSprite14.layerDepth = (float) num40;
      temporarySprites14.Add(temporaryAnimatedSprite14);
      Game1.ambientLight = new Color(70, 90, 0);
    }

    public override void UpdateWhenCurrentLocation(GameTime time)
    {
      base.UpdateWhenCurrentLocation(time);
      if (Game1.player.caveChoice == 1 && Game1.random.NextDouble() < 0.002)
      {
        List<TemporaryAnimatedSprite> temporarySprites = this.TemporarySprites;
        TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(640, 1664, 16, 16), 80f, 4, 9999, new Vector2((float) Game1.random.Next(this.map.Layers[0].LayerWidth), (float) this.map.Layers[0].LayerHeight) * (float) Game1.tileSize, false, false, 1f, 0.0f, Color.Black, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
        temporaryAnimatedSprite.xPeriodic = true;
        temporaryAnimatedSprite.xPeriodicLoopTime = 2000f;
        double tileSize = (double) Game1.tileSize;
        temporaryAnimatedSprite.xPeriodicRange = (float) tileSize;
        Vector2 vector2 = new Vector2(0.0f, (float) (-Game1.pixelZoom * 2));
        temporaryAnimatedSprite.motion = vector2;
        temporarySprites.Add(temporaryAnimatedSprite);
        if (Game1.random.NextDouble() < 0.15)
          Game1.playSound("batScreech");
        for (int index = 1; index < 5; ++index)
          DelayedAction.playSoundAfterDelay("batFlap", 320 * index - 80);
      }
      else
      {
        if (Game1.player.caveChoice != 1 || Game1.random.NextDouble() >= 0.005)
          return;
        this.temporarySprites.Add((TemporaryAnimatedSprite) new BatTemporarySprite(new Vector2(Game1.random.NextDouble() < 0.5 ? 0.0f : (float) (this.map.DisplayWidth - Game1.tileSize), (float) (this.map.DisplayHeight - Game1.tileSize))));
      }
    }

    public override void checkForMusic(GameTime time)
    {
    }

    public override void cleanupBeforePlayerExit()
    {
      base.cleanupBeforePlayerExit();
    }

    public override void DayUpdate(int dayOfMonth)
    {
      base.DayUpdate(dayOfMonth);
      if (Game1.player.caveChoice != 1)
        return;
      while (Game1.random.NextDouble() < 0.5)
      {
        int parentSheetIndex = 410;
        switch (Game1.random.Next(5))
        {
          case 0:
            parentSheetIndex = 296;
            break;
          case 1:
            parentSheetIndex = 396;
            break;
          case 2:
            parentSheetIndex = 406;
            break;
          case 3:
            parentSheetIndex = 410;
            break;
          case 4:
            if (Game1.random.NextDouble() < 0.75)
            {
              parentSheetIndex = Game1.random.NextDouble() < 0.1 ? 613 : Game1.random.Next(634, 639);
              break;
            }
            break;
        }
        Vector2 v = new Vector2((float) Game1.random.Next(1, this.map.Layers[0].LayerWidth - 1), (float) Game1.random.Next(1, this.map.Layers[0].LayerHeight - 4));
        if (this.isTileLocationTotallyClearAndPlaceable(v))
          this.setObject(v, new Object(parentSheetIndex, 1, false, -1, 0)
          {
            isSpawnedObject = true
          });
      }
    }

    public void setUpMushroomHouse()
    {
      this.setObject(new Vector2(4f, 5f), new Object(new Vector2(4f, 5f), 128, false));
      this.setObject(new Vector2(6f, 5f), new Object(new Vector2(6f, 5f), 128, false));
      this.setObject(new Vector2(8f, 5f), new Object(new Vector2(8f, 5f), 128, false));
      this.setObject(new Vector2(4f, 7f), new Object(new Vector2(4f, 7f), 128, false));
      this.setObject(new Vector2(6f, 7f), new Object(new Vector2(6f, 7f), 128, false));
      this.setObject(new Vector2(8f, 7f), new Object(new Vector2(8f, 7f), 128, false));
    }
  }
}
