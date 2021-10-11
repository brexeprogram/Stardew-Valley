// Decompiled with JetBrains decompiler
// Type: StardewValley.Locations.SeedShop
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using xTile;

namespace StardewValley.Locations
{
  public class SeedShop : GameLocation
  {
    public List<Item> itemsFromPlayerToSell = new List<Item>();
    public List<Item> itemsToStartSellingTomorrow = new List<Item>();
    public const int maxItemsToSellFromPlayer = 11;

    public SeedShop()
    {
    }

    public SeedShop(Map map, string name)
      : base(map, name)
    {
    }

    public string getPurchasedItemDialogueForNPC(StardewValley.Object i, NPC n)
    {
      string str1 = "...";
      string[] strArray = Game1.content.LoadString("Strings\\Lexicon:GenericPlayerTerm").Split('^');
      string str2 = strArray[0];
      if (strArray.Length > 1 && !Game1.player.isMale)
        str2 = strArray[1];
      string str3 = Game1.random.NextDouble() < (double) (Game1.player.getFriendshipLevelForNPC(n.name) / 1250) ? Game1.player.name : str2;
      if (n.age != 0)
        str3 = Game1.player.name;
      string str4 = LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.en ? Game1.getProperArticleForWord(i.name) : "";
      if ((i.category == -4 || i.category == -75 || i.category == -79) && Game1.random.NextDouble() < 0.5)
        str4 = Game1.content.LoadString("Strings\\StringsFromCSFiles:SeedShop.cs.9701");
      int num = Game1.random.Next(5);
      if (n.manners == 2)
        num = 2;
      switch (num)
      {
        case 0:
          if (Game1.random.NextDouble() < (double) i.quality * 0.5 + 0.2)
          {
            str1 = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_1_QualityHigh", (object) str3, (object) str4, (object) i.DisplayName, (object) Lexicon.getRandomDeliciousAdjective(n));
            break;
          }
          str1 = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_1_QualityLow", (object) str3, (object) str4, (object) i.DisplayName, (object) Lexicon.getRandomNegativeFoodAdjective(n));
          break;
        case 1:
          if (i.quality == 0)
          {
            str1 = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_2_QualityLow", (object) str3, (object) str4, (object) i.DisplayName);
            break;
          }
          if (n.name.Equals("Jodi"))
          {
            str1 = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_2_QualityHigh_Jodi", (object) str3, (object) str4, (object) i.DisplayName);
            break;
          }
          str1 = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_2_QualityHigh", (object) str3, (object) str4, (object) i.DisplayName);
          break;
        case 2:
          if (n.manners == 2)
          {
            if (i.quality != 2)
            {
              str1 = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_3_QualityLow_Rude", (object) str3, (object) str4, (object) i.DisplayName, (object) (i.salePrice() / 2), (object) Lexicon.getRandomNegativeFoodAdjective(n), (object) Lexicon.getRandomNegativeItemSlanderNoun());
              break;
            }
            Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_3_QualityHigh_Rude", (object) str3, (object) str4, (object) i.DisplayName, (object) (i.salePrice() / 2), (object) Lexicon.getRandomSlightlyPositiveAdjectiveForEdibleNoun(n));
            break;
          }
          Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_3_NonRude", (object) str3, (object) str4, (object) i.DisplayName, (object) (i.salePrice() / 2));
          break;
        case 3:
          str1 = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_4", (object) str3, (object) str4, (object) i.DisplayName);
          break;
        case 4:
          if (i.category == -75 || i.category == -79)
          {
            str1 = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_5_VegetableOrFruit", (object) str3, (object) str4, (object) i.DisplayName);
            break;
          }
          if (i.category == -7)
          {
            string forEventOrPerson = Lexicon.getRandomPositiveAdjectiveForEventOrPerson(n);
            str1 = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_5_Cooking", (object) str3, (object) str4, (object) i.DisplayName, (object) Game1.getProperArticleForWord(forEventOrPerson), (object) forEventOrPerson);
            break;
          }
          str1 = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_5_Foraged", (object) str3, (object) str4, (object) i.DisplayName);
          break;
      }
      if (n.age == 1 && Game1.random.NextDouble() < 0.6)
        str1 = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_Teen", (object) str3, (object) str4, (object) i.DisplayName);
      string name = n.name;
      // ISSUE: reference to a compiler-generated method
      uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(name);
      if (stringHash <= 1708213605U)
      {
        if ((int) stringHash != 208794864)
        {
          if ((int) stringHash != 786557384)
          {
            if ((int) stringHash == 1708213605 && name == "Alex")
              str1 = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_Alex", (object) str3, (object) str4, (object) i.DisplayName);
          }
          else if (name == "Caroline")
          {
            if (i.quality == 0)
              str1 = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_Caroline_QualityLow", (object) str3, (object) str4, (object) i.DisplayName);
            else
              str1 = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_Caroline_QualityHigh", (object) str3, (object) str4, (object) i.DisplayName);
          }
        }
        else if (name == "Pierre")
        {
          if (i.quality == 0)
            str1 = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_Pierre_QualityLow", (object) str3, (object) str4, (object) i.DisplayName);
          else
            str1 = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_Pierre_QualityHigh", (object) str3, (object) str4, (object) i.DisplayName);
        }
      }
      else if (stringHash <= 2732913340U)
      {
        if ((int) stringHash != -1860673204)
        {
          if ((int) stringHash == -1562053956 && name == "Abigail")
          {
            if (i.quality == 0)
              str1 = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_Abigail_QualityLow", (object) str3, (object) str4, (object) i.DisplayName, (object) Lexicon.getRandomNegativeItemSlanderNoun());
            else
              str1 = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_Abigail_QualityHigh", (object) str3, (object) str4, (object) i.DisplayName);
          }
        }
        else if (name == "Haley")
          str1 = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_Haley", (object) str3, (object) str4, (object) i.DisplayName);
      }
      else if ((int) stringHash != -1468719973)
      {
        if ((int) stringHash == -1228790996 && name == "Elliott")
          str1 = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_Elliott", (object) str3, (object) str4, (object) i.DisplayName);
      }
      else if (name == "Leah")
        str1 = Game1.content.LoadString("Data\\ExtraDialogue:PurchasedItem_Leah", (object) str3, (object) str4, (object) i.DisplayName);
      return str1;
    }

