// Decompiled with JetBrains decompiler
// Type: StardewValley.KeyEventArgs
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework.Input;
using System;

namespace StardewValley
{
  public class KeyEventArgs : EventArgs
  {
    private Keys keyCode;

    public KeyEventArgs(Keys keyCode)
    {
      this.keyCode = keyCode;
    }

    public Keys KeyCode
    {
      get
      {
        return this.keyCode;
      }
    }
  }
}
