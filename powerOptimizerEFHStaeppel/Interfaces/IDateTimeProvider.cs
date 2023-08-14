using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace powerOptimizerEFHStaeppel.Interfaces
{
    public interface IDateTimeProvider
    {
        DateTime DateTimeNow { get; }
    }
}
