// Decompiled with JetBrains decompiler
// Type: StardewValley.Stats
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using StardewValley.Locations;
using System;
using System.Collections.Generic;

namespace StardewValley
{
  public class Stats
  {
    public SerializableDictionary<string, int> specificMonstersKilled = new SerializableDictionary<string, int>();
    public uint seedsSown;
    public uint itemsShipped;
    public uint itemsCooked;
    public uint itemsCrafted;
    public uint chickenEggsLayed;
    public uint duckEggsLayed;
    public uint cowMilkProduced;
    public uint goatMilkProduced;
    public uint rabbitWoolProduced;
    public uint sheepWoolProduced;
    public uint cheeseMade;
    public uint goatCheeseMade;
    public uint trufflesFound;
    public uint stoneGathered;
    public uint rocksCrushed;
    public uint dirtHoed;
    public uint giftsGiven;
    public uint timesUnconscious;
    public uint averageBedtime;
    public uint timesFished;
    public uint fishCaught;
    public uint bouldersCracked;
    public uint stumpsChopped;
    public uint stepsTaken;
    public uint monstersKilled;
    public uint diamondsFound;
    public uint prismaticShardsFound;
    public uint otherPreciousGemsFound;
    public uint caveCarrotsFound;
    public uint copperFound;
    public uint ironFound;
    public uint coalFound;
    public uint coinsFound;
    public uint goldFound;
    public uint iridiumFound;
    public uint barsSmelted;
    public uint beveragesMade;
    public uint preservesMade;
    public uint piecesOfTrashRecycled;
    public uint mysticStonesCrushed;
    public uint daysPlayed;
    public uint weedsEliminated;
    public uint sticksChopped;
    public uint notesFound;
    public uint questsCompleted;
    public uint starLevelCropsShipped;
    public uint cropsShipped;
    public uint itemsForaged;
    public uint slimesKilled;
    public uint geodesCracked;
    public uint goodFriends;

    public void monsterKilled(string name)
    {
      if (this.specificMonstersKilled.ContainsKey(name))
      {
        if (AdventureGuild.willThisKillCompleteAMonsterSlayerQuest(name))
        {
          Game1.showGlobalMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Stats.cs.5129"));
          SerializableDictionary<string, int> specificMonstersKilled = this.specificMonstersKilled;
          string str = name;
          string index1 = str;
          int num1 = specificMonstersKilled[index1];
          string index2 = str;
          int num2 = num1 + 1;
          specificMonstersKilled[index2] = num2;
          if (!AdventureGuild.areAllMonsterSlayerQuestsComplete())
            return;
          Game1.getSteamAchievement("Achievement_KeeperOfTheMysticRings");
        }
        else
        {
          SerializableDictionary<string, int> specificMonstersKilled = this.specificMonstersKilled;
          string str = name;
          string index1 = str;
          int num1 = specificMonstersKilled[index1];
          string index2 = str;
          int num2 = num1 + 1;
          specificMonstersKilled[index2] = num2;
        }
      }
      else
        this.specificMonstersKilled.Add(name, 1);
    }

    public int getMonstersKilled(string name)
    {
      if (this.specificMonstersKilled.ContainsKey(name))
        return this.specificMonstersKilled[name];
      return 0;
    }

    public uint GoodFriends
    {
      get
      {
        return this.goodFriends;
      }
      set
      {
        this.goodFriends = value;
      }
    }

    public uint CropsShipped
    {
      get
      {
        return this.cropsShipped;
      }
      set
      {
        this.cropsShipped = value;
      }
    }

    public uint ItemsForaged
    {
      get
      {
        return this.itemsForaged;
      }
      set
      {
        this.itemsForaged = value;
      }
    }

    public uint GeodesCracked
    {
      get
      {
        return this.geodesCracked;
      }
      set
      {
        this.geodesCracked = value;
      }
    }

    public uint SlimesKilled
    {
      get
      {
        return this.slimesKilled;
      }
      set
      {
        this.slimesKilled = value;
      }
    }

