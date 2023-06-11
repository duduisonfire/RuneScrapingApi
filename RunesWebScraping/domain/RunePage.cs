using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RunesWebScraping.domain.interfaces;

namespace RunesWebScraping.domain
{
    public class RunePage : IRunePage
    {
        public int primaryStyleId { get; }
        public int subStyleId { get; }
        public List<int> selectedPerksIds { get; }

        public RunePage(List<string> runes)
        {
            selectedPerksIds = new();
            primaryStyleId = RunesParseTable.table[runes[0]];
            subStyleId = RunesParseTable.table[runes[1]];

            for (int i = 2; i < 11; i++)
            {
                selectedPerksIds.Add(RunesParseTable.table[runes[i]]);
            }
        }
    }
}
