using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCoreRollback.Models
{
    public class Wallet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Money { get; set; }
    }
}
