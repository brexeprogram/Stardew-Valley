// Decompiled with JetBrains decompiler
// Type: StardewValley.Utility
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.Buildings;
using StardewValley.Characters;
using StardewValley.Events;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Minigames;
using StardewValley.Monsters;
using StardewValley.Objects;
using StardewValley.Quests;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using xTile.Dimensions;

namespace StardewValley
{
  public class Utility
  {
    public static readonly char[] CharSpace = new char[1]
    {
      ' '
    };
    public static List<VertexPositionColor[]> straightLineVertex = new List<VertexPositionColor[]>()
    {
      new VertexPositionColor[2],
      new VertexPositionColor[2],
      new VertexPositionColor[2],
      new VertexPositionColor[2]
    };
    private static readonly List<NPC> _allCharacters = new List<NPC>();
    public static readonly Vector2[] DirectionsTileVectors = new Vector2[4]
    {
      new Vector2(0.0f, -1f),
      new Vector2(1f, 0.0f),
      new Vector2(0.0f, 1f),
      new Vector2(-1f, 0.0f)
    };

    public static Microsoft.Xna.Framework.Rectangle controllerMapSourceRect(Microsoft.Xna.Framework.Rectangle xboxSourceRect)
    {
      return xboxSourceRect;
    }

    public static char getRandomSlotCharacter()
    {
      return Utility.getRandomSlotCharacter('o');
    }

    public static List<Vector2> removeDuplicates(List<Vector2> list)
    {
      for (int index1 = 0; index1 < list.Count; ++index1)
      {
        for (int index2 = list.Count - 1; index2 >= 0; --index2)
        {
          if (index2 != index1 && list[index1].Equals(list[index2]))
            list.RemoveAt(index2);
        }
      }
      return list;
    }

    public static Point Vector2ToPoint(Vector2 v)
    {
      return new Point((int) v.X, (int) v.Y);
    }

    public static Vector2 PointToVector2(Point p)
    {
      return new Vector2((float) p.X, (float) p.Y);
    }

    public static int getStartTimeOfFestival()
    {
      if (Game1.weatherIcon != 1)
        return -1;
      return Convert.ToInt32(Game1.temporaryContent.Load<Dictionary<string, string>>("Data\\Festivals\\" + Game1.currentSeason + (object) Game1.dayOfMonth)["conditions"].Split('/')[1].Split(' ')[0]);
    }

    public static bool isFestivalDay(int day, string season)
    {
      return Game1.temporaryContent.Load<Dictionary<string, string>>("Data\\Festivals\\FestivalDates").ContainsKey(season + (object) day);
    }

    public static bool isObjectOffLimitsForSale(int index)
    {
      switch (index)
      {
        case 680:
        case 681:
        case 682:
        case 688:
        case 689:
        case 690:
        case 774:
        case 775:
        case 454:
        case 460:
        case 645:
        case 413:
        case 437:
        case 439:
        case 158:
        case 159:
        case 160:
        case 161:
        case 162:
        case 163:
        case 326:
        case 341:
          return true;
        default:
          return false;
      }
    }

    public static Microsoft.Xna.Framework.Rectangle xTileToMicrosoftRectangle(xTile.Dimensions.Rectangle rect)
    {
      return new Microsoft.Xna.Framework.Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
    }

    public static Microsoft.Xna.Framework.Rectangle getSafeArea()
    {
      Microsoft.Xna.Framework.Rectangle titleSafeArea = Game1.game1.GraphicsDevice.Viewport.TitleSafeArea;
      if (Game1.game1.GraphicsDevice.GetRenderTargets().Length == 0)
      {
        titleSafeArea.X = (int) ((double) titleSafeArea.X * (1.0 / (double) Game1.options.zoomLevel));
        titleSafeArea.Y = (int) ((double) titleSafeArea.Y * (1.0 / (double) Game1.options.zoomLevel));
        titleSafeArea.Width = (int) ((double) titleSafeArea.Width * (1.0 / (double) Game1.options.zoomLevel));
        titleSafeArea.Height = (int) ((double) titleSafeArea.Height * (1.0 / (double) Game1.options.zoomLevel));
      }
      return titleSafeArea;
    }

    public static Vector2 makeSafe(Vector2 renderPos, Vector2 renderSize)
    {
      Utility.getSafeArea();
      int x1 = (int) renderPos.X;
      int y1 = (int) renderPos.Y;
      int x2 = (int) renderSize.X;
      int y2 = (int) renderSize.Y;
      Utility.makeSafe(ref x1, ref y1, x2, y2);
      return new Vector2((float) x1, (float) y1);
    }

    public static void makeSafe(ref Vector2 position, int width, int height)
    {
      int x = (int) position.X;
      int y = (int) position.Y;
      Utility.makeSafe(ref x, ref y, width, height);
      position.X = (float) x;
      position.Y = (float) y;
    }

    public static void makeSafe(ref Microsoft.Xna.Framework.Rectangle bounds)
    {
      Utility.makeSafe(ref bounds.X, ref bounds.Y, bounds.Width, bounds.Height);
    }

    public static void makeSafe(ref int x, ref int y, int width, int height)
    {
      Microsoft.Xna.Framework.Rectangle safeArea = Utility.getSafeArea();
      if (x < safeArea.Left)
        x = safeArea.Left;
      if (y < safeArea.Top)
        y = safeArea.Top;
      if (x + width > safeArea.Right)
        x = safeArea.Right - width;
      if (y + height <= safeArea.Bottom)
        return;
      y = safeArea.Bottom - height;
    }

    internal static void makeSafeY(ref int y, int height)
    {
      Vector2 vector2 = Utility.makeSafe(new Vector2(0.0f, (float) y), new Vector2(0.0f, (float) height));
      y = (int) vector2.Y;
    }

    public static int makeSafeMarginX(int marginx)
    {
      Viewport viewport = Game1.game1.GraphicsDevice.Viewport;
      Microsoft.Xna.Framework.Rectangle safeArea = Utility.getSafeArea();
      if (safeArea.Left > viewport.Bounds.Left)
        marginx = safeArea.Left;
      int num = safeArea.Right - viewport.Bounds.Right;
      if (num > marginx)
        marginx = num;
      return marginx;
    }

    public static int makeSafeMarginY(int marginy)
    {
      Viewport viewport = Game1.game1.GraphicsDevice.Viewport;
      Microsoft.Xna.Framework.Rectangle safeArea = Utility.getSafeArea();
      int num1 = safeArea.Top - viewport.Bounds.Top;
      if (num1 > marginy)
        marginy = num1;
      int num2 = viewport.Bounds.Bottom - safeArea.Bottom;
      if (num2 > marginy)
        marginy = num2;
      return marginy;
    }

    public static Dictionary<Item, int[]> getTravelingMerchantStock()
    {
      Dictionary<Item, int[]> dictionary = new Dictionary<Item, int[]>();
      Random r = new Random((int) ((long) Game1.uniqueIDForThisGame + (long) Game1.stats.DaysPlayed));
      for (int index1 = 0; index1 < 10; ++index1)
      {
        int index2 = r.Next(2, 790);
        string[] strArray;
        do
        {
          do
          {
            index2 = (index2 + 1) % 790;
          }
          while (!Game1.objectInformation.ContainsKey(index2) || Utility.isObjectOffLimitsForSale(index2));
          strArray = Game1.objectInformation[index2].Split('/');
        }
        while (!strArray[3].Contains<char>('-') || Convert.ToInt32(strArray[1]) <= 0 || (strArray[3].Contains("-13") || strArray[3].Equals("Quest")) || (strArray[0].Equals("Weeds") || strArray[3].Contains("Minerals") || strArray[3].Contains("Arch")));
        dictionary.Add((Item) new Object(index2, 1, false, -1, 0), new int[2]
        {
          Math.Max(r.Next(1, 11) * 100, Convert.ToInt32(strArray[1]) * r.Next(3, 6)),
          r.NextDouble() < 0.1 ? 5 : 1
        });
      }
      dictionary.Add((Item) Utility.getRandomFurniture(r, (List<Item>) null, 0, 1613), new int[2]
      {
        r.Next(1, 11) * 250,
        1
      });
      if (Utility.getSeasonNumber(Game1.currentSeason) < 2)
        dictionary.Add((Item) new Object(347, 1, false, -1, 0), new int[2]
        {
          1000,
          r.NextDouble() < 0.1 ? 5 : 1
        });
      else if (r.NextDouble() < 0.4)
        dictionary.Add((Item) new Object(Vector2.Zero, 136, false), new int[2]
        {
          4000,
          1
        });
      if (r.NextDouble() < 0.25)
        dictionary.Add((Item) new Object(433, 1, false, -1, 0), new int[2]
        {
          2500,
          1
        });
      return dictionary;
    }

    public static Dictionary<Item, int[]> getDwarfShopStock()
    {
      Dictionary<Item, int[]> dictionary = new Dictionary<Item, int[]>();
      dictionary.Add((Item) new Object(773, 1, false, -1, 0), new int[2]
      {
        2000,
        int.MaxValue
      });
      dictionary.Add((Item) new Object(772, 1, false, -1, 0), new int[2]
      {
        3000,
        int.MaxValue
      });
      dictionary.Add((Item) new Object(286, 1, false, -1, 0), new int[2]
      {
        300,
        int.MaxValue
      });
      dictionary.Add((Item) new Object(287, 1, false, -1, 0), new int[2]
      {
        600,
        int.MaxValue
      });
      dictionary.Add((Item) new Object(288, 1, false, -1, 0), new int[2]
      {
        1000,
        int.MaxValue
      });
      dictionary.Add((Item) new Object(243, 1, false, -1, 0), new int[2]
      {
        1000,
        int.MaxValue
      });
      dictionary.Add((Item) new Object(Vector2.Zero, 138, false), new int[2]
      {
        2500,
        int.MaxValue
      });
      if (!Game1.player.craftingRecipes.ContainsKey("Weathered Floor"))
        dictionary.Add((Item) new Object(331, 1, true, -1, 0), new int[2]
        {
          500,
          1
        });
      return dictionary;
    }

    public static Dictionary<Item, int[]> getHospitalStock()
    {
      return new Dictionary<Item, int[]>()
      {
        {
          (Item) new Object(349, 1, false, -1, 0),
          new int[2]{ 1000, int.MaxValue }
        },
        {
          (Item) new Object(351, 1, false, -1, 0),
          new int[2]{ 1000, int.MaxValue }
        }
      };
    }

    public static bool hasFarmerShippedAllItems()
    {
      int num1 = 0;
      int num2 = 0;
      foreach (KeyValuePair<int, string> keyValuePair in Game1.objectInformation)
      {
        string str = keyValuePair.Value.Split('/')[3];
        if (!str.Contains("Arch") && !str.Contains("Fish") && (!str.Contains("Mineral") && !str.Substring(str.Length - 3).Equals("-2")) && (!str.Contains("Cooking") && !str.Substring(str.Length - 3).Equals("-7") && Object.isPotentialBasicShippedCategory(keyValuePair.Key, str.Substring(str.Length - 3))))
        {
          ++num2;
          if (Game1.player.basicShipped.ContainsKey(keyValuePair.Key))
            ++num1;
        }
      }
      return num2 == num1;
    }

    public static Dictionary<Item, int[]> getQiShopStock()
    {
      return new Dictionary<Item, int[]>()
      {
        {
          (Item) new Furniture(1552, Vector2.Zero),
          new int[2]{ 5000, int.MaxValue }
        },
        {
          (Item) new Furniture(1545, Vector2.Zero),
          new int[2]{ 4000, int.MaxValue }
        },
        {
          (Item) new Furniture(1563, Vector2.Zero),
          new int[2]{ 4000, int.MaxValue }
        },
        {
          (Item) new Furniture(1561, Vector2.Zero),
          new int[2]{ 3000, int.MaxValue }
        },
        {
          (Item) new Hat(2),
          new int[2]{ 8000, int.MaxValue }
        },
        {
          (Item) new Object(Vector2.Zero, 126, false),
          new int[2]{ 10000, int.MaxValue }
        },
        {
          (Item) new Object(298, 1, false, -1, 0),
          new int[2]{ 100, int.MaxValue }
        },
        {
          (Item) new Object(703, 1, false, -1, 0),
          new int[2]{ 1000, int.MaxValue }
        },
        {
          (Item) new Object(688, 1, false, -1, 0),
          new int[2]{ 500, int.MaxValue }
        }
      };
    }

    public static Dictionary<Item, int[]> getJojaStock()
    {
      Dictionary<Item, int[]> dictionary1 = new Dictionary<Item, int[]>();
      dictionary1.Add((Item) new Object(Vector2.Zero, 167, int.MaxValue), new int[2]
      {
        75,
        int.MaxValue
      });
      Dictionary<Item, int[]> dictionary2 = dictionary1;
      Wallpaper wallpaper1 = new Wallpaper(21, false);
      int maxValue1 = int.MaxValue;
      wallpaper1.stack = maxValue1;
      int[] numArray1 = new int[2]{ 20, int.MaxValue };
      dictionary2.Add((Item) wallpaper1, numArray1);
      Dictionary<Item, int[]> dictionary3 = dictionary1;
      Furniture furniture = new Furniture(1609, Vector2.Zero);
      int maxValue2 = int.MaxValue;
      furniture.stack = maxValue2;
      int[] numArray2 = new int[2]{ 500, int.MaxValue };
      dictionary3.Add((Item) furniture, numArray2);
      float num = Game1.player.hasOrWillReceiveMail("JojaMember") ? 2f : 2.5f;
      if (Game1.currentSeason.Equals("spring"))
      {
        dictionary1.Add((Item) new Object(Vector2.Zero, 472, int.MaxValue), new int[2]
        {
          (int) ((double) Convert.ToInt32(Game1.objectInformation[472].Split('/')[1]) * (double) num),
          int.MaxValue
        });
        dictionary1.Add((Item) new Object(Vector2.Zero, 473, int.MaxValue), new int[2]
        {
          (int) ((double) Convert.ToInt32(Game1.objectInformation[473].Split('/')[1]) * (double) num),
          int.MaxValue
        });
        dictionary1.Add((Item) new Object(Vector2.Zero, 474, int.MaxValue), new int[2]
        {
          (int) ((double) Convert.ToInt32(Game1.objectInformation[474].Split('/')[1]) * (double) num),
          int.MaxValue
        });
        dictionary1.Add((Item) new Object(Vector2.Zero, 475, int.MaxValue), new int[2]
        {
          (int) ((double) Convert.ToInt32(Game1.objectInformation[475].Split('/')[1]) * (double) num),
          int.MaxValue
        });
        dictionary1.Add((Item) new Object(Vector2.Zero, 427, int.MaxValue), new int[2]
        {
          (int) ((double) Convert.ToInt32(Game1.objectInformation[427].Split('/')[1]) * (double) num),
          int.MaxValue
        });
        dictionary1.Add((Item) new Object(Vector2.Zero, 429, int.MaxValue), new int[2]
        {
          (int) ((double) Convert.ToInt32(Game1.objectInformation[429].Split('/')[1]) * (double) num),
          int.MaxValue
        });
        dictionary1.Add((Item) new Object(Vector2.Zero, 477, int.MaxValue), new int[2]
        {
          (int) ((double) Convert.ToInt32(Game1.objectInformation[477].Split('/')[1]) * (double) num),
          int.MaxValue
        });
      }
      if (Game1.currentSeason.Equals("summer"))
      {
        dictionary1.Add((Item) new Object(Vector2.Zero, 480, int.MaxValue), new int[2]
        {
          (int) ((double) Convert.ToInt32(Game1.objectInformation[480].Split('/')[1]) * (double) num),
          int.MaxValue
        });
        dictionary1.Add((Item) new Object(Vector2.Zero, 482, int.MaxValue), new int[2]
        {
          (int) ((double) Convert.ToInt32(Game1.objectInformation[482].Split('/')[1]) * (double) num),
          int.MaxValue
        });
        dictionary1.Add((Item) new Object(Vector2.Zero, 483, int.MaxValue), new int[2]
        {
          (int) ((double) Convert.ToInt32(Game1.objectInformation[483].Split('/')[1]) * (double) num),
          int.MaxValue
        });
        dictionary1.Add((Item) new Object(Vector2.Zero, 484, int.MaxValue), new int[2]
        {
          (int) ((double) Convert.ToInt32(Game1.objectInformation[484].Split('/')[1]) * (double) num),
          int.MaxValue
        });
        dictionary1.Add((Item) new Object(Vector2.Zero, 479, int.MaxValue), new int[2]
        {
          (int) ((double) Convert.ToInt32(Game1.objectInformation[479].Split('/')[1]) * (double) num),
          int.MaxValue
        });
        dictionary1.Add((Item) new Object(Vector2.Zero, 302, int.MaxValue), new int[2]
        {
          (int) ((double) Convert.ToInt32(Game1.objectInformation[302].Split('/')[1]) * (double) num),
          int.MaxValue
        });
        dictionary1.Add((Item) new Object(Vector2.Zero, 453, int.MaxValue), new int[2]
        {
          (int) ((double) Convert.ToInt32(Game1.objectInformation[453].Split('/')[1]) * (double) num),
          int.MaxValue
        });
        dictionary1.Add((Item) new Object(Vector2.Zero, 455, int.MaxValue), new int[2]
        {
          (int) ((double) Convert.ToInt32(Game1.objectInformation[455].Split('/')[1]) * (double) num),
          int.MaxValue
        });
        dictionary1.Add((Item) new Object(431, int.MaxValue, false, 100, 0), new int[2]
        {
          (int) (50.0 * (double) num),
          int.MaxValue
        });
      }
      if (Game1.currentSeason.Equals("fall"))
      {
        dictionary1.Add((Item) new Object(Vector2.Zero, 487, int.MaxValue), new int[2]
        {
          (int) ((double) Convert.ToInt32(Game1.objectInformation[487].Split('/')[1]) * (double) num),
          int.MaxValue
        });
        dictionary1.Add((Item) new Object(Vector2.Zero, 488, int.MaxValue), new int[2]
        {
          (int) ((double) Convert.ToInt32(Game1.objectInformation[488].Split('/')[1]) * (double) num),
          int.MaxValue
        });
        dictionary1.Add((Item) new Object(Vector2.Zero, 483, int.MaxValue), new int[2]
        {
          (int) ((double) Convert.ToInt32(Game1.objectInformation[483].Split('/')[1]) * (double) num),
          int.MaxValue
        });
        dictionary1.Add((Item) new Object(Vector2.Zero, 490, int.MaxValue), new int[2]
        {
          (int) ((double) Convert.ToInt32(Game1.objectInformation[490].Split('/')[1]) * (double) num),
          int.MaxValue
        });
        dictionary1.Add((Item) new Object(Vector2.Zero, 299, int.MaxValue), new int[2]
        {
          (int) ((double) Convert.ToInt32(Game1.objectInformation[299].Split('/')[1]) * (double) num),
          int.MaxValue
        });
        dictionary1.Add((Item) new Object(Vector2.Zero, 301, int.MaxValue), new int[2]
        {
          (int) ((double) Convert.ToInt32(Game1.objectInformation[301].Split('/')[1]) * (double) num),
          int.MaxValue
        });
        dictionary1.Add((Item) new Object(Vector2.Zero, 492, int.MaxValue), new int[2]
        {
          (int) ((double) Convert.ToInt32(Game1.objectInformation[492].Split('/')[1]) * (double) num),
          int.MaxValue
        });
        dictionary1.Add((Item) new Object(Vector2.Zero, 491, int.MaxValue), new int[2]
        {
          (int) ((double) Convert.ToInt32(Game1.objectInformation[491].Split('/')[1]) * (double) num),
          int.MaxValue
        });
        dictionary1.Add((Item) new Object(Vector2.Zero, 493, int.MaxValue), new int[2]
        {
          (int) ((double) Convert.ToInt32(Game1.objectInformation[493].Split('/')[1]) * (double) num),
          int.MaxValue
        });
        dictionary1.Add((Item) new Object(431, int.MaxValue, false, 100, 0), new int[2]
        {
          (int) (50.0 * (double) num),
          int.MaxValue
        });
        dictionary1.Add((Item) new Object(Vector2.Zero, 425, int.MaxValue), new int[2]
        {
          (int) ((double) Convert.ToInt32(Game1.objectInformation[425].Split('/')[1]) * (double) num),
          int.MaxValue
        });
      }
      dictionary1.Add((Item) new Object(Vector2.Zero, 297, int.MaxValue), new int[2]
      {
        (int) ((double) Convert.ToInt32(Game1.objectInformation[297].Split('/')[1]) * (double) num),
        int.MaxValue
      });
      dictionary1.Add((Item) new Object(Vector2.Zero, 245, int.MaxValue), new int[2]
      {
        (int) ((double) Convert.ToInt32(Game1.objectInformation[245].Split('/')[1]) * (double) num),
        int.MaxValue
      });
      dictionary1.Add((Item) new Object(Vector2.Zero, 246, int.MaxValue), new int[2]
      {
        (int) ((double) Convert.ToInt32(Game1.objectInformation[246].Split('/')[1]) * (double) num),
        int.MaxValue
      });
      dictionary1.Add((Item) new Object(Vector2.Zero, 423, int.MaxValue), new int[2]
      {
        (int) ((double) Convert.ToInt32(Game1.objectInformation[423].Split('/')[1]) * (double) num),
        int.MaxValue
      });
      Random random = new Random((int) Game1.stats.DaysPlayed + (int) Game1.uniqueIDForThisGame / 2 + 1);
      int which = random.Next(112);
      if (which == 21)
        which = 22;
      Dictionary<Item, int[]> dictionary4 = dictionary1;
      Wallpaper wallpaper2 = new Wallpaper(which, false);
      int maxValue3 = int.MaxValue;
      wallpaper2.stack = maxValue3;
      int[] numArray3 = new int[2]{ 250, int.MaxValue };
      dictionary4.Add((Item) wallpaper2, numArray3);
      Dictionary<Item, int[]> dictionary5 = dictionary1;
      Wallpaper wallpaper3 = new Wallpaper(random.Next(40), true);
      int maxValue4 = int.MaxValue;
      wallpaper3.stack = maxValue4;
      int[] numArray4 = new int[2]{ 250, int.MaxValue };
      dictionary5.Add((Item) wallpaper3, numArray4);
      return dictionary1;
    }

    public static Dictionary<Item, int[]> getHatStock()
    {
      Dictionary<Item, int[]> dictionary = new Dictionary<Item, int[]>();
      foreach (KeyValuePair<int, string> keyValuePair in Game1.content.Load<Dictionary<int, string>>("Data\\Achievements"))
      {
        if (Game1.player.achievements.Contains(keyValuePair.Key))
          dictionary.Add((Item) new Hat(Convert.ToInt32(keyValuePair.Value.Split('^')[4])), new int[2]
          {
            1000,
            int.MaxValue
          });
      }
      return dictionary;
    }

    public static NPC getTodaysBirthdayNPC(string season, int day)
    {
      foreach (NPC allCharacter in Utility.getAllCharacters())
      {
        if (allCharacter.isBirthday(season, day))
          return allCharacter;
      }
      return (NPC) null;
    }

    public static bool highlightEdibleItems(Item i)
    {
      if (i is Object)
        return (i as Object).edibility != -300;
      return false;
    }

    public static int getRandomSingleTileFurniture(Random r)
    {
      switch (r.Next(3))
      {
        case 0:
          return r.Next(10) * 3;
        case 1:
          return r.Next(1376, 1391);
        case 2:
          return r.Next(7) * 2 + 1391;
        default:
          return 0;
      }
    }

    public static void improveFriendshipWithEveryoneInRegion(Farmer who, int amount, int region)
    {
      foreach (GameLocation location in Game1.locations)
      {
        foreach (NPC character in location.characters)
        {
          if (character.homeRegion == region && who.friendships.ContainsKey(character.name))
            who.changeFriendship(amount, character);
        }
      }
    }

    public static Item getGiftFromNPC(NPC who)
    {
      Random random = new Random((int) Game1.uniqueIDForThisGame / 2 + Game1.year + Game1.dayOfMonth + Utility.getSeasonNumber(Game1.currentSeason) + who.getTileX());
      List<Item> objList = new List<Item>();
      string name = who.name;
      if (!(name == "Clint"))
      {
        if (!(name == "Marnie"))
        {
          if (!(name == "Robin"))
          {
            if (!(name == "Willy"))
            {
              if (name == "Evelyn")
                objList.Add((Item) new Object(223, 1, false, -1, 0));
              else if (who.age == 2)
              {
                objList.Add((Item) new Object(330, 1, false, -1, 0));
                objList.Add((Item) new Object(103, 1, false, -1, 0));
                objList.Add((Item) new Object(394, 1, false, -1, 0));
                objList.Add((Item) new Object(random.Next(535, 538), 1, false, -1, 0));
              }
              else
              {
                objList.Add((Item) new Object(608, 1, false, -1, 0));
                objList.Add((Item) new Object(651, 1, false, -1, 0));
                objList.Add((Item) new Object(611, 1, false, -1, 0));
                objList.Add((Item) new Ring(517));
                objList.Add((Item) new Object(466, 10, false, -1, 0));
                objList.Add((Item) new Object(422, 1, false, -1, 0));
                objList.Add((Item) new Object(392, 1, false, -1, 0));
                objList.Add((Item) new Object(348, 1, false, -1, 0));
                objList.Add((Item) new Object(346, 1, false, -1, 0));
                objList.Add((Item) new Object(341, 1, false, -1, 0));
                objList.Add((Item) new Object(221, 1, false, -1, 0));
                objList.Add((Item) new Object(64, 1, false, -1, 0));
                objList.Add((Item) new Object(60, 1, false, -1, 0));
                objList.Add((Item) new Object(70, 1, false, -1, 0));
              }
            }
            else
            {
              objList.Add((Item) new Object(690, 25, false, -1, 0));
              objList.Add((Item) new Object(687, 1, false, -1, 0));
              objList.Add((Item) new Object(703, 1, false, -1, 0));
            }
          }
          else
          {
            objList.Add((Item) new Object(388, 99, false, -1, 0));
            objList.Add((Item) new Object(390, 50, false, -1, 0));
            objList.Add((Item) new Object(709, 25, false, -1, 0));
          }
        }
        else
          objList.Add((Item) new Object(176, 12, false, -1, 0));
      }
      else
      {
        objList.Add((Item) new Object(337, 1, false, -1, 0));
        objList.Add((Item) new Object(336, 5, false, -1, 0));
        objList.Add((Item) new Object(random.Next(535, 538), 5, false, -1, 0));
      }
      return objList[random.Next(objList.Count)];
    }

    public static NPC getTopRomanticInterest(Farmer who)
    {
      NPC npc = (NPC) null;
      int num = -1;
      foreach (NPC allCharacter in Utility.getAllCharacters())
      {
        if (who.friendships.ContainsKey(allCharacter.name) && allCharacter.datable && who.getFriendshipLevelForNPC(allCharacter.name) > num)
        {
          npc = allCharacter;
          num = who.getFriendshipLevelForNPC(allCharacter.name);
        }
      }
      return npc;
    }

    public static Color getRandomRainbowColor(Random r = null)
    {
      switch (r == null ? Game1.random.Next(8) : r.Next(8))
      {
        case 0:
          return Color.Red;
        case 1:
          return Color.Orange;
        case 2:
          return Color.Yellow;
        case 3:
          return Color.Lime;
        case 4:
          return Color.Cyan;
        case 5:
          return new Color(0, 100, (int) byte.MaxValue);
        case 6:
          return new Color(152, 96, (int) byte.MaxValue);
        case 7:
          return new Color((int) byte.MaxValue, 100, (int) byte.MaxValue);
        default:
          return Color.White;
      }
    }

    public static NPC getTopNonRomanticInterest(Farmer who)
    {
      NPC npc = (NPC) null;
      int num = -1;
      foreach (NPC allCharacter in Utility.getAllCharacters())
      {
        if (who.friendships.ContainsKey(allCharacter.name) && !allCharacter.datable && who.getFriendshipLevelForNPC(allCharacter.name) > num)
        {
          npc = allCharacter;
          num = who.getFriendshipLevelForNPC(allCharacter.name);
        }
      }
      return npc;
    }

    public static int getHighestSkill(Farmer who)
    {
      int num1 = 0;
      int num2 = 0;
      for (int index = 0; index < who.experiencePoints.Length; ++index)
      {
        if (who.experiencePoints[index] > num1)
          num2 = index;
      }
      return num2;
    }

    public static int getNumberOfFriendsWithinThisRange(Farmer who, int minFriendshipPoints, int maxFriendshipPoints, bool romanceOnly = false)
    {
      int num = 0;
      foreach (NPC allCharacter in Utility.getAllCharacters())
      {
        int? friendshipLevelForNpc = who.tryGetFriendshipLevelForNPC(allCharacter.name);
        if (friendshipLevelForNpc.HasValue && friendshipLevelForNpc.Value >= minFriendshipPoints && friendshipLevelForNpc.Value <= maxFriendshipPoints && (!romanceOnly || allCharacter.datable))
          ++num;
      }
      return num;
    }

    public static bool highlightEdibleNonCookingItems(Item i)
    {
      if (i is Object && (i as Object).edibility != -300)
        return (i as Object).category != -7;
      return false;
    }

    public static bool highlightSmallObjects(Item i)
    {
      if (i is Object)
        return !(i as Object).bigCraftable;
      return false;
    }

    public static bool highlightShippableObjects(Item i)
    {
      if (i is Object)
        return (i as Object).canBeShipped();
      return false;
    }

    public static Farmer getFarmerFromFarmerNumberString(string s)
    {
      if (s.Equals("farmer"))
        return Game1.player;
      return Utility.getFarmerFromFarmerNumber(Convert.ToInt32(s[s.Length - 1].ToString() ?? ""));
    }

    public static int getFarmerNumberFromFarmer(Farmer who)
    {
      for (int number = 1; number <= 4; ++number)
      {
        if (Utility.getFarmerFromFarmerNumber(number).Equals((object) who))
          return number;
      }
      return -1;
    }

    public static Farmer getFarmerFromFarmerNumber(int number)
    {
      if (!Game1.IsMultiplayer)
      {
        if (number == 1)
          return Game1.player;
        return (Farmer) null;
      }
      if (number == 1 && Game1.serverHost != null)
        return Game1.serverHost;
      if (number > Game1.numberOfPlayers())
        return (Farmer) null;
      long[] numArray = new long[Game1.numberOfPlayers() - 1];
      int index1 = 0;
      for (int index2 = 0; index2 < Game1.otherFarmers.Count; ++index2)
      {
        if (Game1.otherFarmers.Values.ElementAt<Farmer>(index2).uniqueMultiplayerID != Game1.serverHost.uniqueMultiplayerID)
        {
          numArray[index1] = Game1.otherFarmers.Values.ElementAt<Farmer>(index2).uniqueMultiplayerID;
          ++index1;
        }
      }
      return Game1.otherFarmers[numArray[number - 2]];
    }

    public static string getLoveInterest(string who)
    {
      // ISSUE: reference to a compiler-generated method
      uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(who);
      if (stringHash <= 1866496948U)
      {
        if (stringHash <= 1067922812U)
        {
          if ((int) stringHash != 161540545)
          {
            if ((int) stringHash != 587846041)
            {
              if ((int) stringHash == 1067922812 && who == "Sam")
                return "Penny";
            }
            else if (who == "Penny")
              return "Sam";
          }
          else if (who == "Sebastian")
            return "Abigail";
        }
        else if ((int) stringHash != 1281010426)
        {
          if ((int) stringHash != 1708213605)
          {
            if ((int) stringHash == 1866496948 && who == "Shane")
              return "Emily";
          }
          else if (who == "Alex")
            return "Haley";
        }
        else if (who == "Maru")
          return "Harvey";
      }
      else if (stringHash <= 2571828641U)
      {
        if ((int) stringHash != 2010304804)
        {
          if ((int) stringHash != -1860673204)
          {
            if ((int) stringHash == -1723138655 && who == "Emily")
              return "Shane";
          }
          else if (who == "Haley")
            return "Alex";
        }
        else if (who == "Harvey")
          return "Maru";
      }
      else if ((int) stringHash != -1562053956)
      {
        if ((int) stringHash != -1468719973)
        {
          if ((int) stringHash == -1228790996 && who == "Elliott")
            return "Leah";
        }
        else if (who == "Leah")
          return "Elliott";
      }
      else if (who == "Abigail")
        return "Sebastian";
      return "";
    }

