// Decompiled with JetBrains decompiler
// Type: StardewValley.Locations.Forest
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;
using StardewValley.TerrainFeatures;
using System.Collections.Generic;
using System.Xml.Serialization;
using xTile;
using xTile.Dimensions;

namespace StardewValley.Locations
{
  public class Forest : GameLocation
  {
    private int chimneyTimer = 500;
    private Microsoft.Xna.Framework.Rectangle hatterSource = new Microsoft.Xna.Framework.Rectangle(600, 1957, 64, 32);
    private Vector2 hatterPos = new Vector2((float) (32 * Game1.tileSize + 2 * Game1.pixelZoom), (float) (94 * Game1.tileSize));
    [XmlIgnore]
    public List<FarmAnimal> marniesLivestock;
    [XmlIgnore]
    public List<Microsoft.Xna.Framework.Rectangle> travelingMerchantBounds;
    [XmlIgnore]
    public Dictionary<Item, int[]> travelingMerchantStock;
    [XmlIgnore]
    public bool travelingMerchantDay;
    public ResourceClump log;

    public Forest()
    {
    }

    public Forest(Map map, string name)
      : base(map, name)
    {
      this.marniesLivestock = new List<FarmAnimal>();
      this.marniesLivestock.Add(new FarmAnimal("Dairy Cow", MultiplayerUtility.getNewID(), -1L));
      this.marniesLivestock.Add(new FarmAnimal("Dairy Cow", MultiplayerUtility.getNewID(), -1L));
      this.marniesLivestock[0].position = new Vector2((float) (98 * Game1.tileSize), (float) (20 * Game1.tileSize));
      this.marniesLivestock[1].position = new Vector2((float) (101 * Game1.tileSize), (float) (20 * Game1.tileSize));
      this.log = new ResourceClump(602, 2, 2, new Vector2(1f, 6f));
    }

    public override void resetForPlayerEntry()
    {
      base.resetForPlayerEntry();
      this.addFrog();
    }

    public override bool performToolAction(Tool t, int tileX, int tileY)
    {
      if (this.log == null || !this.log.getBoundingBox(this.log.tile).Contains(tileX * Game1.tileSize, tileY * Game1.tileSize))
        return base.performToolAction(t, tileX, tileY);
      if (this.log.performToolAction(t, 1, this.log.tile, (GameLocation) null))
        this.log = (ResourceClump) null;
      return true;
    }

    public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
    {
      switch (this.map.GetLayer("Buildings").Tiles[tileLocation] != null ? this.map.GetLayer("Buildings").Tiles[tileLocation].TileIndex : -1)
      {
        case 901:
          if (!who.mailReceived.Contains("wizardJunimoNote") && !who.mailReceived.Contains("JojaMember"))
          {
            Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Forest_WizardTower_Locked"));
            return false;
          }
          break;
        case 1394:
          if (who.hasRustyKey && !who.mailReceived.Contains("OpenedSewer"))
          {
            Game1.playSound("openBox");
            Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\Locations:Forest_OpenedSewer")));
            who.mailReceived.Add("OpenedSewer");
            break;
          }
          if (who.mailReceived.Contains("OpenedSewer"))
          {
            Game1.warpFarmer("Sewer", 3, 48, 0);
            Game1.playSound("openChest");
            break;
          }
          Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:LockedDoor"));
          break;
        case 1972:
          if (who.achievements.Count > 0)
          {
            Game1.activeClickableMenu = (IClickableMenu) new ShopMenu(Utility.getHatStock(), 0, "HatMouse");
            break;
          }
          Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Forest_HatMouseStore_Abandoned"));
          break;
      }
      if (this.travelingMerchantDay && Game1.timeOfDay < 2000)
      {
        if (tileLocation.X == 27 && tileLocation.Y == 11 && this.travelingMerchantStock != null)
          Game1.activeClickableMenu = (IClickableMenu) new ShopMenu(this.travelingMerchantStock, 0, "Traveler");
        else if (tileLocation.X == 23 && tileLocation.Y == 11)
          Game1.playSound("pig");
      }
      if (this.log == null || !this.log.getBoundingBox(this.log.tile).Intersects(new Microsoft.Xna.Framework.Rectangle(tileLocation.X * Game1.tileSize, tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize)))
        return base.checkAction(tileLocation, viewport, who);
      this.log.performUseAction(new Vector2((float) tileLocation.X, (float) tileLocation.Y));
      return true;
    }

    public override bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer, int damagesFarmer, bool glider, Character character)
    {
      if (this.log != null && this.log.getBoundingBox(this.log.tile).Intersects(position))
        return true;
      if (this.travelingMerchantBounds != null)
      {
        foreach (Microsoft.Xna.Framework.Rectangle travelingMerchantBound in this.travelingMerchantBounds)
        {
          if (position.Intersects(travelingMerchantBound))
            return true;
        }
      }
      return base.isCollidingPosition(position, viewport, isFarmer, damagesFarmer, glider, character);
    }

