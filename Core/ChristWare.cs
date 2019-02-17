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
        private readonly IntPtr windowHandle;
        private readonly ChristConfiguration configuration;
        private readonly List<Component> components;
        private short? pressedKey;

        public ChristWare(string processName, ChristConfiguration configuration)
        {
            if (!ProcessUtility.TryGetProcessHandle("csgo", out var process, out processHandle))
                throw new ArgumentException("No CSGO process found.");

            this.windowHandle = process.MainWindowHandle;

            Console.WriteLine("Process Handle: " + processHandle);

            if (!ProcessUtility.TryGetProcessModule(process, "client_panorama.dll", out clientAddress))
                throw new ArgumentException("No CSGO client panorama module found.");

            this.configuration = configuration;

            Console.WriteLine($"Client Module: 0x{clientAddress.ToString("x")}");

            ConsoleUtility.WriteLineColor($"{ASCII_ART}\n", ConsoleColor.Yellow, false);

            Console.CursorVisible = false;

            components = new List<Component>();

            components.Add(new ESP(processHandle, clientAddress, configuration));
            components.Add(new Radar(processHandle, clientAddress, configuration));
            components.Add(new TriggerBot(processHandle, clientAddress, configuration));
            components.Add(new BunnyHop(processHandle, clientAddress, configuration));
        }


        public void Run()
        {
            while (true)
            {
                if (!WindowUtility.IsHandleWindow(windowHandle))
                {
                    ExitSequence();
                    Environment.Exit(0);
                }

                Console.SetCursorPosition(0, 10);

                if (pressedKey.HasValue && !KeyUtility.IsKeyDown(pressedKey.Value))
                    pressedKey = null;

                foreach (var component in components)
                {
                    ConsoleUtility.WriteColor($"{component.Name} ({component.Hotkey}): ");
                    ConsoleUtility.WriteLineColor(component.Enabled ? "Enabled" : "Disabled", component.Enabled ? ConsoleColor.Green : ConsoleColor.Red);
                    var currentlyPressedKey = VkKeyScanA(component.Hotkey) & 0x00FF;

                    if (!pressedKey.HasValue
                        && WindowUtility.GetForegroundWindow() == windowHandle
                        && KeyUtility.IsKeyDown(currentlyPressedKey)
                        && (Memory.Read<int>(processHandle, (int)clientAddress + Signatures.dwMouseEnable) ^ (int)clientAddress + Signatures.dwMouseEnablePtr) != 0)
                    {
                        pressedKey = (short?)currentlyPressedKey;

                        if (component.Enabled)
                            component.Disable();
                        else
                            component.Enable();
                    }

                    if (component.Enabled && component is ITickHandler handler)
                        handler.OnTick();
                }

                for (int i = 1; i <= 32; i++)
                {
                    var entity = Memory.Read<int>(processHandle, (int)clientAddress + Signatures.dwEntityList + i * 0x10);

                    if (entity != 0)
                    {
                        foreach (var component in components.Where(x => x.Enabled).OfType<IEntityHandler>())
                        {
                            component.HandleEntity(entity);
                        }
                    }
                }
                
                Thread.Sleep(5);
            }
        }

        public static void ExitSequence()
        {
            Console.Beep(1760, 235);
            Console.Beep(1319, 235);
            Console.Beep(440, 235);
            Console.Beep(494, 235 * 2);
        }

        private const string ASCII_ART = @"
   _____ _          _     ___          __            
  / ____| |        (_)   | \ \        / /             
 | |    | |__  _ __ _ ___| |\ \  /\  / /_ _ _ __ ___ 
 | |    | '_ \| '__| / __| __\ \/  \/ / _` | '__/ _ \
 | |____| | | | |  | \__ \ |_ \  /\  / (_| | | |  __/
  \_____|_| |_|_|  |_|___/\__| \/  \/ \__,_|_|  \___|
";
    }
}
