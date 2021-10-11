// Decompiled with JetBrains decompiler
// Type: StardewValley.Buildings.Barn
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using xTile;

namespace StardewValley.Buildings
{
  public class Barn : Building
  {
    public static int openAnimalDoorPosition = -Game1.tileSize - 24 + Game1.pixelZoom * 3;
    private const int closedAnimalDoorPosition = 0;
    private int yPositionOfAnimalDoor;
    private int animalDoorMotion;

    public Barn(BluePrint b, Vector2 tileLocation)
      : base(b, tileLocation)
    {
    }

    public Barn()
    {
    }

    protected override GameLocation getIndoors()
    {
      if (this.indoors != null)
        this.nameOfIndoorsWithoutUnique = this.indoors.name;
      string indoorsWithoutUnique1 = this.nameOfIndoorsWithoutUnique;
      if (!(indoorsWithoutUnique1 == "Big Barn"))
      {
        if (indoorsWithoutUnique1 == "Deluxe Barn")
          this.nameOfIndoorsWithoutUnique = "Barn3";
      }
      else
        this.nameOfIndoorsWithoutUnique = "Barn2";
      GameLocation gameLocation = (GameLocation) new AnimalHouse(Game1.game1.xTileContent.Load<Map>("Maps\\" + this.nameOfIndoorsWithoutUnique), this.buildingType);
      gameLocation.IsFarm = true;
      gameLocation.isStructure = true;
      string indoorsWithoutUnique2 = this.nameOfIndoorsWithoutUnique;
      if (!(indoorsWithoutUnique2 == "Big Barn"))
      {
        if (indoorsWithoutUnique2 == "Deluxe Barn")
          (gameLocation as AnimalHouse).animalLimit = 12;
      }
      else
        (gameLocation as AnimalHouse).animalLimit = 8;
      foreach (Warp warp in gameLocation.warps)
      {
        int num1 = this.humanDoor.X + this.tileX;
        warp.TargetX = num1;
        int num2 = this.humanDoor.Y + this.tileY + 1;
        warp.TargetY = num2;
      }
      if (this.animalDoorOpen)
        this.yPositionOfAnimalDoor = Barn.openAnimalDoorPosition;
      return gameLocation;
    }

    public override Rectangle getRectForAnimalDoor()
    {
      return new Rectangle((this.animalDoor.X + this.tileX) * Game1.tileSize, (this.tileY + this.animalDoor.Y) * Game1.tileSize, Game1.tileSize * 2, Game1.tileSize);
    }

    public override bool doAction(Vector2 tileLocation, Farmer who)
    {
      if (this.daysOfConstructionLeft > 0 || (double) tileLocation.X != (double) (this.tileX + this.animalDoor.X) && (double) tileLocation.X != (double) (this.tileX + this.animalDoor.X + 1) || (double) tileLocation.Y != (double) (this.tileY + this.animalDoor.Y))
        return base.doAction(tileLocation, who);
      if (!this.animalDoorOpen)
        Game1.playSound("doorCreak");
      else
        Game1.playSound("doorCreakReverse");
      this.animalDoorOpen = !this.animalDoorOpen;
      this.animalDoorMotion = this.animalDoorOpen ? -3 : 2;
      return true;
    }

    public override void updateWhenFarmNotCurrentLocation(GameTime time)
    {
      base.updateWhenFarmNotCurrentLocation(time);
      ((AnimalHouse) this.indoors).updateWhenNotCurrentLocation((Building) this, time);
    }

    public override Rectangle getSourceRectForMenu()
    {
      return new Rectangle(0, 0, this.texture.Bounds.Width, this.texture.Bounds.Height - 16);
    }

    public override void performActionOnUpgrade(GameLocation location)
    {
      (this.indoors as AnimalHouse).animalLimit += 4;
      if ((this.indoors as AnimalHouse).animalLimit != 8)
        return;
      this.indoors.objects.Add(new Vector2(1f, 3f), new StardewValley.Object(new Vector2(1f, 3f), 104, false)
      {
        fragility = 2
      });
    }

    public override void performActionOnConstruction(GameLocation location)
    {
      base.performActionOnConstruction(location);
      this.indoors.objects.Add(new Vector2(6f, 3f), new StardewValley.Object(new Vector2(6f, 3f), 99, false)
      {
        fragility = 2
      });
      this.daysOfConstructionLeft = 3;
    }

    public override void dayUpdate(int dayOfMonth)
    {
      base.dayUpdate(dayOfMonth);
      if (this.daysOfConstructionLeft > 0)
        return;
      this.currentOccupants = ((AnimalHouse) this.indoors).animals.Count;
      if ((this.indoors as AnimalHouse).animalLimit != 16)
        return;
      int num = Math.Min((this.indoors as AnimalHouse).animals.Count - this.indoors.numberOfObjectsWithName("Hay"), (Game1.getLocationFromName("Farm") as Farm).piecesOfHay);
      (Game1.getLocationFromName("Farm") as Farm).piecesOfHay -= num;
      for (int index = 0; index < 16 && num > 0; ++index)
      {
        Vector2 key = new Vector2((float) (8 + index), 3f);
        if (!this.indoors.objects.ContainsKey(key))
          this.indoors.objects.Add(key, new StardewValley.Object(178, 1, false, -1, 0));
        --num;
      }
    }

