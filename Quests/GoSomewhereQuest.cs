// Decompiled with JetBrains decompiler
// Type: StardewValley.Quests.GoSomewhereQuest
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

namespace StardewValley.Quests
{
  public class GoSomewhereQuest : Quest
  {
    public string whereToGo;

    public GoSomewhereQuest()
    {
    }

    public GoSomewhereQuest(string where)
    {
      this.whereToGo = where;
    }

    public override void adjustGameLocation(GameLocation location)
    {
      this.checkIfComplete((NPC) null, -1, -2, (Item) null, location.name);
    }

    public override bool checkIfComplete(NPC n = null, int number1 = -1, int number2 = -2, Item item = null, string str = null)
    {
      if (str == null || !str.Equals(this.whereToGo))
        return false;
      this.questComplete();
      return true;
    }
  }
}
