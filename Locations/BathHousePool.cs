// Decompiled with JetBrains decompiler
// Type: StardewValley.Locations.BathHousePool
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using xTile;

namespace StardewValley.Locations
{
  public class BathHousePool : GameLocation
  {
    public const float steamZoom = 4f;
    public const float steamYMotionPerMillisecond = 0.1f;
    public const float millisecondsPerSteamFrame = 50f;
    private Texture2D steamAnimation;
    private Texture2D swimShadow;
    private Vector2 steamPosition;
    private int swimShadowTimer;
    private int swimShadowFrame;

    public BathHousePool()
    {
    }

    public BathHousePool(Map map, string name)
      : base(map, name)
    {
    }

    public override void resetForPlayerEntry()
    {
      base.resetForPlayerEntry();
      Game1.changeMusicTrack("pool_ambient");
      this.steamPosition = new Vector2(0.0f, 0.0f);
      this.steamAnimation = Game1.temporaryContent.Load<Texture2D>("LooseSprites\\steamAnimation");
      this.swimShadow = Game1.temporaryContent.Load<Texture2D>("LooseSprites\\swimShadow");
    }

    public override void cleanupBeforePlayerExit()
    {
      base.cleanupBeforePlayerExit();
      Game1.changeMusicTrack("none");
    }

    public override void draw(SpriteBatch b)
    {
      base.draw(b);
      if (this.currentEvent != null)
      {
        foreach (NPC actor in this.currentEvent.actors)
        {
          if (actor.swimming)
            b.Draw(this.swimShadow, Game1.GlobalToLocal(Game1.viewport, actor.position + new Vector2(0.0f, (float) (actor.sprite.spriteHeight / 3 * Game1.pixelZoom + Game1.pixelZoom))), new Rectangle?(new Rectangle(this.swimShadowFrame * 16, 0, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.0f);
        }
      }
      else
      {
        foreach (NPC character in this.characters)
        {
          if (character.swimming)
            b.Draw(this.swimShadow, Game1.GlobalToLocal(Game1.viewport, character.position + new Vector2(0.0f, (float) (character.sprite.spriteHeight / 3 * Game1.pixelZoom + Game1.pixelZoom))), new Rectangle?(new Rectangle(this.swimShadowFrame * 16, 0, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.0f);
        }
        foreach (Farmer farmer in this.farmers)
        {
          if (farmer.swimming)
            b.Draw(this.swimShadow, Game1.GlobalToLocal(Game1.viewport, farmer.position + new Vector2(0.0f, (float) (farmer.sprite.spriteHeight / 3 * Game1.pixelZoom))), new Rectangle?(new Rectangle(this.swimShadowFrame * 16, 0, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.0f);
        }
      }
      if (!Game1.player.swimming)
        return;
      b.Draw(this.swimShadow, Game1.GlobalToLocal(Game1.viewport, Game1.player.position + new Vector2(0.0f, (float) (Game1.player.sprite.spriteHeight / 4 * Game1.pixelZoom))), new Rectangle?(new Rectangle(this.swimShadowFrame * 16, 0, 16, 16)), Color.Blue * 0.75f, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.0f);
    }

    public override void checkForMusic(GameTime time)
    {
      base.checkForMusic(time);
    }

    public override void drawAboveAlwaysFrontLayer(SpriteBatch b)
    {
      base.drawAboveAlwaysFrontLayer(b);
      float x = this.steamPosition.X;
      while ((double) x < (double) Game1.graphics.GraphicsDevice.Viewport.Width + 256.0)
      {
        float y = this.steamPosition.Y;
        while ((double) y < (double) (Game1.graphics.GraphicsDevice.Viewport.Height + 128))
        {
          b.Draw(this.steamAnimation, new Vector2(x, y), new Rectangle?(new Rectangle(0, 0, 64, 64)), Color.White * 0.8f, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
          y += 256f;
        }
        x += 256f;
      }
    }

    public override void UpdateWhenCurrentLocation(GameTime time)
    {
      base.UpdateWhenCurrentLocation(time);
      this.steamPosition.Y -= (float) time.ElapsedGameTime.Milliseconds * 0.1f;
      this.steamPosition.Y %= -256f;
      this.steamPosition = this.steamPosition - Game1.getMostRecentViewportMotion();
      this.swimShadowTimer = this.swimShadowTimer - time.ElapsedGameTime.Milliseconds;
      if (this.swimShadowTimer > 0)
        return;
      this.swimShadowTimer = 70;
      this.swimShadowFrame = this.swimShadowFrame + 1;
      this.swimShadowFrame = this.swimShadowFrame % 10;
    }
  }
}
