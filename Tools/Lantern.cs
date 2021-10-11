// Decompiled with JetBrains decompiler
// Type: StardewValley.Tools.Lantern
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using System.Linq;

namespace StardewValley.Tools
{
  public class Lantern : Tool
  {
    public const float baseRadius = 10f;
    public const int millisecondsPerFuelUnit = 6000;
    public const int maxFuel = 100;
    public int fuelLeft;
    private int fuelTimer;
    public bool on;

    public Lantern()
      : base(nameof (Lantern), 0, 74, 74, false, 0)
    {
      this.upgradeLevel = 0;
      this.CurrentParentTileIndex = this.indexOfMenuItemView;
      this.instantUse = true;
    }

    protected override string loadDescription()
    {
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:Lantern.cs.14115");
    }

    protected override string loadDisplayName()
    {
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:Lantern.cs.14114");
    }

    public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
    {
      base.DoFunction(location, x, y, power, who);
      this.on = !this.on;
      this.CurrentParentTileIndex = this.indexOfMenuItemView;
      if (this.on)
      {
        Game1.currentLightSources.Add(new LightSource(Game1.lantern, new Vector2(who.position.X + (float) (Game1.tileSize / 3), who.position.Y + (float) Game1.tileSize), (float) (2.5 + (double) this.fuelLeft / 100.0 * 10.0 * 0.75), new Color(0, 131, (int) byte.MaxValue), -85736));
      }
      else
      {
        for (int index = Game1.currentLightSources.Count - 1; index >= 0; --index)
        {
          if (Game1.currentLightSources.ElementAt<LightSource>(index).identifier == -85736)
          {
            Game1.currentLightSources.Remove(Game1.currentLightSources.ElementAt<LightSource>(index));
            break;
          }
        }
      }
    }

    public override void tickUpdate(GameTime time, Farmer who)
    {
      if (this.on && this.fuelLeft > 0 && Game1.drawLighting)
      {
        this.fuelTimer = this.fuelTimer + time.ElapsedGameTime.Milliseconds;
        if (this.fuelTimer > 6000)
        {
          this.fuelLeft = this.fuelLeft - 1;
          this.fuelTimer = 0;
        }
        bool flag = false;
        foreach (LightSource currentLightSource in Game1.currentLightSources)
        {
          if (currentLightSource.identifier == -85736)
          {
            currentLightSource.position = new Vector2(who.position.X + (float) (Game1.tileSize / 3), who.position.Y + (float) Game1.tileSize);
            flag = true;
            break;
          }
        }
        if (!flag)
          Game1.currentLightSources.Add(new LightSource(Game1.lantern, new Vector2(who.position.X + (float) (Game1.tileSize / 3), who.position.Y + (float) Game1.tileSize), (float) (2.5 + (double) this.fuelLeft / 100.0 * 10.0 * 0.75), new Color(0, 131, (int) byte.MaxValue), -85736));
      }
      if (!this.on || this.fuelLeft > 0)
        return;
      Utility.removeLightSource(1);
    }
  }
}
