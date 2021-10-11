// Decompiled with JetBrains decompiler
// Type: StardewValley.Locations.Mountain
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using xTile;
using xTile.Dimensions;
using xTile.Tiles;

namespace StardewValley.Locations
{
  public class Mountain : GameLocation
  {
    private bool railroadAreaBlocked = Game1.stats.DaysPlayed < 31U;
    private bool landslide = Game1.stats.DaysPlayed < 5U;
    private Microsoft.Xna.Framework.Rectangle landSlideRect = new Microsoft.Xna.Framework.Rectangle(50 * Game1.tileSize, 4 * Game1.tileSize, 3 * Game1.tileSize, 5 * Game1.tileSize);
    private Microsoft.Xna.Framework.Rectangle railroadBlockRect = new Microsoft.Xna.Framework.Rectangle(8 * Game1.tileSize, 0, 4 * Game1.tileSize, 5 * Game1.tileSize);
    private Microsoft.Xna.Framework.Rectangle boulderSourceRect = new Microsoft.Xna.Framework.Rectangle(439, 1385, 39, 48);
    private Microsoft.Xna.Framework.Rectangle raildroadBlocksourceRect = new Microsoft.Xna.Framework.Rectangle(640, 2176, 64, 80);
    private Microsoft.Xna.Framework.Rectangle landSlideSourceRect = new Microsoft.Xna.Framework.Rectangle(646, 1218, 48, 80);
    private Vector2 boulderPosition = new Vector2(47f, 3f) * (float) Game1.tileSize - new Vector2(4f, 3f) * (float) Game1.pixelZoom;
    public const int daysBeforeLandslide = 31;
    private TemporaryAnimatedSprite minecartSteam;
    private bool bridgeRestored;
    private bool oreBoulderPresent;
    private int oldTime;

    public Mountain()
    {
      if (Game1.stats.DaysPlayed >= 5U)
        this.landslide = false;
      if (Game1.stats.DaysPlayed < 31U)
        return;
      this.railroadAreaBlocked = false;
    }

    public Mountain(Map map, string name)
      : base(map, name)
    {
      for (int index = 0; index < 10; ++index)
        this.quarryDayUpdate();
      if (Game1.stats.DaysPlayed >= 5U)
        this.landslide = false;
      if (Game1.stats.DaysPlayed < 31U)
        return;
      this.railroadAreaBlocked = false;
    }

