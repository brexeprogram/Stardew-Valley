// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.AnimalQueryMenu
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.Buildings;
using System;
using System.Collections.Generic;
using System.Linq;
using xTile.Dimensions;

namespace StardewValley.Menus
{
  public class AnimalQueryMenu : IClickableMenu
  {
    public new static int width = Game1.tileSize * 6;
    public new static int height = Game1.tileSize * 8;
    private string hoverText = "";
    public const int region_okButton = 101;
    public const int region_love = 102;
    public const int region_sellButton = 103;
    public const int region_moveHomeButton = 104;
    public const int region_noButton = 105;
    public const int region_allowReproductionButton = 106;
    public const int region_fullnessHover = 107;
    public const int region_happinessHover = 108;
    public const int region_loveHover = 109;
    public const int region_textBoxCC = 110;
    private FarmAnimal animal;
    private TextBox textBox;
    private TextBoxEvent e;
    public ClickableTextureComponent okButton;
    public ClickableTextureComponent love;
    public ClickableTextureComponent sellButton;
    public ClickableTextureComponent moveHomeButton;
    public ClickableTextureComponent yesButton;
    public ClickableTextureComponent noButton;
    public ClickableTextureComponent allowReproductionButton;
    public ClickableComponent fullnessHover;
    public ClickableComponent happinessHover;
    public ClickableComponent loveHover;
    public ClickableComponent textBoxCC;
    private double fullnessLevel;
    private double happinessLevel;
    private double loveLevel;
    private bool confirmingSell;
    private bool movingAnimal;
    private string parentName;

