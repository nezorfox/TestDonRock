using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioDonRock.DonRock
{
    public class MovDiaria
    {
        public string Item { get; set; }
        public DateTime Data_do_Lancamento { get; set; }
        public decimal Lancamentos_Entrada_Quantidade { get; set; }
        public decimal Lancamentos_Entrada_Valor { get; set; }
        public decimal Lancamentos_Saida_Quantidade { get; set; }
        public decimal Lancamentos_Saida_Valor { get; set; }
        public decimal Saldo_Inicial_Quantidade { get; set; }
        public decimal Saldo_Inicial_Valor { get; set; }
        public decimal Saldo_Final_Quantidade { get; set; }
        public decimal Saldo_Final_Valor { get; set; }
    }
}
