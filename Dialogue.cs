// Decompiled with JetBrains decompiler
// Type: StardewValley.Dialogue
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley
{
  public class Dialogue
  {
    private static bool nameArraysTranslated = false;
    public static string[] adjectives = new string[20]
    {
      "Purple",
      "Gooey",
      "Chalky",
      "Green",
      "Plush",
      "Chunky",
      "Gigantic",
      "Greasy",
      "Gloomy",
      "Practical",
      "Lanky",
      "Dopey",
      "Crusty",
      "Fantastic",
      "Rubbery",
      "Silly",
      "Courageous",
      "Reasonable",
      "Lonely",
      "Bitter"
    };
    public static string[] nouns = new string[23]
    {
      "Dragon",
      "Buffet",
      "Biscuit",
      "Robot",
      "Planet",
      "Pepper",
      "Tomb",
      "Hyena",
      "Lip",
      "Quail",
      "Cheese",
      "Disaster",
      "Raincoat",
      "Shoe",
      "Castle",
      "Elf",
      "Pump",
      "Crisp",
      "Wig",
      "Mermaid",
      "Drumstick",
      "Puppet",
      "Submarine"
    };
    public static string[] verbs = new string[13]
    {
      "ran",
      "danced",
      "spoke",
      "galloped",
      "ate",
      "floated",
      "stood",
      "flowed",
      "smelled",
      "swam",
      "grilled",
      "cracked",
      "melted"
    };
    public static string[] positional = new string[13]
    {
      "atop",
      "near",
      "with",
      "alongside",
      "away from",
      "too close to",
      "dangerously close to",
      "far, far away from",
      "uncomfortably close to",
      "way above the",
      "miles below",
      "on a different planet from",
      "in a different century than"
    };
    public static string[] places = new string[12]
    {
      "Castle Village",
      "Basket Town",
      "Pine Mesa City",
      "Point Drake",
      "Minister Valley",
      "Grampleton",
      "Zuzu City",
      "a small island off the coast",
      "Fort Josa",
      "Chestervale",
      "Fern Islands",
      "Tanker Grove"
    };
    public static string[] colors = new string[16]
    {
      "/crimson",
      "/green",
      "/tan",
      "/purple",
      "/deep blue",
      "/neon pink",
      "/pale/yellow",
      "/chocolate/brown",
      "/sky/blue",
      "/bubblegum/pink",
      "/blood/red",
      "/bright/orange",
      "/aquamarine",
      "/silvery",
      "/glimmering/gold",
      "/rainbow"
    };
    private List<string> dialogues = new List<string>();
    public const string dialogueHappy = "$h";
    public const string dialogueSad = "$s";
    public const string dialogueUnique = "$u";
    public const string dialogueNeutral = "$neutral";
    public const string dialogueLove = "$l";
    public const string dialogueAngry = "$a";
    public const string dialogueEnd = "$e";
    public const string dialogueBreak = "$b";
    public const string dialogueKill = "$k";
    public const string dialogueChance = "$c";
    public const string dialogueDependingOnWorldState = "$d";
    public const string dialogueQuickResponse = "$y";
    public const string dialoguePrerequisite = "$p";
    public const string dialogueSingle = "$1";
    public const string dialogueQuestion = "$q";
    public const string dialogueResponse = "$r";
    public const string breakSpecialCharacter = "{";
    public const string playerNameSpecialCharacter = "@";
    public const string genderDialogueSplitCharacter = "^";
    public const string genderDialogueSplitCharacter2 = "¦";
    public const string quickResponseDelineator = "*";
    public const string randomAdjectiveSpecialCharacter = "%adj";
    public const string randomNounSpecialCharacter = "%noun";
    public const string randomPlaceSpecialCharacter = "%place";
    public const string spouseSpecialCharacter = "%spouse";
    public const string randomNameSpecialCharacter = "%name";
    public const string firstNameLettersSpecialCharacter = "%firstnameletter";
    public const string timeSpecialCharacter = "%time";
    public const string bandNameSpecialCharacter = "%band";
    public const string bookNameSpecialCharacter = "%book";
    public const string rivalSpecialCharacter = "%rival";
    public const string petSpecialCharacter = "%pet";
    public const string farmNameSpecialCharacter = "%farm";
    public const string favoriteThingSpecialCharacter = "%favorite";
    public const string eventForkSpecialCharacter = "%fork";
    public const string kid1specialCharacter = "%kid1";
    public const string kid2SpecialCharacter = "%kid2";
    private List<NPCDialogueResponse> playerResponses;
    private List<string> quickResponses;
    private bool isLastDialogueInteractive;
    private bool quickResponse;
    public bool isCurrentStringContinuedOnNextScreen;
    private bool dialogueToBeKilled;
    private bool finishedLastDialogue;
    public bool showPortrait;
    public bool removeOnNextMove;
    public int currentDialogueIndex;
    private string currentEmotion;
    public string temporaryDialogue;
    public NPC speaker;
    public Dialogue.onAnswerQuestion answerQuestionBehavior;

    private static void TranslateArraysOfStrings()
    {
      Dialogue.colors = new string[16]
      {
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.795"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.796"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.797"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.798"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.799"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.800"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.801"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.802"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.803"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.804"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.805"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.806"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.807"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.808"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.809"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.810")
      };
      Dialogue.adjectives = new string[20]
      {
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.679"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.680"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.681"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.682"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.683"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.684"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.685"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.686"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.687"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.688"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.689"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.690"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.691"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.692"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.693"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.694"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.695"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.696"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.697"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.698")
      };
      Dialogue.nouns = new string[23]
      {
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.699"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.700"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.701"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.702"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.703"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.704"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.705"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.706"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.707"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.708"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.709"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.710"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.711"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.712"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.713"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.714"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.715"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.716"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.717"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.718"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.719"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.720"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.721")
      };
      Dialogue.verbs = new string[13]
      {
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.722"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.723"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.724"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.725"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.726"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.727"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.728"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.729"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.730"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.731"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.732"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.733"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.734")
      };
      Dialogue.positional = new string[13]
      {
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.735"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.736"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.737"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.738"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.739"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.740"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.741"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.742"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.743"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.744"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.745"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.746"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.747")
      };
      Dialogue.places = new string[12]
      {
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.748"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.749"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.750"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.751"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.752"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.753"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.754"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.755"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.756"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.757"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.758"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.759")
      };
      Dialogue.nameArraysTranslated = true;
    }

    public string CurrentEmotion
    {
      get
      {
        return this.currentEmotion;
      }
      set
      {
        this.currentEmotion = value;
      }
    }

    public Dialogue(string masterDialogue, NPC speaker)
    {
      if (!Dialogue.nameArraysTranslated)
        Dialogue.TranslateArraysOfStrings();
      this.speaker = speaker;
      this.parseDialogueString(masterDialogue);
      this.checkForSpecialDialogueAttributes();
    }

    public void setCurrentDialogue(string dialogue)
    {
      this.dialogues.Clear();
      this.currentDialogueIndex = 0;
      this.parseDialogueString(dialogue);
    }

    public void addMessageToFront(string dialogue)
    {
      this.currentDialogueIndex = 0;
      List<string> stringList = new List<string>();
      stringList.AddRange((IEnumerable<string>) this.dialogues);
      this.dialogues.Clear();
      this.parseDialogueString(dialogue);
      this.dialogues.AddRange((IEnumerable<string>) stringList);
      this.checkForSpecialDialogueAttributes();
    }

    public static string getRandomVerb()
    {
      if (!Dialogue.nameArraysTranslated)
        Dialogue.TranslateArraysOfStrings();
      return Dialogue.verbs[Game1.random.Next(((IEnumerable<string>) Dialogue.verbs).Count<string>())];
    }

    public static string getRandomAdjective()
    {
      if (!Dialogue.nameArraysTranslated)
        Dialogue.TranslateArraysOfStrings();
      return Dialogue.adjectives[Game1.random.Next(((IEnumerable<string>) Dialogue.adjectives).Count<string>())];
    }

    public static string getRandomNoun()
    {
      if (!Dialogue.nameArraysTranslated)
        Dialogue.TranslateArraysOfStrings();
      return Dialogue.nouns[Game1.random.Next(((IEnumerable<string>) Dialogue.nouns).Count<string>())];
    }

    public static string getRandomPositional()
    {
      if (!Dialogue.nameArraysTranslated)
        Dialogue.TranslateArraysOfStrings();
      return Dialogue.positional[Game1.random.Next(((IEnumerable<string>) Dialogue.positional).Count<string>())];
    }

    public int getPortraitIndex()
    {
      string currentEmotion = this.currentEmotion;
      if (currentEmotion == "$neutral")
        return 0;
      if (currentEmotion == "$h")
        return 1;
      if (currentEmotion == "$s")
        return 2;
      if (currentEmotion == "$u")
        return 3;
      if (currentEmotion == "$l")
        return 4;
      if (currentEmotion == "$a")
        return 5;
      int result;
      if (int.TryParse(this.currentEmotion.Substring(1), out result))
        return Convert.ToInt32(this.currentEmotion.Substring(1));
      return 0;
    }

    private void parseDialogueString(string masterString)
    {
      if (masterString == null)
        masterString = "...";
      this.temporaryDialogue = (string) null;
      if (this.playerResponses != null)
        this.playerResponses.Clear();
      string[] strArray1 = masterString.Split('#');
      for (int count = 0; count < strArray1.Length; ++count)
      {
        if (strArray1[count].Length >= 2)
        {
          strArray1[count] = this.checkForSpecialCharacters(strArray1[count]);
          string str1;
          try
          {
            str1 = strArray1[count].Substring(0, 2);
          }
          catch (Exception ex)
          {
            str1 = "     ";
          }
          if (!str1.Equals("$e"))
          {
            if (str1.Equals("$b"))
            {
              if (this.dialogues.Count > 0)
              {
                List<string> dialogues = this.dialogues;
                int index = this.dialogues.Count - 1;
                dialogues[index] = dialogues[index] + "{";
              }
            }
            else if (str1.Equals("$k"))
            {
              this.dialogueToBeKilled = true;
            }
            else
            {
              if (str1.Equals("$1"))
              {
                if (strArray1[count].Split(' ').Length > 1)
                {
                  string str2 = strArray1[count].Split(' ')[1];
                  if (Game1.player.mailReceived.Contains(str2))
                  {
                    count += 3;
                    strArray1[count] = this.checkForSpecialCharacters(strArray1[count]);
                    continue;
                  }
                  strArray1[count + 1] = this.checkForSpecialCharacters(strArray1[count + 1]);
                  this.dialogues.Add(str2 + "}" + strArray1[count + 1]);
                  break;
                }
              }
              if (str1.Equals("$c"))
              {
                if (strArray1[count].Split(' ').Length > 1)
                {
                  double num = Convert.ToDouble(strArray1[count].Split(' ')[1]);
                  if (Game1.random.NextDouble() > num)
                  {
                    ++count;
                    continue;
                  }
                  this.dialogues.Add(strArray1[count + 1]);
                  count += 2;
                  continue;
                }
              }
              if (str1.Equals("$q"))
              {
                if (this.dialogues.Count > 0)
                {
                  List<string> dialogues = this.dialogues;
                  int index = this.dialogues.Count - 1;
                  dialogues[index] = dialogues[index] + "{";
                }
                string[] strArray2 = strArray1[count].Split(' ');
                string[] strArray3 = strArray2[1].Split('/');
                bool flag = false;
                for (int index = 0; index < strArray3.Length; ++index)
                {
                  if (Game1.player.DialogueQuestionsAnswered.Contains(Convert.ToInt32(strArray3[index])))
                  {
                    flag = true;
                    break;
                  }
                }
                if (flag && Convert.ToInt32(strArray3[0]) != -1)
                {
                  if (!strArray2[2].Equals("null"))
                  {
                    strArray1 = ((IEnumerable<string>) ((IEnumerable<string>) strArray1).Take<string>(count).ToArray<string>()).Concat<string>((IEnumerable<string>) this.speaker.Dialogue[strArray2[2]].Split('#')).ToArray<string>();
                    --count;
                  }
                }
                else
                  this.isLastDialogueInteractive = true;
              }
              else if (str1.Equals("$r"))
              {
                string[] strArray2 = strArray1[count].Split(' ');
                if (this.playerResponses == null)
                  this.playerResponses = new List<NPCDialogueResponse>();
                this.isLastDialogueInteractive = true;
                this.playerResponses.Add(new NPCDialogueResponse(Convert.ToInt32(strArray2[1]), Convert.ToInt32(strArray2[2]), strArray2[3], strArray1[count + 1]));
                ++count;
              }
              else if (str1.Equals("$p"))
              {
                string[] strArray2 = strArray1[count].Split(' ');
                string[] strArray3 = strArray1[count + 1].Split('|');
                bool flag = false;
                for (int index = 1; index < strArray2.Length; ++index)
                {
                  if (Game1.player.DialogueQuestionsAnswered.Contains(Convert.ToInt32(strArray2[1])))
                  {
                    flag = true;
                    break;
                  }
                }
                if (flag)
                {
                  strArray1 = strArray3[0].Split('#');
                  count = -1;
                }
                else
                  strArray1[count + 1] = ((IEnumerable<string>) strArray1[count + 1].Split('|')).Last<string>();
              }
              else if (str1.Equals("$d"))
              {
                string[] strArray2 = strArray1[count].Split(' ');
                string source = masterString.Substring(masterString.IndexOf('#') + 1);
                bool flag = false;
                int index = 1;
                string lower = strArray2[index].ToLower();
                if (!(lower == "joja"))
                {
                  if (!(lower == "cc") && !(lower == "communitycenter"))
                  {
                    if (lower == "bus")
                      flag = Game1.player.mailReceived.Contains("ccVault");
                  }
                  else
                    flag = Game1.isLocationAccessible("CommunityCenter");
                }
                else
                  flag = Game1.isLocationAccessible("JojaMart");
                char ch = source.Contains<char>('|') ? '|' : '#';
                if (flag)
                  strArray1 = new string[1]
                  {
                    source.Split(ch)[0]
                  };
                else
                  strArray1 = new string[1]
                  {
                    source.Split(ch)[1]
                  };
                --count;
              }
              else if (str1.Equals("$y"))
              {
                this.quickResponse = true;
                this.isLastDialogueInteractive = true;
                if (this.quickResponses == null)
                  this.quickResponses = new List<string>();
                if (this.playerResponses == null)
                  this.playerResponses = new List<NPCDialogueResponse>();
                string str2 = strArray1[count].Substring(strArray1[count].IndexOf('\'') + 1);
                string[] strArray2 = str2.Substring(0, str2.Length - 1).Split('_');
                this.dialogues.Add(strArray2[0]);
                int index = 1;
                while (index < strArray2.Length)
                {
                  this.playerResponses.Add(new NPCDialogueResponse(-1, -1, "quickResponse" + (object) index, Game1.parseText(strArray2[index])));
                  this.quickResponses.Add(strArray2[index + 1].Replace("*", "#$b#"));
                  index += 2;
                }
              }
              else if (strArray1[count].Contains("^"))
              {
                if (Game1.player.IsMale)
                  this.dialogues.Add(strArray1[count].Substring(0, strArray1[count].IndexOf("^")));
                else
                  this.dialogues.Add(strArray1[count].Substring(strArray1[count].IndexOf("^") + 1));
              }
              else if (strArray1[count].Contains("¦"))
              {
                if (Game1.player.IsMale)
                  this.dialogues.Add(strArray1[count].Substring(0, strArray1[count].IndexOf("¦")));
                else
                  this.dialogues.Add(strArray1[count].Substring(strArray1[count].IndexOf("¦") + 1));
              }
              else
                this.dialogues.Add(strArray1[count]);
            }
          }
        }
      }
    }

    public string getCurrentDialogue()
    {
      if (this.currentDialogueIndex >= this.dialogues.Count || this.finishedLastDialogue)
        return "";
      this.showPortrait = true;
      if (this.speaker.name.Equals("Dwarf") && !Game1.player.canUnderstandDwarves)
        return Dialogue.convertToDwarvish(this.dialogues[this.currentDialogueIndex]);
      if (this.temporaryDialogue != null)
        return this.temporaryDialogue;
      if (this.dialogues.Count > 0 && this.dialogues[this.currentDialogueIndex].Contains("}"))
      {
        Game1.player.mailReceived.Add(this.dialogues[this.currentDialogueIndex].Split('}')[0]);
        this.dialogues[this.currentDialogueIndex] = this.dialogues[this.currentDialogueIndex].Substring(this.dialogues[this.currentDialogueIndex].IndexOf("}") + 1);
        this.dialogues[this.currentDialogueIndex] = this.dialogues[this.currentDialogueIndex].Replace("$k", "");
      }
      if (this.dialogues.Count > 0 && this.dialogues[this.currentDialogueIndex].Contains<char>('['))
      {
        string str = this.dialogues[this.currentDialogueIndex].Substring(this.dialogues[this.currentDialogueIndex].IndexOf('[') + 1, this.dialogues[this.currentDialogueIndex].IndexOf(']') - this.dialogues[this.currentDialogueIndex].IndexOf('[') - 1);
        string[] strArray = str.Split(' ');
        Game1.player.addItemToInventoryBool((Item) new Object(Vector2.Zero, Convert.ToInt32(strArray[Game1.random.Next(strArray.Length)]), (string) null, false, true, false, false), true);
        Game1.player.showCarrying();
        this.dialogues[this.currentDialogueIndex] = this.dialogues[this.currentDialogueIndex].Replace("[" + str + "]", "");
      }
      if (this.dialogues.Count > 0 && this.dialogues[this.currentDialogueIndex].Contains("$k"))
      {
        this.dialogues[this.currentDialogueIndex] = this.dialogues[this.currentDialogueIndex].Replace("$k", "");
        this.dialogues.RemoveRange(this.currentDialogueIndex + 1, this.dialogues.Count - 1 - this.currentDialogueIndex);
        this.finishedLastDialogue = true;
      }
      if (this.dialogues.Count > 0 && this.dialogues[this.currentDialogueIndex].Length > 1 && (int) this.dialogues[this.currentDialogueIndex][0] == 37)
      {
        this.showPortrait = false;
        return this.dialogues[this.currentDialogueIndex].Substring(1);
      }
      if (this.dialogues.Count<string>() <= 0)
        return Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.792");
      return this.dialogues[this.currentDialogueIndex].Replace("%time", Game1.getTimeOfDayString(Game1.timeOfDay));
    }

    public bool isItemGrabDialogue()
    {
      if (this.dialogues.Count <= 0)
        return false;
      return this.dialogues[this.currentDialogueIndex].Contains<char>('[');
    }

    public bool isOnFinalDialogue()
    {
      return this.currentDialogueIndex == this.dialogues.Count - 1;
    }

    public bool isDialogueFinished()
    {
      return this.finishedLastDialogue;
    }

    public string checkForSpecialCharacters(string str)
    {
      str = str.Replace("@", Game1.player.Name);
      str = str.Replace("%adj", Dialogue.adjectives[Game1.random.Next(Dialogue.adjectives.Length)].ToLower());
      if (str.Contains("%noun"))
        str = LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.de ? str.Substring(0, str.IndexOf("%noun") + "%noun".Length).Replace("%noun", Dialogue.nouns[Game1.random.Next(Dialogue.nouns.Length)]) + str.Substring(str.IndexOf("%noun") + "%noun".Length).Replace("%noun", Dialogue.nouns[Game1.random.Next(Dialogue.nouns.Length)]) : str.Substring(0, str.IndexOf("%noun") + "%noun".Length).Replace("%noun", Dialogue.nouns[Game1.random.Next(Dialogue.nouns.Length)].ToLower()) + str.Substring(str.IndexOf("%noun") + "%noun".Length).Replace("%noun", Dialogue.nouns[Game1.random.Next(Dialogue.nouns.Length)].ToLower());
      str = str.Replace("%place", Dialogue.places[Game1.random.Next(Dialogue.places.Length)]);
      str = str.Replace("%name", Dialogue.randomName());
      str = str.Replace("%firstnameletter", Game1.player.Name.Substring(0, Math.Max(0, Game1.player.Name.Length / 2)));
      str = str.Replace("%band", Game1.samBandName);
      str = str.Replace("%book", Game1.elliottBookName);
      if (!string.IsNullOrEmpty(str) && str.Contains("%spouse"))
      {
        string[] strArray = Game1.content.Load<Dictionary<string, string>>("Data\\NPCDispositions")[Game1.player.spouse].Split('/');
        str = str.Replace("%spouse", strArray[strArray.Length - 1]);
      }
      str = str.Replace("%farm", Game1.player.farmName);
      str = str.Replace("%favorite", Game1.player.favoriteThing);
      int numberOfChildren = Game1.player.getNumberOfChildren();
      str = str.Replace("%kid1", numberOfChildren > 0 ? Game1.player.getChildren()[0].displayName : Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.793"));
      str = str.Replace("%kid2", numberOfChildren > 1 ? Game1.player.getChildren()[1].displayName : Game1.content.LoadString("Strings\\StringsFromCSFiles:Dialogue.cs.794"));
      str = str.Replace("%pet", Game1.player.getPetDisplayName());
      if (str.Contains("¦"))
        str = Game1.player.IsMale ? str.Substring(0, str.IndexOf("¦")) : str.Substring(str.IndexOf("¦") + 1);
      if (str.Contains("%fork"))
      {
        str = str.Replace("%fork", "");
        if (Game1.currentLocation.currentEvent != null)
          Game1.currentLocation.currentEvent.specialEventVariable1 = true;
      }
      str = str.Replace("%rival", Utility.getOtherFarmerNames()[0].Split(' ')[1]);
      return str;
    }

    public static string randomName()
    {
      string str1 = "";
      string source;
      if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ja)
      {
        string[] strArray = new string[38]
        {
          "ローゼン",
          "ミルド",
          "ココ",
          "ナミ",
          "こころ",
          "サルコ",
          "ハンゾー",
          "クッキー",
          "ココナツ",
          "せん",
          "ハル",
          "ラン",
          "オサム",
          "ヨシ",
          "ソラ",
          "ホシ",
          "まこと",
          "マサ",
          "ナナ",
          "リオ",
          "リン",
          "フジ",
          "うどん",
          "ミント",
          "さくら",
          "ボンボン",
          "レオ",
          "モリ",
          "コーヒー",
          "ミルク",
          "マロン",
          "クルミ",
          "サムライ",
          "カミ",
          "ゴロ",
          "マル",
          "チビ",
          "ユキダマ"
        };
        source = strArray[new Random().Next(0, strArray.Length)];
      }
      else if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.zh)
      {
        string[] strArray = new string[183]
        {
          "雨果",
          "蛋挞",
          "小百合",
          "毛毛",
          "小雨",
          "小溪",
          "精灵",
          "安琪儿",
          "小糕",
          "玫瑰",
          "小黄",
          "晓雨",
          "阿江",
          "铃铛",
          "马琪",
          "果粒",
          "郁金香",
          "小黑",
          "雨露",
          "小江",
          "灵力",
          "萝拉",
          "豆豆",
          "小莲",
          "斑点",
          "小雾",
          "阿川",
          "丽丹",
          "玛雅",
          "阿豆",
          "花花",
          "琉璃",
          "滴答",
          "阿山",
          "丹麦",
          "梅西",
          "橙子",
          "花儿",
          "晓璃",
          "小夕",
          "山大",
          "咪咪",
          "卡米",
          "红豆",
          "花朵",
          "洋洋",
          "太阳",
          "小岩",
          "汪汪",
          "玛利亚",
          "小菜",
          "花瓣",
          "阳阳",
          "小夏",
          "石头",
          "阿狗",
          "邱洁",
          "苹果",
          "梨花",
          "小希",
          "天天",
          "浪子",
          "阿猫",
          "艾薇儿",
          "雪梨",
          "桃花",
          "阿喜",
          "云朵",
          "风儿",
          "狮子",
          "绮丽",
          "雪莉",
          "樱花",
          "小喜",
          "朵朵",
          "田田",
          "小红",
          "宝娜",
          "梅子",
          "小樱",
          "嘻嘻",
          "云儿",
          "小草",
          "小黄",
          "纳香",
          "阿梅",
          "茶花",
          "哈哈",
          "芸儿",
          "东东",
          "小羽",
          "哈豆",
          "桃子",
          "茶叶",
          "双双",
          "沫沫",
          "楠楠",
          "小爱",
          "麦当娜",
          "杏仁",
          "椰子",
          "小王",
          "泡泡",
          "小林",
          "小灰",
          "马格",
          "鱼蛋",
          "小叶",
          "小李",
          "晨晨",
          "小琳",
          "小慧",
          "布鲁",
          "晓梅",
          "绿叶",
          "甜豆",
          "小雪",
          "晓林",
          "康康",
          "安妮",
          "樱桃",
          "香板",
          "甜甜",
          "雪花",
          "虹儿",
          "美美",
          "葡萄",
          "薇儿",
          "金豆",
          "雪玲",
          "瑶瑶",
          "龙眼",
          "丁香",
          "晓云",
          "雪豆",
          "琪琪",
          "麦子",
          "糖果",
          "雪丽",
          "小艺",
          "小麦",
          "小圆",
          "雨佳",
          "小火",
          "麦茶",
          "圆圆",
          "春儿",
          "火灵",
          "板子",
          "黑点",
          "冬冬",
          "火花",
          "米粒",
          "喇叭",
          "晓秋",
          "跟屁虫",
          "米果",
          "欢欢",
          "爱心",
          "松子",
          "丫头",
          "双子",
          "豆芽",
          "小子",
          "彤彤",
          "棉花糖",
          "阿贵",
          "仙儿",
          "冰淇淋",
          "小彬",
          "贤儿",
          "冰棒",
          "仔仔",
          "格子",
          "水果",
          "悠悠",
          "莹莹",
          "巧克力",
          "梦洁",
          "汤圆",
          "静香",
          "茄子",
          "珍珠"
        };
        source = strArray[new Random().Next(0, strArray.Length)];
      }
      else if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ru)
      {
        string[] strArray = new string[50]
        {
          "Августина",
          "Альф",
          "Анфиса",
          "Ариша",
          "Афоня",
          "Баламут",
          "Балкан",
          "Бандит",
          "Бланка",
          "Бобик",
          "Боня",
          "Борька",
          "Буренка",
          "Бусинка",
          "Вася",
          "Гаврюша",
          "Глаша",
          "Гоша",
          "Дуня",
          "Дуся",
          "Зорька",
          "Ивонна",
          "Игнат",
          "Кеша",
          "Клара",
          "Кузя",
          "Лада",
          "Максимус",
          "Маня",
          "Марта",
          "Маруся",
          "Моня",
          "Мотя",
          "Мурзик",
          "Мурка",
          "Нафаня",
          "Ника",
          "Нюша",
          "Проша",
          "Пятнушка",
          "Сеня",
          "Сивка",
          "Тихон",
          "Тоша",
          "Фунтик",
          "Шайтан",
          "Юнона",
          "Юпитер",
          "Ягодка",
          "Яшка"
        };
        source = strArray[new Random().Next(0, strArray.Length)];
      }
      else
      {
        int num = Game1.random.Next(3, 6);
        string[] strArray1 = new string[24]
        {
          "B",
          "Br",
          "J",
          "F",
          "S",
          "M",
          "C",
          "Ch",
          "L",
          "P",
          "K",
          "W",
          "G",
          "Z",
          "Tr",
          "T",
          "Gr",
          "Fr",
          "Pr",
          "N",
          "Sn",
          "R",
          "Sh",
          "St"
        };
        string[] strArray2 = new string[12]
        {
          "ll",
          "tch",
          "l",
          "m",
          "n",
          "p",
          "r",
          "s",
          "t",
          "c",
          "rt",
          "ts"
        };
        string[] strArray3 = new string[5]
        {
          "a",
          "e",
          "i",
          "o",
          "u"
        };
        string[] strArray4 = new string[5]
        {
          "ie",
          "o",
          "a",
          "ers",
          "ley"
        };
        Dictionary<string, string[]> dictionary1 = new Dictionary<string, string[]>();
        Dictionary<string, string[]> dictionary2 = new Dictionary<string, string[]>();
        dictionary1.Add("a", new string[6]
        {
          "nie",
          "bell",
          "bo",
          "boo",
          "bella",
          "s"
        });
        dictionary1.Add("e", new string[4]
        {
          "ll",
          "llo",
          "",
          "o"
        });
        dictionary1.Add("i", new string[18]
        {
          "ck",
          "e",
          "bo",
          "ba",
          "lo",
          "la",
          "to",
          "ta",
          "no",
          "na",
          "ni",
          "a",
          "o",
          "zor",
          "que",
          "ca",
          "co",
          "mi"
        });
        dictionary1.Add("o", new string[12]
        {
          "nie",
          "ze",
          "dy",
          "da",
          "o",
          "ver",
          "la",
          "lo",
          "s",
          "ny",
          "mo",
          "ra"
        });
        dictionary1.Add("u", new string[4]
        {
          "rt",
          "mo",
          "",
          "s"
        });
        dictionary2.Add("a", new string[12]
        {
          "nny",
          "sper",
          "trina",
          "bo",
          "-bell",
          "boo",
          "lbert",
          "sko",
          "sh",
          "ck",
          "ishe",
          "rk"
        });
        dictionary2.Add("e", new string[9]
        {
          "lla",
          "llo",
          "rnard",
          "cardo",
          "ffe",
          "ppo",
          "ppa",
          "tch",
          "x"
        });
        dictionary2.Add("i", new string[18]
        {
          "llard",
          "lly",
          "lbo",
          "cky",
          "card",
          "ne",
          "nnie",
          "lbert",
          "nono",
          "nano",
          "nana",
          "ana",
          "nsy",
          "msy",
          "skers",
          "rdo",
          "rda",
          "sh"
        });
        dictionary2.Add("o", new string[17]
        {
          "nie",
          "zzy",
          "do",
          "na",
          "la",
          "la",
          "ver",
          "ng",
          "ngus",
          "ny",
          "-mo",
          "llo",
          "ze",
          "ra",
          "ma",
          "cco",
          "z"
        });
        dictionary2.Add("u", new string[11]
        {
          "ssie",
          "bbie",
          "ffy",
          "bba",
          "rt",
          "s",
          "mby",
          "mbo",
          "mbus",
          "ngus",
          "cky"
        });
        source = str1 + strArray1[Game1.random.Next(strArray1.Length - 1)];
        for (int index = 1; index < num - 1; ++index)
        {
          source = index % 2 != 0 ? source + strArray3[Game1.random.Next(strArray3.Length)] : source + strArray2[Game1.random.Next(strArray2.Length)];
          if (source.Length >= num)
            break;
        }
        char ch;
        if (Game1.random.NextDouble() < 0.5 && !((IEnumerable<string>) strArray3).Contains<string>(source.ElementAt<char>(source.Length - 1).ToString() ?? ""))
          source += strArray4[Game1.random.Next(strArray4.Length)];
        else if (((IEnumerable<string>) strArray3).Contains<string>(source.ElementAt<char>(source.Length - 1).ToString() ?? ""))
        {
          if (Game1.random.NextDouble() < 0.8)
          {
            if (source.Length <= 3)
            {
              string str2 = source;
              string[] strArray5 = dictionary2[source.ElementAt<char>(source.Length - 1).ToString() ?? ""];
              Random random = Game1.random;
              Dictionary<string, string[]> dictionary3 = dictionary2;
              ch = source.ElementAt<char>(source.Length - 1);
              string index1 = ch.ToString() ?? "";
              int maxValue = dictionary3[index1].Length - 1;
              int index2 = random.Next(maxValue);
              string str3 = ((IEnumerable<string>) strArray5).ElementAt<string>(index2);
              source = str2 + str3;
            }
            else
            {
              string str2 = source;
              string[] strArray5 = dictionary1[source.ElementAt<char>(source.Length - 1).ToString() ?? ""];
              Random random = Game1.random;
              Dictionary<string, string[]> dictionary3 = dictionary1;
              ch = source.ElementAt<char>(source.Length - 1);
              string index1 = ch.ToString() ?? "";
              int maxValue = dictionary3[index1].Length - 1;
              int index2 = random.Next(maxValue);
              string str3 = ((IEnumerable<string>) strArray5).ElementAt<string>(index2);
              source = str2 + str3;
            }
          }
        }
        else
          source += strArray3[Game1.random.Next(strArray3.Length)];
        for (int index = source.Length - 1; index > 2; --index)
        {
          string[] strArray5 = strArray3;
          ch = source[index];
          string str2 = ch.ToString();
          if (((IEnumerable<string>) strArray5).Contains<string>(str2))
          {
            string[] strArray6 = strArray3;
            ch = source[index - 2];
            string str3 = ch.ToString();
            if (((IEnumerable<string>) strArray6).Contains<string>(str3))
            {
              ch = source[index - 1];
              switch (ch)
              {
                case 'c':
                  source = source.Substring(0, index) + "k" + source.Substring(index);
                  --index;
                  continue;
                case 'l':
                  source = source.Substring(0, index - 1) + "n" + source.Substring(index);
                  --index;
                  continue;
                case 'r':
                  source = source.Substring(0, index - 1) + "k" + source.Substring(index);
                  --index;
                  continue;
                default:
                  continue;
              }
            }
          }
        }
        if (source.Length <= 3 && Game1.random.NextDouble() < 0.1)
          source = Game1.random.NextDouble() < 0.5 ? source + source : source + "-" + source;
        if (source.Length <= 2 && (int) source.Last<char>() == 101)
          source += Game1.random.NextDouble() < 0.3 ? "m" : (Game1.random.NextDouble() < 0.5 ? "p" : "b");
        if (source.ToLower().Contains("sex") || source.ToLower().Contains("taboo") || (source.ToLower().Contains("fuck") || source.ToLower().Contains("rape")) || (source.ToLower().Contains("cock") || source.ToLower().Contains("willy") || (source.ToLower().Contains("cum") || source.ToLower().Contains("goock"))) || (source.ToLower().Contains("trann") || source.ToLower().Contains("gook") || (source.ToLower().Contains("bitch") || source.ToLower().Contains("shit")) || (source.ToLower().Contains("pusie") || source.ToLower().Contains("kike") || (source.ToLower().Contains("nigg") || source.ToLower().Contains("puss")))))
          source = Game1.random.NextDouble() < 0.5 ? "Bobo" : "Wumbus";
      }
      return source;
    }

    public string exitCurrentDialogue()
    {
      if (this.temporaryDialogue != null)
        return (string) null;
      int num = this.isCurrentStringContinuedOnNextScreen ? 1 : 0;
      if (this.currentDialogueIndex < this.dialogues.Count - 1)
      {
        this.currentDialogueIndex = this.currentDialogueIndex + 1;
        this.checkForSpecialDialogueAttributes();
      }
      else
        this.finishedLastDialogue = true;
      if (num != 0)
        return this.getCurrentDialogue();
      return (string) null;
    }

    private void checkForSpecialDialogueAttributes()
    {
      if (this.dialogues.Count > 0 && this.dialogues[this.currentDialogueIndex].Contains("{"))
      {
        this.dialogues[this.currentDialogueIndex] = this.dialogues[this.currentDialogueIndex].Replace("{", "");
        this.isCurrentStringContinuedOnNextScreen = true;
      }
      else
        this.isCurrentStringContinuedOnNextScreen = false;
      this.checkEmotions();
    }

    private void checkEmotions()
    {
      if (this.dialogues.Count > 0 && this.dialogues[this.currentDialogueIndex].Contains("$h"))
      {
        this.currentEmotion = "$h";
        this.dialogues[this.currentDialogueIndex] = this.dialogues[this.currentDialogueIndex].Replace("$h", "");
      }
      else if (this.dialogues.Count > 0 && this.dialogues[this.currentDialogueIndex].Contains("$s"))
      {
        this.currentEmotion = "$s";
        this.dialogues[this.currentDialogueIndex] = this.dialogues[this.currentDialogueIndex].Replace("$s", "");
      }
      else if (this.dialogues.Count > 0 && this.dialogues[this.currentDialogueIndex].Contains("$u"))
      {
        this.currentEmotion = "$u";
        this.dialogues[this.currentDialogueIndex] = this.dialogues[this.currentDialogueIndex].Replace("$u", "");
      }
      else if (this.dialogues.Count > 0 && this.dialogues[this.currentDialogueIndex].Contains("$l"))
      {
        this.currentEmotion = "$l";
        this.dialogues[this.currentDialogueIndex] = this.dialogues[this.currentDialogueIndex].Replace("$l", "");
      }
      else if (this.dialogues.Count > 0 && this.dialogues[this.currentDialogueIndex].Contains("$a"))
      {
        this.currentEmotion = "$a";
        this.dialogues[this.currentDialogueIndex] = this.dialogues[this.currentDialogueIndex].Replace("$a", "");
      }
      else if (this.dialogues.Count > 0 && this.dialogues[this.currentDialogueIndex].Contains("$"))
      {
        string oldValue = this.dialogues[this.currentDialogueIndex].Substring(this.dialogues[this.currentDialogueIndex].IndexOf("$"), (this.dialogues[this.currentDialogueIndex].Length <= this.dialogues[this.currentDialogueIndex].IndexOf("$") + 2 || !char.IsDigit(this.dialogues[this.currentDialogueIndex][this.dialogues[this.currentDialogueIndex].IndexOf("$") + 2]) ? 1 : 2) + 1);
        this.currentEmotion = oldValue;
        this.dialogues[this.currentDialogueIndex] = this.dialogues[this.currentDialogueIndex].Replace(oldValue, "");
      }
      else
        this.currentEmotion = "$neutral";
    }

    public List<NPCDialogueResponse> getNPCResponseOptions()
    {
      return this.playerResponses;
    }

    public List<Response> getResponseOptions()
    {
      return new List<Response>(this.playerResponses.Select<NPCDialogueResponse, Response>((Func<NPCDialogueResponse, Response>) (x => (Response) x)));
    }

    public bool isCurrentDialogueAQuestion()
    {
      if (this.isLastDialogueInteractive)
        return this.currentDialogueIndex == this.dialogues.Count - 1;
      return false;
    }

    public bool chooseResponse(Response response)
    {
      for (int whichResponse = 0; whichResponse < this.playerResponses.Count; ++whichResponse)
      {
        if (this.playerResponses[whichResponse].responseKey != null && response.responseKey != null && this.playerResponses[whichResponse].responseKey.Equals(response.responseKey))
        {
          if (this.answerQuestionBehavior != null)
          {
            if (this.answerQuestionBehavior(whichResponse))
              Game1.currentSpeaker = (NPC) null;
            this.isLastDialogueInteractive = false;
            this.finishedLastDialogue = true;
            this.answerQuestionBehavior = (Dialogue.onAnswerQuestion) null;
            return true;
          }
          if (this.quickResponse)
          {
            this.isLastDialogueInteractive = false;
            this.finishedLastDialogue = true;
            this.isCurrentStringContinuedOnNextScreen = true;
            this.speaker.setNewDialogue(this.quickResponses[whichResponse], false, false);
            Game1.drawDialogue(this.speaker);
            this.speaker.faceTowardFarmerForPeriod(4000, 3, false, Game1.player);
            return true;
          }
          if (Game1.isFestival())
          {
            Game1.currentLocation.currentEvent.answerDialogueQuestion(this.speaker, this.playerResponses[whichResponse].responseKey);
            this.isLastDialogueInteractive = false;
            this.finishedLastDialogue = true;
            return false;
          }
          Game1.player.changeFriendship(this.playerResponses[whichResponse].friendshipChange, this.speaker);
          if (this.playerResponses[whichResponse].id != -1)
            Game1.player.addSeenResponse(this.playerResponses[whichResponse].id);
          this.isLastDialogueInteractive = false;
          this.finishedLastDialogue = false;
          this.parseDialogueString(this.speaker.Dialogue[this.playerResponses[whichResponse].responseKey]);
          this.isCurrentStringContinuedOnNextScreen = true;
          return false;
        }
      }
      return false;
    }

    public static string convertToDwarvish(string str)
    {
      string str1 = "";
      for (int index = 0; index < str.Length; ++index)
      {
        char ch = str[index];
        if ((uint) ch <= 63U)
        {
          if ((uint) ch <= 39U)
          {
            switch (ch)
            {
              case '\n':
                continue;
              case ' ':
              case '!':
              case '"':
              case '\'':
                break;
              default:
                goto label_39;
            }
          }
          else if ((uint) ch <= 53U)
          {
            switch (ch)
            {
              case ',':
              case '.':
                break;
              case '0':
                str1 += "Q";
                continue;
              case '1':
                str1 += "M";
                continue;
              case '5':
                str1 += "X";
                continue;
              default:
                goto label_39;
            }
          }
          else if ((int) ch != 57)
          {
            if ((int) ch != 63)
              goto label_39;
          }
          else
          {
            str1 += "V";
            continue;
          }
        }
        else if ((uint) ch <= 73U)
        {
          if ((int) ch != 65)
          {
            if ((int) ch != 69)
            {
              if ((int) ch == 73)
              {
                str1 += "E";
                continue;
              }
              goto label_39;
            }
            else
            {
              str1 += "U";
              continue;
            }
          }
          else
          {
            str1 += "O";
            continue;
          }
        }
        else if ((uint) ch <= 117U)
        {
          switch (ch)
          {
            case 'O':
              str1 += "A";
              continue;
            case 'U':
              str1 += "I";
              continue;
            case 'Y':
              str1 += "Ol";
              continue;
            case 'Z':
              str1 += "B";
              continue;
            case 'a':
              str1 += "o";
              continue;
            case 'c':
              str1 += "t";
              continue;
            case 'd':
              str1 += "p";
              continue;
            case 'e':
              str1 += "u";
              continue;
            case 'g':
              str1 += "l";
              continue;
            case 'h':
            case 'm':
            case 's':
              break;
            case 'i':
              str1 += "e";
              continue;
            case 'n':
            case 'p':
              continue;
            case 'o':
              str1 += "a";
              continue;
            case 't':
              str1 += "n";
              continue;
            case 'u':
              str1 += "i";
              continue;
            default:
              goto label_39;
          }
        }
        else if ((int) ch != 121)
        {
          if ((int) ch == 122)
          {
            str1 += "b";
            continue;
          }
          goto label_39;
        }
        else
        {
          str1 += "ol";
          continue;
        }
        string str2 = str1;
        ch = str[index];
        string str3 = ch.ToString();
        str1 = str2 + str3;
        continue;
label_39:
        if (char.IsLetterOrDigit(str[index]))
        {
          string str4 = str1;
          ch = (char) ((uint) str[index] + 2U);
          string str5 = ch.ToString();
          str1 = str4 + str5;
        }
      }
      return str1.Replace("nhu", "doo");
    }

    public delegate bool onAnswerQuestion(int whichResponse);
  }
}
