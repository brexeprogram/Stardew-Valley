// Decompiled with JetBrains decompiler
// Type: StardewValley.Locations.Summit
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using xTile;

namespace StardewValley.Locations
{
  public class Summit : GameLocation
  {
    public Summit()
    {
    }

    public Summit(Map map, string name)
      : base(map, name)
    {
    }

    public override void checkForMusic(GameTime time)
    {
    }

    public override void UpdateWhenCurrentLocation(GameTime time)
    {
      base.UpdateWhenCurrentLocation(time);
      if (this.temporarySprites.Count != 0 || Game1.random.NextDouble() >= (Game1.timeOfDay >= 1800 ? (!Game1.currentSeason.Equals("summer") || Game1.dayOfMonth != 20 ? 0.001 : 1.0) : 0.0006))
        return;
      Rectangle sourceRect = Rectangle.Empty;
      Vector2 position = new Vector2((float) Game1.viewport.Width, (float) Game1.random.Next(0, 200));
      float x = -4f;
      int numberOfLoops = 100;
      float animationInterval = 100f;
      if (Game1.timeOfDay < 1800)
      {
        if (Game1.currentSeason.Equals("spring") || Game1.currentSeason.Equals("fall"))
        {
          sourceRect = new Rectangle(640, 736, 16, 16);
          int num = Game1.random.Next(1, 4);
          x = -1f;
          for (int index = 0; index < num; ++index)
          {
            this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, sourceRect, (float) Game1.random.Next(80, 121), 4, 100, position + new Vector2((float) ((index + 1) * Game1.random.Next(15, 18)), (float) ((index + 1) * -20)), false, false, 0.01f, 0.0f, Color.White, 4f, 0.0f, 0.0f, 0.0f, true)
            {
              motion = new Vector2(-1f, 0.0f)
            });
            this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, sourceRect, (float) Game1.random.Next(80, 121), 4, 100, position + new Vector2((float) ((index + 1) * Game1.random.Next(15, 18)), (float) ((index + 1) * 20)), false, false, 0.01f, 0.0f, Color.White, 4f, 0.0f, 0.0f, 0.0f, true)
            {
              motion = new Vector2(-1f, 0.0f)
            });
          }
        }
        else if (Game1.currentSeason.Equals("summer"))
        {
          sourceRect = new Rectangle(640, 752 + (Game1.random.NextDouble() < 0.5 ? 16 : 0), 16, 16);
          x = -0.5f;
          animationInterval = 150f;
        }
      }
      else if (Game1.timeOfDay >= 1900)
      {
        sourceRect = new Rectangle(640, 816, 16, 16);
        x = -2f;
        numberOfLoops = 0;
        position.X -= (float) Game1.random.Next(Game1.tileSize, Game1.viewport.Width);
        if (Game1.currentSeason.Equals("summer") && Game1.dayOfMonth == 20)
        {
          int num = Game1.random.Next(3);
          for (int index = 0; index < num; ++index)
          {
            this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, sourceRect, (float) Game1.random.Next(80, 121), Game1.currentSeason.Equals("winter") ? 2 : 4, numberOfLoops, position, false, false, 0.01f, 0.0f, Color.White, 4f, 0.0f, 0.0f, 0.0f, true)
            {
              motion = new Vector2(x, 0.0f)
            });
            position.X -= (float) Game1.random.Next(Game1.tileSize, Game1.viewport.Width);
            position.Y = (float) Game1.random.Next(0, 200);
          }
        }
        else if (Game1.currentSeason.Equals("winter") && Game1.timeOfDay >= 1700 && Game1.random.NextDouble() < 0.1)
        {
          sourceRect = new Rectangle(640, 800, 32, 16);
          numberOfLoops = 1000;
          position.X = (float) Game1.viewport.Width;
        }
        else if (Game1.currentSeason.Equals("winter"))
          sourceRect = Rectangle.Empty;
      }
      if (Game1.timeOfDay >= 2200 && !Game1.currentSeason.Equals("winter") && (Game1.currentSeason.Equals("summer") && Game1.dayOfMonth == 20) && Game1.random.NextDouble() < 0.05)
      {
        sourceRect = new Rectangle(640, 784, 16, 16);
        numberOfLoops = 100;
        position.X = (float) Game1.viewport.Width;
        x = -3f;
      }
      if (sourceRect.Equals(Rectangle.Empty))
        return;
      this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, sourceRect, animationInterval, Game1.currentSeason.Equals("winter") ? 2 : 4, numberOfLoops, position, false, false, 0.01f, 0.0f, Color.White, 4f, 0.0f, 0.0f, 0.0f, true)
      {
        motion = new Vector2(x, 0.0f)
      });
    }

    public override void cleanupBeforePlayerExit()
    {
      base.cleanupBeforePlayerExit();
      Game1.background = (Background) null;
    }

    public override void resetForPlayerEntry()
    {
      base.resetForPlayerEntry();
      Game1.background = new Background();
      this.temporarySprites.Clear();
    }

    public override void draw(SpriteBatch b)
    {
      base.draw(b);
    }
  }
}
