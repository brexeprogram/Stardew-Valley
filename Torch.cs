// Decompiled with JetBrains decompiler
// Type: StardewValley.Torch
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using System;

namespace StardewValley
{
  public class Torch : Object
  {
    private Vector2[] ashes = new Vector2[3];
    public const float yVelocity = 1f;
    public const float yDissapearLevel = -100f;
    public const double ashChance = 0.015;
    private float color;

    public Torch()
    {
    }

    public Torch(Vector2 tileLocation, int initialStack)
      : base(tileLocation, 93, initialStack)
    {
      this.boundingBox = new Rectangle((int) tileLocation.X * Game1.tileSize, (int) tileLocation.Y * Game1.tileSize, Game1.tileSize / 4, Game1.tileSize / 4);
    }

    public Torch(Vector2 tileLocation, int initialStack, int index)
      : base(tileLocation, index, initialStack)
    {
      this.boundingBox = new Rectangle((int) tileLocation.X * Game1.tileSize, (int) tileLocation.Y * Game1.tileSize, Game1.tileSize / 4, Game1.tileSize / 4);
    }

    public Torch(Vector2 tileLocation, int index, bool bigCraftable)
      : base(tileLocation, index, false)
    {
      this.boundingBox = new Rectangle((int) tileLocation.X * Game1.tileSize, (int) tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
    }

    public override Item getOne()
    {
      if (this.bigCraftable)
      {
        Torch torch = new Torch(this.tileLocation, this.parentSheetIndex, true);
        int num = this.isRecipe ? 1 : 0;
        torch.isRecipe = num != 0;
        return (Item) torch;
      }
      Torch torch1 = new Torch(this.tileLocation, 1);
      int num1 = this.isRecipe ? 1 : 0;
      torch1.isRecipe = num1 != 0;
      return (Item) torch1;
    }

    public override void actionOnPlayerEntry()
    {
      base.actionOnPlayerEntry();
      if (!this.bigCraftable || !this.isOn)
        return;
      AmbientLocationSounds.addSound(this.tileLocation, 1);
    }

    public override bool checkForAction(Farmer who, bool justCheckingForActivity = false)
    {
      if (!this.bigCraftable)
        return base.checkForAction(who, justCheckingForActivity);
      if (justCheckingForActivity)
        return true;
      this.isOn = !this.isOn;
      if (this.isOn)
      {
        if (this.bigCraftable)
        {
          if (who != null)
            Game1.playSound("fireball");
          this.initializeLightSource(this.tileLocation);
          AmbientLocationSounds.addSound(this.tileLocation, 1);
        }
      }
      else if (this.bigCraftable)
      {
        this.performRemoveAction(this.tileLocation, Game1.currentLocation);
        if (who != null)
          Game1.playSound("woodyHit");
      }
      return true;
    }

    public override bool placementAction(GameLocation location, int x, int y, Farmer who)
    {
      Vector2 vector2 = new Vector2((float) (x / Game1.tileSize), (float) (y / Game1.tileSize));
      Torch torch = this.bigCraftable ? new Torch(vector2, this.parentSheetIndex, true) : new Torch(vector2, 1, this.parentSheetIndex);
      if (this.bigCraftable)
        torch.isOn = false;
      torch.tileLocation = vector2;
      torch.initializeLightSource(vector2);
      location.objects.Add(vector2, (Object) torch);
      if (who != null)
        Game1.playSound("woodyStep");
      return true;
    }

    public override void DayUpdate(GameLocation location)
    {
      base.DayUpdate(location);
    }

    public override bool isPassable()
    {
      return !this.bigCraftable;
    }

    public override void updateWhenCurrentLocation(GameTime time)
    {
      base.updateWhenCurrentLocation(time);
      this.updateAshes((int) ((double) this.tileLocation.X * 2000.0 + (double) this.tileLocation.Y));
    }

    public override void actionWhenBeingHeld(Farmer who)
    {
      base.actionWhenBeingHeld(who);
    }

    private void updateAshes(int identifier)
    {
      if (!Utility.isOnScreen(this.tileLocation * (float) Game1.tileSize, 4 * Game1.tileSize))
        return;
      for (int index = this.ashes.Length - 1; index >= 0; --index)
      {
        Vector2 ash = this.ashes[index];
        ash.Y -= (float) (1.0 * ((double) (index + 1) * 0.25));
        if (index % 2 != 0)
          ash.X += (float) Math.Sin((double) this.ashes[index].Y / (2.0 * Math.PI)) / 2f;
        this.ashes[index] = ash;
        if (Game1.random.NextDouble() < 3.0 / 400.0 && (double) this.ashes[index].Y < -100.0)
          this.ashes[index] = new Vector2((float) (Game1.random.Next(-1, 3) * Game1.pixelZoom) * 0.75f, 0.0f);
      }
      this.color = Math.Max(-0.8f, Math.Min(0.7f, this.color + this.ashes[0].Y / 1200f));
    }

    public override void performRemoveAction(Vector2 tileLocation, GameLocation environment)
    {
      AmbientLocationSounds.removeSound(this.tileLocation);
      if (this.bigCraftable)
        this.isOn = false;
      base.performRemoveAction(this.tileLocation, environment);
    }

    public override void draw(SpriteBatch spriteBatch, int xNonTile, int yNonTile, float layerDepth, float alpha = 1f)
    {
      Rectangle sourceRectForObject = Game1.currentLocation.getSourceRectForObject(this.ParentSheetIndex);
      sourceRectForObject.Y += 8;
      sourceRectForObject.Height /= 2;
      spriteBatch.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) xNonTile, (float) (yNonTile + Game1.tileSize / 2))), new Rectangle?(sourceRectForObject), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, layerDepth);
      sourceRectForObject.X = 276 + (int) ((Game1.currentGameTime.TotalGameTime.TotalMilliseconds + (double) (xNonTile * 320) + (double) (yNonTile * 49)) % 700.0 / 100.0) * 8;
      sourceRectForObject.Y = 1965;
      sourceRectForObject.Width = 8;
      sourceRectForObject.Height = 8;
      spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (xNonTile + Game1.tileSize / 2 + Game1.pixelZoom), (float) (yNonTile + Game1.tileSize / 4 + Game1.pixelZoom))), new Rectangle?(sourceRectForObject), Color.White * 0.75f, 0.0f, new Vector2(4f, 4f), (float) (Game1.pixelZoom * 3 / 4), SpriteEffects.None, layerDepth + 1E-05f);
      spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (xNonTile + Game1.tileSize / 2 + Game1.pixelZoom), (float) (yNonTile + Game1.tileSize / 4 + Game1.pixelZoom))), new Rectangle?(new Rectangle(88, 1779, 30, 30)), Color.PaleGoldenrod * (Game1.currentLocation.IsOutdoors ? 0.35f : 0.43f), 0.0f, new Vector2(15f, 15f), (float) (Game1.pixelZoom * 2) + (float) ((double) (Game1.tileSize / 2) * Math.Sin((Game1.currentGameTime.TotalGameTime.TotalMilliseconds + (double) (xNonTile * 777) + (double) (yNonTile * 9746)) % 3140.0 / 1000.0) / 50.0), SpriteEffects.None, 1f);
    }

    public override void draw(SpriteBatch spriteBatch, int x, int y, float alpha = 1f)
    {
      if (Game1.eventUp && !Game1.currentLocation.IsFarm)
        return;
      if (!this.bigCraftable)
      {
        Rectangle sourceRectForObject = Game1.currentLocation.getSourceRectForObject(this.ParentSheetIndex);
        sourceRectForObject.Y += 8;
        sourceRectForObject.Height /= 2;
        SpriteBatch spriteBatch1 = spriteBatch;
        Texture2D objectSpriteSheet = Game1.objectSpriteSheet;
        Vector2 local1 = Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (x * Game1.tileSize), (float) (y * Game1.tileSize + Game1.tileSize / 2)));
        Rectangle? sourceRectangle1 = new Rectangle?(sourceRectForObject);
        Color white = Color.White;
        double num1 = 0.0;
        Vector2 zero = Vector2.Zero;
        Vector2 scale = this.scale;
        double num2 = (double) this.scale.Y > 1.0 ? (double) this.getScale().Y : (double) Game1.pixelZoom;
        int num3 = 0;
        double num4 = (double) this.getBoundingBox(new Vector2((float) x, (float) y)).Bottom / 10000.0;
        spriteBatch1.Draw(objectSpriteSheet, local1, sourceRectangle1, white, (float) num1, zero, (float) num2, (SpriteEffects) num3, (float) num4);
        SpriteBatch spriteBatch2 = spriteBatch;
        Texture2D mouseCursors = Game1.mouseCursors;
        Vector2 local2 = Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (x * Game1.tileSize + Game1.tileSize / 2 + Game1.pixelZoom / 2), (float) (y * Game1.tileSize + Game1.tileSize / 4)));
        Rectangle? sourceRectangle2 = new Rectangle?(new Rectangle(88, 1779, 30, 30));
        Color color = Color.PaleGoldenrod * (Game1.currentLocation.IsOutdoors ? 0.35f : 0.43f);
        double num5 = 0.0;
        Vector2 origin = new Vector2(15f, 15f);
        double pixelZoom = (double) Game1.pixelZoom;
        double tileSize = (double) Game1.tileSize;
        TimeSpan totalGameTime = Game1.currentGameTime.TotalGameTime;
        double num6 = Math.Sin((totalGameTime.TotalMilliseconds + (double) (x * Game1.tileSize * 777) + (double) (y * Game1.tileSize * 9746)) % 3140.0 / 1000.0);
        double num7 = tileSize * num6 / 50.0;
        double num8 = pixelZoom + num7;
        int num9 = 0;
        double num10 = 1.0;
        spriteBatch2.Draw(mouseCursors, local2, sourceRectangle2, color, (float) num5, origin, (float) num8, (SpriteEffects) num9, (float) num10);
        // ISSUE: explicit reference operation
        // ISSUE: variable of a reference type
        Rectangle& local3 = @sourceRectForObject;
        int num11 = 276;
        totalGameTime = Game1.currentGameTime.TotalGameTime;
        int num12 = (int) ((totalGameTime.TotalMilliseconds + (double) (x * 3204) + (double) (y * 49)) % 700.0 / 100.0) * 8;
        int num13 = num11 + num12;
        // ISSUE: explicit reference operation
        (^local3).X = num13;
        sourceRectForObject.Y = 1965;
        sourceRectForObject.Width = 8;
        sourceRectForObject.Height = 8;
        spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (x * Game1.tileSize + Game1.tileSize / 2 + Game1.pixelZoom), (float) (y * Game1.tileSize + Game1.tileSize / 4 + Game1.pixelZoom))), new Rectangle?(sourceRectForObject), Color.White * 0.75f, 0.0f, new Vector2(4f, 4f), (float) (Game1.pixelZoom * 3 / 4), SpriteEffects.None, (float) (this.getBoundingBox(new Vector2((float) x, (float) y)).Bottom + 1) / 10000f);
        for (int index = 0; index < this.ashes.Length; ++index)
          spriteBatch.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (x * Game1.tileSize + Game1.tileSize / 2) + this.ashes[index].X, (float) (y * Game1.tileSize + Game1.tileSize / 2) + this.ashes[index].Y)), new Rectangle?(new Rectangle(344 + index % 3, 53, 1, 1)), Color.White * 0.5f * (float) ((-100.0 - (double) this.ashes[index].Y / 2.0) / -100.0), 0.0f, Vector2.Zero, (float) Game1.pixelZoom * 0.75f, SpriteEffects.None, (float) this.getBoundingBox(new Vector2((float) x, (float) y)).Bottom / 10000f);
      }
      else
      {
        base.draw(spriteBatch, x, y, alpha);
        if (!this.isOn)
          return;
        if (this.parentSheetIndex == 146)
        {
          SpriteBatch spriteBatch1 = spriteBatch;
          Texture2D mouseCursors1 = Game1.mouseCursors;
          Vector2 local1 = Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (x * Game1.tileSize + Game1.tileSize / 4 - Game1.pixelZoom), (float) (y * Game1.tileSize - Game1.pixelZoom * 2)));
          int num1 = 276;
          TimeSpan totalGameTime = Game1.currentGameTime.TotalGameTime;
          int num2 = (int) ((totalGameTime.TotalMilliseconds + (double) (x * 3047) + (double) (y * 88)) % 400.0 / 100.0) * 12;
          Rectangle? sourceRectangle1 = new Rectangle?(new Rectangle(num1 + num2, 1985, 12, 11));
          Color white1 = Color.White;
          double num3 = 0.0;
          Vector2 zero1 = Vector2.Zero;
          double num4 = (double) (Game1.pixelZoom * 3 / 4);
          int num5 = 0;
          double num6 = (double) (this.getBoundingBox(new Vector2((float) x, (float) y)).Bottom - 16) / 10000.0;
          spriteBatch1.Draw(mouseCursors1, local1, sourceRectangle1, white1, (float) num3, zero1, (float) num4, (SpriteEffects) num5, (float) num6);
          SpriteBatch spriteBatch2 = spriteBatch;
          Texture2D mouseCursors2 = Game1.mouseCursors;
          Vector2 local2 = Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (x * Game1.tileSize + Game1.tileSize / 2 - Game1.pixelZoom * 3), (float) (y * Game1.tileSize)));
          int num7 = 276;
          totalGameTime = Game1.currentGameTime.TotalGameTime;
          int num8 = (int) ((totalGameTime.TotalMilliseconds + (double) (x * 2047) + (double) (y * 98)) % 400.0 / 100.0) * 12;
          Rectangle? sourceRectangle2 = new Rectangle?(new Rectangle(num7 + num8, 1985, 12, 11));
          Color white2 = Color.White;
          double num9 = 0.0;
          Vector2 zero2 = Vector2.Zero;
          double num10 = (double) (Game1.pixelZoom * 3 / 4);
          int num11 = 0;
          double num12 = (double) (this.getBoundingBox(new Vector2((float) x, (float) y)).Bottom - 15) / 10000.0;
          spriteBatch2.Draw(mouseCursors2, local2, sourceRectangle2, white2, (float) num9, zero2, (float) num10, (SpriteEffects) num11, (float) num12);
          SpriteBatch spriteBatch3 = spriteBatch;
          Texture2D mouseCursors3 = Game1.mouseCursors;
          Vector2 local3 = Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (x * Game1.tileSize + Game1.tileSize / 2 - Game1.pixelZoom * 5), (float) (y * Game1.tileSize + Game1.pixelZoom * 3)));
          int num13 = 276;
          totalGameTime = Game1.currentGameTime.TotalGameTime;
          int num14 = (int) ((totalGameTime.TotalMilliseconds + (double) (x * 2077) + (double) (y * 98)) % 400.0 / 100.0) * 12;
          Rectangle? sourceRectangle3 = new Rectangle?(new Rectangle(num13 + num14, 1985, 12, 11));
          Color white3 = Color.White;
          double num15 = 0.0;
          Vector2 zero3 = Vector2.Zero;
          double num16 = (double) (Game1.pixelZoom * 3 / 4);
          int num17 = 0;
          double num18 = (double) (this.getBoundingBox(new Vector2((float) x, (float) y)).Bottom - 14) / 10000.0;
          spriteBatch3.Draw(mouseCursors3, local3, sourceRectangle3, white3, (float) num15, zero3, (float) num16, (SpriteEffects) num17, (float) num18);
        }
        else
          spriteBatch.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (x * Game1.tileSize + Game1.tileSize / 4 - Game1.pixelZoom * 2), (float) (y * Game1.tileSize - Game1.tileSize + Game1.pixelZoom * 2))), new Rectangle?(new Rectangle(276 + (int) ((Game1.currentGameTime.TotalGameTime.TotalMilliseconds + (double) (x * 3047) + (double) (y * 88)) % 400.0 / 100.0) * 12, 1985, 12, 11)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, (float) (this.getBoundingBox(new Vector2((float) x, (float) y)).Bottom - 16) / 10000f);
      }
    }
  }
}
