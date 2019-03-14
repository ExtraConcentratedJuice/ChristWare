using ChristWare.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChristWare.Core
{
    public class ConfigurationManager<T>
    {
        private readonly string path;
        private readonly string fileName;

        public event FileSystemEventHandler OnConfigurationChanged;
        private readonly object configurationLock = new object();
        private readonly FileSystemWatcher watcher;
        private T value;

        public T Value
        {
            get
            {
                lock (configurationLock)
                    return value;
            }
        }

        private void OnFileChanged(object s, FileSystemEventArgs args)
        {
            if (args.Name != fileName || args.ChangeType != WatcherChangeTypes.Changed)
                return;

            FileUtility.WaitForFile(args.FullPath);

            ReadFromFile();

            OnConfigurationChanged?.Invoke(s, args);
        }

        public void ReadFromFile()
        {
            lock (configurationLock)
            {
                using (var stream = File.Open(path, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        value = JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
                    }
                }
            }
        }

        public void WriteToFile()
        {
            lock (configurationLock)
            {
                using (var stream = File.Open(path, FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(JsonConvert.SerializeObject(Value, Formatting.Indented));
                        writer.Flush();
                    }
                }
            }
        }

        public ConfigurationManager(string relativePath)
        {
            this.path = Directory.GetCurrentDirectory() + "\\" + relativePath;
            this.fileName = Path.GetFileName(path);
            this.watcher = new FileSystemWatcher(Path.GetDirectoryName(path));
            watcher.EnableRaisingEvents = true;
            watcher.Changed += OnFileChanged;

            ReadFromFile();
        }
    }
}
