using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoTask.Service
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}
