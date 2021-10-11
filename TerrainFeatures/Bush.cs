// Decompiled with JetBrains decompiler
// Type: StardewValley.TerrainFeatures.Bush
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Objects;
using StardewValley.Tools;
using System;

namespace StardewValley.TerrainFeatures
{
  public class Bush : LargeTerrainFeature
  {
    public static Rectangle treeTopSourceRect = new Rectangle(0, 0, 48, 96);
    public static Rectangle stumpSourceRect = new Rectangle(32, 96, 16, 32);
    public static Rectangle shadowSourceRect = new Rectangle(663, 1011, 41, 30);
    public bool drawShadow = true;
    private float alpha = 1f;
    public const float shakeRate = 0.01570796f;
    public const float shakeDecayRate = 0.003067962f;
    public const int smallBush = 0;
    public const int mediumBush = 1;
    public const int largeBush = 2;
    public static Texture2D texture;
    public int size;
    public int tileSheetOffset;
    public float health;
    public bool flipped;
    public bool townBush;
    private bool shakeLeft;
    private float shakeRotation;
    private float maxShake;
    private long lastPlayerToHit;
    private float shakeTimer;
    private Rectangle sourceRect;

    public Bush()
    {
    }

    public Bush(Vector2 tileLocation, int size, GameLocation location)
    {
      this.tilePosition = tileLocation;
      this.size = size;
      if (size == 0)
        this.tileSheetOffset = Game1.random.Next(2);
      if (location is Town && (double) tileLocation.X % 5.0 != 0.0)
        this.townBush = true;
      if (location.map.GetLayer("Front").Tiles[(int) tileLocation.X, (int) tileLocation.Y] != null)
        this.drawShadow = false;
      this.loadSprite();
      this.flipped = Game1.random.NextDouble() < 0.5;
    }

    public void setUpSourceRect()
    {
      int seasonNumber = Utility.getSeasonNumber(Game1.currentSeason);
      if (this.size == 0)
        this.sourceRect = new Rectangle(seasonNumber * 16 * 2 + this.tileSheetOffset * 16, 224, 16, 32);
      else if (this.size == 1)
      {
        if (this.townBush)
          this.sourceRect = new Rectangle(seasonNumber * 16 * 2, 96, 32, 32);
        else
          this.sourceRect = new Rectangle((seasonNumber * 16 * 4 + this.tileSheetOffset * 16 * 2) % Bush.texture.Bounds.Width, (seasonNumber * 16 * 4 + this.tileSheetOffset * 16 * 2) / Bush.texture.Bounds.Width * 3 * 16, 32, 48);
      }
      else
      {
        if (this.size != 2)
          return;
        if (this.townBush && (seasonNumber == 0 || seasonNumber == 1))
        {
          this.sourceRect = new Rectangle(48, 176, 48, 48);
        }
        else
        {
          switch (seasonNumber)
          {
            case 0:
            case 1:
              this.sourceRect = new Rectangle(0, 128, 48, 48);
              break;
            case 2:
              this.sourceRect = new Rectangle(48, 128, 48, 48);
              break;
            case 3:
              this.sourceRect = new Rectangle(0, 176, 48, 48);
              break;
          }
        }
      }
    }

    public bool inBloom(string season, int dayOfMonth)
    {
      if (season.Equals("spring"))
      {
        if (dayOfMonth > 14 && dayOfMonth < 19)
          return true;
      }
      else if (season.Equals("fall") && dayOfMonth > 7 && dayOfMonth < 12)
        return true;
      return false;
    }

    public override bool isActionable()
    {
      return true;
    }

    public override void loadSprite()
    {
      if (Bush.texture == null)
      {
        try
        {
          Bush.texture = Game1.content.Load<Texture2D>("TileSheets\\bushes");
        }
        catch (Exception ex)
        {
        }
      }
      if (this.size == 1 && this.tileSheetOffset == 0 && (new Random((int) Game1.stats.DaysPlayed + (int) Game1.uniqueIDForThisGame + (int) this.tilePosition.X + (int) this.tilePosition.Y * 777).NextDouble() < 0.5 && this.inBloom(Game1.currentSeason, Game1.dayOfMonth)))
        this.tileSheetOffset = 1;
      else if (!Game1.currentSeason.Equals("summer") && !this.inBloom(Game1.currentSeason, Game1.dayOfMonth))
        this.tileSheetOffset = 0;
      this.setUpSourceRect();
    }

