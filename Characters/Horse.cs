// Decompiled with JetBrains decompiler
// Type: StardewValley.Characters.Horse
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace StardewValley.Characters
{
  public class Horse : NPC
  {
    [XmlIgnore]
    public Farmer rider;
    [XmlIgnore]
    public bool mounting;
    [XmlIgnore]
    public bool dismounting;
    private Vector2 dismountTile;
    private bool squeezingThroughGate;

    public Horse()
    {
      this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Animals\\horse"), 0, 32, 32);
      this.breather = false;
      this.willDestroyObjectsUnderfoot = false;
      this.hideShadow = true;
      this.sprite.textureUsesFlippedRightForLeft = true;
      this.sprite.loop = true;
      this.drawOffset = new Vector2((float) (-Game1.tileSize / 4), 0.0f);
      this.faceDirection(3);
    }

    public Horse(int xTile, int yTile)
    {
      this.name = "";
      this.displayName = this.name;
      this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Animals\\horse"), 0, 32, 32);
      this.position = new Vector2((float) xTile, (float) yTile) * (float) Game1.tileSize;
      this.breather = false;
      this.willDestroyObjectsUnderfoot = false;
      this.currentLocation = Game1.currentLocation;
      this.hideShadow = true;
      this.sprite.textureUsesFlippedRightForLeft = true;
      this.sprite.loop = true;
      this.drawOffset = new Vector2((float) (-Game1.tileSize / 4), 0.0f);
      this.faceDirection(3);
    }

    public override void reloadSprite()
    {
    }

    public override void dayUpdate(int dayOfMonth)
    {
      this.faceDirection(3);
    }

    public override Rectangle GetBoundingBox()
    {
      Rectangle boundingBox = base.GetBoundingBox();
      if (this.squeezingThroughGate && (this.facingDirection == 0 || this.facingDirection == 2))
        boundingBox.Inflate(-Game1.tileSize / 2 - Game1.pixelZoom, 0);
      return boundingBox;
    }

    public override bool canPassThroughActionTiles()
    {
      return false;
    }

    public void squeezeForGate()
    {
      this.squeezingThroughGate = true;
      if (this.rider == null)
        return;
      this.rider.temporaryImpassableTile = this.GetBoundingBox();
    }

    public override void update(GameTime time, GameLocation location)
    {
      this.squeezingThroughGate = false;
      this.faceTowardFarmer = false;
      this.faceTowardFarmerTimer = -1;
      base.update(time, location);
      this.flip = this.facingDirection == 3;
      if (this.mounting)
      {
        if ((double) this.rider.position.X < (double) (this.GetBoundingBox().X + Game1.tileSize / 4 - Game1.pixelZoom))
          this.rider.position.X += (float) Game1.pixelZoom;
        else if ((double) this.rider.position.X > (double) (this.GetBoundingBox().X + Game1.tileSize / 4 + Game1.pixelZoom))
          this.rider.position.X -= (float) Game1.pixelZoom;
        if (this.rider.getStandingY() < this.GetBoundingBox().Y - Game1.pixelZoom)
          this.rider.position.Y += (float) Game1.pixelZoom;
        else if (this.rider.getStandingY() > this.GetBoundingBox().Y + Game1.pixelZoom)
          this.rider.position.Y -= (float) Game1.pixelZoom;
        if (this.rider.yJumpOffset < -8 || (double) this.rider.yJumpVelocity > 0.0)
          return;
        this.sprite.loop = true;
        this.rider.mountUp(this);
        this.rider.freezePause = -1;
        this.mounting = false;
        this.rider.canMove = true;
        if (this.facingDirection != 1)
          return;
        this.rider.xOffset += 8f;
      }
      else if (this.dismounting)
      {
        if ((double) Math.Abs(this.rider.position.X - this.dismountTile.X * (float) Game1.tileSize + (float) (Game1.tileSize / 2) - (float) (this.rider.GetBoundingBox().Width / 2)) > (double) Game1.pixelZoom)
        {
          if ((double) this.rider.position.X < (double) this.dismountTile.X * (double) Game1.tileSize + (double) (Game1.tileSize / 2) - (double) (this.rider.GetBoundingBox().Width / 2))
            this.rider.position.X += Math.Min((float) Game1.pixelZoom, this.dismountTile.X * (float) Game1.tileSize + (float) (Game1.tileSize / 2) - (float) (this.rider.GetBoundingBox().Width / 2) - this.rider.position.X);
          else if ((double) this.rider.position.X > (double) this.dismountTile.X * (double) Game1.tileSize + (double) (Game1.tileSize / 2) - (double) (this.rider.GetBoundingBox().Width / 2))
            this.rider.position.X += Math.Max((float) -Game1.pixelZoom, this.dismountTile.X * (float) Game1.tileSize + (float) (Game1.tileSize / 2) - (float) (this.rider.GetBoundingBox().Width / 2) - this.rider.position.X);
        }
        if ((double) Math.Abs(this.rider.position.Y - this.dismountTile.Y * (float) Game1.tileSize + (float) Game1.pixelZoom) > (double) Game1.pixelZoom)
        {
          if ((double) this.rider.position.Y < (double) this.dismountTile.Y * (double) Game1.tileSize + (double) Game1.pixelZoom)
            this.rider.position.Y += Math.Min((float) Game1.pixelZoom, this.dismountTile.Y * (float) Game1.tileSize + (float) Game1.pixelZoom - this.rider.position.Y);
          else if ((double) this.rider.position.Y > (double) this.dismountTile.Y * (double) Game1.tileSize + (double) Game1.pixelZoom)
            this.rider.position.Y += Math.Max((float) -Game1.pixelZoom, this.dismountTile.Y * (float) Game1.tileSize + (float) Game1.pixelZoom - this.rider.position.Y);
        }
        if (this.rider.yJumpOffset < 0 || (double) this.rider.yJumpVelocity > 0.0)
          return;
        this.rider.position.Y += (float) (Game1.pixelZoom * 2);
        int num = 0;
        while (this.rider.currentLocation.isCollidingPosition(this.rider.GetBoundingBox(), Game1.viewport, true, 0, false, (Character) this.rider) && num < 6)
        {
          ++num;
          this.rider.position.Y -= (float) Game1.pixelZoom;
        }
        if (num == 6)
        {
          this.rider.position = this.position;
          this.dismounting = false;
          this.rider.freezePause = -1;
          this.rider.canMove = true;
        }
        else
          this.dismount();
      }
      else if (this.rider != null)
      {
        this.rider.xOffset = -6f;
        this.drawOffset = new Vector2((float) (-Game1.tileSize / 4), 0.0f);
        switch (this.facingDirection)
        {
          case 0:
            this.rider.FarmerSprite.setCurrentSingleFrame(113, (short) 32000, false, false);
            break;
          case 1:
            this.rider.FarmerSprite.setCurrentSingleFrame(106, (short) 32000, false, false);
            this.rider.xOffset += 2f;
            break;
          case 2:
            this.rider.FarmerSprite.setCurrentSingleFrame(107, (short) 32000, false, false);
            break;
          case 3:
            this.rider.FarmerSprite.setCurrentSingleFrame(106, (short) 32000, false, true);
            this.drawOffset = Vector2.Zero;
            this.rider.xOffset = -12f;
            break;
        }
        this.rider.facingDirection = this.facingDirection;
        if (this.rider.isMoving())
        {
          switch (this.sprite.currentAnimationIndex)
          {
            case 0:
              this.rider.yOffset = 0.0f;
              break;
            case 1:
              this.rider.yOffset = (float) -Game1.pixelZoom;
              break;
            case 2:
              this.rider.yOffset = (float) -Game1.pixelZoom;
              break;
            case 3:
              this.rider.yOffset = 0.0f;
              break;
            case 4:
              this.rider.yOffset = (float) Game1.pixelZoom;
              break;
            case 5:
              this.rider.yOffset = (float) Game1.pixelZoom;
              break;
          }
        }
        else
          this.rider.yOffset = 0.0f;
      }
      else
      {
        if (this.facingDirection == 2 || this.sprite.currentAnimation != null || Game1.random.NextDouble() >= 0.002)
          return;
        this.sprite.loop = false;
        switch (this.facingDirection)
        {
          case 0:
            this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
            {
              new FarmerSprite.AnimationFrame(25, Game1.random.Next(250, 750)),
              new FarmerSprite.AnimationFrame(14, 10)
            });
            break;
          case 1:
            this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
            {
              new FarmerSprite.AnimationFrame(21, 100),
              new FarmerSprite.AnimationFrame(22, 100),
              new FarmerSprite.AnimationFrame(23, 400),
              new FarmerSprite.AnimationFrame(24, 400),
              new FarmerSprite.AnimationFrame(23, 400),
              new FarmerSprite.AnimationFrame(24, 400),
              new FarmerSprite.AnimationFrame(23, 400),
              new FarmerSprite.AnimationFrame(24, 400),
              new FarmerSprite.AnimationFrame(23, 400),
              new FarmerSprite.AnimationFrame(22, 100),
              new FarmerSprite.AnimationFrame(21, 100)
            });
            break;
          case 3:
            this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
            {
              new FarmerSprite.AnimationFrame(21, 100, false, true, (AnimatedSprite.endOfAnimationBehavior) null, false),
              new FarmerSprite.AnimationFrame(22, 100, false, true, (AnimatedSprite.endOfAnimationBehavior) null, false),
              new FarmerSprite.AnimationFrame(23, 100, false, true, (AnimatedSprite.endOfAnimationBehavior) null, false),
              new FarmerSprite.AnimationFrame(24, 400, false, true, (AnimatedSprite.endOfAnimationBehavior) null, false),
              new FarmerSprite.AnimationFrame(23, 400, false, true, (AnimatedSprite.endOfAnimationBehavior) null, false),
              new FarmerSprite.AnimationFrame(24, 400, false, true, (AnimatedSprite.endOfAnimationBehavior) null, false),
              new FarmerSprite.AnimationFrame(23, 400, false, true, (AnimatedSprite.endOfAnimationBehavior) null, false),
              new FarmerSprite.AnimationFrame(24, 400, false, true, (AnimatedSprite.endOfAnimationBehavior) null, false),
              new FarmerSprite.AnimationFrame(23, 400, false, true, (AnimatedSprite.endOfAnimationBehavior) null, false),
              new FarmerSprite.AnimationFrame(22, 100, false, true, (AnimatedSprite.endOfAnimationBehavior) null, false),
              new FarmerSprite.AnimationFrame(21, 100, false, true, (AnimatedSprite.endOfAnimationBehavior) null, false)
            });
            break;
        }
      }
    }

    public override void collisionWithFarmerBehavior()
    {
      base.collisionWithFarmerBehavior();
    }

    public void dismount()
    {
      this.rider.dismount();
      this.rider.temporaryImpassableTile = new Rectangle((int) this.dismountTile.X * Game1.tileSize, (int) this.dismountTile.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
      this.rider.freezePause = -1;
      this.dismounting = false;
      this.rider.canMove = true;
      this.rider.forceCanMove();
      this.rider.xOffset = 0.0f;
      this.rider = (Farmer) null;
      this.Halt();
      this.farmerPassesThrough = false;
    }

    public void nameHorse(string name)
    {
      if (name.Length <= 0)
        return;
      foreach (NPC allCharacter in Utility.getAllCharacters())
      {
        if (allCharacter.isVillager() && allCharacter.name.Equals(name))
          name += " ";
      }
      this.name = name;
      this.displayName = name;
      Game1.exitActiveMenu();
      Game1.playSound("newArtifact");
    }

    public override bool checkAction(Farmer who, GameLocation l)
    {
      if (this.rider == null)
      {
        if (this.name.Length <= 0)
        {
          Game1.activeClickableMenu = (IClickableMenu) new NamingMenu(new NamingMenu.doneNamingBehavior(this.nameHorse), Game1.content.LoadString("Strings\\Characters:NameYourHorse"), Game1.content.LoadString("Strings\\Characters:DefaultHorseName"));
          return true;
        }
        this.rider = who;
        this.rider.freezePause = 5000;
        this.rider.yJumpVelocity = 6f;
        this.rider.yJumpOffset = -1;
        this.rider.faceGeneralDirection(Utility.PointToVector2(this.GetBoundingBox().Center), 0);
        this.rider.showNotCarrying();
        this.rider.Halt();
        if ((double) this.rider.position.X < (double) this.position.X)
          this.rider.faceDirection(1);
        Game1.playSound("dwop");
        this.mounting = true;
        return true;
      }
      this.dismounting = true;
      this.farmerPassesThrough = false;
      this.rider.temporaryImpassableTile = Rectangle.Empty;
      Vector2 tileForCharacter = Utility.recursiveFindOpenTileForCharacter((Character) this.rider, this.rider.currentLocation, this.rider.getTileLocation(), 8);
      this.dismounting = false;
      this.Halt();
      if (!tileForCharacter.Equals(Vector2.Zero) && (double) Vector2.Distance(tileForCharacter, this.rider.getTileLocation()) < 2.0)
      {
        this.rider.yJumpVelocity = 6f;
        this.rider.yJumpOffset = -1;
        Game1.playSound("dwop");
        this.rider.freezePause = 5000;
        this.rider.Halt();
        this.rider.xOffset = 0.0f;
        this.dismounting = true;
        this.dismountTile = tileForCharacter;
        Game1.debugOutput = "dismount tile: " + tileForCharacter.ToString();
      }
      else
        this.dismount();
      return true;
    }

    public override void draw(SpriteBatch b)
    {
      base.draw(b);
      if (this.facingDirection != 2 || this.rider == null)
        return;
      b.Draw(this.sprite.Texture, this.getLocalPosition(Game1.viewport) + new Vector2((float) (Game1.tileSize * 3 / 4), (float) (-Game1.tileSize / 2 + Game1.pixelZoom * 2) - this.rider.yOffset), new Rectangle?(new Rectangle(160, 96, 9, 15)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, (float) (((double) this.position.Y + (double) Game1.tileSize) / 10000.0));
    }
  }
}
