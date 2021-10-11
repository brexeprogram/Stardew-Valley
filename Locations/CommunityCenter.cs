// Decompiled with JetBrains decompiler
// Type: StardewValley.Locations.CommunityCenter
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Characters;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using xTile;
using xTile.Dimensions;
using xTile.Tiles;

namespace StardewValley.Locations
{
  public class CommunityCenter : GameLocation
  {
    public bool[] areasComplete = new bool[6];
    public const int AREA_Pantry = 0;
    public const int AREA_FishTank = 2;
    public const int AREA_CraftsRoom = 1;
    public const int AREA_BoilerRoom = 3;
    public const int AREA_Vault = 4;
    public const int AREA_Bulletin = 5;
    public const int AREA_Bulletin2 = 6;
    public const int AREA_JunimoHut = 7;
    private bool refurbishedLoaded;
    private bool warehouse;
    public SerializableDictionary<int, bool[]> bundles;
    public SerializableDictionary<int, bool> bundleRewards;
    public int numberOfStarsOnPlaque;
    private float messageAlpha;
    private List<int> junimoNotesViewportTargets;
    private Dictionary<int, List<int>> areaToBundleDictionary;
    private Dictionary<int, int> bundleToAreaDictionary;
    public const int PHASE_firstPause = 0;
    public const int PHASE_junimoAppear = 1;
    public const int PHASE_junimoDance = 2;
    public const int PHASE_restore = 3;
    private int restoreAreaTimer;
    private int restoreAreaPhase;
    private int restoreAreaIndex;
    private Cue buildUpSound;

    public CommunityCenter()
    {
    }

    public CommunityCenter(string name)
      : base(Game1.game1.xTileContent.Load<Map>("Maps\\CommunityCenter_Ruins"), name)
    {
      Dictionary<string, string> dictionary = Game1.content.Load<Dictionary<string, string>>("Data\\Bundles");
      this.bundles = new SerializableDictionary<int, bool[]>();
      this.bundleRewards = new SerializableDictionary<int, bool>();
      this.areaToBundleDictionary = new Dictionary<int, List<int>>();
      this.bundleToAreaDictionary = new Dictionary<int, int>();
      for (int key = 0; key < 6; ++key)
        this.areaToBundleDictionary.Add(key, new List<int>());
      foreach (KeyValuePair<string, string> keyValuePair in dictionary)
      {
        this.bundles.Add(Convert.ToInt32(keyValuePair.Key.Split('/')[1]), new bool[keyValuePair.Value.Split('/')[2].Split(' ').Length]);
        this.bundleRewards.Add(Convert.ToInt32(keyValuePair.Key.Split('/')[1]), false);
        this.areaToBundleDictionary[this.getAreaNumberFromName(keyValuePair.Key.Split('/')[0])].Add(Convert.ToInt32(keyValuePair.Key.Split('/')[1]));
        this.bundleToAreaDictionary.Add(Convert.ToInt32(keyValuePair.Key.Split('/')[1]), this.getAreaNumberFromName(keyValuePair.Key.Split('/')[0]));
      }
    }

    private int getAreaNumberFromName(string name)
    {
      // ISSUE: reference to a compiler-generated method
      uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(name);
      if (stringHash <= 2486580683U)
      {
        if (stringHash <= 696049845U)
        {
          if ((int) stringHash != 244995591)
          {
            if ((int) stringHash == 696049845 && name == "Pantry")
              return 0;
            goto label_27;
          }
          else if (name == "FishTank")
            goto label_23;
          else
            goto label_27;
        }
        else if ((int) stringHash != 1618314778)
        {
          if ((int) stringHash != 1881810045)
          {
            if ((int) stringHash != -1808386613 || !(name == "BoilerRoom"))
              goto label_27;
            else
              goto label_24;
          }
          else if (!(name == "CraftsRoom"))
            goto label_27;
        }
        else if (name == "Bulletin")
          goto label_26;
        else
          goto label_27;
      }
      else if (stringHash <= 3168044721U)
      {
        if ((int) stringHash != -1717972835)
        {
          if ((int) stringHash != -1231095930)
          {
            if ((int) stringHash != -1126922575 || !(name == "Crafts Room"))
              goto label_27;
          }
          else if (name == "Bulletin Board")
            goto label_26;
          else
            goto label_27;
        }
        else if (name == "Fish Tank")
          goto label_23;
        else
          goto label_27;
      }
      else if ((int) stringHash != -1124258565)
      {
        if ((int) stringHash != -734883505)
        {
          if ((int) stringHash != -190500582 || !(name == "BulletinBoard"))
            goto label_27;
          else
            goto label_26;
        }
        else
        {
          if (name == "Vault")
            return 4;
          goto label_27;
        }
      }
      else if (name == "Boiler Room")
        goto label_24;
      else
        goto label_27;
      return 1;
label_23:
      return 2;
label_24:
      return 3;
label_26:
      return 5;
label_27:
      return -1;
    }

