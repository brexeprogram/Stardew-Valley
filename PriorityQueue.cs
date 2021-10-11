// Decompiled with JetBrains decompiler
// Type: StardewValley.PriorityQueue
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using System.Collections.Generic;

namespace StardewValley
{
  public class PriorityQueue
  {
    private int total_size;
    private SortedDictionary<int, Queue<PathNode>> nodes;

    public PriorityQueue()
    {
      this.nodes = new SortedDictionary<int, Queue<PathNode>>();
      this.total_size = 0;
    }

    public bool IsEmpty()
    {
      return this.total_size == 0;
    }

    public bool Contains(PathNode p, int priority)
    {
      if (!this.nodes.ContainsKey(priority))
        return false;
      return this.nodes[priority].Contains(p);
    }

    public PathNode Dequeue()
    {
      if (!this.IsEmpty())
      {
        foreach (Queue<PathNode> pathNodeQueue in this.nodes.Values)
        {
          if (pathNodeQueue.Count > 0)
          {
            this.total_size = this.total_size - 1;
            return pathNodeQueue.Dequeue();
          }
        }
      }
      return (PathNode) null;
    }

    public object Peek()
    {
      if (!this.IsEmpty())
      {
        foreach (Queue<PathNode> pathNodeQueue in this.nodes.Values)
        {
          if (pathNodeQueue.Count > 0)
            return (object) pathNodeQueue.Peek();
        }
      }
      return (object) null;
    }

    public object Dequeue(int priority)
    {
      this.total_size = this.total_size - 1;
      return (object) this.nodes[priority].Dequeue();
    }

    public void Enqueue(PathNode item, int priority)
    {
      if (!this.nodes.ContainsKey(priority))
      {
        this.nodes.Add(priority, new Queue<PathNode>());
        this.Enqueue(item, priority);
      }
      else
      {
        this.nodes[priority].Enqueue(item);
        this.total_size = this.total_size + 1;
      }
    }
  }
}
