using ChristWare.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChristWare.Core
{
    public abstract class Component
    {
        protected readonly IntPtr processHandle;
        protected readonly IntPtr clientAddress;
        protected readonly IntPtr engineAddress;

        protected readonly ConfigurationManager<ChristConfiguration> configuration;

        public abstract string Name { get; }
        public abstract HotKey DefaultHotkey { get; }
        public HotKey Hotkey { get; private set; }

        public bool Enabled { get; protected set; }

        protected virtual void OnDisable() { }
        protected virtual void OnEnable() { }

        public void Disable()
        {
            Enabled = false;
            OnDisable();
            Beeper.Beep(195, 215);
        }

        public void Enable()
        {
            Enabled = true;
            OnEnable();
            Beeper.Beep(783, 215);
        }

        private void OnConfigurationChangedInternal(object s, FileSystemEventArgs args)
        {
            if (configuration.Value.KeyBinds.TryGetValue(Name, out var unparsed) && Hotkey.Key != unparsed)
            {
                Hotkey = new HotKey(unparsed);
            }

            OnConfigurationChanged(s, args);
        }

        protected virtual void OnConfigurationChanged(object s, FileSystemEventArgs args) { }

        public Component(IntPtr processHandle, IntPtr clientAddress, IntPtr engineAddress, ConfigurationManager<ChristConfiguration> configuration)
        {
            this.processHandle = processHandle;
            this.clientAddress = clientAddress;
            this.engineAddress = engineAddress;
            this.configuration = configuration;

            if (!configuration.Value.KeyBinds.TryGetValue(Name, out var unparsed))
            {
                configuration.Value.KeyBinds[Name] = DefaultHotkey.ToString();
                Hotkey = DefaultHotkey;
            }
            else
            {
                Hotkey = new HotKey(unparsed);
            }

            configuration.OnConfigurationChanged += OnConfigurationChangedInternal;
        }
    }
}