    public uint StarLevelCropsShipped
    {
      get
      {
        return this.starLevelCropsShipped;
      }
      set
      {
        this.starLevelCropsShipped = value;
        this.checkForStarCropsAchievements();
      }
    }

    public uint StoneGathered
    {
      get
      {
        return this.stoneGathered;
      }
      set
      {
        this.stoneGathered = value;
        this.checkForStoneAchievements();
      }
    }

    public uint QuestsCompleted
    {
      get
      {
        return this.questsCompleted;
      }
      set
      {
        this.questsCompleted = value;
        this.checkForQuestAchievements();
      }
    }

    public uint FishCaught
    {
      get
      {
        return this.fishCaught;
      }
      set
      {
        this.fishCaught = value;
      }
    }

    public uint NotesFound
    {
      get
      {
        return this.notesFound;
      }
      set
      {
        this.notesFound = value;
      }
    }

    public uint SticksChopped
    {
      get
      {
        return this.sticksChopped;
      }
      set
      {
        this.sticksChopped = value;
        this.checkForWoodAchievements();
      }
    }

    public uint WeedsEliminated
    {
      get
      {
        return this.weedsEliminated;
      }
      set
      {
        this.weedsEliminated = value;
      }
    }

    public uint DaysPlayed
    {
      get
      {
        return this.daysPlayed;
      }
      set
      {
        this.daysPlayed = value;
      }
    }

    public uint BouldersCracked
    {
      get
      {
        return this.bouldersCracked;
      }
      set
      {
        this.bouldersCracked = value;
      }
    }

    public uint MysticStonesCrushed
    {
      get
      {
        return this.mysticStonesCrushed;
      }
      set
      {
        this.mysticStonesCrushed = value;
      }
    }

    public uint GoatCheeseMade
    {
      get
      {
        return this.goatCheeseMade;
      }
      set
      {
        this.goatCheeseMade = value;
        this.checkForCheeseAchievements();
      }
    }

    public uint CheeseMade
    {
      get
      {
        return this.cheeseMade;
      }
      set
      {
        this.cheeseMade = value;
        this.checkForCheeseAchievements();
      }
    }

    public uint PiecesOfTrashRecycled
    {
      get
      {
        return this.piecesOfTrashRecycled;
      }
      set
      {
        this.piecesOfTrashRecycled = value;
      }
    }

    public uint PreservesMade
    {
      get
      {
        return this.preservesMade;
      }
      set
      {
        this.preservesMade = value;
      }
    }

    public uint BeveragesMade
    {
      get
      {
        return this.beveragesMade;
      }
      set
      {
        this.beveragesMade = value;
      }
    }

    public uint BarsSmelted
    {
      get
      {
        return this.barsSmelted;
      }
      set
      {
        this.barsSmelted = value;
      }
    }

    public uint IridiumFound
    {
      get
      {
        return this.iridiumFound;
      }
      set
      {
        this.iridiumFound = value;
        this.checkForIridiumOreAchievements();
      }
    }

    public uint GoldFound
    {
      get
      {
        return this.goldFound;
      }
      set
      {
        this.goldFound = value;
        this.checkForGoldOreAchievements();
      }
    }

    public uint CoinsFound
    {
      get
      {
        return this.coinsFound;
      }
      set
      {
        this.coinsFound = value;
      }
    }

    public uint CoalFound
    {
      get
      {
        return this.coalFound;
      }
      set
      {
        this.coalFound = value;
        this.checkForCoalOreAchievements();
      }
    }

    public uint IronFound
    {
      get
      {
        return this.ironFound;
      }
      set
      {
        this.ironFound = value;
        this.checkForIronOreAchievements();
      }
    }

    public uint CopperFound
    {
      get
      {
        return this.copperFound;
      }
      set
      {
        this.copperFound = value;
        this.checkForCopperOreAchievements();
      }
    }

    public uint CaveCarrotsFound
    {
      get
      {
        return this.caveCarrotsFound;
      }
      set
      {
        this.caveCarrotsFound = value;
      }
    }

