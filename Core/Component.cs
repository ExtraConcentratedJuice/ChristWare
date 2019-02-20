using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChristWare.Core
{
    public abstract class Component
    {
        protected readonly IntPtr processHandle;
        protected readonly IntPtr clientAddress;
        protected readonly IntPtr engineAddress;

        protected readonly ChristConfiguration configuration;

        public abstract string Name { get; }
        public abstract HotKey DefaultHotkey { get; }
        public HotKey Hotkey { get; }

        public bool Enabled { get; protected set; }

        protected abstract void OnDisable();
        protected abstract void OnEnable();

        public void Disable()
        {
            Enabled = false;
            OnDisable();
        }

        public void Enable()
        {
            Enabled = true;
            OnEnable();
        }

        public Component(IntPtr processHandle, IntPtr clientAddress, IntPtr engineAddress, ChristConfiguration configuration)
        {
            this.processHandle = processHandle;
            this.clientAddress = clientAddress;
            this.engineAddress = engineAddress;
            this.configuration = configuration;

            if (!configuration.KeyBinds.TryGetValue(Name, out var unparsed))
            {
                configuration.KeyBinds[Name] = DefaultHotkey.ToString();
                Hotkey = DefaultHotkey;
            }
            else
            {
                Hotkey = new HotKey(unparsed);
            }
        }
    }
}
