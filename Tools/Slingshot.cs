// Decompiled with JetBrains decompiler
// Type: StardewValley.Tools.Slingshot
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.Projectiles;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace StardewValley.Tools
{
  public class Slingshot : Tool
  {
    public const int basicDamage = 5;
    public const int basicSlingshot = 32;
    public const int masterSlingshot = 33;
    public const int galaxySlingshot = 34;
    public const int drawBackSoundThreshold = 8;
    [XmlIgnore]
    public int recentClickX;
    [XmlIgnore]
    public int recentClickY;
    [XmlIgnore]
    public int lastClickX;
    [XmlIgnore]
    public int lastClickY;
    [XmlIgnore]
    public int mouseDragAmount;
    private bool canPlaySound;
    private bool startedWithGamePad;

    public Slingshot()
    {
      this.initialParentTileIndex = 32;
      this.currentParentTileIndex = this.initialParentTileIndex;
      this.indexOfMenuItemView = this.currentParentTileIndex;
      this.name = Game1.content.Load<Dictionary<int, string>>("Data\\weapons")[this.initialParentTileIndex].Split('/')[0];
      this.numAttachmentSlots = 1;
      this.attachments = new StardewValley.Object[1];
    }

    protected override string loadDisplayName()
    {
      string[] strArray = Game1.content.Load<Dictionary<int, string>>("Data\\weapons")[this.initialParentTileIndex].Split('/');
      if (LocalizedContentManager.CurrentLanguageCode != LocalizedContentManager.LanguageCode.en)
        return strArray[strArray.Length - 1];
      return this.name;
    }

    protected override string loadDescription()
    {
      return Game1.content.Load<Dictionary<int, string>>("Data\\weapons")[this.initialParentTileIndex].Split('/')[1];
    }

    public override bool doesShowTileLocationMarker()
    {
      return false;
    }

    public Slingshot(int which = 32)
    {
      this.initialParentTileIndex = which;
      this.currentParentTileIndex = this.initialParentTileIndex;
      this.indexOfMenuItemView = this.currentParentTileIndex;
      this.name = Game1.content.Load<Dictionary<int, string>>("Data\\weapons")[this.initialParentTileIndex].Split('/')[0];
      this.numAttachmentSlots = 1;
      this.attachments = new StardewValley.Object[1];
    }

    public bool didStartWithGamePad()
    {
      return this.startedWithGamePad;
    }

    public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
    {
      this.indexOfMenuItemView = this.initialParentTileIndex;
      who.usingSlingshot = false;
      who.canReleaseTool = true;
      who.usingTool = false;
      who.canMove = true;
      if (this.attachments[0] != null)
      {
        StardewValley.Object one = (StardewValley.Object) this.attachments[0].getOne();
        --this.attachments[0].Stack;
        if (this.attachments[0].Stack <= 0)
          this.attachments[0] = (StardewValley.Object) null;
        int num1 = Game1.getOldMouseX() + Game1.viewport.X;
        int num2 = Game1.getOldMouseY() + Game1.viewport.Y;
        if (this.startedWithGamePad)
        {
          Point point = Utility.Vector2ToPoint(Game1.player.getStandingPosition() + new Vector2(Game1.oldPadState.ThumbSticks.Left.X, -Game1.oldPadState.ThumbSticks.Left.Y) * (float) Game1.tileSize * 4f);
          num1 = point.X;
          num2 = point.Y;
        }
        int num3 = Math.Min(20, (int) Vector2.Distance(new Vector2((float) who.getStandingX(), (float) (who.getStandingY() - Game1.tileSize)), new Vector2((float) num1, (float) num2)) / 20);
        Vector2 velocityTowardPoint = Utility.getVelocityTowardPoint(new Point(who.getStandingX(), who.getStandingY() + Game1.tileSize), new Vector2((float) num1, (float) (num2 + Game1.tileSize)), (float) (15 + Game1.random.Next(4, 6)) * (1f + who.weaponSpeedModifier));
        int num4 = 4;
        if (num3 > num4 && !this.canPlaySound)
        {
          int num5 = 1;
          BasicProjectile.onCollisionBehavior collisionBehavior = (BasicProjectile.onCollisionBehavior) null;
          string collisionSound = "hammer";
          float num6 = 1f;
          if (this.initialParentTileIndex == 33)
            num6 = 2f;
          else if (this.initialParentTileIndex == 34)
            num6 = 4f;
          switch (one.ParentSheetIndex)
          {
            case 378:
              num5 = 10;
              ++one.ParentSheetIndex;
              break;
            case 380:
              num5 = 20;
              ++one.ParentSheetIndex;
              break;
            case 382:
              num5 = 15;
              ++one.ParentSheetIndex;
              break;
            case 384:
              num5 = 30;
              ++one.ParentSheetIndex;
              break;
            case 386:
              num5 = 50;
              ++one.ParentSheetIndex;
              break;
            case 388:
              num5 = 2;
              ++one.ParentSheetIndex;
              break;
            case 390:
              num5 = 5;
              ++one.ParentSheetIndex;
              break;
            case 441:
              num5 = 20;
              collisionBehavior = new BasicProjectile.onCollisionBehavior(BasicProjectile.explodeOnImpact);
              collisionSound = "explosion";
              break;
          }
          if (one.category == -5)
            collisionSound = "slimedead";
          List<Projectile> projectiles = location.projectiles;
          BasicProjectile basicProjectile = new BasicProjectile((int) ((double) num6 * (double) (num5 + Game1.random.Next(-(num5 / 2), num5 + 2)) * (1.0 + (double) who.attackIncreaseModifier)), one.ParentSheetIndex, 0, 0, (float) (Math.PI / (64.0 + (double) Game1.random.Next(-63, 64))), -velocityTowardPoint.X, -velocityTowardPoint.Y, new Vector2((float) (who.getStandingX() - 16), (float) (who.getStandingY() - Game1.tileSize - 8)), collisionSound, "", false, true, (Character) who, true, collisionBehavior);
          int num7 = Game1.currentLocation.currentEvent != null ? 1 : 0;
          basicProjectile.ignoreLocationCollision = num7 != 0;
          projectiles.Add((Projectile) basicProjectile);
        }
      }
      else
        Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Slingshot.cs.14254"));
      this.canPlaySound = true;
      who.Halt();
    }

    public override bool canThisBeAttached(StardewValley.Object o)
    {
      return o == null || !o.bigCraftable && (o.parentSheetIndex >= 378 && o.parentSheetIndex <= 390 || (o.category == -5 || o.category == -79) || (o.category == -75 || o.parentSheetIndex == 441));
    }

    public override StardewValley.Object attach(StardewValley.Object o)
    {
      StardewValley.Object attachment = this.attachments[0];
      this.attachments[0] = o;
      Game1.playSound("button1");
      return attachment;
    }

    public override string getHoverBoxText(Item hoveredItem)
    {
      if (hoveredItem != null && hoveredItem is StardewValley.Object && this.canThisBeAttached(hoveredItem as StardewValley.Object))
        return Game1.content.LoadString("Strings\\StringsFromCSFiles:Slingshot.cs.14256", (object) this.DisplayName, (object) hoveredItem.DisplayName);
      if (hoveredItem != null || this.attachments == null || this.attachments[0] == null)
        return (string) null;
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:Slingshot.cs.14258", (object) this.attachments[0].DisplayName);
    }

    public override bool onRelease(GameLocation location, int x, int y, Farmer who)
    {
      this.DoFunction(location, x, y, 1, who);
      return true;
    }

    public override bool beginUsing(GameLocation location, int x, int y, Farmer who)
    {
      who.usingSlingshot = true;
      who.canReleaseTool = false;
      this.mouseDragAmount = 0;
      int num1 = who.FacingDirection == 3 || who.FacingDirection == 1 ? 1 : (who.FacingDirection == 0 ? 2 : 0);
      who.FarmerSprite.setCurrentFrame(42 + num1);
      double num2 = (double) (Game1.getOldMouseX() + Game1.viewport.X - who.getStandingX());
      double num3 = (double) (Game1.getOldMouseY() + Game1.viewport.Y - who.getStandingY());
      double num4;
      double num5;
      if (Math.Abs(num2) > Math.Abs(num3))
      {
        num4 = num2 / Math.Abs(num2);
        num5 = 0.5;
      }
      else
      {
        num5 = num3 / Math.Abs(num3);
        num4 = 0.0;
      }
      double num6 = num4 * 16.0;
      double num7 = num5 * 16.0;
      if (this.didStartWithGamePad())
      {
        Mouse.SetPosition(who.getStandingX() - Game1.viewport.X + (int) num6, who.getStandingY() - Game1.viewport.Y + (int) num7);
        Game1.lastCursorMotionWasMouse = false;
      }
      Game1.oldMouseState = Mouse.GetState();
      Game1.lastMousePositionBeforeFade = Game1.getMousePosition();
      this.lastClickX = Game1.getOldMouseX() + Game1.viewport.X;
      this.lastClickY = Game1.getOldMouseY() + Game1.viewport.Y;
      this.startedWithGamePad = false;
      if (Game1.options.gamepadControls && GamePad.GetState(Game1.playerOneIndex).IsButtonDown(Buttons.X))
        this.startedWithGamePad = true;
      return true;
    }

    public override void tickUpdate(GameTime time, Farmer who)
    {
      if (!who.usingSlingshot)
        return;
      Point point = Game1.getMousePosition();
      if (this.startedWithGamePad)
      {
        point = Utility.Vector2ToPoint(Game1.player.getStandingPosition() + new Vector2(Game1.oldPadState.ThumbSticks.Left.X, -Game1.oldPadState.ThumbSticks.Left.Y) * (float) Game1.tileSize * 4f);
        point.X -= Game1.viewport.X;
        point.Y -= Game1.viewport.Y;
      }
      int num1 = point.X + Game1.viewport.X;
      int num2 = point.Y + Game1.viewport.Y;
      Game1.debugOutput = "playerPos: " + Game1.player.getStandingPosition().ToString() + ", mousePos: " + (object) num1 + ", " + (object) num2;
      this.mouseDragAmount = this.mouseDragAmount + 1;
      who.faceGeneralDirection(new Vector2((float) num1, (float) num2), 0);
      who.faceDirection((who.FacingDirection + 2) % 4);
      int num3 = who.FacingDirection == 3 || who.FacingDirection == 1 ? 1 : (who.FacingDirection == 0 ? 2 : 0);
      who.FarmerSprite.setCurrentFrame(42 + num3);
      if (this.canPlaySound && (Math.Abs(num1 - this.lastClickX) > 8 || Math.Abs(num2 - this.lastClickY) > 8) && this.mouseDragAmount > 4)
      {
        Game1.playSound("slingshot");
        this.canPlaySound = false;
      }
      this.lastClickX = num1;
      this.lastClickY = num2;
      Game1.mouseCursor = -1;
    }

    public override void drawAttachments(SpriteBatch b, int x, int y)
    {
      if (this.attachments[0] == null)
      {
        b.Draw(Game1.menuTexture, new Vector2((float) x, (float) y), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 43, -1, -1)), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.86f);
      }
      else
      {
        b.Draw(Game1.menuTexture, new Vector2((float) x, (float) y), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.menuTexture, 10, -1, -1)), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.86f);
        this.attachments[0].drawInMenu(b, new Vector2((float) x, (float) y), 1f);
      }
    }

    public override void draw(SpriteBatch b)
    {
      if (!Game1.player.usingSlingshot)
        return;
      int num1 = Game1.getOldMouseX() + Game1.viewport.X;
      int num2 = Game1.getOldMouseY() + Game1.viewport.Y;
      if (this.startedWithGamePad)
      {
        Point point = Utility.Vector2ToPoint(Game1.player.getStandingPosition() + new Vector2(Game1.oldPadState.ThumbSticks.Left.X, -Game1.oldPadState.ThumbSticks.Left.Y) * (float) Game1.tileSize * 4f);
        num1 = point.X;
        num2 = point.Y;
      }
      Vector2 velocityTowardPoint = Utility.getVelocityTowardPoint(new Point(Game1.player.getStandingX(), Game1.player.getStandingY() + Game1.tileSize / 2), new Vector2((float) num1, (float) num2), 256f);
      if ((double) Math.Abs(velocityTowardPoint.X) < 1.0)
      {
        int mouseDragAmount = this.mouseDragAmount;
      }
      double num3 = Math.Sqrt((double) velocityTowardPoint.X * (double) velocityTowardPoint.X + (double) velocityTowardPoint.Y * (double) velocityTowardPoint.Y) - 181.0;
      double num4 = (double) velocityTowardPoint.X / 256.0;
      double num5 = (double) velocityTowardPoint.Y / 256.0;
      int num6 = (int) ((double) velocityTowardPoint.X - num3 * num4);
      int num7 = (int) ((double) velocityTowardPoint.Y - num3 * num5);
      b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (Game1.player.getStandingX() - num6), (float) (Game1.player.getStandingY() - Game1.tileSize - 8 - num7))), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 43, -1, -1)), Color.White, 0.0f, new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2)), 1f, SpriteEffects.None, 0.999999f);
    }

    public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, bool drawStackNumber)
    {
      if (this.indexOfMenuItemView == 0 || this.indexOfMenuItemView == 21 || (this.indexOfMenuItemView == 47 || this.currentParentTileIndex == 47))
      {
        string name = this.name;
        if (!(name == nameof (Slingshot)))
        {
          if (!(name == "Master Slingshot"))
          {
            if (name == "Galaxy Slingshot")
              this.currentParentTileIndex = 34;
          }
          else
            this.currentParentTileIndex = 33;
        }
        else
          this.currentParentTileIndex = 32;
        this.indexOfMenuItemView = this.currentParentTileIndex;
      }
      spriteBatch.Draw(Tool.weaponsTexture, location + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 3 + 8)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Tool.weaponsTexture, this.indexOfMenuItemView, 16, 16)), Color.White * transparency, 0.0f, new Vector2(8f, 8f), scaleSize * (float) Game1.pixelZoom, SpriteEffects.None, layerDepth);
      if (!drawStackNumber || this.attachments == null || this.attachments[0] == null)
        return;
      Utility.drawTinyDigits(this.attachments[0].Stack, spriteBatch, location + new Vector2((float) (Game1.tileSize - Utility.getWidthOfTinyDigitString(this.attachments[0].Stack, 3f * scaleSize)) + 3f * scaleSize, (float) ((double) Game1.tileSize - 18.0 * (double) scaleSize + 2.0)), 3f * scaleSize, 1f, Color.White);
    }
  }
}