    public uint OtherPreciousGemsFound
    {
      get
      {
        return this.otherPreciousGemsFound;
      }
      set
      {
        this.otherPreciousGemsFound = value;
      }
    }

    public uint PrismaticShardsFound
    {
      get
      {
        return this.prismaticShardsFound;
      }
      set
      {
        this.prismaticShardsFound = value;
      }
    }

    public uint DiamondsFound
    {
      get
      {
        return this.diamondsFound;
      }
      set
      {
        this.diamondsFound = value;
      }
    }

    public uint MonstersKilled
    {
      get
      {
        return this.monstersKilled;
      }
      set
      {
        this.monstersKilled = value;
      }
    }

    public uint StepsTaken
    {
      get
      {
        return this.stepsTaken;
      }
      set
      {
        this.stepsTaken = value;
      }
    }

    public uint StumpsChopped
    {
      get
      {
        return this.stumpsChopped;
      }
      set
      {
        this.stumpsChopped = value;
        this.checkForWoodAchievements();
      }
    }

    public uint TimesFished
    {
      get
      {
        return this.timesFished;
      }
      set
      {
        this.timesFished = value;
      }
    }

    public uint AverageBedtime
    {
      get
      {
        return this.averageBedtime;
      }
      set
      {
        this.averageBedtime = (this.averageBedtime * (this.daysPlayed - 1U) + value) / this.daysPlayed;
      }
    }

    public uint TimesUnconscious
    {
      get
      {
        return this.timesUnconscious;
      }
      set
      {
        this.timesUnconscious = value;
      }
    }

    public uint GiftsGiven
    {
      get
      {
        return this.giftsGiven;
      }
      set
      {
        this.giftsGiven = value;
      }
    }

    public uint DirtHoed
    {
      get
      {
        return this.dirtHoed;
      }
      set
      {
        this.dirtHoed = value;
      }
    }

    public uint RocksCrushed
    {
      get
      {
        return this.rocksCrushed;
      }
      set
      {
        this.rocksCrushed = value;
        this.checkForStoneBreakAchievements();
      }
    }

    public uint TrufflesFound
    {
      get
      {
        return this.trufflesFound;
      }
      set
      {
        this.trufflesFound = value;
      }
    }

    public uint SheepWoolProduced
    {
      get
      {
        return this.sheepWoolProduced;
      }
      set
      {
        this.sheepWoolProduced = value;
        this.checkForWoolAchievements();
      }
    }

    public uint RabbitWoolProduced
    {
      get
      {
        return this.rabbitWoolProduced;
      }
      set
      {
        this.rabbitWoolProduced = value;
        this.checkForWoolAchievements();
      }
    }

    public uint GoatMilkProduced
    {
      get
      {
        return this.goatMilkProduced;
      }
      set
      {
        this.goatMilkProduced = value;
        this.checkForGoatMilkAchievements();
      }
    }

    public uint CowMilkProduced
    {
      get
      {
        return this.cowMilkProduced;
      }
      set
      {
        this.cowMilkProduced = value;
        this.checkForCowMilkAchievements();
      }
    }

    public uint DuckEggsLayed
    {
      get
      {
        return this.duckEggsLayed;
      }
      set
      {
        this.duckEggsLayed = value;
        this.checkForDuckEggAchievements();
      }
    }

    public uint ItemsCrafted
    {
      get
      {
        return this.itemsCrafted;
      }
      set
      {
        this.itemsCrafted = value;
        this.checkForCraftingAchievements();
      }
    }

    public uint ChickenEggsLayed
    {
      get
      {
        return this.chickenEggsLayed;
      }
      set
      {
        this.chickenEggsLayed = value;
        this.checkForChickenEggAchievements();
      }
    }

    public uint ItemsCooked
    {
      get
      {
        return this.itemsCooked;
      }
      set
      {
        this.itemsCooked = value;
      }
    }

    public uint ItemsShipped
    {
      get
      {
        return this.itemsShipped;
      }
      set
      {
        this.itemsShipped = value;
      }
    }

