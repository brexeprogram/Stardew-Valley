// Decompiled with JetBrains decompiler
// Type: StardewValley.StartupPreferences
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace StardewValley
{
  public class StartupPreferences
  {
    public static string filename = "startup_preferences";
    public static XmlSerializer serializer = new XmlSerializer(typeof (StartupPreferences));
    public const int windowed_borderless = 0;
    public const int windowed = 1;
    public const int fullscreen = 2;
    public bool startMuted;
    public bool levelTenFishing;
    public bool levelTenMining;
    public bool levelTenForaging;
    public bool levelTenCombat;
    public bool skipWindowPreparation;
    public int timesPlayed;
    public int windowMode;
    public LocalizedContentManager.LanguageCode languageCode;

    public void Init()
    {
      this.ensureFolderStructureExists();
      LocalizedContentManager.OnLanguageChange += new LocalizedContentManager.LanguageChangedHandler(this.OnLanguageChange);
    }

    private void OnLanguageChange(LocalizedContentManager.LanguageCode code)
    {
      this.savePreferences();
    }

    private void ensureFolderStructureExists()
    {
      FileInfo fileInfo = new FileInfo(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StardewValley", "placeholder")));
      if (!fileInfo.Directory.Exists)
        fileInfo.Directory.Create();
    }

    public void savePreferences()
    {
      Task task = new Task((Action) (() =>
      {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        this._savePreferences();
      }));
      task.Start();
      task.Wait();
      if (task.IsFaulted)
        throw task.Exception.GetBaseException();
    }

    private void _savePreferences()
    {
      string path = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StardewValley"), StartupPreferences.filename);
      try
      {
        this.ensureFolderStructureExists();
        if (File.Exists(path))
          File.Delete(path);
        using (FileStream fileStream = File.Create(path))
          this.writeSettings((Stream) fileStream);
      }
      catch (Exception ex)
      {
        Game1.debugOutput = Game1.parseText(ex.Message);
      }
    }

    private void writeSettings(Stream stream)
    {
      using (XmlWriter xmlWriter = XmlWriter.Create(stream, new XmlWriterSettings()
      {
        CloseOutput = true
      }))
      {
        xmlWriter.WriteStartDocument();
        this.languageCode = LocalizedContentManager.CurrentLanguageCode;
        StartupPreferences.serializer.Serialize(xmlWriter, (object) this);
        xmlWriter.WriteEndDocument();
        xmlWriter.Flush();
      }
    }

    public void loadPreferences()
    {
      this.Init();
      Task task = new Task((Action) (() =>
      {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        this._loadPreferences();
      }));
      task.Start();
      task.Wait();
      if (task.IsFaulted)
        throw task.Exception.GetBaseException();
    }

    private void _loadPreferences()
    {
      string path = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StardewValley"), StartupPreferences.filename);
      if (!File.Exists(path))
      {
        Game1.debugOutput = "File does not exist (-_-)";
      }
      else
      {
        try
        {
          using (FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read))
            this.readSettings((Stream) fileStream);
        }
        catch (Exception ex)
        {
          Game1.debugOutput = Game1.parseText(ex.Message);
        }
      }
    }

    private void readSettings(Stream stream)
    {
      StartupPreferences startupPreferences = (StartupPreferences) StartupPreferences.serializer.Deserialize(stream);
      this.startMuted = startupPreferences.startMuted;
      this.timesPlayed = startupPreferences.timesPlayed + 1;
      this.levelTenCombat = startupPreferences.levelTenCombat;
      this.levelTenFishing = startupPreferences.levelTenFishing;
      this.levelTenForaging = startupPreferences.levelTenForaging;
      this.levelTenMining = startupPreferences.levelTenMining;
      this.skipWindowPreparation = startupPreferences.skipWindowPreparation;
      this.windowMode = startupPreferences.windowMode;
      LocalizedContentManager.CurrentLanguageCode = startupPreferences.languageCode;
    }
  }
}
