// Decompiled with JetBrains decompiler
// Type: StardewValley.Tool
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace StardewValley
{
  [XmlInclude(typeof (MagnifyingGlass))]
  [XmlInclude(typeof (Shears))]
  [XmlInclude(typeof (MilkPail))]
  [XmlInclude(typeof (Axe))]
  [XmlInclude(typeof (Wand))]
  [XmlInclude(typeof (Hoe))]
  [XmlInclude(typeof (FishingRod))]
  [XmlInclude(typeof (MeleeWeapon))]
  [XmlInclude(typeof (Pan))]
  [XmlInclude(typeof (Pickaxe))]
  [XmlInclude(typeof (WateringCan))]
  [XmlInclude(typeof (Slingshot))]
  public abstract class Tool : Item
  {
    public static Color copperColor = new Color(198, 108, 43);
    public static Color steelColor = new Color(197, 226, 222);
    public static Color goldColor = new Color(248, (int) byte.MaxValue, 73);
    public static Color iridiumColor = new Color(144, 135, 181);
    public const int standardStaminaReduction = 2;
    public const int nonUpgradeable = -1;
    public const int stone = 0;
    public const int copper = 1;
    public const int steel = 2;
    public const int gold = 3;
    public const int iridium = 4;
    public const int parsnipSpriteIndex = 0;
    public const int hoeSpriteIndex = 21;
    public const int hammerSpriteIndex = 105;
    public const int axeSpriteIndex = 189;
    public const int wateringCanSpriteIndex = 273;
    public const int fishingRodSpriteIndex = 8;
    public const int batteredSwordSpriteIndex = 67;
    public const int axeMenuIndex = 215;
    public const int hoeMenuIndex = 47;
    public const int pickAxeMenuIndex = 131;
    public const int wateringCanMenuIndex = 296;
    public const int startOfNegativeWeaponIndex = -10000;
    public static Texture2D weaponsTexture;
    public string name;
    [XmlIgnore]
    private string _description;
    public int initialParentTileIndex;
    public int currentParentTileIndex;
    public int indexOfMenuItemView;
    public bool stackable;
    public bool instantUse;
    public int upgradeLevel;
    public int numAttachmentSlots;
    protected Farmer lastUser;
    public Object[] attachments;
    [XmlIgnore]
    protected string displayName;

    [XmlIgnore]
    public string description
    {
      get
      {
        if (this._description == null)
          this._description = this.loadDescription();
        return this._description;
      }
      set
      {
        this._description = value;
      }
    }

    [XmlIgnore]
    public override string DisplayName
    {
      get
      {
        this.displayName = this.loadDisplayName();
        switch (this.upgradeLevel)
        {
          case 1:
            return Game1.content.LoadString("Strings\\StringsFromCSFiles:Tool.cs.14299", (object) this.displayName);
          case 2:
            return Game1.content.LoadString("Strings\\StringsFromCSFiles:Tool.cs.14300", (object) this.displayName);
          case 3:
            return Game1.content.LoadString("Strings\\StringsFromCSFiles:Tool.cs.14301", (object) this.displayName);
          case 4:
            return Game1.content.LoadString("Strings\\StringsFromCSFiles:Tool.cs.14302", (object) this.displayName);
          default:
            return this.displayName;
        }
      }
      set
      {
        this.displayName = value;
      }
    }

    public override string Name
    {
      get
      {
        switch (this.upgradeLevel)
        {
          case 1:
            return "Copper " + this.name;
          case 2:
            return "Steel " + this.name;
          case 3:
            return "Gold " + this.name;
          case 4:
            return "Iridium " + this.name;
          default:
            return this.name;
        }
      }
    }

    public override int Stack
    {
      get
      {
        if (this.stackable)
          return ((Stackable) this).NumberInStack;
        return 1;
      }
      set
      {
        if (!this.stackable)
          return;
        this.Stack = Math.Min(Math.Max(0, value), this.maximumStackSize());
      }
    }

    public string Description
    {
      get
      {
        return this.description;
      }
    }

    [XmlIgnore]
    public int CurrentParentTileIndex
    {
      get
      {
        return this.currentParentTileIndex;
      }
      set
      {
        this.currentParentTileIndex = value;
      }
    }

    [XmlIgnore]
    public virtual int UpgradeLevel
    {
      get
      {
        return this.upgradeLevel;
      }
      set
      {
        this.upgradeLevel = value;
        this.setNewTileIndexForUpgradeLevel();
      }
    }

    public Tool()
    {
      this.category = -99;
    }

    public Tool(string name, int upgradeLevel, int initialParentTileIndex, int indexOfMenuItemView, bool stackable, int numAttachmentSlots = 0)
    {
      this.name = name;
      this.initialParentTileIndex = initialParentTileIndex;
      this.indexOfMenuItemView = indexOfMenuItemView;
      this.stackable = stackable;
      this.currentParentTileIndex = initialParentTileIndex;
      this.numAttachmentSlots = numAttachmentSlots;
      if (numAttachmentSlots > 0)
        this.attachments = new Object[numAttachmentSlots];
      this.category = -99;
    }

    protected abstract string loadDisplayName();

    protected abstract string loadDescription();

    public override string getCategoryName()
    {
      if (!(this is MeleeWeapon) || (this as MeleeWeapon).indexOfMenuItemView == 47)
        return Game1.content.LoadString("Strings\\StringsFromCSFiles:Tool.cs.14307");
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:Tool.cs.14303", (object) (this as MeleeWeapon).getItemLevel(), (object) ((this as MeleeWeapon).type == 1 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Tool.cs.14304") : ((this as MeleeWeapon).type == 2 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Tool.cs.14305") : Game1.content.LoadString("Strings\\StringsFromCSFiles:Tool.cs.14306"))));
    }

    public override Color getCategoryColor()
    {
      return Color.DarkSlateGray;
    }

    public virtual void draw(SpriteBatch b)
    {
      if (Game1.player.toolPower <= 0 || !Game1.player.canReleaseTool)
        return;
      foreach (Vector2 vector2 in this.tilesAffected(Game1.player.GetToolLocation(false) / (float) Game1.tileSize, Game1.player.toolPower, Game1.player))
        b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(new Vector2((float) ((int) vector2.X * Game1.tileSize), (float) ((int) vector2.Y * Game1.tileSize))), new Rectangle?(new Rectangle(194, 388, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.01f);
    }

    public virtual void tickUpdate(GameTime time, Farmer who)
    {
    }

    public void Update(int direction, int farmerMotionFrame)
    {
      this.Update(direction, farmerMotionFrame, Game1.player);
    }

    public bool isHeavyHitter()
    {
      if (!(this is MeleeWeapon) && !(this is Hoe) && !(this is Axe))
        return this is Pickaxe;
      return true;
    }

    public void Update(int direction, int farmerMotionFrame, Farmer who)
    {
      int num = 0;
      if (this is WateringCan)
      {
        switch (direction)
        {
          case 0:
            num = 4;
            break;
          case 1:
            num = 2;
            break;
          case 2:
            num = 0;
            break;
          case 3:
            num = 2;
            break;
        }
      }
      else if (this is FishingRod)
      {
        switch (direction)
        {
          case 0:
            num = 3;
            break;
          case 1:
            num = 0;
            break;
          case 3:
            num = 0;
            break;
        }
      }
      else
      {
        switch (direction)
        {
          case 0:
            num = 3;
            break;
          case 1:
            num = 2;
            break;
          case 3:
            num = 2;
            break;
        }
      }
      if (!this.Name.Equals("Watering Can"))
      {
        if (farmerMotionFrame < 1)
          this.currentParentTileIndex = this.initialParentTileIndex;
        else if (who.FacingDirection == 0 || who.FacingDirection == 2 && farmerMotionFrame >= 2)
          this.currentParentTileIndex = this.initialParentTileIndex + 1;
      }
      else
        this.currentParentTileIndex = farmerMotionFrame < 5 || direction == 0 ? this.initialParentTileIndex : this.initialParentTileIndex + 1;
      this.currentParentTileIndex = this.currentParentTileIndex + num;
    }

    public override int attachmentSlots()
    {
      return this.numAttachmentSlots;
    }

    public Farmer getLastFarmerToUse()
    {
      return this.lastUser;
    }

    public virtual void leftClick(Farmer who)
    {
    }

    public virtual void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
    {
      this.lastUser = who;
      short seed = (short) Game1.random.Next((int) short.MinValue, 32768);
      if (Game1.IsClient && who.Equals((object) Game1.player))
      {
        Game1.recentMultiplayerRandom = new Random((int) seed);
        ToolDescription indexFromTool = ToolFactory.getIndexFromTool(this);
        Game1.client.sendMessage((byte) 7, new object[7]
        {
          (object) indexFromTool.index,
          (object) indexFromTool.upgradeLevel,
          (object) (short) x,
          (object) (short) y,
          (object) location.name,
          (object) (byte) who.FacingDirection,
          (object) seed
        });
      }
      else if (Game1.IsServer && who.Equals((object) Game1.player))
      {
        Game1.recentMultiplayerRandom = new Random((int) seed);
        MultiplayerUtility.broadcastToolAction(this, x, y, location.name, (byte) who.FacingDirection, seed, who.uniqueMultiplayerID);
      }
      if (this.isHeavyHitter() && !(this is MeleeWeapon))
      {
        Rumble.rumble(0.1f + (float) (Game1.random.NextDouble() / 4.0), (float) (100 + Game1.random.Next(50)));
        location.damageMonster(new Rectangle(x - Game1.tileSize / 2, y - Game1.tileSize / 2, Game1.tileSize, Game1.tileSize), this.upgradeLevel + 1, (this.upgradeLevel + 1) * 3, false, who);
      }
      if (!(this is MeleeWeapon) || who.UsingTool && Game1.mouseClickPolling < 50 && ((this as MeleeWeapon).type != 1 && (this as MeleeWeapon).initialParentTileIndex != 47) && (MeleeWeapon.timedHitTimer <= 0 && who.FarmerSprite.indexInCurrentAnimation == 5 && (double) who.FarmerSprite.timer < (double) who.FarmerSprite.interval / 4.0))
        return;
      if ((this as MeleeWeapon).type == 2 && (this as MeleeWeapon).isOnSpecial)
      {
        (this as MeleeWeapon).doClubFunction(who);
      }
      else
      {
        if (who.FarmerSprite.indexInCurrentAnimation <= 0)
          return;
        MeleeWeapon.timedHitTimer = 500;
      }
    }

    public virtual bool beginUsing(GameLocation location, int x, int y, Farmer who)
    {
      return false;
    }

    public virtual bool onRelease(GameLocation location, int x, int y, Farmer who)
    {
      return false;
    }

    public override bool canBeDropped()
    {
      return false;
    }

    public virtual bool canThisBeAttached(Object o)
    {
      if (this.attachments != null)
      {
        for (int index = 0; index < this.attachments.Length; ++index)
        {
          if (this.attachments[index] == null)
            return true;
        }
      }
      return false;
    }

    public virtual Object attach(Object o)
    {
      for (int index = 0; index < this.attachments.Length; ++index)
      {
        if (this.attachments[index] == null)
        {
          this.attachments[index] = o;
          Game1.playSound("button1");
          return (Object) null;
        }
      }
      return o;
    }

    public void colorTool(int level)
    {
      int targetColorIndex = 0;
      int startPixel = 0;
      string str = ((IEnumerable<string>) this.name.Split(' ')).Last<string>();
      if (!(str == "Hoe"))
      {
        if (!(str == "Pickaxe"))
        {
          if (!(str == "Axe"))
          {
            if (str == "Can")
            {
              targetColorIndex = 168713;
              startPixel = 163840;
            }
          }
          else
          {
            targetColorIndex = 134681;
            startPixel = 131072;
          }
        }
        else
        {
          targetColorIndex = 100749;
          startPixel = 98304;
        }
      }
      else
      {
        targetColorIndex = 69129;
        startPixel = 65536;
      }
      int red = 0;
      int green = 0;
      int blue = 0;
      switch (level)
      {
        case 1:
          red = 198;
          green = 108;
          blue = 43;
          break;
        case 2:
          red = 197;
          green = 226;
          blue = 222;
          break;
        case 3:
          red = 248;
          green = (int) byte.MaxValue;
          blue = 73;
          break;
        case 4:
          red = 144;
          green = 135;
          blue = 181;
          break;
      }
      if (startPixel <= 0 || level <= 0)
        return;
      if (this.name.Contains("Can"))
        ColorChanger.swapColor(Game1.toolSpriteSheet, targetColorIndex + 36, red * 5 / 4, green * 5 / 4, blue * 5 / 4, startPixel, startPixel + 32768);
      ColorChanger.swapColor(Game1.toolSpriteSheet, targetColorIndex + 8, red, green, blue, startPixel, startPixel + 32768);
      ColorChanger.swapColor(Game1.toolSpriteSheet, targetColorIndex + 4, red * 3 / 4, green * 3 / 4, blue * 3 / 4, startPixel, startPixel + 32768);
      ColorChanger.swapColor(Game1.toolSpriteSheet, targetColorIndex, red * 3 / 8, green * 3 / 8, blue * 3 / 8, startPixel, startPixel + 32768);
    }

    public override bool actionWhenPurchased()
    {
      if (!(this is Axe) && !(this is Pickaxe) && (!(this is Hoe) && !(this is WateringCan)))
        return base.actionWhenPurchased();
      Tool toolFromName = Game1.player.getToolFromName(this.name);
      ++toolFromName.UpgradeLevel;
      Game1.player.toolBeingUpgraded = toolFromName;
      Game1.player.daysLeftForToolUpgrade = 2;
      Game1.playSound("parry");
      Game1.player.removeItemFromInventory((Item) toolFromName);
      Game1.exitActiveMenu();
      Game1.drawDialogue(Game1.getCharacterFromName("Clint", false), Game1.content.LoadString("Strings\\StringsFromCSFiles:Tool.cs.14317"));
      return true;
    }

    protected List<Vector2> tilesAffected(Vector2 tileLocation, int power, Farmer who)
    {
      ++power;
      List<Vector2> vector2List = new List<Vector2>();
      vector2List.Add(tileLocation);
      if (who.facingDirection == 0)
      {
        if (power >= 2)
        {
          vector2List.Add(tileLocation + new Vector2(0.0f, -1f));
          vector2List.Add(tileLocation + new Vector2(0.0f, -2f));
        }
        if (power >= 3)
        {
          vector2List.Add(tileLocation + new Vector2(0.0f, -3f));
          vector2List.Add(tileLocation + new Vector2(0.0f, -4f));
        }
        if (power >= 4)
        {
          vector2List.RemoveAt(vector2List.Count - 1);
          vector2List.RemoveAt(vector2List.Count - 1);
          vector2List.Add(tileLocation + new Vector2(1f, -2f));
          vector2List.Add(tileLocation + new Vector2(1f, -1f));
          vector2List.Add(tileLocation + new Vector2(1f, 0.0f));
          vector2List.Add(tileLocation + new Vector2(-1f, -2f));
          vector2List.Add(tileLocation + new Vector2(-1f, -1f));
          vector2List.Add(tileLocation + new Vector2(-1f, 0.0f));
        }
        if (power >= 5)
        {
          for (int index = vector2List.Count - 1; index >= 0; --index)
            vector2List.Add(vector2List[index] + new Vector2(0.0f, -3f));
        }
      }
      else if (who.facingDirection == 1)
      {
        if (power >= 2)
        {
          vector2List.Add(tileLocation + new Vector2(1f, 0.0f));
          vector2List.Add(tileLocation + new Vector2(2f, 0.0f));
        }
        if (power >= 3)
        {
          vector2List.Add(tileLocation + new Vector2(3f, 0.0f));
          vector2List.Add(tileLocation + new Vector2(4f, 0.0f));
        }
        if (power >= 4)
        {
          vector2List.RemoveAt(vector2List.Count - 1);
          vector2List.RemoveAt(vector2List.Count - 1);
          vector2List.Add(tileLocation + new Vector2(0.0f, -1f));
          vector2List.Add(tileLocation + new Vector2(1f, -1f));
          vector2List.Add(tileLocation + new Vector2(2f, -1f));
          vector2List.Add(tileLocation + new Vector2(0.0f, 1f));
          vector2List.Add(tileLocation + new Vector2(1f, 1f));
          vector2List.Add(tileLocation + new Vector2(2f, 1f));
        }
        if (power >= 5)
        {
          for (int index = vector2List.Count - 1; index >= 0; --index)
            vector2List.Add(vector2List[index] + new Vector2(3f, 0.0f));
        }
      }
      else if (who.facingDirection == 2)
      {
        if (power >= 2)
        {
          vector2List.Add(tileLocation + new Vector2(0.0f, 1f));
          vector2List.Add(tileLocation + new Vector2(0.0f, 2f));
        }
        if (power >= 3)
        {
          vector2List.Add(tileLocation + new Vector2(0.0f, 3f));
          vector2List.Add(tileLocation + new Vector2(0.0f, 4f));
        }
        if (power >= 4)
        {
          vector2List.RemoveAt(vector2List.Count - 1);
          vector2List.RemoveAt(vector2List.Count - 1);
          vector2List.Add(tileLocation + new Vector2(1f, 2f));
          vector2List.Add(tileLocation + new Vector2(1f, 1f));
          vector2List.Add(tileLocation + new Vector2(1f, 0.0f));
          vector2List.Add(tileLocation + new Vector2(-1f, 2f));
          vector2List.Add(tileLocation + new Vector2(-1f, 1f));
          vector2List.Add(tileLocation + new Vector2(-1f, 0.0f));
        }
        if (power >= 5)
        {
          for (int index = vector2List.Count - 1; index >= 0; --index)
            vector2List.Add(vector2List[index] + new Vector2(0.0f, 3f));
        }
      }
      else if (who.facingDirection == 3)
      {
        if (power >= 2)
        {
          vector2List.Add(tileLocation + new Vector2(-1f, 0.0f));
          vector2List.Add(tileLocation + new Vector2(-2f, 0.0f));
        }
        if (power >= 3)
        {
          vector2List.Add(tileLocation + new Vector2(-3f, 0.0f));
          vector2List.Add(tileLocation + new Vector2(-4f, 0.0f));
        }
        if (power >= 4)
        {
          vector2List.RemoveAt(vector2List.Count - 1);
          vector2List.RemoveAt(vector2List.Count - 1);
          vector2List.Add(tileLocation + new Vector2(0.0f, -1f));
          vector2List.Add(tileLocation + new Vector2(-1f, -1f));
          vector2List.Add(tileLocation + new Vector2(-2f, -1f));
          vector2List.Add(tileLocation + new Vector2(0.0f, 1f));
          vector2List.Add(tileLocation + new Vector2(-1f, 1f));
          vector2List.Add(tileLocation + new Vector2(-2f, 1f));
        }
        if (power >= 5)
        {
          for (int index = vector2List.Count - 1; index >= 0; --index)
            vector2List.Add(vector2List[index] + new Vector2(-3f, 0.0f));
        }
      }
      return vector2List;
    }

    public virtual bool doesShowTileLocationMarker()
    {
      return true;
    }

    public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber)
    {
      spriteBatch.Draw(Game1.toolSpriteSheet, location + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2)), new Rectangle?(Game1.getSquareSourceRectForNonStandardTileSheet(Game1.toolSpriteSheet, Game1.tileSize / 4, Game1.tileSize / 4, this.indexOfMenuItemView)), Color.White * transparency, 0.0f, new Vector2((float) (Game1.tileSize / 4 / 2), (float) (Game1.tileSize / 4 / 2)), (float) Game1.pixelZoom * scaleSize, SpriteEffects.None, layerDepth);
      if (!this.stackable)
        return;
      Game1.drawWithBorder(string.Concat((object) ((Stackable) this).NumberInStack), Color.Black, Color.White, location + new Vector2((float) Game1.tileSize - Game1.dialogueFont.MeasureString(string.Concat((object) ((Stackable) this).NumberInStack)).X, (float) Game1.tileSize - (float) ((double) Game1.dialogueFont.MeasureString(string.Concat((object) ((Stackable) this).NumberInStack)).Y * 3.0 / 4.0)), 0.0f, 0.5f, 1f);
    }

    public override bool isPlaceable()
    {
      return false;
    }

    public override int maximumStackSize()
    {
      return this.stackable ? 99 : -1;
    }

    public virtual void setNewTileIndexForUpgradeLevel()
    {
      if (this is MeleeWeapon || this is MagnifyingGlass || (this is MilkPail || this is Shears) || (this is Pan || this is Slingshot || this is Wand))
        return;
      int num1 = 21;
      if (this is FishingRod)
      {
        this.initialParentTileIndex = 8 + this.upgradeLevel;
        this.currentParentTileIndex = this.initialParentTileIndex;
        this.indexOfMenuItemView = this.initialParentTileIndex;
      }
      else
      {
        if (this is Axe)
          num1 = 189;
        else if (this is Hoe)
          num1 = 21;
        else if (this is Pickaxe)
          num1 = 105;
        else if (this is WateringCan)
          num1 = 273;
        int num2 = num1 + this.upgradeLevel * 7;
        if (this.upgradeLevel > 2)
          num2 += 21;
        this.initialParentTileIndex = num2;
        this.currentParentTileIndex = this.initialParentTileIndex;
        this.indexOfMenuItemView = this.initialParentTileIndex + (this is WateringCan ? 2 : 5) + 21;
      }
    }

    public override Item getOne()
    {
      if (this.stackable)
        return (Item) new Seeds(((Seeds) this).SeedType, 1);
      return (Item) this;
    }

    public override int getStack()
    {
      if (this.stackable)
        return ((Stackable) this).NumberInStack;
      return 1;
    }

    public override int addToStack(int amount)
    {
      if (!this.stackable)
        return amount;
      ((Stackable) this).NumberInStack += amount;
      if (((Stackable) this).NumberInStack <= 99)
        return 0;
      int num = ((Stackable) this).NumberInStack - 99;
      ((Stackable) this).NumberInStack = 99;
      return num;
    }

    public override string getDescription()
    {
      return Game1.parseText(this.description, Game1.smallFont, Game1.tileSize * 4 + Game1.tileSize / 4);
    }
  }
}
