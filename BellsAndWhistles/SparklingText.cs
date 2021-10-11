// Decompiled with JetBrains decompiler
// Type: StardewValley.BellsAndWhistles.SparklingText
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StardewValley.BellsAndWhistles
{
  public class SparklingText
  {
    public static int maxDistanceForSparkle = Game1.tileSize / 2;
    public float offsetDecay = 1f;
    public float alpha = 1f;
    private SpriteFont font;
    private Color color;
    private Color sparkleColor;
    private bool rainbow;
    private int millisecondsDuration;
    private int amplitude;
    private int period;
    private int colorCycle;
    public string text;
    private float[] individualCharacterOffsets;
    public float textWidth;
    private double sparkleFrequency;
    private List<TemporaryAnimatedSprite> sparkles;
    private List<Vector2> sparkleTrash;
    private Rectangle boundingBox;

    public SparklingText(SpriteFont font, string text, Color color, Color sparkleColor, bool rainbow = false, double sparkleFrequency = 0.1, int millisecondsDuration = 2500, int amplitude = -1, int speed = 500)
    {
      if (amplitude == -1)
        amplitude = Game1.tileSize;
      SparklingText.maxDistanceForSparkle = Game1.tileSize / 2;
      this.font = font;
      this.color = color;
      this.sparkleColor = sparkleColor;
      this.text = text;
      this.rainbow = rainbow;
      if (rainbow)
        color = Color.Yellow;
      this.sparkleFrequency = sparkleFrequency;
      this.millisecondsDuration = millisecondsDuration;
      this.individualCharacterOffsets = new float[text.Length];
      this.amplitude = amplitude;
      this.period = speed;
      this.sparkles = new List<TemporaryAnimatedSprite>();
      this.boundingBox = new Rectangle(-SparklingText.maxDistanceForSparkle, -SparklingText.maxDistanceForSparkle, (int) font.MeasureString(text).X + SparklingText.maxDistanceForSparkle * 2, (int) font.MeasureString(text).Y + SparklingText.maxDistanceForSparkle * 2);
      this.sparkleTrash = new List<Vector2>();
      this.textWidth = font.MeasureString(text).X;
    }

    public bool update(GameTime time)
    {
      this.millisecondsDuration = this.millisecondsDuration - time.ElapsedGameTime.Milliseconds;
      this.offsetDecay = this.offsetDecay - 1f / 1000f;
      this.amplitude = (int) ((double) this.amplitude * (double) this.offsetDecay);
      if (this.millisecondsDuration <= 500)
        this.alpha = (float) this.millisecondsDuration / 500f;
      for (int index = 0; index < this.individualCharacterOffsets.Length; ++index)
        this.individualCharacterOffsets[index] = (float) (this.amplitude / 2) * (float) Math.Sin(2.0 * Math.PI / (double) this.period * (double) (this.millisecondsDuration - index * 100));
      if (this.millisecondsDuration > 500 && Game1.random.NextDouble() < this.sparkleFrequency)
      {
        int num = Game1.random.Next(100, 600);
        this.sparkles.Add(new TemporaryAnimatedSprite(Game1.animations, new Rectangle(0, 704, 64, 64), (float) (num / 6), 6, 0, new Vector2((float) Game1.random.Next(this.boundingBox.X, this.boundingBox.Right), (float) Game1.random.Next(this.boundingBox.Y, this.boundingBox.Bottom)), false, false, 1f, 0.0f, this.rainbow ? this.color : this.sparkleColor, 1f, 0.0f, 0.0f, 0.0f, false));
      }
      for (int index = this.sparkles.Count - 1; index >= 0; --index)
      {
        if (this.sparkles[index].update(time))
          this.sparkles.RemoveAt(index);
      }
      if (this.rainbow)
        this.incrementRainbowColors();
      return this.millisecondsDuration <= 0;
    }

    private void incrementRainbowColors()
    {
      if (this.colorCycle != 0)
        return;
      if ((int) (this.color.G += (byte) 4) >= (int) byte.MaxValue)
      {
        this.colorCycle = 1;
      }
      else
      {
        if (this.colorCycle != 1)
          return;
        if ((int) (this.color.R -= (byte) 4) <= 0)
        {
          this.colorCycle = 2;
        }
        else
        {
          if (this.colorCycle != 2)
            return;
          if ((int) (this.color.B += (byte) 4) >= (int) byte.MaxValue)
          {
            this.colorCycle = 3;
          }
          else
          {
            if (this.colorCycle != 3)
              return;
            if ((int) (this.color.G -= (byte) 4) <= 0)
            {
              this.colorCycle = 4;
            }
            else
            {
              if (this.colorCycle != 4)
                return;
              if ((int) ++this.color.R >= (int) byte.MaxValue)
              {
                this.colorCycle = 5;
              }
              else
              {
                if (this.colorCycle != 5 || (int) (this.color.B -= (byte) 4) > 0)
                  return;
                this.colorCycle = 0;
              }
            }
          }
        }
      }
    }

    private static Color getRainbowColorFromIndex(int index)
    {
      switch (index % 8)
      {
        case 0:
          return Color.Red;
        case 1:
          return Color.Orange;
        case 2:
          return Color.Yellow;
        case 3:
          return Color.Chartreuse;
        case 4:
          return Color.Green;
        case 5:
          return Color.Cyan;
        case 6:
          return Color.Blue;
        case 7:
          return Color.Violet;
        default:
          return Color.White;
      }
    }

    public void draw(SpriteBatch b, Vector2 onScreenPosition)
    {
      int num1 = 0;
      for (int index = 0; index < this.text.Length; ++index)
      {
        SpriteBatch spriteBatch1 = b;
        SpriteFont font1 = this.font;
        char ch = this.text[index];
        string text1 = ch.ToString() ?? "";
        Vector2 position1 = onScreenPosition + new Vector2((float) (num1 - 2), this.individualCharacterOffsets[index]);
        Color black1 = Color.Black;
        double num2 = 0.0;
        Vector2 zero1 = Vector2.Zero;
        double num3 = 1.0;
        int num4 = 0;
        double num5 = 0.990000009536743;
        spriteBatch1.DrawString(font1, text1, position1, black1, (float) num2, zero1, (float) num3, (SpriteEffects) num4, (float) num5);
        SpriteBatch spriteBatch2 = b;
        SpriteFont font2 = this.font;
        ch = this.text[index];
        string text2 = ch.ToString() ?? "";
        Vector2 position2 = onScreenPosition + new Vector2((float) (num1 + 2), this.individualCharacterOffsets[index]);
        Color black2 = Color.Black;
        double num6 = 0.0;
        Vector2 zero2 = Vector2.Zero;
        double num7 = 1.0;
        int num8 = 0;
        double num9 = 0.99099999666214;
        spriteBatch2.DrawString(font2, text2, position2, black2, (float) num6, zero2, (float) num7, (SpriteEffects) num8, (float) num9);
        SpriteBatch spriteBatch3 = b;
        SpriteFont font3 = this.font;
        ch = this.text[index];
        string text3 = ch.ToString() ?? "";
        Vector2 position3 = onScreenPosition + new Vector2((float) num1, this.individualCharacterOffsets[index] - 2f);
        Color black3 = Color.Black;
        double num10 = 0.0;
        Vector2 zero3 = Vector2.Zero;
        double num11 = 1.0;
        int num12 = 0;
        double num13 = 0.991999983787537;
        spriteBatch3.DrawString(font3, text3, position3, black3, (float) num10, zero3, (float) num11, (SpriteEffects) num12, (float) num13);
        SpriteBatch spriteBatch4 = b;
        SpriteFont font4 = this.font;
        ch = this.text[index];
        string text4 = ch.ToString() ?? "";
        Vector2 position4 = onScreenPosition + new Vector2((float) num1, this.individualCharacterOffsets[index] + 2f);
        Color black4 = Color.Black;
        double num14 = 0.0;
        Vector2 zero4 = Vector2.Zero;
        double num15 = 1.0;
        int num16 = 0;
        double num17 = 0.992999970912933;
        spriteBatch4.DrawString(font4, text4, position4, black4, (float) num14, zero4, (float) num15, (SpriteEffects) num16, (float) num17);
        SpriteBatch spriteBatch5 = b;
        SpriteFont font5 = this.font;
        ch = this.text[index];
        string text5 = ch.ToString() ?? "";
        Vector2 position5 = onScreenPosition + new Vector2((float) num1, this.individualCharacterOffsets[index]);
        Color color = this.rainbow ? SparklingText.getRainbowColorFromIndex(index) : this.color * this.alpha;
        double num18 = 0.0;
        Vector2 zero5 = Vector2.Zero;
        double num19 = 1.0;
        int num20 = 0;
        double num21 = 1.0;
        spriteBatch5.DrawString(font5, text5, position5, color, (float) num18, zero5, (float) num19, (SpriteEffects) num20, (float) num21);
        int num22 = num1;
        SpriteFont font6 = this.font;
        ch = this.text[index];
        string text6 = ch.ToString() ?? "";
        int x = (int) font6.MeasureString(text6).X;
        num1 = num22 + x;
      }
      foreach (TemporaryAnimatedSprite sparkle in this.sparkles)
      {
        Vector2 vector2_1 = sparkle.Position + onScreenPosition;
        sparkle.Position = vector2_1;
        SpriteBatch spriteBatch = b;
        int num2 = 1;
        int xOffset = 0;
        int yOffset = 0;
        sparkle.draw(spriteBatch, num2 != 0, xOffset, yOffset);
        Vector2 vector2_2 = sparkle.Position - onScreenPosition;
        sparkle.Position = vector2_2;
      }
    }
  }
}
