// Decompiled with JetBrains decompiler
// Type: StardewValley.Events.WitchEvent
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Buildings;
using StardewValley.Monsters;
using StardewValley.TerrainFeatures;
using System;

namespace StardewValley.Events
{
  public class WitchEvent : FarmEvent
  {
    public const int identifier = 942069;
    private Vector2 witchPosition;
    private Building targetBuilding;
    private Farm f;
    private Random r;
    private int witchFrame;
    private int witchAnimationTimer;
    private int animationLoopsDone;
    private int timerSinceFade;
    private bool animateLeft;
    private bool terminate;

    public bool setUp()
    {
      this.f = Game1.getLocationFromName("Farm") as Farm;
      this.r = new Random((int) Game1.uniqueIDForThisGame + (int) Game1.stats.DaysPlayed);
      foreach (Building building in this.f.buildings)
      {
        if (building is Coop && !building.buildingType.Equals("Coop") && (!(building.indoors as AnimalHouse).isFull() && building.indoors.objects.Count < 50) && this.r.NextDouble() < 0.8)
          this.targetBuilding = building;
      }
      if (this.targetBuilding == null)
      {
        foreach (Building building in this.f.buildings)
        {
          if (building.buildingType.Equals("Slime Hutch") && building.indoors.characters.Count > 0 && (this.r.NextDouble() < 0.5 && building.indoors.numberOfObjectsOfType(83, true) == 0))
            this.targetBuilding = building;
        }
      }
      if (this.targetBuilding == null)
        return true;
      Game1.currentLightSources.Add(new LightSource(4, this.witchPosition, 2f, Color.Black, 942069));
      Game1.currentLocation = (GameLocation) this.f;
      this.f.resetForPlayerEntry();
      Game1.fadeClear();
      Game1.nonWarpFade = true;
      Game1.timeOfDay = 2400;
      Game1.ambientLight = new Color(200, 190, 40);
      Game1.displayHUD = false;
      Game1.freezeControls = true;
      Game1.viewportFreeze = true;
      Game1.displayFarmer = false;
      Game1.viewport.X = Math.Max(0, Math.Min(this.f.map.DisplayWidth - Game1.viewport.Width, this.targetBuilding.tileX * Game1.tileSize - Game1.viewport.Width / 2));
      Game1.viewport.Y = Math.Max(0, Math.Min(this.f.map.DisplayHeight - Game1.viewport.Height, (this.targetBuilding.tileY - 3) * Game1.tileSize - Game1.viewport.Height / 2));
      this.witchPosition = new Vector2((float) (Game1.viewport.X + Game1.viewport.Width + Game1.tileSize * 2), (float) (this.targetBuilding.tileY * Game1.tileSize - Game1.tileSize));
      Game1.changeMusicTrack("nightTime");
      DelayedAction.playSoundAfterDelay("cacklingWitch", 3200);
      return false;
    }

