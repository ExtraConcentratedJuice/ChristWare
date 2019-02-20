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

        private readonly IntPtr processHandle;
        private readonly IntPtr clientAddress;
        private readonly IntPtr engineAddress;

        private readonly IntPtr windowHandle;
        private readonly ConfigurationManager<ChristConfiguration> configuration;
        private readonly List<Component> components;
        private readonly Timer writeTimer;
        private short? pressedKey;

        public ChristWare(string processName, ConfigurationManager<ChristConfiguration> configuration)
        {
            if (!ProcessUtility.TryGetProcessHandle("csgo", out var process, out processHandle))
                throw new ArgumentException("No CSGO process found.");

            this.windowHandle = process.MainWindowHandle;

            if (!ProcessUtility.TryGetProcessModule(process, "client_panorama.dll", out clientAddress))
                throw new ArgumentException("No CSGO client panorama module found.");

            if (!ProcessUtility.TryGetProcessModule(process, "engine.dll", out engineAddress))
                throw new ArgumentException("No CSGO engine module found.");

            Console.WriteLine("Process Handle: " + processHandle);
            Console.WriteLine($"Client Module: 0x{clientAddress.ToString("x")}");
            ConsoleUtility.WriteLineColor($"{ASCII_ART}\n", ConsoleColor.Yellow);
            Console.CursorVisible = false;

            this.configuration = configuration;

            components = new List<Component>
            {
                new ESP(processHandle, clientAddress, engineAddress, configuration.Configuration),
                new Radar(processHandle, clientAddress, engineAddress, configuration.Configuration),
                new TriggerBot(processHandle, clientAddress, engineAddress, configuration.Configuration),
                new BunnyHop(processHandle, clientAddress, engineAddress, configuration.Configuration),
                new AntiFlash(processHandle, clientAddress, engineAddress, configuration.Configuration),
                new RecoilControl(processHandle, clientAddress, engineAddress, configuration.Configuration)
            };

            // Update configuration
            configuration.Write();

            writeTimer = new Timer((_) => UpdateConsole(), null, 0, (int)TimeSpan.FromSeconds(1.5).TotalMilliseconds);
        }

        public void UpdateConsole()
        {
            Console.Clear();
            ConsoleUtility.WriteLineColor(ASCII_ART + '\n', ConsoleColor.Yellow);
            foreach (var component in components)
            {
                ConsoleUtility.WriteColor($"{component.Name} ({component.Hotkey}): ");
                ConsoleUtility.WriteLineColor(component.Enabled ? "Enabled" : "Disabled", component.Enabled ? ConsoleColor.Green : ConsoleColor.Red);
            }
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

                if (pressedKey.HasValue && !KeyUtility.IsKeyDown(pressedKey.Value))
                    pressedKey = null;

                foreach (var component in components)
                {
                    var currentlyPressedKey = component.Hotkey.Value;

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

    I have the strength to face all conditions
        by the power that Christ gives me.
";
    }
}
