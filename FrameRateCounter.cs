// Decompiled with JetBrains decompiler
// Type: FrameRateCounter
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System;

public class FrameRateCounter : DrawableGameComponent
{
  private TimeSpan elapsedTime = TimeSpan.Zero;
  private LocalizedContentManager content;
  private SpriteBatch spriteBatch;
  private int frameRate;
  private int frameCounter;

  public FrameRateCounter(Game game)
    : base(game)
  {
    this.content = new LocalizedContentManager((IServiceProvider) game.Services, this.Game.Content.RootDirectory);
  }

  protected override void LoadContent()
  {
    this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
  }

  protected override void UnloadContent()
  {
    this.content.Unload();
  }

  public override void Update(GameTime gameTime)
  {
    this.elapsedTime = this.elapsedTime + gameTime.ElapsedGameTime;
    if (!(this.elapsedTime > TimeSpan.FromSeconds(1.0)))
      return;
    this.elapsedTime = this.elapsedTime - TimeSpan.FromSeconds(1.0);
    this.frameRate = this.frameCounter;
    this.frameCounter = 0;
  }

  public override void Draw(GameTime gameTime)
  {
    this.frameCounter = this.frameCounter + 1;
    string text = string.Format("fps: {0}", (object) this.frameRate);
    this.spriteBatch.Begin();
    this.spriteBatch.DrawString(Game1.dialogueFont, text, new Vector2(33f, 33f), Color.Black);
    this.spriteBatch.DrawString(Game1.dialogueFont, text, new Vector2(32f, 32f), Color.White);
    this.spriteBatch.End();
  }
}
