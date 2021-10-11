// Decompiled with JetBrains decompiler
// Type: StardewValley.Tools.Sword
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using System;

namespace StardewValley.Tools
{
  public class Sword : Tool
  {
    public const double baseCritChance = 0.02;
    public int whichUpgrade;

    public Sword()
    {
    }

    public Sword(string name, int spriteIndex)
      : base(name, 0, spriteIndex, spriteIndex, false, 0)
    {
    }

    public void DoFunction(GameLocation location, int x, int y, int facingDirection, int power, Farmer who)
    {
      this.DoFunction(location, x, y, power, who);
      Vector2 index1 = Vector2.Zero;
      Vector2 index2 = Vector2.Zero;
      Rectangle rectangle = Rectangle.Empty;
      Rectangle boundingBox = who.GetBoundingBox();
      switch (facingDirection)
      {
        case 0:
          rectangle = new Rectangle(x - Game1.tileSize, boundingBox.Y - Game1.tileSize, Game1.tileSize * 2, Game1.tileSize);
          index1 = new Vector2((float) ((Game1.random.NextDouble() < 0.5 ? rectangle.Left : rectangle.Right) / Game1.tileSize), (float) (rectangle.Top / Game1.tileSize));
          index2 = new Vector2((float) (rectangle.Center.X / Game1.tileSize), (float) (rectangle.Top / Game1.tileSize));
          break;
        case 1:
          rectangle = new Rectangle(boundingBox.Right, y - Game1.tileSize, Game1.tileSize, Game1.tileSize * 2);
          index1 = new Vector2((float) (rectangle.Center.X / Game1.tileSize), (float) ((Game1.random.NextDouble() < 0.5 ? rectangle.Top : rectangle.Bottom) / Game1.tileSize));
          index2 = new Vector2((float) (rectangle.Center.X / Game1.tileSize), (float) (rectangle.Center.Y / Game1.tileSize));
          break;
        case 2:
          rectangle = new Rectangle(x - Game1.tileSize, boundingBox.Bottom, Game1.tileSize * 2, Game1.tileSize);
          index1 = new Vector2((float) ((Game1.random.NextDouble() < 0.5 ? rectangle.Left : rectangle.Right) / Game1.tileSize), (float) (rectangle.Center.Y / Game1.tileSize));
          index2 = new Vector2((float) (rectangle.Center.X / Game1.tileSize), (float) (rectangle.Center.Y / Game1.tileSize));
          break;
        case 3:
          rectangle = new Rectangle(boundingBox.Left - Game1.tileSize, y - Game1.tileSize, Game1.tileSize, Game1.tileSize * 2);
          index1 = new Vector2((float) (rectangle.Left / Game1.tileSize), (float) ((Game1.random.NextDouble() < 0.5 ? rectangle.Top : rectangle.Bottom) / Game1.tileSize));
          index2 = new Vector2((float) (rectangle.Left / Game1.tileSize), (float) (rectangle.Center.Y / Game1.tileSize));
          break;
      }
      int minDamage = (this.whichUpgrade == 2 ? 3 : (this.whichUpgrade == 4 ? 6 : this.whichUpgrade)) + 1;
      int maxDamage = 4 * ((this.whichUpgrade == 2 ? 3 : (this.whichUpgrade == 4 ? 5 : this.whichUpgrade)) + 1);
      bool flag1 = location.damageMonster(rectangle, minDamage, maxDamage, false, who);
      if (this.whichUpgrade == 4 && !flag1)
        location.temporarySprites.Add(new TemporaryAnimatedSprite(352, (float) Game1.random.Next(50, 120), 2, 1, new Vector2((float) (rectangle.Center.X - Game1.tileSize / 2), (float) (rectangle.Center.Y - Game1.tileSize / 2)) + new Vector2((float) Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize / 2), (float) Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize / 2)), false, Game1.random.NextDouble() < 0.5));
      string cueName = "";
      if (!flag1)
      {
        if (location.objects.ContainsKey(index1) && !location.Objects[index1].Name.Contains("Stone") && (!location.Objects[index1].Name.Contains("Stick") && !location.Objects[index1].Name.Contains("Stump")) && (!location.Objects[index1].Name.Contains("Boulder") && !location.Objects[index1].Name.Contains("Lumber") && !location.Objects[index1].IsHoeDirt))
        {
          if (location.Objects[index1].Name.Contains("Weed"))
          {
            if ((double) who.Stamina <= 0.0)
              return;
            ++Game1.stats.WeedsEliminated;
            if (Game1.questOfTheDay != null && Game1.questOfTheDay.accepted && (!Game1.questOfTheDay.completed && Game1.questOfTheDay.GetType().Name.Equals("WeedingQuest")))
              Game1.questOfTheDay.checkIfComplete((NPC) null, -1, -1, (Item) null, (string) null);
            this.checkWeedForTreasure(index1, who);
            cueName = location.Objects[index1].Category != -2 ? "cut" : "stoneCrack";
            location.removeObject(index1, true);
          }
          else
            location.objects[index1].performToolAction((Tool) this);
        }
        if (location.objects.ContainsKey(index2) && !location.Objects[index2].Name.Contains("Stone") && (!location.Objects[index2].Name.Contains("Stick") && !location.Objects[index2].Name.Contains("Stump")) && (!location.Objects[index2].Name.Contains("Boulder") && !location.Objects[index2].Name.Contains("Lumber") && !location.Objects[index2].IsHoeDirt))
        {
          if (location.Objects[index2].Name.Contains("Weed"))
          {
            if ((double) who.Stamina <= 0.0)
              return;
            ++Game1.stats.WeedsEliminated;
            if (Game1.questOfTheDay != null && Game1.questOfTheDay.accepted && (!Game1.questOfTheDay.completed && Game1.questOfTheDay.GetType().Name.Equals("WeedingQuest")))
              Game1.questOfTheDay.checkIfComplete((NPC) null, -1, -1, (Item) null, (string) null);
            this.checkWeedForTreasure(index2, who);
          }
          else
            location.objects[index2].performToolAction((Tool) this);
        }
      }
      bool flag2 = false;
      foreach (Vector2 index3 in Utility.getListOfTileLocationsForBordersOfNonTileRectangle(rectangle))
      {
        if (location.terrainFeatures.ContainsKey(index3) && location.terrainFeatures[index3].performToolAction((Tool) this, 0, index3, (GameLocation) null))
        {
          location.terrainFeatures.Remove(index3);
          flag2 = true;
        }
      }
      int num = flag2 ? 1 : 0;
      if (!cueName.Equals(""))
        Game1.playSound(cueName);
      this.CurrentParentTileIndex = this.indexOfMenuItemView;
    }

    public void checkWeedForTreasure(Vector2 tileLocation, Farmer who)
    {
      Random random = new Random((int) ((double) (Game1.uniqueIDForThisGame + (ulong) Game1.stats.DaysPlayed) + (double) tileLocation.X * 13.0 + (double) tileLocation.Y * 29.0));
      if (random.NextDouble() < 0.07)
        Game1.createDebris(12, (int) tileLocation.X, (int) tileLocation.Y, random.Next(1, 3), (GameLocation) null);
      else if (random.NextDouble() < 0.02 + (double) who.LuckLevel / 10.0)
      {
        Game1.createDebris(random.NextDouble() < 0.5 ? 4 : 8, (int) tileLocation.X, (int) tileLocation.Y, random.Next(1, 4), (GameLocation) null);
      }
      else
      {
        if (random.NextDouble() >= 0.006 + (double) who.LuckLevel / 20.0)
          return;
        Game1.createObjectDebris(114, (int) tileLocation.X, (int) tileLocation.Y, -1, 0, 1f, (GameLocation) null);
      }
    }

    protected override string loadDisplayName()
    {
      if (this.name.Equals("Battered Sword"))
        return Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1205");
      switch (this.whichUpgrade)
      {
        case 2:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Sword.cs.14292");
        case 3:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Sword.cs.14294");
        case 4:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Sword.cs.14296");
        default:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Sword.cs.14290");
      }
    }

    protected override string loadDescription()
    {
      switch (this.whichUpgrade)
      {
        case 1:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Sword.cs.14291");
        case 2:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Sword.cs.14293");
        case 3:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Sword.cs.14295");
        case 4:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Sword.cs.14297");
        default:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1206");
      }
    }

    public void upgrade(int which)
    {
      if (which > this.whichUpgrade)
      {
        this.whichUpgrade = which;
        switch (which)
        {
          case 1:
            this.name = "Hero's Sword";
            this.indexOfMenuItemView = 68;
            break;
          case 2:
            this.name = "Holy Sword";
            this.indexOfMenuItemView = 70;
            break;
          case 3:
            this.name = "Dark Sword";
            this.indexOfMenuItemView = 69;
            break;
          case 4:
            this.name = "Galaxy Sword";
            this.indexOfMenuItemView = 71;
            break;
        }
        this.displayName = (string) null;
        this.description = (string) null;
        this.upgradeLevel = which;
      }
      this.CurrentParentTileIndex = this.indexOfMenuItemView;
    }
  }
}
