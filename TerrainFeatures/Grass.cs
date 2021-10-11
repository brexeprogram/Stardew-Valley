// Decompiled with JetBrains decompiler
// Type: StardewValley.TerrainFeatures.Grass
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Tools;
using System;

namespace StardewValley.TerrainFeatures
{
  public class Grass : TerrainFeature
  {
    private int[] whichWeed = new int[4];
    private int[] offset1 = new int[4];
    private int[] offset2 = new int[4];
    private int[] offset3 = new int[4];
    private int[] offset4 = new int[4];
    private bool[] flip = new bool[4];
    private double[] shakeRandom = new double[4];
    public const float defaultShakeRate = 0.03926991f;
    public const float maximumShake = 0.3926991f;
    public const float shakeDecayRate = 0.008975979f;
    public const byte springGrass = 1;
    public const byte caveGrass = 2;
    public const byte frostGrass = 3;
    public const byte lavaGrass = 4;
    public static Cue grassSound;
    protected Texture2D texture;
    public byte grassType;
    private bool shakeLeft;
    protected float shakeRotation;
    protected float maxShake;
    protected float shakeRate;
    public int numberOfWeeds;
    public int grassSourceOffset;

    public Grass()
    {
    }

    public Grass(int which, int numberOfWeeds)
    {
      this.grassType = (byte) which;
      this.loadSprite();
      this.numberOfWeeds = numberOfWeeds;
    }

    public override bool isPassable(Character c = null)
    {
      return true;
    }

    public override void loadSprite()
    {
      try
      {
        if (Game1.soundBank != null)
          Grass.grassSound = Game1.soundBank.GetCue("grassyStep");
        this.texture = Game1.content.Load<Texture2D>("TerrainFeatures\\grass");
        if ((int) this.grassType == 1)
        {
          string currentSeason = Game1.currentSeason;
          if (!(currentSeason == "spring"))
          {
            if (!(currentSeason == "summer"))
            {
              if (!(currentSeason == "fall"))
                return;
              this.grassSourceOffset = 40;
            }
            else
              this.grassSourceOffset = 20;
          }
          else
            this.grassSourceOffset = 0;
        }
        else if ((int) this.grassType == 2)
          this.grassSourceOffset = 60;
        else if ((int) this.grassType == 3)
        {
          this.grassSourceOffset = 80;
        }
        else
        {
          if ((int) this.grassType != 4)
            return;
          this.grassSourceOffset = 100;
        }
      }
      catch (Exception ex)
      {
      }
    }

    public override Rectangle getBoundingBox(Vector2 tileLocation)
    {
      return new Rectangle((int) ((double) tileLocation.X * (double) Game1.tileSize), (int) ((double) tileLocation.Y * (double) Game1.tileSize), Game1.tileSize, Game1.tileSize);
    }

    public override void doCollisionAction(Rectangle positionOfCollider, int speedOfCollision, Vector2 tileLocation, Character who, GameLocation location)
    {
      if (speedOfCollision > 0 && (double) this.maxShake == 0.0 && positionOfCollider.Intersects(this.getBoundingBox(tileLocation)))
      {
        if ((who == null || who.GetType() != typeof (FarmAnimal)) && (Grass.grassSound != null && !Grass.grassSound.IsPlaying) && (Utility.isOnScreen(new Point((int) tileLocation.X, (int) tileLocation.Y), 2, location) && Game1.soundBank != null))
        {
          Grass.grassSound = Game1.soundBank.GetCue("grassyStep");
          Grass.grassSound.Play();
        }
        this.shake(0.3926991f / (float) ((5 + Game1.player.addedSpeed) / speedOfCollision), (float) Math.PI / 80f / (float) ((5 + Game1.player.addedSpeed) / speedOfCollision), (double) positionOfCollider.Center.X > (double) tileLocation.X * (double) Game1.tileSize + (double) (Game1.tileSize / 2));
      }
      if (who is Farmer && Game1.player.CurrentTool != null && (Game1.player.CurrentTool is MeleeWeapon && ((MeleeWeapon) Game1.player.CurrentTool).isOnSpecial) && (((MeleeWeapon) Game1.player.CurrentTool).type == 0 && (double) Math.Abs(this.shakeRotation) < 1.0 / 1000.0 && this.performToolAction(Game1.player.CurrentTool, -1, tileLocation, (GameLocation) null)))
        Game1.currentLocation.terrainFeatures.Remove(tileLocation);
      if (!(who is Farmer))
        return;
      (who as Farmer).temporarySpeedBuff = -1f;
    }

    public bool reduceBy(int number, Vector2 tileLocation, bool showDebris)
    {
      this.numberOfWeeds = this.numberOfWeeds - number;
      if (showDebris)
        Game1.createRadialDebris(Game1.currentLocation, this.texture, new Rectangle(2, 8, 8, 8), 1, ((int) tileLocation.X + 1) * Game1.tileSize, ((int) tileLocation.Y + 1) * Game1.tileSize, Game1.random.Next(6, 14), (int) tileLocation.Y + 1, Color.White, (float) Game1.pixelZoom);
      return this.numberOfWeeds <= 0;
    }

