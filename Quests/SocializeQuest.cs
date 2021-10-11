// Decompiled with JetBrains decompiler
// Type: StardewValley.Quests.SocializeQuest
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace StardewValley.Quests
{
  public class SocializeQuest : Quest
  {
    public List<DescriptionElement> parts = new List<DescriptionElement>();
    public List<string> whoToGreet;
    public int total;
    public DescriptionElement objective;

    public SocializeQuest()
    {
      this.questType = 5;
    }

    public void loadQuestInfo()
    {
      if (this.whoToGreet != null)
        return;
      this.whoToGreet = new List<string>();
      this.questTitle = Game1.content.LoadString("Strings\\StringsFromCSFiles:SocializeQuest.cs.13785");
      this.parts.Clear();
      this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:SocializeQuest.cs.13786", this.random.NextDouble() < 0.3 ? (object) new DescriptionElement("Strings\\StringsFromCSFiles:SocializeQuest.cs.13787") : (this.random.NextDouble() < 0.5 ? (object) new DescriptionElement("Strings\\StringsFromCSFiles:SocializeQuest.cs.13788") : (object) new DescriptionElement("Strings\\StringsFromCSFiles:SocializeQuest.cs.13789"))));
      this.parts.Add((DescriptionElement) "Strings\\StringsFromCSFiles:SocializeQuest.cs.13791");
      foreach (GameLocation location in Game1.locations)
      {
        foreach (NPC character in location.characters)
        {
          if (!character.isInvisible && !character.name.Contains("Qi") && (!character.name.Contains("???") && !character.name.Equals("Sandy")) && (!character.name.Contains("Dwarf") && !character.name.Contains("Gunther") && (!character.name.Contains("Mariner") && !character.name.Contains("Henchman"))) && (!character.name.Contains("Marlon") && !character.name.Contains("Wizard") && (!character.name.Contains("Bouncer") && !character.name.Contains("Krobus")) && character.isVillager()))
            this.whoToGreet.Add(character.name);
        }
      }
      this.objective = new DescriptionElement("Strings\\StringsFromCSFiles:SocializeQuest.cs.13802", (object) "2", (object) this.whoToGreet.Count);
      this.total = this.whoToGreet.Count;
      this.whoToGreet.Remove("Lewis");
      this.whoToGreet.Remove("Robin");
    }

    public override void reloadDescription()
    {
      if (this._questDescription == "")
        this.loadQuestInfo();
      if (this.parts.Count == 0 || this.parts == null)
        return;
      string str = "";
      foreach (DescriptionElement part in this.parts)
        str += part.loadDescriptionElement();
      this.questDescription = str;
    }

    public override void reloadObjective()
    {
      this.loadQuestInfo();
      if (this.objective == null && this.whoToGreet.Count > 0)
        this.objective = new DescriptionElement("Strings\\StringsFromCSFiles:SocializeQuest.cs.13802", (object) (this.total - this.whoToGreet.Count), (object) this.total);
      if (this.objective == null)
        return;
      this.currentObjective = this.objective.loadDescriptionElement();
    }

    public override bool checkIfComplete(NPC npc = null, int number1 = -1, int number2 = -1, Item item = null, string monsterName = null)
    {
      this.loadQuestInfo();
      if (npc != null && this.whoToGreet.Remove(npc.name))
        Game1.dayTimeMoneyBox.moneyDial.animations.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(387, 497, 3, 8), 800f, 1, 0, Game1.dayTimeMoneyBox.position + new Vector2(228f, 244f), false, false, 1f, 0.01f, Color.White, 4f, 0.3f, 0.0f, 0.0f, false)
        {
          scaleChangeChange = -0.012f
        });
      if (this.whoToGreet.Count == 0 && !this.completed)
      {
        foreach (string key in Game1.player.friendships.Keys)
        {
          if (Game1.player.friendships[key][0] < 2729)
            Game1.player.friendships[key][0] += 100;
        }
        this.questComplete();
        return true;
      }
      this.objective = new DescriptionElement("Strings\\StringsFromCSFiles:SocializeQuest.cs.13802", (object) (this.total - this.whoToGreet.Count), (object) this.total);
      return false;
    }
  }
}