    public bool tickUpdate(GameTime time)
    {
      if (this.terminate)
        return true;
      Game1.UpdateGameClock(time);
      this.f.UpdateWhenCurrentLocation(time);
      this.f.updateEvenIfFarmerIsntHere(time, false);
      Game1.UpdateOther(time);
      Utility.repositionLightSource(942069, this.witchPosition + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2)));
      TimeSpan timeSpan;
      if (this.animationLoopsDone < 1)
      {
        int timerSinceFade = this.timerSinceFade;
        timeSpan = time.ElapsedGameTime;
        int milliseconds = timeSpan.Milliseconds;
        this.timerSinceFade = timerSinceFade + milliseconds;
      }
      if ((double) this.witchPosition.X > (double) (this.targetBuilding.tileX * Game1.tileSize + Game1.tileSize * 3 / 2))
      {
        if (this.timerSinceFade < 2000)
          return false;
        // ISSUE: explicit reference operation
        // ISSUE: variable of a reference type
        float& local1 = @this.witchPosition.X;
        // ISSUE: explicit reference operation
        double num1 = (double) ^local1;
        timeSpan = time.ElapsedGameTime;
        double num2 = (double) timeSpan.Milliseconds * 0.400000005960464;
        double num3 = num1 - num2;
        // ISSUE: explicit reference operation
        ^local1 = (float) num3;
        // ISSUE: explicit reference operation
        // ISSUE: variable of a reference type
        float& local2 = @this.witchPosition.Y;
        // ISSUE: explicit reference operation
        double num4 = (double) ^local2;
        timeSpan = time.TotalGameTime;
        double num5 = Math.Cos((double) timeSpan.Milliseconds * Math.PI / 512.0) * 1.0;
        double num6 = num4 + num5;
        // ISSUE: explicit reference operation
        ^local2 = (float) num6;
      }
      else if (this.animationLoopsDone < 4)
      {
        // ISSUE: explicit reference operation
        // ISSUE: variable of a reference type
        float& local = @this.witchPosition.Y;
        // ISSUE: explicit reference operation
        double num1 = (double) ^local;
        timeSpan = time.TotalGameTime;
        double num2 = Math.Cos((double) timeSpan.Milliseconds * Math.PI / 512.0) * 1.0;
        double num3 = num1 + num2;
        // ISSUE: explicit reference operation
        ^local = (float) num3;
        int witchAnimationTimer = this.witchAnimationTimer;
        timeSpan = time.ElapsedGameTime;
        int milliseconds = timeSpan.Milliseconds;
        this.witchAnimationTimer = witchAnimationTimer + milliseconds;
        if (this.witchAnimationTimer > 2000)
        {
          this.witchAnimationTimer = 0;
          if (!this.animateLeft)
          {
            this.witchFrame = this.witchFrame + 1;
            if (this.witchFrame == 1)
            {
              this.animateLeft = true;
              for (int index = 0; index < 75; ++index)
                this.f.temporarySprites.Add(new TemporaryAnimatedSprite(10, this.witchPosition + new Vector2((float) (2 * Game1.pixelZoom), (float) (Game1.tileSize * 3 / 2 - Game1.pixelZoom * 4)), this.r.NextDouble() < 0.5 ? Color.Lime : Color.DarkViolet, 8, false, 100f, 0, -1, -1f, -1, 0)
                {
                  motion = new Vector2((float) this.r.Next(-100, 100) / 100f, 1.5f),
                  alphaFade = 0.015f,
                  delayBeforeAnimationStart = index * 30,
                  layerDepth = 1f
                });
              Game1.playSound("debuffSpell");
            }
          }
          else
          {
            this.witchFrame = this.witchFrame - 1;
            this.animationLoopsDone = 4;
            DelayedAction.playSoundAfterDelay("cacklingWitch", 2500);
          }
        }
      }
      else
      {
        int witchAnimationTimer = this.witchAnimationTimer;
        timeSpan = time.ElapsedGameTime;
        int milliseconds1 = timeSpan.Milliseconds;
        this.witchAnimationTimer = witchAnimationTimer + milliseconds1;
        this.witchFrame = 0;
        if (this.witchAnimationTimer > 1000 && (double) this.witchPosition.X > -999999.0)
        {
          // ISSUE: explicit reference operation
          // ISSUE: variable of a reference type
          float& local1 = @this.witchPosition.Y;
          // ISSUE: explicit reference operation
          double num1 = (double) ^local1;
          timeSpan = time.TotalGameTime;
          double num2 = Math.Cos((double) timeSpan.Milliseconds * Math.PI / 256.0) * 2.0;
          double num3 = num1 + num2;
          // ISSUE: explicit reference operation
          ^local1 = (float) num3;
          // ISSUE: explicit reference operation
          // ISSUE: variable of a reference type
          float& local2 = @this.witchPosition.X;
          // ISSUE: explicit reference operation
          double num4 = (double) ^local2;
          timeSpan = time.ElapsedGameTime;
          double num5 = (double) timeSpan.Milliseconds * 0.400000005960464;
          double num6 = num4 - num5;
          // ISSUE: explicit reference operation
          ^local2 = (float) num6;
        }
        if ((double) this.witchPosition.X < (double) (Game1.viewport.X - Game1.tileSize * 2) || float.IsNaN(this.witchPosition.X))
        {
          if (!Game1.fadeToBlack && (double) this.witchPosition.X != -999999.0)
          {
            Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.afterLastFade), 0.02f);
            Game1.changeMusicTrack("none");
            this.timerSinceFade = 0;
            this.witchPosition.X = -999999f;
          }
          int timerSinceFade = this.timerSinceFade;
          timeSpan = time.ElapsedGameTime;
          int milliseconds2 = timeSpan.Milliseconds;
          this.timerSinceFade = timerSinceFade + milliseconds2;
        }
      }
      return false;
    }

    public void afterLastFade()
    {
      this.terminate = true;
      Game1.globalFadeToClear((Game1.afterFadeFunction) null, 0.02f);
    }

    public void draw(SpriteBatch b)
    {
      b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.witchPosition), new Rectangle?(new Rectangle(277, 1886 + this.witchFrame * 29, 34, 29)), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 0.9999999f);
    }

    public void makeChangesToLocation()
    {
      if (this.targetBuilding.buildingType.Equals("Slime Hutch"))
      {
        foreach (NPC character in this.targetBuilding.indoors.characters)
        {
          if (character is GreenSlime)
            (character as GreenSlime).color = new Color(40 + this.r.Next(10), 40 + this.r.Next(10), 40 + this.r.Next(10));
        }
      }
      else
      {
        for (int index1 = 0; index1 < 200; ++index1)
        {
          Vector2 index2 = new Vector2((float) this.r.Next(2, this.targetBuilding.indoors.map.Layers[0].LayerWidth - 2), (float) this.r.Next(2, this.targetBuilding.indoors.map.Layers[0].LayerHeight - 2));
          if (this.targetBuilding.indoors.isTileLocationTotallyClearAndPlaceable(index2) || this.targetBuilding.indoors.terrainFeatures.ContainsKey(index2) && this.targetBuilding.indoors.terrainFeatures[index2] is Flooring)
          {
            this.targetBuilding.indoors.objects.Add(index2, new StardewValley.Object(Vector2.Zero, 305, (string) null, false, true, false, true));
            break;
          }
        }
      }
    }

    public void drawAboveEverything(SpriteBatch b)
    {
    }
  }
}
