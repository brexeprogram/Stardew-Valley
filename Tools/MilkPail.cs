// Decompiled with JetBrains decompiler
// Type: StardewValley.Tools.MilkPail
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley.Tools
{
  public class MilkPail : Tool
  {
    private FarmAnimal animal;

    public MilkPail()
      : base("Milk Pail", -1, 6, 6, false, 0)
    {
    }

    protected override string loadDisplayName()
    {
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:MilkPail.cs.14167");
    }

    protected override string loadDescription()
    {
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:MilkPail.cs.14168");
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
      if (this.animal != null && this.animal.currentProduce > 0 && (this.animal.age >= (int) this.animal.ageWhenMature && this.animal.toolUsedForHarvest.Equals(this.name)) && who.couldInventoryAcceptThisObject(this.animal.currentProduce, 1, 0))
      {
        this.animal.doEmote(20, true);
        this.animal.friendshipTowardFarmer = Math.Min(1000, this.animal.friendshipTowardFarmer + 5);
        Game1.playSound("Milking");
        this.animal.pauseTimer = 1500;
      }
      else if (this.animal != null && this.animal.currentProduce > 0 && this.animal.age >= (int) this.animal.ageWhenMature)
      {
        if (!this.animal.toolUsedForHarvest.Equals(this.name))
        {
          if (this.animal.toolUsedForHarvest != null && !this.animal.toolUsedForHarvest.Equals("null"))
            Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:MilkPail.cs.14167", (object) this.animal.toolUsedForHarvest));
        }
        else if (!who.couldInventoryAcceptThisObject(this.animal.currentProduce, 1, 0))
          Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Crop.cs.588"));
      }
      else
      {
        DelayedAction.playSoundAfterDelay("fishingRodBend", 300);
        DelayedAction.playSoundAfterDelay("fishingRodBend", 1200);
        string dialogue = "";
        if (this.animal != null && !this.animal.toolUsedForHarvest.Equals(this.name))
          dialogue = Game1.content.LoadString("Strings\\StringsFromCSFiles:MilkPail.cs.14175", (object) this.animal.displayName);
        if (this.animal != null && this.animal.isBaby() && this.animal.toolUsedForHarvest.Equals(this.name))
          dialogue = Game1.content.LoadString("Strings\\StringsFromCSFiles:MilkPail.cs.14176", (object) this.animal.displayName);
        if (this.animal != null && this.animal.age >= (int) this.animal.ageWhenMature && this.animal.toolUsedForHarvest.Equals(this.name))
          dialogue = Game1.content.LoadString("Strings\\StringsFromCSFiles:MilkPail.cs.14177", (object) this.animal.displayName);
        if (dialogue.Length > 0)
          DelayedAction.showDialogueAfterDelay(dialogue, 1000);
      }
      who.Halt();
      int currentFrame = who.FarmerSprite.currentFrame;
      who.FarmerSprite.animateOnce(287 + who.FacingDirection, 50f, 4);
      who.FarmerSprite.oldFrame = currentFrame;
      who.UsingTool = true;
      who.CanMove = false;
      return true;
    }

    public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
    {
      base.DoFunction(location, x, y, power, who);
      who.Stamina -= 4f;
      this.currentParentTileIndex = 6;
      this.indexOfMenuItemView = 6;
      if (this.animal != null && this.animal.currentProduce > 0 && (this.animal.age >= (int) this.animal.ageWhenMature && this.animal.toolUsedForHarvest.Equals(this.name)))
      {
        Farmer farmer = who;
        StardewValley.Object @object = new StardewValley.Object(Vector2.Zero, this.animal.currentProduce, (string) null, false, true, false, false);
        @object.quality = this.animal.produceQuality;
        int num = 0;
        if (farmer.addItemToInventoryBool((Item) @object, num != 0))
        {
          Game1.playSound("coin");
          this.animal.currentProduce = -1;
          if (this.animal.showDifferentTextureWhenReadyForHarvest)
            this.animal.sprite.Texture = Game1.content.Load<Texture2D>("Animals\\Sheared" + this.animal.type);
          who.gainExperience(0, 5);
        }
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