    public uint SeedsSown
    {
      get
      {
        return this.seedsSown;
      }
      set
      {
        this.seedsSown = value;
      }
    }

    public void checkForWoodAchievements()
    {
      uint num = this.SticksChopped + this.StumpsChopped * 4U;
      if (num >= 5000U || num >= 1500U)
        ;
    }

    public void checkForStoneAchievements()
    {
      uint num = this.RocksCrushed + this.BouldersCracked * 4U;
      if (num >= 5000U || num >= 1500U)
        ;
    }

    public void checkForStoneBreakAchievements()
    {
    }

    public void checkForCookingAchievements()
    {
      Dictionary<string, string> dictionary = Game1.content.Load<Dictionary<string, string>>("Data\\CookingRecipes");
      int num1 = 0;
      int num2 = 0;
      foreach (KeyValuePair<string, string> keyValuePair in dictionary)
      {
        if (Game1.player.cookingRecipes.ContainsKey(keyValuePair.Key))
        {
          int int32 = Convert.ToInt32(keyValuePair.Value.Split('/')[2].Split(' ')[0]);
          if (Game1.player.recipesCooked.ContainsKey(int32))
          {
            num2 += Game1.player.recipesCooked[int32];
            ++num1;
          }
        }
      }
      this.itemsCooked = (uint) num2;
      if (num1 == dictionary.Count)
        Game1.getAchievement(17);
      if (num1 >= 25)
        Game1.getAchievement(16);
      if (num1 < 10)
        return;
      Game1.getAchievement(15);
    }

    public void checkForCraftingAchievements()
    {
      Dictionary<string, string> dictionary = Game1.content.Load<Dictionary<string, string>>("Data\\CraftingRecipes");
      int num1 = 0;
      int num2 = 0;
      foreach (string key in dictionary.Keys)
      {
        if (Game1.player.craftingRecipes.ContainsKey(key))
        {
          num2 += Game1.player.craftingRecipes[key];
          if (Game1.player.craftingRecipes[key] > 0)
            ++num1;
        }
      }
      this.itemsCrafted = (uint) num2;
      if (num1 >= dictionary.Count)
        Game1.getAchievement(22);
      if (num1 >= 30)
        Game1.getAchievement(21);
      if (num1 < 15)
        return;
      Game1.getAchievement(20);
    }

    public void checkForShippingAchievements()
    {
      if (this.farmerShipped(24, 15) && this.farmerShipped(188, 15) && (this.farmerShipped(190, 15) && this.farmerShipped(192, 15)) && (this.farmerShipped(248, 15) && this.farmerShipped(250, 15) && (this.farmerShipped(252, 15) && this.farmerShipped(254, 15))) && (this.farmerShipped(256, 15) && this.farmerShipped(258, 15) && (this.farmerShipped(260, 15) && this.farmerShipped(262, 15)) && (this.farmerShipped(264, 15) && this.farmerShipped(266, 15) && (this.farmerShipped(268, 15) && this.farmerShipped(270, 15)))) && (this.farmerShipped(272, 15) && this.farmerShipped(274, 15) && (this.farmerShipped(276, 15) && this.farmerShipped(278, 15)) && (this.farmerShipped(280, 15) && this.farmerShipped(282, 15) && (this.farmerShipped(284, 15) && this.farmerShipped(300, 15))) && (this.farmerShipped(304, 15) && this.farmerShipped(398, 15) && (this.farmerShipped(400, 15) && this.farmerShipped(433, 15)))))
        Game1.getAchievement(31);
      if (!this.farmerShipped(24, 300) && !this.farmerShipped(188, 300) && (!this.farmerShipped(190, 300) && !this.farmerShipped(192, 300)) && (!this.farmerShipped(248, 300) && !this.farmerShipped(250, 300) && (!this.farmerShipped(252, 300) && !this.farmerShipped(254, 300))) && (!this.farmerShipped(256, 300) && !this.farmerShipped(258, 300) && (!this.farmerShipped(260, 300) && !this.farmerShipped(262, 300)) && (!this.farmerShipped(264, 300) && !this.farmerShipped(266, 300) && (!this.farmerShipped(268, 300) && !this.farmerShipped(270, 300)))) && (!this.farmerShipped(272, 300) && !this.farmerShipped(274, 300) && (!this.farmerShipped(276, 300) && !this.farmerShipped(278, 300)) && (!this.farmerShipped(280, 300) && !this.farmerShipped(282, 300) && (!this.farmerShipped(284, 300) && !this.farmerShipped(454, 300))) && (!this.farmerShipped(300, 300) && !this.farmerShipped(304, 300) && (!(this.farmerShipped(398, 300) | this.farmerShipped(433, 300)) && !this.farmerShipped(400, 300)) && (!this.farmerShipped(591, 300) && !this.farmerShipped(593, 300) && (!this.farmerShipped(595, 300) && !this.farmerShipped(597, 300))))))
        return;
      Game1.getAchievement(32);
    }

