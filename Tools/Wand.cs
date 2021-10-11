// Decompiled with JetBrains decompiler
// Type: StardewValley.Tools.Wand
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace StardewValley.Tools
{
  public class Wand : Tool
  {
    public bool charged;

    public Wand()
      : base("Return Scepter", 0, 2, 2, false, 0)
    {
      this.upgradeLevel = 0;
      this.CurrentParentTileIndex = this.indexOfMenuItemView;
      this.instantUse = true;
    }

    protected override string loadDisplayName()
    {
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:Wand.cs.14318");
    }

    protected override string loadDescription()
    {
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:Wand.cs.14319");
    }

    public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
    {
      if (who.bathingClothes)
        return;
      this.indexOfMenuItemView = 2;
      this.CurrentParentTileIndex = 2;
      if (who.IsMainPlayer)
      {
        for (int index = 0; index < 12; ++index)
          who.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(354, (float) Game1.random.Next(25, 75), 6, 1, new Vector2((float) Game1.random.Next((int) who.position.X - Game1.tileSize * 4, (int) who.position.X + Game1.tileSize * 3), (float) Game1.random.Next((int) who.position.Y - Game1.tileSize * 4, (int) who.position.Y + Game1.tileSize * 3)), false, Game1.random.NextDouble() < 0.5));
        Game1.playSound("wand");
        Game1.displayFarmer = false;
        Game1.player.Halt();
        Game1.player.faceDirection(2);
        Game1.player.freezePause = 1000;
        Game1.flashAlpha = 1f;
        DelayedAction.fadeAfterDelay(new Game1.afterFadeFunction(this.wandWarpForReal), 1000);
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
      this.CurrentParentTileIndex = this.indexOfMenuItemView;
    }

    public override bool actionWhenPurchased()
    {
      Game1.player.mailReceived.Add("ReturnScepter");
      return base.actionWhenPurchased();
    }

    private void wandWarpForReal()
    {
      Game1.warpFarmer("Farm", 64, 15, false);
      if (!Game1.isStartingToGetDarkOut())
        Game1.playMorningSong();
      else
        Game1.changeMusicTrack("none");
      Game1.fadeToBlackAlpha = 0.99f;
      Game1.screenGlow = false;
      Game1.player.temporarilyInvincible = false;
      Game1.player.temporaryInvincibilityTimer = 0;
      Game1.displayFarmer = true;
    }
  }
}
