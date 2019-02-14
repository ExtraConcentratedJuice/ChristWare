using ChristWare.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;
using System.IO;
using ChristWare.Core;
using System.Reflection;
using ChristWare.Core.Components;
using System.Runtime.InteropServices;

namespace ChristWare
{
    public class ChristWare
    {
        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);

        [DllImport("user32.dll")]
        public static extern short VkKeyScanA(char ch);

        private readonly IntPtr processHandle;
        private readonly IntPtr clientAddress;
        private readonly ChristConfiguration configuration;
        private readonly List<Component> components;
        private short? pressedKey;

        public ChristWare(string processName)
        {
            if (!ProcessUtility.TryGetProcessHandle("csgo", out processHandle))
                throw new ArgumentException("No CSGO process found.");

            Console.WriteLine("Acquired process handle @ " + processHandle);

            if (!ProcessUtility.TryGetProcessModule("csgo", "client_panorama.dll", out clientAddress))
                throw new ArgumentException("No CSGO client panorama module found.");

            configuration = JsonConvert.DeserializeObject<ChristConfiguration>(File.ReadAllText("christconfig.json"));

            Console.WriteLine("Client module @ " + clientAddress);

            this.components = new List<Component>();

            components.Add(new ESP(processHandle, clientAddress, configuration));
        }

        public void Run()
        {
            while (true)
            {
                if (pressedKey.HasValue && GetAsyncKeyState(pressedKey.Value) == 0)
                    pressedKey = null;

                foreach (var component in components)
                {
                    var currentlyPressedKey = VkKeyScanA(component.Hotkey) & 0x00FF;

                    if (!pressedKey.HasValue
                        && WindowUtility.GetActiveWindowName() == Constants.CSGO_WINDOW 
                        && GetAsyncKeyState(currentlyPressedKey) != 0)
                    {
                        pressedKey = (short?)currentlyPressedKey;

                        if (component.Enabled)
                            component.Disable();
                        else
                            component.Enable();
                    }

                    if (component.Enabled)
                    {
                        component.OnTick();
                    }
                }

                Thread.Sleep(5);
            }
        }
    }
}
