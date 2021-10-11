// Decompiled with JetBrains decompiler
// Type: StardewValley.Events.WorldChangeEvent
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StardewValley.Events
{
  public class WorldChangeEvent : FarmEvent
  {
    private int soundInterval = 99999;
    public const int identifier = 942066;
    public const int jojaGreenhouse = 0;
    public const int junimoGreenHouse = 1;
    public const int jojaBoiler = 2;
    public const int junimoBoiler = 3;
    public const int jojaBridge = 4;
    public const int junimoBridge = 5;
    public const int jojaBus = 6;
    public const int junimoBus = 7;
    public const int jojaBoulder = 8;
    public const int junimoBoulder = 9;
    private int whichEvent;
    private int cutsceneLengthTimer;
    private int timerSinceFade;
    private int soundTimer;
    private GameLocation location;
    private string sound;
    private bool kill;
    private bool wasRaining;

    public WorldChangeEvent(int which)
    {
      this.whichEvent = which;
    }

    public bool setUp()
    {
      Game1.currentLightSources.Clear();
      this.location = (GameLocation) null;
      int sourceXTile = 0;
      int sourceYTile = 0;
      this.cutsceneLengthTimer = 8000;
      this.wasRaining = Game1.isRaining;
      Game1.isRaining = false;
      switch (this.whichEvent)
      {
        case 0:
          this.location = Game1.getLocationFromName("Farm");
          sourceXTile = 28;
          sourceYTile = 13;
          this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(288, 1349, 19, 28), 150f, 5, 999, new Vector2((float) (25 * Game1.tileSize + 2 * Game1.pixelZoom), (float) (12 * Game1.tileSize - Game1.tileSize / 2)), false, false)
          {
            scale = (float) Game1.pixelZoom,
            layerDepth = 0.0961f
          });
          this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(288, 1377, 19, 28), 140f, 5, 999, new Vector2((float) (31 * Game1.tileSize - 4 * Game1.pixelZoom), (float) (11 * Game1.tileSize)), false, false)
          {
            scale = (float) Game1.pixelZoom,
            layerDepth = 0.0961f
          });
          this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(390, 1405, 18, 32), 1000f, 2, 999, new Vector2((float) (28 * Game1.tileSize + 2 * Game1.pixelZoom), (float) (9 * Game1.tileSize)), false, false)
          {
            scale = (float) Game1.pixelZoom,
            layerDepth = 0.0961f
          });
          this.soundInterval = 560;
          Game1.currentLightSources.Add(new LightSource(4, new Vector2((float) sourceXTile, (float) sourceYTile) * (float) Game1.tileSize, 4f));
          this.sound = "axchop";
          break;
        case 1:
          this.location = Game1.getLocationFromName("Farm");
          sourceXTile = 28;
          sourceYTile = 13;
          Utility.addSprinklesToLocation(this.location, sourceXTile, 12, 7, 7, 15000, 150, Color.LightCyan, (string) null, false);
          Utility.addStarsAndSpirals(this.location, sourceXTile, 12, 7, 7, 15000, 150, Color.White, (string) null, false);
          this.sound = "junimoMeep1";
          Game1.currentLightSources.Add(new LightSource(4, new Vector2((float) sourceXTile, (float) sourceYTile) * (float) Game1.tileSize, 4f, Color.DarkGoldenrod));
          List<TemporaryAnimatedSprite> temporarySprites1 = this.location.temporarySprites;
          TemporaryAnimatedSprite temporaryAnimatedSprite1 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(294, 1432, 16, 16), 300f, 4, 999, new Vector2((float) (28 * Game1.tileSize), (float) (12 * Game1.tileSize - Game1.tileSize)), false, false);
          temporaryAnimatedSprite1.scale = (float) Game1.pixelZoom;
          temporaryAnimatedSprite1.layerDepth = 1f;
          int num1 = 1;
          temporaryAnimatedSprite1.xPeriodic = num1 != 0;
          double num2 = 2000.0;
          temporaryAnimatedSprite1.xPeriodicLoopTime = (float) num2;
          double num3 = (double) (Game1.tileSize / 4);
          temporaryAnimatedSprite1.xPeriodicRange = (float) num3;
          int num4 = 1;
          temporaryAnimatedSprite1.light = num4 != 0;
          Color darkGoldenrod1 = Color.DarkGoldenrod;
          temporaryAnimatedSprite1.lightcolor = darkGoldenrod1;
          double num5 = 1.0;
          temporaryAnimatedSprite1.lightRadius = (float) num5;
          temporarySprites1.Add(temporaryAnimatedSprite1);
          this.soundInterval = 800;
          break;
        case 2:
          this.location = Game1.getLocationFromName("Town");
          sourceXTile = 105;
          sourceYTile = 79;
          this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(288, 1377, 19, 28), 100f, 5, 999, new Vector2((float) (104 * Game1.tileSize), (float) (79 * Game1.tileSize - Game1.tileSize / 2)), false, false)
          {
            scale = (float) Game1.pixelZoom,
            layerDepth = 1f
          });
          this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(288, 1406, 22, 26), 700f, 2, 999, new Vector2((float) (108 * Game1.tileSize - 6 * Game1.pixelZoom), (float) (79 * Game1.tileSize - Game1.tileSize * 2 / 3)), false, false)
          {
            scale = (float) Game1.pixelZoom,
            layerDepth = 1f
          });
          this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(390, 1405, 18, 32), 1500f, 2, 999, new Vector2((float) (106 * Game1.tileSize + 2 * Game1.pixelZoom), (float) (76 * Game1.tileSize)), false, false)
          {
            scale = (float) Game1.pixelZoom,
            layerDepth = 1f
          });
          this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(335, 1410, 21, 21), 999f, 1, 9999, new Vector2((float) (108 * Game1.tileSize), (float) (80 * Game1.tileSize + Game1.tileSize / 4)), false, false)
          {
            scale = (float) Game1.pixelZoom,
            layerDepth = 1f
          });
          this.soundInterval = 500;
          Game1.currentLightSources.Add(new LightSource(4, new Vector2((float) sourceXTile, (float) sourceYTile) * (float) Game1.tileSize, 4f));
          this.sound = "clank";
          break;
        case 3:
          this.location = Game1.getLocationFromName("Town");
          sourceXTile = 105;
          sourceYTile = 79;
          Utility.addSprinklesToLocation(this.location, sourceXTile + 1, sourceYTile, 6, 4, 15000, 350, Color.LightCyan, (string) null, false);
          Utility.addStarsAndSpirals(this.location, sourceXTile + 1, sourceYTile, 6, 4, 15000, 350, Color.White, (string) null, false);
          List<TemporaryAnimatedSprite> temporarySprites2 = this.location.temporarySprites;
          TemporaryAnimatedSprite temporaryAnimatedSprite2 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(294, 1432, 16, 16), 300f, 4, 999, new Vector2((float) (104 * Game1.tileSize), (float) (80 * Game1.tileSize - Game1.tileSize)), false, false);
          temporaryAnimatedSprite2.scale = (float) Game1.pixelZoom;
          temporaryAnimatedSprite2.layerDepth = 1f;
          int num6 = 1;
          temporaryAnimatedSprite2.xPeriodic = num6 != 0;
          double num7 = 2000.0;
          temporaryAnimatedSprite2.xPeriodicLoopTime = (float) num7;
          double num8 = (double) (Game1.tileSize / 4);
          temporaryAnimatedSprite2.xPeriodicRange = (float) num8;
          int num9 = 1;
          temporaryAnimatedSprite2.light = num9 != 0;
          Color darkGoldenrod2 = Color.DarkGoldenrod;
          temporaryAnimatedSprite2.lightcolor = darkGoldenrod2;
          double num10 = 1.0;
          temporaryAnimatedSprite2.lightRadius = (float) num10;
          temporarySprites2.Add(temporaryAnimatedSprite2);
          List<TemporaryAnimatedSprite> temporarySprites3 = this.location.temporarySprites;
          TemporaryAnimatedSprite temporaryAnimatedSprite3 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(294, 1432, 16, 16), 300f, 4, 999, new Vector2((float) (108 * Game1.tileSize), (float) (80 * Game1.tileSize - Game1.tileSize)), false, false);
          temporaryAnimatedSprite3.scale = (float) Game1.pixelZoom;
          temporaryAnimatedSprite3.layerDepth = 1f;
          int num11 = 1;
          temporaryAnimatedSprite3.xPeriodic = num11 != 0;
          double num12 = 2300.0;
          temporaryAnimatedSprite3.xPeriodicLoopTime = (float) num12;
          double num13 = (double) (Game1.tileSize / 4);
          temporaryAnimatedSprite3.xPeriodicRange = (float) num13;
          Color hotPink = Color.HotPink;
          temporaryAnimatedSprite3.color = hotPink;
          int num14 = 1;
          temporaryAnimatedSprite3.light = num14 != 0;
          Color darkGoldenrod3 = Color.DarkGoldenrod;
          temporaryAnimatedSprite3.lightcolor = darkGoldenrod3;
          double num15 = 1.0;
          temporaryAnimatedSprite3.lightRadius = (float) num15;
          temporarySprites3.Add(temporaryAnimatedSprite3);
          this.sound = "junimoMeep1";
          Game1.currentLightSources.Add(new LightSource(4, new Vector2((float) sourceXTile, (float) sourceYTile) * (float) Game1.tileSize, 4f, Color.DarkGoldenrod));
          this.soundInterval = 800;
          break;
        case 4:
          this.location = Game1.getLocationFromName("Mountain");
          sourceXTile = 95;
          sourceYTile = 27;
          this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(383, 1378, 28, 27), 400f, 2, 999, new Vector2((float) (86 * Game1.tileSize), (float) (26 * Game1.tileSize - Game1.tileSize / 2)), false, false)
          {
            scale = (float) Game1.pixelZoom,
            layerDepth = 1f,
            motion = new Vector2(0.5f, 0.0f)
          });
          this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(288, 1406, 22, 26), 350f, 2, 999, new Vector2((float) (98 * Game1.tileSize), (float) (26 * Game1.tileSize - Game1.tileSize / 2)), false, false)
          {
            scale = (float) Game1.pixelZoom,
            layerDepth = 1f
          });
          this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(358, 1415, 31, 20), 999f, 1, 9999, new Vector2((float) (92 * Game1.tileSize), (float) (26 * Game1.tileSize - Game1.tileSize / 4)), false, false)
          {
            scale = (float) Game1.pixelZoom,
            layerDepth = 1f
          });
          this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(335, 1410, 21, 21), 999f, 1, 9999, new Vector2((float) (100 * Game1.tileSize), (float) (26 * Game1.tileSize - Game1.tileSize / 4)), false, false)
          {
            scale = (float) Game1.pixelZoom,
            layerDepth = 1f
          });
          this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(390, 1405, 18, 32), 1500f, 2, 999, new Vector2((float) (91 * Game1.tileSize), (float) (25 * Game1.tileSize - Game1.tileSize / 4)), false, false)
          {
            scale = (float) Game1.pixelZoom,
            layerDepth = 0.8f
          });
          this.soundInterval = 700;
          Game1.currentLightSources.Add(new LightSource(4, new Vector2((float) sourceXTile, (float) sourceYTile) * (float) Game1.tileSize, 4f));
          this.sound = "axchop";
          break;
        case 5:
          this.location = Game1.getLocationFromName("Mountain");
          sourceXTile = 95;
          sourceYTile = 27;
          Utility.addSprinklesToLocation(this.location, sourceXTile, sourceYTile, 7, 4, 15000, 150, Color.LightCyan, (string) null, false);
          Utility.addStarsAndSpirals(this.location, sourceXTile + 1, sourceYTile, 7, 4, 15000, 350, Color.White, (string) null, false);
          List<TemporaryAnimatedSprite> temporarySprites4 = this.location.temporarySprites;
          TemporaryAnimatedSprite temporaryAnimatedSprite4 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(294, 1432, 16, 16), 300f, 4, 999, new Vector2((float) (91 * Game1.tileSize), (float) (26 * Game1.tileSize - Game1.tileSize / 4)), false, false);
          temporaryAnimatedSprite4.scale = (float) Game1.pixelZoom;
          temporaryAnimatedSprite4.layerDepth = 1f;
          int num16 = 1;
          temporaryAnimatedSprite4.xPeriodic = num16 != 0;
          double num17 = 2000.0;
          temporaryAnimatedSprite4.xPeriodicLoopTime = (float) num17;
          double num18 = (double) (Game1.tileSize / 4);
          temporaryAnimatedSprite4.xPeriodicRange = (float) num18;
          int num19 = 1;
          temporaryAnimatedSprite4.light = num19 != 0;
          Color darkGoldenrod4 = Color.DarkGoldenrod;
          temporaryAnimatedSprite4.lightcolor = darkGoldenrod4;
          double num20 = 1.0;
          temporaryAnimatedSprite4.lightRadius = (float) num20;
          temporarySprites4.Add(temporaryAnimatedSprite4);
          List<TemporaryAnimatedSprite> temporarySprites5 = this.location.temporarySprites;
          TemporaryAnimatedSprite temporaryAnimatedSprite5 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(294, 1432, 16, 16), 300f, 4, 999, new Vector2((float) (99 * Game1.tileSize), (float) (26 * Game1.tileSize - Game1.tileSize / 4)), false, false);
          temporaryAnimatedSprite5.scale = (float) Game1.pixelZoom;
          temporaryAnimatedSprite5.layerDepth = 1f;
          int num21 = 1;
          temporaryAnimatedSprite5.xPeriodic = num21 != 0;
          double num22 = 2300.0;
          temporaryAnimatedSprite5.xPeriodicLoopTime = (float) num22;
          double num23 = (double) (Game1.tileSize / 4);
          temporaryAnimatedSprite5.xPeriodicRange = (float) num23;
          Color yellow1 = Color.Yellow;
          temporaryAnimatedSprite5.color = yellow1;
          int num24 = 1;
          temporaryAnimatedSprite5.light = num24 != 0;
          Color darkGoldenrod5 = Color.DarkGoldenrod;
          temporaryAnimatedSprite5.lightcolor = darkGoldenrod5;
          double num25 = 1.0;
          temporaryAnimatedSprite5.lightRadius = (float) num25;
          temporarySprites5.Add(temporaryAnimatedSprite5);
          this.sound = "junimoMeep1";
          Game1.currentLightSources.Add(new LightSource(4, new Vector2((float) sourceXTile, (float) sourceYTile) * (float) Game1.tileSize, 4f, Color.DarkGoldenrod));
          this.soundInterval = 800;
          break;
        case 6:
          this.location = Game1.getLocationFromName("BusStop");
          sourceXTile = 14;
          sourceYTile = 8;
          this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(288, 1349, 19, 28), 150f, 5, 999, new Vector2((float) (19 * Game1.tileSize), (float) (8 * Game1.tileSize - Game1.tileSize / 2)), false, false)
          {
            scale = (float) Game1.pixelZoom,
            layerDepth = 1f
          });
          this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(288, 1377, 19, 28), 140f, 5, 999, new Vector2((float) (10 * Game1.tileSize), (float) (8 * Game1.tileSize)), false, false)
          {
            scale = (float) Game1.pixelZoom,
            layerDepth = 1f
          });
          this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(390, 1405, 18, 32), 1500f, 2, 999, new Vector2((float) (14 * Game1.tileSize + 2 * Game1.pixelZoom), (float) (3 * Game1.tileSize)), false, false)
          {
            scale = (float) Game1.pixelZoom,
            layerDepth = 1f
          });
          this.soundInterval = 560;
          Game1.currentLightSources.Add(new LightSource(4, new Vector2((float) sourceXTile, (float) sourceYTile) * (float) Game1.tileSize, 4f));
          this.sound = "clank";
          break;
        case 7:
          this.location = Game1.getLocationFromName("BusStop");
          sourceXTile = 14;
          sourceYTile = 8;
          Utility.addSprinklesToLocation(this.location, sourceXTile, sourceYTile, 9, 4, 10000, 200, Color.LightCyan, (string) null, true);
          Utility.addStarsAndSpirals(this.location, sourceXTile, sourceYTile, 9, 4, 15000, 150, Color.White, (string) null, false);
          this.sound = "junimoMeep1";
          List<TemporaryAnimatedSprite> temporarySprites6 = this.location.temporarySprites;
          TemporaryAnimatedSprite temporaryAnimatedSprite6 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(294, 1432, 16, 16), 300f, 4, 999, new Vector2((float) (10 * Game1.tileSize), (float) (11 * Game1.tileSize - Game1.tileSize)), false, false);
          temporaryAnimatedSprite6.scale = (float) Game1.pixelZoom;
          temporaryAnimatedSprite6.layerDepth = 1f;
          int num26 = 1;
          temporaryAnimatedSprite6.xPeriodic = num26 != 0;
          double num27 = 2000.0;
          temporaryAnimatedSprite6.xPeriodicLoopTime = (float) num27;
          double num28 = (double) (Game1.tileSize / 4);
          temporaryAnimatedSprite6.xPeriodicRange = (float) num28;
          int num29 = 1;
          temporaryAnimatedSprite6.light = num29 != 0;
          Color darkGoldenrod6 = Color.DarkGoldenrod;
          temporaryAnimatedSprite6.lightcolor = darkGoldenrod6;
          double num30 = 1.0;
          temporaryAnimatedSprite6.lightRadius = (float) num30;
          temporarySprites6.Add(temporaryAnimatedSprite6);
          List<TemporaryAnimatedSprite> temporarySprites7 = this.location.temporarySprites;
          TemporaryAnimatedSprite temporaryAnimatedSprite7 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(294, 1432, 16, 16), 300f, 4, 999, new Vector2((float) (12 * Game1.tileSize), (float) (11 * Game1.tileSize - Game1.tileSize)), false, false);
          temporaryAnimatedSprite7.scale = (float) Game1.pixelZoom;
          temporaryAnimatedSprite7.layerDepth = 1f;
          int num31 = 1;
          temporaryAnimatedSprite7.xPeriodic = num31 != 0;
          double num32 = 2300.0;
          temporaryAnimatedSprite7.xPeriodicLoopTime = (float) num32;
          double num33 = (double) (Game1.tileSize / 4);
          temporaryAnimatedSprite7.xPeriodicRange = (float) num33;
          Color pink = Color.Pink;
          temporaryAnimatedSprite7.color = pink;
          int num34 = 1;
          temporaryAnimatedSprite7.light = num34 != 0;
          Color darkGoldenrod7 = Color.DarkGoldenrod;
          temporaryAnimatedSprite7.lightcolor = darkGoldenrod7;
          double num35 = 1.0;
          temporaryAnimatedSprite7.lightRadius = (float) num35;
          temporarySprites7.Add(temporaryAnimatedSprite7);
          List<TemporaryAnimatedSprite> temporarySprites8 = this.location.temporarySprites;
          TemporaryAnimatedSprite temporaryAnimatedSprite8 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(294, 1432, 16, 16), 300f, 4, 999, new Vector2((float) (14 * Game1.tileSize), (float) (11 * Game1.tileSize - Game1.tileSize)), false, false);
          temporaryAnimatedSprite8.scale = (float) Game1.pixelZoom;
          temporaryAnimatedSprite8.layerDepth = 1f;
          int num36 = 1;
          temporaryAnimatedSprite8.xPeriodic = num36 != 0;
          double num37 = 2200.0;
          temporaryAnimatedSprite8.xPeriodicLoopTime = (float) num37;
          double num38 = (double) (Game1.tileSize / 4);
          temporaryAnimatedSprite8.xPeriodicRange = (float) num38;
          Color yellow2 = Color.Yellow;
          temporaryAnimatedSprite8.color = yellow2;
          int num39 = 1;
          temporaryAnimatedSprite8.light = num39 != 0;
          Color darkGoldenrod8 = Color.DarkGoldenrod;
          temporaryAnimatedSprite8.lightcolor = darkGoldenrod8;
          double num40 = 1.0;
          temporaryAnimatedSprite8.lightRadius = (float) num40;
          temporarySprites8.Add(temporaryAnimatedSprite8);
          List<TemporaryAnimatedSprite> temporarySprites9 = this.location.temporarySprites;
          TemporaryAnimatedSprite temporaryAnimatedSprite9 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(294, 1432, 16, 16), 300f, 4, 999, new Vector2((float) (16 * Game1.tileSize), (float) (11 * Game1.tileSize - Game1.tileSize)), false, false);
          temporaryAnimatedSprite9.scale = (float) Game1.pixelZoom;
          temporaryAnimatedSprite9.layerDepth = 1f;
          int num41 = 1;
          temporaryAnimatedSprite9.xPeriodic = num41 != 0;
          double num42 = 2100.0;
          temporaryAnimatedSprite9.xPeriodicLoopTime = (float) num42;
          double num43 = (double) (Game1.tileSize / 4);
          temporaryAnimatedSprite9.xPeriodicRange = (float) num43;
          Color lightBlue = Color.LightBlue;
          temporaryAnimatedSprite9.color = lightBlue;
          int num44 = 1;
          temporaryAnimatedSprite9.light = num44 != 0;
          Color darkGoldenrod9 = Color.DarkGoldenrod;
          temporaryAnimatedSprite9.lightcolor = darkGoldenrod9;
          double num45 = 1.0;
          temporaryAnimatedSprite9.lightRadius = (float) num45;
          temporarySprites9.Add(temporaryAnimatedSprite9);
          Game1.currentLightSources.Add(new LightSource(4, new Vector2((float) sourceXTile, (float) sourceYTile) * (float) Game1.tileSize, 4f, Color.DarkGoldenrod));
          this.soundInterval = 500;
          break;
        case 8:
          this.location = Game1.getLocationFromName("Mountain");
          this.location.resetForPlayerEntry();
          sourceXTile = 48;
          sourceYTile = 5;
          this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(288, 1377, 19, 28), 100f, 5, 999, new Vector2((float) (45 * Game1.tileSize), (float) (5 * Game1.tileSize - Game1.tileSize / 2)), false, false)
          {
            scale = (float) Game1.pixelZoom,
            layerDepth = 1f
          });
          List<TemporaryAnimatedSprite> temporarySprites10 = this.location.temporarySprites;
          TemporaryAnimatedSprite temporaryAnimatedSprite10 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(387, 1340, 17, 37), 50f, 2, 99999, new Vector2((float) (47 * Game1.tileSize + Game1.tileSize / 2), (float) (3 * Game1.tileSize - Game1.tileSize / 2)), false, false);
          temporaryAnimatedSprite10.scale = (float) Game1.pixelZoom;
          temporaryAnimatedSprite10.layerDepth = 1f;
          int num46 = 1;
          temporaryAnimatedSprite10.yPeriodic = num46 != 0;
          double num47 = 100.0;
          temporaryAnimatedSprite10.yPeriodicLoopTime = (float) num47;
          double num48 = (double) (Game1.pixelZoom / 2);
          temporaryAnimatedSprite10.yPeriodicRange = (float) num48;
          temporarySprites10.Add(temporaryAnimatedSprite10);
          this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(335, 1410, 21, 21), 999f, 1, 9999, new Vector2((float) (44 * Game1.tileSize), (float) (6 * Game1.tileSize - Game1.tileSize / 4)), false, false)
          {
            scale = (float) Game1.pixelZoom,
            layerDepth = 1f
          });
          this.location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(390, 1405, 18, 32), 1500f, 2, 999, new Vector2((float) (50 * Game1.tileSize), (float) (6 * Game1.tileSize - Game1.tileSize / 4)), false, false)
          {
            scale = (float) Game1.pixelZoom,
            layerDepth = 1f
          });
          this.soundInterval = 100;
          Game1.currentLightSources.Add(new LightSource(4, new Vector2((float) sourceXTile, (float) sourceYTile) * (float) Game1.tileSize, 4f));
          this.sound = "thudStep";
          break;
        case 9:
          this.location = Game1.getLocationFromName("Mountain");
          this.location.resetForPlayerEntry();
          sourceXTile = 48;
          sourceYTile = 5;
          Utility.addSprinklesToLocation(this.location, sourceXTile, sourceYTile, 4, 4, 15000, 350, Color.LightCyan, (string) null, false);
          Utility.addStarsAndSpirals(this.location, sourceXTile + 1, sourceYTile, 4, 4, 15000, 550, Color.White, (string) null, false);
          List<TemporaryAnimatedSprite> temporarySprites11 = this.location.temporarySprites;
          TemporaryAnimatedSprite temporaryAnimatedSprite11 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(294, 1432, 16, 16), 300f, 4, 999, new Vector2((float) (45 * Game1.tileSize), (float) (6 * Game1.tileSize - Game1.tileSize / 4)), false, false);
          temporaryAnimatedSprite11.scale = (float) Game1.pixelZoom;
          temporaryAnimatedSprite11.layerDepth = 1f;
          int num49 = 1;
          temporaryAnimatedSprite11.xPeriodic = num49 != 0;
          double num50 = 2000.0;
          temporaryAnimatedSprite11.xPeriodicLoopTime = (float) num50;
          double num51 = (double) (Game1.tileSize / 4);
          temporaryAnimatedSprite11.xPeriodicRange = (float) num51;
          int num52 = 1;
          temporaryAnimatedSprite11.light = num52 != 0;
          Color darkGoldenrod10 = Color.DarkGoldenrod;
          temporaryAnimatedSprite11.lightcolor = darkGoldenrod10;
          double num53 = 1.0;
          temporaryAnimatedSprite11.lightRadius = (float) num53;
          temporarySprites11.Add(temporaryAnimatedSprite11);
          List<TemporaryAnimatedSprite> temporarySprites12 = this.location.temporarySprites;
          TemporaryAnimatedSprite temporaryAnimatedSprite12 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(294, 1432, 16, 16), 300f, 4, 999, new Vector2((float) (50 * Game1.tileSize), (float) (6 * Game1.tileSize - Game1.tileSize / 4)), false, false);
          temporaryAnimatedSprite12.scale = (float) Game1.pixelZoom;
          temporaryAnimatedSprite12.layerDepth = 1f;
          int num54 = 1;
          temporaryAnimatedSprite12.xPeriodic = num54 != 0;
          double num55 = 2300.0;
          temporaryAnimatedSprite12.xPeriodicLoopTime = (float) num55;
          double num56 = (double) (Game1.tileSize / 4);
          temporaryAnimatedSprite12.xPeriodicRange = (float) num56;
          Color yellow3 = Color.Yellow;
          temporaryAnimatedSprite12.color = yellow3;
          int num57 = 1;
          temporaryAnimatedSprite12.light = num57 != 0;
          Color darkGoldenrod11 = Color.DarkGoldenrod;
          temporaryAnimatedSprite12.lightcolor = darkGoldenrod11;
          double num58 = 1.0;
          temporaryAnimatedSprite12.lightRadius = (float) num58;
          temporarySprites12.Add(temporaryAnimatedSprite12);
          this.sound = "junimoMeep1";
          Game1.currentLightSources.Add(new LightSource(4, new Vector2((float) sourceXTile, (float) sourceYTile) * (float) Game1.tileSize, 1f));
          Game1.currentLightSources.Add(new LightSource(4, new Vector2((float) sourceXTile, (float) sourceYTile) * (float) Game1.tileSize, 1f, Color.DarkCyan));
          Game1.currentLightSources.Add(new LightSource(4, new Vector2((float) sourceXTile, (float) sourceYTile) * (float) Game1.tileSize, 4f, Color.DarkGoldenrod));
          this.soundInterval = 1000;
          break;
      }
      this.soundTimer = this.soundInterval;
      Game1.currentLocation = this.location;
      Game1.fadeClear();
      Game1.nonWarpFade = true;
      Game1.timeOfDay = 2400;
      Game1.displayHUD = false;
      Game1.viewportFreeze = true;
      Game1.player.position.X = -999999f;
      Game1.viewport.X = Math.Max(0, Math.Min(this.location.map.DisplayWidth - Game1.viewport.Width, sourceXTile * Game1.tileSize - Game1.viewport.Width / 2));
      Game1.viewport.Y = Math.Max(0, Math.Min(this.location.map.DisplayHeight - Game1.viewport.Height, sourceYTile * Game1.tileSize - Game1.viewport.Height / 2));
      Game1.changeMusicTrack("nightTime");
      return false;
    }

    public bool tickUpdate(GameTime time)
    {
      Game1.UpdateGameClock(time);
      this.cutsceneLengthTimer = this.cutsceneLengthTimer - time.ElapsedGameTime.Milliseconds;
      if (this.timerSinceFade > 0)
      {
        this.timerSinceFade = this.timerSinceFade - time.ElapsedGameTime.Milliseconds;
        Game1.globalFade = true;
        Game1.fadeToBlackAlpha = 1f;
        return this.timerSinceFade <= 0;
      }
      if (this.cutsceneLengthTimer <= 0 && !Game1.globalFade)
        Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.endEvent), 0.01f);
      this.soundTimer = this.soundTimer - time.ElapsedGameTime.Milliseconds;
      if (this.soundTimer <= 0 && this.sound != null)
      {
        Game1.playSound(this.sound);
        this.soundTimer = this.soundInterval;
      }
      return false;
    }

    public void endEvent()
    {
      Game1.changeMusicTrack("none");
      this.timerSinceFade = 1500;
      Game1.isRaining = this.wasRaining;
      Game1.getFarm().temporarySprites.Clear();
    }

    public void draw(SpriteBatch b)
    {
    }

    public void makeChangesToLocation()
    {
    }

    public void drawAboveEverything(SpriteBatch b)
    {
    }
  }
}
