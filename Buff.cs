// Decompiled with JetBrains decompiler
// Type: StardewValley.Buff
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Text;

namespace StardewValley
{
  public class Buff
  {
    private int[] buffAttributes = new int[12];
    public int sheetIndex = -1;
    public int which = -1;
    public const float glowRate = 0.05f;
    public const int farming = 0;
    public const int fishing = 1;
    public const int mining = 2;
    public const int luck = 4;
    public const int foraging = 5;
    public const int crafting = 6;
    public const int maxStamina = 7;
    public const int magneticRadius = 8;
    public const int speed = 9;
    public const int defense = 10;
    public const int attack = 11;
    public const int totalNumberOfBuffableAttriutes = 12;
    public const int goblinsCurse = 12;
    public const int slimed = 13;
    public const int evilEye = 14;
    public const int chickenedOut = 15;
    public const int tipsy = 17;
    public const int fear = 18;
    public const int frozen = 19;
    public const int warriorEnergy = 20;
    public const int yobaBlessing = 21;
    public const int adrenalineRush = 22;
    public const int avoidMonsters = 23;
    public const int full = 6;
    public const int quenched = 7;
    public int millisecondsDuration;
    public string description;
    public string source;
    public string displaySource;
    public int total;
    public Color glow;

    public Buff(string description, int millisecondsDuration, string source, int index)
    {
      this.description = description;
      this.millisecondsDuration = millisecondsDuration;
      this.sheetIndex = index;
      this.source = source;
    }

