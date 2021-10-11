// Decompiled with JetBrains decompiler
// Type: StardewValley.IKeyboardSubscriber
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework.Input;

namespace StardewValley
{
  public interface IKeyboardSubscriber
  {
    void RecieveTextInput(char inputChar);

    void RecieveTextInput(string text);

    void RecieveCommandInput(char command);

    void RecieveSpecialInput(Keys key);

    bool Selected { get; set; }
  }
}
