using System.IO;
using Newtonsoft.Json;
// ReSharper disable MemberHidesStaticFromOuterClass

namespace BlackTrader.Config
{
    public static class Configs
    {
        private static ConfigData _data = new ConfigData();

        public static int UpdateDateHoursLimit
        {
            get => _data.UpdateDateHoursLimit;
            set => _data.UpdateDateHoursLimit = value;
        }
        public static string NameOfDataPoolAutoSaveFile
        {
            get => _data.NameOfDataPoolAutoSaveFile;
            set => _data.NameOfDataPoolAutoSaveFile = value;
        }

        public static void Save()
        {
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), nameof(Configs) + ".json"),
                JsonConvert.SerializeObject(_data));
        }

        public static void Load()
        {
            if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), nameof(Configs) + ".json")))
                _data = JsonConvert.DeserializeObject<ConfigData>(
                    File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), nameof(Configs) + ".json")));
        }

        private class ConfigData
        {
            public int UpdateDateHoursLimit;
            public string NameOfDataPoolAutoSaveFile;

            public ConfigData()
            {
                UpdateDateHoursLimit = 24;
                NameOfDataPoolAutoSaveFile = "DataPool.AutoSave";
            }
        }
    }
}