using Newtonsoft.Json;
using System;
using System.IO;

namespace Common
{
    public class Config
    {
        public static readonly string DefaultConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PushbulletOVRlay");
        public static readonly string DefaultConfigFile = Path.Combine(DefaultConfigPath, "config.json");

        [JsonProperty("pushbullet_token")]
        public string PushbulletToken = "";

        public void Write(string path = null, bool createIfMissing = true)
        {
            if (path == null || path == "") path = DefaultConfigFile;

            if (!File.Exists(path) && createIfMissing)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }

            File.WriteAllText(path, JsonConvert.SerializeObject(this));
        }

        public static Config Read(string path = null, bool createIfMissing = true)
        {
            if (path == null || path == "") path = DefaultConfigFile;

            Config conf;

            if (!File.Exists(path) && createIfMissing)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                conf = new Config();
                File.WriteAllText(path, JsonConvert.SerializeObject(conf));
                return conf;
            }

            using(StreamReader file = File.OpenText(path))
            {
                var js = new JsonSerializer();
                conf = (Config)js.Deserialize(file, typeof(Config));
            }

            return conf;
        }
    }
}
