// Decompiled with JetBrains decompiler
// Type: StardewValley.Menus.MapPage
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley.BellsAndWhistles;
using System;
using System.Collections.Generic;

namespace StardewValley.Menus
{
  public class MapPage : IClickableMenu
  {
    private string descriptionText = "";
    private string hoverText = "";
    public List<ClickableComponent> points = new List<ClickableComponent>();
    public const int region_desert = 1001;
    public const int region_farm = 1002;
    public const int region_backwoods = 1003;
    public const int region_busstop = 1004;
    public const int region_wizardtower = 1005;
    public const int region_marnieranch = 1006;
    public const int region_leahcottage = 1007;
    public const int region_samhouse = 1008;
    public const int region_haleyhouse = 1009;
    public const int region_townsquare = 1010;
    public const int region_harveyclinic = 1011;
    public const int region_generalstore = 1012;
    public const int region_blacksmith = 1013;
    public const int region_saloon = 1014;
    public const int region_manor = 1015;
    public const int region_museum = 1016;
    public const int region_elliottcabin = 1017;
    public const int region_sewer = 1018;
    public const int region_graveyard = 1019;
    public const int region_trailer = 1020;
    public const int region_alexhouse = 1021;
    public const int region_sciencehouse = 1022;
    public const int region_tent = 1023;
    public const int region_mines = 1024;
    public const int region_adventureguild = 1025;
    public const int region_quarry = 1026;
    public const int region_jojamart = 1027;
    public const int region_fishshop = 1028;
    public const int region_spa = 1029;
    public const int region_secretwoods = 1030;
    public const int region_ruinedhouse = 1031;
    public const int region_communitycenter = 1032;
    public const int region_sewerpipe = 1033;
    public const int region_railroad = 1034;
    private string playerLocationName;
    private Texture2D map;
    private int mapX;
    private int mapY;
    private Vector2 playerMapPosition;
    public ClickableTextureComponent okButton;