    public override void DayUpdate(int dayOfMonth)
    {
      for (int index = this.itemsToStartSellingTomorrow.Count - 1; index >= 0; --index)
      {
        if (this.itemsFromPlayerToSell.Count < 11)
        {
          bool flag = false;
          foreach (Item obj in this.itemsFromPlayerToSell)
          {
            if (obj.Name.Equals(this.itemsToStartSellingTomorrow[index].Name) && (obj as StardewValley.Object).quality == (this.itemsToStartSellingTomorrow[index] as StardewValley.Object).quality)
            {
              ++obj.Stack;
              flag = true;
              break;
            }
          }
          if (!flag)
            this.itemsFromPlayerToSell.Add(this.itemsToStartSellingTomorrow[index]);
          this.itemsToStartSellingTomorrow.RemoveAt(index);
        }
      }
      base.DayUpdate(dayOfMonth);
    }

    public override void draw(SpriteBatch b)
    {
      base.draw(b);
      if (Game1.player.maxItems == 12)
        b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(new Vector2((float) (7 * Game1.tileSize + Game1.pixelZoom * 2), (float) (17 * Game1.tileSize))), new Rectangle?(new Rectangle((int) byte.MaxValue, 1436, 12, 14)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, (float) (19.25 * (double) Game1.tileSize / 10000.0));
      else if (Game1.player.maxItems < 36)
        b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(new Vector2((float) (7 * Game1.tileSize + Game1.pixelZoom * 2), (float) (17 * Game1.tileSize))), new Rectangle?(new Rectangle(267, 1436, 12, 14)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, (float) (19.25 * (double) Game1.tileSize / 10000.0));
      else
        b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Rectangle(7 * Game1.tileSize + Game1.pixelZoom, 18 * Game1.tileSize + Game1.tileSize / 2, Game1.tileSize * 3 / 2 + Game1.pixelZoom * 4, Game1.tileSize / 4 + Game1.pixelZoom)), new Rectangle?(new Rectangle(258, 1449, 1, 1)), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, (float) (19.25 * (double) Game1.tileSize / 10000.0));
    }

    public List<Item> shopStock()
    {
      List<Item> objList1 = new List<Item>();
      if (Game1.currentSeason.Equals("spring"))
      {
        objList1.Add((Item) new StardewValley.Object(Vector2.Zero, 472, int.MaxValue));
        objList1.Add((Item) new StardewValley.Object(Vector2.Zero, 473, int.MaxValue));
        objList1.Add((Item) new StardewValley.Object(Vector2.Zero, 474, int.MaxValue));
        objList1.Add((Item) new StardewValley.Object(Vector2.Zero, 475, int.MaxValue));
        objList1.Add((Item) new StardewValley.Object(Vector2.Zero, 427, int.MaxValue));
        objList1.Add((Item) new StardewValley.Object(Vector2.Zero, 477, int.MaxValue));
        objList1.Add((Item) new StardewValley.Object(Vector2.Zero, 429, int.MaxValue));
        if (Game1.year > 1)
          objList1.Add((Item) new StardewValley.Object(Vector2.Zero, 476, int.MaxValue));
      }
      if (Game1.currentSeason.Equals("summer"))
      {
        objList1.Add((Item) new StardewValley.Object(Vector2.Zero, 479, int.MaxValue));
        objList1.Add((Item) new StardewValley.Object(Vector2.Zero, 480, int.MaxValue));
        objList1.Add((Item) new StardewValley.Object(Vector2.Zero, 481, int.MaxValue));
        objList1.Add((Item) new StardewValley.Object(Vector2.Zero, 482, int.MaxValue));
        objList1.Add((Item) new StardewValley.Object(Vector2.Zero, 483, int.MaxValue));
        objList1.Add((Item) new StardewValley.Object(Vector2.Zero, 484, int.MaxValue));
        objList1.Add((Item) new StardewValley.Object(Vector2.Zero, 453, int.MaxValue));
        objList1.Add((Item) new StardewValley.Object(Vector2.Zero, 455, int.MaxValue));
        objList1.Add((Item) new StardewValley.Object(Vector2.Zero, 302, int.MaxValue));
        objList1.Add((Item) new StardewValley.Object(Vector2.Zero, 487, int.MaxValue));
        objList1.Add((Item) new StardewValley.Object(431, int.MaxValue, false, 100, 0));
        if (Game1.year > 1)
          objList1.Add((Item) new StardewValley.Object(Vector2.Zero, 485, int.MaxValue));
      }
      if (Game1.currentSeason.Equals("fall"))
      {
        objList1.Add((Item) new StardewValley.Object(Vector2.Zero, 490, int.MaxValue));
        objList1.Add((Item) new StardewValley.Object(Vector2.Zero, 487, int.MaxValue));
        objList1.Add((Item) new StardewValley.Object(Vector2.Zero, 488, int.MaxValue));
        objList1.Add((Item) new StardewValley.Object(Vector2.Zero, 491, int.MaxValue));
        objList1.Add((Item) new StardewValley.Object(Vector2.Zero, 492, int.MaxValue));
        objList1.Add((Item) new StardewValley.Object(Vector2.Zero, 493, int.MaxValue));
        objList1.Add((Item) new StardewValley.Object(Vector2.Zero, 483, int.MaxValue));
        objList1.Add((Item) new StardewValley.Object(431, int.MaxValue, false, 100, 0));
        objList1.Add((Item) new StardewValley.Object(Vector2.Zero, 425, int.MaxValue));
        objList1.Add((Item) new StardewValley.Object(Vector2.Zero, 299, int.MaxValue));
        objList1.Add((Item) new StardewValley.Object(Vector2.Zero, 301, int.MaxValue));
        if (Game1.year > 1)
          objList1.Add((Item) new StardewValley.Object(Vector2.Zero, 489, int.MaxValue));
      }
      objList1.Add((Item) new StardewValley.Object(Vector2.Zero, 297, int.MaxValue));
      objList1.Add((Item) new StardewValley.Object(Vector2.Zero, 245, int.MaxValue));
      objList1.Add((Item) new StardewValley.Object(Vector2.Zero, 246, int.MaxValue));
      objList1.Add((Item) new StardewValley.Object(Vector2.Zero, 423, int.MaxValue));
      objList1.Add((Item) new StardewValley.Object(Vector2.Zero, 247, int.MaxValue));
      objList1.Add((Item) new StardewValley.Object(Vector2.Zero, 419, int.MaxValue));
      if ((int) Game1.stats.DaysPlayed >= 15)
      {
        objList1.Add((Item) new StardewValley.Object(368, int.MaxValue, false, 50, 0));
        objList1.Add((Item) new StardewValley.Object(370, int.MaxValue, false, 50, 0));
        objList1.Add((Item) new StardewValley.Object(465, int.MaxValue, false, 50, 0));
      }
      if (Game1.year > 1)
      {
        objList1.Add((Item) new StardewValley.Object(369, int.MaxValue, false, 75, 0));
        objList1.Add((Item) new StardewValley.Object(371, int.MaxValue, false, 75, 0));
        objList1.Add((Item) new StardewValley.Object(466, int.MaxValue, false, 75, 0));
      }
      Random random = new Random((int) Game1.stats.DaysPlayed + (int) Game1.uniqueIDForThisGame / 2);
      int which = random.Next(112);
      if (which == 21)
        which = 36;
      List<Item> objList2 = objList1;
      Wallpaper wallpaper1 = new Wallpaper(which, false);
      int maxValue1 = int.MaxValue;
      wallpaper1.stack = maxValue1;
      objList2.Add((Item) wallpaper1);
      List<Item> objList3 = objList1;
      Wallpaper wallpaper2 = new Wallpaper(random.Next(40), true);
      int maxValue2 = int.MaxValue;
      wallpaper2.stack = maxValue2;
      objList3.Add((Item) wallpaper2);
      List<Item> objList4 = objList1;
      Furniture furniture = new Furniture(1308, Vector2.Zero);
      int maxValue3 = int.MaxValue;
      furniture.stack = maxValue3;
      objList4.Add((Item) furniture);
      objList1.Add((Item) new StardewValley.Object(628, int.MaxValue, false, 1700, 0));
      objList1.Add((Item) new StardewValley.Object(629, int.MaxValue, false, 1000, 0));
      objList1.Add((Item) new StardewValley.Object(630, int.MaxValue, false, 2000, 0));
      objList1.Add((Item) new StardewValley.Object(631, int.MaxValue, false, 3000, 0));
      objList1.Add((Item) new StardewValley.Object(632, int.MaxValue, false, 3000, 0));
      objList1.Add((Item) new StardewValley.Object(633, int.MaxValue, false, 2000, 0));
      foreach (Item obj in this.itemsFromPlayerToSell)
        objList1.Add(obj);
      if (Game1.player.hasAFriendWithHeartLevel(8, true))
        objList1.Add((Item) new StardewValley.Object(Vector2.Zero, 458, int.MaxValue));
      return objList1;
    }
  }
}
