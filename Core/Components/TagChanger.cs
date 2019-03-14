using ChristWare.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChristWare.Core.Components
{
    public class TagChanger : Component, ITickHandler
    {
        public override string Name => "TagChanger";

        public override HotKey DefaultHotkey => new HotKey('.');

        private readonly byte[] shellcode =
        {
            0xB9, /* 1 */ 0x0, 0x0, 0x0, 0x0,
            0xBA, /* 6 */ 0x0, 0x0, 0x0, 0x0,
            0xB8, /* 11 */ 0x0, 0x0, 0x0, 0x0,
            0xFF, 0xD0,
            0xC3,
            /* 18 */ 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        };

        private IntPtr shellcodeAddress;
        private readonly TagManager tagManager;
        private DateTime lastChanged = DateTime.Now;

        public TagChanger(IntPtr processHandle, IntPtr clientAddress, IntPtr engineAddress, ConfigurationManager<ChristConfiguration> configuration)
            : base(processHandle, clientAddress, engineAddress, configuration)
        {
            this.tagManager = new TagManager(configuration.Value.ClanTag);
        }

        protected override void OnConfigurationChanged(object s, FileSystemEventArgs args)
        {
            if (configuration.Value.ClanTag != tagManager.Tag)
                tagManager.Tag = configuration.Value.ClanTag;

            ChangeTag(tagManager.Tag);
            lastChanged = DateTime.Now;
        }

        public void OnTick()
        {
            if (!configuration.Value.RotateClanTag || (DateTime.Now - lastChanged).TotalSeconds < 1)
                return;

            lastChanged = DateTime.Now;
            ThreadPool.QueueUserWorkItem(_ => ChangeTag(tagManager.ShiftOne()), null);
        }

        protected override void OnDisable()
        {
            ChangeTag(Encoding.UTF8.GetString(Enumerable.Repeat<byte>(0x0, 15).ToArray()));
        }

        private void ChangeTag(string tag)
        {
            var tagBytes = Encoding.UTF8.GetBytes(tag + '\0');

            if (tag.Length > 16)
                throw new ArgumentException("A tag length of 15 or less is expected.", nameof(tag));

            if (shellcodeAddress == IntPtr.Zero)
            {
                shellcodeAddress = ExternalMemoryUtility.Allocate(processHandle, IntPtr.Zero, (uint)shellcode.Length);
            }

            var executedShellcode = new byte[shellcode.Length];
            Array.Copy(shellcode, 0, executedShellcode, 0, shellcode.Length);

            var addressBytes = BitConverter.GetBytes((int)(shellcodeAddress + 18));
            var changeTagAddress = BitConverter.GetBytes((int)(engineAddress + Signatures.dwSetClanTag));

            Array.Copy(addressBytes, 0, executedShellcode, 1, 4);
            Array.Copy(addressBytes, 0, executedShellcode, 6, 4);
            Array.Copy(changeTagAddress, 0, executedShellcode, 11, 4);
            Array.Copy(Enumerable.Repeat<byte>(0x0, 16).ToArray(), 0, executedShellcode, 18, 16);
            Array.Copy(tagBytes, 0, executedShellcode, 18, tagBytes.Length);

            ExternalMemoryUtility.ExecuteShellcode(processHandle, shellcodeAddress, executedShellcode);
        }

        protected override void OnEnable()
        {
            ChangeTag(tagManager.Tag);
            lastChanged = DateTime.Now;
        }
    }
}
