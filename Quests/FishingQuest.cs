// Decompiled with JetBrains decompiler
// Type: StardewValley.Quests.FishingQuest
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Quests
{
  public class FishingQuest : Quest
  {
    public List<DescriptionElement> parts = new List<DescriptionElement>();
    public List<DescriptionElement> dialogueparts = new List<DescriptionElement>();
    public string target;
    public string targetMessage;
    public int numberToFish;
    public int reward;
    public int numberFished;
    public int whichFish;
    public StardewValley.Object fish;
    public DescriptionElement objective;

    public FishingQuest()
    {
      this.questType = 7;
    }

    public void loadQuestInfo()
    {
      if (this.target != null && this.fish != null)
        return;
      this.questTitle = Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingQuest.cs.13227");
      if (this.random.NextDouble() < 0.5)
      {
        string currentSeason = Game1.currentSeason;
        if (!(currentSeason == "spring"))
        {
          if (!(currentSeason == "summer"))
          {
            if (!(currentSeason == "fall"))
            {
              if (currentSeason == "winter")
              {
                int[] numArray = new int[10]
                {
                  130,
                  131,
                  136,
                  141,
                  143,
                  144,
                  146,
                  147,
                  150,
                  151
                };
                this.whichFish = numArray[this.random.Next(numArray.Length)];
              }
            }
            else
            {
              int[] numArray = new int[8]
              {
                129,
                131,
                136,
                137,
                139,
                142,
                143,
                150
              };
              this.whichFish = numArray[this.random.Next(numArray.Length)];
            }
          }
          else
          {
            int[] numArray = new int[10]
            {
              130,
              131,
              136,
              138,
              142,
              144,
              145,
              146,
              149,
              150
            };
            this.whichFish = numArray[this.random.Next(numArray.Length)];
          }
        }
        else
        {
          int[] numArray = new int[8]
          {
            129,
            131,
            136,
            137,
            142,
            143,
            145,
            147
          };
          this.whichFish = numArray[this.random.Next(numArray.Length)];
        }
        this.fish = new StardewValley.Object(Vector2.Zero, this.whichFish, 1);
        this.numberToFish = (int) Math.Ceiling(90.0 / (double) Math.Max(1, this.fish.price)) + Game1.player.FishingLevel / 5;
        this.reward = this.numberToFish * this.fish.price;
        this.target = "Demetrius";
        this.parts.Clear();
        this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13228", (object) this.fish, (object) this.numberToFish));
        this.dialogueparts.Clear();
        this.dialogueparts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13231", (object) this.fish, (object) ((IEnumerable<DescriptionElement>) new DescriptionElement[4]
        {
          (DescriptionElement) "Strings\\StringsFromCSFiles:FishingQuest.cs.13233",
          (DescriptionElement) "Strings\\StringsFromCSFiles:FishingQuest.cs.13234",
          (DescriptionElement) "Strings\\StringsFromCSFiles:FishingQuest.cs.13235",
          new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13236", (object) this.fish)
        }).ElementAt<DescriptionElement>(this.random.Next(4))));
        this.objective = this.fish.name.Equals("Octopus") ? new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13243", (object) 0, (object) this.numberToFish) : new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13244", (object) 0, (object) this.numberToFish, (object) this.fish);
      }
      else
      {
        string currentSeason = Game1.currentSeason;
        if (!(currentSeason == "spring"))
        {
          if (!(currentSeason == "summer"))
          {
            if (!(currentSeason == "fall"))
            {
              if (currentSeason == "winter")
              {
                int[] numArray = new int[13]
                {
                  130,
                  131,
                  136,
                  141,
                  143,
                  144,
                  146,
                  147,
                  150,
                  151,
                  699,
                  702,
                  705
                };
                this.whichFish = numArray[this.random.Next(numArray.Length)];
              }
            }
            else
            {
              int[] numArray = new int[11]
              {
                129,
                131,
                136,
                137,
                139,
                142,
                143,
                150,
                699,
                702,
                705
              };
              this.whichFish = numArray[this.random.Next(numArray.Length)];
            }
          }
          else
          {
            int[] numArray = new int[12]
            {
              128,
              130,
              131,
              136,
              138,
              142,
              144,
              145,
              146,
              149,
              150,
              702
            };
            this.whichFish = numArray[this.random.Next(numArray.Length)];
          }
        }
        else
        {
          int[] numArray = new int[9]
          {
            129,
            131,
            136,
            137,
            142,
            143,
            145,
            147,
            702
          };
          this.whichFish = numArray[this.random.Next(numArray.Length)];
        }
        this.target = "Willy";
        this.fish = new StardewValley.Object(Vector2.Zero, this.whichFish, 1);
        this.numberToFish = (int) Math.Ceiling(90.0 / (double) Math.Max(1, this.fish.price)) + Game1.player.FishingLevel / 5;
        this.reward = this.numberToFish * this.fish.price;
        this.parts.Clear();
        if (Game1.player.isMale)
          this.parts.Add(this.fish.name.Equals("Squid") ? new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13248", (object) this.reward, (object) this.numberToFish, (object) new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13253")) : new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13248", (object) this.reward, (object) this.numberToFish, (object) this.fish));
        else
          this.parts.Add(this.fish.name.Equals("Squid") ? new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13251", (object) this.reward, (object) this.numberToFish, (object) new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13253")) : new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13251", (object) this.reward, (object) this.numberToFish, (object) this.fish));
        this.dialogueparts.Clear();
        this.dialogueparts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13256", (object) this.fish));
        this.dialogueparts.Add(((IEnumerable<DescriptionElement>) new DescriptionElement[4]
        {
          (DescriptionElement) "Strings\\StringsFromCSFiles:FishingQuest.cs.13258",
          (DescriptionElement) "Strings\\StringsFromCSFiles:FishingQuest.cs.13259",
          new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13260", (object) ((IEnumerable<DescriptionElement>) new DescriptionElement[6]
          {
            (DescriptionElement) "Strings\\StringsFromCSFiles:FishingQuest.cs.13261",
            (DescriptionElement) "Strings\\StringsFromCSFiles:FishingQuest.cs.13262",
            (DescriptionElement) "Strings\\StringsFromCSFiles:FishingQuest.cs.13263",
            (DescriptionElement) "Strings\\StringsFromCSFiles:FishingQuest.cs.13264",
            (DescriptionElement) "Strings\\StringsFromCSFiles:FishingQuest.cs.13265",
            (DescriptionElement) "Strings\\StringsFromCSFiles:FishingQuest.cs.13266"
          }).ElementAt<DescriptionElement>(this.random.Next(6))),
          (DescriptionElement) "Strings\\StringsFromCSFiles:FishingQuest.cs.13267"
        }).ElementAt<DescriptionElement>(this.random.Next(4)));
        this.dialogueparts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13268"));
        this.objective = this.fish.name.Equals("Squid") ? new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13255", (object) 0, (object) this.numberToFish) : new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13244", (object) 0, (object) this.numberToFish, (object) this.fish);
      }
      this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13274", (object) this.reward));
      this.parts.Add((DescriptionElement) "Strings\\StringsFromCSFiles:FishingQuest.cs.13275");
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
      if (this.numberFished < this.numberToFish)
        this.objective = this.fish.name.Equals("Octopus") ? new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13243", (object) this.numberFished, (object) this.numberToFish) : (this.fish.name.Equals("Squid") ? new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13255", (object) this.numberFished, (object) this.numberToFish) : new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13244", (object) this.numberFished, (object) this.numberToFish, (object) this.fish));
      if (this.objective == null)
        return;
      this.currentObjective = this.objective.loadDescriptionElement();
    }

    public override bool checkIfComplete(NPC n = null, int fishid = -1, int number2 = -1, Item item = null, string monsterName = null)
    {
      this.loadQuestInfo();
      if (n == null && fishid != -1 && (fishid == this.whichFish && this.numberFished < this.numberToFish))
      {
        this.numberFished = Math.Min(this.numberToFish, this.numberFished + 1);
        if (this.numberFished >= this.numberToFish)
        {
          this.dailyQuest = false;
          if (this.target == null)
            this.target = "Willy";
          this.objective = new DescriptionElement("Strings\\Quests:ObjectiveReturnToNPC", (object) Game1.getCharacterFromName(this.target, false));
          Game1.playSound("jingle1");
        }
      }
      else if (n != null && this.numberFished >= this.numberToFish && (this.target != null && n.name.Equals(this.target)) && (n.isVillager() && !this.completed))
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
