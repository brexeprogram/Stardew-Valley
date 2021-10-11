// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.ColorPicker
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley.Menus
{
  public class ColorPicker
  {
    public const int sliderChunks = 24;
    private int colorIndexSelection;
    private Rectangle bounds;
    public SliderBar hueBar;
    public SliderBar valueBar;
    public SliderBar saturationBar;
    public SliderBar recentSliderBar;

    public ColorPicker(int x, int y)
    {
      this.hueBar = new SliderBar(0, 0, 50);
      this.saturationBar = new SliderBar(0, 20, 50);
      this.valueBar = new SliderBar(0, 40, 50);
      this.bounds = new Rectangle(x, y, SliderBar.defaultWidth, 60);
    }

    public Color getSelectedColor()
    {
      return this.HsvToRgb((double) this.hueBar.value / 100.0 * 360.0, (double) this.saturationBar.value / 100.0, (double) this.valueBar.value / 100.0);
    }

    public Color click(int x, int y)
    {
      if (this.bounds.Contains(x, y))
      {
        x -= this.bounds.X;
        y -= this.bounds.Y;
        if (this.hueBar.bounds.Contains(x, y))
        {
          this.hueBar.click(x, y);
          this.recentSliderBar = this.hueBar;
        }
        if (this.saturationBar.bounds.Contains(x, y))
        {
          this.recentSliderBar = this.saturationBar;
          this.saturationBar.click(x, y);
        }
        if (this.valueBar.bounds.Contains(x, y))
        {
          this.recentSliderBar = this.valueBar;
          this.valueBar.click(x, y);
        }
      }
      return this.getSelectedColor();
    }

    public void changeHue(int amount)
    {
      this.hueBar.changeValueBy(amount);
      this.recentSliderBar = this.hueBar;
    }

    public void changeSaturation(int amount)
    {
      this.saturationBar.changeValueBy(amount);
      this.recentSliderBar = this.saturationBar;
    }

    public void changeValue(int amount)
    {
      this.valueBar.changeValueBy(amount);
      this.recentSliderBar = this.valueBar;
    }

    public Color clickHeld(int x, int y)
    {
      if (this.recentSliderBar != null)
      {
        x = Math.Max(x, this.bounds.X);
        x = Math.Min(x, this.bounds.Right - 1);
        y = this.recentSliderBar.bounds.Center.Y;
        x -= this.bounds.X;
        if (this.recentSliderBar.Equals((object) this.hueBar))
          this.hueBar.click(x, y);
        if (this.recentSliderBar.Equals((object) this.saturationBar))
          this.saturationBar.click(x, y);
        if (this.recentSliderBar.Equals((object) this.valueBar))
          this.valueBar.click(x, y);
      }
      return this.getSelectedColor();
    }

    public void releaseClick()
    {
      this.hueBar.release(0, 0);
      this.saturationBar.release(0, 0);
      this.valueBar.release(0, 0);
      this.recentSliderBar = (SliderBar) null;
    }

    public void draw(SpriteBatch b)
    {
      for (int index = 0; index < 24; ++index)
      {
        Color rgb = this.HsvToRgb((double) index / 24.0 * 360.0, 0.9, 0.9);
        b.Draw(Game1.staminaRect, new Rectangle(this.bounds.X + this.bounds.Width / 24 * index, this.bounds.Y + this.hueBar.bounds.Center.Y - 2, this.hueBar.bounds.Width / 24, 4), rgb);
      }
      b.Draw(Game1.mouseCursors, new Vector2((float) (this.bounds.X + (int) ((double) this.hueBar.value / 100.0 * (double) this.hueBar.bounds.Width)), (float) (this.bounds.Y + this.hueBar.bounds.Center.Y)), new Rectangle?(new Rectangle(64, 256, 32, 32)), Color.White, 0.0f, new Vector2(16f, 9f), 1f, SpriteEffects.None, 0.86f);
      Utility.drawTextWithShadow(b, string.Concat((object) this.hueBar.value), Game1.smallFont, new Vector2((float) (this.bounds.X + this.bounds.Width + Game1.pixelZoom * 2), (float) (this.bounds.Y + this.hueBar.bounds.Y)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
      for (int index = 0; index < 24; ++index)
      {
        Color rgb = this.HsvToRgb((double) this.hueBar.value / 100.0 * 360.0, (double) index / 24.0, (double) this.valueBar.value / 100.0);
        b.Draw(Game1.staminaRect, new Rectangle(this.bounds.X + this.bounds.Width / 24 * index, this.bounds.Y + this.saturationBar.bounds.Center.Y - 2, this.saturationBar.bounds.Width / 24, 4), rgb);
      }
      b.Draw(Game1.mouseCursors, new Vector2((float) (this.bounds.X + (int) ((double) this.saturationBar.value / 100.0 * (double) this.saturationBar.bounds.Width)), (float) (this.bounds.Y + this.saturationBar.bounds.Center.Y)), new Rectangle?(new Rectangle(64, 256, 32, 32)), Color.White, 0.0f, new Vector2(16f, 9f), 1f, SpriteEffects.None, 0.87f);
      Utility.drawTextWithShadow(b, string.Concat((object) this.saturationBar.value), Game1.smallFont, new Vector2((float) (this.bounds.X + this.bounds.Width + Game1.pixelZoom * 2), (float) (this.bounds.Y + this.saturationBar.bounds.Y)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
      for (int index = 0; index < 24; ++index)
      {
        Color rgb = this.HsvToRgb((double) this.hueBar.value / 100.0 * 360.0, (double) this.saturationBar.value / 100.0, (double) index / 24.0);
        b.Draw(Game1.staminaRect, new Rectangle(this.bounds.X + this.bounds.Width / 24 * index, this.bounds.Y + this.valueBar.bounds.Center.Y - 2, this.valueBar.bounds.Width / 24, 4), rgb);
      }
      b.Draw(Game1.mouseCursors, new Vector2((float) (this.bounds.X + (int) ((double) this.valueBar.value / 100.0 * (double) this.valueBar.bounds.Width)), (float) (this.bounds.Y + this.valueBar.bounds.Center.Y)), new Rectangle?(new Rectangle(64, 256, 32, 32)), Color.White, 0.0f, new Vector2(16f, 9f), 1f, SpriteEffects.None, 0.86f);
      Utility.drawTextWithShadow(b, string.Concat((object) this.valueBar.value), Game1.smallFont, new Vector2((float) (this.bounds.X + this.bounds.Width + Game1.pixelZoom * 2), (float) (this.bounds.Y + this.valueBar.bounds.Y)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
    }

    public bool containsPoint(int x, int y)
    {
      return this.bounds.Contains(x, y);
    }

    public void setColor(Color color)
    {
      float h;
      float s;
      float v;
      this.RGBtoHSV((float) color.R, (float) color.G, (float) color.B, out h, out s, out v);
      this.hueBar.value = (int) ((double) h / 360.0 * 100.0);
      this.saturationBar.value = (int) ((double) s * 100.0);
      this.valueBar.value = (int) ((double) v / (double) byte.MaxValue * 100.0);
    }

    private void RGBtoHSV(float r, float g, float b, out float h, out float s, out float v)
    {
      float num1 = Math.Min(Math.Min(r, g), b);
      float num2 = Math.Max(Math.Max(r, g), b);
      v = num2;
      float num3 = num2 - num1;
      if ((double) num2 != 0.0)
      {
        s = num3 / num2;
        h = (double) r != (double) num2 ? ((double) g != (double) num2 ? (float) (4.0 + ((double) r - (double) g) / (double) num3) : (float) (2.0 + ((double) b - (double) r) / (double) num3)) : (g - b) / num3;
        h = h * 60f;
        if ((double) h >= 0.0)
          return;
        h = h + 360f;
      }
      else
      {
        s = 0.0f;
        h = -1f;
      }
    }

    private Color HsvToRgb(double h, double S, double V)
    {
      double num1 = h;
      while (num1 < 0.0)
      {
        ++num1;
        if (num1 < -1000000.0)
          num1 = 0.0;
      }
      while (num1 >= 360.0)
        --num1;
      double num2;
      double num3;
      double num4;
      if (V <= 0.0)
      {
        double num5;
        num2 = num5 = 0.0;
        num3 = num5;
        num4 = num5;
      }
      else if (S <= 0.0)
      {
        double num5;
        num2 = num5 = V;
        num3 = num5;
        num4 = num5;
      }
      else
      {
        double d = num1 / 60.0;
        int num5 = (int) Math.Floor(d);
        double num6 = (double) num5;
        double num7 = d - num6;
        double num8 = V * (1.0 - S);
        double num9 = V * (1.0 - S * num7);
        double num10 = V * (1.0 - S * (1.0 - num7));
        switch (num5)
        {
          case -1:
            num4 = V;
            num3 = num8;
            num2 = num9;
            break;
          case 0:
            num4 = V;
            num3 = num10;
            num2 = num8;
            break;
          case 1:
            num4 = num9;
            num3 = V;
            num2 = num8;
            break;
          case 2:
            num4 = num8;
            num3 = V;
            num2 = num10;
            break;
          case 3:
            num4 = num8;
            num3 = num9;
            num2 = V;
            break;
          case 4:
            num4 = num10;
            num3 = num8;
            num2 = V;
            break;
          case 5:
            num4 = V;
            num3 = num8;
            num2 = num9;
            break;
          case 6:
            num4 = V;
            num3 = num10;
            num2 = num8;
            break;
          default:
            double num11;
            num2 = num11 = V;
            num3 = num11;
            num4 = num11;
            break;
        }
      }
      return new Color(this.Clamp((int) (num4 * (double) byte.MaxValue)), this.Clamp((int) (num3 * (double) byte.MaxValue)), this.Clamp((int) (num2 * (double) byte.MaxValue)));
    }

    private int Clamp(int i)
    {
      if (i < 0)
        return 0;
      if (i > (int) byte.MaxValue)
        return (int) byte.MaxValue;
      return i;
    }
  }
}
