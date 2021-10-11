// Decompiled with JetBrains decompiler
// Type: StardewValley.Polygon
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley
{
  public class Polygon
  {
    public List<Polygon.Line> lines = new List<Polygon.Line>();

    public List<Polygon.Line> Lines
    {
      get
      {
        return this.lines;
      }
      set
      {
        this.lines = value;
      }
    }

    public void addPoint(Vector2 point)
    {
      if (this.lines.Count <= 0)
        return;
      this.lines.Add(new Polygon.Line(this.Lines[this.Lines.Count - 1].End, point));
    }

    public bool containsPoint(Vector2 point)
    {
      foreach (Polygon.Line line in this.Lines)
      {
        if (line.End.Equals(point))
          return true;
      }
      return false;
    }

    public static Polygon getGentlerBorderForLakes(Rectangle room, Random mineRandom)
    {
      return Polygon.getGentlerBorderForLakes(room, mineRandom, Rectangle.Empty);
    }

    public static Polygon getEdgeBorder(Rectangle room, Random mineRandom)
    {
      return Polygon.getEdgeBorder(room, mineRandom, new List<Rectangle>(), (room.Width - 2) / 2, (room.Height - 2) / 2);
    }

    public static Polygon getEdgeBorder(Rectangle room, Random mineRandom, List<Rectangle> smoothZone)
    {
      return Polygon.getEdgeBorder(room, mineRandom, smoothZone, (room.Width - 2) / 2, (room.Height - 2) / 2);
    }

    public static Polygon getEdgeBorder(Rectangle room, Random mineRandom, List<Rectangle> smoothZone, int horizontalInwardLimit, int verticalInwardLimit)
    {
      if (smoothZone == null)
        smoothZone = new List<Rectangle>();
      int width = room.Width - 2;
      int height = room.Height - 2;
      int x = room.X + 1;
      int y = room.Y + 1;
      Rectangle rectangle = new Rectangle(x, y, width, height);
      Polygon polygon = new Polygon();
      Vector2 vector2 = new Vector2((float) mineRandom.Next(x + 5, x + 8), (float) mineRandom.Next(y + 5, y + 8));
      polygon.Lines.Add(new Polygon.Line(vector2, new Vector2(vector2.X + 1f, vector2.Y)));
      ++vector2.X;
      int num1 = width - 12;
      List<int> source1 = new List<int>() { 2, 2, 2 };
      int num2 = 0;
      while (num2 < num1)
      {
        int num3 = mineRandom.Next(3);
        if (source1.Last<int>() != num3 && source1[source1.Count - 2] != source1.Last<int>())
          num3 = source1.Last<int>();
        if (num3 == 0 && (double) vector2.Y > (double) y && !source1.Contains(1))
        {
          --vector2.Y;
          source1.Add(0);
        }
        else if (num3 == 1 && (double) vector2.Y < (double) (y + verticalInwardLimit) && !source1.Contains(0))
        {
          ++vector2.Y;
          source1.Add(1);
        }
        else
        {
          ++vector2.X;
          ++num2;
          source1.Add(2);
        }
        source1.RemoveAt(0);
        polygon.addPoint(vector2);
      }
      int num4 = height - 4 - (int) ((double) vector2.Y - (double) room.Y);
      ++vector2.Y;
      List<int> source2 = new List<int>() { 2, 2, 2 };
      polygon.addPoint(vector2);
      int num5 = 0;
      while (num5 < num4)
      {
        int num3 = mineRandom.Next(3);
        if (source2.Last<int>() != num3 && source2[source2.Count - 2] != source2.Last<int>())
          num3 = source2.Last<int>();
        if (num5 > 4 && num3 == 0 && ((double) vector2.X < (double) (x + width - 1) && !source2.Contains(1)) && !Utility.pointInRectangles(smoothZone, (int) vector2.X, (int) vector2.Y))
        {
          ++vector2.X;
          source2.Add(0);
        }
        else if (num5 > 4 && num3 == 1 && ((double) vector2.X > (double) (x + width - horizontalInwardLimit + 1) && !source2.Contains(0)) && !Utility.pointInRectangles(smoothZone, (int) vector2.X, (int) vector2.Y))
        {
          --vector2.X;
          source2.Add(1);
        }
        else
        {
          ++vector2.Y;
          ++num5;
          source2.Add(2);
        }
        source2.RemoveAt(0);
        polygon.addPoint(vector2);
      }
      int num6 = (int) vector2.X - (int) polygon.Lines[0].Start.X + 1;
      --vector2.X;
      List<int> source3 = new List<int>() { 2, 2, 2 };
      polygon.addPoint(vector2);
      int num7 = 0;
      while (num7 < num6)
      {
        int num3 = mineRandom.Next(3);
        if (source3.Last<int>() != num3 && source3[source3.Count - 2] != source3.Last<int>())
          num3 = source3.Last<int>();
        if (num7 > 4 && num3 == 0 && ((double) vector2.Y > (double) (y + height - verticalInwardLimit) && !source3.Contains(1)) && !polygon.containsPoint(new Vector2(vector2.X, vector2.Y - 1f)))
        {
          --vector2.Y;
          source3.Add(0);
        }
        else if (num7 > 4 && num3 == 1 && ((double) vector2.Y < (double) (y + height) && !source3.Contains(0)))
        {
          ++vector2.Y;
          source3.Add(1);
        }
        else
        {
          --vector2.X;
          ++num7;
          source3.Add(2);
        }
        source3.RemoveAt(0);
        polygon.addPoint(vector2);
      }
      int num8 = (int) vector2.Y - (int) polygon.Lines[0].Start.Y - 1;
      --vector2.Y;
      List<int> source4 = new List<int>() { 2, 2, 2 };
      polygon.addPoint(vector2);
      int num9 = 0;
      while (num9 < num8)
      {
        int num3 = mineRandom.Next(3);
        if (source4.Last<int>() != num3 && source4[source4.Count - 2] != source4.Last<int>())
          num3 = source4.Last<int>();
        if (num9 > 4 && num3 == 0 && ((double) vector2.X < (double) (int) polygon.Lines[0].Start.X && !source4.Contains(1)) && (!polygon.containsPoint(new Vector2(vector2.X + 1f, vector2.Y)) && !polygon.containsPoint(new Vector2(vector2.X, vector2.Y - 1f)) && !Utility.pointInRectangles(smoothZone, (int) vector2.X, (int) vector2.Y)))
        {
          ++vector2.X;
          source4.Add(0);
        }
        else if (num9 > 4 && num3 == 1 && ((double) vector2.X > (double) (x + 1) && !source4.Contains(0)) && !Utility.pointInRectangles(smoothZone, (int) vector2.X, (int) vector2.Y))
        {
          --vector2.X;
          source4.Add(1);
        }
        else
        {
          --vector2.Y;
          ++num9;
          source4.Add(2);
        }
        source4.RemoveAt(0);
        polygon.addPoint(vector2);
      }
      if ((double) vector2.X < (double) (int) polygon.Lines[0].Start.X)
      {
        int num3 = (int) polygon.Lines[0].Start.X + 1 - (int) vector2.X - 1;
        for (int index = 0; index < num3; ++index)
        {
          ++vector2.X;
          polygon.addPoint(vector2);
        }
      }
      return polygon;
    }

    public static Polygon getGentlerBorderForLakes(Rectangle room, Random mineRandom, Rectangle smoothZone)
    {
      int width = room.Width - 2;
      int height = room.Height - 2;
      int x = room.X + 1;
      int y = room.Y + 1;
      Rectangle rectangle = new Rectangle(x, y, width, height);
      Polygon polygon = new Polygon();
      Vector2 vector2 = new Vector2((float) mineRandom.Next(x + 5, x + 8), (float) mineRandom.Next(y + 5, y + 8));
      polygon.Lines.Add(new Polygon.Line(vector2, new Vector2(vector2.X + 1f, vector2.Y)));
      ++vector2.X;
      int num1 = width - 12;
      List<int> intList1 = new List<int>() { 2, 2, 2 };
      int num2 = 0;
      while (num2 < num1)
      {
        int num3 = mineRandom.Next(3);
        if (num3 == 0 && (double) vector2.Y > (double) y && (!intList1.Contains(1) && !smoothZone.Contains((int) vector2.X, (int) vector2.Y)))
        {
          --vector2.Y;
          intList1.Add(0);
        }
        else if (num3 == 1 && (double) vector2.Y < (double) (y + height / 2) && (!intList1.Contains(0) && !smoothZone.Contains((int) vector2.X, (int) vector2.Y)))
        {
          ++vector2.Y;
          intList1.Add(1);
        }
        else
        {
          ++vector2.X;
          ++num2;
          intList1.Add(2);
        }
        intList1.RemoveAt(0);
        polygon.addPoint(vector2);
      }
      int num4 = height - 4 - (int) ((double) vector2.Y - (double) room.Y);
      ++vector2.Y;
      List<int> intList2 = new List<int>() { 2, 2, 2 };
      polygon.addPoint(vector2);
      int num5 = 0;
      while (num5 < num4)
      {
        int num3 = mineRandom.Next(3);
        if (num3 == 0 && (double) vector2.X < (double) (x + width) && (!intList2.Contains(1) && !smoothZone.Contains((int) vector2.X, (int) vector2.Y)))
        {
          ++vector2.X;
          intList2.Add(0);
        }
        else if (num3 == 1 && (double) vector2.X > (double) (x + width / 2 + 1) && (!intList2.Contains(0) && !smoothZone.Contains((int) vector2.X, (int) vector2.Y)))
        {
          --vector2.X;
          intList2.Add(1);
        }
        else
        {
          ++vector2.Y;
          ++num5;
          intList2.Add(2);
        }
        intList2.RemoveAt(0);
        polygon.addPoint(vector2);
      }
      int num6 = (int) vector2.X - (int) polygon.Lines[0].Start.X + 1;
      --vector2.X;
      List<int> intList3 = new List<int>() { 2, 2, 2 };
      polygon.addPoint(vector2);
      int num7 = 0;
      while (num7 < num6)
      {
        int num3 = mineRandom.Next(3);
        if (num3 == 0 && (double) vector2.Y > (double) (y + height / 2) && (!intList3.Contains(1) && !polygon.containsPoint(new Vector2(vector2.X, vector2.Y - 1f))) && !smoothZone.Contains((int) vector2.X, (int) vector2.Y))
        {
          --vector2.Y;
          intList3.Add(0);
        }
        else if (num3 == 1 && (double) vector2.Y < (double) (y + height) && (!intList3.Contains(0) && !smoothZone.Contains((int) vector2.X, (int) vector2.Y)))
        {
          ++vector2.Y;
          intList3.Add(1);
        }
        else
        {
          --vector2.X;
          ++num7;
          intList3.Add(2);
        }
        intList3.RemoveAt(0);
        polygon.addPoint(vector2);
      }
      int num8 = (int) vector2.Y - (int) polygon.Lines[0].Start.Y - 1;
      --vector2.Y;
      List<int> intList4 = new List<int>() { 2, 2, 2 };
      polygon.addPoint(vector2);
      int num9 = 0;
      while (num9 < num8)
      {
        int num3 = mineRandom.Next(3);
        if (num3 == 0 && (double) vector2.X < (double) (int) polygon.Lines[0].Start.X && (!intList4.Contains(1) && !polygon.containsPoint(new Vector2(vector2.X + 1f, vector2.Y))) && (!polygon.containsPoint(new Vector2(vector2.X, vector2.Y - 1f)) && !smoothZone.Contains((int) vector2.X, (int) vector2.Y)))
        {
          ++vector2.X;
          intList4.Add(0);
        }
        else if (num3 == 1 && (double) vector2.X > (double) (x + 1) && (!intList4.Contains(0) && !smoothZone.Contains((int) vector2.X, (int) vector2.Y)))
        {
          --vector2.X;
          intList4.Add(1);
        }
        else
        {
          --vector2.Y;
          ++num9;
          intList4.Add(2);
        }
        intList4.RemoveAt(0);
        polygon.addPoint(vector2);
      }
      if ((double) vector2.X < (double) (int) polygon.Lines[0].Start.X)
      {
        int num3 = (int) polygon.Lines[0].Start.X + 1 - (int) vector2.X - 1;
        for (int index = 0; index < num3; ++index)
        {
          ++vector2.X;
          polygon.addPoint(vector2);
        }
      }
      return polygon;
    }

    public static Polygon getRandomBorderRoom(Rectangle room, Random mineRandom)
    {
      int width = room.Width - 2;
      int height = room.Height - 2;
      int x = room.X + 1;
      int y = room.Y + 1;
      Rectangle rectangle = new Rectangle(x, y, width, height);
      Polygon polygon = new Polygon();
      Vector2 vector2 = new Vector2((float) mineRandom.Next(x + 5, x + 8), (float) mineRandom.Next(y + 5, y + 8));
      polygon.Lines.Add(new Polygon.Line(vector2, new Vector2(vector2.X + 1f, vector2.Y)));
      ++vector2.X;
      int num1 = room.Right - (int) vector2.X - width / 8;
      int num2 = 2;
      int num3 = 0;
      while (num3 < num1)
      {
        int num4 = mineRandom.Next(3);
        if (num4 == 0 && (double) vector2.Y > (double) room.Y && num2 != 1 || num2 == 2 && (double) vector2.Y >= (double) (y + height / 2))
        {
          --vector2.Y;
          num2 = 0;
        }
        else if (num4 == 1 && (double) vector2.Y < (double) (y + height / 2) && num2 != 0 || num2 == 2 && (double) vector2.Y <= (double) room.Y)
        {
          ++vector2.Y;
          num2 = 1;
        }
        else
        {
          ++vector2.X;
          ++num3;
          num2 = 2;
        }
        polygon.addPoint(vector2);
      }
      int num5 = height - 4 - (int) ((double) vector2.Y - (double) room.Y);
      ++vector2.Y;
      int num6 = 2;
      polygon.addPoint(vector2);
      int num7 = 0;
      while (num7 < num5)
      {
        int num4 = mineRandom.Next(3);
        if (num4 == 0 && (double) vector2.X < (double) room.Right && num6 != 1 || num6 == 2 && (double) vector2.X <= (double) (x + width / 2 + 1))
        {
          ++vector2.X;
          num6 = 0;
        }
        else if (num4 == 1 && (double) vector2.X > (double) (x + width / 2 + 1) && num6 != 0 || num6 == 2 && (double) vector2.X >= (double) room.Right)
        {
          --vector2.X;
          num6 = 1;
        }
        else
        {
          ++vector2.Y;
          ++num7;
          num6 = 2;
        }
        polygon.addPoint(vector2);
      }
      int num8 = (int) vector2.X - (int) polygon.Lines[0].Start.X + width / 4;
      --vector2.X;
      int num9 = 2;
      polygon.addPoint(vector2);
      int num10 = 0;
      while (num10 < num8)
      {
        int num4 = mineRandom.Next(3);
        if (num4 == 0 && (double) vector2.Y > (double) (y + height / 2) && (num9 != 1 && !polygon.containsPoint(new Vector2(vector2.X, vector2.Y - 1f))) || num9 == 2 && (double) vector2.Y >= (double) room.Bottom)
        {
          --vector2.Y;
          num9 = 0;
        }
        else if (num4 == 1 && (double) vector2.Y < (double) room.Bottom && num9 != 0 || num9 == 2 && (double) vector2.Y <= (double) (y + height / 2))
        {
          ++vector2.Y;
          num9 = 1;
        }
        else
        {
          --vector2.X;
          ++num10;
          num9 = 2;
        }
        polygon.addPoint(vector2);
      }
      int num11 = (int) vector2.Y - (int) polygon.Lines[0].Start.Y - 1;
      --vector2.Y;
      int num12 = 2;
      polygon.addPoint(vector2);
      int num13 = 0;
      while (num13 < num11)
      {
        int num4 = mineRandom.Next(3);
        if (num4 == 0 && (double) vector2.X < (double) room.Center.X && (!polygon.containsPoint(new Vector2(vector2.X + 1f, vector2.Y)) && !polygon.containsPoint(new Vector2(vector2.X, vector2.Y - 1f))) || num12 == 2 && (double) vector2.X <= (double) room.X)
        {
          ++vector2.X;
          num12 = 0;
        }
        else if (num4 == 1 && (double) vector2.X > (double) room.X && num12 != 0 || num12 == 2 && (double) vector2.X >= (double) room.Center.X)
        {
          --vector2.X;
          num12 = 1;
        }
        else
        {
          --vector2.Y;
          ++num13;
          num12 = 2;
        }
        polygon.addPoint(vector2);
      }
      if ((double) vector2.X < (double) (int) polygon.Lines[0].Start.X)
      {
        int num4 = (int) polygon.Lines[0].Start.X + 1 - (int) vector2.X - 1;
        for (int index = 0; index < num4; ++index)
        {
          ++vector2.X;
          polygon.addPoint(vector2);
        }
      }
      return polygon;
    }

    public class Line
    {
      public Vector2 Start;
      public Vector2 End;

      public Line(Vector2 Start, Vector2 End)
      {
        this.Start = Start;
        this.End = End;
      }
    }
  }
}
