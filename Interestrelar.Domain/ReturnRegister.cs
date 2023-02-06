using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Interestrelar.Domain
{
    public class ReturnRegister
    {
        public int Id { get; set; }
        public int ReturnCargoId { get; set; }
        public ReturnCargo ReturnCargo { get; set; }
        public DateTime CreateAd { get; set; } = DateTime.Now;
        public DateTime ReturneDate { get; set; }
    }
}