    public override void DayUpdate(int dayOfMonth)
    {
      base.DayUpdate(dayOfMonth);
      if (dayOfMonth % 7 % 5 == 0)
      {
        this.travelingMerchantDay = true;
        this.travelingMerchantBounds = new List<Microsoft.Xna.Framework.Rectangle>();
        this.travelingMerchantBounds.Add(new Microsoft.Xna.Framework.Rectangle(23 * Game1.tileSize, 10 * Game1.tileSize, 123 * Game1.pixelZoom, 28 * Game1.pixelZoom));
        this.travelingMerchantBounds.Add(new Microsoft.Xna.Framework.Rectangle(23 * Game1.tileSize + 45 * Game1.pixelZoom, 10 * Game1.tileSize + 26 * Game1.pixelZoom, 19 * Game1.pixelZoom, 12 * Game1.pixelZoom));
        this.travelingMerchantBounds.Add(new Microsoft.Xna.Framework.Rectangle(23 * Game1.tileSize + 85 * Game1.pixelZoom, 10 * Game1.tileSize + 26 * Game1.pixelZoom, 26 * Game1.pixelZoom, 12 * Game1.pixelZoom));
        this.travelingMerchantStock = Utility.getTravelingMerchantStock();
        foreach (Microsoft.Xna.Framework.Rectangle travelingMerchantBound in this.travelingMerchantBounds)
          Utility.clearObjectsInArea(travelingMerchantBound, (GameLocation) this);
      }
      else
      {
        this.travelingMerchantBounds = (List<Microsoft.Xna.Framework.Rectangle>) null;
        this.travelingMerchantDay = false;
        this.travelingMerchantStock = (Dictionary<Item, int[]>) null;
      }
      if (!Game1.currentSeason.Equals("spring"))
        return;
      for (int index = 0; index < 7; ++index)
      {
        Vector2 tileLocation = new Vector2((float) Game1.random.Next(70, this.map.Layers[0].LayerWidth - 10), (float) Game1.random.Next(68, this.map.Layers[0].LayerHeight - 15));
        if ((double) tileLocation.Y > 30.0)
        {
          foreach (Vector2 openTile in Utility.recursiveFindOpenTiles((GameLocation) this, tileLocation, 16, 50))
          {
            string str = this.doesTileHaveProperty((int) openTile.X, (int) openTile.Y, "Diggable", "Back");
            if (!this.terrainFeatures.ContainsKey(openTile) && str != null && Game1.random.NextDouble() < 1.0 - (double) Vector2.Distance(tileLocation, openTile) * 0.150000005960464)
              this.terrainFeatures.Add(openTile, (TerrainFeature) new HoeDirt(0, new Crop(true, 1, (int) openTile.X, (int) openTile.Y)));
          }
        }
      }
    }

