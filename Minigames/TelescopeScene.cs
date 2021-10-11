// Decompiled with JetBrains decompiler
// Type: StardewValley.Minigames.TelescopeScene
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using xTile;
using xTile.Dimensions;
using xTile.Layers;

namespace StardewValley.Minigames
{
  public class TelescopeScene : IMinigame
  {
    public LocalizedContentManager temporaryContent;
    public Texture2D background;
    public Texture2D trees;
    public float yOffset;
    public GameLocation walkSpace;

    public TelescopeScene(NPC Maru)
    {
      this.temporaryContent = Game1.content.CreateTemporary();
      this.background = this.temporaryContent.Load<Texture2D>("LooseSprites\\nightSceneMaru");
      this.trees = this.temporaryContent.Load<Texture2D>("LooseSprites\\nightSceneMaruTrees");
      Map map = new Map();
      map.AddLayer(new Layer("Back", map, new Size(30, 1), new Size(Game1.tileSize)));
      this.walkSpace = new GameLocation(map, nameof (walkSpace));
      Game1.currentLocation = this.walkSpace;
    }

    public bool overrideFreeMouseMovement()
    {
      return false;
    }

    public bool tick(GameTime time)
    {
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
      b.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
      SpriteBatch spriteBatch1 = b;
      Texture2D background = this.background;
      Viewport viewport1 = Game1.graphics.GraphicsDevice.Viewport;
      double num1 = (double) (viewport1.Width / 2 - this.background.Bounds.Width / 2 * Game1.pixelZoom);
      int num2 = -(this.background.Bounds.Height * Game1.pixelZoom);
      viewport1 = Game1.graphics.GraphicsDevice.Viewport;
      int height1 = viewport1.Height;
      double num3 = (double) (num2 + height1);
      Vector2 position1 = new Vector2((float) num1, (float) num3);
      Microsoft.Xna.Framework.Rectangle? sourceRectangle1 = new Microsoft.Xna.Framework.Rectangle?(this.background.Bounds);
      Color white1 = Color.White;
      double num4 = 0.0;
      Vector2 zero1 = Vector2.Zero;
      double pixelZoom1 = (double) Game1.pixelZoom;
      int num5 = 0;
      double num6 = 1.0 / 1000.0;
      spriteBatch1.Draw(background, position1, sourceRectangle1, white1, (float) num4, zero1, (float) pixelZoom1, (SpriteEffects) num5, (float) num6);
      SpriteBatch spriteBatch2 = b;
      Texture2D trees = this.trees;
      Viewport viewport2 = Game1.graphics.GraphicsDevice.Viewport;
      double num7 = (double) (viewport2.Width / 2 - this.trees.Bounds.Width / 2 * Game1.pixelZoom);
      int num8 = -(this.trees.Bounds.Height * Game1.pixelZoom);
      viewport2 = Game1.graphics.GraphicsDevice.Viewport;
      int height2 = viewport2.Height;
      double num9 = (double) (num8 + height2);
      Vector2 position2 = new Vector2((float) num7, (float) num9);
      Microsoft.Xna.Framework.Rectangle? sourceRectangle2 = new Microsoft.Xna.Framework.Rectangle?(this.trees.Bounds);
      Color white2 = Color.White;
      double num10 = 0.0;
      Vector2 zero2 = Vector2.Zero;
      double pixelZoom2 = (double) Game1.pixelZoom;
      int num11 = 0;
      double num12 = 1.0;
      spriteBatch2.Draw(trees, position2, sourceRectangle2, white2, (float) num10, zero2, (float) pixelZoom2, (SpriteEffects) num11, (float) num12);
      b.End();
    }

    public void changeScreenSize()
    {
    }

    public void unload()
    {
      this.temporaryContent.Unload();
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