    protected void shake(float shake, float rate, bool left)
    {
      this.maxShake = shake;
      this.shakeRate = rate;
      this.shakeRotation = 0.0f;
      this.shakeLeft = left;
    }

    public override bool tickUpdate(GameTime time, Vector2 tileLocation)
    {
      if (this.shakeRandom[0] == 0.0)
        this.setUpRandom(tileLocation);
      if ((double) this.maxShake > 0.0)
      {
        if (this.shakeLeft)
        {
          this.shakeRotation = this.shakeRotation - this.shakeRate;
          if ((double) Math.Abs(this.shakeRotation) >= (double) this.maxShake)
            this.shakeLeft = false;
        }
        else
        {
          this.shakeRotation = this.shakeRotation + this.shakeRate;
          if ((double) this.shakeRotation >= (double) this.maxShake)
          {
            this.shakeLeft = true;
            this.shakeRotation = this.shakeRotation - this.shakeRate;
          }
        }
        this.maxShake = Math.Max(0.0f, this.maxShake - (float) Math.PI / 350f);
      }
      else
        this.shakeRotation = this.shakeRotation / 2f;
      return false;
    }

    public override void dayUpdate(GameLocation environment, Vector2 tileLocation)
    {
      if ((int) this.grassType == 1 && !Game1.currentSeason.Equals("winter") && this.numberOfWeeds < 4)
      {
        this.numberOfWeeds = this.numberOfWeeds + Game1.random.Next(1, 4);
        this.numberOfWeeds = Math.Min(this.numberOfWeeds, 4);
      }
      this.setUpRandom(tileLocation);
    }

    public void setUpRandom(Vector2 tileLocation)
    {
      Random random = new Random((int) Game1.uniqueIDForThisGame + (int) Game1.stats.DaysPlayed / 28 + (int) tileLocation.X * 7 + (int) tileLocation.Y * 11);
      for (int index = 0; index < 4; ++index)
      {
        this.whichWeed[index] = random.Next(3);
        this.offset1[index] = random.Next(-2, 3);
        this.offset2[index] = random.Next(-2, 3);
        this.offset3[index] = random.Next(-2, 3);
        this.offset4[index] = random.Next(-2, 3);
        this.flip[index] = random.NextDouble() < 0.5;
        this.shakeRandom[index] = random.NextDouble();
      }
    }

    public override bool seasonUpdate(bool onLoad)
    {
      if ((int) this.grassType == 1 && Game1.currentSeason.Equals("winter"))
        return true;
      if ((int) this.grassType == 1)
        this.loadSprite();
      return false;
    }

