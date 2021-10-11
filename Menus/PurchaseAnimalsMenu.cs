// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.PurchaseAnimalsMenu
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Buildings;
using System;
using System.Collections.Generic;
using xTile.Dimensions;

namespace StardewValley.Menus
{
  public class PurchaseAnimalsMenu : IClickableMenu
  {
    public static int menuHeight = Game1.tileSize * 5;
    public static int menuWidth = Game1.tileSize * 7;
    public List<ClickableTextureComponent> animalsToPurchase = new List<ClickableTextureComponent>();
    public const int region_okButton = 101;
    public const int region_doneNamingButton = 102;
    public const int region_randomButton = 103;
    public const int region_namingBox = 104;
    public ClickableTextureComponent okButton;
    public ClickableTextureComponent doneNamingButton;
    public ClickableTextureComponent randomButton;
    public ClickableTextureComponent hovered;
    public ClickableComponent textBoxCC;
    private bool onFarm;
    private bool namingAnimal;
    private bool freeze;
    private FarmAnimal animalBeingPurchased;
    private TextBox textBox;
    private TextBoxEvent e;
    private Building newAnimalHome;
    private int priceOfAnimal;

    public PurchaseAnimalsMenu(List<StardewValley.Object> stock)
      : base(Game1.viewport.Width / 2 - PurchaseAnimalsMenu.menuWidth / 2 - IClickableMenu.borderWidth * 2, Game1.viewport.Height / 2 - PurchaseAnimalsMenu.menuHeight - IClickableMenu.borderWidth * 2, PurchaseAnimalsMenu.menuWidth + IClickableMenu.borderWidth * 2, PurchaseAnimalsMenu.menuHeight + IClickableMenu.borderWidth, false)
    {
      this.height = this.height + Game1.tileSize;
      for (int index = 0; index < stock.Count; ++index)
      {
        List<ClickableTextureComponent> animalsToPurchase = this.animalsToPurchase;
        ClickableTextureComponent textureComponent = new ClickableTextureComponent(string.Concat((object) stock[index].salePrice()), new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + IClickableMenu.borderWidth + index % 3 * Game1.tileSize * 2, this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth / 2 + index / 3 * (Game1.tileSize + Game1.tileSize / 3), Game1.tileSize * 2, Game1.tileSize), (string) null, stock[index].Name, Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(index % 3 * 16 * 2, 448 + index / 3 * 16, 32, 16), 4f, stock[index].type == null);
        StardewValley.Object @object = stock[index];
        textureComponent.item = (Item) @object;
        int num1 = index;
        textureComponent.myID = num1;
        int num2 = index % 3 == 2 ? -1 : index + 1;
        textureComponent.rightNeighborID = num2;
        int num3 = index % 3 == 0 ? -1 : index - 1;
        textureComponent.leftNeighborID = num3;
        int num4 = index + 3;
        textureComponent.downNeighborID = num4;
        int num5 = index - 3;
        textureComponent.upNeighborID = num5;
        animalsToPurchase.Add(textureComponent);
      }
      ClickableTextureComponent textureComponent1 = new ClickableTextureComponent(new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + this.width + 4, this.yPositionOnScreen + this.height - Game1.tileSize - IClickableMenu.borderWidth, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 47, -1, -1), 1f, false);
      int num6 = 101;
      textureComponent1.myID = num6;
      int num7 = 103;
      textureComponent1.upNeighborID = num7;
      int num8 = 103;
      textureComponent1.leftNeighborID = num8;
      this.okButton = textureComponent1;
      ClickableTextureComponent textureComponent2 = new ClickableTextureComponent(new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize * 4 / 5 + Game1.tileSize, Game1.viewport.Height / 2, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(381, 361, 10, 10), (float) Game1.pixelZoom, false);
      int num9 = 103;
      textureComponent2.myID = num9;
      int num10 = 101;
      textureComponent2.downNeighborID = num10;
      int num11 = 101;
      textureComponent2.rightNeighborID = num11;
      this.randomButton = textureComponent2;
      PurchaseAnimalsMenu.menuHeight = Game1.tileSize * 5;
      PurchaseAnimalsMenu.menuWidth = Game1.tileSize * 7;
      this.textBox = new TextBox((Texture2D) null, (Texture2D) null, Game1.dialogueFont, Game1.textColor);
      this.textBox.X = Game1.viewport.Width / 2 - Game1.tileSize * 3;
      this.textBox.Y = Game1.viewport.Height / 2;
      this.textBox.Width = Game1.tileSize * 4;
      this.textBox.Height = Game1.tileSize * 3;
      this.e = new TextBoxEvent(this.textBoxEnter);
      this.textBoxCC = new ClickableComponent(new Microsoft.Xna.Framework.Rectangle(this.textBox.X, this.textBox.Y, 192, 48), "")
      {
        myID = 104,
        rightNeighborID = 102,
        downNeighborID = 101
      };
      ClickableTextureComponent textureComponent3 = new ClickableTextureComponent(new Microsoft.Xna.Framework.Rectangle(this.textBox.X + this.textBox.Width + Game1.tileSize + Game1.tileSize * 3 / 4 - Game1.pixelZoom * 2, Game1.viewport.Height / 2 + Game1.pixelZoom, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(381, 361, 10, 10), (float) Game1.pixelZoom, false);
      int num12 = 103;
      textureComponent3.myID = num12;
      int num13 = 102;
      textureComponent3.leftNeighborID = num13;
      int num14 = 101;
      textureComponent3.downNeighborID = num14;
      int num15 = 101;
      textureComponent3.rightNeighborID = num15;
      this.randomButton = textureComponent3;
      ClickableTextureComponent textureComponent4 = new ClickableTextureComponent(new Microsoft.Xna.Framework.Rectangle(this.textBox.X + this.textBox.Width + Game1.tileSize / 2 + Game1.pixelZoom, Game1.viewport.Height / 2 - Game1.pixelZoom * 2, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f, false);
      int num16 = 102;
      textureComponent4.myID = num16;
      int num17 = 103;
      textureComponent4.rightNeighborID = num17;
      int num18 = 104;
      textureComponent4.leftNeighborID = num18;
      int num19 = 101;
      textureComponent4.downNeighborID = num19;
      this.doneNamingButton = textureComponent4;
      if (!Game1.options.SnappyMenus)
        return;
      this.populateClickableComponentList();
      this.snapToDefaultClickableComponent();
    }

    public override void snapToDefaultClickableComponent()
    {
      this.currentlySnappedComponent = this.getComponentWithID(0);
      this.snapCursorToCurrentSnappedComponent();
    }

    public void textBoxEnter(TextBox sender)
    {
      if (!this.namingAnimal)
        return;
      if (Game1.activeClickableMenu == null || !(Game1.activeClickableMenu is PurchaseAnimalsMenu))
      {
        this.textBox.OnEnterPressed -= this.e;
      }
      else
      {
        if (sender.Text.Length < 1)
          return;
        if (Utility.areThereAnyOtherAnimalsWithThisName(sender.Text))
        {
          Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:PurchaseAnimalsMenu.cs.11308"));
        }
        else
        {
          this.textBox.OnEnterPressed -= this.e;
          this.animalBeingPurchased.name = sender.Text;
          this.animalBeingPurchased.displayName = sender.Text;
          this.animalBeingPurchased.home = this.newAnimalHome;
          this.animalBeingPurchased.homeLocation = new Vector2((float) this.newAnimalHome.tileX, (float) this.newAnimalHome.tileY);
          this.animalBeingPurchased.setRandomPosition(this.animalBeingPurchased.home.indoors);
          (this.newAnimalHome.indoors as AnimalHouse).animals.Add(this.animalBeingPurchased.myID, this.animalBeingPurchased);
          (this.newAnimalHome.indoors as AnimalHouse).animalsThatLiveHere.Add(this.animalBeingPurchased.myID);
          this.newAnimalHome = (Building) null;
          this.namingAnimal = false;
          Game1.player.money -= this.priceOfAnimal;
          Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.setUpForReturnAfterPurchasingAnimal), 0.02f);
        }
      }
    }

    public void setUpForReturnAfterPurchasingAnimal()
    {
      Game1.currentLocation.cleanupBeforePlayerExit();
      Game1.currentLocation = Game1.getLocationFromName("AnimalShop");
      Game1.currentLocation.resetForPlayerEntry();
      Game1.globalFadeToClear((Game1.afterFadeFunction) null, 0.02f);
      this.onFarm = false;
      this.okButton.bounds.X = this.xPositionOnScreen + this.width + 4;
      Game1.displayHUD = true;
      Game1.displayFarmer = true;
      this.freeze = false;
      this.textBox.OnEnterPressed -= this.e;
      this.textBox.Selected = false;
      Game1.viewportFreeze = false;
      Game1.globalFadeToClear(new Game1.afterFadeFunction(this.marnieAnimalPurchaseMessage), 0.02f);
    }

    public void marnieAnimalPurchaseMessage()
    {
      this.exitThisMenu(true);
      Game1.player.forceCanMove();
      this.freeze = false;
      NPC characterFromName = Game1.getCharacterFromName("Marnie", false);
      string dialogue;
      if (!this.animalBeingPurchased.isMale())
        dialogue = Game1.content.LoadString("Strings\\StringsFromCSFiles:PurchaseAnimalsMenu.cs.11314", (object) this.animalBeingPurchased.displayName);
      else
        dialogue = Game1.content.LoadString("Strings\\StringsFromCSFiles:PurchaseAnimalsMenu.cs.11311", (object) this.animalBeingPurchased.displayName);
      Game1.drawDialogue(characterFromName, dialogue);
    }

    public void setUpForAnimalPlacement()
    {
      Game1.displayFarmer = false;
      Game1.currentLocation = Game1.getLocationFromName("Farm");
      Game1.currentLocation.resetForPlayerEntry();
      Game1.globalFadeToClear((Game1.afterFadeFunction) null, 0.02f);
      this.onFarm = true;
      this.freeze = false;
      this.okButton.bounds.X = Game1.viewport.Width - Game1.tileSize * 2;
      this.okButton.bounds.Y = Game1.viewport.Height - Game1.tileSize * 2;
      Game1.displayHUD = false;
      Game1.viewportFreeze = true;
      Game1.viewport.Location = new Location(49 * Game1.tileSize, 5 * Game1.tileSize);
      Game1.panScreen(0, 0);
    }

    public void setUpForReturnToShopMenu()
    {
      this.freeze = false;
      Game1.displayFarmer = true;
      Game1.currentLocation.cleanupBeforePlayerExit();
      Game1.currentLocation = Game1.getLocationFromName("AnimalShop");
      Game1.currentLocation.resetForPlayerEntry();
      Game1.globalFadeToClear((Game1.afterFadeFunction) null, 0.02f);
      this.onFarm = false;
      this.okButton.bounds.X = this.xPositionOnScreen + this.width + 4;
      this.okButton.bounds.Y = this.yPositionOnScreen + this.height - Game1.tileSize - IClickableMenu.borderWidth;
      Game1.displayHUD = true;
      Game1.viewportFreeze = false;
      this.namingAnimal = false;
      this.textBox.OnEnterPressed -= this.e;
      this.textBox.Selected = false;
      if (!Game1.options.SnappyMenus)
        return;
      this.snapToDefaultClickableComponent();
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
      if (Game1.globalFade || this.freeze)
        return;
      if (this.okButton != null && this.okButton.containsPoint(x, y) && this.readyToClose())
      {
        if (this.onFarm)
        {
          Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.setUpForReturnToShopMenu), 0.02f);
          Game1.playSound("smallSelect");
        }
        else
        {
          Game1.exitActiveMenu();
          Game1.playSound("bigDeSelect");
        }
      }
      if (this.onFarm)
      {
        Building buildingAt = (Game1.getLocationFromName("Farm") as Farm).getBuildingAt(new Vector2((float) ((x + Game1.viewport.X) / Game1.tileSize), (float) ((y + Game1.viewport.Y) / Game1.tileSize)));
        if (buildingAt != null && !this.namingAnimal)
        {
          if (buildingAt.buildingType.Contains(this.animalBeingPurchased.buildingTypeILiveIn))
          {
            if ((buildingAt.indoors as AnimalHouse).isFull())
              Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:PurchaseAnimalsMenu.cs.11321"));
            else if ((int) this.animalBeingPurchased.harvestType != 2)
            {
              this.namingAnimal = true;
              this.newAnimalHome = buildingAt;
              if (this.animalBeingPurchased.sound != null && Game1.soundBank != null)
              {
                Cue cue = Game1.soundBank.GetCue(this.animalBeingPurchased.sound);
                string name = "Pitch";
                double num = (double) (1200 + Game1.random.Next(-200, 201));
                cue.SetVariable(name, (float) num);
                cue.Play();
              }
              this.textBox.OnEnterPressed += this.e;
              this.textBox.Text = this.animalBeingPurchased.displayName;
              Game1.keyboardDispatcher.Subscriber = (IKeyboardSubscriber) this.textBox;
              if (Game1.options.SnappyMenus)
              {
                this.currentlySnappedComponent = this.getComponentWithID(104);
                this.snapCursorToCurrentSnappedComponent();
              }
            }
            else if (Game1.player.money >= this.priceOfAnimal)
            {
              this.newAnimalHome = buildingAt;
              this.animalBeingPurchased.home = this.newAnimalHome;
              this.animalBeingPurchased.homeLocation = new Vector2((float) this.newAnimalHome.tileX, (float) this.newAnimalHome.tileY);
              this.animalBeingPurchased.setRandomPosition(this.animalBeingPurchased.home.indoors);
              (this.newAnimalHome.indoors as AnimalHouse).animals.Add(this.animalBeingPurchased.myID, this.animalBeingPurchased);
              (this.newAnimalHome.indoors as AnimalHouse).animalsThatLiveHere.Add(this.animalBeingPurchased.myID);
              this.newAnimalHome = (Building) null;
              this.namingAnimal = false;
              if (this.animalBeingPurchased.sound != null && Game1.soundBank != null)
              {
                Cue cue = Game1.soundBank.GetCue(this.animalBeingPurchased.sound);
                string name = "Pitch";
                double num = (double) (1200 + Game1.random.Next(-200, 201));
                cue.SetVariable(name, (float) num);
                cue.Play();
              }
              Game1.player.money -= this.priceOfAnimal;
              Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:PurchaseAnimalsMenu.cs.11324", (object) this.animalBeingPurchased.displayType), Color.LimeGreen, 3500f));
              this.animalBeingPurchased = new FarmAnimal(this.animalBeingPurchased.type, MultiplayerUtility.getNewID(), Game1.player.uniqueMultiplayerID);
            }
            else if (Game1.player.money < this.priceOfAnimal)
              Game1.dayTimeMoneyBox.moneyShakeTimer = 1000;
          }
          else
            Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:PurchaseAnimalsMenu.cs.11326", (object) this.animalBeingPurchased.displayType));
        }
        if (!this.namingAnimal)
          return;
        if (this.doneNamingButton.containsPoint(x, y))
        {
          this.textBoxEnter(this.textBox);
          Game1.playSound("smallSelect");
        }
        else if (this.namingAnimal && this.randomButton.containsPoint(x, y))
        {
          this.animalBeingPurchased.name = Dialogue.randomName();
          this.animalBeingPurchased.displayName = this.animalBeingPurchased.name;
          this.textBox.Text = this.animalBeingPurchased.displayName;
          this.randomButton.scale = this.randomButton.baseScale;
          Game1.playSound("drumkit6");
        }
        this.textBox.Update();
      }
      else
      {
        foreach (ClickableTextureComponent textureComponent in this.animalsToPurchase)
        {
          if (textureComponent.containsPoint(x, y) && (textureComponent.item as StardewValley.Object).type == null)
          {
            int num = textureComponent.item.salePrice();
            if (Game1.player.money >= num)
            {
              Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.setUpForAnimalPlacement), 0.02f);
              Game1.playSound("smallSelect");
              this.onFarm = true;
              this.animalBeingPurchased = new FarmAnimal(textureComponent.hoverText, MultiplayerUtility.getNewID(), Game1.player.uniqueMultiplayerID);
              this.priceOfAnimal = num;
            }
            else
              Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:PurchaseAnimalsMenu.cs.11325"), Color.Red, 3500f));
          }
        }
      }
    }

    public override bool overrideSnappyMenuCursorMovementBan()
    {
      if (this.onFarm)
        return !this.namingAnimal;
      return false;
    }

    public override void receiveGamePadButton(Buttons b)
    {
      base.receiveGamePadButton(b);
      if (b != Buttons.B || Game1.globalFade || (!this.onFarm || !this.namingAnimal))
        return;
      Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.setUpForReturnToShopMenu), 0.02f);
      Game1.playSound("smallSelect");
    }

    public override void receiveKeyPress(Keys key)
    {
      if (Game1.globalFade || this.freeze)
        return;
      if (!Game1.globalFade && this.onFarm)
      {
        if (!this.namingAnimal)
        {
          if (Game1.options.doesInputListContain(Game1.options.menuButton, key) && this.readyToClose())
          {
            Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.setUpForReturnToShopMenu), 0.02f);
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
        else
        {
          if (!Game1.options.SnappyMenus)
            return;
          if (!this.textBox.Selected && Game1.options.doesInputListContain(Game1.options.menuButton, key))
          {
            Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.setUpForReturnToShopMenu), 0.02f);
            Game1.playSound("smallSelect");
          }
          else
          {
            if (this.textBox.Selected && Game1.options.doesInputListContain(Game1.options.menuButton, key))
              return;
            base.receiveKeyPress(key);
          }
        }
      }
      else if (Game1.options.doesInputListContain(Game1.options.menuButton, key) && !Game1.globalFade)
      {
        if (!this.readyToClose())
          return;
        Game1.player.forceCanMove();
        Game1.exitActiveMenu();
        Game1.playSound("bigDeSelect");
      }
      else
      {
        if (!Game1.options.SnappyMenus)
          return;
        base.receiveKeyPress(key);
      }
    }

    public override void update(GameTime time)
    {
      base.update(time);
      if (!this.onFarm || this.namingAnimal)
        return;
      int num1 = Game1.getOldMouseX() + Game1.viewport.X;
      int num2 = Game1.getOldMouseY() + Game1.viewport.Y;
      if (num1 - Game1.viewport.X < Game1.tileSize)
        Game1.panScreen(-8, 0);
      else if (num1 - (Game1.viewport.X + Game1.viewport.Width) >= -Game1.tileSize)
        Game1.panScreen(8, 0);
      if (num2 - Game1.viewport.Y < Game1.tileSize)
        Game1.panScreen(0, -8);
      else if (num2 - (Game1.viewport.Y + Game1.viewport.Height) >= -Game1.tileSize)
        Game1.panScreen(0, 8);
      foreach (Keys pressedKey in Game1.oldKBState.GetPressedKeys())
        this.receiveKeyPress(pressedKey);
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
    }

    public override void performHoverAction(int x, int y)
    {
      this.hovered = (ClickableTextureComponent) null;
      if (Game1.globalFade || this.freeze)
        return;
      if (this.okButton != null)
      {
        if (this.okButton.containsPoint(x, y))
          this.okButton.scale = Math.Min(1.1f, this.okButton.scale + 0.05f);
        else
          this.okButton.scale = Math.Max(1f, this.okButton.scale - 0.05f);
      }
      if (this.onFarm)
      {
        if (!this.namingAnimal)
        {
          Vector2 tile = new Vector2((float) ((x + Game1.viewport.X) / Game1.tileSize), (float) ((y + Game1.viewport.Y) / Game1.tileSize));
          Farm locationFromName = Game1.getLocationFromName("Farm") as Farm;
          foreach (Building building in locationFromName.buildings)
            building.color = Color.White;
          Building buildingAt = locationFromName.getBuildingAt(tile);
          if (buildingAt != null)
            buildingAt.color = !buildingAt.buildingType.Contains(this.animalBeingPurchased.buildingTypeILiveIn) || (buildingAt.indoors as AnimalHouse).isFull() ? Color.Red * 0.8f : Color.LightGreen * 0.8f;
        }
        if (this.doneNamingButton != null)
        {
          if (this.doneNamingButton.containsPoint(x, y))
            this.doneNamingButton.scale = Math.Min(1.1f, this.doneNamingButton.scale + 0.05f);
          else
            this.doneNamingButton.scale = Math.Max(1f, this.doneNamingButton.scale - 0.05f);
        }
        this.randomButton.tryHover(x, y, 0.5f);
      }
      else
      {
        foreach (ClickableTextureComponent textureComponent in this.animalsToPurchase)
        {
          if (textureComponent.containsPoint(x, y))
          {
            textureComponent.scale = Math.Min(textureComponent.scale + 0.05f, 4.1f);
            this.hovered = textureComponent;
          }
          else
            textureComponent.scale = Math.Max(4f, textureComponent.scale - 0.025f);
        }
      }
    }

    public static string getAnimalTitle(string name)
    {
      // ISSUE: reference to a compiler-generated method
      uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(name);
      if (stringHash <= 889009852U)
      {
        if ((int) stringHash != 292886277)
        {
          if ((int) stringHash != 613168024)
          {
            if ((int) stringHash == 889009852 && name == "Chicken")
              return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5922");
          }
          else if (name == "Duck")
            return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5937");
        }
        else if (name == "Rabbit")
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5945");
      }
      else if (stringHash <= 2392565758U)
      {
        if ((int) stringHash != 1601263067)
        {
          if ((int) stringHash == -1902401538 && name == "Goat")
            return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5933");
        }
        else if (name == "Dairy Cow")
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5927");
      }
      else if ((int) stringHash != -1342046060)
      {
        if ((int) stringHash == -850948305 && name == "Pig")
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5948");
      }
      else if (name == "Sheep")
        return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5942");
      return "";
    }

    public static string getAnimalDescription(string name)
    {
      // ISSUE: reference to a compiler-generated method
      uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(name);
      if (stringHash <= 889009852U)
      {
        if ((int) stringHash != 292886277)
        {
          if ((int) stringHash != 613168024)
          {
            if ((int) stringHash == 889009852 && name == "Chicken")
              return Game1.content.LoadString("Strings\\StringsFromCSFiles:PurchaseAnimalsMenu.cs.11334") + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:PurchaseAnimalsMenu.cs.11335");
          }
          else if (name == "Duck")
            return Game1.content.LoadString("Strings\\StringsFromCSFiles:PurchaseAnimalsMenu.cs.11337") + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:PurchaseAnimalsMenu.cs.11335");
        }
        else if (name == "Rabbit")
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:PurchaseAnimalsMenu.cs.11340") + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:PurchaseAnimalsMenu.cs.11335");
      }
      else if (stringHash <= 2392565758U)
      {
        if ((int) stringHash != 1601263067)
        {
          if ((int) stringHash == -1902401538 && name == "Goat")
            return Game1.content.LoadString("Strings\\StringsFromCSFiles:PurchaseAnimalsMenu.cs.11349") + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:PurchaseAnimalsMenu.cs.11344");
        }
        else if (name == "Dairy Cow")
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:PurchaseAnimalsMenu.cs.11343") + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:PurchaseAnimalsMenu.cs.11344");
      }
      else if ((int) stringHash != -1342046060)
      {
        if ((int) stringHash == -850948305 && name == "Pig")
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:PurchaseAnimalsMenu.cs.11346") + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:PurchaseAnimalsMenu.cs.11344");
      }
      else if (name == "Sheep")
        return Game1.content.LoadString("Strings\\StringsFromCSFiles:PurchaseAnimalsMenu.cs.11352") + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:PurchaseAnimalsMenu.cs.11344");
      return "";
    }

    public override void draw(SpriteBatch b)
    {
      if (!this.onFarm && !Game1.dialogueUp && !Game1.globalFade)
      {
        b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.75f);
        SpriteText.drawStringWithScrollBackground(b, Game1.content.LoadString("Strings\\StringsFromCSFiles:PurchaseAnimalsMenu.cs.11354"), this.xPositionOnScreen + Game1.tileSize * 3 / 2, this.yPositionOnScreen, "", 1f, -1);
        Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true, (string) null, false);
        Game1.dayTimeMoneyBox.drawMoneyBox(b, -1, -1);
        foreach (ClickableTextureComponent textureComponent in this.animalsToPurchase)
          textureComponent.draw(b, (textureComponent.item as StardewValley.Object).type != null ? Color.Black * 0.4f : Color.White, 0.87f);
      }
      else if (!Game1.globalFade && this.onFarm)
      {
        string s = Game1.content.LoadString("Strings\\StringsFromCSFiles:PurchaseAnimalsMenu.cs.11355", (object) this.animalBeingPurchased.displayHouse, (object) this.animalBeingPurchased.displayType);
        SpriteText.drawStringWithScrollBackground(b, s, Game1.viewport.Width / 2 - SpriteText.getWidthOfString(s) / 2, Game1.tileSize / 4, "", 1f, -1);
        if (this.namingAnimal)
        {
          b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.75f);
          Game1.drawDialogueBox(Game1.viewport.Width / 2 - Game1.tileSize * 4, Game1.viewport.Height / 2 - Game1.tileSize * 3 - Game1.tileSize / 2, Game1.tileSize * 8, Game1.tileSize * 3, false, true, (string) null, false);
          Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\StringsFromCSFiles:PurchaseAnimalsMenu.cs.11357"), Game1.dialogueFont, new Vector2((float) (Game1.viewport.Width / 2 - Game1.tileSize * 4 + Game1.tileSize / 2 + 8), (float) (Game1.viewport.Height / 2 - Game1.tileSize * 2 + 8)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
          this.textBox.Draw(b);
          this.doneNamingButton.draw(b);
          this.randomButton.draw(b);
        }
      }
      if (!Game1.globalFade && this.okButton != null)
        this.okButton.draw(b);
      if (this.hovered != null)
      {
        if ((this.hovered.item as StardewValley.Object).type != null)
        {
          IClickableMenu.drawHoverText(b, Game1.parseText((this.hovered.item as StardewValley.Object).type, Game1.dialogueFont, Game1.tileSize * 5), Game1.dialogueFont, 0, 0, -1, (string) null, -1, (string[]) null, (Item) null, 0, -1, -1, -1, -1, 1f, (CraftingRecipe) null);
        }
        else
        {
          string animalTitle = PurchaseAnimalsMenu.getAnimalTitle(this.hovered.hoverText);
          SpriteText.drawStringWithScrollBackground(b, animalTitle, this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize, this.yPositionOnScreen + this.height + -Game1.tileSize / 2 + IClickableMenu.spaceToClearTopBorder / 2 + 8, "Truffle Pig", 1f, -1);
          SpriteText.drawStringWithScrollBackground(b, "$" + Game1.content.LoadString("Strings\\StringsFromCSFiles:LoadGameMenu.cs.11020", (object) this.hovered.item.salePrice()), this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize * 2, this.yPositionOnScreen + this.height + Game1.tileSize + IClickableMenu.spaceToClearTopBorder / 2 + 8, "$99999999g", (float) (Game1.player.Money >= this.hovered.item.salePrice() ? 1.0 : 0.5), -1);
          string animalDescription = PurchaseAnimalsMenu.getAnimalDescription(this.hovered.hoverText);
          IClickableMenu.drawHoverText(b, Game1.parseText(animalDescription, Game1.smallFont, Game1.tileSize * 5), Game1.smallFont, 0, 0, -1, animalTitle, -1, (string[]) null, (Item) null, 0, -1, -1, -1, -1, 1f, (CraftingRecipe) null);
        }
      }
      Game1.mouseCursorTransparency = 1f;
      this.drawMouse(b);
    }
  }
}
