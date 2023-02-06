using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Interestrelar.Domain
{
    public class FreightersAvailable
    {
        public int Id { get; set; }
        public Freighter Freighter { get; set; }
        public int Amount { get; set; }
        
        
    }
}