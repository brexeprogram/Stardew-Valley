// Decompiled with JetBrains decompiler
// Type: StardewValley.Locations.Railroad
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using xTile;
using xTile.Dimensions;

namespace StardewValley.Locations
{
  public class Railroad : GameLocation
  {
    private int trainTime = -1;
    public const int trainSoundDelay = 10000;
    [XmlIgnore]
    public Train train;
    private bool hasTrainPassed;
    [XmlIgnore]
    public int trainTimer;
    public static Cue trainLoop;
    public bool witchStatueGone;

    public Railroad()
    {
    }

    public Railroad(Map map, string name)
      : base(map, name)
    {
    }

    public override void resetForPlayerEntry()
    {
      base.resetForPlayerEntry();
      if (Game1.currentSong != null && Game1.currentSong.Name.ToLower().Contains("ambient"))
        Game1.changeMusicTrack("none");
      if (this.witchStatueGone || Game1.player.mailReceived.Contains("witchStatueGone"))
      {
        this.removeTile(54, 35, "Buildings");
        this.removeTile(54, 34, "Front");
      }
      if (Game1.IsWinter)
        return;
      AmbientLocationSounds.addSound(new Vector2(15f, 56f), 0);
    }

    public override void cleanupBeforePlayerExit()
    {
      base.cleanupBeforePlayerExit();
      if (Railroad.trainLoop != null)
        Railroad.trainLoop.Stop(AudioStopOptions.Immediate);
      Railroad.trainLoop = (Cue) null;
    }

    public override void checkForMusic(GameTime time)
    {
      if (Game1.timeOfDay < 1800 && !Game1.isRaining && !Game1.eventUp)
      {
        string currentSeason = Game1.currentSeason;
        if (!(currentSeason == "summer") && !(currentSeason == "fall") && !(currentSeason == "spring"))
          return;
        Game1.changeMusicTrack(Game1.currentSeason + "_day_ambient");
      }
      else
      {
        if (Game1.timeOfDay < 2000 || Game1.isRaining || Game1.eventUp)
          return;
        string currentSeason = Game1.currentSeason;
        if (!(currentSeason == "summer") && !(currentSeason == "fall") && !(currentSeason == "spring"))
          return;
        Game1.changeMusicTrack("spring_night_ambient");
      }
    }

    public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
    {
      if (this.map.GetLayer("Buildings").Tiles[tileLocation] == null || this.map.GetLayer("Buildings").Tiles[tileLocation].TileIndex != 287)
        return base.checkAction(tileLocation, viewport, who);
      if (Game1.player.hasDarkTalisman)
      {
        Game1.player.freezePause = 7000;
        Game1.playSound("fireball");
        DelayedAction.playSoundAfterDelay("secret1", 2000);
        DelayedAction.removeTileAfterDelay(54, 35, 2000, (GameLocation) this, "Buildings");
        DelayedAction.removeTileAfterDelay(54, 34, 2000, (GameLocation) this, "Front");
        DelayedAction.removeTemporarySpriteAfterDelay((GameLocation) this, 9999f, 2000);
        this.witchStatueGone = true;
        who.mailReceived.Add("witchStatueGone");
        for (int index = 0; index < 22; ++index)
          DelayedAction.playSoundAfterDelay("batFlap", 2220 + 240 * index);
        List<TemporaryAnimatedSprite> temporarySprites1 = this.temporarySprites;
        TemporaryAnimatedSprite temporaryAnimatedSprite1 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(576, 271, 28, 31), 60f, 3, 999, new Vector2(54f, 34f) * (float) Game1.tileSize + new Vector2(-2f, 1f) * (float) Game1.pixelZoom, false, false, (float) (34 * Game1.tileSize) / 10000f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
        temporaryAnimatedSprite1.xPeriodic = true;
        temporaryAnimatedSprite1.xPeriodicLoopTime = 8000f;
        double num1 = (double) (Game1.tileSize * 6);
        temporaryAnimatedSprite1.xPeriodicRange = (float) num1;
        Vector2 vector2_1 = new Vector2(-2f, 0.0f);
        temporaryAnimatedSprite1.motion = vector2_1;
        Vector2 vector2_2 = new Vector2(0.0f, -0.015f);
        temporaryAnimatedSprite1.acceleration = vector2_2;
        int num2 = 1;
        temporaryAnimatedSprite1.pingPong = num2 != 0;
        int num3 = 2000;
        temporaryAnimatedSprite1.delayBeforeAnimationStart = num3;
        temporarySprites1.Add(temporaryAnimatedSprite1);
        List<TemporaryAnimatedSprite> temporarySprites2 = this.temporarySprites;
        TemporaryAnimatedSprite temporaryAnimatedSprite2 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(0, 499, 10, 11), 50f, 7, 999, new Vector2(54f, 34f) * (float) Game1.tileSize + new Vector2(7f, 11f) * (float) Game1.pixelZoom, false, false, (float) ((double) (34 * Game1.tileSize) / 10000.0 + 9.99999974737875E-05), 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
        temporaryAnimatedSprite2.xPeriodic = true;
        temporaryAnimatedSprite2.xPeriodicLoopTime = 8000f;
        double num4 = (double) (Game1.tileSize * 6);
        temporaryAnimatedSprite2.xPeriodicRange = (float) num4;
        Vector2 vector2_3 = new Vector2(-2f, 0.0f);
        temporaryAnimatedSprite2.motion = vector2_3;
        Vector2 vector2_4 = new Vector2(0.0f, -0.015f);
        temporaryAnimatedSprite2.acceleration = vector2_4;
        int num5 = 2000;
        temporaryAnimatedSprite2.delayBeforeAnimationStart = num5;
        temporarySprites2.Add(temporaryAnimatedSprite2);
        this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(0, 499, 10, 11), 35.715f, 7, 8, new Vector2(54f, 34f) * (float) Game1.tileSize + new Vector2(3f, 10f) * (float) Game1.pixelZoom, false, false, (float) ((double) (36 * Game1.tileSize) / 10000.0 + 9.99999974737875E-05), 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
        {
          id = 9999f
        });
      }
      else
        Game1.drawObjectDialogue("???");
      return true;
    }

