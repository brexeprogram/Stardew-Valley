// Decompiled with JetBrains decompiler
// Type: StardewValley.Minigames.FishingGame
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.Menus;
using StardewValley.Tools;
using System;
using xTile;
using xTile.Dimensions;

namespace StardewValley.Minigames
{
  public class FishingGame : IMinigame
  {
    private int timerToStart = 1000;
    private GameLocation location;
    private LocalizedContentManager content;
    private Item tempItemStash;
    private int gameEndTimer;
    private int showResultsTimer;
    private bool exit;
    private bool gameDone;
    public int score;
    public int fishCaught;
    public int starTokensWon;
    public int perfections;
    public int perfectionBonus;

    public FishingGame()
    {
      this.content = Game1.content.CreateTemporary();
      this.location = new GameLocation(this.content.Load<Map>("Maps\\FishingGame"), "fishingGame");
      Game1.player.CurrentToolIndex = 0;
      this.tempItemStash = Game1.player.addItemToInventory((Item) new FishingRod(), 0);
      (Game1.player.CurrentTool as FishingRod).attachments[0] = new StardewValley.Object(690, 99, false, -1, 0);
      (Game1.player.CurrentTool as FishingRod).attachments[1] = new StardewValley.Object(687, 1, false, -1, 0);
      Game1.player.CurrentToolIndex = 0;
      Game1.globalFadeToClear((Game1.afterFadeFunction) null, 0.01f);
      this.location.Map.LoadTileSheets(Game1.mapDisplayDevice);
      Game1.player.position = new Vector2(14f, 7f) * (float) Game1.tileSize;
      Game1.player.currentLocation = this.location;
      this.changeScreenSize();
      this.gameEndTimer = 100000;
      this.showResultsTimer = -1;
      Game1.player.faceDirection(3);
      Game1.player.Halt();
    }

    public bool overrideFreeMouseMovement()
    {
      return false;
    }

