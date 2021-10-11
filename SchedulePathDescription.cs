// Decompiled with JetBrains decompiler
// Type: StardewValley.SchedulePathDescription
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace StardewValley
{
  public class SchedulePathDescription
  {
    public Stack<Point> route;
    public int facingDirection;
    public string endOfRouteBehavior;
    public string endOfRouteMessage;

    public SchedulePathDescription(Stack<Point> route, int facingDirection, string endBehavior, string endMessage)
    {
      this.endOfRouteMessage = endMessage;
      this.route = route;
      this.facingDirection = facingDirection;
      this.endOfRouteBehavior = endBehavior;
    }
  }
}
