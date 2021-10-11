﻿// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.CarpenterMenu
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Buildings;
using StardewValley.Locations;
using System.Collections.Generic;
using System.Linq;
using xTile.Dimensions;

namespace StardewValley.Menus
{
  public class CarpenterMenu : IClickableMenu
  {
    public int maxWidthOfBuildingViewer = 7 * Game1.tileSize;
    public int maxHeightOfBuildingViewer = 8 * Game1.tileSize;
    public int maxWidthOfDescription = 6 * Game1.tileSize;
    private List<Item> ingredients = new List<Item>();
    private bool drawBG = true;
    private string hoverText = "";
    public const int region_backButton = 101;
    public const int region_forwardButton = 102;
    public const int region_upgradeIcon = 103;
    public const int region_demolishButton = 104;
    public const int region_moveBuitton = 105;
    public const int region_okButton = 106;
    public const int region_cancelButton = 107;
    private List<BluePrint> blueprints;
    private int currentBlueprintIndex;
    public ClickableTextureComponent okButton;
    public ClickableTextureComponent cancelButton;
    public ClickableTextureComponent backButton;
    public ClickableTextureComponent forwardButton;
    public ClickableTextureComponent upgradeIcon;
    public ClickableTextureComponent demolishButton;
    public ClickableTextureComponent moveButton;
    private Building currentBuilding;
    private Building buildingToMove;
    private string buildingDescription;
    private string buildingName;
    private int price;
    private bool onFarm;
    private bool freeze;
    private bool upgrading;
    private bool demolishing;
    private bool moving;
    private bool magicalConstruction;

    public BluePrint CurrentBlueprint
    {
      get
      {
        return this.blueprints[this.currentBlueprintIndex];
      }
    }

    public CarpenterMenu(bool magicalConstruction = false)
    {
      this.magicalConstruction = magicalConstruction;
      Game1.player.forceCanMove();
      this.resetBounds();
      this.blueprints = new List<BluePrint>();
      if (magicalConstruction)
      {
        this.blueprints.Add(new BluePrint("Junimo Hut"));
        this.blueprints.Add(new BluePrint("Earth Obelisk"));
        this.blueprints.Add(new BluePrint("Water Obelisk"));
        this.blueprints.Add(new BluePrint("Gold Clock"));
      }
      else
      {
        this.blueprints.Add(new BluePrint("Coop"));
        this.blueprints.Add(new BluePrint("Barn"));
        this.blueprints.Add(new BluePrint("Well"));
        this.blueprints.Add(new BluePrint("Silo"));
        this.blueprints.Add(new BluePrint("Mill"));
        this.blueprints.Add(new BluePrint("Shed"));
        if (!Game1.getFarm().isBuildingConstructed("Stable"))
          this.blueprints.Add(new BluePrint("Stable"));
        this.blueprints.Add(new BluePrint("Slime Hutch"));
        if (Game1.getFarm().isBuildingConstructed("Coop"))
          this.blueprints.Add(new BluePrint("Big Coop"));
        if (Game1.getFarm().isBuildingConstructed("Big Coop"))
          this.blueprints.Add(new BluePrint("Deluxe Coop"));
        if (Game1.getFarm().isBuildingConstructed("Barn"))
          this.blueprints.Add(new BluePrint("Big Barn"));
        if (Game1.getFarm().isBuildingConstructed("Big Barn"))
          this.blueprints.Add(new BluePrint("Deluxe Barn"));
      }
      this.setNewActiveBlueprint();
      if (!Game1.options.SnappyMenus)
        return;
      this.populateClickableComponentList();
      this.snapToDefaultClickableComponent();
    }

    public override void snapToDefaultClickableComponent()
    {
      this.currentlySnappedComponent = this.getComponentWithID(107);
      this.snapCursorToCurrentSnappedComponent();
    }

