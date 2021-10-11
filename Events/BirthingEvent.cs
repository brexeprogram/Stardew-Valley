// Decompiled with JetBrains decompiler
// Type: StardewValley.Events.BirthingEvent
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.BellsAndWhistles;
using StardewValley.Characters;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Events
{
  public class BirthingEvent : FarmEvent
  {
    private int behavior;
    private int timer;
    private string soundName;
    private string message;
    private string babyName;
    private bool playedSound;
    private bool showedMessage;
    private bool isMale;
    private bool getBabyName;
    private bool naming;
    private Vector2 targetLocation;
    private TextBox babyNameBox;
    private ClickableTextureComponent okButton;

    public bool setUp()
    {
      Random random = new Random((int) Game1.uniqueIDForThisGame + (int) Game1.stats.DaysPlayed);
      Utility.getHomeOfFarmer(Game1.player);
      NPC characterFromName = Game1.getCharacterFromName(Game1.player.spouse, false);
      Game1.player.CanMove = false;
      this.isMale = Game1.player.getNumberOfChildren() != 0 ? Game1.player.getChildren()[0].gender == 1 : random.NextDouble() < 0.5;
      if (characterFromName.isGaySpouse())
        this.message = Game1.content.LoadString("Strings\\Events:BirthMessage_Adoption", (object) Lexicon.getGenderedChildTerm(this.isMale));
      else if (characterFromName.gender == 0)
        this.message = Game1.content.LoadString("Strings\\Events:BirthMessage_PlayerMother", (object) Lexicon.getGenderedChildTerm(this.isMale));
      else
        this.message = Game1.content.LoadString("Strings\\Events:BirthMessage_SpouseMother", (object) Lexicon.getGenderedChildTerm(this.isMale), (object) characterFromName.displayName);
      return false;
    }

    public void returnBabyName(string name)
    {
      this.babyName = name;
      Game1.exitActiveMenu();
    }

    public void afterMessage()
    {
      this.getBabyName = true;
    }

    public bool tickUpdate(GameTime time)
    {
      Game1.player.CanMove = false;
      this.timer = this.timer + time.ElapsedGameTime.Milliseconds;
      Game1.fadeToBlackAlpha = 1f;
      if (this.timer > 1500 && !this.playedSound && !this.getBabyName)
      {
        if (this.soundName != null && !this.soundName.Equals(""))
        {
          Game1.playSound(this.soundName);
          this.playedSound = true;
        }
        if (!this.playedSound && this.message != null && (!Game1.dialogueUp && Game1.activeClickableMenu == null))
        {
          Game1.drawObjectDialogue(this.message);
          Game1.afterDialogues = new Game1.afterFadeFunction(this.afterMessage);
        }
      }
      else if (this.getBabyName)
      {
        if (!this.naming)
        {
          Game1.activeClickableMenu = (IClickableMenu) new NamingMenu(new NamingMenu.doneNamingBehavior(this.returnBabyName), Game1.content.LoadString(this.isMale ? "Strings\\Events:BabyNamingTitle_Male" : "Strings\\Events:BabyNamingTitle_Female"), "");
          this.naming = true;
        }
        if (this.babyName != null && this.babyName != "" && this.babyName.Length > 0)
        {
          double num1 = (Game1.player.spouse.Equals("Maru") ? 0.5 : 0.0) + (Game1.player.hasDarkSkin() ? 0.5 : 0.0);
          bool isDarkSkinned = new Random((int) Game1.uniqueIDForThisGame + (int) Game1.stats.DaysPlayed).NextDouble() < num1;
          string babyName = this.babyName;
          foreach (Character allCharacter in Utility.getAllCharacters())
          {
            if (allCharacter.name.Equals(babyName))
            {
              babyName += " ";
              break;
            }
          }
          Utility.getHomeOfFarmer(Game1.player).characters.Add((NPC) new Child(babyName, this.isMale, isDarkSkinned, Game1.player));
          Game1.playSound("smallSelect");
          Game1.player.getSpouse().daysAfterLastBirth = 5;
          Game1.player.getSpouse().daysUntilBirthing = -1;
          if (Game1.player.getChildren().Count == 2)
          {
            NPC spouse = Game1.player.getSpouse();
            string s;
            if (Game1.random.NextDouble() >= 0.5)
            {
              if (Game1.player.getSpouse().gender != 0)
                s = ((IEnumerable<string>) Game1.content.LoadString("Data\\ExtraDialogue:NewChild_SecondChild2").Split('/')).Last<string>();
              else
                s = ((IEnumerable<string>) Game1.content.LoadString("Data\\ExtraDialogue:NewChild_SecondChild2").Split('/')).First<string>();
            }
            else if (Game1.player.getSpouse().gender != 0)
              s = ((IEnumerable<string>) Game1.content.LoadString("Data\\ExtraDialogue:NewChild_SecondChild1").Split('/')).Last<string>();
            else
              s = ((IEnumerable<string>) Game1.content.LoadString("Data\\ExtraDialogue:NewChild_SecondChild1").Split('/')).First<string>();
            int num2 = 0;
            int num3 = 0;
            spouse.setNewDialogue(s, num2 != 0, num3 != 0);
            Game1.getSteamAchievement("Achievement_FullHouse");
          }
          else if (Game1.player.getSpouse().isGaySpouse())
          {
            NPC spouse = Game1.player.getSpouse();
            string s;
            if (Game1.player.getSpouse().gender != 0)
              s = ((IEnumerable<string>) Game1.content.LoadString("Data\\ExtraDialogue:NewChild_Adoption", (object) this.babyName).Split('/')).Last<string>();
            else
              s = ((IEnumerable<string>) Game1.content.LoadString("Data\\ExtraDialogue:NewChild_Adoption", (object) this.babyName).Split('/')).First<string>();
            int num2 = 0;
            int num3 = 0;
            spouse.setNewDialogue(s, num2 != 0, num3 != 0);
          }
          else
          {
            NPC spouse = Game1.player.getSpouse();
            string s;
            if (Game1.player.getSpouse().gender != 0)
              s = ((IEnumerable<string>) Game1.content.LoadString("Data\\ExtraDialogue:NewChild_FirstChild", (object) this.babyName).Split('/')).Last<string>();
            else
              s = ((IEnumerable<string>) Game1.content.LoadString("Data\\ExtraDialogue:NewChild_FirstChild", (object) this.babyName).Split('/')).First<string>();
            int num2 = 0;
            int num3 = 0;
            spouse.setNewDialogue(s, num2 != 0, num3 != 0);
          }
          if (Game1.keyboardDispatcher != null)
            Game1.keyboardDispatcher.Subscriber = (IKeyboardSubscriber) null;
          Game1.player.position = Utility.PointToVector2(Utility.getHomeOfFarmer(Game1.player).getBedSpot()) * (float) Game1.tileSize;
          Game1.globalFadeToClear((Game1.afterFadeFunction) null, 0.02f);
          return true;
        }
      }
      return false;
    }

    public void draw(SpriteBatch b)
    {
    }

    public void makeChangesToLocation()
    {
    }

    public void drawAboveEverything(SpriteBatch b)
    {
    }
  }
}
