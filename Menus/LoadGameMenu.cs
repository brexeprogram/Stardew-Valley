// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.LoadGameMenu
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StardewValley.Menus
{
  public class LoadGameMenu : IClickableMenu, IDisposable
  {
    public List<ClickableComponent> gamesToLoadButton = new List<ClickableComponent>();
    public List<ClickableTextureComponent> deleteButtons = new List<ClickableTextureComponent>();
    private int selected = -1;
    private int selectedForDelete = -1;
    private List<Farmer> saveGames = new List<Farmer>();
    private string hoverText = "";
    public const int region_upArrow = 800;
    public const int region_downArrow = 801;
    public const int region_okDelete = 802;
    public const int region_cancelDelete = 803;
    public const int itemsPerPage = 4;
    private int currentItemIndex;
    private int timerToLoad;
    public ClickableTextureComponent upArrow;
    public ClickableTextureComponent downArrow;
    public ClickableTextureComponent scrollBar;
    public ClickableTextureComponent okDeleteButton;
    public ClickableTextureComponent cancelDeleteButton;
    public ClickableComponent backButton;
    public bool scrolling;
    public bool deleteConfirmationScreen;
    private Rectangle scrollBarRunner;
    private bool loading;
    private bool deleting;
    private Task<List<Farmer>> _initTask;
    private Task _deleteTask;
    private bool disposedValue;

    public override bool readyToClose()
    {
      if (this._initTask == null && this._deleteTask == null && !this.loading)
        return !this.deleting;
      return false;
    }

    public LoadGameMenu()
      : base(Game1.viewport.Width / 2 - (1100 + IClickableMenu.borderWidth * 2) / 2, Game1.viewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2, 1100 + IClickableMenu.borderWidth * 2, 600 + IClickableMenu.borderWidth * 2, false)
    {
      this.backButton = new ClickableComponent(new Rectangle(Game1.viewport.Width - 198 - 48, Game1.viewport.Height - 81 - 24, 198, 81), "")
      {
        myID = 81114,
        upNeighborID = 801,
        leftNeighborID = 801
      };
      ClickableTextureComponent textureComponent1 = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize / 4, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 459, 11, 12), (float) Game1.pixelZoom, false);
      int num1 = 800;
      textureComponent1.myID = num1;
      int num2 = 801;
      textureComponent1.downNeighborID = num2;
      int num3 = 100;
      textureComponent1.leftNeighborID = num3;
      this.upArrow = textureComponent1;
      ClickableTextureComponent textureComponent2 = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize / 4, this.yPositionOnScreen + this.height - Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 472, 11, 12), (float) Game1.pixelZoom, false);
      int num4 = 801;
      textureComponent2.myID = num4;
      int num5 = 800;
      textureComponent2.upNeighborID = num5;
      int num6 = 103;
      textureComponent2.leftNeighborID = num6;
      int num7 = 81114;
      textureComponent2.rightNeighborID = num7;
      int num8 = 81114;
      textureComponent2.downNeighborID = num8;
      this.downArrow = textureComponent2;
      this.scrollBar = new ClickableTextureComponent(new Rectangle(this.upArrow.bounds.X + Game1.pixelZoom * 3, this.upArrow.bounds.Y + this.upArrow.bounds.Height + Game1.pixelZoom, 6 * Game1.pixelZoom, 10 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(435, 463, 6, 10), (float) Game1.pixelZoom, false);
      this.scrollBarRunner = new Rectangle(this.scrollBar.bounds.X, this.upArrow.bounds.Y + this.upArrow.bounds.Height + Game1.pixelZoom, this.scrollBar.bounds.Width, this.height - Game1.tileSize - this.upArrow.bounds.Height - Game1.pixelZoom * 7);
      ClickableTextureComponent textureComponent3 = new ClickableTextureComponent(Game1.content.LoadString("Strings\\StringsFromCSFiles:LoadGameMenu.cs.10992"), new Rectangle((int) Utility.getTopLeftPositionForCenteringOnScreen(Game1.tileSize, Game1.tileSize, 0, 0).X - Game1.tileSize, (int) Utility.getTopLeftPositionForCenteringOnScreen(Game1.tileSize, Game1.tileSize, 0, 0).Y + Game1.tileSize * 2, Game1.tileSize, Game1.tileSize), (string) null, (string) null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f, false);
      int num9 = 802;
      textureComponent3.myID = num9;
      int num10 = 803;
      textureComponent3.rightNeighborID = num10;
      this.okDeleteButton = textureComponent3;
      ClickableTextureComponent textureComponent4 = new ClickableTextureComponent(Game1.content.LoadString("Strings\\StringsFromCSFiles:LoadGameMenu.cs.10993"), new Rectangle((int) Utility.getTopLeftPositionForCenteringOnScreen(Game1.tileSize, Game1.tileSize, 0, 0).X + Game1.tileSize, (int) Utility.getTopLeftPositionForCenteringOnScreen(Game1.tileSize, Game1.tileSize, 0, 0).Y + Game1.tileSize * 2, Game1.tileSize, Game1.tileSize), (string) null, (string) null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 47, -1, -1), 1f, false);
      int num11 = 803;
      textureComponent4.myID = num11;
      int num12 = 802;
      textureComponent4.leftNeighborID = num12;
      this.cancelDeleteButton = textureComponent4;
      for (int index = 0; index < 4; ++index)
      {
        List<ClickableComponent> gamesToLoadButton = this.gamesToLoadButton;
        ClickableComponent clickableComponent = new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize / 4 + index * (this.height / 4), this.width - Game1.tileSize / 2, this.height / 4 + Game1.pixelZoom), string.Concat((object) index));
        clickableComponent.myID = index;
        int num13 = index < 3 ? index + 1 : -7777;
        clickableComponent.downNeighborID = num13;
        int num14 = index > 0 ? index - 1 : -7777;
        clickableComponent.upNeighborID = num14;
        int num15 = index + 100;
        clickableComponent.rightNeighborID = num15;
        gamesToLoadButton.Add(clickableComponent);
        List<ClickableTextureComponent> deleteButtons = this.deleteButtons;
        ClickableTextureComponent textureComponent5 = new ClickableTextureComponent("", new Rectangle(this.xPositionOnScreen + this.width - Game1.tileSize - Game1.pixelZoom, this.yPositionOnScreen + Game1.tileSize / 2 + Game1.pixelZoom + index * (this.height / 4), 12 * Game1.pixelZoom, 12 * Game1.pixelZoom), "", Game1.content.LoadString("Strings\\StringsFromCSFiles:LoadGameMenu.cs.10994"), Game1.mouseCursors, new Rectangle(322, 498, 12, 12), (float) ((double) Game1.pixelZoom * 3.0 / 4.0), false);
        int num16 = index + 100;
        textureComponent5.myID = num16;
        int num17 = index;
        textureComponent5.leftNeighborID = num17;
        int num18 = index + 1 + 100;
        textureComponent5.downNeighborID = num18;
        int num19 = index - 1 + 100;
        textureComponent5.upNeighborID = num19;
        int num20 = index < 2 ? 800 : 801;
        textureComponent5.rightNeighborID = num20;
        deleteButtons.Add(textureComponent5);
      }
      this._initTask = new Task<List<Farmer>>((Func<List<Farmer>>) (() =>
      {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        return LoadGameMenu.FindSaveGames();
      }));
      this._initTask.Start();
      if (!Game1.options.snappyMenus || !Game1.options.gamepadControls)
        return;
      this.populateClickableComponentList();
      this.snapToDefaultClickableComponent();
    }

    private static List<Farmer> FindSaveGames()
    {
      List<Farmer> farmerList = new List<Farmer>();
      string str = Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StardewValley"), "Saves"));
      if (Directory.Exists(str))
      {
        foreach (string directory in Directory.GetDirectories(str))
        {
          string path = Path.Combine(str, directory, "SaveGameInfo");
          Farmer target = (Farmer) null;
          try
          {
            using (FileStream fileStream = File.Open(path, FileMode.Open))
            {
              target = (Farmer) SaveGame.farmerSerializer.Deserialize((Stream) fileStream);
              SaveGame.loadDataToFarmer(target);
              target.favoriteThing = ((IEnumerable<string>) directory.Split(Path.DirectorySeparatorChar)).Last<string>();
              farmerList.Add(target);
              fileStream.Close();
            }
          }
          catch (Exception ex)
          {
            if (target != null)
              target.unload();
          }
        }
      }
      farmerList.Sort();
      return farmerList;
    }

    public override void receiveGamePadButton(Buttons b)
    {
      if (b != Buttons.B || !this.deleteConfirmationScreen)
        return;
      this.deleteConfirmationScreen = false;
      this.selectedForDelete = -1;
      Game1.playSound("smallSelect");
      if (!Game1.options.snappyMenus || !Game1.options.gamepadControls)
        return;
      this.currentlySnappedComponent = this.getComponentWithID(0);
      this.snapCursorToCurrentSnappedComponent();
    }

    public override void snapToDefaultClickableComponent()
    {
      this.currentlySnappedComponent = this.getComponentWithID(0);
      this.snapCursorToCurrentSnappedComponent();
    }

    protected override void customSnapBehavior(int direction, int oldRegion, int oldID)
    {
      if (direction == 2 && this.currentItemIndex < Math.Max(0, this.saveGames.Count - 4))
      {
        this.downArrowPressed();
        this.currentlySnappedComponent = this.getComponentWithID(3);
        this.snapCursorToCurrentSnappedComponent();
      }
      else
      {
        if (direction != 0 || this.currentItemIndex <= 0)
          return;
        this.upArrowPressed();
        this.currentlySnappedComponent = this.getComponentWithID(0);
        this.snapCursorToCurrentSnappedComponent();
      }
    }

    public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
    {
      base.gameWindowSizeChanged(oldBounds, newBounds);
      ClickableTextureComponent textureComponent1 = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize / 4, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 459, 11, 12), (float) Game1.pixelZoom, false);
      int num1 = 800;
      textureComponent1.myID = num1;
      int num2 = 801;
      textureComponent1.downNeighborID = num2;
      this.upArrow = textureComponent1;
      ClickableTextureComponent textureComponent2 = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width + Game1.tileSize / 4, this.yPositionOnScreen + this.height - Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 472, 11, 12), (float) Game1.pixelZoom, false);
      int num3 = 801;
      textureComponent2.myID = num3;
      int num4 = 800;
      textureComponent2.upNeighborID = num4;
      this.downArrow = textureComponent2;
      this.scrollBar = new ClickableTextureComponent(new Rectangle(this.upArrow.bounds.X + Game1.pixelZoom * 3, this.upArrow.bounds.Y + this.upArrow.bounds.Height + Game1.pixelZoom, 6 * Game1.pixelZoom, 10 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(435, 463, 6, 10), (float) Game1.pixelZoom, false);
      this.scrollBarRunner = new Rectangle(this.scrollBar.bounds.X, this.upArrow.bounds.Y + this.upArrow.bounds.Height + Game1.pixelZoom, this.scrollBar.bounds.Width, this.height - Game1.tileSize - this.upArrow.bounds.Height - Game1.pixelZoom * 7);
      ClickableTextureComponent textureComponent3 = new ClickableTextureComponent(Game1.content.LoadString("Strings\\StringsFromCSFiles:LoadGameMenu.cs.10992"), new Rectangle((int) Utility.getTopLeftPositionForCenteringOnScreen(Game1.tileSize, Game1.tileSize, 0, 0).X - Game1.tileSize, (int) Utility.getTopLeftPositionForCenteringOnScreen(Game1.tileSize, Game1.tileSize, 0, 0).Y + Game1.tileSize * 2, Game1.tileSize, Game1.tileSize), (string) null, (string) null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f, false);
      int num5 = 802;
      textureComponent3.myID = num5;
      int num6 = 803;
      textureComponent3.rightNeighborID = num6;
      this.okDeleteButton = textureComponent3;
      ClickableTextureComponent textureComponent4 = new ClickableTextureComponent(Game1.content.LoadString("Strings\\StringsFromCSFiles:LoadGameMenu.cs.10993"), new Rectangle((int) Utility.getTopLeftPositionForCenteringOnScreen(Game1.tileSize, Game1.tileSize, 0, 0).X + Game1.tileSize, (int) Utility.getTopLeftPositionForCenteringOnScreen(Game1.tileSize, Game1.tileSize, 0, 0).Y + Game1.tileSize * 2, Game1.tileSize, Game1.tileSize), (string) null, (string) null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 47, -1, -1), 1f, false);
      int num7 = 803;
      textureComponent4.myID = num7;
      int num8 = 802;
      textureComponent4.leftNeighborID = num8;
      this.cancelDeleteButton = textureComponent4;
      this.gamesToLoadButton.Clear();
      this.deleteButtons.Clear();
      for (int index = 0; index < 4; ++index)
      {
        List<ClickableComponent> gamesToLoadButton = this.gamesToLoadButton;
        ClickableComponent clickableComponent = new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize / 4 + index * (this.height / 4), this.width - Game1.tileSize / 2, this.height / 4 + Game1.pixelZoom), string.Concat((object) index));
        clickableComponent.myID = index;
        int num9 = index < 3 ? index + 1 : -7777;
        clickableComponent.downNeighborID = num9;
        int num10 = index > 0 ? index - 1 : -7777;
        clickableComponent.upNeighborID = num10;
        int num11 = index + 100;
        clickableComponent.rightNeighborID = num11;
        gamesToLoadButton.Add(clickableComponent);
        List<ClickableTextureComponent> deleteButtons = this.deleteButtons;
        ClickableTextureComponent textureComponent5 = new ClickableTextureComponent("", new Rectangle(this.xPositionOnScreen + this.width - Game1.tileSize - Game1.pixelZoom, this.yPositionOnScreen + Game1.tileSize / 2 + Game1.pixelZoom + index * (this.height / 4), 12 * Game1.pixelZoom, 12 * Game1.pixelZoom), "", Game1.content.LoadString("Strings\\StringsFromCSFiles:LoadGameMenu.cs.10994"), Game1.mouseCursors, new Rectangle(322, 498, 12, 12), (float) ((double) Game1.pixelZoom * 3.0 / 4.0), false);
        int num12 = index + 100;
        textureComponent5.myID = num12;
        int num13 = index;
        textureComponent5.leftNeighborID = num13;
        int num14 = index + 1 + 100;
        textureComponent5.downNeighborID = num14;
        int num15 = index - 1 + 100;
        textureComponent5.upNeighborID = num15;
        int num16 = index < 2 ? 800 : 801;
        textureComponent5.rightNeighborID = num16;
        deleteButtons.Add(textureComponent5);
      }
    }

    public override void performHoverAction(int x, int y)
    {
      this.hoverText = "";
      base.performHoverAction(x, y);
      if (this.deleteConfirmationScreen)
      {
        this.okDeleteButton.tryHover(x, y, 0.1f);
        this.cancelDeleteButton.tryHover(x, y, 0.1f);
        if (this.okDeleteButton.containsPoint(x, y))
        {
          this.hoverText = "";
        }
        else
        {
          if (!this.cancelDeleteButton.containsPoint(x, y))
            return;
          this.hoverText = Game1.content.LoadString("Strings\\StringsFromCSFiles:LoadGameMenu.cs.10993");
        }
      }
      else
      {
        this.upArrow.tryHover(x, y, 0.1f);
        this.downArrow.tryHover(x, y, 0.1f);
        this.scrollBar.tryHover(x, y, 0.1f);
        foreach (ClickableTextureComponent deleteButton in this.deleteButtons)
        {
          int x1 = x;
          int y1 = y;
          double num = 0.200000002980232;
          deleteButton.tryHover(x1, y1, (float) num);
          int x2 = x;
          int y2 = y;
          if (deleteButton.containsPoint(x2, y2))
          {
            this.hoverText = Game1.content.LoadString("Strings\\StringsFromCSFiles:LoadGameMenu.cs.10994");
            return;
          }
        }
        if (this.scrolling)
          return;
        for (int index = 0; index < this.gamesToLoadButton.Count; ++index)
        {
          if (this.currentItemIndex + index < this.saveGames.Count && this.gamesToLoadButton[index].containsPoint(x, y))
          {
            if ((double) this.gamesToLoadButton[index].scale == 1.0)
              Game1.playSound("Cowboy_gunshot");
            this.gamesToLoadButton[index].scale = Math.Min(this.gamesToLoadButton[index].scale + 0.03f, 1.1f);
          }
          else
            this.gamesToLoadButton[index].scale = Math.Max(1f, this.gamesToLoadButton[index].scale - 0.03f);
        }
      }
    }

    public override void leftClickHeld(int x, int y)
    {
      base.leftClickHeld(x, y);
      if (!this.scrolling)
        return;
      int y1 = this.scrollBar.bounds.Y;
      this.scrollBar.bounds.Y = Math.Min(this.yPositionOnScreen + this.height - Game1.tileSize - Game1.pixelZoom * 3 - this.scrollBar.bounds.Height, Math.Max(y, this.yPositionOnScreen + this.upArrow.bounds.Height + Game1.pixelZoom * 5));
      this.currentItemIndex = Math.Min(this.saveGames.Count - 4, Math.Max(0, (int) ((double) this.saveGames.Count * (double) ((float) (y - this.scrollBarRunner.Y) / (float) this.scrollBarRunner.Height))));
      this.setScrollBarToCurrentIndex();
      int y2 = this.scrollBar.bounds.Y;
      if (y1 == y2)
        return;
      Game1.playSound("shiny4");
    }

    public override void releaseLeftClick(int x, int y)
    {
      base.releaseLeftClick(x, y);
      this.scrolling = false;
    }

    private void setScrollBarToCurrentIndex()
    {
      if (this.saveGames.Count <= 0)
        return;
      this.scrollBar.bounds.Y = this.scrollBarRunner.Height / Math.Max(1, this.saveGames.Count - 4 + 1) * this.currentItemIndex + this.upArrow.bounds.Bottom + Game1.pixelZoom;
      if (this.currentItemIndex != this.saveGames.Count - 4)
        return;
      this.scrollBar.bounds.Y = this.downArrow.bounds.Y - this.scrollBar.bounds.Height - Game1.pixelZoom;
    }

    public override void receiveScrollWheelAction(int direction)
    {
      base.receiveScrollWheelAction(direction);
      if (direction > 0 && this.currentItemIndex > 0)
      {
        this.upArrowPressed();
      }
      else
      {
        if (direction >= 0 || this.currentItemIndex >= Math.Max(0, this.saveGames.Count - 4))
          return;
        this.downArrowPressed();
      }
    }

    private void downArrowPressed()
    {
      this.downArrow.scale = this.downArrow.baseScale;
      this.currentItemIndex = this.currentItemIndex + 1;
      Game1.playSound("shwip");
      this.setScrollBarToCurrentIndex();
    }

    private void upArrowPressed()
    {
      this.upArrow.scale = this.upArrow.baseScale;
      this.currentItemIndex = this.currentItemIndex - 1;
      Game1.playSound("shwip");
      this.setScrollBarToCurrentIndex();
    }

    private void deleteFile(int which)
    {
      string path = Path.Combine(Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StardewValley"), "Saves"), this.saveGames[which].favoriteThing));
      Thread.Sleep(Game1.random.Next(1000, 5000));
      if (!Directory.Exists(path))
        return;
      Directory.Delete(path, true);
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
      if (this.timerToLoad > 0 || this.loading || this.deleting)
        return;
      if (this.deleteConfirmationScreen)
      {
        if (this.cancelDeleteButton.containsPoint(x, y))
        {
          this.deleteConfirmationScreen = false;
          this.selectedForDelete = -1;
          Game1.playSound("smallSelect");
          if (!Game1.options.snappyMenus || !Game1.options.gamepadControls)
            return;
          this.currentlySnappedComponent = this.getComponentWithID(0);
          this.snapCursorToCurrentSnappedComponent();
        }
        else
        {
          if (!this.okDeleteButton.containsPoint(x, y))
            return;
          this.deleting = true;
          this._deleteTask = new Task((Action) (() =>
          {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            this.deleteFile(this.selectedForDelete);
          }));
          this._deleteTask.Start();
          this.deleteConfirmationScreen = false;
          if (Game1.options.snappyMenus && Game1.options.gamepadControls)
          {
            this.currentlySnappedComponent = this.getComponentWithID(0);
            this.snapCursorToCurrentSnappedComponent();
          }
          Game1.playSound("trashcan");
        }
      }
      else
      {
        base.receiveLeftClick(x, y, playSound);
        if (this.downArrow.containsPoint(x, y) && this.currentItemIndex < Math.Max(0, this.saveGames.Count - 4))
          this.downArrowPressed();
        else if (this.upArrow.containsPoint(x, y) && this.currentItemIndex > 0)
          this.upArrowPressed();
        else if (this.scrollBar.containsPoint(x, y))
          this.scrolling = true;
        else if (!this.downArrow.containsPoint(x, y) && x > this.xPositionOnScreen + this.width && (x < this.xPositionOnScreen + this.width + Game1.tileSize * 2 && y > this.yPositionOnScreen) && y < this.yPositionOnScreen + this.height)
        {
          this.scrolling = true;
          this.leftClickHeld(x, y);
          this.releaseLeftClick(x, y);
        }
        if (this.selected == -1)
        {
          for (int index = 0; index < this.deleteButtons.Count; ++index)
          {
            if (this.deleteButtons[index].containsPoint(x, y) && index < this.saveGames.Count && !this.deleteConfirmationScreen)
            {
              this.deleteConfirmationScreen = true;
              Game1.playSound("drumkit6");
              this.selectedForDelete = this.currentItemIndex + index;
              if (!Game1.options.snappyMenus || !Game1.options.gamepadControls)
                return;
              this.currentlySnappedComponent = this.getComponentWithID(803);
              this.snapCursorToCurrentSnappedComponent();
              return;
            }
          }
        }
        if (!this.deleteConfirmationScreen)
        {
          for (int index = 0; index < this.gamesToLoadButton.Count; ++index)
          {
            if (this.gamesToLoadButton[index].containsPoint(x, y) && index < this.saveGames.Count)
            {
              this.timerToLoad = 2150;
              this.loading = true;
              Game1.playSound("select");
              this.selected = this.currentItemIndex + index;
              return;
            }
          }
        }
        this.currentItemIndex = Math.Max(0, Math.Min(this.saveGames.Count - 4, this.currentItemIndex));
      }
    }

    public override void update(GameTime time)
    {
      base.update(time);
      if (this._initTask != null)
      {
        if (!this._initTask.IsCanceled && !this._initTask.IsCompleted && !this._initTask.IsFaulted)
          return;
        if (this._initTask.IsCompleted)
        {
          foreach (Farmer saveGame in this.saveGames)
            saveGame.unload();
          this.saveGames.Clear();
          this.saveGames.AddRange((IEnumerable<Farmer>) this._initTask.Result);
        }
        this._initTask = (Task<List<Farmer>>) null;
      }
      else if (this._deleteTask != null)
      {
        if (!this._deleteTask.IsCanceled && !this._deleteTask.IsCompleted && !this._deleteTask.IsFaulted)
          return;
        if (!this._deleteTask.IsCompleted)
          this.selectedForDelete = -1;
        this._deleteTask = (Task) null;
        this.deleting = false;
      }
      else
      {
        if (this.selectedForDelete != -1 && !this.deleteConfirmationScreen && !this.deleting)
        {
          this.saveGames[this.selectedForDelete].unload();
          this.saveGames.RemoveAt(this.selectedForDelete);
          this.selectedForDelete = -1;
          this.gamesToLoadButton.Clear();
          this.deleteButtons.Clear();
          for (int index = 0; index < 4; ++index)
          {
            this.gamesToLoadButton.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize / 4 + index * (this.height / 4), this.width - Game1.tileSize / 2, this.height / 4 + Game1.pixelZoom), string.Concat((object) index)));
            this.deleteButtons.Add(new ClickableTextureComponent("", new Rectangle(this.xPositionOnScreen + this.width - Game1.tileSize - Game1.pixelZoom, this.yPositionOnScreen + Game1.tileSize / 2 + Game1.pixelZoom + index * (this.height / 4), 12 * Game1.pixelZoom, 12 * Game1.pixelZoom), "", "Delete File", Game1.mouseCursors, new Rectangle(322, 498, 12, 12), (float) ((double) Game1.pixelZoom * 3.0 / 4.0), false));
          }
        }
        if (this.timerToLoad <= 0)
          return;
        this.timerToLoad = this.timerToLoad - time.ElapsedGameTime.Milliseconds;
        if (this.timerToLoad > 0)
          return;
        SaveGame.Load(this.saveGames[this.selected].favoriteThing);
        Game1.exitActiveMenu();
      }
    }

    public override void draw(SpriteBatch b)
    {
      IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(384, 373, 18, 18), this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height + Game1.tileSize / 2, Color.White, (float) Game1.pixelZoom, true);
      if (this.selectedForDelete == -1 || !this.deleting || this.deleteConfirmationScreen)
      {
        for (int index = 0; index < this.gamesToLoadButton.Count; ++index)
        {
          if (this.currentItemIndex + index < this.saveGames.Count)
          {
            Farmer saveGame = this.saveGames[this.currentItemIndex + index];
            IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(384, 396, 15, 15), this.gamesToLoadButton[index].bounds.X, this.gamesToLoadButton[index].bounds.Y, this.gamesToLoadButton[index].bounds.Width, this.gamesToLoadButton[index].bounds.Height, this.currentItemIndex + index == this.selected && this.timerToLoad % 150 > 75 && this.timerToLoad > 1000 || this.selected == -1 && this.gamesToLoadButton[index].containsPoint(Game1.getOldMouseX(), Game1.getOldMouseY()) && (!this.scrolling && !this.deleteConfirmationScreen) ? (this.deleteButtons[index].containsPoint(Game1.getOldMouseX(), Game1.getOldMouseY()) ? Color.White : Color.Wheat) : Color.White, (float) Game1.pixelZoom, false);
            SpriteText.drawString(b, (this.currentItemIndex + index + 1).ToString() + ".", this.gamesToLoadButton[index].bounds.X + Game1.pixelZoom * 7 + Game1.tileSize / 2 - SpriteText.getWidthOfString((this.currentItemIndex + index + 1).ToString() + ".") / 2, this.gamesToLoadButton[index].bounds.Y + Game1.pixelZoom * 9, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
            SpriteText.drawString(b, saveGame.Name, this.gamesToLoadButton[index].bounds.X + Game1.tileSize * 2 + Game1.pixelZoom * 9, this.gamesToLoadButton[index].bounds.Y + Game1.pixelZoom * 9, 999999, -1, 999999, 1f, 0.88f, false, -1, "", -1);
            SpriteBatch spriteBatch = b;
            Texture2D shadowTexture = Game1.shadowTexture;
            Vector2 position1 = new Vector2((float) (this.gamesToLoadButton[index].bounds.X + Game1.tileSize + Game1.tileSize - Game1.pixelZoom), (float) (this.gamesToLoadButton[index].bounds.Y + Game1.tileSize * 2 + Game1.pixelZoom * 4));
            Rectangle? sourceRectangle = new Rectangle?(Game1.shadowTexture.Bounds);
            Color white = Color.White;
            double num1 = 0.0;
            Rectangle bounds = Game1.shadowTexture.Bounds;
            double x1 = (double) bounds.Center.X;
            bounds = Game1.shadowTexture.Bounds;
            double y = (double) bounds.Center.Y;
            Vector2 origin = new Vector2((float) x1, (float) y);
            double num2 = 4.0;
            int num3 = 0;
            double num4 = 0.800000011920929;
            spriteBatch.Draw(shadowTexture, position1, sourceRectangle, white, (float) num1, origin, (float) num2, (SpriteEffects) num3, (float) num4);
            saveGame.FarmerRenderer.draw(b, new FarmerSprite.AnimationFrame(0, 0, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false), 0, new Rectangle(0, 0, 16, 32), new Vector2((float) (this.gamesToLoadButton[index].bounds.X + Game1.tileSize / 4 + Game1.tileSize + Game1.pixelZoom * 3), (float) (this.gamesToLoadButton[index].bounds.Y + Game1.pixelZoom * 5)), Vector2.Zero, 0.8f, 2, Color.White, 0.0f, 1f, saveGame);
            string text1 = !saveGame.dayOfMonthForSaveGame.HasValue || !saveGame.seasonForSaveGame.HasValue || !saveGame.yearForSaveGame.HasValue ? saveGame.dateStringForSaveGame : Utility.getDateStringFor(saveGame.dayOfMonthForSaveGame.Value, saveGame.seasonForSaveGame.Value, saveGame.yearForSaveGame.Value);
            Utility.drawTextWithShadow(b, text1, Game1.dialogueFont, new Vector2((float) (this.gamesToLoadButton[index].bounds.X + Game1.tileSize * 2 + Game1.pixelZoom * 8), (float) (this.gamesToLoadButton[index].bounds.Y + Game1.tileSize + Game1.pixelZoom * 10)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
            Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\StringsFromCSFiles:LoadGameMenu.cs.11019", (object) saveGame.farmName), Game1.dialogueFont, new Vector2((float) (this.gamesToLoadButton[index].bounds.X + this.width - Game1.tileSize * 2) - Game1.dialogueFont.MeasureString(saveGame.farmName + " Farm").X, (float) (this.gamesToLoadButton[index].bounds.Y + Game1.pixelZoom * 11)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
            string text2 = Game1.content.LoadString("Strings\\StringsFromCSFiles:LoadGameMenu.cs.11020", (object) Utility.getNumberWithCommas(saveGame.Money));
            if (saveGame.Money == 1 && LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.pt)
              text2 = text2.Substring(0, text2.Length - 1);
            int x2 = (int) Game1.dialogueFont.MeasureString(text2).X;
            Utility.drawWithShadow(b, Game1.mouseCursors, new Vector2((float) (this.gamesToLoadButton[index].bounds.X + this.width - Game1.tileSize * 3 - Game1.pixelZoom * 25 - x2), (float) (this.gamesToLoadButton[index].bounds.Y + Game1.tileSize + Game1.pixelZoom * 11)), new Rectangle(193, 373, 9, 9), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
            Vector2 position2 = new Vector2((float) (this.gamesToLoadButton[index].bounds.X + this.width - Game1.tileSize * 3 - Game1.pixelZoom * 15 - x2), (float) (this.gamesToLoadButton[index].bounds.Y + Game1.tileSize + Game1.pixelZoom * 11));
            if (LocalizedContentManager.CurrentLanguageCode != LocalizedContentManager.LanguageCode.en)
              position2.Y += 5f;
            Utility.drawTextWithShadow(b, text2, Game1.dialogueFont, position2, Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
            position2 = new Vector2((float) (this.gamesToLoadButton[index].bounds.X + this.width - Game1.tileSize * 3 - Game1.pixelZoom * 11), (float) (this.gamesToLoadButton[index].bounds.Y + Game1.tileSize + Game1.pixelZoom * 9));
            Utility.drawWithShadow(b, Game1.mouseCursors, position2, new Rectangle(595, 1748, 9, 11), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, false, 1f, -1, -1, 0.35f);
            position2 = new Vector2((float) (this.gamesToLoadButton[index].bounds.X + this.width - Game1.tileSize * 3 - Game1.pixelZoom), (float) (this.gamesToLoadButton[index].bounds.Y + Game1.tileSize + Game1.pixelZoom * 11));
            if (LocalizedContentManager.CurrentLanguageCode != LocalizedContentManager.LanguageCode.en)
              position2.Y += 5f;
            Utility.drawTextWithShadow(b, Utility.getHoursMinutesStringFromMilliseconds(saveGame.millisecondsPlayed), Game1.dialogueFont, position2, Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
            if (this.deleteButtons.Count > index)
              this.deleteButtons[index].draw(b, Color.White * 0.75f, 1f);
          }
        }
      }
      string str = (string) null;
      if (this.saveGames.Count == 0)
        str = Game1.content.LoadString("Strings\\StringsFromCSFiles:LoadGameMenu.cs.11022");
      if (this._initTask != null)
        str = Game1.content.LoadString("Strings\\UI:LoadGameMenu_LookingForSavedGames");
      if (this.deleting)
        str = Game1.content.LoadString("Strings\\UI:LoadGameMenu_Deleting");
      if (str != null)
      {
        SpriteBatch b1 = b;
        string s = str;
        Viewport viewport = Game1.graphics.GraphicsDevice.Viewport;
        Rectangle bounds = viewport.Bounds;
        int x = bounds.Center.X;
        viewport = Game1.graphics.GraphicsDevice.Viewport;
        bounds = viewport.Bounds;
        int y = bounds.Center.Y;
        int characterPosition = 999999;
        int width = -1;
        int height = 999999;
        double num1 = 1.0;
        double num2 = 0.879999995231628;
        int num3 = 0;
        int color = -1;
        SpriteText.drawStringHorizontallyCenteredAt(b1, s, x, y, characterPosition, width, height, (float) num1, (float) num2, num3 != 0, color);
      }
      this.upArrow.draw(b);
      this.downArrow.draw(b);
      if (this.saveGames.Count > 4)
      {
        IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(403, 383, 6, 6), this.scrollBarRunner.X, this.scrollBarRunner.Y, this.scrollBarRunner.Width, this.scrollBarRunner.Height, Color.White, (float) Game1.pixelZoom, false);
        this.scrollBar.draw(b);
      }
      if (this.deleteConfirmationScreen)
      {
        b.Draw(Game1.staminaRect, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), Color.Black * 0.75f);
        string s = Game1.content.LoadString("Strings\\StringsFromCSFiles:LoadGameMenu.cs.11023", (object) this.saveGames[this.selectedForDelete].name);
        int num = this.okDeleteButton.bounds.X + (this.cancelDeleteButton.bounds.X - this.okDeleteButton.bounds.X) / 2 + this.okDeleteButton.bounds.Width / 2;
        SpriteText.drawString(b, s, num - SpriteText.getWidthOfString(s) / 2, (int) Utility.getTopLeftPositionForCenteringOnScreen(Game1.tileSize * 3, Game1.tileSize, 0, 0).Y, 9999, -1, 9999, 1f, 1f, false, -1, "", 4);
        this.okDeleteButton.draw(b);
        this.cancelDeleteButton.draw(b);
      }
      base.draw(b);
      if (this.hoverText.Length > 0)
        IClickableMenu.drawHoverText(b, this.hoverText, Game1.dialogueFont, 0, 0, -1, (string) null, -1, (string[]) null, (Item) null, 0, -1, -1, -1, -1, 1f, (CraftingRecipe) null);
      if (this.selected == -1 || this.timerToLoad >= 1000)
        return;
      b.Draw(Game1.staminaRect, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), Color.Black * (float) (1.0 - (double) this.timerToLoad / 1000.0));
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposedValue)
        return;
      if (disposing)
      {
        if (this.saveGames != null)
        {
          foreach (Farmer saveGame in this.saveGames)
            saveGame.unload();
          this.saveGames.Clear();
          this.saveGames = (List<Farmer>) null;
        }
        if (this._initTask != null)
          this._initTask = (Task<List<Farmer>>) null;
        if (this._deleteTask != null)
          this._deleteTask = (Task) null;
      }
      this.disposedValue = true;
    }

    ~LoadGameMenu()
    {
      this.Dispose(false);
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }
  }
}
