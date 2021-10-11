// Decompiled with JetBrains decompiler
// Type: StardewValley.Background
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley
{
  public class Background
  {
    private Vector2 position = Vector2.Zero;
    public int defaultChunkIndex;
    public int numChunksInSheet;
    public double chanceForDeviationFromDefault;
    private Texture2D backgroundImage;
    private int chunksWide;
    private int chunksHigh;
    private int chunkWidth;
    private int chunkHeight;
    private int[] chunks;
    private float zoom;
    public Color c;
    private bool summitBG;
    public int yOffset;

    public Background()
    {
      this.summitBG = true;
      this.c = Color.White;
    }

    public Background(Texture2D bgImage, int seedValue, int chunksWide, int chunksHigh, int chunkWidth, int chunkHeight, float zoom, int defaultChunkIndex, int numChunksInSheet, double chanceForDeviation, Color c)
    {
      this.backgroundImage = bgImage;
      this.chunksWide = chunksWide;
      this.chunksHigh = chunksHigh;
      this.zoom = zoom;
      this.chunkWidth = chunkWidth;
      this.chunkHeight = chunkHeight;
      this.defaultChunkIndex = defaultChunkIndex;
      this.numChunksInSheet = numChunksInSheet;
      this.chanceForDeviationFromDefault = chanceForDeviation;
      this.c = c;
      Random random = new Random(seedValue);
      this.chunks = new int[chunksWide * chunksHigh];
      for (int index = 0; index < chunksHigh * chunksWide; ++index)
        this.chunks[index] = random.NextDouble() >= this.chanceForDeviationFromDefault ? defaultChunkIndex : random.Next(numChunksInSheet);
    }

    public void update(xTile.Dimensions.Rectangle viewport)
    {
      this.position.X = (float) -((double) (viewport.X + viewport.Width / 2) / ((double) Game1.currentLocation.map.GetLayer("Back").LayerWidth * (double) Game1.tileSize) * ((double) (this.chunksWide * this.chunkWidth) * (double) this.zoom - (double) viewport.Width));
      this.position.Y = (float) -((double) (viewport.Y + viewport.Height / 2) / ((double) Game1.currentLocation.map.GetLayer("Back").LayerHeight * (double) Game1.tileSize) * ((double) (this.chunksHigh * this.chunkHeight) * (double) this.zoom - (double) viewport.Height));
    }

    public void draw(SpriteBatch b)
    {
      if (this.summitBG)
      {
        int num1 = 0;
        string currentSeason = Game1.currentSeason;
        if (!(currentSeason == "summer"))
        {
          if (!(currentSeason == "fall"))
          {
            if (currentSeason == "winter")
              num1 = 2;
          }
          else
            num1 = 1;
        }
        else
          num1 = 0;
        float num2 = 1f;
        Color color = Color.White;
        if (Game1.timeOfDay >= 1800)
        {
          int num3 = (int) ((double) (Game1.timeOfDay - Game1.timeOfDay % 100) + (double) (Game1.timeOfDay % 100 / 10) * 16.6599998474121);
          this.c = new Color((float) byte.MaxValue, (float) byte.MaxValue - Math.Max(100f, (float) ((double) num3 + (double) Game1.gameTimeInterval / 7000.0 * 16.6000003814697 - 1800.0)), (float) byte.MaxValue - Math.Max(100f, (float) (((double) num3 + (double) Game1.gameTimeInterval / 7000.0 * 16.6000003814697 - 1800.0) / 2.0)));
          color = Color.Blue * 0.5f;
          num2 = Math.Max(0.0f, Math.Min(1f, (float) ((2000.0 - ((double) num3 + (double) Game1.gameTimeInterval / 7000.0 * 16.6000003814697)) / 200.0)));
        }
        b.Draw(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height * 3 / 4), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(639 + (num1 + 1), 1051, 1, 400)), this.c * num2, 0.0f, Vector2.Zero, SpriteEffects.None, 1E-07f);
        b.Draw(Game1.mouseCursors, new Vector2(0.0f, (float) (Game1.viewport.Height - 596)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 736 + num1 * 149, 639, 149)), Color.White * Math.Max((float) this.c.A, 0.5f), 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 1E-06f);
        b.Draw(Game1.mouseCursors, new Vector2(0.0f, (float) (Game1.viewport.Height - 596)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 736 + num1 * 149, 639, 149)), color * (0.75f - num2), 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 1E-06f);
      }
      else
      {
        Vector2 zero = Vector2.Zero;
        Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle(0, 0, this.chunkWidth, this.chunkHeight);
        for (int index = 0; index < this.chunks.Length; ++index)
        {
          zero.X = this.position.X + (float) (index * this.chunkWidth % (this.chunksWide * this.chunkWidth)) * this.zoom;
          zero.Y = this.position.Y + (float) (index * this.chunkWidth / (this.chunksWide * this.chunkWidth) * this.chunkHeight) * this.zoom;
          if (this.backgroundImage == null)
          {
            b.Draw(Game1.staminaRect, new Microsoft.Xna.Framework.Rectangle((int) zero.X, (int) zero.Y, Game1.viewport.Width, Game1.viewport.Height), new Microsoft.Xna.Framework.Rectangle?(rectangle), this.c, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
          }
          else
          {
            rectangle.X = this.chunks[index] * this.chunkWidth % this.backgroundImage.Width;
            rectangle.Y = this.chunks[index] * this.chunkWidth / this.backgroundImage.Width * this.chunkHeight;
            b.Draw(this.backgroundImage, zero, new Microsoft.Xna.Framework.Rectangle?(rectangle), this.c, 0.0f, Vector2.Zero, this.zoom, SpriteEffects.None, 0.0f);
          }
        }
      }
    }
  }
}
