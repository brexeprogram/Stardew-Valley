// Decompiled with JetBrains decompiler
// Type: StardewValley.Locations.AdventureGuild
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;
using StardewValley.Objects;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Text;
using xTile;
using xTile.Dimensions;

namespace StardewValley.Locations
{
  public class AdventureGuild : GameLocation
  {
    private NPC Gil = new NPC((AnimatedSprite) null, new Vector2(-1000f, -1000f), nameof (AdventureGuild), 2, nameof (Gil), false, (Dictionary<int, int[]>) null, Game1.content.Load<Texture2D>("Portraits\\Gil"));
    private bool talkedToGil;

    public AdventureGuild()
    {
    }

    public AdventureGuild(Map map, string name)
      : base(map, name)
    {
    }

    public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
    {
      switch (this.map.GetLayer("Buildings").Tiles[tileLocation] != null ? this.map.GetLayer("Buildings").Tiles[tileLocation].TileIndex : -1)
      {
        case 1306:
          this.showMonsterKillList();
          return true;
        case 1355:
        case 1356:
        case 1357:
        case 1358:
        case 1291:
        case 1292:
          this.gil();
          return true;
        default:
          return base.checkAction(tileLocation, viewport, who);
      }
    }

    public override void resetForPlayerEntry()
    {
      base.resetForPlayerEntry();
      this.talkedToGil = false;
      if (Game1.player.mailReceived.Contains("guildMember"))
        return;
      Game1.player.mailReceived.Add("guildMember");
    }

