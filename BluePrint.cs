// Decompiled with JetBrains decompiler
// Type: StardewValley.BluePrint
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StardewValley
{
  public class BluePrint
  {
    public List<string> namesOfOkayBuildingLocations = new List<string>();
    public Dictionary<int, int> itemsRequired = new Dictionary<int, int>();
    public string name;
    public int woodRequired;
    public int stoneRequired;
    public int copperRequired;
    public int IronRequired;
    public int GoldRequired;
    public int IridiumRequired;
    public int tilesWidth;
    public int tilesHeight;
    public int maxOccupants;
    public int moneyRequired;
    public Point humanDoor;
    public Point animalDoor;
    public string mapToWarpTo;
    public string displayName;
    public string description;
    public string blueprintType;
    public string nameOfBuildingToUpgrade;
    public string actionBehavior;
    public Texture2D texture;
    public Rectangle sourceRectForMenuView;
    public bool canBuildOnCurrentMap;
    public bool magical;

    public BluePrint(string name)
    {
      this.name = name;
      if (name.Equals("Info Tool"))
      {
        this.texture = Game1.content.Load<Texture2D>("LooseSprites\\Cursors");
        this.displayName = name;
        this.description = Game1.content.LoadString("Strings\\StringsFromCSFiles:BluePrint.cs.1");
        this.sourceRectForMenuView = new Rectangle(9 * Game1.tileSize, 0, Game1.tileSize, Game1.tileSize);
      }
      else
      {
        Dictionary<string, string> dictionary = Game1.content.Load<Dictionary<string, string>>("Data\\Blueprints");
        string str1 = (string) null;
        string key = name;
        dictionary.TryGetValue(key, out str1);
        if (str1 == null)
          return;
        string[] strArray1 = str1.Split('/');
        if (strArray1[0].Equals("animal"))
        {
          try
          {
            this.texture = Game1.content.Load<Texture2D>("Animals\\" + (name.Equals("Chicken") ? "White Chicken" : name));
          }
          catch (Exception ex)
          {
            Game1.debugOutput = "Blueprint loaded with no texture!";
          }
          this.moneyRequired = Convert.ToInt32(strArray1[1]);
          this.sourceRectForMenuView = new Rectangle(0, 0, Convert.ToInt32(strArray1[2]), Convert.ToInt32(strArray1[3]));
          this.blueprintType = "Animals";
          this.tilesWidth = 1;
          this.tilesHeight = 1;
          this.displayName = strArray1[4];
          this.description = strArray1[5];
          this.humanDoor = new Point(-1, -1);
          this.animalDoor = new Point(-1, -1);
        }
        else
        {
          try
          {
            this.texture = Game1.content.Load<Texture2D>("Buildings\\" + name);
          }
          catch (Exception ex)
          {
          }
          string[] strArray2 = strArray1[0].Split(' ');
          int index = 0;
          while (index < strArray2.Length)
          {
            if (!strArray2[index].Equals(""))
              this.itemsRequired.Add(Convert.ToInt32(strArray2[index]), Convert.ToInt32(strArray2[index + 1]));
            index += 2;
          }
          this.tilesWidth = Convert.ToInt32(strArray1[1]);
          this.tilesHeight = Convert.ToInt32(strArray1[2]);
          this.humanDoor = new Point(Convert.ToInt32(strArray1[3]), Convert.ToInt32(strArray1[4]));
          this.animalDoor = new Point(Convert.ToInt32(strArray1[5]), Convert.ToInt32(strArray1[6]));
          this.mapToWarpTo = strArray1[7];
          this.displayName = strArray1[8];
          this.description = strArray1[9];
          this.blueprintType = strArray1[10];
          if (this.blueprintType.Equals("Upgrades"))
            this.nameOfBuildingToUpgrade = strArray1[11];
          this.sourceRectForMenuView = new Rectangle(0, 0, Convert.ToInt32(strArray1[12]), Convert.ToInt32(strArray1[13]));
          this.maxOccupants = Convert.ToInt32(strArray1[14]);
          this.actionBehavior = strArray1[15];
          string str2 = strArray1[16];
          char[] chArray = new char[1]{ ' ' };
          foreach (string str3 in str2.Split(chArray))
            this.namesOfOkayBuildingLocations.Add(str3);
          int num = 17;
          if (strArray1.Length > num)
            this.moneyRequired = Convert.ToInt32(strArray1[17]);
          if (strArray1.Length <= num + 1)
            return;
          this.magical = Convert.ToBoolean(strArray1[18]);
        }
      }
    }

    public void consumeResources()
    {
      foreach (KeyValuePair<int, int> keyValuePair in this.itemsRequired)
        Game1.player.consumeObject(keyValuePair.Key, keyValuePair.Value);
      Game1.player.Money -= this.moneyRequired;
    }

    public int getTileSheetIndexForStructurePlacementTile(int x, int y)
    {
      if (x == this.humanDoor.X && y == this.humanDoor.Y)
        return 2;
      return x == this.animalDoor.X && y == this.animalDoor.Y ? 4 : 0;
    }

    public bool isUpgrade()
    {
      if (this.nameOfBuildingToUpgrade != null)
        return this.nameOfBuildingToUpgrade.Length > 0;
      return false;
    }

    public bool doesFarmerHaveEnoughResourcesToBuild()
    {
      foreach (KeyValuePair<int, int> keyValuePair in this.itemsRequired)
      {
        if (!Game1.player.hasItemInInventory(keyValuePair.Key, keyValuePair.Value, 0))
          return false;
      }
      return Game1.player.Money >= this.moneyRequired;
    }

    public void drawDescription(SpriteBatch b, int x, int y, int width)
    {
      b.DrawString(Game1.smallFont, this.name, new Vector2((float) x, (float) y), Game1.textColor);
      string text = Game1.parseText(this.description, Game1.smallFont, width);
      b.DrawString(Game1.smallFont, text, new Vector2((float) x, (float) y + Game1.smallFont.MeasureString(this.name).Y), Game1.textColor * 0.75f);
      int num1 = (int) ((double) y + (double) Game1.smallFont.MeasureString(this.name).Y + (double) Game1.smallFont.MeasureString(text).Y);
      foreach (KeyValuePair<int, int> keyValuePair in this.itemsRequired)
      {
        b.Draw(Game1.objectSpriteSheet, new Vector2((float) (x + Game1.tileSize / 8), (float) num1), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, keyValuePair.Key, 16, 16)), Color.White, 0.0f, new Vector2(6f, 3f), (float) Game1.pixelZoom * 0.5f, SpriteEffects.None, 0.999f);
        Color color = Game1.player.hasItemInInventory(keyValuePair.Key, keyValuePair.Value, 0) ? Color.DarkGreen : Color.DarkRed;
        Utility.drawTinyDigits(keyValuePair.Value, b, new Vector2((float) (x + Game1.tileSize / 2) - Game1.tinyFont.MeasureString(string.Concat((object) keyValuePair.Value)).X, (float) (num1 + Game1.tileSize / 2) - Game1.tinyFont.MeasureString(string.Concat((object) keyValuePair.Value)).Y), 1f, 0.9f, Color.AntiqueWhite);
        b.DrawString(Game1.smallFont, Game1.objectInformation[keyValuePair.Key].Split('/')[4], new Vector2((float) (x + Game1.tileSize / 2 + Game1.tileSize / 4), (float) num1), color);
        num1 += (int) Game1.smallFont.MeasureString("P").Y;
      }
      if (this.moneyRequired <= 0)
        return;
      b.Draw(Game1.debrisSpriteSheet, new Vector2((float) x, (float) num1), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.debrisSpriteSheet, 8, -1, -1)), Color.White, 0.0f, new Vector2((float) (Game1.tileSize / 2 - Game1.tileSize / 8), (float) (Game1.tileSize / 2 - Game1.tileSize / 3)), 0.5f, SpriteEffects.None, 0.999f);
      Color color1 = Game1.player.money >= this.moneyRequired ? Color.DarkGreen : Color.DarkRed;
      b.DrawString(Game1.smallFont, Game1.content.LoadString("Strings\\StringsFromCSFiles:LoadGameMenu.cs.11020", (object) this.moneyRequired), new Vector2((float) (x + Game1.tileSize / 4 + Game1.tileSize / 8), (float) num1), color1);
      int num2 = num1 + (int) Game1.smallFont.MeasureString(string.Concat((object) this.moneyRequired)).Y;
    }
  }
}