    public MapPage(int x, int y, int width, int height)
      : base(x, y, width, height, false)
    {
      this.okButton = new ClickableTextureComponent(Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11059"), new Rectangle(this.xPositionOnScreen + width + Game1.tileSize, this.yPositionOnScreen + height - IClickableMenu.borderWidth - Game1.tileSize / 4, Game1.tileSize, Game1.tileSize), (string) null, (string) null, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f, false);
      this.map = Game1.content.Load<Texture2D>("LooseSprites\\map");
      Vector2 centeringOnScreen = Utility.getTopLeftPositionForCenteringOnScreen(this.map.Bounds.Width * Game1.pixelZoom, 180 * Game1.pixelZoom, 0, 0);
      this.mapX = (int) centeringOnScreen.X;
      this.mapY = (int) centeringOnScreen.Y;
      this.points.Add(new ClickableComponent(new Rectangle(this.mapX, this.mapY, 292, 152), Game1.player.mailReceived.Contains("ccVault") ? Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11062") : "???")
      {
        myID = 1001,
        rightNeighborID = 1003,
        downNeighborID = 1030
      });
      this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 324, this.mapY + 252, 188, 132), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11064", (object) Game1.player.farmName))
      {
        myID = 1002,
        leftNeighborID = 1005,
        upNeighborID = 1003,
        rightNeighborID = 1004,
        downNeighborID = 1006
      });
      this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 360, this.mapY + 96, 188, 132), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11065"))
      {
        myID = 1003,
        downNeighborID = 1002,
        leftNeighborID = 1001,
        rightNeighborID = 1022,
        upNeighborID = 1029
      });
      this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 516, this.mapY + 224, 76, 100), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11066"))
      {
        myID = 1004,
        leftNeighborID = 1002,
        upNeighborID = 1003,
        downNeighborID = 1006,
        rightNeighborID = 1011
      });
      this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 196, this.mapY + 352, 36, 76), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11067"))
      {
        myID = 1005,
        upNeighborID = 1001,
        downNeighborID = 1031,
        rightNeighborID = 1006,
        leftNeighborID = 1030
      });
      this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 420, this.mapY + 392, 76, 40), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11068") + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11069"))
      {
        myID = 1006,
        leftNeighborID = 1005,
        downNeighborID = 1007,
        upNeighborID = 1002,
        rightNeighborID = 1008
      });
      this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 452, this.mapY + 436, 32, 24), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11070"))
      {
        myID = 1007,
        upNeighborID = 1006,
        downNeighborID = 1033,
        leftNeighborID = 1005,
        rightNeighborID = 1008
      });
      this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 612, this.mapY + 396, 36, 52), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11071") + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11072"))
      {
        myID = 1008,
        leftNeighborID = 1006,
        upNeighborID = 1010,
        rightNeighborID = 1009
      });
      this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 652, this.mapY + 408, 40, 36), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11073") + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11074"))
      {
        myID = 1009,
        leftNeighborID = 1008,
        upNeighborID = 1010,
        rightNeighborID = 1018
      });
      this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 672, this.mapY + 340, 44, 60), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11075"))
      {
        myID = 1010,
        leftNeighborID = 1008,
        downNeighborID = 1009,
        rightNeighborID = 1014,
        upNeighborID = 1011
      });
      this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 680, this.mapY + 304, 16, 32), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11076") + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11077"))
      {
        myID = 1011,
        leftNeighborID = 1004,
        rightNeighborID = 1012,
        downNeighborID = 1010,
        upNeighborID = 1032
      });
      this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 696, this.mapY + 296, 28, 40), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11078") + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11079") + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11080"))
      {
        myID = 1012,
        leftNeighborID = 1011,
        downNeighborID = 1014,
        rightNeighborID = 1021,
        upNeighborID = 1032
      });
      this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 852, this.mapY + 388, 80, 36), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11081") + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11082"))
      {
        myID = 1013,
        upNeighborID = 1027,
        rightNeighborID = 1016,
        downNeighborID = 1017,
        leftNeighborID = 1015
      });
      this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 716, this.mapY + 352, 28, 40), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11083") + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11084"))
      {
        myID = 1014,
        leftNeighborID = 1010,
        rightNeighborID = 1020,
        downNeighborID = 1019,
        upNeighborID = 1012
      });
      this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 768, this.mapY + 388, 44, 56), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11085"))
      {
        myID = 1015,
        leftNeighborID = 1019,
        upNeighborID = 1020,
        rightNeighborID = 1013,
        downNeighborID = 1017
      });
      this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 892, this.mapY + 416, 32, 28), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11086") + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11087"))
      {
        myID = 1016,
        downNeighborID = 1017,
        leftNeighborID = 1013,
        upNeighborID = 1027,
        rightNeighborID = 99989
      });
      this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 824, this.mapY + 564, 28, 20), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11088"))
      {
        myID = 1017,
        downNeighborID = 1028,
        upNeighborID = 1015,
        rightNeighborID = 99989
      });
      this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 696, this.mapY + 448, 24, 20), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11089"))
      {
        myID = 1018,
        downNeighborID = 1017,
        rightNeighborID = 1019,
        upNeighborID = 1014,
        leftNeighborID = 1009
      });
      this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 724, this.mapY + 424, 40, 32), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11090"))
      {
        myID = 1019,
        leftNeighborID = 1018,
        upNeighborID = 1014,
        rightNeighborID = 1015,
        downNeighborID = 1017
      });
      this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 780, this.mapY + 360, 24, 20), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11091"))
      {
        myID = 1020,
        upNeighborID = 1021,
        leftNeighborID = 1014,
        downNeighborID = 1015,
        rightNeighborID = 1027
      });
      this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 748, this.mapY + 316, 36, 36), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11092") + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11093"))
      {
        myID = 1021,
        rightNeighborID = 1027,
        downNeighborID = 1020,
        leftNeighborID = 1012,
        upNeighborID = 1032
      });
      this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 732, this.mapY + 148, 48, 32), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11094") + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11095") + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11096"))
      {
        myID = 1022,
        downNeighborID = 1032,
        leftNeighborID = 1003,
        upNeighborID = 1034,
        rightNeighborID = 1023
      });
      this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 784, this.mapY + 128, 12, 16), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11097"))
      {
        myID = 1023,
        leftNeighborID = 1034,
        downNeighborID = 1022,
        rightNeighborID = 1024
      });
      this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 880, this.mapY + 96, 16, 24), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11098"))
      {
        myID = 1024,
        leftNeighborID = 1023,
        rightNeighborID = 1025,
        downNeighborID = 1027
      });
      this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 900, this.mapY + 108, 32, 36), Game1.stats.DaysPlayed >= 5U ? Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11099") + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11100") : "???")
      {
        myID = 1025,
        leftNeighborID = 1024,
        rightNeighborID = 1026,
        downNeighborID = 1027
      });
      this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 968, this.mapY + 116, 88, 76), Game1.player.mailReceived.Contains("ccCraftsRoom") ? Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11103") : "???")
      {
        myID = 1026,
        leftNeighborID = 1025,
        downNeighborID = 1027
      });
      this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 872, this.mapY + 280, 52, 52), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11105") + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11106"))
      {
        myID = 1027,
        upNeighborID = 1025,
        leftNeighborID = 1021,
        downNeighborID = 1013
      });
      this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 844, this.mapY + 608, 36, 40), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11107") + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11108"))
      {
        myID = 1028,
        upNeighborID = 1017,
        rightNeighborID = 99989
      });
      this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 576, this.mapY + 60, 48, 36), Game1.isLocationAccessible("Railroad") ? Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11110") + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11111") : "???")
      {
        myID = 1029,
        rightNeighborID = 1034,
        downNeighborID = 1003,
        leftNeighborID = 1001
      });
      this.points.Add(new ClickableComponent(new Rectangle(this.mapX, this.mapY + 272, 196, 176), Game1.player.mailReceived.Contains("beenToWoods") ? Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11114") : "???")
      {
        myID = 1030,
        upNeighborID = 1001,
        rightNeighborID = 1005
      });
      this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 260, this.mapY + 572, 20, 20), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11116"))
      {
        myID = 1031,
        rightNeighborID = 1033,
        upNeighborID = 1005
      });
      this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 692, this.mapY + 204, 44, 36), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11117"))
      {
        myID = 1032,
        downNeighborID = 1012,
        upNeighborID = 1022,
        leftNeighborID = 1004
      });
      this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 380, this.mapY + 596, 24, 32), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11118"))
      {
        myID = 1033,
        leftNeighborID = 1031,
        rightNeighborID = 1017,
        upNeighborID = 1007
      });
      this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 644, this.mapY + 64, 16, 8), Game1.isLocationAccessible("Railroad") ? Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11119") : "???")
      {
        myID = 1034,
        leftNeighborID = 1029,
        rightNeighborID = 1023,
        downNeighborID = 1022
      });
      this.points.Add(new ClickableComponent(new Rectangle(this.mapX + 728, this.mapY + 652, 28, 28), Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11122")));
      this.setUpPlayerMapPosition();
    }

    public override void snapToDefaultClickableComponent()
    {
      this.currentlySnappedComponent = this.getComponentWithID(1002);
      this.snapCursorToCurrentSnappedComponent();
    }

    public void setUpPlayerMapPosition()
    {
      this.playerMapPosition = new Vector2(-999f, -999f);
      string str = Game1.player.currentLocation.Name;
      string name1 = Game1.player.currentLocation.Name;
      // ISSUE: reference to a compiler-generated method
      uint stringHash1 = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(name1);
      if (stringHash1 <= 2026102357U)
      {
        if (stringHash1 <= 1078463463U)
        {
          if (stringHash1 <= 278567071U)
          {
            if ((int) stringHash1 != 144182059)
            {
              if ((int) stringHash1 != 263498407)
              {
                if ((int) stringHash1 != 278567071 || !(name1 == "HarveyRoom"))
                  goto label_79;
              }
              else if (name1 == "BathHouse_Pool")
                goto label_69;
              else
                goto label_79;
            }
            else if (name1 == "WizardHouseBasement")
              goto label_68;
            else
              goto label_79;
          }
          else if (stringHash1 <= 746089795U)
          {
            if ((int) stringHash1 != 437214172)
            {
              if ((int) stringHash1 != 746089795 || !(name1 == "ScienceHouse"))
                goto label_79;
              else
                goto label_72;
            }
            else if (name1 == "Desert")
              goto label_63;
            else
              goto label_79;
          }
          else if ((int) stringHash1 != 807500499)
          {
            if ((int) stringHash1 == 1078463463 && name1 == "Temp" && Game1.player.currentLocation.Map.Id.Contains("Town"))
            {
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11190");
              goto label_79;
            }
            else
              goto label_79;
          }
          else if (!(name1 == "Hospital"))
            goto label_79;
          str = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11076") + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11077");
          goto label_79;
        }
        else if (stringHash1 <= 1446049731U)
        {
          if (stringHash1 <= 1253908523U)
          {
            if ((int) stringHash1 != 1167876998)
            {
              if ((int) stringHash1 == 1253908523 && name1 == "JoshHouse")
              {
                str = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11092") + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11093");
                goto label_79;
              }
              else
                goto label_79;
            }
            else if (name1 == "ManorHouse")
            {
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11085");
              goto label_79;
            }
            else
              goto label_79;
          }
          else if ((int) stringHash1 != 1428365440)
          {
            if ((int) stringHash1 == 1446049731 && name1 == "CommunityCenter")
            {
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11117");
              goto label_79;
            }
            else
              goto label_79;
          }
          else if (name1 == "SeedShop")
          {
            str = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11078") + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11079") + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11080");
            goto label_79;
          }
          else
            goto label_79;
        }
        else if (stringHash1 <= 1840909614U)
        {
          if ((int) stringHash1 != 1807680626)
          {
            if ((int) stringHash1 != 1840909614 || !(name1 == "SandyHouse"))
              goto label_79;
          }
          else if (!(name1 == "SandyShop"))
            goto label_79;
        }
        else if ((int) stringHash1 != 1919215024)
        {
          if ((int) stringHash1 != 2026102357 || !(name1 == "UndergroundMine"))
            goto label_79;
          else
            goto label_71;
        }
        else if (name1 == "ElliottHouse")
        {
          str = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11088");
          goto label_79;
        }
        else
          goto label_79;
      }
      else if (stringHash1 <= 3095702198U)
      {
        if (stringHash1 <= 2706464810U)
        {
          if (stringHash1 <= 2204429310U)
          {
            if ((int) stringHash1 != 2028543928)
            {
              if ((int) stringHash1 != -2090537986 || !(name1 == "SebastianRoom"))
                goto label_79;
              else
                goto label_72;
            }
            else if (name1 == "Backwoods")
              goto label_79;
            else
              goto label_79;
          }
          else if ((int) stringHash1 != -1816351185)
          {
            if ((int) stringHash1 != -1588502486 || !(name1 == "WizardHouse"))
              goto label_79;
            else
              goto label_68;
          }
          else if (name1 == "BathHouse_Entry")
            goto label_69;
          else
            goto label_79;
        }
        else if (stringHash1 <= 2844260897U)
        {
          if ((int) stringHash1 != -1585981025)
          {
            if ((int) stringHash1 == -1450706399 && name1 == "Woods")
            {
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11114");
              goto label_79;
            }
            else
              goto label_79;
          }
          else if (name1 == "ArchaeologyHouse")
          {
            str = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11086");
            goto label_79;
          }
          else
            goto label_79;
        }
        else if ((int) stringHash1 != -1288340593)
        {
          if ((int) stringHash1 == -1199265098 && name1 == "AdventureGuild")
          {
            str = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11099");
            goto label_79;
          }
          else
            goto label_79;
        }
        else if (name1 == "FishShop")
        {
          str = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11107");
          goto label_79;
        }
        else
          goto label_79;
      }
      else
      {
        if (stringHash1 <= 3848897750U)
        {
          if (stringHash1 <= 3647688262U)
          {
            if ((int) stringHash1 != -1106302145)
            {
              if ((int) stringHash1 != -647279034 || !(name1 == "BathHouse_WomensLocker"))
                goto label_79;
              else
                goto label_69;
            }
            else if (!(name1 == "Railroad"))
              goto label_79;
          }
          else if ((int) stringHash1 != -641179001)
          {
            if ((int) stringHash1 != -446069546 || !(name1 == "Mine"))
              goto label_79;
            else
              goto label_71;
          }
          else if (name1 == "SkullCave")
            goto label_63;
          else
            goto label_79;
        }
        else if (stringHash1 <= 3978811393U)
        {
          if ((int) stringHash1 != -370771440)
          {
            if ((int) stringHash1 == -316155903 && name1 == "AnimalShop")
            {
              str = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11068");
              goto label_79;
            }
            else
              goto label_79;
          }
          else if (name1 == "BathHouse_MensLocker")
            goto label_69;
          else
            goto label_79;
        }
        else if ((int) stringHash1 != -315394387)
        {
          if ((int) stringHash1 != -292510757 || !(name1 == "WitchWarpCave"))
            goto label_79;
        }
        else if (name1 == "Club")
          goto label_63;
        else
          goto label_79;
        str = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11119");
        goto label_79;
      }
