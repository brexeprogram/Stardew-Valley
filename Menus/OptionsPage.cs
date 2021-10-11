// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.OptionsPage
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace StardewValley.Menus
{
  public class OptionsPage : IClickableMenu
  {
    private string descriptionText = "";
    private string hoverText = "";
    public List<ClickableComponent> optionSlots = new List<ClickableComponent>();
    private List<OptionsElement> options = new List<OptionsElement>();
    private int optionsSlotHeld = -1;
    public const int itemsPerPage = 7;
    public const int indexOfGraphicsPage = 6;
    public int currentItemIndex;
    private ClickableTextureComponent upArrow;
    private ClickableTextureComponent downArrow;
    private ClickableTextureComponent scrollBar;
    private bool scrolling;
    private Rectangle scrollBarRunner;

    public OptionsPage(int x, int y, int width, int height)
      : base(x, y, width, height, false)
    {
      this.upArrow = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + width + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 459, 11, 12), (float) Game1.pixelZoom, false);
      this.downArrow = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + width + Game1.tileSize / 4, this.yPositionOnScreen + height - Game1.tileSize, 11 * Game1.pixelZoom, 12 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(421, 472, 11, 12), (float) Game1.pixelZoom, false);
      this.scrollBar = new ClickableTextureComponent(new Rectangle(this.upArrow.bounds.X + Game1.pixelZoom * 3, this.upArrow.bounds.Y + this.upArrow.bounds.Height + Game1.pixelZoom, 6 * Game1.pixelZoom, 10 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(435, 463, 6, 10), (float) Game1.pixelZoom, false);
      this.scrollBarRunner = new Rectangle(this.scrollBar.bounds.X, this.upArrow.bounds.Y + this.upArrow.bounds.Height + Game1.pixelZoom, this.scrollBar.bounds.Width, height - Game1.tileSize * 2 - this.upArrow.bounds.Height - Game1.pixelZoom * 2);
      for (int index = 0; index < 7; ++index)
      {
        List<ClickableComponent> optionSlots = this.optionSlots;
        ClickableComponent clickableComponent = new ClickableComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 4, this.yPositionOnScreen + Game1.tileSize * 5 / 4 + Game1.pixelZoom + index * ((height - Game1.tileSize * 2) / 7), width - Game1.tileSize / 2, (height - Game1.tileSize * 2) / 7 + Game1.pixelZoom), string.Concat((object) index));
        clickableComponent.myID = index;
        int num1 = index < 6 ? index + 1 : -7777;
        clickableComponent.downNeighborID = num1;
        int num2 = index > 0 ? index - 1 : -7777;
        clickableComponent.upNeighborID = num2;
        int num3 = 1;
        clickableComponent.fullyImmutable = num3 != 0;
        optionSlots.Add(clickableComponent);
      }
      this.options.Add(new OptionsElement(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11233")));
      this.options.Add((OptionsElement) new OptionsCheckbox(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11234"), 0, -1, -1));
      this.options.Add((OptionsElement) new OptionsCheckbox(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11235"), 7, -1, -1));
      this.options.Add((OptionsElement) new OptionsCheckbox(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11236"), 8, -1, -1));
      this.options.Add((OptionsElement) new OptionsCheckbox(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11237"), 11, -1, -1));
      this.options.Add((OptionsElement) new OptionsCheckbox(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11238"), 12, -1, -1));
      this.options.Add((OptionsElement) new OptionsCheckbox(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11239"), 27, -1, -1));
      this.options.Add((OptionsElement) new OptionsCheckbox(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11240"), 14, -1, -1));
      this.options.Add((OptionsElement) new OptionsCheckbox(Game1.content.LoadString("Strings\\UI:Options_GamepadStyleMenus"), 29, -1, -1));
      this.options.Add(new OptionsElement(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11241")));
      this.options.Add((OptionsElement) new OptionsSlider(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11242"), 1, -1, -1));
      this.options.Add((OptionsElement) new OptionsSlider(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11243"), 2, -1, -1));
      this.options.Add((OptionsElement) new OptionsSlider(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11244"), 20, -1, -1));
      this.options.Add((OptionsElement) new OptionsSlider(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11245"), 21, -1, -1));
      this.options.Add((OptionsElement) new OptionsCheckbox(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11246"), 3, -1, -1));
      this.options.Add(new OptionsElement(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11247")));
      if (!Game1.conventionMode)
      {
        this.options.Add((OptionsElement) new OptionsDropDown(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11248"), 13, -1, -1));
        this.options.Add((OptionsElement) new OptionsDropDown(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11251"), 6, -1, -1));
      }
      this.options.Add((OptionsElement) new OptionsCheckbox(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11252"), 9, -1, -1));
      this.options.Add((OptionsElement) new OptionsCheckbox(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11253"), 15, -1, -1));
      List<OptionsElement> options1 = this.options;
      string label1 = Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11254");
      int whichOption1 = 18;
      List<string> options2 = new List<string>();
      options2.Add("75%");
      options2.Add("80%");
      options2.Add("85%");
      options2.Add("90%");
      options2.Add("95%");
      options2.Add("100%");
      options2.Add("105%");
      options2.Add("110%");
      options2.Add("115%");
      options2.Add("120%");
      options2.Add("125%");
      List<string> displayOptions1 = new List<string>();
      displayOptions1.Add("75%");
      displayOptions1.Add("80%");
      displayOptions1.Add("85%");
      displayOptions1.Add("90%");
      displayOptions1.Add("95%");
      displayOptions1.Add("100%");
      displayOptions1.Add("105%");
      displayOptions1.Add("110%");
      displayOptions1.Add("115%");
      displayOptions1.Add("120%");
      displayOptions1.Add("125%");
      int x1 = -1;
      int y1 = -1;
      OptionsPlusMinus optionsPlusMinus1 = new OptionsPlusMinus(label1, whichOption1, options2, displayOptions1, x1, y1);
      options1.Add((OptionsElement) optionsPlusMinus1);
      this.options.Add((OptionsElement) new OptionsCheckbox(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11266"), 19, -1, -1));
      List<OptionsElement> options3 = this.options;
      string label2 = Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11267");
      int whichOption2 = 25;
      List<string> options4 = new List<string>();
      options4.Add("Low");
      options4.Add("Med.");
      options4.Add("High");
      List<string> displayOptions2 = new List<string>()
      {
        Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11268"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11269"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11270")
      };
      int x2 = -1;
      int y2 = -1;
      OptionsPlusMinus optionsPlusMinus2 = new OptionsPlusMinus(label2, whichOption2, options4, displayOptions2, x2, y2);
      options3.Add((OptionsElement) optionsPlusMinus2);
      this.options.Add((OptionsElement) new OptionsSlider(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11271"), 23, -1, -1));
      this.options.Add((OptionsElement) new OptionsCheckbox(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11272"), 24, -1, -1));
      this.options.Add((OptionsElement) new OptionsCheckbox(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11273"), 26, -1, -1));
      this.options.Add(new OptionsElement(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11274")));
      this.options.Add((OptionsElement) new OptionsCheckbox(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11275"), 16, -1, -1));
      this.options.Add((OptionsElement) new OptionsCheckbox(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11276"), 22, -1, -1));
      this.options.Add((OptionsElement) new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11277"), -1, this.optionSlots[0].bounds.Width, -1, -1));
      this.options.Add((OptionsElement) new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11278"), 7, this.optionSlots[0].bounds.Width, -1, -1));
      this.options.Add((OptionsElement) new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11279"), 10, this.optionSlots[0].bounds.Width, -1, -1));
      this.options.Add((OptionsElement) new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11280"), 15, this.optionSlots[0].bounds.Width, -1, -1));
      this.options.Add((OptionsElement) new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11281"), 18, this.optionSlots[0].bounds.Width, -1, -1));
      this.options.Add((OptionsElement) new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11282"), 19, this.optionSlots[0].bounds.Width, -1, -1));
      this.options.Add((OptionsElement) new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11283"), 11, this.optionSlots[0].bounds.Width, -1, -1));
      this.options.Add((OptionsElement) new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11284"), 14, this.optionSlots[0].bounds.Width, -1, -1));
      this.options.Add((OptionsElement) new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11285"), 13, this.optionSlots[0].bounds.Width, -1, -1));
      this.options.Add((OptionsElement) new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11286"), 12, this.optionSlots[0].bounds.Width, -1, -1));
      if (Game1.IsMultiplayer)
        this.options.Add((OptionsElement) new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11287"), 17, this.optionSlots[0].bounds.Width, -1, -1));
      this.options.Add((OptionsElement) new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11288"), 16, this.optionSlots[0].bounds.Width, -1, -1));
      this.options.Add((OptionsElement) new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11289"), 20, this.optionSlots[0].bounds.Width, -1, -1));
      this.options.Add((OptionsElement) new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11290"), 21, this.optionSlots[0].bounds.Width, -1, -1));
      this.options.Add((OptionsElement) new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11291"), 22, this.optionSlots[0].bounds.Width, -1, -1));
      this.options.Add((OptionsElement) new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11292"), 23, this.optionSlots[0].bounds.Width, -1, -1));
      this.options.Add((OptionsElement) new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11293"), 24, this.optionSlots[0].bounds.Width, -1, -1));
      this.options.Add((OptionsElement) new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11294"), 25, this.optionSlots[0].bounds.Width, -1, -1));
      this.options.Add((OptionsElement) new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11295"), 26, this.optionSlots[0].bounds.Width, -1, -1));
      this.options.Add((OptionsElement) new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11296"), 27, this.optionSlots[0].bounds.Width, -1, -1));
      this.options.Add((OptionsElement) new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11297"), 28, this.optionSlots[0].bounds.Width, -1, -1));
      this.options.Add((OptionsElement) new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11298"), 29, this.optionSlots[0].bounds.Width, -1, -1));
      this.options.Add((OptionsElement) new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11299"), 30, this.optionSlots[0].bounds.Width, -1, -1));
      this.options.Add((OptionsElement) new OptionsInputListener(Game1.content.LoadString("Strings\\StringsFromCSFiles:OptionsPage.cs.11300"), 31, this.optionSlots[0].bounds.Width, -1, -1));
    }

    public override void snapToDefaultClickableComponent()
    {
      this.currentItemIndex = 0;
      base.snapToDefaultClickableComponent();
      this.currentlySnappedComponent = this.getComponentWithID(1);
      this.snapCursorToCurrentSnappedComponent();
    }

    protected override void customSnapBehavior(int direction, int oldRegion, int oldID)
    {
      base.customSnapBehavior(direction, oldRegion, oldID);
      if (oldID == 6 && direction == 2 && this.currentItemIndex < Math.Max(0, this.options.Count - 7))
      {
        this.downArrowPressed();
        Game1.playSound("shiny4");
      }
      else
      {
        if (oldID != 0 || direction != 0)
          return;
        if (this.currentItemIndex > 0)
        {
          this.upArrowPressed();
          Game1.playSound("shiny4");
        }
        else
        {
          this.currentlySnappedComponent = this.getComponentWithID(12346);
          if (this.currentlySnappedComponent != null)
            this.currentlySnappedComponent.downNeighborID = 0;
          this.snapCursorToCurrentSnappedComponent();
        }
      }
    }

    private void setScrollBarToCurrentIndex()
    {
      if (this.options.Count <= 0)
        return;
      this.scrollBar.bounds.Y = this.scrollBarRunner.Height / Math.Max(1, this.options.Count - 7 + 1) * this.currentItemIndex + this.upArrow.bounds.Bottom + Game1.pixelZoom;
      if (this.currentItemIndex != this.options.Count - 7)
        return;
      this.scrollBar.bounds.Y = this.downArrow.bounds.Y - this.scrollBar.bounds.Height - Game1.pixelZoom;
    }

    public override void snapCursorToCurrentSnappedComponent()
    {
      if (this.currentlySnappedComponent != null && this.currentlySnappedComponent.myID < this.options.Count)
      {
        if (this.options[this.currentlySnappedComponent.myID + this.currentItemIndex] is OptionsInputListener)
          Game1.setMousePosition(this.currentlySnappedComponent.bounds.Right - Game1.tileSize * 3 / 4, this.currentlySnappedComponent.bounds.Center.Y - Game1.pixelZoom * 3);
        else
          Game1.setMousePosition(this.currentlySnappedComponent.bounds.Left + Game1.tileSize * 3 / 4, this.currentlySnappedComponent.bounds.Center.Y - Game1.pixelZoom * 3);
      }
      else
      {
        if (this.currentlySnappedComponent == null)
          return;
        base.snapCursorToCurrentSnappedComponent();
      }
    }

    public override void leftClickHeld(int x, int y)
    {
      if (GameMenu.forcePreventClose)
        return;
      base.leftClickHeld(x, y);
      if (this.scrolling)
      {
        int y1 = this.scrollBar.bounds.Y;
        this.scrollBar.bounds.Y = Math.Min(this.yPositionOnScreen + this.height - Game1.tileSize - Game1.pixelZoom * 3 - this.scrollBar.bounds.Height, Math.Max(y, this.yPositionOnScreen + this.upArrow.bounds.Height + Game1.pixelZoom * 5));
        this.currentItemIndex = Math.Min(this.options.Count - 7, Math.Max(0, (int) ((double) this.options.Count * (double) ((float) (y - this.scrollBarRunner.Y) / (float) this.scrollBarRunner.Height))));
        this.setScrollBarToCurrentIndex();
        int y2 = this.scrollBar.bounds.Y;
        if (y1 == y2)
          return;
        Game1.playSound("shiny4");
      }
      else
      {
        if (this.optionsSlotHeld == -1 || this.optionsSlotHeld + this.currentItemIndex >= this.options.Count)
          return;
        this.options[this.currentItemIndex + this.optionsSlotHeld].leftClickHeld(x - this.optionSlots[this.optionsSlotHeld].bounds.X, y - this.optionSlots[this.optionsSlotHeld].bounds.Y);
      }
    }

    public override ClickableComponent getCurrentlySnappedComponent()
    {
      return this.currentlySnappedComponent;
    }

    public override void setCurrentlySnappedComponentTo(int id)
    {
      this.currentlySnappedComponent = this.getComponentWithID(id);
      this.snapCursorToCurrentSnappedComponent();
    }

    public override void receiveKeyPress(Keys key)
    {
      if (this.optionsSlotHeld != -1 && this.optionsSlotHeld + this.currentItemIndex < this.options.Count || Game1.options.snappyMenus && Game1.options.gamepadControls)
      {
        if (this.currentlySnappedComponent != null && Game1.options.snappyMenus && (Game1.options.gamepadControls && this.options.Count > this.currentItemIndex + this.currentlySnappedComponent.myID) && this.currentItemIndex + this.currentlySnappedComponent.myID >= 0)
          this.options[this.currentItemIndex + this.currentlySnappedComponent.myID].receiveKeyPress(key);
        else if (this.options.Count > this.currentItemIndex + this.optionsSlotHeld && this.currentItemIndex + this.optionsSlotHeld >= 0)
          this.options[this.currentItemIndex + this.optionsSlotHeld].receiveKeyPress(key);
      }
      base.receiveKeyPress(key);
    }

    public override void receiveScrollWheelAction(int direction)
    {
      if (GameMenu.forcePreventClose)
        return;
      base.receiveScrollWheelAction(direction);
      if (direction > 0 && this.currentItemIndex > 0)
      {
        this.upArrowPressed();
        Game1.playSound("shiny4");
      }
      else
      {
        if (direction >= 0 || this.currentItemIndex >= Math.Max(0, this.options.Count - 7))
          return;
        this.downArrowPressed();
        Game1.playSound("shiny4");
      }
    }

    public override void releaseLeftClick(int x, int y)
    {
      if (GameMenu.forcePreventClose)
        return;
      base.releaseLeftClick(x, y);
      if (this.optionsSlotHeld != -1 && this.optionsSlotHeld + this.currentItemIndex < this.options.Count)
        this.options[this.currentItemIndex + this.optionsSlotHeld].leftClickReleased(x - this.optionSlots[this.optionsSlotHeld].bounds.X, y - this.optionSlots[this.optionsSlotHeld].bounds.Y);
      this.optionsSlotHeld = -1;
      this.scrolling = false;
    }

    private void downArrowPressed()
    {
      this.downArrow.scale = this.downArrow.baseScale;
      this.currentItemIndex = this.currentItemIndex + 1;
      this.setScrollBarToCurrentIndex();
    }

    private void upArrowPressed()
    {
      this.upArrow.scale = this.upArrow.baseScale;
      this.currentItemIndex = this.currentItemIndex - 1;
      this.setScrollBarToCurrentIndex();
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
      if (GameMenu.forcePreventClose)
        return;
      if (this.downArrow.containsPoint(x, y) && this.currentItemIndex < Math.Max(0, this.options.Count - 7))
      {
        this.downArrowPressed();
        Game1.playSound("shwip");
      }
      else if (this.upArrow.containsPoint(x, y) && this.currentItemIndex > 0)
      {
        this.upArrowPressed();
        Game1.playSound("shwip");
      }
      else if (this.scrollBar.containsPoint(x, y))
        this.scrolling = true;
      else if (!this.downArrow.containsPoint(x, y) && x > this.xPositionOnScreen + this.width && (x < this.xPositionOnScreen + this.width + Game1.tileSize * 2 && y > this.yPositionOnScreen) && y < this.yPositionOnScreen + this.height)
      {
        this.scrolling = true;
        this.leftClickHeld(x, y);
        this.releaseLeftClick(x, y);
      }
      this.currentItemIndex = Math.Max(0, Math.Min(this.options.Count - 7, this.currentItemIndex));
      for (int index = 0; index < this.optionSlots.Count; ++index)
      {
        if (this.optionSlots[index].bounds.Contains(x, y) && this.currentItemIndex + index < this.options.Count && this.options[this.currentItemIndex + index].bounds.Contains(x - this.optionSlots[index].bounds.X, y - this.optionSlots[index].bounds.Y))
        {
          this.options[this.currentItemIndex + index].receiveLeftClick(x - this.optionSlots[index].bounds.X, y - this.optionSlots[index].bounds.Y);
          this.optionsSlotHeld = index;
          break;
        }
      }
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
    }

    public override void performHoverAction(int x, int y)
    {
      for (int index = 0; index < this.optionSlots.Count; ++index)
      {
        if (this.currentItemIndex >= 0 && this.currentItemIndex + index < this.options.Count && this.options[this.currentItemIndex + index].bounds.Contains(x - this.optionSlots[index].bounds.X, y - this.optionSlots[index].bounds.Y))
        {
          Game1.SetFreeCursorDrag();
          break;
        }
      }
      if (this.scrollBarRunner.Contains(x, y))
        Game1.SetFreeCursorDrag();
      if (GameMenu.forcePreventClose)
        return;
      this.descriptionText = "";
      this.hoverText = "";
      this.upArrow.tryHover(x, y, 0.1f);
      this.downArrow.tryHover(x, y, 0.1f);
      this.scrollBar.tryHover(x, y, 0.1f);
      int num = this.scrolling ? 1 : 0;
    }

    public override void draw(SpriteBatch b)
    {
      b.End();
      b.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
      for (int index = 0; index < this.optionSlots.Count; ++index)
      {
        if (this.currentItemIndex >= 0 && this.currentItemIndex + index < this.options.Count)
          this.options[this.currentItemIndex + index].draw(b, this.optionSlots[index].bounds.X, this.optionSlots[index].bounds.Y);
      }
      b.End();
      b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
      if (!GameMenu.forcePreventClose)
      {
        this.upArrow.draw(b);
        this.downArrow.draw(b);
        if (this.options.Count > 7)
        {
          IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(403, 383, 6, 6), this.scrollBarRunner.X, this.scrollBarRunner.Y, this.scrollBarRunner.Width, this.scrollBarRunner.Height, Color.White, (float) Game1.pixelZoom, false);
          this.scrollBar.draw(b);
        }
      }
      if (this.hoverText.Equals(""))
        return;
      IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, -1, (string) null, -1, (string[]) null, (Item) null, 0, -1, -1, -1, -1, 1f, (CraftingRecipe) null);
    }
  }
}
