// Decompiled with JetBrains decompiler
// Type: StardewValley.Buildings.Building
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Locations;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using xTile;

namespace StardewValley.Buildings
{
  [XmlInclude(typeof (Coop))]
  [XmlInclude(typeof (Barn))]
  [XmlInclude(typeof (Stable))]
  [XmlInclude(typeof (Mill))]
  [XmlInclude(typeof (JunimoHut))]
  public class Building
  {
    public static Rectangle leftShadow = new Rectangle(656, 394, 16, 16);
    public static Rectangle middleShadow = new Rectangle(672, 394, 16, 16);
    public static Rectangle rightShadow = new Rectangle(688, 394, 16, 16);
    public Color color = Color.White;
    public GameLocation indoors;
    [XmlIgnore]
    public Texture2D texture;
    public int tileX;
    public int tileY;
    public int tilesWide;
    public int tilesHigh;
    public int maxOccupants;
    public int currentOccupants;
    public int daysOfConstructionLeft;
    public int daysUntilUpgrade;
    public string buildingType;
    public string nameOfIndoors;
    public string baseNameOfIndoors;
    public string nameOfIndoorsWithoutUnique;
    public Point humanDoor;
    public Point animalDoor;
    public bool animalDoorOpen;
    public bool magical;
    public long owner;
    private int newConstructionTimer;
    protected float alpha;

    public Building()
    {
    }

    public Building(string buildingType, string nameOfIndoors, int tileX, int tileY, int tilesWide, int tilesTall, Point humanDoor, Point animalDoor, GameLocation indoors, Texture2D texture, bool magical, long owner)
    {
      this.tileX = tileX;
      this.tileY = tileY;
      this.tilesWide = tilesWide;
      this.tilesHigh = tilesTall;
      this.buildingType = buildingType;
      this.nameOfIndoors = nameOfIndoors + (object) (tileX * 2000 + tileY);
      this.texture = texture;
      this.indoors = indoors;
      this.baseNameOfIndoors = indoors.name;
      this.nameOfIndoorsWithoutUnique = this.baseNameOfIndoors;
      this.humanDoor = humanDoor;
      this.animalDoor = animalDoor;
      this.daysOfConstructionLeft = 2;
      this.magical = magical;
    }

    public int getTileSheetIndexForStructurePlacementTile(int x, int y)
    {
      if (x == this.humanDoor.X && y == this.humanDoor.Y)
        return 2;
      return x == this.animalDoor.X && y == this.animalDoor.Y ? 4 : 0;
    }

    public virtual void performTenMinuteAction(int timeElapsed)
    {
    }

    public virtual void performActionOnPlayerLocationEntry()
    {
      this.color = Color.White;
    }

    public virtual bool doAction(Vector2 tileLocation, Farmer who)
    {
      if (who.IsMainPlayer && (double) tileLocation.X >= (double) this.tileX && ((double) tileLocation.X < (double) (this.tileX + this.tilesWide) && (double) tileLocation.Y >= (double) this.tileY) && ((double) tileLocation.Y < (double) (this.tileY + this.tilesHigh) && this.daysOfConstructionLeft > 0))
      {
        Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Buildings:UnderConstruction"));
      }
      else
      {
        if (who.IsMainPlayer && (double) tileLocation.X == (double) (this.humanDoor.X + this.tileX) && ((double) tileLocation.Y == (double) (this.humanDoor.Y + this.tileY) && this.indoors != null))
        {
          if (who.getMount() != null)
          {
            Game1.showRedMessage(Game1.content.LoadString("Strings\\Buildings:DismountBeforeEntering"));
            return false;
          }
          this.indoors.isStructure = true;
          this.indoors.uniqueName = this.baseNameOfIndoors + (object) (this.tileX * 2000 + this.tileY);
          Game1.warpFarmer(this.indoors, this.indoors.warps[0].X, this.indoors.warps[0].Y - 1, Game1.player.facingDirection, true);
          Game1.playSound("doorClose");
          return true;
        }
        if (who.IsMainPlayer && this.buildingType.Equals("Silo") && !this.isTilePassable(tileLocation))
        {
          if (who.ActiveObject != null && who.ActiveObject.parentSheetIndex == 178)
          {
            if (who.ActiveObject.Stack == 0)
              who.ActiveObject.stack = 1;
            int stack = who.ActiveObject.Stack;
            int addHay = (Game1.getLocationFromName("Farm") as Farm).tryToAddHay(who.ActiveObject.Stack);
            who.ActiveObject.stack = addHay;
            if (who.ActiveObject.stack < stack)
            {
              Game1.playSound("Ship");
              DelayedAction.playSoundAfterDelay("grassyStep", 100);
              Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Buildings:AddedHay", (object) (stack - who.ActiveObject.Stack)));
            }
            if (who.ActiveObject.Stack <= 0)
              who.removeItemFromInventory((Item) who.ActiveObject);
          }
          else
            Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Buildings:PiecesOfHay", (object) (Game1.getLocationFromName("Farm") as Farm).piecesOfHay, (object) (Utility.numSilos() * 240)));
        }
        else if (who.IsMainPlayer && this.buildingType.Contains("Obelisk") && !this.isTilePassable(tileLocation))
        {
          for (int index = 0; index < 12; ++index)
            who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(354, (float) Game1.random.Next(25, 75), 6, 1, new Vector2((float) Game1.random.Next((int) who.position.X - Game1.tileSize * 4, (int) who.position.X + Game1.tileSize * 3), (float) Game1.random.Next((int) who.position.Y - Game1.tileSize * 4, (int) who.position.Y + Game1.tileSize * 3)), false, Game1.random.NextDouble() < 0.5));
          Game1.playSound("wand");
          Game1.displayFarmer = false;
          Game1.player.freezePause = 1000;
          Game1.flashAlpha = 1f;
          DelayedAction.fadeAfterDelay(new Game1.afterFadeFunction(this.obeliskWarpForReal), 1000);
          new Rectangle(who.GetBoundingBox().X, who.GetBoundingBox().Y, Game1.tileSize, Game1.tileSize).Inflate(Game1.tileSize * 3, Game1.tileSize * 3);
          int num1 = 0;
          for (int index = who.getTileX() + 8; index >= who.getTileX() - 8; --index)
          {
            List<TemporaryAnimatedSprite> temporarySprites = who.currentLocation.temporarySprites;
            TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(6, new Vector2((float) index, (float) who.getTileY()) * (float) Game1.tileSize, Color.White, 8, false, 50f, 0, -1, -1f, -1, 0);
            temporaryAnimatedSprite.layerDepth = 1f;
            int num2 = num1 * 25;
            temporaryAnimatedSprite.delayBeforeAnimationStart = num2;
            Vector2 vector2 = new Vector2(-0.25f, 0.0f);
            temporaryAnimatedSprite.motion = vector2;
            temporarySprites.Add(temporaryAnimatedSprite);
            ++num1;
          }
        }
      }
      return false;
    }

