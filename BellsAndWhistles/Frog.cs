// Decompiled with JetBrains decompiler
// Type: StardewValley.BellsAndWhistles.Frog
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StardewValley.BellsAndWhistles
{
  public class Frog : Critter
  {
    private int characterCheckTimer = 200;
    private float alpha = 1f;
    private bool waterLeaper;
    private bool leapingIntoWater;
    private bool splash;
    private int beforeFadeTimer;

    public Frog(Vector2 position, bool waterLeaper = false, bool forceFlip = false)
    {
      this.waterLeaper = waterLeaper;
      this.position = position * (float) Game1.tileSize;
      this.sprite = new AnimatedSprite(Critter.critterTexture, waterLeaper ? 300 : 280, 16, 16);
      this.sprite.loop = true;
      if (!this.flip & forceFlip)
        this.flip = true;
      if (waterLeaper)
      {
        this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
        {
          new FarmerSprite.AnimationFrame(300, 600),
          new FarmerSprite.AnimationFrame(304, 100),
          new FarmerSprite.AnimationFrame(305, 100),
          new FarmerSprite.AnimationFrame(306, 300),
          new FarmerSprite.AnimationFrame(305, 100),
          new FarmerSprite.AnimationFrame(304, 100)
        });
      }
      else
      {
        this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
        {
          new FarmerSprite.AnimationFrame(280, 60),
          new FarmerSprite.AnimationFrame(281, 70),
          new FarmerSprite.AnimationFrame(282, 140),
          new FarmerSprite.AnimationFrame(283, 90)
        });
        this.beforeFadeTimer = 1000;
        this.flip = (double) this.position.X + (double) Game1.pixelZoom < (double) Game1.player.position.X;
      }
      this.startingPosition = position;
    }

    public void startSplash(Farmer who)
    {
      this.splash = true;
    }

    public override bool update(GameTime time, GameLocation environment)
    {
      if (this.waterLeaper)
      {
        if (!this.leapingIntoWater)
        {
          this.characterCheckTimer = this.characterCheckTimer - time.ElapsedGameTime.Milliseconds;
          if (this.characterCheckTimer <= 0)
          {
            if (Utility.isThereAFarmerOrCharacterWithinDistance(this.position / (float) Game1.tileSize, 6, environment) != null)
            {
              this.leapingIntoWater = true;
              this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
              {
                new FarmerSprite.AnimationFrame(300, 100),
                new FarmerSprite.AnimationFrame(301, 100),
                new FarmerSprite.AnimationFrame(302, 100),
                new FarmerSprite.AnimationFrame(303, 1500, false, false, new AnimatedSprite.endOfAnimationBehavior(this.startSplash), true)
              });
              this.sprite.loop = false;
              this.sprite.oldFrame = 303;
              this.gravityAffectedDY = -6f;
            }
            else if (Game1.random.NextDouble() < 0.01)
              Game1.playSound("croak");
            this.characterCheckTimer = 200;
          }
        }
        else
          this.position.X += this.flip ? -4f : 4f;
      }
      else
      {
        this.position.X += this.flip ? -3f : 3f;
        this.beforeFadeTimer = this.beforeFadeTimer - time.ElapsedGameTime.Milliseconds;
        if (this.beforeFadeTimer <= 0)
        {
          this.alpha = this.alpha - 1f / 1000f * (float) time.ElapsedGameTime.Milliseconds;
          if ((double) this.alpha <= 0.0)
            return true;
        }
        if (environment.doesTileHaveProperty((int) this.position.X / Game1.tileSize, (int) this.position.Y / Game1.tileSize, "Water", "Back") != null)
          this.splash = true;
      }
      if (!this.splash)
        return base.update(time, environment);
      environment.TemporarySprites.Add(new TemporaryAnimatedSprite(28, 50f, 2, 1, this.position, false, false));
      Game1.playSound("dropItemInWater");
      return true;
    }

    public override void draw(SpriteBatch b)
    {
      this.sprite.draw(b, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2(0.0f, (float) (-Game1.pixelZoom * 5))), (float) (((double) this.position.Y + (double) Game1.tileSize) / 10000.0), 0, 0, Color.White * this.alpha, this.flip, 4f, 0.0f, false);
      SpriteBatch spriteBatch = b;
      Texture2D shadowTexture = Game1.shadowTexture;
      Vector2 local = Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2 + Game1.pixelZoom * 2)));
      Rectangle? sourceRectangle = new Rectangle?(Game1.shadowTexture.Bounds);
      Color color = Color.White * this.alpha;
      double num1 = 0.0;
      Rectangle bounds = Game1.shadowTexture.Bounds;
      double x = (double) bounds.Center.X;
      bounds = Game1.shadowTexture.Bounds;
      double y = (double) bounds.Center.Y;
      Vector2 origin = new Vector2((float) x, (float) y);
      double num2 = 3.0 + (double) Math.Max(-3f, (float) (((double) this.yJumpOffset + (double) this.yOffset) / 16.0));
      int num3 = 0;
      double num4 = ((double) this.position.Y - 1.0) / 10000.0;
      spriteBatch.Draw(shadowTexture, local, sourceRectangle, color, (float) num1, origin, (float) num2, (SpriteEffects) num3, (float) num4);
    }

    public override void drawAboveFrontLayer(SpriteBatch b)
    {
    }
  }
}
