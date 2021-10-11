// Decompiled with JetBrains decompiler
// Type: StardewValley.Item
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Objects;
using StardewValley.Tools;
using System;
using System.Xml.Serialization;

namespace StardewValley
{
  [XmlInclude(typeof (Object))]
  [XmlInclude(typeof (Tool))]
  public abstract class Item : IComparable
  {
    public int specialVariable;
    public int category;
    public bool specialItem;
    public bool hasBeenInInventory;

    public abstract void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber);

    public void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth)
    {
      this.drawInMenu(spriteBatch, location, scaleSize, transparency, layerDepth, true);
    }

    public void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize)
    {
      this.drawInMenu(spriteBatch, location, scaleSize, 1f, 0.9f, true);
    }

    public abstract int maximumStackSize();

    public abstract int getStack();

    public abstract int addToStack(int amount);

    public abstract string getDescription();

    public abstract bool isPlaceable();

    public virtual int salePrice()
    {
      return -1;
    }

    public virtual bool canBeTrashed()
    {
      if (this is Tool && (!(this is MeleeWeapon) || this.Name == null || this.Name.Equals("Scythe")))
        return this is FishingRod;
      return true;
    }

    public virtual bool canBePlacedInWater()
    {
      return false;
    }

    public virtual int parentSheetIndex
    {
      get
      {
        if (!(this is Object))
          return -1;
        return (this as Object).parentSheetIndex;
      }
    }

    public virtual bool actionWhenPurchased()
    {
      return false;
    }

    public virtual bool canBeDropped()
    {
      return true;
    }

    public virtual void actionWhenBeingHeld(Farmer who)
    {
    }

    public virtual void actionWhenStopBeingHeld(Farmer who)
    {
    }

    public int getRemainingStackSpace()
    {
      return this.maximumStackSize() - this.Stack;
    }

    public virtual string getHoverBoxText(Item hoveredItem)
    {
      return (string) null;
    }

    public virtual bool canBeGivenAsGift()
    {
      return false;
    }

    public virtual void drawAttachments(SpriteBatch b, int x, int y)
    {
    }

    public virtual bool canBePlacedHere(GameLocation l, Vector2 tile)
    {
      return false;
    }

    public virtual int attachmentSlots()
    {
      return 0;
    }

    public virtual string getCategoryName()
    {
      if (this is Boots)
        return Game1.content.LoadString("Strings\\StringsFromCSFiles:Item.cs.3829");
      return "";
    }

    public virtual Color getCategoryColor()
    {
      return Color.Black;
    }

    public bool canStackWith(Item other)
    {
      if (other == null || (!(other is Object) || !(this is Object)) && (!(other is ColoredObject) || !(this is ColoredObject)) || (this is ColoredObject && other is ColoredObject && !(this as ColoredObject).color.Equals((other as ColoredObject).color) || (this.maximumStackSize() <= 1 || other.maximumStackSize() <= 1 || ((this as Object).ParentSheetIndex != (other as Object).ParentSheetIndex || (this as Object).bigCraftable != (other as Object).bigCraftable))) || (this as Object).quality != (other as Object).quality)
        return false;
      return this.Name.Equals(other.Name);
    }

    public virtual string checkForSpecialItemHoldUpMeessage()
    {
      return (string) null;
    }

    public abstract string DisplayName { get; set; }

    public abstract string Name { get; }

    public abstract int Stack { get; set; }

    public abstract Item getOne();

    public int CompareTo(object obj)
    {
      if (!(obj is Item))
        return 0;
      if ((obj as Item).category != this.category)
        return (obj as Item).category - this.category;
      if (this is Object && obj is Object)
        return string.Compare((this as Object).type + this.Name, (obj as Object).type + (obj as Item).Name);
      return string.Compare(this.Name, (obj as Item).Name);
    }
  }
}
