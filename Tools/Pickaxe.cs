// Decompiled with JetBrains decompiler
// Type: StardewValley.Tools.Pickaxe
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace StardewValley.Tools
{
  public class Pickaxe : Tool
  {
    public const int hitMargin = 8;
    public const int BoulderStrength = 4;
    private int boulderTileX;
    private int boulderTileY;
    private int hitsToBoulder;

    public Pickaxe()
      : base(nameof (Pickaxe), 0, 105, 131, false, 0)
    {
      this.upgradeLevel = 0;
    }

    protected override string loadDisplayName()
    {
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:Pickaxe.cs.14184");
    }

    protected override string loadDescription()
    {
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:Pickaxe.cs.14185");
    }

    public override bool beginUsing(GameLocation location, int x, int y, Farmer who)
    {
      this.Update(who.facingDirection, 0, who);
      if (who.IsMainPlayer)
      {
        Game1.releaseUseToolButton();
        return true;
      }
      switch (who.FacingDirection)
      {
        case 0:
          who.FarmerSprite.setCurrentFrame(176);
          who.CurrentTool.Update(0, 0);
          break;
        case 1:
          who.FarmerSprite.setCurrentFrame(168);
          who.CurrentTool.Update(1, 0);
          break;
        case 2:
          who.FarmerSprite.setCurrentFrame(160);
          who.CurrentTool.Update(2, 0);
          break;
        case 3:
          who.FarmerSprite.setCurrentFrame(184);
          who.CurrentTool.Update(3, 0);
          break;
      }
      return true;
    }

    public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
    {
      base.DoFunction(location, x, y, power, who);
      power = who.toolPower;
      who.Stamina = who.Stamina - ((float) (2 * (power + 1)) - (float) who.MiningLevel * 0.1f);
      Utility.clampToTile(new Vector2((float) x, (float) y));
      int num1 = x / Game1.tileSize;
      int num2 = y / Game1.tileSize;
      Vector2 index = new Vector2((float) num1, (float) num2);
      if (location.performToolAction((Tool) this, num1, num2))
        return;
      StardewValley.Object @object = (StardewValley.Object) null;
      location.Objects.TryGetValue(index, out @object);
      if (@object == null)
      {
        if (who.FacingDirection == 0 || who.FacingDirection == 2)
        {
          num1 = (x - 8) / Game1.tileSize;
          location.Objects.TryGetValue(new Vector2((float) num1, (float) num2), out @object);
          if (@object == null)
          {
            num1 = (x + 8) / Game1.tileSize;
            location.Objects.TryGetValue(new Vector2((float) num1, (float) num2), out @object);
          }
        }
        else
        {
          num2 = (y + 8) / Game1.tileSize;
          location.Objects.TryGetValue(new Vector2((float) num1, (float) num2), out @object);
          if (@object == null)
          {
            num2 = (y - 8) / Game1.tileSize;
            location.Objects.TryGetValue(new Vector2((float) num1, (float) num2), out @object);
          }
        }
        x = num1 * Game1.tileSize;
        y = num2 * Game1.tileSize;
        if (location.terrainFeatures.ContainsKey(index) && location.terrainFeatures[index].performToolAction((Tool) this, 0, index, (GameLocation) null))
          location.terrainFeatures.Remove(index);
      }
      index = new Vector2((float) num1, (float) num2);
      if (@object != null)
      {
        if (@object.Name.Equals("Stone"))
        {
          Game1.playSound("hammer");
          if (@object.minutesUntilReady > 0)
          {
            int num3 = Math.Max(1, this.upgradeLevel + 1);
            @object.minutesUntilReady -= num3;
            @object.shakeTimer = 200;
            if (@object.minutesUntilReady > 0)
            {
              Game1.createRadialDebris(Game1.currentLocation, 14, num1, num2, Game1.random.Next(2, 5), false, -1, false, -1);
              return;
            }
          }
          if (@object.ParentSheetIndex < 200 && !Game1.objectInformation.ContainsKey(@object.ParentSheetIndex + 1))
            location.TemporarySprites.Add(new TemporaryAnimatedSprite(@object.ParentSheetIndex + 1, 300f, 1, 2, new Vector2((float) (x - x % Game1.tileSize), (float) (y - y % Game1.tileSize)), true, @object.flipped)
            {
              alphaFade = 0.01f
            });
          else
            location.TemporarySprites.Add(new TemporaryAnimatedSprite(47, new Vector2((float) (num1 * Game1.tileSize), (float) (num2 * Game1.tileSize)), Color.Gray, 10, false, 80f, 0, -1, -1f, -1, 0));
          Game1.createRadialDebris(location, 14, num1, num2, Game1.random.Next(2, 5), false, -1, false, -1);
          location.TemporarySprites.Add(new TemporaryAnimatedSprite(46, new Vector2((float) (num1 * Game1.tileSize), (float) (num2 * Game1.tileSize)), Color.White, 10, false, 80f, 0, -1, -1f, -1, 0)
          {
            motion = new Vector2(0.0f, -0.6f),
            acceleration = new Vector2(0.0f, 1f / 500f),
            alphaFade = 0.015f
          });
          if (!location.Name.Equals("UndergroundMine"))
          {
            if (@object.parentSheetIndex == 343 || @object.parentSheetIndex == 450)
            {
              Random random = new Random((int) Game1.stats.DaysPlayed + (int) Game1.uniqueIDForThisGame / 2 + num1 * 2000 + num2);
              if (random.NextDouble() < 0.035 && Game1.stats.DaysPlayed > 1U)
                Game1.createObjectDebris(535 + (Game1.stats.DaysPlayed <= 60U || random.NextDouble() >= 0.2 ? (Game1.stats.DaysPlayed <= 120U || random.NextDouble() >= 0.2 ? 0 : 2) : 1), num1, num2, this.getLastFarmerToUse().uniqueMultiplayerID);
              if (random.NextDouble() < 0.035 * (who.professions.Contains(21) ? 2.0 : 1.0) && Game1.stats.DaysPlayed > 1U)
                Game1.createObjectDebris(382, num1, num2, this.getLastFarmerToUse().uniqueMultiplayerID);
              if (random.NextDouble() < 0.01 && Game1.stats.DaysPlayed > 1U)
                Game1.createObjectDebris(390, num1, num2, this.getLastFarmerToUse().uniqueMultiplayerID);
            }
            location.breakStone(@object.parentSheetIndex, num1, num2, who, new Random((int) Game1.stats.DaysPlayed + (int) Game1.uniqueIDForThisGame / 2 + num1 * 4000 + num2));
          }
          else
            Game1.mine.checkStoneForItems(@object.ParentSheetIndex, num1, num2, who);
          if (@object.minutesUntilReady > 0)
            return;
          location.Objects.Remove(new Vector2((float) num1, (float) num2));
          Game1.playSound("stoneCrack");
          ++Game1.stats.RocksCrushed;
        }
        else if (@object.Name.Contains("Boulder"))
        {
          Game1.playSound("hammer");
          if (this.UpgradeLevel < 2)
          {
            Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:Pickaxe.cs.14194")));
          }
          else
          {
            if (num1 == this.boulderTileX && num2 == this.boulderTileY)
            {
              this.hitsToBoulder = this.hitsToBoulder + (power + 1);
              @object.shakeTimer = 190;
            }
            else
            {
              this.hitsToBoulder = 0;
              this.boulderTileX = num1;
              this.boulderTileY = num2;
            }
            if (this.hitsToBoulder < 4)
              return;
            location.removeObject(index, false);
            location.temporarySprites.Add(new TemporaryAnimatedSprite(5, new Vector2((float) Game1.tileSize * index.X - (float) (Game1.tileSize / 2), (float) Game1.tileSize * (index.Y - 1f)), Color.Gray, 8, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, -1, 0)
            {
              delayBeforeAnimationStart = 0
            });
            location.temporarySprites.Add(new TemporaryAnimatedSprite(5, new Vector2((float) Game1.tileSize * index.X + (float) (Game1.tileSize / 2), (float) Game1.tileSize * (index.Y - 1f)), Color.Gray, 8, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, -1, 0)
            {
              delayBeforeAnimationStart = 200
            });
            location.temporarySprites.Add(new TemporaryAnimatedSprite(5, new Vector2((float) Game1.tileSize * index.X, (float) Game1.tileSize * (index.Y - 1f) - (float) (Game1.tileSize / 2)), Color.Gray, 8, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, -1, 0)
            {
              delayBeforeAnimationStart = 400
            });
            location.temporarySprites.Add(new TemporaryAnimatedSprite(5, new Vector2((float) Game1.tileSize * index.X, (float) Game1.tileSize * index.Y - (float) (Game1.tileSize / 2)), Color.Gray, 8, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, -1, 0)
            {
              delayBeforeAnimationStart = 600
            });
            location.temporarySprites.Add(new TemporaryAnimatedSprite(25, new Vector2((float) Game1.tileSize * index.X, (float) Game1.tileSize * index.Y), Color.White, 8, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, Game1.tileSize * 2, 0));
            location.temporarySprites.Add(new TemporaryAnimatedSprite(25, new Vector2((float) Game1.tileSize * index.X + (float) (Game1.tileSize / 2), (float) Game1.tileSize * index.Y), Color.White, 8, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, Game1.tileSize * 2, 0)
            {
              delayBeforeAnimationStart = 250
            });
            location.temporarySprites.Add(new TemporaryAnimatedSprite(25, new Vector2((float) Game1.tileSize * index.X - (float) (Game1.tileSize / 2), (float) Game1.tileSize * index.Y), Color.White, 8, Game1.random.NextDouble() < 0.5, 50f, 0, -1, -1f, Game1.tileSize * 2, 0)
            {
              delayBeforeAnimationStart = 500
            });
            Game1.playSound("boulderBreak");
            ++Game1.stats.BouldersCracked;
          }
        }
        else
        {
          if (!@object.performToolAction((Tool) this))
            return;
          @object.performRemoveAction(index, location);
          if (@object.type.Equals("Crafting") && @object.fragility != 2)
          {
            List<Debris> debris1 = Game1.currentLocation.debris;
            int objectIndex = @object.bigCraftable ? -@object.ParentSheetIndex : @object.ParentSheetIndex;
            Vector2 toolLocation = who.GetToolLocation(false);
            Rectangle boundingBox = who.GetBoundingBox();
            double x1 = (double) boundingBox.Center.X;
            boundingBox = who.GetBoundingBox();
            double y1 = (double) boundingBox.Center.Y;
            Vector2 playerPosition = new Vector2((float) x1, (float) y1);
            Debris debris2 = new Debris(objectIndex, toolLocation, playerPosition);
            debris1.Add(debris2);
          }
          Game1.currentLocation.Objects.Remove(index);
        }
      }
      else
      {
        Game1.playSound("woodyHit");
        if (location.doesTileHaveProperty(num1, num2, "Diggable", "Back") == null)
          return;
        location.TemporarySprites.Add(new TemporaryAnimatedSprite(12, new Vector2((float) (num1 * Game1.tileSize), (float) (num2 * Game1.tileSize)), Color.White, 8, false, 80f, 0, -1, -1f, -1, 0)
        {
          alphaFade = 0.015f
        });
      }
    }
  }
}
