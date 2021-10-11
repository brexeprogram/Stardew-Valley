// Decompiled with JetBrains decompiler
// Type: StardewValley.AnimatedSprite
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StardewValley
{
  public class AnimatedSprite
  {
    public float interval = 175f;
    public int framesPerAnimation = 4;
    public int spriteWidth = 16;
    public int spriteHeight = 24;
    public bool loop = true;
    protected Texture2D spriteTexture;
    public float timer;
    public int currentFrame;
    public Rectangle sourceRect;
    public bool ignoreStopAnimation;
    public bool textureUsesFlippedRightForLeft;
    protected AnimatedSprite.endOfAnimationBehavior endOfAnimationFunction;
    protected int textureWidth;
    protected int textureHeight;
    public List<FarmerSprite.AnimationFrame> currentAnimation;
    public int oldFrame;
    public int currentAnimationIndex;
    public bool ignoreSourceRectUpdates;

    public Texture2D Texture
    {
      get
      {
        return this.spriteTexture;
      }
      set
      {
        this.spriteTexture = value;
      }
    }

    public virtual int CurrentFrame
    {
      get
      {
        return this.currentFrame;
      }
      set
      {
        this.currentFrame = value;
        this.UpdateSourceRect();
      }
    }

    public List<FarmerSprite.AnimationFrame> CurrentAnimation
    {
      get
      {
        return this.currentAnimation;
      }
      set
      {
        this.currentAnimation = value;
      }
    }

    public Rectangle SourceRect
    {
      get
      {
        return this.sourceRect;
      }
      set
      {
        this.sourceRect = value;
      }
    }

    public AnimatedSprite(Texture2D texture, int currentFrame, int spriteWidth, int spriteHeight)
    {
      this.spriteTexture = texture;
      this.textureWidth = texture == null ? 96 : texture.Width;
      this.textureHeight = texture == null ? 128 : texture.Height;
      this.currentFrame = currentFrame;
      this.spriteWidth = spriteWidth;
      this.spriteHeight = spriteHeight;
      if (this.spriteTexture == null)
        return;
      this.UpdateSourceRect();
    }

    public AnimatedSprite(Texture2D texture)
    {
      this.spriteTexture = texture;
      this.UpdateSourceRect();
      this.textureWidth = texture == null ? 96 : texture.Width;
      this.textureHeight = texture == null ? 128 : texture.Height;
    }

    public int getHeight()
    {
      return this.spriteHeight;
    }

    public int getWidth()
    {
      return this.spriteWidth;
    }

    public virtual void StopAnimation()
    {
      if (this.ignoreStopAnimation)
        return;
      if (this.currentAnimation != null)
      {
        this.currentAnimation = (List<FarmerSprite.AnimationFrame>) null;
        this.CurrentFrame = this.oldFrame;
      }
      else
      {
        if (this is FarmerSprite && this.CurrentFrame >= 232)
          this.CurrentFrame = this.CurrentFrame - 8;
        this.CurrentFrame = this.CurrentFrame < 64 || this.CurrentFrame > 155 ? (this.currentFrame - this.currentFrame % (this.textureWidth / this.spriteWidth)) % 32 : (this.CurrentFrame - this.CurrentFrame % (this.textureWidth / this.spriteWidth)) % 32 + 96;
        this.UpdateSourceRect();
      }
    }

    public virtual void standAndFaceDirection(int direction)
    {
      switch (direction)
      {
        case 0:
          this.CurrentFrame = 12;
          break;
        case 1:
          this.CurrentFrame = 6;
          break;
        case 2:
          this.CurrentFrame = 0;
          break;
        case 3:
          this.CurrentFrame = 6;
          break;
      }
      this.UpdateSourceRect();
    }

    public void faceDirectionStandard(int direction)
    {
      if (direction == 0)
        direction = 2;
      else if (direction == 2)
        direction = 0;
      this.CurrentFrame = direction * 4;
      this.UpdateSourceRect();
    }

    public virtual void faceDirection(int direction)
    {
      if (this.ignoreStopAnimation)
        return;
      try
      {
        switch (direction)
        {
          case 0:
            this.CurrentFrame = this.textureWidth / this.spriteWidth * 2 + this.CurrentFrame % (this.textureWidth / this.spriteWidth);
            break;
          case 1:
            this.CurrentFrame = this.textureWidth / this.spriteWidth + this.CurrentFrame % (this.textureWidth / this.spriteWidth);
            break;
          case 2:
            this.CurrentFrame = this.CurrentFrame % (this.textureWidth / this.spriteWidth);
            break;
          case 3:
            this.CurrentFrame = !this.textureUsesFlippedRightForLeft ? this.textureWidth / this.spriteWidth * 3 + this.CurrentFrame % (this.textureWidth / this.spriteWidth) : this.textureWidth / this.spriteWidth + this.CurrentFrame % (this.textureWidth / this.spriteWidth);
            break;
        }
      }
      catch (Exception ex)
      {
      }
      this.UpdateSourceRect();
    }

    public void AnimateRight(GameTime gameTime, int intervalOffset = 0, string soundForFootstep = "")
    {
      if (this.CurrentFrame >= this.framesPerAnimation * 2 || this.CurrentFrame < this.framesPerAnimation)
        this.CurrentFrame = this.framesPerAnimation + this.CurrentFrame % this.framesPerAnimation;
      this.timer = this.timer + (float) gameTime.ElapsedGameTime.TotalMilliseconds;
      if ((double) this.timer > (double) this.interval + (double) intervalOffset)
      {
        this.CurrentFrame = this.CurrentFrame + 1;
        this.timer = 0.0f;
        if (this.CurrentFrame % 2 != 0 && soundForFootstep.Length > 0 && (Game1.currentSong == null || Game1.currentSong.IsStopped))
          Game1.playSound(soundForFootstep);
        if (this.CurrentFrame >= this.framesPerAnimation * 2 && this.loop)
          this.CurrentFrame = this.framesPerAnimation;
      }
      this.UpdateSourceRect();
    }

    public void AnimateUp(GameTime gameTime, int intervalOffset = 0, string soundForFootstep = "")
    {
      if (this.CurrentFrame >= this.framesPerAnimation * 3 || this.CurrentFrame < this.framesPerAnimation * 2)
        this.CurrentFrame = this.framesPerAnimation * 2 + this.CurrentFrame % this.framesPerAnimation;
      this.timer = this.timer + (float) gameTime.ElapsedGameTime.TotalMilliseconds;
      if ((double) this.timer > (double) this.interval + (double) intervalOffset)
      {
        this.CurrentFrame = this.CurrentFrame + 1;
        this.timer = 0.0f;
        if (this.CurrentFrame % 2 != 0 && soundForFootstep.Length > 0 && (Game1.currentSong == null || Game1.currentSong.IsStopped))
          Game1.playSound(soundForFootstep);
        if (this.CurrentFrame >= this.framesPerAnimation * 3 && this.loop)
          this.CurrentFrame = this.framesPerAnimation * 2;
      }
      this.UpdateSourceRect();
    }

    public void AnimateDown(GameTime gameTime, int intervalOffset = 0, string soundForFootstep = "")
    {
      if (this.CurrentFrame >= this.framesPerAnimation || this.CurrentFrame < 0)
        this.CurrentFrame = this.CurrentFrame % this.framesPerAnimation;
      this.timer = this.timer + (float) gameTime.ElapsedGameTime.TotalMilliseconds;
      if ((double) this.timer > (double) this.interval + (double) intervalOffset)
      {
        this.CurrentFrame = this.CurrentFrame + 1;
        this.timer = 0.0f;
        if (this.CurrentFrame % 2 != 0 && soundForFootstep.Length > 0 && (Game1.currentSong == null || Game1.currentSong.IsStopped))
          Game1.playSound(soundForFootstep);
        if (this.CurrentFrame >= this.framesPerAnimation && this.loop)
          this.CurrentFrame = 0;
      }
      this.UpdateSourceRect();
    }

    public void AnimateLeft(GameTime gameTime, int intervalOffset = 0, string soundForFootstep = "")
    {
      if (this.CurrentFrame >= this.framesPerAnimation * 4 || this.CurrentFrame < this.framesPerAnimation * 3)
        this.CurrentFrame = this.framesPerAnimation * 3 + this.CurrentFrame % this.framesPerAnimation;
      this.timer = this.timer + (float) gameTime.ElapsedGameTime.TotalMilliseconds;
      if ((double) this.timer > (double) this.interval + (double) intervalOffset)
      {
        this.CurrentFrame = this.CurrentFrame + 1;
        this.timer = 0.0f;
        if (this.CurrentFrame % 2 != 0 && soundForFootstep.Length > 0 && (Game1.currentSong == null || Game1.currentSong.IsStopped))
          Game1.playSound(soundForFootstep);
        if (this.CurrentFrame >= this.framesPerAnimation * 4 && this.loop)
          this.CurrentFrame = this.framesPerAnimation * 3;
      }
      this.UpdateSourceRect();
    }

    public bool Animate(GameTime gameTime, int startFrame, int numberOfFrames, float interval)
    {
      if (this.CurrentFrame >= startFrame + numberOfFrames || this.CurrentFrame < startFrame)
        this.CurrentFrame = startFrame + this.CurrentFrame % numberOfFrames;
      this.timer = this.timer + (float) gameTime.ElapsedGameTime.TotalMilliseconds;
      if ((double) this.timer > (double) interval)
      {
        this.CurrentFrame = this.CurrentFrame + 1;
        this.timer = 0.0f;
        if (this.CurrentFrame >= startFrame + numberOfFrames)
        {
          if (this.loop)
            this.CurrentFrame = startFrame;
          this.UpdateSourceRect();
          return true;
        }
      }
      this.UpdateSourceRect();
      return false;
    }

    public bool AnimateBackwards(GameTime gameTime, int startFrame, int numberOfFrames, float interval)
    {
      if (this.CurrentFrame >= startFrame + numberOfFrames || this.CurrentFrame < startFrame)
        this.CurrentFrame = startFrame + this.CurrentFrame % numberOfFrames;
      this.timer = this.timer + (float) gameTime.ElapsedGameTime.TotalMilliseconds;
      if ((double) this.timer > (double) interval)
      {
        this.CurrentFrame = this.CurrentFrame - 1;
        this.timer = 0.0f;
        if (this.CurrentFrame <= startFrame - numberOfFrames)
        {
          if (this.loop)
            this.CurrentFrame = startFrame;
          this.UpdateSourceRect();
          return true;
        }
      }
      this.UpdateSourceRect();
      return false;
    }

    public virtual void setCurrentAnimation(List<FarmerSprite.AnimationFrame> animation)
    {
      this.currentAnimation = animation;
      this.oldFrame = this.currentFrame;
      this.currentAnimationIndex = 0;
      if (this.currentAnimation.Count <= 0)
        return;
      this.timer = (float) this.currentAnimation[0].milliseconds;
      this.CurrentFrame = this.currentAnimation[0].frame;
    }

    public bool animateOnce(GameTime time)
    {
      if (this.currentAnimation == null)
        return true;
      this.timer = this.timer - (float) time.ElapsedGameTime.Milliseconds;
      if ((double) this.timer <= 0.0)
      {
        this.currentAnimationIndex = this.currentAnimationIndex + 1;
        if (this.currentAnimationIndex >= this.currentAnimation.Count)
        {
          if (this.loop)
          {
            this.currentAnimationIndex = 0;
          }
          else
          {
            this.currentFrame = this.oldFrame;
            this.currentAnimation = (List<FarmerSprite.AnimationFrame>) null;
            return true;
          }
        }
        if (this.currentAnimation[this.currentAnimationIndex].frameBehavior != null)
          this.currentAnimation[this.currentAnimationIndex].frameBehavior((Farmer) null);
        if (this.currentAnimation != null)
        {
          this.timer = (float) this.currentAnimation[this.currentAnimationIndex].milliseconds;
          this.CurrentFrame = this.currentAnimation[this.currentAnimationIndex].frame;
          this.UpdateSourceRect();
        }
      }
      return false;
    }

    public virtual void UpdateSourceRect()
    {
      if (this.ignoreSourceRectUpdates)
        return;
      this.SourceRect = new Rectangle(this.CurrentFrame * this.spriteWidth % this.spriteTexture.Width, this.CurrentFrame * this.spriteWidth / this.spriteTexture.Width * this.spriteHeight, this.spriteWidth, this.spriteHeight);
    }

    public void draw(SpriteBatch b, Vector2 screenPosition, float layerDepth)
    {
      b.Draw(this.spriteTexture, screenPosition, new Rectangle?(this.sourceRect), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, this.currentAnimation == null || !this.currentAnimation[this.currentAnimationIndex].flip ? SpriteEffects.None : SpriteEffects.FlipHorizontally, layerDepth);
    }

    public void draw(SpriteBatch b, Vector2 screenPosition, float layerDepth, int xOffset, int yOffset, Color c, bool flip = false, float scale = 1f, float rotation = 0.0f, bool characterSourceRectOffset = false)
    {
      b.Draw(this.spriteTexture, screenPosition, new Rectangle?(new Rectangle(this.sourceRect.X + xOffset, this.sourceRect.Y + yOffset, this.sourceRect.Width, this.sourceRect.Height)), c, rotation, characterSourceRectOffset ? new Vector2((float) (this.spriteWidth / 2), (float) ((double) this.spriteHeight * 3.0 / 4.0)) : Vector2.Zero, scale, flip || this.currentAnimation != null && this.currentAnimation[this.currentAnimationIndex].flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth);
    }

    public void drawShadow(SpriteBatch b, Vector2 screenPosition, float scale = 4f)
    {
      b.Draw(Game1.shadowTexture, screenPosition + new Vector2((float) (this.spriteWidth / 2 * Game1.pixelZoom) - scale, (float) (this.spriteHeight * Game1.pixelZoom) - scale), new Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0.0f, Utility.PointToVector2(Game1.shadowTexture.Bounds.Center), scale, SpriteEffects.None, 1E-05f);
    }

    public delegate void endOfAnimationBehavior(Farmer who);
  }
}
