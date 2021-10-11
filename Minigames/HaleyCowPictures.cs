// Decompiled with JetBrains decompiler
// Type: StardewValley.Minigames.HaleyCowPictures
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace StardewValley.Minigames
{
  public class HaleyCowPictures : IMinigame
  {
    private int betweenPhotoTimer = 1000;
    private const int pictureWidth = 416;
    private const int pictureHeight = 496;
    private const int sourceWidth = 104;
    private const int sourceHeight = 124;
    private int numberOfPhotosSoFar;
    private LocalizedContentManager content;
    private Vector2 centerOfScreen;
    private Texture2D pictures;
    private float fadeAlpha;

    public HaleyCowPictures()
    {
      this.content = Game1.content.CreateTemporary();
      this.pictures = Game1.currentSeason.Equals("winter") ? this.content.Load<Texture2D>("LooseSprites\\cowPhotosWinter") : this.content.Load<Texture2D>("LooseSprites\\cowPhotos");
      this.centerOfScreen = new Vector2((float) (Game1.graphics.GraphicsDevice.Viewport.Width / 2), (float) (Game1.graphics.GraphicsDevice.Viewport.Height / 2));
    }

    public bool overrideFreeMouseMovement()
    {
      return false;
    }

    public bool tick(GameTime time)
    {
      this.betweenPhotoTimer = this.betweenPhotoTimer - time.ElapsedGameTime.Milliseconds;
      if (this.betweenPhotoTimer <= 0)
      {
        this.betweenPhotoTimer = 5000;
        this.numberOfPhotosSoFar = this.numberOfPhotosSoFar + 1;
        if (this.numberOfPhotosSoFar < 5)
          Game1.playSound("cameraNoise");
        if (this.numberOfPhotosSoFar >= 6)
        {
          ++Game1.currentLocation.currentEvent.CurrentCommand;
          return true;
        }
      }
      if (this.numberOfPhotosSoFar >= 5)
        this.fadeAlpha = Math.Min(1f, this.fadeAlpha = this.fadeAlpha + 0.007f);
      return false;
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
    }

    public void receiveKeyRelease(Keys k)
    {
    }

    public void draw(SpriteBatch b)
    {
      b.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointWrap, (DepthStencilState) null, (RasterizerState) null);
      Rectangle bounds;
      if (this.numberOfPhotosSoFar > 0)
      {
        b.Draw(this.pictures, this.centerOfScreen + new Vector2(-208f, -248f), new Rectangle?(new Rectangle(0, 0, 104, 124)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.0f);
        Game1.player.faceDirection(2);
        Game1.player.FarmerRenderer.draw(b, Game1.player, 0, this.centerOfScreen + new Vector2(-208f, -248f) + new Vector2(70f, 66f) * (float) Game1.pixelZoom, 0.01f, false);
        SpriteBatch spriteBatch = b;
        Texture2D shadowTexture = Game1.shadowTexture;
        Vector2 position = this.centerOfScreen + new Vector2(-208f, -248f) + new Vector2(70f, 66f) * (float) Game1.pixelZoom + new Vector2(32f, (float) (24 + Game1.tileSize * 3 / 2));
        Rectangle? sourceRectangle = new Rectangle?(Game1.shadowTexture.Bounds);
        Color white = Color.White;
        double num1 = 0.0;
        double x = (double) Game1.shadowTexture.Bounds.Center.X;
        bounds = Game1.shadowTexture.Bounds;
        double y = (double) bounds.Center.Y;
        Vector2 origin = new Vector2((float) x, (float) y);
        double num2 = 4.0;
        int num3 = 0;
        double num4 = 0.00499999988824129;
        spriteBatch.Draw(shadowTexture, position, sourceRectangle, white, (float) num1, origin, (float) num2, (SpriteEffects) num3, (float) num4);
      }
      if (this.numberOfPhotosSoFar > 1)
      {
        Game1.player.faceDirection(3);
        b.Draw(this.pictures, this.centerOfScreen + new Vector2(-208f, -248f) + new Vector2((float) (Game1.pixelZoom * 4), (float) (Game1.pixelZoom * 4)), new Rectangle?(new Rectangle(104, 0, 104, 124)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.1f);
        Game1.player.FarmerRenderer.draw(b, Game1.player, 6, this.centerOfScreen + new Vector2(-208f, -248f) + new Vector2((float) (Game1.pixelZoom * 4), (float) (Game1.pixelZoom * 4)) + new Vector2(64f, 66f) * (float) Game1.pixelZoom, 0.11f, true);
        SpriteBatch spriteBatch = b;
        Texture2D shadowTexture = Game1.shadowTexture;
        Vector2 position = this.centerOfScreen + new Vector2(-208f, -248f) + new Vector2((float) (Game1.pixelZoom * 4), (float) (Game1.pixelZoom * 4)) + new Vector2(64f, 66f) * (float) Game1.pixelZoom + new Vector2(32f, (float) (24 + Game1.tileSize * 3 / 2));
        Rectangle? sourceRectangle = new Rectangle?(Game1.shadowTexture.Bounds);
        Color white = Color.White;
        double num1 = 0.0;
        bounds = Game1.shadowTexture.Bounds;
        double x = (double) bounds.Center.X;
        bounds = Game1.shadowTexture.Bounds;
        double y = (double) bounds.Center.Y;
        Vector2 origin = new Vector2((float) x, (float) y);
        double num2 = 4.0;
        int num3 = 0;
        double num4 = 0.104999996721745;
        spriteBatch.Draw(shadowTexture, position, sourceRectangle, white, (float) num1, origin, (float) num2, (SpriteEffects) num3, (float) num4);
      }
      if (this.numberOfPhotosSoFar > 2)
      {
        Game1.player.faceDirection(3);
        b.Draw(this.pictures, this.centerOfScreen + new Vector2(-208f, -248f) - new Vector2((float) (Game1.pixelZoom * 6), (float) (Game1.pixelZoom * 2)), new Rectangle?(new Rectangle(0, 124, 104, 124)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.2f);
        Game1.player.FarmerRenderer.draw(b, Game1.player, 89, this.centerOfScreen + new Vector2(-208f, -248f) - new Vector2((float) (Game1.pixelZoom * 6), (float) (Game1.pixelZoom * 2)) + new Vector2(55f, 66f) * (float) Game1.pixelZoom, 0.21f, true);
        SpriteBatch spriteBatch = b;
        Texture2D shadowTexture = Game1.shadowTexture;
        Vector2 position = this.centerOfScreen + new Vector2(-208f, -248f) - new Vector2((float) (Game1.pixelZoom * 6), (float) (Game1.pixelZoom * 2)) + new Vector2(55f, 66f) * (float) Game1.pixelZoom + new Vector2(32f, (float) (24 + Game1.tileSize * 3 / 2));
        Rectangle? sourceRectangle = new Rectangle?(Game1.shadowTexture.Bounds);
        Color white = Color.White;
        double num1 = 0.0;
        bounds = Game1.shadowTexture.Bounds;
        double x = (double) bounds.Center.X;
        bounds = Game1.shadowTexture.Bounds;
        double y = (double) bounds.Center.Y;
        Vector2 origin = new Vector2((float) x, (float) y);
        double num2 = 4.0;
        int num3 = 0;
        double num4 = 0.204999998211861;
        spriteBatch.Draw(shadowTexture, position, sourceRectangle, white, (float) num1, origin, (float) num2, (SpriteEffects) num3, (float) num4);
      }
      if (this.numberOfPhotosSoFar > 3)
      {
        Game1.player.faceDirection(2);
        b.Draw(this.pictures, this.centerOfScreen + new Vector2(-208f, -248f), new Rectangle?(new Rectangle(104, 124, 104, 124)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.3f);
        Game1.player.FarmerRenderer.draw(b, Game1.player, 94, this.centerOfScreen + new Vector2(-208f, -248f) + new Vector2(70f, 66f) * (float) Game1.pixelZoom, 0.31f, false);
        SpriteBatch spriteBatch = b;
        Texture2D shadowTexture = Game1.shadowTexture;
        Vector2 position = this.centerOfScreen + new Vector2(-208f, -248f) + new Vector2(70f, 66f) * (float) Game1.pixelZoom + new Vector2(32f, (float) (24 + Game1.tileSize * 3 / 2));
        Rectangle? sourceRectangle = new Rectangle?(Game1.shadowTexture.Bounds);
        Color white = Color.White;
        double num1 = 0.0;
        bounds = Game1.shadowTexture.Bounds;
        double x = (double) bounds.Center.X;
        bounds = Game1.shadowTexture.Bounds;
        double y = (double) bounds.Center.Y;
        Vector2 origin = new Vector2((float) x, (float) y);
        double num2 = 4.0;
        int num3 = 0;
        double num4 = 0.305000007152557;
        spriteBatch.Draw(shadowTexture, position, sourceRectangle, white, (float) num1, origin, (float) num2, (SpriteEffects) num3, (float) num4);
      }
      b.Draw(Game1.staminaRect, new Rectangle(0, 0, Game1.graphics.GraphicsDevice.Viewport.Width, Game1.graphics.GraphicsDevice.Viewport.Height), new Rectangle?(Game1.staminaRect.Bounds), Color.Black * this.fadeAlpha, 0.0f, Vector2.Zero, SpriteEffects.None, 1f);
      b.End();
    }

    public void changeScreenSize()
    {
      Viewport viewport = Game1.graphics.GraphicsDevice.Viewport;
      double num1 = (double) (viewport.Width / 2);
      viewport = Game1.graphics.GraphicsDevice.Viewport;
      double num2 = (double) (viewport.Height / 2);
      this.centerOfScreen = new Vector2((float) num1, (float) num2);
    }

    public void unload()
    {
      this.content.Unload();
    }

    public void receiveEventPoke(int data)
    {
      throw new NotImplementedException();
    }

    public string minigameId()
    {
      return (string) null;
    }
  }
}
