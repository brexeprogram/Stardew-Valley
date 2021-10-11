// Decompiled with JetBrains decompiler
// Type: StardewValley.Characters.Pet
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Locations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Characters
{
  public class Pet : NPC
  {
    public const int bedTime = 2000;
    public const int maxFriendship = 1000;
    public const int behavior_walking = 0;
    public const int behavior_Sleep = 1;
    public const int behavior_Sit_Down = 2;
    public const int frame_basicSit = 18;
    private int currentBehavior;
    private bool wasPetToday;
    public int friendshipTowardFarmer;
    private int pushingTimer;

    public int CurrentBehavior
    {
      get
      {
        return this.currentBehavior;
      }
      set
      {
        this.currentBehavior = value;
        this.initiateCurrentBehavior();
      }
    }

    public override void behaviorOnFarmerLocationEntry(GameLocation location, Farmer who)
    {
      if ((location is Farm || location is FarmHouse && this.currentBehavior != 1) && Game1.timeOfDay >= 2000)
      {
        if (this.currentBehavior == 1 && !(this.currentLocation is Farm))
          return;
        this.warpToFarmHouse(who);
      }
      else
      {
        if (Game1.timeOfDay >= 2000 || Game1.random.NextDouble() >= 0.5)
          return;
        this.CurrentBehavior = 1;
      }
    }

    public override void reloadSprite()
    {
      this.DefaultPosition = new Vector2(54f, 8f) * (float) Game1.tileSize;
      this.hideShadow = true;
      this.breather = false;
      this.setAtFarmPosition();
    }

    public void warpToFarmHouse(Farmer who)
    {
      FarmHouse homeOfFarmer = Utility.getHomeOfFarmer(who);
      Vector2 vector2 = Vector2.Zero;
      int num = 0;
      for (vector2 = new Vector2((float) Game1.random.Next(2, homeOfFarmer.map.Layers[0].LayerWidth - 3), (float) Game1.random.Next(3, homeOfFarmer.map.Layers[0].LayerHeight - 5)); num < 50 && (!homeOfFarmer.isTileLocationTotallyClearAndPlaceable(vector2) || !homeOfFarmer.isTileLocationTotallyClearAndPlaceable(vector2 + new Vector2(1f, 0.0f)) || homeOfFarmer.isTileOnWall((int) vector2.X, (int) vector2.Y)); ++num)
        vector2 = new Vector2((float) Game1.random.Next(2, homeOfFarmer.map.Layers[0].LayerWidth - 3), (float) Game1.random.Next(3, homeOfFarmer.map.Layers[0].LayerHeight - 4));
      if (num >= 50)
        return;
      Game1.warpCharacter((NPC) this, "FarmHouse", vector2, false, false);
      Game1.getFarm().characters.Remove((NPC) this);
      this.currentBehavior = 1;
      this.initiateCurrentBehavior();
    }

    public override void dayUpdate(int dayOfMonth)
    {
      this.DefaultPosition = new Vector2(54f, 8f) * (float) Game1.tileSize;
      this.sprite.loop = false;
      this.breather = false;
      if (Game1.isRaining)
      {
        this.CurrentBehavior = 2;
        if (this.currentLocation is Farm)
          this.warpToFarmHouse(Game1.player);
      }
      else if (this.currentLocation is FarmHouse)
        this.setAtFarmPosition();
      if (this.currentLocation is Farm)
      {
        if (this.currentLocation.getTileIndexAt(54, 7, "Buildings") == 1939)
          this.friendshipTowardFarmer = Math.Min(1000, this.friendshipTowardFarmer + 6);
        this.currentLocation.setMapTileIndex(54, 7, 1938, "Buildings", 0);
        this.setTilePosition(54, 8);
        this.position.X -= (float) Game1.tileSize;
      }
      this.Halt();
      this.CurrentBehavior = 1;
      this.wasPetToday = false;
    }

    public void setAtFarmPosition()
    {
      bool flag = this.currentLocation is Farm;
      if (Game1.isRaining)
        return;
      this.faceDirection(2);
      this.currentLocation.characters.Remove((NPC) this);
      Game1.warpCharacter((NPC) this, "Farm", new Vector2(54f, 8f), false, false);
      this.position.X -= (float) Game1.tileSize;
    }

    public override bool shouldCollideWithBuildingLayer(GameLocation location)
    {
      return true;
    }

    public override bool canPassThroughActionTiles()
    {
      return false;
    }

    public override bool checkAction(Farmer who, GameLocation l)
    {
      if (this.wasPetToday)
        return false;
      this.wasPetToday = true;
      this.friendshipTowardFarmer = Math.Min(1000, this.friendshipTowardFarmer + 12);
      if (this.friendshipTowardFarmer >= 1000 && who != null && !who.mailReceived.Contains("petLoveMessage"))
      {
        Game1.showGlobalMessage(Game1.content.LoadString("Strings\\Characters:PetLovesYou", (object) this.displayName));
        who.mailReceived.Add("petLoveMessage");
      }
      this.doEmote(20, true);
      this.playContentSound();
      return true;
    }

    public virtual void playContentSound()
    {
    }

    public void hold(Farmer who)
    {
      this.flip = this.sprite.currentAnimation.Last<FarmerSprite.AnimationFrame>().flip;
      this.sprite.CurrentFrame = this.sprite.currentAnimation.Last<FarmerSprite.AnimationFrame>().frame;
      this.sprite.currentAnimation = (List<FarmerSprite.AnimationFrame>) null;
      this.sprite.loop = false;
    }

    public override void behaviorOnFarmerPushing()
    {
      if (this is Dog && (this as Dog).currentBehavior == 51)
        return;
      this.pushingTimer = this.pushingTimer + 2;
      if (this.pushingTimer <= 100)
        return;
      Vector2 playerTrajectory = Utility.getAwayFromPlayerTrajectory(this.GetBoundingBox());
      this.setTrajectory((int) playerTrajectory.X / 2, (int) playerTrajectory.Y / 2);
      this.pushingTimer = 0;
      this.Halt();
      this.facePlayer(Game1.player);
      this.facingDirection = this.facingDirection + 2;
      this.facingDirection = this.facingDirection % 4;
      this.faceDirection(this.facingDirection);
      this.CurrentBehavior = 0;
    }

    public override void update(GameTime time, GameLocation location, long id, bool move)
    {
      base.update(time, location, id, move);
      this.pushingTimer = Math.Max(0, this.pushingTimer - 1);
    }

    public virtual void initiateCurrentBehavior()
    {
      this.flip = false;
      bool flip1 = false;
      switch (this.currentBehavior)
      {
        case 0:
          this.Halt();
          this.faceDirection(Game1.random.Next(4));
          this.setMovingInFacingDirection();
          break;
        case 1:
          this.sprite.loop = true;
          bool flip2 = Game1.random.NextDouble() < 0.5;
          this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
          {
            new FarmerSprite.AnimationFrame(28, 1000, false, flip2, (AnimatedSprite.endOfAnimationBehavior) null, false),
            new FarmerSprite.AnimationFrame(29, 1000, false, flip2, (AnimatedSprite.endOfAnimationBehavior) null, false)
          });
          break;
        case 2:
          this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
          {
            new FarmerSprite.AnimationFrame(16, 100, false, flip1, (AnimatedSprite.endOfAnimationBehavior) null, false),
            new FarmerSprite.AnimationFrame(17, 100, false, flip1, (AnimatedSprite.endOfAnimationBehavior) null, false),
            new FarmerSprite.AnimationFrame(18, 100, false, flip1, new AnimatedSprite.endOfAnimationBehavior(this.hold), false)
          });
          break;
      }
    }

    public override Rectangle GetBoundingBox()
    {
      return new Rectangle((int) this.position.X + Game1.tileSize / 4, (int) this.position.Y + Game1.tileSize / 4, this.sprite.spriteWidth * Game1.pixelZoom * 3 / 4, Game1.tileSize / 2);
    }

    public override void draw(SpriteBatch b)
    {
      base.draw(b);
      if (!this.IsEmoting)
        return;
      Vector2 localPosition = this.getLocalPosition(Game1.viewport);
      localPosition.X += (float) (Game1.tileSize / 2);
      localPosition.Y -= (float) (Game1.tileSize * 3 / 2 + (this is Dog ? Game1.tileSize / 4 : 0));
      b.Draw(Game1.emoteSpriteSheet, localPosition, new Rectangle?(new Rectangle(this.CurrentEmoteIndex * 16 % Game1.emoteSpriteSheet.Width, this.CurrentEmoteIndex * 16 / Game1.emoteSpriteSheet.Width * 16, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, (float) ((double) this.getStandingY() / 10000.0 + 9.99999974737875E-05));
    }
  }
}
