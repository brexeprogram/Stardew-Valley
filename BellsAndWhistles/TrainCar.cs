// Decompiled with JetBrains decompiler
// Type: StardewValley.BellsAndWhistles.TrainCar
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley.BellsAndWhistles
{
  public class TrainCar
  {
    public int[] topFeatures = new int[6];
    public const int spotsForTopFeatures = 6;
    public const double chanceForTopFeature = 0.2;
    public const int engine = 3;
    public const int passengerCar = 2;
    public const int coalCar = 1;
    public const int plainCar = 0;
    public const int coal = 0;
    public const int metal = 1;
    public const int wood = 2;
    public const int compartments = 3;
    public const int grass = 4;
    public const int hay = 5;
    public const int bricks = 6;
    public const int rocks = 7;
    public const int packages = 8;
    public const int presents = 9;
    public int frontDecal;
    public int carType;
    public int resourceType;
    public int loaded;
    public bool alternateCar;
    public Color color;

    public TrainCar(Random random, int carType, int frontDecal, Color color, int resourceType = 0, int loaded = 0)
    {
      this.carType = carType;
      this.frontDecal = frontDecal;
      this.color = color;
      this.resourceType = resourceType;
      this.loaded = loaded;
      if (carType != 0 && carType != 1)
        this.color = Color.White;
      if (carType == 0 && !color.Equals(Color.DimGray))
      {
        for (int index = 0; index < this.topFeatures.Length; ++index)
          this.topFeatures[index] = random.NextDouble() >= 0.2 ? -1 : random.Next(2);
      }
      if (carType != 2 || random.NextDouble() >= 0.5)
        return;
      this.alternateCar = true;
    }

    public void draw(SpriteBatch b, Vector2 globalPosition, float wheelRotation)
    {
      b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, globalPosition), new Rectangle?(new Rectangle(192 + this.carType * 128, 512 - (this.alternateCar ? 64 : 0), 128, 57)), this.color, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, (float) (((double) globalPosition.Y + 256.0) / 10000.0));
      b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, globalPosition + new Vector2(0.0f, 228f)), new Rectangle?(new Rectangle(192 + this.carType * 128, 569, 128, 7)), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, (float) (((double) globalPosition.Y + 256.0) / 10000.0));
      if (this.carType == 1)
      {
        b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, globalPosition), new Rectangle?(new Rectangle(448 + this.resourceType * 128 % 256, 576 + this.resourceType / 2 * 32, 128, 32)), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, (float) (((double) globalPosition.Y + 260.0) / 10000.0));
        if (this.loaded > 0 && Game1.random.NextDouble() < 0.003 && ((double) globalPosition.X > (double) (Game1.tileSize * 4) && (double) globalPosition.X < (double) (Game1.currentLocation.map.DisplayWidth - Game1.tileSize * 4)))
        {
          this.loaded = this.loaded - 1;
          int objectIndex = -1;
          switch (this.resourceType)
          {
            case 0:
              objectIndex = 382;
              break;
            case 1:
              objectIndex = (int) this.color.R > (int) this.color.G ? 378 : ((int) this.color.G > (int) this.color.B ? 380 : ((int) this.color.B > (int) this.color.R ? 384 : 378));
              break;
            case 2:
              objectIndex = 388;
              break;
            case 6:
              objectIndex = 390;
              break;
            case 7:
              objectIndex = Game1.currentSeason.Equals("winter") ? 536 : (Game1.stats.DaysPlayed <= 120U || (int) this.color.R <= (int) this.color.G ? 535 : 537);
              break;
          }
          if (objectIndex != -1)
            Game1.createObjectDebris(objectIndex, (int) globalPosition.X / Game1.tileSize, (int) globalPosition.Y / Game1.tileSize, (int) ((double) globalPosition.Y + (double) (Game1.tileSize * 5)), 0, 1f, (GameLocation) null);
        }
      }
      if (this.carType == 0)
      {
        int index = 0;
        while (index < this.topFeatures.Length)
        {
          if (this.topFeatures[index] != -1)
            b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, globalPosition + new Vector2((float) (Game1.tileSize + index), 20f)), new Rectangle?(new Rectangle(192, 608 + this.topFeatures[index] * 16, 16, 16)), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, (float) (((double) globalPosition.Y + 260.0) / 10000.0));
          index += Game1.tileSize;
        }
      }
      if (this.frontDecal != -1 && (this.carType == 0 || this.carType == 1))
        b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, globalPosition + new Vector2(192f, 92f)), new Rectangle?(new Rectangle(224 + this.frontDecal * 32 % 224, 576 + this.frontDecal * 32 / 224 * 32, 32, 32)), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, (float) (((double) globalPosition.Y + 260.0) / 10000.0));
      if (this.carType != 3)
        return;
      Vector2 local1 = Game1.GlobalToLocal(Game1.viewport, globalPosition + new Vector2(72f, 208f));
      Vector2 local2 = Game1.GlobalToLocal(Game1.viewport, globalPosition + new Vector2(316f, 208f));
      b.Draw(Game1.mouseCursors, local1, new Rectangle?(new Rectangle(192, 576, 20, 20)), Color.White, wheelRotation, new Vector2(10f, 10f), 4f, SpriteEffects.None, (float) (((double) globalPosition.Y + 260.0) / 10000.0));
      b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, globalPosition + new Vector2(228f, 208f)), new Rectangle?(new Rectangle(192, 576, 20, 20)), Color.White, wheelRotation, new Vector2(10f, 10f), 4f, SpriteEffects.None, (float) (((double) globalPosition.Y + 260.0) / 10000.0));
      b.Draw(Game1.mouseCursors, local2, new Rectangle?(new Rectangle(192, 576, 20, 20)), Color.White, wheelRotation, new Vector2(10f, 10f), 4f, SpriteEffects.None, (float) (((double) globalPosition.Y + 260.0) / 10000.0));
      int x1 = (int) ((double) local1.X + 4.0 + 24.0 * Math.Cos((double) wheelRotation));
      int y1 = (int) ((double) local1.Y + 4.0 + 24.0 * Math.Sin((double) wheelRotation));
      int x2 = (int) ((double) local2.X + 4.0 + 24.0 * Math.Cos((double) wheelRotation));
      int y2 = (int) ((double) local2.Y + 4.0 + 24.0 * Math.Sin((double) wheelRotation));
      Utility.drawLineWithScreenCoordinates(x1, y1, x2, y2, b, new Color(112, 98, 92), (float) (((double) globalPosition.Y + 264.0) / 10000.0));
      Utility.drawLineWithScreenCoordinates(x1, y1 + 2, x2, y2 + 2, b, new Color(112, 98, 92), (float) (((double) globalPosition.Y + 264.0) / 10000.0));
      Utility.drawLineWithScreenCoordinates(x1, y1 + 4, x2, y2 + 4, b, new Color(53, 46, 43), (float) (((double) globalPosition.Y + 264.0) / 10000.0));
      Utility.drawLineWithScreenCoordinates(x1, y1 + 6, x2, y2 + 6, b, new Color(53, 46, 43), (float) (((double) globalPosition.Y + 264.0) / 10000.0));
      b.Draw(Game1.mouseCursors, new Vector2((float) (x1 - 8), (float) (y1 - 8)), new Rectangle?(new Rectangle(192, 640, 24, 24)), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, (float) (((double) globalPosition.Y + 268.0) / 10000.0));
      b.Draw(Game1.mouseCursors, new Vector2((float) (x2 - 8), (float) (y2 - 8)), new Rectangle?(new Rectangle(192, 640, 24, 24)), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, (float) (((double) globalPosition.Y + 268.0) / 10000.0));
    }
  }
}
