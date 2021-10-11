// Decompiled with JetBrains decompiler
// Type: StardewValley.BellsAndWhistles.Butterfly
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StardewValley.BellsAndWhistles
{
  public class Butterfly : Critter
  {
    private int flapSpeed = 50;
    private float motionMultiplier = 1f;
    public const float maxSpeed = 3f;
    private int flapTimer;
    private int checkForLandingSpotTimer;
    private int landedTimer;
    private Vector2 motion;
    private bool summerButterfly;

    public Butterfly(Vector2 position)
    {
      this.position = position * (float) Game1.tileSize;
      this.startingPosition = this.position;
      if (Game1.currentSeason.Equals("spring"))
      {
        this.baseFrame = Game1.random.NextDouble() < 0.5 ? Game1.random.Next(3) * 3 + 160 : Game1.random.Next(3) * 3 + 180;
      }
      else
      {
        this.baseFrame = Game1.random.NextDouble() < 0.5 ? Game1.random.Next(3) * 4 + 128 : Game1.random.Next(3) * 4 + 148;
        this.summerButterfly = true;
      }
      this.motion = new Vector2((float) ((Game1.random.NextDouble() + 0.25) * 3.0 * (Game1.random.NextDouble() < 0.5 ? -1.0 : 1.0) / 2.0), (float) ((Game1.random.NextDouble() + 0.5) * 3.0 * (Game1.random.NextDouble() < 0.5 ? -1.0 : 1.0) / 2.0));
      this.flapSpeed = Game1.random.Next(45, 80);
      this.sprite = new AnimatedSprite(Critter.critterTexture, this.baseFrame, 16, 16);
      this.sprite.loop = false;
      this.startingPosition = position;
    }

    public void doneWithFlap(Farmer who)
    {
      this.flapTimer = 200 + Game1.random.Next(-5, 6);
    }

    public override bool update(GameTime time, GameLocation environment)
    {
      this.flapTimer = this.flapTimer - time.ElapsedGameTime.Milliseconds;
      if (this.flapTimer <= 0 && this.sprite.currentAnimation == null)
      {
        this.motionMultiplier = 1f;
        this.motion.X += (float) Game1.random.Next(-80, 81) / 100f;
        this.motion.Y = (float) ((Game1.random.NextDouble() + 0.25) * -3.0 / 2.0);
        if ((double) Math.Abs(this.motion.X) > 1.5)
          this.motion.X = (float) (3.0 * (double) Math.Sign(this.motion.X) / 2.0);
        if ((double) Math.Abs(this.motion.Y) > 3.0)
          this.motion.Y = 3f * (float) Math.Sign(this.motion.Y);
        if (this.summerButterfly)
          this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
          {
            new FarmerSprite.AnimationFrame(this.baseFrame + 1, this.flapSpeed),
            new FarmerSprite.AnimationFrame(this.baseFrame + 2, this.flapSpeed),
            new FarmerSprite.AnimationFrame(this.baseFrame + 3, this.flapSpeed),
            new FarmerSprite.AnimationFrame(this.baseFrame + 2, this.flapSpeed),
            new FarmerSprite.AnimationFrame(this.baseFrame + 1, this.flapSpeed),
            new FarmerSprite.AnimationFrame(this.baseFrame, this.flapSpeed, false, false, new AnimatedSprite.endOfAnimationBehavior(this.doneWithFlap), false)
          });
        else
          this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
          {
            new FarmerSprite.AnimationFrame(this.baseFrame + 1, this.flapSpeed),
            new FarmerSprite.AnimationFrame(this.baseFrame + 2, this.flapSpeed),
            new FarmerSprite.AnimationFrame(this.baseFrame + 1, this.flapSpeed),
            new FarmerSprite.AnimationFrame(this.baseFrame, this.flapSpeed, false, false, new AnimatedSprite.endOfAnimationBehavior(this.doneWithFlap), false)
          });
      }
      this.position = this.position + this.motion * this.motionMultiplier;
      this.motion.Y += 0.005f * (float) time.ElapsedGameTime.Milliseconds;
      this.motionMultiplier = this.motionMultiplier - 0.0005f * (float) time.ElapsedGameTime.Milliseconds;
      if ((double) this.motionMultiplier <= 0.0)
        this.motionMultiplier = 0.0f;
      return base.update(time, environment);
    }

    public override void draw(SpriteBatch b)
    {
    }

    public override void drawAboveFrontLayer(SpriteBatch b)
    {
      this.sprite.draw(b, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2(-64f, this.yJumpOffset - 128f + this.yOffset)), this.position.Y / 10000f, 0, 0, Color.White, this.flip, 4f, 0.0f, false);
    }
  }
}
