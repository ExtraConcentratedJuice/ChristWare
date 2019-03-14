using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChristWare.Core
{
    public static class Ranks
    {
        private static readonly string[] ranks = new string[]
        {
            "Unranked",
            "Silver I",
            "Silver II",
            "Silver III",
            "Silver IV",
            "Silver Elite",
            "Silver Elite Master",
            "Gold Nova I",
            "Gold Nova II",
            "Gold Nova III",
            "Gold Nova Master",
            "Master Guardian I",
            "Master Guardian II",
            "Master Guardian Elite",
            "Distinguished Master Guardian",
            "Legendary Eagle",
            "Legendary Eagle Master",
            "Supreme Master First Class",
            "The Global Elite"
        };

        public static string GetName(int index)
        {
            if (index >= 0 && index < ranks.Length)
                return ranks[index];

            return "Invalid Rank";
        }
    }
}
