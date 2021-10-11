// Decompiled with JetBrains decompiler
// Type: StardewValley.BellsAndWhistles.SpriteText
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using BmFont;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.BellsAndWhistles
{
  public class SpriteText
  {
    public static float fontPixelZoom = 3f;
    public static float shadowAlpha = 0.15f;
    private static FontFile FontFile = (FontFile) null;
    private static List<Texture2D> fontPages = (List<Texture2D>) null;
    public const int scrollStyle_scroll = 0;
    public const int scrollStyle_scrollleftjustified = 0;
    public const int scrollStyle_speechBubble = 1;
    public const int scrollStyle_darkMetal = 2;
    public const int maxCharacter = 999999;
    public const int maxHeight = 999999;
    public const int characterWidth = 8;
    public const int characterHeight = 16;
    public const int horizontalSpaceBetweenCharacters = 0;
    public const int verticalSpaceBetweenCharacters = 2;
    public const char newLine = '^';
    private static Dictionary<char, FontChar> _characterMap;
    public static Texture2D spriteTexture;
    public static Texture2D coloredTexture;
    public const int color_Black = 0;
    public const int color_Blue = 1;
    public const int color_Red = 2;
    public const int color_Purple = 3;
    public const int color_White = 4;
    public const int color_Orange = 5;
    public const int color_Green = 6;
    public const int color_Cyan = 7;

    public static void drawStringHorizontallyCenteredAt(SpriteBatch b, string s, int x, int y, int characterPosition = 999999, int width = -1, int height = 999999, float alpha = 1f, float layerDepth = 0.88f, bool junimoText = false, int color = -1)
    {
      SpriteText.drawString(b, s, x - SpriteText.getWidthOfString(s) / 2, y, characterPosition, width, height, alpha, layerDepth, junimoText, -1, "", color);
    }

    public static int getWidthOfString(string s)
    {
      SpriteText.setUpCharacterMap();
      int val1 = 0;
      int val2 = 0;
      for (int index = 0; index < s.Length; ++index)
      {
        if (!LocalizedContentManager.CurrentLanguageLatin)
        {
          FontChar fontChar;
          if (SpriteText._characterMap.TryGetValue(s[index], out fontChar))
            val1 += fontChar.XAdvance;
          val2 = Math.Max(val1, val2);
          if ((int) s[index] == 94)
            val1 = 0;
        }
        else
        {
          val1 += 8 + SpriteText.getWidthOffsetForChar(s[index]);
          val2 = Math.Max(val1, val2);
          if ((int) s[index] == 94)
            val1 = 0;
        }
      }
      return (int) ((double) val2 * (double) SpriteText.fontPixelZoom);
    }

    public static int getHeightOfString(string s, int widthConstraint = 999999)
    {
      if (s.Length == 0)
        return 0;
      Vector2 vector2 = new Vector2();
      int accumulatedHorizontalSpaceBetweenCharacters = 0;
      s = s.Replace(Environment.NewLine, "");
      SpriteText.setUpCharacterMap();
      if (!LocalizedContentManager.CurrentLanguageLatin)
      {
        for (int index = 0; index < s.Length; ++index)
        {
          if ((int) s[index] == 94)
          {
            vector2.Y += (float) (SpriteText.FontFile.Common.LineHeight + 2) * SpriteText.fontPixelZoom;
            vector2.X = 0.0f;
          }
          else
          {
            if (SpriteText.positionOfNextSpace(s, index, (int) vector2.X, accumulatedHorizontalSpaceBetweenCharacters) >= widthConstraint)
            {
              vector2.Y += (float) (SpriteText.FontFile.Common.LineHeight + 2) * SpriteText.fontPixelZoom;
              accumulatedHorizontalSpaceBetweenCharacters = 0;
              vector2.X = 0.0f;
            }
            FontChar fontChar;
            if (SpriteText._characterMap.TryGetValue(s[index], out fontChar))
              vector2.X += (float) fontChar.XAdvance * SpriteText.fontPixelZoom;
          }
        }
        return (int) ((double) vector2.Y + (double) (SpriteText.FontFile.Common.LineHeight + 2) * (double) SpriteText.fontPixelZoom);
      }
      for (int index = 0; index < s.Length; ++index)
      {
        if ((int) s[index] == 94)
        {
          vector2.Y += 18f * SpriteText.fontPixelZoom;
          vector2.X = 0.0f;
          accumulatedHorizontalSpaceBetweenCharacters = 0;
        }
        else
        {
          if (index > 0)
            vector2.X += (float) (8.0 * (double) SpriteText.fontPixelZoom + (double) accumulatedHorizontalSpaceBetweenCharacters + (double) (SpriteText.getWidthOffsetForChar(s[index]) + SpriteText.getWidthOffsetForChar(s[index - 1])) * (double) SpriteText.fontPixelZoom);
          accumulatedHorizontalSpaceBetweenCharacters = (int) (0.0 * (double) SpriteText.fontPixelZoom);
          if (SpriteText.positionOfNextSpace(s, index, (int) vector2.X, accumulatedHorizontalSpaceBetweenCharacters) >= widthConstraint)
          {
            vector2.Y += 18f * SpriteText.fontPixelZoom;
            accumulatedHorizontalSpaceBetweenCharacters = 0;
            vector2.X = 0.0f;
          }
        }
      }
      return (int) ((double) vector2.Y + 16.0 * (double) SpriteText.fontPixelZoom);
    }

    public static Color getColorFromIndex(int index)
    {
      switch (index)
      {
        case -1:
          if (LocalizedContentManager.CurrentLanguageLatin)
            return Color.White;
          return new Color(86, 22, 12);
        case 1:
          return Color.SkyBlue;
        case 2:
          return Color.Red;
        case 3:
          return new Color(110, 43, (int) byte.MaxValue);
        case 4:
          return Color.White;
        case 5:
          return Color.OrangeRed;
        case 6:
          return Color.LimeGreen;
        case 7:
          return Color.Cyan;
        default:
          return Color.Black;
      }
    }

    public static string getSubstringBeyondHeight(string s, int width, int height)
    {
      Vector2 vector2 = new Vector2();
      int accumulatedHorizontalSpaceBetweenCharacters = 0;
      s = s.Replace(Environment.NewLine, "");
      SpriteText.setUpCharacterMap();
      if (!LocalizedContentManager.CurrentLanguageLatin)
      {
        for (int index = 0; index < s.Length; ++index)
        {
          if ((int) s[index] == 94)
          {
            vector2.Y += (float) (SpriteText.FontFile.Common.LineHeight + 2) * SpriteText.fontPixelZoom;
            vector2.X = 0.0f;
            accumulatedHorizontalSpaceBetweenCharacters = 0;
          }
          else
          {
            FontChar fontChar;
            if (SpriteText._characterMap.TryGetValue(s[index], out fontChar))
            {
              if (index > 0)
                vector2.X += (float) fontChar.XAdvance * SpriteText.fontPixelZoom;
              if (SpriteText.positionOfNextSpace(s, index, (int) vector2.X, accumulatedHorizontalSpaceBetweenCharacters) >= width)
              {
                vector2.Y += (float) (SpriteText.FontFile.Common.LineHeight + 2) * SpriteText.fontPixelZoom;
                accumulatedHorizontalSpaceBetweenCharacters = 0;
                vector2.X = 0.0f;
              }
            }
            if ((double) vector2.Y >= (double) height - (double) SpriteText.FontFile.Common.LineHeight * (double) SpriteText.fontPixelZoom * 2.0)
              return s.Substring(SpriteText.getLastSpace(s, index));
          }
        }
        return "";
      }
      for (int index = 0; index < s.Length; ++index)
      {
        if ((int) s[index] == 94)
        {
          vector2.Y += 18f * SpriteText.fontPixelZoom;
          vector2.X = 0.0f;
          accumulatedHorizontalSpaceBetweenCharacters = 0;
        }
        else
        {
          if (index > 0)
            vector2.X += (float) (8.0 * (double) SpriteText.fontPixelZoom + (double) accumulatedHorizontalSpaceBetweenCharacters + (double) (SpriteText.getWidthOffsetForChar(s[index]) + SpriteText.getWidthOffsetForChar(s[index - 1])) * (double) SpriteText.fontPixelZoom);
          accumulatedHorizontalSpaceBetweenCharacters = (int) (0.0 * (double) SpriteText.fontPixelZoom);
          if (SpriteText.positionOfNextSpace(s, index, (int) vector2.X, accumulatedHorizontalSpaceBetweenCharacters) >= width)
          {
            vector2.Y += 18f * SpriteText.fontPixelZoom;
            accumulatedHorizontalSpaceBetweenCharacters = 0;
            vector2.X = 0.0f;
          }
          if ((double) vector2.Y >= (double) height - 16.0 * (double) SpriteText.fontPixelZoom * 2.0)
            return s.Substring(SpriteText.getLastSpace(s, index));
        }
      }
      return "";
    }

    public static int getIndexOfSubstringBeyondHeight(string s, int width, int height)
    {
      Vector2 vector2 = new Vector2();
      int accumulatedHorizontalSpaceBetweenCharacters = 0;
      s = s.Replace(Environment.NewLine, "");
      SpriteText.setUpCharacterMap();
      if (!LocalizedContentManager.CurrentLanguageLatin)
      {
        for (int index = 0; index < s.Length; ++index)
        {
          if ((int) s[index] == 94)
          {
            vector2.Y += (float) (SpriteText.FontFile.Common.LineHeight + 2) * SpriteText.fontPixelZoom;
            vector2.X = 0.0f;
            accumulatedHorizontalSpaceBetweenCharacters = 0;
          }
          else
          {
            FontChar fontChar;
            if (SpriteText._characterMap.TryGetValue(s[index], out fontChar))
            {
              if (index > 0)
                vector2.X += (float) fontChar.XAdvance * SpriteText.fontPixelZoom;
              if (SpriteText.positionOfNextSpace(s, index, (int) vector2.X, accumulatedHorizontalSpaceBetweenCharacters) >= width)
              {
                vector2.Y += (float) (SpriteText.FontFile.Common.LineHeight + 2) * SpriteText.fontPixelZoom;
                accumulatedHorizontalSpaceBetweenCharacters = 0;
                vector2.X = 0.0f;
              }
            }
            if ((double) vector2.Y >= (double) height - (double) SpriteText.FontFile.Common.LineHeight * (double) SpriteText.fontPixelZoom * 2.0)
              return index - 1;
          }
        }
        return s.Length - 1;
      }
      for (int index = 0; index < s.Length; ++index)
      {
        if ((int) s[index] == 94)
        {
          vector2.Y += 18f * SpriteText.fontPixelZoom;
          vector2.X = 0.0f;
          accumulatedHorizontalSpaceBetweenCharacters = 0;
        }
        else
        {
          if (index > 0)
            vector2.X += (float) (8.0 * (double) SpriteText.fontPixelZoom + (double) accumulatedHorizontalSpaceBetweenCharacters + (double) (SpriteText.getWidthOffsetForChar(s[index]) + SpriteText.getWidthOffsetForChar(s[index - 1])) * (double) SpriteText.fontPixelZoom);
          accumulatedHorizontalSpaceBetweenCharacters = (int) (0.0 * (double) SpriteText.fontPixelZoom);
          if (SpriteText.positionOfNextSpace(s, index, (int) vector2.X, accumulatedHorizontalSpaceBetweenCharacters) >= width)
          {
            vector2.Y += 18f * SpriteText.fontPixelZoom;
            accumulatedHorizontalSpaceBetweenCharacters = 0;
            vector2.X = 0.0f;
          }
          if ((double) vector2.Y >= (double) height - 16.0 * (double) SpriteText.fontPixelZoom)
            return index - 1;
        }
      }
      return s.Length - 1;
    }

    public static List<string> getStringBrokenIntoSectionsOfHeight(string s, int width, int height)
    {
      List<string> source = new List<string>();
      for (; s.Length > 0; s = s.Substring(source.Last<string>().Length))
      {
        string thisHeightCutoff = SpriteText.getStringPreviousToThisHeightCutoff(s, width, height);
        if (thisHeightCutoff.Length > 0)
          source.Add(thisHeightCutoff);
        else
          break;
      }
      return source;
    }

    public static string getStringPreviousToThisHeightCutoff(string s, int width, int height)
    {
      return s.Substring(0, SpriteText.getIndexOfSubstringBeyondHeight(s, width, height) + 1);
    }

    private static int getLastSpace(string s, int startIndex)
    {
      if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ja || LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.zh || LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.th)
        return startIndex;
      for (int index = startIndex; index >= 0; --index)
      {
        if ((int) s[index] == 32)
          return index;
      }
      return startIndex;
    }

    public static int getWidthOffsetForChar(char c)
    {
      if ((uint) c <= 46U)
      {
        if ((uint) c <= 36U)
        {
          if ((int) c != 33)
          {
            if ((int) c == 36)
              return 1;
            goto label_13;
          }
        }
        else
        {
          if ((int) c == 44 || (int) c == 46)
            return -2;
          goto label_13;
        }
      }
      else
      {
        if ((uint) c <= 108U)
        {
          switch (c)
          {
            case '^':
              return -8;
            case 'i':
              break;
            case 'j':
            case 'l':
              goto label_9;
            default:
              goto label_13;
          }
        }
        else
        {
          switch (c)
          {
            case '¡':
              goto label_9;
            case 'ì':
            case 'í':
            case 'î':
            case 'ï':
              break;
            default:
              goto label_13;
          }
        }
        return -1;
      }
label_9:
      return -1;
label_13:
      return 0;
    }

    public static void drawStringWithScrollCenteredAt(SpriteBatch b, string s, int x, int y, string placeHolderWidthText = "", float alpha = 1f, int color = -1, int scrollType = 0, float layerDepth = 0.88f, bool junimoText = false)
    {
      SpriteText.drawString(b, s, x - SpriteText.getWidthOfString(placeHolderWidthText.Length > 0 ? placeHolderWidthText : s) / 2, y, 999999, -1, 999999, alpha, layerDepth, junimoText, scrollType, placeHolderWidthText, color);
    }

    public static void drawStringWithScrollBackground(SpriteBatch b, string s, int x, int y, string placeHolderWidthText = "", float alpha = 1f, int color = -1)
    {
      SpriteText.drawString(b, s, x, y, 999999, -1, 999999, alpha, 0.88f, false, 0, placeHolderWidthText, color);
    }

    private static FontFile loadFont(string assetName)
    {
      return FontLoader.Parse(Game1.content.Load<XmlSource>(assetName).Source);
    }

    private static void setUpCharacterMap()
    {
      if (!LocalizedContentManager.CurrentLanguageLatin && SpriteText._characterMap == null)
      {
        SpriteText._characterMap = new Dictionary<char, FontChar>();
        SpriteText.fontPages = new List<Texture2D>();
        switch (LocalizedContentManager.CurrentLanguageCode)
        {
          case LocalizedContentManager.LanguageCode.ja:
            SpriteText.FontFile = SpriteText.loadFont("Fonts\\Japanese");
            SpriteText.fontPixelZoom = 1.75f;
            break;
          case LocalizedContentManager.LanguageCode.ru:
            SpriteText.FontFile = SpriteText.loadFont("Fonts\\Russian");
            SpriteText.fontPixelZoom = 3f;
            break;
          case LocalizedContentManager.LanguageCode.zh:
            SpriteText.FontFile = SpriteText.loadFont("Fonts\\Chinese");
            SpriteText.fontPixelZoom = 1.5f;
            break;
          case LocalizedContentManager.LanguageCode.th:
            SpriteText.FontFile = SpriteText.loadFont("Fonts\\Thai");
            SpriteText.fontPixelZoom = 1.5f;
            break;
        }
        foreach (FontChar fontChar in SpriteText.FontFile.Chars)
        {
          char id = (char) fontChar.ID;
          SpriteText._characterMap.Add(id, fontChar);
        }
        foreach (FontPage page in SpriteText.FontFile.Pages)
          SpriteText.fontPages.Add(Game1.content.Load<Texture2D>("Fonts\\" + page.File));
        LocalizedContentManager.OnLanguageChange += new LocalizedContentManager.LanguageChangedHandler(SpriteText.OnLanguageChange);
      }
      else
      {
        if (!LocalizedContentManager.CurrentLanguageLatin || (double) SpriteText.fontPixelZoom >= 3.0)
          return;
        SpriteText.fontPixelZoom = 3f;
      }
    }

    public static void drawString(SpriteBatch b, string s, int x, int y, int characterPosition = 999999, int width = -1, int height = 999999, float alpha = 1f, float layerDepth = 0.88f, bool junimoText = false, int drawBGScroll = -1, string placeHolderScrollWidthText = "", int color = -1)
    {
      SpriteText.setUpCharacterMap();
      if (width == -1)
      {
        width = Game1.graphics.GraphicsDevice.Viewport.Width - x;
        if (drawBGScroll == 1)
          width = SpriteText.getWidthOfString(s) * 2;
      }
      if ((double) SpriteText.fontPixelZoom < 4.0)
        y += (int) ((4.0 - (double) SpriteText.fontPixelZoom) * (double) Game1.pixelZoom);
      Vector2 vector2_1 = new Vector2((float) x, (float) y);
      int accumulatedHorizontalSpaceBetweenCharacters = 0;
      if (drawBGScroll != 1)
      {
        if ((double) vector2_1.X + (double) width > (double) (Game1.graphics.GraphicsDevice.Viewport.Width - Game1.pixelZoom))
          vector2_1.X = (float) (Game1.graphics.GraphicsDevice.Viewport.Width - width - Game1.pixelZoom);
        if ((double) vector2_1.X < 0.0)
          vector2_1.X = 0.0f;
      }
      if (drawBGScroll == 0 || drawBGScroll == 0)
      {
        b.Draw(Game1.mouseCursors, vector2_1 + new Vector2(-12f, -3f) * (float) Game1.pixelZoom, new Rectangle?(new Rectangle(325, 318, 12, 18)), Color.White * alpha, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, layerDepth - 1f / 1000f);
        b.Draw(Game1.mouseCursors, vector2_1 + new Vector2(0.0f, -3f) * (float) Game1.pixelZoom, new Rectangle?(new Rectangle(337, 318, 1, 18)), Color.White * alpha, 0.0f, Vector2.Zero, new Vector2((float) SpriteText.getWidthOfString(placeHolderScrollWidthText.Length > 0 ? placeHolderScrollWidthText : s), (float) Game1.pixelZoom), SpriteEffects.None, layerDepth - 1f / 1000f);
        b.Draw(Game1.mouseCursors, vector2_1 + new Vector2((float) SpriteText.getWidthOfString(placeHolderScrollWidthText.Length > 0 ? placeHolderScrollWidthText : s), (float) (-3 * Game1.pixelZoom)), new Rectangle?(new Rectangle(338, 318, 12, 18)), Color.White * alpha, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, layerDepth - 1f / 1000f);
        if (placeHolderScrollWidthText.Length > 0)
        {
          if (drawBGScroll != 0)
            x += SpriteText.getWidthOfString(placeHolderScrollWidthText) / 2 - SpriteText.getWidthOfString(s) / 2;
          vector2_1.X = (float) x;
        }
        vector2_1.Y += (4f - SpriteText.fontPixelZoom) * (float) Game1.pixelZoom;
      }
      else if (drawBGScroll == 1)
      {
        b.Draw(Game1.mouseCursors, vector2_1 + new Vector2(-7f, -3f) * (float) Game1.pixelZoom, new Rectangle?(new Rectangle(324, 299, 7, 17)), Color.White * alpha, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, layerDepth - 1f / 1000f);
        b.Draw(Game1.mouseCursors, vector2_1 + new Vector2(0.0f, -3f) * (float) Game1.pixelZoom, new Rectangle?(new Rectangle(331, 299, 1, 17)), Color.White * alpha, 0.0f, Vector2.Zero, new Vector2((float) SpriteText.getWidthOfString(placeHolderScrollWidthText.Length > 0 ? placeHolderScrollWidthText : s), (float) Game1.pixelZoom), SpriteEffects.None, layerDepth - 1f / 1000f);
        b.Draw(Game1.mouseCursors, vector2_1 + new Vector2((float) SpriteText.getWidthOfString(placeHolderScrollWidthText.Length > 0 ? placeHolderScrollWidthText : s), (float) (-3 * Game1.pixelZoom)), new Rectangle?(new Rectangle(332, 299, 7, 17)), Color.White * alpha, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, layerDepth - 1f / 1000f);
        b.Draw(Game1.mouseCursors, vector2_1 + new Vector2((float) (SpriteText.getWidthOfString(placeHolderScrollWidthText.Length > 0 ? placeHolderScrollWidthText : s) / 2), (float) (13 * Game1.pixelZoom)), new Rectangle?(new Rectangle(341, 308, 6, 5)), Color.White * alpha, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, layerDepth - 0.0001f);
        if (placeHolderScrollWidthText.Length > 0)
        {
          x += SpriteText.getWidthOfString(placeHolderScrollWidthText) / 2 - SpriteText.getWidthOfString(s) / 2;
          vector2_1.X = (float) x;
        }
        vector2_1.Y += (4f - SpriteText.fontPixelZoom) * (float) Game1.pixelZoom;
      }
      else if (drawBGScroll == 2)
      {
        b.Draw(Game1.mouseCursors, vector2_1 + new Vector2(-3f, -3f) * (float) Game1.pixelZoom, new Rectangle?(new Rectangle(327, 281, 3, 17)), Color.White * alpha, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, layerDepth - 1f / 1000f);
        b.Draw(Game1.mouseCursors, vector2_1 + new Vector2(0.0f, -3f) * (float) Game1.pixelZoom, new Rectangle?(new Rectangle(330, 281, 1, 17)), Color.White * alpha, 0.0f, Vector2.Zero, new Vector2((float) (SpriteText.getWidthOfString(placeHolderScrollWidthText.Length > 0 ? placeHolderScrollWidthText : s) + Game1.pixelZoom), (float) Game1.pixelZoom), SpriteEffects.None, layerDepth - 1f / 1000f);
        b.Draw(Game1.mouseCursors, vector2_1 + new Vector2((float) (SpriteText.getWidthOfString(placeHolderScrollWidthText.Length > 0 ? placeHolderScrollWidthText : s) + Game1.pixelZoom), (float) (-3 * Game1.pixelZoom)), new Rectangle?(new Rectangle(333, 281, 3, 17)), Color.White * alpha, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, layerDepth - 1f / 1000f);
        if (placeHolderScrollWidthText.Length > 0)
        {
          x += SpriteText.getWidthOfString(placeHolderScrollWidthText) / 2 - SpriteText.getWidthOfString(s) / 2;
          vector2_1.X = (float) x;
        }
        vector2_1.Y += (4f - SpriteText.fontPixelZoom) * (float) Game1.pixelZoom;
      }
      s = s.Replace(Environment.NewLine, "");
      if (!junimoText && (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ja || LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.zh || LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.th))
        vector2_1.Y -= (4f - SpriteText.fontPixelZoom) * (float) Game1.pixelZoom;
      s = s.Replace('♡', '<');
      for (int index = 0; index < Math.Min(s.Length, characterPosition); ++index)
      {
        if (((LocalizedContentManager.CurrentLanguageLatin ? 1 : (SpriteText.IsSpecialCharacter(s[index]) ? 1 : 0)) | (junimoText ? 1 : 0)) != 0)
        {
          float fontPixelZoom = SpriteText.fontPixelZoom;
          if (SpriteText.IsSpecialCharacter(s[index]) | junimoText)
            SpriteText.fontPixelZoom = 3f;
          if ((int) s[index] == 94)
          {
            vector2_1.Y += 18f * SpriteText.fontPixelZoom;
            vector2_1.X = (float) x;
            accumulatedHorizontalSpaceBetweenCharacters = 0;
          }
          else
          {
            if (index > 0)
              vector2_1.X += (float) (8.0 * (double) SpriteText.fontPixelZoom + (double) accumulatedHorizontalSpaceBetweenCharacters + (double) (SpriteText.getWidthOffsetForChar(s[index]) + SpriteText.getWidthOffsetForChar(s[index - 1])) * (double) SpriteText.fontPixelZoom);
            accumulatedHorizontalSpaceBetweenCharacters = (int) (0.0 * (double) SpriteText.fontPixelZoom);
            if (SpriteText.positionOfNextSpace(s, index, (int) vector2_1.X, accumulatedHorizontalSpaceBetweenCharacters) >= x + width - Game1.pixelZoom)
            {
              vector2_1.Y += 18f * SpriteText.fontPixelZoom;
              accumulatedHorizontalSpaceBetweenCharacters = 0;
              vector2_1.X = (float) x;
            }
            bool flag = char.IsUpper(s[index]) || (int) s[index] == 223;
            Vector2 vector2_2 = new Vector2(0.0f, (float) ((!junimoText & flag ? -3 : 0) - 1));
            if ((int) s[index] == 199)
              vector2_2.Y += 2f;
            b.Draw(color != -1 ? SpriteText.coloredTexture : SpriteText.spriteTexture, vector2_1 + vector2_2 * SpriteText.fontPixelZoom, new Rectangle?(SpriteText.getSourceRectForChar(s[index], junimoText)), (SpriteText.IsSpecialCharacter(s[index]) | junimoText ? Color.White : SpriteText.getColorFromIndex(color)) * alpha, 0.0f, Vector2.Zero, SpriteText.fontPixelZoom, SpriteEffects.None, layerDepth);
            SpriteText.fontPixelZoom = fontPixelZoom;
          }
        }
        else if ((int) s[index] == 94)
        {
          vector2_1.Y += (float) (SpriteText.FontFile.Common.LineHeight + 2) * SpriteText.fontPixelZoom;
          vector2_1.X = (float) x;
          accumulatedHorizontalSpaceBetweenCharacters = 0;
        }
        else
        {
          if (index > 0 && SpriteText.IsSpecialCharacter(s[index - 1]))
            vector2_1.X += 24f;
          FontChar fontChar;
          if (SpriteText._characterMap.TryGetValue(s[index], out fontChar))
          {
            Rectangle rectangle = new Rectangle(fontChar.X, fontChar.Y, fontChar.Width, fontChar.Height);
            Texture2D fontPage = SpriteText.fontPages[fontChar.Page];
            if (SpriteText.positionOfNextSpace(s, index, (int) vector2_1.X, accumulatedHorizontalSpaceBetweenCharacters) >= x + width - Game1.pixelZoom)
            {
              vector2_1.Y += (float) (SpriteText.FontFile.Common.LineHeight + 2) * SpriteText.fontPixelZoom;
              accumulatedHorizontalSpaceBetweenCharacters = 0;
              vector2_1.X = (float) x;
            }
            Vector2 position = new Vector2(vector2_1.X + (float) fontChar.XOffset * SpriteText.fontPixelZoom, vector2_1.Y + (float) fontChar.YOffset * SpriteText.fontPixelZoom);
            if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ru)
            {
              Vector2 vector2_2 = new Vector2(-1f, 1f) * SpriteText.fontPixelZoom;
              b.Draw(fontPage, position + vector2_2, new Rectangle?(rectangle), SpriteText.getColorFromIndex(color) * alpha * SpriteText.shadowAlpha, 0.0f, Vector2.Zero, SpriteText.fontPixelZoom, SpriteEffects.None, layerDepth);
              b.Draw(fontPage, position + new Vector2(0.0f, vector2_2.Y), new Rectangle?(rectangle), SpriteText.getColorFromIndex(color) * alpha * SpriteText.shadowAlpha, 0.0f, Vector2.Zero, SpriteText.fontPixelZoom, SpriteEffects.None, layerDepth);
              b.Draw(fontPage, position + new Vector2(vector2_2.X, 0.0f), new Rectangle?(rectangle), SpriteText.getColorFromIndex(color) * alpha * SpriteText.shadowAlpha, 0.0f, Vector2.Zero, SpriteText.fontPixelZoom, SpriteEffects.None, layerDepth);
            }
            b.Draw(fontPage, position, new Rectangle?(rectangle), SpriteText.getColorFromIndex(color) * alpha, 0.0f, Vector2.Zero, SpriteText.fontPixelZoom, SpriteEffects.None, layerDepth);
            vector2_1.X += (float) fontChar.XAdvance * SpriteText.fontPixelZoom;
          }
        }
      }
    }

    private static bool IsSpecialCharacter(char c)
    {
      if (!c.Equals('<') && !c.Equals('=') && (!c.Equals('>') && !c.Equals('@')) && (!c.Equals('$') && !c.Equals('`')))
        return c.Equals('+');
      return true;
    }

    private static void OnLanguageChange(LocalizedContentManager.LanguageCode code)
    {
      if (SpriteText._characterMap != null)
        SpriteText._characterMap.Clear();
      else
        SpriteText._characterMap = new Dictionary<char, FontChar>();
      if (SpriteText.fontPages != null)
        SpriteText.fontPages.Clear();
      else
        SpriteText.fontPages = new List<Texture2D>();
      switch (code)
      {
        case LocalizedContentManager.LanguageCode.ja:
          SpriteText.FontFile = SpriteText.loadFont("Fonts\\Japanese");
          SpriteText.fontPixelZoom = 1.75f;
          break;
        case LocalizedContentManager.LanguageCode.ru:
          SpriteText.FontFile = SpriteText.loadFont("Fonts\\Russian");
          SpriteText.fontPixelZoom = 3f;
          break;
        case LocalizedContentManager.LanguageCode.zh:
          SpriteText.FontFile = SpriteText.loadFont("Fonts\\Chinese");
          SpriteText.fontPixelZoom = 1.5f;
          break;
        case LocalizedContentManager.LanguageCode.th:
          SpriteText.FontFile = SpriteText.loadFont("Fonts\\Thai");
          SpriteText.fontPixelZoom = 1.5f;
          break;
      }
      foreach (FontChar fontChar in SpriteText.FontFile.Chars)
      {
        char id = (char) fontChar.ID;
        SpriteText._characterMap.Add(id, fontChar);
      }
      foreach (FontPage page in SpriteText.FontFile.Pages)
        SpriteText.fontPages.Add(Game1.content.Load<Texture2D>("Fonts\\" + page.File));
    }

    public static int positionOfNextSpace(string s, int index, int currentXPosition, int accumulatedHorizontalSpaceBetweenCharacters)
    {
      SpriteText.setUpCharacterMap();
      if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.zh || LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.th)
      {
        FontChar fontChar;
        if (SpriteText._characterMap.TryGetValue(s[index], out fontChar))
          return currentXPosition + (int) ((double) fontChar.XAdvance * (double) SpriteText.fontPixelZoom);
        return currentXPosition + (int) ((double) SpriteText.FontFile.Common.LineHeight * (double) SpriteText.fontPixelZoom);
      }
      if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ja)
      {
        FontChar fontChar;
        if (SpriteText._characterMap.TryGetValue(s[index], out fontChar))
          return currentXPosition + (int) ((double) fontChar.XAdvance * (double) SpriteText.fontPixelZoom);
        return currentXPosition + (int) ((double) SpriteText.FontFile.Common.LineHeight * (double) SpriteText.fontPixelZoom);
      }
      for (int index1 = index; index1 < s.Length; ++index1)
      {
        if (!LocalizedContentManager.CurrentLanguageLatin)
        {
          if ((int) s[index1] == 32)
            return currentXPosition;
          FontChar fontChar;
          if (SpriteText._characterMap.TryGetValue(s[index1], out fontChar))
            currentXPosition += (int) ((double) fontChar.XAdvance * (double) SpriteText.fontPixelZoom);
          else
            currentXPosition += (int) ((double) SpriteText.FontFile.Common.LineHeight * (double) SpriteText.fontPixelZoom);
        }
        else
        {
          if ((int) s[index1] == 32)
            return currentXPosition;
          currentXPosition += (int) (8.0 * (double) SpriteText.fontPixelZoom + (double) accumulatedHorizontalSpaceBetweenCharacters + (double) (SpriteText.getWidthOffsetForChar(s[index1]) + SpriteText.getWidthOffsetForChar(s[Math.Max(0, index1 - 1)])) * (double) SpriteText.fontPixelZoom);
          accumulatedHorizontalSpaceBetweenCharacters = (int) (0.0 * (double) SpriteText.fontPixelZoom);
        }
      }
      return currentXPosition;
    }

    private static Rectangle getSourceRectForChar(char c, bool junimoText)
    {
      int num = (int) c - 32;
      return new Rectangle(num * 8 % SpriteText.spriteTexture.Width, num * 8 / SpriteText.spriteTexture.Width * 16 + (junimoText ? 224 : 0), 8, 16);
    }
  }
}
