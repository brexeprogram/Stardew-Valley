// Decompiled with JetBrains decompiler
// Type: StardewValley.BellsAndWhistles.AmbientLocationSounds
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.BellsAndWhistles
{
  public class AmbientLocationSounds
  {
    private static Dictionary<Vector2, int> sounds = new Dictionary<Vector2, int>();
    private static int updateTimer = 100;
    private static int farthestSoundDistance = Game1.tileSize * 16;
    public const int sound_babblingBrook = 0;
    public const int sound_cracklingFire = 1;
    public const int sound_engine = 2;
    public const int sound_cricket = 3;
    public const int numberOfSounds = 4;
    public const float doNotPlay = 9999999f;
    private static float[] shortestDistanceForCue;
    private static Cue babblingBrook;
    private static Cue cracklingFire;
    private static Cue engine;
    private static Cue cricket;
    private static float volumeOverrideForLocChange;

    public static void InitShared()
    {
      if (Game1.soundBank != null)
      {
        if (AmbientLocationSounds.babblingBrook == null)
        {
          AmbientLocationSounds.babblingBrook = Game1.soundBank.GetCue("babblingBrook");
          AmbientLocationSounds.babblingBrook.Play();
          AmbientLocationSounds.babblingBrook.Pause();
        }
        if (AmbientLocationSounds.cracklingFire == null)
        {
          AmbientLocationSounds.cracklingFire = Game1.soundBank.GetCue("cracklingFire");
          AmbientLocationSounds.cracklingFire.Play();
          AmbientLocationSounds.cracklingFire.Pause();
        }
        if (AmbientLocationSounds.engine == null)
        {
          AmbientLocationSounds.engine = Game1.soundBank.GetCue("heavyEngine");
          AmbientLocationSounds.engine.Play();
          AmbientLocationSounds.engine.Pause();
        }
        if (AmbientLocationSounds.cricket == null)
        {
          AmbientLocationSounds.cricket = Game1.soundBank.GetCue("cricketsAmbient");
          AmbientLocationSounds.cricket.Play();
          AmbientLocationSounds.cricket.Pause();
        }
      }
      AmbientLocationSounds.shortestDistanceForCue = new float[4];
    }

    public static void update(GameTime time)
    {
      if (AmbientLocationSounds.sounds.Count == 0)
        return;
      if ((double) AmbientLocationSounds.volumeOverrideForLocChange < 1.0)
        AmbientLocationSounds.volumeOverrideForLocChange += (float) time.ElapsedGameTime.Milliseconds * 0.0003f;
      AmbientLocationSounds.updateTimer -= time.ElapsedGameTime.Milliseconds;
      if (AmbientLocationSounds.updateTimer > 0)
        return;
      for (int index = 0; index < AmbientLocationSounds.shortestDistanceForCue.Length; ++index)
        AmbientLocationSounds.shortestDistanceForCue[index] = 9999999f;
      Vector2 standingPosition = Game1.player.getStandingPosition();
      for (int index = 0; index < AmbientLocationSounds.sounds.Count; ++index)
      {
        float num = Vector2.Distance(AmbientLocationSounds.sounds.ElementAt<KeyValuePair<Vector2, int>>(index).Key, standingPosition);
        if ((double) AmbientLocationSounds.shortestDistanceForCue[AmbientLocationSounds.sounds.ElementAt<KeyValuePair<Vector2, int>>(index).Value] > (double) num)
          AmbientLocationSounds.shortestDistanceForCue[AmbientLocationSounds.sounds.ElementAt<KeyValuePair<Vector2, int>>(index).Value] = num;
      }
      if ((double) AmbientLocationSounds.volumeOverrideForLocChange >= 0.0)
      {
        for (int index = 0; index < AmbientLocationSounds.shortestDistanceForCue.Length; ++index)
        {
          if ((double) AmbientLocationSounds.shortestDistanceForCue[index] <= (double) AmbientLocationSounds.farthestSoundDistance)
          {
            float num = Math.Min(AmbientLocationSounds.volumeOverrideForLocChange, Math.Min(1f, (float) (1.0 - (double) AmbientLocationSounds.shortestDistanceForCue[index] / (double) AmbientLocationSounds.farthestSoundDistance)));
            switch (index)
            {
              case 0:
                if (AmbientLocationSounds.babblingBrook != null)
                {
                  AmbientLocationSounds.babblingBrook.SetVariable("Volume", num * 100f * Math.Min(Game1.ambientPlayerVolume, Game1.options.ambientVolumeLevel));
                  AmbientLocationSounds.babblingBrook.Resume();
                  continue;
                }
                continue;
              case 1:
                if (AmbientLocationSounds.cracklingFire != null)
                {
                  AmbientLocationSounds.cracklingFire.SetVariable("Volume", num * 100f * Math.Min(Game1.ambientPlayerVolume, Game1.options.ambientVolumeLevel));
                  AmbientLocationSounds.cracklingFire.Resume();
                  continue;
                }
                continue;
              case 2:
                if (AmbientLocationSounds.engine != null)
                {
                  AmbientLocationSounds.engine.SetVariable("Volume", num * 100f * Math.Min(Game1.ambientPlayerVolume, Game1.options.ambientVolumeLevel));
                  AmbientLocationSounds.engine.Resume();
                  continue;
                }
                continue;
              case 3:
                if (AmbientLocationSounds.cricket != null)
                {
                  AmbientLocationSounds.cricket.SetVariable("Volume", num * 100f * Math.Min(Game1.ambientPlayerVolume, Game1.options.ambientVolumeLevel));
                  AmbientLocationSounds.cricket.Resume();
                  continue;
                }
                continue;
              default:
                continue;
            }
          }
          else
          {
            switch (index)
            {
              case 0:
                if (AmbientLocationSounds.babblingBrook != null)
                {
                  AmbientLocationSounds.babblingBrook.Pause();
                  continue;
                }
                continue;
              case 1:
                if (AmbientLocationSounds.cracklingFire != null)
                {
                  AmbientLocationSounds.cracklingFire.Pause();
                  continue;
                }
                continue;
              case 2:
                if (AmbientLocationSounds.engine != null)
                {
                  AmbientLocationSounds.engine.Pause();
                  continue;
                }
                continue;
              case 3:
                if (AmbientLocationSounds.cricket != null)
                {
                  AmbientLocationSounds.cricket.Pause();
                  continue;
                }
                continue;
              default:
                continue;
            }
          }
        }
      }
      AmbientLocationSounds.updateTimer = 100;
    }

    public static void changeSpecificVariable(string variableName, float value, int whichSound)
    {
      if (whichSound != 2 || AmbientLocationSounds.engine == null)
        return;
      AmbientLocationSounds.engine.SetVariable(variableName, value);
    }

    public static void addSound(Vector2 tileLocation, int whichSound)
    {
      if (AmbientLocationSounds.sounds.ContainsKey(tileLocation * (float) Game1.tileSize))
        return;
      AmbientLocationSounds.sounds.Add(tileLocation * (float) Game1.tileSize, whichSound);
    }

    public static void removeSound(Vector2 tileLocation)
    {
      if (!AmbientLocationSounds.sounds.ContainsKey(tileLocation * (float) Game1.tileSize))
        return;
      switch (AmbientLocationSounds.sounds[tileLocation * (float) Game1.tileSize])
      {
        case 0:
          if (AmbientLocationSounds.babblingBrook != null)
          {
            AmbientLocationSounds.babblingBrook.Pause();
            break;
          }
          break;
        case 1:
          if (AmbientLocationSounds.cracklingFire != null)
          {
            AmbientLocationSounds.cracklingFire.Pause();
            break;
          }
          break;
        case 2:
          if (AmbientLocationSounds.engine != null)
          {
            AmbientLocationSounds.engine.Pause();
            break;
          }
          break;
        case 3:
          if (AmbientLocationSounds.cricket != null)
          {
            AmbientLocationSounds.cricket.Pause();
            break;
          }
          break;
      }
      AmbientLocationSounds.sounds.Remove(tileLocation * (float) Game1.tileSize);
    }

    public static void onLocationLeave()
    {
      AmbientLocationSounds.sounds.Clear();
      AmbientLocationSounds.volumeOverrideForLocChange = -0.5f;
      if (AmbientLocationSounds.babblingBrook != null)
        AmbientLocationSounds.babblingBrook.Pause();
      if (AmbientLocationSounds.cracklingFire != null)
        AmbientLocationSounds.cracklingFire.Pause();
      if (AmbientLocationSounds.engine != null)
      {
        AmbientLocationSounds.engine.SetVariable("Frequency", 100f);
        AmbientLocationSounds.engine.Pause();
      }
      if (AmbientLocationSounds.cricket == null)
        return;
      AmbientLocationSounds.cricket.Pause();
    }
  }
}
