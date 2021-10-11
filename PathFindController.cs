// Decompiled with JetBrains decompiler
// Type: StardewValley.PathFindController
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using StardewValley.Locations;
using System;
using System.Collections.Generic;

namespace StardewValley
{
  public class PathFindController
  {
    public const byte impassable = 255;
    public const int timeToWaitBeforeCancelling = 5000;
    private Character character;
    public GameLocation location;
    public Stack<Point> pathToEndPoint;
    public Point endPoint;
    public int finalFacingDirection;
    public int pausedTimer;
    public int limit;
    private PathFindController.isAtEnd endFunction;
    public PathFindController.endBehavior endBehaviorFunction;
    public bool NPCSchedule;
    public int timerSinceLastCheckPoint;

    public PathFindController(Character c, GameLocation location, Point endPoint, int finalFacingDirection)
      : this(c, location, new PathFindController.isAtEnd(PathFindController.isAtEndPoint), finalFacingDirection, false, (PathFindController.endBehavior) null, 10000, endPoint)
    {
    }

    public PathFindController(Character c, GameLocation location, Point endPoint, int finalFacingDirection, PathFindController.endBehavior endBehaviorFunction)
      : this(c, location, new PathFindController.isAtEnd(PathFindController.isAtEndPoint), finalFacingDirection, false, (PathFindController.endBehavior) null, 10000, endPoint)
    {
      this.endPoint = endPoint;
      this.endBehaviorFunction = endBehaviorFunction;
    }

    public PathFindController(Character c, GameLocation location, Point endPoint, int finalFacingDirection, PathFindController.endBehavior endBehaviorFunction, int limit)
      : this(c, location, new PathFindController.isAtEnd(PathFindController.isAtEndPoint), finalFacingDirection, false, (PathFindController.endBehavior) null, limit, endPoint)
    {
      this.endPoint = endPoint;
      this.endBehaviorFunction = endBehaviorFunction;
    }

    public PathFindController(Character c, GameLocation location, Point endPoint, int finalFacingDirection, bool eraseOldPathController)
      : this(c, location, new PathFindController.isAtEnd(PathFindController.isAtEndPoint), finalFacingDirection, eraseOldPathController, (PathFindController.endBehavior) null, 10000, endPoint)
    {
    }

    public static bool isAtEndPoint(PathNode currentNode, Point endPoint, GameLocation location, Character c)
    {
      if (currentNode.x == endPoint.X)
        return currentNode.y == endPoint.Y;
      return false;
    }

    public PathFindController(Stack<Point> pathToEndPoint, GameLocation location, Character c, Point endPoint)
    {
      this.pathToEndPoint = pathToEndPoint;
      this.location = location;
      this.character = c;
      this.endPoint = endPoint;
    }

    public PathFindController(Stack<Point> pathToEndPoint, Character c, GameLocation l)
    {
      this.pathToEndPoint = pathToEndPoint;
      this.character = c;
      this.location = l;
      this.NPCSchedule = true;
    }

    public PathFindController(Character c, GameLocation location, PathFindController.isAtEnd endFunction, int finalFacingDirection, bool eraseOldPathController, PathFindController.endBehavior endBehaviorFunction, int limit, Point endPoint)
    {
      this.limit = limit;
      this.character = c;
      if (c is NPC && (c as NPC).CurrentDialogue.Count > 0 && (c as NPC).CurrentDialogue.Peek().removeOnNextMove)
        (c as NPC).CurrentDialogue.Pop();
      this.location = location;
      this.endFunction = endFunction == null ? new PathFindController.isAtEnd(PathFindController.isAtEndPoint) : endFunction;
      this.endBehaviorFunction = endBehaviorFunction;
      if (endPoint == Point.Zero)
        endPoint = new Point((int) c.getTileLocation().X, (int) c.getTileLocation().Y);
      this.finalFacingDirection = finalFacingDirection;
      if (!(this.character is NPC) && !Game1.currentLocation.Name.Equals(location.Name) && (endFunction == new PathFindController.isAtEnd(PathFindController.isAtEndPoint) && endPoint.X > 0) && endPoint.Y > 0)
      {
        this.character.position = new Vector2((float) (endPoint.X * Game1.tileSize), (float) (endPoint.Y * Game1.tileSize - Game1.tileSize / 2));
      }
      else
      {
        this.pathToEndPoint = PathFindController.findPath(new Point((int) c.getTileLocation().X, (int) c.getTileLocation().Y), endPoint, endFunction, location, this.character, limit);
        if (this.pathToEndPoint != null)
          return;
        FarmHouse farmHouse = location as FarmHouse;
      }
    }

