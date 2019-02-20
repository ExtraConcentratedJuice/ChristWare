using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChristWare.Core
{
    public class ConfigurationManager<T>
    {
        private readonly string path;
        public T Configuration { get; private set; }

        public void Read()
        {
            Configuration = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
        }

        public void Write()
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(Configuration, Formatting.Indented));
        }

        public ConfigurationManager(string path)
        {
            this.path = path;
            Read();
        }
    }
}
