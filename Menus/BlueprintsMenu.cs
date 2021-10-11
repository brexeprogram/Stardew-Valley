// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.BlueprintsMenu
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Buildings;
using StardewValley.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using xTile;
using xTile.Dimensions;

namespace StardewValley.Menus
{
  public class BlueprintsMenu : IClickableMenu
  {
    public static int heightOfDescriptionBox = Game1.tileSize * 6;
    public static int blueprintButtonMargin = Game1.tileSize / 2;
    public new static int tabYPositionRelativeToMenuY = -Game1.tileSize * 3 / 4;
    private string hoverText = "";
    private List<Dictionary<ClickableComponent, BluePrint>> blueprintButtons = new List<Dictionary<ClickableComponent, BluePrint>>();
    private List<ClickableComponent> tabs = new List<ClickableComponent>();
    public const int buildingsTab = 0;
    public const int upgradesTab = 1;
    public const int decorationsTab = 2;
    public const int demolishTab = 3;
    public const int animalsTab = 4;
    public const int numberOfTabs = 5;
    private bool placingStructure;
    private bool demolishing;
    private bool upgrading;
    private bool queryingAnimals;
    private int currentTab;
    private Vector2 positionOfAnimalWhenClicked;
    private BluePrint hoveredItem;
    private BluePrint structureForPlacement;
    private FarmAnimal currentAnimal;
    private Texture2D buildingPlacementTiles;

    public BlueprintsMenu(int x, int y)
      : base(x, y, Game1.viewport.Width / 2 + Game1.tileSize * 3 / 2, 0, false)
    {
      BlueprintsMenu.tabYPositionRelativeToMenuY = -Game1.tileSize * 3 / 4;
      BlueprintsMenu.blueprintButtonMargin = Game1.tileSize / 2;
      BlueprintsMenu.heightOfDescriptionBox = Game1.tileSize * 6;
      for (int index = 0; index < 5; ++index)
        this.blueprintButtons.Add(new Dictionary<ClickableComponent, BluePrint>());
      this.xPositionOnScreen = x;
      this.yPositionOnScreen = y;
      int[] numArray = new int[5];
      for (int index = 0; index < Game1.player.blueprints.Count; ++index)
      {
        BluePrint bluePrint = new BluePrint(Game1.player.blueprints[index]);
        int tabNumberFromName = this.getTabNumberFromName(bluePrint.blueprintType);
        if (bluePrint.blueprintType != null)
        {
          int width = (int) ((double) Math.Max(bluePrint.tilesWidth, 4) / 4.0 * (double) Game1.tileSize) + BlueprintsMenu.blueprintButtonMargin;
          if (numArray[tabNumberFromName] % (this.width - IClickableMenu.borderWidth * 2) + width > this.width - IClickableMenu.borderWidth * 2)
            numArray[tabNumberFromName] += this.width - IClickableMenu.borderWidth * 2 - numArray[tabNumberFromName] % (this.width - IClickableMenu.borderWidth * 2);
          this.blueprintButtons[Math.Min(4, tabNumberFromName)].Add(new ClickableComponent(new Microsoft.Xna.Framework.Rectangle(x + IClickableMenu.borderWidth + numArray[tabNumberFromName] % (this.width - IClickableMenu.borderWidth * 2), y + IClickableMenu.borderWidth + numArray[tabNumberFromName] / (this.width - IClickableMenu.borderWidth * 2) * Game1.tileSize * 2 + Game1.tileSize, width, Game1.tileSize * 2), bluePrint.name), bluePrint);
          numArray[tabNumberFromName] += width;
        }
      }
      this.blueprintButtons[4].Add(new ClickableComponent(new Microsoft.Xna.Framework.Rectangle(x + IClickableMenu.borderWidth + numArray[4] % (this.width - IClickableMenu.borderWidth * 2), y + IClickableMenu.borderWidth + numArray[4] / (this.width - IClickableMenu.borderWidth * 2) * Game1.tileSize * 2 + Game1.tileSize, Game1.tileSize + BlueprintsMenu.blueprintButtonMargin, Game1.tileSize * 2), "Info Tool"), new BluePrint("Info Tool"));
      int num = 0;
      for (int index = 0; index < numArray.Length; ++index)
      {
        if (numArray[index] > num)
          num = numArray[index];
      }
      this.height = Game1.tileSize * 2 + num / (this.width - IClickableMenu.borderWidth * 2) * Game1.tileSize * 2 + IClickableMenu.borderWidth * 4 + BlueprintsMenu.heightOfDescriptionBox;
      this.buildingPlacementTiles = Game1.content.Load<Texture2D>("LooseSprites\\buildingPlacementTiles");
      this.tabs.Add(new ClickableComponent(new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + Game1.tileSize, this.yPositionOnScreen + BlueprintsMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "Buildings"));
      this.tabs.Add(new ClickableComponent(new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + Game1.tileSize + Game1.tileSize + 4, this.yPositionOnScreen + BlueprintsMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "Upgrades"));
      this.tabs.Add(new ClickableComponent(new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + Game1.tileSize + (Game1.tileSize + 4) * 2, this.yPositionOnScreen + BlueprintsMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "Decorations"));
      this.tabs.Add(new ClickableComponent(new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + Game1.tileSize + (Game1.tileSize + 4) * 3, this.yPositionOnScreen + BlueprintsMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "Demolish"));
      this.tabs.Add(new ClickableComponent(new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + Game1.tileSize + (Game1.tileSize + 4) * 4, this.yPositionOnScreen + BlueprintsMenu.tabYPositionRelativeToMenuY + Game1.tileSize, Game1.tileSize, Game1.tileSize), "Animals"));
    }

