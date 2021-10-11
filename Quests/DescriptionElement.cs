// Decompiled with JetBrains decompiler
// Type: StardewValley.Quests.DescriptionElement
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using StardewValley.Monsters;
using System.Collections.Generic;
using System.Linq;

namespace StardewValley.Quests
{
  public class DescriptionElement
  {
    public string xmlKey;
    public List<object> param;

    public static implicit operator DescriptionElement(string key)
    {
      return new DescriptionElement(key);
    }

    public DescriptionElement()
    {
      this.xmlKey = string.Empty;
      this.param = new List<object>();
    }

    public DescriptionElement(string key)
    {
      this.xmlKey = key;
      this.param = new List<object>();
    }

    public DescriptionElement(string key, object param1)
    {
      this.xmlKey = key;
      this.param = new List<object>();
      this.param.Add(param1);
    }

    public DescriptionElement(string key, List<object> paramlist)
    {
      this.xmlKey = key;
      this.param = new List<object>();
      foreach (object obj in paramlist)
        this.param.Add(obj);
    }

    public DescriptionElement(string key, object param1, object param2)
    {
      this.xmlKey = key;
      this.param = new List<object>();
      this.param.Add(param1);
      this.param.Add(param2);
    }

    public DescriptionElement(string key, object param1, object param2, object param3)
    {
      this.xmlKey = key;
      this.param = new List<object>();
      this.param.Add(param1);
      this.param.Add(param2);
      this.param.Add(param3);
    }

    public string loadDescriptionElement()
    {
      DescriptionElement descriptionElement1 = new DescriptionElement(this.xmlKey, this.param);
      for (int index1 = 0; index1 < descriptionElement1.param.Count; ++index1)
      {
        if (descriptionElement1.param[index1].GetType() == typeof (DescriptionElement))
        {
          DescriptionElement descriptionElement2 = descriptionElement1.param[index1] as DescriptionElement;
          descriptionElement1.param[index1] = (object) descriptionElement2.loadDescriptionElement();
        }
        if (descriptionElement1.param[index1].GetType() == typeof (Object))
        {
          string str;
          Game1.objectInformation.TryGetValue((descriptionElement1.param[index1] as Object).parentSheetIndex, out str);
          descriptionElement1.param[index1] = (object) str.Split('/')[4];
        }
        if (descriptionElement1.param[index1].GetType() == typeof (Monster))
        {
          DescriptionElement descriptionElement2;
          if ((descriptionElement1.param[index1] as Monster).name.Equals("Frost Jelly"))
          {
            descriptionElement2 = new DescriptionElement("Strings\\StringsFromCSFiles:SlayMonsterQuest.cs.13772");
            descriptionElement1.param[index1] = (object) descriptionElement2.loadDescriptionElement();
          }
          else
          {
            descriptionElement2 = new DescriptionElement("Data\\Monsters:" + (descriptionElement1.param[index1] as Monster).name);
            List<object> objectList = descriptionElement1.param;
            int index2 = index1;
            string str;
            if (LocalizedContentManager.CurrentLanguageCode != LocalizedContentManager.LanguageCode.en)
              str = ((IEnumerable<string>) descriptionElement2.loadDescriptionElement().Split('/')).Last<string>();
            else
              str = ((IEnumerable<string>) descriptionElement2.loadDescriptionElement().Split('/')).Last<string>() + "s";
            objectList[index2] = (object) str;
          }
          descriptionElement1.param[index1] = (object) ((IEnumerable<string>) descriptionElement2.loadDescriptionElement().Split('/')).Last<string>();
        }
        if (descriptionElement1.param[index1].GetType() == typeof (NPC))
        {
          DescriptionElement descriptionElement2 = new DescriptionElement("Data\\NPCDispositions:" + (descriptionElement1.param[index1] as NPC).name);
          descriptionElement1.param[index1] = (object) ((IEnumerable<string>) descriptionElement2.loadDescriptionElement().Split('/')).Last<string>();
        }
      }
      if (descriptionElement1.xmlKey == "")
        return string.Empty;
      string str1;
      switch (descriptionElement1.param.Count)
      {
        case 1:
          str1 = Game1.content.LoadString(descriptionElement1.xmlKey, descriptionElement1.param[0]);
          break;
        case 2:
          str1 = Game1.content.LoadString(descriptionElement1.xmlKey, descriptionElement1.param[0], descriptionElement1.param[1]);
          break;
        case 3:
          str1 = Game1.content.LoadString(descriptionElement1.xmlKey, descriptionElement1.param[0], descriptionElement1.param[1], descriptionElement1.param[2]);
          break;
        case 4:
          str1 = Game1.content.LoadString(descriptionElement1.xmlKey, descriptionElement1.param[0], descriptionElement1.param[1], descriptionElement1.param[2], descriptionElement1.param[3]);
          break;
        default:
          str1 = Game1.content.LoadString(descriptionElement1.xmlKey);
          if (this.xmlKey.Contains("Dialogue.cs.7") || this.xmlKey.Contains("Dialogue.cs.8"))
          {
            string str2 = Game1.content.LoadString(descriptionElement1.xmlKey).Replace("/", " ");
            str1 = (int) str2[0] == 32 ? str2.Substring(1) : str2;
            break;
          }
          break;
      }
      return str1;
    }
  }
}
