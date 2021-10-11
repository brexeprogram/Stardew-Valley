// Decompiled with JetBrains decompiler
// Type: StardewValley.BellsAndWhistles.Lexicon
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using System;

namespace StardewValley.BellsAndWhistles
{
  public class Lexicon
  {
    public static string getRandomNegativeItemSlanderNoun()
    {
      Random random = new Random((int) Game1.stats.DaysPlayed + (int) Game1.uniqueIDForThisGame / 2);
      string[] strArray = Game1.content.LoadString("Strings\\Lexicon:RandomNegativeItemNoun").Split('#');
      return strArray[random.Next(strArray.Length)];
    }

    public static string appendArticle(string word)
    {
      return Game1.getProperArticleForWord(word) + " " + word;
    }

    public static string getRandomPositiveAdjectiveForEventOrPerson(NPC n = null)
    {
      Random random = new Random((int) Game1.stats.DaysPlayed + (int) Game1.uniqueIDForThisGame / 2);
      string[] strArray;
      if (n != null && n.age != 0)
        strArray = Game1.content.LoadString("Strings\\Lexicon:RandomPositiveAdjective_Child").Split('#');
      else if (n != null && n.gender == 0)
        strArray = Game1.content.LoadString("Strings\\Lexicon:RandomPositiveAdjective_AdultMale").Split('#');
      else if (n != null && n.gender == 1)
        strArray = Game1.content.LoadString("Strings\\Lexicon:RandomPositiveAdjective_AdultFemale").Split('#');
      else
        strArray = Game1.content.LoadString("Strings\\Lexicon:RandomPositiveAdjective_PlaceOrEvent").Split('#');
      return strArray[random.Next(strArray.Length)];
    }

    public static string getRandomNegativeAdjectiveForEventOrPerson(NPC n = null)
    {
      Random random = new Random((int) Game1.stats.DaysPlayed + (int) Game1.uniqueIDForThisGame / 2);
      string[] strArray;
      if (n != null && n.age != 0)
        strArray = Game1.content.LoadString("Strings\\Lexicon:RandomNegativeAdjective_Child").Split('#');
      else if (n != null && n.gender == 0)
        strArray = Game1.content.LoadString("Strings\\Lexicon:RandomNegativeAdjective_AdultMale").Split('#');
      else if (n != null && n.gender == 1)
        strArray = Game1.content.LoadString("Strings\\Lexicon:RandomNegativeAdjective_AdultFemale").Split('#');
      else
        strArray = Game1.content.LoadString("Strings\\Lexicon:RandomNegativeAdjective_PlaceOrEvent").Split('#');
      return strArray[random.Next(strArray.Length)];
    }

    public static string getRandomDeliciousAdjective(NPC n = null)
    {
      Random random = new Random((int) Game1.stats.DaysPlayed + (int) Game1.uniqueIDForThisGame / 2);
      string[] strArray;
      if (n != null && n.age == 2)
        strArray = Game1.content.LoadString("Strings\\Lexicon:RandomDeliciousAdjective_Child").Split('#');
      else
        strArray = Game1.content.LoadString("Strings\\Lexicon:RandomDeliciousAdjective").Split('#');
      return strArray[random.Next(strArray.Length)];
    }

    public static string getRandomNegativeFoodAdjective(NPC n = null)
    {
      Random random = new Random((int) Game1.stats.DaysPlayed + (int) Game1.uniqueIDForThisGame / 2);
      string[] strArray;
      if (n != null && n.age == 2)
        strArray = Game1.content.LoadString("Strings\\Lexicon:RandomNegativeFoodAdjective_Child").Split('#');
      else if (n != null && n.manners == 1)
        strArray = Game1.content.LoadString("Strings\\Lexicon:RandomNegativeFoodAdjective_Polite").Split('#');
      else
        strArray = Game1.content.LoadString("Strings\\Lexicon:RandomNegativeFoodAdjective").Split('#');
      return strArray[random.Next(strArray.Length)];
    }

    public static string getRandomSlightlyPositiveAdjectiveForEdibleNoun(NPC n = null)
    {
      Random random = new Random((int) Game1.stats.DaysPlayed + (int) Game1.uniqueIDForThisGame / 2);
      string[] strArray = Game1.content.LoadString("Strings\\Lexicon:RandomSlightlyPositiveFoodAdjective").Split('#');
      return strArray[random.Next(strArray.Length)];
    }

    public static string getGenderedChildTerm(bool isMale)
    {
      if (isMale)
        return Game1.content.LoadString("Strings\\Lexicon:ChildTerm_Male");
      return Game1.content.LoadString("Strings\\Lexicon:ChildTerm_Female");
    }
  }
}
