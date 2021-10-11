// Decompiled with JetBrains decompiler
// Type: StardewValley.CraftingRecipe
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Locations;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley
{
  public class CraftingRecipe
  {
    private Dictionary<int, int> recipeList = new Dictionary<int, int>();
    private List<int> itemToProduce = new List<int>();
    public string name;
    public string DisplayName;
    private string description;
    public static Dictionary<string, string> craftingRecipes;
    public static Dictionary<string, string> cookingRecipes;
    public bool bigCraftable;
    public bool isCookingRecipe;
    public int timesCrafted;
    public int numberProducedPerCraft;

    public static void InitShared()
    {
      CraftingRecipe.craftingRecipes = Game1.content.Load<Dictionary<string, string>>("Data//CraftingRecipes");
      CraftingRecipe.cookingRecipes = Game1.content.Load<Dictionary<string, string>>("Data//CookingRecipes");
    }

    public CraftingRecipe(string name, bool isCookingRecipe)
    {
      this.isCookingRecipe = isCookingRecipe;
      this.name = name;
      string str1 = !isCookingRecipe || !CraftingRecipe.cookingRecipes.ContainsKey(name) ? (CraftingRecipe.craftingRecipes.ContainsKey(name) ? CraftingRecipe.craftingRecipes[name] : (string) null) : CraftingRecipe.cookingRecipes[name];
      if (str1 == null)
      {
        this.name = "Torch";
        name = "Torch";
        str1 = CraftingRecipe.craftingRecipes[name];
      }
      string[] strArray1 = str1.Split('/');
      string[] strArray2 = strArray1[0].Split(' ');
      int index1 = 0;
      while (index1 < strArray2.Length)
      {
        this.recipeList.Add(Convert.ToInt32(strArray2[index1]), Convert.ToInt32(strArray2[index1 + 1]));
        index1 += 2;
      }
      string[] strArray3 = strArray1[2].Split(' ');
      int index2 = 0;
      while (index2 < strArray3.Length)
      {
        this.itemToProduce.Add(Convert.ToInt32(strArray3[index2]));
        this.numberProducedPerCraft = strArray3.Length > 1 ? Convert.ToInt32(strArray3[index2 + 1]) : 1;
        index2 += 2;
      }
      this.bigCraftable = !isCookingRecipe && Convert.ToBoolean(strArray1[3]);
      try
      {
        string str2;
        if (!this.bigCraftable)
          str2 = Game1.objectInformation[this.itemToProduce[0]].Split('/')[5];
        else
          str2 = Game1.bigCraftablesInformation[this.itemToProduce[0]].Split('/')[4];
        this.description = str2;
      }
      catch (Exception ex)
      {
        this.description = "";
      }
      this.timesCrafted = Game1.player.craftingRecipes.ContainsKey(name) ? Game1.player.craftingRecipes[name] : 0;
      if (name.Equals("Crab Pot") && Game1.player.professions.Contains(7))
      {
        this.recipeList = new Dictionary<int, int>();
        this.recipeList.Add(388, 25);
        this.recipeList.Add(334, 2);
      }
      if (LocalizedContentManager.CurrentLanguageCode != LocalizedContentManager.LanguageCode.en)
        this.DisplayName = strArray1[strArray1.Length - 1];
      else
        this.DisplayName = name;
    }

    public int getIndexOfMenuView()
    {
      if (this.itemToProduce.Count <= 0)
        return -1;
      return this.itemToProduce[0];
    }

    public bool doesFarmerHaveIngredientsInInventory(List<Item> extraToCheck = null)
    {
      foreach (KeyValuePair<int, int> recipe in this.recipeList)
      {
        if (!Game1.player.hasItemInInventory(recipe.Key, recipe.Value, 5) && (extraToCheck == null || !Game1.player.hasItemInList(extraToCheck, recipe.Key, recipe.Value, 5)))
          return false;
      }
      return true;
    }

    public void drawMenuView(SpriteBatch b, int x, int y, float layerDepth = 0.88f, bool shadow = true)
    {
      if (this.bigCraftable)
        Utility.drawWithShadow(b, Game1.bigCraftableSpriteSheet, new Vector2((float) x, (float) y), Game1.getSourceRectForStandardTileSheet(Game1.bigCraftableSpriteSheet, this.getIndexOfMenuView(), 16, 32), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, false, layerDepth, -1, -1, 0.35f);
      else
        Utility.drawWithShadow(b, Game1.objectSpriteSheet, new Vector2((float) x, (float) y), Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.getIndexOfMenuView(), 16, 16), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, false, layerDepth, -1, -1, 0.35f);
    }

    public Item createItem()
    {
      int num = this.itemToProduce.ElementAt<int>(Game1.random.Next(this.itemToProduce.Count));
      if (this.bigCraftable)
      {
        if (this.name.Equals("Chest"))
          return (Item) new Chest(true);
        return (Item) new Object(Vector2.Zero, num, false);
      }
      if (this.name.Equals("Torch"))
        return (Item) new Torch(Vector2.Zero, this.numberProducedPerCraft);
      if (num >= 516 && num <= 534)
        return (Item) new Ring(num);
      return (Item) new Object(Vector2.Zero, num, this.numberProducedPerCraft);
    }

    public void consumeIngredients()
    {
      for (int index1 = this.recipeList.Count - 1; index1 >= 0; --index1)
      {
        int recipe1 = this.recipeList[this.recipeList.Keys.ElementAt<int>(index1)];
        bool flag = false;
        for (int index2 = Game1.player.items.Count - 1; index2 >= 0; --index2)
        {
          if (Game1.player.items[index2] != null && Game1.player.items[index2] is Object && !(Game1.player.items[index2] as Object).bigCraftable && (((Object) Game1.player.items[index2]).parentSheetIndex == this.recipeList.Keys.ElementAt<int>(index1) || Game1.player.items[index2].category == this.recipeList.Keys.ElementAt<int>(index1)))
          {
            int recipe2 = this.recipeList[this.recipeList.Keys.ElementAt<int>(index1)];
            Dictionary<int, int> recipeList = this.recipeList;
            int index3 = this.recipeList.Keys.ElementAt<int>(index1);
            recipeList[index3] = recipeList[index3] - Game1.player.items[index2].Stack;
            Game1.player.items[index2].Stack -= recipe2;
            if (Game1.player.items[index2].Stack <= 0)
              Game1.player.items[index2] = (Item) null;
            if (this.recipeList[this.recipeList.Keys.ElementAt<int>(index1)] <= 0)
            {
              this.recipeList[this.recipeList.Keys.ElementAt<int>(index1)] = recipe1;
              flag = true;
              break;
            }
          }
        }
        if (this.isCookingRecipe && !flag)
        {
          FarmHouse homeOfFarmer = Utility.getHomeOfFarmer(Game1.player);
          if (homeOfFarmer != null)
          {
            for (int index2 = homeOfFarmer.fridge.items.Count - 1; index2 >= 0; --index2)
            {
              if (homeOfFarmer.fridge.items[index2] != null && homeOfFarmer.fridge.items[index2] is Object && (((Object) homeOfFarmer.fridge.items[index2]).parentSheetIndex == this.recipeList.Keys.ElementAt<int>(index1) || homeOfFarmer.fridge.items[index2].category == this.recipeList.Keys.ElementAt<int>(index1)))
              {
                int recipe2 = this.recipeList[this.recipeList.Keys.ElementAt<int>(index1)];
                Dictionary<int, int> recipeList = this.recipeList;
                int index3 = this.recipeList.Keys.ElementAt<int>(index1);
                recipeList[index3] = recipeList[index3] - homeOfFarmer.fridge.items[index2].Stack;
                homeOfFarmer.fridge.items[index2].Stack -= recipe2;
                if (homeOfFarmer.fridge.items[index2].Stack <= 0)
                  homeOfFarmer.fridge.items[index2] = (Item) null;
                if (this.recipeList[this.recipeList.Keys.ElementAt<int>(index1)] <= 0)
                {
                  this.recipeList[this.recipeList.Keys.ElementAt<int>(index1)] = recipe1;
                  break;
                }
              }
            }
          }
        }
      }
    }

    public int getDescriptionHeight(int width)
    {
      return (int) ((double) Game1.smallFont.MeasureString(Game1.parseText(this.description, Game1.smallFont, width)).Y + (double) (this.getNumberOfIngredients() * (Game1.tileSize / 2 + Game1.pixelZoom)) + (double) (int) Game1.smallFont.MeasureString(Game1.content.LoadString("Strings\\StringsFromCSFiles:CraftingRecipe.cs.567")).Y + (double) (Game1.tileSize / 3));
    }

    public void drawRecipeDescription(SpriteBatch b, Vector2 position, int width)
    {
      b.Draw(Game1.staminaRect, new Rectangle((int) ((double) position.X + 8.0), (int) ((double) position.Y + (double) (Game1.tileSize / 2) + (double) Game1.smallFont.MeasureString("Ing").Y) - Game1.pixelZoom - 2, width - Game1.tileSize / 2, Game1.pixelZoom / 2), Game1.textColor * 0.35f);
      Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\StringsFromCSFiles:CraftingRecipe.cs.567"), Game1.smallFont, position + new Vector2(8f, (float) (Game1.tileSize / 2 - Game1.pixelZoom)), Game1.textColor * 0.75f, 1f, -1f, -1, -1, 1f, 3);
      for (int index = 0; index < this.recipeList.Count; ++index)
      {
        Color color = Game1.player.hasItemInInventory(this.recipeList.Keys.ElementAt<int>(index), this.recipeList.Values.ElementAt<int>(index), 8) ? Game1.textColor : Color.Red;
        if (this.isCookingRecipe && Game1.player.hasItemInList(Utility.getHomeOfFarmer(Game1.player).fridge.items, this.recipeList.Keys.ElementAt<int>(index), this.recipeList.Values.ElementAt<int>(index), 8))
          color = Game1.textColor;
        b.Draw(Game1.objectSpriteSheet, new Vector2(position.X, position.Y + (float) Game1.tileSize + (float) (index * Game1.tileSize / 2) + (float) (index * 4)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.getSpriteIndexFromRawIndex(this.recipeList.Keys.ElementAt<int>(index)), 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom / 2f, SpriteEffects.None, 0.86f);
        Utility.drawTinyDigits(this.recipeList.Values.ElementAt<int>(index), b, new Vector2(position.X + (float) (Game1.tileSize / 2) - Game1.tinyFont.MeasureString(string.Concat((object) this.recipeList.Values.ElementAt<int>(index))).X, position.Y + (float) Game1.tileSize + (float) (index * Game1.tileSize / 2) + (float) (index * 4) + (float) (Game1.tileSize / 3)), (float) Game1.pixelZoom / 2f, 0.87f, Color.AntiqueWhite);
        Utility.drawTextWithShadow(b, this.getNameFromIndex(this.recipeList.Keys.ElementAt<int>(index)), Game1.smallFont, new Vector2((float) ((double) position.X + (double) (Game1.tileSize / 2) + 8.0), (float) ((double) position.Y + (double) Game1.tileSize + (double) (index * Game1.tileSize / 2) + (double) (index * 4) + 4.0)), color, 1f, -1f, -1, -1, 1f, 3);
      }
      b.Draw(Game1.staminaRect, new Rectangle((int) position.X + 8, (int) position.Y + Game1.tileSize + Game1.pixelZoom + this.recipeList.Count * (Game1.tileSize / 2 + 4), width - Game1.tileSize / 2, Game1.pixelZoom / 2), Game1.textColor * 0.35f);
      Utility.drawTextWithShadow(b, Game1.parseText(this.description, Game1.smallFont, width - 8), Game1.smallFont, position + new Vector2(0.0f, (float) (Game1.tileSize + Game1.pixelZoom * 3 + this.recipeList.Count * (Game1.tileSize / 2 + 4))), Game1.textColor * 0.75f, 1f, -1f, -1, -1, 1f, 3);
    }

    public int getNumberOfIngredients()
    {
      return this.recipeList.Count;
    }

    public int getSpriteIndexFromRawIndex(int index)
    {
      switch (index)
      {
        case -6:
          return 184;
        case -5:
          return 176;
        case -4:
          return 145;
        case -3:
          return 24;
        case -2:
          return 80;
        case -1:
          return 20;
        default:
          return index;
      }
    }

    public string getNameFromIndex(int index)
    {
      if (index < 0)
      {
        switch (index)
        {
          case -6:
            return Game1.content.LoadString("Strings\\StringsFromCSFiles:CraftingRecipe.cs.573");
          case -5:
            return Game1.content.LoadString("Strings\\StringsFromCSFiles:CraftingRecipe.cs.572");
          case -4:
            return Game1.content.LoadString("Strings\\StringsFromCSFiles:CraftingRecipe.cs.571");
          case -3:
            return Game1.content.LoadString("Strings\\StringsFromCSFiles:CraftingRecipe.cs.570");
          case -2:
            return Game1.content.LoadString("Strings\\StringsFromCSFiles:CraftingRecipe.cs.569");
          case -1:
            return Game1.content.LoadString("Strings\\StringsFromCSFiles:CraftingRecipe.cs.568");
          default:
            return "???";
        }
      }
      else
      {
        string str = Game1.content.LoadString("Strings\\StringsFromCSFiles:CraftingRecipe.cs.575");
        if (Game1.objectInformation.ContainsKey(index))
          str = Game1.objectInformation[index].Split('/')[4];
        return str;
      }
    }
  }
}