    public bool update(GameTime time)
    {
      if (this.pathToEndPoint == null || this.pathToEndPoint.Count == 0)
        return true;
      if (!this.NPCSchedule && !Game1.currentLocation.Name.Equals(this.location.Name) && (this.endPoint.X > 0 && this.endPoint.Y > 0))
      {
        this.character.position = new Vector2((float) (this.endPoint.X * Game1.tileSize), (float) (this.endPoint.Y * Game1.tileSize - Game1.tileSize / 2));
        return true;
      }
      if (Game1.activeClickableMenu == null)
      {
        int sinceLastCheckPoint = this.timerSinceLastCheckPoint;
        TimeSpan elapsedGameTime = time.ElapsedGameTime;
        int milliseconds1 = elapsedGameTime.Milliseconds;
        this.timerSinceLastCheckPoint = sinceLastCheckPoint + milliseconds1;
        Vector2 position = this.character.position;
        this.moveCharacter(time);
        if (this.character.position.Equals(position))
        {
          int pausedTimer = this.pausedTimer;
          elapsedGameTime = time.ElapsedGameTime;
          int milliseconds2 = elapsedGameTime.Milliseconds;
          this.pausedTimer = pausedTimer + milliseconds2;
        }
        else
          this.pausedTimer = 0;
        if (!this.NPCSchedule && this.pausedTimer > 5000)
          return true;
      }
      return false;
    }

    public static Stack<Point> findPath(Point startPoint, Point endPoint, PathFindController.isAtEnd endPointFunction, GameLocation location, Character character, int limit)
    {
      sbyte[,] numArray = new sbyte[4, 2]
      {
        {
          (sbyte) -1,
          (sbyte) 0
        },
        {
          (sbyte) 1,
          (sbyte) 0
        },
        {
          (sbyte) 0,
          (sbyte) 1
        },
        {
          (sbyte) 0,
          (sbyte) -1
        }
      };
      PriorityQueue priorityQueue = new PriorityQueue();
      Dictionary<PathNode, PathNode> closedList = new Dictionary<PathNode, PathNode>();
      int num = 0;
      priorityQueue.Enqueue(new PathNode(startPoint.X, startPoint.Y, (byte) 0, (PathNode) null), Math.Abs(endPoint.X - startPoint.X) + Math.Abs(endPoint.Y - startPoint.Y));
      while (!priorityQueue.IsEmpty())
      {
        PathNode pathNode1 = priorityQueue.Dequeue();
        if (endPointFunction(pathNode1, endPoint, location, character))
          return PathFindController.reconstructPath(pathNode1, closedList);
        if (!closedList.ContainsKey(pathNode1))
          closedList.Add(pathNode1, pathNode1.parent);
        for (int index = 0; index < 4; ++index)
        {
          PathNode pathNode2 = new PathNode(pathNode1.x + (int) numArray[index, 0], pathNode1.y + (int) numArray[index, 1], pathNode1);
          pathNode2.g = (byte) ((uint) pathNode1.g + 1U);
          if (!closedList.ContainsKey(pathNode2) && (pathNode2.x == endPoint.X && pathNode2.y == endPoint.Y || pathNode2.x >= 0 && pathNode2.y >= 0 && (pathNode2.x < location.map.Layers[0].LayerWidth && pathNode2.y < location.map.Layers[0].LayerHeight)) && !location.isCollidingPosition(new Rectangle(pathNode2.x * Game1.tileSize + 1, pathNode2.y * Game1.tileSize + 1, Game1.tileSize - 2, Game1.tileSize - 2), Game1.viewport, false, 0, false, character, true, false, false))
          {
            int priority = (int) pathNode2.g + (Math.Abs(endPoint.X - pathNode2.x) + Math.Abs(endPoint.Y - pathNode2.y));
            if (!priorityQueue.Contains(pathNode2, priority))
              priorityQueue.Enqueue(pathNode2, priority);
          }
        }
        ++num;
        if (num >= limit)
          return (Stack<Point>) null;
      }
      return (Stack<Point>) null;
    }

    public static Stack<Point> reconstructPath(PathNode finalNode, Dictionary<PathNode, PathNode> closedList)
    {
      Stack<Point> pointStack = new Stack<Point>();
      pointStack.Push(new Point(finalNode.x, finalNode.y));
      for (PathNode index = finalNode.parent; index != null; index = closedList[index])
        pointStack.Push(new Point(index.x, index.y));
      return pointStack;
    }

