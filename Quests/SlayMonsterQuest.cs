// Decompiled with JetBrains decompiler
// Type: StardewValley.Quests.SlayMonsterQuest
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using StardewValley.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Quests
{
  public class SlayMonsterQuest : Quest
  {
    public List<DescriptionElement> parts = new List<DescriptionElement>();
    public List<DescriptionElement> dialogueparts = new List<DescriptionElement>();
    public string targetMessage;
    public string monsterName;
    public string target;
    public Monster monster;
    public NPC actualTarget;
    public int numberToKill;
    public int reward;
    public int numberKilled;
    public DescriptionElement objective;

    public SlayMonsterQuest()
    {
      this.questType = 4;
    }

    public void loadQuestInfo()
    {
      if (this.target != null && this.monster != null)
        return;
      this.questTitle = Game1.content.LoadString("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13696");
      List<string> source = new List<string>();
      int deepestMineLevel = Game1.player.deepestMineLevel;
      if (deepestMineLevel < 39)
      {
        source.Add("Green Slime");
        if (deepestMineLevel > 10)
          source.Add("Rock Crab");
        if (deepestMineLevel > 30)
          source.Add("Duggy");
      }
      else if (deepestMineLevel < 79)
      {
        source.Add("Frost Jelly");
        source.Add("Skeleton");
        source.Add("Dust Spirit");
      }
      else
      {
        source.Add("Sludge");
        source.Add("Ghost");
        source.Add("Lava Crab");
        source.Add("Squid Kid");
      }
      int num = this.monsterName == null ? 1 : 0;
      if (num != 0)
        this.monsterName = source.ElementAt<string>(this.random.Next(source.Count));
      if (this.monsterName == "Frost Jelly" || this.monsterName == "Sludge")
      {
        this.monster = new Monster("Green Slime", Vector2.Zero);
        this.monster.name = this.monsterName;
      }
      else
        this.monster = new Monster(this.monsterName, Vector2.Zero);
      if (num != 0)
      {
        string monsterName = this.monsterName;
        // ISSUE: reference to a compiler-generated method
        uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(monsterName);
        if (stringHash <= 703662834U)
        {
          if (stringHash <= 503018864U)
          {
            if ((int) stringHash != 165007071)
            {
              if ((int) stringHash == 503018864 && monsterName == "Ghost")
              {
                this.numberToKill = this.random.Next(1, 3);
                this.reward = this.numberToKill * 250;
              }
            }
            else if (monsterName == "Lava Crab")
            {
              this.numberToKill = this.random.Next(2, 6);
              this.reward = this.numberToKill * 180;
            }
          }
          else if ((int) stringHash != 510600819)
          {
            if ((int) stringHash == 703662834 && monsterName == "Rock Crab")
            {
              this.numberToKill = this.random.Next(2, 6);
              this.reward = this.numberToKill * 75;
            }
          }
          else if (monsterName == "Duggy")
          {
            this.parts.Clear();
            this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13711", (object) this.numberToKill));
            this.target = "Clint";
            this.numberToKill = this.random.Next(2, 4);
            this.reward = this.numberToKill * 150;
          }
        }
        else if (stringHash <= 1114282268U)
        {
          if ((int) stringHash != 1104688147)
          {
            if ((int) stringHash == 1114282268 && monsterName == "Green Slime")
            {
              this.numberToKill = this.random.Next(4, 9);
              this.numberToKill = this.numberToKill - this.numberToKill % 2;
              this.reward = this.numberToKill * 60;
            }
          }
          else if (monsterName == "Sludge")
          {
            this.numberToKill = this.random.Next(4, 9);
            this.numberToKill = this.numberToKill - this.numberToKill % 2;
            this.reward = this.numberToKill * 125;
          }
        }
        else if ((int) stringHash != 2124830350)
        {
          if ((int) stringHash != -2071440691)
          {
            if ((int) stringHash == -1169118115 && monsterName == "Frost Jelly")
            {
              this.numberToKill = this.random.Next(4, 9);
              this.numberToKill = this.numberToKill - this.numberToKill % 2;
              this.reward = this.numberToKill * 85;
            }
          }
          else if (monsterName == "Squid Kid")
          {
            this.numberToKill = this.random.Next(1, 3);
            this.reward = this.numberToKill * 350;
          }
        }
        else if (monsterName == "Skeleton")
        {
          this.numberToKill = this.random.Next(1, 4);
          this.reward = this.numberToKill * 120;
        }
      }
      if (this.monsterName.Equals("Green Slime") || this.monsterName.Equals("Frost Jelly") || this.monsterName.Equals("Sludge"))
      {
        this.parts.Clear();
        this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13723", (object) this.numberToKill, this.monsterName.Equals("Frost Jelly") ? (object) new DescriptionElement("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13725") : (this.monsterName.Equals("Sludge") ? (object) new DescriptionElement("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13727") : (object) new DescriptionElement("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13728"))));
        this.target = "Lewis";
        this.dialogueparts.Clear();
        this.dialogueparts.Add((DescriptionElement) "Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13730");
        if (this.random.NextDouble() < 0.5)
        {
          this.dialogueparts.Add((DescriptionElement) "Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13731");
          this.dialogueparts.Add((DescriptionElement) (this.random.NextDouble() < 0.5 ? "Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13732" : "Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13733"));
          this.dialogueparts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13734", this.random.NextDouble() < 0.5 ? (object) new DescriptionElement("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13735") : (object) new DescriptionElement("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13736"), (object) ((IEnumerable<DescriptionElement>) new DescriptionElement[16]
          {
            (DescriptionElement) "Strings\\StringsFromCSFiles:Dialogue.cs.795",
            (DescriptionElement) "Strings\\StringsFromCSFiles:Dialogue.cs.796",
            (DescriptionElement) "Strings\\StringsFromCSFiles:Dialogue.cs.797",
            (DescriptionElement) "Strings\\StringsFromCSFiles:Dialogue.cs.798",
            (DescriptionElement) "Strings\\StringsFromCSFiles:Dialogue.cs.799",
            (DescriptionElement) "Strings\\StringsFromCSFiles:Dialogue.cs.800",
            (DescriptionElement) "Strings\\StringsFromCSFiles:Dialogue.cs.801",
            (DescriptionElement) "Strings\\StringsFromCSFiles:Dialogue.cs.802",
            (DescriptionElement) "Strings\\StringsFromCSFiles:Dialogue.cs.803",
            (DescriptionElement) "Strings\\StringsFromCSFiles:Dialogue.cs.804",
            (DescriptionElement) "Strings\\StringsFromCSFiles:Dialogue.cs.805",
            (DescriptionElement) "Strings\\StringsFromCSFiles:Dialogue.cs.806",
            (DescriptionElement) "Strings\\StringsFromCSFiles:Dialogue.cs.807",
            (DescriptionElement) "Strings\\StringsFromCSFiles:Dialogue.cs.808",
            (DescriptionElement) "Strings\\StringsFromCSFiles:Dialogue.cs.809",
            (DescriptionElement) "Strings\\StringsFromCSFiles:Dialogue.cs.810"
          }).ElementAt<DescriptionElement>(this.random.Next(16)), this.random.NextDouble() < 0.3 ? (object) new DescriptionElement("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13740") : (this.random.NextDouble() < 0.5 ? (object) new DescriptionElement("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13741") : (object) new DescriptionElement("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13742"))));
        }
        else
          this.dialogueparts.Add((DescriptionElement) "Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13744");
      }
      else if (this.monsterName.Equals("Rock Crab") || this.monsterName.Equals("Lava Crab"))
      {
        this.parts.Clear();
        this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13747", (object) this.numberToKill));
        this.target = "Demetrius";
        this.dialogueparts.Clear();
        this.dialogueparts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13750", (object) this.monster));
      }
      else
      {
        this.parts.Clear();
        this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13752", (object) this.monster, (object) this.numberToKill, this.random.NextDouble() < 0.3 ? (object) new DescriptionElement("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13755") : (this.random.NextDouble() < 0.5 ? (object) new DescriptionElement("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13756") : (object) new DescriptionElement("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13757"))));
        this.target = "Wizard";
        this.dialogueparts.Clear();
        this.dialogueparts.Add((DescriptionElement) "Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13760");
      }
      if (this.target.Equals("Wizard") && !Game1.player.mailReceived.Contains("wizardJunimoNote") && !Game1.player.mailReceived.Contains("JojaMember"))
      {
        this.parts.Clear();
        this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13764", (object) this.numberToKill, (object) this.monster));
        this.target = "Lewis";
        this.dialogueparts.Clear();
        this.dialogueparts.Add((DescriptionElement) "Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13767");
      }
      this.actualTarget = Game1.getCharacterFromName(this.target, false);
      this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13274", (object) this.reward));
      this.objective = new DescriptionElement("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13770", (object) "0", (object) this.numberToKill, (object) this.monster);
    }

    public override void reloadDescription()
    {
      if (this._questDescription == "")
        this.loadQuestInfo();
      if (this.parts.Count == 0 || this.parts == null || (this.dialogueparts.Count == 0 || this.dialogueparts == null))
        return;
      string str1 = "";
      string str2 = "";
      foreach (DescriptionElement part in this.parts)
        str1 += part.loadDescriptionElement();
      foreach (DescriptionElement dialoguepart in this.dialogueparts)
        str2 += dialoguepart.loadDescriptionElement();
      this.questDescription = str1;
      this.targetMessage = str2;
    }

    public override void reloadObjective()
    {
      if (this.numberKilled == 0 && this.id != 0)
        return;
      if (this.numberKilled < this.numberToKill)
        this.objective = new DescriptionElement("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13770", (object) this.numberKilled, (object) this.numberToKill, (object) this.monster);
      if (this.objective == null)
        return;
      this.currentObjective = this.objective.loadDescriptionElement();
    }

    public override bool checkIfComplete(NPC n = null, int number1 = -1, int number2 = -1, Item item = null, string monsterName = null)
    {
      if (this.completed)
        return false;
      if (monsterName == null)
        monsterName = "Green Slime";
      if ((object) n == null && monsterName != null && (monsterName.Contains(this.monsterName) && this.numberKilled < this.numberToKill))
      {
        this.numberKilled = Math.Min(this.numberToKill, this.numberKilled + 1);
        if (this.numberKilled >= this.numberToKill)
        {
          if (this.target == null || this.target.Equals("null"))
          {
            this.questComplete();
          }
          else
          {
            if ((object) this.actualTarget == null)
              this.actualTarget = Game1.getCharacterFromName(this.target, false);
            this.objective = new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13277", (object) this.actualTarget);
            Game1.playSound("jingle1");
          }
        }
        else if (this.monster == null)
        {
          if (monsterName == "Frost Jelly" || monsterName == "Sludge")
          {
            this.monster = new Monster("Green Slime", Vector2.Zero);
            this.monster.name = monsterName;
          }
          else
            this.monster = new Monster(monsterName, Vector2.Zero);
        }
        Game1.dayTimeMoneyBox.moneyDial.animations.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(387, 497, 3, 8), 800f, 1, 0, Game1.dayTimeMoneyBox.position + new Vector2(228f, 244f), false, false, 1f, 0.01f, Color.White, 4f, 0.3f, 0.0f, 0.0f, false)
        {
          scaleChangeChange = -0.012f
        });
      }
      else if ((object) n != null && this.target != null && (!this.target.Equals("null") && this.numberKilled >= this.numberToKill) && (n.name.Equals(this.target) && n.isVillager()))
      {
        n.CurrentDialogue.Push(new Dialogue(this.targetMessage, n));
        this.moneyReward = this.reward;
        this.questComplete();
        Game1.drawDialogue(n);
        return true;
      }
      return false;
    }
  }
}