    public override Rectangle getBoundingBox(Vector2 tileLocation)
    {
      switch (this.size)
      {
        case 0:
          return new Rectangle((int) tileLocation.X * Game1.tileSize, (int) tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
        case 1:
          return new Rectangle((int) tileLocation.X * Game1.tileSize, (int) tileLocation.Y * Game1.tileSize, Game1.tileSize * 2, Game1.tileSize);
        case 2:
          return new Rectangle((int) tileLocation.X * Game1.tileSize, (int) tileLocation.Y * Game1.tileSize, Game1.tileSize * 3, Game1.tileSize);
        default:
          return Rectangle.Empty;
      }
    }

    public override bool performUseAction(Vector2 tileLocation)
    {
      if ((double) this.maxShake == 0.0)
        Game1.playSound("leafrustle");
      this.shake(tileLocation, false);
      return true;
    }

    public override bool tickUpdate(GameTime time, Vector2 tileLocation)
    {
      if ((double) this.shakeTimer > 0.0)
        this.shakeTimer = this.shakeTimer - (float) time.ElapsedGameTime.Milliseconds;
      this.alpha = Math.Min(1f, this.alpha + 0.05f);
      if ((double) this.maxShake > 0.0)
      {
        if (this.shakeLeft)
        {
          this.shakeRotation = this.shakeRotation - (float) Math.PI / 200f;
          if ((double) this.shakeRotation <= -(double) this.maxShake)
            this.shakeLeft = false;
        }
        else
        {
          this.shakeRotation = this.shakeRotation + (float) Math.PI / 200f;
          if ((double) this.shakeRotation >= (double) this.maxShake)
            this.shakeLeft = true;
        }
      }
      if ((double) this.maxShake > 0.0)
        this.maxShake = Math.Max(0.0f, this.maxShake - 0.003067962f);
      return false;
    }

    private void shake(Vector2 tileLocation, bool doEvenIfStillShaking)
    {
      if (!((double) this.maxShake == 0.0 | doEvenIfStillShaking))
        return;
      this.shakeLeft = (double) Game1.player.getTileLocation().X > (double) tileLocation.X || (double) Game1.player.getTileLocation().X == (double) tileLocation.X && Game1.random.NextDouble() < 0.5;
      this.maxShake = (float) Math.PI / 128f;
      if (!this.townBush && this.tileSheetOffset == 1 && this.inBloom(Game1.currentSeason, Game1.dayOfMonth))
      {
        int parentSheetIndex = -1;
        string currentSeason = Game1.currentSeason;
        if (!(currentSeason == "spring"))
        {
          if (currentSeason == "fall")
            parentSheetIndex = 410;
        }
        else
          parentSheetIndex = 296;
        if (parentSheetIndex == -1)
          return;
        this.tileSheetOffset = 0;
        this.setUpSourceRect();
        int num = new Random((int) tileLocation.X + (int) tileLocation.Y * 5000 + (int) Game1.uniqueIDForThisGame + (int) Game1.stats.DaysPlayed).Next(1, 2) + Game1.player.ForagingLevel / 4;
        for (int index = 0; index < num; ++index)
          Game1.createItemDebris((Item) new StardewValley.Object(parentSheetIndex, 1, false, -1, Game1.player.professions.Contains(16) ? 4 : 0), Utility.PointToVector2(this.getBoundingBox().Center), Game1.random.Next(1, 4), (GameLocation) null);
        DelayedAction.playSoundAfterDelay("leafrustle", 100);
      }
      else
      {
        if ((double) tileLocation.X != 20.0 || (double) tileLocation.Y != 8.0 || (Game1.dayOfMonth != 28 || Game1.timeOfDay != 1200) || Game1.player.mailReceived.Contains("junimoPlush"))
          return;
        Game1.player.addItemByMenuIfNecessaryElseHoldUp((Item) new Furniture(1733, Vector2.Zero), new ItemGrabMenu.behaviorOnItemSelect(this.junimoPlushCallback));
      }
    }

    public void junimoPlushCallback(Item item, Farmer who)
    {
      if (item == null || !(item is Furniture) || ((item as Furniture).parentSheetIndex != 1733 || who == null))
        return;
      who.mailReceived.Add("junimoPlush");
    }

    public override bool isPassable(Character c = null)
    {
      return false;
    }

    public override void dayUpdate(GameLocation environment, Vector2 tileLocation)
    {
      if (this.size == 1 && this.tileSheetOffset == 0 && (Game1.random.NextDouble() < 0.2 && this.inBloom(Game1.currentSeason, Game1.dayOfMonth)))
      {
        this.tileSheetOffset = 1;
        this.setUpSourceRect();
      }
      else if (!Game1.currentSeason.Equals("summer") && !this.inBloom(Game1.currentSeason, Game1.dayOfMonth))
      {
        this.tileSheetOffset = 0;
        this.setUpSourceRect();
      }
      this.health = 0.0f;
    }

    public override bool seasonUpdate(bool onLoad)
    {
      this.tileSheetOffset = this.size != 1 || !Game1.currentSeason.Equals("summer") || Game1.random.NextDouble() >= 0.5 ? 0 : 1;
      this.loadSprite();
      return false;
    }

    public override bool performToolAction(Tool t, int explosion, Vector2 tileLocation, GameLocation location = null)
    {
      if (location == null)
        location = Game1.currentLocation;
      if (explosion > 0)
      {
        this.shake(tileLocation, true);
        return false;
      }
      if (t != null && t is Axe && this.isDestroyable(location, tileLocation))
      {
        Game1.playSound("leafrustle");
        this.shake(tileLocation, true);
        if ((t as Axe).upgradeLevel >= 1)
        {
          this.health = this.health - (float) (t as Axe).upgradeLevel / 5f;
          if ((double) this.health <= -1.0)
          {
            Game1.playSound("treethud");
            DelayedAction.playSoundAfterDelay("leafrustle", 100);
            Color color = Color.Green;
            string currentSeason = Game1.currentSeason;
            if (!(currentSeason == "spring"))
            {
              if (!(currentSeason == "summer"))
              {
                if (!(currentSeason == "fall"))
                {
                  if (currentSeason == "winter")
                    color = Color.Cyan;
                }
                else
                  color = Color.IndianRed;
              }
              else
                color = Color.ForestGreen;
            }
            else
              color = Color.Green;
            for (int index1 = 0; index1 <= this.size; ++index1)
            {
              for (int index2 = 0; index2 < 12; ++index2)
              {
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Rectangle(355, 1200 + (Game1.IsFall ? 16 : (Game1.IsWinter ? -16 : 0)), 16, 16), Utility.getRandomPositionInThisRectangle(this.getBoundingBox(), Game1.random) - new Vector2(0.0f, (float) Game1.random.Next(Game1.tileSize)), false, 0.01f, Game1.IsWinter ? Color.Cyan : Color.White)
                {
                  motion = new Vector2((float) Game1.random.Next(-10, 11) / 10f, (float) -Game1.random.Next(5, 7)),
                  acceleration = new Vector2(0.0f, (float) Game1.random.Next(13, 17) / 100f),
                  accelerationChange = new Vector2(0.0f, -1f / 1000f),
                  scale = (float) Game1.pixelZoom,
                  layerDepth = (float) ((double) tileLocation.Y * (double) Game1.tileSize / 10000.0),
                  animationLength = 11,
                  totalNumberOfLoops = 99,
                  interval = (float) Game1.random.Next(20, 90),
                  delayBeforeAnimationStart = (index1 + 1) * index2 * 20
                });
                if (index2 % 6 == 0)
                {
                  location.TemporarySprites.Add(new TemporaryAnimatedSprite(50, Utility.getRandomPositionInThisRectangle(this.getBoundingBox(), Game1.random) - new Vector2((float) (Game1.tileSize / 2), (float) Game1.random.Next(Game1.tileSize / 2, Game1.tileSize)), color, 8, false, 100f, 0, -1, -1f, -1, 0));
                  location.TemporarySprites.Add(new TemporaryAnimatedSprite(12, Utility.getRandomPositionInThisRectangle(this.getBoundingBox(), Game1.random) - new Vector2((float) (Game1.tileSize / 2), (float) Game1.random.Next(Game1.tileSize / 2, Game1.tileSize)), Color.White, 8, false, 100f, 0, -1, -1f, -1, 0));
                }
              }
            }
            return true;
          }
          Game1.playSound("axchop");
        }
      }
      return false;
    }

