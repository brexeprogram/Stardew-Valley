// Decompiled with JetBrains decompiler
// Type: StardewValley.Locations.WizardHouse
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using xTile;

namespace StardewValley.Locations
{
  public class WizardHouse : GameLocation
  {
    private int cauldronTimer = 250;

    public WizardHouse()
    {
    }

    public WizardHouse(Map m, string name)
      : base(m, name)
    {
    }

    public override void UpdateWhenCurrentLocation(GameTime time)
    {
      if (this.wasUpdated)
        return;
      base.UpdateWhenCurrentLocation(time);
      this.cauldronTimer = this.cauldronTimer - time.ElapsedGameTime.Milliseconds;
      if (this.cauldronTimer > 0)
        return;
      List<TemporaryAnimatedSprite> temporarySprites = this.temporarySprites;
      TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(372, 1956, 10, 10), new Vector2(3f, 20f) * (float) Game1.tileSize + new Vector2((float) Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize), (float) Game1.random.Next(Game1.tileSize / 4)), false, 1f / 500f, Color.Lime);
      temporaryAnimatedSprite.alpha = 0.75f;
      Vector2 vector2_1 = new Vector2(0.0f, -0.5f);
      temporaryAnimatedSprite.motion = vector2_1;
      Vector2 vector2_2 = new Vector2(-1f / 500f, 0.0f);
      temporaryAnimatedSprite.acceleration = vector2_2;
      double num1 = 99999.0;
      temporaryAnimatedSprite.interval = (float) num1;
      double num2 = 22.5 * (double) Game1.tileSize / 10000.0 - (double) Game1.random.Next(100) / 10000.0;
      temporaryAnimatedSprite.layerDepth = (float) num2;
      double num3 = (double) (Game1.pixelZoom * 3) / 4.0;
      temporaryAnimatedSprite.scale = (float) num3;
      double num4 = 0.00999999977648258;
      temporaryAnimatedSprite.scaleChange = (float) num4;
      double num5 = (double) Game1.random.Next(-5, 6) * 3.14159274101257 / 256.0;
      temporaryAnimatedSprite.rotationChange = (float) num5;
      temporarySprites.Add(temporaryAnimatedSprite);
      this.cauldronTimer = 100;
    }

    public override void cleanupBeforePlayerExit()
    {
      Game1.changeMusicTrack("none");
      base.cleanupBeforePlayerExit();
    }

    public override void resetForPlayerEntry()
    {
      base.resetForPlayerEntry();
      List<TemporaryAnimatedSprite> temporarySprites1 = this.temporarySprites;
      TemporaryAnimatedSprite temporaryAnimatedSprite1 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(276, 1985, 12, 11), new Vector2(10f, 12f) * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), (float) (-Game1.tileSize / 2)), false, 0.0f, Color.White);
      temporaryAnimatedSprite1.interval = 50f;
      temporaryAnimatedSprite1.totalNumberOfLoops = 99999;
      temporaryAnimatedSprite1.animationLength = 4;
      temporaryAnimatedSprite1.light = true;
      temporaryAnimatedSprite1.lightRadius = 2f;
      double pixelZoom = (double) Game1.pixelZoom;
      temporaryAnimatedSprite1.scale = (float) pixelZoom;
      temporarySprites1.Add(temporaryAnimatedSprite1);
      List<TemporaryAnimatedSprite> temporarySprites2 = this.temporarySprites;
      TemporaryAnimatedSprite temporaryAnimatedSprite2 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(276, 1985, 12, 11), new Vector2(2f, 21f) * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize * 4 / 5), (float) (Game1.tileSize / 2)), false, 0.0f, Color.White);
      temporaryAnimatedSprite2.interval = 50f;
      temporaryAnimatedSprite2.totalNumberOfLoops = 99999;
      temporaryAnimatedSprite2.animationLength = 4;
      temporaryAnimatedSprite2.light = true;
      temporaryAnimatedSprite2.lightRadius = 1f;
      double num1 = (double) (Game1.pixelZoom / 2);
      temporaryAnimatedSprite2.scale = (float) num1;
      temporarySprites2.Add(temporaryAnimatedSprite2);
      List<TemporaryAnimatedSprite> temporarySprites3 = this.temporarySprites;
      TemporaryAnimatedSprite temporaryAnimatedSprite3 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(276, 1985, 12, 11), new Vector2(3f, 21f) * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 4), (float) (Game1.tileSize / 2)), false, 0.0f, Color.White);
      temporaryAnimatedSprite3.interval = 50f;
      temporaryAnimatedSprite3.totalNumberOfLoops = 99999;
      temporaryAnimatedSprite3.animationLength = 4;
      temporaryAnimatedSprite3.light = true;
      temporaryAnimatedSprite3.lightRadius = 1f;
      double num2 = (double) (Game1.pixelZoom * 3 / 4);
      temporaryAnimatedSprite3.scale = (float) num2;
      temporarySprites3.Add(temporaryAnimatedSprite3);
      List<TemporaryAnimatedSprite> temporarySprites4 = this.temporarySprites;
      TemporaryAnimatedSprite temporaryAnimatedSprite4 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(276, 1985, 12, 11), new Vector2(4f, 21f) * (float) Game1.tileSize + new Vector2((float) (-Game1.tileSize / 4), (float) (Game1.tileSize / 2)), false, 0.0f, Color.White);
      temporaryAnimatedSprite4.interval = 50f;
      temporaryAnimatedSprite4.totalNumberOfLoops = 99999;
      temporaryAnimatedSprite4.animationLength = 4;
      temporaryAnimatedSprite4.light = true;
      temporaryAnimatedSprite4.lightRadius = 1f;
      double num3 = (double) (Game1.pixelZoom / 2);
      temporaryAnimatedSprite4.scale = (float) num3;
      temporarySprites4.Add(temporaryAnimatedSprite4);
      if (!Game1.player.eventsSeen.Contains(418172))
        return;
      this.setMapTileIndex(2, 12, 2143, "Front", 1);
    }
  }
}
