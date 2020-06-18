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
using System.Diagnostics;
using System.Drawing;


namespace ChristWare
{
    public class ChristWare
    {
        private const string VERSION = "v1.5.1";

        private readonly IntPtr processHandle;
        private readonly IntPtr clientAddress;
        private readonly IntPtr engineAddress;
        private readonly IntPtr csgoWindowHandle;
        private readonly ConfigurationManager<ChristConfiguration> configuration;
        private readonly List<Component> components;
        private readonly Timer writeTimer;
        private short? pressedKey;

        public ChristWare(string processName, ConfigurationManager<ChristConfiguration> configuration)
        {
            if (!ProcessUtility.TryGetProcessHandle("csgo", out var process, out processHandle))
                throw new ArgumentException("No CSGO process found.");

            this.csgoWindowHandle = process.MainWindowHandle;

            if (!ProcessUtility.TryGetProcessModule(process, "client.dll", out var clientModule))
                throw new ArgumentException("No CSGO client panorama module found.");

            clientAddress = clientModule.BaseAddress;

            if (!ProcessUtility.TryGetProcessModule(process, "engine.dll", out var engineModule))
                throw new ArgumentException("No CSGO engine module found.");

            engineAddress = engineModule.BaseAddress;

            Console.WriteLine("Process Handle: " + processHandle);
            Console.WriteLine($"Client Module: 0x{clientAddress.ToString("x")}");

            Signatures.Initialize(processHandle, clientModule, engineModule);
            Netvars.Initialize(processHandle, clientModule);

            ConsoleUtility.WriteLineColor($"{ASCII_ART}\n", ConsoleColor.Yellow);
            Console.CursorVisible = false;


            this.configuration = configuration;

            components = new List<Component>
            {
                new ESP(processHandle, clientAddress, engineAddress, configuration),
                new Radar(processHandle, clientAddress, engineAddress, configuration),
                new TriggerBot(processHandle, clientAddress, engineAddress, configuration),
                new BunnyHop(processHandle, clientAddress, engineAddress, configuration),
                new AntiFlash(processHandle, clientAddress, engineAddress, configuration),
                new Aimbot(processHandle, clientAddress, engineAddress, configuration),
                new RecoilControl(processHandle, clientAddress, engineAddress, configuration),
                new Chams(processHandle, clientAddress, engineAddress, configuration),
                new TagChanger(processHandle, clientAddress, engineAddress, configuration),
                new ThirdPerson(processHandle, clientAddress, engineAddress, configuration),
            };

            // Update configuration
            configuration.WriteToFile();

            writeTimer = new Timer((_) => UpdateConsole(), null, 0, (int)TimeSpan.FromSeconds(1.5).TotalMilliseconds);
        }

