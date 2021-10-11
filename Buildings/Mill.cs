// Decompiled with JetBrains decompiler
// Type: StardewValley.Buildings.Mill
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;
using StardewValley.Objects;
using System;

namespace StardewValley.Buildings
{
  public class Mill : Building
  {
    private Rectangle baseSourceRect = new Rectangle(0, 0, 64, 128);
    public Chest input;
    public Chest output;
    private bool hasLoadedToday;

    public Mill(BluePrint b, Vector2 tileLocation)
      : base(b, tileLocation)
    {
      this.input = new Chest(true);
      this.output = new Chest(true);
    }

    public Mill()
    {
    }

    public override Rectangle getSourceRectForMenu()
    {
      return new Rectangle(0, 0, 64, this.texture.Bounds.Height);
    }

    public override void load()
    {
      base.load();
    }

    public override bool doAction(Vector2 tileLocation, Farmer who)
    {
      if (this.daysOfConstructionLeft <= 0)
      {
        if ((double) tileLocation.X == (double) (this.tileX + 1) && (double) tileLocation.Y == (double) (this.tileY + 1))
        {
          if (who != null && who.ActiveObject != null)
          {
            bool flag = false;
            switch (who.ActiveObject.parentSheetIndex)
            {
              case 262:
              case 284:
                flag = true;
                break;
            }
            if (!flag)
            {
              Game1.showRedMessage(Game1.content.LoadString("Strings\\Buildings:CantMill"));
              return false;
            }
            Item thisInventoryList = Utility.addItemToThisInventoryList((Item) who.ActiveObject, this.input.items, 36);
            if (thisInventoryList != null)
            {
              who.ActiveObject = (StardewValley.Object) null;
              who.ActiveObject = (StardewValley.Object) thisInventoryList;
            }
            else
              who.ActiveObject = (StardewValley.Object) null;
            this.hasLoadedToday = true;
            Game1.playSound("Ship");
            if (who.ActiveObject != null)
              Game1.showRedMessage(Game1.content.LoadString("Strings\\Buildings:MillFull"));
          }
        }
        else if ((double) tileLocation.X == (double) (this.tileX + 3) && (double) tileLocation.Y == (double) (this.tileY + 1))
          Game1.activeClickableMenu = (IClickableMenu) new ItemGrabMenu(this.output.items, false, true, new InventoryMenu.highlightThisItem(InventoryMenu.highlightAllItems), new ItemGrabMenu.behaviorOnItemSelect(this.output.grabItemFromInventory), (string) null, new ItemGrabMenu.behaviorOnItemSelect(this.output.grabItemFromChest), false, true, true, true, true, 1, (Item) null, -1, (object) null);
      }
      return base.doAction(tileLocation, who);
    }

    public override void dayUpdate(int dayOfMonth)
    {
      this.hasLoadedToday = false;
      for (int index = this.input.items.Count - 1; index >= 0; --index)
      {
        if (this.input.items[index] != null)
        {
          Item i = (Item) null;
          switch (this.input.items[index].parentSheetIndex)
          {
            case 262:
              i = (Item) new StardewValley.Object(246, this.input.items[index].Stack, false, -1, 0);
              break;
            case 284:
              i = (Item) new StardewValley.Object(245, 3 * this.input.items[index].Stack, false, -1, 0);
              break;
            case 245:
            case 246:
              i = this.input.items[index];
              break;
          }
          if (i != null && Utility.canItemBeAddedToThisInventoryList(i, this.output.items, 36))
            this.input.items[index] = Utility.addItemToThisInventoryList(i, this.output.items, 36);
        }
      }
      base.dayUpdate(dayOfMonth);
    }

    public override void drawInMenu(SpriteBatch b, int x, int y)
    {
      this.drawShadow(b, x, y);
      b.Draw(this.texture, new Vector2((float) x, (float) y), new Rectangle?(this.getSourceRectForMenu()), this.color, 0.0f, new Vector2(0.0f, 0.0f), (float) Game1.pixelZoom, SpriteEffects.None, 0.89f);
      b.Draw(this.texture, new Vector2((float) (x + Game1.tileSize / 2), (float) (y + Game1.pixelZoom)), new Rectangle?(new Rectangle(64 + (int) Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 800 / 89 * 32 % 160, (int) Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 800 / 89 * 32 / 160 * 32, 32, 32)), this.color, 0.0f, new Vector2(0.0f, 0.0f), 4f, SpriteEffects.None, 0.9f);
    }

    public override void draw(SpriteBatch b)
    {
      if (this.daysOfConstructionLeft > 0)
      {
        this.drawInConstruction(b);
      }
      else
      {
        this.drawShadow(b, -1, -1);
        b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (this.tileX * Game1.tileSize), (float) (this.tileY * Game1.tileSize + this.tilesHigh * Game1.tileSize))), new Rectangle?(this.baseSourceRect), this.color * this.alpha, 0.0f, new Vector2(0.0f, (float) this.texture.Bounds.Height), 4f, SpriteEffects.None, (float) ((this.tileY + this.tilesHigh - 1) * Game1.tileSize) / 10000f);
        b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (this.tileX * Game1.tileSize + Game1.tileSize / 2), (float) (this.tileY * Game1.tileSize + this.tilesHigh * Game1.tileSize + Game1.pixelZoom))), new Rectangle?(new Rectangle(64 + (int) Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 800 / 89 * 32 % 160, (int) Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 800 / 89 * 32 / 160 * 32, 32, 32)), this.color * this.alpha, 0.0f, new Vector2(0.0f, (float) this.texture.Bounds.Height), 4f, SpriteEffects.None, (float) ((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000f);
        if (this.hasLoadedToday)
          b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (this.tileX * Game1.tileSize + 13 * Game1.pixelZoom), (float) (this.tileY * Game1.tileSize + this.tilesHigh * Game1.tileSize + Game1.pixelZoom * 69))), new Rectangle?(new Rectangle(64 + (int) Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 700 / 100 * 21, 72, 21, 8)), this.color * this.alpha, 0.0f, new Vector2(0.0f, (float) this.texture.Bounds.Height), 4f, SpriteEffects.None, (float) ((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000f);
        if (this.output.items.Count <= 0 || this.output.items[0] == null || this.output.items[0].parentSheetIndex != 245 && this.output.items[0].parentSheetIndex != 246)
          return;
        float num = (float) (4.0 * Math.Round(Math.Sin(DateTime.Now.TimeOfDay.TotalMilliseconds / 250.0), 2));
        b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (this.tileX * Game1.tileSize + Game1.tileSize * 3), (float) (this.tileY * Game1.tileSize - Game1.tileSize * 3 / 2) + num)), new Rectangle?(new Rectangle(141, 465, 20, 24)), Color.White * 0.75f, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, (float) ((double) ((this.tileY + 1) * Game1.tileSize) / 10000.0 + 9.99999997475243E-07 + (double) this.tileX / 10000.0));
        b.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (this.tileX * Game1.tileSize + Game1.tileSize * 3 + Game1.tileSize / 2 + Game1.pixelZoom), (float) (this.tileY * Game1.tileSize - Game1.tileSize + Game1.tileSize / 8) + num)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, this.output.items[0].parentSheetIndex, 16, 16)), Color.White * 0.75f, 0.0f, new Vector2(8f, 8f), (float) Game1.pixelZoom, SpriteEffects.None, (float) ((double) ((this.tileY + 1) * Game1.tileSize) / 10000.0 + 9.99999974737875E-06 + (double) this.tileX / 10000.0));
      }
    }
  }
}
