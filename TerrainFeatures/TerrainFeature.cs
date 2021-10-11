// Decompiled with JetBrains decompiler
// Type: StardewValley.TerrainFeatures.TerrainFeature
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Serialization;

namespace StardewValley.TerrainFeatures
{
  [XmlInclude(typeof (Grass))]
  [XmlInclude(typeof (Tree))]
  [XmlInclude(typeof (Quartz))]
  [XmlInclude(typeof (Stalagmite))]
  [XmlInclude(typeof (HoeDirt))]
  [XmlInclude(typeof (Flooring))]
  [XmlInclude(typeof (CosmeticPlant))]
  [XmlInclude(typeof (ResourceClump))]
  [XmlInclude(typeof (GiantCrop))]
  [XmlInclude(typeof (FruitTree))]
  [XmlInclude(typeof (Bush))]
  public abstract class TerrainFeature
  {
    public virtual Rectangle getBoundingBox(Vector2 tileLocation)
    {
      return new Rectangle((int) tileLocation.X * Game1.tileSize, (int) tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
    }

    public virtual void loadSprite()
    {
    }

    public virtual bool isPassable(Character c = null)
    {
      return false;
    }

    public virtual void doCollisionAction(Rectangle positionOfCollider, int speedOfCollision, Vector2 tileLocation, Character who, GameLocation location)
    {
    }

    public virtual bool performUseAction(Vector2 tileLocation)
    {
      return false;
    }

    public virtual bool performToolAction(Tool t, int damage, Vector2 tileLocation, GameLocation location = null)
    {
      return false;
    }

    public virtual bool tickUpdate(GameTime time, Vector2 tileLocation)
    {
      return false;
    }

    public virtual void dayUpdate(GameLocation environment, Vector2 tileLocation)
    {
    }

    public virtual bool seasonUpdate(bool onLoad)
    {
      return false;
    }

    public virtual bool isActionable()
    {
      return false;
    }

    public virtual void performPlayerEntryAction(Vector2 tileLocation)
    {
    }

    public virtual void draw(SpriteBatch spriteBatch, Vector2 tileLocation)
    {
    }

    public virtual bool forceDraw()
    {
      return false;
    }

    public virtual void drawInMenu(SpriteBatch spriteBatch, Vector2 positionOnScreen, Vector2 tileLocation, float scale, float layerDepth)
    {
    }
  }
}
