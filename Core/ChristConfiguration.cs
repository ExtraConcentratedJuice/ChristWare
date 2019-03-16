using ChristWare.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChristWare
{
    // These Obfuscation attributes are present for compatability with ConfuserEX.
    // Sadly, ConfuserEX does not respect the "ApplyToMembers" field, so the attribute must be present on all members.
    [Obfuscation(Exclude = false, Feature = "-rename")]
    public class ChristConfiguration
    {
        [DefaultValue("0x2D")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        [Obfuscation(Exclude = false, Feature = "-rename")]
        public string ConsoleMenuKey { get; set; }

        [Obfuscation(Exclude = false, Feature = "-rename")]
        public string ClanTag { get; set; }

        [DefaultValue(true)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        [Obfuscation(Exclude = false, Feature = "-rename")]
        public bool RotateClanTag { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        [Obfuscation(Exclude = false, Feature = "-rename")]
        public RGBColor EnemyColor { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        [Obfuscation(Exclude = false, Feature = "-rename")]
        public RGBColor FriendlyColor { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        [Obfuscation(Exclude = false, Feature = "-rename")]
        public RGBColor EnemyChamsColor { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        [Obfuscation(Exclude = false, Feature = "-rename")]
        public RGBColor FriendlyChamsColor { get; set; }

        [DefaultValue(3.5)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        [Obfuscation(Exclude = false, Feature = "-rename")]
        public float ChamsBrightness { get; set; }

        [DefaultValue("0x05")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        [Obfuscation(Exclude = false, Feature = "-rename")]
        public string TriggerBotHoldKey { get; set; }

        [DefaultValue("0x04")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        [Obfuscation(Exclude = false, Feature = "-rename")]
        public string BunnyHopHoldKey { get; set; }

        [DefaultValue(true)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        [Obfuscation(Exclude = false, Feature = "-rename")]
        public bool UseHoldKeyForAimbot { get; set; }

        [DefaultValue("0x12")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        [Obfuscation(Exclude = false, Feature = "-rename")]
        public string AimbotHoldKey { get; set; }

        [DefaultValue(14)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        [Obfuscation(Exclude = false, Feature = "-rename")]
        public int AimbotFOV { get; set; }

        [DefaultValue(true)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        [Obfuscation(Exclude = false, Feature = "-rename")]
        public bool RecoilControlOnAimbot { get; set; }

        [DefaultValue(true)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        [Obfuscation(Exclude = false, Feature = "-rename")]
        public bool SmoothAim { get; set; }

        [DefaultValue(true)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        [Obfuscation(Exclude = false, Feature = "-rename")]
        public bool LockTargetOnAimbotWhileShooting { get; set; }

        [DefaultValue(4.5F)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        [Obfuscation(Exclude = false, Feature = "-rename")]
        public float AimSmoothingFactor { get; set; }

        [DefaultValue(40)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        [Obfuscation(Exclude = false, Feature = "-rename")]
        public int TriggerBotDelayLowerBoundMilliseconds { get; set; }

        [DefaultValue(80)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        [Obfuscation(Exclude = false, Feature = "-rename")]
        public int TriggerBotDelayUpperBoundMilliseconds { get; set; }

        [Obfuscation(Exclude = false, Feature = "-rename")]
        public Dictionary<string, string> KeyBinds { get; set; }
    }
}
