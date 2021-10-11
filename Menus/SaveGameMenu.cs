// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.SaveGameMenu
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using System;
using System.Collections.Generic;
using System.Text;

namespace StardewValley.Menus
{
  public class SaveGameMenu : IClickableMenu, IDisposable
  {
    private int completePause = -1;
    private int margin = 500;
    private StringBuilder _stringBuilder = new StringBuilder();
    private float _ellipsisDelay = 0.5f;
    private IEnumerator<int> loader;
    public bool quit;
    public bool hasDrawn;
    private SparklingText saveText;
    private int _ellipsisCount;

    public SaveGameMenu()
    {
      this.saveText = new SparklingText(Game1.dialogueFont, Game1.content.LoadString("Strings\\StringsFromCSFiles:SaveGameMenu.cs.11378"), Color.LimeGreen, Color.Black * (1f / 1000f), false, 0.1, 1500, Game1.tileSize / 2, 500);
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
    }

    public override void update(GameTime time)
    {
      if (this.quit)
        return;
      base.update(time);
      if (!Game1.saveOnNewDay)
      {
        this.quit = true;
        if (!Game1.activeClickableMenu.Equals((object) this))
          return;
        Game1.player.checkForLevelTenStatus();
        Game1.exitActiveMenu();
      }
      else
      {
        TimeSpan elapsedGameTime;
        if (this.loader != null)
        {
          this.loader.MoveNext();
          if (this.loader.Current >= 100)
          {
            int margin = this.margin;
            elapsedGameTime = time.ElapsedGameTime;
            int milliseconds = elapsedGameTime.Milliseconds;
            this.margin = margin - milliseconds;
            if (this.margin <= 0)
            {
              Game1.playSound("money");
              this.completePause = 1500;
              this.loader = (IEnumerator<int>) null;
              Game1.game1.IsSaving = false;
            }
          }
          double ellipsisDelay = (double) this._ellipsisDelay;
          elapsedGameTime = time.ElapsedGameTime;
          double totalSeconds = elapsedGameTime.TotalSeconds;
          this._ellipsisDelay = (float) (ellipsisDelay - totalSeconds);
          if ((double) this._ellipsisDelay <= 0.0)
          {
            this._ellipsisDelay = this._ellipsisDelay + 0.75f;
            this._ellipsisCount = this._ellipsisCount + 1;
            if (this._ellipsisCount > 3)
              this._ellipsisCount = 1;
          }
        }
        else if (this.hasDrawn && this.completePause == -1)
        {
          Game1.game1.IsSaving = true;
          this.loader = SaveGame.Save();
        }
        if (this.completePause < 0)
          return;
        int completePause = this.completePause;
        elapsedGameTime = time.ElapsedGameTime;
        int milliseconds1 = elapsedGameTime.Milliseconds;
        this.completePause = completePause - milliseconds1;
        this.saveText.update(time);
        if (this.completePause >= 0)
          return;
        this.quit = true;
        this.completePause = -9999;
        if (Game1.activeClickableMenu.Equals((object) this))
        {
          Game1.player.checkForLevelTenStatus();
          Game1.exitActiveMenu();
        }
        Game1.currentLocation.resetForPlayerEntry();
      }
    }

    public override void draw(SpriteBatch b)
    {
      base.draw(b);
      Vector2 vector2 = Utility.makeSafe(new Vector2((float) Game1.tileSize, (float) (Game1.viewport.Height - Game1.tileSize)), new Vector2((float) Game1.tileSize, (float) Game1.tileSize));
      if (this.completePause >= 0)
      {
        this.saveText.draw(b, vector2);
      }
      else
      {
        this._stringBuilder.Clear();
        this._stringBuilder.Append(Game1.content.LoadString("Strings\\StringsFromCSFiles:SaveGameMenu.cs.11381"));
        for (int index = 0; index < this._ellipsisCount; ++index)
          this._stringBuilder.Append(".");
        b.DrawString(Game1.dialogueFont, this._stringBuilder, vector2, Color.White);
      }
      this.hasDrawn = true;
    }

    public void Dispose()
    {
      Game1.game1.IsSaving = false;
    }
  }
}
