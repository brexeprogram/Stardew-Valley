// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.ChatBox
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Menus
{
  public class ChatBox : IClickableMenu
  {
    private List<ChatMessage> messages = new List<ChatMessage>();
    public int maxMessages = 10;
    public const int errorMessage = 0;
    public const int userNotificationMessage = -1;
    public const int blankMessage = -2;
    public const int defaultMaxMessages = 10;
    public const int timeToDisplayMessages = 600;
    public TextBox chatBox;
    private TextBoxEvent e;
    private KeyboardDispatcher keyboardDispatcher;

    public ChatBox()
      : base(Game1.viewport.Width / 2 - Game1.tileSize * 12 / 2 - Game1.tileSize, Game1.viewport.Height - Game1.tileSize * 2 - Game1.tileSize / 2, Game1.tileSize * 14, 56, false)
    {
      this.chatBox = new TextBox(Game1.content.Load<Texture2D>("LooseSprites\\chatBox"), (Texture2D) null, Game1.smallFont, Color.White);
      this.e = new TextBoxEvent(this.textBoxEnter);
      this.chatBox.OnEnterPressed += this.e;
      this.keyboardDispatcher = Game1.keyboardDispatcher;
      this.keyboardDispatcher.Subscriber = (IKeyboardSubscriber) this.chatBox;
      this.chatBox.X = this.xPositionOnScreen;
      this.chatBox.Y = this.yPositionOnScreen;
      this.chatBox.Width = this.width;
      this.chatBox.Height = 56;
      this.chatBox.Selected = false;
    }

    public void textBoxEnter(TextBox sender)
    {
      if (Game1.IsMultiplayer && sender.Text.Length > 0)
      {
        string message = sender.Text.Trim();
        if (message.Length < 1)
          return;
        if ((int) message[0] == 47)
        {
          if (message.Split(' ')[0].Length > 1)
          {
            try
            {
              string s = message.Split(' ')[0].Substring(1);
              // ISSUE: reference to a compiler-generated method
              uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(s);
              if (stringHash <= 1013213428U)
              {
                if ((int) stringHash != 355814093)
                {
                  if ((int) stringHash != 405908334)
                  {
                    if ((int) stringHash == 1013213428 && s == "texture")
                    {
                      Game1.player.Sprite.Texture = Game1.content.Load<Texture2D>("Characters\\" + message.Split(' ')[1]);
                      goto label_26;
                    }
                    else
                      goto label_21;
                  }
                  else if (!(s == "nick"))
                    goto label_21;
                }
                else if (!(s == "nickname"))
                  goto label_21;
              }
              else if (stringHash <= 2180167635U)
              {
                if ((int) stringHash != 1158129075)
                {
                  if ((int) stringHash != -2114799661 || !(s == "rename"))
                    goto label_21;
                }
                else if (s == "othergirl")
                {
                  Game1.otherFarmers.Values.ElementAt<Farmer>(0).Sprite.Texture = Game1.content.Load<Texture2D>("Characters\\farmergirl");
                  goto label_26;
                }
                else
                  goto label_21;
              }
              else if ((int) stringHash != -1925595674)
              {
                if ((int) stringHash == -1571474013 && s == "girl")
                {
                  Game1.player.Sprite.Texture = Game1.content.Load<Texture2D>("Characters\\farmergirl");
                  Game1.player.isMale = false;
                  goto label_26;
                }
                else
                  goto label_21;
              }
              else if (!(s == "name"))
                goto label_21;
              MultiplayerUtility.sendNameChange(message.Substring(message.IndexOf(' ') + 1), Game1.player.uniqueMultiplayerID);
              goto label_26;
label_21:
              this.receiveChatMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:ChatBox.cs.10261"), 0L);
              goto label_26;
            }
            catch (Exception ex)
            {
              this.receiveChatMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:ChatBox.cs.10262"), 0L);
              goto label_26;
            }
          }
        }
        MultiplayerUtility.sendChatMessage(message, Game1.player.uniqueMultiplayerID);
        if (Game1.IsServer)
          this.receiveChatMessage(message, Game1.player.uniqueMultiplayerID);
      }
label_26:
      sender.Text = "";
      this.clickAway();
    }

    public override void clickAway()
    {
      base.clickAway();
      this.chatBox.Selected = false;
      Game1.isChatting = false;
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
    }

    public void receiveChatMessage(string message, long who)
    {
      string name = Game1.player.name;
      using (Dictionary<long, Farmer>.ValueCollection.Enumerator enumerator = Game1.otherFarmers.Values.GetEnumerator())
      {
        if (enumerator.MoveNext())
        {
          Farmer current = enumerator.Current;
          if (current.uniqueMultiplayerID == who)
            name = current.name;
        }
      }
      string str = name + ":";
      if (who == 0L)
        str = "::";
      else if (who == -1L)
        str = ">";
      else if (who == -2L)
        str = "";
      ChatMessage chatMessage = new ChatMessage();
      chatMessage.message = Game1.parseText(str + " " + message, this.chatBox.Font, Game1.viewport.Width / 2 - Game1.tileSize * 12 / 2 - Game1.tileSize);
      chatMessage.timeLeftToDisplay = 600;
      chatMessage.verticalSize = (int) this.chatBox.Font.MeasureString(chatMessage.message).Y;
      this.messages.Add(chatMessage);
      if (this.messages.Count <= this.maxMessages)
        return;
      this.messages.RemoveAt(0);
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
    }

    public override void performHoverAction(int x, int y)
    {
    }

    public void update()
    {
      for (int index = 0; index < this.messages.Count; ++index)
      {
        if (this.messages[index].timeLeftToDisplay > 0)
          --this.messages[index].timeLeftToDisplay;
        if (this.messages[index].timeLeftToDisplay < 75)
          this.messages[index].alpha = (float) this.messages[index].timeLeftToDisplay / 75f;
      }
      if (!this.chatBox.Selected)
        return;
      foreach (ChatMessage message in this.messages)
        message.alpha = 1f;
    }

    public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
    {
      this.yPositionOnScreen = newBounds.Height - Game1.tileSize * 2 - Game1.tileSize / 2;
      this.chatBox.Y = this.yPositionOnScreen;
      this.chatBox.X = Game1.viewport.Width / 2 - Game1.tileSize * 12 / 2 - Game1.tileSize;
      this.chatBox.Width = Game1.tileSize * 14;
      this.width = Game1.tileSize * 14;
      this.chatBox.Height = 56;
    }

    public override void draw(SpriteBatch b)
    {
      int num = 0;
      for (int index = this.messages.Count - 1; index >= 0; --index)
      {
        num += this.messages[index].verticalSize;
        b.DrawString(this.chatBox.Font, this.messages[index].message, new Vector2(4f, (float) (Game1.viewport.Height - num - 8)), this.chatBox.TextColor * this.messages[index].alpha, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.99f);
      }
      if (this.chatBox.Selected)
        this.chatBox.Draw(b);
      this.update();
    }
  }
}
