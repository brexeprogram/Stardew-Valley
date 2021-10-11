// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.CooperativeMenu
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Network;
using System;

namespace StardewValley.Menus
{
  public class CooperativeMenu : IClickableMenu
  {
    private string connectionError = "";
    public const int timeOutForServerFind = 5000;
    public const byte joinOrHostScreen = 0;
    public const byte joinGameScreen = 1;
    public const byte hostGameScreen = 2;
    private KeyboardDispatcher keyboardDispatcher;
    private TextBox textBox;
    private ClickableComponent joinGame;
    private ClickableComponent hostGame;
    private byte currentView;
    private bool attemptingConnectToServer;
    private long endTimeForConnectionAttempt;
    private TextBoxEvent e;

    public CooperativeMenu()
      : base(Game1.viewport.Width / 2 - Game1.viewport.Width / 5, (int) ((double) Game1.viewport.Height * 0.200000002980232 + (double) Game1.tileSize), (int) ((double) Game1.viewport.Width * 0.400000005960464), (int) ((double) Game1.viewport.Height * 0.200000002980232), false)
    {
      this.keyboardDispatcher = Game1.keyboardDispatcher;
      this.textBox = new TextBox((Texture2D) null, (Texture2D) null, Game1.dialogueFont, Color.Black);
      this.textBox.X = this.xPositionOnScreen;
      this.textBox.Y = this.yPositionOnScreen;
      this.textBox.Width = Game1.tileSize * 6;
      this.textBox.Height = Game1.tileSize * 3;
      this.e = new TextBoxEvent(this.textBoxEnter);
      this.textBox.OnEnterPressed += this.e;
      this.keyboardDispatcher.Subscriber = (IKeyboardSubscriber) this.textBox;
      this.joinGame = new ClickableComponent(new Rectangle(this.xPositionOnScreen, this.height + Game1.tileSize, this.width * 2 / 3, this.height), Game1.content.LoadString("Strings\\StringsFromCSFiles:CooperativeMenu.cs.10318"));
      this.hostGame = new ClickableComponent(new Rectangle(this.xPositionOnScreen, this.height * 2 + Game1.tileSize, this.width * 2 / 3, this.height), Game1.content.LoadString("Strings\\StringsFromCSFiles:CooperativeMenu.cs.10319"));
    }

    public void textBoxEnter(TextBox sender)
    {
      if ((int) this.currentView == 1)
      {
        this.connectionError = "";
        this.attemptingConnectToServer = true;
        Game1.client = (Client) new LidgrenClient();
        Game1.client.initializeConnection(sender.Text);
        this.endTimeForConnectionAttempt = DateTime.Now.Ticks / 10000L + 5000L;
        Game1.playSound("phone");
      }
      else
      {
        if ((int) this.currentView != 2)
          return;
        this.textBox.OnEnterPressed -= this.e;
        Game1.multiplayerMode = (byte) 2;
        Game1.server = (Server) new LidgrenServer(sender.Text);
        Game1.fadeIn = false;
        Game1.fadeToBlack = true;
        Game1.fadeToBlackAlpha = 0.99f;
        Game1.playSound("select");
        Game1.exitActiveMenu();
      }
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
      if ((int) this.currentView == 0)
      {
        if (this.joinGame.containsPoint(x, y))
        {
          this.currentView = (byte) 1;
          this.textBox.Text = "";
          Game1.playSound("smallSelect");
        }
        else if (this.hostGame.containsPoint(x, y))
        {
          this.currentView = (byte) 2;
          this.textBox.Text = "";
          Game1.playSound("smallSelect");
        }
      }
      else
        this.textBox.Update();
      this.isWithinBounds(x, y);
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
      if ((int) this.currentView == 0)
        return;
      this.currentView = (byte) 0;
    }

    public override void performHoverAction(int x, int y)
    {
      this.joinGame.scale = !this.joinGame.containsPoint(x, y) ? Math.Max(this.joinGame.scale - 1f, 1f) : Math.Min(this.joinGame.scale + 1f, 8f);
      if (this.hostGame.containsPoint(x, y))
        this.hostGame.scale = Math.Min(this.hostGame.scale + 1f, 8f);
      else
        this.hostGame.scale = Math.Max(this.hostGame.scale - 1f, 1f);
    }

    public void updateConnection()
    {
      if (!Game1.client.hasHandshaked && DateTime.Now.Ticks / 10000L < this.endTimeForConnectionAttempt)
        Game1.client.receiveMessages();
      else if (!Game1.client.hasHandshaked)
      {
        this.connectionError = "No response";
        this.attemptingConnectToServer = false;
      }
      else
      {
        this.textBox.OnEnterPressed -= this.e;
        Game1.multiplayerMode = (byte) 1;
        Game1.fadeIn = false;
        Game1.fadeToBlack = true;
        Game1.fadeToBlackAlpha = 0.99f;
        Game1.playSound("select");
        Game1.exitActiveMenu();
      }
    }

    public override void draw(SpriteBatch b)
    {
      if (this.attemptingConnectToServer)
      {
        this.updateConnection();
        int tilePosition = 20 + (int) (DateTime.Now.Ticks / 10000L % 1000L / 250L);
        b.Draw(Game1.animations, new Vector2((float) (this.textBox.X + this.textBox.Width + Game1.tileSize / 2), (float) (this.yPositionOnScreen - Game1.tileSize / 4)), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.animations, tilePosition, -1, -1)), Color.White);
      }
      if ((int) this.currentView == 0)
      {
        Game1.drawDialogueBox(this.joinGame.bounds.X - (int) this.joinGame.scale, this.joinGame.bounds.Y - Game1.tileSize, this.joinGame.bounds.Width + (int) this.joinGame.scale, this.joinGame.bounds.Height + Game1.tileSize, false, true, (string) null, false);
        b.DrawString(Game1.dialogueFont, Game1.content.LoadString("Strings\\StringsFromCSFiles:CooperativeMenu.cs.10326"), new Vector2((float) (this.xPositionOnScreen + Game1.tileSize * 5 / 3), (float) (this.joinGame.bounds.Y + Game1.tileSize * 5 / 4)), Game1.textColor);
        Game1.drawDialogueBox(this.hostGame.bounds.X - (int) this.hostGame.scale, this.hostGame.bounds.Y - Game1.tileSize, this.hostGame.bounds.Width + (int) this.hostGame.scale, this.hostGame.bounds.Height + Game1.tileSize, false, true, (string) null, false);
        b.DrawString(Game1.dialogueFont, Game1.content.LoadString("Strings\\StringsFromCSFiles:CooperativeMenu.cs.10327"), new Vector2((float) (this.xPositionOnScreen + Game1.tileSize * 5 / 3), (float) (this.hostGame.bounds.Y + Game1.tileSize * 5 / 4)), Game1.textColor);
      }
      else if ((int) this.currentView == 2)
      {
        this.textBox.Draw(b);
      }
      else
      {
        if ((int) this.currentView != 1)
          return;
        this.textBox.Draw(b);
        b.DrawString(Game1.dialogueFont, this.connectionError, new Vector2((float) this.xPositionOnScreen, (float) (this.yPositionOnScreen + Game1.tileSize)), Color.Red);
      }
    }
  }
}
