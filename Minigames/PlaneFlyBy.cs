// Decompiled with JetBrains decompiler
// Type: StardewValley.Minigames.PlaneFlyBy
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace StardewValley.Minigames
{
  public class PlaneFlyBy : IMinigame
  {
    public const float backGroundSpeed = 0.25f;
    public const float robotSpeed = 1f;
    public const int skyLength = 2560;
    public int millisecondsSinceStart;
    public int backgroundPosition;
    public int smokeTimer;
    public Vector2 robotPosition;
    public List<TemporaryAnimatedSprite> tempSprites;

    public bool overrideFreeMouseMovement()
    {
      return false;
    }

    public bool tick(GameTime time)
    {
      this.millisecondsSinceStart = this.millisecondsSinceStart + time.ElapsedGameTime.Milliseconds;
      this.robotPosition.X -= (float) (1.0 * (double) time.ElapsedGameTime.Milliseconds / 4.0);
      this.smokeTimer = this.smokeTimer - time.ElapsedGameTime.Milliseconds;
      if (this.smokeTimer <= 0)
      {
        this.smokeTimer = 100;
        this.tempSprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(173, 1828, 15, 20), 1500f, 2, 0, this.robotPosition + new Vector2((float) (17 * Game1.pixelZoom), (float) (-6 * Game1.pixelZoom)), false, false)
        {
          motion = new Vector2(0.0f, 0.1f),
          scale = (float) Game1.pixelZoom,
          scaleChange = 1f / 500f,
          alphaFade = 1f / 400f,
          rotation = -1.570796f
        });
      }
      for (int index = this.tempSprites.Count - 1; index >= 0; --index)
      {
        if (this.tempSprites[index].update(time))
          this.tempSprites.RemoveAt(index);
      }
      if ((double) this.robotPosition.X < (double) (-Game1.tileSize * 2) && !Game1.globalFade)
        Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.afterFade), 3f / 500f);
      return false;
    }

    public void afterFade()
    {
      Game1.currentMinigame = (IMinigame) null;
      Game1.globalFadeToClear((Game1.afterFadeFunction) null, 0.02f);
      if (Game1.currentLocation.currentEvent == null)
        return;
      ++Game1.currentLocation.currentEvent.CurrentCommand;
      Game1.currentLocation.temporarySprites.Clear();
    }

    public void receiveLeftClick(int x, int y, bool playSound = true)
    {
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
      if (k != Keys.Escape)
        return;
      this.robotPosition.X = -1000f;
      this.tempSprites.Clear();
    }

    public void receiveKeyRelease(Keys k)
    {
    }

    public void draw(SpriteBatch b)
    {
      b.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
      b.Draw(Game1.mouseCursors, new Rectangle(0, this.backgroundPosition, Game1.graphics.GraphicsDevice.Viewport.Width, 2560), new Rectangle?(new Rectangle(264, 1858, 1, 84)), Color.White);
      b.Draw(Game1.mouseCursors, new Vector2(0.0f, (float) this.backgroundPosition), new Rectangle?(new Rectangle(0, 1454, 639, 188)), Color.White * 0.5f, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1f);
      b.Draw(Game1.mouseCursors, new Vector2(0.0f, (float) (this.backgroundPosition - 188 * Game1.pixelZoom)), new Rectangle?(new Rectangle(0, 1454, 639, 188)), Color.White * 0.75f, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1f);
      b.Draw(Game1.mouseCursors, new Vector2(0.0f, (float) (this.backgroundPosition - 188 * Game1.pixelZoom * 2)), new Rectangle?(new Rectangle(0, 1454, 639, 188)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1f);
      b.Draw(Game1.mouseCursors, new Vector2(0.0f, (float) (this.backgroundPosition - 188 * Game1.pixelZoom * 3)), new Rectangle?(new Rectangle(0, 1454, 639, 188)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1f);
      b.Draw(Game1.mouseCursors, this.robotPosition, new Rectangle?(new Rectangle(222 + this.millisecondsSinceStart / 50 % 2 * 20, 1890, 20, 9)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1f);
      foreach (TemporaryAnimatedSprite tempSprite in this.tempSprites)
        tempSprite.draw(b, true, 0, 0);
      b.End();
    }

    public void changeScreenSize()
    {
      this.backgroundPosition = 2560 - Game1.graphics.GraphicsDevice.Viewport.Height;
      Viewport viewport = Game1.graphics.GraphicsDevice.Viewport;
      double num = (double) (viewport.Width / 2);
      viewport = Game1.graphics.GraphicsDevice.Viewport;
      double height = (double) viewport.Height;
      this.robotPosition = new Vector2((float) num, (float) height);
    }

    public void unload()
    {
    }

    public void receiveEventPoke(int data)
    {
      throw new NotImplementedException();
    }

    public string minigameId()
    {
      return (string) null;
    }

    public PlaneFlyBy()
    {
      Viewport viewport = Game1.graphics.GraphicsDevice.Viewport;
      double width = (double) viewport.Width;
      viewport = Game1.graphics.GraphicsDevice.Viewport;
      double num = (double) (viewport.Height / 2);
      this.robotPosition = new Vector2((float) width, (float) num);
      this.tempSprites = new List<TemporaryAnimatedSprite>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
