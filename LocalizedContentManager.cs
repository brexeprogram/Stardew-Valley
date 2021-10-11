// Decompiled with JetBrains decompiler
// Type: StardewValley.LocalizedContentManager
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace StardewValley
{
  public class LocalizedContentManager : ContentManager
  {
    public const string genderDialogueSplitCharacter = "¦";
    private static LocalizedContentManager.LanguageCode _currentLangCode;
    public CultureInfo CurrentCulture;
    public string LanguageCodeOverride;

    public static event LocalizedContentManager.LanguageChangedHandler OnLanguageChange;

    public static LocalizedContentManager.LanguageCode CurrentLanguageCode
    {
      get
      {
        return LocalizedContentManager._currentLangCode;
      }
      set
      {
        if (LocalizedContentManager._currentLangCode == value)
          return;
        LocalizedContentManager._currentLangCode = value;
        if (LocalizedContentManager.OnLanguageChange != null)
          LocalizedContentManager.OnLanguageChange(LocalizedContentManager._currentLangCode);
        Console.WriteLine("Changed language to: " + (object) value);
      }
    }

    public static bool CurrentLanguageLatin
    {
      get
      {
        if (LocalizedContentManager.CurrentLanguageCode != LocalizedContentManager.LanguageCode.en && LocalizedContentManager.CurrentLanguageCode != LocalizedContentManager.LanguageCode.es && LocalizedContentManager.CurrentLanguageCode != LocalizedContentManager.LanguageCode.de)
          return LocalizedContentManager.CurrentLanguageCode == LocalizedContentManager.LanguageCode.pt;
        return true;
      }
    }

    public LocalizedContentManager(IServiceProvider serviceProvider, string rootDirectory, CultureInfo currentCulture, string languageCodeOverride)
      : base(serviceProvider, rootDirectory)
    {
      this.CurrentCulture = currentCulture;
      this.LanguageCodeOverride = languageCodeOverride;
    }

    public LocalizedContentManager(IServiceProvider serviceProvider, string rootDirectory)
      : this(serviceProvider, rootDirectory, Thread.CurrentThread.CurrentUICulture, (string) null)
    {
    }

    public T LoadBase<T>(string assetName)
    {
      return base.Load<T>(assetName);
    }

    public override T Load<T>(string assetName)
    {
      if (LocalizedContentManager.CurrentLanguageCode != LocalizedContentManager.LanguageCode.en)
      {
        string assetName1 = assetName + "." + this.languageCode();
        try
        {
          return base.Load<T>(assetName1);
        }
        catch (ContentLoadException ex)
        {
        }
      }
      return base.Load<T>(assetName);
    }

    private string languageCode()
    {
      if (this.LanguageCodeOverride != null)
        return this.LanguageCodeOverride;
      string str = "";
      switch (LocalizedContentManager.CurrentLanguageCode)
      {
        case LocalizedContentManager.LanguageCode.ja:
          str = "ja-JP";
          break;
        case LocalizedContentManager.LanguageCode.ru:
          str = "ru-RU";
          break;
        case LocalizedContentManager.LanguageCode.zh:
          str = "zh-CN";
          break;
        case LocalizedContentManager.LanguageCode.pt:
          str = "pt-BR";
          break;
        case LocalizedContentManager.LanguageCode.es:
          str = "es-ES";
          break;
        case LocalizedContentManager.LanguageCode.de:
          str = "de-DE";
          break;
        case LocalizedContentManager.LanguageCode.th:
          str = "th-TH";
          break;
      }
      return str;
    }

    public LocalizedContentManager.LanguageCode GetCurrentLanguage()
    {
      return LocalizedContentManager.CurrentLanguageCode;
    }

    private string GetString(Dictionary<string, string> strings, string key)
    {
      string str;
      if (strings.TryGetValue(key + ".desktop", out str))
        return str;
      return strings[key];
    }

    public string LoadString(string path, params object[] substitutions)
    {
      string assetName;
      string key;
      this.parseStringPath(path, out assetName, out key);
      Dictionary<string, string> strings = this.Load<Dictionary<string, string>>(assetName);
      if (strings == null || !strings.ContainsKey(key))
        return this.LoadBaseString(path, substitutions);
      string format = this.GetString(strings, key);
      if (format.Contains("¦"))
        format = !Game1.player.IsMale ? format.Substring(format.IndexOf("¦") + 1) : format.Substring(0, format.IndexOf("¦"));
      if (substitutions.Length == 0)
        return format;
      try
      {
        return string.Format(format, substitutions);
      }
      catch (Exception ex)
      {
        return format;
      }
    }

    public string LoadBaseString(string path, params object[] substitutions)
    {
      string assetName;
      string key;
      this.parseStringPath(path, out assetName, out key);
      Dictionary<string, string> strings = base.Load<Dictionary<string, string>>(assetName);
      if (strings == null || !strings.ContainsKey(key))
        return path;
      string format = this.GetString(strings, key);
      if (substitutions.Length == 0)
        return format;
      try
      {
        return string.Format(format, substitutions);
      }
      catch (Exception ex)
      {
        return format;
      }
    }

    private void parseStringPath(string path, out string assetName, out string key)
    {
      int length = path.IndexOf(':');
      if (length == -1)
        throw new ContentLoadException("Unable to parse string path: " + path);
      assetName = path.Substring(0, length);
      key = path.Substring(length + 1, path.Length - length - 1);
    }

    public LocalizedContentManager CreateTemporary()
    {
      return new LocalizedContentManager(this.ServiceProvider, this.RootDirectory, this.CurrentCulture, this.LanguageCodeOverride);
    }

    public delegate void LanguageChangedHandler(LocalizedContentManager.LanguageCode code);

    public enum LanguageCode
    {
      en,
      ja,
      ru,
      zh,
      pt,
      es,
      de,
      th,
    }
  }
}