    public bool isDestroyable(GameLocation location, Vector2 tile)
    {
      if (location != null && location is Farm)
      {
        switch (Game1.whichFarm)
        {
          case 1:
            return new Rectangle(32, 11, 11, 25).Contains((int) tile.X, (int) tile.Y);
          case 2:
            if ((double) tile.X == 13.0 && (double) tile.Y == 35.0 || (double) tile.X == 37.0 && (double) tile.Y == 9.0)
              return true;
            return new Rectangle(43, 11, 34, 50).Contains((int) tile.X, (int) tile.Y);
          case 3:
            return new Rectangle(24, 56, 10, 8).Contains((int) tile.X, (int) tile.Y);
        }
      }
      return false;
    }

    public override void drawInMenu(SpriteBatch spriteBatch, Vector2 positionOnScreen, Vector2 tileLocation, float scale, float layerDepth)
    {
      layerDepth += positionOnScreen.X / 100000f;
      spriteBatch.Draw(Bush.texture, positionOnScreen + new Vector2(0.0f, (float) -Game1.tileSize * scale), new Rectangle?(new Rectangle(32, 96, 16, 32)), Color.White, 0.0f, Vector2.Zero, scale, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth + (float) (((double) positionOnScreen.Y + (double) (7 * Game1.tileSize) * (double) scale - 1.0) / 20000.0));
    }

