// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.SkillsPage
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
  public class SkillsPage : IClickableMenu
  {
    public List<ClickableTextureComponent> skillBars = new List<ClickableTextureComponent>();
    public List<ClickableTextureComponent> skillAreas = new List<ClickableTextureComponent>();
    public List<ClickableTextureComponent> specialItems = new List<ClickableTextureComponent>();
    private string hoverText = "";
    private string hoverTitle = "";
    private int professionImage = -1;
    private int[] playerPanelFrames = new int[4]
    {
      0,
      1,
      0,
      2
    };
    public const int region_special1 = 10201;
    public const int region_special2 = 10202;
    public const int region_special3 = 10203;
    public const int region_special4 = 10204;
    public const int region_special5 = 10205;
    public const int region_special6 = 10206;
    public const int region_special7 = 10207;
    public const int region_skillArea1 = 0;
    public const int region_skillArea2 = 1;
    public const int region_skillArea3 = 2;
    public const int region_skillArea4 = 3;
    public const int region_skillArea5 = 4;
    private int playerPanelIndex;
    private int playerPanelTimer;
    private Rectangle playerPanel;

    public SkillsPage(int x, int y, int width, int height)
      : base(x, y, width, height, false)
    {
      int x1 = this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize * 5 / 4;
      int y1 = this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + (int) ((double) height / 2.0) + Game1.tileSize * 5 / 4;
      this.playerPanel = new Rectangle(this.xPositionOnScreen + Game1.tileSize, this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder, Game1.tileSize * 2, Game1.tileSize * 3);
      if (Game1.player.canUnderstandDwarves)
      {
        List<ClickableTextureComponent> specialItems = this.specialItems;
        ClickableTextureComponent textureComponent = new ClickableTextureComponent("", new Rectangle(x1, y1, Game1.tileSize, Game1.tileSize), (string) null, Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11587"), Game1.mouseCursors, new Rectangle(129, 320, 16, 16), (float) Game1.pixelZoom, true);
        int num1 = 10201;
        textureComponent.myID = num1;
        int num2 = 10202;
        textureComponent.rightNeighborID = num2;
        int num3 = 4;
        textureComponent.upNeighborID = num3;
        specialItems.Add(textureComponent);
      }
      if (Game1.player.hasRustyKey)
      {
        List<ClickableTextureComponent> specialItems = this.specialItems;
        ClickableTextureComponent textureComponent = new ClickableTextureComponent("", new Rectangle(x1 + (Game1.tileSize + Game1.pixelZoom), y1, Game1.tileSize, Game1.tileSize), (string) null, Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11588"), Game1.mouseCursors, new Rectangle(145, 320, 16, 16), (float) Game1.pixelZoom, true);
        int num1 = 10202;
        textureComponent.myID = num1;
        int num2 = 10203;
        textureComponent.rightNeighborID = num2;
        int num3 = 10201;
        textureComponent.leftNeighborID = num3;
        int num4 = 4;
        textureComponent.upNeighborID = num4;
        specialItems.Add(textureComponent);
      }
      if (Game1.player.hasClubCard)
      {
        List<ClickableTextureComponent> specialItems = this.specialItems;
        ClickableTextureComponent textureComponent = new ClickableTextureComponent("", new Rectangle(x1 + 2 * (Game1.tileSize + Game1.pixelZoom), y1, Game1.tileSize, Game1.tileSize), (string) null, Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11589"), Game1.mouseCursors, new Rectangle(161, 320, 16, 16), (float) Game1.pixelZoom, true);
        int num1 = 10203;
        textureComponent.myID = num1;
        int num2 = 10204;
        textureComponent.rightNeighborID = num2;
        int num3 = 10202;
        textureComponent.leftNeighborID = num3;
        int num4 = 4;
        textureComponent.upNeighborID = num4;
        specialItems.Add(textureComponent);
      }
      if (Game1.player.hasSpecialCharm)
      {
        List<ClickableTextureComponent> specialItems = this.specialItems;
        ClickableTextureComponent textureComponent = new ClickableTextureComponent("", new Rectangle(x1 + 3 * (Game1.tileSize + Game1.pixelZoom), y1, Game1.tileSize, Game1.tileSize), (string) null, Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11590"), Game1.mouseCursors, new Rectangle(177, 320, 16, 16), (float) Game1.pixelZoom, true);
        int num1 = 10204;
        textureComponent.myID = num1;
        int num2 = 10205;
        textureComponent.rightNeighborID = num2;
        int num3 = 10203;
        textureComponent.leftNeighborID = num3;
        int num4 = 4;
        textureComponent.upNeighborID = num4;
        specialItems.Add(textureComponent);
      }
      if (Game1.player.hasSkullKey)
      {
        List<ClickableTextureComponent> specialItems = this.specialItems;
        ClickableTextureComponent textureComponent = new ClickableTextureComponent("", new Rectangle(x1 + 4 * (Game1.tileSize + Game1.pixelZoom), y1, Game1.tileSize, Game1.tileSize), (string) null, Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11591"), Game1.mouseCursors, new Rectangle(193, 320, 16, 16), (float) Game1.pixelZoom, true);
        int num1 = 10205;
        textureComponent.myID = num1;
        int num2 = 10206;
        textureComponent.rightNeighborID = num2;
        int num3 = 10204;
        textureComponent.leftNeighborID = num3;
        int num4 = 4;
        textureComponent.upNeighborID = num4;
        specialItems.Add(textureComponent);
      }
      if (Game1.player.hasDarkTalisman)
      {
        List<ClickableTextureComponent> specialItems = this.specialItems;
        ClickableTextureComponent textureComponent = new ClickableTextureComponent("", new Rectangle(x1 + 5 * (Game1.tileSize + Game1.pixelZoom), y1, Game1.tileSize, Game1.tileSize), (string) null, Game1.content.LoadString("Strings\\Objects:DarkTalisman"), Game1.mouseCursors, new Rectangle(225, 320, 16, 16), (float) Game1.pixelZoom, true);
        int num1 = 10206;
        textureComponent.myID = num1;
        int num2 = 10207;
        textureComponent.rightNeighborID = num2;
        int num3 = 10205;
        textureComponent.leftNeighborID = num3;
        int num4 = 4;
        textureComponent.upNeighborID = num4;
        specialItems.Add(textureComponent);
      }
      if (Game1.player.hasMagicInk)
      {
        List<ClickableTextureComponent> specialItems = this.specialItems;
        ClickableTextureComponent textureComponent = new ClickableTextureComponent("", new Rectangle(x1 + 6 * (Game1.tileSize + Game1.pixelZoom), y1, Game1.tileSize, Game1.tileSize), (string) null, Game1.content.LoadString("Strings\\Objects:MagicInk"), Game1.mouseCursors, new Rectangle(241, 320, 16, 16), (float) Game1.pixelZoom, true);
        int num1 = 10207;
        textureComponent.myID = num1;
        int num2 = 10206;
        textureComponent.leftNeighborID = num2;
        int num3 = 4;
        textureComponent.upNeighborID = num3;
        specialItems.Add(textureComponent);
      }
      int num5 = 0;
      int num6 = LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ru ? this.xPositionOnScreen + width - Game1.tileSize * 7 - Game1.tileSize * 3 / 4 + Game1.pixelZoom : this.xPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + 4 * Game1.tileSize - Game1.pixelZoom;
      int num7 = this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth - Game1.pixelZoom * 3;
      int num8 = 4;
      while (num8 < 10)
      {
        for (int index = 0; index < 5; ++index)
        {
          string professionBlurb = "";
          string professionTitle = "";
          bool flag = false;
          int whichProfession = -1;
          switch (index)
          {
            case 0:
              flag = Game1.player.FarmingLevel > num8;
              whichProfession = Game1.player.getProfessionForSkill(0, num8 + 1);
              this.parseProfessionDescription(ref professionBlurb, ref professionTitle, LevelUpMenu.getProfessionDescription(whichProfession));
              break;
            case 1:
              flag = Game1.player.MiningLevel > num8;
              whichProfession = Game1.player.getProfessionForSkill(3, num8 + 1);
              this.parseProfessionDescription(ref professionBlurb, ref professionTitle, LevelUpMenu.getProfessionDescription(whichProfession));
              break;
            case 2:
              flag = Game1.player.ForagingLevel > num8;
              whichProfession = Game1.player.getProfessionForSkill(2, num8 + 1);
              this.parseProfessionDescription(ref professionBlurb, ref professionTitle, LevelUpMenu.getProfessionDescription(whichProfession));
              break;
            case 3:
              flag = Game1.player.FishingLevel > num8;
              whichProfession = Game1.player.getProfessionForSkill(1, num8 + 1);
              this.parseProfessionDescription(ref professionBlurb, ref professionTitle, LevelUpMenu.getProfessionDescription(whichProfession));
              break;
            case 4:
              flag = Game1.player.CombatLevel > num8;
              whichProfession = Game1.player.getProfessionForSkill(4, num8 + 1);
              this.parseProfessionDescription(ref professionBlurb, ref professionTitle, LevelUpMenu.getProfessionDescription(whichProfession));
              break;
            case 5:
              flag = Game1.player.LuckLevel > num8;
              whichProfession = Game1.player.getProfessionForSkill(5, num8 + 1);
              this.parseProfessionDescription(ref professionBlurb, ref professionTitle, LevelUpMenu.getProfessionDescription(whichProfession));
              break;
          }
          if (flag && (num8 + 1) % 5 == 0)
          {
            List<ClickableTextureComponent> skillBars = this.skillBars;
            ClickableTextureComponent textureComponent = new ClickableTextureComponent(string.Concat((object) whichProfession), new Rectangle(num5 + num6 - Game1.pixelZoom + num8 * (Game1.tileSize / 2 + Game1.pixelZoom), num7 + index * (Game1.tileSize / 2 + Game1.pixelZoom * 6), 14 * Game1.pixelZoom, 9 * Game1.pixelZoom), (string) null, professionBlurb, Game1.mouseCursors, new Rectangle(159, 338, 14, 9), (float) Game1.pixelZoom, true);
            int num1 = num8 + 1 == 5 ? 100 + index : 200 + index;
            textureComponent.myID = num1;
            int num2 = num8 + 1 == 5 ? index : 100 + index;
            textureComponent.leftNeighborID = num2;
            int num3 = num8 + 1 == 5 ? 200 + index : -1;
            textureComponent.rightNeighborID = num3;
            int num4 = 10201;
            textureComponent.downNeighborID = num4;
            skillBars.Add(textureComponent);
          }
        }
        num5 += Game1.pixelZoom * 6;
        num8 += 5;
      }
      for (int index = 0; index < this.skillBars.Count; ++index)
      {
        if (index < this.skillBars.Count - 1 && Math.Abs(this.skillBars[index + 1].myID - this.skillBars[index].myID) < 50)
        {
          this.skillBars[index].downNeighborID = this.skillBars[index + 1].myID;
          this.skillBars[index + 1].upNeighborID = this.skillBars[index].myID;
        }
      }
      if (this.skillBars.Count > 1 && this.skillBars.Last<ClickableTextureComponent>().myID >= 200 && this.skillBars[this.skillBars.Count - 2].myID >= 200)
        this.skillBars.Last<ClickableTextureComponent>().upNeighborID = this.skillBars[this.skillBars.Count - 2].myID;
      for (int index = 0; index < 5; ++index)
      {
        int num1 = index;
        switch (num1)
        {
          case 1:
            num1 = 3;
            break;
          case 3:
            num1 = 1;
            break;
        }
        string hoverText = "";
        switch (num1)
        {
          case 0:
            if (Game1.player.FarmingLevel > 0)
            {
              hoverText = Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11592", (object) Game1.player.FarmingLevel) + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11594", (object) Game1.player.FarmingLevel);
              break;
            }
            break;
          case 1:
            if (Game1.player.FishingLevel > 0)
            {
              hoverText = Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11598", (object) Game1.player.FishingLevel);
              break;
            }
            break;
          case 2:
            if (Game1.player.ForagingLevel > 0)
            {
              hoverText = Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11596", (object) Game1.player.ForagingLevel);
              break;
            }
            break;
          case 3:
            if (Game1.player.MiningLevel > 0)
            {
              hoverText = Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11600", (object) Game1.player.MiningLevel);
              break;
            }
            break;
          case 4:
            if (Game1.player.CombatLevel > 0)
            {
              hoverText = Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11602", (object) (Game1.player.CombatLevel * 5));
              break;
            }
            break;
        }
        List<ClickableTextureComponent> skillAreas = this.skillAreas;
        ClickableTextureComponent textureComponent = new ClickableTextureComponent(string.Concat((object) num1), new Rectangle(num6 - Game1.tileSize * 2 - Game1.tileSize * 3 / 4, num7 + index * (Game1.tileSize / 2 + Game1.pixelZoom * 6), Game1.tileSize * 2 + Game1.pixelZoom * 5, 9 * Game1.pixelZoom), string.Concat((object) num1), hoverText, (Texture2D) null, Rectangle.Empty, 1f, false);
        int num2 = index;
        textureComponent.myID = num2;
        int num3 = index < 4 ? index + 1 : 10201;
        textureComponent.downNeighborID = num3;
        int num4 = index > 0 ? index - 1 : 12341;
        textureComponent.upNeighborID = num4;
        int num9 = 100 + index;
        textureComponent.rightNeighborID = num9;
        skillAreas.Add(textureComponent);
      }
    }

    private void parseProfessionDescription(ref string professionBlurb, ref string professionTitle, List<string> professionDescription)
    {
      if (professionDescription.Count <= 0)
        return;
      professionTitle = professionDescription[0];
      for (int index = 1; index < professionDescription.Count; ++index)
      {
        professionBlurb = professionBlurb + professionDescription[index];
        if (index < professionDescription.Count - 1)
          professionBlurb = professionBlurb + Environment.NewLine;
      }
    }

    public override void snapToDefaultClickableComponent()
    {
      this.currentlySnappedComponent = this.skillAreas.Count > 0 ? this.getComponentWithID(0) : (ClickableComponent) null;
      if (this.currentlySnappedComponent == null || !Game1.options.snappyMenus || !Game1.options.gamepadControls)
        return;
      this.currentlySnappedComponent.snapMouseCursorToCenter();
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
    }

    public override void performHoverAction(int x, int y)
    {
      this.hoverText = "";
      this.hoverTitle = "";
      this.professionImage = -1;
      foreach (ClickableTextureComponent specialItem in this.specialItems)
      {
        if (specialItem.containsPoint(x, y))
        {
          this.hoverText = specialItem.hoverText;
          break;
        }
      }
      foreach (ClickableTextureComponent skillBar in this.skillBars)
      {
        skillBar.scale = (float) Game1.pixelZoom;
        if (skillBar.containsPoint(x, y) && skillBar.hoverText.Length > 0 && !skillBar.name.Equals("-1"))
        {
          this.hoverText = skillBar.hoverText;
          this.hoverTitle = LevelUpMenu.getProfessionTitleFromNumber(Convert.ToInt32(skillBar.name));
          this.professionImage = Convert.ToInt32(skillBar.name);
          skillBar.scale = 0.0f;
        }
      }
      foreach (ClickableTextureComponent skillArea in this.skillAreas)
      {
        if (skillArea.containsPoint(x, y) && skillArea.hoverText.Length > 0)
        {
          this.hoverText = skillArea.hoverText;
          this.hoverTitle = Farmer.getSkillDisplayNameFromIndex(Convert.ToInt32(skillArea.name));
          break;
        }
      }
      if (this.playerPanel.Contains(x, y))
      {
        this.playerPanelTimer = this.playerPanelTimer - Game1.currentGameTime.ElapsedGameTime.Milliseconds;
        if (this.playerPanelTimer > 0)
          return;
        this.playerPanelIndex = (this.playerPanelIndex + 1) % 4;
        this.playerPanelTimer = 150;
      }
      else
        this.playerPanelIndex = 0;
    }

    public override void draw(SpriteBatch b)
    {
      Game1.drawDialogueBox(this.xPositionOnScreen, this.yPositionOnScreen, this.width, this.height, false, true, (string) null, false);
      int num1 = this.xPositionOnScreen + Game1.tileSize - Game1.pixelZoom * 3;
      int num2 = this.yPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder;
      b.Draw(Game1.timeOfDay >= 1900 ? Game1.nightbg : Game1.daybg, new Vector2((float) num1, (float) num2), Color.White);
      Game1.player.FarmerRenderer.draw(b, new FarmerSprite.AnimationFrame(Game1.player.bathingClothes ? 108 : this.playerPanelFrames[this.playerPanelIndex], 0, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false), Game1.player.bathingClothes ? 108 : this.playerPanelFrames[this.playerPanelIndex], new Rectangle(this.playerPanelFrames[this.playerPanelIndex] * 16, Game1.player.bathingClothes ? 576 : 0, 16, 32), new Vector2((float) (num1 + Game1.tileSize / 2), (float) (num2 + Game1.tileSize / 2)), Vector2.Zero, 0.8f, 2, Color.White, 0.0f, 1f, Game1.player);
      if (Game1.timeOfDay >= 1900)
        Game1.player.FarmerRenderer.draw(b, new FarmerSprite.AnimationFrame(this.playerPanelFrames[this.playerPanelIndex], 0, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false), this.playerPanelFrames[this.playerPanelIndex], new Rectangle(this.playerPanelFrames[this.playerPanelIndex] * 16, 0, 16, 32), new Vector2((float) (num1 + Game1.tileSize / 2), (float) (num2 + Game1.tileSize / 2)), Vector2.Zero, 0.8f, 2, Color.DarkBlue * 0.3f, 0.0f, 1f, Game1.player);
      b.DrawString(Game1.smallFont, Game1.player.name, new Vector2((float) (num1 + Game1.tileSize) - Game1.smallFont.MeasureString(Game1.player.name).X / 2f, (float) (num2 + 3 * Game1.tileSize + 4)), Game1.textColor);
      b.DrawString(Game1.smallFont, Game1.player.getTitle(), new Vector2((float) (num1 + Game1.tileSize) - Game1.smallFont.MeasureString(Game1.player.getTitle()).X / 2f, (float) (num2 + 4 * Game1.tileSize - Game1.tileSize / 2)), Game1.textColor);
      int num3 = LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ru ? this.xPositionOnScreen + this.width - Game1.tileSize * 7 - Game1.tileSize * 3 / 4 : this.xPositionOnScreen + IClickableMenu.borderWidth + IClickableMenu.spaceToClearTopBorder + 4 * Game1.tileSize - 8;
      int num4 = this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + IClickableMenu.borderWidth - Game1.pixelZoom * 2;
      int num5 = 0;
      for (int index1 = 0; index1 < 10; ++index1)
      {
        for (int index2 = 0; index2 < 5; ++index2)
        {
          bool flag1 = false;
          bool flag2 = false;
          string text = "";
          int number = 0;
          Rectangle rectangle = Rectangle.Empty;
          switch (index2)
          {
            case 0:
              flag1 = Game1.player.FarmingLevel > index1;
              if (index1 == 0)
                text = Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11604");
              number = Game1.player.FarmingLevel;
              flag2 = Game1.player.addedFarmingLevel > 0;
              rectangle = new Rectangle(10, 428, 10, 10);
              break;
            case 1:
              flag1 = Game1.player.MiningLevel > index1;
              if (index1 == 0)
                text = Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11605");
              number = Game1.player.MiningLevel;
              flag2 = Game1.player.addedMiningLevel > 0;
              rectangle = new Rectangle(30, 428, 10, 10);
              break;
            case 2:
              flag1 = Game1.player.ForagingLevel > index1;
              if (index1 == 0)
                text = Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11606");
              number = Game1.player.ForagingLevel;
              flag2 = Game1.player.addedForagingLevel > 0;
              rectangle = new Rectangle(60, 428, 10, 10);
              break;
            case 3:
              flag1 = Game1.player.FishingLevel > index1;
              if (index1 == 0)
                text = Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11607");
              number = Game1.player.FishingLevel;
              flag2 = Game1.player.addedFishingLevel > 0;
              rectangle = new Rectangle(20, 428, 10, 10);
              break;
            case 4:
              flag1 = Game1.player.CombatLevel > index1;
              if (index1 == 0)
                text = Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11608");
              number = Game1.player.CombatLevel;
              flag2 = Game1.player.addedCombatLevel > 0;
              rectangle = new Rectangle(120, 428, 10, 10);
              break;
            case 5:
              flag1 = Game1.player.LuckLevel > index1;
              if (index1 == 0)
                text = Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11609");
              number = Game1.player.LuckLevel;
              flag2 = Game1.player.addedLuckLevel > 0;
              rectangle = new Rectangle(50, 428, 10, 10);
              break;
          }
          if (!text.Equals(""))
          {
            b.DrawString(Game1.smallFont, text, new Vector2((float) num3 - Game1.smallFont.MeasureString(text).X + (float) Game1.pixelZoom - (float) Game1.tileSize, (float) (num4 + Game1.pixelZoom + index2 * (Game1.tileSize / 2 + Game1.pixelZoom * 6))), Game1.textColor);
            b.Draw(Game1.mouseCursors, new Vector2((float) (num3 - Game1.pixelZoom * 14), (float) (num4 + index2 * (Game1.tileSize / 2 + Game1.pixelZoom * 6))), new Rectangle?(rectangle), Color.Black * 0.3f, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.85f);
            b.Draw(Game1.mouseCursors, new Vector2((float) (num3 - Game1.pixelZoom * 13), (float) (num4 - Game1.pixelZoom + index2 * (Game1.tileSize / 2 + Game1.pixelZoom * 6))), new Rectangle?(rectangle), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.87f);
          }
          if (!flag1 && (index1 + 1) % 5 == 0)
          {
            b.Draw(Game1.mouseCursors, new Vector2((float) (num5 + num3 - Game1.pixelZoom + index1 * (Game1.tileSize / 2 + Game1.pixelZoom)), (float) (num4 + index2 * (Game1.tileSize / 2 + Game1.pixelZoom * 6))), new Rectangle?(new Rectangle(145, 338, 14, 9)), Color.Black * 0.35f, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.87f);
            b.Draw(Game1.mouseCursors, new Vector2((float) (num5 + num3 + index1 * (Game1.tileSize / 2 + Game1.pixelZoom)), (float) (num4 - Game1.pixelZoom + index2 * (Game1.tileSize / 2 + Game1.pixelZoom * 6))), new Rectangle?(new Rectangle(145 + (flag1 ? 14 : 0), 338, 14, 9)), Color.White * (flag1 ? 1f : 0.65f), 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.87f);
          }
          else if ((index1 + 1) % 5 != 0)
          {
            b.Draw(Game1.mouseCursors, new Vector2((float) (num5 + num3 - Game1.pixelZoom + index1 * (Game1.tileSize / 2 + Game1.pixelZoom)), (float) (num4 + index2 * (Game1.tileSize / 2 + Game1.pixelZoom * 6))), new Rectangle?(new Rectangle(129, 338, 8, 9)), Color.Black * 0.35f, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.85f);
            b.Draw(Game1.mouseCursors, new Vector2((float) (num5 + num3 + index1 * (Game1.tileSize / 2 + Game1.pixelZoom)), (float) (num4 - Game1.pixelZoom + index2 * (Game1.tileSize / 2 + Game1.pixelZoom * 6))), new Rectangle?(new Rectangle(129 + (flag1 ? 8 : 0), 338, 8, 9)), Color.White * (flag1 ? 1f : 0.65f), 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.87f);
          }
          if (index1 == 9)
          {
            NumberSprite.draw(number, b, new Vector2((float) (num5 + num3 + (index1 + 2) * (Game1.tileSize / 2 + Game1.pixelZoom) + Game1.pixelZoom * 3 + (number >= 10 ? Game1.pixelZoom * 3 : 0)), (float) (num4 + Game1.pixelZoom * 4 + index2 * (Game1.tileSize / 2 + Game1.pixelZoom * 6))), Color.Black * 0.35f, 1f, 0.85f, 1f, 0, 0);
            NumberSprite.draw(number, b, new Vector2((float) (num5 + num3 + (index1 + 2) * (Game1.tileSize / 2 + Game1.pixelZoom) + Game1.pixelZoom * 4 + (number >= 10 ? Game1.pixelZoom * 3 : 0)), (float) (num4 + Game1.pixelZoom * 3 + index2 * (Game1.tileSize / 2 + Game1.pixelZoom * 6))), (flag2 ? Color.LightGreen : Color.SandyBrown) * (number == 0 ? 0.75f : 1f), 1f, 0.87f, 1f, 0, 0);
          }
        }
        if ((index1 + 1) % 5 == 0)
          num5 += Game1.pixelZoom * 6;
      }
      foreach (ClickableTextureComponent skillBar in this.skillBars)
        skillBar.draw(b);
      foreach (ClickableTextureComponent skillBar in this.skillBars)
      {
        if ((double) skillBar.scale == 0.0)
        {
          IClickableMenu.drawTextureBox(b, skillBar.bounds.X - Game1.tileSize / 4 - Game1.pixelZoom * 2, skillBar.bounds.Y - Game1.tileSize / 4 - Game1.pixelZoom * 4, Game1.tileSize * 5 / 4 + Game1.pixelZoom * 4, Game1.tileSize * 5 / 4 + Game1.pixelZoom * 4, Color.White);
          b.Draw(Game1.mouseCursors, new Vector2((float) (skillBar.bounds.X - Game1.pixelZoom * 2), (float) (skillBar.bounds.Y - Game1.tileSize / 2 + Game1.tileSize / 4)), new Rectangle?(new Rectangle(this.professionImage % 6 * 16, 624 + this.professionImage / 6 * 16, 16, 16)), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 1f);
        }
      }
      Game1.drawDialogueBox(this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize / 2, this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + (int) ((double) this.height / 2.0) - Game1.tileSize / 2, this.width - Game1.tileSize - IClickableMenu.spaceToClearSideBorder * 2, this.height / 4 + Game1.tileSize, false, true, (string) null, false);
      this.drawBorderLabel(b, Game1.content.LoadString("Strings\\StringsFromCSFiles:SkillsPage.cs.11610"), Game1.smallFont, this.xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + Game1.tileSize * 3 / 2, this.yPositionOnScreen + IClickableMenu.spaceToClearTopBorder + (int) ((double) this.height / 2.0) - Game1.tileSize / 2);
      foreach (ClickableTextureComponent specialItem in this.specialItems)
        specialItem.draw(b);
      if (this.hoverText.Length <= 0)
        return;
      IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, -1, this.hoverTitle.Length > 0 ? this.hoverTitle : (string) null, -1, (string[]) null, (Item) null, 0, -1, -1, -1, -1, 1f, (CraftingRecipe) null);
    }
  }
}