    private Point getNotePosition(int area)
    {
      switch (area)
      {
        case 0:
          return new Point(14, 5);
        case 1:
          return new Point(14, 23);
        case 2:
          return new Point(40, 10);
        case 3:
          return new Point(63, 14);
        case 4:
          return new Point(55, 6);
        case 5:
          return new Point(46, 11);
        default:
          return Point.Zero;
      }
    }

    public void addJunimoNote(int area)
    {
      Point notePosition = this.getNotePosition(area);
      if (notePosition.Equals((object) Vector2.Zero))
        return;
      StaticTile[] junimoNoteTileFrames = this.getJunimoNoteTileFrames(area);
      string layerId = area == 5 ? "Front" : "Buildings";
      this.map.GetLayer(layerId).Tiles[notePosition.X, notePosition.Y] = (Tile) new AnimatedTile(this.map.GetLayer(layerId), junimoNoteTileFrames, 70L);
      Game1.currentLightSources.Add(new LightSource(4, new Vector2((float) (notePosition.X * Game1.tileSize), (float) (notePosition.Y * Game1.tileSize)), 1f));
      List<TemporaryAnimatedSprite> temporarySprites1 = this.temporarySprites;
      TemporaryAnimatedSprite temporaryAnimatedSprite1 = new TemporaryAnimatedSprite(6, new Vector2((float) (notePosition.X * Game1.tileSize), (float) (notePosition.Y * Game1.tileSize)), Color.White, 8, false, 100f, 0, -1, -1f, -1, 0);
      temporaryAnimatedSprite1.layerDepth = 1f;
      temporaryAnimatedSprite1.interval = 50f;
      Vector2 vector2_1 = new Vector2(1f, 0.0f);
      temporaryAnimatedSprite1.motion = vector2_1;
      Vector2 vector2_2 = new Vector2(-0.005f, 0.0f);
      temporaryAnimatedSprite1.acceleration = vector2_2;
      temporarySprites1.Add(temporaryAnimatedSprite1);
      List<TemporaryAnimatedSprite> temporarySprites2 = this.temporarySprites;
      TemporaryAnimatedSprite temporaryAnimatedSprite2 = new TemporaryAnimatedSprite(6, new Vector2((float) (notePosition.X * Game1.tileSize - Game1.pixelZoom * 3), (float) (notePosition.Y * Game1.tileSize - Game1.pixelZoom * 3)), Color.White, 8, false, 100f, 0, -1, -1f, -1, 0);
      temporaryAnimatedSprite2.scale = 0.75f;
      temporaryAnimatedSprite2.layerDepth = 1f;
      temporaryAnimatedSprite2.interval = 50f;
      Vector2 vector2_3 = new Vector2(1f, 0.0f);
      temporaryAnimatedSprite2.motion = vector2_3;
      Vector2 vector2_4 = new Vector2(-0.005f, 0.0f);
      temporaryAnimatedSprite2.acceleration = vector2_4;
      int num1 = 50;
      temporaryAnimatedSprite2.delayBeforeAnimationStart = num1;
      temporarySprites2.Add(temporaryAnimatedSprite2);
      List<TemporaryAnimatedSprite> temporarySprites3 = this.temporarySprites;
      TemporaryAnimatedSprite temporaryAnimatedSprite3 = new TemporaryAnimatedSprite(6, new Vector2((float) (notePosition.X * Game1.tileSize - Game1.pixelZoom * 3), (float) (notePosition.Y * Game1.tileSize + Game1.pixelZoom * 3)), Color.White, 8, false, 100f, 0, -1, -1f, -1, 0);
      temporaryAnimatedSprite3.layerDepth = 1f;
      temporaryAnimatedSprite3.interval = 50f;
      Vector2 vector2_5 = new Vector2(1f, 0.0f);
      temporaryAnimatedSprite3.motion = vector2_5;
      Vector2 vector2_6 = new Vector2(-0.005f, 0.0f);
      temporaryAnimatedSprite3.acceleration = vector2_6;
      int num2 = 100;
      temporaryAnimatedSprite3.delayBeforeAnimationStart = num2;
      temporarySprites3.Add(temporaryAnimatedSprite3);
      List<TemporaryAnimatedSprite> temporarySprites4 = this.temporarySprites;
      TemporaryAnimatedSprite temporaryAnimatedSprite4 = new TemporaryAnimatedSprite(6, new Vector2((float) (notePosition.X * Game1.tileSize), (float) (notePosition.Y * Game1.tileSize)), Color.White, 8, false, 100f, 0, -1, -1f, -1, 0);
      temporaryAnimatedSprite4.layerDepth = 1f;
      temporaryAnimatedSprite4.scale = 0.75f;
      temporaryAnimatedSprite4.interval = 50f;
      Vector2 vector2_7 = new Vector2(1f, 0.0f);
      temporaryAnimatedSprite4.motion = vector2_7;
      Vector2 vector2_8 = new Vector2(-0.005f, 0.0f);
      temporaryAnimatedSprite4.acceleration = vector2_8;
      int num3 = 150;
      temporaryAnimatedSprite4.delayBeforeAnimationStart = num3;
      temporarySprites4.Add(temporaryAnimatedSprite4);
    }

