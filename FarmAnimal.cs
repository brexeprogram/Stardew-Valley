// Decompiled with JetBrains decompiler
// Type: StardewValley.FarmAnimal
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Buildings;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace StardewValley
{
  public class FarmAnimal : Character
  {
    public int uniqueFrameAccumulator = -1;
    public bool allowReproduction = true;
    public long parentId = -1;
    public const byte eatGrassBehavior = 0;
    public const short newHome = 0;
    public const short happy = 1;
    public const short neutral = 2;
    public const short unhappy = 3;
    public const short hungry = 4;
    public const short disturbedByDog = 5;
    public const short leftOutAtNight = 6;
    public const int hitsTillDead = 3;
    public const double chancePerUpdateToChangeDirection = 0.007;
    public const byte fullnessValueOfGrass = 60;
    public const int noWarpTimerTime = 3000;
    public new const double chanceForSound = 0.002;
    public const double chanceToGoOutside = 0.002;
    public const int uniqueDownFrame = 16;
    public const int uniqueRightFrame = 18;
    public const int uniqueUpFrame = 20;
    public const int uniqueLeftFrame = 22;
    public const int pushAccumulatorTimeTillPush = 40;
    public const int timePerUniqueFrame = 500;
    public const byte layHarvestType = 0;
    public const byte grabHarvestType = 1;
    public int defaultProduceIndex;
    public int deluxeProduceIndex;
    public int currentProduce;
    public int friendshipTowardFarmer;
    public int daysSinceLastFed;
    public int pushAccumulator;
    public int age;
    public int meatIndex;
    public int health;
    public int price;
    public int produceQuality;
    public byte daysToLay;
    public byte daysSinceLastLay;
    public byte ageWhenMature;
    public byte harvestType;
    public byte happiness;
    public byte fullness;
    public byte happinessDrain;
    public byte fullnessDrain;
    public bool wasPet;
    public bool showDifferentTextureWhenReadyForHarvest;
    public string sound;
    public string type;
    public string buildingTypeILiveIn;
    public string toolUsedForHarvest;
    private string _displayHouse;
    private string _displayType;
    public Microsoft.Xna.Framework.Rectangle frontBackBoundingBox;
    public Microsoft.Xna.Framework.Rectangle sidewaysBoundingBox;
    public Microsoft.Xna.Framework.Rectangle frontBackSourceRect;
    public Microsoft.Xna.Framework.Rectangle sidewaysSourceRect;
    public long myID;
    public long ownerID;
    [XmlIgnore]
    public Building home;
    public Vector2 homeLocation;
    [XmlIgnore]
    public int noWarpTimer;
    [XmlIgnore]
    public int hitGlowTimer;
    [XmlIgnore]
    public int pauseTimer;
    public short moodMessage;
    private bool isEating;

    [XmlIgnore]
    public string displayHouse
    {
      get
      {
        if (this._displayHouse == null)
        {
          string str;
          Game1.content.Load<Dictionary<string, string>>("Data\\FarmAnimals").TryGetValue(this.type, out str);
          this._displayHouse = this.buildingTypeILiveIn;
          if (str != null && LocalizedContentManager.CurrentLanguageCode != LocalizedContentManager.LanguageCode.en)
            this._displayHouse = str.Split('/')[26];
        }
        return this._displayHouse;
      }
      set
      {
        this._displayHouse = value;
      }
    }

    [XmlIgnore]
    public string displayType
    {
      get
      {
        if (this._displayType == null)
        {
          string str;
          Game1.content.Load<Dictionary<string, string>>("Data\\FarmAnimals").TryGetValue(this.type, out str);
          if (str != null)
            this._displayType = str.Split('/')[25];
        }
        return this._displayType;
      }
      set
      {
        this._displayType = value;
      }
    }

    public FarmAnimal()
    {
    }

    public FarmAnimal(string type, long id, long ownerID)
      : base((AnimatedSprite) null, new Vector2((float) (Game1.tileSize * Game1.random.Next(2, 9)), (float) (Game1.tileSize * Game1.random.Next(4, 8))), 2, type)
    {
      this.ownerID = ownerID;
      this.health = 3;
      if (type.Contains("Chicken") && !type.Equals("Void Chicken"))
      {
        if (Game1.IsMultiplayer)
        {
          type = type.Contains("Brown") || new Random((int) id).NextDouble() < 0.5 ? "Brown Chicken" : "White Chicken";
        }
        else
        {
          type = Game1.random.NextDouble() < 0.5 || type.Contains("Brown") ? "Brown Chicken" : "White Chicken";
          if (Game1.player.eventsSeen.Contains(3900074) && Game1.random.NextDouble() < 0.25)
            type = "Blue Chicken";
        }
      }
      if (type.Contains("Cow"))
        type = !Game1.IsMultiplayer ? (type.Contains("White") || Game1.random.NextDouble() >= 0.5 && !type.Contains("Brown") ? "White Cow" : "Brown Cow") : (type.Contains("White") || !type.Contains("Brown") && new Random((int) id).NextDouble() >= 0.5 ? "White Cow" : "Brown Cow");
      Dictionary<string, string> dictionary = Game1.content.Load<Dictionary<string, string>>("Data\\FarmAnimals");
      this.myID = id;
      string key = type;
      string str;
      dictionary.TryGetValue(key, out str);
      if (str != null)
      {
        string[] strArray = str.Split('/');
        this.daysToLay = Convert.ToByte(strArray[0]);
        this.ageWhenMature = Convert.ToByte(strArray[1]);
        this.defaultProduceIndex = Convert.ToInt32(strArray[2]);
        this.deluxeProduceIndex = Convert.ToInt32(strArray[3]);
        this.sound = strArray[4].Equals("none") ? (string) null : strArray[4];
        this.frontBackBoundingBox = new Microsoft.Xna.Framework.Rectangle(Convert.ToInt32(strArray[5]), Convert.ToInt32(strArray[6]), Convert.ToInt32(strArray[7]), Convert.ToInt32(strArray[8]));
        this.sidewaysBoundingBox = new Microsoft.Xna.Framework.Rectangle(Convert.ToInt32(strArray[9]), Convert.ToInt32(strArray[10]), Convert.ToInt32(strArray[11]), Convert.ToInt32(strArray[12]));
        this.harvestType = Convert.ToByte(strArray[13]);
        this.showDifferentTextureWhenReadyForHarvest = Convert.ToBoolean(strArray[14]);
        this.buildingTypeILiveIn = strArray[15];
        int int32 = Convert.ToInt32(strArray[16]);
        this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Animals\\" + ((int) this.ageWhenMature > 0 ? "Baby" : "") + (type.Equals("Duck") ? "White Chicken" : type)), 0, int32, Convert.ToInt32(strArray[17]));
        this.frontBackSourceRect = new Microsoft.Xna.Framework.Rectangle(0, 0, Convert.ToInt32(strArray[16]), Convert.ToInt32(strArray[17]));
        this.sidewaysSourceRect = new Microsoft.Xna.Framework.Rectangle(0, 0, Convert.ToInt32(strArray[18]), Convert.ToInt32(strArray[19]));
        this.fullnessDrain = Convert.ToByte(strArray[20]);
        this.happinessDrain = Convert.ToByte(strArray[21]);
        this.happiness = byte.MaxValue;
        this.fullness = byte.MaxValue;
        this.toolUsedForHarvest = strArray[22].Length > 0 ? strArray[22] : "";
        this.meatIndex = Convert.ToInt32(strArray[23]);
        this.price = Convert.ToInt32(strArray[24]);
      }
      this.type = type;
      this.name = Dialogue.randomName();
      this.displayName = this.name;
    }

    public string shortDisplayType()
    {
      switch (LocalizedContentManager.CurrentLanguageCode)
      {
        case LocalizedContentManager.LanguageCode.en:
          return ((IEnumerable<string>) this.displayType.Split(' ')).Last<string>();
        case LocalizedContentManager.LanguageCode.ja:
          if (this.displayType.Contains("トリ"))
            return "トリ";
          if (this.displayType.Contains("ウシ"))
            return "ウシ";
          if (!this.displayType.Contains("ブタ"))
            return this.displayType;
          return "ブタ";
        case LocalizedContentManager.LanguageCode.ru:
          if (this.displayType.ToLower().Contains("курица"))
            return "Курица";
          if (!this.displayType.ToLower().Contains("корова"))
            return this.displayType;
          return "Корова";
        case LocalizedContentManager.LanguageCode.zh:
          if (this.displayType.Contains("鸡"))
            return "鸡";
          if (this.displayType.Contains("牛"))
            return "牛";
          if (!this.displayType.Contains("猪"))
            return this.displayType;
          return "猪";
        case LocalizedContentManager.LanguageCode.pt:
        case LocalizedContentManager.LanguageCode.es:
          return ((IEnumerable<string>) this.displayType.Split(' ')).First<string>();
        case LocalizedContentManager.LanguageCode.de:
          return ((IEnumerable<string>) ((IEnumerable<string>) this.displayType.Split(' ')).Last<string>().Split('-')).Last<string>();
        default:
          return this.displayType;
      }
    }

    public bool isCoopDweller()
    {
      if (this.home != null)
        return this.home is Coop;
      return false;
    }

    public override Microsoft.Xna.Framework.Rectangle GetBoundingBox()
    {
      return new Microsoft.Xna.Framework.Rectangle((int) ((double) this.position.X + (double) (this.sprite.getWidth() * 4 / 2) - (double) (Game1.tileSize / 2) + 8.0), (int) ((double) this.position.Y + (double) (this.sprite.getHeight() * 4) - (double) Game1.tileSize + 8.0), Game1.tileSize * 3 / 4, Game1.tileSize * 3 / 4);
    }

    public void reload()
    {
      string str = this.type;
      if (this.age < (int) this.ageWhenMature)
        str = "Baby" + (this.type.Equals("Duck") ? "White Chicken" : this.type);
      else if (this.showDifferentTextureWhenReadyForHarvest && this.currentProduce <= 0)
        str = "Sheared" + this.type;
      this.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Animals\\" + str), 0, this.frontBackSourceRect.Width, this.frontBackSourceRect.Height);
      if (Game1.getLocationFromName("Farm") == null)
        return;
      foreach (Building building in (Game1.getLocationFromName("Farm") as Farm).buildings)
      {
        if (building.tileX == (int) this.homeLocation.X && building.tileY == (int) this.homeLocation.Y)
        {
          this.home = building;
          break;
        }
      }
    }

    public void pet(Farmer who)
    {
      if (who.FarmerSprite.pauseForSingleAnimation)
        return;
      who.Halt();
      who.faceGeneralDirection(this.position, 0);
      if (Game1.timeOfDay >= 1900 && !this.isMoving())
      {
        Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\FarmAnimals:TryingToSleep", (object) this.displayName));
      }
      else
      {
        this.Halt();
        this.sprite.StopAnimation();
        this.uniqueFrameAccumulator = -1;
        switch (Game1.player.FacingDirection)
        {
          case 0:
            this.sprite.currentFrame = 0;
            break;
          case 1:
            this.sprite.currentFrame = 12;
            break;
          case 2:
            this.sprite.currentFrame = 8;
            break;
          case 3:
            this.sprite.currentFrame = 4;
            break;
        }
        if (!this.wasPet)
        {
          this.wasPet = true;
          this.friendshipTowardFarmer = Math.Min(1000, this.friendshipTowardFarmer + 15);
          if (who.professions.Contains(3) && !this.isCoopDweller())
          {
            this.friendshipTowardFarmer = Math.Min(1000, this.friendshipTowardFarmer + 15);
            this.happiness = Math.Min(byte.MaxValue, (byte) ((uint) this.happiness + (uint) Math.Max(5, 40 - (int) this.happinessDrain)));
          }
          else if (who.professions.Contains(2) && this.isCoopDweller())
          {
            this.friendshipTowardFarmer = Math.Min(1000, this.friendshipTowardFarmer + 15);
            this.happiness = Math.Min(byte.MaxValue, (byte) ((uint) this.happiness + (uint) Math.Max(5, 40 - (int) this.happinessDrain)));
          }
          this.doEmote((int) this.moodMessage == 4 ? 12 : 20, true);
          this.happiness = (byte) Math.Min((int) byte.MaxValue, (int) this.happiness + Math.Max(5, 40 - (int) this.happinessDrain));
          if (this.sound != null && Game1.soundBank != null)
          {
            Cue cue = Game1.soundBank.GetCue(this.sound);
            string name = "Pitch";
            double num = (double) (1200 + Game1.random.Next(-200, 201));
            cue.SetVariable(name, (float) num);
            cue.Play();
          }
          who.gainExperience(0, 5);
        }
        else if (who.ActiveObject == null || who.ActiveObject.parentSheetIndex != 178)
          Game1.activeClickableMenu = (IClickableMenu) new AnimalQueryMenu(this);
        if (!this.type.Equals("Sheep") || this.friendshipTowardFarmer < 900)
          return;
        this.daysToLay = (byte) 2;
      }
    }

    public void farmerPushing()
    {
      this.pushAccumulator = this.pushAccumulator + 1;
      if (this.pushAccumulator <= 40)
        return;
      switch (Game1.player.facingDirection)
      {
        case 0:
          this.Halt();
          this.SetMovingUp(true);
          break;
        case 1:
          this.Halt();
          this.SetMovingRight(true);
          break;
        case 2:
          this.Halt();
          this.SetMovingDown(true);
          break;
        case 3:
          this.Halt();
          this.SetMovingLeft(true);
          break;
      }
      Game1.player.temporaryImpassableTile = this.GetBoundingBox();
      this.pushAccumulator = 0;
    }

    public void setRandomPosition(GameLocation location)
    {
      string[] strArray = location.getMapProperty("ProduceArea").Split(' ');
      int index1 = 0;
      int int32_1 = Convert.ToInt32(strArray[index1]);
      int index2 = 1;
      int int32_2 = Convert.ToInt32(strArray[index2]);
      int index3 = 2;
      int int32_3 = Convert.ToInt32(strArray[index3]);
      int index4 = 3;
      int int32_4 = Convert.ToInt32(strArray[index4]);
      this.position = new Vector2((float) (Game1.random.Next(int32_1, int32_1 + int32_3) * Game1.tileSize), (float) (Game1.random.Next(int32_2, int32_2 + int32_4) * Game1.tileSize));
      int num = 0;
      while (this.position.Equals(Vector2.Zero) || location.Objects.ContainsKey(this.position) || location.isCollidingPosition(this.GetBoundingBox(), Game1.viewport, false, 0, false, (Character) this))
      {
        this.position = new Vector2((float) Game1.random.Next(int32_1, int32_1 + int32_3), (float) Game1.random.Next(int32_2, int32_2 + int32_4)) * (float) Game1.tileSize;
        ++num;
        if (num > 64)
          break;
      }
    }

    public void dayUpdate(GameLocation environtment)
    {
      this.controller = (PathFindController) null;
      this.health = 3;
      bool flag1 = false;
      if (this.home != null && !(this.home.indoors as AnimalHouse).animals.ContainsKey(this.myID) && environtment is Farm)
      {
        if (!this.home.animalDoorOpen)
        {
          this.moodMessage = (short) 6;
          flag1 = true;
          this.happiness = (byte) ((uint) this.happiness / 2U);
        }
        else
        {
          (environtment as Farm).animals.Remove(this.myID);
          (this.home.indoors as AnimalHouse).animals.Add(this.myID, this);
          if (Game1.timeOfDay > 1800 && this.controller == null)
            this.happiness = (byte) ((uint) this.happiness / 2U);
          environtment = this.home.indoors;
          this.setRandomPosition(environtment);
        }
      }
      this.daysSinceLastLay = (byte) ((uint) this.daysSinceLastLay + 1U);
      if (!this.wasPet)
      {
        this.friendshipTowardFarmer = Math.Max(0, this.friendshipTowardFarmer - (10 - this.friendshipTowardFarmer / 200));
        this.happiness = (byte) Math.Max(0, (int) this.happiness - (int) this.happinessDrain * 5);
      }
      this.wasPet = false;
      if (((int) this.fullness < 200 || Game1.timeOfDay < 1700) && environtment is AnimalHouse)
      {
        for (int index = environtment.objects.Count - 1; index >= 0; --index)
        {
          KeyValuePair<Vector2, Object> keyValuePair = environtment.objects.ElementAt<KeyValuePair<Vector2, Object>>(index);
          if (keyValuePair.Value.Name.Equals("Hay"))
          {
            SerializableDictionary<Vector2, Object> objects = environtment.objects;
            keyValuePair = environtment.objects.ElementAt<KeyValuePair<Vector2, Object>>(index);
            Vector2 key = keyValuePair.Key;
            objects.Remove(key);
            this.fullness = byte.MaxValue;
            break;
          }
        }
      }
      Random random = new Random((int) this.myID / 2 + (int) Game1.stats.DaysPlayed);
      if ((int) this.fullness > 200 || random.NextDouble() < (double) ((int) this.fullness - 30) / 170.0)
      {
        this.age = this.age + 1;
        if (this.age == (int) this.ageWhenMature)
        {
          this.sprite.Texture = Game1.content.Load<Texture2D>("Animals\\" + this.type);
          if (this.type.Contains("Sheep"))
            this.currentProduce = this.defaultProduceIndex;
          this.daysSinceLastLay = (byte) 99;
        }
        this.happiness = (byte) Math.Min((int) byte.MaxValue, (int) this.happiness + (int) this.happinessDrain * 2);
      }
      if ((int) this.fullness < 200)
      {
        this.happiness = (byte) Math.Max(0, (int) this.happiness - 100);
        this.friendshipTowardFarmer = Math.Max(0, this.friendshipTowardFarmer - 20);
      }
      bool flag2 = (int) this.daysSinceLastLay >= (int) this.daysToLay - (!this.type.Equals("Sheep") || !Game1.getFarmer(this.ownerID).professions.Contains(3) ? 0 : 1) && random.NextDouble() < (double) this.fullness / 200.0 && random.NextDouble() < (double) this.happiness / 70.0;
      int parentSheetIndex;
      if (!flag2 || this.age < (int) this.ageWhenMature)
      {
        parentSheetIndex = -1;
      }
      else
      {
        parentSheetIndex = this.defaultProduceIndex;
        if (random.NextDouble() < (double) this.happiness / 150.0)
        {
          float num1 = (int) this.happiness > 200 ? (float) this.happiness * 1.5f : ((int) this.happiness <= 100 ? (float) ((int) this.happiness - 100) : 0.0f);
          if (this.type.Equals("Duck") && random.NextDouble() < ((double) this.friendshipTowardFarmer + (double) num1) / 5000.0 + Game1.dailyLuck + (double) Game1.player.LuckLevel * 0.01)
            parentSheetIndex = this.deluxeProduceIndex;
          else if (this.type.Equals("Rabbit") && random.NextDouble() < ((double) this.friendshipTowardFarmer + (double) num1) / 5000.0 + Game1.dailyLuck + (double) Game1.player.LuckLevel * 0.02)
            parentSheetIndex = this.deluxeProduceIndex;
          this.daysSinceLastLay = (byte) 0;
          if (parentSheetIndex <= 180)
          {
            if (parentSheetIndex != 176)
            {
              if (parentSheetIndex == 180)
                ++Game1.stats.ChickenEggsLayed;
            }
            else
              ++Game1.stats.ChickenEggsLayed;
          }
          else if (parentSheetIndex != 440)
          {
            if (parentSheetIndex == 442)
              ++Game1.stats.DuckEggsLayed;
          }
          else
            ++Game1.stats.RabbitWoolProduced;
          if (random.NextDouble() < ((double) this.friendshipTowardFarmer + (double) num1) / 1200.0 && !this.type.Equals("Duck") && (!this.type.Equals("Rabbit") && this.deluxeProduceIndex != -1) && this.friendshipTowardFarmer >= 200)
            parentSheetIndex = this.deluxeProduceIndex;
          double num2 = (double) this.friendshipTowardFarmer / 1000.0 - (1.0 - (double) this.happiness / 225.0);
          if (!this.isCoopDweller() && Game1.getFarmer(this.ownerID).professions.Contains(3) || this.isCoopDweller() && Game1.getFarmer(this.ownerID).professions.Contains(2))
            num2 += 0.33;
          this.produceQuality = num2 < 0.95 || random.NextDouble() >= num2 / 2.0 ? (random.NextDouble() >= num2 / 2.0 ? (random.NextDouble() >= num2 ? 0 : 1) : 2) : 4;
        }
      }
      if ((int) this.harvestType == 1 & flag2)
      {
        this.currentProduce = parentSheetIndex;
        parentSheetIndex = -1;
      }
      if (parentSheetIndex != -1 && this.home != null && !this.home.indoors.Objects.ContainsKey(this.getTileLocation()))
        this.home.indoors.Objects.Add(this.getTileLocation(), new Object(Vector2.Zero, parentSheetIndex, (string) null, false, true, false, true)
        {
          quality = this.produceQuality
        });
      if (!flag1)
        this.moodMessage = (int) this.fullness >= 30 ? ((int) this.happiness >= 30 ? ((int) this.happiness >= 200 ? (short) 1 : (short) 2) : (short) 3) : (short) 4;
      if (Game1.timeOfDay < 1700)
        this.fullness = (byte) Math.Max(0, (int) this.fullness - (int) this.fullnessDrain * (1700 - Game1.timeOfDay) / 100);
      this.fullness = (byte) 0;
      if (Utility.isFestivalDay(Game1.dayOfMonth, Game1.currentSeason))
        this.fullness = (byte) 250;
      this.reload();
    }

    public int getSellPrice()
    {
      return (int) ((double) this.price * ((double) this.friendshipTowardFarmer / 1000.0 + 0.3));
    }

    public bool isMale()
    {
      string type = this.type;
      if (type == "Rabbit" || type == "Truffle Pig" || (type == "Hog" || type == "Pig"))
        return this.myID % 2L == 0L;
      return false;
    }

    public string getMoodMessage()
    {
      if ((int) this.harvestType == 2)
        this.name = "It";
      string str = this.isMale() ? "Male" : "Female";
      switch (this.moodMessage)
      {
        case 0:
          if (this.parentId != -1L)
            return Game1.content.LoadString("Strings\\FarmAnimals:MoodMessage_NewHome_Baby_" + str, (object) this.displayName);
          return Game1.content.LoadString("Strings\\FarmAnimals:MoodMessage_NewHome_Adult_" + str + "_" + (object) (Game1.dayOfMonth % 2 + 1), (object) this.displayName);
        case 4:
          return Game1.content.LoadString("Strings\\FarmAnimals:MoodMessage_" + (((long) Game1.dayOfMonth + this.myID) % 2L == 0L ? "Hungry1" : "Hungry2"), (object) this.displayName);
        case 5:
          return Game1.content.LoadString("Strings\\FarmAnimals:MoodMessage_DisturbedByDog_" + str, (object) this.displayName);
        case 6:
          return Game1.content.LoadString("Strings\\FarmAnimals:MoodMessage_LeftOutsideAtNight_" + str, (object) this.displayName);
        default:
          this.moodMessage = (int) this.happiness >= 30 ? ((int) this.happiness >= 200 ? (short) 1 : (short) 2) : (short) 3;
          switch (this.moodMessage)
          {
            case 1:
              return Game1.content.LoadString("Strings\\FarmAnimals:MoodMessage_Happy", (object) this.displayName);
            case 2:
              return Game1.content.LoadString("Strings\\FarmAnimals:MoodMessage_Fine", (object) this.displayName);
            case 3:
              return Game1.content.LoadString("Strings\\FarmAnimals:MoodMessage_Sad", (object) this.displayName);
            default:
              return "";
          }
      }
    }

    public bool isBaby()
    {
      return this.age < (int) this.ageWhenMature;
    }

    public void warpHome(Farm f, FarmAnimal a)
    {
      if (this.home == null)
        return;
      (this.home.indoors as AnimalHouse).animals.Add(this.myID, this);
      f.animals.Remove(this.myID);
      this.controller = (PathFindController) null;
      this.setRandomPosition(this.home.indoors);
      ++this.home.currentOccupants;
    }

    public override void draw(SpriteBatch b)
    {
      if (this.isCoopDweller())
        this.sprite.drawShadow(b, Game1.GlobalToLocal(Game1.viewport, this.position - new Vector2(0.0f, (float) (Game1.tileSize * 3 / 8))), this.isBaby() ? 3f : 4f);
      this.sprite.draw(b, Game1.GlobalToLocal(Game1.viewport, this.position - new Vector2(0.0f, (float) (Game1.tileSize * 3 / 8))), (float) (((double) (this.GetBoundingBox().Center.Y + Game1.pixelZoom) + (double) this.position.X / 1000.0) / 10000.0), 0, 0, this.hitGlowTimer > 0 ? Color.Red : Color.White, this.FacingDirection == 3, 4f, 0.0f, false);
      if (!this.isEmoting)
        return;
      Vector2 local = Game1.GlobalToLocal(Game1.viewport, this.position + new Vector2((float) (this.frontBackSourceRect.Width / 2 * 4 - Game1.tileSize / 2), this.isCoopDweller() ? (float) (-Game1.tileSize * 3 / 2) : (float) -Game1.tileSize));
      b.Draw(Game1.emoteSpriteSheet, local, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(this.CurrentEmoteIndex * 16 % Game1.emoteSpriteSheet.Width, this.CurrentEmoteIndex * 16 / Game1.emoteSpriteSheet.Width * 16, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, (float) this.GetBoundingBox().Bottom / 10000f);
    }

    public virtual void updateWhenNotCurrentLocation(Building currentBuilding, GameTime time, GameLocation environment)
    {
      if (!Game1.shouldTimePass())
        return;
      this.update(time, environment, this.myID, false);
      if (currentBuilding != null && Game1.random.NextDouble() < 0.002 && (currentBuilding.animalDoorOpen && Game1.timeOfDay < 1630) && (!Game1.isRaining && !Game1.currentSeason.Equals("winter") && (environment.getFarmersCount() == 0 && !environment.Equals((object) Game1.currentLocation))))
      {
        Farm locationFromName = (Farm) Game1.getLocationFromName("Farm");
        if (locationFromName.isCollidingPosition(new Microsoft.Xna.Framework.Rectangle((currentBuilding.tileX + currentBuilding.animalDoor.X) * Game1.tileSize + 2, (currentBuilding.tileY + currentBuilding.animalDoor.Y) * Game1.tileSize + 2, (this.isCoopDweller() ? Game1.tileSize : Game1.tileSize * 2) - 4, Game1.tileSize - 4), Game1.viewport, false, 0, false, (Character) this, false, false, false) || locationFromName.isCollidingPosition(new Microsoft.Xna.Framework.Rectangle((currentBuilding.tileX + currentBuilding.animalDoor.X) * Game1.tileSize + 2, (currentBuilding.tileY + currentBuilding.animalDoor.Y + 1) * Game1.tileSize + 2, (this.isCoopDweller() ? Game1.tileSize : Game1.tileSize * 2) - 4, Game1.tileSize - 4), Game1.viewport, false, 0, false, (Character) this, false, false, false))
          return;
        if (locationFromName.animals.ContainsKey(this.myID))
        {
          for (int index = locationFromName.animals.Count - 1; index >= 0; --index)
          {
            if (locationFromName.animals.ElementAt<KeyValuePair<long, FarmAnimal>>(index).Key.Equals(this.myID))
            {
              locationFromName.animals.Remove(this.myID);
              break;
            }
          }
        }
        locationFromName.animals.Add(this.myID, this);
        ((AnimalHouse) currentBuilding.indoors).animals.Remove(this.myID);
        this.faceDirection(2);
        this.SetMovingDown(true);
        this.position = new Vector2((float) currentBuilding.getRectForAnimalDoor().X, (float) ((currentBuilding.tileY + currentBuilding.animalDoor.Y) * Game1.tileSize - (this.sprite.getHeight() * Game1.pixelZoom - this.GetBoundingBox().Height) + Game1.tileSize / 2));
        this.controller = new PathFindController((Character) this, (GameLocation) locationFromName, new PathFindController.isAtEnd(FarmAnimal.grassEndPointFunction), Game1.random.Next(4), false, new PathFindController.endBehavior(FarmAnimal.behaviorAfterFindingGrassPatch), 200, Point.Zero);
        if (this.controller == null || this.controller.pathToEndPoint == null || this.controller.pathToEndPoint.Count < 3)
        {
          this.SetMovingDown(true);
          this.controller = (PathFindController) null;
        }
        else
        {
          this.faceDirection(2);
          this.position = new Vector2((float) (this.controller.pathToEndPoint.Peek().X * Game1.tileSize), (float) (this.controller.pathToEndPoint.Peek().Y * Game1.tileSize - (this.sprite.getHeight() * Game1.pixelZoom - this.GetBoundingBox().Height) + Game1.tileSize / 4));
          if (!this.isCoopDweller())
            this.position.X -= (float) (Game1.tileSize / 2);
        }
        this.noWarpTimer = 3000;
        --currentBuilding.currentOccupants;
        if (Utility.isOnScreen(this.getTileLocationPoint(), Game1.tileSize * 3, (GameLocation) locationFromName))
          Game1.playSound("sandyStep");
        if (environment.isTileOccupiedByFarmer(this.getTileLocation()) != null)
          environment.isTileOccupiedByFarmer(this.getTileLocation()).temporaryImpassableTile = this.GetBoundingBox();
      }
      this.behaviors(time, environment);
      this.MovePosition(time, Game1.viewport, environment);
    }

    public static void behaviorAfterFindingGrassPatch(Character c, GameLocation environment)
    {
      if ((int) ((FarmAnimal) c).fullness >= (int) byte.MaxValue)
        return;
      ((FarmAnimal) c).eatGrass(environment);
    }

    public static bool animalDoorEndPointFunction(PathNode currentPoint, Point endPoint, GameLocation location, Character c)
    {
      Vector2 vector2 = new Vector2((float) currentPoint.x, (float) currentPoint.y);
      foreach (Building building in ((BuildableGameLocation) location).buildings)
      {
        if (building.animalDoor.X >= 0 && (double) (building.animalDoor.X + building.tileX) == (double) vector2.X && ((double) (building.animalDoor.Y + building.tileY) == (double) vector2.Y && building.buildingType.Contains(((FarmAnimal) c).buildingTypeILiveIn)) && building.currentOccupants < building.maxOccupants)
        {
          ++building.currentOccupants;
          Game1.playSound("dwop");
          return true;
        }
      }
      return false;
    }

    public static bool grassEndPointFunction(PathNode currentPoint, Point endPoint, GameLocation location, Character c)
    {
      Vector2 key = new Vector2((float) currentPoint.x, (float) currentPoint.y);
      if (!location.terrainFeatures.ContainsKey(key) || !(location.terrainFeatures[key].GetType() == typeof (Grass)))
        return false;
      foreach (KeyValuePair<long, FarmAnimal> animal in (Dictionary<long, FarmAnimal>) ((Farm) location).animals)
      {
        if ((double) animal.Value.getTileLocation().X == (double) currentPoint.x)
        {
          if ((double) animal.Value.getTileLocation().Y == (double) currentPoint.y)
            break;
        }
      }
      return true;
    }

    public virtual void updatePerTenMinutes(int timeOfDay, GameLocation environment)
    {
      if (timeOfDay >= 1800)
      {
        if (environment.IsOutdoors && timeOfDay > 1900 || !environment.IsOutdoors && (int) this.happiness > 150 || (environment.isOutdoors && Game1.isRaining || environment.isOutdoors && Game1.currentSeason.Equals("winter")))
          this.happiness = (byte) Math.Min((int) byte.MaxValue, Math.Max(0, (int) this.happiness - (environment.numberOfObjectsWithName("Heater") <= 0 || !Game1.currentSeason.Equals("winter") ? (int) this.happinessDrain : (int) -this.happinessDrain)));
        else if (environment.IsOutdoors)
          this.happiness = (byte) Math.Min((int) byte.MaxValue, (int) this.happiness + (int) this.happinessDrain);
      }
      if (environment.isTileOccupiedByFarmer(this.getTileLocation()) == null)
        return;
      environment.isTileOccupiedByFarmer(this.getTileLocation()).temporaryImpassableTile = this.GetBoundingBox();
    }

    public void eatGrass(GameLocation environment)
    {
      Vector2 index;
      // ISSUE: explicit reference operation
      // ISSUE: variable of a reference type
      Vector2& local = @index;
      Microsoft.Xna.Framework.Rectangle boundingBox = this.GetBoundingBox();
      double num1 = (double) (boundingBox.Center.X / Game1.tileSize);
      boundingBox = this.GetBoundingBox();
      double num2 = (double) (boundingBox.Center.Y / Game1.tileSize);
      // ISSUE: explicit reference operation
      ^local = new Vector2((float) num1, (float) num2);
      if (!environment.terrainFeatures.ContainsKey(index) || !(environment.terrainFeatures[index].GetType() == typeof (Grass)))
        return;
      this.isEating = true;
      if (((Grass) environment.terrainFeatures[index]).reduceBy(this.isCoopDweller() ? 2 : 4, index, environment.Equals((object) Game1.currentLocation)))
        environment.terrainFeatures.Remove(index);
      this.sprite.loop = false;
      this.fullness = byte.MaxValue;
      if ((int) this.moodMessage != 5 && (int) this.moodMessage != 6 && !Game1.isRaining)
      {
        this.happiness = byte.MaxValue;
        this.friendshipTowardFarmer = Math.Min(1000, this.friendshipTowardFarmer + 8);
      }
      if (!Game1.IsServer)
        return;
      MultiplayerUtility.broadcastNPCBehavior(this.myID, environment, (byte) 0);
    }

    public override void performBehavior(byte which)
    {
      if ((int) which != 0)
        return;
      this.eatGrass(Game1.currentLocation);
    }

    private bool behaviors(GameTime time, GameLocation location)
    {
      if (this.home == null)
        return false;
      if (this.isEating)
      {
        if (this.home != null && this.home.getRectForAnimalDoor().Intersects(this.GetBoundingBox()))
        {
          FarmAnimal.behaviorAfterFindingGrassPatch((Character) this, location);
          this.isEating = false;
          this.Halt();
          return false;
        }
        if (this.buildingTypeILiveIn.Contains("Barn"))
        {
          this.sprite.Animate(time, 16, 4, 100f);
          if (this.sprite.CurrentFrame >= 20)
          {
            this.isEating = false;
            this.sprite.loop = true;
            this.sprite.CurrentFrame = 0;
            this.faceDirection(2);
          }
        }
        else
        {
          this.sprite.Animate(time, 24, 4, 100f);
          if (this.sprite.CurrentFrame >= 28)
          {
            this.isEating = false;
            this.sprite.loop = true;
            this.sprite.CurrentFrame = 0;
            this.faceDirection(2);
          }
        }
        return true;
      }
      if (!Game1.IsClient)
      {
        if (this.controller != null)
          return true;
        if (location.IsOutdoors && (int) this.fullness < 195 && Game1.random.NextDouble() < 0.002)
          this.controller = new PathFindController((Character) this, location, new PathFindController.isAtEnd(FarmAnimal.grassEndPointFunction), -1, false, new PathFindController.endBehavior(FarmAnimal.behaviorAfterFindingGrassPatch), 200, Point.Zero);
        if (Game1.timeOfDay >= 1700 && location.IsOutdoors && (this.controller == null && Game1.random.NextDouble() < 0.002))
        {
          this.controller = new PathFindController((Character) this, location, new PathFindController.isAtEnd(PathFindController.isAtEndPoint), 0, false, (PathFindController.endBehavior) null, 200, new Point(this.home.tileX + this.home.animalDoor.X, this.home.tileY + this.home.animalDoor.Y));
          if (location.getFarmersCount() == 0)
          {
            ((AnimalHouse) this.home.indoors).animals.Add(this.myID, this);
            this.setRandomPosition(this.home.indoors);
            this.faceDirection(Game1.random.Next(4));
            this.controller = (PathFindController) null;
            (location as Farm).animals.Remove(this.myID);
            return true;
          }
        }
        if (location.IsOutdoors && !Game1.isRaining && (!Game1.currentSeason.Equals("winter") && this.currentProduce != -1) && (this.age >= (int) this.ageWhenMature && this.type.Contains("Pig") && Game1.random.NextDouble() < 0.0002))
        {
          Microsoft.Xna.Framework.Rectangle boundingBox = this.GetBoundingBox();
          for (int corner = 0; corner < 4; ++corner)
          {
            Vector2 cornersOfThisRectangle = Utility.getCornersOfThisRectangle(ref boundingBox, corner);
            Vector2 key = new Vector2((float) (int) ((double) cornersOfThisRectangle.X / (double) Game1.tileSize), (float) (int) ((double) cornersOfThisRectangle.Y / (double) Game1.tileSize));
            if (location.terrainFeatures.ContainsKey(key) || location.objects.ContainsKey(key))
              return false;
          }
          if (Game1.player.currentLocation.Equals((object) location))
          {
            DelayedAction.playSoundAfterDelay("dirtyHit", 450);
            DelayedAction.playSoundAfterDelay("dirtyHit", 900);
            DelayedAction.playSoundAfterDelay("dirtyHit", 1350);
          }
          if (location.Equals((object) Game1.currentLocation))
          {
            switch (this.FacingDirection)
            {
              case 0:
                this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
                {
                  new FarmerSprite.AnimationFrame(9, 250),
                  new FarmerSprite.AnimationFrame(11, 250),
                  new FarmerSprite.AnimationFrame(9, 250),
                  new FarmerSprite.AnimationFrame(11, 250),
                  new FarmerSprite.AnimationFrame(9, 250),
                  new FarmerSprite.AnimationFrame(11, 250, false, false, new AnimatedSprite.endOfAnimationBehavior(this.findTruffle), false)
                });
                break;
              case 1:
                this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
                {
                  new FarmerSprite.AnimationFrame(5, 250),
                  new FarmerSprite.AnimationFrame(7, 250),
                  new FarmerSprite.AnimationFrame(5, 250),
                  new FarmerSprite.AnimationFrame(7, 250),
                  new FarmerSprite.AnimationFrame(5, 250),
                  new FarmerSprite.AnimationFrame(7, 250, false, false, new AnimatedSprite.endOfAnimationBehavior(this.findTruffle), false)
                });
                break;
              case 2:
                this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
                {
                  new FarmerSprite.AnimationFrame(1, 250),
                  new FarmerSprite.AnimationFrame(3, 250),
                  new FarmerSprite.AnimationFrame(1, 250),
                  new FarmerSprite.AnimationFrame(3, 250),
                  new FarmerSprite.AnimationFrame(1, 250),
                  new FarmerSprite.AnimationFrame(3, 250, false, false, new AnimatedSprite.endOfAnimationBehavior(this.findTruffle), false)
                });
                break;
              case 3:
                this.sprite.setCurrentAnimation(new List<FarmerSprite.AnimationFrame>()
                {
                  new FarmerSprite.AnimationFrame(5, 250, false, true, (AnimatedSprite.endOfAnimationBehavior) null, false),
                  new FarmerSprite.AnimationFrame(7, 250, false, true, (AnimatedSprite.endOfAnimationBehavior) null, false),
                  new FarmerSprite.AnimationFrame(5, 250, false, true, (AnimatedSprite.endOfAnimationBehavior) null, false),
                  new FarmerSprite.AnimationFrame(7, 250, false, true, (AnimatedSprite.endOfAnimationBehavior) null, false),
                  new FarmerSprite.AnimationFrame(5, 250, false, true, (AnimatedSprite.endOfAnimationBehavior) null, false),
                  new FarmerSprite.AnimationFrame(7, 250, false, true, new AnimatedSprite.endOfAnimationBehavior(this.findTruffle), false)
                });
                break;
            }
            this.sprite.loop = false;
          }
          else
            this.findTruffle(Game1.player);
        }
      }
      return false;
    }

    private void findTruffle(Farmer who)
    {
      Utility.spawnObjectAround(Utility.getTranslatedVector2(this.getTileLocation(), this.FacingDirection, 1f), new Object(this.getTileLocation(), 430, 1), (GameLocation) Game1.getFarm());
      if (new Random((int) this.myID / 2 + (int) Game1.stats.DaysPlayed + Game1.timeOfDay).NextDouble() <= (double) this.friendshipTowardFarmer / 1500.0)
        return;
      this.currentProduce = -1;
    }

    public void hitWithWeapon(MeleeWeapon t)
    {
    }

    public void makeSound()
    {
      if (this.sound == null || Game1.soundBank == null)
        return;
      Cue cue = Game1.soundBank.GetCue(this.sound);
      string name = "Pitch";
      double num = (double) (1200 + Game1.random.Next(-200, 201));
      cue.SetVariable(name, (float) num);
      cue.Play();
    }

    public virtual bool updateWhenCurrentLocation(GameTime time, GameLocation location)
    {
      if (!Game1.shouldTimePass())
        return false;
      if (this.health <= 0)
        return true;
      if (this.hitGlowTimer > 0)
        this.hitGlowTimer = this.hitGlowTimer - time.ElapsedGameTime.Milliseconds;
      if (this.sprite.currentAnimation != null)
      {
        if (this.sprite.animateOnce(time))
          this.sprite.currentAnimation = (List<FarmerSprite.AnimationFrame>) null;
        return false;
      }
      this.update(time, location, this.myID, false);
      if (this.behaviors(time, location) || this.sprite.currentAnimation != null)
        return false;
      if (this.controller != null && this.controller.timerSinceLastCheckPoint > 10000)
      {
        this.controller = (PathFindController) null;
        this.Halt();
      }
      if (location.GetType() == typeof (Farm) && this.noWarpTimer <= 0 && this.home != null && this.home.getRectForAnimalDoor().Contains(this.GetBoundingBox().Center.X, this.GetBoundingBox().Top))
      {
        ((AnimalHouse) this.home.indoors).animals.Add(this.myID, this);
        this.setRandomPosition(this.home.indoors);
        this.faceDirection(Game1.random.Next(4));
        this.controller = (PathFindController) null;
        if (Utility.isOnScreen(this.getTileLocationPoint(), Game1.tileSize * 3, location))
          Game1.playSound("dwoop");
        return true;
      }
      int val1 = 0;
      int noWarpTimer = this.noWarpTimer;
      TimeSpan elapsedGameTime = time.ElapsedGameTime;
      int milliseconds1 = elapsedGameTime.Milliseconds;
      int val2 = noWarpTimer - milliseconds1;
      this.noWarpTimer = Math.Max(val1, val2);
      if (this.pauseTimer > 0)
      {
        int pauseTimer = this.pauseTimer;
        elapsedGameTime = time.ElapsedGameTime;
        int milliseconds2 = elapsedGameTime.Milliseconds;
        this.pauseTimer = pauseTimer - milliseconds2;
      }
      if (Game1.timeOfDay >= 2000)
      {
        this.sprite.currentFrame = this.buildingTypeILiveIn.Contains("Coop") ? 16 : 12;
        this.sprite.UpdateSourceRect();
        if (!this.isEmoting && Game1.random.NextDouble() < 0.002)
          this.doEmote(24, true);
      }
      else if (this.pauseTimer <= 0)
      {
        if (Game1.random.NextDouble() < 0.001 && this.age >= (int) this.ageWhenMature && ((int) Game1.gameMode == 3 && this.sound != null) && (Utility.isOnScreen(this.position, Game1.tileSize * 3) && Game1.soundBank != null))
        {
          Cue cue = Game1.soundBank.GetCue(this.sound);
          string name = "Pitch";
          double num = (double) (1200 + Game1.random.Next(-200, 201));
          cue.SetVariable(name, (float) num);
          cue.Play();
        }
        if (!Game1.IsClient && Game1.random.NextDouble() < 0.007 && this.uniqueFrameAccumulator == -1)
        {
          int direction = Game1.random.Next(5);
          if (direction != (this.facingDirection + 2) % 4)
          {
            if (direction < 4)
            {
              int facingDirection = this.facingDirection;
              this.faceDirection(direction);
              if (!location.isOutdoors && location.isCollidingPosition(this.nextPosition(direction), Game1.viewport, (Character) this))
              {
                this.faceDirection(facingDirection);
                return false;
              }
            }
            switch (direction)
            {
              case 0:
                this.SetMovingUp(true);
                break;
              case 1:
                this.SetMovingRight(true);
                break;
              case 2:
                this.SetMovingDown(true);
                break;
              case 3:
                this.SetMovingLeft(true);
                break;
              default:
                this.Halt();
                this.sprite.StopAnimation();
                break;
            }
          }
          else if (this.noWarpTimer <= 0)
          {
            this.Halt();
            this.sprite.StopAnimation();
          }
        }
        if (!Game1.IsClient && this.isMoving() && (Game1.random.NextDouble() < 0.014 && this.uniqueFrameAccumulator == -1) || Game1.IsClient && Game1.random.NextDouble() < 0.014 && (double) this.distanceFromLastServerPosition() <= 4.0)
        {
          this.Halt();
          this.sprite.StopAnimation();
          if (Game1.random.NextDouble() < 0.75)
          {
            this.uniqueFrameAccumulator = 0;
            if (this.buildingTypeILiveIn.Contains("Coop"))
            {
              switch (this.facingDirection)
              {
                case 0:
                  this.sprite.currentFrame = 20;
                  break;
                case 1:
                  this.sprite.currentFrame = 18;
                  break;
                case 2:
                  this.sprite.currentFrame = 16;
                  break;
                case 3:
                  this.sprite.currentFrame = 22;
                  break;
              }
            }
            else if (this.buildingTypeILiveIn.Contains("Barn"))
            {
              switch (this.facingDirection)
              {
                case 0:
                  this.sprite.currentFrame = 15;
                  break;
                case 1:
                  this.sprite.currentFrame = 14;
                  break;
                case 2:
                  this.sprite.currentFrame = 13;
                  break;
                case 3:
                  this.sprite.currentFrame = 14;
                  break;
              }
            }
          }
          this.sprite.UpdateSourceRect();
        }
        if (this.uniqueFrameAccumulator != -1)
        {
          int frameAccumulator = this.uniqueFrameAccumulator;
          elapsedGameTime = time.ElapsedGameTime;
          int milliseconds2 = elapsedGameTime.Milliseconds;
          this.uniqueFrameAccumulator = frameAccumulator + milliseconds2;
          if (this.uniqueFrameAccumulator > 500)
          {
            if (this.buildingTypeILiveIn.Contains("Coop"))
              this.sprite.CurrentFrame = this.sprite.CurrentFrame + 1 - this.sprite.CurrentFrame % 2 * 2;
            else if (this.sprite.CurrentFrame > 12)
            {
              this.sprite.CurrentFrame = (this.sprite.CurrentFrame - 13) * 4;
            }
            else
            {
              switch (this.facingDirection)
              {
                case 0:
                  this.sprite.CurrentFrame = 15;
                  break;
                case 1:
                  this.sprite.CurrentFrame = 14;
                  break;
                case 2:
                  this.sprite.CurrentFrame = 13;
                  break;
                case 3:
                  this.sprite.CurrentFrame = 14;
                  break;
              }
            }
            this.uniqueFrameAccumulator = 0;
            if (Game1.random.NextDouble() < 0.4)
              this.uniqueFrameAccumulator = -1;
          }
        }
        else if (!Game1.IsClient)
          this.MovePosition(time, Game1.viewport, location);
      }
      if (!this.isMoving() && location is Farm && this.controller == null)
      {
        this.Halt();
        Microsoft.Xna.Framework.Rectangle boundingBox1 = this.GetBoundingBox();
        foreach (KeyValuePair<long, FarmAnimal> animal in (Dictionary<long, FarmAnimal>) (location as Farm).animals)
        {
          if (!animal.Value.Equals((object) this))
          {
            Microsoft.Xna.Framework.Rectangle boundingBox2 = animal.Value.GetBoundingBox();
            if (boundingBox2.Intersects(boundingBox1))
            {
              int x1 = boundingBox1.Center.X;
              boundingBox2 = animal.Value.GetBoundingBox();
              int x2 = boundingBox2.Center.X;
              int num1 = x1 - x2;
              int y1 = boundingBox1.Center.Y;
              boundingBox2 = animal.Value.GetBoundingBox();
              int y2 = boundingBox2.Center.Y;
              int num2 = y1 - y2;
              if (num1 > 0 && Math.Abs(num1) > Math.Abs(num2))
                this.SetMovingUp(true);
              else if (num1 < 0 && Math.Abs(num1) > Math.Abs(num2))
                this.SetMovingDown(true);
              else if (num2 > 0)
                this.SetMovingLeft(true);
              else
                this.SetMovingRight(true);
            }
          }
        }
      }
      return false;
    }

    public override bool shouldCollideWithBuildingLayer(GameLocation location)
    {
      return true;
    }

    public override void MovePosition(GameTime time, xTile.Dimensions.Rectangle viewport, GameLocation currentLocation)
    {
      if (this.pauseTimer > 0)
        return;
      if (this.moveUp)
      {
        if (!currentLocation.isCollidingPosition(this.nextPosition(0), Game1.viewport, false, 0, false, (Character) this, false, false, false))
        {
          this.position.Y -= (float) this.speed;
          this.sprite.AnimateUp(time, 0, "");
        }
        else
        {
          this.Halt();
          this.sprite.StopAnimation();
          if (Game1.random.NextDouble() < 0.6)
            this.SetMovingDown(true);
        }
        this.faceDirection(0);
      }
      else if (this.moveRight)
      {
        if (!currentLocation.isCollidingPosition(this.nextPosition(1), Game1.viewport, false, 0, false, (Character) this))
        {
          this.position.X += (float) this.speed;
          this.sprite.AnimateRight(time, 0, "");
        }
        else
        {
          this.Halt();
          this.sprite.StopAnimation();
          if (Game1.random.NextDouble() < 0.6)
            this.SetMovingLeft(true);
        }
        this.faceDirection(1);
      }
      else if (this.moveDown)
      {
        if (!currentLocation.isCollidingPosition(this.nextPosition(2), Game1.viewport, false, 0, false, (Character) this))
        {
          this.position.Y += (float) this.speed;
          this.sprite.AnimateDown(time, 0, "");
        }
        else
        {
          this.Halt();
          this.sprite.StopAnimation();
          if (Game1.random.NextDouble() < 0.6)
            this.SetMovingUp(true);
        }
        this.faceDirection(2);
      }
      else
      {
        if (!this.moveLeft)
          return;
        if (!currentLocation.isCollidingPosition(this.nextPosition(3), Game1.viewport, false, 0, false, (Character) this))
        {
          this.position.X -= (float) this.speed;
          this.sprite.AnimateRight(time, 0, "");
        }
        else
        {
          this.Halt();
          this.sprite.StopAnimation();
          if (Game1.random.NextDouble() < 0.6)
            this.SetMovingRight(true);
        }
        this.facingDirection = 3;
        if (this.isCoopDweller() || this.sprite.CurrentFrame <= 7)
          return;
        this.sprite.CurrentFrame = 4;
      }
    }
  }
}
