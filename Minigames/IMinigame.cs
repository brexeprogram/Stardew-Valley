// Decompiled with JetBrains decompiler
// Type: StardewValley.Minigames.IMinigame
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StardewValley.Minigames
{
  public interface IMinigame
  {
    bool tick(GameTime time);

    bool overrideFreeMouseMovement();

    void receiveLeftClick(int x, int y, bool playSound = true);

    void leftClickHeld(int x, int y);

    void receiveRightClick(int x, int y, bool playSound = true);

    void releaseLeftClick(int x, int y);

    void releaseRightClick(int x, int y);

    void receiveKeyPress(Keys k);

    void receiveKeyRelease(Keys k);

    void draw(SpriteBatch b);

    void changeScreenSize();

    void unload();

    void receiveEventPoke(int data);

    string minigameId();
  }
}