    private byte[,] createMapGrid(GameLocation location, Point endPoint)
    {
      byte[,] numArray = new byte[location.map.Layers[0].LayerWidth, location.map.Layers[0].LayerHeight];
      for (int index1 = 0; index1 < location.map.Layers[0].LayerWidth; ++index1)
      {
        for (int index2 = 0; index2 < location.map.Layers[0].LayerHeight; ++index2)
          numArray[index1, index2] = location.isCollidingPosition(new Rectangle(index1 * Game1.tileSize + 1, index2 * Game1.tileSize + 1, Game1.tileSize - 2, Game1.tileSize - 2), Game1.viewport, false, 0, false, this.character, true, false, false) ? byte.MaxValue : (byte) (Math.Abs(endPoint.X - index1) + Math.Abs(endPoint.Y - index2));
      }
      return numArray;
    }

    private void moveCharacter(GameTime time)
    {
      Rectangle rectangle = new Rectangle(this.pathToEndPoint.Peek().X * Game1.tileSize, this.pathToEndPoint.Peek().Y * Game1.tileSize, Game1.tileSize, Game1.tileSize);
      rectangle.Inflate(-2, 0);
      Rectangle boundingBox = this.character.GetBoundingBox();
      if ((rectangle.Contains(boundingBox) || boundingBox.Width > rectangle.Width && rectangle.Contains(boundingBox.Center)) && rectangle.Bottom - boundingBox.Bottom >= 2)
      {
        this.timerSinceLastCheckPoint = 0;
        this.pathToEndPoint.Pop();
        this.character.stopWithoutChangingFrame();
        if (this.pathToEndPoint.Count != 0)
          return;
        this.character.Halt();
        if (this.finalFacingDirection != -1)
          this.character.faceDirection(this.finalFacingDirection);
        if (this.NPCSchedule)
        {
          (this.character as NPC).DirectionsToNewLocation = (SchedulePathDescription) null;
          (this.character as NPC).endOfRouteMessage = (this.character as NPC).nextEndOfRouteMessage;
        }
        if (this.endBehaviorFunction == null)
          return;
        this.endBehaviorFunction(this.character, this.location);
      }
      else
      {
        if (this.character is Farmer)
        {
          (this.character as Farmer).movementDirections.Clear();
        }
        else
        {
          foreach (NPC character in this.location.characters)
          {
            if (!character.Equals((object) this.character) && (character.GetBoundingBox().Intersects(boundingBox) && character.isMoving() && string.Compare(character.name, this.character.name) < 0))
            {
              this.character.Halt();
              return;
            }
          }
        }
        if (boundingBox.Left < rectangle.Left && boundingBox.Right < rectangle.Right)
          this.character.SetMovingRight(true);
        else if (boundingBox.Right > rectangle.Right && boundingBox.Left > rectangle.Left)
          this.character.SetMovingLeft(true);
        else if (boundingBox.Top <= rectangle.Top)
          this.character.SetMovingDown(true);
        else if (boundingBox.Bottom >= rectangle.Bottom - 2)
          this.character.SetMovingUp(true);
        this.character.MovePosition(time, Game1.viewport, this.location);
        if (!this.NPCSchedule)
          return;
        Warp warp = this.location.isCollidingWithWarpOrDoor(this.character.nextPosition(this.character.getDirection()));
        if (warp == null)
          return;
        if (this.character is NPC && (this.character as NPC).isMarried() && (this.character as NPC).followSchedule)
        {
          NPC character = this.character as NPC;
          if (this.location is FarmHouse)
            warp = new Warp(warp.X, warp.Y, "BusStop", 0, 23, false);
          if (this.location is BusStop && warp.X <= 0)
            warp = new Warp(warp.X, warp.Y, character.getHome().name, (character.getHome() as FarmHouse).getEntryLocation().X, (character.getHome() as FarmHouse).getEntryLocation().Y, false);
          if (character.temporaryController != null && character.controller != null)
            character.controller.location = Game1.getLocationFromName(warp.TargetName);
        }
        Game1.warpCharacter(this.character as NPC, warp.TargetName, new Vector2((float) warp.TargetX, (float) warp.TargetY), false, this.location.isOutdoors);
        this.location.characters.Remove(this.character as NPC);
        if (this.location.Equals((object) Game1.currentLocation) && Utility.isOnScreen(new Vector2((float) (warp.X * Game1.tileSize), (float) (warp.Y * Game1.tileSize)), Game1.tileSize * 6) && this.location.doors.ContainsKey(new Point(warp.X, warp.Y)))
          Game1.playSound("doorClose");
        this.location = Game1.getLocationFromName(warp.TargetName);
        if (this.location.Equals((object) Game1.currentLocation) && Utility.isOnScreen(new Vector2((float) (warp.TargetX * Game1.tileSize), (float) (warp.TargetY * Game1.tileSize)), Game1.tileSize * 6) && this.location.doors.ContainsKey(new Point(warp.TargetX, warp.TargetY - 1)))
          Game1.playSound("doorClose");
        if (this.pathToEndPoint.Count > 0)
          this.pathToEndPoint.Pop();
        while (this.pathToEndPoint.Count > 0 && (Math.Abs(this.pathToEndPoint.Peek().X - this.character.getTileX()) > 1 || Math.Abs(this.pathToEndPoint.Peek().Y - this.character.getTileY()) > 1))
          this.pathToEndPoint.Pop();
      }
    }

