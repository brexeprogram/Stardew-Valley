// Decompiled with JetBrains decompiler
// Type: StardewValley.NameSelect
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StardewValley
{
  public class NameSelect
  {
    public static string name = "";
    private static int selection = 0;
    public const int maxNameLength = 9;
    public const int charactersPerRow = 15;
    private static List<char> namingCharacters;

    public static void load()
    {
      NameSelect.namingCharacters = new List<char>();
      int num = 0;
      while (num < 25)
      {
        for (int index = 0; index < 5; ++index)
          NameSelect.namingCharacters.Add((char) (97 + num + index));
        for (int index = 0; index < 5; ++index)
          NameSelect.namingCharacters.Add((char) (65 + num + index));
        if (num < 10)
        {
          for (int index = 0; index < 5; ++index)
            NameSelect.namingCharacters.Add((char) (48 + num + index));
        }
        else if (num < 15)
        {
          NameSelect.namingCharacters.Add('?');
          NameSelect.namingCharacters.Add('$');
          NameSelect.namingCharacters.Add('\'');
          NameSelect.namingCharacters.Add('#');
          NameSelect.namingCharacters.Add('[');
        }
        else if (num < 20)
        {
          NameSelect.namingCharacters.Add('-');
          NameSelect.namingCharacters.Add('=');
          NameSelect.namingCharacters.Add('~');
          NameSelect.namingCharacters.Add('&');
          NameSelect.namingCharacters.Add('!');
        }
        else
        {
          NameSelect.namingCharacters.Add('Z');
          NameSelect.namingCharacters.Add('z');
          NameSelect.namingCharacters.Add('<');
          NameSelect.namingCharacters.Add('"');
          NameSelect.namingCharacters.Add(']');
        }
        num += 5;
      }
    }

    public static void draw()
    {
      Viewport viewport1 = Game1.graphics.GraphicsDevice.Viewport;
      int width1 = viewport1.TitleSafeArea.Width;
      viewport1 = Game1.graphics.GraphicsDevice.Viewport;
      int num1 = viewport1.TitleSafeArea.Width % Game1.tileSize;
      int val1_1 = width1 - num1;
      viewport1 = Game1.graphics.GraphicsDevice.Viewport;
      int width2 = viewport1.Width;
      viewport1 = Game1.graphics.GraphicsDevice.Viewport;
      int num2 = viewport1.Width % Game1.tileSize;
      int val2_1 = width2 - num2 - Game1.tileSize * 2;
      int width3 = Math.Min(val1_1, val2_1);
      Viewport viewport2 = Game1.graphics.GraphicsDevice.Viewport;
      int height1 = viewport2.TitleSafeArea.Height;
      viewport2 = Game1.graphics.GraphicsDevice.Viewport;
      int num3 = viewport2.TitleSafeArea.Height % Game1.tileSize;
      int val1_2 = height1 - num3;
      viewport2 = Game1.graphics.GraphicsDevice.Viewport;
      int height2 = viewport2.Height;
      viewport2 = Game1.graphics.GraphicsDevice.Viewport;
      int num4 = viewport2.Height % Game1.tileSize;
      int val2_2 = height2 - num4 - Game1.tileSize;
      int height3 = Math.Min(val1_2, val2_2);
      int x1 = Game1.graphics.GraphicsDevice.Viewport.Width / 2 - width3 / 2;
      int y = Game1.graphics.GraphicsDevice.Viewport.Height / 2 - height3 / 2;
      int num5 = (width3 - Game1.tileSize * 2) / 15;
      int num6 = (height3 - Game1.tileSize * 4) / 5;
      Game1.drawDialogueBox(x1, y, width3, height3, false, true, (string) null, false);
      string text1 = "";
      string nameSelectType = Game1.nameSelectType;
      if (!(nameSelectType == "samBand"))
      {
        if (nameSelectType == "Animal" || nameSelectType == "playerName" || nameSelectType == "coopDwellerBorn")
          text1 = Game1.content.LoadString("Strings\\StringsFromCSFiles:NameSelect.cs.3860");
      }
      else
        text1 = Game1.content.LoadString("Strings\\StringsFromCSFiles:NameSelect.cs.3856");
      Game1.spriteBatch.DrawString(Game1.dialogueFont, text1, new Vector2((float) (x1 + 2 * Game1.tileSize), (float) (y + Game1.tileSize * 2)), Game1.textColor);
      int x2 = (int) Game1.dialogueFont.MeasureString(text1).X;
      string text2 = "";
      char namingCharacter;
      for (int index = 0; index < 9; ++index)
      {
        if (NameSelect.name.Length > index)
        {
          SpriteBatch spriteBatch = Game1.spriteBatch;
          SpriteFont dialogueFont1 = Game1.dialogueFont;
          namingCharacter = NameSelect.name[index];
          string text3 = namingCharacter.ToString() ?? "";
          double num7 = (double) (x1 + 2 * Game1.tileSize + x2) + (double) Game1.dialogueFont.MeasureString(text2).X;
          double x3 = (double) Game1.dialogueFont.MeasureString("_").X;
          SpriteFont dialogueFont2 = Game1.dialogueFont;
          namingCharacter = NameSelect.name[index];
          string text4 = namingCharacter.ToString() ?? "";
          double x4 = (double) dialogueFont2.MeasureString(text4).X;
          double num8 = (x3 - x4) / 2.0;
          Vector2 position = new Vector2((float) (num7 + num8 - 2.0), (float) (y + Game1.tileSize * 2 - Game1.tileSize / 10));
          Color textColor = Game1.textColor;
          spriteBatch.DrawString(dialogueFont1, text3, position, textColor);
        }
        text2 += "_ ";
      }
      Game1.spriteBatch.DrawString(Game1.dialogueFont, "_ _ _ _ _ _ _ _ _", new Vector2((float) (x1 + 2 * Game1.tileSize + x2), (float) (y + Game1.tileSize * 2)), Game1.textColor);
      Game1.spriteBatch.DrawString(Game1.dialogueFont, Game1.content.LoadString("Strings\\StringsFromCSFiles:NameSelect.cs.3864"), new Vector2((float) (x1 + width3 - Game1.tileSize * 3), (float) (y + height3 - Game1.tileSize * 3 / 2)), Game1.textColor);
      for (int index1 = 0; index1 < 5; ++index1)
      {
        int num7 = 0;
        for (int index2 = 0; index2 < 15; ++index2)
        {
          if (index2 != 0 && index2 % 5 == 0)
            num7 += num5 / 3;
          SpriteBatch spriteBatch = Game1.spriteBatch;
          SpriteFont dialogueFont = Game1.dialogueFont;
          namingCharacter = NameSelect.namingCharacters[index1 * 15 + index2];
          string text3 = namingCharacter.ToString() ?? "";
          Vector2 position = new Vector2((float) (num7 + x1 + Game1.tileSize + num5 * index2), (float) (y + Game1.tileSize * 3 + num6 * index1));
          Color textColor = Game1.textColor;
          spriteBatch.DrawString(dialogueFont, text3, position, textColor);
          if (NameSelect.selection == index1 * 15 + index2)
            Game1.spriteBatch.Draw(Game1.objectSpriteSheet, new Vector2((float) (num7 + x1 + num5 * index2 - Game1.tileSize / 10), (float) (y + Game1.tileSize * 3 + num6 * index1 - Game1.tileSize / 8)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 26, -1, -1)), Color.White);
        }
      }
      if (NameSelect.selection != -1)
        return;
      Game1.spriteBatch.Draw(Game1.objectSpriteSheet, new Vector2((float) (x1 + width3 - Game1.tileSize * 3 - Game1.tileSize - Game1.tileSize / 10), (float) (y + height3 - Game1.tileSize * 3 / 2 - Game1.tileSize / 8)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 26, -1, -1)), Color.White);
    }

    public static bool select()
    {
      if (NameSelect.selection == -1)
      {
        if (NameSelect.name.Length > 0)
          return true;
      }
      else if (NameSelect.name.Length < 9)
      {
        NameSelect.name += NameSelect.namingCharacters[NameSelect.selection].ToString();
        Game1.playSound("smallSelect");
      }
      return false;
    }

    public static void startButton()
    {
      if (NameSelect.name.Length <= 0)
        return;
      NameSelect.selection = -1;
      Game1.playSound("smallSelect");
    }

    public static bool isOnDone()
    {
      return NameSelect.selection == -1;
    }

    public static void backspace()
    {
      if (NameSelect.name.Length <= 0)
        return;
      NameSelect.name = NameSelect.name.Remove(NameSelect.name.Length - 1);
      Game1.playSound("toolSwap");
    }

    public static bool cancel()
    {
      if ((Game1.nameSelectType.Equals("samBand") || Game1.nameSelectType.Equals("coopDwellerBorn")) && NameSelect.name.Length > 0)
      {
        Game1.playSound("toolSwap");
        NameSelect.name = NameSelect.name.Remove(NameSelect.name.Length - 1);
        return false;
      }
      NameSelect.selection = 0;
      NameSelect.name = "";
      return true;
    }

    public static void moveSelection(int direction)
    {
      Game1.playSound("toolSwap");
      if (direction.Equals(0))
      {
        if (NameSelect.selection == -1)
          NameSelect.selection = NameSelect.namingCharacters.Count - 2;
        else if (NameSelect.selection - 15 < 0)
          NameSelect.selection = NameSelect.namingCharacters.Count - 15 + NameSelect.selection;
        else
          NameSelect.selection -= 15;
      }
      else if (direction.Equals(1))
      {
        ++NameSelect.selection;
        if (NameSelect.selection % 15 != 0)
          return;
        NameSelect.selection -= 15;
      }
      else if (direction.Equals(2))
      {
        if (NameSelect.selection >= NameSelect.namingCharacters.Count - 2)
        {
          NameSelect.selection = -1;
        }
        else
        {
          NameSelect.selection += 15;
          if (NameSelect.selection < NameSelect.namingCharacters.Count)
            return;
          NameSelect.selection -= NameSelect.namingCharacters.Count;
        }
      }
      else
      {
        if (!direction.Equals(3))
          return;
        if (NameSelect.selection % 15 == 0)
          NameSelect.selection += 14;
        else
          --NameSelect.selection;
      }
    }
  }
}
