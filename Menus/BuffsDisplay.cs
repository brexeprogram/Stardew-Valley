// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.BuffsDisplay
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StardewValley.Menus
{
  public class BuffsDisplay : IClickableMenu
  {
    public static int sideSpace = Game1.tileSize / 2;
    public new static int width = Game1.tileSize * 4 + Game1.tileSize / 2;
    private Dictionary<ClickableTextureComponent, Buff> buffs = new Dictionary<ClickableTextureComponent, Buff>();
    public List<Buff> otherBuffs = new List<Buff>();
    public string hoverText = "";
    public const int fullnessLength = 180000;
    public const int quenchedLength = 60000;
    public Buff food;
    public Buff drink;
    public int fullnessLeft;
    public int quenchedLeft;

    public BuffsDisplay()
      : base(Game1.viewport.Width - 320 - BuffsDisplay.sideSpace - BuffsDisplay.width, Game1.tileSize / 8, BuffsDisplay.width, Game1.tileSize, false)
    {
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
    }

    private void updatePosition()
    {
      int width = 320 + BuffsDisplay.sideSpace + BuffsDisplay.width;
      this.xPositionOnScreen = Game1.viewport.Width - width;
      Vector2 position = new Vector2((float) this.xPositionOnScreen, 0.0f);
      Utility.makeSafe(ref position, width, 0);
      this.xPositionOnScreen = (int) position.X;
      this.syncIcons();
    }

    public override void performHoverAction(int x, int y)
    {
      this.updatePosition();
      this.hoverText = "";
      foreach (KeyValuePair<ClickableTextureComponent, Buff> buff in this.buffs)
      {
        if (buff.Key.containsPoint(x, y))
        {
          this.hoverText = buff.Key.hoverText + Environment.NewLine + buff.Value.getTimeLeft();
          buff.Key.scale = Math.Min(buff.Key.baseScale + 0.1f, buff.Key.scale + 0.02f);
          break;
        }
      }
    }

    public void arrangeTheseComponentsInThisRectangle(int rectangleX, int rectangleY, int rectangleWidthInComponentWidthUnits, int componentWidth, int componentHeight, int buffer, bool rightToLeft)
    {
      int num1 = 0;
      int num2 = 0;
      foreach (KeyValuePair<ClickableTextureComponent, Buff> buff in this.buffs)
      {
        ClickableTextureComponent key = buff.Key;
        if (rightToLeft)
          key.bounds = new Rectangle(rectangleX + rectangleWidthInComponentWidthUnits * componentWidth - (num1 + 1) * (componentWidth + buffer), rectangleY + num2 * (componentHeight + buffer), componentWidth, componentHeight);
        else
          key.bounds = new Rectangle(rectangleX + num1 * (componentWidth + buffer), rectangleY + num2 * (componentHeight + buffer), componentWidth, componentHeight);
        ++num1;
        if (num1 > rectangleWidthInComponentWidthUnits)
        {
          ++num2;
          num1 %= rectangleWidthInComponentWidthUnits;
        }
      }
    }

    public void syncIcons()
    {
      this.buffs.Clear();
      if (this.food != null)
      {
        foreach (ClickableTextureComponent clickableComponent in this.food.getClickableComponents())
          this.buffs.Add(clickableComponent, this.food);
      }
      if (this.drink != null)
      {
        foreach (ClickableTextureComponent clickableComponent in this.drink.getClickableComponents())
          this.buffs.Add(clickableComponent, this.drink);
      }
      foreach (Buff otherBuff in this.otherBuffs)
      {
        foreach (ClickableTextureComponent clickableComponent in otherBuff.getClickableComponents())
          this.buffs.Add(clickableComponent, otherBuff);
      }
      this.arrangeTheseComponentsInThisRectangle(this.xPositionOnScreen, Game1.tileSize / 8, BuffsDisplay.width / Game1.tileSize, Game1.tileSize, Game1.tileSize, Game1.tileSize / 8, true);
    }

    public bool hasBuff(int which)
    {
      foreach (Buff otherBuff in this.otherBuffs)
      {
        if (otherBuff.which == which)
          return true;
      }
      return false;
    }

    public bool tryToAddFoodBuff(Buff b, int duration)
    {
      if (b.total <= 0 || this.fullnessLeft > 0)
        return false;
      if (this.food != null)
        this.food.removeBuff();
      this.food = b;
      this.food.addBuff();
      this.syncIcons();
      return true;
    }

    public bool tryToAddDrinkBuff(Buff b)
    {
      if (b.source.Contains("Beer") || b.source.Contains("Wine") || (b.source.Contains("Mead") || b.source.Contains("Pale Ale")))
        this.addOtherBuff(new Buff(17));
      else if (b.source.Equals("Oil of Garlic"))
        this.addOtherBuff(new Buff(23));
      else if (b.source.Equals("Life Elixir"))
        Game1.player.health = Game1.player.maxHealth;
      else if (b.source.Equals("Muscle Remedy"))
        Game1.player.exhausted = false;
      if (b.total <= 0 || this.quenchedLeft > 0)
        return false;
      if (this.drink != null)
        this.drink.removeBuff();
      this.drink = b;
      this.drink.addBuff();
      this.syncIcons();
      return true;
    }

    public bool addOtherBuff(Buff buff)
    {
      if (buff.which != -1)
      {
        foreach (KeyValuePair<ClickableTextureComponent, Buff> buff1 in this.buffs)
        {
          if (buff.which == buff1.Value.which)
          {
            buff1.Value.millisecondsDuration = buff.millisecondsDuration;
            buff1.Key.scale = buff1.Key.baseScale + 0.2f;
            return false;
          }
        }
      }
      this.otherBuffs.Add(buff);
      buff.addBuff();
      this.syncIcons();
      return true;
    }

    public new void update(GameTime time)
    {
      this.updatePosition();
      if (this.food != null && this.food.update(time))
      {
        this.food.removeBuff();
        this.food = (Buff) null;
        this.syncIcons();
      }
      if (this.drink != null && this.drink.update(time))
      {
        this.drink.removeBuff();
        this.drink = (Buff) null;
        this.syncIcons();
      }
      for (int index = this.otherBuffs.Count - 1; index >= 0; --index)
      {
        if (this.otherBuffs[index].update(time))
        {
          this.otherBuffs[index].removeBuff();
          this.otherBuffs.RemoveAt(index);
          this.syncIcons();
        }
      }
      foreach (KeyValuePair<ClickableTextureComponent, Buff> buff in this.buffs)
      {
        ClickableTextureComponent key = buff.Key;
        key.scale = Math.Max(key.baseScale, key.scale - 0.01f);
      }
    }

    public void clearAllBuffs()
    {
      this.otherBuffs.Clear();
      if (this.food != null)
      {
        this.food.removeBuff();
        this.food = (Buff) null;
      }
      if (this.drink != null)
      {
        this.drink.removeBuff();
        this.drink = (Buff) null;
      }
      this.buffs.Clear();
    }

    public override void draw(SpriteBatch b)
    {
      this.updatePosition();
      foreach (KeyValuePair<ClickableTextureComponent, Buff> buff in this.buffs)
        buff.Key.draw(b);
      if (this.hoverText.Length == 0 || !this.isWithinBounds(Game1.getOldMouseX(), Game1.getOldMouseY()))
        return;
      IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, -1, (string) null, -1, (string[]) null, (Item) null, 0, -1, -1, -1, -1, 1f, (CraftingRecipe) null);
    }
  }
}
