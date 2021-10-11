// Decompiled with JetBrains decompiler
// Type: StardewValley.Quests.ResourceCollectionQuest
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Quests
{
  public class ResourceCollectionQuest : Quest
  {
    public List<DescriptionElement> parts = new List<DescriptionElement>();
    public List<DescriptionElement> dialogueparts = new List<DescriptionElement>();
    public string target;
    public string targetMessage;
    public int numberCollected;
    public int number;
    public int reward;
    public int resource;
    public StardewValley.Object deliveryItem;
    public DescriptionElement objective;

    public ResourceCollectionQuest()
    {
      this.questType = 10;
    }

    public void loadQuestInfo()
    {
      if (this.target != null || (int) Game1.gameMode == 6)
        return;
      this.questTitle = Game1.content.LoadString("Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13640");
      this.resource = this.random.Next(6) * 2;
      switch (this.resource)
      {
        case 0:
          this.resource = 378;
          this.deliveryItem = new StardewValley.Object(Vector2.Zero, this.resource, 1);
          this.number = 20 + Game1.player.MiningLevel * 2 + this.random.Next(-2, 4) * 2;
          this.reward = this.number * 10;
          this.number = this.number - this.number % 5;
          this.target = "Clint";
          break;
        case 2:
          this.resource = 380;
          this.deliveryItem = new StardewValley.Object(Vector2.Zero, this.resource, 1);
          this.number = 15 + Game1.player.MiningLevel + this.random.Next(-1, 3) * 2;
          this.reward = this.number * 15;
          this.number = (int) ((double) this.number * 0.75);
          this.number = this.number - this.number % 5;
          this.target = "Clint";
          break;
        case 4:
          this.resource = 382;
          this.deliveryItem = new StardewValley.Object(Vector2.Zero, this.resource, 1);
          this.number = 10 + Game1.player.MiningLevel + this.random.Next(-1, 3) * 2;
          this.reward = this.number * 25;
          this.number = (int) ((double) this.number * 0.75);
          this.number = this.number - this.number % 5;
          this.target = "Clint";
          break;
        case 6:
          this.resource = Game1.player.deepestMineLevel > 40 ? 384 : 378;
          this.deliveryItem = new StardewValley.Object(Vector2.Zero, this.resource, 1);
          this.number = 8 + Game1.player.MiningLevel / 2 + this.random.Next(-1, 1) * 2;
          this.reward = this.number * 30;
          this.number = (int) ((double) this.number * 0.75);
          this.number = this.number - this.number % 2;
          this.target = "Clint";
          break;
        case 8:
          this.resource = 388;
          this.deliveryItem = new StardewValley.Object(Vector2.Zero, this.resource, 1);
          this.number = 25 + Game1.player.ForagingLevel + this.random.Next(-3, 3) * 2;
          this.number = this.number - this.number % 5;
          this.reward = this.number * 8;
          this.target = "Robin";
          break;
        case 10:
          this.resource = 390;
          this.deliveryItem = new StardewValley.Object(Vector2.Zero, this.resource, 1);
          this.number = 25 + Game1.player.MiningLevel + this.random.Next(-3, 3) * 2;
          this.number = this.number - this.number % 5;
          this.reward = this.number * 8;
          this.target = "Robin";
          break;
      }
      if (this.target == null)
        return;
      if (this.resource < 388)
      {
        this.parts.Clear();
        int index = this.random.Next(4);
        this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13647", (object) this.number, (object) this.deliveryItem, (object) ((IEnumerable<DescriptionElement>) new DescriptionElement[4]
        {
          (DescriptionElement) "Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13649",
          (DescriptionElement) "Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13650",
          (DescriptionElement) "Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13651",
          (DescriptionElement) "Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13652"
        }).ElementAt<DescriptionElement>(index)));
        if (index == 3)
        {
          this.dialogueparts.Clear();
          this.dialogueparts.Add((DescriptionElement) "Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13655");
          this.dialogueparts.Add((DescriptionElement) (this.random.NextDouble() < 0.3 ? "Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13656" : (this.random.NextDouble() < 0.5 ? "Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13657" : "Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13658")));
          this.dialogueparts.Add((DescriptionElement) "Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13659");
        }
        else
        {
          this.dialogueparts.Clear();
          this.dialogueparts.Add((DescriptionElement) "Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13662");
          this.dialogueparts.Add((DescriptionElement) (this.random.NextDouble() < 0.3 ? "Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13656" : (this.random.NextDouble() < 0.5 ? "Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13657" : "Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13658")));
          this.dialogueparts.Add(this.random.NextDouble() < 0.5 ? new DescriptionElement("Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13667", this.random.NextDouble() < 0.3 ? (object) new DescriptionElement("Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13668") : (this.random.NextDouble() < 0.5 ? (object) new DescriptionElement("Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13669") : (object) new DescriptionElement("Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13670"))) : (DescriptionElement) "Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13672");
          this.dialogueparts.Add((DescriptionElement) "Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13673");
        }
      }
      else
      {
        this.parts.Clear();
        this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13674", (object) this.number, (object) this.deliveryItem));
        this.dialogueparts.Clear();
        this.dialogueparts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13677", this.resource == 13 ? (object) new DescriptionElement("Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13678") : (object) new DescriptionElement("Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13679")));
        this.dialogueparts.Add((DescriptionElement) (this.random.NextDouble() < 0.3 ? "Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13681" : (this.random.NextDouble() < 0.5 ? "Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13682" : "Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13683")));
      }
      this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13607", (object) this.reward));
      this.parts.Add((DescriptionElement) (this.target.Equals("Clint") ? "Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13688" : ""));
      this.objective = new DescriptionElement("Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13691", (object) "0", (object) this.number, (object) this.deliveryItem);
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
      if (this.numberCollected < this.number)
        this.objective = new DescriptionElement("Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13691", (object) this.numberCollected, (object) this.number, (object) this.deliveryItem);
      if (this.objective == null)
        return;
      this.currentObjective = this.objective.loadDescriptionElement();
    }

    public override bool checkIfComplete(NPC n = null, int resourceCollected = -1, int amount = -1, Item item = null, string monsterName = null)
    {
      if (this.completed)
        return false;
      if (n == null && resourceCollected != -1 && (amount != -1 && resourceCollected == this.resource) && this.numberCollected < this.number)
      {
        this.numberCollected = Math.Min(this.number, this.numberCollected + amount);
        if (this.numberCollected < this.number)
        {
          if (this.deliveryItem == null)
            this.deliveryItem = new StardewValley.Object(Vector2.Zero, this.resource, 1);
        }
        else
        {
          this.objective = new DescriptionElement("Strings\\StringsFromCSFiles:FishingQuest.cs.13277", (object) Game1.getCharacterFromName(this.target, false));
          Game1.playSound("jingle1");
        }
        Game1.dayTimeMoneyBox.moneyDial.animations.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(387, 497, 3, 8), 800f, 1, 0, Game1.dayTimeMoneyBox.position + new Vector2(228f, 244f), false, false, 1f, 0.01f, Color.White, 4f, 0.3f, 0.0f, 0.0f, false)
        {
          scaleChangeChange = -0.012f
        });
      }
      else if (n != null && this.target != null && (this.numberCollected >= this.number && n.name.Equals(this.target)) && n.isVillager())
      {
        n.CurrentDialogue.Push(new Dialogue(this.targetMessage, n));
        this.moneyReward = this.reward;
        n.name.Equals("Robin");
        this.questComplete();
        Game1.drawDialogue(n);
        return true;
      }
      return false;
    }
  }
}
