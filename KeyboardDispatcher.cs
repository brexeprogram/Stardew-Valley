// Decompiled with JetBrains decompiler
// Type: StardewValley.KeyboardDispatcher
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading;
using System.Windows;

namespace StardewValley
{
  public class KeyboardDispatcher
  {
    private string _pasteResult = "";
    private IKeyboardSubscriber _subscriber;

    public KeyboardDispatcher(GameWindow window)
    {
      KeyboardInput.Initialize(window);
      KeyboardInput.CharEntered += new CharEnteredHandler(this.EventInput_CharEntered);
      KeyboardInput.KeyDown += new KeyEventHandler(this.EventInput_KeyDown);
    }

    private void Event_KeyDown(object sender, Keys key)
    {
      if (this._subscriber == null)
        return;
      if (key == Keys.Back)
        this._subscriber.RecieveCommandInput('\b');
      if (key == Keys.Enter)
        this._subscriber.RecieveCommandInput('\r');
      if (key == Keys.Tab)
        this._subscriber.RecieveCommandInput('\t');
      this._subscriber.RecieveSpecialInput(key);
    }

    private void EventInput_KeyDown(object sender, KeyEventArgs e)
    {
      if (this._subscriber == null)
        return;
      this._subscriber.RecieveSpecialInput(e.KeyCode);
    }

    private void EventInput_CharEntered(object sender, CharacterEventArgs e)
    {
      if (this._subscriber == null)
        return;
      if (char.IsControl(e.Character))
      {
        if ((int) e.Character == 22)
        {
          Thread thread = new Thread(new ThreadStart(this.PasteThread));
          int num = 0;
          thread.SetApartmentState((ApartmentState) num);
          thread.Start();
          thread.Join();
          this._subscriber.RecieveTextInput(this._pasteResult);
        }
        else
          this._subscriber.RecieveCommandInput(e.Character);
      }
      else
        this._subscriber.RecieveTextInput(e.Character);
    }

    public IKeyboardSubscriber Subscriber
    {
      get
      {
        return this._subscriber;
      }
      set
      {
        if (this._subscriber == value)
          return;
        if (this._subscriber != null)
          this._subscriber.Selected = false;
        this._subscriber = value;
        if (this._subscriber == null)
          return;
        this._subscriber.Selected = true;
      }
    }

    [STAThread]
    private void PasteThread()
    {
      if (Clipboard.ContainsText())
        this._pasteResult = Clipboard.GetText();
      else
        this._pasteResult = "";
    }
  }
}
