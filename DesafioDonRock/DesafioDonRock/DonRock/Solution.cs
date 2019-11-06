using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace DesafioDonRock.DonRock
{
    public class Solution
    {
        public void Start()
        {
            var listSaldoItem = GetSaldoItem();
            var listMovItem = GetMovItem();
            var listMovDiario = GetMovDiario(listSaldoItem, listMovItem);

            var csvFile = ListToCsv(listMovDiario);

            using (TextWriter tw = File.CreateText(@"C:\Users\farantes\Downloads\DonRock\result.csv"))
            {
                foreach (var line in csvFile)
                {
                    tw.WriteLine(line);
                }
            }
        }

        #region IMPORT

        private List<SaldoItem> GetSaldoItem()
        {
            var listSaldoItem = new List<SaldoItem>();

            using (var reader = new StreamReader(@"C:\Users\farantes\Downloads\DonRock\SaldoITEM.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    if (values[0] == "item")
                        continue;

                    var item = new SaldoItem();
                    item.Item = values[0];
                    item.DataInicio = GetDate(values[1]);
                    item.QtdInicio = GetDecimal(values[2]);
                    item.ValorInicio = GetDecimal(values[3]);
                    item.DataFinal = GetDate(values[4]);
                    item.QtdFinal = GetDecimal(values[5]);
                    item.ValorFinal = GetDecimal(values[6]);

                    listSaldoItem.Add(item);
                }
            }

            return listSaldoItem;
        }

        private List<MovToItem> GetMovItem()
        {
            var listMovItem = new List<MovToItem>();

            using (var reader = new StreamReader(@"C:\Users\farantes\Downloads\DonRock\MovtoITEM_csv.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    if (values[0] == "item")
                        continue;

                    var item = new MovToItem();
                    item.Item = values[0];
                    item.IsEntrada = values[1] == "Ent";
                    item.DataLancamento = GetDate(values[2]);
                    item.Quantidade = GetDecimal(values[3]);
                    item.Valor = GetDecimal(values[4]);

                    listMovItem.Add(item);
                }
            }

            return listMovItem;
        }

        private DateTime GetDate(string value)
        {
            try
            {
                var date = value.Split('/');

                return new DateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[1]), Convert.ToInt32(date[0]), 0, 0, 0);
            }
            catch (Exception ex)
            {
                return DateTime.Now;
            }
        }

        private decimal GetDecimal(string value)
        {
            try
            {
                return Convert.ToDecimal(value);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        #endregion

        private List<MovDiaria> GetMovDiario(List<SaldoItem> listSaldoItem, List<MovToItem> listMovItem)
        {
            var listMovDiario = new List<MovDiaria>();

            foreach (var item in listSaldoItem)
            {
                var listAllMovs = listMovItem.Where(i => i.Item == item.Item
                                                && i.DataLancamento >= item.DataInicio
                                                && i.DataLancamento <= item.DataFinal).ToList();

                var listMovsGroup = listAllMovs.GroupBy(i => new { i.Item, i.DataLancamento })
                                                .Select(i => new
                                                {
                                                    item = i.Key.Item,
                                                    data = i.Key.DataLancamento,
                                                    quantidadeEntrada = i.Where(j => j.IsEntrada).Sum(l => l.Quantidade),
                                                    valorEntrada = i.Where(j => j.IsEntrada).Sum(l => l.Valor),
                                                    quantidadeSaida = i.Where(j => !j.IsEntrada).Sum(l => l.Quantidade),
                                                    valorSaida = i.Where(j => !j.IsEntrada).Sum(l => l.Valor),
                                                })
                                               .ToList();

                var saldoInicialQuantidade = item.QtdInicio;
                var saldoInicialValor = item.ValorInicio;

                foreach (var mov in listMovsGroup)
                {
                    var movDiario = new MovDiaria();
                    movDiario.Item = mov.item;
                    movDiario.Data_do_Lancamento = mov.data;
                    movDiario.Lancamentos_Entrada_Quantidade = mov.quantidadeEntrada;
                    movDiario.Lancamentos_Entrada_Valor = mov.valorEntrada;
                    movDiario.Lancamentos_Saida_Quantidade = mov.quantidadeSaida;
                    movDiario.Lancamentos_Saida_Valor = mov.valorSaida;
                    movDiario.Saldo_Inicial_Quantidade = saldoInicialQuantidade;
                    movDiario.Saldo_Inicial_Valor = saldoInicialValor;
                    movDiario.Saldo_Final_Quantidade = saldoInicialQuantidade + mov.quantidadeEntrada - mov.quantidadeSaida;
                    movDiario.Saldo_Final_Valor = saldoInicialValor + mov.valorEntrada - mov.valorSaida;

                    saldoInicialQuantidade = movDiario.Saldo_Final_Quantidade;
                    saldoInicialValor = movDiario.Saldo_Final_Valor;
                    listMovDiario.Add(movDiario);
                }
            }

            return listMovDiario;
        }

        public static IEnumerable<string> ListToCsv<T>(IEnumerable<T> objectlist, string separator = ";", bool header = true)
        {
            FieldInfo[] fields = typeof(T).GetFields();
            PropertyInfo[] properties = typeof(T).GetProperties();
            if (header)
            {
                yield return String.Join(separator, fields.Select(f => f.Name).Concat(properties.Select(p => p.Name)).ToArray());
            }
            foreach (var o in objectlist)
            {
                yield return string.Join(separator, fields.Select(f => (f.GetValue(o) ?? "").ToString())
                    .Concat(properties.Select(p => (p.GetValue(o, null) ?? "").ToString())).ToArray());
            }
        }

    }
}
