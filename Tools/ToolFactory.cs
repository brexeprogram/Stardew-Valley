// Decompiled with JetBrains decompiler
// Type: StardewValley.Tools.ToolFactory
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

namespace StardewValley.Tools
{
  public class ToolFactory
  {
    public const byte axe = 0;
    public const byte hoe = 1;
    public const byte fishingRod = 2;
    public const byte pickAxe = 3;
    public const byte wateringCan = 4;
    public const byte meleeWeapon = 5;
    public const byte slingshot = 6;

    public static ToolDescription getIndexFromTool(Tool t)
    {
      if (t is Axe)
        return new ToolDescription((byte) 0, (byte) t.upgradeLevel);
      if (t is Hoe)
        return new ToolDescription((byte) 1, (byte) t.upgradeLevel);
      if (t is FishingRod)
        return new ToolDescription((byte) 2, (byte) t.upgradeLevel);
      if (t is Pickaxe)
        return new ToolDescription((byte) 3, (byte) t.upgradeLevel);
      if (t is WateringCan)
        return new ToolDescription((byte) 4, (byte) t.upgradeLevel);
      if (t is MeleeWeapon)
        return new ToolDescription((byte) 5, (byte) t.upgradeLevel);
      if (t is Slingshot)
        return new ToolDescription((byte) 6, (byte) t.upgradeLevel);
      return new ToolDescription((byte) 0, (byte) 0);
    }

    public static Tool getToolFromDescription(byte index, int upgradeLevel)
    {
      Tool tool = (Tool) null;
      switch (index)
      {
        case 0:
          tool = (Tool) new Axe();
          break;
        case 1:
          tool = (Tool) new Hoe();
          break;
        case 2:
          tool = (Tool) new FishingRod();
          break;
        case 3:
          tool = (Tool) new Pickaxe();
          break;
        case 4:
          tool = (Tool) new WateringCan();
          break;
        case 5:
          tool = (Tool) new MeleeWeapon(0, upgradeLevel);
          break;
        case 6:
          tool = (Tool) new Slingshot();
          break;
      }
      tool.UpgradeLevel = upgradeLevel;
      return tool;
    }
  }
}
