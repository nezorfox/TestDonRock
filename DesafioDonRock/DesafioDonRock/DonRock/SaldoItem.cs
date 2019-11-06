using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioDonRock.DonRock
{
    public class SaldoItem
    {
        public string Item { get; set; }
        public DateTime DataInicio { get; set; }
        public decimal QtdInicio { get; set; }
        public decimal ValorInicio { get; set; }
        public DateTime DataFinal { get; set; }
        public decimal QtdFinal { get; set; }
        public decimal ValorFinal { get; set; }

    }
}