    public int numberOfCompleteBundles()
    {
      int num = 0;
      foreach (KeyValuePair<int, bool[]> bundle in (Dictionary<int, bool[]>) this.bundles)
      {
        ++num;
        for (int index = 0; index < bundle.Value.Length; ++index)
        {
          if (!bundle.Value[index])
          {
            --num;
            break;
          }
        }
      }
      return num;
    }

    public void addStarToPlaque()
    {
      this.numberOfStarsOnPlaque = this.numberOfStarsOnPlaque + 1;
    }

    private string getMessageForAreaCompletion()
    {
      int numberOfAreasComplete = this.getNumberOfAreasComplete();
      if (numberOfAreasComplete < 1 || numberOfAreasComplete > 6)
        return "";
      return Game1.content.LoadString("Strings\\Locations:CommunityCenter_AreaCompletion" + (object) numberOfAreasComplete, (object) Game1.player.name);
    }

    private int getNumberOfAreasComplete()
    {
      int num = 0;
      for (int index = 0; index < this.areasComplete.Length; ++index)
      {
        if (this.areasComplete[index])
          ++num;
      }
      return num;
    }

    public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
    {
      switch (this.map.GetLayer("Buildings").Tiles[tileLocation] != null ? this.map.GetLayer("Buildings").Tiles[tileLocation].TileIndex : -1)
      {
        case 1799:
          if (this.numberOfCompleteBundles() > 2)
          {
            Game1.activeClickableMenu = (IClickableMenu) new JunimoNoteMenu(5, (Dictionary<int, bool[]>) this.bundles);
            break;
          }
          break;
        case 1824:
        case 1825:
        case 1826:
        case 1827:
        case 1828:
        case 1829:
        case 1830:
        case 1831:
        case 1832:
        case 1833:
          Game1.activeClickableMenu = (IClickableMenu) new JunimoNoteMenu(this.getAreaNumberFromLocation(who.getTileLocation()), (Dictionary<int, bool[]>) this.bundles);
          break;
      }
      return base.checkAction(tileLocation, viewport, who);
    }

    public void addJunimoNoteViewportTarget(int area)
    {
      if (this.junimoNotesViewportTargets == null)
        this.junimoNotesViewportTargets = new List<int>();
      this.junimoNotesViewportTargets.Add(area);
    }

    public void checkForNewJunimoNotes()
    {
      for (int area = 0; area < this.areasComplete.Length; ++area)
      {
        if (!this.isJunimoNoteAtArea(area) && this.shouldNoteAppearInArea(area))
          this.addJunimoNoteViewportTarget(area);
      }
    }

    public void removeJunimoNote(int area)
    {
      Point notePosition = this.getNotePosition(area);
      if (area == 5)
        this.map.GetLayer("Front").Tiles[notePosition.X, notePosition.Y] = (Tile) null;
      else
        this.map.GetLayer("Buildings").Tiles[notePosition.X, notePosition.Y] = (Tile) null;
    }

    public bool isJunimoNoteAtArea(int area)
    {
      Point notePosition = this.getNotePosition(area);
      if (area == 5)
        return this.map.GetLayer("Front").Tiles[notePosition.X, notePosition.Y] != null;
      return this.map.GetLayer("Buildings").Tiles[notePosition.X, notePosition.Y] != null;
    }

    public bool shouldNoteAppearInArea(int area)
    {
      if (area >= 0 && this.areasComplete.Length > area && !this.areasComplete[area])
      {
        switch (area)
        {
          case 0:
          case 2:
            if (this.numberOfCompleteBundles() > 0)
              return true;
            break;
          case 1:
            return true;
          case 3:
            if (this.numberOfCompleteBundles() > 1)
              return true;
            break;
          case 4:
            if (this.numberOfCompleteBundles() > 3)
              return true;
            break;
          case 5:
            if (this.numberOfCompleteBundles() > 2)
              return true;
            break;
        }
      }
      return false;
    }