    public override bool performToolAction(Tool t, int explosion, Vector2 tileLocation, GameLocation location = null)
    {
      if (location == null)
        location = Game1.currentLocation;
      if (t != null && t is MeleeWeapon && ((MeleeWeapon) t).type != 2 || explosion > 0)
      {
        if (t != null && (t as MeleeWeapon).type != 1)
          DelayedAction.playSoundAfterDelay("daggerswipe", 50);
        else if (location.Equals((object) Game1.currentLocation))
          Game1.playSound("swordswipe");
        this.shake(3f * (float) Math.PI / 32f, (float) Math.PI / 40f, Game1.random.NextDouble() < 0.5);
        this.numberOfWeeds = this.numberOfWeeds - (explosion <= 0 ? 1 : Math.Max(1, explosion + 2 - Game1.recentMultiplayerRandom.Next(2)));
        Color color = Color.Green;
        switch (this.grassType)
        {
          case 1:
            string currentSeason = Game1.currentSeason;
            if (!(currentSeason == "spring"))
            {
              if (!(currentSeason == "summer"))
              {
                if (currentSeason == "fall")
                {
                  color = new Color(219, 102, 58);
                  break;
                }
                break;
              }
              color = new Color(110, 190, 24);
              break;
            }
            color = new Color(60, 180, 58);
            break;
          case 2:
            color = new Color(148, 146, 71);
            break;
          case 3:
            color = new Color(216, 240, (int) byte.MaxValue);
            break;
          case 4:
            color = new Color(165, 93, 58);
            break;
        }
        location.temporarySprites.Add(new TemporaryAnimatedSprite(28, tileLocation * (float) Game1.tileSize + new Vector2((float) Game1.random.Next(-Game1.pixelZoom * 4, Game1.pixelZoom * 4), (float) Game1.random.Next(-Game1.pixelZoom * 4, Game1.pixelZoom * 4)), color, 8, Game1.random.NextDouble() < 0.5, (float) Game1.random.Next(60, 100), 0, -1, -1f, -1, 0));
        if (this.numberOfWeeds <= 0)
        {
          if ((int) this.grassType != 1)
          {
            Random random = Game1.IsMultiplayer ? Game1.recentMultiplayerRandom : new Random((int) ((double) Game1.uniqueIDForThisGame + (double) tileLocation.X * 1000.0 + (double) tileLocation.Y * 11.0 + (double) Game1.mine.mineLevel + (double) Game1.player.timesReachedMineBottom));
            if (random.NextDouble() < 0.005)
              Game1.createObjectDebris(114, (int) tileLocation.X, (int) tileLocation.Y, -1, 0, 1f, (GameLocation) null);
            else if (random.NextDouble() < 0.01)
              Game1.createDebris(4, (int) tileLocation.X, (int) tileLocation.Y, random.Next(1, 2), (GameLocation) null);
            else if (random.NextDouble() < 0.02)
              Game1.createDebris(92, (int) tileLocation.X, (int) tileLocation.Y, random.Next(2, 4), (GameLocation) null);
          }
          else if (t is MeleeWeapon && (t.Name.Contains("Scythe") || t.parentSheetIndex == 47) && ((Game1.IsMultiplayer ? Game1.recentMultiplayerRandom : new Random((int) ((double) Game1.uniqueIDForThisGame + (double) tileLocation.X * 1000.0 + (double) tileLocation.Y * 11.0))).NextDouble() < 0.5 && (Game1.getLocationFromName("Farm") as Farm).tryToAddHay(1) == 0))
          {
            t.getLastFarmerToUse().currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.objectSpriteSheet, Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, 178, 16, 16), 750f, 1, 0, t.getLastFarmerToUse().position - new Vector2(0.0f, (float) (Game1.tileSize * 2)), false, false, t.getLastFarmerToUse().position.Y / 10000f, 0.005f, Color.White, (float) Game1.pixelZoom, -0.005f, 0.0f, 0.0f, false)
            {
              motion = {
                Y = -1f
              },
              layerDepth = (float) (1.0 - (double) Game1.random.Next(100) / 10000.0),
              delayBeforeAnimationStart = Game1.random.Next(350)
            });
            Game1.addHUDMessage(new HUDMessage("Hay", 1, true, Color.LightGoldenrodYellow, (Item) new StardewValley.Object(178, 1, false, -1, 0)));
          }
          return true;
        }
      }
      return false;
    }

    public override void drawInMenu(SpriteBatch spriteBatch, Vector2 positionOnScreen, Vector2 tileLocation, float scale, float layerDepth)
    {
      Random random = new Random((int) Game1.uniqueIDForThisGame + (int) Game1.stats.DaysPlayed / 28 + (int) positionOnScreen.X * 7 + (int) positionOnScreen.Y * 11);
      for (int index = 0; index < this.numberOfWeeds; ++index)
      {
        int num = random.Next(3);
        Vector2 position = index != 4 ? tileLocation * (float) Game1.tileSize + new Vector2((float) (index % 2 * Game1.tileSize / 2 + random.Next(-2, 2) * Game1.pixelZoom - Game1.pixelZoom) + 7.5f * (float) Game1.pixelZoom, (float) (index / 2 * Game1.tileSize / 2 + random.Next(-2, 2) * Game1.pixelZoom + 10 * Game1.pixelZoom)) : tileLocation * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 4 + random.Next(-2, 2) * Game1.pixelZoom - Game1.pixelZoom) + 7.5f * (float) Game1.pixelZoom, (float) (Game1.tileSize / 4 + random.Next(-2, 2) * Game1.pixelZoom + 10 * Game1.pixelZoom));
        spriteBatch.Draw(this.texture, position, new Rectangle?(new Rectangle(num * 15, this.grassSourceOffset, 15, 20)), Color.White, this.shakeRotation / (float) (random.NextDouble() + 1.0), Vector2.Zero, scale, SpriteEffects.None, layerDepth + (float) (((double) (Game1.tileSize / 2) * (double) scale + 300.0) / 20000.0));
      }
    }

    public override void draw(SpriteBatch spriteBatch, Vector2 tileLocation)
    {
      for (int index = 0; index < this.numberOfWeeds; ++index)
      {
        Vector2 globalPosition = index != 4 ? tileLocation * (float) Game1.tileSize + new Vector2((float) (index % 2 * Game1.tileSize / 2 + this.offset3[index] * Game1.pixelZoom - Game1.pixelZoom) + 7.5f * (float) Game1.pixelZoom, (float) (index / 2 * Game1.tileSize / 2 + this.offset4[index] * Game1.pixelZoom + 10 * Game1.pixelZoom)) : tileLocation * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 4 + this.offset1[index] * Game1.pixelZoom - Game1.pixelZoom) + 7.5f * (float) Game1.pixelZoom, (float) (Game1.tileSize / 4 + this.offset2[index] * Game1.pixelZoom + 10 * Game1.pixelZoom));
        spriteBatch.Draw(this.texture, Game1.GlobalToLocal(Game1.viewport, globalPosition), new Rectangle?(new Rectangle(this.whichWeed[index] * 15, this.grassSourceOffset, 15, 20)), Color.White, this.shakeRotation / (float) (this.shakeRandom[index] + 1.0), new Vector2(7.5f, 17.5f), (float) Game1.pixelZoom, this.flip[index] ? SpriteEffects.FlipHorizontally : SpriteEffects.None, (float) (((double) globalPosition.Y + (double) (Game1.tileSize / 4) - (double) (Game1.pixelZoom * 5)) / 10000.0 + (double) globalPosition.X / 10000000.0));
      }
    }
  }
}
