// Decompiled with JetBrains decompiler
// Type: StardewValley.SerializableDictionary`2
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace StardewValley
{
  [XmlRoot("dictionary")]
  public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable, INotifyCollectionChanged
  {
    public XmlSchema GetSchema()
    {
      return (XmlSchema) null;
    }

    public new void Add(TKey key, TValue value)
    {
      base.Add(key, value);
      this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, (object) key, 0));
    }

    public new bool Remove(TKey key)
    {
      int num = base.Remove(key) ? 1 : 0;
      this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, (object) key, 0));
      return num != 0;
    }

    protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.CollectionChanged == null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.CollectionChanged((object) null, e);
    }

    public void ReadXml(XmlReader reader)
    {
      XmlSerializer xmlSerializer1 = new XmlSerializer(typeof (TKey));
      XmlSerializer xmlSerializer2 = new XmlSerializer(typeof (TValue));
      int num = reader.IsEmptyElement ? 1 : 0;
      reader.Read();
      if (num != 0)
        return;
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        reader.ReadStartElement("item");
        reader.ReadStartElement("key");
        TKey key = (TKey) xmlSerializer1.Deserialize(reader);
        reader.ReadEndElement();
        reader.ReadStartElement("value");
        TValue obj = (TValue) xmlSerializer2.Deserialize(reader);
        reader.ReadEndElement();
        this.Add(key, obj);
        reader.ReadEndElement();
        int content = (int) reader.MoveToContent();
      }
      reader.ReadEndElement();
    }

    public void WriteXml(XmlWriter writer)
    {
      XmlSerializer xmlSerializer1 = new XmlSerializer(typeof (TKey));
      XmlSerializer xmlSerializer2 = new XmlSerializer(typeof (TValue));
      foreach (TKey key in this.Keys)
      {
        writer.WriteStartElement("item");
        writer.WriteStartElement("key");
        xmlSerializer1.Serialize(writer, (object) key);
        writer.WriteEndElement();
        writer.WriteStartElement("value");
        TValue obj = this[key];
        xmlSerializer2.Serialize(writer, (object) obj);
        writer.WriteEndElement();
        writer.WriteEndElement();
      }
    }

    public event NotifyCollectionChangedEventHandler CollectionChanged;
  }
}
