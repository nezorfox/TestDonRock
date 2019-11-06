using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioDonRock.DonRock
{
    public class MovToItem
    {
        public string Item { get; set; }
        public bool IsEntrada { get; set; }
        public DateTime DataLancamento { get; set; }
        public decimal Quantidade { get; set; }
        public decimal Valor { get; set; }
    }
}
