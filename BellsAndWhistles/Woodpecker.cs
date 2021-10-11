// Decompiled with JetBrains decompiler
// Type: StardewValley.BellsAndWhistles.Woodpecker
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;

namespace StardewValley.BellsAndWhistles
{
  public class Woodpecker : Critter
  {
    private int characterCheckTimer = 200;
    public const int flyingSpeed = 6;
    private bool flyingAway;
    private Tree tree;
    private int peckTimer;

    public Woodpecker(Tree tree, Vector2 position)
    {
      this.tree = tree;
      position *= (float) Game1.tileSize;
      this.position = position;
      this.position.X += (float) (Game1.tileSize / 2);
      this.position.Y += 0.0f;
      this.startingPosition = position;
      this.baseFrame = 320;
      this.sprite = new AnimatedSprite(Critter.critterTexture, 320, 16, 16);
    }

    public override void drawAboveFrontLayer(SpriteBatch b)
    {
      this.sprite.draw(b, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2((float) (-64 - Game1.tileSize / 4), (float) (Game1.tileSize - 128) + this.yJumpOffset + this.yOffset)), 1f, 0, 0, Color.White, this.flip, 4f, 0.0f, false);
    }

    public override void draw(SpriteBatch b)
    {
      SpriteBatch spriteBatch = b;
      Texture2D shadowTexture = Game1.shadowTexture;
      Vector2 local = Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2(0.0f, -4f));
      Rectangle? sourceRectangle = new Rectangle?(Game1.shadowTexture.Bounds);
      Color white = Color.White;
      double num1 = 0.0;
      Rectangle bounds = Game1.shadowTexture.Bounds;
      double x = (double) bounds.Center.X;
      bounds = Game1.shadowTexture.Bounds;
      double y = (double) bounds.Center.Y;
      Vector2 origin = new Vector2((float) x, (float) y);
      double num2 = 3.0 + (double) Math.Max(-3f, (float) (((double) this.yJumpOffset + (double) this.yOffset) / 16.0));
      int num3 = 0;
      double num4 = ((double) this.position.Y - 1.0) / 10000.0;
      spriteBatch.Draw(shadowTexture, local, sourceRectangle, white, (float) num1, origin, (float) num2, (SpriteEffects) num3, (float) num4);
    }

    private void donePecking(Farmer who)
    {
      this.peckTimer = Game1.random.Next(1000, 3000);
    }

    private void playFlap(Farmer who)
    {
      if (!Utility.isOnScreen(this.position, Game1.tileSize))
        return;
      Game1.playSound("batFlap");
    }

    private void playPeck(Farmer who)
    {
      if (!Utility.isOnScreen(this.position, Game1.tileSize))
        return;
      Game1.playSound("Cowboy_gunshot");
    }

    public override bool update(GameTime time, GameLocation environment)
    {
      if (environment == null || this.sprite == null || this.tree == null)
        return true;
      if ((double) this.yJumpOffset < 0.0 && !this.flyingAway)
      {
        if (!this.flip && !environment.isCollidingPosition(this.getBoundingBox(-2, 0), Game1.viewport, false, 0, false, (Character) null, false, false, true))
          this.position.X -= 2f;
        else if (!environment.isCollidingPosition(this.getBoundingBox(2, 0), Game1.viewport, false, 0, false, (Character) null, false, false, true))
          this.position.X += 2f;
      }
      this.peckTimer = this.peckTimer - time.ElapsedGameTime.Milliseconds;
      if (!this.flyingAway && this.peckTimer <= 0 && this.sprite.currentAnimation == null)
      {
        int num = Game1.random.Next(2, 8);
        List<FarmerSprite.AnimationFrame> animation = new List<FarmerSprite.AnimationFrame>();
        for (int index = 0; index < num; ++index)
        {
          animation.Add(new FarmerSprite.AnimationFrame(this.baseFrame, 100));
          animation.Add(new FarmerSprite.AnimationFrame(this.baseFrame + 1, 100, false, false, new AnimatedSprite.endOfAnimationBehavior(this.playPeck), false));
        }
        animation.Add(new FarmerSprite.AnimationFrame(this.baseFrame, 200, false, false, new AnimatedSprite.endOfAnimationBehavior(this.donePecking), false));
        this.sprite.setCurrentAnimation(animation);
        this.sprite.loop = false;
      }
      this.characterCheckTimer = this.characterCheckTimer - time.ElapsedGameTime.Milliseconds;
      if (this.characterCheckTimer < 0)
      {
        Character character = Utility.isThereAFarmerOrCharacterWithinDistance(this.position / (float) Game1.tileSize, 6, environment);
        this.characterCheckTimer = 200;
        if ((character != null || this.tree.stump) && !this.flyingAway)
        {
          this.flyingAway = true;
          if (character != null && (double) character.position.X > (double) this.position.X)
            this.flip = false;
          else
            this.flip = true;
          this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
          {
            new FarmerSprite.AnimationFrame((int) (short) (this.baseFrame + 2), 70),
            new FarmerSprite.AnimationFrame((int) (short) (this.baseFrame + 3), 60, false, this.flip, new AnimatedSprite.endOfAnimationBehavior(this.playFlap), false),
            new FarmerSprite.AnimationFrame((int) (short) (this.baseFrame + 4), 70),
            new FarmerSprite.AnimationFrame((int) (short) (this.baseFrame + 3), 60)
          });
          this.sprite.loop = true;
        }
      }
      if (this.flyingAway)
      {
        if (!this.flip)
          this.position.X -= 6f;
        else
          this.position.X += 6f;
        this.yOffset = this.yOffset - 1f;
      }
      return base.update(time, environment);
    }
  }
}