    public override void performPlayerEntryAction(Vector2 tileLocation)
    {
      base.performPlayerEntryAction(tileLocation);
      if (Game1.currentSeason.Equals("winter") || Game1.isRaining || !Game1.isDarkOut() || Game1.random.NextDouble() >= (Game1.currentSeason.Equals("summer") ? 0.08 : 0.04))
        return;
      AmbientLocationSounds.addSound(tileLocation, 3);
      Game1.debugOutput = Game1.debugOutput + "  added cricket at " + tileLocation.ToString();
    }

    public override void draw(SpriteBatch spriteBatch, Vector2 tileLocation)
    {
      Rectangle rectangle;
      if (this.drawShadow)
      {
        if (this.size > 0)
        {
          spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((tileLocation.X + (this.size == 1 ? 0.5f : 1f)) * (float) Game1.tileSize - (float) (Game1.tileSize * 4 / 5), tileLocation.Y * (float) Game1.tileSize - (float) (Game1.tileSize / 4))), new Rectangle?(Bush.shadowSourceRect), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, this.flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 1E-06f);
        }
        else
        {
          SpriteBatch spriteBatch1 = spriteBatch;
          Texture2D shadowTexture = Game1.shadowTexture;
          Vector2 local = Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float) Game1.tileSize + (float) (Game1.tileSize / 2), tileLocation.Y * (float) Game1.tileSize + (float) Game1.tileSize - (float) Game1.pixelZoom));
          Rectangle? sourceRectangle = new Rectangle?(Game1.shadowTexture.Bounds);
          Color color = Color.White * this.alpha;
          double num1 = 0.0;
          double x = (double) Game1.shadowTexture.Bounds.Center.X;
          rectangle = Game1.shadowTexture.Bounds;
          double y = (double) rectangle.Center.Y;
          Vector2 origin = new Vector2((float) x, (float) y);
          double num2 = 4.0;
          int num3 = 0;
          double num4 = 9.99999997475243E-07;
          spriteBatch1.Draw(shadowTexture, local, sourceRectangle, color, (float) num1, origin, (float) num2, (SpriteEffects) num3, (float) num4);
        }
      }
      SpriteBatch spriteBatch2 = spriteBatch;
      Texture2D texture = Bush.texture;
      Vector2 local1 = Game1.GlobalToLocal(Game1.viewport, new Vector2(tileLocation.X * (float) Game1.tileSize + (float) ((this.size + 1) * Game1.tileSize / 2), (float) (((double) tileLocation.Y + 1.0) * (double) Game1.tileSize - (this.size <= 0 || this.townBush && this.size == 1 ? 0.0 : (double) Game1.tileSize))));
      Rectangle? sourceRectangle1 = new Rectangle?(this.sourceRect);
      Color color1 = Color.White * this.alpha;
      double shakeRotation = (double) this.shakeRotation;
      Vector2 origin1 = new Vector2((float) ((this.size + 1) * 16 / 2), 32f);
      double pixelZoom = (double) Game1.pixelZoom;
      int num5 = this.flipped ? 1 : 0;
      rectangle = this.getBoundingBox(tileLocation);
      double num6 = (double) (rectangle.Center.Y + 48) / 10000.0 - (double) tileLocation.X / 1000000.0;
      spriteBatch2.Draw(texture, local1, sourceRectangle1, color1, (float) shakeRotation, origin1, (float) pixelZoom, (SpriteEffects) num5, (float) num6);
    }
  }
}
