// Decompiled with JetBrains decompiler
// Type: StardewValley.Quests.ItemDeliveryQuest
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Quests
{
  public class ItemDeliveryQuest : Quest
  {
    public int number = 1;
    public List<DescriptionElement> parts = new List<DescriptionElement>();
    public List<DescriptionElement> dialogueparts = new List<DescriptionElement>();
    public string targetMessage;
    public string target;
    public int item;
    public NPC actualTarget;
    public StardewValley.Object deliveryItem;
    public DescriptionElement objective;

    public ItemDeliveryQuest()
    {
      this.questType = 3;
    }

    public void loadQuestInfo()
    {
      if (this.target != null)
        return;
      this.questTitle = Game1.content.LoadString("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13285");
      if (Game1.player.friendships == null || Game1.player.friendships.Count <= 0)
        return;
      this.target = Game1.player.friendships.Keys.ElementAt<string>(this.random.Next(Game1.player.friendships.Count));
      int num1 = 0;
      this.actualTarget = Game1.getCharacterFromName(this.target, false);
      if ((object) this.actualTarget == null)
        return;
      for (; num1 < 30 && (this.target == null || (object) this.actualTarget == null || (this.actualTarget.isInvisible || this.actualTarget.name.Equals(Game1.player.spouse)) || (this.actualTarget.name.Equals("Krobus") || this.actualTarget.name.Contains("Qi") || (this.actualTarget.name.Contains("Dwarf") || this.actualTarget.name.Contains("Gunther"))) || (this.actualTarget.age == 2 || this.actualTarget.name.Contains("Bouncer") || (this.actualTarget.name.Contains("Henchman") || this.actualTarget.name.Contains("Marlon")) || (this.actualTarget.name.Contains("Mariner") || !this.actualTarget.isVillager() || this.actualTarget.name.Equals("Sandy") && !Game1.player.eventsSeen.Contains(67)))); this.actualTarget = Game1.getCharacterFromName(this.target, false))
      {
        ++num1;
        this.target = Game1.player.friendships.Keys.ElementAt<string>(this.random.Next(Game1.player.friendships.Count));
      }
      if ((object) this.actualTarget == null)
        return;
      if (num1 >= 30 || this.target.Equals("Wizard") && !Game1.player.mailReceived.Contains("wizardJunimoNote") && !Game1.player.mailReceived.Contains("JojaMember"))
      {
        this.target = "Demetrius";
        this.actualTarget = Game1.getCharacterFromName(this.target, false);
      }
      if (!Game1.currentSeason.Equals("winter") && this.random.NextDouble() < 0.15)
      {
        List<int> source = Utility.possibleCropsAtThisTime(Game1.currentSeason, Game1.dayOfMonth <= 7);
        this.item = source.ElementAt<int>(this.random.Next(source.Count));
        this.deliveryItem = new StardewValley.Object(Vector2.Zero, this.item, 1);
        this.parts.Clear();
        this.parts.Add((DescriptionElement) (this.random.NextDouble() < 0.3 ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13299" : (this.random.NextDouble() < 0.5 ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13300" : "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13301")));
        this.parts.Add(this.random.NextDouble() < 0.3 ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13302", (object) this.deliveryItem) : (this.random.NextDouble() < 0.5 ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13303", (object) this.deliveryItem) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13304", (object) this.deliveryItem)));
        this.parts.Add((DescriptionElement) (this.random.NextDouble() < 0.25 ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13306" : (this.random.NextDouble() < 0.33 ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13307" : (this.random.NextDouble() < 0.5 ? "" : "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13308"))));
        this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", (object) this.actualTarget));
        if (this.target.Equals("Demetrius"))
        {
          this.parts.Clear();
          this.parts.Add(this.random.NextDouble() < 0.5 ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13311", (object) this.deliveryItem) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13314", (object) this.deliveryItem));
        }
        if (this.target.Equals("Marnie"))
        {
          this.parts.Clear();
          this.parts.Add(this.random.NextDouble() < 0.5 ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13317", (object) this.deliveryItem) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13320", (object) this.deliveryItem));
        }
        if (this.target.Equals("Sebastian"))
        {
          this.parts.Clear();
          this.parts.Add(this.random.NextDouble() < 0.5 ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13324", (object) this.deliveryItem) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13327", (object) this.deliveryItem));
        }
      }
      else
      {
        this.item = Utility.getRandomItemFromSeason(Game1.currentSeason, 1000, true);
        if (this.item == -5)
          this.item = 176;
        if (this.item == -6)
          this.item = 184;
        this.deliveryItem = new StardewValley.Object(Vector2.Zero, this.item, 1);
        DescriptionElement[] descriptionElementArray1 = (DescriptionElement[]) null;
        DescriptionElement[] descriptionElementArray2 = (DescriptionElement[]) null;
        DescriptionElement[] descriptionElementArray3 = (DescriptionElement[]) null;
        if (Game1.objectInformation[this.item].Split('/')[3].Split(' ')[0].Equals("Cooking") && !this.target.Equals("Wizard"))
        {
          if (this.random.NextDouble() < 0.33)
          {
            DescriptionElement[] descriptionElementArray4 = new DescriptionElement[12]
            {
              (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13336",
              (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13337",
              (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13338",
              (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13339",
              (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13340",
              (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13341",
              Game1.samBandName.Equals(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2156")) ? (!Game1.elliottBookName.Equals(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2157")) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13342", (object) new DescriptionElement("Strings\\StringsFromCSFiles:Game1.cs.2157")) : (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13346") : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13347", (object) new DescriptionElement("Strings\\StringsFromCSFiles:Game1.cs.2156")),
              (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13349",
              (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13350",
              (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13351",
              (DescriptionElement) (Game1.currentSeason.Equals("winter") ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13353" : (Game1.currentSeason.Equals("summer") ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13355" : "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13356")),
              (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13357"
            };
            this.parts.Clear();
            this.parts.Add(this.random.NextDouble() < 0.5 ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13333", (object) this.deliveryItem, (object) ((IEnumerable<DescriptionElement>) descriptionElementArray4).ElementAt<DescriptionElement>(this.random.Next(12))) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13334", (object) this.deliveryItem, (object) ((IEnumerable<DescriptionElement>) descriptionElementArray4).ElementAt<DescriptionElement>(this.random.Next(12))));
            this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", (object) this.actualTarget));
          }
          else
          {
            DescriptionElement descriptionElement = new DescriptionElement();
            switch (Game1.dayOfMonth % 7)
            {
              case 0:
                descriptionElement = (DescriptionElement) "Strings\\StringsFromCSFiles:Game1.cs.3042";
                break;
              case 1:
                descriptionElement = (DescriptionElement) "Strings\\StringsFromCSFiles:Game1.cs.3043";
                break;
              case 2:
                descriptionElement = (DescriptionElement) "Strings\\StringsFromCSFiles:Game1.cs.3044";
                break;
              case 3:
                descriptionElement = (DescriptionElement) "Strings\\StringsFromCSFiles:Game1.cs.3045";
                break;
              case 4:
                descriptionElement = (DescriptionElement) "Strings\\StringsFromCSFiles:Game1.cs.3046";
                break;
              case 5:
                descriptionElement = (DescriptionElement) "Strings\\StringsFromCSFiles:Game1.cs.3047";
                break;
              case 6:
                descriptionElement = (DescriptionElement) "Strings\\StringsFromCSFiles:Game1.cs.3048";
                break;
            }
            descriptionElementArray1 = new DescriptionElement[5]
            {
              new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13360", (object) this.deliveryItem),
              new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13364", (object) this.deliveryItem),
              new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13367", (object) this.deliveryItem),
              new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13370", (object) this.deliveryItem),
              new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13373", (object) descriptionElement, (object) this.deliveryItem, (object) this.actualTarget)
            };
            descriptionElementArray2 = new DescriptionElement[5]
            {
              new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", (object) this.actualTarget),
              new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", (object) this.actualTarget),
              new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", (object) this.actualTarget),
              new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", (object) this.actualTarget),
              (DescriptionElement) ""
            };
            descriptionElementArray3 = new DescriptionElement[5]
            {
              (DescriptionElement) "",
              (DescriptionElement) "",
              (DescriptionElement) "",
              (DescriptionElement) "",
              (DescriptionElement) ""
            };
          }
          this.parts.Clear();
          int index = this.random.Next(((IEnumerable<DescriptionElement>) descriptionElementArray1).Count<DescriptionElement>());
          this.parts.Add(descriptionElementArray1[index]);
          this.parts.Add(descriptionElementArray2[index]);
          this.parts.Add(descriptionElementArray3[index]);
          if (this.target.Equals("Sebastian"))
          {
            this.parts.Clear();
            this.parts.Add(this.random.NextDouble() < 0.5 ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13378", (object) this.deliveryItem) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13381", (object) this.deliveryItem));
          }
        }
        else
        {
          if (this.random.NextDouble() < 0.5)
          {
            if (Convert.ToInt32(Game1.objectInformation[this.item].Split('/')[2]) > 0)
            {
              DescriptionElement[] descriptionElementArray4 = new DescriptionElement[2]
              {
                new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13383", (object) this.deliveryItem, (object) ((IEnumerable<DescriptionElement>) new DescriptionElement[12]
                {
                  (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13385",
                  (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13386",
                  (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13387",
                  (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13388",
                  (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13389",
                  (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13390",
                  (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13391",
                  (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13392",
                  (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13393",
                  (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13394",
                  (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13395",
                  (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13396"
                }).ElementAt<DescriptionElement>(this.random.Next(12))),
                new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13400", (object) this.deliveryItem)
              };
              DescriptionElement[] descriptionElementArray5 = new DescriptionElement[2]
              {
                new DescriptionElement(this.random.NextDouble() < 0.5 ? "" : "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13398"),
                new DescriptionElement(this.random.NextDouble() < 0.5 ? "" : "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13402")
              };
              DescriptionElement[] descriptionElementArray6 = new DescriptionElement[2]
              {
                new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", (object) this.actualTarget),
                new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", (object) this.actualTarget)
              };
              if (this.random.NextDouble() < 0.33)
              {
                DescriptionElement[] descriptionElementArray7 = new DescriptionElement[12]
                {
                  (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13336",
                  (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13337",
                  (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13338",
                  (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13339",
                  (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13340",
                  (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13341",
                  Game1.samBandName.Equals(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2156")) ? (!Game1.elliottBookName.Equals(Game1.content.LoadString("Strings\\StringsFromCSFiles:Game1.cs.2157")) ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13342", (object) new DescriptionElement("Strings\\StringsFromCSFiles:Game1.cs.2157")) : (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13346") : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13347", (object) new DescriptionElement("Strings\\StringsFromCSFiles:Game1.cs.2156")),
                  (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13420",
                  (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13421",
                  (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13422",
                  (DescriptionElement) (Game1.currentSeason.Equals("winter") ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13424" : (Game1.currentSeason.Equals("summer") ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13426" : "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13427")),
                  (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13357"
                };
                this.parts.Clear();
                this.parts.Add(this.random.NextDouble() < 0.5 ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13333", (object) this.deliveryItem, (object) ((IEnumerable<DescriptionElement>) descriptionElementArray7).ElementAt<DescriptionElement>(this.random.Next(12))) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13334", (object) this.deliveryItem, (object) ((IEnumerable<DescriptionElement>) descriptionElementArray7).ElementAt<DescriptionElement>(this.random.Next(12))));
                this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", (object) this.actualTarget));
              }
              else
              {
                this.parts.Clear();
                int index = this.random.Next(((IEnumerable<DescriptionElement>) descriptionElementArray4).Count<DescriptionElement>());
                this.parts.Add(descriptionElementArray4[index]);
                this.parts.Add(descriptionElementArray5[index]);
                this.parts.Add(descriptionElementArray6[index]);
              }
              if (this.target.Equals("Demetrius"))
              {
                this.parts.Clear();
                this.parts.Add(this.random.NextDouble() < 0.5 ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13311", (object) this.deliveryItem) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13314", (object) this.deliveryItem));
              }
              if (this.target.Equals("Marnie"))
              {
                this.parts.Clear();
                this.parts.Add(this.random.NextDouble() < 0.5 ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13317", (object) this.deliveryItem) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13320", (object) this.deliveryItem));
              }
              if (this.target.Equals("Harvey"))
              {
                DescriptionElement[] descriptionElementArray7 = new DescriptionElement[12]
                {
                  (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13448",
                  (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13449",
                  (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13450",
                  (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13451",
                  (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13452",
                  (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13453",
                  (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13454",
                  (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13455",
                  (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13456",
                  (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13457",
                  (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13458",
                  (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13459"
                };
                this.parts.Clear();
                this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13446", (object) this.deliveryItem, (object) ((IEnumerable<DescriptionElement>) descriptionElementArray7).ElementAt<DescriptionElement>(this.random.Next(12))));
              }
              if (this.target.Equals("Gus") && this.random.NextDouble() < 0.6)
              {
                this.parts.Clear();
                this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13462", (object) this.deliveryItem));
                goto label_55;
              }
              else
                goto label_55;
            }
          }
          if (this.random.NextDouble() < 0.5)
          {
            if (Convert.ToInt32(Game1.objectInformation[this.item].Split('/')[2]) < 0)
            {
              this.parts.Clear();
              this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13464", (object) this.deliveryItem, (object) ((IEnumerable<DescriptionElement>) new DescriptionElement[5]
              {
                (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13465",
                (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13466",
                (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13467",
                (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13468",
                (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13469"
              }).ElementAt<DescriptionElement>(this.random.Next(5))));
              this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", (object) this.actualTarget.displayName));
              if (this.target.Equals("Emily"))
              {
                this.parts.Clear();
                this.parts.Add(this.random.NextDouble() < 0.5 ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13473", (object) this.deliveryItem) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13476", (object) this.deliveryItem));
                goto label_55;
              }
              else
                goto label_55;
            }
          }
          DescriptionElement[] descriptionElementArray8 = new DescriptionElement[9]
          {
            new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13480", (object) this.actualTarget, (object) this.deliveryItem),
            new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13481", (object) this.deliveryItem),
            new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13485", (object) this.deliveryItem),
            this.random.NextDouble() < 0.4 ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13491", (object) this.deliveryItem) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13492", (object) this.deliveryItem),
            new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13494", (object) this.deliveryItem),
            new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13497", (object) this.deliveryItem),
            new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13500", (object) this.deliveryItem, (object) ((IEnumerable<DescriptionElement>) new DescriptionElement[12]
            {
              (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13502",
              (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13503",
              (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13504",
              (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13505",
              (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13506",
              (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13507",
              (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13508",
              (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13509",
              (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13510",
              (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13511",
              (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13512",
              (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13513"
            }).ElementAt<DescriptionElement>(this.random.Next(12))),
            new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13518", (object) this.actualTarget, (object) this.deliveryItem),
            this.random.NextDouble() < 0.5 ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13520", (object) this.deliveryItem) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13523", (object) this.deliveryItem)
          };
          DescriptionElement[] descriptionElementArray9 = new DescriptionElement[9]
          {
            (DescriptionElement) "",
            (DescriptionElement) (this.random.NextDouble() < 0.3 ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13482" : (this.random.NextDouble() < 0.5 ? "" : "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13483")),
            (DescriptionElement) (this.random.NextDouble() < 0.25 ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13487" : (this.random.NextDouble() < 0.33 ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13488" : (this.random.NextDouble() < 0.5 ? "" : "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13489"))),
            new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", (object) this.actualTarget),
            new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", (object) this.actualTarget),
            new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", (object) this.actualTarget),
            (DescriptionElement) (this.random.NextDouble() < 0.5 ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13514" : "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13516"),
            (DescriptionElement) "",
            new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", (object) this.actualTarget)
          };
          DescriptionElement[] descriptionElementArray10 = new DescriptionElement[9]
          {
            (DescriptionElement) "",
            new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", (object) this.actualTarget),
            new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", (object) this.actualTarget),
            (DescriptionElement) "",
            (DescriptionElement) "",
            (DescriptionElement) "",
            new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13620", (object) this.actualTarget),
            (DescriptionElement) "",
            (DescriptionElement) ""
          };
          this.parts.Clear();
          int index1 = this.random.Next(((IEnumerable<DescriptionElement>) descriptionElementArray8).Count<DescriptionElement>());
          this.parts.Add(descriptionElementArray8[index1]);
          this.parts.Add(descriptionElementArray9[index1]);
          this.parts.Add(descriptionElementArray10[index1]);
        }
      }
label_55:
      this.dialogueparts.Clear();
      this.dialogueparts.Add(this.random.NextDouble() < 0.3 || this.target.Equals("Evelyn") ? (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13526" : (this.random.NextDouble() < 0.5 ? (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13527" : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13528", (object) Game1.player.name)));
      this.dialogueparts.Add(this.random.NextDouble() < 0.3 ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13530", (object) this.deliveryItem) : (this.random.NextDouble() < 0.5 ? (DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13532" : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13533", this.random.NextDouble() < 0.3 ? (object) new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13534") : (this.random.NextDouble() < 0.5 ? (object) new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13535") : (object) new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13536")))));
      this.dialogueparts.Add((DescriptionElement) (this.random.NextDouble() < 0.3 ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13538" : (this.random.NextDouble() < 0.5 ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13539" : "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13540")));
      this.dialogueparts.Add((DescriptionElement) (this.random.NextDouble() < 0.3 ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13542" : (this.random.NextDouble() < 0.5 ? "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13543" : "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13544")));
      if (this.target.Equals("Wizard"))
      {
        this.parts.Clear();
        if (this.random.NextDouble() < 0.5)
          this.parts.Add(this.random.NextDouble() < 0.5 ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13546", (object) this.deliveryItem) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13548", (object) this.deliveryItem));
        else
          this.parts.Add(this.random.NextDouble() < 0.5 ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13551", (object) this.deliveryItem) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13553", (object) this.deliveryItem));
        this.dialogueparts.Clear();
        this.dialogueparts.Add((DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13555");
      }
      if (this.target.Equals("Haley"))
      {
        this.parts.Clear();
        this.parts.Add(this.random.NextDouble() < 0.5 ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13557", (object) this.deliveryItem) : (Game1.player.isMale ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13560", (object) this.deliveryItem) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13563", (object) this.deliveryItem)));
        this.dialogueparts.Clear();
        this.dialogueparts.Add((DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13566");
      }
      if (this.target.Equals("Sam"))
      {
        this.parts.Clear();
        this.parts.Add(this.random.NextDouble() < 0.5 ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13568", (object) this.deliveryItem) : (Game1.player.isMale ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13571", (object) this.deliveryItem) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13574", (object) this.deliveryItem)));
        this.dialogueparts.Clear();
        this.dialogueparts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13577", (object) Game1.player.name));
      }
      if (this.target.Equals("Maru"))
      {
        this.parts.Clear();
        double num2 = this.random.NextDouble();
        this.parts.Add(num2 < 0.5 ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13580", (object) this.deliveryItem) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13583", (object) this.deliveryItem));
        this.dialogueparts.Clear();
        this.dialogueparts.Add(num2 < 0.5 ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13585", (object) Game1.player.name) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13587", (object) Game1.player.name));
      }
      if (this.target.Equals("Abigail"))
      {
        this.parts.Clear();
        double num2 = this.random.NextDouble();
        this.parts.Add(num2 < 0.5 ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13590", (object) this.deliveryItem) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13593", (object) this.deliveryItem));
        this.dialogueparts.Add(num2 < 0.5 ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13597", (object) Game1.player.name) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13599", (object) Game1.player.name));
      }
      if (this.target.Equals("Sebastian"))
      {
        this.dialogueparts.Clear();
        this.dialogueparts.Add((DescriptionElement) "Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13602");
      }
      if (this.target.Equals("Elliott"))
      {
        this.dialogueparts.Clear();
        this.dialogueparts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13604", (object) this.deliveryItem, (object) Game1.player.name));
      }
      DescriptionElement descriptionElement1 = this.random.NextDouble() >= 0.3 ? (this.random.NextDouble() >= 0.5 ? new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13612", (object) this.actualTarget) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13610", (object) this.actualTarget)) : new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13608", (object) this.actualTarget);
      this.parts.Add(new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13607", (object) (this.deliveryItem.price * 3)));
      this.parts.Add(descriptionElement1);
      this.objective = new DescriptionElement("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13614", (object) this.actualTarget, (object) this.deliveryItem);
    }

    public override void reloadDescription()
    {
      if (this._questDescription == "" && this.target != null)
        return;
      if (this._questDescription == "")
        this.loadQuestInfo();
      if (this.parts.Count == 0 || this.parts == null || (this.dialogueparts.Count == 0 || this.dialogueparts == null))
        return;
      string str1 = "";
      string str2 = "";
      foreach (DescriptionElement part in this.parts)
        str1 += part.loadDescriptionElement();
      foreach (DescriptionElement dialoguepart in this.dialogueparts)
        str2 += dialoguepart.loadDescriptionElement();
      this.questDescription = str1;
      this.targetMessage = str2;
    }

    public override void reloadObjective()
    {
      if (this.objective == null)
        return;
      this.currentObjective = this.objective.loadDescriptionElement();
    }

    public override bool checkIfComplete(NPC n = null, int number1 = -1, int number2 = -1, Item item = null, string monsterName = null)
    {
      if (this.completed || item == null || (!(item is StardewValley.Object) || n == null) || (!n.isVillager() || !n.name.Equals(this.target) || (item as StardewValley.Object).ParentSheetIndex != this.item && (item as StardewValley.Object).Category != this.item))
        return false;
      if (item.Stack >= this.number)
      {
        Game1.player.ActiveObject.Stack -= this.number - 1;
        n.CurrentDialogue.Push(new Dialogue(this.targetMessage, n));
        Game1.drawDialogue(n);
        Game1.player.reduceActiveItemByOne();
        if (this.dailyQuest)
        {
          Game1.player.changeFriendship(150, n);
          if (this.deliveryItem == null)
            this.deliveryItem = new StardewValley.Object(Vector2.Zero, this.item, 1);
          this.moneyReward = this.deliveryItem.price * 3;
        }
        else
          Game1.player.changeFriendship((int) byte.MaxValue, n);
        this.questComplete();
        return true;
      }
      n.CurrentDialogue.Push(new Dialogue(Game1.content.LoadString("Strings\\StringsFromCSFiles:ItemDeliveryQuest.cs.13615", (object) this.number), n));
      Game1.drawDialogue(n);
      return false;
    }
  }
}