    public override void UpdateWhenCurrentLocation(GameTime time)
    {
      base.UpdateWhenCurrentLocation(time);
      foreach (FarmAnimal farmAnimal in this.marniesLivestock)
        farmAnimal.updateWhenCurrentLocation(time, (GameLocation) this);
      if (this.log != null)
        this.log.tickUpdate(time, this.log.tile);
      if (Game1.timeOfDay >= 2000)
        return;
      if (this.travelingMerchantDay)
      {
        if (Game1.random.NextDouble() < 0.001)
          this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(99, 1423, 13, 19), new Vector2((float) (23 * Game1.tileSize), (float) (10 * Game1.tileSize + Game1.tileSize / 2 - Game1.pixelZoom)), false, 0.0f, Color.White)
          {
            interval = (float) Game1.random.Next(500, 1500),
            layerDepth = (float) ((double) (12 * Game1.tileSize) / 10000.0 + 1.99999994947575E-05),
            scale = (float) Game1.pixelZoom
          });
        if (Game1.random.NextDouble() < 0.001)
        {
          List<TemporaryAnimatedSprite> temporarySprites = this.temporarySprites;
          TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(51, 1444, 5, 5), new Vector2((float) (23 * Game1.tileSize + Game1.tileSize / 2 - Game1.pixelZoom), (float) (11 * Game1.tileSize + Game1.tileSize / 2 + Game1.pixelZoom * 2)), false, 0.0f, Color.White);
          temporaryAnimatedSprite.interval = 500f;
          temporaryAnimatedSprite.animationLength = 1;
          double num = (double) (12 * Game1.tileSize) / 10000.0 + 1.99999994947575E-05;
          temporaryAnimatedSprite.layerDepth = (float) num;
          double pixelZoom = (double) Game1.pixelZoom;
          temporaryAnimatedSprite.scale = (float) pixelZoom;
          temporarySprites.Add(temporaryAnimatedSprite);
        }
        if (Game1.random.NextDouble() < 0.003)
        {
          List<TemporaryAnimatedSprite> temporarySprites = this.temporarySprites;
          TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(89, 1445, 6, 3), new Vector2((float) (27 * Game1.tileSize + Game1.tileSize / 2 + Game1.pixelZoom), (float) (10 * Game1.tileSize + 6 * Game1.pixelZoom)), false, 0.0f, Color.White);
          temporaryAnimatedSprite.interval = 50f;
          temporaryAnimatedSprite.animationLength = 3;
          temporaryAnimatedSprite.pingPong = true;
          temporaryAnimatedSprite.totalNumberOfLoops = 1;
          double num = (double) (12 * Game1.tileSize) / 10000.0 + 1.99999994947575E-05;
          temporaryAnimatedSprite.layerDepth = (float) num;
          double pixelZoom = (double) Game1.pixelZoom;
          temporaryAnimatedSprite.scale = (float) pixelZoom;
          temporarySprites.Add(temporaryAnimatedSprite);
        }
      }
      this.chimneyTimer = this.chimneyTimer - time.ElapsedGameTime.Milliseconds;
      if (this.chimneyTimer > 0)
        return;
      this.chimneyTimer = this.travelingMerchantDay ? 500 : Game1.random.Next(200, 2000);
      Vector2 position = this.travelingMerchantDay ? new Vector2((float) (29 * Game1.tileSize + Game1.pixelZoom * 3), (float) (8 * Game1.tileSize + Game1.pixelZoom * 3)) : new Vector2((float) (87 * Game1.tileSize + Game1.pixelZoom * 6), (float) (9 * Game1.tileSize + Game1.pixelZoom * 8));
      List<TemporaryAnimatedSprite> temporarySprites1 = this.temporarySprites;
      TemporaryAnimatedSprite temporaryAnimatedSprite1 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(372, 1956, 10, 10), position, false, 1f / 500f, Color.Gray);
      temporaryAnimatedSprite1.alpha = 0.75f;
      Vector2 vector2_1 = new Vector2(0.0f, -0.5f);
      temporaryAnimatedSprite1.motion = vector2_1;
      Vector2 vector2_2 = new Vector2(1f / 500f, 0.0f);
      temporaryAnimatedSprite1.acceleration = vector2_2;
      double num1 = 99999.0;
      temporaryAnimatedSprite1.interval = (float) num1;
      double num2 = 1.0;
      temporaryAnimatedSprite1.layerDepth = (float) num2;
      double num3 = (double) (Game1.pixelZoom * 3) / 4.0;
      temporaryAnimatedSprite1.scale = (float) num3;
      double num4 = 0.00999999977648258;
      temporaryAnimatedSprite1.scaleChange = (float) num4;
      double num5 = (double) Game1.random.Next(-5, 6) * 3.14159274101257 / 256.0;
      temporaryAnimatedSprite1.rotationChange = (float) num5;
      temporarySprites1.Add(temporaryAnimatedSprite1);
      if (!this.travelingMerchantDay)
        return;
      this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(225, 1388, 7, 5), new Vector2((float) (29 * Game1.tileSize + Game1.pixelZoom * 3), (float) (8 * Game1.tileSize + Game1.pixelZoom * 6)), false, 0.0f, Color.White)
      {
        interval = (float) (this.chimneyTimer - this.chimneyTimer / 5),
        animationLength = 1,
        layerDepth = 0.99f,
        scale = (float) ((double) Game1.pixelZoom + 0.300000011920929),
        scaleChange = -0.015f
      });
    }

    public override void performTenMinuteUpdate(int timeOfDay)
    {
      base.performTenMinuteUpdate(timeOfDay);
      if (!this.travelingMerchantDay || Game1.random.NextDouble() >= 0.4)
        return;
      List<TemporaryAnimatedSprite> temporarySprites1 = this.temporarySprites;
      TemporaryAnimatedSprite temporaryAnimatedSprite1 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(57, 1430, 4, 12), new Vector2((float) (28 * Game1.tileSize), (float) (10 * Game1.tileSize + 4 * Game1.pixelZoom)), false, 0.0f, Color.White);
      temporaryAnimatedSprite1.interval = 50f;
      temporaryAnimatedSprite1.animationLength = 10;
      temporaryAnimatedSprite1.pingPong = true;
      temporaryAnimatedSprite1.totalNumberOfLoops = 1;
      double num1 = (double) (12 * Game1.tileSize) / 10000.0 + 1.99999994947575E-05;
      temporaryAnimatedSprite1.layerDepth = (float) num1;
      double pixelZoom1 = (double) Game1.pixelZoom;
      temporaryAnimatedSprite1.scale = (float) pixelZoom1;
      temporarySprites1.Add(temporaryAnimatedSprite1);
      if (Game1.random.NextDouble() >= 0.66)
        return;
      List<TemporaryAnimatedSprite> temporarySprites2 = this.temporarySprites;
      TemporaryAnimatedSprite temporaryAnimatedSprite2 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(89, 1445, 6, 3), new Vector2((float) (27 * Game1.tileSize + Game1.tileSize / 2 + Game1.pixelZoom), (float) (10 * Game1.tileSize + 6 * Game1.pixelZoom)), false, 0.0f, Color.White);
      temporaryAnimatedSprite2.interval = 50f;
      temporaryAnimatedSprite2.animationLength = 3;
      temporaryAnimatedSprite2.pingPong = true;
      temporaryAnimatedSprite2.totalNumberOfLoops = 1;
      double num2 = (double) (12 * Game1.tileSize) / 10000.0 + 2.99999992421363E-05;
      temporaryAnimatedSprite2.layerDepth = (float) num2;
      double pixelZoom2 = (double) Game1.pixelZoom;
      temporaryAnimatedSprite2.scale = (float) pixelZoom2;
      temporarySprites2.Add(temporaryAnimatedSprite2);
    }

    public override int getFishingLocation(Vector2 tile)
    {
      return (double) tile.X < 53.0 && (double) tile.Y < 43.0 ? 1 : 0;
    }

    public override Object getFish(float millisecondsAfterNibble, int bait, int waterDepth, Farmer who, double baitPotency)
    {
      if (Game1.currentSeason.Equals("winter") && who.getTileX() == 58 && (who.getTileY() == 87 && who.FishingLevel >= 6) && (!who.fishCaught.ContainsKey(775) && waterDepth >= 3 && Game1.random.NextDouble() < 0.5))
        return new Object(775, 1, false, -1, 0);
      return base.getFish(millisecondsAfterNibble, bait, waterDepth, who, baitPotency);
    }

    public override void draw(SpriteBatch spriteBatch)
    {
      base.draw(spriteBatch);
      foreach (Character character in this.marniesLivestock)
        character.draw(spriteBatch);
      if (this.log != null)
        this.log.draw(spriteBatch, this.log.tile);
      if (this.travelingMerchantDay)
      {
        spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(new Vector2((float) (24 * Game1.tileSize), (float) (8 * Game1.tileSize))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(142, 1382, 109, 70)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, (float) (12 * Game1.tileSize) / 10000f);
        spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(new Vector2((float) (23 * Game1.tileSize), (float) (10 * Game1.tileSize + Game1.tileSize / 2))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(112, 1424, 30, 24)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, (float) ((double) (12 * Game1.tileSize) / 10000.0 + 9.99999974737875E-06));
        spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(new Vector2((float) (24 * Game1.tileSize), (float) (11 * Game1.tileSize + Game1.tileSize / 2 - Game1.pixelZoom * 2))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(142, 1424, 16, 3)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, (float) ((double) (12 * Game1.tileSize) / 10000.0 + 1.99999994947575E-05));
        spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(new Vector2((float) (24 * Game1.tileSize + Game1.pixelZoom * 2), (float) (10 * Game1.tileSize - Game1.tileSize / 2 - Game1.pixelZoom * 2))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(71, 1966, 18, 18)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, (float) ((double) (12 * Game1.tileSize) / 10000.0 - 1.99999994947575E-05));
        spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(new Vector2((float) (23 * Game1.tileSize), (float) (10 * Game1.tileSize - Game1.tileSize / 2))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(167, 1966, 18, 18)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, (float) ((double) (12 * Game1.tileSize) / 10000.0 - 1.99999994947575E-05));
        if (Game1.timeOfDay >= 2000)
          spriteBatch.Draw(Game1.staminaRect, Game1.GlobalToLocal(Game1.viewport, new Microsoft.Xna.Framework.Rectangle(27 * Game1.tileSize + Game1.tileSize / 4, 10 * Game1.tileSize, Game1.tileSize, Game1.tileSize)), new Microsoft.Xna.Framework.Rectangle?(Game1.staminaRect.Bounds), Color.Black, 0.0f, Vector2.Zero, SpriteEffects.None, (float) ((double) (12 * Game1.tileSize) / 10000.0 + 3.9999998989515E-05));
      }
      if (Game1.player.achievements.Count <= 0)
        return;
      spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(this.hatterPos), new Microsoft.Xna.Framework.Rectangle?(this.hatterSource), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, (float) (94 * Game1.tileSize) / 10000f);
    }
  }
}
