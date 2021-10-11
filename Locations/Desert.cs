// Decompiled with JetBrains decompiler
// Type: StardewValley.Locations.Desert
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using xTile;
using xTile.Dimensions;

namespace StardewValley.Locations
{
  public class Desert : GameLocation
  {
    private Microsoft.Xna.Framework.Rectangle busSource = new Microsoft.Xna.Framework.Rectangle(288, 1247, 128, 64);
    private Microsoft.Xna.Framework.Rectangle pamSource = new Microsoft.Xna.Framework.Rectangle(384, 1311, 15, 19);
    private Vector2 pamOffset = new Vector2(0.0f, 29f);
    public const int busDefaultXTile = 17;
    public const int busDefaultYTile = 24;
    private TemporaryAnimatedSprite busDoor;
    private Vector2 busPosition;
    private Vector2 busMotion;
    private bool drivingOff;
    private bool drivingBack;

    public Desert()
    {
    }

    public Desert(Map map, string name)
      : base(map, name)
    {
    }

    public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
    {
      if (this.map.GetLayer("Buildings").Tiles[tileLocation] == null)
        return base.checkAction(tileLocation, viewport, who);
      int tileIndex = this.map.GetLayer("Buildings").Tiles[tileLocation].TileIndex;
      return base.checkAction(tileLocation, viewport, who);
    }

    private void pamReachedBusDoor(Character c, GameLocation l)
    {
      Game1.changeMusicTrack("none");
      c.position.X = -10000f;
      Game1.playSound("stoneStep");
    }

    private void playerReachedBusDoor(Character c, GameLocation l)
    {
      Game1.player.position.X = -10000f;
      this.busDriveOff();
      Game1.playSound("stoneStep");
    }

    public override bool answerDialogue(Response answer)
    {
      if (!(this.lastQuestionKey.Split(' ')[0] + "_" + answer.responseKey == "DesertBus_Yes"))
        return base.answerDialogue(answer);
      this.playerReachedBusDoor((Character) Game1.player, (GameLocation) this);
      return true;
    }

