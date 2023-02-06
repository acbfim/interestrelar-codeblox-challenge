using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Interestrelar.Domain
{
    public class Freighter
    {
        public int Id { get; set; }
        public string Class { get; set; }
        public decimal Capacity { get; set; }
        public List<Mineral> CompatibleMinerals { get; set; }
    }
}