// Decompiled with JetBrains decompiler
// Type: StardewValley.Quests.Quest
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using StardewValley.Monsters;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace StardewValley.Quests
{
  [XmlInclude(typeof (SocializeQuest))]
  [XmlInclude(typeof (SlayMonsterQuest))]
  [XmlInclude(typeof (ResourceCollectionQuest))]
  [XmlInclude(typeof (ItemDeliveryQuest))]
  [XmlInclude(typeof (ItemHarvestQuest))]
  [XmlInclude(typeof (CraftingQuest))]
  [XmlInclude(typeof (FishingQuest))]
  [XmlInclude(typeof (GoSomewhereQuest))]
  [XmlInclude(typeof (LostItemQuest))]
  [XmlInclude(typeof (DescriptionElement))]
  public class Quest
  {
    public string _currentObjective = "";
    public string _questDescription = "";
    public string _questTitle = "";
    protected Random random = new Random((int) Game1.uniqueIDForThisGame + (int) Game1.stats.DaysPlayed);
    public List<int> nextQuests = new List<int>();
    public const int type_basic = 1;
    public const int type_crafting = 2;
    public const int type_itemDelivery = 3;
    public const int type_monster = 4;
    public const int type_socialize = 5;
    public const int type_location = 6;
    public const int type_fishing = 7;
    public const int type_building = 8;
    public const int type_harvest = 9;
    public const int type_resource = 10;
    public const int type_weeding = 11;
    public string rewardDescription;
    public string completionString;
    public bool accepted;
    public bool completed;
    public bool dailyQuest;
    public bool showNew;
    public bool canBeCancelled;
    public bool destroy;
    public int id;
    public int moneyReward;
    public int questType;
    public int daysLeft;

    public string questTitle
    {
      get
      {
        switch (this.questType)
        {
          case 3:
            this._questTitle = Game1.content.LoadString("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13285");
            break;
          case 4:
            this._questTitle = Game1.content.LoadString("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13696");
            break;
          case 5:
            this._questTitle = Game1.content.LoadString("Strings\\StringsFromCSFiles:SocializeQuest.cs.13785");
            break;
          case 7:
            this._questTitle = Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingQuest.cs.13227");
            break;
          case 10:
            this._questTitle = Game1.content.LoadString("Strings\\StringsFromCSFiles:ResourceCollectionQuest.cs.13640");
            break;
        }
        Dictionary<int, string> dictionary = Game1.temporaryContent.Load<Dictionary<int, string>>("Data\\Quests");
        if (dictionary != null && dictionary.ContainsKey(this.id))
          this._questTitle = dictionary[this.id].Split('/')[1];
        if (this._questTitle == null)
          this._questTitle = "";
        return this._questTitle;
      }
      set
      {
        this._questTitle = value;
      }
    }

    [XmlIgnore]
    public string questDescription
    {
      get
      {
        this.reloadDescription();
        Dictionary<int, string> dictionary = Game1.temporaryContent.Load<Dictionary<int, string>>("Data\\Quests");
        if (dictionary != null && dictionary.ContainsKey(this.id))
          this._questDescription = dictionary[this.id].Split('/')[2];
        if (this._questDescription == null)
          this._questDescription = "";
        return this._questDescription;
      }
      set
      {
        this._questDescription = value;
      }
    }

    [XmlIgnore]
    public string currentObjective
    {
      get
      {
        Dictionary<int, string> dictionary = Game1.temporaryContent.Load<Dictionary<int, string>>("Data\\Quests");
        if (dictionary != null && dictionary.ContainsKey(this.id))
        {
          string[] strArray = dictionary[this.id].Split('/');
          if (strArray[3].Length > 1)
            this._currentObjective = strArray[3];
        }
        this.reloadObjective();
        if (this._currentObjective == null)
          this._currentObjective = "";
        return this._currentObjective;
      }
      set
      {
        this._currentObjective = value;
      }
    }

    public static Quest getQuestFromId(int id)
    {
      Dictionary<int, string> dictionary = Game1.temporaryContent.Load<Dictionary<int, string>>("Data\\Quests");
      if (dictionary == null || !dictionary.ContainsKey(id))
        return (Quest) null;
      string[] strArray1 = dictionary[id].Split('/');
      string s = strArray1[0];
      Quest quest = (Quest) null;
      string[] strArray2 = strArray1[4].Split(' ');
      // ISSUE: reference to a compiler-generated method
      uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(s);
      if (stringHash <= 1539345862U)
      {
        if (stringHash <= 133275711U)
        {
          if ((int) stringHash != 126609884)
          {
            if ((int) stringHash == 133275711 && s == "Crafting")
            {
              quest = (Quest) new CraftingQuest(Convert.ToInt32(strArray2[0]), strArray2[1].ToLower().Equals("true"));
              quest.questType = 2;
            }
          }
          else if (s == "LostItem")
            quest = (Quest) new LostItemQuest(strArray2[0], strArray2[2], Convert.ToInt32(strArray2[1]), Convert.ToInt32(strArray2[3]), Convert.ToInt32(strArray2[4]));
        }
        else if ((int) stringHash != 1217142150)
        {
          if ((int) stringHash == 1539345862 && s == "Location")
          {
            quest = (Quest) new GoSomewhereQuest(strArray2[0]);
            quest.questType = 6;
          }
        }
        else if (s == "ItemDelivery")
        {
          quest = (Quest) new ItemDeliveryQuest();
          (quest as ItemDeliveryQuest).target = strArray2[0];
          (quest as ItemDeliveryQuest).item = Convert.ToInt32(strArray2[1]);
          (quest as ItemDeliveryQuest).targetMessage = strArray1[9];
          if (strArray2.Length > 2)
            (quest as ItemDeliveryQuest).number = Convert.ToInt32(strArray2[2]);
          quest.questType = 3;
        }
      }
      else if (stringHash <= 2324152213U)
      {
        if ((int) stringHash != 1629445681)
        {
          if ((int) stringHash == -1970815083 && s == "Building")
          {
            quest = new Quest();
            quest.questType = 8;
            quest.completionString = strArray2[0];
          }
        }
        else if (s == "Monster")
        {
          quest = (Quest) new SlayMonsterQuest();
          (quest as SlayMonsterQuest).loadQuestInfo();
          (quest as SlayMonsterQuest).monster.name = strArray2[0].Replace('_', ' ');
          if ((quest as SlayMonsterQuest).monsterName == "Frost Jelly" || (quest as SlayMonsterQuest).monsterName == "Sludge")
          {
            (quest as SlayMonsterQuest).monster = new Monster();
            (quest as SlayMonsterQuest).monster.name = (quest as SlayMonsterQuest).monsterName;
          }
          else
            (quest as SlayMonsterQuest).monster = new Monster((quest as SlayMonsterQuest).monsterName, Vector2.Zero);
          (quest as SlayMonsterQuest).numberToKill = Convert.ToInt32(strArray2[1]);
          if (strArray2.Length > 2)
            (quest as SlayMonsterQuest).target = strArray2[2];
          else
            (quest as SlayMonsterQuest).target = "null";
          quest.questType = 4;
        }
      }
      else if ((int) stringHash != -684751651)
      {
        if ((int) stringHash != -271098705)
        {
          if ((int) stringHash == -117419790 && s == "Social")
            quest = (Quest) new SocializeQuest();
        }
        else if (s == "ItemHarvest")
          quest = (Quest) new ItemHarvestQuest(Convert.ToInt32(strArray2[0]), strArray2.Length > 1 ? Convert.ToInt32(strArray2[1]) : 1);
      }
      else if (s == "Basic")
      {
        quest = new Quest();
        quest.questType = 1;
      }
      quest.id = id;
      quest.questTitle = strArray1[1];
      quest.questDescription = strArray1[2];
      if (strArray1[3].Length > 1)
        quest.currentObjective = strArray1[3];
      string str1 = strArray1[5];
      char[] chArray = new char[1]{ ' ' };
      foreach (string str2 in str1.Split(chArray))
        quest.nextQuests.Add(Convert.ToInt32(str2));
      quest.showNew = true;
      quest.moneyReward = Convert.ToInt32(strArray1[6]);
      quest.rewardDescription = strArray1[6].Equals("-1") ? (string) null : strArray1[7];
      if (strArray1.Length > 8)
        quest.canBeCancelled = strArray1[8].Equals("true");
      return quest;
    }

    public virtual void reloadObjective()
    {
    }

    public virtual void reloadDescription()
    {
    }

    public virtual void adjustGameLocation(GameLocation location)
    {
    }

    public virtual void accept()
    {
      this.accepted = true;
    }

    public virtual bool checkIfComplete(NPC n = null, int number1 = -1, int number2 = -2, Item item = null, string str = null)
    {
      if (this.completionString == null || str == null || !str.Equals(this.completionString))
        return false;
      this.questComplete();
      return true;
    }

    public bool hasReward()
    {
      if (this.moneyReward > 0)
        return true;
      if (this.rewardDescription != null)
        return this.rewardDescription.Length > 2;
      return false;
    }

    public void questComplete()
    {
      if (this.completed)
        return;
      if (this.dailyQuest || this.questType == 7)
        ++Game1.stats.QuestsCompleted;
      this.completed = true;
      if (this.nextQuests.Count > 0)
      {
        foreach (int nextQuest in this.nextQuests)
        {
          if (nextQuest > 0)
            Game1.player.questLog.Add(Quest.getQuestFromId(nextQuest));
        }
        Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Quest.cs.13636"), 2));
      }
      if (this.moneyReward <= 0 && (this.rewardDescription == null || this.rewardDescription.Length <= 2))
        Game1.player.questLog.Remove(this);
      else
        Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Quest.cs.13636"), 2));
      Game1.playSound("questcomplete");
    }
  }
}
