// Decompiled with JetBrains decompiler
// Type: StardewValley.Quests.LostItemQuest
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace StardewValley.Quests
{
  public class LostItemQuest : Quest
  {
    public string npcName;
    public string locationOfItem;
    public int itemIndex;
    public int tileX;
    public int tileY;
    public bool itemFound;
    public DescriptionElement objective;

    public LostItemQuest()
    {
    }

    public LostItemQuest(string npcName, string locationOfItem, int itemIndex, int tileX, int tileY)
    {
      this.npcName = npcName;
      this.locationOfItem = locationOfItem;
      this.itemIndex = itemIndex;
      this.tileX = tileX;
      this.tileY = tileY;
      this.questType = 9;
    }

    public override void adjustGameLocation(GameLocation location)
    {
      if (this.itemFound || !location.name.Equals(this.locationOfItem))
        return;
      Vector2 vector2 = new Vector2((float) this.tileX, (float) this.tileY);
      if (location.objects.ContainsKey(vector2))
        location.objects.Remove(vector2);
      location.objects.Add(vector2, new Object(vector2, this.itemIndex, 1)
      {
        questItem = true,
        isSpawnedObject = true
      });
    }

    public new void reloadObjective()
    {
      if (this.objective == null)
        return;
      this.currentObjective = this.objective.loadDescriptionElement();
    }

    public override bool checkIfComplete(NPC n = null, int number1 = -1, int number2 = -2, Item item = null, string str = null)
    {
      if (this.completed)
        return false;
      if (item != null && item is Object && ((item as Object).parentSheetIndex == this.itemIndex && !this.itemFound))
      {
        this.itemFound = true;
        string str1 = this.npcName;
        NPC characterFromName = Game1.getCharacterFromName(this.npcName, false);
        if (characterFromName != null)
          str1 = characterFromName.displayName;
        Game1.player.completelyStopAnimatingOrDoingAction();
        Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Quests:MessageFoundLostItem", (object) item.DisplayName, (object) str1));
        this.objective = new DescriptionElement("Strings\\Quests:ObjectiveReturnToNPC", (object) characterFromName);
        Game1.playSound("jingle1");
      }
      else if (n != null && n.name.Equals(this.npcName) && (n.isVillager() && this.itemFound) && Game1.player.hasItemInInventory(this.itemIndex, 1, 0))
      {
        this.questComplete();
        Dictionary<int, string> dictionary = Game1.temporaryContent.Load<Dictionary<int, string>>("Data\\Quests");
        string str1;
        if (dictionary[this.id].Length <= 9)
          str1 = Game1.content.LoadString("Data\\ExtraDialogue:LostItemQuest_DefaultThankYou");
        else
          str1 = dictionary[this.id].Split('/')[9];
        string s = str1;
        n.setNewDialogue(s, false, false);
        Game1.drawDialogue(n);
        Game1.player.changeFriendship(250, n);
        Game1.player.removeFirstOfThisItemFromInventory(this.itemIndex);
        return true;
      }
      return false;
    }
  }
}
