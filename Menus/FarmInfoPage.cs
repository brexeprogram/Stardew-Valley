// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.FarmInfoPage
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Buildings;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Menus
{
  public class FarmInfoPage : IClickableMenu
  {
    private string descriptionText = "";
    private string hoverText = "";
    private List<ClickableTextureComponent> animals = new List<ClickableTextureComponent>();
    private List<ClickableTextureComponent> mapBuildings = new List<ClickableTextureComponent>();
    private List<MiniatureTerrainFeature> mapFeatures = new List<MiniatureTerrainFeature>();
    private ClickableTextureComponent moneyIcon;
    private ClickableTextureComponent farmMap;
    private ClickableTextureComponent mapFarmer;
    private ClickableTextureComponent farmHouse;
    private Farm farm;
    private int mapX;
    private int mapY;

    public FarmInfoPage(int x, int y, int width, int height)
      : base(x, y, width, height, false)
    {
      this.moneyIcon = new ClickableTextureComponent("", new Rectangle(x + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2, y + IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 2, Game1.player.Money > 9999 ? 18 : 20, 16), Game1.player.Money.ToString() + "g", "", Game1.debrisSpriteSheet, new Rectangle(88, 280, 16, 16), 1f, false);
      this.mapX = x + IClickableMenu.spaceToClearSideBorder + Game1.tileSize * 2 + Game1.tileSize / 2 + Game1.tileSize / 4;
      this.mapY = y + IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 3 - 4;
      this.farmMap = new ClickableTextureComponent(new Rectangle(this.mapX, this.mapY, 20, 20), Game1.content.Load<Texture2D>("LooseSprites\\farmMap"), Rectangle.Empty, 1f, false);
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      int num4 = 0;
      int num5 = 0;
      int num6 = 0;
      int num7 = 0;
      int num8 = 0;
      int num9 = 0;
      int num10 = 0;
      int num11 = 0;
      int num12 = 0;
      int num13 = 0;
      int num14 = 0;
      int num15 = 0;
      int num16 = 0;
      this.farm = (Farm) Game1.getLocationFromName("Farm");
      this.farmHouse = new ClickableTextureComponent("FarmHouse", new Rectangle(this.mapX + 443, this.mapY + 43, 80, 72), "FarmHouse", "", Game1.content.Load<Texture2D>("Buildings\\houses"), new Rectangle(0, 0, 160, 144), 0.5f, false);
      foreach (FarmAnimal allFarmAnimal in this.farm.getAllFarmAnimals())
      {
        if (allFarmAnimal.type.Contains("Chicken"))
        {
          ++num1;
          num9 += allFarmAnimal.friendshipTowardFarmer;
        }
        else
        {
          string type = allFarmAnimal.type;
          if (!(type == "Cow"))
          {
            if (!(type == "Duck"))
            {
              if (!(type == "Rabbit"))
              {
                if (!(type == "Sheep"))
                {
                  if (!(type == "Goat"))
                  {
                    if (type == "Pig")
                    {
                      ++num8;
                      num16 += allFarmAnimal.friendshipTowardFarmer;
                    }
                    else
                    {
                      ++num4;
                      num12 += allFarmAnimal.friendshipTowardFarmer;
                    }
                  }
                  else
                  {
                    ++num7;
                    num14 += allFarmAnimal.friendshipTowardFarmer;
                  }
                }
                else
                {
                  ++num6;
                  num15 += allFarmAnimal.friendshipTowardFarmer;
                }
              }
              else
              {
                ++num3;
                num10 += allFarmAnimal.friendshipTowardFarmer;
              }
            }
            else
            {
              ++num2;
              num11 += allFarmAnimal.friendshipTowardFarmer;
            }
          }
          else
          {
            ++num5;
            num13 += allFarmAnimal.friendshipTowardFarmer;
          }
        }
      }
      List<ClickableTextureComponent> animals1 = this.animals;
      string name1 = "";
      Rectangle bounds1 = new Rectangle(x + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2, y + IClickableMenu.spaceToClearTopBorder + Game1.tileSize, Game1.tileSize / 2 + 8, Game1.tileSize / 2);
      string label1 = string.Concat((object) num1);
      string str1 = "Chickens";
      string str2;
      if (num1 <= 0)
        str2 = "";
      else
        str2 = Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:FarmInfoPage.cs.10425", (object) (num9 / num1));
      string hoverText1 = str1 + str2;
      Texture2D mouseCursors1 = Game1.mouseCursors;
      Rectangle sourceRect1 = new Rectangle(Game1.tileSize * 4, Game1.tileSize, Game1.tileSize / 2, Game1.tileSize / 2);
      double num17 = 1.0;
      int num18 = 0;
      ClickableTextureComponent textureComponent1 = new ClickableTextureComponent(name1, bounds1, label1, hoverText1, mouseCursors1, sourceRect1, (float) num17, num18 != 0);
      animals1.Add(textureComponent1);
      List<ClickableTextureComponent> animals2 = this.animals;
      string name2 = "";
      Rectangle bounds2 = new Rectangle(x + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2, y + IClickableMenu.spaceToClearTopBorder + Game1.tileSize + (Game1.tileSize / 2 + 4), Game1.tileSize / 2 + 8, Game1.tileSize / 2);
      string label2 = string.Concat((object) num2);
      string str3 = "Ducks";
      string str4;
      if (num2 <= 0)
        str4 = "";
      else
        str4 = Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:FarmInfoPage.cs.10425", (object) (num11 / num2));
      string hoverText2 = str3 + str4;
      Texture2D mouseCursors2 = Game1.mouseCursors;
      Rectangle sourceRect2 = new Rectangle(Game1.tileSize * 4 + Game1.tileSize / 2, Game1.tileSize, Game1.tileSize / 2, Game1.tileSize / 2);
      double num19 = 1.0;
      int num20 = 0;
      ClickableTextureComponent textureComponent2 = new ClickableTextureComponent(name2, bounds2, label2, hoverText2, mouseCursors2, sourceRect2, (float) num19, num20 != 0);
      animals2.Add(textureComponent2);
      List<ClickableTextureComponent> animals3 = this.animals;
      string name3 = "";
      Rectangle bounds3 = new Rectangle(x + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2, y + IClickableMenu.spaceToClearTopBorder + Game1.tileSize + 2 * (Game1.tileSize / 2 + 4), Game1.tileSize / 2 + 8, Game1.tileSize / 2);
      string label3 = string.Concat((object) num3);
      string str5 = "Rabbits";
      string str6;
      if (num3 <= 0)
        str6 = "";
      else
        str6 = Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:FarmInfoPage.cs.10425", (object) (num10 / num3));
      string hoverText3 = str5 + str6;
      Texture2D mouseCursors3 = Game1.mouseCursors;
      Rectangle sourceRect3 = new Rectangle(Game1.tileSize * 4, Game1.tileSize + Game1.tileSize / 2, Game1.tileSize / 2, Game1.tileSize / 2);
      double num21 = 1.0;
      int num22 = 0;
      ClickableTextureComponent textureComponent3 = new ClickableTextureComponent(name3, bounds3, label3, hoverText3, mouseCursors3, sourceRect3, (float) num21, num22 != 0);
      animals3.Add(textureComponent3);
      List<ClickableTextureComponent> animals4 = this.animals;
      string name4 = "";
      Rectangle bounds4 = new Rectangle(x + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2, y + IClickableMenu.spaceToClearTopBorder + Game1.tileSize + 3 * (Game1.tileSize / 2 + 4), Game1.tileSize / 2 + 8, Game1.tileSize / 2);
      string label4 = string.Concat((object) num5);
      string str7 = "Cows";
      string str8;
      if (num5 <= 0)
        str8 = "";
      else
        str8 = Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:FarmInfoPage.cs.10425", (object) (num13 / num5));
      string hoverText4 = str7 + str8;
      Texture2D mouseCursors4 = Game1.mouseCursors;
      Rectangle sourceRect4 = new Rectangle(Game1.tileSize * 5, Game1.tileSize, Game1.tileSize / 2, Game1.tileSize / 2);
      double num23 = 1.0;
      int num24 = 0;
      ClickableTextureComponent textureComponent4 = new ClickableTextureComponent(name4, bounds4, label4, hoverText4, mouseCursors4, sourceRect4, (float) num23, num24 != 0);
      animals4.Add(textureComponent4);
      List<ClickableTextureComponent> animals5 = this.animals;
      string name5 = "";
      Rectangle bounds5 = new Rectangle(x + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2, y + IClickableMenu.spaceToClearTopBorder + Game1.tileSize + 4 * (Game1.tileSize / 2 + 4), Game1.tileSize / 2 + 8, Game1.tileSize / 2);
      string label5 = string.Concat((object) num7);
      string str9 = "Goats";
      string str10;
      if (num7 <= 0)
        str10 = "";
      else
        str10 = Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:FarmInfoPage.cs.10425", (object) (num14 / num7));
      string hoverText5 = str9 + str10;
      Texture2D mouseCursors5 = Game1.mouseCursors;
      Rectangle sourceRect5 = new Rectangle(Game1.tileSize * 5 + Game1.tileSize / 2, Game1.tileSize, Game1.tileSize / 2, Game1.tileSize / 2);
      double num25 = 1.0;
      int num26 = 0;
      ClickableTextureComponent textureComponent5 = new ClickableTextureComponent(name5, bounds5, label5, hoverText5, mouseCursors5, sourceRect5, (float) num25, num26 != 0);
      animals5.Add(textureComponent5);
      List<ClickableTextureComponent> animals6 = this.animals;
      string name6 = "";
      Rectangle bounds6 = new Rectangle(x + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2, y + IClickableMenu.spaceToClearTopBorder + Game1.tileSize + 5 * (Game1.tileSize / 2 + 4), Game1.tileSize / 2 + 8, Game1.tileSize / 2);
      string label6 = string.Concat((object) num6);
      string str11 = "Sheep";
      string str12;
      if (num6 <= 0)
        str12 = "";
      else
        str12 = Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:FarmInfoPage.cs.10425", (object) (num15 / num6));
      string hoverText6 = str11 + str12;
      Texture2D mouseCursors6 = Game1.mouseCursors;
      Rectangle sourceRect6 = new Rectangle(Game1.tileSize * 5 + Game1.tileSize / 2, Game1.tileSize + Game1.tileSize / 2, Game1.tileSize / 2, Game1.tileSize / 2);
      double num27 = 1.0;
      int num28 = 0;
      ClickableTextureComponent textureComponent6 = new ClickableTextureComponent(name6, bounds6, label6, hoverText6, mouseCursors6, sourceRect6, (float) num27, num28 != 0);
      animals6.Add(textureComponent6);
      List<ClickableTextureComponent> animals7 = this.animals;
      string name7 = "";
      Rectangle bounds7 = new Rectangle(x + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2, y + IClickableMenu.spaceToClearTopBorder + Game1.tileSize + 6 * (Game1.tileSize / 2 + 4), Game1.tileSize / 2 + 8, Game1.tileSize / 2);
      string label7 = string.Concat((object) num8);
      string str13 = "Pigs";
      string str14;
      if (num8 <= 0)
        str14 = "";
      else
        str14 = Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:FarmInfoPage.cs.10425", (object) (num16 / num8));
      string hoverText7 = str13 + str14;
      Texture2D mouseCursors7 = Game1.mouseCursors;
      Rectangle sourceRect7 = new Rectangle(Game1.tileSize * 5, Game1.tileSize + Game1.tileSize / 2, Game1.tileSize / 2, Game1.tileSize / 2);
      double num29 = 1.0;
      int num30 = 0;
      ClickableTextureComponent textureComponent7 = new ClickableTextureComponent(name7, bounds7, label7, hoverText7, mouseCursors7, sourceRect7, (float) num29, num30 != 0);
      animals7.Add(textureComponent7);
      List<ClickableTextureComponent> animals8 = this.animals;
      string name8 = "";
      Rectangle bounds8 = new Rectangle(x + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2, y + IClickableMenu.spaceToClearTopBorder + Game1.tileSize + 7 * (Game1.tileSize / 2 + 4), Game1.tileSize / 2 + 8, Game1.tileSize / 2);
      string label8 = string.Concat((object) num4);
      string str15 = "???";
      string str16;
      if (num4 <= 0)
        str16 = "";
      else
        str16 = Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:FarmInfoPage.cs.10425", (object) (num12 / num4));
      string hoverText8 = str15 + str16;
      Texture2D mouseCursors8 = Game1.mouseCursors;
      Rectangle sourceRect8 = new Rectangle(Game1.tileSize * 4 + Game1.tileSize / 2, Game1.tileSize + Game1.tileSize / 2, Game1.tileSize / 2, Game1.tileSize / 2);
      double num31 = 1.0;
      int num32 = 0;
      ClickableTextureComponent textureComponent8 = new ClickableTextureComponent(name8, bounds8, label8, hoverText8, mouseCursors8, sourceRect8, (float) num31, num32 != 0);
      animals8.Add(textureComponent8);
      this.animals.Add(new ClickableTextureComponent("", new Rectangle(x + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2, y + IClickableMenu.spaceToClearTopBorder + Game1.tileSize + 8 * (Game1.tileSize / 2 + 4), Game1.tileSize / 2 + 8, Game1.tileSize / 2), string.Concat((object) Game1.stats.CropsShipped), Game1.content.LoadString("Strings\\StringsFromCSFiles:FarmInfoPage.cs.10440"), Game1.mouseCursors, new Rectangle(Game1.tileSize * 7 + Game1.tileSize / 2, Game1.tileSize, Game1.tileSize / 2, Game1.tileSize / 2), 1f, false));
      this.animals.Add(new ClickableTextureComponent("", new Rectangle(x + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2, y + IClickableMenu.spaceToClearTopBorder + Game1.tileSize + 9 * (Game1.tileSize / 2 + 4), Game1.tileSize / 2 + 8, Game1.tileSize / 2), string.Concat((object) this.farm.buildings.Count<Building>()), Game1.content.LoadString("Strings\\StringsFromCSFiles:FarmInfoPage.cs.10441"), Game1.mouseCursors, new Rectangle(Game1.tileSize * 7, Game1.tileSize, Game1.tileSize / 2, Game1.tileSize / 2), 1f, false));
      int num33 = 8;
      foreach (Building building in this.farm.buildings)
        this.mapBuildings.Add(new ClickableTextureComponent("", new Rectangle(this.mapX + building.tileX * num33, this.mapY + building.tileY * num33 + (building.tilesHigh + 1) * num33 - (int) ((double) building.texture.Height / 8.0), building.tilesWide * num33, (int) ((double) building.texture.Height / 8.0)), "", building.buildingType, building.texture, building.getSourceRectForMenu(), 0.125f, false));
      foreach (KeyValuePair<Vector2, TerrainFeature> terrainFeature in (Dictionary<Vector2, TerrainFeature>) this.farm.terrainFeatures)
        this.mapFeatures.Add(new MiniatureTerrainFeature(terrainFeature.Value, new Vector2(terrainFeature.Key.X * (float) num33 + (float) this.mapX, terrainFeature.Key.Y * (float) num33 + (float) this.mapY), terrainFeature.Key, 0.125f));
      if (!(Game1.currentLocation.GetType() == typeof (Farm)))
        return;
      this.mapFarmer = new ClickableTextureComponent("", new Rectangle(this.mapX + (int) ((double) Game1.player.Position.X / 8.0), this.mapY + (int) ((double) Game1.player.position.Y / 8.0), 8, 12), "", Game1.player.name, (Texture2D) null, new Rectangle(0, 0, Game1.tileSize, Game1.tileSize * 3 / 2), 0.125f, false);
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
    }

    public override void performHoverAction(int x, int y)
    {
      this.descriptionText = "";
      this.hoverText = "";
      foreach (ClickableTextureComponent animal in this.animals)
      {
        if (animal.containsPoint(x, y))
        {
          this.hoverText = animal.hoverText;
          return;
        }
      }
      foreach (ClickableTextureComponent mapBuilding in this.mapBuildings)
      {
        if (mapBuilding.containsPoint(x, y))
        {
          this.hoverText = mapBuilding.hoverText;
          return;
        }
      }
      if (this.mapFarmer == null || !this.mapFarmer.containsPoint(x, y))
        return;
      this.hoverText = this.mapFarmer.hoverText;
    }

    public override void draw(SpriteBatch b)
    {
      this.drawVerticalPartition(b, this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize * 2, false);
      this.moneyIcon.draw(b);
      foreach (ClickableTextureComponent animal in this.animals)
        animal.draw(b);
      this.farmMap.draw(b);
      foreach (ClickableTextureComponent mapBuilding in this.mapBuildings)
        mapBuilding.draw(b);
      b.End();
      b.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
      this.farmMap.draw(b);
      foreach (ClickableTextureComponent mapBuilding in this.mapBuildings)
        mapBuilding.draw(b);
      foreach (MiniatureTerrainFeature mapFeature in this.mapFeatures)
        mapFeature.draw(b);
      this.farmHouse.draw(b);
      if (this.mapFarmer != null)
        Game1.player.FarmerRenderer.drawMiniPortrat(b, new Vector2((float) (this.mapFarmer.bounds.X - 16), (float) (this.mapFarmer.bounds.Y - 16)), 0.99f, 2f, 2, Game1.player);
      foreach (KeyValuePair<long, FarmAnimal> animal in (Dictionary<long, FarmAnimal>) this.farm.animals)
        b.Draw(animal.Value.sprite.Texture, new Vector2((float) (this.mapX + (int) ((double) animal.Value.position.X / 8.0)), (float) (this.mapY + (int) ((double) animal.Value.position.Y / 8.0))), new Rectangle?(animal.Value.sprite.SourceRect), Color.White, 0.0f, Vector2.Zero, 0.125f, SpriteEffects.None, (float) (0.860000014305115 + (double) animal.Value.position.Y / 8.0 / 20000.0 + 0.0125000001862645));
      foreach (KeyValuePair<Vector2, StardewValley.Object> keyValuePair in (Dictionary<Vector2, StardewValley.Object>) this.farm.objects)
        keyValuePair.Value.drawInMenu(b, new Vector2((float) this.mapX + keyValuePair.Key.X * 8f, (float) this.mapY + keyValuePair.Key.Y * 8f), 0.125f, 1f, (float) (0.860000014305115 + ((double) this.mapY + (double) keyValuePair.Key.Y * 8.0 - 25.0) / 20000.0));
      b.End();
      b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
      if (this.hoverText.Equals(""))
        return;
      IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, -1, (string) null, -1, (string[]) null, (Item) null, 0, -1, -1, -1, -1, 1f, (CraftingRecipe) null);
    }
  }
}
