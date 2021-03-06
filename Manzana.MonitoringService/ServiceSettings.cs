using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manzana.MonitoringService
{
    public class ServiceSettings
    {
        public string NewFilePath { get; set; }
        public string GarbageFilePath { get; set; }
        public string CompleteFilePath { get; set; }
        public string WcfServiceUrl { get; set; }
    }
}