    public override void resetForPlayerEntry()
    {
      base.resetForPlayerEntry();
      if (Game1.player.mailReceived.Contains("JojaMember"))
      {
        this.map = Game1.game1.xTileContent.Load<Map>("Maps\\CommunityCenter_Joja");
        this.warehouse = true;
        this.refurbishedLoaded = true;
      }
      else if (this.areAllAreasComplete() && !this.refurbishedLoaded)
      {
        this.map = Game1.game1.xTileContent.Load<Map>("Maps\\CommunityCenter_Refurbished");
        this.refurbishedLoaded = true;
      }
      else
      {
        for (int index = 0; index < this.areasComplete.Length; ++index)
        {
          if (this.shouldNoteAppearInArea(index))
          {
            this.addJunimoNote(index);
            this.characters.Add((NPC) new Junimo(new Vector2((float) this.getNotePosition(index).X, (float) (this.getNotePosition(index).Y + 2)) * (float) Game1.tileSize, index, false));
          }
          else if (this.areasComplete[index])
            this.loadArea(index, false);
        }
      }
      this.numberOfStarsOnPlaque = 0;
      for (int index = 0; index < this.areasComplete.Length; ++index)
      {
        if (this.areasComplete[index])
          this.numberOfStarsOnPlaque = this.numberOfStarsOnPlaque + 1;
      }
      if (Game1.eventUp || this.areAllAreasComplete())
        return;
      Game1.changeMusicTrack("communityCenter");
    }

    private int getAreaNumberFromLocation(Vector2 tileLocation)
    {
      for (int area = 0; area < this.areasComplete.Length; ++area)
      {
        if (this.getAreaBounds(area).Contains((int) tileLocation.X, (int) tileLocation.Y))
          return area;
      }
      return -1;
    }

    private Microsoft.Xna.Framework.Rectangle getAreaBounds(int area)
    {
      switch (area)
      {
        case 0:
          return new Microsoft.Xna.Framework.Rectangle(0, 0, 22, 11);
        case 1:
          return new Microsoft.Xna.Framework.Rectangle(0, 12, 21, 17);
        case 2:
          return new Microsoft.Xna.Framework.Rectangle(35, 4, 9, 9);
        case 3:
          return new Microsoft.Xna.Framework.Rectangle(52, 9, 16, 12);
        case 4:
          return new Microsoft.Xna.Framework.Rectangle(45, 0, 15, 9);
        case 5:
          return new Microsoft.Xna.Framework.Rectangle(22, 13, 28, 9);
        case 6:
          return new Microsoft.Xna.Framework.Rectangle(44, 10, 6, 3);
        case 7:
          return new Microsoft.Xna.Framework.Rectangle(22, 4, 13, 9);
        default:
          return Microsoft.Xna.Framework.Rectangle.Empty;
      }
    }

    public override void cleanupBeforePlayerExit()
    {
      base.cleanupBeforePlayerExit();
      for (int index = this.characters.Count - 1; index >= 0; --index)
      {
        if (this.characters[index] is Junimo)
          this.characters.RemoveAt(index);
      }
      Game1.changeMusicTrack("none");
    }

    public bool isBundleComplete(int bundleIndex)
    {
      for (int index = 0; index < this.bundles[bundleIndex].Length; ++index)
      {
        if (!this.bundles[bundleIndex][index])
          return false;
      }
      return true;
    }

    public void areaCompleteReward(int whichArea)
    {
      string str = "";
      switch (whichArea)
      {
        case 0:
          str = "ccPantry";
          break;
        case 1:
          str = "ccCraftsRoom";
          break;
        case 2:
          str = "ccFishTank";
          break;
        case 3:
          str = "ccBoilerRoom";
          break;
        case 4:
          str = "ccVault";
          break;
        case 5:
          str = "ccBulletin";
          Game1.addMailForTomorrow("ccBulletinThankYou", false, false);
          using (List<NPC>.Enumerator enumerator = Utility.getAllCharacters().GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              NPC current = enumerator.Current;
              if (!current.datable)
                Game1.player.changeFriendship(500, current);
            }
            break;
          }
      }
      if (str.Length <= 0 || Game1.player.mailReceived.Contains(str))
        return;
      Game1.player.mailForTomorrow.Add(str + "%&NL&%");
    }

    public void completeBundle(int which)
    {
      bool flag1 = false;
      for (int index = 0; index < this.bundles[which].Length; ++index)
      {
        if (!flag1 && !this.bundles[which][index])
          flag1 = true;
        this.bundles[which][index] = true;
      }
      if (flag1)
        this.bundleRewards[which] = true;
      int bundleToArea = this.bundleToAreaDictionary[which];
      if (this.areasComplete[bundleToArea])
        return;
      bool flag2 = false;
      foreach (int bundleIndex in this.areaToBundleDictionary[bundleToArea])
      {
        if (!this.isBundleComplete(bundleIndex))
        {
          flag2 = true;
          break;
        }
      }
      if (flag2)
        return;
      this.areasComplete[bundleToArea] = true;
      this.areaCompleteReward(bundleToArea);
      if (Game1.IsMultiplayer)
        Game1.ChatBox.receiveChatMessage(Game1.content.LoadString("Strings\\Locations:CommunityCenter_AreaRestored", (object) CommunityCenter.getAreaDisplayNameFromNumber(bundleToArea)), -1L);
      else
        Game1.showGlobalMessage(Game1.content.LoadString("Strings\\Locations:CommunityCenter_AreaRestored", (object) CommunityCenter.getAreaDisplayNameFromNumber(bundleToArea)));
    }