    public static Dictionary<Item, int[]> getFishShopStock(Farmer who)
    {
      Dictionary<Item, int[]> dictionary = new Dictionary<Item, int[]>();
      dictionary.Add((Item) new Object(219, 1, false, -1, 0), new int[2]
      {
        250,
        int.MaxValue
      });
      if (Game1.player.fishingLevel >= 2)
        dictionary.Add((Item) new Object(685, 1, false, -1, 0), new int[2]
        {
          5,
          int.MaxValue
        });
      if (Game1.player.fishingLevel >= 3)
        dictionary.Add((Item) new Object(710, 1, false, -1, 0), new int[2]
        {
          1500,
          int.MaxValue
        });
      if (Game1.player.fishingLevel >= 6)
      {
        dictionary.Add((Item) new Object(686, 1, false, -1, 0), new int[2]
        {
          500,
          int.MaxValue
        });
        dictionary.Add((Item) new Object(694, 1, false, -1, 0), new int[2]
        {
          500,
          int.MaxValue
        });
        dictionary.Add((Item) new Object(692, 1, false, -1, 0), new int[2]
        {
          200,
          int.MaxValue
        });
      }
      if (Game1.player.fishingLevel >= 7)
      {
        dictionary.Add((Item) new Object(693, 1, false, -1, 0), new int[2]
        {
          750,
          int.MaxValue
        });
        dictionary.Add((Item) new Object(695, 1, false, -1, 0), new int[2]
        {
          750,
          int.MaxValue
        });
      }
      if (Game1.player.fishingLevel >= 8)
      {
        dictionary.Add((Item) new Object(691, 1, false, -1, 0), new int[2]
        {
          1000,
          int.MaxValue
        });
        dictionary.Add((Item) new Object(687, 1, false, -1, 0), new int[2]
        {
          1000,
          int.MaxValue
        });
      }
      if (Game1.player.fishingLevel >= 9)
        dictionary.Add((Item) new Object(703, 1, false, -1, 0), new int[2]
        {
          1000,
          int.MaxValue
        });
      dictionary.Add((Item) new FishingRod(0), new int[2]
      {
        500,
        int.MaxValue
      });
      if (Game1.player.fishingLevel >= 2)
        dictionary.Add((Item) new FishingRod(2), new int[2]
        {
          1800,
          int.MaxValue
        });
      if (Game1.player.fishingLevel >= 6)
        dictionary.Add((Item) new FishingRod(3), new int[2]
        {
          7500,
          int.MaxValue
        });
      return dictionary;
    }

    public static void Shuffle<T>(Random rng, T[] array)
    {
      int length = array.Length;
      while (length > 1)
      {
        int index = rng.Next(length--);
        T obj = array[length];
        array[length] = array[index];
        array[index] = obj;
      }
    }

    public static int getSeasonNumber(string whichSeason)
    {
      string lower = whichSeason.ToLower();
      if (lower == "spring")
        return 0;
      if (lower == "summer")
        return 1;
      if (lower == "autumn" || lower == "fall")
        return 2;
      return lower == "winter" ? 3 : -1;
    }

    public static char getRandomSlotCharacter(char current)
    {
      char ch = 'o';
      while ((int) ch == 111 || (int) ch == (int) current)
      {
        switch (Game1.random.Next(8))
        {
          case 0:
            ch = '=';
            continue;
          case 1:
            ch = '\\';
            continue;
          case 2:
            ch = ']';
            continue;
          case 3:
            ch = '[';
            continue;
          case 4:
            ch = '<';
            continue;
          case 5:
            ch = '*';
            continue;
          case 6:
            ch = '$';
            continue;
          case 7:
            ch = '}';
            continue;
          default:
            continue;
        }
      }
      return ch;
    }

    public static List<Vector2> getPositionsInClusterAroundThisTile(Vector2 startTile, int number)
    {
      Queue<Vector2> vector2Queue = new Queue<Vector2>();
      List<Vector2> vector2List = new List<Vector2>();
      Vector2 vector2_1 = startTile;
      vector2Queue.Enqueue(vector2_1);
      while (vector2List.Count < number)
      {
        Vector2 vector2_2 = vector2Queue.Dequeue();
        vector2List.Add(vector2_2);
        if (!vector2List.Contains(new Vector2(vector2_2.X + 1f, vector2_2.Y)))
          vector2Queue.Enqueue(new Vector2(vector2_2.X + 1f, vector2_2.Y));
        if (!vector2List.Contains(new Vector2(vector2_2.X - 1f, vector2_2.Y)))
          vector2Queue.Enqueue(new Vector2(vector2_2.X - 1f, vector2_2.Y));
        if (!vector2List.Contains(new Vector2(vector2_2.X, vector2_2.Y + 1f)))
          vector2Queue.Enqueue(new Vector2(vector2_2.X, vector2_2.Y + 1f));
        if (!vector2List.Contains(new Vector2(vector2_2.X, vector2_2.Y - 1f)))
          vector2Queue.Enqueue(new Vector2(vector2_2.X, vector2_2.Y - 1f));
      }
      return vector2List;
    }

    public static bool doesPointHaveLineOfSightInMine(Vector2 start, Vector2 end, int visionDistance)
    {
      if ((double) Vector2.Distance(start, end) > (double) visionDistance)
        return false;
      foreach (Point p in Utility.GetPointsOnLine((int) start.X, (int) start.Y, (int) end.X, (int) end.Y))
      {
        if (Game1.mine.getTileIndexAt(p, "Buildings") != -1)
          return false;
      }
      return true;
    }

    public static void addSprinklesToLocation(GameLocation l, int sourceXTile, int sourceYTile, int tilesWide, int tilesHigh, int totalSprinkleDuration, int millisecondsBetweenSprinkles, Color sprinkleColor, string sound = null, bool motionTowardCenter = false)
    {
      Microsoft.Xna.Framework.Rectangle r = new Microsoft.Xna.Framework.Rectangle(sourceXTile - tilesWide / 2, sourceYTile - tilesHigh / 2, tilesWide, tilesHigh);
      Random random = new Random();
      int num1 = totalSprinkleDuration / millisecondsBetweenSprinkles;
      for (int index = 0; index < num1; ++index)
      {
        Vector2 vector2_1 = Utility.getRandomPositionInThisRectangle(r, random) * (float) Game1.tileSize;
        List<TemporaryAnimatedSprite> temporarySprites = l.temporarySprites;
        TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(random.Next(10, 12), vector2_1, sprinkleColor, 8, false, 50f, 0, -1, -1f, -1, 0);
        temporaryAnimatedSprite.layerDepth = 1f;
        int num2 = millisecondsBetweenSprinkles * index;
        temporaryAnimatedSprite.delayBeforeAnimationStart = num2;
        double num3 = 100.0;
        temporaryAnimatedSprite.interval = (float) num3;
        string str = sound;
        temporaryAnimatedSprite.startSound = str;
        Vector2 vector2_2 = motionTowardCenter ? Utility.getVelocityTowardPoint(vector2_1, new Vector2((float) sourceXTile, (float) sourceYTile) * (float) Game1.tileSize, Vector2.Distance(new Vector2((float) sourceXTile, (float) sourceYTile) * (float) Game1.tileSize, vector2_1) / 64f) : Vector2.Zero;
        temporaryAnimatedSprite.motion = vector2_2;
        int num4 = sourceXTile;
        temporaryAnimatedSprite.xStopCoordinate = num4;
        int num5 = sourceYTile;
        temporaryAnimatedSprite.yStopCoordinate = num5;
        temporarySprites.Add(temporaryAnimatedSprite);
      }
    }

    public static void addStarsAndSpirals(GameLocation l, int sourceXTile, int sourceYTile, int tilesWide, int tilesHigh, int totalSprinkleDuration, int millisecondsBetweenSprinkles, Color sprinkleColor, string sound = null, bool motionTowardCenter = false)
    {
      Microsoft.Xna.Framework.Rectangle r = new Microsoft.Xna.Framework.Rectangle(sourceXTile - tilesWide / 2, sourceYTile - tilesHigh / 2, tilesWide, tilesHigh);
      Random random = new Random();
      int num1 = totalSprinkleDuration / millisecondsBetweenSprinkles;
      for (int index = 0; index < num1; ++index)
      {
        Vector2 position = Utility.getRandomPositionInThisRectangle(r, random) * (float) Game1.tileSize;
        List<TemporaryAnimatedSprite> temporarySprites = l.temporarySprites;
        TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, random.NextDouble() < 0.5 ? new Microsoft.Xna.Framework.Rectangle(359, 1437, 14, 14) : new Microsoft.Xna.Framework.Rectangle(377, 1438, 9, 9), position, false, 0.01f, sprinkleColor);
        temporaryAnimatedSprite.xPeriodic = true;
        double num2 = (double) random.Next(2000, 3000);
        temporaryAnimatedSprite.xPeriodicLoopTime = (float) num2;
        double num3 = (double) random.Next(-Game1.tileSize, Game1.tileSize);
        temporaryAnimatedSprite.xPeriodicRange = (float) num3;
        Vector2 vector2 = new Vector2(0.0f, -2f);
        temporaryAnimatedSprite.motion = vector2;
        double num4 = 3.14159274101257 / (double) random.Next(4, 64);
        temporaryAnimatedSprite.rotationChange = (float) num4;
        int num5 = millisecondsBetweenSprinkles * index;
        temporaryAnimatedSprite.delayBeforeAnimationStart = num5;
        double num6 = 1.0;
        temporaryAnimatedSprite.layerDepth = (float) num6;
        double num7 = 0.0399999991059303;
        temporaryAnimatedSprite.scaleChange = (float) num7;
        double num8 = -0.0007999999797903;
        temporaryAnimatedSprite.scaleChangeChange = (float) num8;
        double num9 = 4.0;
        temporaryAnimatedSprite.scale = (float) num9;
        temporarySprites.Add(temporaryAnimatedSprite);
      }
    }

    public static Vector2 clampToTile(Vector2 nonTileLocation)
    {
      nonTileLocation.X -= nonTileLocation.X % (float) Game1.tileSize;
      nonTileLocation.Y -= nonTileLocation.Y % (float) Game1.tileSize;
      return nonTileLocation;
    }

    public static float distance(float x1, float x2, float y1, float y2)
    {
      return (float) Math.Sqrt(((double) x2 - (double) x1) * ((double) x2 - (double) x1) + ((double) y2 - (double) y1) * ((double) y2 - (double) y1));
    }

    public static void facePlayerEndBehavior(Character c, GameLocation location)
    {
      Character character = c;
      Microsoft.Xna.Framework.Rectangle boundingBox = Game1.player.GetBoundingBox();
      double x = (double) boundingBox.Center.X;
      boundingBox = Game1.player.GetBoundingBox();
      double y = (double) boundingBox.Center.Y;
      Vector2 target = new Vector2((float) x, (float) y);
      int yBias = 0;
      character.faceGeneralDirection(target, yBias);
    }

    public static bool couldSeePlayerInPeripheralVision(Character c)
    {
      switch (c.facingDirection)
      {
        case 0:
          if (Game1.player.GetBoundingBox().Center.Y < c.GetBoundingBox().Center.Y + Game1.tileSize / 2)
            return true;
          break;
        case 1:
          if (Game1.player.GetBoundingBox().Center.X > c.GetBoundingBox().Center.X - Game1.tileSize / 2)
            return true;
          break;
        case 2:
          if (Game1.player.GetBoundingBox().Center.Y > c.GetBoundingBox().Center.Y - Game1.tileSize / 2)
            return true;
          break;
        case 3:
          if (Game1.player.GetBoundingBox().Center.X < c.GetBoundingBox().Center.X + Game1.tileSize / 2)
            return true;
          break;
      }
      return false;
    }

    public static List<Microsoft.Xna.Framework.Rectangle> divideThisRectangleIntoQuarters(Microsoft.Xna.Framework.Rectangle rect)
    {
      return new List<Microsoft.Xna.Framework.Rectangle>()
      {
        new Microsoft.Xna.Framework.Rectangle(rect.X, rect.Y, rect.Width / 2, rect.Height / 2),
        new Microsoft.Xna.Framework.Rectangle(rect.X + rect.Width / 2, rect.Y, rect.Width / 2, rect.Height / 2),
        new Microsoft.Xna.Framework.Rectangle(rect.X, rect.Y + rect.Height / 2, rect.Width / 2, rect.Height / 2),
        new Microsoft.Xna.Framework.Rectangle(rect.X + rect.Width / 2, rect.Y + rect.Height / 2, rect.Width / 2, rect.Height / 2)
      };
    }

    public static Item getUncommonItemForThisMineLevel(int level, Point location)
    {
      Dictionary<int, string> dictionary = Game1.content.Load<Dictionary<int, string>>("Data\\weapons");
      List<int> source = new List<int>();
      int num1 = -1;
      int num2 = -1;
      int num3 = 12;
      Random random = new Random(location.X * 1000 + location.Y + (int) Game1.uniqueIDForThisGame + level);
      foreach (KeyValuePair<int, string> keyValuePair in dictionary)
      {
        if (Game1.mine.mineLevel >= Convert.ToInt32(keyValuePair.Value.Split('/')[10]))
        {
          if (Convert.ToInt32(keyValuePair.Value.Split('/')[9]) != -1)
          {
            int int32 = Convert.ToInt32(keyValuePair.Value.Split('/')[9]);
            if (num1 == -1 || num2 > Math.Abs(Game1.mine.mineLevel - int32))
            {
              num1 = keyValuePair.Key;
              num2 = Convert.ToInt32(keyValuePair.Value.Split('/')[9]);
            }
            double num4 = Math.Pow(Math.E, -Math.Pow((double) (Game1.mine.mineLevel - int32), 2.0) / (double) (2 * (num3 * num3)));
            if (random.NextDouble() < num4)
              source.Add(keyValuePair.Key);
          }
        }
      }
      source.Add(num1);
      return (Item) new MeleeWeapon(source.ElementAt<int>(random.Next(source.Count)));
    }

    public static IEnumerable<Point> GetPointsOnLine(int x0, int y0, int x1, int y1)
    {
      return Utility.GetPointsOnLine(x0, y0, x1, y1, false);
    }

    public static List<Vector2> getBorderOfThisRectangle(Microsoft.Xna.Framework.Rectangle r)
    {
      List<Vector2> vector2List = new List<Vector2>();
      for (int x = r.X; x < r.Right; ++x)
        vector2List.Add(new Vector2((float) x, (float) r.Y));
      for (int index = r.Y + 1; index < r.Bottom; ++index)
        vector2List.Add(new Vector2((float) (r.Right - 1), (float) index));
      for (int index = r.Right - 2; index >= r.X; --index)
        vector2List.Add(new Vector2((float) index, (float) (r.Bottom - 1)));
      for (int index = r.Bottom - 2; index >= r.Y + 1; --index)
        vector2List.Add(new Vector2((float) r.X, (float) index));
      return vector2List;
    }

    public static Point getTranslatedPoint(Point p, int direction, int movementAmount)
    {
      switch (direction)
      {
        case 0:
          return new Point(p.X, p.Y - movementAmount);
        case 1:
          return new Point(p.X + movementAmount, p.Y);
        case 2:
          return new Point(p.X, p.Y + movementAmount);
        case 3:
          return new Point(p.X - movementAmount, p.Y);
        default:
          return p;
      }
    }

    public static Vector2 getTranslatedVector2(Vector2 p, int direction, float movementAmount)
    {
      switch (direction)
      {
        case 0:
          return new Vector2(p.X, p.Y - movementAmount);
        case 1:
          return new Vector2(p.X + movementAmount, p.Y);
        case 2:
          return new Vector2(p.X, p.Y + movementAmount);
        case 3:
          return new Vector2(p.X - movementAmount, p.Y);
        default:
          return p;
      }
    }

    public static IEnumerable<Point> GetPointsOnLine(int x0, int y0, int x1, int y1, bool ignoreSwap)
    {
      bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
      if (steep)
      {
        int num1 = x0;
        x0 = y0;
        y0 = num1;
        int num2 = x1;
        x1 = y1;
        y1 = num2;
      }
      if (!ignoreSwap && x0 > x1)
      {
        int num1 = x0;
        x0 = x1;
        x1 = num1;
        int num2 = y0;
        y0 = y1;
        y1 = num2;
      }
      int dx = x1 - x0;
      int dy = Math.Abs(y1 - y0);
      int error = dx / 2;
      int ystep = y0 < y1 ? 1 : -1;
      int y = y0;
      for (int x = x0; x <= x1; ++x)
      {
        yield return new Point(steep ? y : x, steep ? x : y);
        error -= dy;
        if (error < 0)
        {
          y += ystep;
          error += dx;
        }
      }
    }

    public static Vector2 getRandomAdjacentOpenTile(Vector2 tile)
    {
      List<Vector2> adjacentTileLocations = Utility.getAdjacentTileLocations(tile);
      int num = 0;
      int index = Game1.random.Next(adjacentTileLocations.Count);
      Vector2 tileLocation;
      for (tileLocation = adjacentTileLocations[index]; num < 4 && (Game1.currentLocation.isTileOccupiedForPlacement(tileLocation, (Object) null) || !Game1.currentLocation.isTilePassable(new Location((int) tileLocation.X, (int) tileLocation.Y), Game1.viewport)); ++num)
      {
        index = (index + 1) % adjacentTileLocations.Count;
        tileLocation = adjacentTileLocations[index];
      }
      if (num >= 4)
        return Vector2.Zero;
      return tileLocation;
    }

    public static int getObjectIndexFromSlotCharacter(char character)
    {
      if ((uint) character <= 60U)
      {
        if ((int) character == 36)
          return 398;
        if ((int) character == 42)
          return 176;
        if ((int) character == 60)
          return 400;
      }
      else
      {
        switch (character)
        {
          case '=':
            return 72;
          case '[':
            return 276;
          case '\\':
            return 336;
          case ']':
            return 221;
          case '}':
            return 184;
        }
      }
      return 0;
    }

