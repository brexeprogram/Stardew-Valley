// Decompiled with JetBrains decompiler
// Type: StardewValley.Quests.ItemHarvestQuest
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

namespace StardewValley.Quests
{
  public class ItemHarvestQuest : Quest
  {
    public int itemIndex;
    public int number;

    public ItemHarvestQuest()
    {
    }

    public ItemHarvestQuest(int index, int number = 1)
    {
      this.itemIndex = index;
      this.number = number;
      this.questType = 9;
    }

    public override bool checkIfComplete(NPC n = null, int itemIndex = -1, int numberHarvested = 1, Item item = null, string str = null)
    {
      if (!this.completed && itemIndex != -1 && itemIndex == this.itemIndex)
      {
        this.number = this.number - numberHarvested;
        if (this.number <= 0)
        {
          this.questComplete();
          return true;
        }
      }
      return false;
    }
  }
}
