// Decompiled with JetBrains decompiler
// Type: StardewValley.KeyboardLayout
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using System.Runtime.InteropServices;
using System.Text;

namespace StardewValley
{
  public class KeyboardLayout
  {
    private const uint KLF_ACTIVATE = 1;
    private const int KL_NAMELENGTH = 9;
    private const string LANG_EN_US = "00000409";
    private const string LANG_HE_IL = "0001101A";

    [DllImport("user32.dll")]
    private static extern long LoadKeyboardLayout(string pwszKLID, uint Flags);

    [DllImport("user32.dll")]
    private static extern long GetKeyboardLayoutName(StringBuilder pwszKLID);

    public static string getName()
    {
      StringBuilder pwszKLID = new StringBuilder(9);
      KeyboardLayout.GetKeyboardLayoutName(pwszKLID);
      return pwszKLID.ToString();
    }
  }
}