label_63:
      str = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11062");
      goto label_79;
label_68:
      str = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11067");
      goto label_79;
label_69:
      str = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11110") + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11111");
      goto label_79;
label_71:
      str = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11098");
      goto label_79;
label_72:
      str = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11094") + Environment.NewLine + Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11095");
label_79:
      foreach (ClickableComponent point in this.points)
      {
        if (point.name.Equals(str) || point.name.Replace(" ", "").Equals(str) || point.name.Contains(Environment.NewLine) && point.name.Substring(0, point.name.IndexOf(Environment.NewLine)).Equals(str.Substring(0, str.Contains(Environment.NewLine) ? str.IndexOf(Environment.NewLine) : str.Length)))
        {
          this.playerMapPosition = new Vector2((float) point.bounds.Center.X, (float) point.bounds.Center.Y);
          this.playerLocationName = point.name.Contains(Environment.NewLine) ? point.name.Substring(0, point.name.IndexOf(Environment.NewLine)) : point.name;
          return;
        }
      }
      int tileX = Game1.player.getTileX();
      int tileY = Game1.player.getTileY();
      string name2 = Game1.player.currentLocation.name;
      // ISSUE: reference to a compiler-generated method
      uint stringHash2 = \u003CPrivateImplementationDetails\u003E.ComputeStringHash(name2);
      if (stringHash2 <= 2151182681U)
      {
        if (stringHash2 <= 1078463463U)
        {
          if (stringHash2 <= 411937663U)
          {
            if ((int) stringHash2 != 109565081)
            {
              if ((int) stringHash2 != 411937663 || !(name2 == "Shed"))
                return;
            }
            else if (!(name2 == "Slime Hutch"))
              return;
          }
          else if ((int) stringHash2 != 784782095)
          {
            if ((int) stringHash2 != 846075854)
            {
              if ((int) stringHash2 != 1078463463 || !(name2 == "Temp") || !Game1.player.currentLocation.Map.Id.Contains("Town"))
                return;
              if (tileX > 84 && tileY < 68)
              {
                this.playerMapPosition = new Vector2((float) (this.mapX + 225 * Game1.pixelZoom), (float) (this.mapY + 81 * Game1.pixelZoom));
                this.playerLocationName = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11190");
                return;
              }
              if (tileX > 80 && tileY >= 68)
              {
                this.playerMapPosition = new Vector2((float) (this.mapX + 220 * Game1.pixelZoom), (float) (this.mapY + 108 * Game1.pixelZoom));
                this.playerLocationName = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11190");
                return;
              }
              if (tileY <= 42)
              {
                this.playerMapPosition = new Vector2((float) (this.mapX + 178 * Game1.pixelZoom), (float) (this.mapY + 64 * Game1.pixelZoom));
                this.playerLocationName = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11190");
                return;
              }
              if (tileY > 42 && tileY < 76)
              {
                this.playerMapPosition = new Vector2((float) (this.mapX + 175 * Game1.pixelZoom), (float) (this.mapY + 88 * Game1.pixelZoom));
                this.playerLocationName = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11190");
                return;
              }
              this.playerMapPosition = new Vector2((float) (this.mapX + 182 * Game1.pixelZoom), (float) (this.mapY + 109 * Game1.pixelZoom));
              this.playerLocationName = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11190");
              return;
            }
            if (!(name2 == "Big Barn"))
              return;
          }
          else if (!(name2 == "FarmHouse"))
            return;
        }
        else
        {
          if (stringHash2 <= 1684694008U)
          {
            if ((int) stringHash2 != 1667813495)
            {
              if ((int) stringHash2 != 1684694008 || !(name2 == "Coop"))
                return;
              goto label_152;
            }
            else if (!(name2 == "Tunnel"))
              return;
          }
          else if ((int) stringHash2 != 1972213674)
          {
            if ((int) stringHash2 != 2028543928)
            {
              if ((int) stringHash2 != -2143784615 || !(name2 == "Farm"))
                return;
              goto label_152;
            }
            else if (!(name2 == "Backwoods"))
              return;
          }
          else
          {
            if (!(name2 == "Big Coop"))
              return;
            goto label_152;
          }
          this.playerMapPosition = new Vector2((float) (this.mapX + 109 * Game1.pixelZoom), (float) (this.mapY + 47 * Game1.pixelZoom));
          this.playerLocationName = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11180");
          return;
        }
      }
      else if (stringHash2 <= 3014964069U)
      {
        if (stringHash2 <= 2503779456U)
        {
          if ((int) stringHash2 != -2061409120)
          {
            if ((int) stringHash2 != -1791187840 || !(name2 == "Forest"))
              return;
            if (tileY > 51)
            {
              this.playerLocationName = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11186");
              this.playerMapPosition = new Vector2((float) (this.mapX + 70 * Game1.pixelZoom), (float) (this.mapY + 135 * Game1.pixelZoom));
              return;
            }
            if (tileX < 58)
            {
              this.playerLocationName = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11186");
              this.playerMapPosition = new Vector2((float) (this.mapX + 63 * Game1.pixelZoom), (float) (this.mapY + 104 * Game1.pixelZoom));
              return;
            }
            this.playerLocationName = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11188");
            this.playerMapPosition = new Vector2((float) (this.mapX + 109 * Game1.pixelZoom), (float) (this.mapY + 107 * Game1.pixelZoom));
            return;
          }
          if (!(name2 == "Greenhouse"))
            return;
        }
        else
        {
          if ((int) stringHash2 != -1693112273)
          {
            if ((int) stringHash2 != -1385590711)
            {
              if ((int) stringHash2 != -1280003227 || !(name2 == "Town"))
                return;
              if (tileX > 84 && tileY < 68)
              {
                this.playerMapPosition = new Vector2((float) (this.mapX + 225 * Game1.pixelZoom), (float) (this.mapY + 81 * Game1.pixelZoom));
                this.playerLocationName = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11190");
                return;
              }
              if (tileX > 80 && tileY >= 68)
              {
                this.playerMapPosition = new Vector2((float) (this.mapX + 220 * Game1.pixelZoom), (float) (this.mapY + 108 * Game1.pixelZoom));
                this.playerLocationName = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11190");
                return;
              }
              if (tileY <= 42)
              {
                this.playerMapPosition = new Vector2((float) (this.mapX + 178 * Game1.pixelZoom), (float) (this.mapY + 64 * Game1.pixelZoom));
                this.playerLocationName = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11190");
                return;
              }
              if (tileY > 42 && tileY < 76)
              {
                this.playerMapPosition = new Vector2((float) (this.mapX + 175 * Game1.pixelZoom), (float) (this.mapY + 88 * Game1.pixelZoom));
                this.playerLocationName = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11190");
                return;
              }
              this.playerMapPosition = new Vector2((float) (this.mapX + 182 * Game1.pixelZoom), (float) (this.mapY + 109 * Game1.pixelZoom));
              this.playerLocationName = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11190");
              return;
            }
            if (!(name2 == "Saloon"))
              return;
            this.playerLocationName = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11172");
            return;
          }
          if (!(name2 == "Deluxe Barn"))
            return;
        }
      }
      else if (stringHash2 <= 3308967874U)
      {
        if ((int) stringHash2 != -1111878468)
        {
          if ((int) stringHash2 != -985999422 || !(name2 == "Mountain"))
            return;
          if (tileX < 38)
          {
            this.playerLocationName = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11176");
            this.playerMapPosition = new Vector2((float) (this.mapX + 185 * Game1.pixelZoom), (float) (this.mapY + 36 * Game1.pixelZoom));
            return;
          }
          if (tileX < 96)
          {
            this.playerLocationName = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11177");
            this.playerMapPosition = new Vector2((float) (this.mapX + 220 * Game1.pixelZoom), (float) (this.mapY + 38 * Game1.pixelZoom));
            return;
          }
          this.playerLocationName = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11178");
          this.playerMapPosition = new Vector2((float) (this.mapX + 253 * Game1.pixelZoom), (float) (this.mapY + 40 * Game1.pixelZoom));
          return;
        }
        if (!(name2 == "Barn"))
          return;
      }
      else if ((int) stringHash2 != -961618456)
      {
        if ((int) stringHash2 != -667870852)
        {
          if ((int) stringHash2 != -560689829 || !(name2 == "Deluxe Coop"))
            return;
        }
        else if (!(name2 == "FarmCave"))
          return;
      }
      else
      {
        if (!(name2 == "Beach"))
          return;
        this.playerLocationName = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11174");
        this.playerMapPosition = new Vector2((float) (this.mapX + 202 * Game1.pixelZoom), (float) (this.mapY + 141 * Game1.pixelZoom));
        return;
      }
