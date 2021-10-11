// Decompiled with JetBrains decompiler
// Type: StardewValley.Quests.WeedingQuest
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace StardewValley.Quests
{
  public class WeedingQuest : Quest
  {
    public List<DescriptionElement> parts = new List<DescriptionElement>();
    public DescriptionElement dialogue = new DescriptionElement();
    public NPC target;
    public string targetMessage;
    public bool complete;
    public int totalWeeds;
    public DescriptionElement objective;

    public WeedingQuest()
    {
      this.questType = 11;
    }

    public void loadQuestInfo()
    {
      GameLocation locationFromName = Game1.getLocationFromName("Town");
      for (int index = 0; index < 10; ++index)
        locationFromName.spawnWeeds(true);
      this.target = Game1.getCharacterFromName("Lewis", false);
      this.parts.Clear();
      this.parts.Add((DescriptionElement) "Strings\\StringsFromCSFiles:WeedingQuest.cs.13816");
      this.parts.Add((DescriptionElement) "Strings\\StringsFromCSFiles:WeedingQuest.cs.13817");
      this.parts.Add((DescriptionElement) "Strings\\StringsFromCSFiles:SocializeQuest.cs.13791");
      this.dialogue = (DescriptionElement) "Strings\\StringsFromCSFiles:WeedingQuest.cs.13819";
      this.currentObjective = "";
    }

    public override void accept()
    {
      base.accept();
      foreach (Object @object in Game1.getLocationFromName("Town").Objects.Values)
      {
        if (@object.name.Contains("Weed"))
          this.totalWeeds = this.totalWeeds + 1;
      }
      this.checkIfComplete((NPC) null, -1, -1, (Item) null, (string) null);
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
      this.targetMessage = this.dialogue.loadDescriptionElement();
    }

    private int weedsLeft()
    {
      int num = 0;
      foreach (Object @object in Game1.getLocationFromName("Town").Objects.Values)
      {
        if (@object.name.Contains("Weed"))
          ++num;
      }
      return num;
    }

    public override void reloadObjective()
    {
      if (this.weedsLeft() > 0)
        this.objective = new DescriptionElement("Strings\\StringsFromCSFiles:WeedingQuest.cs.13826", (object) (this.totalWeeds - this.weedsLeft()), (object) this.totalWeeds);
      if (this.objective == null)
        return;
      this.currentObjective = this.objective.loadDescriptionElement();
    }

    public override bool checkIfComplete(NPC n = null, int number1 = -1, int number2 = -1, Item item = null, string monsterName = null)
    {
      if (n == null && !this.complete)
      {
        if (this.weedsLeft() == 0)
        {
          this.complete = true;
          this.objective = new DescriptionElement("Strings\\StringsFromCSFiles:WeedingQuest.cs.13824");
          Game1.playSound("jingle1");
        }
        if (Game1.currentLocation.Name.Equals("Town"))
          Game1.dayTimeMoneyBox.moneyDial.animations.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(387, 497, 3, 8), 800f, 1, 0, Game1.dayTimeMoneyBox.position + new Vector2(220f, 260f), false, false, 1f, 0.01f, Color.White, 4f, 0.3f, 0.0f, 0.0f, false)
          {
            scaleChangeChange = -0.015f
          });
      }
      else if (n != null && n.Equals((object) this.target) && this.complete)
      {
        n.CurrentDialogue.Push(new Dialogue(this.targetMessage, n));
        this.completed = true;
        Game1.player.Money += 300;
        foreach (string key in Game1.player.friendships.Keys)
        {
          if (Game1.player.friendships[key][0] < 2729)
            Game1.player.friendships[key][0] += 20;
        }
        this.questComplete();
        return true;
      }
      return false;
    }
  }
}
