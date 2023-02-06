using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Interestrelar.Domain
{
    public class ExitRegister
    {
        public int Id { get; set; }
        public int FreighterId { get; set; }
        public Freighter Freighter { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ExitDate { get; set; }
        public int? ReturnRegisterId { get; set; }
        public ReturnRegister? ReturnRegister { get; set; }


    }
}