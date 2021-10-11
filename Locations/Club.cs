// Decompiled with JetBrains decompiler
// Type: StardewValley.Locations.Club
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using System.Collections.Generic;
using xTile;

namespace StardewValley.Locations
{
  public class Club : GameLocation
  {
    public static int timesPlayedCalicoJack;
    public static int timesPlayedSlots;
    private string coinBuffer;

    public Club()
    {
    }

    public Club(Map map, string name)
      : base(map, name)
    {
    }

    public override void resetForPlayerEntry()
    {
      base.resetForPlayerEntry();
      this.lightGlows.Clear();
      this.coinBuffer = LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ru ? "     " : (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.zh ? "　　" : "  ");
      if (Game1.player.hasClubCard)
        return;
      Game1.currentLocation = Game1.getLocationFromName("SandyHouse");
      Game1.changeMusicTrack("none");
      Game1.currentLocation.resetForPlayerEntry();
      NPC characterFromName = Game1.currentLocation.getCharacterFromName("Bouncer");
      if (characterFromName != null)
      {
        Vector2 vector2 = new Vector2(17f, 4f);
        characterFromName.showTextAboveHead(Game1.content.LoadString("Strings\\Locations:Club_Bouncer_TextAboveHead" + (object) (Game1.random.Next(2) + 1)), -1, 2, 3000, 0);
        int num1 = Game1.random.Next();
        Game1.playSound("thudStep");
        List<TemporaryAnimatedSprite> temporarySprites1 = Game1.currentLocation.TemporarySprites;
        TemporaryAnimatedSprite temporaryAnimatedSprite1 = new TemporaryAnimatedSprite(288, 100f, 1, 24, vector2 * (float) Game1.tileSize, true, false, Game1.currentLocation, Game1.player);
        temporaryAnimatedSprite1.shakeIntensity = 0.5f;
        temporaryAnimatedSprite1.shakeIntensityChange = 1f / 500f;
        temporaryAnimatedSprite1.extraInfoForEndBehavior = num1;
        TemporaryAnimatedSprite.endBehavior endBehavior = new TemporaryAnimatedSprite.endBehavior(Game1.currentLocation.removeTemporarySpritesWithID);
        temporaryAnimatedSprite1.endFunction = endBehavior;
        temporarySprites1.Add(temporaryAnimatedSprite1);
        Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(598, 1279, 3, 4), 53f, 5, 9, vector2 * (float) Game1.tileSize + new Vector2(5f, 0.0f) * (float) Game1.pixelZoom, true, false, (float) (4 * Game1.tileSize + 7) / 10000f, 0.0f, Color.Yellow, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
        {
          id = (float) num1
        });
        List<TemporaryAnimatedSprite> temporarySprites2 = Game1.currentLocation.TemporarySprites;
        TemporaryAnimatedSprite temporaryAnimatedSprite2 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(598, 1279, 3, 4), 53f, 5, 9, vector2 * (float) Game1.tileSize + new Vector2(5f, 0.0f) * (float) Game1.pixelZoom, true, true, (float) (4 * Game1.tileSize + 7) / 10000f, 0.0f, Color.Orange, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
        temporaryAnimatedSprite2.delayBeforeAnimationStart = 100;
        double num2 = (double) num1;
        temporaryAnimatedSprite2.id = (float) num2;
        temporarySprites2.Add(temporaryAnimatedSprite2);
        List<TemporaryAnimatedSprite> temporarySprites3 = Game1.currentLocation.TemporarySprites;
        TemporaryAnimatedSprite temporaryAnimatedSprite3 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(598, 1279, 3, 4), 53f, 5, 9, vector2 * (float) Game1.tileSize + new Vector2(5f, 0.0f) * (float) Game1.pixelZoom, true, false, (float) (4 * Game1.tileSize + 7) / 10000f, 0.0f, Color.White, (float) Game1.pixelZoom * 0.75f, 0.0f, 0.0f, 0.0f, false);
        temporaryAnimatedSprite3.delayBeforeAnimationStart = 200;
        double num3 = (double) num1;
        temporaryAnimatedSprite3.id = (float) num3;
        temporarySprites3.Add(temporaryAnimatedSprite3);
        if (Game1.fuseSound != null && !Game1.fuseSound.IsPlaying)
        {
          Game1.fuseSound = Game1.soundBank.GetCue("fuse");
          Game1.fuseSound.Play();
        }
      }
      Game1.player.position = new Vector2(17f, 4f) * (float) Game1.tileSize;
    }

    public override void checkForMusic(GameTime time)
    {
      if (Game1.random.NextDouble() >= 0.002)
        return;
      Game1.playSound("boop");
    }

    public override void cleanupBeforePlayerExit()
    {
      Game1.changeMusicTrack("none");
      base.cleanupBeforePlayerExit();
    }

    public override void drawAboveAlwaysFrontLayer(SpriteBatch b)
    {
      base.drawAboveAlwaysFrontLayer(b);
      SpriteText.drawStringWithScrollBackground(b, this.coinBuffer + (object) Game1.player.clubCoins, Game1.tileSize, Game1.tileSize / 4, "", 1f, -1);
      Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float) (Game1.tileSize + Game1.pixelZoom), (float) (Game1.tileSize / 4 + Game1.pixelZoom)), new Rectangle(211, 373, 9, 10), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
    }
  }
}