        public void UpdateConsole()
        {
            Console.Clear();
            ConsoleUtility.WriteLineColor(ASCII_ART + $"\n\n\tChristWare {VERSION}\n", ConsoleColor.Yellow);

            foreach (var component in components)
            {
                ConsoleUtility.WriteColor($"{component.Name} ({component.Hotkey}): ");
                ConsoleUtility.WriteLineColor(component.Enabled ? "Enabled" : "Disabled", component.Enabled ? ConsoleColor.Green : ConsoleColor.Red);
            }

            var clientState = Memory.Read<int>(processHandle, (int)engineAddress + Signatures.dwClientState);
            var flags = Memory.Read<int>(processHandle, clientState + Signatures.dwClientState_State);
            var inGame = flags == (int)SignOnState.IN_GAME;

            Console.WriteLine();

            ConsoleUtility.WriteColor("Status: ");
            ConsoleUtility.WriteLineColor(inGame ? "IN-GAME" : "NOT IN-GAME", inGame ? ConsoleColor.Green : ConsoleColor.Red);

            Console.WriteLine();

            if (!inGame)
                return;

            // should probably update to map to entity struct but oh well
            var friendly = new List<string>();
            var enemy = new List<string>();

            var localPlayer = Memory.Read<int>(processHandle, (int)clientAddress + Signatures.dwLocalPlayer);
            var teamId = Memory.Read<int>(processHandle, localPlayer + Netvars.m_iTeamNum);
            var playerResources = Memory.Read<int>(processHandle, (int)clientAddress + Signatures.dwPlayerResource);

            // I don't even know what I am doing here, somebody please help me.
            // This is supposed to get me the PlayerInfo array.
            var addr = Memory.Read<int>(processHandle, 
                Memory.Read<int>(processHandle, 
                    Memory.Read<int>(processHandle, clientState + Signatures.dwClientState_PlayerInfo)
                    + 0x40)
                + 0x0C);            

            for (int i = 1; i <= 32; i++)
            {
                var entity = Memory.Read<int>(processHandle, (int)clientAddress + Signatures.dwEntityList + i * 0x10);

                if (entity != 0)
                {
                    var otherTeamId = Memory.Read<int>(processHandle, entity + Netvars.m_iTeamNum);

                    var rank = Memory.Read<int>(processHandle, playerResources + Netvars.m_iCompetitiveRanking + (i - 0x1) * 0x4);
                    var hp = Memory.Read<int>(processHandle, entity + Netvars.m_iHealth);

                    // Read playerinfo struct of index i then add offset of name
                    var name = Memory.ReadString(processHandle, 
                            Memory.Read<int>(processHandle, addr + 0x28 + i * 0x34) + 0x10,
                        128, Encoding.UTF8);

                    var weaponIndex = Memory.Read<int>(processHandle, entity + Netvars.m_hActiveWeapon) & 0xFFF;
                    var gun = Memory.Read<int>(processHandle, (int)clientAddress + Signatures.dwEntityList + (weaponIndex - 0x1) * 0x10);
                    var gunId = Memory.Read<int>(processHandle, gun + Netvars.m_iItemDefinitionIndex);

                    List<string> list;

                    if (otherTeamId == 2 || otherTeamId == 3)
                        list = otherTeamId == teamId ? friendly : enemy;
                    else
                        list = friendly;
                    
                    list.Add($"{name.Trim()}\nHP: {hp,-3} | Weapon: {Weapons.GetName(gunId),-16} | Rank: {Ranks.GetName(rank)}\n");
                }
            }
            
            Console.WriteLine();
            ConsoleUtility.WriteLineColor("Friendly", ConsoleColor.Green);
            foreach (var x in friendly)
                ConsoleUtility.WriteLineColor(x);

            Console.WriteLine();
            ConsoleUtility.WriteLineColor("Enemy", ConsoleColor.Red);
            foreach (var x in enemy)
                ConsoleUtility.WriteLineColor(x);
        }

        public void Run()
        {
            while (true)
            {
                if (!WindowUtility.IsHandleWindow(csgoWindowHandle))
                {
                    ExitSequence();
                    Environment.Exit(0);
                }


                if (pressedKey.HasValue && !KeyUtility.IsKeyDown(pressedKey.Value))
                    pressedKey = null;

                var foregroundWindow = WindowUtility.GetForegroundWindow();

                var consoleMenuKey = new HotKey(configuration.Value.ConsoleMenuKey);

                if ((foregroundWindow == csgoWindowHandle || foregroundWindow == Process.GetCurrentProcess().MainWindowHandle)
                    && KeyUtility.IsKeyDown(consoleMenuKey.Value)
                    && !pressedKey.HasValue)
                {
                    pressedKey = (short)consoleMenuKey.Value;
                    if (foregroundWindow == csgoWindowHandle)
                        WindowUtility.SetTopWindow(Process.GetCurrentProcess().MainWindowHandle);
                    else
                        WindowUtility.SetTopWindow(csgoWindowHandle);
                }

                var clientState = Memory.Read<int>(processHandle, (int)engineAddress + Signatures.dwClientState);
                var flags = Memory.Read<int>(processHandle, clientState + Signatures.dwClientState_State);
                var inGame = flags == (int)SignOnState.IN_GAME;

                if (!inGame)
                {
                    Thread.Sleep(5);
                    continue;
                }

                foreach (var component in components)
                {
                    var currentHotkey = component.Hotkey.Value;

                    if (!pressedKey.HasValue
                        && foregroundWindow == csgoWindowHandle
                        && KeyUtility.IsKeyDown(currentHotkey)
                        && (Memory.Read<int>(processHandle, (int)clientAddress + Signatures.dwMouseEnable) ^ (int)clientAddress + Signatures.dwMouseEnablePtr) != 0)
                    {
                        pressedKey = (short?)currentHotkey;

                        if (component.Enabled)
                            component.Disable();
                        else
                            component.Enable();
                    }

                    if (component.Enabled && component is ITickHandler tickHandler)
                        tickHandler.OnTick();

                    if (component is IGuiHandler guiHandler)
                        guiHandler.OnGui();
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
