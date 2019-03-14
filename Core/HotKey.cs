using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ChristWare.Core
{
    public class HotKey
    {
        [DllImport("user32.dll")]
        public static extern short VkKeyScanA(char ch);

        public string Key { get; set; }
        public int Value { get; private set; }

        public HotKey(string key)
        {
            if (key.StartsWith("0x"))
            {
                Value = Convert.ToInt32(key, 16);
                return;
            }

            if (key.Length != 1)
                throw new ArgumentException("Hotkey character is NOT a keyboard key!");

            Value = VkKeyScanA(key[0]) & 0x00FF;
            Key = key;
        }

        public HotKey(int rawValue)
        {
            Value = rawValue;
        }

        public HotKey(char value)
        {
            Key = value.ToString();
            Value = VkKeyScanA(value) & 0x00FF;
        }

        public override string ToString() => Key ?? $"0x{Value:X}";
    }
}