    public int getTabNumberFromName(string name)
    {
      int num = -1;
      if (!(name == "Buildings"))
      {
        if (!(name == "Upgrades"))
        {
          if (!(name == "Decorations"))
          {
            if (!(name == "Demolish"))
            {
              if (name == "Animals")
                num = 4;
            }
            else
              num = 3;
          }
          else
            num = 2;
        }
        else
          num = 1;
      }
      else
        num = 0;
      return num;
    }

    public void changePosition(int x, int y)
    {
      int num1 = this.xPositionOnScreen - x;
      int num2 = this.yPositionOnScreen - y;
      this.xPositionOnScreen = x;
      this.yPositionOnScreen = y;
      foreach (Dictionary<ClickableComponent, BluePrint> blueprintButton in this.blueprintButtons)
      {
        foreach (ClickableComponent key in blueprintButton.Keys)
        {
          key.bounds.X += num1;
          key.bounds.Y -= num2;
        }
      }
      foreach (ClickableComponent tab in this.tabs)
      {
        tab.bounds.X += num1;
        tab.bounds.Y -= num2;
      }
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
      if (this.currentAnimal != null)
      {
        this.currentAnimal = (FarmAnimal) null;
        this.placingStructure = true;
        this.queryingAnimals = true;
      }
      if (!this.placingStructure)
      {
        Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height);
        foreach (ClickableComponent key in this.blueprintButtons[this.currentTab].Keys)
        {
          if (key.containsPoint(x, y))
          {
            if (key.name.Equals("Info Tool"))
            {
              this.placingStructure = true;
              this.queryingAnimals = true;
              Game1.playSound("smallSelect");
              return;
            }
            if (this.blueprintButtons[this.currentTab][key].doesFarmerHaveEnoughResourcesToBuild())
            {
              this.structureForPlacement = this.blueprintButtons[this.currentTab][key];
              this.placingStructure = true;
              if (this.currentTab == 1)
                this.upgrading = true;
              Game1.playSound("smallSelect");
              return;
            }
            Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:BlueprintsMenu.cs.10002"), Color.Red, 3500f));
            return;
          }
        }
        foreach (ClickableComponent tab in this.tabs)
        {
          if (tab.containsPoint(x, y))
          {
            this.currentTab = this.getTabNumberFromName(tab.name);
            Game1.playSound("smallSelect");
            if (this.currentTab != 3)
              return;
            this.placingStructure = true;
            this.demolishing = true;
            return;
          }
        }
        if (rectangle.Contains(x, y))
          return;
        Game1.exitActiveMenu();
      }
      else if (this.demolishing)
      {
        Building buildingAt = ((BuildableGameLocation) Game1.getLocationFromName("Farm")).getBuildingAt(new Vector2((float) ((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float) ((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize)));
        if (buildingAt != null && ((BuildableGameLocation) Game1.getLocationFromName("Farm")).destroyStructure(buildingAt))
        {
          int num = buildingAt.tileY + buildingAt.tilesHigh;
          for (int index = 0; index < buildingAt.texture.Bounds.Height / Game1.tileSize; ++index)
          {
            GameLocation currentLocation = Game1.currentLocation;
            Texture2D texture = buildingAt.texture;
            Microsoft.Xna.Framework.Rectangle bounds = buildingAt.texture.Bounds;
            int x1 = bounds.Center.X;
            bounds = buildingAt.texture.Bounds;
            int y1 = bounds.Center.Y;
            int width = Game1.tileSize / 16;
            int height = Game1.tileSize / 16;
            Microsoft.Xna.Framework.Rectangle sourcerectangle = new Microsoft.Xna.Framework.Rectangle(x1, y1, width, height);
            int xTile = buildingAt.tileX + Game1.random.Next(buildingAt.tilesWide);
            int yTile = buildingAt.tileY + buildingAt.tilesHigh - index;
            int numberOfChunks = Game1.random.Next(20, 45);
            int groundLevelTile = num;
            Game1.createRadialDebris(currentLocation, texture, sourcerectangle, xTile, yTile, numberOfChunks, groundLevelTile);
          }
          Game1.playSound("explosion");
          Utility.spreadAnimalsAround(buildingAt, (Farm) Game1.getLocationFromName("Farm"));
        }
        else
          Game1.exitActiveMenu();
      }
      else if (this.upgrading && Game1.currentLocation.GetType() == typeof (Farm))
      {
        Building buildingAt = ((BuildableGameLocation) Game1.getLocationFromName("Farm")).getBuildingAt(new Vector2((float) ((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float) ((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize)));
        if (buildingAt != null && this.structureForPlacement.name != null && buildingAt.buildingType.Equals(this.structureForPlacement.nameOfBuildingToUpgrade))
        {
          buildingAt.indoors.map = Game1.game1.xTileContent.Load<Map>("Maps\\" + this.structureForPlacement.mapToWarpTo);
          buildingAt.indoors.name = this.structureForPlacement.mapToWarpTo;
          buildingAt.buildingType = this.structureForPlacement.name;
          buildingAt.texture = this.structureForPlacement.texture;
          if (buildingAt.indoors.GetType() == typeof (AnimalHouse))
            ((AnimalHouse) buildingAt.indoors).resetPositionsOfAllAnimals();
          Game1.playSound("axe");
          this.structureForPlacement.consumeResources();
          buildingAt.color = Color.White;
          Game1.exitActiveMenu();
        }
        else if (buildingAt != null)
          Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:BlueprintsMenu.cs.10011"), Color.Red, 3500f));
        else
          Game1.exitActiveMenu();
      }
      else if (this.queryingAnimals)
      {
        if (!(Game1.currentLocation.GetType() == typeof (Farm)) && !(Game1.currentLocation.GetType() == typeof (AnimalHouse)))
          return;
        foreach (FarmAnimal farmAnimal in Game1.currentLocation.GetType() == typeof (Farm) ? ((Farm) Game1.currentLocation).animals.Values.ToList<FarmAnimal>() : ((AnimalHouse) Game1.currentLocation).animals.Values.ToList<FarmAnimal>())
        {
          if (new Microsoft.Xna.Framework.Rectangle((int) farmAnimal.position.X, (int) farmAnimal.position.Y, farmAnimal.sprite.SourceRect.Width, farmAnimal.sprite.SourceRect.Height).Contains(Game1.viewport.X + Game1.getOldMouseX(), Game1.viewport.Y + Game1.getOldMouseY()))
          {
            this.positionOfAnimalWhenClicked = Game1.GlobalToLocal(Game1.viewport, farmAnimal.position);
            this.currentAnimal = farmAnimal;
            this.queryingAnimals = false;
            this.placingStructure = false;
            if (farmAnimal.sound == null || farmAnimal.sound.Equals(""))
              break;
            Game1.playSound(farmAnimal.sound);
            break;
          }
        }
      }
      else if (Game1.currentLocation.GetType() != typeof (Farm))
        Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:BlueprintsMenu.cs.10012"), Color.Red, 3500f));
      else if (!this.structureForPlacement.doesFarmerHaveEnoughResourcesToBuild())
        Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:BlueprintsMenu.cs.10002"), Color.Red, 3500f));
      else if (this.tryToBuild())
      {
        this.structureForPlacement.consumeResources();
        if (this.structureForPlacement.blueprintType.Equals("Animals"))
          return;
        Game1.playSound("axe");
      }
      else
        Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:BlueprintsMenu.cs.10016"), Color.Red, 3500f));
    }

    public bool tryToBuild()
    {
      if (this.structureForPlacement.blueprintType.Equals("Animals"))
        return ((Farm) Game1.getLocationFromName("Farm")).placeAnimal(this.structureForPlacement, new Vector2((float) ((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float) ((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize)), false, Game1.player.uniqueMultiplayerID);
      return ((BuildableGameLocation) Game1.getLocationFromName("Farm")).buildStructure(this.structureForPlacement, new Vector2((float) ((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float) ((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize)), false, Game1.player, false);
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
      if (this.currentAnimal != null)
      {
        this.currentAnimal = (FarmAnimal) null;
        this.queryingAnimals = true;
        this.placingStructure = true;
      }
      else if (this.placingStructure)
      {
        this.placingStructure = false;
        this.queryingAnimals = false;
        this.upgrading = false;
        this.demolishing = false;
      }
      else
        Game1.exitActiveMenu();
    }

    public override void performHoverAction(int x, int y)
    {
      if (this.demolishing)
      {
        foreach (Building building in ((BuildableGameLocation) Game1.getLocationFromName("Farm")).buildings)
          building.color = Color.White;
        Building buildingAt = ((BuildableGameLocation) Game1.getLocationFromName("Farm")).getBuildingAt(new Vector2((float) ((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float) ((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize)));
        if (buildingAt == null)
          return;
        buildingAt.color = Color.Red * 0.8f;
      }
      else if (this.upgrading)
      {
        foreach (Building building in ((BuildableGameLocation) Game1.getLocationFromName("Farm")).buildings)
          building.color = Color.White;
        Building buildingAt = ((BuildableGameLocation) Game1.getLocationFromName("Farm")).getBuildingAt(new Vector2((float) ((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float) ((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize)));
        if (buildingAt != null && this.structureForPlacement.nameOfBuildingToUpgrade != null && this.structureForPlacement.nameOfBuildingToUpgrade.Equals(buildingAt.buildingType))
        {
          buildingAt.color = Color.Green * 0.8f;
        }
        else
        {
          if (buildingAt == null)
            return;
          buildingAt.color = Color.Red * 0.8f;
        }
      }
      else
      {
        if (this.placingStructure)
          return;
        foreach (ClickableComponent tab in this.tabs)
        {
          if (tab.containsPoint(x, y))
          {
            this.hoverText = tab.name;
            return;
          }
        }
        this.hoverText = "";
        bool flag = false;
        foreach (ClickableComponent key in this.blueprintButtons[this.currentTab].Keys)
        {
          if (key.containsPoint(x, y))
          {
            key.scale = Math.Min(key.scale + 0.01f, 1.1f);
            this.hoveredItem = this.blueprintButtons[this.currentTab][key];
            flag = true;
          }
          else
            key.scale = Math.Max(key.scale - 0.01f, 1f);
        }
        if (flag)
          return;
        this.hoveredItem = (BluePrint) null;
      }
    }

    public int getTileSheetIndexForStructurePlacementTile(int x, int y)
    {
      if (x == this.structureForPlacement.humanDoor.X && y == this.structureForPlacement.humanDoor.Y)
        return 2;
      return x == this.structureForPlacement.animalDoor.X && y == this.structureForPlacement.animalDoor.Y ? 4 : 0;
    }

    public override void draw(SpriteBatch b)
    {
      if (this.currentAnimal != null)
      {
        int x = (int) Math.Max(0.0f, Math.Min(this.positionOfAnimalWhenClicked.X - (float) (Game1.tileSize * 4) + (float) (Game1.tileSize / 2), (float) (Game1.viewport.Width - Game1.tileSize * 8)));
        int y = (int) Math.Max(0.0f, Math.Min((float) (Game1.viewport.Height - Game1.tileSize * 4 - this.currentAnimal.frontBackSourceRect.Height), this.positionOfAnimalWhenClicked.Y - (float) (Game1.tileSize * 4) - (float) this.currentAnimal.frontBackSourceRect.Height));
        Game1.drawDialogueBox(x, y, Game1.tileSize * 8, Game1.tileSize * 5 + Game1.tileSize / 2, false, true, (string) null, false);
        b.Draw(this.currentAnimal.sprite.Texture, new Vector2((float) (x + IClickableMenu.borderWidth + Game1.tileSize * 3 / 2 - this.currentAnimal.frontBackSourceRect.Width / 2), (float) (y + IClickableMenu.borderWidth + Game1.tileSize * 3 / 2)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, this.currentAnimal.frontBackSourceRect.Width, this.currentAnimal.frontBackSourceRect.Height)), Color.White);
        float num1 = (float) this.currentAnimal.fullness / (float) byte.MaxValue;
        float num2 = (float) this.currentAnimal.happiness / (float) byte.MaxValue;
        string text1 = Game1.content.LoadString("Strings\\StringsFromCSFiles:BlueprintsMenu.cs.10026");
        string text2 = Game1.content.LoadString("Strings\\StringsFromCSFiles:BlueprintsMenu.cs.10027");
        b.DrawString(Game1.dialogueFont, this.currentAnimal.displayName, new Vector2((float) (x + IClickableMenu.borderWidth + Game1.tileSize * 3 / 2) - Game1.dialogueFont.MeasureString(this.currentAnimal.name).X / 2f, (float) (y + IClickableMenu.borderWidth + Game1.tileSize * 3 / 2 + this.currentAnimal.frontBackSourceRect.Height + Game1.tileSize / 8)), Game1.textColor);
        b.DrawString(Game1.dialogueFont, text1, new Vector2((float) (x + IClickableMenu.borderWidth + Game1.tileSize * 3), (float) (y + IClickableMenu.borderWidth + Game1.tileSize * 3 / 2)), Game1.textColor);
        b.Draw(Game1.fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle(x + IClickableMenu.borderWidth + Game1.tileSize * 3, y + IClickableMenu.borderWidth + Game1.tileSize * 3 / 2 + (int) Game1.dialogueFont.MeasureString(text1).Y + Game1.tileSize / 8, Game1.tileSize * 3, Game1.tileSize / 4), Color.Gray);
        b.Draw(Game1.fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle(x + IClickableMenu.borderWidth + Game1.tileSize * 3, y + IClickableMenu.borderWidth + Game1.tileSize * 3 / 2 + (int) Game1.dialogueFont.MeasureString(text1).Y + Game1.tileSize / 8, (int) ((double) (Game1.tileSize * 3) * (double) num1), Game1.tileSize / 4), (double) num1 > 0.33 ? ((double) num1 > 0.66 ? Color.Green : Color.Goldenrod) : Color.Red);
        b.DrawString(Game1.dialogueFont, text2, new Vector2((float) (x + IClickableMenu.borderWidth + Game1.tileSize * 3), (float) (y + IClickableMenu.borderWidth + Game1.tileSize * 3 / 2) + Game1.dialogueFont.MeasureString(text1).Y + (float) (Game1.tileSize / 2)), Game1.textColor);
        b.Draw(Game1.fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle(x + IClickableMenu.borderWidth + Game1.tileSize * 3, y + IClickableMenu.borderWidth + Game1.tileSize * 3 / 2 + (int) Game1.dialogueFont.MeasureString(text1).Y + (int) Game1.dialogueFont.MeasureString(text2).Y + Game1.tileSize / 2, Game1.tileSize * 3, Game1.tileSize / 4), Color.Gray);
        b.Draw(Game1.fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle(x + IClickableMenu.borderWidth + Game1.tileSize * 3, y + IClickableMenu.borderWidth + Game1.tileSize * 3 / 2 + (int) Game1.dialogueFont.MeasureString(text1).Y + (int) Game1.dialogueFont.MeasureString(text2).Y + Game1.tileSize / 2, (int) ((double) (Game1.tileSize * 3) * (double) num2), Game1.tileSize / 4), (double) num2 > 0.33 ? ((double) num2 > 0.66 ? Color.Green : Color.Goldenrod) : Color.Red);
      }
      else if (!this.placingStructure)
      {
        b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.4f);
        Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height - BlueprintsMenu.heightOfDescriptionBox, false, true, (string) null, false);
        foreach (ClickableComponent tab in this.tabs)
        {
          int tilePosition = 0;
          string name = tab.name;
          if (!(name == "Buildings"))
          {
            if (!(name == "Upgrades"))
            {
              if (!(name == "Decorations"))
              {
                if (!(name == "Demolish"))
                {
                  if (name == "Animals")
                    tilePosition = 8;
                }
                else
                  tilePosition = 6;
              }
              else
                tilePosition = 7;
            }
            else
              tilePosition = 5;
          }
          else
            tilePosition = 4;
          b.Draw(Game1.mouseCursors, new Vector2((float) tab.bounds.X, (float) (tab.bounds.Y + (this.currentTab == this.getTabNumberFromName(tab.name) ? 8 : 0))), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, tilePosition, -1, -1)), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0001f);
        }
        foreach (ClickableComponent key in this.blueprintButtons[this.currentTab].Keys)
        {
          Texture2D texture = this.blueprintButtons[this.currentTab][key].texture;
          Vector2 origin = key.name.Equals("Info Tool") ? new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2)) : new Vector2((float) this.blueprintButtons[this.currentTab][key].sourceRectForMenuView.Center.X, (float) this.blueprintButtons[this.currentTab][key].sourceRectForMenuView.Center.Y);
          b.Draw(texture, new Vector2((float) key.bounds.Center.X, (float) key.bounds.Center.Y), new Microsoft.Xna.Framework.Rectangle?(this.blueprintButtons[this.currentTab][key].sourceRectForMenuView), Color.White, 0.0f, origin, (float) (0.25 * (double) key.scale + (this.currentTab == 4 ? 0.75 : 0.0)), SpriteEffects.None, 0.9f);
        }
        Game1.drawWithBorder(this.hoverText, Color.Black, Color.White, new Vector2((float) (Game1.getOldMouseX() + Game1.tileSize), (float) (Game1.getOldMouseY() + Game1.tileSize)), 0.0f, 1f, 1f);
        Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen + (this.height - BlueprintsMenu.heightOfDescriptionBox) - IClickableMenu.borderWidth * 2, this.width, BlueprintsMenu.heightOfDescriptionBox, false, true, (string) null, false);
        if (this.hoveredItem == null)
          ;
      }
      else if (!this.demolishing && !this.upgrading && !this.queryingAnimals)
      {
        Vector2 vector2_1 = new Vector2((float) ((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float) ((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize));
        for (int y = 0; y < this.structureForPlacement.tilesHeight; ++y)
        {
          for (int x = 0; x < this.structureForPlacement.tilesWidth; ++x)
          {
            int structurePlacementTile = this.getTileSheetIndexForStructurePlacementTile(x, y);
            Vector2 vector2_2 = new Vector2(vector2_1.X + (float) x, vector2_1.Y + (float) y);
            if (Game1.player.getTileLocation().Equals(vector2_2) || Game1.currentLocation.isTileOccupied(vector2_2, "") || !Game1.currentLocation.isTilePassable(new Location((int) vector2_2.X, (int) vector2_2.Y), Game1.viewport))
              ++structurePlacementTile;
            b.Draw(this.buildingPlacementTiles, Game1.GlobalToLocal(Game1.viewport, vector2_2 * (float) Game1.tileSize), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(this.buildingPlacementTiles, structurePlacementTile, -1, -1)), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.999f);
          }
        }
      }
      b.Draw(Game1.mouseCursors, new Vector2((float) Game1.getOldMouseX(), (float) Game1.getOldMouseY()), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, this.queryingAnimals || this.currentAnimal != null ? 9 : 0, -1, -1)), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
    }
  }
}