    public override void DayUpdate(int dayOfMonth)
    {
      base.DayUpdate(dayOfMonth);
      this.hasTrainPassed = false;
      this.trainTime = -1;
      Random random = new Random((int) Game1.uniqueIDForThisGame / 2 + (int) Game1.stats.DaysPlayed);
      if (random.NextDouble() >= 0.2 || !Game1.isLocationAccessible(nameof (Railroad)))
        return;
      this.trainTime = random.Next(900, 1800);
      this.trainTime = this.trainTime - this.trainTime % 10;
    }

    public override bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer, int damagesFarmer, bool glider, Character character)
    {
      if (this.train != null && this.train.getBoundingBox().Intersects(position))
        return true;
      return base.isCollidingPosition(position, viewport, isFarmer, damagesFarmer, glider, character);
    }

    public void setTrainComing(int delay)
    {
      this.trainTimer = delay;
      if (!Game1.currentLocation.isOutdoors || Game1.isFestival())
        return;
      Game1.showGlobalMessage(Game1.content.LoadString("Strings\\Locations:Railroad_TrainComing"));
      if (Game1.soundBank == null)
        return;
      Cue cue = Game1.soundBank.GetCue("distantTrain");
      string name = "Volume";
      double num = 100.0;
      cue.SetVariable(name, (float) num);
      cue.Play();
    }

    public override void updateEvenIfFarmerIsntHere(GameTime time, bool skipWasUpdatedFlush = false)
    {
      base.updateEvenIfFarmerIsntHere(time, skipWasUpdatedFlush);
      if (Game1.timeOfDay == this.trainTime - this.trainTime % 10 && this.trainTimer == 0 && (!Game1.isFestival() && this.train == null))
        this.setTrainComing(10000);
      if (this.trainTimer > 0)
      {
        this.trainTimer = this.trainTimer - time.ElapsedGameTime.Milliseconds;
        if (this.trainTimer <= 0)
        {
          this.train = new Train();
          if (Game1.currentLocation.Equals((object) this))
            Game1.playSound("trainWhistle");
        }
        if (this.trainTimer < 3500)
        {
          if (Game1.soundBank != null && (Railroad.trainLoop == null || Railroad.trainLoop.IsStopped))
          {
            Railroad.trainLoop = Game1.soundBank.GetCue("trainLoop");
            Railroad.trainLoop.SetVariable("Volume", 0.0f);
          }
          if (Game1.currentLocation.Equals((object) this) && Railroad.trainLoop != null && !Railroad.trainLoop.IsPlaying)
            Railroad.trainLoop.Play();
        }
      }
      if (this.train != null)
      {
        if (Railroad.trainLoop == null || !Railroad.trainLoop.IsPlaying)
        {
          if (Game1.soundBank != null && (Railroad.trainLoop == null || Railroad.trainLoop.IsStopped))
          {
            Railroad.trainLoop = Game1.soundBank.GetCue("trainLoop");
            Railroad.trainLoop.SetVariable("Volume", 0.0f);
          }
          if (Game1.currentLocation.Equals((object) this) && Railroad.trainLoop != null)
            Railroad.trainLoop.Play();
        }
        if (this.train.Update(time, (GameLocation) this))
          this.train = (Train) null;
        if (Railroad.trainLoop == null || (double) Railroad.trainLoop.GetVariable("Volume") >= 100.0)
          return;
        Railroad.trainLoop.SetVariable("Volume", Railroad.trainLoop.GetVariable("Volume") + 0.5f);
      }
      else if (Railroad.trainLoop != null && this.trainTimer <= 0)
      {
        Railroad.trainLoop.SetVariable("Volume", Railroad.trainLoop.GetVariable("Volume") - 0.15f);
        if ((double) Railroad.trainLoop.GetVariable("Volume") > 0.0)
          return;
        Railroad.trainLoop.Stop(AudioStopOptions.Immediate);
        Railroad.trainLoop = (Cue) null;
      }
      else
      {
        if (this.trainTimer <= 0 || Railroad.trainLoop == null)
          return;
        Railroad.trainLoop.SetVariable("Volume", Railroad.trainLoop.GetVariable("Volume") + 0.15f);
      }
    }

    public override void UpdateWhenCurrentLocation(GameTime time)
    {
      base.UpdateWhenCurrentLocation(time);
    }

    public override void draw(SpriteBatch b)
    {
      base.draw(b);
      if (this.train == null || Game1.eventUp)
        return;
      this.train.draw(b);
    }
  }
}
