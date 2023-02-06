using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Interestrelar.Domain
{
    public class ReturnCargo
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public decimal Cargo { get; set; }
        public int MineralId { get; set; }
        public Mineral Mineral { get; set; }
    }
}