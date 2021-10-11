// Decompiled with JetBrains decompiler
// Type: StardewValley.BellsAndWhistles.Squirrel
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
  public class Squirrel : Critter
  {
    private int nextNibbleTimer = 1000;
    private int characterCheckTimer = 200;
    private int treeRunTimer;
    private bool running;
    private Tree climbed;
    private Vector2 treeTile;

    public Squirrel(Vector2 position, bool flip)
    {
      this.position = position * (float) Game1.tileSize;
      this.flip = flip;
      this.baseFrame = 60;
      this.sprite = new AnimatedSprite(Critter.critterTexture, this.baseFrame, 32, 32);
      this.sprite.loop = false;
      this.startingPosition = position;
    }

    private void doneNibbling(Farmer who)
    {
      this.nextNibbleTimer = Game1.random.Next(2000);
    }

    public override void draw(SpriteBatch b)
    {
      this.sprite.draw(b, Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2((float) ((this.treeRunTimer > 0 ? (this.flip ? Game1.tileSize * 3 + Game1.tileSize / 2 : -Game1.tileSize / 4) : 0) - 64), (float) ((double) (-32 * Game1.pixelZoom / 2) + (double) this.yJumpOffset + (double) this.yOffset + (this.treeRunTimer > 0 ? (this.flip ? 0.0 : (double) (Game1.tileSize * 2)) : 0.0)))), (float) (((double) this.position.Y + (double) Game1.tileSize + (this.treeRunTimer > 0 ? (double) (Game1.tileSize * 2) : 0.0)) / 10000.0 + (double) this.position.X / 1000000.0), 0, 0, Color.White, this.flip, 4f, this.treeRunTimer > 0 ? (float) ((this.flip ? 1.0 : -1.0) * Math.PI / 2.0) : 0.0f, false);
      if (this.treeRunTimer > 0)
        return;
      SpriteBatch spriteBatch = b;
      Texture2D shadowTexture = Game1.shadowTexture;
      Vector2 local = Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2(0.0f, (float) (Game1.tileSize - 4)));
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

    public override bool update(GameTime time, GameLocation environment)
    {
      this.nextNibbleTimer = this.nextNibbleTimer - time.ElapsedGameTime.Milliseconds;
      if (this.sprite.currentAnimation == null && this.nextNibbleTimer <= 0)
      {
        int num = Game1.random.Next(2, 8);
        List<FarmerSprite.AnimationFrame> animation = new List<FarmerSprite.AnimationFrame>();
        for (int index = 0; index < num; ++index)
        {
          animation.Add(new FarmerSprite.AnimationFrame(this.baseFrame, 200));
          animation.Add(new FarmerSprite.AnimationFrame(this.baseFrame + 1, 200));
        }
        animation.Add(new FarmerSprite.AnimationFrame(this.baseFrame, 200, false, false, new AnimatedSprite.endOfAnimationBehavior(this.doneNibbling), false));
        this.sprite.setCurrentAnimation(animation);
      }
      this.characterCheckTimer = this.characterCheckTimer - time.ElapsedGameTime.Milliseconds;
      if (this.characterCheckTimer <= 0 && !this.running)
      {
        if (Utility.isThereAFarmerOrCharacterWithinDistance(this.position / (float) Game1.tileSize, 12, environment) != null)
        {
          this.running = true;
          this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
          {
            new FarmerSprite.AnimationFrame(this.baseFrame + 2, 50),
            new FarmerSprite.AnimationFrame(this.baseFrame + 3, 50),
            new FarmerSprite.AnimationFrame(this.baseFrame + 4, 50),
            new FarmerSprite.AnimationFrame(this.baseFrame + 5, 120),
            new FarmerSprite.AnimationFrame(this.baseFrame + 6, 80),
            new FarmerSprite.AnimationFrame(this.baseFrame + 7, 50)
          });
          this.sprite.loop = true;
        }
        this.characterCheckTimer = 200;
      }
      if (this.running)
      {
        if (this.treeRunTimer > 0)
          this.position.Y -= 4f;
        else
          this.position.X += this.flip ? -4f : 4f;
      }
      if (this.running && this.characterCheckTimer <= 0 && this.treeRunTimer <= 0)
      {
        this.characterCheckTimer = 100;
        Vector2 key = new Vector2((float) (int) ((double) this.position.X / (double) Game1.tileSize), (float) ((int) this.position.Y / Game1.tileSize));
        if (environment.terrainFeatures.ContainsKey(key) && environment.terrainFeatures[key] is Tree)
        {
          this.treeRunTimer = 700;
          this.climbed = environment.terrainFeatures[key] as Tree;
          this.treeTile = key;
          this.position = key * (float) Game1.tileSize;
          return false;
        }
        key = new Vector2((float) (int) (((double) this.position.X + (double) Game1.tileSize + 1.0) / (double) Game1.tileSize), (float) ((int) this.position.Y / Game1.tileSize));
      }
      if (this.treeRunTimer > 0)
      {
        this.treeRunTimer = this.treeRunTimer - time.ElapsedGameTime.Milliseconds;
        if (this.treeRunTimer <= 0)
        {
          this.climbed.performUseAction(this.treeTile);
          return true;
        }
      }
      return base.update(time, environment);
    }
  }
}
