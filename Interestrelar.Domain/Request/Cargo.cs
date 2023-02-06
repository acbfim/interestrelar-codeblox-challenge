using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Interestrelar.Domain.Request;

public class Cargo
{
    public int Year { get; set; }
    public int Month { get; set; }
    public int Week { get; set; }
    public decimal A { get; set; }
    public decimal B { get; set; }
    public decimal C { get; set; }
    public decimal D { get; set; }
    public List<ExitRegister> Exits { get; set; }
    public Dashboard Dashboard { get; set; }
}