    private void resetBounds()
    {
      this.xPositionOnScreen = Game1.viewport.Width / 2 - this.maxWidthOfBuildingViewer - IClickableMenu.spaceToClearSideBorder;
      this.yPositionOnScreen = Game1.viewport.Height / 2 - this.maxHeightOfBuildingViewer / 2 - IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 2;
      this.width = this.maxWidthOfBuildingViewer + this.maxWidthOfDescription + IClickableMenu.spaceToClearSideBorder * 2 + Game1.tileSize;
      this.height = this.maxHeightOfBuildingViewer + IClickableMenu.spaceToClearTopBorder;
      this.initialize(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, true);
      ClickableTextureComponent textureComponent1 = new ClickableTextureComponent("OK", new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + this.width - IClickableMenu.borderWidth - IClickableMenu.spaceToClearSideBorder - Game1.tileSize * 3 - Game1.pixelZoom * 3, this.yPositionOnScreen + this.maxHeightOfBuildingViewer + Game1.tileSize, Game1.tileSize, Game1.tileSize), (string) null, (string) null, Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(366, 373, 16, 16), (float) Game1.pixelZoom, false);
      int num1 = 106;
      textureComponent1.myID = num1;
      int num2 = 104;
      textureComponent1.rightNeighborID = num2;
      int num3 = 105;
      textureComponent1.leftNeighborID = num3;
      this.okButton = textureComponent1;
      ClickableTextureComponent textureComponent2 = new ClickableTextureComponent("OK", new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + this.width - IClickableMenu.borderWidth - IClickableMenu.spaceToClearSideBorder - Game1.tileSize, this.yPositionOnScreen + this.maxHeightOfBuildingViewer + Game1.tileSize, Game1.tileSize, Game1.tileSize), (string) null, (string) null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 47, -1, -1), 1f, false);
      int num4 = 107;
      textureComponent2.myID = num4;
      int num5 = 104;
      textureComponent2.leftNeighborID = num5;
      this.cancelButton = textureComponent2;
      ClickableTextureComponent textureComponent3 = new ClickableTextureComponent(new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + Game1.tileSize, this.yPositionOnScreen + this.maxHeightOfBuildingViewer + Game1.tileSize, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(352, 495, 12, 11), (float) Game1.pixelZoom, false);
      int num6 = 101;
      textureComponent3.myID = num6;
      int num7 = 102;
      textureComponent3.rightNeighborID = num7;
      this.backButton = textureComponent3;
      ClickableTextureComponent textureComponent4 = new ClickableTextureComponent(new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + this.maxWidthOfBuildingViewer - Game1.tileSize * 4 + Game1.tileSize / 4, this.yPositionOnScreen + this.maxHeightOfBuildingViewer + Game1.tileSize, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(365, 495, 12, 11), (float) Game1.pixelZoom, false);
      int num8 = 102;
      textureComponent4.myID = num8;
      int num9 = 101;
      textureComponent4.leftNeighborID = num9;
      int num10 = 105;
      textureComponent4.rightNeighborID = num10;
      this.forwardButton = textureComponent4;
      ClickableTextureComponent textureComponent5 = new ClickableTextureComponent(Game1.content.LoadString("Strings\\UI:Carpenter_Demolish"), new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + this.width - IClickableMenu.borderWidth - IClickableMenu.spaceToClearSideBorder - Game1.tileSize * 2 - Game1.pixelZoom * 2, this.yPositionOnScreen + this.maxHeightOfBuildingViewer + Game1.tileSize - Game1.pixelZoom, Game1.tileSize, Game1.tileSize), (string) null, (string) null, Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(348, 372, 17, 17), (float) Game1.pixelZoom, false);
      int num11 = 104;
      textureComponent5.myID = num11;
      int num12 = 107;
      textureComponent5.rightNeighborID = num12;
      int num13 = 106;
      textureComponent5.leftNeighborID = num13;
      this.demolishButton = textureComponent5;
      ClickableTextureComponent textureComponent6 = new ClickableTextureComponent(new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + this.maxWidthOfBuildingViewer - Game1.tileSize * 2 + Game1.tileSize / 2, this.yPositionOnScreen + Game1.pixelZoom * 2, 9 * Game1.pixelZoom, 13 * Game1.pixelZoom), Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(402, 328, 9, 13), (float) Game1.pixelZoom, false);
      int num14 = 103;
      textureComponent6.myID = num14;
      int num15 = 104;
      textureComponent6.rightNeighborID = num15;
      int num16 = 105;
      textureComponent6.leftNeighborID = num16;
      this.upgradeIcon = textureComponent6;
      ClickableTextureComponent textureComponent7 = new ClickableTextureComponent(Game1.content.LoadString("Strings\\UI:Carpenter_MoveBuildings"), new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + this.width - IClickableMenu.borderWidth - IClickableMenu.spaceToClearSideBorder - Game1.tileSize * 4 - Game1.pixelZoom * 5, this.yPositionOnScreen + this.maxHeightOfBuildingViewer + Game1.tileSize, Game1.tileSize, Game1.tileSize), (string) null, (string) null, Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(257, 284, 16, 16), (float) Game1.pixelZoom, false);
      int num17 = 105;
      textureComponent7.myID = num17;
      int num18 = 106;
      textureComponent7.rightNeighborID = num18;
      int num19 = 102;
      textureComponent7.leftNeighborID = num19;
      this.moveButton = textureComponent7;
    }

    public void setNewActiveBlueprint()
    {
      this.currentBuilding = !this.blueprints[this.currentBlueprintIndex].name.Contains("Coop") ? (!this.blueprints[this.currentBlueprintIndex].name.Contains("Barn") ? (!this.blueprints[this.currentBlueprintIndex].name.Contains("Mill") ? (!this.blueprints[this.currentBlueprintIndex].name.Contains("Junimo Hut") ? new Building(this.blueprints[this.currentBlueprintIndex], Vector2.Zero) : (Building) new JunimoHut(this.blueprints[this.currentBlueprintIndex], Vector2.Zero)) : (Building) new Mill(this.blueprints[this.currentBlueprintIndex], Vector2.Zero)) : (Building) new Barn(this.blueprints[this.currentBlueprintIndex], Vector2.Zero)) : (Building) new Coop(this.blueprints[this.currentBlueprintIndex], Vector2.Zero);
      this.price = this.blueprints[this.currentBlueprintIndex].moneyRequired;
      this.ingredients.Clear();
      foreach (KeyValuePair<int, int> keyValuePair in this.blueprints[this.currentBlueprintIndex].itemsRequired)
        this.ingredients.Add((Item) new Object(keyValuePair.Key, keyValuePair.Value, false, -1, 0));
      this.buildingDescription = this.blueprints[this.currentBlueprintIndex].description;
      this.buildingName = this.blueprints[this.currentBlueprintIndex].displayName;
    }

    public override void performHoverAction(int x, int y)
    {
      this.cancelButton.tryHover(x, y, 0.1f);
      base.performHoverAction(x, y);
      if (!this.onFarm)
      {
        this.backButton.tryHover(x, y, 1f);
        this.forwardButton.tryHover(x, y, 1f);
        this.okButton.tryHover(x, y, 0.1f);
        this.demolishButton.tryHover(x, y, 0.1f);
        this.moveButton.tryHover(x, y, 0.1f);
        if (this.CurrentBlueprint.isUpgrade() && this.upgradeIcon.containsPoint(x, y))
          this.hoverText = Game1.content.LoadString("Strings\\UI:Carpenter_Upgrade", (object) new BluePrint(this.CurrentBlueprint.nameOfBuildingToUpgrade).displayName);
        else if (this.demolishButton.containsPoint(x, y))
          this.hoverText = Game1.content.LoadString("Strings\\UI:Carpenter_Demolish");
        else if (this.moveButton.containsPoint(x, y))
          this.hoverText = Game1.content.LoadString("Strings\\UI:Carpenter_MoveBuildings");
        else if (this.okButton.containsPoint(x, y) && this.CurrentBlueprint.doesFarmerHaveEnoughResourcesToBuild())
          this.hoverText = Game1.content.LoadString("Strings\\UI:Carpenter_Build");
        else
          this.hoverText = "";
      }
      else
      {
        if (!this.upgrading && !this.demolishing && !this.moving || this.freeze)
          return;
        foreach (Building building in ((BuildableGameLocation) Game1.getLocationFromName("Farm")).buildings)
          building.color = Color.White;
        Building building1 = ((BuildableGameLocation) Game1.getLocationFromName("Farm")).getBuildingAt(new Vector2((float) ((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float) ((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize))) ?? ((BuildableGameLocation) Game1.getLocationFromName("Farm")).getBuildingAt(new Vector2((float) ((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float) ((Game1.viewport.Y + Game1.getOldMouseY() + Game1.tileSize * 2) / Game1.tileSize))) ?? ((BuildableGameLocation) Game1.getLocationFromName("Farm")).getBuildingAt(new Vector2((float) ((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float) ((Game1.viewport.Y + Game1.getOldMouseY() + Game1.tileSize * 3) / Game1.tileSize)));
        if (this.upgrading)
        {
          if (building1 != null && this.CurrentBlueprint.nameOfBuildingToUpgrade != null && this.CurrentBlueprint.nameOfBuildingToUpgrade.Equals(building1.buildingType))
          {
            building1.color = Color.Lime * 0.8f;
          }
          else
          {
            if (building1 == null)
              return;
            building1.color = Color.Red * 0.8f;
          }
        }
        else if (this.demolishing)
        {
          if (building1 == null)
            return;
          building1.color = Color.Red * 0.8f;
        }
        else
        {
          if (!this.moving || building1 == null)
            return;
          building1.color = Color.Lime * 0.8f;
        }
      }
    }

    public override bool readyToClose()
    {
      if (base.readyToClose())
        return this.buildingToMove == null;
      return false;
    }

    public override void receiveGamePadButton(Buttons b)
    {
      base.receiveGamePadButton(b);
      if (!this.onFarm && b == Buttons.LeftTrigger)
      {
        this.currentBlueprintIndex = this.currentBlueprintIndex - 1;
        if (this.currentBlueprintIndex < 0)
          this.currentBlueprintIndex = this.blueprints.Count - 1;
        this.setNewActiveBlueprint();
        Game1.playSound("shwip");
      }
      if (this.onFarm || b != Buttons.RightTrigger)
        return;
      this.currentBlueprintIndex = (this.currentBlueprintIndex + 1) % this.blueprints.Count;
      this.setNewActiveBlueprint();
      Game1.playSound("shwip");
    }

    public override void receiveKeyPress(Keys key)
    {
      if (this.freeze)
        return;
      if (!this.onFarm)
        base.receiveKeyPress(key);
      if (Game1.globalFade || !this.onFarm)
        return;
      if (Game1.options.doesInputListContain(Game1.options.menuButton, key) && this.readyToClose())
      {
        Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.returnToCarpentryMenu), 0.02f);
      }
      else
      {
        if (Game1.options.SnappyMenus)
          return;
        if (Game1.options.doesInputListContain(Game1.options.moveDownButton, key))
          Game1.panScreen(0, 4);
        else if (Game1.options.doesInputListContain(Game1.options.moveRightButton, key))
          Game1.panScreen(4, 0);
        else if (Game1.options.doesInputListContain(Game1.options.moveUpButton, key))
        {
          Game1.panScreen(0, -4);
        }
        else
        {
          if (!Game1.options.doesInputListContain(Game1.options.moveLeftButton, key))
            return;
          Game1.panScreen(-4, 0);
        }
      }
    }

    public override void update(GameTime time)
    {
      base.update(time);
      if (!this.onFarm || Game1.globalFade)
        return;
      int num1 = Game1.getOldMouseX() + Game1.viewport.X;
      int num2 = Game1.getOldMouseY() + Game1.viewport.Y;
      if (num1 - Game1.viewport.X < Game1.tileSize)
        Game1.panScreen(-8, 0);
      else if (num1 - (Game1.viewport.X + Game1.viewport.Width) >= -Game1.tileSize * 2)
        Game1.panScreen(8, 0);
      if (num2 - Game1.viewport.Y < Game1.tileSize)
        Game1.panScreen(0, -8);
      else if (num2 - (Game1.viewport.Y + Game1.viewport.Height) >= -Game1.tileSize)
        Game1.panScreen(0, 8);
      foreach (Keys pressedKey in Game1.oldKBState.GetPressedKeys())
        this.receiveKeyPress(pressedKey);
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
      if (this.freeze)
        return;
      if (!this.onFarm)
        base.receiveLeftClick(x, y, playSound);
      if (this.cancelButton.containsPoint(x, y))
      {
        if (!this.onFarm)
        {
          this.exitThisMenu(true);
          Game1.player.forceCanMove();
          Game1.playSound("bigDeSelect");
        }
        else
        {
          if (this.moving && this.buildingToMove != null)
          {
            Game1.playSound("cancel");
            return;
          }
          Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.returnToCarpentryMenu), 0.02f);
          Game1.playSound("smallSelect");
          return;
        }
      }
      if (!this.onFarm && this.backButton.containsPoint(x, y))
      {
        this.currentBlueprintIndex = this.currentBlueprintIndex - 1;
        if (this.currentBlueprintIndex < 0)
          this.currentBlueprintIndex = this.blueprints.Count - 1;
        this.setNewActiveBlueprint();
        Game1.playSound("shwip");
        this.backButton.scale = this.backButton.baseScale;
      }
      if (!this.onFarm && this.forwardButton.containsPoint(x, y))
      {
        this.currentBlueprintIndex = (this.currentBlueprintIndex + 1) % this.blueprints.Count;
        this.setNewActiveBlueprint();
        this.backButton.scale = this.backButton.baseScale;
        Game1.playSound("shwip");
      }
      if (!this.onFarm && this.demolishButton.containsPoint(x, y))
      {
        Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.setUpForBuildingPlacement), 0.02f);
        Game1.playSound("smallSelect");
        this.onFarm = true;
        this.demolishing = true;
      }
      if (!this.onFarm && this.moveButton.containsPoint(x, y))
      {
        Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.setUpForBuildingPlacement), 0.02f);
        Game1.playSound("smallSelect");
        this.onFarm = true;
        this.moving = true;
      }
      if (this.okButton.containsPoint(x, y) && !this.onFarm && (Game1.player.money >= this.price && this.blueprints[this.currentBlueprintIndex].doesFarmerHaveEnoughResourcesToBuild()))
      {
        Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.setUpForBuildingPlacement), 0.02f);
        Game1.playSound("smallSelect");
        this.onFarm = true;
      }
      if (!this.onFarm || this.freeze || Game1.globalFade)
        return;
      if (this.demolishing)
      {
        Building buildingAt = ((BuildableGameLocation) Game1.getLocationFromName("Farm")).getBuildingAt(new Vector2((float) ((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float) ((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize)));
        if (buildingAt != null && (buildingAt.daysOfConstructionLeft > 0 || buildingAt.daysUntilUpgrade > 0))
          Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\UI:Carpenter_CantDemolish_DuringConstruction"), Color.Red, 3500f));
        else if (buildingAt != null && buildingAt.indoors != null && (buildingAt.indoors is AnimalHouse && (buildingAt.indoors as AnimalHouse).animalsThatLiveHere.Count > 0))
        {
          Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\UI:Carpenter_CantDemolish_AnimalsHere"), Color.Red, 3500f));
        }
        else
        {
          if (buildingAt == null || !((BuildableGameLocation) Game1.getLocationFromName("Farm")).destroyStructure(buildingAt))
            return;
          int tileY = buildingAt.tileY;
          int tilesHigh = buildingAt.tilesHigh;
          Game1.flashAlpha = 1f;
          buildingAt.showDestroyedAnimation((GameLocation) Game1.getFarm());
          Game1.playSound("explosion");
          Utility.spreadAnimalsAround(buildingAt, (Farm) Game1.getLocationFromName("Farm"));
          DelayedAction.fadeAfterDelay(new Game1.afterFadeFunction(this.returnToCarpentryMenu), 1500);
          this.freeze = true;
        }
      }
      else if (this.upgrading)
      {
        Building buildingAt = ((BuildableGameLocation) Game1.getLocationFromName("Farm")).getBuildingAt(new Vector2((float) ((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float) ((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize)));
        if (buildingAt != null && this.CurrentBlueprint.name != null && buildingAt.buildingType.Equals(this.CurrentBlueprint.nameOfBuildingToUpgrade))
        {
          this.CurrentBlueprint.consumeResources();
          buildingAt.daysUntilUpgrade = 2;
          buildingAt.showUpgradeAnimation((GameLocation) Game1.getFarm());
          Game1.playSound("axe");
          DelayedAction.fadeAfterDelay(new Game1.afterFadeFunction(this.returnToCarpentryMenuAfterSuccessfulBuild), 1500);
          this.freeze = true;
        }
        else
        {
          if (buildingAt == null)
            return;
          Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\UI:Carpenter_CantUpgrade_BuildingType"), Color.Red, 3500f));
        }
      }
      else if (this.moving)
      {
        if (this.buildingToMove == null)
        {
          this.buildingToMove = ((BuildableGameLocation) Game1.getLocationFromName("Farm")).getBuildingAt(new Vector2((float) ((Game1.viewport.X + Game1.getMouseX()) / Game1.tileSize), (float) ((Game1.viewport.Y + Game1.getMouseY()) / Game1.tileSize)));
          if (this.buildingToMove == null)
            return;
          if (this.buildingToMove.daysOfConstructionLeft > 0)
          {
            this.buildingToMove = (Building) null;
          }
          else
          {
            ((BuildableGameLocation) Game1.getLocationFromName("Farm")).buildings.Remove(this.buildingToMove);
            Game1.playSound("axchop");
          }
        }
        else if (((BuildableGameLocation) Game1.getLocationFromName("Farm")).buildStructure(this.buildingToMove, new Vector2((float) ((Game1.viewport.X + Game1.getMouseX()) / Game1.tileSize), (float) ((Game1.viewport.Y + Game1.getMouseY()) / Game1.tileSize)), false, Game1.player))
        {
          this.buildingToMove = (Building) null;
          Game1.playSound("axchop");
          DelayedAction.playSoundAfterDelay("dirtyHit", 50);
          DelayedAction.playSoundAfterDelay("dirtyHit", 150);
        }
        else
          Game1.playSound("cancel");
      }
      else if (this.tryToBuild())
      {
        this.CurrentBlueprint.consumeResources();
        DelayedAction.fadeAfterDelay(new Game1.afterFadeFunction(this.returnToCarpentryMenuAfterSuccessfulBuild), 2000);
        this.freeze = true;
      }
      else
        Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\UI:Carpenter_CantBuild"), Color.Red, 3500f));
    }

    public bool tryToBuild()
    {
      return ((BuildableGameLocation) Game1.getLocationFromName("Farm")).buildStructure(this.CurrentBlueprint, new Vector2((float) ((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float) ((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize)), false, Game1.player, this.magicalConstruction);
    }

    public void returnToCarpentryMenu()
    {
      Game1.currentLocation.cleanupBeforePlayerExit();
      Game1.currentLocation = Game1.getLocationFromName(this.magicalConstruction ? "WizardHouse" : "ScienceHouse");
      Game1.currentLocation.resetForPlayerEntry();
      Game1.globalFadeToClear((Game1.afterFadeFunction) null, 0.02f);
      this.onFarm = false;
      this.resetBounds();
      this.upgrading = false;
      this.moving = false;
      this.freeze = false;
      Game1.displayHUD = true;
      Game1.viewportFreeze = false;
      Game1.viewport.Location = new Location(5 * Game1.tileSize, 24 * Game1.tileSize);
      this.drawBG = true;
      this.demolishing = false;
      Game1.displayFarmer = true;
    }

    public void returnToCarpentryMenuAfterSuccessfulBuild()
    {
      Game1.currentLocation.cleanupBeforePlayerExit();
      Game1.currentLocation = Game1.getLocationFromName(this.magicalConstruction ? "WizardHouse" : "ScienceHouse");
      Game1.currentLocation.resetForPlayerEntry();
      Game1.globalFadeToClear(new Game1.afterFadeFunction(this.robinConstructionMessage), 0.02f);
      Game1.displayHUD = true;
      Game1.viewportFreeze = false;
      Game1.viewport.Location = new Location(5 * Game1.tileSize, 24 * Game1.tileSize);
      this.freeze = true;
      Game1.displayFarmer = true;
    }

    public void robinConstructionMessage()
    {
      this.exitThisMenu(true);
      Game1.player.forceCanMove();
      if (this.magicalConstruction)
        return;
      string str1 = "Data\\ExtraDialogue:Robin_" + (this.upgrading ? "Upgrade" : "New") + "Construction";
      if (Utility.isFestivalDay(Game1.dayOfMonth + 1, Game1.currentSeason))
        str1 += "_Festival";
      NPC characterFromName = Game1.getCharacterFromName("Robin", false);
      LocalizedContentManager content = Game1.content;
      string path = str1;
      object[] objArray = new object[2]
      {
        (object) (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.de ? this.CurrentBlueprint.displayName : this.CurrentBlueprint.displayName.ToLower()),
        null
      };
      int index = 1;
      string str2;
      if (LocalizedContentManager.CurrentLanguageCode != LocalizedContentManager.LanguageCode.de)
      {
        if (LocalizedContentManager.CurrentLanguageCode != LocalizedContentManager.LanguageCode.pt && LocalizedContentManager.CurrentLanguageCode != LocalizedContentManager.LanguageCode.es)
          str2 = ((IEnumerable<string>) this.CurrentBlueprint.displayName.ToLower().Split(' ')).Last<string>();
        else
          str2 = ((IEnumerable<string>) this.CurrentBlueprint.displayName.ToLower().Split(' ')).First<string>();
      }
      else
        str2 = ((IEnumerable<string>) ((IEnumerable<string>) this.CurrentBlueprint.displayName.Split(' ')).Last<string>().Split('-')).Last<string>();
      objArray[index] = (object) str2;
      string dialogue = content.LoadString(path, objArray);
      Game1.drawDialogue(characterFromName, dialogue);
    }

    public override bool overrideSnappyMenuCursorMovementBan()
    {
      return this.onFarm;
    }

    public void setUpForBuildingPlacement()
    {
      Game1.currentLocation.cleanupBeforePlayerExit();
      this.hoverText = "";
      Game1.currentLocation = Game1.getLocationFromName("Farm");
      Game1.currentLocation.resetForPlayerEntry();
      Game1.globalFadeToClear((Game1.afterFadeFunction) null, 0.02f);
      this.onFarm = true;
      this.cancelButton.bounds.X = Game1.viewport.Width - Game1.tileSize * 2;
      this.cancelButton.bounds.Y = Game1.viewport.Height - Game1.tileSize * 2;
      Game1.displayHUD = false;
      Game1.viewportFreeze = true;
      Game1.viewport.Location = new Location(49 * Game1.tileSize, 5 * Game1.tileSize);
      Game1.panScreen(0, 0);
      this.drawBG = false;
      this.freeze = false;
      Game1.displayFarmer = false;
      if (this.demolishing || this.CurrentBlueprint.nameOfBuildingToUpgrade == null || (this.CurrentBlueprint.nameOfBuildingToUpgrade.Length <= 0 || this.moving))
        return;
      this.upgrading = true;
    }

    public override void gameWindowSizeChanged(Microsoft.Xna.Framework.Rectangle oldBounds, Microsoft.Xna.Framework.Rectangle newBounds)
    {
      this.resetBounds();
    }

    public override void draw(SpriteBatch b)
    {
      if (this.drawBG)
        b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.5f);
      if (Game1.globalFade || this.freeze)
        return;
      if (!this.onFarm)
      {
        base.draw(b);
        IClickableMenu.drawTextureBox(b, this.xPositionOnScreen - Game1.tileSize * 3 / 2, this.yPositionOnScreen - Game1.tileSize / 4, this.maxWidthOfBuildingViewer + Game1.tileSize, this.maxHeightOfBuildingViewer + Game1.tileSize, this.magicalConstruction ? Color.RoyalBlue : Color.White);
        this.currentBuilding.drawInMenu(b, this.xPositionOnScreen + this.maxWidthOfBuildingViewer / 2 - this.currentBuilding.tilesWide * Game1.tileSize / 2 - Game1.tileSize, this.yPositionOnScreen + this.maxHeightOfBuildingViewer / 2 - this.currentBuilding.getSourceRectForMenu().Height * Game1.pixelZoom / 2);
        if (this.CurrentBlueprint.isUpgrade())
          this.upgradeIcon.draw(b);
        string str = " Deluxe  Barn   ";
        SpriteText.drawStringWithScrollBackground(b, this.buildingName, this.xPositionOnScreen + this.maxWidthOfBuildingViewer - IClickableMenu.spaceToClearSideBorder - Game1.tileSize / 4 + Game1.tileSize + ((this.width - (this.maxWidthOfBuildingViewer + Game1.tileSize * 2)) / 2 - SpriteText.getWidthOfString(str) / 2), this.yPositionOnScreen, str, 1f, -1);
        IClickableMenu.drawTextureBox(b, this.xPositionOnScreen + this.maxWidthOfBuildingViewer - Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize * 5 / 4, this.maxWidthOfDescription + Game1.tileSize, this.maxWidthOfDescription + Game1.tileSize * 3 / 2, this.magicalConstruction ? Color.RoyalBlue : Color.White);
        if (this.magicalConstruction)
        {
          Utility.drawTextWithShadow(b, Game1.parseText(this.buildingDescription, Game1.dialogueFont, this.maxWidthOfDescription + Game1.tileSize / 2), Game1.dialogueFont, new Vector2((float) (this.xPositionOnScreen + this.maxWidthOfDescription + Game1.tileSize - Game1.pixelZoom), (float) (this.yPositionOnScreen + Game1.tileSize * 5 / 4 + Game1.pixelZoom * 4 + Game1.pixelZoom)), Game1.textColor * 0.25f, 1f, -1f, -1, -1, 0.0f, 3);
          Utility.drawTextWithShadow(b, Game1.parseText(this.buildingDescription, Game1.dialogueFont, this.maxWidthOfDescription + Game1.tileSize / 2), Game1.dialogueFont, new Vector2((float) (this.xPositionOnScreen + this.maxWidthOfDescription + Game1.tileSize - 1), (float) (this.yPositionOnScreen + Game1.tileSize * 5 / 4 + Game1.pixelZoom * 4 + Game1.pixelZoom)), Game1.textColor * 0.25f, 1f, -1f, -1, -1, 0.0f, 3);
        }
        Utility.drawTextWithShadow(b, Game1.parseText(this.buildingDescription, Game1.dialogueFont, this.maxWidthOfDescription + Game1.tileSize / 2), Game1.dialogueFont, new Vector2((float) (this.xPositionOnScreen + this.maxWidthOfDescription + Game1.tileSize), (float) (this.yPositionOnScreen + Game1.tileSize * 5 / 4 + Game1.pixelZoom * 4)), this.magicalConstruction ? Color.PaleGoldenrod : Game1.textColor, 1f, -1f, -1, -1, this.magicalConstruction ? 0.0f : 0.25f, 3);
        Vector2 location = new Vector2((float) (this.xPositionOnScreen + this.maxWidthOfDescription + Game1.tileSize / 4 + Game1.tileSize), (float) (this.yPositionOnScreen + Game1.tileSize * 4 + Game1.tileSize / 2));
        SpriteText.drawString(b, "$", (int) location.X, (int) location.Y, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
        if (this.magicalConstruction)
        {
          Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\StringsFromCSFiles:LoadGameMenu.cs.11020", (object) this.price), Game1.dialogueFont, new Vector2(location.X + (float) Game1.tileSize, location.Y + (float) (Game1.pixelZoom * 2)), Game1.textColor * 0.5f, 1f, -1f, -1, -1, (float) (this.magicalConstruction ? 0.0 : 0.25), 3);
          Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\StringsFromCSFiles:LoadGameMenu.cs.11020", (object) this.price), Game1.dialogueFont, new Vector2((float) ((double) location.X + (double) Game1.tileSize + (double) Game1.pixelZoom - 1.0), location.Y + (float) (Game1.pixelZoom * 2)), Game1.textColor * 0.25f, 1f, -1f, -1, -1, (float) (this.magicalConstruction ? 0.0 : 0.25), 3);
        }
        Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\StringsFromCSFiles:LoadGameMenu.cs.11020", (object) this.price), Game1.dialogueFont, new Vector2(location.X + (float) Game1.tileSize + (float) Game1.pixelZoom, location.Y + (float) Game1.pixelZoom), Game1.player.money >= this.price ? (this.magicalConstruction ? Color.PaleGoldenrod : Game1.textColor) : Color.Red, 1f, -1f, -1, -1, (float) (this.magicalConstruction ? 0.0 : 0.25), 3);
        location.X -= (float) (Game1.tileSize / 4);
        location.Y -= (float) (Game1.tileSize / 3);
        foreach (Item ingredient in this.ingredients)
        {
          location.Y += (float) (Game1.tileSize + Game1.pixelZoom);
          ingredient.drawInMenu(b, location, 1f);
          bool flag = !(ingredient is Object) || Game1.player.hasItemInInventory((ingredient as Object).parentSheetIndex, ingredient.Stack, 0);
          if (this.magicalConstruction)
          {
            Utility.drawTextWithShadow(b, ingredient.DisplayName, Game1.dialogueFont, new Vector2(location.X + (float) Game1.tileSize + (float) (Game1.pixelZoom * 3), location.Y + (float) (Game1.pixelZoom * 6)), Game1.textColor * 0.25f, 1f, -1f, -1, -1, this.magicalConstruction ? 0.0f : 0.25f, 3);
            Utility.drawTextWithShadow(b, ingredient.DisplayName, Game1.dialogueFont, new Vector2((float) ((double) location.X + (double) Game1.tileSize + (double) (Game1.pixelZoom * 4) - 1.0), location.Y + (float) (Game1.pixelZoom * 6)), Game1.textColor * 0.25f, 1f, -1f, -1, -1, this.magicalConstruction ? 0.0f : 0.25f, 3);
          }
          Utility.drawTextWithShadow(b, ingredient.DisplayName, Game1.dialogueFont, new Vector2(location.X + (float) Game1.tileSize + (float) (Game1.pixelZoom * 4), location.Y + (float) (Game1.pixelZoom * 5)), flag ? (this.magicalConstruction ? Color.PaleGoldenrod : Game1.textColor) : Color.Red, 1f, -1f, -1, -1, this.magicalConstruction ? 0.0f : 0.25f, 3);
        }
        this.backButton.draw(b);
        this.forwardButton.draw(b);
        this.okButton.draw(b, this.blueprints[this.currentBlueprintIndex].doesFarmerHaveEnoughResourcesToBuild() ? Color.White : Color.Gray * 0.8f, 0.88f);
        this.demolishButton.draw(b);
        this.moveButton.draw(b);
      }
      else
      {
        string str;
        if (!this.upgrading)
          str = this.demolishing ? Game1.content.LoadString("Strings\\UI:Carpenter_SelectBuilding_Demolish") : Game1.content.LoadString("Strings\\UI:Carpenter_ChooseLocation");
        else
          str = Game1.content.LoadString("Strings\\UI:Carpenter_SelectBuilding_Upgrade", (object) new BluePrint(this.CurrentBlueprint.nameOfBuildingToUpgrade).displayName);
        string s = str;
        SpriteText.drawStringWithScrollBackground(b, s, Game1.viewport.Width / 2 - SpriteText.getWidthOfString(s) / 2, Game1.tileSize / 4, "", 1f, -1);
        if (!this.upgrading && !this.demolishing && !this.moving)
        {
          Vector2 vector2 = new Vector2((float) ((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float) ((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize));
          for (int y = 0; y < this.CurrentBlueprint.tilesHeight; ++y)
          {
            for (int x = 0; x < this.CurrentBlueprint.tilesWidth; ++x)
            {
              int structurePlacementTile = this.CurrentBlueprint.getTileSheetIndexForStructurePlacementTile(x, y);
              Vector2 tileLocation = new Vector2(vector2.X + (float) x, vector2.Y + (float) y);
              if (!(Game1.currentLocation as BuildableGameLocation).isBuildable(tileLocation))
                ++structurePlacementTile;
              b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, tileLocation * (float) Game1.tileSize), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(194 + structurePlacementTile * 16, 388, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.999f);
            }
          }
        }
        else if (this.moving && this.buildingToMove != null)
        {
          Vector2 vector2 = new Vector2((float) ((Game1.viewport.X + Game1.getOldMouseX()) / Game1.tileSize), (float) ((Game1.viewport.Y + Game1.getOldMouseY()) / Game1.tileSize));
          for (int y = 0; y < this.buildingToMove.tilesHigh; ++y)
          {
            for (int x = 0; x < this.buildingToMove.tilesWide; ++x)
            {
              int structurePlacementTile = this.buildingToMove.getTileSheetIndexForStructurePlacementTile(x, y);
              Vector2 tileLocation = new Vector2(vector2.X + (float) x, vector2.Y + (float) y);
              if (!(Game1.currentLocation as BuildableGameLocation).isBuildable(tileLocation))
                ++structurePlacementTile;
              b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, tileLocation * (float) Game1.tileSize), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(194 + structurePlacementTile * 16, 388, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.999f);
            }
          }
        }
      }
      this.cancelButton.draw(b);
      this.drawMouse(b);
      if (this.hoverText.Length <= 0)
        return;
      IClickableMenu.drawHoverText(b, this.hoverText, Game1.dialogueFont, 0, 0, -1, (string) null, -1, (string[]) null, (Item) null, 0, -1, -1, -1, -1, 1f, (CraftingRecipe) null);
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
    }
  }
}
