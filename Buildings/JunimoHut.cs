// Decompiled with JetBrains decompiler
// Type: StardewValley.Buildings.JunimoHut
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using StardewValley.Characters;
using StardewValley.Menus;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace StardewValley.Buildings
{
  public class JunimoHut : Building
  {
    [XmlIgnore]
    public List<JunimoHarvester> myJunimos = new List<JunimoHarvester>();
    [XmlIgnore]
    public Point lastKnownCropLocation = Point.Zero;
    private Rectangle lightInteriorRect = new Rectangle(195, 0, 18, 17);
    private Rectangle bagRect = new Rectangle(208, 51, 15, 13);
    public const int cropHarvestRadius = 8;
    public Chest output;
    public bool noHarvest;
    public Rectangle sourceRect;
    private int junimoSendOutTimer;
    private bool wasLit;

    public JunimoHut(BluePrint b, Vector2 tileLocation)
      : base(b, tileLocation)
    {
      this.sourceRect = this.getSourceRectForMenu();
      this.output = new Chest(true);
    }

    public JunimoHut()
    {
      this.sourceRect = this.getSourceRectForMenu();
    }

    public override Rectangle getRectForAnimalDoor()
    {
      return new Rectangle((1 + this.tileX) * Game1.tileSize, (this.tileY + 1) * Game1.tileSize, Game1.tileSize, Game1.tileSize);
    }

    public override Rectangle getSourceRectForMenu()
    {
      return new Rectangle(Utility.getSeasonNumber(Game1.currentSeason) * 48, 0, 48, 64);
    }

    public override void load()
    {
      base.load();
      this.sourceRect = this.getSourceRectForMenu();
    }

    public override void dayUpdate(int dayOfMonth)
    {
      base.dayUpdate(dayOfMonth);
      int constructionLeft = this.daysOfConstructionLeft;
      this.sourceRect = this.getSourceRectForMenu();
      this.myJunimos.Clear();
      this.wasLit = false;
    }

    public void sendOutJunimos()
    {
      this.junimoSendOutTimer = 1000;
    }

    public override void performActionOnConstruction(GameLocation location)
    {
      base.performActionOnConstruction(location);
      this.sendOutJunimos();
    }

    public override void performActionOnPlayerLocationEntry()
    {
      base.performActionOnPlayerLocationEntry();
      if (Game1.timeOfDay < 2000 || Game1.timeOfDay >= 2400 || Game1.IsWinter)
        return;
      Game1.currentLightSources.Add(new LightSource(4, new Vector2((float) (this.tileX + 1), (float) (this.tileY + 1)) * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2)), 0.5f)
      {
        identifier = this.tileX + this.tileY * 777
      });
      AmbientLocationSounds.addSound(new Vector2((float) (this.tileX + 1), (float) (this.tileY + 1)), 1);
      this.wasLit = true;
    }

    public int getUnusedJunimoNumber()
    {
      for (int index = 0; index < 3; ++index)
      {
        if (index >= this.myJunimos.Count<JunimoHarvester>())
          return index;
        bool flag = false;
        foreach (JunimoHarvester junimo in this.myJunimos)
        {
          if (junimo.whichJunimoFromThisHut == index)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          return index;
      }
      return 2;
    }

    public override void Update(GameTime time)
    {
      base.Update(time);
      if (this.junimoSendOutTimer <= 0)
        return;
      this.junimoSendOutTimer = this.junimoSendOutTimer - time.ElapsedGameTime.Milliseconds;
      if (this.junimoSendOutTimer > 0 || this.myJunimos.Count<JunimoHarvester>() >= 3 || (Game1.IsWinter || Game1.isRaining) || (!this.areThereMatureCropsWithinRadius() || Game1.farmEvent != null))
        return;
      JunimoHarvester junimoHarvester = new JunimoHarvester(new Vector2((float) (this.tileX + 1), (float) (this.tileY + 1)) * (float) Game1.tileSize + new Vector2(0.0f, (float) (Game1.tileSize / 2)), this, this.getUnusedJunimoNumber());
      Game1.getFarm().characters.Add((NPC) junimoHarvester);
      this.myJunimos.Add(junimoHarvester);
      this.junimoSendOutTimer = 1000;
      if (!Utility.isOnScreen(Utility.Vector2ToPoint(new Vector2((float) (this.tileX + 1), (float) (this.tileY + 1))), Game1.tileSize, (GameLocation) Game1.getFarm()))
        return;
      try
      {
        Game1.playSound("junimoMeep1");
      }
      catch (Exception ex)
      {
      }
    }

    public bool areThereMatureCropsWithinRadius()
    {
      Farm farm = Game1.getFarm();
      for (int index1 = this.tileX + 1 - 8; index1 < this.tileX + 2 + 8; ++index1)
      {
        for (int index2 = this.tileY - 8 + 1; index2 < this.tileY + 2 + 8; ++index2)
        {
          if (farm.isCropAtTile(index1, index2) && (farm.terrainFeatures[new Vector2((float) index1, (float) index2)] as HoeDirt).readyForHarvest())
          {
            this.lastKnownCropLocation = new Point(index1, index2);
            return true;
          }
        }
      }
      this.lastKnownCropLocation = Point.Zero;
      return false;
    }

    public override void performTenMinuteAction(int timeElapsed)
    {
      base.performTenMinuteAction(timeElapsed);
      for (int index = this.myJunimos.Count - 1; index >= 0; --index)
      {
        if (!Game1.getFarm().characters.Contains((NPC) this.myJunimos[index]))
          this.myJunimos.RemoveAt(index);
        else
          this.myJunimos[index].pokeToHarvest();
      }
      if (this.myJunimos.Count<JunimoHarvester>() < 3 && Game1.timeOfDay < 1900)
        this.junimoSendOutTimer = 1;
      if (Game1.timeOfDay >= 2000 && Game1.timeOfDay < 2400 && (!Game1.IsWinter && Utility.getLightSource(this.tileX + this.tileY * 777) == null) && Game1.random.NextDouble() < 0.2)
      {
        Game1.currentLightSources.Add(new LightSource(4, new Vector2((float) (this.tileX + 1), (float) (this.tileY + 1)) * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2)), 0.5f)
        {
          identifier = this.tileX + this.tileY * 777
        });
        AmbientLocationSounds.addSound(new Vector2((float) (this.tileX + 1), (float) (this.tileY + 1)), 1);
        this.wasLit = true;
      }
      else
      {
        if (Game1.timeOfDay != 2400 || Game1.IsWinter)
          return;
        Utility.removeLightSource(this.tileX + this.tileY * 777);
        AmbientLocationSounds.removeSound(new Vector2((float) (this.tileX + 1), (float) (this.tileY + 1)));
      }
    }

    public override bool doAction(Vector2 tileLocation, Farmer who)
    {
      if (!who.IsMainPlayer || (double) tileLocation.X < (double) this.tileX || ((double) tileLocation.X >= (double) (this.tileX + this.tilesWide) || (double) tileLocation.Y < (double) this.tileY) || ((double) tileLocation.Y >= (double) (this.tileY + this.tilesHigh) || this.output == null))
        return base.doAction(tileLocation, who);
      Game1.activeClickableMenu = (IClickableMenu) new ItemGrabMenu(this.output.items, false, true, new InventoryMenu.highlightThisItem(InventoryMenu.highlightAllItems), new ItemGrabMenu.behaviorOnItemSelect(this.output.grabItemFromInventory), (string) null, new ItemGrabMenu.behaviorOnItemSelect(this.output.grabItemFromChest), false, true, true, true, true, 1, (Item) null, 1, (object) this);
      return true;
    }

    public override void drawInMenu(SpriteBatch b, int x, int y)
    {
      this.drawShadow(b, x, y);
      b.Draw(this.texture, new Vector2((float) x, (float) y), new Rectangle?(new Rectangle(0, 0, 48, 64)), this.color, 0.0f, new Vector2(0.0f, 0.0f), (float) Game1.pixelZoom, SpriteEffects.None, 0.89f);
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
        b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (this.tileX * Game1.tileSize), (float) (this.tileY * Game1.tileSize + this.tilesHigh * Game1.tileSize))), new Rectangle?(this.sourceRect), this.color * this.alpha, 0.0f, new Vector2(0.0f, (float) this.texture.Bounds.Height), 4f, SpriteEffects.None, (float) ((this.tileY + this.tilesHigh - 1) * Game1.tileSize) / 10000f);
        if (!this.output.isEmpty())
          b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (this.tileX * Game1.tileSize + Game1.tileSize * 2 + Game1.pixelZoom * 3), (float) (this.tileY * Game1.tileSize + this.tilesHigh * Game1.tileSize - Game1.tileSize / 2))), new Rectangle?(this.bagRect), this.color * this.alpha, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, (float) ((this.tileY + this.tilesHigh - 1) * Game1.tileSize + 1) / 10000f);
        if (Game1.timeOfDay < 2000 || Game1.timeOfDay >= 2400 || (Game1.IsWinter || !this.wasLit))
          return;
        b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (this.tileX * Game1.tileSize + Game1.tileSize), (float) (this.tileY * Game1.tileSize + this.tilesHigh * Game1.tileSize - Game1.tileSize))), new Rectangle?(this.lightInteriorRect), this.color * this.alpha, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, (float) ((this.tileY + this.tilesHigh - 1) * Game1.tileSize + 1) / 10000f);
      }
    }
  }
}
