using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadGen.Core.Store
{
    public class StateStoreOptions
    {
        public string? TokenStore { get; set; }
        public string? RedisHost { get; set; }
        public string? AuditStore { get; set; }
        public string? MonitorStore { get; set; }
        public string? ErrorStore { get; set; }
    }
}