    private static string farmerAccomplishments()
    {
      string str = Game1.player.isMale ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5229") : Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5230");
      if (Game1.player.hasRustyKey)
        str += Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5235");
      if (Game1.player.achievements.Contains(71))
        str += Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5236");
      if (Game1.player.achievements.Contains(45))
        str += Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5237");
      if (str.Length > 115)
        str += "#$b#";
      if (Game1.player.achievements.Contains(63))
        str += Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5239");
      if (Game1.player.timesReachedMineBottom > 0)
        str += Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5240");
      return str + Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5241", (object) (uint) ((int) Game1.player.totalMoneyEarned - (int) (Game1.player.totalMoneyEarned % 1000U)));
    }

    public static string getCreditsString()
    {
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5243") + Environment.NewLine + " " + Environment.NewLine + Environment.NewLine + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5244") + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5245") + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5246") + Environment.NewLine + Environment.NewLine + "-Eric Barone" + Environment.NewLine + " " + Environment.NewLine + Environment.NewLine + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5248") + Environment.NewLine + Environment.NewLine + "-Amber Hageman" + Environment.NewLine + "-Shane Waletzko" + Environment.NewLine + "-Fiddy, Nuns, Kappy &" + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5252") + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5253") + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5254");
    }

    public static string getStardewHeroCelebrationEventString(int finalFarmerScore)
    {
      string str;
      if (finalFarmerScore >= Game1.percentageToWinStardewHero)
        str = "title_day/-100 -100/farmer 18 20 1 rival 27 20 2" + Utility.getCelebrationPositionsForDatables(Game1.player.spouse) + (Game1.player.spouse == null || Game1.player.spouse.Contains("engaged") ? "" : Game1.player.spouse + " 17 21 1 ") + "Lewis 22 19 2 Marnie 21 22 0 Caroline 24 22 0 Pierre 25 22 0 Gus 26 22 0 Clint 26 23 0 Emily 25 23 0 Shane 27 23 0 " + (!Game1.player.friendships.ContainsKey("Sandy") || Game1.player.friendships["Sandy"][0] <= 0 ? "" : "Sandy 24 23 0 ") + "George 21 23 0 Evelyn 20 23 0 Pam 19 23 0 Jodi 27 24 0 " + ((object) Game1.getCharacterFromName("Kent", false) != null ? "Kent 26 24 0 " : "") + "Linus 24 24 0 Robin 21 24 0 Demetrius 20 24 0" + (Game1.player.timesReachedMineBottom > 0 ? " Dwarf 19 24 0" : "") + "/addObject 18 19 " + (object) Game1.random.Next(313, 320) + "/addObject 19 19 " + (object) Game1.random.Next(313, 320) + "/addObject 20 19 " + (object) Game1.random.Next(313, 320) + "/addObject 25 19 " + (object) Game1.random.Next(313, 320) + "/addObject 26 19 " + (object) Game1.random.Next(313, 320) + "/addObject 27 19 " + (object) Game1.random.Next(313, 320) + "/addObject 23 19 468/viewport 22 20 true/pause 4000/speak Lewis \"" + Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5256") + "\"/pause 400/faceDirection Lewis 3/pause 500/faceDirection Lewis 1/pause 600/faceDirection Lewis 2/speak Lewis \"" + Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5259") + "\"/pause 200/showRivalFrame 16/pause 600/speak Lewis \"" + Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5260") + "\"/pause 700/move Lewis 0 1 3/stopMusic/move Lewis -2 0 3/playMusic musicboxsong/faceDirection farmer 1/showRivalFrame 12/speak Lewis \"" + Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5263", (object) Utility.farmerAccomplishments()) + "\"/pause 800/move Lewis 5 0 1/showRivalFrame 12/playMusic rival/pause 500/speak Lewis \"" + (Game1.player.isMale ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5306") : Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5307")) + "\"/pause 500/speak rival \"" + Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5308") + "\"/move rival 0 1 2/showRivalFrame 17/pause 500/speak rival \"" + (Game1.player.isMale ? (Game1.random.NextDouble() < 0.5 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5310") : Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5311")) : (Game1.random.NextDouble() < 0.5 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5312") : Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5313"))) + "\"/pause 600/emote farmer 40/showRivalFrame 16/pause 900/move rival 0 -1 2/showRivalFrame 16/move Lewis -3 0 2/stopMusic/pause 500/speak Lewis \"" + Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5314") + "\"/stopMusic/move Lewis 0 -1 2/pause 600/faceDirection Lewis 1/pause 600/faceDirection Lewis 3/pause 600/faceDirection Lewis 2/speak Lewis \"" + Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5315") + "\"/pause 300/move rival -2 0 2/showRivalFrame 16/pause 1500/speak Lewis \"" + Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5316") + "\"/pause 500/showRivalFrame 18/pause 400/playMusic happy/emote farmer 16/move farmer 5 0 2/move Lewis 0 1 1/speak Lewis \"" + Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5317", (object) finalFarmerScore) + "\"/speak Emily \"" + Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5318") + "\"/speak Gus \"" + Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5319") + "\"/speak Pierre \"" + Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5320") + "\"/showRivalFrame 12/pause 500/speak rival \"" + Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5321") + "\"/speed rival 4/move rival 6 0 0/faceDirection farmer 1 true/speed rival 4/move rival 0 -10 1/warp rival -100 -100/move farmer 0 1 2/emote farmer 20/fade/viewport -1000 -1000/message \"" + Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5322", (object) Utility.getOtherFarmerNames()[0]) + "\"/end credits";
      else
        str = "title_day/-100 -100/farmer 18 20 1 rival 27 20 2" + Utility.getCelebrationPositionsForDatables(Game1.player.spouse) + (Game1.player.spouse == null || Game1.player.spouse.Contains("engaged") ? "" : Game1.player.spouse + " 17 21 1 ") + "Lewis 22 19 2 Marnie 21 22 0 Caroline 24 22 0 Pierre 25 22 0 Gus 26 22 0 Clint 26 23 0 Emily 25 23 0 Shane 27 23 0 " + (!Game1.player.friendships.ContainsKey("Sandy") || Game1.player.friendships["Sandy"][0] <= 0 ? "" : "Sandy 24 23 0 ") + "George 21 23 0 Evelyn 20 23 0 Pam 19 23 0 Jodi 27 24 0 " + ((object) Game1.getCharacterFromName("Kent", false) != null ? "Kent 26 24 0 " : "") + "Linus 24 24 0 Robin 21 24 0 Demetrius 20 24 0" + (Game1.player.timesReachedMineBottom > 0 ? " Dwarf 19 24 0" : "") + "/addObject 18 19 " + (object) Game1.random.Next(313, 320) + "/addObject 19 19 " + (object) Game1.random.Next(313, 320) + "/addObject 20 19 " + (object) Game1.random.Next(313, 320) + "/addObject 25 19 " + (object) Game1.random.Next(313, 320) + "/addObject 26 19 " + (object) Game1.random.Next(313, 320) + "/addObject 27 19 " + (object) Game1.random.Next(313, 320) + "/addObject 23 19 468/viewport 22 20 true/pause 4000/speak Lewis \"" + Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5256") + "\"/pause 400/faceDirection Lewis 3/pause 500/faceDirection Lewis 1/pause 600/faceDirection Lewis 2/speak Lewis \"" + Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5259") + "\"/pause 200/showRivalFrame 16/pause 600/speak Lewis \"" + Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5260") + "\"/pause 700/move Lewis 0 1 3/stopMusic/move Lewis -2 0 3/playMusic musicboxsong/faceDirection farmer 1/showRivalFrame 12/speak Lewis \"" + Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5263", (object) Utility.farmerAccomplishments()) + "\"/pause 800/move Lewis 5 0 1/showRivalFrame 12/playMusic rival/pause 500/speak Lewis \"" + (Game1.player.isMale ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5306") : Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5307")) + "\"/pause 500/speak rival \"" + Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5308") + "\"/move rival 0 1 2/showRivalFrame 17/pause 500/speak rival \"" + (Game1.player.isMale ? (Game1.random.NextDouble() < 0.5 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5310") : Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5311")) : (Game1.random.NextDouble() < 0.5 ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5312") : Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5313"))) + "\"/pause 600/emote farmer 40/showRivalFrame 16/pause 900/move rival 0 -1 2/showRivalFrame 16/move Lewis -3 0 2/stopMusic/pause 500/speak Lewis \"" + Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5314") + "\"/stopMusic/move Lewis 0 -1 2/pause 600/faceDirection Lewis 1/pause 600/faceDirection Lewis 3/pause 600/faceDirection Lewis 2/speak Lewis \"" + Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5315") + "\"/pause 300/move rival -2 0 2/showRivalFrame 16/pause 1500/speak Lewis \"" + Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5323") + "\"/pause 200/showFrame 32/move rival -2 0 2/showRivalFrame 19/pause 400/playSound death/emote farmer 28/speak Lewis \"" + Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5324", (object) (Game1.percentageToWinStardewHero - finalFarmerScore)) + "\"/speak rival \"" + Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5325") + "\"/pause 600/faceDirection Lewis 3/speak Lewis \"" + Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5326") + "\"/speak Emily \"" + Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5327") + "\"/fade/viewport -1000 -1000/message \"" + Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5328", (object) finalFarmerScore) + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5329") + "\"/end credits";
      return str;
    }

    public static void perpareDayForStardewCelebration(int finalFarmerScore)
    {
      bool flag = finalFarmerScore >= Game1.percentageToWinStardewHero;
      foreach (GameLocation location in Game1.locations)
      {
        foreach (NPC character in location.characters)
        {
          string masterDialogue = "";
          if (flag)
          {
            switch (Game1.random.Next(6))
            {
              case 0:
                masterDialogue = Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5348");
                break;
              case 1:
                masterDialogue = Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5349");
                break;
              case 2:
                masterDialogue = Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5350");
                break;
              case 3:
                masterDialogue = Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5351");
                break;
              case 4:
                masterDialogue = Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5352");
                break;
              case 5:
                masterDialogue = Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5353");
                break;
            }
            if (character.name.Equals("Sebastian") || character.name.Equals("Abigail"))
              masterDialogue = Game1.player.isMale ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5356") : Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5357");
            else if (character.name.Equals("George"))
              masterDialogue = Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5360");
          }
          else
          {
            switch (Game1.random.Next(4))
            {
              case 0:
                masterDialogue = Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5361");
                break;
              case 1:
                masterDialogue = Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5362");
                break;
              case 2:
                masterDialogue = Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5363");
                break;
              case 3:
                masterDialogue = Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5364");
                break;
            }
            if (character.name.Equals("George"))
              masterDialogue = Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5360");
          }
          character.CurrentDialogue.Push(new Dialogue(masterDialogue, character));
        }
      }
      if (!flag)
        return;
      Game1.player.stardewHero = true;
    }

    public static string getCelebrationPositionsForDatables(string personToExclude)
    {
      if (personToExclude == null)
        personToExclude = "";
      string str = " ";
      if (!personToExclude.Equals("Sam"))
        str += "Sam 25 65 0 ";
      if (!personToExclude.Equals("Sebastian"))
        str += "Sebastian 24 65 0 ";
      if (!personToExclude.Equals("Alex"))
        str += "Alex 25 69 0 ";
      if (!personToExclude.Equals("Harvey"))
        str += "Harvey 23 67 0 ";
      if (!personToExclude.Equals("Elliott"))
        str += "Elliott 32 65 0 ";
      if (!personToExclude.Equals("Haley"))
        str += "Haley 26 69 0 ";
      if (!personToExclude.Equals("Penny"))
        str += "Penny 23 66 0 ";
      if (!personToExclude.Equals("Maru"))
        str += "Maru 24 68 0 ";
      if (!personToExclude.Equals("Leah"))
        str += "Leah 33 65 0 ";
      if (!personToExclude.Equals("Abigail"))
        str += "Abigail 23 65 0 ";
      return str;
    }

    public static void fixAllAnimals()
    {
      Farm farm = Game1.getFarm();
      foreach (Building building in farm.buildings)
      {
        if (building.indoors != null && building.indoors is AnimalHouse)
        {
          foreach (long id in (building.indoors as AnimalHouse).animalsThatLiveHere)
          {
            FarmAnimal animal = Utility.getAnimal(id);
            if (animal != null)
            {
              animal.home = building;
              animal.homeLocation = new Vector2((float) building.tileX, (float) building.tileY);
              animal.setRandomPosition(animal.home.indoors);
              if (!(animal.home.indoors as AnimalHouse).animals.ContainsKey(animal.myID))
                (animal.home.indoors as AnimalHouse).animals.Add(animal.myID, animal);
            }
          }
        }
      }
      List<FarmAnimal> farmAnimalList1 = new List<FarmAnimal>();
      foreach (FarmAnimal allFarmAnimal in farm.getAllFarmAnimals())
      {
        if (allFarmAnimal.home == null)
          farmAnimalList1.Add(allFarmAnimal);
      }
      foreach (FarmAnimal farmAnimal in farmAnimalList1)
      {
        KeyValuePair<long, FarmAnimal> keyValuePair;
        foreach (Building building in farm.buildings)
        {
          if (building.indoors != null && building.indoors is AnimalHouse)
          {
            for (int index = (building.indoors as AnimalHouse).animals.Count - 1; index >= 0; --index)
            {
              keyValuePair = (building.indoors as AnimalHouse).animals.ElementAt<KeyValuePair<long, FarmAnimal>>(index);
              if (keyValuePair.Value.Equals((object) farmAnimal))
              {
                SerializableDictionary<long, FarmAnimal> animals = (building.indoors as AnimalHouse).animals;
                keyValuePair = (building.indoors as AnimalHouse).animals.ElementAt<KeyValuePair<long, FarmAnimal>>(index);
                long key = keyValuePair.Key;
                animals.Remove(key);
              }
            }
          }
        }
        for (int index = farm.animals.Count - 1; index >= 0; --index)
        {
          keyValuePair = farm.animals.ElementAt<KeyValuePair<long, FarmAnimal>>(index);
          if (keyValuePair.Value.Equals((object) farmAnimal))
          {
            SerializableDictionary<long, FarmAnimal> animals = farm.animals;
            keyValuePair = farm.animals.ElementAt<KeyValuePair<long, FarmAnimal>>(index);
            long key = keyValuePair.Key;
            animals.Remove(key);
          }
        }
      }
      foreach (Building building in farm.buildings)
      {
        if (building.indoors != null && building.indoors is AnimalHouse)
        {
          for (int index = (building.indoors as AnimalHouse).animalsThatLiveHere.Count - 1; index >= 0; --index)
          {
            if (Utility.getAnimal((building.indoors as AnimalHouse).animalsThatLiveHere[index]).home != building)
              (building.indoors as AnimalHouse).animalsThatLiveHere.RemoveAt(index);
          }
        }
      }
      foreach (FarmAnimal farmAnimal in farmAnimalList1)
      {
        foreach (Building building in farm.buildings)
        {
          if (building.buildingType.Contains(farmAnimal.buildingTypeILiveIn) && building.indoors != null && (building.indoors is AnimalHouse && !(building.indoors as AnimalHouse).isFull()))
          {
            farmAnimal.home = building;
            farmAnimal.homeLocation = new Vector2((float) building.tileX, (float) building.tileY);
            farmAnimal.setRandomPosition(farmAnimal.home.indoors);
            (farmAnimal.home.indoors as AnimalHouse).animals.Add(farmAnimal.myID, farmAnimal);
            (farmAnimal.home.indoors as AnimalHouse).animalsThatLiveHere.Add(farmAnimal.myID);
            break;
          }
        }
      }
      List<FarmAnimal> farmAnimalList2 = new List<FarmAnimal>();
      foreach (FarmAnimal farmAnimal in farmAnimalList1)
      {
        if (farmAnimal.home == null)
          farmAnimalList2.Add(farmAnimal);
      }
      foreach (FarmAnimal farmAnimal in farmAnimalList2)
      {
        farmAnimal.position = Utility.recursiveFindOpenTileForCharacter((Character) farmAnimal, (GameLocation) farm, new Vector2(40f, 40f), 200) * (float) Game1.tileSize;
        if (!farm.animals.ContainsKey(farmAnimal.myID))
          farm.animals.Add(farmAnimal.myID, farmAnimal);
      }
    }

    public static string getWeddingEvent()
    {
      string[] strArray = new string[23];
      strArray[0] = "sweet/-1000 -100/farmer 27 63 2 spouse 28 63 2";
      strArray[1] = Utility.getCelebrationPositionsForDatables(Game1.player.spouse);
      strArray[2] = "Lewis 27 64 2 Marnie 26 65 0 Caroline 29 65 0 Pierre 30 65 0 Gus 31 65 0 Clint 31 66 0 ";
      strArray[3] = Game1.player.spouse.Contains("Emily") ? "" : "Emily 30 66 0 ";
      strArray[4] = Game1.player.spouse.Contains("Shane") ? "" : "Shane 32 66 0 ";
      strArray[5] = !Game1.player.friendships.ContainsKey("Sandy") || Game1.player.friendships["Sandy"][0] <= 0 ? "" : "Sandy 29 66 0 ";
      strArray[6] = "George 26 66 0 Evelyn 25 66 0 Pam 24 66 0 Jodi 32 67 0 ";
      strArray[7] = Game1.getCharacterFromName("Kent", false) != null ? "Kent 31 67 0 " : "";
      strArray[8] = "Linus 29 67 0 Robin 25 67 0 Demetrius 26 67 0 Vincent 26 68 3 Jas 25 68 1";
      strArray[9] = Game1.player.timesReachedMineBottom > 0 ? " Dwarf 30 67 0" : "";
      strArray[10] = "/showFrame spouse 36/specificTemporarySprite wedding/viewport 27 64 true/pause 4000/showFrame 133/pause 2000/speak Lewis \"";
      int index1 = 11;
      string str1;
      if (!Game1.player.IsMale)
        str1 = Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5369", (object) Game1.dayOfMonth, (object) Game1.CurrentSeasonDisplayName);
      else
        str1 = Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5367", (object) Game1.dayOfMonth, (object) Game1.CurrentSeasonDisplayName);
      strArray[index1] = str1;
      int index2 = 12;
      string str2 = "\"/faceDirection farmer 1/showFrame spouse 37/pause 500/faceDirection Lewis 0/pause 2000/speak Lewis \"";
      strArray[index2] = str2;
      int index3 = 13;
      string str3 = Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5370");
      strArray[index3] = str3;
      int index4 = 14;
      string str4 = "\"/move Lewis 0 1 0/playMusic none/pause 1000/showFrame Lewis 20/speak Lewis \"";
      strArray[index4] = str4;
      int index5 = 15;
      string str5 = Game1.player.IsMale ? (Utility.isMale(Game1.player.spouse) ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5371") : Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5373")) : (Utility.isMale(Game1.player.spouse) ? Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5377") : Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5375"));
      strArray[index5] = str5;
      int index6 = 16;
      string str6 = "\"/pause 500/speak Lewis \"";
      strArray[index6] = str6;
      int index7 = 17;
      string str7 = Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5379");
      strArray[index7] = str7;
      int index8 = 18;
      string str8 = "\"/pause 1000/showFrame 101/showFrame spouse 38/specificTemporarySprite heart 27 63/playSound dwop/pause 2000/specificTemporarySprite wed/warp Marnie -2000 -2000/faceDirection farmer 2/showFrame spouse 36/faceDirection Pam 1 true/faceDirection Evelyn 3 true/faceDirection Pierre 3 true/faceDirection Caroline 1 true/animate Robin false true 500 20 21 20 22/animate Demetrius false true 500 24 25 24 26/move Lewis 0 3 3 true/move Caroline 0 -1 3 false/pause 4000/faceDirection farmer 1/showFrame spouse 37/globalFade/viewport -1000 -1000/pause 1000/message \"";
      strArray[index8] = str8;
      int index9 = 19;
      string str9 = Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5381");
      strArray[index9] = str9;
      int index10 = 20;
      string str10 = "\"/pause 500/message \"";
      strArray[index10] = str10;
      int index11 = 21;
      string str11 = Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5383");
      strArray[index11] = str11;
      int index12 = 22;
      string str12 = "\"/pause 4000/end wedding";
      strArray[index12] = str12;
      return string.Concat(strArray);
    }

    public static void drawTinyDigits(int toDraw, SpriteBatch b, Vector2 position, float scale, float layerDepth, Color c)
    {
      int num1 = 0;
      int num2 = toDraw;
      int num3 = 0;
      do
      {
        ++num3;
      }
      while ((toDraw /= 10) >= 1);
      int num4 = (int) Math.Pow(10.0, (double) (num3 - 1));
      bool flag = false;
      for (int index = 0; index < num3; ++index)
      {
        int num5 = num2 / num4 % 10;
        if (num5 > 0 || index == num3 - 1)
          flag = true;
        if (flag)
          b.Draw(Game1.mouseCursors, position + new Vector2((float) num1, 0.0f), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(368 + num5 * 5, 56, 5, 7)), c, 0.0f, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
        num1 += (int) (5.0 * (double) scale) - 1;
        num4 /= 10;
      }
    }

    public static int getWidthOfTinyDigitString(int toDraw, float scale)
    {
      int num = 0;
      do
      {
        ++num;
      }
      while ((toDraw /= 10) >= 1);
      return (int) ((double) (num * 5) * (double) scale);
    }

    public static bool isMale(string who)
    {
      // ISSUE: reference to a compiler-generated method
      uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(who);
      if (stringHash <= 2434294092U)
      {
        if ((int) stringHash != 587846041)
        {
          if ((int) stringHash != 1281010426)
          {
            if ((int) stringHash != -1860673204 || !(who == "Haley"))
              goto label_14;
          }
          else if (!(who == "Maru"))
            goto label_14;
        }
        else if (!(who == "Penny"))
          goto label_14;
      }
      else if (stringHash <= 2732913340U)
      {
        if ((int) stringHash != -1723138655)
        {
          if ((int) stringHash != -1562053956 || !(who == "Abigail"))
            goto label_14;
        }
        else if (!(who == "Emily"))
          goto label_14;
      }
      else if ((int) stringHash != -1468719973)
      {
        if ((int) stringHash != -100384626 || !(who == "Sandy"))
          goto label_14;
      }
      else if (!(who == "Leah"))
        goto label_14;
      return false;
label_14:
      return true;
    }

    public static bool doesItemWithThisIndexExistAnywhere(int index, bool bigCraftable = false)
    {
      for (int index1 = Game1.player.items.Count - 1; index1 >= 0; --index1)
      {
        if (Game1.player.items[index1] != null && Game1.player.items[index1] is Object && (Game1.player.items[index1].parentSheetIndex == index && (Game1.player.items[index1] as Object).bigCraftable == bigCraftable))
          return true;
      }
      foreach (GameLocation location in Game1.locations)
      {
        foreach (KeyValuePair<Vector2, Object> keyValuePair in (Dictionary<Vector2, Object>) location.objects)
        {
          if (keyValuePair.Value != null)
          {
            if (keyValuePair.Value.parentSheetIndex == index && keyValuePair.Value.bigCraftable == bigCraftable)
              return true;
            if (keyValuePair.Value is Chest)
            {
              foreach (Item obj in (keyValuePair.Value as Chest).items)
              {
                if (obj != null && obj is Object && (obj.parentSheetIndex == index && (obj as Object).bigCraftable == bigCraftable))
                  return true;
              }
            }
            if (keyValuePair.Value.heldObject != null && keyValuePair.Value.heldObject.parentSheetIndex == index && keyValuePair.Value.heldObject.bigCraftable == bigCraftable)
              return true;
          }
        }
        foreach (Debris debri in location.debris)
        {
          if (debri.item != null && debri.item is Object && (debri.item.parentSheetIndex == index && (debri.item as Object).bigCraftable == bigCraftable))
            return true;
        }
        if (location is Farm)
        {
          foreach (Building building in (location as Farm).buildings)
          {
            if (building.indoors != null)
            {
              foreach (KeyValuePair<Vector2, Object> keyValuePair in (Dictionary<Vector2, Object>) building.indoors.objects)
              {
                if (keyValuePair.Value != null)
                {
                  if (keyValuePair.Value.parentSheetIndex == index && keyValuePair.Value.bigCraftable == bigCraftable)
                    return true;
                  if (keyValuePair.Value is Chest)
                  {
                    foreach (Item obj in (keyValuePair.Value as Chest).items)
                    {
                      if (obj != null && obj is Object && (obj.parentSheetIndex == index && (obj as Object).bigCraftable == bigCraftable))
                        return true;
                    }
                  }
                }
              }
              foreach (Debris debri in building.indoors.debris)
              {
                if (debri.item != null && debri.item is Object && (debri.item.parentSheetIndex == index && (debri.item as Object).bigCraftable == bigCraftable))
                  return true;
              }
              if (building.indoors is DecoratableLocation)
              {
                foreach (Furniture furniture in (building.indoors as DecoratableLocation).furniture)
                {
                  if (furniture.heldObject != null && furniture.heldObject.parentSheetIndex == index && furniture.bigCraftable == bigCraftable)
                    return true;
                }
              }
            }
            else if (building is Mill)
            {
              foreach (Item obj in (building as Mill).output.items)
              {
                if (obj != null && obj is Object && (obj.parentSheetIndex == index && (obj as Object).bigCraftable == bigCraftable))
                  return true;
              }
            }
            else if (building is JunimoHut)
            {
              foreach (Item obj in (building as JunimoHut).output.items)
              {
                if (obj != null && obj is Object && (obj.parentSheetIndex == index && (obj as Object).bigCraftable == bigCraftable))
                  return true;
              }
            }
          }
        }
        else if (location is FarmHouse)
        {
          foreach (Item obj in (location as FarmHouse).fridge.items)
          {
            if (obj != null && obj is Object && (obj.parentSheetIndex == index && (obj as Object).bigCraftable == bigCraftable))
              return true;
          }
        }
        if (location is DecoratableLocation)
        {
          foreach (Furniture furniture in (location as DecoratableLocation).furniture)
          {
            if (furniture.heldObject != null && furniture.heldObject.parentSheetIndex == index && furniture.bigCraftable == bigCraftable)
              return true;
          }
        }
      }
      return false;
    }

    public static int getSwordUpgradeLevel()
    {
      foreach (Item obj in Game1.player.items)
      {
        if (obj != null && obj.GetType() == typeof (Sword))
          return ((Tool) obj).upgradeLevel;
      }
      foreach (Tool tool in Game1.player.toolBox)
      {
        if (tool != null && tool.name.Contains("Sword"))
          return tool.upgradeLevel;
      }
      return 0;
    }

    public static bool tryToAddObjectToHome(Object o)
    {
      GameLocation locationFromName = Game1.getLocationFromName("FarmHouse");
      for (int index1 = locationFromName.map.GetLayer("Back").LayerWidth - 1; index1 >= 0; --index1)
      {
        for (int index2 = locationFromName.map.GetLayer("Back").LayerHeight - 1; index2 >= 0; --index2)
        {
          if (locationFromName.map.GetLayer("Back").Tiles[index1, index2] != null && locationFromName.dropObject(o, new Vector2((float) (index1 * Game1.tileSize), (float) (index2 * Game1.tileSize)), Game1.viewport, false, (Farmer) null))
          {
            if (o.ParentSheetIndex == 468)
              locationFromName.objects[new Vector2((float) index1, (float) index2)] = new Object(new Vector2((float) index1, (float) index2), 308, (string) null, true, true, false, false)
              {
                heldObject = o
              };
            return true;
          }
        }
      }
      return false;
    }

    public static List<Color> getOverallsColors()
    {
      return new List<Color>()
      {
        new Color(33, 36, 105),
        new Color(40, 68, 21),
        new Color(68, 44, 21),
        new Color(39, 10, 71),
        new Color(23, 64, 66),
        new Color(62, 23, 66),
        new Color(53, 11, 30),
        new Color(23, 21, 24),
        new Color(50, 50, 50),
        new Color(57, 54, 34),
        new Color(7, 84, 55),
        new Color(29, 32, 0),
        new Color(30, 1, 1),
        new Color(0, 30, 1),
        new Color(31, 0, 30)
      };
    }

    public static List<Color> getShirtColors()
    {
      return new List<Color>()
      {
        new Color(97, 96, 97),
        new Color(21, 22, 21),
        new Color(40, 90, 41),
        new Color(90, 41, 40),
        new Color(41, 40, 90),
        new Color(49, 0, 0),
        new Color(0, 51, 0),
        new Color(1, 0, 52),
        new Color(80, 1, 80),
        new Color(0, 84, 84),
        new Color(80, 84, 0)
      };
    }

    public static List<Color> getSkinColors()
    {
      return new List<Color>()
      {
        new Color(96, 57, 19),
        new Color(170, 120, 18),
        new Color(150, 100, 18),
        new Color(85, 62, 10),
        new Color(60, 48, 11)
      };
    }

    public static List<Color> getHairColors()
    {
      return new List<Color>()
      {
        new Color(35, 21, 11),
        new Color(1, 1, 1),
        new Color(30, 20, 3),
        new Color(52, 26, 14),
        new Color(100, 80, 10),
        new Color(27, 38, 39),
        new Color(120, 90, 0),
        new Color(90, 29, 37),
        new Color(18, 15, 51),
        new Color(49, 17, 41),
        new Color(19, 50, 16),
        new Color(13, 46, 73)
      };
    }

    public static List<Color> getEyeColors()
    {
      return new List<Color>()
      {
        new Color(82, 49, 16),
        new Color(3, 3, 3),
        new Color(58, 64, 90),
        new Color(0, 0, 61),
        new Color(3, 75, 176),
        new Color(60, 11, 11),
        new Color(70, 11, 95),
        new Color(0, 79, 53),
        new Color(50, 26, 10),
        new Color(41, 52, 46),
        new Color(39, 0, 94),
        new Color(71, 57, 36)
      };
    }

    public static void changeFarmerSkinColor(Color baseColor)
    {
      int targetColorIndex = 3214 * Game1.player.Sprite.Texture.Bounds.Width + 209;
      if ((int) baseColor.R < 60)
      {
        Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, targetColorIndex, (int) baseColor.R, (int) baseColor.G, (int) baseColor.B);
        Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, targetColorIndex + 4, (int) baseColor.R + 45, (int) baseColor.G + 15, (int) baseColor.B);
        Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, targetColorIndex + 8, (int) baseColor.R + 50, (int) baseColor.G + 20, (int) baseColor.B + 5);
        Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, targetColorIndex + 12, (int) baseColor.R + 60, (int) baseColor.G + 30, (int) baseColor.B + 10);
        Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, targetColorIndex + 16, (int) baseColor.R + 90, (int) baseColor.G + 50, (int) baseColor.B + 20);
        Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, targetColorIndex + 20, (int) baseColor.R + 120, (int) baseColor.G + 70, (int) baseColor.B + 30);
      }
      else if ((int) baseColor.R <= 85 || (int) baseColor.B == 18)
      {
        Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, targetColorIndex, (int) baseColor.B == 18 ? (int) baseColor.R - (int) baseColor.R / 2 : (int) baseColor.R, (int) baseColor.B == 18 ? (int) baseColor.G - (int) baseColor.G / 2 : (int) baseColor.G, (int) baseColor.B);
        Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, targetColorIndex + 4, (int) baseColor.R + 65, (int) baseColor.G + 20, (int) baseColor.B);
        Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, targetColorIndex + 8, (int) baseColor.R + 75, (int) baseColor.G + 30, (int) baseColor.B + 20);
        Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, targetColorIndex + 12, (int) baseColor.R + 90, (int) baseColor.G + 40, (int) baseColor.B + 40);
        Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, targetColorIndex + 16, (int) baseColor.R + 110, (int) baseColor.G + 60, (int) baseColor.B + 60);
        Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, targetColorIndex + 20, (int) baseColor.R + 130, (int) baseColor.G + 80, (int) baseColor.B + 70);
        if (Game1.player.eyeColor != 0)
          return;
        Game1.player.eyeColor = 1;
        Utility.changeFarmerEyeColor(Utility.getEyeColors()[Game1.player.eyeColor]);
      }
      else
      {
        Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, targetColorIndex, (int) baseColor.R, (int) baseColor.G, (int) baseColor.B);
        Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, targetColorIndex + 4, (int) baseColor.R + 112, (int) baseColor.G + 69, (int) baseColor.B);
        Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, targetColorIndex + 8, (int) baseColor.R + 129, (int) baseColor.G + 115, (int) baseColor.B + 86);
        Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, targetColorIndex + 12, (int) baseColor.R + 149, (int) baseColor.G + 129, (int) baseColor.B + 93);
        Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, targetColorIndex + 16, (int) baseColor.R + 150, (int) baseColor.G + 145, (int) baseColor.B + 118);
        Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, targetColorIndex + 20, (int) baseColor.R + 157, (int) baseColor.G + 169, (int) baseColor.B + 160);
      }
    }

    public static void changeFarmerHairColor(Color baseColor)
    {
      int num = 3206 * Game1.player.Sprite.Texture.Bounds.Width + 201;
      if ((int) baseColor.B == 3)
      {
        Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, num + 4, (int) baseColor.R, (int) baseColor.G, (int) baseColor.B + 1);
        Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, num + 8, (int) baseColor.R + 10, (int) baseColor.G + 5, (int) baseColor.B + 2);
        Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, num + 12, (int) baseColor.R + 20, (int) baseColor.G + 10, (int) baseColor.B + 3);
        Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, num + 16, (int) baseColor.R + 30, (int) baseColor.G + 20, (int) baseColor.B + 4);
        Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, num + 20, (int) baseColor.R + 40, (int) baseColor.G + 30, (int) baseColor.B + 5);
        Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, num + 24, (int) baseColor.R + 50, (int) baseColor.G + 40, (int) baseColor.B + 6);
        Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, num + 28, (int) baseColor.R + 60, (int) baseColor.G + 50, (int) baseColor.B + 8);
        Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, num + 32, (int) baseColor.R + 80, (int) baseColor.G + 70, (int) baseColor.B + 20);
      }
      else if ((int) baseColor.B == 1)
      {
        Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, num + 4, (int) baseColor.R, (int) baseColor.G, (int) baseColor.B);
        Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, num + 8, (int) baseColor.R + 5, (int) baseColor.G + 5, (int) baseColor.B + 5);
        Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, num + 12, (int) baseColor.R + 10, (int) baseColor.G + 10, (int) baseColor.B + 10);
        Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, num + 16, (int) baseColor.R + 15, (int) baseColor.G + 15, (int) baseColor.B + 15);
        Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, num + 20, (int) baseColor.R + 20, (int) baseColor.G + 20, (int) baseColor.B + 20);
        Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, num + 24, (int) baseColor.R + 25, (int) baseColor.G + 25, (int) baseColor.B + 25);
        Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, num + 28, (int) baseColor.R + 35, (int) baseColor.G + 35, (int) baseColor.B + 35);
        Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, num + 32, (int) baseColor.R + 50, (int) baseColor.G + 50, (int) baseColor.B + 50);
      }
      else
      {
        Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, num + 4, (int) baseColor.R + 10, (int) baseColor.G + 10, (int) baseColor.B + 10);
        Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, num + 8, (int) baseColor.R + 20, (int) baseColor.G + 20, (int) baseColor.B + 20);
        Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, num + 12, (int) baseColor.R + 30, (int) baseColor.G + 30, (int) baseColor.B + 30);
        Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, num + 16, (int) baseColor.R + 40, (int) baseColor.G + 40, (int) baseColor.B + 40);
        Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, num + 20, (int) baseColor.R + 50, (int) baseColor.G + 50, (int) baseColor.B + 50);
        Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, num + 24, (int) baseColor.R + 60, (int) baseColor.G + 60, (int) baseColor.B + 60);
        Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, num + 28, (int) baseColor.R + 80, (int) baseColor.G + 80, (int) baseColor.B + 80);
        Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, num + 32, (int) baseColor.R + 120, (int) baseColor.G + 120, (int) baseColor.B + 120);
      }
    }

    internal static void CollectGarbage(string filePath = "", int lineNumber = 0)
    {
      GC.Collect(0, GCCollectionMode.Forced);
    }

    public static void changeFarmerShirtColor(Color baseColor)
    {
      int targetColorIndex = 3221 * Game1.player.Sprite.Texture.Bounds.Width + 213;
      Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, targetColorIndex, (int) baseColor.R, (int) baseColor.G, (int) baseColor.B);
      Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, targetColorIndex + 4, (int) baseColor.R + 50, (int) baseColor.G + 50, (int) baseColor.B + 50);
      Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, targetColorIndex + 8, (int) baseColor.R + 100, (int) baseColor.G + 100, (int) baseColor.B + 100);
    }

    public static void changeFarmerEyeColor(Color baseColor)
    {
      int targetColorIndex = 3230 * Game1.player.Sprite.Texture.Bounds.Width + 217;
      Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, targetColorIndex, (int) baseColor.R, (int) baseColor.G, (int) baseColor.B);
      Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, targetColorIndex - 4, (int) baseColor.R + 50, (int) baseColor.G + 50, (int) baseColor.B + 50);
    }

    public static string InvokeSimpleReturnTypeMethod(object toBeCalled, string methodName, object[] parameters)
    {
      Type type = toBeCalled.GetType();
      try
      {
        return (string) type.InvokeMember(methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod, (Binder) null, toBeCalled, parameters) ?? "";
      }
      catch (Exception ex)
      {
        return Game1.parseText("Didn't work - " + ex.Message);
      }
    }

    public static List<int> possibleCropsAtThisTime(string season, bool firstWeek)
    {
      List<int> intList1 = (List<int>) null;
      List<int> intList2 = (List<int>) null;
      if (season.Equals("spring"))
      {
        intList1 = new List<int>() { 24, 192 };
        if (Game1.year > 1)
          intList1.Add(250);
        if (Game1.player.eventsSeen.Contains(61))
          intList1.Add(248);
        intList2 = new List<int>() { 190, 188 };
        if (Game1.player.eventsSeen.Contains(61))
          intList2.Add(252);
        intList2.AddRange((IEnumerable<int>) intList1);
      }
      else if (season.Equals("summer"))
      {
        intList1 = new List<int>() { 264, 262, 260 };
        intList2 = new List<int>() { 254, 256 };
        if (Game1.year > 1)
          intList1.Add(266);
        if (Game1.player.eventsSeen.Contains(61))
          intList2.AddRange((IEnumerable<int>) new int[2]
          {
            258,
            268
          });
        intList2.AddRange((IEnumerable<int>) intList1);
      }
      else if (season.Equals("fall"))
      {
        intList1 = new List<int>() { 272, 278 };
        intList2 = new List<int>() { 270, 276, 280 };
        if (Game1.year > 1)
          intList2.Add(274);
        if (Game1.player.eventsSeen.Contains(61))
        {
          intList1.Add(284);
          intList2.Add(282);
        }
        intList2.AddRange((IEnumerable<int>) intList1);
      }
      if (firstWeek)
        return intList1;
      return intList2;
    }

    public static int[] cropsOfTheWeek()
    {
      Random random = new Random((int) Game1.uniqueIDForThisGame + (int) (Game1.stats.DaysPlayed / 29U));
      int[] numArray = new int[4];
      List<int> source1 = Utility.possibleCropsAtThisTime(Game1.currentSeason, true);
      List<int> source2 = Utility.possibleCropsAtThisTime(Game1.currentSeason, false);
      if (source1 != null)
      {
        numArray[0] = source1.ElementAt<int>(random.Next(source1.Count));
        for (int index = 1; index < 4; ++index)
        {
          numArray[index] = source2.ElementAt<int>(random.Next(source2.Count));
          while (numArray[index] == numArray[index - 1])
            numArray[index] = source2.ElementAt<int>(random.Next(source2.Count));
        }
      }
      return numArray;
    }

    public static int getRandomItemFromSeason(string season, int randomSeedAddition, bool forQuest)
    {
      Random random = new Random((int) Game1.uniqueIDForThisGame + (int) Game1.stats.DaysPlayed + randomSeedAddition);
      List<int> source = new List<int>()
      {
        68,
        66,
        78,
        80,
        86,
        152,
        167,
        153,
        420
      };
      if (Game1.mine != null)
      {
        if (Game1.mine.mineLevel > 40 || Game1.player.timesReachedMineBottom >= 1)
          source.AddRange((IEnumerable<int>) new int[5]
          {
            62,
            70,
            72,
            84,
            422
          });
        if (Game1.mine.mineLevel > 80 || Game1.player.timesReachedMineBottom >= 1)
          source.AddRange((IEnumerable<int>) new int[3]
          {
            64,
            60,
            82
          });
      }
      if (Game1.player.eventsSeen.Contains(61))
        source.AddRange((IEnumerable<int>) new int[4]
        {
          88,
          90,
          164,
          165
        });
      if (Game1.player.craftingRecipes.Keys.Contains<string>("Furnace"))
        source.AddRange((IEnumerable<int>) new int[4]
        {
          334,
          335,
          336,
          338
        });
      if (Game1.player.craftingRecipes.Keys.Contains<string>("Quartz Globe"))
        source.Add(339);
      if (season.Equals("spring"))
        source.AddRange((IEnumerable<int>) new int[16]
        {
          16,
          18,
          20,
          22,
          129,
          131,
          132,
          136,
          137,
          142,
          143,
          145,
          147,
          148,
          152,
          167
        });
      else if (season.Equals("summer"))
        source.AddRange((IEnumerable<int>) new int[17]
        {
          128,
          130,
          131,
          132,
          136,
          138,
          142,
          144,
          145,
          146,
          149,
          150,
          155,
          396,
          398,
          400,
          402
        });
      else if (season.Equals("fall"))
        source.AddRange((IEnumerable<int>) new int[17]
        {
          404,
          406,
          408,
          410,
          129,
          131,
          132,
          136,
          137,
          139,
          140,
          142,
          143,
          148,
          150,
          154,
          155
        });
      else if (season.Equals("winter"))
        source.AddRange((IEnumerable<int>) new int[17]
        {
          412,
          414,
          416,
          418,
          130,
          131,
          132,
          136,
          140,
          141,
          143,
          144,
          146,
          147,
          150,
          151,
          154
        });
      if (forQuest)
      {
        if (Game1.player.coopUpgradeLevel >= 1)
        {
          if (Game1.player.hasCoopDweller("WhiteChicken") || Game1.player.hasCoopDweller("BrownChicken") || Game1.player.hasCoopDweller("Duck"))
            source.Add(-5);
          if (Game1.player.hasCoopDweller("Rabbit"))
            source.Add(440);
        }
        if (Game1.player.BarnUpgradeLevel >= 1)
        {
          if (Game1.player.hasBarnDweller("WhiteBlackCow") || Game1.player.hasBarnDweller("Goat"))
            source.Add(-6);
          if (Game1.player.hasBarnDweller("Sheep") && !source.Contains(440))
            source.Add(440);
          if (Game1.player.hasBarnDweller("Pig"))
            source.Add(430);
        }
        foreach (string key in Game1.player.cookingRecipes.Keys)
        {
          if (random.NextDouble() >= 0.4)
          {
            List<int> intList = Utility.possibleCropsAtThisTime(Game1.currentSeason, Game1.dayOfMonth <= 7);
            Dictionary<string, string> dictionary = Game1.content.Load<Dictionary<string, string>>("Data//CookingRecipes");
            if (dictionary.ContainsKey(key))
            {
              string[] strArray = dictionary[key].Split('/')[0].Split(' ');
              bool flag = true;
              for (int index = 0; index < strArray.Length; ++index)
              {
                if (!source.Contains(Convert.ToInt32(strArray[index])) && !Utility.isCategoryIngredientAvailable(Convert.ToInt32(strArray[index])) && (intList == null || !intList.Contains(Convert.ToInt32(strArray[index]))))
                {
                  flag = false;
                  break;
                }
              }
              if (flag)
                source.Add(Convert.ToInt32(dictionary[key].Split('/')[2]));
            }
          }
        }
      }
      return source.ElementAt<int>(random.Next(source.Count));
    }

    private static bool isCategoryIngredientAvailable(int category)
    {
      return category < 0 && (category != -5 || Game1.player.hasCoopDweller("WhiteChicken") || (Game1.player.hasCoopDweller("BrownChicken") || Game1.player.hasCoopDweller("Duck"))) && (category != -6 || Game1.player.hasBarnDweller("WhiteBlackCow") || Game1.player.hasBarnDweller("Goat"));
    }

    public static int weatherDebrisOffsetForSeason(string season)
    {
      if (season == "spring")
        return 16;
      if (season == "summer")
        return 24;
      if (season == "fall")
        return 18;
      return season == "winter" ? 20 : 0;
    }

    public static Color getSkyColorForSeason(string season)
    {
      if (season == "spring")
        return new Color(92, 170, (int) byte.MaxValue);
      if (season == "summer")
        return new Color(24, 163, (int) byte.MaxValue);
      if (season == "fall")
        return new Color((int) byte.MaxValue, 184, 151);
      if (season == "winter")
        return new Color(165, 207, (int) byte.MaxValue);
      return new Color(92, 170, (int) byte.MaxValue);
    }

    public static void farmerHeardSong(string trackName)
    {
      string str1 = "coin";
      string str2;
      if (trackName.Equals("springsongs"))
      {
        if (Game1.player.songsHeard.Contains("Bouncy"))
          return;
        str2 = "Bouncy";
        str2 = "Wonky Tonk";
        str1 = "Pink Petals";
      }
      else if (trackName.Equals("summersongs"))
      {
        if (Game1.player.songsHeard.Contains("Orange"))
          return;
        str2 = "Tropical Jam";
        str2 = "Orange";
        str1 = "Hometone";
      }
      else if (trackName.Equals("fallsongs"))
      {
        if (Game1.player.songsHeard.Contains("Majestic"))
          return;
        str2 = "Majestic";
        str2 = "Plums";
        str1 = "Ghost Synth";
      }
      else if (trackName.Equals("wintersongs"))
      {
        if (Game1.player.songsHeard.Contains("Ancient"))
          return;
        str2 = "New Snow";
        str2 = "Cyclops";
        str1 = "Ancient";
      }
      else if (trackName.Equals("EarthMine"))
      {
        if (Game1.player.songsHeard.Contains("Cavern"))
          return;
        str2 = "Crystal Bells";
        str2 = "Cavern";
        str1 = "Secret Gnomes";
      }
      else if (trackName.Equals("FrostMine"))
      {
        if (Game1.player.songsHeard.Contains("Cloth"))
          return;
        str2 = "Cloth";
        str2 = "Icicles";
        str1 = "XOR";
      }
      else if (trackName.Equals("LavaMine"))
      {
        if (Game1.player.songsHeard.Contains("Of Dwarves"))
          return;
        str2 = "Near The Planet Core";
        str1 = "Of Dwarves";
      }
      else if (!trackName.Equals("none") && !trackName.Equals("rain"))
        str1 = trackName;
      if (Game1.player.songsHeard.Contains(str1))
        return;
      Game1.player.songsHeard.Add(str1);
    }

    public static int percentGameComplete()
    {
      int num = 0;
      if (Game1.player.spouse != null && !Game1.player.spouse.Contains("engaged"))
        ++num;
      if (Utility.playerHasGalaxySword())
        ++num;
      return num + Utility.itemsShippedPercent() + Utility.upgradePercent() + Utility.friendshipPercent() + Utility.achievementsPercent() + Utility.artifactsPercent() + Utility.fishPercent() + Utility.cosmicFruitPercent() + Utility.cookingPercent() + Utility.craftingPercent() + Utility.minePercentage() + Game1.player.Level;
    }

    public static List<string> getOtherFarmerNames()
    {
      List<string> stringList = new List<string>();
      Random random1 = new Random((int) Game1.uniqueIDForThisGame);
      Random random2 = new Random((int) Game1.uniqueIDForThisGame + (int) Game1.stats.DaysPlayed);
      string[] strArray1 = new string[33]
      {
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5499"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5500"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5501"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5502"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5503"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5504"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5505"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5506"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5507"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5508"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5509"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5510"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5511"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5512"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5513"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5514"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5515"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5516"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5517"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5518"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5519"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5520"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5521"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5522"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5523"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5524"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5525"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5526"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5527"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5528"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5529"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5530"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5531")
      };
      string[] strArray2 = new string[29]
      {
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5532"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5533"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5534"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5535"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5536"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5537"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5538"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5539"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5540"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5541"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5542"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5543"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5544"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5545"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5546"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5547"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5548"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5549"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5550"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5551"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5552"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5553"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5554"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5555"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5556"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5557"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5558"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5559"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5560")
      };
      string[] strArray3 = new string[17]
      {
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5561"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5562"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5563"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5564"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5565"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5566"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5567"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5568"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5569"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5570"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5571"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5572"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5573"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5574"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5575"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5576"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5577")
      };
      string[] strArray4 = new string[12]
      {
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5561"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5562"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5573"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5581"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5582"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5583"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5568"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5585"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5586"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5587"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5588"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5589")
      };
      string[] strArray5 = new string[28]
      {
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5590"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5591"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5592"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5593"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5594"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5595"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5596"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5597"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5598"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5599"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5600"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5601"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5602"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5603"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5604"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5605"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5606"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5607"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5608"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5609"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5610"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5611"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5612"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5613"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5614"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5615"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5616"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5617")
      };
      string[] strArray6 = new string[21]
      {
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5618"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5619"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5620"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5607"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5622"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5623"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5624"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5625"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5626"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5627"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5628"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5629"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5630"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5631"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5632"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5633"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5634"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5635"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5636"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5637"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5638")
      };
      string[] strArray7 = new string[9]
      {
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5639"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5640"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5641"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5642"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5643"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5644"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5645"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5646"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5647")
      };
      string[] strArray8 = new string[4]
      {
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5561"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5568"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5569"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5651")
      };
      string[] strArray9 = new string[4]
      {
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5561"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5568"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5585"),
        Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5655")
      };
      if (Game1.player.isMale)
      {
        string str = strArray1[random1.Next(strArray1.Length)];
        for (int index = 0; index < 2; ++index)
        {
          while (stringList.Contains(str) || Game1.player.name.Equals(str))
            str = index != 0 ? strArray1[random2.Next(strArray1.Length)] : strArray1[random1.Next(strArray1.Length)];
          str = index != 0 ? strArray3[random2.Next(strArray3.Length)] + " " + str : strArray8[random1.Next(strArray8.Length)] + " " + str;
          stringList.Add(str);
        }
      }
      else
      {
        string str = strArray2[random1.Next(strArray2.Length)];
        for (int index = 0; index < 2; ++index)
        {
          while (stringList.Contains(str) || Game1.player.name.Equals(str))
            str = index != 0 ? strArray2[random2.Next(strArray2.Length)] : strArray2[random1.Next(strArray2.Length)];
          str = index != 0 ? strArray4[random2.Next(strArray4.Length)] + " " + str : strArray9[random1.Next(strArray9.Length)] + " " + str;
          stringList.Add(str);
        }
      }
      string str1;
      if (random2.NextDouble() < 0.5)
      {
        string str2 = strArray1[random2.Next(strArray1.Length)];
        while (Game1.player.name.Equals(str2))
          str2 = strArray1[random2.Next(strArray1.Length)];
        str1 = random2.NextDouble() >= 0.5 ? str2 + " " + strArray7[random2.Next(strArray7.Length)] : strArray5[random2.Next(strArray5.Length)] + " " + str2;
      }
      else
      {
        string str2 = strArray2[random2.Next(strArray2.Length)];
        while (Game1.player.name.Equals(str2))
          str2 = strArray2[random2.Next(strArray2.Length)];
        str1 = strArray6[random2.Next(strArray6.Length)] + " " + str2;
      }
      stringList.Add(str1);
      return stringList;
    }

    public static string getStardewHeroStandingsString()
    {
      string str = "";
      Random random = new Random((int) Game1.uniqueIDForThisGame + (int) Game1.stats.DaysPlayed);
      List<string> otherFarmerNames = Utility.getOtherFarmerNames();
      int[] numArray = new int[otherFarmerNames.Count];
      numArray[0] = (int) ((double) Game1.stats.DaysPlayed / 208.0 * (double) Game1.percentageToWinStardewHero);
      numArray[1] = (int) ((double) numArray[0] * 0.75 + (double) random.Next(-5, 5));
      numArray[2] = Math.Max(0, numArray[1] / 2 + random.Next(-10, 0));
      if (Game1.stats.DaysPlayed < 30U)
        numArray[0] += 3;
      else if (Game1.stats.DaysPlayed < 60U)
        numArray[0] += 7;
      int num = Utility.percentGameComplete();
      bool flag = false;
      for (int index = 0; index < 3; ++index)
      {
        if (num > numArray[index] && !flag)
        {
          flag = true;
          str = str + Game1.player.getTitle() + " " + Game1.player.Name + " ....... " + (object) num + Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5657") + Environment.NewLine;
        }
        str = str + otherFarmerNames[index] + " ....... " + (object) numArray[index] + Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5657") + Environment.NewLine;
      }
      if (!flag)
        str = str + Game1.player.getTitle() + " " + Game1.player.Name + " ....... " + (object) num + Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5657");
      return str;
    }

    private static int cosmicFruitPercent()
    {
      return Math.Max(0, (Game1.player.MaxStamina - 120) / 20);
    }

    private static int minePercentage()
    {
      if (Game1.player.timesReachedMineBottom > 0)
        return 4;
      if (Game1.mine != null && Game1.mine.mineLevel >= 80)
        return 2;
      return Game1.mine != null && Game1.mine.mineLevel >= 40 ? 1 : 0;
    }

    private static int cookingPercent()
    {
      int num = 0;
      foreach (string key in Game1.player.cookingRecipes.Keys)
      {
        if (Game1.player.cookingRecipes[key] > 0)
          ++num;
      }
      return (int) ((double) (num / Game1.content.Load<Dictionary<string, string>>("Data\\CookingRecipes").Count) * 3.0);
    }

    private static int craftingPercent()
    {
      int num = 0;
      foreach (string key in Game1.player.craftingRecipes.Keys)
      {
        if (Game1.player.craftingRecipes[key] > 0)
          ++num;
      }
      return (int) ((double) (num / Game1.content.Load<Dictionary<string, string>>("Data\\CraftingRecipes").Count) * 3.0);
    }

    private static int achievementsPercent()
    {
      return (int) ((double) (Game1.player.achievements.Count / Game1.content.Load<Dictionary<int, string>>("Data\\achievements").Count) * 15.0);
    }

    private static int itemsShippedPercent()
    {
      return (int) ((double) Game1.player.basicShipped.Count / 92.0 * 5.0);
    }

    private static int artifactsPercent()
    {
      return (int) ((double) Game1.player.archaeologyFound.Count / 32.0 * 3.0);
    }

    private static int fishPercent()
    {
      return (int) ((double) Game1.player.fishCaught.Count / 42.0 * 3.0);
    }

    private static int upgradePercent()
    {
      int num1 = 0;
      foreach (Item obj in Game1.player.items)
      {
        if (obj != null && obj.GetType() == typeof (Tool) && (obj.Name.Contains("Hoe") || obj.Name.Contains("Axe") || (obj.Name.Contains("Pickaxe") || obj.Name.Contains("Can"))) && ((Tool) obj).upgradeLevel == 4)
          ++num1;
      }
      foreach (Tool tool in Game1.player.toolBox)
      {
        if (tool != null && (tool.name.Contains("Hoe") || tool.name.Contains("Axe") || (tool.name.Contains("Pickaxe") || tool.name.Contains("Can"))) && tool.upgradeLevel == 4)
          ++num1;
      }
      int num2 = num1 + Game1.player.HouseUpgradeLevel + Game1.player.CoopUpgradeLevel + Game1.player.BarnUpgradeLevel;
      if (Game1.player.hasGreenhouse)
        ++num2;
      return num2;
    }

    private static int friendshipPercent()
    {
      int num = 0;
      foreach (string key in Game1.player.friendships.Keys)
        num += Game1.player.friendships[key][0];
      return Math.Min(10, (int) ((double) num / 70000.0 * 10.0));
    }

    private static bool playerHasGalaxySword()
    {
      foreach (Item obj in Game1.player.Items)
      {
        if (obj != null && obj.GetType() == typeof (Sword) && obj.Name.Contains("Galaxy"))
          return true;
      }
      foreach (Tool tool in Game1.player.toolBox)
      {
        if (tool != null && tool.name.Contains("Galaxy"))
          return true;
      }
      return false;
    }

    public static Quest getQuestOfTheDay()
    {
      if (Game1.stats.DaysPlayed <= 1U)
        return (Quest) null;
      double num = new Random((int) Game1.uniqueIDForThisGame + (int) Game1.stats.DaysPlayed).NextDouble();
      return num >= 0.08 ? (num >= 0.18 || Game1.mine == null || Game1.stats.DaysPlayed <= 5U ? (num >= 0.53 ? (num >= 0.6 ? (Quest) new ItemDeliveryQuest() : (Quest) new FishingQuest()) : (Quest) null) : (Quest) new SlayMonsterQuest()) : (Quest) new ResourceCollectionQuest();
    }

    public static Color getOppositeColor(Color color)
    {
      return new Color((int) byte.MaxValue - (int) color.R, (int) byte.MaxValue - (int) color.G, (int) byte.MaxValue - (int) color.B);
    }

    public static void drawLightningBolt(Vector2 strikePosition, GameLocation l)
    {
      Microsoft.Xna.Framework.Rectangle sourceRect = new Microsoft.Xna.Framework.Rectangle(644, 1078, 37, 57);
      Vector2 position = strikePosition + new Vector2((float) (-sourceRect.Width * Game1.pixelZoom / 2), (float) (-sourceRect.Height * Game1.pixelZoom));
      while ((double) position.Y > (double) (-sourceRect.Height * Game1.pixelZoom))
      {
        l.temporarySprites.Add(new TemporaryAnimatedSprite(Game1.mouseCursors, sourceRect, 9999f, 1, 999, position, false, Game1.random.NextDouble() < 0.5, (float) (((double) strikePosition.Y + (double) (Game1.tileSize / 2)) / 10000.0 + 1.0 / 1000.0), 0.025f, Color.White, (float) Game1.pixelZoom, 0.0f, 0.0f, 0.0f, false)
        {
          light = true,
          lightRadius = 2f,
          delayBeforeAnimationStart = 200,
          lightcolor = Color.Black
        });
        position.Y -= (float) (sourceRect.Height * Game1.pixelZoom);
      }
    }

    public static string getDateStringFor(int currentDay, int currentSeason, int currentYear)
    {
      if (currentDay <= 0)
      {
        currentDay += 28;
        --currentSeason;
        if (currentSeason < 0)
        {
          currentSeason = 3;
          --currentYear;
        }
      }
      else if (currentDay > 28)
      {
        currentDay -= 28;
        ++currentSeason;
        if (currentSeason > 3)
        {
          currentSeason = 0;
          ++currentYear;
        }
      }
      if (currentYear == 0)
        return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5677");
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5678", (object) currentDay, (object) (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.es ? Utility.getSeasonNameFromNumber(currentSeason).ToLower() : Utility.getSeasonNameFromNumber(currentSeason)), (object) currentYear);
    }

    public static string getDateString(int offset = 0)
    {
      int dayOfMonth = Game1.dayOfMonth;
      int seasonNumber = Utility.getSeasonNumber(Game1.currentSeason);
      int year = Game1.year;
      int num = offset;
      return Utility.getDateStringFor(dayOfMonth + num, seasonNumber, year);
    }

    public static string getYesterdaysDate()
    {
      return Utility.getDateString(-1);
    }

    public static string getSeasonNameFromNumber(int number)
    {
      switch (number)
      {
        case 0:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5680");
        case 1:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5681");
        case 2:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5682");
        case 3:
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5683");
        default:
          return "";
      }
    }

    public static string getNumberEnding(int number)
    {
      if (number % 100 > 10 && number % 100 < 20)
        return "th";
      switch (number % 10)
      {
        case 0:
        case 4:
        case 5:
        case 6:
        case 7:
        case 8:
        case 9:
          return "th";
        case 1:
          return "st";
        case 2:
          return "nd";
        case 3:
          return "rd";
        default:
          return "";
      }
    }

    public static void killAllStaticLoopingSoundCues()
    {
      if (Game1.soundBank == null)
        return;
      if (Intro.roadNoise != null)
        Intro.roadNoise.Stop(AudioStopOptions.Immediate);
      if (Fly.buzz != null)
        Fly.buzz.Stop(AudioStopOptions.Immediate);
      if (Railroad.trainLoop != null)
        Railroad.trainLoop.Stop(AudioStopOptions.Immediate);
      if (BobberBar.reelSound != null)
        BobberBar.reelSound.Stop(AudioStopOptions.Immediate);
      if (BobberBar.unReelSound != null)
        BobberBar.unReelSound.Stop(AudioStopOptions.Immediate);
      if (FishingRod.reelSound != null)
        FishingRod.reelSound.Stop(AudioStopOptions.Immediate);
      if (Game1.fuseSound == null)
        return;
      Game1.fuseSound.Stop(AudioStopOptions.Immediate);
    }

    public static void consolidateStacks(List<Item> objects)
    {
      for (int index1 = 0; index1 < objects.Count; ++index1)
      {
        if (objects[index1] != null && objects[index1] is Object)
        {
          Object @object = objects[index1] as Object;
          for (int index2 = index1 + 1; index2 < objects.Count; ++index2)
          {
            if (objects[index2] != null && @object.canStackWith(objects[index2]))
            {
              @object.Stack = objects[index2].addToStack(@object.Stack);
              if (@object.Stack <= 0)
                break;
            }
          }
        }
      }
      for (int index = objects.Count - 1; index >= 0; --index)
      {
        if (objects[index] != null && objects[index].Stack <= 0)
          objects.RemoveAt(index);
      }
    }

    public static void performLightningUpdate()
    {
      Random random = new Random((int) Game1.uniqueIDForThisGame + (int) Game1.stats.DaysPlayed + Game1.timeOfDay);
      if (random.NextDouble() < 0.125 + Game1.dailyLuck + (double) Game1.player.luckLevel / 100.0)
      {
        if (Game1.currentLocation.IsOutdoors && !(Game1.currentLocation is Desert) && !Game1.newDay)
        {
          Game1.flashAlpha = (float) (0.5 + random.NextDouble());
          Game1.playSound("thunder");
        }
        GameLocation locationFromName = Game1.getLocationFromName("Farm");
        List<Vector2> source = new List<Vector2>();
        foreach (KeyValuePair<Vector2, Object> keyValuePair in (Dictionary<Vector2, Object>) locationFromName.objects)
        {
          if (keyValuePair.Value.bigCraftable && keyValuePair.Value.ParentSheetIndex == 9)
            source.Add(keyValuePair.Key);
        }
        if (source.Count > 0)
        {
          for (int index1 = 0; index1 < 2; ++index1)
          {
            Vector2 index2 = source.ElementAt<Vector2>(random.Next(source.Count));
            if (locationFromName.objects[index2].heldObject == null)
            {
              locationFromName.objects[index2].heldObject = new Object(787, 1, false, -1, 0);
              locationFromName.objects[index2].minutesUntilReady = 3000 - Game1.timeOfDay;
              locationFromName.objects[index2].shakeTimer = 1000;
              if (!(Game1.currentLocation is Farm))
                return;
              Utility.drawLightningBolt(index2 * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), 0.0f), locationFromName);
              return;
            }
          }
        }
        if (random.NextDouble() >= 0.25 - Game1.dailyLuck - (double) Game1.player.luckLevel / 100.0)
          return;
        try
        {
          KeyValuePair<Vector2, TerrainFeature> keyValuePair = locationFromName.terrainFeatures.ElementAt<KeyValuePair<Vector2, TerrainFeature>>(random.Next(locationFromName.terrainFeatures.Count));
          if (!(keyValuePair.Value is FruitTree) && keyValuePair.Value.performToolAction((Tool) null, 50, keyValuePair.Key, locationFromName))
          {
            locationFromName.terrainFeatures.Remove(keyValuePair.Key);
            if (!Game1.currentLocation.name.Equals("Farm"))
              return;
            locationFromName.temporarySprites.Add(new TemporaryAnimatedSprite(362, 75f, 6, 1, keyValuePair.Key, false, false));
            Utility.drawLightningBolt(keyValuePair.Key * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), (float) (-Game1.tileSize * 2)), locationFromName);
          }
          else
          {
            if (!(keyValuePair.Value is FruitTree))
              return;
            (keyValuePair.Value as FruitTree).struckByLightningCountdown = 4;
            (keyValuePair.Value as FruitTree).shake(keyValuePair.Key, true);
            Utility.drawLightningBolt(keyValuePair.Key * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), (float) (-Game1.tileSize * 2)), locationFromName);
          }
        }
        catch (Exception ex)
        {
        }
      }
      else
      {
        if (random.NextDouble() >= 0.1 || !Game1.currentLocation.IsOutdoors || (Game1.currentLocation is Desert || Game1.newDay))
          return;
        Game1.flashAlpha = (float) (0.5 + random.NextDouble());
        if (random.NextDouble() < 0.5)
          DelayedAction.screenFlashAfterDelay((float) (0.3 + random.NextDouble()), random.Next(500, 1000), "");
        DelayedAction.playSoundAfterDelay("thunder_small", random.Next(500, 1500));
      }
    }

    public static void overnightLightning()
    {
      int num = (2400 - Game1.timeOfDay) / 100;
      for (int index = 0; index < num; ++index)
        Utility.performLightningUpdate();
    }

    public static List<Vector2> getAdjacentTileLocations(Vector2 tileLocation)
    {
      return new List<Vector2>()
      {
        new Vector2(-1f, 0.0f) + tileLocation,
        new Vector2(1f, 0.0f) + tileLocation,
        new Vector2(0.0f, 1f) + tileLocation,
        new Vector2(0.0f, -1f) + tileLocation
      };
    }

    public static List<Point> getAdjacentTilePoints(float xTile, float yTile)
    {
      List<Point> pointList = new List<Point>();
      int x = (int) xTile;
      int y = (int) yTile;
      Point point1 = new Point(x - 1, y);
      pointList.Add(point1);
      Point point2 = new Point(1 + x, y);
      pointList.Add(point2);
      Point point3 = new Point(x, 1 + y);
      pointList.Add(point3);
      Point point4 = new Point(x, y - 1);
      pointList.Add(point4);
      return pointList;
    }

    public static Vector2[] getAdjacentTileLocationsArray(Vector2 tileLocation)
    {
      return new Vector2[4]
      {
        new Vector2(-1f, 0.0f) + tileLocation,
        new Vector2(1f, 0.0f) + tileLocation,
        new Vector2(0.0f, 1f) + tileLocation,
        new Vector2(0.0f, -1f) + tileLocation
      };
    }

    public static Vector2[] getDiagonalTileLocationsArray(Vector2 tileLocation)
    {
      return new Vector2[4]
      {
        new Vector2(-1f, -1f) + tileLocation,
        new Vector2(1f, 1f) + tileLocation,
        new Vector2(-1f, 1f) + tileLocation,
        new Vector2(1f, -1f) + tileLocation
      };
    }

    public static Vector2[] getSurroundingTileLocationsArray(Vector2 tileLocation)
    {
      return new Vector2[8]
      {
        new Vector2(-1f, 0.0f) + tileLocation,
        new Vector2(1f, 0.0f) + tileLocation,
        new Vector2(0.0f, 1f) + tileLocation,
        new Vector2(0.0f, -1f) + tileLocation,
        new Vector2(-1f, -1f) + tileLocation,
        new Vector2(1f, -1f) + tileLocation,
        new Vector2(1f, 1f) + tileLocation,
        new Vector2(-1f, 1f) + tileLocation
      };
    }

    public static Crop findCloseFlower(Vector2 startTileLocation)
    {
      Queue<Vector2> vector2Queue = new Queue<Vector2>();
      HashSet<Vector2> vector2Set = new HashSet<Vector2>();
      Farm locationFromName = Game1.getLocationFromName("Farm") as Farm;
      vector2Queue.Enqueue(startTileLocation);
      for (int index1 = 0; index1 <= 150 && vector2Queue.Count > 0; ++index1)
      {
        Vector2 index2 = vector2Queue.Dequeue();
        if (locationFromName.terrainFeatures.ContainsKey(index2) && locationFromName.terrainFeatures[index2] is HoeDirt && ((locationFromName.terrainFeatures[index2] as HoeDirt).crop != null && (locationFromName.terrainFeatures[index2] as HoeDirt).crop.programColored) && ((locationFromName.terrainFeatures[index2] as HoeDirt).crop.currentPhase >= (locationFromName.terrainFeatures[index2] as HoeDirt).crop.phaseDays.Count - 1 && !(locationFromName.terrainFeatures[index2] as HoeDirt).crop.dead))
          return (locationFromName.terrainFeatures[index2] as HoeDirt).crop;
        foreach (Vector2 adjacentTileLocation in Utility.getAdjacentTileLocations(index2))
        {
          if (!vector2Set.Contains(adjacentTileLocation))
            vector2Queue.Enqueue(adjacentTileLocation);
        }
        vector2Set.Add(index2);
      }
      return (Crop) null;
    }

    public static Point findCloseMatureCrop(Vector2 startTileLocation)
    {
      Queue<Vector2> source = new Queue<Vector2>();
      HashSet<Vector2> vector2Set = new HashSet<Vector2>();
      Farm locationFromName = Game1.getLocationFromName("Farm") as Farm;
      source.Enqueue(startTileLocation);
      for (int index1 = 0; index1 <= 40 && source.Count<Vector2>() > 0; ++index1)
      {
        Vector2 index2 = source.Dequeue();
        if (locationFromName.terrainFeatures.ContainsKey(index2) && locationFromName.terrainFeatures[index2] is HoeDirt && ((locationFromName.terrainFeatures[index2] as HoeDirt).crop != null && (locationFromName.terrainFeatures[index2] as HoeDirt).readyForHarvest()))
          return Utility.Vector2ToPoint(index2);
        foreach (Vector2 adjacentTileLocation in Utility.getAdjacentTileLocations(index2))
        {
          if (!vector2Set.Contains(adjacentTileLocation))
            source.Enqueue(adjacentTileLocation);
        }
        vector2Set.Add(index2);
      }
      return Point.Zero;
    }

    public static void recursiveFenceBuild(Vector2 position, int direction, GameLocation location, Random r)
    {
      if (r.NextDouble() < 0.04 || location.objects.ContainsKey(position) || !location.isTileLocationOpen(new Location((int) position.X * Game1.tileSize, (int) position.Y * Game1.tileSize)))
        return;
      location.objects.Add(position, (Object) new Fence(position, 1, false));
      int direction1 = direction;
      if (r.NextDouble() < 0.16)
        direction1 = r.Next(4);
      if (direction1 == (direction + 2) % 4)
        direction1 = (direction1 + 1) % 4;
      switch (direction)
      {
        case 0:
          Utility.recursiveFenceBuild(position + new Vector2(0.0f, -1f), direction1, location, r);
          break;
        case 1:
          Utility.recursiveFenceBuild(position + new Vector2(1f, 0.0f), direction1, location, r);
          break;
        case 2:
          Utility.recursiveFenceBuild(position + new Vector2(0.0f, 1f), direction1, location, r);
          break;
        case 3:
          Utility.recursiveFenceBuild(position + new Vector2(-1f, 0.0f), direction1, location, r);
          break;
      }
    }

    public static bool addAnimalToFarm(FarmAnimal animal)
    {
      if (animal == null || animal.sprite == null)
        return false;
      foreach (Building building in ((BuildableGameLocation) Game1.getLocationFromName("Farm")).buildings)
      {
        if (building.buildingType.Contains(animal.buildingTypeILiveIn))
        {
          ((AnimalHouse) building.indoors).animals.Add(animal.myID, animal);
          animal.setRandomPosition(building.indoors);
          return true;
        }
      }
      return false;
    }

    public static Vector2 getAwayFromPlayerTrajectory(Microsoft.Xna.Framework.Rectangle monsterBox)
    {
      return Utility.getAwayFromPlayerTrajectory(monsterBox, Game1.player);
    }

    public static Item getItemFromStandardTextDescription(string description, Farmer who, char delimiter = ' ')
    {
      string[] strArray = description.Split(delimiter);
      int index1 = 0;
      string s = strArray[index1];
      int index2 = 1;
      int int32_1 = Convert.ToInt32(strArray[index2]);
      int index3 = 2;
      int int32_2 = Convert.ToInt32(strArray[index3]);
      Item obj = (Item) null;
      // ISSUE: reference to a compiler-generated method
      uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(s);
      if (stringHash <= 3082879841U)
      {
        if (stringHash <= 1430892386U)
        {
          if (stringHash <= 568155902U)
          {
            if ((int) stringHash != 551378283)
            {
              if ((int) stringHash != 568155902 || !(s == "BO"))
                goto label_46;
            }
            else if (s == "BL")
              goto label_43;
            else
              goto label_46;
          }
          else if ((int) stringHash != 930363637)
          {
            if ((int) stringHash != 1430892386 || !(s == "Hat"))
              goto label_46;
            else
              goto label_44;
          }
          else if (s == "Boot")
            goto label_41;
          else
            goto label_46;
        }
        else if (stringHash <= 2089749334U)
        {
          if ((int) stringHash != 2005354379)
          {
            if ((int) stringHash != 2089749334 || !(s == "BigObject"))
              goto label_46;
          }
          else if (s == "Ring")
            goto label_40;
          else
            goto label_46;
        }
        else if ((int) stringHash != -1619739605)
        {
          if ((int) stringHash != -1587019264)
          {
            if ((int) stringHash != -1212087455 || !(s == "Weapon"))
              goto label_46;
            else
              goto label_42;
          }
          else if (s == "Blueprint")
            goto label_43;
          else
            goto label_46;
        }
        else if (s == "BBL")
          goto label_45;
        else
          goto label_46;
        obj = (Item) new Object(Vector2.Zero, int32_1, false);
        goto label_46;
label_43:
        obj = (Item) new Object(int32_1, 1, true, -1, 0);
        goto label_46;
      }
      else
      {
        if (stringHash <= 3440116983U)
        {
          if (stringHash <= 3272340793U)
          {
            if ((int) stringHash != -1082855797)
            {
              if ((int) stringHash != -1022626503 || !(s == "F"))
                goto label_46;
            }
            else if (s == "BBl")
              goto label_45;
            else
              goto label_46;
          }
          else if ((int) stringHash != -955516027)
          {
            if ((int) stringHash != -905183170)
            {
              if ((int) stringHash != -854850313 || !(s == "H"))
                goto label_46;
              else
                goto label_44;
            }
            else if (s == "O")
              goto label_38;
            else
              goto label_46;
          }
          else if (s == "B")
            goto label_41;
          else
            goto label_46;
        }
        else if (stringHash <= 3579274004U)
        {
          if ((int) stringHash != -770962218)
          {
            if ((int) stringHash != -715693292 || !(s == "BigBlueprint"))
              goto label_46;
            else
              goto label_45;
          }
          else if (s == "W")
            goto label_42;
          else
            goto label_46;
        }
        else if ((int) stringHash != -687074123)
        {
          if ((int) stringHash != -443652902)
          {
            if ((int) stringHash != -69378299 || !(s == "Furniture"))
              goto label_46;
          }
          else if (s == "Object")
            goto label_38;
          else
            goto label_46;
        }
        else if (s == "R")
          goto label_40;
        else
          goto label_46;
        obj = (Item) new Furniture(int32_1, Vector2.Zero);
        goto label_46;
label_38:
        obj = (Item) new Object(int32_1, 1, false, -1, 0);
        goto label_46;
      }
label_40:
      obj = (Item) new Ring(int32_1);
      goto label_46;
label_41:
      obj = (Item) new Boots(int32_1);
      goto label_46;
label_42:
      obj = (Item) new MeleeWeapon(int32_1);
      goto label_46;
label_44:
      obj = (Item) new Hat(int32_1);
      goto label_46;
label_45:
      obj = (Item) new Object(Vector2.Zero, int32_1, true);
label_46:
      if (!s.Equals("BO") && !s.Equals("BigObject"))
        obj.Stack = int32_2;
      if (obj is Object && (obj as Object).isRecipe && who.knowsRecipe(obj.Name))
        return (Item) null;
      return obj;
    }

    public static List<TemporaryAnimatedSprite> sparkleWithinArea(Microsoft.Xna.Framework.Rectangle bounds, int numberOfSparkles, Color sparkleColor, int delayBetweenSparkles = 100, int delayBeforeStarting = 0, string sparkleSound = "")
    {
      return Utility.getTemporarySpritesWithinArea(new int[2]
      {
        10,
        11
      }, bounds, numberOfSparkles, sparkleColor, delayBetweenSparkles, delayBeforeStarting, sparkleSound);
    }

    public static List<TemporaryAnimatedSprite> getTemporarySpritesWithinArea(int[] temporarySpriteRowNumbers, Microsoft.Xna.Framework.Rectangle bounds, int numberOfsprites, Color color, int delayBetweenSprites = 100, int delayBeforeStarting = 0, string sound = "")
    {
      List<TemporaryAnimatedSprite> temporaryAnimatedSpriteList = new List<TemporaryAnimatedSprite>();
      for (int index = 0; index < numberOfsprites; ++index)
        temporaryAnimatedSpriteList.Add(new TemporaryAnimatedSprite(temporarySpriteRowNumbers[Game1.random.Next(temporarySpriteRowNumbers.Length)], new Vector2((float) Game1.random.Next(bounds.X, bounds.Right), (float) Game1.random.Next(bounds.Y, bounds.Bottom)), color, 8, false, 100f, 0, -1, -1f, -1, 0)
        {
          delayBeforeAnimationStart = delayBeforeStarting + delayBetweenSprites * index,
          startSound = sound.Length > 0 ? sound : (string) null
        });
      return temporaryAnimatedSpriteList;
    }

    public static Vector2 getAwayFromPlayerTrajectory(Microsoft.Xna.Framework.Rectangle monsterBox, Farmer who)
    {
      if (who == null)
        who = Game1.player;
      Microsoft.Xna.Framework.Rectangle boundingBox = who.GetBoundingBox();
      double num1 = (double) -(boundingBox.Center.X - monsterBox.Center.X);
      boundingBox = who.GetBoundingBox();
      float num2 = (float) (boundingBox.Center.Y - monsterBox.Center.Y);
      float num3 = Math.Abs((float) num1) + Math.Abs(num2);
      if ((double) num3 < 1.0)
        num3 = 5f;
      double num4 = (double) num3;
      return new Vector2((float) (num1 / num4) * (float) (50 + Game1.random.Next(-20, 20)), num2 / num3 * (float) (50 + Game1.random.Next(-20, 20)));
    }

    public static string getSongTitleFromCueName(string cueName)
    {
      string lower = cueName.ToLower();
      // ISSUE: reference to a compiler-generated method
      uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(lower);
      if (stringHash <= 2381310912U)
      {
        if (stringHash <= 1014963253U)
        {
          if (stringHash <= 513174751U)
          {
            if (stringHash <= 311403980U)
            {
              if (stringHash <= 31721691U)
              {
                if ((int) stringHash != 14925162)
                {
                  if ((int) stringHash == 31721691 && lower == "poppy")
                    return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5806");
                  goto label_247;
                }
                else
                {
                  if (lower == "cowboy_boss")
                    return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5855");
                  goto label_247;
                }
              }
              else if ((int) stringHash != 214487563)
              {
                if ((int) stringHash != 295673152)
                {
                  if ((int) stringHash == 311403980 && lower == "spirits_eve")
                    return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5788");
                  goto label_247;
                }
                else
                {
                  if (lower == "heavy")
                    return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5794");
                  goto label_247;
                }
              }
              else
              {
                if (lower == "cloth")
                  return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5756");
                goto label_247;
              }
            }
            else if (stringHash <= 369403945U)
            {
              if ((int) stringHash != 322927272)
              {
                if ((int) stringHash == 369403945 && lower == "flowerdance")
                  return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5764");
                goto label_247;
              }
              else
              {
                if (lower == "moonlightjellies")
                  return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5812");
                goto label_247;
              }
            }
            else if ((int) stringHash != 437551800)
            {
              if ((int) stringHash != 461776425)
              {
                if ((int) stringHash == 513174751 && lower == "summer1")
                  return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5740");
                goto label_247;
              }
              else
              {
                if (lower == "marnieshop")
                  return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5814");
                goto label_247;
              }
            }
            else
            {
              if (lower == "emilydance")
                return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5864_5");
              goto label_247;
            }
          }
          else if (stringHash <= 613046764U)
          {
            if (stringHash <= 545110351U)
            {
              if ((int) stringHash != 529952370)
              {
                if ((int) stringHash == 545110351 && lower == "cowboy_overworld")
                  return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5849");
                goto label_247;
              }
              else
              {
                if (lower == "summer2")
                  return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5742");
                goto label_247;
              }
            }
            else if ((int) stringHash != 546729989)
            {
              if ((int) stringHash != 575982768)
              {
                if ((int) stringHash == 613046764 && lower == "musicboxsong")
                  return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5864_8");
                goto label_247;
              }
              else if (!(lower == "title_day"))
                goto label_247;
            }
            else
            {
              if (lower == "summer3")
                return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5744");
              goto label_247;
            }
          }
          else if (stringHash <= 906885789U)
          {
            if ((int) stringHash != 766748620)
            {
              if ((int) stringHash == 906885789 && lower == "near the planet core")
                return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5738");
              goto label_247;
            }
            else
            {
              if (lower == "of dwarves")
                return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5760");
              goto label_247;
            }
          }
          else if ((int) stringHash != 971922320)
          {
            if ((int) stringHash != 1003090502)
            {
              if ((int) stringHash == 1014963253 && lower == "sampractice")
                return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5800");
              goto label_247;
            }
            else
            {
              if (lower == "abigailflute")
                return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5726");
              goto label_247;
            }
          }
          else
          {
            if (lower == "shimmeringbastion")
              return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5792");
            goto label_247;
          }
        }
        else if (stringHash <= 1422361652U)
        {
          if (stringHash <= 1203340822U)
          {
            if (stringHash <= 1150398120U)
            {
              if ((int) stringHash != 1104502631)
              {
                if ((int) stringHash == 1150398120 && lower == "distantbanjo")
                  return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5843");
                goto label_247;
              }
              else
              {
                if (lower == "gusviolin")
                  return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5833");
                goto label_247;
              }
            }
            else if ((int) stringHash != 1176080900)
            {
              if ((int) stringHash != 1186563203)
              {
                if ((int) stringHash == 1203340822 && lower == "fall2")
                  return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5748");
                goto label_247;
              }
              else
              {
                if (lower == "fall1")
                  return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5746");
                goto label_247;
              }
            }
            else
            {
              if (lower == "jojaOfficeSoundscape")
                return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5863");
              goto label_247;
            }
          }
          else if (stringHash <= 1220118441U)
          {
            if ((int) stringHash != 1216379696)
            {
              if ((int) stringHash == 1220118441 && lower == "fall3")
                return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5750");
              goto label_247;
            }
            else
            {
              if (lower == "overcast")
                return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5810");
              goto label_247;
            }
          }
          else if ((int) stringHash != 1266391031)
          {
            if ((int) stringHash != 1403190614)
            {
              if ((int) stringHash == 1422361652 && lower == "title_night")
                return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5821");
              goto label_247;
            }
            else
            {
              if (lower == "woodstheme")
                return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5774");
              goto label_247;
            }
          }
          else
          {
            if (lower == "wedding")
              return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5776");
            goto label_247;
          }
        }
        else if (stringHash <= 1637564208U)
        {
          if (stringHash <= 1505686774U)
          {
            if ((int) stringHash != 1434728751)
            {
              if ((int) stringHash == 1505686774 && lower == "playful")
                return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5808");
              goto label_247;
            }
            else
            {
              if (lower == "sweet")
                return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5864_2");
              goto label_247;
            }
          }
          else if ((int) stringHash != 1611928003)
          {
            if ((int) stringHash != 1626719312)
            {
              if ((int) stringHash == 1637564208 && lower == "kindadumbautumn")
                return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5827");
              goto label_247;
            }
            else
            {
              if (lower == "cavern")
                return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5859");
              goto label_247;
            }
          }
          else
          {
            if (lower == "buglevelloop")
              return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5864_9");
            goto label_247;
          }
        }
        else if (stringHash <= 1898587220U)
        {
          if ((int) stringHash != 1833789906)
          {
            if ((int) stringHash != 1855523881)
            {
              if ((int) stringHash == 1898587220 && lower == "sadpiano")
                return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5804");
              goto label_247;
            }
            else
            {
              if (lower == "50s")
                return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5724");
              goto label_247;
            }
          }
          else
          {
            if (lower == "secret gnomes")
              return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5798");
            goto label_247;
          }
        }
        else if ((int) stringHash != -1989566623)
        {
          if ((int) stringHash != -1977105332)
          {
            if ((int) stringHash == -1913656384 && lower == "wavy")
              return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5861");
            goto label_247;
          }
          else
          {
            if (lower == "starshoot")
              return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5784");
            goto label_247;
          }
        }
        else
        {
          if (lower == "shanetheme")
            return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5864_3");
          goto label_247;
        }
      }
      else if (stringHash <= 3366607222U)
      {
        if (stringHash <= 2966460075U)
        {
          if (stringHash <= 2720137060U)
          {
            if (stringHash <= 2549047255U)
            {
              if ((int) stringHash != -1869675188)
              {
                if ((int) stringHash == -1745920041 && lower == "aerobics")
                  return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5730");
                goto label_247;
              }
              else
              {
                if (lower == "cloudcountry")
                  return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5758");
                goto label_247;
              }
            }
            else if ((int) stringHash != -1740261688)
            {
              if ((int) stringHash != -1693600679)
              {
                if ((int) stringHash == -1574830236 && lower == "christmastheme")
                  return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5864_7");
                goto label_247;
              }
              else
              {
                if (lower == "tribal")
                  return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5778");
                goto label_247;
              }
            }
            else
            {
              if (lower == "saloon1")
                return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5802");
              goto label_247;
            }
          }
          else if (stringHash <= 2844201942U)
          {
            if ((int) stringHash != -1475759312)
            {
              if ((int) stringHash == -1450765354 && lower == "emilytheme")
                return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5864_4");
              goto label_247;
            }
            else
            {
              if (lower == "jaunty")
                return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5829");
              goto label_247;
            }
          }
          else if ((int) stringHash != -1418313515)
          {
            if ((int) stringHash != -1333700075)
            {
              if ((int) stringHash == -1328507221 && lower == "crystal bells")
                return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5847");
              goto label_247;
            }
            else
            {
              if (lower == "librarytheme")
                return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5823");
              goto label_247;
            }
          }
          else
          {
            if (lower == "echos")
              return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5841");
            goto label_247;
          }
        }
        else if (stringHash <= 3121092129U)
        {
          if (stringHash <= 2998131962U)
          {
            if ((int) stringHash != -1313612953)
            {
              if ((int) stringHash == -1296835334 && lower == "spring2")
                return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5720");
              goto label_247;
            }
            else
            {
              if (lower == "spring1")
                return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5718");
              goto label_247;
            }
          }
          else if ((int) stringHash != -1280057715)
          {
            if ((int) stringHash != -1227184899)
            {
              if ((int) stringHash == -1173875167 && lower == "elliottpiano")
                return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5839");
              goto label_247;
            }
            else if (!(lower == "maintheme"))
              goto label_247;
          }
          else
          {
            if (lower == "spring3")
              return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5722");
            goto label_247;
          }
        }
        else if (stringHash <= 3293919832U)
        {
          if ((int) stringHash != -1171500888)
          {
            if ((int) stringHash != -1070580973)
            {
              if ((int) stringHash == -1001047464 && lower == "ragtime")
                return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5864_1");
              goto label_247;
            }
            else
            {
              if (lower == "ticktock")
                return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5782");
              goto label_247;
            }
          }
          else
          {
            if (lower == "springtown")
              return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5786");
            goto label_247;
          }
        }
        else if ((int) stringHash != -990527197)
        {
          if ((int) stringHash != -966241400)
          {
            if ((int) stringHash == -928360074 && lower == "settlingin")
              return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5796");
            goto label_247;
          }
          else
          {
            if (lower == "desolate")
              return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5845");
            goto label_247;
          }
        }
        else
        {
          if (lower == "wizardsong")
            return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5772");
          goto label_247;
        }
      }
      else if (stringHash <= 3570958826U)
      {
        if (stringHash <= 3460154593U)
        {
          if (stringHash <= 3406989666U)
          {
            if ((int) stringHash != -921421213)
            {
              if ((int) stringHash == -887977630 && lower == "breezy")
                return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5752");
              goto label_247;
            }
            else
            {
              if (lower == "tinymusicbox")
                return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5780");
              goto label_247;
            }
          }
          else if ((int) stringHash != -865346690)
          {
            if ((int) stringHash != -837952024)
            {
              if ((int) stringHash == -834812703 && lower == "grandpas_theme")
                return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5762");
              goto label_247;
            }
            else
            {
              if (lower == "emilydream")
                return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5864_6");
              goto label_247;
            }
          }
          else
          {
            if (lower == "xor")
              return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5770");
            goto label_247;
          }
        }
        else if (stringHash <= 3490171344U)
        {
          if ((int) stringHash != -830914198)
          {
            if ((int) stringHash == -804795952 && lower == "abigailfluteduet")
              return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5728");
            goto label_247;
          }
          else
          {
            if (lower == "cowboy_outlawsong")
              return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5851");
            goto label_247;
          }
        }
        else if ((int) stringHash != -802718524)
        {
          if ((int) stringHash != -740786089)
          {
            if ((int) stringHash == -724008470 && lower == "event1")
              return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5835");
            goto label_247;
          }
          else
          {
            if (lower == "event2")
              return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5837");
            goto label_247;
          }
        }
        else
        {
          if (lower == "christmasTheme")
            return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5754");
          goto label_247;
        }
      }
      else if (stringHash <= 3975651554U)
      {
        if (stringHash <= 3858316600U)
        {
          if ((int) stringHash != -692969381)
          {
            if ((int) stringHash == -436650696 && lower == "cowboy_singing")
              return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5857");
            goto label_247;
          }
          else
          {
            if (lower == "icicles")
              return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5768");
            goto label_247;
          }
        }
        else if ((int) stringHash != -414237521)
        {
          if ((int) stringHash != -352473762)
          {
            if ((int) stringHash == -319315742 && lower == "cowboy_undead")
              return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5853");
            goto label_247;
          }
          else
          {
            if (lower == "marlonstheme")
              return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5816");
            goto label_247;
          }
        }
        else
        {
          if (lower == "honkytonky")
            return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5831");
          goto label_247;
        }
      }
      else if (stringHash <= 4049627284U)
      {
        if ((int) stringHash != -268720633)
        {
          if ((int) stringHash != -251943014)
          {
            if ((int) stringHash == -245340012 && lower == "spacemusic")
              return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5790");
            goto label_247;
          }
          else
          {
            if (lower == "winter2")
              return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5734");
            goto label_247;
          }
        }
        else
        {
          if (lower == "winter1")
            return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5732");
          goto label_247;
        }
      }
      else if ((int) stringHash != -235165395)
      {
        if ((int) stringHash != -119179726)
        {
          if ((int) stringHash == -5831836 && lower == "junimostarsong")
            return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5825");
          goto label_247;
        }
        else
        {
          if (lower == "fallfest")
            return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5766");
          goto label_247;
        }
      }
      else
      {
        if (lower == "winter3")
          return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5736");
        goto label_247;
      }
      return Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5819");
label_247:
      return cueName;
    }

    public static bool isOffScreenEndFunction(PathNode currentNode, Point endPoint, GameLocation location, Character c)
    {
      return !Utility.isOnScreen(new Vector2((float) (currentNode.x * Game1.tileSize), (float) (currentNode.y * Game1.tileSize)), Game1.tileSize / 2);
    }

    public static Vector2 getAwayFromPositionTrajectory(Microsoft.Xna.Framework.Rectangle monsterBox, Vector2 position)
    {
      double num1 = -((double) position.X - (double) monsterBox.Center.X);
      float num2 = position.Y - (float) monsterBox.Center.Y;
      float num3 = Math.Abs((float) num1) + Math.Abs(num2);
      if ((double) num3 < 1.0)
        num3 = 5f;
      double num4 = (double) num3;
      return new Vector2((float) (num1 / num4 * 20.0), (float) ((double) num2 / (double) num3 * 20.0));
    }

    public static bool tileWithinRadiusOfPlayer(int xTile, int yTile, int tileRadius, Farmer f)
    {
      Point point = new Point(xTile, yTile);
      Vector2 tileLocation = f.getTileLocation();
      if ((double) Math.Abs((float) point.X - tileLocation.X) <= (double) tileRadius)
        return (double) Math.Abs((float) point.Y - tileLocation.Y) <= (double) tileRadius;
      return false;
    }

    public static bool withinRadiusOfPlayer(int x, int y, int tileRadius, Farmer f)
    {
      Point point = new Point(x / Game1.tileSize, y / Game1.tileSize);
      Vector2 tileLocation = f.getTileLocation();
      if ((double) Math.Abs((float) point.X - tileLocation.X) <= (double) tileRadius)
        return (double) Math.Abs((float) point.Y - tileLocation.Y) <= (double) tileRadius;
      return false;
    }

    public static bool isThereAnObjectHereWhichAcceptsThisItem(GameLocation location, Item item, int x, int y)
    {
      if (item is Tool)
        return false;
      Vector2 key = new Vector2((float) (x / Game1.tileSize), (float) (y / Game1.tileSize));
      if (!location.Objects.ContainsKey(key) || location.objects[key].heldObject != null)
        return false;
      location.objects[key].performObjectDropInAction((Object) item, true, Game1.player);
      int num = location.objects[key].heldObject != null ? 1 : 0;
      location.objects[key].heldObject = (Object) null;
      return num != 0;
    }

    public static bool buyWallpaper()
    {
      if (Game1.player.Money < Game1.wallpaperPrice)
        return false;
      Game1.updateWallpaperInFarmHouse(Game1.currentWallpaper);
      Game1.farmerWallpaper = Game1.currentWallpaper;
      Game1.player.Money -= Game1.wallpaperPrice;
      Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5865"), Color.Green, 3500f));
      return true;
    }

    public static FarmAnimal getAnimal(long id)
    {
      if (Game1.getFarm().animals.ContainsKey(id))
        return Game1.getFarm().animals[id];
      foreach (Building building in Game1.getFarm().buildings)
      {
        if (building.indoors is AnimalHouse && (building.indoors as AnimalHouse).animals.ContainsKey(id))
          return (building.indoors as AnimalHouse).animals[id];
      }
      return (FarmAnimal) null;
    }

    public static bool buyFloor()
    {
      if (Game1.player.Money < Game1.floorPrice)
        return false;
      Game1.FarmerFloor = Game1.currentFloor;
      Game1.updateFloorInFarmHouse(Game1.currentFloor);
      Game1.player.Money -= Game1.floorPrice;
      Game1.addHUDMessage(new HUDMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5868"), Color.Green, 3500f));
      return true;
    }

    public static int numSilos()
    {
      int num = 0;
      foreach (Building building in (Game1.getLocationFromName("Farm") as Farm).buildings)
      {
        if (building.buildingType.Equals("Silo") && building.daysOfConstructionLeft <= 0)
          ++num;
      }
      return num;
    }

    public static List<Item> getCarpenterStock()
    {
      List<Item> stock = new List<Item>();
      stock.Add((Item) new Object(Vector2.Zero, 388, int.MaxValue));
      stock.Add((Item) new Object(Vector2.Zero, 390, int.MaxValue));
      Random r = new Random((int) Game1.stats.DaysPlayed + (int) Game1.uniqueIDForThisGame / 2);
      stock.Add((Item) new Furniture(1614, Vector2.Zero));
      stock.Add((Item) new Furniture(1616, Vector2.Zero));
      switch (Game1.dayOfMonth % 7)
      {
        case 0:
          stock.Add((Item) Utility.getRandomFurniture(r, stock, 1296, 1391));
          stock.Add((Item) Utility.getRandomFurniture(r, stock, 416, 537));
          break;
        case 1:
          stock.Add((Item) new Furniture(0, Vector2.Zero));
          stock.Add((Item) new Furniture(192, Vector2.Zero));
          stock.Add((Item) new Furniture(704, Vector2.Zero));
          stock.Add((Item) new Furniture(1120, Vector2.Zero));
          stock.Add((Item) new Furniture(1216, Vector2.Zero));
          stock.Add((Item) new Furniture(1391, Vector2.Zero));
          break;
        case 2:
          stock.Add((Item) new Furniture(3, Vector2.Zero));
          stock.Add((Item) new Furniture(197, Vector2.Zero));
          stock.Add((Item) new Furniture(709, Vector2.Zero));
          stock.Add((Item) new Furniture(1122, Vector2.Zero));
          stock.Add((Item) new Furniture(1218, Vector2.Zero));
          stock.Add((Item) new Furniture(1393, Vector2.Zero));
          break;
        case 3:
          stock.Add((Item) new Furniture(6, Vector2.Zero));
          stock.Add((Item) new Furniture(202, Vector2.Zero));
          stock.Add((Item) new Furniture(714, Vector2.Zero));
          stock.Add((Item) new Furniture(1124, Vector2.Zero));
          stock.Add((Item) new Furniture(1220, Vector2.Zero));
          stock.Add((Item) new Furniture(1395, Vector2.Zero));
          break;
        case 4:
          stock.Add((Item) Utility.getRandomFurniture(r, stock, 1296, 1391));
          stock.Add((Item) Utility.getRandomFurniture(r, stock, 1296, 1391));
          break;
        case 5:
          stock.Add((Item) Utility.getRandomFurniture(r, stock, 1443, 1450));
          stock.Add((Item) Utility.getRandomFurniture(r, stock, 288, 313));
          break;
        case 6:
          stock.Add((Item) Utility.getRandomFurniture(r, stock, 1565, 1607));
          stock.Add((Item) Utility.getRandomFurniture(r, stock, 12, 129));
          break;
      }
      stock.Add((Item) Utility.getRandomFurniture(r, stock, 0, 1462));
      stock.Add((Item) Utility.getRandomFurniture(r, stock, 0, 1462));
      while (r.NextDouble() < 0.25)
        stock.Add((Item) Utility.getRandomFurniture(r, stock, 1673, 1815));
      stock.Add((Item) new Furniture(1402, Vector2.Zero));
      stock.Add((Item) new TV(1466, Vector2.Zero));
      stock.Add((Item) new TV(1680, Vector2.Zero));
      if (Utility.getHomeOfFarmer(Game1.player).upgradeLevel > 0)
        stock.Add((Item) new TV(1468, Vector2.Zero));
      if (Utility.getHomeOfFarmer(Game1.player).upgradeLevel > 0)
        stock.Add((Item) new Furniture(1226, Vector2.Zero));
      if (!Game1.player.craftingRecipes.ContainsKey("Wooden Brazier"))
      {
        List<Item> objList = stock;
        Torch torch = new Torch(Vector2.Zero, 143, true);
        int num = 1;
        torch.isRecipe = num != 0;
        objList.Add((Item) torch);
      }
      else if (!Game1.player.craftingRecipes.ContainsKey("Stone Brazier"))
      {
        List<Item> objList = stock;
        Torch torch = new Torch(Vector2.Zero, 144, true);
        int num = 1;
        torch.isRecipe = num != 0;
        objList.Add((Item) torch);
      }
      else if (!Game1.player.craftingRecipes.ContainsKey("Barrel Brazier"))
      {
        List<Item> objList = stock;
        Torch torch = new Torch(Vector2.Zero, 150, true);
        int num = 1;
        torch.isRecipe = num != 0;
        objList.Add((Item) torch);
      }
      else if (!Game1.player.craftingRecipes.ContainsKey("Stump Brazier"))
      {
        List<Item> objList = stock;
        Torch torch = new Torch(Vector2.Zero, 147, true);
        int num = 1;
        torch.isRecipe = num != 0;
        objList.Add((Item) torch);
      }
      else if (!Game1.player.craftingRecipes.ContainsKey("Gold Brazier"))
      {
        List<Item> objList = stock;
        Torch torch = new Torch(Vector2.Zero, 145, true);
        int num = 1;
        torch.isRecipe = num != 0;
        objList.Add((Item) torch);
      }
      else if (!Game1.player.craftingRecipes.ContainsKey("Carved Brazier"))
      {
        List<Item> objList = stock;
        Torch torch = new Torch(Vector2.Zero, 148, true);
        int num = 1;
        torch.isRecipe = num != 0;
        objList.Add((Item) torch);
      }
      else if (!Game1.player.craftingRecipes.ContainsKey("Skull Brazier"))
      {
        List<Item> objList = stock;
        Torch torch = new Torch(Vector2.Zero, 149, true);
        int num = 1;
        torch.isRecipe = num != 0;
        objList.Add((Item) torch);
      }
      else if (!Game1.player.craftingRecipes.ContainsKey("Marble Brazier"))
      {
        List<Item> objList = stock;
        Torch torch = new Torch(Vector2.Zero, 151, true);
        int num = 1;
        torch.isRecipe = num != 0;
        objList.Add((Item) torch);
      }
      if (!Game1.player.craftingRecipes.ContainsKey("Wood Lamp-post"))
        stock.Add((Item) new Object(Vector2.Zero, 152, true)
        {
          isRecipe = true
        });
      if (!Game1.player.craftingRecipes.ContainsKey("Iron Lamp-post"))
        stock.Add((Item) new Object(Vector2.Zero, 153, true)
        {
          isRecipe = true
        });
      if (!Game1.player.craftingRecipes.ContainsKey("Wood Floor"))
        stock.Add((Item) new Object(328, 1, true, 50, 0));
      if (!Game1.player.craftingRecipes.ContainsKey("Stone Floor"))
        stock.Add((Item) new Object(329, 1, true, 50, 0));
      if (!Game1.player.craftingRecipes.ContainsKey("Stepping Stone Path"))
        stock.Add((Item) new Object(415, 1, true, 50, 0));
      if (!Game1.player.craftingRecipes.ContainsKey("Straw Floor"))
        stock.Add((Item) new Object(401, 1, true, 100, 0));
      if (!Game1.player.craftingRecipes.ContainsKey("Crystal Path"))
        stock.Add((Item) new Object(409, 1, true, 100, 0));
      return stock;
    }

    private static bool isFurnitureOffLimitsForSale(int index)
    {
      switch (index)
      {
        case 1680:
        case 1733:
        case 1669:
        case 1671:
        case 1541:
        case 1545:
        case 1554:
        case 1402:
        case 1466:
        case 1468:
        case 131:
        case 1226:
        case 1298:
        case 1299:
        case 1300:
        case 1301:
        case 1302:
        case 1303:
        case 1304:
        case 1305:
        case 1306:
        case 1307:
        case 1308:
          return true;
        default:
          return false;
      }
    }

    private static Furniture getRandomFurniture(Random r, List<Item> stock, int lowerIndexBound = 0, int upperIndexBound = 1462)
    {
      Dictionary<int, string> dictionary = Game1.content.Load<Dictionary<int, string>>("Data\\Furniture");
      int num;
      do
      {
        num = r.Next(lowerIndexBound, upperIndexBound);
        if (stock != null)
        {
          foreach (Item obj in stock)
          {
            if (obj is Furniture && obj.parentSheetIndex == num)
              num = -1;
          }
        }
      }
      while (Utility.isFurnitureOffLimitsForSale(num) || !dictionary.ContainsKey(num));
      Furniture furniture = new Furniture(num, Vector2.Zero);
      int maxValue = int.MaxValue;
      furniture.stack = maxValue;
      return furniture;
    }

    public static List<Item> getSaloonStock()
    {
      List<Item> objList = new List<Item>();
      objList.Add((Item) new Object(Vector2.Zero, 346, int.MaxValue));
      objList.Add((Item) new Object(Vector2.Zero, 196, int.MaxValue));
      objList.Add((Item) new Object(Vector2.Zero, 216, int.MaxValue));
      objList.Add((Item) new Object(Vector2.Zero, 224, int.MaxValue));
      objList.Add((Item) new Object(Vector2.Zero, 206, int.MaxValue));
      objList.Add((Item) new Object(Vector2.Zero, 395, int.MaxValue));
      if (!Game1.player.cookingRecipes.ContainsKey("Hashbrowns"))
        objList.Add((Item) new Object(210, 1, true, 25, 0));
      if (!Game1.player.cookingRecipes.ContainsKey("Omelet"))
        objList.Add((Item) new Object(195, 1, true, 50, 0));
      if (!Game1.player.cookingRecipes.ContainsKey("Pancakes"))
        objList.Add((Item) new Object(211, 1, true, 50, 0));
      if (!Game1.player.cookingRecipes.ContainsKey("Bread"))
        objList.Add((Item) new Object(216, 1, true, 50, 0));
      if (!Game1.player.cookingRecipes.ContainsKey("Tortilla"))
        objList.Add((Item) new Object(229, 1, true, 50, 0));
      if (!Game1.player.cookingRecipes.ContainsKey("Pizza"))
        objList.Add((Item) new Object(206, 1, true, 75, 0));
      if (!Game1.player.cookingRecipes.ContainsKey("Maki Roll"))
        objList.Add((Item) new Object(228, 1, true, 150, 0));
      if (!Game1.player.cookingRecipes.ContainsKey("Cookies") && Game1.player.eventsSeen.Contains(19))
        objList.Add((Item) new Object(223, 1, true, 150, 0)
        {
          name = "Cookies"
        });
      if (Game1.dishOfTheDay.stack > 0)
        objList.Add((Item) Game1.dishOfTheDay);
      return objList;
    }

    public static bool removeLightSource(int identifier)
    {
      bool flag = false;
      for (int index = Game1.currentLightSources.Count - 1; index >= 0; --index)
      {
        if (Game1.currentLightSources.ElementAt<LightSource>(index).identifier == identifier)
        {
          Game1.currentLightSources.Remove(Game1.currentLightSources.ElementAt<LightSource>(index));
          flag = true;
        }
      }
      return flag;
    }

    public static Horse findHorse()
    {
      foreach (GameLocation location in Game1.locations)
      {
        foreach (NPC character in location.characters)
        {
          if (character is Horse)
            return character as Horse;
        }
      }
      return (Horse) null;
    }

    public static void addSmokePuff(GameLocation l, Vector2 v, int delay = 0)
    {
      List<TemporaryAnimatedSprite> temporarySprites = l.temporarySprites;
      TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(Game1.mouseCursors, new Microsoft.Xna.Framework.Rectangle(372, 1956, 10, 10), v, false, 1f / 500f, Color.Gray);
      temporaryAnimatedSprite.alpha = 0.75f;
      Vector2 vector2_1 = new Vector2(0.0f, -0.5f);
      temporaryAnimatedSprite.motion = vector2_1;
      Vector2 vector2_2 = new Vector2(1f / 500f, 0.0f);
      temporaryAnimatedSprite.acceleration = vector2_2;
      double num1 = 99999.0;
      temporaryAnimatedSprite.interval = (float) num1;
      double num2 = 1.0;
      temporaryAnimatedSprite.layerDepth = (float) num2;
      double num3 = (double) (Game1.pixelZoom / 2);
      temporaryAnimatedSprite.scale = (float) num3;
      double num4 = 0.0199999995529652;
      temporaryAnimatedSprite.scaleChange = (float) num4;
      double num5 = (double) Game1.random.Next(-5, 6) * 3.14159274101257 / 256.0;
      temporaryAnimatedSprite.rotationChange = (float) num5;
      int num6 = delay;
      temporaryAnimatedSprite.delayBeforeAnimationStart = num6;
      temporarySprites.Add(temporaryAnimatedSprite);
    }

    public static LightSource getLightSource(int identifier)
    {
      foreach (LightSource currentLightSource in Game1.currentLightSources)
      {
        if (currentLightSource.identifier == identifier)
          return currentLightSource;
      }
      return (LightSource) null;
    }

    public static Dictionary<Item, int[]> getAllWallpapersAndFloorsForFree()
    {
      Dictionary<Item, int[]> dictionary1 = new Dictionary<Item, int[]>();
      for (int which = 0; which < 112; ++which)
      {
        Dictionary<Item, int[]> dictionary2 = dictionary1;
        Wallpaper wallpaper = new Wallpaper(which, false);
        int maxValue = int.MaxValue;
        wallpaper.stack = maxValue;
        int[] numArray = new int[2]{ 0, int.MaxValue };
        dictionary2.Add((Item) wallpaper, numArray);
      }
      for (int which = 0; which < 40; ++which)
      {
        Dictionary<Item, int[]> dictionary2 = dictionary1;
        Wallpaper wallpaper = new Wallpaper(which, true);
        int maxValue = int.MaxValue;
        wallpaper.stack = maxValue;
        int[] numArray = new int[2]{ 0, int.MaxValue };
        dictionary2.Add((Item) wallpaper, numArray);
      }
      return dictionary1;
    }

    public static Dictionary<Item, int[]> getAllFurnituresForFree()
    {
      Dictionary<Item, int[]> dictionary = new Dictionary<Item, int[]>();
      foreach (KeyValuePair<int, string> keyValuePair in Game1.content.Load<Dictionary<int, string>>("Data\\Furniture"))
      {
        if (!Utility.isFurnitureOffLimitsForSale(keyValuePair.Key))
          dictionary.Add((Item) new Furniture(keyValuePair.Key, Vector2.Zero), new int[2]
          {
            0,
            int.MaxValue
          });
      }
      dictionary.Add((Item) new Furniture(1402, Vector2.Zero), new int[2]
      {
        0,
        int.MaxValue
      });
      dictionary.Add((Item) new TV(1680, Vector2.Zero), new int[2]
      {
        0,
        int.MaxValue
      });
      dictionary.Add((Item) new TV(1466, Vector2.Zero), new int[2]
      {
        0,
        int.MaxValue
      });
      dictionary.Add((Item) new TV(1468, Vector2.Zero), new int[2]
      {
        0,
        int.MaxValue
      });
      return dictionary;
    }

    public static FarmEvent pickFarmEvent()
    {
      Random random = new Random((int) Game1.stats.DaysPlayed + (int) Game1.uniqueIDForThisGame / 2);
      if (Game1.weddingToday)
        return (FarmEvent) null;
      if ((int) Game1.stats.DaysPlayed == 31)
        return (FarmEvent) new SoundInTheNightEvent(4);
      if (Game1.player.mailForTomorrow.Contains("jojaPantry%&NL&%") || Game1.player.mailForTomorrow.Contains("jojaPantry"))
        return (FarmEvent) new WorldChangeEvent(0);
      if (Game1.player.mailForTomorrow.Contains("ccPantry%&NL&%") || Game1.player.mailForTomorrow.Contains("ccPantry"))
        return (FarmEvent) new WorldChangeEvent(1);
      if (Game1.player.mailForTomorrow.Contains("jojaVault%&NL&%") || Game1.player.mailForTomorrow.Contains("jojaVault"))
        return (FarmEvent) new WorldChangeEvent(6);
      if (Game1.player.mailForTomorrow.Contains("ccVault%&NL&%") || Game1.player.mailForTomorrow.Contains("ccVault"))
        return (FarmEvent) new WorldChangeEvent(7);
      if (Game1.player.mailForTomorrow.Contains("jojaBoilerRoom%&NL&%") || Game1.player.mailForTomorrow.Contains("jojaBoilerRoom"))
        return (FarmEvent) new WorldChangeEvent(2);
      if (Game1.player.mailForTomorrow.Contains("ccBoilerRoom%&NL&%") || Game1.player.mailForTomorrow.Contains("ccBoilerRoom"))
        return (FarmEvent) new WorldChangeEvent(3);
      if (Game1.player.mailForTomorrow.Contains("jojaCraftsRoom%&NL&%") || Game1.player.mailForTomorrow.Contains("jojaCraftsRoom"))
        return (FarmEvent) new WorldChangeEvent(4);
      if (Game1.player.mailForTomorrow.Contains("ccCraftsRoom%&NL&%") || Game1.player.mailForTomorrow.Contains("ccCraftsRoom"))
        return (FarmEvent) new WorldChangeEvent(5);
      if (Game1.player.mailForTomorrow.Contains("jojaFishTank%&NL&%") || Game1.player.mailForTomorrow.Contains("jojaFishTank"))
        return (FarmEvent) new WorldChangeEvent(8);
      if (Game1.player.mailForTomorrow.Contains("ccFishTank%&NL&%") || Game1.player.mailForTomorrow.Contains("ccFishTank"))
        return (FarmEvent) new WorldChangeEvent(9);
      if (Game1.player.isMarried() && Game1.player.spouse != null && Game1.getCharacterFromName(Game1.player.spouse, false).daysUntilBirthing == 0)
        return (FarmEvent) new BirthingEvent();
      if (Game1.player.isMarried() && Game1.player.spouse != null && (Game1.getCharacterFromName(Game1.player.spouse, false).canGetPregnant() && random.NextDouble() < 0.05))
        return (FarmEvent) new QuestionEvent(1);
      if (random.NextDouble() < 0.01 && !Game1.currentSeason.Equals("winter"))
        return (FarmEvent) new FairyEvent();
      if (random.NextDouble() < 0.01)
        return (FarmEvent) new WitchEvent();
      if (random.NextDouble() < 0.01)
        return (FarmEvent) new SoundInTheNightEvent(1);
      if (random.NextDouble() < 0.01 && Game1.year > 1)
        return (FarmEvent) new SoundInTheNightEvent(0);
      if (random.NextDouble() < 0.01)
        return (FarmEvent) new SoundInTheNightEvent(3);
      if (random.NextDouble() < 0.5)
        return (FarmEvent) new QuestionEvent(2);
      return (FarmEvent) new SoundInTheNightEvent(2);
    }

    public static string capitalizeFirstLetter(string s)
    {
      if (s == null || s.Length < 1)
        return "";
      return s[0].ToString().ToUpper() + (s.Length > 1 ? s.Substring(1) : "");
    }

    public static Dictionary<Item, int[]> getBlacksmithStock()
    {
      return new Dictionary<Item, int[]>()
      {
        {
          (Item) new Object(Vector2.Zero, 378, int.MaxValue),
          new int[2]{ 75, int.MaxValue }
        },
        {
          (Item) new Object(Vector2.Zero, 380, int.MaxValue),
          new int[2]{ 150, int.MaxValue }
        },
        {
          (Item) new Object(Vector2.Zero, 382, int.MaxValue),
          new int[2]{ 150, int.MaxValue }
        },
        {
          (Item) new Object(Vector2.Zero, 384, int.MaxValue),
          new int[2]{ 400, int.MaxValue }
        }
      };
    }

    public static bool alreadyHasLightSourceWithThisID(int identifier)
    {
      foreach (LightSource currentLightSource in Game1.currentLightSources)
      {
        if (currentLightSource.identifier == identifier)
          return true;
      }
      return false;
    }

    public static void repositionLightSource(int identifier, Vector2 position)
    {
      foreach (LightSource currentLightSource in Game1.currentLightSources)
      {
        if (currentLightSource.identifier == identifier)
          currentLightSource.position = position;
      }
    }

    public static Dictionary<Item, int[]> getAnimalShopStock()
    {
      Dictionary<Item, int[]> dictionary = new Dictionary<Item, int[]>();
      dictionary.Add((Item) new Object(178, 1, false, -1, 0), new int[2]
      {
        50,
        int.MaxValue
      });
      Object @object = new Object(Vector2.Zero, 104, false);
      @object.price = 2000;
      @object.Stack = 1;
      dictionary.Add((Item) @object, new int[2]
      {
        2000,
        int.MaxValue
      });
      if (Game1.player.hasItemWithNameThatContains("Milk Pail") == null)
        dictionary.Add((Item) new MilkPail(), new int[2]
        {
          1000,
          1
        });
      if (Game1.player.hasItemWithNameThatContains("Shears") == null)
        dictionary.Add((Item) new Shears(), new int[2]
        {
          1000,
          1
        });
      return dictionary;
    }

    public static bool areThereAnyOtherAnimalsWithThisName(string name)
    {
      Farm locationFromName = Game1.getLocationFromName("Farm") as Farm;
      foreach (Building building in locationFromName.buildings)
      {
        if (building.indoors is AnimalHouse)
        {
          foreach (FarmAnimal farmAnimal in (building.indoors as AnimalHouse).animals.Values)
          {
            if (farmAnimal.displayName != null && farmAnimal.displayName.Equals(name))
              return true;
          }
        }
      }
      foreach (FarmAnimal farmAnimal in locationFromName.animals.Values)
      {
        if (farmAnimal.displayName != null && farmAnimal.displayName.Equals(name))
          return true;
      }
      return false;
    }

    public static string getNumberWithCommas(int number)
    {
      StringBuilder stringBuilder = new StringBuilder(string.Concat((object) number));
      string str = ",";
      if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.es || LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.de || LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.pt)
        str = ".";
      if (LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.ru)
        str = " ";
      int num = stringBuilder.Length - 4;
      while (num >= 0)
      {
        stringBuilder.Insert(num + 1, str);
        num -= 3;
      }
      return stringBuilder.ToString();
    }

    public static List<Object> getPurchaseAnimalStock()
    {
      return new List<Object>()
      {
        new Object(100, 1, false, 400, 0)
        {
          name = "Chicken",
          type = Game1.getFarm().isBuildingConstructed("Coop") || Game1.getFarm().isBuildingConstructed("Deluxe Coop") || Game1.getFarm().isBuildingConstructed("Big Coop") ? (string) null : Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5926"),
          displayName = Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5922")
        },
        new Object(100, 1, false, 750, 0)
        {
          name = "Dairy Cow",
          type = Game1.getFarm().isBuildingConstructed("Barn") || Game1.getFarm().isBuildingConstructed("Deluxe Barn") || Game1.getFarm().isBuildingConstructed("Big Barn") ? (string) null : Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5931"),
          displayName = Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5927")
        },
        new Object(100, 1, false, 2000, 0)
        {
          name = "Goat",
          type = Game1.getFarm().isBuildingConstructed("Big Barn") || Game1.getFarm().isBuildingConstructed("Deluxe Barn") ? (string) null : Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5936"),
          displayName = Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5933")
        },
        new Object(100, 1, false, 2000, 0)
        {
          name = "Duck",
          type = Game1.getFarm().isBuildingConstructed("Big Coop") || Game1.getFarm().isBuildingConstructed("Deluxe Coop") ? (string) null : Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5940"),
          displayName = Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5937")
        },
        new Object(100, 1, false, 4000, 0)
        {
          name = "Sheep",
          type = Game1.getFarm().isBuildingConstructed("Deluxe Barn") ? (string) null : Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5944"),
          displayName = Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5942")
        },
        new Object(100, 1, false, 4000, 0)
        {
          name = "Rabbit",
          type = Game1.getFarm().isBuildingConstructed("Deluxe Coop") ? (string) null : Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5947"),
          displayName = Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5945")
        },
        new Object(100, 1, false, 8000, 0)
        {
          name = "Pig",
          type = Game1.getFarm().isBuildingConstructed("Deluxe Barn") ? (string) null : Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5950"),
          displayName = Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.5948")
        }
      };
    }

    public static List<Item> getShopStock(bool Pierres)
    {
      List<Item> objList1 = new List<Item>();
      if (Pierres)
      {
        if (Game1.currentSeason.Equals("spring"))
        {
          objList1.Add((Item) new Object(Vector2.Zero, 472, int.MaxValue));
          objList1.Add((Item) new Object(Vector2.Zero, 473, int.MaxValue));
          objList1.Add((Item) new Object(Vector2.Zero, 474, int.MaxValue));
          objList1.Add((Item) new Object(Vector2.Zero, 475, int.MaxValue));
          objList1.Add((Item) new Object(Vector2.Zero, 427, int.MaxValue));
          objList1.Add((Item) new Object(Vector2.Zero, 429, int.MaxValue));
          objList1.Add((Item) new Object(Vector2.Zero, 477, int.MaxValue));
          objList1.Add((Item) new Object(628, int.MaxValue, false, 1700, 0));
          objList1.Add((Item) new Object(629, int.MaxValue, false, 1000, 0));
          if (Game1.year > 1)
            objList1.Add((Item) new Object(Vector2.Zero, 476, int.MaxValue));
        }
        if (Game1.currentSeason.Equals("summer"))
        {
          objList1.Add((Item) new Object(Vector2.Zero, 480, int.MaxValue));
          objList1.Add((Item) new Object(Vector2.Zero, 482, int.MaxValue));
          objList1.Add((Item) new Object(Vector2.Zero, 483, int.MaxValue));
          objList1.Add((Item) new Object(Vector2.Zero, 484, int.MaxValue));
          objList1.Add((Item) new Object(Vector2.Zero, 479, int.MaxValue));
          objList1.Add((Item) new Object(Vector2.Zero, 302, int.MaxValue));
          objList1.Add((Item) new Object(Vector2.Zero, 453, int.MaxValue));
          objList1.Add((Item) new Object(Vector2.Zero, 455, int.MaxValue));
          objList1.Add((Item) new Object(630, int.MaxValue, false, 2000, 0));
          objList1.Add((Item) new Object(631, int.MaxValue, false, 3000, 0));
          if (Game1.year > 1)
            objList1.Add((Item) new Object(Vector2.Zero, 485, int.MaxValue));
        }
        if (Game1.currentSeason.Equals("fall"))
        {
          objList1.Add((Item) new Object(Vector2.Zero, 487, int.MaxValue));
          objList1.Add((Item) new Object(Vector2.Zero, 488, int.MaxValue));
          objList1.Add((Item) new Object(Vector2.Zero, 490, int.MaxValue));
          objList1.Add((Item) new Object(Vector2.Zero, 299, int.MaxValue));
          objList1.Add((Item) new Object(Vector2.Zero, 301, int.MaxValue));
          objList1.Add((Item) new Object(Vector2.Zero, 492, int.MaxValue));
          objList1.Add((Item) new Object(Vector2.Zero, 491, int.MaxValue));
          objList1.Add((Item) new Object(Vector2.Zero, 493, int.MaxValue));
          objList1.Add((Item) new Object(431, int.MaxValue, false, 100, 0));
          objList1.Add((Item) new Object(Vector2.Zero, 425, int.MaxValue));
          objList1.Add((Item) new Object(632, int.MaxValue, false, 3000, 0));
          objList1.Add((Item) new Object(633, int.MaxValue, false, 2000, 0));
          if (Game1.year > 1)
            objList1.Add((Item) new Object(Vector2.Zero, 489, int.MaxValue));
        }
        objList1.Add((Item) new Object(Vector2.Zero, 297, int.MaxValue));
        objList1.Add((Item) new Object(Vector2.Zero, 245, int.MaxValue));
        objList1.Add((Item) new Object(Vector2.Zero, 246, int.MaxValue));
        objList1.Add((Item) new Object(Vector2.Zero, 423, int.MaxValue));
        Random random = new Random((int) Game1.stats.DaysPlayed + (int) Game1.uniqueIDForThisGame / 2);
        List<Item> objList2 = objList1;
        Wallpaper wallpaper1 = new Wallpaper(random.Next(112), false);
        int maxValue1 = int.MaxValue;
        wallpaper1.stack = maxValue1;
        objList2.Add((Item) wallpaper1);
        List<Item> objList3 = objList1;
        Wallpaper wallpaper2 = new Wallpaper(random.Next(40), true);
        int maxValue2 = int.MaxValue;
        wallpaper2.stack = maxValue2;
        objList3.Add((Item) wallpaper2);
        if (Game1.player.achievements.Contains(38))
          objList1.Add((Item) new Object(Vector2.Zero, 458, int.MaxValue));
      }
      else
      {
        if (Game1.currentSeason.Equals("spring"))
          objList1.Add((Item) new Object(Vector2.Zero, 478, int.MaxValue));
        if (Game1.currentSeason.Equals("summer"))
        {
          objList1.Add((Item) new Object(Vector2.Zero, 486, int.MaxValue));
          objList1.Add((Item) new Object(Vector2.Zero, 481, int.MaxValue));
        }
        if (Game1.currentSeason.Equals("fall"))
        {
          objList1.Add((Item) new Object(Vector2.Zero, 493, int.MaxValue));
          objList1.Add((Item) new Object(Vector2.Zero, 494, int.MaxValue));
        }
        objList1.Add((Item) new Object(Vector2.Zero, 88, int.MaxValue));
        objList1.Add((Item) new Object(Vector2.Zero, 90, int.MaxValue));
      }
      return objList1;
    }

    public static Vector2 getCornersOfThisRectangle(ref Microsoft.Xna.Framework.Rectangle r, int corner)
    {
      switch (corner)
      {
        case 1:
          return new Vector2((float) (r.Right - 1), (float) r.Y);
        case 2:
          return new Vector2((float) (r.Right - 1), (float) (r.Bottom - 1));
        case 3:
          return new Vector2((float) r.X, (float) (r.Bottom - 1));
        default:
          return new Vector2((float) r.X, (float) r.Y);
      }
    }

    public static Farmer getNearestFarmerInCurrentLocation(Vector2 tileLocation)
    {
      float val1 = 999999f;
      List<Farmer> farmers = Game1.currentLocation.getFarmers();
      Farmer farmer = (Farmer) null;
      for (int index = 0; index < farmers.Count; ++index)
      {
        float num = val1;
        val1 = Math.Min(val1, Math.Abs(farmers[index].getTileLocation().X - tileLocation.X) + Math.Abs(farmers[index].getTileLocation().Y - tileLocation.Y));
        if ((double) val1 < (double) num)
          farmer = farmers[index];
      }
      return farmer;
    }

    private static int priceForToolUpgradeLevel(int level)
    {
      switch (level)
      {
        case 1:
          return 2000;
        case 2:
          return 5000;
        case 3:
          return 10000;
        case 4:
          return 25000;
        default:
          return 2000;
      }
    }

    private static int indexOfExtraMaterialForToolUpgrade(int level)
    {
      switch (level)
      {
        case 1:
          return 334;
        case 2:
          return 335;
        case 3:
          return 336;
        case 4:
          return 337;
        default:
          return 334;
      }
    }

    public static Dictionary<Item, int[]> getBlacksmithUpgradeStock(Farmer who)
    {
      Dictionary<Item, int[]> dictionary = new Dictionary<Item, int[]>();
      Tool toolFromName1 = who.getToolFromName("Axe");
      Tool toolFromName2 = who.getToolFromName("Watering Can");
      Tool toolFromName3 = who.getToolFromName("Pickaxe");
      Tool toolFromName4 = who.getToolFromName("Hoe");
      if (toolFromName1 != null && toolFromName1.upgradeLevel < 4)
      {
        Tool tool = (Tool) new Axe();
        tool.UpgradeLevel = toolFromName1.upgradeLevel + 1;
        dictionary.Add((Item) tool, new int[3]
        {
          Utility.priceForToolUpgradeLevel(tool.UpgradeLevel),
          1,
          Utility.indexOfExtraMaterialForToolUpgrade(tool.upgradeLevel)
        });
      }
      if (toolFromName2 != null && toolFromName2.upgradeLevel < 4)
      {
        Tool tool = (Tool) new WateringCan();
        tool.UpgradeLevel = toolFromName2.upgradeLevel + 1;
        dictionary.Add((Item) tool, new int[3]
        {
          Utility.priceForToolUpgradeLevel(tool.UpgradeLevel),
          1,
          Utility.indexOfExtraMaterialForToolUpgrade(tool.upgradeLevel)
        });
      }
      if (toolFromName3 != null && toolFromName3.upgradeLevel < 4)
      {
        Tool tool = (Tool) new Pickaxe();
        tool.UpgradeLevel = toolFromName3.upgradeLevel + 1;
        dictionary.Add((Item) tool, new int[3]
        {
          Utility.priceForToolUpgradeLevel(tool.UpgradeLevel),
          1,
          Utility.indexOfExtraMaterialForToolUpgrade(tool.upgradeLevel)
        });
      }
      if (toolFromName4 != null && toolFromName4.upgradeLevel < 4)
      {
        Tool tool = (Tool) new Hoe();
        tool.UpgradeLevel = toolFromName4.upgradeLevel + 1;
        dictionary.Add((Item) tool, new int[3]
        {
          Utility.priceForToolUpgradeLevel(tool.UpgradeLevel),
          1,
          Utility.indexOfExtraMaterialForToolUpgrade(tool.upgradeLevel)
        });
      }
      return dictionary;
    }

    public static Vector2 GetCurvePoint(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
    {
      float num1 = (float) (3.0 * ((double) p1.X - (double) p0.X));
      float num2 = (float) (3.0 * ((double) p1.Y - (double) p0.Y));
      float num3 = (float) (3.0 * ((double) p2.X - (double) p1.X)) - num1;
      float num4 = (float) (3.0 * ((double) p2.Y - (double) p1.Y)) - num2;
      double num5 = (double) p3.X - (double) p0.X - (double) num1 - (double) num3;
      float num6 = p3.Y - p0.Y - num2 - num4;
      float num7 = t * t * t;
      float num8 = t * t;
      double num9 = (double) num7;
      return new Vector2((float) (num5 * num9 + (double) num3 * (double) num8 + (double) num1 * (double) t) + p0.X, (float) ((double) num6 * (double) num7 + (double) num4 * (double) num8 + (double) num2 * (double) t) + p0.Y);
    }

    public static GameLocation getGameLocationOfCharacter(NPC n)
    {
      foreach (GameLocation location in Game1.locations)
      {
        if (location.characters.Contains(n))
          return location;
      }
      return (GameLocation) null;
    }

    public static int[] parseStringToIntArray(string s, char delimiter = ' ')
    {
      string[] strArray = s.Split(delimiter);
      int[] numArray = new int[strArray.Length];
      for (int index = 0; index < strArray.Length; ++index)
        numArray[index] = Convert.ToInt32(strArray[index]);
      return numArray;
    }

    public static void drawLineWithScreenCoordinates(int x1, int y1, int x2, int y2, SpriteBatch b, Color color1, float layerDepth = 1f)
    {
      Vector2 vector2_1 = new Vector2((float) x2, (float) y2);
      Vector2 vector2_2 = new Vector2((float) x1, (float) y1);
      Vector2 vector2_3 = vector2_2;
      Vector2 vector2_4 = vector2_1 - vector2_3;
      float rotation = (float) Math.Atan2((double) vector2_4.Y, (double) vector2_4.X);
      b.Draw(Game1.fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle((int) vector2_2.X, (int) vector2_2.Y, (int) vector2_4.Length(), 1), new Microsoft.Xna.Framework.Rectangle?(), color1, rotation, new Vector2(0.0f, 0.0f), SpriteEffects.None, layerDepth);
      b.Draw(Game1.fadeToBlackRect, new Microsoft.Xna.Framework.Rectangle((int) vector2_2.X, (int) vector2_2.Y + 1, (int) vector2_4.Length(), 1), new Microsoft.Xna.Framework.Rectangle?(), color1, rotation, new Vector2(0.0f, 0.0f), SpriteEffects.None, layerDepth);
    }

    public static string getRandomNonLoopingSong()
    {
      switch (Game1.random.Next(7))
      {
        case 0:
          return "springsongs";
        case 1:
          return "summersongs";
        case 2:
          return "fallsongs";
        case 3:
          return "wintersongs";
        case 4:
          return "EarthMine";
        case 5:
          return "FrostMine";
        case 6:
          return "LavaMine";
        default:
          return "fallsongs";
      }
    }

    public static Farmer isThereAFarmerWithinDistance(Vector2 tileLocation, int tilesAway)
    {
      foreach (Farmer farmer in Game1.currentLocation.getFarmers())
      {
        if ((double) Math.Abs(tileLocation.X - farmer.getTileLocation().X) <= (double) tilesAway && (double) Math.Abs(tileLocation.Y - farmer.getTileLocation().Y) <= (double) tilesAway)
          return farmer;
      }
      return (Farmer) null;
    }

    public static Character isThereAFarmerOrCharacterWithinDistance(Vector2 tileLocation, int tilesAway, GameLocation environment)
    {
      foreach (Character character in environment.characters)
      {
        if ((double) Vector2.Distance(character.getTileLocation(), tileLocation) <= (double) tilesAway)
          return character;
      }
      return (Character) Utility.isThereAFarmerWithinDistance(tileLocation, tilesAway);
    }

    public static Color getRedToGreenLerpColor(float power)
    {
      return new Color((double) power <= 0.5 ? (int) byte.MaxValue : (int) ((1.0 - (double) power) * 2.0 * (double) byte.MaxValue), (int) Math.Min((float) byte.MaxValue, (float) ((double) power * 2.0 * (double) byte.MaxValue)), 0);
    }

    public static FarmHouse getHomeOfFarmer(Farmer who)
    {
      if (!Game1.IsMultiplayer)
        return Game1.getLocationFromName("FarmHouse") as FarmHouse;
      return (FarmHouse) null;
    }

    public static Vector2 getRandomPositionOnScreen()
    {
      return new Vector2((float) Game1.random.Next(Game1.viewport.Width), (float) Game1.random.Next(Game1.viewport.Height));
    }

    public static Microsoft.Xna.Framework.Rectangle getRectangleCenteredAt(Vector2 v, int size)
    {
      return new Microsoft.Xna.Framework.Rectangle((int) v.X - size / 2, (int) v.Y - size / 2, size, size);
    }

    public static bool checkForCharacterInteractionAtTile(Vector2 tileLocation, Farmer who)
    {
      if (Game1.currentLocation.isCharacterAtTile(tileLocation) == null || Game1.currentLocation.isCharacterAtTile(tileLocation).IsMonster)
        return false;
      if (who.ActiveObject != null && who.ActiveObject.canBeGivenAsGift() && (who.friendships.ContainsKey(Game1.currentLocation.isCharacterAtTile(tileLocation).name) && who.friendships[Game1.currentLocation.isCharacterAtTile(tileLocation).name][3] != 1) && !Game1.eventUp)
        Game1.mouseCursor = 3;
      else if (Game1.currentLocation.isCharacterAtTile(tileLocation).CurrentDialogue != null && (Game1.currentLocation.isCharacterAtTile(tileLocation).CurrentDialogue.Count > 0 || Game1.currentLocation.isCharacterAtTile(tileLocation).hasTemporaryMessageAvailable()) && !Game1.currentLocation.isCharacterAtTile(tileLocation).isOnSilentTemporaryMessage())
        Game1.mouseCursor = 4;
      Game1.currentLocation.checkForSpecialCharacterIconAtThisTile(tileLocation);
      if (Game1.mouseCursor == 3 || Game1.mouseCursor == 4)
        Game1.mouseCursorTransparency = !Utility.tileWithinRadiusOfPlayer((int) tileLocation.X, (int) tileLocation.Y, 1, who) ? 0.5f : 1f;
      return true;
    }

    public static bool canGrabSomethingFromHere(int x, int y, Farmer who)
    {
      Vector2 index = new Vector2((float) (x / Game1.tileSize), (float) (y / Game1.tileSize));
      if (Game1.currentLocation.isObjectAt(x, y))
        Game1.currentLocation.getObjectAt(x, y).hoverAction();
      if (Utility.checkForCharacterInteractionAtTile(index, who) || Utility.checkForCharacterInteractionAtTile(index + new Vector2(0.0f, 1f), who) || !who.IsMainPlayer)
        return false;
      if (Game1.currentLocation.Objects.ContainsKey(index))
      {
        if (Game1.currentLocation.Objects[index].readyForHarvest || Game1.currentLocation.Objects[index].Name.Contains("Table") && Game1.currentLocation.Objects[index].heldObject != null || Game1.currentLocation.Objects[index].isSpawnedObject)
        {
          Game1.mouseCursor = 6;
          if (Utility.withinRadiusOfPlayer(x, y, 1, who))
            return true;
          Game1.mouseCursorTransparency = 0.5f;
          return false;
        }
      }
      else if (Game1.currentLocation.terrainFeatures.ContainsKey(index) && Game1.currentLocation.terrainFeatures[index].GetType() == typeof (HoeDirt) && ((HoeDirt) Game1.currentLocation.terrainFeatures[index]).readyForHarvest())
      {
        Game1.mouseCursor = 6;
        if (Utility.withinRadiusOfPlayer(x, y, 1, who))
          return true;
        Game1.mouseCursorTransparency = 0.5f;
        return false;
      }
      return false;
    }

    public static Microsoft.Xna.Framework.Rectangle getSourceRectWithinRectangularRegion(int regionX, int regionY, int regionWidth, int sourceIndex, int sourceWidth, int sourceHeight)
    {
      int num = regionWidth / sourceWidth;
      return new Microsoft.Xna.Framework.Rectangle(regionX + sourceIndex % num * sourceWidth, regionY + sourceIndex / num * sourceHeight, sourceWidth, sourceHeight);
    }

    public static void drawWithShadow(SpriteBatch b, Texture2D texture, Vector2 position, Microsoft.Xna.Framework.Rectangle sourceRect, Color color, float rotation, Vector2 origin, float scale = -1f, bool flipped = false, float layerDepth = -1f, int horizontalShadowOffset = -1, int verticalShadowOffset = -1, float shadowIntensity = 0.35f)
    {
      if ((double) scale == -1.0)
        scale = (float) Game1.pixelZoom;
      if ((double) layerDepth == -1.0)
        layerDepth = position.Y / 10000f;
      if (horizontalShadowOffset == -1)
        horizontalShadowOffset = -Game1.pixelZoom;
      if (verticalShadowOffset == -1)
        verticalShadowOffset = Game1.pixelZoom;
      b.Draw(texture, position + new Vector2((float) horizontalShadowOffset, (float) verticalShadowOffset), new Microsoft.Xna.Framework.Rectangle?(sourceRect), Color.Black * shadowIntensity, rotation, origin, scale, flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth - 0.0001f);
      b.Draw(texture, position, new Microsoft.Xna.Framework.Rectangle?(sourceRect), color, rotation, origin, scale, flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layerDepth);
    }

    public static void drawTextWithShadow(SpriteBatch b, string text, SpriteFont font, Vector2 position, Color color, float scale = 1f, float layerDepth = -1f, int horizontalShadowOffset = -1, int verticalShadowOffset = -1, float shadowIntensity = 1f, int numShadows = 3)
    {
      if ((double) layerDepth == -1.0)
        layerDepth = position.Y / 10000f;
      bool flag = Game1.content.GetCurrentLanguage() == LocalizedContentManager.LanguageCode.ru || Game1.content.GetCurrentLanguage() == LocalizedContentManager.LanguageCode.de;
      if (horizontalShadowOffset == -1)
        horizontalShadowOffset = font.Equals((object) Game1.smallFont) | flag ? -Game1.pixelZoom + 2 : -Game1.pixelZoom + 1;
      if (verticalShadowOffset == -1)
        verticalShadowOffset = font.Equals((object) Game1.smallFont) | flag ? Game1.pixelZoom - 2 : Game1.pixelZoom - 1;
      if (text == null)
        text = "";
      b.DrawString(font, text, position + new Vector2((float) horizontalShadowOffset, (float) verticalShadowOffset), new Color(221, 148, 84) * shadowIntensity, 0.0f, Vector2.Zero, scale, SpriteEffects.None, layerDepth - 0.0001f);
      if (numShadows == 2)
        b.DrawString(font, text, position + new Vector2((float) horizontalShadowOffset, 0.0f), new Color(221, 148, 84) * shadowIntensity, 0.0f, Vector2.Zero, scale, SpriteEffects.None, layerDepth - 0.0002f);
      if (numShadows == 3)
        b.DrawString(font, text, position + new Vector2(0.0f, (float) verticalShadowOffset), new Color(221, 148, 84) * shadowIntensity, 0.0f, Vector2.Zero, scale, SpriteEffects.None, layerDepth - 0.0003f);
      b.DrawString(font, text, position, color, 0.0f, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
    }

    public static void drawBoldText(SpriteBatch b, string text, SpriteFont font, Vector2 position, Color color, float scale = 1f, float layerDepth = -1f, int boldnessOffset = 1)
    {
      if ((double) layerDepth == -1.0)
        layerDepth = position.Y / 10000f;
      b.DrawString(font, text, position, color, 0.0f, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
      b.DrawString(font, text, position + new Vector2((float) boldnessOffset, 0.0f), color, 0.0f, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
      b.DrawString(font, text, position + new Vector2((float) boldnessOffset, (float) boldnessOffset), color, 0.0f, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
      b.DrawString(font, text, position + new Vector2(0.0f, (float) boldnessOffset), color, 0.0f, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
    }

    public static bool playerCanPlaceItemHere(GameLocation location, Item item, int x, int y, Farmer f)
    {
      if (item == null || item is Tool || (Game1.eventUp || f.bathingClothes) || !Utility.withinRadiusOfPlayer(x, y, 1, f) && (!Utility.withinRadiusOfPlayer(x, y, 2, f) || !Game1.isAnyGamePadButtonBeingPressed() || (double) Game1.mouseCursorTransparency != 0.0) && (!(item is Furniture) && !(item is Wallpaper) || !(location is DecoratableLocation)))
        return false;
      Vector2 index1 = new Vector2((float) (x / Game1.tileSize), (float) (y / Game1.tileSize));
      if (item.canBePlacedHere(location, index1))
      {
        Microsoft.Xna.Framework.Rectangle rectangle;
        if (!((Object) item).isPassable())
        {
          for (int index2 = 0; index2 < location.farmers.Count; ++index2)
          {
            rectangle = location.farmers[index2].GetBoundingBox();
            if (rectangle.Intersects(new Microsoft.Xna.Framework.Rectangle((int) index1.X * Game1.tileSize, (int) index1.Y * Game1.tileSize, Game1.tileSize, Game1.tileSize)))
              return false;
          }
        }
        if (location.isTilePlaceable(index1, item) && item.isPlaceable())
        {
          if (!((Object) item).isPassable())
          {
            rectangle = new Microsoft.Xna.Framework.Rectangle((int) ((double) index1.X * (double) Game1.tileSize), (int) ((double) index1.Y * (double) Game1.tileSize), Game1.tileSize, Game1.tileSize);
            if (!rectangle.Intersects(Game1.player.GetBoundingBox()))
              goto label_13;
          }
          else
            goto label_13;
        }
        if (item.category != -74 || !location.terrainFeatures.ContainsKey(index1) || (!(location.terrainFeatures[index1].GetType() == typeof (HoeDirt)) || !((HoeDirt) location.terrainFeatures[index1]).canPlantThisSeedHere((item as Object).ParentSheetIndex, (int) index1.X, (int) index1.Y, false)))
          goto label_14;
label_13:
        return true;
      }
label_14:
      return false;
    }

    public static int getDirectionFromChange(Vector2 current, Vector2 previous, bool yBias = false)
    {
      if (!yBias && (double) current.X > (double) previous.X)
        return 1;
      if (!yBias && (double) current.X < (double) previous.X)
        return 3;
      if ((double) current.Y > (double) previous.Y)
        return 2;
      if ((double) current.Y < (double) previous.Y)
        return 0;
      if ((double) current.X > (double) previous.X)
        return 1;
      return (double) current.X < (double) previous.X ? 3 : -1;
    }

    public static bool doesRectangleIntersectTile(Microsoft.Xna.Framework.Rectangle r, int tileX, int tileY)
    {
      Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle(tileX * Game1.tileSize, tileY * Game1.tileSize, Game1.tileSize, Game1.tileSize);
      return r.Intersects(rectangle);
    }

    public static List<NPC> getAllCharacters()
    {
      Utility._allCharacters.Clear();
      foreach (GameLocation location in Game1.locations)
        Utility._allCharacters.AddRange((IEnumerable<NPC>) location.characters);
      Farm farm = Game1.getFarm();
      if (farm != null)
      {
        foreach (Building building in farm.buildings)
        {
          if (building.indoors != null)
          {
            foreach (NPC character in building.indoors.characters)
              character.currentLocation = building.indoors;
            Utility._allCharacters.AddRange((IEnumerable<NPC>) building.indoors.characters);
          }
        }
      }
      return Utility._allCharacters;
    }

    public static Item removeItemFromInventory(int whichItemIndex, List<Item> items)
    {
      if (whichItemIndex < 0 || whichItemIndex >= items.Count || items[whichItemIndex] == null)
        return (Item) null;
      Item obj = items[whichItemIndex];
      if (whichItemIndex == Game1.player.CurrentToolIndex && items.Equals((object) Game1.player.items) && obj != null)
        obj.actionWhenStopBeingHeld(Game1.player);
      items[whichItemIndex] = (Item) null;
      return obj;
    }

    public static NPC getRandomTownNPC()
    {
      return Utility.getRandomTownNPC(Game1.random, 0);
    }

    public static NPC getRandomTownNPC(Random r, int offset)
    {
      Dictionary<string, string> source = Game1.content.Load<Dictionary<string, string>>("Data\\NPCDispositions");
      int index = r.Next(source.Count);
      NPC characterFromName;
      for (characterFromName = Game1.getCharacterFromName(source.ElementAt<KeyValuePair<string, string>>(index).Key, false); source.ElementAt<KeyValuePair<string, string>>(index).Key.Equals("Wizard") || source.ElementAt<KeyValuePair<string, string>>(index).Key.Equals("Krobus") || (source.ElementAt<KeyValuePair<string, string>>(index).Key.Equals("Sandy") || source.ElementAt<KeyValuePair<string, string>>(index).Key.Equals("Dwarf")) || (source.ElementAt<KeyValuePair<string, string>>(index).Key.Equals("Marlon") || characterFromName == null); characterFromName = Game1.getCharacterFromName(source.ElementAt<KeyValuePair<string, string>>(index).Key, false))
        index = r.Next(source.Count);
      return characterFromName;
    }

    public static bool foundAllStardrops()
    {
      if (Game1.player.hasOrWillReceiveMail("CF_Fair") && Game1.player.hasOrWillReceiveMail("CF_Fish") && (Game1.player.hasOrWillReceiveMail("CF_Mines") && Game1.player.hasOrWillReceiveMail("CF_Sewer")) && (Game1.player.hasOrWillReceiveMail("museumComplete") && Game1.player.hasOrWillReceiveMail("CF_Spouse")))
        return Game1.player.hasOrWillReceiveMail("CF_Statue");
      return false;
    }

    public static int getGrandpaScore()
    {
      int num1 = 0;
      if (Game1.player.totalMoneyEarned >= 50000U)
        ++num1;
      if (Game1.player.totalMoneyEarned >= 100000U)
        ++num1;
      if (Game1.player.totalMoneyEarned >= 200000U)
        ++num1;
      if (Game1.player.totalMoneyEarned >= 300000U)
        ++num1;
      if (Game1.player.totalMoneyEarned >= 500000U)
        ++num1;
      if (Game1.player.totalMoneyEarned >= 1000000U)
        num1 += 2;
      if (Game1.player.achievements.Contains(5))
        ++num1;
      if (Game1.player.hasSkullKey)
        ++num1;
      int num2 = Game1.isLocationAccessible("CommunityCenter") ? 1 : 0;
      if (num2 != 0 || Game1.player.hasCompletedCommunityCenter())
        ++num1;
      if (num2 != 0)
        num1 += 2;
      if (Game1.player.isMarried() && Utility.getHomeOfFarmer(Game1.player).upgradeLevel >= 2)
        ++num1;
      if (Game1.player.hasRustyKey)
        ++num1;
      if (Game1.player.achievements.Contains(26))
        ++num1;
      if (Game1.player.achievements.Contains(34))
        ++num1;
      int friendsWithinThisRange = Utility.getNumberOfFriendsWithinThisRange(Game1.player, 1975, 999999, false);
      int num3 = 5;
      if (friendsWithinThisRange >= num3)
        ++num1;
      int num4 = 10;
      if (friendsWithinThisRange >= num4)
        ++num1;
      int level = Game1.player.Level;
      int num5 = 15;
      if (level >= num5)
        ++num1;
      int num6 = 25;
      if (level >= num6)
        ++num1;
      string petName = Game1.player.getPetName();
      if (petName != null)
      {
        Pet characterFromName = Game1.getCharacterFromName(petName, false) as Pet;
        if (characterFromName != null && characterFromName.friendshipTowardFarmer >= 999)
          ++num1;
      }
      return num1;
    }

    public static int getGrandpaCandlesFromScore(int score)
    {
      if (score >= 12)
        return 4;
      if (score >= 8)
        return 3;
      return score >= 4 ? 2 : 1;
    }

    public static bool canItemBeAddedToThisInventoryList(Item i, List<Item> list, int listMaxSpace = -1)
    {
      if (listMaxSpace != -1 && list.Count < listMaxSpace)
        return true;
      int stack = i.Stack;
      foreach (Item obj in list)
      {
        if (obj == null)
          return true;
        if (obj.canStackWith(i) && obj.getRemainingStackSpace() > 0)
        {
          stack -= obj.getRemainingStackSpace();
          if (stack <= 0)
            return true;
        }
      }
      return false;
    }

    public static Item addItemToThisInventoryList(Item i, List<Item> list, int listMaxSpace = -1)
    {
      if (i.Stack == 0)
        i.Stack = 1;
      foreach (Item obj in list)
      {
        if (obj != null && obj.canStackWith(i) && obj.getRemainingStackSpace() > 0)
        {
          i.Stack = obj.addToStack(i.Stack);
          if (i.Stack <= 0)
            return (Item) null;
        }
      }
      for (int index = list.Count - 1; index >= 0; --index)
      {
        if (list[index] == null)
        {
          if (i.Stack > i.maximumStackSize())
          {
            list[index] = i.getOne();
            list[index].Stack = i.maximumStackSize();
            i.Stack -= i.maximumStackSize();
          }
          else
          {
            list[index] = i;
            return (Item) null;
          }
        }
      }
      while (listMaxSpace != -1 && list.Count < listMaxSpace)
      {
        if (i.Stack > i.maximumStackSize())
        {
          Item one = i.getOne();
          one.Stack = i.maximumStackSize();
          i.Stack -= i.maximumStackSize();
          list.Add(one);
        }
        else
        {
          list.Add(i);
          return (Item) null;
        }
      }
      return i;
    }

    public static Item addItemToInventory(Item item, int position, List<Item> items, ItemGrabMenu.behaviorOnItemSelect onAddFunction = null)
    {
      if (items.Equals((object) Game1.player.items) && item is Object && (item as Object).specialItem)
      {
        if ((item as Object).bigCraftable)
        {
          if (!Game1.player.specialBigCraftables.Contains((item as Object).isRecipe ? -(item as Object).parentSheetIndex : (item as Object).parentSheetIndex))
            Game1.player.specialBigCraftables.Add((item as Object).isRecipe ? -(item as Object).parentSheetIndex : (item as Object).parentSheetIndex);
        }
        else if (!Game1.player.specialItems.Contains((item as Object).isRecipe ? -(item as Object).parentSheetIndex : (item as Object).parentSheetIndex))
          Game1.player.specialItems.Add((item as Object).isRecipe ? -(item as Object).parentSheetIndex : (item as Object).parentSheetIndex);
      }
      if (position < 0 || position >= items.Count)
        return item;
      if (items[position] == null)
      {
        items[position] = item;
        if (onAddFunction != null)
          onAddFunction(item, (Farmer) null);
        return (Item) null;
      }
      if (items[position].maximumStackSize() != -1 && items[position].Name.Equals(item.Name) && (!(item is Object) || !(items[position] is Object) || (item as Object).quality == (items[position] as Object).quality && (item as Object).parentSheetIndex == (items[position] as Object).parentSheetIndex) && item.canStackWith(items[position]))
      {
        int stack = items[position].addToStack(item.getStack());
        if (stack <= 0)
          return (Item) null;
        item.Stack = stack;
        if (onAddFunction != null)
          onAddFunction(item, (Farmer) null);
        return item;
      }
      Item obj = items[position];
      if (position == Game1.player.CurrentToolIndex && items.Equals((object) Game1.player.items) && obj != null)
      {
        obj.actionWhenStopBeingHeld(Game1.player);
        item.actionWhenBeingHeld(Game1.player);
      }
      items[position] = item;
      if (onAddFunction != null)
        onAddFunction(item, (Farmer) null);
      return obj;
    }

    public static void spawnObjectAround(Vector2 tileLocation, Object o, GameLocation l)
    {
      if (o == null || l == null || tileLocation.Equals(Vector2.Zero))
        return;
      int num = 0;
      Queue<Vector2> vector2Queue = new Queue<Vector2>();
      HashSet<Vector2> vector2Set = new HashSet<Vector2>();
      vector2Queue.Enqueue(tileLocation);
      Vector2 vector2 = Vector2.Zero;
      for (; num < 100; ++num)
      {
        vector2 = vector2Queue.Dequeue();
        if (l.isTileOccupiedForPlacement(vector2, (Object) null) || l.isOpenWater((int) vector2.X, (int) vector2.Y))
        {
          vector2Set.Add(vector2);
          foreach (Vector2 adjacentTileLocation in Utility.getAdjacentTileLocations(vector2))
          {
            if (!vector2Set.Contains(adjacentTileLocation))
              vector2Queue.Enqueue(adjacentTileLocation);
          }
        }
        else
          break;
      }
      o.isSpawnedObject = true;
      o.canBeGrabbed = true;
      o.tileLocation = vector2;
      if (vector2.Equals(Vector2.Zero) || l.isTileOccupiedForPlacement(vector2, (Object) null) || l.isOpenWater((int) vector2.X, (int) vector2.Y))
        return;
      l.objects.Add(vector2, o);
      if (!l.Equals((object) Game1.currentLocation))
        return;
      Game1.playSound("coin");
      l.temporarySprites.Add(new TemporaryAnimatedSprite(5, vector2 * (float) Game1.tileSize, Color.White, 8, false, 100f, 0, -1, -1f, -1, 0));
    }

    public static Object getTreasureFromGeode(Item geode)
    {
      try
      {
        Random random = new Random((int) Game1.stats.GeodesCracked + (int) Game1.uniqueIDForThisGame / 2);
        int parentSheetIndex = (geode as Object).parentSheetIndex;
        if (random.NextDouble() < 0.5)
        {
          int initialStack = random.Next(3) * 2 + 1;
          if (random.NextDouble() < 0.1)
            initialStack = 10;
          if (random.NextDouble() < 0.01)
            initialStack = 20;
          if (random.NextDouble() < 0.5)
          {
            switch (random.Next(4))
            {
              case 0:
              case 1:
                return new Object(390, initialStack, false, -1, 0);
              case 2:
                return new Object(330, 1, false, -1, 0);
              case 3:
                return new Object(parentSheetIndex == 535 ? 86 : (parentSheetIndex == 536 ? 84 : 82), 1, false, -1, 0);
            }
          }
          else if (parentSheetIndex == 535)
          {
            switch (random.Next(3))
            {
              case 0:
                return new Object(378, initialStack, false, -1, 0);
              case 1:
                return new Object(Game1.player.deepestMineLevel > 25 ? 380 : 378, initialStack, false, -1, 0);
              case 2:
                return new Object(382, initialStack, false, -1, 0);
            }
          }
          else if (parentSheetIndex == 536)
          {
            switch (random.Next(4))
            {
              case 0:
                return new Object(378, initialStack, false, -1, 0);
              case 1:
                return new Object(380, initialStack, false, -1, 0);
              case 2:
                return new Object(382, initialStack, false, -1, 0);
              case 3:
                return new Object(Game1.player.deepestMineLevel > 75 ? 384 : 380, initialStack, false, -1, 0);
            }
          }
          else
          {
            switch (random.Next(5))
            {
              case 0:
                return new Object(378, initialStack, false, -1, 0);
              case 1:
                return new Object(380, initialStack, false, -1, 0);
              case 2:
                return new Object(382, initialStack, false, -1, 0);
              case 3:
                return new Object(384, initialStack, false, -1, 0);
              case 4:
                return new Object(386, initialStack / 2 + 1, false, -1, 0);
            }
          }
        }
        else
        {
          string[] strArray = Game1.objectInformation[parentSheetIndex].Split('/')[6].Split(' ');
          int int32 = Convert.ToInt32(strArray[random.Next(strArray.Length)]);
          if (parentSheetIndex == 749 && random.NextDouble() < 0.008 && (int) Game1.stats.GeodesCracked > 15)
            return new Object(74, 1, false, -1, 0);
          return new Object(int32, 1, false, -1, 0);
        }
      }
      catch (Exception ex)
      {
      }
      return new Object(Vector2.Zero, 390, 1);
    }

    public static Vector2 snapToInt(Vector2 v)
    {
      v.X = (float) (int) v.X;
      v.Y = (float) (int) v.Y;
      return v;
    }

    public static void tryToPlaceItem(GameLocation location, Item item, int x, int y)
    {
      if (item is Tool)
        return;
      Vector2 vector2 = new Vector2((float) (x / Game1.tileSize), (float) (y / Game1.tileSize));
      if (Utility.playerCanPlaceItemHere(location, item, x, y, Game1.player))
      {
        if (!((Object) item).placementAction(location, x, y, Game1.player))
          return;
        if (Game1.IsClient)
          Game1.client.sendMessage((byte) 4, new object[5]
          {
            (object) (short) x,
            (object) (short) y,
            (object) (byte) 0,
            (object) (byte) (((Object) item).bigCraftable ? 1 : 0),
            (object) ((Object) item).ParentSheetIndex
          });
        Game1.player.reduceActiveItemByOne();
      }
      else
        Utility.withinRadiusOfPlayer(Game1.getOldMouseX() + Game1.viewport.X, Game1.getOldMouseY() + Game1.viewport.Y, 3, Game1.player);
    }

    public static int showLanternBar()
    {
      foreach (Item obj in Game1.player.Items)
      {
        if (obj != null && obj.GetType() == typeof (Lantern) && ((Lantern) obj).on)
          return ((Lantern) obj).fuelLeft;
      }
      return -1;
    }

    public static void plantCrops(GameLocation farm, int seedType, int x, int y, int width, int height, int daysOld)
    {
      for (int index1 = x; index1 < x + width; ++index1)
      {
        for (int index2 = y; index2 < y + height; ++index2)
        {
          Vector2 index3 = new Vector2((float) index1, (float) index2);
          farm.makeHoeDirt(index3);
          if (farm.terrainFeatures.ContainsKey(index3) && farm.terrainFeatures[index3].GetType() == typeof (HoeDirt))
            ((HoeDirt) farm.terrainFeatures[index3]).crop = new Crop(seedType, x, y);
        }
      }
    }

    public static bool pointInRectangles(List<Microsoft.Xna.Framework.Rectangle> rectangles, int x, int y)
    {
      foreach (Microsoft.Xna.Framework.Rectangle rectangle in rectangles)
      {
        if (rectangle.Contains(x, y))
          return true;
      }
      return false;
    }

    public static Keys mapGamePadButtonToKey(Buttons b)
    {
      if (b == Buttons.A)
        return Game1.options.getFirstKeyboardKeyFromInputButtonList(Game1.options.actionButton);
      if (b == Buttons.X)
        return Game1.options.getFirstKeyboardKeyFromInputButtonList(Game1.options.useToolButton);
      if (b == Buttons.B)
        return Game1.options.getFirstKeyboardKeyFromInputButtonList(Game1.options.menuButton);
      if (b == Buttons.Back)
        return Game1.options.getFirstKeyboardKeyFromInputButtonList(Game1.options.journalButton);
      if (b == Buttons.Start || b == Buttons.Y)
        return Game1.options.getFirstKeyboardKeyFromInputButtonList(Game1.options.menuButton);
      if (b == Buttons.RightTrigger || b == Buttons.LeftTrigger)
        return Game1.options.getFirstKeyboardKeyFromInputButtonList(Game1.options.toolSwapButton);
      if (b == Buttons.RightShoulder || b == Buttons.LeftShoulder)
        return Game1.options.getFirstKeyboardKeyFromInputButtonList(Game1.options.actionButton);
      if (b == Buttons.DPadUp)
        return Game1.options.getFirstKeyboardKeyFromInputButtonList(Game1.options.moveUpButton);
      if (b == Buttons.DPadRight)
        return Game1.options.getFirstKeyboardKeyFromInputButtonList(Game1.options.moveRightButton);
      if (b == Buttons.DPadDown)
        return Game1.options.getFirstKeyboardKeyFromInputButtonList(Game1.options.moveDownButton);
      if (b == Buttons.DPadLeft)
        return Game1.options.getFirstKeyboardKeyFromInputButtonList(Game1.options.moveLeftButton);
      if (b == Buttons.LeftThumbstickUp)
        return Game1.options.getFirstKeyboardKeyFromInputButtonList(Game1.options.moveUpButton);
      if (b == Buttons.LeftThumbstickRight)
        return Game1.options.getFirstKeyboardKeyFromInputButtonList(Game1.options.moveRightButton);
      if (b == Buttons.LeftThumbstickDown)
        return Game1.options.getFirstKeyboardKeyFromInputButtonList(Game1.options.moveDownButton);
      if (b == Buttons.LeftThumbstickLeft)
        return Game1.options.getFirstKeyboardKeyFromInputButtonList(Game1.options.moveLeftButton);
      return Keys.None;
    }

    public static ButtonCollection getPressedButtons(GamePadState padState, GamePadState oldPadState)
    {
      return new ButtonCollection(ref padState, ref oldPadState);
    }

    public static bool thumbstickIsInDirection(int direction, GamePadState padState)
    {
      if (Game1.currentMinigame != null)
        return true;
      GamePadThumbSticks thumbSticks;
      if (direction == 0)
      {
        thumbSticks = padState.ThumbSticks;
        double num = (double) Math.Abs(thumbSticks.Left.X);
        thumbSticks = padState.ThumbSticks;
        double y = (double) thumbSticks.Left.Y;
        if (num < y)
          return true;
      }
      if (direction == 1)
      {
        thumbSticks = padState.ThumbSticks;
        double x = (double) thumbSticks.Left.X;
        thumbSticks = padState.ThumbSticks;
        double num = (double) Math.Abs(thumbSticks.Left.Y);
        if (x > num)
          return true;
      }
      if (direction == 2)
      {
        thumbSticks = padState.ThumbSticks;
        double num1 = (double) Math.Abs(thumbSticks.Left.X);
        thumbSticks = padState.ThumbSticks;
        double num2 = (double) Math.Abs(thumbSticks.Left.Y);
        if (num1 < num2)
          return true;
      }
      if (direction == 3)
      {
        thumbSticks = padState.ThumbSticks;
        double num1 = (double) Math.Abs(thumbSticks.Left.X);
        thumbSticks = padState.ThumbSticks;
        double num2 = (double) Math.Abs(thumbSticks.Left.Y);
        if (num1 > num2)
          return true;
      }
      return false;
    }

    public static ButtonCollection getHeldButtons(GamePadState padState)
    {
      return new ButtonCollection(ref padState);
    }

    public static bool toggleMuteMusic()
    {
      if (Game1.soundBank != null)
      {
        if ((double) Game1.options.musicVolumeLevel != 0.0)
        {
          Utility.disableMusic();
          return true;
        }
        Utility.enableMusic();
      }
      return false;
    }

    public static void enableMusic()
    {
      if (Game1.soundBank == null)
        return;
      Game1.options.musicVolumeLevel = 0.75f;
      Game1.musicCategory.SetVolume(0.75f);
      Game1.musicPlayerVolume = 0.75f;
      Game1.options.ambientVolumeLevel = 0.75f;
      Game1.ambientCategory.SetVolume(0.75f);
      Game1.ambientPlayerVolume = 0.75f;
    }

    public static void disableMusic()
    {
      if (Game1.soundBank == null)
        return;
      Game1.options.musicVolumeLevel = 0.0f;
      Game1.musicCategory.SetVolume(0.0f);
      Game1.options.ambientVolumeLevel = 0.0f;
      Game1.ambientCategory.SetVolume(0.0f);
      Game1.ambientPlayerVolume = 0.0f;
      Game1.musicPlayerVolume = 0.0f;
    }

    public static Vector2 getVelocityTowardPlayer(Point startingPoint, float speed, Farmer f)
    {
      return Utility.getVelocityTowardPoint(startingPoint, new Vector2((float) f.GetBoundingBox().X, (float) f.GetBoundingBox().Y), speed);
    }

    public static string getHoursMinutesStringFromMilliseconds(uint milliseconds)
    {
      return ((int) (milliseconds / 3600000U)).ToString() + ":" + (milliseconds % 3600000U / 60000U < 10U ? "0" : "") + (object) (milliseconds % 3600000U / 60000U);
    }

    public static string getMinutesSecondsStringFromMilliseconds(int milliseconds)
    {
      return (milliseconds / 60000).ToString() + ":" + (milliseconds % 60000 / 1000 < 10 ? "0" : "") + (object) (milliseconds % 60000 / 1000);
    }

    public static Vector2 getVelocityTowardPoint(Vector2 startingPoint, Vector2 endingPoint, float speed)
    {
      double x1 = (double) endingPoint.X - (double) startingPoint.X;
      double x2 = (double) endingPoint.Y - (double) startingPoint.Y;
      double y = 2.0;
      double num1 = Math.Sqrt(Math.Pow(x1, y) + Math.Pow(x2, 2.0));
      double num2 = num1;
      double num3 = x1 / num2;
      double num4 = x2 / num1;
      double num5 = (double) speed;
      return new Vector2((float) (num3 * num5), (float) num4 * speed);
    }

    public static Vector2 getVelocityTowardPoint(Point startingPoint, Vector2 endingPoint, float speed)
    {
      double x1 = (double) endingPoint.X - (double) startingPoint.X;
      double x2 = (double) endingPoint.Y - (double) startingPoint.Y;
      double y = 2.0;
      double num1 = Math.Sqrt(Math.Pow(x1, y) + Math.Pow(x2, 2.0));
      double num2 = num1;
      double num3 = x1 / num2;
      double num4 = x2 / num1;
      double num5 = (double) speed;
      return new Vector2((float) (num3 * num5), (float) num4 * speed);
    }

    public static Vector2 getRandomPositionInThisRectangle(Microsoft.Xna.Framework.Rectangle r, Random random)
    {
      return new Vector2((float) random.Next(r.X, r.X + r.Width), (float) random.Next(r.Y, r.Y + r.Height));
    }

    public static Vector2 getTopLeftPositionForCenteringOnScreen(int width, int height, int xOffset = 0, int yOffset = 0)
    {
      return new Vector2((float) (Game1.viewport.Width / 2 - width / 2 + xOffset), (float) (Game1.viewport.Height / 2 - height / 2 + yOffset));
    }

    public static void recursiveFindPositionForCharacter(NPC c, GameLocation l, Vector2 tileLocation, int maxIterations)
    {
      int num = 0;
      Queue<Vector2> vector2Queue = new Queue<Vector2>();
      vector2Queue.Enqueue(tileLocation);
      List<Vector2> vector2List = new List<Vector2>();
      for (; num < maxIterations && vector2Queue.Count > 0; ++num)
      {
        Vector2 vector2 = vector2Queue.Dequeue();
        vector2List.Add(vector2);
        c.position = new Vector2(vector2.X * (float) Game1.tileSize + (float) (Game1.tileSize / 2) - (float) (c.GetBoundingBox().Width / 2), vector2.Y * (float) Game1.tileSize - (float) c.GetBoundingBox().Height);
        if (!l.isCollidingPosition(c.GetBoundingBox(), Game1.viewport, false, 0, false, (Character) c, true, false, false))
        {
          if (l.characters.Contains(c))
            break;
          l.characters.Add(c);
          break;
        }
        foreach (Vector2 directionsTileVector in Utility.DirectionsTileVectors)
        {
          if (!vector2List.Contains(vector2 + directionsTileVector))
            vector2Queue.Enqueue(vector2 + directionsTileVector);
        }
      }
    }

    public static Vector2 recursiveFindOpenTileForCharacter(Character c, GameLocation l, Vector2 tileLocation, int maxIterations)
    {
      int num = 0;
      Queue<Vector2> vector2Queue = new Queue<Vector2>();
      vector2Queue.Enqueue(tileLocation);
      List<Vector2> vector2List = new List<Vector2>();
      Vector2 position = c.position;
      for (; num < maxIterations && vector2Queue.Count > 0; ++num)
      {
        Vector2 vector2 = vector2Queue.Dequeue();
        vector2List.Add(vector2);
        c.position = new Vector2(vector2.X * (float) Game1.tileSize + (float) (Game1.tileSize / 2) - (float) (c.GetBoundingBox().Width / 2), vector2.Y * (float) Game1.tileSize + (float) Game1.pixelZoom);
        if (!l.isCollidingPosition(c.GetBoundingBox(), Game1.viewport, c is Farmer, 0, false, c, true, false, false))
        {
          c.position = position;
          return vector2;
        }
        foreach (Vector2 directionsTileVector in Utility.DirectionsTileVectors)
        {
          if (!vector2List.Contains(vector2 + directionsTileVector))
            vector2Queue.Enqueue(vector2 + directionsTileVector);
        }
      }
      c.position = position;
      return Vector2.Zero;
    }

    public static List<Vector2> recursiveFindOpenTiles(GameLocation l, Vector2 tileLocation, int maxOpenTilesToFind = 24, int maxIterations = 50)
    {
      int num = 0;
      Queue<Vector2> vector2Queue = new Queue<Vector2>();
      vector2Queue.Enqueue(tileLocation);
      List<Vector2> vector2List1 = new List<Vector2>();
      List<Vector2> vector2List2;
      for (vector2List2 = new List<Vector2>(); num < maxIterations && vector2Queue.Count > 0 && vector2List2.Count < maxOpenTilesToFind; ++num)
      {
        Vector2 v = vector2Queue.Dequeue();
        vector2List1.Add(v);
        if (l.isTileLocationTotallyClearAndPlaceable(v))
          vector2List2.Add(v);
        foreach (Vector2 directionsTileVector in Utility.DirectionsTileVectors)
        {
          if (!vector2List1.Contains(v + directionsTileVector))
            vector2Queue.Enqueue(v + directionsTileVector);
        }
      }
      return vector2List2;
    }

    public static void spreadAnimalsAround(Building b, Farm environment)
    {
      try
      {
      }
      catch (Exception ex)
      {
      }
    }

    public static void spreadAnimalsAround(Building b, Farm environment, List<FarmAnimal> animalsList)
    {
      if (b.indoors == null || !(b.indoors.GetType() == typeof (AnimalHouse)))
        return;
      Queue<FarmAnimal> farmAnimalQueue = new Queue<FarmAnimal>((IEnumerable<FarmAnimal>) animalsList);
      int num = 0;
      Queue<Vector2> vector2Queue = new Queue<Vector2>();
      vector2Queue.Enqueue(new Vector2((float) (b.tileX + b.animalDoor.X), (float) (b.tileY + b.animalDoor.Y + 1)));
      for (; farmAnimalQueue.Count > 0 && num < 40 && vector2Queue.Count > 0; ++num)
      {
        Vector2 vector2 = vector2Queue.Dequeue();
        farmAnimalQueue.Peek().position = new Vector2(vector2.X * (float) Game1.tileSize + (float) (Game1.tileSize / 2) - (float) (farmAnimalQueue.Peek().GetBoundingBox().Width / 2), vector2.Y * (float) Game1.tileSize - (float) (Game1.tileSize / 2) - (float) (farmAnimalQueue.Peek().GetBoundingBox().Height / 2));
        if (!environment.isCollidingPosition(farmAnimalQueue.Peek().GetBoundingBox(), Game1.viewport, false, 0, false, (Character) farmAnimalQueue.Peek(), true, false, false))
        {
          FarmAnimal farmAnimal = farmAnimalQueue.Dequeue();
          environment.animals.Add(farmAnimal.myID, farmAnimal);
        }
        if (farmAnimalQueue.Count > 0)
        {
          foreach (Vector2 directionsTileVector in Utility.DirectionsTileVectors)
          {
            farmAnimalQueue.Peek().position = new Vector2((vector2.X + directionsTileVector.X) * (float) Game1.tileSize + (float) (Game1.tileSize / 2) - (float) (farmAnimalQueue.Peek().GetBoundingBox().Width / 2), (vector2.Y + directionsTileVector.Y) * (float) Game1.tileSize - (float) (Game1.tileSize / 2) - (float) (farmAnimalQueue.Peek().GetBoundingBox().Height / 2));
            if (!environment.isCollidingPosition(farmAnimalQueue.Peek().GetBoundingBox(), Game1.viewport, false, 0, false, (Character) farmAnimalQueue.Peek(), true, false, false))
              vector2Queue.Enqueue(vector2 + directionsTileVector);
          }
        }
      }
    }

    public static bool[] horizontalOrVerticalCollisionDirections(Microsoft.Xna.Framework.Rectangle boundingBox, bool projectile = false)
    {
      return Utility.horizontalOrVerticalCollisionDirections(boundingBox, (Character) null, projectile);
    }

    public static Point findTile(GameLocation location, int tileIndex, string layer)
    {
      for (int y = 0; y < location.map.GetLayer(layer).LayerHeight; ++y)
      {
        for (int x = 0; x < location.map.GetLayer(layer).LayerWidth; ++x)
        {
          if (location.getTileIndexAt(x, y, layer) == tileIndex)
            return new Point(x, y);
        }
      }
      return new Point(-1, -1);
    }

    public static bool[] horizontalOrVerticalCollisionDirections(Microsoft.Xna.Framework.Rectangle boundingBox, Character c, bool projectile = false)
    {
      bool[] flagArray = new bool[2];
      Microsoft.Xna.Framework.Rectangle position = new Microsoft.Xna.Framework.Rectangle(boundingBox.X, boundingBox.Y, boundingBox.Width, boundingBox.Height);
      position.Width = 1;
      position.X = boundingBox.Center.X;
      if (c != null)
      {
        if (Game1.currentLocation.isCollidingPosition(position, Game1.viewport, false, -1, projectile, c, false, projectile, false))
          flagArray[1] = true;
      }
      else if (Game1.currentLocation.isCollidingPosition(position, Game1.viewport, false, -1, projectile, c, false, projectile, false))
        flagArray[1] = true;
      position.Width = boundingBox.Width;
      position.X = boundingBox.X;
      position.Height = 1;
      position.Y = boundingBox.Center.Y;
      if (c != null)
      {
        if (Game1.currentLocation.isCollidingPosition(position, Game1.viewport, false, -1, projectile, c, false, projectile, false))
          flagArray[0] = true;
      }
      else if (Game1.currentLocation.isCollidingPosition(position, Game1.viewport, false, -1, projectile, c, false, projectile, false))
        flagArray[0] = true;
      return flagArray;
    }

    public static Color getBlendedColor(Color c1, Color c2)
    {
      return new Color(Game1.random.NextDouble() < 0.5 ? (int) Math.Max(c1.R, c2.R) : ((int) c1.R + (int) c2.R) / 2, Game1.random.NextDouble() < 0.5 ? (int) Math.Max(c1.G, c2.G) : ((int) c1.G + (int) c2.G) / 2, Game1.random.NextDouble() < 0.5 ? (int) Math.Max(c1.B, c2.B) : ((int) c1.B + (int) c2.B) / 2);
    }

    public static Character checkForCharacterWithinArea(Type kindOfCharacter, Vector2 positionToAvoid, GameLocation location, Microsoft.Xna.Framework.Rectangle area)
    {
      foreach (NPC character in location.characters)
      {
        if (character.GetType().Equals(kindOfCharacter) && (character.GetBoundingBox().Intersects(area) && !character.position.Equals(positionToAvoid)))
          return (Character) character;
      }
      return (Character) null;
    }

    public static int getNumberOfCharactersInRadius(GameLocation l, Point position, int tileRadius)
    {
      Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle(position.X - tileRadius * Game1.tileSize, position.Y - tileRadius * Game1.tileSize, (tileRadius * 2 + 1) * Game1.tileSize, (tileRadius * 2 + 1) * Game1.tileSize);
      int num = 0;
      foreach (NPC character in l.characters)
      {
        if (rectangle.Contains(Utility.Vector2ToPoint(character.position)))
          ++num;
      }
      return num;
    }

    public static List<Vector2> getListOfTileLocationsForBordersOfNonTileRectangle(Microsoft.Xna.Framework.Rectangle rectangle)
    {
      return new List<Vector2>()
      {
        new Vector2((float) (rectangle.Left / Game1.tileSize), (float) (rectangle.Top / Game1.tileSize)),
        new Vector2((float) (rectangle.Right / Game1.tileSize), (float) (rectangle.Top / Game1.tileSize)),
        new Vector2((float) (rectangle.Left / Game1.tileSize), (float) (rectangle.Bottom / Game1.tileSize)),
        new Vector2((float) (rectangle.Right / Game1.tileSize), (float) (rectangle.Bottom / Game1.tileSize)),
        new Vector2((float) (rectangle.Left / Game1.tileSize), (float) (rectangle.Center.Y / Game1.tileSize)),
        new Vector2((float) (rectangle.Right / Game1.tileSize), (float) (rectangle.Center.Y / Game1.tileSize)),
        new Vector2((float) (rectangle.Center.X / Game1.tileSize), (float) (rectangle.Bottom / Game1.tileSize)),
        new Vector2((float) (rectangle.Center.X / Game1.tileSize), (float) (rectangle.Top / Game1.tileSize)),
        new Vector2((float) (rectangle.Center.X / Game1.tileSize), (float) (rectangle.Center.Y / Game1.tileSize))
      };
    }

    public static void makeTemporarySpriteJuicier(TemporaryAnimatedSprite t, GameLocation l, int numAddOns = 4, int xRange = 64, int yRange = 64)
    {
      t.position.Y -= (float) (Game1.pixelZoom * 2);
      l.temporarySprites.Add(t);
      for (int index = 0; index < numAddOns; ++index)
      {
        TemporaryAnimatedSprite clone = t.getClone();
        clone.delayBeforeAnimationStart = index * 100;
        clone.position += new Vector2((float) Game1.random.Next(-xRange / 2, xRange / 2 + 1), (float) Game1.random.Next(-yRange / 2, yRange / 2 + 1));
        l.temporarySprites.Add(clone);
      }
    }

    public static void recursiveObjectPlacement(Object o, int tileX, int tileY, double growthRate, double decay, GameLocation location, string terrainToExclude = "", int objectIndexAddRange = 0, double failChance = 0.0, int objectIndeAddRangeMultiplier = 1)
    {
      if (!location.isTileLocationOpen(new Location(tileX * Game1.tileSize, tileY * Game1.tileSize)) || location.isTileOccupied(new Vector2((float) tileX, (float) tileY), "") || location.getTileIndexAt(tileX, tileY, "Back") == -1 || !terrainToExclude.Equals("") && (location.doesTileHaveProperty(tileX, tileY, "Type", "Back") == null || location.doesTileHaveProperty(tileX, tileY, "Type", "Back").Equals(terrainToExclude)))
        return;
      Vector2 vector2 = new Vector2((float) tileX, (float) tileY);
      if (Game1.random.NextDouble() > failChance * 2.0)
        location.objects.Add(vector2, new Object(vector2, o.parentSheetIndex + Game1.random.Next(objectIndexAddRange + 1) * objectIndeAddRangeMultiplier, o.name, o.canBeSetDown, o.canBeGrabbed, o.isHoedirt, o.isSpawnedObject)
        {
          fragility = o.fragility,
          minutesUntilReady = o.minutesUntilReady
        });
      growthRate -= decay;
      if (Game1.random.NextDouble() < growthRate)
        Utility.recursiveObjectPlacement(o, tileX + 1, tileY, growthRate, decay, location, terrainToExclude, objectIndexAddRange, failChance, objectIndeAddRangeMultiplier);
      if (Game1.random.NextDouble() < growthRate)
        Utility.recursiveObjectPlacement(o, tileX - 1, tileY, growthRate, decay, location, terrainToExclude, objectIndexAddRange, failChance, objectIndeAddRangeMultiplier);
      if (Game1.random.NextDouble() < growthRate)
        Utility.recursiveObjectPlacement(o, tileX, tileY + 1, growthRate, decay, location, terrainToExclude, objectIndexAddRange, failChance, objectIndeAddRangeMultiplier);
      if (Game1.random.NextDouble() >= growthRate)
        return;
      Utility.recursiveObjectPlacement(o, tileX, tileY - 1, growthRate, decay, location, terrainToExclude, objectIndexAddRange, failChance, objectIndeAddRangeMultiplier);
    }

    public static void recursiveFarmGrassPlacement(int tileX, int tileY, double growthRate, double decay, GameLocation farm)
    {
      if (!farm.isTileLocationOpen(new Location(tileX * Game1.tileSize, tileY * Game1.tileSize)) || farm.isTileOccupied(new Vector2((float) tileX, (float) tileY), "") || farm.doesTileHaveProperty(tileX, tileY, "Diggable", "Back") == null)
        return;
      Vector2 key = new Vector2((float) tileX, (float) tileY);
      if (Game1.random.NextDouble() < 0.05)
        farm.objects.Add(new Vector2((float) tileX, (float) tileY), new Object(new Vector2((float) tileX, (float) tileY), Game1.random.NextDouble() < 0.5 ? 674 : 675, 1));
      else
        farm.terrainFeatures.Add(key, (TerrainFeature) new Grass(1, 4 - (int) ((1.0 - growthRate) * 4.0)));
      growthRate -= decay;
      if (Game1.random.NextDouble() < growthRate)
        Utility.recursiveFarmGrassPlacement(tileX + 1, tileY, growthRate, decay, farm);
      if (Game1.random.NextDouble() < growthRate)
        Utility.recursiveFarmGrassPlacement(tileX - 1, tileY, growthRate, decay, farm);
      if (Game1.random.NextDouble() < growthRate)
        Utility.recursiveFarmGrassPlacement(tileX, tileY + 1, growthRate, decay, farm);
      if (Game1.random.NextDouble() >= growthRate)
        return;
      Utility.recursiveFarmGrassPlacement(tileX, tileY - 1, growthRate, decay, farm);
    }

    public static void recursiveTreePlacement(int tileX, int tileY, double growthRate, int growthStage, double skipChance, GameLocation l, Microsoft.Xna.Framework.Rectangle clearPatch, bool sparse)
    {
      if (clearPatch.Contains(tileX, tileY))
        return;
      Vector2 vector2 = new Vector2((float) tileX, (float) tileY);
      if (l.doesTileHaveProperty((int) vector2.X, (int) vector2.Y, "Diggable", "Back") == null || l.doesTileHaveProperty((int) vector2.X, (int) vector2.Y, "NoSpawn", "Back") != null || (!l.isTileLocationOpen(new Location((int) vector2.X * Game1.tileSize, (int) vector2.Y * Game1.tileSize)) || l.isTileOccupied(vector2, "")) || sparse && (l.isTileOccupied(new Vector2((float) tileX, (float) (tileY - 1)), "") || l.isTileOccupied(new Vector2((float) tileX, (float) (tileY + 1)), "") || (l.isTileOccupied(new Vector2((float) (tileX + 1), (float) tileY), "") || l.isTileOccupied(new Vector2((float) (tileX - 1), (float) tileY), "")) || l.isTileOccupied(new Vector2((float) (tileX + 1), (float) (tileY + 1)), "")))
        return;
      if (Game1.random.NextDouble() > skipChance)
      {
        if (sparse && (double) vector2.X < 70.0 && ((double) vector2.X < 48.0 || (double) vector2.Y > 26.0) && Game1.random.NextDouble() < 0.07)
          (l as Farm).resourceClumps.Add(new ResourceClump(Game1.random.NextDouble() < 0.5 ? 672 : (Game1.random.NextDouble() < 0.5 ? 600 : 602), 2, 2, vector2));
        else
          l.terrainFeatures.Add(vector2, (TerrainFeature) new Tree(Game1.random.Next(1, 4), growthStage < 5 ? Game1.random.Next(5) : 5));
        growthRate -= 0.05;
      }
      if (Game1.random.NextDouble() < growthRate)
        Utility.recursiveTreePlacement(tileX + Game1.random.Next(1, 3), tileY, growthRate, growthStage, skipChance, l, clearPatch, sparse);
      if (Game1.random.NextDouble() < growthRate)
        Utility.recursiveTreePlacement(tileX - Game1.random.Next(1, 3), tileY, growthRate, growthStage, skipChance, l, clearPatch, sparse);
      if (Game1.random.NextDouble() < growthRate)
        Utility.recursiveTreePlacement(tileX, tileY + Game1.random.Next(1, 3), growthRate, growthStage, skipChance, l, clearPatch, sparse);
      if (Game1.random.NextDouble() >= growthRate)
        return;
      Utility.recursiveTreePlacement(tileX, tileY - Game1.random.Next(1, 3), growthRate, growthStage, skipChance, l, clearPatch, sparse);
    }

    public static void recursiveRemoveTerrainFeatures(int tileX, int tileY, double growthRate, double decay, GameLocation l)
    {
      Vector2 key = new Vector2((float) tileX, (float) tileY);
      l.terrainFeatures.Remove(key);
      growthRate -= decay;
      if (Game1.random.NextDouble() < growthRate)
        Utility.recursiveRemoveTerrainFeatures(tileX + 1, tileY, growthRate, decay, l);
      if (Game1.random.NextDouble() < growthRate)
        Utility.recursiveRemoveTerrainFeatures(tileX - 1, tileY, growthRate, decay, l);
      if (Game1.random.NextDouble() < growthRate)
        Utility.recursiveRemoveTerrainFeatures(tileX, tileY + 1, growthRate, decay, l);
      if (Game1.random.NextDouble() >= growthRate)
        return;
      Utility.recursiveRemoveTerrainFeatures(tileX, tileY - 1, growthRate, decay, l);
    }

    public static IEnumerator<int> generateNewFarm(bool skipFarmGeneration)
    {
      return Utility.generateNewFarm(skipFarmGeneration, true);
    }

    public static IEnumerator<int> generateNewFarm(bool skipFarmGeneration, bool loadForNewGame)
    {
      Game1.fadeToBlack = false;
      Game1.fadeToBlackAlpha = 1f;
      Game1.debrisWeather.Clear();
      Game1.viewport.X = -9999;
      Game1.changeMusicTrack("none");
      if (loadForNewGame)
        Game1.loadForNewGame(false);
      if (Game1.IsClient)
      {
        long currentTime = DateTime.Now.Ticks / 10000L;
        while (!Game1.client.isConnected && DateTime.Now.Ticks / 10000L < currentTime + 4000L)
        {
          Thread.Sleep(1);
          Game1.client.receiveMessages();
          yield return 50;
        }
        Game1.loadingMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.6047");
        yield return 75;
        Game1.receiveNewLocationInfoFromServer(Game1.getLocationFromName("FarmHouse"));
        Game1.loadingMessage = Game1.content.LoadString("Strings\\StringsFromCSFiles:Utility.cs.6049");
        yield return 99;
      }
      Game1.currentLocation = Game1.getLocationFromName("Farmhouse");
      Game1.currentLocation.currentEvent = new Event("none/-600 -600/farmer 4 8 2/warp farmer 4 8/end beginGame", -1);
      Game1.gameMode = (byte) 2;
      yield return 100;
    }

    public static void lightSourceOptimization(Vector2 tileLocation)
    {
      List<Vector2> vector2List = new List<Vector2>();
      Queue<Vector2> vector2Queue = new Queue<Vector2>();
      vector2Queue.Enqueue(tileLocation);
      while (vector2Queue.Count != 0)
      {
        Vector2 tileLocation1 = vector2Queue.Dequeue();
        vector2List.Add(tileLocation1);
        Vector2[] tileLocationsArray = Utility.getAdjacentTileLocationsArray(tileLocation1);
        bool flag = true;
        for (int index = 0; index < tileLocationsArray.Length; ++index)
        {
          if (Utility.alreadyHasLightSourceWithThisID((int) ((double) tileLocationsArray[index].X * 2000.0 + (double) tileLocationsArray[index].Y)))
          {
            flag = false;
            break;
          }
        }
        if (flag)
          Utility.removeLightSource((int) ((double) tileLocation1.X * 2000.0 + (double) tileLocation1.Y));
        else if (!tileLocation1.Equals(tileLocation) && !Utility.alreadyHasLightSourceWithThisID((int) ((double) tileLocation1.X * 2000.0 + (double) tileLocation1.Y)))
          Game1.currentLightSources.Add(new LightSource(4, new Vector2(tileLocation.X * (float) Game1.tileSize + (float) (Game1.tileSize / 2), tileLocation.Y * (float) Game1.tileSize), Game1.currentLocation.GetType() == typeof (MineShaft) ? 1.5f : 1.25f, new Color(0, 131, (int) byte.MaxValue) * 0.9f, (int) ((double) tileLocation1.X * 2000.0 + (double) tileLocation1.Y)));
        for (int index = 0; index < tileLocationsArray.Length; ++index)
        {
          if (Game1.currentLocation.Objects.ContainsKey(tileLocationsArray[index]) && Game1.currentLocation.Objects[tileLocationsArray[index]] is Torch && !vector2List.Contains(tileLocationsArray[index]))
            vector2Queue.Enqueue(tileLocationsArray[index]);
        }
      }
    }

    public static bool isOnScreen(Vector2 positionNonTile, int acceptableDistanceFromScreen)
    {
      if ((double) positionNonTile.X > (double) (Game1.viewport.X - acceptableDistanceFromScreen) && (double) positionNonTile.X < (double) (Game1.viewport.X + Game1.viewport.Width + acceptableDistanceFromScreen) && (double) positionNonTile.Y > (double) (Game1.viewport.Y - acceptableDistanceFromScreen))
        return (double) positionNonTile.Y < (double) (Game1.viewport.Y + Game1.viewport.Height + acceptableDistanceFromScreen);
      return false;
    }

    public static bool isOnScreen(Point positionTile, int acceptableDistanceFromScreenNonTile, GameLocation location = null)
    {
      if (location != null && !location.Equals((object) Game1.currentLocation) || (positionTile.X * Game1.tileSize <= Game1.viewport.X - acceptableDistanceFromScreenNonTile || positionTile.X * Game1.tileSize >= Game1.viewport.X + Game1.viewport.Width + acceptableDistanceFromScreenNonTile) || positionTile.Y * Game1.tileSize <= Game1.viewport.Y - acceptableDistanceFromScreenNonTile)
        return false;
      return positionTile.Y * Game1.tileSize < Game1.viewport.Y + Game1.viewport.Height + acceptableDistanceFromScreenNonTile;
    }

    public static void createPotteryTreasure(int tileX, int tileY)
    {
    }

    public static void changeFarmerOverallsColor(Color baseColor)
    {
      int targetColorIndex = 3198 * Game1.player.Sprite.Texture.Bounds.Width + 205;
      Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, targetColorIndex, (int) baseColor.R - 40, (int) baseColor.G - 40, (int) baseColor.B - 40);
      Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, targetColorIndex + 4, (int) baseColor.R, (int) baseColor.G, (int) baseColor.B);
      Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, targetColorIndex + 8, (int) baseColor.R + 15, (int) baseColor.G + 15, (int) baseColor.B + 15);
      Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, targetColorIndex + 12, (int) baseColor.R + 20, (int) baseColor.G + 20, (int) baseColor.B + 20);
      Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, targetColorIndex + 16, (int) baseColor.R + 30, (int) baseColor.G + 30, (int) baseColor.B + 30);
      Game1.player.sprite.Texture = ColorChanger.swapColor(Game1.player.sprite.Texture, targetColorIndex + 20, (int) baseColor.R + 70, (int) baseColor.G + 70, (int) baseColor.B + 70);
    }

    public static void clearObjectsInArea(Microsoft.Xna.Framework.Rectangle r, GameLocation l)
    {
      int left = r.Left;
      while (left < r.Right)
      {
        int top = r.Top;
        while (top < r.Bottom)
        {
          l.removeEverythingFromThisTile(left / Game1.tileSize, top / Game1.tileSize);
          top += Game1.tileSize;
        }
        left += Game1.tileSize;
      }
    }

    public static void recolorDialogueAndMenu(string theme)
    {
      Color color1 = Color.White;
      Color color2 = Color.White;
      Color color3 = Color.White;
      Color color4 = Color.White;
      Color color5 = Color.White;
      Color color6 = Color.White;
      Color color7 = Color.White;
      Color color8 = Color.White;
      Color color9 = Color.White;
      // ISSUE: reference to a compiler-generated method
      uint stringHash = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(theme);
      if (stringHash <= 2465108186U)
      {
        if (stringHash <= 1139731784U)
        {
          if ((int) stringHash != 260554019)
          {
            if ((int) stringHash != 338638484)
            {
              if ((int) stringHash == 1139731784 && theme == "Bombs Away")
              {
                color1 = new Color(50, 20, 0);
                color2 = new Color((int) color1.R + 60, (int) color1.G + 60, (int) color1.B + 60);
                color3 = new Color((int) color2.R + 30, (int) color2.G + 30, (int) color2.B + 30);
                color4 = new Color((int) color3.R + 30, (int) color3.G + 30, (int) color3.B + 30);
                Color tan = Color.Tan;
                color6 = new Color((int) color4.R + 30, (int) color4.G + 30, (int) color4.B + 30);
                color7 = new Color(192, 167, 143);
                color8 = new Color(Math.Min((int) byte.MaxValue, (int) color7.R + 30), Math.Min((int) byte.MaxValue, (int) color7.G + 30), Math.Min((int) byte.MaxValue, (int) color7.B + 30));
                color9 = new Color((int) color7.R - 30, (int) color7.G - 30, (int) color7.B - 30);
              }
            }
            else if (theme == "Ghosts N' Goblins")
            {
              color1 = new Color(55, 0, 0);
              color2 = new Color((int) color1.R + 60, (int) color1.G + 60, (int) color1.B + 60);
              color3 = new Color((int) color2.R + 30, (int) color2.G + 30, (int) color2.B + 30);
              color4 = new Color((int) color3.R + 30, (int) color3.G + 30, (int) color3.B + 30);
              color5 = new Color((int) color4.R + 15, (int) color4.G + 15, (int) color4.B + 15);
              color6 = new Color((int) color5.R + 15, (int) color5.G + 15, (int) color5.B + 15);
              color7 = new Color(196, 197, 230);
              color8 = new Color(Math.Min((int) byte.MaxValue, (int) color7.R + 30), Math.Min((int) byte.MaxValue, (int) color7.G + 30), Math.Min((int) byte.MaxValue, (int) color7.B + 30));
              color9 = new Color((int) color7.R - 30, (int) color7.G - 30, (int) color7.B - 30);
            }
          }
          else if (theme == "Polynomial")
          {
            color1 = new Color(60, 60, 60);
            color2 = new Color((int) color1.R + 60, (int) color1.G + 60, (int) color1.B + 60);
            color3 = new Color((int) color2.R + 30, (int) color2.G + 30, (int) color2.B + 30);
            color4 = new Color((int) color3.R + 30, (int) color3.G + 30, (int) color3.B + 30);
            color6 = new Color(254, 254, 254);
            color5 = new Color((int) color4.R + 30, (int) color4.G + 30, (int) color4.B + 30);
            color7 = new Color(225, 225, 225);
            color8 = new Color(Math.Min((int) byte.MaxValue, (int) color7.R + 30), Math.Min((int) byte.MaxValue, (int) color7.G + 30), Math.Min((int) byte.MaxValue, (int) color7.B + 30));
            color9 = new Color((int) color7.R - 30, (int) color7.G - 30, (int) color7.B - 30);
          }
        }
        else if ((int) stringHash != 1519897354)
        {
          if ((int) stringHash != 2098783532)
          {
            if ((int) stringHash == -1829859110 && theme == "Sports")
            {
              color1 = new Color(110, 45, 0);
              color2 = new Color((int) color1.R + 60, (int) color1.G + 60, (int) color1.B + 60);
              color3 = new Color((int) color2.R + 30, (int) color2.G + 30, (int) color2.B + 30);
              color4 = new Color((int) color3.R + 30, (int) color3.G + 30, (int) color3.B + 30);
              color5 = new Color((int) color4.R + 15, (int) color4.G + 15, (int) color4.B + 15);
              color6 = new Color((int) color5.R + 15, (int) color5.G + 15, (int) color5.B + 15);
              color7 = new Color((int) byte.MaxValue, 214, 168);
              color8 = new Color(Math.Min((int) byte.MaxValue, (int) color7.R + 30), Math.Min((int) byte.MaxValue, (int) color7.G + 30), Math.Min((int) byte.MaxValue, (int) color7.B + 30));
              color9 = new Color((int) color7.R - 30, (int) color7.G - 30, (int) color7.B - 30);
            }
          }
          else if (theme == "Wasteland")
          {
            color1 = new Color(14, 12, 10);
            color2 = new Color((int) color1.R + 60, (int) color1.G + 60, (int) color1.B + 60);
            color3 = new Color((int) color2.R + 30, (int) color2.G + 30, (int) color2.B + 30);
            color4 = new Color((int) color3.R + 30, (int) color3.G + 30, (int) color3.B + 30);
            color5 = new Color((int) color4.R + 15, (int) color4.G + 15, (int) color4.B + 15);
            color6 = new Color((int) color5.R + 15, (int) color5.G + 15, (int) color5.B + 15);
            color7 = new Color(185, 178, 165);
            color8 = new Color(Math.Min((int) byte.MaxValue, (int) color7.R + 30), Math.Min((int) byte.MaxValue, (int) color7.G + 30), Math.Min((int) byte.MaxValue, (int) color7.B + 30));
            color9 = new Color((int) color7.R - 30, (int) color7.G - 30, (int) color7.B - 30);
          }
        }
        else if (theme == "Earthy")
        {
          color1 = new Color(44, 35, 0);
          color2 = new Color(115, 147, 102);
          color3 = new Color(91, 65, 0);
          color4 = new Color(122, 83, 0);
          color5 = new Color(179, 181, 125);
          color6 = new Color(144, 96, 0);
          color7 = new Color(234, 227, 190);
          color8 = new Color((int) byte.MaxValue, (int) byte.MaxValue, 227);
          color9 = new Color(193, 187, 156);
        }
      }
      else if (stringHash <= 2707891832U)
      {
        if ((int) stringHash != -1700225014)
        {
          if ((int) stringHash != -1692243438)
          {
            if ((int) stringHash == -1587075464 && theme == "Skyscape")
            {
              color1 = new Color(15, 31, 57);
              color2 = new Color((int) color1.R + 60, (int) color1.G + 60, (int) color1.B + 60);
              color3 = new Color((int) color2.R + 30, (int) color2.G + 30, (int) color2.B + 30);
              color4 = new Color((int) color3.R + 30, (int) color3.G + 30, (int) color3.B + 30);
              color5 = new Color((int) color4.R + 15, (int) color4.G + 15, (int) color4.B + 15);
              color6 = new Color((int) color5.R + 15, (int) color5.G + 15, (int) color5.B + 15);
              color7 = new Color(206, 237, 254);
              color8 = new Color(Math.Min((int) byte.MaxValue, (int) color7.R + 30), Math.Min((int) byte.MaxValue, (int) color7.G + 30), Math.Min((int) byte.MaxValue, (int) color7.B + 30));
              color9 = new Color((int) color7.R - 30, (int) color7.G - 30, (int) color7.B - 30);
            }
          }
          else if (theme == "Sweeties")
          {
            color1 = new Color(120, 60, 60);
            color2 = new Color((int) color1.R + 60, (int) color1.G + 60, (int) color1.B + 60);
            color3 = new Color((int) color2.R + 30, (int) color2.G + 30, (int) color2.B + 30);
            color4 = new Color((int) color3.R + 30, (int) color3.G + 30, (int) color3.B + 30);
            color5 = new Color((int) color4.R + 15, (int) color4.G + 15, (int) color4.B + 15);
            color6 = new Color((int) color5.R + 15, (int) color5.G + 15, (int) color5.B + 15);
            color7 = new Color((int) byte.MaxValue, 213, 227);
            color8 = new Color(Math.Min((int) byte.MaxValue, (int) color7.R + 30), Math.Min((int) byte.MaxValue, (int) color7.G + 30), Math.Min((int) byte.MaxValue, (int) color7.B + 30));
            color9 = new Color((int) color7.R - 30, (int) color7.G - 30, (int) color7.B - 30);
          }
        }
        else if (theme == "Duchess")
        {
          color1 = new Color(69, 45, 0);
          color2 = new Color((int) color1.R + 60, (int) color1.G + 60, (int) color1.B + 30);
          color3 = new Color((int) color2.R + 30, (int) color2.G + 30, (int) color2.B + 20);
          color4 = new Color((int) color3.R + 30, (int) color3.G + 30, (int) color3.B + 20);
          color5 = new Color((int) color4.R + 15, (int) color4.G + 15, (int) color4.B + 10);
          color6 = new Color((int) color5.R + 15, (int) color5.G + 15, (int) color5.B + 10);
          color7 = new Color(227, 221, 174);
          color8 = new Color(Math.Min((int) byte.MaxValue, (int) color7.R + 30), Math.Min((int) byte.MaxValue, (int) color7.G + 30), Math.Min((int) byte.MaxValue, (int) color7.B + 30));
          color9 = new Color((int) color7.R - 30, (int) color7.G - 30, (int) color7.B - 30);
        }
      }
      else if ((int) stringHash != -1425201616)
      {
        if ((int) stringHash != -1243996382)
        {
          if ((int) stringHash == -684751651 && theme == "Basic")
          {
            color1 = new Color(47, 46, 36);
            color2 = new Color((int) color1.R + 60, (int) color1.G + 60, (int) color1.B + 60);
            color3 = new Color((int) color2.R + 30, (int) color2.G + 30, (int) color2.B + 30);
            color4 = new Color((int) color3.R + 30, (int) color3.G + 30, (int) color3.B + 30);
            color5 = new Color((int) color4.R + 15, (int) color4.G + 15, (int) color4.B + 15);
            color6 = new Color((int) color5.R + 15, (int) color5.G + 15, (int) color5.B + 15);
            color7 = new Color(220, 215, 194);
            color8 = new Color(Math.Min((int) byte.MaxValue, (int) color7.R + 30), Math.Min((int) byte.MaxValue, (int) color7.G + 30), Math.Min((int) byte.MaxValue, (int) color7.B + 30));
            color9 = new Color((int) color7.R - 30, (int) color7.G - 30, (int) color7.B - 30);
          }
        }
        else if (theme == "Outer Space")
        {
          color1 = new Color(20, 20, 20);
          color2 = new Color((int) color1.R + 60, (int) color1.G + 60, (int) color1.B + 60);
          color3 = new Color((int) color2.R + 30, (int) color2.G + 30, (int) color2.B + 30);
          color4 = new Color((int) color3.R + 30, (int) color3.G + 30, (int) color3.B + 30);
          color5 = new Color((int) color4.R + 15, (int) color4.G + 15, (int) color4.B + 15);
          color6 = new Color((int) color5.R + 15, (int) color5.G + 15, (int) color5.B + 15);
          color7 = new Color(194, 189, 202);
          color8 = new Color(Math.Min((int) byte.MaxValue, (int) color7.R + 30), Math.Min((int) byte.MaxValue, (int) color7.G + 30), Math.Min((int) byte.MaxValue, (int) color7.B + 30));
          color9 = new Color((int) color7.R - 30, (int) color7.G - 30, (int) color7.B - 30);
        }
      }
      else if (theme == "Biomes")
      {
        color1 = new Color(17, 36, 0);
        color2 = new Color((int) color1.R + 60, (int) color1.G + 60, (int) color1.B + 60);
        color3 = new Color((int) color2.R + 30, (int) color2.G + 30, (int) color2.B + 30);
        color4 = new Color((int) color3.R + 30, (int) color3.G + 30, (int) color3.B + 30);
        color5 = new Color((int) color4.R + 15, (int) color4.G + 15, (int) color4.B + 15);
        color6 = new Color((int) color5.R + 15, (int) color5.G + 15, (int) color5.B + 15);
        color7 = new Color(192, (int) byte.MaxValue, 183);
        color8 = new Color(Math.Min((int) byte.MaxValue, (int) color7.R + 30), Math.Min((int) byte.MaxValue, (int) color7.G + 30), Math.Min((int) byte.MaxValue, (int) color7.B + 30));
        color9 = new Color((int) color7.R - 30, (int) color7.G - 30, (int) color7.B - 30);
      }
      Game1.menuTexture = ColorChanger.swapColor(Game1.menuTexture, 15633, (int) color1.R, (int) color1.G, (int) color1.B);
      Game1.menuTexture = ColorChanger.swapColor(Game1.menuTexture, 15645, (int) color6.R, (int) color6.G, (int) color6.B);
      Game1.menuTexture = ColorChanger.swapColor(Game1.menuTexture, 15649, (int) color4.R, (int) color4.G, (int) color4.B);
      Game1.menuTexture = ColorChanger.swapColor(Game1.menuTexture, 15641, (int) color4.R, (int) color4.G, (int) color4.B);
      Game1.menuTexture = ColorChanger.swapColor(Game1.menuTexture, 15637, (int) color3.R, (int) color3.G, (int) color3.B);
      Game1.menuTexture = ColorChanger.swapColor(Game1.menuTexture, 15666, (int) color7.R, (int) color7.G, (int) color7.B);
      Game1.menuTexture = ColorChanger.swapColor(Game1.menuTexture, 40577, (int) color8.R, (int) color8.G, (int) color8.B);
      Game1.menuTexture = ColorChanger.swapColor(Game1.menuTexture, 40637, (int) color9.R, (int) color9.G, (int) color9.B);
      Game1.toolIconBox = ColorChanger.swapColor(Game1.toolIconBox, 1760, (int) color1.R, (int) color1.G, (int) color1.B);
      Game1.toolIconBox = ColorChanger.swapColor(Game1.toolIconBox, 1764, (int) color3.R, (int) color3.G, (int) color3.B);
      Game1.toolIconBox = ColorChanger.swapColor(Game1.toolIconBox, 1768, (int) color4.R, (int) color4.G, (int) color4.B);
      Game1.toolIconBox = ColorChanger.swapColor(Game1.toolIconBox, 1841, (int) color6.R, (int) color6.G, (int) color6.B);
      Game1.toolIconBox = ColorChanger.swapColor(Game1.toolIconBox, 1792, (int) color7.R, (int) color7.G, (int) color7.B);
      Game1.toolIconBox = ColorChanger.swapColor(Game1.toolIconBox, 1834, (int) color8.R, (int) color8.G, (int) color8.B);
      Game1.toolIconBox = ColorChanger.swapColor(Game1.toolIconBox, 1773, (int) color9.R, (int) color9.G, (int) color9.B);
    }
  }
}
