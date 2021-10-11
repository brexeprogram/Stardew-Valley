// Decompiled with JetBrains decompiler
// Type: StardewValley.Quests.CraftingQuest
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

namespace StardewValley.Quests
{
  public class CraftingQuest : Quest
  {
    public bool isBigCraftable;
    public int indexToCraft;

    public CraftingQuest()
    {
    }

    public CraftingQuest(int indexToCraft, bool bigCraftable)
    {
      this.indexToCraft = indexToCraft;
      this.isBigCraftable = bigCraftable;
    }

    public override bool checkIfComplete(NPC n = null, int number1 = -1, int number2 = -2, Item item = null, string str = null)
    {
      if (item == null || !(item is Object) || ((item as Object).bigCraftable != this.isBigCraftable || (item as Object).parentSheetIndex != this.indexToCraft))
        return false;
      this.questComplete();
      return true;
    }
  }
}
