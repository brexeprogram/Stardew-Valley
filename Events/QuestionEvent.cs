// Decompiled with JetBrains decompiler
// Type: StardewValley.Events.QuestionEvent
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Buildings;
using StardewValley.Menus;

namespace StardewValley.Events
{
  public class QuestionEvent : FarmEvent
  {
    public const int pregnancyQuestion = 1;
    public const int barnBirth = 2;
    private int whichQuestion;
    private AnimalHouse animalHouse;
    public FarmAnimal animal;
    public bool forceProceed;

    public QuestionEvent(int whichQuestion)
    {
      this.whichQuestion = whichQuestion;
    }

    public bool setUp()
    {
      switch (this.whichQuestion)
      {
        case 1:
          Response[] answerChoices = new Response[2]
          {
            new Response("Yes", Game1.content.LoadString("Strings\\Events:HaveBabyAnswer_Yes")),
            new Response("Not", Game1.content.LoadString("Strings\\Events:HaveBabyAnswer_No"))
          };
          if (!Game1.getCharacterFromName(Game1.player.spouse, false).isGaySpouse())
            Game1.currentLocation.createQuestionDialogue(Game1.content.LoadString("Strings\\Events:HaveBabyQuestion", (object) Game1.player.name), answerChoices, new GameLocation.afterQuestionBehavior(this.answerPregnancyQuestion), Game1.getCharacterFromName(Game1.player.spouse, false));
          else
            Game1.currentLocation.createQuestionDialogue(Game1.content.LoadString("Strings\\Events:HaveBabyQuestion_Adoption", (object) Game1.player.name), answerChoices, new GameLocation.afterQuestionBehavior(this.answerPregnancyQuestion), Game1.getCharacterFromName(Game1.player.spouse, false));
          Game1.messagePause = true;
          return false;
        case 2:
          FarmAnimal farmAnimal = (FarmAnimal) null;
          foreach (Building building in Game1.getFarm().buildings)
          {
            if ((building.owner.Equals((object) Game1.uniqueIDForThisGame) || !Game1.IsMultiplayer) && (building.buildingType.Contains("Barn") && !building.buildingType.Equals("Barn")) && (!(building.indoors as AnimalHouse).isFull() && Game1.random.NextDouble() < (double) (building.indoors as AnimalHouse).animalsThatLiveHere.Count * 0.0055))
            {
              farmAnimal = Utility.getAnimal((building.indoors as AnimalHouse).animalsThatLiveHere[Game1.random.Next((building.indoors as AnimalHouse).animalsThatLiveHere.Count)]);
              this.animalHouse = building.indoors as AnimalHouse;
              break;
            }
          }
          if (farmAnimal != null && !farmAnimal.isBaby() && farmAnimal.allowReproduction)
          {
            Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Events:AnimalBirth", (object) farmAnimal.displayName, (object) farmAnimal.shortDisplayType()));
            Game1.messagePause = true;
            this.animal = farmAnimal;
            return false;
          }
          break;
      }
      return true;
    }

    private void answerPregnancyQuestion(Farmer who, string answer)
    {
      if (answer.Equals("Yes"))
      {
        Game1.getCharacterFromName(who.spouse, false).daysUntilBirthing = 14;
        Game1.getCharacterFromName(who.spouse, false).isGaySpouse();
      }
      Game1.player.position = Utility.PointToVector2(Utility.getHomeOfFarmer(Game1.player).getBedSpot()) * (float) Game1.tileSize;
    }

    public bool tickUpdate(GameTime time)
    {
      if (this.forceProceed)
        return true;
      if (this.whichQuestion != 2 || Game1.dialogueUp)
        return !Game1.dialogueUp;
      if (Game1.activeClickableMenu == null)
      {
        NamingMenu.doneNamingBehavior b = new NamingMenu.doneNamingBehavior(this.animalHouse.addNewHatchedAnimal);
        string title;
        if (this.animal == null)
          title = Game1.content.LoadString("Strings\\StringsFromCSFiles:QuestionEvent.cs.6692");
        else
          title = Game1.content.LoadString("Strings\\Events:AnimalNamingTitle", (object) this.animal.displayType);
        // ISSUE: variable of the null type
        __Null local = null;
        Game1.activeClickableMenu = (IClickableMenu) new NamingMenu(b, title, (string) local);
      }
      return false;
    }

    public void draw(SpriteBatch b)
    {
    }

    public void drawAboveEverything(SpriteBatch b)
    {
    }

    public void makeChangesToLocation()
    {
      Game1.messagePause = false;
      Game1.player.position = Game1.player.mostRecentBed;
    }
  }
}
