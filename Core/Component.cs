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
        protected readonly ChristConfiguration configuration;

        public abstract string Name { get; }
        public abstract char Hotkey { get; }

        public bool Enabled { get; protected set; }

        public abstract void OnTick();
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

        public Component(IntPtr processHandle, IntPtr clientAddress, ChristConfiguration configuration)
        {
            this.processHandle = processHandle;
            this.clientAddress = clientAddress;
            this.configuration = configuration;
        }
    }
}