    public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
    {
      if (this.map.GetLayer("Buildings").Tiles[tileLocation] != null)
      {
        switch (this.map.GetLayer("Buildings").Tiles[tileLocation].TileIndex)
        {
          case 1081:
          case 958:
          case 1080:
            if (Game1.player.getMount() != null)
              return true;
            if (Game1.player.mailReceived.Contains("ccBoilerRoom"))
            {
              if (Game1.player.isRidingHorse() && Game1.player.getMount() != null)
              {
                Game1.player.getMount().checkAction(Game1.player, (GameLocation) this);
                break;
              }
              Response[] answerChoices = new Response[4]
              {
                new Response("Bus", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_BusStop")),
                new Response("Mines", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Mines")),
                new Response("Town", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Town")),
                new Response("Cancel", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Cancel"))
              };
              this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:MineCart_ChooseDestination"), answerChoices, "Minecart");
              break;
            }
            Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:MineCart_OutOfOrder"));
            return true;
          case 1136:
            if (!who.mailReceived.Contains("guildMember") && !who.hasQuest(16))
            {
              Game1.drawLetterMessage(Game1.content.LoadString("Strings\\Locations:Mountain_AdventurersGuildNote").Replace('\n', '^'));
              return true;
            }
            break;
        }
      }
      return base.checkAction(tileLocation, viewport, who);
    }

    private void restoreBridge()
    {
      LocalizedContentManager temporary = Game1.content.CreateTemporary();
      Map map = temporary.Load<Map>("Maps\\Mountain-BridgeFixed");
      int num1 = 92;
      int num2 = 24;
      for (int index1 = 0; index1 < map.GetLayer("Back").LayerWidth; ++index1)
      {
        for (int index2 = 0; index2 < map.GetLayer("Back").LayerHeight; ++index2)
        {
          this.map.GetLayer("Back").Tiles[index1 + num1, index2 + num2] = map.GetLayer("Back").Tiles[index1, index2] == null ? (Tile) null : (Tile) new StaticTile(this.map.GetLayer("Back"), this.map.TileSheets[0], BlendMode.Alpha, map.GetLayer("Back").Tiles[index1, index2].TileIndex);
          this.map.GetLayer("Buildings").Tiles[index1 + num1, index2 + num2] = map.GetLayer("Buildings").Tiles[index1, index2] == null ? (Tile) null : (Tile) new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, map.GetLayer("Buildings").Tiles[index1, index2].TileIndex);
          this.map.GetLayer("Front").Tiles[index1 + num1, index2 + num2] = map.GetLayer("Front").Tiles[index1, index2] == null ? (Tile) null : (Tile) new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, map.GetLayer("Front").Tiles[index1, index2].TileIndex);
        }
      }
      this.bridgeRestored = true;
      temporary.Unload();
    }

    public override void resetForPlayerEntry()
    {
      base.resetForPlayerEntry();
      if (Game1.player.mailReceived.Contains("ccBoilerRoom"))
        this.minecartSteam = new TemporaryAnimatedSprite(27, new Vector2((float) (126 * Game1.tileSize + Game1.pixelZoom * 2), (float) (11 * Game1.tileSize) - (float) ((double) Game1.tileSize * 3.0 / 4.0)), Color.White, 8, false, 100f, 0, -1, -1f, -1, 0)
        {
          totalNumberOfLoops = 999999,
          interval = 60f,
          flipped = true
        };
      if (!this.bridgeRestored && Game1.player.mailReceived.Contains("ccCraftsRoom"))
        this.restoreBridge();
      this.oreBoulderPresent = !Game1.player.mailReceived.Contains("ccFishTank") || Game1.farmEvent != null;
      this.boulderSourceRect = new Microsoft.Xna.Framework.Rectangle(439 + (Game1.currentSeason.Equals("winter") ? 39 : 0), 1385, 39, 48);
      if (!this.objects.ContainsKey(new Vector2(29f, 9f)))
      {
        Vector2 tileLocation = new Vector2(29f, 9f);
        SerializableDictionary<Vector2, Object> objects = this.objects;
        Vector2 key = tileLocation;
        Torch torch = new Torch(tileLocation, 146, true);
        int num1 = 0;
        torch.isOn = num1 != 0;
        int num2 = 2;
        torch.fragility = num2;
        objects.Add(key, (Object) torch);
        this.objects[tileLocation].checkForAction((Farmer) null, false);
      }
      this.raildroadBlocksourceRect = !Game1.IsSpring ? new Microsoft.Xna.Framework.Rectangle(640, 1453, 64, 80) : new Microsoft.Xna.Framework.Rectangle(640, 2176, 64, 80);
      this.addFrog();
    }

    public override void DayUpdate(int dayOfMonth)
    {
      base.DayUpdate(dayOfMonth);
      this.quarryDayUpdate();
      if (Game1.stats.DaysPlayed >= 31U)
        this.railroadAreaBlocked = false;
      if (Game1.stats.DaysPlayed < 5U)
        return;
      this.landslide = false;
      if (Game1.player.hasOrWillReceiveMail("landslideDone"))
        return;
      Game1.mailbox.Enqueue("landslideDone");
    }

    private void quarryDayUpdate()
    {
      Microsoft.Xna.Framework.Rectangle r = new Microsoft.Xna.Framework.Rectangle(106, 13, 21, 21);
      int num = 5;
      for (int index = 0; index < num; ++index)
      {
        Vector2 positionInThisRectangle = Utility.getRandomPositionInThisRectangle(r, Game1.random);
        if (this.isTileOpenForQuarryStone((int) positionInThisRectangle.X, (int) positionInThisRectangle.Y))
        {
          if (Game1.random.NextDouble() < 0.06)
          {
            if (!this.isTileOpenForQuarryStone((int) positionInThisRectangle.X + 1, (int) positionInThisRectangle.Y) || !this.isTileOpenForQuarryStone((int) positionInThisRectangle.Y, (int) positionInThisRectangle.Y + 1) || !this.isTileOpenForQuarryStone((int) positionInThisRectangle.X + 1, (int) positionInThisRectangle.Y + 1))
              ;
          }
          else if (Game1.random.NextDouble() < 0.02)
          {
            if (Game1.random.NextDouble() < 0.1)
              this.objects.Add(positionInThisRectangle, new Object(positionInThisRectangle, 46, "Stone", true, false, false, false)
              {
                minutesUntilReady = 12
              });
            else
              this.objects.Add(positionInThisRectangle, new Object(positionInThisRectangle, (Game1.random.Next(7) + 1) * 2, "Stone", true, false, false, false)
              {
                minutesUntilReady = 5
              });
          }
          else if (Game1.random.NextDouble() < 0.1)
          {
            if (Game1.random.NextDouble() < 0.001)
              this.objects.Add(positionInThisRectangle, new Object(positionInThisRectangle, 765, 1)
              {
                minutesUntilReady = 16
              });
            else if (Game1.random.NextDouble() < 0.1)
              this.objects.Add(positionInThisRectangle, new Object(positionInThisRectangle, 764, 1)
              {
                minutesUntilReady = 8
              });
            else if (Game1.random.NextDouble() < 0.33)
              this.objects.Add(positionInThisRectangle, new Object(positionInThisRectangle, 290, 1)
              {
                minutesUntilReady = 5
              });
            else
              this.objects.Add(positionInThisRectangle, new Object(positionInThisRectangle, 751, 1)
              {
                minutesUntilReady = 3
              });
          }
          else
            this.objects.Add(positionInThisRectangle, new Object(positionInThisRectangle, Game1.random.NextDouble() < 0.25 ? 32 : (Game1.random.NextDouble() < 0.33 ? 38 : (Game1.random.NextDouble() < 0.5 ? 40 : 42)), 1)
            {
              minutesUntilReady = 2,
              name = "Stone"
            });
        }
      }
    }

    private bool isTileOpenForQuarryStone(int tileX, int tileY)
    {
      if (this.doesTileHaveProperty(tileX, tileY, "Diggable", "Back") != null)
        return this.isTileLocationTotallyClearAndPlaceable(new Vector2((float) tileX, (float) tileY));
      return false;
    }

    public override void cleanupBeforePlayerExit()
    {
      base.cleanupBeforePlayerExit();
      this.minecartSteam = (TemporaryAnimatedSprite) null;
    }

    public override void UpdateWhenCurrentLocation(GameTime time)
    {
      base.UpdateWhenCurrentLocation(time);
      if (this.minecartSteam != null)
        this.minecartSteam.update(time);
      if (!this.landslide || ((int) ((Game1.currentGameTime.TotalGameTime.TotalMilliseconds - 400.0) / 1600.0) % 2 == 0 || !Utility.isOnScreen(new Point(this.landSlideRect.X / Game1.tileSize, this.landSlideRect.Y / Game1.tileSize), Game1.tileSize * 2, (GameLocation) null)))
        return;
      if (Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 400.0 < (double) (this.oldTime % 400))
        Game1.playSound("hammer");
      this.oldTime = (int) time.TotalGameTime.TotalMilliseconds;
    }

    public override Object getFish(float millisecondsAfterNibble, int bait, int waterDepth, Farmer who, double baitPotency)
    {
      if (Game1.currentSeason.Equals("spring") && Game1.isRaining && (who.FishingLevel >= 10 && !who.fishCaught.ContainsKey(163)) && (waterDepth >= 4 && Game1.random.NextDouble() < 0.1))
        return new Object(163, 1, false, -1, 0);
      return base.getFish(millisecondsAfterNibble, bait, waterDepth, who, baitPotency);
    }

    public override bool isCollidingPosition(Microsoft.Xna.Framework.Rectangle position, xTile.Dimensions.Rectangle viewport, bool isFarmer, int damagesFarmer, bool glider, Character character)
    {
      if (this.landslide && position.Intersects(this.landSlideRect) || this.railroadAreaBlocked && position.Intersects(this.railroadBlockRect))
        return true;
      return base.isCollidingPosition(position, viewport, isFarmer, damagesFarmer, glider, character);
    }

    public override void draw(SpriteBatch spriteBatch)
    {
      base.draw(spriteBatch);
      if (this.minecartSteam != null)
        this.minecartSteam.draw(spriteBatch, false, 0, 0);
      if (this.oreBoulderPresent)
        spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.boulderPosition), new Microsoft.Xna.Framework.Rectangle?(this.boulderSourceRect), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.0001f);
      if (this.railroadAreaBlocked)
        spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.railroadBlockRect), new Microsoft.Xna.Framework.Rectangle?(this.raildroadBlocksourceRect), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, (float) ((double) (3 * Game1.tileSize) / 10000.0 + 9.99999974737875E-05));
      if (!this.landslide)
        return;
      spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.landSlideRect), new Microsoft.Xna.Framework.Rectangle?(this.landSlideSourceRect), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, (float) (3 * Game1.tileSize) / 10000f);
      SpriteBatch spriteBatch1 = spriteBatch;
      Texture2D shadowTexture = Game1.shadowTexture;
      Vector2 local = Game1.GlobalToLocal(new Vector2((float) (this.landSlideRect.X + Game1.tileSize * 3 - Game1.pixelZoom * 5), (float) (this.landSlideRect.Y + Game1.tileSize * 3 + Game1.pixelZoom * 5)) + new Vector2(32f, 24f));
      Microsoft.Xna.Framework.Rectangle? sourceRectangle = new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds);
      Color white = Color.White;
      double num1 = 0.0;
      Microsoft.Xna.Framework.Rectangle bounds = Game1.shadowTexture.Bounds;
      double x = (double) bounds.Center.X;
      bounds = Game1.shadowTexture.Bounds;
      double y = (double) bounds.Center.Y;
      Vector2 origin = new Vector2((float) x, (float) y);
      double pixelZoom = (double) Game1.pixelZoom;
      int num2 = 0;
      double num3 = 3.5 * (double) Game1.tileSize / 10000.0;
      spriteBatch1.Draw(shadowTexture, local, sourceRectangle, white, (float) num1, origin, (float) pixelZoom, (SpriteEffects) num2, (float) num3);
      spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(new Vector2((float) (this.landSlideRect.X + Game1.tileSize * 3 - Game1.pixelZoom * 5), (float) (this.landSlideRect.Y + Game1.tileSize * 2))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(288 + ((int) (Game1.currentGameTime.TotalGameTime.TotalMilliseconds / 1600.0 % 2.0) == 0 ? 0 : (int) (Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 400.0 / 100.0) * 19), 1349, 19, 28)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, (float) (4 * Game1.tileSize) / 10000f);
      spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(new Vector2((float) (this.landSlideRect.X + Game1.tileSize * 4 - Game1.pixelZoom * 5), (float) (this.landSlideRect.Y + Game1.tileSize * 2))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(335, 1410, 21, 21)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, (float) (2 * Game1.tileSize) / 10000f);
    }
  }
}
