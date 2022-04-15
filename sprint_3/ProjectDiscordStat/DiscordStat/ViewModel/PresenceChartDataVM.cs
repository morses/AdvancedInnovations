using System.Collections.Generic;
using DiscordStats.Models;
using DiscordStats.DAL.Abstract;
using DiscordStats.ViewModels;
using System.Collections;
using System.Linq;

namespace DiscordStats.ViewModel
{
    public class PresenceChartDataVM
    {
        public Dictionary<string, int>? AllPresenceNameListAndCount(List<Presence> presences)
        {           
            Dictionary<string, int> keyValueCounts = presences
            .GroupBy(x => x.Name)
            .ToDictionary(g => g.Key, g => g.Count());

            //var sortedKeyValueCounts = from entry in keyValueCounts orderby entry.Value descending select entry;

            return keyValueCounts;
        }
    }
}
