// Decompiled with JetBrains decompiler
// Type: StardewValley.Locations.LibraryMuseum
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using xTile;
using xTile.Dimensions;

namespace StardewValley.Locations
{
  public class LibraryMuseum : GameLocation
  {
    private Dictionary<int, Vector2> lostBooksLocations = new Dictionary<int, Vector2>();
    public const int dwarvenGuide = 0;
    public const int totalArtifacts = 95;
    public const int totalNotes = 21;
    public SerializableDictionary<Vector2, int> museumPieces;

    public LibraryMuseum()
    {
    }

    public LibraryMuseum(Map map, string name)
      : base(map, name)
    {
      this.museumPieces = new SerializableDictionary<Vector2, int>();
      for (int xTile = 0; xTile < map.Layers[0].LayerWidth; ++xTile)
      {
        for (int yTile = 0; yTile < map.Layers[0].LayerHeight; ++yTile)
        {
          if (this.doesTileHaveProperty(xTile, yTile, "Action", "Buildings") != null && this.doesTileHaveProperty(xTile, yTile, "Action", "Buildings").Contains("Notes"))
            this.lostBooksLocations.Add(Convert.ToInt32(this.doesTileHaveProperty(xTile, yTile, "Action", "Buildings").Split(' ')[1]), new Vector2((float) xTile, (float) yTile));
        }
      }
    }

    public bool museumAlreadyHasArtifact(int index)
    {
      foreach (KeyValuePair<Vector2, int> museumPiece in (Dictionary<Vector2, int>) this.museumPieces)
      {
        if (museumPiece.Value == index)
          return true;
      }
      return false;
    }

