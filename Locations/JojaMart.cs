// Decompiled with JetBrains decompiler
// Type: StardewValley.Locations.JojaMart
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;
using System.Collections.Generic;
using xTile;
using xTile.Dimensions;
using xTile.ObjectModel;

namespace StardewValley.Locations
{
  public class JojaMart : GameLocation
  {
    public const int JojaMembershipPrice = 5000;
    public static NPC Morris;
    private Texture2D communityDevelopmentTexture;

    public JojaMart()
    {
    }

    public JojaMart(Map map, string name)
      : base(map, name)
    {
    }

    private bool signUpForJoja(int response)
    {
      if (response == 0)
      {
        this.createQuestionDialogue(Game1.parseText(Game1.content.LoadString("Strings\\Locations:JojaMart_SignUp")), this.createYesNoResponses(), "JojaSignUp");
        return true;
      }
      Game1.dialogueUp = false;
      Game1.player.forceCanMove();
      Game1.playSound("smallSelect");
      Game1.currentSpeaker = (NPC) null;
      Game1.dialogueTyping = false;
      return true;
    }

    public override bool answerDialogue(Response answer)
    {
      if (!(this.lastQuestionKey.Split(' ')[0] + "_" + answer.responseKey == "JojaSignUp_Yes"))
        return base.answerDialogue(answer);
      if (Game1.player.Money >= 5000)
      {
        Game1.player.Money -= 5000;
        Game1.addMailForTomorrow("JojaMember", true, true);
        Game1.player.removeQuest(26);
        JojaMart.Morris.setNewDialogue(Game1.content.LoadString("Data\\ExtraDialogue:Morris_PlayerSignedUp"), false, false);
        Game1.drawDialogue(JojaMart.Morris);
      }
      else if (Game1.player.Money < 5000)
        Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\UI:NotEnoughMoney1"));
      return true;
    }

    public override void cleanupBeforePlayerExit()
    {
      if (!Game1.isRaining)
        Game1.changeMusicTrack("none");
      base.cleanupBeforePlayerExit();
    }

    public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
    {
      if (this.map.GetLayer("Buildings").Tiles[tileLocation] != null)
      {
        PropertyValue propertyValue = (PropertyValue) "";
        this.map.GetLayer("Buildings").Tiles[tileLocation].Properties.TryGetValue("Action", out propertyValue);
        if (propertyValue != null)
        {
          string str1 = propertyValue.ToString();
          if (!(str1 == "JojaShop") && str1 == "JoinJoja")
          {
            JojaMart.Morris.CurrentDialogue.Clear();
            if (Game1.player.mailForTomorrow.Contains("JojaMember%&NL&%"))
            {
              JojaMart.Morris.setNewDialogue(Game1.content.LoadString("Data\\ExtraDialogue:Morris_ComeBackLater"), false, false);
              Game1.drawDialogue(JojaMart.Morris);
            }
            else if (!Game1.player.mailReceived.Contains("JojaMember"))
            {
              if (!Game1.player.mailReceived.Contains("JojaGreeting"))
              {
                JojaMart.Morris.setNewDialogue(Game1.content.LoadString("Data\\ExtraDialogue:Morris_Greeting"), false, false);
                Game1.player.mailReceived.Add("JojaGreeting");
                Game1.drawDialogue(JojaMart.Morris);
              }
              else if (Game1.stats.DaysPlayed < 0U)
              {
                string path = Game1.dayOfMonth % 7 == 0 || Game1.dayOfMonth % 7 == 6 ? "Data\\ExtraDialogue:Morris_WeekendGreeting" : "Data\\ExtraDialogue:Morris_FirstGreeting";
                JojaMart.Morris.setNewDialogue(Game1.content.LoadString(path), false, false);
                Game1.drawDialogue(JojaMart.Morris);
              }
              else
              {
                string str2 = Game1.dayOfMonth % 7 == 0 || Game1.dayOfMonth % 7 == 6 ? "Data\\ExtraDialogue:Morris_WeekendGreeting" : "Data\\ExtraDialogue:Morris_FirstGreeting";
                if (!Game1.IsMultiplayer || Game1.IsServer)
                {
                  JojaMart.Morris.setNewDialogue(Game1.content.LoadString(str2 + "_MembershipAvailable", (object) 5000), false, false);
                  JojaMart.Morris.CurrentDialogue.Peek().answerQuestionBehavior = new Dialogue.onAnswerQuestion(this.signUpForJoja);
                }
                else
                  JojaMart.Morris.setNewDialogue(str2 + "_SecondPlayer", false, false);
                Game1.drawDialogue(JojaMart.Morris);
              }
            }
            else
            {
              if (Game1.player.mailForTomorrow.Contains("jojaFishTank%&NL&%") || Game1.player.mailForTomorrow.Contains("jojaPantry%&NL&%") || (Game1.player.mailForTomorrow.Contains("jojaCraftsRoom%&NL&%") || Game1.player.mailForTomorrow.Contains("jojaBoilerRoom%&NL&%")) || Game1.player.mailForTomorrow.Contains("jojaVault%&NL&%"))
              {
                JojaMart.Morris.setNewDialogue(Game1.content.LoadString("Data\\ExtraDialogue:Morris_StillProcessingOrder"), false, false);
              }
              else
              {
                if (Game1.player.isMale)
                  JojaMart.Morris.setNewDialogue(Game1.content.LoadString("Data\\ExtraDialogue:Morris_CommunityDevelopmentForm_PlayerMale"), false, false);
                else
                  JojaMart.Morris.setNewDialogue(Game1.content.LoadString("Data\\ExtraDialogue:Morris_CommunityDevelopmentForm_PlayerFemale"), false, false);
                JojaMart.Morris.CurrentDialogue.Peek().answerQuestionBehavior = new Dialogue.onAnswerQuestion(this.viewJojaNote);
              }
              Game1.drawDialogue(JojaMart.Morris);
            }
          }
        }
      }
      return base.checkAction(tileLocation, viewport, who);
    }

    private bool viewJojaNote(int response)
    {
      if (response == 0)
        Game1.activeClickableMenu = (IClickableMenu) new JojaCDMenu(this.communityDevelopmentTexture);
      Game1.dialogueUp = false;
      Game1.player.forceCanMove();
      Game1.playSound("smallSelect");
      Game1.currentSpeaker = (NPC) null;
      Game1.dialogueTyping = false;
      return true;
    }

    public override void resetForPlayerEntry()
    {
      this.communityDevelopmentTexture = Game1.temporaryContent.Load<Texture2D>("LooseSprites\\JojaCDForm");
      JojaMart.Morris = new NPC((AnimatedSprite) null, Vector2.Zero, nameof (JojaMart), 2, "Morris", false, (Dictionary<int, int[]>) null, Game1.temporaryContent.Load<Texture2D>("Portraits\\Morris"));
      base.resetForPlayerEntry();
    }
  }
}
