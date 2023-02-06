using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interestrelar.Domain.Request;

namespace Interestrelar.Domain
{
    public class Dashboard
    {
        public List<ByMineral> TotalByMineral { get; set; }
        public List<ByFreighter> TotalByFreighter { get; set; }



        public record ByMineral 
        {
            public Mineral Mineral { get; set; }
            public decimal Amount { get; set; }
        }

        public record ByFreighter 
        {
            public int TotalUsed { get; set; }
            public int TotalIdle { get; set; }
            public Freighter Freighter { get; set; }
        }
    }
}