    public void checkForStarCropsAchievements()
    {
      if (this.StarLevelCropsShipped < 100U)
        return;
      Game1.getAchievement(77);
    }

    private bool farmerShipped(int index, int number)
    {
      return Game1.player.basicShipped.ContainsKey(index) && Game1.player.basicShipped[index] >= number;
    }

    public void checkForFishingAchievements()
    {
      int num1 = 0;
      int num2 = 0;
      int num3 = 59;
      foreach (KeyValuePair<int, string> keyValuePair in Game1.objectInformation)
      {
        if (keyValuePair.Value.Split('/')[3].Contains("Fish") && (keyValuePair.Key < 167 || keyValuePair.Key >= 173) && Game1.player.fishCaught.ContainsKey(keyValuePair.Key))
        {
          num1 += Game1.player.fishCaught[keyValuePair.Key][0];
          ++num2;
        }
      }
      this.fishCaught = (uint) num1;
      if (num1 >= 100)
        Game1.getAchievement(27);
      if (num2 == num3)
      {
        Game1.getAchievement(26);
        if (!Game1.player.hasOrWillReceiveMail("CF_Fish"))
          Game1.addMailForTomorrow("CF_Fish", false, false);
      }
      if (num2 >= 24)
        Game1.getAchievement(25);
      if (num2 < 10)
        return;
      Game1.getAchievement(24);
    }

    public void checkForChickenEggAchievements()
    {
      if (this.ChickenEggsLayed < 800U && this.ChickenEggsLayed < 350U)
      {
        int chickenEggsLayed1 = (int) this.ChickenEggsLayed;
      }
      int chickenEggsLayed2 = (int) this.chickenEggsLayed;
      int num = (int) this.chickenEggsLayed + (int) this.duckEggsLayed * 2;
    }

    public void checkForDuckEggAchievements()
    {
      if (this.DuckEggsLayed < 200U)
      {
        int duckEggsLayed = (int) this.DuckEggsLayed;
      }
      int num = (int) this.chickenEggsLayed + (int) this.duckEggsLayed * 2;
    }

    public void checkForCowMilkAchievements()
    {
      if (this.CowMilkProduced < 500U)
      {
        int cowMilkProduced1 = (int) this.CowMilkProduced;
      }
      int cowMilkProduced2 = (int) this.cowMilkProduced;
      int num1 = (int) this.CowMilkProduced + (int) this.SheepWoolProduced * 3;
      int num2 = (int) this.CowMilkProduced + (int) this.SheepWoolProduced * 3 + (int) this.GoatMilkProduced * 2;
    }

    public void checkForGoatMilkAchievements()
    {
      if (this.GoatMilkProduced < 300U)
      {
        int goatMilkProduced = (int) this.GoatMilkProduced;
      }
      int num = (int) this.CowMilkProduced + (int) this.SheepWoolProduced * 3 + (int) this.GoatMilkProduced * 2;
    }