    public Buff(int which)
    {
      this.which = which;
      this.sheetIndex = which;
      bool flag = true;
      switch (which)
      {
        case 6:
          this.description = Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.456");
          this.millisecondsDuration = 180000;
          flag = false;
          break;
        case 7:
          this.description = Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.457");
          this.millisecondsDuration = 60000;
          flag = false;
          break;
        case 12:
          this.description = Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.453") + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.454") + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.455");
          this.buffAttributes[9] = -3;
          this.buffAttributes[10] = -3;
          this.buffAttributes[11] = -3;
          this.glow = Color.Yellow;
          this.millisecondsDuration = 6000;
          break;
        case 13:
          this.description = Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.460") + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.461");
          this.buffAttributes[9] = -4;
          this.glow = Color.Green;
          this.millisecondsDuration = 2500 + Game1.random.Next(500);
          break;
        case 14:
          this.description = Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.464") + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.465");
          this.buffAttributes[10] = -8;
          this.glow = Color.HotPink;
          this.millisecondsDuration = 8000;
          break;
        case 17:
          this.description = Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.458") + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.459");
          this.buffAttributes[9] = -1;
          this.glow = Color.OrangeRed * 0.5f;
          this.millisecondsDuration = 30000;
          break;
        case 18:
          this.description = Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.462") + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.463");
          this.buffAttributes[11] = -8;
          this.glow = new Color(50, 0, 30);
          this.millisecondsDuration = 8000;
          break;
        case 19:
          this.description = Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.466") + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.467");
          this.buffAttributes[9] = -8;
          this.glow = Color.LightBlue;
          this.millisecondsDuration = 2000;
          break;
        case 20:
          this.description = Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.468") + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.469");
          this.buffAttributes[11] = 10;
          this.glow = Color.Red;
          this.millisecondsDuration = 5000;
          flag = false;
          break;
        case 21:
          this.description = Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.470") + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.471");
          this.glow = Color.Orange;
          this.millisecondsDuration = 5000;
          flag = false;
          break;
        case 22:
          this.description = Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.472") + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.473");
          this.glow = Color.Cyan;
          this.millisecondsDuration = 3000;
          this.sheetIndex = 9;
          this.buffAttributes[9] = 2;
          flag = false;
          break;
        case 23:
          this.description = Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.474") + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.475");
          this.glow = Color.LightGreen * 0.25f;
          this.millisecondsDuration = 600000;
          flag = false;
          break;
      }
      if (!flag || !Game1.player.isWearingRing(525))
        return;
      this.millisecondsDuration = this.millisecondsDuration / 2;
    }

    public Buff(int farming, int fishing, int mining, int digging, int luck, int foraging, int crafting, int maxStamina, int magneticRadius, int speed, int defense, int attack, int minutesDuration, string source, string displaySource)
    {
      this.buffAttributes[0] = farming;
      this.buffAttributes[1] = fishing;
      this.buffAttributes[2] = mining;
      this.buffAttributes[4] = luck;
      this.buffAttributes[5] = foraging;
      this.buffAttributes[6] = crafting;
      this.buffAttributes[7] = maxStamina;
      this.buffAttributes[8] = magneticRadius;
      this.buffAttributes[9] = speed;
      this.buffAttributes[10] = defense;
      this.buffAttributes[11] = attack;
      this.total = Math.Abs(this.buffAttributes[0]) + Math.Abs(this.buffAttributes[2]) + Math.Abs(this.buffAttributes[1]) + Math.Abs(this.buffAttributes[4]) + Math.Abs(this.buffAttributes[5]) + Math.Abs(this.buffAttributes[6]) + Math.Abs(this.buffAttributes[7]) + Math.Abs(this.buffAttributes[8]) + Math.Abs(this.buffAttributes[9]) + Math.Abs(this.buffAttributes[10]) + Math.Abs(this.buffAttributes[11]);
      this.millisecondsDuration = Math.Min(2400 - Game1.timeOfDay, minutesDuration) / 10 * 7000;
      this.source = source;
      this.displaySource = displaySource;
    }

    public string getTimeLeft()
    {
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.476") + (object) (this.millisecondsDuration / 60000) + ":" + (object) (this.millisecondsDuration % 60000 / 10000) + (object) (this.millisecondsDuration % 60000 % 10000 / 1000);
    }

    public bool update(GameTime time)
    {
      int millisecondsDuration = this.millisecondsDuration;
      this.millisecondsDuration = this.millisecondsDuration - time.ElapsedGameTime.Milliseconds;
      if (this.which == 13 && millisecondsDuration % 500 < this.millisecondsDuration % 500 && millisecondsDuration < 3000)
        Game1.player.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(44, Game1.player.getStandingPosition() + new Vector2((float) (-Game1.tileSize / 2 - Game1.pixelZoom * 2 + Game1.random.Next(-Game1.pixelZoom * 2, Game1.pixelZoom * 3)), (float) Game1.random.Next(-Game1.tileSize / 2, -Game1.tileSize / 4)), Color.Green * 0.5f, 8, Game1.random.NextDouble() < 0.5, 70f, 0, -1, -1f, -1, 0)
        {
          scale = 1f
        });
      return this.millisecondsDuration <= 0;
    }

    public void addBuff()
    {
      Game1.player.addedFarmingLevel += this.buffAttributes[0];
      Game1.player.addedFishingLevel += this.buffAttributes[1];
      Game1.player.addedMiningLevel += this.buffAttributes[2];
      Game1.player.addedLuckLevel += this.buffAttributes[4];
      Game1.player.addedForagingLevel += this.buffAttributes[5];
      Game1.player.CraftingTime -= this.buffAttributes[6];
      Game1.player.MaxStamina += this.buffAttributes[7];
      Game1.player.MagneticRadius += this.buffAttributes[8];
      Game1.player.resilience += this.buffAttributes[10];
      Game1.player.attack += this.buffAttributes[11];
      Game1.player.addedSpeed += this.buffAttributes[9];
      Color glow = this.glow;
      if (this.glow.Equals(Color.White))
        return;
      Game1.player.startGlowing(this.glow, false, 0.05f);
    }

    public string getDescription(int which)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (this.description != null && this.description.Length > 1)
      {
        stringBuilder.AppendLine(this.description);
      }
      else
      {
        if (which == 0)
        {
          if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.es)
            stringBuilder.AppendLine(Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.480") + (this.buffAttributes[0] > 0 ? (object) "+" : (object) "-") + (object) this.buffAttributes[0]);
          else
            stringBuilder.AppendLine((this.buffAttributes[0] > 0 ? (object) "+" : (object) "-").ToString() + (object) this.buffAttributes[0] + Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.480"));
        }
        if (which == 1)
        {
          if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.es)
            stringBuilder.AppendLine(Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.483") + (this.buffAttributes[1] > 0 ? (object) "+" : (object) "-") + (object) this.buffAttributes[1]);
          else
            stringBuilder.AppendLine((this.buffAttributes[1] > 0 ? (object) "+" : (object) "-").ToString() + (object) this.buffAttributes[1] + Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.483"));
        }
        if (which == 2)
        {
          if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.es)
            stringBuilder.AppendLine(Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.486") + (this.buffAttributes[2] > 0 ? (object) "+" : (object) "-") + (object) this.buffAttributes[2]);
          else
            stringBuilder.AppendLine((this.buffAttributes[2] > 0 ? (object) "+" : (object) "-").ToString() + (object) this.buffAttributes[2] + Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.486"));
        }
        if (which == 4)
        {
          if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.es)
            stringBuilder.AppendLine(Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.489") + (this.buffAttributes[4] > 0 ? (object) "+" : (object) "-") + (object) this.buffAttributes[4]);
          else
            stringBuilder.AppendLine((this.buffAttributes[4] > 0 ? (object) "+" : (object) "-").ToString() + (object) this.buffAttributes[4] + Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.489"));
        }
        if (which == 5)
        {
          if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.es)
            stringBuilder.AppendLine(Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.492") + (this.buffAttributes[5] > 0 ? (object) "+" : (object) "-") + (object) this.buffAttributes[5]);
          else
            stringBuilder.AppendLine((this.buffAttributes[5] > 0 ? (object) "+" : (object) "-").ToString() + (object) this.buffAttributes[5] + Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.492"));
        }
        if (which == 7)
        {
          if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.es)
            stringBuilder.AppendLine(Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.495") + (this.buffAttributes[7] > 0 ? (object) "+" : (object) "-") + (object) this.buffAttributes[7]);
          else
            stringBuilder.AppendLine((this.buffAttributes[7] > 0 ? (object) "+" : (object) "-").ToString() + (object) this.buffAttributes[7] + Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.495"));
        }
        if (which == 8)
        {
          if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.es)
            stringBuilder.AppendLine(Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.498") + (this.buffAttributes[8] > 0 ? (object) "+" : (object) "-") + (object) this.buffAttributes[8]);
          else
            stringBuilder.AppendLine((this.buffAttributes[8] > 0 ? (object) "+" : (object) "-").ToString() + (object) this.buffAttributes[8] + Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.498"));
        }
        if (which == 10)
        {
          if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.es)
            stringBuilder.AppendLine(Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.501") + (this.buffAttributes[10] > 0 ? (object) "+" : (object) "-") + (object) this.buffAttributes[10]);
          else
            stringBuilder.AppendLine((this.buffAttributes[10] > 0 ? (object) "+" : (object) "-").ToString() + (object) this.buffAttributes[10] + Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.501"));
        }
        if (which == 11)
        {
          if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.es)
            stringBuilder.AppendLine(Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.504") + (this.buffAttributes[11] > 0 ? (object) "+" : (object) "-") + (object) this.buffAttributes[11]);
          else
            stringBuilder.AppendLine((this.buffAttributes[11] > 0 ? (object) "+" : (object) "-").ToString() + (object) this.buffAttributes[11] + Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.504"));
        }
        if (which == 9)
        {
          if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.es)
            stringBuilder.AppendLine(Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.507") + (this.buffAttributes[9] > 0 ? (object) "+" : (object) "-") + (object) this.buffAttributes[9]);
          else
            stringBuilder.AppendLine((this.buffAttributes[9] > 0 ? (object) "+" : (object) "-").ToString() + (object) this.buffAttributes[9] + Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.507"));
        }
      }
      if (this.source != null && !this.source.Equals(""))
        stringBuilder.AppendLine(Game1.content.LoadString("Strings\\StringsFromCSFiles:Buff.cs.508") + this.displaySource);
      return stringBuilder.ToString();
    }

    public bool betterThan(Buff other)
    {
      return this.total > 0 && (other == null || this.total > other.total);
    }

    public void removeBuff()
    {
      if (this.buffAttributes[0] != 0)
        Game1.player.addedFarmingLevel = Math.Max(0, Game1.player.addedFarmingLevel - this.buffAttributes[0]);
      if (this.buffAttributes[1] != 0)
        Game1.player.addedFishingLevel = Math.Max(0, Game1.player.addedFishingLevel - this.buffAttributes[1]);
      if (this.buffAttributes[2] != 0)
        Game1.player.addedMiningLevel = Math.Max(0, Game1.player.addedMiningLevel - this.buffAttributes[2]);
      if (this.buffAttributes[4] != 0)
        Game1.player.addedLuckLevel = Math.Max(0, Game1.player.addedLuckLevel - this.buffAttributes[4]);
      if (this.buffAttributes[5] != 0)
        Game1.player.addedForagingLevel = Math.Max(0, Game1.player.addedForagingLevel - this.buffAttributes[5]);
      if (this.buffAttributes[6] != 0)
        Game1.player.CraftingTime = Math.Max(0, Game1.player.CraftingTime - this.buffAttributes[6]);
      if (this.buffAttributes[7] != 0)
        Game1.player.MaxStamina = Math.Max(0, Game1.player.MaxStamina - this.buffAttributes[7]);
      if (this.buffAttributes[8] != 0)
        Game1.player.MagneticRadius = Math.Max(0, Game1.player.MagneticRadius - this.buffAttributes[8]);
      if (this.buffAttributes[10] != 0)
        Game1.player.resilience = Math.Max(0, Game1.player.resilience - this.buffAttributes[10]);
      if (this.buffAttributes[9] != 0)
      {
        if (this.buffAttributes[9] < 0)
          Game1.player.addedSpeed += Math.Abs(this.buffAttributes[9]);
        else
          Game1.player.addedSpeed -= this.buffAttributes[9];
      }
      if (this.buffAttributes[11] != 0)
      {
        if (this.buffAttributes[11] < 0)
          Game1.player.attack += Math.Abs(this.buffAttributes[11]);
        else
          Game1.player.attack -= this.buffAttributes[11];
      }
      Color glow1 = this.glow;
      if (this.glow.Equals(Color.White))
        return;
      Game1.player.stopGlowing();
      foreach (Buff otherBuff in Game1.buffsDisplay.otherBuffs)
      {
        if (!otherBuff.Equals((object) this))
        {
          Color glow2 = otherBuff.glow;
          if (!otherBuff.glow.Equals(Color.White))
            Game1.player.startGlowing(otherBuff.glow, false, 0.05f);
        }
      }
    }

    public List<ClickableTextureComponent> getClickableComponents()
    {
      Dictionary<int, int> dictionary = new Dictionary<int, int>();
      if (this.sheetIndex != -1)
      {
        dictionary.Add(this.sheetIndex, 0);
      }
      else
      {
        if (this.buffAttributes[0] != 0)
          dictionary.Add(0, this.buffAttributes[0]);
        if (this.buffAttributes[1] != 0)
          dictionary.Add(1, this.buffAttributes[1]);
        if (this.buffAttributes[2] != 0)
          dictionary.Add(2, this.buffAttributes[2]);
        if (this.buffAttributes[4] != 0)
          dictionary.Add(4, this.buffAttributes[4]);
        if (this.buffAttributes[5] != 0)
          dictionary.Add(5, this.buffAttributes[5]);
        if (this.buffAttributes[7] != 0)
          dictionary.Add(16, this.buffAttributes[7]);
        if (this.buffAttributes[11] != 0)
          dictionary.Add(11, this.buffAttributes[11]);
        if (this.buffAttributes[8] != 0)
          dictionary.Add(8, this.buffAttributes[8]);
        if (this.buffAttributes[10] != 0)
          dictionary.Add(10, this.buffAttributes[10]);
        if (this.buffAttributes[9] != 0)
          dictionary.Add(9, this.buffAttributes[9]);
      }
      List<ClickableTextureComponent> textureComponentList = new List<ClickableTextureComponent>();
      foreach (KeyValuePair<int, int> keyValuePair in dictionary)
        textureComponentList.Add(new ClickableTextureComponent("", Rectangle.Empty, (string) null, this.getDescription(Buff.getAttributeIndexFromSourceRectIndex(keyValuePair.Key)), Game1.buffsIcons, Game1.getSourceRectForStandardTileSheet(Game1.buffsIcons, keyValuePair.Key, 16, 16), (float) Game1.pixelZoom, false));
      return textureComponentList;
    }

    public static int getAttributeIndexFromSourceRectIndex(int index)
    {
      if (index == 16)
        return 7;
      return index;
    }

    public static string getBuffTypeFromBuffDescriptionIndex(int index)
    {
      string str = "";
      switch (index)
      {
        case 0:
          str = "farming";
          break;
        case 1:
          str = "fishing";
          break;
        case 2:
          str = "mining";
          break;
        case 3:
          str = "digging";
          break;
        case 4:
          str = "luck";
          break;
        case 5:
          str = "foraging";
          break;
        case 6:
          str = "crafting speed";
          break;
        case 7:
          str = "max energy";
          break;
        case 8:
          str = "magnetism";
          break;
        case 9:
          str = "speed";
          break;
        case 10:
          str = "defense";
          break;
        case 11:
          str = "attack";
          break;
      }
      return str;
    }
  }
}
