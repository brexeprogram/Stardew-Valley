// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.StrengthGame
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace StardewValley.Menus
{
  public class StrengthGame : IClickableMenu
  {
    private float transparency = 1f;
    private float power;
    private float changeSpeed;
    private float endTimer;
    private Color barColor;
    private bool victorySound;
    private bool clicked;
    private bool showedResult;

    public StrengthGame()
      : base(31 * Game1.tileSize + 6 * Game1.pixelZoom, 56 * Game1.tileSize + 10 * Game1.pixelZoom, 5 * Game1.pixelZoom, 34 * Game1.pixelZoom, false)
    {
      this.power = 0.0f;
      this.changeSpeed = (float) (3 + Game1.random.Next(2));
      this.barColor = Color.Red;
      Game1.playSound("cowboy_monsterhit");
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
      if (!this.clicked)
      {
        Game1.player.faceDirection(1);
        Game1.player.CurrentToolIndex = 107;
        Game1.player.FarmerSprite.animateOnce(168, 80f, 8);
        Game1.player.toolOverrideFunction = new AnimatedSprite.endOfAnimationBehavior(this.afterSwingAnimation);
        Game1.player.FarmerSprite.ignoreDefaultActionThisTime = false;
        this.clicked = true;
      }
      if (this.showedResult && Game1.dialogueTyping)
        Game1.currentDialogueCharacterIndex = Game1.currentObjectDialogue.Peek().Length - 1;
      if (!this.showedResult || Game1.dialogueTyping)
        return;
      Game1.player.toolOverrideFunction = (AnimatedSprite.endOfAnimationBehavior) null;
      Game1.exitActiveMenu();
      Game1.afterDialogues = (Game1.afterFadeFunction) null;
      Game1.pressActionButton(Game1.oldKBState, Game1.oldMouseState, Game1.oldPadState);
    }

    public void afterSwingAnimation(Farmer who)
    {
      if (!Game1.isFestival())
      {
        who.toolOverrideFunction = (AnimatedSprite.endOfAnimationBehavior) null;
      }
      else
      {
        this.changeSpeed = 0.0f;
        Game1.playSound("hammer");
        Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(46, new Vector2(30f, 56f) * (float) Game1.tileSize, Color.White, 8, false, 100f, 0, -1, -1f, -1, 0));
        if ((double) this.power >= 99.0)
          this.endTimer = 2000f;
        else
          this.endTimer = 1000f;
      }
    }

    public override void receiveKeyPress(Keys key)
    {
      base.receiveKeyPress(key);
    }

    public override void update(GameTime time)
    {
      base.update(time);
      if ((double) this.changeSpeed == 0.0)
      {
        this.endTimer = this.endTimer - (float) time.ElapsedGameTime.Milliseconds;
        if ((double) this.power >= 99.0)
        {
          if ((double) this.endTimer < 1500.0)
          {
            if (!this.victorySound)
            {
              this.victorySound = true;
              Game1.playSound("getNewSpecialItem");
              this.barColor = Color.Orange;
            }
            if (!this.showedResult && Game1.random.NextDouble() < 0.08)
              Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(10 + Game1.random.Next(2), new Vector2(31f, 55f) * (float) Game1.tileSize + new Vector2((float) Game1.random.Next(-Game1.tileSize, Game1.tileSize), (float) Game1.random.Next(-Game1.tileSize, Game1.tileSize)), Color.Yellow, 8, false, 100f, 0, -1, -1f, -1, 0)
              {
                layerDepth = 1f
              });
          }
        }
        else
          this.transparency = Math.Max(0.0f, this.transparency - 0.02f);
        if ((double) this.endTimer > 0.0 || this.showedResult)
          return;
        this.showedResult = true;
        if ((double) this.power >= 99.0)
        {
          ++Game1.player.festivalScore;
          Game1.playSound("purchase");
          Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11660"));
        }
        else if ((double) this.power >= 2.0)
        {
          string str = "";
          switch (this.power)
          {
            case 2f:
            case 3f:
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11701");
              break;
            case 4f:
            case 5f:
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11700");
              break;
            case 6f:
            case 7f:
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11699");
              break;
            case 8f:
            case 9f:
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11698");
              break;
            case 10f:
            case 11f:
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11697");
              break;
            case 12f:
            case 13f:
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11696");
              break;
            case 14f:
            case 15f:
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11695");
              break;
            case 16f:
            case 17f:
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11694");
              break;
            case 18f:
            case 19f:
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11693");
              break;
            case 20f:
            case 21f:
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11692");
              break;
            case 22f:
            case 23f:
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11691");
              break;
            case 24f:
            case 25f:
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11690");
              break;
            case 26f:
            case 27f:
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11689");
              break;
            case 28f:
            case 29f:
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11688");
              break;
            case 30f:
            case 31f:
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11687");
              break;
            case 32f:
            case 33f:
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11686");
              break;
            case 34f:
            case 35f:
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11685");
              break;
            case 36f:
            case 37f:
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11684");
              break;
            case 38f:
            case 39f:
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11683");
              break;
            case 40f:
            case 41f:
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11682");
              break;
            case 42f:
            case 43f:
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11681");
              break;
            case 44f:
            case 45f:
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11680");
              break;
            case 46f:
            case 47f:
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11679");
              break;
            case 48f:
            case 49f:
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11678");
              break;
            case 50f:
            case 51f:
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11677");
              break;
            case 52f:
            case 53f:
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11676");
              break;
            case 54f:
            case 55f:
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11675");
              break;
            case 56f:
            case 57f:
            case 58f:
            case 59f:
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11674");
              break;
            case 60f:
            case 61f:
            case 62f:
            case 63f:
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11673");
              break;
            case 64f:
            case 65f:
            case 66f:
            case 67f:
            case 68f:
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11672");
              break;
            case 69f:
            case 70f:
            case 71f:
            case 72f:
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11671");
              break;
            case 73f:
            case 74f:
            case 75f:
            case 76f:
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11670");
              break;
            case 77f:
            case 78f:
            case 79f:
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11669");
              break;
            case 80f:
            case 81f:
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11668");
              break;
            case 82f:
            case 83f:
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11667");
              break;
            case 84f:
            case 85f:
            case 86f:
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11666");
              break;
            case 87f:
            case 89f:
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11665");
              break;
            case 88f:
            case 90f:
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11664");
              break;
            case 91f:
            case 92f:
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11663");
              break;
            case 93f:
            case 94f:
            case 95f:
            case 96f:
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11662");
              break;
            case 97f:
            case 98f:
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11661");
              break;
          }
          Game1.playSound("dwop");
          Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11703", (object) str));
        }
        else
        {
          ++Game1.player.festivalScore;
          Game1.playSound("purchase");
          Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:StrengthGame.cs.11705")));
        }
        Game1.afterDialogues = new Game1.afterFadeFunction(((IClickableMenu) this).exitThisMenuNoSound);
      }
      else
      {
        this.power = this.power + this.changeSpeed;
        if ((double) this.power > 100.0)
        {
          this.power = 100f;
          this.changeSpeed = -this.changeSpeed;
        }
        else
        {
          if ((double) this.power >= 0.0)
            return;
          this.power = 0.0f;
          this.changeSpeed = -this.changeSpeed;
        }
      }
    }

    public override void performHoverAction(int x, int y)
    {
    }

    public override void draw(SpriteBatch b)
    {
      if (!Game1.dialogueUp)
        b.Draw(Game1.staminaRect, Game1.GlobalToLocal(Game1.viewport, new Rectangle(this.xPositionOnScreen, (int) ((double) this.yPositionOnScreen - (double) this.power / 100.0 * (double) this.height), this.width, (int) ((double) this.power / 100.0 * (double) this.height))), new Rectangle?(Game1.staminaRect.Bounds), this.barColor * this.transparency, 0.0f, Vector2.Zero, SpriteEffects.None, 1E-05f);
      if (!Game1.player.FarmerSprite.isOnToolAnimation())
        return;
      Game1.drawTool(Game1.player, Game1.player.CurrentToolIndex);
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
    }
  }
}