    public AnimalQueryMenu(FarmAnimal animal)
      : base(Game1.viewport.Width / 2 - AnimalQueryMenu.width / 2, Game1.viewport.Height / 2 - AnimalQueryMenu.height / 2, AnimalQueryMenu.width, AnimalQueryMenu.height, false)
    {
      Game1.player.Halt();
      Game1.player.faceGeneralDirection(animal.position, 0);
      AnimalQueryMenu.width = Game1.tileSize * 6;
      AnimalQueryMenu.height = Game1.tileSize * 8;
      this.animal = animal;
      this.textBox = new TextBox((Texture2D) null, (Texture2D) null, Game1.dialogueFont, Game1.textColor);
      this.textBox.X = Game1.viewport.Width / 2 - Game1.tileSize * 2 - 12;
      this.textBox.Y = this.yPositionOnScreen - 4 + Game1.tileSize * 2;
      this.textBox.Width = Game1.tileSize * 4;
      this.textBox.Height = Game1.tileSize * 3;
      this.textBoxCC = new ClickableComponent(new Microsoft.Xna.Framework.Rectangle(this.textBox.X, this.textBox.Y, this.textBox.Width, Game1.tileSize), "")
      {
        myID = 110,
        downNeighborID = 104
      };
      this.textBox.Text = animal.displayName;
      Game1.keyboardDispatcher.Subscriber = (IKeyboardSubscriber) this.textBox;
      this.textBox.Selected = false;
      if (animal.parentId != -1L)
      {
        FarmAnimal animal1 = Utility.getAnimal(animal.parentId);
        if (animal1 != null)
          this.parentName = animal1.displayName;
      }
      if (animal.sound != null && Game1.soundBank != null)
      {
        Cue cue = Game1.soundBank.GetCue(animal.sound);
        string name = "Pitch";
        double num = (double) (1200 + Game1.random.Next(-200, 201));
        cue.SetVariable(name, (float) num);
        cue.Play();
      }
      ClickableTextureComponent textureComponent1 = new ClickableTextureComponent(new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + AnimalQueryMenu.width + 4, this.yPositionOnScreen + AnimalQueryMenu.height - Game1.tileSize - IClickableMenu.borderWidth, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f, false);
      int num1 = 101;
      textureComponent1.myID = num1;
      int num2 = 103;
      textureComponent1.upNeighborID = num2;
      this.okButton = textureComponent1;
      ClickableTextureComponent textureComponent2 = new ClickableTextureComponent(new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + AnimalQueryMenu.width + 4, this.yPositionOnScreen + AnimalQueryMenu.height - Game1.tileSize * 3 - IClickableMenu.borderWidth, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(0, 384, 16, 16), 4f, false);
      int num3 = 103;
      textureComponent2.myID = num3;
      int num4 = 101;
      textureComponent2.downNeighborID = num4;
      int num5 = 104;
      textureComponent2.upNeighborID = num5;
      this.sellButton = textureComponent2;
      ClickableTextureComponent textureComponent3 = new ClickableTextureComponent(new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + AnimalQueryMenu.width + 4, this.yPositionOnScreen + AnimalQueryMenu.height - Game1.tileSize * 4 - IClickableMenu.borderWidth, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(16, 384, 16, 16), 4f, false);
      int num6 = 104;
      textureComponent3.myID = num6;
      int num7 = 103;
      textureComponent3.downNeighborID = num7;
      int num8 = 110;
      textureComponent3.upNeighborID = num8;
      this.moveHomeButton = textureComponent3;
      if (!animal.isBaby() && !animal.isCoopDweller())
      {
        ClickableTextureComponent textureComponent4 = new ClickableTextureComponent(new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + AnimalQueryMenu.width + Game1.pixelZoom * 4, this.yPositionOnScreen + AnimalQueryMenu.height - Game1.tileSize * 2 - IClickableMenu.borderWidth + Game1.pixelZoom * 2, Game1.pixelZoom * 9, Game1.pixelZoom * 9), Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(animal.allowReproduction ? 128 : 137, 393, 9, 9), 4f, false);
        int num9 = 106;
        textureComponent4.myID = num9;
        this.allowReproductionButton = textureComponent4;
      }
      ClickableTextureComponent textureComponent5 = new ClickableTextureComponent((Math.Round((double) animal.friendshipTowardFarmer, 0) / 10.0).ToString() + "<", new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2 + 16, this.yPositionOnScreen - Game1.tileSize / 2 + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 4 - Game1.tileSize / 2, AnimalQueryMenu.width - Game1.tileSize * 2, Game1.tileSize), (string) null, "Friendship", Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(172, 512, 16, 16), 4f, false);
      int num10 = 102;
      textureComponent5.myID = num10;
      this.love = textureComponent5;
      this.loveHover = new ClickableComponent(new Microsoft.Xna.Framework.Rectangle(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder, this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize * 3 - Game1.tileSize / 2, AnimalQueryMenu.width, Game1.tileSize), "Friendship")
      {
        myID = 109
      };
      this.fullnessLevel = (double) animal.fullness / (double) byte.MaxValue;
      if (animal.home != null && animal.home.indoors != null)
      {
        int num9 = animal.home.indoors.numberOfObjectsWithName("Hay");
        if (num9 > 0)
        {
          int count = (animal.home.indoors as AnimalHouse).animalsThatLiveHere.Count;
          this.fullnessLevel = Math.Min(1.0, this.fullnessLevel + (double) num9 / (double) count);
        }
      }
      else
        Utility.fixAllAnimals();
      this.happinessLevel = (double) animal.happiness / (double) byte.MaxValue;
      this.loveLevel = (double) animal.friendshipTowardFarmer / 1000.0;
      if (!Game1.options.SnappyMenus)
        return;
      this.populateClickableComponentList();
      this.snapToDefaultClickableComponent();
    }

    public override void snapToDefaultClickableComponent()
    {
      this.currentlySnappedComponent = this.getComponentWithID(101);
      this.snapCursorToCurrentSnappedComponent();
    }

    public void textBoxEnter(TextBox sender)
    {
    }

    public override void receiveKeyPress(Keys key)
    {
      if (Game1.globalFade)
        return;
      if (((IEnumerable<InputButton>) Game1.options.menuButton).Contains<InputButton>(new InputButton(key)) && (this.textBox == null || !this.textBox.Selected))
      {
        Game1.playSound("smallSelect");
        if (this.readyToClose())
        {
          Game1.exitActiveMenu();
          if (this.textBox.Text.Length <= 0 || Utility.areThereAnyOtherAnimalsWithThisName(this.textBox.Text))
            return;
          this.animal.displayName = this.textBox.Text;
          this.animal.name = this.textBox.Text;
        }
        else
        {
          if (!this.movingAnimal)
            return;
          Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.prepareForReturnFromPlacement), 0.02f);
        }
      }
      else
      {
        if (!Game1.options.SnappyMenus || ((IEnumerable<InputButton>) Game1.options.menuButton).Contains<InputButton>(new InputButton(key)) && this.textBox != null && this.textBox.Selected)
          return;
        base.receiveKeyPress(key);
      }
    }

    public override void update(GameTime time)
    {
      base.update(time);
      if (!this.movingAnimal)
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

    public void finishedPlacingAnimal()
    {
      Game1.exitActiveMenu();
      Game1.currentLocation = Game1.player.currentLocation;
      Game1.currentLocation.resetForPlayerEntry();
      Game1.globalFadeToClear((Game1.afterFadeFunction) null, 0.02f);
      Game1.displayHUD = true;
      Game1.viewportFreeze = false;
      Game1.displayFarmer = true;
      Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\UI:AnimalQuery_Moving_HomeChanged"), Color.LimeGreen, 3500f));
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
      if (Game1.globalFade)
        return;
      if (this.movingAnimal)
      {
        if (this.okButton != null && this.okButton.containsPoint(x, y))
        {
          Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.prepareForReturnFromPlacement), 0.02f);
          Game1.playSound("smallSelect");
        }
        Building buildingAt = (Game1.getLocationFromName("Farm") as Farm).getBuildingAt(new Vector2((float) ((x + Game1.viewport.X) / Game1.tileSize), (float) ((y + Game1.viewport.Y) / Game1.tileSize)));
        if (buildingAt == null)
          return;
        if (buildingAt.buildingType.Contains(this.animal.buildingTypeILiveIn))
        {
          if ((buildingAt.indoors as AnimalHouse).isFull())
            Game1.showRedMessage(Game1.content.LoadString("Strings\\UI:AnimalQuery_Moving_BuildingFull"));
          else if (buildingAt.Equals((object) this.animal.home))
          {
            Game1.showRedMessage(Game1.content.LoadString("Strings\\UI:AnimalQuery_Moving_AlreadyHome"));
          }
          else
          {
            (this.animal.home.indoors as AnimalHouse).animalsThatLiveHere.Remove(this.animal.myID);
            if ((this.animal.home.indoors as AnimalHouse).animals.ContainsKey(this.animal.myID))
            {
              (buildingAt.indoors as AnimalHouse).animals.Add(this.animal.myID, this.animal);
              (this.animal.home.indoors as AnimalHouse).animals.Remove(this.animal.myID);
            }
            this.animal.home = buildingAt;
            this.animal.homeLocation = new Vector2((float) buildingAt.tileX, (float) buildingAt.tileY);
            (buildingAt.indoors as AnimalHouse).animalsThatLiveHere.Add(this.animal.myID);
            this.animal.makeSound();
            Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.finishedPlacingAnimal), 0.02f);
          }
        }
        else
          Game1.showRedMessage(Game1.content.LoadString("Strings\\UI:AnimalQuery_Moving_CantLiveThere", (object) this.animal.shortDisplayType()));
      }
      else if (this.confirmingSell)
      {
        if (this.yesButton.containsPoint(x, y))
        {
          Game1.player.money += this.animal.getSellPrice();
          (this.animal.home.indoors as AnimalHouse).animalsThatLiveHere.Remove(this.animal.myID);
          this.animal.health = -1;
          int num1 = this.animal.frontBackSourceRect.Width / 2;
          for (int index = 0; index < num1; ++index)
          {
            int num2 = Game1.random.Next(25, 200);
            Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(5, this.animal.position + new Vector2((float) Game1.random.Next(-Game1.tileSize / 2, this.animal.frontBackSourceRect.Width * 3), (float) Game1.random.Next(-Game1.tileSize / 2, this.animal.frontBackSourceRect.Height * 3)), new Color((int) byte.MaxValue - num2, (int) byte.MaxValue, (int) byte.MaxValue - num2), 8, false, Game1.random.NextDouble() < 0.5 ? 50f : (float) Game1.random.Next(30, 200), 0, Game1.tileSize, -1f, Game1.tileSize, Game1.random.NextDouble() < 0.5 ? 0 : Game1.random.Next(0, 600))
            {
              scale = (float) ((double) Game1.random.Next(2, 5) * 0.25),
              alpha = (float) ((double) Game1.random.Next(2, 5) * 0.25),
              motion = new Vector2(0.0f, (float) -Game1.random.NextDouble())
            });
          }
          Game1.playSound("newRecipe");
          Game1.playSound("money");
          Game1.exitActiveMenu();
        }
        else
        {
          if (!this.noButton.containsPoint(x, y))
            return;
          this.confirmingSell = false;
          Game1.playSound("smallSelect");
          if (!Game1.options.SnappyMenus)
            return;
          this.currentlySnappedComponent = this.getComponentWithID(103);
          this.snapCursorToCurrentSnappedComponent();
        }
      }
      else
      {
        if (this.okButton != null && this.okButton.containsPoint(x, y) && this.readyToClose())
        {
          Game1.exitActiveMenu();
          if (this.textBox.Text.Length > 0 && !Utility.areThereAnyOtherAnimalsWithThisName(this.textBox.Text))
          {
            this.animal.displayName = this.textBox.Text;
            this.animal.name = this.textBox.Text;
          }
          Game1.playSound("smallSelect");
        }
        if (this.sellButton.containsPoint(x, y))
        {
          this.confirmingSell = true;
          ClickableTextureComponent textureComponent1 = new ClickableTextureComponent(new Microsoft.Xna.Framework.Rectangle(Game1.viewport.Width / 2 - Game1.tileSize - 4, Game1.viewport.Height / 2 - Game1.tileSize / 2, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f, false);
          int num1 = 111;
          textureComponent1.myID = num1;
          int num2 = 105;
          textureComponent1.rightNeighborID = num2;
          this.yesButton = textureComponent1;
          ClickableTextureComponent textureComponent2 = new ClickableTextureComponent(new Microsoft.Xna.Framework.Rectangle(Game1.viewport.Width / 2 + 4, Game1.viewport.Height / 2 - Game1.tileSize / 2, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 47, -1, -1), 1f, false);
          int num3 = 105;
          textureComponent2.myID = num3;
          int num4 = 111;
          textureComponent2.leftNeighborID = num4;
          this.noButton = textureComponent2;
          Game1.playSound("smallSelect");
          if (!Game1.options.SnappyMenus)
            return;
          this.populateClickableComponentList();
          this.currentlySnappedComponent = (ClickableComponent) this.noButton;
          this.snapCursorToCurrentSnappedComponent();
        }
        else
        {
          if (this.moveHomeButton.containsPoint(x, y))
          {
            Game1.playSound("smallSelect");
            Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.prepareForAnimalPlacement), 0.02f);
          }
          if (this.allowReproductionButton != null && this.allowReproductionButton.containsPoint(x, y))
          {
            Game1.playSound("drumkit6");
            this.animal.allowReproduction = !this.animal.allowReproduction;
            this.allowReproductionButton.sourceRect.X = !this.animal.allowReproduction ? 137 : 128;
          }
          this.textBox.Update();
        }
      }
    }

    public override bool overrideSnappyMenuCursorMovementBan()
    {
      return this.movingAnimal;
    }

    public void prepareForAnimalPlacement()
    {
      this.movingAnimal = true;
      Game1.currentLocation = Game1.getLocationFromName("Farm");
      Game1.globalFadeToClear((Game1.afterFadeFunction) null, 0.02f);
      this.okButton.bounds.X = Game1.viewport.Width - Game1.tileSize * 2;
      this.okButton.bounds.Y = Game1.viewport.Height - Game1.tileSize * 2;
      Game1.displayHUD = false;
      Game1.viewportFreeze = true;
      Game1.viewport.Location = new Location(49 * Game1.tileSize, 5 * Game1.tileSize);
      Game1.panScreen(0, 0);
      Game1.currentLocation.resetForPlayerEntry();
      Game1.displayFarmer = false;
    }

    public void prepareForReturnFromPlacement()
    {
      Game1.currentLocation = Game1.player.currentLocation;
      Game1.currentLocation.resetForPlayerEntry();
      Game1.globalFadeToClear((Game1.afterFadeFunction) null, 0.02f);
      this.okButton.bounds.X = this.xPositionOnScreen + AnimalQueryMenu.width + 4;
      this.okButton.bounds.Y = this.yPositionOnScreen + AnimalQueryMenu.height - Game1.tileSize - IClickableMenu.borderWidth;
      Game1.displayHUD = true;
      Game1.viewportFreeze = false;
      Game1.displayFarmer = true;
      this.movingAnimal = false;
    }

    public override bool readyToClose()
    {
      this.textBox.Selected = false;
      if (base.readyToClose() && !this.movingAnimal)
        return !Game1.globalFade;
      return false;
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
      if (Game1.globalFade)
        return;
      if (this.readyToClose())
      {
        Game1.exitActiveMenu();
        if (this.textBox.Text.Length > 0 && !Utility.areThereAnyOtherAnimalsWithThisName(this.textBox.Text))
        {
          this.animal.displayName = this.textBox.Text;
          this.animal.name = this.textBox.Text;
        }
        Game1.playSound("smallSelect");
      }
      else
      {
        if (!this.movingAnimal)
          return;
        Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.prepareForReturnFromPlacement), 0.02f);
      }
    }

    public override void performHoverAction(int x, int y)
    {
      this.hoverText = "";
      if (this.movingAnimal)
      {
        Vector2 tile = new Vector2((float) ((x + Game1.viewport.X) / Game1.tileSize), (float) ((y + Game1.viewport.Y) / Game1.tileSize));
        Farm locationFromName = Game1.getLocationFromName("Farm") as Farm;
        foreach (Building building in locationFromName.buildings)
          building.color = Color.White;
        Building buildingAt = locationFromName.getBuildingAt(tile);
        if (buildingAt != null)
          buildingAt.color = !buildingAt.buildingType.Contains(this.animal.buildingTypeILiveIn) || (buildingAt.indoors as AnimalHouse).isFull() || buildingAt.Equals((object) this.animal.home) ? Color.Red * 0.8f : Color.LightGreen * 0.8f;
      }
      if (this.okButton != null)
      {
        if (this.okButton.containsPoint(x, y))
          this.okButton.scale = Math.Min(1.1f, this.okButton.scale + 0.05f);
        else
          this.okButton.scale = Math.Max(1f, this.okButton.scale - 0.05f);
      }
      if (this.sellButton != null)
      {
        if (this.sellButton.containsPoint(x, y))
        {
          this.sellButton.scale = Math.Min(4.1f, this.sellButton.scale + 0.05f);
          this.hoverText = Game1.content.LoadString("Strings\\UI:AnimalQuery_Sell", (object) this.animal.getSellPrice());
        }
        else
          this.sellButton.scale = Math.Max(4f, this.sellButton.scale - 0.05f);
      }
      if (this.moveHomeButton != null)
      {
        if (this.moveHomeButton.containsPoint(x, y))
        {
          this.moveHomeButton.scale = Math.Min(4.1f, this.moveHomeButton.scale + 0.05f);
          this.hoverText = Game1.content.LoadString("Strings\\UI:AnimalQuery_Move");
        }
        else
          this.moveHomeButton.scale = Math.Max(4f, this.moveHomeButton.scale - 0.05f);
      }
      if (this.allowReproductionButton != null)
      {
        if (this.allowReproductionButton.containsPoint(x, y))
        {
          this.allowReproductionButton.scale = Math.Min(4.1f, this.allowReproductionButton.scale + 0.05f);
          this.hoverText = Game1.content.LoadString("Strings\\UI:AnimalQuery_AllowReproduction");
        }
        else
          this.allowReproductionButton.scale = Math.Max(4f, this.allowReproductionButton.scale - 0.05f);
      }
      if (this.yesButton != null)
      {
        if (this.yesButton.containsPoint(x, y))
          this.yesButton.scale = Math.Min(1.1f, this.yesButton.scale + 0.05f);
        else
          this.yesButton.scale = Math.Max(1f, this.yesButton.scale - 0.05f);
      }
      if (this.noButton == null)
        return;
      if (this.noButton.containsPoint(x, y))
        this.noButton.scale = Math.Min(1.1f, this.noButton.scale + 0.05f);
      else
        this.noButton.scale = Math.Max(1f, this.noButton.scale - 0.05f);
    }

    public override void draw(SpriteBatch b)
    {
      if (!this.movingAnimal && !Game1.globalFade)
      {
        b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.75f);
        Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen + Game1.tileSize * 2, AnimalQueryMenu.width, AnimalQueryMenu.height - Game1.tileSize * 2, false, true, (string) null, false);
        if ((int) this.animal.harvestType != 2)
          this.textBox.Draw(b);
        int num1 = (this.animal.age + 1) / 28 + 1;
        string text1;
        if (num1 > 1)
          text1 = Game1.content.LoadString("Strings\\UI:AnimalQuery_AgeN", (object) num1);
        else
          text1 = Game1.content.LoadString("Strings\\UI:AnimalQuery_Age1");
        if (this.animal.age < (int) this.animal.ageWhenMature)
          text1 += Game1.content.LoadString("Strings\\UI:AnimalQuery_AgeBaby");
        Utility.drawTextWithShadow(b, text1, Game1.smallFont, new Vector2((float) (this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2), (float) (this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 4 + Game1.tileSize * 2)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
        int num2 = 0;
        if (this.parentName != null)
        {
          num2 = Game1.tileSize / 3;
          Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:AnimalQuery_Parent", (object) this.parentName), Game1.smallFont, new Vector2((float) (this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2), (float) (Game1.tileSize / 2 + this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + Game1.tileSize / 4 + Game1.tileSize * 2)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
        }
        int num3 = this.loveLevel * 1000.0 % 200.0 >= 100.0 ? (int) (this.loveLevel * 1000.0 / 200.0) : -100;
        for (int index = 0; index < 5; ++index)
        {
          b.Draw(Game1.mouseCursors, new Vector2((float) (this.xPositionOnScreen + Game1.tileSize * 3 / 2 + 8 * Game1.pixelZoom * index), (float) (num2 + this.yPositionOnScreen - Game1.tileSize / 2 + Game1.tileSize * 5)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(211 + (this.loveLevel * 1000.0 <= (double) ((index + 1) * 195) ? 7 : 0), 428, 7, 6)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.89f);
          if (num3 == index)
            b.Draw(Game1.mouseCursors, new Vector2((float) (this.xPositionOnScreen + Game1.tileSize * 3 / 2 + 8 * Game1.pixelZoom * index), (float) (num2 + this.yPositionOnScreen - Game1.tileSize / 2 + Game1.tileSize * 5)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(211, 428, 4, 6)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.891f);
        }
        Utility.drawTextWithShadow(b, Game1.parseText(this.animal.getMoodMessage(), Game1.smallFont, AnimalQueryMenu.width - IClickableMenu.spaceToClearSideBorder * 2 - Game1.tileSize), Game1.smallFont, new Vector2((float) (this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2), (float) (num2 + this.yPositionOnScreen + Game1.tileSize * 6 - Game1.tileSize + Game1.pixelZoom)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
        this.okButton.draw(b);
        this.sellButton.draw(b);
        this.moveHomeButton.draw(b);
        if (this.allowReproductionButton != null)
          this.allowReproductionButton.draw(b);
        if (this.confirmingSell)
        {
          b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.75f);
          Game1.drawDialogueBox(Game1.viewport.Width / 2 - Game1.tileSize * 5 / 2, Game1.viewport.Height / 2 - Game1.tileSize * 3, Game1.tileSize * 5, Game1.tileSize * 4, false, true, (string) null, false);
          string text2 = Game1.content.LoadString("Strings\\UI:AnimalQuery_ConfirmSell");
          b.DrawString(Game1.dialogueFont, text2, new Vector2((float) (Game1.viewport.Width / 2) - Game1.dialogueFont.MeasureString(text2).X / 2f, (float) (Game1.viewport.Height / 2 - Game1.tileSize * 3 / 2 + 8)), Game1.textColor);
          this.yesButton.draw(b);
          this.noButton.draw(b);
        }
        else if (this.hoverText != null && this.hoverText.Length > 0)
          IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, -1, (string) null, -1, (string[]) null, (Item) null, 0, -1, -1, -1, -1, 1f, (CraftingRecipe) null);
      }
      else if (!Game1.globalFade)
      {
        string text = Game1.content.LoadString("Strings\\UI:AnimalQuery_ChooseBuilding", (object) this.animal.displayHouse, (object) this.animal.displayType);
        Game1.drawDialogueBox(Game1.tileSize / 2, -Game1.tileSize, (int) Game1.dialogueFont.MeasureString(text).X + IClickableMenu.borderWidth * 2 + Game1.tileSize / 4, Game1.tileSize * 2 + IClickableMenu.borderWidth * 2, false, true, (string) null, false);
        b.DrawString(Game1.dialogueFont, text, new Vector2((float) (Game1.tileSize / 2 + IClickableMenu.spaceToClearSideBorder * 2 + 8), (float) (Game1.tileSize / 2 + Game1.pixelZoom * 3)), Game1.textColor);
        this.okButton.draw(b);
      }
      this.drawMouse(b);
    }
  }
}
