﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChristWare.Core
{
    public static class Weapons
    {
        private static readonly int[] singleShots = new int[]
        {
            1, 2, 3, 4, 9, 30, 32, 36, 40, 61, 63, 64
        };

        public static bool IsSingleShot(int id) => singleShots.Contains(id);

        public static string GetName(int id)
        {
            switch (id)
            {
                case 0: return "None";
                case 1: return "Desert Eagle";
                case 2: return "Dual Berettas";
                case 3: return "Five-SeveN";
                case 4: return "Glock-18";
                case 7: return "AK-47";
                case 8: return "AUG";
                case 9: return "AWP";
                case 10: return "FAMAS";
                case 11: return "G3SG1";
                case 13: return "Galil AR";
                case 14: return "M249";
                case 16: return "M4A4";
                case 17: return "MAC-10";
                case 19: return "P90";
                case 23: return "MP5-SD";
                case 24: return "UMP-45";
                case 25: return "XM1014";
                case 26: return "PP-Bizon";
                case 27: return "MAG-7";
                case 28: return "Negev";
                case 29: return "Sawed-Off";
                case 30: return "Tec-9";
                case 31: return "Zeus x27";
                case 32: return "P2000";
                case 33: return "MP7";
                case 34: return "MP9";
                case 35: return "Nova";
                case 36: return "P250";
                case 38: return "SCAR-20";
                case 39: return "SG 553";
                case 40: return "SSG 08";
                case 41: return "Knife";
                case 42: return "Knife";
                case 43: return "Flashbang";
                case 44: return "HE";
                case 45: return "Smoke Grenade";
                case 46: return "Molotov";
                case 47: return "Decoy Grenade";
                case 48: return "Incendiary";
                case 49: return "C4 Explosive";
                case 50: return "Kevlar Vest";
                case 51: return "Kevlar and Helmet";
                case 52: return "Heavy Assault Suit";
                case 54: return "Night Vision";
                case 55: return "Defuse Kit";
                case 56: return "Rescue Kit";
                case 57: return "Medi-Shot";
                case 58: return "Music Kit";
                case 59: return "Knife";
                case 60: return "M4A1-S";
                case 61: return "USP-S";
                case 63: return "CZ75-Auto";
                case 64: return "R8 Revolver";
                case 68: return "Tactical Awareness Grenade";
                case 69: return "Bare Hands";
                case 70: return "Breach Charge";
                case 72: return "Tablet";
                case 75: return "Axe";
                case 76: return "Hammer";
                case 78: return "Wrench";
                case 80: return "Spectral Shiv";
                case 81: return "Fire Bomb";
                case 82: return "Diversion Device";
                case 83: return "Frag Grenade";
                case 84: return "Snowball";
                case 5027: return "Bloodhound Gloves";
                case 5028: return "Default T Gloves";
                case 5029: return "Default CT Gloves";
                case 5030: return "Sport Gloves";
                case 5031: return "Driver Gloves";
                case 5032: return "Hand Wraps";
                case 5033: return "Moto Gloves";
                case 5034: return "Specialist Gloves";
                case 5035: return "Hydra Gloves";
                case 262205: return "USP-S";
                case 262145: return "Five-Seven";
                case 262204: return "Silent M4";
                case 589833: return "AWP";
                default: return "Invalid";
            };
        }
    }
}