    public static Stack<Point> findPathForNPCSchedules(Point startPoint, Point endPoint, GameLocation location, int limit)
    {
      sbyte[,] numArray = new sbyte[4, 2]
      {
        {
          (sbyte) -1,
          (sbyte) 0
        },
        {
          (sbyte) 1,
          (sbyte) 0
        },
        {
          (sbyte) 0,
          (sbyte) 1
        },
        {
          (sbyte) 0,
          (sbyte) -1
        }
      };
      PriorityQueue priorityQueue = new PriorityQueue();
      Dictionary<PathNode, PathNode> closedList = new Dictionary<PathNode, PathNode>();
      int num = 0;
      priorityQueue.Enqueue(new PathNode(startPoint.X, startPoint.Y, (byte) 0, (PathNode) null), Math.Abs(endPoint.X - startPoint.X) + Math.Abs(endPoint.Y - startPoint.Y));
      PathNode pathNode1 = (PathNode) priorityQueue.Peek();
      while (!priorityQueue.IsEmpty())
      {
        PathNode pathNode2 = priorityQueue.Dequeue();
        if (pathNode2.x == endPoint.X && pathNode2.y == endPoint.Y)
          return PathFindController.reconstructPath(pathNode2, closedList);
        if (pathNode2.x == 79)
        {
          int y = pathNode2.y;
        }
        if (!closedList.ContainsKey(pathNode2))
          closedList.Add(pathNode2, pathNode2.parent);
        for (int index = 0; index < 4; ++index)
        {
          PathNode pathNode3 = new PathNode(pathNode2.x + (int) numArray[index, 0], pathNode2.y + (int) numArray[index, 1], pathNode2);
          pathNode3.g = (byte) ((uint) pathNode2.g + 1U);
          if (!closedList.ContainsKey(pathNode3) && (pathNode3.x == endPoint.X && pathNode3.y == endPoint.Y || pathNode3.x >= 0 && pathNode3.y >= 0 && (pathNode3.x < location.map.Layers[0].LayerWidth && pathNode3.y < location.map.Layers[0].LayerHeight) && !PathFindController.isPositionImpassableForNPCSchedule(location, pathNode3.x, pathNode3.y)))
          {
            int priority = (int) pathNode3.g + PathFindController.getPreferenceValueForTerrainType(location, pathNode3.x, pathNode3.y) + (Math.Abs(endPoint.X - pathNode3.x) + Math.Abs(endPoint.Y - pathNode3.y) + (pathNode3.x == pathNode2.x && pathNode3.x == pathNode1.x || pathNode3.y == pathNode2.y && pathNode3.y == pathNode1.y ? -2 : 0));
            if (!priorityQueue.Contains(pathNode3, priority))
              priorityQueue.Enqueue(pathNode3, priority);
          }
        }
        pathNode1 = pathNode2;
        ++num;
        if (num >= limit)
          return (Stack<Point>) null;
      }
      return (Stack<Point>) null;
    }

    private static bool isPositionImpassableForNPCSchedule(GameLocation l, int x, int y)
    {
      return l.getTileIndexAt(x, y, "Buildings") != -1 && (l.doesTileHaveProperty(x, y, "Action", "Buildings") == null || !l.doesTileHaveProperty(x, y, "Action", "Buildings").Contains("Door") && !l.doesTileHaveProperty(x, y, "Action", "Buildings").Contains("Passable")) || l.isTerrainFeatureAt(x, y);
    }

    private static int getPreferenceValueForTerrainType(GameLocation l, int x, int y)
    {
      string str = l.doesTileHaveProperty(x, y, "Type", "Back");
      if (str != null)
      {
        string lower = str.ToLower();
        if (lower == "stone")
          return -7;
        if (lower == "wood")
          return -4;
        if (lower == "dirt")
          return -2;
        if (lower == "grass")
          return -1;
      }
      return 0;
    }

    public delegate bool isAtEnd(PathNode currentNode, Point endPoint, GameLocation location, Character c);

    public delegate void endBehavior(Character c, GameLocation location);
  }
}