    private void obeliskWarpForReal()
    {
      string buildingType = this.buildingType;
      if (!(buildingType == "Earth Obelisk"))
      {
        if (buildingType == "Water Obelisk")
          Game1.warpFarmer("Beach", 20, 4, false);
      }
      else
        Game1.warpFarmer("Mountain", 31, 20, false);
      Game1.fadeToBlackAlpha = 0.99f;
      Game1.screenGlow = false;
      Game1.player.temporarilyInvincible = false;
      Game1.player.temporaryInvincibilityTimer = 0;
      Game1.displayFarmer = true;
    }

    public virtual bool isActionableTile(int xTile, int yTile, Farmer who)
    {
      return this.humanDoor.X >= 0 && xTile == this.tileX + this.humanDoor.X && yTile == this.tileY + this.humanDoor.Y || this.animalDoor.X >= 0 && xTile == this.tileX + this.animalDoor.X && yTile == this.tileY + this.animalDoor.Y;
    }

    public virtual void performActionOnConstruction(GameLocation location)
    {
      this.daysOfConstructionLeft = 2;
      this.newConstructionTimer = this.magical ? 2000 : 1000;
      if (!this.magical)
      {
        Game1.playSound("axchop");
        for (int tileX = this.tileX; tileX < this.tileX + this.tilesWide; ++tileX)
        {
          for (int tileY = this.tileY; tileY < this.tileY + this.tilesHigh; ++tileY)
          {
            for (int index = 0; index < 5; ++index)
              location.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.random.NextDouble() < 0.5 ? 46 : 12, new Vector2((float) tileX, (float) tileY) * (float) Game1.tileSize + new Vector2((float) Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 2), (float) Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 2)), Color.White, 10, Game1.random.NextDouble() < 0.5, 100f, 0, -1, -1f, -1, 0)
              {
                delayBeforeAnimationStart = Math.Max(0, Game1.random.Next(-200, 400)),
                motion = new Vector2(0.0f, -1f),
                interval = (float) Game1.random.Next(50, 80)
              });
            location.temporarySprites.Add(new TemporaryAnimatedSprite(14, new Vector2((float) tileX, (float) tileY) * (float) Game1.tileSize + new Vector2((float) Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 2), (float) Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 2)), Color.White, 10, Game1.random.NextDouble() < 0.5, 100f, 0, -1, -1f, -1, 0));
          }
        }
        for (int index = 0; index < 8; ++index)
          DelayedAction.playSoundAfterDelay("dirtyHit", 250 + index * 150);
      }
      else
      {
        for (int index = 0; index < 8; ++index)
          DelayedAction.playSoundAfterDelay("dirtyHit", 100 + index * 210);
        Game1.flashAlpha = 2f;
        Game1.playSound("wand");
        for (int index1 = 0; index1 < this.getSourceRectForMenu().Width / 16 * 2; ++index1)
        {
          for (int index2 = this.texture.Bounds.Height / 16 * 2; index2 >= 0; --index2)
          {
            List<TemporaryAnimatedSprite> temporarySprites1 = location.temporarySprites;
            TemporaryAnimatedSprite temporaryAnimatedSprite1 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(666, 1851, 8, 8), 40f, 4, 2, new Vector2((float) this.tileX, (float) this.tileY) * (float) Game1.tileSize + new Vector2((float) (index1 * Game1.tileSize / 2), (float) (index2 * Game1.tileSize / 2 - this.texture.Bounds.Height * Game1.pixelZoom + this.tilesHigh * Game1.tileSize)) + new Vector2((float) Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize / 2), (float) Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize / 2)), false, false);
            temporaryAnimatedSprite1.layerDepth = (float) ((double) ((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000.0 + (double) index1 / 10000.0);
            int num1 = 1;
            temporaryAnimatedSprite1.pingPong = num1 != 0;
            int num2 = (this.texture.Bounds.Height / 16 * 2 - index2) * 100;
            temporaryAnimatedSprite1.delayBeforeAnimationStart = num2;
            double pixelZoom1 = (double) Game1.pixelZoom;
            temporaryAnimatedSprite1.scale = (float) pixelZoom1;
            double num3 = 0.00999999977648258;
            temporaryAnimatedSprite1.alphaFade = (float) num3;
            Color aliceBlue1 = Color.AliceBlue;
            temporaryAnimatedSprite1.color = aliceBlue1;
            temporarySprites1.Add(temporaryAnimatedSprite1);
            List<TemporaryAnimatedSprite> temporarySprites2 = location.temporarySprites;
            TemporaryAnimatedSprite temporaryAnimatedSprite2 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(666, 1851, 8, 8), 40f, 4, 2, new Vector2((float) this.tileX, (float) this.tileY) * (float) Game1.tileSize + new Vector2((float) (index1 * Game1.tileSize / 2), (float) (index2 * Game1.tileSize / 2 - this.texture.Bounds.Height * Game1.pixelZoom + this.tilesHigh * Game1.tileSize)) + new Vector2((float) Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize / 2), (float) Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize / 2)), false, false);
            temporaryAnimatedSprite2.layerDepth = (float) ((double) ((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000.0 + (double) index1 / 10000.0 + 9.99999974737875E-05);
            int num4 = 1;
            temporaryAnimatedSprite2.pingPong = num4 != 0;
            int num5 = (this.texture.Bounds.Height / 16 * 2 - index2) * 100;
            temporaryAnimatedSprite2.delayBeforeAnimationStart = num5;
            double pixelZoom2 = (double) Game1.pixelZoom;
            temporaryAnimatedSprite2.scale = (float) pixelZoom2;
            double num6 = 0.00999999977648258;
            temporaryAnimatedSprite2.alphaFade = (float) num6;
            Color aliceBlue2 = Color.AliceBlue;
            temporaryAnimatedSprite2.color = aliceBlue2;
            temporarySprites2.Add(temporaryAnimatedSprite2);
          }
        }
      }
    }

    public virtual void performActionOnDemolition(GameLocation location)
    {
    }

    public virtual void performActionOnUpgrade(GameLocation location)
    {
    }

    public virtual string isThereAnythingtoPreventConstruction(GameLocation location)
    {
      return (string) null;
    }

    public virtual void updateWhenFarmNotCurrentLocation(GameTime time)
    {
      if (this.indoors == null)
        return;
      this.indoors.updateEvenIfFarmerIsntHere(time, false);
    }

    public virtual void Update(GameTime time)
    {
      if (this.newConstructionTimer > 0)
      {
        this.newConstructionTimer = this.newConstructionTimer - time.ElapsedGameTime.Milliseconds;
        if (this.newConstructionTimer <= 0 && this.magical)
          this.daysOfConstructionLeft = 0;
      }
      this.alpha = Math.Min(1f, this.alpha + 0.05f);
      if (!Game1.player.GetBoundingBox().Intersects(new Rectangle(Game1.tileSize * this.tileX, Game1.tileSize * (this.tileY + (-(this.getSourceRectForMenu().Height / 16) + this.tilesHigh)), this.tilesWide * Game1.tileSize, (this.getSourceRectForMenu().Height / 16 - this.tilesHigh) * Game1.tileSize + Game1.tileSize / 2)))
        return;
      this.alpha = Math.Max(0.4f, this.alpha - 0.09f);
    }

    public void showUpgradeAnimation(GameLocation location)
    {
      this.color = Color.White;
      location.temporarySprites.Add(new TemporaryAnimatedSprite(46, this.getUpgradeSignLocation() + new Vector2((float) Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 4), (float) Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 4)), Color.Beige, 10, Game1.random.NextDouble() < 0.5, 75f, 0, -1, -1f, -1, 0)
      {
        motion = new Vector2(0.0f, -0.5f),
        acceleration = new Vector2(-0.02f, 0.01f),
        delayBeforeAnimationStart = Game1.random.Next(100),
        layerDepth = 0.89f
      });
      location.temporarySprites.Add(new TemporaryAnimatedSprite(46, this.getUpgradeSignLocation() + new Vector2((float) Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 4), (float) Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 4)), Color.Beige, 10, Game1.random.NextDouble() < 0.5, 75f, 0, -1, -1f, -1, 0)
      {
        motion = new Vector2(0.0f, -0.5f),
        acceleration = new Vector2(-0.02f, 0.01f),
        delayBeforeAnimationStart = Game1.random.Next(40),
        layerDepth = 0.89f
      });
    }

    public virtual Vector2 getUpgradeSignLocation()
    {
      return new Vector2((float) (this.tileX * Game1.tileSize + Game1.tileSize / 2), (float) (this.tileY * Game1.tileSize - Game1.tileSize / 2));
    }

    public string getNameOfNextUpgrade()
    {
      string lower = this.buildingType.ToLower();
      if (lower == "coop")
        return "Big Coop";
      if (lower == "big coop")
        return "Deluxe Coop";
      if (lower == "barn")
        return "Big Barn";
      return lower == "big barn" ? "Deluxe Barn" : "well";
    }

    public void showDestroyedAnimation(GameLocation location)
    {
      for (int tileX = this.tileX; tileX < this.tileX + this.tilesWide; ++tileX)
      {
        for (int tileY = this.tileY; tileY < this.tileY + this.tilesHigh; ++tileY)
        {
          location.temporarySprites.Add(new TemporaryAnimatedSprite(362, (float) Game1.random.Next(30, 90), 6, 1, new Vector2((float) (tileX * Game1.tileSize), (float) (tileY * Game1.tileSize)) + new Vector2((float) Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 4), (float) Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 4)), false, Game1.random.NextDouble() < 0.5)
          {
            delayBeforeAnimationStart = Game1.random.Next(300)
          });
          location.temporarySprites.Add(new TemporaryAnimatedSprite(362, (float) Game1.random.Next(30, 90), 6, 1, new Vector2((float) (tileX * Game1.tileSize), (float) (tileY * Game1.tileSize)) + new Vector2((float) Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 4), (float) Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 4)), false, Game1.random.NextDouble() < 0.5)
          {
            delayBeforeAnimationStart = 250 + Game1.random.Next(300)
          });
          List<TemporaryAnimatedSprite> temporarySprites = location.temporarySprites;
          TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(276, 1985, 12, 11), new Vector2((float) tileX, (float) tileY) * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), (float) (-Game1.tileSize / 2)) + new Vector2((float) Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize / 2), (float) Game1.random.Next(-Game1.tileSize / 4, Game1.tileSize / 4)), false, 0.0f, Color.White);
          temporaryAnimatedSprite.interval = 30f;
          temporaryAnimatedSprite.totalNumberOfLoops = 99999;
          temporaryAnimatedSprite.animationLength = 4;
          double pixelZoom = (double) Game1.pixelZoom;
          temporaryAnimatedSprite.scale = (float) pixelZoom;
          double num = 0.00999999977648258;
          temporaryAnimatedSprite.alphaFade = (float) num;
          temporarySprites.Add(temporaryAnimatedSprite);
        }
      }
    }

    public virtual void dayUpdate(int dayOfMonth)
    {
      if (this.daysOfConstructionLeft > 0 && !Utility.isFestivalDay(dayOfMonth, Game1.currentSeason))
      {
        this.daysOfConstructionLeft = this.daysOfConstructionLeft - 1;
        if (this.daysOfConstructionLeft > 0)
          return;
        Game1.player.checkForQuestComplete((NPC) null, -1, -1, (Item) null, this.buildingType, 8, -1);
        if (!this.buildingType.Equals("Slime Hutch") || this.indoors == null)
          return;
        this.indoors.objects.Add(new Vector2(1f, 4f), new StardewValley.Object(new Vector2(1f, 4f), 156, false)
        {
          fragility = 2
        });
        if (Game1.player.mailReceived.Contains("slimeHutchBuilt"))
          return;
        Game1.player.mailReceived.Add("slimeHutchBuilt");
      }
      else
      {
        if (this.daysUntilUpgrade > 0 && !Utility.isFestivalDay(dayOfMonth, Game1.currentSeason))
        {
          this.daysUntilUpgrade = this.daysUntilUpgrade - 1;
          if (this.daysUntilUpgrade <= 0)
          {
            Game1.player.checkForQuestComplete((NPC) null, -1, -1, (Item) null, this.getNameOfNextUpgrade(), 8, -1);
            BluePrint bluePrint = new BluePrint(this.getNameOfNextUpgrade());
            this.indoors.map = Game1.game1.xTileContent.Load<Map>("Maps\\" + bluePrint.mapToWarpTo);
            this.indoors.name = bluePrint.mapToWarpTo;
            this.buildingType = bluePrint.name;
            this.texture = bluePrint.texture;
            if (this.indoors.GetType() == typeof (AnimalHouse))
            {
              ((AnimalHouse) this.indoors).resetPositionsOfAllAnimals();
              ((AnimalHouse) this.indoors).animalLimit += 4;
              this.indoors.loadLights();
            }
            this.upgrade();
          }
        }
        if (this.indoors != null)
          this.indoors.DayUpdate(dayOfMonth);
        if (!this.buildingType.Contains("Deluxe"))
          return;
        (this.indoors as AnimalHouse).feedAllAnimals();
      }
    }

    public virtual void upgrade()
    {
    }

    public virtual Rectangle getSourceRectForMenu()
    {
      return this.texture.Bounds;
    }

    public Building(BluePrint blueprint, Vector2 tileLocation)
    {
      this.tileX = (int) tileLocation.X;
      this.tileY = (int) tileLocation.Y;
      this.tilesWide = blueprint.tilesWidth;
      this.tilesHigh = blueprint.tilesHeight;
      this.buildingType = blueprint.name;
      this.texture = blueprint.texture;
      this.humanDoor = blueprint.humanDoor;
      this.animalDoor = blueprint.animalDoor;
      this.nameOfIndoors = blueprint.mapToWarpTo;
      this.baseNameOfIndoors = this.nameOfIndoors;
      this.nameOfIndoorsWithoutUnique = this.baseNameOfIndoors;
      this.indoors = this.getIndoors();
      this.nameOfIndoors = this.nameOfIndoors + (object) (this.tileX * 2000 + this.tileY);
      this.maxOccupants = blueprint.maxOccupants;
      this.daysOfConstructionLeft = 2;
      this.magical = blueprint.magical;
    }

    protected virtual GameLocation getIndoors()
    {
      if (this.buildingType.Equals("Slime Hutch"))
      {
        if (this.indoors != null)
          this.nameOfIndoorsWithoutUnique = this.indoors.name;
        if (this.nameOfIndoorsWithoutUnique == "Slime Hutch")
          this.nameOfIndoorsWithoutUnique = "SlimeHutch";
        GameLocation gameLocation = (GameLocation) new SlimeHutch(Game1.game1.xTileContent.Load<Map>("Maps\\" + this.nameOfIndoorsWithoutUnique), this.buildingType);
        gameLocation.IsFarm = true;
        gameLocation.isStructure = true;
        foreach (Warp warp in gameLocation.warps)
        {
          int num1 = this.humanDoor.X + this.tileX;
          warp.TargetX = num1;
          int num2 = this.humanDoor.Y + this.tileY + 1;
          warp.TargetY = num2;
        }
        return gameLocation;
      }
      if (this.buildingType.Equals("Shed"))
      {
        if (this.indoors != null)
          this.nameOfIndoorsWithoutUnique = this.indoors.name;
        if (this.nameOfIndoorsWithoutUnique == "Shed")
          this.nameOfIndoorsWithoutUnique = "Shed";
        GameLocation gameLocation = (GameLocation) new Shed(Game1.game1.xTileContent.Load<Map>("Maps\\" + this.nameOfIndoorsWithoutUnique), this.buildingType);
        gameLocation.IsFarm = true;
        gameLocation.isStructure = true;
        foreach (Warp warp in gameLocation.warps)
        {
          int num1 = this.humanDoor.X + this.tileX;
          warp.TargetX = num1;
          int num2 = this.humanDoor.Y + this.tileY + 1;
          warp.TargetY = num2;
        }
        return gameLocation;
      }
      if (this.nameOfIndoorsWithoutUnique == null || this.nameOfIndoorsWithoutUnique.Length <= 0 || this.nameOfIndoorsWithoutUnique.Equals("null"))
        return (GameLocation) null;
      GameLocation gameLocation1 = new GameLocation(Game1.game1.xTileContent.Load<Map>("Maps\\" + this.nameOfIndoorsWithoutUnique), this.buildingType);
      gameLocation1.IsFarm = true;
      gameLocation1.isStructure = true;
      if (gameLocation1.name.Equals("Greenhouse"))
        gameLocation1.terrainFeatures = new SerializableDictionary<Vector2, TerrainFeature>();
      foreach (Warp warp in gameLocation1.warps)
      {
        int num1 = this.humanDoor.X + this.tileX;
        warp.TargetX = num1;
        int num2 = this.humanDoor.Y + this.tileY + 1;
        warp.TargetY = num2;
      }
      if (gameLocation1 is AnimalHouse)
      {
        AnimalHouse animalHouse = gameLocation1 as AnimalHouse;
        string str = this.buildingType.Split(' ')[0];
        animalHouse.animalLimit = str == "Big" ? 8 : (str == "Deluxe" ? 12 : 4);
      }
      return gameLocation1;
    }

    public virtual Rectangle getRectForAnimalDoor()
    {
      return new Rectangle((this.animalDoor.X + this.tileX) * Game1.tileSize, (this.tileY + this.animalDoor.Y) * Game1.tileSize, Game1.tileSize, Game1.tileSize);
    }

    public virtual void load()
    {
      this.texture = Game1.content.Load<Texture2D>("Buildings\\" + this.buildingType);
      GameLocation indoors1 = this.getIndoors();
      if (indoors1 == null)
        return;
      indoors1.characters = this.indoors.characters;
      indoors1.objects = this.indoors.objects;
      indoors1.terrainFeatures = this.indoors.terrainFeatures;
      indoors1.IsFarm = true;
      indoors1.IsOutdoors = false;
      indoors1.isStructure = true;
      indoors1.uniqueName = indoors1.name + (object) (this.tileX * 2000 + this.tileY);
      indoors1.numberOfSpawnedObjectsOnMap = this.indoors.numberOfSpawnedObjectsOnMap;
      if (this.indoors.GetType() == typeof (AnimalHouse))
      {
        ((AnimalHouse) indoors1).animals = ((AnimalHouse) this.indoors).animals;
        ((AnimalHouse) indoors1).animalsThatLiveHere = ((AnimalHouse) this.indoors).animalsThatLiveHere;
        foreach (KeyValuePair<long, FarmAnimal> animal in (Dictionary<long, FarmAnimal>) ((AnimalHouse) indoors1).animals)
          animal.Value.reload();
      }
      if (this.indoors is Shed)
      {
        ((DecoratableLocation) indoors1).furniture = ((DecoratableLocation) this.indoors).furniture;
        foreach (Furniture furniture in ((DecoratableLocation) indoors1).furniture)
          furniture.updateDrawPosition();
        ((DecoratableLocation) indoors1).wallPaper = ((DecoratableLocation) this.indoors).wallPaper;
        ((DecoratableLocation) indoors1).floor = ((DecoratableLocation) this.indoors).floor;
      }
      this.indoors = indoors1;
      foreach (Warp warp in this.indoors.warps)
      {
        int num1 = this.humanDoor.X + this.tileX;
        warp.TargetX = num1;
        int num2 = this.humanDoor.Y + this.tileY + 1;
        warp.TargetY = num2;
      }
      if (this.indoors.IsFarm && this.indoors.terrainFeatures == null)
        this.indoors.terrainFeatures = new SerializableDictionary<Vector2, TerrainFeature>();
      foreach (NPC character in this.indoors.characters)
        character.reloadSprite();
      foreach (TerrainFeature terrainFeature in this.indoors.terrainFeatures.Values)
        terrainFeature.loadSprite();
      foreach (KeyValuePair<Vector2, StardewValley.Object> keyValuePair in (Dictionary<Vector2, StardewValley.Object>) this.indoors.objects)
      {
        keyValuePair.Value.initializeLightSource(keyValuePair.Key);
        keyValuePair.Value.reloadSprite();
      }
      if (!(this.indoors is AnimalHouse))
        return;
      AnimalHouse indoors2 = this.indoors as AnimalHouse;
      string str = this.buildingType.Split(' ')[0];
      if (!(str == "Big"))
      {
        if (str == "Deluxe")
          indoors2.animalLimit = 12;
        else
          indoors2.animalLimit = 4;
      }
      else
        indoors2.animalLimit = 8;
    }

    public bool isUnderConstruction()
    {
      return this.daysOfConstructionLeft > 0;
    }

    public virtual bool isTilePassable(Vector2 tile)
    {
      if ((double) tile.X >= (double) this.tileX && (double) tile.X < (double) (this.tileX + this.tilesWide) && (double) tile.Y >= (double) this.tileY)
        return (double) tile.Y >= (double) (this.tileY + this.tilesHigh);
      return true;
    }

    public virtual bool intersects(Rectangle boundingBox)
    {
      return new Rectangle(this.tileX * Game1.tileSize, this.tileY * Game1.tileSize, this.tilesWide * Game1.tileSize, this.tilesHigh * Game1.tileSize).Intersects(boundingBox);
    }

    public virtual void drawInMenu(SpriteBatch b, int x, int y)
    {
      if (this.tilesWide <= 8)
      {
        this.drawShadow(b, x, y);
        b.Draw(this.texture, new Vector2((float) x, (float) y), new Rectangle?(this.texture.Bounds), this.color, 0.0f, new Vector2(0.0f, 0.0f), (float) Game1.pixelZoom, SpriteEffects.None, 0.89f);
      }
      else
      {
        int num1 = Game1.tileSize + 11 * Game1.pixelZoom;
        int num2 = Game1.tileSize / 2 - Game1.pixelZoom;
        b.Draw(this.texture, new Vector2((float) (x + num1), (float) (y + num2)), new Rectangle?(new Rectangle(this.texture.Bounds.Center.X - 64, this.texture.Bounds.Bottom - 136 - 2, 122, 138)), this.color, 0.0f, new Vector2(0.0f, 0.0f), (float) Game1.pixelZoom, SpriteEffects.None, 0.89f);
      }
    }

    public virtual void draw(SpriteBatch b)
    {
      if (this.daysOfConstructionLeft > 0)
      {
        this.drawInConstruction(b);
      }
      else
      {
        this.drawShadow(b, -1, -1);
        b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (this.tileX * Game1.tileSize), (float) (this.tileY * Game1.tileSize + this.tilesHigh * Game1.tileSize))), new Rectangle?(this.texture.Bounds), this.color * this.alpha, 0.0f, new Vector2(0.0f, (float) this.texture.Bounds.Height), 4f, SpriteEffects.None, (float) ((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000f);
        if (!this.magical || !this.buildingType.Equals("Gold Clock"))
          return;
        b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (this.tileX * Game1.tileSize + 23 * Game1.pixelZoom), (float) (this.tileY * Game1.tileSize - 10 * Game1.pixelZoom))), new Rectangle?(Town.hourHandSource), Color.White * this.alpha, (float) (2.0 * Math.PI * ((double) (Game1.timeOfDay % 1200) / 1200.0) + (double) Game1.gameTimeInterval / 7000.0 / 23.0), new Vector2(2.5f, 8f), (float) (Game1.pixelZoom * 3) / 4f, SpriteEffects.None, (float) ((double) ((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000.0 + 9.99999974737875E-05));
        b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (this.tileX * Game1.tileSize + 23 * Game1.pixelZoom), (float) (this.tileY * Game1.tileSize - 10 * Game1.pixelZoom))), new Rectangle?(Town.minuteHandSource), Color.White * this.alpha, (float) (2.0 * Math.PI * ((double) (Game1.timeOfDay % 1000 % 100 % 60) / 60.0) + (double) Game1.gameTimeInterval / 7000.0 * 1.01999998092651), new Vector2(2.5f, 12f), (float) (Game1.pixelZoom * 3) / 4f, SpriteEffects.None, (float) ((double) ((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000.0 + 0.000110000000859145));
        b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (this.tileX * Game1.tileSize + 23 * Game1.pixelZoom), (float) (this.tileY * Game1.tileSize - 10 * Game1.pixelZoom))), new Rectangle?(Town.clockNub), Color.White * this.alpha, 0.0f, new Vector2(2f, 2f), (float) Game1.pixelZoom, SpriteEffects.None, (float) ((double) ((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000.0 + 0.000119999996968545));
      }
    }

    public virtual void drawShadow(SpriteBatch b, int localX = -1, int localY = -1)
    {
      Vector2 position = localX == -1 ? Game1.GlobalToLocal(new Vector2((float) (this.tileX * Game1.tileSize), (float) ((this.tileY + this.tilesHigh) * Game1.tileSize))) : new Vector2((float) localX, (float) (localY + this.getSourceRectForMenu().Height * Game1.pixelZoom));
      b.Draw(Game1.mouseCursors, position, new Rectangle?(Building.leftShadow), Color.White * (localX == -1 ? this.alpha : 1f), 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1E-05f);
      for (int index = 1; index < this.tilesWide - 1; ++index)
        b.Draw(Game1.mouseCursors, position + new Vector2((float) (index * Game1.tileSize), 0.0f), new Rectangle?(Building.middleShadow), Color.White * (localX == -1 ? this.alpha : 1f), 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1E-05f);
      b.Draw(Game1.mouseCursors, position + new Vector2((float) ((this.tilesWide - 1) * Game1.tileSize), 0.0f), new Rectangle?(Building.rightShadow), Color.White * (localX == -1 ? this.alpha : 1f), 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1E-05f);
    }

    public void drawInConstruction(SpriteBatch b)
    {
      int height = Math.Min(16, Math.Max(0, (int) (16.0 - (double) this.newConstructionTimer / 1000.0 * 16.0)));
      float num = (float) (2000 - this.newConstructionTimer) / 2000f;
      if (this.magical)
      {
        b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (this.tileX * Game1.tileSize), (float) (this.tileY * Game1.tileSize + this.tilesHigh * Game1.tileSize) + (float) (this.texture.Bounds.Height * Game1.pixelZoom) * (1f - num))), new Rectangle?(new Rectangle(0, (int) ((double) this.texture.Bounds.Bottom - (double) num * (double) this.texture.Bounds.Height), this.getSourceRectForMenu().Width, (int) ((double) this.texture.Bounds.Height * (double) num))), this.color * this.alpha, 0.0f, new Vector2(0.0f, (float) this.texture.Bounds.Height), 4f, SpriteEffects.None, (float) ((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000f);
        for (int index = 0; index < this.tilesWide * 4; ++index)
        {
          b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (this.tileX * Game1.tileSize + index * (Game1.tileSize / 4)), (float) (this.tileY * Game1.tileSize - this.texture.Bounds.Height * Game1.pixelZoom + this.tilesHigh * Game1.tileSize) + (float) (this.texture.Bounds.Height * Game1.pixelZoom) * (1f - num))) + new Vector2((float) Game1.random.Next(-1, 2), (float) (Game1.random.Next(-1, 2) - (index % 2 == 0 ? Game1.pixelZoom * 8 : Game1.pixelZoom * 2))), new Rectangle?(new Rectangle(536 + (this.newConstructionTimer + index * 4) % 56 / 8 * 8, 1945, 8, 8)), index % 2 == 1 ? Color.Pink * this.alpha : Color.LightPink * this.alpha, 0.0f, new Vector2(0.0f, 0.0f), (float) (4.0 + (double) Game1.random.Next(100) / 100.0), SpriteEffects.None, (float) ((double) ((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000.0 + 9.99999974737875E-05));
          if (index % 2 == 0)
            b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (this.tileX * Game1.tileSize + index * (Game1.tileSize / 4)), (float) (this.tileY * Game1.tileSize - this.texture.Bounds.Height * Game1.pixelZoom + this.tilesHigh * Game1.tileSize) + (float) (this.texture.Bounds.Height * Game1.pixelZoom) * (1f - num))) + new Vector2((float) Game1.random.Next(-1, 2), (float) (Game1.random.Next(-1, 2) + (index % 2 == 0 ? Game1.pixelZoom * 8 : Game1.pixelZoom * 2))), new Rectangle?(new Rectangle(536 + (this.newConstructionTimer + index * 4) % 56 / 8 * 8, 1945, 8, 8)), Color.White * this.alpha, 0.0f, new Vector2(0.0f, 0.0f), (float) (4.0 + (double) Game1.random.Next(100) / 100.0), SpriteEffects.None, (float) ((double) ((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000.0 + 9.99999974737875E-05));
        }
      }
      else
      {
        bool flag = this.daysOfConstructionLeft == 1;
        for (int tileX = this.tileX; tileX < this.tileX + this.tilesWide; ++tileX)
        {
          for (int tileY = this.tileY; tileY < this.tileY + this.tilesHigh; ++tileY)
          {
            if (tileX == this.tileX + this.tilesWide / 2 && tileY == this.tileY + this.tilesHigh - 1)
            {
              if (flag)
                b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) tileX, (float) tileY) * (float) Game1.tileSize) + new Vector2(0.0f, (float) (Game1.tileSize - height * Game1.pixelZoom + Game1.tileSize / 4 - Game1.pixelZoom)), new Rectangle?(new Rectangle(367, 277, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1E-05f);
              b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) tileX, (float) tileY) * (float) Game1.tileSize) + new Vector2(0.0f, (float) (Game1.tileSize - height * Game1.pixelZoom)) + (this.newConstructionTimer > 0 ? new Vector2((float) Game1.random.Next(-1, 2), (float) Game1.random.Next(-1, 2)) : Vector2.Zero), new Rectangle?(new Rectangle(367, 309, 16, height)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, (float) (tileY * Game1.tileSize + Game1.tileSize - 1) / 10000f);
            }
            else if (tileX == this.tileX && tileY == this.tileY)
            {
              if (flag)
                b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) tileX, (float) tileY) * (float) Game1.tileSize) + new Vector2(0.0f, (float) (Game1.tileSize - height * Game1.pixelZoom + Game1.tileSize / 4)), new Rectangle?(new Rectangle(351, 261, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1E-05f);
              b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) tileX, (float) tileY) * (float) Game1.tileSize) + new Vector2(0.0f, (float) (Game1.tileSize - height * Game1.pixelZoom)) + (this.newConstructionTimer > 0 ? new Vector2((float) Game1.random.Next(-1, 2), (float) Game1.random.Next(-1, 2)) : Vector2.Zero), new Rectangle?(new Rectangle(351, 293, 16, height)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, (float) (tileY * Game1.tileSize + Game1.tileSize - 1) / 10000f);
            }
            else if (tileX == this.tileX + this.tilesWide - 1 && tileY == this.tileY)
            {
              if (flag)
                b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) tileX, (float) tileY) * (float) Game1.tileSize) + new Vector2(0.0f, (float) (Game1.tileSize - height * Game1.pixelZoom + Game1.tileSize / 4)), new Rectangle?(new Rectangle(383, 261, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1E-05f);
              b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) tileX, (float) tileY) * (float) Game1.tileSize) + new Vector2(0.0f, (float) (Game1.tileSize - height * Game1.pixelZoom)) + (this.newConstructionTimer > 0 ? new Vector2((float) Game1.random.Next(-1, 2), (float) Game1.random.Next(-1, 2)) : Vector2.Zero), new Rectangle?(new Rectangle(383, 293, 16, height)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, (float) (tileY * Game1.tileSize + Game1.tileSize - 1) / 10000f);
            }
            else if (tileX == this.tileX + this.tilesWide - 1 && tileY == this.tileY + this.tilesHigh - 1)
            {
              if (flag)
                b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) tileX, (float) tileY) * (float) Game1.tileSize) + new Vector2(0.0f, (float) (Game1.tileSize - height * Game1.pixelZoom + Game1.tileSize / 4)), new Rectangle?(new Rectangle(383, 277, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1E-05f);
              b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) tileX, (float) tileY) * (float) Game1.tileSize) + new Vector2(0.0f, (float) (Game1.tileSize - height * Game1.pixelZoom)) + (this.newConstructionTimer > 0 ? new Vector2((float) Game1.random.Next(-1, 2), (float) Game1.random.Next(-1, 2)) : Vector2.Zero), new Rectangle?(new Rectangle(383, 325, 16, height)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, (float) (tileY * Game1.tileSize) / 10000f);
            }
            else if (tileX == this.tileX && tileY == this.tileY + this.tilesHigh - 1)
            {
              if (flag)
                b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) tileX, (float) tileY) * (float) Game1.tileSize) + new Vector2(0.0f, (float) (Game1.tileSize - height * Game1.pixelZoom + Game1.tileSize / 4)), new Rectangle?(new Rectangle(351, 277, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1E-05f);
              b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) tileX, (float) tileY) * (float) Game1.tileSize) + new Vector2(0.0f, (float) (Game1.tileSize - height * Game1.pixelZoom)) + (this.newConstructionTimer > 0 ? new Vector2((float) Game1.random.Next(-1, 2), (float) Game1.random.Next(-1, 2)) : Vector2.Zero), new Rectangle?(new Rectangle(351, 325, 16, height)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, (float) (tileY * Game1.tileSize) / 10000f);
            }
            else if (tileX == this.tileX + this.tilesWide - 1)
            {
              if (flag)
                b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) tileX, (float) tileY) * (float) Game1.tileSize) + new Vector2(0.0f, (float) (Game1.tileSize - height * Game1.pixelZoom + Game1.tileSize / 4)), new Rectangle?(new Rectangle(383, 261, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1E-05f);
              b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) tileX, (float) tileY) * (float) Game1.tileSize) + new Vector2(0.0f, (float) (Game1.tileSize - height * Game1.pixelZoom)) + (this.newConstructionTimer > 0 ? new Vector2((float) Game1.random.Next(-1, 2), (float) Game1.random.Next(-1, 2)) : Vector2.Zero), new Rectangle?(new Rectangle(383, 309, 16, height)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, (float) (tileY * Game1.tileSize) / 10000f);
            }
            else if (tileY == this.tileY + this.tilesHigh - 1)
            {
              if (flag)
                b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) tileX, (float) tileY) * (float) Game1.tileSize) + new Vector2(0.0f, (float) (Game1.tileSize - height * Game1.pixelZoom + Game1.tileSize / 4)), new Rectangle?(new Rectangle(367, 277, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1E-05f);
              b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) tileX, (float) tileY) * (float) Game1.tileSize) + new Vector2(0.0f, (float) (Game1.tileSize - height * Game1.pixelZoom)) + (this.newConstructionTimer > 0 ? new Vector2((float) Game1.random.Next(-1, 2), (float) Game1.random.Next(-1, 2)) : Vector2.Zero), new Rectangle?(new Rectangle(367, 325, 16, height)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, (float) (tileY * Game1.tileSize) / 10000f);
            }
            else if (tileX == this.tileX)
            {
              if (flag)
                b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) tileX, (float) tileY) * (float) Game1.tileSize) + new Vector2(0.0f, (float) (Game1.tileSize - height * Game1.pixelZoom + Game1.tileSize / 4)), new Rectangle?(new Rectangle(351, 261, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1E-05f);
              b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) tileX, (float) tileY) * (float) Game1.tileSize) + new Vector2(0.0f, (float) (Game1.tileSize - height * Game1.pixelZoom)) + (this.newConstructionTimer > 0 ? new Vector2((float) Game1.random.Next(-1, 2), (float) Game1.random.Next(-1, 2)) : Vector2.Zero), new Rectangle?(new Rectangle(351, 309, 16, height)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, (float) (tileY * Game1.tileSize) / 10000f);
            }
            else if (tileY == this.tileY)
            {
              if (flag)
                b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) tileX, (float) tileY) * (float) Game1.tileSize) + new Vector2(0.0f, (float) (Game1.tileSize - height * Game1.pixelZoom + Game1.tileSize / 4)), new Rectangle?(new Rectangle(367, 261, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1E-05f);
              b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) tileX, (float) tileY) * (float) Game1.tileSize) + new Vector2(0.0f, (float) (Game1.tileSize - height * Game1.pixelZoom)) + (this.newConstructionTimer > 0 ? new Vector2((float) Game1.random.Next(-1, 2), (float) Game1.random.Next(-1, 2)) : Vector2.Zero), new Rectangle?(new Rectangle(367, 293, 16, height)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, (float) (tileY * Game1.tileSize + Game1.tileSize - 1) / 10000f);
            }
            else if (flag)
              b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) tileX, (float) tileY) * (float) Game1.tileSize) + new Vector2(0.0f, (float) (Game1.tileSize - height * Game1.pixelZoom + Game1.tileSize / 4)), new Rectangle?(new Rectangle(367, 261, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1E-05f);
          }
        }
      }
    }
  }
}
