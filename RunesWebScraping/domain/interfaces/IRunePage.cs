using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RunesWebScraping.domain.interfaces
{
    public interface IRunePage
    {
        int primaryStyleId { get; }
        int subStyleId { get; }
        List<int> selectedPerksIds { get; }
    }
}
