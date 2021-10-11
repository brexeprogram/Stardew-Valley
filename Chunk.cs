// Decompiled with JetBrains decompiler
// Type: StardewValley.Chunk
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using System.Xml.Serialization;

namespace StardewValley
{
  public class Chunk
  {
    public Vector2 position;
    [XmlIgnore]
    public float xVelocity;
    [XmlIgnore]
    public float yVelocity;
    [XmlIgnore]
    public bool hasPassedRestingLineOnce;
    [XmlIgnore]
    public int bounces;
    public int debrisType;
    [XmlIgnore]
    public bool hitWall;
    public int xSpriteSheet;
    public int ySpriteSheet;
    [XmlIgnore]
    public float rotation;
    [XmlIgnore]
    public float rotationVelocity;
    public float scale;
    public float alpha;

    public Chunk()
    {
    }

    public Chunk(Vector2 position, float xVelocity, float yVelocity, int debrisType)
    {
      this.position = position;
      this.xVelocity = xVelocity;
      this.yVelocity = yVelocity;
      this.debrisType = debrisType;
      this.alpha = 1f;
    }
  }
}