    public void loadArea(int area, bool showEffects = true)
    {
      Microsoft.Xna.Framework.Rectangle areaBounds = this.getAreaBounds(area);
      Map map = Game1.game1.xTileContent.Load<Map>("Maps\\CommunityCenter_Refurbished");
      for (int x = areaBounds.X; x < areaBounds.Right; ++x)
      {
        for (int y = areaBounds.Y; y < areaBounds.Bottom; ++y)
        {
          if (map.GetLayer("Back").Tiles[x, y] != null)
            this.map.GetLayer("Back").Tiles[x, y].TileIndex = map.GetLayer("Back").Tiles[x, y].TileIndex;
          if (map.GetLayer("Buildings").Tiles[x, y] != null)
          {
            this.map.GetLayer("Buildings").Tiles[x, y] = (Tile) new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, map.GetLayer("Buildings").Tiles[x, y].TileIndex);
            this.adjustMapLightPropertiesForLamp(map.GetLayer("Buildings").Tiles[x, y].TileIndex, x, y, "Buildings");
          }
          else
            this.map.GetLayer("Buildings").Tiles[x, y] = (Tile) null;
          if (map.GetLayer("Front").Tiles[x, y] != null)
          {
            this.map.GetLayer("Front").Tiles[x, y] = (Tile) new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, map.GetLayer("Front").Tiles[x, y].TileIndex);
            this.adjustMapLightPropertiesForLamp(map.GetLayer("Front").Tiles[x, y].TileIndex, x, y, "Front");
          }
          else
            this.map.GetLayer("Front").Tiles[x, y] = (Tile) null;
          if (map.GetLayer("Paths").Tiles[x, y] != null && map.GetLayer("Paths").Tiles[x, y].TileIndex == 8)
            Game1.currentLightSources.Add(new LightSource(4, new Vector2((float) (x * Game1.tileSize), (float) (y * Game1.tileSize)), 2f));
          if (showEffects && Game1.random.NextDouble() < 0.58 && map.GetLayer("Buildings").Tiles[x, y] == null)
          {
            List<TemporaryAnimatedSprite> temporarySprites = this.temporarySprites;
            TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(6, new Vector2((float) (x * Game1.tileSize), (float) (y * Game1.tileSize)), Color.White, 8, false, 100f, 0, -1, -1f, -1, 0);
            temporaryAnimatedSprite.layerDepth = 1f;
            temporaryAnimatedSprite.interval = 50f;
            Vector2 vector2_1 = new Vector2((float) Game1.random.Next(17) / 10f, 0.0f);
            temporaryAnimatedSprite.motion = vector2_1;
            Vector2 vector2_2 = new Vector2(-0.005f, 0.0f);
            temporaryAnimatedSprite.acceleration = vector2_2;
            int num = Game1.random.Next(500);
            temporaryAnimatedSprite.delayBeforeAnimationStart = num;
            temporarySprites.Add(temporaryAnimatedSprite);
          }
        }
      }
      if (area == 5)
        this.loadArea(6, true);
      this.addLightGlows();
    }

    public void restoreAreaCutscene(int whichArea)
    {
      Game1.freezeControls = true;
      this.restoreAreaIndex = whichArea;
      this.restoreAreaPhase = 0;
      this.restoreAreaTimer = 1000;
      Game1.changeMusicTrack("none");
      this.areasComplete[whichArea] = true;
    }

    public override void UpdateWhenCurrentLocation(GameTime time)
    {
      base.UpdateWhenCurrentLocation(time);
      if (this.restoreAreaTimer > 0)
      {
        int restoreAreaTimer = this.restoreAreaTimer;
        this.restoreAreaTimer = this.restoreAreaTimer - time.ElapsedGameTime.Milliseconds;
        switch (this.restoreAreaPhase)
        {
          case 0:
            if (this.restoreAreaTimer > 0)
              break;
            this.restoreAreaTimer = 3000;
            this.restoreAreaPhase = 1;
            Game1.player.faceDirection(2);
            Game1.player.jump();
            Game1.player.jitterStrength = 1f;
            Game1.player.showFrame(94, false);
            break;
          case 1:
            if (Game1.random.NextDouble() < 0.4)
            {
              Vector2 positionInThisRectangle = Utility.getRandomPositionInThisRectangle(this.getAreaBounds(this.restoreAreaIndex), Game1.random);
              Junimo junimo = new Junimo(positionInThisRectangle * (float) Game1.tileSize, this.restoreAreaIndex, true);
              if (!this.isCollidingPosition(junimo.GetBoundingBox(), Game1.viewport, (Character) junimo))
              {
                this.characters.Add((NPC) junimo);
                this.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.random.NextDouble() < 0.5 ? 5 : 46, positionInThisRectangle * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 4), (float) (Game1.tileSize / 4)), Color.White, 8, false, 100f, 0, -1, -1f, -1, 0)
                {
                  layerDepth = 1f
                });
                Game1.playSound("tinyWhip");
              }
            }
            if (this.restoreAreaTimer <= 0)
            {
              this.restoreAreaTimer = 999999;
              this.restoreAreaPhase = 2;
              Game1.screenGlowOnce(Color.White, true, 0.005f, 1f);
              if (Game1.soundBank != null)
              {
                this.buildUpSound = Game1.soundBank.GetCue("wind");
                this.buildUpSound.SetVariable("Volume", 0.0f);
                this.buildUpSound.SetVariable("Frequency", 0.0f);
                this.buildUpSound.Play();
              }
              Game1.player.jitterStrength = 2f;
              Game1.player.stopShowingFrame();
            }
            Game1.drawLighting = false;
            break;
          case 2:
            if (this.buildUpSound != null)
            {
              this.buildUpSound.SetVariable("Volume", Game1.screenGlowAlpha * 150f);
              this.buildUpSound.SetVariable("Frequency", Game1.screenGlowAlpha * 150f);
            }
            if ((double) Game1.screenGlowAlpha >= (double) Game1.screenGlowMax)
            {
              this.messageAlpha = this.messageAlpha + 0.008f;
              this.messageAlpha = Math.Min(this.messageAlpha, 1f);
            }
            if ((double) Game1.screenGlowAlpha == (double) Game1.screenGlowMax && this.restoreAreaTimer > 5200)
              this.restoreAreaTimer = 5200;
            if (this.restoreAreaTimer < 5200 && Game1.random.NextDouble() < (double) (5200 - this.restoreAreaTimer) / 10000.0)
              Game1.playSound(Game1.random.NextDouble() < 0.5 ? "dustMeep" : "junimoMeep1");
            if (this.restoreAreaTimer > 0)
              break;
            this.restoreAreaTimer = 2000;
            this.restoreAreaPhase = 3;
            Game1.screenGlowHold = false;
            this.loadArea(this.restoreAreaIndex, true);
            if (this.buildUpSound != null)
              this.buildUpSound.Stop(AudioStopOptions.Immediate);
            Game1.playSound("wand");
            Game1.changeMusicTrack("junimoStarSong");
            Game1.playSound("woodyHit");
            this.messageAlpha = 0.0f;
            Game1.flashAlpha = 1f;
            Game1.player.stopJittering();
            for (int index = this.characters.Count - 1; index >= 0; --index)
            {
              if (this.characters[index] is Junimo && (this.characters[index] as Junimo).temporaryJunimo)
                this.characters.RemoveAt(index);
            }
            Game1.drawLighting = true;
            break;
          case 3:
            if (restoreAreaTimer > 1000 && this.restoreAreaTimer <= 1000)
            {
              Junimo junimoForArea = this.getJunimoForArea(this.restoreAreaIndex);
              if (junimoForArea != null)
              {
                junimoForArea.position = Utility.getRandomAdjacentOpenTile(Game1.player.getTileLocation()) * (float) Game1.tileSize;
                int num;
                for (num = 0; this.isCollidingPosition(junimoForArea.GetBoundingBox(), Game1.viewport, (Character) junimoForArea) && num < 20; ++num)
                  junimoForArea.position = Utility.getRandomPositionInThisRectangle(this.getAreaBounds(this.restoreAreaIndex), Game1.random);
                if (num < 20)
                {
                  junimoForArea.fadeBack();
                  junimoForArea.returnToJunimoHutToFetchStar((GameLocation) this);
                }
              }
            }
            if (this.restoreAreaTimer > 0)
              break;
            Game1.freezeControls = false;
            break;
        }
      }
      else
      {
        if (Game1.activeClickableMenu != null || this.junimoNotesViewportTargets == null || (this.junimoNotesViewportTargets.Count <= 0 || Game1.isViewportOnCustomPath()))
          return;
        this.setViewportToNextJunimoNoteTarget();
      }
    }

    private void setViewportToNextJunimoNoteTarget()
    {
      if (this.junimoNotesViewportTargets.Count > 0)
      {
        Game1.freezeControls = true;
        Point notePosition = this.getNotePosition(this.junimoNotesViewportTargets[0]);
        Game1.moveViewportTo(new Vector2((float) notePosition.X, (float) notePosition.Y) * (float) Game1.tileSize, 5f, 2000, new Game1.afterFadeFunction(this.afterViewportGetsToJunimoNotePosition), new Game1.afterFadeFunction(this.setViewportToNextJunimoNoteTarget));
      }
      else
      {
        Game1.viewportFreeze = true;
        Game1.viewportHold = 10000;
        Game1.globalFadeToBlack(new Game1.afterFadeFunction(Game1.afterFadeReturnViewportToPlayer), 0.02f);
        Game1.freezeControls = false;
        Game1.afterViewport = (Game1.afterFadeFunction) null;
      }
    }

    private void afterViewportGetsToJunimoNotePosition()
    {
      int notesViewportTarget = this.junimoNotesViewportTargets[0];
      this.junimoNotesViewportTargets.RemoveAt(0);
      this.addJunimoNote(notesViewportTarget);
      Game1.playSound("reward");
    }

    public Junimo getJunimoForArea(int whichArea)
    {
      foreach (Character character in this.characters)
      {
        if (character is Junimo && (character as Junimo).whichArea == whichArea)
          return character as Junimo;
      }
      Junimo junimo = new Junimo(Vector2.Zero, whichArea, false);
      this.addCharacter((NPC) junimo);
      return junimo;
    }

    public bool areAllAreasComplete()
    {
      foreach (bool flag in this.areasComplete)
      {
        if (!flag)
          return false;
      }
      return true;
    }

    public void junimoGoodbyeDance()
    {
      this.getJunimoForArea(0).position = new Vector2(23f, 11f) * (float) Game1.tileSize;
      this.getJunimoForArea(1).position = new Vector2(27f, 11f) * (float) Game1.tileSize;
      this.getJunimoForArea(2).position = new Vector2(24f, 12f) * (float) Game1.tileSize;
      this.getJunimoForArea(4).position = new Vector2(26f, 12f) * (float) Game1.tileSize;
      this.getJunimoForArea(3).position = new Vector2(28f, 12f) * (float) Game1.tileSize;
      this.getJunimoForArea(5).position = new Vector2(25f, 11f) * (float) Game1.tileSize;
      for (int whichArea = 0; whichArea < this.areasComplete.Length; ++whichArea)
      {
        this.getJunimoForArea(whichArea).stayStill();
        this.getJunimoForArea(whichArea).faceDirection(1);
        this.getJunimoForArea(whichArea).fadeBack();
        this.getJunimoForArea(whichArea).isInvisible = false;
        this.getJunimoForArea(whichArea).setAlpha(1f);
      }
      Game1.moveViewportTo(new Vector2((float) Game1.player.getStandingX(), (float) Game1.player.getStandingY()), 2f, 5000, new Game1.afterFadeFunction(this.startGoodbyeDance), new Game1.afterFadeFunction(this.endGoodbyeDance));
      Game1.viewportFreeze = false;
      Game1.freezeControls = true;
    }

    private void startGoodbyeDance()
    {
      Game1.freezeControls = true;
      this.getJunimoForArea(0).position = new Vector2(23f, 11f) * (float) Game1.tileSize;
      this.getJunimoForArea(1).position = new Vector2(27f, 11f) * (float) Game1.tileSize;
      this.getJunimoForArea(2).position = new Vector2(24f, 12f) * (float) Game1.tileSize;
      this.getJunimoForArea(4).position = new Vector2(26f, 12f) * (float) Game1.tileSize;
      this.getJunimoForArea(3).position = new Vector2(28f, 12f) * (float) Game1.tileSize;
      this.getJunimoForArea(5).position = new Vector2(25f, 11f) * (float) Game1.tileSize;
      for (int whichArea = 0; whichArea < this.areasComplete.Length; ++whichArea)
      {
        this.getJunimoForArea(whichArea).stayStill();
        this.getJunimoForArea(whichArea).faceDirection(1);
        this.getJunimoForArea(whichArea).fadeBack();
        this.getJunimoForArea(whichArea).isInvisible = false;
        this.getJunimoForArea(whichArea).setAlpha(1f);
        this.getJunimoForArea(whichArea).sayGoodbye();
      }
    }

    private void endGoodbyeDance()
    {
      for (int whichArea = 0; whichArea < this.areasComplete.Length; ++whichArea)
        this.getJunimoForArea(whichArea).fadeAway();
      Game1.pauseThenDoFunction(3600, new Game1.afterFadeFunction(this.loadJunimoHut));
      Game1.freezeControls = true;
    }

    private void loadJunimoHut()
    {
      this.loadArea(7, true);
      Game1.flashAlpha = 1f;
      Game1.playSound("wand");
      Game1.freezeControls = false;
      Game1.showGlobalMessage(Game1.content.LoadString("Strings\\Locations:CommunityCenter_JunimosReturned"));
    }

    public override void draw(SpriteBatch b)
    {
      base.draw(b);
      for (int index = 0; index < this.numberOfStarsOnPlaque; ++index)
      {
        switch (index)
        {
          case 0:
            b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (33 * Game1.tileSize + 6 * Game1.pixelZoom), (float) (5 * Game1.tileSize + Game1.pixelZoom))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(354, 401, 7, 7)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.8f);
            break;
          case 1:
            b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (33 * Game1.tileSize + 6 * Game1.pixelZoom), (float) (5 * Game1.tileSize + 11 * Game1.pixelZoom))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(354, 401, 7, 7)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.8f);
            break;
          case 2:
            b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (33 * Game1.tileSize - 4 * Game1.pixelZoom), (float) (6 * Game1.tileSize))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(354, 401, 7, 7)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.8f);
            break;
          case 3:
            b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (32 * Game1.tileSize + 2 * Game1.pixelZoom), (float) (5 * Game1.tileSize + 11 * Game1.pixelZoom))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(354, 401, 7, 7)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.8f);
            break;
          case 4:
            b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (32 * Game1.tileSize + 2 * Game1.pixelZoom), (float) (5 * Game1.tileSize + Game1.pixelZoom))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(354, 401, 7, 7)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.8f);
            break;
          case 5:
            b.Draw(Game1.mouseCursors, Game1.GlobalToLocal(Game1.viewport, new Vector2((float) (33 * Game1.tileSize - 4 * Game1.pixelZoom), (float) (5 * Game1.tileSize - 3 * Game1.pixelZoom))), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(354, 401, 7, 7)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, 0.8f);
            break;
        }
      }
    }

    public override void drawAboveAlwaysFrontLayer(SpriteBatch b)
    {
      base.drawAboveAlwaysFrontLayer(b);
      if ((double) this.messageAlpha <= 0.0)
        return;
      Junimo junimoForArea = this.getJunimoForArea(0);
      if (junimoForArea != null)
        b.Draw(junimoForArea.Sprite.Texture, new Vector2((float) (Game1.viewport.Width / 2 - Game1.tileSize / 2), (float) (Game1.viewport.Height * 2) / 3f - (float) Game1.tileSize), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle((int) (Game1.currentGameTime.TotalGameTime.TotalMilliseconds % 800.0) / 100 * 16, 0, 16, 16)), Color.Lime * this.messageAlpha, 0.0f, new Vector2((float) (junimoForArea.sprite.spriteWidth * Game1.pixelZoom / 2), (float) ((double) (junimoForArea.sprite.spriteHeight * Game1.pixelZoom) * 3.0 / 4.0)) / (float) Game1.pixelZoom, Math.Max(0.2f, 1f) * (float) Game1.pixelZoom, junimoForArea.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 1f);
      b.DrawString(Game1.dialogueFont, "\"" + Game1.parseText(this.getMessageForAreaCompletion() + "\"", Game1.dialogueFont, Game1.tileSize * 10), new Vector2((float) (Game1.viewport.Width / 2 - Game1.tileSize * 5), (float) (Game1.viewport.Height * 2) / 3f), Game1.textColor * this.messageAlpha * 0.6f);
    }

    public static string getAreaNameFromNumber(int areaNumber)
    {
      switch (areaNumber)
      {
        case 0:
          return "Pantry";
        case 1:
          return "Crafts Room";
        case 2:
          return "Fish Tank";
        case 3:
          return "Boiler Room";
        case 4:
          return "Vault";
        case 5:
          return "Bulletin Board";
        default:
          return "";
      }
    }

    public static string getAreaEnglishDisplayNameFromNumber(int areaNumber)
    {
      return Game1.content.LoadBaseString("Strings\\Locations:CommunityCenter_AreaName_" + CommunityCenter.getAreaNameFromNumber(areaNumber).Replace(" ", ""));
    }

    public static string getAreaDisplayNameFromNumber(int areaNumber)
    {
      return Game1.content.LoadString("Strings\\Locations:CommunityCenter_AreaName_" + CommunityCenter.getAreaNameFromNumber(areaNumber).Replace(" ", ""));
    }

    private StaticTile[] getJunimoNoteTileFrames(int area)
    {
      if (area == 5)
        return new StaticTile[13]
        {
          new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, 1741),
          new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, 1741),
          new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, 1741),
          new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, 1741),
          new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, 1741),
          new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, 1741),
          new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, 1741),
          new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, 1741),
          new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, 1741),
          new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, 1773),
          new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, 1805),
          new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, 1805),
          new StaticTile(this.map.GetLayer("Front"), this.map.TileSheets[0], BlendMode.Alpha, 1773)
        };
      return new StaticTile[20]
      {
        new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1833),
        new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1833),
        new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1833),
        new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1833),
        new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1833),
        new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1833),
        new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1833),
        new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1833),
        new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1833),
        new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1832),
        new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1824),
        new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1825),
        new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1826),
        new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1827),
        new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1828),
        new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1829),
        new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1830),
        new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1831),
        new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1832),
        new StaticTile(this.map.GetLayer("Buildings"), this.map.TileSheets[0], BlendMode.Alpha, 1833)
      };
    }
  }
}
