// Decompiled with JetBrains decompiler
// Type: StardewValley.Locations.BusStop
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Characters;
using System;
using xTile;
using xTile.Dimensions;

namespace StardewValley.Locations
{
  public class BusStop : GameLocation
  {
    private Microsoft.Xna.Framework.Rectangle busSource = new Microsoft.Xna.Framework.Rectangle(288, 1247, 128, 64);
    private Microsoft.Xna.Framework.Rectangle pamSource = new Microsoft.Xna.Framework.Rectangle(384, 1311, 15, 19);
    private Vector2 pamOffset = new Vector2(0.0f, 29f);
    public const int busDefaultXTile = 11;
    public const int busDefaultYTile = 6;
    private TemporaryAnimatedSprite minecartSteam;
    private TemporaryAnimatedSprite busDoor;
    private Vector2 busPosition;
    private Vector2 busMotion;
    private bool drivingOff;
    private bool drivingBack;
    private int forceWarpTimer;

    public BusStop()
    {
    }

    public BusStop(Map map, string name)
      : base(map, name)
    {
      this.busPosition = new Vector2(11f, 6f) * (float) Game1.tileSize;
    }

    public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
    {
      if (this.map.GetLayer("Buildings").Tiles[tileLocation] != null)
      {
        switch (this.map.GetLayer("Buildings").Tiles[tileLocation].TileIndex)
        {
          case 1080:
          case 1081:
          case 958:
            if (Game1.player.getMount() != null)
              return true;
            if (Game1.player.mailReceived.Contains("ccBoilerRoom"))
            {
              if (Game1.player.isRidingHorse() && Game1.player.getMount() != null)
              {
                Game1.player.getMount().checkAction(Game1.player, (GameLocation) this);
              }
              else
              {
                Response[] answerChoices;
                if (Game1.player.mailReceived.Contains("ccCraftsRoom"))
                  answerChoices = new Response[4]
                  {
                    new Response("Mines", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Mines")),
                    new Response("Town", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Town")),
                    new Response("Quarry", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Quarry")),
                    new Response("Cancel", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Cancel"))
                  };
                else
                  answerChoices = new Response[3]
                  {
                    new Response("Mines", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Mines")),
                    new Response("Town", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Town")),
                    new Response("Cancel", Game1.content.LoadString("Strings\\Locations:MineCart_Destination_Cancel"))
                  };
                this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:MineCart_ChooseDestination"), answerChoices, "Minecart");
                break;
              }
            }
            else
              Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:MineCart_OutOfOrder"));
            return true;
          case 1057:
            if (Game1.player.mailReceived.Contains("ccVault"))
            {
              if (Game1.player.isRidingHorse() && Game1.player.getMount() != null)
              {
                Game1.player.getMount().checkAction(Game1.player, (GameLocation) this);
              }
              else
              {
                if (Game1.content.GetCurrentLanguage() == LocalizedContentManager.LanguageCode.es)
                {
                  this.createQuestionDialogueWithCustomWidth(Game1.content.LoadString("Strings\\Locations:BusStop_BuyTicketToDesert"), this.createYesNoResponses(), "Bus");
                  break;
                }
                this.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:BusStop_BuyTicketToDesert"), this.createYesNoResponses(), "Bus");
                break;
              }
            }
            else
              Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:BusStop_DesertOutOfService"));
            return true;
        }
      }
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
      this.forceWarpTimer = 0;
      Game1.player.position.X = -10000f;
      this.busDriveOff();
      Game1.playSound("stoneStep");
      if (Game1.player.getMount() == null)
        return;
      Game1.player.getMount().farmerPassesThrough = false;
    }

    public override bool answerDialogue(Response answer)
    {
      if (!(this.lastQuestionKey.Split(' ')[0] + "_" + answer.responseKey == "Bus_Yes"))
        return base.answerDialogue(answer);
      NPC characterFromName = Game1.getCharacterFromName("Pam", false);
      if (Game1.player.Money >= (Game1.shippingTax ? 50 : 500) && this.characters.Contains(characterFromName) && characterFromName.getTileLocation().Equals(new Vector2(11f, 10f)))
      {
        Game1.player.Money -= Game1.shippingTax ? 50 : 500;
        characterFromName.ignoreMultiplayerUpdates = true;
        characterFromName.faceTowardFarmerTimer = 0;
        characterFromName.faceTowardFarmer = false;
        characterFromName.faceAwayFromFarmer = false;
        characterFromName.movementPause = 1;
        characterFromName.controller = new PathFindController((Character) characterFromName, (GameLocation) this, new Point(12, 9), 0, new PathFindController.endBehavior(this.pamReachedBusDoor));
        characterFromName.forceUpdateTimer = 15000;
        Game1.freezeControls = true;
        Game1.viewportFreeze = true;
        this.forceWarpTimer = 8000;
        Game1.player.controller = new PathFindController((Character) Game1.player, (GameLocation) this, new Point(12, 9), 0, new PathFindController.endBehavior(this.playerReachedBusDoor));
        Game1.player.setRunning(true, false);
        if (Game1.player.getMount() != null)
          Game1.player.getMount().farmerPassesThrough = true;
      }
      else if (Game1.player.Money < (Game1.shippingTax ? 50 : 500))
        Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:BusStop_NotEnoughMoneyForTicket"));
      else
        Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:BusStop_NoDriver"));
      return true;
    }

    public override void resetForPlayerEntry()
    {
      base.resetForPlayerEntry();
      if (Game1.player.mailReceived.Contains("ccBoilerRoom"))
        this.minecartSteam = new TemporaryAnimatedSprite(27, new Vector2((float) (6 * Game1.tileSize + Game1.pixelZoom * 2), (float) (3 * Game1.tileSize) - (float) ((double) Game1.tileSize * 3.0 / 4.0)), Color.White, 8, false, 100f, 0, -1, -1f, -1, 0)
        {
          totalNumberOfLoops = 999999,
          interval = 60f,
          flipped = true
        };
      if (Game1.getFarm().grandpaScore == 0 && Game1.year >= 3 && Game1.player.eventsSeen.Contains(558292))
        Game1.player.eventsSeen.Remove(558292);
      if (Game1.player.getTileY() > 16 || Game1.eventUp || (Game1.player.getTileX() == 0 || Game1.player.isRidingHorse()) || !Game1.player.previousLocationName.Equals("Desert"))
      {
        this.drivingOff = false;
        this.drivingBack = false;
        this.busMotion = Vector2.Zero;
        this.busPosition = new Vector2(11f, 6f) * (float) Game1.tileSize;
        TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(288, 1311, 16, 38), this.busPosition + new Vector2(16f, 26f) * (float) Game1.pixelZoom, false, 0.0f, Color.White);
        temporaryAnimatedSprite.interval = 999999f;
        temporaryAnimatedSprite.animationLength = 6;
        temporaryAnimatedSprite.holdLastFrame = true;
        double num = ((double) this.busPosition.Y + (double) (3 * Game1.tileSize)) / 10000.0 + 9.99999974737875E-06;
        temporaryAnimatedSprite.layerDepth = (float) num;
        double pixelZoom = (double) Game1.pixelZoom;
        temporaryAnimatedSprite.scale = (float) pixelZoom;
        this.busDoor = temporaryAnimatedSprite;
      }
      else
      {
        this.busPosition = new Vector2(11f, 6f) * (float) Game1.tileSize;
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
      this.minecartSteam = (TemporaryAnimatedSprite) null;
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
    }

    public void busDriveBack()
    {
      this.busPosition.X = (float) this.map.GetLayer("Back").DisplayWidth;
      this.busDoor.Position = this.busPosition + new Vector2(16f, 26f) * (float) Game1.pixelZoom;
      this.drivingBack = true;
      this.drivingOff = false;
      Game1.playSound("busDriveOff");
      this.busMotion = new Vector2(-6f, 0.0f);
      Game1.freezeControls = true;
    }

    private void busStartMovingOff(int extraInfo)
    {
      Game1.playSound("batFlap");
      this.drivingOff = true;
      Game1.playSound("busDriveOff");
      Game1.changeMusicTrack("none");
    }

    private void pamReturnedToSpot(Character c, GameLocation l)
    {
      c.ignoreMultiplayerUpdates = false;
    }

    private void doorOpenAfterReturn(int extraInfo)
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
      Game1.player.position = new Vector2(12f, 10f) * (float) Game1.tileSize;
      this.lastTouchActionLocation = Game1.player.getTileLocation();
      Game1.displayFarmer = true;
      Game1.player.forceCanMove();
      Game1.player.faceDirection(2);
      NPC characterFromName = Game1.getCharacterFromName("Pam", false);
      if (characterFromName == null)
        return;
      if (this.characters.Contains(characterFromName))
      {
        characterFromName.speed = 2;
        characterFromName.position = new Vector2(11f, 6f) * (float) Game1.tileSize;
        characterFromName.controller = new PathFindController((Character) characterFromName, (GameLocation) this, new Point(11, 10), 2, new PathFindController.endBehavior(this.pamReturnedToSpot));
      }
      else
        characterFromName.ignoreMultiplayerUpdates = false;
    }

    private void busLeftToDesert()
    {
      Game1.viewport.Y = -100000;
      Game1.viewportFreeze = true;
      Game1.warpFarmer("Desert", 16, 24, true);
      Game1.freezeControls = false;
      Game1.globalFade = false;
    }

    public override void UpdateWhenCurrentLocation(GameTime time)
    {
      base.UpdateWhenCurrentLocation(time);
      if (this.forceWarpTimer > 0)
      {
        this.forceWarpTimer = this.forceWarpTimer - time.ElapsedGameTime.Milliseconds;
        if (this.forceWarpTimer <= 0)
          this.playerReachedBusDoor((Character) Game1.player, (GameLocation) this);
      }
      if (this.minecartSteam != null)
        this.minecartSteam.update(time);
      if (this.drivingOff)
      {
        this.busMotion.X -= 0.075f;
        if ((double) this.busPosition.X + (double) (8 * Game1.tileSize) < 0.0)
        {
          this.drivingOff = false;
          Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.busLeftToDesert), 0.01f);
        }
      }
      if (this.drivingBack)
      {
        Game1.player.position = this.busPosition;
        if ((double) this.busPosition.X - (double) (11 * Game1.tileSize) < (double) (Game1.tileSize * 4))
          this.busMotion.X = Math.Min(-1f, this.busMotion.X * 0.98f);
        if ((double) Math.Abs(this.busPosition.X - (float) (11 * Game1.tileSize)) <= (double) Math.Abs(this.busMotion.X * 1.5f))
        {
          this.busPosition.X = (float) (11 * Game1.tileSize);
          this.busMotion = Vector2.Zero;
          this.drivingBack = false;
          this.busDoor.Position = this.busPosition + new Vector2(16f, 26f) * (float) Game1.pixelZoom;
          this.busDoor.pingPong = true;
          this.busDoor.interval = 70f;
          this.busDoor.currentParentTileIndex = 5;
          this.busDoor.endFunction = new TemporaryAnimatedSprite.endBehavior(this.doorOpenAfterReturn);
          Game1.playSound("trashcanlid");
          for (int index = 0; index < this.characters.Count; ++index)
          {
            if (this.characters[index] is Horse)
              Game1.showGlobalMessage(Game1.content.LoadString("Strings\\Locations:BusStop_ReturnToHorse" + (object) (Game1.random.Next(2) + 1), (object) this.characters[index].displayName));
          }
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
      if (this.minecartSteam != null)
        this.minecartSteam.draw(spriteBatch, false, 0, 0);
      spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.busPosition), new Microsoft.Xna.Framework.Rectangle?(this.busSource), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, (float) (((double) this.busPosition.Y + (double) (3 * Game1.tileSize)) / 10000.0));
      if (this.busDoor != null)
        this.busDoor.draw(spriteBatch, false, 0, 0);
      if (!this.drivingOff && !this.drivingBack)
        return;
      spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, this.busPosition + this.pamOffset * (float) Game1.pixelZoom), new Microsoft.Xna.Framework.Rectangle?(this.pamSource), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, (float) (((double) this.busPosition.Y + (double) (3 * Game1.tileSize) + (double) Game1.pixelZoom) / 10000.0));
    }
  }
}