    public override void draw(SpriteBatch b)
    {
      base.draw(b);
      if (Game1.player.mailReceived.Contains("checkedMonsterBoard"))
        return;
      float num = (float) (4.0 * Math.Round(Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 250.0), 2));
      b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (8 * Game1.tileSize - 8), (float) (9 * Game1.tileSize - Game1.tileSize * 3 / 2 - 16) + num)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(141, 465, 20, 24)), Color.White * 0.75f, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, (float) ((double) (10 * Game1.tileSize) / 10000.0 + 9.99999997475243E-07 + 0.0007999999797903));
      b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (8 * Game1.tileSize + Game1.tileSize / 2), (float) (9 * Game1.tileSize - Game1.tileSize - Game1.tileSize / 8) + num)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(175, 425, 12, 12)), Color.White * 0.75f, 0.0f, new Vector2(6f, 6f), (float) Game1.pixelZoom, SpriteEffects.None, (float) ((double) (10 * Game1.tileSize) / 10000.0 + 9.99999974737875E-06 + 0.0007999999797903));
    }

    private string killListLine(string monsterType, int killCount, int target)
    {
      string str = Game1.content.LoadString("Strings\\Locations:AdventureGuild_KillList_" + monsterType);
      if (killCount == 0)
        return Game1.content.LoadString("Strings\\Locations:AdventureGuild_KillList_LineFormat_None", (object) killCount, (object) target, (object) str) + "^";
      if (killCount >= target)
        return Game1.content.LoadString("Strings\\Locations:AdventureGuild_KillList_LineFormat_OverTarget", (object) killCount, (object) target, (object) str) + "^";
      return Game1.content.LoadString("Strings\\Locations:AdventureGuild_KillList_LineFormat", (object) killCount, (object) target, (object) str) + "^";
    }

    public void showMonsterKillList()
    {
      if (!Game1.player.mailReceived.Contains("checkedMonsterBoard"))
        Game1.player.mailReceived.Add("checkedMonsterBoard");
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(Game1.content.LoadString("Strings\\Locations:AdventureGuild_KillList_Header").Replace('\n', '^') + "^");
      int killCount1 = Game1.stats.getMonstersKilled("Green Slime") + Game1.stats.getMonstersKilled("Frost Jelly") + Game1.stats.getMonstersKilled("Sludge");
      int killCount2 = Game1.stats.getMonstersKilled("Shadow Guy") + Game1.stats.getMonstersKilled("Shadow Shaman") + Game1.stats.getMonstersKilled("Shadow Brute");
      int killCount3 = Game1.stats.getMonstersKilled("Skeleton") + Game1.stats.getMonstersKilled("Skeleton Mage");
      int killCount4 = Game1.stats.getMonstersKilled("Grub") + Game1.stats.getMonstersKilled("Fly") + Game1.stats.getMonstersKilled("Bug");
      int killCount5 = Game1.stats.getMonstersKilled("Bat") + Game1.stats.getMonstersKilled("Frost Bat") + Game1.stats.getMonstersKilled("Lava Bat");
      int monstersKilled1 = Game1.stats.getMonstersKilled("Duggy");
      int monstersKilled2 = Game1.stats.getMonstersKilled("Dust Spirit");
      stringBuilder.Append(this.killListLine("Slimes", killCount1, 1000));
      stringBuilder.Append(this.killListLine("VoidSpirits", killCount2, 150));
      stringBuilder.Append(this.killListLine("Bats", killCount5, 200));
      stringBuilder.Append(this.killListLine("Skeletons", killCount3, 50));
      stringBuilder.Append(this.killListLine("CaveInsects", killCount4, 125));
      stringBuilder.Append(this.killListLine("Duggies", monstersKilled1, 30));
      stringBuilder.Append(this.killListLine("DustSprites", monstersKilled2, 500));
      stringBuilder.Append(Game1.content.LoadString("Strings\\Locations:AdventureGuild_KillList_Footer").Replace('\n', '^'));
      Game1.drawLetterMessage(stringBuilder.ToString());
    }

    public override void cleanupBeforePlayerExit()
    {
      base.cleanupBeforePlayerExit();
      Game1.changeMusicTrack("none");
    }

    public static bool areAllMonsterSlayerQuestsComplete()
    {
      int num1 = Game1.stats.getMonstersKilled("Green Slime") + Game1.stats.getMonstersKilled("Frost Jelly") + Game1.stats.getMonstersKilled("Sludge");
      int num2 = Game1.stats.getMonstersKilled("Shadow Guy") + Game1.stats.getMonstersKilled("Shadow Shaman") + Game1.stats.getMonstersKilled("Shadow Brute");
      int num3 = Game1.stats.getMonstersKilled("Skeleton") + Game1.stats.getMonstersKilled("Skeleton Mage");
      Game1.stats.getMonstersKilled("Rock Crab");
      Game1.stats.getMonstersKilled("Lava Crab");
      int num4 = Game1.stats.getMonstersKilled("Grub") + Game1.stats.getMonstersKilled("Fly") + Game1.stats.getMonstersKilled("Bug");
      int num5 = Game1.stats.getMonstersKilled("Bat") + Game1.stats.getMonstersKilled("Frost Bat") + Game1.stats.getMonstersKilled("Lava Bat");
      int monstersKilled1 = Game1.stats.getMonstersKilled("Duggy");
      Game1.stats.getMonstersKilled("Metal Head");
      Game1.stats.getMonstersKilled("Stone Golem");
      int monstersKilled2 = Game1.stats.getMonstersKilled("Dust Spirit");
      int num6 = 1000;
      return num1 >= num6 && num2 >= 150 && (num3 >= 50 && num4 >= 125) && (num5 >= 200 && monstersKilled1 >= 30 && monstersKilled2 >= 500);
    }

    public static bool willThisKillCompleteAMonsterSlayerQuest(string nameOfMonster)
    {
      int num1 = Game1.stats.getMonstersKilled("Green Slime") + Game1.stats.getMonstersKilled("Frost Jelly") + Game1.stats.getMonstersKilled("Sludge");
      int num2 = Game1.stats.getMonstersKilled("Shadow Guy") + Game1.stats.getMonstersKilled("Shadow Shaman") + Game1.stats.getMonstersKilled("Shadow Brute");
      int num3 = Game1.stats.getMonstersKilled("Skeleton") + Game1.stats.getMonstersKilled("Skeleton Mage");
      int num4 = Game1.stats.getMonstersKilled("Rock Crab") + Game1.stats.getMonstersKilled("Lava Crab");
      int num5 = Game1.stats.getMonstersKilled("Grub") + Game1.stats.getMonstersKilled("Fly") + Game1.stats.getMonstersKilled("Bug");
      int num6 = Game1.stats.getMonstersKilled("Bat") + Game1.stats.getMonstersKilled("Frost Bat") + Game1.stats.getMonstersKilled("Lava Bat");
      int monstersKilled1 = Game1.stats.getMonstersKilled("Duggy");
      Game1.stats.getMonstersKilled("Metal Head");
      Game1.stats.getMonstersKilled("Stone Golem");
      int monstersKilled2 = Game1.stats.getMonstersKilled("Dust Spirit");
      int num7 = nameOfMonster.Equals("Green Slime") || nameOfMonster.Equals("Frost Jelly") || nameOfMonster.Equals("Sludge") ? 1 : 0;
      int num8 = num1 + num7;
      int num9 = num2 + (nameOfMonster.Equals("Shadow Guy") || nameOfMonster.Equals("Shadow Shaman") || nameOfMonster.Equals("Shadow Brute") ? 1 : 0);
      int num10 = num3 + (nameOfMonster.Equals("Skeleton") || nameOfMonster.Equals("Skeleton Mage") ? 1 : 0);
      if (!nameOfMonster.Equals("Rock Crab"))
        nameOfMonster.Equals("Lava Crab");
      int num11 = num5 + (nameOfMonster.Equals("Grub") || nameOfMonster.Equals("Fly") || nameOfMonster.Equals("Bug") ? 1 : 0);
      int num12 = num6 + (nameOfMonster.Equals("Bat") || nameOfMonster.Equals("Frost Bat") || nameOfMonster.Equals("Lava Bat") ? 1 : 0);
      int num13 = monstersKilled1 + (nameOfMonster.Equals("Duggy") ? 1 : 0);
      nameOfMonster.Equals("Metal Head");
      nameOfMonster.Equals("Stone Golem");
      int num14 = monstersKilled2 + (nameOfMonster.Equals("Dust Spirit") ? 1 : 0);
      int num15 = 1000;
      return num1 < num15 && num8 >= 1000 && !Game1.player.mailReceived.Contains("Gil_Slime Charmer Ring") || num2 < 150 && num9 >= 150 && !Game1.player.mailReceived.Contains("Gil_Savage Ring") || (num3 < 50 && num10 >= 50 && !Game1.player.mailReceived.Contains("Gil_Skeleton Mask") || num5 < 125 && num11 >= 125 && !Game1.player.mailReceived.Contains("Gil_Insect Head")) || (num6 < 200 && num12 >= 200 && !Game1.player.mailReceived.Contains("Gil_Vampire Ring") || monstersKilled1 < 30 && num13 >= 30 && !Game1.player.mailReceived.Contains("Gil_Hard Hat") || monstersKilled2 < 500 && num14 >= 500 && !Game1.player.mailReceived.Contains("Gil_Burglar's Ring"));
    }

    private void gil()
    {
      List<Item> inventory = new List<Item>();
      int num1 = Game1.stats.getMonstersKilled("Green Slime") + Game1.stats.getMonstersKilled("Frost Jelly") + Game1.stats.getMonstersKilled("Sludge");
      int num2 = Game1.stats.getMonstersKilled("Shadow Guy") + Game1.stats.getMonstersKilled("Shadow Shaman") + Game1.stats.getMonstersKilled("Shadow Brute");
      int num3 = Game1.stats.getMonstersKilled("Skeleton") + Game1.stats.getMonstersKilled("Skeleton Mage");
      int num4 = Game1.stats.getMonstersKilled("Goblin Warrior") + Game1.stats.getMonstersKilled("Goblin Wizard");
      int num5 = Game1.stats.getMonstersKilled("Rock Crab") + Game1.stats.getMonstersKilled("Lava Crab");
      int num6 = Game1.stats.getMonstersKilled("Grub") + Game1.stats.getMonstersKilled("Fly") + Game1.stats.getMonstersKilled("Bug");
      int num7 = Game1.stats.getMonstersKilled("Bat") + Game1.stats.getMonstersKilled("Frost Bat") + Game1.stats.getMonstersKilled("Lava Bat");
      int monstersKilled1 = Game1.stats.getMonstersKilled("Duggy");
      int monstersKilled2 = Game1.stats.getMonstersKilled("Metal Head");
      int monstersKilled3 = Game1.stats.getMonstersKilled("Stone Golem");
      int monstersKilled4 = Game1.stats.getMonstersKilled("Dust Spirit");
      int num8 = 1000;
      if (num1 >= num8 && !Game1.player.mailReceived.Contains("Gil_Slime Charmer Ring"))
        inventory.Add((Item) new Ring(520));
      if (num2 >= 150 && !Game1.player.mailReceived.Contains("Gil_Savage Ring"))
        inventory.Add((Item) new Ring(523));
      if (num3 >= 50 && !Game1.player.mailReceived.Contains("Gil_Skeleton Mask"))
        inventory.Add((Item) new Hat(8));
      if (num4 >= 50)
        Game1.player.specialItems.Contains(9);
      if (num5 >= 60)
        Game1.player.specialItems.Contains(524);
      if (num6 >= 125 && !Game1.player.mailReceived.Contains("Gil_Insect Head"))
        inventory.Add((Item) new MeleeWeapon(13));
      if (num7 >= 200 && !Game1.player.mailReceived.Contains("Gil_Vampire Ring"))
        inventory.Add((Item) new Ring(522));
      if (monstersKilled1 >= 30 && !Game1.player.mailReceived.Contains("Gil_Hard Hat"))
        inventory.Add((Item) new Hat(27));
      if (monstersKilled2 >= 50)
        Game1.player.specialItems.Contains(519);
      if (monstersKilled3 >= 50)
        Game1.player.specialItems.Contains(517);
      if (monstersKilled4 >= 500 && !Game1.player.mailReceived.Contains("Gil_Burglar's Ring"))
        inventory.Add((Item) new Ring(526));
      foreach (Item obj in inventory)
      {
        if (obj is StardewValley.Object)
          (obj as StardewValley.Object).specialItem = true;
        else if (!Game1.player.hasOrWillReceiveMail("Gil_" + obj.Name))
          Game1.player.mailReceived.Add("Gil_" + obj.Name);
      }
      if (inventory.Count > 0)
      {
        Game1.activeClickableMenu = (IClickableMenu) new ItemGrabMenu(inventory);
      }
      else
      {
        if (this.talkedToGil)
          Game1.drawDialogue(this.Gil, Game1.content.LoadString("Characters\\Dialogue\\Gil:Snoring"));
        else
          Game1.drawDialogue(this.Gil, Game1.content.LoadString("Characters\\Dialogue\\Gil:ComeBackLater"));
        this.talkedToGil = true;
      }
    }
  }
}
