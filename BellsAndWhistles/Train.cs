// Decompiled with JetBrains decompiler
// Type: StardewValley.BellsAndWhistles.Train
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Monsters;
using System;
using System.Collections.Generic;

namespace StardewValley.BellsAndWhistles
{
  public class Train
  {
    public List<TrainCar> cars = new List<TrainCar>();
    public const int minCars = 8;
    public const int maxCars = 24;
    public const double chanceForLongTrain = 0.1;
    public const int randomTrain = 0;
    public const int jojaTrain = 1;
    public const int coalTrain = 2;
    public const int passengerTrain = 3;
    public const int uniformColorPlainTrain = 4;
    public const int prisonTrain = 5;
    public const int christmasTrain = 6;
    public int type;
    public float position;
    public float speed;
    public float wheelRotation;
    public float smokeTimer;
    private TemporaryAnimatedSprite whistleSteam;

    public Train()
    {
      Random random = new Random();
      this.type = random.NextDouble() >= 0.1 ? (random.NextDouble() >= 0.1 ? (random.NextDouble() >= 0.1 ? (random.NextDouble() >= 0.05 ? (!Game1.currentSeason.ToLower().Equals("winter") || random.NextDouble() >= 0.2 ? 0 : 6) : 5) : 2) : 1) : 3;
      int num1 = random.Next(8, 25);
      if (random.NextDouble() < 0.1)
        num1 *= 2;
      this.speed = 0.2f;
      this.smokeTimer = this.speed * 2000f;
      Color color1 = Color.White;
      double num2 = 1.0;
      double num3 = 1.0;
      switch (this.type)
      {
        case 0:
          num2 = 0.2;
          num3 = 0.2;
          break;
        case 1:
          num2 = 0.0;
          num3 = 0.0;
          color1 = Color.DimGray;
          break;
        case 2:
          num2 = 0.0;
          num3 = 0.7;
          break;
        case 3:
          num2 = 1.0;
          num3 = 0.0;
          this.speed = 0.4f;
          break;
        case 5:
          num3 = 0.0;
          num2 = 0.0;
          color1 = Color.MediumBlue;
          this.speed = 0.4f;
          break;
        case 6:
          num2 = 0.0;
          num3 = 1.0;
          color1 = Color.Red;
          break;
      }
      this.cars.Add(new TrainCar(random, 3, -1, Color.White, 0, 0));
      for (int index = 1; index < num1; ++index)
      {
        int carType = 0;
        if (random.NextDouble() < num2)
          carType = 2;
        else if (random.NextDouble() < num3)
          carType = 1;
        Color color2 = color1;
        if (color1.Equals(Color.White))
        {
          bool flag1 = false;
          bool flag2 = false;
          bool flag3 = false;
          switch (random.Next(3))
          {
            case 0:
              flag1 = true;
              break;
            case 1:
              flag2 = true;
              break;
            case 2:
              flag3 = true;
              break;
          }
          color2 = new Color(random.Next(flag1 ? 0 : 100, 250), random.Next(flag2 ? 0 : 100, 250), random.Next(flag3 ? 0 : 100, 250));
        }
        int frontDecal = -1;
        if (this.type == 1)
          frontDecal = 2;
        else if (this.type == 5)
          frontDecal = 1;
        else if (this.type == 6)
          frontDecal = -1;
        else if (random.NextDouble() < 0.3)
          frontDecal = random.Next(35);
        int resourceType = 0;
        if (carType == 1)
        {
          resourceType = random.Next(9);
          if (this.type == 6)
            resourceType = 9;
        }
        this.cars.Add(new TrainCar(random, carType, frontDecal, color2, resourceType, random.Next(1, 3)));
      }
    }

    public Rectangle getBoundingBox()
    {
      return new Rectangle(-this.cars.Count * 128 * 4 + (int) this.position, 45 * Game1.tileSize - Game1.tileSize * 2 - 32, this.cars.Count * 128 * 4, Game1.tileSize * 2);
    }

    public bool Update(GameTime time, GameLocation location)
    {
      this.position = this.position + (float) time.ElapsedGameTime.Milliseconds * this.speed;
      this.wheelRotation = this.wheelRotation + (float) time.ElapsedGameTime.Milliseconds * ((float) Math.PI / 256f);
      this.wheelRotation = this.wheelRotation % 6.283185f;
      foreach (Farmer farmer1 in location.getFarmers())
      {
        Rectangle boundingBox = farmer1.GetBoundingBox();
        if (boundingBox.Intersects(this.getBoundingBox()))
        {
          farmer1.xVelocity = 8f;
          Farmer farmer2 = farmer1;
          boundingBox = this.getBoundingBox();
          int y1 = boundingBox.Center.Y;
          boundingBox = farmer1.GetBoundingBox();
          int y2 = boundingBox.Center.Y;
          double num = (double) (y1 - y2) / 4.0;
          farmer2.yVelocity = (float) num;
          Game1.farmerTakeDamage(20, true, (Monster) null);
          if (farmer1.usingTool)
            Game1.playSound("clank");
        }
      }
      if (Game1.random.NextDouble() < 0.001 && location.Equals((object) Game1.currentLocation))
      {
        Game1.playSound("trainWhistle");
        this.whistleSteam = new TemporaryAnimatedSprite(27, new Vector2(this.position - 250f, (float) (45 * Game1.tileSize - Game1.tileSize * 4)), Color.White, 8, false, 100f, 0, Game1.tileSize, 1f, Game1.tileSize, 0);
      }
      if (this.whistleSteam != null)
      {
        this.whistleSteam.Position = new Vector2(this.position - 258f, (float) (45 * Game1.tileSize - Game1.tileSize * 4 - 32));
        if (this.whistleSteam.update(time))
          this.whistleSteam = (TemporaryAnimatedSprite) null;
      }
      this.smokeTimer = this.smokeTimer - (float) time.ElapsedGameTime.Milliseconds;
      if ((double) this.smokeTimer <= 0.0)
      {
        location.temporarySprites.Add(new TemporaryAnimatedSprite(25, new Vector2(this.position - 170f, (float) (45 * Game1.tileSize - Game1.tileSize * 6)), Color.White, 8, false, 100f, 0, Game1.tileSize, 1f, Game1.tileSize * 2, 0));
        this.smokeTimer = this.speed * 2000f;
      }
      return (double) this.position > (double) (this.cars.Count * 128 * 4 + 70 * Game1.tileSize);
    }

    public void draw(SpriteBatch b)
    {
      for (int index = 0; index < this.cars.Count; ++index)
        this.cars[index].draw(b, new Vector2(this.position - (float) ((index + 1) * 512), (float) (45 * Game1.tileSize - Game1.tileSize * 4 - 32)), this.wheelRotation);
      if (this.whistleSteam == null)
        return;
      this.whistleSteam.draw(b, false, 0, 0);
    }
  }
}
