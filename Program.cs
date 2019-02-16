using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChristWare
{
    public class Program
    {
        
        static void Main(string[] args)
        {
            var configuration = JsonConvert.DeserializeObject<ChristConfiguration>(File.ReadAllText("christconfig.json"));
            new ChristWare("csgo", configuration).Run();
        }
    }
}
