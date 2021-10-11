// Decompiled with JetBrains decompiler
// Type: StardewValley.Tools.Shears
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley.Tools
{
  public class Shears : Tool
  {
    private FarmAnimal animal;

    public Shears()
      : base(nameof (Shears), -1, 7, 7, false, 0)
    {
    }

    protected override string loadDisplayName()
    {
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:Shears.cs.14240");
    }

    protected override string loadDescription()
    {
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:Shears.cs.14241");
    }

    public override bool beginUsing(GameLocation location, int x, int y, Farmer who)
    {
      x = (int) who.GetToolLocation(false).X;
      y = (int) who.GetToolLocation(false).Y;
      Rectangle rectangle = new Rectangle(x - Game1.tileSize / 2, y - Game1.tileSize / 2, Game1.tileSize, Game1.tileSize);
      if (location is Farm)
      {
        foreach (FarmAnimal farmAnimal in (location as Farm).animals.Values)
        {
          if (farmAnimal.GetBoundingBox().Intersects(rectangle))
          {
            this.animal = farmAnimal;
            break;
          }
        }
      }
      else if (location is AnimalHouse)
      {
        foreach (FarmAnimal farmAnimal in (location as AnimalHouse).animals.Values)
        {
          if (farmAnimal.GetBoundingBox().Intersects(rectangle))
          {
            this.animal = farmAnimal;
            break;
          }
        }
      }
      who.Halt();
      int currentFrame = who.FarmerSprite.currentFrame;
      who.FarmerSprite.animateOnce(283 + who.FacingDirection, 50f, 4);
      who.FarmerSprite.oldFrame = currentFrame;
      who.UsingTool = true;
      who.CanMove = false;
      return true;
    }

    public static void playSnip(Farmer who)
    {
      Game1.playSound("scissors");
    }

    public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
    {
      base.DoFunction(location, x, y, power, who);
      who.Stamina -= 4f;
      Shears.playSnip(who);
      this.currentParentTileIndex = 7;
      this.indexOfMenuItemView = 7;
      if (this.animal != null && this.animal.currentProduce > 0 && (this.animal.age >= (int) this.animal.ageWhenMature && this.animal.toolUsedForHarvest.Equals(this.name)))
      {
        Farmer farmer = who;
        StardewValley.Object @object = new StardewValley.Object(Vector2.Zero, this.animal.currentProduce, (string) null, false, true, false, false);
        @object.quality = this.animal.produceQuality;
        int num = 0;
        if (farmer.addItemToInventoryBool((Item) @object, num != 0))
        {
          this.animal.currentProduce = -1;
          Game1.playSound("coin");
          this.animal.friendshipTowardFarmer = Math.Min(1000, this.animal.friendshipTowardFarmer + 5);
          if (this.animal.showDifferentTextureWhenReadyForHarvest)
            this.animal.sprite.Texture = Game1.content.Load<Texture2D>("Animals\\Sheared" + this.animal.type);
          who.gainExperience(0, 5);
        }
      }
      else
      {
        string dialogue = "";
        if (this.animal != null && !this.animal.toolUsedForHarvest.Equals(this.name))
          dialogue = Game1.content.LoadString("Strings\\StringsFromCSFiles:Shears.cs.14245", (object) this.animal.displayName);
        if (this.animal != null && this.animal.isBaby() && this.animal.toolUsedForHarvest.Equals(this.name))
          dialogue = Game1.content.LoadString("Strings\\StringsFromCSFiles:Shears.cs.14246", (object) this.animal.displayName);
        if (this.animal != null && this.animal.age >= (int) this.animal.ageWhenMature && this.animal.toolUsedForHarvest.Equals(this.name))
          dialogue = Game1.content.LoadString("Strings\\StringsFromCSFiles:Shears.cs.14247", (object) this.animal.displayName);
        if (dialogue.Length > 0)
          Game1.drawObjectDialogue(dialogue);
      }
      this.animal = (FarmAnimal) null;
      if (Game1.activeClickableMenu == null)
        who.CanMove = true;
      else
        who.Halt();
      who.usingTool = false;
      who.canReleaseTool = true;
    }
  }
}