label_152:
      this.playerMapPosition = new Vector2((float) (this.mapX + 96 * Game1.pixelZoom), (float) (this.mapY + 72 * Game1.pixelZoom));
      this.playerLocationName = Game1.content.LoadString("Strings\\StringsFromCSFiles:MapPage.cs.11064", (object) Game1.player.farmName);
    }

    public override void receiveKeyPress(Keys key)
    {
      base.receiveKeyPress(key);
      if (!Game1.options.doesInputListContain(Game1.options.mapButton, key))
        return;
      this.exitThisMenu(true);
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true)
    {
      if (this.okButton.containsPoint(x, y))
      {
        this.okButton.scale -= 0.25f;
        this.okButton.scale = Math.Max(0.75f, this.okButton.scale);
        (Game1.activeClickableMenu as GameMenu).changeTab(0);
      }
      foreach (ClickableComponent point in this.points)
      {
        if (point.containsPoint(x, y) && point.name == "Lonely Stone")
          Game1.playSound("stoneCrack");
      }
      if (Game1.activeClickableMenu == null)
        return;
      (Game1.activeClickableMenu as GameMenu).changeTab(0);
    }

    public override void receiveRightClick(int x, int y, bool playSound = true)
    {
    }

    public override void performHoverAction(int x, int y)
    {
      this.descriptionText = "";
      this.hoverText = "";
      foreach (ClickableComponent point in this.points)
      {
        if (point.containsPoint(x, y))
        {
          this.hoverText = point.name;
          return;
        }
      }
      if (this.okButton.containsPoint(x, y))
        this.okButton.scale = Math.Min(this.okButton.scale + 0.02f, this.okButton.baseScale + 0.1f);
      else
        this.okButton.scale = Math.Max(this.okButton.scale - 0.02f, this.okButton.baseScale);
    }

    public override void draw(SpriteBatch b)
    {
      Game1.drawDialogueBox(this.mapX - Game1.pixelZoom * 8, this.mapY - Game1.pixelZoom * 24, (this.map.Bounds.Width + 16) * Game1.pixelZoom, 212 * Game1.pixelZoom, false, true, (string) null, false);
      b.Draw(this.map, new Vector2((float) this.mapX, (float) this.mapY), new Rectangle?(new Rectangle(0, 0, 300, 180)), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 0.86f);
      switch (Game1.whichFarm)
      {
        case 1:
          b.Draw(this.map, new Vector2((float) this.mapX, (float) (this.mapY + 43 * Game1.pixelZoom)), new Rectangle?(new Rectangle(0, 180, 131, 61)), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 0.861f);
          break;
        case 2:
          b.Draw(this.map, new Vector2((float) this.mapX, (float) (this.mapY + 43 * Game1.pixelZoom)), new Rectangle?(new Rectangle(131, 180, 131, 61)), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 0.861f);
          break;
        case 3:
          b.Draw(this.map, new Vector2((float) this.mapX, (float) (this.mapY + 43 * Game1.pixelZoom)), new Rectangle?(new Rectangle(0, 241, 131, 61)), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 0.861f);
          break;
        case 4:
          b.Draw(this.map, new Vector2((float) this.mapX, (float) (this.mapY + 43 * Game1.pixelZoom)), new Rectangle?(new Rectangle(131, 241, 131, 61)), Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 0.861f);
          break;
      }
      Game1.player.FarmerRenderer.drawMiniPortrat(b, this.playerMapPosition - new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize / 2)), 0.00011f, 4f, 2, Game1.player);
      if (this.playerLocationName != null)
        SpriteText.drawStringWithScrollCenteredAt(b, this.playerLocationName, this.xPositionOnScreen + this.width / 2, this.yPositionOnScreen + this.height + Game1.tileSize / 2 + Game1.pixelZoom * 4, "", 1f, -1, 0, 0.88f, false);
      this.okButton.draw(b);
      if (this.hoverText.Equals(""))
        return;
      IClickableMenu.drawHoverText(b, this.hoverText, Game1.smallFont, 0, 0, -1, (string) null, -1, (string[]) null, (Item) null, 0, -1, -1, -1, -1, 1f, (CraftingRecipe) null);
    }
  }
}
