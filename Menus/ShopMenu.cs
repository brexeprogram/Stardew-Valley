// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.ShopMenu
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Locations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Menus
{
  public class ShopMenu : IClickableMenu
  {
    private string descriptionText = "";
    private string hoverText = "";
    private string boldTitleText = "";
    private List<Item> forSale = new List<Item>();
    public List<ClickableComponent> forSaleButtons = new List<ClickableComponent>();
    private List<int> categoriesToSellHere = new List<int>();
    private Dictionary<Item, int[]> itemPriceAndStock = new Dictionary<Item, int[]>();
    private float sellPercentage = 1f;
    private List<TemporaryAnimatedSprite> animations = new List<TemporaryAnimatedSprite>();
    private int hoverPrice = -1;
    public const int region_shopButtonModifier = 3546;
    public const int region_upArrow = 97865;
    public const int region_downArrow = 97866;
    public const int howManyRecipesFitOnPage = 28;
    public const int infiniteStock = 2147483647;
    public const int salePriceIndex = 0;
    public const int stockIndex = 1;
    public const int extraTradeItemIndex = 2;
    public const int itemsPerPage = 4;
    public const int numberRequiredForExtraItemTrade = 5;
    public InventoryMenu inventory;
    private Item heldItem;
    private Item hoveredItem;
    private Texture2D wallpapers;
    private Texture2D floors;
    private int lastWallpaperFloorPrice;
    private TemporaryAnimatedSprite poof;
    private Rectangle scrollBarRunner;
    private int currency;
    private int currentItemIndex;
    public ClickableTextureComponent upArrow;
    public ClickableTextureComponent downArrow;
    public ClickableTextureComponent scrollBar;
    public NPC portraitPerson;
    public string potraitPersonDialogue;
    private bool scrolling;

    public ShopMenu(Dictionary<Item, int[]> itemPriceAndStock, int currency = 0, string who = null)
      : this(itemPriceAndStock.Keys.ToList<Item>(), currency, who)
    {
      this.itemPriceAndStock = itemPriceAndStock;
      if (this.potraitPersonDialogue != null)
        return;
      this.setUpShopOwner(who);
    }

    public ShopMenu(List<Item> itemsForSale, int currency = 0, string who = null)
      : base(Game1.viewport.Width / 2 - (800 + IClickableMenu.borderWidth * 2) / 2, Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2, 1000 + IClickableMenu.borderWidth * 2, 600 + IClickableMenu.borderWidth * 2, true)
    {
      this.currency = currency;
      if (Game1.viewport.Width < 1500)
        this.xPositionOnScreen = Game1.tileSize / 2;
      Game1.player.forceCanMove();
      Game1.playSound("dwop");
      this.inventory = new InventoryMenu(this.xPositionOnScreen + this.width, this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth + Game1.tileSize * 5 + Game1.pixelZoom * 10, false, (List<Item>) null, new InventoryMenu.highlightThisItem(this.highlightItemToSell), -1, 3, 0, 0, true)
      {
        showGrayedOutSlots = true
      };
      this.inventory.movePosition(-this.inventory.width - Game1.tileSize / 2, 0);
      this.currency = currency;
      int positionOnScreen1 = this.xPositionOnScreen;
      int borderWidth1 = IClickableMenu.borderWidth;
      int toClearSideBorder = IClickableMenu.spaceToClearSideBorder;
      int positionOnScreen2 = this.yPositionOnScreen;
      int borderWidth2 = IClickableMenu.borderWidth;
      int toClearTopBorder = IClickableMenu.spaceToClearTopBorder;
      ClickableTextureComponent textureComponent1 = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize / 4, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 459, 11, 12), (float) Game1.pixelZoom, false);
      int num1 = 97865;
      textureComponent1.myID = num1;
      int num2 = 106;
      textureComponent1.downNeighborID = num2;
      int num3 = 3546;
      textureComponent1.leftNeighborID = num3;
      this.upArrow = textureComponent1;
      ClickableTextureComponent textureComponent2 = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize / 4, this.yPositionOnScreen + this.height - Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 472, 11, 12), (float) Game1.pixelZoom, false);
      int num4 = 106;
      textureComponent2.myID = num4;
      int num5 = 97865;
      textureComponent2.upNeighborID = num5;
      int num6 = 3546;
      textureComponent2.leftNeighborID = num6;
      this.downArrow = textureComponent2;
      this.scrollBar = new ClickableTextureComponent(new Rectangle(this.upArrow.bounds.X + Game1.pixelZoom * 3, this.upArrow.bounds.Y + this.upArrow.bounds.Height + Game1.pixelZoom, 6 * Game1.pixelZoom, 10 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(435, 463, 6, 10), (float) Game1.pixelZoom, false);
      this.scrollBarRunner = new Rectangle(this.scrollBar.bounds.X, this.upArrow.bounds.Y + this.upArrow.bounds.Height + Game1.pixelZoom, this.scrollBar.bounds.Width, this.height - Game1.tileSize - this.upArrow.bounds.Height - Game1.pixelZoom * 7);
      for (int index = 0; index < 4; ++index)
      {
        List<ClickableComponent> forSaleButtons = this.forSaleButtons;
        ClickableComponent clickableComponent = new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize / 4 + index * ((this.height - Game1.tileSize * 4) / 4), this.width - Game1.tileSize / 2, (this.height - Game1.tileSize * 4) / 4 + Game1.pixelZoom), string.Concat((object) index));
        clickableComponent.myID = index + 3546;
        clickableComponent.upNeighborID = index > 0 ? index + 3546 - 1 : -7777;
        clickableComponent.downNeighborID = index >= 3 || index >= itemsForSale.Count ? -7777 : index + 3546 + 1;
        clickableComponent.rightNeighborID = 97865;
        int num7 = 1;
        clickableComponent.fullyImmutable = num7 != 0;
        forSaleButtons.Add(clickableComponent);
      }
      foreach (Item key in itemsForSale)
      {
        if (key is StardewValley.Object && (key as StardewValley.Object).isRecipe)
        {
          if (!Game1.player.knowsRecipe(key.Name))
            key.Stack = 1;
          else
            continue;
        }
        this.forSale.Add(key);
        this.itemPriceAndStock.Add(key, new int[2]
        {
          key.salePrice(),
          key.Stack
        });
      }
      if (this.itemPriceAndStock.Count >= 2)
        this.setUpShopOwner(who);
      string name = Game1.currentLocation.name;
      if (!(name == "SeedShop"))
      {
        if (!(name == "Blacksmith"))
        {
          if (!(name == "ScienceHouse"))
          {
            if (!(name == "AnimalShop"))
            {
              if (!(name == "FishShop"))
              {
                if (name == "AdventureGuild")
                  this.categoriesToSellHere.AddRange((IEnumerable<int>) new int[4]
                  {
                    -28,
                    -98,
                    -97,
                    -96
                  });
              }
              else
                this.categoriesToSellHere.AddRange((IEnumerable<int>) new int[4]
                {
                  -4,
                  -23,
                  -21,
                  -22
                });
            }
            else
              this.categoriesToSellHere.AddRange((IEnumerable<int>) new int[4]
              {
                -18,
                -6,
                -5,
                -14
              });
          }
          else
            this.categoriesToSellHere.AddRange((IEnumerable<int>) new int[1]
            {
              -16
            });
        }
        else
          this.categoriesToSellHere.AddRange((IEnumerable<int>) new int[3]
          {
            -12,
            -2,
            -15
          });
      }
      else
        this.categoriesToSellHere.AddRange((IEnumerable<int>) new int[14]
        {
          -81,
          -75,
          -79,
          -80,
          -74,
          -17,
          -18,
          -6,
          -26,
          -5,
          -14,
          -19,
          -7,
          -25
        });
      Game1.currentLocation.Name.Equals("SeedShop");
      if (!Game1.options.snappyMenus || !Game1.options.gamepadControls)
        return;
      this.populateClickableComponentList();
      this.snapToDefaultClickableComponent();
    }

    protected override void customSnapBehavior(int direction, int oldRegion, int oldID)
    {
      if (direction == 2)
      {
        if (this.currentItemIndex < Math.Max(0, this.forSale.Count - 4))
        {
          this.currentItemIndex = this.currentItemIndex + 1;
        }
        else
        {
          int num = -1;
          for (int index = 0; index < 12; ++index)
          {
            this.inventory.inventory[index].upNeighborID = oldID;
            if (num == -1 && this.heldItem != null && (this.inventory.actualInventory != null && this.inventory.actualInventory.Count > index) && this.inventory.actualInventory[index] == null)
              num = index;
          }
          this.currentlySnappedComponent = this.getComponentWithID(num != -1 ? num : 0);
          this.snapCursorToCurrentSnappedComponent();
        }
      }
      else
      {
        if (direction != 0 || this.currentItemIndex <= 0)
          return;
        this.upArrowPressed();
        this.currentlySnappedComponent = this.getComponentWithID(3546);
        this.snapCursorToCurrentSnappedComponent();
      }
    }

    public override void snapToDefaultClickableComponent()
    {
      this.currentlySnappedComponent = this.getComponentWithID(3546);
      this.snapCursorToCurrentSnappedComponent();
    }

    public void setUpShopOwner(string who)
    {
      if (who == null)
        return;
      Random random = new Random((int) ((long) Game1.uniqueIDForThisGame + (long) Game1.stats.DaysPlayed));
      string text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11457");
      // ISSUE: reference to a compiler-generated method
      uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(who);
      if (stringHash <= 1771728057U)
      {
        if (stringHash <= 1305917497U)
        {
          if ((int) stringHash != 208794864)
          {
            if ((int) stringHash != 1089105211)
            {
              if ((int) stringHash == 1305917497 && who == "Krobus")
              {
                this.portraitPerson = Game1.getCharacterFromName("Krobus", false);
                text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11497");
              }
            }
            else if (who == "Dwarf")
            {
              this.portraitPerson = Game1.getCharacterFromName("Dwarf", false);
              text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11492");
            }
          }
          else if (who == "Pierre")
          {
            this.portraitPerson = Game1.getCharacterFromName("Pierre", false);
            switch (Game1.dayOfMonth % 7)
            {
              case 0:
                text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11487");
                break;
              case 1:
                text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11481");
                break;
              case 2:
                text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11482");
                break;
              case 3:
                text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11483");
                break;
              case 4:
                text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11484");
                break;
              case 5:
                text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11485");
                break;
              case 6:
                text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11486");
                break;
            }
            text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11488") + text;
            if (Game1.dayOfMonth == 28)
              text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11489");
          }
        }
        else if ((int) stringHash != 1409564722)
        {
          if ((int) stringHash != 1639180769)
          {
            if ((int) stringHash == 1771728057 && who == "ClintUpgrade")
            {
              this.portraitPerson = Game1.getCharacterFromName("Clint", false);
              text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11474");
            }
          }
          else if (who == "HatMouse")
            text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11494");
        }
        else if (who == "Traveler")
        {
          switch (random.Next(5))
          {
            case 0:
              text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11499");
              break;
            case 1:
              text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11500");
              break;
            case 2:
              text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11501");
              break;
            case 3:
              text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11502", (object) this.itemPriceAndStock.ElementAt<KeyValuePair<Item, int[]>>(random.Next(this.itemPriceAndStock.Count)).Key.DisplayName);
              break;
            case 4:
              text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11504");
              break;
          }
        }
      }
      else if (stringHash <= 2750361957U)
      {
        if ((int) stringHash != -1915364453)
        {
          if ((int) stringHash != -1583169328)
          {
            if ((int) stringHash == -1544605339 && who == "Marnie")
            {
              this.portraitPerson = Game1.getCharacterFromName("Marnie", false);
              text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11507");
              if (random.NextDouble() < 0.0001)
                text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11508");
            }
          }
          else if (who == "Marlon")
          {
            this.portraitPerson = Game1.getCharacterFromName("Marlon", false);
            switch (random.Next(4))
            {
              case 0:
                text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11517");
                break;
              case 1:
                text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11518");
                break;
              case 2:
                text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11519");
                break;
              case 3:
                text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11520");
                break;
            }
            if (random.NextDouble() < 0.001)
              text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11521");
          }
        }
        else if (who == "Robin")
        {
          this.portraitPerson = Game1.getCharacterFromName("Robin", false);
          switch (Game1.random.Next(5))
          {
            case 0:
              text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11460");
              break;
            case 1:
              text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11461");
              break;
            case 2:
              text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11462");
              break;
            case 3:
              text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11463");
              break;
            case 4:
              string displayName = this.itemPriceAndStock.ElementAt<KeyValuePair<Item, int[]>>(Game1.random.Next(2, this.itemPriceAndStock.Count)).Key.DisplayName;
              text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11464", (object) displayName, (object) Lexicon.getRandomPositiveAdjectiveForEventOrPerson((NPC) null), (object) Game1.getProperArticleForWord(displayName));
              break;
          }
        }
      }
      else if (stringHash <= 3818424508U)
      {
        if ((int) stringHash != -1279271762)
        {
          if ((int) stringHash == -476542788 && who == "Willy")
          {
            this.portraitPerson = Game1.getCharacterFromName("Willy", false);
            text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11477");
            if (Game1.random.NextDouble() < 0.05)
              text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11478");
          }
        }
        else if (who == "Gus")
        {
          this.portraitPerson = Game1.getCharacterFromName("Gus", false);
          switch (Game1.random.Next(4))
          {
            case 0:
              text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11511");
              break;
            case 1:
              text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11512", (object) this.itemPriceAndStock.ElementAt<KeyValuePair<Item, int[]>>(random.Next(this.itemPriceAndStock.Count)).Key.DisplayName);
              break;
            case 2:
              text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11513");
              break;
            case 3:
              text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11514");
              break;
          }
        }
      }
      else if ((int) stringHash != -449630045)
      {
        if ((int) stringHash == -100384626 && who == "Sandy")
        {
          this.portraitPerson = Game1.getCharacterFromName("Sandy", false);
          text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11524");
          if (random.NextDouble() < 0.0001)
            text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11525");
        }
      }
      else if (who == "Clint")
      {
        this.portraitPerson = Game1.getCharacterFromName("Clint", false);
        switch (Game1.random.Next(3))
        {
          case 0:
            text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11469");
            break;
          case 1:
            text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11470");
            break;
          case 2:
            text = Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11471");
            break;
        }
      }
      this.potraitPersonDialogue = Game1.parseText(text, Game1.dialogueFont, Game1.tileSize * 5 - Game1.pixelZoom * 4);
    }

    public bool highlightItemToSell(Item i)
    {
      return this.categoriesToSellHere.Contains(i.category);
    }

    public static int getPlayerCurrencyAmount(Farmer who, int currencyType)
    {
      switch (currencyType)
      {
        case 0:
          return who.Money;
        case 1:
          return who.festivalScore;
        case 2:
          return who.clubCoins;
        default:
          return 0;
      }
    }

    public override void leftClickHeld(int x, int y)
    {
      base.leftClickHeld(x, y);
      if (!this.scrolling)
        return;
      int y1 = this.scrollBar.bounds.Y;
      this.scrollBar.bounds.Y = Math.Min(this.yPositionOnScreen + this.height - Game1.tileSize - Game1.pixelZoom * 3 - this.scrollBar.bounds.Height, Math.Max(y, this.yPositionOnScreen + this.upArrow.bounds.Height + Game1.pixelZoom * 5));
      this.currentItemIndex = Math.Min(this.forSale.Count - 4, Math.Max(0, (int) ((double) this.forSale.Count * (double) ((float) (y - this.scrollBarRunner.Y) / (float) this.scrollBarRunner.Height))));
      this.setScrollBarToCurrentIndex();
      int y2 = this.scrollBar.bounds.Y;
      if (y1 == y2)
        return;
      Game1.playSound("shiny4");
    }

    public override void releaseLeftClick(int x, int y)
    {
      base.releaseLeftClick(x, y);
      this.scrolling = false;
    }

    private void setScrollBarToCurrentIndex()
    {
      if (this.forSale.Count <= 0)
        return;
      this.scrollBar.bounds.Y = this.scrollBarRunner.Height / Math.Max(1, this.forSale.Count - 4 + 1) * this.currentItemIndex + this.upArrow.bounds.Bottom + Game1.pixelZoom;
      if (this.currentItemIndex != this.forSale.Count - 4)
        return;
      this.scrollBar.bounds.Y = this.downArrow.bounds.Y - this.scrollBar.bounds.Height - Game1.pixelZoom;
    }

    public override void receiveScrollWheelAction(int direction)
    {
      base.receiveScrollWheelAction(direction);
      if (direction > 0 && this.currentItemIndex > 0)
      {
        this.upArrowPressed();
        Game1.playSound("shiny4");
      }
      else
      {
        if (direction >= 0 || this.currentItemIndex >= Math.Max(0, this.forSale.Count - 4))
          return;
        this.downArrowPressed();
        Game1.playSound("shiny4");
      }
    }

    private void downArrowPressed()
    {
      this.downArrow.scale = this.downArrow.baseScale;
      this.currentItemIndex = this.currentItemIndex + 1;
      this.setScrollBarToCurrentIndex();
    }

    private void upArrowPressed()
    {
      this.upArrow.scale = this.upArrow.baseScale;
      this.currentItemIndex = this.currentItemIndex - 1;
      this.setScrollBarToCurrentIndex();
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
      base.receiveLeftClick(x, y, true);
      if (Game1.activeClickableMenu == null)
        return;
      Vector2 clickableComponent = this.inventory.snapToClickableComponent(x, y);
      if (this.downArrow.containsPoint(x, y) && this.currentItemIndex < Math.Max(0, this.forSale.Count - 4))
      {
        this.downArrowPressed();
        Game1.playSound("shwip");
      }
      else if (this.upArrow.containsPoint(x, y) && this.currentItemIndex > 0)
      {
        this.upArrowPressed();
        Game1.playSound("shwip");
      }
      else if (this.scrollBar.containsPoint(x, y))
        this.scrolling = true;
      else if (!this.downArrow.containsPoint(x, y) && x > this.xPositionOnScreen + this.width && (x < this.xPositionOnScreen + this.width + Game1.tileSize * 2 && y > this.yPositionOnScreen) && y < this.yPositionOnScreen + this.height)
      {
        this.scrolling = true;
        this.leftClickHeld(x, y);
        this.releaseLeftClick(x, y);
      }
      this.currentItemIndex = Math.Max(0, Math.Min(this.forSale.Count - 4, this.currentItemIndex));
      if (this.heldItem == null)
      {
        Item obj = this.inventory.leftClick(x, y, (Item) null, false);
        if (obj != null)
        {
          ShopMenu.chargePlayer(Game1.player, this.currency, -((obj is StardewValley.Object ? (int) ((double) (obj as StardewValley.Object).sellToStorePrice() * (double) this.sellPercentage) : (int) ((double) (obj.salePrice() / 2) * (double) this.sellPercentage)) * obj.Stack));
          int num1 = obj.Stack / 8 + 2;
          for (int index = 0; index < num1; ++index)
          {
            List<TemporaryAnimatedSprite> animations = this.animations;
            TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.debrisSpriteSheet, new Rectangle(Game1.random.Next(2) * 16, 64, 16, 16), 9999f, 1, 999, clickableComponent + new Vector2(32f, 32f), false, false);
            temporaryAnimatedSprite.alphaFade = 0.025f;
            Vector2 vector2_1 = new Vector2((float) Game1.random.Next(-3, 4), -4f);
            temporaryAnimatedSprite.motion = vector2_1;
            Vector2 vector2_2 = new Vector2(0.0f, 0.5f);
            temporaryAnimatedSprite.acceleration = vector2_2;
            int num2 = index * 25;
            temporaryAnimatedSprite.delayBeforeAnimationStart = num2;
            double num3 = (double) Game1.pixelZoom * 0.5;
            temporaryAnimatedSprite.scale = (float) num3;
            animations.Add(temporaryAnimatedSprite);
            this.animations.Add(new TemporaryAnimatedSprite(Game1.debrisSpriteSheet, new Rectangle(Game1.random.Next(2) * 16, 64, 16, 16), 9999f, 1, 999, clickableComponent + new Vector2(32f, 32f), false, false)
            {
              scale = (float) Game1.pixelZoom,
              alphaFade = 0.025f,
              delayBeforeAnimationStart = index * 50,
              motion = Utility.getVelocityTowardPoint(new Point((int) clickableComponent.X + 32, (int) clickableComponent.Y + 32), new Vector2((float) (this.xPositionOnScreen - Game1.pixelZoom * 9), (float) (this.yPositionOnScreen + this.height - this.inventory.height - Game1.pixelZoom * 4)), 8f),
              acceleration = Utility.getVelocityTowardPoint(new Point((int) clickableComponent.X + 32, (int) clickableComponent.Y + 32), new Vector2((float) (this.xPositionOnScreen - Game1.pixelZoom * 9), (float) (this.yPositionOnScreen + this.height - this.inventory.height - Game1.pixelZoom * 4)), 0.5f)
            });
          }
          if (obj is StardewValley.Object && (obj as StardewValley.Object).edibility != -300)
          {
            for (int index = 0; index < obj.Stack; ++index)
            {
              if (Game1.random.NextDouble() < 0.0399999991059303)
                (Game1.getLocationFromName("SeedShop") as SeedShop).itemsToStartSellingTomorrow.Add(obj.getOne());
            }
          }
          Game1.playSound("sell");
          Game1.playSound("purchase");
          if (this.inventory.getItemAt(x, y) == null)
            this.animations.Add(new TemporaryAnimatedSprite(5, clickableComponent + new Vector2(32f, 32f), Color.White, 8, false, 100f, 0, -1, -1f, -1, 0)
            {
              motion = new Vector2(0.0f, -0.5f)
            });
        }
      }
      else
        this.heldItem = this.inventory.leftClick(x, y, this.heldItem, true);
      for (int index1 = 0; index1 < this.forSaleButtons.Count; ++index1)
      {
        if (this.currentItemIndex + index1 < this.forSale.Count && this.forSaleButtons[index1].containsPoint(x, y))
        {
          int index2 = this.currentItemIndex + index1;
          if (this.forSale[index2] != null)
          {
            int numberToBuy = Math.Min(Game1.oldKBState.IsKeyDown(Keys.LeftShift) ? Math.Min(Math.Min(5, ShopMenu.getPlayerCurrencyAmount(Game1.player, this.currency) / Math.Max(1, this.itemPriceAndStock[this.forSale[index2]][0])), Math.Max(1, this.itemPriceAndStock[this.forSale[index2]][1])) : 1, this.forSale[index2].maximumStackSize());
            if (numberToBuy == -1)
              numberToBuy = 1;
            if (numberToBuy > 0 && this.tryToPurchaseItem(this.forSale[index2], this.heldItem, numberToBuy, x, y, index2))
            {
              this.itemPriceAndStock.Remove(this.forSale[index2]);
              this.forSale.RemoveAt(index2);
            }
            else if (numberToBuy <= 0)
            {
              Game1.dayTimeMoneyBox.moneyShakeTimer = 1000;
              Game1.playSound("cancel");
            }
            if (this.heldItem != null && Game1.options.SnappyMenus && (Game1.activeClickableMenu != null && Game1.activeClickableMenu is ShopMenu) && Game1.player.addItemToInventoryBool(this.heldItem, false))
            {
              this.heldItem = (Item) null;
              DelayedAction.playSoundAfterDelay("coin", 100);
            }
          }
          this.currentItemIndex = Math.Max(0, Math.Min(this.forSale.Count - 4, this.currentItemIndex));
          return;
        }
      }
      if (!this.readyToClose() || x >= this.xPositionOnScreen - Game1.tileSize && y >= this.yPositionOnScreen - Game1.tileSize && (x <= this.xPositionOnScreen + this.width + Game1.tileSize * 2 && y <= this.yPositionOnScreen + this.height + Game1.tileSize))
        return;
      this.exitThisMenu(true);
    }

    public override bool readyToClose()
    {
      if (this.heldItem == null)
        return this.animations.Count == 0;
      return false;
    }

    public override void emergencyShutDown()
    {
      base.emergencyShutDown();
      if (this.heldItem == null)
        return;
      Game1.player.addItemToInventoryBool(this.heldItem, false);
      Game1.playSound("coin");
    }

    public static void chargePlayer(Farmer who, int currencyType, int amount)
    {
      switch (currencyType)
      {
        case 0:
          who.Money -= amount;
          break;
        case 1:
          who.festivalScore -= amount;
          break;
        case 2:
          who.clubCoins -= amount;
          break;
      }
    }

    private bool tryToPurchaseItem(Item item, Item heldItem, int numberToBuy, int x, int y, int indexInForSaleList)
    {
      if (heldItem == null)
      {
        int amount = this.itemPriceAndStock[item][0] * numberToBuy;
        int num = -1;
        if (this.itemPriceAndStock[item].Length > 2)
          num = this.itemPriceAndStock[item][2];
        if (ShopMenu.getPlayerCurrencyAmount(Game1.player, this.currency) >= amount && (num == -1 || Game1.player.hasItemInInventory(num, 5, 0)))
        {
          this.heldItem = item.getOne();
          this.heldItem.Stack = numberToBuy;
          if (!Game1.player.couldInventoryAcceptThisItem(this.heldItem))
          {
            Game1.playSound("smallSelect");
            this.heldItem = (Item) null;
            return false;
          }
          if (this.itemPriceAndStock[item][1] != int.MaxValue)
          {
            this.itemPriceAndStock[item][1] -= numberToBuy;
            this.forSale[indexInForSaleList].Stack -= numberToBuy;
          }
          ShopMenu.chargePlayer(Game1.player, this.currency, amount);
          if (num != -1)
            Game1.player.removeItemsFromInventory(num, 5);
          if (item.actionWhenPurchased())
          {
            if (this.heldItem is StardewValley.Object && (this.heldItem as StardewValley.Object).isRecipe)
            {
              string key = this.heldItem.Name.Substring(0, this.heldItem.Name.IndexOf("Recipe") - 1);
              try
              {
                if ((this.heldItem as StardewValley.Object).category == -7)
                  Game1.player.cookingRecipes.Add(key, 0);
                else
                  Game1.player.craftingRecipes.Add(key, 0);
                Game1.playSound("newRecipe");
              }
              catch (Exception ex)
              {
              }
              heldItem = (Item) null;
              this.heldItem = (Item) null;
            }
          }
          else if (Game1.mouseClickPolling > 300)
            Game1.playSound("purchaseRepeat");
          else
            Game1.playSound("purchaseClick");
        }
        else
        {
          Game1.dayTimeMoneyBox.moneyShakeTimer = 1000;
          Game1.playSound("cancel");
        }
      }
      else if (heldItem.Name.Equals(item.Name))
      {
        numberToBuy = Math.Min(numberToBuy, heldItem.maximumStackSize() - heldItem.Stack);
        if (numberToBuy > 0)
        {
          int amount = this.itemPriceAndStock[item][0] * numberToBuy;
          int index = -1;
          if (this.itemPriceAndStock[item].Length > 2)
            index = this.itemPriceAndStock[item][2];
          if (ShopMenu.getPlayerCurrencyAmount(Game1.player, this.currency) >= amount)
          {
            this.heldItem.Stack += numberToBuy;
            if (this.itemPriceAndStock[item][1] != int.MaxValue)
              this.itemPriceAndStock[item][1] -= numberToBuy;
            ShopMenu.chargePlayer(Game1.player, this.currency, amount);
            if (Game1.mouseClickPolling > 300)
              Game1.playSound("purchaseRepeat");
            else
              Game1.playSound("purchaseClick");
            if (index != -1)
              Game1.player.removeItemsFromInventory(index, 5);
            if (item.actionWhenPurchased())
              this.heldItem = (Item) null;
          }
          else
          {
            Game1.dayTimeMoneyBox.moneyShakeTimer = 1000;
            Game1.playSound("cancel");
          }
        }
      }
      if (this.itemPriceAndStock[item][1] > 0)
        return false;
      this.hoveredItem = (Item) null;
      return true;
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
      Vector2 clickableComponent = this.inventory.snapToClickableComponent(x, y);
      if (this.heldItem == null)
      {
        Item obj1 = this.inventory.rightClick(x, y, (Item) null, false);
        if (obj1 != null)
        {
          ShopMenu.chargePlayer(Game1.player, this.currency, -((obj1 is StardewValley.Object ? (int) ((double) (obj1 as StardewValley.Object).sellToStorePrice() * (double) this.sellPercentage) : (int) ((double) (obj1.salePrice() / 2) * (double) this.sellPercentage)) * obj1.Stack));
          Item obj2 = (Item) null;
          if (Game1.mouseClickPolling > 300)
            Game1.playSound("purchaseRepeat");
          else
            Game1.playSound("purchaseClick");
          List<TemporaryAnimatedSprite> animations = this.animations;
          TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.debrisSpriteSheet, new Rectangle(Game1.random.Next(2) * Game1.tileSize, 256, Game1.tileSize, Game1.tileSize), 9999f, 1, 999, clickableComponent + new Vector2(32f, 32f), false, false);
          temporaryAnimatedSprite.alphaFade = 0.025f;
          Vector2 velocityTowardPoint1 = Utility.getVelocityTowardPoint(new Point((int) clickableComponent.X + 32, (int) clickableComponent.Y + 32), Game1.dayTimeMoneyBox.position + new Vector2(96f, 196f), 12f);
          temporaryAnimatedSprite.motion = velocityTowardPoint1;
          Vector2 velocityTowardPoint2 = Utility.getVelocityTowardPoint(new Point((int) clickableComponent.X + 32, (int) clickableComponent.Y + 32), Game1.dayTimeMoneyBox.position + new Vector2(96f, 196f), 0.5f);
          temporaryAnimatedSprite.acceleration = velocityTowardPoint2;
          animations.Add(temporaryAnimatedSprite);
          if (obj2 is StardewValley.Object && (obj2 as StardewValley.Object).edibility != -300 && Game1.random.NextDouble() < 0.0399999991059303)
            (Game1.getLocationFromName("SeedShop") as SeedShop).itemsToStartSellingTomorrow.Add(obj2.getOne());
          if (this.inventory.getItemAt(x, y) == null)
          {
            Game1.playSound("sell");
            this.animations.Add(new TemporaryAnimatedSprite(5, clickableComponent + new Vector2(32f, 32f), Color.White, 8, false, 100f, 0, -1, -1f, -1, 0)
            {
              motion = new Vector2(0.0f, -0.5f)
            });
          }
        }
      }
      else
        this.heldItem = this.inventory.rightClick(x, y, this.heldItem, true);
      for (int index1 = 0; index1 < this.forSaleButtons.Count; ++index1)
      {
        if (this.currentItemIndex + index1 < this.forSale.Count && this.forSaleButtons[index1].containsPoint(x, y))
        {
          int index2 = this.currentItemIndex + index1;
          if (this.forSale[index2] == null)
            break;
          int numberToBuy = Game1.oldKBState.IsKeyDown(Keys.LeftShift) ? Math.Min(Math.Min(5, ShopMenu.getPlayerCurrencyAmount(Game1.player, this.currency) / this.itemPriceAndStock[this.forSale[index2]][0]), this.itemPriceAndStock[this.forSale[index2]][1]) : 1;
          if (numberToBuy > 0 && this.tryToPurchaseItem(this.forSale[index2], this.heldItem, numberToBuy, x, y, index2))
          {
            this.itemPriceAndStock.Remove(this.forSale[index2]);
            this.forSale.RemoveAt(index2);
          }
          if (this.heldItem == null || !Game1.options.SnappyMenus || (Game1.activeClickableMenu == null || !(Game1.activeClickableMenu is ShopMenu)) || !Game1.player.addItemToInventoryBool(this.heldItem, false))
            break;
          this.heldItem = (Item) null;
          DelayedAction.playSoundAfterDelay("coin", 100);
          break;
        }
      }
    }

    public override void performHoverAction(int x, int y)
    {
      base.performHoverAction(x, y);
      this.descriptionText = "";
      this.hoverText = "";
      this.hoveredItem = (Item) null;
      this.hoverPrice = -1;
      this.boldTitleText = "";
      this.upArrow.tryHover(x, y, 0.1f);
      this.downArrow.tryHover(x, y, 0.1f);
      this.scrollBar.tryHover(x, y, 0.1f);
      if (this.scrolling)
        return;
      for (int index = 0; index < this.forSaleButtons.Count; ++index)
      {
        if (this.currentItemIndex + index < this.forSale.Count && this.forSaleButtons[index].containsPoint(x, y))
        {
          Item key = this.forSale[this.currentItemIndex + index];
          this.hoverText = key.getDescription();
          this.boldTitleText = key.DisplayName;
          this.hoverPrice = this.itemPriceAndStock == null || !this.itemPriceAndStock.ContainsKey(key) ? key.salePrice() : this.itemPriceAndStock[key][0];
          this.hoveredItem = key;
          this.forSaleButtons[index].scale = Math.Min(this.forSaleButtons[index].scale + 0.03f, 1.1f);
        }
        else
          this.forSaleButtons[index].scale = Math.Max(1f, this.forSaleButtons[index].scale - 0.03f);
      }
      if (this.heldItem != null)
        return;
      foreach (ClickableComponent c in this.inventory.inventory)
      {
        if (c.containsPoint(x, y))
        {
          Item clickableComponent = this.inventory.getItemFromClickableComponent(c);
          if (clickableComponent != null && this.highlightItemToSell(clickableComponent))
          {
            this.hoverText = clickableComponent.DisplayName + " x" + (object) clickableComponent.Stack;
            this.hoverPrice = (clickableComponent is StardewValley.Object ? (int) ((double) (clickableComponent as StardewValley.Object).sellToStorePrice() * (double) this.sellPercentage) : (int) ((double) (clickableComponent.salePrice() / 2) * (double) this.sellPercentage)) * clickableComponent.Stack;
          }
        }
      }
    }

    public override void update(GameTime time)
    {
      base.update(time);
      if (this.poof == null || !this.poof.update(time))
        return;
      this.poof = (TemporaryAnimatedSprite) null;
    }

    public void drawCurrency(SpriteBatch b)
    {
      if (this.currency == 0)
        Game1.dayTimeMoneyBox.drawMoneyBox(b, this.xPositionOnScreen - Game1.pixelZoom * 9, this.yPositionOnScreen + this.height - this.inventory.height - Game1.pixelZoom * 3);
    }

    public override void receiveGamePadButton(Buttons b)
    {
      base.receiveGamePadButton(b);
      if (b != Buttons.RightTrigger && b != Buttons.LeftTrigger)
        return;
      if (this.currentlySnappedComponent != null && this.currentlySnappedComponent.myID >= 3546)
      {
        int num = -1;
        for (int index = 0; index < 12; ++index)
        {
          this.inventory.inventory[index].upNeighborID = 3546 + this.forSaleButtons.Count - 1;
          if (num == -1 && this.heldItem != null && (this.inventory.actualInventory != null && this.inventory.actualInventory.Count > index) && this.inventory.actualInventory[index] == null)
            num = index;
        }
        this.currentlySnappedComponent = this.getComponentWithID(num != -1 ? num : 0);
        this.snapCursorToCurrentSnappedComponent();
      }
      else
        this.snapToDefaultClickableComponent();
      Game1.playSound("shiny4");
    }

    private int getHoveredItemExtraItemIndex()
    {
      if (this.itemPriceAndStock != null && this.hoveredItem != null && (this.itemPriceAndStock.ContainsKey(this.hoveredItem) && this.itemPriceAndStock[this.hoveredItem].Length > 2))
        return this.itemPriceAndStock[this.hoveredItem][2];
      return -1;
    }

    private int getHoveredItemExtraItemAmount()
    {
      return 5;
    }

    public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
    {
      this.xPositionOnScreen = Game1.viewport.Width / 2 - (800 + IClickableMenu.borderWidth * 2) / 2;
      this.yPositionOnScreen = Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2;
      this.width = 1000 + IClickableMenu.borderWidth * 2;
      this.height = 600 + IClickableMenu.borderWidth * 2;
      this.initializeUpperRightCloseButton();
      if (Game1.viewport.Width < 1500)
        this.xPositionOnScreen = Game1.tileSize / 2;
      Game1.player.forceCanMove();
      this.inventory = new InventoryMenu(this.xPositionOnScreen + this.width, this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth + Game1.tileSize * 5 + Game1.pixelZoom * 10, false, (List<Item>) null, new InventoryMenu.highlightThisItem(this.highlightItemToSell), -1, 3, 0, 0, true)
      {
        showGrayedOutSlots = true
      };
      this.inventory.movePosition(-this.inventory.width - Game1.tileSize / 2, 0);
      int positionOnScreen1 = this.xPositionOnScreen;
      int borderWidth1 = IClickableMenu.borderWidth;
      int toClearSideBorder = IClickableMenu.spaceToClearSideBorder;
      int positionOnScreen2 = this.yPositionOnScreen;
      int borderWidth2 = IClickableMenu.borderWidth;
      int toClearTopBorder = IClickableMenu.spaceToClearTopBorder;
      this.upArrow = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize / 4, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 459, 11, 12), (float) Game1.pixelZoom, false);
      this.downArrow = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize / 4, this.yPositionOnScreen + this.height - Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 472, 11, 12), (float) Game1.pixelZoom, false);
      this.scrollBar = new ClickableTextureComponent(new Rectangle(this.upArrow.bounds.X + Game1.pixelZoom * 3, this.upArrow.bounds.Y + this.upArrow.bounds.Height + Game1.pixelZoom, 6 * Game1.pixelZoom, 10 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(435, 463, 6, 10), (float) Game1.pixelZoom, false);
      this.scrollBarRunner = new Rectangle(this.scrollBar.bounds.X, this.upArrow.bounds.Y + this.upArrow.bounds.Height + Game1.pixelZoom, this.scrollBar.bounds.Width, this.height - Game1.tileSize - this.upArrow.bounds.Height - Game1.pixelZoom * 7);
      this.forSaleButtons.Clear();
      for (int index = 0; index < 4; ++index)
        this.forSaleButtons.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize / 4 + index * ((this.height - Game1.tileSize * 4) / 4), this.width - Game1.tileSize / 2, (this.height - Game1.tileSize * 4) / 4 + Game1.pixelZoom), string.Concat((object) index)));
    }

    public override void draw(SpriteBatch b)
    {
      if (!Game1.options.showMenuBackground)
        b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.75f);
      IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(384, 373, 18, 18), this.xPositionOnScreen + this.width - this.inventory.width - Game1.tileSize / 2 - Game1.pixelZoom * 6, this.yPositionOnScreen + this.height - Game1.tileSize * 4 + Game1.pixelZoom * 10, this.inventory.width + Game1.pixelZoom * 14, this.height - Game1.tileSize * 7 + Game1.pixelZoom * 5, Color.White, (float) Game1.pixelZoom, true);
      IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(384, 373, 18, 18), this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height - Game1.tileSize * 4 + Game1.tileSize / 2 + Game1.pixelZoom, Color.White, (float) Game1.pixelZoom, true);
      this.drawCurrency(b);
      for (int index = 0; index < this.forSaleButtons.Count; ++index)
      {
        if (this.currentItemIndex + index < this.forSale.Count)
        {
          IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(384, 396, 15, 15), this.forSaleButtons[index].bounds.X, this.forSaleButtons[index].bounds.Y, this.forSaleButtons[index].bounds.Width, this.forSaleButtons[index].bounds.Height, !this.forSaleButtons[index].containsPoint(Game1.getOldMouseX(), Game1.getOldMouseY()) || this.scrolling ? Color.White : Color.Wheat, (float) Game1.pixelZoom, false);
          b.Draw(Game1.mouseCursors, new Vector2((float) (this.forSaleButtons[index].bounds.X + Game1.tileSize / 2 - Game1.pixelZoom * 3), (float) (this.forSaleButtons[index].bounds.Y + Game1.pixelZoom * 6 - Game1.pixelZoom)), new Rectangle?(new Rectangle(296, 363, 18, 18)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1f);
          this.forSale[this.currentItemIndex + index].drawInMenu(b, new Vector2((float) (this.forSaleButtons[index].bounds.X + Game1.tileSize / 2 - Game1.pixelZoom * 2), (float) (this.forSaleButtons[index].bounds.Y + Game1.pixelZoom * 6)), 1f);
          SpriteText.drawString(b, this.forSale[this.currentItemIndex + index].DisplayName, this.forSaleButtons[index].bounds.X + Game1.tileSize * 3 / 2 + Game1.pixelZoom * 2, this.forSaleButtons[index].bounds.Y + Game1.pixelZoom * 7, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
          SpriteText.drawString(b, this.itemPriceAndStock[this.forSale[this.currentItemIndex + index]][0].ToString() + " ", this.forSaleButtons[index].bounds.Right - SpriteText.getWidthOfString(this.itemPriceAndStock[this.forSale[this.currentItemIndex + index]][0].ToString() + " ") - Game1.pixelZoom * 15, this.forSaleButtons[index].bounds.Y + Game1.pixelZoom * 7, 999999, -1, 999999, ShopMenu.getPlayerCurrencyAmount(Game1.player, this.currency) >= this.itemPriceAndStock[this.forSale[this.currentItemIndex + index]][0] ? 1f : 0.5f, 0.88f, false, -1, "", -1);
          Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float) (this.forSaleButtons[index].bounds.Right - Game1.pixelZoom * 13), (float) (this.forSaleButtons[index].bounds.Y + Game1.pixelZoom * 10 - Game1.pixelZoom)), new Rectangle(193 + this.currency * 9, 373, 9, 10), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
        }
      }
      if (this.forSale.Count == 0)
        SpriteText.drawString(b, Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11583"), this.xPositionOnScreen + this.width / 2 - SpriteText.getWidthOfString(Game1.content.LoadString("Strings\\StringsFromCSFiles:ShopMenu.cs.11583")) / 2, this.yPositionOnScreen + this.height / 2 - Game1.tileSize * 2, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
      this.inventory.draw(b);
      for (int index = this.animations.Count - 1; index >= 0; --index)
      {
        if (this.animations[index].update(Game1.currentGameTime))
          this.animations.RemoveAt(index);
        else
          this.animations[index].draw(b, true, 0, 0);
      }
      if (this.poof != null)
        this.poof.draw(b, false, 0, 0);
      this.upArrow.draw(b);
      this.downArrow.draw(b);
      if (this.forSale.Count > 4)
      {
        IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(403, 383, 6, 6), this.scrollBarRunner.X, this.scrollBarRunner.Y, this.scrollBarRunner.Width, this.scrollBarRunner.Height, Color.White, (float) Game1.pixelZoom, true);
        this.scrollBar.draw(b);
      }
      if (!this.hoverText.Equals(""))
        IClickableMenu.drawToolTip(b, this.hoverText, this.boldTitleText, this.hoveredItem, this.heldItem != null, -1, this.currency, this.getHoveredItemExtraItemIndex(), this.getHoveredItemExtraItemAmount(), (CraftingRecipe) null, this.hoverPrice);
      if (this.heldItem != null)
        this.heldItem.drawInMenu(b, new Vector2((float) (Game1.getOldMouseX() + 8), (float) (Game1.getOldMouseY() + 8)), 1f);
      base.draw(b);
      if (Game1.viewport.Width > 800 && Game1.options.showMerchantPortraits)
      {
        if (this.portraitPerson != null)
        {
          Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float) (this.xPositionOnScreen - 80 * Game1.pixelZoom), (float) this.yPositionOnScreen), new Rectangle(603, 414, 74, 74), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, false, 0.91f, -1, -1, 0.35f);
          if (this.portraitPerson.Portrait != null)
            b.Draw(this.portraitPerson.Portrait, new Vector2((float) (this.xPositionOnScreen - 80 * Game1.pixelZoom + Game1.pixelZoom * 5), (float) (this.yPositionOnScreen + Game1.pixelZoom * 5)), new Rectangle?(new Rectangle(0, 0, 64, 64)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.92f);
        }
        if (this.potraitPersonDialogue != null)
          IClickableMenu.drawHoverText(b, this.potraitPersonDialogue, Game1.dialogueFont, 0, 0, -1, (string) null, -1, (string[]) null, (Item) null, 0, -1, -1, this.xPositionOnScreen - (int) Game1.dialogueFont.MeasureString(this.potraitPersonDialogue).X - Game1.tileSize, this.yPositionOnScreen + (this.portraitPerson != null ? 78 * Game1.pixelZoom : 0), 1f, (CraftingRecipe) null);
      }
      this.drawMouse(b);
    }
  }
}
