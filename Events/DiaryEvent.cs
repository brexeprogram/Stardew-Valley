// Decompiled with JetBrains decompiler
// Type: StardewValley.Events.DiaryEvent
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley.Events
{
  public class DiaryEvent : FarmEvent
  {
    public string NPCname;

    public bool setUp()
    {
      if (Game1.player.isMarried())
        return true;
      foreach (string str1 in Game1.player.mailReceived)
      {
        if (str1.Contains("diary"))
        {
          string str2 = str1.Split('_')[1];
          if (!Game1.player.mailReceived.Contains("diary_" + str2 + "_finished"))
          {
            Convert.ToInt32(str2.Split('/')[1]);
            this.NPCname = str2.Split('/')[0];
            NPC characterFromName = Game1.getCharacterFromName(this.NPCname, false);
            int gender = characterFromName.gender;
            Game1.player.mailReceived.Add("diary_" + str2 + "_finished");
            string text = (Game1.player.isMale ? Game1.content.LoadString("Strings\\StringsFromCSFiles:DiaryEvent.cs.6658") : Game1.content.LoadString("Strings\\StringsFromCSFiles:DiaryEvent.cs.6660")) + Environment.NewLine + Environment.NewLine + "-" + Utility.capitalizeFirstLetter(Game1.CurrentSeasonDisplayName) + " " + (object) Game1.dayOfMonth + "-" + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:DiaryEvent.cs.6664", (object) this.NPCname);
            Response[] answerChoices = new Response[3]
            {
              new Response("...We're", Game1.content.LoadString("Strings\\StringsFromCSFiles:DiaryEvent.cs.6667")),
              new Response("...I", characterFromName.gender == 0 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:DiaryEvent.cs.6669") : Game1.content.LoadString("Strings\\StringsFromCSFiles:DiaryEvent.cs.6670")),
              new Response("(Write", Game1.content.LoadString("Strings\\StringsFromCSFiles:DiaryEvent.cs.6672"))
            };
            Game1.currentLocation.createQuestionDialogue(Game1.parseText(text), answerChoices, "diary");
            Game1.messagePause = true;
            return false;
          }
        }
      }
      return true;
    }

    public bool tickUpdate(GameTime time)
    {
      return !Game1.dialogueUp;
    }

    public void draw(SpriteBatch b)
    {
    }

    public void makeChangesToLocation()
    {
      Game1.messagePause = false;
    }

    public void drawAboveEverything(SpriteBatch b)
    {
    }
  }
}
