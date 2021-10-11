// Decompiled with JetBrains decompiler
// Type: StardewValley.Minigames.Test
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.Objects;
using System.Collections.Generic;

namespace StardewValley.Minigames
{
  public class Test : IMinigame
  {
    public List<Wallpaper> wallpaper = new List<Wallpaper>();

    public Test()
    {
      for (int which = 0; which < 40; ++which)
        this.wallpaper.Add(new Wallpaper(which, true));
    }

    public bool overrideFreeMouseMovement()
    {
      return false;
    }

    public bool tick(GameTime time)
    {
      return false;
    }

    public void afterFade()
    {
    }

    public void receiveLeftClick(int x, int y, bool playSound = true)
    {
      Game1.currentMinigame = (IMinigame) null;
    }

    public void leftClickHeld(int x, int y)
    {
    }

    public void receiveRightClick(int x, int y, bool playSound = true)
    {
    }

    public void releaseLeftClick(int x, int y)
    {
    }

    public void releaseRightClick(int x, int y)
    {
    }

    public void receiveKeyPress(Keys k)
    {
    }

    public void receiveKeyRelease(Keys k)
    {
    }

    public void draw(SpriteBatch b)
    {
      b.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
      b.Draw(Game1.staminaRect, new Rectangle(0, 0, 2000, 2000), Color.White);
      Vector2 location = new Vector2((float) (Game1.tileSize / 4), (float) (Game1.tileSize / 4));
      for (int index = 0; index < this.wallpaper.Count; ++index)
      {
        this.wallpaper[index].drawInMenu(b, location, 1f);
        location.X += (float) (Game1.tileSize * 2);
        if ((double) location.X >= (double) (Game1.graphics.GraphicsDevice.Viewport.Width - Game1.tileSize * 2))
        {
          location.X = (float) (Game1.tileSize / 4);
          location.Y += (float) (Game1.tileSize * 2);
        }
      }
      b.End();
    }

    public void changeScreenSize()
    {
    }

    public void unload()
    {
    }

    public void receiveEventPoke(int data)
    {
    }

    public string minigameId()
    {
      return (string) null;
    }
  }
}