    public void checkForWoolAchievements()
    {
      if (this.RabbitWoolProduced + this.SheepWoolProduced < 200U)
        ;
      int num1 = (int) this.CowMilkProduced + (int) this.SheepWoolProduced * 3;
      int num2 = (int) this.CowMilkProduced + (int) this.SheepWoolProduced * 3 + (int) this.GoatMilkProduced * 2;
    }

    public void checkForCheeseAchievements()
    {
      int num = (int) this.GoatCheeseMade + (int) this.CheeseMade;
    }

    public void checkForArchaeologyAchievements()
    {
    }

    public void checkForCopperOreAchievements()
    {
      if (this.CopperFound >= 2500U)
        return;
      int copperFound = (int) this.CopperFound;
    }

    public void checkForIronOreAchievements()
    {
      if (this.IronFound >= 1000U)
        return;
      int ironFound = (int) this.IronFound;
    }

    public void checkForCoalOreAchievements()
    {
      if (this.CoalFound >= 750U)
        return;
      int coalFound = (int) this.CoalFound;
    }

    public void checkForGoldOreAchievements()
    {
      if (this.GoldFound >= 500U)
        return;
      int goldFound = (int) this.GoldFound;
    }

    public void checkForIridiumOreAchievements()
    {
      if (this.IridiumFound >= 30U)
        return;
      int iridiumFound = (int) this.IridiumFound;
    }

    public void checkForMoneyAchievements()
    {
      if (Game1.player.totalMoneyEarned >= 10000000U)
        Game1.getAchievement(4);
      if (Game1.player.totalMoneyEarned >= 1000000U)
        Game1.getAchievement(3);
      if (Game1.player.totalMoneyEarned >= 250000U)
        Game1.getAchievement(2);
      if (Game1.player.totalMoneyEarned >= 50000U)
        Game1.getAchievement(1);
      if (Game1.player.totalMoneyEarned < 15000U)
        return;
      Game1.getAchievement(0);
    }

    public void checkForBuildingUpgradeAchievements()
    {
      if (Game1.player.HouseUpgradeLevel == 2)
        Game1.getAchievement(19);
      if (Game1.player.HouseUpgradeLevel != 1)
        return;
      Game1.getAchievement(18);
    }

    public void checkForQuestAchievements()
    {
      if (this.QuestsCompleted >= 40U)
      {
        Game1.getAchievement(30);
        Game1.addMailForTomorrow("quest35", false, false);
      }
      if (this.QuestsCompleted < 10U)
        return;
      Game1.getAchievement(29);
      Game1.addMailForTomorrow("quest10", false, false);
    }

    public void checkForFriendshipAchievements()
    {
      uint num1 = 0;
      uint num2 = 0;
      uint num3 = 0;
      foreach (int[] numArray in Game1.player.friendships.Values)
      {
        int index1 = 0;
        if (numArray[index1] >= 2500)
          ++num3;
        int index2 = 0;
        if (numArray[index2] >= 2000)
          ++num2;
        int index3 = 0;
        if (numArray[index3] >= 1250)
          ++num1;
      }
      this.GoodFriends = num2;
      if (num1 >= 20U)
        Game1.getAchievement(13);
      if (num1 >= 10U)
        Game1.getAchievement(12);
      if (num1 >= 4U)
        Game1.getAchievement(11);
      if (num1 >= 1U)
        Game1.getAchievement(6);
      if (num3 >= 8U)
        Game1.getAchievement(9);
      if (num3 >= 1U)
        Game1.getAchievement(7);
      Dictionary<string, string> dictionary = Game1.content.Load<Dictionary<string, string>>("Data\\CookingRecipes");
      foreach (string key in dictionary.Keys)
      {
        string[] strArray = dictionary[key].Split('/')[3].Split(' ');
        if (strArray[0].Equals("f") && Game1.player.friendships.ContainsKey(strArray[1]) && (Game1.player.friendships[strArray[1]][0] >= Convert.ToInt32(strArray[2]) * 250 && !Game1.player.cookingRecipes.ContainsKey(key)))
          Game1.addMailForTomorrow(strArray[1] + "Cooking", false, false);
      }
    }
  }
}
