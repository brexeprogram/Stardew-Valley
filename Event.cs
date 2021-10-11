// Decompiled with JetBrains decompiler
// Type: StardewValley.Event
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using StardewValley.Characters;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Minigames;
using StardewValley.Objects;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using xTile;
using xTile.Dimensions;

namespace StardewValley
{
  public class Event
  {
    public int oldPixelZoom = Game1.pixelZoom;
    public List<NPC> actors = new List<NPC>();
    public List<Object> props = new List<Object>();
    public List<Prop> festivalProps = new List<Prop>();
    public bool showGroundObjects = true;
    public List<NPC> npcsWithUniquePortraits = new List<NPC>();
    private LocalizedContentManager festivalContent = Game1.content.CreateTemporary();
    public List<Vector2> characterWalkLocations = new List<Vector2>();
    public int grangeScore = -1000;
    private int previousFacingDirection = -1;
    private int previousAnswerChoice = -1;
    private const float timeBetweenSpeech = 500f;
    private const float viewportMoveSpeed = 3f;
    public string[] eventCommands;
    public int currentCommand;
    public int readyConfirmationTimer;
    public int farmerAddedSpeed;
    public string messageToScreen;
    public string playerControlSequenceID;
    public bool showActiveObject;
    public bool continueAfterMove;
    public bool specialEventVariable1;
    public bool forked;
    public bool wasBloomDay;
    public bool wasBloomVisible;
    public bool playerControlSequence;
    public bool eventSwitched;
    public bool isFestival;
    public bool sentReadyConfirmation;
    public bool allPlayersReady;
    public bool playerWasMounted;
    private Dictionary<string, Vector3> actorPositionsAfterMove;
    private float timeAccumulator;
    private float viewportXAccumulator;
    private float viewportYAccumulator;
    private Vector3 viewportTarget;
    private Color previousAmbientLight;
    private BloomSettings previousBloomSettings;
    private GameLocation temporaryLocation;
    public Point playerControlTargetTile;
    private Texture2D _festivalTexture;
    public List<NPCController> npcControllers;
    public NPC secretSantaRecipient;
    public NPC mySecretSanta;
    public bool skippable;
    private int id;
    private Dictionary<string, string> festivalData;
    private int oldShirt;
    private Color oldPants;
    private Item tmpItem;
    private bool drawTool;
    public bool skipped;
    private bool waitingForMenuClose;
    private int oldTime;
    public List<TemporaryAnimatedSprite> underwaterSprites;
    public List<TemporaryAnimatedSprite> aboveMapSprites;
    private NPC festivalHost;
    private string hostMessage;
    public int festivalTimer;
    private Item tempItemStash;
    public Farmer playerUsingGrangeDisplay;
    public Dictionary<string, Dictionary<Item, int[]>> festivalShops;
    private bool startSecretSantaAfterDialogue;
    public List<Item> grangeDisplay;
    public bool specialEventVariable2;
    public List<Item> luauIngredients;

    public Texture2D festivalTexture
    {
      get
      {
        if (this._festivalTexture == null)
          this._festivalTexture = this.festivalContent.Load<Texture2D>("Maps\\Festivals");
        return this._festivalTexture;
      }
    }

    public int CurrentCommand
    {
      get
      {
        return this.currentCommand;
      }
      set
      {
        this.currentCommand = value;
      }
    }

    public Event(string eventString, int eventID = -1)
    {
      this.id = eventID;
      this.eventCommands = eventString.Split('/');
      this.actorPositionsAfterMove = new Dictionary<string, Vector3>();
      this.previousAmbientLight = Game1.ambientLight;
      this.wasBloomDay = Game1.bloomDay;
      this.wasBloomVisible = Game1.bloom != null && Game1.bloom.Visible;
      if (this.wasBloomDay)
        this.previousBloomSettings = Game1.bloom.Settings;
      if (Game1.player.getMount() != null)
      {
        this.playerWasMounted = true;
        Game1.player.getMount().dismount();
      }
      Game1.player.canOnlyWalk = true;
      Game1.player.showNotCarrying();
      this.drawTool = false;
    }

    public Event()
    {
    }

    public bool tryToLoadFestival(string festival)
    {
      Game1.player.festivalScore = 0;
      foreach (Farmer farmer in Game1.otherFarmers.Values)
        farmer.festivalScore = 0;
      try
      {
        this.festivalData = this.festivalContent.Load<Dictionary<string, string>>("Data\\Festivals\\" + festival);
        this.festivalData.Add("file", festival);
      }
      catch (Exception ex)
      {
        return false;
      }
      string str = this.festivalData["conditions"].Split('/')[0];
      int int32_1 = Convert.ToInt32(this.festivalData["conditions"].Split('/')[1].Split(' ')[0]);
      int int32_2 = Convert.ToInt32(this.festivalData["conditions"].Split('/')[1].Split(' ')[1]);
      if (!str.Equals(Game1.currentLocation.Name) || Game1.timeOfDay < int32_1 || (Game1.timeOfDay >= int32_2 || Game1.currentLocation.getFarmersCount() + 1 < Game1.numberOfPlayers()))
        return false;
      this.eventCommands = this.festivalData["set-up"].Split('/');
      this.actorPositionsAfterMove = new Dictionary<string, Vector3>();
      this.previousAmbientLight = Game1.ambientLight;
      int num = this.wasBloomDay ? 1 : 0;
      this.isFestival = true;
      Game1.setRichPresence(nameof (festival), (object) festival);
      return true;
    }

    public void endBehaviors(string[] split, GameLocation location)
    {
      Game1.pixelZoom = this.oldPixelZoom;
      if (Game1.currentSong != null && !Game1.currentSong.Name.Contains(Game1.currentSeason) && !this.eventCommands[0].Equals("continue"))
        Game1.changeMusicTrack("none");
      if (split != null && split.Length > 1)
      {
        string s = split[1];
        // ISSUE: reference to a compiler-generated method
        uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(s);
        if (stringHash <= 1997527009U)
        {
          if (stringHash <= 1353598700U)
          {
            if ((int) stringHash != 878656485)
            {
              if ((int) stringHash != 1266391031)
              {
                if ((int) stringHash == 1353598700 && s == "bed")
                  Game1.player.position = Game1.player.mostRecentBed + new Vector2(0.0f, (float) Game1.tileSize);
              }
              else if (s == "wedding")
              {
                if (Game1.player.isMale)
                {
                  Game1.player.changeShirt(this.oldShirt);
                  Game1.player.changePants(this.oldPants);
                  Game1.getCharacterFromName("Lewis", false).CurrentDialogue.Push(new Dialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1025"), Game1.getCharacterFromName("Lewis", false)));
                }
                Game1.warpFarmer("Farm", Utility.getHomeOfFarmer(Game1.player).getPorchStandingSpot().X - 1, Utility.getHomeOfFarmer(Game1.player).getPorchStandingSpot().Y, 2);
              }
            }
            else if (s == "busIntro")
              Game1.currentMinigame = (IMinigame) new Intro(4);
          }
          else if ((int) stringHash != 1358361813)
          {
            if ((int) stringHash != 1619733218)
            {
              if ((int) stringHash == 1997527009 && s == "warpOut")
              {
                int index = 0;
                if (location is BathHousePool && Game1.player.isMale)
                  index = 1;
                Game1.warpFarmer(location.warps[index].TargetName, location.warps[index].TargetX, location.warps[index].TargetY, true);
                Game1.eventOver = true;
                this.CurrentCommand = this.CurrentCommand + 2;
                Game1.screenGlowHold = false;
              }
            }
            else if (s == "invisibleWarpOut")
            {
              Game1.getCharacterFromName(split[2], false).isInvisible = true;
              Game1.warpFarmer(location.warps[0].TargetName, location.warps[0].TargetX, location.warps[0].TargetY, true);
              Game1.fadeScreenToBlack();
              Game1.eventOver = true;
              this.CurrentCommand = this.CurrentCommand + 2;
              Game1.screenGlowHold = false;
            }
          }
          else if (s == "credits")
          {
            Game1.debrisWeather.Clear();
            Game1.isDebrisWeather = false;
            Game1.changeMusicTrack("wedding");
            Game1.gameMode = (byte) 10;
            this.CurrentCommand = this.CurrentCommand + 2;
          }
        }
        else if (stringHash <= 2519057040U)
        {
          if ((int) stringHash != -2136135913)
          {
            if ((int) stringHash != -1823519222)
            {
              if ((int) stringHash == -1775910256 && s == "invisible")
                Game1.getCharacterFromName(split[2], false).isInvisible = true;
            }
            else if (s == "position")
              Game1.player.positionBeforeEvent = new Vector2((float) Convert.ToInt32(split[2]), (float) Convert.ToInt32(split[3]));
          }
          else if (s == "dialogueWarpOut")
          {
            int index = 0;
            if (location is BathHousePool && Game1.player.isMale)
              index = 1;
            Game1.warpFarmer(location.warps[index].TargetName, location.warps[index].TargetX, location.warps[index].TargetY, true);
            NPC characterFromName = Game1.getCharacterFromName(split[2], false);
            int startIndex = this.eventCommands[this.CurrentCommand].IndexOf('"') + 1;
            int length = this.eventCommands[this.CurrentCommand].Substring(this.eventCommands[this.CurrentCommand].IndexOf('"') + 1).IndexOf('"');
            characterFromName.CurrentDialogue.Clear();
            characterFromName.CurrentDialogue.Push(new Dialogue(this.eventCommands[this.CurrentCommand].Substring(startIndex, length), characterFromName));
            Game1.eventOver = true;
            this.CurrentCommand = this.CurrentCommand + 2;
            Game1.screenGlowHold = false;
          }
        }
        else if (stringHash <= 2988976489U)
        {
          if ((int) stringHash != -1371854509)
          {
            if ((int) stringHash == -1305990807 && s == "newDay")
            {
              if (Game1.player.isRidingHorse())
                Game1.player.getMount().dismount();
              Game1.player.faceDirection(2);
              Game1.warpFarmer((GameLocation) Utility.getHomeOfFarmer(Game1.player), (int) Game1.player.mostRecentBed.X / Game1.tileSize, (int) Game1.player.mostRecentBed.Y / Game1.tileSize, 2, false);
              Game1.newDay = true;
              Game1.player.currentLocation.lastTouchActionLocation = new Vector2((float) ((int) Game1.player.mostRecentBed.X / Game1.tileSize), (float) ((int) Game1.player.mostRecentBed.Y / Game1.tileSize));
              Game1.player.completelyStopAnimatingOrDoingAction();
              if (Game1.player.bathingClothes)
                Game1.player.changeOutOfSwimSuit();
              Game1.player.swimming = false;
              Game1.player.CanMove = false;
              Game1.changeMusicTrack("none");
            }
          }
          else if (s == "dialogue")
          {
            NPC characterFromName = Game1.getCharacterFromName(split[2], false);
            int startIndex = this.eventCommands[this.CurrentCommand].IndexOf('"') + 1;
            int length = this.eventCommands[this.CurrentCommand].Substring(this.eventCommands[this.CurrentCommand].IndexOf('"') + 1).IndexOf('"');
            if (characterFromName != null)
            {
              characterFromName.CurrentDialogue.Clear();
              characterFromName.CurrentDialogue.Push(new Dialogue(this.eventCommands[this.CurrentCommand].Substring(startIndex, length), characterFromName));
            }
          }
        }
        else if ((int) stringHash != -898604268)
        {
          if ((int) stringHash == -38085231 && s == "Maru1")
          {
            Game1.getCharacterFromName("Demetrius", false).setNewDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1018"), false, false);
            Game1.getCharacterFromName("Maru", false).setNewDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1020"), false, false);
            Game1.warpFarmer(location.warps[0].TargetName, location.warps[0].TargetX, location.warps[0].TargetY, true);
            Game1.fadeScreenToBlack();
            Game1.eventOver = true;
            this.CurrentCommand = this.CurrentCommand + 2;
          }
        }
        else if (s == "beginGame")
        {
          Game1.gameMode = (byte) 3;
          if (Game1.IsServer)
            Game1.initializeMultiplayerServer();
          if (Game1.IsClient)
            Game1.initializeMultiplayerClient();
          Game1.warpFarmer("FarmHouse", 9, 9, false);
          Game1.NewDay(1000f);
        }
      }
      this.exitEvent();
    }

    public void exitEvent()
    {
      if (this.id != -1 && !Game1.player.eventsSeen.Contains(this.id))
        Game1.player.eventsSeen.Add(this.id);
      Game1.player.canOnlyWalk = false;
      Game1.nonWarpFade = true;
      if (!Game1.fadeIn || (double) Game1.fadeToBlackAlpha >= 1.0)
        Game1.fadeScreenToBlack();
      Game1.eventOver = true;
      Game1.fadeToBlack = true;
      this.CurrentCommand = this.CurrentCommand + 2;
      Game1.screenGlowHold = false;
      if (this.isFestival)
      {
        Game1.timeOfDay = 2200;
        string str = this.festivalData["file"];
        int minutes = 780;
        if (this.festivalData != null && (this.festivalData["file"].Equals("summer28") || this.festivalData["file"].Equals("fall27")))
        {
          Game1.timeOfDay = 2400;
          minutes = 240;
        }
        Game1.warpFarmer((GameLocation) Game1.getFarm(), 64 - Utility.getFarmerNumberFromFarmer(Game1.player), 15, 2, false);
        Game1.player.toolOverrideFunction = (AnimatedSprite.endOfAnimationBehavior) null;
        this.isFestival = false;
        if (Game1.player.getSpouse() != null)
          Game1.warpCharacter(Game1.player.getSpouse(), "FarmHouse", Utility.getHomeOfFarmer(Game1.player).getSpouseBedSpot(), false, true);
        Game1.currentLocation.currentEvent = (Event) null;
        foreach (GameLocation location in Game1.locations)
        {
          location.currentEvent = (Event) null;
          foreach (Object @object in location.objects.Values)
            @object.minutesElapsed(minutes, location);
        }
        Game1.player.freezePause = 1500;
      }
      else
      {
        if (this.playerWasMounted && Game1.currentLocation.isOutdoors)
        {
          Horse horse = Utility.findHorse();
          if (horse != null)
            Game1.warpCharacter((NPC) horse, Game1.currentLocation.name, new Vector2((float) Game1.xLocationAfterWarp, (float) Game1.yLocationAfterWarp), false, true);
        }
        Game1.player.forceCanMove();
      }
    }

    public void incrementCommandAfterFade()
    {
      this.CurrentCommand = this.CurrentCommand + 1;
      Game1.globalFade = false;
    }

    public void cleanup()
    {
      Game1.ambientLight = this.previousAmbientLight;
      if (Game1.bloom != null)
      {
        Game1.bloom.Settings = this.previousBloomSettings;
        Game1.bloom.Visible = this.wasBloomVisible;
        Game1.bloom.reload();
      }
      foreach (NPC withUniquePortrait in this.npcsWithUniquePortraits)
      {
        withUniquePortrait.Portrait = Game1.content.Load<Texture2D>("Portraits\\" + withUniquePortrait.name);
        withUniquePortrait.uniquePortraitActive = false;
      }
      if (this._festivalTexture != null)
        this._festivalTexture = (Texture2D) null;
      this.festivalContent.Unload();
    }

    public void checkForNextCommand(GameLocation location, GameTime time)
    {
      if (this.skipped)
        return;
      foreach (NPC actor in this.actors)
      {
        actor.update(time, Game1.currentLocation);
        if (actor.Sprite.currentAnimation != null)
          actor.Sprite.animateOnce(time);
      }
      if (this.aboveMapSprites != null)
      {
        for (int index = this.aboveMapSprites.Count - 1; index >= 0; --index)
        {
          if (this.aboveMapSprites[index].update(time))
            this.aboveMapSprites.RemoveAt(index);
        }
      }
      if (!this.playerControlSequence)
        Game1.player.setRunning(false, false);
      if (this.npcControllers != null)
      {
        for (int index = this.npcControllers.Count - 1; index >= 0; --index)
        {
          if (this.npcControllers[index].update(time, location, this.npcControllers))
            this.npcControllers.RemoveAt(index);
        }
      }
      if (this.isFestival)
        this.festivalUpdate(time);
      string[] split = this.eventCommands[Math.Min(this.eventCommands.Length - 1, this.CurrentCommand)].Split(' ');
      if (this.temporaryLocation != null && !Game1.currentLocation.Equals((object) this.temporaryLocation))
        this.temporaryLocation.updateEvenIfFarmerIsntHere(time, true);
      TimeSpan elapsedGameTime;
      if (this.CurrentCommand == 0 && !this.forked && !this.eventSwitched)
      {
        Game1.player.speed = 2;
        Game1.player.running = false;
        Game1.eventOver = false;
        if ((!this.eventCommands[0].Equals("none") || !Game1.isRaining) && (!this.eventCommands[0].Equals("continue") && !this.eventCommands[0].Contains("pause")))
          Game1.changeMusicTrack(this.eventCommands[0]);
        if (location is Farm)
        {
          Point positionForFarmer = Farm.getFrontDoorPositionForFarmer(Game1.player);
          // ISSUE: explicit reference operation
          // ISSUE: variable of a reference type
          xTile.Dimensions.Rectangle& local1 = @Game1.viewport;
          Viewport viewport;
          int num1;
          if (!Game1.currentLocation.IsOutdoors)
          {
            num1 = positionForFarmer.X - Game1.graphics.GraphicsDevice.Viewport.Width / 2;
          }
          else
          {
            int val1_1 = 0;
            int x = positionForFarmer.X;
            viewport = Game1.graphics.GraphicsDevice.Viewport;
            int num2 = viewport.Width / 2;
            int val1_2 = x - num2;
            int displayWidth = Game1.currentLocation.Map.DisplayWidth;
            viewport = Game1.graphics.GraphicsDevice.Viewport;
            int width = viewport.Width;
            int val2_1 = displayWidth - width;
            int val2_2 = Math.Min(val1_2, val2_1);
            num1 = Math.Max(val1_1, val2_2);
          }
          // ISSUE: explicit reference operation
          (^local1).X = num1;
          // ISSUE: explicit reference operation
          // ISSUE: variable of a reference type
          xTile.Dimensions.Rectangle& local2 = @Game1.viewport;
          int num3;
          if (!Game1.currentLocation.IsOutdoors)
          {
            int y = positionForFarmer.Y;
            viewport = Game1.graphics.GraphicsDevice.Viewport;
            int num2 = viewport.Height / 2;
            num3 = y - num2;
          }
          else
          {
            int val1_1 = 0;
            int y = positionForFarmer.Y;
            viewport = Game1.graphics.GraphicsDevice.Viewport;
            int num2 = viewport.Height / 2;
            int val1_2 = y - num2;
            int displayHeight = Game1.currentLocation.Map.DisplayHeight;
            viewport = Game1.graphics.GraphicsDevice.Viewport;
            int height = viewport.Height;
            int val2_1 = displayHeight - height;
            int val2_2 = Math.Min(val1_2, val2_1);
            num3 = Math.Max(val1_1, val2_2);
          }
          // ISSUE: explicit reference operation
          (^local2).Y = num3;
        }
        else if (!this.eventCommands[1].Equals("follow"))
        {
          try
          {
            string[] strArray = this.eventCommands[1].Split(' ');
            Game1.viewportFreeze = true;
            int index1 = 0;
            int num1 = Convert.ToInt32(strArray[index1]) * Game1.tileSize + Game1.tileSize / 2;
            int index2 = 1;
            int num2 = Convert.ToInt32(strArray[index2]) * Game1.tileSize + Game1.tileSize / 2;
            int index3 = 0;
            if ((int) strArray[index3][0] == 45)
            {
              Game1.viewport.X = num1;
              Game1.viewport.Y = num2;
            }
            else
            {
              Game1.viewport.X = Game1.currentLocation.IsOutdoors ? Math.Max(0, Math.Min(num1 - Game1.viewport.Width / 2, Game1.currentLocation.Map.DisplayWidth - Game1.viewport.Width)) : num1 - Game1.viewport.Width / 2;
              Game1.viewport.Y = Game1.currentLocation.IsOutdoors ? Math.Max(0, Math.Min(num2 - Game1.viewport.Height / 2, Game1.currentLocation.Map.DisplayHeight - Game1.viewport.Height)) : num2 - Game1.viewport.Height / 2;
            }
            if (num1 > 0 && Game1.graphics.GraphicsDevice.Viewport.Width > Game1.currentLocation.Map.DisplayWidth)
              Game1.viewport.X = (Game1.currentLocation.Map.DisplayWidth - Game1.viewport.Width) / 2;
            if (num2 > 0)
            {
              if (Game1.graphics.GraphicsDevice.Viewport.Height > Game1.currentLocation.Map.DisplayHeight)
                Game1.viewport.Y = (Game1.currentLocation.Map.DisplayHeight - Game1.viewport.Height) / 2;
            }
          }
          catch (Exception ex)
          {
            this.forked = true;
            return;
          }
        }
        this.setUpCharacters(this.eventCommands[2], location);
        this.populateWalkLocationsList();
        this.CurrentCommand = 3;
        foreach (NPC actor in this.actors)
          ;
      }
      else if (!Game1.fadeToBlack || this.actorPositionsAfterMove.Count > 0 || (this.CurrentCommand > 3 || this.forked))
      {
        if (this.eventCommands.Length <= this.CurrentCommand)
          return;
        Vector3 viewportTarget = this.viewportTarget;
        if (!this.viewportTarget.Equals(Vector3.Zero))
        {
          int speed = Game1.player.speed;
          Game1.player.speed = (int) this.viewportTarget.X;
          Game1.viewport.X += (int) this.viewportTarget.X;
          if ((double) this.viewportTarget.X != 0.0)
            Game1.updateRainDropPositionForPlayerMovement((double) this.viewportTarget.X < 0.0 ? 3 : 1, true, Math.Abs(this.viewportTarget.X + (!Game1.player.isMoving() || Game1.player.facingDirection != 3 ? (!Game1.player.isMoving() || Game1.player.facingDirection != 1 ? 0.0f : (float) Game1.player.speed) : (float) -Game1.player.speed)));
          Game1.viewport.Y += (int) this.viewportTarget.Y;
          Game1.player.speed = (int) this.viewportTarget.Y;
          if ((double) this.viewportTarget.Y != 0.0)
            Game1.updateRainDropPositionForPlayerMovement((double) this.viewportTarget.Y < 0.0 ? 0 : 2, true, Math.Abs(this.viewportTarget.Y - (!Game1.player.isMoving() || Game1.player.facingDirection != 0 ? (!Game1.player.isMoving() || Game1.player.facingDirection != 2 ? 0.0f : (float) Game1.player.speed) : (float) -Game1.player.speed)));
          Game1.player.speed = speed;
          // ISSUE: explicit reference operation
          // ISSUE: variable of a reference type
          float& local = @this.viewportTarget.Z;
          // ISSUE: explicit reference operation
          double num1 = (double) ^local;
          elapsedGameTime = time.ElapsedGameTime;
          double milliseconds = (double) elapsedGameTime.Milliseconds;
          double num2 = num1 - milliseconds;
          // ISSUE: explicit reference operation
          ^local = (float) num2;
          if ((double) this.viewportTarget.Z <= 0.0)
            this.viewportTarget = Vector3.Zero;
        }
        if (this.actorPositionsAfterMove.Count > 0)
        {
          foreach (string index in this.actorPositionsAfterMove.Keys.ToArray<string>())
          {
            Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle((int) this.actorPositionsAfterMove[index].X * Game1.tileSize, (int) this.actorPositionsAfterMove[index].Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
            rectangle.Inflate(-Game1.pixelZoom, 0);
            if (this.getActorByName(index) != null && this.getActorByName(index).GetBoundingBox().Width > Game1.tileSize)
            {
              rectangle.Width = this.getActorByName(index).GetBoundingBox().Width + Game1.pixelZoom;
              rectangle.Height = this.getActorByName(index).GetBoundingBox().Height + Game1.pixelZoom;
            }
            if (index.Contains("farmer"))
            {
              Farmer farmerNumberString = Utility.getFarmerFromFarmerNumberString(index);
              if (farmerNumberString != null && rectangle.Contains(farmerNumberString.GetBoundingBox()) && ((double) (farmerNumberString.GetBoundingBox().Y - rectangle.Top) <= (double) (Game1.tileSize / 4) + (double) farmerNumberString.getMovementSpeed() && farmerNumberString.FacingDirection != 2 || (double) (rectangle.Bottom - farmerNumberString.GetBoundingBox().Bottom) <= (double) (Game1.tileSize / 4) + (double) farmerNumberString.getMovementSpeed() && farmerNumberString.FacingDirection == 2))
              {
                farmerNumberString.showNotCarrying();
                farmerNumberString.Halt();
                farmerNumberString.faceDirection((int) this.actorPositionsAfterMove[index].Z);
                farmerNumberString.FarmerSprite.StopAnimation();
                farmerNumberString.Halt();
                this.actorPositionsAfterMove.Remove(index);
              }
              else if (farmerNumberString != null)
              {
                farmerNumberString.canOnlyWalk = false;
                farmerNumberString.setRunning(false, true);
                farmerNumberString.canOnlyWalk = true;
                farmerNumberString.lastPosition = Game1.player.position;
                farmerNumberString.MovePosition(time, Game1.viewport, location);
              }
            }
            else
            {
              foreach (NPC actor in this.actors)
              {
                Microsoft.Xna.Framework.Rectangle boundingBox = actor.GetBoundingBox();
                if (actor.name.Equals(index) && rectangle.Contains(boundingBox) && actor.GetBoundingBox().Y - rectangle.Top <= Game1.tileSize / 4)
                {
                  actor.Halt();
                  actor.faceDirection((int) this.actorPositionsAfterMove[index].Z);
                  this.actorPositionsAfterMove.Remove(index);
                  break;
                }
                if (actor.name.Equals(index))
                {
                  actor.MovePosition(time, Game1.viewport, (GameLocation) null);
                  break;
                }
              }
            }
          }
          if (this.actorPositionsAfterMove.Count == 0)
          {
            if (this.continueAfterMove)
              this.continueAfterMove = false;
            else
              this.CurrentCommand = this.CurrentCommand + 1;
          }
          if (!this.continueAfterMove)
            return;
        }
        if (split[0].Equals("move"))
        {
          int index = 1;
          while (index < split.Length && split.Length - index >= 3)
          {
            if (split[index].Contains("farmer") && !this.actorPositionsAfterMove.ContainsKey(split[index]))
            {
              Farmer farmerNumberString = Utility.getFarmerFromFarmerNumberString(split[index]);
              if (farmerNumberString != null)
              {
                farmerNumberString.canOnlyWalk = false;
                farmerNumberString.setRunning(false, true);
                farmerNumberString.canOnlyWalk = true;
                farmerNumberString.convertEventMotionCommandToMovement(new Vector2((float) Convert.ToInt32(split[index + 1]), (float) Convert.ToInt32(split[index + 2])));
                this.actorPositionsAfterMove.Add(split[index], this.getPositionAfterMove((Character) Game1.player, Convert.ToInt32(split[index + 1]), Convert.ToInt32(split[index + 2]), Convert.ToInt32(split[index + 3])));
              }
            }
            else
            {
              NPC actorByName = this.getActorByName(split[index]);
              string key = split[index].Equals("rival") ? Utility.getOtherFarmerNames()[0] : split[index];
              if (!this.actorPositionsAfterMove.ContainsKey(key))
              {
                actorByName.convertEventMotionCommandToMovement(new Vector2((float) Convert.ToInt32(split[index + 1]), (float) Convert.ToInt32(split[index + 2])));
                this.actorPositionsAfterMove.Add(key, this.getPositionAfterMove((Character) actorByName, Convert.ToInt32(split[index + 1]), Convert.ToInt32(split[index + 2]), Convert.ToInt32(split[index + 3])));
              }
            }
            index += 4;
          }
          if (((IEnumerable<string>) split).Last<string>().Equals("true"))
          {
            this.continueAfterMove = true;
            this.CurrentCommand = this.CurrentCommand + 1;
          }
          else if (((IEnumerable<string>) split).Last<string>().Equals("false"))
          {
            this.continueAfterMove = false;
            if (split.Length == 2 && this.actorPositionsAfterMove.Count == 0)
              this.CurrentCommand = this.CurrentCommand + 1;
          }
        }
        else if (split[0].Equals("speak"))
        {
          if (this.skipped)
            return;
          if (!Game1.dialogueUp)
          {
            double timeAccumulator = (double) this.timeAccumulator;
            elapsedGameTime = time.ElapsedGameTime;
            double milliseconds = (double) elapsedGameTime.Milliseconds;
            this.timeAccumulator = (float) (timeAccumulator + milliseconds);
            if ((double) this.timeAccumulator < 500.0)
              return;
            this.timeAccumulator = 0.0f;
            NPC npc = Game1.getCharacterFromName(split[1].Equals("rival") ? Utility.getOtherFarmerNames()[0] : split[1], false) ?? this.getActorByName(split[1]);
            if (npc == null)
            {
              Game1.eventFinished();
              return;
            }
            int num = this.eventCommands[this.currentCommand].IndexOf('"');
            if (num > 0)
            {
              int length = this.eventCommands[this.CurrentCommand].Substring(num + 1).IndexOf('"');
              Game1.player.checkForQuestComplete(npc, -1, -1, (Item) null, (string) null, 5, -1);
              if (Game1.NPCGiftTastes.ContainsKey(split[1]) && !Game1.player.friendships.ContainsKey(split[1]))
                Game1.player.friendships.Add(split[1], new int[6]);
              if (length > 0)
                npc.CurrentDialogue.Push(new Dialogue(this.eventCommands[this.CurrentCommand].Substring(num + 1, length), npc));
              else
                npc.CurrentDialogue.Push(new Dialogue("...", npc));
            }
            else
              npc.CurrentDialogue.Push(new Dialogue(Game1.content.LoadString(split[2]), npc));
            Game1.drawDialogue(npc);
          }
        }
        else if (split[0].Equals("minedeath"))
        {
          if (!Game1.dialogueUp)
          {
            Random random = new Random((int) Game1.uniqueIDForThisGame / 2 + (int) Game1.stats.DaysPlayed + Game1.timeOfDay);
            int num1 = Math.Min(random.Next(Game1.player.Money / 20, Game1.player.Money / 4), 5000);
            int num2 = num1 - (int) ((double) Game1.player.LuckLevel * 0.01 * (double) num1);
            int num3 = num2 - num2 % 100;
            int num4 = 0;
            double num5 = 0.25 - (double) Game1.player.LuckLevel * 0.05 - Game1.dailyLuck;
            for (int index = Game1.player.Items.Count - 1; index >= 0; --index)
            {
              if (Game1.player.Items[index] != null && (!(Game1.player.Items[index] is Tool) || Game1.player.Items[index] is MeleeWeapon && (Game1.player.Items[index] as MeleeWeapon).initialParentTileIndex != 47 && (Game1.player.Items[index] as MeleeWeapon).initialParentTileIndex != 4) && (Game1.player.Items[index].canBeTrashed() && !(Game1.player.Items[index] is Ring) && random.NextDouble() < num5))
              {
                ++num4;
                Game1.player.Items[index] = (Item) null;
              }
            }
            Game1.player.Stamina = Math.Min(Game1.player.Stamina, 2f);
            int num6 = (int) ((double) (10 - Game1.player.LuckLevel / 3) - Game1.dailyLuck * 20.0);
            Game1.player.deepestMineLevel = Math.Max(1, Game1.mine.lowestLevelReached - num6);
            if (Game1.mine != null)
              Game1.mine.lowestLevelReached = Math.Max(1, Game1.mine.lowestLevelReached - num6);
            Game1.player.Money = Math.Max(0, Game1.player.money - num3);
            string str1;
            if (num6 <= 0)
              str1 = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1057");
            else
              str1 = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1055", (object) num6);
            string str2;
            if (num3 > 0)
              str2 = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1058", (object) num3);
            else
              str2 = "";
            string str3;
            if (num4 <= 0)
              str3 = num3 <= 0 ? "" : ".";
            else if (num3 > 0)
            {
              string str4 = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1063");
              string str5;
              if (num4 != 1)
                str5 = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1062", (object) num4);
              else
                str5 = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1061");
              str3 = str4 + str5;
            }
            else
            {
              string str4 = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1060");
              string str5;
              if (num4 != 1)
                str5 = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1062", (object) num4);
              else
                str5 = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1061");
              str3 = str4 + str5;
            }
            Game1.drawObjectDialogue(str1 + str2 + str3);
          }
        }
        else if (split[0].Equals("hospitaldeath"))
        {
          if (!Game1.dialogueUp)
          {
            Random random = new Random((int) Game1.uniqueIDForThisGame / 2 + (int) Game1.stats.DaysPlayed + Game1.timeOfDay);
            int num1 = 0;
            double num2 = 0.25 - (double) Game1.player.LuckLevel * 0.05 - Game1.dailyLuck;
            for (int index = Game1.player.Items.Count - 1; index >= 0; --index)
            {
              if (Game1.player.Items[index] != null && (!(Game1.player.Items[index] is Tool) || Game1.player.Items[index] is MeleeWeapon && (Game1.player.Items[index] as MeleeWeapon).initialParentTileIndex != 47 && (Game1.player.Items[index] as MeleeWeapon).initialParentTileIndex != 4) && (Game1.player.Items[index].canBeTrashed() && !(Game1.player.Items[index] is Ring) && random.NextDouble() < num2))
              {
                ++num1;
                Game1.player.Items[index] = (Item) null;
              }
            }
            Game1.player.Stamina = Math.Min(Game1.player.Stamina, 2f);
            int num3 = Math.Min(1000, Game1.player.money);
            Game1.player.Money -= num3;
            string str1;
            if (num3 <= 0)
              str1 = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1070");
            else
              str1 = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1068", (object) num3);
            string str2;
            if (num1 <= 0)
            {
              str2 = "";
            }
            else
            {
              string str3 = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1071");
              string str4;
              if (num1 != 1)
                str4 = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1062", (object) num1);
              else
                str4 = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1061");
              str2 = str3 + str4;
            }
            Game1.drawObjectDialogue(str1 + str2);
          }
        }
        else if (split[0].Equals("end"))
          this.endBehaviors(split, location);
        else if (split[0].Equals("skippable"))
        {
          this.skippable = true;
          this.CurrentCommand = this.CurrentCommand + 1;
        }
        else if (split[0].Equals("emote"))
        {
          bool flag = split.Length > 3;
          if (split[1].Contains("farmer"))
          {
            if (Utility.getFarmerFromFarmerNumberString(split[1]) != null)
              Game1.player.doEmote(Convert.ToInt32(split[2]), !flag);
          }
          else
          {
            NPC actorByName = this.getActorByName(split[1]);
            if (!actorByName.isEmoting)
              actorByName.doEmote(Convert.ToInt32(split[2]), !flag);
          }
          if (flag)
          {
            this.CurrentCommand = this.CurrentCommand + 1;
            this.checkForNextCommand(location, time);
          }
        }
        else if (split[0].Equals("stopMusic"))
        {
          Game1.changeMusicTrack("none");
          this.CurrentCommand = this.CurrentCommand + 1;
        }
        else if (split[0].Equals("playSound"))
        {
          Game1.playSound(split[1]);
          this.CurrentCommand = this.CurrentCommand + 1;
          this.checkForNextCommand(location, time);
        }
        else if (split[0].Equals("pause"))
        {
          if ((double) Game1.pauseTime <= 0.0)
            Game1.pauseTime = (float) Convert.ToInt32(split[1]);
        }
        else if (split[0].Equals("resetVariable"))
        {
          this.specialEventVariable1 = false;
          this.currentCommand = this.currentCommand + 1;
        }
        else if (split[0].Equals("faceDirection"))
        {
          if (split[1].Contains("farmer"))
          {
            Farmer farmerNumberString = Utility.getFarmerFromFarmerNumberString(split[1]);
            if (farmerNumberString != null)
            {
              farmerNumberString.FarmerSprite.StopAnimation();
              farmerNumberString.completelyStopAnimatingOrDoingAction();
              farmerNumberString.faceDirection(Convert.ToInt32(split[2]));
              farmerNumberString.FarmerSprite.StopAnimation();
            }
          }
          else if (split[1].Contains("spouse"))
          {
            if (Game1.player.spouse != null && Game1.player.spouse.Length > 0 && this.getActorByName(Game1.player.spouse.Replace("engaged", "")) != null)
              this.getActorByName(Game1.player.spouse.Replace("engaged", "")).faceDirection(Convert.ToInt32(split[2]));
          }
          else
            this.getActorByName(split[1]).faceDirection(Convert.ToInt32(split[2]));
          if (split.Length == 3 && (double) Game1.pauseTime <= 0.0)
            Game1.pauseTime = 500f;
          else if (split.Length > 3)
          {
            this.CurrentCommand = this.CurrentCommand + 1;
            this.checkForNextCommand(location, time);
          }
        }
        else if (split[0].Equals("warp"))
        {
          if (split[1].Contains("farmer"))
          {
            Farmer farmerNumberString = Utility.getFarmerFromFarmerNumberString(split[1]);
            if (farmerNumberString != null)
            {
              farmerNumberString.position.X = (float) (Convert.ToInt32(split[2]) * Game1.tileSize);
              farmerNumberString.position.Y = (float) (Convert.ToInt32(split[3]) * Game1.tileSize);
              if (Game1.IsClient)
                farmerNumberString.remotePosition = new Vector2(farmerNumberString.position.X, farmerNumberString.position.Y);
            }
          }
          else if (split[1].Contains("spouse"))
          {
            if (Game1.player.spouse != null && Game1.player.spouse.Length > 0 && this.getActorByName(Game1.player.spouse.Replace("engaged", "")) != null)
            {
              for (int index = this.npcControllers.Count - 1; index >= 0; --index)
              {
                if (this.npcControllers[index].puppet.name.Equals(Game1.player.spouse.Replace("engaged", "")))
                  this.npcControllers.RemoveAt(index);
              }
              this.getActorByName(Game1.player.spouse.Replace("engaged", "")).position = new Vector2((float) (Convert.ToInt32(split[2]) * Game1.tileSize), (float) (Convert.ToInt32(split[3]) * Game1.tileSize));
            }
          }
          else
          {
            NPC actorByName = this.getActorByName(split[1]);
            if (actorByName != null)
            {
              actorByName.position.X = (float) (Convert.ToInt32(split[2]) * Game1.tileSize + Game1.pixelZoom);
              actorByName.position.Y = (float) (Convert.ToInt32(split[3]) * Game1.tileSize);
            }
          }
          this.CurrentCommand = this.CurrentCommand + 1;
          if (split.Length > 4)
            this.checkForNextCommand(location, time);
        }
        else if (split[0].Equals("speed"))
        {
          if (split[1].Equals("farmer"))
            this.farmerAddedSpeed = Convert.ToInt32(split[2]);
          else
            this.getActorByName(split[1]).speed = Convert.ToInt32(split[2]);
          this.CurrentCommand = this.CurrentCommand + 1;
        }
        else if (split[0].Equals("stopAdvancedMoves"))
        {
          this.npcControllers.Clear();
          this.CurrentCommand = this.CurrentCommand + 1;
        }
        else if (split[0].Equals("doAction"))
        {
          location.checkAction(new Location(Convert.ToInt32(split[1]), Convert.ToInt32(split[2])), Game1.viewport, Game1.player);
          this.CurrentCommand = this.CurrentCommand + 1;
        }
        else if (split[0].Equals("removeTile"))
        {
          location.removeTile(Convert.ToInt32(split[1]), Convert.ToInt32(split[2]), split[3]);
          this.CurrentCommand = this.CurrentCommand + 1;
        }
        else if (split[0].Equals("textAboveHead"))
        {
          NPC actorByName = this.getActorByName(split[1]);
          if (actorByName != null)
          {
            int startIndex = this.eventCommands[this.CurrentCommand].IndexOf('"') + 1;
            int length = this.eventCommands[this.CurrentCommand].Substring(this.eventCommands[this.CurrentCommand].IndexOf('"') + 1).IndexOf('"');
            actorByName.showTextAboveHead(this.eventCommands[this.CurrentCommand].Substring(startIndex, length), -1, 2, 3000, 0);
          }
          this.CurrentCommand = this.CurrentCommand + 1;
        }
        else if (split[0].Equals("showFrame"))
        {
          if (split.Length > 2 && !split[2].Equals("flip") && !split[1].Contains("farmer"))
          {
            NPC actorByName = this.getActorByName(split[1]);
            if (actorByName != null)
            {
              actorByName.sprite.CurrentFrame = Convert.ToInt32(split[2]);
              if (split[1].Equals("spouse") && actorByName.gender == 0 && (actorByName.sprite.CurrentFrame >= 36 && actorByName.sprite.CurrentFrame <= 38))
                actorByName.sprite.CurrentFrame += 12;
            }
          }
          else
          {
            Farmer farmer = Utility.getFarmerFromFarmerNumberString(split[1]);
            if (split.Length == 2)
              farmer = Game1.player;
            if (farmer != null)
            {
              if (split.Length > 2)
                split[1] = split[2];
              farmer.FarmerSprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
              {
                new FarmerSprite.AnimationFrame(Convert.ToInt32(split[1]), 100, false, split.Length > 2, (AnimatedSprite.endOfAnimationBehavior) null, false)
              }.ToArray());
              farmer.FarmerSprite.loopThisAnimation = true;
              farmer.FarmerSprite.PauseForSingleAnimation = true;
              farmer.sprite.CurrentFrame = Convert.ToInt32(split[1]);
            }
          }
          this.CurrentCommand = this.CurrentCommand + 1;
          this.checkForNextCommand(location, time);
        }
        else if (split[0].Equals("farmerAnimation"))
        {
          Game1.player.FarmerSprite.setCurrentSingleAnimation(Convert.ToInt32(split[1]));
          this.CurrentCommand = this.CurrentCommand + 1;
        }
        else if (split[0].Equals("animate"))
        {
          int int32 = Convert.ToInt32(split[4]);
          bool flip = split[2].Equals("true");
          bool flag = split[3].Equals("true");
          List<FarmerSprite.AnimationFrame> animation = new List<FarmerSprite.AnimationFrame>();
          for (int index = 5; index < split.Length; ++index)
            animation.Add(new FarmerSprite.AnimationFrame(Convert.ToInt32(split[index]), int32, false, flip, (AnimatedSprite.endOfAnimationBehavior) null, false));
          if (split[1].Contains("farmer"))
          {
            Farmer farmerNumberString = Utility.getFarmerFromFarmerNumberString(split[1]);
            if (farmerNumberString != null)
            {
              farmerNumberString.FarmerSprite.setCurrentAnimation(animation.ToArray());
              farmerNumberString.FarmerSprite.loopThisAnimation = flag;
              farmerNumberString.FarmerSprite.PauseForSingleAnimation = true;
            }
          }
          else
          {
            NPC actorByName = this.getActorByName(split[1].Replace('_', ' '));
            if (actorByName != null)
            {
              actorByName.Sprite.setCurrentAnimation(animation);
              actorByName.Sprite.loop = flag;
            }
          }
          this.CurrentCommand = this.CurrentCommand + 1;
          this.checkForNextCommand(location, time);
        }
        else if (split[0].Equals("stopAnimation"))
        {
          if (split[1].Contains("farmer"))
          {
            Farmer farmerNumberString = Utility.getFarmerFromFarmerNumberString(split[1]);
            if (farmerNumberString != null)
            {
              farmerNumberString.completelyStopAnimatingOrDoingAction();
              farmerNumberString.Halt();
              farmerNumberString.FarmerSprite.currentAnimation = (List<FarmerSprite.AnimationFrame>) null;
              switch (farmerNumberString.facingDirection)
              {
                case 0:
                  farmerNumberString.FarmerSprite.setCurrentSingleFrame(12, (short) 32000, false, false);
                  break;
                case 1:
                  farmerNumberString.FarmerSprite.setCurrentSingleFrame(6, (short) 32000, false, false);
                  break;
                case 2:
                  farmerNumberString.FarmerSprite.setCurrentSingleFrame(0, (short) 32000, false, false);
                  break;
                case 3:
                  farmerNumberString.FarmerSprite.setCurrentSingleFrame(6, (short) 32000, false, true);
                  break;
              }
            }
          }
          else
          {
            NPC actorByName = this.getActorByName(split[1]);
            if (actorByName != null)
            {
              actorByName.Sprite.StopAnimation();
              if (split.Length > 2)
                actorByName.Sprite.CurrentFrame = Convert.ToInt32(split[2]);
            }
          }
          this.CurrentCommand = this.CurrentCommand + 1;
          this.checkForNextCommand(location, time);
        }
        else if (split[0].Equals("showRivalFrame"))
        {
          this.getActorByName("rival").sprite.CurrentFrame = Convert.ToInt32(split[1]);
          this.CurrentCommand = this.CurrentCommand + 1;
        }
        else if (split[0].Equals("weddingSprite"))
        {
          this.getActorByName("WeddingOutfits").sprite.CurrentFrame = Convert.ToInt32(split[1]);
          this.CurrentCommand = this.CurrentCommand + 1;
        }
        else if (split[0].Equals("changeLocation"))
        {
          Event currentEvent = Game1.currentLocation.currentEvent;
          Game1.currentLocation.currentEvent = (Event) null;
          Game1.currentLocation.cleanupBeforePlayerExit();
          Game1.currentLocation = Game1.getLocationFromName(split[1]);
          Game1.currentLocation.currentEvent = currentEvent;
          Game1.player.currentLocation = Game1.currentLocation;
          Game1.currentLocation.resetForPlayerEntry();
          Game1.currentLocation.Map.LoadTileSheets(Game1.mapDisplayDevice);
          this.temporaryLocation = (GameLocation) null;
          this.CurrentCommand = this.CurrentCommand + 1;
        }
        else if (split[0].Equals("halt"))
        {
          foreach (Character actor in this.actors)
            actor.Halt();
          Game1.player.Halt();
          foreach (Character character in Game1.otherFarmers.Values)
            character.Halt();
          this.CurrentCommand = this.CurrentCommand + 1;
          this.continueAfterMove = false;
          this.actorPositionsAfterMove.Clear();
        }
        else if (split[0].Equals("message"))
        {
          if (!Game1.dialogueUp && Game1.activeClickableMenu == null)
          {
            int startIndex = this.eventCommands[this.CurrentCommand].IndexOf('"') + 1;
            int num = this.eventCommands[this.CurrentCommand].LastIndexOf('"');
            if (num > 0 && num > startIndex)
              Game1.drawDialogueNoTyping(Game1.parseText(this.eventCommands[this.CurrentCommand].Substring(startIndex, num - startIndex)));
            else
              Game1.drawDialogueNoTyping("...");
          }
        }
        else if (split[0].Equals("addCookingRecipe"))
        {
          Game1.player.cookingRecipes.Add(this.eventCommands[this.CurrentCommand].Substring(this.eventCommands[this.CurrentCommand].IndexOf(' ') + 1), 0);
          this.CurrentCommand = this.CurrentCommand + 1;
        }
        else if (split[0].Equals("itemAboveHead"))
        {
          if (split.Length > 1 && split[1].Equals("pan"))
            Game1.player.holdUpItemThenMessage((Item) new Pan(), true);
          else if (split.Length > 1 && split[1].Equals("hero"))
            Game1.player.holdUpItemThenMessage((Item) new Object(Vector2.Zero, 116, false), true);
          else if (split.Length > 1 && split[1].Equals("sculpture"))
            Game1.player.holdUpItemThenMessage((Item) new Furniture(1306, Vector2.Zero), true);
          else if (split.Length > 1 && split[1].Equals("joja"))
            Game1.player.holdUpItemThenMessage((Item) new Object(Vector2.Zero, 117, false), true);
          else if (split.Length > 1 && split[1].Equals("slimeEgg"))
            Game1.player.holdUpItemThenMessage((Item) new Object(680, 1, false, -1, 0), true);
          else if (split.Length > 1 && split[1].Equals("rod"))
            Game1.player.holdUpItemThenMessage((Item) new FishingRod(), true);
          else if (split.Length > 1 && split[1].Equals("sword"))
            Game1.player.holdUpItemThenMessage((Item) new MeleeWeapon(0), true);
          else if (split.Length > 1 && split[1].Equals("ore"))
            Game1.player.holdUpItemThenMessage((Item) new Object(378, 1, false, -1, 0), false);
          else
            Game1.player.holdUpItemThenMessage((Item) null, false);
          this.CurrentCommand = this.CurrentCommand + 1;
        }
        else if (split[0].Equals("addCraftingRecipe"))
        {
          if (!Game1.player.craftingRecipes.ContainsKey(this.eventCommands[this.CurrentCommand].Substring(this.eventCommands[this.CurrentCommand].IndexOf(' ') + 1)))
            Game1.player.craftingRecipes.Add(this.eventCommands[this.CurrentCommand].Substring(this.eventCommands[this.CurrentCommand].IndexOf(' ') + 1), 0);
          this.CurrentCommand = this.CurrentCommand + 1;
        }
        else if (split[0].Equals("mail"))
        {
          if (!Game1.player.hasOrWillReceiveMail(split[1]))
            Game1.addMailForTomorrow(split[1], false, false);
          this.CurrentCommand = this.CurrentCommand + 1;
        }
        else if (split[0].Equals("shake"))
        {
          this.getActorByName(split[1]).shake(Convert.ToInt32(split[2]));
          this.CurrentCommand = this.CurrentCommand + 1;
        }
        else if (split[0].Equals("temporarySprite"))
        {
          location.TemporarySprites.Add(new TemporaryAnimatedSprite(Convert.ToInt32(split[3]), new Vector2((float) (Convert.ToInt32(split[1]) * Game1.tileSize), (float) (Convert.ToInt32(split[2]) * Game1.tileSize)), Color.White, Convert.ToInt32(split[4]), split.Length > 6 && split[6] == "true", split.Length > 5 ? (float) Convert.ToInt32(split[5]) : 300f, 0, Game1.tileSize, split.Length > 7 ? (float) Convert.ToDouble(split[7]) : -1f, -1, 0));
          this.CurrentCommand = this.CurrentCommand + 1;
        }
        else if (split[0].Equals("removeTemporarySprites"))
        {
          location.TemporarySprites.Clear();
          this.CurrentCommand = this.CurrentCommand + 1;
        }
        else if (split[0].Equals("specificTemporarySprite"))
        {
          this.addSpecificTemporarySprite(split[1], location, split);
          this.CurrentCommand = this.CurrentCommand + 1;
        }
        else if (split[0].Equals("playMusic"))
        {
          if (split[1].Equals("samBand"))
          {
            if (Game1.player.DialogueQuestionsAnswered.Contains(78))
              Game1.changeMusicTrack("shimmeringbastion");
            else if (Game1.player.DialogueQuestionsAnswered.Contains(79))
              Game1.changeMusicTrack("honkytonky");
            else if (Game1.player.DialogueQuestionsAnswered.Contains(76))
              Game1.changeMusicTrack("poppy");
            else if (Game1.player.DialogueQuestionsAnswered.Contains(77))
              Game1.changeMusicTrack("heavy");
          }
          else if ((double) Game1.options.musicVolumeLevel > 0.0)
          {
            StringBuilder stringBuilder = new StringBuilder(split[1]);
            for (int index = 2; index < split.Length; ++index)
              stringBuilder.Append(" " + split[index]);
            Game1.changeMusicTrack(stringBuilder.ToString());
          }
          this.CurrentCommand = this.CurrentCommand + 1;
        }
        else if (split[0].Equals("nameSelect"))
        {
          if (!Game1.nameSelectUp)
            Game1.showNameSelectScreen(split[1]);
        }
        else if (split[0].Equals("characterSelect"))
        {
          if ((int) Game1.gameMode != 5)
          {
            Game1.gameMode = (byte) 5;
            Game1.menuChoice = 0;
          }
        }
        else if (split[0].Equals("addObject"))
        {
          location.TemporarySprites.Add(new TemporaryAnimatedSprite(Convert.ToInt32(split[3]), 9999f, 1, 9999, new Vector2((float) Convert.ToInt32(split[1]), (float) Convert.ToInt32(split[2])) * (float) Game1.tileSize, false, false)
          {
            layerDepth = (float) ((double) (Convert.ToInt32(split[2]) * Game1.tileSize) / 10000.0)
          });
          this.CurrentCommand = this.CurrentCommand + 1;
          this.checkForNextCommand(location, time);
        }
        else if (split[0].Equals("addBigProp"))
        {
          this.props.Add(new Object(new Vector2((float) Convert.ToInt32(split[1]), (float) Convert.ToInt32(split[2])), Convert.ToInt32(split[3]), false));
          this.CurrentCommand = this.CurrentCommand + 1;
          this.checkForNextCommand(location, time);
        }
        else if (split[0].Equals("addProp") || split[0].Equals("addFloorProp"))
        {
          int int32_1 = Convert.ToInt32(split[2]);
          int int32_2 = Convert.ToInt32(split[3]);
          int int32_3 = Convert.ToInt32(split[1]);
          int tilesWideSolid = split.Length > 4 ? Convert.ToInt32(split[4]) : 1;
          int tilesHighDraw = split.Length > 5 ? Convert.ToInt32(split[5]) : 1;
          int tilesHighSolid = split.Length > 6 ? Convert.ToInt32(split[6]) : tilesHighDraw;
          bool solid = !split[0].Contains("Floor");
          this.festivalProps.Add(new Prop(this.festivalTexture, int32_3, tilesWideSolid, tilesHighSolid, tilesHighDraw, int32_1, int32_2, solid));
          if (split.Length > 7)
          {
            int int32_4 = Convert.ToInt32(split[7]);
            int tileX = int32_1 + int32_4;
            while (tileX != int32_1)
            {
              this.festivalProps.Add(new Prop(this.festivalTexture, int32_3, tilesWideSolid, tilesHighSolid, tilesHighDraw, tileX, int32_2, solid));
              tileX -= Math.Sign(int32_4);
            }
          }
          if (split.Length > 8)
          {
            int int32_4 = Convert.ToInt32(split[8]);
            int tileY = int32_2 + int32_4;
            while (tileY != int32_2)
            {
              this.festivalProps.Add(new Prop(this.festivalTexture, int32_3, tilesWideSolid, tilesHighSolid, tilesHighDraw, int32_1, tileY, solid));
              tileY -= Math.Sign(int32_4);
            }
          }
          this.CurrentCommand = this.CurrentCommand + 1;
          this.checkForNextCommand(location, time);
        }
        else if (split[0].Equals("addToTable"))
        {
          if (location is FarmHouse)
            (location as FarmHouse).furniture[0].heldObject = new Object(Vector2.Zero, Convert.ToInt32(split[3]), 1);
          else
            location.objects[new Vector2((float) Convert.ToInt32(split[1]), (float) Convert.ToInt32(split[2]))].heldObject = new Object(Vector2.Zero, Convert.ToInt32(split[3]), 1);
          this.CurrentCommand = this.CurrentCommand + 1;
          this.checkForNextCommand(location, time);
        }
        else if (split[0].Equals("removeObject"))
        {
          Vector2 other = new Vector2((float) Convert.ToInt32(split[1]), (float) Convert.ToInt32(split[2]));
          for (int index = this.props.Count - 1; index >= 0; --index)
          {
            if (this.props[index].TileLocation.Equals(other))
            {
              this.props.RemoveAt(index);
              break;
            }
          }
          this.CurrentCommand = this.CurrentCommand + 1;
          this.checkForNextCommand(location, time);
        }
        else if (split[0].Equals("glow"))
        {
          bool hold = false;
          if (split.Length > 4 && split[4].Equals("true"))
            hold = true;
          Game1.screenGlowOnce(new Color(Convert.ToInt32(split[1]), Convert.ToInt32(split[2]), Convert.ToInt32(split[3])), hold, 0.005f, 0.3f);
          this.CurrentCommand = this.CurrentCommand + 1;
        }
        else if (split[0].Equals("stopGlowing"))
        {
          Game1.screenGlowUp = false;
          Game1.screenGlowHold = false;
          this.CurrentCommand = this.CurrentCommand + 1;
        }
        else if (split[0].Equals("addQuest"))
        {
          Game1.player.addQuest(Convert.ToInt32(split[1]));
          this.CurrentCommand = this.CurrentCommand + 1;
        }
        else if (split[0].Equals("removeQuest"))
        {
          Game1.player.removeQuest(Convert.ToInt32(split[1]));
          this.CurrentCommand = this.CurrentCommand + 1;
        }
        else if (split[0].Equals("awardFestivalPrize"))
        {
          if (Game1.player.festivalScore == -9999)
          {
            string str = this.festivalData["file"];
            if (!(str == "spring13"))
            {
              if (str == "winter8")
              {
                if (!Game1.player.mailReceived.Contains("Ice Festival"))
                {
                  if (Game1.activeClickableMenu == null)
                    Game1.activeClickableMenu = (IClickableMenu) new ItemGrabMenu(new List<Item>()
                    {
                      (Item) new Hat(17),
                      (Item) new Object(687, 1, false, -1, 0),
                      (Item) new Object(691, 1, false, -1, 0),
                      (Item) new Object(703, 1, false, -1, 0)
                    });
                  Game1.player.mailReceived.Add("Ice Festival");
                  this.CurrentCommand = this.CurrentCommand + 1;
                  return;
                }
                Game1.player.money += 2000;
                Game1.playSound("money");
                Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1164"));
                this.CurrentCommand = this.CurrentCommand + 2;
              }
            }
            else
            {
              if (!Game1.player.mailReceived.Contains("Egg Festival"))
              {
                if (Game1.activeClickableMenu == null)
                  Game1.player.addItemByMenuIfNecessary((Item) new Hat(4), (ItemGrabMenu.behaviorOnItemSelect) null);
                Game1.player.mailReceived.Add("Egg Festival");
                this.CurrentCommand = this.CurrentCommand + 1;
                if (Game1.activeClickableMenu != null)
                  return;
                this.CurrentCommand = this.CurrentCommand + 1;
                return;
              }
              Game1.player.money += 1000;
              Game1.playSound("money");
              this.CurrentCommand = this.CurrentCommand + 2;
              Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1159"));
            }
          }
          else if (split.Length > 1)
          {
            string lower = split[1].ToLower();
            // ISSUE: reference to a compiler-generated method
            uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(lower);
            if (stringHash <= 890111454U)
            {
              if ((int) stringHash != 456875097)
              {
                if ((int) stringHash != 659772054)
                {
                  if ((int) stringHash == 890111454 && lower == "rod")
                  {
                    Game1.player.addItemByMenuIfNecessary((Item) new FishingRod(), (ItemGrabMenu.behaviorOnItemSelect) null);
                    if (Game1.activeClickableMenu == null)
                      this.CurrentCommand = this.CurrentCommand + 1;
                    this.CurrentCommand = this.CurrentCommand + 1;
                    return;
                  }
                }
                else if (lower == "sword")
                {
                  Game1.player.addItemByMenuIfNecessary((Item) new MeleeWeapon(0), (ItemGrabMenu.behaviorOnItemSelect) null);
                  if (Game1.activeClickableMenu == null)
                    this.CurrentCommand = this.CurrentCommand + 1;
                  this.CurrentCommand = this.CurrentCommand + 1;
                  return;
                }
              }
              else if (lower == "hero")
              {
                Game1.getSteamAchievement("Achievement_LocalLegend");
                Game1.player.addItemByMenuIfNecessary((Item) new Object(Vector2.Zero, 116, false), (ItemGrabMenu.behaviorOnItemSelect) null);
                if (Game1.activeClickableMenu == null)
                  this.CurrentCommand = this.CurrentCommand + 1;
                this.CurrentCommand = this.CurrentCommand + 1;
                return;
              }
            }
            else if (stringHash <= 1716282848U)
            {
              if ((int) stringHash != 1331031788)
              {
                if ((int) stringHash == 1716282848 && lower == "slimeegg")
                {
                  Game1.player.addItemByMenuIfNecessary((Item) new Object(680, 1, false, -1, 0), (ItemGrabMenu.behaviorOnItemSelect) null);
                  if (Game1.activeClickableMenu == null)
                    this.CurrentCommand = this.CurrentCommand + 1;
                  this.CurrentCommand = this.CurrentCommand + 1;
                  return;
                }
              }
              else if (lower == "pan")
              {
                Game1.player.addItemByMenuIfNecessary((Item) new Pan(), (ItemGrabMenu.behaviorOnItemSelect) null);
                if (Game1.activeClickableMenu == null)
                  this.CurrentCommand = this.CurrentCommand + 1;
                this.CurrentCommand = this.CurrentCommand + 1;
                return;
              }
            }
            else if ((int) stringHash != -501574385)
            {
              if ((int) stringHash == -474961868 && lower == "sculpture")
              {
                Game1.player.addItemByMenuIfNecessary((Item) new Furniture(1306, Vector2.Zero), (ItemGrabMenu.behaviorOnItemSelect) null);
                if (Game1.activeClickableMenu == null)
                  this.CurrentCommand = this.CurrentCommand + 1;
                this.CurrentCommand = this.CurrentCommand + 1;
                return;
              }
            }
            else if (lower == "joja")
            {
              Game1.getSteamAchievement("Achievement_Joja");
              Game1.player.addItemByMenuIfNecessary((Item) new Object(Vector2.Zero, 117, false), (ItemGrabMenu.behaviorOnItemSelect) null);
              if (Game1.activeClickableMenu == null)
                this.CurrentCommand = this.CurrentCommand + 1;
              this.CurrentCommand = this.CurrentCommand + 1;
              return;
            }
          }
          else
            this.CurrentCommand = this.CurrentCommand + 2;
        }
        else if (split[0].Equals("pixelZoom"))
        {
          Game1.pixelZoom = Convert.ToInt32(split[1]);
          this.CurrentCommand = this.CurrentCommand + 1;
        }
        else if (split[0].Equals("attachCharacterToTempSprite"))
        {
          TemporaryAnimatedSprite temporaryAnimatedSprite = location.temporarySprites.Last<TemporaryAnimatedSprite>();
          if (temporaryAnimatedSprite != null)
            temporaryAnimatedSprite.attachedCharacter = (Character) this.getActorByName(split[1]);
        }
        else if (split[0].Equals("fork"))
        {
          if (split.Length > 2)
          {
            int result;
            if (Game1.player.mailReceived.Contains(split[1]) || int.TryParse(split[1], out result) && Game1.player.dialogueQuestionsAnswered.Contains(result))
            {
              this.eventCommands = Game1.content.Load<Dictionary<string, string>>("Data\\Events\\" + Game1.currentLocation.Name)[split[2]].Split('/');
              this.CurrentCommand = 0;
              this.forked = !this.forked;
            }
            else
              this.CurrentCommand = this.CurrentCommand + 1;
          }
          else if (this.specialEventVariable1)
          {
            string[] strArray;
            if (!this.isFestival)
              strArray = Game1.content.Load<Dictionary<string, string>>("Data\\Events\\" + Game1.currentLocation.Name)[split[1]].Split('/');
            else
              strArray = this.festivalData[split[1]].Split('/');
            this.eventCommands = strArray;
            this.CurrentCommand = 0;
            this.forked = !this.forked;
          }
          else
            this.CurrentCommand = this.CurrentCommand + 1;
        }
        else if (split[0].Equals("switchEvent"))
        {
          string[] strArray;
          if (this.isFestival)
            strArray = this.festivalData[split[1]].Split('/');
          else
            strArray = Game1.content.Load<Dictionary<string, string>>("Data\\Events\\" + Game1.currentLocation.Name)[split[1]].Split('/');
          this.eventCommands = strArray;
          this.CurrentCommand = 0;
          this.eventSwitched = true;
        }
        else if (split[0].Equals("globalFade"))
        {
          if (!Game1.globalFade)
          {
            if (split.Length > 2)
            {
              Game1.globalFadeToBlack((Game1.afterFadeFunction) null, split.Length > 1 ? (float) Convert.ToDouble(split[1]) : 0.007f);
              this.CurrentCommand = this.CurrentCommand + 1;
            }
            else
              Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.incrementCommandAfterFade), split.Length > 1 ? (float) Convert.ToDouble(split[1]) : 0.007f);
          }
        }
        else if (split[0].Equals("globalFadeToClear"))
        {
          if (!Game1.globalFade)
          {
            if (split.Length > 2)
            {
              Game1.globalFadeToClear((Game1.afterFadeFunction) null, split.Length > 1 ? (float) Convert.ToDouble(split[1]) : 0.007f);
              this.CurrentCommand = this.CurrentCommand + 1;
            }
            else
              Game1.globalFadeToClear(new Game1.afterFadeFunction(this.incrementCommandAfterFade), split.Length > 1 ? (float) Convert.ToDouble(split[1]) : 0.007f);
          }
        }
        else if (split[0].Equals("cutscene"))
        {
          if (Game1.currentMinigame == null)
          {
            string s = split[1];
            // ISSUE: reference to a compiler-generated method
            uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(s);
            if (stringHash <= 679977874U)
            {
              if (stringHash <= 306545799U)
              {
                if ((int) stringHash != 48516534)
                {
                  if ((int) stringHash != 113222854)
                  {
                    if ((int) stringHash == 306545799 && s == "linusMoneyGone")
                    {
                      foreach (TemporaryAnimatedSprite temporarySprite in location.temporarySprites)
                      {
                        double num = 0.00999999977648258;
                        temporarySprite.alphaFade = (float) num;
                        Vector2 vector2 = new Vector2(0.0f, -1f);
                        temporarySprite.motion = vector2;
                      }
                      this.CurrentCommand = this.CurrentCommand + 1;
                      return;
                    }
                  }
                  else if (s == "clearTempSprites")
                  {
                    location.temporarySprites.Clear();
                    this.CurrentCommand = this.CurrentCommand + 1;
                  }
                }
                else if (s == "AbigailGame")
                  Game1.currentMinigame = (IMinigame) new AbigailGame(true);
              }
              else if (stringHash <= 477618658U)
              {
                if ((int) stringHash != 323687113)
                {
                  if ((int) stringHash == 477618658 && s == "governorTaste")
                  {
                    this.governorTaste();
                    this.currentCommand = this.currentCommand + 1;
                    return;
                  }
                }
                else if (s == "boardGame")
                {
                  Game1.currentMinigame = (IMinigame) new FantasyBoardGame();
                  this.CurrentCommand = this.CurrentCommand + 1;
                }
              }
              else if ((int) stringHash != 658671424)
              {
                if ((int) stringHash == 679977874 && s == "balloonChangeMap")
                {
                  location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(0, 1183, 84, 160), 10000f, 1, 99999, new Vector2(22f, 36f) * (float) Game1.tileSize + new Vector2(-23f, 0.0f) * (float) Game1.pixelZoom, false, false, 2E-05f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
                  {
                    motion = new Vector2(0.0f, -2f),
                    yStopCoordinate = 9 * Game1.tileSize,
                    reachedStopCoordinate = new TemporaryAnimatedSprite.endBehavior(this.balloonInSky),
                    attachedCharacter = (Character) Game1.player,
                    id = 1f
                  });
                  location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(84, 1205, 38, 26), 10000f, 1, 99999, new Vector2(22f, 36f) * (float) Game1.tileSize + new Vector2(0.0f, 134f) * (float) Game1.pixelZoom, false, false, (float) ((double) (41 * Game1.tileSize) / 10000.0 + 9.99999974737875E-05), 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
                  {
                    motion = new Vector2(0.0f, -2f),
                    id = 2f,
                    attachedCharacter = (Character) this.getActorByName("Harvey")
                  });
                  this.CurrentCommand = this.CurrentCommand + 1;
                }
              }
              else if (s == "eggHuntWinner")
              {
                this.eggHuntWinner();
                this.CurrentCommand = this.CurrentCommand + 1;
                return;
              }
            }
            else if (stringHash <= 3320997440U)
            {
              if ((int) stringHash != -2023567283)
              {
                if ((int) stringHash != -1287780222)
                {
                  if ((int) stringHash == -973969856 && s == "haleyCows")
                    Game1.currentMinigame = (IMinigame) new HaleyCowPictures();
                }
                else if (s == "bandFork")
                {
                  int answerChoice = 76;
                  if (Game1.player.dialogueQuestionsAnswered.Contains(77))
                    answerChoice = 77;
                  else if (Game1.player.dialogueQuestionsAnswered.Contains(78))
                    answerChoice = 78;
                  else if (Game1.player.dialogueQuestionsAnswered.Contains(79))
                    answerChoice = 79;
                  this.answerDialogue("bandFork", answerChoice);
                  this.CurrentCommand = this.CurrentCommand + 1;
                  return;
                }
              }
              else if (s == "iceFishingWinner")
              {
                this.iceFishingWinner();
                this.currentCommand = this.currentCommand + 1;
                return;
              }
            }
            else if (stringHash <= 3457429727U)
            {
              if ((int) stringHash != -859111339)
              {
                if ((int) stringHash == -837537569 && s == "robot")
                  Game1.currentMinigame = (IMinigame) new RobotBlastoff();
              }
              else if (s == "plane")
                Game1.currentMinigame = (IMinigame) new PlaneFlyBy();
            }
            else if ((int) stringHash != -520418454)
            {
              if ((int) stringHash == -32084758 && s == "addSecretSantaItem")
              {
                Game1.player.addItemByMenuIfNecessaryElseHoldUp(Utility.getGiftFromNPC(this.mySecretSanta), (ItemGrabMenu.behaviorOnItemSelect) null);
                this.currentCommand = this.currentCommand + 1;
                return;
              }
            }
            else if (s == "balloonDepart")
            {
              TemporaryAnimatedSprite temporarySpriteById1 = location.getTemporarySpriteByID(1);
              Farmer player = Game1.player;
              temporarySpriteById1.attachedCharacter = (Character) player;
              Vector2 vector2_1 = new Vector2(0.0f, -2f);
              temporarySpriteById1.motion = vector2_1;
              TemporaryAnimatedSprite temporarySpriteById2 = location.getTemporarySpriteByID(2);
              NPC actorByName = this.getActorByName("Harvey");
              temporarySpriteById2.attachedCharacter = (Character) actorByName;
              Vector2 vector2_2 = new Vector2(0.0f, -2f);
              temporarySpriteById2.motion = vector2_2;
              location.getTemporarySpriteByID(3).scaleChange = -0.01f;
              this.CurrentCommand = this.CurrentCommand + 1;
              return;
            }
            Game1.globalFadeToClear((Game1.afterFadeFunction) null, 0.02f);
          }
        }
        else if (split[0].Equals("grabObject"))
        {
          Game1.player.grabObject(new Object(Vector2.Zero, Convert.ToInt32(split[1]), (string) null, false, true, false, false));
          this.showActiveObject = true;
          this.CurrentCommand = this.CurrentCommand + 1;
        }
        else if (split[0].Equals("addTool"))
        {
          if (split[1].Equals("Sword"))
          {
            if (!Game1.player.addItemToInventoryBool((Item) new Sword("Battered Sword", 67), false))
            {
              Game1.player.addItemToInventoryBool((Item) new Sword("Battered Sword", 67), false);
              Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1209")));
            }
            else
            {
              for (int index = 0; index < Game1.player.Items.Count<Item>(); ++index)
              {
                if (Game1.player.Items[index] != null && Game1.player.Items[index] is Tool && Game1.player.Items[index].Name.Contains("Sword"))
                {
                  Game1.player.CurrentToolIndex = index;
                  Game1.switchToolAnimation();
                  break;
                }
              }
            }
          }
          else if (split[1].Equals("Wand") && !Game1.player.addItemToInventoryBool((Item) new Wand(), false))
          {
            Game1.player.addItemToInventoryBool((Item) new Wand(), false);
            Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1212")));
          }
          this.CurrentCommand = this.CurrentCommand + 1;
        }
        else if (split[0].Equals("waitForKey"))
        {
          string str = split[1];
          KeyboardState state = Keyboard.GetState();
          bool flag = false;
          if (!Game1.player.UsingTool && !Game1.pickingTool)
          {
            foreach (Keys pressedKey in state.GetPressedKeys())
            {
              if (Enum.GetName(pressedKey.GetType(), (object) pressedKey).Equals(str.ToUpper()))
              {
                flag = true;
                if (pressedKey != Keys.C)
                {
                  if (pressedKey != Keys.S)
                  {
                    if (pressedKey == Keys.Z)
                    {
                      Game1.pressSwitchToolButton();
                      break;
                    }
                    break;
                  }
                  Game1.pressAddItemToInventoryButton();
                  this.showActiveObject = false;
                  Game1.player.showNotCarrying();
                  break;
                }
                Game1.pressUseToolButton();
                Game1.releaseUseToolButton();
                break;
              }
            }
          }
          this.messageToScreen = this.eventCommands[this.CurrentCommand].Substring(this.eventCommands[this.CurrentCommand].IndexOf('"') + 1, this.eventCommands[this.CurrentCommand].Substring(this.eventCommands[this.CurrentCommand].IndexOf('"') + 1).IndexOf('"'));
          if (flag)
          {
            this.messageToScreen = (string) null;
            this.CurrentCommand = this.CurrentCommand + 1;
          }
        }
        else if (split[0].Equals("cave"))
        {
          if (Game1.activeClickableMenu == null)
          {
            Response[] answerChoices = new Response[2]
            {
              new Response("Mushrooms", Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1220")),
              new Response("Bats", Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1222"))
            };
            Game1.currentLocation.createQuestionDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1223"), answerChoices, "cave");
            Game1.dialogueTyping = false;
          }
        }
        else if (split[0].Equals("updateMinigame"))
        {
          if (Game1.currentMinigame != null)
            Game1.currentMinigame.receiveEventPoke(Convert.ToInt32(split[1]));
          this.CurrentCommand = this.CurrentCommand + 1;
        }
        else if (split[0].Equals("startJittering"))
        {
          Game1.player.jitterStrength = 1f;
          this.CurrentCommand = this.CurrentCommand + 1;
        }
        else if (split[0].Equals("stopJittering"))
        {
          Game1.player.stopJittering();
          this.CurrentCommand = this.CurrentCommand + 1;
        }
        else if (split[0].Equals("addLantern"))
        {
          List<TemporaryAnimatedSprite> temporarySprites = location.TemporarySprites;
          TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Convert.ToInt32(split[1]), 999999f, 1, 0, new Vector2((float) Convert.ToInt32(split[2]), (float) Convert.ToInt32(split[3])) * (float) Game1.tileSize, false, false);
          temporaryAnimatedSprite.light = true;
          double int32 = (double) Convert.ToInt32(split[4]);
          temporaryAnimatedSprite.lightRadius = (float) int32;
          temporarySprites.Add(temporaryAnimatedSprite);
          this.CurrentCommand = this.CurrentCommand + 1;
        }
        else if (split[0].Equals("rustyKey"))
        {
          Game1.player.hasRustyKey = true;
          this.CurrentCommand = this.CurrentCommand + 1;
        }
        else if (split[0].Equals("swimming"))
        {
          if (split[1].Equals("farmer"))
          {
            Game1.player.bathingClothes = true;
            Game1.player.swimming = true;
          }
          else
            this.getActorByName(split[1]).swimming = true;
          this.CurrentCommand = this.CurrentCommand + 1;
        }
        else if (split[0].Equals("stopSwimming"))
        {
          if (split[1].Equals("farmer"))
          {
            Game1.player.bathingClothes = location is BathHousePool;
            Game1.player.swimming = false;
          }
          else
            this.getActorByName(split[1]).swimming = false;
          this.CurrentCommand = this.CurrentCommand + 1;
        }
        else if (split[0].Equals("tutorialMenu"))
        {
          if (Game1.activeClickableMenu == null)
            Game1.activeClickableMenu = (IClickableMenu) new TutorialMenu();
        }
        else if (split[0].Equals("animalNaming"))
        {
          if (Game1.activeClickableMenu == null)
            Game1.activeClickableMenu = (IClickableMenu) new NamingMenu(new NamingMenu.doneNamingBehavior((Game1.currentLocation as AnimalHouse).addNewHatchedAnimal), Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1236"), (string) null);
        }
        else if (split[0].Equals("splitSpeak"))
        {
          if (!Game1.dialogueUp)
          {
            double timeAccumulator = (double) this.timeAccumulator;
            elapsedGameTime = time.ElapsedGameTime;
            double milliseconds = (double) elapsedGameTime.Milliseconds;
            this.timeAccumulator = (float) (timeAccumulator + milliseconds);
            if ((double) this.timeAccumulator < 500.0)
              return;
            this.timeAccumulator = 0.0f;
            string[] strArray = this.eventCommands[this.CurrentCommand].Substring(this.eventCommands[this.CurrentCommand].IndexOf('"') + 1, this.eventCommands[this.CurrentCommand].Substring(this.eventCommands[this.CurrentCommand].IndexOf('"') + 1).IndexOf('"')).Split('~');
            NPC speaker = Game1.getCharacterFromName(split[1].Equals("rival") ? Utility.getOtherFarmerNames()[0] : split[1], false) ?? this.getActorByName(split[1]);
            if (speaker == null || this.previousAnswerChoice < 0 || this.previousAnswerChoice >= strArray.Length)
            {
              this.CurrentCommand = this.CurrentCommand + 1;
              return;
            }
            speaker.CurrentDialogue.Push(new Dialogue(strArray[this.previousAnswerChoice], speaker));
            Game1.drawDialogue(speaker);
          }
        }
        else if (split[0].Equals("catQuestion"))
        {
          if (!Game1.isQuestion && Game1.activeClickableMenu == null)
            Game1.currentLocation.createQuestionDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1241") + (Game1.player.catPerson ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1242") : Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1243")) + Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1244"), Game1.currentLocation.createYesNoResponses(), "pet");
        }
        else if (split[0].Equals("taxvote"))
        {
          if (!Game1.isQuestion)
          {
            Response[] answerChoices = new Response[2]
            {
              new Response("For", Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1247")),
              new Response("Against", Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1250"))
            };
            Game1.currentLocation.createQuestionDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1251"), answerChoices, "taxvote");
            Game1.dialogueTyping = false;
            Game1.currentDialogueCharacterIndex = Game1.currentObjectDialogue.Peek().Length - 1;
          }
        }
        else if (split[0].Equals("ambientLight"))
        {
          Game1.ambientLight = new Color(Convert.ToInt32(split[1]), Convert.ToInt32(split[2]), Convert.ToInt32(split[3]));
          this.CurrentCommand = this.CurrentCommand + 1;
        }
        else if (split[0].Equals("bloom"))
        {
          Game1.bloom.Settings = new BloomSettings("eventBloom", (float) Convert.ToDouble(split[1]) / 10f, (float) Convert.ToDouble(split[2]) / 10f, (float) Convert.ToDouble(split[3]) / 10f, (float) Convert.ToDouble(split[4]) / 10f, (float) Convert.ToDouble(split[5]) / 10f, (float) Convert.ToDouble(split[6]) / 10f, split.Length > 7);
          Game1.bloom.reload();
          Game1.bloomDay = true;
          Game1.bloom.Visible = true;
          this.CurrentCommand = this.CurrentCommand + 1;
        }
        else if (split[0].Equals("elliottbooktalk"))
        {
          if (!Game1.dialogueUp)
          {
            string masterDialogue = !Game1.player.dialogueQuestionsAnswered.Contains(958699) ? (!Game1.player.dialogueQuestionsAnswered.Contains(958700) ? (!Game1.player.dialogueQuestionsAnswered.Contains(9586701) ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1260") : Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1259")) : Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1258")) : Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1257");
            NPC characterFromName = Game1.getCharacterFromName("Elliott", false);
            characterFromName.CurrentDialogue.Push(new Dialogue(masterDialogue, characterFromName));
            Game1.drawDialogue(characterFromName);
          }
        }
        else if (split[0].Equals("removeItem"))
        {
          Game1.player.removeFirstOfThisItemFromInventory(Convert.ToInt32(split[1]));
          this.CurrentCommand = this.CurrentCommand + 1;
        }
        else if (split[0].Equals("friendship"))
        {
          Game1.player.friendships[split[1]][0] += Convert.ToInt32(split[2]);
          this.CurrentCommand = this.CurrentCommand + 1;
        }
      }
      if (split[0].Equals("setRunning"))
      {
        Game1.player.setRunning(true, false);
        this.CurrentCommand = this.CurrentCommand + 1;
      }
      if (split[0].Equals("extendSourceRect"))
      {
        if (split[2].Equals("reset"))
        {
          this.getActorByName(split[1]).reloadSprite();
          this.getActorByName(split[1]).sprite.spriteWidth = 16;
          this.getActorByName(split[1]).sprite.spriteHeight = 32;
        }
        else
          this.getActorByName(split[1]).extendSourceRect(Convert.ToInt32(split[2]), Convert.ToInt32(split[3]), split.Length <= 4);
        this.CurrentCommand = this.CurrentCommand + 1;
      }
      if (split[0].Equals("waitForOtherPlayers"))
      {
        if (Game1.IsMultiplayer)
        {
          if (Game1.IsServer)
            Game1.player.readyConfirmation = true;
          int confirmationTimer = this.readyConfirmationTimer;
          elapsedGameTime = time.ElapsedGameTime;
          int milliseconds = elapsedGameTime.Milliseconds;
          this.readyConfirmationTimer = confirmationTimer - milliseconds;
          if (this.readyConfirmationTimer <= 0 && Game1.IsClient)
          {
            MultiplayerUtility.sendReadyConfirmation(Game1.player.uniqueMultiplayerID);
            this.sentReadyConfirmation = true;
            this.readyConfirmationTimer = 2000;
          }
          if (this.allPlayersReady)
          {
            this.readyConfirmationTimer = -1;
            this.allPlayersReady = false;
            this.sentReadyConfirmation = false;
            this.CurrentCommand = this.CurrentCommand + 1;
          }
        }
        else
          this.CurrentCommand = this.CurrentCommand + 1;
      }
      if (split[0].Equals("advancedMove"))
      {
        this.setUpAdvancedMove(split, (NPCController.endBehavior) null);
        this.CurrentCommand = this.CurrentCommand + 1;
      }
      if (split[0].Equals("stopRunning"))
      {
        Game1.player.setRunning(false, false);
        this.CurrentCommand = this.CurrentCommand + 1;
      }
      if (split[0].Equals("eyes"))
      {
        Game1.player.currentEyes = Convert.ToInt32(split[1]);
        Game1.player.blinkTimer = Convert.ToInt32(split[2]);
        this.CurrentCommand = this.CurrentCommand + 1;
      }
      if (split[0].Equals("addMailReceived"))
      {
        Game1.player.mailReceived.Add(split[1]);
        this.CurrentCommand = this.CurrentCommand + 1;
      }
      if (split[0].Equals("fade"))
      {
        Game1.fadeToBlack = true;
        Game1.fadeIn = true;
        if ((double) Game1.fadeToBlackAlpha >= 0.970000028610229)
        {
          if (split.Length == 1)
            Game1.fadeIn = false;
          this.CurrentCommand = this.CurrentCommand + 1;
        }
      }
      else if (split[0].Equals("changeMapTile"))
      {
        string layerId = split[1];
        int int32_1 = Convert.ToInt32(split[2]);
        int int32_2 = Convert.ToInt32(split[3]);
        int int32_3 = Convert.ToInt32(split[4]);
        location.map.GetLayer(layerId).Tiles[int32_1, int32_2].TileIndex = int32_3;
        this.CurrentCommand = this.CurrentCommand + 1;
      }
      else if (split[0].Equals("changeSprite"))
      {
        this.getActorByName(split[1]).Sprite.Texture = Game1.content.Load<Texture2D>("Characters\\" + split[1] + "_" + split[2]);
        this.CurrentCommand = this.CurrentCommand + 1;
      }
      else if (split[0].Equals("proceedPosition"))
      {
        this.continueAfterMove = true;
        try
        {
          if (this.getCharacterByName(split[1]).isMoving())
          {
            if (this.npcControllers != null)
            {
              if (this.npcControllers.Count != 0)
                goto label_652;
            }
            else
              goto label_652;
          }
          this.getCharacterByName(split[1]).Halt();
          this.CurrentCommand = this.CurrentCommand + 1;
        }
        catch (Exception ex)
        {
          this.CurrentCommand = this.CurrentCommand + 1;
        }
      }
      else if (split[0].Equals("changePortrait"))
      {
        NPC characterFromName = Game1.getCharacterFromName(split[1], false);
        characterFromName.Portrait = Game1.content.Load<Texture2D>("Portraits\\" + split[1] + "_" + split[2]);
        characterFromName.uniquePortraitActive = true;
        this.npcsWithUniquePortraits.Add(characterFromName);
        this.CurrentCommand = this.CurrentCommand + 1;
      }
      else if (split[0].Equals("changeYSourceRectOffset"))
      {
        NPC actorByName = this.getActorByName(split[1]);
        if (actorByName != null)
          actorByName.ySourceRectOffset = Convert.ToInt32(split[2]);
        this.CurrentCommand = this.CurrentCommand + 1;
      }
label_652:
      if (split[0].Equals("addTemporaryActor"))
      {
        string str = "Characters\\";
        if (split.Length > 8 && split[8].Equals("Animal"))
          str = "Animals\\";
        if (split.Length > 8 && split[8].Equals("Monster"))
          str = "Characters\\Monsters\\";
        NPC speaker = new NPC(new AnimatedSprite(this.festivalContent.Load<Texture2D>(str + split[1].Replace('_', ' ')), 0, Convert.ToInt32(split[2]), Convert.ToInt32(split[3])), new Vector2((float) Convert.ToInt32(split[4]), (float) Convert.ToInt32(split[5])) * (float) Game1.tileSize, Convert.ToInt32(split[6]), split[1].Replace('_', ' '), this.festivalContent);
        if (split.Length > 7)
          speaker.breather = Convert.ToBoolean(split[7]);
        if (this.isFestival)
        {
          try
          {
            speaker.CurrentDialogue.Push(new Dialogue(this.festivalData[speaker.name], speaker));
          }
          catch (Exception ex)
          {
          }
        }
        if (str.Contains("Animals") && split.Length > 9)
          speaker.name = split[9];
        this.actors.Add(speaker);
        this.CurrentCommand = this.CurrentCommand + 1;
      }
      else if (split[0].Equals("changeToTemporaryMap"))
      {
        this.temporaryLocation = new GameLocation(this.festivalContent.Load<Map>("Maps\\" + split[1]), "Temp");
        this.temporaryLocation.map.LoadTileSheets(Game1.mapDisplayDevice);
        Event currentEvent = Game1.currentLocation.currentEvent;
        Game1.currentLocation.cleanupBeforePlayerExit();
        Game1.currentLightSources.Clear();
        Game1.currentLocation = this.temporaryLocation;
        Game1.currentLocation.resetForPlayerEntry();
        Game1.currentLocation.currentEvent = currentEvent;
        this.CurrentCommand = this.CurrentCommand + 1;
        Game1.player.currentLocation = Game1.currentLocation;
        if (this.isFestival)
        {
          foreach (Farmer farmer in Game1.otherFarmers.Values)
          {
            farmer.currentLocation = Game1.currentLocation;
            Game1.currentLocation.farmers.Add(farmer);
          }
        }
        if (split.Length >= 3)
          return;
        Game1.panScreen(0, 0);
      }
      else if (split[0].Equals("positionOffset"))
      {
        if (split[1].Contains("farmer"))
        {
          Farmer farmerNumberString = Utility.getFarmerFromFarmerNumberString(split[1]);
          if (farmerNumberString != null)
          {
            farmerNumberString.position.X += (float) Convert.ToInt32(split[2]);
            farmerNumberString.position.Y += (float) Convert.ToInt32(split[3]);
          }
        }
        else
        {
          NPC actorByName = this.getActorByName(split[1]);
          if (actorByName != null)
          {
            actorByName.position.X += (float) Convert.ToInt32(split[2]);
            actorByName.position.Y += (float) Convert.ToInt32(split[3]);
          }
        }
        this.CurrentCommand = this.CurrentCommand + 1;
        if (split.Length <= 4)
          return;
        this.checkForNextCommand(location, time);
      }
      else if (split[0].Equals("question"))
      {
        if (Game1.isQuestion || Game1.activeClickableMenu != null)
          return;
        string[] strArray = this.eventCommands[Math.Min(this.eventCommands.Length - 1, this.CurrentCommand)].Split('"')[1].Split('#');
        string question = strArray[0];
        Response[] answerChoices = new Response[strArray.Length - 1];
        for (int index = 1; index < strArray.Length; ++index)
          answerChoices[index - 1] = new Response((index - 1).ToString(), strArray[index]);
        Game1.currentLocation.createQuestionDialogue(question, answerChoices, split[1]);
      }
      else if (split[0].Equals("jump"))
      {
        float jumpVelocity = split.Length > 2 ? (float) Convert.ToDouble(split[2]) : 8f;
        if (split[1].Equals("farmer"))
          Game1.player.jump(jumpVelocity);
        else
          this.getActorByName(split[1]).jump(jumpVelocity);
        this.CurrentCommand = this.CurrentCommand + 1;
        this.checkForNextCommand(location, time);
      }
      else if (split[0].Equals("farmerEat"))
      {
        Game1.playerEatObject(new Object(Convert.ToInt32(split[1]), 1, false, -1, 0), true);
        this.CurrentCommand = this.CurrentCommand + 1;
      }
      else if (split[0].Equals("screenFlash"))
      {
        Game1.flashAlpha = (float) Convert.ToDouble(split[1]);
        this.CurrentCommand = this.CurrentCommand + 1;
      }
      else if (split[0].Equals("grandpaCandles"))
      {
        int candlesFromScore = Utility.getGrandpaCandlesFromScore(Utility.getGrandpaScore());
        Game1.getFarm().grandpaScore = candlesFromScore;
        for (int index = 0; index < candlesFromScore; ++index)
          DelayedAction.playSoundAfterDelay("fireball", 100 * index);
        Game1.getFarm().addGrandpaCandles();
        this.CurrentCommand = this.CurrentCommand + 1;
      }
      else if (split[0].Equals("grandpaEvaluation2"))
      {
        switch (Utility.getGrandpaCandlesFromScore(Utility.getGrandpaScore()))
        {
          case 1:
            this.eventCommands[this.currentCommand] = "speak Grandpa \"" + Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1306") + "\"";
            break;
          case 2:
            this.eventCommands[this.currentCommand] = "speak Grandpa \"" + Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1307") + "\"";
            break;
          case 3:
            this.eventCommands[this.currentCommand] = "speak Grandpa \"" + Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1308") + "\"";
            break;
          case 4:
            this.eventCommands[this.currentCommand] = "speak Grandpa \"" + Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1309") + "\"";
            break;
        }
        Game1.player.eventsSeen.Remove(2146991);
      }
      else if (split[0].Equals("grandpaEvaluation"))
      {
        switch (Utility.getGrandpaCandlesFromScore(Utility.getGrandpaScore()))
        {
          case 1:
            this.eventCommands[this.currentCommand] = "speak Grandpa \"" + Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1315") + "\"";
            break;
          case 2:
            this.eventCommands[this.currentCommand] = "speak Grandpa \"" + Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1316") + "\"";
            break;
          case 3:
            this.eventCommands[this.currentCommand] = "speak Grandpa \"" + Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1317") + "\"";
            break;
          case 4:
            this.eventCommands[this.currentCommand] = "speak Grandpa \"" + Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1318") + "\"";
            break;
        }
      }
      else if (split[0].Equals("loadActors"))
      {
        if (this.temporaryLocation != null && this.temporaryLocation.map.GetLayer(split[1]) != null)
        {
          this.actors.Clear();
          if (this.npcControllers != null)
            this.npcControllers.Clear();
          Dictionary<string, string> source = Game1.content.Load<Dictionary<string, string>>("Data\\NPCDispositions");
          for (int x = 0; x < this.temporaryLocation.map.GetLayer(split[1]).LayerWidth; ++x)
          {
            for (int y = 0; y < this.temporaryLocation.map.GetLayer(split[1]).LayerHeight; ++y)
            {
              if (this.temporaryLocation.map.GetLayer(split[1]).Tiles[x, y] != null)
              {
                int index = this.temporaryLocation.map.GetLayer(split[1]).Tiles[x, y].TileIndex / 4;
                int facingDirection = this.temporaryLocation.map.GetLayer(split[1]).Tiles[x, y].TileIndex % 4;
                string key = source.ElementAt<KeyValuePair<string, string>>(index).Key;
                if (key != null && Game1.getCharacterFromName(key, false) != null)
                  this.addActor(key, x, y, facingDirection, this.temporaryLocation);
              }
            }
          }
        }
        this.CurrentCommand = this.CurrentCommand + 1;
      }
      else if (split[0].Equals("playerControl"))
      {
        if (this.playerControlSequence)
          return;
        this.setUpPlayerControlSequence(split[1]);
      }
      else if (split[0].Equals("removeSprite"))
      {
        Vector2 other = new Vector2((float) Convert.ToInt32(split[1]), (float) Convert.ToInt32(split[2])) * (float) Game1.tileSize;
        for (int index = Game1.currentLocation.temporarySprites.Count - 1; index >= 0; --index)
        {
          if (Game1.currentLocation.temporarySprites[index].position.Equals(other))
            Game1.currentLocation.temporarySprites.RemoveAt(index);
        }
        this.CurrentCommand = this.CurrentCommand + 1;
      }
      else
      {
        if (!split[0].Equals("viewport"))
          return;
        if (split[1].Equals("move"))
        {
          this.viewportTarget = new Vector3((float) Convert.ToInt32(split[2]), (float) Convert.ToInt32(split[3]), (float) Convert.ToInt32(split[4]));
        }
        else
        {
          if (this.aboveMapSprites != null && Convert.ToInt32(split[1]) < 0)
          {
            this.aboveMapSprites.Clear();
            this.aboveMapSprites = (List<TemporaryAnimatedSprite>) null;
          }
          Game1.viewportFreeze = true;
          Game1.viewport.X = Convert.ToInt32(split[1]) * Game1.tileSize + Game1.tileSize / 2 - Game1.viewport.Width / 2;
          Game1.viewport.Y = Convert.ToInt32(split[2]) * Game1.tileSize + Game1.tileSize / 2 - Game1.viewport.Height / 2;
          if (Game1.viewport.X > 0 && Game1.viewport.Width > Game1.currentLocation.Map.DisplayWidth)
            Game1.viewport.X = (Game1.currentLocation.Map.DisplayWidth - Game1.viewport.Width) / 2;
          if (Game1.viewport.Y > 0 && Game1.viewport.Height > Game1.currentLocation.Map.DisplayHeight)
            Game1.viewport.Y = (Game1.currentLocation.Map.DisplayHeight - Game1.viewport.Height) / 2;
          if (split.Length > 3 && split[3].Equals("true"))
          {
            Game1.fadeScreenToBlack();
            Game1.fadeToBlackAlpha = 1f;
            Game1.nonWarpFade = true;
          }
          else if (split.Length > 3 && split[3].Equals("clamp"))
          {
            Viewport viewport;
            if (Game1.viewport.X + Game1.viewport.Width > Game1.currentLocation.Map.DisplayWidth)
            {
              // ISSUE: explicit reference operation
              // ISSUE: variable of a reference type
              xTile.Dimensions.Rectangle& local = @Game1.viewport;
              int displayWidth = Game1.currentLocation.Map.DisplayWidth;
              viewport = Game1.graphics.GraphicsDevice.Viewport;
              int width = viewport.Width;
              int num = displayWidth - width;
              // ISSUE: explicit reference operation
              (^local).X = num;
            }
            if (Game1.viewport.Y + Game1.viewport.Height > Game1.currentLocation.Map.DisplayHeight)
            {
              // ISSUE: explicit reference operation
              // ISSUE: variable of a reference type
              xTile.Dimensions.Rectangle& local = @Game1.viewport;
              int displayHeight = Game1.currentLocation.Map.DisplayHeight;
              viewport = Game1.graphics.GraphicsDevice.Viewport;
              int height = viewport.Height;
              int num = displayHeight - height;
              // ISSUE: explicit reference operation
              (^local).Y = num;
            }
            if (Game1.viewport.X < 0)
              Game1.viewport.X = 0;
            if (Game1.viewport.Y < 0)
              Game1.viewport.Y = 0;
            if (split.Length > 4 && split[4].Equals("true"))
            {
              Game1.fadeScreenToBlack();
              Game1.fadeToBlackAlpha = 1f;
              Game1.nonWarpFade = true;
            }
          }
          if (split.Length > 4 && split[4].Equals("unfreeze"))
            Game1.viewportFreeze = false;
          if ((int) Game1.gameMode == 2)
            Game1.viewport.X = Game1.currentLocation.Map.DisplayWidth - Game1.viewport.Width;
        }
        this.CurrentCommand = this.CurrentCommand + 1;
      }
    }

    public bool isTileWalkedOn(int x, int y)
    {
      return this.characterWalkLocations.Contains(new Vector2((float) x, (float) y));
    }

    private void populateWalkLocationsList()
    {
      Vector2 tileLocation1 = Game1.player.getTileLocation();
      this.characterWalkLocations.Add(tileLocation1);
      for (int index1 = 2; index1 < this.eventCommands.Length; ++index1)
      {
        string[] strArray = this.eventCommands[index1].Split(' ');
        if (strArray[0] == "move" && strArray[1].Equals("farmer"))
        {
          for (int index2 = 0; index2 < Math.Abs(Convert.ToInt32(strArray[2])); ++index2)
          {
            tileLocation1.X += (float) Math.Sign(Convert.ToInt32(strArray[2]));
            this.characterWalkLocations.Add(tileLocation1);
          }
          for (int index2 = 0; index2 < Math.Abs(Convert.ToInt32(strArray[3])); ++index2)
          {
            tileLocation1.Y += (float) Math.Sign(Convert.ToInt32(strArray[3]));
            this.characterWalkLocations.Add(tileLocation1);
          }
        }
      }
      foreach (NPC actor in this.actors)
      {
        Vector2 tileLocation2 = actor.getTileLocation();
        this.characterWalkLocations.Add(tileLocation2);
        for (int index1 = 2; index1 < this.eventCommands.Length; ++index1)
        {
          string[] strArray = this.eventCommands[index1].Split(' ');
          if (strArray[0] == "move" && strArray[1].Equals(actor.name))
          {
            for (int index2 = 0; index2 < Math.Abs(Convert.ToInt32(strArray[2])); ++index2)
            {
              tileLocation2.X += (float) Math.Sign(Convert.ToInt32(strArray[2]));
              this.characterWalkLocations.Add(tileLocation2);
            }
            for (int index2 = 0; index2 < Math.Abs(Convert.ToInt32(strArray[3])); ++index2)
            {
              tileLocation2.Y += (float) Math.Sign(Convert.ToInt32(strArray[3]));
              this.characterWalkLocations.Add(tileLocation2);
            }
          }
        }
      }
    }

    public NPC getActorByName(string name)
    {
      if (name.Equals("rival"))
        name = Utility.getOtherFarmerNames()[0];
      if (name.Equals("spouse"))
        name = Game1.player.spouse.Replace("engaged", "");
      foreach (NPC actor in this.actors)
      {
        if (actor.name.Equals(name))
          return actor;
      }
      return (NPC) null;
    }

    private void addActor(string name, int x, int y, int facingDirection, GameLocation location)
    {
      Texture2D portrait = (Texture2D) null;
      try
      {
        portrait = Game1.content.Load<Texture2D>("Portraits\\" + (name.Equals("WeddingOutfits") ? Game1.player.spouse : name));
      }
      catch (Exception ex)
      {
      }
      int num = name.Contains("Dwarf") || name.Equals("Krobus") ? Game1.tileSize * 3 / 2 : Game1.tileSize * 2;
      NPC npc = new NPC(new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\" + name), 0, Game1.tileSize / 4, num / 4), new Vector2((float) (x * Game1.tileSize), (float) (y * Game1.tileSize)), location.Name, facingDirection, name.Contains("Rival") ? Utility.getOtherFarmerNames()[0] : name, (Dictionary<int, int[]>) null, portrait, true);
      npc.eventActor = true;
      if (this.isFestival)
      {
        try
        {
          npc.setNewDialogue(this.festivalData[npc.name], false, false);
        }
        catch (Exception ex)
        {
        }
      }
      this.actors.Add(npc);
    }

    public Character getCharacterByName(string name)
    {
      if (name.Equals("rival"))
        name = Utility.getOtherFarmerNames()[0];
      if (name.Contains("farmer"))
        return (Character) Utility.getFarmerFromFarmerNumberString(name);
      foreach (NPC actor in this.actors)
      {
        if (actor.name.Equals(name))
          return (Character) actor;
      }
      return (Character) null;
    }

    public Vector3 getPositionAfterMove(Character c, int xMove, int yMove, int facingDirection)
    {
      Vector2 tileLocation = c.getTileLocation();
      return new Vector3(tileLocation.X + (float) xMove, tileLocation.Y + (float) yMove, (float) facingDirection);
    }

    private void setUpCharacters(string description, GameLocation location)
    {
      Game1.player.Halt();
      Game1.player.positionBeforeEvent = Game1.player.getTileLocation();
      Game1.player.orientationBeforeEvent = Game1.player.facingDirection;
      string[] strArray = description.Split(' ');
      int index = 0;
      while (index < strArray.Length)
      {
        if (strArray[index + 1].Equals("-1") && !strArray[index].Equals("farmer"))
        {
          foreach (NPC character in location.getCharacters())
          {
            if (character.name.Equals(strArray[index]))
              this.actors.Add(character);
          }
        }
        else if (!strArray[index].Equals("farmer"))
        {
          string name = strArray[index];
          if (strArray[index].Equals("spouse"))
            name = Game1.player.spouse.Replace("engaged", "");
          if (strArray[index].Equals("rival"))
            name = Game1.player.isMale ? "maleRival" : "femaleRival";
          if (strArray[index].Equals("cat"))
          {
            this.actors.Add((NPC) new Cat(Convert.ToInt32(strArray[index + 1]), Convert.ToInt32(strArray[index + 2])));
            this.actors.Last<NPC>().name = "Cat";
            this.actors.Last<NPC>().position.X -= (float) (Game1.tileSize / 2);
          }
          else if (strArray[index].Equals("dog"))
          {
            this.actors.Add((NPC) new Dog(Convert.ToInt32(strArray[index + 1]), Convert.ToInt32(strArray[index + 2])));
            this.actors.Last<NPC>().name = "Dog";
            this.actors.Last<NPC>().position.X -= (float) (Game1.tileSize * 2 / 3);
          }
          else if (strArray[index].Equals("Junimo"))
          {
            List<NPC> actors = this.actors;
            Junimo junimo = new Junimo(new Vector2((float) (Convert.ToInt32(strArray[index + 1]) * Game1.tileSize), (float) (Convert.ToInt32(strArray[index + 2]) * Game1.tileSize - Game1.tileSize / 2)), -1, false);
            string str = "Junimo";
            junimo.name = str;
            int num = 1;
            junimo.eventActor = num != 0;
            actors.Add((NPC) junimo);
          }
          else
          {
            int x = Convert.ToInt32(strArray[index + 1]);
            int y = Convert.ToInt32(strArray[index + 2]);
            int facingDirection = Convert.ToInt32(strArray[index + 3]);
            if (location is Farm)
            {
              x = Farm.getFrontDoorPositionForFarmer(Game1.player).X;
              y = Farm.getFrontDoorPositionForFarmer(Game1.player).Y + 2;
              facingDirection = 0;
            }
            this.addActor(name, x, y, facingDirection, location);
          }
        }
        else if (!strArray[index + 1].Equals("-1"))
        {
          Game1.player.position.X = (float) (Convert.ToInt32(strArray[index + 1]) * Game1.tileSize);
          Game1.player.position.Y = (float) (Convert.ToInt32(strArray[index + 2]) * Game1.tileSize + Game1.tileSize / 4);
          Game1.player.faceDirection(Convert.ToInt32(strArray[index + 3]));
          if (location is Farm)
          {
            Game1.player.position.X = (float) (Farm.getFrontDoorPositionForFarmer(Game1.player).X * Game1.tileSize);
            Game1.player.position.Y = (float) ((Farm.getFrontDoorPositionForFarmer(Game1.player).Y + 1) * Game1.tileSize);
            Game1.player.faceDirection(2);
          }
          Game1.player.FarmerSprite.StopAnimation();
          if (this.isFestival)
          {
            foreach (Farmer farmer in Game1.otherFarmers.Values)
            {
              farmer.position.X = (float) (Convert.ToInt32(strArray[index + 1]) * Game1.tileSize);
              farmer.position.Y = (float) (Convert.ToInt32(strArray[index + 2]) * Game1.tileSize + Game1.tileSize / 4);
              int int32 = Convert.ToInt32(strArray[index + 3]);
              farmer.faceDirection(int32);
              farmer.FarmerSprite.StopAnimation();
            }
          }
        }
        index += 4;
      }
    }

    private void beakerSmashEndFunction(int extraInfo)
    {
      Game1.playSound("breakingGlass");
      Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(47, new Vector2(9f, 16f) * (float) Game1.tileSize, Color.LightBlue, 10, false, 100f, 0, -1, -1f, -1, 0));
      Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(400, 3008, Game1.tileSize, Game1.tileSize), 99999f, 2, 0, new Vector2(9f, 16f) * (float) Game1.tileSize, false, false, 0.01f, 0.0f, Color.LightBlue, 1f, 0.0f, 0.0f, 0.0f, false)
      {
        delayBeforeAnimationStart = 700
      });
      Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(46, new Vector2(9f, 16f) * (float) Game1.tileSize, Color.White * 0.75f, 10, false, 100f, 0, -1, -1f, -1, 0)
      {
        motion = new Vector2(0.0f, -1f)
      });
    }

    private void eggSmashEndFunction(int extraInfo)
    {
      Game1.playSound("slimedead");
      Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(47, new Vector2(9f, 16f) * (float) Game1.tileSize, Color.White, 10, false, 100f, 0, -1, -1f, -1, 0));
      Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(177, 99999f, 9999, 0, new Vector2(6f, 5f) * (float) Game1.tileSize, false, false)
      {
        layerDepth = 1E-06f
      });
    }

    private void balloonInSky(int extraInfo)
    {
      TemporaryAnimatedSprite temporarySpriteById1 = Game1.currentLocation.getTemporarySpriteByID(2);
      if (temporarySpriteById1 != null)
        temporarySpriteById1.motion = Vector2.Zero;
      TemporaryAnimatedSprite temporarySpriteById2 = Game1.currentLocation.getTemporarySpriteByID(1);
      if (temporarySpriteById2 == null)
        return;
      temporarySpriteById2.motion = Vector2.Zero;
    }

    private void marcelloBalloonLand(int extraInfo)
    {
      Game1.playSound("thudStep");
      Game1.playSound("dirtyHit");
      TemporaryAnimatedSprite temporarySpriteById1 = Game1.currentLocation.getTemporarySpriteByID(2);
      if (temporarySpriteById1 != null)
        temporarySpriteById1.motion = Vector2.Zero;
      TemporaryAnimatedSprite temporarySpriteById2 = Game1.currentLocation.getTemporarySpriteByID(3);
      if (temporarySpriteById2 != null)
        temporarySpriteById2.scaleChange = 0.0f;
      Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(0, 2944, 64, 64), 120f, 8, 1, new Vector2(25f, 39f) * (float) Game1.tileSize + new Vector2((float) (-Game1.tileSize / 2), (float) (Game1.pixelZoom * 8)), false, true, 1f, 0.0f, Color.White, 1f, 0.0f, 0.0f, 0.0f, false));
      Game1.currentLocation.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(0, 2944, 64, 64), 120f, 8, 1, new Vector2(27f, 39f) * (float) Game1.tileSize + new Vector2(0.0f, (float) (Game1.pixelZoom * 12)), false, false, 1f, 0.0f, Color.White, 1f, 0.0f, 0.0f, 0.0f, false)
      {
        delayBeforeAnimationStart = 300
      });
      this.CurrentCommand = this.CurrentCommand + 1;
    }

    private void samPreOllie(int extraInfo)
    {
      this.getActorByName("Sam").sprite.CurrentFrame = 27;
      Game1.player.faceDirection(0);
      TemporaryAnimatedSprite temporarySpriteById = Game1.currentLocation.getTemporarySpriteByID(1);
      int num = 22 * Game1.tileSize;
      temporarySpriteById.xStopCoordinate = num;
      TemporaryAnimatedSprite.endBehavior endBehavior = new TemporaryAnimatedSprite.endBehavior(this.samOllie);
      temporarySpriteById.reachedStopCoordinate = endBehavior;
      Vector2 vector2 = new Vector2(2f, 0.0f);
      temporarySpriteById.motion = vector2;
    }

    private void samOllie(int extraInfo)
    {
      Game1.playSound("crafting");
      this.getActorByName("Sam").sprite.CurrentFrame = 26;
      TemporaryAnimatedSprite temporarySpriteById = Game1.currentLocation.getTemporarySpriteByID(1);
      int num1 = 0;
      temporarySpriteById.currentNumberOfLoops = num1;
      int num2 = 1;
      temporarySpriteById.totalNumberOfLoops = num2;
      temporarySpriteById.motion.Y = -9f;
      temporarySpriteById.motion.X = 2f;
      Vector2 vector2 = new Vector2(0.0f, 0.4f);
      temporarySpriteById.acceleration = vector2;
      int num3 = 1;
      temporarySpriteById.animationLength = num3;
      double num4 = 530.0;
      temporarySpriteById.interval = (float) num4;
      double num5 = 0.0;
      temporarySpriteById.timer = (float) num5;
      TemporaryAnimatedSprite.endBehavior endBehavior = new TemporaryAnimatedSprite.endBehavior(this.samGrind);
      temporarySpriteById.endFunction = endBehavior;
      int num6 = 0;
      temporarySpriteById.destroyable = num6 != 0;
    }

    private void samGrind(int extraInfo)
    {
      Game1.playSound("hammer");
      this.getActorByName("Sam").sprite.CurrentFrame = 28;
      TemporaryAnimatedSprite temporarySpriteById = Game1.currentLocation.getTemporarySpriteByID(1);
      int num1 = 0;
      temporarySpriteById.currentNumberOfLoops = num1;
      int num2 = 9999;
      temporarySpriteById.totalNumberOfLoops = num2;
      temporarySpriteById.motion.Y = 0.0f;
      temporarySpriteById.motion.X = 2f;
      Vector2 vector2 = new Vector2(0.0f, 0.0f);
      temporarySpriteById.acceleration = vector2;
      int num3 = 1;
      temporarySpriteById.animationLength = num3;
      double num4 = 99999.0;
      temporarySpriteById.interval = (float) num4;
      double num5 = 0.0;
      temporarySpriteById.timer = (float) num5;
      int num6 = 26 * Game1.tileSize;
      temporarySpriteById.xStopCoordinate = num6;
      int num7 = -1;
      temporarySpriteById.yStopCoordinate = num7;
      TemporaryAnimatedSprite.endBehavior endBehavior = new TemporaryAnimatedSprite.endBehavior(this.samDropOff);
      temporarySpriteById.reachedStopCoordinate = endBehavior;
    }

    private void samDropOff(int extraInfo)
    {
      NPC actorByName = this.getActorByName("Sam");
      actorByName.sprite.CurrentFrame = 31;
      TemporaryAnimatedSprite temporarySpriteById = Game1.currentLocation.getTemporarySpriteByID(1);
      int num1 = 9999;
      temporarySpriteById.currentNumberOfLoops = num1;
      int num2 = 0;
      temporarySpriteById.totalNumberOfLoops = num2;
      temporarySpriteById.motion.Y = 0.0f;
      temporarySpriteById.motion.X = 2f;
      Vector2 vector2 = new Vector2(0.0f, 0.4f);
      temporarySpriteById.acceleration = vector2;
      int num3 = 1;
      temporarySpriteById.animationLength = num3;
      double num4 = 99999.0;
      temporarySpriteById.interval = (float) num4;
      int num5 = 90 * Game1.tileSize;
      temporarySpriteById.yStopCoordinate = num5;
      TemporaryAnimatedSprite.endBehavior endBehavior = new TemporaryAnimatedSprite.endBehavior(this.samGround);
      temporarySpriteById.reachedStopCoordinate = endBehavior;
      // ISSUE: variable of the null type
      __Null local = null;
      temporarySpriteById.endFunction = (TemporaryAnimatedSprite.endBehavior) local;
      actorByName.Sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
      {
        new FarmerSprite.AnimationFrame(29, 100),
        new FarmerSprite.AnimationFrame(30, 100),
        new FarmerSprite.AnimationFrame(31, 100),
        new FarmerSprite.AnimationFrame(32, 100)
      });
      actorByName.Sprite.loop = false;
    }

    private void samGround(int extraInfo)
    {
      TemporaryAnimatedSprite temporarySpriteById = Game1.currentLocation.getTemporarySpriteByID(1);
      Game1.playSound("thudStep");
      // ISSUE: variable of the null type
      __Null local1 = null;
      temporarySpriteById.attachedCharacter = (Character) local1;
      // ISSUE: variable of the null type
      __Null local2 = null;
      temporarySpriteById.reachedStopCoordinate = (TemporaryAnimatedSprite.endBehavior) local2;
      int num1 = -1;
      temporarySpriteById.totalNumberOfLoops = num1;
      double num2 = 0.0;
      temporarySpriteById.interval = (float) num2;
      int num3 = 1;
      temporarySpriteById.destroyable = num3 != 0;
      this.CurrentCommand = this.CurrentCommand + 1;
    }

    private void catchFootball(int extraInfo)
    {
      TemporaryAnimatedSprite temporarySpriteById = Game1.currentLocation.getTemporarySpriteByID(1);
      Game1.playSound("fishSlap");
      Vector2 vector2 = new Vector2(2f, -8f);
      temporarySpriteById.motion = vector2;
      double num1 = 0.130899697542191;
      temporarySpriteById.rotationChange = (float) num1;
      TemporaryAnimatedSprite.endBehavior endBehavior = new TemporaryAnimatedSprite.endBehavior(this.footballLand);
      temporarySpriteById.reachedStopCoordinate = endBehavior;
      int num2 = 17 * Game1.tileSize;
      temporarySpriteById.yStopCoordinate = num2;
      Game1.player.jump();
    }

    private void footballLand(int extraInfo)
    {
      TemporaryAnimatedSprite temporarySpriteById = Game1.currentLocation.getTemporarySpriteByID(1);
      Game1.playSound("sandyStep");
      Vector2 vector2 = new Vector2(0.0f, 0.0f);
      temporarySpriteById.motion = vector2;
      double num1 = 0.0;
      temporarySpriteById.rotationChange = (float) num1;
      // ISSUE: variable of the null type
      __Null local = null;
      temporarySpriteById.reachedStopCoordinate = (TemporaryAnimatedSprite.endBehavior) local;
      int num2 = 1;
      temporarySpriteById.animationLength = num2;
      double num3 = 999999.0;
      temporarySpriteById.interval = (float) num3;
      this.CurrentCommand = this.CurrentCommand + 1;
    }

    private void parrotSplat(int extraInfo)
    {
      Game1.playSound("drumkit0");
      DelayedAction.playSoundAfterDelay("drumkit5", 100);
      Game1.playSound("slimeHit");
      foreach (TemporaryAnimatedSprite aboveMapSprite in this.aboveMapSprites)
        aboveMapSprite.alpha = 0.0f;
      Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(174, 168, 4, 11), 99999f, 1, 99999, new Vector2((float) (24 * Game1.tileSize - Game1.tileSize / 2), (float) (87 * Game1.tileSize)), false, false, 0.02f, 0.01f, Color.White, (float) Game1.pixelZoom, 0.0f, 1.570796f, (float) Math.PI / 64f, false)
      {
        motion = new Vector2(2f, -2f),
        acceleration = new Vector2(0.0f, 0.1f)
      });
      Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(174, 168, 4, 11), 99999f, 1, 99999, new Vector2((float) (24 * Game1.tileSize - Game1.tileSize / 2), (float) (87 * Game1.tileSize)), false, false, 0.02f, 0.01f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.7853982f, (float) Math.PI / 64f, false)
      {
        motion = new Vector2(-2f, -1f),
        acceleration = new Vector2(0.0f, 0.1f)
      });
      Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(174, 168, 4, 11), 99999f, 1, 99999, new Vector2((float) (24 * Game1.tileSize - Game1.tileSize / 2), (float) (87 * Game1.tileSize)), false, false, 0.02f, 0.01f, Color.White, (float) Game1.pixelZoom, 0.0f, 3.141593f, (float) Math.PI / 64f, false)
      {
        motion = new Vector2(1f, 1f),
        acceleration = new Vector2(0.0f, 0.1f)
      });
      Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(174, 168, 4, 11), 99999f, 1, 99999, new Vector2((float) (24 * Game1.tileSize - Game1.tileSize / 2), (float) (87 * Game1.tileSize)), false, false, 0.02f, 0.01f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, (float) Math.PI / 64f, false)
      {
        motion = new Vector2(-2f, -2f),
        acceleration = new Vector2(0.0f, 0.1f)
      });
      Game1.currentLocation.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(148, 165, 25, 23), 99999f, 1, 99999, new Vector2((float) (24 * Game1.tileSize - Game1.tileSize / 2), (float) (87 * Game1.tileSize)), false, false, 0.01f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
      {
        id = 666f
      });
      this.CurrentCommand = this.CurrentCommand + 1;
    }

    private void addSpecificTemporarySprite(string key, GameLocation location, string[] split)
    {
      // ISSUE: reference to a compiler-generated method
      uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(key);
      if (stringHash <= 2358341695U)
      {
        if (stringHash <= 1042337593U)
        {
          if (stringHash <= 390240131U)
          {
            if (stringHash <= 246031843U)
            {
              if (stringHash <= 84876044U)
              {
                if ((int) stringHash != 37764568)
                {
                  if ((int) stringHash != 84876044 || !(key == "pennyFieldTrip"))
                    return;
                  location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(0, 1813, 86, 54), 999999f, 1, 0, new Vector2(68f, 44f) * (float) Game1.tileSize, false, false, 0.0001f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false));
                }
                else
                {
                  if (!(key == "JoshMom"))
                    return;
                  TemporaryAnimatedSprite temporaryAnimatedSprite1 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(416, 1931, 58, 65), 750f, 2, 99999, new Vector2((float) (Game1.viewport.Width / 2), (float) Game1.viewport.Height), false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                  temporaryAnimatedSprite1.alpha = 0.6f;
                  temporaryAnimatedSprite1.local = true;
                  temporaryAnimatedSprite1.xPeriodic = true;
                  temporaryAnimatedSprite1.xPeriodicLoopTime = 2000f;
                  double num1 = (double) (Game1.tileSize / 2);
                  temporaryAnimatedSprite1.xPeriodicRange = (float) num1;
                  Vector2 vector2_1 = new Vector2(0.0f, -1.25f);
                  temporaryAnimatedSprite1.motion = vector2_1;
                  Vector2 vector2_2 = new Vector2((float) (Game1.viewport.Width / 2), (float) Game1.viewport.Height);
                  temporaryAnimatedSprite1.initialPosition = vector2_2;
                  TemporaryAnimatedSprite temporaryAnimatedSprite2 = temporaryAnimatedSprite1;
                  location.temporarySprites.Add(temporaryAnimatedSprite2);
                  for (int index = 0; index < 19; ++index)
                  {
                    List<TemporaryAnimatedSprite> temporarySprites = location.temporarySprites;
                    TemporaryAnimatedSprite temporaryAnimatedSprite3 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(516, 1916, 7, 10), 99999f, 1, 99999, new Vector2((float) Game1.tileSize, (float) (Game1.tileSize / 2)), false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                    temporaryAnimatedSprite3.alphaFade = 0.01f;
                    temporaryAnimatedSprite3.local = true;
                    Vector2 vector2_3 = new Vector2(-1f, -1f);
                    temporaryAnimatedSprite3.motion = vector2_3;
                    TemporaryAnimatedSprite temporaryAnimatedSprite4 = temporaryAnimatedSprite2;
                    temporaryAnimatedSprite3.parentSprite = temporaryAnimatedSprite4;
                    int num2 = (index + 1) * 1000;
                    temporaryAnimatedSprite3.delayBeforeAnimationStart = num2;
                    temporarySprites.Add(temporaryAnimatedSprite3);
                  }
                }
              }
              else if ((int) stringHash != 172887865)
              {
                if ((int) stringHash != 199811880)
                {
                  if ((int) stringHash != 246031843 || !(key == "heart"))
                    return;
                  location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(211, 428, 7, 6), 2000f, 1, 0, new Vector2((float) Convert.ToInt32(split[2]), (float) Convert.ToInt32(split[3])) * (float) Game1.tileSize + new Vector2((float) (-Game1.tileSize / 4), (float) (-Game1.pixelZoom * 4)), false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
                  {
                    motion = new Vector2(0.0f, -0.5f),
                    alphaFade = 0.01f
                  });
                }
                else
                {
                  if (!(key == "shaneCliffProps"))
                    return;
                  location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(549, 1891, 19, 12), 99999f, 1, 99999, new Vector2(104f, 96f) * (float) Game1.tileSize, false, false, 0.01f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
                  {
                    id = 999f
                  });
                }
              }
              else
              {
                if (!(key == "dropEgg"))
                  return;
                List<TemporaryAnimatedSprite> temporarySprites = location.TemporarySprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(176, 800f, 1, 0, new Vector2(6f, 4f) * (float) Game1.tileSize + new Vector2(0.0f, (float) (Game1.tileSize / 2)), false, false);
                temporaryAnimatedSprite.rotationChange = 0.1308997f;
                Vector2 vector2_1 = new Vector2(0.0f, -7f);
                temporaryAnimatedSprite.motion = vector2_1;
                Vector2 vector2_2 = new Vector2(0.0f, 0.3f);
                temporaryAnimatedSprite.acceleration = vector2_2;
                TemporaryAnimatedSprite.endBehavior endBehavior = new TemporaryAnimatedSprite.endBehavior(this.eggSmashEndFunction);
                temporaryAnimatedSprite.endFunction = endBehavior;
                double num = 1.0;
                temporaryAnimatedSprite.layerDepth = (float) num;
                temporarySprites.Add(temporaryAnimatedSprite);
              }
            }
            else if (stringHash <= 299483063U)
            {
              if ((int) stringHash != 262838603)
              {
                if ((int) stringHash != 264360623)
                {
                  if ((int) stringHash != 299483063 || !(key == "wed"))
                    return;
                  this.aboveMapSprites = new List<TemporaryAnimatedSprite>();
                  Game1.flashAlpha = 1f;
                  for (int index = 0; index < 150; ++index)
                  {
                    Vector2 position = new Vector2((float) Game1.random.Next(Game1.viewport.Width - Game1.tileSize * 2), (float) Game1.random.Next(Game1.viewport.Height));
                    int num = Game1.random.Next(2, 5);
                    List<TemporaryAnimatedSprite> aboveMapSprites = this.aboveMapSprites;
                    TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(424, 1266, 8, 8), 60f + (float) Game1.random.Next(-10, 10), 7, 999999, position, false, false, 0.99f, 0.0f, Color.White, (float) num, 0.0f, 0.0f, 0.0f, false);
                    temporaryAnimatedSprite.local = true;
                    Vector2 vector2 = new Vector2(0.1625f, -0.25f) * (float) num;
                    temporaryAnimatedSprite.motion = vector2;
                    aboveMapSprites.Add(temporaryAnimatedSprite);
                  }
                  location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(558, 1425, 20, 26), 400f, 3, 99999, new Vector2(26f, 64f) * (float) Game1.tileSize, false, false, (float) (65 * Game1.tileSize) / 10000f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
                  {
                    pingPong = true
                  });
                  Game1.changeMusicTrack("wedding");
                  Game1.musicPlayerVolume = 0.0f;
                }
                else
                {
                  if (!(key == "haleyRoomDark"))
                    return;
                  Game1.currentLightSources.Clear();
                  Game1.ambientLight = new Color(200, 200, 100);
                  List<TemporaryAnimatedSprite> temporarySprites = location.TemporarySprites;
                  TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(743, 999999f, 1, 0, new Vector2(4f, 1f) * (float) Game1.tileSize, false, false);
                  temporaryAnimatedSprite.light = true;
                  Color color = new Color(0, (int) byte.MaxValue, (int) byte.MaxValue);
                  temporaryAnimatedSprite.lightcolor = color;
                  double num = 2.0;
                  temporaryAnimatedSprite.lightRadius = (float) num;
                  temporarySprites.Add(temporaryAnimatedSprite);
                }
              }
              else
              {
                if (!(key == "EmilyBoomBox"))
                  return;
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(586, 1871, 24, 14), 99999f, 1, 99999, new Vector2(15f, 4f) * (float) Game1.tileSize, false, false, 0.01f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
                {
                  id = 999f
                });
              }
            }
            else if ((int) stringHash != 321043561)
            {
              if ((int) stringHash != 354301824)
              {
                if ((int) stringHash != 390240131 || !(key == "shaneCliffs"))
                  return;
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(533, 1864, 19, 27), 99999f, 1, 99999, new Vector2(83f, 98f) * (float) Game1.tileSize, false, false, 0.01f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
                {
                  id = 999f
                });
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(552, 1862, 31, 21), 99999f, 1, 99999, new Vector2(83f, 98f) * (float) Game1.tileSize + new Vector2((float) (-Game1.tileSize / 4), 0.0f), false, false, 0.0001f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false));
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(549, 1891, 19, 12), 99999f, 1, 99999, new Vector2(84f, 99f) * (float) Game1.tileSize, false, false, 0.01f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
                {
                  id = 999f
                });
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(549, 1891, 19, 12), 99999f, 1, 99999, new Vector2(82f, 98f) * (float) Game1.tileSize, false, false, 0.01f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
                {
                  id = 999f
                });
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(542, 1893, 4, 6), 99999f, 1, 99999, new Vector2(83f, 99f) * (float) Game1.tileSize + new Vector2(-8f, 4f) * (float) Game1.pixelZoom, false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false));
              }
              else
              {
                if (!(key == "sebastianGarage"))
                  return;
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(276, 1843, 48, 42), 9999f, 1, 999, new Vector2(17f, 23f) * (float) Game1.tileSize + new Vector2(0.0f, (float) (Game1.pixelZoom * 2)), false, false, (float) (23 * Game1.tileSize) / 10000f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false));
                this.getActorByName("Sebastian").hideShadow = true;
              }
            }
            else
            {
              if (!(key == "sebastianOnBike"))
                return;
              location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(0, 1600, 64, 128), 80f, 8, 9999, new Vector2(19f, 27f) * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), (float) (-Game1.pixelZoom * 4)), false, true, (float) (28 * Game1.tileSize) / 10000f, 0.0f, Color.White, 1f, 0.0f, 0.0f, 0.0f, false));
              location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(405, 1854, 47, 33), 9999f, 1, 999, new Vector2(17f, 27f) * (float) Game1.tileSize + new Vector2(0.0f, (float) (-Game1.pixelZoom * 2)), false, false, (float) (28 * Game1.tileSize) / 10000f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false));
            }
          }
          else if (stringHash <= 750296570U)
          {
            if (stringHash <= 428895204U)
            {
              if ((int) stringHash != 416176087)
              {
                if ((int) stringHash != 428895204 || !(key == "balloonBirds"))
                  return;
                int num = 0;
                if (split != null && split.Length > 2)
                  num = Convert.ToInt32(split[2]);
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(388, 1894, 24, 22), 100f, 6, 9999, new Vector2(48f, (float) (num + 12)) * (float) Game1.tileSize, false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
                {
                  motion = new Vector2(-3f, 0.0f),
                  delayBeforeAnimationStart = 1500
                });
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(388, 1894, 24, 22), 100f, 6, 9999, new Vector2(47f, (float) (num + 13)) * (float) Game1.tileSize, false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
                {
                  motion = new Vector2(-3f, 0.0f),
                  delayBeforeAnimationStart = 1250
                });
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(388, 1894, 24, 22), 100f, 6, 9999, new Vector2(46f, (float) (num + 14)) * (float) Game1.tileSize, false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
                {
                  motion = new Vector2(-3f, 0.0f),
                  delayBeforeAnimationStart = 1100
                });
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(388, 1894, 24, 22), 100f, 6, 9999, new Vector2(45f, (float) (num + 15)) * (float) Game1.tileSize, false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
                {
                  motion = new Vector2(-3f, 0.0f),
                  delayBeforeAnimationStart = 1000
                });
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(388, 1894, 24, 22), 100f, 6, 9999, new Vector2(46f, (float) (num + 16)) * (float) Game1.tileSize, false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
                {
                  motion = new Vector2(-3f, 0.0f),
                  delayBeforeAnimationStart = 1080
                });
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(388, 1894, 24, 22), 100f, 6, 9999, new Vector2(47f, (float) (num + 17)) * (float) Game1.tileSize, false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
                {
                  motion = new Vector2(-3f, 0.0f),
                  delayBeforeAnimationStart = 1300
                });
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(388, 1894, 24, 22), 100f, 6, 9999, new Vector2(48f, (float) (num + 18)) * (float) Game1.tileSize, false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
                {
                  motion = new Vector2(-3f, 0.0f),
                  delayBeforeAnimationStart = 1450
                });
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(388, 1894, 24, 22), 100f, 6, 9999, new Vector2(46f, (float) (num + 15)) * (float) Game1.tileSize, false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
                {
                  motion = new Vector2(-4f, 0.0f),
                  delayBeforeAnimationStart = 5450
                });
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(388, 1894, 24, 22), 100f, 6, 9999, new Vector2(48f, (float) (num + 10)) * (float) Game1.tileSize, false, false, 0.0f, 0.0f, Color.White, (float) (Game1.pixelZoom / 2), 0.0f, 0.0f, 0.0f, false)
                {
                  motion = new Vector2(-2f, 0.0f),
                  delayBeforeAnimationStart = 500
                });
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(388, 1894, 24, 22), 100f, 6, 9999, new Vector2(47f, (float) (num + 11)) * (float) Game1.tileSize, false, false, 0.0f, 0.0f, Color.White, (float) (Game1.pixelZoom / 2), 0.0f, 0.0f, 0.0f, false)
                {
                  motion = new Vector2(-2f, 0.0f),
                  delayBeforeAnimationStart = 250
                });
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(388, 1894, 24, 22), 100f, 6, 9999, new Vector2(46f, (float) (num + 12)) * (float) Game1.tileSize, false, false, 0.0f, 0.0f, Color.White, (float) (Game1.pixelZoom / 2), 0.0f, 0.0f, 0.0f, false)
                {
                  motion = new Vector2(-2f, 0.0f),
                  delayBeforeAnimationStart = 100
                });
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(388, 1894, 24, 22), 100f, 6, 9999, new Vector2(45f, (float) (num + 13)) * (float) Game1.tileSize, false, false, 0.0f, 0.0f, Color.White, (float) (Game1.pixelZoom / 2), 0.0f, 0.0f, 0.0f, false)
                {
                  motion = new Vector2(-2f, 0.0f)
                });
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(388, 1894, 24, 22), 100f, 6, 9999, new Vector2(46f, (float) (num + 14)) * (float) Game1.tileSize, false, false, 0.0f, 0.0f, Color.White, (float) (Game1.pixelZoom / 2), 0.0f, 0.0f, 0.0f, false)
                {
                  motion = new Vector2(-2f, 0.0f),
                  delayBeforeAnimationStart = 80
                });
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(388, 1894, 24, 22), 100f, 6, 9999, new Vector2(47f, (float) (num + 15)) * (float) Game1.tileSize, false, false, 0.0f, 0.0f, Color.White, (float) (Game1.pixelZoom / 2), 0.0f, 0.0f, 0.0f, false)
                {
                  motion = new Vector2(-2f, 0.0f),
                  delayBeforeAnimationStart = 300
                });
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(388, 1894, 24, 22), 100f, 6, 9999, new Vector2(48f, (float) (num + 16)) * (float) Game1.tileSize, false, false, 0.0f, 0.0f, Color.White, (float) (Game1.pixelZoom / 2), 0.0f, 0.0f, 0.0f, false)
                {
                  motion = new Vector2(-2f, 0.0f),
                  delayBeforeAnimationStart = 450
                });
              }
              else
              {
                if (!(key == "abbyOuija"))
                  return;
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(0, 960, Game1.tileSize * 2, Game1.tileSize * 2), 60f, 4, 0, new Vector2(6f, 9f) * (float) Game1.tileSize, false, false, 1f, 0.0f, Color.White, 1f, 0.0f, 0.0f, 0.0f, false));
              }
            }
            else if ((int) stringHash != 477416675)
            {
              if ((int) stringHash != 655907427)
              {
                if ((int) stringHash != 750296570 || !(key == "dickBag"))
                  return;
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(528, 1435, 16, 16), 99999f, 1, 99999, new Vector2(48f, 7f) * (float) Game1.tileSize, false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false));
              }
              else
              {
                if (!(key == "jojaCeremony"))
                  return;
                this.aboveMapSprites = new List<TemporaryAnimatedSprite>();
                for (int index = 0; index < 16; ++index)
                {
                  Vector2 position = new Vector2((float) Game1.random.Next(Game1.viewport.Width - Game1.tileSize * 2), (float) (Game1.viewport.Height + index * Game1.tileSize));
                  List<TemporaryAnimatedSprite> aboveMapSprites1 = this.aboveMapSprites;
                  TemporaryAnimatedSprite temporaryAnimatedSprite1 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(534, 1413, 11, 16), 99999f, 1, 99999, position, false, false, 0.99f, 0.0f, Color.DeepSkyBlue, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                  temporaryAnimatedSprite1.local = true;
                  Vector2 vector2_1 = new Vector2(0.25f, -1.5f);
                  temporaryAnimatedSprite1.motion = vector2_1;
                  Vector2 vector2_2 = new Vector2(0.0f, -1f / 1000f);
                  temporaryAnimatedSprite1.acceleration = vector2_2;
                  aboveMapSprites1.Add(temporaryAnimatedSprite1);
                  List<TemporaryAnimatedSprite> aboveMapSprites2 = this.aboveMapSprites;
                  TemporaryAnimatedSprite temporaryAnimatedSprite2 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(545, 1413, 11, 34), 99999f, 1, 99999, position + new Vector2(0.0f, 0.0f), false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                  temporaryAnimatedSprite2.local = true;
                  Vector2 vector2_3 = new Vector2(0.25f, -1.5f);
                  temporaryAnimatedSprite2.motion = vector2_3;
                  Vector2 vector2_4 = new Vector2(0.0f, -1f / 1000f);
                  temporaryAnimatedSprite2.acceleration = vector2_4;
                  aboveMapSprites2.Add(temporaryAnimatedSprite2);
                }
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(0, 1363, 114, 58), 99999f, 1, 99999, new Vector2(50f, 20f) * (float) Game1.tileSize, false, false, (float) (23 * Game1.tileSize) / 10000f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false));
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(595, 1387, 14, 34), 200f, 3, 99999, new Vector2(48f, 20f) * (float) Game1.tileSize, false, false, (float) ((double) (23 * Game1.tileSize) / 10000.0 + 0.00999999977648258), 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
                {
                  pingPong = true
                });
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(595, 1387, 14, 34), 200f, 3, 99999, new Vector2(49f, 20f) * (float) Game1.tileSize, false, false, (float) ((double) (23 * Game1.tileSize) / 10000.0 + 0.00999999977648258), 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
                {
                  pingPong = true
                });
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(595, 1387, 14, 34), 210f, 3, 99999, new Vector2(62f, 20f) * (float) Game1.tileSize, false, false, (float) ((double) (23 * Game1.tileSize) / 10000.0 + 0.00999999977648258), 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
                {
                  pingPong = true
                });
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(595, 1387, 14, 34), 190f, 3, 99999, new Vector2(60f, 20f) * (float) Game1.tileSize, false, false, (float) ((double) (23 * Game1.tileSize) / 10000.0 + 0.00999999977648258), 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
                {
                  pingPong = true
                });
              }
            }
            else
            {
              if (!(key == "EmilyBoomBoxStop"))
                return;
              location.getTemporarySpriteByID(999).pulse = false;
              location.getTemporarySpriteByID(999).scale = (float) Game1.pixelZoom;
            }
          }
          else if (stringHash <= 820334071U)
          {
            if ((int) stringHash != 762742231)
            {
              if ((int) stringHash != 789298023)
              {
                if ((int) stringHash != 820334071 || !(key == "leahTree"))
                  return;
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(744, 999999f, 1, 0, new Vector2(42f, 8f) * (float) Game1.tileSize, false, false));
              }
              else
              {
                if (!(key == "leahLaptop"))
                  return;
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(130, 1849, 19, 19), 9999f, 1, 999, new Vector2(12f, 10f) * (float) Game1.tileSize + new Vector2(0.0f, (float) (Game1.tileSize / 4 + Game1.pixelZoom * 2)), false, false, (float) (29 * Game1.tileSize) / 10000f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false));
              }
            }
            else
            {
              if (!(key == "curtainClose"))
                return;
              location.getTemporarySpriteByID(999).sourceRect.X = 644;
              Game1.playSound("shwip");
            }
          }
          else if ((int) stringHash != 888455659)
          {
            if ((int) stringHash != 1006429515)
            {
              if ((int) stringHash != 1042337593 || !(key == "iceFishingCatch"))
                return;
              location.TemporarySprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(160, 368, 16, 32), 500f, 3, 99999, new Vector2(68f, 30f) * (float) Game1.tileSize, false, false, (float) (31 * Game1.tileSize) / 10000f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false));
              location.TemporarySprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(160, 368, 16, 32), 510f, 3, 99999, new Vector2(74f, 30f) * (float) Game1.tileSize, false, false, (float) (31 * Game1.tileSize) / 10000f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false));
              location.TemporarySprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(160, 368, 16, 32), 490f, 3, 99999, new Vector2(67f, 36f) * (float) Game1.tileSize, false, false, (float) (37 * Game1.tileSize) / 10000f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false));
              location.TemporarySprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(160, 368, 16, 32), 500f, 3, 99999, new Vector2(76f, 35f) * (float) Game1.tileSize, false, false, (float) (36 * Game1.tileSize) / 10000f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false));
            }
            else
            {
              if (!(key == "sebastianRide"))
                return;
              location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(405, 1843, 14, 9), 40f, 4, 999, new Vector2(19f, 8f) * (float) Game1.tileSize + new Vector2(0.0f, (float) (Game1.pixelZoom * 7)), false, false, (float) (28 * Game1.tileSize) / 10000f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
              {
                motion = new Vector2(-2f, 0.0f)
              });
            }
          }
          else
          {
            if (!(key == "WizardPromise"))
              return;
            Utility.addSprinklesToLocation(location, 16, 15, 9, 9, 2000, 50, Color.White, (string) null, false);
          }
        }
        else if (stringHash <= 1984971571U)
        {
          if (stringHash <= 1382680148U)
          {
            if (stringHash <= 1139299912U)
            {
              if ((int) stringHash != 1057059047)
              {
                if ((int) stringHash != 1139299912 || !(key == "moonlightJellies"))
                  return;
                if (this.npcControllers != null)
                  this.npcControllers.Clear();
                this.underwaterSprites = new List<TemporaryAnimatedSprite>();
                List<TemporaryAnimatedSprite> underwaterSprites1 = this.underwaterSprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite1 = new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(26f, 49f) * (float) Game1.tileSize, false, false, 0.1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                temporaryAnimatedSprite1.motion = new Vector2(0.0f, -1f);
                int num1 = 1;
                temporaryAnimatedSprite1.xPeriodic = num1 != 0;
                double num2 = 3000.0;
                temporaryAnimatedSprite1.xPeriodicLoopTime = (float) num2;
                double num3 = (double) (Game1.tileSize / 4);
                temporaryAnimatedSprite1.xPeriodicRange = (float) num3;
                int num4 = 1;
                temporaryAnimatedSprite1.light = num4 != 0;
                Color black1 = Color.Black;
                temporaryAnimatedSprite1.lightcolor = black1;
                double num5 = 1.0;
                temporaryAnimatedSprite1.lightRadius = (float) num5;
                int num6 = 40 * Game1.tileSize;
                temporaryAnimatedSprite1.yStopCoordinate = num6;
                int num7 = 10000;
                temporaryAnimatedSprite1.delayBeforeAnimationStart = num7;
                int num8 = 1;
                temporaryAnimatedSprite1.pingPong = num8 != 0;
                underwaterSprites1.Add(temporaryAnimatedSprite1);
                List<TemporaryAnimatedSprite> underwaterSprites2 = this.underwaterSprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite2 = new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(29f, 49f) * (float) Game1.tileSize, false, false, 0.1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                temporaryAnimatedSprite2.motion = new Vector2(0.0f, -1f);
                int num9 = 1;
                temporaryAnimatedSprite2.xPeriodic = num9 != 0;
                double num10 = 3000.0;
                temporaryAnimatedSprite2.xPeriodicLoopTime = (float) num10;
                double num11 = (double) (Game1.tileSize / 4);
                temporaryAnimatedSprite2.xPeriodicRange = (float) num11;
                int num12 = 1;
                temporaryAnimatedSprite2.light = num12 != 0;
                Color black2 = Color.Black;
                temporaryAnimatedSprite2.lightcolor = black2;
                double num13 = 1.0;
                temporaryAnimatedSprite2.lightRadius = (float) num13;
                int num14 = 40 * Game1.tileSize;
                temporaryAnimatedSprite2.yStopCoordinate = num14;
                int num15 = 1;
                temporaryAnimatedSprite2.pingPong = num15 != 0;
                underwaterSprites2.Add(temporaryAnimatedSprite2);
                List<TemporaryAnimatedSprite> underwaterSprites3 = this.underwaterSprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite3 = new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(31f, 49f) * (float) Game1.tileSize, false, false, 0.1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                temporaryAnimatedSprite3.motion = new Vector2(0.0f, -1f);
                int num16 = 1;
                temporaryAnimatedSprite3.xPeriodic = num16 != 0;
                double num17 = 3000.0;
                temporaryAnimatedSprite3.xPeriodicLoopTime = (float) num17;
                double num18 = (double) (Game1.tileSize / 4);
                temporaryAnimatedSprite3.xPeriodicRange = (float) num18;
                int num19 = 1;
                temporaryAnimatedSprite3.light = num19 != 0;
                Color black3 = Color.Black;
                temporaryAnimatedSprite3.lightcolor = black3;
                double num20 = 1.0;
                temporaryAnimatedSprite3.lightRadius = (float) num20;
                int num21 = 41 * Game1.tileSize;
                temporaryAnimatedSprite3.yStopCoordinate = num21;
                int num22 = 12000;
                temporaryAnimatedSprite3.delayBeforeAnimationStart = num22;
                int num23 = 1;
                temporaryAnimatedSprite3.pingPong = num23 != 0;
                underwaterSprites3.Add(temporaryAnimatedSprite3);
                List<TemporaryAnimatedSprite> underwaterSprites4 = this.underwaterSprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite4 = new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(20f, 49f) * (float) Game1.tileSize, false, false, 0.1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                temporaryAnimatedSprite4.motion = new Vector2(0.0f, -1f);
                int num24 = 1;
                temporaryAnimatedSprite4.xPeriodic = num24 != 0;
                double num25 = 3000.0;
                temporaryAnimatedSprite4.xPeriodicLoopTime = (float) num25;
                double num26 = (double) (Game1.tileSize / 4);
                temporaryAnimatedSprite4.xPeriodicRange = (float) num26;
                int num27 = 1;
                temporaryAnimatedSprite4.light = num27 != 0;
                Color black4 = Color.Black;
                temporaryAnimatedSprite4.lightcolor = black4;
                double num28 = 1.0;
                temporaryAnimatedSprite4.lightRadius = (float) num28;
                int num29 = 27 * Game1.tileSize;
                temporaryAnimatedSprite4.yStopCoordinate = num29;
                int num30 = 14000;
                temporaryAnimatedSprite4.delayBeforeAnimationStart = num30;
                int num31 = 1;
                temporaryAnimatedSprite4.pingPong = num31 != 0;
                underwaterSprites4.Add(temporaryAnimatedSprite4);
                List<TemporaryAnimatedSprite> underwaterSprites5 = this.underwaterSprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite5 = new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(17f, 49f) * (float) Game1.tileSize, false, false, 0.1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                temporaryAnimatedSprite5.motion = new Vector2(0.0f, -1f);
                int num32 = 1;
                temporaryAnimatedSprite5.xPeriodic = num32 != 0;
                double num33 = 3000.0;
                temporaryAnimatedSprite5.xPeriodicLoopTime = (float) num33;
                double num34 = (double) (Game1.tileSize / 4);
                temporaryAnimatedSprite5.xPeriodicRange = (float) num34;
                int num35 = 1;
                temporaryAnimatedSprite5.light = num35 != 0;
                Color black5 = Color.Black;
                temporaryAnimatedSprite5.lightcolor = black5;
                double num36 = 1.0;
                temporaryAnimatedSprite5.lightRadius = (float) num36;
                int num37 = 29 * Game1.tileSize;
                temporaryAnimatedSprite5.yStopCoordinate = num37;
                int num38 = 19500;
                temporaryAnimatedSprite5.delayBeforeAnimationStart = num38;
                int num39 = 1;
                temporaryAnimatedSprite5.pingPong = num39 != 0;
                underwaterSprites5.Add(temporaryAnimatedSprite5);
                List<TemporaryAnimatedSprite> underwaterSprites6 = this.underwaterSprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite6 = new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(16f, 49f) * (float) Game1.tileSize, false, false, 0.1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                temporaryAnimatedSprite6.motion = new Vector2(0.0f, -1f);
                int num40 = 1;
                temporaryAnimatedSprite6.xPeriodic = num40 != 0;
                double num41 = 3000.0;
                temporaryAnimatedSprite6.xPeriodicLoopTime = (float) num41;
                double num42 = (double) (Game1.tileSize / 4);
                temporaryAnimatedSprite6.xPeriodicRange = (float) num42;
                int num43 = 1;
                temporaryAnimatedSprite6.light = num43 != 0;
                Color black6 = Color.Black;
                temporaryAnimatedSprite6.lightcolor = black6;
                double num44 = 1.0;
                temporaryAnimatedSprite6.lightRadius = (float) num44;
                int num45 = 32 * Game1.tileSize;
                temporaryAnimatedSprite6.yStopCoordinate = num45;
                int num46 = 20300;
                temporaryAnimatedSprite6.delayBeforeAnimationStart = num46;
                int num47 = 1;
                temporaryAnimatedSprite6.pingPong = num47 != 0;
                underwaterSprites6.Add(temporaryAnimatedSprite6);
                List<TemporaryAnimatedSprite> underwaterSprites7 = this.underwaterSprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite7 = new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(17f, 49f) * (float) Game1.tileSize, false, false, 0.1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                temporaryAnimatedSprite7.motion = new Vector2(0.0f, -1f);
                int num48 = 1;
                temporaryAnimatedSprite7.xPeriodic = num48 != 0;
                double num49 = 3000.0;
                temporaryAnimatedSprite7.xPeriodicLoopTime = (float) num49;
                double num50 = (double) (Game1.tileSize / 4);
                temporaryAnimatedSprite7.xPeriodicRange = (float) num50;
                int num51 = 1;
                temporaryAnimatedSprite7.light = num51 != 0;
                Color black7 = Color.Black;
                temporaryAnimatedSprite7.lightcolor = black7;
                double num52 = 1.0;
                temporaryAnimatedSprite7.lightRadius = (float) num52;
                int num53 = 39 * Game1.tileSize;
                temporaryAnimatedSprite7.yStopCoordinate = num53;
                int num54 = 21500;
                temporaryAnimatedSprite7.delayBeforeAnimationStart = num54;
                int num55 = 1;
                temporaryAnimatedSprite7.pingPong = num55 != 0;
                underwaterSprites7.Add(temporaryAnimatedSprite7);
                List<TemporaryAnimatedSprite> underwaterSprites8 = this.underwaterSprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite8 = new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(16f, 49f) * (float) Game1.tileSize, false, false, 0.1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                temporaryAnimatedSprite8.motion = new Vector2(0.0f, -1f);
                int num56 = 1;
                temporaryAnimatedSprite8.xPeriodic = num56 != 0;
                double num57 = 3000.0;
                temporaryAnimatedSprite8.xPeriodicLoopTime = (float) num57;
                double num58 = (double) (Game1.tileSize / 4);
                temporaryAnimatedSprite8.xPeriodicRange = (float) num58;
                int num59 = 1;
                temporaryAnimatedSprite8.light = num59 != 0;
                Color black8 = Color.Black;
                temporaryAnimatedSprite8.lightcolor = black8;
                double num60 = 1.0;
                temporaryAnimatedSprite8.lightRadius = (float) num60;
                int num61 = 44 * Game1.tileSize;
                temporaryAnimatedSprite8.yStopCoordinate = num61;
                int num62 = 22400;
                temporaryAnimatedSprite8.delayBeforeAnimationStart = num62;
                int num63 = 1;
                temporaryAnimatedSprite8.pingPong = num63 != 0;
                underwaterSprites8.Add(temporaryAnimatedSprite8);
                List<TemporaryAnimatedSprite> underwaterSprites9 = this.underwaterSprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite9 = new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(12f, 49f) * (float) Game1.tileSize, false, false, 0.1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                temporaryAnimatedSprite9.motion = new Vector2(0.0f, -1f);
                int num64 = 1;
                temporaryAnimatedSprite9.xPeriodic = num64 != 0;
                double num65 = 3000.0;
                temporaryAnimatedSprite9.xPeriodicLoopTime = (float) num65;
                double num66 = (double) (Game1.tileSize / 4);
                temporaryAnimatedSprite9.xPeriodicRange = (float) num66;
                int num67 = 1;
                temporaryAnimatedSprite9.light = num67 != 0;
                Color black9 = Color.Black;
                temporaryAnimatedSprite9.lightcolor = black9;
                double num68 = 1.0;
                temporaryAnimatedSprite9.lightRadius = (float) num68;
                int num69 = 42 * Game1.tileSize;
                temporaryAnimatedSprite9.yStopCoordinate = num69;
                int num70 = 23200;
                temporaryAnimatedSprite9.delayBeforeAnimationStart = num70;
                int num71 = 1;
                temporaryAnimatedSprite9.pingPong = num71 != 0;
                underwaterSprites9.Add(temporaryAnimatedSprite9);
                List<TemporaryAnimatedSprite> underwaterSprites10 = this.underwaterSprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite10 = new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(9f, 49f) * (float) Game1.tileSize, false, false, 0.1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                temporaryAnimatedSprite10.motion = new Vector2(0.0f, -1f);
                int num72 = 1;
                temporaryAnimatedSprite10.xPeriodic = num72 != 0;
                double num73 = 3000.0;
                temporaryAnimatedSprite10.xPeriodicLoopTime = (float) num73;
                double num74 = (double) (Game1.tileSize / 4);
                temporaryAnimatedSprite10.xPeriodicRange = (float) num74;
                int num75 = 1;
                temporaryAnimatedSprite10.light = num75 != 0;
                Color black10 = Color.Black;
                temporaryAnimatedSprite10.lightcolor = black10;
                double num76 = 1.0;
                temporaryAnimatedSprite10.lightRadius = (float) num76;
                int num77 = 43 * Game1.tileSize;
                temporaryAnimatedSprite10.yStopCoordinate = num77;
                int num78 = 24000;
                temporaryAnimatedSprite10.delayBeforeAnimationStart = num78;
                int num79 = 1;
                temporaryAnimatedSprite10.pingPong = num79 != 0;
                underwaterSprites10.Add(temporaryAnimatedSprite10);
                List<TemporaryAnimatedSprite> underwaterSprites11 = this.underwaterSprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite11 = new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(18f, 49f) * (float) Game1.tileSize, false, false, 0.1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                temporaryAnimatedSprite11.motion = new Vector2(0.0f, -1f);
                int num80 = 1;
                temporaryAnimatedSprite11.xPeriodic = num80 != 0;
                double num81 = 3000.0;
                temporaryAnimatedSprite11.xPeriodicLoopTime = (float) num81;
                double num82 = (double) (Game1.tileSize / 4);
                temporaryAnimatedSprite11.xPeriodicRange = (float) num82;
                int num83 = 1;
                temporaryAnimatedSprite11.light = num83 != 0;
                Color black11 = Color.Black;
                temporaryAnimatedSprite11.lightcolor = black11;
                double num84 = 1.0;
                temporaryAnimatedSprite11.lightRadius = (float) num84;
                int num85 = 30 * Game1.tileSize;
                temporaryAnimatedSprite11.yStopCoordinate = num85;
                int num86 = 24600;
                temporaryAnimatedSprite11.delayBeforeAnimationStart = num86;
                int num87 = 1;
                temporaryAnimatedSprite11.pingPong = num87 != 0;
                underwaterSprites11.Add(temporaryAnimatedSprite11);
                List<TemporaryAnimatedSprite> underwaterSprites12 = this.underwaterSprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite12 = new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(33f, 49f) * (float) Game1.tileSize, false, false, 0.1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                temporaryAnimatedSprite12.motion = new Vector2(0.0f, -1f);
                int num88 = 1;
                temporaryAnimatedSprite12.xPeriodic = num88 != 0;
                double num89 = 3000.0;
                temporaryAnimatedSprite12.xPeriodicLoopTime = (float) num89;
                double num90 = (double) (Game1.tileSize / 4);
                temporaryAnimatedSprite12.xPeriodicRange = (float) num90;
                int num91 = 1;
                temporaryAnimatedSprite12.light = num91 != 0;
                Color black12 = Color.Black;
                temporaryAnimatedSprite12.lightcolor = black12;
                double num92 = 1.0;
                temporaryAnimatedSprite12.lightRadius = (float) num92;
                int num93 = 40 * Game1.tileSize;
                temporaryAnimatedSprite12.yStopCoordinate = num93;
                int num94 = 25600;
                temporaryAnimatedSprite12.delayBeforeAnimationStart = num94;
                int num95 = 1;
                temporaryAnimatedSprite12.pingPong = num95 != 0;
                underwaterSprites12.Add(temporaryAnimatedSprite12);
                List<TemporaryAnimatedSprite> underwaterSprites13 = this.underwaterSprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite13 = new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(36f, 49f) * (float) Game1.tileSize, false, false, 0.1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                temporaryAnimatedSprite13.motion = new Vector2(0.0f, -1f);
                int num96 = 1;
                temporaryAnimatedSprite13.xPeriodic = num96 != 0;
                double num97 = 3000.0;
                temporaryAnimatedSprite13.xPeriodicLoopTime = (float) num97;
                double num98 = (double) (Game1.tileSize / 4);
                temporaryAnimatedSprite13.xPeriodicRange = (float) num98;
                int num99 = 1;
                temporaryAnimatedSprite13.light = num99 != 0;
                Color black13 = Color.Black;
                temporaryAnimatedSprite13.lightcolor = black13;
                double num100 = 1.0;
                temporaryAnimatedSprite13.lightRadius = (float) num100;
                int num101 = 39 * Game1.tileSize;
                temporaryAnimatedSprite13.yStopCoordinate = num101;
                int num102 = 26900;
                temporaryAnimatedSprite13.delayBeforeAnimationStart = num102;
                int num103 = 1;
                temporaryAnimatedSprite13.pingPong = num103 != 0;
                underwaterSprites13.Add(temporaryAnimatedSprite13);
                List<TemporaryAnimatedSprite> underwaterSprites14 = this.underwaterSprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite14 = new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(304, 16, 16, 16), 200f, 3, 9999, new Vector2(21f, 49f) * (float) Game1.tileSize, false, false, 0.1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                temporaryAnimatedSprite14.motion = new Vector2(0.0f, -1.5f);
                int num104 = 1;
                temporaryAnimatedSprite14.xPeriodic = num104 != 0;
                double num105 = 2500.0;
                temporaryAnimatedSprite14.xPeriodicLoopTime = (float) num105;
                double num106 = (double) (Game1.tileSize / 6);
                temporaryAnimatedSprite14.xPeriodicRange = (float) num106;
                int num107 = 1;
                temporaryAnimatedSprite14.light = num107 != 0;
                Color black14 = Color.Black;
                temporaryAnimatedSprite14.lightcolor = black14;
                double num108 = 1.0;
                temporaryAnimatedSprite14.lightRadius = (float) num108;
                int num109 = 34 * Game1.tileSize;
                temporaryAnimatedSprite14.yStopCoordinate = num109;
                int num110 = 28000;
                temporaryAnimatedSprite14.delayBeforeAnimationStart = num110;
                int num111 = 1;
                temporaryAnimatedSprite14.pingPong = num111 != 0;
                underwaterSprites14.Add(temporaryAnimatedSprite14);
                List<TemporaryAnimatedSprite> underwaterSprites15 = this.underwaterSprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite15 = new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(304, 16, 16, 16), 200f, 3, 9999, new Vector2(20f, 49f) * (float) Game1.tileSize, false, false, 0.1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                temporaryAnimatedSprite15.motion = new Vector2(0.0f, -1.5f);
                int num112 = 1;
                temporaryAnimatedSprite15.xPeriodic = num112 != 0;
                double num113 = 2500.0;
                temporaryAnimatedSprite15.xPeriodicLoopTime = (float) num113;
                double num114 = (double) (Game1.tileSize / 6);
                temporaryAnimatedSprite15.xPeriodicRange = (float) num114;
                int num115 = 1;
                temporaryAnimatedSprite15.light = num115 != 0;
                Color black15 = Color.Black;
                temporaryAnimatedSprite15.lightcolor = black15;
                double num116 = 1.0;
                temporaryAnimatedSprite15.lightRadius = (float) num116;
                int num117 = 35 * Game1.tileSize;
                temporaryAnimatedSprite15.yStopCoordinate = num117;
                int num118 = 28500;
                temporaryAnimatedSprite15.delayBeforeAnimationStart = num118;
                int num119 = 1;
                temporaryAnimatedSprite15.pingPong = num119 != 0;
                underwaterSprites15.Add(temporaryAnimatedSprite15);
                List<TemporaryAnimatedSprite> underwaterSprites16 = this.underwaterSprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite16 = new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(304, 16, 16, 16), 200f, 3, 9999, new Vector2(22f, 49f) * (float) Game1.tileSize, false, false, 0.1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                temporaryAnimatedSprite16.motion = new Vector2(0.0f, -1.5f);
                int num120 = 1;
                temporaryAnimatedSprite16.xPeriodic = num120 != 0;
                double num121 = 2500.0;
                temporaryAnimatedSprite16.xPeriodicLoopTime = (float) num121;
                double num122 = (double) (Game1.tileSize / 6);
                temporaryAnimatedSprite16.xPeriodicRange = (float) num122;
                int num123 = 1;
                temporaryAnimatedSprite16.light = num123 != 0;
                Color black16 = Color.Black;
                temporaryAnimatedSprite16.lightcolor = black16;
                double num124 = 1.0;
                temporaryAnimatedSprite16.lightRadius = (float) num124;
                int num125 = 36 * Game1.tileSize;
                temporaryAnimatedSprite16.yStopCoordinate = num125;
                int num126 = 28500;
                temporaryAnimatedSprite16.delayBeforeAnimationStart = num126;
                int num127 = 1;
                temporaryAnimatedSprite16.pingPong = num127 != 0;
                underwaterSprites16.Add(temporaryAnimatedSprite16);
                List<TemporaryAnimatedSprite> underwaterSprites17 = this.underwaterSprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite17 = new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(304, 16, 16, 16), 200f, 3, 9999, new Vector2(33f, 49f) * (float) Game1.tileSize, false, false, 0.1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                temporaryAnimatedSprite17.motion = new Vector2(0.0f, -1.5f);
                int num128 = 1;
                temporaryAnimatedSprite17.xPeriodic = num128 != 0;
                double num129 = 2500.0;
                temporaryAnimatedSprite17.xPeriodicLoopTime = (float) num129;
                double num130 = (double) (Game1.tileSize / 6);
                temporaryAnimatedSprite17.xPeriodicRange = (float) num130;
                int num131 = 1;
                temporaryAnimatedSprite17.light = num131 != 0;
                Color black17 = Color.Black;
                temporaryAnimatedSprite17.lightcolor = black17;
                double num132 = 1.0;
                temporaryAnimatedSprite17.lightRadius = (float) num132;
                int num133 = 43 * Game1.tileSize;
                temporaryAnimatedSprite17.yStopCoordinate = num133;
                int num134 = 29000;
                temporaryAnimatedSprite17.delayBeforeAnimationStart = num134;
                int num135 = 1;
                temporaryAnimatedSprite17.pingPong = num135 != 0;
                underwaterSprites17.Add(temporaryAnimatedSprite17);
                List<TemporaryAnimatedSprite> underwaterSprites18 = this.underwaterSprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite18 = new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(304, 16, 16, 16), 200f, 3, 9999, new Vector2(36f, 49f) * (float) Game1.tileSize, false, false, 0.1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                temporaryAnimatedSprite18.motion = new Vector2(0.0f, -1.5f);
                int num136 = 1;
                temporaryAnimatedSprite18.xPeriodic = num136 != 0;
                double num137 = 2500.0;
                temporaryAnimatedSprite18.xPeriodicLoopTime = (float) num137;
                double num138 = (double) (Game1.tileSize / 6);
                temporaryAnimatedSprite18.xPeriodicRange = (float) num138;
                int num139 = 1;
                temporaryAnimatedSprite18.light = num139 != 0;
                Color black18 = Color.Black;
                temporaryAnimatedSprite18.lightcolor = black18;
                double num140 = 1.0;
                temporaryAnimatedSprite18.lightRadius = (float) num140;
                int num141 = 43 * Game1.tileSize;
                temporaryAnimatedSprite18.yStopCoordinate = num141;
                int num142 = 30000;
                temporaryAnimatedSprite18.delayBeforeAnimationStart = num142;
                int num143 = 1;
                temporaryAnimatedSprite18.pingPong = num143 != 0;
                underwaterSprites18.Add(temporaryAnimatedSprite18);
                List<TemporaryAnimatedSprite> underwaterSprites19 = this.underwaterSprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite19 = new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 32, 16, 16), 250f, 3, 9999, new Vector2(28f, 49f) * (float) Game1.tileSize, false, false, 0.1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                temporaryAnimatedSprite19.motion = new Vector2(-0.5f, -0.5f);
                int num144 = 1;
                temporaryAnimatedSprite19.xPeriodic = num144 != 0;
                double num145 = 4000.0;
                temporaryAnimatedSprite19.xPeriodicLoopTime = (float) num145;
                double num146 = (double) (Game1.tileSize / 4);
                temporaryAnimatedSprite19.xPeriodicRange = (float) num146;
                int num147 = 1;
                temporaryAnimatedSprite19.light = num147 != 0;
                Color black19 = Color.Black;
                temporaryAnimatedSprite19.lightcolor = black19;
                double num148 = 2.0;
                temporaryAnimatedSprite19.lightRadius = (float) num148;
                int num149 = 19 * Game1.tileSize;
                temporaryAnimatedSprite19.xStopCoordinate = num149;
                int num150 = 38 * Game1.tileSize;
                temporaryAnimatedSprite19.yStopCoordinate = num150;
                int num151 = 32000;
                temporaryAnimatedSprite19.delayBeforeAnimationStart = num151;
                int num152 = 1;
                temporaryAnimatedSprite19.pingPong = num152 != 0;
                underwaterSprites19.Add(temporaryAnimatedSprite19);
                List<TemporaryAnimatedSprite> underwaterSprites20 = this.underwaterSprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite20 = new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(40f, 49f) * (float) Game1.tileSize, false, false, 0.1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                temporaryAnimatedSprite20.motion = new Vector2(0.0f, -1f);
                int num153 = 1;
                temporaryAnimatedSprite20.xPeriodic = num153 != 0;
                double num154 = 3000.0;
                temporaryAnimatedSprite20.xPeriodicLoopTime = (float) num154;
                double num155 = (double) (Game1.tileSize / 4);
                temporaryAnimatedSprite20.xPeriodicRange = (float) num155;
                int num156 = 1;
                temporaryAnimatedSprite20.light = num156 != 0;
                Color black20 = Color.Black;
                temporaryAnimatedSprite20.lightcolor = black20;
                double num157 = 1.0;
                temporaryAnimatedSprite20.lightRadius = (float) num157;
                int num158 = 40 * Game1.tileSize;
                temporaryAnimatedSprite20.yStopCoordinate = num158;
                int num159 = 10000;
                temporaryAnimatedSprite20.delayBeforeAnimationStart = num159;
                int num160 = 1;
                temporaryAnimatedSprite20.pingPong = num160 != 0;
                underwaterSprites20.Add(temporaryAnimatedSprite20);
                List<TemporaryAnimatedSprite> underwaterSprites21 = this.underwaterSprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite21 = new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(42f, 49f) * (float) Game1.tileSize, false, false, 0.1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                temporaryAnimatedSprite21.motion = new Vector2(0.0f, -1f);
                int num161 = 1;
                temporaryAnimatedSprite21.xPeriodic = num161 != 0;
                double num162 = 3000.0;
                temporaryAnimatedSprite21.xPeriodicLoopTime = (float) num162;
                double num163 = (double) (Game1.tileSize / 4);
                temporaryAnimatedSprite21.xPeriodicRange = (float) num163;
                int num164 = 1;
                temporaryAnimatedSprite21.light = num164 != 0;
                Color black21 = Color.Black;
                temporaryAnimatedSprite21.lightcolor = black21;
                double num165 = 1.0;
                temporaryAnimatedSprite21.lightRadius = (float) num165;
                int num166 = 43 * Game1.tileSize;
                temporaryAnimatedSprite21.yStopCoordinate = num166;
                int num167 = 1;
                temporaryAnimatedSprite21.pingPong = num167 != 0;
                underwaterSprites21.Add(temporaryAnimatedSprite21);
                List<TemporaryAnimatedSprite> underwaterSprites22 = this.underwaterSprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite22 = new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(43f, 49f) * (float) Game1.tileSize, false, false, 0.1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                temporaryAnimatedSprite22.motion = new Vector2(0.0f, -1f);
                int num168 = 1;
                temporaryAnimatedSprite22.xPeriodic = num168 != 0;
                double num169 = 3000.0;
                temporaryAnimatedSprite22.xPeriodicLoopTime = (float) num169;
                double num170 = (double) (Game1.tileSize / 4);
                temporaryAnimatedSprite22.xPeriodicRange = (float) num170;
                int num171 = 1;
                temporaryAnimatedSprite22.light = num171 != 0;
                Color black22 = Color.Black;
                temporaryAnimatedSprite22.lightcolor = black22;
                double num172 = 1.0;
                temporaryAnimatedSprite22.lightRadius = (float) num172;
                int num173 = 41 * Game1.tileSize;
                temporaryAnimatedSprite22.yStopCoordinate = num173;
                int num174 = 12000;
                temporaryAnimatedSprite22.delayBeforeAnimationStart = num174;
                int num175 = 1;
                temporaryAnimatedSprite22.pingPong = num175 != 0;
                underwaterSprites22.Add(temporaryAnimatedSprite22);
                List<TemporaryAnimatedSprite> underwaterSprites23 = this.underwaterSprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite23 = new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(45f, 49f) * (float) Game1.tileSize, false, false, 0.1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                temporaryAnimatedSprite23.motion = new Vector2(0.0f, -1f);
                int num176 = 1;
                temporaryAnimatedSprite23.xPeriodic = num176 != 0;
                double num177 = 3000.0;
                temporaryAnimatedSprite23.xPeriodicLoopTime = (float) num177;
                double num178 = (double) (Game1.tileSize / 4);
                temporaryAnimatedSprite23.xPeriodicRange = (float) num178;
                int num179 = 1;
                temporaryAnimatedSprite23.light = num179 != 0;
                Color black23 = Color.Black;
                temporaryAnimatedSprite23.lightcolor = black23;
                double num180 = 1.0;
                temporaryAnimatedSprite23.lightRadius = (float) num180;
                int num181 = 39 * Game1.tileSize;
                temporaryAnimatedSprite23.yStopCoordinate = num181;
                int num182 = 14000;
                temporaryAnimatedSprite23.delayBeforeAnimationStart = num182;
                int num183 = 1;
                temporaryAnimatedSprite23.pingPong = num183 != 0;
                underwaterSprites23.Add(temporaryAnimatedSprite23);
                List<TemporaryAnimatedSprite> underwaterSprites24 = this.underwaterSprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite24 = new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(46f, 49f) * (float) Game1.tileSize, false, false, 0.1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                temporaryAnimatedSprite24.motion = new Vector2(0.0f, -1f);
                int num184 = 1;
                temporaryAnimatedSprite24.xPeriodic = num184 != 0;
                double num185 = 3000.0;
                temporaryAnimatedSprite24.xPeriodicLoopTime = (float) num185;
                double num186 = (double) (Game1.tileSize / 4);
                temporaryAnimatedSprite24.xPeriodicRange = (float) num186;
                int num187 = 1;
                temporaryAnimatedSprite24.light = num187 != 0;
                Color black24 = Color.Black;
                temporaryAnimatedSprite24.lightcolor = black24;
                double num188 = 1.0;
                temporaryAnimatedSprite24.lightRadius = (float) num188;
                int num189 = 29 * Game1.tileSize;
                temporaryAnimatedSprite24.yStopCoordinate = num189;
                int num190 = 19500;
                temporaryAnimatedSprite24.delayBeforeAnimationStart = num190;
                int num191 = 1;
                temporaryAnimatedSprite24.pingPong = num191 != 0;
                underwaterSprites24.Add(temporaryAnimatedSprite24);
                List<TemporaryAnimatedSprite> underwaterSprites25 = this.underwaterSprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite25 = new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(48f, 49f) * (float) Game1.tileSize, false, false, 0.1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                temporaryAnimatedSprite25.motion = new Vector2(0.0f, -1f);
                int num192 = 1;
                temporaryAnimatedSprite25.xPeriodic = num192 != 0;
                double num193 = 3000.0;
                temporaryAnimatedSprite25.xPeriodicLoopTime = (float) num193;
                double num194 = (double) (Game1.tileSize / 4);
                temporaryAnimatedSprite25.xPeriodicRange = (float) num194;
                int num195 = 1;
                temporaryAnimatedSprite25.light = num195 != 0;
                Color black25 = Color.Black;
                temporaryAnimatedSprite25.lightcolor = black25;
                double num196 = 1.0;
                temporaryAnimatedSprite25.lightRadius = (float) num196;
                int num197 = 35 * Game1.tileSize;
                temporaryAnimatedSprite25.yStopCoordinate = num197;
                int num198 = 20300;
                temporaryAnimatedSprite25.delayBeforeAnimationStart = num198;
                int num199 = 1;
                temporaryAnimatedSprite25.pingPong = num199 != 0;
                underwaterSprites25.Add(temporaryAnimatedSprite25);
                List<TemporaryAnimatedSprite> underwaterSprites26 = this.underwaterSprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite26 = new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(49f, 49f) * (float) Game1.tileSize, false, false, 0.1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                temporaryAnimatedSprite26.motion = new Vector2(0.0f, -1f);
                int num200 = 1;
                temporaryAnimatedSprite26.xPeriodic = num200 != 0;
                double num201 = 3000.0;
                temporaryAnimatedSprite26.xPeriodicLoopTime = (float) num201;
                double num202 = (double) (Game1.tileSize / 4);
                temporaryAnimatedSprite26.xPeriodicRange = (float) num202;
                int num203 = 1;
                temporaryAnimatedSprite26.light = num203 != 0;
                Color black26 = Color.Black;
                temporaryAnimatedSprite26.lightcolor = black26;
                double num204 = 1.0;
                temporaryAnimatedSprite26.lightRadius = (float) num204;
                int num205 = 40 * Game1.tileSize;
                temporaryAnimatedSprite26.yStopCoordinate = num205;
                int num206 = 21500;
                temporaryAnimatedSprite26.delayBeforeAnimationStart = num206;
                int num207 = 1;
                temporaryAnimatedSprite26.pingPong = num207 != 0;
                underwaterSprites26.Add(temporaryAnimatedSprite26);
                List<TemporaryAnimatedSprite> underwaterSprites27 = this.underwaterSprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite27 = new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(50f, 49f) * (float) Game1.tileSize, false, false, 0.1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                temporaryAnimatedSprite27.motion = new Vector2(0.0f, -1f);
                int num208 = 1;
                temporaryAnimatedSprite27.xPeriodic = num208 != 0;
                double num209 = 3000.0;
                temporaryAnimatedSprite27.xPeriodicLoopTime = (float) num209;
                double num210 = (double) (Game1.tileSize / 4);
                temporaryAnimatedSprite27.xPeriodicRange = (float) num210;
                int num211 = 1;
                temporaryAnimatedSprite27.light = num211 != 0;
                Color black27 = Color.Black;
                temporaryAnimatedSprite27.lightcolor = black27;
                double num212 = 1.0;
                temporaryAnimatedSprite27.lightRadius = (float) num212;
                int num213 = 30 * Game1.tileSize;
                temporaryAnimatedSprite27.yStopCoordinate = num213;
                int num214 = 22400;
                temporaryAnimatedSprite27.delayBeforeAnimationStart = num214;
                int num215 = 1;
                temporaryAnimatedSprite27.pingPong = num215 != 0;
                underwaterSprites27.Add(temporaryAnimatedSprite27);
                List<TemporaryAnimatedSprite> underwaterSprites28 = this.underwaterSprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite28 = new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(51f, 49f) * (float) Game1.tileSize, false, false, 0.1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                temporaryAnimatedSprite28.motion = new Vector2(0.0f, -1f);
                int num216 = 1;
                temporaryAnimatedSprite28.xPeriodic = num216 != 0;
                double num217 = 3000.0;
                temporaryAnimatedSprite28.xPeriodicLoopTime = (float) num217;
                double num218 = (double) (Game1.tileSize / 4);
                temporaryAnimatedSprite28.xPeriodicRange = (float) num218;
                int num219 = 1;
                temporaryAnimatedSprite28.light = num219 != 0;
                Color black28 = Color.Black;
                temporaryAnimatedSprite28.lightcolor = black28;
                double num220 = 1.0;
                temporaryAnimatedSprite28.lightRadius = (float) num220;
                int num221 = 33 * Game1.tileSize;
                temporaryAnimatedSprite28.yStopCoordinate = num221;
                int num222 = 23200;
                temporaryAnimatedSprite28.delayBeforeAnimationStart = num222;
                int num223 = 1;
                temporaryAnimatedSprite28.pingPong = num223 != 0;
                underwaterSprites28.Add(temporaryAnimatedSprite28);
                List<TemporaryAnimatedSprite> underwaterSprites29 = this.underwaterSprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite29 = new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(52f, 49f) * (float) Game1.tileSize, false, false, 0.1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                temporaryAnimatedSprite29.motion = new Vector2(0.0f, -1f);
                int num224 = 1;
                temporaryAnimatedSprite29.xPeriodic = num224 != 0;
                double num225 = 3000.0;
                temporaryAnimatedSprite29.xPeriodicLoopTime = (float) num225;
                double num226 = (double) (Game1.tileSize / 4);
                temporaryAnimatedSprite29.xPeriodicRange = (float) num226;
                int num227 = 1;
                temporaryAnimatedSprite29.light = num227 != 0;
                Color black29 = Color.Black;
                temporaryAnimatedSprite29.lightcolor = black29;
                double num228 = 1.0;
                temporaryAnimatedSprite29.lightRadius = (float) num228;
                int num229 = 38 * Game1.tileSize;
                temporaryAnimatedSprite29.yStopCoordinate = num229;
                int num230 = 24000;
                temporaryAnimatedSprite29.delayBeforeAnimationStart = num230;
                int num231 = 1;
                temporaryAnimatedSprite29.pingPong = num231 != 0;
                underwaterSprites29.Add(temporaryAnimatedSprite29);
                List<TemporaryAnimatedSprite> underwaterSprites30 = this.underwaterSprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite30 = new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(53f, 49f) * (float) Game1.tileSize, false, false, 0.1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                temporaryAnimatedSprite30.motion = new Vector2(0.0f, -1f);
                int num232 = 1;
                temporaryAnimatedSprite30.xPeriodic = num232 != 0;
                double num233 = 3000.0;
                temporaryAnimatedSprite30.xPeriodicLoopTime = (float) num233;
                double num234 = (double) (Game1.tileSize / 4);
                temporaryAnimatedSprite30.xPeriodicRange = (float) num234;
                int num235 = 1;
                temporaryAnimatedSprite30.light = num235 != 0;
                Color black30 = Color.Black;
                temporaryAnimatedSprite30.lightcolor = black30;
                double num236 = 1.0;
                temporaryAnimatedSprite30.lightRadius = (float) num236;
                int num237 = 35 * Game1.tileSize;
                temporaryAnimatedSprite30.yStopCoordinate = num237;
                int num238 = 24600;
                temporaryAnimatedSprite30.delayBeforeAnimationStart = num238;
                int num239 = 1;
                temporaryAnimatedSprite30.pingPong = num239 != 0;
                underwaterSprites30.Add(temporaryAnimatedSprite30);
                List<TemporaryAnimatedSprite> underwaterSprites31 = this.underwaterSprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite31 = new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(54f, 49f) * (float) Game1.tileSize, false, false, 0.1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                temporaryAnimatedSprite31.motion = new Vector2(0.0f, -1f);
                int num240 = 1;
                temporaryAnimatedSprite31.xPeriodic = num240 != 0;
                double num241 = 3000.0;
                temporaryAnimatedSprite31.xPeriodicLoopTime = (float) num241;
                double num242 = (double) (Game1.tileSize / 4);
                temporaryAnimatedSprite31.xPeriodicRange = (float) num242;
                int num243 = 1;
                temporaryAnimatedSprite31.light = num243 != 0;
                Color black31 = Color.Black;
                temporaryAnimatedSprite31.lightcolor = black31;
                double num244 = 1.0;
                temporaryAnimatedSprite31.lightRadius = (float) num244;
                int num245 = 30 * Game1.tileSize;
                temporaryAnimatedSprite31.yStopCoordinate = num245;
                int num246 = 25600;
                temporaryAnimatedSprite31.delayBeforeAnimationStart = num246;
                int num247 = 1;
                temporaryAnimatedSprite31.pingPong = num247 != 0;
                underwaterSprites31.Add(temporaryAnimatedSprite31);
                List<TemporaryAnimatedSprite> underwaterSprites32 = this.underwaterSprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite32 = new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(55f, 49f) * (float) Game1.tileSize, false, false, 0.1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                temporaryAnimatedSprite32.motion = new Vector2(0.0f, -1f);
                int num248 = 1;
                temporaryAnimatedSprite32.xPeriodic = num248 != 0;
                double num249 = 3000.0;
                temporaryAnimatedSprite32.xPeriodicLoopTime = (float) num249;
                double num250 = (double) (Game1.tileSize / 4);
                temporaryAnimatedSprite32.xPeriodicRange = (float) num250;
                int num251 = 1;
                temporaryAnimatedSprite32.light = num251 != 0;
                Color black32 = Color.Black;
                temporaryAnimatedSprite32.lightcolor = black32;
                double num252 = 1.0;
                temporaryAnimatedSprite32.lightRadius = (float) num252;
                int num253 = 40 * Game1.tileSize;
                temporaryAnimatedSprite32.yStopCoordinate = num253;
                int num254 = 26900;
                temporaryAnimatedSprite32.delayBeforeAnimationStart = num254;
                int num255 = 1;
                temporaryAnimatedSprite32.pingPong = num255 != 0;
                underwaterSprites32.Add(temporaryAnimatedSprite32);
                List<TemporaryAnimatedSprite> underwaterSprites33 = this.underwaterSprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite33 = new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(4f, 49f) * (float) Game1.tileSize, false, false, 0.1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                temporaryAnimatedSprite33.motion = new Vector2(0.0f, -1f);
                int num256 = 1;
                temporaryAnimatedSprite33.xPeriodic = num256 != 0;
                double num257 = 3000.0;
                temporaryAnimatedSprite33.xPeriodicLoopTime = (float) num257;
                double num258 = (double) (Game1.tileSize / 4);
                temporaryAnimatedSprite33.xPeriodicRange = (float) num258;
                int num259 = 1;
                temporaryAnimatedSprite33.light = num259 != 0;
                Color black33 = Color.Black;
                temporaryAnimatedSprite33.lightcolor = black33;
                double num260 = 1.0;
                temporaryAnimatedSprite33.lightRadius = (float) num260;
                int num261 = 30 * Game1.tileSize;
                temporaryAnimatedSprite33.yStopCoordinate = num261;
                int num262 = 24000;
                temporaryAnimatedSprite33.delayBeforeAnimationStart = num262;
                int num263 = 1;
                temporaryAnimatedSprite33.pingPong = num263 != 0;
                underwaterSprites33.Add(temporaryAnimatedSprite33);
                List<TemporaryAnimatedSprite> underwaterSprites34 = this.underwaterSprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite34 = new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(5f, 49f) * (float) Game1.tileSize, false, false, 0.1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                temporaryAnimatedSprite34.motion = new Vector2(0.0f, -1f);
                int num264 = 1;
                temporaryAnimatedSprite34.xPeriodic = num264 != 0;
                double num265 = 3000.0;
                temporaryAnimatedSprite34.xPeriodicLoopTime = (float) num265;
                double num266 = (double) (Game1.tileSize / 4);
                temporaryAnimatedSprite34.xPeriodicRange = (float) num266;
                int num267 = 1;
                temporaryAnimatedSprite34.light = num267 != 0;
                Color black34 = Color.Black;
                temporaryAnimatedSprite34.lightcolor = black34;
                double num268 = 1.0;
                temporaryAnimatedSprite34.lightRadius = (float) num268;
                int num269 = 40 * Game1.tileSize;
                temporaryAnimatedSprite34.yStopCoordinate = num269;
                int num270 = 24600;
                temporaryAnimatedSprite34.delayBeforeAnimationStart = num270;
                int num271 = 1;
                temporaryAnimatedSprite34.pingPong = num271 != 0;
                underwaterSprites34.Add(temporaryAnimatedSprite34);
                List<TemporaryAnimatedSprite> underwaterSprites35 = this.underwaterSprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite35 = new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(3f, 49f) * (float) Game1.tileSize, false, false, 0.1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                temporaryAnimatedSprite35.motion = new Vector2(0.0f, -1f);
                int num272 = 1;
                temporaryAnimatedSprite35.xPeriodic = num272 != 0;
                double num273 = 3000.0;
                temporaryAnimatedSprite35.xPeriodicLoopTime = (float) num273;
                double num274 = (double) (Game1.tileSize / 4);
                temporaryAnimatedSprite35.xPeriodicRange = (float) num274;
                int num275 = 1;
                temporaryAnimatedSprite35.light = num275 != 0;
                Color black35 = Color.Black;
                temporaryAnimatedSprite35.lightcolor = black35;
                double num276 = 1.0;
                temporaryAnimatedSprite35.lightRadius = (float) num276;
                int num277 = 34 * Game1.tileSize;
                temporaryAnimatedSprite35.yStopCoordinate = num277;
                int num278 = 25600;
                temporaryAnimatedSprite35.delayBeforeAnimationStart = num278;
                int num279 = 1;
                temporaryAnimatedSprite35.pingPong = num279 != 0;
                underwaterSprites35.Add(temporaryAnimatedSprite35);
                List<TemporaryAnimatedSprite> underwaterSprites36 = this.underwaterSprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite36 = new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(6f, 49f) * (float) Game1.tileSize, false, false, 0.1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                temporaryAnimatedSprite36.motion = new Vector2(0.0f, -1f);
                int num280 = 1;
                temporaryAnimatedSprite36.xPeriodic = num280 != 0;
                double num281 = 3000.0;
                temporaryAnimatedSprite36.xPeriodicLoopTime = (float) num281;
                double num282 = (double) (Game1.tileSize / 4);
                temporaryAnimatedSprite36.xPeriodicRange = (float) num282;
                int num283 = 1;
                temporaryAnimatedSprite36.light = num283 != 0;
                Color black36 = Color.Black;
                temporaryAnimatedSprite36.lightcolor = black36;
                double num284 = 1.0;
                temporaryAnimatedSprite36.lightRadius = (float) num284;
                int num285 = 37 * Game1.tileSize;
                temporaryAnimatedSprite36.yStopCoordinate = num285;
                int num286 = 26900;
                temporaryAnimatedSprite36.delayBeforeAnimationStart = num286;
                int num287 = 1;
                temporaryAnimatedSprite36.pingPong = num287 != 0;
                underwaterSprites36.Add(temporaryAnimatedSprite36);
                List<TemporaryAnimatedSprite> underwaterSprites37 = this.underwaterSprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite37 = new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(256, 16, 16, 16), 250f, 3, 9999, new Vector2(8f, 49f) * (float) Game1.tileSize, false, false, 0.1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                temporaryAnimatedSprite37.motion = new Vector2(0.0f, -1f);
                int num288 = 1;
                temporaryAnimatedSprite37.xPeriodic = num288 != 0;
                double num289 = 3000.0;
                temporaryAnimatedSprite37.xPeriodicLoopTime = (float) num289;
                double num290 = (double) (Game1.tileSize / 4);
                temporaryAnimatedSprite37.xPeriodicRange = (float) num290;
                int num291 = 1;
                temporaryAnimatedSprite37.light = num291 != 0;
                Color black37 = Color.Black;
                temporaryAnimatedSprite37.lightcolor = black37;
                double num292 = 1.0;
                temporaryAnimatedSprite37.lightRadius = (float) num292;
                int num293 = 42 * Game1.tileSize;
                temporaryAnimatedSprite37.yStopCoordinate = num293;
                int num294 = 26900;
                temporaryAnimatedSprite37.delayBeforeAnimationStart = num294;
                int num295 = 1;
                temporaryAnimatedSprite37.pingPong = num295 != 0;
                underwaterSprites37.Add(temporaryAnimatedSprite37);
                List<TemporaryAnimatedSprite> underwaterSprites38 = this.underwaterSprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite38 = new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(304, 16, 16, 16), 200f, 3, 9999, new Vector2(50f, 49f) * (float) Game1.tileSize, false, false, 0.1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                temporaryAnimatedSprite38.motion = new Vector2(0.0f, -1.5f);
                int num296 = 1;
                temporaryAnimatedSprite38.xPeriodic = num296 != 0;
                double num297 = 2500.0;
                temporaryAnimatedSprite38.xPeriodicLoopTime = (float) num297;
                double num298 = (double) (Game1.tileSize / 6);
                temporaryAnimatedSprite38.xPeriodicRange = (float) num298;
                int num299 = 1;
                temporaryAnimatedSprite38.light = num299 != 0;
                Color black38 = Color.Black;
                temporaryAnimatedSprite38.lightcolor = black38;
                double num300 = 1.0;
                temporaryAnimatedSprite38.lightRadius = (float) num300;
                int num301 = 42 * Game1.tileSize;
                temporaryAnimatedSprite38.yStopCoordinate = num301;
                int num302 = 28500;
                temporaryAnimatedSprite38.delayBeforeAnimationStart = num302;
                int num303 = 1;
                temporaryAnimatedSprite38.pingPong = num303 != 0;
                underwaterSprites38.Add(temporaryAnimatedSprite38);
                List<TemporaryAnimatedSprite> underwaterSprites39 = this.underwaterSprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite39 = new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(304, 16, 16, 16), 200f, 3, 9999, new Vector2(51f, 49f) * (float) Game1.tileSize, false, false, 0.1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                temporaryAnimatedSprite39.motion = new Vector2(0.0f, -1.5f);
                int num304 = 1;
                temporaryAnimatedSprite39.xPeriodic = num304 != 0;
                double num305 = 2500.0;
                temporaryAnimatedSprite39.xPeriodicLoopTime = (float) num305;
                double num306 = (double) (Game1.tileSize / 6);
                temporaryAnimatedSprite39.xPeriodicRange = (float) num306;
                int num307 = 1;
                temporaryAnimatedSprite39.light = num307 != 0;
                Color black39 = Color.Black;
                temporaryAnimatedSprite39.lightcolor = black39;
                double num308 = 1.0;
                temporaryAnimatedSprite39.lightRadius = (float) num308;
                int num309 = 43 * Game1.tileSize;
                temporaryAnimatedSprite39.yStopCoordinate = num309;
                int num310 = 28500;
                temporaryAnimatedSprite39.delayBeforeAnimationStart = num310;
                int num311 = 1;
                temporaryAnimatedSprite39.pingPong = num311 != 0;
                underwaterSprites39.Add(temporaryAnimatedSprite39);
                List<TemporaryAnimatedSprite> underwaterSprites40 = this.underwaterSprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite40 = new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(304, 16, 16, 16), 200f, 3, 9999, new Vector2(52f, 49f) * (float) Game1.tileSize, false, false, 0.1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                temporaryAnimatedSprite40.motion = new Vector2(0.0f, -1.5f);
                int num312 = 1;
                temporaryAnimatedSprite40.xPeriodic = num312 != 0;
                double num313 = 2500.0;
                temporaryAnimatedSprite40.xPeriodicLoopTime = (float) num313;
                double num314 = (double) (Game1.tileSize / 6);
                temporaryAnimatedSprite40.xPeriodicRange = (float) num314;
                int num315 = 1;
                temporaryAnimatedSprite40.light = num315 != 0;
                Color black40 = Color.Black;
                temporaryAnimatedSprite40.lightcolor = black40;
                double num316 = 1.0;
                temporaryAnimatedSprite40.lightRadius = (float) num316;
                int num317 = 44 * Game1.tileSize;
                temporaryAnimatedSprite40.yStopCoordinate = num317;
                int num318 = 29000;
                temporaryAnimatedSprite40.delayBeforeAnimationStart = num318;
                int num319 = 1;
                temporaryAnimatedSprite40.pingPong = num319 != 0;
                underwaterSprites40.Add(temporaryAnimatedSprite40);
              }
              else
              {
                if (!(key == "waterShane"))
                  return;
                this.drawTool = true;
                this.tmpItem = Game1.player.CurrentItem;
                Game1.player.items[Game1.player.CurrentToolIndex] = (Item) new WateringCan();
                Game1.player.CurrentTool.Update(1, 0);
                Game1.player.FarmerSprite.animateOnce(new FarmerSprite.AnimationFrame[4]
                {
                  new FarmerSprite.AnimationFrame(58, 0, false, false, (AnimatedSprite.endOfAnimationBehavior) null, false),
                  new FarmerSprite.AnimationFrame(58, 75, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.showToolSwipeEffect), false),
                  new FarmerSprite.AnimationFrame(59, 100, false, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.useTool), true),
                  new FarmerSprite.AnimationFrame(45, 500, true, false, new AnimatedSprite.endOfAnimationBehavior(Farmer.canMoveNow), true)
                });
              }
            }
            else if ((int) stringHash != 1215102451)
            {
              if ((int) stringHash != 1266391031)
              {
                if ((int) stringHash != 1382680148 || !(key == "EmilySleeping"))
                  return;
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(574, 1892, 11, 11), 1000f, 2, 99999, new Vector2(20f, 3f) * (float) Game1.tileSize + new Vector2((float) (2 * Game1.pixelZoom), (float) (Game1.pixelZoom * 8)), false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
                {
                  id = 999f
                });
              }
              else
              {
                if (!(key == "wedding"))
                  return;
                if (Game1.player.isMale)
                {
                  this.oldShirt = Game1.player.shirt;
                  Game1.player.changeShirt(10);
                  this.oldPants = Game1.player.pantsColor;
                  Game1.player.changePants(new Color(49, 49, 49));
                }
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(540, 1196, 98, 54), 99999f, 1, 99999, new Vector2(25f, 60f) * (float) Game1.tileSize + new Vector2(0.0f, (float) -Game1.tileSize), false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false));
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(540, 1250, 98, 25), 99999f, 1, 99999, new Vector2(25f, 60f) * (float) Game1.tileSize + new Vector2(0.0f, 54f) * (float) Game1.pixelZoom + new Vector2(0.0f, (float) -Game1.tileSize), false, false, 0.0f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false));
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(527, 1249, 12, 25), 99999f, 1, 99999, new Vector2(24f, 62f) * (float) Game1.tileSize, false, false, 0.0f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false));
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(527, 1249, 12, 25), 99999f, 1, 99999, new Vector2(32f, 62f) * (float) Game1.tileSize, false, false, 0.0f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false));
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(527, 1249, 12, 25), 99999f, 1, 99999, new Vector2(24f, 69f) * (float) Game1.tileSize, false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false));
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(527, 1249, 12, 25), 99999f, 1, 99999, new Vector2(32f, 69f) * (float) Game1.tileSize, false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false));
              }
            }
            else
            {
              if (!(key == "waterShaneDone"))
                return;
              Game1.player.completelyStopAnimatingOrDoingAction();
              Game1.player.items[Game1.player.CurrentToolIndex] = this.tmpItem;
              this.drawTool = false;
              location.removeTemporarySpritesWithID(999);
            }
          }
          else if (stringHash <= 1834783535U)
          {
            if ((int) stringHash != 1782528198)
            {
              if ((int) stringHash != 1797522365)
              {
                if ((int) stringHash != 1834783535 || !(key == "jasGift"))
                  return;
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(288, 1231, 16, 16), 100f, 6, 1, new Vector2(22f, 16f) * (float) Game1.tileSize, false, false, 0.01f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
                {
                  id = 999f,
                  paused = true,
                  holdLastFrame = true
                });
              }
              else
              {
                if (!(key == "marcelloLand"))
                  return;
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(0, 1183, 84, 160), 10000f, 1, 99999, new Vector2(25f, 19f) * (float) Game1.tileSize + new Vector2(-23f, 0.0f) * (float) Game1.pixelZoom, false, false, 2E-05f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
                {
                  motion = new Vector2(0.0f, 2f),
                  yStopCoordinate = 41 * Game1.tileSize - 160 * Game1.pixelZoom,
                  reachedStopCoordinate = new TemporaryAnimatedSprite.endBehavior(this.marcelloBalloonLand),
                  attachedCharacter = (Character) this.getActorByName("Marcello"),
                  id = 1f
                });
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(84, 1205, 38, 26), 10000f, 1, 99999, new Vector2(25f, 19f) * (float) Game1.tileSize + new Vector2(0.0f, 134f) * (float) Game1.pixelZoom, false, false, (float) ((double) (41 * Game1.tileSize) / 10000.0 + 9.99999974737875E-05), 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
                {
                  motion = new Vector2(0.0f, 2f),
                  id = 2f
                });
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(24, 1343, 36, 19), 7000f, 1, 99999, new Vector2(25f, 40f) * (float) Game1.tileSize, false, false, 1E-05f, 0.0f, Color.White, 0.0f, 0.0f, 0.0f, 0.0f, false)
                {
                  scaleChange = 0.01f,
                  id = 3f
                });
              }
            }
            else
            {
              if (!(key == "maruTelescope"))
                return;
              for (int index = 0; index < 9; ++index)
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(256, 1680, 16, 16), 80f, 5, 0, new Vector2((float) Game1.random.Next(1, 28), (float) Game1.random.Next(1, 20)) * (float) Game1.tileSize, false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
                {
                  delayBeforeAnimationStart = 8000 + index * Game1.random.Next(2000),
                  motion = new Vector2(4f, 4f)
                });
            }
          }
          else if ((int) stringHash != 1927366026)
          {
            if ((int) stringHash != 1976122661)
            {
              if ((int) stringHash != 1984971571 || !(key == "secretGift"))
                return;
              location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(288, 1231, 16, 16), new Vector2(30f, 70f) * (float) Game1.tileSize + new Vector2(0.0f, (float) (-Game1.tileSize / 3)), false, 0.0f, Color.White)
              {
                animationLength = 1,
                interval = 999999f,
                id = 666f,
                scale = 4f
              });
            }
            else
            {
              if (!(key == "curtainOpen"))
                return;
              location.getTemporarySpriteByID(999).sourceRect.X = 672;
              Game1.playSound("shwip");
            }
          }
          else
          {
            if (!(key == "elliottBoat"))
              return;
            location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(461, 1843, 32, 51), 1000f, 2, 9999, new Vector2(15f, 26f) * (float) Game1.tileSize + new Vector2((float) (-Game1.pixelZoom * 7), 0.0f), false, false, (float) (26 * Game1.tileSize) / 10000f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false));
          }
        }
        else if (stringHash <= 2236820530U)
        {
          if (stringHash <= 2103090580U)
          {
            if ((int) stringHash != 1992359646)
            {
              if ((int) stringHash != 2082305658)
              {
                if ((int) stringHash != 2103090580 || !(key == "grandpaNight"))
                  return;
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(0, 1453, 639, 176), 9999f, 1, 999999, new Vector2(0.0f, 1f) * (float) Game1.tileSize, false, false, 0.9f, 0.0f, Color.Cyan, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, true)
                {
                  alpha = 0.01f,
                  alphaFade = -1f / 500f,
                  local = true
                });
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(0, 1453, 639, 176), 9999f, 1, 999999, new Vector2(0.0f, (float) (176 * Game1.pixelZoom + Game1.tileSize)), false, true, 0.9f, 0.0f, Color.Blue, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, true)
                {
                  alpha = 0.01f,
                  alphaFade = -1f / 500f,
                  local = true
                });
              }
              else
              {
                if (!(key == "EmilySongBackLights"))
                  return;
                this.aboveMapSprites = new List<TemporaryAnimatedSprite>();
                Viewport viewport;
                for (int index = 0; index < 5; ++index)
                {
                  int num1 = 0;
                  while (true)
                  {
                    int num2 = num1;
                    viewport = Game1.graphics.GraphicsDevice.Viewport;
                    int num3 = viewport.Height + 12 * Game1.pixelZoom;
                    if (num2 < num3)
                    {
                      List<TemporaryAnimatedSprite> aboveMapSprites = this.aboveMapSprites;
                      Texture2D mouseCursors = Game1.mouseCursors;
                      Microsoft.Xna.Framework.Rectangle sourceRect = new Microsoft.Xna.Framework.Rectangle(681, 1890, 18, 12);
                      double num4 = 42241.0;
                      int animationLength = 1;
                      int numberOfLoops = 1;
                      int num5 = index + 1;
                      viewport = Game1.graphics.GraphicsDevice.Viewport;
                      int width = viewport.Width;
                      int num6 = num5 * width / 5;
                      viewport = Game1.graphics.GraphicsDevice.Viewport;
                      int num7 = viewport.Width / 7;
                      Vector2 position = new Vector2((float) (num6 - num7), (float) (-6 * Game1.pixelZoom + num1));
                      int num8 = 0;
                      int num9 = 0;
                      double num10 = 0.00999999977648258;
                      double num11 = 0.0;
                      Color white = Color.White;
                      double pixelZoom = (double) Game1.pixelZoom;
                      double num12 = 0.0;
                      double num13 = 0.0;
                      double num14 = 0.0;
                      int num15 = 0;
                      TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(mouseCursors, sourceRect, (float) num4, animationLength, numberOfLoops, position, num8 != 0, num9 != 0, (float) num10, (float) num11, white, (float) pixelZoom, (float) num12, (float) num13, (float) num14, num15 != 0);
                      temporaryAnimatedSprite.xPeriodic = true;
                      temporaryAnimatedSprite.xPeriodicLoopTime = 1760f;
                      double num16 = (double) (Game1.tileSize * 2 + num1 / 12 * Game1.pixelZoom);
                      temporaryAnimatedSprite.xPeriodicRange = (float) num16;
                      int num17 = index * 100 + num1 / 4;
                      temporaryAnimatedSprite.delayBeforeAnimationStart = num17;
                      int num18 = 1;
                      temporaryAnimatedSprite.local = num18 != 0;
                      aboveMapSprites.Add(temporaryAnimatedSprite);
                      num1 += 12 * Game1.pixelZoom;
                    }
                    else
                      break;
                  }
                }
                for (int index1 = 0; index1 < 27; ++index1)
                {
                  int num1 = 0;
                  Random random = Game1.random;
                  int tileSize = Game1.tileSize;
                  viewport = Game1.graphics.GraphicsDevice.Viewport;
                  int maxValue = viewport.Height - Game1.tileSize;
                  int num2 = random.Next(tileSize, maxValue);
                  int num3 = Game1.random.Next(800, 2000);
                  int num4 = Game1.random.Next(Game1.tileSize / 2, Game1.tileSize);
                  bool flag = Game1.random.NextDouble() < 0.25;
                  int num5 = Game1.random.Next(-6, -3);
                  for (int index2 = 0; index2 < 8; ++index2)
                  {
                    List<TemporaryAnimatedSprite> aboveMapSprites = this.aboveMapSprites;
                    Texture2D mouseCursors = Game1.mouseCursors;
                    Microsoft.Xna.Framework.Rectangle sourceRect = new Microsoft.Xna.Framework.Rectangle(616 + num1 * 10, 1891, 10, 10);
                    double num6 = 42241.0;
                    int animationLength = 1;
                    int numberOfLoops = 1;
                    viewport = Game1.graphics.GraphicsDevice.Viewport;
                    Vector2 position = new Vector2((float) viewport.Width, (float) num2);
                    int num7 = 0;
                    int num8 = 0;
                    double num9 = 0.00999999977648258;
                    double num10 = 0.0;
                    Color color = Color.White * (float) (1.0 - (double) index2 * 0.109999999403954);
                    double pixelZoom = (double) Game1.pixelZoom;
                    double num11 = 0.0;
                    double num12 = 0.0;
                    double num13 = 0.0;
                    int num14 = 0;
                    TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(mouseCursors, sourceRect, (float) num6, animationLength, numberOfLoops, position, num7 != 0, num8 != 0, (float) num9, (float) num10, color, (float) pixelZoom, (float) num11, (float) num12, (float) num13, num14 != 0);
                    temporaryAnimatedSprite.yPeriodic = true;
                    Vector2 vector2 = new Vector2((float) num5, 0.0f);
                    temporaryAnimatedSprite.motion = vector2;
                    double num15 = (double) num3;
                    temporaryAnimatedSprite.yPeriodicLoopTime = (float) num15;
                    int num16 = flag ? 1 : 0;
                    temporaryAnimatedSprite.pulse = num16 != 0;
                    double num17 = 440.0;
                    temporaryAnimatedSprite.pulseTime = (float) num17;
                    double num18 = 1.5;
                    temporaryAnimatedSprite.pulseAmount = (float) num18;
                    double num19 = (double) num4;
                    temporaryAnimatedSprite.yPeriodicRange = (float) num19;
                    int num20 = 14000 + index1 * 900 + index2 * 100;
                    temporaryAnimatedSprite.delayBeforeAnimationStart = num20;
                    int num21 = 1;
                    temporaryAnimatedSprite.local = num21 != 0;
                    aboveMapSprites.Add(temporaryAnimatedSprite);
                  }
                }
                for (int index = 0; index < 15; ++index)
                {
                  int num1 = 0;
                  Random random = Game1.random;
                  viewport = Game1.graphics.GraphicsDevice.Viewport;
                  int maxValue = viewport.Width - Game1.tileSize * 2;
                  int num2 = random.Next(maxValue);
                  viewport = Game1.graphics.GraphicsDevice.Viewport;
                  int height = viewport.Height;
                  while (height >= -Game1.tileSize)
                  {
                    List<TemporaryAnimatedSprite> aboveMapSprites = this.aboveMapSprites;
                    TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(597, 1888, 16, 16), 99999f, 1, 99999, new Vector2((float) num2, (float) height), false, false, 1f, 0.02f, Color.White, (float) Game1.pixelZoom, 0.0f, -1.570796f, 0.0f, false);
                    temporaryAnimatedSprite.delayBeforeAnimationStart = 27500 + index * 880 + num1 * 25;
                    int num3 = 1;
                    temporaryAnimatedSprite.local = num3 != 0;
                    aboveMapSprites.Add(temporaryAnimatedSprite);
                    ++num1;
                    height -= Game1.tileSize * 3 / 4;
                  }
                }
                for (int index = 0; index < 120; ++index)
                {
                  List<TemporaryAnimatedSprite> aboveMapSprites = this.aboveMapSprites;
                  Texture2D mouseCursors = Game1.mouseCursors;
                  Microsoft.Xna.Framework.Rectangle sourceRect = new Microsoft.Xna.Framework.Rectangle(626 + index / 28 * 10, 1891, 10, 10);
                  double num1 = 2000.0;
                  int animationLength = 1;
                  int numberOfLoops = 1;
                  Random random1 = Game1.random;
                  viewport = Game1.graphics.GraphicsDevice.Viewport;
                  int width = viewport.Width;
                  double num2 = (double) random1.Next(width);
                  Random random2 = Game1.random;
                  viewport = Game1.graphics.GraphicsDevice.Viewport;
                  int height = viewport.Height;
                  double num3 = (double) random2.Next(height);
                  Vector2 position = new Vector2((float) num2, (float) num3);
                  int num4 = 0;
                  int num5 = 0;
                  double num6 = 0.00999999977648258;
                  double num7 = 0.0;
                  Color white = Color.White;
                  double num8 = 0.100000001490116;
                  double num9 = 0.0;
                  double num10 = 0.0;
                  double num11 = 0.0;
                  int num12 = 0;
                  TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(mouseCursors, sourceRect, (float) num1, animationLength, numberOfLoops, position, num4 != 0, num5 != 0, (float) num6, (float) num7, white, (float) num8, (float) num9, (float) num10, (float) num11, num12 != 0);
                  temporaryAnimatedSprite.motion = new Vector2(0.0f, -2f);
                  temporaryAnimatedSprite.alphaFade = 1f / 500f;
                  temporaryAnimatedSprite.scaleChange = 0.5f;
                  temporaryAnimatedSprite.scaleChangeChange = -0.0085f;
                  temporaryAnimatedSprite.delayBeforeAnimationStart = 27500 + index * 110;
                  int num13 = 1;
                  temporaryAnimatedSprite.local = num13 != 0;
                  aboveMapSprites.Add(temporaryAnimatedSprite);
                }
              }
            }
            else
            {
              if (!(key == "shaneThrowCan"))
                return;
              location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(542, 1893, 4, 6), 99999f, 1, 99999, new Vector2(103f, 95f) * (float) Game1.tileSize + new Vector2(0.0f, 4f) * (float) Game1.pixelZoom, false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
              {
                motion = new Vector2(0.0f, -4f),
                acceleration = new Vector2(0.0f, 0.25f),
                rotationChange = (float) Math.PI / 128f
              });
              Game1.playSound("shwip");
            }
          }
          else if ((int) stringHash != 2140977878)
          {
            if ((int) stringHash != -2096988464)
            {
              if ((int) stringHash != -2058146766 || !(key == "shaneHospital"))
                return;
              location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(533, 1864, 19, 10), 99999f, 1, 99999, new Vector2(20f, 3f) * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 4), (float) (3 * Game1.pixelZoom)), false, false, 0.01f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
              {
                id = 999f
              });
            }
            else
            {
              if (!(key == "EmilyCamping"))
                return;
              this.showGroundObjects = false;
              location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(644, 1578, 59, 53), 999999f, 1, 99999, new Vector2(26f, 9f) * (float) Game1.tileSize + new Vector2((float) (-Game1.pixelZoom * 4), 0.0f), false, false, (float) (9 * Game1.tileSize + 53 * Game1.pixelZoom) / 10000f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
              {
                id = 999f
              });
              location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(675, 1299, 29, 24), 999999f, 1, 99999, new Vector2(27f, 14f) * (float) Game1.tileSize, false, false, 1f / 1000f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
              {
                id = 99f
              });
              List<TemporaryAnimatedSprite> temporarySprites = location.TemporarySprites;
              TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(276, 1985, 12, 11), new Vector2(27f, 14f) * (float) Game1.tileSize + new Vector2(8f, 4f) * (float) Game1.pixelZoom, false, 0.0f, Color.White);
              temporaryAnimatedSprite.interval = 50f;
              temporaryAnimatedSprite.totalNumberOfLoops = 99999;
              temporaryAnimatedSprite.animationLength = 4;
              temporaryAnimatedSprite.light = true;
              temporaryAnimatedSprite.lightID = 666;
              temporaryAnimatedSprite.id = 666f;
              temporaryAnimatedSprite.lightRadius = 2f;
              double pixelZoom = (double) Game1.pixelZoom;
              temporaryAnimatedSprite.scale = (float) pixelZoom;
              double num = 0.00999999977648258;
              temporaryAnimatedSprite.layerDepth = (float) num;
              temporarySprites.Add(temporaryAnimatedSprite);
              Game1.currentLightSources.Add(new LightSource(4, new Vector2(27f, 14f) * (float) Game1.tileSize, 2f));
              location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(585, 1846, 26, 22), 999999f, 1, 99999, new Vector2(25f, 12f) * (float) Game1.tileSize + new Vector2((float) (-Game1.tileSize / 2), 0.0f), false, false, 1f / 1000f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
              {
                id = 96f
              });
              AmbientLocationSounds.addSound(new Vector2(27f, 14f), 1);
            }
          }
          else
          {
            if (!(key == "ClothingTherapy"))
              return;
            location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(644, 1405, 28, 46), 999999f, 1, 99999, new Vector2(5f, 6f) * (float) Game1.tileSize + new Vector2((float) (-8 * Game1.pixelZoom), (float) (-36 * Game1.pixelZoom)), false, false, (float) (6 * Game1.tileSize + 10 * Game1.pixelZoom) / 10000f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
            {
              id = 999f
            });
          }
        }
        else if (stringHash <= 2303195829U)
        {
          if ((int) stringHash != -2040595911)
          {
            if ((int) stringHash != -2014990251)
            {
              if ((int) stringHash != -1991771467 || !(key == "dickGlitter"))
                return;
              location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(432, 1435, 16, 16), 100f, 6, 99999, new Vector2(47f, 8f) * (float) Game1.tileSize, false, false, 1f, 0.0f, Color.White, (float) (Game1.pixelZoom / 2), 0.0f, 0.0f, 0.0f, false));
              location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(432, 1435, 16, 16), 100f, 6, 99999, new Vector2(47f, 8f) * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), 0.0f), false, false, 1f, 0.0f, Color.White, (float) (Game1.pixelZoom / 2), 0.0f, 0.0f, 0.0f, false)
              {
                delayBeforeAnimationStart = 200
              });
              location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(432, 1435, 16, 16), 100f, 6, 99999, new Vector2(47f, 8f) * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2)), false, false, 1f, 0.0f, Color.White, (float) (Game1.pixelZoom / 2), 0.0f, 0.0f, 0.0f, false)
              {
                delayBeforeAnimationStart = 300
              });
              location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(432, 1435, 16, 16), 100f, 6, 99999, new Vector2(47f, 8f) * (float) Game1.tileSize + new Vector2(0.0f, (float) (Game1.tileSize / 2)), false, false, 1f, 0.0f, Color.White, (float) (Game1.pixelZoom / 2), 0.0f, 0.0f, 0.0f, false)
              {
                delayBeforeAnimationStart = 100
              });
              location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(432, 1435, 16, 16), 100f, 6, 99999, new Vector2(47f, 8f) * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 4), (float) (Game1.tileSize / 4)), false, false, 1f, 0.0f, Color.White, (float) (Game1.pixelZoom / 2), 0.0f, 0.0f, 0.0f, false)
              {
                delayBeforeAnimationStart = 400
              });
            }
            else
            {
              if (!(key == "abbyAtLake"))
                return;
              location.TemporarySprites.Add(new TemporaryAnimatedSprite(735, 999999f, 1, 0, new Vector2(48f, 30f) * (float) Game1.tileSize, false, false)
              {
                light = true,
                lightRadius = 2f
              });
              List<TemporaryAnimatedSprite> temporarySprites1 = location.TemporarySprites;
              TemporaryAnimatedSprite temporaryAnimatedSprite1 = new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(232, 328, 4, 4), 9999999f, 1, 0, new Vector2(48f, 30f) * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), 0.0f), false, false, 1f, 0.0f, Color.White, 1f, 0.0f, 0.0f, 0.0f, false);
              temporaryAnimatedSprite1.light = true;
              temporaryAnimatedSprite1.lightRadius = 0.2f;
              temporaryAnimatedSprite1.xPeriodic = true;
              temporaryAnimatedSprite1.yPeriodic = true;
              temporaryAnimatedSprite1.xPeriodicLoopTime = 2000f;
              temporaryAnimatedSprite1.yPeriodicLoopTime = 1600f;
              double num1 = (double) (Game1.tileSize / 2);
              temporaryAnimatedSprite1.xPeriodicRange = (float) num1;
              double num2 = (double) (Game1.tileSize / 3);
              temporaryAnimatedSprite1.yPeriodicRange = (float) num2;
              temporarySprites1.Add(temporaryAnimatedSprite1);
              List<TemporaryAnimatedSprite> temporarySprites2 = location.TemporarySprites;
              TemporaryAnimatedSprite temporaryAnimatedSprite2 = new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(232, 328, 4, 4), 9999999f, 1, 0, new Vector2(48f, 30f) * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), 0.0f), false, false, 1f, 0.0f, Color.White, 1f, 0.0f, 0.0f, 0.0f, false);
              temporaryAnimatedSprite2.light = true;
              temporaryAnimatedSprite2.lightRadius = 0.2f;
              temporaryAnimatedSprite2.xPeriodic = true;
              temporaryAnimatedSprite2.yPeriodic = true;
              temporaryAnimatedSprite2.xPeriodicLoopTime = 1000f;
              temporaryAnimatedSprite2.yPeriodicLoopTime = 1600f;
              double num3 = (double) (Game1.tileSize / 4);
              temporaryAnimatedSprite2.xPeriodicRange = (float) num3;
              double num4 = (double) (Game1.tileSize / 3);
              temporaryAnimatedSprite2.yPeriodicRange = (float) num4;
              temporarySprites2.Add(temporaryAnimatedSprite2);
              List<TemporaryAnimatedSprite> temporarySprites3 = location.TemporarySprites;
              TemporaryAnimatedSprite temporaryAnimatedSprite3 = new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(232, 328, 4, 4), 9999999f, 1, 0, new Vector2(48f, 30f) * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), 0.0f), false, false, 1f, 0.0f, Color.White, 1f, 0.0f, 0.0f, 0.0f, false);
              temporaryAnimatedSprite3.light = true;
              temporaryAnimatedSprite3.lightRadius = 0.2f;
              temporaryAnimatedSprite3.xPeriodic = true;
              temporaryAnimatedSprite3.yPeriodic = true;
              temporaryAnimatedSprite3.xPeriodicLoopTime = 2400f;
              temporaryAnimatedSprite3.yPeriodicLoopTime = 2800f;
              double num5 = (double) (Game1.tileSize / 3);
              temporaryAnimatedSprite3.xPeriodicRange = (float) num5;
              double num6 = (double) (Game1.tileSize / 2);
              temporaryAnimatedSprite3.yPeriodicRange = (float) num6;
              temporarySprites3.Add(temporaryAnimatedSprite3);
              List<TemporaryAnimatedSprite> temporarySprites4 = location.TemporarySprites;
              TemporaryAnimatedSprite temporaryAnimatedSprite4 = new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(232, 328, 4, 4), 9999999f, 1, 0, new Vector2(48f, 30f) * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), 0.0f), false, false, 1f, 0.0f, Color.White, 1f, 0.0f, 0.0f, 0.0f, false);
              temporaryAnimatedSprite4.light = true;
              temporaryAnimatedSprite4.lightRadius = 0.2f;
              temporaryAnimatedSprite4.xPeriodic = true;
              temporaryAnimatedSprite4.yPeriodic = true;
              temporaryAnimatedSprite4.xPeriodicLoopTime = 2000f;
              temporaryAnimatedSprite4.yPeriodicLoopTime = 2400f;
              double num7 = (double) (Game1.tileSize / 4);
              temporaryAnimatedSprite4.xPeriodicRange = (float) num7;
              double num8 = (double) (Game1.tileSize / 4);
              temporaryAnimatedSprite4.yPeriodicRange = (float) num8;
              temporarySprites4.Add(temporaryAnimatedSprite4);
              List<TemporaryAnimatedSprite> temporarySprites5 = location.TemporarySprites;
              TemporaryAnimatedSprite temporaryAnimatedSprite5 = new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(232, 328, 4, 4), 9999999f, 1, 0, new Vector2(66f, 34f) * (float) Game1.tileSize + new Vector2((float) (-Game1.tileSize / 2), 0.0f), false, false, 1f, 0.0f, Color.White, 1f, 0.0f, 0.0f, 0.0f, false);
              temporaryAnimatedSprite5.lightcolor = Color.Orange;
              temporaryAnimatedSprite5.light = true;
              temporaryAnimatedSprite5.lightRadius = 0.2f;
              temporaryAnimatedSprite5.xPeriodic = true;
              temporaryAnimatedSprite5.yPeriodic = true;
              temporaryAnimatedSprite5.xPeriodicLoopTime = 2000f;
              temporaryAnimatedSprite5.yPeriodicLoopTime = 2600f;
              double num9 = (double) (Game1.tileSize / 3);
              temporaryAnimatedSprite5.xPeriodicRange = (float) num9;
              double num10 = (double) (Game1.tileSize * 3 / 4);
              temporaryAnimatedSprite5.yPeriodicRange = (float) num10;
              temporarySprites5.Add(temporaryAnimatedSprite5);
              List<TemporaryAnimatedSprite> temporarySprites6 = location.TemporarySprites;
              TemporaryAnimatedSprite temporaryAnimatedSprite6 = new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(232, 328, 4, 4), 9999999f, 1, 0, new Vector2(66f, 34f) * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), 0.0f), false, false, 1f, 0.0f, Color.White, 1f, 0.0f, 0.0f, 0.0f, false);
              temporaryAnimatedSprite6.lightcolor = Color.Orange;
              temporaryAnimatedSprite6.light = true;
              temporaryAnimatedSprite6.lightRadius = 0.2f;
              temporaryAnimatedSprite6.xPeriodic = true;
              temporaryAnimatedSprite6.yPeriodic = true;
              temporaryAnimatedSprite6.xPeriodicLoopTime = 2000f;
              temporaryAnimatedSprite6.yPeriodicLoopTime = 2600f;
              double num11 = (double) (Game1.tileSize / 2);
              temporaryAnimatedSprite6.xPeriodicRange = (float) num11;
              double num12 = (double) (Game1.tileSize / 3);
              temporaryAnimatedSprite6.yPeriodicRange = (float) num12;
              temporarySprites6.Add(temporaryAnimatedSprite6);
              List<TemporaryAnimatedSprite> temporarySprites7 = location.TemporarySprites;
              TemporaryAnimatedSprite temporaryAnimatedSprite7 = new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(232, 328, 4, 4), 9999999f, 1, 0, new Vector2(66f, 34f) * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2)), false, false, 1f, 0.0f, Color.White, 1f, 0.0f, 0.0f, 0.0f, false);
              temporaryAnimatedSprite7.lightcolor = Color.Orange;
              temporaryAnimatedSprite7.light = true;
              temporaryAnimatedSprite7.lightRadius = 0.2f;
              temporaryAnimatedSprite7.xPeriodic = true;
              temporaryAnimatedSprite7.yPeriodic = true;
              temporaryAnimatedSprite7.xPeriodicLoopTime = 4000f;
              temporaryAnimatedSprite7.yPeriodicLoopTime = 5000f;
              double num13 = (double) (Game1.tileSize * 2 / 3);
              temporaryAnimatedSprite7.xPeriodicRange = (float) num13;
              double num14 = (double) (Game1.tileSize / 2);
              temporaryAnimatedSprite7.yPeriodicRange = (float) num14;
              temporarySprites7.Add(temporaryAnimatedSprite7);
              List<TemporaryAnimatedSprite> temporarySprites8 = location.TemporarySprites;
              TemporaryAnimatedSprite temporaryAnimatedSprite8 = new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(232, 328, 4, 4), 9999999f, 1, 0, new Vector2(66f, 34f) * (float) Game1.tileSize + new Vector2(0.0f, (float) (-Game1.tileSize / 2)), false, false, 1f, 0.0f, Color.White, 1f, 0.0f, 0.0f, 0.0f, false);
              temporaryAnimatedSprite8.lightcolor = Color.Orange;
              temporaryAnimatedSprite8.light = true;
              temporaryAnimatedSprite8.lightRadius = 0.2f;
              temporaryAnimatedSprite8.xPeriodic = true;
              temporaryAnimatedSprite8.yPeriodic = true;
              temporaryAnimatedSprite8.xPeriodicLoopTime = 4000f;
              temporaryAnimatedSprite8.yPeriodicLoopTime = 5500f;
              double num15 = (double) (Game1.tileSize / 2);
              temporaryAnimatedSprite8.xPeriodicRange = (float) num15;
              double num16 = (double) (Game1.tileSize / 2);
              temporaryAnimatedSprite8.yPeriodicRange = (float) num16;
              temporarySprites8.Add(temporaryAnimatedSprite8);
              List<TemporaryAnimatedSprite> temporarySprites9 = location.TemporarySprites;
              TemporaryAnimatedSprite temporaryAnimatedSprite9 = new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(232, 328, 4, 4), 9999999f, 1, 0, new Vector2(69f, 28f) * (float) Game1.tileSize + new Vector2((float) (-Game1.tileSize / 2), 0.0f), false, false, 1f, 0.0f, Color.White, 1f, 0.0f, 0.0f, 0.0f, false);
              temporaryAnimatedSprite9.lightcolor = Color.Orange;
              temporaryAnimatedSprite9.light = true;
              temporaryAnimatedSprite9.lightRadius = 0.2f;
              temporaryAnimatedSprite9.xPeriodic = true;
              temporaryAnimatedSprite9.yPeriodic = true;
              temporaryAnimatedSprite9.xPeriodicLoopTime = 2400f;
              temporaryAnimatedSprite9.yPeriodicLoopTime = 3600f;
              double num17 = (double) (Game1.tileSize / 2);
              temporaryAnimatedSprite9.xPeriodicRange = (float) num17;
              double num18 = (double) (Game1.tileSize / 3);
              temporaryAnimatedSprite9.yPeriodicRange = (float) num18;
              temporarySprites9.Add(temporaryAnimatedSprite9);
              List<TemporaryAnimatedSprite> temporarySprites10 = location.TemporarySprites;
              TemporaryAnimatedSprite temporaryAnimatedSprite10 = new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(232, 328, 4, 4), 9999999f, 1, 0, new Vector2(69f, 28f) * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), 0.0f), false, false, 1f, 0.0f, Color.White, 1f, 0.0f, 0.0f, 0.0f, false);
              temporaryAnimatedSprite10.lightcolor = Color.Orange;
              temporaryAnimatedSprite10.light = true;
              temporaryAnimatedSprite10.lightRadius = 0.2f;
              temporaryAnimatedSprite10.xPeriodic = true;
              temporaryAnimatedSprite10.yPeriodic = true;
              temporaryAnimatedSprite10.xPeriodicLoopTime = 2500f;
              temporaryAnimatedSprite10.yPeriodicLoopTime = 3600f;
              double num19 = (double) (Game1.tileSize * 2 / 3);
              temporaryAnimatedSprite10.xPeriodicRange = (float) num19;
              double num20 = (double) (Game1.tileSize * 4 / 5);
              temporaryAnimatedSprite10.yPeriodicRange = (float) num20;
              temporarySprites10.Add(temporaryAnimatedSprite10);
              List<TemporaryAnimatedSprite> temporarySprites11 = location.TemporarySprites;
              TemporaryAnimatedSprite temporaryAnimatedSprite11 = new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(232, 328, 4, 4), 9999999f, 1, 0, new Vector2(69f, 28f) * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2)), false, false, 1f, 0.0f, Color.White, 1f, 0.0f, 0.0f, 0.0f, false);
              temporaryAnimatedSprite11.lightcolor = Color.Orange;
              temporaryAnimatedSprite11.light = true;
              temporaryAnimatedSprite11.lightRadius = 0.2f;
              temporaryAnimatedSprite11.xPeriodic = true;
              temporaryAnimatedSprite11.yPeriodic = true;
              temporaryAnimatedSprite11.xPeriodicLoopTime = 4500f;
              temporaryAnimatedSprite11.yPeriodicLoopTime = 3000f;
              double num21 = (double) (Game1.tileSize / 3);
              temporaryAnimatedSprite11.xPeriodicRange = (float) num21;
              double num22 = (double) (Game1.tileSize / 2);
              temporaryAnimatedSprite11.yPeriodicRange = (float) num22;
              temporarySprites11.Add(temporaryAnimatedSprite11);
              List<TemporaryAnimatedSprite> temporarySprites12 = location.TemporarySprites;
              TemporaryAnimatedSprite temporaryAnimatedSprite12 = new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(232, 328, 4, 4), 9999999f, 1, 0, new Vector2(69f, 28f) * (float) Game1.tileSize + new Vector2(0.0f, (float) (-Game1.tileSize / 2)), false, false, 1f, 0.0f, Color.White, 1f, 0.0f, 0.0f, 0.0f, false);
              temporaryAnimatedSprite12.lightcolor = Color.Orange;
              temporaryAnimatedSprite12.light = true;
              temporaryAnimatedSprite12.lightRadius = 0.2f;
              temporaryAnimatedSprite12.xPeriodic = true;
              temporaryAnimatedSprite12.yPeriodic = true;
              temporaryAnimatedSprite12.xPeriodicLoopTime = 5000f;
              temporaryAnimatedSprite12.yPeriodicLoopTime = 4500f;
              double tileSize = (double) Game1.tileSize;
              temporaryAnimatedSprite12.xPeriodicRange = (float) tileSize;
              double num23 = (double) (Game1.tileSize * 3 / 4);
              temporaryAnimatedSprite12.yPeriodicRange = (float) num23;
              temporarySprites12.Add(temporaryAnimatedSprite12);
              List<TemporaryAnimatedSprite> temporarySprites13 = location.TemporarySprites;
              TemporaryAnimatedSprite temporaryAnimatedSprite13 = new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(232, 328, 4, 4), 9999999f, 1, 0, new Vector2(72f, 33f) * (float) Game1.tileSize + new Vector2((float) (-Game1.tileSize / 2), 0.0f), false, false, 1f, 0.0f, Color.White, 1f, 0.0f, 0.0f, 0.0f, false);
              temporaryAnimatedSprite13.lightcolor = Color.Orange;
              temporaryAnimatedSprite13.light = true;
              temporaryAnimatedSprite13.lightRadius = 0.2f;
              temporaryAnimatedSprite13.xPeriodic = true;
              temporaryAnimatedSprite13.yPeriodic = true;
              temporaryAnimatedSprite13.xPeriodicLoopTime = 2000f;
              temporaryAnimatedSprite13.yPeriodicLoopTime = 3000f;
              double num24 = (double) (Game1.tileSize / 2);
              temporaryAnimatedSprite13.xPeriodicRange = (float) num24;
              double num25 = (double) (Game1.tileSize / 3);
              temporaryAnimatedSprite13.yPeriodicRange = (float) num25;
              temporarySprites13.Add(temporaryAnimatedSprite13);
              List<TemporaryAnimatedSprite> temporarySprites14 = location.TemporarySprites;
              TemporaryAnimatedSprite temporaryAnimatedSprite14 = new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(232, 328, 4, 4), 9999999f, 1, 0, new Vector2(72f, 33f) * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), 0.0f), false, false, 1f, 0.0f, Color.White, 1f, 0.0f, 0.0f, 0.0f, false);
              temporaryAnimatedSprite14.lightcolor = Color.Orange;
              temporaryAnimatedSprite14.light = true;
              temporaryAnimatedSprite14.lightRadius = 0.2f;
              temporaryAnimatedSprite14.xPeriodic = true;
              temporaryAnimatedSprite14.yPeriodic = true;
              temporaryAnimatedSprite14.xPeriodicLoopTime = 2900f;
              temporaryAnimatedSprite14.yPeriodicLoopTime = 3200f;
              double num26 = (double) (Game1.tileSize / 3);
              temporaryAnimatedSprite14.xPeriodicRange = (float) num26;
              double num27 = (double) (Game1.tileSize / 2);
              temporaryAnimatedSprite14.yPeriodicRange = (float) num27;
              temporarySprites14.Add(temporaryAnimatedSprite14);
              List<TemporaryAnimatedSprite> temporarySprites15 = location.TemporarySprites;
              TemporaryAnimatedSprite temporaryAnimatedSprite15 = new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(232, 328, 4, 4), 9999999f, 1, 0, new Vector2(72f, 33f) * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2)), false, false, 1f, 0.0f, Color.White, 1f, 0.0f, 0.0f, 0.0f, false);
              temporaryAnimatedSprite15.lightcolor = Color.Orange;
              temporaryAnimatedSprite15.light = true;
              temporaryAnimatedSprite15.lightRadius = 0.2f;
              temporaryAnimatedSprite15.xPeriodic = true;
              temporaryAnimatedSprite15.yPeriodic = true;
              temporaryAnimatedSprite15.xPeriodicLoopTime = 4200f;
              temporaryAnimatedSprite15.yPeriodicLoopTime = 3300f;
              double num28 = (double) (Game1.tileSize / 4);
              temporaryAnimatedSprite15.xPeriodicRange = (float) num28;
              double num29 = (double) (Game1.tileSize / 2);
              temporaryAnimatedSprite15.yPeriodicRange = (float) num29;
              temporarySprites15.Add(temporaryAnimatedSprite15);
              List<TemporaryAnimatedSprite> temporarySprites16 = location.TemporarySprites;
              TemporaryAnimatedSprite temporaryAnimatedSprite16 = new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(232, 328, 4, 4), 9999999f, 1, 0, new Vector2(72f, 33f) * (float) Game1.tileSize + new Vector2(0.0f, (float) (-Game1.tileSize / 2)), false, false, 1f, 0.0f, Color.White, 1f, 0.0f, 0.0f, 0.0f, false);
              temporaryAnimatedSprite16.lightcolor = Color.Orange;
              temporaryAnimatedSprite16.light = true;
              temporaryAnimatedSprite16.lightRadius = 0.2f;
              temporaryAnimatedSprite16.xPeriodic = true;
              temporaryAnimatedSprite16.yPeriodic = true;
              temporaryAnimatedSprite16.xPeriodicLoopTime = 5100f;
              temporaryAnimatedSprite16.yPeriodicLoopTime = 4000f;
              double num30 = (double) (Game1.tileSize / 2);
              temporaryAnimatedSprite16.xPeriodicRange = (float) num30;
              double num31 = (double) (Game1.tileSize / 4);
              temporaryAnimatedSprite16.yPeriodicRange = (float) num31;
              temporarySprites16.Add(temporaryAnimatedSprite16);
            }
          }
          else
          {
            if (!(key == "junimoCage"))
              return;
            location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(325, 1977, 18, 19), 60f, 3, 999999, new Vector2(10f, 17f) * (float) Game1.tileSize + new Vector2(0.0f, (float) -Game1.pixelZoom), false, false, 0.0f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
            {
              light = true,
              lightRadius = 1f,
              lightcolor = Color.Black,
              id = 1f,
              shakeIntensity = 0.0f
            });
            List<TemporaryAnimatedSprite> temporarySprites1 = location.TemporarySprites;
            TemporaryAnimatedSprite temporaryAnimatedSprite1 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(379, 1991, 5, 5), 9999f, 1, 999999, new Vector2(10f, 17f) * (float) Game1.tileSize + new Vector2(0.0f, (float) -Game1.pixelZoom), false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
            temporaryAnimatedSprite1.light = true;
            temporaryAnimatedSprite1.lightRadius = 0.5f;
            temporaryAnimatedSprite1.lightcolor = Color.Black;
            temporaryAnimatedSprite1.id = 1f;
            temporaryAnimatedSprite1.xPeriodic = true;
            temporaryAnimatedSprite1.xPeriodicLoopTime = 2000f;
            double num1 = (double) (6 * Game1.pixelZoom);
            temporaryAnimatedSprite1.xPeriodicRange = (float) num1;
            int num2 = 1;
            temporaryAnimatedSprite1.yPeriodic = num2 != 0;
            double num3 = 2000.0;
            temporaryAnimatedSprite1.yPeriodicLoopTime = (float) num3;
            double num4 = (double) (6 * Game1.pixelZoom);
            temporaryAnimatedSprite1.yPeriodicRange = (float) num4;
            temporarySprites1.Add(temporaryAnimatedSprite1);
            List<TemporaryAnimatedSprite> temporarySprites2 = location.TemporarySprites;
            TemporaryAnimatedSprite temporaryAnimatedSprite2 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(379, 1991, 5, 5), 9999f, 1, 999999, new Vector2(10f, 17f) * (float) Game1.tileSize + new Vector2((float) (18 * Game1.pixelZoom), (float) -Game1.pixelZoom), false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
            temporaryAnimatedSprite2.light = true;
            temporaryAnimatedSprite2.lightRadius = 0.5f;
            temporaryAnimatedSprite2.lightcolor = Color.Black;
            temporaryAnimatedSprite2.id = 1f;
            temporaryAnimatedSprite2.xPeriodic = true;
            temporaryAnimatedSprite2.xPeriodicLoopTime = 2000f;
            double num5 = (double) (-6 * Game1.pixelZoom);
            temporaryAnimatedSprite2.xPeriodicRange = (float) num5;
            int num6 = 1;
            temporaryAnimatedSprite2.yPeriodic = num6 != 0;
            double num7 = 2000.0;
            temporaryAnimatedSprite2.yPeriodicLoopTime = (float) num7;
            double num8 = (double) (6 * Game1.pixelZoom);
            temporaryAnimatedSprite2.yPeriodicRange = (float) num8;
            int num9 = 250;
            temporaryAnimatedSprite2.delayBeforeAnimationStart = num9;
            temporarySprites2.Add(temporaryAnimatedSprite2);
            List<TemporaryAnimatedSprite> temporarySprites3 = location.TemporarySprites;
            TemporaryAnimatedSprite temporaryAnimatedSprite3 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(379, 1991, 5, 5), 9999f, 1, 999999, new Vector2(10f, 17f) * (float) Game1.tileSize + new Vector2(0.0f, (float) (13 * Game1.pixelZoom)), false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
            temporaryAnimatedSprite3.light = true;
            temporaryAnimatedSprite3.lightRadius = 0.5f;
            temporaryAnimatedSprite3.lightcolor = Color.Black;
            temporaryAnimatedSprite3.id = 1f;
            temporaryAnimatedSprite3.xPeriodic = true;
            temporaryAnimatedSprite3.xPeriodicLoopTime = 2000f;
            double num10 = (double) (-6 * Game1.pixelZoom);
            temporaryAnimatedSprite3.xPeriodicRange = (float) num10;
            int num11 = 1;
            temporaryAnimatedSprite3.yPeriodic = num11 != 0;
            double num12 = 2000.0;
            temporaryAnimatedSprite3.yPeriodicLoopTime = (float) num12;
            double num13 = (double) (6 * Game1.pixelZoom);
            temporaryAnimatedSprite3.yPeriodicRange = (float) num13;
            int num14 = 450;
            temporaryAnimatedSprite3.delayBeforeAnimationStart = num14;
            temporarySprites3.Add(temporaryAnimatedSprite3);
            List<TemporaryAnimatedSprite> temporarySprites4 = location.TemporarySprites;
            TemporaryAnimatedSprite temporaryAnimatedSprite4 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(379, 1991, 5, 5), 9999f, 1, 999999, new Vector2(10f, 17f) * (float) Game1.tileSize + new Vector2((float) (18 * Game1.pixelZoom), (float) (13 * Game1.pixelZoom)), false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
            temporaryAnimatedSprite4.light = true;
            temporaryAnimatedSprite4.lightRadius = 0.5f;
            temporaryAnimatedSprite4.lightcolor = Color.Black;
            temporaryAnimatedSprite4.id = 1f;
            temporaryAnimatedSprite4.xPeriodic = true;
            temporaryAnimatedSprite4.xPeriodicLoopTime = 2000f;
            double num15 = (double) (6 * Game1.pixelZoom);
            temporaryAnimatedSprite4.xPeriodicRange = (float) num15;
            int num16 = 1;
            temporaryAnimatedSprite4.yPeriodic = num16 != 0;
            double num17 = 2000.0;
            temporaryAnimatedSprite4.yPeriodicLoopTime = (float) num17;
            double num18 = (double) (6 * Game1.pixelZoom);
            temporaryAnimatedSprite4.yPeriodicRange = (float) num18;
            int num19 = 650;
            temporaryAnimatedSprite4.delayBeforeAnimationStart = num19;
            temporarySprites4.Add(temporaryAnimatedSprite4);
          }
        }
        else if ((int) stringHash != -1987988496)
        {
          if ((int) stringHash != -1940397926)
          {
            if ((int) stringHash != -1936625601 || !(key == "secretGiftOpen"))
              return;
            TemporaryAnimatedSprite temporarySpriteById = location.getTemporarySpriteByID(666);
            if (temporarySpriteById == null)
              return;
            temporarySpriteById.animationLength = 6;
            temporarySpriteById.interval = 100f;
            temporarySpriteById.totalNumberOfLoops = 1;
            temporarySpriteById.timer = 0.0f;
            temporarySpriteById.holdLastFrame = true;
          }
          else
          {
            if (!(key == "beachStuff"))
              return;
            location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(324, 1887, 47, 29), 9999f, 1, 999, new Vector2(44f, 21f) * (float) Game1.tileSize, false, false, 1E-05f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false));
          }
        }
        else
        {
          if (!(key == "shakeTent"))
            return;
          location.getTemporarySpriteByID(999).shakeIntensity = 1f;
        }
      }
      else if (stringHash <= 3216534692U)
      {
        if (stringHash <= 2807316816U)
        {
          if (stringHash <= 2478151365U)
          {
            if (stringHash <= 2369812317U)
            {
              if ((int) stringHash != -1928754448)
              {
                if ((int) stringHash != -1925154979 || !(key == "leahPicnic"))
                  return;
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(96, 1808, 32, 48), 9999f, 1, 999, new Vector2(75f, 37f) * (float) Game1.tileSize, false, false, (float) (39 * Game1.tileSize) / 10000f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false));
                this.actors.Add(new NPC(new AnimatedSprite(this.festivalContent.Load<Texture2D>("Characters\\" + (Game1.player.isMale ? "LeahExMale" : "LeahExFemale")), 0, 16, 32), new Vector2(-100f, -100f) * (float) Game1.tileSize, 2, "LeahEx", (LocalizedContentManager) null));
              }
              else
              {
                if (!(key == "stopShakeTent"))
                  return;
                location.getTemporarySpriteByID(999).shakeIntensity = 0.0f;
              }
            }
            else if ((int) stringHash != -1883159383)
            {
              if ((int) stringHash != -1831310266)
              {
                if ((int) stringHash != -1816815931 || !(key == "samSkate1"))
                  return;
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(0, 0, 0, 0), 9999f, 1, 999, new Vector2(12f, 90f) * (float) Game1.tileSize, false, false, 1E-05f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
                {
                  motion = new Vector2(4f, 0.0f),
                  acceleration = new Vector2(-0.008f, 0.0f),
                  xStopCoordinate = 21 * Game1.tileSize,
                  reachedStopCoordinate = new TemporaryAnimatedSprite.endBehavior(this.samPreOllie),
                  attachedCharacter = (Character) this.getActorByName("Sam"),
                  id = 1f
                });
              }
              else
              {
                if (!(key == "wizardWarp2"))
                  return;
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(387, 1965, 16, 31), 9999f, 1, 999999, new Vector2(54f, 34f) * (float) Game1.tileSize + new Vector2(0.0f, (float) Game1.pixelZoom), false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
                {
                  motion = new Vector2(-1f, 2f),
                  acceleration = new Vector2(-0.1f, 0.2f),
                  scaleChange = 0.03f,
                  alphaFade = 1f / 1000f
                });
              }
            }
            else
            {
              if (!(key == "parrots1"))
                return;
              this.aboveMapSprites = new List<TemporaryAnimatedSprite>();
              List<TemporaryAnimatedSprite> aboveMapSprites1 = this.aboveMapSprites;
              TemporaryAnimatedSprite temporaryAnimatedSprite1 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(0, 165, 24, 22), 100f, 6, 9999, new Vector2((float) Game1.graphics.GraphicsDevice.Viewport.Width, (float) (Game1.tileSize * 4)), false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
              temporaryAnimatedSprite1.motion = new Vector2(-3f, 0.0f);
              int num1 = 1;
              temporaryAnimatedSprite1.yPeriodic = num1 != 0;
              double num2 = 2000.0;
              temporaryAnimatedSprite1.yPeriodicLoopTime = (float) num2;
              double num3 = (double) (Game1.tileSize / 2);
              temporaryAnimatedSprite1.yPeriodicRange = (float) num3;
              int num4 = 0;
              temporaryAnimatedSprite1.delayBeforeAnimationStart = num4;
              int num5 = 1;
              temporaryAnimatedSprite1.local = num5 != 0;
              aboveMapSprites1.Add(temporaryAnimatedSprite1);
              List<TemporaryAnimatedSprite> aboveMapSprites2 = this.aboveMapSprites;
              TemporaryAnimatedSprite temporaryAnimatedSprite2 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(0, 165, 24, 22), 100f, 6, 9999, new Vector2((float) Game1.graphics.GraphicsDevice.Viewport.Width, (float) (Game1.tileSize * 3)), false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
              temporaryAnimatedSprite2.motion = new Vector2(-3f, 0.0f);
              int num6 = 1;
              temporaryAnimatedSprite2.yPeriodic = num6 != 0;
              double num7 = 2000.0;
              temporaryAnimatedSprite2.yPeriodicLoopTime = (float) num7;
              double num8 = (double) (Game1.tileSize / 2);
              temporaryAnimatedSprite2.yPeriodicRange = (float) num8;
              int num9 = 600;
              temporaryAnimatedSprite2.delayBeforeAnimationStart = num9;
              int num10 = 1;
              temporaryAnimatedSprite2.local = num10 != 0;
              aboveMapSprites2.Add(temporaryAnimatedSprite2);
              List<TemporaryAnimatedSprite> aboveMapSprites3 = this.aboveMapSprites;
              TemporaryAnimatedSprite temporaryAnimatedSprite3 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(0, 165, 24, 22), 100f, 6, 9999, new Vector2((float) Game1.graphics.GraphicsDevice.Viewport.Width, (float) (Game1.tileSize * 5)), false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
              temporaryAnimatedSprite3.motion = new Vector2(-3f, 0.0f);
              int num11 = 1;
              temporaryAnimatedSprite3.yPeriodic = num11 != 0;
              double num12 = 2000.0;
              temporaryAnimatedSprite3.yPeriodicLoopTime = (float) num12;
              double num13 = (double) (Game1.tileSize / 2);
              temporaryAnimatedSprite3.yPeriodicRange = (float) num13;
              int num14 = 1200;
              temporaryAnimatedSprite3.delayBeforeAnimationStart = num14;
              int num15 = 1;
              temporaryAnimatedSprite3.local = num15 != 0;
              aboveMapSprites3.Add(temporaryAnimatedSprite3);
            }
          }
          else if (stringHash <= 2748349572U)
          {
            if ((int) stringHash != -1684408185)
            {
              if ((int) stringHash != -1648484440)
              {
                if ((int) stringHash != -1546617724 || !(key == "abbyManyBats"))
                  return;
                for (int index = 0; index < 100; ++index)
                {
                  List<TemporaryAnimatedSprite> temporarySprites = location.TemporarySprites;
                  TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(640, 1664, 16, 16), 80f, 4, 9999, new Vector2(23f, 9f) * (float) Game1.tileSize, false, false, 1f, 3f / 1000f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                  temporaryAnimatedSprite.xPeriodic = true;
                  double num1 = (double) Game1.random.Next(1500, 2500);
                  temporaryAnimatedSprite.xPeriodicLoopTime = (float) num1;
                  double num2 = (double) Game1.random.Next(Game1.tileSize, Game1.tileSize * 3);
                  temporaryAnimatedSprite.xPeriodicRange = (float) num2;
                  Vector2 vector2 = new Vector2((float) Game1.random.Next(-2, 3), (float) Game1.random.Next(-Game1.pixelZoom * 2, -Game1.pixelZoom));
                  temporaryAnimatedSprite.motion = vector2;
                  int num3 = index * 30;
                  temporaryAnimatedSprite.delayBeforeAnimationStart = num3;
                  string str = index % 10 == 0 || Game1.random.NextDouble() < 0.1 ? "batScreech" : (string) null;
                  temporaryAnimatedSprite.startSound = str;
                  temporarySprites.Add(temporaryAnimatedSprite);
                }
                for (int index = 0; index < 100; ++index)
                  location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(640, 1664, 16, 16), 80f, 4, 9999, new Vector2(23f, 9f) * (float) Game1.tileSize, false, false, 1f, 3f / 1000f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
                  {
                    motion = new Vector2((float) Game1.random.Next(-4, 5), (float) Game1.random.Next(-Game1.pixelZoom * 2, -Game1.pixelZoom)),
                    delayBeforeAnimationStart = 10 + index * 30
                  });
              }
              else
              {
                if (!(key == "joshFootball"))
                  return;
                List<TemporaryAnimatedSprite> temporarySprites = location.TemporarySprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(405, 1916, 14, 8), 40f, 6, 9999, new Vector2(25f, 16f) * (float) Game1.tileSize, false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                temporaryAnimatedSprite.rotation = -0.7853982f;
                temporaryAnimatedSprite.rotationChange = (float) Math.PI / 200f;
                Vector2 vector2_1 = new Vector2(6f, -4f);
                temporaryAnimatedSprite.motion = vector2_1;
                Vector2 vector2_2 = new Vector2(0.0f, 0.2f);
                temporaryAnimatedSprite.acceleration = vector2_2;
                int num1 = 29 * Game1.tileSize;
                temporaryAnimatedSprite.xStopCoordinate = num1;
                TemporaryAnimatedSprite.endBehavior endBehavior = new TemporaryAnimatedSprite.endBehavior(this.catchFootball);
                temporaryAnimatedSprite.reachedStopCoordinate = endBehavior;
                double num2 = 1.0;
                temporaryAnimatedSprite.layerDepth = (float) num2;
                double num3 = 1.0;
                temporaryAnimatedSprite.id = (float) num3;
                temporarySprites.Add(temporaryAnimatedSprite);
              }
            }
            else
            {
              if (!(key == "wizardSewerMagic"))
                return;
              location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(276, 1985, 12, 11), 50f, 4, 20, new Vector2(15f, 13f) * (float) Game1.tileSize + new Vector2((float) (Game1.pixelZoom * 2), 0.0f), false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
              {
                light = true,
                lightRadius = 1f,
                lightcolor = Color.Black,
                alphaFade = 0.005f
              });
              location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(276, 1985, 12, 11), 50f, 4, 20, new Vector2(17f, 13f) * (float) Game1.tileSize + new Vector2((float) (Game1.pixelZoom * 2), 0.0f), false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
              {
                light = true,
                lightRadius = 1f,
                lightcolor = Color.Black,
                alphaFade = 0.005f
              });
            }
          }
          else if ((int) stringHash != -1545563500)
          {
            if ((int) stringHash != -1489236869)
            {
              if ((int) stringHash != -1487650480 || !(key == "EmilySign"))
                return;
              this.aboveMapSprites = new List<TemporaryAnimatedSprite>();
              for (int index = 0; index < 10; ++index)
              {
                int num1 = 0;
                int num2 = Game1.random.Next(Game1.graphics.GraphicsDevice.Viewport.Height - Game1.tileSize * 2);
                int width = Game1.graphics.GraphicsDevice.Viewport.Width;
                while (width >= -Game1.tileSize)
                {
                  List<TemporaryAnimatedSprite> aboveMapSprites = this.aboveMapSprites;
                  TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(597, 1888, 16, 16), 99999f, 1, 99999, new Vector2((float) width, (float) num2), false, false, 1f, 0.02f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                  temporaryAnimatedSprite.delayBeforeAnimationStart = index * 600 + num1 * 25;
                  temporaryAnimatedSprite.startSound = num1 == 0 ? "dwoop" : (string) null;
                  int num3 = 1;
                  temporaryAnimatedSprite.local = num3 != 0;
                  aboveMapSprites.Add(temporaryAnimatedSprite);
                  ++num1;
                  width -= Game1.tileSize * 3 / 4;
                }
              }
            }
            else
            {
              if (!(key == "grandpaSpirit"))
                return;
              TemporaryAnimatedSprite temporaryAnimatedSprite1 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(555, 1956, 18, 35), 9999f, 1, 99999, new Vector2(-1000f, -1010f) * (float) Game1.tileSize, false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
              temporaryAnimatedSprite1.yStopCoordinate = -1002 * Game1.tileSize;
              int num1 = 1;
              temporaryAnimatedSprite1.xPeriodic = num1 != 0;
              double num2 = 3000.0;
              temporaryAnimatedSprite1.xPeriodicLoopTime = (float) num2;
              double num3 = (double) (Game1.tileSize / 4);
              temporaryAnimatedSprite1.xPeriodicRange = (float) num3;
              Vector2 vector2 = new Vector2(0.0f, 1f);
              temporaryAnimatedSprite1.motion = vector2;
              int num4 = 1;
              temporaryAnimatedSprite1.overrideLocationDestroy = num4 != 0;
              TemporaryAnimatedSprite temporaryAnimatedSprite2 = temporaryAnimatedSprite1;
              location.temporarySprites.Add(temporaryAnimatedSprite2);
              for (int index = 0; index < 19; ++index)
              {
                List<TemporaryAnimatedSprite> temporarySprites = location.temporarySprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite3 = new TemporaryAnimatedSprite(10, new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2)), Color.White, 8, false, 100f, 0, -1, -1f, -1, 0);
                temporaryAnimatedSprite3.parentSprite = temporaryAnimatedSprite2;
                int num5 = (index + 1) * 500;
                temporaryAnimatedSprite3.delayBeforeAnimationStart = num5;
                int num6 = 1;
                temporaryAnimatedSprite3.overrideLocationDestroy = num6 != 0;
                double num7 = 1.0;
                temporaryAnimatedSprite3.scale = (float) num7;
                double num8 = 1.0;
                temporaryAnimatedSprite3.alpha = (float) num8;
                temporarySprites.Add(temporaryAnimatedSprite3);
              }
            }
          }
          else
          {
            if (!(key == "skateboardFly"))
              return;
            List<TemporaryAnimatedSprite> temporarySprites = location.TemporarySprites;
            TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(388, 1875, 16, 6), 9999f, 1, 999, new Vector2(26f, 90f) * (float) Game1.tileSize, false, false, 1E-05f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
            temporaryAnimatedSprite.rotationChange = 0.1308997f;
            Vector2 vector2_1 = new Vector2(-8f, -10f);
            temporaryAnimatedSprite.motion = vector2_1;
            Vector2 vector2_2 = new Vector2(0.02f, 0.3f);
            temporaryAnimatedSprite.acceleration = vector2_2;
            int num1 = 91 * Game1.tileSize;
            temporaryAnimatedSprite.yStopCoordinate = num1;
            int num2 = 16 * Game1.tileSize;
            temporaryAnimatedSprite.xStopCoordinate = num2;
            double num3 = 1.0;
            temporaryAnimatedSprite.layerDepth = (float) num3;
            temporarySprites.Add(temporaryAnimatedSprite);
          }
        }
        else if (stringHash <= 3017460216U)
        {
          if (stringHash <= 2881735469U)
          {
            if ((int) stringHash != -1461791617)
            {
              if ((int) stringHash != -1432028472)
              {
                if ((int) stringHash != -1413231827 || !(key == "maruTrapdoor"))
                  return;
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(640, 1632, 16, 32), 150f, 4, 0, new Vector2(1f, 5f) * (float) Game1.tileSize, false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false));
                location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(688, 1632, 16, 32), 99999f, 1, 0, new Vector2(1f, 5f) * (float) Game1.tileSize, false, false, 0.99f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
                {
                  delayBeforeAnimationStart = 500
                });
              }
              else
              {
                if (!(key == "witchFlyby"))
                  return;
                List<TemporaryAnimatedSprite> overlayTempSprites = Game1.screenOverlayTempSprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(276, 1886, 35, 29), 9999f, 1, 999999, new Vector2((float) Game1.graphics.GraphicsDevice.Viewport.Width, (float) (Game1.tileSize * 3)), false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
                temporaryAnimatedSprite.motion = new Vector2(-4f, 0.0f);
                temporaryAnimatedSprite.acceleration = new Vector2(-0.025f, 0.0f);
                int num1 = 1;
                temporaryAnimatedSprite.yPeriodic = num1 != 0;
                double num2 = 2000.0;
                temporaryAnimatedSprite.yPeriodicLoopTime = (float) num2;
                double tileSize = (double) Game1.tileSize;
                temporaryAnimatedSprite.yPeriodicRange = (float) tileSize;
                int num3 = 1;
                temporaryAnimatedSprite.local = num3 != 0;
                overlayTempSprites.Add(temporaryAnimatedSprite);
              }
            }
            else
            {
              if (!(key == "farmerForestVision"))
                return;
              location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(393, 1973, 1, 1), 9999f, 1, 999999, new Vector2(0.0f, 0.0f) * (float) Game1.tileSize, false, false, 0.9f, 0.0f, Color.LimeGreen * 0.85f, (float) (Game1.viewport.Width * 2), 0.0f, 0.0f, 0.0f, true)
              {
                alpha = 0.0f,
                alphaFade = -1f / 500f,
                id = 1f
              });
              Game1.player.mailReceived.Add("canReadJunimoText");
              int num1 = -Game1.tileSize;
              int num2 = -Game1.tileSize;
              int num3 = 0;
              int num4 = 0;
              while (num2 < Game1.viewport.Height + Game1.tileSize * 2)
              {
                List<TemporaryAnimatedSprite> temporarySprites = location.TemporarySprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(367 + (num3 % 2 == 0 ? 8 : 0), 1969, 8, 8), 9999f, 1, 999999, new Vector2((float) num1, (float) num2), false, false, 0.99f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, true);
                temporaryAnimatedSprite.alpha = 0.0f;
                temporaryAnimatedSprite.alphaFade = -0.0015f;
                temporaryAnimatedSprite.xPeriodic = true;
                temporaryAnimatedSprite.xPeriodicLoopTime = 4000f;
                double tileSize = (double) Game1.tileSize;
                temporaryAnimatedSprite.xPeriodicRange = (float) tileSize;
                int num5 = 1;
                temporaryAnimatedSprite.yPeriodic = num5 != 0;
                double num6 = 5000.0;
                temporaryAnimatedSprite.yPeriodicLoopTime = (float) num6;
                double num7 = (double) (Game1.tileSize * 3 / 2);
                temporaryAnimatedSprite.yPeriodicRange = (float) num7;
                double num8 = (double) Game1.random.Next(-1, 2) * 3.14159274101257 / 256.0;
                temporaryAnimatedSprite.rotationChange = (float) num8;
                double num9 = 1.0;
                temporaryAnimatedSprite.id = (float) num9;
                int num10 = 20 * num3;
                temporaryAnimatedSprite.delayBeforeAnimationStart = num10;
                temporarySprites.Add(temporaryAnimatedSprite);
                num1 += Game1.tileSize * 2;
                if (num1 > Game1.viewport.Width + Game1.tileSize)
                {
                  ++num4;
                  num1 = num4 % 2 == 0 ? -Game1.tileSize : Game1.tileSize;
                  num2 += Game1.tileSize * 2;
                }
                ++num3;
              }
              List<TemporaryAnimatedSprite> temporarySprites1 = location.TemporarySprites;
              TemporaryAnimatedSprite temporaryAnimatedSprite1 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(648, 895, 51, 101), 9999f, 1, 999999, new Vector2((float) (Game1.viewport.Width / 2 - 25 * Game1.pixelZoom), (float) (Game1.viewport.Height / 2 - 60 * Game1.pixelZoom)), false, false, 1f, 0.0f, Color.White, (float) (Game1.pixelZoom * 3) / 4f, 0.0f, 0.0f, 0.0f, true);
              temporaryAnimatedSprite1.alpha = 0.0f;
              temporaryAnimatedSprite1.alphaFade = -1f / 1000f;
              temporaryAnimatedSprite1.id = 1f;
              temporaryAnimatedSprite1.delayBeforeAnimationStart = 6000;
              temporaryAnimatedSprite1.scaleChange = 0.004f;
              temporaryAnimatedSprite1.xPeriodic = true;
              temporaryAnimatedSprite1.xPeriodicLoopTime = 4000f;
              double tileSize1 = (double) Game1.tileSize;
              temporaryAnimatedSprite1.xPeriodicRange = (float) tileSize1;
              int num11 = 1;
              temporaryAnimatedSprite1.yPeriodic = num11 != 0;
              double num12 = 5000.0;
              temporaryAnimatedSprite1.yPeriodicLoopTime = (float) num12;
              double num13 = (double) (Game1.tileSize / 2);
              temporaryAnimatedSprite1.yPeriodicRange = (float) num13;
              temporarySprites1.Add(temporaryAnimatedSprite1);
              List<TemporaryAnimatedSprite> temporarySprites2 = location.TemporarySprites;
              TemporaryAnimatedSprite temporaryAnimatedSprite2 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(648, 895, 51, 101), 9999f, 1, 999999, new Vector2((float) (Game1.viewport.Width / 4 - 25 * Game1.pixelZoom), (float) (Game1.viewport.Height / 4 - 30 * Game1.pixelZoom)), false, false, 0.99f, 0.0f, Color.White, (float) (Game1.pixelZoom * 3) / 4f, 0.0f, 0.0f, 0.0f, true);
              temporaryAnimatedSprite2.alpha = 0.0f;
              temporaryAnimatedSprite2.alphaFade = -1f / 1000f;
              temporaryAnimatedSprite2.id = 1f;
              temporaryAnimatedSprite2.delayBeforeAnimationStart = 9000;
              temporaryAnimatedSprite2.scaleChange = 0.004f;
              temporaryAnimatedSprite2.xPeriodic = true;
              temporaryAnimatedSprite2.xPeriodicLoopTime = 4000f;
              double tileSize2 = (double) Game1.tileSize;
              temporaryAnimatedSprite2.xPeriodicRange = (float) tileSize2;
              int num14 = 1;
              temporaryAnimatedSprite2.yPeriodic = num14 != 0;
              double num15 = 5000.0;
              temporaryAnimatedSprite2.yPeriodicLoopTime = (float) num15;
              double num16 = (double) (Game1.tileSize / 2);
              temporaryAnimatedSprite2.yPeriodicRange = (float) num16;
              temporarySprites2.Add(temporaryAnimatedSprite2);
              List<TemporaryAnimatedSprite> temporarySprites3 = location.TemporarySprites;
              TemporaryAnimatedSprite temporaryAnimatedSprite3 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(648, 895, 51, 101), 9999f, 1, 999999, new Vector2((float) (Game1.viewport.Width * 3 / 4), (float) (Game1.viewport.Height / 3 - 30 * Game1.pixelZoom)), false, false, 0.98f, 0.0f, Color.White, (float) (Game1.pixelZoom * 3) / 4f, 0.0f, 0.0f, 0.0f, true);
              temporaryAnimatedSprite3.alpha = 0.0f;
              temporaryAnimatedSprite3.alphaFade = -1f / 1000f;
              temporaryAnimatedSprite3.id = 1f;
              temporaryAnimatedSprite3.delayBeforeAnimationStart = 12000;
              temporaryAnimatedSprite3.scaleChange = 0.004f;
              temporaryAnimatedSprite3.xPeriodic = true;
              temporaryAnimatedSprite3.xPeriodicLoopTime = 4000f;
              double tileSize3 = (double) Game1.tileSize;
              temporaryAnimatedSprite3.xPeriodicRange = (float) tileSize3;
              int num17 = 1;
              temporaryAnimatedSprite3.yPeriodic = num17 != 0;
              double num18 = 5000.0;
              temporaryAnimatedSprite3.yPeriodicLoopTime = (float) num18;
              double num19 = (double) (Game1.tileSize / 2);
              temporaryAnimatedSprite3.yPeriodicRange = (float) num19;
              temporarySprites3.Add(temporaryAnimatedSprite3);
              List<TemporaryAnimatedSprite> temporarySprites4 = location.TemporarySprites;
              TemporaryAnimatedSprite temporaryAnimatedSprite4 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(648, 895, 51, 101), 9999f, 1, 999999, new Vector2((float) (Game1.viewport.Width / 3 - 15 * Game1.pixelZoom), (float) (Game1.viewport.Height * 3 / 4 - 30 * Game1.pixelZoom)), false, false, 0.97f, 0.0f, Color.White, (float) (Game1.pixelZoom * 3) / 4f, 0.0f, 0.0f, 0.0f, true);
              temporaryAnimatedSprite4.alpha = 0.0f;
              temporaryAnimatedSprite4.alphaFade = -1f / 1000f;
              temporaryAnimatedSprite4.id = 1f;
              temporaryAnimatedSprite4.delayBeforeAnimationStart = 15000;
              temporaryAnimatedSprite4.scaleChange = 0.004f;
              temporaryAnimatedSprite4.xPeriodic = true;
              temporaryAnimatedSprite4.xPeriodicLoopTime = 4000f;
              double tileSize4 = (double) Game1.tileSize;
              temporaryAnimatedSprite4.xPeriodicRange = (float) tileSize4;
              int num20 = 1;
              temporaryAnimatedSprite4.yPeriodic = num20 != 0;
              double num21 = 5000.0;
              temporaryAnimatedSprite4.yPeriodicLoopTime = (float) num21;
              double num22 = (double) (Game1.tileSize / 2);
              temporaryAnimatedSprite4.yPeriodicRange = (float) num22;
              temporarySprites4.Add(temporaryAnimatedSprite4);
              List<TemporaryAnimatedSprite> temporarySprites5 = location.TemporarySprites;
              TemporaryAnimatedSprite temporaryAnimatedSprite5 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(648, 895, 51, 101), 9999f, 1, 999999, new Vector2((float) (Game1.viewport.Width * 2 / 3), (float) (Game1.viewport.Height * 2 / 3 - 30 * Game1.pixelZoom)), false, false, 0.96f, 0.0f, Color.White, (float) (Game1.pixelZoom * 3) / 4f, 0.0f, 0.0f, 0.0f, true);
              temporaryAnimatedSprite5.alpha = 0.0f;
              temporaryAnimatedSprite5.alphaFade = -1f / 1000f;
              temporaryAnimatedSprite5.id = 1f;
              temporaryAnimatedSprite5.delayBeforeAnimationStart = 18000;
              temporaryAnimatedSprite5.scaleChange = 0.004f;
              temporaryAnimatedSprite5.xPeriodic = true;
              temporaryAnimatedSprite5.xPeriodicLoopTime = 4000f;
              double tileSize5 = (double) Game1.tileSize;
              temporaryAnimatedSprite5.xPeriodicRange = (float) tileSize5;
              int num23 = 1;
              temporaryAnimatedSprite5.yPeriodic = num23 != 0;
              double num24 = 5000.0;
              temporaryAnimatedSprite5.yPeriodicLoopTime = (float) num24;
              double num25 = (double) (Game1.tileSize / 2);
              temporaryAnimatedSprite5.yPeriodicRange = (float) num25;
              temporarySprites5.Add(temporaryAnimatedSprite5);
              List<TemporaryAnimatedSprite> temporarySprites6 = location.TemporarySprites;
              TemporaryAnimatedSprite temporaryAnimatedSprite6 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(648, 895, 51, 101), 9999f, 1, 999999, new Vector2((float) (Game1.viewport.Width / 8), (float) (Game1.viewport.Height / 5 - 30 * Game1.pixelZoom)), false, false, 0.95f, 0.0f, Color.White, (float) (Game1.pixelZoom * 3) / 4f, 0.0f, 0.0f, 0.0f, true);
              temporaryAnimatedSprite6.alpha = 0.0f;
              temporaryAnimatedSprite6.alphaFade = -1f / 1000f;
              temporaryAnimatedSprite6.id = 1f;
              temporaryAnimatedSprite6.delayBeforeAnimationStart = 19500;
              temporaryAnimatedSprite6.scaleChange = 0.004f;
              temporaryAnimatedSprite6.xPeriodic = true;
              temporaryAnimatedSprite6.xPeriodicLoopTime = 4000f;
              double tileSize6 = (double) Game1.tileSize;
              temporaryAnimatedSprite6.xPeriodicRange = (float) tileSize6;
              int num26 = 1;
              temporaryAnimatedSprite6.yPeriodic = num26 != 0;
              double num27 = 5000.0;
              temporaryAnimatedSprite6.yPeriodicLoopTime = (float) num27;
              double num28 = (double) (Game1.tileSize / 2);
              temporaryAnimatedSprite6.yPeriodicRange = (float) num28;
              temporarySprites6.Add(temporaryAnimatedSprite6);
              List<TemporaryAnimatedSprite> temporarySprites7 = location.TemporarySprites;
              TemporaryAnimatedSprite temporaryAnimatedSprite7 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(648, 895, 51, 101), 9999f, 1, 999999, new Vector2((float) (Game1.viewport.Width * 2 / 3), (float) (Game1.viewport.Height / 5 - 30 * Game1.pixelZoom)), false, false, 0.94f, 0.0f, Color.White, (float) (Game1.pixelZoom * 3) / 4f, 0.0f, 0.0f, 0.0f, true);
              temporaryAnimatedSprite7.alpha = 0.0f;
              temporaryAnimatedSprite7.alphaFade = -1f / 1000f;
              temporaryAnimatedSprite7.id = 1f;
              temporaryAnimatedSprite7.delayBeforeAnimationStart = 21000;
              temporaryAnimatedSprite7.scaleChange = 0.004f;
              temporaryAnimatedSprite7.xPeriodic = true;
              temporaryAnimatedSprite7.xPeriodicLoopTime = 4000f;
              double tileSize7 = (double) Game1.tileSize;
              temporaryAnimatedSprite7.xPeriodicRange = (float) tileSize7;
              int num29 = 1;
              temporaryAnimatedSprite7.yPeriodic = num29 != 0;
              double num30 = 5000.0;
              temporaryAnimatedSprite7.yPeriodicLoopTime = (float) num30;
              double num31 = (double) (Game1.tileSize / 2);
              temporaryAnimatedSprite7.yPeriodicRange = (float) num31;
              temporarySprites7.Add(temporaryAnimatedSprite7);
            }
          }
          else if ((int) stringHash != -1338786494)
          {
            if ((int) stringHash != -1291845602)
            {
              if ((int) stringHash != -1277507080 || !(key == "shanePassedOut"))
                return;
              location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(533, 1864, 19, 27), 99999f, 1, 99999, new Vector2(25f, 7f) * (float) Game1.tileSize, false, false, 0.01f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
              {
                id = 999f
              });
              location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(552, 1862, 31, 21), 99999f, 1, 99999, new Vector2(25f, 7f) * (float) Game1.tileSize + new Vector2((float) (-Game1.tileSize / 4), 0.0f), false, false, 0.0001f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false));
            }
            else
            {
              if (!(key == "morrisFlying"))
                return;
              location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(105, 1318, 13, 31), 9999f, 1, 99999, new Vector2(32f, 13f) * (float) Game1.tileSize, false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
              {
                motion = new Vector2(4f, -8f),
                rotationChange = 0.1963495f,
                shakeIntensity = 1f
              });
            }
          }
          else
          {
            if (!(key == "junimoCageGone"))
              return;
            location.removeTemporarySpritesWithID(1);
          }
        }
        else if (stringHash <= 3091229030U)
        {
          if ((int) stringHash != -1271504038)
          {
            if ((int) stringHash != -1229501950)
            {
              if ((int) stringHash != -1203738266 || !(key == "candleBoat"))
                return;
              location.TemporarySprites.Add(new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(240, 112, 16, 32), 1000f, 2, 99999, new Vector2(22f, 36f) * (float) Game1.tileSize, false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
              {
                id = 1f,
                light = true,
                lightRadius = 2f,
                lightcolor = Color.Black
              });
            }
            else
            {
              if (!(key == "abbyvideoscreen"))
                return;
              location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(167, 1714, 19, 14), 100f, 3, 9999, new Vector2(2f, 3f) * (float) Game1.tileSize + new Vector2(7f, 12f) * (float) Game1.pixelZoom, false, false, 0.0002f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false));
            }
          }
          else
          {
            if (!(key == "arcaneBook"))
              return;
            for (int index = 0; index < 16; ++index)
            {
              List<TemporaryAnimatedSprite> temporarySprites = location.TemporarySprites;
              TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(536, 1945, 8, 8), new Vector2((float) (2 * Game1.tileSize), (float) (12 * Game1.tileSize + 6 * Game1.pixelZoom)) + new Vector2((float) Game1.random.Next(Game1.tileSize / 2), (float) (Game1.random.Next(Game1.tileSize / 2) - index * Game1.pixelZoom)), false, 0.0f, Color.White);
              temporaryAnimatedSprite.interval = 50f;
              temporaryAnimatedSprite.totalNumberOfLoops = 99999;
              temporaryAnimatedSprite.animationLength = 7;
              temporaryAnimatedSprite.layerDepth = 1f;
              double pixelZoom = (double) Game1.pixelZoom;
              temporaryAnimatedSprite.scale = (float) pixelZoom;
              double num = 0.00800000037997961;
              temporaryAnimatedSprite.alphaFade = (float) num;
              Vector2 vector2 = new Vector2(0.0f, -0.5f);
              temporaryAnimatedSprite.motion = vector2;
              temporarySprites.Add(temporaryAnimatedSprite);
            }
            this.aboveMapSprites = new List<TemporaryAnimatedSprite>();
            List<TemporaryAnimatedSprite> aboveMapSprites1 = this.aboveMapSprites;
            TemporaryAnimatedSprite temporaryAnimatedSprite1 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(325, 1977, 18, 18), new Vector2((float) (2 * Game1.tileSize + 8 * Game1.pixelZoom), (float) (12 * Game1.tileSize + 8 * Game1.pixelZoom)), false, 0.0f, Color.White);
            temporaryAnimatedSprite1.interval = 25f;
            temporaryAnimatedSprite1.totalNumberOfLoops = 99999;
            temporaryAnimatedSprite1.animationLength = 3;
            temporaryAnimatedSprite1.layerDepth = 1f;
            temporaryAnimatedSprite1.scale = 1f;
            temporaryAnimatedSprite1.scaleChange = 1f;
            temporaryAnimatedSprite1.scaleChangeChange = -0.05f;
            temporaryAnimatedSprite1.alpha = 0.65f;
            temporaryAnimatedSprite1.alphaFade = 0.005f;
            Vector2 vector2_1 = new Vector2(-8f, -8f);
            temporaryAnimatedSprite1.motion = vector2_1;
            Vector2 vector2_2 = new Vector2(0.4f, 0.4f);
            temporaryAnimatedSprite1.acceleration = vector2_2;
            aboveMapSprites1.Add(temporaryAnimatedSprite1);
            for (int index = 0; index < 16; ++index)
            {
              List<TemporaryAnimatedSprite> aboveMapSprites2 = this.aboveMapSprites;
              TemporaryAnimatedSprite temporaryAnimatedSprite2 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(372, 1956, 10, 10), new Vector2(2f, 12f) * (float) Game1.tileSize + new Vector2((float) Game1.random.Next(-Game1.tileSize / 2, Game1.tileSize), 0.0f), false, 1f / 500f, Color.Gray);
              temporaryAnimatedSprite2.alpha = 0.75f;
              Vector2 vector2_3 = new Vector2(1f, -1f) + new Vector2((float) (Game1.random.Next(100) - 50) / 100f, (float) (Game1.random.Next(100) - 50) / 100f);
              temporaryAnimatedSprite2.motion = vector2_3;
              double num1 = 99999.0;
              temporaryAnimatedSprite2.interval = (float) num1;
              double num2 = (double) (6 * Game1.tileSize) / 10000.0 + (double) Game1.random.Next(100) / 10000.0;
              temporaryAnimatedSprite2.layerDepth = (float) num2;
              double num3 = (double) (Game1.pixelZoom * 3) / 4.0;
              temporaryAnimatedSprite2.scale = (float) num3;
              double num4 = 0.00999999977648258;
              temporaryAnimatedSprite2.scaleChange = (float) num4;
              double num5 = (double) Game1.random.Next(-5, 6) * 3.14159274101257 / 256.0;
              temporaryAnimatedSprite2.rotationChange = (float) num5;
              int num6 = index * 25;
              temporaryAnimatedSprite2.delayBeforeAnimationStart = num6;
              aboveMapSprites2.Add(temporaryAnimatedSprite2);
            }
            location.setMapTileIndex(2, 12, 2143, "Front", 1);
          }
        }
        else if ((int) stringHash != -1196689138)
        {
          if ((int) stringHash != -1091569103)
          {
            if ((int) stringHash != -1078432604 || !(key == "abbyOneBat"))
              return;
            List<TemporaryAnimatedSprite> temporarySprites = location.TemporarySprites;
            TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(640, 1664, 16, 16), 80f, 4, 9999, new Vector2(23f, 9f) * (float) Game1.tileSize, false, false, 1f, 3f / 1000f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
            temporaryAnimatedSprite.xPeriodic = true;
            temporaryAnimatedSprite.xPeriodicLoopTime = 2000f;
            double num = (double) (Game1.tileSize * 2);
            temporaryAnimatedSprite.xPeriodicRange = (float) num;
            Vector2 vector2 = new Vector2(0.0f, (float) (-Game1.pixelZoom * 2));
            temporaryAnimatedSprite.motion = vector2;
            temporarySprites.Add(temporaryAnimatedSprite);
          }
          else
          {
            if (!(key == "pennyMess"))
              return;
            location.TemporarySprites.Add(new TemporaryAnimatedSprite(739, 999999f, 1, 0, new Vector2(10f, 5f) * (float) Game1.tileSize, false, false));
            location.TemporarySprites.Add(new TemporaryAnimatedSprite(740, 999999f, 1, 0, new Vector2(15f, 5f) * (float) Game1.tileSize, false, false));
            location.TemporarySprites.Add(new TemporaryAnimatedSprite(741, 999999f, 1, 0, new Vector2(16f, 6f) * (float) Game1.tileSize, false, false));
          }
        }
        else
        {
          if (!(key == "leahShow"))
            return;
          location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(144, 688, 16, 32), 9999f, 1, 999, new Vector2(29f, 59f) * (float) Game1.tileSize - new Vector2(0.0f, (float) (Game1.tileSize / 4)), false, false, (float) ((double) (59 * Game1.tileSize) / 10000.0 - 9.99999974737875E-05), 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false));
          location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(112, 656, 16, 64), 9999f, 1, 999, new Vector2(29f, 56f) * (float) Game1.tileSize, false, false, (float) (59 * Game1.tileSize) / 10000f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false));
          location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(144, 688, 16, 32), 9999f, 1, 999, new Vector2(33f, 59f) * (float) Game1.tileSize - new Vector2(0.0f, (float) (Game1.tileSize / 4)), false, false, (float) ((double) (59 * Game1.tileSize) / 10000.0 - 9.99999974737875E-05), 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false));
          location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(128, 688, 16, 32), 9999f, 1, 999, new Vector2(33f, 58f) * (float) Game1.tileSize, false, false, (float) (59 * Game1.tileSize) / 10000f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false));
          location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(160, 656, 32, 64), 9999f, 1, 999, new Vector2(29f, 60f) * (float) Game1.tileSize, false, false, (float) (63 * Game1.tileSize) / 10000f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false));
          location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(144, 688, 16, 32), 9999f, 1, 999, new Vector2(34f, 63f) * (float) Game1.tileSize, false, false, (float) ((double) (63 * Game1.tileSize) / 10000.0 - 9.99999974737875E-05), 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false));
          location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(113, 592, 16, 64), 100f, 4, 99999, new Vector2(34f, 60f) * (float) Game1.tileSize, false, false, (float) (63 * Game1.tileSize) / 10000f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false));
          this.actors.Add(new NPC(new AnimatedSprite(this.festivalContent.Load<Texture2D>("Characters\\" + (Game1.player.isMale ? "LeahExMale" : "LeahExFemale")), 0, 16, 32), new Vector2(46f, 57f) * (float) Game1.tileSize, 2, "LeahEx", (LocalizedContentManager) null));
        }
      }
      else if (stringHash <= 3744618201U)
      {
        if (stringHash <= 3447055920U)
        {
          if (stringHash <= 3339287078U)
          {
            if ((int) stringHash != -955796267)
            {
              if ((int) stringHash != -955680218 || !(key == "parrotSlide"))
                return;
              location.getTemporarySpriteByID(666).yStopCoordinate = 88 * Game1.tileSize;
              location.getTemporarySpriteByID(666).motion.X = 0.0f;
              location.getTemporarySpriteByID(666).motion.Y = 1f;
            }
            else
            {
              if (!(key == "joshSteak"))
                return;
              location.temporarySprites.Clear();
              location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(324, 1936, 12, 20), 80f, 4, 99999, new Vector2(53f, 67f) * (float) Game1.tileSize + new Vector2(3f, 3f) * (float) Game1.pixelZoom, false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
              {
                id = 1f
              });
              location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(497, 1918, 11, 11), 999f, 1, 9999, new Vector2(50f, 68f) * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), (float) (-Game1.tileSize / 8)), false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false));
            }
          }
          else if ((int) stringHash != -909396307)
          {
            if ((int) stringHash != -853200263)
            {
              if ((int) stringHash != -847911376 || !(key == "abbyGraveyard"))
                return;
              location.TemporarySprites.Add(new TemporaryAnimatedSprite(736, 999999f, 1, 0, new Vector2(48f, 86f) * (float) Game1.tileSize, false, false));
            }
            else
            {
              if (!(key == "joshDog"))
                return;
              location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(324, 1916, 12, 20), 500f, 6, 9999, new Vector2(53f, 67f) * (float) Game1.tileSize + new Vector2(3f, 3f) * (float) Game1.pixelZoom, false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
              {
                id = 1f
              });
            }
          }
          else
          {
            if (!(key == "joshDinner"))
              return;
            location.TemporarySprites.Add(new TemporaryAnimatedSprite(649, 9999f, 1, 9999, new Vector2(6f, 4f) * (float) Game1.tileSize + new Vector2((float) (Game1.pixelZoom * 2), (float) (Game1.tileSize / 2)), false, false)
            {
              layerDepth = (float) ((double) (4 * Game1.tileSize) / 10000.0)
            });
            location.TemporarySprites.Add(new TemporaryAnimatedSprite(664, 9999f, 1, 9999, new Vector2(8f, 4f) * (float) Game1.tileSize + new Vector2((float) (-Game1.pixelZoom * 2), (float) (Game1.tileSize / 2)), false, false)
            {
              layerDepth = (float) ((double) (4 * Game1.tileSize) / 10000.0)
            });
          }
        }
        else if (stringHash <= 3513988944U)
        {
          if ((int) stringHash != -846680659)
          {
            if ((int) stringHash != -837537569)
            {
              if ((int) stringHash != -780978352 || !(key == "junimoCageGone2"))
                return;
              location.removeTemporarySpritesWithID(1);
              Game1.viewportFreeze = true;
              Game1.viewport.X = -1000;
              Game1.viewport.Y = -1000;
            }
            else
            {
              if (!(key == "robot"))
                return;
              TemporaryAnimatedSprite temporaryAnimatedSprite1 = new TemporaryAnimatedSprite(this.getActorByName("robot").Sprite.Texture, new Microsoft.Xna.Framework.Rectangle(35, 42, 35, 42), 50f, 1, 9999, new Vector2(13f, 27f) * (float) Game1.tileSize - new Vector2(0.0f, (float) (Game1.tileSize / 2)), false, false, 0.98f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
              {
                acceleration = new Vector2(0.0f, -0.01f),
                accelerationChange = new Vector2(0.0f, -0.0001f)
              };
              location.temporarySprites.Add(temporaryAnimatedSprite1);
              for (int index = 0; index < 420; ++index)
              {
                List<TemporaryAnimatedSprite> temporarySprites = location.TemporarySprites;
                TemporaryAnimatedSprite temporaryAnimatedSprite2 = new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(Game1.random.Next(4) * Game1.tileSize, 320, Game1.tileSize, Game1.tileSize), new Vector2((float) Game1.random.Next(Game1.tileSize * 3 / 2), (float) (Game1.tileSize * 2 + Game1.pixelZoom * 2)), false, 0.01f, Color.White * 0.75f);
                temporaryAnimatedSprite2.layerDepth = 1f;
                int num1 = index * 10;
                temporaryAnimatedSprite2.delayBeforeAnimationStart = num1;
                int num2 = 1;
                temporaryAnimatedSprite2.animationLength = num2;
                int num3 = 0;
                temporaryAnimatedSprite2.currentNumberOfLoops = num3;
                double num4 = 9999.0;
                temporaryAnimatedSprite2.interval = (float) num4;
                Vector2 vector2 = new Vector2((float) (Game1.random.Next(-100, 100) / (index + 20)), (float) (0.25 + (double) index / 100.0));
                temporaryAnimatedSprite2.motion = vector2;
                TemporaryAnimatedSprite temporaryAnimatedSprite3 = temporaryAnimatedSprite1;
                temporaryAnimatedSprite2.parentSprite = temporaryAnimatedSprite3;
                temporarySprites.Add(temporaryAnimatedSprite2);
              }
            }
          }
          else
          {
            if (!(key == "ccCelebration"))
              return;
            this.aboveMapSprites = new List<TemporaryAnimatedSprite>();
            for (int index = 0; index < 32; ++index)
            {
              Vector2 position = new Vector2((float) Game1.random.Next(Game1.viewport.Width - Game1.tileSize * 2), (float) (Game1.viewport.Height + index * Game1.tileSize));
              List<TemporaryAnimatedSprite> aboveMapSprites1 = this.aboveMapSprites;
              TemporaryAnimatedSprite temporaryAnimatedSprite1 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(534, 1413, 11, 16), 99999f, 1, 99999, position, false, false, 1f, 0.0f, Utility.getRandomRainbowColor((Random) null), (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
              temporaryAnimatedSprite1.local = true;
              Vector2 vector2_1 = new Vector2(0.25f, -1.5f);
              temporaryAnimatedSprite1.motion = vector2_1;
              Vector2 vector2_2 = new Vector2(0.0f, -1f / 1000f);
              temporaryAnimatedSprite1.acceleration = vector2_2;
              aboveMapSprites1.Add(temporaryAnimatedSprite1);
              List<TemporaryAnimatedSprite> aboveMapSprites2 = this.aboveMapSprites;
              TemporaryAnimatedSprite temporaryAnimatedSprite2 = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(545, 1413, 11, 34), 99999f, 1, 99999, position + new Vector2(0.0f, 0.0f), false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
              temporaryAnimatedSprite2.local = true;
              Vector2 vector2_3 = new Vector2(0.25f, -1.5f);
              temporaryAnimatedSprite2.motion = vector2_3;
              Vector2 vector2_4 = new Vector2(0.0f, -1f / 1000f);
              temporaryAnimatedSprite2.acceleration = vector2_4;
              aboveMapSprites2.Add(temporaryAnimatedSprite2);
            }
            location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(558, 1425, 20, 26), 400f, 3, 99999, new Vector2(53f, 21f) * (float) Game1.tileSize, false, false, 0.5f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
            {
              pingPong = true
            });
          }
        }
        else if ((int) stringHash != -713998007)
        {
          if ((int) stringHash != -562813360)
          {
            if ((int) stringHash != -550349095 || !(key == "linusLights"))
              return;
            Game1.currentLightSources.Add(new LightSource(2, new Vector2(55f, 62f) * (float) Game1.tileSize, 2f));
            Game1.currentLightSources.Add(new LightSource(2, new Vector2(60f, 62f) * (float) Game1.tileSize, 2f));
            Game1.currentLightSources.Add(new LightSource(2, new Vector2(57f, 60f) * (float) Game1.tileSize, 3f));
            Game1.currentLightSources.Add(new LightSource(2, new Vector2(57f, 60f) * (float) Game1.tileSize, 2f));
            Game1.currentLightSources.Add(new LightSource(2, new Vector2(47f, 70f) * (float) Game1.tileSize, 2f));
            Game1.currentLightSources.Add(new LightSource(2, new Vector2(52f, 63f) * (float) Game1.tileSize, 2f));
          }
          else
          {
            if (!(key == "wizardWarp"))
              return;
            location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(387, 1965, 16, 31), 9999f, 1, 999999, new Vector2(8f, 16f) * (float) Game1.tileSize + new Vector2(0.0f, (float) Game1.pixelZoom), false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
            {
              motion = new Vector2(2f, -2f),
              acceleration = new Vector2(0.1f, 0.0f),
              scaleChange = -0.02f,
              alphaFade = 1f / 1000f
            });
          }
        }
        else
        {
          if (!(key == "EmilyBoomBoxStart"))
            return;
          location.getTemporarySpriteByID(999).pulse = true;
          location.getTemporarySpriteByID(999).pulseTime = 420f;
        }
      }
      else if (stringHash <= 3992366603U)
      {
        if (stringHash <= 3872194338U)
        {
          if ((int) stringHash != -534653896)
          {
            if ((int) stringHash != -519749352)
            {
              if ((int) stringHash != -422772958 || !(key == "alexDiningDog"))
                return;
              location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(324, 1936, 12, 20), 80f, 4, 99999, new Vector2(7f, 2f) * (float) Game1.tileSize + new Vector2(2f, -8f) * (float) Game1.pixelZoom, false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
              {
                id = 1f
              });
            }
            else
            {
              if (!(key == "maruElectrocution"))
                return;
              location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(432, 1664, 16, 32), 40f, 1, 20, new Vector2(7f, 5f) * (float) Game1.tileSize - new Vector2((float) (-Game1.tileSize / 8 + Game1.pixelZoom), (float) (Game1.tileSize / 8)), true, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false));
            }
          }
          else
          {
            if (!(key == "linusMoney"))
              return;
            location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(397, 1941, 19, 20), 9999f, 1, 99999, new Vector2(-1002f, -1000f) * (float) Game1.tileSize, false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
            {
              startSound = "money",
              delayBeforeAnimationStart = 10,
              overrideLocationDestroy = true
            });
            location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(397, 1941, 19, 20), 9999f, 1, 99999, new Vector2(-1003f, -1002f) * (float) Game1.tileSize, false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
            {
              startSound = "money",
              delayBeforeAnimationStart = 100,
              overrideLocationDestroy = true
            });
            location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(397, 1941, 19, 20), 9999f, 1, 99999, new Vector2(-999f, -1000f) * (float) Game1.tileSize, false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
            {
              startSound = "money",
              delayBeforeAnimationStart = 200,
              overrideLocationDestroy = true
            });
            location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(397, 1941, 19, 20), 9999f, 1, 99999, new Vector2(-1004f, -1001f) * (float) Game1.tileSize, false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
            {
              startSound = "money",
              delayBeforeAnimationStart = 300,
              overrideLocationDestroy = true
            });
            location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(397, 1941, 19, 20), 9999f, 1, 99999, new Vector2(-1001f, -998f) * (float) Game1.tileSize, false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
            {
              startSound = "money",
              delayBeforeAnimationStart = 400,
              overrideLocationDestroy = true
            });
            location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(397, 1941, 19, 20), 9999f, 1, 99999, new Vector2(-998f, -999f) * (float) Game1.tileSize, false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
            {
              startSound = "money",
              delayBeforeAnimationStart = 500,
              overrideLocationDestroy = true
            });
            location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(397, 1941, 19, 20), 9999f, 1, 99999, new Vector2(-998f, -1002f) * (float) Game1.tileSize, false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
            {
              startSound = "money",
              delayBeforeAnimationStart = 600,
              overrideLocationDestroy = true
            });
            location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(397, 1941, 19, 20), 9999f, 1, 99999, new Vector2(-997f, -1001f) * (float) Game1.tileSize, false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
            {
              startSound = "money",
              delayBeforeAnimationStart = 700,
              overrideLocationDestroy = true
            });
          }
        }
        else if ((int) stringHash != -374159723)
        {
          if ((int) stringHash != -369126931)
          {
            if ((int) stringHash != -302600693 || !(key == "parrotSplat"))
              return;
            List<TemporaryAnimatedSprite> aboveMapSprites = this.aboveMapSprites;
            TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(0, 165, 24, 22), 100f, 6, 9999, new Vector2((float) (Game1.viewport.X + Game1.graphics.GraphicsDevice.Viewport.Width), (float) (Game1.viewport.Y + Game1.tileSize)), false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false);
            temporaryAnimatedSprite.id = 999f;
            Vector2 vector2_1 = new Vector2(-2f, 4f);
            temporaryAnimatedSprite.motion = vector2_1;
            Vector2 vector2_2 = new Vector2(-0.1f, 0.0f);
            temporaryAnimatedSprite.acceleration = vector2_2;
            int num1 = 0;
            temporaryAnimatedSprite.delayBeforeAnimationStart = num1;
            int num2 = 87 * Game1.tileSize;
            temporaryAnimatedSprite.yStopCoordinate = num2;
            int num3 = 24 * Game1.tileSize - Game1.tileSize / 2;
            temporaryAnimatedSprite.xStopCoordinate = num3;
            TemporaryAnimatedSprite.endBehavior endBehavior = new TemporaryAnimatedSprite.endBehavior(this.parrotSplat);
            temporaryAnimatedSprite.reachedStopCoordinate = endBehavior;
            aboveMapSprites.Add(temporaryAnimatedSprite);
          }
          else
          {
            if (!(key == "pennyCook"))
              return;
            List<TemporaryAnimatedSprite> temporarySprites1 = location.TemporarySprites;
            TemporaryAnimatedSprite temporaryAnimatedSprite1 = new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(256, 1856, Game1.tileSize, Game1.tileSize * 2), new Vector2(10f, 6f) * (float) Game1.tileSize, false, 0.0f, Color.White);
            temporaryAnimatedSprite1.layerDepth = 1f;
            temporaryAnimatedSprite1.animationLength = 6;
            temporaryAnimatedSprite1.interval = 75f;
            Vector2 vector2_1 = new Vector2(0.0f, -0.5f);
            temporaryAnimatedSprite1.motion = vector2_1;
            temporarySprites1.Add(temporaryAnimatedSprite1);
            List<TemporaryAnimatedSprite> temporarySprites2 = location.TemporarySprites;
            TemporaryAnimatedSprite temporaryAnimatedSprite2 = new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(256, 1856, Game1.tileSize, Game1.tileSize * 2), new Vector2(10f, 6f) * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 4), 0.0f), false, 0.0f, Color.White);
            temporaryAnimatedSprite2.layerDepth = 0.1f;
            temporaryAnimatedSprite2.animationLength = 6;
            temporaryAnimatedSprite2.interval = 75f;
            Vector2 vector2_2 = new Vector2(0.0f, -0.5f);
            temporaryAnimatedSprite2.motion = vector2_2;
            int num1 = 500;
            temporaryAnimatedSprite2.delayBeforeAnimationStart = num1;
            temporarySprites2.Add(temporaryAnimatedSprite2);
            List<TemporaryAnimatedSprite> temporarySprites3 = location.TemporarySprites;
            TemporaryAnimatedSprite temporaryAnimatedSprite3 = new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(256, 1856, Game1.tileSize, Game1.tileSize * 2), new Vector2(10f, 6f) * (float) Game1.tileSize + new Vector2((float) (-Game1.tileSize / 4), 0.0f), false, 0.0f, Color.White);
            temporaryAnimatedSprite3.layerDepth = 1f;
            temporaryAnimatedSprite3.animationLength = 6;
            temporaryAnimatedSprite3.interval = 75f;
            Vector2 vector2_3 = new Vector2(0.0f, -0.5f);
            temporaryAnimatedSprite3.motion = vector2_3;
            int num2 = 750;
            temporaryAnimatedSprite3.delayBeforeAnimationStart = num2;
            temporarySprites3.Add(temporaryAnimatedSprite3);
            List<TemporaryAnimatedSprite> temporarySprites4 = location.TemporarySprites;
            TemporaryAnimatedSprite temporaryAnimatedSprite4 = new TemporaryAnimatedSprite(Game1.animations, new Microsoft.Xna.Framework.Rectangle(256, 1856, Game1.tileSize, Game1.tileSize * 2), new Vector2(10f, 6f) * (float) Game1.tileSize, false, 0.0f, Color.White);
            temporaryAnimatedSprite4.layerDepth = 0.1f;
            temporaryAnimatedSprite4.animationLength = 6;
            temporaryAnimatedSprite4.interval = 75f;
            Vector2 vector2_4 = new Vector2(0.0f, -0.5f);
            temporaryAnimatedSprite4.motion = vector2_4;
            int num3 = 1000;
            temporaryAnimatedSprite4.delayBeforeAnimationStart = num3;
            temporarySprites4.Add(temporaryAnimatedSprite4);
          }
        }
        else
        {
          if (!(key == "linusCampfire"))
            return;
          location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(276, 1985, 12, 11), 50f, 4, 99999, new Vector2(29f, 9f) * (float) Game1.tileSize + new Vector2((float) (Game1.pixelZoom * 2), 0.0f), false, false, (float) (9 * Game1.tileSize) / 10000f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
          {
            light = true,
            lightRadius = 3f,
            lightcolor = Color.Black
          });
        }
      }
      else if (stringHash <= 4119912933U)
      {
        if ((int) stringHash != -267853805)
        {
          if ((int) stringHash != -266170093)
          {
            if ((int) stringHash != -175054363 || !(key == "umbrella"))
              return;
            location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(324, 1843, 27, 23), 80f, 3, 9999, new Vector2(12f, 39f) * (float) Game1.tileSize + new Vector2((float) (-5 * Game1.pixelZoom), (float) (-Game1.tileSize * 3 / 2 - Game1.pixelZoom * 2)), false, false, 1f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false));
          }
          else
          {
            if (!(key == "abbyOuijaCandles"))
              return;
            location.TemporarySprites.Add(new TemporaryAnimatedSprite(737, 999999f, 1, 0, new Vector2(5f, 9f) * (float) Game1.tileSize, false, false)
            {
              light = true,
              lightRadius = 1f
            });
            location.TemporarySprites.Add(new TemporaryAnimatedSprite(737, 999999f, 1, 0, new Vector2(7f, 8f) * (float) Game1.tileSize, false, false)
            {
              light = true,
              lightRadius = 1f
            });
          }
        }
        else
        {
          if (!(key == "candleBoatMove"))
            return;
          location.getTemporarySpriteByID(1).motion = new Vector2(0.0f, 2f);
        }
      }
      else if ((int) stringHash != -106740754)
      {
        if ((int) stringHash != -95746765)
        {
          if ((int) stringHash != -79247288 || !(key == "maruBeaker"))
            return;
          List<TemporaryAnimatedSprite> temporarySprites = location.TemporarySprites;
          TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(738, 1380f, 1, 0, new Vector2(9f, 14f) * (float) Game1.tileSize + new Vector2(0.0f, (float) (Game1.tileSize / 2)), false, false);
          temporaryAnimatedSprite.rotationChange = 0.1308997f;
          Vector2 vector2_1 = new Vector2(0.0f, -7f);
          temporaryAnimatedSprite.motion = vector2_1;
          Vector2 vector2_2 = new Vector2(0.0f, 0.2f);
          temporaryAnimatedSprite.acceleration = vector2_2;
          TemporaryAnimatedSprite.endBehavior endBehavior = new TemporaryAnimatedSprite.endBehavior(this.beakerSmashEndFunction);
          temporaryAnimatedSprite.endFunction = endBehavior;
          double num = 1.0;
          temporaryAnimatedSprite.layerDepth = (float) num;
          temporarySprites.Add(temporaryAnimatedSprite);
        }
        else
        {
          if (!(key == "jasGiftOpen"))
            return;
          location.getTemporarySpriteByID(999).paused = false;
          location.TemporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(537, 1850, 11, 10), 1500f, 1, 1, new Vector2(23f, 16f) * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 4), (float) (-Game1.tileSize * 3) / 4f), false, false, 0.99f, 0.0f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
          {
            motion = new Vector2(0.0f, -0.25f),
            delayBeforeAnimationStart = 500,
            yStopCoordinate = 14 * Game1.tileSize + Game1.tileSize / 2
          });
          location.temporarySprites.AddRange((IEnumerable<TemporaryAnimatedSprite>) Utility.sparkleWithinArea(new Microsoft.Xna.Framework.Rectangle(23 * Game1.tileSize - Game1.tileSize / 2, 16 * Game1.tileSize - Game1.tileSize / 2, Game1.tileSize * 2, Game1.tileSize), 5, Color.White, 300, 0, ""));
        }
      }
      else
      {
        if (!(key == "parrotGone"))
          return;
        location.removeTemporarySpritesWithID(666);
      }
    }

    public void receiveMouseClick(int x, int y)
    {
      int pixelZoom = Game1.pixelZoom;
      Microsoft.Xna.Framework.Rectangle bounds = new Microsoft.Xna.Framework.Rectangle(Game1.pixelZoom * 2, Game1.viewport.Height - Game1.tileSize, 22 * pixelZoom, 15 * pixelZoom);
      Utility.makeSafe(ref bounds);
      if (this.skipped || !this.skippable || !bounds.Contains(x, y))
        return;
      this.skipped = true;
      this.skipEvent();
      Game1.freezeControls = false;
    }

    public void skipEvent()
    {
      Game1.playSound("drumkit6");
      this.actorPositionsAfterMove.Clear();
      foreach (NPC actor in this.actors)
      {
        actor.Halt();
        actor.CurrentDialogue.Clear();
      }
      Game1.player.Halt();
      Game1.exitActiveMenu();
      Game1.dialogueUp = false;
      Game1.dialogueTyping = false;
      switch (this.id)
      {
        case 739330:
          if (Game1.player.hasItemWithNameThatContains("Bamboo Pole") == null)
            Game1.player.addItemByMenuIfNecessary((Item) new FishingRod(), (ItemGrabMenu.behaviorOnItemSelect) null);
          this.endBehaviors(new string[4]
          {
            "end",
            "position",
            "43",
            "36"
          }, Game1.currentLocation);
          break;
        case 992553:
          if (!Game1.player.craftingRecipes.ContainsKey("Furnace"))
            Game1.player.craftingRecipes.Add("Furnace", 0);
          if (!Game1.player.hasQuest(11))
            Game1.player.addQuest(11);
          this.endBehaviors(new string[1]{ "end" }, Game1.currentLocation);
          break;
        case 100162:
          if (Game1.player.hasItemWithNameThatContains("Rusty Sword") == null)
            Game1.player.addItemByMenuIfNecessary((Item) new MeleeWeapon(0), (ItemGrabMenu.behaviorOnItemSelect) null);
          Game1.player.position = new Vector2(-9999f, -99999f);
          this.endBehaviors(new string[1]{ "end" }, Game1.currentLocation);
          break;
        case 558292:
          Game1.player.eventsSeen.Remove(2146991);
          this.endBehaviors(new string[2]{ "end", "bed" }, Game1.currentLocation);
          break;
        case 19:
          if (!Game1.player.cookingRecipes.ContainsKey("Cookies"))
            Game1.player.cookingRecipes.Add("Cookies", 0);
          this.endBehaviors(new string[1]{ "end" }, Game1.currentLocation);
          break;
        case 112:
          this.endBehaviors(new string[1]{ "end" }, Game1.currentLocation);
          Game1.player.mailReceived.Add("canReadJunimoText");
          break;
        case 60367:
          this.endBehaviors(new string[2]
          {
            "end",
            "beginGame"
          }, Game1.currentLocation);
          break;
        default:
          this.endBehaviors(new string[1]{ "end" }, Game1.currentLocation);
          break;
      }
    }

    public void receiveKeyPress(Keys k)
    {
    }

    public void receiveKeyRelease(Keys k)
    {
    }

    public void receiveActionPress(int xTile, int yTile)
    {
      if (xTile != this.playerControlTargetTile.X || yTile != this.playerControlTargetTile.Y)
        return;
      string controlSequenceId = this.playerControlSequenceID;
      if (!(controlSequenceId == "haleyBeach"))
      {
        if (!(controlSequenceId == "haleyBeach2"))
          return;
        this.CurrentCommand = this.CurrentCommand + 1;
      }
      else
      {
        this.props.Clear();
        Game1.playSound("coin");
        this.playerControlTargetTile = new Point(35, 11);
        this.playerControlSequenceID = "haleyBeach2";
      }
    }

    public void startSecretSantaEvent()
    {
      this.playerControlSequence = false;
      this.playerControlSequenceID = (string) null;
      this.eventCommands = this.festivalData["secretSanta"].Split('/');
      this.setUpSecretSantaCommands();
      this.currentCommand = 0;
    }

    public void festivalUpdate(GameTime time)
    {
      if (this.festivalTimer > 0)
      {
        this.oldTime = this.festivalTimer;
        this.festivalTimer = this.festivalTimer - time.ElapsedGameTime.Milliseconds;
        if (this.playerControlSequenceID == "iceFishing")
        {
          if (!Game1.player.usingTool)
            Game1.player.forceCanMove();
          if (this.oldTime % 500 < this.festivalTimer % 500)
          {
            NPC actorByName1 = this.getActorByName("Pam");
            actorByName1.sprite.sourceRect.Offset(actorByName1.sprite.SourceRect.Width, 0);
            if (actorByName1.sprite.sourceRect.X >= actorByName1.sprite.Texture.Width)
              actorByName1.sprite.sourceRect.Offset(-actorByName1.sprite.Texture.Width, 0);
            NPC actorByName2 = this.getActorByName("Elliott");
            actorByName2.sprite.sourceRect.Offset(actorByName2.sprite.SourceRect.Width, 0);
            if (actorByName2.sprite.sourceRect.X >= actorByName2.sprite.Texture.Width)
              actorByName2.sprite.sourceRect.Offset(-actorByName2.sprite.Texture.Width, 0);
            NPC actorByName3 = this.getActorByName("Willy");
            actorByName3.sprite.sourceRect.Offset(actorByName3.sprite.SourceRect.Width, 0);
            if (actorByName3.sprite.sourceRect.X >= actorByName3.sprite.Texture.Width)
              actorByName3.sprite.sourceRect.Offset(-actorByName3.sprite.Texture.Width, 0);
          }
          if (this.oldTime % 29900 < this.festivalTimer % 29900)
          {
            this.getActorByName("Willy").shake(500);
            Game1.playSound("dwop");
            List<TemporaryAnimatedSprite> temporarySprites = this.temporaryLocation.temporarySprites;
            TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(112, 432, 16, 16), this.getActorByName("Willy").position + new Vector2(0.0f, (float) (-Game1.tileSize * 3 / 2)), false, 0.015f, Color.White);
            temporaryAnimatedSprite.layerDepth = 1f;
            double pixelZoom = (double) Game1.pixelZoom;
            temporaryAnimatedSprite.scale = (float) pixelZoom;
            double num = 9999.0;
            temporaryAnimatedSprite.interval = (float) num;
            Vector2 vector2 = new Vector2(0.0f, -1f);
            temporaryAnimatedSprite.motion = vector2;
            temporarySprites.Add(temporaryAnimatedSprite);
          }
          if (this.oldTime % 45900 < this.festivalTimer % 45900)
          {
            this.getActorByName("Pam").shake(500);
            Game1.playSound("dwop");
            List<TemporaryAnimatedSprite> temporarySprites = this.temporaryLocation.temporarySprites;
            TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(112, 432, 16, 16), this.getActorByName("Pam").position + new Vector2(0.0f, (float) (-Game1.tileSize * 3 / 2)), false, 0.015f, Color.White);
            temporaryAnimatedSprite.layerDepth = 1f;
            double pixelZoom = (double) Game1.pixelZoom;
            temporaryAnimatedSprite.scale = (float) pixelZoom;
            double num = 9999.0;
            temporaryAnimatedSprite.interval = (float) num;
            Vector2 vector2 = new Vector2(0.0f, -1f);
            temporaryAnimatedSprite.motion = vector2;
            temporarySprites.Add(temporaryAnimatedSprite);
          }
          if (this.oldTime % 59900 < this.festivalTimer % 59900)
          {
            this.getActorByName("Elliott").shake(500);
            Game1.playSound("dwop");
            List<TemporaryAnimatedSprite> temporarySprites = this.temporaryLocation.temporarySprites;
            TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(this.festivalTexture, new Microsoft.Xna.Framework.Rectangle(112, 432, 16, 16), this.getActorByName("Elliott").position + new Vector2(0.0f, (float) (-Game1.tileSize * 3 / 2)), false, 0.015f, Color.White);
            temporaryAnimatedSprite.layerDepth = 1f;
            double pixelZoom = (double) Game1.pixelZoom;
            temporaryAnimatedSprite.scale = (float) pixelZoom;
            double num = 9999.0;
            temporaryAnimatedSprite.interval = (float) num;
            Vector2 vector2 = new Vector2(0.0f, -1f);
            temporaryAnimatedSprite.motion = vector2;
            temporarySprites.Add(temporaryAnimatedSprite);
          }
        }
        if (this.festivalTimer <= 0)
        {
          string controlSequenceId = this.playerControlSequenceID;
          if (!(controlSequenceId == "eggHunt"))
          {
            if (controlSequenceId == "iceFishing")
            {
              this.playerControlSequence = false;
              this.playerControlSequenceID = (string) null;
              this.eventCommands = this.festivalData["afterIceFishing"].Split('/');
              this.currentCommand = 0;
              if (Game1.activeClickableMenu != null)
                Game1.activeClickableMenu.emergencyShutDown();
              Game1.activeClickableMenu = (IClickableMenu) null;
              if (Game1.player.UsingTool && Game1.player.CurrentTool != null && Game1.player.CurrentTool is FishingRod)
                (Game1.player.CurrentTool as FishingRod).doneFishing(Game1.player, false);
              Game1.screenOverlayTempSprites.Clear();
              if (this.tempItemStash != null)
              {
                Game1.player.addItemToInventory(this.tempItemStash, 0);
                this.tempItemStash = (Item) null;
              }
              Game1.player.forceCanMove();
            }
          }
          else
          {
            this.playerControlSequence = false;
            this.playerControlSequenceID = (string) null;
            this.eventCommands = this.festivalData["afterEggHunt"].Split('/');
            this.currentCommand = 0;
          }
        }
      }
      if (this.underwaterSprites != null)
      {
        foreach (TemporaryAnimatedSprite underwaterSprite in this.underwaterSprites)
          underwaterSprite.update(time);
      }
      if (this.startSecretSantaAfterDialogue && !Game1.dialogueUp)
      {
        Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.startSecretSantaEvent), 0.01f);
        this.startSecretSantaAfterDialogue = false;
      }
      Game1.player.festivalScore = Math.Min(Game1.player.festivalScore, 9999);
      if (!this.waitingForMenuClose || Game1.activeClickableMenu != null)
        return;
      if (this.festivalData["file"] == "fall16")
      {
        MultiplayerUtility.sendMessageToEveryone(2, "null", 0L);
        if (Game1.IsServer)
          this.playerUsingGrangeDisplay = (Farmer) null;
      }
      this.waitingForMenuClose = false;
    }

    private void setUpSecretSantaCommands()
    {
      int tileX;
      int tileY;
      try
      {
        tileX = this.getActorByName(this.mySecretSanta.name).getTileX();
        tileY = this.getActorByName(this.mySecretSanta.name).getTileY();
      }
      catch (Exception ex)
      {
        this.mySecretSanta = this.getActorByName("Lewis");
        tileX = this.getActorByName(this.mySecretSanta.name).getTileX();
        tileY = this.getActorByName(this.mySecretSanta.name).getTileY();
      }
      string newValue1 = "";
      string newValue2 = "";
      switch (this.mySecretSanta.age)
      {
        case 0:
        case 1:
          switch (this.mySecretSanta.manners)
          {
            case 0:
            case 1:
              newValue1 = Game1.LoadStringByGender(this.mySecretSanta.gender, "Strings\\StringsFromCSFiles:Event.cs.1499");
              newValue2 = Game1.LoadStringByGender(this.mySecretSanta.gender, "Strings\\StringsFromCSFiles:Event.cs.1500");
              break;
            case 2:
              newValue1 = Game1.LoadStringByGender(this.mySecretSanta.gender, "Strings\\StringsFromCSFiles:Event.cs.1501");
              newValue2 = this.mySecretSanta.name.Equals("George") ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1503") : Game1.LoadStringByGender(this.mySecretSanta.gender, "Strings\\StringsFromCSFiles:Event.cs.1504");
              break;
          }
        case 2:
          newValue1 = Game1.LoadStringByGender(this.mySecretSanta.gender, "Strings\\StringsFromCSFiles:Event.cs.1497");
          newValue2 = Game1.LoadStringByGender(this.mySecretSanta.gender, "Strings\\StringsFromCSFiles:Event.cs.1498");
          break;
      }
      for (int index = 0; index < this.eventCommands.Length; ++index)
      {
        this.eventCommands[index] = this.eventCommands[index].Replace("secretSanta", this.mySecretSanta.name);
        this.eventCommands[index] = this.eventCommands[index].Replace("warpX", string.Concat((object) tileX));
        this.eventCommands[index] = this.eventCommands[index].Replace("warpY", string.Concat((object) tileY));
        this.eventCommands[index] = this.eventCommands[index].Replace("dialogue1", newValue1);
        this.eventCommands[index] = this.eventCommands[index].Replace("dialogue2", newValue2);
      }
    }

    public void draw(SpriteBatch b)
    {
      foreach (NPC actor in this.actors)
      {
        actor.name.Equals("Marcello");
        if (actor.ySourceRectOffset == 0)
          actor.draw(b);
        else
          actor.draw(b, actor.ySourceRectOffset, 1f);
      }
      foreach (Object prop in this.props)
        prop.drawAsProp(b);
      foreach (Prop festivalProp in this.festivalProps)
        festivalProp.draw(b);
      if (this.isFestival)
      {
        foreach (Character farmer in Game1.currentLocation.farmers)
          farmer.draw(b);
        if (Game1.IsMultiplayer && Game1.ChatBox != null)
          Game1.ChatBox.draw(b);
        if (this.festivalData["file"] == "fall16" && this.grangeDisplay != null)
        {
          Vector2 local = Game1.GlobalToLocal(Game1.viewport, new Vector2(37f, 56f) * (float) Game1.tileSize);
          local.X += (float) Game1.pixelZoom;
          int num = (int) local.X + 3 * (Game1.tileSize - Game1.pixelZoom * 2);
          local.Y += (float) (Game1.tileSize / 8);
          for (int index = 0; index < this.grangeDisplay.Count; ++index)
          {
            if (this.grangeDisplay[index] != null)
            {
              local.Y += (float) (Game1.tileSize * 2 / 3);
              local.X += (float) Game1.pixelZoom;
              b.Draw(Game1.shadowTexture, local, new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.0001f);
              local.Y -= (float) (Game1.tileSize * 2 / 3);
              local.X -= (float) Game1.pixelZoom;
              this.grangeDisplay[index].drawInMenu(b, local, 1f, 1f, (float) ((double) index / 1000.0 + 1.0 / 1000.0), false);
            }
            local.X += (float) (Game1.tileSize - Game1.pixelZoom);
            if ((double) local.X >= (double) num)
            {
              local.X = (float) (num - (Game1.tileSize - Game1.pixelZoom * 2) * 3);
              local.Y += (float) Game1.tileSize;
            }
          }
        }
      }
      if (!this.drawTool)
        return;
      Game1.drawTool(Game1.player);
    }

    public void drawUnderWater(SpriteBatch b)
    {
      if (this.underwaterSprites == null)
        return;
      foreach (TemporaryAnimatedSprite underwaterSprite in this.underwaterSprites)
        underwaterSprite.draw(b, false, 0, 0);
    }

    public void drawAfterMap(SpriteBatch b)
    {
      if (this.aboveMapSprites != null)
      {
        foreach (TemporaryAnimatedSprite aboveMapSprite in this.aboveMapSprites)
          aboveMapSprite.draw(b, false, 0, 0);
      }
      if (this.playerControlSequenceID != null)
      {
        string controlSequenceId = this.playerControlSequenceID;
        if (!(controlSequenceId == "eggHunt"))
        {
          if (!(controlSequenceId == "fair"))
          {
            if (controlSequenceId == "iceFishing")
            {
              b.End();
              b.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
              b.Draw(Game1.fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle(Game1.tileSize / 4, Game1.tileSize / 4, Game1.tileSize * 2 + (Game1.player.festivalScore > 999 ? Game1.tileSize / 4 : 0), Game1.tileSize * 2), Color.Black * 0.75f);
              b.Draw(this.festivalTexture, new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 4)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(112, 432, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1f);
              Game1.drawWithBorder(string.Concat((object) Game1.player.festivalScore), Color.Black, Color.White, new Vector2((float) (Game1.tileSize / 2 + 16 * Game1.pixelZoom), (float) (Game1.tileSize / 3 + (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.en ? Game1.pixelZoom * 2 : (LocalizedContentManager.CurrentLanguageLatin ? Game1.pixelZoom * 4 : Game1.pixelZoom * 2)))), 0.0f, 1f, 1f, false);
              Game1.drawWithBorder(Utility.getMinutesSecondsStringFromMilliseconds(this.festivalTimer), Color.Black, Color.White, new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 3 + Game1.tileSize + Game1.pixelZoom * 2)), 0.0f, 1f, 1f, false);
              b.End();
              b.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
            }
          }
          else
          {
            b.End();
            b.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
            b.Draw(Game1.fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle(Game1.tileSize / 4, Game1.tileSize / 4, Game1.tileSize * 2 + (Game1.player.festivalScore > 999 ? Game1.tileSize / 4 : 0), Game1.tileSize), Color.Black * 0.75f);
            b.Draw(Game1.mouseCursors, new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(338, 400, 8, 8)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 1f);
            Game1.drawWithBorder(string.Concat((object) Game1.player.festivalScore), Color.Black, Color.White, new Vector2((float) (Game1.tileSize / 2 + 10 * Game1.pixelZoom), (float) (Game1.tileSize / 3 + (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.en ? Game1.pixelZoom * 2 : (LocalizedContentManager.CurrentLanguageLatin ? Game1.pixelZoom * 4 : Game1.pixelZoom * 2)))), 0.0f, 1f, 1f, false);
            if (Game1.activeClickableMenu == null)
              Game1.dayTimeMoneyBox.drawMoneyBox(b, Game1.dayTimeMoneyBox.xPositionOnScreen, Game1.pixelZoom);
            b.End();
            b.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null);
          }
        }
        else
        {
          b.Draw(Game1.fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle(Game1.tileSize / 2, Game1.tileSize / 2, Game1.tileSize * 3 + Game1.tileSize / 2, Game1.tileSize * 2 + Game1.tileSize / 2), Color.Black * 0.5f);
          Game1.drawWithBorder(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1514", (object) (this.festivalTimer / 1000)), Color.Black, Color.Yellow, new Vector2((float) Game1.tileSize, (float) Game1.tileSize), 0.0f, 1f, 1f, false);
          Game1.drawWithBorder(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1515", (object) Game1.player.festivalScore), Color.Black, Color.Pink, new Vector2((float) Game1.tileSize, (float) (Game1.tileSize * 2)), 0.0f, 1f, 1f, false);
        }
      }
      foreach (Character actor in this.actors)
        actor.drawAboveAlwaysFrontLayer(b);
      if (this.skippable && !Game1.options.SnappyMenus)
      {
        int pixelZoom = Game1.pixelZoom;
        Microsoft.Xna.Framework.Rectangle bounds = new Microsoft.Xna.Framework.Rectangle(Game1.pixelZoom * 2, Game1.viewport.Height - Game1.tileSize, 22 * pixelZoom, 15 * pixelZoom);
        Utility.makeSafe(ref bounds);
        Color white = Color.White;
        if (bounds.Contains(Game1.getOldMouseX(), Game1.getOldMouseY()))
          white *= 0.5f;
        Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle(205, 406, 22, 15);
        b.Draw(Game1.mouseCursors, Utility.PointToVector2(bounds.Location), new Microsoft.Xna.Framework.Rectangle?(rectangle), white, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.92f);
      }
      if (this.isFestival || Game1.options.hardwareCursor || Game1.options.SnappyMenus)
        return;
      b.Draw(Game1.mouseCursors, new Vector2((float) Game1.getOldMouseX(), (float) Game1.getOldMouseY()), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 0, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
    }

    public void setUpPlayerControlSequence(string id)
    {
      this.playerControlSequenceID = id;
      this.playerControlSequence = true;
      Game1.player.CanMove = true;
      foreach (Farmer farmer in Game1.otherFarmers.Values)
        farmer.CanMove = true;
      Game1.viewportFreeze = false;
      // ISSUE: reference to a compiler-generated method
      uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(id);
      if (stringHash <= 1073062698U)
      {
        if (stringHash <= 750634491U)
        {
          if ((int) stringHash != 5462067)
          {
            if ((int) stringHash != 750634491 || !(id == "christmas"))
              return;
            Random r = new Random((int) (Game1.uniqueIDForThisGame / 2UL) - Game1.year);
            this.secretSantaRecipient = Utility.getRandomTownNPC(r, Utility.getFarmerNumberFromFarmer(Game1.player));
            while (this.mySecretSanta == null || this.mySecretSanta.Equals((object) this.secretSantaRecipient))
              this.mySecretSanta = Utility.getRandomTownNPC(r, Utility.getFarmerNumberFromFarmer(Game1.player) + 10);
            Game1.debugOutput = "Secret Santa Recipient: " + this.secretSantaRecipient.name + "  My Secret Santa: " + this.mySecretSanta.name;
          }
          else
          {
            if (!(id == "fair"))
              return;
            this.festivalHost = this.getActorByName("Lewis");
            this.hostMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1535");
          }
        }
        else if ((int) stringHash != 863075767)
        {
          if ((int) stringHash != 875582698)
          {
            if ((int) stringHash != 1073062698 || !(id == "flowerFestival"))
              return;
            this.festivalHost = this.getActorByName("Lewis");
            this.hostMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1524");
          }
          else
          {
            if (!(id == "eggFestival"))
              return;
            this.festivalHost = this.getActorByName("Lewis");
            this.hostMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1521");
          }
        }
        else
        {
          if (!(id == "eggHunt"))
            return;
          for (int tileX = 0; tileX < Game1.currentLocation.map.GetLayer("Paths").LayerWidth; ++tileX)
          {
            for (int tileY = 0; tileY < Game1.currentLocation.map.GetLayer("Paths").LayerHeight; ++tileY)
            {
              if (Game1.currentLocation.map.GetLayer("Paths").Tiles[tileX, tileY] != null)
                this.festivalProps.Add(new Prop(this.festivalTexture, Game1.currentLocation.map.GetLayer("Paths").Tiles[tileX, tileY].TileIndex, 1, 1, 1, tileX, tileY, true));
            }
          }
          this.festivalTimer = 52000;
          this.currentCommand = this.currentCommand + 1;
        }
      }
      else if (stringHash <= 2614493766U)
      {
        if ((int) stringHash != 2052688871)
        {
          if ((int) stringHash != -2117052016)
          {
            if ((int) stringHash != -1680473530 || !(id == "iceFishing"))
              return;
            this.festivalTimer = 120000;
            foreach (Farmer farmer in this.temporaryLocation.getFarmers())
            {
              int num1 = 0;
              farmer.festivalScore = num1;
              int num2 = 0;
              farmer.CurrentToolIndex = num2;
              this.tempItemStash = Game1.player.addItemToInventory((Item) new FishingRod(), 0);
              (farmer.CurrentTool as FishingRod).attachments[0] = new Object(690, 99, false, -1, 0);
              (farmer.CurrentTool as FishingRod).attachments[1] = new Object(687, 1, false, -1, 0);
              int num3 = 0;
              farmer.CurrentToolIndex = num3;
            }
          }
          else
          {
            if (!(id == "iceFestival"))
              return;
            this.festivalHost = this.getActorByName("Lewis");
            this.hostMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1548");
          }
        }
        else
        {
          if (!(id == "haleyBeach"))
            return;
          this.playerControlTargetTile = new Point(53, 8);
          this.props.Add(new Object(new Vector2(53f, 8f), 742, 1)
          {
            flipped = false
          });
          Game1.player.canOnlyWalk = false;
        }
      }
      else if ((int) stringHash != -938212325)
      {
        if ((int) stringHash != -671295024)
        {
          if ((int) stringHash != -518763012 || !(id == "luau"))
            return;
          this.festivalHost = this.getActorByName("Lewis");
          this.hostMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1527");
        }
        else
        {
          if (!(id == "halloween"))
            return;
          SerializableDictionary<Vector2, Object> objects = this.temporaryLocation.objects;
          Vector2 key = new Vector2(33f, 13f);
          int coins = 0;
          List<Item> items = new List<Item>();
          items.Add((Item) new Object(373, 1, false, -1, 0));
          Vector2 location = new Vector2(33f, 13f);
          int num = 0;
          Chest chest = new Chest(coins, items, location, num != 0);
          objects.Add(key, (Object) chest);
        }
      }
      else
      {
        if (!(id == "jellies"))
          return;
        this.festivalHost = this.getActorByName("Lewis");
        this.hostMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1531");
      }
    }

    public bool canMoveAfterDialogue()
    {
      if (this.playerControlSequenceID != null && this.playerControlSequenceID.Equals("eggHunt"))
      {
        Game1.player.canMove = true;
        this.CurrentCommand = this.CurrentCommand + 1;
      }
      return this.playerControlSequence;
    }

    public void forceFestivalContinue()
    {
      if (this.festivalData["file"].Equals("fall16"))
      {
        this.initiateGrangeJudging();
      }
      else
      {
        Game1.dialogueUp = false;
        if (Game1.activeClickableMenu != null)
          Game1.activeClickableMenu.emergencyShutDown();
        Game1.exitActiveMenu();
        this.eventCommands = this.festivalData["mainEvent"].Split('/');
        this.CurrentCommand = 0;
        this.eventSwitched = true;
        this.playerControlSequence = false;
        this.setUpFestivalMainEvent();
      }
    }

    public void setUpFestivalMainEvent()
    {
      if (!this.festivalData["file"].Equals("spring24"))
        return;
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>();
      List<string> source = new List<string>()
      {
        "Abigail",
        "Penny",
        "Leah",
        "Maru",
        "Haley",
        "Emily"
      };
      List<string> stringList3 = new List<string>()
      {
        "Sebastian",
        "Sam",
        "Elliott",
        "Harvey",
        "Alex",
        "Shane"
      };
      for (int index = 0; index < Game1.numberOfPlayers(); ++index)
      {
        Farmer fromFarmerNumber = Utility.getFarmerFromFarmerNumber(index + 1);
        if (fromFarmerNumber.dancePartner != null)
        {
          if (fromFarmerNumber.dancePartner.gender == 1)
          {
            stringList1.Add(fromFarmerNumber.dancePartner.name);
            source.Remove(fromFarmerNumber.dancePartner.name);
            stringList2.Add("farmer" + (object) (index + 1));
          }
          else
          {
            stringList2.Add(fromFarmerNumber.dancePartner.name);
            stringList3.Remove(fromFarmerNumber.dancePartner.name);
            stringList1.Add("farmer" + (object) (index + 1));
          }
        }
      }
      while (stringList1.Count < 6)
      {
        string who = source.Last<string>();
        if (stringList3.Contains(Utility.getLoveInterest(who)))
        {
          stringList1.Add(who);
          stringList2.Add(Utility.getLoveInterest(who));
        }
        source.Remove(who);
      }
      string str = this.festivalData["mainEvent"];
      for (int index = 1; index <= 6; ++index)
        str = str.Replace("Girl" + (object) index, stringList1[index - 1]).Replace("Guy" + (object) index, stringList2[index - 1]);
      Regex regex1 = new Regex("showFrame (?<farmerName>farmer\\d) 44");
      Regex regex2 = new Regex("showFrame (?<farmerName>farmer\\d) 40");
      Regex regex3 = new Regex("animate (?<farmerName>farmer\\d) false true 600 44 45");
      Regex regex4 = new Regex("animate (?<farmerName>farmer\\d) false true 600 43 41 43 42");
      Regex regex5 = new Regex("animate (?<farmerName>farmer\\d) false true 300 46 47");
      Regex regex6 = new Regex("animate (?<farmerName>farmer\\d) false true 600 46 47");
      string input1 = str;
      string replacement = "showFrame $1 12/faceDirection $1 0";
      string input2 = regex1.Replace(input1, replacement);
      string input3 = regex2.Replace(input2, "showFrame $1 0/faceDirection $1 2");
      string input4 = regex3.Replace(input3, "animate $1 false true 600 12 13 12 14");
      string input5 = regex4.Replace(input4, "animate $1 false true 596 4 0");
      string input6 = regex5.Replace(input5, "animate $1 false true 150 12 13 12 14");
      this.eventCommands = regex6.Replace(input6, "animate $1 false true 600 0 3").Split('/');
    }

    public string FestivalName
    {
      get
      {
        if (this.festivalData == null)
          return "";
        return this.festivalData["name"];
      }
    }

    private void lewisDoneJudgingGrange()
    {
      int num1 = 14;
      Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
      int num2 = 0;
      bool flag = false;
      if (this.grangeDisplay != null)
      {
        foreach (Item obj in this.grangeDisplay)
        {
          if (obj != null && obj is Object)
          {
            if ((obj as Object).parentSheetIndex == 789)
              flag = true;
            num1 += (obj as Object).quality + 1;
            int storePrice = (obj as Object).sellToStorePrice();
            int num3 = 20;
            if (storePrice >= num3)
              ++num1;
            int num4 = 90;
            if (storePrice >= num4)
              ++num1;
            int num5 = 200;
            if (storePrice >= num5)
              ++num1;
            int num6 = 300;
            if (storePrice >= num6 && (obj as Object).quality < 2)
              ++num1;
            int num7 = 400;
            if (storePrice >= num7 && (obj as Object).quality < 1)
              ++num1;
            switch ((obj as Object).Category)
            {
              case -26:
                dictionary[-26] = true;
                continue;
              case -18:
              case -14:
              case -6:
              case -5:
                dictionary[-5] = true;
                continue;
              case -12:
              case -2:
                dictionary[-12] = true;
                continue;
              case -7:
                dictionary[-7] = true;
                continue;
              case -4:
                dictionary[-4] = true;
                continue;
              case -81:
              case -80:
              case -27:
                dictionary[-81] = true;
                continue;
              case -79:
                dictionary[-79] = true;
                continue;
              case -75:
                dictionary[-75] = true;
                continue;
              default:
                continue;
            }
          }
          else if (obj == null)
            ++num2;
        }
      }
      int num8 = num1 + Math.Min(30, dictionary.Count * 5);
      int num9 = 9 - 2 * num2;
      if (this.grangeDisplay == null)
        num9 = 0;
      this.grangeScore = num8 + num9;
      if (flag)
        this.grangeScore = -666;
      if (Game1.IsServer)
      {
        MultiplayerUtility.sendMessageToEveryone(4, string.Concat((object) this.grangeScore), Game1.player.uniqueMultiplayerID);
        Game1.ChatBox.receiveChatMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1584"), -1L);
        Game1.player.Halt();
      }
      else if (Game1.activeClickableMenu == null)
      {
        Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1584")));
        Game1.player.Halt();
      }
      this.interpretGrangeResults();
    }

    public void interpretGrangeResults()
    {
      List<Character> characterList = new List<Character>();
      characterList.Add((Character) this.getActorByName("Pierre"));
      characterList.Add((Character) this.getActorByName("Marnie"));
      characterList.Add((Character) this.getActorByName("Willy"));
      if (this.grangeScore >= 90)
        characterList.Insert(0, (Character) Game1.player);
      else if (this.grangeScore >= 75)
        characterList.Insert(1, (Character) Game1.player);
      else if (this.grangeScore >= 60)
        characterList.Insert(2, (Character) Game1.player);
      else
        characterList.Add((Character) Game1.player);
      if (characterList[0] is NPC && characterList[0].name.Equals("Pierre"))
        this.getActorByName("Pierre").setNewDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1591"), false, false);
      else
        this.getActorByName("Pierre").setNewDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1593"), false, false);
      this.getActorByName("Marnie").setNewDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1595"), false, false);
      this.getActorByName("Willy").setNewDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1597"), false, false);
      if (this.grangeScore != -666)
        return;
      this.getActorByName("Marnie").setNewDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1600"), false, false);
    }

    private void initiateGrangeJudging()
    {
      this.hostMessage = (string) null;
      this.setUpAdvancedMove("advancedMove Lewis False 2 0 0 7 8 0 4 3000 3 0 4 3000 3 0 4 3000 3 0 4 3000 -14 0 2 1000".Split(' '), new NPCController.endBehavior(this.lewisDoneJudgingGrange));
      this.getActorByName("Lewis").CurrentDialogue.Clear();
      this.setUpAdvancedMove("advancedMove Marnie False 0 1 4 1000".Split(' '), (NPCController.endBehavior) null);
      this.getActorByName("Marnie").setNewDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1602"), false, false);
      this.getActorByName("Pierre").setNewDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1604"), false, false);
      this.getActorByName("Willy").setNewDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1606"), false, false);
    }

    public void answerDialogueQuestion(NPC who, string answerKey)
    {
      if (!this.isFestival)
        return;
      if (!(answerKey == "yes"))
      {
        if (answerKey == "no" || !(answerKey == "danceAsk"))
          return;
        if (Game1.player.spouse != null && who.name.Equals(Game1.player.spouse))
        {
          Game1.player.dancePartner = who;
          who.hasPartnerForDance = true;
          string spouse = Game1.player.spouse;
          // ISSUE: reference to a compiler-generated method
          uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(spouse);
          if (stringHash <= 1708213605U)
          {
            if (stringHash <= 587846041U)
            {
              if ((int) stringHash != 161540545)
              {
                if ((int) stringHash == 587846041 && spouse == "Penny")
                {
                  who.setNewDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1615"), false, false);
                  goto label_41;
                }
              }
              else if (spouse == "Sebastian")
              {
                who.setNewDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1623"), false, false);
                goto label_41;
              }
            }
            else if ((int) stringHash != 1067922812)
            {
              if ((int) stringHash != 1281010426)
              {
                if ((int) stringHash == 1708213605 && spouse == "Alex")
                {
                  who.setNewDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1631"), false, false);
                  goto label_41;
                }
              }
              else if (spouse == "Maru")
              {
                who.setNewDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1617"), false, false);
                goto label_41;
              }
            }
            else if (spouse == "Sam")
            {
              who.setNewDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1625"), false, false);
              goto label_41;
            }
          }
          else if (stringHash <= 2434294092U)
          {
            if ((int) stringHash != 2010304804)
            {
              if ((int) stringHash == -1860673204 && spouse == "Haley")
              {
                who.setNewDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1621"), false, false);
                goto label_41;
              }
            }
            else if (spouse == "Harvey")
            {
              who.setNewDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1627"), false, false);
              goto label_41;
            }
          }
          else if ((int) stringHash != -1562053956)
          {
            if ((int) stringHash != -1468719973)
            {
              if ((int) stringHash == -1228790996 && spouse == "Elliott")
              {
                who.setNewDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1629"), false, false);
                goto label_41;
              }
            }
            else if (spouse == "Leah")
            {
              who.setNewDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1619"), false, false);
              goto label_41;
            }
          }
          else if (spouse == "Abigail")
          {
            who.setNewDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1613"), false, false);
            goto label_41;
          }
          who.setNewDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1632"), false, false);
label_41:
          foreach (NPC actor in this.actors)
          {
            if (actor.CurrentDialogue != null && actor.CurrentDialogue.Count > 0 && actor.CurrentDialogue.Peek().getCurrentDialogue().Equals("..."))
              actor.CurrentDialogue.Clear();
          }
        }
        else if (!who.hasPartnerForDance && Game1.player.getFriendshipLevelForNPC(who.name) >= 1000)
        {
          string s = "";
          switch (who.gender)
          {
            case 0:
              s = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1633");
              break;
            case 1:
              s = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1634");
              break;
          }
          if (Game1.IsMultiplayer)
            MultiplayerUtility.sendMessageToEveryone(0, who.name, Game1.player.uniqueMultiplayerID);
          try
          {
            Game1.player.changeFriendship(250, Game1.getCharacterFromName(who.name, false));
          }
          catch (Exception ex)
          {
          }
          if (!Game1.IsMultiplayer || Game1.IsServer)
          {
            Game1.player.dancePartner = who;
            who.hasPartnerForDance = true;
          }
          who.setNewDialogue(s, false, false);
          foreach (NPC actor in this.actors)
          {
            if (actor.CurrentDialogue != null && actor.CurrentDialogue.Count > 0 && actor.CurrentDialogue.Peek().getCurrentDialogue().Equals("..."))
              actor.CurrentDialogue.Clear();
          }
        }
        else if (who.hasPartnerForDance)
        {
          who.setNewDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1635"), false, false);
        }
        else
        {
          try
          {
            who.setNewDialogue(Game1.content.Load<Dictionary<string, string>>("Characters\\Dialogue\\" + who.name)["danceRejection"], false, false);
          }
          catch (Exception ex)
          {
            return;
          }
        }
        Game1.drawDialogue(who);
        who.immediateSpeak = true;
        who.facePlayer(Game1.player);
        who.Halt();
      }
      else if (this.festivalData["file"].Equals("fall16"))
      {
        this.initiateGrangeJudging();
        if (!Game1.IsServer)
          return;
        MultiplayerUtility.sendServerToClientsMessage("festivalEvent");
      }
      else
      {
        this.eventCommands = this.festivalData["mainEvent"].Split('/');
        this.CurrentCommand = 0;
        this.eventSwitched = true;
        this.playerControlSequence = false;
        this.setUpFestivalMainEvent();
        if (!Game1.IsServer)
          return;
        MultiplayerUtility.sendServerToClientsMessage("festivalEvent");
      }
    }

    public void addItemToGrangeDisplay(Item i, int position, bool force)
    {
      if (this.grangeDisplay == null)
      {
        this.grangeDisplay = new List<Item>();
        for (int index = 0; index < 9; ++index)
          this.grangeDisplay.Add((Item) null);
      }
      if (position < 0 || position >= this.grangeDisplay.Count || this.grangeDisplay[position] != null && !force)
        return;
      this.grangeDisplay[position] = i;
    }

    public void setGrangeDisplayUser(Farmer who)
    {
      this.playerUsingGrangeDisplay = who;
      if (who != null && who.IsMainPlayer)
      {
        who.Halt();
        who.movementDirections.Clear();
        Game1.activeClickableMenu = (IClickableMenu) new StorageContainer(this.grangeDisplay, 9, 3, new StorageContainer.behaviorOnItemChange(this.onGrangeChange), new InventoryMenu.highlightThisItem(Utility.highlightSmallObjects));
        this.waitingForMenuClose = true;
      }
      if (!Game1.IsServer)
        return;
      MultiplayerUtility.sendMessageToEveryone(2, who != null ? who.name : "null null", who != null ? who.uniqueMultiplayerID : 0L);
    }

    private bool onGrangeChange(Item i, int position, Item old, StorageContainer container, bool onRemoval)
    {
      if (!onRemoval)
      {
        if (i.Stack > 1 || i.Stack == 1 && old != null && (old.Stack == 1 && i.canStackWith(old)))
        {
          if (old != null && i != null && old.canStackWith(i))
          {
            container.ItemsToGrabMenu.actualInventory[position].Stack = 1;
            container.heldItem = old;
            return false;
          }
          if (old != null)
          {
            Utility.addItemToInventory(old, position, container.ItemsToGrabMenu.actualInventory, (ItemGrabMenu.behaviorOnItemSelect) null);
            container.heldItem = i;
            return false;
          }
          int num = i.Stack - 1;
          Item one = i.getOne();
          one.Stack = num;
          container.heldItem = one;
          i.Stack = 1;
        }
      }
      else if (old != null && old.Stack > 1 && !old.Equals((object) i))
        return false;
      if (Game1.IsMultiplayer)
      {
        if (onRemoval && old == null)
          MultiplayerUtility.sendMessageToEveryone(3, position.ToString() + " null null", Game1.player.uniqueMultiplayerID);
        else
          MultiplayerUtility.sendMessageToEveryone(3, position.ToString() + " " + (object) (i as Object).ParentSheetIndex + " " + (object) (i as Object).quality, Game1.player.uniqueMultiplayerID);
      }
      else
        this.addItemToGrangeDisplay(!onRemoval || old != null && !old.Equals((object) i) ? i : (Item) null, position, true);
      return true;
    }

    public bool canPlayerUseTool()
    {
      if (this.festivalData == null || !this.festivalData.ContainsKey("file") || (!this.festivalData["file"].Equals("winter8") || this.festivalTimer <= 0) || Game1.player.usingTool)
        return false;
      this.previousFacingDirection = Game1.player.FacingDirection;
      return true;
    }

    public bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
    {
      if (this.isFestival)
      {
        if (this.temporaryLocation != null && this.temporaryLocation.objects.ContainsKey(new Vector2((float) tileLocation.X, (float) tileLocation.Y)))
          this.temporaryLocation.objects[new Vector2((float) tileLocation.X, (float) tileLocation.Y)].checkForAction(who, false);
        int tileIndexAt = Game1.currentLocation.getTileIndexAt(tileLocation.X, tileLocation.Y, "Buildings");
        string str1 = Game1.currentLocation.doesTileHaveProperty(tileLocation.X, tileLocation.Y, "Action", "Buildings");
        bool flag = true;
        switch (tileIndexAt)
        {
          case 349:
          case 350:
          case 351:
            if (this.festivalData["file"].Equals("fall16"))
            {
              if (this.grangeDisplay == null)
              {
                this.grangeDisplay = new List<Item>();
                for (int index = 0; index < 9; ++index)
                  this.grangeDisplay.Add((Item) null);
              }
              if (this.playerUsingGrangeDisplay == null)
              {
                if (Game1.IsMultiplayer)
                {
                  if (Game1.IsServer)
                  {
                    this.setGrangeDisplayUser(who);
                    break;
                  }
                  break;
                }
                Game1.activeClickableMenu = (IClickableMenu) new StorageContainer(this.grangeDisplay.ToList<Item>(), 9, 3, new StorageContainer.behaviorOnItemChange(this.onGrangeChange), new InventoryMenu.highlightThisItem(Utility.highlightSmallObjects));
                this.waitingForMenuClose = true;
                break;
              }
              if (who.IsMainPlayer)
              {
                Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1675")));
                break;
              }
              break;
            }
            break;
          case 501:
          case 502:
            Response[] answerChoices1 = new Response[2]
            {
              new Response("Play", Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1662")),
              new Response("Leave", Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1663"))
            };
            if (who.IsMainPlayer && this.festivalData["file"].Equals("fall16"))
            {
              Game1.currentLocation.createQuestionDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1666"), answerChoices1, "slingshotGame");
              break;
            }
            break;
          case 503:
          case 504:
            Response[] answerChoices2 = new Response[2]
            {
              new Response("Play", Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1662")),
              new Response("Leave", Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1663"))
            };
            if (who.IsMainPlayer && this.festivalData["file"].Equals("fall16"))
            {
              Game1.currentLocation.createQuestionDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1681"), answerChoices2, "fishingGame");
              break;
            }
            break;
          case 505:
          case 506:
            if (who.IsMainPlayer && this.festivalData["file"].Equals("fall16"))
            {
              if (who.Money >= 100 && !who.mailReceived.Contains("fortuneTeller" + (object) Game1.year))
              {
                Response[] answerChoices3 = new Response[2]
                {
                  new Response("Read", Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1688")),
                  new Response("No", Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1690"))
                };
                Game1.currentLocation.createQuestionDialogue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1691")), answerChoices3, "fortuneTeller");
              }
              else if (who.mailReceived.Contains("fortuneTeller" + (object) Game1.year))
                Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1694")));
              else
                Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1695")));
              who.Halt();
              break;
            }
            break;
          case 510:
          case 511:
            if (who.IsMainPlayer && this.festivalData["file"].Equals("fall16"))
            {
              if (this.festivalShops == null)
                this.festivalShops = new Dictionary<string, Dictionary<Item, int[]>>();
              if (!this.festivalShops.ContainsKey("starTokenShop"))
              {
                Dictionary<Item, int[]> dictionary = new Dictionary<Item, int[]>();
                dictionary.Add((Item) new Furniture(1307, Vector2.Zero), new int[2]
                {
                  100,
                  1
                });
                dictionary.Add((Item) new Hat(19), new int[2]
                {
                  500,
                  1
                });
                dictionary.Add((Item) new Object(Vector2.Zero, 110, false), new int[2]
                {
                  800,
                  1
                });
                if (!Game1.player.mailReceived.Contains("CF_Fair"))
                  dictionary.Add((Item) new Object(434, 1, false, -1, 0), new int[2]
                  {
                    2000,
                    1
                  });
                this.festivalShops.Add("starTokenShop", dictionary);
              }
              Game1.currentLocation.createQuestionDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1672"), Game1.currentLocation.createYesNoResponses(), "starTokenShop");
              break;
            }
            break;
          case 540:
            if (who.IsMainPlayer && this.festivalData["file"].Equals("fall16"))
            {
              if (who.getTileX() == 29)
              {
                Game1.activeClickableMenu = (IClickableMenu) new StrengthGame();
                break;
              }
              Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1684")));
              break;
            }
            break;
          case 308:
          case 309:
            Response[] answerChoices4 = new Response[3]
            {
              new Response("Orange", Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1645")),
              new Response("Green", Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1647")),
              new Response("I", Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1650"))
            };
            if (who.IsMainPlayer && this.festivalData["file"].Equals("fall16"))
            {
              Game1.currentLocation.createQuestionDialogue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1652")), answerChoices4, "wheelBet");
              break;
            }
            break;
          case 175:
          case 176:
            if (who.IsMainPlayer && this.festivalData["file"].Equals("fall16"))
            {
              Game1.playerEatObject(new Object(241, 1, false, -1, 0), true);
              break;
            }
            break;
          case 87:
          case 88:
            Response[] answerChoices5 = new Response[2]
            {
              new Response("Buy", Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1654")),
              new Response("Leave", Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1656"))
            };
            if (who.IsMainPlayer && this.festivalData["file"].Equals("fall16"))
            {
              Game1.currentLocation.createQuestionDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1659"), answerChoices5, "StarTokenShop");
              break;
            }
            break;
          default:
            flag = false;
            break;
        }
        if (flag)
          return true;
        if (str1 != null)
        {
          try
          {
            string[] actionparams = str1.Split(' ');
            string str2 = actionparams[0];
            if (!(str2 == "Shop"))
            {
              if (!(str2 == "Message"))
              {
                if (!(str2 == "Dialogue"))
                {
                  if (str2 == "LuauSoup")
                  {
                    if (!this.specialEventVariable2)
                      Game1.activeClickableMenu = (IClickableMenu) new ItemGrabMenu((List<Item>) null, true, false, new InventoryMenu.highlightThisItem(Utility.highlightEdibleNonCookingItems), new ItemGrabMenu.behaviorOnItemSelect(this.clickToAddItemToLuauSoup), Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1719"), (ItemGrabMenu.behaviorOnItemSelect) null, false, true, true, true, false, 0, (Item) null, -1, (object) null);
                  }
                }
                else
                  Game1.drawObjectDialogue(Game1.currentLocation.actionParamsToString(actionparams).Replace("#", " "));
              }
              else
                Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromMaps:" + actionparams[1].Replace("\"", "")));
            }
            else
            {
              if (!who.IsMainPlayer)
                return false;
              if (this.festivalShops == null)
                this.festivalShops = new Dictionary<string, Dictionary<Item, int[]>>();
              Dictionary<Item, int[]> itemPriceAndStock;
              if (!this.festivalShops.ContainsKey(actionparams[1]))
              {
                string str3 = actionparams[1];
                string[] strArray = this.festivalData[actionparams[1]].Split(' ');
                itemPriceAndStock = new Dictionary<Item, int[]>();
                int maxValue = int.MaxValue;
                int index = 0;
                while (index < strArray.Length)
                {
                  string s = strArray[index];
                  string str4 = strArray[index + 1];
                  int int32_1 = Convert.ToInt32(strArray[index + 1]);
                  int int32_2 = Convert.ToInt32(strArray[index + 2]);
                  int int32_3 = Convert.ToInt32(strArray[index + 3]);
                  Item key = (Item) null;
                  // ISSUE: reference to a compiler-generated method
                  uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(s);
                  if (stringHash <= 2707948032U)
                  {
                    if (stringHash <= 1430892386U)
                    {
                      if (stringHash <= 568155902U)
                      {
                        if ((int) stringHash != 551378283)
                        {
                          if ((int) stringHash != 568155902 || !(s == "BO"))
                            goto label_100;
                        }
                        else if (s == "BL")
                          goto label_97;
                        else
                          goto label_100;
                      }
                      else if ((int) stringHash != 930363637)
                      {
                        if ((int) stringHash != 1430892386 || !(s == "Hat"))
                          goto label_100;
                        else
                          goto label_98;
                      }
                      else if (s == "Boot")
                        goto label_95;
                      else
                        goto label_100;
                    }
                    else if (stringHash <= 2089749334U)
                    {
                      if ((int) stringHash != 2005354379)
                      {
                        if ((int) stringHash != 2089749334 || !(s == "BigObject"))
                          goto label_100;
                      }
                      else if (s == "Ring")
                        goto label_94;
                      else
                        goto label_100;
                    }
                    else if ((int) stringHash != -1619739605)
                    {
                      if ((int) stringHash != -1587019264 || !(s == "Blueprint"))
                        goto label_100;
                      else
                        goto label_97;
                    }
                    else if (s == "BBL")
                      goto label_99;
                    else
                      goto label_100;
                    key = (Item) new Object(Vector2.Zero, int32_1, false);
                    goto label_100;
label_97:
                    key = (Item) new Object(int32_1, 1, true, -1, 0);
                    goto label_100;
                  }
                  else
                  {
                    if (stringHash <= 3389784126U)
                    {
                      if (stringHash <= 3212111499U)
                      {
                        if ((int) stringHash != -1212087455)
                        {
                          if ((int) stringHash != -1082855797 || !(s == "BBl"))
                            goto label_100;
                          else
                            goto label_99;
                        }
                        else if (s == "Weapon")
                          goto label_96;
                        else
                          goto label_100;
                      }
                      else if ((int) stringHash != -955516027)
                      {
                        if ((int) stringHash != -905183170 || !(s == "O"))
                          goto label_100;
                      }
                      else if (s == "B")
                        goto label_95;
                      else
                        goto label_100;
                    }
                    else if (stringHash <= 3524005078U)
                    {
                      if ((int) stringHash != -854850313)
                      {
                        if ((int) stringHash != -770962218 || !(s == "W"))
                          goto label_100;
                        else
                          goto label_96;
                      }
                      else if (s == "H")
                        goto label_98;
                      else
                        goto label_100;
                    }
                    else if ((int) stringHash != -715693292)
                    {
                      if ((int) stringHash != -687074123)
                      {
                        if ((int) stringHash != -443652902 || !(s == "Object"))
                          goto label_100;
                      }
                      else if (s == "R")
                        goto label_94;
                      else
                        goto label_100;
                    }
                    else if (s == "BigBlueprint")
                      goto label_99;
                    else
                      goto label_100;
                    key = (Item) new Object(int32_1, 1, false, -1, 0);
                    goto label_100;
label_96:
                    key = (Item) new MeleeWeapon(int32_1);
                    goto label_100;
                  }
label_94:
                  key = (Item) new Ring(int32_1);
                  goto label_100;
label_95:
                  key = (Item) new Boots(int32_1);
                  goto label_100;
label_98:
                  key = (Item) new Hat(int32_1);
                  goto label_100;
label_99:
                  key = (Item) new Object(Vector2.Zero, int32_1, true);
label_100:
                  if ((!(key is Object) || !(key as Object).isRecipe || !who.knowsRecipe(key.Name)) && key != null)
                    itemPriceAndStock.Add(key, new int[2]
                    {
                      int32_2,
                      int32_3 <= 0 ? maxValue : int32_3
                    });
                  index += 4;
                }
                this.festivalShops.Add(actionparams[1], itemPriceAndStock);
              }
              else
                itemPriceAndStock = this.festivalShops[actionparams[1]];
              if (itemPriceAndStock != null && itemPriceAndStock.Count > 0)
                Game1.activeClickableMenu = (IClickableMenu) new ShopMenu(itemPriceAndStock, 0, (string) null);
              else
                Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1714"));
            }
          }
          catch (Exception ex)
          {
          }
        }
        else if (this.isFestival)
        {
          if (who.IsMainPlayer)
          {
            foreach (NPC actor in this.actors)
            {
              if (actor.getTileX() == tileLocation.X && actor.getTileY() == tileLocation.Y && (actor.CurrentDialogue.Count<Dialogue>() >= 1 || actor.CurrentDialogue.Count<Dialogue>() > 0 && !actor.CurrentDialogue.Peek().isOnFinalDialogue() || (actor.Equals((object) this.festivalHost) || actor.datable && this.festivalData["file"].Equals("spring24")) || (object) this.secretSantaRecipient != null && actor.name.Equals(this.secretSantaRecipient.name)))
              {
                if ((this.grangeScore > -100 || this.grangeScore == -666) && actor.Equals((object) this.festivalHost))
                {
                  string s;
                  if (this.grangeScore >= 90)
                  {
                    Game1.playSound("reward");
                    s = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1723", (object) this.grangeScore);
                    Game1.player.festivalScore += 1000;
                  }
                  else if (this.grangeScore >= 75)
                  {
                    Game1.playSound("reward");
                    s = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1726", (object) this.grangeScore);
                    Game1.player.festivalScore += 500;
                  }
                  else if (this.grangeScore >= 60)
                  {
                    Game1.playSound("newArtifact");
                    s = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1729", (object) this.grangeScore);
                    Game1.player.festivalScore += 250;
                  }
                  else if (this.grangeScore == -666)
                  {
                    Game1.playSound("secret1");
                    s = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1730");
                    Game1.player.festivalScore += 750;
                  }
                  else
                  {
                    Game1.playSound("newArtifact");
                    s = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1732", (object) this.grangeScore);
                    Game1.player.festivalScore += 50;
                  }
                  this.grangeScore = -100;
                  actor.setNewDialogue(s, false, false);
                }
                else if ((Game1.serverHost == null || Game1.player.Equals((object) Game1.serverHost)) && actor.Equals((object) this.festivalHost) && ((actor.CurrentDialogue.Count<Dialogue>() == 0 || actor.CurrentDialogue.Peek().isOnFinalDialogue()) && this.hostMessage != null))
                  actor.setNewDialogue(this.hostMessage, false, false);
                else if ((Game1.serverHost == null || Game1.player.Equals((object) Game1.serverHost)) && actor.Equals((object) this.festivalHost) && ((actor.CurrentDialogue.Count == 0 || actor.CurrentDialogue.Peek().isOnFinalDialogue()) && this.hostMessage != null))
                  actor.setNewDialogue(this.hostMessage, false, false);
                if (this.festivalData != null && this.festivalData["file"].Equals("spring24") && (actor.datable || who.spouse != null && actor.name.Equals(who.spouse)))
                {
                  if ((object) who.dancePartner == null)
                  {
                    if (actor.CurrentDialogue.Count > 0 && actor.CurrentDialogue.Peek().getCurrentDialogue().Equals("..."))
                      actor.CurrentDialogue.Clear();
                    if (actor.CurrentDialogue.Count == 0)
                    {
                      actor.CurrentDialogue.Push(new Dialogue("...", actor));
                      if (actor.name.Equals(who.spouse))
                        actor.setNewDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1736", (object) actor.displayName), true, false);
                      else
                        actor.setNewDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1738", (object) actor.displayName), true, false);
                    }
                    else if (actor.CurrentDialogue.Peek().isOnFinalDialogue())
                    {
                      Dialogue dialogue = actor.CurrentDialogue.Peek();
                      Game1.drawDialogue(actor);
                      actor.faceTowardFarmerForPeriod(3000, 2, false, who);
                      who.Halt();
                      actor.CurrentDialogue = new Stack<Dialogue>();
                      actor.CurrentDialogue.Push(new Dialogue("...", actor));
                      actor.CurrentDialogue.Push(dialogue);
                      return true;
                    }
                  }
                  else if (actor.CurrentDialogue.Count > 0 && actor.CurrentDialogue.Peek().getCurrentDialogue().Equals("..."))
                    actor.CurrentDialogue.Clear();
                }
                if ((object) this.secretSantaRecipient != null && actor.name.Equals(this.secretSantaRecipient.name))
                {
                  GameLocation currentLocation = Game1.currentLocation;
                  string text1;
                  if (this.secretSantaRecipient.gender != 0)
                    text1 = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1741", (object) this.secretSantaRecipient.displayName);
                  else
                    text1 = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1740", (object) this.secretSantaRecipient.displayName);
                  string text2 = Game1.parseText(text1);
                  Response[] yesNoResponses = Game1.currentLocation.createYesNoResponses();
                  string dialogKey = "secretSanta";
                  currentLocation.createQuestionDialogue(text2, yesNoResponses, dialogKey);
                  who.Halt();
                  return true;
                }
                if (actor.CurrentDialogue.Count == 0)
                  return true;
                if (who.spouse != null && actor.name.Equals(who.spouse) && (!this.festivalData["file"].Equals("spring24") && this.festivalData.ContainsKey(actor.name + "_spouse")))
                {
                  actor.CurrentDialogue.Clear();
                  actor.CurrentDialogue.Push(new Dialogue(this.festivalData[actor.name + "_spouse"], actor));
                }
                Game1.drawDialogue(actor);
                actor.faceTowardFarmerForPeriod(3000, 2, false, who);
                who.Halt();
                return true;
              }
            }
          }
          if (this.festivalData != null && this.festivalData["file"].Equals("spring13"))
          {
            Microsoft.Xna.Framework.Rectangle r = new Microsoft.Xna.Framework.Rectangle(tileLocation.X * Game1.tileSize, tileLocation.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
            for (int index = this.festivalProps.Count - 1; index >= 0; --index)
            {
              if (this.festivalProps[index].isColliding(r))
              {
                ++who.festivalScore;
                this.festivalProps.RemoveAt(index);
                if (who.IsMainPlayer)
                  Game1.playSound("coin");
                return false;
              }
            }
          }
        }
      }
      return false;
    }

    public void checkForSpecialCharacterIconAtThisTile(Vector2 tileLocation)
    {
      if (!this.isFestival || this.festivalHost == null || !this.festivalHost.getTileLocation().Equals(tileLocation))
        return;
      Game1.mouseCursor = 4;
    }

    public void forceEndFestival(Farmer who)
    {
      Game1.currentMinigame = (IMinigame) null;
      Game1.exitActiveMenu();
      this.endBehaviors((string[]) null, Game1.currentLocation);
      if (Game1.IsServer)
        MultiplayerUtility.sendServerToClientsMessage("endFest");
      Game1.changeMusicTrack("none");
    }

    public bool checkForCollision(Microsoft.Xna.Framework.Rectangle position, Farmer who)
    {
      foreach (NPC actor in this.actors)
      {
        if (actor.GetBoundingBox().Intersects(position) && !Game1.player.temporarilyInvincible && (Game1.player.temporaryImpassableTile.Equals(Microsoft.Xna.Framework.Rectangle.Empty) && !actor.isInvisible))
          return true;
      }
      if (position.X < 0 || position.Y < 0 || (position.X >= Game1.currentLocation.map.Layers[0].DisplayWidth || position.Y >= Game1.currentLocation.map.Layers[0].DisplayHeight))
      {
        if (who.IsMainPlayer && this.isFestival && (Game1.IsServer || !Game1.IsMultiplayer) && Game1.activeClickableMenu == null)
        {
          who.Halt();
          who.position = who.lastPosition;
          Game1.activeClickableMenu = (IClickableMenu) new ConfirmationDialog(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1758", (object) this.FestivalName), new ConfirmationDialog.behavior(this.forceEndFestival), (ConfirmationDialog.behavior) null);
        }
        return true;
      }
      foreach (Object prop in this.props)
      {
        Vector2 tileLocation = prop.tileLocation;
        if (prop.getBoundingBox(tileLocation).Intersects(position))
          return true;
      }
      if (this.temporaryLocation != null)
      {
        foreach (Object @object in this.temporaryLocation.objects.Values)
        {
          Vector2 tileLocation = @object.tileLocation;
          if (@object.getBoundingBox(tileLocation).Intersects(position))
            return true;
        }
      }
      foreach (Prop festivalProp in this.festivalProps)
      {
        if (festivalProp.isColliding(position))
          return true;
      }
      return false;
    }

    public void answerDialogue(string questionKey, int answerChoice)
    {
      this.previousAnswerChoice = answerChoice;
      if (questionKey.Contains("fork"))
      {
        int int32 = Convert.ToInt32(questionKey.Replace("fork", ""));
        if (answerChoice != int32)
          return;
        this.specialEventVariable1 = !this.specialEventVariable1;
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(questionKey);
        if (stringHash <= 1836559258U)
        {
          if (stringHash <= 390240131U)
          {
            if ((int) stringHash != 119764934)
            {
              if ((int) stringHash != 269688027)
              {
                if ((int) stringHash != 390240131 || !(questionKey == "shaneCliffs"))
                  return;
                switch (answerChoice)
                {
                  case 0:
                    this.eventCommands[this.currentCommand + 2] = "speak Shane \"" + Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1760") + "\"";
                    break;
                  case 1:
                    this.eventCommands[this.currentCommand + 2] = "speak Shane \"" + Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1761") + "\"";
                    break;
                  case 2:
                    this.eventCommands[this.currentCommand + 2] = "speak Shane \"" + Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1763") + "\"";
                    break;
                  case 3:
                    this.eventCommands[this.currentCommand + 2] = "speak Shane \"" + Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1764") + "\"";
                    break;
                }
              }
              else
              {
                if (!(questionKey == "wheelBet"))
                  return;
                this.specialEventVariable2 = answerChoice == 1;
                if (answerChoice == 2)
                  return;
                Game1.activeClickableMenu = (IClickableMenu) new NumberSelectionMenu(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1776"), new NumberSelectionMenu.behaviorOnNumberSelect(this.betStarTokens), -1, 1, Game1.player.festivalScore, 1);
              }
            }
            else
            {
              if (!(questionKey == "shaneLoan"))
                return;
              if (answerChoice == 0)
              {
                this.specialEventVariable1 = true;
                this.eventCommands[this.currentCommand + 1] = "fork giveShaneLoan";
                Game1.player.money -= 3000;
              }
            }
          }
          else if (stringHash <= 504494762U)
          {
            if ((int) stringHash != 472382138)
            {
              if ((int) stringHash != 504494762 || !(questionKey == "starTokenShop") || answerChoice != 0)
                return;
              if (this.festivalShops["starTokenShop"].Count == 0)
                Game1.drawObjectDialogue(Game1.parseText(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1785")));
              else
                Game1.activeClickableMenu = (IClickableMenu) new ShopMenu(this.festivalShops["starTokenShop"], 1, (string) null);
            }
            else
            {
              if (!(questionKey == "fortuneTeller") || answerChoice != 0)
                return;
              Game1.globalFadeToBlack(new Game1.afterFadeFunction(this.readFortune), 0.02f);
              Game1.player.Money -= 100;
              Game1.player.mailReceived.Add("fortuneTeller" + (object) Game1.year);
            }
          }
          else if ((int) stringHash != 1766558334)
          {
            if ((int) stringHash != 1836559258 || !(questionKey == "secretSanta") || answerChoice != 0)
              return;
            Game1.activeClickableMenu = (IClickableMenu) new ItemGrabMenu((List<Item>) null, true, false, new InventoryMenu.highlightThisItem(Utility.highlightSmallObjects), new ItemGrabMenu.behaviorOnItemSelect(this.chooseSecretSantaGift), Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1788", (object) this.secretSantaRecipient.displayName), (ItemGrabMenu.behaviorOnItemSelect) null, false, false, true, true, false, 0, (Item) null, -1, (object) null);
          }
          else
          {
            if (!(questionKey == "pet"))
              return;
            if (answerChoice == 0)
            {
              Game1.activeClickableMenu = (IClickableMenu) new NamingMenu(new NamingMenu.doneNamingBehavior(this.namePet), Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1236"), Game1.player.isMale ? (Game1.player.catPerson ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1794") : Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1795")) : (Game1.player.catPerson ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1796") : Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1797")));
            }
            else
            {
              Game1.player.mailReceived.Add("rejectedPet");
              this.eventCommands = new string[2];
              this.eventCommands[1] = "end";
              this.eventCommands[0] = "speak Marnie \"" + Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1798") + "\"";
              this.currentCommand = 0;
              this.eventSwitched = true;
              this.specialEventVariable1 = true;
            }
          }
        }
        else if (stringHash <= 2337399242U)
        {
          if ((int) stringHash != -2089303069)
          {
            if ((int) stringHash != -2045149249)
            {
              if ((int) stringHash != -1957568054 || !(questionKey == "StarTokenShop") || answerChoice != 0)
                return;
              Game1.activeClickableMenu = (IClickableMenu) new NumberSelectionMenu(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1774"), new NumberSelectionMenu.behaviorOnNumberSelect(this.buyStarTokens), 50, 0, 999, 0);
            }
            else
            {
              if (!(questionKey == "chooseCharacter"))
                return;
              switch (answerChoice)
              {
                case 0:
                  this.specialEventVariable1 = true;
                  this.eventCommands[this.currentCommand + 1] = "fork warrior";
                  break;
                case 1:
                  this.specialEventVariable1 = true;
                  this.eventCommands[this.currentCommand + 1] = "fork healer";
                  break;
              }
            }
          }
          else
          {
            if (!(questionKey == "haleyDarkRoom"))
              return;
            switch (answerChoice)
            {
              case 0:
                this.specialEventVariable1 = true;
                this.eventCommands[this.currentCommand + 1] = "fork decorate";
                break;
              case 1:
                this.specialEventVariable1 = true;
                this.eventCommands[this.currentCommand + 1] = "fork leave";
                break;
            }
          }
        }
        else if (stringHash <= 2900380439U)
        {
          if ((int) stringHash != -1758331304)
          {
            if ((int) stringHash != -1394586857 || !(questionKey == "fishingGame"))
              return;
            if (answerChoice == 0 && Game1.player.Money >= 50)
            {
              Game1.globalFadeToBlack(new Game1.afterFadeFunction(FishingGame.startMe), 0.01f);
              Game1.player.Money -= 50;
            }
            else
            {
              if (answerChoice != 0 || Game1.player.Money >= 50)
                return;
              Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1780"));
            }
          }
          else
          {
            if (!(questionKey == "cave"))
              return;
            if (answerChoice == 0)
            {
              Game1.player.caveChoice = 2;
              (Game1.getLocationFromName("FarmCave") as FarmCave).setUpMushroomHouse();
            }
            else
              Game1.player.caveChoice = 1;
          }
        }
        else if ((int) stringHash != -1287780222)
        {
          if ((int) stringHash != -746818044 || !(questionKey == "slingshotGame"))
            return;
          if (answerChoice == 0 && Game1.player.Money >= 50)
          {
            Game1.globalFadeToBlack(new Game1.afterFadeFunction(TargetGame.startMe), 0.01f);
            Game1.player.Money -= 50;
          }
          else
          {
            if (answerChoice != 0 || Game1.player.Money >= 50)
              return;
            Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1780"));
          }
        }
        else
        {
          if (!(questionKey == "bandFork"))
            return;
          switch (answerChoice)
          {
            case 76:
              this.specialEventVariable1 = true;
              this.eventCommands[this.currentCommand + 1] = "fork poppy";
              break;
            case 77:
              this.specialEventVariable1 = true;
              this.eventCommands[this.currentCommand + 1] = "fork heavy";
              break;
            case 78:
              this.specialEventVariable1 = true;
              this.eventCommands[this.currentCommand + 1] = "fork techno";
              break;
            case 79:
              this.specialEventVariable1 = true;
              this.eventCommands[this.currentCommand + 1] = "fork honkytonk";
              break;
          }
        }
      }
    }

    private void namePet(string name)
    {
      Pet pet = !Game1.player.catPerson ? (Pet) new Dog(68, 13) : (Pet) new Cat(68, 13);
      pet.warpToFarmHouse(Game1.player);
      pet.name = name;
      pet.displayName = pet.name;
      Game1.exitActiveMenu();
      this.CurrentCommand = this.CurrentCommand + 1;
    }

    public void chooseSecretSantaGift(Item i, Farmer who)
    {
      if (i == null)
        return;
      if (i is Object)
      {
        Game1.exitActiveMenu();
        NPC actorByName = this.getActorByName(this.secretSantaRecipient.name);
        actorByName.faceTowardFarmerForPeriod(15000, 5, false, who);
        actorByName.receiveGift(i as Object, who, false, 5f, false);
        actorByName.CurrentDialogue.Clear();
        actorByName.CurrentDialogue.Push(new Dialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1801", (object) i.DisplayName), actorByName));
        Game1.drawDialogue(actorByName);
        this.secretSantaRecipient = (NPC) null;
        this.startSecretSantaAfterDialogue = true;
        who.Halt();
        who.completelyStopAnimatingOrDoingAction();
        who.faceGeneralDirection(actorByName.position, 0);
      }
      else
        Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1803"));
    }

    public void perfectFishing()
    {
      if (!this.isFestival || Game1.currentMinigame == null || !this.festivalData["file"].Equals("fall16"))
        return;
      ++(Game1.currentMinigame as FishingGame).perfections;
    }

    public void caughtFish(int whichFish, int size, Farmer who)
    {
      if (!this.isFestival)
        return;
      if (whichFish != -1 && Game1.currentMinigame != null && this.festivalData["file"].Equals("fall16"))
      {
        (Game1.currentMinigame as FishingGame).score += size > 0 ? size + 5 : 1;
        if (size > 0)
          ++(Game1.currentMinigame as FishingGame).fishCaught;
        Game1.player.FarmerSprite.pauseForSingleAnimation = false;
        Game1.player.FarmerSprite.StopAnimation();
      }
      else
      {
        if (whichFish == -1 || !this.festivalData["file"].Equals("winter8"))
          return;
        if (size > 0 && who.getTileX() < 79 && who.getTileY() < 43)
        {
          ++who.festivalScore;
          Game1.playSound("newArtifact");
        }
        who.forceCanMove();
        if (this.previousFacingDirection == -1)
          return;
        who.faceDirection(this.previousFacingDirection);
      }
    }

    public void readFortune()
    {
      Game1.globalFade = true;
      Game1.fadeToBlackAlpha = 1f;
      NPC romanticInterest1 = Utility.getTopRomanticInterest(Game1.player);
      NPC romanticInterest2 = Utility.getTopNonRomanticInterest(Game1.player);
      int highestSkill = Utility.getHighestSkill(Game1.player);
      string[] messages = new string[5];
      if (romanticInterest2 != null && Game1.player.getFriendshipLevelForNPC(romanticInterest2.name) > 100)
      {
        if (Utility.getNumberOfFriendsWithinThisRange(Game1.player, Game1.player.getFriendshipLevelForNPC(romanticInterest2.name) - 100, Game1.player.getFriendshipLevelForNPC(romanticInterest2.name), false) > 3 && Game1.random.NextDouble() < 0.5)
        {
          messages[0] = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1810");
        }
        else
        {
          switch (Game1.random.Next(4))
          {
            case 0:
              messages[0] = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1811", (object) romanticInterest2.displayName);
              break;
            case 1:
              messages[0] = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1813", (object) romanticInterest2.displayName) + (romanticInterest2.gender == 0 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1815") : Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1816"));
              break;
            case 2:
              messages[0] = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1818", (object) romanticInterest2.displayName);
              break;
            case 3:
              messages[0] = (romanticInterest2.gender == 0 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1820") : Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1821")) + Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1823", (object) romanticInterest2.displayName);
              break;
          }
        }
      }
      else
        messages[0] = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1825");
      if (romanticInterest1 != null && Game1.player.getFriendshipLevelForNPC(romanticInterest1.name) > 250)
      {
        if (Utility.getNumberOfFriendsWithinThisRange(Game1.player, Game1.player.getFriendshipLevelForNPC(romanticInterest1.name) - 100, Game1.player.getFriendshipLevelForNPC(romanticInterest1.name), true) > 2)
        {
          messages[1] = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1826");
        }
        else
        {
          switch (Game1.random.Next(4))
          {
            case 0:
              messages[1] = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1827", (object) romanticInterest1.displayName);
              break;
            case 1:
              messages[1] = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1827", (object) romanticInterest1.displayName);
              break;
            case 2:
              string[] strArray = messages;
              int index = 1;
              string str1 = romanticInterest1.gender == 0 ? (romanticInterest1.socialAnxiety == 1 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1831") : Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1832")) : (romanticInterest1.socialAnxiety == 1 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1833") : Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1834"));
              string str2 = " ";
              string str3;
              if (romanticInterest1.gender != 0)
                str3 = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1838", (object) romanticInterest1.displayName[0]);
              else
                str3 = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1837", (object) romanticInterest1.displayName[0]);
              string str4 = str1 + str2 + str3;
              strArray[index] = str4;
              break;
            case 3:
              messages[1] = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1843", (object) romanticInterest1.displayName);
              break;
          }
        }
      }
      else
        messages[1] = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1845");
      switch (highestSkill)
      {
        case 0:
          messages[2] = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1846");
          break;
        case 1:
          messages[2] = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1849");
          break;
        case 2:
          messages[2] = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1850");
          break;
        case 3:
          messages[2] = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1847");
          break;
        case 4:
          messages[2] = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1848");
          break;
        case 5:
          messages[2] = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1851");
          break;
      }
      messages[3] = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1852");
      messages[4] = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1853");
      Game1.multipleDialogues(messages);
      Game1.afterDialogues = new Game1.afterFadeFunction(this.fadeClearAndviewportUnfreeze);
      Game1.viewportFreeze = true;
      Game1.viewport.X = -9999;
    }

    public void fadeClearAndviewportUnfreeze()
    {
      Game1.fadeClear();
      Game1.viewportFreeze = false;
    }

    public void betStarTokens(int value, int price, Farmer who)
    {
      if (value > who.festivalScore)
        return;
      Game1.playSound("smallSelect");
      Game1.activeClickableMenu = (IClickableMenu) new WheelSpinGame(value);
    }

    public void buyStarTokens(int value, int price, Farmer who)
    {
      if (value <= 0 || value * price > who.Money)
        return;
      who.Money -= price * value;
      who.festivalScore += value;
      Game1.playSound("purchase");
      Game1.exitActiveMenu();
    }

    public void clickToAddItemToLuauSoup(Item i, Farmer who)
    {
      if (!Game1.IsMultiplayer || Game1.IsServer)
        this.addItemToLuauSoup(i, who);
      if (!Game1.IsMultiplayer)
        return;
      MultiplayerUtility.sendMessageToEveryone(1, (i as Object).parentSheetIndex.ToString() + " " + (object) (i as Object).quality, who.uniqueMultiplayerID);
    }

    public void setUpAdvancedMove(string[] split, NPCController.endBehavior endBehavior = null)
    {
      if (this.npcControllers == null)
        this.npcControllers = new List<NPCController>();
      List<Vector2> path = new List<Vector2>();
      int index = 3;
      while (index < split.Length)
      {
        path.Add(new Vector2((float) Convert.ToInt32(split[index]), (float) Convert.ToInt32(split[index + 1])));
        index += 2;
      }
      NPC actorByName = this.getActorByName(split[1].Replace('_', ' '));
      if (actorByName == null)
        return;
      this.npcControllers.Add(new NPCController(actorByName, path, Convert.ToBoolean(split[2]), endBehavior));
    }

    public void addItemToLuauSoup(Item i, Farmer who)
    {
      if (i == null)
        return;
      if (this.luauIngredients == null)
        this.luauIngredients = new List<Item>();
      this.luauIngredients.Add(i);
      if (who.IsMainPlayer)
      {
        this.specialEventVariable2 = true;
        if (i != null && i.Stack > 1)
        {
          --i.Stack;
          who.addItemToInventory(i);
        }
        Game1.exitActiveMenu();
        Game1.playSound("dropItemInWater");
        if (i == null)
          return;
        Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1857", (object) i.DisplayName));
      }
      else
        Game1.ChatBox.receiveChatMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1859", (object) who.displayName, (object) i.DisplayName), -1L);
    }

    private void governorTaste()
    {
      int num1 = 5;
      if (this.luauIngredients != null)
      {
        foreach (Item luauIngredient in this.luauIngredients)
        {
          Object @object = luauIngredient as Object;
          int num2 = 5;
          if (@object.quality >= 2 && @object.price >= 160 || @object.quality == 1 && @object.price >= 300 && @object.edibility > 10)
          {
            num2 = 4;
            Utility.improveFriendshipWithEveryoneInRegion(Game1.player, 120, 2);
          }
          else if (@object.edibility >= 20 || @object.price >= 100 || @object.price >= 70 && @object.quality >= 1)
          {
            num2 = 3;
            Utility.improveFriendshipWithEveryoneInRegion(Game1.player, 60, 2);
          }
          else if (@object.price > 20 && @object.edibility >= 10 || @object.price >= 40 && @object.edibility >= 5)
            num2 = 2;
          else if (@object.edibility >= 0)
          {
            num2 = 1;
            Utility.improveFriendshipWithEveryoneInRegion(Game1.player, -50, 2);
          }
          if (@object.edibility > -300 && @object.edibility < 0)
          {
            num2 = 0;
            Utility.improveFriendshipWithEveryoneInRegion(Game1.player, -100, 2);
          }
          if (num2 < num1)
            num1 = num2;
        }
        if (this.luauIngredients.Count < Game1.numberOfPlayers())
          num1 = 5;
      }
      this.eventCommands[this.CurrentCommand + 1] = "switchEvent governorReaction" + (object) num1;
    }

    private void eggHuntWinner()
    {
      int num = 12;
      switch (Game1.numberOfPlayers())
      {
        case 1:
          num = 9;
          break;
        case 2:
          num = 6;
          break;
        case 3:
          num = 5;
          break;
        case 4:
          num = 4;
          break;
      }
      List<Farmer> source = new List<Farmer>();
      Farmer player = Game1.player;
      int festivalScore = Game1.player.festivalScore;
      for (int number = 1; number <= Game1.numberOfPlayers(); ++number)
      {
        Farmer fromFarmerNumber = Utility.getFarmerFromFarmerNumber(number);
        if (fromFarmerNumber != null && fromFarmerNumber.festivalScore > festivalScore)
          festivalScore = fromFarmerNumber.festivalScore;
      }
      for (int number = 1; number <= Game1.numberOfPlayers(); ++number)
      {
        Farmer fromFarmerNumber = Utility.getFarmerFromFarmerNumber(number);
        if (fromFarmerNumber != null && fromFarmerNumber.festivalScore == festivalScore)
        {
          source.Add(fromFarmerNumber);
          fromFarmerNumber.festivalScore = -9999;
        }
      }
      string masterDialogue = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1862");
      if (festivalScore >= num)
      {
        if (source.Count == 1)
        {
          masterDialogue = LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.es ? "¡" + source[0].displayName + "!" : source[0].displayName + "!";
        }
        else
        {
          string str = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1864");
          for (int index = 0; index < source.Count; ++index)
          {
            if (index == source.Count<Farmer>() - 1)
              str += Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1865");
            str = str + " " + source[index].displayName;
            if (index < source.Count - 1)
              str += ",";
          }
          masterDialogue = str + "!";
        }
        this.specialEventVariable1 = false;
      }
      else
        this.specialEventVariable1 = true;
      this.getActorByName("Lewis").CurrentDialogue.Push(new Dialogue(masterDialogue, this.getActorByName("Lewis")));
      Game1.drawDialogue(this.getActorByName("Lewis"));
    }

    private void iceFishingWinner()
    {
      int num = 5;
      List<Farmer> source = new List<Farmer>();
      Farmer player = Game1.player;
      int festivalScore = Game1.player.festivalScore;
      for (int number = 1; number <= Game1.numberOfPlayers(); ++number)
      {
        Farmer fromFarmerNumber = Utility.getFarmerFromFarmerNumber(number);
        if (fromFarmerNumber != null && fromFarmerNumber.festivalScore > festivalScore)
          festivalScore = fromFarmerNumber.festivalScore;
      }
      for (int number = 1; number <= Game1.numberOfPlayers(); ++number)
      {
        Farmer fromFarmerNumber = Utility.getFarmerFromFarmerNumber(number);
        if (fromFarmerNumber != null && fromFarmerNumber.festivalScore == festivalScore)
          source.Add(fromFarmerNumber);
      }
      string masterDialogue = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1871");
      if (festivalScore >= num)
      {
        if (source.Count == 1)
        {
          masterDialogue = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1872", (object) source[0].displayName, (object) source[0].festivalScore);
          source[0].festivalScore = -9999;
        }
        else
        {
          string str = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1864");
          for (int index = 0; index < source.Count; ++index)
          {
            if (index == source.Count<Farmer>() - 1)
              str += Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1865");
            str = str + " " + source[index].displayName;
            if (index < source.Count - 1)
              str += ",";
          }
          masterDialogue = str + "!";
        }
        this.specialEventVariable1 = false;
      }
      else
        this.specialEventVariable1 = true;
      this.getActorByName("Lewis").CurrentDialogue.Push(new Dialogue(masterDialogue, this.getActorByName("Lewis")));
      Game1.drawDialogue(this.getActorByName("Lewis"));
    }
  }
}
