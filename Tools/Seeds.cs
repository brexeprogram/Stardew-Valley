// Decompiled with JetBrains decompiler
// Type: StardewValley.Tools.Seeds
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

namespace StardewValley.Tools
{
  public class Seeds : Stackable
  {
    private string seedType;
    private int numberInStack;

    public new int NumberInStack
    {
      get
      {
        return this.numberInStack;
      }
      set
      {
        this.numberInStack = value;
      }
    }

    public string SeedType
    {
      get
      {
        return this.seedType;
      }
      set
      {
        this.seedType = value;
      }
    }

    public Seeds()
    {
    }

    public Seeds(string seedType, int numberInStack)
      : base(nameof (Seeds), 0, 0, 0, true)
    {
      this.seedType = seedType;
      this.numberInStack = numberInStack;
      this.setCurrentTileIndexToSeedType();
      this.indexOfMenuItemView = this.CurrentParentTileIndex;
    }

    protected override string loadDisplayName()
    {
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:Seeds.cs.14209");
    }

    protected override string loadDescription()
    {
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:Seeds.cs.14210");
    }

    public override void DoFunction(GameLocation location, int x, int y, int power, Farmer who)
    {
      who.Stamina = who.Stamina - (float) (2.0 - (double) who.FarmingLevel * 0.100000001490116);
      this.numberInStack = this.numberInStack - 1;
      this.setCurrentTileIndexToSeedType();
      Game1.playSound("seeds");
    }

    private void setCurrentTileIndexToSeedType()
    {
      string seedType = this.seedType;
      // ISSUE: reference to a compiler-generated method
      uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(seedType);
      if (stringHash <= 2309904358U)
      {
        if (stringHash <= 1020152658U)
        {
          if (stringHash <= 137760495U)
          {
            if ((int) stringHash != 100164663)
            {
              if ((int) stringHash != 121410417)
              {
                if ((int) stringHash != 137760495 || !(seedType == "Garlic"))
                  return;
                this.CurrentParentTileIndex = 4;
              }
              else
              {
                if (!(seedType == "Beet"))
                  return;
                this.CurrentParentTileIndex = 62;
              }
            }
            else
            {
              if (!(seedType == "Rhubarb"))
                return;
              this.CurrentParentTileIndex = 6;
            }
          }
          else if (stringHash <= 321905522U)
          {
            if ((int) stringHash != 252262714)
            {
              if ((int) stringHash != 321905522 || !(seedType == "Potato"))
                return;
              this.CurrentParentTileIndex = 3;
            }
            else
            {
              if (!(seedType == "Spring Mix"))
                return;
              this.CurrentParentTileIndex = 63;
            }
          }
          else if ((int) stringHash != 795997688)
          {
            if ((int) stringHash != 1020152658 || !(seedType == "Summer Mix"))
              return;
            this.CurrentParentTileIndex = 64;
          }
          else
          {
            if (!(seedType == "Radish"))
              return;
            this.CurrentParentTileIndex = 12;
          }
        }
        else if (stringHash <= 1418370675U)
        {
          if ((int) stringHash != 1026502717)
          {
            if ((int) stringHash != 1155325948)
            {
              if ((int) stringHash != 1418370675 || !(seedType == "Corn"))
                return;
              this.CurrentParentTileIndex = 15;
            }
            else
            {
              if (!(seedType == "Melon"))
                return;
              this.CurrentParentTileIndex = 7;
            }
          }
          else
          {
            if (!(seedType == "Starfruit"))
              return;
            this.CurrentParentTileIndex = 14;
          }
        }
        else if (stringHash <= 1787800187U)
        {
          if ((int) stringHash != 1651195363)
          {
            if ((int) stringHash != 1787800187 || !(seedType == "Eggplant"))
              return;
            this.CurrentParentTileIndex = 56;
          }
          else
          {
            if (!(seedType == "Tomato"))
              return;
            this.CurrentParentTileIndex = 8;
          }
        }
        else if ((int) stringHash != 2051905485)
        {
          if ((int) stringHash != -1985062938 || !(seedType == "Kale"))
            return;
          this.CurrentParentTileIndex = 5;
        }
        else
        {
          if (!(seedType == "Cranberries"))
            return;
          this.CurrentParentTileIndex = 61;
        }
      }
      else if (stringHash <= 3388885286U)
      {
        if (stringHash <= 2510277530U)
        {
          if ((int) stringHash != -1893603845)
          {
            if ((int) stringHash != -1867812560)
            {
              if ((int) stringHash != -1784689766 || !(seedType == "Winter Mix"))
                return;
              this.CurrentParentTileIndex = 66;
            }
            else
            {
              if (!(seedType == "Wheat"))
                return;
              this.CurrentParentTileIndex = 11;
            }
          }
          else
          {
            if (!(seedType == "Yellow Pepper"))
              return;
            this.CurrentParentTileIndex = 10;
          }
        }
        else if (stringHash <= 2981019750U)
        {
          if ((int) stringHash != -1457663081)
          {
            if ((int) stringHash != -1313947546 || !(seedType == "Yam"))
              return;
            this.CurrentParentTileIndex = 60;
          }
          else
          {
            if (!(seedType == "Blueberry"))
              return;
            this.CurrentParentTileIndex = 9;
          }
        }
        else if ((int) stringHash != -1188360954)
        {
          if ((int) stringHash != -906082010 || !(seedType == "Bok Choy"))
            return;
          this.CurrentParentTileIndex = 59;
        }
        else
        {
          if (!(seedType == "Green Bean"))
            return;
          this.CurrentParentTileIndex = 1;
        }
      }
      else if (stringHash <= 3607993668U)
      {
        if ((int) stringHash != -825971770)
        {
          if ((int) stringHash != -777860941)
          {
            if ((int) stringHash != -686973628 || !(seedType == "Parsnip"))
              return;
            this.CurrentParentTileIndex = 0;
          }
          else
          {
            if (!(seedType == "Red Cabbage"))
              return;
            this.CurrentParentTileIndex = 13;
          }
        }
        else
        {
          if (!(seedType == "Fall Mix"))
            return;
          this.CurrentParentTileIndex = 65;
        }
      }
      else if (stringHash <= 3782215428U)
      {
        if ((int) stringHash != -522887687)
        {
          if ((int) stringHash != -512751868 || !(seedType == "Cauliflower"))
            return;
          this.CurrentParentTileIndex = 2;
        }
        else
        {
          if (!(seedType == "Ancient Fruit"))
            return;
          this.CurrentParentTileIndex = 72;
        }
      }
      else if ((int) stringHash != -296405793)
      {
        if ((int) stringHash != -113893165 || !(seedType == "Artichoke"))
          return;
        this.CurrentParentTileIndex = 57;
      }
      else
      {
        if (!(seedType == "Pumpkin"))
          return;
        this.CurrentParentTileIndex = 58;
      }
    }
  }
}