    public override void resetForPlayerEntry()
    {
      base.resetForPlayerEntry();
      Game1.ambientLight = Color.White;
      if (Game1.player.getTileY() > 40 || Game1.player.getTileY() < 10)
      {
        this.drivingOff = false;
        this.drivingBack = false;
        this.busMotion = Vector2.Zero;
        this.busPosition = new Vector2(17f, 24f) * (float) Game1.tileSize;
        TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(288, 1311, 16, 38), this.busPosition + new Vector2(16f, 26f) * (float) Game1.pixelZoom, false, 0.0f, Color.White);
        temporaryAnimatedSprite.interval = 999999f;
        temporaryAnimatedSprite.animationLength = 6;
        temporaryAnimatedSprite.holdLastFrame = true;
        double num = ((double) this.busPosition.Y + (double) (3 * Game1.tileSize)) / 10000.0 + 9.99999974737875E-06;
        temporaryAnimatedSprite.layerDepth = (float) num;
        double pixelZoom = (double) Game1.pixelZoom;
        temporaryAnimatedSprite.scale = (float) pixelZoom;
        this.busDoor = temporaryAnimatedSprite;
        Game1.changeMusicTrack("wavy");
      }
      else
      {
        if (Game1.isRaining)
          Game1.changeMusicTrack("none");
        this.busPosition = new Vector2(17f, 24f) * (float) Game1.tileSize;
        TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(368, 1311, 16, 38), this.busPosition + new Vector2(16f, 26f) * (float) Game1.pixelZoom, false, 0.0f, Color.White);
        temporaryAnimatedSprite.interval = 999999f;
        temporaryAnimatedSprite.animationLength = 1;
        temporaryAnimatedSprite.holdLastFrame = true;
        double num = ((double) this.busPosition.Y + (double) (3 * Game1.tileSize)) / 10000.0 + 9.99999974737875E-06;
        temporaryAnimatedSprite.layerDepth = (float) num;
        double pixelZoom = (double) Game1.pixelZoom;
        temporaryAnimatedSprite.scale = (float) pixelZoom;
        this.busDoor = temporaryAnimatedSprite;
        Game1.displayFarmer = false;
        this.busDriveBack();
      }
    }

    public override void cleanupBeforePlayerExit()
    {
      base.cleanupBeforePlayerExit();
      this.busDoor = (TemporaryAnimatedSprite) null;
    }

    public void busDriveOff()
    {
      TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(288, 1311, 16, 38), this.busPosition + new Vector2(16f, 26f) * (float) Game1.pixelZoom, false, 0.0f, Color.White);
      temporaryAnimatedSprite.interval = 999999f;
      temporaryAnimatedSprite.animationLength = 6;
      temporaryAnimatedSprite.holdLastFrame = true;
      double num = ((double) this.busPosition.Y + (double) (3 * Game1.tileSize)) / 10000.0 + 9.99999974737875E-06;
      temporaryAnimatedSprite.layerDepth = (float) num;
      double pixelZoom = (double) Game1.pixelZoom;
      temporaryAnimatedSprite.scale = (float) pixelZoom;
      this.busDoor = temporaryAnimatedSprite;
      this.busDoor.timer = 0.0f;
      this.busDoor.interval = 70f;
      this.busDoor.endFunction = new TemporaryAnimatedSprite.endBehavior(this.busStartMovingOff);
      Game1.playSound("trashcanlid");
      this.drivingBack = false;
      this.busDoor.paused = false;
      Game1.changeMusicTrack("none");
    }

    public void busDriveBack()
    {
      this.busPosition.X = (float) this.map.GetLayer("Back").DisplayWidth;
      this.busDoor.Position = this.busPosition + new Vector2(16f, 26f) * (float) Game1.pixelZoom;
      this.drivingBack = true;
      this.drivingOff = false;
      Game1.playSound("busDriveOff");
      this.busMotion = new Vector2(-6f, 0.0f);
    }

    private void busStartMovingOff(int extraInfo)
    {
      Game1.playSound("batFlap");
      this.drivingOff = true;
      Game1.playSound("busDriveOff");
    }

    public override void performTouchAction(string fullActionString, Vector2 playerStandingPosition)
    {
      if (fullActionString.Split(' ')[0] == "DesertBus")
      {
        Response[] answerChoices = new Response[2]
        {
          new Response("Yes", Game1.content.LoadString("Strings\\Locations:Desert_Return_Yes")),
          new Response("Not", Game1.content.LoadString("Strings\\Locations:Desert_Return_No"))
        };
        this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:Desert_Return_Question"), answerChoices, "DesertBus");
      }
      else
        base.performTouchAction(fullActionString, playerStandingPosition);
    }

    private void doorOpenAfterReturn(int extraInfo)
    {
      Game1.playSound("batFlap");
      TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(288, 1311, 16, 38), this.busPosition + new Vector2(16f, 26f) * (float) Game1.pixelZoom, false, 0.0f, Color.White);
      temporaryAnimatedSprite.interval = 999999f;
      temporaryAnimatedSprite.animationLength = 6;
      temporaryAnimatedSprite.holdLastFrame = true;
      double num = ((double) this.busPosition.Y + (double) (3 * Game1.tileSize)) / 10000.0 + 9.99999974737875E-06;
      temporaryAnimatedSprite.layerDepth = (float) num;
      double pixelZoom = (double) Game1.pixelZoom;
      temporaryAnimatedSprite.scale = (float) pixelZoom;
      this.busDoor = temporaryAnimatedSprite;
      Game1.player.position = new Vector2(18f, 27f) * (float) Game1.tileSize;
      this.lastTouchActionLocation = Game1.player.getTileLocation();
      Game1.displayFarmer = true;
      Game1.player.forceCanMove();
      Game1.player.faceDirection(2);
      Game1.changeMusicTrack("wavy");
    }

    private void busLeftToValley()
    {
      Game1.viewport.Y = -100000;
      Game1.viewportFreeze = true;
      Game1.warpFarmer("BusStop", 12, 10, true);
      Game1.freezeControls = false;
    }

    public override void UpdateWhenCurrentLocation(GameTime time)
    {
      base.UpdateWhenCurrentLocation(time);
      if (this.drivingOff)
      {
        this.busMotion.X -= 0.075f;
        if ((double) this.busPosition.X + (double) (8 * Game1.tileSize) < 0.0)
        {
          this.drivingOff = false;
          Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.busLeftToValley), 0.01f);
        }
      }
      if (this.drivingBack)
      {
        Game1.player.position = this.busDoor.position;
        Game1.player.freezePause = 100;
        if ((double) this.busPosition.X - (double) (17 * Game1.tileSize) < (double) (Game1.tileSize * 4))
          this.busMotion.X = Math.Min(-1f, this.busMotion.X * 0.98f);
        if ((double) Math.Abs(this.busPosition.X - (float) (17 * Game1.tileSize)) <= (double) Math.Abs(this.busMotion.X * 1.5f))
        {
          this.busPosition.X = (float) (17 * Game1.tileSize);
          this.busMotion = Vector2.Zero;
          this.drivingBack = false;
          this.busDoor.Position = this.busPosition + new Vector2(16f, 26f) * (float) Game1.pixelZoom;
          this.busDoor.pingPong = true;
          this.busDoor.interval = 70f;
          this.busDoor.currentParentTileIndex = 5;
          this.busDoor.endFunction = new TemporaryAnimatedSprite.endBehavior(this.doorOpenAfterReturn);
          Game1.playSound("trashcanlid");
        }
      }
      if (!this.busMotion.Equals(Vector2.Zero))
      {
        this.busPosition = this.busPosition + this.busMotion;
        this.busDoor.Position += this.busMotion;
      }
      if (this.busDoor == null)
        return;
      this.busDoor.update(time);
    }

    public override void draw(SpriteBatch spriteBatch)
    {
      base.draw(spriteBatch);
      spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.busPosition), new Microsoft.Xna.Framework.Rectangle?(this.busSource), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, (float) (((double) this.busPosition.Y + (double) (3 * Game1.tileSize)) / 10000.0));
      if (this.busDoor != null)
        this.busDoor.draw(spriteBatch, false, 0, 0);
      if (!this.drivingOff && !this.drivingBack)
        return;
      spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.busPosition + this.pamOffset * (float) Game1.pixelZoom), new Microsoft.Xna.Framework.Rectangle?(this.pamSource), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, (float) (((double) this.busPosition.Y + (double) (3 * Game1.tileSize) + (double) Game1.pixelZoom) / 10000.0));
    }
  }
}
