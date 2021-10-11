// Decompiled with JetBrains decompiler
// Type: StardewValley.BellsAndWhistles.ScreenSwipe
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace StardewValley.BellsAndWhistles
{
  public class ScreenSwipe
  {
    private List<Vector2> flairPositions = new List<Vector2>();
    public const int swipe_bundleComplete = 0;
    public const int borderPixelWidth = 7;
    private Rectangle bgSource;
    private Rectangle flairSource;
    private Rectangle messageSource;
    private Rectangle movingFlairSource;
    private Rectangle bgDest;
    private int yPosition;
    private int durationAfterSwipe;
    private int originalBGSourceXLimit;
    private Vector2 messagePosition;
    private Vector2 movingFlairPosition;
    private Vector2 movingFlairMotion;
    private float swipeVelocity;

    public ScreenSwipe(int which, float swipeVelocity = -1f, int durationAfterSwipe = -1)
    {
      Game1.playSound("throw");
      if ((double) swipeVelocity == -1.0)
        swipeVelocity = 5f;
      if (durationAfterSwipe == -1)
        durationAfterSwipe = 2700;
      this.swipeVelocity = swipeVelocity;
      this.durationAfterSwipe = durationAfterSwipe;
      Vector2 vector2;
      // ISSUE: explicit reference operation
      // ISSUE: variable of a reference type
      Vector2& local = @vector2;
      Viewport viewport = Game1.graphics.GraphicsDevice.Viewport;
      double num1 = (double) (viewport.Width / 2);
      viewport = Game1.graphics.GraphicsDevice.Viewport;
      double num2 = (double) (viewport.Height / 2);
      // ISSUE: explicit reference operation
      ^local = new Vector2((float) num1, (float) num2);
      if (which == 0)
        this.messageSource = new Rectangle(128, 1367, 110, 14);
      if (which == 0)
      {
        this.bgSource = new Rectangle(128, 1296, 1, 71);
        this.flairSource = new Rectangle(144, 1303, 144, 58);
        this.movingFlairSource = new Rectangle(643, 768, 8, 13);
        this.originalBGSourceXLimit = this.bgSource.X + this.bgSource.Width;
        this.yPosition = (int) vector2.Y - this.bgSource.Height * Game1.pixelZoom / 2;
        this.messagePosition = new Vector2(vector2.X - (float) (this.messageSource.Width * Game1.pixelZoom / 2), vector2.Y - (float) (this.messageSource.Height * Game1.pixelZoom / 2));
        this.flairPositions.Add(new Vector2(this.messagePosition.X - (float) (this.flairSource.Width * Game1.pixelZoom) - (float) Game1.tileSize, (float) (this.yPosition + 7 * Game1.pixelZoom)));
        this.flairPositions.Add(new Vector2(this.messagePosition.X + (float) (this.messageSource.Width * Game1.pixelZoom) + (float) Game1.tileSize, (float) (this.yPosition + 7 * Game1.pixelZoom)));
        this.movingFlairPosition = new Vector2(this.messagePosition.X + (float) (this.messageSource.Width * Game1.pixelZoom) + (float) (Game1.tileSize * 3), vector2.Y + (float) (Game1.tileSize / 2));
        this.movingFlairMotion = new Vector2(0.0f, -0.5f);
      }
      this.bgDest = new Rectangle(0, this.yPosition, this.bgSource.Width * Game1.pixelZoom, this.bgSource.Height * Game1.pixelZoom);
    }

    public bool update(GameTime time)
    {
      TimeSpan elapsedGameTime;
      if (this.durationAfterSwipe > 0 && this.bgDest.Width <= Game1.viewport.Width)
      {
        // ISSUE: explicit reference operation
        // ISSUE: variable of a reference type
        int& local = @this.bgDest.Width;
        // ISSUE: explicit reference operation
        int num1 = ^local;
        double swipeVelocity = (double) this.swipeVelocity;
        elapsedGameTime = time.ElapsedGameTime;
        double totalMilliseconds = elapsedGameTime.TotalMilliseconds;
        int num2 = (int) (swipeVelocity * totalMilliseconds);
        int num3 = num1 + num2;
        // ISSUE: explicit reference operation
        ^local = num3;
        if (this.bgDest.Width > Game1.viewport.Width)
          Game1.playSound("newRecord");
      }
      else if (this.durationAfterSwipe <= 0)
      {
        // ISSUE: explicit reference operation
        // ISSUE: variable of a reference type
        int& local = @this.bgDest.X;
        // ISSUE: explicit reference operation
        int num1 = ^local;
        double swipeVelocity = (double) this.swipeVelocity;
        elapsedGameTime = time.ElapsedGameTime;
        double totalMilliseconds = elapsedGameTime.TotalMilliseconds;
        int num2 = (int) (swipeVelocity * totalMilliseconds);
        int num3 = num1 + num2;
        // ISSUE: explicit reference operation
        ^local = num3;
        for (int index = 0; index < this.flairPositions.Count; ++index)
        {
          if ((double) this.bgDest.X > (double) this.flairPositions[index].X)
            this.flairPositions[index] = new Vector2((float) this.bgDest.X, this.flairPositions[index].Y);
        }
        if ((double) this.bgDest.X > (double) this.messagePosition.X)
          this.messagePosition = new Vector2((float) this.bgDest.X, this.messagePosition.Y);
        if ((double) this.bgDest.X > (double) this.movingFlairPosition.X)
          this.movingFlairPosition = new Vector2((float) this.bgDest.X, this.movingFlairPosition.Y);
      }
      if (this.bgDest.Width > Game1.viewport.Width && this.durationAfterSwipe > 0)
      {
        if (Game1.oldMouseState.LeftButton == ButtonState.Pressed)
          this.durationAfterSwipe = 0;
        int durationAfterSwipe = this.durationAfterSwipe;
        elapsedGameTime = time.ElapsedGameTime;
        int totalMilliseconds = (int) elapsedGameTime.TotalMilliseconds;
        this.durationAfterSwipe = durationAfterSwipe - totalMilliseconds;
        if (this.durationAfterSwipe <= 0)
          Game1.playSound("tinyWhip");
      }
      this.movingFlairPosition = this.movingFlairPosition + this.movingFlairMotion;
      return this.bgDest.X > Game1.viewport.Width;
    }

    public Rectangle getAdjustedSourceRect(Rectangle sourceRect, float xStartPosition)
    {
      if ((double) xStartPosition > (double) this.bgDest.Width || (double) xStartPosition + (double) (sourceRect.Width * Game1.pixelZoom) < (double) this.bgDest.X)
        return Rectangle.Empty;
      int x = (int) Math.Max((float) sourceRect.X, (float) sourceRect.X + ((float) this.bgDest.X - xStartPosition) / (float) Game1.pixelZoom);
      return new Rectangle(x, sourceRect.Y, (int) Math.Min((float) (sourceRect.Width - (x - sourceRect.X) / Game1.pixelZoom), ((float) this.bgDest.Width - xStartPosition) / (float) Game1.pixelZoom), sourceRect.Height);
    }

    public void draw(SpriteBatch b)
    {
      b.Draw(Game1.mouseCursors, this.bgDest, new Rectangle?(this.bgSource), Color.White);
      foreach (Vector2 flairPosition in this.flairPositions)
      {
        Rectangle adjustedSourceRect = this.getAdjustedSourceRect(this.flairSource, flairPosition.X);
        if (adjustedSourceRect.Right >= this.originalBGSourceXLimit)
          adjustedSourceRect.Width = this.originalBGSourceXLimit - adjustedSourceRect.X;
        b.Draw(Game1.mouseCursors, flairPosition, new Rectangle?(adjustedSourceRect), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1f);
      }
      b.Draw(Game1.mouseCursors, this.movingFlairPosition, new Rectangle?(this.getAdjustedSourceRect(this.movingFlairSource, this.movingFlairPosition.X)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1f);
      b.Draw(Game1.mouseCursors, this.messagePosition, new Rectangle?(this.getAdjustedSourceRect(this.messageSource, this.messagePosition.X)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1f);
    }
  }
}