    public bool tick(GameTime time)
    {
      Rumble.update((float) time.ElapsedGameTime.Milliseconds);
      this.location.UpdateWhenCurrentLocation(time);
      this.location.updateEvenIfFarmerIsntHere(time, false);
      Game1.player.Stamina = (float) Game1.player.MaxStamina;
      Game1.player.Update(time, this.location);
      for (int index = Game1.screenOverlayTempSprites.Count - 1; index >= 0; --index)
      {
        if (Game1.screenOverlayTempSprites[index].update(time))
          Game1.screenOverlayTempSprites.RemoveAt(index);
      }
      if (Game1.activeClickableMenu != null)
        Game1.updateActiveMenu(time);
      if (this.timerToStart > 0)
      {
        Game1.player.faceDirection(3);
        this.timerToStart = this.timerToStart - time.ElapsedGameTime.Milliseconds;
        if (this.timerToStart <= 0)
          Game1.playSound("whistle");
      }
      else if (this.showResultsTimer >= 0)
      {
        int showResultsTimer = this.showResultsTimer;
        this.showResultsTimer = this.showResultsTimer - time.ElapsedGameTime.Milliseconds;
        int num1 = 11000;
        if (showResultsTimer > num1 && this.showResultsTimer <= 11000)
          Game1.playSound("smallSelect");
        int num2 = 9000;
        if (showResultsTimer > num2 && this.showResultsTimer <= 9000)
          Game1.playSound("smallSelect");
        int num3 = 7000;
        if (showResultsTimer > num3 && this.showResultsTimer <= 7000)
        {
          if (this.perfections > 0)
          {
            this.score = this.score + this.perfections * 10;
            this.perfectionBonus = this.perfections * 10;
            if (this.fishCaught >= 3 && this.perfections >= 3)
            {
              this.perfectionBonus = this.perfectionBonus + this.score;
              this.score = this.score * 2;
            }
            Game1.playSound("newArtifact");
          }
          else
            Game1.playSound("smallSelect");
        }
        int num4 = 5000;
        if (showResultsTimer > num4 && this.showResultsTimer <= 5000)
        {
          if (this.score >= 10)
          {
            Game1.playSound("reward");
            this.starTokensWon = (this.score + 5) / 10 * 6;
            Game1.player.festivalScore += this.starTokensWon;
          }
          else
            Game1.playSound("fishEscape");
        }
        if (this.showResultsTimer <= 0)
        {
          Game1.globalFadeToClear((Game1.afterFadeFunction) null, 0.02f);
          return true;
        }
      }
      else if (!this.gameDone)
      {
        this.gameEndTimer = this.gameEndTimer - time.ElapsedGameTime.Milliseconds;
        if (this.gameEndTimer <= 0 && Game1.activeClickableMenu == null && (!Game1.player.UsingTool || (Game1.player.CurrentTool as FishingRod).isFishing))
        {
          if (Game1.player.usingTool)
          {
            this.receiveLeftClick(0, 0, true);
            if (Game1.player.CurrentTool != null && Game1.player.CurrentTool is FishingRod)
              (Game1.player.CurrentTool as FishingRod).doneFishing(Game1.player, false);
          }
          Game1.player.completelyStopAnimatingOrDoingAction();
          Game1.playSound("whistle");
          this.gameEndTimer = 1000;
          this.gameDone = true;
        }
      }
      else if (this.gameDone && this.gameEndTimer > 0)
      {
        this.gameEndTimer = this.gameEndTimer - time.ElapsedGameTime.Milliseconds;
        if (this.gameEndTimer <= 0)
        {
          Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.gameDoneAfterFade), 0.01f);
          Game1.exitActiveMenu();
          Game1.player.forceCanMove();
        }
      }
      return this.exit;
    }

    public void gameDoneAfterFade()
    {
      this.showResultsTimer = 11100;
      Game1.player.canMove = false;
      Game1.player.position = new Vector2(24f, 71f) * (float) Game1.tileSize;
      Game1.player.temporaryImpassableTile = new Microsoft.Xna.Framework.Rectangle(24 * Game1.tileSize, 71 * Game1.tileSize, Game1.tileSize, Game1.tileSize);
      Game1.player.faceDirection(2);
      Utility.killAllStaticLoopingSoundCues();
      if (FishingRod.reelSound == null || !FishingRod.reelSound.IsPlaying)
        return;
      FishingRod.reelSound.Stop(AudioStopOptions.Immediate);
    }

    public void receiveLeftClick(int x, int y, bool playSound = true)
    {
      if (Game1.isAnyGamePadButtonBeingPressed())
        return;
      if (this.timerToStart <= 0 && this.showResultsTimer < 0 && (!this.gameDone && Game1.activeClickableMenu == null) && (!(Game1.player.CurrentTool as FishingRod).hit && !(Game1.player.CurrentTool as FishingRod).pullingOutOfWater && (!(Game1.player.CurrentTool as FishingRod).isCasting && !(Game1.player.CurrentTool as FishingRod).fishCaught)))
      {
        Game1.player.lastClick = Vector2.Zero;
        Game1.player.Halt();
        Game1.pressUseToolButton();
      }
      else if (this.showResultsTimer > 11000)
        this.showResultsTimer = 11001;
      else if (this.showResultsTimer > 9000)
        this.showResultsTimer = 9001;
      else if (this.showResultsTimer > 7000)
        this.showResultsTimer = 7001;
      else if (this.showResultsTimer > 5000)
      {
        this.showResultsTimer = 5001;
      }
      else
      {
        if (this.showResultsTimer >= 5000 || this.showResultsTimer <= 1000)
          return;
        this.showResultsTimer = 1500;
        Game1.playSound("smallSelect");
      }
    }

    public void leftClickHeld(int x, int y)
    {
    }

    public void receiveRightClick(int x, int y, bool playSound = true)
    {
      if (!Game1.isAnyGamePadButtonBeingPressed())
        return;
      if (this.timerToStart <= 0 && this.showResultsTimer < 0 && (!this.gameDone && Game1.activeClickableMenu == null) && (!(Game1.player.CurrentTool as FishingRod).hit && !(Game1.player.CurrentTool as FishingRod).pullingOutOfWater && (!(Game1.player.CurrentTool as FishingRod).isCasting && !(Game1.player.CurrentTool as FishingRod).fishCaught)))
      {
        Game1.player.lastClick = Vector2.Zero;
        Game1.player.Halt();
        Game1.pressUseToolButton();
      }
      else if (this.showResultsTimer > 11000)
        this.showResultsTimer = 11001;
      else if (this.showResultsTimer > 9000)
        this.showResultsTimer = 9001;
      else if (this.showResultsTimer > 7000)
        this.showResultsTimer = 7001;
      else if (this.showResultsTimer > 5000)
      {
        this.showResultsTimer = 5001;
      }
      else
      {
        if (this.showResultsTimer >= 5000 || this.showResultsTimer <= 1000)
          return;
        this.showResultsTimer = 1500;
        Game1.playSound("smallSelect");
      }
    }

    public void releaseLeftClick(int x, int y)
    {
      if (this.showResultsTimer >= 0 || Game1.player.CurrentTool == null || ((Game1.player.CurrentTool as FishingRod).isCasting || Game1.activeClickableMenu != null) || !Game1.player.CurrentTool.onRelease(this.location, x, y, Game1.player))
        return;
      Game1.player.Halt();
    }

    public void releaseRightClick(int x, int y)
    {
    }

    public void receiveKeyPress(Keys k)
    {
      if (!this.gameDone && Game1.player.movementDirections.Count < 2 && (!Game1.player.UsingTool && this.timerToStart <= 0))
      {
        if (Game1.options.doesInputListContain(Game1.options.moveUpButton, k))
          Game1.player.setMoving((byte) 1);
        if (Game1.options.doesInputListContain(Game1.options.moveRightButton, k))
          Game1.player.setMoving((byte) 2);
        if (Game1.options.doesInputListContain(Game1.options.moveDownButton, k))
          Game1.player.setMoving((byte) 4);
        if (Game1.options.doesInputListContain(Game1.options.moveLeftButton, k))
          Game1.player.setMoving((byte) 8);
      }
      if (Game1.options.doesInputListContain(Game1.options.runButton, k) || Game1.isGamePadThumbstickInMotion(0.2))
        Game1.player.setRunning(true, false);
      if (!this.gameDone && k == Keys.Escape)
      {
        if (Game1.activeClickableMenu == null)
          this.gameEndTimer = 1;
        else if (Game1.activeClickableMenu is BobberBar)
          (Game1.activeClickableMenu as BobberBar).receiveKeyPress(k);
      }
      if (k != Keys.End)
        return;
      this.gameEndTimer = this.gameEndTimer - 10000;
    }

    public void receiveKeyRelease(Keys k)
    {
      if (Game1.options.doesInputListContain(Game1.options.moveUpButton, k))
        Game1.player.setMoving((byte) 33);
      if (Game1.options.doesInputListContain(Game1.options.moveRightButton, k))
        Game1.player.setMoving((byte) 34);
      if (Game1.options.doesInputListContain(Game1.options.moveDownButton, k))
        Game1.player.setMoving((byte) 36);
      if (Game1.options.doesInputListContain(Game1.options.moveLeftButton, k))
        Game1.player.setMoving((byte) 40);
      if (Game1.options.doesInputListContain(Game1.options.runButton, k))
        Game1.player.setRunning(false, false);
      if (Game1.player.movementDirections.Count != 0 || Game1.player.UsingTool)
        return;
      Game1.player.Halt();
    }

    public void draw(SpriteBatch b)
    {
      if (this.showResultsTimer < 0)
      {
        b.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
        Game1.mapDisplayDevice.BeginScene(b);
        this.location.Map.GetLayer("Back").Draw(Game1.mapDisplayDevice, Game1.viewport, Location.Origin, false, Game1.pixelZoom);
        this.location.drawWater(b);
        SpriteBatch spriteBatch = b;
        Texture2D shadowTexture = Game1.shadowTexture;
        Vector2 local = Game1.GlobalToLocal(Game1.viewport, Game1.player.position + new Vector2(32f, 24f));
        Microsoft.Xna.Framework.Rectangle? sourceRectangle = new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds);
        Color white = Color.White;
        double num1 = 0.0;
        Microsoft.Xna.Framework.Rectangle bounds = Game1.shadowTexture.Bounds;
        double x = (double) bounds.Center.X;
        bounds = Game1.shadowTexture.Bounds;
        double y = (double) bounds.Center.Y;
        Vector2 origin = new Vector2((float) x, (float) y);
        double num2 = 4.0 - (Game1.player.running || Game1.player.usingTool ? (double) Math.Abs(FarmerRenderer.featureYOffsetPerFrame[Game1.player.FarmerSprite.CurrentFrame]) * 0.800000011920929 : 0.0);
        int num3 = 0;
        double num4 = (double) Math.Max(0.0f, (float) ((double) Game1.player.getStandingY() / 10000.0 + 0.000110000000859145)) - 1.0000000116861E-07;
        spriteBatch.Draw(shadowTexture, local, sourceRectangle, white, (float) num1, origin, (float) num2, (SpriteEffects) num3, (float) num4);
        this.location.Map.GetLayer("Buildings").Draw(Game1.mapDisplayDevice, Game1.viewport, Location.Origin, false, Game1.pixelZoom);
        this.location.draw(b);
        if (Game1.player.UsingTool)
          Game1.drawTool(Game1.player);
        Game1.player.draw(b);
        this.location.Map.GetLayer("Front").Draw(Game1.mapDisplayDevice, Game1.viewport, Location.Origin, false, Game1.pixelZoom);
        if (Game1.lastCursorMotionWasMouse && !Game1.options.hardwareCursor)
          b.Draw(Game1.mouseCursors, new Vector2((float) Game1.getMouseX(), (float) Game1.getMouseY()), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 0, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
        if (Game1.activeClickableMenu != null)
          Game1.activeClickableMenu.draw(b);
        b.DrawString(Game1.dialogueFont, Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1514", (object) Utility.getMinutesSecondsStringFromMilliseconds(Math.Max(0, this.gameEndTimer))), new Vector2((float) (Game1.tileSize / 4), (float) Game1.tileSize), Color.White);
        b.DrawString(Game1.dialogueFont, Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingGame.cs.10444", (object) this.score), new Vector2((float) (Game1.tileSize / 4), (float) (Game1.tileSize / 2)), Color.White);
        foreach (TemporaryAnimatedSprite overlayTempSprite in Game1.screenOverlayTempSprites)
          overlayTempSprite.draw(b, false, 0, 0);
        b.End();
      }
      else
      {
        b.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
        Vector2 position = new Vector2((float) (Game1.graphics.GraphicsDevice.Viewport.Width / 2 - Game1.tileSize * 2), (float) (Game1.graphics.GraphicsDevice.Viewport.Height / 2 - Game1.tileSize));
        if (this.showResultsTimer <= 11000)
          Game1.drawWithBorder(Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingGame.cs.10444", (object) this.score), Game1.textColor, this.showResultsTimer > 7000 || this.perfectionBonus <= 0 ? Color.White : Color.Lime, position);
        if (this.showResultsTimer <= 9000)
        {
          position.Y += (float) (Game1.tileSize * 3 / 4);
          Game1.drawWithBorder(Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingGame.cs.12010", (object) this.fishCaught), Game1.textColor, Color.White, position);
        }
        if (this.showResultsTimer <= 7000)
        {
          position.Y += (float) (Game1.tileSize * 3 / 4);
          if (this.perfectionBonus > 1)
            Game1.drawWithBorder(Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingGame.cs.12011", (object) this.perfectionBonus), Game1.textColor, Color.Yellow, position);
          else
            Game1.drawWithBorder(Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingGame.cs.12012"), Game1.textColor, Color.Red, position);
        }
        if (this.showResultsTimer <= 5000)
        {
          position.Y += (float) Game1.tileSize;
          if (this.starTokensWon > 0)
          {
            float num = Math.Min(1f, (float) (this.showResultsTimer - 2000) / 4000f);
            Game1.drawWithBorder(Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingGame.cs.12013", (object) this.starTokensWon), Game1.textColor * 0.2f * num, Color.SkyBlue * 0.3f * num, position + new Vector2((float) Game1.random.Next(-1, 2), (float) Game1.random.Next(-1, 2)) * (float) Game1.pixelZoom * 2f, 0.0f, 1f, 1f);
            Game1.drawWithBorder(Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingGame.cs.12013", (object) this.starTokensWon), Game1.textColor * 0.2f * num, Color.SkyBlue * 0.3f * num, position + new Vector2((float) Game1.random.Next(-1, 2), (float) Game1.random.Next(-1, 2)) * (float) Game1.pixelZoom * 2f, 0.0f, 1f, 1f);
            Game1.drawWithBorder(Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingGame.cs.12013", (object) this.starTokensWon), Game1.textColor * 0.2f * num, Color.SkyBlue * 0.3f * num, position + new Vector2((float) Game1.random.Next(-1, 2), (float) Game1.random.Next(-1, 2)) * (float) Game1.pixelZoom * 2f, 0.0f, 1f, 1f);
            Game1.drawWithBorder(Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingGame.cs.12013", (object) this.starTokensWon), Game1.textColor, Color.SkyBlue, position, 0.0f, 1f, 1f);
          }
          else
            Game1.drawWithBorder(Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingGame.cs.12021"), Game1.textColor, Color.Red, position);
        }
        if (this.showResultsTimer <= 1000)
          b.Draw(Game1.fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), Color.Black * (float) (1.0 - (double) this.showResultsTimer / 1000.0));
        b.Draw(Game1.fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle(Game1.tileSize / 4, Game1.tileSize / 4, Game1.tileSize * 2 + (Game1.player.festivalScore > 999 ? Game1.tileSize / 4 : 0), Game1.tileSize), Color.Black * 0.75f);
        b.Draw(Game1.mouseCursors, new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(338, 400, 8, 8)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1f);
        Game1.drawWithBorder(string.Concat((object) Game1.player.festivalScore), Color.Black, Color.White, new Vector2((float) (Game1.tileSize / 2 + 10 * Game1.pixelZoom), (float) (Game1.tileSize / 3 + Game1.pixelZoom * 2)), 0.0f, 1f, 1f, false);
        b.End();
      }
    }

    public static void startMe()
    {
      Game1.currentMinigame = (IMinigame) new FishingGame();
    }

    public void changeScreenSize()
    {
      Game1.viewport.X = this.location.Map.Layers[0].LayerWidth * Game1.tileSize / 2 - Game1.graphics.GraphicsDevice.Viewport.Width / 2;
      Game1.viewport.Y = this.location.Map.Layers[0].LayerHeight * Game1.tileSize / 2 - Game1.graphics.GraphicsDevice.Viewport.Height / 2;
    }

    public void unload()
    {
      (Game1.player.CurrentTool as FishingRod).castingEndFunction(-1);
      (Game1.player.CurrentTool as FishingRod).doneFishing(Game1.player, false);
      Game1.player.addItemToInventory(this.tempItemStash, 0);
      Game1.player.currentLocation = Game1.currentLocation;
      Game1.player.completelyStopAnimatingOrDoingAction();
      Game1.player.forceCanMove();
      Game1.player.faceDirection(2);
      this.content.Unload();
      this.content.Dispose();
      this.content = (LocalizedContentManager) null;
    }

    public void receiveEventPoke(int data)
    {
    }

    public string minigameId()
    {
      return nameof (FishingGame);
    }
  }
}
