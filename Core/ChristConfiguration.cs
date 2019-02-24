﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChristWare
{
    public class ChristConfiguration
    {
        [DefaultValue(255)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public byte EnemyR { get; set; }

        public byte EnemyG { get; set; }

        public byte EnemyB { get; set; }


        public byte FriendlyR { get; set; }

        [DefaultValue(255)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public byte FriendlyG { get; set; }

        public byte FriendlyB { get; set; }

        [DefaultValue(255)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public byte EnemyChamsR { get; set; }

        public byte EnemyChamsG { get; set; }

        public byte EnemyChamsB { get; set; }


        public byte FriendlyChamsR { get; set; }

        [DefaultValue(255)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public byte FriendlyChamsG { get; set; }

        public byte FriendlyChamsB { get; set; }

        [DefaultValue(3.5)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public float ChamsBrightness { get; set; }

        [DefaultValue("0x05")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public string TriggerBotHoldKey { get; set; }

        [DefaultValue("0x04")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public string BunnyHopHoldKey { get; set; }

        public Dictionary<string, string> KeyBinds { get; set; }
    }
}
