using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Interestrelar.Domain
{
    public class FreightersMinerals
    {
        public int FreighterId { get; set; }
        public Freighter? Freighter { get; set; }
        public int MineralId { get; set; }
        public Mineral? Mineral { get; set; }
    }
}