    public bool isItemSuitableForDonation(Item i)
    {
      if (i is StardewValley.Object && (i as StardewValley.Object).type != null && ((i as StardewValley.Object).type.Equals("Arch") || (i as StardewValley.Object).type.Equals("Minerals")))
      {
        int parentSheetIndex = (i as StardewValley.Object).parentSheetIndex;
        bool flag = false;
        foreach (KeyValuePair<Vector2, int> museumPiece in (Dictionary<Vector2, int>) this.museumPieces)
        {
          if (museumPiece.Value == parentSheetIndex)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          return true;
      }
      return false;
    }

    public bool doesFarmerHaveAnythingToDonate(Farmer who)
    {
      for (int index = 0; index < who.maxItems; ++index)
      {
        if (index < who.items.Count && who.items[index] is StardewValley.Object && (who.items[index] as StardewValley.Object).type != null && ((who.items[index] as StardewValley.Object).type.Equals("Arch") || (who.items[index] as StardewValley.Object).type.Equals("Minerals")))
        {
          int parentSheetIndex = (who.items[index] as StardewValley.Object).parentSheetIndex;
          bool flag = false;
          foreach (KeyValuePair<Vector2, int> museumPiece in (Dictionary<Vector2, int>) this.museumPieces)
          {
            if (museumPiece.Value == parentSheetIndex)
            {
              flag = true;
              break;
            }
          }
          if (!flag)
            return true;
        }
      }
      return false;
    }

    private bool museumContainsTheseItems(int[] items, HashSet<int> museumItems)
    {
      for (int index = 0; index < items.Length; ++index)
      {
        if (!museumItems.Contains(items[index]))
          return false;
      }
      return true;
    }

    private int numberOfMuseumItemsOfType(string type)
    {
      int num = 0;
      foreach (KeyValuePair<Vector2, int> museumPiece in (Dictionary<Vector2, int>) this.museumPieces)
      {
        if (Game1.objectInformation[museumPiece.Value].Split('/')[3].Contains(type))
          ++num;
      }
      return num;
    }

    public override void resetForPlayerEntry()
    {
      if (!Game1.player.eventsSeen.Contains(0) && this.doesFarmerHaveAnythingToDonate(Game1.player) && !Game1.player.mailReceived.Contains("somethingToDonate"))
        Game1.player.mailReceived.Add("somethingToDonate");
      base.resetForPlayerEntry();
      if (!Game1.isRaining)
        Game1.changeMusicTrack("libraryTheme");
      int num1 = Game1.player.archaeologyFound.ContainsKey(102) ? Game1.player.archaeologyFound[102][0] : 0;
      for (int index = 0; index < this.lostBooksLocations.Count; ++index)
      {
        KeyValuePair<int, Vector2> keyValuePair = this.lostBooksLocations.ElementAt<KeyValuePair<int, Vector2>>(index);
        if (keyValuePair.Key <= num1)
        {
          List<string> mailReceived = Game1.player.mailReceived;
          string str1 = "lb_";
          keyValuePair = this.lostBooksLocations.ElementAt<KeyValuePair<int, Vector2>>(index);
          // ISSUE: variable of a boxed type
          __Boxed<int> key1 = (ValueType) keyValuePair.Key;
          string str2 = str1 + (object) key1;
          if (!mailReceived.Contains(str2))
          {
            List<TemporaryAnimatedSprite> temporarySprites = this.temporarySprites;
            Texture2D mouseCursors = Game1.mouseCursors;
            Microsoft.Xna.Framework.Rectangle sourceRect = new Microsoft.Xna.Framework.Rectangle(144, 447, 15, 15);
            keyValuePair = this.lostBooksLocations.ElementAt<KeyValuePair<int, Vector2>>(index);
            double num2 = (double) keyValuePair.Value.X * (double) Game1.tileSize;
            keyValuePair = this.lostBooksLocations.ElementAt<KeyValuePair<int, Vector2>>(index);
            double num3 = (double) keyValuePair.Value.Y * (double) Game1.tileSize - (double) (Game1.tileSize * 3 / 2) - 16.0;
            Vector2 position = new Vector2((float) num2, (float) num3);
            int num4 = 0;
            double num5 = 0.0;
            Color white = Color.White;
            TemporaryAnimatedSprite temporaryAnimatedSprite = new TemporaryAnimatedSprite(mouseCursors, sourceRect, position, num4 != 0, (float) num5, white);
            temporaryAnimatedSprite.interval = 99999f;
            temporaryAnimatedSprite.animationLength = 1;
            temporaryAnimatedSprite.totalNumberOfLoops = 9999;
            temporaryAnimatedSprite.yPeriodic = true;
            temporaryAnimatedSprite.yPeriodicLoopTime = 4000f;
            double num6 = (double) (Game1.tileSize / 4);
            temporaryAnimatedSprite.yPeriodicRange = (float) num6;
            double num7 = 1.0;
            temporaryAnimatedSprite.layerDepth = (float) num7;
            double pixelZoom = (double) Game1.pixelZoom;
            temporaryAnimatedSprite.scale = (float) pixelZoom;
            keyValuePair = this.lostBooksLocations.ElementAt<KeyValuePair<int, Vector2>>(index);
            double key2 = (double) keyValuePair.Key;
            temporaryAnimatedSprite.id = (float) key2;
            temporarySprites.Add(temporaryAnimatedSprite);
          }
        }
      }
    }

    public override void cleanupBeforePlayerExit()
    {
      base.cleanupBeforePlayerExit();
      if (Game1.isRaining)
        return;
      Game1.changeMusicTrack("none");
    }

    public List<Item> getRewardsForPlayer(Farmer who)
    {
      List<Item> objList = new List<Item>();
      HashSet<int> museumItems = new HashSet<int>((IEnumerable<int>) this.museumPieces.Values);
      int num1 = this.numberOfMuseumItemsOfType("Arch");
      int num2 = this.numberOfMuseumItemsOfType("Minerals");
      int num3 = num1 + num2;
      if (!who.canUnderstandDwarves && museumItems.Contains(96) && (museumItems.Contains(97) && museumItems.Contains(98)) && museumItems.Contains(99))
        objList.Add((Item) new StardewValley.Object(326, 1, false, -1, 0));
      if (!who.specialBigCraftables.Contains(1305) && museumItems.Contains(113) && num1 > 4)
        objList.Add((Item) new Furniture(1305, Vector2.Zero));
      if (!who.specialBigCraftables.Contains(1304) && num1 >= 15)
        objList.Add((Item) new Furniture(1304, Vector2.Zero));
      if (!who.specialBigCraftables.Contains(139) && num1 >= 20)
        objList.Add((Item) new StardewValley.Object(Vector2.Zero, 139, false));
      if (!who.specialBigCraftables.Contains(1545))
      {
        if (this.museumContainsTheseItems(new int[2]
        {
          108,
          122
        }, museumItems) && num1 > 10)
          objList.Add((Item) new Furniture(1545, Vector2.Zero));
      }
      if (!who.specialItems.Contains(464) && museumItems.Contains(119) && num1 > 2)
        objList.Add((Item) new StardewValley.Object(464, 1, false, -1, 0));
      if (!who.specialItems.Contains(463) && museumItems.Contains(123) && num1 > 2)
        objList.Add((Item) new StardewValley.Object(463, 1, false, -1, 0));
      if (!who.specialItems.Contains(499) && museumItems.Contains(114))
      {
        objList.Add((Item) new StardewValley.Object(499, 1, false, -1, 0));
        objList.Add((Item) new StardewValley.Object(499, 1, true, -1, 0));
      }
      if (!who.specialBigCraftables.Contains(1301))
      {
        if (this.museumContainsTheseItems(new int[3]
        {
          579,
          581,
          582
        }, museumItems))
          objList.Add((Item) new Furniture(1301, Vector2.Zero));
      }
      if (!who.specialBigCraftables.Contains(1302))
      {
        if (this.museumContainsTheseItems(new int[2]
        {
          583,
          584
        }, museumItems))
          objList.Add((Item) new Furniture(1302, Vector2.Zero));
      }
      if (!who.specialBigCraftables.Contains(1303))
      {
        if (this.museumContainsTheseItems(new int[2]
        {
          580,
          585
        }, museumItems))
          objList.Add((Item) new Furniture(1303, Vector2.Zero));
      }
      if (!who.specialBigCraftables.Contains(1298) && num2 > 10)
        objList.Add((Item) new Furniture(1298, Vector2.Zero));
      if (!who.specialBigCraftables.Contains(1299) && num2 > 30)
        objList.Add((Item) new Furniture(1299, Vector2.Zero));
      if (!who.specialBigCraftables.Contains(94) && num2 > 20)
        objList.Add((Item) new StardewValley.Object(Vector2.Zero, 94, false));
      if (!who.specialBigCraftables.Contains(21) && num2 >= 50)
        objList.Add((Item) new StardewValley.Object(Vector2.Zero, 21, false));
      if (!who.specialBigCraftables.Contains(131) && num2 > 40)
        objList.Add((Item) new Furniture(131, Vector2.Zero));
      foreach (Item obj in objList)
        obj.specialItem = true;
      if (!who.mailReceived.Contains("museum5") && num3 >= 5)
        objList.Add((Item) new StardewValley.Object(474, 9, false, -1, 0));
      if (!who.mailReceived.Contains("museum10") && num3 >= 10)
        objList.Add((Item) new StardewValley.Object(479, 9, false, -1, 0));
      if (!who.mailReceived.Contains("museum15") && num3 >= 15)
        objList.Add((Item) new StardewValley.Object(486, 1, false, -1, 0));
      if (!who.mailReceived.Contains("museum20") && num3 >= 20)
        objList.Add((Item) new Furniture(1541, Vector2.Zero));
      if (!who.mailReceived.Contains("museum25") && num3 >= 25)
        objList.Add((Item) new Furniture(1554, Vector2.Zero));
      if (!who.mailReceived.Contains("museum30") && num3 >= 30)
        objList.Add((Item) new Furniture(1669, Vector2.Zero));
      if (!who.mailReceived.Contains("museum40") && num3 >= 40)
        objList.Add((Item) new StardewValley.Object(Vector2.Zero, 140, false));
      if (!who.mailReceived.Contains("museum50") && num3 >= 50)
        objList.Add((Item) new Furniture(1671, Vector2.Zero));
      if (!who.mailReceived.Contains("museumComplete") && num3 >= 95)
        objList.Add((Item) new StardewValley.Object(434, 1, false, -1, 0));
      if (num3 >= 60)
      {
        if (!Game1.player.eventsSeen.Contains(295672))
          Game1.player.eventsSeen.Add(295672);
        else if (!Game1.player.hasRustyKey)
          Game1.player.eventsSeen.Remove(66);
      }
      return objList;
    }

    public void collectedReward(Item item, Farmer who)
    {
      if (item == null || !(item is StardewValley.Object))
        return;
      (item as StardewValley.Object).specialItem = true;
      switch ((item as StardewValley.Object).ParentSheetIndex)
      {
        case 1554:
          who.mailReceived.Add("museum25");
          break;
        case 1669:
          who.mailReceived.Add("museum30");
          break;
        case 1671:
          who.mailReceived.Add("museum50");
          break;
        case 486:
          who.mailReceived.Add("museum15");
          break;
        case 1541:
          who.mailReceived.Add("museum20");
          break;
        case 474:
          who.mailReceived.Add("museum5");
          break;
        case 479:
          who.mailReceived.Add("museum10");
          break;
        case 140:
          who.mailReceived.Add("museum40");
          break;
        case 434:
          who.mailReceived.Add("museumComplete");
          break;
      }
    }

    public override bool checkAction(Location tileLocation, xTile.Dimensions.Rectangle viewport, Farmer who)
    {
      foreach (KeyValuePair<Vector2, int> museumPiece in (Dictionary<Vector2, int>) this.museumPieces)
      {
        if ((double) museumPiece.Key.X == (double) tileLocation.X && ((double) museumPiece.Key.Y == (double) tileLocation.Y || (double) museumPiece.Key.Y == (double) (tileLocation.Y - 1)))
        {
          Game1.drawObjectDialogue(Game1.parseText(" - " + Game1.objectInformation[museumPiece.Value].Split('/')[4] + " - " + Environment.NewLine + Game1.objectInformation[museumPiece.Value].Split('/')[5]));
          return true;
        }
      }
      return base.checkAction(tileLocation, viewport, who);
    }

    public bool isTileSuitableForMuseumPiece(int x, int y)
    {
      if (!this.museumPieces.ContainsKey(new Vector2((float) x, (float) y)))
      {
        switch (this.getTileIndexAt(new Point(x, y), "Buildings"))
        {
          case 1073:
          case 1074:
          case 1072:
          case 1237:
          case 1238:
            return true;
        }
      }
      return false;
    }

    public override void drawAboveAlwaysFrontLayer(SpriteBatch b)
    {
      foreach (TemporaryAnimatedSprite temporarySprite in this.temporarySprites)
      {
        if ((double) temporarySprite.layerDepth >= 1.0)
          temporarySprite.draw(b, false, 0, 0);
      }
    }

    public override void draw(SpriteBatch b)
    {
      base.draw(b);
      foreach (KeyValuePair<Vector2, int> museumPiece in (Dictionary<Vector2, int>) this.museumPieces)
      {
        SpriteBatch spriteBatch = b;
        Texture2D shadowTexture = Game1.shadowTexture;
        Vector2 local = Game1.GlobalToLocal(Game1.viewport, museumPiece.Key * (float) Game1.tileSize + new Vector2((float) (Game1.tileSize / 2), (float) (Game1.tileSize - 12)));
        Microsoft.Xna.Framework.Rectangle? sourceRectangle = new Microsoft.Xna.Framework.Rectangle?(Game1.shadowTexture.Bounds);
        Color white = Color.White;
        double num1 = 0.0;
        Microsoft.Xna.Framework.Rectangle bounds = Game1.shadowTexture.Bounds;
        double x = (double) bounds.Center.X;
        bounds = Game1.shadowTexture.Bounds;
        double y = (double) bounds.Center.Y;
        Vector2 origin = new Vector2((float) x, (float) y);
        double num2 = 4.0;
        int num3 = 0;
        double num4 = ((double) museumPiece.Key.Y * (double) Game1.tileSize - 2.0) / 10000.0;
        spriteBatch.Draw(shadowTexture, local, sourceRectangle, white, (float) num1, origin, (float) num2, (SpriteEffects) num3, (float) num4);
        b.Draw(Game1.objectSpriteSheet, Game1.GlobalToLocal(Game1.viewport, museumPiece.Key * (float) Game1.tileSize), new Microsoft.Xna.Framework.Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.objectSpriteSheet, museumPiece.Value, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float) Game1.pixelZoom, SpriteEffects.None, (float) ((double) museumPiece.Key.Y * (double) Game1.tileSize / 10000.0));
      }
    }
  }
}
