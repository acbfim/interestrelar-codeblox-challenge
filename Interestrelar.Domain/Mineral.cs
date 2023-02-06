using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Interestrelar.Domain
{
    public class Mineral
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Feature { get; set; }
        public decimal Price { get; set; }
    }
}