// Decompiled with JetBrains decompiler
// Type: StardewValley.TerrainFeatures.DiggableWall
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StardewValley.TerrainFeatures
{
  public class DiggableWall : TerrainFeature
  {
    public const int startingHealth = 3;
    public const int N = 1000;
    public const int E = 100;
    public const int S = 500;
    public const int W = 10;
    private Texture2D texture;
    private int health;
    public static Dictionary<int, int> fenceDrawGuide;

    public DiggableWall()
    {
      this.loadSprite();
      this.health = 3;
      if (DiggableWall.fenceDrawGuide != null)
        return;
      DiggableWall.populateFenceDrawGuide();
    }

    public static void populateFenceDrawGuide()
    {
      DiggableWall.fenceDrawGuide = new Dictionary<int, int>();
      DiggableWall.fenceDrawGuide.Add(0, 0);
      DiggableWall.fenceDrawGuide.Add(10, 15);
      DiggableWall.fenceDrawGuide.Add(100, 13);
      DiggableWall.fenceDrawGuide.Add(1000, 12);
      DiggableWall.fenceDrawGuide.Add(500, 4);
      DiggableWall.fenceDrawGuide.Add(1010, 11);
      DiggableWall.fenceDrawGuide.Add(1100, 9);
      DiggableWall.fenceDrawGuide.Add(1500, 8);
      DiggableWall.fenceDrawGuide.Add(600, 1);
      DiggableWall.fenceDrawGuide.Add(510, 3);
      DiggableWall.fenceDrawGuide.Add(110, 14);
      DiggableWall.fenceDrawGuide.Add(1600, 5);
      DiggableWall.fenceDrawGuide.Add(1610, 6);
      DiggableWall.fenceDrawGuide.Add(1510, 7);
      DiggableWall.fenceDrawGuide.Add(1110, 10);
      DiggableWall.fenceDrawGuide.Add(610, 2);
    }

    public override void loadSprite()
    {
      try
      {
        this.texture = Game1.content.Load<Texture2D>("TerrainFeatures\\DiggableWall");
      }
      catch (Exception ex)
      {
        this.texture = Game1.content.Load<Texture2D>("TerrainFeatures\\DiggableWall");
      }
    }

    public override Rectangle getBoundingBox(Vector2 tileLocation)
    {
      if (this.health > 0)
        return new Rectangle((int) tileLocation.X * Game1.tileSize, (int) tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
      return Rectangle.Empty;
    }

    public override bool tickUpdate(GameTime time, Vector2 tileLocation)
    {
      return false;
    }

    public override bool isPassable(Character c = null)
    {
      return this.health <= -99;
    }

    public override void dayUpdate(GameLocation environment, Vector2 tileLocation)
    {
    }

    public override bool seasonUpdate(bool onLoad)
    {
      return false;
    }

    public override bool performToolAction(Tool t, int explosion, Vector2 tileLocation, GameLocation location = null)
    {
      if (this.health <= -99)
        return false;
      if (t != null && t.name.Contains("Pickaxe"))
      {
        Game1.playSound("hammer");
        Game1.currentLocation.debris.Add(new Debris(this.texture, new Rectangle(Game1.tileSize / 4, Game1.tileSize, Game1.tileSize / 2, Game1.tileSize / 2), Game1.random.Next(t.upgradeLevel * 2, t.upgradeLevel * 4), Game1.player.GetToolLocation(false) + new Vector2((float) (Game1.tileSize / 4), 0.0f)));
      }
      else if (explosion <= 0)
        return false;
      int num = 1;
      if (explosion > 0)
      {
        num = explosion;
      }
      else
      {
        if (t == null)
          return false;
        switch (t.upgradeLevel)
        {
          case 0:
            num = 1;
            break;
          case 1:
            num = 2;
            break;
          case 2:
            num = 3;
            break;
          case 3:
            num = 4;
            break;
          case 4:
            num = 5;
            break;
        }
      }
      this.health = this.health - num;
      return this.health <= 0;
    }

    public override void draw(SpriteBatch b, Vector2 tileLocation)
    {
    }
  }
}