    public override void Update(GameTime time)
    {
      base.Update(time);
      if (this.animalDoorMotion == 0)
        return;
      if (this.animalDoorOpen && this.yPositionOfAnimalDoor <= Barn.openAnimalDoorPosition)
      {
        this.animalDoorMotion = 0;
        this.yPositionOfAnimalDoor = Barn.openAnimalDoorPosition;
      }
      else if (!this.animalDoorOpen && this.yPositionOfAnimalDoor >= 0)
      {
        this.animalDoorMotion = 0;
        this.yPositionOfAnimalDoor = 0;
      }
      this.yPositionOfAnimalDoor = this.yPositionOfAnimalDoor + this.animalDoorMotion;
    }

    public override void upgrade()
    {
      if (this.buildingType.Equals("Big Barn"))
      {
        ++this.animalDoor.X;
        this.indoors.moveObject(15, 3, 18, 13);
        this.indoors.moveObject(16, 3, 19, 13);
        this.indoors.moveObject(1, 4, 20, 3);
        for (int index = 4; index < 13; ++index)
          this.indoors.moveObject(16, index, 20, index);
      }
      else
      {
        this.indoors.moveObject(20, 3, 1, 4);
        for (int index = 6; index < 12; ++index)
          this.indoors.moveObject(20, index, 23, index);
        this.indoors.moveObject(20, 4, 20, 13);
        this.indoors.moveObject(20, 5, 21, 13);
        this.indoors.moveObject(20, 12, 22, 13);
      }
    }

    public override Vector2 getUpgradeSignLocation()
    {
      return new Vector2((float) this.tileX, (float) (this.tileY + 1)) * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize * 3), (float) Game1.pixelZoom);
    }

    public override void drawInMenu(SpriteBatch b, int x, int y)
    {
      this.drawShadow(b, x, y);
      b.Draw(this.texture, new Vector2((float) x, (float) y) + new Vector2((float) this.animalDoor.X, (float) (this.animalDoor.Y + 3)) * (float) Game1.tileSize, new Rectangle?(new Rectangle(64, 112, 32, 16)), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 0.888f);
      b.Draw(this.texture, new Vector2((float) x, (float) y) + new Vector2((float) this.animalDoor.X, (float) this.animalDoor.Y + 2.25f) * (float) Game1.tileSize, new Rectangle?(new Rectangle(0, 112, 32, 16)), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, (float) ((double) ((this.tileY + this.tilesHigh - 1) * Game1.tileSize) / 10000.0 - 1.0000000116861E-07));
      b.Draw(this.texture, new Vector2((float) x, (float) y), new Rectangle?(new Rectangle(0, 0, 112, 112)), this.color, 0.0f, new Vector2(0.0f, 0.0f), 4f, SpriteEffects.None, 0.89f);
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
        b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (this.tileX + this.animalDoor.X), (float) (this.tileY + this.animalDoor.Y - 1)) * (float) Game1.tileSize), new Rectangle?(new Rectangle(32, 112, 32, 16)), Color.White * this.alpha, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 1E-06f);
        b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (this.tileX + this.animalDoor.X), (float) (this.tileY + this.animalDoor.Y)) * (float) Game1.tileSize), new Rectangle?(new Rectangle(64, 112, 32, 16)), Color.White * this.alpha, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 1E-06f);
        b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) ((this.tileX + this.animalDoor.X) * Game1.tileSize), (float) ((this.tileY + this.animalDoor.Y) * Game1.tileSize + this.yPositionOfAnimalDoor - Game1.tileSize * 3 / 4))), new Rectangle?(new Rectangle(0, 112, 32, 12)), Color.White * this.alpha, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, (float) ((double) ((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000.0 - 9.99999974737875E-05));
        b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) ((this.tileX + this.animalDoor.X) * Game1.tileSize), (float) ((this.tileY + this.animalDoor.Y) * Game1.tileSize + this.yPositionOfAnimalDoor))), new Rectangle?(new Rectangle(0, 112, 32, 16)), Color.White * this.alpha, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, (float) ((double) ((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000.0 - 9.99999974737875E-05));
        b.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (this.tileX * Game1.tileSize), (float) (this.tileY * Game1.tileSize + this.tilesHigh * Game1.tileSize))), new Rectangle?(new Rectangle(0, 0, 112, 112)), this.color * this.alpha, 0.0f, new Vector2(0.0f, 112f), 4f, SpriteEffects.None, (float) ((double) ((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000.0 - 9.99999974737875E-06));
        if (this.daysUntilUpgrade <= 0)
          return;
        b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.getUpgradeSignLocation()), new Rectangle?(new Rectangle(367, 309, 16, 15)), Color.White * this.alpha, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, (float) ((double) ((this.tileY + this.tilesHigh) * Game1.tileSize) / 10000.0 + 9.99999974737875E-05));
      }
    }
  }
}
