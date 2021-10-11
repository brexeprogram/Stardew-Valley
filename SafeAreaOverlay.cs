// Decompiled with JetBrains decompiler
// Type: StardewValley.SafeAreaOverlay
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValley
{
  public class SafeAreaOverlay : DrawableGameComponent
  {
    private SpriteBatch spriteBatch;
    private Texture2D dummyTexture;

    public SafeAreaOverlay(Game game)
      : base(game)
    {
      this.DrawOrder = 1000;
    }

    protected override void LoadContent()
    {
      this.spriteBatch = new SpriteBatch(Game1.graphics.GraphicsDevice);
      this.dummyTexture = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1);
      this.dummyTexture.SetData<Color>(new Color[1]
      {
        Color.White
      });
    }

    public override void Draw(GameTime gameTime)
    {
      Viewport viewport = Game1.graphics.GraphicsDevice.Viewport;
      Rectangle titleSafeArea = viewport.TitleSafeArea;
      int num1 = viewport.X + viewport.Width;
      int num2 = viewport.Y + viewport.Height;
      Rectangle destinationRectangle1 = new Rectangle(viewport.X, viewport.Y, titleSafeArea.X - viewport.X, viewport.Height);
      Rectangle destinationRectangle2 = new Rectangle(titleSafeArea.Right, viewport.Y, num1 - titleSafeArea.Right, viewport.Height);
      Rectangle destinationRectangle3 = new Rectangle(titleSafeArea.Left, viewport.Y, titleSafeArea.Width, titleSafeArea.Top - viewport.Y);
      Rectangle destinationRectangle4 = new Rectangle(titleSafeArea.Left, titleSafeArea.Bottom, titleSafeArea.Width, num2 - titleSafeArea.Bottom);
      Color red = Color.Red;
      this.spriteBatch.Begin();
      this.spriteBatch.Draw(this.dummyTexture, destinationRectangle1, red);
      this.spriteBatch.Draw(this.dummyTexture, destinationRectangle2, red);
      this.spriteBatch.Draw(this.dummyTexture, destinationRectangle3, red);
      this.spriteBatch.Draw(this.dummyTexture, destinationRectangle4, red);
      this.spriteBatch.End();
    }
  }
}
